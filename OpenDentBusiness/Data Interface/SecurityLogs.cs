using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class SecurityLogs
    {
        ///<summary>The log source of the current application.</summary>
        public static LogSources LogSource = LogSources.None;

        #region Get Methods
        ///<summary>Returns one SecurityLog from the db.  Called from SecurityLogHashs.CreateSecurityLogHash()</summary>
        public static SecurityLog GetOne(long securityLogNum)
        {
            return Crud.SecurityLogCrud.SelectOne(securityLogNum);
        }

        ///<summary>Gets many security logs matching the passed in parameters.///</summary>
        public static List<SecurityLog> GetMany(params SQLWhere[] whereClause)
        {
            List<SQLWhere> listWheres = new List<SQLWhere>();
            foreach (SQLWhere where in whereClause)
            {
                listWheres.Add(where);
            }
            return GetMany(listWheres);
        }

        ///<summary>Gets a list of all securitylogs matching the passed in parameters.</summary>
        public static List<SecurityLog> GetMany(List<SQLWhere> listWheres)
        {
            string command = "SELECT * FROM securitylog ";
            if (listWheres != null && listWheres.Count > 0)
            {
                command += "WHERE " + string.Join(" AND ", listWheres);
            }
            return Crud.SecurityLogCrud.SelectMany(command);
        }

        #endregion

        public static void DeleteWithMaxPriKey(long securityLogMaxPriKey)
        {
            if (securityLogMaxPriKey == 0)
            {
                return;
            }
            string command = "DELETE FROM securitylog WHERE SecurityLogNum <= " + POut.Long(securityLogMaxPriKey);
            Db.NonQ(command);
        }

        ///<summary>Used when viewing securityLog from the security admin window.  PermTypes can be length 0 to get all types.
        ///Throws exceptions.</summary>
        public static SecurityLog[] Refresh(DateTime dateFrom, DateTime dateTo, Permissions permType, long patNum, long userNum,
            DateTime datePreviousFrom, DateTime datePreviousTo, bool includeArchived, int limit = 0)
        {
            string command = "SELECT securitylog.*,LName,FName,Preferred,MiddleI,LogHash FROM securitylog "
                + "LEFT JOIN patient ON patient.PatNum=securitylog.PatNum "
                + "LEFT JOIN securityloghash ON securityloghash.SecurityLogNum=securitylog.SecurityLogNum "
                + "WHERE LogDateTime >= " + POut.Date(dateFrom) + " "
                + "AND LogDateTime <= " + POut.Date(dateTo.AddDays(1)) + " "
                + "AND DateTPrevious >= " + POut.Date(datePreviousFrom) + " "
                + "AND DateTPrevious <= " + POut.Date(datePreviousTo.AddDays(1));
            if (patNum != 0)
            {
                command += " AND securitylog.PatNum IN (" + string.Join(",",
                    PatientLinks.GetPatNumsLinkedToRecursive(patNum, PatientLinkType.Merge).Select(x => POut.Long(x))) + ")";
            }
            if (permType != Permissions.None)
            {
                command += " AND PermType=" + POut.Long((int)permType);
            }
            if (userNum != 0)
            {
                command += " AND UserNum=" + POut.Long(userNum);
            }
            command += " ORDER BY LogDateTime DESC";//Using DESC so that the most recent ones appear in the list
            if (limit > 0)
            {
                command = DbHelper.LimitOrderBy(command, limit);
            }
            DataTable table = Db.GetTable(command);
            List<SecurityLog> listLogs = Crud.SecurityLogCrud.TableToList(table);
            for (int i = 0; i < listLogs.Count; i++)
            {
                if (table.Rows[i]["PatNum"].ToString() == "0")
                {
                    listLogs[i].PatientName = "";
                }
                else
                {
                    listLogs[i].PatientName = table.Rows[i]["PatNum"].ToString() + "-"
                        + Patients.GetNameLF(table.Rows[i]["LName"].ToString()
                        , table.Rows[i]["FName"].ToString()
                        , table.Rows[i]["Preferred"].ToString()
                        , table.Rows[i]["MiddleI"].ToString());
                }
                listLogs[i].LogHash = table.Rows[i]["LogHash"].ToString();
            }
            if (includeArchived)
            {
                //This will purposefully throw exceptions.
                DataTable tableArchive = MiscData.RunFuncOnArchiveDatabase<DataTable>(() =>
                {
                    return Db.GetTable(command);
                });
                List<SecurityLog> listLogsArchive = Crud.SecurityLogCrud.TableToList(tableArchive);
                Dictionary<long, Patient> dictPats = Patients.GetMultPats(listLogsArchive.Select(x => x.PatNum).Distinct().ToList())
                    .ToDictionary(x => x.PatNum);
                for (int i = 0; i < listLogsArchive.Count; i++)
                {
                    Patient pat;
                    if (listLogsArchive[i].PatNum == 0 || !dictPats.TryGetValue(listLogsArchive[i].PatNum, out pat))
                    {
                        listLogsArchive[i].PatientName = "";
                    }
                    else
                    {
                        listLogsArchive[i].PatientName = listLogsArchive[i].PatNum + "-" + pat.GetNameLF();
                    }
                    listLogsArchive[i].LogHash = tableArchive.Rows[i]["LogHash"].ToString();
                }
                listLogs.AddRange(listLogsArchive);//Add archived entries to returned list.
            }
            return listLogs.OrderBy(x => x.LogDateTime).ToArray();
        }

        ///<summary></summary>
        public static long Insert(SecurityLog log)
        {
            return Crud.SecurityLogCrud.Insert(log);
        }

        //there are no methods for deleting or changing log entries because that will never be allowed.

        ///<summary>Used when viewing various audit trails of specific types.  Only implemented Appointments,ProcFeeEdit,InsPlanChangeCarrierName so far. patNum only used for Appointments.  The other two are zero.</summary>
        public static SecurityLog[] Refresh(long patNum, List<Permissions> permTypes, long fKey, bool includeArchived)
        {
            //No need to check RemotingRole; no call to db.
            return Refresh(patNum, permTypes, new List<long>() { fKey }, includeArchived);
        }

        ///<summary>Used when viewing various audit trails of specific types.  This overload will return security logs for multiple objects (or fKeys).
        ///Typically you will only need a specific type audit log for one type.
        ///However, for things like ortho charts, each row (FK) in the database represents just one part of a larger ortho chart "object".
        ///Thus, to get the full experience of a specific type audit trail window, we need to get security logs for multiple objects (FKs) that
        ///comprise the larger object (what the user sees).  Only implemented with ortho chart so far.  FKeys can be null.
        ///Throws exceptions.</summary>
        public static SecurityLog[] Refresh(long patNum, List<Permissions> permTypes, List<long> fKeys, bool includeArchived)
        {
            string types = "";
            for (int i = 0; i < permTypes.Count; i++)
            {
                if (i > 0)
                {
                    types += " OR";
                }
                types += " PermType=" + POut.Long((int)permTypes[i]);
            }
            string command = "SELECT * FROM securitylog "
                + "WHERE (" + types + ") ";
            if (fKeys != null && fKeys.Count > 0)
            {
                command += "AND FKey IN (" + String.Join(",", fKeys) + ") ";
            }
            if (patNum != 0)
            {//appointments
                command += " AND PatNum IN (" + string.Join(",",
                    PatientLinks.GetPatNumsLinkedToRecursive(patNum, PatientLinkType.Merge).Select(x => POut.Long(x))) + ")";
            }
            command += "ORDER BY LogDateTime";
            List<SecurityLog> listLogs = Crud.SecurityLogCrud.SelectMany(command);
            if (includeArchived)
            {
                //This will purposefully throw exceptions.
                listLogs.AddRange(MiscData.RunFuncOnArchiveDatabase<List<SecurityLog>>(() =>
                {
                    return Crud.SecurityLogCrud.SelectMany(command);
                }));
            }
            return listLogs.OrderBy(x => x.LogDateTime).ToArray();
        }

        ///<summary>Gets all security logs for the given foreign keys and permissions.</summary>
        public static List<SecurityLog> GetFromFKeysAndType(List<long> listFKeys, List<Permissions> permTypes)
        {
            if (listFKeys == null || listFKeys.FindAll(x => x != 0).Count == 0)
            {
                return new List<SecurityLog>();
            }
            string command = "SELECT * FROM securitylog WHERE FKey IN(" + string.Join(",", listFKeys.FindAll(x => x != 0)) + ") AND PermType IN" +
                "(" + string.Join(",", permTypes.Select(x => POut.Int((int)x))) + ")";
            return Crud.SecurityLogCrud.SelectMany(command);
        }

        ///<summary>Used to insert a list of security logs.</summary>
        ///<param name="permType">The type of permission to be logged in the security log.</param>
        ///<param name="patNum">The PatNum for the patient associated to the security log. Can be 0.</param>
        ///<param name="listSecurityLogs">A list of the security log text that should be inserted.</param>
        public static void MakeLogEntries(Permissions permType, long patNum, List<string> listSecurityLogs)
        {
            if (listSecurityLogs == null || listSecurityLogs.Count == 0)
            {
                return;
            }
            foreach (string securityLogEntry in listSecurityLogs)
            {
                MakeLogEntry(permType, patNum, securityLogEntry);
            }
        }

        ///<summary>PatNum can be 0.</summary>
        public static void MakeLogEntry(Permissions permType, long patNum, string logText)
        {
            //No need to check RemotingRole; no call to db.
            MakeLogEntry(permType, patNum, logText, 0, LogSource, DateTime.MinValue);
        }

        ///<summary>Used when the security log needs to be identified by a particular source.  PatNum can be 0.</summary>
        public static void MakeLogEntry(Permissions permType, long patNum, string logText, LogSources logSource)
        {
            //No need to check RemotingRole; no call to db.
            MakeLogEntry(permType, patNum, logText, 0, logSource, DateTime.MinValue);
        }

        ///<summary>Takes a foreign key to a table associated with that PermType.  PatNum can be 0.</summary>
        public static void MakeLogEntry(Permissions permType, long patNum, string logText, long fKey, DateTime DateTPrevious)
        {
            //No need to check RemotingRole; no call to db.
            MakeLogEntry(permType, patNum, logText, fKey, LogSource, DateTPrevious);
        }

        ///<summary>Takes a foreign key to a table associated with that PermType.  PatNum can be 0.</summary>
        public static void MakeLogEntry(Permissions permType, long patNum, string logText, long fKey, LogSources logSource, DateTime DateTPrevious)
        {
            MakeLogEntry(permType, patNum, logText, fKey, logSource, 0, 0, DateTPrevious);
        }

        ///<summary>Takes a foreign key to a table associated with that PermType.  PatNum can be 0.</summary>
        public static void MakeLogEntry(Permissions permType, long patNum, string logText, long fKey, LogSources logSource, long defNum, long defNumError,
            DateTime DateTPrevious)
        {
            //No need to check RemotingRole; no call to db.
            SecurityLog securityLog = MakeLogEntryNoInsert(permType, patNum, logText, fKey, logSource, defNum, defNumError, DateTPrevious);
            MakeLogEntry(securityLog);
        }

        ///<summary>Take a SecurityLog object to save to the database. Creates a SecurityLogHash object as well.</summary>
        public static void MakeLogEntry(SecurityLog secLog)
        {
            secLog.SecurityLogNum = SecurityLogs.Insert(secLog);
            SecurityLogHashes.InsertSecurityLogHash(secLog.SecurityLogNum);//uses db date/time
            if (secLog.PermType == Permissions.AppointmentCreate)
            {
                EntryLogs.Insert(new EntryLog(secLog.UserNum, EntryLogFKeyType.Appointment, secLog.FKey, secLog.LogSource));
            }
        }

        ///<summary>Creates security log entries for all that PatNums passed in.</summary>
        public static void MakeLogEntry(Permissions permType, List<long> listPatNums, string logText)
        {
            List<SecurityLog> listSecLogs = new List<SecurityLog>();
            foreach (long patNum in listPatNums)
            {
                SecurityLog secLog = MakeLogEntryNoInsert(permType, patNum, logText, 0, LogSource);
                SecurityLogs.Insert(secLog);
                listSecLogs.Add(secLog);
            }
            List<SecurityLogHash> listHash = new List<SecurityLogHash>();
            List<EntryLog> listEntries = new List<EntryLog>();
            listSecLogs = SecurityLogs.GetMany(SQLWhere.CreateIn(nameof(SecurityLog.SecurityLogNum),
                listSecLogs.Select(x => x.SecurityLogNum).ToList()));
            foreach (SecurityLog log in listSecLogs)
            {
                SecurityLogHash secLogHash = new SecurityLogHash();
                secLogHash.SecurityLogNum = log.SecurityLogNum;
                secLogHash.LogHash = SecurityLogHashes.GetHashString(log);
                listHash.Add(secLogHash);
                if (log.PermType == Permissions.AppointmentCreate)
                {
                    listEntries.Add(new EntryLog(log.UserNum, EntryLogFKeyType.Appointment, log.FKey, log.LogSource));
                }
            }
            EntryLogs.InsertMany(listEntries);
            SecurityLogHashes.InsertMany(listHash);
        }

        ///<summary>Takes a foreign key to a table associated with that PermType.  PatNum can be 0.  Returns the created SecurityLog object.  Does not perform an insert.</summary>
        public static SecurityLog MakeLogEntryNoInsert(Permissions permType, long patNum, string logText, long fKey, LogSources logSource, long defNum = 0,
            long defNumError = 0, DateTime DateTPrevious = default(DateTime))
        {
            //No need to check RemotingRole; no call to db.
            SecurityLog securityLog = new SecurityLog();
            securityLog.PermType = permType;
            securityLog.UserNum = Security.CurUser.UserNum;
            securityLog.LogText = logText;
            securityLog.CompName = Security.CurComputerName;
            securityLog.PatNum = patNum;
            securityLog.FKey = fKey;
            securityLog.LogSource = logSource;
            securityLog.DefNum = defNum;
            securityLog.DefNumError = defNumError;
            securityLog.DateTPrevious = DateTPrevious;
            return securityLog;
        }

        ///<summary>Used when making a security log from a remote server, possibly with multithreaded connections.</summary>
        public static void MakeLogEntryNoCache(Permissions permType, long patnum, string logText)
        {
            MakeLogEntryNoCache(permType, patnum, logText, 0, LogSource);
        }

        ///<summary>Used when making a security log from a remote server, possibly with multithreaded connections.</summary>
        public static void MakeLogEntryNoCache(Permissions permType, long patnum, string logText, long userNum, LogSources source)
        {
            SecurityLog securityLog = new SecurityLog();
            securityLog.PermType = permType;
            securityLog.UserNum = userNum;
            securityLog.LogText = logText;
            securityLog.CompName = Security.CurComputerName;
            securityLog.PatNum = patnum;
            securityLog.FKey = 0;
            securityLog.LogSource = source;
            securityLog.SecurityLogNum = SecurityLogs.InsertNoCache(securityLog);
            SecurityLogHashes.InsertSecurityLogHashNoCache(securityLog.SecurityLogNum);
        }

        ///<summary>Insertion logic that doesn't use the cache. Has special cases for generating random PK's and handling Oracle insertions.</summary>
        public static long InsertNoCache(SecurityLog securityLog)
        {
            return Crud.SecurityLogCrud.InsertNoCache(securityLog);
        }
    }
}