using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using CodeBase;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class Reactivations
    {
        #region Get Methods
        ///<summary></summary>
        public static List<Reactivation> Refresh(long patNum)
        {
            string command = "SELECT * FROM reactivation WHERE PatNum = " + POut.Long(patNum);
            return Crud.ReactivationCrud.SelectMany(command);
        }

        ///<summary>Gets one Reactivation from the db.</summary>
        public static Reactivation GetOne(long reactivationNum)
        {
            return Crud.ReactivationCrud.SelectOne(reactivationNum);
        }

        public static int GetNumReminders(long patNum)
        {
            long commType = Commlogs.GetTypeAuto(CommItemTypeAuto.REACT);
            if (commType == 0)
            {
                return 0;
            }
            string cmd =
                @"SELECT
					COUNT(*) AS NumReminders
					FROM commlog
					WHERE commlog.CommType=" + POut.Long(commType) + " " +
                    "AND commlog.PatNum=" + POut.Long(patNum);
            return PIn.Int(Db.GetScalar(cmd));
        }

        public static DateTime GetDateLastContacted(long patNum)
        {
            long commType = Commlogs.GetTypeAuto(CommItemTypeAuto.REACT);
            if (commType == 0)
            {
                return DateTime.MinValue;
            }
            string cmd =
                @"SELECT
					MAX(commlog.CommDateTime) AS DateLastContacted
					FROM commlog
					WHERE commlog.CommType=" + POut.Long(commType) + " " +
                    "AND commlog.PatNum=" + POut.Long(patNum) + " " +
                    "GROUP BY commlog.PatNum";
            return PIn.DateT(Db.GetScalar(cmd));
        }

        ///<summary>Gets the list of patients that need to be on the reactivation list based on the passed in filters.</summary>
        public static DataTable GetReactivationList(DateTime dateSince, bool groupFamilies, bool showDoNotContact, long provNum, long clinicNum, long siteNum
            , long billingType, ReactivationListSort sortBy, RecallListShowNumberReminders showReactivations)
        {

            //Get information we will need to do the query
            List<long> listReactCommLogTypeDefNums = Defs.GetDefsForCategory(DefCat.CommLogTypes, isShort: true)
                .FindAll(x => CommItemTypeAuto.REACT.GetDescription(useShortVersionIfAvailable: true).Equals(x.ItemValue)).Select(x => x.DefNum).ToList();
            int contactInterval = Preferences.GetInt(PrefName.ReactivationContactInterval);
            //Get the raw set of patients who should be on the reactivation list
            string cmd =
                $@"SELECT 
						pat.PatNum,
						pat.LName,
						pat.FName,
						pat.MiddleI,
						pat.Preferred,
						pat.Guarantor,
						pat.PatStatus,
						pat.Birthdate,
						pat.PriProv,
						COALESCE(billingtype.ItemName,'') AS BillingType,
						pat.ClinicNum,
						pat.SiteNum,
						pat.PreferRecallMethod,
						{(groupFamilies ? "COALESCE(guarantor.Email,pat.Email,'') AS Email," : "pat.Email,")}
						MAX(proc.ProcDate) AS DateLastProc,
						COALESCE(comm.DateLastContacted,'') AS DateLastContacted,
						COALESCE(comm.ContactedCount,0) AS ContactedCount,
						COALESCE(react.ReactivationNum,0) AS ReactivationNum,
						COALESCE(react.ReactivationStatus,0) AS ReactivationStatus,
						COALESCE(react.DoNotContact,0) as DoNotContact,
						react.ReactivationNote,
						guarantor.PatNum as GuarNum,
						guarantor.LName as GuarLName,
						guarantor.FName as GuarFName
					FROM patient pat
					INNER JOIN procedurelog proc ON pat.PatNum=proc.PatNum AND proc.ProcStatus={POut.Int((int)ProcStat.C)}
					LEFT JOIN appointment appt ON pat.PatNum=appt.PatNum AND appt.AptDateTime >= CURDATE() 
					LEFT JOIN (
						SELECT
							commlog.PatNum,
							MAX(commlog.CommDateTime) AS DateLastContacted,
							COUNT(*) AS ContactedCount
							FROM commlog
							WHERE commlog.CommType IN ({string.Join(",", listReactCommLogTypeDefNums)}) 
							GROUP BY commlog.PatNum
					) comm ON pat.PatNum=comm.PatNum
					LEFT JOIN reactivation react ON pat.PatNum=react.PatNum
					LEFT JOIN definition billingtype ON pat.BillingType=billingtype.DefNum
					INNER JOIN patient guarantor ON pat.Guarantor=guarantor.PatNum
					WHERE pat.PatStatus IN ({POut.Int((int)PatientStatus.Patient)}
						,{POut.Int((int)PatientStatus.Inactive)}
						,{POut.Int((int)PatientStatus.Prospective)}) ";
            cmd += provNum > 0 ? " AND pat.PriProv=" + POut.Long(provNum) : "";
            cmd += clinicNum > -1 ? " AND pat.ClinicNum=" + POut.Long(clinicNum) : "";//might still want to get the 0 clinic pats
            cmd += siteNum > 0 ? " AND pat.SiteNum=" + POut.Long(siteNum) : "";
            cmd += billingType > 0 ? " AND pat.BillingType=" + POut.Long(billingType) : "";
            cmd += showDoNotContact ? "" : " AND (react.DoNotContact IS NULL OR react.DoNotContact=0)";
            cmd += contactInterval > -1 ? " AND (comm.DateLastContacted IS NULL OR comm.DateLastContacted <= " + POut.DateT(DateTime.Today.AddDays(-contactInterval)) + ") " : "";
            //set number of contact attempts
            int maxReminds = Preferences.GetInt(PrefName.ReactivationCountContactMax);
            if (showReactivations == RecallListShowNumberReminders.SixPlus)
            {
                cmd += " AND ContactedCount>=6 "; //don't need to look at pref this only shows in UI if the prefvalue allows it
            }
            else if (showReactivations == RecallListShowNumberReminders.Zero)
            {
                cmd += " AND (comm.ContactedCount=0 OR comm.ContactedCount IS NULL) ";
            }
            else if (showReactivations != RecallListShowNumberReminders.All)
            {
                int filter = (int)showReactivations - 1;
                //if the contactmax pref is not -1 or 0, and the contactmax is smaller than the requested filter, replace the filter with the contactmax
                cmd += " AND comm.ContactedCount=" + POut.Int((maxReminds > 0 && maxReminds < filter) ? maxReminds : filter) + " ";
            }
            else if (showReactivations == RecallListShowNumberReminders.All)
            { //get all but filter on the contactmax
                cmd += " AND (comm.ContactedCount < " + POut.Int(maxReminds) + " OR comm.ContactedCount IS NULL) ";
            }
            cmd += $@" GROUP BY pat.PatNum 
							HAVING MAX(proc.ProcDate) < {POut.Date(dateSince)}
							AND MIN(appt.AptDateTime) IS NULL ";
            //set the sort by
            switch (sortBy)
            {
                case ReactivationListSort.Alphabetical:
                    cmd += " ORDER BY " + (groupFamilies ? "guarantor.LName,guarantor.FName,pat.FName" : "pat.LName,pat.FName");
                    break;
                case ReactivationListSort.BillingType:
                    cmd += " ORDER BY billingtype.ItemName,DateLastContacted" + (groupFamilies ? ",guarantor.LName,guarantor.FName" : "");
                    break;
                case ReactivationListSort.LastContacted:
                    cmd += " ORDER BY IF(comm.DateLastContacted='' OR comm.DateLastContacted IS NULL,1,0),comm.DateLastContacted" + (groupFamilies ? ",guarantor.LName,guarantor.FName" : "");
                    break;
            }
            DataTable dtReturn = Db.GetTable(cmd);
            return dtReturn;
        }

        ///<summary>Follows the format of the Recall addrTable, used in the RecallList to duplicate functionality for mailing/emailing patients.</summary>
        public static DataTable GetAddrTable(List<Patient> listPats, List<Patient> listGuars, bool groupFamilies, ReactivationListSort sortBy)
        {
            //Get the addresses for the patients
            DataTable table = Recalls.GetAddrTableStructure();
            foreach (Patient patCur in listPats)
            {
                Patient guarCur = listGuars.First(x => x.PatNum == patCur.Guarantor);
                List<string> listPatNames = Patients.GetFamily(patCur.PatNum).ListPats.Select(pat => pat.FName).ToList();
                DataRow row = table.NewRow();
                row["address"] = patCur.Address + (!string.IsNullOrWhiteSpace(patCur.Address2) ? Environment.NewLine + patCur.Address2 : "");
                row["City"] = patCur.City;
                row["clinicNum"] = groupFamilies ? guarCur.ClinicNum : patCur.ClinicNum;
                row["dateDue"] = DateTime.MinValue;//This isn't used for reactivations, but it's here keep the table the same as recall addrTable
                row["email"] = groupFamilies ? guarCur.Email : patCur.Email;
                row["emailPatNum"] = groupFamilies ? guarCur.PatNum : patCur.PatNum;
                row["famList"] = listPatNames.Count > 1 ? string.Join(",", listPatNames) : "";
                row["guarLName"] = groupFamilies ? guarCur.LName : patCur.LName;
                row["numberOfReminders"] = Reactivations.GetNumReminders(patCur.PatNum);
                row["patientNameF"] = patCur.GetNameFirstOrPreferred();
                row["patientNameFL"] = patCur.GetNameFLnoPref();
                row["patNums"] = patCur.PatNum;
                row["State"] = patCur.State;
                row["Zip"] = patCur.Zip;
                table.Rows.Add(row);
            }
            return table;
        }

        #endregion Get Methods
        #region Modification Methods
        #region Insert
        ///<summary></summary>
        public static long Insert(Reactivation reactivation)
        {
            return Crud.ReactivationCrud.Insert(reactivation);
        }
        #endregion Insert
        #region Update
        ///<summary></summary>
        public static void Update(Reactivation reactivation)
        {
            Crud.ReactivationCrud.Update(reactivation);
        }

        public static void UpdateStatus(long reactivationNum, long statusDefNum)
        {
            string cmd = "UPDATE reactivation SET ReactivationStatus=" + POut.Long(statusDefNum) + " WHERE ReactivationNum=" + POut.Long(reactivationNum);
            Db.NonQ(cmd);
        }

        #endregion Update
        #region Delete
        ///<summary></summary>
        public static void Delete(long reactivationNum)
        {
            Crud.ReactivationCrud.Delete(reactivationNum);
        }
        #endregion Delete
        #endregion Modification Methods

    }

    ///<summary></summary>
    public enum ReactivationListSort
    {
        ///<summary></summary>
        LastContacted,
        ///<summary></summary>
        BillingType,
        ///<summary></summary>
        Alphabetical,
    }
}