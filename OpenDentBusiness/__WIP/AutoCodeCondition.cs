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
using System.Linq;

namespace OpenDentBusiness
{
    /// <summary>
    /// Auto code condition. Always attached to an auto code item, which is then, in turn, attached
    /// to an auto code. There is usually only one or two conditions for a given auto code item.
    /// </summary>
    public class AutoCodeCondition : DataRecord
    {
        private static readonly DataRecordCache<AutoCodeCondition> cache =
            new DataRecordCache<AutoCodeCondition>("SELECT * FROM `auto_code_conditions` ORDER BY `condition`", FromReader);

        /// <summary>
        /// The ID of the auto code item the condition applies to.
        /// </summary>
        public long AutoCodeItemId;

        /// <summary>
        /// The condition.
        /// </summary>
        public AutoCodeConditionType Condition;

        /// <summary>
        /// Constructs a new instance of the <see cref="AutoCodeCondition"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="AutoCodeCondition"/> instance.</returns>
        private static AutoCodeCondition FromReader(MySqlDataReader dataReader)
        {
            return new AutoCodeCondition
            {
                Id = (long)dataReader["id"],
                AutoCodeItemId = (long)dataReader["auto_code_item_id"],
                Condition = (AutoCodeConditionType)Convert.ToInt32(dataReader["condition"])
            };
        }

        /// <summary>
        /// Gets a list of all auto code conditions.
        /// </summary>
        /// <returns>A list of auto code conditions.</returns>
        public static List<AutoCodeCondition> All() =>
            cache.All().ToList();

        /// <summary>
        /// Inserts the specified auto code condition into the database.
        /// </summary>
        /// <param name="autoCodeCondition">The auto code condition.</param>
        /// <returns>The ID assigned to the auto code condition.</returns>
        public static long Insert(AutoCodeCondition autoCodeCondition) =>
            autoCodeCondition.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `auto_code_conditions` (`auto_code_item_id`, `condition`) VALUES (?auto_code_item_id, ?condition)",
                    new MySqlParameter("auto_code_item_id", autoCodeCondition.AutoCodeItemId),
                    new MySqlParameter("condition", autoCodeCondition.Condition));

        /// <summary>
        /// Updates the specified auto code condition in the database.
        /// </summary>
        /// <param name="autoCodeCondition">The auto code condition.</param>
        public static void Update(AutoCodeCondition autoCodeCondition) =>
             DataConnection.ExecuteNonQuery(
                "UPDATE `auto_code_conditions` SET `auto_code_item_id` = ?auto_code_item_id, `condition` = ?condition WHERE `id` = ?id",
                    new MySqlParameter("auto_code_item_id", autoCodeCondition.AutoCodeItemId),
                    new MySqlParameter("condition", autoCodeCondition.Condition),
                    new MySqlParameter("id", autoCodeCondition.Id));

        /// <summary>
        /// Deletes the specified auto code condition from the database.
        /// </summary>
        /// <param name="autoCodeCondition">The auto code condition.</param>
        public static void Delete(AutoCodeCondition autoCodeCondition) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `auto_code_conditions` WHERE `id` = " + autoCodeCondition.Id);

        /// <summary>
        /// Gets a list of auto code conditions for the specified auto code item.
        /// </summary>
        /// <param name="autoCodeItemId">The ID of the auto code item.</param>
        /// <returns>A list of auto code conditions.</returns>
        public static List<AutoCodeCondition> GetByAutoCodeItem(long autoCodeItemId) =>
            cache.Where(autoCodeCond => autoCodeCond.AutoCodeItemId == autoCodeItemId).ToList();
        
        public static bool IsSurface(AutoCodeConditionType condition)
        {
            switch (condition)
            {
                case AutoCodeConditionType.OneSurface:
                case AutoCodeConditionType.TwoSurfaces:
                case AutoCodeConditionType.ThreeSurfaces:
                case AutoCodeConditionType.FourSurfaces:
                case AutoCodeConditionType.FiveSurfaces:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines whether the specified details meet the given condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="toothNumber"></param>
        /// <param name="surface"></param>
        /// <param name="isAdditional"></param>
        /// <param name="willBeMissing"></param>
        /// <param name="age"></param>
        /// <returns>True if the condition is met; otherwise, false.</returns>
        public static bool CheckCondition(AutoCodeConditionType condition, string toothNumber, string surface, bool isAdditional, bool willBeMissing, int age)
        {
            switch (condition)
            {
                case AutoCodeConditionType.Anterior:        return Tooth.IsAnterior(toothNumber);
                case AutoCodeConditionType.Posterior:       return Tooth.IsPosterior(toothNumber);
                case AutoCodeConditionType.Premolar:        return Tooth.IsPreMolar(toothNumber);
                case AutoCodeConditionType.Molar:           return Tooth.IsMolar(toothNumber);
                case AutoCodeConditionType.OneSurface:      return surface.Length == 1;
                case AutoCodeConditionType.TwoSurfaces:     return surface.Length == 2;
                case AutoCodeConditionType.ThreeSurfaces:   return surface.Length == 3;
                case AutoCodeConditionType.FourSurfaces:    return surface.Length == 4;
                case AutoCodeConditionType.FiveSurfaces:    return surface.Length == 5;
                case AutoCodeConditionType.First:           return !isAdditional;
                case AutoCodeConditionType.EachAdditional:  return isAdditional;
                case AutoCodeConditionType.Maxillary:       return Tooth.IsMaxillary(toothNumber);
                case AutoCodeConditionType.Mandibular:      return !Tooth.IsMaxillary(toothNumber);
                case AutoCodeConditionType.Primary:         return Tooth.IsPrimary(toothNumber);
                case AutoCodeConditionType.Permanent:       return !Tooth.IsPrimary(toothNumber);
                case AutoCodeConditionType.Pontic:          return willBeMissing;
                case AutoCodeConditionType.Retainer:        return !willBeMissing;
                case AutoCodeConditionType.AgeOver18:       return age > 18;
            }

            return false;
        }
    }
}
