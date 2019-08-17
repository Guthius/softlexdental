using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    public class AllergyDefs
    {
        public static AllergyDef GetOne(long allergyDefNum) => Crud.AllergyDefCrud.SelectOne(allergyDefNum);

        /// <summary>
        /// Gets one AllergyDef matching the specified allergyDefNum from the list passed in. 
        /// If none found will search the db for a matching allergydef. Returns null if not found in the db.
        /// </summary>
        public static AllergyDef GetOne(long allergyDefNum, List<AllergyDef> listAllergyDef)
        {
            for (int i = 0; i < listAllergyDef.Count; i++)
            {
                if (allergyDefNum == listAllergyDef[i].Id)
                {
                    return listAllergyDef[i];
                }
            }
            return GetOne(allergyDefNum);
        }

        /// <summary>
        /// Gets one AllergyDef from the db with matching description, returns null if not found.
        /// </summary>
        public static AllergyDef GetByDescription(string allergyDescription)
        {
            List<AllergyDef> retVal = Crud.AllergyDefCrud.SelectMany("SELECT * FROM allergydef WHERE Description='" + POut.String(allergyDescription) + "'");
            if (retVal.Count > 0)
            {
                return retVal[0];
            }
            return null;
        }

        public static long Insert(AllergyDef allergyDef) => Crud.AllergyDefCrud.Insert(allergyDef);

        public static void Update(AllergyDef allergyDef) => Crud.AllergyDefCrud.Update(allergyDef);

        public static List<AllergyDef> TableToList(DataTable table) => Crud.AllergyDefCrud.TableToList(table);

        public static void Delete(long allergyDefNum)
        {
            Db.NonQ("DELETE FROM allergydef WHERE AllergyDefNum = " + POut.Long(allergyDefNum));
        }

        /// <summary>
        /// Gets all AllergyDefs based on hidden status.
        /// </summary>
        public static List<AllergyDef> GetAll(bool isHidden)
        {
            string command = "";
            if (!isHidden)
            {
                command = "SELECT * FROM allergydef WHERE IsHidden=" + POut.Bool(isHidden) + " ORDER BY Description";
            }
            else
            {
                command = "SELECT * FROM allergydef ORDER BY Description";
            }
            return Crud.AllergyDefCrud.SelectMany(command);
        }

        /// <summary>
        /// Returns true if the allergy def is in use and false if not.
        /// </summary>
        public static bool DefIsInUse(long allergyDefNum)
        {
            if (Db.GetCount("SELECT COUNT(*) FROM allergy WHERE AllergyDefNum=" + POut.Long(allergyDefNum)) != "0")
            {
                return true;
            }

            if (Db.GetCount("SELECT COUNT(*) FROM rxalert WHERE AllergyDefNum=" + POut.Long(allergyDefNum)) != "0")
            {
                return true;
            }

            if (allergyDefNum == Preference.GetLong(PreferenceName.AllergiesIndicateNone))
            {
                return true;
            }
            return false;
        }

        public static List<long> GetChangedSinceAllergyDefNums(DateTime changedSince)
        {
            DataTable dt = Db.GetTable("SELECT AllergyDefNum FROM allergydef WHERE DateTStamp > " + POut.DateT(changedSince));

            List<long> allergyDefNums = new List<long>(dt.Rows.Count);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                allergyDefNums.Add(PIn.Long(dt.Rows[i]["AllergyDefNum"].ToString()));
            }

            return allergyDefNums;
        }

        /// <summary>
        /// Used along with GetChangedSinceAllergyDefNums
        /// </summary>
        public static List<AllergyDef> GetMultAllergyDefs(List<long> allergyDefNums)
        {
            string strAllergyDefNums = "";

            DataTable table;
            if (allergyDefNums.Count > 0)
            {
                for (int i = 0; i < allergyDefNums.Count; i++)
                {
                    if (i > 0)
                    {
                        strAllergyDefNums += "OR ";
                    }
                    strAllergyDefNums += "AllergyDefNum='" + allergyDefNums[i].ToString() + "' ";
                }
                table = Db.GetTable("SELECT * FROM allergydef WHERE " + strAllergyDefNums);
            }
            else
            {
                table = new DataTable();
            }

            AllergyDef[] multAllergyDefs = Crud.AllergyDefCrud.TableToList(table).ToArray();
            List<AllergyDef> allergyDefList = new List<AllergyDef>(multAllergyDefs);
            return allergyDefList;
        }

        /// <summary>
        /// Do not call from outside of ehr. 
        /// Returns the text for a SnomedAllergy Enum as it should appear in human readable form for a CCD.
        /// </summary>
        public static string GetSnomedAllergyDesc(SnomedAllergy snomed)
        {
            string result;
            switch (snomed)
            {
                case SnomedAllergy.AdverseReactions:
                    result = "420134006 - Propensity to adverse reactions (disorder)";
                    break;
                case SnomedAllergy.AdverseReactionsToDrug:
                    result = "419511003 - Propensity to adverse reactions to drug (disorder)";
                    break;
                case SnomedAllergy.AdverseReactionsToFood:
                    result = "418471000 - Propensity to adverse reactions to food (disorder)";
                    break;
                case SnomedAllergy.AdverseReactionsToSubstance:
                    result = "419199007 - Propensity to adverse reactions to substance (disorder)";
                    break;
                case SnomedAllergy.AllergyToSubstance:
                    result = "418038007 - Allergy to substance (disorder)";
                    break;
                case SnomedAllergy.DrugAllergy:
                    result = "416098002 - Drug allergy (disorder)";
                    break;
                case SnomedAllergy.DrugIntolerance:
                    result = "59037007 - Drug intolerance (disorder)";
                    break;
                case SnomedAllergy.FoodAllergy:
                    result = "235719002 - Food allergy (disorder)";
                    break;
                case SnomedAllergy.FoodIntolerance:
                    result = "420134006 - Food intolerance (disorder)";
                    break;
                case SnomedAllergy.None:
                    result = "";
                    break;
                default:
                    result = "Error";
                    break;
            }
            return result;
        }

        /// <summary>
        /// Returns the name of the allergy. Returns an empty string if allergyDefNum=0.
        /// </summary>
        public static string GetDescription(long allergyDefNum)
        {
            if (allergyDefNum == 0)
            {
                return "";
            }
            return Crud.AllergyDefCrud.SelectOne(allergyDefNum).Description;
        }

        /// <summary>
        /// Returns the AllergyDef with the corresponding SNOMED allergyTo code. Returns null if codeValue is empty string.
        /// </summary>
        public static AllergyDef GetAllergyDefFromCode(string codeValue)
        {
            if (codeValue == "")
            {
                return null;
            }
            return Crud.AllergyDefCrud.SelectOne("SELECT * FROM allergydef WHERE SnomedAllergyTo=" + POut.String(codeValue));
        }

        /// <summary>
        /// Returns the AllergyDef with the corresponding Medication. Returns null if medicationNum is 0.
        /// </summary>
        public static AllergyDef GetAllergyDefFromMedication(long medicationNum)
        {
            if (medicationNum == 0)
            {
                return null;
            }
            return Crud.AllergyDefCrud.SelectOne("SELECT * FROM allergydef WHERE MedicationNum=" + POut.Long(medicationNum));
        }

        /// <summary>
        /// Returns the AllergyDef set to SnomedType 2 (DrugAllergy) or SnomedType 3 (DrugIntolerance) that is 
        /// attached to a medication with this rxnorm.  Returns null if rxnorm is 0 or no allergydef for this rxnorm exists.
        /// Used by HL7 service for inserting drug allergies for patients.
        /// </summary>
        public static AllergyDef GetAllergyDefFromRxnorm(long rxnorm)
        {
            if (rxnorm == 0) return null;

            return Crud.AllergyDefCrud.SelectOne(
                "SELECT allergydef.* FROM allergydef " +
                "INNER JOIN medication ON allergydef.MedicationNum=medication.MedicationNum " +
                "AND medication.RxCui=" + POut.Long(rxnorm) + " " +
                "WHERE allergydef.SnomedType IN(" + (int)SnomedAllergy.DrugAllergy + "," + (int)SnomedAllergy.DrugIntolerance + ")");
        }
    }
}