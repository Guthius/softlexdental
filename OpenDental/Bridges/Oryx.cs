using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using OpenDentBusiness;

namespace OpenDental.Bridges
{
    public class Oryx
    {
        public static class Preferences
        {
            public const string Username = "oryx.username";

            public const string Password = "oryx.password";
        }

        ///<summary>Makes an API call to get an Oryx URL to launch that is specific to the current user and patient.</summary>
        public static void SendData(Program progOryx, Patient pat)
        {
            try
            {
                string clientUrl = OpenDentBusiness.ProgramProperties.GetPropVal(progOryx.Id, ProgramProperties.ClientUrl);
                if (clientUrl == "")
                {//Office has not signed up with Oryx yet, launch a promotional page.
                    string promoUrl = "http://www.opendental.com/resources/redirects/redirectoryx.html";
#if DEBUG
                    promoUrl = "http://www.opendental.com/resources/redirects/redirectoryxdebug.html";
#endif
                    Process.Start(promoUrl);
                    return;
                }
                if (!progOryx.Enabled)
                {
                    MsgBox.Show("Oryx", "Oryx must be enabled in Program Links.");
                    return;
                }
                if (!clientUrl.StartsWith("http"))
                {
                    clientUrl = "https://" + clientUrl;
                }

                var username = UserPreference.GetString(Security.CurrentUser.Id, Preferences.Username);
                var password = UserPreference.GetString(Security.CurrentUser.Id, Preferences.Password);

                if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
                {
                    //User hasn't entered credentials yet. Launch the office's Oryx page where the user can then log in.
                    Process.Start(clientUrl);
                    return;
                }
                string apiUrl = clientUrl.TrimEnd('/') + "/api/auth/opendental/v1/login";
                string passwordPlain;
                if (!Encryption.TryDecrypt(password, out passwordPlain))
                {
                    MsgBox.Show("Oryx", "Unable to decrypt password");
                    return;
                }
                var content = new
                {
                    username = username,
                    password = passwordPlain,
                    patientId = (pat != null ? pat.PatNum.ToString() : ""),
                };
                string contentJson = JsonConvert.SerializeObject(content);
                string responseStr;
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    responseStr = client.UploadString(apiUrl, "POST", contentJson);
                }
                var response = new
                {
                    success = false,
                    redirectUrl = "",
                };
                response = JsonConvert.DeserializeAnonymousType(responseStr, response);
                if (!response.success)
                {
                    MsgBox.Show("Oryx", "Invalid username or password");
                    return;
                }
                Process.Start(response.redirectUrl);
            }
            catch (Exception ex)
            {
                FormFriendlyException.Show(Lans.g("Oryx", "Unable to launch Oryx."), ex);
            }
        }

        ///<summary>Shows a form where the user can enter their username and password.</summary>
        public static void menuItemUserSettingsClick(object sender, EventArgs e)
        {
            FormOryxUserSettings FormOUS = new FormOryxUserSettings();
            FormOUS.Show();
        }

        public class ProgramProperties
        {
            public static string ClientUrl = "Client URL";
            public static string DisableAdvertising = "Disable Advertising";
        }
    }
}