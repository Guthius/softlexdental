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
using System.Windows.Forms;
using System.Xml;

namespace OpenDental.Bridges
{
    public class ICatBridge : CommandLineBridge
    {
        // TODO: This bridge needs some more work... The FileWatcher is never started...

        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Custom("acq_computer_name", "Acquisition Computer Name", BridgePreferenceType.String),
            BridgePreference.Custom("xml_output_file_path", "XML Output File Path", BridgePreferenceType.FilePath),
            BridgePreference.Custom("return_folder_path", "Return Folder Path", BridgePreferenceType.FolderPath)
        };

        private static Program program;
        private static FileSystemWatcher fileSystemWatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="ICatBridge"/> class.
        /// </summary>
        public ICatBridge() : base("iCat", "", preferences)
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

            var computerName = ProgramPreference.GetString(programId, "Acquisition Computer Name");

            if (computerName.ToUpper().Equals(Environment.MachineName.ToUpper()))
            {
                QueuePatientForAcquisition(programId, patient);

                // When we are the acquisition machine we don't actually want to launch the program.
                return false;
            }
            else
            {
                arguments = "PatientID" + GetPatientId(programId, patient);
            }

            return true;
        }

        /// <summary>
        ///     <para>
        ///         Queues the specified <paramref name="patient"/> for image acquisitioning by
        ///         writing the details of the patient to the PM.XML file.
        ///     </para>
        ///     <para>
        ///         If the <paramref name="patient"/> already exists in the PM.XML file, the
        ///         existing element will be updated instead of adding a new one.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient.</param>
        private void QueuePatientForAcquisition(long programId, Patient patient)
        {
            var id = GetPatientId(programId, patient);

            var xmlFilePath = ProgramPreference.GetString(programId, "xml_output_file_path");

            var xmlDocument = new XmlDocument();
            if (File.Exists(xmlFilePath))
            {
                try
                {
                    xmlDocument.Load(xmlFilePath);
                }
                catch
                {
                }
            }

            var xmlFileDir = Path.GetDirectoryName(xmlFilePath);
            if (!Directory.Exists(xmlFileDir))
            {
                Directory.CreateDirectory(xmlFileDir);
            }

            var patientsElement = xmlDocument.DocumentElement;
            if (patientsElement == null)
            {
                patientsElement = xmlDocument.CreateElement("Patients");
                xmlDocument.AppendChild(patientsElement);
            }

            // Search the document to see if the patient already exists in the file.
            XmlElement patientElement = null;
            for (int i = 0; i < patientsElement.ChildNodes.Count; i++)
            {
                if (patientsElement.ChildNodes[i].SelectSingleNode("ID").InnerText == id)
                {
                    patientElement = (XmlElement)patientsElement.ChildNodes[i];
                    break;
                }
            }

            // If no element exists for the patient we create one; otherwise we clear the existing one.
            if (patientElement == null)
            {
                patientElement = xmlDocument.CreateElement("Patient");
                patientsElement.AppendChild(patientElement);
            }
            else patientElement.RemoveAll();

            // Get the path of the return folder.
            var returnFolderPath = ProgramPreference.GetString(programId, "return_folder_path");
            if (!Directory.Exists(returnFolderPath))
            {
                Directory.CreateDirectory(returnFolderPath);
            }

            // Create the child elements.
            CreateElement(xmlDocument, patientElement, "ID", id);
            CreateElement(xmlDocument, patientElement, "LastName", patient.LName);
            CreateElement(xmlDocument, patientElement, "FirstName", patient.FName);
            CreateElement(xmlDocument, patientElement, "MiddleName", patient.MiddleI);
            CreateElement(xmlDocument, patientElement, "BirthDate", patient.Birthdate.ToString("yyyy/MM/dd"));
            CreateElement(xmlDocument, patientElement, "Gender", patient.Gender == PatientGender.Female ? "Female" : "Male");
            CreateElement(xmlDocument, patientElement, "Remarks", "");
            CreateElement(xmlDocument, patientElement, "ReturnPath", returnFolderPath.Replace(@"\", "/"));

            using (var stream = File.Open(xmlFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var xmlWriter = XmlWriter.Create(
                stream, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "   "
                }))
            {
                xmlDocument.Save(xmlWriter);

                xmlWriter.Flush();
            }

            MessageBox.Show(
                "Done.", 
                Name, 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }

        /// <summary>
        ///     <para>
        ///         Creates a new element with the specified <paramref name="name"/> and
        ///         <paramref name="value"/> and appends it to the specified 
        ///         <paramref name="parent"/> element.
        ///     </para>
        /// </summary>
        /// <param name="document">The containing document.</param>
        /// <param name="parent">The parent element.</param>
        /// <param name="name">The name of the element.</param>
        /// <param name="value">The element value.</param>
        private static void CreateElement(XmlDocument document, XmlElement parent, string name, string value)
        {
            var element = document.CreateElement(name);
            element.InnerText = value;
            parent.AppendChild(element);
        }

        /// <summary>
        ///     <para>
        ///         Begins watching the return folder. When iCal generated a response file we
        ///         will pick it up and process it.
        ///     </para>
        /// </summary>
        public static void StartFileWatcher()
        {
            program = Program.GetByType<ICatBridge>();
            if (!program.Enabled)
            {
                return;
            }

            var returnFolderPath = ProgramPreference.GetString(program.Id, "return_folder_path");
            if (!Directory.Exists(returnFolderPath))
            {
                return;
            }

            fileSystemWatcher = new FileSystemWatcher(returnFolderPath, "*.xml");
            fileSystemWatcher.Created += new FileSystemEventHandler(OnCreated);
            fileSystemWatcher.Renamed += new RenamedEventHandler(OnRenamed);
            fileSystemWatcher.EnableRaisingEvents = true;

            // Process all waiting files
            var existingFiles = Directory.GetFiles(returnFolderPath, "*.xml");
            for (int i = 0; i < existingFiles.Length; i++)
            {
                ProcessFile(program.Id, existingFiles[i]);
            }
        }

        private static void OnCreated(object source, FileSystemEventArgs e)
        {
            int i = 0;
            while (i < 5)
            {
                try
                {
                    ProcessFile(program.Id, e.FullPath);
                    break;
                }
                catch { }

                i++;
            }
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            int i = 0;
            while (i < 5)
            {
                try
                {
                    ProcessFile(program.Id, e.FullPath);
                    break;
                }
                catch { }

                i++;
            }
        }

        /// <summary>
        /// Processes the specified file.
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="path">The full path of the file to process.</param>
        private static void ProcessFile(long programId, string path)
        {
            var xmlFilePath = ProgramPreference.GetString(programId, "xml_output_file_path");
            if (!File.Exists(xmlFilePath))
            {
                return;
            }

            var fileName = Path.GetFileName(path);
            
            try
            {
                string patId = fileName.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)[0];

                var xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlFilePath);

                var patientsElement = xmlDocument.DocumentElement;
                if (patientsElement == null)
                {
                    return;
                }

                XmlElement patientElement = null;
                for (int i = 0; i < patientsElement.ChildNodes.Count; i++)
                {
                    if (patientsElement.ChildNodes[i].SelectSingleNode("ID").InnerXml == patId)
                    {
                        patientElement = (XmlElement)patientsElement.ChildNodes[i];

                        break;
                    }
                }

                if (patientElement == null) return;

                patientsElement.RemoveChild(patientElement);

                using (var stream = File.Open(xmlFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var xmlWriter = XmlWriter.Create(
                    stream, new XmlWriterSettings
                    {
                        Indent = true,
                        IndentChars = "   "
                    }))
                {
                    xmlDocument.Save(xmlWriter);

                    xmlWriter.Flush();
                }

                File.Delete(path);
            }
            catch
            {
            }
        }
    }
}
