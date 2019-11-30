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
using System.Xml;

namespace OpenDental.Bridges
{
    public class EwooBridge : CommandLineBridge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EwooBridge"/> class.
        /// </summary>
        public EwooBridge() : base("Ewoo", "")
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

            var programPath = GetProgramPath(programId);

            var filePath = Path.Combine(Path.GetDirectoryName(programPath), "linkage.xml");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "   ",
                NewLineChars = "\r\n",
                OmitXmlDeclaration = true
            };

            using (var stream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
            {
                var address = patient.Address;
                if (patient.Address2 != "")
                {
                    address += ", " + patient.Address2;
                }
                address += ", " + patient.City + ", " + patient.State;

                xmlWriter.WriteStartElement("LinkageParameter");
                xmlWriter.WriteStartElement("Patient");
                xmlWriter.WriteAttributeString("LastName", patient.LName);
                xmlWriter.WriteAttributeString("FirstName", patient.FName);
                xmlWriter.WriteAttributeString("ChartNumber", GetPatientId(programId, patient));
                xmlWriter.WriteElementString("Birthday", patient.Birthdate.ToString("dd/MM/yyyy"));
                xmlWriter.WriteElementString("Address", address);
                xmlWriter.WriteElementString("ZipCode", patient.Zip);
                xmlWriter.WriteElementString("Phone", patient.HmPhone);
                xmlWriter.WriteElementString("Mobile", patient.WirelessPhone);
                xmlWriter.WriteElementString("SocialID", patient.SSN);
                xmlWriter.WriteElementString("Gender", patient.Gender == PatientGender.Female ? "Female" : "Male");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }

            return true;
        }
    }
}
