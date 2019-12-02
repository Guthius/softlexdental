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
    ///     <para>
    ///         Represents dynamic informational items that are drawn as parts of appointments in 
    ///         the scheduling module.
    ///     </para>
    ///     <para>
    ///         The type of information displayed by the item is determined by the
    ///         <see cref="OperatoryId"/>, <see cref="ProviderId"/>, 
    ///         <see cref="AppointmentFieldDefinitionId"/>, <see cref="PatientFieldDefinitionId"/> 
    ///         and <see cref="Description"/> fields. Only one of the ID fields may be set
    ///         per item. If all the ID fields are null, the item represents a build-in field and
    ///         <see cref="Description"/> will hold the name of the field.
    ///     </para>
    /// </summary>
    public class AppointmentViewItem : DataRecord
    {
        public long AppointmentViewId;
        public long? OperatoryId;
        public long? ProviderId;
        public long? AppointmentFieldDefinitionId;
        public long? PatientFieldDefinitionId;

        /// <summary>
        /// Must be one of the hard coded strings picked from the available list.
        /// </summary>
        public string Description;

        /// <summary>
        /// The sort order of the item.
        /// </summary>
        public int Order;

        /// <summary>
        /// The color of the item.
        /// </summary>
        public Color Color = Color.Black;

        /// <summary>
        /// The location of the item within the appointment.
        /// </summary>
        public AppointmentViewLocation Location = AppointmentViewLocation.Main;

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
                Location = (AppointmentViewLocation)(int)dataReader["location"]
            };
        }

        /// <summary>
        /// Gets all appointment view items for the appointment view with the specified ID.
        /// </summary>
        /// <param name="appointmentViewId">The ID of the appointment view.</param>
        /// <returns>The appointment view items.</returns>
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
                "`patient_field_definition_id`, `description`, `order`, `color`, `location`) VALUES (?appointment_view_id, ?operatory_id, ?provider_id, " +
                "?appointment_field_definition_id, ?patient_field_definition_id, ?description, ?order, ?color, ?location)",
                    new MySqlParameter("appointment_view_id", appointmentViewItem.AppointmentViewId),
                    new MySqlParameter("operatory_id", ValueOrDbNull(appointmentViewItem.OperatoryId)),
                    new MySqlParameter("provider_id", ValueOrDbNull(appointmentViewItem.ProviderId)),
                    new MySqlParameter("appointment_field_definition_id", ValueOrDbNull(appointmentViewItem.AppointmentFieldDefinitionId)),
                    new MySqlParameter("patient_field_definition_id", ValueOrDbNull(appointmentViewItem.PatientFieldDefinitionId)),
                    new MySqlParameter("description", appointmentViewItem.Description ?? ""),
                    new MySqlParameter("order", appointmentViewItem.Order),
                    new MySqlParameter("color", ColorTranslator.ToHtml(appointmentViewItem.Color)),
                    new MySqlParameter("location", (int)appointmentViewItem.Location));

        /// <summary>
        /// Updates the specified appointment view item in the database.
        /// </summary>
        /// <param name="appointmentViewItem">The appointment view item.</param>
        public static void Update(AppointmentViewItem appointmentViewItem) =>
           DataConnection.ExecuteNonQuery(
               "UPDATE `appointment_view_items` SET `appointment_view_id` = ?appointment_view_id, `operatory_id` = ?operatory_id, `provider_id` = ?provider_id, " +
               "`appointment_field_definition_id` = ?appointment_field_definition_id, `patient_field_definition_id` = ?patient_field_definition_id, " +
               "`description` = ?description, `order` = ?order, `color` = ?color, `location` = ?location WHERE `id` = ?id",
                   new MySqlParameter("appointment_view_id", appointmentViewItem.AppointmentViewId),
                   new MySqlParameter("operatory_id", ValueOrDbNull(appointmentViewItem.OperatoryId)),
                   new MySqlParameter("provider_id", ValueOrDbNull(appointmentViewItem.ProviderId)),
                   new MySqlParameter("appointment_field_definition_id", ValueOrDbNull(appointmentViewItem.AppointmentFieldDefinitionId)),
                   new MySqlParameter("patient_field_definition_id", ValueOrDbNull(appointmentViewItem.PatientFieldDefinitionId)),
                   new MySqlParameter("description", appointmentViewItem.Description ?? ""),
                   new MySqlParameter("order", appointmentViewItem.Order),
                   new MySqlParameter("color", ColorTranslator.ToHtml(appointmentViewItem.Color)),
                   new MySqlParameter("location", (int)appointmentViewItem.Location),
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
