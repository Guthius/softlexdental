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
using NDde.Advanced;
using NDde.Client;
using OpenDentBusiness;
using OpenDentBusiness.Bridges;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class DentalEyeBridge : DdeBridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.ProgramPath
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="DentalEyeBridge"/> class.
        /// </summary>
        public DentalEyeBridge() : base(
            "DentalEye",
            "Captures images from intraoral cameras, digital cameras and scanners. Import and export functions, image enhancement tools. Integration with digital X-ray devices.", 
            "http://www.dentaleye.com/", 
            preferences)
        {
            RequirePatient = true;
        }

        /// <summary>
        /// Starts the program.
        /// </summary>
        /// <param name="programId">The ID of the program</param>
        /// <param name="patient">The patient.</param>
        /// <returns>True if the program is running; otherwise, false.</returns>
        protected override bool RunProgram(long programId, Patient patient)
        {
            var programPath = ProgramPreference.GetString(programId, ProgramPreferenceName.ProgramPath);
            if (string.IsNullOrEmpty(programPath) || !File.Exists(programPath))
            {
                MessageBox.Show(
                    string.Format("'{0}' does not exist.", programPath),
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            if (Process.GetProcessesByName("DentalEye").Length == 0)
            {
                Process.Start(programPath);

                Thread.Sleep(TimeSpan.FromSeconds(4));
            }

            return true;
        }

        /// <summary>
        /// Creates a DDE client using the specified context.
        /// </summary>
        /// <param name="context">The DDE context.</param>
        /// <param name="programId">The ID of the program</param>
        /// <param name="patient">The patient.</param>
        /// <returns>A DDE client.</returns>
        protected override DdeClient CreateClient(DdeContext context, long programId, Patient patient) =>
            new DdeClient("DENTEYE", "Patient", context);

        /// <summary>
        /// Sends the required DDE commands to the specified <paramref name="client"/>.
        /// </summary>
        /// <param name="client">The DDE client.</param>
        /// <param name="programId">The ID of the program</param>
        /// <param name="patient">The patient.</param>
        protected override void SendCommands(DdeClient client, long programId, Patient patient)
        {
            var patientId = GetPatientId(programId, patient);

            var patientAddress = patient.Address + "|";
            if (patient.Address2 != "")
            {
                patientAddress += patient.Address2 + "|";
            }
            patientAddress += patient.City + ", " + patient.State + " " + patient.Zip;

            // Add patient or modify if already exists.
            client.Execute(
                "[Add]" +
                "[" + patientId + "]" +
                "[" + patient.FName + "]" +
                "[" + patient.LName + "]" +
                "[" + patientAddress + "]" +
                "[" + patient.HmPhone + "]" +
                "[" + patient.WkPhone + "]" +
                "[" + patient.WirelessPhone + "]" +
                "[" + patient.Email + "]" +
                (patient.Gender == PatientGender.Female ? "[F]" : "[M]") +
                "[" + patient.Birthdate.ToString("yyyy-MM-dd") + "]", 
                2000);

            // Then, select patient.
            client.Execute(
                "[Search][" + patientId + "]", 
                2000);
        }
    }
}
