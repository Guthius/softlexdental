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
    /// <summary>
    ///     <para>
    ///         CaptureLink reads the bridge parameters from the clipboard.
    ///     </para>
    /// </summary>
    public class CaptureLinkBridge : CommandLineBridge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CaptureLinkBridge"/> class.
        /// </summary>
        public CaptureLinkBridge() : base("CaptureLink", "", "https://www.henryschein.ca/")
        {
            RequirePatient = true;
        }

        /// <summary>
        ///     <para>
        ///         Copies patient information to the clipboard.
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

            var payload = Tidy(patient.LName) + " " + Tidy(patient.FName) + " " + Tidy(GetPatientId(programId, patient));

            Clipboard.Clear();
            Clipboard.SetText(payload, TextDataFormat.Text);

            return true;
        }

        /// <summary>
        /// Removes double-quotes and spaces from the specified string.
        /// </summary>
        private static string Tidy(string input) => input.Replace("\"", "").Replace(" ", "");
    }
}
