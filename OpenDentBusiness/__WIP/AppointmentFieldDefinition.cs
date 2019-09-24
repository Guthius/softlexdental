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
    /// These are the definitions for the custom patient fields added and managed by the user.
    /// </summary>
    public class AppointmentFieldDefinition : DataRecord
    {
        private static readonly DataRecordCache<AppointmentFieldDefinition> cache = new DataRecordCache<AppointmentFieldDefinition>(
            "SELECT * FROM `appointment_field_defintions` ORDER BY `name`", FromReader);

        /// <summary>
        /// The name of the field that the user will be allowed to fill in the appt edit window.  Duplicates are prevented.
        /// </summary>
        public string FieldName;

        /// <summary>
        /// Enum:ApptFieldType Text=0,PickList=1
        /// </summary>
        public AppointmentFieldType FieldType;

        /// <summary>
        /// The text that contains pick list values.  Length 4000.
        /// </summary>
        public string PickList;

        /// <summary>
        /// Constructs a new instance of the <see cref="AppointmentFieldDefinition"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="AppointmentFieldDefinition"/> instance.</returns>
        private static AppointmentFieldDefinition FromReader(MySqlDataReader dataReader)
        {
            return new AppointmentFieldDefinition
            {
                Id = (long)dataReader["id"],
                FieldName = (string)dataReader["name"],
                FieldType = (AppointmentFieldType)Convert.ToInt32(dataReader["type"]),
                PickList = (string)dataReader["pick_list"]
            };
        }

        /// <summary>
        /// Gets a list of all appointment field definitions.
        /// </summary>
        /// <returns>A list of appointment field definitions.</returns>
        public static List<AppointmentFieldDefinition> All() =>
            cache.All().ToList();

        /// <summary>
        /// Updates the specified appointment field defintiion in the database.
        /// </summary>
        /// <param name="apptFieldDef">The appointment field defintion.</param>
        /// <exception cref="Exception">If the name of the field is already in use by another field.</exception>
        public static void Update(AppointmentFieldDefinition apptFieldDef)
        {
            var count = DataConnection.ExecuteLong(
                "SELECT COUNT(*) FROM `appointment_field_defintions` " +
                "WHERE `name` = '" + MySqlHelper.EscapeString(apptFieldDef.FieldName) + "' " +
                "AND `id` != " + apptFieldDef.Id);

            if (count > 0)
                throw new Exception("Field name already in use.");

            DataConnection.ExecuteNonQuery(
                "UPDATE `appointment_field_definitions` SET `name` = ?name, `type` = ?type, " +
                "`pick_list` = ?pick_list WHERE `id` = ?id",
                    new MySqlParameter("name", apptFieldDef.FieldName ?? ""),
                    new MySqlParameter("type", (int)apptFieldDef.FieldType),
                    new MySqlParameter("pick_list", apptFieldDef.PickList ?? ""),
                    new MySqlParameter("id", apptFieldDef.Id));
        }

        /// <summary>
        /// Inserts the specified appointment field definition into the datbase.
        /// </summary>
        /// <param name="apptFieldDef">The appointment field defintion.</param>
        /// <returns>The ID assigned to the appointment field definition.</returns>
        /// <exception cref="Exception">If the name of the field is already in use by another field.</exception>
        public static long Insert(AppointmentFieldDefinition apptFieldDef)
        {
            var count = DataConnection.ExecuteLong(
                "SELECT COUNT(*) FROM `appointment_field_defintions` " +
                "WHERE `name` = '" + MySqlHelper.EscapeString(apptFieldDef.FieldName) + "'");

            if (count > 0)
                throw new Exception("Field name already in use.");

            return apptFieldDef.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `appointment_field_definitions` (`name`, `type`, `pick_list`) VALUES (?name, ?type, ?pick_list)",
                    new MySqlParameter("name", apptFieldDef.FieldName ?? ""),
                    new MySqlParameter("type", (int)apptFieldDef.FieldType),
                    new MySqlParameter("pick_list", apptFieldDef.PickList ?? ""));
        }

        /// <summary>
        /// Deletes the specified appointment field definition from the database.
        /// </summary>
        /// <param name="apptFieldDef">The appointment field definition.</param>
        /// <exception cref="Exception">If the appointment field definition is use and cannot be deleted.</exception>
        public static void Delete(AppointmentFieldDefinition apptFieldDef)
        {
            var commandText =
                "SELECT `lastname`, `firstname`, `appointment_date` " +
                "FROM `patients`, `appointment_fields`, `appointments` " +
                "WHERE `patients`.`id` = `appointments`.`patient_id` " +
                "AND `appointments`.`id` = `appointment_fields`.`appointment_id` " +
                "AND `field_name` = '" + MySqlHelper.EscapeString(apptFieldDef.FieldName) + "'";

            using (var table = DataConnection.ExecuteDataTable(commandText))
            {
                if (table.Rows.Count > 0)
                {
                    var message = "Unable to delete field definition. Already in use by " + table.Rows.Count.ToString() + " appointments, including\r\n";

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        if (i > 5) break;
                        
                        var appointmentdate = (DateTime)table.Rows[i]["appointment_date"];

                        message += 
                            table.Rows[i]["lastname"].ToString() + ", " + 
                            table.Rows[i]["firstname"].ToString() + 
                            appointmentdate.ToString() + "\r\n";
                    }

                    throw new Exception(message);
                }
            }

            DataConnection.ExecuteNonQuery(
                "DELETE FROM `appointment_field_definitions` WHERE `id` = " + apptFieldDef.Id);
        }

        /// <summary>
        /// Gets the name of the field with the specified ID.
        /// </summary>
        /// <param name="apptFieldDefId">The ID of the field.</param>
        /// <returns>The name of the field with the specified id.</returns>
        public static string GetFieldName(long apptFieldDefId) =>
            cache.SelectOne(apptFieldDef => apptFieldDef.Id == apptFieldDefId)?.FieldName ?? "";

        /// <summary>
        /// Gets the appointment field definition with the specified field name.
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        /// <returns>The field definition with the specified name.</returns>
        public static AppointmentFieldDefinition GetByFieldName(string fieldName) => 
            cache.SelectOne(apptFieldDef => apptFieldDef.FieldName == fieldName);

        /// <summary>
        /// Gets the picklist of the field definition with the specified field name.
        /// </summary>
        public static string GetPickListByFieldName(string fieldName) =>
            GetByFieldName(fieldName)?.PickList ?? "";
    }


}
