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
    public class VixWinNumberedBridge : CommandLineBridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Custom("image_path", "Image Path", BridgePreferenceType.FolderPath)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="VixWinNumberedBridge"/> class.
        /// </summary>
        public VixWinNumberedBridge() : this("VixWin (Numbered)", "")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VixWinNumberedBridge"/> class.
        /// </summary>
        /// <param name="name">The name of the bridge.</param>
        /// <param name="description">A description of the bridge.</param>
        internal VixWinNumberedBridge(string name, string description) : base (name, description, "https://www.kavo.com/en-us/gendex", preferences)
        {
            RequirePatient = true;
        }

        /// <summary>
        /// Gets the image storage location for the specified <paramref name="patient"/>.
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="baseImagePath">The base path for images.</param>
        /// <param name="patient">The patient.</param>
        /// <returns>The image location for the specified patient.</returns>
        protected virtual string GetPatientImagePath(long programId, string baseImagePath, Patient patient)
        {
            var imageGroup = (patient.PatNum % 100).ToString().PadLeft(2, '0');

            return Path.Combine(baseImagePath, imageGroup, patient.PatNum.ToString());
        }

        /// <summary>
        /// Gets a identifier for the specified patient.
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient.</param>
        /// <returns>The ID of the patient.</returns>
        protected virtual string GetIdentifier(long programId, Patient patient) => GetPatientId(programId, patient);

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
        protected sealed override bool PrepareToRun(long programId, Patient patient, out string arguments)
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

            imagePath = GetPatientImagePath(programId, imagePath, patient);
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
                " -I " + GetIdentifier(programId, patient) +
                " -N " + Tidy(patient.FName) + "^" + Tidy(patient.LName) +
                " -P " + imagePath;

            return true;
        }
    }
}
