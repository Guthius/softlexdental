﻿using CodeBase;

namespace OpenDentBusiness
{
    public class AppointmentEvent
    {
        public static event ODEventHandler Fired;

        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class ASAPEvent
    {
        public static event ODEventHandler Fired;

        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class BillingEvent : IODEvent
    {
        public static event ODEventHandler Fired;

        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }

        public void FireEvent(ODEventArgs e) { Fired?.Invoke(e); }
    }

    public class ClearinghouseEvent : IODEvent
    {
        public static event ODEventHandler Fired;
        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
        public void FireEvent(ODEventArgs e) { Fired?.Invoke(e); }
    }

    public class ClinicEvent
    {
        public static event ODEventHandler Fired;

        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class CommItemSaveEvent
    {
        public static event ODEventHandler Fired;
        public static void Fire(ODEventType odEventType, object tag = null) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class ConfirmationListEvent
    {
        public static event ODEventHandler Fired;
        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class CredentialsFailedAfterLoginEvent
    {
        public static event ODEventHandler Fired;
    }

    public class EmailSaveEvent
    {
        public static event ODEventHandler Fired;
        public static void Fire(ODEventType odEventType, object tag = null) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class EtransEvent
    {
        public static event ODEventHandler Fired;
        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class FeeSchedEvent : IODEvent
    {
        public static event ODEventHandler Fired;
        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
        public void FireEvent(ODEventArgs e) { Fired?.Invoke(e); }
    }

    public class GeneralProgramEvent
    {
        public static event ODEventHandler Fired;

        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class InsuranceVerificationEvent
    {
        public static event ODEventHandler Fired;

        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class PatientChangedEvent
    {
        public static event ODEventHandler Fired;

        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class PatientEvent
    {
        public static event ODEventHandler Fired;
        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class PlannedTrackerEvent
    {
        public static event ODEventHandler Fired;
        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class RecallListEvent
    {
        public static event ODEventHandler Fired;
        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class RecallSyncEvent
    {
        public static event ODEventHandler Fired;
        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class ReportComplexEvent
    {
        public static event ODEventHandler Fired;
        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class ScheduleEvent
    {
        public static event ODEventHandler Fired;
        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class SendToPinboardEvent
    {
        public static event ODEventHandler Fired;
        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class SplashProgressEvent
    {
        public static event ODEventHandler Fired;
        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class UnscheduleListEvent
    {
        public static event ODEventHandler Fired;
        public static void Fire(ODEventType odEventType, object tag) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class WikiSaveEvent
    {
        public static event ODEventHandler Fired;
        public static void Fire(ODEventType odEventType, object tag = null) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }

    public class ODGridEvent
    {
        public static event ODEventHandler Fired;
        public static void Fire(ODEventType odEventType, object tag = null) { Fired?.Invoke(new ODEventArgs(odEventType, tag)); }
    }
}
