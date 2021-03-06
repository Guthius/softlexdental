using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using OpenDentBusiness;
using System.IO;
using CodeBase;

namespace OpenDental
{
    public partial class FormBillingDefaults : ODForm
    {
        private List<Clinic> _listClinics;
        private List<Ebill> _listEbills;
        ///<summary>Stale deep copy of _listEbills to use with sync.</summary>
        private List<Ebill> _listEbillsOld;
        ///<summary>The eBill corresponding to the currently selected clinic if clinics are enabled.</summary>
        private Ebill _eBillCur;
        ///<summary>The eBill corresponding to the default credentials.</summary>
        private Ebill _eBillDefault;
        ///<summary>Corresponds to the output path/url of the 5 items in listElectBilling</summary>
        private string[] arrayOutputPaths = new string[5];
        public bool IsUserPassOnly;

        public FormBillingDefaults()
        {
            InitializeComponent();

        }

        private void FormBillingDefaults_Load(object sender, EventArgs e)
        {
            textDays.Text = Preference.GetLong(PreferenceName.BillingDefaultsLastDays).ToString();
            checkIntermingled.Checked = Preference.GetBool(PreferenceName.BillingDefaultsIntermingle);
            checkSinglePatient.Checked = Preference.GetBool(PreferenceName.BillingDefaultsSinglePatient);
            textNote.Text = Preference.GetString(PreferenceName.BillingDefaultsNote);
            checkCreatePDF.Checked = Preference.GetBool(PreferenceName.BillingElectCreatePDF);
            checkBoxBillShowTransSinceZero.Checked = Preference.GetBool(PreferenceName.BillingShowTransSinceBalZero);
            listElectBilling.SelectedIndex = 0;
            int billingUseElectronicIdx = Preference.GetInt(PreferenceName.BillingUseElectronic);
            if (billingUseElectronicIdx == 1)
            {
                listElectBilling.SelectedIndex = 1;
                checkCreatePDF.Enabled = true;
                labelBlankForDefault.Visible = true;
            }
            if (billingUseElectronicIdx == 2)
            {
                listElectBilling.SelectedIndex = 2;
            }
            if (billingUseElectronicIdx == 3)
            {
                checkCreatePDF.Enabled = true;
                listElectBilling.SelectedIndex = 3;
            }
            if (billingUseElectronicIdx == 4)
            {
                listElectBilling.SelectedIndex = 4;
            }
            arrayOutputPaths[0] = "";//Will never be used, but is helpful to keep the indexes of arrayOutputPaths aligned with the options listed in listElectBilling.
            arrayOutputPaths[1] = Preference.GetString(PreferenceName.BillingElectStmtUploadURL);
            arrayOutputPaths[2] = Preference.GetString(PreferenceName.BillingElectStmtOutputPathPos);
            arrayOutputPaths[3] = Preference.GetString(PreferenceName.BillingElectStmtOutputPathClaimX);
            arrayOutputPaths[4] = Preference.GetString(PreferenceName.BillingElectStmtOutputPathEds);
            textStatementURL.Text = arrayOutputPaths[billingUseElectronicIdx];
            textVendorId.Text = Preference.GetString(PreferenceName.BillingElectVendorId);
            textVendorPMScode.Text = Preference.GetString(PreferenceName.BillingElectVendorPMSCode);
            string cc = Preference.GetString(PreferenceName.BillingElectCreditCardChoices);
            if (cc.Contains("MC"))
            {
                checkMC.Checked = true;
            }
            if (cc.Contains("V"))
            {
                checkV.Checked = true;
            }
            if (cc.Contains("D"))
            {
                checkD.Checked = true;
            }
            if (cc.Contains("A"))
            {
                checkAmEx.Checked = true;
            }
            textBillingEmailSubject.Text = Preference.GetString(PreferenceName.BillingEmailSubject);
            textBillingEmailBody.Text = Preference.GetString(PreferenceName.BillingEmailBodyText);
            textInvoiceNote.Text = Preference.GetString(PreferenceName.BillingDefaultsInvoiceNote);
            _listEbills = Ebills.GetDeepCopy();
            _listEbillsOld = _listEbills.Select(x => x.Copy()).ToList();
            //Find the default Ebill
            for (int i = 0; i < _listEbills.Count; i++)
            {
                if (_listEbills[i].ClinicNum == 0)
                {
                    _eBillDefault = _listEbills[i];
                }
            }
            if (_eBillDefault == null)
            {
                MsgBox.Show(this, "The default ebill entry is missing. Run " + nameof(DatabaseMaintenances.EbillMissingDefaultEntry)
                    + " in the Database Maintenance Tool before continuing.");
                DialogResult = DialogResult.Cancel;
                return;
            }
            _eBillCur = _eBillDefault;
            //Set the textboxes to default values.
            textClientAcctNumber.Text = _eBillDefault.ClientAcctNumber;
            textUserName.Text = _eBillDefault.ElectUserName;
            textPassword.Text = _eBillDefault.ElectPassword;
            string[] arrayEbillAddressEnums = Enum.GetNames(typeof(EbillAddress));
            for (int i = 0; i < arrayEbillAddressEnums.Length; i++)
            {
                comboPracticeAddr.Items.Add(arrayEbillAddressEnums[i]);
                comboRemitAddr.Items.Add(arrayEbillAddressEnums[i]);
            }

            comboClinic.Visible = true;
            labelClinic.Visible = true;
            //Bold clinic specific fields.
            groupBoxBilling.Text = Lan.g(this, "Electronic Billing - Bolded fields are clinic specific");
            labelAcctNum.Font = new Font(labelAcctNum.Font, FontStyle.Bold);
            labelUserName.Font = new Font(labelUserName.Font, FontStyle.Bold);
            labelPassword.Font = new Font(labelPassword.Font, FontStyle.Bold);
            labelClinic.Font = new Font(labelClinic.Font, FontStyle.Bold);
            labelPracticeAddr.Font = new Font(labelPracticeAddr.Font, FontStyle.Bold);
            labelRemitAddr.Font = new Font(labelRemitAddr.Font, FontStyle.Bold);
            comboClinic.Items.Clear();
            _listClinics = new List<Clinic>();
            //Add "Unassigned/Default" option if the user isn't restricted or the selected clinic is 0.
            if (!Security.CurrentUser.ClinicRestricted || Clinics.ClinicId == 0)
            {
                Clinic clinicUnassigned = new Clinic();
                clinicUnassigned.Abbr = Lan.g(this, "Unassigned/Default");
                _listClinics.Add(clinicUnassigned);
            }
            _listClinics.AddRange(Clinic.GetByUser(Security.CurrentUser));
            for (int i = 0; i < _listClinics.Count; i++)
            {
                comboClinic.Items.Add(_listClinics[i].Abbr);
                //If the clinic we're adding is the one currently selected in OD, attempt to find its Ebill entry.
                if (_listClinics[i].Id == Clinics.ClinicId)
                {
                    comboClinic.SelectedIndex = i;
                    Ebill eBill = null;
                    if (Clinics.ClinicId == 0)
                    {//Use the default Ebill if OD has Headquarters selected or if clinics are disabled.
                        eBill = _eBillDefault;
                    }
                    else
                    {
                        eBill = _listEbills.FirstOrDefault(x => x.ClinicNum == _listClinics[i].Id);//Can be null.
                    }
                    //_eBillCur will be the default Ebill, the clinic's Ebill, or null if there are no existing ebills for OD's selected clinic.
                    _eBillCur = eBill;
                }
            }

            listModesToText.Items.Clear();
            foreach (StatementMode stateMode in Enum.GetValues(typeof(StatementMode)))
            {
                listModesToText.Items.Add(new ODBoxItem<StatementMode>(Lan.g("enumStatementMode", stateMode.GetDescription()), stateMode));
            }
            foreach (string modeIdx in Preference.GetString(PreferenceName.BillingDefaultsModesToText)
                .Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                listModesToText.SetSelected(PIn.Int(modeIdx), true);
            }
            textSmsTemplate.Text = Preference.GetString(PreferenceName.BillingDefaultsSmsTemplate);
            //Load _eBillCur's fields into the UI.
            LoadEbill(_eBillCur);
        }

        ///<summary>eBill can be null, creates Ebill if needed.</summary>
        private void LoadEbill(Ebill eBill)
        {
            if (eBill == null)
            {//Matching Ebill entry not found.  Make a new entry with default values.
                eBill = new Ebill();
                eBill.ClinicNum = _listClinics[comboClinic.SelectedIndex].Id;
                eBill.ClientAcctNumber = "";
                eBill.ElectUserName = "";
                eBill.ElectPassword = "";
                eBill.PracticeAddress = EbillAddress.PracticePhysical;
                eBill.RemitAddress = EbillAddress.PracticeBilling;
                _listEbills.Add(eBill);
            }
            textClientAcctNumber.Text = _eBillDefault.ClientAcctNumber;
            if (eBill.ClientAcctNumber != "")
            {//If the Ebill field is blank use default value.
                textClientAcctNumber.Text = eBill.ClientAcctNumber;
            }
            textUserName.Text = _eBillDefault.ElectUserName;
            if (eBill.ElectUserName != "")
            {//If the Ebill field is blank use default value.
                textUserName.Text = eBill.ElectUserName;
            }
            textPassword.Text = _eBillDefault.ElectPassword;
            if (eBill.ElectPassword != "")
            {//If the Ebill field is blank use default value.
                textPassword.Text = eBill.ElectPassword;
            }
            //If clinics are disabled and the eBill had a clinic specific enum, set it to default value.  May happen if clinics were previously enabled.

            comboPracticeAddr.SelectedIndex = (int)eBill.PracticeAddress;
            comboRemitAddr.SelectedIndex = (int)eBill.RemitAddress;

            _eBillCur = eBill;
            if (IsUserPassOnly)
            {
                Controls.OfType<Control>().ToList().ForEach(x => x.Enabled = false);
                groupBoxBilling.Enabled = true;
                butOK.Enabled = true;
                butCancel.Enabled = true;
                this.Text = Lan.g(this, "Billing Defaults") + " - {" + Lan.g(this, "Limited") + "}";
            }
        }

        ///<summary>Saves the current Ebill information from the UI into the cache.</summary>
        private void SaveEbill(Ebill eBill)
        {
            if (eBill.ClinicNum == 0)
            {//If the ebill being edited is for the defaults use what's in the text
                eBill.ClientAcctNumber = textClientAcctNumber.Text;
                eBill.ElectUserName = textUserName.Text;
                eBill.ElectPassword = textPassword.Text;
            }
            else
            {//If the ebill isn't the default
                if (textClientAcctNumber.Text != "" && textClientAcctNumber.Text != _eBillDefault.ClientAcctNumber)
                {
                    eBill.ClientAcctNumber = textClientAcctNumber.Text;
                }
                else
                {//Text was blank or the same as the default, blank it.
                    eBill.ClientAcctNumber = "";
                }
                if (textUserName.Text != "" && textUserName.Text != _eBillDefault.ElectUserName)
                {
                    eBill.ElectUserName = textUserName.Text;
                }
                else
                {//Text was blank or the same as the default, blank it.
                    eBill.ElectUserName = "";
                }
                if (textPassword.Text != "" && textPassword.Text != _eBillDefault.ElectPassword)
                {
                    eBill.ElectPassword = textPassword.Text;
                }
                else
                {//Text was blank or the same as the default, blank it.
                    eBill.ElectPassword = "";
                }
            }
            eBill.PracticeAddress = (EbillAddress)comboPracticeAddr.SelectedIndex;
            eBill.RemitAddress = (EbillAddress)comboRemitAddr.SelectedIndex;
        }

        private void textStatementURL_KeyUp(object sender, KeyEventArgs e)
        {
            arrayOutputPaths[listElectBilling.SelectedIndex] = textStatementURL.Text;
        }

        private void listElectBilling_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If Dental X Change is selected, enable its textboxes and combo.
            if (listElectBilling.SelectedIndex == 1)
            {
                comboRemitAddr.Enabled = true;
                textUserName.ReadOnly = false;
                textPassword.ReadOnly = false;
                textClientAcctNumber.ReadOnly = false;
                textVendorId.ReadOnly = false;
                textVendorPMScode.ReadOnly = false;
                labelBlankForDefault.Visible = true;
                labelStatementURL.Text = Lan.g(this, "URL Override");
            }
            else
            {
                //If Dental X Change is not selected, disable changing information for fields the selected format won't use.
                comboRemitAddr.Enabled = false;
                textUserName.ReadOnly = true;
                textPassword.ReadOnly = true;
                textClientAcctNumber.ReadOnly = true;
                textVendorId.ReadOnly = true;
                textVendorPMScode.ReadOnly = true;
                labelBlankForDefault.Visible = false;
                labelStatementURL.Text = Lan.g(this, "Output Path");
            }
            textStatementURL.Text = arrayOutputPaths[listElectBilling.SelectedIndex];
            if (listElectBilling.SelectedIndex == 1 || listElectBilling.SelectedIndex == 3)
            {
                checkCreatePDF.Enabled = true;
            }
            else
            {
                checkCreatePDF.Enabled = false;
            }
        }

        private void comboClinic_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SaveEbill(_eBillCur);
            Ebill eBill = null;
            if ((!Security.CurrentUser.ClinicRestricted || Clinics.ClinicId == 0) && comboClinic.SelectedIndex == 0)
            {//Unassigned/Default
                eBill = _eBillDefault;
            }
            else
            {//Otherwise locate the Ebill from the cache.
                for (int i = 0; i < _listEbills.Count; i++)
                {
                    if (_listEbills[i].ClinicNum == _listClinics[comboClinic.SelectedIndex].Id)
                    {//Check for existing Ebill entry
                        eBill = _listEbills[i];
                        break;
                    }
                }
            }
            LoadEbill(eBill);//Could be null if user switches to a clinic which has not Ebill entry yet.
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            if (textDays.errorProvider1.GetError(textDays) != "")
            {
                MsgBox.Show(this, "Please fix data entry errors first.");
                return;
            }
            if (listElectBilling.SelectedIndex.In(2, 3, 4) && !Directory.Exists(textStatementURL.Text))
            {
                MsgBox.Show(this, "Please choose a valid Output Path.");
                return;
            }
            if (checkSinglePatient.Checked && checkIntermingled.Checked)
            {
                MsgBox.Show(this, "Cannot select both 'Intermingle family members' and 'Single patient only' as defaults.");
                return;
            }
            string cc = "";
            if (checkMC.Checked)
            {
                cc = "MC";
            }
            if (checkV.Checked)
            {
                if (cc != "")
                {
                    cc += ",";
                }
                cc += "V";
            }
            if (checkD.Checked)
            {
                if (cc != "")
                {
                    cc += ",";
                }
                cc += "D";
            }
            if (checkAmEx.Checked)
            {
                if (cc != "")
                {
                    cc += ",";
                }
                cc += "A";
            }
            string billingUseElectronic = listElectBilling.SelectedIndex.ToString();
            SaveEbill(_eBillCur);
            if (listElectBilling.SelectedIndex == 1 && string.IsNullOrEmpty(textStatementURL.Text))
            {
                textStatementURL.Text = @"https://claimconnect.dentalxchange.com/dci/upload.svl";//default value from before 16.2.19
            }
            string modesToText = string.Join(",", listModesToText.SelectedTags<StatementMode>().Select(x => POut.Int((int)x)));
            if (Preference.Update(PreferenceName.BillingDefaultsLastDays, PIn.Long(textDays.Text))
                | Preference.Update(PreferenceName.BillingDefaultsIntermingle, checkIntermingled.Checked)
                | Preference.Update(PreferenceName.BillingDefaultsNote, textNote.Text)
                | Preference.Update(PreferenceName.BillingUseElectronic, billingUseElectronic)
                | Preference.Update(PreferenceName.BillingEmailSubject, textBillingEmailSubject.Text)
                | Preference.Update(PreferenceName.BillingEmailBodyText, textBillingEmailBody.Text)
                | Preference.Update(PreferenceName.BillingElectVendorId, textVendorId.Text)
                | Preference.Update(PreferenceName.BillingElectVendorPMSCode, textVendorPMScode.Text)
                | Preference.Update(PreferenceName.BillingElectCreditCardChoices, cc)
                | Preference.Update(PreferenceName.BillingDefaultsInvoiceNote, textInvoiceNote.Text)
                | Preference.Update(PreferenceName.BillingElectCreatePDF, checkCreatePDF.Checked)
                | (listElectBilling.SelectedIndex == 1 && Preference.Update(PreferenceName.BillingElectStmtUploadURL, textStatementURL.Text))
                | (listElectBilling.SelectedIndex == 2 && Preference.Update(PreferenceName.BillingElectStmtOutputPathPos, textStatementURL.Text))
                | (listElectBilling.SelectedIndex == 3 && Preference.Update(PreferenceName.BillingElectStmtOutputPathClaimX, textStatementURL.Text))
                | (listElectBilling.SelectedIndex == 4 && Preference.Update(PreferenceName.BillingElectStmtOutputPathEds, textStatementURL.Text))
                | Preference.Update(PreferenceName.BillingDefaultsSinglePatient, checkSinglePatient.Checked)
                | Preference.Update(PreferenceName.BillingDefaultsModesToText, modesToText)
                | Preference.Update(PreferenceName.BillingDefaultsSmsTemplate, textSmsTemplate.Text)
                | Preference.Update(PreferenceName.BillingShowTransSinceBalZero, checkBoxBillShowTransSinceZero.Checked))
            {
                DataValid.SetInvalid(InvalidType.Prefs);
            }
            if (Ebills.Sync(_listEbills, _listEbillsOld))
            {//Includes the default Ebill
                DataValid.SetInvalid(InvalidType.Ebills);//Also updates cache.
            }
            DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
