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
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace OpenDental
{
    public class EvaSoftBridge : Bridge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvaSoftBridge"/> class.
        /// </summary>
        public EvaSoftBridge() : base(
            "EVAsoft",
            "EVAsoft Dental Image Management software meets the imaging demands of today’s digital " +
            "practices. A sophisticated software that is easy to learn and simple to operate, " +
            "EVAsoft has all of the advanced features necessary to deliver rapid, effective " +
            "diagnoses from multiple imaging modalities.", 
            "https://www.imageworkscorporation.com/")
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
            string Tidy(string input) => input.Replace(",", "").Trim();

            if (patient == null)
            {
                MessageBox.Show(
                  "Please select a patient first.",
                  Name,
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Information);

                return;
            }

            var evaSoftInstances = Process.GetProcessesByName("EvaSoft");
            if (evaSoftInstances.Length == 0)
            {
                MessageBox.Show(
                    "EvaSoft is not running. EvaSoft must be running before the bridge will work.",
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            var udpMessage =
                "REQUEST01123581321~~~0.1b~~~pmaddpatient~~~" +
                Tidy(GetPatientId(programId, patient)) + "," +
                Tidy(patient.FName) + "," +
                Tidy(patient.LName) + "," +
                patient.Birthdate.ToString("MM/dd/yyyy") + "," +
                ((patient.Gender == PatientGender.Female) ? "female" : "male") + "," +
                Tidy(patient.Address + " " + patient.Address2) + "," +
                Tidy(patient.City) + "," +
                Tidy(patient.State) + "," +
                Tidy(patient.Zip);

            try
            {
                using (var udpClient = new UdpClient())
                {
                    var udpMessageBytes = Encoding.ASCII.GetBytes(udpMessage);

                    udpClient.Send(
                        udpMessageBytes,
                        udpMessageBytes.Length,
                        "localhost",
                        35678);
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
    }
}
