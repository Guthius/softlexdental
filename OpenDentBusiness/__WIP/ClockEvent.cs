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

    public enum ClockEventStatus
    {
        Home,
        Lunch,
        Break
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
        /// Can be 01-01-0001 if not clocked out yet.
        /// </summary>
        public DateTime? Date2Entered;

        /// <summary>
        /// User can edit. Can be 01-01-0001 if not clocked out yet.
        /// </summary>
        public DateTime? Date2Displayed;

        /// <summary>
        /// This is a manual override for OTimeAuto. 
        /// Typically -1 hour (-01:00:00) to indicate no override. 
        /// When used as override, allowed values are zero or positive. 
        /// This is an alternative to using a TimeAdjust row.
        /// </summary>
        public TimeSpan? Overtime;

        /// <summary>
        /// Automatically calculated OT.  Will be zero if none.
        /// </summary>
        public TimeSpan OvertimeAuto;
        
        /// <summary>
        /// This is a manual override of AdjustAuto.
        /// </summary>
        public TimeSpan? Adjust;
        
        /// <summary>
        /// Automatically calculated Adjust. Will be zero if none.
        /// </summary>
        public TimeSpan AdjustAuto;
        
        /// <summary>
        /// This is a manual override for Rate2Auto.
        /// Typically -1 hour (-01:00:00) to indicate no override. 
        /// When used as override, allowed values are zero or positive. 
        /// This is the portion of the hours worked which are at Rate2, so it's not in addition to the hours worked. 
        /// Also used to calculate the Rate2 OT.
        /// </summary>
        public TimeSpan? Rate2;
        
        /// <summary>
        /// Automatically calculated rate2 pay.
        /// Will be zero if none.
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
            SelectOne("SELECT * FROM clock_events WHERE id = " + clockEventId, FromReader);

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

        public ClockEvent()
        {
            Overtime = TimeSpan.FromHours(-1);
            Rate2 = TimeSpan.FromHours(-1);
        }

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
        /// Currently only used internally for payroll benefits report.
        /// </summary>
        public static List<ClockEvent> GetAllForPeriod(DateTime fromDate, DateTime toDate) =>
            SelectMany(
                "SELECT * FROM `clock_events` WHERE `time1_displayed` >= ?from_date AND `time1_displayed` < ?to_date", FromReader,
                    new MySqlParameter("from_date", fromDate),
                    new MySqlParameter("to_date", toDate.AddDays(1)));





        public static void ClockIn(long employeeId)
        {
            var minClockInTime = 
                TimeCardRule.All()
                    .Where(
                        timeCardRule => 
                            (timeCardRule.EmployeeId == null || timeCardRule.EmployeeId == employeeId) && timeCardRule.ClockInTime != TimeSpan.Zero)
                    .OrderBy(
                        timeCardRule => timeCardRule.ClockInTime)
                    .FirstOrDefault()?.ClockInTime ?? TimeSpan.Zero;

            if (DateTime.Now.TimeOfDay < minClockInTime)
                throw new Exception("Error. Cannot clock in until " + minClockInTime.ToStringHmm());
            
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

        public static List<ClockEvent> GetListForTimeCardManage(long employeeId, long? clinicId, DateTime fromDate, DateTime toDate, bool isAll)
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

            if (errors.Length > 0) throw new Exception("Clock event errors:\r\n" + errors);
            
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
        /// Returns number of work weeks spanned by dates. 
        /// Example: "11-01-2013"(Friday), to "11-14-2013"(Thursday) spans 3 weeks, if the workweek
        /// starts on Sunday it would return a list containing "10-27-2013"(Sunday), 
        /// "11-03-2013"(Sunday), and"11-10-2013"(Sunday). Used to determine which week time 
        /// adjustments and clock events belong to when totalling timespans.
        /// </summary>
        private static List<DateTime> WeekStartHelper(DateTime startDate, DateTime stopDate)
        {
            var days = new List<DateTime>();
            var currentDate = startDate;

            var dayOfWeek = (DayOfWeek)Preference.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek);
            for (int i = 0; i < 7; i++)
            {
                currentDate = currentDate.AddDays(-1);
                if (currentDate.DayOfWeek == dayOfWeek)
                {
                    days.Add(currentDate);
                    break;
                }
            }

            currentDate = currentDate.AddDays(-7);
            for (; currentDate < stopDate; currentDate = currentDate.AddDays(-7))
            {
                days.Add(currentDate);
            }

            return days;
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
        /// Returns clockevent information for all non-hidden employees. Used only in the time card
        /// manage window.  
        /// 
        /// Set isAll to true to return all employee time cards (used for clinics).
        /// </summary>
        public static DataTable GetTimeCardManage(DateTime startDate, DateTime stopDate, long clinicId, bool isAll)
        {
            var resultsDataTable = new DataTable("TimeCardManage");
            resultsDataTable.Columns.Add("PayrollID");
            resultsDataTable.Columns.Add("EmployeeNum");
            resultsDataTable.Columns.Add("firstName");
            resultsDataTable.Columns.Add("lastName");
            resultsDataTable.Columns.Add("totalHours");//should be sum of RegularHours and OvertimeHours
            resultsDataTable.Columns.Add("rate1Hours");
            resultsDataTable.Columns.Add("rate1OTHours");
            resultsDataTable.Columns.Add("rate2Hours");
            resultsDataTable.Columns.Add("rate2OTHours");
            resultsDataTable.Columns.Add("Note");


            List<Employee> listEmployees= Employee.GetForTimeCard();
            List<Employee> listEmpsForClinic = Employee.GetEmpsForClinic(clinicId);

            //get all pay period notes for all employees for this pay period. 
            List<TimeAdjustment> listPayPeriodNotes = TimeAdjustment.GetNotesForPayPeriod(startDate);
            for (int e = 0; e < listEmployees.Count; e++)
            {
                string employeeErrors = "";
                string note = "";
                DataRow dataRowCur = resultsDataTable.NewRow();
                dataRowCur.ItemArray.Initialize();//changes all nulls to blanks and zeros.
                                                  //PayrollID-------------------------------------------------------------------------------------------------------------------------------------
                dataRowCur["PayrollID"] = listEmployees[e].PayrollID;
                //EmployeeNum and Name----------------------------------------------------------------------------------------------------------------------------------
                dataRowCur["EmployeeNum"] = listEmployees[e].Id;
                dataRowCur["firstName"] = listEmployees[e].FirstName;
                dataRowCur["lastName"] = listEmployees[e].LastName;
                //Begin calculations------------------------------------------------------------------------------------------------------------------------------------
                //each list below will contain one entry per week.
                List<TimeSpan> listTsRegularHoursWeekly = new List<TimeSpan>();//Total non-overtime hours.  Used for calculation, not displayed or part of dataTable.
                List<TimeSpan> listTsOTHoursWeekly = new List<TimeSpan>();//Total overtime hours.  Used for calculation, not displayed or part of dataTable.
                List<TimeSpan> listTsDifferentialHoursWeekly = new List<TimeSpan>();//Not included in total hours worked.  tsDifferentialHours is differant than r2Hours and r2OTHours
                List<ClockEvent> listClockEvents = new List<ClockEvent>();//per clinic
                List<TimeAdjustment> listTimeAdjusts = new List<TimeAdjustment>();//per clinic
                try
                {
                    listClockEvents = ClockEvents.GetListForTimeCardManage(listEmployees[e].Id, clinicId, startDate, stopDate, isAll);
                }
                catch (Exception ex)
                {
                    employeeErrors += ex.Message;
                }
                try
                {
                    listTimeAdjusts = TimeAdjusts.GetListForTimeCardManage(listEmployees[e].Id, clinicId, startDate, stopDate, isAll);
                }
                catch (Exception ex)
                {
                    employeeErrors += ex.Message;
                }
                //If there are no clock events, nor time adjusts, and the current employee isn't "assigned" to the clinic passed in, skip.
                if (listClockEvents.Count == 0 //employee has no clock events for this clinic.
                    && listTimeAdjusts.Count == 0 //employee has no time adjusts for this clinic.
                    && (!isAll && listEmpsForClinic.Count(x => x.Id == listEmployees[e].Id) == 0)) //employee not explicitly assigned to clinic
                {
                    continue;
                }
                //report errors in note column and move to next employee.----------------------------------------------------------------
                if (employeeErrors != "")
                {
                    dataRowCur["Note"] = employeeErrors.Trim();
                    resultsDataTable.Rows.Add(dataRowCur);
                    continue;//display employee errors in note field for employee. All columns will be blank for just this employee.
                }
                //sum values for each week----------------------------------------------------------------------------------------------------
                List<DateTime> weekStartDates = weekStartHelper(startDate, stopDate);
                for (int i = 0; i < weekStartDates.Count; i++)
                {
                    listTsRegularHoursWeekly.Add(TimeSpan.Zero);
                    listTsOTHoursWeekly.Add(TimeSpan.Zero);
                    listTsDifferentialHoursWeekly.Add(TimeSpan.Zero);
                }
                int weekCur = 0;
                for (int i = 0; i < listClockEvents.Count; i++)
                {
                    //set current week for clock event
                    for (int j = 0; j < weekStartDates.Count; j++)
                    {
                        if (listClockEvents[i].Date1Displayed < weekStartDates[j].AddDays(7))
                        {
                            weekCur = j;//clock event occurs during the week "j"
                            break;
                        }
                    }
                    if (i == 0)
                    {//we only want the comment from the first clock event entry.
                        note = listClockEvents[i].Note;
                    }
                    //TimeDisplayed-----
                    listTsRegularHoursWeekly[weekCur] += listClockEvents[i].Date2Displayed - listClockEvents[i].Date1Displayed;
                    //Adjusts-----
                    if (listClockEvents[i].AdjustOverridden)
                    {
                        listTsRegularHoursWeekly[weekCur] += listClockEvents[i].Adjust;
                    }
                    else
                    {
                        listTsRegularHoursWeekly[weekCur] += listClockEvents[i].AdjustAuto;
                    }
                    //OverTime-----
                    if (listClockEvents[i].Overtime != TimeSpan.FromHours(-1))
                    {//Manual override
                        listTsOTHoursWeekly[weekCur] += listClockEvents[i].Overtime;
                        listTsRegularHoursWeekly[weekCur] += -listClockEvents[i].Overtime;
                    }
                    else
                    {
                        listTsOTHoursWeekly[weekCur] += listClockEvents[i].OvertimeAuto;
                        listTsRegularHoursWeekly[weekCur] += -listClockEvents[i].OvertimeAuto;
                    }
                    //Differential/Rate2
                    if (listClockEvents[i].Rate2 != TimeSpan.FromHours(-1))
                    {//Manual override
                        listTsDifferentialHoursWeekly[weekCur] += listClockEvents[i].Rate2;
                    }
                    else
                    {
                        listTsDifferentialHoursWeekly[weekCur] += listClockEvents[i].Rate2Auto;
                    }
                }
                //reset current week to itterate through time adjusts
                weekCur = 0;
                for (int i = 0; i < listTimeAdjusts.Count; i++)
                {//list of timeAdjusts have already been filtered. All timeAdjusts in this list are applicable.
                 //set current week for time adjusts-----
                    for (int j = 0; j < weekStartDates.Count; j++)
                    {
                        if (listTimeAdjusts[i].Date < weekStartDates[j].AddDays(7))
                        {
                            weekCur = j;//clock event occurs during the week "j"
                            break;
                        }
                    }
                    listTsRegularHoursWeekly[weekCur] += listTimeAdjusts[i].HoursRegular;
                    listTsOTHoursWeekly[weekCur] += listTimeAdjusts[i].HoursOvertime;
                }
                //Overtime should have already been calculated by CalcWeekly(); No calculations needed, just sum values.------------------------------------------------------
                double totalHoursWorked = 0;
                double totalRegularHoursWorked = 0;
                double totalOTHoursWorked = 0;
                double totalDiffHoursWorked = 0;
                //sum weekly totals.
                for (int i = 0; i < weekStartDates.Count; i++)
                {
                    totalHoursWorked += listTsRegularHoursWeekly[i].TotalHours;
                    totalHoursWorked += listTsOTHoursWeekly[i].TotalHours;
                    totalRegularHoursWorked += listTsRegularHoursWeekly[i].TotalHours;
                    totalOTHoursWorked += listTsOTHoursWeekly[i].TotalHours;
                    totalDiffHoursWorked += listTsDifferentialHoursWeekly[i].TotalHours;
                }
                //Regular time at R1 and R2
                double rate1ratio = 0;
                if (totalHoursWorked != 0)
                {
                    rate1ratio = 1 - totalDiffHoursWorked / totalHoursWorked;
                }
                dataRowCur["totalHours"] = TimeSpan.FromHours(totalHoursWorked).ToString();
                double r1Hours = rate1ratio * totalRegularHoursWorked;
                double r2Hours = totalRegularHoursWorked - r1Hours;
                double r1OTHours = rate1ratio * totalOTHoursWorked;
                double r2OTHours = totalHoursWorked - r1Hours - r2Hours - r1OTHours;//"self correcting math" uses guaranteed to total correctly.
                dataRowCur["rate1Hours"] = TimeSpan.FromHours(r1Hours).ToString();
                dataRowCur["rate2Hours"] = TimeSpan.FromHours(r2Hours).ToString();
                dataRowCur["rate1OTHours"] = TimeSpan.FromHours(r1OTHours).ToString();
                dataRowCur["rate2OTHours"] = TimeSpan.FromHours(r2OTHours).ToString();
                string payPeriodNote = listPayPeriodNotes.FirstOrDefault(x => x.EmployeeId == listEmployees[e].Id)?.Note;
                if (string.IsNullOrEmpty(payPeriodNote))
                {
                    dataRowCur["Note"] = note;
                }
                else
                {
                    dataRowCur["Note"] = payPeriodNote;
                }
                resultsDataTable.Rows.Add(dataRowCur);
            }
            return resultsDataTable;
        }

    }
}
