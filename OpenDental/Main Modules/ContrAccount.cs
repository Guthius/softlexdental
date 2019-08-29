using CodeBase;
using OpenDental.Properties;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class ContrAccount : Module
    {

        private FormPayPlan FormPayPlan2;

        ///<summary>Gets updated to PatCur.PatNum that the last security log was made with so that we don't make too many security logs for this patient.  When _patNumLast no longer matches PatCur.PatNum (e.g. switched to a different patient within a module), a security log will be entered.  Gets reset (cleared and the set back to PatCur.PatNum) any time a module button is clicked which will cause another security log to be entered.</summary>
        private long _patNumLast;

        ///<summary>Partially implemented lock object for an attempted bug fix.</summary>
        private object _lockDataSetMain = new object();
        ///<summary>This holds some of the data needed for display.  It is retrieved in one call to the database.</summary>
        private DataSet DataSetMain;
        ///<summary>This holds nearly all of the data needed for display.  It is retrieved in one call to the database.</summary>
        private AccountModules.LoadData _loadData;
        private Family FamCur;
        ///<summary></summary>
        private Patient PatCur;
        private PatientNote PatientNoteCur;
        private RepeatCharge[] RepeatChargeList;
        ///<summary>Public so this can be checked from FormOpenDental and the note can be saved.  Necessary because in some cases the leave event doesn't
        ///fire, like when a user switches to a non-modal form, like big phones, and switches patients from that form.</summary>
        public bool FinNoteChanged;
        ///<summary>Public so this can be checked from FormOpenDental and the note can be saved.  Necessary because in some cases the leave event doesn't
        ///fire, like when a user switches to a non-modal form, like big phones, and switches patients from that form.</summary>
        public bool UrgFinNoteChanged;
        private int Actscrollval;
        ///<summary>Set to true if this control is placed in the recall edit window. This affects the control behavior.</summary>
        public bool ViewingInRecall = false;
        private List<DisplayField> fieldsForMainGrid;
        private List<DisplayField> _patInfoDisplayFields;
        private bool InitializedOnStartup;
        private decimal PPBalanceTotal;
        private PatField[] _patFieldList;
        private MenuItem menuPrepayment;
        private Definition[] _acctProcQuickAddDefs;
        ///<summary>True if 'Entire Family' is selected in the Select Patient grid.</summary>
        public bool _isSelectingFamily
        {
            get
            {
                if (DataSetMain == null)
                {
                    return false;
                }
                return gridAcctPat.GetSelectedIndex() == gridAcctPat.Rows.Count - 1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContrAccount"/> class.
        /// </summary>
        public ContrAccount()
        {
            Logger.Write(LogLevel.Info, "Initializing account module...");

            InitializeComponent();// This call is required by the Windows.Forms Form Designer.
        }


        public void InitializeOnStartup()
        {
            if (InitializedOnStartup && !ViewingInRecall)
            {
                return;
            }
            InitializedOnStartup = true;

            LayoutToolBar();
            textQuickProcs.AcceptsTab = true;
            textQuickProcs.KeyDown += textQuickCharge_KeyDown;
            textQuickProcs.MouseDown += textQuickCharge_MouseClick;
            textQuickProcs.MouseCaptureChanged += textQuickCharge_CaptureChange;
            textQuickProcs.LostFocus += textQuickCharge_FocusLost;
            ToolBarMain.Controls.Add(textQuickProcs);
            splitContainerAccountCommLog.SplitterDistance = splitContainerParent.Panel2.Height * 3 / 5;//Make Account grid slightly bigger than commlog
                                                                                                       //This just makes the patient information grid show up or not.
            _patInfoDisplayFields = DisplayFields.GetForCategory(DisplayFieldCategory.AccountPatientInformation);
            LayoutPanels();
            checkShowFamilyComm.Checked = Preference.GetBool(PreferenceName.ShowAccountFamilyCommEntries, true);
            checkShowCompletePayPlans.Checked = Preference.GetBool(PreferenceName.AccountShowCompletedPaymentPlans);
            //Plugins.HookAddCode(this,"ContrAccount.InitializeOnStartup_end");
        }

        private void textQuickCharge_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X < 0 || e.X > textQuickProcs.Width || e.Y < 0 || e.Y > textQuickProcs.Height)
            {
                textQuickProcs.Text = "";
                textQuickProcs.Visible = false;
                textQuickProcs.Capture = false;
            }
        }

        private void textQuickCharge_CaptureChange(object sender, EventArgs e)
        {
            if (textQuickProcs.Visible == true)
            {
                textQuickProcs.Capture = true;
            }
        }

        private void ContrAccount_Load(object sender, System.EventArgs e)
        {
            Parent.MouseWheel += new MouseEventHandler(Parent_MouseWheel);

            menuPrepayment.Visible = false;
            menuPrepayment.Enabled = false;
        }

        ///<summary>Causes the toolbar to be laid out again.</summary>
        public void LayoutToolBar()
        {
            ToolBarMain.Buttons.Clear();
            ODToolBarButton button;
            _butPayment = new ODToolBarButton(Lan.g(this, "Payment"), Resources.IconMoneyAdd, "", "Payment");
            _butPayment.Style = ODToolBarButtonStyle.DropDownButton;
            _butPayment.DropDownMenu = contextMenuPayment;
            ToolBarMain.Buttons.Add(_butPayment);
            button = new ODToolBarButton(Lan.g(this, "Adjustment"), null, "", "Adjustment");
            button.Style = ODToolBarButtonStyle.DropDownButton;
            button.DropDownMenu = contextMenuAdjust;
            ToolBarMain.Buttons.Add(button);
            button = new ODToolBarButton(Lan.g(this, "New Claim"), Resources.IconInsurance, "", "Insurance");
            button.Style = ODToolBarButtonStyle.DropDownButton;
            button.DropDownMenu = contextMenuIns;
            ToolBarMain.Buttons.Add(button);
            ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
            button = new ODToolBarButton(Lan.g(this, "Payment Plan"), null, "", "PayPlan");
            button.Style = ODToolBarButtonStyle.DropDownButton;
            button.DropDownMenu = contextMenuPayPlan;
            ToolBarMain.Buttons.Add(button);
            ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this, "Installment Plan"), null, "", "InstallPlan"));
            if (Security.IsAuthorized(Permissions.AccountProcsQuickAdd, true))
            {
                //If the user doesn't have permission to use the quick charge button don't add it to the toolbar.
                ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
                _butQuickProcs = new ODToolBarButton(Lan.g(this, "Quick Procs"), null, "", "QuickProcs");
                _butQuickProcs.Style = ODToolBarButtonStyle.DropDownButton;
                _butQuickProcs.DropDownMenu = contextMenuQuickProcs;
                contextMenuQuickProcs.Popup += new EventHandler(contextMenuQuickProcs_Popup);
                ToolBarMain.Buttons.Add(_butQuickProcs);
            }
            if (!Preference.GetBool(PreferenceName.EasyHideRepeatCharges))
            {
                button = new ODToolBarButton(Lan.g(this, "Repeating Charge"), null, "", "RepeatCharge");
                button.Style = ODToolBarButtonStyle.PushButton;
                ToolBarMain.Buttons.Add(button);
            }
            ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
            button = new ODToolBarButton(Lan.g(this, "Statement"), Resources.IconPrint, "", "Statement");
            button.Style = ODToolBarButtonStyle.DropDownButton;
            button.DropDownMenu = contextMenuStatement;
            ToolBarMain.Buttons.Add(button);
            if (Preference.GetBool(PreferenceName.AccountShowQuestionnaire))
            {
                ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
                ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this, "Questionnaire"), null, "", "Questionnaire"));
            }
            if (Preference.GetBool(PreferenceName.AccountShowTrojanExpressCollect))
            {
                ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
                ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this, "TrojanCollect"), null, "", "TrojanCollect"));
            }
            ProgramL.LoadToolbar(ToolBarMain, ToolBarsAvail.AccountModule);
            ToolBarMain.Invalidate();
            //Plugins.HookAddCode(this,"ContrAccount.LayoutToolBar_end",PatCur);
        }

        ///<summary>This gets run just prior to the contextMenuQuickCharge menu displaying to the user.</summary>
        private void contextMenuQuickProcs_Popup(object sender, EventArgs e)
        {
            //Dynamically fill contextMenuQuickCharge's menu items because the definitions may have changed since last time it was filled.
            _acctProcQuickAddDefs = Definition.GetByCategory(DefinitionCategory.AccountQuickCharge).ToArray();
            contextMenuQuickProcs.MenuItems.Clear();
            for (int i = 0; i < _acctProcQuickAddDefs.Length; i++)
            {
                contextMenuQuickProcs.MenuItems.Add(new MenuItem(_acctProcQuickAddDefs[i].Description, menuItemQuickProcs_Click));
            }
            if (_acctProcQuickAddDefs.Length == 0)
            {
                contextMenuQuickProcs.MenuItems.Add(new MenuItem(Lan.g(this, "No quick charge procedures defined. Go to Setup | Definitions to add."), (x, y) => { }));//"null" event handler.
            }
        }

        private void ContrAccount_Layout(object sender, System.Windows.Forms.LayoutEventArgs e)
        {
            //see LayoutPanels()
        }

        private void ContrAccount_Resize(object sender, EventArgs e)
        {
            LayoutPanels();
        }

        ///<summary>This used to be a layout event, but that was making it get called far too frequently.  Now, this must explicitly and intelligently be called.</summary>
        private void LayoutPanels()
        {
            //Collapse panels according to what is visible at the given time.
            //If both are not visible, collapse the entire parent panel so it does not show extra white space.
            splitContainerParent.Panel1Collapsed = !gridRepeat.Visible && !gridPayPlan.Visible;
            if (!gridRepeat.Visible)
            {
                splitContainerRepChargesPP.Panel1Collapsed = true;
                splitContainerParent.Panel1MinSize = 20;
            }
            if (!gridPayPlan.Visible)
            {
                splitContainerRepChargesPP.Panel2Collapsed = true;
                splitContainerParent.Panel1MinSize = 20;
            }
            //If both visible, make sure the minimum size is set back to orignal value.
            if (gridPayPlan.Visible && gridRepeat.Visible)
            {
                splitContainerParent.Panel1MinSize = 45;
            }
            //60px is the height needed for the tabs, the grid title, and the horizontal scrollbar.
            splitContainerAccountCommLog.Panel1MinSize = 60 - (gridAccount.HScrollVisible ? 0 : gridAccount.HScrollHeight);
            //85px is the height needed for the account grid and the commlog grid.
            splitContainerParent.Panel2MinSize = 85 - (gridAccount.HScrollVisible ? 0 : gridAccount.HScrollHeight);
            if (_patInfoDisplayFields != null && _patInfoDisplayFields.Count > 0)
            {
                patientInfoGrid.Height = Height - patientInfoGrid.Top;
                patientInfoGrid.Invalidate();
                patientInfoGrid.Visible = true;
            }
            else
            {
                patientInfoGrid.Visible = false;
            }
            gridProg.Top = 0;
            gridProg.Height = panelProgNotes.Height;
            /*
			panelBoldBalance.Left=329;
			panelBoldBalance.Top=29;
			panelInsInfoDetail.Top = panelBoldBalance.Top + panelBoldBalance.Height;
			panelInsInfoDetail.Left = panelBoldBalance.Left + panelBoldBalance.Width - panelInsInfoDetail.Width;*/
            int left = textUrgFinNote.Left;//769;
            labelFamFinancial.Location = new Point(left, gridAcctPat.Bottom);
            textFinNote.Location = new Point(left, labelFamFinancial.Bottom);
            //tabControlShow.Height=panelCommButs.Top-tabControlShow.Top;
            textFinNote.Height = tabMain.Height - textFinNote.Top;
            //only show the ortho grid and tab control if they have the show feature enabled.
            //otherwise, hide the tabs and re-size the account grid.
            if (!Preference.GetBool(PreferenceName.OrthoEnabled))
            {
                tabControlAccount.TabPages.Remove(tabPageOrtho);
                tabControlAccount.Appearance = TabAppearance.FlatButtons;
                tabControlAccount.SizeMode = TabSizeMode.Fixed;
                tabControlAccount.ItemSize = new Size(0, 1);
                tabControlAccount.DrawMode = TabDrawMode.OwnerDrawFixed;
                tabControlAccount.Bounds = new Rectangle(-4, tabControlAccount.Bounds.Y, gridComm.Width + 8, tabControlAccount.Height);
            }
            else if (!tabControlAccount.TabPages.Contains(tabPageOrtho))
            {
                tabControlAccount.TabPages.Add(tabPageOrtho);
                tabControlAccount.Appearance = TabAppearance.Normal;
                tabControlAccount.SizeMode = TabSizeMode.Normal;
                tabControlAccount.DrawMode = TabDrawMode.Normal;
                tabControlAccount.ItemSize = new Size(370, 18);
                tabControlAccount.Bounds = new Rectangle(0, tabControlAccount.Bounds.Y, gridComm.Width + 6, tabControlAccount.Height);
            }
        }

        ///<summary></summary>
        public void ModuleSelected(long patNum)
        {
            ModuleSelected(patNum, false);
        }

        ///<summary></summary>
        public void ModuleSelected(long patNum, bool isSelectingFamily)
        {
            UserOdPref userOdPrefProcBreakdown = UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum, UserOdFkeyType.AcctProcBreakdown).FirstOrDefault();
            if (userOdPrefProcBreakdown == null)
            {
                checkShowDetail.Checked = true;
            }
            else
            {
                checkShowDetail.Checked = PIn.Bool(userOdPrefProcBreakdown.ValueString);
            }
            RefreshModuleData(patNum, isSelectingFamily);
            RefreshModuleScreen(isSelectingFamily);

            //Plugins.HookAddCode(this,"ContrAccount.ModuleSelected_end",patNum,isSelectingFamily);
        }

        ///<summary>Used when jumping to this module and directly to a claim.</summary>
        public void ModuleSelected(long patNum, long claimNum)
        {
            ModuleSelected(patNum);
            DataTable table = DataSetMain.Tables["account"];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i]["ClaimPaymentNum"].ToString() != "0")
                {//claimpayment
                    continue;
                }
                if (table.Rows[i]["ClaimNum"].ToString() == "0")
                {//not a claim or claimpayment
                    continue;
                }
                long claimNumRow = PIn.Long(table.Rows[i]["ClaimNum"].ToString());
                if (claimNumRow != claimNum)
                {
                    continue;
                }
                gridAccount.SetSelected(i, true);
            }
        }

        ///<summary></summary>
        public void ModuleUnselected()
        {
            UpdateUrgFinNote();
            UpdateFinNote();
            FamCur = null;
            RepeatChargeList = null;
            _patNumLast = 0;//Clear out the last pat num so that a security log gets entered that the module was "visited" or "refreshed".
                            //Plugins.HookAddCode(this,"ContrAccount.ModuleUnselected_end");
        }

        ///<summary></summary>
        private void RefreshModuleData(long patNum, bool isSelectingFamily)
        {
            UpdateUrgFinNote();
            UpdateFinNote();
            if (patNum == 0)
            {
                PatCur = null;
                FamCur = null;
                DataSetMain = null;
                //Plugins.HookAddCode(this,"ContrAccount.RefreshModuleData_null");
                return;
            }
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MaxValue;
            if (textDateStart.errorProvider1.GetError(textDateStart) == ""
                && textDateEnd.errorProvider1.GetError(textDateEnd) == "")
            {
                if (textDateStart.Text != "")
                {
                    fromDate = PIn.Date(textDateStart.Text);
                }
                if (textDateEnd.Text != "")
                {
                    toDate = PIn.Date(textDateEnd.Text);
                }
            }
            bool viewingInRecall = ViewingInRecall;
            if (Preference.GetBool(PreferenceName.FuchsOptionsOn))
            {
                panelTotalOwes.Top = -38;
                viewingInRecall = true;
            }
            bool doMakeSecLog = false;
            if (_patNumLast != patNum)
            {
                doMakeSecLog = true;
                _patNumLast = patNum;
            }
            bool doGetOrtho = Preference.GetBool(PreferenceName.OrthoEnabled);
            _loadData = AccountModules.GetAll(patNum, viewingInRecall, fromDate, toDate, isSelectingFamily, checkShowDetail.Checked, true, true, doMakeSecLog, doGetOrtho);

            lock (_lockDataSetMain)
            {
                DataSetMain = _loadData.DataSetMain;
            }
            FamCur = _loadData.Fam;
            PatCur = FamCur.GetPatient(patNum);
            PatientNoteCur = _loadData.PatNote;
            _patFieldList = _loadData.ArrPatFields;
            FillSummary();
            //Plugins.HookAddCode(this,"ContrAccount.RefreshModuleData_end",FamCur,PatCur,DataSetMain,PPBalanceTotal,isSelectingFamily);
        }

        ///<summary>Returns a deep copy of the corresponding table from the main data set.
        ///Utilizes a lock object that is partially implemented in an attempt to fix an error when invoking DataTable.Clone()</summary>
        private DataTable GetTableFromDataSet(string tableName)
        {
            DataTable table;
            lock (_lockDataSetMain)
            {
                table = DataSetMain.Tables[tableName].Clone();
                foreach (DataRow row in DataSetMain.Tables[tableName].Rows)
                {
                    table.ImportRow(row);
                }
            }
            return table;
        }

        private void RefreshModuleScreen(bool isSelectingFamily)
        {
            if (PatCur == null)
            {
                tabControlAccount.Enabled = false;
                ToolBarMain.Buttons["Payment"].Enabled = false;
                ToolBarMain.Buttons["Adjustment"].Enabled = false;
                ToolBarMain.Buttons["Insurance"].Enabled = false;
                ToolBarMain.Buttons["PayPlan"].Enabled = false;
                ToolBarMain.Buttons["InstallPlan"].Enabled = false;
                if (ToolBarMain.Buttons["QuickProcs"] != null)
                {
                    ToolBarMain.Buttons["QuickProcs"].Enabled = false;
                }
                if (ToolBarMain.Buttons["RepeatCharge"] != null)
                {
                    ToolBarMain.Buttons["RepeatCharge"].Enabled = false;
                }
                ToolBarMain.Buttons["Statement"].Enabled = false;
                if (ToolBarMain.Buttons["Questionnaire"] != null && Preference.GetBool(PreferenceName.AccountShowQuestionnaire))
                {
                    ToolBarMain.Buttons["Questionnaire"].Enabled = false;
                }
                if (ToolBarMain.Buttons["TrojanCollect"] != null && Preference.GetBool(PreferenceName.AccountShowTrojanExpressCollect))
                {
                    ToolBarMain.Buttons["TrojanCollect"].Enabled = false;
                }
                ToolBarMain.Invalidate();
                textUrgFinNote.Enabled = false;
                textFinNote.Enabled = false;
                //butComm.Enabled=false;
                tabControlShow.Enabled = false;
                //Plugins.HookAddCode(this,"ContrAccount.RefreshModuleScreen_null");
            }
            else
            {
                tabControlAccount.Enabled = true;
                ToolBarMain.Buttons["Payment"].Enabled = true;
                ToolBarMain.Buttons["Adjustment"].Enabled = true;
                ToolBarMain.Buttons["Insurance"].Enabled = true;
                ToolBarMain.Buttons["PayPlan"].Enabled = true;
                ToolBarMain.Buttons["InstallPlan"].Enabled = true;
                if (ToolBarMain.Buttons["QuickProcs"] != null)
                {
                    ToolBarMain.Buttons["QuickProcs"].Enabled = true;
                }
                if (ToolBarMain.Buttons["RepeatCharge"] != null)
                {
                    ToolBarMain.Buttons["RepeatCharge"].Enabled = true;
                }
                ToolBarMain.Buttons["Statement"].Enabled = true;
                if (ToolBarMain.Buttons["Questionnaire"] != null && Preference.GetBool(PreferenceName.AccountShowQuestionnaire))
                {
                    ToolBarMain.Buttons["Questionnaire"].Enabled = true;
                }
                if (ToolBarMain.Buttons["TrojanCollect"] != null && Preference.GetBool(PreferenceName.AccountShowTrojanExpressCollect))
                {
                    ToolBarMain.Buttons["TrojanCollect"].Enabled = true;
                }
                ToolBarMain.Invalidate();
                textUrgFinNote.Enabled = true;
                textFinNote.Enabled = true;
                //butComm.Enabled=true;
                tabControlShow.Enabled = true;
            }
            FillPats(isSelectingFamily);
            FillMisc();
            FillAging(isSelectingFamily);
            //must be in this order.
            FillRepeatCharges();
            FillPaymentPlans();
            FillMain();

            if (Preference.GetBool(PreferenceName.OrthoEnabled))
            {
                FillOrtho(false);
            }
            FillPatInfo();

            LayoutPanels();
            if (ViewingInRecall || Preference.GetBool(PreferenceName.FuchsOptionsOn, false))
            {
                panelProgNotes.Visible = true;
                FillProgNotes();

                if (Preference.GetBool(PreferenceName.FuchsOptionsOn) && PatCur != null)
                {//show prog note options
                    groupBox6.Visible = true;
                    groupBox7.Visible = true;
                    showAllButton.Visible = true;
                    showNoneButton.Visible = true;
                    //FillInsInfo();
                }
            }
            else
            {
                panelProgNotes.Visible = false;
                FillComm();
            }
            //Plugins.HookAddCode(this,"ContrAccount.RefreshModuleScreen_end",FamCur,PatCur,DataSetMain,PPBalanceTotal,isSelectingFamily);
        }

        ///<summary>Call this before inserting new repeat charge to update patient.BillingCycleDay if no other repeat charges exist.
        ///Changes the patient's BillingCycleDay to today if no other active repeat charges are on the patient's account</summary>
        private void UpdatePatientBillingDay(long patNum)
        {
            if (RepeatCharges.ActiveRepeatChargeExists(patNum))
            {
                return;
            }
            Patient patOld = Patients.GetPat(patNum);
            if (patOld.BillingCycleDay == DateTimeOD.Today.Day)
            {
                return;
            }
            Patient patNew = patOld.Copy();
            patNew.BillingCycleDay = DateTimeOD.Today.Day;
            Patients.Update(patNew, patOld);
        }

        //private void FillPatientButton() {
        //	Patients.AddPatsToMenu(menuPatient,new EventHandler(menuPatient_Click),PatCur,FamCur);
        //}

        private void FillPats(bool isSelectingFamily)
        {
            if (PatCur == null)
            {
                gridAcctPat.BeginUpdate();
                gridAcctPat.Rows.Clear();
                gridAcctPat.EndUpdate();
                return;
            }
            gridAcctPat.BeginUpdate();
            gridAcctPat.Columns.Clear();
            ODGridColumn col = new ODGridColumn(Lan.g("TableAccountPat", "Patient"), 105);
            gridAcctPat.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableAccountPat", "Bal"), 49, textAlignment: HorizontalAlignment.Right);
            gridAcctPat.Columns.Add(col);
            gridAcctPat.Rows.Clear();
            ODGridRow row;
            DataTable table = DataSetMain.Tables["patient"];
            decimal bal = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (i != table.Rows.Count - 1 && PatientLinks.WasPatientMerged(PIn.Long(table.Rows[i]["PatNum"].ToString()), _loadData.ListMergeLinks)
                    && FamCur.ListPats[i].PatNum != PatCur.PatNum && ((decimal)table.Rows[i]["balanceDouble"]) == 0)
                {
                    //Hide merged patients so that new things don't get added to them. If the user really wants to find this patient, they will have to use 
                    //the Select Patient window.
                    continue;
                }
                bal += (decimal)table.Rows[i]["balanceDouble"];
                row = new ODGridRow();
                row.Cells.Add(GetPatNameFromTable(table, i));
                row.Cells.Add(table.Rows[i]["balance"].ToString());
                row.Tag = PIn.Long(table.Rows[i]["PatNum"].ToString());
                if (i == 0 || i == table.Rows.Count - 1)
                {
                    row.Bold = true;
                }
                gridAcctPat.Rows.Add(row);
            }
            gridAcctPat.EndUpdate();
            if (isSelectingFamily)
            {
                gridAcctPat.SetSelected(gridAcctPat.Rows.Count - 1, true);
            }
            else
            {
                int index = gridAcctPat.Rows.ToList().FindIndex(x => (long)x.Tag == PatCur.PatNum);
                if (index >= 0)
                {
                    //If the index is greater than the number of rows, it will return and not select anything.
                    gridAcctPat.SetSelected(index, true);
                }
            }
            if (isSelectingFamily)
            {
                ToolBarMain.Buttons["Insurance"].Enabled = false;
            }
            else
            {
                ToolBarMain.Buttons["Insurance"].Enabled = true;
            }
        }

        private string GetPatNameFromTable(DataTable table, int index)
        {
            string name = table.Rows[index]["name"].ToString();
            if (Preference.GetBool(PreferenceName.TitleBarShowSpecialty) && string.Compare(name, "Entire Family", true) != 0)
            {
                long patNum = PIn.Long(table.Rows[index]["PatNum"].ToString());
                string specialty = Patients.GetPatientSpecialtyDef(patNum)?.Description ?? "";
                name += string.IsNullOrWhiteSpace(specialty) ? "" : "\r\n" + specialty;
            }
            return name;
        }

        private void FillMisc()
        {
            //textCC.Text="";
            //textCCexp.Text="";
            if (PatCur == null)
            {
                textUrgFinNote.Text = "";
                textFinNote.Text = "";
            }
            else
            {
                textUrgFinNote.Text = FamCur.ListPats[0].FamFinUrgNote;
                textFinNote.Text = PatientNoteCur.FamFinancial;
                if (!textFinNote.Focused)
                {
                    textFinNote.SelectionStart = textFinNote.Text.Length;
                    //This will cause a crash if the richTextBox currently has focus. We don't know why.
                    //Only happens if you call this during a Leave event, and only when moving between two ODtextBoxes.
                    //Tested with two ordinary richTextBoxes, and the problem does not exist.
                    //We may pursue fixing the root problem some day, but this workaround will do for now.
                    textFinNote.ScrollToCaret();
                }
                if (!textUrgFinNote.Focused)
                {
                    textUrgFinNote.SelectionStart = 0;
                    textUrgFinNote.ScrollToCaret();
                }
                //if(PrefC.GetBool(PrefName.StoreCCnumbers)) {
                //string cc=PatientNoteCur.CCNumber;
                //if(Regex.IsMatch(cc,@"^\d{16}$")){
                //  textCC.Text=cc.Substring(0,4)+"-"+cc.Substring(4,4)+"-"+cc.Substring(8,4)+"-"+cc.Substring(12,4);
                //}
                //else{
                //  textCC.Text=cc;
                //}
                //if(PatientNoteCur.CCExpiration.Year>2000){
                //  textCCexp.Text=PatientNoteCur.CCExpiration.ToString("MM/yy");
                //}
                //else{
                //  textCCexp.Text="";
                //}
                //}
            }
            UrgFinNoteChanged = false;
            FinNoteChanged = false;
            //CCChanged=false;
            if (ViewingInRecall)
            {
                textUrgFinNote.ReadOnly = true;
                textFinNote.ReadOnly = true;
            }
            else
            {
                textUrgFinNote.ReadOnly = false;
                textFinNote.ReadOnly = false;
            }
        }

        private void FillAging(bool isSelectingFamily)
        {
            //if(Plugins.HookMethod(this,"ContrAccount.FillAging",FamCur,PatCur,DataSetMain,isSelectingFamily)) {
            //	return;
            //}
            if (PatCur != null)
            {
                textOver90.Text = FamCur.ListPats[0].BalOver90.ToString("F");
                text61_90.Text = FamCur.ListPats[0].Bal_61_90.ToString("F");
                text31_60.Text = FamCur.ListPats[0].Bal_31_60.ToString("F");
                text0_30.Text = FamCur.ListPats[0].Bal_0_30.ToString("F");
                decimal total = (decimal)FamCur.ListPats[0].BalTotal;
                labelTotalAmt.Text = total.ToString("F");
                labelInsEstAmt.Text = FamCur.ListPats[0].InsEst.ToString("F");
                labelBalanceAmt.Text = (total - (decimal)FamCur.ListPats[0].InsEst).ToString("F");
                labelPatEstBalAmt.Text = "";
                DataTable tableMisc = DataSetMain.Tables["misc"];
                if (!isSelectingFamily)
                {
                    for (int i = 0; i < tableMisc.Rows.Count; i++)
                    {
                        if (tableMisc.Rows[i]["descript"].ToString() == "patInsEst")
                        {
                            decimal estBal = (decimal)PatCur.EstBalance - PIn.Decimal(tableMisc.Rows[i]["value"].ToString());
                            labelPatEstBalAmt.Text = estBal.ToString("F");
                        }
                    }
                }
                labelUnearnedAmt.Text = "";
                for (int i = 0; i < tableMisc.Rows.Count; i++)
                {
                    if (tableMisc.Rows[i]["descript"].ToString() == "unearnedIncome")
                    {
                        labelUnearnedAmt.Text = PaySplits.GetUnearnedForFam(FamCur, _loadData.ListPrePayments).ToString("F");
                        if (PIn.Double(labelUnearnedAmt.Text) <= 0)
                        {
                            labelUnearnedAmt.ForeColor = Color.Black;
                            labelUnearnedAmt.Font = new Font(labelUnearnedAmt.Font, FontStyle.Regular);
                        }
                        else
                        {
                            labelUnearnedAmt.ForeColor = Color.Firebrick;
                            labelUnearnedAmt.Font = new Font(labelUnearnedAmt.Font, FontStyle.Bold);
                        }
                    }
                }
                //labelInsLeft.Text=Lan.g(this,"Ins Left");
                //labelInsLeftAmt.Text="";//etc. Will be same for everyone
                Font fontBold = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Bold);
                //In the new way of doing it, they are all visible and calculated identically,
                //but the emphasis simply changes by slight renaming of labels
                //and by font size changes.
                if (Preference.GetBool(PreferenceName.BalancesDontSubtractIns))
                {
                    labelTotal.Text = Lan.g(this, "Balance");
                    labelTotalAmt.Font = fontBold;
                    labelTotalAmt.ForeColor = Color.Firebrick;
                    panelAgeLine.Visible = true;//verical line
                    labelInsEst.Text = Lan.g(this, "Ins Pending");
                    labelBalance.Text = Lan.g(this, "After Ins");
                    labelBalanceAmt.Font = this.Font;
                    labelBalanceAmt.ForeColor = Color.Black;
                }
                else
                {//this is more common
                    labelTotal.Text = Lan.g(this, "Total");
                    labelTotalAmt.Font = this.Font;
                    labelTotalAmt.ForeColor = Color.Black;
                    panelAgeLine.Visible = false;
                    labelInsEst.Text = Lan.g(this, "-InsEst");
                    labelBalance.Text = Lan.g(this, "=Est Bal");
                    labelBalanceAmt.Font = fontBold;
                    labelBalanceAmt.ForeColor = Color.Firebrick;
                    if (Preference.GetBool(PreferenceName.FuchsOptionsOn))
                    {
                        labelTotal.Text = Lan.g(this, "Balance");
                        labelBalance.Text = Lan.g(this, "=Owed Now");
                        labelTotalAmt.Font = fontBold;
                    }
                }
            }
            else
            {
                textOver90.Text = "";
                text61_90.Text = "";
                text31_60.Text = "";
                text0_30.Text = "";
                labelTotalAmt.Text = "";
                labelInsEstAmt.Text = "";
                labelBalanceAmt.Text = "";
                labelPatEstBalAmt.Text = "";
                labelUnearnedAmt.Text = "";
                //labelInsLeftAmt.Text="";
            }
        }

        ///<summary></summary>
        private void FillRepeatCharges()
        {
            //Uncollapse the first panel just in case. If this is left collapsed, setting visible properties on controls within it will have no effect
            splitContainerParent.Panel1Collapsed = false;
            gridRepeat.Visible = false;
            splitContainerRepChargesPP.Panel1Collapsed = true;
            if (PatCur == null)
            {
                return;
            }
            RepeatChargeList = _loadData.ArrRepeatCharges;
            if (RepeatChargeList.Length == 0)
            {
                return;
            }
            if (Preference.GetBool(PreferenceName.BillingUseBillingCycleDay))
            {
                gridRepeat.Title = Lan.g(gridRepeat, "Repeat Charges") + " - Billing Day " + PatCur.BillingCycleDay;
            }
            else
            {
                gridRepeat.Title = Lan.g(gridRepeat, "Repeat Charges");
            }
            splitContainerRepChargesPP.Panel1Collapsed = false;
            gridRepeat.Visible = true;
            gridRepeat.BeginUpdate();
            gridRepeat.Columns.Clear();
            ODGridColumn col = new ODGridColumn(Lan.g("TableRepeatCharges", "Description"), 150);
            gridRepeat.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableRepeatCharges", "Amount"), 60, textAlignment: HorizontalAlignment.Right);
            gridRepeat.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableRepeatCharges", "Start Date"), 70, textAlignment: HorizontalAlignment.Center);
            gridRepeat.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableRepeatCharges", "Stop Date"), 70, textAlignment: HorizontalAlignment.Center);
            gridRepeat.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableRepeatCharges", "Enabled"), 55, textAlignment: HorizontalAlignment.Center);
            gridRepeat.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableRepeatCharges", "Note"), 355);
            gridRepeat.Columns.Add(col);
            gridRepeat.Rows.Clear();
            UI.ODGridRow row;
            ProcedureCode procCode;
            for (int i = 0; i < RepeatChargeList.Length; i++)
            {
                row = new ODGridRow();
                procCode = ProcedureCodes.GetProcCode(RepeatChargeList[i].ProcCode);
                row.Cells.Add(procCode.Descript);
                row.Cells.Add(RepeatChargeList[i].ChargeAmt.ToString("F"));
                if (RepeatChargeList[i].DateStart.Year > 1880)
                {
                    row.Cells.Add(RepeatChargeList[i].DateStart.ToShortDateString());
                }
                else
                {
                    row.Cells.Add("");
                }
                if (RepeatChargeList[i].DateStop.Year > 1880)
                {
                    row.Cells.Add(RepeatChargeList[i].DateStop.ToShortDateString());
                }
                else
                {
                    row.Cells.Add("");
                }
                if (RepeatChargeList[i].IsEnabled)
                {
                    row.Cells.Add("X");
                }
                else
                {
                    row.Cells.Add("");
                }
                string note = "";
                if (!string.IsNullOrEmpty(RepeatChargeList[i].Npi))
                {
                    note += "NPI=" + RepeatChargeList[i].Npi + " ";
                }
                if (!string.IsNullOrEmpty(RepeatChargeList[i].ErxAccountId))
                {
                    note += "ErxAccountId=" + RepeatChargeList[i].ErxAccountId + " ";
                }
                if (!string.IsNullOrEmpty(RepeatChargeList[i].ProviderName))
                {
                    note += RepeatChargeList[i].ProviderName + " ";
                }
                note += RepeatChargeList[i].Note;
                row.Cells.Add(note);
                gridRepeat.Rows.Add(row);
            }
            gridRepeat.EndUpdate();
        }

        private void FillPaymentPlans()
        {
            PPBalanceTotal = 0;
            //Uncollapse the first panel just in case. If this is left collapsed, setting visible properties on controls within it will have no effect
            splitContainerParent.Panel1Collapsed = false;
            gridPayPlan.Visible = false;
            splitContainerRepChargesPP.Panel2Collapsed = true;
            if (PatCur == null)
            {
                return;
            }
            DataTable table = DataSetMain.Tables["payplan"];
            if (table.Rows.OfType<DataRow>().Count(x => PIn.Long(x["Guarantor"].ToString()) == PatCur.PatNum
                 || PIn.Long(x["PatNum"].ToString()) == PatCur.PatNum) == 0 && !_isSelectingFamily) //if we are looking at the entire family, show all the payplans 
            {
                return;
            }
            //do not hide payment plans that still have a balance when not on v2
            if (!checkShowCompletePayPlans.Checked)
            { //Hide the payment plans grid if there are no payment plans currently visible.
                bool existsOpenPayPlan = false;
                for (int i = 0; i < table.Rows.Count; i++)
                { //for every payment plan
                    if (DoShowPayPlan(checkShowCompletePayPlans.Checked, PIn.Bool(table.Rows[i]["IsClosed"].ToString()),
                        PIn.Double(table.Rows[i]["balance"].ToString())))
                    {
                        existsOpenPayPlan = true;
                        break; //break
                    }
                }
                if (!existsOpenPayPlan)
                {
                    return;//no need to do anything else.
                }
            }
            splitContainerRepChargesPP.Panel2Collapsed = false;
            gridPayPlan.Visible = true;
            gridPayPlan.BeginUpdate();
            gridPayPlan.Columns.Clear();
            ODGridColumn col = new ODGridColumn(Lan.g("TablePaymentPlans", "Date"), 65);
            gridPayPlan.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TablePaymentPlans", "Guarantor"), 100);
            gridPayPlan.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TablePaymentPlans", "Patient"), 100);
            gridPayPlan.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TablePaymentPlans", "Type"), 30, textAlignment: HorizontalAlignment.Center);
            gridPayPlan.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TablePaymentPlans", "Category"), 60, textAlignment: HorizontalAlignment.Center);
            gridPayPlan.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TablePaymentPlans", "Principal"), 60, textAlignment: HorizontalAlignment.Right);
            gridPayPlan.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TablePaymentPlans", "Total Cost"), 60, textAlignment: HorizontalAlignment.Right);
            gridPayPlan.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TablePaymentPlans", "Paid"), 60, textAlignment: HorizontalAlignment.Right);
            gridPayPlan.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TablePaymentPlans", "PrincPaid"), 60, textAlignment: HorizontalAlignment.Right);
            gridPayPlan.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TablePaymentPlans", "Balance"), 60, textAlignment: HorizontalAlignment.Right);
            gridPayPlan.Columns.Add(col);
            if (Preference.GetBool(PreferenceName.PayPlanHideDueNow))
            {
                col = new ODGridColumn("Closed", 60, textAlignment: HorizontalAlignment.Center);
            }
            else
            {
                col = new ODGridColumn(Lan.g("TablePaymentPlans", "Due Now"), 60, textAlignment: HorizontalAlignment.Right);
            }
            gridPayPlan.Columns.Add(col);
            gridPayPlan.Rows.Clear();
            UI.ODGridRow row;
            UI.ODGridCell cell;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (!DoShowPayPlan(checkShowCompletePayPlans.Checked, PIn.Bool(table.Rows[i]["IsClosed"].ToString()),
                    PIn.Double(table.Rows[i]["balance"].ToString())))
                {
                    continue;//hide
                }
                row = new ODGridRow();
                row.Cells.Add(table.Rows[i]["date"].ToString());
                if (table.Rows[i]["InstallmentPlanNum"].ToString() != "0" && table.Rows[i]["PatNum"].ToString() != PatCur.Guarantor.ToString())
                {//Installment plan and not on guar
                    cell = new ODGridCell(((string)"Invalid Guarantor"));
                    cell.Bold = true;
                    cell.ColorText = Color.Red;
                }
                else
                {
                    cell = new ODGridCell(table.Rows[i]["guarantor"].ToString());
                }
                row.Cells.Add(cell);
                row.Cells.Add(table.Rows[i]["patient"].ToString());
                row.Cells.Add(table.Rows[i]["type"].ToString());
                long planCat = PIn.Long(table.Rows[i]["PlanCategory"].ToString());
                if (planCat == 0)
                {
                    row.Cells.Add(Lan.g(this, "None"));
                }
                else
                {
                    row.Cells.Add(Defs.GetDef(DefinitionCategory.PayPlanCategories, planCat).Description);
                }
                row.Cells.Add(table.Rows[i]["principal"].ToString());
                row.Cells.Add(table.Rows[i]["totalCost"].ToString());
                row.Cells.Add(table.Rows[i]["paid"].ToString());
                row.Cells.Add(table.Rows[i]["princPaid"].ToString());
                row.Cells.Add(table.Rows[i]["balance"].ToString());
                if (table.Rows[i]["IsClosed"].ToString() == "1" && Preference.GetInt(PreferenceName.PayPlansVersion) == 2)
                {
                    cell = new ODGridCell(Lan.g(this, "Closed"));
                    row.ColorText = Color.Gray;
                }
                else if (Preference.GetBool(PreferenceName.PayPlanHideDueNow))
                {//pref can only be enabled when PayPlansVersion == 2.
                    cell = new ODGridCell("");
                }
                else
                { //they aren't hiding the "Due Now" cell text.
                    cell = new ODGridCell(table.Rows[i]["due"].ToString());
                    //Only color the due now red and bold in version 1 and 3 of payplans.
                    if (Preference.GetInt(PreferenceName.PayPlansVersion).In((int)PayPlanVersions.DoNotAge, (int)PayPlanVersions.AgeCreditsOnly, (int)PayPlanVersions.NoCharges))
                    {
                        if (table.Rows[i]["type"].ToString() != "Ins")
                        {
                            cell.Bold = true;
                            cell.ColorText = Color.Red;
                        }
                    }
                }
                row.Cells.Add(cell);
                row.Tag = table.Rows[i];
                gridPayPlan.Rows.Add(row);
                PPBalanceTotal += (Convert.ToDecimal(PIn.Double(table.Rows[i]["balance"].ToString())));
            }
            gridPayPlan.EndUpdate();
            if (Preference.GetBool(PreferenceName.FuchsOptionsOn))
            {
                panelTotalOwes.Top = 1;
                labelTotalPtOwes.Text = (PPBalanceTotal + (decimal)FamCur.ListPats[0].BalTotal - (decimal)FamCur.ListPats[0].InsEst).ToString("F");
            }
        }

        ///<summary>Returns true if the payment plan should be displayed.</summary>
        private bool DoShowPayPlan(bool doShowCompletedPlans, bool isClosed, double balance)
        {
            if (doShowCompletedPlans)
            {
                return true;
            }
            //do not hide payment plans that still have a balance when not on v2
            bool doShowClosedPlansWithBalance = (Preference.GetInt(PreferenceName.PayPlansVersion) != (int)PayPlanVersions.AgeCreditsAndDebits);
            return !isClosed
                        || (doShowClosedPlansWithBalance && !balance.IsEqual(0)); //Or the payment plan has a balance
        }

        /// <summary>Fills the commlog grid on this form.  It does not refresh the data from the database.</summary>
        private void FillComm()
        {
            if (DataSetMain == null)
            {
                gridComm.BeginUpdate();
                gridComm.Rows.Clear();
                gridComm.EndUpdate();
                return;
            }
            gridComm.BeginUpdate();
            gridComm.Columns.Clear();
            ODGridColumn col = new ODGridColumn(Lan.g("TableCommLogAccount", "Date"), 70);
            gridComm.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableCommLogAccount", "Time"), 42);//,HorizontalAlignment.Right);
            gridComm.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableCommLogAccount", "Name"), 80);
            gridComm.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableCommLogAccount", "Type"), 80);
            gridComm.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableCommLogAccount", "Mode"), 55);
            gridComm.Columns.Add(col);
            //col = new ODGridColumn(Lan.g("TableCommLogAccount", "Sent/Recd"), 75);
            //gridComm.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableCommLogAccount", "Note"), 455);
            gridComm.Columns.Add(col);
            gridComm.Rows.Clear();
            OpenDental.UI.ODGridRow row;
            DataTable table = DataSetMain.Tables["Commlog"];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                //Skip commlog entries which belong to other family members per user option.
                if (!this.checkShowFamilyComm.Checked                                       //show family not checked
                    && !_isSelectingFamily                                                                  //family not selected
                    && table.Rows[i]["PatNum"].ToString() != PatCur.PatNum.ToString()   //not this patient
                    && table.Rows[i]["FormPatNum"].ToString() == "0")               //not a questionnaire (FormPat)
                {
                    continue;
                }

                row = new ODGridRow();
                int argbColor = PIn.Int(table.Rows[i]["colorText"].ToString());//Convert to int. If blank or 0, will use default color.
                if (argbColor != Color.Empty.ToArgb())
                {//A color was set for this commlog type
                    row.ColorText = Color.FromArgb(argbColor);
                }
                row.Cells.Add(table.Rows[i]["commDate"].ToString());
                row.Cells.Add(table.Rows[i]["commTime"].ToString());
                if (_isSelectingFamily)
                {
                    row.Cells.Add(table.Rows[i]["patName"].ToString());
                }
                else
                {//one patient
                    if (table.Rows[i]["PatNum"].ToString() == PatCur.PatNum.ToString())
                    {//if this patient
                        row.Cells.Add("");
                    }
                    else
                    {//other patient
                        row.Cells.Add(table.Rows[i]["patName"].ToString());
                    }
                }
                row.Cells.Add(table.Rows[i]["commType"].ToString());
                row.Cells.Add(table.Rows[i]["mode"].ToString());
                //row.Cells.Add(table.Rows[i]["sentOrReceived"].ToString());
                row.Cells.Add(table.Rows[i]["Note"].ToString());
                row.Tag = i;
                gridComm.Rows.Add(row);
            }
            gridComm.EndUpdate();
            gridComm.ScrollToEnd();
        }

        private void FillMain()
        {
            gridAccount.BeginUpdate();
            gridAccount.Columns.Clear();
            ODGridColumn col;
            fieldsForMainGrid = DisplayFields.GetForCategory(DisplayFieldCategory.AccountModule);
            if (!Preferences.HasClinicsEnabled)
            {
                //remove clinics from displayfields if clinics are disabled
                fieldsForMainGrid.RemoveAll(x => x.InternalName.ToLower().Contains("clinic"));
            }
            HorizontalAlignment align;
            for (int i = 0; i < fieldsForMainGrid.Count; i++)
            {
                align = HorizontalAlignment.Left;
                if (fieldsForMainGrid[i].InternalName == "Charges"
                    || fieldsForMainGrid[i].InternalName == "Credits"
                    || fieldsForMainGrid[i].InternalName == "Balance")
                {
                    align = HorizontalAlignment.Right;
                }
                if (fieldsForMainGrid[i].InternalName == "Signed")
                {
                    align = HorizontalAlignment.Center;
                }
                if (fieldsForMainGrid[i].Description == "")
                {
                    col = new ODGridColumn(fieldsForMainGrid[i].InternalName, fieldsForMainGrid[i].ColumnWidth, textAlignment: align);
                }
                else
                {
                    col = new ODGridColumn(fieldsForMainGrid[i].Description, fieldsForMainGrid[i].ColumnWidth, textAlignment: align);
                }
                gridAccount.Columns.Add(col);
            }
            if (gridAccount.Columns.Sum(x => x.Width) > gridAccount.Width)
            {
                gridAccount.HScrollVisible = true;
            }
            else
            {
                gridAccount.HScrollVisible = false;
            }
            gridAccount.Rows.Clear();
            ODGridRow row;
            DataTable table = null;
            if (PatCur == null)
            {
                table = new DataTable();
            }
            else
            {
                table = DataSetMain.Tables["account"];
            }
            for (int i = 0; i < table.Rows.Count; i++)
            {
                row = new ODGridRow();
                for (int f = 0; f < fieldsForMainGrid.Count; f++)
                {
                    switch (fieldsForMainGrid[f].InternalName)
                    {
                        case "Date":
                            row.Cells.Add(table.Rows[i]["date"].ToString());
                            break;
                        case "Patient":
                            row.Cells.Add(table.Rows[i]["patient"].ToString());
                            break;
                        case "Prov":
                            row.Cells.Add(table.Rows[i]["prov"].ToString());
                            break;
                        case "Clinic":
                            row.Cells.Add(Clinics.GetAbbr(PIn.Long(table.Rows[i]["ClinicNum"].ToString())));
                            break;
                        case "ClinicDesc":
                            row.Cells.Add(Clinics.GetDesc(PIn.Long(table.Rows[i]["ClinicNum"].ToString())));
                            break;
                        case "Code":
                            row.Cells.Add(table.Rows[i]["ProcCode"].ToString());
                            break;
                        case "Tth":
                            row.Cells.Add(table.Rows[i]["tth"].ToString());
                            break;
                        case "Description":
                            row.Cells.Add(table.Rows[i]["description"].ToString());
                            break;
                        case "Charges":
                            row.Cells.Add(table.Rows[i]["charges"].ToString());
                            break;
                        case "Credits":
                            row.Cells.Add(table.Rows[i]["credits"].ToString());
                            break;
                        case "Balance":
                            row.Cells.Add(table.Rows[i]["balance"].ToString());
                            break;
                        case "Signed":
                            row.Cells.Add(table.Rows[i]["signed"].ToString());
                            break;
                        case "Abbr": //procedure abbreviation
                            if (!String.IsNullOrEmpty(table.Rows[i]["AbbrDesc"].ToString()))
                            {
                                row.Cells.Add(table.Rows[i]["AbbrDesc"].ToString());
                            }
                            else
                            {
                                row.Cells.Add("");
                            }
                            break;
                        default:
                            row.Cells.Add("");
                            break;
                    }
                }
                row.ColorText = Color.FromArgb(PIn.Int(table.Rows[i]["colorText"].ToString()));
                if (i == table.Rows.Count - 1//last row
                    || (DateTime)table.Rows[i]["DateTime"] != (DateTime)table.Rows[i + 1]["DateTime"])
                {
                    row.ColorLborder = Color.Black;
                }
                gridAccount.Rows.Add(row);
            }
            gridAccount.EndUpdate();
            if (Actscrollval == 0)
            {
                gridAccount.ScrollToEnd();
            }
            else
            {
                gridAccount.ScrollValue = Actscrollval;
                Actscrollval = 0;
            }
        }

        private void FillSummary()
        {
            textFamPriMax.Text = "";
            textFamPriDed.Text = "";
            textFamSecMax.Text = "";
            textFamSecDed.Text = "";
            textPriMax.Text = "";
            textPriDed.Text = "";
            textPriDedRem.Text = "";
            textPriUsed.Text = "";
            textPriPend.Text = "";
            textPriRem.Text = "";
            textSecMax.Text = "";
            textSecDed.Text = "";
            textSecDedRem.Text = "";
            textSecUsed.Text = "";
            textSecPend.Text = "";
            textSecRem.Text = "";
            if (PatCur == null)
            {
                return;
            }
            double maxFam = 0;
            double maxInd = 0;
            double ded = 0;
            double dedFam = 0;
            double dedRem = 0;
            double remain = 0;
            double pend = 0;
            double used = 0;
            InsPlan PlanCur;
            InsSub SubCur;
            List<InsSub> subList = _loadData.ListInsSubs;
            List<InsPlan> InsPlanList = _loadData.ListInsPlans;
            List<PatPlan> PatPlanList = _loadData.ListPatPlans;
            List<Benefit> BenefitList = _loadData.ListBenefits;
            List<Claim> ClaimList = _loadData.ListClaims;
            List<ClaimProcHist> HistList = _loadData.HistList;
            if (PatPlanList.Count > 0)
            {
                SubCur = InsSubs.GetSub(PatPlanList[0].InsSubNum, subList);
                PlanCur = InsPlans.GetPlan(SubCur.PlanNum, InsPlanList);
                pend = InsPlans.GetPendingDisplay(HistList, DateTime.Today, PlanCur, PatPlanList[0].PatPlanNum, -1, PatCur.PatNum, PatPlanList[0].InsSubNum, BenefitList);
                used = InsPlans.GetInsUsedDisplay(HistList, DateTime.Today, PlanCur.PlanNum, PatPlanList[0].PatPlanNum, -1, InsPlanList, BenefitList, PatCur.PatNum, PatPlanList[0].InsSubNum);
                textPriPend.Text = pend.ToString("F");
                textPriUsed.Text = used.ToString("F");
                maxFam = Benefits.GetAnnualMaxDisplay(BenefitList, PlanCur.PlanNum, PatPlanList[0].PatPlanNum, true);
                maxInd = Benefits.GetAnnualMaxDisplay(BenefitList, PlanCur.PlanNum, PatPlanList[0].PatPlanNum, false);
                if (maxFam == -1)
                {
                    textFamPriMax.Text = "";
                }
                else
                {
                    textFamPriMax.Text = maxFam.ToString("F");
                }
                if (maxInd == -1)
                {//if annual max is blank
                    textPriMax.Text = "";
                    textPriRem.Text = "";
                }
                else
                {
                    remain = maxInd - used - pend;
                    if (remain < 0)
                    {
                        remain = 0;
                    }
                    //textFamPriMax.Text=max.ToString("F");
                    textPriMax.Text = maxInd.ToString("F");
                    textPriRem.Text = remain.ToString("F");
                }
                //deductible:
                ded = Benefits.GetDeductGeneralDisplay(BenefitList, PlanCur.PlanNum, PatPlanList[0].PatPlanNum, BenefitCoverageLevel.Individual);
                dedFam = Benefits.GetDeductGeneralDisplay(BenefitList, PlanCur.PlanNum, PatPlanList[0].PatPlanNum, BenefitCoverageLevel.Family);
                if (ded != -1)
                {
                    textPriDed.Text = ded.ToString("F");
                    dedRem = InsPlans.GetDedRemainDisplay(HistList, DateTime.Today, PlanCur.PlanNum, PatPlanList[0].PatPlanNum, -1, InsPlanList, PatCur.PatNum, ded, dedFam);
                    textPriDedRem.Text = dedRem.ToString("F");
                }
                if (dedFam != -1)
                {
                    textFamPriDed.Text = dedFam.ToString("F");
                }
            }
            if (PatPlanList.Count > 1)
            {
                SubCur = InsSubs.GetSub(PatPlanList[1].InsSubNum, subList);
                PlanCur = InsPlans.GetPlan(SubCur.PlanNum, InsPlanList);
                pend = InsPlans.GetPendingDisplay(HistList, DateTime.Today, PlanCur, PatPlanList[1].PatPlanNum, -1, PatCur.PatNum, PatPlanList[1].InsSubNum, BenefitList);
                textSecPend.Text = pend.ToString("F");
                used = InsPlans.GetInsUsedDisplay(HistList, DateTime.Today, PlanCur.PlanNum, PatPlanList[1].PatPlanNum, -1, InsPlanList, BenefitList, PatCur.PatNum, PatPlanList[1].InsSubNum);
                textSecUsed.Text = used.ToString("F");
                //max=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum);
                maxFam = Benefits.GetAnnualMaxDisplay(BenefitList, PlanCur.PlanNum, PatPlanList[1].PatPlanNum, true);
                maxInd = Benefits.GetAnnualMaxDisplay(BenefitList, PlanCur.PlanNum, PatPlanList[1].PatPlanNum, false);
                if (maxFam == -1)
                {
                    textFamSecMax.Text = "";
                }
                else
                {
                    textFamSecMax.Text = maxFam.ToString("F");
                }
                if (maxInd == -1)
                {//if annual max is blank
                    textSecMax.Text = "";
                    textSecRem.Text = "";
                }
                else
                {
                    remain = maxInd - used - pend;
                    if (remain < 0)
                    {
                        remain = 0;
                    }
                    //textFamSecMax.Text=max.ToString("F");
                    textSecMax.Text = maxInd.ToString("F");
                    textSecRem.Text = remain.ToString("F");
                }
                //deductible:
                ded = Benefits.GetDeductGeneralDisplay(BenefitList, PlanCur.PlanNum, PatPlanList[1].PatPlanNum, BenefitCoverageLevel.Individual);
                dedFam = Benefits.GetDeductGeneralDisplay(BenefitList, PlanCur.PlanNum, PatPlanList[1].PatPlanNum, BenefitCoverageLevel.Family);
                if (ded != -1)
                {
                    textSecDed.Text = ded.ToString("F");
                    dedRem = InsPlans.GetDedRemainDisplay(HistList, DateTime.Today, PlanCur.PlanNum, PatPlanList[1].PatPlanNum, -1, InsPlanList, PatCur.PatNum, ded, dedFam);
                    textSecDedRem.Text = dedRem.ToString("F");
                }
                if (dedFam != -1)
                {
                    textFamSecDed.Text = dedFam.ToString("F");
                }
            }
        }

        private void FillPatInfo()
        {
            if (PatCur == null)
            {
                patientInfoGrid.BeginUpdate();
                patientInfoGrid.Rows.Clear();
                patientInfoGrid.Columns.Clear();
                patientInfoGrid.EndUpdate();
                return;
            }
            patientInfoGrid.BeginUpdate();
            patientInfoGrid.Columns.Clear();
            ODGridColumn col = new ODGridColumn("", 80);
            patientInfoGrid.Columns.Add(col);
            col = new ODGridColumn("", 150);
            patientInfoGrid.Columns.Add(col);
            patientInfoGrid.Rows.Clear();
            ODGridRow row;
            _patInfoDisplayFields = DisplayFields.GetForCategory(DisplayFieldCategory.AccountPatientInformation);
            for (int f = 0; f < _patInfoDisplayFields.Count; f++)
            {
                row = new ODGridRow();
                if (_patInfoDisplayFields[f].Description == "")
                {
                    if (_patInfoDisplayFields[f].InternalName == "PatFields")
                    {
                        //don't add a cell
                    }
                    else
                    {
                        row.Cells.Add(_patInfoDisplayFields[f].InternalName);
                    }
                }
                else
                {
                    if (_patInfoDisplayFields[f].InternalName == "PatFields")
                    {
                        //don't add a cell
                    }
                    else
                    {
                        row.Cells.Add(_patInfoDisplayFields[f].Description);
                    }
                }
                switch (_patInfoDisplayFields[f].InternalName)
                {
                    case "Billing Type":
                        row.Cells.Add(Defs.GetName(DefinitionCategory.BillingTypes, PatCur.BillingType));
                        break;
                    case "PatFields":
                        PatFieldL.AddPatFieldsToGrid(patientInfoGrid, _patFieldList.ToList(), FieldLocations.Account, _loadData.ListFieldDefLinksAcct);
                        break;
                }
                if (_patInfoDisplayFields[f].InternalName == "PatFields")
                {
                    //don't add the row here
                }
                else
                {
                    patientInfoGrid.Rows.Add(row);
                }
            }
            patientInfoGrid.EndUpdate();
        }

        #region Ortho Case
        private void FillOrtho(bool doCalculateFirstDate = true)
        {
            if (PatCur == null)
            {
                return;
            }
            gridOrtho.BeginUpdate();
            gridOrtho.Columns.Clear();
            gridOrtho.Columns.Add(new ODGridColumn("", (gridOrtho.Width / 2) - 20));//,HorizontalAlignment.Right));
            gridOrtho.Columns.Add(new ODGridColumn("", (gridOrtho.Width / 2) + 20));
            gridOrtho.Rows.Clear();
            ODGridRow row = new ODGridRow();
            //Insurance Information
            //PriClaimType
            List<PatPlan> listPatPlans = _loadData.ListPatPlans;
            if (listPatPlans.Count == 0)
            {
                row = new ODGridRow();
                row.Cells.Add("");
                row.Cells.Add(Lan.g(this, "Patient has no insurance."));
                gridOrtho.Rows.Add(row);
            }
            else
            {
                List<Definition> listDefs = Definition.GetByCategory(DefinitionCategory.MiscColors);;
                for (int i = 0; i < listPatPlans.Count; i++)
                {
                    PatPlan patPlanCur = listPatPlans[i];
                    InsSub insSub = InsSubs.GetSub(patPlanCur.InsSubNum, _loadData.ListInsSubs);
                    InsPlan insPlanCur = InsPlans.GetPlan(insSub.PlanNum, _loadData.ListInsPlans);
                    string carrierNameCur = Carriers.GetCarrier(insPlanCur.CarrierNum).CarrierName;
                    string subIDCur = insSub.SubscriberID;
                    row = new ODGridRow();
                    OrthoPat orthoPatCur = new OrthoPat()
                    {
                        InsPlan = insPlanCur,
                        PatPlan = patPlanCur,
                        CarrierName = carrierNameCur,
                        DefaultFee = insPlanCur.OrthoAutoFeeBilled,
                        SubID = subIDCur
                    };
                    if (i == listPatPlans.Count - 1)
                    { //last row in the insurance info section
                        row.ColorLborder = Color.Black;
                    }
                    row.ColorBackG = listDefs[0].Color; //same logic as family module insurance colors.
                    switch (i)
                    {
                        case 0: //primary
                            row.Cells.Add(Lan.g(this, "Primary Ins"));
                            break;
                        case 1: //secondary
                            row.Cells.Add(Lan.g(this, "Secondary Ins"));
                            break;
                        case 2: //tertiary
                            row.Cells.Add(Lan.g(this, "Tertiary Ins"));
                            break;
                        default: //other
                            row.Cells.Add(Lan.g(this, "Other Ins"));
                            break;
                    }
                    row.Cells.Add("");
                    row.Bold = true;
                    row.Tag = orthoPatCur;
                    gridOrtho.Rows.Add(row);
                    //claimtype
                    row = new ODGridRow();
                    row.Cells.Add(Lan.g(this, "ClaimType"));
                    if (insPlanCur == null)
                    {
                        row.Cells.Add("");
                    }
                    else
                    {
                        row.Cells.Add(insPlanCur.OrthoType.ToString());
                    }
                    row.Tag = orthoPatCur;
                    gridOrtho.Rows.Add(row);
                    //Only show for initialPlusPeriodic claimtype.
                    if (insPlanCur.OrthoType == OrthoClaimType.InitialPlusPeriodic)
                    {
                        //Frequency
                        row = new ODGridRow();
                        row.Cells.Add(Lan.g(this, "Frequency"));
                        row.Cells.Add(insPlanCur.OrthoAutoProcFreq.ToString());
                        row.Tag = orthoPatCur;
                        gridOrtho.Rows.Add(row);
                        //Fee
                        row = new ODGridRow();
                        row.Cells.Add(Lan.g(this, "FeeBilled"));
                        row.Cells.Add(patPlanCur.OrthoAutoFeeBilledOverride == -1 ? POut.Double(insPlanCur.OrthoAutoFeeBilled) : POut.Double(patPlanCur.OrthoAutoFeeBilledOverride));
                        row.Tag = orthoPatCur;
                        gridOrtho.Rows.Add(row);
                    }
                    //Last Claim Date
                    row = new ODGridRow();
                    DateTime dateLast;
                    if (!_loadData.DictDateLastOrthoClaims.TryGetValue(patPlanCur.PatPlanNum, out dateLast))
                    {
                        dateLast = Claims.GetDateLastOrthoClaim(patPlanCur, insPlanCur.OrthoType);
                    }
                    row.Cells.Add(Lan.g(this, "LastClaim"));
                    row.Cells.Add(dateLast == null || dateLast.Date == DateTime.MinValue.Date ? Lan.g(this, "None Sent") : dateLast.ToShortDateString());
                    row.Tag = orthoPatCur;
                    gridOrtho.Rows.Add(row);
                    //NextClaimDate - Only show for initialPlusPeriodic claimtype.
                    if (insPlanCur.OrthoType == OrthoClaimType.InitialPlusPeriodic)
                    {
                        row = new ODGridRow();
                        row.Cells.Add(Lan.g(this, "NextClaim"));
                        row.Cells.Add(patPlanCur.OrthoAutoNextClaimDate.Date == DateTime.MinValue.Date ? Lan.g(this, "Stopped") : patPlanCur.OrthoAutoNextClaimDate.ToShortDateString());
                        row.Tag = orthoPatCur;
                        gridOrtho.Rows.Add(row);
                    }
                }
            }
            //Pat Ortho Info Title
            row = new ODGridRow();
            row.Cells.Add(Lan.g(this, "Pat Ortho Info"));
            row.Cells.Add("");
            row.ColorBackG = Color.LightCyan;
            row.Bold = true;
            row.ColorLborder = Color.Black;
            gridOrtho.Rows.Add(row);
            //OrthoAutoProc Freq
            if (doCalculateFirstDate)
            {
                _loadData.FirstOrthoProcDate = Procedures.GetFirstOrthoProcDate(PatientNoteCur);
            }
            DateTime firstOrthoProcDate = _loadData.FirstOrthoProcDate;
            if (firstOrthoProcDate != DateTime.MinValue)
            {
                row = new ODGridRow();
                row.Cells.Add(Lan.g(this, "Total Tx Time")); //Number of Years/Months/Days since the first ortho procedure on this account
                DateSpan dateSpan = new DateSpan(firstOrthoProcDate, DateTimeOD.Today);
                string strDateDiff = "";
                if (dateSpan.YearsDiff != 0)
                {
                    strDateDiff += dateSpan.YearsDiff + " " + Lan.g(this, "year" + (dateSpan.YearsDiff == 1 ? "" : "s"));
                }
                if (dateSpan.MonthsDiff != 0)
                {
                    if (strDateDiff != "")
                    {
                        strDateDiff += ", ";
                    }
                    strDateDiff += dateSpan.MonthsDiff + " " + Lan.g(this, "month" + (dateSpan.MonthsDiff == 1 ? "" : "s"));
                }
                if (dateSpan.DaysDiff != 0 || strDateDiff == "")
                {
                    if (strDateDiff != "")
                    {
                        strDateDiff += ", ";
                    }
                    strDateDiff += dateSpan.DaysDiff + " " + Lan.g(this, "day" + (dateSpan.DaysDiff == 1 ? "" : "s"));
                }
                row.Cells.Add(strDateDiff);
                gridOrtho.Rows.Add(row);
                //Date Start
                row = new ODGridRow();
                row.Cells.Add(Lan.g(this, "Date Start")); //Date of the first ortho procedure on this account
                row.Cells.Add(firstOrthoProcDate.ToShortDateString());
                gridOrtho.Rows.Add(row);
                //Tx Months Total
                row = new ODGridRow();
                row.Cells.Add(Lan.g(this, "Tx Months Total")); //this patient's OrthoClaimMonthsTreatment, or the practice default if 0.
                int txMonthsTotal = (PatientNoteCur.OrthoMonthsTreatOverride == -1 ? Preference.GetByte(PreferenceName.OrthoDefaultMonthsTreat) : PatientNoteCur.OrthoMonthsTreatOverride);
                row.Cells.Add(txMonthsTotal.ToString());
                gridOrtho.Rows.Add(row);
                //Months in treatment
                row = new ODGridRow();
                int txTimeInMonths = (dateSpan.YearsDiff * 12) + dateSpan.MonthsDiff + (dateSpan.DaysDiff < 15 ? 0 : 1);
                row.Cells.Add(Lan.g(this, "Months in Treatment"));
                row.Cells.Add(txTimeInMonths.ToString());
                gridOrtho.Rows.Add(row);
                //Months Rem
                row = new ODGridRow();
                row.Cells.Add(Lan.g(this, "Months Rem")); //Months Total - Total Tx Time
                row.Cells.Add(Math.Max(0, txMonthsTotal - txTimeInMonths).ToString());
                gridOrtho.Rows.Add(row);
            }
            else
            { //no ortho procedures charted for this patient.
                row = new ODGridRow();
                row.Cells.Add("");
                row.Cells.Add(Lan.g(this, "No ortho procedures charted."));
                gridOrtho.Rows.Add(row);
            }
            gridOrtho.EndUpdate();
        }

        private void butEditOrthoPlacement_Click(object sender, EventArgs e)
        {
            DateTime dateOrthoPlacement;
            try
            {
                dateOrthoPlacement = PIn.Date(textDateOrthoPlacement.Text);
            }
            catch
            {
                MsgBox.Show(this, "Invalid date.");
                return;
            }
            PatientNoteCur.DateOrthoPlacementOverride = dateOrthoPlacement;
            PatientNotes.Update(PatientNoteCur, PatCur.Guarantor);
            FillOrtho();
        }

        private void butOrthoEditMonthsTreat_Click(object sender, EventArgs e)
        {
            int txMonths;
            try
            {
                txMonths = PIn.Byte(textOrthoMonthsTreat.Text);
            }
            catch
            {
                MsgBox.Show(this, "Please enter a number between 0 and 255.");
                return;
            }
            PatientNoteCur.OrthoMonthsTreatOverride = txMonths;
            PatientNotes.Update(PatientNoteCur, PatCur.Guarantor);
            FillOrtho();
        }

        private void butOrthoDefaultPlacement_Click(object sender, EventArgs e)
        {
            PatientNoteCur.DateOrthoPlacementOverride = DateTime.MinValue;
            PatientNotes.Update(PatientNoteCur, PatCur.Guarantor);
            FillOrtho();
        }

        private void butOrthoDefaultMonthsTreat_Click(object sender, EventArgs e)
        {
            //Setting OrthoMonthsTreatOverride locks this value into place just in case it the pref changes down the road.
            PatientNoteCur.OrthoMonthsTreatOverride = Preference.GetByte(PreferenceName.OrthoDefaultMonthsTreat);
            PatientNotes.Update(PatientNoteCur, PatCur.Guarantor);
            FillOrtho();
        }

        private void gridOrtho_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (gridOrtho.Rows[e.Row].Tag == null || gridOrtho.Rows[e.Row].Tag.GetType() != typeof(OrthoPat))
            {
                return;
            }
            OrthoPat orthoPatCur = (OrthoPat)gridOrtho.Rows[e.Row].Tag;
            if (orthoPatCur.InsPlan.OrthoType != OrthoClaimType.InitialPlusPeriodic)
            {
                MsgBox.Show(this, "To view this setup window, the insurance plan must be set to have an Ortho Claim Type of Initial Plus Periodic.");
                return;
            }
            FormOrthoPat FormOP = new FormOrthoPat(orthoPatCur.PatPlan, orthoPatCur.InsPlan, orthoPatCur.CarrierName, orthoPatCur.SubID, orthoPatCur.DefaultFee);
            FormOP.ShowDialog();
            if (FormOP.DialogResult == DialogResult.OK)
            {
                PatPlans.Update(orthoPatCur.PatPlan);
                FillOrtho();
            }
        }

        private struct OrthoPat
        {
            public InsPlan InsPlan;
            public PatPlan PatPlan;
            public string CarrierName;
            public string SubID;
            public double DefaultFee;
        }
        #endregion

        private void gridAccount_CellClick(object sender, OpenDental.UI.ODGridClickEventArgs e)
        {
            DataTable table = DataSetMain.Tables["account"];
            //this seems to fire after a doubleclick, so this prevents error:
            if (e.Row >= table.Rows.Count)
            {
                return;
            }
            gridPayPlan.SetSelected(false);
            foreach (int rowNum in gridAccount.SelectedIndices)
            {
                if (table.Rows[rowNum]["PayPlanNum"].ToString() != "0")
                {
                    for (int i = 0; i < gridPayPlan.Rows.Count; i++)
                    {
                        if (((DataRow)(gridPayPlan.Rows[i].Tag))["PayPlanNum"].ToString() == table.Rows[rowNum]["PayPlanNum"].ToString())
                        {
                            gridPayPlan.SetSelected(i, true);
                        }
                    }
                    if (table.Rows[rowNum]["procsOnObj"].ToString() != "0")
                    {
                        for (int i = 0; i < table.Rows.Count; i++)
                        {//loop through all rows
                            if (table.Rows[i]["ProcNum"].ToString() == table.Rows[rowNum]["procsOnObj"].ToString())
                            {
                                gridAccount.SetSelected(i, true);//select the pertinent procedure
                                break;
                            }
                        }
                    }
                }
            }
            if (ViewingInRecall)
            {
                return;
            }
            foreach (int rowNum in gridAccount.SelectedIndices)
            {
                DataRow rowCur = table.Rows[rowNum];
                if (rowCur["ClaimNum"].ToString() != "0")
                {//claims and claimpayments
                 //Since we removed all selected items above, we need to reselect the claim the user just clicked on at the very least.
                 //The "procsOnObj" column is going to be a comma delimited list of ProcNums associated to the corresponding claim.
                    List<string> procsOnClaim = rowCur["procsOnObj"].ToString().Split(',').ToList();
                    //Loop through the entire table and select any rows that are related to this claim (payments) while keeping track of their related ProcNums.
                    for (int i = 0; i < table.Rows.Count; i++)
                    {//loop through all rows
                        if (table.Rows[i]["ClaimNum"].ToString() == rowCur["ClaimNum"].ToString())
                        {
                            gridAccount.SetSelected(i, true);//for the claim payments
                            procsOnClaim.AddRange(table.Rows[i]["procsOnObj"].ToString().Split(','));
                        }
                    }
                    //Other software companies allow claims to be created with no procedures attached.
                    //This would cause "procsOnObj" to contain a ProcNum of '0' which the following loop would then select seemingly random rows (any w/ ProcNum=0)
                    //Therefore, we need to specifically remove any entries of '0' from our procsOnClaim list before looping through it.
                    procsOnClaim.RemoveAll(x => x == "0");
                    //Loop through the table again in order to select any related procedures.
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        if (procsOnClaim.Contains(table.Rows[i]["ProcNum"].ToString()))
                        {
                            gridAccount.SetSelected(i, true);
                        }
                    }
                }
                else if (rowCur["PayNum"].ToString() != "0")
                {
                    List<string> procsOnPayment = rowCur["procsOnObj"].ToString().Split(',').ToList();
                    List<string> paymentsOnObj = rowCur["paymentsOnObj"].ToString().Split(',').ToList();
                    List<string> adjustsOnPayment = rowCur["adjustsOnObj"].ToString().Split(',').ToList();
                    for (int i = 0; i < table.Rows.Count; i++)
                    {//loop through all rows
                        if (table.Rows[i]["PayNum"].ToString() == rowCur["PayNum"].ToString())
                        {
                            gridAccount.SetSelected(i, true);//for other splits in family view
                            procsOnPayment.AddRange(table.Rows[i]["procsOnObj"].ToString().Split(','));
                            paymentsOnObj.AddRange(table.Rows[i]["paymentsOnObj"].ToString().Split(','));
                            adjustsOnPayment.AddRange(table.Rows[i]["adjustsOnObj"].ToString().Split(','));
                        }
                    }
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        if (procsOnPayment.Contains(table.Rows[i]["ProcNum"].ToString()))
                        {
                            gridAccount.SetSelected(i, true);
                        }
                        if (paymentsOnObj.Contains(table.Rows[i]["PayNum"].ToString()))
                        {
                            gridAccount.SetSelected(i, true);
                        }
                        if (adjustsOnPayment.Contains(table.Rows[i]["Adjnum"].ToString()))
                        {
                            gridAccount.SetSelected(i, true);
                        }
                    }
                }
                else if (gridAccount.SelectedIndices.Contains(e.Row) && rowCur["AdjNum"].ToString() != "0" && rowCur["procsOnObj"].ToString() != "0")
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        if (table.Rows[i]["ProcNum"].ToString() == rowCur["procsOnObj"].ToString())
                        {
                            gridAccount.SetSelected(i, true);
                            break;
                        }
                    }
                }
                else if (rowCur["ProcNumLab"].ToString() != "0" && rowCur["ProcNumLab"].ToString() != "")
                {//Canadian Lab procedure, select parents and other associated labs too.
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        if (table.Rows[i]["ProcNum"].ToString() == rowCur["ProcNumLab"].ToString())
                        {
                            gridAccount.SetSelected(i, true);
                            continue;
                        }
                        if (table.Rows[i]["ProcNumLab"].ToString() == rowCur["ProcNumLab"].ToString())
                        {
                            gridAccount.SetSelected(i, true);
                            continue;
                        }
                    }
                }
                else if (rowCur["ProcNum"].ToString() != "0")
                {//Not a Canadian lab and is a procedure.
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        if (table.Rows[i]["ProcNumLab"].ToString() == rowCur["ProcNum"].ToString())
                        {
                            gridAccount.SetSelected(i, true);
                            continue;
                        }
                    }
                }
            }
        }

        private void gridAccount_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e)
        {
            if (ViewingInRecall) return;
            Actscrollval = gridAccount.ScrollValue;
            DataTable table = DataSetMain.Tables["account"];
            if (table.Rows[e.Row]["ProcNum"].ToString() != "0")
            {
                Procedure proc = Procedures.GetOneProc(PIn.Long(table.Rows[e.Row]["ProcNum"].ToString()), true);
                Patient pat = FamCur.GetPatient(proc.PatNum);
                FormProcEdit FormPE = new FormProcEdit(proc, pat, FamCur);
                FormPE.ShowDialog();
            }
            else if (table.Rows[e.Row]["AdjNum"].ToString() != "0")
            {
                Adjustment adj = Adjustments.GetOne(PIn.Long(table.Rows[e.Row]["AdjNum"].ToString()));
                if (adj == null)
                {
                    MsgBox.Show(this, "The adjustment has been deleted.");//Don't return. Fall through to the refresh. 
                }
                else
                {
                    FormAdjust FormAdj = new FormAdjust(PatCur, adj);
                    FormAdj.ShowDialog();
                }
            }
            else if (table.Rows[e.Row]["PayNum"].ToString() != "0")
            {
                Payment PaymentCur = Payments.GetPayment(PIn.Long(table.Rows[e.Row]["PayNum"].ToString()));
                if (PaymentCur == null)
                {
                    MessageBox.Show(Lans.g(this, "No payment exists.  Please run database maintenance method") + " " + nameof(DatabaseMaintenances.PaySplitWithInvalidPayNum));
                    return;
                }
                /*
				if(PaymentCur.PayType==0){//provider income transfer
					FormProviderIncTrans FormPIT=new FormProviderIncTrans();
					FormPIT.PatNum=PatCur.PatNum;
					FormPIT.PaymentCur=PaymentCur;
					FormPIT.IsNew=false;
					FormPIT.ShowDialog();
				}
				else{*/
                FormPayment FormPayment2 = new FormPayment(PatCur, FamCur, PaymentCur, false);
                FormPayment2.IsNew = false;
                FormPayment2.ShowDialog();
                //}
            }
            else if (table.Rows[e.Row]["ClaimNum"].ToString() != "0")
            {//claims and claimpayments
                if (!Security.IsAuthorized(Permissions.ClaimView))
                {
                    return;
                }
                Claim claim = Claims.GetClaim(PIn.Long(table.Rows[e.Row]["ClaimNum"].ToString()));
                if (claim == null)
                {
                    MsgBox.Show(this, "The claim has been deleted.");
                }
                else
                {
                    Patient pat = FamCur.GetPatient(claim.PatNum);
                    FormClaimEdit FormClaimEdit2 = new FormClaimEdit(claim, pat, FamCur);
                    FormClaimEdit2.IsNew = false;
                    FormClaimEdit2.ShowDialog();
                }
            }
            else if (table.Rows[e.Row]["StatementNum"].ToString() != "0")
            {
                Statement stmt = Statements.GetStatement(PIn.Long(table.Rows[e.Row]["StatementNum"].ToString()));
                if (stmt == null)
                {
                    MsgBox.Show(this, "The statement has been deleted");//Don't return. Fall through to the refresh. 
                }
                else
                {
                    FormStatementOptions FormS = new FormStatementOptions();
                    FormS.StmtCur = stmt;
                    FormS.ShowDialog();
                }
            }
            else if (table.Rows[e.Row]["PayPlanNum"].ToString() != "0")
            {
                PayPlan payplan = PayPlans.GetOne(PIn.Long(table.Rows[e.Row]["PayPlanNum"].ToString()));
                if (payplan == null)
                {
                    MsgBox.Show(this, "This pay plan has been deleted by another user.");
                }
                else
                {
                    FormPayPlan2 = new FormPayPlan(payplan);
                    FormPayPlan2.ShowDialog();
                    if (FormPayPlan2.GotoPatNum != 0)
                    {
                        FormOpenDental.S_Contr_PatientSelected(Patients.GetPat(FormPayPlan2.GotoPatNum), false);
                        ModuleSelected(FormPayPlan2.GotoPatNum, false);
                        return;
                    }
                }
            }
            ModuleSelected(PatCur.PatNum, _isSelectingFamily);
        }

        private void gridPayPlan_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            DataRow selectedRow = ((DataRow)(gridPayPlan.Rows[e.Row].Tag));
            if (selectedRow["PayPlanNum"].ToString() != "0")
            {//Payment plan
                PayPlan payplan = PayPlans.GetOne(PIn.Long(selectedRow["PayPlanNum"].ToString()));
                if (payplan == null)
                {
                    MsgBox.Show(this, "This pay plan has been deleted by another user.");
                }
                else
                {
                    FormPayPlan2 = new FormPayPlan(payplan);
                    FormPayPlan2.ShowDialog();
                    if (FormPayPlan2.GotoPatNum != 0)
                    {
                        FormOpenDental.S_Contr_PatientSelected(Patients.GetPat(FormPayPlan2.GotoPatNum), false);
                        ModuleSelected(FormPayPlan2.GotoPatNum, false);
                        return;
                    }
                }
                ModuleSelected(PatCur.PatNum, _isSelectingFamily);
            }
            else
            {//Installment Plan
                FormInstallmentPlanEdit FormIPE = new FormInstallmentPlanEdit();
                FormIPE.InstallmentPlanCur = InstallmentPlans.GetOne(PIn.Long(selectedRow["InstallmentPlanNum"].ToString()));
                FormIPE.IsNew = false;
                FormIPE.ShowDialog();
                ModuleSelected(PatCur.PatNum);
            }
        }

        private void gridAcctPat_CellClick(object sender, ODGridClickEventArgs e)
        {
            if (ViewingInRecall)
            {
                return;
            }
            if (e.Row == gridAcctPat.Rows.Count - 1)
            {//last row
                FormOpenDental.S_Contr_PatientSelected(FamCur.ListPats[0], false);
                ModuleSelected(FamCur.ListPats[0].PatNum, true);
            }
            else
            {
                long patNum = (long)gridAcctPat.Rows[e.Row].Tag;
                Patient pat = FamCur.ListPats.First(x => x.PatNum == patNum);
                if (pat == null)
                {
                    return;
                }
                FormOpenDental.S_Contr_PatientSelected(pat, false);
                ModuleSelected(patNum);
            }
        }

        private delegate void ToolBarClick();

        private void ToolBarMain_ButtonClick(object sender, OpenDental.UI.ODToolBarButtonClickEventArgs e)
        {
            if (e.Button.Tag.GetType() == typeof(string))
            {
                //standard predefined button
                switch (e.Button.Tag.ToString())
                {
                    //case "Patient":
                    //	OnPat_Click();
                    //	break;
                    case "Payment":
                        bool isTsiPayment = (TsiTransLogs.IsTransworldEnabled(PatCur.ClinicNum)
                            && Patients.IsGuarCollections(PatCur.Guarantor)
                            && !MsgBox.Show(this, MsgBoxButtons.YesNo, "The guarantor of this family has been sent to TSI for a past due balance.  "
                                + "Is the payment you are applying directly from the debtor or guarantor?\r\n\r\n"
                                + "Yes - this payment is directly from the debtor/guarantor\r\n\r\n"
                                + "No - this payment is from TSI"));
                        InputBox inputBox = new InputBox(new List<InputBoxParam>() { new InputBoxParam(InputBoxType.ValidDouble,Lan.g(this,"Please enter an amount: ")),
                            FamCur.ListPats.Length>1 ? (new InputBoxParam(InputBoxType.CheckBox,"",Lan.g(this," - Prefer this patient"),new Size(120,20))) : null }
                            , new Func<string, bool>((text) =>
                            {
                                if (text == "")
                                {
                                    MsgBox.Show(this, "Please enter a value.");
                                    return false;//Should stop user from continuing to payment window.
                                }
                                return true;//Allow user to the payment window.
                            })
                        );
                        //Plugins.HookAddCode(this,"ContrAccount.ToolBarMain_ButtonClick_paymentInputBox",inputBox,PatCur);
                        if (inputBox.ShowDialog() != DialogResult.OK)
                        {
                            break;
                        }
                        toolBarButPay_Click(PIn.Double(inputBox.textResult.Text), preferCurrentPat: (inputBox.checkBoxResult?.Checked ?? false), isPayPressed: true, isTsiPayment: isTsiPayment);
                        break;
                    case "Adjustment":
                        toolBarButAdj_Click();
                        break;
                    case "Insurance":
                        CreateClaimDataWrapper createClaimDataWrapper = ClaimL.GetCreateClaimDataWrapper(PatCur, FamCur, GetCreateClaimItemsFromUI(), true);
                        if (createClaimDataWrapper.HasError)
                        {
                            break;
                        }
                        createClaimDataWrapper = ClaimL.CreateClaimFromWrapper(true, createClaimDataWrapper);
                        if (!createClaimDataWrapper.HasError && createClaimDataWrapper.DoRefresh)
                        {
                            ModuleSelected(PatCur.PatNum);
                        }
                        break;
                    case "PayPlan":
                        contextMenuPayPlan.Show(ToolBarMain, new Point(e.Button.Bounds.Location.X, e.Button.Bounds.Height));
                        break;
                    case "InstallPlan":
                        toolBarButInstallPlan_Click();
                        break;
                    case "RepeatCharge":
                        toolBarButRepeatCharge_Click();
                        break;
                    case "Statement":
                        //The reason we are using a delegate and BeginInvoke() is because of a Microsoft bug that causes the Print Dialog window to not be in focus			
                        //when it comes from a toolbar click.
                        //https://social.msdn.microsoft.com/Forums/windows/en-US/681a50b4-4ae3-407a-a747-87fb3eb427fd/first-mouse-click-after-showdialog-hits-the-parent-form?forum=winforms
                        ToolBarClick toolClick = toolBarButStatement_Click;
                        this.BeginInvoke(toolClick);
                        break;
                    case "Questionnaire":
                        toolBarButComm_Click();
                        break;
                    case "TrojanCollect":
                        toolBarButTrojan_Click();
                        break;
                    case "QuickProcs":
                        toolBarButQuickProcs_Click();
                        break;
                }
            }
            else if (e.Button.Tag.GetType() == typeof(Program))
            {
                ProgramL.Execute(((Program)e.Button.Tag).ProgramNum, PatCur);
            }
            //Plugins.HookAddCode(this,"ContrAccount.ToolBarMain_ButtonClick_end",PatCur,e);
        }

        private void toolBarButPay_Click(double payAmt, bool preferCurrentPat = false, bool isPrePay = false, bool isIncomeTransfer = false, bool isPayPressed = false, bool isTsiPayment = false)
        {
            Payment PaymentCur = new Payment();
            PaymentCur.PayDate = DateTimeOD.Today;
            PaymentCur.PatNum = PatCur.PatNum;
            //Explicitly set ClinicNum=0, since a pat's ClinicNum will remain set if the user enabled clinics, assigned patients to clinics, and then
            //disabled clinics because we use the ClinicNum to determine which PayConnect or XCharge/XWeb credentials to use for payments.
            PaymentCur.ClinicNum = 0;
            if (Preferences.HasClinicsEnabled)
            {//if clinics aren't enabled default to 0
                if ((PayClinicSetting)Preference.GetInt(PreferenceName.PaymentClinicSetting) == PayClinicSetting.PatientDefaultClinic)
                {
                    PaymentCur.ClinicNum = PatCur.ClinicNum;
                }
                else if ((PayClinicSetting)Preference.GetInt(PreferenceName.PaymentClinicSetting) == PayClinicSetting.SelectedExceptHQ)
                {
                    PaymentCur.ClinicNum = (Clinics.ClinicNum == 0) ? PatCur.ClinicNum : Clinics.ClinicNum;
                }
                else
                {
                    PaymentCur.ClinicNum = Clinics.ClinicNum;
                }
            }
            PaymentCur.DateEntry = DateTimeOD.Today;//So that it will show properly in the new window.
            List<Definition> listDefs = Definition.GetByCategory(DefinitionCategory.PaymentTypes);;
            if (listDefs.Count > 0)
            {
                PaymentCur.PayType = listDefs[0].Id;
            }
            PaymentCur.PaymentSource = CreditCardSource.None;
            PaymentCur.ProcessStatus = ProcessStat.OfficeProcessed;
            PaymentCur.PayAmt = payAmt;
            FormPayment FormP = new FormPayment(PatCur, FamCur, PaymentCur, preferCurrentPat);
            FormP.IsNew = true;
            FormP.IsIncomeTransfer = isIncomeTransfer;
            List<AccountEntry> listAcctEntries = new List<AccountEntry>();
            if (gridAccount.SelectedIndices.Length > 0)
            {
                DataTable table = DataSetMain.Tables["account"];
                for (int i = 0; i < gridAccount.SelectedIndices.Length; i++)
                {
                    if (table.Rows[gridAccount.SelectedIndices[i]]["ProcNum"].ToString() != "0")
                    {
                        //Add each selected proc to the list
                        listAcctEntries.Add(new AccountEntry(Procedures.GetOneProc(PIn.Long(table.Rows[gridAccount.SelectedIndices[i]]["ProcNum"].ToString()), false)));
                    }
                    if (PIn.Double(table.Rows[gridAccount.SelectedIndices[i]]["chargesDouble"].ToString()) > 0
                        && table.Rows[gridAccount.SelectedIndices[i]]["PayPlanChargeNum"].ToString() != "0")
                    {//PaymentPlanCharges
                     //Add selected positive pay plan debit to the list. Important to check for chargesDouble because there can be negative debits.
                        listAcctEntries.Add(new AccountEntry(PayPlanCharges.GetOne(PIn.Long(table.Rows[gridAccount.SelectedIndices[i]]["PayPlanChargeNum"].ToString()))));
                    }
                    if (table.Rows[gridAccount.SelectedIndices[i]]["AdjNum"].ToString() != "0")
                    {//Adjustments
                        Adjustment adjustment = Adjustments.GetOne(PIn.Long(table.Rows[gridAccount.SelectedIndices[i]]["AdjNum"].ToString()));
                        if (adjustment.AdjAmt > 0 && adjustment.ProcNum == 0)
                        {
                            listAcctEntries.Add(new AccountEntry(adjustment));//don't include negative adjustments, or adjs attached to procs, since then we pay off the proc
                        }
                    }
                }
            }
            if (isPrePay && PIn.Double(labelUnearnedAmt.Text) != 0)
            {
                if (listAcctEntries.Count < 1)
                {
                    FormProcSelect FormPS = new FormProcSelect(PatCur.PatNum, false, true);
                    if (FormPS.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    listAcctEntries = PaymentEdit.CreateAccountEntries(FormPS.ListSelectedProcs);
                }
                FormP.UnearnedAmt = PIn.Double(labelUnearnedAmt.Text);
            }
            FormP.ListEntriesPayFirst = listAcctEntries;
            if (PaymentCur.PayDate.Date > DateTime.Today.Date && !Preference.GetBool(PreferenceName.FutureTransDatesAllowed) && !Preference.GetBool(PreferenceName.AccountAllowFutureDebits))
            {
                MsgBox.Show(this, "Payments cannot be in the future.");
                return;
            }
            PaymentCur.PayAmt = payAmt;
            Payments.Insert(PaymentCur);
            FormP.ShowDialog();
            //If this is a payment received from Transworld, we don't want to send any new update messages to Transworld for any splits on this payment.
            //To prevent new msgs from being sent, we will insert TsiTransLogs linked to all splits with TsiTransType.None.  The ODService will update the
            //log TransAmt for any edits to this paysplit instead of sending a new msg to Transworld.
            if (isTsiPayment)
            {
                Payment payCur = Payments.GetPayment(PaymentCur.PayNum);
                if (payCur != null)
                {
                    List<PaySplit> listSplits = PaySplits.GetForPayment(payCur.PayNum);
                    if (listSplits.Count > 0)
                    {
                        PatAging pAging = Patients.GetAgingListFromGuarNums(new List<long>() { PatCur.Guarantor }).FirstOrDefault();
                        List<TsiTransLog> listLogsForInsert = new List<TsiTransLog>();
                        foreach (PaySplit splitCur in listSplits)
                        {
                            double logAmt = pAging.ListTsiLogs.FindAll(x => x.FKeyType == TsiFKeyType.PaySplit && x.FKey == splitCur.SplitNum).Sum(x => x.TransAmt);
                            if (splitCur.SplitAmt.IsEqual(logAmt))
                            {
                                continue;//split already linked to logs that sum to the split amount, nothing to do with this one
                            }
                            listLogsForInsert.Add(new TsiTransLog()
                            {
                                PatNum = pAging.PatNum,//this is the account guarantor, since these are reconciled by guars
                                UserNum = Security.CurUser.UserNum,
                                TransType = TsiTransType.None,
                                //TransDateTime=DateTime.Now,//set on insert, not editable by user
                                //DemandType=TsiDemandType.Accelerator,//only valid for placement msgs
                                //ServiceCode=TsiServiceCode.Diplomatic,//only valid for placement msgs
                                ClientId = pAging.ListTsiLogs.FirstOrDefault()?.ClientId ?? "",//can be blank, not used since this isn't really sent to Transworld
                                TransAmt = -splitCur.SplitAmt - logAmt,//Ex. already logged -10; split changed to -20; -20-(-10)=-10; -10 this split + -10 already logged = -20 split amt
                                AccountBalance = pAging.AmountDue - splitCur.SplitAmt - logAmt,
                                FKeyType = TsiFKeyType.PaySplit,
                                FKey = splitCur.SplitNum,
                                RawMsgText = "This was not a message sent to Transworld.  This paysplit was entered due to a payment received from Transworld.",
                                ClinicNum = (Preferences.HasClinicsEnabled ? pAging.ClinicNum : 0)
                                //,TransJson=""//only valid for placement msgs
                            });
                        }
                        if (listLogsForInsert.Count > 0)
                        {
                            TsiTransLogs.InsertMany(listLogsForInsert);
                        }
                    }
                }
            }
            ModuleSelected(PatCur.PatNum);
        }

        private void menuItemIncomeTransfer_Click(object sender, EventArgs e)
        {
            Payment PaymentCur = new Payment();
            PaymentCur.PayDate = DateTimeOD.Today;
            PaymentCur.PatNum = PatCur.PatNum;
            //Explicitly set ClinicNum=0, since a pat's ClinicNum will remain set if the user enabled clinics, assigned patients to clinics, and then
            //disabled clinics because we use the ClinicNum to determine which PayConnect or XCharge/XWeb credentials to use for payments.
            PaymentCur.ClinicNum = 0;
            if (Preferences.HasClinicsEnabled)
            {//if clinics aren't enabled default to 0
                PaymentCur.ClinicNum = Clinics.ClinicNum;
                if ((PayClinicSetting)Preference.GetInt(PreferenceName.PaymentClinicSetting) == PayClinicSetting.PatientDefaultClinic)
                {
                    PaymentCur.ClinicNum = PatCur.ClinicNum;
                }
                else if ((PayClinicSetting)Preference.GetInt(PreferenceName.PaymentClinicSetting) == PayClinicSetting.SelectedExceptHQ)
                {
                    PaymentCur.ClinicNum = (Clinics.ClinicNum == 0 ? PatCur.ClinicNum : Clinics.ClinicNum);
                }
            }
            PaymentCur.DateEntry = DateTimeOD.Today;//So that it will show properly in the new window.
            PaymentCur.PaymentSource = CreditCardSource.None;
            PaymentCur.ProcessStatus = ProcessStat.OfficeProcessed;
            PaymentCur.PayAmt = 0;
            PaymentCur.PayType = 0;
            Payments.Insert(PaymentCur);
            FormIncomeTransferManage FormITM = new FormIncomeTransferManage(FamCur, PatCur, PaymentCur);
            if (FormITM.ShowDialog() != DialogResult.OK)
            {
                Payments.Delete(PaymentCur);
            }
            ModuleSelected(PatCur.PatNum);
        }

        private void toolBarButAdj_Click()
        {
            AddAdjustmentToSelectedProcsHelper();
        }

        private void menuItemAddAdj_Click(object sender, EventArgs e)
        {
            AddAdjustmentToSelectedProcsHelper();
        }

        ///<summary>If the user selects multiple procedures (validated) then we pass the selected procedures to FormMultiAdj. Otherwise if the user
        ///selects one procedure (not validated) we maintain the previous functionality of opening FormAdjust.</summary>
        private void AddAdjustmentToSelectedProcsHelper(bool openMultiAdj = false)
        {
            bool isTsiAdj = (TsiTransLogs.IsTransworldEnabled(PatCur.ClinicNum)
                && Patients.IsGuarCollections(PatCur.Guarantor)
                && !MsgBox.Show(this, MsgBoxButtons.YesNo, "The guarantor of this family has been sent to TSI for a past due balance.  "
                    + "Is this an adjustment applied by the office?\r\n\r\n"
                    + "Yes - this is an adjustment applied by the office\r\n\r\n"
                    + "No - this adjustment is the result of a payment received from TSI"));
            DataTable tableAcct = DataSetMain.Tables["account"];
            List<Procedure> listSelectedProcs = new List<Procedure>();
            for (int i = 0; i < gridAccount.SelectedIndices.Length; i++)
            {
                long procNumCur = PIn.Long(tableAcct.Rows[gridAccount.SelectedIndices[i]]["ProcNum"].ToString());
                if (procNumCur == 0)
                {
                    MsgBox.Show(this, "You can only select procedures.");
                    return;
                }
                listSelectedProcs.Add(Procedures.GetOneProc(procNumCur, false));
            }
            //If the user selects multiple adjustments, open FormMultiAdj with the selected procedures
            if (listSelectedProcs.Count > 1 || openMultiAdj)
            {
                //Open the form with only the selected procedures
                FormAdjMulti form = new FormAdjMulti(PatCur, listSelectedProcs);
                form.ShowDialog();
            }
            else
            {
                Adjustment adjustmentCur = new Adjustment();
                adjustmentCur.DateEntry = DateTime.Today;//cannot be changed. Handled automatically
                adjustmentCur.AdjDate = DateTime.Today;
                adjustmentCur.ProcDate = DateTime.Today;
                adjustmentCur.ProvNum = PatCur.PriProv;
                adjustmentCur.PatNum = PatCur.PatNum;
                adjustmentCur.ClinicNum = PatCur.ClinicNum;
                if (gridAccount.SelectedGridRows.Count == 1)
                {
                    adjustmentCur.ProcNum = PIn.Long(tableAcct.Rows[gridAccount.SelectedIndices[0]]["ProcNum"].ToString());
                    Procedure proc = Procedures.GetOneProc(adjustmentCur.ProcNum, false);
                    if (proc != null)
                    {
                        adjustmentCur.ProvNum = proc.ProvNum;
                        adjustmentCur.ClinicNum = proc.ClinicNum;
                    }
                }
                FormAdjust FormAdjust2 = new FormAdjust(PatCur, adjustmentCur, isTsiAdj);
                FormAdjust2.IsNew = true;
                FormAdjust2.ShowDialog();
                //Shared.ComputeBalances();
            }
            ModuleSelected(PatCur.PatNum);
        }

        ///<summary>Returns a list of CreateClaimItems comprised from the selected items within gridAccount.
        ///If no rows are currently selected then the list returned will be comprised of all items within the "account" table in the DataSet.</summary>
        private List<CreateClaimItem> GetCreateClaimItemsFromUI()
        {
            //There have been reports of concurrency issues so make a deep copy of the selected indices and the table first to help alleviate the problem.
            //See task #830623 and task #1266253 for more details.
            int[] arraySelectedIndices = (int[])gridAccount.SelectedIndices.Clone();
            DataTable table = GetTableFromDataSet("account");
            List<CreateClaimItem> listCreateClaimItems = ClaimL.GetCreateClaimItems(table, arraySelectedIndices);
            if (CultureInfo.CurrentCulture.Name.EndsWith("CA"))
            {
                //We do not want to consider Canadian lab procs to be selected.  If we do, these lab procs will later cause the corresponding lab ClaimProcs to 
                //be included in the Claim's list of ClaimProcs, which will then cause the ClaimProcs for the labs to get a LineNumber, which will in turn cause
                //the EOB Importer to fail because the LineNumbers in the database's list of ClaimProcs no longer match the EOB LineNumbers.
                listCreateClaimItems.RemoveAll(x => x.ProcNumLab != 0);
            }
            return listCreateClaimItems;
        }

        private void menuItemSalesTax_Click(object sender, EventArgs e)
        {
            if (gridAccount.SelectedIndices.Length == 0)
            {
                MsgBox.Show(this, "Please select at least one procedure.");
                return;
            }
            DataTable table = DataSetMain.Tables["account"];
            double taxPercent = Preference.GetDouble(PreferenceName.SalesTaxPercentage);
            long adjType = Preference.GetLong(PreferenceName.SalesTaxAdjustmentType);
            foreach (int idx in gridAccount.SelectedIndices)
            {
                if (table.Rows[idx]["ProcNum"].ToString() == "0")
                {
                    continue;//They selected a whole bunch, if it's not a proc don't make a sales tax adjustment
                }
                Procedure proc = Procedures.GetOneProc(PIn.Long(table.Rows[idx]["ProcNum"].ToString()), false);
                List<ClaimProc> listClaimProcs = ClaimProcs.GetForProcs(new List<long>() { proc.ProcNum });
                double writeOff = 0;
                foreach (ClaimProc claimProc in listClaimProcs)
                {
                    if (claimProc.Status == ClaimProcStatus.Estimate)
                    {
                        if (claimProc.WriteOffEstOverride != -1)
                        {
                            writeOff += claimProc.WriteOffEstOverride;
                        }
                        else if (claimProc.WriteOffEst != -1)
                        {
                            writeOff += claimProc.WriteOffEst;
                        }
                    }
                    else if ((claimProc.Status == ClaimProcStatus.Received || claimProc.Status == ClaimProcStatus.NotReceived) && claimProc.WriteOff != -1)
                    {
                        writeOff += claimProc.WriteOff;
                    }
                }
                Adjustment adjustment = new Adjustment();
                adjustment.AdjDate = DateTime.Today;
                adjustment.ProcDate = proc.ProcDate;
                adjustment.ProvNum = Preference.GetLong(PreferenceName.PracticeDefaultProv);
                Clinic procClinic = Clinics.GetClinic(proc.ClinicNum);
                if (proc.ClinicNum != 0 && procClinic.DefaultProv != 0)
                {
                    adjustment.ProvNum = procClinic.DefaultProv;
                }
                adjustment.PatNum = PatCur.PatNum;
                adjustment.ClinicNum = proc.ClinicNum;
                adjustment.AdjAmt = Math.Round((proc.ProcFee - writeOff) * (taxPercent / 100), 2);//Round to two places
                adjustment.AdjType = adjType;
                adjustment.ProcNum = proc.ProcNum;
                //adjustment.AdjNote=Lan.g(this,"Sales Tax");
                Adjustments.Insert(adjustment);
                TsiTransLogs.CheckAndInsertLogsIfAdjTypeExcluded(adjustment, PatCur.Guarantor, PatCur.ClinicNum);
            }
            ModuleSelected(PatCur.PatNum);
        }

        private void menuItemAddMultAdj_Click(object sender, EventArgs e)
        {
            AddAdjustmentToSelectedProcsHelper(true);
        }

        private void menuInsPri_Click(object sender, System.EventArgs e)
        {
            CreateClaimDataWrapper createClaimDataWrapper = ClaimL.GetCreateClaimDataWrapper(PatCur, FamCur, GetCreateClaimItemsFromUI(), true, true);
            if (createClaimDataWrapper.HasError)
            {
                return;
            }
            if (PatPlans.GetOrdinal(PriSecMed.Primary, createClaimDataWrapper.ClaimData.ListPatPlans, createClaimDataWrapper.ClaimData.ListInsPlans
                , createClaimDataWrapper.ClaimData.ListInsSubs) == 0)
            {
                MsgBox.Show(this, "The patient does not have any dental insurance plans.");
                return;
            }
            Claim claimCur = new Claim();
            claimCur.ClaimStatus = "W";
            claimCur.DateSent = DateTime.Today;
            claimCur.DateSentOrig = DateTime.MinValue;
            //Set ClaimCur to CreateClaim because the reference to ClaimCur gets broken when inserting.
            claimCur = ClaimL.CreateClaim(claimCur, "P", true, createClaimDataWrapper);
            if (claimCur.ClaimNum == 0)
            {
                ModuleSelected(PatCur.PatNum);
                return;
            }
            //still have not saved some changes to the claim at this point
            FormClaimEdit FormCE = new FormClaimEdit(claimCur, PatCur, FamCur);
            FormCE.IsNew = true;//this causes it to delete the claim if cancelling.
                                //If there's unallocated amounts, we want to redistribute the money to other procedures.
            if (FormCE.ShowDialog() == DialogResult.OK && PIn.Double(labelUnearnedAmt.Text) > 0)
            {
                ClaimL.AllocateUnearnedPayment(PatCur, FamCur, PIn.Double(labelUnearnedAmt.Text), claimCur);
            }
            ModuleSelected(PatCur.PatNum);
        }

        private void menuInsSec_Click(object sender, System.EventArgs e)
        {
            CreateClaimDataWrapper createClaimDataWrapper = ClaimL.GetCreateClaimDataWrapper(PatCur, FamCur, GetCreateClaimItemsFromUI(), true, true);
            if (createClaimDataWrapper.HasError)
            {
                return;
            }
            if (createClaimDataWrapper.ClaimData.ListPatPlans.Count < 2)
            {
                MessageBox.Show(Lan.g(this, "Patient does not have secondary insurance."));
                return;
            }
            if (PatPlans.GetOrdinal(PriSecMed.Secondary, createClaimDataWrapper.ClaimData.ListPatPlans, createClaimDataWrapper.ClaimData.ListInsPlans
                , createClaimDataWrapper.ClaimData.ListInsSubs) == 0)
            {
                MsgBox.Show(this, "Patient does not have secondary insurance.");
                return;
            }
            Claim claimCur = new Claim();
            claimCur.ClaimStatus = "W";
            claimCur.DateSent = DateTimeOD.Today;
            claimCur.DateSentOrig = DateTime.MinValue;
            //Set ClaimCur to CreateClaim because the reference to ClaimCur gets broken when inserting.
            claimCur = ClaimL.CreateClaim(claimCur, "S", true, createClaimDataWrapper);
            if (claimCur.ClaimNum == 0)
            {
                ModuleSelected(PatCur.PatNum);
                return;
            }
            FormClaimEdit FormCE = new FormClaimEdit(claimCur, PatCur, FamCur);
            FormCE.IsNew = true;//this causes it to delete the claim if cancelling.
                                //If there's unallocated amounts, we want to redistribute the money to other procedures.
            if (FormCE.ShowDialog() == DialogResult.OK && PIn.Double(labelUnearnedAmt.Text) > 0)
            {
                ClaimL.AllocateUnearnedPayment(PatCur, FamCur, PIn.Double(labelUnearnedAmt.Text), claimCur);
            }
            ModuleSelected(PatCur.PatNum);
        }

        private void menuInsMedical_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.ClaimView))
            {
                return;
            }
            if (!ClaimL.CheckClearinghouseDefaults())
            {
                return;
            }
            AccountModules.CreateClaimData data = AccountModules.GetCreateClaimData(PatCur, FamCur);
            long medSubNum = 0;
            for (int i = 0; i < data.ListPatPlans.Count; i++)
            {
                InsSub sub = InsSubs.GetSub(data.ListPatPlans[i].InsSubNum, data.ListInsSubs);
                if (InsPlans.GetPlan(sub.PlanNum, data.ListInsPlans).IsMedical)
                {
                    medSubNum = sub.InsSubNum;
                    break;
                }
            }
            if (medSubNum == 0)
            {
                MsgBox.Show(this, "Patient does not have medical insurance.");
                return;
            }
            DataTable table = DataSetMain.Tables["account"];
            Procedure proc;
            if (gridAccount.SelectedIndices.Length == 0)
            {
                //autoselect procedures
                for (int i = 0; i < table.Rows.Count; i++)
                {//loop through every line showing on screen
                    if (table.Rows[i]["ProcNum"].ToString() == "0")
                    {
                        continue;//ignore non-procedures
                    }
                    proc = Procedures.GetProcFromList(data.ListProcs, PIn.Long(table.Rows[i]["ProcNum"].ToString()));
                    if (proc.ProcFee == 0)
                    {
                        continue;//ignore zero fee procedures, but user can explicitly select them
                    }
                    if (proc.MedicalCode == "")
                    {
                        continue;//ignore non-medical procedures
                    }
                    if (Procedures.NeedsSent(proc.ProcNum, medSubNum, data.ListClaimProcs))
                    {
                        gridAccount.SetSelected(i, true);
                    }
                }
                if (gridAccount.SelectedIndices.Length == 0)
                {//if still none selected
                    MsgBox.Show(this, "Please select procedures first.");
                    return;
                }
            }
            bool allAreProcedures = true;
            for (int i = 0; i < gridAccount.SelectedIndices.Length; i++)
            {
                if (table.Rows[gridAccount.SelectedIndices[i]]["ProcNum"].ToString() == "0")
                {
                    allAreProcedures = false;
                }
            }
            if (!allAreProcedures)
            {
                MsgBox.Show(this, "You can only select procedures.");
                return;
            }
            //Medical claims are slightly different so we'll just manually create the CreateClaimDataWrapper needed for creating the claim.
            CreateClaimDataWrapper createClaimDataWrapper = new CreateClaimDataWrapper()
            {
                Pat = PatCur,
                Fam = FamCur,
                ListCreateClaimItems = GetCreateClaimItemsFromUI(),
                ClaimData = data,
            };
            Claim claimCur = new Claim();
            claimCur.ClaimStatus = "W";
            claimCur.DateSent = DateTimeOD.Today;
            claimCur.DateSentOrig = DateTime.MinValue;
            //Set ClaimCur to CreateClaim because the reference to ClaimCur gets broken when inserting.
            claimCur = ClaimL.CreateClaim(claimCur, "Med", true, createClaimDataWrapper);
            if (claimCur.ClaimNum == 0)
            {
                ModuleSelected(PatCur.PatNum);
                return;
            }
            //still have not saved some changes to the claim at this point
            FormClaimEdit FormCE = new FormClaimEdit(claimCur, PatCur, FamCur);
            FormCE.IsNew = true;//this causes it to delete the claim if cancelling.
                                //If there's unallocated amounts, we want to redistribute the money to other procedures.
            if (FormCE.ShowDialog() == DialogResult.OK && PIn.Double(labelUnearnedAmt.Text) > 0)
            {
                ClaimL.AllocateUnearnedPayment(PatCur, FamCur, PIn.Double(labelUnearnedAmt.Text), claimCur);
            }
            ModuleSelected(PatCur.PatNum);
        }

        private void menuInsOther_Click(object sender, System.EventArgs e)
        {
            CreateClaimDataWrapper createClaimDataWrapper = ClaimL.GetCreateClaimDataWrapper(PatCur, FamCur, GetCreateClaimItemsFromUI(), true, true);
            if (createClaimDataWrapper.HasError)
            {
                return;
            }
            Claim claimCur = new Claim();
            claimCur.ClaimStatus = "U";
            //Set ClaimCur to CreateClaim because the reference to ClaimCur gets broken when inserting.
            claimCur = ClaimL.CreateClaim(claimCur, "Other", true, createClaimDataWrapper);
            if (claimCur.ClaimNum == 0)
            {
                ModuleSelected(PatCur.PatNum);
                return;
            }
            //still have not saved some changes to the claim at this point
            FormClaimEdit FormCE = new FormClaimEdit(claimCur, PatCur, FamCur);
            FormCE.IsNew = true;//this causes it to delete the claim if cancelling.
            if (FormCE.ShowDialog() == DialogResult.OK && PIn.Double(labelUnearnedAmt.Text) > 0)
            {
                ClaimL.AllocateUnearnedPayment(PatCur, FamCur, PIn.Double(labelUnearnedAmt.Text), claimCur);
            }
            ModuleSelected(PatCur.PatNum);
        }

        private void menuItemInsPayPlan_Click(object sender, EventArgs e)
        {
            menuItemPayPlan_Click(true);
        }

        private void menuItemPatPayPlan_Click(object sender, EventArgs e)
        {
            menuItemPayPlan_Click(false);
        }

        private void menuItemPayPlan_Click(bool isInsPayPlan)
        {
            if (!Security.IsAuthorized(Permissions.PayPlanEdit))
            {
                return;
            }
            bool isTsiPayplan = TsiTransLogs.IsTransworldEnabled(FamCur.Guarantor.ClinicNum) && Patients.IsGuarCollections(PatCur.Guarantor, false);
            string msg = "";
            if (isTsiPayplan)
            {
                if (!Security.IsAuthorized(Permissions.Billing, true))
                {
                    msg = Lan.g(this, "The guarantor of this family has been sent to TSI for a past due balance.") + "\r\n"
                        + Lan.g(this, "Creating a payment plan for this guarantor would cause the account to be suspended in the TSI system but you are not "
                            + "authorized for") + "\r\n"
                        + GroupPermissions.GetDesc(Permissions.Billing);
                    MessageBox.Show(this, msg);
                    return;
                }
                string billingType = Defs.GetName(DefinitionCategory.BillingTypes, Preference.GetLong(PreferenceName.TransworldPaidInFullBillingType));
                msg = Lan.g(this, "The guarantor of this family has been sent to TSI for a past due balance.") + "\r\n"
                    + Lan.g(this, "Creating this payment plan will suspend the TSI account for a maximum of 50 days if the account is in the Accelerator or "
                        + "Profit Recovery stage.") + "\r\n"
                    + Lan.g(this, "Continue creating the payment plan?") + "\r\n\r\n"
                    + Lan.g(this, "Yes - Create the payment plan, send a suspend message to TSI, and change the guarantor's billing type to") + " "
                        + billingType + ".\r\n\r\n"
                    + Lan.g(this, "No - Do not create the payment plan and allow TSI to continue managing the account.");
                if (!MsgBox.Show(this, MsgBoxButtons.YesNo, msg))
                {
                    return;
                }
            }
            PayPlan payPlan = new PayPlan();
            payPlan.PatNum = PatCur.PatNum;
            payPlan.Guarantor = PatCur.Guarantor;
            payPlan.PayPlanDate = DateTimeOD.Today;
            payPlan.CompletedAmt = 0;
            payPlan.PayPlanNum = PayPlans.Insert(payPlan);
            FormPayPlan FormPP = new FormPayPlan(payPlan);
            FormPP.TotalAmt = PatCur.EstBalance;
            FormPP.IsNew = true;
            FormPP.IsInsPayPlan = isInsPayPlan;
            FormPP.ShowDialog();
            if (FormPP.GotoPatNum != 0)
            {
                FormOpenDental.S_Contr_PatientSelected(Patients.GetPat(FormPP.GotoPatNum), false);
                ModuleSelected(FormPP.GotoPatNum);//switches to other patient.
            }
            else
            {
                ModuleSelected(PatCur.PatNum);
            }
            if (isTsiPayplan && PayPlans.GetOne(payPlan.PayPlanNum) != null)
            {
                msg = TsiTransLogs.SuspendGuar(FamCur.Guarantor);
                if (!string.IsNullOrEmpty(msg))
                {
                    MessageBox.Show(this, msg + "\r\n" + Lan.g(this, "The account will have to be suspended manually using the A/R Manager or the TSI web portal."));
                }
            }
        }

        private void toolBarButInstallPlan_Click()
        {
            if (InstallmentPlans.GetOneForFam(PatCur.Guarantor) != null)
            {
                MsgBox.Show(this, "Family already has an installment plan.");
                return;
            }
            InstallmentPlan installPlan = new InstallmentPlan();
            installPlan.PatNum = PatCur.Guarantor;
            installPlan.DateAgreement = DateTime.Today;
            installPlan.DateFirstPayment = DateTime.Today;
            //InstallmentPlans.Insert(installPlan);
            FormInstallmentPlanEdit FormIPE = new FormInstallmentPlanEdit();
            FormIPE.InstallmentPlanCur = installPlan;
            FormIPE.IsNew = true;
            FormIPE.ShowDialog();
            ModuleSelected(PatCur.PatNum);
        }

        private void toolBarButRepeatCharge_Click()
        {
            RepeatCharge repeat = new RepeatCharge();
            repeat.PatNum = PatCur.PatNum;
            repeat.DateStart = DateTime.Today;
            FormRepeatChargeEdit FormR = new FormRepeatChargeEdit(repeat);
            FormR.IsNew = true;
            FormR.ShowDialog();
            ModuleSelected(PatCur.PatNum);
        }

        private void MenuItemRepeatStand_Click(object sender, System.EventArgs e)
        {
            if (!ProcedureCodes.GetContainsKey("001"))
            {
                return;
            }
            UpdatePatientBillingDay(PatCur.PatNum);
            RepeatCharge repeat = new RepeatCharge();
            repeat.PatNum = PatCur.PatNum;
            repeat.ProcCode = "001";
            repeat.ChargeAmt = 169;
            repeat.DateStart = DateTimeOD.Today;
            repeat.DateStop = DateTimeOD.Today.AddMonths(11);
            repeat.IsEnabled = true;
            RepeatCharges.Insert(repeat);
            repeat = new RepeatCharge();
            repeat.PatNum = PatCur.PatNum;
            repeat.ProcCode = "001";
            repeat.ChargeAmt = 119;
            repeat.DateStart = DateTimeOD.Today.AddYears(1);
            repeat.IsEnabled = true;
            RepeatCharges.Insert(repeat);
            ModuleSelected(PatCur.PatNum);
        }

        private void toolBarButStatement_Click()
        {
            Statement stmt = new Statement();
            stmt.PatNum = PatCur.Guarantor;
            stmt.DateSent = DateTimeOD.Today;
            stmt.IsSent = true;
            stmt.Mode_ = StatementMode.InPerson;
            stmt.HidePayment = false;
            stmt.SinglePatient = false;
            stmt.Intermingled = Preference.GetBool(PreferenceName.IntermingleFamilyDefault);
            stmt.StatementType = StmtType.NotSet;
            stmt.DateRangeFrom = DateTime.MinValue;
            if (Preference.GetBool(PreferenceName.FuchsOptionsOn))
            {
                stmt.DateRangeFrom = PIn.Date(DateTime.Today.AddDays(-45).ToShortDateString());
                stmt.DateRangeTo = PIn.Date(DateTime.Today.ToShortDateString());
            }
            else
            {
                if (textDateStart.errorProvider1.GetError(textDateStart) == "")
                {
                    if (textDateStart.Text != "")
                    {
                        stmt.DateRangeFrom = PIn.Date(textDateStart.Text);
                    }
                }
            }
            stmt.DateRangeTo = DateTimeOD.Today;//This is needed for payment plan accuracy.//new DateTime(2200,1,1);
            if (textDateEnd.errorProvider1.GetError(textDateEnd) == "")
            {
                if (textDateEnd.Text != "")
                {
                    stmt.DateRangeTo = PIn.Date(textDateEnd.Text);
                }
            }
            stmt.Note = "";
            stmt.NoteBold = "";
            Patient guarantor = null;
            if (PatCur != null)
            {
                guarantor = Patients.GetPat(PatCur.Guarantor);
            }
            if (guarantor != null)
            {
                stmt.IsBalValid = true;
                stmt.BalTotal = guarantor.BalTotal;
                stmt.InsEst = guarantor.InsEst;
            }
            PrintStatement(stmt);
            ModuleSelected(PatCur.PatNum);
        }

        private void menuItemStatementWalkout_Click(object sender, System.EventArgs e)
        {
            Statement stmt = new Statement();
            stmt.PatNum = PatCur.PatNum;
            stmt.DateSent = DateTimeOD.Today;
            stmt.IsSent = true;
            stmt.Mode_ = StatementMode.InPerson;
            stmt.HidePayment = true;
            stmt.Intermingled = Preference.GetBool(PreferenceName.IntermingleFamilyDefault);
            stmt.SinglePatient = !stmt.Intermingled;
            stmt.IsReceipt = false;
            stmt.StatementType = StmtType.NotSet;
            stmt.DateRangeFrom = DateTimeOD.Today;
            stmt.DateRangeTo = DateTimeOD.Today;
            stmt.Note = "";
            stmt.NoteBold = "";
            Patient guarantor = null;
            if (PatCur != null)
            {
                guarantor = Patients.GetPat(PatCur.Guarantor);
            }
            if (guarantor != null)
            {
                stmt.IsBalValid = true;
                stmt.BalTotal = guarantor.BalTotal;
                stmt.InsEst = guarantor.InsEst;
            }
            PrintStatement(stmt);
            ModuleSelected(PatCur.PatNum);
        }

        private void menuItemStatementEmail_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.EmailSend))
            {
                Cursor = Cursors.Default;
                return;
            }
            Statement stmt = new Statement();
            stmt.PatNum = PatCur.Guarantor;
            stmt.DateSent = DateTimeOD.Today;
            stmt.IsSent = true;
            stmt.Mode_ = StatementMode.Email;
            stmt.HidePayment = false;
            stmt.SinglePatient = false;
            stmt.Intermingled = Preference.GetBool(PreferenceName.IntermingleFamilyDefault);
            stmt.IsReceipt = false;
            stmt.StatementType = StmtType.NotSet;
            stmt.DateRangeFrom = DateTime.MinValue;
            if (textDateStart.errorProvider1.GetError(textDateStart) == "")
            {
                if (textDateStart.Text != "")
                {
                    stmt.DateRangeFrom = PIn.Date(textDateStart.Text);
                }
            }
            stmt.DateRangeTo = DateTimeOD.Today;//Needed for payplan accuracy.  Used to be setting to new DateTime(2200,1,1);
            if (textDateEnd.errorProvider1.GetError(textDateEnd) == "")
            {
                if (textDateEnd.Text != "")
                {
                    stmt.DateRangeTo = PIn.Date(textDateEnd.Text);
                }
            }
            stmt.Note = "";
            stmt.NoteBold = "";
            Patient guarantor = null;
            if (PatCur != null)
            {
                guarantor = Patients.GetPat(PatCur.Guarantor);
            }
            if (guarantor != null)
            {
                stmt.IsBalValid = true;
                stmt.BalTotal = guarantor.BalTotal;
                stmt.InsEst = guarantor.InsEst;
            }
            //It's pointless to give the user the window to select statement options, because they could just as easily have hit the More Options dropdown, then Email from there.
            PrintStatement(stmt);
            ModuleSelected(PatCur.PatNum);
        }

        private void menuItemReceipt_Click(object sender, EventArgs e)
        {
            Statement stmt = new Statement();
            stmt.PatNum = PatCur.PatNum;
            stmt.DateSent = DateTimeOD.Today;
            stmt.IsSent = true;
            stmt.Mode_ = StatementMode.InPerson;
            stmt.HidePayment = true;
            stmt.Intermingled = Preference.GetBool(PreferenceName.IntermingleFamilyDefault);
            stmt.SinglePatient = !stmt.Intermingled;
            stmt.IsReceipt = true;
            stmt.StatementType = StmtType.NotSet;
            stmt.DateRangeFrom = DateTimeOD.Today;
            stmt.DateRangeTo = DateTimeOD.Today;
            stmt.Note = "";
            stmt.NoteBold = "";
            Patient guarantor = null;
            if (PatCur != null)
            {
                guarantor = Patients.GetPat(PatCur.Guarantor);
            }
            if (guarantor != null)
            {
                stmt.IsBalValid = true;
                stmt.BalTotal = guarantor.BalTotal;
                stmt.InsEst = guarantor.InsEst;
            }
            PrintStatement(stmt);
            ModuleSelected(PatCur.PatNum);
        }

        private void menuItemInvoice_Click(object sender, EventArgs e)
        {
            DataTable table = DataSetMain.Tables["account"];
            Dictionary<string, List<long>> dictSuperFamItems = new Dictionary<string, List<long>>();
            Patient guarantor = Patients.GetPat(PatCur.Guarantor);
            Patient superHead = Patients.GetPat(PatCur.SuperFamily);
            if (gridAccount.SelectedIndices.Length == 0
                && (!Preference.GetBool(PreferenceName.ShowFeatureSuperfamilies) || !guarantor.HasSuperBilling || !superHead.HasSuperBilling))
            {
                //autoselect procedures, adjustments, and some pay plan charges
                for (int i = 0; i < table.Rows.Count; i++)
                {//loop through every line showing on screen
                    if (table.Rows[i]["ProcNum"].ToString() == "0"
                        && table.Rows[i]["AdjNum"].ToString() == "0"
                        && table.Rows[i]["PayPlanChargeNum"].ToString() == "0")
                    {
                        continue;//ignore items that aren't procs, adjustments, or pay plan charges
                    }
                    if (PIn.Date(table.Rows[i]["date"].ToString()) != DateTime.Today)
                    {
                        continue;
                    }
                    if (table.Rows[i]["ProcNum"].ToString() != "0")
                    {//if selected item is a procedure
                        Procedure proc = Procedures.GetOneProc(PIn.Long(table.Rows[i]["ProcNum"].ToString()), false);
                        if (proc.StatementNum != 0)
                        {//already attached so don't autoselect
                            continue;
                        }
                        if (proc.PatNum != PatCur.PatNum)
                        {
                            continue;
                        }
                    }
                    else if (table.Rows[i]["PayPlanChargeNum"].ToString() != "0")
                    {//selected item is pay plan charge
                        PayPlanCharge payPlanCharges = PayPlanCharges.GetOne(PIn.Long(table.Rows[i]["PayPlanChargeNum"].ToString()));
                        if (payPlanCharges.PatNum != PatCur.PatNum)
                        {
                            continue;
                        }
                        if (payPlanCharges.ChargeType != PayPlanChargeType.Debit)
                        {
                            continue;
                        }
                        if (payPlanCharges.StatementNum != 0)
                        {
                            continue;
                        }
                    }
                    else
                    {//item must be adjustment
                        Adjustment adj = Adjustments.GetOne(PIn.Long(table.Rows[i]["AdjNum"].ToString()));
                        if (adj.StatementNum != 0)
                        {//already attached so don't autoselect
                            continue;
                        }
                        if (adj.PatNum != PatCur.PatNum)
                        {
                            continue;
                        }
                    }
                    gridAccount.SetSelected(i, true);
                }
                if (gridAccount.SelectedIndices.Length == 0)
                {//if still none selected
                    MsgBox.Show(this, "Please select procedures, adjustments or payment plan charges first.");
                    return;
                }
            }
            else if (gridAccount.SelectedIndices.Length == 0
                && (Preference.GetBool(PreferenceName.ShowFeatureSuperfamilies) && guarantor.HasSuperBilling && superHead.HasSuperBilling))
            {
                //No selections and superbilling is enabled for this family.  Show a window to select and attach procs to this statement for the superfamily.
                FormInvoiceItemSelect FormIIS = new FormInvoiceItemSelect(PatCur.SuperFamily);
                if (FormIIS.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
                dictSuperFamItems = FormIIS.DictSelectedItems;
            }
            for (int i = 0; i < gridAccount.SelectedIndices.Length; i++)
            {
                DataRow row = table.Rows[gridAccount.SelectedIndices[i]];
                if (row["ProcNum"].ToString() == "0"
                    && row["AdjNum"].ToString() == "0"
                    && row["PayPlanChargeNum"].ToString() == "0") //the selected item is neither a procedure nor an adjustment
                {
                    MsgBox.Show(this, "You can only select procedures, payment plan charges or adjustments.");
                    gridAccount.SetSelected(false);
                    return;
                }
                if (row["ProcNum"].ToString() != "0")
                {//the selected item is a proc
                    Procedure proc = Procedures.GetOneProc(PIn.Long(row["ProcNum"].ToString()), false);
                    if (proc.PatNum != PatCur.PatNum)
                    {
                        MsgBox.Show(this, "You can only select procedures, payment plan charges or adjustments for the current patient on an invoice.");
                        gridAccount.SetSelected(false);
                        return;
                    }
                    if (proc.StatementNum != 0)
                    {
                        MsgBox.Show(this, "Selected procedure(s) are already attached to an invoice.");
                        gridAccount.SetSelected(false);
                        return;
                    }
                }
                else if (row["PayPlanChargeNum"].ToString() != "0")
                {
                    PayPlanCharge ppCharge = PayPlanCharges.GetOne(PIn.Long(row["PayPlanChargeNum"].ToString()));
                    if (ppCharge.PatNum != PatCur.PatNum)
                    {
                        MsgBox.Show(this, "You can only select procedures, payment plan charges or adjustments for a single patient on an invoice.");
                        gridAccount.SetSelected(false);
                        return;
                    }
                    if (ppCharge.ChargeType != PayPlanChargeType.Debit)
                    {
                        MsgBox.Show(this, "You can only select payment plans charges that are debits.");
                        gridAccount.SetSelected(false);
                        return;
                    }
                    if (ppCharge.StatementNum != 0)
                    {
                        MsgBox.Show(this, "Selected payment plan charges(s) are already attached to an invoice.");
                        gridAccount.SetSelected(false);
                        return;
                    }
                }
                else
                {//the selected item must be an adjustment
                    Adjustment adj = Adjustments.GetOne(PIn.Long(row["AdjNum"].ToString()));
                    if (adj.AdjDate.Date > DateTime.Today.Date && !Preference.GetBool(PreferenceName.FutureTransDatesAllowed))
                    {
                        MsgBox.Show(this, "Adjustments cannot be made for future dates");
                        return;
                    }
                    if (adj.PatNum != PatCur.PatNum)
                    {
                        MsgBox.Show(this, "You can only select procedures, payment plan charges or adjustments for a single patient on an invoice.");
                        gridAccount.SetSelected(false);
                        return;
                    }
                    if (adj.StatementNum != 0)
                    {
                        MsgBox.Show(this, "Selected adjustment(s) are already attached to an invoice.");
                        gridAccount.SetSelected(false);
                        return;
                    }
                }
            }
            //At this point, all selected items are procedures or adjustments, and are not already attached, and are for a single patient.
            Statement stmt = new Statement();
            stmt.PatNum = PatCur.PatNum;
            stmt.DateSent = DateTimeOD.Today;
            stmt.IsSent = false;
            stmt.Mode_ = StatementMode.InPerson;
            stmt.HidePayment = true;
            stmt.SinglePatient = true;
            stmt.Intermingled = false;
            stmt.IsReceipt = false;
            stmt.IsInvoice = true;
            stmt.StatementType = StmtType.NotSet;
            stmt.DateRangeFrom = DateTime.MinValue;
            stmt.DateRangeTo = DateTimeOD.Today;
            stmt.Note = Preference.GetString(PreferenceName.BillingDefaultsInvoiceNote);
            stmt.NoteBold = "";
            stmt.IsBalValid = true;
            stmt.BalTotal = guarantor.BalTotal;
            stmt.InsEst = guarantor.InsEst;
            if (dictSuperFamItems.Count > 0)
            {
                stmt.SuperFamily = PatCur.SuperFamily;
            }
            Statements.Insert(stmt);
            stmt.IsNew = true;
            List<Procedure> procsForPat = Procedures.Refresh(PatCur.PatNum);
            for (int i = 0; i < gridAccount.SelectedIndices.Length; i++)
            {
                DataRow row = table.Rows[gridAccount.SelectedIndices[i]];
                if (row["ProcNum"].ToString() != "0")
                {//if selected item is a procedure
                    Procedure proc = Procedures.GetProcFromList(procsForPat, PIn.Long(row["ProcNum"].ToString()));
                    Procedure oldProc = proc.Copy();
                    proc.StatementNum = stmt.StatementNum;
                    if (proc.ProcStatus == ProcStat.C && proc.ProcDate.Date > DateTime.Today.Date && !Preference.GetBool(PreferenceName.FutureTransDatesAllowed))
                    {
                        MsgBox.Show(this, "Completed procedures cannot be set for future dates.");
                        return;
                    }
                    Procedures.Update(proc, oldProc);
                }
                else if (row["PayPlanChargeNum"].ToString() != "0")
                {
                    PayPlanCharge ppCharge = PayPlanCharges.GetOne(PIn.Long(row["PayPlanChargeNum"].ToString()));
                    ppCharge.StatementNum = stmt.StatementNum;
                    PayPlanCharges.Update(ppCharge);
                }
                else
                {//selected item must be adjustment
                    Adjustment adj = Adjustments.GetOne(PIn.Long(row["AdjNum"].ToString()));
                    adj.StatementNum = stmt.StatementNum;
                    Adjustments.Update(adj);
                }
            }
            foreach (KeyValuePair<string, List<long>> entry in dictSuperFamItems)
            {//Should really only have three keys, Proc, Pay Plan, and Adj
                if (entry.Key == "Proc")
                {//Procedure key, loop through all procedures
                    foreach (long priKey in entry.Value)
                    {
                        Procedure newProc = Procedures.GetOneProc(priKey, false);
                        Procedure oldProc = newProc.Copy();
                        newProc.StatementNum = stmt.StatementNum;
                        if (newProc.ProcStatus == ProcStat.C && newProc.ProcDate.Date > DateTime.Today.Date && !Preference.GetBool(PreferenceName.FutureTransDatesAllowed))
                        {
                            MsgBox.Show(this, "Procedures cannot be set for future dates.");
                            return;
                        }
                        Procedures.Update(newProc, oldProc);
                    }
                }
                else if (entry.Key == "Pay Plan")
                {
                    foreach (long priKey in entry.Value)
                    {
                        PayPlanCharge newCharge = PayPlanCharges.GetOne(priKey);
                        newCharge.StatementNum = stmt.StatementNum;
                        PayPlanCharges.Update(newCharge);
                    }
                }
                else
                {//Adjustment key, loop through all adjustments
                    foreach (long priKey in entry.Value)
                    {
                        Adjustment adj = Adjustments.GetOne(priKey);
                        adj.StatementNum = stmt.StatementNum;
                        Adjustments.Update(adj);
                    }
                }
            }
            //All printing and emailing will be done from within the form:
            FormStatementOptions FormSO = new FormStatementOptions();
            FormSO.StmtCur = stmt;
            FormSO.ShowDialog();
            if (FormSO.DialogResult != DialogResult.OK)
            {
                Statements.Delete(stmt.StatementNum);//detached from adjustments, procedurelogs, and paysplits as well
            }
            ModuleSelected(PatCur.PatNum);
        }

        private void menuItemLimited_Click(object sender, EventArgs e)
        {
            DataTable table = DataSetMain.Tables["account"];
            Patient guarantor = Patients.GetPat(PatCur.Guarantor);
            DataRow row;
            #region Autoselect Today's Procedures
            if (gridAccount.SelectedIndices.Length == 0)
            {//autoselect procedures
                for (int i = 0; i < table.Rows.Count; i++)
                {//loop through every line showing on screen
                    row = table.Rows[i];
                    if (row["ProcNum"].ToString() == "0" //ignore items that aren't procs
                        || PIn.Date(row["date"].ToString()) != DateTime.Today //autoselecting todays procs only
                        || PIn.Long(row["PatNum"].ToString()) != PatCur.PatNum) //only procs for the current patient
                    {
                        continue;
                    }
                    gridAccount.SetSelected(i, true);
                }
            }
            #endregion Autoselect Today's Procedures
            List<long> listPatNumsSelected = new List<long>();
            List<long> listProcClaimNums = new List<long>();
            List<long> listPaymentClaimNums = new List<long>();
            List<long> listProcNums = new List<long>();
            List<long> listAdjNums = new List<long>();
            List<long> listPayNums = new List<long>();
            if (gridAccount.SelectedIndices.Length == 0)
            {
                //if the nothing is selected still show the limited picker window.
                FormLimitedStatementSelect formLimitedStatementSelect = new FormLimitedStatementSelect(table.Copy());
                if (formLimitedStatementSelect.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                listPatNumsSelected = formLimitedStatementSelect.ListPatNumsSelected;
                listProcClaimNums = formLimitedStatementSelect.ListSelectedProcClaimNums;
                listPaymentClaimNums = formLimitedStatementSelect.ListSelectedPaymentClaimNums;
                listProcNums = formLimitedStatementSelect.ListSelectedProcedureNums;
                listAdjNums = formLimitedStatementSelect.ListSelectedAdjustments;
                listPayNums = formLimitedStatementSelect.ListSelectedPayNums;
            }
            else
            {
                //guaranteed to have rows selected from here down, verify they are allowed transactions
                if (gridAccount.SelectedIndices.Any(x => table.Rows[x]["StatementNum"].ToString() != "0" || table.Rows[x]["PayPlanNum"].ToString() != "0"))
                {
                    MsgBox.Show(this, "You can only select procedures, adjustments, payments, and claims.");
                    gridAccount.SetSelected(false);
                    return;
                }
                //get all ClaimNums from claimprocs for the selected procs
                listProcClaimNums = ClaimProcs.GetForProcs(gridAccount.SelectedIndices.Where(x => table.Rows[x]["ProcNum"].ToString() != "0")
                    .Select(x => PIn.Long(table.Rows[x]["ProcNum"].ToString())).ToList()).FindAll(x => x.ClaimNum != 0).Select(x => x.ClaimNum).ToList();
                //get all ClaimNums for any selected claimpayments
                listPaymentClaimNums = gridAccount.SelectedIndices
                    .Where(x => table.Rows[x]["ClaimNum"].ToString() != "0" && table.Rows[x]["ClaimPaymentNum"].ToString() == "1")
                    .Select(x => PIn.Long(table.Rows[x]["ClaimNum"].ToString())).ToList();
                //prevent user from selecting a claimpayment that is not associatede with any of the selected procs
                if (listPaymentClaimNums.Any(x => !listProcClaimNums.Contains(x)))
                {
                    MsgBox.Show(this, "You can only select claim payments for the selected procedures.");
                    gridAccount.SetSelected(false);
                    return;
                }
                listPatNumsSelected = gridAccount.SelectedIndices.Select(x => table.Rows[x]["PatNum"].ToString()).Distinct().Select(x => PIn.Long(x)).ToList();
                listAdjNums = gridAccount.SelectedIndices
                    .Where(x => table.Rows[x]["AdjNum"].ToString() != "0")
                    .Select(x => PIn.Long(table.Rows[x]["AdjNum"].ToString())).ToList();
                listPayNums = gridAccount.SelectedIndices
                    .Where(x => table.Rows[x]["PayNum"].ToString() != "0")
                    .Select(x => PIn.Long(table.Rows[x]["PayNum"].ToString())).ToList();
                listProcNums = gridAccount.SelectedIndices
                    .Where(x => table.Rows[x]["ProcNum"].ToString() != "0")
                    .Select(x => PIn.Long(table.Rows[x]["ProcNum"].ToString())).ToList();
            }
            //At this point, all selected items are procedures, adjustments, payments, or claims.
            Statement stmt = Statements.CreateLimitedStatement(listPatNumsSelected, PatCur.PatNum, listProcClaimNums, listPaymentClaimNums, listAdjNums, listPayNums
                , listProcNums);
            //All printing and emailing will be done from within the form:
            FormStatementOptions FormSO = new FormStatementOptions();
            FormSO.StmtCur = stmt;
            FormSO.ShowDialog();
            if (FormSO.DialogResult != DialogResult.OK)
            {
                Statements.Delete(stmt.StatementNum);//detached from adjustments, procedurelogs, and paysplits as well
            }
            ModuleSelected(PatCur.PatNum);
        }

        private void menuItemStatementMore_Click(object sender, System.EventArgs e)
        {
            Statement stmt = new Statement();
            stmt.PatNum = PatCur.PatNum;
            stmt.DateSent = DateTime.Today;
            stmt.IsSent = false;
            stmt.Mode_ = StatementMode.InPerson;
            stmt.HidePayment = false;
            stmt.SinglePatient = false;
            stmt.Intermingled = Preference.GetBool(PreferenceName.IntermingleFamilyDefault);
            stmt.IsReceipt = false;
            stmt.StatementType = StmtType.NotSet;
            stmt.DateRangeFrom = DateTime.MinValue;
            stmt.DateRangeFrom = DateTime.MinValue;
            if (textDateStart.errorProvider1.GetError(textDateStart) == "")
            {
                if (textDateStart.Text != "")
                {
                    stmt.DateRangeFrom = PIn.Date(textDateStart.Text);
                }
            }
            if (Preference.GetBool(PreferenceName.FuchsOptionsOn))
            {
                stmt.DateRangeFrom = DateTime.Today.AddDays(-90);
            }
            stmt.DateRangeTo = DateTime.Today;//Needed for payplan accuracy.//new DateTime(2200,1,1);
            if (textDateEnd.errorProvider1.GetError(textDateEnd) == "")
            {
                if (textDateEnd.Text != "")
                {
                    stmt.DateRangeTo = PIn.Date(textDateEnd.Text);
                }
            }
            stmt.Note = "";
            stmt.NoteBold = "";
            Patient guarantor = null;
            if (PatCur != null)
            {
                guarantor = Patients.GetPat(PatCur.Guarantor);
            }
            if (guarantor != null)
            {
                stmt.IsBalValid = true;
                stmt.BalTotal = guarantor.BalTotal;
                stmt.InsEst = guarantor.InsEst;
            }
            //All printing and emailing will be done from within the form:
            FormStatementOptions FormSO = new FormStatementOptions();
            stmt.IsNew = true;
            FormSO.StmtCur = stmt;
            FormSO.ShowDialog();
            ModuleSelected(PatCur.PatNum);
        }

        /// <summary>Saves the statement.  Attaches a pdf to it by creating a doc object.  Prints it or emails it.  </summary>
        private void PrintStatement(Statement stmt)
        {
            Cursor = Cursors.WaitCursor;
            Statements.Insert(stmt);
            SheetDef sheetDef = SheetUtil.GetStatementSheetDef();
            Sheet sheet = SheetUtil.CreateSheet(sheetDef, stmt.PatNum, stmt.HidePayment);
            DataSet dataSet = AccountModules.GetAccount(stmt.PatNum, stmt);
            sheet.Parameters.Add(new SheetParameter(true, "Statement") { ParamValue = stmt });
            SheetFiller.FillFields(sheet, dataSet, stmt);
            SheetUtil.CalculateHeights(sheet, dataSet, stmt);
            string tempPath = CodeBase.ODFileUtils.CombinePaths(Preferences.GetTempFolderPath(), stmt.PatNum.ToString() + ".pdf");
            SheetPrinting.CreatePdf(sheet, tempPath, stmt, dataSet, null);
            long category = 0;
            List<Definition> listDefs = Definition.GetByCategory(DefinitionCategory.ImageCats);;
            for (int i = 0; i < listDefs.Count; i++)
            {
                if (Regex.IsMatch(listDefs[i].Value, @"S"))
                {
                    category = listDefs[i].Id;
                    break;
                }
            }
            if (category == 0)
            {
                category = listDefs[0].Id;//put it in the first category.
            }
            //create doc--------------------------------------------------------------------------------------
            OpenDentBusiness.Document docc = null;
            try
            {
                docc = ImageStore.Import(tempPath, category, Patients.GetPat(stmt.PatNum));
            }
            catch
            {
                MsgBox.Show(this, "Error saving document.");
                //this.Cursor=Cursors.Default;
                return;
            }
            docc.ImgType = ImageType.Document;
            docc.DateCreated = stmt.DateSent;
            stmt.DocNum = docc.DocNum;//this signals the calling class that the pdf was created successfully.
            Statements.AttachDoc(stmt.StatementNum, docc);
            //if(ImageStore.UpdatePatient == null){
            //	ImageStore.UpdatePatient = new FileStore.UpdatePatientDelegate(Patients.Update);
            //}
            Patient guar = Patients.GetPat(stmt.PatNum);
            string guarFolder = ImageStore.GetPatientFolder(guar, ImageStore.GetPreferredAtoZpath());
            //OpenDental.Imaging.ImageStoreBase imageStore = OpenDental.Imaging.ImageStore.GetImageStore(guar);
            if (stmt.Mode_ == StatementMode.Email)
            {
                if (!Security.IsAuthorized(Permissions.EmailSend))
                {
                    Cursor = Cursors.Default;
                    return;
                }
                string attachPath = EmailAttachment.GetAttachmentPath();
                Random rnd = new Random();
                string fileName = DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.TimeOfDay.Ticks.ToString() + rnd.Next(1000).ToString() + ".pdf";
                string filePathAndName = FileAtoZ.CombinePaths(attachPath, fileName);
                FileAtoZ.Copy(ImageStore.GetFilePath(Documents.GetByNum(stmt.DocNum), guarFolder), filePathAndName, FileAtoZSourceDestination.AtoZToAtoZ);
                //Process.Start(filePathAndName);
                EmailMessage message = Statements.GetEmailMessageForStatement(stmt, guar);
                EmailAttachment attach = new EmailAttachment();
                attach.Description = "Statement.pdf";
                attach.FileName = fileName;
                message.Attachments.Add(attach);
                FormEmailMessageEdit FormE = new FormEmailMessageEdit(message, EmailAddress.GetByClinic(guar.ClinicNum));
                FormE.ShowDialog();
                //If user clicked delete or cancel, delete pdf and statement
                if (FormE.DialogResult == DialogResult.Cancel)
                {
                    Patient pat;
                    string patFolder;
                    if (stmt.DocNum != 0)
                    {
                        //delete the pdf
                        pat = Patients.GetPat(stmt.PatNum);
                        patFolder = ImageStore.GetPatientFolder(pat, ImageStore.GetPreferredAtoZpath());
                        List<Document> listdocs = new List<Document>();
                        listdocs.Add(Documents.GetByNum(stmt.DocNum));
                        try
                        {
                            ImageStore.DeleteDocuments(listdocs, patFolder);
                        }
                        catch
                        {  //Image could not be deleted, in use.
                           //This should never get hit because the file was created by this user within this method.  
                           //If the doc cannot be deleted, then we will not stop them, they will have to manually delete it from the images module.
                        }
                    }
                    //delete statement
                    Statements.Delete(stmt);
                }
            }
            else
            {//not email
#if DEBUG
                //don't bother to check valid path because it's just debug.
                Document doc = Documents.GetByNum(stmt.DocNum);
                string imgPath = ImageStore.GetFilePath(doc, guarFolder);
                DateTime now = DateTime.Now;
                while (DateTime.Now < now.AddSeconds(5) && !FileAtoZ.Exists(imgPath))
                {//wait up to 5 seconds.
                    Application.DoEvents();
                }
                try
                {
                    FileAtoZ.StartProcess(imgPath);
                }
                catch (Exception ex)
                {
                    FormFriendlyException.Show($"Unable to open the following file: {doc.FileName}", ex);
                }
#else
				//Thread thread=new Thread(new ParameterizedThreadStart(SheetPrinting.PrintStatement));
				//thread.Start(new List<object> { sheetDef,stmt,tempPath });
				//NOTE: This is printing a "fresh" GDI+ version of the statment which is ever so slightly different than the PDFSharp statment that was saved to disk.
				sheet=SheetUtil.CreateSheet(sheetDef,stmt.PatNum,stmt.HidePayment);
				SheetFiller.FillFields(sheet,dataSet,stmt);
				SheetUtil.CalculateHeights(sheet,dataSet,stmt);
				SheetPrinting.Print(sheet,1,false,stmt);//use GDI+ printing, which is slightly different than the pdf.
#endif
            }
            Cursor = Cursors.Default;

        }

        private void textUrgFinNote_TextChanged(object sender, System.EventArgs e)
        {
            UrgFinNoteChanged = true;
        }

        private void textFinNote_TextChanged(object sender, System.EventArgs e)
        {
            FinNoteChanged = true;
        }

        //private void textCC_TextChanged(object sender,EventArgs e) {
        //  CCChanged=true;
        //  if(Regex.IsMatch(textCC.Text,@"^\d{4}$")
        //    || Regex.IsMatch(textCC.Text,@"^\d{4}-\d{4}$")
        //    || Regex.IsMatch(textCC.Text,@"^\d{4}-\d{4}-\d{4}$")) 
        //  {
        //    textCC.Text=textCC.Text+"-";
        //    textCC.Select(textCC.Text.Length,0);
        //  }
        //}

        //private void textCCexp_TextChanged(object sender,EventArgs e) {
        //  CCChanged=true;
        //}

        private void textUrgFinNote_Leave(object sender, System.EventArgs e)
        {
            //need to skip this if selecting another module. Handled in ModuleUnselected due to click event
            UpdateUrgFinNote();
        }

        public void UpdateUrgFinNote()
        {
            if (FamCur == null)
                return;
            if (UrgFinNoteChanged)
            {
                Patient PatOld = FamCur.ListPats[0].Copy();
                FamCur.ListPats[0].FamFinUrgNote = textUrgFinNote.Text;
                Patients.Update(FamCur.ListPats[0], PatOld);
                UrgFinNoteChanged = false;
            }
        }

        private void textFinNote_Leave(object sender, System.EventArgs e)
        {
            UpdateFinNote();
        }

        public void UpdateFinNote()
        {
            if (FamCur == null)
                return;
            if (FinNoteChanged)
            {
                PatientNoteCur.FamFinancial = textFinNote.Text;
                PatientNotes.Update(PatientNoteCur, PatCur.Guarantor);
                FinNoteChanged = false;
            }
        }

        //private void textCC_Leave(object sender,EventArgs e) {
        //  if(FamCur==null)
        //    return;
        //  if(CCChanged) {
        //    CCSave();
        //    CCChanged=false;
        //    ModuleSelected(PatCur.PatNum);
        //  }
        //}

        //private void textCCexp_Leave(object sender,EventArgs e) {
        //  if(FamCur==null)
        //    return;
        //  if(CCChanged){
        //    CCSave();
        //    CCChanged=false;
        //    ModuleSelected(PatCur.PatNum);
        //  }
        //}

        //private void CCSave(){
        //  string cc=textCC.Text;
        //  if(Regex.IsMatch(cc,@"^\d{4}-\d{4}-\d{4}-\d{4}$")){
        //    PatientNoteCur.CCNumber=cc.Substring(0,4)+cc.Substring(5,4)+cc.Substring(10,4)+cc.Substring(15,4);
        //  }
        //  else{
        //    PatientNoteCur.CCNumber=cc;
        //  }
        //  string exp=textCCexp.Text;
        //  if(Regex.IsMatch(exp,@"^\d\d[/\- ]\d\d$")){//08/07 or 08-07 or 08 07
        //    PatientNoteCur.CCExpiration=new DateTime(Convert.ToInt32("20"+exp.Substring(3,2)),Convert.ToInt32(exp.Substring(0,2)),1);
        //  }
        //  else if(Regex.IsMatch(exp,@"^\d{4}$")){//0807
        //    PatientNoteCur.CCExpiration=new DateTime(Convert.ToInt32("20"+exp.Substring(2,2)),Convert.ToInt32(exp.Substring(0,2)),1);
        //  } 
        //  else if(exp=="") {
        //    PatientNoteCur.CCExpiration=new DateTime();//Allow the experation date to be deleted.
        //  } 
        //  else {
        //    MsgBox.Show(this,"Expiration format invalid.");
        //  }
        //  PatientNotes.Update(PatientNoteCur,PatCur.Guarantor);
        //}

        private void butToday_Click(object sender, EventArgs e)
        {
            textDateStart.Text = DateTime.Today.ToShortDateString();
            textDateEnd.Text = DateTime.Today.ToShortDateString();
            ModuleSelected(PatCur.PatNum);
        }

        private void but45days_Click(object sender, EventArgs e)
        {
            textDateStart.Text = DateTime.Today.AddDays(-45).ToShortDateString();
            textDateEnd.Text = "";
            ModuleSelected(PatCur.PatNum);
        }

        private void but90days_Click(object sender, EventArgs e)
        {
            textDateStart.Text = DateTime.Today.AddDays(-90).ToShortDateString();
            textDateEnd.Text = "";
            ModuleSelected(PatCur.PatNum);
        }

        private void butDatesAll_Click(object sender, EventArgs e)
        {
            textDateStart.Text = "";
            textDateEnd.Text = "";
            ModuleSelected(PatCur.PatNum);
        }

        private void butRefresh_Click(object sender, EventArgs e)
        {
            if (PatCur == null)
            {
                return;
            }
            ModuleSelected(PatCur.PatNum);
        }

        private void checkShowDetail_Click(object sender, EventArgs e)
        {
            UserOdPref userOdPrefProcBreakdown = UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum, UserOdFkeyType.AcctProcBreakdown).FirstOrDefault();
            if (userOdPrefProcBreakdown == null)
            {
                userOdPrefProcBreakdown = new UserOdPref();
                userOdPrefProcBreakdown.UserNum = Security.CurUser.UserNum;
                userOdPrefProcBreakdown.FkeyType = UserOdFkeyType.AcctProcBreakdown;
                userOdPrefProcBreakdown.Fkey = 0;
            }
            userOdPrefProcBreakdown.ValueString = POut.Bool(checkShowDetail.Checked);
            UserOdPrefs.Upsert(userOdPrefProcBreakdown);
            if (PatCur == null)
            {
                return;
            }
            ModuleSelected(PatCur.PatNum);
        }

        private void toolBarButComm_Click()
        {
            FormPat form = new FormPat();
            form.PatNum = PatCur.PatNum;
            form.FormDateTime = DateTime.Now;
            FormFormPatEdit FormP = new FormFormPatEdit();
            FormP.FormPatCur = form;
            FormP.IsNew = true;
            FormP.ShowDialog();
            if (FormP.DialogResult == DialogResult.OK)
            {
                ModuleSelected(PatCur.PatNum);
            }
        }

        void creditCardButton_Click(object sender, EventArgs e)
        {
            using (var formCreditCardManage = new FormCreditCardManage(PatCur))
            {
                formCreditCardManage.ShowDialog();
            }
        }

        private void toolBarButTrojan_Click()
        {
            FormTrojanCollect FormT = new FormTrojanCollect();
            FormT.PatNum = PatCur.PatNum;
            FormT.ShowDialog();
        }

        private void toolBarButQuickProcs_Click()
        {
            if (!Security.IsAuthorized(Permissions.AccountProcsQuickAdd, true))
            {
                //only happens if permissions are changed after the program is opened. (Very Rare)
                MsgBox.Show(this, "Not authorized for Quick Procs.");
                return;
            }
            //Main QuickCharge button was clicked.  Create a textbox that can be entered so users can insert manually entered proc codes.
            if (!Security.IsAuthorized(Permissions.ProcComplCreate, true))
            {//Button doesn't show up unless they have AccountQuickCharge permission. 
             //user can still use dropdown, just not type in codes.
                contextMenuQuickProcs.Show(this, new Point(_butQuickProcs.Bounds.X, _butQuickProcs.Bounds.Y + _butQuickProcs.Bounds.Height));
                return;
            }
            textQuickProcs.SetBounds(_butQuickProcs.Bounds.X + 1, _butQuickProcs.Bounds.Y + 2, _butQuickProcs.Bounds.Width - 17, _butQuickProcs.Bounds.Height - 2);
            textQuickProcs.Visible = true;
            textQuickProcs.BringToFront();
            textQuickProcs.Focus();
            textQuickProcs.Capture = true;
        }

        private void textQuickCharge_FocusLost(object sender, EventArgs e)
        {
            textQuickProcs.Text = "";
            textQuickProcs.Visible = false;
            textQuickProcs.Capture = false;
        }

        private void textQuickCharge_KeyDown(object sender, KeyEventArgs e)
        {
            //This is only the KeyDown event, user can still type if we return here.
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
            textQuickProcs.Visible = false;
            textQuickProcs.Capture = false;
            e.Handled = true;//Suppress the "ding" in windows when pressing enter.
            e.SuppressKeyPress = true;//Suppress the "ding" in windows when pressing enter.
            if (textQuickProcs.Text == "")
            {
                return;
            }
            Provider patProv = Providers.GetProv(PatCur.PriProv);
            if (AddProcAndValidate(textQuickProcs.Text, patProv))
            {
                SecurityLogs.MakeLogEntry(Permissions.AccountProcsQuickAdd, PatCur.PatNum
                    , Lan.g(this, "The following procedures were added via the Quick Charge button from the Account module")
                        + ": " + string.Join(",", textQuickProcs.Text));
                ModuleSelected(PatCur.PatNum);
            }
            textQuickProcs.Text = "";
        }

        private void menuItemPrePay_Click(object sender, EventArgs e)
        {
            toolBarButPay_Click(0, isPrePay: true, isIncomeTransfer: true);
        }

        private void menuItemQuickProcs_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.AccountProcsQuickAdd, true))
            {
                //only happens if permissions are changed after the program is opened or a different user logs in
                MsgBox.Show(this, "Not authorized for Quick Procs.");
                return;
            }
            //One of the QuickCharge menu items was clicked.
            if (sender.GetType() != typeof(MenuItem))
            {
                return;
            }
            Definition quickChargeDef = _acctProcQuickAddDefs[contextMenuQuickProcs.MenuItems.IndexOf((MenuItem)sender)];
            string[] procCodes = quickChargeDef.Value.Split(',');
            if (procCodes.Length == 0)
            {
                //No items entered into the definition category.  Notify the user.
                MsgBox.Show(this, "There are no Quick Charge items in Setup | Definitions.  There must be at least one in order to use the Quick Charge drop down menu.");
            }
            List<string> procCodesAdded = new List<string>();
            Provider patProv = Providers.GetProv(PatCur.PriProv);
            for (int i = 0; i < procCodes.Length; i++)
            {
                if (AddProcAndValidate(procCodes[i], patProv))
                {
                    procCodesAdded.Add(procCodes[i]);
                }
            }
            if (procCodesAdded.Count > 0)
            {
                SecurityLogs.MakeLogEntry(Permissions.AccountProcsQuickAdd, PatCur.PatNum
                    , Lan.g(this, "The following procedures were added via the Quick Charge button from the Account module")
                        + ": " + string.Join(",", procCodesAdded));
                ModuleSelected(PatCur.PatNum);
            }
        }

        ///<summary>Validated the procedure code using FormProcEdit and prompts user for input if required.</summary>
        private bool AddProcAndValidate(string procString, Provider patProv)
        {
            ProcedureCode procCode = ProcedureCodes.GetProcCode(procString);
            if (procCode.CodeNum == 0)
            {
                MsgBox.Show(this, "Invalid Procedure Code: " + procString);
                return false; //Invalid ProcCode string manually entered.
            }
            Procedure proc = new Procedure();
            proc.ProcStatus = ProcStat.C;
            proc.ClinicNum = PatCur.ClinicNum;
            proc.CodeNum = procCode.CodeNum;
            proc.DateEntryC = DateTime.Now;
            proc.DateTP = DateTime.Now;
            proc.PatNum = PatCur.PatNum;
            proc.ProcDate = DateTime.Now;
            proc.ToothRange = "";
            proc.PlaceService = (PlaceOfService)Preference.GetInt(PreferenceName.DefaultProcedurePlaceService);//Default Proc Place of Service for the Practice is used. 
            if (!Preference.GetBool(PreferenceName.EasyHidePublicHealth))
            {
                proc.SiteNum = PatCur.SiteNum;
            }
            proc.ProvNum = procCode.ProvNumDefault;//use proc default prov if set
            if (proc.ProvNum == 0)
            { //if none set, use primary provider.
                proc.ProvNum = patProv.ProvNum;
            }
            List<InsSub> insSubList = InsSubs.RefreshForFam(FamCur);
            List<InsPlan> insPlanList = InsPlans.RefreshForSubList(insSubList);
            List<PatPlan> patPlanList = PatPlans.Refresh(PatCur.PatNum);
            InsPlan insPlanPrimary = null;
            InsSub insSubPrimary = null;
            if (patPlanList.Count > 0)
            {
                insSubPrimary = InsSubs.GetSub(patPlanList[0].InsSubNum, insSubList);
                insPlanPrimary = InsPlans.GetPlan(insSubPrimary.PlanNum, insPlanList);
            }
            proc.MedicalCode = procCode.MedicalCode;
            proc.ProcFee = Procedures.GetProcFee(PatCur, patPlanList, insSubList, insPlanList, proc.CodeNum, proc.ProvNum, proc.ClinicNum, proc.MedicalCode);
            proc.UnitQty = 1;
            Procedures.Insert(proc);
            //launch form silently to validate code. If entry errors occur the form will be shown to user, otherwise it will close immediately.
            FormProcEdit FormPE = new FormProcEdit(proc, PatCur, FamCur, true);
            FormPE.IsNew = true;
            FormPE.ShowDialog();
            if (FormPE.DialogResult != DialogResult.OK)
            {
                Procedures.Delete(proc.ProcNum);
                return false;
            }
            if (proc.ProcStatus == ProcStat.C)
            {
                AutomationL.Trigger(AutomationTrigger.CompleteProcedure, new List<string>() { ProcedureCodes.GetStringProcCode(proc.CodeNum) }, PatCur.PatNum);
            }
            return true;
        }

        private void gridComm_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e)
        {
            int row = (int)gridComm.Rows[e.Row].Tag;
            if (DataSetMain.Tables["Commlog"].Rows[row]["CommlogNum"].ToString() != "0")
            {
                Commlog CommlogCur =
                    Commlogs.GetOne(PIn.Long(DataSetMain.Tables["Commlog"].Rows[row]["CommlogNum"].ToString()));
                if (CommlogCur == null)
                {
                    MsgBox.Show(this, "This commlog has been deleted by another user.");
                    ModuleSelected(PatCur.PatNum);
                }
                else
                {
                    FormCommItem FormCI = new FormCommItem(CommlogCur);
                    if (FormCI.ShowDialog() == DialogResult.OK)
                    {
                        ModuleSelected(PatCur.PatNum);
                    }
                }
            }
            else if (DataSetMain.Tables["Commlog"].Rows[row]["EmailMessageNum"].ToString() != "0")
            {
                EmailMessage email =
                    EmailMessage.GetById(PIn.Long(DataSetMain.Tables["Commlog"].Rows[row]["EmailMessageNum"].ToString()));

                    FormEmailMessageEdit FormE = new FormEmailMessageEdit(email);
                    FormE.ShowDialog();
                    if (FormE.DialogResult == DialogResult.OK)
                    {
                        ModuleSelected(PatCur.PatNum);
                    }
                
            }
            else if (DataSetMain.Tables["Commlog"].Rows[row]["FormPatNum"].ToString() != "0")
            {
                FormPat form = FormPats.GetOne(PIn.Long(DataSetMain.Tables["Commlog"].Rows[row]["FormPatNum"].ToString()));
                FormFormPatEdit FormP = new FormFormPatEdit();
                FormP.FormPatCur = form;
                FormP.ShowDialog();
                if (FormP.DialogResult == DialogResult.OK)
                {
                    ModuleSelected(PatCur.PatNum);
                }
            }
            else if (DataSetMain.Tables["Commlog"].Rows[row]["SheetNum"].ToString() != "0")
            {
                Sheet sheet = Sheets.GetSheet(PIn.Long(DataSetMain.Tables["Commlog"].Rows[row]["SheetNum"].ToString()));
                FormSheetFillEdit.ShowForm(sheet, FormSheetFillEdit_FormClosing);
            }
        }

        /// <summary>Event handler for closing FormSheetFillEdit when it is non-modal.</summary>
        private void FormSheetFillEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (((FormSheetFillEdit)sender).DialogResult == DialogResult.OK || ((FormSheetFillEdit)sender).DidChangeSheet)
            {
                ModuleSelected(PatCur.PatNum);
            }
        }

        private void Parent_MouseWheel(Object sender, MouseEventArgs e)
        {
            if (Visible)
            {
                this.OnMouseWheel(e);
            }
        }

        private void gridRepeat_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e)
        {
            FormRepeatChargeEdit FormR = new FormRepeatChargeEdit(RepeatChargeList[e.Row]);
            FormR.ShowDialog();
            ModuleSelected(PatCur.PatNum);
        }

        private void gridPatInfo_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (TerminalActives.PatIsInUse(PatCur.PatNum))
            {
                MsgBox.Show(this, "Patient is currently entering info at a reception terminal.  Please try again later.");
                return;
            }
            if (patientInfoGrid.Rows[e.Row].Tag is PatFieldDef)
            {//patfield for an existing PatFieldDef
                PatFieldDef patFieldDef = (PatFieldDef)patientInfoGrid.Rows[e.Row].Tag;
                PatField field = PatFields.GetByName(patFieldDef.FieldName, _patFieldList);
                PatFieldL.OpenPatField(field, patFieldDef, PatCur.PatNum);
            }
            else if (patientInfoGrid.Rows[e.Row].Tag is PatField)
            {//PatField for a PatFieldDef that no longer exists
                PatField field = (PatField)patientInfoGrid.Rows[e.Row].Tag;
                FormPatFieldEdit FormPF = new FormPatFieldEdit(field);
                FormPF.ShowDialog();
            }
            else
            {
                FormPatientEdit FormP = new FormPatientEdit(PatCur, FamCur);
                FormP.IsNew = false;
                FormP.ShowDialog();
                if (FormP.DialogResult == DialogResult.OK)
                {
                    FormOpenDental.S_Contr_PatientSelected(PatCur, false);
                }
            }
            ModuleSelected(PatCur.PatNum);
        }

        #region ProgressNotes
        ///<summary>The supplied procedure row must include these columns: ProcDate,ProcStatus,ProcCode,Surf,ToothNum, and ToothRange, all in raw database format.</summary>
        private bool ShouldDisplayProc(DataRow row)
        {
            switch ((ProcStat)PIn.Long(row["ProcStatus"].ToString()))
            {
                case ProcStat.TP:
                    if (checkShowTP.Checked)
                    {
                        return true;
                    }
                    break;
                case ProcStat.C:
                    if (checkShowC.Checked)
                    {
                        return true;
                    }
                    break;
                case ProcStat.EC:
                    if (checkShowE.Checked)
                    {
                        return true;
                    }
                    break;
                case ProcStat.EO:
                    if (checkShowE.Checked)
                    {
                        return true;
                    }
                    break;
                case ProcStat.R:
                    if (checkShowR.Checked)
                    {
                        return true;
                    }
                    break;
                case ProcStat.D:
                    if (checkAudit.Checked)
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        private void FillProgNotes()
        {
            ArrayList selectedTeeth = new ArrayList();//integers 1-32
            for (int i = 0; i < 32; i++)
            {
                selectedTeeth.Add(i);
            }
            gridProg.BeginUpdate();
            gridProg.Columns.Clear();
            ODGridColumn col = new ODGridColumn(Lan.g("TableProg", "Date"), 67);
            gridProg.Columns.Add(col);
            if (!Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum))
            {
                col = new ODGridColumn(Lan.g("TableProg", "Th"), 27);
                gridProg.Columns.Add(col);
                col = new ODGridColumn(Lan.g("TableProg", "Surf"), 40);
                gridProg.Columns.Add(col);
            }
            col = new ODGridColumn(Lan.g("TableProg", "Dx"), 28);
            gridProg.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableProg", "Description"), 218);
            gridProg.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableProg", "Stat"), 25);
            gridProg.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableProg", "Prov"), 42);
            gridProg.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableProg", "Amount"), 48, textAlignment: HorizontalAlignment.Right);
            gridProg.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableProg", "ADA Code"), 62, textAlignment: HorizontalAlignment.Center);
            gridProg.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableProg", "User"), 62, textAlignment: HorizontalAlignment.Center);
            gridProg.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableProg", "Signed"), 55, textAlignment: HorizontalAlignment.Center);
            gridProg.Columns.Add(col);
            gridProg.NoteSpanStart = 2;
            gridProg.NoteSpanStop = 7;
            gridProg.Rows.Clear();
            ODGridRow row;
            //Type type;
            if (DataSetMain == null)
            {
                gridProg.EndUpdate();
                return;
            }
            DataTable table = DataSetMain.Tables["ProgNotes"];
            //ProcList = new List<DataRow>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i]["ProcNum"].ToString() != "0")
                {//if this is a procedure
                    if (ShouldDisplayProc(table.Rows[i]))
                    {
                        //ProcList.Add(table.Rows[i]);//show it in the graphical tooth chart
                        //and add it to the grid below.
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (table.Rows[i]["CommlogNum"].ToString() != "0")
                {//if this is a commlog
                    if (!checkComm.Checked)
                    {
                        continue;
                    }
                }
                else if (table.Rows[i]["RxNum"].ToString() != "0")
                {//if this is an Rx
                    if (!checkRx.Checked)
                    {
                        continue;
                    }
                }
                else if (table.Rows[i]["LabCaseNum"].ToString() != "0")
                {//if this is a LabCase
                    if (!checkLabCase.Checked)
                    {
                        continue;
                    }
                }
                else if (table.Rows[i]["AptNum"].ToString() != "0")
                {//if this is an Appointment
                    if (!checkAppt.Checked)
                    {
                        continue;
                    }
                }

                row = new ODGridRow();
                row.ColorLborder = Color.Black;
                //remember that columns that start with lowercase are already altered for display rather than being raw data.
                row.Cells.Add(table.Rows[i]["procDate"].ToString());
                if (!Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum))
                {
                    row.Cells.Add(table.Rows[i]["toothNum"].ToString());
                    row.Cells.Add(table.Rows[i]["Surf"].ToString());
                }
                row.Cells.Add(table.Rows[i]["dx"].ToString());
                row.Cells.Add(table.Rows[i]["description"].ToString());
                row.Cells.Add(table.Rows[i]["procStatus"].ToString());
                row.Cells.Add(table.Rows[i]["prov"].ToString());
                row.Cells.Add(table.Rows[i]["procFee"].ToString());
                row.Cells.Add(table.Rows[i]["ProcCode"].ToString());
                row.Cells.Add(table.Rows[i]["user"].ToString());
                row.Cells.Add(table.Rows[i]["signature"].ToString());
                if (checkNotes.Checked)
                {
                    row.Note = table.Rows[i]["note"].ToString();
                }
                row.ColorText = Color.FromArgb(PIn.Int(table.Rows[i]["colorText"].ToString()));
                row.ColorBackG = Color.FromArgb(PIn.Int(table.Rows[i]["colorBackG"].ToString()));
                row.Tag = table.Rows[i];
                gridProg.Rows.Add(row);

            }
            gridProg.EndUpdate();
            gridProg.ScrollToEnd();
        }

        private void gridProg_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            //Chartscrollval = gridProg.ScrollValue;
            DataRow row = (DataRow)gridProg.Rows[e.Row].Tag;
            if (row["ProcNum"].ToString() != "0")
            {
                if (checkAudit.Checked)
                {
                    MsgBox.Show(this, "Not allowed to edit procedures when in audit mode.");
                    return;
                }
                Procedure proc = Procedures.GetOneProc(PIn.Long(row["ProcNum"].ToString()), true);
                FormProcEdit FormP = new FormProcEdit(proc, PatCur, FamCur);
                FormP.ShowDialog();
                if (FormP.DialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            else if (row["CommlogNum"].ToString() != "0")
            {
                Commlog comm = Commlogs.GetOne(PIn.Long(row["CommlogNum"].ToString()));
                if (comm == null)
                {
                    MsgBox.Show(this, "This commlog has been deleted by another user.");
                }
                else
                {
                    FormCommItem FormCI = new FormCommItem(comm);
                    if (FormCI.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                }
            }
            else if (row["RxNum"].ToString() != "0")
            {
                RxPat rx = RxPats.GetRx(PIn.Long(row["RxNum"].ToString()));
                if (rx == null)
                {
                    MsgBox.Show(this, "This prescription has been deleted by another user.");
                }
                else
                {
                    FormRxEdit FormRxE = new FormRxEdit(PatCur, rx);
                    FormRxE.ShowDialog();
                    if (FormRxE.DialogResult != DialogResult.OK)
                    {
                        return;
                    }
                }
            }
            else if (row["LabCaseNum"].ToString() != "0")
            {
                LabCase lab = LabCases.GetOne(PIn.Long(row["LabCaseNum"].ToString()));
                FormLabCaseEdit FormL = new FormLabCaseEdit();
                FormL.CaseCur = lab;
                FormL.ShowDialog();
            }
            else if (row["TaskNum"].ToString() != "0")
            {
                Task task = Tasks.GetOne(PIn.Long(row["TaskNum"].ToString()));
                FormTaskEdit FormT = new FormTaskEdit(task);
                FormT.Closing += new CancelEventHandler(TaskGoToEvent);
                FormT.Show();//non-modal
            }
            else if (row["AptNum"].ToString() != "0")
            {
                //Appointment apt=Appointments.GetOneApt(
                FormApptEdit FormA = new FormApptEdit(PIn.Long(row["AptNum"].ToString()));
                //PinIsVisible=false
                FormA.ShowDialog();
                if (FormA.DialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            else if (row["EmailMessageNum"].ToString() != "0")
            {
                EmailMessage msg = EmailMessage.GetById(PIn.Long(row["EmailMessageNum"].ToString()));
                FormEmailMessageEdit FormE = new FormEmailMessageEdit(msg);
                FormE.ShowDialog();
                if (FormE.DialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            ModuleSelected(PatCur.PatNum);
        }

        public void TaskGoToEvent(object sender, CancelEventArgs e)
        {
            FormTaskEdit FormT = (FormTaskEdit)sender;
            TaskObjectType GotoType = FormT.GotoType;
            long keyNum = FormT.GotoKeyNum;
            if (GotoType == TaskObjectType.None)
            {
                return;
            }
            if (GotoType == TaskObjectType.Patient)
            {
                if (keyNum != 0)
                {
                    Patient pat = Patients.GetPat(keyNum);
                    FormOpenDental.S_Contr_PatientSelected(pat, false);
                    ModuleSelected(pat.PatNum);
                    return;
                }
            }
            if (GotoType == TaskObjectType.Appointment)
            {
                //There's nothing to do here, since we're not in the appt module.
                return;
            }
        }

        private void checkShowTP_Click(object sender, EventArgs e)
        {
            FillProgNotes();
        }

        private void checkShowC_Click(object sender, EventArgs e)
        {
            FillProgNotes();

        }

        private void checkShowE_Click(object sender, EventArgs e)
        {
            FillProgNotes();

        }

        private void checkShowR_Click(object sender, EventArgs e)
        {
            FillProgNotes();

        }

        private void checkAppt_Click(object sender, EventArgs e)
        {
            FillProgNotes();

        }

        private void checkComm_Click(object sender, EventArgs e)
        {
            FillProgNotes();

        }

        private void checkLabCase_Click(object sender, EventArgs e)
        {
            FillProgNotes();

        }

        private void checkRx_Click(object sender, EventArgs e)
        {
            if (checkRx.Checked)//since there is no double click event...this allows almost the same thing
            {
                checkShowTP.Checked = false;
                checkShowC.Checked = false;
                checkShowE.Checked = false;
                checkShowR.Checked = false;
                checkNotes.Checked = true;
                checkRx.Checked = true;
                checkComm.Checked = false;
                checkAppt.Checked = false;
                checkLabCase.Checked = false;
                checkExtraNotes.Checked = false;

            }

            FillProgNotes();

        }

        private void checkExtraNotes_Click(object sender, EventArgs e)
        {
            FillProgNotes();

        }

        private void checkNotes_Click(object sender, EventArgs e)
        {
            FillProgNotes();

        }

        private void butShowNone_Click(object sender, EventArgs e)
        {
            checkShowTP.Checked = false;
            checkShowC.Checked = false;
            checkShowE.Checked = false;
            checkShowR.Checked = false;
            checkAppt.Checked = false;
            checkComm.Checked = false;
            checkLabCase.Checked = false;
            checkRx.Checked = false;
            checkShowTeeth.Checked = false;

            FillProgNotes();

        }

        private void butShowAll_Click(object sender, EventArgs e)
        {
            checkShowTP.Checked = true;
            checkShowC.Checked = true;
            checkShowE.Checked = true;
            checkShowR.Checked = true;
            checkAppt.Checked = true;
            checkComm.Checked = true;
            checkLabCase.Checked = true;
            checkRx.Checked = true;
            checkShowTeeth.Checked = false;
            FillProgNotes();

        }

        private void gridProg_MouseUp(object sender, MouseEventArgs e)
        {

        }
        #endregion ProgressNotes

        private void checkShowFamilyComm_Click(object sender, EventArgs e)
        {
            FillComm();
        }
        private void checkShowCompletePayPlans_Click(object sender, EventArgs e)
        {
            Preference.Update(PreferenceName.AccountShowCompletedPaymentPlans, checkShowCompletePayPlans.Checked);
            FillPaymentPlans();
            RefreshModuleScreen(false); //so the grids get redrawn if the payment plans grid hides/shows itself.
        }

        private void labelInsRem_MouseEnter(object sender, EventArgs e)
        {
            groupBoxFamilyIns.Visible = true;
            groupBoxIndIns.Visible = true;
        }

        private void labelInsRem_MouseLeave(object sender, EventArgs e)
        {
            groupBoxFamilyIns.Visible = false;
            groupBoxIndIns.Visible = false;
        }

        private void labelInsRem_Click(object sender, EventArgs e)
        {
            if (!CultureInfo.CurrentCulture.Name.EndsWith("CA"))
            {//Canadian. en-CA or fr-CA
             //Since the bonus information in FormInsRemain is currently only helpful in Canada,
             //we have decided not to show the form for other countries at this time.
                return;
            }
            if (PatCur == null)
            {
                return;
            }
            FormInsRemain FormIR = new FormInsRemain(PatCur.PatNum);
            FormIR.ShowDialog();
        }

        private void menuPrepayment_Click(object sender, EventArgs e)
        {
            if (PatCur == null)
            {
                return;
            }
            FormPrepaymentTool FormPT = new FormPrepaymentTool(PatCur);
            if (FormPT.ShowDialog() == DialogResult.OK)
            {
                Family famCur = Patients.GetFamily(PatCur.PatNum);
                FormPayment FormP = new FormPayment(PatCur, famCur, FormPT.ReturnPayment, false);
                FormP.IsNew = true;
                Payments.Insert(FormPT.ReturnPayment);
                RefreshModuleData(PatCur.PatNum, false);
                RefreshModuleScreen(false);
                FormP.ShowDialog();
            }
            RefreshModuleData(PatCur.PatNum, false);
            RefreshModuleScreen(false);
        }

        ///<summary>Hides the 'Add Adjustment' context menu if anything other than a procedure is selected.</summary>
        private void contextMenuAcctGrid_Popup(object sender, EventArgs e)
        {
            DataTable table = DataSetMain.Tables["account"];
            List<int> listSelectedRows = gridAccount.SelectedIndices.ToList();
            foreach (int row in listSelectedRows)
            {
                if (table.Rows[row]["ProcNum"].ToString() == "0")
                {
                    menuItemAddAdj.Enabled = false;
                    return;
                }
            }
            //If all selected rows are adjustments enable the 'Add Adjustment' button.
            menuItemAddAdj.Enabled = true;
        }
    }
}