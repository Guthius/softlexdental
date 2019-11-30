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
using System.Text;

namespace OpenDental.Bridges
{
    public class TrianaBridge : CommandLineBridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Custom("ini_file_path", "INI File Path", BridgePreferenceType.FilePath)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="TrianaBridge"/> clas.
        /// </summary>
        public TrianaBridge() : base(
            "Genoray Triana",
            "Triana is is able to manage Panoramic, Cephalometric, CT, and Portable, with Sensor image, and Triana is compatible with PACS", 
            preferences)
        {
        }

        /// <summary>
        /// Gets a string representation of the specified <paramref name="gender"/>.
        /// </summary>
        /// <param name="gender">The gender.</param>
        /// <returns>A string representation of the gender.</returns>
        private static string GetPatientGender(PatientGender gender)
        {
            switch (gender)
            {
                case PatientGender.Male:
                    return "1";

                case PatientGender.Female:
                    return "2";
            }
            return "3";
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
            string Tidy(string input) => input.Replace(";", "").Replace(" ", "");

            var iniFilePath = ProgramPreference.GetString(programId, "ini_file_path");

            arguments = "-F" + iniFilePath;

            using (var stream = File.Open(iniFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var streamWriter = new StreamWriter(stream, Encoding.GetEncoding(1252)))
            {
                streamWriter.WriteLine("[OPERATION]");
                streamWriter.WriteLine("EXECUTE=3");

                streamWriter.WriteLine("[PATIENT]");
                streamWriter.WriteLine("PATIENTID=" + Tidy(GetPatientId(programId, patient)));
                streamWriter.WriteLine("FIRSTNAME=" + Tidy(patient.FName));
                streamWriter.WriteLine("LASTNAME=" + Tidy(patient.LName));
                streamWriter.WriteLine("SOCIAL_SECURITY=");
                streamWriter.WriteLine("BIRTHDAY=" + (patient.Birthdate.Year > 1880 ? patient.Birthdate.ToString("yyyyMMdd") : ""));
                streamWriter.WriteLine("PATIENTCOMMENT=");
                streamWriter.WriteLine("GENDER=" + GetPatientGender(patient.Gender));
                streamWriter.Flush();
            }

            return true;
        }
    }
}
