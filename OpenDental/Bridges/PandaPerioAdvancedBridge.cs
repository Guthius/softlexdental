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
using Microsoft.Win32;
using OpenDentBusiness;
using OpenDentBusiness.Bridges;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class PandaPerioAdvancedBridge : CommandLineBridge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PandaPerioAdvancedBridge"/> class.
        /// </summary>
        public PandaPerioAdvancedBridge() : base(
            "Panda Perio (Advanced)",
            "PANDA Perio® is more than a unique and revolutionary dental software application. It " +
            "represents a new paradigm in the way computer software can enhance the practice of " +
            "dentistry. On its most basic level PANDA Perio is the most customizable, " +
            "comprehensive and detailed digital clinical record ever devised. However the real " +
            "power of PANDA Perio is its ability to formulate detailed treatment plans and " +
            "automatically generate customizable letters and reports.", 
            "https://www.pandaperio.com/")
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

            if (patient.Birthdate.Year < 1880)
            {
                MessageBox.Show(
                    "Patient must have a valid birthdate.", 
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            var iniFilePath = GetIniFilePathFromRegistry();
            if (string.IsNullOrEmpty(iniFilePath))
            {
                MessageBox.Show(
                    "The INI file is not available.",
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            WriteIniFile(programId, patient, iniFilePath);

            return true;
        }

        /// <summary>
        /// Writes the INI file with the patient information.
        /// </summary>
        /// <param name="programId">THe ID of the program.</param>
        /// <param name="patient">The patient.</param>
        /// <param name="iniFilePath">The path of the INI file to write.</param>
        private static void WriteIniFile(long programId, Patient patient, string iniFilePath)
        {
            using (var stream = File.Open(iniFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var streamWriter = new StreamWriter(stream))
            {
                streamWriter.WriteLine("[PATIENT]");
                streamWriter.WriteLine("PatID=" + GetPatientId(programId, patient));
                streamWriter.WriteLine("Title=" + patient.Title);
                streamWriter.WriteLine("FirstName=" + patient.FName);
                streamWriter.WriteLine("LastName=" + patient.LName);
                streamWriter.WriteLine("MiddleInitial=" + patient.MiddleI);
                streamWriter.WriteLine("NickName=" + patient.Preferred);
                streamWriter.WriteLine("Sex=" + (patient.Gender == PatientGender.Female ? "F" : "M"));
                streamWriter.WriteLine("BirthDate=" + patient.Birthdate.ToShortDateString());
                streamWriter.WriteLine("HomePhone=" + patient.HmPhone);
                streamWriter.WriteLine("WorkPhone=" + patient.WkPhone);
                streamWriter.WriteLine("WorkPhoneExt=");
                streamWriter.WriteLine("SSN=" + (patient.SSN.Replace("0", "").Trim() != "" ? patient.SSN : ""));
                streamWriter.WriteLine("ProviderName=" + Provider.GetById(patient.PriProv).GetFormalName());

                var guarantor = Patients.GetPat(patient.Guarantor);
                streamWriter.WriteLine("[ACCOUNT]");
                streamWriter.WriteLine("AccountNO=" + guarantor?.PatNum.ToString());
                streamWriter.WriteLine("Title=" + guarantor?.Title);
                streamWriter.WriteLine("FirstName=" + guarantor?.FName);
                streamWriter.WriteLine("LastName=" + guarantor?.LName);
                streamWriter.WriteLine("MiddleInitial=" + guarantor?.MiddleI);
                streamWriter.WriteLine("Suffix=");
                streamWriter.WriteLine("HomePhone=" + guarantor?.HmPhone);
                streamWriter.WriteLine("WorkPhone=" + guarantor?.WkPhone);
                streamWriter.WriteLine("WorkPhoneExt=");
                streamWriter.WriteLine("Street=" + (guarantor?.Address + " " + guarantor?.Address2).Trim());
                streamWriter.WriteLine("City=" + guarantor?.City);
                streamWriter.WriteLine("State=" + guarantor?.State);
                streamWriter.WriteLine("Zip=" + guarantor?.Zip);

                var referral = Referrals.GetIsDoctorReferralsForPat(patient.PatNum).LastOrDefault();
                streamWriter.WriteLine("[REFERDR]");
                streamWriter.WriteLine("RefdrID=" + referral?.ReferralNum.ToString());
                streamWriter.WriteLine("RefdrLastName=" + referral?.LName);
                streamWriter.WriteLine("RefdrFirstName=" + referral?.FName);
                streamWriter.WriteLine("RefdrMiddleInitial=" + referral?.MName);
                streamWriter.WriteLine("RefdrNickName=");
                streamWriter.WriteLine("RefdrStreet=" + referral?.Address);
                streamWriter.WriteLine("RefdrStreet2=" + referral?.Address2);
                streamWriter.WriteLine("RefdrCity=" + referral?.City);
                streamWriter.WriteLine("RefdrState=" + referral?.ST);
                streamWriter.WriteLine("RefdrZip=" + referral?.Zip);
                streamWriter.WriteLine("RefdrWorkPhone=" + referral?.Telephone);
                streamWriter.WriteLine("RefdrFax=");
            }
        }

        /// <summary>
        /// Gets the path of the Panda Perio pass file from the system registry.
        /// </summary>
        /// <returns></returns>
        private static string GetIniFilePathFromRegistry()
        {
            var passFilePath = "";

            using (var registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Panda Perio\Panda"))
            {
                if (registryKey != null)
                {
                    object registryValue = registryKey.GetValue("PassfilePath");
                    if (registryValue != null)
                    {
                        passFilePath = registryValue.ToString();
                    }
                }
            }

            return passFilePath;
        }
    }
}
