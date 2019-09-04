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
using System.Linq;

namespace OpenDentBusiness
{
    /// <summary>
    /// Other tables generally use the <see cref="Code"/> string as their foreign key.
    /// Currently synched to mobile server in a very inefficient manner.
    /// 
    /// It is implied that these are all ICD9CMs, although that may not be the case in the future.
    /// </summary>
    public class ICD9 : DataRecord
    {
        /// <summary>
        /// The ICD9 code.
        /// </summary>
        public string Code;

        /// <summary>
        /// A description of the ICD9 code.
        /// </summary>
        public string Description;

        /// <summary>
        /// The date on which the ICD9 code was last modified.
        /// </summary>
        public DateTime LastModified;

        /// <summary>
        /// Constructs a new instance of the <see cref="ICD9"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="ICD9"/> instance.</returns>
        static ICD9 FromReader(MySqlDataReader dataReader)
        {
            return new ICD9
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Code = Convert.ToString(dataReader["code"]),
                Description = Convert.ToString(dataReader["description"]),
                LastModified = Convert.ToDateTime(dataReader["last_modified"])
            };
        }

        /// <summary>
        /// Gets a list of all ICD9 codes.
        /// </summary>
        /// <returns>A list of ICD9 codes.</returns>
        public static List<ICD9> All() => 
            SelectMany("SELECT * FROM `icd9`", FromReader);

        /// <summary>
        /// Gets the ICD9 code with the specified ID.
        /// </summary>
        /// <param name="icd9id">The ID of the specified ICD9 code.</param>
        /// <returns>The ICD9 code with the specified ID.</returns>
        public static ICD9 GetById(long icd9id) => 
            SelectOne("SELECT * FROM `icd9` WHERE `id` = " + icd9id, FromReader);

        /// <summary>
        /// Gets the ICD9 with the specified code.
        /// </summary>
        /// <param name="icd9Code"></param>
        /// <returns></returns>
        public static ICD9 GetByCode(string icd9Code) =>
            SelectOne("SELECT * FROM `icd9` WHERE `code` = ?code", FromReader,
                new MySqlParameter("code", icd9Code));

        /// <summary>
        /// Searches for all ICD9 codes matching the specified search text.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns>A list of ICD9 codes.</returns>
        public static List<ICD9> Find(string searchText) =>
            SelectMany("SELECT * FROM `icd9` WHERE `code` LIKE ?search_text OR `description` LIKE ?search_text", FromReader,
                new MySqlParameter("search_text", $"%{searchText}%"));

        /// <summary>
        /// Gets the total number of ICD9 codes.
        /// </summary>
        /// <returns>The number of ICD9 codes.</returns>
        public static long GetCount() =>
            DataConnection.ExecuteLong("SELECT COUNT(*) FROM `icd9`");

        /// <summary>
        /// Checks whether the specified ICD9 code exists in the database.
        /// </summary>
        /// <param name="icd9Code">The ICD9 code.</param>
        /// <returns>True if the code exists in the database; otherwise, false.</returns>
        public static bool CodeExists(string icd9Code) =>
            DataConnection.ExecuteLong(
                "SELECT COUNT(*) FROM `icd9` WHERE `code` = @code", 
                    new MySqlParameter("code", icd9Code ?? "")) > 0;

        /// <summary>
        /// Gets a list of ID's of all ICD9 codes that have been modified since the specified date.
        /// </summary>
        /// <param name="changedSince"></param>
        /// <returns></returns>
        public static List<long> GetChangedSince(DateTime changedSince) =>
            SelectMany(
                "SELECT `id` FROM `icd9` WHERE `last_modified` > ?changed_since AND `code` IN (SELECT `icd9_code` FROM `diseases`)",
                    dataReader => Convert.ToInt64(dataReader[0]),
                        new MySqlParameter("changed_since", changedSince));

        /// <summary>
        /// Inserts the specified ICD9 code into the database.
        /// </summary>
        /// <param name="icd9">The ICD9 code.</param>
        /// <returns>The ID assigned to the ICD9 code.</returns>
        public static long Insert(ICD9 icd9) =>
            icd9.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `icd9` (`code`, `description`) VALUES (?code, ?description) RETURNING id",
                    new MySqlParameter("code", icd9.Code ?? ""),
                    new MySqlParameter("description", icd9.Description ?? ""));

        /// <summary>
        /// Updates the specified ICD9 code in the database.
        /// </summary>
        /// <param name="icd9">The ICD9 code.</param>
        public static void Update(ICD9 icd9) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `icd9` SET `code` = ?code, `description` = ?description WHERE `id` = ?id",
                    new MySqlParameter("code", icd9.Code ?? ""),
                    new MySqlParameter("description", icd9.Description ?? ""),
                    new MySqlParameter("id", icd9.Id));

        /// <summary>
        /// Deletes the ICD9 code with the specified ID from the database.
        /// </summary>
        /// <param name="icd9id">The ID of the ICD9 code.</param>
        public static void Delete(long icd9id)
        {
            // TODO: Optimize this.

            var dataTable = 
                DataConnection.ExecuteDataTable(
                    "SELECT lastname, firstname, patient.id " +
                    "FROM patients, patient_diseases, icd9" +
                    "WHERE patient.id = patient_diseases.patient_id " +
                    "AND patient_diseases.disease_id = diseases.id " +
                    "AND diseases.icd9_code = icd9.code " +
                    "AND icd9.id = @id " +
                    "GROUP BY patient.id", 
                        new MySqlParameter("id", icd9id));

            if (dataTable.Rows.Count > 0)
            {
                throw new ApplicationException($"Not allowed to delete. Already in use by {dataTable.Rows.Count} patients");
            }

            DataConnection.ExecuteNonQuery("DELETE FROM `icd9` WHERE `id` = " + icd9id);

            dataTable.Dispose();
        }

        #region CLEANUP

        /// <summary>
        /// Used along with GetChangedSinceICD9Nums
        /// </summary>
        public static List<ICD9> GetMultICD9s(List<int> icd9IdList)
        {
            string searchCriteria = "";

            if (icd9IdList.Count > 0)
            {
                for (int i = 0; i < icd9IdList.Count; i++)
                {
                    if (i > 0)
                    {
                        searchCriteria += "OR ";
                    }
                    searchCriteria += "id = " + icd9IdList[i] + " ";
                }

                return SelectMany("SELECT * FROM icd9 WHERE " + searchCriteria, FromReader);
            }

            return new List<ICD9>();
        }

        /// <summary>
        /// Returns the code and description of the icd9.
        /// </summary>
        public static string GetCodeAndDescription(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return "";
            }

            var icd9 = ICD9.GetByCode(code);

            return icd9 == null ? "" : $"{icd9.Code} - {icd9.Description}";
        }

        /// <summary>
        /// Returns true if any of the procs have a ICD9 code.
        /// </summary>
        public static bool HasICD9Codes(List<Procedure> listProcs)
        {
            //No need to check RemotingRole; no call to db.
            List<string> listIcd9Codes = new List<string>();
            List<Procedure> listIcd9Procs = listProcs.FindAll(x => x.IcdVersion == 9);
            listIcd9Codes.AddRange(listIcd9Procs.Where(x => !string.IsNullOrEmpty(x.DiagnosticCode)).Select(x => x.DiagnosticCode));
            listIcd9Codes.AddRange(listIcd9Procs.Where(x => !string.IsNullOrEmpty(x.DiagnosticCode2)).Select(x => x.DiagnosticCode2));
            listIcd9Codes.AddRange(listIcd9Procs.Where(x => !string.IsNullOrEmpty(x.DiagnosticCode3)).Select(x => x.DiagnosticCode3));
            listIcd9Codes.AddRange(listIcd9Procs.Where(x => !string.IsNullOrEmpty(x.DiagnosticCode4)).Select(x => x.DiagnosticCode4));
            if (listIcd9Codes.Count != 0)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}