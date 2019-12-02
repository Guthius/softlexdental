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
using System.Drawing;

namespace OpenDentBusiness
{
    /// <summary>
    /// Each item is attached to a row in the apptview table.  Each item specifies ONE of: OpNum, ProvNum, ElementDesc, or ApptFieldDefNum.  
    /// The other three will be 0 or "".
    /// </summary>
    public class AppointmentViewItem : DataRecord
    {
        // TODO: Cache this and link cache to AppointmentView...

        public long AppointmentViewId;
        public long? OperatoryId;
        public long? ProviderId;

        /// <summary>
        /// Must be one of the hard coded strings picked from the available list.
        /// </summary>
        public string Description;

        /// <summary>
        /// If this is a row Element, then this is the 0-based order within its area.  For example, UR starts over with 0 ordering.
        /// </summary>
        public int Order;

        /// <summary>
        /// If this is an element, then this is the color.
        /// </summary>
        public Color Color = Color.Black;

        /// <summary>
        /// Enum:ApptViewAlignment If this is an element, then this is the alignment of the element within the appointment.
        /// </summary>
        public AppointmentViewLocation Alignment = AppointmentViewLocation.Main;

        /// <summary>
        /// FK to apptfielddef.ApptFieldDefNum.  If this is an element, and the element is an appt field, then this tells us which one.
        /// </summary>
        public long? AppointmentFieldDefinitionId;

        /// <summary>
        /// FK to patfielddef.PatFieldDefNum.  If this is an element, and the element is an appt field, then this tells us which one.  Not implemented yet.
        /// </summary>
        public long? PatientFieldDefinitionId;

        /// <summary>
        /// Returns a string representation of the appointment view item.
        /// </summary>
        public override string ToString() => Description ?? "";

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentViewItem"/> class.
        /// </summary>
        public AppointmentViewItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentViewItem"/> class.
        /// </summary>
        /// <param name="description">A description of the item.</param>
        /// <param name="order">The sort order of the item.</param>
        /// <param name="color">The color.</param>
        public AppointmentViewItem(string description, int order, Color color)
        {
            Description = description;
            Order = order;
            Color = color;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="AppointmentViewItem"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="AppointmentViewItem"/> instance.</returns>
        public static AppointmentViewItem FromReader(MySqlDataReader dataReader)
        {
            return new AppointmentViewItem
            {
                Id = (long)dataReader["id"],
                AppointmentViewId = (long)dataReader["appointment_view_id"],
                OperatoryId = dataReader["operatory_id"] as long?,
                ProviderId = dataReader["provider_id"] as long?,
                AppointmentFieldDefinitionId = dataReader["appointment_field_definition_id"] as long?,
                PatientFieldDefinitionId = dataReader["patient_field_definition_id"] as long?,
                Description = (string)dataReader["description"],
                Order = (int)dataReader["order"],
                Color = ColorTranslator.FromHtml((string)dataReader["color"]),
                Alignment = (AppointmentViewLocation)(int)dataReader["alignment"]
            };
        }

        public static IEnumerable<AppointmentViewItem> GetByAppointmentView(long appointmentViewId) =>
            SelectMany("SELECT * FROM `appointment_view_items` WHERE `appointment_view_id` = " + appointmentViewId, FromReader);

        /// <summary>
        /// Inserts the specified appointment view item into the database.
        /// </summary>
        /// <param name="appointmentViewItem">The appointment view item.</param>
        /// <returns>The ID assigned to the appointment view item.</returns>
        public static long Insert(AppointmentViewItem appointmentViewItem) =>
            appointmentViewItem.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `appointment_view_items` (`appointment_view_id`, `operatory_id`, `provider_id`, `appointment_field_definition_id`, " +
                "`patient_field_definition_id`, `description`, `order`, `color`, `alignment`) VALUES (?appointment_view_id, ?operatory_id, ?provider_id, " +
                "?appointment_field_definition_id, ?patient_field_definition_id, ?description, ?order, ?color, ?alignment)",
                    new MySqlParameter("appointment_view_id", appointmentViewItem.AppointmentViewId),
                    new MySqlParameter("operatory_id", ValueOrDbNull(appointmentViewItem.OperatoryId)),
                    new MySqlParameter("provider_id", ValueOrDbNull(appointmentViewItem.ProviderId)),
                    new MySqlParameter("appointment_field_definition_id", ValueOrDbNull(appointmentViewItem.AppointmentFieldDefinitionId)),
                    new MySqlParameter("patient_field_definition_id", ValueOrDbNull(appointmentViewItem.PatientFieldDefinitionId)),
                    new MySqlParameter("description", appointmentViewItem.Description ?? ""),
                    new MySqlParameter("order", appointmentViewItem.Order),
                    new MySqlParameter("color", ColorTranslator.ToHtml(appointmentViewItem.Color)),
                    new MySqlParameter("alignment", (int)appointmentViewItem.Alignment));

        /// <summary>
        /// Updates the specified appointment view item in the database.
        /// </summary>
        /// <param name="appointmentViewItem">The appointment view item.</param>
        public static void Update(AppointmentViewItem appointmentViewItem) =>
           DataConnection.ExecuteNonQuery(
               "UPDATE `appointment_view_items` SET `appointment_view_id` = ?appointment_view_id, `operatory_id` = ?operatory_id, `provider_id` = ?provider_id, " +
               "`appointment_field_definition_id` = ?appointment_field_definition_id, `patient_field_definition_id` = ?patient_field_definition_id, " +
               "`description` = ?description, `order` = ?order, `color` = ?color, `alignment` = ?alignment WHERE `id` = ?id",
                   new MySqlParameter("appointment_view_id", appointmentViewItem.AppointmentViewId),
                   new MySqlParameter("operatory_id", ValueOrDbNull(appointmentViewItem.OperatoryId)),
                   new MySqlParameter("provider_id", ValueOrDbNull(appointmentViewItem.ProviderId)),
                   new MySqlParameter("appointment_field_definition_id", ValueOrDbNull(appointmentViewItem.AppointmentFieldDefinitionId)),
                   new MySqlParameter("patient_field_definition_id", ValueOrDbNull(appointmentViewItem.PatientFieldDefinitionId)),
                   new MySqlParameter("description", appointmentViewItem.Description ?? ""),
                   new MySqlParameter("order", appointmentViewItem.Order),
                   new MySqlParameter("color", ColorTranslator.ToHtml(appointmentViewItem.Color)),
                   new MySqlParameter("alignment", (int)appointmentViewItem.Alignment),
                   new MySqlParameter("id", appointmentViewItem.Id));

        /// <summary>
        /// Deletes the appointment view item with the specified ID from the database.
        /// </summary>
        /// <param name="appointmentViewItemId">The ID of the appointment view item.</param>
        public static void Delete(long appointmentViewItemId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `appointment_view_items` WHERE `id` = " + appointmentViewItemId);

        /// <summary>
        /// Deletes all appointment view items with the specified appointment view ID from the database.
        /// </summary>
        /// <param name="appointmentViewId">The ID of the appointment view.</param>
        public static void DeleteForAppointmentView(long appointmentViewId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `appointment_view_items` WHERE `appointment_view_id` = " + appointmentViewId);
    }
}
