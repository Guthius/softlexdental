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
using System.IO;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class CarestreamBridge : CommandLineBridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Define("ini_file_path", "Patient.ini path", BridgePreferenceType.File)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="CarestreamBridge"/> class.
        /// </summary>
        public CarestreamBridge() : base("Carestream Ortho/OMS", "", "https://www.carestreamdental.com/", preferences)
        {
            RequirePatient = true;
        }

        /// <summary>
        ///     <para>
        ///         Generates a file with the patient details.
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
            arguments = "";

            var filePath = ProgramPreference.GetString(programId, "ini_file_path");
            if (filePath.Length > 150)
            {
                MessageBox.Show(
                    "Patient.ini file folder path too long. Must be 150 characters or less.",
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            using (var stream = File.Open(filePath, FileMode.Create, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine("[PATIENT]");
                writer.WriteLine("ID=" + Tidy(GetPatientId(programId, patient), 15));
                writer.WriteLine("FIRSTNAME=" + Tidy(patient.FName, 255));

                if (!string.IsNullOrEmpty(patient.Preferred))
                {
                    writer.WriteLine("COMMONNAME=" + Tidy(patient.Preferred, 255));
                }

                writer.WriteLine("LASTNAME=" + Tidy(patient.LName, 255));
                if (!string.IsNullOrEmpty(patient.MiddleI))
                {
                    writer.WriteLine("MIDDLENAME=" + Tidy(patient.MiddleI, 255));
                }

                if (patient.Birthdate.Year > 1880)
                {
                    writer.WriteLine("DOB=" + patient.Birthdate.ToString("yyyyMMdd"));
                }

                writer.Write(patient.Gender == PatientGender.Female ? "GENDER=F" : "GENDER=M");
                writer.Flush();
            }

            arguments = @"-I """ + filePath + @"""";

            return true;
        }

        /// <summary>
        /// Removes semicolons and spaces from the specified string. Optionally truncates the 
        /// string if it exceeds the specified maximum length.
        /// </summary>
        /// <param name="input">The string to clean.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>The updated string.</returns>
        private static string Tidy(string input, int maxLength)
        {
            var result = input.Replace(";", "").Replace(" ", "");
            if (maxLength > 0 && result.Length > maxLength)
            {
                result = result.Substring(0, maxLength);
            }

            return result;
        }
    }
}
