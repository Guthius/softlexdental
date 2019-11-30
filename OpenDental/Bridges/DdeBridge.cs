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
using System.Linq;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    /// <summary>
    /// Bridge using DDE (Dynamic Data Exchange) for communication.
    /// </summary>
    public abstract class DdeBridge : Bridge
    {
        private static readonly BridgePreference[] preferences =
        {
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
        /// Initializes a new instance of the <see cref="DdeBridge"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="url">The URL of the external program.</param>
        /// <param name="preferences">The preferences used by the bridge.</param>
        public DdeBridge(string name, string description, string url, params BridgePreference[] preferences) : 
            base(name, description, url,
                preferences != null ?
                    DdeBridge.preferences.Concat(preferences).ToArray() :
                    DdeBridge.preferences)
        {
        }

        /// <summary>
        ///     <para>
        ///         Sends the specified <paramref name="patient"/> data to the remote program
        ///         using DDE (Dynamic Data Exchange).
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient details.</param>
        public sealed override void Send(long programId, Patient patient)
        {
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
                    RunProgram(programId, patient);
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
                if (RunProgram(programId, patient))
                {
                    using (var context = new DdeContext())
                    using (var client = CreateClient(context, programId, patient))
                    {
                        client.Connect();

                        SendCommands(client, programId, patient);
                    }
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

        /// <summary>
        /// Starts the program.
        /// </summary>
        /// <param name="programId">The ID of the program</param>
        /// <param name="patient">The patient.</param>
        /// <returns>True if the program is running; otherwise, false.</returns>
        protected abstract bool RunProgram(long programId, Patient patient);

        /// <summary>
        /// Creates a DDE client using the specified context.
        /// </summary>
        /// <param name="context">The DDE context.</param>
        /// <param name="programId">The ID of the program</param>
        /// <param name="patient">The patient.</param>
        /// <returns>A DDE client.</returns>
        protected abstract DdeClient CreateClient(DdeContext context, long programId, Patient patient);

        /// <summary>
        /// Sends the required DDE commands to the specified <paramref name="client"/>.
        /// </summary>
        /// <param name="client">The DDE client.</param>
        /// <param name="programId">The ID of the program</param>
        /// <param name="patient">The patient.</param>
        protected abstract void SendCommands(DdeClient client, long programId, Patient patient);
    }
}
