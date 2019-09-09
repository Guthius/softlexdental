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

namespace OpenDentBusiness
{
    /// <summary>
    /// Used on employee time cards to make adjustments. 
    /// Used to make the end-of-the week overtime entries. 
    /// Can be used instead of a clock event by a administrator so that a clock event doesn't have to be created.
    /// </summary>
    public class TimeAdjustment : DataRecord, IDateTimeStamped
    {
        public long ClinicId;
        public long EmployeeId;

        /// <summary>
        /// The date and time that this entry will show on timecard.
        /// </summary>
        public DateTime Date;

        /// <summary>
        /// The number of regular hours to adjust timecard by.  Can be + or -.
        /// </summary>
        public TimeSpan HoursRegular;

        /// <summary>
        /// Overtime hours. Usually +.  Automatically combined with a - adj to <see cref="HoursRegular"/>.  Another option is clockevent.OTimeHours.
        /// </summary>
        public TimeSpan HoursOvertime;

        /// <summary>
        /// Optional note for the adjustment.
        /// </summary>
        public string Note;

        /// <summary>
        /// Set to true if this adjustment was automatically made by the system.
        /// When the calc weekly OT tool is run, these types of adjustments are fair game for deletion.
        /// Other adjustments are preserved.
        /// </summary>
        public bool IsAuto;

        /// <summary>
        /// Gets the date and time of the time adjustment.
        /// </summary>
        public DateTime GetDateTime() => Date;

        static TimeAdjustment FromReader(MySqlDataReader dataReader)
        {
            return new TimeAdjustment
            {
                Id = Convert.ToInt64(dataReader["id"])
            };
        }

        public TimeAdjustment Copy() => (TimeAdjustment)MemberwiseClone();

        public static long Insert(TimeAdjustment timeAdjustment) =>
            timeAdjustment.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `time_adjustments` (`employee_id`, `clinic_id`, `date`, `hours_regular`, `hours_overtime`, `note`, `is_auto`) " +
                "VALUES (?employee_id, ?clinic_id, ?date, ?hours_regular, ?hours_overtime, ?note, ?is_auto)",
                    new MySqlParameter("employee_id", timeAdjustment.EmployeeId),
                    new MySqlParameter("clinic_id", timeAdjustment.ClinicId),
                    new MySqlParameter("date", timeAdjustment.Date),
                    new MySqlParameter("hours_regular", timeAdjustment.HoursRegular),
                    new MySqlParameter("hours_overtime", timeAdjustment.HoursOvertime),
                    new MySqlParameter("note", timeAdjustment.Note ?? ""),
                    new MySqlParameter("is_auto", timeAdjustment.IsAuto));

        public static void Update(TimeAdjustment timeAdjustment) =>
             DataConnection.ExecuteNonQuery(
                "UPDATE `time_adjustments` SET `employee_id` = ?employee_id, `clinic_id` = ?clinic_id, `date` = ?date, `hours_regular` = ?hours_regular, " +
                 "`hours_overtime` = ?hours_overtime, `note` = ?note, `is_auto` = ?is_auto WHERE `id` = ?id",
                    new MySqlParameter("employee_id", timeAdjustment.EmployeeId),
                    new MySqlParameter("clinic_id", timeAdjustment.ClinicId),
                    new MySqlParameter("date", timeAdjustment.Date),
                    new MySqlParameter("hours_regular", timeAdjustment.HoursRegular),
                    new MySqlParameter("hours_overtime", timeAdjustment.HoursOvertime),
                    new MySqlParameter("note", timeAdjustment.Note ?? ""),
                    new MySqlParameter("is_auto", timeAdjustment.IsAuto),
                    new MySqlParameter("id", timeAdjustment.Id));

        public static void Delete(TimeAdjustment timeAdjustment) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `time_adjustments` WHERE `id` = " + timeAdjustment.Id);

        public static TimeAdjustment GetPayPeriodNote(long employeeId, DateTime startDate) =>
            SelectOne("SELECT * FROM time_adjustments WHERE employee_id = ?employee_id AND time_entry = ?date AND is_auto = 0", FromReader,
                new MySqlParameter("employee_id", employeeId),
                new MySqlParameter("date", startDate));
        
        public static TimeAdjustment GetPayPeriodNote(DateTime startDate) =>
            SelectOne("SELECT * FROM time_adjustments WHERE time_entry = ?date AND is_auto = 0", FromReader,
                new MySqlParameter("date", startDate));

        public static List<TimeAdjustment> Refresh(long employeeId, DateTime fromDate, DateTime toDate) =>
            SelectMany("SELECT * FROM `time_adjustments` WHERE `employee_id` = ?employee_id AND DATE(`date`) >= ?from_date AND DATE(`date`) <= ?to_date", FromReader,
                new MySqlParameter("employee_id", employeeId),
                new MySqlParameter("from_date", fromDate),
                new MySqlParameter("to_date", toDate));

        public static List<TimeAdjustment> GetValidList(long employeeId, DateTime fromDate, DateTime toDate) =>
            SelectMany("SELECT * FROM `time_adjustments` WHERE `employee_id` = ?employee_id AND DATE(`date`) >= ?from_date AND DATE(`date`) <= ?to_date ORDER BY `date`", FromReader,
                new MySqlParameter("employee_id", employeeId),
                new MySqlParameter("from_date", fromDate),
                new MySqlParameter("to_date", toDate));
        

        public static List<TimeAdjustment> GetListForTimeCardManage(long employeeId, long clinicId, DateTime fromDate, DateTime toDate, bool isAll) =>
            SelectMany(
                "SELECT * FROM `time_adjustments` WHERE `employee_id` = ?employee_id AND DATE(`date`) >= ?from_date AND DATE(`date`) <= ?to_date AND (?clinic_id = 0 OR `clinic_id` = ?clinic_id) ORDER BY `date`", FromReader,
                    new MySqlParameter("employee_id", employeeId),
                    new MySqlParameter("clinic_id", isAll ? 0 : clinicId),
                    new MySqlParameter("from_date", fromDate),
                    new MySqlParameter("to_date", toDate));
        
        public static List<TimeAdjustment> GetSimpleListAuto(long employeeId, DateTime fromDate, DateTime toDate) =>
            SelectMany(
                "SELECT * FROM `time_adjustments` WHERE `employee_id` = ?employee_id AND `is_auto` = 1 AND DATE(`date`) >= ?from_date AND DATE(`date`) < ?to_date", FromReader,
                    new MySqlParameter("employee_id", employeeId),
                    new MySqlParameter("from_date", fromDate),
                    new MySqlParameter("to_date", toDate.AddDays(1)));
    }
}