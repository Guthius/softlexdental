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
    public class DigoraBridge : Bridge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DigoraBridge"/> class.
        /// </summary>
        public DigoraBridge() : base("Digora", "", "https://www.kavo.com/")
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
            string Tidy(string input) => input.Replace("\"", "");

            if (patient == null)
            {
                MessageBox.Show(
                    "Please select a patient first.",
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            try
            {
                Clipboard.SetText(
                    "$$DFWIN$$ OPEN " +
                        "-n\"" + Tidy(patient.LName) + ", " + Tidy(patient.FName) + "\" " +
                        "-c\"" + GetPatientId(programId, patient) + "\" -r -a", 

                    TextDataFormat.Text);
            }
            catch
            {
                MessageBox.Show(
                    "Error accessing the clipboard, please try again.",
                    Name, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }
        }
    }
}
