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
using OpenDentBusiness;
using OpenDentBusiness.Bridges;

namespace OpenDental.Bridges
{
    public class PlanmecaBridge : CommandLineBridge
    {
        // TODO: This is the same as RomexisBridge?

        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Custom("date_format", "Date Format", BridgePreferenceType.String)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanmecaBridge"/> class.
        /// </summary>
        public PlanmecaBridge() : base("Dimaxis", "", "https://www.planmeca.com/software/", preferences)
        {
            RequirePatient = true;
        }

        /// <summary>
        ///     <para>
        ///         Generates the appropriate command line arguments for the specified patient.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient.</param>
        /// <param name="arguments">The command line arguments to pass to the program.</param>
        /// <returns>
        ///     True if the preparation was successful and the program can be started; otherwise, false.
        /// </returns>
        protected override bool PrepareToRun(long programId, Patient patient, out string arguments)
        {
            string Tidy(string input) => input.Replace("\"", "");

            var dateFormat = ProgramPreference.GetString(programId, "date_format", "dd/MM/yyyy");

            arguments =
                "\"" + Tidy(GetPatientId(programId, patient)) + "\" " +
                "\"" + Tidy(patient.LName) + "\" " +
                "\"" + Tidy(patient.FName) + "\" " +
                "\"" + patient.Birthdate.ToString(dateFormat) + "\"";

            return true;
        }
    }
}
