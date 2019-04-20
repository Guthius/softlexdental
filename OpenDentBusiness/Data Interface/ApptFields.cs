using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class ApptFields
    {
        ///<summary>Gets one ApptField from the db.</summary>
        public static ApptField GetOne(long apptFieldNum)
        {
            return Crud.ApptFieldCrud.SelectOne(apptFieldNum);
        }

        ///<summary></summary>
        public static long Insert(ApptField apptField)
        {
            return Crud.ApptFieldCrud.Insert(apptField);
        }

        ///<summary></summary>
        public static void Update(ApptField apptField)
        {
            Crud.ApptFieldCrud.Update(apptField);
        }

        ///<summary></summary>
        public static void Delete(long apptFieldNum)
        {
            string command = "DELETE FROM apptfield WHERE ApptFieldNum = " + POut.Long(apptFieldNum);
            Db.NonQ(command);
        }

        public static List<ApptField> GetForAppt(long aptNum)
        {
            string command = "SELECT * FROM apptfield WHERE AptNum = " + POut.Long(aptNum);
            return Crud.ApptFieldCrud.SelectMany(command);
        }
    }
}