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
using System.Text.RegularExpressions;

namespace OpenDental.Bridges
{
    public class TscanBridge : CommandLineBridge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TscanBridge"/> class.
        /// </summary>
        public TscanBridge() : base("Tscan", "")
        {
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
            string Tidy(string input) => Regex.Replace(input, "[^a-zA-Z0-9]", "");

            arguments = "-f" + Tidy(patient.FName) + " ";

            // Only send middle name if available, since it is optional.
            if (Tidy(patient.MiddleI) != "")
            {
                arguments += "-m" + Tidy(patient.MiddleI) + " ";
            }

            arguments +=
                "-l" + Tidy(patient.LName) + " " + 
                "-i" + Tidy(GetPatientId(programId, patient)) + " ";

            // Birthdate is optional, so we only send if valid.
            if (patient.Birthdate.Year > 1880)
            {
                arguments +=
                    "-d" + patient.Birthdate.Day.ToString().PadLeft(2, '0') + " " +
                    "-j" + patient.Birthdate.Month.ToString().PadLeft(2, '0') + " " +
                    "-y" + patient.Birthdate.Year.ToString() + " ";
            }

            // Gender is optional, so we only send if not unknown.
            if (patient.Gender != PatientGender.Unknown)
            {
                arguments += "-g" + (patient.Gender == PatientGender.Female ? "1" : "2") + " ";
            }

            return true;
        }
    }
}
