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
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace OpenDental.Bridges
{
    public class AudaxCephBridge : CommandLineBridge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AudaxCephBridge"/> class.
        /// </summary>
        public AudaxCephBridge() : base("AudaxCeph", "AudaxCeph is an integrated orthodontic software suite.", "https://www.audaxceph.com/")
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

            var processes = Process.GetProcessesByName("AxCeph");
            if (processes.Length == 0)
            {
                MessageBox.Show(
                    "AxCeph.exe not found. Please make sure AudaxCeph is running and try again.",
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            var writerSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "   ",
                NewLineChars = "\r\n",
                OmitXmlDeclaration = true
            };

            var programPath = ProgramPreference.GetString(programId, ProgramPreferenceName.ProgramPath);
            var programXmlPath = Path.Combine(Path.GetDirectoryName(programPath), "update.xml");

            using (var stream = File.Open(programXmlPath, FileMode.Create, FileAccess.Write))
            using (var writer = XmlWriter.Create(stream, writerSettings))
            {
                writer.WriteProcessingInstruction("xml", "version='1.0' encoding='utf-8'");
                writer.WriteStartElement("AxCephComData");
                writer.WriteStartElement("Patients");
                writer.WriteStartElement("Patient");
                writer.WriteElementString("PIDOutside", GetPatientId(programId, patient));
                writer.WriteElementString("NameOfPatient", Tidy(patient.LName) + ", " + Tidy(patient.FName));
                writer.WriteElementString("DateOfBirth", patient.Birthdate.ToString("yyyyMMdd"));
                writer.WriteElementString("Sex", patient.Gender == PatientGender.Female ? "F" : "M");
                writer.WriteElementString("Active", "1");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteElementString("Command", "UpdateOrInsertPatient");
                writer.WriteElementString("ResultXMLFileName", "result.xml");
                writer.WriteElementString("ResultStatus", "");
                writer.WriteElementString("ResultMessage", "0");
                writer.WriteEndElement();
                writer.Flush();
            }

            return true;
        }

        /// <summary>
        /// Get rid of any character that isn't A-Z, a hyphen or a period.
        /// </summary>
        private static string Tidy(string input)
        {
            string result = string.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '-' || input[i] == '.' || char.IsLetter(input[i]))
                {
                    result += char.ToUpper(input[i]);
                }
            }
            return result;
        }
    }
}
