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
            return null; // TODO: Implement me...
        }

        public static ClockEvent GetById(long clockEventId) =>
            SelectOne("SELECT * FROM clock_events WHERE id = " + clockEventId, FromReader);

        public static void Delete(long clockEventId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM clock_events WHERE id = " + clockEventId);

        public ClockEvent()
        {
            Overtime = TimeSpan.FromHours(-1);
            Rate2 = TimeSpan.FromHours(-1);
        }

        public ClockEvent Copy() => (ClockEvent)MemberwiseClone();
    }
}
