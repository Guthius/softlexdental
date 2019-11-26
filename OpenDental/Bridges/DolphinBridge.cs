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
    public class DolphinBridge : Bridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Custom("program_folder", "Dolphin Program Folder", BridgePreferenceType.FolderPath),
            BridgePreference.Custom("file_path", "Info File Path", BridgePreferenceType.FilePath)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="DolphinBridge"/> class.
        /// </summary>
        public DolphinBridge() : base("Dolphin", "", preferences)
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
            var programFolder = ProgramPreference.GetString(programId, "program_folder");
            if (string.IsNullOrEmpty(programFolder))
            {
                MessageBox.Show(
                    "The Dolphin program folder has not been configured.",
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            var dolphinDbPath = Path.Combine(programFolder, "dolDb.exe");
            var dolphinCtrlPath = Path.Combine(programFolder, "dolCtrl.exe");

            if (patient == null)
            {
                try
                {
                    Process.Start(dolphinCtrlPath);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        exception.Message, 
                        Name, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }

                return;
            }

            try
            {
                var infoFilePath = CreatePatientInfoFile(programId, patient);
                if (!string.IsNullOrEmpty(infoFilePath))
                {
                    var patientId = GetPatientId(programId, patient);
                    var process = Process.Start(dolphinDbPath, "Find -i" + patientId);

                    process.WaitForExit();
                    switch (process.ExitCode)
                    {
                        // Patient Not Found
                        case 0:
                            Process.Start(dolphinDbPath, "AddPatient -f\"" + infoFilePath + "\" -i" + patientId);
                            break;

                        // Patient Exists
                        case 135:
                            Process.Start(dolphinDbPath, "UpdatePatient -f\"" + infoFilePath + "\" -i" + patientId);
                            break;

                        default:
                            MessageBox.Show(
                                "Error synchronizing patient information with Dolphin. Unexpected exit code, Dolphin DB returned " + process.ExitCode + ".",
                                Name,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                            return;
                    }

                    Process.Start(dolphinCtrlPath, patientId);
                }
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
        ///     <para>
        ///         Writes a INI file that contains the details of the specified 
        ///         <paramref name="patient"/>.
        ///     </para>
        /// </summary>
        /// <param name="programId">The program ID.</param>
        /// <param name="patient">The patient.</param>
        private static string CreatePatientInfoFile(long programId, Patient patient)
        {
            string Tidy(string input, int maxLength) => input.Length > maxLength ? input.Substring(0, maxLength) : input;

            var infoFilePath = ProgramPreference.GetString(programId, "file_path");
            if (string.IsNullOrEmpty(infoFilePath))
            {
                infoFilePath = Path.Combine(Path.GetTempPath(), "dolphin_patient_info.ini");
            }

            using (var stream = File.Open(infoFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var streamWriter = new StreamWriter(stream))
            {
                streamWriter.WriteLine("[Patient Info]");
                streamWriter.WriteLine("PatientID=" + Tidy(GetPatientId(programId, patient), 10));
                streamWriter.WriteLine("LastName=" + Tidy(patient.LName, 50));
                streamWriter.WriteLine("FirstName=" + Tidy(patient.FName, 50));

                if (patient.Birthdate.Year > 1880)
                {
                    streamWriter.WriteLine("Birthdate=" + patient.Birthdate.ToString("MM-dd-yyyy"));
                }

                streamWriter.WriteLine("Gender=" + (patient.Gender == PatientGender.Female ? "1" : "0"));
                streamWriter.WriteLine("NickName=" + Tidy(patient.Preferred, 30));
                streamWriter.WriteLine("Title=" + Tidy(patient.Title, 10));
                streamWriter.WriteLine("Email=" + Tidy(patient.Email, 60));
            }

            return infoFilePath;
        }
    }
}
