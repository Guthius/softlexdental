/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAbout : FormBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormAbout"/> class.
        /// </summary>
        public FormAbout() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormAbout_Load(object sender, EventArgs e)
        {
            string softwareName = Preference.GetString(PreferenceName.SoftwareName);

            versionLabel.Text = "Version: " + Application.ProductVersion;

            var updateHistory = UpdateHistories.GetForVersion(Application.ProductVersion);
            if (updateHistory != null)
            {
                versionLabel.Text += "  Since: " + updateHistory.DateTimeUpdated.ToShortDateString();
            }

            copyrightLabel.Text = softwareName + " Copyright 2003-" + DateTime.Now.ToString("yyyy") + ", Jordan S. Sparks, D.M.D.";
            copyrightMySqlLabel.Text = "MySQL - Copyright 1995-" + DateTime.Now.ToString("yyyy") + ", www.mysql.com";

            var serviceList = Computer.GetServiceInfo();
            machineNameLabel.Text = Environment.MachineName.ToUpper();
            serviceNameLabel.Text = serviceList[0].ToString();
            serviceCommentLabel.Text = serviceList[1].ToString();
            serverNameLabel.Text = serviceList[2].ToString();
            serviceVersionLabel.Text = serviceList[3].ToString();
        }

        /// <summary>
        /// Displays all licenses.
        /// </summary>
        void LicensesButton_Click(object sender, EventArgs e)
        {
            using (var formLicense = new FormLicense())
            {
                formLicense.ShowDialog(this);
            }
        }
    }
}