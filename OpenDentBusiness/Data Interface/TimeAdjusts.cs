using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class TimeAdjusts
    {
        #region Get Methods
        ///<summary>Attempts to get one TimeAdjust based on a time.  Returns null if not found. </summary>
        public static TimeAdjust GetPayPeriodNote(long empNum, DateTime startDate)
        {
            string command = "SELECT * FROM timeadjust WHERE EmployeeNum=" + POut.Long(empNum) + " AND TimeEntry=" + POut.DateT(startDate) + " AND IsAuto=0";
            return Crud.TimeAdjustCrud.SelectOne(command);
        }

        ///<summary>Gets a list of payperiod notes.  Start Date should be the start date of the pay period trying to get notes for.</summary>
        public static List<TimeAdjust> GetNotesForPayPeriod(DateTime startDate)
        {
            string command = "SELECT * FROM timeadjust WHERE TimeEntry=" + POut.DateT(startDate) + " AND isAuto=0";
            return Crud.TimeAdjustCrud.SelectMany(command);
        }
        #endregion

        #region Modification Methods

        #region Insert
        #endregion

        #region Update
        #endregion

        #region Delete
        #endregion

        #endregion

        #region Misc Methods
        #endregion


        ///<summary></summary>
        public static List<TimeAdjust> Refresh(long empNum, DateTime fromDate, DateTime toDate)
        {
            string command =
                "SELECT * FROM timeadjust WHERE "
                + "EmployeeNum = " + POut.Long(empNum) + " "
                + "AND " + DbHelper.DtimeToDate("TimeEntry") + " >= " + POut.Date(fromDate) + " "
                + "AND " + DbHelper.DtimeToDate("TimeEntry") + " <= " + POut.Date(toDate) + " "
                + "ORDER BY TimeEntry";
            return Crud.TimeAdjustCrud.SelectMany(command);
        }

        ///<summary>Validates and throws exceptions. Gets all time adjusts between date range and time adjusts made during the current work week. </summary>
        public static List<TimeAdjust> GetValidList(long empNum, DateTime fromDate, DateTime toDate)
        {
            List<TimeAdjust> retVal = new List<TimeAdjust>();
            string command =
                "SELECT * FROM timeadjust WHERE "
                + "EmployeeNum = " + POut.Long(empNum) + " "
                + "AND " + DbHelper.DtimeToDate("TimeEntry") + " >= " + POut.Date(fromDate) + " "
                + "AND " + DbHelper.DtimeToDate("TimeEntry") + " <= " + POut.Date(toDate) + " "
                + "ORDER BY TimeEntry";
            retVal = Crud.TimeAdjustCrud.SelectMany(command);
            //Validate---------------------------------------------------------------------------------------------------------------
            //none necessary at this time.
            return retVal;
        }

        ///<summary>Validates and throws exceptions.  Deletes automatic adjustments that fall within the pay period.</summary>
        public static List<TimeAdjust> GetListForTimeCardManage(long empNum, long clinicNum, DateTime fromDate, DateTime toDate, bool isAll)
        {
            List<TimeAdjust> retVal = new List<TimeAdjust>();
            //List<TimeAdjust> listTimeAdjusts=new List<TimeAdjust>();
            string command =
                "SELECT * FROM timeadjust WHERE "
                + "EmployeeNum = " + POut.Long(empNum) + " "
                + "AND " + DbHelper.DtimeToDate("TimeEntry") + " >= " + POut.Date(fromDate) + " "
                + "AND " + DbHelper.DtimeToDate("TimeEntry") + " <= " + POut.Date(toDate) + " ";
            if (!isAll)
            {
                command += "AND ClinicNum = " + POut.Long(clinicNum) + " ";
            }
            command += "ORDER BY TimeEntry";
            //listTimeAdjusts=Crud.TimeAdjustCrud.SelectMany(command);
            return Crud.TimeAdjustCrud.SelectMany(command);
            //Delete automatic adjustments.------------------------------------------------------------------------------------------
            //for(int i=0;i<listTimeAdjusts.Count;i++) {
            //	if(!listTimeAdjusts[i].IsAuto) {//skip and never delete manual adjustments
            //		retVal.Add(listTimeAdjusts[i]);
            //		continue;
            //	}
            //	TimeAdjusts.Delete(listTimeAdjusts[i]);//delete auto adjustments for current pay period
            //}
            //Validate---------------------------------------------------------------------------------------------------------------
            //none necessary at this time.
            //return retVal;
        }

        ///<summary>Dates are INCLUSIVE.</summary>
        public static List<TimeAdjust> GetAllForPeriod(DateTime fromDate, DateTime toDate)
        {
            string command = "SELECT * FROM timeadjust "
                + "WHERE TimeEntry >= " + POut.Date(fromDate) + " "
                + "AND TimeEntry < " + POut.Date(toDate.AddDays(1)) + " ";
            return Crud.TimeAdjustCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static long Insert(TimeAdjust timeAdjust)
        {
            return Crud.TimeAdjustCrud.Insert(timeAdjust);
        }

        ///<summary></summary>
        public static void Update(TimeAdjust timeAdjust)
        {
            Crud.TimeAdjustCrud.Update(timeAdjust);
        }

        ///<summary></summary>
        public static void Delete(TimeAdjust adj)
        {
            string command = "DELETE FROM timeadjust WHERE TimeAdjustNum = " + POut.Long(adj.TimeAdjustNum);
            Db.NonQ(command);
        }

        ///<summary>Returns all automatically generated timeAdjusts for a given employee between the date range (inclusive).</summary>
        public static List<TimeAdjust> GetSimpleListAuto(long employeeNum, DateTime startDate, DateTime stopDate)
        {
            List<TimeAdjust> retVal = new List<TimeAdjust>();
            //List<TimeAdjust> listTimeAdjusts=new List<TimeAdjust>();
            string command =
                "SELECT * FROM timeadjust WHERE "
                + "EmployeeNum = " + POut.Long(employeeNum) + " "
                + "AND " + DbHelper.DtimeToDate("TimeEntry") + " >= " + POut.Date(startDate) + " "
                + "AND " + DbHelper.DtimeToDate("TimeEntry") + " < " + POut.Date(stopDate.AddDays(1)) + " "//add one day to go the end of the specified date.
                + "AND IsAuto=1";
            //listTimeAdjusts=Crud.TimeAdjustCrud.SelectMany(command);
            return Crud.TimeAdjustCrud.SelectMany(command);

        }
    }
}