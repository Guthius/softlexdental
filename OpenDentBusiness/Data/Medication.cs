/**
 * Copyright (C) 2019 Dental Stars SRL
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
using System.Data;
using System.Linq;

namespace OpenDentBusiness
{
    /// <summary>
    /// A list of medications, not attached to any particular patient.
    /// Not allowed to delete if in use by a patient. Not allowed to edit name
    /// once created due to possibility of damage to patient record.
    /// </summary>
    public class Medication : DataRecord
    {
        /// <summary>
        /// RxNorm Code identifier. We should have used a string type. Used by EHR in CQM. But the 
        /// queries should use medicationpat.RxCui, NOT this RxCui, because all medicationpats 
        /// (meds and orders) coming back from NewCrop will not have a FK to this medication table.
        /// When this RxCui is modified by the user, then medicationpat. RxCui is automatically 
        /// updated where medicationpat.MedicationNum matches this medication.
        /// </summary>
        public string RxCui;

        /// <summary>
        /// Name of the medication. User can change this. If an RxCui is present, the RxNorm string
        /// can be pulled from the in-memory table for UI display in addition to the MedName.
        /// </summary>
        public string Description;

        /// <summary>
        /// FK to medication.MedicationNum. Cannot be zero. If this is a generic drug, then the 
        /// GenericNum will be the same as the MedicationNum. Otherwise, if this is a brand drug, 
        /// then the GenericNum will be a non-zero value corresponding to another medicaiton.
        /// </summary>
        public long? GenericId;

        /// <summary>
        /// The last date and time this row was altered. Not user editable.
        /// </summary>
        public DateTime LastModified;

        /// <summary>
        /// Notes.
        /// </summary>
        public string Notes;

        /// <summary>
        /// Constructs a new instance of the <see cref="Medication"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Medication"/> instance.</returns>
        static Medication FromReader(MySqlDataReader dataReader)
        {
            return new Medication
            {
                Id = Convert.ToInt64(dataReader["id"]),
                RxCui = Convert.ToString(dataReader["rxcui"]),
                Description = Convert.ToString(dataReader["description"]),
                GenericId = dataReader["generic_id"] as long?,
                LastModified = Convert.ToDateTime(dataReader["last_modified"]),
                Notes = Convert.ToString(dataReader["notes"])
            };
        }

        /// <summary>
        /// Selects all medications using the specified SQL command.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">Parameters for the command.</param>
        /// <returns>A list of medications.</returns>
        public static List<Medication> SelectMany(string commandText, params MySqlParameter[] parameters) => 
            SelectMany(commandText, FromReader, parameters);

        /// <summary>
        /// Gets a list of all medications.
        /// </summary>
        /// <returns>A list of medications.</returns>
        public static List<Medication> All() =>
            SelectMany("SELECT * FROM `medications`", FromReader);

        /// <summary>
        /// Gets the medication with the specified ID.
        /// </summary>
        /// <param name="medicationId">The ID of the medification.</param>
        /// <returns>The medication with the specified ID.</returns>
        public static Medication GetById(long medicationId) =>
            SelectOne("SELECT * FROM `medications` WHERE `id` = " + medicationId, FromReader);

        /// <summary>
        /// Gets the medication with the specified RXCUI code.
        /// </summary>
        /// <param name="rxcui">The RXCUI code.</param>
        /// <returns>The medication with the specified RXCUI code.</returns>
        public static Medication GetByRxCui(string rxcui) =>
            SelectOne("SELECT * FROM `medications` WHERE `rxcui` = ?rxcui ORDER BY `id`", FromReader,
                new MySqlParameter("rxcui", rxcui ?? ""));

        /// <summary>
        /// Gets the medication with the specified description.
        /// </summary>
        /// <param name="description">The medication description.</param>
        /// <returns>The medication with the specified description.</returns>
        public static Medication GetByDescription(string description) =>
            SelectOne("SELECT * FROM `medications` WHERE `description` = ?description ORDER BY `id`", FromReader,
                new MySqlParameter("description", description));

        /// <summary>
        /// Gets a list of all brand medications for the generic medication with the specified ID.
        /// </summary>
        /// <param name="medicationId">The ID of the generic medication.</param>
        /// <returns>A list of medications.</returns>
        public static List<Medication> GetBrands(long medicationId) =>
            SelectMany("SELECT* FROM `medications` WHERE `generic_id` = " + medicationId, FromReader);

        /// <summary>
        /// Gets a list of all brands names for the generic medication with the specified ID.
        /// </summary>
        /// <param name="medicationId">The ID of the generic medication.</param>
        /// <returns>A list of brand names.</returns>
        public static List<string> GetBrandNames(long medicationId) =>
            SelectMany("SELECT `description` FROM `medications` WHERE `generic_id` = " + medicationId, reader => Convert.ToString(reader[0]));

        /// <summary>
        /// Finds all medications matching the specified search text.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns>A list of medications.</returns>
        public static List<Medication> Find(string searchText = "")
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return SelectMany("SELECT * FROM `medications`", FromReader);

            return
                SelectMany("SELECT * FROM `medications` WHERE `description` LIKE ?description", FromReader,
                    new MySqlParameter("description", $"%{searchText}%"));
        }

        /// <summary>
        /// Inserts the specified medication into the database.
        /// </summary>
        /// <param name="medication">The medication.</param>
        /// <returns>The ID assigned to the medication.</returns>
        public static long Insert(Medication medication) =>
            medication.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `medications` (`rxcui`, `description`, `generic_id`, `last_modified`, `notes`) VALUES (?rxcui, ?description, ?generic_id, ?last_modified, ?notes)",
                    new MySqlParameter("rxcui", medication.RxCui ?? ""),
                    new MySqlParameter("description", medication.Description ?? ""),
                    new MySqlParameter("generic_id", medication.GenericId.HasValue ? (object)medication.GenericId.Value : DBNull.Value),
                    new MySqlParameter("last_modified", (medication.LastModified = DateTime.UtcNow)),
                    new MySqlParameter("notes", medication.Notes ?? ""));

        /// <summary>
        /// Updates the specified medication in the database.
        /// </summary>
        /// <param name="medication">The medication.</param>
        public static void Update(Medication medication) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `medications` SET `rxcui` = ?rxcui, `description` = ?description, `generic_id` = ?generic_id, `last_modified` = ?last_modified, `notes` = ?notes WHERE `id` = ?id",
                    new MySqlParameter("rxcui", medication.RxCui ?? ""),
                    new MySqlParameter("description", medication.Description ?? ""),
                    new MySqlParameter("generic_id", medication.GenericId.HasValue ? (object)medication.GenericId.Value : DBNull.Value),
                    new MySqlParameter("last_modified", (medication.LastModified = DateTime.UtcNow)),
                    new MySqlParameter("notes", medication.Notes ?? ""),
                    new MySqlParameter("id", medication.Id));

        #region CLEANUP

        public static void Delete(Medication medication)
        {
            // TODO: Implement me
        }

        public static Medication GetGeneric(long medicationId)
        {
            var medication = GetById(medicationId);

            if (medication.GenericId.HasValue)
            {
                medication = GetById(medication.GenericId.Value);
            }

            return medication;
        }

        public static string GetGenericName(long medicationId) 
            => GetDescription(medicationId, false);

        public static string GetDescription(long medicationId, bool includeGeneric = true)
        {
            var medication = GetById(medicationId);

            if (medication != null)
            {
                if (includeGeneric && medication.GenericId.HasValue)
                {
                    var generic = GetById(medication.GenericId.Value);
                    if (generic != null)
                    {
                        return $"{medication.Description} ({generic.Description})";
                    }
                }

                return medication.Description;
            }

            return "";
        }

        public static List<Medication> GetMultMedications(List<long> medicationIdList)
        {
            string searchCriteria = "";

            if (medicationIdList.Count > 0)
            {
                for (int i = 0; i < medicationIdList.Count; i++)
                {
                    if (i > 0)
                    {
                        searchCriteria += "OR ";
                    }
                    searchCriteria += "id = " + medicationIdList[i] + " ";
                }
                return SelectMany("SELECT * FROM medications WHERE " + searchCriteria, FromReader);
            }

            return new List<Medication>();
        }

        public static int CountPats(long medicationId)
        {
            // TODO: Fix me

            return DataConnection.ExecuteInt("SELECT COUNT(DISTINCT medicationpat.PatNum) FROM medicationpat WHERE medication_id = " + medicationId);
        }

        [Obsolete("Use MedicationPat.Refresh() instead.")]
        public static List<Medication> GetByPatient(long patientId)
        {
            // TODO: Fix me

            return 
                SelectMany("SELECT medication.* FROM medications, medicationpat WHERE medication.id = medicationpat.medication_id AND medicationpat.patient_id = @patient_id", FromReader, 
                    new MySqlParameter("patient_id", patientId));
        }

        public static List<long> GetAllInUseMedicationNums()
        {
            //If any more tables are added here in the future, then also update IsInUse() to include the new table.
            string command = "SELECT MedicationNum FROM medicationpat WHERE MedicationNum!=0 "
                + "UNION SELECT MedicationNum FROM allergydef WHERE MedicationNum!=0 "
                + "UNION SELECT MedicationNum FROM eduresource WHERE MedicationNum!=0 "
                + "GROUP BY MedicationNum";
            List<long> listMedicationNums = Db.GetListLong(command);
            if (Preference.GetLong(PreferenceName.MedicationsIndicateNone) != 0)
            {
                listMedicationNums.Add(Preference.GetLong(PreferenceName.MedicationsIndicateNone));
            }
            return listMedicationNums;
        }

        ///<summary>Returns an array of all patient names who are using this medication.</summary>
        public static string[] GetPatNamesForMed(long medicationNum)
        {
            string command =
                "SELECT CONCAT(CONCAT(CONCAT(CONCAT(LName,', '),FName),' '),Preferred) FROM medicationpat,patient "
                + "WHERE medicationpat.PatNum=patient.PatNum "
                + "AND medicationpat.MedicationNum=" + POut.Long(medicationNum);
            DataTable table = Db.GetTable(command);
            string[] retVal = new string[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                retVal[i] = PIn.String(table.Rows[i][0].ToString());
            }
            return retVal;
        }


        ///<summary>Medication merge tool.  Returns the number of rows changed.  Deletes the medication associated with medNumInto.</summary>
        public static long Merge(long medicationIdFrom, long medicationIdTo)
        {
            // TODO: Fix me

            long rowsAffected;

            var medicationIdFromParam = new MySqlParameter("prev_id", medicationIdFrom);
            var medicationIdToParam = new MySqlParameter("new_id", medicationIdTo);

            // Update all the foreign keys to point to the new medication.
            rowsAffected = DataConnection.ExecuteNonQuery("UPDATE allergydef SET medication_id = @new_id WHERE medication_id = @prev_id", medicationIdFromParam, medicationIdToParam);
            rowsAffected += DataConnection.ExecuteNonQuery("UPDATE eduresource SET medication_id = @new_id WHERE medication_id = @prev_id", medicationIdFromParam, medicationIdToParam);
            rowsAffected += DataConnection.ExecuteNonQuery("UPDATE medication SET generic_id = @new_id WHERE generic_id = @prev_id", medicationIdFromParam, medicationIdToParam);
            rowsAffected += DataConnection.ExecuteNonQuery("UPDATE medicationpat SET medication_id = @new_id WHERE medication_id = @prev_id", medicationIdFromParam, medicationIdToParam);
            rowsAffected += DataConnection.ExecuteNonQuery("UPDATE rxalert SET medication_id = @new_id WHERE medication_id = @prev_id", medicationIdFromParam, medicationIdToParam);

            // Get the RXCUI of the new medication.
            var rxCui = DataConnection.ExecuteString("SELECT rxcui FROM medications WHERE id = @new_id", medicationIdToParam);

            // Update the patient medications with the RXCU
            rowsAffected +=
                DataConnection.ExecuteNonQuery(
                    "UPDATE medicationpat SET rxcui = @rxcui WHERE medication_id = @new_id",
                    medicationIdToParam, new MySqlParameter("rxcui", rxCui));

            // Update the ERH triggers.
            var erhTriggerList = Crud.EhrTriggerCrud.SelectMany("SELECT * FROM ehrtrigger WHERE MedicationNumList LIKE '% " + POut.Long(medicationIdFrom) + " %'");
            foreach (var erhTrigger in erhTriggerList)
            {
                var medicationIds =
                    erhTrigger.MedicationNumList.Split(' ').Where(
                        medicationId =>
                            int.TryParse(medicationId, out var result) &&
                            result != medicationIdFrom &&
                            result != medicationIdTo)
                    .ToList();

                medicationIds.Add(medicationIdTo.ToString());

                erhTrigger.MedicationNumList = string.Join(" ", medicationIds.ToArray());

                EhrTriggers.Update(erhTrigger);

                rowsAffected++;
            }

            rowsAffected += DataConnection.ExecuteNonQuery("DELETE FROM medications WHERE id = " + medicationIdFrom);

            return rowsAffected;
        }

        public static void Refresh()
        {
            // TODO: Implement me
        }

        #endregion
    }
}