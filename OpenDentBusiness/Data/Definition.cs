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
using System.Drawing;

namespace OpenDentBusiness
{
    public class Definition : DataRecord
    {
        public DefinitionCategory Category;

        /// <summary>
        /// Each category is a little different. This field is usually the common name of the item.
        /// </summary>
        public string Description;

        /// <summary>
        /// This field can be used to store extra info about the item.
        /// </summary>
        public string Value;

        /// <summary>
        /// Some categories include a color option.
        /// </summary>
        public Color Color;

        /// <summary>
        /// The sort order of the definition.
        /// </summary>
        public int SortOrder;

        /// <summary>
        /// If hidden, the item will not show on any list, but can still be referenced.
        /// </summary>
        public bool Hidden;

        /// <summary>
        /// Returns a string representation of the definition.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Description ?? "";

        /// <summary>
        /// Constructs a new instance of the <see cref="Definition"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Definition"/> instance.</returns>
        static Definition FromReader(MySqlDataReader dataReader)
        {
            var definition = new Definition
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Category = (DefinitionCategory)Convert.ToInt32(dataReader["category"]),
                Description = Convert.ToString(dataReader["description"]),
                Value = Convert.ToString(dataReader["value"]),
                Hidden = Convert.ToBoolean(dataReader["hidden"])
            };

            var color = dataReader["color"];
            if (color != DBNull.Value)
            {
                definition.Color = ColorTranslator.FromHtml(Convert.ToString(color));
            }

            return definition;
        }

        /// <summary>
        /// Gets a list of all definitions.
        /// </summary>
        /// <returns>A list of definitions.</returns>
        public static List<Definition> All() =>
            SelectMany("SELECT * FROM `definitions` ORDER BY `category`, `sort_order`", FromReader);

        /// <summary>
        /// Gets the definition with the specified ID.
        /// </summary>
        /// <param name="definitionId">The ID of the definition.</param>
        /// <returns>The definition with the specified ID.</returns>
        public static Definition GetById(long definitionId) =>
            SelectOne("SELECT * FROM `definitions` WHERE `id` = " + definitionId, FromReader);

        /// <summary>
        /// Gets a list of all definition in the specified category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>A list of definitions.</returns>
        public static List<Definition> GetByCategory(DefinitionCategory category, bool includeHidden = false) =>
            SelectMany("SELECT * FROM `definitions` WHERE `category` = ?category AND (?include_hidden OR `hidden` = 0) ORDER BY `sort_order`", FromReader,
                new MySqlParameter("category", (int)category),
                new MySqlParameter("include_hidden", includeHidden));

        /// <summary>
        /// Gets the definition with the specified ID in the specified category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="definitionId">The ID of the definition.</param>
        /// <returns>The definition with the specified ID.</returns>
        public static Definition GetByCategory(DefinitionCategory category, long definitionId) =>
            SelectOne("SELECT * FROM `definitions` WHERE `category` = ?category AND `id` = ?id", FromReader,
                new MySqlParameter("category", (int)category),
                new MySqlParameter("id", definitionId));

        /// <summary>
        /// Gets the definition with the specified description in the specified category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="description">The description of the definition.</param>
        /// <returns>The definition with the specified description.</returns>
        public static Definition GetByDescription(DefinitionCategory category, string description) =>
            SelectOne("SELECT * FROM `definitions` WHERE `category` = ?category AND `description` = ?description", FromReader, 
                new MySqlParameter("category", (int)category), 
                new MySqlParameter("description", description));

        /// <summary>
        /// Inserts the specified definition into the database.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <returns>The ID assigned to the definition.</returns>
        public static long Insert(Definition definition) =>
            definition.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `definitions` (`category`, `description`, `value`, `color`, `sort_order`, `hidden`) VALUES (?category, ?description, ?value, ?color, ?sort_order, ?hidden)",
                    new MySqlParameter("category", (int)definition.Category),
                    new MySqlParameter("description", definition.Description),
                    new MySqlParameter("value", definition.Value),
                    new MySqlParameter("color", ColorTranslator.ToHtml(definition.Color)),
                    new MySqlParameter("sort_order", definition.SortOrder),
                    new MySqlParameter("hidden", definition.Hidden));

        /// <summary>
        /// Updates the specified definition in the database.
        /// </summary>
        /// <param name="definition">The definition.</param>
        public static void Update(Definition definition) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `definitions` SET `category` = ?category, `description` = ?description, `value` = ?value, `color` = ?color, `sort_order` = ?sort_order, `hidden` = ?hidden WHERE `id` = ?id",
                     new MySqlParameter("category", (int)definition.Category),
                     new MySqlParameter("description", definition.Description),
                     new MySqlParameter("value", definition.Value),
                     new MySqlParameter("color", ColorTranslator.ToHtml(definition.Color)),
                     new MySqlParameter("sort_order", definition.SortOrder),
                     new MySqlParameter("hidden", definition.Hidden),
                     new MySqlParameter("id", definition.Id));

        /// <summary>
        /// Deletes the definition with the specified ID from the database.
        /// </summary>
        /// <param name="definitionId">The ID of the definition.</param>
        public static void Delete(long definitionId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `definitions` WHERE `id` = " + definitionId);

        #region CLEANUP

        public Definition Copy() => (Definition)MemberwiseClone();

        #endregion
    }
}
