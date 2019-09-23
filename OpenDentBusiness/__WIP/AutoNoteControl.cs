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
using System.Linq;

namespace OpenDentBusiness
{
    /// <summary>
    /// In the program, this is now called an autonote prompt.
    /// </summary>
    public class AutoNoteControl : DataRecord
    {
        private static readonly DataRecordCache<AutoNoteControl> cache =
            new DataRecordCache<AutoNoteControl>("SELECT * FROM `auto_note_controls`", FromReader);

        /// <summary>
        /// A description of the auto note control.
        /// </summary>
        public string Description;

        /// <summary>
        /// 'Text', 'OneResponse', or 'MultiResponse'.  More types to be added later.
        /// </summary>
        public string ControlType;

        /// <summary>
        /// The prompt text.
        /// </summary>
        public string ControlLabel;

        /// <summary>
        /// For TextBox, this is the default text.
        /// For a ComboBox, this is the list of possible responses, one per line.
        /// </summary>
        public string ControlOptions;

        /// <summary>
        /// Constructs a new instance of the <see cref="AutoNoteControl"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="AutoNoteControl"/> instance.</returns>
        private static AutoNoteControl FromReader(MySqlDataReader dataReader)
        {
            return new AutoNoteControl
            {
                Id = (long)dataReader["id"],
                ControlType = (string)dataReader["type"],
                ControlLabel = (string)dataReader["label"],
                ControlOptions = (string)dataReader["options"]
            };
        }

        /// <summary>
        /// Gets the auto note control with the specified description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>The auto note control with the specified description.</returns>
        public static AutoNoteControl GetByDescription(string description) =>
            cache.FirstOrDefault(autoNoteControl => autoNoteControl.Description == description);

        /// <summary>
        /// Inserts the specified <see cref="AutoNoteControl"/> into the database.
        /// </summary>
        /// <param name="autoNoteControl">The auto note control.</param>
        /// <returns>The ID assigned to the auto note control.</returns>
        public static long Insert(AutoNoteControl autoNoteControl) =>
            autoNoteControl.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `auto_note_controls` (`type`, `label`, `options`) VALUES (?type, ?label, ?options)",
                    new MySqlParameter("type", autoNoteControl.ControlType ?? ""),
                    new MySqlParameter("label", autoNoteControl.ControlLabel ?? ""),
                    new MySqlParameter("options", autoNoteControl.ControlOptions ?? ""));
        
        /// <summary>
        /// Updates the specified auto note control in the database.
        /// </summary>
        /// <param name="autoNoteControl">The auto note control.</param>
        public static void Update(AutoNoteControl autoNoteControl) =>
            DataConnection.ExecuteInsert(
                "UPDATE `auto_note_controls` SET `type` = ?type, `label` = ?label, `options` = ?options WHERE `id` = ?id",
                    new MySqlParameter("type", autoNoteControl.ControlType ?? ""),
                    new MySqlParameter("label", autoNoteControl.ControlLabel ?? ""),
                    new MySqlParameter("options", autoNoteControl.ControlOptions ?? ""),
                    new MySqlParameter("options", autoNoteControl.Id));

        /// <summary>
        /// Deletes the specified auto note control from the database.
        /// </summary>
        /// <param name="autoNoteControlId"></param>
        public static void Delete(long autoNoteControlId) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `auto_note_controls` WHERE `id` = " + autoNoteControlId);
    }
}
