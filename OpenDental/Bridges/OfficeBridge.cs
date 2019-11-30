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
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class OfficeBridge : Bridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Custom("program_path", "Program Path", BridgePreferenceType.FilePath),
            BridgePreference.Custom("documents_path", "Document Folder", BridgePreferenceType.FolderPath),
            BridgePreference.Custom("file_ext", "File Extension", BridgePreferenceType.String)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="OfficeBridge"/> class.
        /// </summary>
        public OfficeBridge() : base("Office", "", "", preferences)
        {
        }

        /// <summary>
        ///     <para>
        ///         Sends the specified <paramref name="patient"/> data to the remote program or 
        ///         service.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient details.</param>
        public override void Send(long programId, Patient patient)
        {
            if (patient == null)
            {
                MessageBox.Show(
                    "Please select a patient first.",
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            var programPath = ProgramPreference.GetString(programId, "program_path");
            if (string.IsNullOrEmpty(programPath) || !File.Exists(programPath))
            {
                MessageBox.Show(
                    string.Format("'{0}' does not exist.", programPath),
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            try
            {
                var documentsPath = ProgramPreference.GetString(programId, "documents_path");

                var fileExt = ProgramPreference.GetString(programId, "file_ext");
                var fileName = Path.Combine(documentsPath, Tidy(GetPatientId(programId, patient)) + fileExt);

                Process.Start(programPath, fileName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Strips all non alphanumeric characters from the given string.
        /// </summary>
        private static string Tidy(string input)
        {
            var result = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsLetterOrDigit(input, i))
                {
                    result += input[i];
                }
            }

            return result;
        }
    }
}
