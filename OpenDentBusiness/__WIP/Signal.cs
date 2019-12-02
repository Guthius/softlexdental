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
using CodeBase;
using MySql.Data.MySqlClient;
using OpenDentBusiness.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OpenDentBusiness
{
    /// <summary>
    /// Signals are the means by which we communicate between workstations.
    /// </summary>
    public class Signal : DataRecord
    {
        private static readonly object threadLock = new object();
        private static Thread thread;

        /// <summary>
        /// Represents a method that can process a list of signals.
        /// </summary>
        /// <param name="signals">The signals to process.</param>
        public delegate void SignalProcessor(IEnumerable<Signal> signals);

        /// <summary>
        /// The exact server time the signal was created.
        /// </summary>
        public DateTime Date;

        /// <summary>
        /// The signal key.
        /// </summary>
        public string Name;

        /// <summary>
        /// The ID of the external record that relates to the signal.
        /// </summary>
        public long? ExternalId;

        /// <summary>
        /// A date related the record identified by <see cref="ExternalId"/>, typically the 
        /// last modified date of the record pointed at by <see cref="ExternalId"/>.
        /// </summary>
        public DateTime? ExternalDate;

        /// <summary>
        /// The first (optional) parameter of the signal.
        /// </summary>
        public string Param1;

        /// <summary>
        /// The second (optional) parameter of the signal.
        /// </summary>
        public string Param2;

        /// <summary>
        /// The message value of the signal.
        /// </summary>
        public string Message;

        /// <summary>
        /// Constructs a new instance of the <see cref="Signal"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Signal"/> instance.</returns>
        private static Signal FromReader(MySqlDataReader dataReader)
        {
            return new Signal
            {
                Id = (long)dataReader["id"],
                Date = (DateTime)dataReader["date"],
                Name = (string)dataReader["name"],
                ExternalId = dataReader["external_id"] as long?,
                ExternalDate = dataReader["external_date"] as DateTime?,
                Param1 = (string)dataReader["param1"],
                Param2 = (string)dataReader["param2"],
                Message = (string)dataReader["message"]
            };
        }

        /// <summary>
        /// Checks whether this signal is equal to the specified object.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is Signal other)
            {
                if (other == this) return true;

                if (Id != other.Id ||
                    Date != other.Date ||
                    ExternalId != other.ExternalId ||
                    ExternalDate != other.ExternalDate ||
                    Name != other.Name ||
                    Param1 != other.Param1 ||
                    Param2 != other.Param2 ||
                    Message != other.Message)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// We must define GetHashCode() because we defined Equals() above, or else we get a warning message.
        /// </summary>
        public override int GetHashCode() => (int)Id;

        /// <summary>
        /// This is not the actual date/time last refreshed. It is really the server based 
        /// date/time of the last item in the database retrieved on previous refreshes. That way, 
        /// the local workstation time is irrelevant.
        /// </summary>
        public static DateTime LastRefreshDate;

        /// <summary>
        /// Raised whenever there are signals to be processed.
        /// </summary>
        public static event SignalProcessor Process = delegate { };

        /// <summary>
        /// Raises the <see cref="Process"/> event.
        /// </summary>
        /// <param name="signals"></param>
        protected static void OnProcess(IEnumerable<Signal> signals) => Process.Invoke(signals);

        /// <summary>
        /// Gets a list of all signals that have been added since the specified date.
        /// </summary>
        /// <param name="dateTimeSince"></param>
        /// <param name="signalNames">The names of the signals to include in the list; if null all signals will be included.</param>
        /// <returns>A list of signals.</returns>
        public static List<Signal> GetSince(DateTime dateTimeSince, List<string> signalNames = null)
        {
            var commandText = "SELECT * FROM `signals` WHERE (`date` > ?date AND `date` < NOW())";
            if (signalNames != null)
            {
                commandText += " AND `name` IN(" + string.Join(", ", signalNames.Select(signalName => "'" + MySqlHelper.EscapeString(signalName) + "'")) + ") ";
            }
            commandText += " GROUP BY `name`, `external_id`, `external_date`, `param1`, `param2` ORDER BY `date`";

            return SelectMany(commandText, FromReader,
                new MySqlParameter("date", dateTimeSince));
        }

        /// <summary>
        /// Performs a single update tick.
        /// </summary>
        private static void Tick()
        {
            // TODO: Right now this has second precision, something better would be nice...

            var signals = GetSince(LastRefreshDate).Distinct();

            if (signals.Count() == 0) return;

            LastRefreshDate = signals.Max(signal => signal.Date);

            // Get all signals that relate to cache invalidation...
            var invalidateCacheSignals =
                signals.Where(
                    signal =>
                        signal.Name == SignalName.CacheInvalidate && 
                        !string.IsNullOrEmpty(signal.Param1));

            // Fetch all fee signals...
            var feeSignals =
                invalidateCacheSignals.Where(
                    signal => 
                        signal.Param1 == typeof(FeeSched).FullName && 
                        signal.ExternalId.HasValue);

            if (feeSignals.Count() > 0)
            {
                Fees.InvalidateFeeSchedules(
                    feeSignals.Select(
                        feeSignal => feeSignal.ExternalId.Value).ToList());
            }

            foreach (var signal in invalidateCacheSignals)
            {
                try
                {
                    var type = Type.GetType(signal.Param1);
                    if (type != null)
                    {
                        CacheManager.Invalidate(type);
                    }
                }
                catch
                {
                    // TODO: Log the invalid type...
                }
            }

            OnProcess(signals);

            Thread.Sleep(100);
        }

        /// <summary>
        /// Starts processing signals.
        /// </summary>
        public static void StartProcessing()
        {
            lock (threadLock)
            {
                if (thread != null) return;

                thread = new Thread(Tick);
                thread.Start();
            }
        }

        /// <summary>
        /// Stops processing signals.
        /// </summary>
        public static void StopProcessing()
        {
            lock (threadLock)
            {
                if (thread != null)
                {
                    try
                    {
                        thread.Abort();
                    }
                    finally
                    {
                        thread = null;
                    }
                }
            }
        }

        /// <summary>
        /// Inserts the specified signals into the database.
        /// </summary>
        /// <param name="signals">The signals.</param>
        /// <returns>The ID assigned to the last signal.</returns>
        public static long Insert(params Signal[] signals)
        {
            if (signals == null || signals.Length == 0) return 0;

            var currentDateTime = MiscData.GetNowDateTime();

            foreach (var signal in signals)
            {
                signal.Date = currentDateTime;
                signal.Id = DataConnection.ExecuteInsert(
                    "INSERT INTO `signals` (`date`, `name`, `external_id`, `param1`, `param2`, `message`) " +
                    "VALUES (?date, ?name, ?external_id, ?param1, ?param2, ?message)",
                        new MySqlParameter("date", signal.Date),
                        new MySqlParameter("name", signal.Name ?? ""),
                        new MySqlParameter("external_id", signal.ExternalId.HasValue ? (object)signal.ExternalId.Value : DBNull.Value),
                        new MySqlParameter("param1", signal.Param1 ?? ""),
                        new MySqlParameter("param2", signal.Param2 ?? ""),
                        new MySqlParameter("message", signal.Message ?? ""));
            }

            return signals[signals.Length - 1].Id;
        }

        /// <summary>
        /// <para>
        /// Suspends signal processing by the specified number of seconds.
        /// </para>
        /// <para>
        /// This effectively makes the signal processor ignore all alerts that are generated during
        /// the suspended period. This can lead to the program entering a bad state. Only use this
        /// when its absolutely neccesary.
        /// </para>
        /// </summary>
        /// <param name="seconds">The number of seconds to suspend signal processing for.</param>
        public static void SuspendProcessing(int seconds) => LastRefreshDate = MiscData.GetNowDateTime().AddSeconds(seconds);

        /// <summary>
        /// Returns true if the <see cref="Appointment.AptDateTime"/> is between 
        /// <see cref="DateTime.Today"/> and the number of days specified by the
        /// <see cref="PreferenceName.ApptAutoRefreshRange"/> preference.
        /// </summary>
        public static bool IsApptInRefreshRange(Appointment appointment)
        {
            if (appointment == null) return false;

            int days = Preference.GetInt(PreferenceName.ApptAutoRefreshRange);
            if (days == -1)
            {
                return true;
            }

            return appointment.AptDateTime.Between(DateTime.Today, DateTime.Today.AddDays(days));
        }

        /// <summary>
        /// The given dateStart must be less than or equal to dateEnd. Both dates must be valid
        /// dates (not min date, etc).
        /// </summary>
        public static void SetInvalidSchedForOps(Dictionary<DateTime, List<long>> operatoryIdsByDate)
        {
            var signals = new List<Signal>();

            foreach (var date in operatoryIdsByDate.Keys)
            {
                var uniqueOperatoryIds = operatoryIdsByDate[date].Distinct();

                foreach (var operatoryId in uniqueOperatoryIds)
                {
                    signals.Add(new Signal
                    {
                        Name = SignalName.ScheduleChanged,
                        ExternalId = operatoryId,
                        Param1 = "operatory",
                        Param2 = date.ToString()
                    });
                }
            }

            Insert(signals.ToArray());
        }

        /// <summary>
        /// Schedules, when we don't have a specific FKey and want to set an invalid for the entire
        /// type. Includes the dateViewing parameter for Refresh. A dateViewing of 01-01-0001 will
        /// be ignored because it would otherwise cause a full refresh for all connected client 
        /// workstations.
        /// </summary>
        public static void SetInvalidSched(DateTime dateViewing)
        {
            // A dateViewing of 01-01-0001 will be ignored because it would otherwise cause a full 
            // refresh for all connected client workstations.
            if (dateViewing == DateTime.MinValue) return;

            Insert(new Signal()
            {
                Name = SignalName.ScheduleChanged,
                Param1 = dateViewing.ToString()
            });
        }




        /// <summary>
        /// Pass one or two appointments into this function to send 2 to 6 invalid signals 
        /// depending on the changes made to the appointment. Always call a refresh of the 
        /// appointment module before calling this method. Cannot pass null for both 
        /// parameters.
        /// </summary>
        /// <param name="newAppointment">
        /// Required. If changes are made to an appointment or a new appointment is made, it should 
        /// be passed in here.
        /// </param>
        /// <param name="oldAppointment">
        /// Optional. Only used if changes are being made to an existing appointment. Generally
        /// should not be called outside of Appointments.cs
        /// </param>
        public static void SetInvalidAppt(Appointment newAppointment, Appointment oldAppointment = null)
        {
            if (newAppointment == null)
            {
                // If apptOld is not null then use it as the apptNew so we can send signals
                // Most likely occurred due to appointment delete.
                if (oldAppointment != null)
                {
                    newAppointment = oldAppointment;
                    oldAppointment = null;
                }
                else
                {
                    return; // Should never happen. Both apptNew and apptOld are null in this scenario
                }
            }

            // The six possible signals are:
            //   1. New Provider
            //   2. New Hyg
            //   3. New Op
            //   4. Old Provider
            //   5. Old Hyg
            //   6. Old Op

            // If there is no change between new and old, or if there is not an old appt provided, then fewer than 6 signals may be generated.
            var signals = new List<Signal>();
            if (IsApptInRefreshRange(newAppointment))
            {
                //  1.New Provider
                signals.Add(
                    new Signal()
                    {
                        ExternalId = newAppointment.ProvNum,
                        ExternalDate = newAppointment.AptDateTime,
                        Name = "appointment",
                        Param1 = "provider",
                    });
                //  2.New Hyg
                if (newAppointment.ProvHyg > 0)
                {
                    signals.Add(
                        new Signal()
                        {
                            ExternalId = newAppointment.ProvHyg,
                            ExternalDate = newAppointment.AptDateTime,
                            Name = "appointment",
                            Param1 = "provider",
                        });
                }
                //  3.New Op
                if (newAppointment.Op > 0)
                {
                    signals.Add(
                        new Signal()
                        {
                            ExternalId = newAppointment.Op,
                            ExternalDate = newAppointment.AptDateTime,
                            Name = "appointment",
                            Param1 = "operatory",
                        });
                }
            }

            if (IsApptInRefreshRange(oldAppointment))
            {
                //  4.Old Provider
                if (oldAppointment != null && oldAppointment.ProvNum > 0 && (oldAppointment.AptDateTime.Date != newAppointment.AptDateTime.Date || oldAppointment.ProvNum != newAppointment.ProvNum))
                {
                    signals.Add(
                        new Signal()
                        {
                            ExternalId = oldAppointment.ProvNum,
                            ExternalDate = oldAppointment.AptDateTime,
                            Name = "appointment",
                            Param1 = "provider",
                        });
                }
                //  5.Old Hyg
                if (oldAppointment != null && oldAppointment.ProvHyg > 0 && (oldAppointment.AptDateTime.Date != newAppointment.AptDateTime.Date || oldAppointment.ProvHyg != newAppointment.ProvHyg))
                {
                    signals.Add(
                        new Signal()
                        {
                            ExternalId = oldAppointment.ProvHyg,
                            ExternalDate = oldAppointment.AptDateTime,
                            Name = "appointment",
                            Param1 = "provider",
                        });
                }
                //  6.Old Op
                if (oldAppointment != null && oldAppointment.Op > 0 && (oldAppointment.AptDateTime.Date != newAppointment.AptDateTime.Date || oldAppointment.Op != newAppointment.Op))
                {
                    signals.Add(
                        new Signal()
                        {
                            ExternalId = oldAppointment.Op,
                            ExternalDate = oldAppointment.AptDateTime,
                            Name = "appointment",
                            Param1 = "operatory",
                        });
                }
            }

            signals.ForEach(x => Insert(x));
        }





        /// <summary>
        /// Inserts a signal for each operatory in the schedule that has been changed, and for the
        /// provider the schedule is for. This only inserts a signal for today's schedules. 
        /// Generally should not be called outside of Schedules.cs
        /// </summary>
        public static void SetInvalidSched(params Schedule[] schedules)
        {
            // Per Nathan, we are only going to insert signals for today's schedules. Most
            // workstations will not be looking at other days for extended lengths of time.

            var currentDateTime = MiscData.GetNowDateTime();

            // Make an array of signals for every operatory involved.
            var operatorySignals = schedules
                .Where(
                    schedule =>
                        schedule.SchedDate == DateTime.Today ||
                        schedule.SchedDate == currentDateTime.Date)
                .SelectMany(
                    schedule => schedule.Ops.Select(
                        operatoryId =>
                            new Signal()
                            {
                                Name = SignalName.ScheduleChanged,
                                ExternalId = operatoryId,
                                ExternalDate = schedule.SchedDate,
                                Param1 = "operatory",
                            }))
                .ToArray();

            // Make a array of signals for every provider involved.
            var providerSignals =
                schedules.Where(schedule => schedule.ProvNum > 0)
                    .Where(
                        schedule =>
                            schedule.SchedDate == DateTime.Today ||
                            schedule.SchedDate == currentDateTime.Date)
                    .Select(
                        schedule =>
                            new Signal()
                            {
                                Name = SignalName.ScheduleChanged,
                                ExternalId = schedule.ProvNum,
                                ExternalDate = schedule.SchedDate,
                                Param1 = "provider",
                            })
                    .ToArray();

            var uniqueSignals = operatorySignals.Union(providerSignals).ToArray();
            if (uniqueSignals.Length > 1000)
            {
                // We've had offices insert tens of thousands of signals at once which severely 
                // slowed down their database.
                Insert(new Signal
                {
                    Name = SignalName.ScheduleChanged
                });

                return;
            }

            Insert(uniqueSignals);
        }

        /// <summary>
        /// Inserts an InvalidType.SmsTextMsgReceivedUnreadCount signal which tells all client 
        /// machines to update the received unread SMS message count. To get the current count from
        /// the database, use SmsFromMobiles.GetSmsNotification().
        /// </summary>
        public static long InsertSmsNotification(string json)
        {
            return Insert(new Signal()
            {
                Name = "sms_message_count",
                Message = json,
            });
        }



        /// <summary>
        /// Check for appointment signals for a single date.
        /// </summary>
        public static bool IsApptRefreshNeeded(DateTime dateTimeShowing, List<Signal> signals) =>
            IsApptRefreshNeeded(dateTimeShowing, dateTimeShowing, signals);

        /// <summary>
        /// After a refresh, this is used to determine whether the Appt Module needs to be 
        /// refreshed. Returns true if there are any signals with InvalidType=Appointment 
        /// where the DateViewing time of the signal falls within the provided daterange, and the 
        /// signal matches either the list of visible operatories or visible providers in the 
        /// current Appt Module View. Always returns true if any signals have
        /// DateViewing=DateTime.MinVal.
        /// </summary>
        public static bool IsApptRefreshNeeded(DateTime startDate, DateTime endDate, IEnumerable<Signal> signals)
        {
            if (signals.Any(s => !s.ExternalDate.HasValue && s.Name == "appointment"))
                return true;

            // Get all signals within the specified date range.
            signals =
                signals.Where(
                    signal =>
                        signal.Name == "appointment" &&
                        signal.ExternalDate.Value.Date >= startDate.Date &&
                        signal.ExternalDate.Value.Date <= endDate.Date);

            if (signals.Count() == 0) return false;

            var visibleOperatoryIds = ApptDrawing.VisOps.Select(x => x.Id).ToList();
            var visibleProviderIds = ApptDrawing.VisProvs.Select(x => x.Id).ToList();

            if (signals.Any(x => x.Param1 == "operatory" && visibleOperatoryIds.Contains(x.ExternalId.Value)) ||
                signals.Any(x => x.Param1 == "provider" && visibleProviderIds.Contains(x.ExternalId.Value)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check for schedule signals for a single date.
        /// </summary>
        public static bool IsSchedRefreshNeeded(DateTime dateTimeShowing, List<Signal> signals) =>
            IsSchedRefreshNeeded(dateTimeShowing, dateTimeShowing, signals);

        /// <summary>
        /// After a refresh, this is used to determine whether the Appt Module needs to be 
        /// refreshed. Returns true if there are any signals with InvalidType=Appointment where the 
        /// DateViewing time of the signal falls within the provided daterange, and the signal 
        /// matches either the list of visible operatories or visible providers in the current 
        /// Appt Module View.  Always returns true if any signals have 
        /// DateViewing=DateTime.MinVal.
        /// </summary>
        public static bool IsSchedRefreshNeeded(DateTime startDate, DateTime endDate, IEnumerable<Signal> signals)
        {
            // Reduce the list of signals to only signals related to scheduling...
            signals = 
                signals.Where(
                    signal => signal.Name == SignalName.ScheduleChanged);

            // IF there is a signal without a date, that means we have to do a global refresh.
            if (signals.Any(signal => !signal.ExternalDate.HasValue)) return true;

            // Reduce the list to only signals that fall within the specified date range.
            signals =
                signals.Where(
                    signal =>
                        signal.ExternalDate.HasValue &&
                        signal.ExternalDate.Value.Date >= startDate.Date &&
                        signal.ExternalDate.Value.Date <= endDate.Date);

            if (signals.Count() == 0) return false;

            var visibleOperatoryIds = ApptDrawing.VisOps.Select(x => x.Id).ToList();
            var visibleProviderIds = ApptDrawing.VisProvs.Select(x => x.Id).ToList();

            if (signals.Any(x => x.Param1 == "operatory" && visibleOperatoryIds.Contains(x.ExternalId.Value)) ||
                signals.Any(x => x.Param1 == "provider" && visibleProviderIds.Contains(x.ExternalId.Value)) ||
                signals.Any(x => x.Param1 == ""))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes all signals older than 2 days if this has not been run within the last week.
        /// </summary>
        public static void ClearOldSignals()
        {
            var currentDateTime = MiscData.GetNowDateTime();

            if (Preference.GetDateTime(PreferenceName.SignalLastClearedDate) <= currentDateTime.AddDays(-7))
            {
                // Delete all signals older then 2 days.
                DataConnection.ExecuteNonQuery("DELETE FROM `signals` WHERE `date` < DATE_ADD(NOW(), INTERVAL -2 DAY)");

                // Clear messaging buttons which use to be stored in the signal table.
                SigMessages.ClearOldSigMessages();

                // Set Last cleared to now.
                Preference.Update(PreferenceName.SignalLastClearedDate, currentDateTime);
            }
        }
    }




    ///<summary>Do not combine with SignalType, they must be seperate. Stored as string, safe to reorder enum values.</summary>
    public enum KeyType
    {
        ///<summary>0</summary>
        Undefined = 0,
        ///<summary>1</summary>
        FeeSched,
        ///<summary>2</summary>
        Job,
        ///<summary>3</summary>
        Operatory,
        ///<summary>5</summary>
        Provider,
        ///<summary>6</summary>
        SigMessage,
        ///<summary>7 - Special KeyType that does not use a FK but instead will set FKey to a count of unread messages.
        ///Used along side the SmsTextMsgReceivedUnreadCount InvalidType.</summary>
        SmsMsgUnreadCount,
        ///<summary>8</summary>
        Task,
        ///<summary>9 - Used to identify which signals a form can ignore.  If the FKey==Process.GetCurrentProcess().Id then this process sent it so ignore
        ///it.  Used in FormTerminal, FormTerminalManager, and FormSheetFillEdit (for forms being filled at a kiosk).</summary>
        ProcessId,
    }
}