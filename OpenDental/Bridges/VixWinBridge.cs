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
    public class VixWinBridge : CommandLineBridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Custom("image_path", "Image Path", BridgePreferenceType.FolderPath)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="VixWinBridge"/> class.
        /// </summary>
        public VixWinBridge() : base("VixWin", "", "https://www.kavo.com/en-us/gendex", preferences)
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
            string Tidy(string input) => input.Replace(" ", "");

            arguments = "";

            var imagePath = ProgramPreference.GetString(programId, "image_path");
            if (string.IsNullOrEmpty(imagePath))
            {
                MessageBox.Show(
                    "Missing Image Path.",
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            if (!Directory.Exists(imagePath))
            {
                try
                {
                    Directory.CreateDirectory(imagePath);
                }
                catch
                {
                    MessageBox.Show(
                        "Patient image path could not be created. This usually indicates a permission issue. Path:\r\n" + imagePath,
                        Name,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return false;
                }
            }

            arguments =
                " -I " + GetPatientId(programId, patient) +
                " -N " + Tidy(patient.FName) + "^" + Tidy(patient.LName) +
                " -P " + imagePath;

            return true;
        }
    }
}
