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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class OwandyBridge : Bridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.ProgramPath,
            BridgePreference.CommandLineArguments
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="OwandyBridge"/> class.
        /// </summary>
        public OwandyBridge() : base(
            "QuickVision", 
            "QuickVision imaging software is especially designed for dental surgeries. It includes " +
            "a patient database, an imaging module and a dental diagram. It can be used with " +
            "digital x-ray equipment as a hub, centralizing all patient images. QuickVision " +
            "performs all routine dental imaging functions: acquisition, visualization, editing, " +
            "measurement, zoom, and annotation, as well as data and image sharing.", 
            "https://www.owandy.com/", 
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
            const int WM_SETTEXT = 0x000C;

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

            var commandLineArguments = ProgramPreference.GetString(programId, ProgramPreferenceName.CommandLineArguments);

            try
            {
                Process.Start(programPath, commandLineArguments);
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (patient != null)
            {
                try
                {
                    var windowHandle = FindWindow("MjLinkWndClass", null);

                    if (IsWindow(windowHandle))
                    {
                        var patientId = GetPatientId(programId, patient);

                        SendMessage(
                            windowHandle,
                            WM_SETTEXT, 0,
                            "/P:" + patientId + "," + patient.LName + "," + patient.FName);
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

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, string lParam);
    }
}
