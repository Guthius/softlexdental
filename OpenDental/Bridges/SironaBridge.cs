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
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class SironaBridge : CommandLineBridge
    {
        // TODO: This implementation needs to tested, the previous implentation used some awkward string byte[] conversions...

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// Initializes a new instance of the <see cref="SironaBridge"/> class.
        /// </summary>
        public SironaBridge() : base(
            "Sirona Sidexis",
            "Sidexis is the software for clear diagnoses. It efficiently structures your workflow and serves as a basis for further planning and diagnosis.")
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
            arguments = "";

            var iniFilePath = Path.Combine(GetProgramPath(programId), "sifiledb.ini");
            if (!File.Exists(iniFilePath))
            {
                MessageBox.Show(
                     "'" + iniFilePath + "' could not be found. Is Sidexis installed properly?",
                     Name,
                     MessageBoxButtons.OK, 
                     MessageBoxIcon.Error);

                return false;
            }

            // read FromStation0 | File to determine location of comm file (sendBox) (siomin.sdx)
            // example:
            // [FromStation0]
            // File=F:\PDATA\siomin.sdx  //only one sendBox on entire network.
            var sendBoxFile = new StringBuilder(255);
            GetPrivateProfileString("FromStation0", "File", "", sendBoxFile, 255, iniFilePath);

            // read Multistations | GetRequest (=1) to determine if station can take xrays.
            // but we don't care at this point, so ignore
            // set OfficeManagement | OffManConnected = 1 to make sidexis ready to accept a message.
            WritePrivateProfileString("OfficeManagement", "OffManConnected", "1", iniFilePath);
            WritePatientToSendBox(programId, patient, sendBoxFile.ToString());

            return true;
        }

        /// <summary>
        /// Writes the details of the specified <paramref name="patient"/> to the specified sendbox.
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient.</param>
        /// <param name="sendBoxFile">The full path of the sendbox file.</param>
        private static void WritePatientToSendBox(long programId, Patient patient, string sendBoxFile)
        {
            var line = new StringBuilder(4096);
            char terminator = '\0';

            using (var stream = File.Open(sendBoxFile, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                // Line Format: first two bytes are the length of line including first two 
                //              bytes and each field is terminated by null (byte 0).

                // Append U: U signifies Update patient in sidexis. Gets ignored if new patient.
                line.Append("U");
                line.Append(terminator);
                line.Append(patient.LName);
                line.Append(terminator);
                line.Append(patient.FName);
                line.Append(terminator);
                line.Append(patient.Birthdate.ToString("dd.MM.yyyy"));
                line.Append(terminator);
                // Leave initial patient id blank. This updates Sidexis to patient ID's used in Open Dental
                line.Append(terminator);
                line.Append(patient.LName);
                line.Append(terminator);
                line.Append(patient.FName);
                line.Append(terminator);
                line.Append(patient.Birthdate.ToString("dd.MM.yyyy"));
                line.Append(terminator);
                line.Append(GetPatientId(programId, patient));
                line.Append(terminator);
                line.Append(patient.Gender == PatientGender.Female ? "F" : "M");
                line.Append(terminator);
                line.Append(Providers.GetAbbr(Patients.GetProvNum(patient)));
                line.Append(terminator);
                line.Append("OpenDental");
                line.Append(terminator);
                line.Append("Sidexis");
                line.Append(terminator);
                line.Append("\r\n");
                WriteLine(stream, line.ToString());

                // Append N: N signifies New patient in sidexis. If patient already exists, then it simply updates any old data.
                line.Clear();
                line.Append("N");
                line.Append(terminator);
                line.Append(patient.LName);
                line.Append(terminator);
                line.Append(patient.FName);
                line.Append(terminator);
                line.Append(patient.Birthdate.ToString("dd.MM.yyyy"));
                line.Append(terminator);
                line.Append(GetPatientId(programId, patient));
                line.Append(terminator);
                line.Append(patient.Gender == PatientGender.Female ? "F" : "M");
                line.Append(terminator);
                line.Append(Providers.GetAbbr(Patients.GetProvNum(patient)));
                line.Append(terminator);
                line.Append("OpenDental");
                line.Append(terminator);
                line.Append("Sidexis");
                line.Append(terminator);
                line.Append("\r\n");
                WriteLine(stream, line.ToString());

                // Append A: A signifies Autoselect patient. 
                line.Clear();
                line.Append("A");
                line.Append(terminator);
                line.Append(patient.LName);
                line.Append(terminator);
                line.Append(patient.FName);
                line.Append(terminator);
                line.Append(patient.Birthdate.ToString("dd.MM.yyyy"));
                line.Append(terminator);
                line.Append(GetPatientId(programId, patient));
                line.Append(terminator);
                line.Append(SystemInformation.ComputerName);
                line.Append(terminator);
                line.Append(DateTime.Now.ToString("dd.MM.yyyy"));
                line.Append(terminator);
                line.Append(DateTime.Now.ToString("HH.mm.ss"));
                line.Append(terminator);
                line.Append("OpenDental");
                line.Append(terminator);
                line.Append("Sidexis");
                line.Append(terminator);
                line.Append("0"); //0 = No Image Selection
                line.Append(terminator);
                line.Append("\r\n");
                WriteLine(stream, line.ToString());
            }
        }

        /// <summary>
        /// Writes the specified <paramref name="line"/> to the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="line">The line to write.</param>
        private static void WriteLine(Stream stream,  string line)
        {
            var lineData = Encoding.ASCII.GetBytes(line);
            int lineLength = lineData.Length + 2;

            stream.WriteByte((byte)((lineLength & 0xFF00) >> 8));
            stream.WriteByte((byte)(lineLength & 0xFF));
            stream.Write(lineData, 0, lineData.Length);
        }
    }
}
