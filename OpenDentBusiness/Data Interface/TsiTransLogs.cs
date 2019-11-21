using CodeBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OpenDentBusiness
{
    public class TsiTransLogs
    {
        #region Get Methods

        ///<summary>Returns all tsitranslogs for the patients in listPatNums.  Returns empty list if listPatNums is empty or null.</summary>
        public static List<TsiTransLog> SelectMany(List<long> listPatNums)
        {
            if (listPatNums == null || listPatNums.Count < 1)
            {
                return new List<TsiTransLog>();
            }
            string command = "SELECT * FROM tsitranslog "
                + "WHERE PatNum IN (" + string.Join(",", listPatNums.Select(x => POut.Long(x))) + ")";
            return Crud.TsiTransLogCrud.SelectMany(command);
        }

        ///<summary>Returns all tsitranslogs for all patients.  Used in FormTsiHistory only.</summary>
        public static List<TsiTransLog> GetAll()
        {
            string command = "SELECT * FROM tsitranslog ORDER BY TransDateTime DESC";
            return Crud.TsiTransLogCrud.SelectMany(command);
        }

        ///<summary>Returns a list of PatNums for guars who have a TsiTransLog with type SS (suspend) less than 50 days ago who don't have a TsiTransLog
        ///with type CN (cancel), PF (paid in full), PT (paid in full, thank you), or PL (placement) with a more recent date, since this would change the
        ///account status from suspended to either closed/canceled or if the more recent message had type PL (placement) back to active.</summary>
        public static List<long> GetSuspendedGuarNums()
        {
            int[] arrayStatusTransTypes = new[] { (int)TsiTransType.SS, (int)TsiTransType.CN, (int)TsiTransType.RI, (int)TsiTransType.PF, (int)TsiTransType.PT, (int)TsiTransType.PL };
            string command = "SELECT DISTINCT tsitranslog.PatNum "
                + "FROM tsitranslog "
                + "INNER JOIN ("
                    + "SELECT PatNum,MAX(TransDateTime) transDateTime "
                    + "FROM tsitranslog "
                    + "WHERE TransType IN(" + string.Join(",", arrayStatusTransTypes) + ") "
                    + "AND TransDateTime>" + POut.DateT(DateTime.Now.AddDays(-50)) + " "
                    + "GROUP BY PatNum"
                + ") mostRecentTrans ON tsitranslog.PatNum=mostRecentTrans.PatNum "
                    + "AND tsitranslog.TransDateTime=mostRecentTrans.transDateTime "
                + "WHERE tsitranslog.TransType=" + (int)TsiTransType.SS;
            return Db.GetListLong(command);
        }

        public static bool IsGuarSuspended(long guarNum)
        {
            int[] arrayStatusTransTypes = new[] { (int)TsiTransType.SS, (int)TsiTransType.CN, (int)TsiTransType.RI, (int)TsiTransType.PF, (int)TsiTransType.PT, (int)TsiTransType.PL };
            string command = "SELECT (CASE WHEN tsitranslog.TransType=" + (int)TsiTransType.SS + " THEN 1 ELSE 0 END) isGuarSuspended "
                + "FROM tsitranslog "
                + "INNER JOIN ("
                    + "SELECT PatNum,MAX(TransDateTime) transDateTime "
                    + "FROM tsitranslog "
                    + "WHERE PatNum=" + POut.Long(guarNum) + " "
                    + "AND TransType IN(" + string.Join(",", arrayStatusTransTypes) + ") "
                    + "AND TransDateTime>" + POut.DateT(DateTime.Now.AddDays(-50)) + " "
                    + "GROUP BY PatNum"
                + ") mostRecentLog ON tsitranslog.PatNum=mostRecentLog.PatNum AND tsitranslog.TransDateTime=mostRecentLog.transDateTime";
            return PIn.Bool(Db.GetScalar(command));
        }

        #endregion Get Methods
        #region Modification Methods
        #region Insert

        public static long Insert(TsiTransLog tsiTransLog)
        {
            return Crud.TsiTransLogCrud.Insert(tsiTransLog);
        }

        public static void InsertMany(List<TsiTransLog> listTsiTransLogs)
        {
            Crud.TsiTransLogCrud.InsertMany(listTsiTransLogs);
        }

        private static void InsertTsiLogsForAdjustment(long patGuar, long adjNum, double adjAmt, string msgText)
        {
            //insert tsitranslog for this transaction so the ODService won't send it to Transworld.  _isTsiAdj means Transworld received a payment on
            //behalf of this guar and took a percentage and send the rest to the office for the account.  This will result in a payment being entered
            //into the account, having been received from Transworld, and an adjustment to account for Transorld's cut.
            PatAging patAgingCur = Patients.GetAgingListFromGuarNums(new List<long>() { patGuar }).FirstOrDefault();//should only ever be 1
            double logAmt = patAgingCur.ListTsiLogs.FindAll(x => x.FKeyType == TsiFKeyType.Adjustment && x.FKey == adjNum).Sum(x => x.TransAmt);
            if (!adjAmt.IsEqual(logAmt))
            {
                TsiTransLog logCur = new TsiTransLog()
                {
                    PatNum = patAgingCur.PatNum,
                    UserNum = Security.CurrentUser.Id,
                    TransType = TsiTransType.None,
                    //TransDateTime=DateTime.Now,//set on insert, not editable by user
                    //DemandType=TsiDemandType.Accelerator,//only valid for placement msgs
                    //ServiceCode=TsiServiceCode.Diplomatic,//only valid for placement msgs
                    ClientId = patAgingCur.ListTsiLogs.FirstOrDefault()?.ClientId ?? "",//can be blank, not used since this isn't really sent to Transworld
                    TransAmt = adjAmt - logAmt,
                    AccountBalance = patAgingCur.AmountDue + adjAmt - logAmt,
                    FKeyType = TsiFKeyType.Adjustment,
                    FKey = adjNum,
                    RawMsgText = msgText,
                    //TransJson=""//only valid for placement msgs
                    ClinicNum = (Preferences.HasClinicsEnabled ? patAgingCur.ClinicNum : 0)
                };
                InsertMany(new List<TsiTransLog>() { logCur });
            }
        }

        /// <summary>Checks adjustments  </summary>
        public static void CheckAndInsertLogsIfAdjTypeExcluded(Adjustment adj, long patGuar, long patClinic, bool isFromTsi = false)
        {
            if (!(IsTransworldEnabled(patClinic) && Patients.IsGuarCollections(patGuar)))
            {
                return;
            }
            string msgText = "Adjustment type is set to excluded type from transworld program properties.";
            if (isFromTsi)
            {
                msgText = "This was not a message sent to Transworld.  This adjustment was entered due to a payment received from Transworld.";
                InsertTsiLogsForAdjustment(patGuar, adj.Id, adj.AdjAmt, msgText);
                return;
            }
            Program transworldProg = Programs.GetCur(ProgramName.Transworld);
            List<ProgramPreference> listProperties = ProgramProperties.GetForProgram(transworldProg.Id);
            string posType = listProperties.FirstOrDefault(x => x.Key == "SyncExcludePosAdjType")?.Value ?? "";
            string negType = listProperties.FirstOrDefault(x => x.Key == "SyncExcludeNegAdjType")?.Value ?? "";
            if (adj.AdjType.In(PIn.Long(posType), PIn.Long(negType)))
            {
                InsertTsiLogsForAdjustment(patGuar, adj.Id, adj.AdjAmt, msgText);
            }
        }

        #endregion Insert
        #region Update

        ///<summary></summary>
        public static void Update(TsiTransLog tsiTransLog, TsiTransLog tsiTransLogOld)
        {
            Crud.TsiTransLogCrud.Update(tsiTransLog, tsiTransLogOld);
        }

        #endregion Update
        #region Delete

        #endregion Delete
        #endregion Modification Methods
        #region Misc Methods

        public static bool ValidateClinicSftpDetails(long programId, bool doTestConnection = true)
        {
            var sftpAddress = ProgramPreference.GetString(programId, "SftpServerAddress");
            var sftpPort = (int)ProgramPreference.GetLong(programId, "SftpServerPort");
            var sftpUsername = ProgramPreference.GetString(programId, "SftpUsername");
            var sftpPassword = ProgramPreference.GetString(programId, "SftpPassword");

            if (sftpPort == 0 || sftpPort < ushort.MinValue || sftpPort > ushort.MaxValue)
                sftpPort = 22;

            if (string.IsNullOrWhiteSpace(sftpAddress) || 
                string.IsNullOrWhiteSpace(sftpUsername) || 
                string.IsNullOrWhiteSpace(sftpPassword))
            {
                return false;
            }

            if (doTestConnection)
            {
                // TODO: Fix...
                //return Sftp.IsConnectionValid(sftpAddress, userName, userPassword, sftpPort);
            }

            return true;
        }

        public static bool IsTransworldEnabled(long clinicId)
        {
            //var program = Programs.GetCur(ProgramName.Transworld);
            //if (program == null || !program.Enabled)
            //{
            //    return false;
            //}

            ////var programProperties = ProgramPreference.GetByProgram(program.Id);
            ////if (programProperties.Count == 0)
            ////{
            ////    return false;
            ////}

            ////var programPropertiesPerClinic = programProperties.GroupBy(x => x.ClinicId).ToDictionary(x => x.Key, x => x.ToList());
            ////if (programPropertiesPerClinic.Count == 0)
            ////{
            ////    return false;
            ////}

            //var disabledClinicIds = new List<long>();

            //if (Preferences.HasClinicsEnabled)
            //{
            //    var clinics = Clinics.GetDeepCopy();

            //    disabledClinicIds.AddRange(
            //        programPropertiesPerClinic
            //            .Where(x => x.Key.HasValue && !ValidateClinicSftpDetails(new ProgramPropertyCollection(x.Value), false))
            //            .Select(x => x.Key.Value));

            //    disabledClinicIds.AddRange(
            //        clinics
            //            .FindAll(clinic => clinic.IsHidden || (disabledClinicIds.Contains(0) && !programPropertiesPerClinic.ContainsKey(clinic.ClinicNum)))
            //            .Select(clinic => clinic.ClinicNum)
            //    );
            //}
            //else
            //{
            //    if (!TsiTransLogs.ValidateClinicSftpDetails(new ProgramPropertyCollection(programPropertiesPerClinic[0]), false))
            //    {
            //        disabledClinicIds.Add(0);
            //    }
            //}

            //return !disabledClinicIds.Contains(clinicId);
            return false;
        }

        ///<summary>Sends an SFTP message to TSI to suspend the account for the guarantor passed in.  Returns empty string if successful.
        ///Returns a translated error message that should be displayed to the user if anything goes wrong.</summary>
        public static string SuspendGuar(Patient guar)
        {
            //PatAging patAging = Patients.GetAgingListFromGuarNums(new List<long>() { guar.PatNum }).FirstOrDefault();
            //if (patAging == null)
            //{//this would only happen if the patient was not in the db??, just in case
            //    return Lans.g("TsiTransLogs", "An error occurred when trying to send a suspend message to TSI.");
            //}
            //long clinicNum = (Preferences.HasClinicsEnabled ? guar.ClinicNum : 0);
            //Program prog = Programs.GetCur(ProgramName.Transworld);
            //if (prog == null)
            //{//shouldn't be possible, the program link should always exist, just in case
            //    return Lans.g("TsiTransLogs", "The Transworld program link does not exist.  Contact support.");
            //}
            //Dictionary<long, List<ProgramProperty>> dictAllProps = ProgramProperties.GetForProgram(prog.ProgramNum)
            //    .GroupBy(x => x.ClinicId)
            //    .ToDictionary(x => x.Key, x => x.ToList());



            //if (dictAllProps.Count == 0)
            //{//shouldn't be possible, there should always be a set of props for ClinicNum 0 even if disabled, just in case
            //    return Lans.g("TsiTransLogs", "The Transworld program link is not setup properly.");
            //}
            //if (Preferences.HasClinicsEnabled && !dictAllProps.ContainsKey(clinicNum) && dictAllProps.ContainsKey(0))
            //{
            //    clinicNum = 0;
            //}
            //string clinicDesc = clinicNum == 0 ? "Headquarters" : Clinics.GetDesc(clinicNum);
            //if (!dictAllProps.ContainsKey(clinicNum)
            //    || !ValidateClinicSftpDetails(dictAllProps[clinicNum], true)) //the props should be valid, but this will test the connection using the props
            //{
            //    return Lans.g("TsiTransLogs", "The Transworld program link is not enabled") + " "
            //        + (Preferences.HasClinicsEnabled ? (Lans.g("TsiTransLogs", "for the guarantor's clinic") + ", " + clinicDesc + ", ") : "")
            //        + Lans.g("TsiTransLogs", "or is not setup properly.");
            //}
            //List<ProgramProperty> listProps = dictAllProps[clinicNum];
            //long newBillType = Preference.GetLong(PreferenceName.TransworldPaidInFullBillingType);
            //if (newBillType == 0 || Defs.GetDef(DefinitionCategory.BillingTypes, newBillType) == null)
            //{
            //    return Lans.g("TsiTransLogs", "The default paid in full billing type is not set.  An automated suspend message cannot be sent until the "
            //        + "default paid in full billing type is set in the Transworld program link")
            //        + (Preferences.HasClinicsEnabled ? (" " + Lans.g("TsiTransLogs", "for the guarantor's clinic") + ", " + clinicDesc) : "") + ".";
            //}
            //string clientId = "";
            //if (patAging.ListTsiLogs.Count > 0)
            //{
            //    clientId = patAging.ListTsiLogs[0].ClientId;
            //}
            //if (string.IsNullOrEmpty(clientId))
            //{
            //    clientId = listProps.Find(x => x.Key == "ClientIdAccelerator")?.Value;
            //}
            //if (string.IsNullOrEmpty(clientId))
            //{
            //    clientId = listProps.Find(x => x.Key == "ClientIdCollection")?.Value;
            //}
            //if (string.IsNullOrEmpty(clientId))
            //{
            //    return Lans.g("TsiTransLogs", "There is no client ID in the Transworld program link")
            //        + (Preferences.HasClinicsEnabled ? (" " + Lans.g("TsiTransLogs", "for the guarantor's clinic") + ", " + clinicDesc) : "") + ".";
            //}
            //string sftpAddress = listProps.Find(x => x.Key == "SftpServerAddress")?.Value ?? "";
            //int sftpPort;
            //if (!int.TryParse(listProps.Find(x => x.Key == "SftpServerPort")?.Value ?? "", out sftpPort))
            //{
            //    sftpPort = 22;//default to port 22
            //}
            //string userName = listProps.Find(x => x.Key == "SftpUsername")?.Value ?? "";
            //string userPassword = listProps.Find(x => x.Key == "SftpPassword")?.Value ?? "";
            //if (new[] { sftpAddress, userName, userPassword }.Any(x => string.IsNullOrEmpty(x)))
            //{
            //    return Lans.g("TsiTransLogs", "The SFTP address, username, or password for the Transworld program link") + " "
            //        + (Preferences.HasClinicsEnabled ? (Lans.g("TsiTransLogs", "for the guarantor's clinic") + ", " + clinicDesc + ", ") : "") + Lans.g("TsiTransLogs", "is blank.");
            //}
            //string msg = TsiMsgConstructor.GenerateUpdate(patAging.PatNum, clientId, TsiTransType.SS, 0.00, patAging.AmountDue);
            //try
            //{
            //    byte[] fileContents = Encoding.ASCII.GetBytes(TsiMsgConstructor.GetUpdateFileHeader() + "\r\n" + msg);
            //    //TaskStateUpload state = new Sftp.Upload(sftpAddress, userName, userPassword, sftpPort)
            //    //{
            //    //    Folder = "/xfer/incoming",
            //    //    FileName = "TsiUpdates_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt",
            //    //    FileContent = fileContents,
            //    //    HasExceptions = true
            //    //};
            //    //state.Execute(false);
            //}
            //catch (Exception ex)
            //{
            //    return Lans.g("TsiTransLogs", "There was an error sending the update message to Transworld")
            //        + (Preferences.HasClinicsEnabled ? (" " + Lans.g("TsiTransLogs", "using the program properties for the guarantor's clinic") + ", " + clinicDesc) : "") + ".\r\n"
            //        + ex.Message;
            //}
            ////Upload was successful
            //TsiTransLog log = new TsiTransLog()
            //{
            //    PatNum = patAging.PatNum,
            //    UserNum = Security.CurrentUser.Id,
            //    TransType = TsiTransType.SS,
            //    //TransDateTime=DateTime.Now,//set on insert, not editable by user
            //    //DemandType=TsiDemandType.Accelerator,//only valid for placement msgs
            //    //ServiceCode=TsiServiceCode.Diplomatic,//only valid for placement msgs
            //    ClientId = clientId,
            //    TransAmt = 0.00,
            //    AccountBalance = patAging.AmountDue,
            //    FKeyType = TsiFKeyType.None,//only used for account trans updates
            //    FKey = 0,//only used for account trans updates
            //    RawMsgText = msg,
            //    ClinicNum = clinicNum
            //    //,TransJson=""//only valid for placement msgs
            //};
            //TsiTransLogs.Insert(log);
            ////update family billing type to the paid in full billing type pref
            //Patients.UpdateFamilyBillingType(newBillType, patAging.PatNum);
            //return "";

            return default;
        }

        #endregion Misc Methods

    }
}