using CodeBase;
using OpenDentBusiness.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace OpenDentBusiness
{
    [Obsolete]
    public class Signalods
    {
        private static readonly List<ISignalProcessor> signalProcessors = new List<ISignalProcessor>();

        /// <summary>
        /// This is not the actual date/time last refreshed. It is really the server based 
        /// date/time of the last item in the database retrieved on previous refreshes. That way, 
        /// the local workstation time is irrelevant.
        /// </summary>
        public static DateTime SignalLastRefreshed;

        /// <summary>
        /// Mimics the behavior of SignalLastRefreshed, but is used exclusively in 
        /// ContrAppt.TickRefresh(). The root issue was that when a client came back from being 
        /// inactive ContrAppt.TickRefresh() was using SignalLastRefreshed, which is only set after
        /// we process signals. Therefore, when a client went inactive, we could potentially query
        /// the SignalOD table for a much larger dataset than intended. E.g.- Client goes inactive 
        /// for 3 hours, comes back, ContrAppt.TickRefresh() is called and calls RefreshTimed() 
        /// with a 3 hour old datetime.
        /// </summary>
        public static DateTime ApptSignalLastRefreshed;

        /// <summary>
        /// Called when loading control to subscribe a given control for signal processing.
        /// </summary>
        public static bool SubscribeSignalProcessor(ISignalProcessor signalProcessor)
        {
            if (signalProcessor is Form form)
            {
                form.FormClosed += delegate
                {
                    signalProcessors.Remove(signalProcessor);
                };
            }
            else if (signalProcessor is Window window)
            {
                window.Closed += delegate
                {
                    signalProcessors.Remove(signalProcessor);
                };
            }
            else
            {
                return false;
            }

            signalProcessors.Add(signalProcessor);

            return true;
        }

        /// <summary>
        /// Retreives new signals from the DB, updates Caches, and broadcasts signals to all 
        /// subscribed forms.
        /// </summary>
        public static void SignalsTick(Action shutdownAction, Action<List<ISignalProcessor>, List<Signalod>> processAction, Action doneAction)
        {
            var refreshThread = new ODThread(thread =>
            {
                var signals = RefreshTimed(SignalLastRefreshed);

                if (signals.Count > 0)
                {
                    SignalLastRefreshed = signals.Max(x => x.SigDateTime);
                    ApptSignalLastRefreshed = SignalLastRefreshed;
                }

                if (signals.Count == 0) return;

                if (signals.Exists(x => x.IType == InvalidType.ShutDownNow))
                {
                    shutdownAction();
                    return;
                }

                var feeSignals = signals.FindAll(x => x.IType == InvalidType.Fees && x.FKeyType == KeyType.FeeSched && x.FKey > 0);
                if (feeSignals.Count > 0)
                {
                    Fees.InvalidateFeeSchedules(feeSignals.Select(x => x.FKey).ToList());
                }

                var invalidTypes = 
                    signals
                        .FindAll(
                            signal => 
                                signal.FKey == 0 && 
                                signal.FKeyType == KeyType.Undefined)
                        .Select(
                            signal => 
                                signal.IType)
                        .Distinct();

                Cache.Refresh(true, invalidTypes.ToArray());

                processAction(signalProcessors, signals);
            });

            refreshThread.AddExceptionHandler(exception =>
            {
                DateTime dateTimeRefreshed;
                try
                {
                    dateTimeRefreshed = MiscData.GetNowDateTime();
                }
                catch
                {
                    // If the server cannot be reached, we still need to move the signal processing
                    // forward so use local time as a fail-safe.
                    dateTimeRefreshed = DateTime.Now;
                }
                SignalLastRefreshed = dateTimeRefreshed;
                ApptSignalLastRefreshed = dateTimeRefreshed;
            });

            refreshThread.AddExitHandler(thread => doneAction());
            refreshThread.Name = "SignalsTick";
            refreshThread.Start(true);
        }

        /// <summary>
        /// Gets all Signals since a given DateTime. If it can't connect to the database, then it 
        /// returns a list of length 0. Remeber that the supplied dateTime is server time. This 
        /// has to be accounted for. ListITypes is an optional parameter for querying specific 
        /// signal types.
        /// </summary>
        public static List<Signalod> RefreshTimed(DateTime sinceDateTime, List<InvalidType> invalidTypes = null)
        {
            // This command was written to take into account the fact that MySQL truncates seconds
            // to the the whole second on DateTime columns. (newer versions support fractional 
            // seconds) By selecting signals less than Now() we avoid missing signals the next time
            // this function is called. Without the addition of Now() it was possible to miss up 
            // to ((N-1)/N)% of the signals generated in the worst case scenario.
            string command = "SELECT * FROM signalod WHERE (SigDateTime>" + POut.DateT(sinceDateTime) + " AND SigDateTime< " + DbHelper.Now() + ") ";
            if (!invalidTypes.IsNullOrEmpty())
            {
                command += "AND IType IN(" + String.Join(",", invalidTypes.Select(x => (int)x)) + ") ";
            }
            command += "ORDER BY SigDateTime";

            // Note: this might return an occasional row that has both times newer.
            var signals = new List<Signalod>();
            try
            {
                signals = Crud.SignalodCrud.SelectMany(command);
            }
            catch
            {
                // We don't want an error message to show, because that can cause a cascade of a
                // large number of error messages.
            }

            return signals;
        }

        /// <summary>
        /// Returns the PK of the signal inserted if only one signal was passed in; Otherwise, 
        /// returns 0.
        /// </summary>
        public static long Insert(params Signalod[] signals)
        {
            if (signals == null || signals.Length == 0) return 0;

            // We need to explicitly get the server time in advance rather than using NOW(), 
            // because we need to update the signal object soon after creation.
            var currentDateTime = MiscData.GetNowDateTime();
            foreach (var signal in signals)
            {
                signal.SigDateTime = currentDateTime;
            }

            if (signals.Length == 1)
                return Crud.SignalodCrud.Insert(signals[0]);

            Crud.SignalodCrud.InsertMany(signals.ToList());
            return 0;
        }

        /// <summary>
        /// Simplest way to use the new fKey and FKeyType. Set isBroadcast=true to process
        /// signals immediately on workstation.
        /// </summary>
        public static long SetInvalid(InvalidType invalidType, KeyType keyType, long key)
        {
            return Insert(new Signalod
            {
                IType = invalidType,
                DateViewing = DateTime.MinValue,
                FKey = key,
                FKeyType = keyType
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
            var signals = new List<Signalod>();
            if (IsApptInRefreshRange(newAppointment))
            {
                //  1.New Provider
                signals.Add(
                    new Signalod()
                    {
                        DateViewing = newAppointment.AptDateTime,
                        IType = InvalidType.Appointment,
                        FKey = newAppointment.ProvNum,
                        FKeyType = KeyType.Provider,
                    });
                //  2.New Hyg
                if (newAppointment.ProvHyg > 0)
                {
                    signals.Add(
                        new Signalod()
                        {
                            DateViewing = newAppointment.AptDateTime,
                            IType = InvalidType.Appointment,
                            FKey = newAppointment.ProvHyg,
                            FKeyType = KeyType.Provider,
                        });
                }
                //  3.New Op
                if (newAppointment.Op > 0)
                {
                    signals.Add(
                        new Signalod()
                        {
                            DateViewing = newAppointment.AptDateTime,
                            IType = InvalidType.Appointment,
                            FKey = newAppointment.Op,
                            FKeyType = KeyType.Operatory,
                        });
                }
            }

            if (IsApptInRefreshRange(oldAppointment))
            {
                //  4.Old Provider
                if (oldAppointment != null && oldAppointment.ProvNum > 0 && (oldAppointment.AptDateTime.Date != newAppointment.AptDateTime.Date || oldAppointment.ProvNum != newAppointment.ProvNum))
                {
                    signals.Add(
                        new Signalod()
                        {
                            DateViewing = oldAppointment.AptDateTime,
                            IType = InvalidType.Appointment,
                            FKey = oldAppointment.ProvNum,
                            FKeyType = KeyType.Provider,
                        });
                }
                //  5.Old Hyg
                if (oldAppointment != null && oldAppointment.ProvHyg > 0 && (oldAppointment.AptDateTime.Date != newAppointment.AptDateTime.Date || oldAppointment.ProvHyg != newAppointment.ProvHyg))
                {
                    signals.Add(
                        new Signalod()
                        {
                            DateViewing = oldAppointment.AptDateTime,
                            IType = InvalidType.Appointment,
                            FKey = oldAppointment.ProvHyg,
                            FKeyType = KeyType.Provider,
                        });
                }
                //  6.Old Op
                if (oldAppointment != null && oldAppointment.Op > 0 && (oldAppointment.AptDateTime.Date != newAppointment.AptDateTime.Date || oldAppointment.Op != newAppointment.Op))
                {
                    signals.Add(
                        new Signalod()
                        {
                            DateViewing = oldAppointment.AptDateTime,
                            IType = InvalidType.Appointment,
                            FKey = oldAppointment.Op,
                            FKeyType = KeyType.Operatory,
                        });
                }
            }
            signals.ForEach(x => Insert(x));

            // There was a delay when using this method to refresh the appointment module due to
            // the time it takes to loop through the signals that iSignalProcessors need to loop
            // through.
            // BroadcastSignals(listSignals);//for immediate update. Signals will be processed again at next tick interval.
        }

        /// <summary>
        /// Returns true if the Apppointment.AptDateTime is between DateTime.Today and the number
        /// of ApptAutoRefreshRange preference days.
        /// </summary>
        public static bool IsApptInRefreshRange(Appointment appointment)
        {
            if (appointment == null)  return false;
            
            int days = Preference.GetInt(PreferenceName.ApptAutoRefreshRange);
            if (days == -1)
            {
                // ApptAutoRefreshRange preference is -1, so all appointments are in range
                return true;
            }

            // Returns true if the appointment is between today and today + the auto refresh day
            // range preference.
            return appointment.AptDateTime.Between(DateTime.Today, DateTime.Today.AddDays(days));
        }

        /// <summary>
        /// The given dateStart must be less than or equal to dateEnd. Both dates must be valid
        /// dates (not min date, etc).
        /// </summary>
        public static void SetInvalidSchedForOps(Dictionary<DateTime, List<long>> operatoryIdsByDate)
        {
            var signals = new List<Signalod>();

            foreach (var date in operatoryIdsByDate.Keys)
            {
                var uniqueOperatoryIds = operatoryIdsByDate[date].Distinct();

                foreach (var operatoryId in uniqueOperatoryIds)
                {
                    signals.Add(new Signalod
                    {
                        IType = InvalidType.Schedules,
                        DateViewing = date,
                        FKey = operatoryId,
                        FKeyType = KeyType.Operatory
                    });
                }
            }

            Insert(signals.ToArray());
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
                            new Signalod()
                            {
                                IType = InvalidType.Schedules,
                                DateViewing = schedule.SchedDate,
                                FKey = operatoryId,
                                FKeyType = KeyType.Operatory,
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
                            new Signalod()
                            {
                                IType = InvalidType.Schedules,
                                DateViewing = schedule.SchedDate,
                                FKey = schedule.ProvNum,
                                FKeyType = KeyType.Provider,
                            })
                    .ToArray();

            var uniqueSignals = operatorySignals.Union(providerSignals).ToArray();
            if (uniqueSignals.Length > 1000)
            {
                // We've had offices insert tens of thousands of signals at once which severely 
                // slowed down their database.
                Insert(new Signalod
                {
                    IType = InvalidType.Schedules,
                    DateViewing = DateTime.MinValue,
                });

                return;
            }

            Insert(uniqueSignals);
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
            if (dateViewing == DateTime.MinValue)  return;

            Insert(new Signalod()
            {
                IType = InvalidType.Schedules,
                DateViewing = dateViewing
            });
        }

        /// <summary>
        /// Inserts an InvalidType.SmsTextMsgReceivedUnreadCount signal which tells all client 
        /// machines to update the received unread SMS message count. To get the current count from
        /// the database, use SmsFromMobiles.GetSmsNotification().
        /// </summary>
        public static long InsertSmsNotification(string json)
        {
            return Insert(new Signalod()
            {
                IType = InvalidType.SmsTextMsgReceivedUnreadCount,
                FKeyType = KeyType.SmsMsgUnreadCount,
                MsgValue = json,
            });
        }

        /// <summary>
        /// Check for appointment signals for a single date.
        /// </summary>
        public static bool IsApptRefreshNeeded(DateTime dateTimeShowing, List<Signalod> signals) =>
            IsApptRefreshNeeded(dateTimeShowing, dateTimeShowing, signals);

        /// <summary>
        /// After a refresh, this is used to determine whether the Appt Module needs to be 
        /// refreshed. Returns true if there are any signals with InvalidType=Appointment 
        /// where the DateViewing time of the signal falls within the provided daterange, and the 
        /// signal matches either the list of visible operatories or visible providers in the 
        /// current Appt Module View. Always returns true if any signals have
        /// DateViewing=DateTime.MinVal.
        /// </summary>
        public static bool IsApptRefreshNeeded(DateTime startDate, DateTime endDate, List<Signalod> signals)
        {
            // A date range was refreshed. Easier to refresh all without checking.
            if (signals.Exists(
                signal => 
                    signal.DateViewing.Date == DateTime.MinValue.Date && 
                    signal.IType == InvalidType.Appointment))
                return true;

            var appointmentSignals = 
                signals.FindAll(
                    signal => 
                        signal.IType == InvalidType.Appointment && 
                        signal.DateViewing.Date >= startDate.Date && 
                        signal.DateViewing.Date <= endDate.Date);

            if (appointmentSignals.Count == 0)  return false;
            
            var visibleOperatoryIds = ApptDrawing.VisOps.Select(x => x.OperatoryNum).ToList();
            var visibleProviderIds = ApptDrawing.VisProvs.Select(x => x.ProvNum).ToList();

            if (appointmentSignals.Any(x => x.FKeyType == KeyType.Operatory && visibleOperatoryIds.Contains(x.FKey)) ||
                appointmentSignals.Any(x => x.FKeyType == KeyType.Provider && visibleProviderIds.Contains(x.FKey)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check for schedule signals for a single date.
        /// </summary>
        public static bool IsSchedRefreshNeeded(DateTime dateTimeShowing, List<Signalod> signals) =>
            IsSchedRefreshNeeded(dateTimeShowing, dateTimeShowing, signals);

        /// <summary>
        /// After a refresh, this is used to determine whether the Appt Module needs to be 
        /// refreshed. Returns true if there are any signals with InvalidType=Appointment where the 
        /// DateViewing time of the signal falls within the provided daterange, and the signal 
        /// matches either the list of visible operatories or visible providers in the current 
        /// Appt Module View.  Always returns true if any signals have 
        /// DateViewing=DateTime.MinVal.
        /// </summary>
        public static bool IsSchedRefreshNeeded(DateTime startDate, DateTime endDate, List<Signalod> signals)
        {
            // A date range was refreshed. Easier to refresh all without checking.
            if (signals.Exists(
                signal =>
                    signal.DateViewing.Date == DateTime.MinValue.Date &&
                    signal.IType == InvalidType.Schedules))
                return true;

            var scheduleSignals =
                signals.FindAll(
                    signal =>
                        signal.IType == InvalidType.Schedules &&
                        signal.DateViewing.Date >= startDate.Date &&
                        signal.DateViewing.Date <= endDate.Date);

            if (scheduleSignals.Count == 0) return false;

            var visibleOperatoryIds = ApptDrawing.VisOps.Select(x => x.OperatoryNum).ToList();
            var visibleProviderIds = ApptDrawing.VisProvs.Select(x => x.ProvNum).ToList();

            if (scheduleSignals.Any(x => x.FKeyType == KeyType.Operatory && visibleOperatoryIds.Contains(x.FKey)) ||
                scheduleSignals.Any(x => x.FKeyType == KeyType.Provider && visibleProviderIds.Contains(x.FKey)) ||
                scheduleSignals.Any(x => x.FKeyType == KeyType.Undefined))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// After a refresh, this is used to get a list containing all flags of types that need to
        /// be refreshed. The FKey must be 0 and the FKeyType must Undefined.Types of Task and 
        /// SmsTextMsgReceivedUnreadCount are not included.
        /// </summary>
        public static InvalidType[] GetInvalidTypes(List<Signalod> signals)
        {
            return signals
                .FindAll(
                    signal =>
                        signal.IType != InvalidType.Task &&
                        signal.IType != InvalidType.TaskPopup &&
                        signal.IType != InvalidType.SmsTextMsgReceivedUnreadCount &&
                        signal.FKey == 0 &&
                        signal.FKeyType == KeyType.Undefined)
                .Select(
                    signal => signal.IType)
                .ToArray();
        }

        /// <summary>
        /// Won't work with InvalidType.Date, InvalidType.Task, or InvalidType.TaskPopup  yet.
        /// </summary>
        public static void SetInvalid(params InvalidType[] invalidTypes)
        {
            foreach (var invalidType in invalidTypes)
            {
                Insert(new Signalod
                {
                    IType = invalidType,
                    DateViewing = DateTime.MinValue
                });
            }
        }

        /// <summary>
        /// Must be called after Preference cache has been filled.
        /// Deletes all signals older than 2 days if this has not been run within the last week.
        /// Will fail silently if anything goes wrong.
        /// </summary>
        public static void ClearOldSignals()
        {
            try
            {
                if (Preference.Exists(PreferenceName.SignalLastClearedDate) && 
                    Preference.GetDateTime(PreferenceName.SignalLastClearedDate) > MiscData.GetNowDateTime().AddDays(-7)) // Has already been run in the past week. This is all server based time.
                {
                    return;
                }

                // Itypes only older than 2 days
                DataConnection.ExecuteNonQuery("DELETE FROM signalod WHERE SigDateTime < DATE_ADD(NOW(),INTERVAL -2 DAY)");

                // Clear messaging buttons which use to be stored in the signal table.
                SigMessages.ClearOldSigMessages();

                // Set Last cleared to now.
                Preference.Update(PreferenceName.SignalLastClearedDate, MiscData.GetNowDateTime());
            }
            catch
            {
            }
        }
    }
}
