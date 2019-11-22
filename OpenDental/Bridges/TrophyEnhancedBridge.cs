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
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class TrophyEnhancedBridge : CommandLineBridge
    {
        // TODO: Test this bridge...

        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Custom("storage_path", "Storage Path", BridgePreferenceType.FolderPath),
            BridgePreference.Custom("numbered_mode_enabled", "Enable Numbered Mode", BridgePreferenceType.Boolean)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="TrophyEnhancedBridge"/> class.
        /// </summary>
        public TrophyEnhancedBridge() : base("Trophy (Enhanced)", "", preferences)
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
            string Tidy(string input) => input.Replace("\"", "").Replace("'", "");

            arguments = "";

            string storagePath = ProgramPreference.GetString(programId, "storage_path");

            if (!Directory.Exists(storagePath))
            {
                MessageBox.Show(
                    "Invalid storage path: " + storagePath,
                    Name, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return false;
            }

            string patientFolder;

            if (patient.TrophyFolder == "")
            {
                var numberedModeEnabled = ProgramPreference.GetBool(programId, "numbered_mode_enabled");
                try
                {
                    if (numberedModeEnabled)
                    {
                        patientFolder = AutomaticallyGetTrophyFolderNumbered(patient, storagePath);
                    }
                    else
                    {
                        patientFolder = AutomaticallyGetTrophyFolder(patient, storagePath);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        exception.Message,
                        Name, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);

                    return false;
                }

                if (string.IsNullOrEmpty(patientFolder)) return false;
                
                patientFolder = Path.Combine(storagePath, patientFolder);
            }
            else
            {
                patientFolder = Path.Combine(storagePath, patient.TrophyFolder);
            }

            arguments = 
                " -P" + patientFolder + 
                " -N" + Tidy(patient.LName) + ", " + Tidy(patient.FName);

            return true;
        }

        /// <summary>
        ///     <para>
        ///         Gets the storage path for the specified <paramref name="patient"/>. If no 
        ///         directory exists for the <paramref name="patient"/> one will be created.
        ///     </para>
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="storagePath">The path to the root storage directory.</param>
        private static string AutomaticallyGetTrophyFolderNumbered(Patient patient, string storagePath)
        {
            var patientId = patient.PatNum.ToString().PadLeft(8, '0');
            var patientPath = Path.Combine(patientId.Substring(patientId.Length - 2), patientId);

            var fullPath = Path.Combine(storagePath, patientPath);
            if (!Directory.Exists(fullPath))
            {
                try
                {
                    Directory.CreateDirectory(fullPath);
                }
                catch (Exception exception)
                {
                    throw new Exception("Error. Could not create folder: " + fullPath, exception);
                }
            }

            var oldPatient = patient.Copy();
            patient.TrophyFolder = patientPath;

            Patients.Update(patient, oldPatient);

            return patientPath;
        }

        /// <summary>
        ///     <para>
        ///         Gets the storage path for the specified <paramref name="patient"/>. If no 
        ///         directory exists for the <paramref name="patient"/> one will be created.
        ///     </para>
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="storagePath">The path to the root storage directory.</param>
        private static string AutomaticallyGetTrophyFolder(Patient patient, string storagePath)
        {
            string result = "";
            string alpha = patient.LName.Substring(0, 1);
            string alphaPath = alpha + ".rvg";

            storagePath = Path.Combine(storagePath, alphaPath);
            if (!Directory.Exists(storagePath))
            {
                throw new ApplicationException(
                    "Could not find expected path: " + storagePath + ". " +
                    "The enhanced Trophy bridge assumes that folders already exist with that naming convention.");
            }

            var unmatchedFolders = new List<TrophyFolderInfo>();
            var partialMatches = new List<TrophyFolderInfo>();
            int maxFolderNumber = 0;

            // Search the directory for the correct directory for the patient.
            var directories = Directory.GetDirectories(storagePath);
            foreach (var directory in directories)
            {
                var directoryName = Path.GetDirectoryName(directory);

                // Find the highest directory number.
                if (!int.TryParse(directoryName.Substring(1), out var folderNumber)) continue;
                if (folderNumber > maxFolderNumber)
                {
                    maxFolderNumber = folderNumber;
                }

                // Check if there is a FILEDATA.txt file, if not skip.
                var fileDataPath = Path.Combine(directory, "FILEDATA.txt");
                if (!File.Exists(fileDataPath))
                {
                    continue;
                }

                // if this folder is already in use by some other patient, then skip.
                if (Patients.IsTrophyFolderInUse(directory)) continue;

                var folderInfo = LoadFileData(fileDataPath, directoryName);
                if (folderInfo == null)
                {
                    continue;
                }

                // Check if the information of this folder matches the patient details.
                if (patient.LName.Equals(folderInfo.LastName, StringComparison.InvariantCultureIgnoreCase) &&
                    patient.FName.Equals(folderInfo.FirstName, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (patient.Birthdate == folderInfo.BirthDate)
                    {
                        result = Path.Combine(alphaPath, directoryName);
                    }
                    else
                    {
                        // If the name matched but the birth date did not, consider a partial match.
                        partialMatches.Add(folderInfo);
                    }
                }
                unmatchedFolders.Add(folderInfo);
            }

            // Check if we found a match, if not we need some manual intervention...
            if (string.IsNullOrEmpty(result))
            {
                // If there is one partial match we'll use that one...
                if (partialMatches.Count == 1)
                {
                    result = Path.Combine(alphaPath, partialMatches[0].FolderName);
                }
                else // If there are no partial matches or if there are more then 1 let the user pick the correct one...
                {
                    using (var formTrophyNamePick = new FormTrophyNamePick(unmatchedFolders))
                    {
                        if (formTrophyNamePick.ShowDialog() != DialogResult.OK) return "";

                        if (string.IsNullOrEmpty(formTrophyNamePick.PickedName))
                        {
                            maxFolderNumber++;

                            result = Path.Combine(alphaPath, alpha + maxFolderNumber.ToString().PadLeft(7, '0'));
                        }
                        else
                        {
                            result = Path.Combine(alphaPath, formTrophyNamePick.PickedName);
                        }
                    }
                }
            }

            var oldPatient = patient.Copy();
            patient.TrophyFolder = result;

            Patients.Update(patient, oldPatient);

            return result;
        }

        /// <summary>
        ///     <para>
        ///         Loads folder information from a given data file.
        ///     </para>
        /// </summary>
        /// <param name="dataFilePath">The full path of the data file.</param>
        /// <param name="directoryName">The directory name.</param>
        /// <returns>The folder information.</returns>
        private static TrophyFolderInfo LoadFileData(string dataFilePath, string directoryName)
        {
            var lines = File.ReadAllLines(dataFilePath);
            if (lines.Length < 2)
            {
                return null;
            }

            var folder = new TrophyFolderInfo
            {
                FolderName = directoryName,
                FirstName = GetValueFromLines("PRENOM", lines),
                LastName = GetValueFromLines("NOM", lines)
            };

            var birthDateData = GetValueFromLines("DATE", lines);
            try
            {
                folder.BirthDate = DateTime.ParseExact(birthDateData, "yyyyMMdd", CultureInfo.CurrentCulture.DateTimeFormat);
            }
            catch { }

            return folder;
        }

        /// <summary>
        ///     <para>
        ///         Searches the specified arry of lines for a line starting with the specified
        ///         <paramref name="key"/> and returns the value assigned to the given key.
        ///     </para>
        ///     <para>
        ///         Lines should be in the format: <b>KEY=VALUE</b>. Keys are not case sensitive.
        ///     </para>
        /// </summary>
        /// <param name="key">The key to look for.</param>
        /// <param name="lines">A array of lines.</param>
        /// <returns>The value of the given key.</returns>
        private static string GetValueFromLines(string key, string[] lines)
        {
            key = key.ToLower();
            foreach (var line in lines)
            {
                if (line.ToLower().StartsWith(key))
                {
                    var index = line.IndexOf('=');
                    if (index == -1)
                    {
                        return "";
                    }

                    return line.Substring(index + 1).TrimStart(' ');
                }
            }

            return "";
        }
    }

    /// <summary>
    ///     <para>
    ///         vStorage class for information about a Trophy folder loaded from a INFOFILE.txt 
    ///         file. Used to match folders to the correct patients.
    ///     </para>
    /// </summary>
    public class TrophyFolderInfo
    {
        public string FolderName;
        public string LastName;
        public string FirstName;
        public DateTime BirthDate;
    }
}
