using ServiceManager.Properties;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ServiceManager
{
    public partial class FormWebConfigSettings : Form
    {
        string servicePath;

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        public string Server
        {
            get => serverTextBox.Text;
            set
            {
                serverTextBox.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        public string Database
        {
            get => databaseTextBox.Text;
            set
            {
                databaseTextBox.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string User
        {
            get => userTextBox.Text;
            set
            {
                userTextBox.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get => passwordTextBox.Text;
            set
            {
                passwordTextBox.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the username of the low priviledge user.
        /// </summary>
        public string UserLow
        {
            get => userLowTextBox.Text;
            set
            {
                userLowTextBox.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the password of the low priviledge user.
        /// </summary>
        public string PasswordLow
        {
            get => passwordLowTextBox.Text;
            set
            {
                passwordLowTextBox.Text = value;
            }
        }

        /// <summary>
        /// Pass in the file information for the service file that is being installed. 
        /// We will use the file path to determine where to put the config file.
        /// </summary>
        public FormWebConfigSettings(string servicePath)
        {
            InitializeComponent();

            this.servicePath = servicePath;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormWebConfigSettings_Load(object sender, EventArgs e)
        {
            logLevelComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Displays the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        void ShowError(string errorMessage)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show(
                    errorMessage,
                    Resources.LangOpenDentalWebConfigSettings,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Test the connection with the database.
        /// </summary>
        /// <returns>True if the connection was succesful; otherwise, false.</returns>
        bool TestConnection()
        {
            //var dataConnection = new DataConnection();
            //
            //try
            //{
            //    dataConnection.SetDb(
            //        serverTextBox.Text, 
            //        databaseTextBox.Text, 
            //        userTextBox.Text, 
            //        passwordTextBox.Text, 
            //        userLowTextBox.Text, 
            //        passwordLowTextBox.Text);
            //
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    ShowError(Resources.LangErrorConnectingToDatabase + " " + ex.Message);
            //
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// Generates the XML configuration file and close the form.
        /// </summary>
        void acceptButton_Click(object sender, EventArgs e)
        {
            var document = new XmlDocument();

            if (Server == "")
            {
                ShowError(Resources.LangCannotLeaveServerBlank);
                return;
            }

            if (databaseTextBox.Text == "")
            {
                ShowError(Resources.LangCannotLeaveDatabaseBlank);
                return;
            }

            if (userTextBox.Text == "")
            {
                ShowError(Resources.LangCannotLeaveUserBlank);
                return;
            }

            // Check the connection details.
            if (!TestConnection()) return;

            // Determine the name of the configuration file based on the service executable name.
            var fileName =
                Path.GetFileName(servicePath).ToLower() == "opendentalservice.exe" ?
                    "OpenDentalServiceConfig.xml" :
                    "OpenDentalWebConfig.xml";

            // Try to save the configuration file.
            try
            {
                using (var fileStream = File.OpenWrite(fileName))
                {
                    var xmlWriterSettings = new XmlWriterSettings
                    {
                        Indent = true,
                        NewLineChars = "\r\n",
                        OmitXmlDeclaration = true
                    };

                    using (var xmlWriter = XmlWriter.Create(fileStream, xmlWriterSettings))
                    {
                        CDT.Class1.Encrypt(passwordTextBox.Text, out string encryptedPassword);

                        xmlWriter.WriteStartElement("ConnectionSettings");
                        xmlWriter.WriteStartElement("DatabaseConnection");
                        xmlWriter.WriteElementString("ComputerName", Server);
                        xmlWriter.WriteElementString("Database", Database);
                        xmlWriter.WriteElementString("User", User);
                        xmlWriter.WriteElementString("Password", string.IsNullOrEmpty(encryptedPassword) ? Password : ""); // Only write the mysql password in plain text if encryption failed.
                        xmlWriter.WriteElementString("MySQLPassHash", encryptedPassword);
                        xmlWriter.WriteElementString("UserLow", UserLow);
                        xmlWriter.WriteElementString("PasswordLow", PasswordLow);
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteElementString("LogLevelOfApplication", logLevelComboBox.Items[logLevelComboBox.SelectedIndex].ToString());
                        xmlWriter.WriteEndElement();
                        xmlWriter.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(Resources.LangErrorWritingConfigurationFile + " " + ex.Message);

                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}