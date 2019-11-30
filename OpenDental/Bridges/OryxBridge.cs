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
using Newtonsoft.Json;
using OpenDentBusiness;
using OpenDentBusiness.Bridges;
using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class OryxBridge : Bridge
    {
        private static BridgePreference[] preferences =
        {
            BridgePreference.Custom("client_url", "Client URL", BridgePreferenceType.Url)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="OryxBridge"/> class.
        /// </summary>
        public OryxBridge() : base("Oryx", "", preferences)
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
            var clientUrl = ProgramPreference.GetString(programId, "client_url");
            if (string.IsNullOrEmpty(clientUrl))
            {
                MessageBox.Show(
                    "You must configure a client URL for Oryx in Program Links.", 
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (!clientUrl.StartsWith("http")) clientUrl = "https://" + clientUrl;

            var username = UserPreference.GetString(Security.CurrentUser.Id, "oryx.username");
            var password = UserPreference.GetString(Security.CurrentUser.Id, "oryx.password");

            // User hasn't entered credentials yet. Launch the office's Oryx page where the user can then log in.
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
            {
                Process.Start(clientUrl);

                return;
            }

            // Decrypt the password of the user.
            if (!Encryption.TryDecrypt(password, out var passwordPlain))
            {
                MessageBox.Show(
                    "Unable to decrypt password.",
                    Name, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            var contentJson = JsonConvert.SerializeObject(new
            {
                username,
                password = passwordPlain,
                patientId = (patient != null ? patient.PatNum.ToString() : ""),
            });

            try
            {
                string apiUrl = clientUrl.TrimEnd('/') + "/api/auth/opendental/v1/login";
                using (var webClient = new WebClient())
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

                    var responseData = webClient.UploadString(apiUrl, "POST", contentJson);
                    var response = new
                    {
                        success = false,
                        redirectUrl = "",
                    };

                    response = JsonConvert.DeserializeAnonymousType(responseData, response);
                    if (!response.success)
                    {
                        MessageBox.Show(
                            "Invalid username or password",
                            Name,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        return;
                    }

                    Process.Start(response.redirectUrl);
                }
            }
            catch (Exception exception)
            {
                FormFriendlyException.Show(
                    "Unable to launch Oryx.", exception);
            }
        }

        /// <summary>
        /// Shows a form where the user can enter their username and password.
        /// </summary>
        public static void MenuItemUserSettingsClick(object sender, EventArgs e)
        {
            // TODO: Why is this here?

            var formOryxUserSettings = new FormOryxUserSettings();
            formOryxUserSettings.Show();
        }
    }
}
