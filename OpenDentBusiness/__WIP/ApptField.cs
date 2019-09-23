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

namespace OpenDentBusiness
{
    public class ApptField : DataRecord
    {
        public long AppointmentId;

        /// <summary>
        /// FK to apptfielddef.FieldName.
        /// The full name is shown here for ease of use when running queries.
        /// But the user is only allowed to change fieldNames in the patFieldDef setup window.
        /// </summary>
        public string FieldName;

        /// <summary>
        /// Any text that the user types in. 
        /// Will later allow some automation.
        /// </summary>
        public string Value;

        /// <summary>
        /// Constructs a new instance of the <see cref="ApptField"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="ApptField"/> instance.</returns>
        private static ApptField FromReader(MySqlDataReader dataReader)
        {
            return new ApptField
            {
                Id = (long)dataReader["id"],
                AppointmentId = (long)dataReader["appointment_id"],
                FieldName = (string)dataReader["field_name"],
                Value = (string)dataReader["value"]
            };
        }

        /// <summary>
        /// Gets the appointment field with the specified ID from the datbase.
        /// </summary>
        /// <param name="apptFieldId">The ID of the appointment field.</param>
        /// <returns>The appointment field.</returns>
        public static ApptField GetById(long apptFieldId) =>
            SelectOne("SELECT * FROM `appointment_fields` WHERE `id` = " + apptFieldId, FromReader);

        /// <summary>
        /// Gets a list of all appointment fields for the specified appointment.
        /// </summary>
        /// <param name="appointmentId">The ID of the appointment.</param>
        /// <returns>A list of appointment fields.</returns>
        public static List<ApptField> GetByAppointment(long appointmentId) =>
            SelectMany("SELECT * FROM `appointment_fields` WHERE `appointment_id` = " + appointmentId, FromReader);

        /// <summary>
        /// Inserts the specified appointment field into the database.
        /// </summary>
        /// <param name="apptField"></param>
        /// <returns></returns>
        public static long Insert(ApptField apptField) =>
            apptField.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `appointment_fields` (`appointment_id`, `field_name`, `value`) WHERE (?appointment_id, ?field_name, ?value)",
                    new MySqlParameter("?appointment_id", apptField.AppointmentId),
                    new MySqlParameter("?field_name", apptField.FieldName),
                    new MySqlParameter("?value", apptField.Value));

        /// <summary>
        /// Updates the specified appointment field in the database.
        /// </summary>
        /// <param name="apptField">The appointment field.</param>
        public static void Update(ApptField apptField) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `appointment_fields` SET `appointment_id` = ?appointment_id, `field_name` = ?field_name, `value` = ?value WHERE `id` = ?id",
                    new MySqlParameter("?appointment_id", apptField.AppointmentId),
                    new MySqlParameter("?field_name", apptField.FieldName),
                    new MySqlParameter("?value", apptField.Value),
                    new MySqlParameter("?id", apptField.Id));

        /// <summary>
        /// Deletes the specified appointment field.
        /// </summary>
        /// <param name="apptFieldId"></param>
        public static void Delete(long apptFieldId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `appointment_fields` WHERE `id` = " + apptFieldId);
    }
}
