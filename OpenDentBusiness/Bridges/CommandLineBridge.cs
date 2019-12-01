/**
 * Copyright (C) 2019 Dental Stars SRL
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
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OpenDentBusiness.Bridges
{
    /// <summary>
    ///     <para>
    ///         Bridge to transfer patient information to program installed on the local system.
    ///     </para>
    ///     <para>
    ///         <b>Note:</b> Subclasses must implement the 
    ///         <see cref="PrepareToRun(long, Patient, out string)"/> method. In this method
    ///         the command line arguments that will be passed to the program can be set and any
    ///         other operations required before the program is started can be performed (e.g. 
    ///         create a file with patient information).
    ///     </para>
    /// </summary>
    public abstract class CommandLineBridge : Bridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Define("program_path", "Executable Path", BridgePreferenceType.File),
            BridgePreference.Define("cmd_line_args", "Command Line Arguments", BridgePreferenceType.String),
        };

        /// <summary>
        ///     <para>
        ///         Gets or sets a value indicating whether a patient is required.
        ///     </para>
        ///     <para>
        ///         If this is true, the <see cref="Patient"/> passed into 
        ///         <see cref="Send(long, Patient)"/> cannot be null.
        ///     </para>
        /// </summary>
        protected bool RequirePatient { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineBridge"/> class.
        /// </summary>
        /// <param name="name">The name of the external progarm or service.</param>
        /// <param name="description">A description of the bridge.</param>
        /// <param name="url">The URL of the external program.</param>
        /// <param name="preferences">The preferences used by the bridge.</param>
        public CommandLineBridge(string name, string description, string url, params BridgePreference[] preferences) :
            base(name, description, url,
                preferences != null ?
                    CommandLineBridge.preferences.Concat(preferences).ToArray() :
                    CommandLineBridge.preferences)
        {
        }

        /// <summary>
        ///     <para>
        ///         Gets or sets a value indicating whether we should wait until the 
        ///         bridge process exits after we start it. This essentially puts us in a sleep
        ///         state until the bridged application closes. Only set this to true if the
        ///         bridged program is extremely resource intensive and needs as much system
        ///         resources as possible.
        ///     </para>
        /// </summary>
        protected bool WaitForExit { get; set; } = false;

        /// <summary>
        ///     <para>
        ///         Sends the specified <paramref name="patient"/> data to the remote program or 
        ///         service.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient details.</param>
        public sealed override void Send(long programId, Patient patient)
        {
            var programPath = GetProgramPath(programId);
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

            var programArguments = GetCommandLineArguments(programId);

            if (patient == null)
            {
                if (RequirePatient)
                {
                    MessageBox.Show(
                        "Please select a patient first.",
                        Name,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                try
                {
                    var process = Process.Start(programPath, programArguments);
                    if (WaitForExit)
                    {
                        process.WaitForExit();
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

                return;
            }

            string arguments;
            try
            {
                if (!PrepareToRun(programId, patient, out arguments))
                {
                    return;
                }
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

            try
            {
                if (!string.IsNullOrEmpty(programArguments))
                {
                    arguments = programArguments + " " + arguments;
                }

                Process.Start(programPath, arguments);
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

        /// <summary>
        ///     <para>
        ///         Prepares the local system enviroment for running the program. Most bridges
        ///         will only fill the <paramref name="arguments"/> parameter with the appropriate 
        ///         data. Some bridges will perform more complex actions such as generating files,
        ///         contacting remote services, etc...
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient.</param>
        /// <param name="arguments">The command line arguments to pass to the program.</param>
        /// <returns>
        ///     True if the preparation was successful and the program can be started; otherwise, false.
        /// </returns>
        protected abstract bool PrepareToRun(long programId, Patient patient, out string arguments);

        /// <summary>
        /// Gets the path of the program with the specified ID.
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <returns>The path of the program.</returns>
        protected static string GetProgramPath(long programId) =>
            ProgramPreference.GetString(programId, "program_path");

        /// <summary>
        /// Gets the command line arguments for the program with the specified ID.
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <returns>The command line arguments for the program.</returns>
        protected static string GetCommandLineArguments(long programId) =>
            ProgramPreference.GetString(programId, "cmd_line_args");
    }
}
