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
    /// Identifies the type of a alert. This determines the action to take when the user clicks the alerts.
    /// </summary>
    public enum AlertType
    {
        /// <summary>
        /// Generic. Informational, has no action associated with it.
        /// </summary>
        Generic,

        /// <summary>
        /// Opens the Online Payments Window when clicked.
        /// </summary>
        OnlinePaymentsPending,

        /// <summary>
        /// Only used by Open Dental HQ. The server monitoring incoming voicemails is not working.
        /// </summary>
        [Obsolete] VoiceMailMonitor,

        /// <summary>
        /// Opens the Radiology Order List window when clicked.
        /// </summary>
        RadiologyProcedures,

        /// <summary>
        /// A patient has clicked "Request Callback" on an e-Confirmation.
        /// </summary>
        CallbackRequested,

        /// <summary>
        /// Alerts related to the Web Sched New Pat eService.
        /// </summary>
        WebSchedNewPat,

        /// <summary>
        /// Alerts related to Web Sched New Patient Appointments.
        /// </summary>
        WebSchedNewPatApptCreated,

        /// <summary>
        /// A number is not able to receive text messages.
        /// </summary>
        NumberBarredFromTexting,

        /// <summary>
        /// The number of MySQL connections to the server has exceeded half the allowed number of connections.
        /// </summary>
        MaxConnectionsMonitor,

        /// <summary>
        /// Alerts related to new ASAP appointments via web sched.
        /// </summary>
        WebSchedASAPApptCreated,

        /// <summary>
        /// Only used by Open Dental HQ. The Asterisk Server is not processing messages or is getting all blank payloads.
        /// </summary>
        [Obsolete] AsteriskServerMonitor,

        /// <summary>
        /// Multiple computers are running eConnector services. There should only ever be one.
        /// </summary>
        MultipleEConnectors,
        
        /// <summary>
        /// The eConnector is in a critical state and not currently turned on. There should only ever be one.
        /// </summary>
        EConnectorDown,
        
        /// <summary>
        /// The eConnector has an error that is not critical but is worth looking into. There should only ever be one.
        /// </summary>
        EConnectorError,
        
        /// <summary>
        /// Alerts related to DoseSpot provider registration.
        /// </summary>
        DoseSpotProviderRegistered,
        
        /// <summary>
        /// Alerts related to DoseSpot clinic registration.
        /// </summary>
        DoseSpotClinicRegistered,
        
        ///<summary>
        ///An appointment has been created via Web Sched Recall.
        ///</summary>
        WebSchedRecallApptCreated,
        
        /// <summary>
        /// Alerts related to turning clinics on or off for eServices.
        /// </summary>
        ClinicsChanged,
        
        /// <summary>
        /// Alerts related to turning clinics on or off for eServices. Internal, not displayed to the customer.
        /// Will be processed by the eConnector and then deleted.
        /// </summary>
        ClinicsChangedInternal,

        /// <summary>
        /// Multiple computers are running OpenDentalServices. There should only ever be one.
        /// </summary>
        MultipleOpenDentalServices,

        /// <summary>
        /// OpenDentalService is down.
        /// </summary>
        OpenDentalServiceDown,

        /// <summary>
        /// Triggered when a new WebMail is recieved from the patient portal.
        /// </summary>
        WebMailRecieved,
    }
}
