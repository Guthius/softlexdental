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
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace OpenDental.Bridges
{
    public class TigerViewBridge : CommandLineBridge
    {
        // TODO: Needs additional work. The file system watcher is never started...

        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Custom("ini_file_path", "Tiger1.ini Path", BridgePreferenceType.FilePath),
            BridgePreference.Custom("date_format", "Birthdate Format (default MM/dd/yy)", BridgePreferenceType.String),
            BridgePreference.Custom("emr_folder_path", "TigerView EMR Folder Path", BridgePreferenceType.FolderPath)
        };

        private static FileSystemWatcher fileSystemWatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="TigerViewBridge"/> class.
        /// </summary>
        public TigerViewBridge() : base("TigerView", "", preferences)
        {
        }

        /// <summary>
        ///     <para>
        ///         Writes the details of the specified patient to the TigerView config file.
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
            string Tidy(string input, int maxLength) => input.Length < (maxLength + 1) ? input : input.Substring(0, maxLength);

            arguments = "";

            var iniFilePath = ProgramPreference.GetString(programId, "ini_file_path");
            if (!File.Exists(iniFilePath))
            {
                return false;
            }

            var dateFormat = ProgramPreference.GetString(programId, "date_format", "MM/dd/yy");

            var patientInfo = new Dictionary<string, string>
            {
                ["LastName"] = patient.LName,
                ["FirstName"] = patient.FName,
                ["PatientID"] = GetPatientId(programId, patient),
                ["PatientSSN"] = patient.SSN,
                ["Gender"] = (patient.Gender == PatientGender.Female ? "Female" : "Male"),
                ["DOB"] = patient.Birthdate.ToString(dateFormat),
                ["AddrStreetNo"] = patient.Address,
                ["AddrCity"] = patient.City,
                ["AddrState"] = patient.State,
                ["AddrZip"] = patient.Zip,
                ["PhHome"] = Tidy(patient.HmPhone, 13),
                ["PhWork"] = Tidy(patient.WkPhone, 13)
            };

            WritePatientInfoToIni(iniFilePath, "", patientInfo);

            return true;
        }

        /// <summary>
        /// Writes the specified patient information to the TigerView INI file.
        /// </summary>
        /// <param name="iniFilePath">The full path of the INI file.</param>
        /// <param name="section">The section to write to.</param>
        /// <param name="patientInfo">The patient information.</param>
        protected static void WritePatientInfoToIni(string iniFilePath, string section, Dictionary<string, string> patientInfo)
        {
            foreach (var keyValuePair in patientInfo)
            {
                WritePrivateProfileString(
                    section, 
                    keyValuePair.Key, 
                    keyValuePair.Value, 
                    iniFilePath);
            }
        }




        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);


        

        public static void StartFileWatcher()
        {
            var program = Program.GetByType<TigerViewBridge>();
            if (!program.Enabled)
            {
                return;
            }

            var folderPath = ProgramPreference.GetString(program.Id, "emr_folder_path");
            if (!Directory.Exists(folderPath))
            {
                return;
            }

            fileSystemWatcher = new FileSystemWatcher(folderPath, "*.tig");
            fileSystemWatcher.Created += (s, e) =>
            {
                try
                {
                    ProcessFile(program.Id, e.FullPath);
                }
                catch { }
            };
            fileSystemWatcher.EnableRaisingEvents = true;

            var unprocessedFiles = Directory.GetFiles(folderPath, "*.tig");
            for (int i = 0; i < unprocessedFiles.Length; i++)
            {
                try
                {
                    ProcessFile(program.Id, unprocessedFiles[i]);
                }
                catch { }
            }
        }

        /// <summary>
        ///     <para>
        ///         Processes the file with the specified <paramref name="path"/>. Will attempt to
        ///         extract the patient identifier from the filename and upon success will move the
        ///         file to the image storage location for that patient and create a document entry
        ///         for the file so that it will visible from the Images module.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="path">The full path of the file to process.</param>
        private static void ProcessFile(long programId, string path)
        {
            var fileName = Path.GetFileName(path);

            // Get the patient identifier from the filename.  Example: tmb123.20091119.XXXXXX.tig where X is identifier
            var tokens = fileName.Split(new char[] { '.' });
            if (tokens.Length != 4)
            {
                return;
            }

            var identifier = tokens[2]; //Third quadrant

            Patient patient;
            if (!IsUsingChartNumber(programId))
            {
                if (!long.TryParse(identifier, out var patientId))
                {
                    return;
                }

                patient = Patients.GetPat(patientId);
            }
            else
            {
                patient = Patients.GetPatByChartNumber(identifier);
            }

            // Could not find a patient matching the given patient ID / chart number.
            if (patient == null) return;

            long imageCategoryId = 0;

            var imageCategoryDefinitions = Definition.GetByCategory(DefinitionCategory.ImageCats);
            foreach (var definition in imageCategoryDefinitions)
            {
                if (definition.Description.Equals("", StringComparison.InvariantCultureIgnoreCase))
                {
                    imageCategoryId = definition.Id;

                    break;
                }
            }

            // If no "Xray" category exists, insert new category with the name "Xray".
            if (imageCategoryId == 0)
            {
                imageCategoryId = Definition.Insert(new Definition
                {
                    Description = "Xray",
                    Category = DefinitionCategory.ImageCats,
                    Value = "X", //Will make this category show in the chart module
                    SortOrder = imageCategoryDefinitions.Count
                });
            }

            var newFileName = "TV_" + fileName.Substring(0, fileName.IndexOf('.')) + CodeBase.MiscUtils.CreateRandomAlphaNumericString(4) + ".tig";
            var newPath = Path.Combine(ImageStore.GetPatientFolder(patient), newFileName);

            File.Move(path, newPath);

            Documents.Insert(
                new Document
                {
                    DocCategory = imageCategoryId,
                    FileName = newFileName,
                    PatNum = patient.PatNum,
                    ImgType = ImageType.Photo,
                    DateCreated = DateTime.Now,
                    Description = newFileName
                }, patient);
        }
    }
}
