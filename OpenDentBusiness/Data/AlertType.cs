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
    public static class AlertType
    {
        /// <summary>
        /// Generic. Informational, has no action associated with it.
        /// </summary>
        public const string Generic = "default";

        /// <summary>
        /// Opens the Online Payments Window when clicked.
        /// </summary>
        public const string OnlinePaymentsPending = "online_payments-pending";

        /// <summary>
        /// Opens the Radiology Order List window when clicked.
        /// </summary>
        public const string RadiologyProcedures = "radiology_procedures";

        /// <summary>
        /// A patient has clicked "Request Callback" on an e-Confirmation.
        /// </summary>
        public const string CallbackRequested = "callback_requested";

        /// <summary>
        /// A number is not able to receive text messages.
        /// </summary>
        public const string NumberBarredFromTexting = "number_barred_from_texting";

        /// <summary>
        /// The number of MySQL connections to the server has exceeded half the allowed number of connections.
        /// </summary>
        public const string MaxConnectionsMonitor = "max_connections_monitor";

        /// <summary>
        /// Alerts related to DoseSpot provider registration.
        /// </summary>
        public const string DoseSpotProviderRegistered = "dose_spot_provider_registered";

        /// <summary>
        /// Alerts related to DoseSpot clinic registration.
        /// </summary>
        public const string DoseSpotClinicRegistered = "dose_spot_clinic_registered";
    }
}
