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
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class PattersonBridge : Bridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Define("program_path", "Program Path", BridgePreferenceType.String),
            BridgePreference.Define("ini_file_path", "Path to Patterson Imaging INI", BridgePreferenceType.String)
        };

        /// <summary>
        /// Initializes a ne instance of the <see cref="PattersonBridge"/> class.
        /// </summary>
        public PattersonBridge() : base("Patterson Imaging", "", "https://www.pattersondental.com/equipment-technology/digital-imaging", preferences)
        {
        }

        /// <summary>
        ///     <para>
        ///         Sends the specified <paramref name="patient"/> data to the remote program or 
        ///         service.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient details.</param>
        public override void Send(long programId, Patient patient)
        {
            if (patient == null)
            {
                return;
            }

            var programPath = ProgramPreference.GetString(programId, "program_path");

            var iniFilePath = ProgramPreference.GetString(programId, "ini_file_path");
            if (!iniFilePath.ToLower().EndsWith(".ini"))
            {
                MessageBox.Show(
                    "Path to Patterson Imaging INI is invalid in program link setup.",
                    Name, 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            var provider = Providers.GetProv(patient.PriProv);

            string ssn = Tidy(patient.SSN.ToString(), 9);
            if (ssn.Replace("-", "").Replace("0", "").Trim() == "")
            {
                ssn = "";//We do not send if the ssn is all zeros, because Patterson treats ssn like a primary key if present. If more than one patient have the same ssn, then they are treated as the same patient.
            }

            // TODO: Extract the logic from the VBBridges assembly and move it to this class.....

            VBbridges.Patterson.Launch(
                Tidy(patient.FName, 40),
                "",//Tidy(pat.MiddleI,1),//When there is no SSN and the name changes, Patterson creates a whole new patient record, which is troublesome for our customers.
                Tidy(patient.LName, 40),
                "",//Tidy(pat.Preferred,40),//When there is no SSN and the name changes, Patterson creates a whole new patient record, which is troublesome for our customers.
                Tidy(patient.Address, 40),
                Tidy(patient.City, 30),
                Tidy(patient.State, 2),
                Tidy(patient.Zip, 10),
                ssn,//This only works with ssn in america with no punctuation
                Tidy((patient.Gender == PatientGender.Male ? "M" : (patient.Gender == PatientGender.Female ? "F" : " ")), 1),//uses "M" for male "F" for female and " " for unkown
                Tidy(patient.Birthdate.ToShortDateString(), 11),
                LTidy(patient.PatNum.ToString(), 5),
                LTidy(provider.ProvNum.ToString(), 3),
                //LTidy(pat.PatNum.ToString(),5),//Limit is 5 characters, but that would only be exceeded if they are using random primary keys or they have a lot of data, neither case is common.
                //LTidy(prov.ProvNum.ToString(),3),//Limit is 3 characters, but that would only be exceeded if they are using random primary keys or they have a lot of data, neither case is common.
                Tidy(patient.FName, 40),
                Tidy(patient.LName, 40),
                programPath,
                iniFilePath);
        }

        private static string Tidy(string input, int maxLength)
        {
            input.Trim();
            if (input.Length > maxLength)
            {
                input = input.Substring(0, maxLength);
            }
            return input.Trim();
        }

        private static string LTidy(string intput, int maxLength)
        {
            if (intput.Length > maxLength)
            {
                intput = intput.Substring(intput.Length - maxLength);
            }
            return intput.Trim();
        }
    }
}
