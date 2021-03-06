using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAudit : FormBase
    {
        private readonly List<string> permissionNames = new List<string>();
        private readonly long? currentPatientId;
        private long patientId;
        private List<User> users;
        private bool headingPrinted;
        private int pagesPrinted;
        private int headingPrintHeight;

        public FormAudit(long? patientId = null)
        {
            InitializeComponent();

            currentPatientId = patientId;
        }

        private void FormAudit_Load(object sender, EventArgs e)
        {
            textDateFrom.Text = DateTime.Today.AddDays(-10).ToShortDateString();
            textDateTo.Text = DateTime.Today.ToShortDateString();

            //TODO: FIx me

            //var permissions = (string[])Enum.GetValues(typeof(Permissions));
            //foreach (var permission in permissions)
            //{
            //    if (GroupPermissions.HasAuditTrail(permission))
            //    {
            //        permissionNames.Add(permission.ToString());
            //    }
            //}

            permissionNames.Sort();

            permissionComboBox.Items.Add("All");
            foreach (var permissionName in permissionNames)
            {
                permissionComboBox.Items.Add(permissionName);
            }
            permissionComboBox.SelectedIndex = 0;

            users = User.All();
            userComboBox.Items.Add("All");
            foreach (var user in users)
            {
                userComboBox.Items.Add(user.UserName);
            }
            userComboBox.SelectedIndex = 0;

            rowsTextBox.Text = Preference.GetString(PreferenceName.AuditTrailEntriesDisplayed);

            CurrentButton_Click(this, EventArgs.Empty);
        }

        private void UserComboBox_SelectionChangeCommitted(object sender, EventArgs e) => LoadAuditLog();

        private void PermissionComboBox_SelectionChangeCommitted(object sender, EventArgs e) => LoadAuditLog();

        private void CurrentButton_Click(object sender, EventArgs e)
        {
            var patientName = "";

            if (currentPatientId.HasValue)
            {
                patientId = currentPatientId.Value;
                if (patientId > 0)
                {
                    patientName = Patients.GetLim(patientId).GetNameLF();
                }
            }

            patientTextBox.Text = patientName;

            LoadAuditLog();
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            using (var formPatientSelect = new FormPatientSelect())
            {
                if (formPatientSelect.ShowDialog() == DialogResult.OK)
                {
                    patientId = formPatientSelect.SelectedPatNum;

                    patientTextBox.Text = Patients.GetLim(patientId).GetNameLF();

                    LoadAuditLog();
                }
            }
        }

        private void AllButton_Click(object sender, EventArgs e)
        {
            patientId = 0;
            patientTextBox.Text = "";
            LoadAuditLog();
        }

        private void LoadAuditLog()
        {
            if (rowsTextBox.errorProvider1.GetError(rowsTextBox) != "")
            {
                MessageBox.Show(
                    "Please fix data entry errors first.",
                    "Audit Trail", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            long userId = 0;
            if (userComboBox.SelectedIndex > 0)
            {
                userId = users[userComboBox.SelectedIndex - 1].Id;
            }


            List<SecurityLog> logList = null;
            DateTime datePreviousFrom = DateTime.Parse(textDateEditedFrom.Text);
            DateTime datePreviousTo = DateTime.Today;

            if (textDateEditedTo.Text != "")
            {
                datePreviousTo = DateTime.Parse(textDateEditedTo.Text);
            }
            try
            {
                if (permissionComboBox.SelectedIndex == 0)
                {
                    logList = 
                        SecurityLog.Find(
                            userId,
                            patientId, 
                            DateTime.Parse(textDateFrom.Text),
                            DateTime.Parse(textDateTo.Text),
                            "",
                            datePreviousFrom, 
                            datePreviousTo, 
                            int.Parse(rowsTextBox.Text));
                }
                else
                {
                    logList = 
                        SecurityLog.Find(
                            patientId,
                            userId,
                            DateTime.Parse(textDateFrom.Text),
                            DateTime.Parse(textDateTo.Text),
                            (string)permissionComboBox.SelectedItem, 
                            datePreviousFrom, 
                            datePreviousTo,
                            int.Parse(rowsTextBox.Text));
                }
            }
            catch (Exception exception)
            {
                FormFriendlyException.Show(
                    "There was a problem refreshing the Audit Trail with the current filters.", exception);

                logList = new List<SecurityLog>();
            }

            grid.BeginUpdate();
            grid.Columns.Clear();
            grid.Columns.Add(new ODGridColumn("Date", 70, sortingStrategy: ODGridSortingStrategy.DateParse));
            grid.Columns.Add(new ODGridColumn("Time", 60, sortingStrategy: ODGridSortingStrategy.DateParse));
            grid.Columns.Add(new ODGridColumn("Patient", 100));
            grid.Columns.Add(new ODGridColumn("User", 70));
            grid.Columns.Add(new ODGridColumn("Permission", 190));
            grid.Columns.Add(new ODGridColumn("Computer", 70));
            grid.Columns.Add(new ODGridColumn("Log Text", 279));
            grid.Columns.Add(new ODGridColumn("Last Edit", 100));
            grid.Rows.Clear();

            foreach (var securityLog in logList)
            {
                var row = new ODGridRow();
                row.Cells.Add(securityLog.LogDate.ToShortDateString());
                row.Cells.Add(securityLog.LogDate.ToShortTimeString());
                row.Cells.Add(""); // TODO: Patient Name
                row.Cells.Add(User.GetById(securityLog.UserId)?.UserName ?? "Unknown");
                row.Cells.Add(securityLog.Event);
                row.Cells.Add(securityLog.ComputerName);

                if (securityLog.Event != Permissions.UserQuery)
                {
                    row.Cells.Add(securityLog.LogMessage);
                }
                else
                {
                    // Only display the first snipet of very long queries. User can double click to view.
                    row.Cells.Add(securityLog.LogMessage.Left(200, true));
                    row.Tag = (Action)(() =>
                    {
                        var msgBoxCopyPaste = new MsgBoxCopyPaste(securityLog.LogMessage);

                        msgBoxCopyPaste.Show();
                    });
                }

                row.Cells.Add(securityLog.ExternalDate.HasValue ? securityLog.ExternalDate.ToString() : "");
                
                // Mark the row with a red background if the hashes don't match
                if (securityLog.Hash != SecurityLogHash.GetHashString(securityLog))
                {
                    row.ColorText = Color.Red; 
                }

                grid.Rows.Add(row);
            }
            grid.EndUpdate();
            grid.ScrollToEnd();
        }

        private void Grid_CellDoubleClick(object sender, ODGridClickEventArgs e) => (grid.Rows[e.Row].Tag as Action)?.Invoke();

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            if (textDateFrom.Text == "" || 
                textDateTo.Text == "" || 
                textDateFrom.errorProvider1.GetError(textDateFrom) != "" ||
                textDateTo.errorProvider1.GetError(textDateTo) != "" ||
                rowsTextBox.errorProvider1.GetError(rowsTextBox) != "" || 
                !textDateEditedFrom.IsValid || 
                !textDateEditedTo.IsValid)
            {
                MessageBox.Show("Please fix data entry errors first.");

                return;
            }

            LoadAuditLog();
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            pagesPrinted = 0;

            headingPrinted = false;

            PrinterL.TryPrintOrDebugClassicPreview(
                PrintLogPage, 
                "Audit trail printed", 
                printoutOrientation: PrintoutOrientation.Landscape);
        }

        private void PrintLogPage(object sender, PrintPageEventArgs e)
        {
            Rectangle bounds = e.MarginBounds;
            //new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
            Graphics g = e.Graphics;
            string text;
            Font headingFont = new Font("Arial", 13, FontStyle.Bold);
            Font subHeadingFont = new Font("Arial", 10, FontStyle.Bold);
            int yPos = bounds.Top;
            int center = bounds.X + bounds.Width / 2;

            if (!headingPrinted)
            {
                text = "Audit Trail";

                g.DrawString(text, headingFont, Brushes.Black, center - g.MeasureString(text, headingFont).Width / 2, yPos);
                yPos += (int)g.MeasureString(text, headingFont).Height;
                text = textDateFrom.Text + " to " + textDateTo.Text;
                g.DrawString(text, subHeadingFont, Brushes.Black, center - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                yPos += 20;
                headingPrinted = true;
                headingPrintHeight = yPos;
            }


            yPos = grid.PrintPage(g, pagesPrinted, bounds, headingPrintHeight);
            pagesPrinted++;
            if (yPos == -1)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
                pagesPrinted = 0;
            }
            g.Dispose();
        }
    }
}
