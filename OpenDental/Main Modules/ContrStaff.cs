using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental
{
    public partial class ContrStaff : Module
    {
        public FormTasks formTasks;
        public FormAccounting formAccounting;

        TimeSpan TimeDelta;
        List<SigMessage> _listSigMessages;
        SigElementDef[] sigElementDefUser;
        SigElementDef[] sigElementDefExtras;
        ErrorProvider errorProvider1 = new ErrorProvider();
        SigElementDef[] sigElementDefMessages;
        Employee EmployeeCur;


        FormBilling formBilling;
        FormClaimsSend formClaimsSend;

        FormEtrans834Import FormE834I;
        FormEmailInbox FormEmailInbox;
        FormArManager formArManager;

        long PatCurNum;
        List<Employee> _listEmployees = new List<Employee>();
        List<ClockEventStatus> _listShownTimeClockStatuses = new List<ClockEventStatus>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContrStaff"/> class.
        /// </summary>
        public ContrStaff()
        {
            Logger.Write(LogLevel.Info, "Initializing management module...");

            InitializeComponent();
        }

        /// <summary>
        /// Only gets run on startup.
        /// </summary>
        public void InitializeOnStartup()
        {
            RefreshFullMessages();
        }

        public void ModuleSelected(long patNum)
        {
            PatCurNum = patNum;
            RefreshModuleData(patNum);
            RefreshModuleScreen();

            Plugin.Trigger(this, "ContrStaff_ModuleSelected", patNum);
        }

        public void ModuleUnselected()
        {
            Plugin.Trigger(this, "ContrStaff_ModuleUnselected");
        }

        void RefreshModuleData(long patNum)
        {
            if (Preference.GetBool(PreferenceName.LocalTimeOverridesServerTime))
            {
                TimeDelta = new TimeSpan(0);
            }
            else
            {
                TimeDelta = MiscData.GetNowDateTime() - DateTime.Now;
            }
            CacheManager.Invalidate<Employee>();
        }

        void RefreshModuleScreen()
        {
            if (Preference.GetBool(PreferenceName.LocalTimeOverridesServerTime))
            {
                labelCurrentTime.Text = "Local Time";
            }
            else
            {
                labelCurrentTime.Text = "Server Time";
            }
            textTime.Text = (DateTime.Now + TimeDelta).ToLongTimeString();

            LoadEmployees();
            FillMessageDefs();

            butManage.Enabled = Security.IsAuthorized(Permissions.TimecardsEditAll, true);
            butBreaks.Visible = Preference.GetBool(PreferenceName.ClockEventAllowBreak);

            importInsPlansButton.Visible = true;
            if (Preference.GetBool(PreferenceName.EasyHidePublicHealth))
            {
                importInsPlansButton.Visible = false; // Import Ins Plans button is only visible when Public Health feature is enabled.
            }

            //collectionsButton.Visible = !ProgramProperties.IsAdvertisingDisabled(ProgramName.Transworld);
        }

        #region Daily

        /// <summary>
        /// Opens the claims send window.
        /// </summary>
        void sendClaimsButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.ClaimSend)) return;
            
            if (formClaimsSend != null && !formClaimsSend.IsDisposed)
            {
                formClaimsSend.Focus();
                return;
            }

            Cursor = Cursors.WaitCursor;

            formClaimsSend = new FormClaimsSend();
            formClaimsSend.Show();
            formClaimsSend.BringToFront();

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Opens the claims pay list window.
        /// </summary>
        void claimPayButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.InsPayCreate, true) && !Security.IsAuthorized(Permissions.InsPayEdit, true))
            {
                MessageBox.Show(
                    "Not authorized.\r\n" +
                    "A user with the SecurityAdmin permission must grant you access for:\r\n" +
                    "Insurance Payment Create or Insurance Payment Edit",
                    "",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            var formClaimPayList = new FormClaimPayList();
            formClaimPayList.Show();
        }

        /// <summary>
        /// Opens the billing window.
        /// </summary>
        void billingButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Billing)) return;

            if (Preferences.HasClinicsEnabled)
            {
                if (Statements.UnsentClinicStatementsExist(Clinics.ClinicNum))
                {
                    ShowBilling(Clinics.ClinicNum);
                }
                else 
                {
                    ShowBillingOptions(Clinics.ClinicNum);
                }
            }
            else
            {
                if (Statements.UnsentStatementsExist())
                {
                    ShowBilling(0);
                }
                else
                {
                    ShowBillingOptions(0);
                }
            }
            SecurityLog.Write(SecurityLogEvents.Billing, "");
        }

        /// <summary>
        /// Shows the deposits window.
        /// </summary>
        void depositsButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.DepositSlips, DateTime.Today)) return;

            using (var formDeposits = new FormDeposits())
            {
                formDeposits.ShowDialog();
            }
        }

        /// <summary>
        /// Shows the supply / inventory window.
        /// </summary>
        void supplyButton_Click(object sender, EventArgs e)
        {
            using (var formSupplyInventory = new FormSupplyInventory())
            {
                formSupplyInventory.ShowDialog();
            }
        }

        void collectionsButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Billing)) return;

            // TODO: This should be moved to a plugin.

            if (!Programs.IsEnabled(ProgramName.Transworld))
            {
                try
                {
                    Process.Start("http://www.opendental.com/manual/transworldsystems.html");
                }
                catch
                {
                    MessageBox.Show(
                        "Failed to open web browser. Please make sure you have a default browser set and are connected to the internet and then try again.",
                        "Collections",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                return;
            }

            if (formArManager == null || formArManager.IsDisposed)
            {
                while (!ValidateConnectionDetails()) // Only validate connection details if the ArManager form does not exist yet
                {
                    var message =
                        Preferences.HasClinicsEnabled ?
                            "An SFTP connection could not be made using the connection details in the enabled Transworld (TSI) program link. Would you like to edit the Transworld program link now?" :
                            "An SFTP connection could not be made using the connection details for any clinic in the enabled Transworld (TSI) program link. Would you like to edit the Transworld program link now?";

                    var result =
                        MessageBox.Show(
                            message, 
                            "Collections",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                    if (result == DialogResult.No) return;

                    using (var formTransworldSetup = new FormTransworldSetup())
                    {
                        if (formTransworldSetup.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }
                    }
                }

                formArManager = new FormArManager();
                formArManager.FormClosed += new FormClosedEventHandler((o, ev) => { formArManager = null; });
            }

            formArManager.Restore();
            formArManager.Show();
            formArManager.BringToFront();
        }

        /// <summary>
        /// Navigates to the task list.
        /// </summary>
        void tasksButton_Click(object sender, EventArgs e) => Navigate(NavigationTargets.Tasks, false, UserControlTasksTab.Invalid);

        void backupButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Backup)) return;

            SecurityLog.Write(SecurityLogEvents.Backup, "FormBackup was accessed");

            using (var formBackup = new FormBackup())
            {
                if (formBackup.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
            }

            //ok signifies that a database was restored
            FormOpenDental.S_Contr_PatientSelected(new Patient(), false);//unload patient after restore.
                                                                         //ParentForm.Text=PrefC.GetString(PrefName.MainWindowTitle");
            // TODO: DataValid.SetInvalid(true);

            ModuleSelected(PatCurNum);
        }

        void accountingButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Accounting)) return;

            if (formAccounting == null || formAccounting.IsDisposed)
            {
                formAccounting = new FormAccounting();
            }
            formAccounting.Show();
            if (formAccounting.WindowState == FormWindowState.Minimized)
            {
                formAccounting.WindowState = FormWindowState.Normal;
            }
            formAccounting.BringToFront();
        }

        void emailButton_Click(object sender, EventArgs e)
        {
            if (FormEmailInbox == null || FormEmailInbox.IsDisposed)
            {
                FormEmailInbox = new FormEmailInbox();
                FormEmailInbox.Show();
            }
            else
            {
                if (FormEmailInbox.WindowState == FormWindowState.Minimized)
                {
                    FormEmailInbox.WindowState = FormWindowState.Maximized;
                }
                FormEmailInbox.BringToFront();
            }
        }

        void erasButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.InsPayCreate)) return;

            Cursor = Cursors.WaitCursor;

            var formEtrans835s = new FormEtrans835s();
            formEtrans835s.Show();

            Cursor = Cursors.Default;
        }

        void importInsPlansButton_Click(object sender, EventArgs e)
        {
            if (FormE834I != null && FormE834I.FormE834P != null && !FormE834I.FormE834P.IsDisposed)
            {
                FormE834I.FormE834P.Show();
                FormE834I.FormE834P.BringToFront();
                return;
            }
            if (FormE834I == null || FormE834I.IsDisposed)
            {
                FormE834I = new FormEtrans834Import();
            }
            FormE834I.Show();
            FormE834I.BringToFront();
        }

        #endregion

        /// <summary>
        /// Shows FormBilling and displays warning message if needed. Pass 0 to show all clinics.
        /// Make sure to check for unsent bills before calling this method.
        /// </summary>
        void ShowBilling(long clinicNum, bool isHistStartMinDate = false)
        {
            bool hadListShowing = false;

            // Check to see if there is an instance of the billing list window already open that needs to be closed.
            // This can happen if multiple people are trying to send bills at the same time.
            if (formBilling != null && !formBilling.IsDisposed)
            {
                hadListShowing = true;

                // It does not hurt to always close this window before loading a new instance, because the unsent bills are saved in the 
                // database and the entire purpose of FormBilling is the Go To feature. Any statements that were showing in the old billing 
                // list window that we are about to close could potentially be stale and are now invalid and should not be sent. Another good 
                // reason to close the window is when using clinics. It was possible to show a different clinic billing list than the one chosen.
                for (int i = 0; i < formBilling.ListClinics.Count; i++)
                {
                    if (formBilling.ListClinics[i].ClinicNum != clinicNum)
                    {
                        // For most users clinic nums will always be 0.
                        // The old billing list was showing a different clinic. 
                        // No need to show the warning message in this scenario.
                        hadListShowing = false;
                    }
                }
                formBilling.Close();
            }

            formBilling = new FormBilling();
            formBilling.ClinicNum = clinicNum;
            formBilling.IsHistoryStartMinDate = isHistStartMinDate;
            formBilling.Show();
            formBilling.BringToFront();

            if (hadListShowing)
            {
                MessageBox.Show(
                    "These unsent bills must either be sent or deleted before a new list can be created.", 
                    "Billing", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Shows FormBillingOptions and FormBilling if needed. Pass 0 to show all clinics.
        /// Make sure to check for unsent bills before calling this method.
        /// </summary>
        void ShowBillingOptions(long clinicNum)
        {
            using (var formBillingOptions = new FormBillingOptions())
            {
                formBillingOptions.ClinicNum = clinicNum;
                if (formBillingOptions.ShowDialog() == DialogResult.OK)
                {
                    ShowBilling(clinicNum, formBillingOptions.IsHistoryStartMinDate);
                }
            }
        }


        bool ValidateConnectionDetails()
        {
            Program progCur = Programs.GetCur(ProgramName.Transworld);
            List<long> listClinicNums = new List<long>();

            if (Preferences.HasClinicsEnabled)
            {
                listClinicNums = Clinics.GetAllForUserod(Security.CurrentUser).Select(x => x.ClinicNum).ToList();
                if (!Security.CurrentUser.ClinicRestricted)
                {
                    listClinicNums.Add(0);
                }
            }
            else
            {
                listClinicNums.Add(0);
            }

            List<ProgramPreference> listProperties = ProgramProperties.GetForProgram(progCur.Id);
            foreach (long clinicNum in listClinicNums)
            {
                List<ProgramPreference> listPropsForClinic = new List<ProgramPreference>();
                if (listProperties.All(x => x.ClinicId != clinicNum))
                {//if no prog props exist for the clinic, continue, clinicNum 0 will be tested once as well
                    continue;
                }
                listPropsForClinic = listProperties.FindAll(x => x.ClinicId == clinicNum);
                string sftpAddress = listPropsForClinic.Find(x => x.Key == "SftpServerAddress")?.Value ?? "";
                int sftpPort;
                if (!int.TryParse(listPropsForClinic.Find(x => x.Key == "SftpServerPort")?.Value ?? "", out sftpPort))
                {
                    sftpPort = 22;//default to port 22
                }

                string userName = listPropsForClinic.Find(x => x.Key == "SftpUsername")?.Value ?? "";
                string userPassword = listPropsForClinic.Find(x => x.Key == "SftpPassword")?.Value ?? "";

                // TODO: Fix me
                //if (Sftp.IsConnectionValid(sftpAddress, userName, userPassword, sftpPort))
                //{
                //    return true;
                //}
            }
            return false;
        }

        /// <summary>
        /// Only used internally to launch the task window with the Triage task list.
        /// </summary>
        public void JumpToTriageTaskWindow() => Navigate(NavigationTargets.Tasks, true, UserControlTasksTab.Invalid);
        
        protected override void OnNavigating(NavigationEventArgs e)
        {
            base.OnNavigating(e);
            if (e.Target == NavigationTargets.Tasks)
            {
                if (e.Arguments.Length > 1 &&
                    e.Arguments[0] is bool isTriage &&
                    e.Arguments[1] is UserControlTasksTab tab)
                {
                    NavigateToTasks(isTriage, tab);

                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Used to launch the task window preloaded with a certain task list open. isTriage is only used at OD HQ.
        /// </summary>
        void NavigateToTasks(bool isTriage, UserControlTasksTab tab = UserControlTasksTab.Invalid)
        {
            if (formTasks == null || formTasks.IsDisposed)
            {
                formTasks = new FormTasks();
            }
            formTasks.Show();

            if (tab != UserControlTasksTab.Invalid)
            {
                formTasks.TaskTab = tab;
            }

            formTasks.Restore();
            formTasks.BringToFront();
        }








        void LoadEmployees()
        {
            employeeGrid.BeginUpdate();
            employeeGrid.Columns.Clear();
            employeeGrid.Columns.Add(new ODGridColumn("Employee", 180));
            employeeGrid.Columns.Add(new ODGridColumn("Status", 100));
            employeeGrid.Rows.Clear();

            if (Preferences.HasClinicsEnabled)
            {
                _listEmployees = Employee.GetEmpsForClinic(Clinics.ClinicNum, false, true);
            }
            else
            {
                _listEmployees = Employee.All();
            }

            foreach (Employee emp in _listEmployees)
            {
                var row = new ODGridRow();
                row.Cells.Add(Employee.GetNameFL(emp));
                row.Cells.Add(ConvertClockStatus(emp.ClockStatus));
                row.Tag = emp;
                employeeGrid.Rows.Add(row);
            }

            employeeGrid.EndUpdate();
            listStatus.Items.Clear();
            _listShownTimeClockStatuses.Clear();

            foreach (ClockEventStatus timeClockStatus in Enum.GetValues(typeof(ClockEventStatus)))
            {
                string statusDescript = timeClockStatus.GetDescription();
                if (!Preference.GetBool(PreferenceName.ClockEventAllowBreak))
                {
                    if (timeClockStatus == ClockEventStatus.Break)
                    {
                        continue;//Skip Break option.
                    }
                    else if (timeClockStatus == ClockEventStatus.Lunch)
                    {
                        statusDescript = ClockEventStatus.Break.GetDescription();//Change "Lunch" to "Break", still functions as Lunch.
                    }
                }
                _listShownTimeClockStatuses.Add(timeClockStatus);
                listStatus.Items.Add(statusDescript);
            }

            for (int i = 0; i < _listEmployees.Count; i++)
            {
                if (_listEmployees[i].Id == Security.CurrentUser.EmployeeId)
                {
                    SelectEmployee(i);
                    return;
                }
            }

            SelectEmployee(-1);
        }

        /// <summary>
        /// Returns a translated TimeClockStatus enum description from the given status.
        /// Also considers PrefName.ClockEventAllowBreak to switch 'Lunch' to 'Break' for the UI.
        /// </summary>
        string ConvertClockStatus(string status)
        {
            if (!Preference.GetBool(PreferenceName.ClockEventAllowBreak) && status == ClockEventStatus.Lunch.GetDescription())
            {
                status = ClockEventStatus.Break.GetDescription();
            }
            return status;
        }

        /// <summary>
        /// -1 is also valid.
        /// </summary>
        void SelectEmployee(int index, bool clearSelection = true)
        {
            if (clearSelection) employeeGrid.SetSelected(false);
            
            if (index == -1)
            {
                butClockIn.Enabled = false;
                butClockOut.Enabled = false;
                butTimeCard.Enabled = false;
                butBreaks.Enabled = false;
                listStatus.Enabled = false;
                return;
            }

            employeeGrid.SetSelected(index, true);
            EmployeeCur = _listEmployees[index];

            ClockEvent clockEvent = ClockEvent.GetLastEvent(EmployeeCur.Id);
            if (clockEvent == null) // New employee. They need to clock in.
            {
                butClockIn.Enabled = true;
                butClockOut.Enabled = false;
                butTimeCard.Enabled = true;
                butBreaks.Enabled = true;
                listStatus.SelectedIndex = _listShownTimeClockStatuses.IndexOf(ClockEventStatus.Home);
                listStatus.Enabled = false;
            }
            else if (clockEvent.Status == ClockEventStatus.Break)
            {
                // Only incomplete breaks will have been returned.
                // Clocked out for break, but not clocked back in
                butClockIn.Enabled = true;
                butClockOut.Enabled = false;
                butTimeCard.Enabled = true;
                butBreaks.Enabled = true;
                if (Preference.GetBool(PreferenceName.ClockEventAllowBreak))
                {
                    listStatus.SelectedIndex = _listShownTimeClockStatuses.IndexOf(ClockEventStatus.Break);
                }
                else
                {
                    //This will only happen when ClockEventAllowBreak has just changed to false, but employees are clocked out for TimeClockStatus.Break.
                    //Because listStatus only contains TimeClockStatus.Home and TimeClockStatus.Lunch(displays as "Break"), we can't choose TimeClockStatus.Break.
                    //Choose TimeClockStatus.Lunch which displays as "Break", and allow normal clocking in/out to handle transition into newly disabled 
                    //preference statuses.
                    listStatus.SelectedIndex = _listShownTimeClockStatuses.IndexOf(ClockEventStatus.Lunch);
                }
                listStatus.Enabled = false;
            }
            else
            {//normal clock in/out
                if (!clockEvent.Date2Displayed.HasValue)
                {//clocked in to work, but not clocked back out.
                    butClockIn.Enabled = false;
                    butClockOut.Enabled = true;
                    butTimeCard.Enabled = true;
                    butBreaks.Enabled = true;
                    listStatus.Enabled = true;
                }
                else
                {//clocked out for home or lunch.  Need to clock back in.
                    butClockIn.Enabled = true;
                    butClockOut.Enabled = false;
                    butTimeCard.Enabled = true;
                    butBreaks.Enabled = true;
                    listStatus.SelectedIndex = (int)clockEvent.Status;
                    listStatus.Enabled = false;
                }
            }
        }

        void employeeGrid_CellClick(object sender, ODGridClickEventArgs e)
        {
            if (employeeGrid.SelectedIndices.Length >= 2)
            {
                SelectEmployee(-1, false);
                return;
            }

            if (Preference.GetBool(PreferenceName.TimecardSecurityEnabled))
            {
                if (Security.CurrentUser.EmployeeId != _listEmployees[e.Row].Id)
                {
                    if (!Security.IsAuthorized(Permissions.TimecardsEditAll, true))
                    {
                        SelectEmployee(-1, false);
                        return;
                    }
                }
            }

            SelectEmployee(e.Row);
        }


        private void butClockIn_Click(object sender, System.EventArgs e)
        {
            if (employeeGrid.SelectedGridRows.Count > 1)
            {
                SelectEmployee(-1);
                return;
            }

            try
            {
                if (!Plugin.Filter(this, "ContrStaff_ClockIn", false, EmployeeCur))
                {
                    throw new Exception("You need to authenticate to clock-in");
                }

                ClockEvent.ClockIn(EmployeeCur.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            EmployeeCur.ClockStatus = Lan.g(this, "Working");
            Employee.Update(EmployeeCur);
            ModuleSelected(PatCurNum);
            if (PayPeriod.GetByDate(DateTime.Today) == null)
            {
                MsgBox.Show(this, "No dates exist for this pay period.  Time clock events will not display until pay periods have been created for this date range");
            }
        }

        private void butClockOut_Click(object sender, System.EventArgs e)
        {
            if (employeeGrid.SelectedGridRows.Count > 1)
            {
                SelectEmployee(-1);
                return;
            }

            if (listStatus.SelectedIndex == -1)
            {
                MsgBox.Show(this, "Please select a status first.");
                return;
            }

            try
            {
                if (!Plugin.Filter(this, "ContrStaff_ClockOut", false, EmployeeCur, _listShownTimeClockStatuses[listStatus.SelectedIndex]))
                {
                    throw new Exception("You need to authenticate to clock-out");
                }
                ClockEvent.ClockOut(EmployeeCur.Id, _listShownTimeClockStatuses[listStatus.SelectedIndex]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            EmployeeCur.ClockStatus = Lan.g("enumTimeClockStatus", (_listShownTimeClockStatuses[listStatus.SelectedIndex]).GetDescription());
            Employee.Update(EmployeeCur);
            ModuleSelected(PatCurNum);
        }

        private void timerUpdateTime_Tick(object sender, System.EventArgs e)
        {
            //this will happen once a second
            if (this.Visible)
            {
                textTime.Text = (DateTime.Now + TimeDelta).ToLongTimeString();
            }
        }

        private void butManage_Click(object sender, EventArgs e)
        {
            FormTimeCardManage FormTCM = new FormTimeCardManage(_listEmployees);
            FormTCM.ShowDialog();
            ModuleSelected(PatCurNum);
        }

        private void gridEmp_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (employeeGrid.SelectedGridRows.Count > 1)
            {//Just in case
                return;
            }

            if (PayPeriod.GetCount() == 0)
            {
                MsgBox.Show(this, "The adminstrator needs to setup pay periods first.");
                return;
            }

            if (!butTimeCard.Enabled)
            {
                return;
            }
            FormTimeCard FormTC = new FormTimeCard(_listEmployees);
            FormTC.EmployeeCur = _listEmployees[e.Row];
            FormTC.ShowDialog();
            ModuleSelected(PatCurNum);
        }

        private void butTimeCard_Click(object sender, System.EventArgs e)
        {
            if (employeeGrid.SelectedGridRows.Count > 1)
            {
                SelectEmployee(-1);
                return;
            }
            if (PayPeriod.GetCount() == 0)
            {
                MsgBox.Show(this, "The adminstrator needs to setup pay periods first.");
                return;
            }
            FormTimeCard FormTC = new FormTimeCard(_listEmployees);
            FormTC.EmployeeCur = EmployeeCur;
            FormTC.ShowDialog();
            ModuleSelected(PatCurNum);
        }

        private void butBreaks_Click(object sender, EventArgs e)
        {
            if (employeeGrid.SelectedGridRows.Count > 1)
            {
                SelectEmployee(-1);
                return;
            }

            if (PayPeriod.GetCount() == 0)
            {
                MessageBox.Show(
                    "The adminstrator needs to setup pay periods first.",
                    "Time Clock",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            FormTimeCard FormTC = new FormTimeCard(_listEmployees);
            FormTC.EmployeeCur = EmployeeCur;
            FormTC.IsBreaks = true;
            FormTC.ShowDialog();
            ModuleSelected(PatCurNum);
        }

        private void butViewSched_Click(object sender, EventArgs e)
        {
            // TODO: Fix me

            //List<long> listPreSelectedEmpNums = employeeGrid.SelectedGridRows.Select(x => ((Employee)x.Tag).Id).ToList();
            //List<long> listPreSelectedProvNums = User.GetWhere(x => listPreSelectedEmpNums.Contains(x.EmployeeId) && x.ProviderId != 0)
            //    .Select(x => x.ProviderId)
            //    .ToList();
            //FormSchedule formSched = new FormSchedule(listPreSelectedEmpNums, listPreSelectedProvNums);
            //formSched.ShowDialog();
        }




        #region Messaging
        ///<summary>Gets run with each module selected.  Should be very fast.</summary>
        private void FillMessageDefs()
        {
            sigElementDefUser = SigElementDefs.GetSubList(SignalElementType.User);
            sigElementDefExtras = SigElementDefs.GetSubList(SignalElementType.Extra);
            sigElementDefMessages = SigElementDefs.GetSubList(SignalElementType.Message);
            listTo.Items.Clear();
            for (int i = 0; i < sigElementDefUser.Length; i++)
            {
                listTo.Items.Add(sigElementDefUser[i].SigText);
            }
            listFrom.Items.Clear();
            for (int i = 0; i < sigElementDefUser.Length; i++)
            {
                listFrom.Items.Add(sigElementDefUser[i].SigText);
            }
            listExtras.Items.Clear();
            for (int i = 0; i < sigElementDefExtras.Length; i++)
            {
                listExtras.Items.Add(sigElementDefExtras[i].SigText);
            }
            listMessages.Items.Clear();
            for (int i = 0; i < sigElementDefMessages.Length; i++)
            {
                listMessages.Items.Add(sigElementDefMessages[i].SigText);
            }
            userComboBox.Items.Clear();
            userComboBox.Items.Add(Lan.g(this, "all"));
            for (int i = 0; i < sigElementDefUser.Length; i++)
            {
                userComboBox.Items.Add(sigElementDefUser[i].SigText);
            }
            userComboBox.SelectedIndex = 0;
        }

        ///<summary>Gets all new data from the database for the text messages.  Not sure yet if this will also reset the lights along the left.</summary>
        private void RefreshFullMessages()
        {
            _listSigMessages = SigMessages.GetSigMessagesSinceDateTime(DateTimeOD.Today);//since midnight this morning.
            FillMessages();
        }

        ///<summary>This does not refresh any data, just fills the grid.</summary>
        private void FillMessages()
        {
            if (textDays.Visible && errorProvider1.GetError(textDays) != "")
            {
                return;
            }
            long[] selected = new long[gridMessages.SelectedIndices.Length];
            for (int i = 0; i < selected.Length; i++)
            {
                selected[i] = _listSigMessages[gridMessages.SelectedIndices[i]].SigMessageNum;
            }
            gridMessages.BeginUpdate();
            gridMessages.Columns.Clear();
            ODGridColumn col = new ODGridColumn(Lan.g("TableTextMessages", "To"), 60);
            gridMessages.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableTextMessages", "From"), 60);
            gridMessages.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableTextMessages", "Sent"), 63);
            gridMessages.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableTextMessages", "Ack'd"), 63);
            col.TextAlign = HorizontalAlignment.Center;
            gridMessages.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableTextMessages", "Text"), 274);
            gridMessages.Columns.Add(col);
            gridMessages.Rows.Clear();
            ODGridRow row;
            string str;
            foreach (SigMessage sigMessage in _listSigMessages)
            {
                if (checkIncludeAck.Checked)
                {
                    if (sigMessage.AckDateTime.Year > 1880//if this is acked
                        && sigMessage.AckDateTime < DateTime.Today.AddDays(1 - PIn.Long(textDays.Text)))
                    {
                        continue;
                    }
                }
                else
                {//user does not want to include acked
                    if (sigMessage.AckDateTime.Year > 1880)
                    {//if this is acked
                        continue;
                    }
                }
                if (sigMessage.ToUser != ""//blank user always shows
                    && userComboBox.SelectedIndex != 0 //anything other than 'all'
                    && sigElementDefUser != null//for startup
                    && sigElementDefUser[userComboBox.SelectedIndex - 1].SigText != sigMessage.ToUser)//and users don't match
                {
                    continue;
                }
                row = new ODGridRow();
                row.Cells.Add(sigMessage.ToUser);
                row.Cells.Add(sigMessage.FromUser);
                if (sigMessage.MessageDateTime.Date == DateTime.Today)
                {
                    row.Cells.Add(sigMessage.MessageDateTime.ToShortTimeString());
                }
                else
                {
                    row.Cells.Add(sigMessage.MessageDateTime.ToShortDateString() + "\r\n" + sigMessage.MessageDateTime.ToShortTimeString());
                }
                if (sigMessage.AckDateTime.Year > 1880)
                {//ok
                    if (sigMessage.AckDateTime.Date == DateTime.Today)
                    {
                        row.Cells.Add(sigMessage.AckDateTime.ToShortTimeString());
                    }
                    else
                    {
                        row.Cells.Add(sigMessage.AckDateTime.ToShortDateString() + "\r\n" + sigMessage.AckDateTime.ToShortTimeString());
                    }
                }
                else
                {
                    row.Cells.Add("");
                }
                str = sigMessage.SigText;
                SigElementDef sigElementDefExtra = SigElementDefs.GetElementDef(sigMessage.SigElementDefNumExtra);
                if (sigElementDefExtra != null && !string.IsNullOrEmpty(sigElementDefExtra.SigText))
                {
                    str += (str == "") ? "" : ".  ";
                    str += sigElementDefExtra.SigText;
                }
                SigElementDef sigElementDefMsg = SigElementDefs.GetElementDef(sigMessage.SigElementDefNumMsg);
                if (sigElementDefMsg != null && !string.IsNullOrEmpty(sigElementDefMsg.SigText))
                {
                    str += (str == "") ? "" : ".  ";
                    str += sigElementDefMsg.SigText;
                }
                row.Cells.Add(str);
                row.Tag = sigMessage.Copy();
                gridMessages.Rows.Add(row);
                if (Array.IndexOf(selected, sigMessage.SigMessageNum) != -1)
                {
                    gridMessages.SetSelected(gridMessages.Rows.Count - 1, true);
                }
            }
            gridMessages.EndUpdate();
        }

        private void butSend_Click(object sender, System.EventArgs e)
        {
            if (textMessage.Text == "")
            {
                MsgBox.Show(this, "Please type in a message first.");
                return;
            }
            SigMessage sigMessage = new SigMessage();
            sigMessage.SigText = textMessage.Text;
            if (listTo.SelectedIndex != -1)
            {
                sigMessage.ToUser = sigElementDefUser[listTo.SelectedIndex].SigText;
                sigMessage.SigElementDefNumUser = sigElementDefUser[listTo.SelectedIndex].SigElementDefNum;
            }
            if (listFrom.SelectedIndex != -1)
            {
                sigMessage.FromUser = sigElementDefUser[listFrom.SelectedIndex].SigText;
            }
            SigMessages.Insert(sigMessage);
            textMessage.Text = "";
            listFrom.SelectedIndex = -1;
            listTo.SelectedIndex = -1;
            listExtras.SelectedIndex = -1;
            listMessages.SelectedIndex = -1;
            ShowSendingLabel();
            // TODO: Signalods.SetInvalid(InvalidType.SigMessages, KeyType.SigMessage, sigMessage.SigMessageNum);
        }

        private void listMessages_Click(object sender, EventArgs e)
        {
            if (listMessages.SelectedIndex == -1)
            {
                return;
            }
            SigMessage sigMessage = new SigMessage();
            sigMessage.SigText = textMessage.Text;
            if (listTo.SelectedIndex != -1)
            {
                sigMessage.ToUser = sigElementDefUser[listTo.SelectedIndex].SigText;
                sigMessage.SigElementDefNumUser = sigElementDefUser[listTo.SelectedIndex].SigElementDefNum;
            }
            if (listFrom.SelectedIndex != -1)
            {
                sigMessage.FromUser = sigElementDefUser[listFrom.SelectedIndex].SigText;
                //We do not set a SigElementDefNumUser for From.
            }
            if (listExtras.SelectedIndex != -1)
            {
                sigMessage.SigElementDefNumExtra = sigElementDefExtras[listExtras.SelectedIndex].SigElementDefNum;
            }
            sigMessage.SigElementDefNumMsg = sigElementDefMessages[listMessages.SelectedIndex].SigElementDefNum;
            //need to do this all as a transaction, so need to do a writelock on the signal table first.
            //alternatively, we could just make sure not to retrieve any signals that were less the 300ms old.
            SigMessages.Insert(sigMessage);
            //reset the controls
            textMessage.Text = "";
            listFrom.SelectedIndex = -1;
            listTo.SelectedIndex = -1;
            listExtras.SelectedIndex = -1;
            listMessages.SelectedIndex = -1;
            ShowSendingLabel();
            // TODO: Signalods.SetInvalid(InvalidType.SigMessages, KeyType.SigMessage, sigMessage.SigMessageNum);
        }

        ///<summary>Shows the sending label for 1 second.</summary>
        private void ShowSendingLabel()
        {
            sendingLabel.Visible = true;
            ODThread odThread = new ODThread((o) =>
            {
                Thread.Sleep((int)TimeSpan.FromSeconds(1).TotalMilliseconds);
                ODException.SwallowAnyException(() => { this.Invoke(() => { sendingLabel.Visible = false; }); });
            });
            odThread.Start();
        }

        ///<summary>This processes timed messages coming in from the main form.
        ///Buttons are handled in the main form, and then sent here for further display.
        ///The list gets filtered before display.</summary>
        public void LogMsgs(List<SigMessage> listSigMessages)
        {
            foreach (SigMessage sigMessage in listSigMessages)
            {
                SigMessage sigMessageUpdate = _listSigMessages.FirstOrDefault(x => x.SigMessageNum == sigMessage.SigMessageNum);
                if (sigMessageUpdate == null)
                {
                    _listSigMessages.Add(sigMessage.Copy());
                }
                else
                {//SigMessage is already in our list and we just need to update it.
                    sigMessageUpdate.AckDateTime = sigMessage.AckDateTime;
                }
            }
            _listSigMessages.Sort();
            FillMessages();
        }

        private void butAck_Click(object sender, EventArgs e)
        {
            if (gridMessages.SelectedIndices.Length == 0)
            {
                MsgBox.Show(this, "Please select at least one item first.");
                return;
            }
            SigMessage sigMessage;
            for (int i = gridMessages.SelectedIndices.Length - 1; i >= 0; i--)
            {//go backwards so that we can remove rows without problems.
                sigMessage = (SigMessage)gridMessages.Rows[gridMessages.SelectedIndices[i]].Tag;
                if (sigMessage.AckDateTime.Year > 1880)
                {
                    continue;//totally ignore if trying to ack a previously acked signal
                }
                SigMessages.AckSigMessage(sigMessage);
                //change the grid temporarily until the next timer event.  This makes it feel more responsive.
                if (checkIncludeAck.Checked)
                {
                    gridMessages.Rows[gridMessages.SelectedIndices[i]].Cells[3].Text = sigMessage.MessageDateTime.ToShortTimeString();
                }
                else
                {
                    try
                    {
                        gridMessages.Rows.RemoveAt(gridMessages.SelectedIndices[i]);
                    }
                    catch
                    {
                        //do nothing
                    }
                }
                // TODO: Signalods.SetInvalid(InvalidType.SigMessages, KeyType.SigMessage, sigMessage.SigMessageNum);
            }
            gridMessages.SetSelected(false);
        }

        private void checkIncludeAck_Click(object sender, EventArgs e)
        {
            if (checkIncludeAck.Checked)
            {
                textDays.Text = "1";
                daysLabel.Visible = true;
                textDays.Visible = true;
            }
            else
            {
                daysLabel.Visible = false;
                textDays.Visible = false;
                _listSigMessages = SigMessages.GetSigMessagesSinceDateTime(DateTimeOD.Today);//since midnight this morning.
            }
            FillMessages();
        }

        private void textDays_TextChanged(object sender, EventArgs e)
        {
            if (!textDays.Visible)
            {
                errorProvider1.SetError(textDays, "");
                return;
            }
            try
            {
                int days = int.Parse(textDays.Text);
                errorProvider1.SetError(textDays, "");
                _listSigMessages = SigMessages.GetSigMessagesSinceDateTime(DateTimeOD.Today.AddDays(-days));
                FillMessages();
            }
            catch
            {
                errorProvider1.SetError(textDays, Lan.g(this, "Invalid number.  Usually 1 or 2."));
            }
        }

        private void userComboBox_SelectionChangeCommitted(object sender, EventArgs e) => FillMessages();
        

        #endregion Messaging
    }

    public delegate void OnPatientSelected(Patient pat);
}