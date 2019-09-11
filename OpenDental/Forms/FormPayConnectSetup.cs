using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormPayConnectSetup : FormBase
    {

        private Program _progCur;
        ///<summary>Local cache of all of the clinic nums the current user has permission to access at the time the form loads.  Filled at the same time
        ///as comboClinic and is used to set programproperty.ClinicNum when saving.</summary>
        private List<long> _listUserClinicNums;
        ///<summary>List of PayConnect program properties for all clinics.
        ///Includes properties with ClinicNum=0, the headquarters props or props not assigned to a clinic.</summary>
        private List<ProgramProperty> _listProgProps;

        private List<ProgramProperty> _listXWebWebPayProgProps = new List<ProgramProperty>();

        ///<summary>Used to revert the slected index in the clinic drop down box if the user tries to change clinics
        ///and the payment type has not been set.</summary>
        private int _indexClinicRevert;

        private List<Definition> _listPaymentTypeDefs;

        public FormPayConnectSetup() => InitializeComponent();




        private void FormPayConnectSetup_Load(object sender, EventArgs e)
        {
            _progCur = Programs.GetCur(ProgramName.PayConnect);
            if (_progCur == null)
            {
                MsgBox.Show(this, "The PayConnect entry is missing from the database.");//should never happen
                return;
            }
            enabledCheckBox.Checked = _progCur.Enabled;
            if (!Preferences.HasClinicsEnabled)
            {//clinics are not enabled, use ClinicNum 0 to indicate 'Headquarters' or practice level program properties
                enabledCheckBox.Text = Lan.g(this, "Enabled");
                paySettingsGroupBox.Text = Lan.g(this, "Payment Settings");
                comboClinic.Visible = false;
                labelClinic.Visible = false;
                labelClinicEnable.Visible = false;
                _listUserClinicNums = new List<long>() { 0 };//if clinics are disabled, programproperty.ClinicNum will be set to 0
            }
            else
            {//Using clinics
                paySettingsGroupBox.Text = Lan.g(this, "Clinic Payment Settings");
                _listUserClinicNums = new List<long>();
                comboClinic.Items.Clear();
                //if PayConnect is enabled and the user is restricted to a clinic, don't allow the user to disable for all clinics
                if (Security.CurUser.ClinicRestricted)
                {
                    if (enabledCheckBox.Checked)
                    {
                        enabledCheckBox.Enabled = false;
                    }
                }
                else
                {
                    comboClinic.Items.Add(Lan.g(this, "Headquarters"));
                    //this way both lists have the same number of items in it and if 'Headquarters' is selected the programproperty.ClinicNum will be set to 0
                    _listUserClinicNums.Add(0);
                    comboClinic.SelectedIndex = 0;
                }
                List<Clinic> listClinics = Clinics.GetForUserod(Security.CurUser);
                for (int i = 0; i < listClinics.Count; i++)
                {
                    comboClinic.Items.Add(listClinics[i].Abbr);
                    _listUserClinicNums.Add(listClinics[i].ClinicNum);
                    if (Clinics.ClinicNum == listClinics[i].ClinicNum)
                    {
                        comboClinic.SelectedIndex = i;
                        if (!Security.CurUser.ClinicRestricted)
                        {
                            comboClinic.SelectedIndex++;//increment the SelectedIndex to account for 'Headquarters' in the list at position 0 if the user is not restricted.
                        }
                    }
                }
                _indexClinicRevert = comboClinic.SelectedIndex;
            }
            _listProgProps = ProgramProperties.GetForProgram(_progCur.ProgramNum);
            FillFields();
        }

        private void FillFields()
        {
            //long clinicNum = 0;
            //if (Preferences.HasClinicsEnabled)
            //{
            //    clinicNum = _listUserClinicNums[comboClinic.SelectedIndex];
            //}
            //textUsername.Text = ProgramProperties.GetPropValFromList(_listProgProps, "Username", clinicNum);
            //textPassword.Text = ProgramProperties.GetPropValFromList(_listProgProps, "Password", clinicNum);
            //string payTypeDefNum = ProgramProperties.GetPropValFromList(_listProgProps, "PaymentType", clinicNum);
            //string processingMethod = ProgramProperties.GetPropValFromList(_listProgProps, PayConnect.ProgramProperties.DefaultProcessingMethod, clinicNum);
            //checkTerminal.Checked = PIn.Bool(ProgramProperties.GetPropValFromList(_listProgProps, "TerminalProcessingEnabled", clinicNum));
            //checkForceRecurring.Checked = PIn.Bool(ProgramProperties.GetPropValFromList(_listProgProps,
            //    PayConnect.ProgramProperties.PayConnectForceRecurringCharge, clinicNum));
            //checkPreventSavingNewCC.Checked = PIn.Bool(ProgramProperties.GetPropValFromList(_listProgProps,
            //    PayConnect.ProgramProperties.PayConnectPreventSavingNewCC, clinicNum));
            //portalPayEnabledCheckBox.Checked = PIn.Bool(ProgramProperties.GetPropValFromList(_listProgProps, PayConnect.ProgramProperties.PatientPortalPaymentsEnabled, clinicNum));
            //tokenTextBox.Text = PIn.String(ProgramProperties.GetPropValFromList(_listProgProps, PayConnect.ProgramProperties.PatientPortalPaymentsToken, clinicNum));
            //comboPaymentType.Items.Clear();
            //_listPaymentTypeDefs = Definition.GetByCategory(DefinitionCategory.PaymentTypes);;
            //for (int i = 0; i < _listPaymentTypeDefs.Count; i++)
            //{
            //    comboPaymentType.Items.Add(_listPaymentTypeDefs[i].Description);
            //    if (_listPaymentTypeDefs[i].Id.ToString() == payTypeDefNum)
            //    {
            //        comboPaymentType.SelectedIndex = i;
            //    }
            //}
            //comboDefaultProcessing.Items.Clear();
            //comboDefaultProcessing.Items.Add(Lan.g(this, PayConnectProcessingMethod.WebService.GetDescription()));
            //comboDefaultProcessing.Items.Add(Lan.g(this, PayConnectProcessingMethod.Terminal.GetDescription()));
            //if (processingMethod == "0" || processingMethod == "1")
            //{
            //    comboDefaultProcessing.SelectedIndex = PIn.Int(processingMethod);
            //}
        }

        private void comboClinic_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboClinic.SelectedIndex == _indexClinicRevert)
            {//didn't change the selected clinic
                return;
            }
            //if PayConnect is enabled and the username and password are set for the current clinic,
            //make the user select a payment type before switching clinics
            if (enabledCheckBox.Checked && textUsername.Text != "" && textPassword.Text != "" && comboPaymentType.SelectedIndex == -1)
            {
                comboClinic.SelectedIndex = _indexClinicRevert;
                MsgBox.Show(this, "Please select a payment type first.");
                return;
            }
            SynchWithHQ();//if the user just modified the HQ credentials, change any credentials that were the same as HQ to keep them synched
            UpdateListProgramPropertiesForClinic(_listUserClinicNums[_indexClinicRevert]);
            _indexClinicRevert = comboClinic.SelectedIndex;//now that we've updated the values for the clinic we're switching from, update _indexClinicRevert
            FillFields();
        }

        ///<summary>For each clinic, if the Username and Password are the same as the HQ (ClinicNum=0) Username and Password, update the clinic with the
        ///values in the text boxes.  Only modifies other clinics if _indexClinicRevert=0, meaning user just modified the HQ clinic credentials.</summary>
        private void SynchWithHQ()
        {
            if (!Preferences.HasClinicsEnabled || _listUserClinicNums[_indexClinicRevert] > 0)
            {//using clinics, and modifying the HQ clinic. otherwise return.
                return;
            }
            string hqUsername = ProgramProperties.GetPropValFromList(_listProgProps, "Username", 0);//HQ Username before updating to value in textbox
            string hqPassword = ProgramProperties.GetPropValFromList(_listProgProps, "Password", 0);//HQ Password before updating to value in textbox
            string hqPayType = ProgramProperties.GetPropValFromList(_listProgProps, "PaymentType", 0);//HQ PaymentType before updating to combo box selection
                                                                                                      //IsOnlinePaymentsEnabled will not be synced with HQ, since some clinics may need to disable patient portal payments
                                                                                                      //Token will not be synced with HQ, since some clinics may need to disable patient portal payments
            string payTypeCur = "";
            if (comboPaymentType.SelectedIndex > -1)
            {
                payTypeCur = _listPaymentTypeDefs[comboPaymentType.SelectedIndex].Id.ToString();
            }
            //for each distinct ClinicNum in the prog property list for PayConnect except HQ
            foreach (long clinicNum in _listProgProps.Select(x => x.ClinicNum).Where(x => x > 0).Distinct())
            {
                //if this clinic has a different username or password, skip it
                if (!_listProgProps.Exists(x => x.ClinicNum == clinicNum && x.PropertyDesc == "Username" && x.PropertyValue == hqUsername)
                    || !_listProgProps.Exists(x => x.ClinicNum == clinicNum && x.PropertyDesc == "Password" && x.PropertyValue == hqPassword))
                {
                    continue;
                }
                //this clinic had a matching username and password, so update the username and password to keep it synched with HQ
                _listProgProps.FindAll(x => x.ClinicNum == clinicNum && x.PropertyDesc == "Username")
                    .ForEach(x => x.PropertyValue = textUsername.Text);//always 1 item; null safe
                _listProgProps.FindAll(x => x.ClinicNum == clinicNum && x.PropertyDesc == "Password")
                    .ForEach(x => x.PropertyValue = textPassword.Text);//always 1 item; null safe
                if (string.IsNullOrEmpty(payTypeCur))
                {
                    continue;
                }
                //update clinic payment type if it originally matched HQ's payment type and the selected payment type is valid
                _listProgProps.FindAll(x => x.ClinicNum == clinicNum && x.PropertyDesc == "PaymentType" && x.PropertyValue == hqPayType)
                    .ForEach(x => x.PropertyValue = payTypeCur);//always 1 item; null safe
            }
        }

        private void UpdateListProgramPropertiesForClinic(long clinicNum)
        {
            //string payTypeSelected = "";
            //if (comboPaymentType.SelectedIndex > -1)
            //{
            //    payTypeSelected = _listPaymentTypeDefs[comboPaymentType.SelectedIndex].Id.ToString();
            //}
            //string processingMethodSelected = comboDefaultProcessing.SelectedIndex.ToString();
            ////set the values in the list for this clinic
            //_listProgProps.FindAll(x => x.ClinicNum == clinicNum && x.PropertyDesc == "Username")
            //    .ForEach(x => x.PropertyValue = textUsername.Text);//always 1 item; null safe
            //_listProgProps.FindAll(x => x.ClinicNum == clinicNum && x.PropertyDesc == "Password")
            //    .ForEach(x => x.PropertyValue = textPassword.Text);//always 1 item; null safe
            //_listProgProps.FindAll(x => x.ClinicNum == clinicNum && x.PropertyDesc == "PaymentType")
            //    .ForEach(x => x.PropertyValue = payTypeSelected);//always 1 item; null safe
            //_listProgProps.FindAll(x => x.ClinicNum == clinicNum && x.PropertyDesc == PayConnect.ProgramProperties.DefaultProcessingMethod)
            //    .ForEach(x => x.PropertyValue = processingMethodSelected);
            //_listProgProps.FindAll(x => x.ClinicNum == clinicNum && x.PropertyDesc == "TerminalProcessingEnabled")
            //    .ForEach(x => x.PropertyValue = POut.Bool(checkTerminal.Checked));//always 1 item; null safe
            //_listProgProps.FindAll(x => x.ClinicNum == clinicNum && x.PropertyDesc == PayConnect.ProgramProperties.PayConnectForceRecurringCharge)
            //    .ForEach(x => x.PropertyValue = POut.Bool(checkForceRecurring.Checked));
            //_listProgProps.FindAll(x => x.ClinicNum == clinicNum && x.PropertyDesc == PayConnect.ProgramProperties.PayConnectPreventSavingNewCC)
            //    .ForEach(x => x.PropertyValue = POut.Bool(checkPreventSavingNewCC.Checked));
            //_listProgProps.FindAll(x => x.ClinicNum == clinicNum && x.PropertyDesc == PayConnect.ProgramProperties.PatientPortalPaymentsEnabled)
            //    .ForEach(x => x.PropertyValue = POut.Bool(portalPayEnabledCheckBox.Checked));
            //_listProgProps.FindAll(x => x.ClinicNum == clinicNum && x.PropertyDesc == PayConnect.ProgramProperties.PatientPortalPaymentsToken)
            //    .ForEach(x => x.PropertyValue = tokenTextBox.Text);
        }

        private void checkPatientPortalPayEnabled_Click(object sender, EventArgs e)
        {
            if (portalPayEnabledCheckBox.Checked)
            {
                long clinicNum = 0;
                if (Preferences.HasClinicsEnabled)
                {
                    clinicNum = _listUserClinicNums[comboClinic.SelectedIndex];
                }
                OpenDentBusiness.WebTypes.Shared.XWeb.WebPaymentProperties xwebProperties = new OpenDentBusiness.WebTypes.Shared.XWeb.WebPaymentProperties();
                try
                {
                    ProgramProperties.GetXWebCreds(clinicNum, out xwebProperties);
                }
                catch
                {

                }
                string msg = 
                    "Online payments is already enabled for XWeb and must be disabled in order to use PayConnect online payments. " + 
                    "Would you like to disable XWeb online payments?";

                if (xwebProperties != null && xwebProperties.IsPaymentsAllowed && MessageBox.Show(msg, "", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    portalPayEnabledCheckBox.Checked = false;
                    return;
                }
                //User wants to disable XWeb online payments and use PayConnect online payments
                ProgramProperty ppOnlinePaymentEnabled = ProgramProperties.GetWhere(x =>
                      x.ProgramNum == Programs.GetCur(ProgramName.Xcharge).ProgramNum
                      && x.ClinicNum == clinicNum
                      && x.PropertyDesc == "IsOnlinePaymentsEnabled")
                    .FirstOrDefault();
                if (ppOnlinePaymentEnabled == null)
                {
                    return;//Should never happen since we successfully found it in the GetXWebCreds method.
                }
                _listXWebWebPayProgProps.Add(ppOnlinePaymentEnabled);
            }
        }

        void GenerateButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textUsername.Text))
            {
                MessageBox.Show(
                    "Username cannot be empty.",
                    "PayConnect Setup",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (string.IsNullOrWhiteSpace(textPassword.Text))
            {
                MessageBox.Show(
                    "Password cannot be empty.",
                    "PayConnect Setup",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (!string.IsNullOrWhiteSpace(tokenTextBox.Text))
            {
                var result =
                    MessageBox.Show(
                        "A token already exists. Do you want to create a new one?",
                        "PayConnect Setup", 
                        MessageBoxButtons.OKCancel, 
                        MessageBoxIcon.Question);

                if (result == DialogResult.Cancel) return;
            }

            try
            {
                //tokenTextBox.Text = 
                //    PayConnectREST.GetAccountToken(
                //        textUsername.Text, 
                //        textPassword.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "PayConnect Setup",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        void WebsiteLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start("http://www.dentalxchange.com");

        private void checkTerminal_CheckedChanged(object sender, EventArgs e)
        {
            downloadButton.Visible = checkTerminal.Checked;
        }

        void DownloadButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            WebClient client = new WebClient();

            string targetFileName = Path.Combine(Path.GetTempPath(), "VeriFoneUSBUARTDriver_Vx_1.0.0.52_B5.zip");

            try
            {
                client.DownloadFile(
                    "http://www.opendental.com/download/drivers/VeriFoneUSBUARTDriver_Vx_1.0.0.52_B5.zip",
                    targetFileName);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;

                MessageBox.Show(
                    "Unable to download driver. " + ex.Message,
                    "PayConnect Setup",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            try
            {
                string setupFileName = "";

                // Create the directory to extract the zip file.
                var targetPath = Path.Combine(Path.GetTempPath(), "VeriFoneUSBUARTDriver");
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                // Check whether the zip file contains a setup.exe file.
                using (var fileStream = File.Open(targetFileName, FileMode.Open, FileAccess.Read, FileShare.None))
                using (var zipArchive = new ZipArchive(fileStream))
                {
                    foreach (var entry in zipArchive.Entries)
                    {
                        if (entry.Name.ToLower() == "")
                        {
                            setupFileName = Path.Combine(targetPath, entry.FullName);

                            break;
                        }
                    }
                }

                // If there is no setup, do nothing.
                if (setupFileName == "")
                {
                    MessageBox.Show(
                        "Unable to install driver. Setup.exe file not found.",
                        "PayConnect Setup",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                // Extract the contents of the zip file.
                ZipFile.ExtractToDirectory(targetFileName, targetPath);

                Cursor = Cursors.Default;

                Process.Start(setupFileName);

                MessageBox.Show(
                    "Download complete. Run the Setup.exe file in " + targetPath + " if it does not start automatically.",
                    "PayConnect Setup",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;

                MessageBox.Show(
                    "Unable to install driver. " + ex.Message,
                    "PayConnect Setup",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        void AcceptButton_Click(object sender, System.EventArgs e)
        {
            //#region Validation
            ////if clinics are not enabled and the PayConnect program link is enabled, make sure there is a username and password set
            ////if clinics are enabled, the program link can be enabled with blank username and/or password fields for some clinics
            ////clinics with blank username and/or password will essentially not have PayConnect enabled
            ////if 'Enable terminal processing' is checked then the practice/clinic will not need a username and password.
            //if (enabledCheckBox.Checked && !checkTerminal.Checked && !Preferences.HasClinicsEnabled &&
            //    (textUsername.Text == "" || textPassword.Text == ""))
            //{
            //    MsgBox.Show(this, "Please enter a username and password first.");
            //    return;
            //}
            //if (enabledCheckBox.Checked //if PayConnect is enabled
            //    && comboPaymentType.SelectedIndex < 0 //and the payment type is not set
            //    && (!Preferences.HasClinicsEnabled  //and either clinics are not enabled (meaning this is the only set of username, password, payment type values)
            //    || (textUsername.Text != "" && textPassword.Text != ""))) //or clinics are enabled and this clinic's link has a username and password set
            //{
            //    MsgBox.Show(this, "Please select a payment type first.");
            //    return;
            //}
            //if (enabledCheckBox.Checked //if PayConnect is enabled
            //    && comboDefaultProcessing.SelectedIndex < 0)
            //{
            //    MsgBox.Show(this, "Please select a default processing method type first.");
            //    return;
            //}
            //SynchWithHQ();//if the user changes the HQ credentials, any clinic that had the same credentials will be kept in synch with HQ
            //long clinicNum = 0;
            //if (Preferences.HasClinicsEnabled)
            //{
            //    clinicNum = _listUserClinicNums[comboClinic.SelectedIndex];
            //}
            //UpdateListProgramPropertiesForClinic(clinicNum);
            //string payTypeCur;
            ////make sure any other clinics with PayConnect enabled also have a payment type selected
            //for (int i = 0; i < _listUserClinicNums.Count; i++)
            //{
            //    if (!enabledCheckBox.Checked)
            //    {//if program link is not enabled, do not bother checking the payment type selected
            //        break;
            //    }
            //    payTypeCur = ProgramProperties.GetPropValFromList(_listProgProps, "PaymentType", _listUserClinicNums[i]);
            //    //if the program is enabled and the username and password fields are not blank,
            //    //PayConnect is enabled for this clinic so make sure the payment type is also set
            //    if (((ProgramProperties.GetPropValFromList(_listProgProps, "Username", _listUserClinicNums[i]) != "" //if username set
            //        && ProgramProperties.GetPropValFromList(_listProgProps, "Password", _listUserClinicNums[i]) != "") //and password set
            //        || ProgramProperties.GetPropValFromList(_listProgProps, "TerminalProcessingEnabled", _listUserClinicNums[i]) == "1")//or terminal enabled
            //        && !_listPaymentTypeDefs.Any(x => x.Id.ToString() == payTypeCur)) //and paytype is not a valid DefNum
            //    {
            //        MsgBox.Show(this, "Please select the payment type for all clinics with PayConnect username and password set.");
            //        return;
            //    }
            //}
            //#endregion Validation
            //#region Save
            //if (_progCur.Enabled != enabledCheckBox.Checked)
            //{//only update the program if the IsEnabled flag has changed
            //    _progCur.Enabled = enabledCheckBox.Checked;
            //    Programs.Update(_progCur);
            //}
            //ProgramProperties.Sync(_listProgProps, _progCur.ProgramNum);
            ////Find all clinics that have PayConnect online payments enabled
            //_listProgProps.FindAll(x => x.PropertyDesc == PayConnect.ProgramProperties.PatientPortalPaymentsEnabled && PIn.Bool(x.PropertyValue)).ForEach(x =>
            //{
            //    //Find all XWeb program properties that we saved in this session.  Only clinics that have changes will have an XWeb property in memory.
            //    //This is needed to ensure that we don't disable XWeb online payments if someone
            //    // checks to use PayConnect online payments and then decides to keep it disabled during the same session.
            //    ProgramProperty ppXWebOnlinePayments = _listXWebWebPayProgProps.FirstOrDefault(y => y.ClinicNum == x.ClinicNum);
            //    if (ppXWebOnlinePayments != null)
            //    {
            //        ProgramProperties.UpdateProgramPropertyWithValue(ppXWebOnlinePayments, POut.Bool(false));
            //    }
            //});
            //#endregion Save
            //DataValid.SetInvalid(InvalidType.Programs);
            DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void textPassword_TextChanged(object sender, EventArgs e)
        {
            //Let the users see what they are typing if they clear out the password field completely
            if (textPassword.Text.Trim().Length == 0)
            {
                textPassword.UseSystemPasswordChar = false;
            }
        }
    }
}