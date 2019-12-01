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
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class DentalTekBridge : Bridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Define("api_token", "Enter your API Token", BridgePreferenceType.String)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="DentalTekBridge"/> class.
        /// </summary>
        public DentalTekBridge() : base("DentalTek", "", "http://dentalsolutionsllc.com/", preferences)
        {
        }

        /// <summary>
        ///     <para>
        ///         Attempt to send a phone number to place a call using DentalTek.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="phoneNumber">The phone number to call.</param>
        /// <returns>
        ///     False if the call was unsuccessful or if no phone number was passed in.
        /// </returns>
        private static bool PlaceCall(long programId, string phoneNumber)
        {
            var token = ProgramPreference.GetString(programId, "api_token");
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }

            using (var client = new WebClient())
            {
                string response = "";
                try
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Encoding = UnicodeEncoding.UTF8;

                    string request;

                    if (token == "")
                    {
                        string domainUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                        request = "https://extapi.dentaltek.com/v1/pbx/rest/ClickToCall?domainUser=" + domainUser + "&phoneNumber=" + phoneNumber;
                    }
                    else
                    {
                        if (token.ToLower() == "premise")
                        {
                            token = "";

                            string domainUsername = WindowsIdentity.GetCurrent().Name.Replace(@"\", "-");
                            string pipeName = domainUsername + "-6a9631ab-be94-4bbe-8822-be68034f9009";

                            try
                            {
                                using (var namedPipeClientStream = new NamedPipeClientStream(pipeName))
                                using (var streamWriter = new StreamWriter(namedPipeClientStream))
                                using (var streamReader = new StreamReader(namedPipeClientStream))
                                {
                                    namedPipeClientStream.Connect(1000);

                                    streamWriter.WriteLine("!token!");
                                    streamWriter.Flush();

                                    namedPipeClientStream.WaitForPipeDrain();

                                    token = streamReader.ReadLine();
                                }
                            }
                            catch (Exception exception)
                            {
                                MessageBox.Show(
                                    "Error occurred: " + exception.Message + "\r\n" +
                                    "Please login to your Xbeyon/DentalTek Application and try again.",
                                    "DentalTek",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);

                                return false;
                            }

                            request = "https://extapi.dentaltek.com/v1/pbx/rest/ClickToCall?phoneNumber=" + phoneNumber + "&token=" + token + "&premise=true";
                        }
                        else
                        {
                            request = "https://extapi.dentaltek.com/v1/pbx/rest/ClickToCall?phoneNumber=" + phoneNumber + "&token=" + token;
                        }
                    }

                    response = client.DownloadString(request);
                }
                catch (Exception)
                {
                    // Can't think of anything useful to tell them about why the call attempt failed.
                }

                return response.Contains("Success");
            }
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
            if (patient == null)
            {
                MessageBox.Show(
                    "Please select a patient first.", 
                    "DentalTek", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            string phoneNumber = "";

            var contactMethods = new Dictionary<ContactMethod, string>
            {
                { ContactMethod.HmPhone, patient.HmPhone },
                { ContactMethod.WkPhone, patient.WkPhone },
                { ContactMethod.WirelessPh, patient.WirelessPhone }
            };

            if (contactMethods.ContainsKey(patient.PreferContactMethod))
            {
                phoneNumber = contactMethods[patient.PreferContactMethod];
            }
            else
            {
                var phoneNumbers = new List<string>
                {
                    "HmPhone: " + patient.HmPhone,
                    "WkPhone: " + patient.WkPhone,
                    "WirelessPhone: " + patient.WirelessPhone
                };

                using (var inputBox = new InputBox("Please select a phone number", phoneNumbers))
                {
                    inputBox.comboSelection.SelectedIndex = 0;

                    if (inputBox.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    phoneNumber = phoneNumbers[inputBox.comboSelection.SelectedIndex];
                }
            }

            phoneNumber = Tidy(phoneNumber);
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                if (!PlaceCall(programId, phoneNumber))
                {
                    MessageBox.Show(
                        "Unable to place phone call.",
                        "DentalTek",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Strips all non-digit characters from the given phonenumber.
        /// </summary>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns></returns>
        private static string Tidy(string phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var stringBuilder = new StringBuilder(phoneNumber.Length);

                for (int i = 0; i < phoneNumber.Length; i++)
                {
                    if (char.IsDigit(phoneNumber[i]))
                    {
                        stringBuilder.Append(phoneNumber[i]);
                    }
                }

                return stringBuilder.ToString();
            }
            return string.Empty;
        }
    }
}
