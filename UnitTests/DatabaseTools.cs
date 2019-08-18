using OpenDentBusiness;
using System;
using System.Reflection;

namespace UnitTests
{
    ///<summary>Contains the queries, scripts, and tools to clear the database of data from previous unitTest runs.</summary>
    class DatabaseTools
    {
        public static string FreshFromDump(string serverAddr, string port, string userName, string password, bool isOracle)
        {
            Security.CurUser = Security.CurUser ?? new User();
            if (!isOracle)
            {
                string command = ;
                try
                {
                    DataConnection.ExecuteNonQuery("DROP DATABASE IF EXISTS " + TestBase.UnitTestDbName);
                }
                catch
                {
                    throw new Exception("Database could not be dropped.  Please remove any remaining text files and try again.");
                }

                DataConnection.ExecuteNonQuery("CREATE DATABASE " + TestBase.UnitTestDbName);
                UnitTestsCore.DatabaseTools.SetDbConnection(TestBase.UnitTestDbName, serverAddr, port, userName, password, false);
 
                DataConnection.ExecuteNonQuery(Properties.Resources.dump);
                string toVersion = Assembly.GetAssembly(typeof(OpenDental.PrefL)).GetName().Version.ToString();

                ProcedureCodes.TcodesClear();
                AutoCodes.SetToDefault();
                ProcButtons.SetToDefault();
                ProcedureCodes.ResetApptProcsQuickAdd();
                ProcedureCodes.RefreshCache();
                DataConnection.ExecuteNonQuery("UPDATE userod SET Password='qhd+xdy/iMpe3xcjbBmB6A==' WHERE UserNum=1");
                AddCdcrecCodes();
            }
            else
            {
                //This stopped working. Might look into it later: for now manually create the unittest db

                //Make sure the command CREATE OR REPLACE DIRECTORY dmpdir AS 'c:\oraclexe\app\tmp'; was run
                //and there is an opendental user with matching username/pass 
                //The unittest.dmp was taken from a fresh unittest db created from the code above.  No need to alter it further. 
                //string command=@"impdp opendental/opendental DIRECTORY=dmpdir DUMPFILE=unittest.dmp TABLE_EXISTS_ACTION=replace LOGFILE=impschema.log";
                //ExecuteCommand(command);
            }
            return "Fresh database loaded from sql dump.\r\n";
        }

        /// <summary>
        /// Manually adds the few CDCREC codes necessary for the HL7 unit tests.
        /// </summary>
        static void AddCdcrecCodes()
        {
            if (DataConnection.ExecuteLong("SELECT COUNT(*) FROM cdcrec") == 0)
            {
                Cdcrecs.Insert(new Cdcrec()
                {
                    CdcrecCode = "2106-3",
                    HeirarchicalCode = "R5",
                    Description = "WHITE"
                });

                Cdcrecs.Insert(new Cdcrec()
                {
                    CdcrecCode = "2135-2",
                    HeirarchicalCode = "E1",
                    Description = "HISPANIC OR LATINO"
                });
            }
        }
    }
}