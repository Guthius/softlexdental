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
using System.Globalization;
using System.Linq;

namespace OpenDentBusiness
{
    /// <summary>
    /// An autocode automates entering procedures.
    /// The user only has to pick composite, for instance, and the autocode
    /// figures out the code based on the number of surfaces, and posterior vs.
    /// anterior.  Autocodes also enforce and suggest changes to a procedure
    /// code if the number of surfaces or other properties change.
    /// </summary>
    public class AutoCode : DataRecord
    {
        private static readonly DataRecordCache<AutoCode> cache = 
            new DataRecordCache<AutoCode>("SELECT * FROM `auto_codes`", FromReader);

        /// <summary>
        /// Displays meaningful decription, like "Amalgam".
        /// </summary>
        public string Description;

        /// <summary>
        /// User can hide autocodes
        /// </summary>
        public bool Hidden;
        
        /// <summary>
        /// This will be true if user no longer wants to see this autocode
        /// message when closing a procedure. This makes it less intrusive, but
        /// it can still be used in procedure buttons.
        /// </summary>
        public bool LessIntrusive;

        /// <summary>
        /// Returns a string representation of the auto code.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Hidden)
            {
                return Description + " (hidden)";
            }
            return Description;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="AutoCode"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="AutoCode"/> instance.</returns>
        private static AutoCode FromReader(MySqlDataReader dataReader)
        {
            return new AutoCode
            {
                Id = (long)dataReader["id"],
                Description = (string)dataReader["description"],
                Hidden = Convert.ToBoolean(dataReader["hidden"]),
                LessIntrusive = Convert.ToBoolean(dataReader["less_intrusive"])
            };
        }

        /// <summary>
        /// Gets a list of all auto codes.
        /// </summary>
        /// <returns>A list of auto codes.</returns>
        public static List<AutoCode> All() =>
            cache.All().ToList();

        /// <summary>
        /// Gets the auto code with the specified ID.
        /// </summary>
        /// <param name="autoCodeId">The ID of the auto code.</param>
        /// <returns>The auto code with the specified ID.</returns>
        public static AutoCode GetById(long autoCodeId) =>
            cache.FirstOrDefault(autoCode => autoCode.Id == autoCodeId);

        /// <summary>
        /// Inserts the specified auto code into the database.
        /// </summary>
        /// <param name="autoCode">The auto code.</param>
        /// <returns>The ID assigned to the auto code.</returns>
        public static long Insert(AutoCode autoCode) =>
            autoCode.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `auto_codes` (`description`, `hidden`, `less_intrusive`) VALUES (?description, ?hidden, ?less_intrusive)",
                    new MySqlParameter("description", autoCode.Description ?? ""),
                    new MySqlParameter("hidden", autoCode.Hidden),
                    new MySqlParameter("less_intrusive", autoCode.LessIntrusive));

        /// <summary>
        /// Updates the specified auto code in the database.
        /// </summary>
        /// <param name="autoCode">The auto code.</param>
        public static void Update(AutoCode autoCode) =>
            DataConnection.ExecuteInsert(
                "UPDATE `auto_codes` SET `description` = ?description, `hidden` = ?hidden, `less_intrusive` = ?less_intrusive WHERE `id` = ?id",
                    new MySqlParameter("description", autoCode.Description ?? ""),
                    new MySqlParameter("hidden", autoCode.Hidden),
                    new MySqlParameter("less_intrusive", autoCode.LessIntrusive),
                    new MySqlParameter("id", autoCode.Id));

        /// <summary>
        /// Deletes the specified auto code from the database.
        /// </summary>
        /// <param name="autoCode">The auto code.</param>
        /// <exception cref="Exception">If the auto code is in use by a procedure button and cannot be deleted.</exception>
        public static void Delete(AutoCode autoCode)
        {
            string inUseButtonsList = "";

            var procButtons = ProcButtons.GetDeepCopy();
            var procButtonItems = ProcButtonItems.GetDeepCopy();

            foreach (var procButton in procButtons)
            {
                foreach (var procButtonItem in procButtonItems)
                {
                    if (procButtonItem.ProcButtonNum == procButton.ProcButtonNum &&
                        procButtonItem.AutoCodeNum == autoCode.Id)
                    {
                        if (inUseButtonsList != "") inUseButtonsList += "; ";
                        
                        inUseButtonsList += procButton.Description;

                        break;
                    }
                }
            }

            if (inUseButtonsList != "")
            {
                throw new Exception(
                    "Not allowed to delete autocode because it is in use. " +
                    "Procedure buttons using this autocode include " + inUseButtonsList);
            }


            DataConnection.ExecuteNonQuery("DELETE FROM `auto_codes` WHERE `id` = " + autoCode.Id);
        }

        /// <summary>
        /// Gets the ID of the auto code with the specified description.
        /// Returns null if the no auto code with the specified description exists.
        /// </summary>
        public static long? GetNumFromDescript(string descript) =>
            cache.FirstOrDefault(autoCode => autoCode.Description == descript)?.Id;

        /// <summary>
        /// Deletes all current autocodes and then adds the default autocodes. 
        /// Procedure codes must have already been entered or they cannot be added as an autocode.
        /// </summary>
        public static void SetToDefault()
        {
            DataConnection.ExecuteNonQuery("DELETE FROM `auto_codes`");
            DataConnection.ExecuteNonQuery("DELETE FROM `auto_code_conditions`");
            DataConnection.ExecuteNonQuery("DELETE FROM `auto_code_items`");

            if (CultureInfo.CurrentCulture.Name.EndsWith("CA")) // Canadian. en-CA or fr-CA
            {
                ResetDefaultsCanada();

                return;
            }

            ResetDefaults();
        }

        private static void ResetDefaults()
        {
            //long autoCodeId;
            //long autoCodeItemId;
            
            
            ////Amalgam-------------------------------------------------------------------------------------------------------
            //string command = "";

            //autoCodeId = DataConnection.ExecuteInsert("INSERT INTO `auto_codes` (`description`, `hidden`, `less_intrusive`) VALUES ('Amalgam', 0, 0)");

            // ufn_create_auto_code(description, hidden, less_intrusive)
            /*
             
            INSERT INTO `auto_codes` (`description`, `hidden`, `less_intrusive`) VALUES (@description, @hidden, @less_intrusive);


             
             */


            // ufn_create_auto_code_item_with_condition(auto_code_id, procedure_code, condition)
            /*
             
            DECLARE @procedure_code_id INT(11);
             
            SELECT `id` INTO @procedure_code_id FROM `procedure_codes` WHERE `code` = @procedure_code;

            IF @procedure_code_id IS NOT NULL THEN

                INSERT INTO `auto_code_items` (`auto_code_id`, `procedure_code_id`) VALUES (@auto_code_id, @procedure_code_id);

                IF @condition IS NOT NULL THEN

                    INSERT INTO `auto_code_conditions` (`auto_code_item_id`, `condition`) VALUES (LAST_INSERT_ID(), @condition);

                END;

            END;

             
             */

            //// 1Surf
            //if (ProcedureCodes.IsValidCode("D2140"))
            //{
            //    autoCodeItemId =
            //        DataConnection.ExecuteInsert(
            //            "INSERT INTO `auto_code_items` (`auto_code_id`, `procedure_code_id`) VALUES (" + autoCodeId + ", " + ProcedureCodes.GetCodeNum("D2140") + ")");

            //    DataConnection.ExecuteNonQuery(
            //        "INSERT INTO `auto_code_conditions` (`auto_code_item_id`, `condition`) VALUES (" + autoCodeItemId + ", " + (int)AutoCondition.One_Surf + ")");
            //}
            //// 2Surf
            //if (ProcedureCodes.IsValidCode("D2150"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D2150") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //}
            //// 3Surf
            //if (ProcedureCodes.IsValidCode("D2160"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D2160") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //}
            ////4Surf
            //if (ProcedureCodes.IsValidCode("D2161"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D2161") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //}
            ////5Surf
            //if (ProcedureCodes.IsValidCode("D2161"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D2161") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //}
            ////Composite-------------------------------------------------------------------------------------------------------
            //command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Composite',0,0)";
            //autoCodeId = Db.NonQ(command, true);
            ////1SurfAnt
            //if (ProcedureCodes.IsValidCode("D2330"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D2330") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.One_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //}
            ////2SurfAnt
            //if (ProcedureCodes.IsValidCode("D2331"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D2331") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //}
            ////3SurfAnt
            //if (ProcedureCodes.IsValidCode("D2332"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D2332") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //}
            ////4SurfAnt
            //if (ProcedureCodes.IsValidCode("D2335"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D2335") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //}
            ////5SurfAnt
            //if (ProcedureCodes.IsValidCode("D2335"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D2335") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //}
            ////Posterior Composite----------------------------------------------------------------------------------------------
            ////1SurfPost
            //if (ProcedureCodes.IsValidCode("D2391"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D2391") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.One_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Posterior) + ")";
            //    Db.NonQ(command);
            //}
            ////2SurfPost
            //if (ProcedureCodes.IsValidCode("D2392"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D2392") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Posterior) + ")";
            //    Db.NonQ(command);
            //}
            ////3SurfPost
            //if (ProcedureCodes.IsValidCode("D2393"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D2393") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Posterior) + ")";
            //    Db.NonQ(command);
            //}
            ////4SurfPost
            //if (ProcedureCodes.IsValidCode("D2394"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D2394") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Posterior) + ")";
            //    Db.NonQ(command);
            //}
            ////5SurfPost
            //if (ProcedureCodes.IsValidCode("D2394"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D2394") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Posterior) + ")";
            //    Db.NonQ(command);
            //}
            ////Root Canal-------------------------------------------------------------------------------------------------------
            //command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Root Canal',0,0)";
            //autoCodeId = Db.NonQ(command, true);
            ////Ant
            //if (ProcedureCodes.IsValidCode("D3310"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D3310") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //}
            ////Premolar
            //if (ProcedureCodes.IsValidCode("D3320"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D3320") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //}
            ////Molar
            //if (ProcedureCodes.IsValidCode("D3330"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D3330") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //}
            ////PFM Bridge-------------------------------------------------------------------------------------------------------
            //command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('PFM Bridge',0,0)";
            //autoCodeId = Db.NonQ(command, true);
            ////Pontic
            //if (ProcedureCodes.IsValidCode("D6242"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D6242") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Pontic) + ")";
            //    Db.NonQ(command);
            //}
            ////Retainer
            //if (ProcedureCodes.IsValidCode("D6752"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D6752") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Retainer) + ")";
            //    Db.NonQ(command);
            //}
            ////Ceramic Bridge-------------------------------------------------------------------------------------------------------
            //command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Ceramic Bridge',0,0)";
            //autoCodeId = Db.NonQ(command, true);
            ////Pontic
            //if (ProcedureCodes.IsValidCode("D6245"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D6245") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Pontic) + ")";
            //    Db.NonQ(command);
            //}
            ////Retainer
            //if (ProcedureCodes.IsValidCode("D6740"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D6740") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Retainer) + ")";
            //    Db.NonQ(command);
            //}
            ////Denture-------------------------------------------------------------------------------------------------------
            //command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Denture',0,0)";
            //autoCodeId = Db.NonQ(command, true);
            ////Max
            //if (ProcedureCodes.IsValidCode("D5110"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D5110") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Maxillary) + ")";
            //    Db.NonQ(command);
            //}
            ////Mand
            //if (ProcedureCodes.IsValidCode("D5120"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D5120") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Mandibular) + ")";
            //    Db.NonQ(command);
            //}
            ////BU/P&C-------------------------------------------------------------------------------------------------------
            //command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('BU/P&C',0,0)";
            //autoCodeId = Db.NonQ(command, true);
            ////BU
            //if (ProcedureCodes.IsValidCode("D2950"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D2950") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Posterior) + ")";
            //    Db.NonQ(command);
            //}
            ////P&C
            //if (ProcedureCodes.IsValidCode("D2954"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D2954") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //}
            ////Root Canal Retreat--------------------------------------------------------------------------------------------------
            //command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('RCT Retreat',0,0)";
            //autoCodeId = Db.NonQ(command, true);
            ////Ant
            //if (ProcedureCodes.IsValidCode("D3346"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D3346") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //}
            ////Premolar
            //if (ProcedureCodes.IsValidCode("D3347"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D3347") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //}
            ////Molar
            //if (ProcedureCodes.IsValidCode("D3348"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeId) + ","
            //        + ProcedureCodes.GetCodeNum("D3348") + ")";
            //    autoCodeItemId = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemId) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //}
        }

        public static void ResetDefaultsCanada()
        {
            //string command;
            //long autoCodeNum;
            //long autoCodeItemNum;
            ////Amalgam-Bonded--------------------------------------------------------------------------------------
            //command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Amalgam-Bonded',0,0)";
            //autoCodeNum = Db.NonQ(command, true);
            ////1SurfPrimaryMolar
            //if (ProcedureCodes.IsValidCode("21121"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21121") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.One_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////1SurfPrimaryAnterior
            //if (ProcedureCodes.IsValidCode("21121"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21121") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.One_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////2SurfPrimaryAnterior
            //if (ProcedureCodes.IsValidCode("21122"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21122") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////2SurfPrimaryMolar
            //if (ProcedureCodes.IsValidCode("21122"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21122") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////3SurfPrimaryAnterior
            //if (ProcedureCodes.IsValidCode("21123"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21123") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////3SurfPrimaryMolar
            //if (ProcedureCodes.IsValidCode("21123"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21123") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////4SurfPrimaryAnterior
            //if (ProcedureCodes.IsValidCode("21124"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21124") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////4SurfPrimaryMolar
            //if (ProcedureCodes.IsValidCode("21124"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21124") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////5SurfPrimaryAnterior
            //if (ProcedureCodes.IsValidCode("21125"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21125") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////5SurfPrimaryMolar
            //if (ProcedureCodes.IsValidCode("21125"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21125") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////1SurfPermanentAnterior
            //if (ProcedureCodes.IsValidCode("21231"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21231") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.One_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////1SurfPermanentPremolar
            //if (ProcedureCodes.IsValidCode("21231"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21231") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.One_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////2SurfPermanentAnterior
            //if (ProcedureCodes.IsValidCode("21232"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21232") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////2SurfPermanentPremolar
            //if (ProcedureCodes.IsValidCode("21232"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21232") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////3SurfPermanentAnterior
            //if (ProcedureCodes.IsValidCode("21233"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21233") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////3SurfPermanentPremolar
            //if (ProcedureCodes.IsValidCode("21233"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21233") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////4SurfPermanentAnterior
            //if (ProcedureCodes.IsValidCode("21234"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21234") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////4SurfPermanentPremolar
            //if (ProcedureCodes.IsValidCode("21234"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21234") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////5SurfPermanentAnterior
            //if (ProcedureCodes.IsValidCode("21235"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21235") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////5SurfPermanentPremolar
            //if (ProcedureCodes.IsValidCode("21235"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21235") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////1SurfPermanentMolar
            //if (ProcedureCodes.IsValidCode("21241"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21241") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.One_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////2SurfPermanentMolar
            //if (ProcedureCodes.IsValidCode("21242"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21242") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////3SurfPermanentMolar
            //if (ProcedureCodes.IsValidCode("21243"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21243") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////4SurfPermanentMolar
            //if (ProcedureCodes.IsValidCode("21244"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21244") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////5SurfPermanentMolar
            //if (ProcedureCodes.IsValidCode("21245"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21245") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////Amalgam Non-Bonded----------------------------------------------------------------------------------
            //command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Amalgam Non-Bonded',0,0)";
            //autoCodeNum = Db.NonQ(command, true);
            ////1SurfPrimaryAnterior
            //if (ProcedureCodes.IsValidCode("21111"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21111") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.One_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////1SurfPrimaryMolar
            //if (ProcedureCodes.IsValidCode("21111"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21111") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.One_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////2SurfPrimaryAnterior
            //if (ProcedureCodes.IsValidCode("21112"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21112") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////2SurfPrimaryMolar
            //if (ProcedureCodes.IsValidCode("21112"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21112") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////3SurfPrimaryMolar
            //if (ProcedureCodes.IsValidCode("21113"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21113") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////3SurfPrimaryAnterior
            //if (ProcedureCodes.IsValidCode("21113"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21113") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////4SurfPrimaryAnterior
            //if (ProcedureCodes.IsValidCode("21114"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21114") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////4SurfPrimaryMolar
            //if (ProcedureCodes.IsValidCode("21114"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21114") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////5SurfPrimaryAnterior
            //if (ProcedureCodes.IsValidCode("21115"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21115") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////5SurfPrimaryMolar
            //if (ProcedureCodes.IsValidCode("21115"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21115") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////1SurfPermanentAnterior
            //if (ProcedureCodes.IsValidCode("21211"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21211") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.One_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////1SurfPermanentPremolar
            //if (ProcedureCodes.IsValidCode("21211"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21211") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.One_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////2SurfPermanentAnterior
            //if (ProcedureCodes.IsValidCode("21212"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21212") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////2SurfPermanentPremolar
            //if (ProcedureCodes.IsValidCode("21212"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21212") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////3SurfPermanentAnterior
            //if (ProcedureCodes.IsValidCode("21213"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21213") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////3SurfPermanentPremolar
            //if (ProcedureCodes.IsValidCode("21213"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21213") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////4SurfPermanentAnterior
            //if (ProcedureCodes.IsValidCode("21214"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21214") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////4SurfPermanentPremolar
            //if (ProcedureCodes.IsValidCode("21214"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21214") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////5SurfPermanentAnterior
            //if (ProcedureCodes.IsValidCode("21215"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21215") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////5SurfPermanentPremolar
            //if (ProcedureCodes.IsValidCode("21215"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21215") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////1SurfPermanentMolar
            //if (ProcedureCodes.IsValidCode("21221"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21221") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.One_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////2SurfPermanentMolar
            //if (ProcedureCodes.IsValidCode("21222"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21222") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////3SurfPermanentMolar
            //if (ProcedureCodes.IsValidCode("21223"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21223") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////4SurfPermanentMolar
            //if (ProcedureCodes.IsValidCode("21224"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21224") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////5SurfPermanentMolar
            //if (ProcedureCodes.IsValidCode("21225"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("21225") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////Composite-------------------------------------------------------------------------------------------
            //command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Composite',0,0)";
            //autoCodeNum = Db.NonQ(command, true);
            ////1SurfPermanentAnterior
            //if (ProcedureCodes.IsValidCode("23111"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23111") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.One_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////2SurfPermanentAnterior
            //if (ProcedureCodes.IsValidCode("23112"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23112") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////3SurfPermanentAnterior
            //if (ProcedureCodes.IsValidCode("23113"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23113") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////4SurfPermanentAnterior
            //if (ProcedureCodes.IsValidCode("23114"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23114") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////5SurfPermanentAnterior
            //if (ProcedureCodes.IsValidCode("23115"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23115") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////1SurfPermanentPremolar
            //if (ProcedureCodes.IsValidCode("23311"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23311") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.One_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////2SurfPermanentPremolar
            //if (ProcedureCodes.IsValidCode("23312"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23312") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////3SurfPermanentPremolar
            //if (ProcedureCodes.IsValidCode("23313"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23313") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////4SurfPermanentPremolar
            //if (ProcedureCodes.IsValidCode("23314"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23314") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////5SurfPermanentPremolar
            //if (ProcedureCodes.IsValidCode("23315"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23315") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////1SurfPermanentMolar
            //if (ProcedureCodes.IsValidCode("23321"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23321") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.One_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////2SurfPermanentMolar
            //if (ProcedureCodes.IsValidCode("23322"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23322") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////3SurfPermanentMolar
            //if (ProcedureCodes.IsValidCode("23323"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23323") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////4SurfPermanentMolar
            //if (ProcedureCodes.IsValidCode("23324"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23324") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////5SurfPermanentMolar
            //if (ProcedureCodes.IsValidCode("23325"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23325") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Permanent) + ")";
            //    Db.NonQ(command);
            //}
            ////1SurfPrimaryAnterior
            //if (ProcedureCodes.IsValidCode("23411"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23411") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.One_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////2SurfPrimaryAnterior
            //if (ProcedureCodes.IsValidCode("23412"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23412") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////3SurfPrimaryAnterior
            //if (ProcedureCodes.IsValidCode("23413"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23413") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////4SurfPrimaryAnterior
            //if (ProcedureCodes.IsValidCode("23414"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23414") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////5SurfPrimaryAnterior
            //if (ProcedureCodes.IsValidCode("23415"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23415") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////1SurfPrimaryMolar
            //if (ProcedureCodes.IsValidCode("23511"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23511") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.One_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////2SurfPrimaryMolar
            //if (ProcedureCodes.IsValidCode("23512"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23512") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Two_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////3SurfPrimaryMolar
            //if (ProcedureCodes.IsValidCode("23513"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23513") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Three_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////4SurfPrimaryMolar
            //if (ProcedureCodes.IsValidCode("23514"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23514") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Four_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////5SurfPrimaryMolar
            //if (ProcedureCodes.IsValidCode("23515"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("23515") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Five_Surf) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Primary) + ")";
            //    Db.NonQ(command);
            //}
            ////Root Canal------------------------------------------------------------------------------------------
            //command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Root Canal',0,0)";
            //autoCodeNum = Db.NonQ(command, true);
            ////Anterior
            //if (ProcedureCodes.IsValidCode("33111"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("33111") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Anterior) + ")";
            //    Db.NonQ(command);
            //}
            ////Premolar
            //if (ProcedureCodes.IsValidCode("33121"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("33121") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Premolar) + ")";
            //    Db.NonQ(command);
            //}
            ////Molar
            //if (ProcedureCodes.IsValidCode("33131"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("33131") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Molar) + ")";
            //    Db.NonQ(command);
            //}
            ////Denture---------------------------------------------------------------------------------------------
            //command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Denture',0,0)";
            //autoCodeNum = Db.NonQ(command, true);
            ////Maxillary
            //if (ProcedureCodes.IsValidCode("51101"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("51101") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Maxillary) + ")";
            //    Db.NonQ(command);
            //}
            ////Mandibular
            //if (ProcedureCodes.IsValidCode("51302"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("51302") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Mandibular) + ")";
            //    Db.NonQ(command);
            //}
            ////Bridge----------------------------------------------------------------------------------------------
            //command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Bridge',0,0)";
            //autoCodeNum = Db.NonQ(command, true);
            ////Pontic
            //if (ProcedureCodes.IsValidCode("62501"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("62501") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Pontic) + ")";
            //    Db.NonQ(command);
            //}
            ////Retainer
            //if (ProcedureCodes.IsValidCode("67211"))
            //{
            //    command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
            //        + ProcedureCodes.GetCodeNum("67211") + ")";
            //    autoCodeItemNum = Db.NonQ(command, true);
            //    command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
            //        + POut.Long((int)AutoCondition.Retainer) + ")";
            //    Db.NonQ(command);
            //}
        }
    }
}
