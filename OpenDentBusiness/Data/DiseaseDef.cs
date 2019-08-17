using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    public class DiseaseDef : DataRecord
    {
        /// <summary>
        /// The name of the disease.
        /// </summary>
        public string Name;

        /// <summary>
        /// The sort order of the disease.
        /// </summary>
        public int SortOrder;

        /// <summary>
        /// A value indicating whether the disease has been hidden.
        /// </summary>
        public bool Hidden;

        /// <summary>
        /// The ICD9 code of the disease.
        /// </summary>
        public string ICD9Code;

        /// <summary>
        /// The ICD10 code of the disease.
        /// </summary>
        public string ICD10Code;

        /// <summary>
        /// The SNOMED code of the disease.
        /// </summary>
        public string SnomedCode;

        [Obsolete] public bool IsNew;

        /// <summary>
        /// Constructs a new instance of the <see cref="DiseaseDef"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="DiseaseDef"/> instance.</returns>
        static DiseaseDef FromReader(MySqlDataReader dataReader)
        {
            return new DiseaseDef
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Name = Convert.ToString(dataReader["name"]),
                SortOrder = Convert.ToInt32(dataReader["sort_order"]),
                Hidden = Convert.ToBoolean(dataReader["hidden"]),
                ICD9Code = Convert.ToString(dataReader["icd9_code"]),
                ICD10Code = Convert.ToString(dataReader["icd10_code"]),
                SnomedCode = Convert.ToString(dataReader["snomed_code"])
            };
        }

        /// <summary>
        /// Gets the total number of diseases.
        /// </summary>
        /// <returns>The number of diseases.</returns>
        public static long GetCount() => 
            DataConnection.ExecuteLong("SELECT COUNT(*) FROM diseases WHERE hidden = 0");

        /// <summary>
        /// Gets a list of all diseases.
        /// </summary>
        /// <param name="excludeHidden">A value indicating whether the exclude diseases.</param>
        /// <returns>A list of diseases.</returns>
        public static List<DiseaseDef> All(bool excludeHidden = true) =>
            SelectMany("SELECT * FROM diseases ORDER BY sort_order WHERE hidden = 0 OR @show_hidden", FromReader,
                new MySqlParameter("show_hidden", !excludeHidden));

        /// <summary>
        /// Gets the disease with the specified ID.
        /// </summary>
        /// <param name="diseaseId">The ID of the disease.</param>
        /// <returns>The disease with the specified ID.</returns>
        public static DiseaseDef GetById(long diseaseId) =>
            SelectOne("SELECT * FROM diseases WHERE id = " + diseaseId, FromReader);

        /// <summary>
        /// Gets the disease with the specified name.
        /// </summary>
        /// <param name="diseaseName">The name of the disease.</param>
        /// <returns>The disease with the specified name.</returns>
        public static DiseaseDef GetByName(string diseaseName) =>
            SelectOne(
                "SELECT * FROM diseases WHERE name = @name AND hidden = 0", FromReader,
                    new MySqlParameter("name", diseaseName));

        /// <summary>
        /// Insers the specified disease into the database.
        /// </summary>
        /// <param name="disease">The disease.</param>
        /// <returns>The ID assigned to the disease.</returns>
        public static long Insert(DiseaseDef disease) =>
            disease.Id = DataConnection.ExecuteInsert(
                "INSERT INTO diseases (name, sort_order, hidden, icd9_code, icd10_code, snomed_code) VALUES (@name, @sort_order, @hidden, @icd9_code, @icd10_code, @snomed_code)",
                    new MySqlParameter("name", disease.Name),
                    new MySqlParameter("sort_order", disease.SortOrder),
                    new MySqlParameter("hidden", disease.Hidden),
                    new MySqlParameter("icd9_code", disease.ICD9Code ?? ""),
                    new MySqlParameter("icd10_code", disease.ICD10Code ?? ""),
                    new MySqlParameter("snomed_code", disease.SnomedCode ?? ""));

        /// <summary>
        /// Updates the specified disease in the database.
        /// </summary>
        /// <param name="disease">The disease.</param>
        public static void Update(DiseaseDef disease) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE diseases SET name = @name, sort_order = @sort_order, hidden = @hidden, icd9_code = @icd9_code, icd10_code = @icd10_code, snomed_code = @snomed_code WHERE id = @id",
                    new MySqlParameter("name", disease.Name),
                    new MySqlParameter("sort_order", disease.SortOrder),
                    new MySqlParameter("hidden", disease.Hidden),
                    new MySqlParameter("icd9_code", disease.ICD9Code ?? ""),
                    new MySqlParameter("icd10_code", disease.ICD10Code ?? ""),
                    new MySqlParameter("snomed_code", disease.SnomedCode ?? ""),
                    new MySqlParameter("id", disease.Id));

        /// <summary>
        /// Deletes the disease with the specified ID from the database.
        /// </summary>
        /// <param name="diseaseId">The ID of the disease.</param>
        public static void Delete(long diseaseId) => 
            DataConnection.ExecuteNonQuery("DELETE FROM diseases WHERE id = " + diseaseId);

        public static int SortItemOrder(DiseaseDef x, DiseaseDef y)
        {
            if (x.SortOrder != y.SortOrder)
            {
                return x.SortOrder.CompareTo(y.SortOrder);
            }
            return x.Id.CompareTo(y.Id);
        }

        /// <summary>
        /// Fix the sort orders of all the diseases.
        /// </summary>
        /// <returns>True if the sort orders where fixed; otherwise, false.</returns>
        public static bool FixSortOrders()
        {
            var diseaseList = SelectMany("SELECT * FROM diseases", FromReader);
            diseaseList.Sort((lhs, rhs) =>
            {
                if (lhs.SortOrder != rhs.SortOrder)
                {
                    return lhs.SortOrder.CompareTo(rhs.SortOrder);
                }
                return lhs.Id.CompareTo(rhs.SortOrder);
            });

            bool updated = false;

            for (int i = 0; i < diseaseList.Count; i++)
            {
                if (diseaseList[i].SortOrder != i)
                {
                    diseaseList[i].SortOrder = i;

                    Update(diseaseList[i]);

                    updated = true;
                }
            }

            return updated;
        }

        /// <summary>
        /// Returns a list of valid diseasedefnums to delete from the passed in list.
        /// </summary>
        public static List<long> ValidateDeleteList(List<long> diseaseIdList)
        {
            List<long> inUseDiseaseIdList = new List<long>();
            if (diseaseIdList == null || diseaseIdList.Count < 1)
            {
                return inUseDiseaseIdList;
            }

            // Diseases in use by preferences.
            if (diseaseIdList.Contains(Preference.GetLong(PreferenceName.ProblemsIndicateNone)))
            {
                inUseDiseaseIdList.Add(Preference.GetLong(PreferenceName.ProblemsIndicateNone));
            }

            DataConnection.Execute(connection =>
            {
                // Diseases attached to patients
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT DISTINCT disease.id " +
                        "FROM patients, patient_diseases " +
                        "WHERE patients.id = patient_diseases.patient_id " +
                        "AND patient_diseases.disease_id IN (" + string.Join(",", diseaseIdList) + ")";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inUseDiseaseIdList.Add(Convert.ToInt32(reader[0]));
                        }
                    }
                }

                // Diseases attached to edu resources
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT DISTINCT eduresources.disease_id " +
                        "FROM eduresources " +
                        "WHERE eduresources.disease_id IN (" + string.Join(",", diseaseIdList) + ")";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inUseDiseaseIdList.Add(Convert.ToInt32(reader[0]));
                        }
                    }
                }

                // Diseases attached to family health
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT DISTINCT familyhealth.disease_id " +
                        "FROM patients, familyhealth " +
                        "WHERE patients.id = familyhealth.patient_id " +
                        "AND familyhealth.disease_id IN (" + string.Join(",", diseaseIdList) + ")";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inUseDiseaseIdList.Add(Convert.ToInt32(reader[0]));
                        }
                    }
                }
            });

            return inUseDiseaseIdList;
        }

        public static string GetName(long diseaseId) => 
            DiseaseDef.GetById(diseaseId)?.Name ?? "";

        /// <summary>
        /// Returns the name of the disease based on SNOMEDCode, then if no match tries ICD9Code, then if no match returns empty string. 
        /// Used in EHR Patient Lists.
        /// </summary>
        public static string GetNameByCode(string SNOMEDorICD9Code)
        {
            var disease = SelectOne(
                "SELECT * FROM diseases WHERE snomed_code = @code", FromReader,
                    new MySqlParameter("code", SNOMEDorICD9Code));

            if (disease != null) return disease.Name;

            disease = SelectOne(
                "SELECT * FROM diseases WHERE icd9_code = @code", FromReader,
                    new MySqlParameter("code", SNOMEDorICD9Code));

            return disease == null ? "" : disease.Name;
        }

        /// <summary>
        /// Returns the DiseaseDefNum based on SNOMEDCode, then if no match tries ICD9Code, then if no match tries ICD10Code, then if no 
        /// match returns 0. Used in EHR Patient Lists and when automatically inserting pregnancy Dx from FormVitalsignEdit2014.
        /// Will match hidden diseases.
        /// </summary>
        public static long GetNumFromCode(string code)
        {
            var disease = SelectOne(
                "SELECT * FROM diseases WHERE snomed_code = @code", FromReader,
                    new MySqlParameter("code", code));

            if (disease != null) return disease.Id;

            disease = SelectOne(
                "SELECT * FROM diseases WHERE icd9_code = @code", FromReader,
                    new MySqlParameter("code", code));

            if (disease != null) return disease.Id;

            disease = SelectOne(
                "SELECT * FROM diseases WHERE icd10_code = @code", FromReader,
                    new MySqlParameter("code", code));

            return disease == null ? 0 : disease.Id;
        }

        /// <summary>
        /// Returns the DiseaseDefNum based on SNOMEDCode.
        /// If no match or if SnomedCode is an empty string returns 0. 
        /// Only matches SNOMEDCode, not ICD9 or ICD10.
        /// </summary>
        public static long GetNumFromSnomed(string SnomedCode)
        {
            var disease = SelectOne(
                "SELECT * FROM diseases WHERE snomed_code = @code", FromReader,
                    new MySqlParameter("code", SnomedCode));

            return disease == null ? 0 : disease.Id;
        }

        /// <summary>
        /// Returns the diseaseDefNum that exactly matches the specified string. 
        /// Used in import functions when you only have the name to work with. 
        /// Can return 0 if no match. Does not match hidden diseases.
        /// </summary>
        public static long GetNumFromName(string diseaseName) => GetNumFromName(diseaseName, false);

        /// <summary>
        /// Returns the diseaseDefNum that exactly matches the specified string. Will return 0 if no match.
        /// Set matchHidden to true to match hidden diseasedefs as well.
        /// </summary>
        public static long GetNumFromName(string diseaseName, bool matchHidden)
        {
            var disease = SelectOne(
                "SELECT * FROM diseases WHERE name = @name AND (hidden = 0 OR @match_hidden)", FromReader,
                    new MySqlParameter("name", diseaseName),
                    new MySqlParameter("match_hidden", matchHidden));

            return disease == null ? 0 : disease.Id;
        }

        #region CLEANUP

        ///<summary>Used along with GetChangedSinceDiseaseDefNums</summary>
        public static List<DiseaseDef> GetMultDiseaseDefs(List<long> diseaseDefNums)
        {
            // TODO: Fix me

            //string strDiseaseDefNums = "";
            //DataTable table;
            //if (diseaseDefNums.Count > 0)
            //{
            //    for (int i = 0; i < diseaseDefNums.Count; i++)
            //    {
            //        if (i > 0)
            //        {
            //            strDiseaseDefNums += "OR ";
            //        }
            //        strDiseaseDefNums += "DiseaseDefNum='" + diseaseDefNums[i].ToString() + "' ";
            //    }
            //    string command = "SELECT * FROM diseasedef WHERE " + strDiseaseDefNums;
            //    table = Db.GetTable(command);
            //}
            //else
            //{
            //    table = new DataTable();
            //}
            //DiseaseDef[] multDiseaseDefs = DiseaseDef.TableToList(table).ToArray();
            //List<DiseaseDef> diseaseDefList = new List<DiseaseDef>(multDiseaseDefs);
            //return diseaseDefList;

            return new List<DiseaseDef>();
        }

        public static bool ContainsSnomed(string snomedCode, long excludeDefNum) =>
            DataConnection.ExecuteLong(
                "SELECT * FROM diseases WHERE snomed_code = @snomed_code AND id != @exclude_id AND hidden = 0",
                    new MySqlParameter("snomed_code", snomedCode),
                    new MySqlParameter("exclude_id", excludeDefNum)) > 0;


        public static bool ContainsICD9(string icd9Code, long excludeDefNum) =>
            DataConnection.ExecuteLong(
                "SELECT * FROM diseases WHERE icd9_code = @icd9_code AND id != @exclude_id AND hidden = 0",
                    new MySqlParameter("icd9_code", icd9Code),
                    new MySqlParameter("exclude_id", excludeDefNum)) > 0;

        public static bool ContainsICD10(string icd10Code, long excludeDefNum) =>
            DataConnection.ExecuteLong(
                "SELECT * FROM diseases WHERE icd10_code = @icd10_code AND id != @exclude_id AND hidden = 0",
                    new MySqlParameter("icd10_code", icd10Code),
                    new MySqlParameter("exclude_id", excludeDefNum)) > 0;

        /// <summary>
        /// Get all diseasedefs that have a pregnancy code that applies to the three CQM measures with pregnancy as an exclusion condition.
        /// </summary>
        public static List<DiseaseDef> GetAllPregDiseaseDefs()
        {
            Dictionary<string, string> pregCodeList =
                EhrCodes.GetCodesExistingInAllSets(
                    new List<string> {
                        "2.16.840.1.113883.3.600.1.1623",
                        "2.16.840.1.113883.3.526.3.378" });

            var diseaseList = SelectMany("SELECT * FROM diseases WHERE hidden = 0 ORDER BY sort_order", FromReader);

            var results = new List<DiseaseDef>();
            foreach (var disease in diseaseList)
            {
                if (pregCodeList.ContainsKey(disease.ICD9Code) && pregCodeList[disease.ICD9Code] == "ICD9CM")
                    results.Add(disease);

                else if (pregCodeList.ContainsKey(disease.ICD10Code) && pregCodeList[disease.ICD10Code] == "ICD10CM")
                    results.Add(disease);

                else if (pregCodeList.ContainsKey(disease.SnomedCode) && pregCodeList[disease.SnomedCode] == "SNOMEDCT")
                    results.Add(disease);
            }

            return results;
        }

        #endregion
    }
}