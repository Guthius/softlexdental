using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using OpenDentBusiness.Crud;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class ReqNeededs
    {
        public static DataTable Refresh(long schoolClass, long schoolCourse)
        {
            string command = "SELECT * FROM reqneeded WHERE SchoolClassNum=" + POut.Long(schoolClass)
                + " AND SchoolCourseNum=" + POut.Long(schoolCourse)
                + " ORDER BY Descript";
            return Db.GetTable(command);
        }

        public static ReqNeeded GetReq(long reqNeededNum)
        {
            string command = "SELECT * FROM reqneeded WHERE ReqNeededNum=" + POut.Long(reqNeededNum);
            return Crud.ReqNeededCrud.SelectOne(command);
        }

        ///<summary></summary>
        public static void Update(ReqNeeded req)
        {
            Crud.ReqNeededCrud.Update(req);
        }

        ///<summary></summary>
        public static long Insert(ReqNeeded req)
        {
            return Crud.ReqNeededCrud.Insert(req);
        }

        ///<summary>Surround with try/catch.</summary>
        public static void Delete(long reqNeededNum)
        {
            //still need to validate
            string command = "DELETE FROM reqneeded "
                + "WHERE ReqNeededNum = " + POut.Long(reqNeededNum);
            Db.NonQ(command);
        }

        ///<summary>Returns a list with all reqneeded entries in the database.</summary>
        public static List<ReqNeeded> GetListFromDb()
        {
            string command = "SELECT * FROM reqneeded ORDER BY Descript";
            return Crud.ReqNeededCrud.SelectMany(command);
        }

        ///<summary>Inserts, updates, or deletes rows to reflect changes between listNew and stale listOld.</summary>
        public static void Sync(List<ReqNeeded> listNew, List<ReqNeeded> listOld)
        {
            Crud.ReqNeededCrud.Sync(listNew, listOld);
        }
    }
}