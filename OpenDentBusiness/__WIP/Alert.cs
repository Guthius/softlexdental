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
    public class Alert : DataRecord
    {
        public long? UserId;
        public long? ClinicId;

        /// <summary>
        /// The alert type.
        /// </summary>
        public string Type;

        /// <summary>
        /// A description of the alert.
        /// </summary>
        public string Description;

        /// <summary>
        /// A detailed description of the alert (optional).
        /// </summary>
        public string Details;

        /// <summary>
        /// The severity of the alert.
        /// </summary>
        public AlertSeverityType Severity;

        /// <summary>
        /// Flags indicatign the actions that are available for the alert.
        /// </summary>
        public AlertActionType Actions;

        /// <summary>
        /// Constructs a new instance of the <see cref="AlertRead"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="AlertRead"/> instance.</returns>
        private static Alert FromReader(MySqlDataReader dataReader)
        {
            return new Alert
            {
                Id = (long)dataReader["id"],
                UserId = dataReader["user_id"] as long?,
                ClinicId = dataReader["clinic_id"] as long?,
                Type = (string)dataReader["type"],
                Description = (string)dataReader["description"],
                Details = (string)dataReader["details"],
                Severity = (AlertSeverityType)Convert.ToInt32(dataReader["severity"]),
                Actions = (AlertActionType)Convert.ToInt32(dataReader["actions"]),
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is Alert alertItem)
            {
                return
                    alertItem.Id == Id &&
                    alertItem.ClinicId == ClinicId &&
                    alertItem.Description == Description &&
                    alertItem.Type == Type &&
                    alertItem.Severity == Severity &&
                    alertItem.Actions == Actions;
            }
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Inserts a generic alert where description will show in the menu item and itemValue will 
        /// be shown within a MsgBoxCopyPaste. Set itemValue to more specific reason for the alert.
        /// E.g. exception text details as to help the techs give better support.
        /// </summary>
        public static void CreateGenericAlert(string description, string itemValue)
        {
            Insert(new Alert
            {
                Actions = AlertActionType.MarkAsRead | AlertActionType.Delete | AlertActionType.ShowItemValue,
                Description = description,
                Severity = AlertSeverityType.Low,
                Details = itemValue
            });
        }

        /// <summary>
        /// Gets the alert with the specified ID from the database.
        /// </summary>
        /// <param name="alertId">The ID of the alert.</param>
        /// <returns>The alert with the specified ID.</returns>
        public static Alert GetById(long alertId) =>
            SelectOne("SELECT * FROM `alerts` WHERE `id` = " + alertId, FromReader);

        /// <summary>
        /// Returns a list of AlertItems for the given clinicNum.  Doesn't include alerts that are assigned to other users.
        /// </summary>
        public static List<Alert> GetByClinic(long clinicId, IEnumerable<string> types = null)
        {
            long? providerId = null;
            if (Security.CurrentUser != null && User.IsUserCPOE(Security.CurrentUser))
            {
                providerId = Security.CurrentUser.ProviderId;
            }

            if (!providerId.HasValue) return new List<Alert>();

            // TODO: Why don't we allow this when there is no CPOE provider set?

            var commandText = "SELECT * FROM `alerts` WHERE";
            if (Security.CurrentUser != null)
            {
                commandText += " (`user_id` IS NULL OR `user_id` = " + Security.CurrentUser.Id + ")";
            }

            if (types != null && types.Count() > 0)
            {
                commandText += " AND (" +
                    string.Join(" OR ",
                        types.Select(type => "`type` = '" + MySqlHelper.EscapeString(type) + "'")) + ")";
            }

            commandText += " AND `clinic_id` = " + clinicId;

            return SelectMany(commandText, FromReader);
        }

        /// <summary>
        /// Gets a list of all alerts of the specified type.
        /// </summary>
        public static List<Alert> GetByAlertType(string type) => 
            SelectMany("SELECT * FROM `alerts` WHERE `type` = '" + MySqlHelper.EscapeString(type) + "'", FromReader);

        /// <summary>
        /// Inserts the specified alert into the database.
        /// </summary>
        /// <param name="alert">The alert.</param>
        /// <returns>The ID assigned to the alert.</returns>
        public static long Insert(Alert alert) =>
            alert.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `alerts` (`user_id`, `clinic_id`, `type`, `description`, `details`, `severity`, `actions`) " +
                "VALUES (?user_id, ?clinic_id, ?type, ?description, ?details, ?severity, ?actions)",
                    new MySqlParameter("user_id", alert.UserId.HasValue ? (object)alert.UserId.Value : DBNull.Value),
                    new MySqlParameter("clinic_id", alert.ClinicId.HasValue ? (object)alert.ClinicId.Value : DBNull.Value),
                    new MySqlParameter("type", alert.Type ?? AlertType.Generic),
                    new MySqlParameter("description", alert.Description ?? ""),
                    new MySqlParameter("details", alert.Details ?? ""),
                    new MySqlParameter("severity", (int)alert.Severity),
                    new MySqlParameter("actions", (int)alert.Actions));

        /// <summary>
        /// Updates the specified alert in the database.
        /// </summary>
        /// <param name="alert">The alert.</param>
        public static void Update(Alert alert) =>
             DataConnection.ExecuteNonQuery(
                "UPDATE `alerts` SET `user_id` = ?user_id, `clinic_id` = ?clinic_id, `type` = ?type, `description` = ?description, " +
                 "`details` = ?details, `severity` = ?severity, `actions` = ?actions WHERE `id` = ?id",
                    new MySqlParameter("user_id", alert.UserId.HasValue ? (object)alert.UserId.Value : DBNull.Value),
                    new MySqlParameter("clinic_id", alert.ClinicId.HasValue ? (object)alert.ClinicId.Value : DBNull.Value),
                    new MySqlParameter("type", alert.Type ?? AlertType.Generic),
                    new MySqlParameter("description", alert.Description ?? ""),
                    new MySqlParameter("details", alert.Details ?? ""),
                    new MySqlParameter("severity", (int)alert.Severity),
                    new MySqlParameter("actions", (int)alert.Actions),
                    new MySqlParameter("id", alert.Id));

        /// <summary>
        /// Deletes all alerts of the specified type from the database.
        /// </summary>
        /// <param name="type"></param>
        public static void DeleteByType(string type) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `alerts` WHERE `type` = '" + MySqlHelper.EscapeString(type) + "'");
        
        /// <summary>
        /// Alerts the specified alert from the database.
        /// </summary>
        /// <param name="alertId">The ID of the alert.</param>
        public static void Delete(long alertId) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `alerts` WHERE `id` = " + alertId);
    }
}
