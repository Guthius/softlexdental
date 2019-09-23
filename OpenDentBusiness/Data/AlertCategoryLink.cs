/**
 * Softlex Dental Project
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
using System.Linq;

namespace OpenDentBusiness
{
    public class AlertCategoryLink : DataRecordBase
    {
        public long AlertCategoryId;
        public string AlertType;

        static AlertCategoryLink FromReader(MySqlDataReader dataReader)
        {
            return new AlertCategoryLink
            {
                AlertCategoryId = (long)dataReader["alert_category_id"],
                AlertType = (string)dataReader["alert_type"]
            };
        }

        public AlertCategoryLink()
        {
        }

        public AlertCategoryLink(long alertCategoryId, string alertType)
        {
            AlertCategoryId = alertCategoryId;
            AlertType = alertType;
        }

        public static List<AlertCategoryLink> GetByAlertCategory(long alertCategoryId) =>
            SelectMany("SELECT * FROM `alert_category_links` WHERE `alert_category_id` = " + alertCategoryId, FromReader);

        public static void Insert(AlertCategoryLink alertCategoryLink) =>
            DataConnection.ExecuteNonQuery(
                "INSERT IGNORE INTO `alert_category_links` (`alert_category_id`, `alert_type`) VALUES (?alert_category_id, ?alert_type)",
                    new MySqlParameter("alert_category_id", alertCategoryLink.AlertCategoryId),
                    new MySqlParameter("alert_type", alertCategoryLink.AlertType ?? ""));

        public static void Delete(AlertCategoryLink alertCategoryLink) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `alert_category_links` WHERE `alert_category_id` = ?alert_category_id AND alert_type = ?alert_type",
                    new MySqlParameter("alert_category_id", alertCategoryLink.AlertCategoryId), 
                    new MySqlParameter("alert_type",alertCategoryLink.AlertType ?? ""));

        public static void Synchronize(List<AlertCategoryLink> newAlertCategoryLinks, List<AlertCategoryLink> oldAlertCategoryLinks)
        {
            foreach (var oldAlertCategoryLink in oldAlertCategoryLinks)
            {
                var count = 
                    newAlertCategoryLinks.Count(
                        newAlertCategoryLink =>
                            newAlertCategoryLink.AlertCategoryId == oldAlertCategoryLink.AlertCategoryId &&
                            newAlertCategoryLink.AlertType == oldAlertCategoryLink.AlertType);

                if (count == 0)
                {
                    Delete(oldAlertCategoryLink);
                }
            }

            foreach (var newAlertCategoryLink in newAlertCategoryLinks) Insert(newAlertCategoryLink);
        }
    }
}
