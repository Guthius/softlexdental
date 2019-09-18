using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    public class Allergies
    {
        public static List<PatientAllergy> Refresh(long patNum)
        {
            return Crud.AllergyCrud.SelectMany("SELECT * FROM allergy WHERE PatNum = " + POut.Long(patNum));
        }

        public static PatientAllergy GetOne(long allergyNum) => Crud.AllergyCrud.SelectOne(allergyNum);

        public static long Insert(PatientAllergy allergy) => Crud.AllergyCrud.Insert(allergy);

        public static void Update(PatientAllergy allergy) => Crud.AllergyCrud.Update(allergy);

        public static void Delete(long allergyNum)
        {
            Db.NonQ("DELETE FROM allergy WHERE AllergyNum = " + POut.Long(allergyNum));
        }

        /// <summary>
        /// Gets all allergies for patient whether active or not.
        /// </summary>
        public static List<PatientAllergy> GetAll(long patNum, bool showInactive)
        {
            string command = "SELECT * FROM allergy WHERE PatNum = " + POut.Long(patNum);
            if (!showInactive)
            {
                command += " AND StatusIsActive<>0";
            }
            return Crud.AllergyCrud.SelectMany(command);
        }

        public static List<long> GetChangedSinceAllergyNums(DateTime changedSince)
        {
            string command = "SELECT AllergyNum FROM allergy WHERE DateTStamp > " + POut.DateT(changedSince);
            DataTable dt = Db.GetTable(command);
            List<long> allergynums = new List<long>(dt.Rows.Count);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                allergynums.Add(PIn.Long(dt.Rows[i]["AllergyNum"].ToString()));
            }
            return allergynums;
        }

        /// <summary>
        /// Used along with GetChangedSinceAllergyNums
        /// </summary>
        public static List<PatientAllergy> GetMultAllergies(List<long> allergyNums)
        {
            string strAllergyNums = "";
            DataTable table;
            if (allergyNums.Count > 0)
            {
                for (int i = 0; i < allergyNums.Count; i++)
                {
                    if (i > 0)
                    {
                        strAllergyNums += "OR ";
                    }
                    strAllergyNums += "AllergyNum='" + allergyNums[i].ToString() + "' ";
                }
                string command = "SELECT * FROM allergy WHERE " + strAllergyNums;
                table = Db.GetTable(command);
            }
            else
            {
                table = new DataTable();
            }
            PatientAllergy[] multAllergies = Crud.AllergyCrud.TableToList(table).ToArray();
            List<PatientAllergy> allergyList = new List<PatientAllergy>(multAllergies);
            return allergyList;
        }

        /// <summary>
        /// Returns an array of all patient names who are using this allergy.
        /// </summary>
        public static string[] GetPatNamesForAllergy(long allergyDefNum)
        {
            DataTable table = Db.GetTable(
                "SELECT CONCAT(CONCAT(CONCAT(CONCAT(LName,', '),FName),' '),Preferred) FROM allergy,patient " +
                "WHERE allergy.PatNum=patient.PatNum " +
                "AND allergy.AllergyDefNum=" + POut.Long(allergyDefNum));

            string[] retVal = new string[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                retVal[i] = PIn.String(table.Rows[i][0].ToString());
            }
            return retVal;
        }

        /// <summary>
        /// Returns a list of PatNums that have an allergy from the PatNums that are passed in.
        /// </summary>
        public static List<long> GetPatientsWithAllergy(List<long> listPatNums)
        {
            if (listPatNums.Count == 0)
            {
                return new List<long>();
            }

            return Db.GetListLong(
                "SELECT DISTINCT PatNum FROM allergy " +
                "WHERE PatNum IN (" + string.Join(",", listPatNums) + ") " +
                "AND allergy.AllergyDefNum != " + POut.Long(Preference.GetLong(PreferenceName.AllergiesIndicateNone)));
        }

        /// <summary>
        /// Changes the value of the DateTStamp column to the current time stamp for all allergies of a patient
        /// </summary>
        public static void ResetTimeStamps(long patNum)
        {
            Db.NonQ("UPDATE allergy SET DateTStamp = CURRENT_TIMESTAMP WHERE PatNum =" + POut.Long(patNum));
        }

        /// <summary>
        /// Changes the value of the DateTStamp column to the current time stamp for all allergies of a patient that are the status specified
        /// </summary>
        public static void ResetTimeStamps(long patNum, bool onlyActive)
        {
            string command = "UPDATE allergy SET DateTStamp = CURRENT_TIMESTAMP WHERE PatNum =" + POut.Long(patNum);
            if (onlyActive)
            {
                command += " AND StatusIsActive = " + POut.Bool(onlyActive);
            }
            Db.NonQ(command);
        }
    }
}