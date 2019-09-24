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
    /// A single autonote template.
    /// </summary>
    public class AutoNote : DataRecord
    {
        private static readonly DataRecordCache<AutoNote> cache = 
            new DataRecordCache<AutoNote>("SELECT * FROM `auto_notes` ORDER BY `name`", FromReader);

        /// <summary>
        /// The name of the auto note.
        /// </summary>
        public string Name;

        /// <summary>
        /// The content of the auto note.
        /// </summary>
        public string Content;

        /// <summary>
        /// FK to definition.DefNum.
        /// This is the AutoNoteCat definition category (DefCat=41), for categorizing autonotes.
        /// Uncategorized autonotes will be set to null.
        /// </summary>
        public long? CategoryId;

        /// <summary>
        /// Constructs a new instance of the <see cref="AutoNote"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="AutoNote"/> instance.</returns>
        private static AutoNote FromReader(MySqlDataReader dataReader)
        {
            return new AutoNote
            {
                Id = (long)dataReader["id"],
                Name = (string)dataReader["name"],
                Content = (string)dataReader["content"]
            };
        }

        /// <summary>
        /// Gets a list of all auto notes.
        /// </summary>
        /// <returns>A list of auto notes.</returns>
        public static List<AutoNote> All() =>
            cache.All().ToList();

        /// <summary>
        /// Inserts the specified auto note into the database.
        /// </summary>
        /// <param name="autoNote">The auto note.</param>
        /// <returns>The ID assigned to the auto note.</returns>
        public static long Insert(AutoNote autoNote) =>
            autoNote.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `auto_notes` (`name`, `content`) VALUES (?name, ?content)",
                    new MySqlParameter("name", autoNote.Name ?? ""),
                    new MySqlParameter("content", autoNote.Content ?? ""));

        /// <summary>
        /// Updates the specified auto note in the database.
        /// </summary>
        /// <param name="autoNote">The auto note.</param>
        public static void Update(AutoNote autoNote) =>
             DataConnection.ExecuteInsert(
                "UPDATE `auto_notes` SET `name` = ?name, `content` = ?content WHERE `id` = ?id",
                    new MySqlParameter("name", autoNote.Name ?? ""),
                    new MySqlParameter("content", autoNote.Content ?? ""),
                    new MySqlParameter("id", autoNote.Id));

        /// <summary>
        /// Deletes the specified auto note from the database.
        /// </summary>
        /// <param name="autoNoteId"></param>
        public static void Delete(long autoNoteId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `auto_notes` WHERE `id` = " + autoNoteId);

        /// <summary>
        /// Gets the text of the auto note with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetByName(string name) =>
            cache.SelectOne(autoNote => autoNote.Name == name)?.Content ?? "";

        /// <summary>
        /// Returns true if there is a valid AutoNote for the passed in AutoNoteName.
        /// </summary>
        public static bool IsValidAutoNote(string name) =>
            cache.Any(autoNote => autoNote.Name == name);

        /// <summary>
        /// Sets the autonote.Category=0 for the autonote category DefNum provided.  Returns the number of rows updated.
        /// </summary>
        public static long RemoveFromCategory(long autoNoteCatDefId) =>
            DataConnection.ExecuteNonQuery("UPDATE `auto_notes` SET `category_id` = NULL WHERE `category_id` = " + autoNoteCatDefId);

        public AutoNote Copy() => (AutoNote)MemberwiseClone();
    }
}
