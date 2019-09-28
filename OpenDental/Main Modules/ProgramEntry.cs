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
using System;
using System.Net;
using System.Windows.Forms;

namespace OpenDental
{
    static class ProgramEntry
    {
        /// <summary>
        /// Main entry point of the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // The default SecurityProtocol is "Ssl3|Tls". We must add Tls12 in order to 
                // support Tls1.2 web reference handshakes, without breaking any web references 
                // using Ssl3 or Tls.
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }
            catch (Exception exception)
            {
                FormFriendlyException.Show(
                    string.Format("A critical error has occurred: {0}", exception.Message), 
                    exception, "&Quit");

                return;
            }

            DataConnection.Configure("localhost", "demo", "root", "softlex");

            var formOpenDental = new FormOpenDental(args);

            Application.ThreadException += (s, e) =>
            {
                UnhandledException(e.Exception);

                formOpenDental.ProcessKillCommand();
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                UnhandledException(e.ExceptionObject as Exception);

                if (e.IsTerminating)
                {
                    formOpenDental.ProcessKillCommand();
                }
            };

            Application.DoEvents();
            Application.AddMessageFilter(new ODGlobalUserActiveHandler());
            Application.Run(formOpenDental);
        }

        /// <summary>
        /// Handles exceptions that haven't been handled anywhere else.
        /// </summary>
        /// <param name="exception">The unhandled exception.</param>
        private static void UnhandledException(Exception exception)
        {
            if (exception != null)
            {
                FormFriendlyException.Show(
                    "Critical Error: " + exception.Message, 
                    exception, 
                    "&Quit");
            }
        }
    }
}
