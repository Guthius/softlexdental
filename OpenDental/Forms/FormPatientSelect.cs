using CodeBase;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormPatientSelect : FormBase
    {
        readonly Patients Patients;

        /// <summary>Use when you want to specify a patient without changing the current patient.  If true, then the Add Patient button will not be visible.</summary>
        public bool SelectionModeOnly;

        ///<summary>When closing the form, this indicates whether a new patient was added from within this form.</summary>
        public bool NewPatientAdded;
        ///<summary>Only used when double clicking blank area in Appts. Sets this value to the currently selected pt.  That patient will come up on the screen already selected and user just has to click OK. Or they can select a different pt or add a new pt.  If 0, then no initial patient is selected.</summary>
        public long InitialPatNum;
        private DataTable _DataTablePats;

        ///<summary>When closing the form, this will hold the value of the newly selected PatNum.</summary>
        public long SelectedPatNum;

        private List<DisplayField> _ListDisplayFields;

        ///<summary>List of all the clinics this userod has access to.  When comboClinic.SelectedIndex=0 it refers to all clinics in this list.  Otherwise their selected clinic will always be _listClinics[comboClinic.SelectedIndex-1].</summary>
        private List<Clinic> _listClinics;
        
        ///<summary>Set to true if constructor passed in patient object to prefill text boxes.  Used to make sure fillGrid is not called 
        ///before FormSelectPatient_Load.</summary>
        private bool _isPreFillLoad = false;
        
        ///<summary>If set, initial patient list will be set to these patients.</summary>
        public List<long> ExplicitPatNums;

        private ODThread _fillGridThread = null;
        private DateTime _dateTimeLastSearch;
        private DateTime _dateTimeLastRequest;
        private Process _processOnScreenKeyboard = null;

        public FormPatientSelect() : this(null)
        {
        }

        public FormPatientSelect(Patient patient)
        {
            InitializeComponent();

            Patients = new Patients();

            if (patient != null)
            {
                PreFillSearchBoxes(patient);
            }
        }

        private List<Site> _listSites;
        private List<Def> _listBillingTypeDefs;


        public Patient PreselectedPatient { get; private set; }


        public void PreselectPatient(Patient value)
        {
            PreselectedPatient = value;
            lastNameTextBox.Text = value.LName;
            firstNameTextBox.Text = value.FName;
            cityTextBox.Text = value.City;
            butSearch_Click(this, EventArgs.Empty);
        }

        ///<summary></summary>
        public void FormSelectPatient_Load(object sender, System.EventArgs e)
        {
            if (!Preferences.GetBool(PrefName.DockPhonePanelShow))
            {
                countryLabel.Visible = false;
                countryTextBox.Visible = false;
            }

            if (SelectionModeOnly)
            {
                addPatientButton.Visible = false;
                addManyButton.Visible = false;
            }
            //Cannot add new patients from OD select patient interface.  Patient must be added from HL7 message.
            if (HL7Defs.IsExistingHL7Enabled())
            {
                HL7Def def = HL7Defs.GetOneDeepEnabled();
                if (def.ShowDemographics != HL7ShowDemographics.ChangeAndAdd)
                {
                    addPatientButton.Visible = false;
                    addManyButton.Visible = false;
                }
            }
            else
            {
                if (Programs.UsingEcwTightOrFullMode())
                {
                    addPatientButton.Visible = false;
                    addManyButton.Visible = false;
                }
            }
            billingTypeComboBox.Items.Add(Lan.g(this, "All"));
            billingTypeComboBox.SelectedIndex = 0;
            _listBillingTypeDefs = Defs.GetDefsForCategory(DefCat.BillingTypes, true);
            for (int i = 0; i < _listBillingTypeDefs.Count; i++)
            {
                billingTypeComboBox.Items.Add(_listBillingTypeDefs[i].ItemName);
            }
            if (Preferences.GetBool(PrefName.EasyHidePublicHealth))
            {
                siteComboBox.Visible = false;
                siteLabel.Visible = false;
            }
            else
            {
                siteComboBox.Items.Add(Lan.g(this, "All"));
                siteComboBox.SelectedIndex = 0;
                _listSites = Sites.GetDeepCopy();
                for (int i = 0; i < _listSites.Count; i++)
                {
                    siteComboBox.Items.Add(_listSites[i].Description);
                }
            }
            if (!Preferences.HasClinicsEnabled)
            {
                clinicLabel.Visible = false;
                clinicComboBox.Visible = false;
            }
            else
            {
                //if the current user is restricted to a clinic (or in the future many clinics), All will refer to only those clinics the user has access to. May only be one clinic.
                clinicComboBox.Items.Add(new ODBoxItem<Clinic>(Lan.g(this, "All"), new Clinic()));
                clinicComboBox.SelectedIndex = 0;
                if (Security.IsAuthorized(Permissions.UnrestrictedSearch, true))
                {//user has permission to search all clinics though restricted. 
                    _listClinics = Clinics.GetDeepCopy();
                }
                else
                {//only authorized to search restricted clinics.
                    _listClinics = Clinics.GetAllForUserod(Security.CurUser);//could be only one if the user is restricted
                }
                for (int i = 0; i < _listClinics.Count; i++)
                {
                    if (_listClinics[i].IsHidden)
                    {
                        continue;//Don't add hidden clinics to the combo
                    }
                    clinicComboBox.Items.Add(new ODBoxItem<Clinic>(_listClinics[i].Abbr, _listClinics[i]));
                    if (Clinics.ClinicNum == _listClinics[i].ClinicNum)
                    {
                        clinicComboBox.SelectedIndex = clinicComboBox.Items.Count - 1;
                    }
                }
            }
            FillSearchOption();
            SetGridCols();
            if (ExplicitPatNums != null && ExplicitPatNums.Count > 0)
            {
                FillGrid(false, ExplicitPatNums);
                return;
            }
            if (InitialPatNum != 0)
            {
                Patient iPatient = Patients.GetLim(InitialPatNum);
                lastNameTextBox.Text = iPatient.LName;
                FillGrid(false);
                /*if(grid2.CurrentRowIndex>-1){
					grid2.UnSelect(grid2.CurrentRowIndex);
				}
				for(int i=0;i<PtDataTable.Rows.Count;i++){
					if(PIn.PInt(PtDataTable.Rows[i][0].ToString())==InitialPatNum){
						grid2.CurrentRowIndex=i;
						grid2.Select(i);
						break;
					}
				}*/
                return;
            }
            //Always fillGrid if _isPreFilledLoad.  Since the first name and last name are pre-filled, the results should be minimal.
            if (checkRefresh.Checked || _isPreFillLoad)
            {
                FillGrid(true);
                _isPreFillLoad = false;
            }
            //Set the Textbox Enter Event Handler to keep track of which TextBox had focus last.  
            //This helps dictate the desired text box for input after opening up the On Screen Keyboard.
            SetAllTextBoxEnterEventListeners();
        }

        ///<summary>This used to be called all the time, now only needs to be called on load.</summary>
        private void FillSearchOption()
        {
            switch (ComputerPrefs.LocalComputer.PatSelectSearchMode)
            {
                case SearchMode.Default:
                    checkRefresh.Checked = !Preferences.GetBool(PrefName.PatientSelectUsesSearchButton);//Use global preference
                    break;
                case SearchMode.RefreshWhileTyping:
                    checkRefresh.Checked = true;
                    break;
                case SearchMode.UseSearchButton:
                default:
                    checkRefresh.Checked = false;
                    break;
            }
        }

        private void SetGridCols()
        {
            //This pattern is wrong.
            gridMain.BeginUpdate();
            gridMain.Columns.Clear();
            ODGridColumn col;
            _ListDisplayFields = DisplayFields.GetForCategory(DisplayFieldCategory.PatientSelect);
            for (int i = 0; i < _ListDisplayFields.Count; i++)
            {
                if (_ListDisplayFields[i].Description == "")
                {
                    col = new ODGridColumn(_ListDisplayFields[i].InternalName, _ListDisplayFields[i].ColumnWidth);
                }
                else
                {
                    col = new ODGridColumn(_ListDisplayFields[i].Description, _ListDisplayFields[i].ColumnWidth);
                }
                gridMain.Columns.Add(col);
            }
            gridMain.EndUpdate();
        }

        ///<summary>The pat must not be null.  Takes a partially built patient object and uses it to fill the search by textboxes.
        ///Currently only implements FName, LName, and HmPhone.</summary>
        public void PreFillSearchBoxes(Patient pat)
        {
            _isPreFillLoad = true; //Set to true to stop FillGrid from being called as a result of textChanged events
            if (pat.LName != "")
            {
                lastNameTextBox.Text = pat.LName;
            }
            if (pat.FName != "")
            {
                firstNameTextBox.Text = pat.FName;
            }
            if (pat.HmPhone != "")
            {
                phoneTextBox.Text = pat.HmPhone;
            }
        }

        private void textLName_TextChanged(object sender, System.EventArgs e)
        {
            OnDataEntered();
        }

        private void textFName_TextChanged(object sender, System.EventArgs e)
        {
            OnDataEntered();
        }

        private void textHmPhone_TextChanged(object sender, System.EventArgs e)
        {
            OnDataEntered();
        }

        private void textWkPhone_TextChanged(object sender, System.EventArgs e)
        {
            OnDataEntered();
        }

        private void textAddress_TextChanged(object sender, System.EventArgs e)
        {
            OnDataEntered();
        }

        private void textCity_TextChanged(object sender, System.EventArgs e)
        {
            OnDataEntered();
        }

        private void textState_TextChanged(object sender, System.EventArgs e)
        {
            OnDataEntered();
        }

        private void textSSN_TextChanged(object sender, System.EventArgs e)
        {
            OnDataEntered();
        }

        private void textPatNum_TextChanged(object sender, System.EventArgs e)
        {
            OnDataEntered();
        }

        private void textChartNumber_TextChanged(object sender, System.EventArgs e)
        {
            OnDataEntered();
        }

        private void textBirthdate_TextChanged(object sender, EventArgs e)
        {
            OnDataEntered();
        }

        private void textSubscriberID_TextChanged(object sender, EventArgs e)
        {
            OnDataEntered();
        }

        private void textEmail_TextChanged(object sender, EventArgs e)
        {
            OnDataEntered();
        }

        private void textCountry_TextChanged(object sender, EventArgs e)
        {
            OnDataEntered();
        }

        private void textInvoiceNumber_TextChanged(object sender, EventArgs e)
        {
            OnDataEntered();
        }

        private void textRegKey_TextChanged(object sender, EventArgs e)
        {
            OnDataEntered();
        }

        private void comboBillingType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            OnDataEntered();
        }

        private void comboSite_SelectionChangeCommitted(object sender, EventArgs e)
        {
            OnDataEntered();
        }

        private void comboClinic_SelectionChangeCommitted(object sender, EventArgs e)
        {
            OnDataEntered();
        }

        private void checkGuarantors_CheckedChanged(object sender, System.EventArgs e)
        {
            OnDataEntered();
        }

        private void checkHideInactive_CheckedChanged(object sender, System.EventArgs e)
        {
            OnDataEntered();
        }

        private void checkShowArchived_CheckedChanged(object sender, EventArgs e)
        {
            OnDataEntered();
        }

        private void checkShowMerged_CheckedChanged(object sender, EventArgs e)
        {
            OnDataEntered();
        }

        private void checkRefresh_Click(object sender, EventArgs e)
        {
            if (checkRefresh.Checked)
            {
                ComputerPrefs.LocalComputer.PatSelectSearchMode = SearchMode.RefreshWhileTyping;
                FillGrid(true);
            }
            else
            {
                ComputerPrefs.LocalComputer.PatSelectSearchMode = SearchMode.UseSearchButton;
            }
            ComputerPrefs.Update(ComputerPrefs.LocalComputer);
        }

        private void butSearch_Click(object sender, System.EventArgs e)
        {
            FillGrid(true);
        }

        private void butGetAll_Click(object sender, EventArgs e)
        {
            FillGrid(false);
        }

        private void OnDataEntered()
        {
            //Do not call FillGrid unless _isPreFillLoad=false.  Since the first name and last name are pre-filled, the results should be minimal.
            if (checkRefresh.Checked && !_isPreFillLoad)
            {
                FillGrid(true);
            }
        }

        private void FillGrid(bool doLimitOnePage, List<long> listtExplicitPatNums = null)
        {
            _dateTimeLastRequest = DateTime.Now;
            if (_fillGridThread != null)
            {
                return;
            }
            _dateTimeLastSearch = _dateTimeLastRequest;
            long billingType = 0;
            if (billingTypeComboBox.SelectedIndex != 0)
            {
                billingType = _listBillingTypeDefs[billingTypeComboBox.SelectedIndex - 1].DefNum;
            }
            long siteNum = 0;
            if (!Preferences.GetBool(PrefName.EasyHidePublicHealth) && siteComboBox.SelectedIndex != 0)
            {
                siteNum = _listSites[siteComboBox.SelectedIndex - 1].SiteNum;
            }
            DateTime birthdate = PIn.Date(birthdateTextBox.Text); //this will frequently be minval.
            string clinicNums = "";
            if (Preferences.HasClinicsEnabled)
            {
                if (clinicComboBox.SelectedIndex == 0)
                {//'All' is selected
                    clinicNums = string.Join(",", _listClinics
                        .Where(x => !x.IsHidden || checkShowArchived.Checked)//Only show hidden clinics if "Show Archived" is checked
                        .Select(x => x.ClinicNum));
                }
                else
                {
                    clinicNums = ((ODBoxItem<Clinic>)clinicComboBox.SelectedItem).Tag.ClinicNum.ToString();
                    if (checkShowArchived.Checked)
                    {
                        foreach (Clinic clinic in _listClinics)
                        {
                            if (clinic.IsHidden)
                            {
                                clinicNums += "," + clinic.ClinicNum.ToString();
                            }
                        }
                    }
                }
            }
            bool hasSpecialty = _ListDisplayFields.Any(x => x.InternalName == "Specialty");
            DataTable dataTablePats = new DataTable();
            _fillGridThread = new ODThread(new ODThread.WorkerDelegate((ODThread o) =>
            {
                dataTablePats = Patients.GetPtDataTable(doLimitOnePage, lastNameTextBox.Text, firstNameTextBox.Text, phoneTextBox.Text,
                    addressTextBox.Text, checkHideInactive.Checked, cityTextBox.Text, stateTextBox.Text,
                    ssnTextBox.Text, patNumTextBox.Text, textChartNumber.Text, billingType,
                    checkGuarantors.Checked, checkShowArchived.Checked,
                    birthdate, siteNum, subscriberIdTextBox.Text, emailTextBox.Text, countryTextBox.Text, "", clinicNums, invoiceNumberTextBox.Text, listtExplicitPatNums,
                    InitialPatNum, checkShowMerged.Checked, hasSpecialty);
            }));
            _fillGridThread.AddExitHandler(new ODThread.WorkerDelegate((ODThread o) =>
            {
                _fillGridThread = null;
                try
                {
                    this.BeginInvoke((Action)(() =>
                    {
                        _DataTablePats = dataTablePats;
                        FillGridFinal(doLimitOnePage);
                    }));
                }
                catch (Exception) { } //do nothing. Usually just a race condition trying to invoke from a disposed form.
            }));
            _fillGridThread.AddExceptionHandler(new ODThread.ExceptionDelegate((e) =>
            {
                try
                {
                    this.BeginInvoke((Action)(() =>
                    {
                        MessageBox.Show(e.Message);
                    }));
                }
                catch (Exception) { } //do nothing. Usually just a race condition trying to invoke from a disposed form.
            }));
            _fillGridThread.Start(true);
        }

        private void FillGridFinal(bool doLimitOnePage)
        {
            //long billingType=0;
            //if(comboBillingType.SelectedIndex!=0) {
            //	billingType=_listBillingTypeDefs[comboBillingType.SelectedIndex-1].DefNum;
            //}
            //long siteNum=0;
            //if(!PrefC.GetBool(PrefName.EasyHidePublicHealth) && comboSite.SelectedIndex!=0) {
            //	siteNum=SiteC.List[comboSite.SelectedIndex-1].SiteNum;
            //}
            //DateTime birthdate=PIn.Date(textBirthdate.Text); //this will frequently be minval.
            //string clinicNums="";
            //if(PrefC.HasClinicsEnabled) {
            //	if(comboClinic.SelectedIndex==0) {
            //		for(int i=0;i<_listClinics.Count;i++) {
            //			if(i>0) {
            //				clinicNums+=",";
            //			}
            //			clinicNums+=_listClinics[i].ClinicNum;
            //		}
            //	}
            //	else {
            //		clinicNums=_listClinics[comboClinic.SelectedIndex-1].ClinicNum.ToString();
            //	}
            //}
            //	PtDataTable=Patients.GetPtDataTable(limit,textLName.Text,textFName.Text,textHmPhone.Text,
            //		textAddress.Text,checkHideInactive.Checked,textCity.Text,textState.Text,
            //		textSSN.Text,textPatNum.Text,textChartNumber.Text,billingType,
            //		checkGuarantors.Checked,checkShowArchived.Checked,
            //		birthdate,siteNum,textSubscriberID.Text,textEmail.Text,textCountry.Text,textRegKey.Text,clinicNums,explicitPatNums);
            if (InitialPatNum != 0 && doLimitOnePage)
            {
                //The InitialPatNum will be at the top, so resort the list alphabetically
                DataView dataView = _DataTablePats.DefaultView;
                dataView.Sort = "LName,FName";
                _DataTablePats = dataView.ToTable();
            }
            gridMain.BeginUpdate();
            gridMain.Rows.Clear();
            ODGridRow row;
            for (int i = 0; i < _DataTablePats.Rows.Count; i++)
            {
                row = new ODGridRow();
                for (int f = 0; f < _ListDisplayFields.Count; f++)
                {
                    switch (_ListDisplayFields[f].InternalName)
                    {
                        case "LastName":
                            row.Cells.Add(_DataTablePats.Rows[i]["LName"].ToString());
                            break;
                        case "First Name":
                            row.Cells.Add(_DataTablePats.Rows[i]["FName"].ToString());
                            break;
                        case "MI":
                            row.Cells.Add(_DataTablePats.Rows[i]["MiddleI"].ToString());
                            break;
                        case "Pref Name":
                            row.Cells.Add(_DataTablePats.Rows[i]["Preferred"].ToString());
                            break;
                        case "Age":
                            row.Cells.Add(_DataTablePats.Rows[i]["age"].ToString());
                            break;
                        case "SSN":
                            row.Cells.Add(_DataTablePats.Rows[i]["SSN"].ToString());
                            break;
                        case "Hm Phone":
                            row.Cells.Add(_DataTablePats.Rows[i]["HmPhone"].ToString());
                            if (Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled)
                            {
                                row.Cells[row.Cells.Count - 1].ColorText = Color.Blue;
                                row.Cells[row.Cells.Count - 1].Underline = true;
                            }
                            break;
                        case "Wk Phone":
                            row.Cells.Add(_DataTablePats.Rows[i]["WkPhone"].ToString());
                            if (Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled)
                            {
                                row.Cells[row.Cells.Count - 1].ColorText = Color.Blue;
                                row.Cells[row.Cells.Count - 1].Underline = true;
                            }
                            break;
                        case "PatNum":
                            row.Cells.Add(_DataTablePats.Rows[i]["PatNum"].ToString());
                            break;
                        case "ChartNum":
                            row.Cells.Add(_DataTablePats.Rows[i]["ChartNumber"].ToString());
                            break;
                        case "Address":
                            row.Cells.Add(_DataTablePats.Rows[i]["Address"].ToString());
                            break;
                        case "Status":
                            row.Cells.Add(_DataTablePats.Rows[i]["PatStatus"].ToString());
                            break;
                        case "Bill Type":
                            row.Cells.Add(_DataTablePats.Rows[i]["BillingType"].ToString());
                            break;
                        case "City":
                            row.Cells.Add(_DataTablePats.Rows[i]["City"].ToString());
                            break;
                        case "State":
                            row.Cells.Add(_DataTablePats.Rows[i]["State"].ToString());
                            break;
                        case "Pri Prov":
                            row.Cells.Add(_DataTablePats.Rows[i]["PriProv"].ToString());
                            break;
                        case "Clinic":
                            row.Cells.Add(_DataTablePats.Rows[i]["clinic"].ToString());
                            break;
                        case "Birthdate":
                            row.Cells.Add(_DataTablePats.Rows[i]["Birthdate"].ToString());
                            break;
                        case "Site":
                            row.Cells.Add(_DataTablePats.Rows[i]["site"].ToString());
                            break;
                        case "Email":
                            row.Cells.Add(_DataTablePats.Rows[i]["Email"].ToString());
                            break;
                        case "Country":
                            row.Cells.Add(_DataTablePats.Rows[i]["Country"].ToString());
                            break;
                        case "RegKey":
                            row.Cells.Add(_DataTablePats.Rows[i]["RegKey"].ToString());
                            break;
                        case "OtherPhone": //will only be available if OD HQ
                            row.Cells.Add(_DataTablePats.Rows[i]["OtherPhone"].ToString());
                            break;
                        case "Wireless Ph":
                            row.Cells.Add(_DataTablePats.Rows[i]["WirelessPhone"].ToString());
                            if (Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled)
                            {
                                row.Cells[row.Cells.Count - 1].ColorText = Color.Blue;
                                row.Cells[row.Cells.Count - 1].Underline = true;
                            }
                            break;
                        case "Sec Prov":
                            row.Cells.Add(_DataTablePats.Rows[i]["SecProv"].ToString());
                            break;
                        case "LastVisit":
                            row.Cells.Add(_DataTablePats.Rows[i]["lastVisit"].ToString());
                            break;
                        case "NextVisit":
                            row.Cells.Add(_DataTablePats.Rows[i]["nextVisit"].ToString());
                            break;
                        case "Invoice Number":
                            row.Cells.Add(_DataTablePats.Rows[i]["StatementNum"].ToString());
                            break;
                        case "Specialty":
                            row.Cells.Add(_DataTablePats.Rows[i]["Specialty"].ToString());
                            break;
                    }
                }
                gridMain.Rows.Add(row);
            }
            gridMain.EndUpdate();
            if (_dateTimeLastSearch != _dateTimeLastRequest)
            {
                FillGrid(doLimitOnePage);//in case data was entered while thread was running.
            }
            gridMain.SetSelected(0, true);
            for (int i = 0; i < _DataTablePats.Rows.Count; i++)
            {
                if (PIn.Long(_DataTablePats.Rows[i][0].ToString()) == InitialPatNum)
                {
                    gridMain.SetSelected(i, true);
                    break;
                }
            }
        }

        private void gridMain_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            PatSelected();
        }

        private void gridMain_CellClick(object sender, ODGridClickEventArgs e)
        {
            ODGridCell gridCellCur = gridMain.Rows[e.Row].Cells[e.Col];
            //Only grid cells with phone numbers are blue and underlined.
            if (gridCellCur.ColorText == Color.Blue && gridCellCur.Underline == true && Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled)
            {
                DentalTek.PlaceCall(gridCellCur.Text);
            }
        }

        void addPatientButton_Click(object sender, System.EventArgs e)
        {
            if (lastNameTextBox.Text == "" && firstNameTextBox.Text == "" && textChartNumber.Text == "")
            {
                MessageBox.Show(
                    "Not allowed to add a new patient until you have done a search to see if that patient already exists.\r\n\r\nHint: just type a few letters into the Last Name box above.",
                    "Select Patient", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);

                return;
            }

            long primaryProviderNum = 0;
            if (!Preferences.GetBool(PrefName.PriProvDefaultToSelectProv))
            {
                if (Preferences.HasClinicsEnabled && clinicComboBox.SelectedIndex != 0)
                {
                    var prov = Providers.GetDefaultProvider(((ODBoxItem<Clinic>)clinicComboBox.SelectedItem).Tag.ClinicNum);
                    if (prov != null)
                    {
                        primaryProviderNum = prov.ProvNum;
                    }
                }
                else
                {
                    var prov = Providers.GetDefaultProvider();
                    if (prov != null)
                    {
                        primaryProviderNum = prov.ProvNum;
                    }
                }
            }

            var patient = 
                Patients.CreateNewPatient(
                    lastNameTextBox.Text, 
                    firstNameTextBox.Text, 
                    PIn.Date(birthdateTextBox.Text), 
                    primaryProviderNum, 
                    Clinics.ClinicNum, 
                    "Created from Select Patient window.");

            var family = Patients.GetFamily(patient.PatNum);

            using (var formPatientEdit = new FormPatientEdit(patient, family))
            {
                formPatientEdit.IsNew = true;
                if (formPatientEdit.ShowDialog() == DialogResult.OK)
                {
                    NewPatientAdded = true;
                    SelectedPatNum = patient.PatNum;

                    DialogResult = DialogResult.OK;
                }
            }
        }

        void addManyButton_Click(object sender, EventArgs e)
        {
            if (lastNameTextBox.Text == "" && firstNameTextBox.Text == "" && textChartNumber.Text == "")
            {
                MessageBox.Show(
                    "Not allowed to add a new patient until you have done a search to see if that patient already exists.\r\n\r\nHint: just type a few letters into the Last Name box above.",
                    "Select Patient",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using (var formPatientAddAll = new FormPatientAddAll())
            {
                if (lastNameTextBox.Text.Length > 1) formPatientAddAll.LName = lastNameTextBox.Text.Substring(0, 1).ToUpper() + lastNameTextBox.Text.Substring(1);
                if (firstNameTextBox.Text.Length > 1) formPatientAddAll.FName = firstNameTextBox.Text.Substring(0, 1).ToUpper() + firstNameTextBox.Text.Substring(1);
                if (birthdateTextBox.Text.Length > 1) formPatientAddAll.Birthdate = PIn.Date(birthdateTextBox.Text);
                
                if (formPatientAddAll.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                NewPatientAdded = true;
                SelectedPatNum = formPatientAddAll.SelectedPatNum;
            }

            DialogResult = DialogResult.OK;
        }

        private void PatSelected()
        {
            if (_fillGridThread != null)
            {
                return;//still filtering results (rarely happens)
            }
            SelectedPatNum = PIn.Long(_DataTablePats.Rows[gridMain.GetSelectedIndex()][0].ToString());
            DialogResult = DialogResult.OK;
        }

        ///<summary>Focus on the Form Patient Select at the last selected textbox.</summary>
        private void SelectTextBox()
        {
            if (selectedTxtBox == null)
            {
                selectedTxtBox = lastNameTextBox;//Default to the first TextBox in the search criteria if none selected.
            }
            selectedTxtBox.Focus();
            selectedTxtBox.Select(selectedTxtBox.Text.Length, 0);
        }

        /// <summary>Keeps track of the latest Selected TextBox (used to maintain cursor location when opening the On-Screen Keyboard)</summary>
        private void textBox_Enter(object sender, EventArgs e)
        {
            selectedTxtBox = (TextBox)sender;
        }

        /// <summary>Sets the handler for all of the Form's TextBox Enter Events</summary>
        private void SetAllTextBoxEnterEventListeners()
        {
            foreach (TextBox textBox in this.GetAllControls().OfType<TextBox>())
            {
                textBox.Enter += new EventHandler(this.textBox_Enter);
            }
        }

        void acceptButton_Click(object sender, System.EventArgs e)
        {
            if (gridMain.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                    "Please select a patient first.",
                    "Select Patient", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }
            PatSelected();
        }
    }
}