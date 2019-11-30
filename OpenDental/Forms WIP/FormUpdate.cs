using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormUpdate : FormBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormUpdate"/> class.
        /// </summary>
        public FormUpdate() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormUpdate_Load(object sender, EventArgs e)
        {
            // TODO: Make sure update history is updated correctly. Where does this happen??

            var updateHistory = UpdateHistories.GetForVersion(Application.ProductVersion);

            versionLabel.Text =
                updateHistory == null ?
                    string.Format(Translation.Language.UsingVersion, Application.ProductVersion) :
                    string.Format(Translation.Language.UsingVersionSince, Application.ProductVersion, updateHistory.DateTimeUpdated);

            if (Security.IsAuthorized(Permissions.Setup, true))
            {
                checkButton.Enabled = true;
                setupButton.Visible = true;
            }
            else
            {
                logTextBox.Text = Translation.Language.UpdateNotAuthorized;
            }
        }

        /// <summary>
        /// Opens the form to configure the update preferences.
        /// </summary>
        void SetupButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup)) return;

            using (var formUpdateSetup = new FormUpdateSetup())
            {
                formUpdateSetup.ShowDialog();
            }
        }

        /// <summary>
        /// Checks for updates.
        /// </summary>
        async void CheckButton_Click(object sender, EventArgs e)
        {
            try
            {
                checkButton.Enabled = false;

                if (ReplicationServers.ServerIsBlocked())
                {
                    MessageBox.Show(
                        Translation.Language.UpdatesNotAllowedOnReplicationServer,
                        Translation.Language.Update,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                await CheckForUpdates();
            }
            finally
            {
                checkButton.Enabled = true;
            }
        }

        /// <summary>
        /// Builds a dictionary of date to submit using a POST request when checking for
        /// updates. The update server can use this information to determine whether the clinic is
        /// eligible for updates.
        /// </summary>
        /// <returns></returns>
        static FormUrlEncodedContent GetPostContent()
        {
            var programList = Program.All.Where(x => x.Enabled && !string.IsNullOrWhiteSpace(x.TypeName)).Select(x => x.TypeName).ToList();

            var postContent = new Dictionary<string, string>
            {
                { "product",            "OpenDental" },
                { "productVersion",     Application.ProductVersion },
                { "registrationKey",    Preference.GetString(PreferenceName.RegistrationKey) }
            };

            // Add the list of all enables programs.
            int programId = 0;
            foreach (var program in programList)
            {
                postContent.Add($"program[{programId}]", program);
                programId++;
            }

            return new FormUrlEncodedContent(postContent);
        }

        /// <summary>
        /// Clears the log textbox.
        /// </summary>
        void LogClear() => logTextBox.Text = "";

        /// <summary>
        /// Appends the specified message to the log textbox.
        /// </summary>
        /// <param name="logMessage">The message to append to the log.</param>
        void Log(string logMessage)
        {
            logTextBox.Text = logTextBox.Text + logMessage + "\r\n";
            logTextBox.SelectionStart = logTextBox.Text.Length;
        }

        /// <summary>
        /// Checks whether there are updates available.
        /// </summary>
        /// <returns></returns>
        async System.Threading.Tasks.Task CheckForUpdates()
        {
            Cursor = Cursors.WaitCursor;

            Application.DoEvents();

            try
            {
                LogClear();
                Log(Translation.Language.UpdateAttemptingToConnectWebService);

                WebProxy webProxy = null;
                var webProxyAddress = Preference.GetString(PreferenceName.UpdateWebProxyAddress);
                if (!string.IsNullOrEmpty(webProxyAddress))
                {
                    webProxy = new WebProxy(webProxyAddress);
                    var webProxyUserName = Preference.GetString(PreferenceName.UpdateWebProxyUserName);
                    if (!string.IsNullOrEmpty(webProxyUserName))
                    {
                        webProxy.Credentials =
                            new NetworkCredential(
                                webProxyUserName,
                                Preference.GetString(PreferenceName.UpdateWebProxyPassword));
                    }
                }

                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = webProxy
                };

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    httpClient.BaseAddress = new Uri(Preference.GetString(PreferenceName.UpdateServerAddress));

                    var result = await httpClient.PostAsync("/check", GetPostContent());

                    Log(Translation.Language.ConnectionSuccessful);

                    Cursor = Cursors.Default;

                    await ParseResponse(result);
                }
            }
            catch (Exception ex)
            {
                ErrorCheckingForUpdates(ex);
            }
        }

        /// <summary>
        /// Reports a error that occurred while checking for updates.
        /// </summary>
        /// <param name="exception"></param>
        void ErrorCheckingForUpdates(Exception exception)
        {
            Cursor = Cursors.Default;

            string friendlyMessage = Translation.Language.ErrorCheckingForUpdates;

            FormFriendlyException.Show(friendlyMessage, exception);

            Log(friendlyMessage);
        }

        /// <summary>
        /// Parse the response from the update server.
        /// </summary>
        /// <param name="responseMessage">The response.</param>
        async System.Threading.Tasks.Task ParseResponse(HttpResponseMessage responseMessage)
        {
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var updateFileUri = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(updateFileUri))
                    {
                        Log(Translation.Language.UpdateAvailable);

                        var result = 
                            MessageBox.Show(
                                Translation.Language.UpdateAvailableInstallNow,
                                Translation.Language.Update, 
                                MessageBoxButtons.YesNo, 
                                MessageBoxIcon.Question);

                        if (result == DialogResult.No) return;

                        result = 
                            MessageBox.Show(
                                Translation.Language.StopProgramOnAllComputersBeforeUpdating,
                                Translation.Language.Update, 
                                MessageBoxButtons.OKCancel, 
                                MessageBoxIcon.Warning);

                        if (result == DialogResult.Cancel) return;

                        DownloadAndInstallUpdate(updateFileUri);
                    }
                }
                catch (Exception ex)
                {
                    ErrorCheckingForUpdates(ex);
                }
            }
            if (responseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                Log(Translation.Language.ThereAreNoUpdatesAvailable);

                MessageBox.Show(
                    Translation.Language.ThereAreNoUpdatesAvailable,
                    Translation.Language.Update,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                Log(Translation.Language.ErrorCheckingForUpdates);
            }
        }

        /// <summary>
        /// Downloads and installs a update.
        /// </summary>
        /// <param name="updateFileUri">The uri of the update file.</param>
        void DownloadAndInstallUpdate(string updateFileUri)
        {
            var updateFileName = Path.GetTempFileName();

            var i = updateFileUri.LastIndexOf('.');
            if (i != -1)
            {
                updateFileName += updateFileUri.Substring(i);
            }
            else
            {
                updateFileName += ".exe";
            }

            PrefL.DownloadInstallPatchFromURI(updateFileUri, updateFileName, true, false, "");
        }

        /// <summary>
        /// Opens the version history form.
        /// </summary>
        void PreviousVersionsButton_Click(object sender, EventArgs e)
        {
            using (var formPreviousVersions = new FormPreviousVersions())
            {
                formPreviousVersions.ShowDialog();
            }
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        void CloseButton_Click(object sender, EventArgs e) => Close();
    }
}