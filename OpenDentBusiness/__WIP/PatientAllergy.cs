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
    /// An allergy attached to a patient and linked to an AllergyDef.
    /// </summary>
    public class PatientAllergy : DataRecord
    {
        public long AllergyId;
        public long PatientId;
        
        /// <summary>
        /// Adverse reaction description.
        /// </summary>
        public string Reaction;

        /// <summary>
        /// Snomed code for reaction. 
        /// Optional and independent of the Reaction text field.
        /// Not needed for reporting.  
        /// Only used for CCD export/import.
        /// </summary>
        public string SnomedReaction;

        /// <summary>
        /// To be used for synch with web server for CertTimelyAccess.
        /// </summary>
        public DateTime DateModified;
        
        /// <summary>
        /// The historical date that the patient had the adverse reaction to this agent.
        /// </summary>
        public DateTime DateAdverseReaction;
        
        /// <summary>
        /// True if still an active allergy.
        /// False helps hide it from the list of active allergies.
        /// </summary>
        public bool Active;

        /// <summary>
        /// Constructs a new instance of the <see cref="PatientAllergy"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="PatientAllergy"/> instance.</returns>
        private static PatientAllergy FromReader(MySqlDataReader dataReader)
        {
            return new PatientAllergy
            {
                Id = (long)dataReader["id"],
                AllergyId = (long)dataReader["allergy_id"],
                PatientId = (long)dataReader["patient_id"],
                Reaction = (string)dataReader["reaction"],
                SnomedReaction = (string)dataReader["snomed_reaction"],
                DateModified = (DateTime)dataReader["date_modified"],
                DateAdverseReaction = (DateTime)dataReader["date_adverse_reaction"],
                Active = Convert.ToBoolean(dataReader["active"])
            };
        }

        /// <summary>
        /// Gets a list of all allergies for the specifeid patient.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <returns>A list of allergies.</returns>
        public static List<PatientAllergy> GetByPatient(long patientId) =>
            SelectMany("SELECT * FROM `patient_allergies` WHERE `patient_id` = " + patientId, FromReader);

        /// <summary>
        /// Gets a list of all allergies for the specifeid patient.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <param name="includeInactive">A value indicating whether to include inactive allergies in the list.</param>
        /// <returns>A list of allergies.</returns>
        public static List<PatientAllergy> GetByPatient(long patientId, bool includeInactive)
        {
            var commandText = "SELECT * FROM `patient_allergies` WHERE `patient_id` = " + patientId;

            if (!includeInactive)
            {
                commandText += " AND `active` = 1";
            }

            return SelectMany(commandText, FromReader);
        }

        /// <summary>
        /// Gets a list of patient allergies that have been modified after the specified date.
        /// </summary>
        /// <param name="changedSince"></param>
        /// <returns>A list of patient allergies.</returns>
        public static List<PatientAllergy> GetChangedSince(DateTime changedSince) =>
            SelectMany(
                "SELECT * FROM `patient_allergies` WHERE `date_modified` > ?date_modified", FromReader,
                    new MySqlParameter("date_modified", changedSince));

        /// <summary>
        /// Inserts the specified patient allergy into the database.
        /// </summary>
        /// <param name="allergy">The patient allergy.</param>
        /// <returns>The ID assigned to the patient allergy.</returns>
        public static long Insert(PatientAllergy allergy) =>
            allergy.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `patient_allergies` (`allergy_id`, `patient_id`, `reaction`, `snomed_reaction`, `date_modified`, " +
                "`date_adverse_reaction`, `active`) VALUES (?allergy_id, ?patient_id, ?reaction, ?snomed_reaction, ?date_modified, " +
                "?date_adverse_reaction, ?active)",
                    new MySqlParameter("allergy_id", allergy.AllergyId),
                    new MySqlParameter("patient_id", allergy.PatientId),
                    new MySqlParameter("reaction", allergy.Reaction ?? ""),
                    new MySqlParameter("snomed_reaction", allergy.SnomedReaction ?? ""),
                    new MySqlParameter("date_modified", allergy.DateModified),
                    new MySqlParameter("date_adverse_reaction", allergy.DateAdverseReaction),
                    new MySqlParameter("active", allergy.Active));

        /// <summary>
        /// Updates the specified patient allergy in the database.
        /// </summary>
        /// <param name="allergy">The patient allergy.</param>
        public static void Update(PatientAllergy allergy) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `patient_allergies` SET `allergy_id` = ?allergy_id, `patient_id` = ?patient_id, `reaction` = ?reaction, " +
                "`snomed_reaction` = ?snomed_reaction, `date_modified` = ?date_modified, `date_adverse_reacion` = ?date_adverse_reaction, " +
                "`active` = ?active WHERE `id` = ?id",
                    new MySqlParameter("allergy_id", allergy.AllergyId),
                    new MySqlParameter("patient_id", allergy.PatientId),
                    new MySqlParameter("reaction", allergy.Reaction ?? ""),
                    new MySqlParameter("snomed_reaction", allergy.SnomedReaction ?? ""),
                    new MySqlParameter("date_modified", allergy.DateModified),
                    new MySqlParameter("date_adverse_reaction", allergy.DateAdverseReaction),
                    new MySqlParameter("active", allergy.Active),
                    new MySqlParameter("id", allergy.Id));

        /// <summary>
        /// Deletes the specified patient allergy from the database.
        /// </summary>
        /// <param name="patientAllergyId">The ID of the patient allergy.</param>
        public static void Delete(long patientAllergyId) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM patient_allergies WHERE id = " + patientAllergyId);

        public static string[] GetPatNamesForAllergy(long allergyId) =>
            SelectMany(
                "SELECT CONCAT(CONCAT(CONCAT(CONCAT(`lastname`, ', '), `firstname`), ' '), `preferred`) FROM `patient_allergies`, `patients` " +
                "WHERE `patient_allergies`.`patient_id` = `patients`.`id` " +
                "AND `patient_allergies`.`allergy_id` = " + allergyId,
                dataReader => (string)dataReader[0])
            .ToArray();

        public static List<long> GetPatientsWithAllergy(List<long> patientIds) =>
            SelectMany(
                "SELECT DISTINCT `patient_id` FROM `patient_allergies` WHERE `patient_id` IN (" + string.Join(", ", patientIds) + ")",
                dataReader => (long)dataReader[0]);

        public static void ResetTimeStamps(long patientId) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `patient_allergies` SET `date_modified` = NOW() WHERE `patient_id` = " + patientId);

        public static void ResetTimeStamps(long patientId, bool onlyActive)
        {
            string commandText = "UPDATE `patient_allergies` SET `date_modified` = NOW() WHERE `patient_id` =" + patientId;
            if (onlyActive)
            {
                commandText += " AND `active` = 1";
            }

            DataConnection.ExecuteNonQuery(commandText);
        }
    }
}
