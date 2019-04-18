using CodeBase;
using Microsoft.Win32;
using ServiceManager.Properties;
using System;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;

namespace ServiceManager
{
    public partial class FormServiceManage : Form
    {
        public bool HadServiceInstalled = false;

        bool isInstallOnly = false;

        /// <summary>
        /// Gets or sets the service name.
        /// </summary>
        public string ServiceName
        {
            get => serviceNameTextBox.Text.Trim();
            set
            {
                serviceNameTextBox.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the executable path of the service.
        /// </summary>
        public string FullPath
        {
            get => pathTextBox.Text.Trim();
            set
            {
                pathTextBox.Text = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormServiceManage"/> class.
        /// </summary>
        /// <param name="serviceName">The name of the service to manage.</param>
        /// <param name="isInstallOnly">Value indicating whether the only allow the install option.</param>
        public FormServiceManage(string serviceName, bool isInstallOnly)
        {
            InitializeComponent();

            ServiceName = serviceName;
            FullPath = Directory.GetCurrentDirectory();

            this.isInstallOnly = isInstallOnly;
        }

        /// <summary>
        /// Fetch the details of the service and update the state of the controls accordingly.
        /// </summary>
        void FetchServiceInformation()
        {
            var service = ServicesHelper.GetOpenDentServiceByName(serviceNameTextBox.Text);
            if (service != null)
            {
                var registryKey = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Services\" + service.ServiceName);
                if (registryKey == null)
                {
                    return;
                }

                serviceNameTextBox.ReadOnly = true;
                pathTextBox.Text = registryKey.GetValue("ImagePath").ToString().Replace("\"", "");
                pathTextBox.ReadOnly = true;
                browseButton.Enabled = false;
                statusTextBox.Text = Resources.LangStatusInstalled;
                installButton.Enabled = false;
                uninstallButton.Enabled = true;
                
                if (service.Status == ServiceControllerStatus.Running)
                {
                    statusTextBox.Text += ", " + Resources.LangStatusRunning;
                    startButton.Enabled = false;
                    stopButton.Enabled = true;
                }
                else
                {
                    statusTextBox.Text += ", " + Resources.LangStatusStopped;
                    startButton.Enabled = true;
                    stopButton.Enabled = false;
                }
            }
            else
            {
                statusTextBox.Text = Resources.LangStatusNotInstalled;
                serviceNameTextBox.ReadOnly = false;
                pathTextBox.ReadOnly = false;
                installButton.Enabled = true;
                uninstallButton.Enabled = false;
                startButton.Enabled = false;
                stopButton.Enabled = false;
            }

            if (isInstallOnly) uninstallButton.Enabled = false;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormServiceManager_Load(object sender, EventArgs e) => FetchServiceInformation();

        /// <summary>
        /// Open the dialog to browse for a executable file.
        /// </summary>
        void browseButton_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = Resources.LangSelectAService;
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                openFileDialog.Filter = Resources.LangExecutableFiles + " (*.exe)|*.exe";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FullPath = openFileDialog.FileName;
                }
            }
        }

        /// <summary>
        /// Refresh the details of the service.
        /// </summary>
        void refreshButton_Click(object sender, EventArgs e) => FetchServiceInformation();

        /// <summary>
        /// Install the service.
        /// </summary>
        void installButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists(FullPath))
            {
                MessageBox.Show(
                    Resources.LangSelectAValidServicePath,
                    Resources.LangManageService,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // Check whether a valid service name has been specified.
            if (ServiceName.Length < 8 || ServiceName.Substring(0, 8) != "OpenDent")
            {
                MessageBox.Show(
                    Resources.LangServiceNameMustBeginWithOpenDent,
                    Resources.LangManageService,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // Check whether there is a service installed with the specified name or executable.
            if (ServicesHelper.HasService(ServiceName, FullPath))
            {
                MessageBox.Show(
                    Resources.LangServiceNameOrDirectoryInUse,
                    Resources.LangManageService,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // When creating a eConnector or OpenDental service open the form to make the configuration file.
            string fileName = Path.GetFileName(FullPath).ToLower();
            if (fileName == "opendentaleconnector.exe" || fileName == "opendentalservice.exe")
            {
                using (var formWebConfigSettings = new FormWebConfigSettings(FullPath))
                {
                    if (formWebConfigSettings.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                }
            }

            // Install the service.
            try
            {
                ServicesHelper.Install(ServiceName, FullPath, out string standardOutput, out int exitCode);
                if (exitCode != 0)
                {
                    MessageBox.Show(
                        string.Format(Resources.LangInstallationFailedWithExitCode, exitCode) + "\r\n" + standardOutput.Trim(),
                        Resources.LangManageService,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    Resources.LangUnexpectedErrorInstall + "\r\n" + ex.Message,
                    Resources.LangManageService,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            // Check whether the service was installed correctly.
            try
            {
                var service = ServicesHelper.GetServiceByServiceName(ServiceName);
                if (service != null)
                {
                    HadServiceInstalled = true; //We verified that the service was successfully installed
                                                //Try to grant access to "Everyone" so that the service can be stopped and started by all users.
                    try
                    {
                        ServicesHelper.SetSecurityDescriptorToAllowEveryoneToManageService(service);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                           Resources.LangErrorUpdatingPermissions + "\r\n\r\n" + ex.Message,
                           Resources.LangManageService,
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Warning);
                    }
                }
            }
            catch
            {
            }

            FetchServiceInformation();
        }

        /// <summary>
        /// Uninstalls the service and closes the form on success.
        /// </summary>
        void uninstallButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists(FullPath))
            {
                MessageBox.Show(
                    Resources.LangSelectedServiceHasInvalidPath,
                    Resources.LangManageService, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            try
            {
                ServicesHelper.Uninstall(serviceNameTextBox.Text, out string standardOutput, out int exitCode);
                if (exitCode != 0)
                {
                    MessageBox.Show(
                        string.Format(Resources.LangUninstallFailedWithExitCode, exitCode) + "\r\n" + standardOutput.Trim(),
                        Resources.LangManageService,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    Resources.LangUnexpectedErrorUninstall + "\r\n" + ex.Message,
                    Resources.LangManageService,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Starts the selected service.
        /// </summary>
        void startButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                ServicesHelper.Start(serviceNameTextBox.Text, true);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
            Cursor = Cursors.Default;
            FetchServiceInformation();
        }

        /// <summary>
        /// Stops the selected service.
        /// </summary>
        void stopButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                ServicesHelper.Stop(serviceNameTextBox.Text, true);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
            Cursor = Cursors.Default;
            FetchServiceInformation();
        }
    }
}