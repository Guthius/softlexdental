using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using System.Diagnostics;
using CodeBase;

namespace OpenDental
{
    public partial class FormPaySimpleSetup : ODForm
    {
		private Program _progCur;
		///<summary>Local cache of all of the ClinicNums the current user has permission to access at the time the form loads.  Filled at the same time
		///as comboClinic and is used to set programproperty.ClinicNum when saving.</summary>
		private List<long> _listUserClinicNums;
		///<summary>List of PaySimple program properties for all clinics.
		///Includes properties with ClinicNum=0, the headquarters props or props not assigned to a clinic.</summary>
		private List<ProgramPreference> _listProgProps;


        ///<summary>Used to revert the slected index in the clinic drop down box if the user tries to change clinics
        ///and the payment type has not been set.</summary>
        private int _indexClinicRevert;
        private List<Definition> _listPaymentTypeDefs;

        public FormPaySimpleSetup()
        {
            InitializeComponent();
        }

        private void FormPaySimpleSetup_Load(object sender, EventArgs e)
        {
            // TODO: Implement me

            //_progCur = Programs.GetCur(ProgramName.PaySimple);
            //if (_progCur == null)
            //{
            //    MsgBox.Show(this, "The PaySimple entry is missing from the database.");//should never happen
            //    return;
            //}
            //checkEnabled.Checked = _progCur.Enabled;

            //groupPaySettings.Text = Lan.g(this, "Clinic Payment Settings");
            //_listUserClinicNums = new List<long>();
            //comboClinic.Items.Clear();
            ////if PaySimple is enabled and the user is restricted to a clinic, don't allow the user to disable for all clinics
            //if (Security.CurrentUser.ClinicRestricted)
            //{
            //    if (checkEnabled.Checked)
            //    {
            //        checkEnabled.Enabled = false;
            //    }
            //}
            //else
            //{
            //    comboClinic.Items.Add("Headquarters");
            //    //this way both lists have the same number of items in it and if 'Headquarters' is selected the programproperty.ClinicNum will be set to 0
            //    _listUserClinicNums.Add(0);
            //    comboClinic.SelectedIndex = 0;
            //}

            //List<Clinic> listClinics = Clinic.GetByUser(Security.CurrentUser).ToList();
            //for (int i = 0; i < listClinics.Count; i++)
            //{
            //    comboClinic.Items.Add(listClinics[i].Abbr);
            //    _listUserClinicNums.Add(listClinics[i].Id);
            //    if (Clinics.ClinicId == listClinics[i].Id)
            //    {
            //        comboClinic.SelectedIndex = i;
            //        if (!Security.CurrentUser.ClinicRestricted)
            //        {
            //            comboClinic.SelectedIndex++;//increment the SelectedIndex to account for 'Headquarters' in the list at position 0 if the user is not restricted.
            //        }
            //    }
            //}

            //_indexClinicRevert = comboClinic.SelectedIndex;

            //_listProgProps = ProgramProperties.GetForProgram(_progCur.ProgramNum);

            //foreach (Clinic clinicCur in Clinic.GetByUser(Security.CurrentUser))
            //{
            //    AddNeededProgramProperties(clinicCur.Id);
            //}

            //FillFields();
        }

        private void AddNeededProgramProperties(long clinicNum)
        {
            //if(!_listProgProps.Any(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiUserName)) {
            //	_listProgProps.Add(new ProgramProperty() {
            //		ClinicNum=clinicNum,
            //		PropertyDesc=PaySimple.PropertyDescs.PaySimpleApiUserName,
            //		ProgramNum=_progCur.ProgramNum,
            //		PropertyValue=_listProgProps.FirstOrDefault(x => x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiUserName && x.ClinicNum==0).PropertyValue,
            //	});
            //}
            //if(!_listProgProps.Any(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiKey)) {
            //	_listProgProps.Add(new ProgramProperty() {
            //		ClinicNum=clinicNum,
            //		PropertyDesc=PaySimple.PropertyDescs.PaySimpleApiKey,
            //		ProgramNum=_progCur.ProgramNum,
            //		PropertyValue=_listProgProps.FirstOrDefault(x => x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiKey && x.ClinicNum==0).PropertyValue,
            //	});
            //}
            //if(!_listProgProps.Any(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePayTypeCC)) {
            //	_listProgProps.Add(new ProgramProperty() {
            //		ClinicNum=clinicNum,
            //		PropertyDesc=PaySimple.PropertyDescs.PaySimplePayTypeCC,
            //		ProgramNum=_progCur.ProgramNum,
            //		PropertyValue=_listProgProps.FirstOrDefault(x => x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePayTypeCC && x.ClinicNum==0).PropertyValue,
            //	});
            //}
            //if(!_listProgProps.Any(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePayTypeACH)) {
            //	_listProgProps.Add(new ProgramProperty() {
            //		ClinicNum=clinicNum,
            //		PropertyDesc=PaySimple.PropertyDescs.PaySimplePayTypeACH,
            //		ProgramNum=_progCur.ProgramNum,
            //		PropertyValue=_listProgProps.FirstOrDefault(x => x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePayTypeACH && x.ClinicNum==0).PropertyValue,
            //	});
            //}
            //if(!_listProgProps.Any(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePreventSavingNewCC)) {
            //	_listProgProps.Add(new ProgramProperty() {
            //		ClinicNum=clinicNum,
            //		PropertyDesc=PaySimple.PropertyDescs.PaySimplePreventSavingNewCC,
            //		ProgramNum=_progCur.ProgramNum,
            //		PropertyValue=_listProgProps.FirstOrDefault(x => x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePreventSavingNewCC && x.ClinicNum==0)?.PropertyValue??"0",
            //	});
            //}
        }

        private void FillFields()
        {
            //long clinicNum=0;
            //if(Preferences.HasClinicsEnabled) {
            //	clinicNum=_listUserClinicNums[comboClinic.SelectedIndex];
            //}
            //textUsername.Text=ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimpleApiUserName,clinicNum);
            //textKey.Text=ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimpleApiKey,clinicNum);
            //string payTypeDefNumCC=ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimplePayTypeCC,clinicNum);
            //string payTypeDefNumACH=ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimplePayTypeACH,clinicNum);
            //checkPreventSavingNewCC.Checked=PIn.Bool(ProgramProperties.GetPropValFromList(_listProgProps,
            //	PaySimple.PropertyDescs.PaySimplePreventSavingNewCC,clinicNum));
            //_listPaymentTypeDefs=Definition.GetByCategory(DefinitionCategory.PaymentTypes);
            //comboPaymentTypeCC.SetItems(_listPaymentTypeDefs,x => x.Description,x => x.Id.ToString()==payTypeDefNumCC);
            //comboPaymentTypeACH.SetItems(_listPaymentTypeDefs,x => x.Description,x => x.Id.ToString()==payTypeDefNumACH);
        }

        private void comboClinic_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //if(comboClinic.SelectedIndex==_indexClinicRevert) {//didn't change the selected clinic
            //	return;
            //}
            ////if PaySimple is enabled and the username and key are set for the current clinic,
            ////make the user select a payment type before switching clinics
            //if(checkEnabled.Checked && !IsClinicCurSetupDone()) {
            //	comboClinic.SelectedIndex=_indexClinicRevert;
            //	MsgBox.Show(this,"Please select a username, key, and/or payment type first.");
            //	return;
            //}
            //SynchWithHQ();//if the user just modified the HQ credentials, change any credentials that were the same as HQ to keep them synched
            ////set the values in the list for the clinic we are switching from, at _clinicIndexRevert
            //_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiUserName)
            //	.ForEach(x => x.PropertyValue=textUsername.Text);//always 1 item; null safe
            //_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiKey)
            //	.ForEach(x => x.PropertyValue=textKey.Text);//always 1 item; null safe
            //_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] 
            //		&& x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePayTypeCC 
            //		&& comboPaymentTypeCC.SelectedIndex>-1)
            //	.ForEach(x => x.PropertyValue=comboPaymentTypeCC.SelectedTag<Definition>().Id.ToString());//always 1 item selected
            //_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert]
            //		&& x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePayTypeACH
            //		&& comboPaymentTypeACH.SelectedIndex>-1)
            //	.ForEach(x => x.PropertyValue=comboPaymentTypeACH.SelectedTag<Definition>().Id.ToString());//always 1 item selected
            //_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert]
            //	&& x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePreventSavingNewCC)
            //	.ForEach(x => x.PropertyValue=POut.Bool(checkPreventSavingNewCC.Checked));
            //_indexClinicRevert=comboClinic.SelectedIndex;//now that we've updated the values for the clinic we're switching from, update _indexClinicRevert
            //FillFields();
        }

        private bool IsClinicCurSetupDone()
        {
            //If nothing is set, they are OK to change clinics and save.
            if (string.IsNullOrWhiteSpace(textUsername.Text) && string.IsNullOrWhiteSpace(textKey.Text))
            {
                return true;
            }
            //If everything is set, they are OK to change clinics and save.
            if (!string.IsNullOrWhiteSpace(textUsername.Text) && !string.IsNullOrWhiteSpace(textKey.Text)
                && comboPaymentTypeCC.SelectedIndex >= 0 && comboPaymentTypeACH.SelectedIndex >= 0)
            {
                return true;
            }
            return false;
        }

        ///<summary>For each clinic, if the Username and Key are the same as the HQ (ClinicNum=0) Username and Key, update the clinic with the
        ///values in the text boxes.  Only modifies other clinics if _indexClinicRevert=0, meaning user just modified the HQ clinic credentials.</summary>
        private void SynchWithHQ()
        {
            //if(!Preferences.HasClinicsEnabled || _listUserClinicNums[_indexClinicRevert]>0) {//using clinics, and modifying the HQ clinic. otherwise return.
            //	return;
            //}
            //string hqUsername=ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimpleApiUserName,0);//HQ Username before updating to value in textbox
            //string hqKey=ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimpleApiKey,0);//HQ Key before updating to value in textbox
            //string hqPayTypeCC=ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimplePayTypeCC,0);//HQ PaymentType before updating to combo box selection
            //string payTypeCC="";
            //if(comboPaymentTypeCC.SelectedIndex>-1) {
            //	payTypeCC=comboPaymentTypeCC.SelectedTag<Definition>().Id.ToString();
            //}
            //string hqPayTypeACH=ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimplePayTypeACH,0);//HQ PaymentType before updating to combo box selection
            //string payTypeACH="";
            //if(comboPaymentTypeACH.SelectedIndex>-1) {
            //	payTypeACH=comboPaymentTypeACH.SelectedTag<Definition>().Id.ToString();
            //}
            ////for each distinct ClinicNum in the prog property list for PaySimple except HQ
            //foreach(long clinicNum in _listProgProps.Select(x => x.ClinicNum).Where(x => x>0).Distinct()) {
            //	//if this clinic has a different username or key, skip it
            //	if(!_listProgProps.Exists(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiUserName && x.PropertyValue==hqUsername)
            //		|| !_listProgProps.Exists(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiKey && x.PropertyValue==hqKey)) {
            //		continue;
            //	}
            //	//this clinic had a matching username and key, so update the username and key to keep it synched with HQ
            //	_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiUserName)
            //		.ForEach(x => x.PropertyValue=textUsername.Text);//always 1 item; null safe
            //	_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiKey)
            //		.ForEach(x => x.PropertyValue=textKey.Text);//always 1 item; null safe
            //	if(!string.IsNullOrEmpty(payTypeCC)) {
            //		//update clinic payment type if it originally matched HQ's payment type and the selected payment type is valid
            //		_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePayTypeCC 
            //				&& x.PropertyValue==hqPayTypeCC)
            //			.ForEach(x => x.PropertyValue=payTypeCC);
            //	}
            //	if(!string.IsNullOrEmpty(payTypeACH)) {
            //		//update clinic payment type if it originally matched HQ's payment type and the selected payment type is valid
            //		_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePayTypeACH
            //				&& x.PropertyValue==hqPayTypeACH)
            //			.ForEach(x => x.PropertyValue=payTypeACH);
            //	}
            //}
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.paysimple.com");
        }

        private void butOK_Click(object sender, System.EventArgs e)
        {
            #region Validation
            //if clinics are not enabled and the PaySimple program link is enabled, make sure there is a username and key set
            //if clinics are enabled, the program link can be enabled with blank username and/or key fields for some clinics
            //clinics with blank username and/or key will essentially not have PaySimple enabled
            if (checkEnabled.Checked && !IsClinicCurSetupDone())
            {//Also works for offices not using clinics
                MsgBox.Show(this, "Please enter a username, key, and/or payment type first.");
                return;
            }
            SynchWithHQ();//if the user changes the HQ credentials, any clinic that had the same credentials will be kept in synch with HQ
                          //long clinicNum=0;
                          //if(Preferences.HasClinicsEnabled) {
                          //	clinicNum=_listUserClinicNums[comboClinic.SelectedIndex];
                          //}
                          //string payTypeCCSelected="";
                          //if(comboPaymentTypeCC.SelectedIndex>-1) {
                          //	payTypeCCSelected=comboPaymentTypeCC.SelectedTag<Definition>().Id.ToString();
                          //}
                          //string payTypeACHSelected="";
                          //if(comboPaymentTypeACH.SelectedIndex>-1) {
                          //	payTypeACHSelected=comboPaymentTypeACH.SelectedTag<Definition>().Id.ToString();
                          //}
                          ////set the values in the list for this clinic
                          //_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiUserName)
                          //	.ForEach(x => x.PropertyValue=textUsername.Text);//always 1 item; null safe
                          //_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiKey)
                          //	.ForEach(x => x.PropertyValue=textKey.Text);//always 1 item; null safe
                          //_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePayTypeCC)
                          //	.ForEach(x => x.PropertyValue=payTypeCCSelected);//always 1 item
                          //_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePayTypeACH)
                          //	.ForEach(x => x.PropertyValue=payTypeACHSelected);//always 1 item
                          //_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePreventSavingNewCC)
                          //	.ForEach(x => x.PropertyValue=POut.Bool(checkPreventSavingNewCC.Checked));
                          //string payTypeCC;
                          //string payTypeACH;
                          ////make sure any other clinics with PaySimple enabled also have a payment type selected
                          //for(int i=0;i<_listUserClinicNums.Count;i++) {
                          //	if(!checkEnabled.Checked) {//if program link is not enabled, do not bother checking the payment type selected
                          //		break;
                          //	}
                          //	payTypeCC=ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimplePayTypeCC,_listUserClinicNums[i]);
                          //	payTypeACH=ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimplePayTypeACH,_listUserClinicNums[i]);
                          //	//if the program is enabled and the username and key fields are not blank,
                          //	//PaySimple is enabled for this clinic so make sure the payment type is also set
                          //	if(ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimpleApiUserName,_listUserClinicNums[i])!="" //if username set
                          //		&& ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimpleApiKey,_listUserClinicNums[i])!=""//and key set
                          //		//and either paytype is not a valid DefNum
                          //		&& (!_listPaymentTypeDefs.Any(x => x.Id.ToString()==payTypeCC)
                          //			|| !_listPaymentTypeDefs.Any(x => x.Id.ToString()==payTypeACH))) 
                          //	{
                          //		MsgBox.Show(this,"Please select payment types for all clinics with PaySimple username and key set.");
                          //		return;
                          //	}
                          //}
            #endregion Validation
            //#region Save
            //if(_progCur.Enabled!=checkEnabled.Checked) {//only update the program if the IsEnabled flag has changed
            //	_progCur.Enabled=checkEnabled.Checked;
            //	Programs.Update(_progCur);
            //}
            //ProgramProperties.Sync(_listProgProps,_progCur.ProgramNum);
            //#endregion Save
            //DataValid.SetInvalid(InvalidType.Programs);
            DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
