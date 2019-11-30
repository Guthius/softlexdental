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
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class ProgenyBridge : Bridge
    {
        private static readonly BridgePreference[] preferences =
{
            BridgePreference.ProgramPath,
            BridgePreference.CommandLineArguments
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgenyBridge"/> class.
        /// </summary>
        public ProgenyBridge() : base(
            "Progeny", 
            "Progeny Imaging Software is designed to provide easy access to digital images, " +
            "simplified storage and image recall, as well as many tools that are useful for image " +
            "evaluation and diagnosis.", 
            "https://www.midmark.com/dental/products/imaging/software/detail/progeny-imaging", 
            preferences)
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
            string Tidy(string input) => input.Replace("\"", "").Replace(",", "");

            var programPath = ProgramPreference.GetString(programId, ProgramPreferenceName.ProgramPath);
            if (string.IsNullOrEmpty(programPath))
            {
                return;
            }

            if (!File.Exists(programPath))
            {
                MessageBox.Show(
                    string.Format("'{0}' does not exist.", programPath),
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (patient != null)
            {
                try
                {
                    Process.Start(programPath, "cmd=start");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        exception.Message,
                        Name,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                return;
            }

            try
            {
                var commandLineArguments =
                    "cmd=open " +
                    "id=\"" + Tidy(GetPatientId(programId, patient)) + "\" " +
                    "first=\"" + Tidy(patient.FName) + "\" " +
                    "last=\"" + Tidy(patient.LName) + "\"";

                Process.Start(programPath, commandLineArguments);
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
    }
}
