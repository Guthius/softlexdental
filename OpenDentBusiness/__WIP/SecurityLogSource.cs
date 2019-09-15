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
using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// Identifies the source of a security log entry.
    /// </summary>
    public static class SecurityLogSource
    {
        /// <summary>
        /// Open Dental and unknown entities.
        /// </summary>
        public const string None = "System";

        /// <summary>
        /// OpenDentalService.
        /// </summary>
        public const string OpenDentalService = "Service";

        /// <summary>
        /// X12 834 Insurance Plan Import from the Manage Module.
        /// </summary>
        public const string InsPlanImport834 = "X12 834 Insurance Plan";

        /// <summary>
        /// HL7 is an automated process which the user may not be aware of.
        /// </summary>
        public const string HL7 = "HL7";

        /// <summary>
        /// Database maintenance. This process creates patients which are known to be missing, but 
        /// the user may not be aware that the fix involves patient recreation.
        /// </summary>
        public const string DBM = "DBM";

        /// <summary>
        /// FHIR is an automated process which the user may not be aware of.
        /// </summary>
        public const string FHIR = "FHIR";

        /// <summary>
        /// Patient Portal application.
        /// </summary>
        public const string PatientPortal = "Patient Portal";

        /// <summary>
        /// GWT Web Sched application Recall version.
        /// </summary>
        [Obsolete]
        public const string WebScheduler = "Web Scheduler";

        /// <summary>
        /// GWT Web Sched application New Patient Appointment version
        /// </summary>
        [Obsolete]
        public const string WebSchedulerNewPatient = "Web Scheduler (New Patient)";

        /// <summary>
        /// Web Sched application for moving ASAP appointments.
        /// </summary>
        [Obsolete]
        public const string WebSchedulerASAP = "Web Scheduler (ASAP)";

        /// <summary>
        /// Automated eConfirmation and eReminders
        /// </summary>
        [Obsolete]
        public const string AutoConfirmations = "AutoConfirmations";

        /// <summary>
        /// Open Dental messages created for debugging and diagnostic purposes. For example, to 
        /// diagnose an unhandled exception or unexpected behavior that is otherwise too hard to 
        /// diagnose.
        /// </summary>
        public const string Diagnostic = "Diagnostics";

        /// <summary>
        /// Mobile Web application.
        /// </summary>
        [Obsolete]
        public const string MobileWeb = "Mobile Web";

        /// <summary>
        /// When retrieving reports in the background of FormOpenDental
        /// </summary>
        [Obsolete]
        public const string CanadaEobAutoImport = "Canada EOB Auto Import";

        /// <summary>
        /// Broadcast Monitor.
        /// </summary>
        public const string BroadcastMonitor = "Broadcast Monitor";

        /// <summary>
        /// Automatic log off from main form. Used to track when auto log off needs to kill the 
        /// program to force close open forms which are blocked or slow to respond.
        /// </summary>
        public const string AutoLogOff = "Log Off";
    }
}
