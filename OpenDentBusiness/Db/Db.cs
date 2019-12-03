using CodeBase;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace OpenDentBusiness
{
    /// <summary
    /// >Used to send queries. The methods are internal since it is not acceptable for the UI to be sending queries.
    /// </summary>
    public class Db
    {
        /// <summary>
        /// A thread safe and thread specific value containing the last SQL command attempted.
        /// </summary>
        [ThreadStatic]
        static string lastCommand;

        /// <summary>
        /// The last SQL command attempted.
        /// </summary>
        public static string LastCommand
        {
            get
            {
                return lastCommand ?? "[COMMAND NOT SET]";
            }
            private set
            {
                lastCommand = value;
            }
        }

        /// <summary>
        /// This is true if a connection to the database has been established.
        /// </summary>
        public static bool HasDatabaseConnection
        {
            get
            {
                return !string.IsNullOrEmpty(DataConnection.Host) || !string.IsNullOrEmpty(DataConnection.ConnectionString);
            }
        }

        /// <summary>
        /// Checks to see if the user has permission to run a command query if it is a command query.
        /// </summary>
        public static bool IsSqlAllowed(string command)
        {
            bool isCommand;

            try
            {
                isCommand = IsCommandSql(command);
            }
            catch
            {
                throw new ApplicationException("Validation failed. Please remove mid-query comments and try again.");
            }

            if (isCommand)
            {
                if (!Security.IsAuthorized(Permissions.CommandQuery))
                {
                    return false;
                }

                SecurityLog.Write(null, SecurityLogEvents.CommandQuery, "Command query run.");
            }

            return true;
        }

        ///<summary>Returns true if the given SQL script in strSql contains any commands (INSERT, UPDATE, DELETE, etc.). Surround with a try/catch.</summary>
        private static bool IsCommandSql(string strSql)
        {
            string trimmedSql = strSql.Trim();//If a line is completely a comment it may have only a trailing \n to make a subquery on. We need to keep it there.
                                              //splits the string while accounting for quotes and case/if/concat statements.
            string[] arraySqlExpressions = UserQueries.SplitQuery(UserQueries.RemoveSQLComments(trimmedSql).ToUpper(), false, ";").ToArray();
            //Because of the complexities of parsing through MySQL and the fact that we don't want to take the time to create a fully functional parser
            //for our simple query runner we elected to err on the side of caution.  If there are comments in the middle of the query this section of
            //code will fire a UE.  This is due to the fact that without massive work we cannot intelligently discern if a comment is in the middle of
            //a string being used or if it is a legitimate comment.  Since we cannot know this we want to block more often than may be absolutely 
            //necessary to catch people doing anything that could potentially lead to SQL injection attacks.  We thus want to inform the user that simply
            //removing intra-query comments is the necessary fix for their problem.
            for (int i = 0; i < arraySqlExpressions.Length; i++)
            {
                //Clean out any leading comments before we do anything else
                while (arraySqlExpressions[i].Trim().StartsWith("#") || arraySqlExpressions[i].Trim().StartsWith("--") || arraySqlExpressions[i].Trim().StartsWith("/*"))
                {
                    if (arraySqlExpressions[i].Trim().StartsWith("/*"))
                    {
                        arraySqlExpressions[i] = arraySqlExpressions[i].Remove(0, arraySqlExpressions[i].IndexOf("*/") + 3).Trim();
                    }
                    else
                    {//Comment starting with # or starting with --
                        int endIndex = arraySqlExpressions[i].IndexOf("\n");
                        if (endIndex != -1)
                        {//This is so it doesn't break if the last line of a command is a comment
                            arraySqlExpressions[i] = arraySqlExpressions[i].Remove(0, arraySqlExpressions[i].IndexOf("\n")).Trim();
                        }
                        else
                        {
                            arraySqlExpressions[i] = arraySqlExpressions[i].Remove(0, arraySqlExpressions[i].Length).Trim();
                        }
                    }
                }
                if (String.IsNullOrWhiteSpace(arraySqlExpressions[i]))
                {
                    continue;//Ignore empty SQL statements.
                }
                if (arraySqlExpressions[i].Trim().StartsWith("SELECT"))
                {//We don't care about select queries
                    continue;
                }
                else if (arraySqlExpressions[i].Trim().StartsWith("SET"))
                {
                    //We need to allow SET statements because we use them to set variables in our query examples.
                    continue;
                }
                else if (arraySqlExpressions[i].Trim().StartsWith("UPDATE"))
                {//These next we allow if they are on temp tables
                    if (HasNonTempTable("UPDATE", arraySqlExpressions[i]))
                    {
                        return true;
                    }
                }
                else if (arraySqlExpressions[i].Trim().StartsWith("ALTER"))
                {
                    if (HasNonTempTable("TABLE", arraySqlExpressions[i]))
                    {
                        return true;
                    }
                }
                else if (arraySqlExpressions[i].Trim().StartsWith("CREATE"))
                {//CREATE INDEX or CREATE TABLE or CREATE TEMPORARY TABLE
                    int a = arraySqlExpressions[i].Trim().IndexOf("INDEX");
                    int b = arraySqlExpressions[i].Trim().IndexOf("TABLE");
                    string keyword = "";
                    if (a == -1 && b == -1)
                    {
                        //Invalid command.  Ignore.
                    }
                    else if (a != -1 && b == -1)
                    {
                        keyword = "INDEX";
                    }
                    else if (a == -1 && b != -1)
                    {
                        keyword = "TABLE";
                    }
                    else if (a != -1 && b != -1)
                    {
                        keyword = arraySqlExpressions[i].Trim().Substring(Math.Min(a, b), 5);//Get the keyword that is closest to the front of the string.
                    }
                    if (keyword != "" && HasNonTempTable(keyword, arraySqlExpressions[i]))
                    {
                        return true;
                    }
                }
                else if (arraySqlExpressions[i].Trim().StartsWith("DROP"))
                { //DROP [TEMPORARY] TABLE [IF EXISTS]
                    int a = arraySqlExpressions[i].Trim().IndexOf("TABLE");
                    //We require exactly one space between these two keywords, because there are all sorts of technically valid ways to write the IF EXISTS which would create a lot of work for us.
                    //Examples "DROP TABLE x IF    EXISTS ...", "DROP TABLE x IF /*comment IF EXISTS*/  EXISTS ...", "DROP TABLE ifexists IF EXISTS /*IF EXISTS*/"
                    int b = arraySqlExpressions[i].Trim().IndexOf("IF EXISTS");
                    string keyword = "";
                    if (a == -1 && b == -1)
                    {
                        //Invalid command.  Ignore.
                    }
                    else if (b == -1)
                    {
                        keyword = "TABLE";//Must have TABLE if it's not invalid
                    }
                    else
                    {
                        keyword = "IF EXISTS";//It has the IF EXISTS statement
                    }
                    if (keyword != "" && HasNonTempTable(keyword, arraySqlExpressions[i]))
                    {
                        return true;
                    }
                }
                else if (arraySqlExpressions[i].Trim().StartsWith("RENAME"))
                {
                    if (HasNonTempTable("TABLE", arraySqlExpressions[i]))
                    {
                        return true;
                    }
                }
                else if (arraySqlExpressions[i].Trim().StartsWith("TRUNCATE"))
                {
                    if (HasNonTempTable("TABLE", arraySqlExpressions[i]))
                    {
                        return true;
                    }
                }
                else if (arraySqlExpressions[i].Trim().StartsWith("DELETE"))
                {
                    if (HasNonTempTable("DELETE", arraySqlExpressions[i]))
                    {
                        return true;
                    }
                }
                else if (arraySqlExpressions[i].Trim().StartsWith("INSERT"))
                {
                    if (HasNonTempTable("INTO", arraySqlExpressions[i]))
                    {
                        return true;
                    }
                }
                else
                {//All the rest of the commands that we won't allow, even with temp tables, also includes if there are any additional comments embedded.
                    return true;
                }
            }
            return false;
        }

        ///<summary>The keywords must be listed in the order they are required to appear within the query.</summary>
        private static bool HasNonTempTable(string keyword, string command)
        {
            int keywordEndIndex = command.IndexOf(keyword) + keyword.Length;
            command = command.Remove(0, keywordEndIndex).Trim();//Everything left will be the table/s or nested queries.
                                                                //Match one or more table names with optional alias for each table name, separated by commas.
                                                                //A word in this contenxt is any string of non-space characters which also does not include ',' or '(' or ')'.
            Match m = Regex.Match(command, @"^([^\s,\(\)]+(\s+[^\s,\(\)]+)?(\s*,\s*[^\s,\(\)]+(\s+[^\s,\(\)]+)?)*)");
            string[] arrayTableNames = m.Result("$1").Split(',');
            for (int i = 0; i < arrayTableNames.Length; i++)
            {//Adding matched strings to list
                string tableName = arrayTableNames[i].Trim().Split(' ')[0];
                if (!tableName.StartsWith("TEMP") && !tableName.StartsWith("TMP"))
                {//A table name that doesn't start with temp nor tmp (non temp table).
                    return true;
                }
            }
            return false;
        }

        [Obsolete]
        internal static DataTable GetTable(string command)
        {
            LastCommand = command;

            return DataConnection.ExecuteDataTable(command);

        }

        ///<summary>Performs PIn.Long on first column of table returned. Surround with try/catch. Returns empty list if nothing found.</summary>
        internal static List<long> GetListLong(string command)
        {
            List<long> retVal = new List<long>();
            DataTable Table = GetTable(command);
            for (int i = 0; i < Table.Rows.Count; i++)
            {
                retVal.Add(PIn.Long(Table.Rows[i][0].ToString()));
            }
            return retVal;
        }

        /// <summary>
        /// Performs PIn.String on first column of table returned. Returns empty list if nothing found.
        /// </summary>
        internal static List<string> GetListString(string command)
        {
            List<string> retVal = new List<string>();
            DataTable Table = GetTable(command);
            for (int i = 0; i < Table.Rows.Count; i++)
            {
                retVal.Add(PIn.String(Table.Rows[i][0].ToString()));
            }
            return retVal;
        }

        [Obsolete("Use proper NpgsqlParameters...")]
        internal static long NonQ(string command, bool p1 = false, string p2 = "", string p3 = "", params OdSqlParameter[] parameters) => NonQ(command, parameters);

        [Obsolete("Use proper NpgsqlParameters...")]
        internal static long NonQ(string command, params OdSqlParameter[] parameters)
        {
            LastCommand = command;

            List<MySqlParameter> pgParameters = new List<MySqlParameter>();
            foreach (var parameter in parameters)
            {
                pgParameters.Add(
                    new MySqlParameter(
                        parameter.ParameterName,
                        parameter.Value));
            }

            return DataConnection.ExecuteNonQuery(command, pgParameters.ToArray());

        }

        /// <summary>
        /// Use this for count(*) queries.
        /// They are always guaranteed to return one and only one value.
        /// Not any faster, just handier.
        /// 
        /// Can also be used when retrieving prefs manually, since they will also return exactly one value.
        /// </summary>
        internal static string GetCount(string command)
        {
            LastCommand = command;

            return DataConnection.ExecuteDataTable(command).Rows[0][0].ToString();
        }

        /// <summary>
        /// Use this only for queries that return one value.
        /// </summary>
        internal static string GetScalar(string command)
        {
            LastCommand = command;

            return DataConnection.ExecuteScalar(command);

        }

        #region old
        ///<summary>Always throws exception.</summary>
        public static DataTable GetTableOld(string command)
        {
            throw new ApplicationException("No queries allowed in the UI layer.");
        }

        ///<summary>Always throws exception.</summary>
        public static int NonQOld(string[] commands)
        {
            throw new ApplicationException("No queries allowed in the UI layer.");
        }

        ///<summary>Always throws exception.</summary>
        public static int NonQOld(string command)
        {
            throw new ApplicationException("No queries allowed in the UI layer.");
        }

        #endregion old
    }
}