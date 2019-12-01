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
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    /// <remarks>
    /// Data Should appear in the following file: 
    /// C:/Program Files/Digirex/Switch.ini 
    /// 
    /// and should be accessed/opened by 
    /// C:/Program Files/Digirex/digirex.ini
    /// </remarks>
    public class ApixiaBridge : CommandLineBridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Define("ini_file_path", "System path to Apixia Digital Imaging INI file", BridgePreferenceType.String)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ApixiaBridge"/> class.
        /// </summary>
        public ApixiaBridge() : base("Apixia Digital Imaging", "", "http://www.apixia.net/", preferences)
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

            var provider = Providers.GetProv(patient.PriProv);
            if (provider == null)
            {
                MessageBox.Show(
                    "The configured system path for the Apixia Digital Imaging INI file is invalid.",
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            var filePath = ProgramPreference.GetString(programId, "ini_file_path");
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show(
                    "Invalid provider for the selected patient.",
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            using (var stream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var streamWriter = new StreamWriter(stream))
            {
                streamWriter.WriteLine("[Patient]");
                streamWriter.WriteLine("ID=" + GetPatientId(programId, patient));
                streamWriter.WriteLine("Gender=" + patient.Gender.ToString());
                streamWriter.WriteLine("First=" + patient.FName);
                streamWriter.WriteLine("Last=" + patient.LName);
                streamWriter.WriteLine("Year=" + patient.Birthdate.Year.ToString());
                streamWriter.WriteLine("Month=" + patient.Birthdate.Month.ToString());
                streamWriter.WriteLine("Day=" + patient.Birthdate.Day.ToString());
                streamWriter.WriteLine("[Dentist]");
                streamWriter.WriteLine("ID=" + provider.Abbr);
                streamWriter.WriteLine("Password=digirex");
                streamWriter.Flush();
            }

            return true;
        }
    }
}
