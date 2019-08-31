/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    /// Other tables generally use the <see cref="Code"/> string as their foreign key.
    /// </summary>
    public class ICD10 : DataRecord
    {
        /// <summary>
        /// ICD-10-CM or ICD-10-PCS code. Dots are included. 
        /// </summary>
        /// <remarks>Not allowed to edit this column once saved in the database.</remarks>
        public string Code;
        
        /// <summary>
        /// Short Description provided by ICD10 documentation.
        /// </summary>
        public string Description;

        /// <summary>
        /// 0 if the code is a “header” – not valid for submission on a UB04. 1 if the code is valid for submission on a UB04.
        /// 
        /// True if the code is a “header” – not valid for submission on a UB04. False if the code is valid for submission on a UB04.
        /// </summary>
        [Obsolete] public bool Header;

        /// <summary>
        /// Constructs a new instance of the <see cref="ICD10"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="ICD10"/> instance.</returns>
        static ICD10 FromReader(MySqlDataReader dataReader)
        {
            return new ICD10
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Code = Convert.ToString(dataReader["code"]),
                Description = Convert.ToString(dataReader["description"]),
                Header = Convert.ToBoolean(dataReader["header"])
            };
        }
        
        /// <summary>
        /// Gets a list of all ICD10 codes.
        /// </summary>
        /// <returns>A list of ICD10 codes.</returns>
        public static List<ICD10> All() => 
            SelectMany("SELECT * FROM `icd10`", FromReader);

        /// <summary>
        /// Gets the ICD10 code with the specified ID.
        /// </summary>
        /// <param name="icd10Id">The ID of the ICD10 code.</param>
        /// <returns>The ICD10 code with the specified ID.</returns>
        public static ICD10 GetById(long icd10Id) =>
            SelectOne("SELECT * FROM `icd10` WHERE `id` = " + icd10Id, FromReader);

        /// <summary>
        /// Gets the ICD10 code with the specified code.
        /// </summary>
        /// <param name="icd10Code">The ICD10 code.</param>
        /// <returns>The ICD10 code with the specified code.</returns>
        public static ICD10 GetByCode(string icd10Code) =>
            SelectOne("SELECT * FROM `icd10` WHERE `code` = ?code", FromReader,
               new MySqlParameter("code", icd10Code));

        /// <summary>
        /// Gets the total number of ICD10 codes.
        /// </summary>
        /// <returns>The number of ICD10 codes.</returns>
        public static long GetCount() =>
            DataConnection.ExecuteLong("SELECT COUNT(*) FROM `icd10` WHERE `header` = 0");

        /// <summary>
        /// Gets the ICD10 codes in the specified list.
        /// </summary>
        /// <param name="icd10CodeList">The list of ICD10 codes.</param>
        /// <returns>A list of ICD10 codes.</returns>
        public static List<ICD10> GetByCodes(List<string> icd10CodeList) =>
            SelectMany(
                "SELECT * FROM `icd10` WHERE code = ANY(:codes)", FromReader, 
                    new MySqlParameter("codes", icd10CodeList));

        /// <summary>
        /// Searches the ICD10 codes for codes matching the specified search text.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns>A list of ICD10 codes matching the search text.</returns>
        public static List<ICD10> Find(string searchText) =>
            SelectMany(
                "SELECT * FROM `icd10` WHERE `code` LIKE ?search_text OR `description` LIKE ?search_text", FromReader,
                    new MySqlParameter("search_text", $"%{searchText}%"));

        /// <summary>
        /// Checks whether the specified ICD10 code exists in the database.
        /// </summary>
        /// <param name="icd10Code">The ICD10 code.</param>
        /// <returns>True if the code exists in the database; otherwise, false.</returns>
        public static bool CodeExists(string icd10Code) =>
            DataConnection.ExecuteLong(
                "SELECT COUNT(*) FROM `icd10` WHERE `code` = ?code", 
                    new MySqlParameter("code", icd10Code)) > 0;

        /// <summary>
        /// Inserts the specified ICD10 code into the database.
        /// </summary>
        /// <param name="icd10">The ICD10 code.</param>
        /// <returns></returns>
        public static long Insert(ICD10 icd10) =>
            icd10.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `icd10` (`code`, `description`, `header`) VALUES (?code, ?description, ?header)",
                    new MySqlParameter("code", icd10.Code ?? ""),
                    new MySqlParameter("description", icd10.Description ?? ""),
                    new MySqlParameter("header", icd10.Header));

        /// <summary>
        /// Updates the specified ICD10 code in the database.
        /// </summary>
        /// <param name="icd10">The ICD10 code.</param>
        public static void Update(ICD10 icd10) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `icd10` SET `code` = ?code, `description` = ?description, `header` = ?header WHERE `id` =  ?id",
                    new MySqlParameter("code", icd10.Code ?? ""),
                    new MySqlParameter("description", icd10.Description ?? ""),
                    new MySqlParameter("header", icd10.Header),
                    new MySqlParameter("id", icd10.Id));

        /// <summary>
        /// Deletes the ICD10 code with the specified ID from the database.
        /// </summary>
        /// <param name="icd10Id">The ID of the ICD10 code.</param>
        public static void Delete(long icd10Id) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `icd10` WHERE `id` = " + icd10Id);

        #region CLEANUP

        public static string GetCodeAndDescription(string icd10Code)
        {
            if (string.IsNullOrEmpty(icd10Code))
            {
                var icd10 = GetByCode(icd10Code);
                if (icd10 != null)
                {
                    return $"{icd10.Code} - {icd10.Description}";
                }
            }

            return string.Empty;
        }

        //public static List<ICD10> GetBySearchText(string searchText)
        //{
        //    string[] searchTokens = searchText.Split(' ');
        //    string command = @"SELECT * FROM icd10 ";
        //    for (int i = 0; i < searchTokens.Length; i++)
        //    {
        //        command += (i == 0 ? "WHERE " : "AND ") + "(Icd10Code LIKE '%" + POut.String(searchTokens[i]) + "%' OR Description LIKE '%" + POut.String(searchTokens[i]) + "%') ";
        //    }
        //    return Crud.Icd10Crud.SelectMany(command);
        //}

        #endregion
    }
}