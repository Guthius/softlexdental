using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental
{
    public partial class FormAbout : FormBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormAbout"/> class.
        /// </summary>
        public FormAbout()
        {
            InitializeComponent();
            Lan.F(this);
        }



        void FormAbout_Load(object sender, System.EventArgs e)
        {
            string softwareName = PrefC.GetString(PrefName.SoftwareName);

            labelVersion.Text = Lan.g(this, "Version:") + " " + Application.ProductVersion;
            UpdateHistory updateHistory = UpdateHistories.GetForVersion(Application.ProductVersion);
            if (updateHistory != null)
            {
                labelVersion.Text += "  " + Lan.g(this, "Since:") + " " + updateHistory.DateTimeUpdated.ToShortDateString();
            }

            //keeps the trailing year up to date
            labelCopyright.Text = softwareName + " " + Lan.g(this, "Copyright 2003-") + DateTime.Now.ToString("yyyy") + ", Jordan S. Sparks, D.M.D.";
            labelMySQLCopyright.Text = Lan.g(this, "MySQL - Copyright 1995-") + DateTime.Now.ToString("yyyy") + Lan.g(this, ", www.mysql.com");

            //Database Server----------------------------------------------------------		
            List<string> serviceList = Computers.GetServiceInfo();
            labelName.Text = serviceList[2].ToString();//MiscData.GetODServer();//server name
            labelService.Text = serviceList[0].ToString();//service name
            labelMySqlVersion.Text = serviceList[3].ToString();//service version
            labelServComment.Text = serviceList[1].ToString();//service comment
            labelMachineName.Text = Environment.MachineName.ToUpper();//current client or remote application machine name
        }

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
                {//DictPrefValues
                    Dictionary<PrefName, string> dictPrefValues = value as Dictionary<PrefName, string>;
                    if (dictPrefValues.Keys.Count > 0)
                    {
                        strBuilder.AppendLine(field.Name + ":");
                        dictPrefValues.ToList().ForEach(x => strBuilder.AppendLine("  " + x.Key.ToString() + ": " + x.Value));
                        strBuilder.AppendLine("-------------");
                    }
                }
                else if (value is List<string>)
                {//EnabledPlugins
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
            MsgBoxCopyPaste msgbox = new MsgBoxCopyPaste(strBuilder.ToString());
            msgbox.Text = Lans.g(this, "Diagnostics");
            msgbox.ShowDialog();
        }

        void licensesButton_Click(object sender, EventArgs e)
        {
            using (var formLicense = new FormLicense())
            {
                formLicense.ShowDialog(this);
            }
        }
    }
}
