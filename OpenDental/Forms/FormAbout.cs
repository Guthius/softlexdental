using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
            string softwareName = Preferences.GetString(PrefName.SoftwareName);

            labelVersion.Text = "Version: " + Application.ProductVersion;

            var updateHistory = UpdateHistories.GetForVersion(Application.ProductVersion);
            if (updateHistory != null)
            {
                labelVersion.Text += "  Since: " + updateHistory.DateTimeUpdated.ToShortDateString();
            }

            // Keeps the trailing year up to date
            labelCopyright.Text = softwareName + " Copyright 2003-" + DateTime.Now.ToString("yyyy") + ", Jordan S. Sparks, D.M.D.";
            labelMySQLCopyright.Text = "MySQL - Copyright 1995-" + DateTime.Now.ToString("yyyy") + ", www.mysql.com";

            List<string> serviceList = Computers.GetServiceInfo();
            labelName.Text = serviceList[2].ToString();
            labelService.Text = serviceList[0].ToString();
            labelMySqlVersion.Text = serviceList[3].ToString();
            labelServComment.Text = serviceList[1].ToString();
            labelMachineName.Text = Environment.MachineName.ToUpper();
        }

        /// <summary>
        /// Generate and display diagnostic information.
        /// </summary>
        void diagnosticsButton_Click(object sender, EventArgs e)
        {
            BugSubmission.SubmissionInfo subInfo = new BugSubmission(new Exception()).Info;
            StringBuilder strBuilder = new StringBuilder();

            foreach (FieldInfo field in subInfo.GetType().GetFields())
            {
                object value = field.GetValue(subInfo);
                if (value.In(null, ""))
                {
                    continue;
                }

                if (value is Dictionary<PrefName, string>)
                {
                    Dictionary<PrefName, string> dictPrefValues = value as Dictionary<PrefName, string>;
                    if (dictPrefValues.Keys.Count > 0)
                    {
                        strBuilder.AppendLine(field.Name + ":");
                        dictPrefValues.ToList().ForEach(x => strBuilder.AppendLine("  " + x.Key.ToString() + ": " + x.Value));
                        strBuilder.AppendLine("-------------");
                    }
                }
                else if (value is List<string>)
                {
                    List<string> enabledPlugins = value as List<string>;
                    if (enabledPlugins.Count > 0)
                    {
                        strBuilder.AppendLine(field.Name + ":");
                        enabledPlugins.ForEach(x => strBuilder.AppendLine("  " + x));
                        strBuilder.AppendLine("-------------");
                    }
                }
                else if (value is bool)
                {
                    strBuilder.AppendLine(field.Name + ": " + (((bool)value) == true ? "true" : "false"));
                }
                else
                {
                    strBuilder.AppendLine(field.Name + ": " + value);
                }
            }

            using (var msgBoxCopyPaste = new MsgBoxCopyPaste(strBuilder.ToString()))
            {
                msgBoxCopyPaste.Text = "Diagnostics";
                msgBoxCopyPaste.ShowDialog();
            }
        }

        /// <summary>
        /// Displays all licenses.
        /// </summary>
        void licensesButton_Click(object sender, EventArgs e)
        {
            using (var formLicense = new FormLicense())
            {
                formLicense.ShowDialog(this);
            }
        }
    }
}
