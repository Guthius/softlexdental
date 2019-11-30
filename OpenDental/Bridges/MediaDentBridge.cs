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
    public class MediaDentBridge : CommandLineBridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Custom("mediadent_version", "MediaDent Version (4 or 5)", BridgePreferenceType.Long),
            BridgePreference.Custom("image_folder", "Image Folder", BridgePreferenceType.FolderPath),
            BridgePreference.Custom("file_path", "Text File Path", BridgePreferenceType.FilePath)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaDentBridge"/> class.
        /// </summary>
        public MediaDentBridge() : base("MediaDent Digital Imaging", "", "http://www.mediadentusa.com/", preferences)
        {
        }

        /// <summary>
        ///     <para>
        ///         Generates a file with the patient details and generates the appropriate command
        ///         line arguments for the specified <paramref name="patient"/>.
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

            var version = ProgramPreference.GetLong(programId, "mediadent_version", 4);
            if (version != 4 && version != 5)
            {
                MessageBox.Show(
                    "The configured MediaDent version is not valid. Version must be 4 or 5.", 
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            switch (version)
            {
                case 4:
                    return PrepareForVersion4(programId, patient, out arguments);

                case 5:
                    return PrepareForVersion5(programId, patient, out arguments);
            }

            return false;
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
        private bool PrepareForVersion4(long programId, Patient patient, out string arguments)
        {
            string Tidy(string input) => input.Replace("\"", "").Replace("'", "").Replace("/", "");

            var provider = Providers.GetProv(Patients.GetProvNum(patient));

            var imageFolder = ProgramPreference.GetString(programId, "image_folder");
            if (string.IsNullOrEmpty(imageFolder))
            {
                imageFolder = Path.GetTempPath();
            }

            arguments = 
                " /P" + Tidy(patient.FName + " " + patient.LName) + 
                " /D" + provider.FName + " " + provider.LName + " /L1" +
                " /F" + Path.Combine(imageFolder,  Tidy(GetPatientId(programId, patient))) + "" +
                " /B" + patient.Birthdate.ToString("ddMMyyyy");

            return true;
        }

        /// <summary>
        ///     <para>
        ///         Generates a file with the patient details for MediaDent v5.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient.</param>
        /// <param name="arguments">The command line arguments to pass to the program.</param>
        /// <returns>
        ///     True if the preparation was successful and the program can be started; otherwise, false.
        /// </returns>
        private bool PrepareForVersion5(long programId, Patient patient, out string arguments)
        {
            arguments = "";

            var infoFilePath = ProgramPreference.GetString(programId, "file_path");
            if (string.IsNullOrEmpty(infoFilePath))
            {
                infoFilePath = Path.Combine(Path.GetTempPath(), "mediadent_info_file.txt");
            }

            using (var stream = File.Open(infoFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var streamWriter = new StreamWriter(stream))
            {
                string id = GetPatientId(programId, patient);

                streamWriter.WriteLine(patient.LName + ", " + patient.FName + " " + patient.Birthdate.ToShortDateString() + " " + id);
                streamWriter.WriteLine();
                streamWriter.WriteLine("PN=" + id);
                streamWriter.WriteLine("LN=" + patient.LName);
                streamWriter.WriteLine("FN=" + patient.FName);
                streamWriter.WriteLine("BD=" + patient.Birthdate.ToString("MM/dd/yyyy"));
                streamWriter.WriteLine("SX=" + (patient.Gender == PatientGender.Female ? "F" : "M"));
            }

            arguments = "@" + infoFilePath;

            return true;
        }
    }
}
