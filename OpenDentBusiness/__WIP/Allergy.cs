/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    /// An allergy definition. Gets linked to an allergy and patient.
    /// Allergies will not show in CCD messages unless they have a valid
    /// Medication (that has an RxNorm) or UniiCode.
    /// </summary>
    public class Allergy : DataRecord
    {
        /// <summary>
        /// Name of the drug. User can change this. 
        /// If an RxCui is present, the RxNorm string can be pulled from the in-memory table for UI display in addition to the Description.
        /// </summary>
        public string Description;

        /// <summary>
        /// SNOMED Allergy Type Code.  Only used to create CCD in FormSummaryOfCare.
        /// </summary>
        public SnomedAllergy SnomedType;

        /// <summary>
        /// Optional, only used with CCD messages.
        /// </summary>
        public long? MedicationId;

        /// <summary>
        /// The Unii code for the Allergen.
        /// Optional, but there must be either a MedicationNum or a UniiCode. 
        /// Used to create CCD in FormSummaryOfCare, or set during CCD allergy reconcile.
        /// </summary>
        public string UniiCode;

        /// <summary>
        /// The last date and time this row was altered.
        /// </summary>
        public DateTime DateModified;

        /// <summary>
        /// Value indicating whether the allergy is hidden.
        /// </summary>
        public bool Hidden;

        /// <summary>
        /// Constructs a new instance of the <see cref="Allergy"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Allergy"/> instance.</returns>
        private static Allergy FromReader(MySqlDataReader dataReader)
        {
            return new Allergy
            {
                Id = (long)dataReader["id"],
                Description = (string)dataReader["description"],
                SnomedType = (SnomedAllergy)Convert.ToInt32(dataReader["snomed_type"]),
                MedicationId = dataReader["medication_id"] as long?,
                UniiCode = (string)dataReader["unii_code"],
                DateModified = (DateTime)dataReader["date_modified"],
                Hidden = (bool)dataReader["hidden"]
            };
        }

        /// <summary>
        /// Gets a list of all allergies from the database.
        /// </summary>
        /// <param name="includeHidden">Value indicating whether to include hidden allergies in the list.</param>
        /// <returns>A list of allergies.</returns>
        public static List<Allergy> All(bool includeHidden)
        {
            var commandText =
                !includeHidden ?
                    "SELECT * FROM `allergies` WHERE `hidden` = 0 ORDER BY `description`" :
                    "SELECT * FROM `allergies` ORDER BY `description`";

            return SelectMany(commandText, FromReader);
        }

        /// <summary>
        /// Gets one AllergyDef matching the specified allergyDefNum from the list passed in. 
        /// If none found will search the db for a matching allergydef. Returns null if not found in the db.
        /// </summary>
        public static Allergy GetById(long allergyId, List<Allergy> allergies = null)
        {
            if (allergies != null && allergies.Count > 0)
            {
                foreach (var allergy in allergies)
                {
                    if (allergyId == allergy.Id)
                    {
                        return allergy;
                    }
                }
            }

            return SelectOne("SELECT * FROM `allergies` WHERE id = " + allergyId, FromReader);
        }

        /// <summary>
        /// Gets one AllergyDef from the db with matching description, returns null if not found.
        /// </summary>
        public static Allergy GetByDescription(string allergyDescription) =>
            SelectOne("SELECT * FROM `allergies` WHERE `description` = ?description", FromReader, 
                new MySqlParameter("description", allergyDescription));

        /// <summary>
        /// Inserts the specified allergy into the database.
        /// </summary>
        /// <param name="allergy">The allergy.</param>
        /// <returns>The ID assigned to the allergy.</returns>
        public static long Insert(Allergy allergy) =>
            allergy.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `allergies` (`description`, `snomed_type`, `medication_id`, `unii_code`, `date_modified`, `hidden`) " +
                "VALUES (?description, ?snomed_type, ?medication_id, ?unii_code, ?date_modified, ?hidden)",
                    new MySqlParameter("description", allergy.Description ?? ""),
                    new MySqlParameter("snomed_type", (int)allergy.SnomedType),
                    new MySqlParameter("medication_id", allergy.MedicationId.HasValue ? (object)allergy.MedicationId.Value : DBNull.Value),
                    new MySqlParameter("unii_code", allergy.UniiCode ?? ""),
                    new MySqlParameter("date_modified", allergy.DateModified),
                    new MySqlParameter("hidden", allergy.Hidden));

        /// <summary>
        /// Updates the specified allergy in the database.
        /// </summary>
        /// <param name="allergy">The allergy.</param>
        public static void Update(Allergy allergy) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `allergies` SET `description` = ?description, `snomed_type` = ?snomed_type, `medication_id` = ?medication_id, " +
                "`unii_code` = ?unii_code, `date_modified` = ?date_modified, `hidden` = ?hidden WHERE `id` = ?id",
                    new MySqlParameter("description", allergy.Description ?? ""),
                    new MySqlParameter("snomed_type", (int)allergy.SnomedType),
                    new MySqlParameter("medication_id", allergy.MedicationId.HasValue ? (object)allergy.MedicationId.Value : DBNull.Value),
                    new MySqlParameter("unii_code", allergy.UniiCode ?? ""),
                    new MySqlParameter("date_modified", allergy.DateModified),
                    new MySqlParameter("hidden", allergy.Hidden),
                    new MySqlParameter("id", allergy.Id));
        
        /// <summary>
        /// Deletes the specified allergy from the database.
        /// </summary>
        /// <param name="allergyId">The ID of the allergy.</param>
        public static void Delete(long allergyId) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `allergies` WHERE `id` = " + allergyId);

        /// <summary>
        /// Checks whether the specified allergy is currently in use.
        /// </summary>
        public static bool IsInUse(long allergyId)
        {
            if (allergyId == Preference.GetLong(PreferenceName.AllergiesIndicateNone))
                return true;

            var count =
                DataConnection.ExecuteLong(
                    "SELECT COUNT(*) FROM `patient_allergies` WHERE `allergy_id` = " + allergyId);

            if (count > 0) return true;
            
            count =
                DataConnection.ExecuteLong(
                    "SELECT COUNT(*) FROM `rxalert` WHERE `allergy_id` = " + allergyId);

            if (count > 0) return true;

            return false;
        }

        public static List<long> GetChangedSince(DateTime changedSince) =>
            SelectMany("SELECT `id` FROM `allergies` WHERE `date_modified` > ?changed_since", dataReader => (long)dataReader[0],
                new MySqlParameter("changed_since", changedSince));
        
        public static List<Allergy> GetMultAllergyDefs(List<long> allergyIds)
        {
            if (allergyIds != null && allergyIds.Count > 0)
            {
                return SelectMany(
                    "SELECT * FROM allergies WHERE id IN (" + string.Join(", ", allergyIds) + ")", 
                        FromReader);
            }

            return new List<Allergy>();
        }

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

        public static string GetDescription(long allergyId) =>
            GetById(allergyId)?.Description ?? "";
        
        public static Allergy GetAllergyDefFromMedication(long medicationId) => 
            SelectOne("SELECT * FROM allergydef WHERE medication_id = " + medicationId, FromReader);

        /// <summary>
        /// Returns the AllergyDef set to SnomedType 2 (DrugAllergy) or SnomedType 3 (DrugIntolerance) that is 
        /// attached to a medication with this rxnorm.  Returns null if rxnorm is 0 or no allergydef for this rxnorm exists.
        /// Used by HL7 service for inserting drug allergies for patients.
        /// </summary>
        public static Allergy GetAllergyDefFromRxnorm(long rxnorm) =>
            SelectOne(
                "SELECT `allergies`.* FROM `allergies` " +
                "INNER JOIN `medications` ON `allergies`.`medication_id` = `medications`.`id` " +
                "AND `medications`.`rx_cui` = " + rxnorm + " " +
                "WHERE `allergies`.`snomed_type` IN(" + (int)SnomedAllergy.DrugAllergy + ", " + (int)SnomedAllergy.DrugIntolerance + ")",
                    FromReader);
        
    }

    public enum SnomedAllergy
    {
        /// <summary>
        /// No SNOMED allergy type code has been assigned.
        /// </summary>
        None,

        /// <summary>
        /// Allergy to substance (disorder), code number 418038007.
        /// </summary>
        AllergyToSubstance = 418038007,

        /// <summary>
        /// Drug allergy (disorder), code number 416098002.
        /// </summary>
        DrugAllergy = 416098002,

        /// <summary>
        /// Drug intolerance (disorder), code number 59037007.
        /// </summary>
        DrugIntolerance = 59037007,

        /// <summary>
        /// Food allergy (disorder), code number 414285001.
        /// </summary>
        FoodAllergy = 414285001,

        /// <summary>
        /// Food intolerance (disorder), code number 235719002.
        /// </summary>
        FoodIntolerance = 235719002,

        /// <summary>
        /// Propensity to adverse reactions (disorder), code number 420134006.
        /// </summary>
        AdverseReactions = 420134006,

        /// <summary>
        /// Propensity to adverse reactions to drug (disorder), code number 419511003
        /// </summary>
        AdverseReactionsToDrug = 419511003,

        /// <summary>
        /// Propensity to adverse reactions to food (disorder), code number 418471000.
        /// </summary>
        AdverseReactionsToFood = 418471000,

        /// <summary>
        /// Propensity to adverse reactions to substance (disorder), code number 419199007.
        /// </summary>
        AdverseReactionsToSubstance = 419199007
    }
}
