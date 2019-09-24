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
using System.Drawing;
using System.Linq;

namespace OpenDentBusiness
{
    /// <summary>
    /// Appointment type is used to override appointment color. 
    /// Might control other properties on appointments in the future.
    /// </summary>
    public class AppointmentType : DataRecord
    {
        private static DataRecordCache<AppointmentType> cache = 
            new DataRecordCache<AppointmentType>("SELECT * FROM `appointment_types`", FromReader);

        /// <summary>
        /// The name of the appoitment type.
        /// </summary>
        public string Name;

        /// <summary>
        /// The color to use when dispaying apppointments of this type.
        /// </summary>
        public Color Color;

        /// <summary>
        /// <para>Time pattern, 'X' for doctor time, '/' for assist time.</para>
        /// <para>
        /// Stored in 5 minute increments. Convert as needed to 10 or 15 minute representations for
        /// display. Will be blank if the pattern should be dynamically calculated via the 
        /// procedures specified by <see cref="ProcedureCodes"/>.
        /// </para>
        /// </summary>
        public string Pattern;

        /// <summary>
        /// Comma delimited list of procedure codes.  E.g. T1234,T4321,N3214
        /// </summary>
        public string ProcedureCodes;


        public int SortOrder;

        /// <summary>
        /// A value indicating whether the appointment type has been hidden.
        /// </summary>
        public bool Hidden;

        /// <summary>
        /// Constructs a new instance of the <see cref="AppointmentType"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="AppointmentType"/> instance.</returns>
        private static AppointmentType FromReader(MySqlDataReader dataReader)
        {
            return new AppointmentType
            {
                Id = (long)dataReader["id"],
                Name = (string)dataReader["name"],
                Color = ColorTranslator.FromHtml((string)dataReader["color"]),
                Pattern = (string)dataReader["pattern"],
                ProcedureCodes = (string)dataReader["procedure_codes"],
                SortOrder = Convert.ToInt32(dataReader["sort_order"]),
                Hidden = Convert.ToBoolean(dataReader["hidden"])
            };
        }

        /// <summary>
        /// Gets a list of all appointment types.
        /// </summary>
        /// <returns>A list of appointment types.</returns>
        public static List<AppointmentType> All() =>
            cache.All().ToList();

        /// <summary>
        /// Gets the name of the specified appointment type. Appends ' (hidden)'  to the end of a
        /// appointment type name when the appointment type has been hidden.
        /// </summary>
        /// <param name="appointmentTypeId">The ID of the appointment type.</param>
        /// <returns>The name of the appointment type.</returns>
        public static string GetName(long appointmentTypeId)
        {
            var appointmentType = GetById(appointmentTypeId);

            if (appointmentType != null)
            {
                return appointmentType.Name + (appointmentType.Hidden ? " (hidden)" : "");
            }

            return "";
        }

        /// <summary>
        /// Returns the time pattern for the specified appointment type (time pattern returned will always be in 5 min increments).
        /// If the Pattern variable is not set on the appointment type object then the pattern will be dynamically calculated.
        /// Optionally pass in provider information in order to use specific provider time patterns.
        /// </summary>
        public static string GetTimePatternForAppointmentType(AppointmentType appointmentType, long provNumDentist = 0, long provNumHyg = 0)
        {
            string timePattern = "";
            if (string.IsNullOrEmpty(appointmentType.Pattern))
            {
                // Dynamically calculate the timePattern from the procedure codes associated to the appointment type passed in.
                List<string> listProcCodeStrings = appointmentType.ProcedureCodes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                List<ProcedureCode> listProcCodes = OpenDentBusiness.ProcedureCodes.GetProcCodes(listProcCodeStrings);

                timePattern = Appointments.CalculatePattern(provNumDentist, provNumHyg, listProcCodes.Select(x => x.CodeNum).ToList(), true);
            }
            else
            {
                timePattern = appointmentType.Pattern; // Already in 5 minute increment so no conversion required.
            }
            return timePattern;
        }

        /// <summary>
        /// Returns the appointment type associated to the definition passed in.
        /// Returns null if no match found.
        /// </summary>
        public static AppointmentType GetWebSchedNewPatApptTypeByDef(long defNum)
        {
            List<DefLink> listDefLinks = DefLinks.GetDefLinksByType(DefLinkType.AppointmentType);

            DefLink defLink = listDefLinks.FirstOrDefault(x => x.DefNum == defNum);
            if (defLink == null)
            {
                return null;
            }

            return cache.SelectOne(appointmentType => appointmentType.Id == defLink.FKey);
        }

        /// <summary>
        /// Gets the appointment type with the specified ID.
        /// </summary>
        /// <param name="appointmentTypeId">The ID of the appointment type.</param>
        /// <returns></returns>
        public static AppointmentType GetById(long appointmentTypeId) =>
            cache.SelectOne(appointmentType => appointmentType.Id == appointmentTypeId);

        /// <summary>
        /// Inserts the specified appointment type into the database.
        /// </summary>
        /// <param name="appointmentType">The appointment type.</param>
        /// <returns>THe ID assigned to the appointment type.</returns>
        public static long Insert(AppointmentType appointmentType) =>
            appointmentType.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `appointment_types` (`color`, `pattern`, `procedure_codes`, `sort_order`, `hidden`) " +
                "VALUES (?color, ?pattern, ?procedure_codes, ?sort_order, ?hidden)",
                    new MySqlParameter("color", ColorTranslator.ToHtml(appointmentType.Color)),
                    new MySqlParameter("pattern", appointmentType.Pattern ?? ""),
                    new MySqlParameter("procedure_codes", appointmentType.ProcedureCodes ?? ""),
                    new MySqlParameter("sort_order", appointmentType.SortOrder),
                    new MySqlParameter("hidden", appointmentType.Hidden));

        /// <summary>
        /// Updates the specified appoinment type in the datbase.
        /// </summary>
        /// <param name="appointmentType">The appointment type.</param>
        public static void Update(AppointmentType appointmentType) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `appointment_types` SET `color` = ?color, `pattern` = ?pattern, `procedure_codes` = ?procedure_codes, " +
                "`sort_order` = ?sort_order, `hidden` = ?hidden WHERE `id` = ?id",
                    new MySqlParameter("color", ColorTranslator.ToHtml(appointmentType.Color)),
                    new MySqlParameter("pattern", appointmentType.Pattern ?? ""),
                    new MySqlParameter("procedure_codes", appointmentType.ProcedureCodes ?? ""),
                    new MySqlParameter("sort_order", appointmentType.SortOrder),
                    new MySqlParameter("hidden", appointmentType.Hidden),
                    new MySqlParameter("id", appointmentType.Id));

        /// <summary>
        /// Deletes the specified appointment type from the database.
        /// </summary>
        /// <exception cref="Exception">If the appointment type is in use and cannot be deleted.</exception>
        public static void Delete(long appointmentTypeId)
        {
            var count = DataConnection.ExecuteLong(
                "SELECT COUNT(*) FROM `appointments` WHERE `appointment_type_id` = " + appointmentTypeId);

            if (count > 0)
                throw new Exception("Not allowed to delete appointment types that are in use on an appointment.");

            count = DataConnection.ExecuteLong(
                "SELECT COUNT(*) FROM `definition_links` " +
                "WHERE `link_type` = " + (int)DefLinkType.AppointmentType + " " +
                "AND `external_id` = " + appointmentTypeId);

            if (count > 0)
                throw new Exception("Not allowed to delete appointment types that are in use by Web Sched New Pat Appt Types definitions.");

            DataConnection.ExecuteNonQuery(
                "DELETE FROM `appointment_types` WHERE `id` = " + appointmentTypeId);
        }

        public static int SortItemOrder(AppointmentType a, AppointmentType b)
        {
            if (a.SortOrder != b.SortOrder)
            {
                return a.SortOrder.CompareTo(b.SortOrder);
            }
            return a.Id.CompareTo(b.Id);
        }

        public static bool Compare(AppointmentType a1, AppointmentType a2)
        {
            return 
                a1.Color == a2.Color &&
                a1.Name == a2.Name &&
                a1.Hidden == a2.Hidden &&
                a1.SortOrder == a2.SortOrder;
        }
    }
}
