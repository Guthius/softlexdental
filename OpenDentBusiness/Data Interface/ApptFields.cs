using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    public class ApptFields
    {
        public static ApptField GetOne(long apptFieldNum) => Crud.ApptFieldCrud.SelectOne(apptFieldNum);

        public static long Insert(ApptField apptField) => Crud.ApptFieldCrud.Insert(apptField);

        public static void Update(ApptField apptField) => Crud.ApptFieldCrud.Update(apptField);

        public static void Delete(long apptFieldNum)
        {
            Db.NonQ("DELETE FROM apptfield WHERE ApptFieldNum = " + POut.Long(apptFieldNum));
        }

        public static List<ApptField> GetForAppt(long aptNum)
        {
            return Crud.ApptFieldCrud.SelectMany("SELECT * FROM apptfield WHERE AptNum = " + POut.Long(aptNum));
        }
    }
}