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
    public class AutoCodeItemCondition : DataRecordBase
    {
        private static readonly DataRecordCacheBase<AutoCodeItemCondition> cache =
            new DataRecordCacheBase<AutoCodeItemCondition>("SELECT * FROM `auto_code_item_conditions` ORDER BY `condition`", FromReader);

        /// <summary>
        /// The ID of the auto code item the condition applies to.
        /// </summary>
        public long AutoCodeItemId;

        /// <summary>
        /// The condition.
        /// </summary>
        public AutoCodeItemConditionType Condition;

        /// <summary>
        /// Constructs a new instance of the <see cref="AutoCodeItemCondition"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="AutoCodeItemCondition"/> instance.</returns>
        private static AutoCodeItemCondition FromReader(MySqlDataReader dataReader)
        {
            return new AutoCodeItemCondition
            {
                AutoCodeItemId = (long)dataReader["auto_code_item_id"],
                Condition = (AutoCodeItemConditionType)Convert.ToInt32(dataReader["condition"])
            };
        }

        /// <summary>
        /// Gets a list of all auto code conditions.
        /// </summary>
        /// <returns>A list of auto code conditions.</returns>
        public static List<AutoCodeItemCondition> All() =>
            cache.All().ToList();

        /// <summary>
        /// Inserts the specified auto code condition into the database.
        /// </summary>
        /// <param name="autoCodeItemCondition">The auto code condition.</param>
        public static void Insert(AutoCodeItemCondition autoCodeItemCondition) =>
            DataConnection.ExecuteInsert(
                "INSERT IGNORE INTO `auto_code_item_conditions` (`auto_code_item_id`, `condition`) VALUES (?auto_code_item_id, ?condition)",
                    new MySqlParameter("auto_code_item_id", autoCodeItemCondition.AutoCodeItemId),
                    new MySqlParameter("condition", autoCodeItemCondition.Condition));

        /// <summary>
        /// Deletes the specified auto code condition from the database.
        /// </summary>
        /// <param name="autoCodeCondition">The auto code condition.</param>
        public static void Delete(AutoCodeItemCondition autoCodeItemCondition) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `auto_code_conditions` WHERE `auto_code_item_id` = ?auto_code_item_id AND `condition` = ?condition",
                    new MySqlParameter("auto_code_item_id", autoCodeItemCondition.AutoCodeItemId),
                    new MySqlParameter("condition", autoCodeItemCondition.Condition));

        /// <summary>
        /// Deletes all conditions for the specified auto code item from the database.
        /// </summary>
        /// <param name="autoCodeItemId">The ID of the auto code item.</param>
        public static void DeleteByAutoCodeItem(long autoCodeItemId) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `auto_code_conditions` WHERE `auto_code_item_id` = " + autoCodeItemId);

        /// <summary>
        /// Gets a list of auto code conditions for the specified auto code item.
        /// </summary>
        /// <param name="autoCodeItemId">The ID of the auto code item.</param>
        /// <returns>A list of auto code conditions.</returns>
        public static List<AutoCodeItemCondition> GetByAutoCodeItem(long autoCodeItemId) =>
            cache.Where(autoCodeCond => autoCodeCond.AutoCodeItemId == autoCodeItemId).ToList();
        
        /// <summary>
        /// Determines whether the specified condition is a surface condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public static bool IsSurface(AutoCodeItemConditionType condition)
        {
            switch (condition)
            {
                case AutoCodeItemConditionType.OneSurface:
                case AutoCodeItemConditionType.TwoSurfaces:
                case AutoCodeItemConditionType.ThreeSurfaces:
                case AutoCodeItemConditionType.FourSurfaces:
                case AutoCodeItemConditionType.FiveSurfaces:
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
        public static bool CheckCondition(AutoCodeItemConditionType condition, string toothNumber, string surface, bool isAdditional, bool willBeMissing, int age)
        {
            switch (condition)
            {
                case AutoCodeItemConditionType.Anterior:        return Tooth.IsAnterior(toothNumber);
                case AutoCodeItemConditionType.Posterior:       return Tooth.IsPosterior(toothNumber);
                case AutoCodeItemConditionType.Premolar:        return Tooth.IsPreMolar(toothNumber);
                case AutoCodeItemConditionType.Molar:           return Tooth.IsMolar(toothNumber);
                case AutoCodeItemConditionType.OneSurface:      return surface.Length == 1;
                case AutoCodeItemConditionType.TwoSurfaces:     return surface.Length == 2;
                case AutoCodeItemConditionType.ThreeSurfaces:   return surface.Length == 3;
                case AutoCodeItemConditionType.FourSurfaces:    return surface.Length == 4;
                case AutoCodeItemConditionType.FiveSurfaces:    return surface.Length == 5;
                case AutoCodeItemConditionType.First:           return !isAdditional;
                case AutoCodeItemConditionType.EachAdditional:  return isAdditional;
                case AutoCodeItemConditionType.Maxillary:       return Tooth.IsMaxillary(toothNumber);
                case AutoCodeItemConditionType.Mandibular:      return !Tooth.IsMaxillary(toothNumber);
                case AutoCodeItemConditionType.Primary:         return Tooth.IsPrimary(toothNumber);
                case AutoCodeItemConditionType.Permanent:       return !Tooth.IsPrimary(toothNumber);
                case AutoCodeItemConditionType.Pontic:          return willBeMissing;
                case AutoCodeItemConditionType.Retainer:        return !willBeMissing;
                case AutoCodeItemConditionType.AgeOver18:       return age > 18;
            }

            return false;
        }
    }
}
