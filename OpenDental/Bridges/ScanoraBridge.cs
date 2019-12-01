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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class ScanoraBridge : CommandLineBridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Define("ini_file_path", "Import.ini Path", BridgePreferenceType.File)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanoraBridge"/> class.
        /// </summary>
        public ScanoraBridge() : base("Scanora", "", "https://www.kavo.com/en-us/soredex", preferences)
        {
            RequirePatient = true;
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

            var iniPath = ProgramPreference.GetString(programId, "ini_file_path");
            if (string.IsNullOrEmpty(iniPath))
            {
                MessageBox.Show(
                    "The location of the Import.ini file has not yet been configured.", 
                    Name, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return false;
            }

            // Chris Bope (Product Manager for Sorodex) said "ANSI" is the preferred encoding.
            // Code page 1252 is the most commonly used ANSI code page, and we use 1252 in other
            // bridges as well.
            var encoding = Encoding.GetEncoding(1252);

            using (var stream = File.Open(iniPath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var streamWriter = new StreamWriter(stream, encoding))
            {
                streamWriter.WriteLine("");
                streamWriter.WriteLine("[PracticeManagementInterface]");
                streamWriter.WriteLine("");
                streamWriter.WriteLine("");
                streamWriter.WriteLine("CLEAR_PRACTICE_MANAGEMENT_AUTOMATICALLY = 1");
                streamWriter.WriteLine("USE_PRACTICE_MANAGEMENT = 1");
                streamWriter.WriteLine("PATID = " + Tidy(GetPatientId(programId, patient)));
                streamWriter.WriteLine("PATLNAME = " + Tidy(patient.LName));
                streamWriter.WriteLine("PATMNAME = " + Tidy(patient.MiddleI));
                streamWriter.WriteLine("PATFNAME = " + Tidy(patient.FName));
                streamWriter.WriteLine("PATSOCSEC = " + (patient.SSN.Replace("0", "").Trim() != "" ? patient.SSN : ""));

                // We changed the date format from yyyy-MM-dd to ToShortDateString() because of an
                // email from Chris Bope (Product Manager for Sorodex). Chris said that a valid 
                // date must be in ToShortDateString() because their program assumes that is the 
                // format when it gets saved.
                streamWriter.WriteLine("PATBD = " + patient.Birthdate.ToShortDateString());

                streamWriter.WriteLine("PROVIDER1 = " + Providers.GetFormalName(patient.PriProv));
                streamWriter.WriteLine("PROVIDER2 = " + Providers.GetFormalName(patient.SecProv));
                streamWriter.WriteLine("ADDRESS1 = " + Tidy(patient.Address));
                streamWriter.WriteLine("ADDRESS2 = " + Tidy(patient.Address2));
                streamWriter.WriteLine("CITY = " + Tidy(patient.City));
                streamWriter.WriteLine("STATE = " + Tidy(patient.State));
                streamWriter.WriteLine("ZIP = " + Tidy(patient.Zip));
                streamWriter.WriteLine("HOMEPHONE = " + new string(patient.HmPhone.Where(x => char.IsDigit(x)).ToArray()));
                streamWriter.WriteLine("WORKPHONE = " + new string(patient.WkPhone.Where(x => char.IsDigit(x)).ToArray()));
                streamWriter.WriteLine("EMAIL1 = " + Tidy(patient.Email));
            }

            return true;
        }
    }
}
