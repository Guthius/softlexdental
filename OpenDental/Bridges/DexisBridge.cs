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
    /// <summary>
    /// Also used by the XDR bridge.
    /// </summary>
    public class DexisBridge : CommandLineBridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Define("info_file_path", "InfoFile Path", BridgePreferenceType.File)
        };

        // Encoding 1252 was specifically requested by the XDR development team to help with accented characters (ex Canadian customers).
        // On 05/19/2015, a reseller noticed UTF8 encoding in the Dexis bridge caused a similar issue.
        // We decided it was safe to switch Dexis from using UTF8 to code page 1252 because the bridge depends entirely on the bridge ID,
        // not the patient names.  Thus there is no chance of breaking the Dexis bridge by using code page 1252 instead.
        // 06/01/2015 A customer tested and confirmed that using the XDR bridge and thus coding page 1252, solved the special characters issue.
        private static readonly Encoding encoding = Encoding.GetEncoding(1252);

        /// <summary>
        /// Initializes a new instance of the <see cref="DexisBridge"/> class.
        /// </summary>
        public DexisBridge() : base(
            "Dexis", 
            "From remarkable image quality to award-winning products, the legacy of DEXIS has joined forces with the excellence in training, support, and innovation of KaVo Imaging Solutions.", 
            "https://www.kavo.com/", 
            preferences)
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
            var infoFilePath = ProgramPreference.GetString(programId, "info_file_path");
            if (string.IsNullOrEmpty(infoFilePath))
            {
                infoFilePath = Path.Combine(Path.GetTempPath(), "infofile.txt");
            }

            arguments = "\"@" + infoFilePath + "\"";
            
            var patientId = GetPatientId(programId, patient);

            using (var stream = File.Open(infoFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var streamWriter = new StreamWriter(stream, encoding))
            {
                streamWriter.WriteLine(
                    patient.LName + ", " + patient.FName + "  " + 
                    patient.Birthdate.ToShortDateString() + 
                    "  (" + patientId + ")");

                streamWriter.WriteLine("PN=" + patientId);
                streamWriter.WriteLine("LN=" + patient.LName);
                streamWriter.WriteLine("FN=" + patient.FName);
                streamWriter.WriteLine("BD=" + patient.Birthdate.ToShortDateString());
                streamWriter.WriteLine(patient.Gender == PatientGender.Female ? "SX=F" : "SX=M");
            }

            return true;
        }
    }
}
