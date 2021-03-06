using CodeBase;
using MigraDoc.DocumentObjectModel;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.HL7;
using OpenDentBusiness.UI;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OpenDental
{
    ///<summary>
    /// AptCur.AptNum can not be trusted fully inside of this form. This form can create new appointments without inserting them into the DB.
    /// Due to this make sure you consider new appointments and handle accordingly. See _isInsertRequired.
    /// Edit window for appointments. Will have a DialogResult of Cancel if the appointment was marked as new and is deleted.
    /// </summary>
    public partial class FormApptEdit : FormBase
    {
        // TODO: The time bar should use the colors assigned to provider/hygienist.

        public bool PinIsVisible;
        public bool PinClicked;
        public bool IsNew;
        private Appointment AptCur;
        private Appointment AptOld;
        ///<summary>The string time pattern in the current increment. Not in the 5 minute increment.</summary>
        //private StringBuilder strBTime;
        private List<InsPlan> PlanList;
        private List<InsSub> SubList;
        private Patient pat;
        private Family fam;
        ///<summary>This is the way to pass a "signal" up to the parent form that OD is to close.</summary>
        public bool CloseOD;
        ///<summary>True if appt was double clicked on from the chart module gridProg.  Currently only used to trigger an appointment overlap check.</summary>
        public bool IsInChartModule;
        ///<summary>True if appt was double clicked on from the ApptsOther form.  Currently only used to trigger an appointment overlap check.</summary>
        public bool IsInViewPatAppts;
        ///<summary>Matches list of appointments in comboAppointmentType. Does not include hidden types unless current appointment is of that type.</summary>
        private List<AppointmentType> _listAppointmentType;
        ///<summary>Procedure were attached/detached from appt and the user clicked cancel or closed the form.
        ///Used in ApptModule to tell if we need to refresh.</summary>
        public bool HasProcsChangedAndCancel;
        ///<summary>Lab for the current appointment.  It may be null if there is no lab.</summary>
        private LabCase _labCur;
        ///<summary>A list of all appointments that are associated to any procedures in the Procedures on this Appointment grid.</summary>
        private List<Appointment> _listAppointments;
        ///<summary>Stale deep copy of _listAppointments to use with sync.</summary>
        private List<Appointment> _listAppointmentsOld;
        private bool _isPlanned;
        private DataTable _tableFields;
        private DataTable _tableComms;
        ///<summary>Local copy of the provider cache for convenience due to how frequently provider cache is accessed.</summary>
        private List<Provider> _listProvidersAll;
        ///<summary>Cached list of clinics available to user. Also includes a dummy Clinic at index 0 for "none".</summary>
        private List<Clinic> _listClinics;
        ///<summary>Filtered list of providers based on which clinic is selected. If no clinic is selected displays all providers. Does not include a "none" option.</summary>
        private List<Provider> _listProvs;
        ///<summary>Filtered list of providers based on which clinic is selected. If no clinic is selected displays all providers. Also includes a dummy provider at index 0 for "none"</summary>
        private List<Provider> _listProvHygs;
        ///<summary>Used to keep track of the current clinic selected. This is because it may be a clinic that is, rarely, not in _listClinics.</summary>
        private long _selectedClinicNum;
        ///<summary>Instead of relying on _listProviders[comboProv.SelectedIndex] to determine the selected Provider we use this variable to store it explicitly.</summary>
        private long _selectedProvNum;
        ///<summary>Instead of relying on _listProviders[comboProvHyg.SelectedIndex] to determine the selected Provider we use this variable to store it explicitly.</summary>
        private long _selectedProvHygNum;
        ///<summary>All ProcNums attached to the appt when form opened.</summary>
        private List<long> _listProcNumsAttachedStart = new List<long>();
        ///<summary>All ProcNums intended to be selected on load, but without altering any procedure properties.</summary>
        private List<long> _listPreSelectedProcNums;
        ///<summary>Used when first loading the form to skip calling fill methods multiple times.</summary>
        private bool _isOnLoad;
        ///<summary>List of all procedures that show within the Procedures on this Appointment grid.  Filled on load.
        ///Used to double check that we update other appointments that we could steal procedures from (e.g. planned appts with tp procs).</summary>
        private List<Procedure> _listProcsForAppt;
        ///<summary>The selected appointment type when this form loads.</summary>
        private AppointmentType _selectedAptType;
        ///<summary>The exact index of the selected item in comboApptType.</summary>
        private int _aptTypeIndex;
        private List<PatPlan> _listPatPlans;
        List<Benefit> _benefitList;
        private bool _isDeleted;
        private bool _isClickLocked;
        private int indexStatusBroken;
        ///<summary>Used when FormApptBreak is required to track what the user has selected.</summary>
        private ApptBreakSelection _formApptBreakSelection = ApptBreakSelection.None;
        private ProcedureCode _procCodeBroken = null;
        private List<Employee> _listEmployees;
        ///<summary>eCW Tight or Full enabled and a DFT msg for this appt has already been sent.  The 'Finish &amp; Send' button will say 'Revise'</summary>
        private bool _isEcwHL7Sent = false;


        /// <summary>
        /// If no aptNum was passed into this form, this boolean will be set to true to indicate that AptCur.AptNum 
        /// cannot be trusted until after the insert occurs. Someday we should consider using the IsNew flag instead
        /// after we remove all of the appointment pre-insert logic.
        /// </summary>
        bool insertRequired = false;


        private List<Definition> _listRecallUnschedStatusDefs;
        private List<Definition> _listApptConfirmedDefs;
        private List<Definition> _listApptProcsQuickAddDefs;
        ///<summary>A list of all ClaimProcs that are related to the patient's current procedures</summary>
        private List<ClaimProc> _listClaimProcs;
        ///<summary>A list of all Adjustments that are related to the patient's current procedures</summary>
        private List<Adjustment> _listAdjustments;
        ///<summary>The data necesary to load the form.</summary>
        private ApptEdit.LoadData _loadData;
        ///<summary>Indicates this appointment has been opened from the Unscheduled list.</summary>
        private bool _isSchedulingUnscheduledAppt;

        #region Properties

        ///<summary>The currently selected ApptStatus.</summary>
        private ApptStatus _selectedApptStatus
        {
            get
            {
                if (AptCur.AptStatus == ApptStatus.Planned)
                {
                    return AptCur.AptStatus;
                }
                else if (statusComboBox.SelectedIndex == -1)
                {
                    return ApptStatus.Scheduled;
                }
                else if (AptCur.AptStatus == ApptStatus.PtNote || AptCur.AptStatus == ApptStatus.PtNoteCompleted)
                {
                    return (ApptStatus)statusComboBox.SelectedIndex + 7;
                }
                else if (statusComboBox.SelectedIndex == 3)
                {//Broken
                    return ApptStatus.Broken;
                }
                else
                {//Scheduled, Complete, Unscheduled
                    return (ApptStatus)statusComboBox.SelectedIndex + 1;
                }
            }
        }

        ///<summary>Indicates the Appointment is being opened from the unscheduled list.</summary>
        public bool IsSchedulingUnscheduledAppt
        {
            get
            {
                return _isSchedulingUnscheduledAppt;
            }
            set
            {
                _isSchedulingUnscheduledAppt = value;
            }
        }

        #endregion Properties

        /// <summary>
        /// When aptNum is 0, make sure to set a valid patNum because a new appointment will be created/inserted on OK click.
        /// Set useApptDrawingSettings to true if the user double clicked on the appointment schedule in order to make a new appointment.
        /// listPreSelectedProcNums is used to preselect procs in the grid without pre-altering the procs properties, such as AptNum/PlannedAptNum
        /// </summary>
        public FormApptEdit(long aptNum, long patNum = 0, bool useApptDrawingSettings = false, Patient patient = null, List<long> listPreSelectedProcNums = null)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            _isClickLocked = true;

            if (aptNum == 0)
            {//Creating a new appointment
                insertRequired = true;
                Patient pat = patient ?? Patients.GetPat(patNum);
                if (pat == null)
                {
                    MessageBox.Show(
                        "Invalid patient passed in. Please call support or try again.", 
                        "", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);

                    DialogResult = DialogResult.Cancel;
                    if (!Modal)
                    {
                        Close();
                    }
                    return;
                }
                AptCur = AppointmentL.MakeNewAppointment(pat, useApptDrawingSettings);
            }
            else
            {
                AptCur = Appointments.GetOneApt(aptNum);//We need this query to get the PatNum for the appointment.
            }
            _listPreSelectedProcNums = listPreSelectedProcNums;
        }

        public Appointment GetAppointmentCur()
        {
            return AptCur.Copy();
        }

        public Appointment GetAppointmentOld()
        {
            return AptOld.Copy();
        }

        private void FormApptEdit_Load(object sender, EventArgs e)
        {
            // Check if the appointment exists, appointment could have been deleted by another workstation.
            if (AptCur == null) 
            {
                MessageBox.Show(
                    "Appointment no longer exists.", 
                    "Appointment", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                DialogResult = DialogResult.Cancel;
                if (!Modal)
                {
                    Close();
                }
                return;
            }


            _selectedAptType = null;
            _aptTypeIndex = 0;
            if (Preference.GetBool(PreferenceName.AppointmentTypeShowPrompt) && IsNew
                && !AptCur.AptStatus.In(ApptStatus.PtNote, ApptStatus.PtNoteCompleted))
            {
                FormApptTypes FormAT = new FormApptTypes();
                FormAT.IsSelectionMode = true;
                FormAT.IsNoneAllowed = true;
                FormAT.ShowDialog();
                if (FormAT.DialogResult == DialogResult.OK)
                {
                    _selectedAptType = FormAT.SelectedAptType;
                }
            }
            _isOnLoad = true;
            new ODThread((o) =>
            {
                //Sleep for the delay and then set the variable to false.
                Thread.Sleep(Math.Max((int)(TimeSpan.FromSeconds(Preference.GetDouble(PreferenceName.FormClickDelay)).TotalMilliseconds), 1));
                _isClickLocked = false;
            }).Start();

            _loadData = ApptEdit.GetLoadData(AptCur, IsNew);
            _listProcsForAppt = _loadData.ListProcsForAppt;
            _listAppointments = _loadData.ListAppointments;
            if (_listAppointments.Find(x => x.AptNum == AptCur.AptNum) == null)
            {
                _listAppointments.Add(AptCur);//Add AptCur if there are no procs attached to it.
            }
            _listAppointmentsOld = _listAppointments.Select(x => x.Copy()).ToList();
            for (int i = 0; i < _listAppointments.Count; i++)
            {
                if (_listAppointments[i].AptNum == AptCur.AptNum)
                {
                    AptCur = _listAppointments[i];//Changing the variable pointer so all changes are done on the element in the list.
                }
            }
            AptOld = AptCur.Copy();
            if (IsNew)
            {
                if (!Security.IsAuthorized(Permissions.AppointmentCreate))
                { //Should have been checked before appointment was inserted into DB and this form was loaded.  Left here just in case.
                    DialogResult = DialogResult.Cancel;
                    if (!this.Modal)
                    {
                        Close();
                    }
                    return;
                }
            }
            else
            {
                //The order of the conditional matters; C# will not evaluate the second part of the conditional if it is not needed. 
                //Changing the order will cause unneeded Security MsgBoxes to pop up.
                if (AptCur.AptStatus != ApptStatus.Complete && !Security.IsAuthorized(Permissions.AppointmentEdit)
                    || (AptCur.AptStatus == ApptStatus.Complete && !Security.IsAuthorized(Permissions.AppointmentCompleteEdit)))
                {//completed apts have their own perm.
                    butOK.Enabled = false;
                    butDelete.Enabled = false;
                    butPin.Enabled = false;
                    butTask.Enabled = false;
                    gridProc.Enabled = false;
                    quickAddListBox.Enabled = false;
                    procedureAddButton.Enabled = false;
                    procedureDeleteButton.Enabled = false;
                    butInsPlan1.Enabled = false;
                    butInsPlan2.Enabled = false;
                    butComplete.Enabled = false;
                }
            }
            if (!Security.IsAuthorized(Permissions.ApptConfirmStatusEdit, true))
            {//Suppress message because it would be very annoying to users.
                comboConfirmed.Enabled = false;
            }
            else if (IsSchedulingUnscheduledAppt)
            {//User is authorized for Permissions.ApptConfirmStatusEdit.
             //Causes the confirmation status to be reset in the UI, mimics ContrAppt.pinBoard_MouseUp(...)
                AptCur.Confirmed = Defs.GetFirstForCategory(DefinitionCategory.ApptConfirmed, true).Id;
            }
            //The objects below are needed when adding procs to this appt.
            fam = _loadData.Family;
            pat = fam.GetPatient(AptCur.PatNum);
            _listPatPlans = _loadData.ListPatPlans;
            _benefitList = _loadData.ListBenefits;
            SubList = _loadData.ListInsSubs;
            PlanList = _loadData.ListInsPlans;
            if (!PatPlans.IsPatPlanListValid(_listPatPlans, listInsSubs: SubList))
            {
                _listPatPlans = PatPlans.Refresh(AptCur.PatNum);
                SubList = InsSubs.RefreshForFam(fam);
                PlanList = InsPlans.RefreshForSubList(SubList);
            }
            _tableFields = _loadData.TableApptFields;
            _tableComms = _loadData.TableComms;
            _listAdjustments = _loadData.ListAdjustments;
            _listClaimProcs = _loadData.ListClaimProcs;
            _listProvidersAll = Provider.All().ToList();
            _isPlanned = false;
            if (AptCur.AptStatus == ApptStatus.Planned)
            {
                _isPlanned = true;
            }
            _labCur = _loadData.Lab;
            if (Preference.GetBool(PreferenceName.EasyHideDentalSchools))
            {
                butRequirement.Visible = false;
                textRequirement.Visible = false;
            }
            if (Preference.GetBool(PreferenceName.ShowFeatureEhr))
            {
                butSyndromicObservations.Visible = true;
                labelSyndromicObservations.Visible = true;
            }
            if (!PinIsVisible)
            {
                butPin.Visible = false;
            }
            string titleText = this.Text;
            if (_isPlanned)
            {
                titleText = Lan.g(this, "Edit Planned Appointment") + " - " + pat.GetNameFL();
                statusLabel.Visible = false;
                statusComboBox.Visible = false;
                butDelete.Visible = false;
                if (_listAppointments.FindAll(x => x.NextAptNum == AptCur.AptNum)//This planned appt is attached to a completed appt.
                    .Exists(x => x.AptStatus == ApptStatus.Complete))
                {
                    labelPlannedComplete.Visible = true;
                }
            }
            else if (AptCur.AptStatus == ApptStatus.PtNote)
            {
                appointmentNoteLabel.Text = "Patient NOTE:";
                titleText = Lan.g(this, "Edit Patient Note") + " - " + pat.GetNameFL() + " on " + AptCur.AptDateTime.DayOfWeek + ", " + AptCur.AptDateTime;
                statusComboBox.Items.Add(Lan.g("enumApptStatus", "Patient Note"));
                statusComboBox.Items.Add(Lan.g("enumApptStatus", "Completed Pt. Note"));
                statusComboBox.SelectedIndex = (int)AptCur.AptStatus - 7;
                quickAddLabel.Visible = false;
                statusLabel.Visible = false;
                gridProc.Visible = false;
                quickAddListBox.Visible = false;
                procedureAddButton.Visible = false;
                procedureDeleteButton.Visible = false;
                procedureAttachAllButton.Visible = false;
                //textNote.Width = 400;
            }
            else if (AptCur.AptStatus == ApptStatus.PtNoteCompleted)
            {
                appointmentNoteLabel.Text = "Completed Patient NOTE:";
                titleText = Lan.g(this, "Edit Completed Patient Note") + " - " + pat.GetNameFL() + " on " + AptCur.AptDateTime.DayOfWeek + ", " + AptCur.AptDateTime;
                statusComboBox.Items.Add(Lan.g("enumApptStatus", "Patient Note"));
                statusComboBox.Items.Add(Lan.g("enumApptStatus", "Completed Pt. Note"));
                statusComboBox.SelectedIndex = (int)AptCur.AptStatus - 7;
                quickAddLabel.Visible = false;
                statusLabel.Visible = false;
                gridProc.Visible = false;
                quickAddListBox.Visible = false;
                procedureAddButton.Visible = false;
                procedureDeleteButton.Visible = false;
                procedureAttachAllButton.Visible = false;
                //textNote.Width = 400;
            }
            else
            {
                titleText = Lan.g(this, "Edit Appointment") + " - " + pat.GetNameFL() + " on " + AptCur.AptDateTime.DayOfWeek + ", " + AptCur.AptDateTime;
                statusComboBox.Items.Add(Lan.g("enumApptStatus", "Scheduled"));
                statusComboBox.Items.Add(Lan.g("enumApptStatus", "Complete"));
                statusComboBox.Items.Add(Lan.g("enumApptStatus", "UnschedList"));
                indexStatusBroken = statusComboBox.Items.Add(Lan.g("enumApptStatus", "Broken"));
                if (AptCur.AptStatus == ApptStatus.Broken)
                {
                    statusComboBox.SelectedIndex = indexStatusBroken;
                }
                else
                {
                    statusComboBox.SelectedIndex = (int)AptCur.AptStatus - 1;
                }
            }
            if (AptCur.Op != 0)
            {
                titleText += " | " + Operatory.GetById(AptCur.Op).Abbr;
            }
            this.Text = titleText;
            checkASAP.Checked = AptCur.Priority == ApptPriority.ASAP;
            if (AptCur.AptStatus == ApptStatus.UnschedList)
            {
                if (HL7Defs.GetOneDeepEnabled() != null && !HL7Defs.GetOneDeepEnabled().ShowAppts)
                {
                    statusComboBox.Enabled = true;
                }
                else
                {
                    statusComboBox.Enabled = false;
                }
            }
            comboUnschedStatus.Items.Add(Lan.g(this, "none"));
            comboUnschedStatus.SelectedIndex = 0;
            _listRecallUnschedStatusDefs = Definition.GetByCategory(DefinitionCategory.RecallUnschedStatus);;
            _listApptConfirmedDefs = Definition.GetByCategory(DefinitionCategory.ApptConfirmed);;
            _listApptProcsQuickAddDefs = Definition.GetByCategory(DefinitionCategory.ApptProcsQuickAdd);;
            for (int i = 0; i < _listRecallUnschedStatusDefs.Count; i++)
            {
                comboUnschedStatus.Items.Add(_listRecallUnschedStatusDefs[i].Description);
                if (_listRecallUnschedStatusDefs[i].Id == AptCur.UnschedStatus)
                    comboUnschedStatus.SelectedIndex = i + 1;
            }
            for (int i = 0; i < _listApptConfirmedDefs.Count; i++)
            {
                comboConfirmed.Items.Add(_listApptConfirmedDefs[i].Description);
                if (_listApptConfirmedDefs[i].Id == AptCur.Confirmed)
                {
                    comboConfirmed.SelectedIndex = i;
                }
            }
            checkTimeLocked.Checked = AptCur.TimeLocked;
            appointmentNoteTextBox.Text = AptCur.Note;
            for (int i = 0; i < _listApptProcsQuickAddDefs.Count; i++)
            {
                quickAddListBox.Items.Add(_listApptProcsQuickAddDefs[i].Description);
            }
            //Fill Clinics
            _listClinics = new List<Clinic>() { new Clinic() { Abbr = Lan.g(this, "None") } }; //Seed with "None"
            Clinic.GetByUser(Security.CurrentUser).ForEach(x => _listClinics.Add(x));//do not re-organize from cache. They could either be alphabetizeded or sorted by item order.
            _listClinics.ForEach(x => comboClinic.Items.Add(x.Abbr));
            //Set Selected Nums
            _selectedClinicNum = AptCur.ClinicNum;
            //if (IsNew)
            //{
            //    //Try to auto-select a provider when in Orion mode. Only for new appointments so we don't change historical data.
            //    AptCur.ProvNum = Providers.GetOrionProvNum(AptCur.ProvNum);
            //}
            _selectedProvNum = AptCur.ProvNum;
            _selectedProvHygNum = AptCur.ProvHyg;
            //Set combo indexes for first pass through fillComboProvHyg
            comboProv.SelectedIndex = -1;//initializes to 0. Must be -1 for fillComboProvHyg.
            comboProvHyg.SelectedIndex = -1;//initializes to 0. Must be -1 for fillComboProvHyg.
            comboClinic.IndexSelectOrSetText(_listClinics.FindIndex(x => x.Id == _selectedClinicNum), () => { return Clinic.GetById(_selectedClinicNum).Abbr; });
            fillComboProvHyg();
            checkIsHygiene.Checked = AptCur.IsHygiene;
            //Fill comboAssistant with employees and none option
            comboAssistant.Items.Add(Lan.g(this, "none"));
            comboAssistant.SelectedIndex = 0;
            _listEmployees = Employee.All();
            for (int i = 0; i < _listEmployees.Count; i++)
            {
                comboAssistant.Items.Add(_listEmployees[i].FirstName);
                if (_listEmployees[i].Id == AptCur.Assistant)
                    comboAssistant.SelectedIndex = i + 1;
            }
            textLabCase.Text = GetLabCaseDescript();
            textTimeArrived.ContextMenu = contextMenuTimeArrived;
            textTimeSeated.ContextMenu = contextMenuTimeSeated;
            textTimeDismissed.ContextMenu = contextMenuTimeDismissed;
            if (AptCur.DateTimeAskedToArrive.TimeOfDay > TimeSpan.FromHours(0))
            {
                textTimeAskedToArrive.Text = AptCur.DateTimeAskedToArrive.ToShortTimeString();
            }
            if (AptCur.DateTimeArrived.TimeOfDay > TimeSpan.FromHours(0))
            {
                textTimeArrived.Text = AptCur.DateTimeArrived.ToShortTimeString();
            }
            if (AptCur.DateTimeSeated.TimeOfDay > TimeSpan.FromHours(0))
            {
                textTimeSeated.Text = AptCur.DateTimeSeated.ToShortTimeString();
            }
            if (AptCur.DateTimeDismissed.TimeOfDay > TimeSpan.FromHours(0))
            {
                textTimeDismissed.Text = AptCur.DateTimeDismissed.ToShortTimeString();
            }
            if (AptCur.AptStatus == ApptStatus.Complete
                || AptCur.AptStatus == ApptStatus.Broken
                || AptCur.AptStatus == ApptStatus.PtNote
                || AptCur.AptStatus == ApptStatus.PtNoteCompleted)
            {
                textInsPlan1.Text = InsPlans.GetCarrierName(AptCur.InsPlan1, PlanList);
                textInsPlan2.Text = InsPlans.GetCarrierName(AptCur.InsPlan2, PlanList);
            }
            else
            {//Get the current ins plans for the patient.
                butInsPlan1.Enabled = false;
                butInsPlan2.Enabled = false;
                InsSub sub1 = InsSubs.GetSub(PatPlans.GetInsSubNum(_listPatPlans, PatPlans.GetOrdinal(PriSecMed.Primary, _listPatPlans, PlanList, SubList)), SubList);
                InsSub sub2 = InsSubs.GetSub(PatPlans.GetInsSubNum(_listPatPlans, PatPlans.GetOrdinal(PriSecMed.Secondary, _listPatPlans, PlanList, SubList)), SubList);
                AptCur.InsPlan1 = sub1.PlanNum;
                AptCur.InsPlan2 = sub2.PlanNum;
                textInsPlan1.Text = InsPlans.GetCarrierName(AptCur.InsPlan1, PlanList);
                textInsPlan2.Text = InsPlans.GetCarrierName(AptCur.InsPlan2, PlanList);
            }
            if (!Preference.GetBool(PreferenceName.EasyHideDentalSchools))
            {
                List<ReqStudent> listStudents = _loadData.ListStudents;
                string requirements = "";
                for (int i = 0; i < listStudents.Count; i++)
                {
                    if (i > 0)
                    {
                        requirements += "\r\n";
                    }
                    Provider student = _listProvidersAll.First(x => x.Id == listStudents[i].ProvNum);
                    requirements += student.LastName + ", " + student.FirstName + ": " + listStudents[i].Descript;
                }
                textRequirement.Text = requirements;
            }
            //IsNewPatient is set well before opening this form.
            checkIsNewPatient.Checked = AptCur.IsNewPatient;
            butColor.BackColor = AptCur.ColorOverride;

            //if (Programs.UsingEcwTightOrFullMode() && !insertRequired)
            //{
            //    //These buttons are ONLY for eCW, not any other HL7 interface.
            //    butComplete.Visible = true;
            //    butPDF.Visible = true;
            //    //for eCW, we need to hide some things--------------------
            //    if (Bridges.ECW.AptNum == AptCur.AptNum)
            //    {
            //        butDelete.Visible = false;
            //    }
            //    butPin.Visible = false;
            //    butTask.Visible = false;
            //    butAddComm.Visible = false;
            //    if (HL7Msgs.MessageWasSent(AptCur.AptNum))
            //    {
            //        _isEcwHL7Sent = true;
            //        butComplete.Text = "Revise";
            //        //if(!Security.IsAuthorized(Permissions.Setup,true)) {
            //        //	butComplete.Enabled=false;
            //        //	butPDF.Enabled=false;
            //        //}
            //        butOK.Enabled = false;
            //        gridProc.Enabled = false;
            //        quickAddListBox.Enabled = false;
            //        procedureAddButton.Enabled = false;
            //        procedureDeleteButton.Enabled = false;
            //    }
            //    else
            //    {//hl7 was not sent for this appt
            //        _isEcwHL7Sent = false;
            //        butComplete.Text = "Finish && Send";
            //        if (Bridges.ECW.AptNum != AptCur.AptNum)
            //        {
            //            butComplete.Enabled = false;
            //        }
            //        butPDF.Enabled = false;
            //    }
            //}
            //else
            //{
                butComplete.Visible = false;
                butPDF.Visible = false;
            //}
            //Hide text message button sometimes
            //if (pat.WirelessPhone == "" || (!Program.IsEnabled(ProgramName.CallFire) && !SmsPhones.IsIntegratedTextingEnabled()))
            //{
            //    butText.Enabled = false;
            //}
            //else
            //{//Pat has a wireless phone number and CallFire is enabled
                butText.Enabled = true;//TxtMsgOk checking performed on button click.
            //}
            //AppointmentType
            _listAppointmentType = AppointmentType.All().Where(x => !x.Hidden || x.Id == AptCur.AppointmentTypeNum).ToList();
            comboApptType.Items.Add(Lan.g(this, "None"));
            comboApptType.SelectedIndex = 0;
            foreach (AppointmentType aptType in _listAppointmentType)
            {
                comboApptType.Items.Add(aptType.Name);
            }
            int selectedIndex = -1;
            if (IsNew && _selectedAptType != null)
            { //selectedAptType will be null if they didn't select anything.
                selectedIndex = _listAppointmentType.FindIndex(x => x.Id == _selectedAptType.Id);
            }
            else
            {
                selectedIndex = _listAppointmentType.FindIndex(x => x.Id == AptCur.AppointmentTypeNum);
            }
            comboApptType.SelectedIndex = selectedIndex + 1;//+1 for none
            _aptTypeIndex = comboApptType.SelectedIndex;
            HasProcsChangedAndCancel = false;
            FillProcedures();
            if (IsNew && comboApptType.SelectedIndex != 0)
            {
                AptTypeHelper();
            }
            //if this is a new appointment with no procedures attached, set the time pattern using the default preference
            else if (IsNew && gridProc.SelectedIndices.Length < 1)
            {
                AptCur.Pattern = Appointments.GetApptTimePatternForNoProcs();
            }
            //convert time pattern from 5 to current increment.

            timeBar.Pattern = AptCur.Pattern;

            FillPatient();//Must be after FillProcedures(), so that the initial amount for the appointment can be calculated.
            FillComm();
            FillFields();
            appointmentNoteTextBox.Focus();
            appointmentNoteTextBox.SelectionStart = 0;
            _isOnLoad = false;

            Plugin.Trigger(this, "FormApptEdit_Loaded", AptCur, pat);
        }

        private void comboClinic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboClinic.SelectedIndex > -1)
            {
                _selectedClinicNum = _listClinics[comboClinic.SelectedIndex].Id;
            }
            if (!_isOnLoad)
            {//If not called when loading form
                fillComboProvHyg();
                FillProcedures();
            }
        }

        private void comboProv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboProv.SelectedIndex > -1)
            {
                _selectedProvNum = _listProvs[comboProv.SelectedIndex].Id;
            }
        }

        private void comboProvHyg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboProvHyg.SelectedIndex > -1)
            {
                _selectedProvHygNum = _listProvHygs[comboProvHyg.SelectedIndex].Id;
            }
        }

        private void butPickDentist_Click(object sender, EventArgs e)
        {
            FormProviderPick formp = new FormProviderPick(_listProvs);
            formp.SelectedProvNum = _selectedProvNum;
            formp.ShowDialog();
            if (formp.DialogResult != DialogResult.OK)
            {
                return;
            }
            _selectedProvNum = formp.SelectedProvNum;
            comboProv.IndexSelectOrSetText(_listProvs.FindIndex(x => x.Id == _selectedProvNum), () => { return Providers.GetAbbr(_selectedProvNum); });
        }

        private void butPickHyg_Click(object sender, EventArgs e)
        {
            FormProviderPick formp = new FormProviderPick(_listProvHygs);//add none option to select providers.
            formp.SelectedProvNum = _selectedProvHygNum;
            formp.ShowDialog();
            if (formp.DialogResult != DialogResult.OK)
            {
                return;
            }
            _selectedProvHygNum = formp.SelectedProvNum;
            comboProvHyg.IndexSelectOrSetText(_listProvHygs.FindIndex(x => x.Id == _selectedProvHygNum), () => { return Providers.GetAbbr(_selectedProvHygNum); });
        }

        ///<summary>Fills combo provider based on which clinic is selected and attempts to preserve provider selection if any.</summary>
        private void fillComboProvHyg()
        {
            if (comboProv.SelectedIndex > -1)
            {//valid prov selected, non none or nothing.
                _selectedProvNum = _listProvs[comboProv.SelectedIndex].Id;
            }
            if (comboProvHyg.SelectedIndex > -1)
            {
                _selectedProvHygNum = _listProvHygs[comboProvHyg.SelectedIndex].Id;
            }
            _listProvs = Providers.GetProvsForClinic(_selectedClinicNum).ToList();
            _listProvHygs = Providers.GetProvsForClinic(_selectedClinicNum);
            _listProvHygs.Add(new Provider() { Abbr = "none" });
            _listProvHygs = _listProvHygs.OrderBy(x => x.Id > 0).ToList();
            //Fill comboProv
            comboProv.Items.Clear();
            _listProvs.ForEach(x => comboProv.Items.Add(x.Abbr));
            comboProv.IndexSelectOrSetText(_listProvs.FindIndex(x => x.Id == _selectedProvNum), () => { return Providers.GetAbbr(_selectedProvNum); });
            //Fill comboProvHyg
            comboProvHyg.Items.Clear();
            _listProvHygs.ForEach(x => comboProvHyg.Items.Add(x.Abbr));
            comboProvHyg.IndexSelectOrSetText(_listProvHygs.FindIndex(x => x.Id == _selectedProvHygNum), () => { return Providers.GetAbbr(_selectedProvHygNum); });
        }

        private void butColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog1 = new ColorDialog();
            colorDialog1.Color = butColor.BackColor;
            colorDialog1.ShowDialog();
            butColor.BackColor = colorDialog1.Color;
        }

        private void butColorClear_Click(object sender, EventArgs e)
        {
            butColor.BackColor = System.Drawing.Color.FromArgb(0);
        }

        private void FillPatient()
        {
            DataTable table = _loadData.PatientTable;
            gridPatient.BeginUpdate();
            gridPatient.Columns.Clear();
            ODGridColumn col = new ODGridColumn("", 120);//Add 2 blank columns
            gridPatient.Columns.Add(col);
            col = new ODGridColumn("", 120);
            gridPatient.Columns.Add(col);
            gridPatient.Rows.Clear();
            ODGridRow row;
            for (int i = 1; i < table.Rows.Count; i++)
            {//starts with 1 to skip name
                row = new ODGridRow();
                row.Cells.Add(table.Rows[i]["field"].ToString());
                row.Cells.Add(table.Rows[i]["value"].ToString());
                if (table.Rows[i]["field"].ToString().EndsWith("Phone") && Program.IsEnabled<DentalTekBridge>())
                {
                    row.Cells[row.Cells.Count - 1].ColorText = System.Drawing.Color.Blue;
                    row.Cells[row.Cells.Count - 1].Underline = true;
                }
                gridPatient.Rows.Add(row);
            }
            //Add a UI managed row to display the total fee for the selected procedures in this appointment.
            row = new ODGridRow();
            row.Cells.Add(Lan.g(this, "Fee This Appt"));
            row.Cells.Add("");//Calculated below
            gridPatient.Rows.Add(row);
            CalcPatientFeeThisAppt();
            gridPatient.EndUpdate();
            gridPatient.ScrollToEnd();
        }

        ///<summary>Calculates the fee for this appointment using the highlighted procedures in the procedure list.</summary>
        private void CalcPatientFeeThisAppt()
        {
            double feeThisAppt = 0;
            for (int i = 0; i < gridProc.SelectedIndices.Length; i++)
            {
                feeThisAppt += ((Procedure)(gridProc.Rows[gridProc.SelectedIndices[i]].Tag)).ProcFeeTotal;
            }
            gridPatient.Rows[gridPatient.Rows.Count - 1].Cells[1].Text = POut.Double(feeThisAppt);
            gridPatient.Invalidate();
        }

        ///<summary>Fully refreshes the data and then calculate the estimated patient portion</summary>
        private void RefreshEstPatientPortion()
        {
            _listClaimProcs = ClaimProcs.RefreshForProcs(_listProcsForAppt.Select(x => x.ProcNum).ToList());
            _listAdjustments = Adjustments.GetForProcs(_listProcsForAppt.Select(x => x.ProcNum).ToList());
            CalcEstPatientPortion();
        }

        ///<summary>Calculates the estimated patient portion to insert into the grid</summary>
        private void CalcEstPatientPortion()
        {
            List<Procedure> listSelectedProcedures = gridProc.SelectedTags<Procedure>();
            decimal totalEstPatientPortion = 0;
            foreach (Procedure proc in listSelectedProcedures)
            {
                totalEstPatientPortion += ClaimProcs.GetPatPortion(proc, _listClaimProcs, _listAdjustments);
            }
            ODGridRow row = gridPatient.Rows.ToList().Find(x => x.Cells[0].Text == Lans.g("FormApptEdit", "Est. Patient Portion"));
            row.Cells[1].Text = totalEstPatientPortion.ToString("F");
        }

        private void FillFields()
        {
            gridFields.BeginUpdate();
            gridFields.Columns.Clear();
            ODGridColumn col = new ODGridColumn("", 100);
            gridFields.Columns.Add(col);
            col = new ODGridColumn("", 100);
            gridFields.Columns.Add(col);
            gridFields.Rows.Clear();
            ODGridRow row;
            for (int i = 0; i < _tableFields.Rows.Count; i++)
            {
                row = new ODGridRow();
                row.Cells.Add(_tableFields.Rows[i]["FieldName"].ToString());
                row.Cells.Add(_tableFields.Rows[i]["FieldValue"].ToString());
                gridFields.Rows.Add(row);
            }
            gridFields.EndUpdate();
        }

        private void FillComm()
        {
            gridComm.BeginUpdate();
            gridComm.Columns.Clear();
            ODGridColumn col = new ODGridColumn(Lan.g("TableCommLog", "DateTime"), 80);
            gridComm.Columns.Add(col);
            col = new ODGridColumn(Lan.g("TableCommLog", "Description"), 80);
            gridComm.Columns.Add(col);
            gridComm.Rows.Clear();
            ODGridRow row;
            List<Definition> listMiscColorDefs = Definition.GetByCategory(DefinitionCategory.MiscColors);;
            for (int i = 0; i < _tableComms.Rows.Count; i++)
            {
                row = new ODGridRow();
                if (PIn.Long(_tableComms.Rows[i]["CommlogNum"].ToString()) > 0)
                {
                    row.Cells.Add(PIn.Date(_tableComms.Rows[i]["commDateTime"].ToString()).ToShortDateString());
                    row.Cells.Add(_tableComms.Rows[i]["Note"].ToString());
                    if (_tableComms.Rows[i]["CommType"].ToString() == Commlogs.GetTypeAuto(CommItemTypeAuto.APPT).ToString())
                    {
                        row.BackColor = listMiscColorDefs[7].Color;
                    }
                }
                else if (PIn.Long(_tableComms.Rows[i]["EmailMessageNum"].ToString()) > 0)
                {
                    row.Cells.Add(PIn.Date(_tableComms.Rows[i]["commDateTime"].ToString()).ToShortDateString());
                    row.Cells.Add(_tableComms.Rows[i]["Subject"].ToString());
                }
                row.Tag = _tableComms.Rows[i];
                gridComm.Rows.Add(row);
            }
            gridComm.EndUpdate();
            gridComm.ScrollToEnd();
        }

        private void gridComm_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            DataRow row = ((DataRow)gridComm.Rows[e.Row].Tag);
            long commNum = PIn.Long(row["CommlogNum"].ToString());
            long msgNum = PIn.Long(row["EmailMessageNum"].ToString());
            if (commNum > 0)
            {
                Commlog item = Commlogs.GetOne(commNum);
                if (item == null)
                {
                    MsgBox.Show(this, "This commlog has been deleted by another user.");
                    return;
                }
                FormCommItem FormCI = new FormCommItem(item);
                FormCI.ShowDialog();
            }
            else if (msgNum > 0)
            {
                EmailMessage email = EmailMessage.GetById(msgNum);
                if (email == null)
                {
                    MsgBox.Show(this, "This e-mail has been deleted by another user.");
                    return;
                }
                FormEmailMessageEdit FormEME = new FormEmailMessageEdit(email, isDeleteAllowed: false);
                FormEME.ShowDialog();
            }
            _tableComms = Appointments.GetCommTable(AptCur.PatNum.ToString(), AptCur.AptNum);
            FillComm();
        }

        private void FillProcedures()
        {
            //Every time the procedures available have been manipulated (associated to appt, deleted, etc) we need to refresh the list from the db.
            //This has the potential to call the database a lot (cell click via a grid) but we accept this insufficiency for the benefit of concurrency.
            //If the following call to the db is to be removed, make sure that all procedure manipulations from FormProcEdit, FormClaimProcEdit, etc.
            //  handle the changes accordingly.  Changing this call to the database should not be done 'lightly'.  Heed our warning.
            List<Procedure> listProcs = _listProcsForAppt;
            listProcs.Sort(ProcedureLogic.CompareProcedures);
            List<long> listNumsSelected = new List<long>();
            if (_isOnLoad && !insertRequired)
            {//First time filling the grid and not a new appointment.
                if (_listPreSelectedProcNums != null)
                {
                    //Allows us to preselect procs without setting proc.PlannedAptNum in AppointmentL.CreatePlannedAppt(). Otherwise, downstream attach/detach
                    //logic has problems if we preselect by setting AptNum/PlannedAptNum because that logic uses _listProcNumsAttachedStart to determine if
                    //these procs were already attached to this appointment.
                    listNumsSelected.AddRange(_listPreSelectedProcNums.FindAll(x => listProcs.Any(y => y.ProcNum == x)));
                }
                if (_isPlanned)
                {
                    _listProcNumsAttachedStart = listProcs.FindAll(x => x.PlannedAptNum == AptCur.AptNum).Select(x => x.ProcNum).ToList();
                }
                else
                {//regular appointment
                 //set ProcNums attached to the appt when form opened for use in automation on closing.
                    _listProcNumsAttachedStart = listProcs.FindAll(x => x.AptNum == AptCur.AptNum).Select(x => x.ProcNum).ToList();
                }
                listNumsSelected.AddRange(_listProcNumsAttachedStart);
            }
            else
            {//Filling the grid later on.
                listNumsSelected.AddRange(gridProc.SelectedIndices.OfType<int>().Select(x => ((Procedure)gridProc.Rows[x].Tag).ProcNum));
            }
            bool isMedical = Clinic.GetById(_selectedClinicNum).IsMedicalOnly;
            gridProc.BeginUpdate();
            gridProc.Rows.Clear();
            gridProc.Columns.Clear();
            List<DisplayField> listAptDisplayFields;
            if (AptCur.AptStatus == ApptStatus.Planned)
            {
                listAptDisplayFields = DisplayFields.GetForCategory(DisplayFieldCategory.PlannedAppointmentEdit);
            }
            else
            {
                listAptDisplayFields = DisplayFields.GetForCategory(DisplayFieldCategory.AppointmentEdit);
            }
            foreach (DisplayField displayField in listAptDisplayFields)
            {
                if (isMedical && (displayField.InternalName == "Surf" || displayField.InternalName == "Tth"))
                {
                    continue;
                }
                gridProc.Columns.Add(new ODGridColumn(displayField.InternalName, displayField.ColumnWidth));
            }
            if (listAptDisplayFields.Sum(x => x.ColumnWidth) > gridProc.Width)
            {
                gridProc.HScrollVisible = true;
            }
            ODGridRow row;
            foreach (Procedure proc in listProcs)
            {
                row = new ODGridRow();
                ProcedureCode procCode = ProcedureCodes.GetProcCode(proc.CodeNum);
                foreach (DisplayField displayField in listAptDisplayFields)
                {
                    switch (displayField.InternalName)
                    {
                        case "Stat":
                            if (ProcMultiVisits.IsProcInProcess(proc.ProcNum))
                            {
                                row.Cells.Add(Lan.g("enumProcStat", ProcStatExt.InProcess));
                            }
                            else
                            {
                                row.Cells.Add(Lans.g("enumProcStat", proc.ProcStatus.ToString()));
                            }
                            break;
                        case "Priority":
                            row.Cells.Add(Defs.GetName(DefinitionCategory.TxPriorities, proc.Priority));
                            break;
                        case "Code":
                            row.Cells.Add(procCode.ProcCode);
                            break;
                        case "Tth":
                            if (isMedical)
                            {
                                continue;
                            }
                            row.Cells.Add(Tooth.GetToothLabel(proc.ToothNum));
                            break;
                        case "Surf":
                            if (isMedical)
                            {
                                continue;
                            }
                            row.Cells.Add(proc.Surf);
                            break;
                        case "Description":
                            string descript = "";
                            //This descript is gotten the same way it was in Appointments.GetProcTable()
                            if (_isPlanned && proc.PlannedAptNum != 0 && proc.PlannedAptNum != AptCur.AptNum)
                            {
                                descript += Lan.g(this, "(other appt) ");
                            }
                            else if (_isPlanned && proc.AptNum != 0 && proc.AptNum != AptCur.AptNum)
                            {
                                descript += Lan.g(this, "(scheduled appt) ");
                            }
                            else if (!_isPlanned && proc.PlannedAptNum != 0 && proc.PlannedAptNum != AptCur.AptNum)
                            {
                                descript += Lan.g(this, "(planned appt) ");
                            }
                            else if (!_isPlanned && proc.AptNum != 0 && proc.AptNum != AptCur.AptNum)
                            {
                                descript += Lan.g(this, "(other appt) ");
                            }
                            if (procCode.LaymanTerm == "")
                            {
                                descript += procCode.Descript;
                            }
                            else
                            {
                                descript += procCode.LaymanTerm;
                            }
                            if (proc.ToothRange != "")
                            {
                                descript += " #" + Tooth.FormatRangeForDisplay(proc.ToothRange);
                            }
                            row.Cells.Add(descript);
                            break;
                        case "Fee":
                            row.Cells.Add(proc.ProcFeeTotal.ToString("F"));
                            break;
                        case "Abbreviation":
                            row.Cells.Add(procCode.AbbrDesc);
                            break;
                        case "Layman's Term":
                            row.Cells.Add(procCode.LaymanTerm);
                            break;
                    }
                }
                row.Tag = proc;
                gridProc.Rows.Add(row);
            }
            gridProc.EndUpdate();
            for (int i = 0; i < listProcs.Count; i++)
            {
                if (listNumsSelected.Contains(listProcs[i].ProcNum))
                {
                    gridProc.SetSelected(i, true);
                }
            }
        }

        private string GetLabCaseDescript()
        {
            string descript = "";
            if (_labCur != null)
            {
                descript = Laboratories.GetOne(_labCur.LaboratoryNum).Description;
                if (_labCur.DateTimeChecked.Year > 1880)
                {//Logic from Appointments.cs lines 1818 to 1840
                    descript += ", " + Lan.g(this, "Quality Checked");
                }
                else
                {
                    if (_labCur.DateTimeRecd.Year > 1880)
                    {
                        descript += ", " + Lan.g(this, "Received");
                    }
                    else
                    {
                        if (_labCur.DateTimeSent.Year > 1880)
                        {
                            descript += ", " + Lan.g(this, "Sent");
                        }
                        else
                        {
                            descript += ", " + Lan.g(this, "Not Sent");
                        }
                        if (_labCur.DateTimeDue.Year > 1880)
                        {
                            descript += ", " + Lan.g(this, "Due: ") + _labCur.DateTimeDue.ToString("ddd") + " "
                                + _labCur.DateTimeDue.ToShortDateString() + " "
                                + _labCur.DateTimeDue.ToShortTimeString();
                        }
                    }
                }
            }
            return descript;
        }

        private void butAddComm_Click(object sender, EventArgs e)
        {
            Commlog CommlogCur = new Commlog();
            CommlogCur.PatNum = AptCur.PatNum;
            CommlogCur.CommDateTime = DateTime.Now;
            CommlogCur.CommType = Commlogs.GetTypeAuto(CommItemTypeAuto.APPT);
            CommlogCur.UserNum = Security.CurrentUser.Id;
            FormCommItem FormCI = new FormCommItem(CommlogCur);
            FormCI.IsNew = true;
            FormCI.ShowDialog();
            _tableComms = Appointments.GetCommTable(AptCur.PatNum.ToString(), AptCur.AptNum);
            FillComm();
        }

        private void butText_Click(object sender, EventArgs e)
        {
            if (Plugin.Trigger(this, "FormApptEdit_Button_Text", pat, AptCur)) return;
            bool updateTextYN = false;
            if (pat.TxtMsgOk == YN.No)
            {
                if (MsgBox.Show(this, MsgBoxButtons.YesNo, "This patient is marked to not receive text messages. "
                    + "Would you like to mark this patient as okay to receive text messages?"))
                {
                    updateTextYN = true;
                }
                else
                {
                    return;
                }
            }
            if (pat.TxtMsgOk == YN.Unknown && Preference.GetBool(PreferenceName.TextMsgOkStatusTreatAsNo))
            {
                if (MsgBox.Show(this, MsgBoxButtons.YesNo, "This patient might not want to receive text messages. "
                    + "Would you like to mark this patient as okay to receive text messages?"))
                {
                    updateTextYN = true;
                }
                else
                {
                    return;
                }
            }
            if (updateTextYN)
            {
                Patient patOld = pat.Copy();
                pat.TxtMsgOk = YN.Yes;
                Patients.Update(pat, patOld);
            }
            string message;
            message = Preference.GetString(PreferenceName.ConfirmTextMessage);
            message = message.Replace("[NameF]", pat.GetNameFirst());
            message = message.Replace("[NameFL]", pat.GetNameFL());
            message = message.Replace("[date]", AptCur.AptDateTime.ToShortDateString());
            message = message.Replace("[time]", AptCur.AptDateTime.ToShortTimeString());
            FormTxtMsgEdit FormTME = new FormTxtMsgEdit();
            FormTME.PatNum = pat.PatNum;
            FormTME.WirelessPhone = pat.WirelessPhone;
            FormTME.Message = message;
            FormTME.TxtMsgOk = pat.TxtMsgOk;
            FormTME.ShowDialog();
        }

        ///<summary>Will only invert the specified procedure in the grid, even if the procedure belongs to another appointment.</summary>
        private void InvertCurProcSelected(int index)
        {
            bool isSelected = gridProc.SelectedIndices.Contains(index);
            gridProc.SetSelected(index, !isSelected);//Invert selection.
        }

        private void gridProc_CellClick(object sender, ODGridClickEventArgs e)
        {
            if (_isClickLocked)
            {
                return;
            }
            InvertCurProcSelected(e.Row);
            CalculateTime();
            CalcPatientFeeThisAppt();
            CalcEstPatientPortion();
        }

        void gridProc_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (_isClickLocked) return;
            
            InvertCurProcSelected(e.Row);

            // This will put the selection back to what is was before the single click event.
            // Get fresh copy from DB so we are not editing a stale procedure
            // If this is to be changed, make sure that this window is registering for procedure changes via signals or by some other means.
            var procedure = Procedures.GetOneProc(((Procedure)gridProc.Rows[e.Row].Tag).ProcNum, true);

            using (var formProcEdit = new FormProcEdit(procedure, pat, fam))
            {
                formProcEdit.ShowDialog();
                if (formProcEdit.DialogResult != DialogResult.OK)
                {
                    CalculateTime();
                    return;
                }

            }

            _listProcsForAppt = Procedures.GetProcsForApptEdit(AptCur); // We need to refresh in case the user changed the ProcCode or set the proc complete.

            FillProcedures();
            CalculateTime();
            RefreshEstPatientPortion(); // Need to refresh in case the user changed the ProcCode or set the proc complete.
        }

        void procedureDeleteButton_Click(object sender, EventArgs e)
        {
            if (gridProc.SelectedIndices.Length == 0)
            {
                MessageBox.Show(
                    "Please select one or more procedures first.", 
                    "Appointment", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            var result =
                MessageBox.Show(
                    "Permanently delete all selected procedure(s)?",
                    "Appointment", 
                    MessageBoxButtons.OKCancel, 
                    MessageBoxIcon.Question, 
                    MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Cancel) return;

            int skipped = 0;
            int skippedSecurity = 0;
            bool isProcDeleted = false;

            try
            {
                for (int i = gridProc.SelectedIndices.Length - 1; i >= 0; i--)
                {
                    Procedure proc = (Procedure)gridProc.Rows[gridProc.SelectedIndices[i]].Tag;
                    if (!Procedures.IsProcComplEditAuthorized(proc))
                    {
                        skipped++;
                        skippedSecurity++;
                        continue;
                    }

                    if (!proc.ProcStatus.In(ProcStat.C, ProcStat.EO, ProcStat.EC) && !Security.IsAuthorized(Permissions.ProcDelete, proc.ProcDate, true))
                    {
                        skippedSecurity++;
                        continue;
                    }

                    Procedures.Delete(proc.ProcNum);
                    isProcDeleted = true;
                    if (proc.ProcStatus.In(ProcStat.C, ProcStat.EO, ProcStat.EC))
                    {
                        string perm = SecurityLogEvents.CompletedProcedureEdited;
                        if (proc.ProcStatus.In(ProcStat.EO, ProcStat.EC))
                        {
                            perm = SecurityLogEvents.ProcedureEdited;
                        }
                        SecurityLog.Write(AptCur.PatNum, perm, ProcedureCodes.GetProcCode(proc.CodeNum).ProcCode + " (" + proc.ProcStatus + "), " + proc.ProcFee.ToString("c") + ", Deleted");
                    }
                    else
                    {
                        SecurityLog.Write(AptCur.PatNum, SecurityLogEvents.ProcDelete, ProcedureCodes.GetProcCode(proc.CodeNum).ProcCode + " (" + proc.ProcStatus + "), " + proc.ProcFee.ToString("c"));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            _listProcsForAppt = Procedures.GetProcsForApptEdit(AptCur);
            if (isProcDeleted)
            {
                Appointments.SetProcDescript(AptCur, _listProcsForAppt);//This is called in Procedures.Delete(...) but is not reflected in our local AptCur.
                                                                        //This is to fix a very rare bug where the user deletes a set of procedures and then re-attaches the same procedures before closing the window.
                                                                        //This would cause the DB to have correct ProcDesript and ProcsColored values at the time. But when the user closes the window after reselecting
                                                                        //the same proces, the Appointments.Update(old,new) will not update those fields due to them being identical.
                                                                        //This would cause the appt bubble to contain the incorrect values.
                AptOld.ProcDescript = AptCur.ProcDescript;
                AptOld.ProcsColored = AptCur.ProcsColored;
            }

            FillProcedures();
            CalculateTime();
            CalcPatientFeeThisAppt();
            RefreshEstPatientPortion();

            if (skippedSecurity > skipped)
            {
                MessageBox.Show(
                    string.Format("Skipped {0} procedures due to lack of permission to delete procedures.", skippedSecurity),
                    "Appointment",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else if (skipped > 0)
            {
                MessageBox.Show(
                    string.Format("Skipped {0} procedures due to lack of permission to edit completed procedures.", skipped),
                    "Appointment",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        void procedureAddButton_Click(object sender, EventArgs e)
        {
            if (_selectedProvNum == 0)
            {
                MessageBox.Show(
                    "Please select a provider.", 
                    "Appointment", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            using (var formProcCodes = new FormProcCodes())
            {
                formProcCodes.IsSelectionMode = true;
                if (formProcCodes.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                ProcedureCode procedureCode = ProcedureCodes.GetProcCode(formProcCodes.SelectedCodeNum);
                List<SubstitutionLink> listSubstitutionLinks = SubstitutionLinks.GetAllForPlans(PlanList);
                List<Fee> listFees = Fees.GetListFromObjects(new List<ProcedureCode>() { procedureCode }, null,//no procs to pull medical codes from yet
                    new List<long>() { _selectedProvNum }, pat.PriProv, pat.SecProv, pat.FeeSched, PlanList, new List<long>() { AptCur.ClinicNum },
                    new List<Appointment>() { AptCur }, listSubstitutionLinks, pat.DiscountPlanNum);
                Procedure proc = Procedures.ConstructProcedureForAppt(formProcCodes.SelectedCodeNum, AptCur, pat, _listPatPlans, PlanList, SubList, listFees);
                Procedures.Insert(proc);
                List<ClaimProc> claimProcList = new List<ClaimProc>();
                Procedures.ComputeEstimates(proc, pat.PatNum, ref claimProcList, true, PlanList, _listPatPlans, _benefitList,
                    null, null, true,
                    pat.Age, SubList,
                    null, false, false, listSubstitutionLinks, false,
                    listFees);


                using (var formProcEdit = new FormProcEdit(proc, pat.Copy(), fam))
                {
                    formProcEdit.IsNew = true;

                    // TODO: Move this to a plugin...
                    //if (Programs.UsingOrion)
                    //{
                    //    formProcEdit.OrionProvNum = _selectedProvNum;
                    //    formProcEdit.OrionDentist = true;
                    //}

                    if (formProcEdit.ShowDialog() == DialogResult.Cancel)
                    {
                        try
                        {
                            Procedures.Delete(proc.ProcNum);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(
                                ex.Message, 
                                "Appointment", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                        }
                        return;
                    }
                }

                _listProcsForAppt = Procedures.GetProcsForApptEdit(AptCur);
                FillProcedures();
                for (int i = 0; i < gridProc.Rows.Count; i++)
                {
                    if (proc.ProcNum == ((Procedure)gridProc.Rows[i].Tag).ProcNum)
                    {
                        gridProc.SetSelected(i, true);//Select those that were just added.
                    }
                }
                CalculateTime();
                CalcPatientFeeThisAppt();
                RefreshEstPatientPortion();
            }
        }

        private void butAttachAll_Click(object sender, EventArgs e)
        {
            if (_isClickLocked)
            {
                return;
            }
            gridProc.SetSelected(true);
            CalculateTime();
            CalcPatientFeeThisAppt();
        }

        void CalculateTime(bool ignoreTimeLocked = false)
        {
            var procedures = new List<Procedure>();
            foreach (int index in gridProc.SelectedIndices)
            {
                procedures.Add((Procedure)gridProc.Rows[index].Tag);
            }

            timeBar.Pattern = 
                Appointments.CalculatePattern(
                    pat, 
                    _selectedProvNum, 
                    _selectedProvHygNum, 
                    procedures, 
                    checkTimeLocked.Checked, 
                    ignoreTimeLocked);
        }

        void checkTimeLocked_Click(object sender, EventArgs e) => CalculateTime();

        private void gridPatient_CellClick(object sender, ODGridClickEventArgs e)
        {
            //ODGridCell gridCellCur = gridPatient.Rows[e.Row].Cells[e.Column];
            ////Only grid cells with phone numbers are blue and underlined.
            //if (gridCellCur.ColorText == System.Drawing.Color.Blue && gridCellCur.Underline == true && Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled)
            //{
            //    DentalTekBridge.PlaceCall(gridCellCur.Text);
            //}
        }

        void quickAddListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _isClickLocked) return;

            if (_selectedProvNum == 0)
            {
                MessageBox.Show(
                    "Please select a provider.", 
                    "Appointment", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            var index = quickAddListBox.IndexFromPoint(e.X, e.Y);
            if (index == -1)
                return;
            
            // If the appointment status is set to complete, any procedures that will be added are going to be
            // marked as completed as soon as the form closes. If the user does not have the permission to mark
            // procedures as complete, don't let them add procedures to this appointment.
            if (AptCur.AptStatus == ApptStatus.Complete)
            {
                if (!Security.IsAuthorized(Permissions.CreateCompletedProcedure))
                {
                    return;
                }
            }

            string[] codes = _listApptProcsQuickAddDefs[index].Value.Split(',');
            for (int i = 0; i < codes.Length; i++)
            {
                if (!ProcedureCodes.GetContainsKey(codes[i])) // these are D codes, not codeNums.
                {
                    MessageBox.Show(
                        "Definition contains invalid code.", 
                        "Appointment", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);

                    return;
                }
            }


            Tuple<List<Procedure>, List<Procedure>> result = ApptEdit.QuickAddProcs(AptCur, pat, codes.ToList(), _selectedProvNum, _selectedProvHygNum, SubList, PlanList, _listPatPlans, _benefitList);
            List<Procedure> listAddedProcs = result.Item1;
            _listProcsForAppt = result.Item2;

            // Orion requires a DPC for every procedure. Force proc edit window open.
            //if (Programs.UsingOrion)
            //{
            //    foreach (var procedure in listAddedProcs)
            //    {
            //        using (var formProcEdit = new FormProcEdit(procedure, pat.Copy(), fam))
            //        {
            //            formProcEdit.IsNew = true;
            //            formProcEdit.OrionDentist = true;

            //            if (formProcEdit.ShowDialog() == DialogResult.Cancel)
            //            {
            //                try
            //                {
            //                    Procedures.Delete(procedure.ProcNum);
            //                }
            //                catch (Exception ex)
            //                {
            //                    MessageBox.Show(
            //                        ex.Message, 
            //                        "Appointment", 
            //                        MessageBoxButtons.OK, 
            //                        MessageBoxIcon.Error);
            //                }
            //            }
            //        }
            //    }
            //}

            // Clear the selection.
            quickAddListBox.SelectedIndex = -1;

            // Reloads the procedures grid.
            FillProcedures();

            // Select all the procedures that were just added.
            for (int i = 0; i < gridProc.Rows.Count; i++)
            {
                long rowProcNum = ((Procedure)gridProc.Rows[i].Tag).ProcNum;
                if (listAddedProcs.Any(x => x.ProcNum == rowProcNum))
                {
                    gridProc.SetSelected(i, true);
                }
            }

            CalculateTime();
            CalcPatientFeeThisAppt();
            RefreshEstPatientPortion();
        }

        private void butLab_Click(object sender, EventArgs e)
        {
            if (insertRequired && !UpdateListAndDB(false))
            {
                return;
            }
            if (_labCur == null)
            {//no labcase
             //so let user pick one to add
                FormLabCaseSelect FormL = new FormLabCaseSelect();
                FormL.PatNum = AptCur.PatNum;
                FormL.IsPlanned = _isPlanned;
                FormL.ShowDialog();
                if (FormL.DialogResult != DialogResult.OK)
                {
                    return;
                }
                if (_isPlanned)
                {
                    LabCases.AttachToPlannedAppt(FormL.SelectedLabCaseNum, AptCur.AptNum);
                }
                else
                {
                    LabCases.AttachToAppt(FormL.SelectedLabCaseNum, AptCur.AptNum);
                }
            }
            else
            {//already a labcase attached
                FormLabCaseEdit FormLCE = new FormLabCaseEdit();
                FormLCE.CaseCur = _labCur;
                FormLCE.ShowDialog();
                if (FormLCE.DialogResult != DialogResult.OK)
                {
                    return;
                }
                //Deleting or detaching labcase would have been done from in that window
            }
            _labCur = LabCases.GetForApt(AptCur);
            textLabCase.Text = GetLabCaseDescript();
        }

        private void butInsPlan1_Click(object sender, EventArgs e)
        {
            FormInsPlanSelect FormIPS = new FormInsPlanSelect(AptCur.PatNum);
            FormIPS.ShowNoneButton = true;
            FormIPS.ViewRelat = false;
            FormIPS.ShowDialog();
            if (FormIPS.DialogResult != DialogResult.OK)
            {
                return;
            }
            if (FormIPS.SelectedPlan == null)
            {
                AptCur.InsPlan1 = 0;
                textInsPlan1.Text = "";
                return;
            }
            AptCur.InsPlan1 = FormIPS.SelectedPlan.PlanNum;
            textInsPlan1.Text = InsPlans.GetCarrierName(AptCur.InsPlan1, PlanList);
        }

        private void butInsPlan2_Click(object sender, EventArgs e)
        {
            FormInsPlanSelect FormIPS = new FormInsPlanSelect(AptCur.PatNum);
            FormIPS.ShowNoneButton = true;
            FormIPS.ViewRelat = false;
            FormIPS.ShowDialog();
            if (FormIPS.DialogResult != DialogResult.OK)
            {
                return;
            }
            if (FormIPS.SelectedPlan == null)
            {
                AptCur.InsPlan2 = 0;
                textInsPlan2.Text = "";
                return;
            }
            AptCur.InsPlan2 = FormIPS.SelectedPlan.PlanNum;
            textInsPlan2.Text = InsPlans.GetCarrierName(AptCur.InsPlan2, PlanList);
        }

        private void butRequirement_Click(object sender, EventArgs e)
        {
        }

        private void butSyndromicObservations_Click(object sender, EventArgs e)
        {
            FormEhrAptObses formE = new FormEhrAptObses(AptCur);
            formE.ShowDialog();
        }

        private void menuItemArrivedNow_Click(object sender, EventArgs e)
        {
            textTimeArrived.Text = DateTime.Now.ToShortTimeString();
        }

        private void menuItemSeatedNow_Click(object sender, EventArgs e)
        {
            textTimeSeated.Text = DateTime.Now.ToShortTimeString();
        }

        private void menuItemDismissedNow_Click(object sender, EventArgs e)
        {
            textTimeDismissed.Text = DateTime.Now.ToShortTimeString();
        }

        private void gridFields_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (insertRequired && !UpdateListAndDB(false))
            {
                return;
            }

            AppointmentField field = AppointmentField.GetById(PIn.Long(_tableFields.Rows[e.Row]["ApptFieldNum"].ToString()));
            if (field == null)
            {
                field = new AppointmentField();
                field.AppointmentId = AptCur.AptNum;
                field.FieldName = _tableFields.Rows[e.Row]["FieldName"].ToString();
                AppointmentFieldDefinition fieldDef = AppointmentFieldDefinition.GetByFieldName(field.FieldName);
                if (fieldDef == null)
                {//This could happen if the field def was deleted while the appointment window was open.
                    MsgBox.Show(this, "This Appointment Field Def no longer exists.");
                }
                else
                {
                    if (fieldDef.FieldType == AppointmentFieldType.Text)
                    {
                        FormApptFieldEdit formAF = new FormApptFieldEdit(field);
                        formAF.IsNew = true;
                        formAF.ShowDialog();
                    }
                    else if (fieldDef.FieldType == AppointmentFieldType.PickList)
                    {
                        FormApptFieldPickEdit formAF = new FormApptFieldPickEdit(field);
                        formAF.IsNew = true;
                        formAF.ShowDialog();
                    }
                }
            }
            else if (AppointmentFieldDefinition.GetByFieldName(field.FieldName) != null)
            {
                if (AppointmentFieldDefinition.GetByFieldName(field.FieldName).FieldType == AppointmentFieldType.Text)
                {
                    FormApptFieldEdit formAF = new FormApptFieldEdit(field);
                    formAF.ShowDialog();
                }
                else if (AppointmentFieldDefinition.GetByFieldName(field.FieldName).FieldType == AppointmentFieldType.PickList)
                {
                    FormApptFieldPickEdit formAF = new FormApptFieldPickEdit(field);
                    formAF.ShowDialog();
                }
            }
            else
            {//This probably won't happen because a field def should not be able to be deleted while in use.
                MsgBox.Show(this, "This Appointment Field Def no longer exists.");
            }
            _tableFields = Appointments.GetApptFields(AptCur.AptNum);
            FillFields();
        }

        ///<summary>Validates and saves appointment and procedure information to DB.</summary>
        private bool UpdateListAndDB(bool isClosing = true, bool doCreateSecLog = false, bool doInsertHL7 = false)
        {
            DateTime datePrevious = AptCur.DateTStamp;
            List<long> listAptsToDelete = new List<long>();
            _listProcsForAppt = Procedures.GetProcsForApptEdit(AptCur);//We need to refresh so we can check for concurrency issues.
            FillProcedures();//This refills the tags in the grid so we can use the tags below.  Will also show concurrent changes by other users.
            #region PrefName.ApptsRequireProc and Permissions.ProcComplCreate check
            //First check that they have an procedures attached to this appointment. If the appointment is an existing appointment that did not originally
            //have any procedures attached, the prompt will not come up.
            if ((IsNew || _listProcNumsAttachedStart.Count > 0)
                && Preference.GetBool(PreferenceName.ApptsRequireProc)
                && gridProc.SelectedIndices.Length == 0
                && !AptCur.AptStatus.In(ApptStatus.PtNote, ApptStatus.PtNoteCompleted))
            {
                MsgBox.Show(this, "At least one procedure must be attached to the appointment.");
                return false;
            }
            if (_selectedApptStatus == ApptStatus.Complete
                && gridProc.SelectedIndices.Select(x => (Procedure)gridProc.Rows[x].Tag).Any(x => x.ProcStatus != ProcStat.C))
            {//Appt is complete, but a selected proc is not.
                List<Procedure> listSelectedProcs = gridProc.SelectedIndices.Select(x => (Procedure)gridProc.Rows[x].Tag).ToList();
                listSelectedProcs.RemoveAll(x => x.ProcStatus == ProcStat.C);//only care about the procs that are not already complete (new attaching procs)
                foreach (Procedure proc in listSelectedProcs)
                {
                    if (!Security.IsAuthorized(Permissions.CreateCompletedProcedure, AptCur.AptDateTime, proc.CodeNum, proc.ProcFee))
                    {
                        return false;
                    }
                }
            }
            #endregion
            #region Check for Procs Attached to Another Appt
            //When _isInsertRequired is true AptCur.AptNum=0.
            //The below logic works when 0 due to AptCur.[Planned]AptNum!=0 checks.
            bool hasProcsConcurrent = false;
            //This dictionary holds the original aptNum for a previously attached procedure. 
            //The value is the count of procedures being moved from the associated aptNum.
            //We will use this to determine if the procedure's original appointment needs to be deleted (if all procedures are moved to another appointment).
            Dictionary<long, int> dictProcsBeingMoved = new Dictionary<long, int>();
            for (int i = 0; i < gridProc.Rows.Count; i++)
            {
                Procedure proc = (Procedure)gridProc.Rows[i].Tag;
                bool isAttaching = gridProc.SelectedIndices.Contains(i);
                bool isAttachedStart = _listProcNumsAttachedStart.Contains(proc.ProcNum);
                if (!isAttachedStart && isAttaching && _isPlanned)
                {//Attaching to this planned appointment.
                    if (proc.PlannedAptNum != 0 && proc.PlannedAptNum != AptCur.AptNum)
                    {//However, the procedure is attached to another planned appointment.
                        hasProcsConcurrent = true;
                        //Make note of the appointment the procedure will be moved off of.
                        if (!dictProcsBeingMoved.ContainsKey(proc.PlannedAptNum))
                        {
                            dictProcsBeingMoved[proc.PlannedAptNum] = 0;
                        }
                        dictProcsBeingMoved[proc.PlannedAptNum]++;
                    }
                }
                else if (!isAttachedStart && isAttaching && !_isPlanned)
                {//Attaching to this appointment.
                    if (proc.AptNum != 0 && proc.AptNum != AptCur.AptNum)
                    {//However, the procedure is attached to another appointment.
                        hasProcsConcurrent = true;
                        //Make note of the appointment the procedure will be moved off of.
                        if (!dictProcsBeingMoved.ContainsKey(proc.AptNum))
                        {
                            dictProcsBeingMoved[proc.AptNum] = 0;
                        }
                        dictProcsBeingMoved[proc.AptNum]++;
                    }
                }
            }
            if (Preference.GetBool(PreferenceName.ApptsRequireProc) && dictProcsBeingMoved.Count > 0)
            {//Only check if we are actually moving procedures.
                Dictionary<long, int> dictAptsProcCount = Appointments.GetProcCountForUnscheduledApts(dictProcsBeingMoved.Keys.ToList());
                //Check to see if the number of procedures we are stealing from the original appointment is the same
                //as the total number of procedures on the appointment. If this is the case the appointment must be deleted.
                //Per the job for this feature we will only delete unscheduled appointments that become empty.
                foreach (long aptNum in dictAptsProcCount.Keys)
                {
                    if (dictProcsBeingMoved[aptNum] == dictAptsProcCount[aptNum])
                    {
                        listAptsToDelete.Add(aptNum);
                    }
                }
            }
            if (listAptsToDelete.Count > 0)
            {
                //Verbiage approved by Allen
                if (!MsgBox.Show(this, MsgBoxButtons.YesNo,
                    "One or more procedures are attached to another appointment.\r\n"
                    + "All selected procedures will be detached from the other appointment which will result in its deletion.\r\n"
                    + "Continue?"))
                {
                    return false;
                }
            }
            else if (hasProcsConcurrent && _isPlanned)
            {
                if (!MsgBox.Show(this, MsgBoxButtons.OKCancel,
                    "One or more procedures are attached to another planned appointment.\r\n"
                    + "All selected procedures will be detached from the other planned appointment.\r\n"
                    + "Continue?"))
                {
                    return false;
                }
            }
            else if (hasProcsConcurrent && !_isPlanned)
            {
                if (!MsgBox.Show(this, MsgBoxButtons.OKCancel,
                    "One or more procedures are attached to another appointment.\r\n"
                    + "All selected procedures will be detached from the other appointment.\r\n"
                    + "Continue?"))
                {
                    return false;
                }
            }
            #endregion Check for Procs Attached to Another Appt
            #region Validate Form Data
            //initial clinic selection based on Op, but user may also edit, so use selection.  The clinic combobox is the logical place to look
            //when being warned/blocked about specialty mismatch.  
            if (!AppointmentL.IsSpecialtyMismatchAllowed(AptCur.PatNum, _selectedClinicNum))
            {
                return false;
            }
            if (AptOld.AptStatus != ApptStatus.UnschedList && statusComboBox.SelectedIndex == 2)
            {//previously not on unsched list and sending to unscheduled list
                if (PatRestrictionL.IsRestricted(AptCur.PatNum, PatRestrict.ApptSchedule, true))
                {
                    MessageBox.Show(Lan.g(this, "Not allowed to send this appointment to the unscheduled list due to patient restriction") + " "
                        + PatRestrictions.GetPatRestrictDesc(PatRestrict.ApptSchedule) + ".");
                    return false;
                }
                if (Preference.GetBool(PreferenceName.UnscheduledListNoRecalls)
                    && Appointments.IsRecallAppointment(AptCur, gridProc.SelectedGridRows.Select(x => (Procedure)(x.Tag)).ToList()))
                {
                    if (MsgBox.Show(this, MsgBoxButtons.YesNo, "Recall appointments cannot be sent to the Unscheduled List.\r\nDelete appointment instead?"))
                    {
                        OnDelete_Click(true);//Skip the standard "Delete Appointment?" prompt since we have already prompted here. Closes form and syncs data.
                    }
                    return false;//Always return false since the appointment was either deleted of the user canceled.
                }
            }
            DateTime dateTimeAskedToArrive = DateTime.MinValue;
            if ((AptOld.AptStatus == ApptStatus.Complete && statusComboBox.SelectedIndex != 1)
                || (AptOld.AptStatus == ApptStatus.Broken && statusComboBox.SelectedIndex != 4)) //Un-completing or un-breaking the appt.  We must use selectedindex due to AptCur gets updated later UpdateDB()
            {
                //If the insurance plans have changed since this appt was completed, warn the user that the historical data will be neutralized.
                List<PatPlan> listPatPlans = PatPlans.Refresh(pat.PatNum);
                InsSub sub1 = InsSubs.GetSub(PatPlans.GetInsSubNum(listPatPlans, PatPlans.GetOrdinal(PriSecMed.Primary, listPatPlans, PlanList, SubList)), SubList);
                InsSub sub2 = InsSubs.GetSub(PatPlans.GetInsSubNum(listPatPlans, PatPlans.GetOrdinal(PriSecMed.Secondary, listPatPlans, PlanList, SubList)), SubList);
                if (sub1.PlanNum != AptCur.InsPlan1 || sub2.PlanNum != AptCur.InsPlan2)
                {
                    if (!MsgBox.Show(this, MsgBoxButtons.OKCancel, "The current insurance plans for this patient are different than the plans associated to this appointment.  They will be updated to the patient's current insurance plans.  Continue?"))
                    {
                        return false;
                    }
                    //Update the ins plans associated to this appointment so that they're the most accurate at this time.
                    AptCur.InsPlan1 = sub1.PlanNum;
                    AptCur.InsPlan2 = sub2.PlanNum;
                }
            }
            if (textTimeAskedToArrive.Text != "")
            {
                try
                {
                    dateTimeAskedToArrive = AptCur.AptDateTime.Date + DateTime.Parse(textTimeAskedToArrive.Text).TimeOfDay;
                }
                catch
                {
                    MsgBox.Show(this, "Time Asked To Arrive invalid.");
                    return false;
                }
            }
            DateTime dateTimeArrived = AptCur.AptDateTime.Date;
            if (textTimeArrived.Text != "")
            {
                try
                {
                    dateTimeArrived = AptCur.AptDateTime.Date + DateTime.Parse(textTimeArrived.Text).TimeOfDay;
                }
                catch
                {
                    MsgBox.Show(this, "Time Arrived invalid.");
                    return false;
                }
            }
            DateTime dateTimeSeated = AptCur.AptDateTime.Date;
            if (textTimeSeated.Text != "")
            {
                try
                {
                    dateTimeSeated = AptCur.AptDateTime.Date + DateTime.Parse(textTimeSeated.Text).TimeOfDay;
                }
                catch
                {
                    MsgBox.Show(this, "Time Seated invalid.");
                    return false;
                }
            }
            DateTime dateTimeDismissed = AptCur.AptDateTime.Date;
            if (textTimeDismissed.Text != "")
            {
                try
                {
                    dateTimeDismissed = AptCur.AptDateTime.Date + DateTime.Parse(textTimeDismissed.Text).TimeOfDay;
                }
                catch
                {
                    MsgBox.Show(this, "Time Dismissed invalid.");
                    return false;
                }
            }
            //This change was just slightly too risky to make to 6.9, so 7.0 only
            if (!Preference.GetBool(PreferenceName.ApptAllowFutureComplete)//Not allowed to set future appts complete.
                && AptCur.AptStatus != ApptStatus.Complete//was not originally complete
                && AptCur.AptStatus != ApptStatus.PtNote
                && AptCur.AptStatus != ApptStatus.PtNoteCompleted
                && statusComboBox.SelectedIndex == 1 //making it complete
                && AptCur.AptDateTime.Date > DateTime.Today)//and future appt
            {
                MsgBox.Show(this, "Not allowed to set future appointments complete.");
                return false;
            }
            bool hasProcsAttached = gridProc.SelectedIndices
                //Get tags on rows as procedures if possible
                .Select(x => gridProc.Rows[x].Tag as Procedure)
                //true if any row had a valid procedure as a tag
                .Any(x => x != null);
            if (!Preference.GetBool(PreferenceName.ApptAllowEmptyComplete)
                && AptCur.AptStatus != ApptStatus.Complete//was not originally complete
                && AptCur.AptStatus != ApptStatus.PtNote
                && AptCur.AptStatus != ApptStatus.PtNoteCompleted
                && statusComboBox.SelectedIndex == 1)//making it complete
            {
                if (!hasProcsAttached)
                {
                    MsgBox.Show(this, "Appointments without procedures attached can not be set complete.");
                    return false;
                }
            }
            #region Security checks
            if (AptCur.AptStatus != ApptStatus.Complete//was not originally complete
                && _selectedApptStatus == ApptStatus.Complete //trying to make it complete
                && hasProcsAttached
                && !Security.IsAuthorized(Permissions.CreateCompletedProcedure, AptCur.AptDateTime))//aren't authorized to complete procedures
            {
                return false;
            }
            #endregion
            #region Provider Term Date Check
            //Prevents appointments with providers that are past their term end date from being scheduled
            Appointment aptModified = AptCur.Copy();//Appt used only for the providers S class method
            aptModified.ProvNum = _selectedProvNum;
            aptModified.ProvHyg = _selectedProvHygNum;
            if (_selectedApptStatus != ApptStatus.UnschedList && _selectedApptStatus != ApptStatus.Planned)
            {
                string message = Providers.CheckApptProvidersTermDates(aptModified);
                if (message != "")
                {
                    MessageBox.Show(this, message);//translated in Providers S class method
                    return false;
                }
            }
            #endregion Provider Term Date Check
            List<Procedure> listProcs = gridProc.SelectedIndices.OfType<int>().Select(x => (Procedure)gridProc.Rows[x].Tag).ToList();
            if (listProcs.Count > 0 && statusComboBox.SelectedIndex == 1 && AptCur.AptDateTime.Date > DateTime.Today.Date
                && !Preference.GetBool(PreferenceName.FutureTransDatesAllowed))
            {
                MsgBox.Show(this, "Not allowed to set procedures complete with future dates.");
                return false;
            }
            #endregion Validate Form Data
            //-----Point of no return-----
            #region Broken appt selections
            if (_formApptBreakSelection == ApptBreakSelection.Unsched && !AppointmentL.ValidateApptUnsched(AptCur))
            {
                _formApptBreakSelection = ApptBreakSelection.None;//This way no additional logic runs below.
            }
            if (_formApptBreakSelection == ApptBreakSelection.Pinboard && !AppointmentL.ValidateApptToPinboard(AptCur))
            {
                _formApptBreakSelection = ApptBreakSelection.None;//This way no additional logic runs below.
            }
            #endregion
            #region Set AptCur Fields
            AptCur.Pattern = timeBar.Pattern;
            //Only run appt overlap check if editing an appt not in unscheduled list and in chart module and eCW program link not enabled.
            //Also need to see if there is a generic HL7 def enabled where Open Dental is not the filler application.
            //Open Dental is the filler application if appointments, schedules, and operatories are maintained by Open Dental and messages are sent out
            //to inform another software of any changes made.  If Open Dental is an auxiliary application, appointments are created from inbound SIU
            //messages and Open Dental no longer has control over whether the appointments overlap or which operatory/provider's schedule the appointment
            //belongs to.  In this case, we do not want to check for overlapping appointments and the appointment module should be hidden.
            HL7Def hl7DefEnabled = HL7Defs.GetOneDeepEnabled();//the ShowAppts check box is hidden for MedLab HL7 interfaces, so only need to check the others
            bool isAuxiliaryRole = false;
            if (hl7DefEnabled != null && !hl7DefEnabled.ShowAppts)
            {//if the appts module is hidden
             //if an inbound SIU message is defined, OD is the auxiliary application which neither exerts control over nor requests changes to a schedule
                isAuxiliaryRole = hl7DefEnabled.hl7DefMessages.Any(x => x.MessageType == MessageTypeHL7.SIU && x.InOrOut == InOutHL7.Incoming);
            }
            if ((IsInChartModule || IsInViewPatAppts)
                && AptCur.AptStatus != ApptStatus.UnschedList
                && !isAuxiliaryRole)//generic HL7 def enabled, appt module hidden and an inbound SIU msg defined, appts created from msgs so no overlap check
            {
                //Adjusts AptCur.Pattern directly when necessary.
                if (ContrAppt.TryAdjustAppointmentPattern(AptCur))
                {
                    MsgBox.Show(this, "Appointment is too long and would overlap another appointment.  Automatically shortened to fit.");
                }
            }
            AptCur.Priority = checkASAP.Checked ? ApptPriority.ASAP : ApptPriority.Normal;
            AptCur.AptStatus = _selectedApptStatus;
            //set procs complete was moved further down
            if (comboUnschedStatus.SelectedIndex == 0)
            {//none
                AptCur.UnschedStatus = 0;
            }
            else
            {
                AptCur.UnschedStatus = _listRecallUnschedStatusDefs[comboUnschedStatus.SelectedIndex - 1].Id;
            }
            if (comboConfirmed.SelectedIndex != -1)
            {
                AptCur.Confirmed = _listApptConfirmedDefs[comboConfirmed.SelectedIndex].Id;
            }
            AptCur.TimeLocked = checkTimeLocked.Checked;
            AptCur.ColorOverride = butColor.BackColor;
            AptCur.Note = appointmentNoteTextBox.Text;
            AptCur.ClinicNum = _selectedClinicNum;
            AptCur.ProvNum = _selectedProvNum;
            AptCur.ProvHyg = _selectedProvHygNum;
            AptCur.IsHygiene = checkIsHygiene.Checked;
            if (comboAssistant.SelectedIndex == 0)
            {//none
                AptCur.Assistant = 0;
            }
            else
            {
                AptCur.Assistant = _listEmployees[comboAssistant.SelectedIndex - 1].Id;
            }
            AptCur.IsNewPatient = checkIsNewPatient.Checked;
            AptCur.DateTimeAskedToArrive = dateTimeAskedToArrive;
            AptCur.DateTimeArrived = dateTimeArrived;
            AptCur.DateTimeSeated = dateTimeSeated;
            AptCur.DateTimeDismissed = dateTimeDismissed;
            //AptCur.InsPlan1 and InsPlan2 already handled 
            if (comboApptType.SelectedIndex == 0)
            {//0 index = none.
                AptCur.AppointmentTypeNum = 0;
            }
            else
            {
                AptCur.AppointmentTypeNum = _listAppointmentType[comboApptType.SelectedIndex - 1].Id;
            }
            #endregion Set AptCur Fields
            #region Update ProcDescript for Appt
            //Use the current selections to set AptCur.ProcDescript.
            List<Procedure> listGridSelectedProcs = new List<Procedure>();
            gridProc.SelectedIndices.ToList().ForEach(x => listGridSelectedProcs.Add(_listProcsForAppt[x].Copy()));
            foreach (Procedure proc in listGridSelectedProcs)
            {
                //This allows Appointments.SetProcDescript(...) to associate all the passed in procs into AptCur.ProcDescript
                //listGridSelectedProcs is only used here and contains copies of procs.
                proc.AptNum = AptCur.AptNum;
                proc.PlannedAptNum = AptCur.AptNum;
            }
            Appointments.SetProcDescript(AptCur, listGridSelectedProcs);
            #endregion Update ProcDescript for Appt
            #region Provider change and fee change check
            //Determins if we would like to update ProcFees when a provider changes, considers PrefName.ProcFeeUpdatePrompt.
            InsPlan aptInsPlan1 = InsPlans.GetPlan(AptCur.InsPlan1, PlanList);//we only care about lining the fees up with the primary insurance plan
            bool updateProcFees = false;
            if (AptCur.AptStatus != ApptStatus.Complete && (_selectedProvNum != AptOld.ProvNum || _selectedProvHygNum != AptOld.ProvHyg))
            {//Either the primary or hygienist changed.
                List<Procedure> listNewProcs = gridProc.SelectedIndices.Select(x => Procedures.UpdateProcInAppointment(AptCur, ((Procedure)gridProc.Rows[x].Tag).Copy())).ToList();
                List<Procedure> listOldProcs = gridProc.SelectedIndices.Select(x => ((Procedure)gridProc.Rows[x].Tag).Copy()).ToList();
                string promptText = "";
                updateProcFees = Procedures.ShouldFeesChange(listNewProcs, listOldProcs, aptInsPlan1, ref promptText);
                if (updateProcFees && promptText != "" && !MsgBox.Show(this, MsgBoxButtons.YesNo, promptText))
                {
                    updateProcFees = false;
                }
            }
            bool removeCompleteProcs = ProcedureL.DoRemoveCompletedProcs(AptCur, _listProcsForAppt, true);
            #endregion
            #region Save to DB
            Appointments.ApptSaveHelperResult result;
            try
            {
                result = Appointments.ApptSaveHelper(AptCur, AptOld, insertRequired, _listProcsForAppt, _listAppointments,
                    gridProc.SelectedIndices.ToList(), _listProcNumsAttachedStart, _isPlanned, PlanList, SubList, _selectedProvNum, _selectedProvHygNum,
                    listGridSelectedProcs, IsNew, pat, fam, updateProcFees, removeCompleteProcs, doCreateSecLog, doInsertHL7);
                AptCur = result.AptCur;
                _listProcsForAppt = result.ListProcsForAppt;
                _listAppointments = result.ListAppts;
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            if (insertRequired && AptOld.AptNum == 0)
            {
                //Update the the old AptNum since this is a new appointment.
                //This stops Appointments.Sync(...) from double insertings this new appointment.
                AptOld.AptNum = AptCur.AptNum;
                _listAppointmentsOld.FirstOrDefault(x => x.AptNum == 0).AptNum = AptCur.AptNum;
            }
            insertRequired = false;//Now that we have inserted the new appointment, let typical appointment logic handle from here on.
            #endregion Save changes to DB
            #region Update gridProc tags
            //update tags with changes made so that anyone accessing it later has an updated copy.
            foreach (int index in gridProc.SelectedIndices)
            {
                Procedure procNew = _listProcsForAppt.FirstOrDefault(x => x.ProcNum == ((Procedure)gridProc.Rows[index].Tag).ProcNum);
                if (procNew == null)
                {
                    continue;
                }
                gridProc.Rows[index].Tag = procNew.Copy();
            }
            #endregion
            #region Automation and Broken Appt Logic
            if (result.DoRunAutomation)
            {
                AutomationL.Trigger(AutomationTrigger.CompleteProcedure, _listProcsForAppt.FindAll(x => x.AptNum == AptCur.AptNum)
                    .Select(x => ProcedureCodes.GetStringProcCode(x.CodeNum)).ToList(), AptCur.PatNum);
            }
            //Do the appointment "break" automation for appointments that were just broken.
            if (AptCur.AptStatus == ApptStatus.Broken && AptOld.AptStatus != ApptStatus.Broken)
            {
                AppointmentL.BreakApptHelper(AptCur, pat, _procCodeBroken);
                if (isClosing)
                {
                    switch (_formApptBreakSelection)
                    {//ApptBreakSelection.None by default.
                        case ApptBreakSelection.Unsched:
                            AppointmentL.SetApptUnschedHelper(AptCur, pat);
                            break;
                        case ApptBreakSelection.Pinboard:
                            AppointmentL.CopyAptToPinboardHelper(AptCur);
                            break;
                        case ApptBreakSelection.None://User did not makes selection
                        case ApptBreakSelection.ApptBook://User made selection, no extra logic required.
                            break;
                    }
                }
            }
            #endregion Broken Appt Logic
            #region Cleanup Empty Apts
            //We have finished saving this appointment. We can now safely delete the unscheduled appointments marked for deletion.
            if (listAptsToDelete.Count > 0)
            {
                Appointments.Delete(listAptsToDelete);
                //Nathan asked for a specific log entry message explaining why each apt was deleted.
                foreach (long aptNumDeleted in listAptsToDelete)
                {
                    SecurityLog.Write(AptCur.PatNum
                        , SecurityLogEvents.AppointmentEdited, "All procedures were moved off of the appointment, resulting in its deletion."
                        , aptNumDeleted, DateTime.MinValue);
                }
            }
            #endregion
            return true;
        }

        private void butPDF_Click(object sender, EventArgs e)
        {
            if (insertRequired)
            {
                MsgBox.Show(this, "Please click OK to create this appointment before taking this action.");
                return;
            }
            //this will only happen for eCW HL7 interface users.
            List<Procedure> listProcsForAppt = Procedures.GetProcsForSingle(AptCur.AptNum, AptCur.AptStatus == ApptStatus.Planned);
            string duplicateProcs = ProcedureL.ProcsContainDuplicates(listProcsForAppt);
            if (duplicateProcs != "")
            {
                MessageBox.Show(duplicateProcs);
                return;
            }
            //Send DFT to eCW containing a dummy procedure with this appointment in a .pdf file.	
            //no security
            string pdfDataStr = GenerateProceduresIntoPdf();
            if (HL7Defs.IsExistingHL7Enabled())
            {
                //PDF messages do not contain FT1 segments, so proc list can be empty
                //MessageHL7 messageHL7=MessageConstructor.GenerateDFT(procs,EventTypeHL7.P03,pat,Patients.GetPat(pat.Guarantor),AptCur.AptNum,"progressnotes",pdfDataStr);
                MessageHL7 messageHL7 = MessageConstructor.GenerateDFT(new List<Procedure>(), EventTypeHL7.P03, pat, Patients.GetPat(pat.Guarantor), AptCur.AptNum, "progressnotes", pdfDataStr);
                if (messageHL7 == null)
                {
                    MsgBox.Show(this, "There is no DFT message type defined for the enabled HL7 definition.");
                    return;
                }
                HL7Msg hl7Msg = new HL7Msg();
                //hl7Msg.AptNum=AptCur.AptNum;
                hl7Msg.AptNum = 0;//Prevents the appt complete button from changing to the "Revise" button prematurely.
                hl7Msg.HL7Status = HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
                hl7Msg.MsgText = messageHL7.ToString();
                hl7Msg.PatNum = pat.PatNum;
                HL7Msgs.Insert(hl7Msg);
#if DEBUG
                MessageBox.Show(this, messageHL7.ToString());
#endif
            }
            else
            {
                //Note: AptCur.ProvNum may not reflect the selected provider in comboProv. This is still the Provider that the appointment was last saved with.
                Bridges.ECW.SendHL7(AptCur.AptNum, AptCur.ProvNum, pat, pdfDataStr, "progressnotes", true, null);//just pdf, passing null proc list
            }
            MsgBox.Show(this, "Notes PDF sent.");
        }

        ///<summary>Creates a new .pdf file containing all of the procedures attached to this appointment and 
        ///returns the contents of the .pdf file as a base64 encoded string.</summary>
        private string GenerateProceduresIntoPdf()
        {
            MigraDoc.DocumentObjectModel.Document doc = new MigraDoc.DocumentObjectModel.Document();
            doc.DefaultPageSetup.PageWidth = Unit.FromInch(8.5);
            doc.DefaultPageSetup.PageHeight = Unit.FromInch(11);
            doc.DefaultPageSetup.TopMargin = Unit.FromInch(.5);
            doc.DefaultPageSetup.LeftMargin = Unit.FromInch(.5);
            doc.DefaultPageSetup.RightMargin = Unit.FromInch(.5);
            MigraDoc.DocumentObjectModel.Section section = doc.AddSection();
            MigraDoc.DocumentObjectModel.Font headingFont = MigraDocHelper.CreateFont(13, true);
            MigraDoc.DocumentObjectModel.Font bodyFontx = MigraDocHelper.CreateFont(9, false);
            string text;
            //Heading---------------------------------------------------------------------------------------------------------------
            #region printHeading
            Paragraph par = section.AddParagraph();
            ParagraphFormat parformat = new ParagraphFormat();
            parformat.Alignment = ParagraphAlignment.Center;
            parformat.Font = MigraDocHelper.CreateFont(10, true);
            par.Format = parformat;
            text = Lan.g(this, "procedures").ToUpper();
            par.AddFormattedText(text, headingFont);
            par.AddLineBreak();
            text = pat.GetNameFLFormal();
            par.AddFormattedText(text, headingFont);
            par.AddLineBreak();
            text = DateTime.Now.ToShortDateString();
            par.AddFormattedText(text, headingFont);
            par.AddLineBreak();
            par.AddLineBreak();
            #endregion
            //Procedure List--------------------------------------------------------------------------------------------------------
            #region Procedure List
            ODGrid gridProg = new ODGrid();
            this.Controls.Add(gridProg);//Only added temporarily so that printing will work. Removed at end with Dispose().
            gridProg.BeginUpdate();
            gridProg.Columns.Clear();
            ODGridColumn col;
            List<DisplayField> fields = DisplayFields.GetDefaultList(DisplayFieldCategory.None);
            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].InternalName == "User" || fields[i].InternalName == "Signed")
                {
                    continue;
                }
                if (fields[i].Description == "")
                {
                    col = new ODGridColumn(fields[i].InternalName, fields[i].ColumnWidth);
                }
                else
                {
                    col = new ODGridColumn(fields[i].Description, fields[i].ColumnWidth);
                }
                if (fields[i].InternalName == "Amount")
                {
                    col.TextAlign = HorizontalAlignment.Right;
                }
                if (fields[i].InternalName == "Proc Code")
                {
                    col.TextAlign = HorizontalAlignment.Center;
                }
                gridProg.Columns.Add(col);
            }
            gridProg.NoteSpanStart = 2;
            gridProg.NoteSpanStop = 7;
            gridProg.Rows.Clear();
            List<Procedure> procsForDay = Procedures.GetProcsForPatByDate(AptCur.PatNum, AptCur.AptDateTime);
            List<Definition> listProgNoteColorDefs = Definition.GetByCategory(DefinitionCategory.ProgNoteColors);;
            List<Definition> listMiscColorDefs = Definition.GetByCategory(DefinitionCategory.MiscColors);;
            for (int i = 0; i < procsForDay.Count; i++)
            {
                Procedure proc = procsForDay[i];
                ProcedureCode procCode = ProcedureCodes.GetProcCode(proc.CodeNum);
                Provider prov = _listProvidersAll.First(x => x.Id == proc.ProvNum);
                User usr = User.GetById(proc.UserNum);
                ODGridRow row = new ODGridRow();
                row.ColorLborder = System.Drawing.Color.Black;
                for (int f = 0; f < fields.Count; f++)
                {
                    switch (fields[f].InternalName)
                    {
                        case "Date":
                            row.Cells.Add(proc.ProcDate.Date.ToShortDateString());
                            break;
                        case "Time":
                            row.Cells.Add(proc.ProcDate.ToString("h:mm") + proc.ProcDate.ToString("%t").ToLower());
                            break;
                        case "Th":
                            row.Cells.Add(Tooth.GetToothLabel(proc.ToothNum));
                            break;
                        case "Surf":
                            row.Cells.Add(proc.Surf);
                            break;
                        case "Dx":
                            row.Cells.Add(proc.Dx.ToString());
                            break;
                        case "Description":
                            row.Cells.Add((procCode.LaymanTerm != "") ? procCode.LaymanTerm : procCode.Descript);
                            break;
                        case "Stat":
                            if (ProcMultiVisits.IsProcInProcess(proc.ProcNum))
                            {
                                row.Cells.Add(Lan.g("enumProcStat", ProcStatExt.InProcess));
                            }
                            else
                            {
                                row.Cells.Add(Lans.g("enumProcStat", proc.ProcStatus.ToString()));
                            }
                            break;
                        case "Prov":
                            row.Cells.Add(prov.Abbr.Left(5));
                            break;
                        case "Amount":
                            row.Cells.Add(proc.ProcFee.ToString("F"));
                            break;
                        case "Proc Code":
                            if (procCode.ProcCode.Length > 5 && procCode.ProcCode.StartsWith("D"))
                            {
                                row.Cells.Add(procCode.ProcCode.Substring(0, 5));//Remove suffix from all D codes.
                            }
                            else
                            {
                                row.Cells.Add(procCode.ProcCode);
                            }
                            break;
                        case "User":
                            row.Cells.Add(usr != null ? usr.UserName : "");
                            break;
                    }
                }
                row.Note = proc.Note;
                //Row text color.
                switch (proc.ProcStatus)
                {
                    case ProcStat.TP:
                        row.ColorText = listProgNoteColorDefs[0].Color;
                        break;
                    case ProcStat.C:
                        row.ColorText = listProgNoteColorDefs[1].Color;
                        break;
                    case ProcStat.EC:
                        row.ColorText = listProgNoteColorDefs[2].Color;
                        break;
                    case ProcStat.EO:
                        row.ColorText = listProgNoteColorDefs[3].Color;
                        break;
                    case ProcStat.R:
                        row.ColorText = listProgNoteColorDefs[4].Color;
                        break;
                    case ProcStat.D:
                        row.ColorText = System.Drawing.Color.Black;
                        break;
                    case ProcStat.Cn:
                        row.ColorText = listProgNoteColorDefs[22].Color;
                        break;
                }
                row.BackColor = System.Drawing.Color.White;
                if (proc.ProcDate.Date == DateTime.Today)
                {
                    row.BackColor = listMiscColorDefs[6].Color;
                }
                gridProg.Rows.Add(row);
            }
            MigraDocHelper.DrawGrid(section, gridProg);
            #endregion
            MigraDoc.Rendering.PdfDocumentRenderer pdfRenderer = new MigraDoc.Rendering.PdfDocumentRenderer(true, PdfFontEmbedding.Always);
            pdfRenderer.Document = doc;
            pdfRenderer.RenderDocument();
            MemoryStream ms = new MemoryStream();
            pdfRenderer.PdfDocument.Save(ms);
            byte[] pdfBytes = ms.GetBuffer();
            //#region Remove when testing is complete.
            //string tempFilePath=Path.GetTempFileName();
            //File.WriteAllBytes(tempFilePath,pdfBytes);
            //#endregion
            string pdfDataStr = Convert.ToBase64String(pdfBytes);
            ms.Dispose();
            return pdfDataStr;
        }

        private void butComplete_Click(object sender, EventArgs e)
        {
            //It is OK to let the user click the OK button as long as AptCur.AptNum is NOT used prior to UpdateListAndDB().
            //if(_isInsertRequired) {
            //	MsgBox.Show(this,"Please click OK to create this appointment before taking this action.");
            //	return;
            //}
            //This is only used with eCW HL7 interface.
            DateTime datePrevious = AptCur.DateTStamp;
            if (_isEcwHL7Sent)
            {
                if (!Security.IsAuthorized(Permissions.EcwAppointmentRevise))
                {
                    return;
                }
                MsgBox.Show(this, "Any changes that you make will not be sent to eCW.  You will also have to make the same changes in eCW.");
                //revise is only clickable if user has permission
                butOK.Enabled = true;
                gridProc.Enabled = true;
                quickAddListBox.Enabled = true;
                procedureAddButton.Enabled = true;
                procedureDeleteButton.Enabled = true;
                return;
            }
            List<Procedure> listProcsForAppt = gridProc.SelectedIndices.OfType<int>().Select(x => (Procedure)gridProc.Rows[x].Tag).ToList();
            string duplicateProcs = ProcedureL.ProcsContainDuplicates(listProcsForAppt);
            if (duplicateProcs != "")
            {
                MessageBox.Show(duplicateProcs);
                return;
            }
            //if (ProgramProperties.GetPropVal(ProgramName.eClinicalWorks, "ProcNotesNoIncomplete") == "1")
            //{
            //    if (listProcsForAppt.Any(x => x.Note != null && x.Note.Contains("\"\"")))
            //    {
            //        MsgBox.Show(this, "This appointment cannot be sent because there are incomplete procedure notes.");
            //        return;
            //    }
            //}
            //if (ProgramProperties.GetPropVal(ProgramName.eClinicalWorks, "ProcRequireSignature") == "1")
            //{
            //    if (listProcsForAppt.Any(x => !string.IsNullOrEmpty(x.Note) && string.IsNullOrEmpty(x.Signature)))
            //    {
            //        MsgBox.Show(this, "This appointment cannot be sent because there are unsigned procedure notes.");
            //        return;
            //    }
            //}
            //user can only get this far if aptNum matches visit num previously passed in by eCW.
            if (!MsgBox.Show(this, MsgBoxButtons.OKCancel, "Send attached procedures to eClinicalWorks and exit?"))
            {
                return;
            }
            statusComboBox.SelectedIndex = 1;//Set the appointment status to complete. This will trigger the procedures to be completed in UpdateToDB() as well.
            if (!UpdateListAndDB())
            {
                return;
            }
            listProcsForAppt = Procedures.GetProcsForSingle(AptCur.AptNum, AptCur.AptStatus == ApptStatus.Planned);
            //Send DFT to eCW containing the attached procedures for this appointment in a .pdf file.				
            string pdfDataStr = GenerateProceduresIntoPdf();
            if (HL7Defs.IsExistingHL7Enabled())
            {
                //MessageConstructor.GenerateDFT(procs,EventTypeHL7.P03,pat,Patients.GetPat(pat.Guarantor),AptCur.AptNum,"progressnotes",pdfDataStr);
                MessageHL7 messageHL7 = MessageConstructor.GenerateDFT(listProcsForAppt, EventTypeHL7.P03, pat, Patients.GetPat(pat.Guarantor), AptCur.AptNum,
                    "progressnotes", pdfDataStr);
                if (messageHL7 == null)
                {
                    MsgBox.Show(this, "There is no DFT message type defined for the enabled HL7 definition.");
                    return;
                }
                HL7Msg hl7Msg = new HL7Msg();
                hl7Msg.AptNum = AptCur.AptNum;
                hl7Msg.HL7Status = HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
                hl7Msg.MsgText = messageHL7.ToString();
                hl7Msg.PatNum = pat.PatNum;
                HL7ProcAttach hl7ProcAttach = new HL7ProcAttach();
                hl7ProcAttach.HL7MsgNum = HL7Msgs.Insert(hl7Msg);
                foreach (Procedure proc in listProcsForAppt)
                {
                    hl7ProcAttach.ProcNum = proc.ProcNum;
                    HL7ProcAttaches.Insert(hl7ProcAttach);
                }
            }
            else
            {
                Bridges.ECW.SendHL7(AptCur.AptNum, AptCur.ProvNum, pat, pdfDataStr, "progressnotes", false, listProcsForAppt);
            }
            CloseOD = true;
            if (IsNew)
            {
                SecurityLog.Write(pat.PatNum, SecurityLogEvents.AppointmentCreate,
                AptCur.AptDateTime.ToString() + ", " + AptCur.ProcDescript,
                AptCur.AptNum, datePrevious);
            }
            DialogResult = DialogResult.OK;
            if (!this.Modal)
            {
                Close();
            }
        }

        void auditButton_Click(object sender, EventArgs e)
        {
            if (insertRequired)
            {
                MessageBox.Show(
                    "Please click OK to create this appointment before taking this action.", 
                    "Appointment", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            var permissions = new List<string>
            {
                Permissions.AppointmentCreate,
                Permissions.AppointmentEdit,
                Permissions.AppointmentMove,
                Permissions.AppointmentCompleteEdit,
                Permissions.ApptConfirmStatusEdit
            };

            using (var formAuditOneType = new FormAuditOneType(pat.PatNum, permissions, "Audit Trail for Appointment", AptCur.AptNum))
            {
                formAuditOneType.ShowDialog();
            }
        }

        void taskButton_Click(object sender, EventArgs e)
        {
            if (insertRequired && !UpdateListAndDB(false)) return;

            using (var formTaskListSelect = new FormTaskListSelect(TaskObjectType.Appointment))
            {
                formTaskListSelect.Text = "Add Task - " + formTaskListSelect.Text;
                if (formTaskListSelect.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var task = new Task
                {
                    TaskListNum = -1 // Don't show it in any list yet.
                };
                Tasks.Insert(task);

                var taskOld = task.Copy();
                task.KeyNum = AptCur.AptNum;
                task.ObjectType = TaskObjectType.Appointment;
                task.TaskListNum = formTaskListSelect.ListSelectedLists[0];
                task.UserNum = Security.CurrentUser.Id;

                using (var formTaskEdit = new FormTaskEdit(task, taskOld))
                {
                    formTaskEdit.IsNew = true;
                    formTaskEdit.ShowDialog();
                }
            }
        }

        private void butPin_Click(object sender, System.EventArgs e)
        {
            if (AptCur.AptStatus.In(ApptStatus.UnschedList, ApptStatus.Planned) && pat.PatStatus.In(PatientStatus.Archived, PatientStatus.Deceased))
            {
                MsgBox.Show(this, "Appointments cannot be scheduled for " + pat.PatStatus.ToString().ToLower() + " patients.");
                return;
            }
            if (!UpdateListAndDB())
            {
                return;
            }
            PinClicked = true;
            DialogResult = DialogResult.OK;
            if (!this.Modal)
            {
                Close();
            }
        }

        ///<summary>Returns true if the appointment type was successfully changed, returns false if the user decided to cancel out of doing so.</summary>
        private bool AptTypeHelper()
        {
            if (comboApptType.SelectedIndex == 0)
            {//'None' is selected so maintain grid selections.
                return true;
            }
            if (AptCur.AptStatus.In(ApptStatus.PtNote, ApptStatus.PtNoteCompleted))
            {
                return true;//Patient notes can't have procedures associated to them.
            }
            AppointmentType aptTypeCur = _listAppointmentType[comboApptType.SelectedIndex - 1];
            List<ProcedureCode> listAptTypeProcs = ProcedureCodes.GetFromCommaDelimitedList(aptTypeCur.ProcedureCodes);
            if (listAptTypeProcs.Count > 0)
            {//AppointmentType is associated to procs.
                List<Procedure> listSelectedProcs = gridProc.Rows.Cast<ODGridRow>()
                    .Where(x => gridProc.SelectedIndices.Contains(gridProc.Rows.IndexOf(x)))
                    .Select(x => ((Procedure)x.Tag)).ToList();
                List<long> listProcCodeNumsToDetach = listSelectedProcs.Select(y => y.CodeNum).ToList()
                .Except(listAptTypeProcs.Select(x => x.CodeNum).ToList()).ToList();
                //if there are procedures that would get detached
                //and if they have the preference AppointmentTypeWarning on,
                //Display the warning
                if (listProcCodeNumsToDetach.Count > 0 && Preference.GetBool(PreferenceName.AppointmentTypeShowWarning))
                {
                    if (!MsgBox.Show(this, MsgBoxButtons.OKCancel, "Selecting this appointment type will dissociate the current procedures from this "
                        + "appointment and attach the procedures defined for this appointment type.  Do you want to continue?"))
                    {
                        return false;
                    }
                }
                Appointments.ApptTypeMissingProcHelper(AptCur, aptTypeCur, _listProcsForAppt, pat, true, _listPatPlans, SubList, PlanList, _benefitList);
                FillProcedures();
                //Since we have detached and attached all pertinent procs by this point it is safe to just use the PlannedAptNum or AptNum.
                gridProc.SetSelected(false);
                foreach (ProcedureCode procCodeCur in listAptTypeProcs)
                {
                    for (int i = 0; i < gridProc.Rows.Count; i++)
                    {
                        Procedure rowProc = (Procedure)gridProc.Rows[i].Tag;
                        if (rowProc.CodeNum == procCodeCur.CodeNum
                            //if the procedure code already exists in the grid and it's not attached to another appointment or planned appointment
                            && (_isPlanned && (rowProc.PlannedAptNum == 0 || rowProc.PlannedAptNum == AptCur.AptNum)
                                || (!_isPlanned && (rowProc.AptNum == 0 || rowProc.AptNum == AptCur.AptNum)))
                            //The row is not already selected. This is necessary so that Apt Types with two of the same procs will select both procs.
                            && !gridProc.SelectedIndices.Contains(i))
                        {
                            gridProc.SetSelected(i, true); //set procedures selected in the grid.
                            break;
                        }
                    }
                }
            }
            butColor.BackColor = aptTypeCur.Color;
            if (aptTypeCur.Pattern != null && aptTypeCur.Pattern != "")
            {
                timeBar.Pattern = aptTypeCur.Pattern;
            }

            //calculate the new time pattern.
            if (aptTypeCur != null && listAptTypeProcs != null)
            {
                //Has Procs, but not time.
                if (aptTypeCur.Pattern == "" && listAptTypeProcs.Count > 0)
                {
                    //Calculate and Fill
                    CalculateTime(true);
                }
                //Has fixed time
                else if (aptTypeCur.Pattern != "")
                {
                    AptCur.Pattern = aptTypeCur.Pattern;
                }
                //No Procs, No time.
                else
                {
                    //do nothing to the time pattern
                }
            }
            return true;
        }

        ///<summary>Only catches user changes, not programatic changes. For instance this does not fire when loading the form.</summary>
        private void comboApptType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!AptTypeHelper())
            {
                comboApptType.SelectedIndex = _aptTypeIndex;
                return;
            }
            _aptTypeIndex = comboApptType.SelectedIndex;
        }

        private void comboConfirmed_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (Preference.GetLong(PreferenceName.AppointmentTimeArrivedTrigger) != 0 //Using appointmentTimeArrivedTrigger preference
                && _listApptConfirmedDefs[comboConfirmed.SelectedIndex].Id == Preference.GetLong(PreferenceName.AppointmentTimeArrivedTrigger) //selected index matches pref
                && String.IsNullOrWhiteSpace(textTimeArrived.Text))//time not already set 
            {
                textTimeArrived.Text = DateTime.Now.ToShortTimeString();
            }
            if (Preference.GetLong(PreferenceName.AppointmentTimeSeatedTrigger) != 0 //Using AppointmentTimeSeatedTrigger preference
                && _listApptConfirmedDefs[comboConfirmed.SelectedIndex].Id == Preference.GetLong(PreferenceName.AppointmentTimeSeatedTrigger) //selected index matches pref
                && String.IsNullOrWhiteSpace(textTimeSeated.Text))//time not already set 
            {
                textTimeSeated.Text = DateTime.Now.ToShortTimeString();
            }
            if (Preference.GetLong(PreferenceName.AppointmentTimeDismissedTrigger) != 0 //Using AppointmentTimeDismissedTrigger preference
                && _listApptConfirmedDefs[comboConfirmed.SelectedIndex].Id == Preference.GetLong(PreferenceName.AppointmentTimeDismissedTrigger) //selected index matches pref
                && String.IsNullOrWhiteSpace(textTimeDismissed.Text))//time not already set 
            {
                textTimeDismissed.Text = DateTime.Now.ToShortTimeString();
            }
        }

        void statusComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex != indexStatusBroken)
            {
                return;
            }

            if (!AppointmentL.HasBrokenApptProcs()) return;

            // Patient note appointment types can't have a aptstatus of broken.
            if (AptCur.AptStatus == ApptStatus.PtNoteCompleted || 
                AptCur.AptStatus == ApptStatus.PtNote)
            {
                return;
            }

            using (var formApptBreak = new FormApptBreak(AptCur))
            {
                if (formApptBreak.ShowDialog() != DialogResult.OK)
                {
                    statusComboBox.SelectedIndex = (int)AptCur.AptStatus - 1; // Sets status back to on load selection.
                    _formApptBreakSelection = ApptBreakSelection.None;
                    _procCodeBroken = null;
                    return;
                }

                _formApptBreakSelection = formApptBreak.Selection;
                _procCodeBroken = formApptBreak.ProcedureCode;
            }
        }




        private void checkASAP_CheckedChanged(object sender, EventArgs e)
        {
            if (checkASAP.Checked)
            {
                checkASAP.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                checkASAP.ForeColor = SystemColors.ControlText;
            }
        }

        private bool CheckFrequencies()
        {
            List<Procedure> listProcsForFrequency = new List<Procedure>();
            foreach (int index in gridProc.SelectedIndices)
            {
                Procedure proc = ((Procedure)gridProc.Rows[index].Tag).Copy();
                if (proc.ProcStatus == ProcStat.TP)
                {
                    listProcsForFrequency.Add(proc);
                }
            }
            if (listProcsForFrequency.Count > 0)
            {
                string frequencyConflicts = "";
                try
                {
                    frequencyConflicts = Procedures.CheckFrequency(listProcsForFrequency, pat.PatNum, AptCur.AptDateTime);
                }
                catch (Exception e)
                {
                    MessageBox.Show(Lan.g(this, "There was an error checking frequencies."
                        + "  Disable the Insurance Frequency Checking feature or try to fix the following error:")
                        + "\r\n" + e.Message);
                    return false;
                }
                if (frequencyConflicts != "" && MessageBox.Show(Lan.g(this, "This appointment will cause frequency conflicts for the following procedures")
                    + ":\r\n" + frequencyConflicts + "\r\n" + Lan.g(this, "Do you want to continue?"), "", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }
            }
            return true;
        }

        private void butDelete_Click(object sender, EventArgs e)
        {
            OnDelete_Click();
        }

        ///<summary>Deletes the appointment, creating appropriate logs and commlogs.  Pass in </summary>
        private void OnDelete_Click(bool isSkipDeletePrompt = false)
        {
            DateTime datePrevious = AptCur.DateTStamp;
            if (AptCur.AptStatus == ApptStatus.PtNote || AptCur.AptStatus == ApptStatus.PtNoteCompleted)
            {
                if (!isSkipDeletePrompt && !MsgBox.Show(this, true, "Delete Patient Note?"))
                {
                    return;
                }
                if (appointmentNoteTextBox.Text != "")
                {
                    if (MessageBox.Show(Commlogs.GetDeleteApptCommlogMessage(appointmentNoteTextBox.Text, AptCur.AptStatus), "Question...", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Commlog CommlogCur = new Commlog();
                        CommlogCur.PatNum = AptCur.PatNum;
                        CommlogCur.CommDateTime = DateTime.Now;
                        CommlogCur.CommType = Commlogs.GetTypeAuto(CommItemTypeAuto.APPT);
                        CommlogCur.Note = "Deleted Pt NOTE from schedule, saved copy: ";
                        CommlogCur.Note += appointmentNoteTextBox.Text;
                        CommlogCur.UserNum = Security.CurrentUser.Id;
                        //there is no dialog here because it is just a simple entry
                        Commlogs.Insert(CommlogCur);
                    }
                }
            }
            else
            {//ordinary appointment
                if (!isSkipDeletePrompt && MessageBox.Show(Lan.g(this, "Delete appointment?"), "", MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
                    return;
                }
                if (appointmentNoteTextBox.Text != "")
                {
                    if (MessageBox.Show(Commlogs.GetDeleteApptCommlogMessage(appointmentNoteTextBox.Text, AptCur.AptStatus), "Question...", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Commlog CommlogCur = new Commlog();
                        CommlogCur.PatNum = AptCur.PatNum;
                        CommlogCur.CommDateTime = DateTime.Now;
                        CommlogCur.CommType = Commlogs.GetTypeAuto(CommItemTypeAuto.APPT);
                        CommlogCur.Note = "Deleted Appt. & saved note: ";
                        if (AptCur.ProcDescript != "")
                        {
                            CommlogCur.Note += AptCur.ProcDescript + ": ";
                        }
                        CommlogCur.Note += appointmentNoteTextBox.Text;
                        CommlogCur.UserNum = Security.CurrentUser.Id;
                        //there is no dialog here because it is just a simple entry
                        Commlogs.Insert(CommlogCur);
                    }
                }
                //If there is an existing HL7 def enabled, send a SIU message if there is an outbound SIU message defined
                if (HL7Defs.IsExistingHL7Enabled())
                {
                    //S17 - Appt Deletion event
                    MessageHL7 messageHL7 = MessageConstructor.GenerateSIU(pat, fam.GetPatient(pat.Guarantor), EventTypeHL7.S17, AptCur);
                    //Will be null if there is no outbound SIU message defined, so do nothing
                    if (messageHL7 != null)
                    {
                        HL7Msg hl7Msg = new HL7Msg();
                        hl7Msg.AptNum = AptCur.AptNum;
                        hl7Msg.HL7Status = HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
                        hl7Msg.MsgText = messageHL7.ToString();
                        hl7Msg.PatNum = pat.PatNum;
                        HL7Msgs.Insert(hl7Msg);
#if DEBUG
                        MessageBox.Show(this, messageHL7.ToString());
#endif
                    }
                }
            }
            _listAppointments.RemoveAll(x => x.AptNum == AptCur.AptNum);
            if (AptOld.AptStatus != ApptStatus.Complete)
            { //seperate log entry for completed appointments
                SecurityLog.Write(pat.PatNum,
                    SecurityLogEvents.AppointmentEdited, "Delete for date/time: " + AptCur.AptDateTime.ToString(),
                    AptCur.AptNum, datePrevious);
            }
            else
            {
                SecurityLog.Write(pat.PatNum, SecurityLogEvents.CompletedAppointmentEdited,
                    "Delete for date/time: " + AptCur.AptDateTime.ToString(),
                    AptCur.AptNum, datePrevious);
            }
            if (IsNew)
            {
                DialogResult = DialogResult.Cancel;
                if (!this.Modal)
                {
                    Close();
                }
            }
            else
            {
                DialogResult = DialogResult.OK;
                _isDeleted = true;
                if (!this.Modal)
                {
                    Close();
                }
            }
            Plugin.Trigger(this, "FormApptEdit_Button_Delete", AptCur);
        }

        private void butOK_Click(object sender, System.EventArgs e)
        {
            DateTime datePrevious = AptCur.DateTStamp;
            if (_selectedProvNum == 0)
            {
                MsgBox.Show(this, "Please select a provider.");
                return;
            }
            if (AptOld.AptStatus != ApptStatus.UnschedList && AptCur.AptStatus == ApptStatus.UnschedList)
            {
                //Extra log entry if the appt was sent to the unscheduled list
                string perm = SecurityLogEvents.AppointmentMove;
                if (AptOld.AptStatus == ApptStatus.Complete)
                {
                    perm = SecurityLogEvents.CompletedAppointmentEdited;
                }
                SecurityLog.Write(AptCur.PatNum, perm, AptCur.ProcDescript + ", " + AptCur.AptDateTime.ToString()
                    + ", Sent to Unscheduled List", AptCur.AptNum, datePrevious);
            }
            #region Validate Apt Start and End

            string pattern = timeBar.Pattern;


            int minutes = pattern.Length * 5;
            //compare beginning of new appointment against end to see if they fall on different days
            if (AptCur.AptDateTime.Day != AptCur.AptDateTime.AddMinutes(minutes).Day)
            {
                MsgBox.Show(this, "You cannot have an appointment that starts and ends on different days.");
                return;
            }
            #endregion
            if (!UpdateListAndDB(true, true, true))
            {
                return;
            }

            Plugin.Trigger(this, "FormApptEdit_OK", AptCur, AptOld, pat);
            DialogResult = DialogResult.OK;
            if (!this.Modal)
            {
                Close();
            }
        }

        private void FormApptEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AptCur == null)
            {//Could not find AptCur in the Db on load.
                return;
            }
            //Do not use pat.PatNum here.  Use AptCur.PatNum instead.  Pat will be null in the case that the user does not have the appt create permission.
            DateTime datePrevious = AptCur.DateTStamp;
            if (DialogResult != DialogResult.OK)
            {
                if (AptCur.AptStatus == ApptStatus.Complete)
                {
                    //This is a completed appointment and we need to warn the user if they are trying to leave the window and need to detach procs first.
                    foreach (ODGridRow row in gridProc.Rows)
                    {
                        bool attached = false;
                        if (AptCur.AptStatus == ApptStatus.Planned && ((Procedure)row.Tag).PlannedAptNum == AptCur.AptNum)
                        {
                            attached = true;
                        }
                        else if (((Procedure)row.Tag).AptNum == AptCur.AptNum)
                        {
                            attached = true;
                        }
                        if (((Procedure)row.Tag).ProcStatus != ProcStat.TP || !attached)
                        {
                            continue;
                        }
                        if (!Security.IsAuthorized(Permissions.AppointmentCompleteEdit, true))
                        {
                            continue;
                        }
                        MsgBox.Show(this, "Detach treatment planned procedures or click OK in the appointment edit window to set them complete.");
                        e.Cancel = true;
                        return;
                    }
                }
                if (IsNew)
                {
                    SecurityLog.Write(AptCur.PatNum, SecurityLogEvents.AppointmentEdited,
                        "Create cancel for date/time: " + AptCur.AptDateTime.ToString(),
                        AptCur.AptNum, datePrevious);
                    //If cancel was pressed we want to un-do any changes to other appointments that were done.
                    _listAppointments = Appointments.GetAppointmentsForProcs(_listProcsForAppt);
                    //Add the current appointment if it is not in this list so it can get properly deleted by the sync later.
                    if (!_listAppointments.Exists(x => x.AptNum == AptCur.AptNum))
                    {
                        _listAppointments.Add(AptCur);
                    }
                    //We need to add this current appointment to the list of old appointments so we run the Appointments.Delete fucntion on it
                    //This will remove any procedure connections that we created while in this window.
                    _listAppointmentsOld = _listAppointments.Select(x => x.Copy()).ToList();
                    //Now we also have to remove the appointment that was pre-inserted and is in this list as well so it is deleted on sync.
                    _listAppointments.RemoveAll(x => x.AptNum == AptCur.AptNum);
                }
                else
                {  //User clicked cancel (or X button) on an existing appt
                    AptCur = AptOld.Copy();  //We do not want to save any other changes made in this form.
                    if (AptCur.AptStatus == ApptStatus.Scheduled && Preference.GetBool(PreferenceName.InsChecksFrequency) && !CheckFrequencies())
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            else
            {//DialogResult==DialogResult.OK (User clicked OK or Delete)
             //Note that Procedures.Sync is never used.  This is intentional.  In order to properly use procedure.Sync logic in this form we would
             //need to enhance ProcEdit and all its possible child forms to also not insert into DB until OK is clicked.  This would be a massive undertaking
             //and as such we just immediately push changes to DB.
                if (AptCur.AptStatus == ApptStatus.Scheduled && !_isDeleted && Preference.GetBool(PreferenceName.InsChecksFrequency) && !CheckFrequencies())
                {
                    e.Cancel = true;
                    return;
                }
                if (AptCur.AptStatus == ApptStatus.Scheduled)
                {
                    //find all procs that are currently attached to the appt that weren't when the form opened
                    List<string> listProcCodes = _listProcsForAppt.FindAll(x => x.AptNum == AptCur.AptNum && !_listProcNumsAttachedStart.Contains(x.ProcNum))
                        .Select(x => ProcedureCodes.GetStringProcCode(x.CodeNum)).Distinct().ToList();//get list of string proc codes
                    AutomationL.Trigger(AutomationTrigger.ScheduleProcedure, listProcCodes, AptCur.PatNum);
                }
            }
            //Sync detaches any attached procedures within Appointments.Delete() but doesn't create any ApptComm items.
            if (Appointments.Sync(_listAppointments, _listAppointmentsOld, AptCur.PatNum))
            {
                AppointmentEvent.Fire(ODEventType.AppointmentEdited, AptCur);
            }
            //Synch the recalls for this patient.  This is necessary in case the date of the appointment has change or has been deleted entirely.
            Recalls.Synch(AptCur.PatNum);
            Recalls.SynchScheduledApptFull(AptCur.PatNum);
        }


        void TimeBar_SelectedTimeChanged(object sender, EventArgs e) => textTime.Text = timeBar.SelectedTime.ToString();
    }
}