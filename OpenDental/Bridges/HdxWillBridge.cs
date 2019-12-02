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

namespace OpenDental.Bridges
{
    public class HdxWillBridge : CommandLineBridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Define("ini_file_path", "System path to HDX WILL Argument INI file", BridgePreferenceType.String)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="HdxWillBridge"/> class.
        /// </summary>
        public HdxWillBridge() : base("HDX WILL", "", "http://www.hdx.co.kr/", preferences)
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

            var iniFilePath = ProgramPreference.GetString(programId, "ini_file_path");

            using (var stream = File.Open(iniFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var streamWriter = new StreamWriter(stream))
            {
                streamWriter.WriteLine("[Patient Info = 00]");
                streamWriter.WriteLine("PATIENT_APPLYNO=" + DateTime.Now.ToString("yyyyMMddhhmmssfff").Substring(1));
                streamWriter.WriteLine("PATIENT_ID=" + patient.PatNum.ToString());
                streamWriter.WriteLine("PATIENT_NAME=" + patient.FName + " " + patient.LName);
                streamWriter.WriteLine("PATIENT_ENAME=");
                streamWriter.WriteLine("PATIENT_SEX=" + patient.Gender.ToString().Substring(0, 1));
                streamWriter.WriteLine("PATIENT_AGE=" + patient.Age);
                streamWriter.WriteLine("PATIENT_BIRTH_DATE=" + patient.Birthdate.ToString("yyyyMMdd"));
                streamWriter.WriteLine("PATIENT_ADDR=" + patient.Address);
                streamWriter.WriteLine("PATIENT_PID=" + patient.ChartNumber.ToString());
                streamWriter.WriteLine("PATIENT_IPDOPD=");
                streamWriter.WriteLine("PATIENT_DOCTOR=" + Provider.GetById(patient.PriProv).GetFormalName());
                streamWriter.WriteLine("PATIENT_PHON1=" + patient.WirelessPhone);
                streamWriter.WriteLine("PATIENT_PHON2=" + patient.HmPhone);
                streamWriter.WriteLine("PATIENT_EXAMNAME=");
                streamWriter.WriteLine("INPUT_DATE=" + DateTime.Now.ToString("yyyyMMdd"));
            }

            return true;
        }
    }
}
