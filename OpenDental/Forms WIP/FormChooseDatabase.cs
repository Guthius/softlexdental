using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormChooseDatabase : FormBaseDialog
    {
        public CentralConnection CentralConnectionCur = new CentralConnection();
        public List<string> ListAdminCompNames = new List<string>();

        /// <summary>
        /// Gets or sets the computer name.
        /// </summary>
        public string ComputerName
        {
            get => computerNameComboBox.Text;
            set
            {
                computerNameComboBox.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string Database
        {
            get => databaseComboBox.Text;
            set
            {
                databaseComboBox.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the user.
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
        /// Gets or sets a value indicating whether to hide this window on startup.
        /// </summary>
        public bool NoShow
        {
            get => checkNoShow.Checked;
            set
            {
                checkNoShow.Checked = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormChooseDatabase"/> class.
        /// </summary>
        public FormChooseDatabase()
        {
            InitializeComponent();

            CentralConnections.GetChooseDatabaseConnectionSettings(
                out CentralConnectionCur,
                out bool noShow);

            NoShow = noShow;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void ChooseDatabaseView_Load(object sender, EventArgs e)
        {
            ComputerName = CentralConnectionCur.ServerName;
            Database = CentralConnectionCur.DatabaseName;
            User = CentralConnectionCur.MySqlUser;
            Password = CentralConnectionCur.MySqlPassword;
        }

        /// <summary>
        /// Updates the connection details.
        /// </summary>
        void UpdateConnection()
        {
            CentralConnectionCur.ServerName = ComputerName;
            CentralConnectionCur.DatabaseName = Database;
            CentralConnectionCur.MySqlUser = User;
            CentralConnectionCur.MySqlPassword = Password;
        }

        /// <summary>
        /// Loads the list of databases.
        /// </summary>
        void DatabaseComboBox_DropDown(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            UpdateConnection();

            databaseComboBox.Items.Clear();

            var databases = CentralConnections.GetDatabases(CentralConnectionCur);
            foreach (var database in databases)
            {
                databaseComboBox.Items.Add(database);
            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Try to connect with the current settings. If connection is succesful close the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            UpdateConnection();
            try
            {
                CentralConnections.TryToConnect(CentralConnectionCur, NoShow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}