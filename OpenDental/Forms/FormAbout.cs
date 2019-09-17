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
            versionLabel.Text = "Version: " + Application.ProductVersion;

            var updateHistory = UpdateHistories.GetForVersion(Application.ProductVersion);
            if (updateHistory != null)
            {
                versionLabel.Text += "  Since: " + updateHistory.DateTimeUpdated.ToShortDateString();
            }

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
