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
    public class DentalStudioBridge : CommandLineBridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Define("username", "UserName (clear to use OD username)", BridgePreferenceType.String),
            BridgePreference.Define("password", "UserPassword", BridgePreferenceType.String)
        };

        /// <summary>
        /// Initializes a new instance fo the <see cref="DentalStudioBridge"/> class.
        /// </summary>
        public DentalStudioBridge() : base("Dental Studio", "", "https://www.villasm.com/", preferences)
        {
            RequirePatient = true;
        }

        /// <summary>
        ///     <para>
        ///         Prepares the local system enviroment for running the program. Most bridges
        ///         will only fill the <paramref name="arguments"/> parameter with the appropriate 
        ///         data. Some bridges will perform more complex actions such as generating files,
        ///         contacting remote services, etc...
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
            string Tidy(string input) => '"' + input.Replace("\"", "") + '"';

            // The parameters must be in a specific order.
            // Param1: UserName
            // Param2: UserPassword
            // Param3: PatientLName
            // Param4: PatientFName
            // Param5: PatientSex
            // Param6: PatientID
            // Param7: Action            -- For future use.  Must always be O for now (not zero).  The O stands for "Open".
            // Param8: PatientBDate      -- DentalStudio patient matching does not depend on birthdate, so we cand send 01/01/0001. DentalStudio will update the birthdate after updated in OD.

            var username = ProgramPreference.GetString(programId, "username");
            if (string.IsNullOrEmpty(username))
            {
                username = Security.CurrentUser.UserName;
            }

            var password = ProgramPreference.GetString(programId, "password");
            if (string.IsNullOrEmpty(username))
            {
                // TODO: Dental Studio might need to be contacted or our bridge might need to be enhanced for sending / updating password hashes.
                password = Security.CurrentUser.PasswordHash;
            }

            arguments =
                Tidy(username) + " " +
                Tidy(password) + " " +
                Tidy(patient.LName) + " " +
                Tidy(patient.FName) + " ";

            arguments +=
                (patient.Gender == PatientGender.Female ?
                    Tidy("F") :
                    (patient.Gender == PatientGender.Male ?
                        Tidy("M") : Tidy("O"))) + " ";

            arguments +=
                Tidy(GetPatientId(programId, patient)) + " " +
                Tidy("O") + " " +
                Tidy(patient.Birthdate.ToString("MM/dd/yyyy"));

            return true;
        }
    }
}
