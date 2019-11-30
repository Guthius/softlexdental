/**
 * Copyright (C) 2019 Dental Stars SRL
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
using System.Collections.Generic;
using System.Linq;

namespace OpenDentBusiness.Bridges
{
    /// <summary>
    ///     <para>
    ///         Bridge to transfer patient information to a external program or service.
    ///     </para>
    /// </summary>
    public abstract class Bridge : IBridge
    {
        private readonly List<BridgePreference> preferences;

        /// <summary>
        /// Gets the name of the external program or service.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a short description of the functionality provided by the bridge.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the URL of the primary website of the external program.
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bridge"/> class.
        /// </summary>
        /// <param name="name">The name of the external progarm or service.</param>
        /// <param name="description">A description of the bridge.</param>
        /// <param name="url">The URL of the external program.</param>
        /// <param name="preferences">The preferences used by the bridge.</param>
        public Bridge(string name, string description = "", string url = "", params BridgePreference[] preferences)
        {
            Name = name ??
                throw new ArgumentNullException(nameof(name));

            
            Description = description ?? "";
            Url = url ?? "";

            this.preferences = preferences?.ToList() ?? new List<BridgePreference>();
            this.preferences.Add(BridgePreference.UseChartNumber);
        }

        /// <summary>
        ///     <para>
        ///         Sends the specified <paramref name="patient"/> data to the remote program or 
        ///         service.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient details.</param>
        public abstract void Send(long programId, Patient patient);

        /// <summary>
        /// Gets the prferences that can be configured for the bridge.
        /// </summary>
        public IEnumerable<BridgePreference> Preferences => preferences;

        /// <summary>
        ///     <para>
        ///         Returns the ID to use to use when transfering data of the specified 
        ///         <paramref name="patient"/> to the external program or service.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient ID.</param>
        /// <returns>The ID of the patient.</returns>
        /// <exception cref="Exception">
        ///     If the <see cref="BridgePreference.UseChartNumber"/> preference has been set to 
        ///     true but the chart number of patient has not been set.
        /// </exception>
        protected static string GetPatientId(long programId, Patient patient)
        {
            if (IsUsingChartNumber(programId))
            {
                if (string.IsNullOrEmpty(patient.ChartNumber))
                {
                    throw new Exception("This patient does not have a chart number.");
                }

                return patient.ChartNumber;
            }

            return patient.PatNum.ToString();
        }

        /// <summary>
        ///     <para>
        ///         Gets a value indicating whether the bridge is using 
        ///         <see cref="Patient.ChartNumber"/> to uniquely identify patients. If false, 
        ///         <see cref="Patient.PatNum"/> is used as identification instead.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <returns>
        ///     True if <see cref="Patient.ChartNumber"/> should be used to identify the patient; 
        ///     otherwise, false.
        /// </returns>
        protected static bool IsUsingChartNumber(long programId) => ProgramPreference.GetBool(programId, ProgramPreferenceName.UseChartNumber);
    }
}
