using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace OpenDental
{
    /// <summary>
    /// This form shows all of the security log entries for one fKey item. So far this only applies to a single appointment or a single procedure code.
    /// </summary>
    public partial class FormAuditOneType : FormBase
    {
        readonly long patientNum;
        readonly List<string> permTypes;
        readonly long foreignKey;
        SecurityLog[] securityLogList;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormAuditOneType"/> class.
        /// </summary>
        /// <param name="patientNum"></param>
        /// <param name="permTypes"></param>
        /// <param name="title"></param>
        /// <param name="foreignKey"></param>
        public FormAuditOneType(long patientNum, List<string> permTypes, string title, long foreignKey)
        {
            InitializeComponent();

            Text = title;

            this.patientNum = patientNum;
            this.permTypes = new List<string>(permTypes);
            this.foreignKey = foreignKey;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormAuditOneType_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Preference.GetString(PreferenceName.ArchiveServerName)))
            {
                includeArchivedCheckBox.Enabled = false;
            }

            LoadSecurityLogs();
        }

        /// <summary>
        /// Loads the security logs and populates the grid.
        /// </summary>
        void LoadSecurityLogs()
        {
            try
            {
                securityLogList = SecurityLogs.Refresh(patientNum, permTypes, foreignKey, includeArchivedCheckBox.Checked);
            }
            catch (Exception ex)
            {
                FormFriendlyException.Show(
                    Translation.Language.ProblemRefreshingAuditTrail, ex);

                securityLogList = new SecurityLog[0];
            }

            logGrid.BeginUpdate();
            logGrid.Columns.Clear();
            logGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDateTime, 120));
            logGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnUser, 70));
            logGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnPermission, 170));
            logGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnLogText, 510));
            logGrid.Rows.Clear();

            foreach (var securityLog in securityLogList)
            {
                var row = new ODGridRow();
                row.Cells.Add(securityLog.LogDateTime.ToShortDateString() + " " + securityLog.LogDateTime.ToShortTimeString());

                var user = User.GetById(securityLog.UserNum);
                if (user == null)
                {
                    row.Cells.Add(Translation.Language.UserUnknown);
                }
                else
                {
                    row.Cells.Add(user.UserName);
                }

                row.Cells.Add(securityLog.PermType.ToString());
                row.Cells.Add(securityLog.LogText);

                logGrid.Rows.Add(row);
            }
            logGrid.EndUpdate();
            logGrid.ScrollToEnd();
        }

        /// <summary>
        /// Reload the security logs when the archive checkbox state changes.
        /// </summary>
        void includeArchivedCheckBox_CheckedChanged(object sender, EventArgs e) => LoadSecurityLogs();
    }
}