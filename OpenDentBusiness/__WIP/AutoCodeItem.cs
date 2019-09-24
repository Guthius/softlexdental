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
using System.Collections.Generic;
using System.Linq;

namespace OpenDentBusiness
{
    /// <summary>
    /// Corresponds to the autocodeitem table in the database.
    /// There are multiple AutoCodeItems for a given AutoCode.
    /// Each Item has one ADA code.
    /// </summary>
    public class AutoCodeItem : DataRecord
    {
        private static readonly DataRecordCache<AutoCodeItem> cache =
            new DataRecordCache<AutoCodeItem>("SELECT * FROM `auto_code_items`", FromReader);

        public long AutoCodeId;
        public long ProcedureCodeId;

        /// <summary>
        /// Only used in the validation section when closing FormAutoCodeEdit.
        /// Will normally be empty.
        /// </summary>
        public List<AutoCodeCondition> Conditions;

        /// <summary>
        /// Constructs a new instance of the <see cref="AutoCodeItem"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="AutoCodeItem"/> instance.</returns>
        private static AutoCodeItem FromReader(MySqlDataReader dataReader)
        {
            return new AutoCodeItem
            {
                Id = (long)dataReader["id"],
                AutoCodeId = (long)dataReader["auto_code_id"],
                ProcedureCodeId = (long)dataReader["procedure_code_id"]
            };
        }

        public static AutoCodeItem GetByProcedureCode(long procedureCodeId) =>
            cache.SelectOne(autoCodeItem => autoCodeItem.ProcedureCodeId == procedureCodeId);

        /// <summary>
        /// Inserts the specified auto code item into the database.
        /// </summary>
        /// <param name="autoCodeItem">The auto code item.</param>
        /// <returns>The ID assigned to the auto code item.</returns>
        public static long Insert(AutoCodeItem autoCodeItem) =>
            autoCodeItem.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `auto_code_items` (`auto_code_id`, `procedure_code_id`) VALUES (?auto_code_id, ?procedure_code_id)",
                    new MySqlParameter("auto_code_id", autoCodeItem.AutoCodeId),
                    new MySqlParameter("procedure_code_id", autoCodeItem.ProcedureCodeId));

        /// <summary>
        /// Updates the specified auto code item in the database.
        /// </summary>
        /// <param name="autoCodeItem">The auto code item.</param>
        public static void Update(AutoCodeItem autoCodeItem) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `auto_code_items` SET `auto_code_id` = ?auto_code_id, `procedure_code_id` = ?procedure_code_id WHERE `id` = ?id",
                    new MySqlParameter("auto_code_id", autoCodeItem.AutoCodeId),
                    new MySqlParameter("procedure_code_id", autoCodeItem.ProcedureCodeId),
                    new MySqlParameter("id", autoCodeItem.Id));

        /// <summary>
        /// Deletes the specified auto code item.
        /// </summary>
        /// <param name="autoCodeItem">The auto code item.</param>
        public static void Delete(AutoCodeItem autoCodeItem) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `auto_code_items` WHERE `id` = " + autoCodeItem.Id);

        /// <summary>
        /// Gets a list of auto code items assigned to the specified auto code.
        /// </summary>
        /// <param name="autoCodeId">The ID of the auto code.</param>
        /// <returns>A list of auto code items.</returns>
        public static List<AutoCodeItem> GetByAutoCode(long autoCodeId) =>
            cache.Where(autoCodeItem => autoCodeItem.AutoCodeId == autoCodeId).ToList();

        public static long GetCodeNum(long autoCodeId, string toothNumber, string surface, bool isAdditional, long patientId, int age, bool willBeMissing)
        {
            var autoCodeItems = GetByAutoCode(autoCodeId);
            if (autoCodeItems.Count == 0)
                return 0;
            

            foreach (var autoCodeItem in autoCodeItems)
            {
                var autoCodeConditions = AutoCodeCondition.GetByAutoCodeItem(autoCodeItem.Id);

                var conditionsMet = true;
                foreach (var autoCodeCondition in autoCodeConditions)
                {
                    if (!AutoCodeCondition.CheckCondition(autoCodeCondition.Condition, toothNumber, surface, isAdditional, willBeMissing, age))
                    {
                        conditionsMet = false;

                        break;
                    }
                }

                if (conditionsMet) return autoCodeItem.ProcedureCodeId;
            }

            return autoCodeItems[0].ProcedureCodeId;
        }

        public static long VerifyCode(long procedureCodeId, string toothNumber, string surface, bool isAdditional, long patientId, int age, out AutoCode autoCode)
        {
            // TODO: Rework this??

            autoCode = null;

            var tempAutoCodeItem = cache.FirstOrDefault(x => x.ProcedureCodeId == procedureCodeId);
            if (tempAutoCodeItem == null)
                return procedureCodeId;
            
            autoCode = AutoCode.GetById(tempAutoCodeItem.AutoCodeId);
            if (autoCode == null || autoCode.LessIntrusive)
                return procedureCodeId;

            bool willBeMissing = Procedures.WillBeMissing(toothNumber, patientId);

            var autoCodeItems = GetByAutoCode(tempAutoCodeItem.AutoCodeId);
            foreach (var autoCodeItem in autoCodeItems)
            {
                var autoCodeConditions = AutoCodeCondition.GetByAutoCodeItem(autoCodeItem.Id);

                var conditionsMet = true;
                foreach (var autoCodeCondition in autoCodeConditions)
                {
                    if (!AutoCodeCondition.CheckCondition(autoCodeCondition.Condition, toothNumber, surface, isAdditional, willBeMissing, age))
                    {
                        conditionsMet = false;

                        break;
                    }
                }

                if (conditionsMet) return autoCodeItem.ProcedureCodeId;
            }

            return procedureCodeId;
        }

        /// <summary>
        /// Checks inputs and determines if user should be prompted to pick a more applicable procedure code.
        /// </summary>
        /// <param name="verifyCode">This is the recommended code based on input. If it matches procCode return value will be false.</param>
        public static bool ShouldPromptForCodeChange(Procedure procedure, ProcedureCode procedureCode, Patient patient, bool isMandibular, List<ClaimProc> claimProcsForProc, out long verifyCode)
        {
            verifyCode = procedure.CodeNum;

            if (procedureCode.TreatArea == TreatmentArea.Mouth || 
                procedureCode.TreatArea == TreatmentArea.Quad || 
                procedureCode.TreatArea == TreatmentArea.Sextant || 
                Procedures.IsAttachedToClaim(procedure, claimProcsForProc))
            {
                return false;
            }

            if (procedureCode.TreatArea == TreatmentArea.Arch)
            {
                if (string.IsNullOrEmpty(procedure.Surf)) return false;

                verifyCode =
                    procedure.Surf == "U" ?
                        VerifyCode(procedureCode.CodeNum, "1", "", procedure.IsAdditional, patient.PatNum, patient.Age, out _) ://max
                        VerifyCode(procedureCode.CodeNum, "32", "", procedure.IsAdditional, patient.PatNum, patient.Age, out _);//mand
            }
            else if (procedureCode.TreatArea == TreatmentArea.ToothRange)
            {
                verifyCode = VerifyCode(
                    procedureCode.CodeNum, 
                    isMandibular ? "32" : "1", 
                    "",
                    procedure.IsAdditional, 
                    patient.PatNum, 
                    patient.Age, 
                    out _);
            }
            else
            {
                verifyCode = VerifyCode(
                    procedureCode.CodeNum, 
                    procedure.ToothNum,
                    Tooth.SurfTidyForClaims(procedure.Surf, procedure.ToothNum), 
                    procedure.IsAdditional, 
                    patient.PatNum, 
                    patient.Age, 
                    out _);
            }

            return procedureCode.CodeNum != verifyCode;
        }
    }
}
