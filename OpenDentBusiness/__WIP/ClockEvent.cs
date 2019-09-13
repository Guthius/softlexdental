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
using System.Data;
using System.Linq;
using System.Text;

namespace OpenDentBusiness
{
    /// <summary>
    /// Represents a object that has been marked with a date/time stamp.
    /// </summary>
    public interface IDateTimeStamped
    {
        /// <summary>
        /// Gets the date/time stamp of the object.
        /// </summary>
        /// <returns>The date/time stamp.</returns>
        DateTime GetDateTime();
    }

    /// <summary>
    /// One clock-in / clock-out pair.  Or, if the pair is a break, then it's an out/in pair. With 
    /// normal clock in/out pairs, we want to know how long the employee was working. It's the 
    /// opposite with breaks. We want to know how long they were not working, so the pair is 
    /// backwards. This means that a normal clock in is left incomplete when the clock out for 
    /// break is created. And once both are finished, the regular in/out will surround the break.
    /// Breaks cannot be viewed easily on the same grid as regular clock events for this reason.
    /// And since breaks do not affect pay, they should not clutter the normal grid.
    /// </summary>
    public class ClockEvent : DataRecord, IDateTimeStamped
    {
        public DateTime GetDateTime() => Date1Displayed;

        public long? ClinicId;
        public long EmployeeId;

        /// <summary>
        /// The actual time that this entry was entered.
        /// </summary>
        public DateTime Date1Entered;

        /// <summary>
        /// The time to display and to use in all calculations.
        /// </summary>
        public DateTime Date1Displayed;

        public string Note;

        /// <summary>
        /// The user can never edit this, but the program has to be able to edit this when user clocks out.
        /// Null if not clocked out yet.
        /// </summary>
        public DateTime? Date2Entered;

        /// <summary>
        /// User editable clock out date. Null if not clocked out yet.
        /// </summary>
        public DateTime? Date2Displayed;

        /// <summary>
        /// This is a manual override for <see cref="OvertimeAuto"/>. 
        /// </summary>
        public TimeSpan? Overtime;

        /// <summary>
        /// Automatically calculated overtime. Will be <see cref="TimeSpan.Zero"/> if none.
        /// </summary>
        public TimeSpan OvertimeAuto;
        
        /// <summary>
        /// This is a manual override of <see cref="AdjustAuto"/>.
        /// </summary>
        public TimeSpan? Adjust;

        /// <summary>
        /// Automatically calculated Adjust. Will be <see cref="TimeSpan.Zero"/> if none.
        /// </summary>
        public TimeSpan AdjustAuto;
        
        /// <summary>
        /// This is a manual override for <see cref="Rate2Auto"/>.
        /// </summary>
        public TimeSpan? Rate2;
        
        /// <summary>
        /// Automatically calculated rate2 pay. Will be <see cref="TimeSpan.Zero"/> if none.
        /// </summary>
        public TimeSpan Rate2Auto;

        /// <summary>
        /// The status really only applies to the clock out.
        /// Except the Break status applies to both out and in.
        /// </summary>
        public ClockEventStatus Status;

        static ClockEvent FromReader(MySqlDataReader dataReader)
        {
            return new ClockEvent
            {
                Id = Convert.ToInt64(dataReader["id"]),
                ClinicId = dataReader["clinic_id"] as long?,
                EmployeeId = Convert.ToInt64(dataReader["employee_id"]),
                Date1Entered = (DateTime)dataReader["date1_entered"],
                Date1Displayed = (DateTime)dataReader["date1_displayed"],
                Note = (string)dataReader["note"],
                Date2Entered = dataReader["date2_entered"] as DateTime?,
                Date2Displayed = dataReader["date2_displayed"] as DateTime?,
                Overtime = dataReader["overtime"] as TimeSpan?,
                OvertimeAuto = (TimeSpan)dataReader["overtime_auto"],
                Adjust = dataReader["adjust"] as TimeSpan?,
                AdjustAuto = (TimeSpan)dataReader["adjust_auto"],
                Rate2 = dataReader["rate2"] as TimeSpan?,
                Rate2Auto = (TimeSpan)dataReader["rate2_auto"],
                Status = (ClockEventStatus)Convert.ToInt32(dataReader["status"])
            };
        }

        public static ClockEvent GetById(long clockEventId) =>
            SelectOne("SELECT * FROM `clock_events` WHERE `id` = " + clockEventId, FromReader);

        public static long Insert(ClockEvent clockEvent) =>
            clockEvent.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `clock_events` (`clinic_id`, `employee_id`, `date1_entered`, `date1_displayed`, `note`, `date2_entered`, `date2_displayed`, " +
                "`overtime`, `overtime_auto`, `adjust`, `adjust_auto`, `rate2`, `rate2_auto`, `status`) VALUES (?clinic_id, ?employee_id, ?date1_entered, " +
                "?date1_displayed, ?note, ?date2_entered, ?date2_displayed, ?overtime, ?overtime_auto, ?adjust, ?adjust_auto, ?rate2, " +
                "?rate2_auto, ?status)",
                    new MySqlParameter("clinic_id", clockEvent.ClinicId.HasValue ? (object)clockEvent.EmployeeId : DBNull.Value),
                    new MySqlParameter("employee_id", clockEvent.EmployeeId),
                    new MySqlParameter("date1_entered", clockEvent.Date1Entered),
                    new MySqlParameter("date1_displayed", clockEvent.Date1Displayed),
                    new MySqlParameter("note", clockEvent.Note ?? ""),
                    new MySqlParameter("date2_entered", clockEvent.Date2Entered.HasValue ? (object)clockEvent.Date2Entered : DBNull.Value),
                    new MySqlParameter("date2_displayed", clockEvent.Date2Displayed.HasValue ? (object)clockEvent.Date2Displayed : DBNull.Value),
                    new MySqlParameter("overtime", clockEvent.Overtime.HasValue ? (object)clockEvent.Overtime : DBNull.Value),
                    new MySqlParameter("overtime_auto", clockEvent.OvertimeAuto),
                    new MySqlParameter("adjust", clockEvent.Adjust.HasValue ? (object)clockEvent.Adjust : DBNull.Value),
                    new MySqlParameter("adjust_auto", clockEvent.AdjustAuto),
                    new MySqlParameter("rate2", clockEvent.Rate2.HasValue ? (object)clockEvent.Rate2 : DBNull.Value),
                    new MySqlParameter("rate2_auto", clockEvent.Rate2Auto),
                    new MySqlParameter("status", (int)clockEvent.Status));

        public static void Update(ClockEvent clockEvent) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `clock_events` SET `clinic_id` = ?clinic_id, `employee_id` = ?employee_id, `date1_entered` = ?date1_entered, " +
                "`date1_displayed` = ?date1_displayed, `note` = ?note, `date2_entered` = ?date2_entered, `date2_displayed` = ?date2_displayed, " +
                "`overtime` = ?overtime, `overtime_auto` = ?overtime_auto, `adjust` = ?adjust, `adjust_auto` = ?adjust_auto, " +
                "`rate2` = ?rate2, `rate2_auto` = ?rate2_auto, `status` = ?status WHERE `id` = ?id",
                    new MySqlParameter("clinic_id", clockEvent.ClinicId.HasValue ? (object)clockEvent.EmployeeId : DBNull.Value),
                    new MySqlParameter("employee_id", clockEvent.EmployeeId),
                    new MySqlParameter("date1_entered", clockEvent.Date1Entered),
                    new MySqlParameter("date1_displayed", clockEvent.Date1Displayed),
                    new MySqlParameter("note", clockEvent.Note ?? ""),
                    new MySqlParameter("date2_entered", clockEvent.Date2Entered.HasValue ? (object)clockEvent.Date2Entered : DBNull.Value),
                    new MySqlParameter("date2_displayed", clockEvent.Date2Displayed.HasValue ? (object)clockEvent.Date2Displayed : DBNull.Value),
                    new MySqlParameter("overtime", clockEvent.Overtime.HasValue ? (object)clockEvent.Overtime : DBNull.Value),
                    new MySqlParameter("overtime_auto", clockEvent.OvertimeAuto),
                    new MySqlParameter("adjust", clockEvent.Adjust.HasValue ? (object)clockEvent.Adjust : DBNull.Value),
                    new MySqlParameter("adjust_auto", clockEvent.AdjustAuto),
                    new MySqlParameter("rate2", clockEvent.Rate2.HasValue ? (object)clockEvent.Rate2 : DBNull.Value),
                    new MySqlParameter("rate2_auto", clockEvent.Rate2Auto),
                    new MySqlParameter("status", (int)clockEvent.Status),
                    new MySqlParameter("id", (int)clockEvent.Id));

        public static void Delete(long clockEventId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `clock_events` WHERE `id` = " + clockEventId);

        public static List<ClockEvent> Refresh(long employeeId, DateTime fromDate, DateTime toDate, bool isBreaks) =>
            SelectMany(
                "SELECT * FROM `clock_events` WHERE `employee_id` = ?employee_id " +
                "AND `date1_displayed` >= ?from_date AND `date1_displayed` < ?to_date AND `status` IN " + (isBreaks ? "(1) " : "(0, 2) ") + "" +
                "ORDER BY `date1_displayed`", FromReader,
                    new MySqlParameter("employee_id", employeeId),
                    new MySqlParameter("from_date", fromDate),
                    new MySqlParameter("to_date", toDate));

        public static List<ClockEvent> GetSimpleList(long employeeId, DateTime fromDate, DateTime toDate) =>
            SelectMany(
                "SELECT * FROM `clock_events` WHERE `employee_id` = ?employee_id " +
                "AND `date1_displayed` >= ?from_date AND `date1_displayed` < ?to_date " +
                "ORDER BY `date1_displayed`", FromReader,
                    new MySqlParameter("employee_id", employeeId),
                    new MySqlParameter("from_date", fromDate),
                    new MySqlParameter("to_date", toDate.AddDays(1)));

        /// <summary>
        /// Returns all clock events (Breaks and Non-Breaks) for all employees across all clinics. 
        /// </summary>
        public static List<ClockEvent> GetByDateRange(DateTime fromDate, DateTime toDate) =>
            SelectMany(
                "SELECT * FROM `clock_events` WHERE `time1_displayed` >= ?from_date AND `time1_displayed` < ?to_date", FromReader,
                    new MySqlParameter("from_date", fromDate),
                    new MySqlParameter("to_date", toDate.AddDays(1)));





        public static void ClockIn(long employeeId)
        {
            var minimumClockInTime = 
                TimeCardRule.All()
                    .Where(
                        timeCardRule => 
                            (timeCardRule.EmployeeId == null || timeCardRule.EmployeeId == employeeId) && timeCardRule.ClockInTime != TimeSpan.Zero)
                    .OrderBy(
                        timeCardRule => timeCardRule.ClockInTime)
                    .FirstOrDefault()?.ClockInTime ?? TimeSpan.Zero;

            if (DateTime.Now.TimeOfDay < minimumClockInTime)
                throw new Exception("Error. Cannot clock in until " + minimumClockInTime.ToStringHmm());
            
            var clockEvent = GetLastEvent(employeeId);

            if (clockEvent == null)
            {
                Insert(clockEvent = new ClockEvent
                {
                    EmployeeId = employeeId,
                    Status = ClockEventStatus.Home,
                    ClinicId = Clinics.ClinicNum
                });
            }
            else if (clockEvent.Status == ClockEventStatus.Break)
            {
                clockEvent.Date2Entered = 
                    Preference.GetBool(PreferenceName.LocalTimeOverridesServerTime) ? 
                        DateTime.Now : 
                        MiscData.GetNowDateTime();

                clockEvent.Date2Displayed = clockEvent.Date2Entered;

                Update(clockEvent);
            }
            else
            {
                if (clockEvent.Date2Displayed == null) throw new Exception("Error. Already clocked in.");
                else
                {
                    var clockEventStatus = clockEvent.Status;

                    Insert(clockEvent = new ClockEvent
                    {
                        EmployeeId = employeeId,
                        Status = clockEventStatus,
                        ClinicId = Clinics.ClinicNum
                    });
                }
            }

            var employee = Employee.GetById(employeeId);

            SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, $"{employee} clocked in from {clockEvent.Status}.");
        }


        public static void ClockOut(long employeeId, ClockEventStatus clockStatus)
        {
            var clockEvent = GetLastEvent(employeeId);
            if (clockEvent == null)
                throw new Exception("Error. New employee never clocked in.");

            if (clockEvent.Status == ClockEventStatus.Break)
                throw new Exception("Error. Already clocked out for break.");

            if (clockEvent.Date2Displayed.HasValue)
                throw new Exception("Error. Already clocked out.");

            if (clockStatus == ClockEventStatus.Break)
            {
                var clinicId = clockEvent.ClinicId;

                Insert(clockEvent = new ClockEvent
                {
                    EmployeeId = employeeId,
                    Status = ClockEventStatus.Break,
                    ClinicId = clinicId
                });
            }
            else
            {
                clockEvent.Date2Entered = 
                    Preference.GetBool(PreferenceName.LocalTimeOverridesServerTime) ? 
                        DateTime.Now : 
                        MiscData.GetNowDateTime();

                clockEvent.Date2Displayed = clockEvent.Date2Entered;
                clockEvent.Status = clockStatus;

                Update(clockEvent);
            }

            var employee = Employee.GetById(employeeId);

            SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, $"{employee} clocked out for {clockEvent.Status}.");
        }


        /// <summary>
        /// Gets directly from the database. If the last event is a completed break, then it 
        /// instead grabs the half-finished clock in. Other possibilities include half-finished 
        /// clock in which truly was the last event, a finished clock in/out, a half-finished 
        /// clock out for break, or null for a new employee. Returns null if employeeNum of 0 
        /// passed in or no clockevent was found for the corresponding employee.
        /// </summary>
        public static ClockEvent GetLastEvent(long employeeId)
        {
            if (employeeId == 0) return null;

            var clockEvent = 
                SelectOne(
                    "SELECT * FROM `clock_events` WHERE `employee_id` = " + employeeId + " ORDER BY `date1_displayed` DESC LIMIT 1", 
                    FromReader);

            if (clockEvent != null && clockEvent.Status == ClockEventStatus.Break && clockEvent.Date2Displayed.HasValue)
            {
                return 
                    SelectOne(
                        "SELECT * FROM `clock_events` WHERE `employee_id` = " + employeeId + " AND `status` != 2 ORDER BY `date1_displayed` DESC LIMIT 1", 
                        FromReader);
            }

            return clockEvent;
        }


        public static string Format(TimeSpan span)
        {
            if (Preference.GetBool(PreferenceName.TimeCardsUseDecimalInsteadOfColon))
            {
                if (span == TimeSpan.Zero)
                {
                    return "";
                }
                return span.TotalHours.ToString("n");
            }
            else if (Preference.GetBool(PreferenceName.TimeCardShowSeconds))
            {
                return span.ToStringHmmss();
            }

            return span.ToStringHmm();
        }

        public static List<ClockEvent> GetListForTimeCardManage(long employeeId, long? clinicId, DateTime fromDate, DateTime toDate)
        {
            string command =
                "SELECT * FROM `clock_events` " +
                "WHERE `employee_id` = ?employee_id " +
                "AND `date1_displayed` >= ?from_date AND `date1_displayed` < ?to_date ";

            if (clinicId.HasValue) command += "AND `clinic_id` = ?clinic_id ";
            
            command += 
                "AND (`status` = 0 OR `status` = 1) " +
                "ORDER BY `date1_displayed`";

            var clockEvents = 
                SelectMany(command, FromReader,
                    new MySqlParameter("employee_id", employeeId),
                    new MySqlParameter("clinic_id", clinicId ?? 0),
                    new MySqlParameter("from_date", fromDate),
                    new MySqlParameter("to_date", toDate.AddDays(1)));

            var errors = new StringBuilder();
            foreach (var clockEvent in clockEvents)
            {
                if (clockEvent.Date2Displayed == null)
                {
                    errors.AppendLine($"{clockEvent.Date1Displayed.ToShortDateString()}: the employee did not clock out.");
                }
                else if (clockEvent.Date2Displayed.Value.Date != clockEvent.Date1Displayed.Date)
                {
                    errors.AppendLine($"{clockEvent.Date1Displayed.ToShortDateString()}: entry spans multiple days.");
                }
            }

            if (errors.Length > 0) throw new Exception(errors.ToString());
            
            return clockEvents;
        }

        /// <summary>
        /// Returns a list of clock events within the date range for employee.
        /// </summary>
        /// <param name="empNum">The primary key of the employee.</param>
        /// <param name="fromDate">The start date of the clock events we are validating for an employee.</param>
        /// <param name="toDate">The end date of the clock events we are validating for an employee.</param>
        /// <param name="isBreaks">Indicates whether we are validating break events as opposed to clock in and out events.</param>
        public static List<ClockEvent> GetValidList(long employeeId, DateTime fromDate, DateTime toDate, bool isBreaks)
        {
            string command =
                "SELECT * FROM `clock_events` " +
                "WHERE `employee_id` = ?employee_id " +
                "AND `date1_displayed` >= ?from_date AND `date1_displayed` < ?to_date ";

            command += isBreaks ? 
                "AND `status` = 2 " :
                "AND (`status` = 0 OR `status` = 1) ";

            command +=
                "ORDER BY `date1_displayed`";

            var clockEvents =
                SelectMany(command, FromReader,
                    new MySqlParameter("employee_id", employeeId),
                    new MySqlParameter("from_date", fromDate),
                    new MySqlParameter("to_date", toDate.AddDays(1)));

            var errors = new StringBuilder();
            foreach (var clockEvent in clockEvents)
            {
                if (clockEvent.Date2Displayed == null)
                {
                    errors.AppendLine($"{clockEvent.Date1Displayed.ToShortDateString()}: the employee did not {(isBreaks ? "in from break" : "clock out ")}.");
                }
                else if (clockEvent.Date2Displayed.Value.Date != clockEvent.Date1Displayed.Date)
                {
                    errors.AppendLine($"{clockEvent.Date1Displayed.ToShortDateString()}: entry spans multiple days.");
                }
            }

            if (errors.Length > 0) throw new Exception((isBreaks ? "Break" : "Clock") + " event errors:\r\n" + errors);

            return clockEvents;
        }


        /// <summary>
        /// Returns all the dates of all first week days in the given period.
        /// </summary>
        private static IEnumerable<DateTime> WeekStartHelper(DateTime startDate, DateTime endDate)
        {
            var currentDate = startDate;

            var dayOfWeek = (DayOfWeek)Preference.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek);
            for (int i = 0; i < 7; i++)
            {
                currentDate = currentDate.AddDays(-1);
                if (currentDate.DayOfWeek == dayOfWeek)
                {
                    yield return currentDate;

                    break;
                }
            }

            currentDate = currentDate.AddDays(7);
            for (; currentDate < endDate; currentDate = currentDate.AddDays(7))
            {
                yield return currentDate;
            }
        }




        /// <summary>
        /// Used in the timecard to track hours worked per week when the week started in a previous 
        /// time period.  This gets all the hours of the first week before the date listed.
        /// Also adds in any adjustments for that week.
        /// </summary>
        public static TimeSpan GetWeekTotal(long employeeId, DateTime date)
        {
            var timeSpan = TimeSpan.Zero;

            var firstDayOfWeek = (DayOfWeek)Preference.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek);

            // If the first day of the pay period is the starting date for the overtime, then there is no need to retrieve any times from the previous pay period.
            if (date.DayOfWeek == firstDayOfWeek)
                return timeSpan;

            // Move back until we find the starting date of the current week.
            var firstDayOfWeekDate = date.AddDays(-1);
            for (int i = 0; i < 6; i++)
            {
                if (firstDayOfWeekDate.DayOfWeek == firstDayOfWeek)
                {
                    break;
                }
                firstDayOfWeekDate = firstDayOfWeekDate.AddDays(-1);
            }

            var clockEvents = Refresh(employeeId, firstDayOfWeekDate, date.AddDays(-1), false);
            foreach (var clockEvent in clockEvents)
            {
                // If someone intentionally backdates a clock out event to get negative time, they can use an adjustment instead.
                if (clockEvent.Date2Displayed < clockEvent.Date1Displayed) continue;

                if (clockEvent.Date2Displayed.HasValue)
                {
                    timeSpan += clockEvent.Date2Displayed.Value - clockEvent.Date1Displayed;
                    timeSpan += clockEvent.Adjust ?? clockEvent.AdjustAuto;
                    timeSpan += clockEvent.Overtime ?? clockEvent.OvertimeAuto;
                }
            }

            // Apply the adjustments...
            var timeAdjustments = TimeAdjustment.Refresh(employeeId, firstDayOfWeekDate, date.AddDays(-1));
            foreach (var timeAdjustment in timeAdjustments)
            {
                timeSpan += timeAdjustment.HoursRegular;
            }

            return timeSpan;
        }







        /// <summary>
        /// Generates the time card totals for the specified time period.
        /// </summary>
        public static DataTable GetTimeCardManage(DateTime startDate, DateTime stopDate, long? clinicId, bool isAll)
        {
            var errors = new StringBuilder();

            var resultsDataTable = new DataTable("TimeCardManage");
            resultsDataTable.Columns.Add("PayrollID");
            resultsDataTable.Columns.Add("EmployeeID");
            resultsDataTable.Columns.Add("FirstName");
            resultsDataTable.Columns.Add("LastName");
            resultsDataTable.Columns.Add("TotalHours");
            resultsDataTable.Columns.Add("Rate1Hours");
            resultsDataTable.Columns.Add("Rate1OTHours");
            resultsDataTable.Columns.Add("Rate2Hours");
            resultsDataTable.Columns.Add("Rate2OTHours");
            resultsDataTable.Columns.Add("Note");

            var employees = Employee.GetForTimeCard();
            var employeesForClinic = 
                clinicId.HasValue ? 
                    Employee.GetEmpsForClinic(clinicId.Value) : new List<Employee>();

            var payPeriodNotes = TimeAdjustment.GetPayPeriodNote(startDate);

            foreach (var employee in employees)
            {
                errors.Clear();

                string note = "";

                var dataRow = resultsDataTable.NewRow();

                dataRow.ItemArray.Initialize();
                dataRow["PayrollID"] = employee.PayrollID;
                dataRow["EmployeeID"] = employee.Id;
                dataRow["FirstName"] = employee.FirstName;
                dataRow["LastName"] = employee.LastName;

                var weeklyHoursRegular = new List<TimeSpan>();
                var weeklyHoursOvertime = new List<TimeSpan>();
                var weeklyHoursDifferential = new List<TimeSpan>();

                var clockEvents = new List<ClockEvent>();
                var timeAdjustments = new List<TimeAdjustment>();

                try
                {
                    clockEvents = GetListForTimeCardManage(employee.Id, clinicId, startDate, stopDate);
                }
                catch (Exception ex)
                {
                    errors.AppendLine(ex.Message);
                }

                try
                {
                    timeAdjustments = TimeAdjustment.GetListForTimeCardManage(employee.Id, clinicId, startDate, stopDate);
                }
                catch (Exception ex)
                {
                    errors.AppendLine(ex.Message);
                }

                // If there are no clock events, nor time adjusts, and the current employee isn't
                // "assigned" to the clinic passed in, skip.
                if (clockEvents.Count == 0 && timeAdjustments.Count == 0 && !isAll && employeesForClinic.Count(x => x.Id == employee.Id) == 0)
                {
                    continue;
                }

                // If there were any errors, report them in the note field and stop further 
                // processing for this employee.
                if (errors.Length > 0)
                {
                    dataRow["Note"] = errors.ToString().Trim();

                    resultsDataTable.Rows.Add(dataRow);

                    continue;
                }

                var weekStartDates = WeekStartHelper(startDate, stopDate);
                foreach (var weekStartDate in weekStartDates)
                {
                    weeklyHoursRegular.Add(TimeSpan.Zero);
                    weeklyHoursOvertime.Add(TimeSpan.Zero);
                    weeklyHoursDifferential.Add(TimeSpan.Zero);
                }

                // Sum the clock events...
                int currentWeek = 0;
                for (int i = 0; i < clockEvents.Count; i++)
                {
                    foreach (var weekStartDate in weekStartDates)
                    {
                        if (clockEvents[i].Date1Displayed < weekStartDate.AddDays(7))
                        {
                            break;
                        }
                        currentWeek++;
                    }

                    // We only want the comment from the first clock event entry.
                    if (i == 0) note = clockEvents[i].Note;
                    
                    weeklyHoursRegular[currentWeek] += 
                        clockEvents[i].Date2Displayed.Value - 
                        clockEvents[i].Date1Displayed + 
                        (clockEvents[i].Adjust ?? clockEvents[i].AdjustAuto) -
                        (clockEvents[i].Overtime ?? clockEvents[i].OvertimeAuto);

                    weeklyHoursOvertime[currentWeek] += clockEvents[i].Overtime ?? clockEvents[i].OvertimeAuto;
                    weeklyHoursDifferential[currentWeek] += clockEvents[i].Rate2 ?? clockEvents[i].Rate2Auto;
                }

                // Sum the adjustments...
                currentWeek = 0;
                for (int i = 0; i < timeAdjustments.Count; i++)
                {
                    foreach (var weekStartDate in weekStartDates)
                    {
                        if (clockEvents[i].Date1Displayed < weekStartDate.AddDays(7))
                        {
                            break;
                        }
                        currentWeek++;
                    }

                    weeklyHoursRegular[currentWeek] += timeAdjustments[i].HoursRegular;
                    weeklyHoursOvertime[currentWeek] += timeAdjustments[i].HoursOvertime;
                }

                double totalHours = 0;
                double totalHoursRegular = 0;
                double totalHoursOvertime = 0;
                double totalHoursDifferential = 0;

                // Sum weekly totals.
                for (int i = 0; i < weekStartDates.Count(); i++)
                {
                    totalHours              += weeklyHoursRegular[i].TotalHours + weeklyHoursOvertime[i].TotalHours;
                    totalHoursRegular       += weeklyHoursRegular[i].TotalHours;
                    totalHoursOvertime      += weeklyHoursOvertime[i].TotalHours;
                    totalHoursDifferential  += weeklyHoursDifferential[i].TotalHours;
                }

                // Regular time at R1 and R2
                double rate1ratio = 0;
                if (totalHours != 0)
                {
                    rate1ratio = 1 - totalHoursDifferential / totalHours;
                }
                dataRow["TotalHours"] = TimeSpan.FromHours(totalHours).ToString();

                double r1Hours = rate1ratio * totalHoursRegular;
                double r2Hours = totalHoursRegular - r1Hours;
                double r1OTHours = rate1ratio * totalHoursOvertime;
                double r2OTHours = totalHours - r1Hours - r2Hours - r1OTHours;//"self correcting math" uses guaranteed to total correctly.

                dataRow["Rate1Hours"] = TimeSpan.FromHours(r1Hours).ToString();
                dataRow["Rate2Hours"] = TimeSpan.FromHours(r2Hours).ToString();
                dataRow["Rate10Hours"] = TimeSpan.FromHours(r1OTHours).ToString();
                dataRow["Rate20Hours"] = TimeSpan.FromHours(r2OTHours).ToString();

                string payPeriodNote = 
                    payPeriodNotes.FirstOrDefault(
                        timeAdjustment => timeAdjustment.EmployeeId == employee.Id)?.Note;

                dataRow["Note"] = string.IsNullOrEmpty(payPeriodNote) ? note : payPeriodNote;

                resultsDataTable.Rows.Add(dataRow);
            }

            return resultsDataTable;
        }
    }
}
