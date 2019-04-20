using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class Interventions
    {
        ///<summary></summary>
        public static long Insert(Intervention intervention)
        {
            return Crud.InterventionCrud.Insert(intervention);
        }

        ///<summary></summary>
        public static void Update(Intervention intervention)
        {
            Crud.InterventionCrud.Update(intervention);
        }

        ///<summary></summary>
        public static void Delete(long interventionNum)
        {
            string command = "DELETE FROM intervention WHERE InterventionNum = " + POut.Long(interventionNum);
            Db.NonQ(command);
        }

        ///<summary></summary>
        public static List<Intervention> Refresh(long patNum)
        {
            string command = "SELECT * FROM intervention WHERE PatNum = " + POut.Long(patNum);
            return Crud.InterventionCrud.SelectMany(command);
        }

        public static List<Intervention> Refresh(long patNum, InterventionCodeSet codeSet)
        {
            string command = "SELECT * FROM intervention WHERE PatNum = " + POut.Long(patNum) + " AND CodeSet = " + POut.Int((int)codeSet);
            return Crud.InterventionCrud.SelectMany(command);
        }

        ///<summary>Gets list of CodeValue strings from interventions with DateEntry in the last year and CodeSet equal to the supplied codeSet.
        ///Result list is grouped by CodeValue, CodeSystem even though we only return the list of CodeValues.  However, there are no codes in the
        ///EHR intervention code list that conflict between code systems, so we should never have a duplicate code in the returned list.</summary>
        public static List<string> GetAllForCodeSet(InterventionCodeSet codeSet)
        {
            string command = "SELECT CodeValue FROM intervention WHERE CodeSet=" + POut.Int((int)codeSet) + " "
                + "AND " + DbHelper.DtimeToDate("DateEntry") + ">=" + POut.Date(MiscData.GetNowDateTime().AddYears(-1)) + " "
                + "GROUP BY CodeValue,CodeSystem";
            return Db.GetListString(command);
        }

        ///<summary>Gets one Intervention from the db.</summary>
        public static Intervention GetOne(long interventionNum)
        {
            return Crud.InterventionCrud.SelectOne(interventionNum);
        }
    }
}