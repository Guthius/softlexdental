using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness
{
    public class ReqStudents
    {
        public static List<ReqStudent> GetForAppt(long aptNum)
        {
            string command = "SELECT * FROM reqstudent WHERE AptNum=" + POut.Long(aptNum) + " ORDER BY ProvNum,Descript";
            return Crud.ReqStudentCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static void Update(ReqStudent req)
        {
            Crud.ReqStudentCrud.Update(req);
        }

        ///<summary></summary>
        public static long Insert(ReqStudent req)
        {
            return Crud.ReqStudentCrud.Insert(req);
        }

        ///<summary>Provider(student) is required.</summary>
        public static DataTable GetForCourseClass(long schoolCourse, long schoolClass)
        {
            string command = "SELECT Descript,ReqNeededNum "
                + "FROM reqneeded ";
            //if(schoolCourse==0){
            //	command+="WHERE ProvNum="+POut.PInt(provNum);
            //}
            //else{
            command += "WHERE SchoolCourseNum=" + POut.Long(schoolCourse)
        //+" AND ProvNum="+POut.PInt(provNum);
        //}
        + " AND SchoolClassNum=" + POut.Long(schoolClass);
            command += " ORDER BY Descript";
            return Db.GetTable(command);
        }


        ///<summary>All fields for all reqs will have already been set.  All except for reqstudent.ReqStudentNum if new.  Now, they just have to be persisted to the database.</summary>
        public static void SynchApt(List<ReqStudent> listReqsAttached, List<ReqStudent> listReqsRemoved, long aptNum)
        {
            string command;
            //first, delete all that were removed from this appt
            if (listReqsRemoved.Count(x => x.ReqStudentNum != 0) > 0)
            {
                command = "DELETE FROM reqstudent WHERE ReqStudentNum IN(" + string.Join(",", listReqsRemoved.Where(x => x.ReqStudentNum != 0)
                    .Select(x => x.ReqStudentNum)) + ")";
                Db.NonQ(command);
            }
            //second, detach all from this appt
            command = "UPDATE reqstudent SET AptNum=0 WHERE AptNum=" + POut.Long(aptNum);
            Db.NonQ(command);
            if (listReqsAttached.Count == 0)
            {
                return;
            }
            for (int i = 0; i < listReqsAttached.Count; i++)
            {
                if (listReqsAttached[i].ReqStudentNum == 0)
                {
                    ReqStudents.Insert(listReqsAttached[i]);
                }
                else
                {
                    ReqStudents.Update(listReqsAttached[i]);
                }
            }
        }
    }
}