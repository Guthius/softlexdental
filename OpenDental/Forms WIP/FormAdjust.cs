using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Linq;
using CodeBase;

namespace OpenDental
{
    ///<summary></summary>
    public partial class FormAdjust : FormBase
    {
        private Patient patient;
        private Adjustment adjustment;

        ///<summary></summary>
        public bool IsNew;
        private ArrayList PosIndex = new ArrayList();
        private ArrayList NegIndex = new ArrayList();

        ///<summary></summary>
        private DateTime dateLimit = DateTime.MinValue;
        ///<summary>When true, the OK click will not let the user leave the window unless the check amount is 0.</summary>
        private bool _checkZeroAmount;
        ///<summary>All positive adjustment defs.</summary>
        private List<Definition> _listAdjPosCats;
        ///<summary>All negative adjustment defs.</summary>
        private List<Definition> _listAdjNegCats;
        ///<summary>Cached list of clinics available to user. Also includes a dummy Clinic at index 0 for "none".</summary>
        private List<Clinic> _listClinics;
        ///<summary>Filtered list of providers based on which clinic is selected. If no clinic is selected displays all providers. Also includes a dummy clinic at index 0 for "none"</summary>
        private List<Provider> _listProviders;
        ///<summary>Used to keep track of the current clinic selected. This is because it may be a clinic that is not in _listClinics.</summary>
        private long _selectedClinicNum;
        ///<summary>Instead of relying on _listProviders[comboProv.SelectedIndex] to determine the selected Provider we use this variable to store it explicitly.</summary>
        private long _selectedProvNum;
        private decimal _adjRemAmt;
        private bool _isTsiAdj;
        private bool _isEditAnyway;

        public FormAdjust(Patient patient, Adjustment adjustment, bool isTsiAdj = false)
        {
            InitializeComponent();
            this.patient = patient;
            this.adjustment = adjustment;
            _isTsiAdj = isTsiAdj;
        }

        private void FormAdjust_Load(object sender, EventArgs e)
        {
            if (AvaTax.IsEnabled())
            {
                // We do not want to allow the user to make edits or delete SalesTax and SalesTaxReturn Adjustments.
                // Popup if no permission so user knows why disabled.
                if (AvaTax.IsEnabled() && (adjustment.AdjType == AvaTax.SalesTaxAdjType || adjustment.AdjType == AvaTax.SalesTaxReturnAdjType) && !Security.IsAuthorized(Permissions.SalesTaxAdjEdit))
                {
                    DisableForm(textNote, cancelButton);

                    textNote.ReadOnly = true; // This will allow the user to copy the note if desired.
                }
            }

            if (IsNew)
            {
                if (!Security.IsAuthorized(Permissions.AdjustmentCreate, true))
                {
                    if (!Security.IsAuthorized(Permissions.AdjustmentEditZero, true)) //Let user create an adjustment of zero if they have this perm.
                    {
                        MessageBox.Show("Not authorized for\r\n" + UserGroupPermission.GetDescription(Permissions.AdjustmentCreate));

                        DialogResult = DialogResult.Cancel;

                        return;
                    }
                    
                    //Make sure amount is 0 after OK click.
                    _checkZeroAmount = true;
                }
            }
            else
            {
                if (!Security.IsAuthorized(Permissions.AdjustmentEdit, adjustment.AdjDate))
                {
                    acceptButton.Enabled = false;
                    deleteButton.Enabled = false;

                    // User can't edit but has edit zero amount perm.  Allow delete only if date is today.
                    if (Security.IsAuthorized(Permissions.AdjustmentEditZero, true)
                        && adjustment.AdjAmt == 0
                        && adjustment.DateEntry.Date == MiscData.GetNowDateTime().Date)
                    {
                        deleteButton.Enabled = true;
                    }
                }


                List<PaySplit> listSplits = PaySplits.GetForAdjustments(new List<long>() { adjustment.Id });
                if (listSplits.Count > 0)
                {
                    butAttachProc.Enabled = false;
                    butDetachProc.Enabled = false;
                    labelProcDisabled.Visible = true;//Do we want to somehow give info on which payments are on this adjustment?
                }
                //Do not let the user change the adjustment type if the current adjustment is a "discount plan" adjustment type.
                if (Defs.GetValue(DefinitionCategory.AdjTypes, adjustment.AdjType) == "dp")
                {
                    labelAdditions.Text = Lan.g(this, "Discount Plan") + ": " + Defs.GetName(DefinitionCategory.AdjTypes, adjustment.AdjType);
                    labelSubtractions.Visible = false;
                    listTypePos.Visible = false;
                    listTypeNeg.Visible = false;
                }
            }
            textDateEntry.Text = adjustment.DateEntry.ToShortDateString();
            textAdjDate.Text = adjustment.AdjDate.ToShortDateString();
            textProcDate.Text = adjustment.ProcDate.ToShortDateString();
            if (Defs.GetValue(DefinitionCategory.AdjTypes, adjustment.AdjType) == "+")
            {//pos
                textAmount.Text = adjustment.AdjAmt.ToString("F");
            }
            else if (Defs.GetValue(DefinitionCategory.AdjTypes, adjustment.AdjType) == "-")
            {//neg
                textAmount.Text = (-adjustment.AdjAmt).ToString("F");//shows without the neg sign
            }
            else if (Defs.GetValue(DefinitionCategory.AdjTypes, adjustment.AdjType) == "dp")
            {//Discount Plan (neg)
                textAmount.Text = (-adjustment.AdjAmt).ToString("F");//shows without the neg sign
            }
                _listClinics = new List<Clinic>() { new Clinic() { Abbr = Lan.g(this, "None") } }; //Seed with "None"
                Clinic.GetByUser(Security.CurrentUser).ForEach(x => _listClinics.Add(x));//do not re-organize from cache. They could either be alphabetizeded or sorted by item order.
                _listClinics.ForEach(x => comboClinic.Items.Add(x.Abbr));
                _selectedClinicNum = adjustment.ClinicNum;
                comboClinic.IndexSelectOrSetText(_listClinics.FindIndex(x => x.Id == _selectedClinicNum), () => { return Clinic.GetById(_selectedClinicNum).Abbr; });
            
            _selectedProvNum = adjustment.ProvNum;
            comboProv.SelectedIndex = -1;
            FillComboProvHyg();
            if (adjustment.ProcNum != 0 && Preference.GetInt(PreferenceName.RigorousAdjustments) == (int)RigorousAdjustments.EnforceFully)
            {
                comboProv.Enabled = false;
                butPickProv.Enabled = false;
                comboClinic.Enabled = false;
                if (Security.IsAuthorized(Permissions.Setup, true))
                {
                    labelEditAnyway.Visible = true;
                    butEditAnyway.Visible = true;
                }
            }
            //prevents FillProcedure from being called too many times.  Event handlers hooked back up after the lists are filled.
            listTypeNeg.SelectedIndexChanged -= listTypeNeg_SelectedIndexChanged;
            listTypePos.SelectedIndexChanged -= listTypePos_SelectedIndexChanged;
            List<Definition> adjCat = Definition.GetByCategory(DefinitionCategory.AdjTypes);;
            //Positive adjustment types
            _listAdjPosCats = adjCat.FindAll(x => x.Value == "+");
            _listAdjPosCats.ForEach(x => listTypePos.Items.Add(x.Description));
            listTypePos.SelectedIndex = _listAdjPosCats.FindIndex(x => x.Id == adjustment.AdjType);//can be -1
                                                                                                           //Negative adjustment types
            _listAdjNegCats = adjCat.FindAll(x => x.Value == "-");
            _listAdjNegCats.ForEach(x => listTypeNeg.Items.Add(x.Description));
            listTypeNeg.SelectedIndex = _listAdjNegCats.FindIndex(x => x.Id == adjustment.AdjType);//can be -1
            listTypeNeg.SelectedIndexChanged += listTypeNeg_SelectedIndexChanged;
            listTypePos.SelectedIndexChanged += listTypePos_SelectedIndexChanged;
            FillProcedure();
            textNote.Text = adjustment.AdjNote;
        }

        private void listTypePos_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listTypePos.SelectedIndex > -1)
            {
                listTypeNeg.SelectedIndex = -1;
                FillProcedure();
            }
        }

        private void listTypeNeg_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listTypeNeg.SelectedIndex > -1)
            {
                listTypePos.SelectedIndex = -1;
                FillProcedure();
            }
        }

        private void textAmount_Validating(object sender, CancelEventArgs e)
        {
            FillProcedure();
        }

        private void butPickProv_Click(object sender, EventArgs e)
        {
            FormProviderPick FormPP = new FormProviderPick(_listProviders);
            FormPP.SelectedProvNum = _selectedProvNum;
            FormPP.ShowDialog();
            if (FormPP.DialogResult != DialogResult.OK)
            {
                return;
            }
            _selectedProvNum = FormPP.SelectedProvNum;
            comboProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.Id == _selectedProvNum), () => { return Providers.GetAbbr(_selectedProvNum); });
        }

        private void comboClinic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboClinic.SelectedIndex > -1)
            {
                _selectedClinicNum = _listClinics[comboClinic.SelectedIndex].Id;
            }
            FillComboProvHyg();
        }

        private void comboProv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboProv.SelectedIndex > -1)
            {
                _selectedProvNum = _listProviders[comboProv.SelectedIndex].Id;
            }
        }

        ///<summary>Fills combo provider based on which clinic is selected and attempts to preserve provider selection if any.</summary>
        private void FillComboProvHyg()
        {
            if (comboProv.SelectedIndex > -1)
            {//valid prov selected, not none or nothing.
                _selectedProvNum = _listProviders[comboProv.SelectedIndex].Id;
            }
            _listProviders = Providers.GetProvsForClinic(_selectedClinicNum);
            comboProv.Items.Clear();
            _listProviders.ForEach(x => comboProv.Items.Add(x.Abbr));
            comboProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.Id == _selectedProvNum), () => { return Providers.GetAbbr(_selectedProvNum); });
        }

        private void FillProcedure()
        {
            if (adjustment.ProcNum == 0)
            {
                textProcDate2.Text = "";
                textProcProv.Text = "";
                textProcTooth.Text = "";
                textProcDescription.Text = "";
                textProcFee.Text = "";
                textProcWriteoff.Text = "";
                textProcInsPaid.Text = "";
                textProcInsEst.Text = "";
                textProcAdj.Text = "";
                textProcPatPaid.Text = "";
                textProcAdjCur.Text = "";
                labelProcRemain.Text = "";
                _adjRemAmt = 0;
                return;
            }
            Procedure procCur = Procedures.GetOneProc(adjustment.ProcNum, false);
            List<ClaimProc> listClaimProcs = ClaimProcs.Refresh(procCur.PatNum);
            List<Adjustment> listAdjustments = Adjustments.Refresh(procCur.PatNum)
                .Where(x => x.ProcNum == procCur.ProcNum && x.Id != adjustment.Id).ToList();
            textProcDate.Text = procCur.ProcDate.ToShortDateString();
            textProcDate2.Text = procCur.ProcDate.ToShortDateString();
            textProcProv.Text = Providers.GetAbbr(procCur.ProvNum);
            textProcTooth.Text = Tooth.ToInternat(procCur.ToothNum);
            textProcDescription.Text = ProcedureCodes.GetProcCode(procCur.CodeNum).Descript;
            double procWO = -ClaimProcs.ProcWriteoff(listClaimProcs, procCur.ProcNum);
            double procInsPaid = -ClaimProcs.ProcInsPay(listClaimProcs, procCur.ProcNum);
            double procInsEst = -ClaimProcs.ProcEstNotReceived(listClaimProcs, procCur.ProcNum);
            double procAdj = listAdjustments.Sum(x => x.AdjAmt);
            double procPatPaid = -PaySplits.GetTotForProc(procCur);
            textProcFee.Text = procCur.ProcFeeTotal.ToString("F");
            textProcWriteoff.Text = procWO == 0 ? "" : procWO.ToString("F");
            textProcInsPaid.Text = procInsPaid == 0 ? "" : procInsPaid.ToString("F");
            textProcInsEst.Text = procInsEst == 0 ? "" : procInsEst.ToString("F");
            textProcAdj.Text = procAdj == 0 ? "" : procAdj.ToString("F");
            textProcPatPaid.Text = procPatPaid == 0 ? "" : procPatPaid.ToString("F");
            //Intelligently sum the values above based on statuses instead of blindly adding all of the values together.
            //The remaining amount is typically called the "patient portion" so utilze the centralized method that gets the patient portion.
            decimal patPort = ClaimProcs.GetPatPortion(procCur, listClaimProcs, listAdjustments);
            double procAdjCur = 0;
            if (textAmount.errorProvider1.GetError(textAmount) == "")
            {
                if (listTypePos.SelectedIndex > -1)
                {//pos
                    procAdjCur = PIn.Double(textAmount.Text);
                }
                else if (listTypeNeg.SelectedIndex > -1 || Defs.GetValue(DefinitionCategory.AdjTypes, adjustment.AdjType) == "dp")
                {//neg or discount plan
                    procAdjCur = -PIn.Double(textAmount.Text);
                }
            }
            textProcAdjCur.Text = procAdjCur == 0 ? "" : procAdjCur.ToString("F");
            //Add the current adjustment amount to the patient portion which will give the newly calculated remaining amount.
            _adjRemAmt = (decimal)procAdjCur + patPort;
            labelProcRemain.Text = _adjRemAmt.ToString("c");
        }

        private void butAttachProc_Click(object sender, System.EventArgs e)
        {
            FormProcSelect FormPS = new FormProcSelect(adjustment.PatNum, false);
            FormPS.ShowDialog();
            if (FormPS.DialogResult != DialogResult.OK)
            {
                return;
            }
            if (Preference.GetInt(PreferenceName.RigorousAdjustments) < 2)
            {//Enforce Linking
                _selectedProvNum = FormPS.ListSelectedProcs[0].ProvNum;
                _selectedClinicNum = FormPS.ListSelectedProcs[0].ClinicNum;
                comboProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.Id == _selectedProvNum), () => { return Providers.GetAbbr(_selectedProvNum); });
                comboClinic.IndexSelectOrSetText(_listClinics.FindIndex(x => x.Id == _selectedClinicNum), () => { return Clinic.GetById(_selectedClinicNum).Abbr; });
                if (Preference.GetInt(PreferenceName.RigorousAdjustments) == (int)RigorousAdjustments.EnforceFully && !_isEditAnyway)
                {
                    if (Security.IsAuthorized(Permissions.Setup, true))
                    {
                        labelEditAnyway.Visible = true;
                        butEditAnyway.Visible = true;
                    }
                    comboProv.Enabled = false;//Don't allow changing if enforce fully
                    butPickProv.Enabled = false;
                    comboClinic.Enabled = false;
                }
            }
            adjustment.ProcNum = FormPS.ListSelectedProcs[0].ProcNum;
            FillProcedure();
            textProcDate.Text = FormPS.ListSelectedProcs[0].ProcDate.ToShortDateString();
        }

        private void butDetachProc_Click(object sender, System.EventArgs e)
        {
            comboProv.Enabled = true;
            butPickProv.Enabled = true;
            comboClinic.Enabled = true;
            labelEditAnyway.Visible = false;
            butEditAnyway.Visible = false;
            adjustment.ProcNum = 0;
            FillProcedure();
        }

        private void butEditAnyway_Click(object sender, EventArgs e)
        {
            _isEditAnyway = true;
            comboClinic.Enabled = true;
            comboProv.Enabled = true;
            butPickProv.Enabled = true;
            labelEditAnyway.Visible = false;
            butEditAnyway.Visible = false;
        }

        private void butOK_Click(object sender, System.EventArgs e)
        {
            bool isDiscountPlanAdj = (Defs.GetValue(DefinitionCategory.AdjTypes, adjustment.AdjType) == "dp");
            if (textAdjDate.errorProvider1.GetError(textAdjDate) != ""
                || textProcDate.errorProvider1.GetError(textProcDate) != ""
                || textAmount.errorProvider1.GetError(textAmount) != "")
            {
                MsgBox.Show(this, "Please fix data entry errors first.");
                return;
            }
            if (PIn.Date(textAdjDate.Text).Date > DateTime.Today.Date && !Preference.GetBool(PreferenceName.FutureTransDatesAllowed))
            {
                MsgBox.Show(this, "Adjustment date can not be in the future.");
                return;
            }
            if (textAmount.Text == "")
            {
                MessageBox.Show(Lan.g(this, "Please enter an amount."));
                return;
            }
            if (!isDiscountPlanAdj && listTypeNeg.SelectedIndex == -1 && listTypePos.SelectedIndex == -1)
            {
                MsgBox.Show(this, "Please select a type first.");
                return;
            }
            if (IsNew && AvaTax.IsEnabled() && listTypePos.SelectedIndex > -1 &&
                (_listAdjPosCats[listTypePos.SelectedIndex].Id == AvaTax.SalesTaxAdjType || _listAdjPosCats[listTypePos.SelectedIndex].Id == AvaTax.SalesTaxReturnAdjType) &&
                !Security.IsAuthorized(Permissions.SalesTaxAdjEdit))
            {
                return;
            }
            if (Preference.GetInt(PreferenceName.RigorousAdjustments) == 0 && adjustment.ProcNum == 0)
            {
                MsgBox.Show(this, "You must attach a procedure to the adjustment.");
                return;
            }
            if (_adjRemAmt < 0)
            {
                if (!MsgBox.Show(this, MsgBoxButtons.OKCancel, "Remaining amount is negative.  Continue?", "Overpaid Procedure Warning"))
                {
                    return;
                }
            }
            bool changeAdjSplit = false;
            List<PaySplit> listPaySplitsForAdjust = new List<PaySplit>();
            if (IsNew)
            {
                //prevents backdating of initial adjustment
                if (!Security.IsAuthorized(Permissions.AdjustmentCreate, PIn.Date(textAdjDate.Text), true))
                {//Give message later.
                    if (!_checkZeroAmount)
                    {//Let user create as long as Amount is zero and has edit zero permissions.  This was checked on load.
                        MessageBox.Show(Lans.g("Security", "Not authorized for") + "\r\n" + UserGroupPermission.GetDescription(Permissions.AdjustmentCreate));
                        return;
                    }
                }
            }
            else
            {
                //Editing an old entry will already be blocked if the date was too old, and user will not be able to click OK button
                //This catches it if user changed the date to be older.
                if (!Security.IsAuthorized(Permissions.AdjustmentEdit, PIn.Date(textAdjDate.Text)))
                {
                    return;
                }
                if (adjustment.ProvNum != _selectedProvNum)
                {
                    listPaySplitsForAdjust = PaySplits.GetForAdjustments(new List<long>() { adjustment.Id });
                    foreach (PaySplit paySplit in listPaySplitsForAdjust)
                    {
                        if (!Security.IsAuthorized(Permissions.PaymentEdit, Payments.GetPayment(paySplit.PayNum).PayDate))
                        {
                            return;
                        }
                        if (_selectedProvNum != paySplit.ProvNum && Preference.GetInt(PreferenceName.RigorousAccounting) == (int)RigorousAdjustments.EnforceFully)
                        {
                            changeAdjSplit = true;
                            break;
                        }
                    }
                    if (changeAdjSplit
                        && !MsgBox.Show(this, MsgBoxButtons.OKCancel, "The provider for the associated payment splits will be changed to match the provider on the "
                        + "adjustment."))
                    {
                        return;
                    }
                }
            }
            //DateEntry not allowed to change
            DateTime datePreviousChange = adjustment.SecDateTEdit;
            adjustment.AdjDate = PIn.Date(textAdjDate.Text);
            adjustment.ProcDate = PIn.Date(textProcDate.Text);
            adjustment.ProvNum = _selectedProvNum;
            adjustment.ClinicNum = _selectedClinicNum;
            if (listTypePos.SelectedIndex != -1)
            {
                adjustment.AdjType = _listAdjPosCats[listTypePos.SelectedIndex].Id;
                adjustment.AdjAmt = PIn.Double(textAmount.Text);
            }
            if (listTypeNeg.SelectedIndex != -1)
            {
                adjustment.AdjType = _listAdjNegCats[listTypeNeg.SelectedIndex].Id;
                adjustment.AdjAmt = -PIn.Double(textAmount.Text);
            }
            if (isDiscountPlanAdj)
            {
                //AdjustmentCur.AdjType is already set to a "discount plan" adj type.
                adjustment.AdjAmt = -PIn.Double(textAmount.Text);
            }
            if (_checkZeroAmount && adjustment.AdjAmt != 0)
            {
                MsgBox.Show(this, "Amount has to be 0.00 due to security permission.");
                return;
            }
            adjustment.AdjNote = textNote.Text;
            try
            {
                if (IsNew)
                {
                    Adjustments.Insert(adjustment);
                    SecurityLog.Write(Permissions.AdjustmentCreate, adjustment.PatNum,
                        patient.GetNameLF() + ", "
                        + adjustment.AdjAmt.ToString("c"));
                    TsiTransLogs.CheckAndInsertLogsIfAdjTypeExcluded(adjustment, patient.Guarantor, patient.ClinicNum, _isTsiAdj);
                }
                else
                {
                    Adjustments.Update(adjustment);
                    SecurityLog.Write(Permissions.AdjustmentEdit, adjustment.PatNum, patient.GetNameLF() + ", " + adjustment.AdjAmt.ToString("c"), 0
                        , datePreviousChange);
                }
            }
            catch (Exception ex)
            {//even though it doesn't currently throw any exceptions
                MessageBox.Show(ex.Message);
                return;
            }
            if (changeAdjSplit)
            {
                PaySplits.UpdateProvForAdjust(adjustment, listPaySplitsForAdjust);
            }
            DialogResult = DialogResult.OK;
        }

        void DeleteButton_Click(object sender, System.EventArgs e)
        {
            if (IsNew)
            {
                DialogResult = DialogResult.Cancel;

                return;
            }

            SecurityLog.Write(
                Permissions.AdjustmentEdit, adjustment.PatNum, 
                "Delete for patient: " + patient.GetNameLF() + ", " + adjustment.AdjAmt.ToString("c"), 
                0, 
                adjustment.SecDateTEdit);

            Adjustments.Delete(adjustment);

            DialogResult = DialogResult.OK;
        }
    }
}