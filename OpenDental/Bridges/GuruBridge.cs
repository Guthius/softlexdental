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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class GuruBridge : Bridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Custom("image_path", "Guru Image Path", BridgePreferenceType.FolderPath)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="GuruBridge"/> class.
        /// </summary>
        public GuruBridge() : base("Guru", "", preferences)
        {
        }

        /// <summary>
        /// Gets a string representation of the specified gender.
        /// </summary>
        /// <param name="gender">The gender.</param>
        /// <returns>A string representation of the gender.</returns>
        private static string GetPatientGender(PatientGender gender)
        {
            switch (gender)
            {
                case PatientGender.Male:
                    return "M";

                case PatientGender.Female:
                    return "F";
            }

            return "0";
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
            string Tidy(string input, int maxLength) => input.Length > maxLength ? input.Substring(0, maxLength) : input;

            if (patient == null)
            {
                MessageBox.Show(
                    "Please select a patient first.",
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            var imagePath = ProgramPreference.GetString(programId, "image_path");
            if (string.IsNullOrEmpty(imagePath))
            {
                MessageBox.Show(
                    "The image path has not yet been configured.",
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            try
            {
                var result = MVStart();
                if (result != 0)
                {
                    MessageBox.Show(
                        "An error has occured.",
                        Name,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                if (string.IsNullOrEmpty(patient.ImageFolder)) // Could happen if the images module has not been visited for a new patient.
                {
                    var oldPatient = patient.Copy();
                    patient.ImageFolder = patient.LName + patient.FName + patient.PatNum;
                    Patients.Update(patient, oldPatient);
                }

                result = MVSendPatient(
                    new MVPatient
                    {
                        LastName = Tidy(patient.LName, 64),
                        FirstName = Tidy(patient.FName, 64),
                        Sex = GetPatientGender(patient.Gender),
                        BirthDate = Tidy(patient.Birthdate.ToString("MMddyyyy"), 8),
                        ID = Tidy(GetPatientId(programId, patient), 64),
                        Directory = Tidy(Path.Combine(imagePath, patient.ImageFolder), 259)
                    });

                if (result != 0)
                {
                    MessageBox.Show(
                        "An error has occured.",
                        Name,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        [DllImport("MedVisorInterface.dll")]
        private static extern int MVStart();

        [DllImport("MedVisorInterface.dll")]
        private static extern int MVSendPatient(MVPatient mvPatient);

        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        public struct MVPatient
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
            public string LastName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
            public string MiddleName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
            public string FirstName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
            public string Prefix;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
            public string Suffix;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
            public string Sex;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string BirthDate;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
            public string ID;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string Directory;
        }
    }
}
