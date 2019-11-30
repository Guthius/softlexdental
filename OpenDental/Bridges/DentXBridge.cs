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
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class DentXBridge : DdeBridge
    {
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private static string ReadValue(string fileName, string section, string key)
        {
            StringBuilder strBuild = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", strBuild, 255, fileName);
            return strBuild.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DentXBridge"/> class.
        /// </summary>
        public DentXBridge() : base("DentX", "", "http://www.dent-x.com/")
        {
        }

        /// <summary>
        /// Starts the program.
        /// </summary>
        /// <param name="programId">The ID of the program</param>
        /// <param name="patient">The patient.</param>
        /// <returns>True if the program is running; otherwise, false.</returns>
        protected override bool RunProgram(long programId, Patient patient)
        {
            var iniFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "dentx.ini");
            if (!File.Exists(iniFile))
            {
                MessageBox.Show(
                    "Could not find " + iniFile, 
                    Name, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return false;
            }

            var programInstances = Process.GetProcessesByName("ProImage");
            if (programInstances.Length == 0)
            {
                string programPath = ReadValue(iniFile, "imagemgt", "MainFile");
                if (File.Exists(programPath))
                {
                    Process.Start(programPath);

                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
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
            new DdeClient("ProImage", "Image", context);

        /// <summary>
        /// Sends the required DDE commands to the specified <paramref name="client"/>.
        /// </summary>
        /// <param name="client">The DDE client.</param>
        /// <param name="programId">The ID of the program</param>
        /// <param name="patient">The patient.</param>
        protected override void SendCommands(DdeClient client, long programId, Patient patient)
        {
            string Tidy(string input) => input.Replace(",", "");

            var command = "Xray," +
                Tidy(GetPatientId(programId, patient)) + "," +
                Tidy(patient.FName) + "," +
                Tidy(patient.LName) + "," +
                patient.Birthdate.ToShortDateString() + "," +
                (patient.Gender == PatientGender.Female ? "F," : "M,") +
                Tidy(patient.Address) + "," +
                Tidy(patient.City) + "," +
                Tidy(patient.State) + "," +
                Tidy(patient.Zip);

            client.Execute(command, 2000);
        }
    }
}
