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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace OpenDentBusiness
{
    public class TimeCardRule : DataRecord
    {
        /// <summary>
        /// The ID of the employee the rule applies to. If null, the rule applies to all employees.
        /// </summary>
        public long? EmployeeId;

        /// <summary>
        /// Indicates the working hours in a day. A typical example is 08:00. In California, any 
        /// work after the first 8 hours is overtime. All time after the specified hours is 
        /// overtime.
        /// </summary>
        public TimeSpan Hours;

        /// <summary>
        /// Indicates the regular work start time. All time worked before this time is at a differential rate.
        /// </summary>
        public TimeSpan TimeStart;

        /// <summary>
        /// Indicates the regular work end time. All time worked after this time is at a differntial rate.
        /// </summary>
        public TimeSpan TimeEnd;

        /// <summary>
        /// Indicates if the employee should have overtime calculated for their hours worked in a pay period.
        /// </summary>
        public bool IsOvertimeExempt;

        /// <summary>
        /// The earliest time at which an employee can clock in. If set to <see cref="TimeSpan.Zero"/> the employee can clock in at any time.
        /// </summary>
        public TimeSpan ClockInTime;

        /// <summary>
        /// Constructs a new instance of the <see cref="TimeCardRule"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="TimeCardRule"/> instance.</returns>
        public static TimeCardRule FromReader(MySqlDataReader dataReader)
        {
            return new TimeCardRule
            {
                Id = Convert.ToInt64(dataReader["id"]),
                EmployeeId = dataReader["employee_id"] as long?,
                Hours = (TimeSpan)dataReader["hours"],
                TimeStart = (TimeSpan)dataReader["time_start"],
                TimeEnd = (TimeSpan)dataReader["time_end"],
                IsOvertimeExempt = Convert.ToBoolean(dataReader["overtime_exempt"]),
                ClockInTime = (TimeSpan)dataReader["clockin_time"]
            };
        }

        public static List<TimeCardRule> All() =>
            SelectMany("SELECT * FROM `time_card_rules`", FromReader);

        /// <summary>
        /// Gets the time card rule with the specified ID from the database.
        /// </summary>
        /// <param name="timeCardRuleId">The ID of the time card rule.</param>
        /// <returns>The time card rule with the specified ID.</returns>
        public static TimeCardRule GetById(long timeCardRuleId) =>
            SelectOne("SELECT * FROM `time_card_rules` WHERE `id` = " + timeCardRuleId, FromReader);

        /// <summary>
        /// Inserts the specified time card rule in the database.
        /// </summary>
        /// <param name="timeCardRule">The time card rule.</param>
        /// <returns>The ID assigned to the time card rule.</returns>
        public static long Insert(TimeCardRule timeCardRule) =>
            timeCardRule.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `time_card_rules` (`employee_id`, `hours`, `time_start`, `time_end`, `overtime_exempt`, `clockin_time`) " +
                "VALUES (?employee_id, ?hours, ?time_start, ?time_end, ?overtime_exempt, ?clockin_time)",
                    new MySqlParameter("employee_id", timeCardRule.EmployeeId),
                    new MySqlParameter("hours", timeCardRule.Hours),
                    new MySqlParameter("time_start", timeCardRule.TimeStart),
                    new MySqlParameter("time_end", timeCardRule.TimeEnd),
                    new MySqlParameter("overtime_exempt", timeCardRule.IsOvertimeExempt),
                    new MySqlParameter("clockin_time", timeCardRule.ClockInTime));

        /// <summary>
        /// Updates the specified time card rule in the database.
        /// </summary>
        /// <param name="timeCardRule">The time card rule.</param>
        public static void Update(TimeCardRule timeCardRule) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE time_card_rules SET employee_id = ?employee_id, hours = ?hours, ",
                    new MySqlParameter("employee_id", timeCardRule.EmployeeId),
                    new MySqlParameter("hours", timeCardRule.Hours),
                    new MySqlParameter("time_start", timeCardRule.TimeStart),
                    new MySqlParameter("time_end", timeCardRule.TimeEnd),
                    new MySqlParameter("overtime_exempt", timeCardRule.IsOvertimeExempt),
                    new MySqlParameter("clockin_time", timeCardRule.ClockInTime),
                    new MySqlParameter("id", timeCardRule.ClockInTime));

        /// <summary>
        /// Deletes the time card rule with the specified ID from the database.
        /// </summary>
        /// <param name="timeCardRuleId">The ID of the time card rule to delete.</param>
        public static void Delete(long timeCardRuleId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `time_card_rules` WHERE `id` = " + timeCardRuleId);

        /// <summary>
        /// Deletes all time card rules with the specified ID's from the database.
        /// </summary>
        /// <param name="timeCardRuleIds">The ID's of the time card rules to delete.</param>
        public static void DeleteMany(IEnumerable<long> timeCardRuleIds)
        {
            if (timeCardRuleIds != null && timeCardRuleIds.Count() > 0)
            {
                DataConnection.ExecuteNonQuery("DELETE FROM `time_card_rules` WHERE `id` IN (" + string.Join(", ", timeCardRuleIds) + ")");
            }
        }








        /// <summary>
        /// Clears all automatic adjustments that have been calculated for the specified employee 
        /// within the given time period.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        public static void ClearAuto(long employeeId, DateTime dateStart, DateTime dateEnd)
        {
            var clockEvents = ClockEvent.GetSimpleList(employeeId, dateStart, dateEnd);
            foreach (var clockEvent in clockEvents)
            {
                clockEvent.AdjustAuto = TimeSpan.Zero;
                clockEvent.OvertimeAuto = TimeSpan.Zero;
                clockEvent.Rate2Auto = TimeSpan.Zero;

                ClockEvent.Update(clockEvent);
            }

            var timeAdjusts = TimeAdjustment.GetSimpleListAuto(employeeId, dateStart, dateEnd);
            foreach (var timeAdjust in timeAdjusts)
            {
                TimeAdjustment.Delete(timeAdjust);
            }
        }

        /// <summary>
        /// Clears all mannual adjustments that have been made for the specified employee within
        /// the given time period.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        public static void ClearManual(long employeeId, DateTime dateStart, DateTime dateEnd)
        {
            var clockEvents = ClockEvent.GetSimpleList(employeeId, dateStart, dateEnd);
            foreach (var clockEvent in clockEvents)
            {
                clockEvent.Adjust = null;
                clockEvent.Overtime = TimeSpan.FromHours(-1);
                clockEvent.Rate2 = TimeSpan.FromHours(-1);

                ClockEvent.Update(clockEvent);
            }
        }







        ///<summary>One list of tuples per week, each list of tuples contains up to one entry per clinic, if employee clocked any time at that clinic.
        ///<para>Tuple is &lt;ClinicNum,TimeSpan></para></summary>
        private static List<List<Tuple<long?, TimeSpan>>> _listEvents;



        ///<summary>Validates pay period before making any adjustments.
        ///If today falls before the stopDate passed in, stopDate will be set to today's date.</summary>
        public static string ValidatePayPeriod(Employee employeeCur, DateTime startDate, DateTime stopDate)
        {
            //If calculating breaks before the end date of the pay period, only calculate breaks and validate clock in and out events for days
            //before today.  Use the server time just because we are dealing with time cards.
            DateTime dateTimeNow = MiscData.GetNowDateTime();
            ClockEvent lastClockEvent = ClockEvent.GetLastEvent(employeeCur.Id);
            bool isStillWorking = (lastClockEvent != null && (lastClockEvent.Status == ClockEventStatus.Break || lastClockEvent.Date2Displayed == null));
            if (dateTimeNow.Date < stopDate.Date && isStillWorking)
            {
                stopDate = dateTimeNow.Date.AddDays(-1);
            }
            List<ClockEvent> breakList = ClockEvent.Refresh(employeeCur.Id, startDate, stopDate, true);
            List<ClockEvent> clockEventList = ClockEvent.Refresh(employeeCur.Id, startDate, stopDate, false);
            bool errorFound = false;
            string retVal = "Time card errors for employee : " + Employee.GetNameFL(employeeCur) + "\r\n";
            //Validate clock events
            foreach (ClockEvent cCur in clockEventList)
            {
                if (cCur.Date2Displayed == null)
                {
                    retVal += "  " + cCur.Date1Displayed.ToShortDateString() + " : Employee not clocked out.\r\n";
                    errorFound = true;
                }
                else if (cCur.Date1Displayed.Date != cCur.Date2Displayed.Value.Date)
                {
                    retVal += "  " + cCur.Date1Displayed.ToShortDateString() + " : Clock entry spans multiple days.\r\n";
                    errorFound = true;
                }
            }
            //Validate Breaks
            foreach (ClockEvent bCur in breakList)
            {
                if (bCur.Date2Displayed == null)
                {
                    retVal += "  " + bCur.Date1Displayed.ToShortDateString() + " : Employee not clocked in from break.\r\n";
                    errorFound = true;
                }
                if (bCur.Date1Displayed.Date != bCur.Date2Displayed.Value.Date)
                {
                    retVal += "  " + bCur.Date1Displayed.ToShortDateString() + " : One break spans multiple days.\r\n";
                    errorFound = true;
                }
                for (int c = clockEventList.Count - 1; c >= 0; c--)
                {
                    if (clockEventList[c].Date1Displayed.Date == bCur.Date1Displayed.Date)
                    {
                        break;
                    }
                    if (c == 0)
                    { //we never found a match
                        retVal += "  " + bCur.Date1Displayed.ToShortDateString() + " : Break found during non-working day.\r\n";
                        errorFound = true;
                    }
                }
            }
            return (errorFound ? retVal : "");
        }

        ///<summary>Cannot have both AM/PM rules and OverHours rules defined. 
        /// We no longer block having multiple rules defined. With a better interface we can improve some of this functionality. Per NS 09/15/2015.</summary>
        public static string ValidateOvertimeRules(List<long> listEmployeeNums = null)
        {
            StringBuilder sb = new StringBuilder();
            CacheManager.Invalidate<TimeCardRule>();
            List<TimeCardRule> listTimeCardRules = TimeCardRule.All();
            if (listEmployeeNums != null && listEmployeeNums.Count > 0)
            {
                listTimeCardRules = listTimeCardRules.FindAll(x => x.EmployeeId == null || listEmployeeNums.Contains(x.EmployeeId.Value));
            }
            //Generate error messages for "All Employees" timecard rules.
            List<TimeCardRule> listTimeCardRulesAll = listTimeCardRules.FindAll(x => x.EmployeeId == 0);
            if (listTimeCardRulesAll.Any(x => x.TimeEnd > TimeSpan.Zero || x.TimeStart > TimeSpan.Zero) //There exists an AM or PM rule
                   && listTimeCardRulesAll.Any(x => x.Hours > TimeSpan.Zero)) //There also exists an Over hours rule.
            {
                sb.AppendLine("Time card errors found for \"All Employees\":");
                sb.AppendLine("  Both a time of day rule and an over hours per day rule found. Only one or the other is allowed.");
                return sb.ToString();
            }
            listEmployeeNums = listTimeCardRules.Where(x => x.EmployeeId != null).Select(x => x.EmployeeId.Value).Distinct().ToList();
            //Generate Employee specific errors
            for (int i = 0; i < listEmployeeNums.Count; i++)
            {
                long empNum = listEmployeeNums[i];
                List<TimeCardRule> listTimeCardRulesEmp = listTimeCardRules.FindAll(x => x.EmployeeId == 0 || x.EmployeeId == empNum);
                if (listTimeCardRulesEmp.Any(x => x.TimeEnd > TimeSpan.Zero || x.TimeStart > TimeSpan.Zero) //There exists an AM or PM rule
                   && listTimeCardRulesEmp.Any(x => x.Hours > TimeSpan.Zero)) //There also exists an Over hours rule.
                {
                    string empName = Employee.GetNameFL(Employee.GetById(empNum));
                    sb.AppendLine("Time card errors found for " + empName + ":");
                    sb.AppendLine("  Both a time of day rule and an over hours per day rule found. Only one or the other is allowed.\r\n");
                }
            }
            return sb.ToString();
        }




        ///<summary>Validates list and throws exceptions. Always returns a value. Creates a timecard rule based on all applicable timecard rules for a given employee.</summary>
        public static TimeCardRule GetTimeCardRule(Employee employeeCur)
        {
            //Validate Rules---------------------------------------------------------------------------------------------------------------
            string errors = ValidateOvertimeRules(new List<long> { employeeCur.Id });
            if (errors.Length > 0)
            {
                throw new Exception(errors);
            }
            //Build return value ----------------------------------------------------------------------------------------------------------
            List<TimeCardRule> listTimeCardRulesEmp = All().Where(x => x.EmployeeId == null || x.EmployeeId == employeeCur.Id).ToList();
            TimeCardRule amRule = listTimeCardRulesEmp.Where(x => x.TimeStart > TimeSpan.Zero).OrderByDescending(x => x.TimeStart).FirstOrDefault();
            TimeCardRule pmRule = listTimeCardRulesEmp.Where(x => x.TimeEnd > TimeSpan.Zero).OrderBy(x => x.TimeEnd).FirstOrDefault();
            TimeCardRule hoursRule = listTimeCardRulesEmp.Where(x => x.Hours > TimeSpan.Zero).OrderBy(x => x.Hours).FirstOrDefault();
            TimeCardRule isOvertimeExempt = listTimeCardRulesEmp.Where(x => x.IsOvertimeExempt).FirstOrDefault();
            TimeCardRule retVal = new TimeCardRule();
            if (amRule != null)
            {
                retVal.TimeStart = amRule.TimeStart;
            }
            if (pmRule != null)
            {
                retVal.TimeEnd = pmRule.TimeEnd;
            }
            if (hoursRule != null)
            {
                retVal.Hours = hoursRule.Hours;
            }
            if (isOvertimeExempt != null)
            {
                retVal.IsOvertimeExempt = isOvertimeExempt.IsOvertimeExempt;
            }
            return retVal;
        }

        ///<summary>Calculates daily overtime.  Daily overtime does not take into account any time adjust events.
        ///All manually entered time adjust events are assumed to be entered correctly and should not be used in calculating automatic totals.
        ///Throws exceptions when encountering errors.</summary>
        public static void CalculateDailyOvertime(Employee employee, DateTime dateStart, DateTime dateStop)
        {
            #region Fill Lists, validate data sets, generate error messages.
            List<ClockEvent> listClockEvent = new List<ClockEvent>();
            List<ClockEvent> listClockEventBreak = new List<ClockEvent>();
            TimeCardRule timeCardRule = new TimeCardRule();
            string errors = "";
            string clockErrors = "";
            string breakErrors = "";
            string ruleErrors = "";
            //If calculating breaks before the end date of the pay period, only calculate breaks and validate clock in and out events for days
            //before today.  Use the server time just because we are dealing with time cards.
            DateTime dateTimeNow = MiscData.GetNowDateTime();
            ClockEvent lastClockEvent = ClockEvent.GetLastEvent(employee.Id);
            bool isStillWorking = (lastClockEvent != null && (lastClockEvent.Status == ClockEventStatus.Break || lastClockEvent.Date2Displayed == null));
            if (dateTimeNow.Date < dateStop.Date && isStillWorking)
            {
                dateStop = dateTimeNow.Date.AddDays(-1);
            }
            //Fill lists and catch validation error messages------------------------------------------------------------------------------------------------------------
            try
            {
                listClockEvent = ClockEvent.GetValidList(employee.Id, dateStart, dateStop, false);
            }
            catch (Exception ex)
            {
                clockErrors += ex.Message;
            }
            try
            {
                listClockEventBreak = ClockEvent.GetValidList(employee.Id, dateStart, dateStop, true);
            }
            catch (Exception ex)
            {
                breakErrors += ex.Message;
            }
            try
            {
                timeCardRule = GetTimeCardRule(employee);
            }
            catch (Exception ex)
            {
                ruleErrors += ex.Message;
            }
            //Validation between two or more lists above----------------------------------------------------------------------------------------------------------------
            for (int b = 0; b < listClockEventBreak.Count; b++)
            {
                bool isValidBreak = false;
                for (int c = 0; c < listClockEvent.Count; c++)
                {
                    if (timeClockEventsOverlapHelper(listClockEventBreak[b], listClockEvent[c]))
                    {
                        if (listClockEventBreak[b].Date1Displayed > listClockEvent[c].Date1Displayed//break started after work did
                            && listClockEventBreak[b].Date2Displayed < listClockEvent[c].Date2Displayed)//break ended before working hours
                        {
                            //valid break.
                            isValidBreak = true;
                            break;
                        }
                        //invalid break.
                        isValidBreak = false;//redundant, but harmless. Makes code more readable.
                        break;
                    }
                }
                if (isValidBreak)
                {
                    continue;
                }
                breakErrors += "  " + listClockEventBreak[b].Date1Displayed.ToString() + " : break found during non-working hours.\r\n";//ToString() instead of ToShortDateString() to show time.
            }
            //Report Errors---------------------------------------------------------------------------------------------------------------------------------------------
            errors = ruleErrors + clockErrors + breakErrors;
            if (errors != "")
            {
                throw new Exception(Employee.GetNameFL(employee) + " has the following errors:\r\n" + errors);
                //throw new Exception(errors);
            }
            #endregion
            #region Fill time card rules
            //Begin calculations=========================================================================================================================================
            TimeSpan tsHoursWorkedTotal = new TimeSpan();
            TimeSpan tsOvertimeHoursRule = new TimeSpan(24, 0, 0);//Example 10:00 for overtime rule after 10 hours per day.
            TimeSpan tsDifferentialAMRule = new TimeSpan();//Example 06:00 for differential rule before 6am.
            TimeSpan tsDifferentialPMRule = new TimeSpan(24, 0, 0);//Example 17:00 for differential rule after  5pm.
                                                                   //Fill over hours rule from list-------------------------------------------------------------------------------------
            if (timeCardRule.Hours != TimeSpan.Zero)
            {//OverHours Rule
                tsOvertimeHoursRule = timeCardRule.Hours;//at most, one non-zero OverHours rule available at this point.
            }
            if (timeCardRule.TimeStart != TimeSpan.Zero)
            {//AM Rule
                tsDifferentialAMRule = timeCardRule.TimeStart;//at most, one non-zero AM rule available at this point.
            }
            if (timeCardRule.TimeEnd != TimeSpan.Zero)
            {//PM Rule
                tsDifferentialPMRule = timeCardRule.TimeEnd;//at most, one non-zero PM rule available at this point.
            }
            #endregion
            //Calculations: Regular Time, Overtime, Rate2 time---------------------------------------------------------------------------------------------------
            TimeSpan tsDailyBreaksAdjustTotal = new TimeSpan();//used to adjust the clock event
            TimeSpan tsDailyBreaksTotal = new TimeSpan();//used in calculating breaks over 30 minutes per day.
            TimeSpan tsDailyDifferentialTotal = new TimeSpan();//hours before and after AM/PM diff rules. Adjusted for overbreaks.
                                                               //Note: If TimeCardsMakesAdjustmentsForOverBreaks is true, only the first 30 minutes of break per day are paid. 
                                                               //All breaktime thereafter will be calculated as if the employee was clocked out at that time.
            for (int i = 0; i < listClockEvent.Count; i++)
            {
                #region  Differential pay (including overbreak adjustments)--------------------------------------------------------------
                if (i == 0 || listClockEvent[i].Date1Displayed.Date != listClockEvent[i - 1].Date1Displayed.Date)
                {
                    tsDailyDifferentialTotal = TimeSpan.Zero;
                }
                //AM-----------------------------------
                if (listClockEvent[i].Date1Displayed.TimeOfDay < tsDifferentialAMRule)
                {//clocked in before AM differential rule
                    tsDailyDifferentialTotal += tsDifferentialAMRule - listClockEvent[i].Date1Displayed.TimeOfDay;
                    if (listClockEvent[i].Date2Displayed?.TimeOfDay < tsDifferentialAMRule)
                    {//clocked out before AM differential rule also
                        tsDailyDifferentialTotal += listClockEvent[i].Date1Displayed.TimeOfDay - tsDifferentialAMRule;//add a negative timespan
                    }
                    //Adjust AM differential by overbreaks-----
                    TimeSpan tsAMBreakTimeCounter = new TimeSpan();//tracks all break time for use in calculating overages.
                    TimeSpan tsAMBreakDuringDiff = new TimeSpan();//tracks only the portion of breaks that occured during differential hours.
                    for (int b = 0; b < listClockEventBreak.Count; b++)
                    {
                        if (tsAMBreakTimeCounter > TimeSpan.FromMinutes(30))
                        {
                            tsAMBreakTimeCounter = TimeSpan.FromMinutes(30);//reset overages for next calculation.
                        }
                        if (listClockEventBreak[b].Date1Displayed.Date != listClockEvent[i].Date1Displayed.Date)
                        {
                            continue;//skip breaks for other days.
                        }
                        tsAMBreakTimeCounter += listClockEventBreak[b].Date2Displayed.Value - listClockEventBreak[b].Date1Displayed;
                        tsAMBreakDuringDiff += calcDifferentialPortion(tsDifferentialAMRule, TimeSpan.FromHours(24), listClockEventBreak[b]);
                        if (tsAMBreakTimeCounter < TimeSpan.FromMinutes(30))
                        {
                            continue;//not over thirty minutes yet.
                        }
                        if (timeClockEventsOverlapHelper(listClockEvent[i], listClockEventBreak[b]))
                        {
                            continue;//There must be multiple clock events for this day, and we have gone over breaks during a later clock event period
                        }
                        if (listClockEventBreak[b].Date1Displayed.TimeOfDay > tsDifferentialAMRule)
                        {
                            continue;//this break started after the AM differential so there is nothing left to do in this loop. break out of the entire loop.
                        }
                        if (listClockEventBreak[b].Date2Displayed?.TimeOfDay - (tsAMBreakTimeCounter - TimeSpan.FromMinutes(30)) > tsDifferentialAMRule)
                        {
                            continue;//entirety of break overage occured after AM differential time.
                        }
                        //Make adjustments because: 30+ minutes of break, break occured during clockEvent, break started before the AM rule.
                        TimeSpan tsAMAdjustAmount = System.TimeSpan.FromMinutes(30) - tsAMBreakTimeCounter;
                        if (tsAMAdjustAmount < -tsAMBreakDuringDiff)
                        {
                            tsAMAdjustAmount = -tsAMBreakDuringDiff;//cannot adjust off more break overage time than we have had breaks during this time.
                        }
                        tsDailyDifferentialTotal += tsAMAdjustAmount;//adjust down
                        tsAMBreakDuringDiff += tsAMAdjustAmount;//adjust down
                    }
                }
                //PM-------------------------------------
                if (listClockEvent[i].Date2Displayed?.TimeOfDay > tsDifferentialPMRule)
                {//clocked out after PM differential rule
                    tsDailyDifferentialTotal += (listClockEvent[i].Date2Displayed?.TimeOfDay - tsDifferentialPMRule) ?? TimeSpan.Zero;
                    if (listClockEvent[i].Date1Displayed.TimeOfDay > tsDifferentialPMRule)
                    {//clocked in after PM differential rule also
                        tsDailyDifferentialTotal += tsDifferentialPMRule - listClockEvent[i].Date1Displayed.TimeOfDay;//add a negative timespan
                    }
                    //Adjust PM differential by overbreaks-----
                    TimeSpan tsPMBreakTimeCounter = new TimeSpan();//tracks all break time for use in calculating overages.
                    TimeSpan tsPMBreakDuringDiff = new TimeSpan();//tracks only the portion of breaks that occured during differential hours.
                    for (int b = 0; b < listClockEventBreak.Count; b++)
                    {
                        if (tsPMBreakTimeCounter > TimeSpan.FromMinutes(30))
                        {
                            tsPMBreakTimeCounter = TimeSpan.FromMinutes(30);//reset overages for next calculation.
                        }
                        if (listClockEventBreak[b].Date1Displayed.Date != listClockEvent[i].Date1Displayed.Date)
                        {
                            continue;//skip breaks for other days.
                        }
                        tsPMBreakTimeCounter += listClockEventBreak[b].Date2Displayed.Value - listClockEventBreak[b].Date1Displayed;
                        tsPMBreakDuringDiff += calcDifferentialPortion(TimeSpan.Zero, tsDifferentialPMRule, listClockEventBreak[b]);
                        if (tsPMBreakTimeCounter < TimeSpan.FromMinutes(30))
                        {
                            continue;//not over thirty minutes yet.
                        }
                        if (!timeClockEventsOverlapHelper(listClockEvent[i], listClockEventBreak[b]))
                        {
                            continue;//There must be multiple clock events for this day, and we have gone over breaks during a different clock event period
                        }
                        if (listClockEventBreak[b].Date2Displayed?.TimeOfDay < tsDifferentialPMRule)
                        {
                            continue;//entirety of break overage occured before PM differential time.
                        }
                        //Make adjustments because: 30+ minutes of break, break occured during clockEvent, break ended after the PM rule.
                        TimeSpan tsPMAdjustAmount = System.TimeSpan.FromMinutes(30) - tsPMBreakTimeCounter;//tsPMBreakTimeCounter is always > 30 at this point in time
                        if (tsPMAdjustAmount < -tsPMBreakDuringDiff)
                        {
                            tsPMAdjustAmount = -tsPMBreakDuringDiff;//cannot adjust off more break overage time than we have had breaks during this time.
                        }
                        tsDailyDifferentialTotal += tsPMAdjustAmount;//adjust down
                        tsPMBreakDuringDiff += tsPMAdjustAmount;//adjust down
                    }
                }
                //Apply differential to clock event-----------------------------------------------------------------------------------
                if (tsDailyDifferentialTotal < TimeSpan.Zero)
                {
                    //this should never happen. If it ever does, we need to know about it, because that means some math has been miscalculated.
                    throw new Exception(" - " + listClockEvent[i].Date1Displayed.Date.ToShortDateString() + ", " + employee.FirstName + " " + employee.LastName + " : calculated differential hours was negative.");
                }
                listClockEvent[i].Rate2Auto = tsDailyDifferentialTotal;//should be zero or greater.
                #endregion
                #region Regular hours and OT hours calulations (including overbreak adjustments)----------------------------------------
                listClockEvent[i].OvertimeAuto = TimeSpan.Zero;
                listClockEvent[i].AdjustAuto = TimeSpan.Zero;
                if (i == 0 || listClockEvent[i].Date1Displayed.Date != listClockEvent[i - 1].Date1Displayed.Date)
                {
                    tsHoursWorkedTotal = TimeSpan.Zero;
                    tsDailyBreaksAdjustTotal = TimeSpan.Zero;
                    tsDailyBreaksTotal = TimeSpan.Zero;
                    tsDailyDifferentialTotal = TimeSpan.Zero;
                }
                tsHoursWorkedTotal += listClockEvent[i].Date2Displayed.Value - listClockEvent[i].Date1Displayed;//Hours worked
                if (tsHoursWorkedTotal > tsOvertimeHoursRule)
                {//if OverHoursPerDay then make AutoOTAdjustments.
                    listClockEvent[i].OvertimeAuto += tsHoursWorkedTotal - tsOvertimeHoursRule;//++OTimeAuto
                                                                                               //listClockEvent[i].AdjustAuto-=tsHoursWorkedTotal-tsOvertimeHoursRule;//--AdjustAuto
                    tsHoursWorkedTotal = tsOvertimeHoursRule;//subsequent clock events should be counted as overtime.
                }
                if (i == listClockEvent.Count - 1 || listClockEvent[i].Date1Displayed.Date != listClockEvent[i + 1].Date1Displayed.Date)
                {
                    //Either the last clock event in the list or last clock event for the day.
                    //OVERBREAKS--------------------------------------------------------------------------------------------------------
                    if (Preference.GetBool(PreferenceName.TimeCardsMakesAdjustmentsForOverBreaks))
                    {//Apply overbreaks to this clockEvent.
                        tsDailyBreaksAdjustTotal = new TimeSpan();//used to adjust the clock event
                        tsDailyBreaksTotal = new TimeSpan();//used in calculating breaks over 30 minutes per day.
                        for (int b = 0; b < listClockEventBreak.Count; b++)
                        {//check all breaks for current day.
                            if (listClockEventBreak[b].Date1Displayed.Date != listClockEvent[i].Date1Displayed.Date)
                            {
                                continue;//skip breaks for other dates than current ClockEvent
                            }
                            tsDailyBreaksTotal += (listClockEventBreak[b].Date2Displayed?.TimeOfDay - listClockEventBreak[b].Date1Displayed.TimeOfDay) ?? TimeSpan.Zero;
                            if (tsDailyBreaksTotal > TimeSpan.FromMinutes(31))
                            {//over 31 to avoid adjustments less than 1 minutes.
                                listClockEventBreak[b].AdjustAuto = TimeSpan.FromMinutes(30) - tsDailyBreaksTotal;
                                ClockEvent.Update(listClockEventBreak[b]);//save adjustments to breaks.
                                tsDailyBreaksAdjustTotal += listClockEventBreak[b].AdjustAuto;
                                tsDailyBreaksTotal = TimeSpan.FromMinutes(30);//reset daily breaks to 30 minutes so the next break is all adjustment.
                            }//end overBreaks>31 minutes
                        }//end checking all breaks for current day
                         //OverBreaks applies to overtime and then to RegularTime
                        listClockEvent[i].OvertimeAuto += tsDailyBreaksAdjustTotal;//tsDailyBreaksTotal<=TimeSpan.Zero
                        listClockEvent[i].AdjustAuto += tsDailyBreaksAdjustTotal;//tsDailyBreaksTotal is less than or equal to zero
                        if (listClockEvent[i].OvertimeAuto < TimeSpan.Zero)
                        {//we have adjusted OT too far
                         //listClockEvent[i].AdjustAuto+=listClockEvent[i].OTimeAuto;
                            listClockEvent[i].OvertimeAuto = TimeSpan.Zero;
                        }
                        tsDailyBreaksTotal = TimeSpan.Zero;//zero out for the next day.
                        tsHoursWorkedTotal = TimeSpan.Zero;//zero out for next day.
                    }//end overbreaks
                }
                #endregion
                ClockEvent.Update(listClockEvent[i]);
            }//end clockEvent loop.
        }

        private static TimeSpan calcDifferentialPortion(TimeSpan tsDifferentialAMRule, TimeSpan tsDifferentialPMRule, ClockEvent clockEventBreak)
        {
            TimeSpan retVal = TimeSpan.Zero;

            if (clockEventBreak.Date2Displayed.HasValue)
            {
                //AM overlap==========================================================
                //Visual representation
                //AM Rule      :           X
                //Entire Break :o-------o  |             Stop-Start == Entire Break
                //Partial Break:      o----|---o         Rule-Start == Partial Break
                //No Break     :           |  o------o   Rule-Rule  == No break (won't actually happen in this block)
                retVal += TimeSpan.FromTicks(
                    Math.Min(clockEventBreak.Date2Displayed.Value.TimeOfDay.Ticks, tsDifferentialAMRule.Ticks)//min of stop or rule
                    - Math.Min(clockEventBreak.Date1Displayed.TimeOfDay.Ticks, tsDifferentialAMRule.Ticks)//min of start or rule
                    );//equals the entire break, part of the break, or non of the break.
                      //PM overlap==========================================================
                      //Visual representation
                      //PM Rule      :           X
                      //Entire Break :o-------o  |             Rule-Rule   == No Break
                      //Partial Break:      o----|---o         Stop-Rule   == Partial Break
                      //No Break     :           |  o------o   Stop-Start  == Entire break
                retVal += TimeSpan.FromTicks(
                    Math.Max(clockEventBreak.Date2Displayed.Value.TimeOfDay.Ticks, tsDifferentialPMRule.Ticks)//max of stop or rule
                    - Math.Max(clockEventBreak.Date1Displayed.TimeOfDay.Ticks, tsDifferentialPMRule.Ticks)//max of start or rule
                    );//equals the entire break, part of the break, or non of the break.
            }
            return retVal;
        }

        ///<summary>Returns true if two clock events overlap. Useful for determining if a break applies to a given clock event.  
        ///Does not matter which order clock events are provided.</summary>
        private static bool timeClockEventsOverlapHelper(ClockEvent clockEvent1, ClockEvent clockEvent2)
        {
            //Visual representation
            //ClockEvent1:            o----------------o
            //ClockEvent2:o---------------o   or  o-------------------o
            if (clockEvent2.Date2Displayed > clockEvent1.Date1Displayed
                && clockEvent2.Date1Displayed < clockEvent1.Date2Displayed)
            {
                return true;
            }
            return false;
        }

        ///<summary>Updates OTimeAuto, AdjustAuto (calculated and set above., and Rate2Auto based on the rules passed in, and calculated break time overages.</summary>
        private static void AdjustAutoClockEventEntriesHelper(List<ClockEvent> listClockEvent, List<ClockEvent> listClockEventBreak, TimeSpan tsDifferentialAMRule, TimeSpan tsDifferentialPMRule, TimeSpan tsOvertimeHoursRule)
        {
            for (int i = 0; i < listClockEvent.Count; i++)
            {
                //listClockEvent[i].OTimeAuto	=TimeSpan.Zero;
                listClockEvent[i].AdjustAuto = TimeSpan.Zero;
                listClockEvent[i].Rate2Auto = TimeSpan.Zero;
                //OTimeAuto and AdjustAuto---------------------------------------------------------------------------------
                //if((listClockEvent[i].TimeDisplayed2.TimeOfDay-listClockEvent[i].TimeDisplayed1.TimeOfDay)>tsOvertimeHoursRule) {
                //	listClockEvent[i].OTimeAuto+=listClockEvent[i].TimeDisplayed2.TimeOfDay-listClockEvent[i].TimeDisplayed1.TimeOfDay-tsOvertimeHoursRule;
                //listClockEvent[i].AdjustAuto+=-listClockEvent[i].OTimeAuto;
                //}
                //AdjustAuto due to break overages-------------------------------------------------------------------------
                if (Preference.GetBool(PreferenceName.TimeCardsMakesAdjustmentsForOverBreaks))
                {
                    if (i == listClockEvent.Count - 1 || listClockEvent[i].Date1Displayed.Date != listClockEvent[i + 1].Date1Displayed.Date)
                    {//last item or last item for a given day.
                        TimeSpan tsTotalBreaksToday = TimeSpan.Zero;
                        for (int j = 0; j < listClockEventBreak.Count; j++)
                        {
                            if (listClockEventBreak[j].Date1Displayed.Date != listClockEvent[i].Date1Displayed.Date)
                            {//skip breaks that occured on different days.
                                continue;
                            }
                            tsTotalBreaksToday += (listClockEventBreak[j].Date2Displayed?.TimeOfDay - listClockEventBreak[j].Date1Displayed.TimeOfDay) ?? TimeSpan.Zero;
                        }
                        if (tsTotalBreaksToday > TimeSpan.FromMinutes(31))
                        {
                            listClockEvent[i].AdjustAuto += TimeSpan.FromMinutes(30) - tsTotalBreaksToday;//should add a negative time span.
                            listClockEvent[i].OvertimeAuto += TimeSpan.FromMinutes(30) - tsTotalBreaksToday;//should add a negative time span.
                            if (listClockEvent[i].OvertimeAuto < TimeSpan.Zero)
                            {//if we removed too much overbreak from otAuto, remove it from adjust auto instead and set otauto to zero
                                listClockEvent[i].AdjustAuto += listClockEvent[i].OvertimeAuto;
                                listClockEvent[i].OvertimeAuto = TimeSpan.Zero;
                            }
                            tsTotalBreaksToday = TimeSpan.FromMinutes(30);//reset break today to 30 minutes, so next break is entirely overBreak.
                        }
                    }
                }
                //Rate2Auto-------------------------------------------------------------------------------------------------
                if (listClockEvent[i].Date1Displayed.TimeOfDay < tsDifferentialAMRule)
                {//AM, example rule before 8am, work from 5am to 7am
                    listClockEvent[i].Rate2Auto += tsDifferentialAMRule - listClockEvent[i].Date1Displayed.TimeOfDay;//8am-5am=3hrs
                    if (listClockEvent[i].Date2Displayed.HasValue && listClockEvent[i].Date2Displayed.Value.TimeOfDay < tsDifferentialAMRule)
                    {
                        listClockEvent[i].Rate2Auto += listClockEvent[i].Date2Displayed.Value.TimeOfDay - tsDifferentialAMRule;//8am-7am=-1hr =>2hrs total
                    }
                }
                if (listClockEvent[i].Date2Displayed.HasValue && listClockEvent[i].Date2Displayed.Value.TimeOfDay > tsDifferentialPMRule)
                {//PM, example diffRule after 8pm, work from 9 to 11pm. 
                    listClockEvent[i].Rate2Auto += listClockEvent[i].Date2Displayed.Value.TimeOfDay - tsDifferentialPMRule;//11pm-8pm = 3hrs 
                    if (listClockEvent[i].Date1Displayed.TimeOfDay > tsDifferentialPMRule)
                    {
                        listClockEvent[i].Rate2Auto += tsDifferentialPMRule - listClockEvent[i].Date1Displayed.TimeOfDay;//8pm-9pm = -1hr =>2hrs total
                    }
                }
                ClockEvent.Update(listClockEvent[i]);
            }//end ClockEvent list
        }

        ///<summary>Deprecated.  This function is aesthetic and has no bearing on actual OT calculations. It adds adjustments to breaks so that when viewing them you can see if they went over 30 minutes.</summary>
        private static void AdjustBreaksHelper(Employee EmployeeCur, DateTime StartDate, DateTime StopDate)
        {
            if (!Preference.GetBool(PreferenceName.TimeCardsMakesAdjustmentsForOverBreaks))
            {
                //Only adjust breaks if preference is set.
                return;
            }
            List<ClockEvent> breakList = ClockEvent.Refresh(EmployeeCur.Id, StartDate, StopDate, true);//PIn.Date(textDateStart.Text),PIn.Date(textDateStop.Text),true);
            TimeSpan totalToday = TimeSpan.Zero;
            TimeSpan totalOne = TimeSpan.Zero;
            DateTime previousDate = DateTime.MinValue;
            for (int b = 0; b < breakList.Count; b++)
            {
                if (breakList[b].Date2Displayed == null)
                {
                    return;
                }
                if (breakList[b].Date1Displayed.Date != breakList[b].Date2Displayed.Value.Date)
                {
                    //MsgBox.Show(this,"Error. One break spans multiple dates.");
                    return;
                }
                //calc time for the one break
                totalOne = breakList[b].Date2Displayed.Value - breakList[b].Date1Displayed;
                //calc daily total
                if (previousDate.Date != breakList[b].Date1Displayed.Date)
                {//if date changed, this is the first pair of the day
                    totalToday = TimeSpan.Zero;//new day
                    previousDate = breakList[b].Date1Displayed.Date;//for the next loop
                }
                totalToday += totalOne;
                //decide if breaks for the day went over 30 min.
                if (totalToday > TimeSpan.FromMinutes(31))
                {//31 to prevent silly fractions less than 1.
                    breakList[b].AdjustAuto = -(totalToday - TimeSpan.FromMinutes(30));
                    ClockEvent.Update(breakList[b]);
                    totalToday = TimeSpan.FromMinutes(30);//reset to 30.  Therefore, any additional breaks will be wholly adjustments.
                }
            }//end breaklist
        }

        ///<summary>Calculates weekly overtime and inserts TimeAdjustments accordingly.</summary>
        public static void CalculateWeeklyOvertime_Old(Employee EmployeeCur, DateTime StartDate, DateTime StopDate)
        {
            List<TimeAdjustment> TimeAdjustList = TimeAdjustment.Refresh(EmployeeCur.Id, StartDate, StopDate);
            List<ClockEvent> ClockEventList = ClockEvent.Refresh(EmployeeCur.Id, StartDate, StopDate, false);
            //first, delete all existing overtime entries
            for (int i = 0; i < TimeAdjustList.Count; i++)
            {
                if (TimeAdjustList[i].HoursOvertime == TimeSpan.Zero)
                {
                    continue;
                }
                if (!TimeAdjustList[i].IsAuto)
                {
                    continue;
                }
                TimeAdjustment.Delete(TimeAdjustList[i]);
            }
            //refresh list after it has been cleaned up.
            TimeAdjustList = TimeAdjustment.Refresh(EmployeeCur.Id, StartDate, StopDate);
            ArrayList mergedAL = new ArrayList();
            foreach (ClockEvent clockEvent in ClockEventList)
            {
                mergedAL.Add(clockEvent);
            }
            foreach (TimeAdjustment timeAdjust in TimeAdjustList)
            {
                mergedAL.Add(timeAdjust);
            }
            //then, fill grid
            Calendar cal = CultureInfo.CurrentCulture.Calendar;
            CalendarWeekRule rule = CalendarWeekRule.FirstFullWeek;//CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule;
                                                                   //rule=CalendarWeekRule.FirstFullWeek;//CalendarWeekRule is an Enum. For these calculations, we want to use FirstFullWeek, not FirstDay;
            List<TimeSpan> WeeklyTotals = new List<TimeSpan>();
            WeeklyTotals = FillWeeklyTotalsHelper(true, EmployeeCur, mergedAL);
            //loop through all rows
            for (int i = 0; i < mergedAL.Count; i++)
            {
                //ignore rows that aren't weekly totals
                if (i < mergedAL.Count - 1//if not the last row
                                          //if the next row has the same week as this row
                    && cal.GetWeekOfYear(GetDateForRowHelper(mergedAL[i + 1]), rule, (DayOfWeek)Preference.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek))//Default is 0-Sunday
                    == cal.GetWeekOfYear(GetDateForRowHelper(mergedAL[i]), rule, (DayOfWeek)Preference.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek)))
                {
                    continue;
                }
                if (WeeklyTotals[i] <= TimeSpan.FromHours(40))
                {
                    continue;
                }
                //found a weekly total over 40 hours
                TimeAdjustment adjust = new TimeAdjustment
                {
                    IsAuto = true,
                    EmployeeId = EmployeeCur.Id,
                    Date = GetDateForRowHelper(mergedAL[i]).AddHours(20),//puts it at 8pm on the same day.
                    HoursOvertime = WeeklyTotals[i] - TimeSpan.FromHours(40)
                };
                adjust.HoursRegular = -adjust.HoursOvertime;
                TimeAdjustment.Insert(adjust);
            }

        }

        ///<summary>Calculates weekly overtime and inserts TimeAdjustments accordingly.</summary>
        public static void CalculateWeeklyOvertime(Employee EmployeeCur, DateTime StartDate, DateTime StopDate)
        {
            TimeCardRule timeCardRule = GetTimeCardRule(EmployeeCur);
            if (timeCardRule != null && timeCardRule.IsOvertimeExempt)
            {
                return;
            }
            List<ClockEvent> listClockEvent = new List<ClockEvent>();
            List<TimeAdjustment> listTimeAdjust = new List<TimeAdjustment>();
            string errors = "";
            string clockErrors = "";
            string timeAdjustErrors = "";
            //Fill lists and catch validation error messages------------------------------------------------------------------------------------------------------------
            try { listClockEvent = ClockEvent.GetValidList(EmployeeCur.Id, StartDate, StopDate, false); } catch (Exception ex) { clockErrors += ex.Message; }
            try { listTimeAdjust = TimeAdjustment.GetValidList(EmployeeCur.Id, StartDate, StopDate); } catch (Exception ex) { timeAdjustErrors += ex.Message; }
            //Report Errors---------------------------------------------------------------------------------------------------------------------------------------------
            errors = clockErrors + timeAdjustErrors;
            if (errors != "")
            {
                throw new Exception(Employee.GetNameFL(EmployeeCur) + " has the following errors:\r\n" + errors);
            }
            //first, delete all existing non manual overtime entries
            for (int i = 0; i < listTimeAdjust.Count; i++)
            {
                if (listTimeAdjust[i].IsAuto)
                {
                    TimeAdjustment.Delete(listTimeAdjust[i]);
                }
            }
            //refresh list after it has been cleaned up.
            listTimeAdjust = TimeAdjustment.Refresh(EmployeeCur.Id, StartDate, StopDate);
            ArrayList mergedAL = MergeClockEventAndTimeAdjust(listClockEvent, listTimeAdjust);
            //then, fill grid
            Calendar cal = CultureInfo.CurrentCulture.Calendar;
            CalendarWeekRule rule = CalendarWeekRule.FirstFullWeek;//CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule;
                                                                   //rule=CalendarWeekRule.FirstFullWeek;//CalendarWeekRule is an Enum. For these calculations, we want to use FirstFullWeek, not FirstDay;
            List<TimeSpan> WeeklyTotals = new List<TimeSpan>();
            WeeklyTotals = FillWeeklyTotalsHelper(true, EmployeeCur, mergedAL);
            //loop through all rows
            int weekIdx = 0;//first week index==0
            for (int i = 0; i < mergedAL.Count; i++)
            {
                //ignore rows that aren't weekly totals
                if (i < mergedAL.Count - 1//if not the last row
                                          //if the next row has the same week as this row
                    && cal.GetWeekOfYear(GetDateForRowHelper(mergedAL[i + 1]), rule, (DayOfWeek)Preference.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek))//Default is 0-Sunday
                    == cal.GetWeekOfYear(GetDateForRowHelper(mergedAL[i]), rule, (DayOfWeek)Preference.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek)))
                {
                    continue;//continue within a single week
                }
                if (WeeklyTotals[i] <= TimeSpan.FromHours(40))
                {
                    weekIdx++;//Going to the next week, go to the next week's entry in the list of tuples
                    continue;
                }
                //======CALUCLATE WEEKLY OVERTIME ADJUSTMENTS IF NEEDED======
                //found a weekly total over 40 hours
                List<Tuple<long?, TimeSpan>> listEvents = _listEvents[weekIdx];//stores all worked hours per clinic in the order they were worked, sans dates, for a week.
                                                                              //validate the list of clock events.
                if (listEvents.GroupBy(x => x.Item1).Any(x => x.Select(y => y.Item2.TotalHours).Sum() < 0))
                {
                    //should never happen.
                    throw new ApplicationException("Clock events for employee total a negative number of hours for a clinic.");
                }
                TimeSpan weeklyHours = TimeSpan.Zero;
                //this tracks each OT entry as it occurs, in the order it occured, so that we can properly account for negative adjustments later.
                List<Tuple<long?, TimeSpan>> listOTEntries = new List<Tuple<long?, TimeSpan>>();
                Dictionary<long?, TimeSpan> dictOvertime = new Dictionary<long?, TimeSpan>();
                foreach (Tuple<long?, TimeSpan> tupleCur in listEvents)
                {
                    TimeSpan prevTotal = TimeSpan.FromHours(weeklyHours.TotalHours);//deep copy of timeSpan.
                    weeklyHours = weeklyHours.Add(tupleCur.Item2);//add new timespan to running total.
                    if (tupleCur.Item2 < TimeSpan.Zero)
                    { //negative span
                        TimeSpan negTime = -tupleCur.Item2;//store as positive balance to simplify comparisons below.
                                                           //First try to compensate "using up" Ot from this clinic
                        for (int j = listOTEntries.Count - 1; j >= 0; j--)
                        {
                            if (listOTEntries[j].Item1 != tupleCur.Item1)
                            {//only events for this clinic to start with.
                                continue;
                            }
                            if (listOTEntries[j].Item2 > negTime)
                            {
                                listOTEntries[j] = new Tuple<long?, TimeSpan>(listOTEntries[j].Item1, listOTEntries[j].Item2.Subtract(negTime));
                                negTime = TimeSpan.Zero;
                                break;//we zeroed out the adjustment using only overtime from this clinic.
                            }
                            else
                            {
                                negTime = negTime.Subtract(listOTEntries[j].Item2);
                                listOTEntries[j] = new Tuple<long?, TimeSpan>(listOTEntries[j].Item1, listOTEntries[j].Item2.Subtract(listOTEntries[j].Item2));//zero it out.
                            }
                        }
                        //houskeeping
                        listOTEntries.RemoveAll(x => x.Item2 == TimeSpan.Zero);
                        //possibly adjust off OT using time from other clinics in the order the time was accrued.
                        for (int j = listOTEntries.Count - 1; j >= 0; j--)
                        {
                            if (negTime <= TimeSpan.Zero)
                            {
                                break;
                            }
                            if (listOTEntries[j].Item2 > negTime)
                            {
                                listOTEntries[j] = new Tuple<long?, TimeSpan>(listOTEntries[j].Item1, listOTEntries[j].Item2.Subtract(negTime));
                                negTime = TimeSpan.Zero;
                                break;//we zeroed out the adjustment using only overtime from this clinic.
                            }
                            else
                            {
                                negTime = negTime.Subtract(listOTEntries[j].Item2);
                                listOTEntries[j] = new Tuple<long?, TimeSpan>(listOTEntries[j].Item1, listOTEntries[j].Item2.Subtract(listOTEntries[j].Item2));//zero it out.
                            }
                        }
                        //houskeeping
                        listOTEntries.RemoveAll(x => x.Item2 == TimeSpan.Zero);
                    }
                    if (weeklyHours.TotalHours > 40)
                    {//this clock event put us into overtime.
                        var otEntry = new Tuple<long?, TimeSpan>(tupleCur.Item1, TimeSpan.FromHours(weeklyHours.TotalHours - Math.Max(40, prevTotal.TotalHours)));
                        if (otEntry.Item2 > TimeSpan.Zero)
                        {
                            listOTEntries.Add(otEntry);
                        }
                    }
                }
                //Build dictOvertime by aggregating all entries in list above. Dict contains one entry per clinic with a timespan for OT time worked.
                dictOvertime = listOTEntries.GroupBy(x => x.Item1).ToDictionary(x => x.Key, x => TimeSpan.FromHours(x.Select(y => y.Item2.TotalHours).Sum()));
                //======ADD OT ADJUSTMENTS FOR ONE WEEK AFTER ALL CALCULATIONS FOR THAT WEEK HAVE BEEN COMPLETED======
                foreach (KeyValuePair<long?, TimeSpan> kvp in dictOvertime)
                {
                    if (kvp.Value <= TimeSpan.Zero)
                    {
                        continue;
                    }
                    TimeAdjustment adjust = new TimeAdjustment
                    {
                        IsAuto = true,
                        ClinicId = kvp.Key,
                        EmployeeId = EmployeeCur.Id,
                        Date = GetDateForRowHelper(mergedAL[i]).AddHours(20),
                        HoursOvertime = kvp.Value
                    };
                    adjust.HoursRegular = -adjust.HoursOvertime;
                    TimeAdjustment.Insert(adjust);
                }
                weekIdx++;
            }
        }

        ///<summary>Merges a list of ClockEvent and a list of TimeAdjust, sorted by ClockEvent.TimeDisplayed1 and TimeAdjust.TimeEntry</summary>
        private static ArrayList MergeClockEventAndTimeAdjust(List<ClockEvent> listClockEvents, List<TimeAdjustment> listTimeAdjusts)
        {
            List<ClockEvent> listOrderedClockEvents = listClockEvents.OrderBy(x => x.Date1Displayed).ToList();//Oldest entries first
            List<TimeAdjustment> listOrderedTimeAdjusts = listTimeAdjusts.OrderBy(x => x.Date).ToList();
            ArrayList mergedAL = new ArrayList();
            int idxCE = 0;
            int idxTA = 0;
            while (idxCE < listOrderedClockEvents.Count || idxTA < listOrderedTimeAdjusts.Count)
            {//Merge listClockEvent and listTimeAdjust, sort by TimeDisplayed1/TimeEntry
                if (idxCE > listOrderedClockEvents.Count || idxTA > listOrderedTimeAdjusts.Count)
                {
                    break;//Shouldn't happen.
                }
                if (idxCE == listOrderedClockEvents.Count)
                {//All ClockEvents added, so remaining TimeAdjusts will all be added.
                    mergedAL.Add(listOrderedTimeAdjusts[idxTA]);
                    idxTA++;
                }
                else if (idxTA == listOrderedTimeAdjusts.Count)
                {//All TimeAdjusts added, so remaining ClockEvents will all be added.
                    mergedAL.Add(listOrderedClockEvents[idxCE]);//So add next ClockEvent
                    idxCE++;
                }
                else if (listOrderedClockEvents[idxCE].Date1Displayed <= listOrderedTimeAdjusts[idxTA].Date)
                {//ClockEvent is next
                    mergedAL.Add(listOrderedClockEvents[idxCE]);
                    idxCE++;
                }
                else
                {//TimeAdjust is next
                    mergedAL.Add(listOrderedTimeAdjusts[idxTA]);
                    idxTA++;
                }
            }
            return mergedAL;
        }

        /// <summary>This was originally analogous to the FormTimeCard.FillGrid(), before this logic was moved to the business layer.</summary>
        private static List<TimeSpan> FillWeeklyTotalsHelper(bool fromDB, Employee EmployeeCur, ArrayList mergedAL)
        {
            List<Tuple<long?, TimeSpan>> listWeek = new List<Tuple<long?, TimeSpan>>();
            _listEvents = new List<List<Tuple<long?, TimeSpan>>>();
            List<TimeSpan> retVal = new List<TimeSpan>();
            TimeSpan[] WeeklyTotals = new TimeSpan[mergedAL.Count];
            TimeSpan alteredSpan = TimeSpan.Zero;//used to display altered times
            TimeSpan oneSpan = TimeSpan.Zero;//used to sum one pair of clock-in/clock-out
            TimeSpan oneAdj;
            TimeSpan oneOT;
            TimeSpan daySpan = TimeSpan.Zero;//used for daily totals.
            TimeSpan weekSpan = TimeSpan.Zero;//used for weekly totals.
            if (mergedAL.Count > 0)
            {
                weekSpan = ClockEvent.GetWeekTotal(EmployeeCur.Id, GetDateForRowHelper(mergedAL[0]));
            }
            bool prevHours = weekSpan != TimeSpan.Zero;
            TimeSpan periodSpan = TimeSpan.Zero;//used to add up totals for entire page. (Not used. Left over from when this code existed in the UI.)
            TimeSpan otspan = TimeSpan.Zero;//overtime for the entire period
            Calendar cal = CultureInfo.CurrentCulture.Calendar;
            CalendarWeekRule rule = CalendarWeekRule.FirstFullWeek;// CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule;
            DateTime curDate = DateTime.MinValue;
            DateTime previousDate = DateTime.MinValue;
            Type type;
            ClockEvent clock;
            TimeAdjustment adjust;
            for (int i = 0; i < mergedAL.Count; i++)
            {
                type = mergedAL[i].GetType();
                previousDate = curDate;
                //clock event row---------------------------------------------------------------------------------------------
                if (type == typeof(ClockEvent))
                {
                    clock = (ClockEvent)mergedAL[i];
                    if (prevHours)
                    {//Add in previous pay period's hours for this week if the week started in the middle of last payperiod.  Only need to do it once.
                        listWeek.Add(Tuple.Create(clock.ClinicId, weekSpan));
                        prevHours = false;
                    }
                    curDate = clock.Date1Displayed.Date;
                    if (clock.Date2Displayed == null)
                    {
                        //ignore clock event where user has not clocked out yet.
                    }
                    else
                    {
                        oneSpan = clock.Date2Displayed.Value - clock.Date1Displayed;
                        daySpan += oneSpan;
                        weekSpan += oneSpan;
                        periodSpan += oneSpan;
                        listWeek.Add(Tuple.Create(clock.ClinicId, oneSpan));
                    }
                    //Adjust---------------------------------
                    oneAdj = TimeSpan.Zero;

                        oneAdj += clock.Adjust ?? clock.AdjustAuto;

                    daySpan += oneAdj;
                    weekSpan += oneAdj;
                    periodSpan += oneAdj;
                    if (oneAdj != TimeSpan.Zero)
                    {//take adjustments from breaks away from the OT values in the dictionary
                        listWeek.Add(Tuple.Create(clock.ClinicId, oneAdj));
                    }
                    //Overtime------------------------------

                    oneOT = clock.Overtime ?? clock.OvertimeAuto;
                    otspan += oneOT;
                    daySpan -= oneOT;
                    weekSpan -= oneOT;
                    periodSpan -= oneOT;
                    if (oneOT > TimeSpan.Zero)
                    {
                        listWeek.Add(Tuple.Create(clock.ClinicId, -oneOT));
                    }
                    //Daily-----------------------------------
                    //if this is the last entry for a given date
                    if (i == mergedAL.Count - 1//if this is the last row
                || GetDateForRowHelper(mergedAL[i + 1]) != curDate)//or the next row is a different date
                    {
                        daySpan = TimeSpan.Zero;
                    }
                    else
                    {//not the last entry for the day
                    }
                    //Weekly-------------------------------------
                    WeeklyTotals[i] = weekSpan;
                    //if this is the last entry for a given week
                    if (i == mergedAL.Count - 1//if this is the last row 
                || cal.GetWeekOfYear(GetDateForRowHelper(mergedAL[i + 1]), rule, (DayOfWeek)Preference.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek))//or the next row has a
                != cal.GetWeekOfYear(clock.Date1Displayed.Date, rule, (DayOfWeek)Preference.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek)))//different week of year
                    {
                        _listEvents.Add(listWeek);
                        listWeek = new List<Tuple<long?, TimeSpan>>();//start over for the next week.
                        weekSpan = TimeSpan.Zero;
                    }
                }
                //adjustment row--------------------------------------------------------------------------------------
                else if (type == typeof(TimeAdjustment))
                {
                    adjust = (TimeAdjustment)mergedAL[i];
                    curDate = adjust.Date.Date;
                    //Adjust------------------------------
                    daySpan += adjust.HoursRegular;//might be negative
                    weekSpan += adjust.HoursRegular;
                    periodSpan += adjust.HoursRegular;
                    oneAdj = adjust.HoursRegular;
                    if (oneAdj != TimeSpan.Zero)
                    {
                        listWeek.Add(Tuple.Create(adjust.ClinicId, oneAdj));
                    }
                    //Overtime------------------------------
                    otspan += adjust.HoursOvertime;
                    oneOT = adjust.HoursOvertime;
                    if (oneOT != TimeSpan.Zero)
                    {
                        listWeek.Add(Tuple.Create(adjust.ClinicId, oneOT));
                    }
                    //Daily-----------------------------------
                    //if this is the last entry for a given date
                    if (i == mergedAL.Count - 1//if this is the last row
                || GetDateForRowHelper(mergedAL[i + 1]) != curDate)//or the next row is a different date
                    {
                        daySpan = new TimeSpan(0);
                    }
                    //Weekly-------------------------------------
                    WeeklyTotals[i] = weekSpan;
                    //if this is the last entry for a given week
                    if (i == mergedAL.Count - 1//if this is the last row 
                || cal.GetWeekOfYear(GetDateForRowHelper(mergedAL[i + 1]), rule, (DayOfWeek)Preference.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek))//or the next row has a
                != cal.GetWeekOfYear(adjust.Date.Date, rule, (DayOfWeek)Preference.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek)))//different week of year
                    {
                        _listEvents.Add(listWeek);
                        listWeek = new List<Tuple<long?, TimeSpan>>();//start over for the next week.
                        weekSpan = new TimeSpan(0);
                    }
                }
            }
            foreach (TimeSpan week in WeeklyTotals)
            {
                retVal.Add(week);
            }
            return retVal;
        }

        private static DateTime GetDateForRowHelper(object timeEvent)
        {
            if (timeEvent.GetType() == typeof(ClockEvent))
            {
                return ((ClockEvent)timeEvent).Date1Displayed.Date;
            }
            else if (timeEvent.GetType() == typeof(TimeAdjustment))
            {
                return ((TimeAdjustment)timeEvent).Date.Date;
            }
            return DateTime.MinValue;
        }
    }
}
