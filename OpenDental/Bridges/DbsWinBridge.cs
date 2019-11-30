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

namespace OpenDental.Bridges
{
    public class DbsWinBridge : CommandLineBridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Custom("file_path", "Patimport.txt path", BridgePreferenceType.FilePath)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="DbsWinBridge"/> class.
        /// </summary>
        public DbsWinBridge() : base(
            "DBSWIN Imaging Software",
            "DBSWIN is an image management system that allows the physician to acquire, display, edit (e.g., resize, adjust contrast, etc.), review, store, print, and distribute medical images within a Picture Archiving and Communication System (PACS) environment.", 
            "https://www.duerrdental.com/", preferences)
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
            string Tidy(string input) => input.Replace(";", "");

            arguments = "";

            var filePath = ProgramPreference.GetString(programId, "file_path");

            using (var stream = File.Open(filePath, FileMode.Create, FileAccess.Write))
            using (var streamWriter = new StreamWriter(stream))
            {
                streamWriter.Write(Tidy(patient.LName) + ";");
                streamWriter.Write(Tidy(patient.FName) + ";");
                streamWriter.Write(patient.Birthdate.ToString("d.M.yyyy") + ";");
                streamWriter.Write(Tidy(GetPatientId(programId, patient)) + ";");
                streamWriter.Write(Tidy(patient.City) + ";");
                streamWriter.Write(Tidy(patient.Address) + ";");
                streamWriter.Write(Tidy(patient.HmPhone) + ";");
                streamWriter.Write(";"); // Title
                streamWriter.Write(patient.Gender == PatientGender.Female ? "f;" : "m;");
                streamWriter.Write(Tidy(patient.Zip) + ";");
            }

            return true;
        }
    }
}
