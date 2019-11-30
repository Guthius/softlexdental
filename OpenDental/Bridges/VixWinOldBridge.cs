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
    public class VixWinOldBridge : CommandLineBridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Custom("quick_link_path", "QuikLink directory", BridgePreferenceType.FolderPath)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="VixWinOldBridge"/> class.
        /// </summary>
        public VixWinOldBridge() : base("VixWin (Legacy)", "", "https://www.kavo.com/en-us/gendex", preferences)
        {
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
            string Tidy(string input) => input.Replace("\"", "");

            arguments = "";

            var quickLinkPath = ProgramPreference.GetString(programId, "quick_link_path");
            if (string.IsNullOrEmpty(quickLinkPath) || !Directory.Exists(quickLinkPath))
            {
                MessageBox.Show(
                    quickLinkPath + " is not a valid folder.",
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            var patientId = GetPatientId(programId, patient).PadLeft(6, '0');
            if (patientId.Length > 6)
            {
                MessageBox.Show(
                    "Patient ID is longer than six digits, so link failed.",
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            var fileName = Path.Combine(quickLinkPath, patientId + ".DDE");

            using (var stream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var streamWriter = new StreamWriter(stream))
            {
                streamWriter.WriteLine(
                    "\"" + Tidy(patient.FName) + "\"," + 
                    "\"" + Tidy(patient.LName) + "\"," +
                    "\"" + patientId + "\"");
            }

            return true;
        }
    }
}
