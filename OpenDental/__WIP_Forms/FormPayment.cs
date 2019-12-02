/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using CodeBase;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.Rendering.Printing;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.WebTypes.Shared.XWeb;
using PdfSharp.Pdf;
using SLDental.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace OpenDental
{
    ///<summary></summary>
    public partial class FormPayment : ODForm
    {
        ///<summary></summary>
        public bool IsNew = false;
        ///<summary>Set this value to a PaySplitNum if you want one of the splits highlighted when opening this form.</summary>
        public long InitialPaySplitNum;
        private Patient _patCur;
        private Payment _paymentCur;
        ///<summary>A current list of splits showing on the left grid.  Public for unit tests.</summary> // Getting Rid of this. Shouldn't need to be public. 
        private List<PaySplit> _listSplitsCur = new List<PaySplit>();
        ///<summary>The original splits that existed when this window was opened.  Empty for new payments.</summary>
        private List<PaySplit> _listPaySplitsOld;
        //private double _splitTotal=0;
        private long[] _arrayDepositAcctNums;
        ///<summary>This table gets created and filled once at the beginning.  After that, only the last column gets carefully updated.</summary>
        private DataTable _tableBalances;
        ///<summary>Program X-Charge.</summary>
        private Program _xProg;
        ///<summary>The local override path or normal path for X-Charge.</summary>
        private string _xPath;
        ///<summary>Stored CreditCards for _patCur.</summary>
        private List<CreditCard> _listCreditCards;
        ///<summary>Set to true when X-Charge or PayConnect makes a successful transaction, except for voids.</summary>
        private bool _wasCreditCardSuccessful;
        //private PayConnectService.creditCardRequest _payConnectRequest;
        //private PaySimple.ApiResponse _paySimpleResponse;
        private System.Drawing.Printing.PrintDocument _pd2;
        private Payment _paymentOld;
        private bool _promptSignature;
        private bool _printReceipt;
        ///<summary>Local cache of all of the clinic nums the current user has permission to access at the time the form loads.  Filled at the same time
        ///as comboClinic and is used to set payment.ClinicNum when saving.</summary>
        private List<long> _listUserClinicNums;
        private bool _isCCDeclined;
        ///<summary>Set to a positive amount if there is an unearned amount for the patient and they want to use it.</summary>
        public double UnearnedAmt;
        ///<summary>Used to track position inside the MakeXChargeTransaction(), for troubleshooting purposes.</summary>
        private string _xChargeMilestone;
        private List<PayPlan> _listValidPayPlans;
        private List<PaySplit> _listPaySplitAllocations;
        ///<summary>List of current account charges for the family.  Gets filled from AutoSplitForPayment</summary>
        private List<AccountEntry> _listAccountCharges;
        ///<summary>The amount entered for the current payment.  Amount currently available for paying off charges.
        ///If this value is zero, it will be set to the summation of the split amounts when OK is clicked.</summary>
        public decimal AmtTotal;
        ///<summary>A dictionary or patients that we may need to reference to fill the grids to eliminate unnecessary calls to the DB.
        ///Should contain all patients in the current family along with any patients of payment plans of which a member of this family is the guarantor.</summary>
        private Dictionary<long, Patient> _dictPatients;
        ///<summary>Set to true if this payment is supposed to be an income transfer.</summary>
        public bool IsIncomeTransfer;
        ///<summary>The XWebResponse that created this payment. Will only be set if the payment originated from XWeb..</summary>
        private XWebResponse _xWebResponse;
        ///<summary>List of select procedures sent by the ContrAccount.If this list contains any procedures, they will be paid off first.</summary>
        //private List<Procedure> ListProcs;
        ///<summary>When making income transfer splits, PaySplitAssociated objects are used.  It keeps track of split associations until we insert into
        ///the database.  PaySplitOrig is the PaySplit the PaySplitLinked will "link to" with its FSplitNum.</summary>
        private List<PaySplits.PaySplitAssociated> _listPaySplitsAssociated;
        /// <summary>Direct family members of the current patient.</summary>
        private readonly Family _famCur;
        /// <summary>Superfamily of the current patient, if one exists.</summary>
        private Family _superFamCur;
        private List<Definition> _listPaymentTypeDefs;

        private bool _isInit;
        private int _originalHeight;
        private bool _preferCurrentPat;











        ///<summary>Procedures and payplan charges from account module we want to make splits for on this payment.</summary>
        public List<AccountEntry> ListEntriesPayFirst;

        ///<summary>Holds most all the data needed to load the form.</summary>
        private PaymentEdit.LoadData _loadData;

        ///<summary>Returns either the family or super family of the current patients 
        ///depending on whether or not the "Show Charges for Superfamily" checkbox is checked.</summary>
        private Family _curFamOrSuperFam
        {
            get
            {
                if (checkShowSuperfamily.Checked)
                {
                    return _superFamCur;
                }
                else
                {
                    return _famCur;
                }
            }
        }

        ///<summary>The list of patnums in _currentFamily, either the superfamily or the regular family depending on the superfam checkbox state.</summary>
        private List<long> _listPatNums
        {
            get
            {
                return _curFamOrSuperFam.Members.Select(x => x.PatNum).ToList();
            }
        }

        public string XchargeMilestone
        {
            get
            {
                return _xChargeMilestone;
            }
        }

        /// <summary>List of selected patnums to filter outstanding charges grid on.</summary>
        private List<long> _listFilteredPatNums
        {
            get
            {
                //Get filtered patients
                if (comboPatientFilter.ListSelectedIndices.Contains(0))
                {//Contains "All"
                    return comboPatientFilter.Items.Cast<ODBoxItem<Patient>>().Where(x => x.Tag != null).Select(x => x.Tag.PatNum).ToList();
                }
                else
                {
                    return comboPatientFilter.ListSelectedItems.Cast<ODBoxItem<Patient>>().Where(x => x.Tag != null).Select(x => x.Tag.PatNum).ToList();
                }
            }
        }
        /// <summary>List of selected provider nums to filter outstanding charges grid on.</summary>
        private List<long> _listFilteredProvNums
        {
            get
            {
                //get filtered providers
                if (comboProviderFilter.ListSelectedIndices.Contains(0))
                {//Contains "All"
                    return comboProviderFilter.Items.Cast<ODBoxItem<Provider>>().Where(x => x.Tag != null).Select(x => x.Tag.Id).ToList();
                }
                else
                {
                    return comboProviderFilter.ListSelectedItems.Cast<ODBoxItem<Provider>>().Where(x => x.Tag != null).Select(x => x.Tag.Id).ToList();
                }
            }
        }
        /// <summary>List of selected clinic nums to filter outstanding charges grid on.</summary>
        private List<long> _listFilteredClinics
        {
            get
            {
                //Get filtered clinics
                if (comboClinicFilter.ListSelectedIndices.Contains(0))
                {//Contains "All"
                    return comboClinicFilter.Items.Cast<ODBoxItem<Clinic>>().Where(x => x.Tag != null).Select(x => x.Tag.Id).ToList();
                }
                else
                {
                    return comboClinicFilter.ListSelectedItems.Cast<ODBoxItem<Clinic>>().Where(x => x.Tag != null).Select(x => x.Tag.Id).ToList();
                }
            }
        }
        /// <summary>List of user-inputed proc codes to filter outstanding charges grid on.</summary>
        private List<long> _listFilteredProcCodes
        {
            get
            {
                List<long> listFilteredProcCodes = new List<long>();
                //Proc codes
                List<string> listCodes = textFilterProcCodes.Text.Split(new char[] { ',' }).ToList();
                foreach (string code in listCodes)
                {
                    long retrievedCode = ProcedureCodes.GetCodeNum(code.Trim());  //returns 0 if code not found
                    if (retrievedCode != 0)
                    {
                        listFilteredProcCodes.Add(retrievedCode);
                    }
                }
                return listFilteredProcCodes;
            }
        }
        /// <summary>List of selected charge types to filter outstanding charges grid on: PaySplit, PayPlan Charge, Adjustment, Procedure.</summary>
        private List<string> _listFilteredType
        {
            get
            {
                List<string> listTypes = new List<string>();
                //Get filtered types
                if (comboTypeFilter.ListSelectedIndices.Contains(0))
                {//'All' is selected
                    for (int i = 1; i < comboTypeFilter.Items.Count; i++)
                    {//Starts at 1 to not include 'All'
                        listTypes.Add(comboTypeFilter.Items[i].ToString());
                    }
                }
                else
                {
                    //Find all selected types
                    for (int i = 0; i < comboTypeFilter.ListSelectedItems.Count; i++)
                    {
                        listTypes.Add(comboTypeFilter.ListSelectedItems[i].ToString());
                    }
                }
                return listTypes;
            }
        }

        ///<summary>PatCur and FamCur are not for the PatCur of the payment.  They are for the patient and family from which this window was accessed.
        ///Use listSelectedProcs to automatically attach payment to specific procedures.</summary>
        public FormPayment(Patient patCur, Family famCur, Payment paymentCur, bool preferCurrentPat)
        {
            InitializeComponent();

            _patCur = patCur;
            _famCur = famCur;
            _paymentCur = paymentCur;
            _preferCurrentPat = preferCurrentPat;

            panelXcharge.ContextMenu = contextMenuXcharge;
            butPayConnect.ContextMenu = contextMenuPayConnect;
            butPaySimple.ContextMenu = contextMenuPaySimple;
            _paymentOld = paymentCur.Clone();
        }

        private void FormPayment_Load(object sender, EventArgs e)
        {
            _loadData = PaymentEdit.GetLoadData(_patCur, _paymentCur, _listPatNums, IsNew, (IsIncomeTransfer || _paymentCur.PayType == 0));
            _superFamCur = _loadData.SuperFam;
            if (IsNew)
            {
                checkPayTypeNone.Enabled = true;
                if (!Security.IsAuthorized(Permissions.PaymentCreate))
                {//date not checked here
                    DialogResult = DialogResult.Cancel;
                    return;
                }
                butDeletePayment.Enabled = false;
            }
            else
            {
                checkPayTypeNone.Enabled = false;
                checkRecurring.Checked = _paymentCur.IsRecurringCC;
                if (!Security.IsAuthorized(Permissions.PaymentEdit, _paymentCur.PayDate))
                {
                    butOK.Enabled = false;
                    butDeletePayment.Enabled = false;
                    butAddManual.Enabled = false;
                    gridSplits.Enabled = false;
                    butPay.Enabled = false;
                    butCreatePartial.Enabled = false;
                    butClear.Enabled = false;
                    butDelete.Enabled = false;
                    checkRecurring.Enabled = false;
                    panelXcharge.Enabled = false;
                    butPayConnect.Enabled = false;
                    butPaySimple.Enabled = false;
                    if (Security.IsAuthorized(Permissions.SplitCreatePastLockDate, true))
                    {
                        //Since we are enabling the OK button, we need to make sure everything else is disabled (except for Add).
                        butOK.Enabled = true;
                        butAddManual.Enabled = true;
                        comboClinic.Enabled = false;
                        textDate.ReadOnly = true;
                        textAmount.ReadOnly = true;
                        butPrePay.Enabled = false;
                        textCheckNum.ReadOnly = true;
                        textBankBranch.ReadOnly = true;
                        textNote.ReadOnly = true;
                        checkPayTypeNone.Enabled = false;
                        listPayType.Enabled = false;
                        comboDepositAccount.Enabled = false;
                        comboCreditCards.Enabled = false;
                        checkProcessed.Enabled = false;
                        gridSplits.Enabled = true;
                    }
                }
            }

            _listUserClinicNums = new List<long>();
            List<Clinic> listClinics = Clinic.GetByUser(Security.CurrentUser).ToList();
            comboClinic.Items.Clear();
            comboClinic.Items.Add(Lan.g(this, "None"));
            _listUserClinicNums.Add(0);//this way both lists have the same number of items in it
            comboClinic.SelectedIndex = 0;
            for (int i = 0; i < listClinics.Count; i++)
            {
                comboClinic.Items.Add(listClinics[i].Abbr);
                _listUserClinicNums.Add(listClinics[i].Id);
                if (listClinics[i].Id == _paymentCur.ClinicNum)
                {
                    comboClinic.SelectedIndex = i + 1;
                }
            }

            if (_paymentCur.ProcessStatus == ProcessStat.OfficeProcessed)
            {
                checkProcessed.Visible = false;//This checkbox will only show if the payment originated online.
            }
            else if (_paymentCur.ProcessStatus == ProcessStat.OnlineProcessed)
            {
                checkProcessed.Checked = true;
            }
            _listCreditCards = _loadData.ListCreditCards;
            FillCreditCards();
            _tableBalances = _loadData.TableBalances;
            //this works even if patient not in family
            textPaidBy.Text = _curFamOrSuperFam.GetNameInFamFL(_paymentCur.PatNum);
            textDateEntry.Text = _paymentCur.DateEntry.ToShortDateString();
            textDate.Text = _paymentCur.PayDate.ToShortDateString();
            textAmount.Text = _paymentCur.PayAmt.ToString("F");
            textCheckNum.Text = _paymentCur.CheckNum;
            textBankBranch.Text = _paymentCur.BankBranch;
            _listPaymentTypeDefs = Definition.GetByCategory(DefinitionCategory.PaymentTypes);
            for (int i = 0; i < _listPaymentTypeDefs.Count; i++)
            {
                listPayType.Items.Add(_listPaymentTypeDefs[i].Description);
                if (IsNew && Preference.GetBool(PreferenceName.PaymentsPromptForPayType))
                {//skip auto selecting payment type if preference is enabled and payment is new
                    continue;//user will be forced to selectan indexbefore closing or clicking ok
                }
                if (_listPaymentTypeDefs[i].Id == _paymentCur.PayType)
                {
                    listPayType.SelectedIndex = i;
                }
            }
            textNote.Text = _paymentCur.PayNote;
            Deposit deposit = null;
            if (_paymentCur.DepositNum != 0)
            {
                deposit = Deposits.GetOne(_paymentCur.DepositNum);
            }
            if (deposit == null)
            {//If there was none or it got deleted, disable controls.
                labelDeposit.Visible = false;
                textDeposit.Visible = false;
            }
            else
            {
                textDeposit.Text = deposit.DateDeposit.ToShortDateString();
                textAmount.ReadOnly = true;
                textAmount.BackColor = SystemColors.Control;
                butPay.Enabled = false;
            }
            _listSplitsCur = _loadData.ListSplits;//Count might be 0
            _listPaySplitsOld = new List<PaySplit>();
            _listPaySplitsAssociated = new List<PaySplits.PaySplitAssociated>();
            for (int i = 0; i < _listSplitsCur.Count; i++)
            {
                _listPaySplitsOld.Add(_listSplitsCur[i].Copy());
            }
            _listPaySplitAllocations = _loadData.ListPaySplitAllocations;
            if (_listPaySplitAllocations.Count == 0)
            {
                tabControlSplits.TabPages.Remove(tabPageAllocated);
            }
            FillListPaySplitAssociated();
            if (IsNew && UnearnedAmt > 0)
            {
                List<PaySplits.PaySplitAssociated> listUnearnedPayAssociated = PaymentEdit.AllocateUnearned(ListEntriesPayFirst, ref _listSplitsCur
                    , _paymentCur, UnearnedAmt, _curFamOrSuperFam);
                if (listUnearnedPayAssociated.Count > 0)
                {
                    _listPaySplitsAssociated.AddRange(listUnearnedPayAssociated);
                }
            }
            if (IsNew)
            {
                //Fill comboDepositAccount based on autopay for listPayType.SelectedIndex
                SetComboDepositAccounts();
                textDepositAccount.Visible = false;
            }
            else
            {
                //put a description in the textbox.  If the user clicks on the same or another item in listPayType,
                //then the textbox will go away, and be replaced by comboDepositAccount.
                labelDepositAccount.Visible = false;
                comboDepositAccount.Visible = false;
                Transaction trans = _loadData.Transaction;
                if (trans == null)
                {
                    textDepositAccount.Visible = false;
                }
                else
                {
                    //add only the description based on PaymentCur attached to transaction
                    List<JournalEntry> jeL = JournalEntries.GetForTrans(trans.TransactionNum);
                    for (int i = 0; i < jeL.Count; i++)
                    {
                        Account account = Account.GetById(jeL[i].AccountNum);
                        //The account could be null if the AccountNum was never set correctly due to the automatic payment entry setup missing an income account from older versions.
                        if (account != null && account.Type == AccountType.Asset)
                        {
                            textDepositAccount.Text = jeL[i].DateDisplayed.ToShortDateString();
                            if (jeL[i].DebitAmt > 0)
                            {
                                textDepositAccount.Text += " " + jeL[i].DebitAmt.ToString("c");
                            }
                            else
                            {//negative
                                textDepositAccount.Text += " " + (-jeL[i].CreditAmt).ToString("c");
                            }
                            break;
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(_paymentCur.Receipt))
            {
                if (Preference.GetBool(PreferenceName.AllowEmailCCReceipt))
                {
                    butEmailReceipt.Visible = true;
                }
                butPrintReceipt.Visible = true;
            }
            _listValidPayPlans = _loadData.ListValidPayPlans;
            comboGroupBy.Items.Add("None");
            comboGroupBy.Items.Add("Provider");
            comboGroupBy.Items.Add("Clinic and Provider");
            comboGroupBy.SelectedIndex = 0;
            if (IsIncomeTransfer || _paymentCur.PayType == 0)
            {
                checkPayTypeNone.Checked = true;
            }
            if (_patCur.SuperFamily <= 0)
            {
                checkShowSuperfamily.Visible = false;
            }
            else
            {
                //Check the Super Family box if there are any splits from a member in the super family who is not in the immediate family.
                List<Patient> listSuperFamExclusive = _superFamCur.Members.Where(x => !_famCur.IsInFamily(x.PatNum)).ToList();
                if (!IsNew && (_listSplitsCur.Any(x => x.PatNum.In(listSuperFamExclusive.Select(y => y.PatNum)))))
                {
                    checkShowSuperfamily.Checked = true;
                }
            }
            Init(_loadData);
            if (InitialPaySplitNum != 0)
            {
                gridSplits.SetSelected(false);
                for (int i = 0; i < _listSplitsCur.Count; i++)
                {
                    if (InitialPaySplitNum == _listSplitsCur[i].SplitNum)
                    {
                        gridSplits.SetSelected(i, true);
                    }
                }
                HighlightChargesForSplits();
            }
            CheckUIState();
            _originalHeight = Height;
            if (Preference.GetBool(PreferenceName.PaymentWindowDefaultHideSplits))
            {
                ToggleShowHideSplits();//set hidden
            }
            textCheckNum.Select();
            Plugin.Trigger(this, "FormPayment_Loaded", _paymentCur, IsNew);
        }

        private void FillCreditCards()
        {
            AddCreditCardsToCombo(_listCreditCards);
            comboCreditCards.SelectedIndex = 0;
            bool isXWebCardPresent = _listCreditCards.Any(x => x.IsXWeb());
            _xWebResponse = _loadData.XWebResponse;
            groupXWeb.Visible = false;
            if (isXWebCardPresent || _xWebResponse != null)
            {
                groupXWeb.Visible = true;
            }
            if (_xWebResponse == null || _xWebResponse.XTransactionType == XWebTransactionType.CreditVoidTransaction)
            {
                //Can't run an XWeb void unless this payment is attached to a non-void XWeb transaction.
                butVoid.Visible = false;
                groupXWeb.Height = 55;
            }
            if (!isXWebCardPresent)
            {
                butReturn.Visible = false;
                butVoid.Location = butReturn.Location;
                groupXWeb.Height = 55;
            }
        }

        private void AddCreditCardsToCombo(List<CreditCard> listCreditCards, Func<CreditCard, bool> funcSelectCard = null)
        {
            comboCreditCards.Items.Clear();
            comboCreditCards.SelectedIndex = -1;
            for (int i = 0; i < listCreditCards.Count; i++)
            {
                string cardNum = listCreditCards[i].CCNumberMasked;
                if (Regex.IsMatch(cardNum, "^\\d{12}(\\d{0,7})"))
                { //Credit cards can have a minimum of 12 digits, maximum of 19
                    int idxLast4Digits = (cardNum.Length - 4);
                    cardNum = (new string('X', 12)) + cardNum.Substring(idxLast4Digits);//replace the first 12 with 12 X's
                }
                if (listCreditCards[i].IsXWeb())
                {
                    cardNum += " (XWeb)";
                }
                string tokensForCC = listCreditCards[i].GetTokenString();
                cardNum += (string.IsNullOrEmpty(tokensForCC) ? "" : " " + tokensForCC);
                comboCreditCards.Items.Add(new ODBoxItem<CreditCard>(cardNum, listCreditCards[i]));
                if (funcSelectCard != null && funcSelectCard(listCreditCards[i]))
                {
                    comboCreditCards.SelectedIndex = i;
                }
            }
            comboCreditCards.Items.Add(new ODBoxItem<CreditCard>(Lan.g(this, "New Card"), new CreditCard() { CreditCardNum = 0 }));
            if (comboCreditCards.SelectedIndex < 0)
            {
                comboCreditCards.SelectedIndex = comboCreditCards.Items.Count - 1;
            }
        }

        ///<summary>Populating _listPaySplitsAssociated with splits that are linked to each other.</summary>
        private void FillListPaySplitAssociated()
        {
            //All paysplits that have an FSplitNum of 0 are original paysplits.
            //Only loop through all allocated paysplits for this particular payment and find their corresponding original paysplit and make a PaySplitAssociated object.
            //We purposefully do not use _loadData.ListPaySplitAllocations because it contains paysplits that are not associated to this particular payment.
            List<PaySplit> listPaySplitAllocations = _listSplitsCur.FindAll(x => x.FSplitNum > 0);
            foreach (PaySplit paySplitAllocated in listPaySplitAllocations)
            {
                if (paySplitAllocated.FSplitNum == 0)
                {
                    continue;
                }
                //Find the corresponding original paysplit for the current allocated paysplit.
                //Look through all of the paysplits that are associated to this particular payment first (prefer _listSplitsCur due to the sync).
                PaySplit psOrig = _listSplitsCur.Find(x => x.SplitNum == paySplitAllocated.FSplitNum);
                if (psOrig == null)
                {
                    //The original is not associated to this particular payment, look through all of the associated prepayment paysplits next.
                    psOrig = _loadData.ListPrePaysForPayment.Find(x => x.SplitNum == paySplitAllocated.FSplitNum);
                }
                if (psOrig != null)
                {
                    _listPaySplitsAssociated.Add(new PaySplits.PaySplitAssociated(psOrig, paySplitAllocated));
                }
            }
        }

        ///<summary>Performs all of the Load functionality.</summary>
        private void Init(PaymentEdit.LoadData loadData = null, bool doAutoSplit = true)
        {
            _isInit = true;
            AmtTotal = (decimal)_paymentCur.PayAmt;
            if (ListEntriesPayFirst == null)
            {
                ListEntriesPayFirst = new List<AccountEntry>();//will be null when double clicking from account module. 
            }
            PaymentEdit.InitData initData = PaymentEdit.Init(_loadData.ListAssociatedPatients, _famCur, _superFamCur, _paymentCur, _listSplitsCur
                , ListEntriesPayFirst, _patCur.PatNum, _dictPatients, checkPayTypeNone.Checked, _preferCurrentPat, loadData, doAutoSplit);
            _paymentCur.PayAmt = (double)AmtTotal;//Reset it.	
            textSplitTotal.Text = initData.SplitTotal.ToString("f");
            _dictPatients = initData.DictPats;
            if (checkPayTypeNone.Checked)
            {

                comboGroupBy.SelectedIndex = 2;

            }
            //Get data from constructing charges list, linking credits, and auto splitting.
            _listSplitsCur = initData.AutoSplitData.ListSplitsCur;
            _listAccountCharges = initData.AutoSplitData.ListAccountCharges;
            _paymentCur = initData.AutoSplitData.Payment;
            FillFilters();
            FillGridSplits();
            //Select all charges on the right side that the paysplits are associated with.  Helps the user see what charges are attached.
            gridSplits.SetSelected(true);
            HighlightChargesForSplits();
            _isInit = false;
        }

        ///<summary>Mimics FormClaimPayEdit.CheckUIState().</summary>
        private void CheckUIState()
        {
            //_xProg = Programs.GetCur(ProgramName.Xcharge);
            //_xPath = Programs.GetProgramPath(_xProg);
            //Program progPayConnect = Programs.GetCur(ProgramName.PayConnect);
            //Program progPaySimple = Programs.GetCur(ProgramName.PaySimple);
            //if (_xProg == null || progPayConnect == null || progPaySimple == null)
            //{//Should not happen.
            //    panelXcharge.Visible = (_xProg != null);
            //    butPayConnect.Visible = (progPayConnect != null);
            //    butPaySimple.Visible = (progPaySimple != null);
            //    return;
            //}
            //panelXcharge.Visible = false;
            //butPayConnect.Visible = false;
            //butPaySimple.Visible = false;
            //if (!progPayConnect.Enabled && !_xProg.Enabled && !progPaySimple.Enabled)
            //{//if none enabled
            // //show all so user can pick
            //    panelXcharge.Visible = true;
            //    butPayConnect.Visible = true;
            //    butPaySimple.Visible = true;
            //    return;
            //}
            ////show if enabled.  User could have all enabled.
            //if (progPayConnect.Enabled)
            //{
            //    //if clinics are disabled, PayConnect is enabled if marked enabled
            //    string paymentType = ProgramProperties.GetPropVal(progPayConnect.ProgramNum, "PaymentType", _paymentCur.ClinicNum);
            //    if (!string.IsNullOrEmpty(ProgramProperties.GetPropVal(progPayConnect.ProgramNum, "Username", _paymentCur.ClinicNum))
            //        && !string.IsNullOrEmpty(ProgramProperties.GetPropVal(progPayConnect.ProgramNum, "Password", _paymentCur.ClinicNum))
            //        && _listPaymentTypeDefs.Any(x => x.Id.ToString() == paymentType))
            //    {
            //        butPayConnect.Visible = true;
            //    }
            //}

            ////show if enabled.  User could have both enabled.
            //if (_xProg.Enabled)
            //{
            //    //if clinics are disabled, X-Charge is enabled if marked enabled
            //    //if clinics are enabled, X-Charge is enabled if the PaymentType is valid and the Username and Password are not blank
            //    string paymentType = ProgramProperties.GetPropVal(_xProg.ProgramNum, "PaymentType", _paymentCur.ClinicNum);
            //    if (!string.IsNullOrEmpty(ProgramProperties.GetPropVal(_xProg.ProgramNum, "Username", _paymentCur.ClinicNum))
            //        && !string.IsNullOrEmpty(ProgramProperties.GetPropVal(_xProg.ProgramNum, "Password", _paymentCur.ClinicNum))
            //        && _listPaymentTypeDefs.Any(x => x.Id.ToString() == paymentType))
            //    {
            //        panelXcharge.Visible = true;
            //    }

            //}

            ////if(progPaySimple.Enabled) {
            ////	//if clinics are disabled, PaySimple is enabled if marked enabled
            ////	if(!Preferences.HasClinicsEnabled) {
            ////		butPaySimple.Visible=true;
            ////	}
            ////	else {//if clinics are enabled, PaySimple is enabled if the PaymentType is valid and the Username and Key are not blank
            ////		string paymentType=ProgramProperties.GetPropValForClinicOrDefault(progPaySimple.ProgramNum,PaySimple.PropertyDescs.PaySimplePayTypeCC,_paymentCur.ClinicNum);
            ////		if(!string.IsNullOrEmpty(ProgramProperties.GetPropValForClinicOrDefault(progPaySimple.ProgramNum,PaySimple.PropertyDescs.PaySimpleApiUserName,_paymentCur.ClinicNum))
            ////			&& !string.IsNullOrEmpty(ProgramProperties.GetPropValForClinicOrDefault(progPaySimple.ProgramNum,PaySimple.PropertyDescs.PaySimpleApiKey,_paymentCur.ClinicNum))
            ////			&& _listPaymentTypeDefs.Any(x => x.Id.ToString()==paymentType)) {
            ////			butPaySimple.Visible=true;
            ////		}
            ////	}
            ////}
            //if (panelXcharge.Visible == false && butPayConnect.Visible == false && butPaySimple.Visible == false)
            //{
            //    //This is an office with clinics and one of the payment processing bridges is enabled but this particular clinic doesn't have one set up.
            //    if (_xProg.Enabled)
            //    {
            //        panelXcharge.Visible = true;
            //    }
            //    if (progPayConnect.Enabled)
            //    {
            //        butPayConnect.Visible = true;
            //    }
            //    if (progPaySimple.Enabled)
            //    {
            //        butPaySimple.Visible = true;
            //    }
            //}
        }

        private void FillGridAllocated()
        {
            if (_listPaySplitAllocations.Count == 0)
            {
                return;
            }

            gridAllocated.BeginUpdate();
            gridAllocated.Columns.Clear();
            gridAllocated.Columns.Add(new ODGridColumn("Date", 80));
            gridAllocated.Columns.Add(new ODGridColumn("Clinic", 80));
            gridAllocated.Columns.Add(new ODGridColumn("Patient", 140));
            gridAllocated.Columns.Add(new ODGridColumn("Amount", 80, HorizontalAlignment.Right));
            gridAllocated.Columns.Add(new ODGridColumn("Unearned", 50));
            gridAllocated.Rows.Clear();

            for (int i = 0; i < _listPaySplitAllocations.Count; i++)
            {
                var row = new ODGridRow();

                row.Cells.Add(_listPaySplitAllocations[i].DatePay.ToShortDateString());
                row.Cells.Add(Clinic.GetById(_listPaySplitAllocations[i].ClinicNum).Abbr);
                row.Cells.Add(_curFamOrSuperFam.GetNameInFamFL(_listPaySplitAllocations[i].PatNum));
                row.Cells.Add(_listPaySplitAllocations[i].SplitAmt.ToString("F"));
                row.Cells.Add(Defs.GetName(DefinitionCategory.PaySplitUnearnedType, _listPaySplitAllocations[i].UnearnedType));//handles 0 just fine

                gridAllocated.Rows.Add(row);
            }
            gridAllocated.EndUpdate();
        }

        ///<summary>Fills the paysplit grid.</summary>
        private void FillGridSplits()
        {
            //Fill left grid with paysplits created
            List<long> listMissingProcsNums = _listSplitsCur.Where(x => x.ProcNum != 0 && !_loadData.ListProcsForSplits.Any(y => y.ProcNum == x.ProcNum))
                .Select(x => x.ProcNum).ToList();
            _loadData.ListProcsForSplits.AddRange(Procedures.GetManyProc(listMissingProcsNums, false));
            gridSplits.BeginUpdate();
            gridSplits.Columns.Clear();
            ODGridColumn col;
            col = new ODGridColumn(Lan.g(this, "Date"), 65, HorizontalAlignment.Center, ODGridSortingStrategy.DateParse);
            gridSplits.Columns.Add(col);
            col = new ODGridColumn(Lan.g(this, "Prov"), 40);
            gridSplits.Columns.Add(col);
            col = new ODGridColumn(Lan.g(this, "Clinic"), 40);
            gridSplits.Columns.Add(col);

            col = new ODGridColumn(Lan.g(this, "Patient"), 100);
            gridSplits.Columns.Add(col);
            col = new ODGridColumn(Lan.g(this, "Code"), 60);
            gridSplits.Columns.Add(col);
            col = new ODGridColumn(Lan.g(this, "Type"), 100);
            gridSplits.Columns.Add(col);
            col = new ODGridColumn(Lan.g(this, "Amount"), 55, HorizontalAlignment.Right, ODGridSortingStrategy.AmountParse);
            gridSplits.Columns.Add(col);
            gridSplits.Rows.Clear();
            ODGridRow row;
            decimal splitTotal = 0;
            for (int i = 0; i < _listSplitsCur.Count; i++)
            {
                splitTotal += (decimal)_listSplitsCur[i].SplitAmt;
                row = new ODGridRow();
                row.Tag = _listSplitsCur[i];
                row.Cells.Add(_listSplitsCur[i].DatePay.ToShortDateString());//Date
                row.Cells.Add(Providers.GetAbbr(_listSplitsCur[i].ProvNum));//Prov

                if (_listSplitsCur[i].ClinicNum != 0)
                {
                    row.Cells.Add(Clinic.GetById(_listSplitsCur[i].ClinicNum).Abbr);//Clinic
                }
                else
                {
                    row.Cells.Add("");//Clinic
                }

                Patient patCur;
                if (!_dictPatients.TryGetValue(_listSplitsCur[i].PatNum, out patCur))
                {
                    patCur = Patients.GetLim(_listSplitsCur[i].PatNum);
                    _dictPatients[patCur.PatNum] = patCur;
                }
                string patName = patCur.LName + ", " + patCur.FName;
                row.Cells.Add(patName);//Patient
                Procedure proc = new Procedure();
                if (_listSplitsCur[i].ProcNum != 0)
                {
                    proc = _loadData.ListProcsForSplits.FirstOrDefault(x => x.ProcNum == _listSplitsCur[i].ProcNum) ?? new Procedure();
                }
                row.Cells.Add(ProcedureCodes.GetStringProcCode(proc.CodeNum));//ProcCode
                string type = "";
                if (_listSplitsCur[i].PayPlanNum != 0)
                {
                    type += "PayPlanCharge";//Type
                    if (_listSplitsCur[i].IsInterestSplit && _listSplitsCur[i].ProcNum == 0 && _listSplitsCur[i].ProvNum != 0)
                    {
                        type += " (interest)";
                    }
                }
                if (_listSplitsCur[i].ProcNum != 0)
                {//Procedure
                    string procDesc = Procedures.GetDescription(proc);
                    if (type != "")
                    {
                        type += "\r\n";
                    }
                    type += "Proc: " + procDesc;//Type
                }
                if (_listSplitsCur[i].UnearnedType != 0 && _listSplitsCur[i].ProvNum == 0)
                {//Unattached split
                    if (type != "")
                    {
                        type += "\r\n";
                    }
                    type += "Unallocated";//Type
                }
                else if (_listSplitsCur[i].UnearnedType != 0 && _listSplitsCur[i].ProvNum != 0)
                {
                    if (type != "")
                    {
                        type += "\r\n";
                    }
                    type += Defs.GetName(DefinitionCategory.PaySplitUnearnedType, _listSplitsCur[i].UnearnedType);//Type
                }
                else if (_listSplitsCur[i].AdjNum != 0)
                {
                    type += "Adjustment";
                }
                row.Cells.Add(type);
                if (row.Cells[row.Cells.Count - 1].Text == "Unallocated")
                {
                    row.Cells[row.Cells.Count - 1].ColorText = System.Drawing.Color.Red;
                }
                row.Cells.Add(_listSplitsCur[i].SplitAmt.ToString("f"));//Amount
                gridSplits.Rows.Add(row);
            }
            textSplitTotal.Text = splitTotal.ToString("f");
            gridSplits.EndUpdate();
            FillGridCharges();
            FillGridAllocated();
        }

        ///<summary>Fills charge grid, and then split grid.</summary>
        private void FillGridCharges()
        {
            //Fill right-hand grid with all the charges, filtered based on checkbox and filters.
            gridCharges.BeginUpdate();
            gridCharges.Columns.Clear();
            ODGridColumn col;
            decimal chargeTotal = 0;
            if (comboGroupBy.SelectedIndex == 1)
            {//Group by 'Provider'
                col = new ODGridColumn(Lan.g(this, "Prov"), checkPayTypeNone.Checked ? 70 : 110);
                gridCharges.Columns.Add(col);
                if (checkPayTypeNone.Checked)
                {
                    col = new ODGridColumn(Lan.g(this, "Patient"), 119);
                    gridCharges.Columns.Add(col);
                }
                col = new ODGridColumn(Lan.g(this, "Codes"), checkPayTypeNone.Checked ? 170 : 249);
                gridCharges.Columns.Add(col);
                col = new ODGridColumn(Lan.g(this, "Amt Orig"), 70, HorizontalAlignment.Right, ODGridSortingStrategy.AmountParse);
                gridCharges.Columns.Add(col);
                col = new ODGridColumn(Lan.g(this, "Amt Start"), 70, HorizontalAlignment.Right, ODGridSortingStrategy.AmountParse);
                gridCharges.Columns.Add(col);
                col = new ODGridColumn(Lan.g(this, "Amt End"), 70, HorizontalAlignment.Right, ODGridSortingStrategy.AmountParse);
                gridCharges.Columns.Add(col);
                gridCharges.Rows.Clear();
                ODGridRow row;
                //Item1=ProvNum, Item2=PatNum
                List<Tuple<long, long>> listAddedProvNums = new List<Tuple<long, long>>();//this needs to be prov/patnum
                foreach (AccountEntry entryCharge in _listAccountCharges)
                {
                    if (Math.Round(entryCharge.AmountStart, 3) == 0)
                    {
                        continue;
                    }
                    if (!DoShowAccountEntry(entryCharge))
                    {
                        continue;
                    }
                    if (listAddedProvNums.Any(x => x.Item1 == entryCharge.ProvNum && x.Item2 == entryCharge.PatNum))
                    {
                        continue;
                    }
                    listAddedProvNums.Add(Tuple.Create(entryCharge.ProvNum, entryCharge.PatNum));
                    List<AccountEntry> listEntriesForProvAndPatient = _listAccountCharges.FindAll(x => x.ProvNum == entryCharge.ProvNum && x.PatNum == entryCharge.PatNum);
                    if (Math.Round(listEntriesForProvAndPatient.Sum(x => x.AmountStart), 3) == 0)
                    {
                        continue;
                    }
                    row = new ODGridRow();
                    row.Tag = listEntriesForProvAndPatient;
                    row.Cells.Add(Providers.GetAbbr(entryCharge.ProvNum));//Provider
                    if (checkPayTypeNone.Checked)
                    {
                        Patient pat;
                        if (!_dictPatients.TryGetValue(entryCharge.PatNum, out pat))
                        {
                            pat = Patients.GetLim(entryCharge.PatNum);
                            _dictPatients[pat.PatNum] = pat;
                        }
                        row.Cells.Add(pat.LName + ", " + pat.FName);//patient
                    }
                    string procCodes = "";
                    int addedCodes = 0;
                    for (int i = 0; i < listEntriesForProvAndPatient.Count; i++)
                    {
                        if (listEntriesForProvAndPatient[i].AmountStart == 0)
                        {
                            continue;
                        }
                        if (listEntriesForProvAndPatient[i].Tag.GetType() == typeof(Procedure))
                        {
                            if (procCodes != "")
                            {
                                procCodes += ", ";
                            }
                            procCodes += ProcedureCodes.GetStringProcCode(((Procedure)listEntriesForProvAndPatient[i].Tag).CodeNum);
                            addedCodes++;
                        }
                        if (addedCodes == 10)
                        {
                            procCodes += ", (...)";
                            break;
                        }
                    }
                    row.Cells.Add(procCodes);//ProcCodes
                    row.Cells.Add(listEntriesForProvAndPatient.Sum(x => x.AmountOriginal).ToString("f"));//Amount Original
                    row.Cells.Add(listEntriesForProvAndPatient.Sum(x => x.AmountStart).ToString("f"));//Amount Start
                    row.Cells.Add(listEntriesForProvAndPatient.Sum(x => x.AmountEnd).ToString("f"));//Amount End
                    chargeTotal += listEntriesForProvAndPatient.Sum(x => x.AmountEnd);
                    gridCharges.Rows.Add(row);
                }
            }
            else if (comboGroupBy.SelectedIndex == 2)
            {//Group by 'Clinic and Provider'
                col = new ODGridColumn(Lan.g(this, "Prov"), checkPayTypeNone.Checked ? 70 : 100);
                gridCharges.Columns.Add(col);
                if (checkPayTypeNone.Checked)
                {
                    col = new ODGridColumn(Lan.g(this, "Patient"), 100);
                    gridCharges.Columns.Add(col);
                }
                col = new ODGridColumn(Lan.g(this, "Clinic"), 60);
                gridCharges.Columns.Add(col);
                col = new ODGridColumn(Lan.g(this, "Codes"), checkPayTypeNone.Checked ? 130 : 200);
                gridCharges.Columns.Add(col);
                col = new ODGridColumn(Lan.g(this, "Amt Orig"), 70, HorizontalAlignment.Right, ODGridSortingStrategy.AmountParse);
                gridCharges.Columns.Add(col);
                col = new ODGridColumn(Lan.g(this, "Amt Start"), 70, HorizontalAlignment.Right, ODGridSortingStrategy.AmountParse);
                gridCharges.Columns.Add(col);
                col = new ODGridColumn(Lan.g(this, "Amt End"), 70, HorizontalAlignment.Right, ODGridSortingStrategy.AmountParse);
                gridCharges.Columns.Add(col);
                gridCharges.Rows.Clear();
                ODGridRow row;
                //Item1=ProvNum,Item2=ClinicNum,Item3=PatNum
                List<Tuple<long, long, long>> listAddedProvNums = new List<Tuple<long, long, long>>();//this needs to be clinic/prov/patnum
                foreach (AccountEntry entryCharge in _listAccountCharges)
                {
                    if (Math.Round(entryCharge.AmountStart, 3) == 0)
                    {
                        continue;
                    }
                    if (!DoShowAccountEntry(entryCharge))
                    {
                        continue;
                    }
                    if (listAddedProvNums.Any(x => x.Item1 == entryCharge.ProvNum && x.Item2 == entryCharge.ClinicNum && x.Item3 == entryCharge.PatNum))
                    {
                        continue;
                    }
                    listAddedProvNums.Add(Tuple.Create(entryCharge.ProvNum, entryCharge.ClinicNum, entryCharge.PatNum));
                    List<AccountEntry> listEntriesForProvAndClinicAndPatient = _listAccountCharges.FindAll(x => x.ProvNum == entryCharge.ProvNum && x.ClinicNum == entryCharge.ClinicNum && x.PatNum == entryCharge.PatNum);
                    if (Math.Round(listEntriesForProvAndClinicAndPatient.Sum(x => x.AmountStart), 3) == 0)
                    {
                        continue;
                    }
                    row = new ODGridRow();
                    row.Tag = listEntriesForProvAndClinicAndPatient;
                    row.Cells.Add(Providers.GetAbbr(entryCharge.ProvNum));//Provider
                    if (checkPayTypeNone.Checked)
                    {
                        Patient pat;
                        if (!_dictPatients.TryGetValue(entryCharge.PatNum, out pat))
                        {
                            pat = Patients.GetLim(entryCharge.PatNum);
                            _dictPatients[pat.PatNum] = pat;
                        }
                        row.Cells.Add(pat.LName + ", " + pat.FName);//Patient
                    }
                    row.Cells.Add(Clinic.GetById(entryCharge.ClinicNum).Abbr);
                    string procCodes = "";
                    int addedCodes = 0;
                    for (int i = 0; i < listEntriesForProvAndClinicAndPatient.Count; i++)
                    {
                        if (Math.Round(listEntriesForProvAndClinicAndPatient[i].AmountStart, 3) == 0)
                        {
                            continue;
                        }
                        if (listEntriesForProvAndClinicAndPatient[i].Tag.GetType() == typeof(Procedure))
                        {
                            if (procCodes != "")
                            {
                                procCodes += ", ";
                            }
                            procCodes += ProcedureCodes.GetStringProcCode(((Procedure)listEntriesForProvAndClinicAndPatient[i].Tag).CodeNum);
                            addedCodes++;
                        }
                        if (addedCodes == 9)
                        {//1 less than above, this column is shorter when filtering by prov + clinic
                            procCodes += ", (...)";
                            break;
                        }
                    }
                    row.Cells.Add(procCodes);//ProcCodes
                    row.Cells.Add(listEntriesForProvAndClinicAndPatient.Sum(x => x.AmountOriginal).ToString("f"));//Amount Original
                    row.Cells.Add(listEntriesForProvAndClinicAndPatient.Sum(x => x.AmountStart).ToString("f"));//Amount Start
                    row.Cells.Add(listEntriesForProvAndClinicAndPatient.Sum(x => x.AmountEnd).ToString("f"));//Amount End
                    chargeTotal += listEntriesForProvAndClinicAndPatient.Sum(x => x.AmountEnd);
                    gridCharges.Rows.Add(row);
                }
            }
            else
            { //Group by 'None'
                col = new ODGridColumn(Lan.g(this, "Date"), 65, sortingStrategy: ODGridSortingStrategy.DateParse);
                gridCharges.Columns.Add(col);
                col = new ODGridColumn(Lan.g(this, "Patient"), 92);
                gridCharges.Columns.Add(col);
                col = new ODGridColumn(Lan.g(this, "Prov"), 40);
                gridCharges.Columns.Add(col);
                col = new ODGridColumn(Lan.g(this, "Clinic"), 55);
                gridCharges.Columns.Add(col);

                col = new ODGridColumn(Lan.g(this, "Code"), 45);
                gridCharges.Columns.Add(col);
                col = new ODGridColumn(Lan.g(this, "Tth"), 25, sortingStrategy: ODGridSortingStrategy.ToothNumberParse);
                gridCharges.Columns.Add(col);
                col = new ODGridColumn(Lan.g(this, "Type"), 90);
                gridCharges.Columns.Add(col);
                col = new ODGridColumn(Lan.g(this, "AmtOrig"), 55, HorizontalAlignment.Right, ODGridSortingStrategy.AmountParse);
                gridCharges.Columns.Add(col);
                col = new ODGridColumn(Lan.g(this, "AmtStart"), 57, HorizontalAlignment.Right, ODGridSortingStrategy.AmountParse);
                gridCharges.Columns.Add(col);
                col = new ODGridColumn(Lan.g(this, "AmtEnd"), 55, HorizontalAlignment.Right, ODGridSortingStrategy.AmountParse);
                gridCharges.Columns.Add(col);
                gridCharges.Rows.Clear();
                ODGridRow row;
                for (int i = 0; i < _listAccountCharges.Count; i++)
                {
                    AccountEntry entryCharge = _listAccountCharges[i];
                    if (!checkShowAll.Checked && Math.Round(entryCharge.AmountStart, 3) == 0)
                    {
                        continue;
                    }
                    if (!DoShowAccountEntry(entryCharge))
                    {
                        continue;
                    }
                    if (!checkShowAll.Checked)
                    {//Filter out those that are paid in full and from other payments if checkbox unchecked.
                        bool isFound = false;
                        if (Math.Round(entryCharge.AmountEnd, 3) != 0)
                        {
                            isFound = true;
                        }
                        for (int j = 0; j < gridSplits.Rows.Count; j++)
                        {
                            PaySplit entryCredit = (PaySplit)gridSplits.Rows[j].Tag;
                            if (entryCharge.SplitCollection.Contains(entryCredit))
                            {
                                //Charge is paid for by a split in this payment, display it.
                                if (entryCharge.GetType() == typeof(Procedure) && entryCredit.PayPlanNum != 0)
                                {
                                    //Don't show the charge if it's a proc being paid by a payplan split.
                                    //From the user's perspective they're paying the "debits" not the procs.
                                }
                                else
                                {
                                    isFound = true;
                                    break;
                                }
                            }
                            else if (entryCharge.GetType() == typeof(PayPlanCharge) && entryCredit.PayPlanNum == ((PayPlanCharge)entryCharge.Tag).PayPlanNum && Math.Round(entryCharge.AmountStart, 3) != 0)
                            {
                                isFound = true;
                                break;
                            }
                        }
                        if (!isFound)
                        {//Hiding charges that aren't associated with the current payment or have been paid in full.
                            continue;
                        }
                    }
                    row = new ODGridRow();
                    row.Tag = _listAccountCharges[i];
                    row.Cells.Add(entryCharge.Date.ToShortDateString());//Date
                    Patient patCur;
                    if (!_dictPatients.TryGetValue(entryCharge.PatNum, out patCur))
                    {
                        patCur = Patients.GetLim(entryCharge.PatNum);
                        _dictPatients[patCur.PatNum] = patCur;
                    }
                    string patName = patCur.LName + ", " + patCur.FName;
                    if (entryCharge.Tag.GetType() == typeof(PayPlanChargeType))
                    {
                        patName += "\r\n" + Lan.g(this, "Guar") + ": " + _dictPatients[((PayPlanCharge)entryCharge.Tag).Guarantor].LName + ", "
                            + _dictPatients[((PayPlanCharge)entryCharge.Tag).Guarantor].FName;
                    }
                    row.Cells.Add(patName);//Patient
                    row.Cells.Add(Providers.GetAbbr(entryCharge.ProvNum));//Provider
                    row.Cells.Add(Clinic.GetById(entryCharge.ClinicNum).Abbr);

                    string procCode = "";
                    string tth = "";
                    Procedure proc = null;
                    if (entryCharge.Tag.GetType() == typeof(Procedure))
                    {
                        proc = (Procedure)entryCharge.Tag;
                        tth = proc.ToothNum == "" ? proc.Surf : Tooth.ToInternat(proc.ToothNum);
                        procCode = ProcedureCodes.GetStringProcCode(proc.CodeNum);
                    }
                    row.Cells.Add(procCode);//ProcCode
                    row.Cells.Add(tth);
                    if (entryCharge.GetType() == typeof(PaySplit))
                    {
                        row.Cells.Add("Unallocated");
                    }
                    else
                    {
                        row.Cells.Add(entryCharge.GetType().Name);//Type
                    }
                    if (entryCharge.GetType() == typeof(Procedure))
                    {
                        //Get the proc and add its description if the row is a proc.
                        row.Cells[row.Cells.Count - 1].Text = Lan.g(this, "Proc") + ": " + Procedures.GetDescription(proc);
                    }
                    row.Cells.Add(entryCharge.AmountOriginal.ToString("f"));//Amount Original
                    row.Cells.Add(entryCharge.AmountStart.ToString("f"));//Amount Start
                    row.Cells.Add(entryCharge.AmountEnd.ToString("f"));//Amount End
                    chargeTotal += entryCharge.AmountEnd;
                    gridCharges.Rows.Add(row);
                }
            }
            textChargeTotal.Text = chargeTotal.ToString("f");
            gridCharges.EndUpdate();
        }

        ///<summary>Returns true if the AccountEntry matches the currently selected filters.</summary>
        private bool DoShowAccountEntry(AccountEntry entryCharge)
        {
            if (entryCharge.GetType() == typeof(PayPlanCharge))
            {
                if (!_listFilteredPatNums.Contains(((PayPlanCharge)entryCharge.Tag).PatNum))
                {
                    return false;
                }
            }
            if (!_listFilteredPatNums.Contains(entryCharge.PatNum))
            {
                return false;
            }
            if (!_listFilteredProvNums.Contains(entryCharge.ProvNum))
            {
                return false;
            }
            if (comboGroupBy.SelectedIndex != 1 && !_listFilteredClinics.Contains(entryCharge.ClinicNum))
            {
                return false;
            }
            //proc code filter
            if (_listFilteredProcCodes.Count > 0
                && (entryCharge.Tag.GetType() != typeof(Procedure) || !_listFilteredProcCodes.Contains(((Procedure)entryCharge.Tag).CodeNum)))
            {
                return false;
            }
            //Charge Amount Filter
            if (amtMaxEnd.Value != 0 && entryCharge.AmountEnd > amtMaxEnd.Value)
            {
                return false;
            }
            //Charge Amount Filter
            if (amtMinEnd.Value != 0 && entryCharge.AmountEnd < amtMinEnd.Value)
            {
                return false;
            }
            //daterange filter
            if ((entryCharge.Date.Date < datePickFrom.Value.Date) || (entryCharge.Date.Date > datePickTo.Value.Date))
            {
                return false;
            }
            //Type Filter
            if (!_listFilteredType.Contains(entryCharge.GetType().Name))
            {
                return false;
            }
            return true;
        }

        ///<summary>Fills the combo boxes with correct values.</summary>
        private void FillFilters()
        {
            //Fill min/max date/amt
            //If there are no account charges, the date time will be DateTime.MinimumDate
            //Get latest proc, or tomorows date, whichever is later.
            if (_listAccountCharges.Count(x => x.Date >= datePickFrom.MinDate) == 0)
            {
                datePickFrom.Value = datePickFrom.Value;
                datePickTo.Value = DateTime.Today;
            }
            else
            {
                datePickFrom.Value = _listAccountCharges.Where(x => x.Date >= datePickFrom.MinDate).Min(x => x.Date);
                datePickTo.Value = ODMathLib.Max(_listAccountCharges.Max(x => x.Date), DateTime.Today);
            }
            amtMinEnd.Value = 0;
            amtMaxEnd.Value = 0;
            //Fill Patient Combo
            comboPatientFilter.Items.Clear();
            comboPatientFilter.Items.Add(new ODBoxItem<Patient>(Lan.g(this, "All")));
            foreach (Patient pat in _listAccountCharges.Where(x => x.PatNum.In(_listPatNums)
                     //Also include patients outside of the family if someone in the family is the guarantor of their payplan.
                     || (x.Tag.GetType() == typeof(PayPlanCharge) && ((PayPlanCharge)x.Tag).Guarantor.In(_listPatNums)))
                .Where(x => _dictPatients.ContainsKey(x.PatNum)).Select(x => _dictPatients[x.PatNum])
                .DistinctBy(x => x.PatNum))
            {
                comboPatientFilter.Items.Add(new ODBoxItem<Patient>(pat.GetNameFirstOrPreferred(), pat));
            }
            comboPatientFilter.SetSelected(0, true);
            //Fill Provider Combo
            comboProviderFilter.Items.Clear();
            comboProviderFilter.Items.Add(new ODBoxItem<Provider>(Lan.g(this, "All")));
            comboProviderFilter.Items.Add(new ODBoxItem<Provider>(Lan.g(this, "None"), new Provider()));
            foreach (Provider prov in _listAccountCharges.Select(x => Provider.GetById(x.ProvNum))
                .Where(x => x != null)
                .DistinctBy(x => x.Id))
            {
                comboProviderFilter.Items.Add(new ODBoxItem<Provider>(prov.Abbr, prov));
            }
            comboProviderFilter.SetSelected(0, true);
            //Fill Clinics Combo

            List<Clinic> listClinicsTemp = Clinic.All().ToList();
            listClinicsTemp.Add(new Clinic() { Abbr = Lan.g(this, "Unassigned") });
            comboClinicFilter.Items.Clear();
            comboClinicFilter.Items.Add(new ODBoxItem<Clinic>(Lan.g(this, "All")));
            foreach (Clinic clinic in _listAccountCharges.Select(x => listClinicsTemp.FirstOrDefault(y => y.Id == x.ClinicNum))
                .DistinctBy(x => x.Id))
            {
                comboClinicFilter.Items.Add(new ODBoxItem<Clinic>(clinic.Abbr, clinic));
            }
            comboClinicFilter.SetSelected(0, true);

            //Fill Type Combo
            comboTypeFilter.Items.Clear();
            comboTypeFilter.Items.Add(Lan.g(this, "All"));
            comboTypeFilter.Items.AddRange(_listAccountCharges.Select(x => x.GetType().Name).Distinct().ToList());
            comboTypeFilter.SetSelected(0, true);
        }

        ///<summary>Called whenever any of the filtering objects are changed.  Rechecks filtering and refreshes the grid.</summary>
        private void FilterChangeCommitted(object sender, EventArgs e)
        {
            if (_isInit)
            {
                return;
            }
            FillGridCharges();
        }

        ///<summary>Adds one split to _listPaySplits to work with.  Does not link the payment plan, that must be done outside this method.
        ///Called when checkPayPlan click, or upon load if auto attaching to payplan, or upon OK click if no splits were created.</summary>
        private bool AddOneSplit(bool promptForPayPlan = false)
        {
            PaySplit paySplitCur = new PaySplit();
            paySplitCur.PatNum = _patCur.PatNum;
            paySplitCur.PayNum = _paymentCur.PayNum;
            paySplitCur.DatePay = _paymentCur.PayDate;//this may be updated upon closing
            if (Preference.GetInt(PreferenceName.RigorousAccounting) < (int)RigorousAccounting.DontEnforce)
            {
                paySplitCur.ProvNum = 0;
                paySplitCur.UnearnedType = Preference.GetLong(PreferenceName.PrepaymentUnearnedType);//Use default unallocated type
            }
            else
            {
                paySplitCur.ProvNum = Patients.GetProvNum(_patCur);
            }
            paySplitCur.ClinicNum = _paymentCur.ClinicNum;
            paySplitCur.SplitAmt = PIn.Double(textAmount.Text);
            if (promptForPayPlan && _listValidPayPlans.Count > 0)
            {
                FormPayPlanSelect FormPPS = new FormPayPlanSelect(_listValidPayPlans, true);
                FormPPS.ShowDialog();
                if (FormPPS.DialogResult != DialogResult.OK)
                {
                    return false;
                }
                paySplitCur.PayPlanNum = FormPPS.SelectedPayPlanNum;
            }
            _listSplitsCur.Add(paySplitCur);
            _paymentCur.PayAmt = PIn.Double(textAmount.Text);
            return true;
        }

        private void listPayType_Click(object sender, EventArgs e)
        {
            textDepositAccount.Visible = false;
            SetComboDepositAccounts();
        }

        ///<summary>Called from all 3 places where listPayType gets changed.</summary>
        private void SetComboDepositAccounts()
        {
            if (listPayType.SelectedIndex == -1)
            {
                if (IsNew && (PayClinicSetting)Preference.GetInt(PreferenceName.PaymentClinicSetting) == PayClinicSetting.PatientDefaultClinic)
                {
                    labelDepositAccount.Visible = false;
                    comboDepositAccount.Visible = false;
                }
                return;
            }
            AccountAutoPay autoPay = AccountAutoPay.GetByPayType(
                _listPaymentTypeDefs[listPayType.SelectedIndex].Id);
            if (autoPay == null)
            {
                labelDepositAccount.Visible = false;
                comboDepositAccount.Visible = false;
            }
            else
            {
                labelDepositAccount.Visible = true;
                comboDepositAccount.Visible = true;
                _arrayDepositAcctNums = AccountAutoPay.GetPickListAccounts(autoPay).ToArray();
                comboDepositAccount.Items.Clear();
                for (int i = 0; i < _arrayDepositAcctNums.Length; i++)
                {
                    comboDepositAccount.Items.Add(Account.GetDescription(_arrayDepositAcctNums[i]));
                }
                if (comboDepositAccount.Items.Count > 0)
                {
                    comboDepositAccount.SelectedIndex = 0;
                }
            }
        }

        private void ToggleShowHideSplits()
        {
            splitContainerCharges.Panel2Collapsed = !splitContainerCharges.Panel2Collapsed;
            if (splitContainerCharges.Panel2Collapsed)
            {
                butShowHide.Text = Lan.g(this, "Show Splits");
                Height = splitContainerCharges.SplitterDistance + 100;//Plus 100 to give room for the buttons
                this.butShowHide.Image = global::OpenDental.Properties.Resources.arrowDownTriangle;
            }
            else
            {
                butShowHide.Text = Lan.g(this, "Hide Splits");
                Height = _originalHeight;
                this.butShowHide.Image = global::OpenDental.Properties.Resources.arrowUpTriangle;
            }
        }

        private void panelXcharge_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button != MouseButtons.Left)
            //{
            //    return;
            //}
            //if (!CanAddNewCreditCard(Programs.GetCur(ProgramName.Xcharge), XCharge.ProgramProperties.XChargePreventSavingNewCC))
            //{
            //    return;
            //}
            //_xChargeMilestone = "";
            //try
            //{
            //    MakeXChargeTransaction();
            //}
            //catch (Exception ex)
            //{
            //    FormFriendlyException.Show(Lan.g(this, "Error processing transaction.\r\n\r\nPlease contact support with the details of this error:")
            //        //The rest of the message is not translated on purpose because we here at HQ need to always be able to quickly read this part.
            //        + "\r\nLast valid milestone reached: " + _xChargeMilestone, ex);
            //}
        }

        ///<summary>Launches the XCharge transaction window and then executes whatever type of transaction was selected for the current payment.
        ///This is to help troubleshooting. Returns null upon failure, otherwise returns the transaction detail as a string.
        ///If prepaidAmt is not zero, then will show the xcharge window with the given prepaid amount and let the user enter card # and exp.
        ///A patient is not required for prepaid cards.</summary>
        public string MakeXChargeTransaction(double prepaidAmt = 0)
        {
            return ""; // TODO: Implement me

    //        //Need to refresh this list locally in case we are coming from another form
    //        _listPaymentTypeDefs = _listPaymentTypeDefs ?? Definition.GetByCategory(DefinitionCategory.PaymentTypes);
    //        _xChargeMilestone = "Validation";
    //        CreditCard cc = null;
    //        List<CreditCard> creditCards = null;
    //        if (prepaidAmt != 0)
    //        {
    //            CheckUIState();//To ensure that _xProg is set and _xPath is set.  Normally this would happen when loading.  Needed for HasXCharge().
    //        }
    //        if (!HasXCharge())
    //        {//Will show setup window if xcharge is not enabled or not completely setup yet.
    //            return null;
    //        }
    //        if (prepaidAmt == 0)
    //        {//Validation for regular credit cards (not prepaid cards).
    //            if (textAmount.Text == "" || PIn.Double(textAmount.Text) == 0)
    //            {
    //                MsgBox.Show(this, "Please enter an amount first.");
    //                textAmount.Focus();
    //                return null;
    //            }
    //            creditCards = CreditCards.Refresh(_patCur.PatNum);
    //            if (comboCreditCards.SelectedIndex < creditCards.Count && comboCreditCards.SelectedIndex > -1)
    //            {
    //                cc = creditCards[comboCreditCards.SelectedIndex];
    //            }
    //            if (cc != null && cc.IsXWeb())
    //            {
    //                MsgBox.Show(this, "Cards saved through XWeb cannot be used with the XCharge client program.");
    //                return null;
    //            }
    //            if (_listSplitsCur.Count > 0 && PIn.Double(textAmount.Text) != PIn.Double(textSplitTotal.Text)
    //                && (_listSplitsCur.Count != 1 || _listSplitsCur[0].PayPlanNum == 0)) //Not one paysplit attached to payplan
    //            {
    //                MsgBox.Show(this, "Split totals must equal payment amount before running a credit card transaction.");
    //                return null;
    //            }
    //        }
    //        if (PIn.Date(textDate.Text).Date > DateTime.Today.Date
    //                && !Preference.GetBool(PreferenceName.FutureTransDatesAllowed) && !Preference.GetBool(PreferenceName.AccountAllowFutureDebits))
    //        {
    //            MsgBox.Show(this, "Payment date cannot be in the future.");
    //            return null;
    //        }
    //        _xChargeMilestone = "XResult File";
    //        string resultfile = Preferences.GetRandomTempFile("txt");
    //        try
    //        {
    //            File.Delete(resultfile);//delete the old result file.
    //        }
    //        catch
    //        {
    //            MsgBox.Show(this, "Could not delete XResult.txt file.  It may be in use by another program, flagged as read-only, or you might not have "
    //                + "sufficient permissions.");
    //            return null;
    //        }
    //        _xChargeMilestone = "Properties";
    //        bool needToken = false;
    //        bool newCard = false;
    //        bool hasXToken = false;
    //        bool notRecurring = false;
    //        if (prepaidAmt == 0)
    //        {
    //            //These UI changes only need to happen for regular credit cards when the payment window is displayed.
    //            string xPayTypeNum = ProgramProperties.GetPropVal(_xProg.ProgramNum, "PaymentType", _paymentCur.ClinicNum);
    //            //still need to add functionality for accountingAutoPay
    //            listPayType.SelectedIndex = Defs.GetOrder(DefinitionCategory.PaymentTypes, PIn.Long(xPayTypeNum));
    //            SetComboDepositAccounts();
    //        }
    //        /*XCharge.exe [/TRANSACTIONTYPE:type] [/AMOUNT:amount] [/ACCOUNT:account] [/EXP:exp]
				//[�/TRACK:track�] [/ZIP:zip] [/ADDRESS:address] [/RECEIPT:receipt] [/CLERK:clerk]
				//[/APPROVALCODE:approval] [/AUTOPROCESS] [/AUTOCLOSE] [/STAYONTOP] [/MID]
				//[/RESULTFILE:�C:\Program Files\X-Charge\LocalTran\XCResult.txt�*/
    //        ProcessStartInfo info = new ProcessStartInfo(_xPath);
    //        Patient pat = null;
    //        if (prepaidAmt == 0)
    //        {
    //            pat = Patients.GetPat(_paymentCur.PatNum);
    //            if (pat == null)
    //            {
    //                MsgBox.Show(this, "Invalid patient associated to this payment.");
    //                return null;
    //            }
    //        }
    //        info.Arguments = "";
    //        double amt = PIn.Double(textAmount.Text);
    //        if (prepaidAmt != 0)
    //        {
    //            amt = prepaidAmt;
    //        }
    //        if (amt < 0)
    //        {//X-Charge always wants a positive number, even for returns.
    //            amt *= -1;
    //        }
    //        info.Arguments += "/AMOUNT:" + amt.ToString("F2") + " ";
    //        _xChargeMilestone = "Get Selected Credit Card";
    //        FormXchargeTrans FormXT = null;
    //        int tranType = 0;//Default to 0 "Purchase" for prepaid cards.
    //        string cashBack = null;
    //        if (prepaidAmt == 0)
    //        {//All regular cards (not prepaid)
    //            _xChargeMilestone = "Transaction Window Launch";
    //            //Show window to lock in the transaction type.
    //            FormXT = new FormXchargeTrans();
    //            FormXT.PrintReceipt = PIn.Bool(ProgramProperties.GetPropVal(_xProg.ProgramNum, "PrintReceipt", _paymentCur.ClinicNum));
    //            FormXT.PromptSignature = PIn.Bool(ProgramProperties.GetPropVal(_xProg.ProgramNum, "PromptSignature", _paymentCur.ClinicNum));
    //            FormXT.ClinicNum = _paymentCur.ClinicNum;
    //            FormXT.ShowDialog();
    //            if (FormXT.DialogResult != DialogResult.OK)
    //            {
    //                return null;
    //            }
    //            _xChargeMilestone = "Transaction Window Digest";
    //            _paymentCur.PaymentSource = CreditCardSource.XServer;
    //            _paymentCur.ProcessStatus = ProcessStat.OfficeProcessed;
    //            tranType = FormXT.TransactionType;
    //            decimal cashAmt = FormXT.CashBackAmount;
    //            cashBack = cashAmt.ToString("F2");
    //            _promptSignature = FormXT.PromptSignature;
    //            _printReceipt = FormXT.PrintReceipt;
    //        }
    //        _xChargeMilestone = "Check Duplicate Cards";
    //        if (cc != null && !string.IsNullOrEmpty(cc.XChargeToken))
    //        {//Have CC on file with an XChargeToken
    //            hasXToken = true;
    //            if (CreditCards.GetXChargeTokenCount(cc.XChargeToken, false) != 1)
    //            {
    //                MsgBox.Show(this, "This card shares a token with another card. Delete it from the Credit Card Manage window and re-add it.");
    //                return null;
    //            }
    //            /*       ***** An example of how recurring charges work***** 
				//C:\Program Files\X-Charge\XCharge.exe /TRANSACTIONTYPE:Purchase /LOCKTRANTYPE
				///AMOUNT:10.00 /LOCKAMOUNT /XCACCOUNTID:XAW0JWtx5kjG8 /RECEIPT:RC001
				///LOCKRECEIPT /CLERK:Clerk /LOCKCLERK /RESULTFILE:C:\ResultFile.txt /USERID:system
				///PASSWORD:system /STAYONTOP /AUTOPROCESS /AUTOCLOSE /HIDEMAINWINDOW
				///RECURRING /SMALLWINDOW /NORESULTDIALOG
				//*/
    //        }
    //        else if (cc != null)
    //        {//Have CC on file, no XChargeToken so not a recurring charge, and might need a token.
    //            notRecurring = true;
    //            if (!Preference.GetBool(PreferenceName.StoreCCnumbers))
    //            {//Use token only if user has has pref unchecked in module setup (allow store credit card nums).
    //                needToken = true;//Will create a token from result file so credit card info isn't saved in our db.
    //            }
    //        }
    //        else
    //        {//CC is null, add card option was selected in credit card drop down, no other possibility.
    //            newCard = true;
    //        }
    //        _xChargeMilestone = "Arguments Fill Card Info";
    //        info.Arguments += GetXChargeTransactionTypeCommands(tranType, hasXToken, notRecurring, cc, cashBack);
    //        if (prepaidAmt != 0)
    //        {
    //            //Zip and address are optional fields and for prepaid cards this information is probably not provided to the user.
    //        }
    //        else if (newCard)
    //        {
    //            info.Arguments += "\"/ZIP:" + pat.Zip + "\" ";
    //            info.Arguments += "\"/ADDRESS:" + pat.Address + "\" ";
    //        }
    //        else
    //        {
    //            if (cc.CCExpiration != null && cc.CCExpiration.Year > 2005)
    //            {
    //                info.Arguments += "/EXP:" + cc.CCExpiration.ToString("MMyy") + " ";
    //            }
    //            if (!string.IsNullOrEmpty(cc.Zip))
    //            {
    //                info.Arguments += "\"/ZIP:" + cc.Zip + "\" ";
    //            }
    //            else
    //            {
    //                info.Arguments += "\"/ZIP:" + pat.Zip + "\" ";
    //            }
    //            if (!string.IsNullOrEmpty(cc.Address))
    //            {
    //                info.Arguments += "\"/ADDRESS:" + cc.Address + "\" ";
    //            }
    //            else
    //            {
    //                info.Arguments += "\"/ADDRESS:" + pat.Address + "\" ";
    //            }
    //            if (hasXToken)
    //            {//Special parameter for tokens.
    //                info.Arguments += "/RECURRING ";
    //            }
    //        }
    //        _xChargeMilestone = "Arguments Fill X-Charge Settings";
    //        if (prepaidAmt == 0)
    //        {
    //            info.Arguments += "/RECEIPT:Pat" + _paymentCur.PatNum.ToString() + " ";//aka invoice#
    //        }
    //        else
    //        {
    //            info.Arguments += "/RECEIPT:PREPAID ";//aka invoice#
    //        }
    //        info.Arguments += "\"/CLERK:" + Security.CurrentUser.UserName + "\" /LOCKCLERK ";
    //        info.Arguments += "/RESULTFILE:\"" + resultfile + "\" ";
    //        info.Arguments += "/USERID:" + ProgramProperties.GetPropVal(_xProg.Id, "Username", _paymentCur.ClinicNum) + " ";
    //        info.Arguments += "/PASSWORD:" + CodeBase.MiscUtils.Decrypt(ProgramProperties.GetPropVal(_xProg.Id, "Password", _paymentCur.ClinicNum)) + " ";
    //        info.Arguments += "/PARTIALAPPROVALSUPPORT:T ";
    //        info.Arguments += "/AUTOCLOSE ";
    //        info.Arguments += "/HIDEMAINWINDOW ";
    //        info.Arguments += "/SMALLWINDOW ";
    //        info.Arguments += "/GETXCACCOUNTID ";
    //        info.Arguments += "/NORESULTDIALOG ";
    //        _xChargeMilestone = "X-Charge Launch";
    //        Cursor = Cursors.WaitCursor;
    //        Process process = new Process();
    //        process.StartInfo = info;
    //        process.EnableRaisingEvents = true;
    //        process.Start();
    //        process.WaitForExit();
    //        _xChargeMilestone = "X-Charge Complete";
    //        Thread.Sleep(200);//Wait 2/10 second to give time for file to be created.
    //        Cursor = Cursors.Default;
    //        string resulttext = "";
    //        string line = "";
    //        bool showApprovedAmtNotice = false;
    //        bool xAdjust = false;
    //        bool xReturn = false;
    //        bool xVoid = false;
    //        double approvedAmt = 0;
    //        double additionalFunds = 0;
    //        string xChargeToken = "";
    //        string accountMasked = "";
    //        string expiration = "";
    //        string signatureResult = "";
    //        string receipt = "";
    //        bool isDigitallySigned = false;
    //        bool updateCard = false;
    //        string newAccount = "";
    //        long creditCardNum;
    //        DateTime newExpiration = new DateTime();
    //        _xChargeMilestone = "Digest XResult";
    //        try
    //        {
    //            using (TextReader reader = new StreamReader(resultfile))
    //            {
    //                line = reader.ReadLine();
    //                /*Example of successful transaction:
				//		RESULT=SUCCESS
				//		TYPE=Purchase
				//		APPROVALCODE=000064
				//		ACCOUNT=XXXXXXXXXXXX6781
				//		ACCOUNTTYPE=VISA*
				//		AMOUNT=1.77
				//		AVSRESULT=Y
				//		CVRESULT=M
				//	*/
    //                while (line != null)
    //                {
    //                    if (!line.StartsWith("RECEIPT="))
    //                    {//Don't include the receipt string in the PayNote
    //                        if (resulttext != "")
    //                        {
    //                            resulttext += "\r\n";
    //                        }
    //                        resulttext += line;
    //                    }
    //                    if (line.StartsWith("RESULT="))
    //                    {
    //                        if (line != "RESULT=SUCCESS")
    //                        {
    //                            //Charge was a failure and there might be a description as to why it failed. Continue to loop through line.
    //                            while (line != null)
    //                            {
    //                                line = reader.ReadLine();
    //                                if (line != null && !line.StartsWith("RECEIPT="))
    //                                {//Don't include the receipt string in the PayNote
    //                                    resulttext += "\r\n" + line;
    //                                }
    //                            }
    //                            needToken = false;//Don't update CCard due to failure
    //                            newCard = false;//Don't insert CCard due to failure
    //                            _isCCDeclined = true;
    //                            break;
    //                        }
    //                        if (tranType == 1)
    //                        {
    //                            xReturn = true;
    //                        }
    //                        if (tranType == 6)
    //                        {
    //                            xAdjust = true;
    //                        }
    //                        if (tranType == 7)
    //                        {
    //                            xVoid = true;
    //                        }
    //                        _isCCDeclined = false;
    //                    }
    //                    if (line.StartsWith("APPROVEDAMOUNT="))
    //                    {
    //                        approvedAmt = PIn.Double(line.Substring(15));
    //                        if (approvedAmt != amt)
    //                        {
    //                            showApprovedAmtNotice = true;
    //                        }
    //                    }
    //                    if (line.StartsWith("XCACCOUNTID="))
    //                    {
    //                        xChargeToken = PIn.String(line.Substring(12));
    //                    }
    //                    if (line.StartsWith("ACCOUNT="))
    //                    {
    //                        accountMasked = PIn.String(line.Substring(8));
    //                    }
    //                    if (line.StartsWith("EXPIRATION="))
    //                    {
    //                        expiration = PIn.String(line.Substring(11));
    //                    }
    //                    if (line.StartsWith("ADDITIONALFUNDSREQUIRED="))
    //                    {
    //                        additionalFunds = PIn.Double(line.Substring(24));
    //                    }
    //                    if (line.StartsWith("SIGNATURE=") && line.Length > 10)
    //                    {
    //                        signatureResult = PIn.String(line.Substring(10));
    //                        //A successful digitally signed signature will say SIGNATURE=C:\Users\Folder\Where\The\Signature\Is\Stored.bmp
    //                        if (signatureResult != "NOT SUPPORTED" && signatureResult != "FAILED")
    //                        {
    //                            isDigitallySigned = true;
    //                        }
    //                    }
    //                    if (line.StartsWith("RECEIPT="))
    //                    {
    //                        receipt = PIn.String(line.Replace("RECEIPT=", "").Replace("\\n", "\n"));//The receipt from X-Charge escapes the newline characters
    //                        if (isDigitallySigned)
    //                        {
    //                            //Replace X____________________________ with 'Electronically signed'
    //                            receipt.Split('\n').ToList().FindAll(x => x.StartsWith("X___")).ForEach(x => x = "Electronically signed");
    //                        }
    //                        receipt = receipt.Replace("\r", "").Replace("\n", "\r\n");//remove any existing \r's before replacing \n's with \r\n's
    //                    }
    //                    if (line == "XCACCOUNTIDUPDATED=T")
    //                    {//Decline minimizer updated the account information since the last time this card was charged
    //                        updateCard = true;
    //                    }
    //                    if (line.StartsWith("ACCOUNT="))
    //                    {
    //                        newAccount = line.Substring("ACCOUNT=".Length);
    //                    }
    //                    if (line.StartsWith("EXPIRATION="))
    //                    {
    //                        string expStr = line.Substring("EXPIRATION=".Length);//Expiration should be MMYY
    //                        newExpiration = new DateTime(PIn.Int("20" + expStr.Substring(2)), PIn.Int(expStr.Substring(0, 2)), 1);//First day of the month
    //                    }
    //                    line = reader.ReadLine();
    //                }
    //                if (needToken && !string.IsNullOrEmpty(xChargeToken) && prepaidAmt == 0)
    //                {//never save token for prepaid cards
    //                    _xChargeMilestone = "Update Token";
    //                    DateTime expDate = new DateTime(PIn.Int("20" + expiration.Right(2)), PIn.Int(expiration.Left(2)), 1);
    //                    //If the stored CC used for this X-Charge payment has a PayConnect token, and X-Charge returns a different masked number or exp date, we
    //                    //will clear out the PayConnect token since this CC no longer refers to the same card that was used to generate the PayConnect token.
    //                    if (!string.IsNullOrEmpty(cc.PayConnectToken) //there is a PayConnect token for this saved CC
    //                        && Regex.IsMatch(cc.CCNumberMasked, @"X+[0-9]{4}") //the saved CC has a masked number with the pattern XXXXXXXXXXXX1234
    //                        && (cc.CCNumberMasked.Right(4) != accountMasked.Right(4) //and either the last four digits don't match what X-Charge returned
    //                                || cc.CCExpiration.Year != expDate.Year //or the exp year doesn't match that returned by X-Charge
    //                                || cc.CCExpiration.Month != expDate.Month)) //or the exp month doesn't match that returned by X-Charge
    //                    {
    //                        cc.PayConnectToken = "";
    //                        cc.PayConnectTokenExp = DateTime.MinValue;
    //                    }
    //                    //Only way this code can be hit is if they have set up a credit card and it does not have a token.
    //                    //So we'll use the created token from result file and assign it to the coresponding account.
    //                    //Also will delete the credit card number and replace it with secure masked number.
    //                    cc.XChargeToken = xChargeToken;
    //                    cc.CCNumberMasked = accountMasked;
    //                    cc.CCExpiration = expDate;
    //                    cc.Procedures = Preference.GetString(PreferenceName.DefaultCCProcs);
    //                    cc.CCSource = CreditCardSource.XServer;
    //                    CreditCards.Update(cc);
    //                }
    //                if (newCard && prepaidAmt == 0)
    //                {//never save card information to the patient account for prepaid cards
    //                    if (!string.IsNullOrEmpty(xChargeToken) && FormXT.SaveToken)
    //                    {
    //                        _xChargeMilestone = "Create New Credit Card Entry";
    //                        cc = new CreditCard();
    //                        List<CreditCard> itemOrderCount = CreditCards.Refresh(_patCur.PatNum);
    //                        cc.ItemOrder = itemOrderCount.Count;
    //                        cc.PatNum = _patCur.PatNum;
    //                        cc.CCExpiration = new DateTime(Convert.ToInt32("20" + expiration.Substring(2, 2)), Convert.ToInt32(expiration.Substring(0, 2)), 1);
    //                        cc.XChargeToken = xChargeToken;
    //                        cc.CCNumberMasked = accountMasked;
    //                        cc.Procedures = Preference.GetString(PreferenceName.DefaultCCProcs);
    //                        cc.CCSource = CreditCardSource.XServer;
    //                        cc.ClinicNum = _paymentCur.ClinicNum;
    //                        creditCardNum = CreditCards.Insert(cc);
    //                    }
    //                    else if (string.IsNullOrEmpty(xChargeToken))
    //                    {//Shouldn't happen again but leaving just in case.
    //                        MsgBox.Show(this, "X-Charge didn't return a token so credit card information couldn't be saved.");
    //                    }
    //                }
    //                if (updateCard && newAccount != "" && newExpiration.Year > 1880 && prepaidAmt == 0)
    //                {//Never save credit card info to patient for prepaid cards.
    //                    if (textNote.Text != "")
    //                    {
    //                        textNote.Text += "\r\n";
    //                    }
    //                    if (cc.CCNumberMasked != newAccount)
    //                    {
    //                        textNote.Text += Lan.g(this, "Account number changed from") + " " + cc.CCNumberMasked + " "
    //                            + Lan.g(this, "to") + " " + newAccount;
    //                    }
    //                    if (cc.CCExpiration != newExpiration)
    //                    {
    //                        textNote.Text += Lan.g(this, "Expiration changed from") + " " + cc.CCExpiration.ToString("MMyy") + " "
    //                            + Lan.g(this, "to") + " " + newExpiration.ToString("MMyy");
    //                    }
    //                    cc.CCNumberMasked = newAccount;
    //                    cc.CCExpiration = newExpiration;
    //                    CreditCards.Update(cc);
    //                }
    //            }
    //        }
    //        catch
    //        {
    //            MessageBox.Show(Lan.g(this, "There was a problem charging the card.  Please run the credit card report from inside X-Charge to verify that "
    //                + "the card was not actually charged.") + "\r\n" + Lan.g(this, "If the card was charged, you need to make sure that the payment amount matches.")
    //                + "\r\n" + Lan.g(this, "If the card was not charged, please try again."));
    //            return null;
    //        }
    //        _xChargeMilestone = "Check Approved Amount";
    //        if (showApprovedAmtNotice && !xVoid && !xAdjust && !xReturn)
    //        {
    //            MessageBox.Show(Lan.g(this, "The amount you typed in") + ": " + amt.ToString("C") + "\r\n" + Lan.g(this, "does not match the approved amount returned")
    //                + ": " + approvedAmt.ToString("C") + ".\r\n" + Lan.g(this, "The amount will be changed to reflect the approved amount charged."), "Alert",
    //                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    //            textAmount.Text = approvedAmt.ToString("F");
    //        }
    //        if (xAdjust)
    //        {
    //            _xChargeMilestone = "Check Adjust";
    //            MessageBox.Show(Lan.g(this, "The amount will be changed to the X-Charge approved amount") + ": " + approvedAmt.ToString("C"));
    //            textNote.Text = "";
    //            textAmount.Text = approvedAmt.ToString("F");
    //        }
    //        else if (xReturn)
    //        {
    //            _xChargeMilestone = "Check Return";
    //            textAmount.Text = "-" + approvedAmt.ToString("F");
    //        }
    //        else if (xVoid)
    //        {//For prepaid cards, tranType is set to 0 "Purchase", therefore xVoid will be false.
    //            _xChargeMilestone = "Check Void";
    //            if (IsNew)
    //            {
    //                if (!_wasCreditCardSuccessful)
    //                {
    //                    textAmount.Text = "-" + approvedAmt.ToString("F");
    //                    textNote.Text += resulttext;
    //                }
    //                _paymentCur.Receipt = receipt;
    //                if (_printReceipt && receipt != "")
    //                {
    //                    PrintReceipt(receipt, Lan.g(this, "X-Charge receipt printed"));
    //                    _printReceipt = false;
    //                }
    //                if (SavePaymentToDb())
    //                {
    //                    DialogResult = DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
    //                }
    //                return resulttext;
    //            }
    //            _xChargeMilestone = "Create Negative Payment";
    //            if (!IsNew || _wasCreditCardSuccessful)
    //            {//Create a new negative payment if the void is being run from an existing payment
    //                if (_listSplitsCur.Count == 0)
    //                {
    //                    AddOneSplit();
    //                    FillGridSplits();
    //                }
    //                else if (_listSplitsCur.Count == 1//if one split
    //                    && _listSplitsCur[0].PayPlanNum != 0//and split is on a payment plan
    //                    && _listSplitsCur[0].SplitAmt != _paymentCur.PayAmt)//and amount doesn't match payment
    //                {
    //                    _listSplitsCur[0].SplitAmt = _paymentCur.PayAmt;//make amounts match automatically
    //                    textSplitTotal.Text = textAmount.Text;
    //                }
    //                _paymentCur.IsSplit = _listSplitsCur.Count > 1;
    //                Payment voidPayment = _paymentCur.Clone();
    //                voidPayment.PayAmt *= -1;//the negation of the original amount
    //                voidPayment.PayNote = resulttext;
    //                voidPayment.Receipt = receipt;
    //                if (_printReceipt && receipt != "")
    //                {
    //                    PrintReceipt(receipt, Lan.g(this, "X-Charge receipt printed"));
    //                }
    //                voidPayment.PaymentSource = CreditCardSource.XServer;
    //                voidPayment.ProcessStatus = ProcessStat.OfficeProcessed;
    //                voidPayment.PayNum = Payments.Insert(voidPayment);
    //                foreach (PaySplit splitCur in _listSplitsCur)
    //                {//Modify the paysplits for the original transaction to work for the void transaction
    //                    PaySplit split = splitCur.Copy();
    //                    split.SplitAmt *= -1;
    //                    split.PayNum = voidPayment.PayNum;
    //                    PaySplits.Insert(split);
    //                }
    //                string strErrorMsg = Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(_patCur.PatNum, _listSplitsCur);
    //                if (!string.IsNullOrEmpty(strErrorMsg))
    //                {
    //                    MessageBox.Show(strErrorMsg);
    //                }
    //            }
    //            DialogResult = DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
    //            return resulttext;
    //        }
    //        _xChargeMilestone = "Check Additional Funds";
    //        _wasCreditCardSuccessful = !_isCCDeclined;//If the transaction is not a void transaction, we will void this transaction if the user hits Cancel
    //        if (additionalFunds > 0)
    //        {
    //            MessageBox.Show(Lan.g(this, "Additional funds required") + ": " + additionalFunds.ToString("C"));
    //        }
    //        if (textNote.Text != "")
    //        {
    //            textNote.Text += "\r\n";
    //        }
    //        textNote.Text += resulttext;
    //        _xChargeMilestone = "Receipt";
    //        _paymentCur.Receipt = receipt;
    //        if (!string.IsNullOrEmpty(receipt))
    //        {
    //            butPrintReceipt.Visible = true;
    //            if (Preference.GetBool(PreferenceName.AllowEmailCCReceipt))
    //            {
    //                butEmailReceipt.Visible = true;
    //            }
    //            if (_printReceipt && prepaidAmt == 0)
    //            {
    //                PrintReceipt(receipt, Lan.g(this, "X-Charge receipt printed"));
    //            }
    //        }
    //        _xChargeMilestone = "Reselect Credit Card in Combo";
    //        if (cc != null && !string.IsNullOrEmpty(cc.XChargeToken) && cc.CCExpiration != null)
    //        {
    //            //Refresh comboCreditCards and select the index of the card used for this payment if the token was saved
    //            creditCards = CreditCards.Refresh(_patCur.PatNum);
    //            AddCreditCardsToCombo(creditCards, x => x.XChargeToken == cc.XChargeToken
    //                 && x.CCExpiration.Year == cc.CCExpiration.Year
    //                 && x.CCExpiration.Month == cc.CCExpiration.Month);
    //        }
    //        if (_isCCDeclined)
    //        {
    //            return null;
    //        }
    //        return resulttext;
        }

        ///<summary>Only used to void a transaction that has just been completed when the user hits Cancel. Uses the same Print Receipt settings as the 
        ///original transaction.</summary>
        private void VoidXChargeTransaction(string transID, string amount, bool isDebit)
        {
            // TODO: Implement me

     //       ProcessStartInfo info = new ProcessStartInfo(_xProg.Path);
     //       string resultfile = Preferences.GetRandomTempFile("txt");
     //       File.Delete(resultfile);//delete the old result file.
     //       info.Arguments = "";
     //       if (isDebit)
     //       {
     //           info.Arguments += "/TRANSACTIONTYPE:DEBITRETURN /LOCKTRANTYPE ";
     //       }
     //       else
     //       {
     //           info.Arguments += "/TRANSACTIONTYPE:VOID /LOCKTRANTYPE ";
     //       }
     //       info.Arguments += "/XCTRANSACTIONID:" + transID + " /LOCKXCTRANSACTIONID ";
     //       info.Arguments += "/AMOUNT:" + amount + " /LOCKAMOUNT ";
     //       info.Arguments += "/RECEIPT:Pat" + _paymentCur.PatNum.ToString() + " ";//aka invoice#
     //       info.Arguments += "\"/CLERK:" + Security.CurrentUser.UserName + "\" /LOCKCLERK ";
     //       info.Arguments += "/RESULTFILE:\"" + resultfile + "\" ";
     //       info.Arguments += "/USERID:" + ProgramProperties.GetPropVal(_xProg.Id, "Username", _paymentCur.ClinicNum) + " ";
     //       info.Arguments += "/PASSWORD:" + CodeBase.MiscUtils.Decrypt(ProgramProperties.GetPropVal(_xProg.Id, "Password", _paymentCur.ClinicNum)) + " ";
     //       info.Arguments += "/AUTOCLOSE ";
     //       info.Arguments += "/HIDEMAINWINDOW /SMALLWINDOW ";
     //       if (!isDebit)
     //       {
     //           info.Arguments += "/AUTOPROCESS ";
     //       }
     //       info.Arguments += "/PROMPTSIGNATURE:F ";
     //       info.Arguments += "/RECEIPTINRESULT ";
     //       Cursor = Cursors.WaitCursor;
     //       Process process = new Process();
     //       process.StartInfo = info;
     //       process.EnableRaisingEvents = true;
     //       process.Start();
     //       process.WaitForExit();
     //       Thread.Sleep(200);//Wait 2/10 second to give time for file to be created.
     //       Cursor = Cursors.Default;
     //       //Next, record the voided payment within Open Dental.  We use to delete the payment but Nathan wants us to negate voids with another payment.
     //       string resulttext = "";
     //       string line = "";
     //       bool showApprovedAmtNotice = false;
     //       double approvedAmt = 0;
     //       string receipt = "";
     //       Payment voidPayment = _paymentCur.Clone();
     //       voidPayment.PayAmt *= -1;//the negation of the original amount
     //       try
     //       {
     //           using (TextReader reader = new StreamReader(resultfile))
     //           {
     //               line = reader.ReadLine();
     //               /*Example of successful void transaction:
					//	RESULT=SUCCESS
					//	TYPE=Void
					//	APPROVALCODE=000000
					//	SWIPED=F
					//	CLERK=Admin
					//	XCACCOUNTID=XAWpQPwLm7MXZ
					//	XCTRANSACTIONID=15042616
					//	ACCOUNT=XXXXXXXXXXXX6781
					//	EXPIRATION=1215
					//	ACCOUNTTYPE=VISA
					//	APPROVEDAMOUNT=11.00
					//*/
     //               while (line != null)
     //               {
     //                   if (!line.StartsWith("RECEIPT="))
     //                   {//Don't include the receipt string in the PayNote
     //                       if (resulttext != "")
     //                       {
     //                           resulttext += "\r\n";
     //                       }
     //                       resulttext += line;
     //                   }
     //                   if (line.StartsWith("RESULT="))
     //                   {
     //                       if (line != "RESULT=SUCCESS")
     //                       {
     //                           //Void was a failure and there might be a description as to why it failed. Continue to loop through line.
     //                           while (line != null)
     //                           {
     //                               line = reader.ReadLine();
     //                               resulttext += "\r\n" + line;
     //                           }
     //                           break;
     //                       }
     //                   }
     //                   if (line.StartsWith("APPROVEDAMOUNT="))
     //                   {
     //                       approvedAmt = PIn.Double(line.Substring(15));
     //                       if (approvedAmt != _paymentCur.PayAmt)
     //                       {
     //                           showApprovedAmtNotice = true;
     //                       }
     //                   }
     //                   if (line.StartsWith("RECEIPT=") && line.Length > 8)
     //                   {
     //                       receipt = PIn.String(line.Substring(8));
     //                       receipt = receipt.Replace("\\n", "\r\n");//The receipt from X-Charge escapes the newline characters
     //                   }
     //                   line = reader.ReadLine();
     //               }
     //           }
     //       }
     //       catch
     //       {
     //           MessageBox.Show(Lan.g(this, "There was a problem voiding this transaction.") + "\r\n" + Lan.g(this, "Please run the credit card report from inside "
     //               + "X-Charge to verify that the transaction was voided.") + "\r\n" + Lan.g(this, "If the transaction was not voided, please create a new payment "
     //               + "to void the transaction."));
     //           return;
     //       }
     //       if (showApprovedAmtNotice)
     //       {
     //           MessageBox.Show(Lan.g(this, "The amount of the original transaction") + ": " + _paymentCur.PayAmt.ToString("C") + "\r\n" + Lan.g(this, "does not match "
     //               + "the approved amount returned") + ": " + approvedAmt.ToString("C") + ".\r\n" + Lan.g(this, "The amount will be changed to reflect the approved "
     //               + "amount charged."), "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
     //           voidPayment.PayAmt = approvedAmt;
     //       }
     //       if (textNote.Text != "")
     //       {
     //           textNote.Text += "\r\n";
     //       }
     //       voidPayment.PayNote = resulttext;
     //       voidPayment.Receipt = receipt;
     //       if (_printReceipt && receipt != "")
     //       {
     //           PrintReceipt(receipt, Lan.g(this, "X-Charge receipt printed"));
     //       }
     //       voidPayment.PaymentSource = CreditCardSource.XServer;
     //       voidPayment.ProcessStatus = ProcessStat.OfficeProcessed;
     //       voidPayment.PayNum = Payments.Insert(voidPayment);
     //       for (int i = 0; i < _listSplitsCur.Count; i++)
     //       {//Modify the paysplits for the original transaction to work for the void transaction
     //           PaySplit split = _listSplitsCur[i].Copy();
     //           split.SplitAmt *= -1;
     //           split.PayNum = voidPayment.PayNum;
     //           PaySplits.Insert(split);
     //       }
     //       SecurityLog.Write(Permissions.PaymentCreate, voidPayment.PatNum, Patients.GetLim(voidPayment.PatNum).GetNameLF() + ", "
     //           + voidPayment.PayAmt.ToString("c"));
        }

        private bool HasXCharge()
        {
            return false; // TODO: Implement me

            //_listPaymentTypeDefs = _listPaymentTypeDefs ?? Definition.GetByCategory(DefinitionCategory.PaymentTypes);
            //if (_xProg == null)
            //{
            //    MsgBox.Show(this, "X-Charge entry is missing from the database.");//should never happen
            //    return false;
            //}
            //bool isSetupRequired = false;
            ////if X-Charge is enabled, but the Username or Password are blank or the PaymentType is not a valid DefNum, setup is required
            //if (_xProg.Enabled)
            //{
            //    //X-Charge is enabled if the username and password are set and the PaymentType is a valid DefNum
            //    //If clinics are disabled, _paymentCur.ClinicNum will be 0 and the Username and Password will be the 'Headquarters' or practice credentials
            //    string paymentType = ProgramProperties.GetPropVal(_xProg.Id, "PaymentType", _paymentCur.ClinicNum);
            //    if (string.IsNullOrEmpty(ProgramProperties.GetPropVal(_xProg.Id, "Username", _paymentCur.ClinicNum))
            //        || string.IsNullOrEmpty(ProgramProperties.GetPropVal(_xProg.Id, "Password", _paymentCur.ClinicNum))
            //        || !_listPaymentTypeDefs.Any(x => x.Id.ToString() == paymentType))
            //    {
            //        isSetupRequired = true;
            //    }
            //}
            //else
            //{//Program link not enabled.  Launch a promo site.
            //    ODException.SwallowAnyException(() =>
            //        Process.Start("http://www.opendental.com/resources/redirects/redirectopenedge.html")
            //    );
            //    return false;
            //}
            ////if X-Charge is enabled and the Username and Password is set and the PaymentType is a valid DefNum,
            ////make sure the path (either local override or program path) is valid
            //if (!isSetupRequired && !File.Exists(_xPath))
            //{
            //    MsgBox.Show(this, "Path is not valid.");
            //    isSetupRequired = true;
            //}
            ////if setup is required and the user is authorized for setup, load the X-Charge setup form, but return false so the validation can happen again
            //if (isSetupRequired && Security.IsAuthorized(Permissions.Setup))
            //{
            //    FormXchargeSetup FormX = new FormXchargeSetup();
            //    FormX.ShowDialog();
            //    CheckUIState();//user may have made a change in setup that affects the state of the UI, e.g. X-Charge is no longer enabled for this clinic
            //    return false;
            //}
            //return true;
        }

        private string GetXChargeTransactionTypeCommands(int tranType, bool hasXToken, bool notRecurring, CreditCard CCard, string cashBack)
        {
            string tranText = "";
            switch (tranType)
            {
                case 0:
                    tranText += "/TRANSACTIONTYPE:PURCHASE /LOCKTRANTYPE /LOCKAMOUNT ";
                    if (hasXToken && CCard != null)
                    {
                        tranText += "/XCACCOUNTID:" + CCard.XChargeToken + " ";
                        tranText += "/AUTOPROCESS ";
                        tranText += "/GETXCACCOUNTIDSTATUS ";
                    }
                    if (notRecurring && CCard != null)
                    {
                        tranText += "/ACCOUNT:" + CCard.CCNumberMasked + " ";
                        tranText += "/AUTOPROCESS ";
                    }
                    break;
                case 1:
                    tranText += "/TRANSACTIONTYPE:RETURN /LOCKTRANTYPE /LOCKAMOUNT ";
                    if (hasXToken)
                    {
                        tranText += "/XCACCOUNTID:" + CCard.XChargeToken + " ";
                        tranText += "/AUTOPROCESS ";
                        tranText += "/GETXCACCOUNTIDSTATUS ";
                    }
                    if (notRecurring)
                    {
                        tranText += "/ACCOUNT:" + CCard.CCNumberMasked + " ";
                        tranText += "/AUTOPROCESS ";
                    }
                    break;
                case 2:
                    tranText += "/TRANSACTIONTYPE:DEBITPURCHASE /LOCKTRANTYPE /LOCKAMOUNT ";
                    tranText += "/CASHBACK:" + cashBack + " ";
                    break;
                case 3:
                    tranText += "/TRANSACTIONTYPE:DEBITRETURN /LOCKTRANTYPE /LOCKAMOUNT ";
                    break;
                case 4:
                    tranText += "/TRANSACTIONTYPE:FORCE /LOCKTRANTYPE /LOCKAMOUNT ";
                    break;
                case 5:
                    tranText += "/TRANSACTIONTYPE:PREAUTH /LOCKTRANTYPE /LOCKAMOUNT ";
                    if (hasXToken)
                    {
                        tranText += "/XCACCOUNTID:" + CCard.XChargeToken + " ";
                        tranText += "/AUTOPROCESS ";
                        tranText += "/GETXCACCOUNTIDSTATUS ";
                    }
                    if (notRecurring)
                    {
                        tranText += "/ACCOUNT:" + CCard.CCNumberMasked + " ";
                        tranText += "/AUTOPROCESS ";
                    }
                    break;
                case 6:
                    tranText += "/TRANSACTIONTYPE:ADJUSTMENT /LOCKTRANTYPE ";//excluding /LOCKAMOUNT, amount must be editable in X-Charge to make an adjustment
                    string adjustTransactionID = "";
                    string[] noteSplit = Regex.Split(textNote.Text, "\r\n");
                    foreach (string XCTrans in noteSplit)
                    {
                        if (XCTrans.StartsWith("XCTRANSACTIONID="))
                        {
                            adjustTransactionID = XCTrans.Substring(16);
                        }
                    }
                    if (adjustTransactionID != "")
                    {
                        tranText += "/XCTRANSACTIONID:" + adjustTransactionID + " ";
                        tranText += "/AUTOPROCESS ";
                    }
                    break;
                case 7:
                    tranText += "/TRANSACTIONTYPE:VOID /LOCKTRANTYPE /LOCKAMOUNT ";
                    break;
            }
            if (_promptSignature)
            {
                tranText += "/PROMPTSIGNATURE:T /SAVESIGNATURE:T ";
            }
            else
            {
                tranText += "/PROMPTSIGNATURE:F ";
            }
            tranText += "/RECEIPTINRESULT ";//So that we can make a few changes to the receipt ourselves
            return tranText;
        }

        private void butReturn_Click(object sender, EventArgs e)
        {
            CreditCard cc = null;
            List<CreditCard> creditCards = CreditCards.Refresh(_patCur.PatNum);
            if (comboCreditCards.SelectedIndex < creditCards.Count && comboCreditCards.SelectedIndex > -1)
            {
                cc = creditCards[comboCreditCards.SelectedIndex];
            }
            if (cc == null)
            {
                MsgBox.Show(this, "Card no longer available. Return cannot be processed.");
                return;
            }
            if (!cc.IsXWeb())
            {
                MsgBox.Show(this, "Only cards that were created from XWeb can process an XWeb return.");
                return;
            }
            FormXWeb FormXW = new FormXWeb(_patCur.PatNum, cc, XWebTransactionType.CreditReturnTransaction, createPayment: false);
            FormXW.LockCardInfo = true;
            if (FormXW.ShowDialog() == DialogResult.OK)
            {
                if (FormXW.ResponseResult != null)
                {
                    textNote.Text = FormXW.ResponseResult.GetFormattedNote(false);
                    textAmount.Text = (-FormXW.ResponseResult.Amount).ToString();//XWeb amounts are always positive even for returns and voids.
                    _xWebResponse = FormXW.ResponseResult;
                    _xWebResponse.PaymentNum = _paymentCur.PayNum;
                    XWebResponses.Update(_xWebResponse);
                    butVoid.Visible = true;
                    groupXWeb.Height = 85;
                }
                MsgBox.Show(this, "Return successful.");
            }
        }

        private void butVoid_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.PaymentCreate))
            {
                return;
            }
            double amount = _xWebResponse.Amount;
            if (_xWebResponse.XTransactionType == XWebTransactionType.CreditReturnTransaction
                || _xWebResponse.XTransactionType == XWebTransactionType.DebitReturnTransaction)
            {
                amount = -amount;//The amount in an xwebresponse is always stored as a positive number.
            }
            if (MessageBox.Show(Lan.g(this, "Void the XWeb transaction of amount") + " " + amount.ToString("f") + " " + Lan.g(this, "attached to this payment?"),
                "", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            try
            {
                Cursor = Cursors.WaitCursor;
                string payNote = Lan.g(this, "Void XWeb payment made from within Open Dental");
                XWebs.VoidPayment(_patCur.PatNum, payNote, _xWebResponse.XWebResponseNum);
                Cursor = Cursors.Default;
                MsgBox.Show(this, "Void successful. A new payment has been created for this void transaction.");
            }
            catch (ODException ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void butPayConnect_Click(object sender, EventArgs e)
        {
            //if(!CanAddNewCreditCard(Programs.GetCur(ProgramName.PayConnect),PayConnect.ProgramProperties.PayConnectPreventSavingNewCC)) {
            //	return;
            //}
            //MakePayConnectTransaction();
        }

        ///<summary>Launches the PayConnect transaction window.  Returns null upon failure, otherwise returns the transaction detail as a string.
        ///If prepaidAmt is not zero, then will show the PayConnect window with the given prepaid amount and let the user enter card # and exp.
        ///A patient is not required for prepaid cards.</summary>
        public string MakePayConnectTransaction(double prepaidAmt = 0)
        {
            //if(!HasPayConnect()) {
            //	return null;
            //}
            //if(prepaidAmt==0) {//Validation for regular credit cards (not prepaid cards).
            //	if(textAmount.Text=="") {
            //		MsgBox.Show(this,"Please enter an amount first.");
            //		textAmount.Focus();
            //		return null;
            //	}
            //	if(_listSplitsCur.Count>0 && PIn.Double(textAmount.Text)!=PIn.Double(textSplitTotal.Text)
            //		&& (_listSplitsCur.Count!=1 || _listSplitsCur[0].PayPlanNum==0)) //Not one paysplit attached to payplan
            //	{
            //		MsgBox.Show(this, "Split totals must equal payment amount before running a credit card transaction.");
            //		return null;
            //	}
            //}
            //if(PIn.Date(textDate.Text).Date > DateTime.Today.Date
            //		&& !Preference.GetBool(PreferenceName.FutureTransDatesAllowed) && !Preference.GetBool(PreferenceName.AccountAllowFutureDebits))
            //{
            //	MsgBox.Show(this,"Payment date cannot be in the future.");
            //	return null;
            //}
            //CreditCard cc=null;
            //List<CreditCard> creditCards=null;
            //decimal amount=Math.Abs(PIn.Decimal(textAmount.Text));//PayConnect always wants a positive number even for voids and returns.
            //if(prepaidAmt==0) {
            //	creditCards=CreditCards.Refresh(_patCur.PatNum);
            //	if(comboCreditCards.SelectedIndex<creditCards.Count) {
            //		cc=creditCards[comboCreditCards.SelectedIndex];
            //	}
            //}
            //else {//Prepaid card
            //	amount=(decimal)prepaidAmt;
            //}
            //FormPayConnect FormP=new FormPayConnect(_paymentCur.ClinicNum,_patCur,amount,cc);
            //FormP.ShowDialog();
            //if(prepaidAmt==0 && FormP.Response!=null) {//Regular credit cards (not prepaid cards).
            //	//If PayConnect response is not null, refresh comboCreditCards and select the index of the card used for this payment if the token was saved
            //	creditCards=CreditCards.Refresh(_patCur.PatNum);
            //	AddCreditCardsToCombo(creditCards,x => x.PayConnectToken==FormP.Response.PaymentToken
            //		&&x.PayConnectTokenExp.Year==FormP.Response.TokenExpiration.Year
            //		&&x.PayConnectTokenExp.Month==FormP.Response.TokenExpiration.Month);
            //	Program prog=Programs.GetCur(ProgramName.PayConnect);
            //	//still need to add functionality for accountingAutoPay
            //	string paytype=ProgramProperties.GetPropVal(prog.ProgramNum,"PaymentType",_paymentCur.ClinicNum);//paytype could be an empty string
            //	listPayType.SelectedIndex=Defs.GetOrder(DefinitionCategory.PaymentTypes,PIn.Long(paytype));
            //	SetComboDepositAccounts();
            //}
            //string resultNote=null;
            //if(FormP.Response!=null) {
            //	resultNote=Lan.g(this,"Transaction Type")+": "+Enum.GetName(typeof(PayConnectService.transType),FormP.TranType)+Environment.NewLine+
            //		Lan.g(this,"Status")+": "+FormP.Response.Description+Environment.NewLine+
            //		Lan.g(this,"Amount")+": "+FormP.AmountCharged+Environment.NewLine+
            //		Lan.g(this,"Card Type")+": "+FormP.Response.CardType+Environment.NewLine+
            //		Lan.g(this,"Account")+": "+FormP.CardNumber.Right(4).PadLeft(FormP.CardNumber.Length,'X');
            //}
            //if(prepaidAmt!=0) {
            //	if(FormP.Response!=null && FormP.Response.StatusCode=="0") { //The transaction succeeded.
            //		return resultNote;
            //	}
            //	return null;
            //}
            //if(FormP.Response!=null) {
            //	if(FormP.Response.StatusCode=="0") { //The transaction succeeded.
            //		_isCCDeclined=false;
            //		resultNote+=Environment.NewLine
            //			+Lan.g(this,"Auth Code")+": "+FormP.Response.AuthCode+Environment.NewLine
            //			+Lan.g(this,"Ref Number")+": "+FormP.Response.RefNumber;
            //		if(FormP.TranType==PayConnectService.transType.RETURN) {
            //			textAmount.Text="-"+FormP.AmountCharged;
            //			_paymentCur.Receipt=FormP.ReceiptStr;
            //		}
            //		else if(FormP.TranType==PayConnectService.transType.AUTH) {
            //			textAmount.Text=FormP.AmountCharged;
            //		}
            //		else if(FormP.TranType==PayConnectService.transType.SALE) {
            //			textAmount.Text=FormP.AmountCharged;
            //			_paymentCur.Receipt=FormP.ReceiptStr;
            //		}
            //		if(FormP.TranType==PayConnectService.transType.VOID) {//Close FormPayment window now so the user will not have the option to hit Cancel
            //			if(IsNew) {
            //				if(!_wasCreditCardSuccessful) {
            //					textAmount.Text="-"+FormP.AmountCharged;
            //					textNote.Text+=((textNote.Text=="")?"":Environment.NewLine)+resultNote;
            //				}
            //				_paymentCur.Receipt=FormP.ReceiptStr;
            //				if(SavePaymentToDb()) {
            //					MsgBox.Show(this,"Void successful.");
            //					DialogResult=DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
            //				}
            //				return resultNote;
            //			}
            //			if(!IsNew || _wasCreditCardSuccessful) {//Create a new negative payment if the void is being run from an existing payment
            //				if(_listSplitsCur.Count==0) {
            //					AddOneSplit();
            //					FillGridSplits();
            //				}
            //				else if(_listSplitsCur.Count==1//if one split
            //					&& _listSplitsCur[0].PayPlanNum!=0//and split is on a payment plan
            //					&& _listSplitsCur[0].SplitAmt!=_paymentCur.PayAmt)//and amount doesn't match payment
            //				{
            //					_listSplitsCur[0].SplitAmt=_paymentCur.PayAmt;//make amounts match automatically
            //					textSplitTotal.Text=textAmount.Text;
            //				}
            //				_paymentCur.IsSplit=_listSplitsCur.Count>1;
            //				Payment voidPayment=_paymentCur.Clone();
            //				voidPayment.PayAmt*=-1;//the negation of the original amount
            //				voidPayment.PayNote=resultNote;
            //				voidPayment.Receipt=FormP.ReceiptStr;
            //				voidPayment.PaymentSource=CreditCardSource.PayConnect;
            //				voidPayment.ProcessStatus=ProcessStat.OfficeProcessed;
            //				voidPayment.PayNum=Payments.Insert(voidPayment);
            //				foreach(PaySplit splitCur in _listSplitsCur) {//Modify the paysplits for the original transaction to work for the void transaction
            //					PaySplit split=splitCur.Copy();
            //					split.SplitAmt*=-1;
            //					split.PayNum=voidPayment.PayNum;
            //					PaySplits.Insert(split);
            //				}
            //				string strErrorMsg=Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(_patCur.PatNum,_listSplitsCur);
            //				if(!string.IsNullOrEmpty(strErrorMsg)) {
            //					MessageBox.Show(strErrorMsg);
            //				}
            //			}
            //			MsgBox.Show(this,"Void successful.");
            //			DialogResult=DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
            //			return resultNote;
            //		}
            //		else {//Not Void
            //			_wasCreditCardSuccessful=true; //Will void the transaction if user cancels out of window.
            //		}
            //		_payConnectRequest=FormP.Request;
            //	}
            //	textNote.Text+=((textNote.Text=="")?"":Environment.NewLine)+resultNote;
            //	textNote.Select(textNote.Text.Length-1,0);
            //	textNote.ScrollToCaret();//Scroll to the end of the text box to see the newest notes.
            //	_paymentCur.PayNote=textNote.Text;
            //	_paymentCur.PaymentSource=CreditCardSource.PayConnect;
            //	_paymentCur.ProcessStatus=ProcessStat.OfficeProcessed;
            //	Payments.Update(_paymentOld,true);
            //}
            //if(!string.IsNullOrEmpty(_paymentCur.Receipt)) {
            //	butPrintReceipt.Visible=true;
            //	if(Preference.GetBool(PreferenceName.AllowEmailCCReceipt)) {
            //		butEmailReceipt.Visible=true;
            //	}
            //}
            //if(FormP.Response==null || FormP.Response.StatusCode!="0") { //The transaction failed.
            //	if(FormP.TranType==PayConnectService.transType.SALE || FormP.TranType==PayConnectService.transType.AUTH) {
            //		textAmount.Text=FormP.AmountCharged;//Preserve the amount so the user can try the payment again more easily.
            //	}
            //	_isCCDeclined=true;
            //	_wasCreditCardSuccessful=false;
            //	return null;
            //}
            //return resultNote;
            return "";
        }

        ///<summary>Returns true if payconnect is enabled and completely setup.</summary>
        private bool HasPayConnect()
        {
            //_listPaymentTypeDefs = _listPaymentTypeDefs ?? Definition.GetByCategory(DefinitionCategory.PaymentTypes);
            //Program prog = Programs.GetCur(ProgramName.PayConnect);
            //bool isSetupRequired = false;
            //if (prog.Enabled)
            //{
            //    //If clinics are disabled, _paymentCur.ClinicNum will be 0 and the Username and Password will be the 'Headquarters' or practice credentials
            //    string paymentType = ProgramProperties.GetPropVal(prog.Id, "PaymentType", _paymentCur.ClinicNum);
            //    if (string.IsNullOrEmpty(ProgramProperties.GetPropVal(prog.Id, "Username", _paymentCur.ClinicNum))
            //        || string.IsNullOrEmpty(ProgramProperties.GetPropVal(prog.Id, "Password", _paymentCur.ClinicNum))
            //        || !_listPaymentTypeDefs.Any(x => x.Id.ToString() == paymentType))
            //    {
            //        isSetupRequired = true;
            //    }
            //}
            //else
            //{//Program link not enabled.  Launch a promo site.
            //    ODException.SwallowAnyException(() =>
            //        Process.Start("http://www.opendental.com/resources/redirects/redirectpayconnect.html")
            //    );
            //    return false;
            //}
            //if (isSetupRequired)
            //{
            //    if (!Security.IsAuthorized(Permissions.Setup))
            //    {
            //        return false;
            //    }
            //    FormPayConnectSetup FormPCS = new FormPayConnectSetup();
            //    FormPCS.ShowDialog();
            //    if (FormPCS.DialogResult != DialogResult.OK)
            //    {
            //        return false;
            //    }
            //    //The user could have corrected the PayConnect bridge, recursively try again.
            //    return HasPayConnect();
            //}
            //return true;

            return false; // TODO: Implement me
        }

        private void butPaySimple_Click(object sender, MouseEventArgs e)
        {
            //if(e.Button!=MouseButtons.Left) {
            //	return;
            //}
            //if(!CanAddNewCreditCard(Programs.GetCur(ProgramName.PaySimple),PaySimple.PropertyDescs.PaySimplePreventSavingNewCC)) {
            //	return;
            //}
            //MakePaySimpleTransaction();
        }

        ///<summary>Launches the PaySimple transaction window.  Returns null upon failure, otherwise returns the transaction detail as a string.
        ///If prepaidAmt is not zero, then will show the PaySimple window with the given prepaid amount and let the user enter card # and exp.
        ///A patient is not required for prepaid cards.</summary>
        public string MakePaySimpleTransaction(double prepaidAmt = 0)
        {
            //if(!HasPaySimple()) {
            //	return null;
            //}
            //CreditCard cc=null;
            //List<CreditCard> creditCards=null;
            //decimal amount=Math.Abs(PIn.Decimal(textAmount.Text));//PaySimple always wants a positive number even for voids and returns.
            //if(prepaidAmt==0) {
            //	creditCards=CreditCards.Refresh(_patCur.PatNum);
            //	if(comboCreditCards.SelectedIndex<creditCards.Count) {
            //		cc=creditCards[comboCreditCards.SelectedIndex];
            //	}
            //}
            //else {//Prepaid card
            //	amount=(decimal)prepaidAmt;
            //}
            //FormPaySimple form=new FormPaySimple(_paymentCur.ClinicNum,_patCur,amount,cc);
            //form.ShowDialog();
            //Program prog=Programs.GetCur(ProgramName.PaySimple);
            //if(prepaidAmt==0) {//Regular credit cards (not prepaid cards).
            //	//If PaySimple response is not null, refresh comboCreditCards and select the index of the card used for this payment if the token was saved
            //	creditCards=CreditCards.Refresh(_patCur.PatNum);
            //	string paySimpleToken=cc==null ? "" : cc.PaySimpleToken;
            //	if(form.ApiResponseOut!=null) {
            //		paySimpleToken=form.ApiResponseOut.PaySimpleToken;
            //	}
            //	AddCreditCardsToCombo(creditCards,x => x.PaySimpleToken==paySimpleToken && !string.IsNullOrEmpty(paySimpleToken));
            //	//still need to add functionality for accountingAutoPay
            //	string paytype=ProgramProperties.GetPropValForClinicOrDefault(prog.ProgramNum,PaySimple.PropertyDescs.PaySimplePayTypeCC,_paymentCur.ClinicNum);//paytype could be an empty string
            //	listPayType.SelectedIndex=Defs.GetOrder(DefinitionCategory.PaymentTypes,PIn.Long(paytype));
            //	SetComboDepositAccounts();
            //}
            //if(prepaidAmt!=0) {
            //	if(form.ApiResponseOut!=null) { //The transaction succeeded.
            //		return form.ApiResponseOut.ToNoteString();
            //	}
            //	return null;
            //}
            //string resultNote=null;
            //if(form.ApiResponseOut!=null) { //The transaction succeeded.
            //	_isCCDeclined=false;
            //	resultNote=form.ApiResponseOut.ToNoteString();
            //	_paymentCur.PaymentSource=form.ApiResponseOut.CCSource;
            //	if(form.ApiResponseOut.CCSource==CreditCardSource.PaySimpleACH) {
            //		string paytype=ProgramProperties.GetPropValForClinicOrDefault(prog.ProgramNum,PaySimple.PropertyDescs.PaySimplePayTypeACH,
            //			_paymentCur.ClinicNum);
            //		int defOrder=Defs.GetOrder(DefinitionCategory.PaymentTypes,PIn.Long(paytype));
            //		//paytype could be an empty string, so then leave listPayType as it was.
            //		if(defOrder>=-1) {
            //			listPayType.SelectedIndex=defOrder;
            //		}
            //	}
            //	if(form.ApiResponseOut.TransType==PaySimple.TransType.RETURN) {
            //		textAmount.Text="-"+form.ApiResponseOut.Amount.ToString("F");
            //		_paymentCur.Receipt=form.ApiResponseOut.TransactionReceipt;
            //	}
            //	else if(form.ApiResponseOut.TransType==PaySimple.TransType.AUTH) {
            //		textAmount.Text=form.ApiResponseOut.Amount.ToString("F");
            //	}
            //	else if(form.ApiResponseOut.TransType==PaySimple.TransType.SALE) {
            //		textAmount.Text=form.ApiResponseOut.Amount.ToString("F");
            //		_paymentCur.Receipt=form.ApiResponseOut.TransactionReceipt;
            //	}
            //	if(form.ApiResponseOut.TransType==PaySimple.TransType.VOID) {//Close FormPayment window now so the user will not have the option to hit Cancel
            //		if(IsNew) {
            //			if(!_wasCreditCardSuccessful) {
            //				textAmount.Text="-"+form.ApiResponseOut.Amount.ToString("F");
            //				textNote.Text+=((textNote.Text=="") ? "" : Environment.NewLine)+resultNote;
            //			}
            //			_paymentCur.Receipt=form.ApiResponseOut.TransactionReceipt;
            //			if(SavePaymentToDb()) {
            //				MsgBox.Show(this,"Void successful.");
            //				DialogResult=DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
            //			}
            //			return resultNote;
            //		}
            //		if(!IsNew || _wasCreditCardSuccessful) {//Create a new negative payment if the void is being run from an existing payment
            //			if(_listSplitsCur.Count==0) {
            //				AddOneSplit();
            //				FillGridSplits();
            //			}
            //			else if(_listSplitsCur.Count==1//if one split
            //				&& _listSplitsCur[0].PayPlanNum!=0//and split is on a payment plan
            //				&& _listSplitsCur[0].SplitAmt!=_paymentCur.PayAmt)//and amount doesn't match payment
            //			{
            //				_listSplitsCur[0].SplitAmt=_paymentCur.PayAmt;//make amounts match automatically
            //				textSplitTotal.Text=textAmount.Text;
            //			}
            //			_paymentCur.IsSplit=_listSplitsCur.Count>1;
            //			Payment voidPayment=_paymentCur.Clone();
            //			voidPayment.PayAmt*=-1;//the negation of the original amount
            //			voidPayment.PayNote=resultNote;
            //			voidPayment.Receipt=form.ApiResponseOut.TransactionReceipt;
            //			voidPayment.PaymentSource=CreditCardSource.PaySimple;
            //			voidPayment.ProcessStatus=ProcessStat.OfficeProcessed;
            //			voidPayment.PayNum=Payments.Insert(voidPayment);
            //			foreach(PaySplit splitCur in _listSplitsCur) {//Modify the paysplits for the original transaction to work for the void transaction
            //				PaySplit split=splitCur.Copy();
            //				split.SplitAmt*=-1;
            //				split.PayNum=voidPayment.PayNum;
            //				PaySplits.Insert(split);
            //			}
            //			string strErrorMsg=Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(_patCur.PatNum,_listSplitsCur);
            //			if(!string.IsNullOrEmpty(strErrorMsg)) {
            //				MessageBox.Show(strErrorMsg);
            //			}
            //		}
            //		MsgBox.Show(this,"Void successful.");
            //		DialogResult=DialogResult.OK;//Close FormPayment window now so the user will not have the option to hit Cancel
            //		return resultNote;
            //	}
            //	else {//Not Void
            //		_wasCreditCardSuccessful=true; //Will void the transaction if user cancels out of window.
            //	}
            //	_paySimpleResponse=form.ApiResponseOut;
            //}
            //if(!string.IsNullOrWhiteSpace(resultNote)) {
            //	textNote.Text+=((textNote.Text=="") ? "" : Environment.NewLine)+resultNote;
            //}
            //textNote.Select(Math.Max(textNote.Text.Length-1,textNote.Text.Length),0);
            //textNote.ScrollToCaret();//Scroll to the end of the text box to see the newest notes.
            //_paymentCur.PayNote=textNote.Text;
            //if(_paymentCur.PaymentSource==CreditCardSource.None) {
            //	_paymentCur.PaymentSource=CreditCardSource.PaySimple;
            //}
            //_paymentCur.ProcessStatus=ProcessStat.OfficeProcessed;
            //Payments.Update(_paymentCur,true);
            //if(!string.IsNullOrEmpty(_paymentCur.Receipt)) {
            //	butPrintReceipt.Visible=true;
            //	if(Preference.GetBool(PreferenceName.AllowEmailCCReceipt)) {
            //		butEmailReceipt.Visible=true;
            //	}
            //}
            //if(form.ApiResponseOut==null || form.ApiResponseOut.Status.ToLower()=="failed") { //The transaction failed.
            //	//PaySimple checks the transaction type here and sets the amount the user chose to the textAmount textbox. 
            //	//We don't have that information here so do nothing.
            //	_isCCDeclined=true;
            //	_wasCreditCardSuccessful=false;
            //	return null;
            //}
            //return resultNote;
            return "";
        }

        ///<summary>Returns true if PaySimple is enabled and completely setup.</summary>
        private bool HasPaySimple()
        {
            //_listPaymentTypeDefs=_listPaymentTypeDefs??Definition.GetByCategory(DefinitionCategory.PaymentTypes);
            //Program prog=Programs.GetCur(ProgramName.PaySimple);
            //bool isSetupRequired=false;
            //if(prog.Enabled) {
            //	//If clinics are disabled, _paymentCur.ClinicNum will be 0 and the Username and Key will be the 'Headquarters' or practice credentials
            //	string paymentType=ProgramProperties.GetPropValForClinicOrDefault(prog.ProgramNum,PaySimple.PropertyDescs.PaySimplePayTypeCC,_paymentCur.ClinicNum);
            //	if(string.IsNullOrEmpty(ProgramProperties.GetPropValForClinicOrDefault(prog.ProgramNum,PaySimple.PropertyDescs.PaySimpleApiUserName,_paymentCur.ClinicNum))
            //		|| string.IsNullOrEmpty(ProgramProperties.GetPropValForClinicOrDefault(prog.ProgramNum,PaySimple.PropertyDescs.PaySimpleApiKey,_paymentCur.ClinicNum))
            //		|| !_listPaymentTypeDefs.Any(x => x.Id.ToString()==paymentType)) 
            //	{
            //		isSetupRequired=true;
            //	}
            //}
            //else {//Program link not enabled.  Launch a promo website.
            //	ODException.SwallowAnyException(() =>
            //		Process.Start("http://www.opendental.com/resources/redirects/redirectpaysimple.html")
            //	);
            //	return false;
            //}
            //if(isSetupRequired) {
            //	if(!Security.IsAuthorized(Permissions.Setup)) {
            //		return false;
            //	}
            //	FormPaySimpleSetup form=new FormPaySimpleSetup();
            //	form.ShowDialog();
            //	if(form.DialogResult!=DialogResult.OK) {
            //		return false;
            //	}
            //	//The user could have corrected the PaySimple bridge, recursively try again.
            //	return HasPaySimple();
            //}
            return true;
        }

        ///<summary>Returns true if the user can add a new credit card.</summary>
        private bool CanAddNewCreditCard(Program prog, string progPropertyDescription)
        {
            return false; // TODO: Implement me

            //if (comboCreditCards.SelectedItem == null || comboCreditCards.SelectedItem.GetType() != typeof(ODBoxItem<CreditCard>))
            //{
            //    MsgBox.Show(this, "Invalid credit card selected.");
            //    return false;
            //}
            //bool hasPreventCcAdd = PIn.Bool(ProgramProperties.GetPropVal(prog.Id, progPropertyDescription, _paymentCur.ClinicNum));
            //CreditCard ccSelected = comboCreditCards.SelectedTag<CreditCard>();
            //if (ccSelected == null)
            //{
            //    return !hasPreventCcAdd;
            //}
            //bool hasToken = false;
            //if (prog.TypeName == ProgramName.Xcharge.ToString() && !string.IsNullOrEmpty(ccSelected.XChargeToken))
            //{
            //    hasToken = true;
            //}
            //else if (prog.TypeName == ProgramName.PayConnect.ToString() && !string.IsNullOrEmpty(ccSelected.PayConnectToken))
            //{
            //    hasToken = true;
            //}
            //else if (prog.TypeName == ProgramName.PaySimple.ToString() && !string.IsNullOrEmpty(ccSelected.PaySimpleToken))
            //{
            //    hasToken = true;
            //}
            //if (hasPreventCcAdd && (ccSelected.CreditCardNum == 0 || !hasToken))
            //{
            //    MsgBox.Show(this, "Cannot add a new credit card.");
            //    return false;
            //}
            //return true;
        }

        private void VoidPayConnectTransaction(string refNum, string amount)
        {
            //PayConnectResponse response=null;
            //string receiptStr="";
            //Cursor=Cursors.WaitCursor;
            //if(_payConnectRequest==null) {//The payment was made through the terminal.
            //	ODProgress.ShowAction(() => {
            //			PosRequest posRequest=PosRequest.CreateVoidByReference(refNum);
            //			PosResponse posResponse=DpsPos.ProcessCreditCard(posRequest);
            //			response=PayConnectTerminal.ToPayConnectResponse(posResponse);
            //			receiptStr=PayConnectTerminal.BuildReceiptString(posRequest,posResponse,null,0);
            //		},
            //		startingMessage:Lan.g(this,"Processing void on terminal."),
            //		actionException:ex => {
            //			this.Invoke(() => {
            //				Cursor=Cursors.Default;
            //				MessageBox.Show(Lan.g(this,"Error voiding payment:")+" "+ex.Message);
            //			});
            //		});
            //}
            //else {//The payment was made through the web service.
            //	_payConnectRequest.TransType=PayConnectService.transType.VOID;
            //	_payConnectRequest.RefNumber=refNum;
            //	_payConnectRequest.Amount=PIn.Decimal(amount);
            //	PayConnectService.transResponse transResponse=PayConnect.ProcessCreditCard(_payConnectRequest,_paymentCur.ClinicNum,x => MessageBox.Show(x));
            //	response=new PayConnectResponse(transResponse,_payConnectRequest);
            //	receiptStr=PayConnect.BuildReceiptString(_payConnectRequest,transResponse,null,0);
            //}
            //Cursor=Cursors.Default;
            //if(response==null || response.StatusCode!="0") {//error in transaction
            //	MsgBox.Show(this,"This credit card payment has already been processed and will have to be voided manually through the web interface.");
            //	return;
            //}
            //else {//Record a new payment for the voided transaction
            //	Payment voidPayment=_paymentCur.Clone();
            //	voidPayment.PayAmt*=-1; //The negated amount of the original payment
            //	voidPayment.Receipt=receiptStr;
            //	voidPayment.PayNote=Lan.g(this,"Transaction Type")+": "+Enum.GetName(typeof(PayConnectService.transType),PayConnectService.transType.VOID)
            //		+Environment.NewLine+Lan.g(this,"Status")+": "+response.Description+Environment.NewLine
            //		+Lan.g(this,"Amount")+": "+voidPayment.PayAmt+Environment.NewLine
            //		+Lan.g(this,"Auth Code")+": "+response.AuthCode+Environment.NewLine
            //		+Lan.g(this,"Ref Number")+": "+response.RefNumber;
            //	voidPayment.PaymentSource=CreditCardSource.PayConnect;
            //	voidPayment.ProcessStatus=ProcessStat.OfficeProcessed;
            //	voidPayment.PayNum=Payments.Insert(voidPayment);
            //	for(int i=0;i<_listSplitsCur.Count;i++) {//Modify the paysplits for the original transaction to work for the void transaction
            //		PaySplit split=_listSplitsCur[i].Copy();
            //		split.SplitAmt*=-1;
            //		split.PayNum=voidPayment.PayNum;
            //		PaySplits.Insert(split);
            //	}
            //	SecurityLog.Write(Permissions.PaymentCreate,voidPayment.PatNum,
            //		Patients.GetLim(voidPayment.PatNum).GetNameLF()+", "+voidPayment.PayAmt.ToString("c"));
            //}
        }

        private void VoidPaySimpleTransaction(string refNum, string originalReceipt)
        {
            //PaySimple.ApiResponse response=null;
            //string receiptStr="";
            //Cursor=Cursors.WaitCursor;
            //try {
            //	response=PaySimple.VoidPayment(refNum,_paymentCur.ClinicNum);
            //}
            //catch(ODException wex) {
            //	MessageBox.Show(wex.Message);//This should have already been Lans.g if applicable.
            //	return;
            //}
            //catch(Exception ex) {
            //	MessageBox.Show(Lan.g(this,"Error:")+" "+ex.Message);
            //	return;
            //}
            //string[] arrayReceiptFields=originalReceipt.Replace("\r\n","\n").Replace("\r","\n").Split(new string[] { "\n" },StringSplitOptions.RemoveEmptyEntries);
            //string ccNum="";
            //string expDateStr="";
            //string nameOnCard="";
            //for(int i=0;i<arrayReceiptFields.Length;i++) {
            //	if(arrayReceiptFields[i].StartsWith("Name")) {
            //		nameOnCard=arrayReceiptFields[i].Substring(4).Replace(".","");
            //	}
            //	if(arrayReceiptFields[i].StartsWith("Account")) {
            //		ccNum=arrayReceiptFields[i].Substring(7).Replace(".","");
            //	}
            //	if(arrayReceiptFields[i].StartsWith("Exp Date")) {
            //		expDateStr=arrayReceiptFields[i].Substring(8).Replace(".","");
            //	}
            //}
            //response.BuildReceiptString(ccNum,PIn.Int(expDateStr.Substring(0,2)),PIn.Int(expDateStr.Substring(2)),nameOnCard,_paymentCur.ClinicNum);
            //receiptStr=response.TransactionReceipt;
            //Cursor=Cursors.Default;
            //Payment voidPayment=_paymentCur.Clone();
            //voidPayment.PayAmt*=-1; //The negated amount of the original payment
            //voidPayment.Receipt=receiptStr;
            //voidPayment.PayNote=response.ToNoteString();
            //voidPayment.PaymentSource=CreditCardSource.PaySimple;
            //voidPayment.ProcessStatus=ProcessStat.OfficeProcessed;
            //voidPayment.PayNum=Payments.Insert(voidPayment);
            //for(int i=0;i<_listSplitsCur.Count;i++) {//Modify the paysplits for the original transaction to work for the void transaction
            //	PaySplit split=_listSplitsCur[i].Copy();
            //	split.SplitAmt*=-1;
            //	split.PayNum=voidPayment.PayNum;
            //	PaySplits.Insert(split);
            //}
            //SecurityLog.Write(Permissions.PaymentCreate,voidPayment.PatNum,
            //	Patients.GetLim(voidPayment.PatNum).GetNameLF()+", "+voidPayment.PayAmt.ToString("c"));
        }

        private void menuXcharge_Click(object sender, EventArgs e)
        {
            if (Security.IsAuthorized(Permissions.Setup))
            {
                FormXchargeSetup FormX = new FormXchargeSetup();
                FormX.ShowDialog();
                CheckUIState();
            }
        }

        private void menuPayConnect_Click(object sender, EventArgs e)
        {
            if (Security.IsAuthorized(Permissions.Setup))
            {
                FormPayConnectSetup fpcs = new FormPayConnectSetup();
                fpcs.ShowDialog();
                CheckUIState();
            }
        }

        private void menuPaySimple_Click(object sender, EventArgs e)
        {
            if (Security.IsAuthorized(Permissions.Setup))
            {
                FormPaySimpleSetup form = new FormPaySimpleSetup();
                form.ShowDialog();
                CheckUIState();
            }
        }

        private void comboClinic_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //_listUserClinicNums contains all clinics the user has access to as well as ClinicNum 0 for 'none'
            _paymentCur.ClinicNum = _listUserClinicNums[comboClinic.SelectedIndex];
            if (_listSplitsCur.Count > 0)
            {
                if (!MsgBox.Show(this, MsgBoxButtons.OKCancel, "Change clinic for all splits?"))
                {
                    return;
                }
                for (int i = 0; i < _listSplitsCur.Count; i++)
                {
                    _listSplitsCur[i].ClinicNum = _paymentCur.ClinicNum;
                }
                FillGridSplits();
            }
            CheckUIState();
        }

        private void checkPayTypeNone_CheckedChanged(object sender, EventArgs e)
        {
            //this fires before the click event.  The Checked property also reflects the new value.
            if (checkPayTypeNone.Checked)
            {
                listPayType.Visible = false;
                panelXcharge.Visible = false;
                butPay.Text = Lan.g(this, "Transfer");

                comboGroupBy.SelectedIndex = 2;

                comboGroupBy.Enabled = false;
                if (Preference.GetInt(PreferenceName.RigorousAccounting) == (int)RigorousAccounting.EnforceFully)
                {
                    butCreatePartial.Text = Lan.g(this, "Proc Breakdown");
                    butPay.Visible = false;
                }
                else
                {
                    butCreatePartial.Visible = false;
                }
            }
            else
            {
                listPayType.Visible = true;
                panelXcharge.Visible = true;
                butPay.Text = Lan.g(this, "Pay");
                comboGroupBy.Enabled = true;
                butCreatePartial.Visible = true;
                butCreatePartial.Text = Lan.g(this, "Add Partials");
                butPay.Visible = true;
            }
        }

        private void checkPayTypeNone_Click(object sender, EventArgs e)
        {
            List<long> listPatNumsFamily = _famCur.Members.Select(x => x.PatNum).ToList();
            if (checkShowSuperfamily.Checked)
            {
                listPatNumsFamily.AddRange(_superFamCur.Members.Select(x => x.PatNum).ToList());
                listPatNumsFamily = listPatNumsFamily.Distinct().ToList();
            }
            _loadData.ConstructChargesData = PaymentEdit.GetConstructChargesData(listPatNumsFamily, _patCur.PatNum, _loadData.ListSplits, _paymentCur.PayNum, checkPayTypeNone.Checked);
            Init(_loadData, false);
        }

        ///<summary>Allows editing of an individual double clicked paysplit entry.</summary>
        private void gridSplits_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            PaySplit paySplitOld = (PaySplit)gridSplits.Rows[e.Row].Tag;
            PaySplit paySplit = paySplitOld.Copy();
            if (paySplit.DateEntry != DateTime.MinValue && !Security.IsAuthorized(Permissions.PaymentEdit, paySplit.DatePay, false))
            {
                return;
            }
            PaySplits.PaySplitAssociated splitAssociatedOld = _listPaySplitsAssociated.Find(x => x.PaySplitLinked == paySplitOld);
            FormPaySplitEdit FormPSE = new FormPaySplitEdit(_curFamOrSuperFam, checkPayTypeNone.Checked);
            FormPSE.ListSplitsCur = _listSplitsCur;
            FormPSE.PaySplitCur = paySplit;
            FormPSE.SplitAssociated = splitAssociatedOld ?? new PaySplits.PaySplitAssociated(null, null);
            FormPSE.ListPaySplitAssociated = _listPaySplitsAssociated;
            if (paySplit.IsInterestSplit && !MsgBox.Show(this, MsgBoxButtons.OKCancel, "Editing or deleting interest splits for a payment plan charge can"
                + " cause future splits to be allocated to the wrong provider. Do you want to continue?"))
            {
                return;
            }
            else if (FormPSE.ShowDialog() == DialogResult.OK)
            {//paySplit contains all the info we want.  
             //Delete paysplit from paysplit grid, credit the charge it's associated to.  
             //Paysplit may be re-associated with a different charge and we wouldn't know, so we need to do this.
             //Paysplits associated to payplancharge cannot be associated to a different payplancharge from this window.
                if (FormPSE.PaySplitCur == null)
                {//remove old association from list.
                    _listPaySplitsAssociated.Remove(splitAssociatedOld);
                }
                else
                {
                    //Since DeleteSelected() deletes the current selected paysplit from the list of ListPaySplitCur, we need to do this in order to maintain object references on form closing.
                    //detaching split in PSE window causes SplitAssociated to be null
                    if (FormPSE.SplitAssociated == null)
                    {
                        _listPaySplitsAssociated.Remove(splitAssociatedOld);
                    }
                    else
                    {
                        PaySplits.PaySplitAssociated splitAssociatedNew = new PaySplits.PaySplitAssociated(FormPSE.SplitAssociated.PaySplitOrig, paySplit);
                        _listPaySplitsAssociated.Remove(splitAssociatedOld);
                        _listPaySplitsAssociated.Add(splitAssociatedNew);
                    }
                }
                List<long> listEditedCharge = DeleteSelected(paySplit);
                if (paySplit != null && !_dictPatients.ContainsKey(paySplit.PatNum))
                {
                    //add new patnum to _dictPatients
                    Patient pat = Patients.GetLim(paySplit.PatNum);
                    if (pat != null)
                    {
                        _dictPatients[paySplit.PatNum] = pat;
                    }
                }
                long payPlanChargeNum = 0;
                if (listEditedCharge.Count > 0)
                {
                    payPlanChargeNum = listEditedCharge[0];
                }
                if (FormPSE.PaySplitCur == null)
                {//Deleted the paysplit, just return here.
                    FillGridSplits();
                    return;
                }
                if (!_listSplitsCur.Contains(paySplitOld))
                {//Paysplit was -actually- deleted from the list.
                    _listSplitsCur.Add(paySplit);
                    PaymentEdit.UpdateForManualSplit(paySplit, _listAccountCharges, payPlanChargeNum);
                    FillGridSplits();
                    _paymentCur.PayAmt -= paySplit.SplitAmt;
                }
            }
        }

        ///<summary>When a paysplit is selected this method highlights all charges associated with it.</summary>
        private void gridSplits_CellClick(object sender, ODGridClickEventArgs e)
        {
            HighlightChargesForSplits();
        }

        ///<summary>When a charge is selected this method highlights all paysplits associated with it.</summary>
        private void gridCharges_CellClick(object sender, ODGridClickEventArgs e)
        {
            gridSplits.SetSelected(false);
            decimal chargeTotal = 0;
            for (int i = 0; i < gridCharges.SelectedIndices.Length; i++)
            {
                if (comboGroupBy.SelectedIndex > 0)
                {
                    List<AccountEntry> listAccountEntryCharges = (List<AccountEntry>)gridCharges.Rows[gridCharges.SelectedIndices[i]].Tag;
                    chargeTotal += listAccountEntryCharges.Sum(x => x.AmountEnd);
                    for (int j = 0; j < gridSplits.Rows.Count; j++)
                    {
                        PaySplit paySplit = (PaySplit)gridSplits.Rows[j].Tag;
                        foreach (AccountEntry entry in listAccountEntryCharges)
                        {
                            if (entry.SplitCollection.Contains(paySplit))
                            {
                                gridSplits.SetSelected(j, true);
                            }
                            if (entry.GetType() == typeof(PayPlanCharge))
                            {
                                if (paySplit.PayPlanNum == ((PayPlanCharge)entry.Tag).PayPlanNum)
                                {
                                    gridSplits.SetSelected(j, true);
                                }
                            }
                        }
                    }
                }
                else
                {
                    AccountEntry accountEntryCharge = (AccountEntry)gridCharges.Rows[gridCharges.SelectedIndices[i]].Tag;
                    for (int j = 0; j < gridSplits.Rows.Count; j++)
                    {
                        PaySplit paySplit = (PaySplit)gridSplits.Rows[j].Tag;
                        if (accountEntryCharge.SplitCollection.Contains(paySplit))
                        {
                            gridSplits.SetSelected(j, true);
                        }
                        if (accountEntryCharge.GetType() == typeof(PayPlanCharge))
                        {
                            if (paySplit.PayPlanNum == ((PayPlanCharge)accountEntryCharge.Tag).PayPlanNum)
                            {
                                gridSplits.SetSelected(j, true);
                            }
                        }
                    }
                    chargeTotal += accountEntryCharge.AmountEnd;
                }
            }
            textChargeTotal.Text = chargeTotal.ToString("f");
        }

        ///<summary>Highlights the charges that corresponds to the selected paysplit.</summary>
        private void HighlightChargesForSplits()
        {
            gridCharges.SetSelected(false);
            for (int i = 0; i < gridSplits.SelectedIndices.Length; i++)
            {
                PaySplit paySplit = (PaySplit)gridSplits.Rows[gridSplits.SelectedIndices[i]].Tag;
                for (int j = 0; j < gridCharges.Rows.Count; j++)
                {
                    if (comboGroupBy.SelectedIndex > 0)
                    {
                        List<AccountEntry> listEntriesForProv = (List<AccountEntry>)gridCharges.Rows[j].Tag;
                        foreach (AccountEntry entry in listEntriesForProv)
                        {
                            if (entry.SplitCollection.Contains(paySplit))
                            {
                                gridCharges.SetSelected(j, true);
                            }
                        }
                    }
                    else
                    {
                        AccountEntry accountEntryCharge = (AccountEntry)gridCharges.Rows[j].Tag;
                        if (accountEntryCharge.SplitCollection.Contains(paySplit))
                        {
                            gridCharges.SetSelected(j, true);
                        }
                    }
                }
                for (int k = 0; k < gridSplits.Rows.Count; k++)
                {
                    PaySplit splitAttached = (PaySplit)gridSplits.Rows[k].Tag;
                    PaySplits.PaySplitAssociated psAssociated = _listPaySplitsAssociated.Find(x => x.PaySplitOrig == paySplit && x.PaySplitLinked == splitAttached);
                    if (psAssociated != null)
                    {
                        gridSplits.SetSelected(k, true);
                    }
                }
            }
        }

        ///<summary>Deletes selected paysplits from the grid and attributes amounts back to where they originated from.
        ///This will return a list of payment plan charges that were affected. This is so that splits can be correctly re-attributed to the payplancharge
        ///when the user edits the paysplit. There should only ever be one payplancharge in that list, since the user can only edit one split at a time.</summary>
        private List<long> DeleteSelected(PaySplit paySplitToBeAdded = null)
        {
            bool suppressMessage = false;
            //we need to return the payplancharge that the paysplit was associated to so that this paysplit can be correctly re-attributed to that charge.
            List<long> listPayPlanChargeNum = new List<long>();
            for (int i = gridSplits.SelectedIndices.Length - 1; i >= 0; i--)
            {
                int idx = gridSplits.SelectedIndices[i];
                PaySplit paySplit = (PaySplit)gridSplits.Rows[idx].Tag;
                if (paySplit.SplitNum != 0 && _listPaySplitAllocations.FindAll(x => x.FSplitNum == paySplit.SplitNum && x.PayNum != paySplit.PayNum).Count > 0)
                {
                    MsgBox.Show(this, "Splits in this payment have been allocated elsewhere.  Please delete those first.");
                    return new List<long>();
                }
                if (paySplit.DateEntry != DateTime.MinValue && !Security.IsAuthorized(Permissions.PaymentEdit, paySplit.DatePay, suppressMessage))
                {
                    suppressMessage = true;
                    continue;//Don't delete this paysplit
                }
                if (paySplit.PayPlanNum != 0)
                {
                    foreach (AccountEntry charge in _listAccountCharges)
                    {
                        if (charge.GetType() != typeof(PayPlanCharge))
                        {//If the charge is not a payplancharge, we don't care
                            continue;
                        }
                        if (!charge.SplitCollection.Contains(paySplit))
                        {//Only care about the charge if this paysplit is for that charge.
                            continue;
                        }
                        if (paySplit.SplitAmt <= 0)
                        {
                            break;
                        }
                        //It is now a charge for the payplan
                        //When a split is deleted, put the money back on the payplan charge
                        listPayPlanChargeNum.Add(charge.PriKey);
                        decimal chargeAmtNew = charge.AmountEnd + (decimal)paySplit.SplitAmt;//Take the current value of the charge and add the split amt to it
                        if (Math.Abs(chargeAmtNew) > Math.Abs(charge.AmountStart))
                        {//The split has more in it than the debit can take, use only a part of it
                         //Find out how much of the split goes into the debit
                            decimal debitDifference = charge.AmountStart - charge.AmountEnd;
                            paySplit.SplitAmt -= (double)debitDifference;
                            charge.AmountEnd = charge.AmountStart;
                            _paymentCur.PayAmt += (double)debitDifference;
                            if (paySplit.ProcNum != 0)
                            {
                                Procedure proc = (Procedure)_listAccountCharges.Find(x => x.GetType() == typeof(Procedure) && x.PriKey == paySplit.ProcNum).Tag;
                                proc.ProcFee += (double)debitDifference;//Put money back on the procfee so when we add splits later it calculates correctly
                            }
                        }
                        else
                        {
                            charge.AmountEnd += (decimal)paySplit.SplitAmt;//Give the money back to the charge so it will display.  Uses full paysplit amount.
                            _paymentCur.PayAmt += paySplit.SplitAmt;
                            if (paySplit.ProcNum != 0)
                            {
                                Procedure proc = (Procedure)_listAccountCharges.Find(x => x.GetType() == typeof(Procedure) && x.PriKey == paySplit.ProcNum).Tag;
                                proc.ProcFee += paySplit.SplitAmt;//Put money back on the procfee so when we add splits later it calculates correctly
                            }
                            paySplit.SplitAmt = 0;
                        }
                        charge.SplitCollection.Remove(paySplit);
                    }
                }
                else if (paySplit.ProcNum != 0)
                {
                    for (int j = 0; j < _listAccountCharges.Count; j++)
                    {
                        AccountEntry charge = _listAccountCharges[j];
                        if (!charge.SplitCollection.Contains(paySplit))
                        {
                            continue;
                        }
                        charge.AmountEnd += (decimal)paySplit.SplitAmt;//Give the money back to the charge so it will display.
                        charge.SplitCollection.Remove(paySplit);
                    }
                }
                else if (paySplit.FSplitNum != 0)
                {//Most likely a negative split used to income transfer.
                    for (int j = 0; j < _listAccountCharges.Count; j++)
                    {
                        AccountEntry charge = _listAccountCharges[j];
                        if (!charge.SplitCollection.Contains(paySplit))
                        {
                            continue;
                        }
                        charge.AmountEnd += (decimal)paySplit.SplitAmt;
                    }
                }
                else
                {//Adjustments
                    for (int j = 0; j < _listAccountCharges.Count; j++)
                    {
                        AccountEntry charge = _listAccountCharges[j];
                        if (!charge.SplitCollection.Contains(paySplit))
                        {
                            continue;
                        }
                        charge.AmountEnd += (decimal)paySplit.SplitAmt;//Give the money back to the charge so it will display.
                        charge.SplitCollection.Remove(paySplit);
                    }
                }
                if (paySplitToBeAdded != null)
                {
                    //We need to do this in order to maintain object references on form closing. 		
                    UpdateListAssociationsBeforeDelete(paySplit, paySplitToBeAdded);
                }
                _listSplitsCur.Remove(paySplit);
            }
            return listPayPlanChargeNum;
        }

        ///<summary>Goes through _listPaySplitsAssociated and replaces the paySplitToBeDeleted with paySplitToBeAdded.</summary>
        private void UpdateListAssociationsBeforeDelete(PaySplit paySplitToBeDeleted, PaySplit paySplitToBeAdded)
        {
            List<PaySplits.PaySplitAssociated> listPaySplitAssociatedOrig = _listPaySplitsAssociated.FindAll(x => x.PaySplitOrig == paySplitToBeDeleted);
            foreach (PaySplits.PaySplitAssociated payAssociatedLink in _listPaySplitsAssociated.Where(x => x.PaySplitLinked == paySplitToBeDeleted))
            {
                payAssociatedLink.PaySplitLinked = paySplitToBeAdded;
            }
            foreach (PaySplits.PaySplitAssociated payAssociatedOrig in _listPaySplitsAssociated.FindAll(x => x.PaySplitOrig == paySplitToBeDeleted))
            {
                payAssociatedOrig.PaySplitOrig = paySplitToBeAdded;
            }
        }

        ///<summary>Creates a split similar to how CreateSplitsForPayment does it, but with selected rows of the grid.
        ///If payAmt==0, attempt to pay charge in full.</summary>
        private void CreateSplit(AccountEntry charge, decimal payAmt, bool isManual = false)
        {
            PaymentEdit.PayResults createdSplit = PaymentEdit.CreatePaySplit(charge.AccountEntryNum, payAmt, _paymentCur, PIn.Decimal(textAmount.Text)
                , _listSplitsCur, _listAccountCharges, isManual);
            _listSplitsCur = createdSplit.ListSplitsCur;
            _listAccountCharges = createdSplit.ListAccountCharges;
            _paymentCur = createdSplit.Payment;
        }

        ///<summary>Creates paysplits associated to the patient passed in for the current payment until the payAmt has been met.  
        ///Returns the list of new paysplits that have been created.  PaymentAmt will attempt to move toward 0 as paysplits are created.</summary>
        private List<PaySplit> AutoSplitForPayment(DateTime date, PaymentEdit.LoadData loadData = null)
        {
            PaymentEdit.ConstructResults constructResults = PaymentEdit.ConstructAndLinkChargeCredits(_listPatNums, _patCur.PatNum, _listSplitsCur, _paymentCur
                , ListEntriesPayFirst, checkPayTypeNone.Checked, _preferCurrentPat, loadData);
            //Create Auto-splits for the current payment to any remaining non-zero charges FIFO by date.
            //At this point we have a list of procs, positive adjustments, and payplancharges that require payment if the Amount>0.   
            //Create and associate new paysplits to their respective charge items.
            PaymentEdit.AutoSplit autoSplit = PaymentEdit.AutoSplitForPayment(constructResults);
            _listAccountCharges = autoSplit.ListAccountCharges;
            _listSplitsCur = autoSplit.ListSplitsCur;
            _paymentCur.PayAmt = autoSplit.Payment.PayAmt;
            return autoSplit.ListAutoSplits;
        }

        private void butDeleteSplits_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridSplits.SelectedIndices.Length; i++)
            {
                if (((PaySplit)gridSplits.Rows[gridSplits.SelectedIndices[i]].Tag).IsInterestSplit)
                {
                    if (MsgBox.Show(this, MsgBoxButtons.OKCancel, "Deleting interest splits for a payment plan charge can cause line item accounting"
                        + " to appear to be off in the payment window. Do you want to continue?"))
                    {
                        break;//they clicked continue.
                    }
                    else
                    {
                        return;//return out of this method -- they don't want to delete.
                    }
                }
            }
            DeleteSelected();
            FillGridSplits();
        }

        ///<summary>Deletes all payment splits in the grid.</summary>
        private void butDeleteAllSplits_Click(object sender, EventArgs e)
        {
            gridSplits.SetSelected(true);
            DeleteSelected();
            FillGridSplits();
        }

        ///<summary>Creates a paysplit for the user to edit manually.</summary>
        private void butAddManualSplit_Click(object sender, EventArgs e)
        {
            PaySplit paySplit = new PaySplit();
            paySplit.SplitAmt = 0;
            paySplit.DatePay = _paymentCur.PayDate;
            paySplit.DateEntry = MiscData.GetNowDateTime();//just a nicety for the user.  Insert uses server time.
            paySplit.PayNum = _paymentCur.PayNum;
            paySplit.ProvNum = Patients.GetProvNum(_patCur);
            paySplit.ClinicNum = _paymentCur.ClinicNum;
            paySplit.IsNew = true;
            FormPaySplitEdit FormPSE = new FormPaySplitEdit(_curFamOrSuperFam, checkPayTypeNone.Checked);
            FormPSE.ListSplitsCur = _listSplitsCur;
            FormPSE.PaySplitCur = paySplit;
            FormPSE.IsNew = true;
            FormPSE.ListPaySplitAssociated = _listPaySplitsAssociated;
            if (FormPSE.ShowDialog() == DialogResult.OK)
            {
                if (!_dictPatients.ContainsKey(paySplit.PatNum))
                {
                    //add new patnum to _dictPatients
                    Patient pat = Patients.GetLim(paySplit.PatNum);
                    if (pat != null)
                    {
                        _dictPatients[paySplit.PatNum] = pat;
                    }
                }
                long prePaymentOrigNum = 0;
                if (FormPSE.SplitAssociated != null && FormPSE.SplitAssociated.PaySplitOrig != null && FormPSE.SplitAssociated.PaySplitLinked != null)
                {
                    if (paySplit.SplitAmt < 0)
                    {
                        //if prepayment, check the charge grid for the original prepayment so we can update the charge grid amounts.
                        List<AccountEntry> listAccountEntries = new List<AccountEntry>();
                        if (comboGroupBy.SelectedIndex != 0)
                        {//Either 'Provider' or 'Clinic and Provider' selected
                            listAccountEntries = gridCharges.Rows.SelectMany(x => x.Tag as List<AccountEntry>).ToList();
                        }
                        else
                        {//'None' selected
                            listAccountEntries = gridCharges.Rows.Select(x => x.Tag as AccountEntry).ToList();
                        }
                        AccountEntry charge = listAccountEntries.FirstOrDefault(x => x.PriKey == FormPSE.SplitAssociated.PaySplitOrig.SplitNum);
                        if (charge != null)
                        {
                            prePaymentOrigNum = charge.PriKey;
                        }
                    }
                    _listPaySplitsAssociated.Add(FormPSE.SplitAssociated);
                }
                _listSplitsCur.Add(paySplit);
                PaymentEdit.UpdateForManualSplit(paySplit, _listAccountCharges, FormPSE.PayPlanChargeNum, prePaymentOrigNum);
                FillGridSplits();
            }
        }

        private void butPay_Click(object sender, EventArgs e)
        {
            _paymentCur.PayAmt = PIn.Double(textAmount.Text) - _listSplitsCur.Sum(x => x.SplitAmt);
            tabControlSplits.SelectedIndex = 0;
            List<List<AccountEntry>> listSelectedCharges = new List<List<AccountEntry>>();
            if (gridCharges.SelectedIndices.Length == 0)
            {//Nothing selected, Select All
                foreach (ODGridRow row in gridCharges.Rows)
                {
                    if (comboGroupBy.SelectedIndex > 0)
                    {//Group by provider and/or clinic
                        listSelectedCharges.Add((List<AccountEntry>)row.Tag);
                    }
                    else
                    {
                        listSelectedCharges.Add(new List<AccountEntry> { (AccountEntry)row.Tag });
                    }
                }
            }
            else
            {
                for (int i = 0; i < gridCharges.SelectedIndices.Length; i++)
                {
                    if (comboGroupBy.SelectedIndex > 0)
                    {//Group by provider and/or clinic
                        listSelectedCharges.Add((List<AccountEntry>)gridCharges.Rows[gridCharges.SelectedIndices[i]].Tag);
                    }
                    else
                    {
                        listSelectedCharges.Add(new List<AccountEntry> { (AccountEntry)gridCharges.Rows[gridCharges.SelectedIndices[i]].Tag });
                    }
                }
            }
            PaymentEdit.PayResults createdSplits = PaymentEdit.MakePayment(listSelectedCharges, _listSplitsCur, checkShowAll.Checked, _paymentCur
                , comboGroupBy.SelectedIndex, checkPayTypeNone.Checked, PIn.Decimal(textAmount.Text), _listAccountCharges);
            _listSplitsCur = createdSplits.ListSplitsCur;
            _listAccountCharges = createdSplits.ListAccountCharges;
            _paymentCur = createdSplits.Payment;
            _paymentCur.PayAmt = PIn.Double(textAmount.Text);//Reset it
            FillGridSplits();//Fills charge grid too.
        }

        private void butCreatePartialSplit_Click(object sender, EventArgs e)
        {
            if (checkPayTypeNone.Checked)
            {//Button only visible if EnforceFully is enabled
                FormPaySplitManage FormPSM = new FormPaySplitManage(checkPayTypeNone.Checked);
                FormPSM.ListSplitsCur = _listSplitsCur;
                FormPSM.ListSplitsAssociated = _listPaySplitsAssociated;
                FormPSM.PayDate = PIn.Date(textDate.Text);
                FormPSM.PatCur = _patCur;
                FormPSM.PaymentAmt = 0;
                FormPSM.PaymentCur = _paymentCur;
                FormPSM.FamCur = _famCur;
                if (FormPSM.ShowDialog() == DialogResult.OK)
                {
                    FillGridSplits();
                }
                return;
            }
            bool doCheckOverPaidProc = true;
            for (int i = 0; i < gridCharges.SelectedIndices.Length; i++)
            {
                string chargeDescript = "";
                if (comboGroupBy.SelectedIndex > 0)
                {
                    chargeDescript = gridCharges.Rows[gridCharges.SelectedIndices[i]].Cells[0].Text;
                    FormAmountEdit FormAE = new FormAmountEdit(chargeDescript);
                    FormAE.Amount = PIn.Decimal(gridCharges.Rows[gridCharges.SelectedIndices[i]].Cells[4].Text);
                    FormAE.ShowDialog();
                    if (FormAE.DialogResult == DialogResult.OK)
                    {
                        decimal amount = FormAE.Amount;
                        if (amount == 0)
                        {
                            continue;
                        }
                        List<AccountEntry> listEntries = (List<AccountEntry>)gridCharges.Rows[gridCharges.SelectedIndices[i]].Tag;
                        //Order the list by AmountEnd to create all of paysplit that do not result in an overpayment before allowing the user to cancel out. 
                        listEntries = listEntries.OrderByDescending(x => x.AmountEnd).ToList();
                        for (int j = 0; j < listEntries.Count; j++)
                        {
                            AccountEntry entry = listEntries[j];
                            if (doCheckOverPaidProc && j == listEntries.Count - 1 && PaymentEdit.IsProcOverPaid(amount, entry))
                            {//if last account entry and overpaying
                                if (!MsgBox.Show(this, MsgBoxButtons.YesNo, "At least one of the procedure's remaining amount is negative. Continue?"))
                                {
                                    FillGridSplits();//Fills charge grid too.
                                    return;//return to the payment window. 
                                }
                                doCheckOverPaidProc = false;//user wants to manually create a split that overpays the charge. No need to check for overpaid procedures again.
                            }
                            CreateSplit(entry, amount, !doCheckOverPaidProc);
                            if (_listSplitsCur.Count > 0)
                            {
                                amount -= (decimal)_listSplitsCur[_listSplitsCur.Count - 1].SplitAmt;
                            }
                            if (amount.IsLessThanOrEqualToZero())
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    chargeDescript = gridCharges.Rows[gridCharges.SelectedIndices[i]].Cells[4].Text;

                    FormAmountEdit FormAE = new FormAmountEdit(chargeDescript);
                    AccountEntry selectedEntry = (AccountEntry)gridCharges.Rows[gridCharges.SelectedIndices[i]].Tag;
                    FormAE.Amount = selectedEntry.AmountEnd;
                    FormAE.ShowDialog();
                    if (FormAE.DialogResult == DialogResult.OK)
                    {
                        decimal amount = FormAE.Amount;
                        if (amount != 0)
                        {
                            if (PaymentEdit.IsProcOverPaid(amount, selectedEntry) && !MsgBox.Show(this, MsgBoxButtons.YesNo, "Remaining amount is negative. Continue?"))
                            {
                                break;
                            }
                            CreateSplit(selectedEntry, amount, true);
                        }
                    }
                }
            }
            FillGridSplits();//Fills charge grid too.
        }

        private void checkShowAll_Clicked(object sender, EventArgs e)
        {
            FillGridSplits();
        }

        ///<summary>Constructs a list of AccountCharges and goes through and links those charges to credits.</summary>
        private void checkShowSuperfamily_Click(object sender, EventArgs e)
        {
            if (_patCur.SuperFamily == 0)
            { //if no super family, just return.
                return;
            }
            List<long> listPatNumsFamily = _famCur.Members.Select(x => x.PatNum).ToList();
            if (checkShowSuperfamily.Checked)
            {
                listPatNumsFamily.AddRange(_superFamCur.Members.Select(x => x.PatNum).ToList());
                listPatNumsFamily = listPatNumsFamily.Distinct().ToList();
            }
            _loadData.ConstructChargesData = PaymentEdit.GetConstructChargesData(listPatNumsFamily, _patCur.PatNum, _loadData.ListSplits, _paymentCur.PayNum, checkPayTypeNone.Checked);
            Init(_loadData, false);
        }

        private void comboGroupBy_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Go through and disable/enable filters depending on the group by state.
            if (comboGroupBy.SelectedIndex == 1)
            {   //Group By Providers
                comboTypeFilter.ArraySelectedIndices = new int[] { 0 }; //Make sure "All" is selected.
                comboClinicFilter.ArraySelectedIndices = new int[] { 0 };
                comboTypeFilter.Enabled = false;
                comboClinicFilter.Enabled = false;
            }
            else if (comboGroupBy.SelectedIndex == 2)
            {   //Group by providers and clinics
                comboTypeFilter.ArraySelectedIndices = new int[] { 0 };
                comboTypeFilter.Enabled = false;
                comboClinicFilter.Enabled = true;
            }
            else
            {       //Not grouping by anything
                comboTypeFilter.Enabled = true;
                comboClinicFilter.Enabled = true;
            }
            FillGridSplits();
        }

        private void butDeletePayment_Click(object sender, System.EventArgs e)
        {
            if (textDeposit.Visible)
            {//this will get checked again by the middle layer
                MsgBox.Show(this, "This payment is attached to a deposit.  Not allowed to delete.");
                return;
            }
            if (PaySplits.GetSplitsForPrepay(_listSplitsCur).Count > 0)
            {
                MsgBox.Show(this, "This prepayment has been allocated.  Not allowed to delete.");
                return;
            }
            if (!MsgBox.Show(this, true, "This will delete the entire payment and all splits."))
            {
                return;
            }
            //If payment is attached to a transaction which is more than 48 hours old, then not allowed to delete.
            //This is hard coded.  User would have to delete or detach from within transaction rather than here.
            Transaction trans = Transactions.GetAttachedToPayment(_paymentCur.PayNum);
            if (trans != null)
            {
                if (trans.DateTimeEntry < MiscData.GetNowDateTime().AddDays(-2))
                {
                    MsgBox.Show(this, "Not allowed to delete.  This payment is already attached to an accounting transaction.  You will need to detach it from "
                        + "within the accounting section of the program.");
                    return;
                }
                if (Transactions.IsReconciled(trans))
                {
                    MsgBox.Show(this, "Not allowed to delete.  This payment is attached to an accounting transaction that has been reconciled.  You will need "
                        + "to detach it from within the accounting section of the program.");
                    return;
                }
                try
                {
                    Transactions.Delete(trans);
                }
                catch (ApplicationException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            try
            {
                Payments.Delete(_paymentCur);
            }
            catch (ApplicationException ex)
            {//error if attached to deposit slip
                MessageBox.Show(ex.Message);
                return;
            }
            if (!IsNew)
            {
                SecurityLog.Write(Permissions.PaymentEdit, _paymentCur.PatNum, "Delete for: " + Patients.GetLim(_paymentCur.PatNum).GetNameLF() + ", "
                    + _paymentOld.PayAmt.ToString("c"), 0, _paymentOld.SecDateTEdit);
            }
            DialogResult = DialogResult.OK;
        }

        private void butPrintReceipt_Click(object sender, EventArgs e)
        {
            PrintReceipt(_paymentCur.Receipt, Lan.g(this, "Receipt printed"));
        }

        private void butEmailReceipt_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.EmailSend))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(_paymentCur.Receipt))
            {
                MsgBox.Show(this, "There is no receipt to send for this payment.");
                return;
            }
            List<string> errors = new List<string>();
            if (!EmailAddress.ExistsValidEmail())
            {
                errors.Add(Lan.g(this, "SMTP server name missing in e-mail setup."));
            }
            if (Preferences.AtoZfolderUsed == DataStorageType.InDatabase)
            {
                errors.Add(Lan.g(this, "No AtoZ folder."));
            }
            if (errors.Count > 0)
            {
                MessageBox.Show(this, Lan.g(this, "The following errors need to be resolved before creating an email") + ":\r\n" + string.Join("\r\n", errors));
                return;
            }
            string attachPath = EmailAttachment.GetAttachmentPath();
            Random rnd = new Random();
            string tempFile = Storage.Default.CombinePath(Preferences.GetTempPath(),
                DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.TimeOfDay.Ticks.ToString() + rnd.Next(1000).ToString() + ".pdf");
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true, PdfFontEmbedding.Always);
            pdfRenderer.Document = CreatePDFDoc(_paymentCur.Receipt);
            pdfRenderer.RenderDocument();
            pdfRenderer.PdfDocument.Save(tempFile);
            Storage.Local.CopyFile(tempFile, Storage.Default.CombinePath(attachPath, Path.GetFileName(tempFile)), Storage.Default);
            EmailMessage message = new EmailMessage();
            message.PatientId = _paymentCur.PatNum;
            message.ToAddress = _patCur.Email;
            EmailAddress address = EmailAddress.GetByClinic(_patCur.ClinicNum);
            message.FromAddress = address.GetFrom();
            message.Subject = Lan.g(this, "Receipt for payment received ") + _paymentCur.PayDate.ToShortDateString();
            EmailAttachment attachRcpt = new EmailAttachment()
            {
                Description = "Receipt.pdf",
                FileName = Path.GetFileName(tempFile)
            };
            message.Attachments = new List<EmailAttachment>() { attachRcpt };
            FormEmailMessageEdit FormE = new FormEmailMessageEdit(message, address);
            FormE.ShowDialog();
        }

        private void PrintReceipt(string receiptStr, string strAuditDescription)
        {//TODO: Implement ODprintout pattern - MigraDoc
            MigraDocPrintDocument printdoc = new MigraDocPrintDocument(new DocumentRenderer(CreatePDFDoc(receiptStr)));
            printdoc.Renderer.PrepareDocument();
#if DEBUG
            FormRpPrintPreview pView = new FormRpPrintPreview(printdoc);
            pView.ShowDialog();
#else
			if(PrinterL.SetPrinter(_pd2,PrintSituation.Receipt,_patCur.PatNum,strAuditDescription)) {
				printdoc.PrinterSettings=_pd2.PrinterSettings;
				try {
					printdoc.Print();
				}
				catch(Exception ex) {
					MessageBox.Show(Lan.g(this,"Unable to print receipt")+". "+ex.Message);
				}
			}
#endif
        }

        private MigraDoc.DocumentObjectModel.Document CreatePDFDoc(string receiptStr)
        {
            string[] receiptLines = receiptStr.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            MigraDoc.DocumentObjectModel.Document doc = new MigraDoc.DocumentObjectModel.Document();
            doc.DefaultPageSetup.PageWidth = Unit.FromInch(3.0);
            doc.DefaultPageSetup.PageHeight = Unit.FromInch(0.181 * receiptLines.Length + 0.56);//enough to print text plus 9/16 in. (0.56) extra space at bottom.
            doc.DefaultPageSetup.TopMargin = Unit.FromInch(0.25);
            doc.DefaultPageSetup.LeftMargin = Unit.FromInch(0.25);
            doc.DefaultPageSetup.RightMargin = Unit.FromInch(0.25);
            MigraDoc.DocumentObjectModel.Font bodyFontx = MigraDocHelper.CreateFont(8, false);
            bodyFontx.Name = FontFamily.GenericMonospace.Name;
            Section section = doc.AddSection();
            Paragraph par = section.AddParagraph();
            ParagraphFormat parformat = new ParagraphFormat();
            parformat.Alignment = ParagraphAlignment.Left;
            parformat.Font = bodyFontx;
            par.Format = parformat;
            par.AddFormattedText(receiptStr, bodyFontx);
            return doc;
        }

        private void butPrePay_Click(object sender, EventArgs e)
        {
            if (PIn.Double(textAmount.Text) == 0)
            {
                MsgBox.Show(this, "Amount cannot be zero.");
                return;
            }
            if (_listSplitsCur.Count > 0)
            {
                if (!MsgBox.Show(this, MsgBoxButtons.YesNo, "This will replace all Payment Splits with one split for the total amount.  Continue?"))
                {
                    return;
                }
            }
            _listSplitsCur.Clear();
            PaySplit split = new PaySplit();
            split.PatNum = _patCur.PatNum;
            split.PayNum = _paymentCur.PayNum;
            split.FSplitNum = 0;
            split.SplitAmt = PIn.Double(textAmount.Text);
            split.DatePay = DateTime.Now;
            split.ClinicNum = _paymentCur.ClinicNum;
            split.UnearnedType = Preference.GetLong(PreferenceName.PrepaymentUnearnedType);
            _listSplitsCur.Add(split);
            FillGridSplits();
            Application.DoEvents();
            if (!SavePaymentToDb())
            {
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private bool SavePaymentToDb()
        {
            if (textDate.errorProvider1.GetError(textDate) != "" || textAmount.errorProvider1.GetError(textAmount) != "")
            {
                MessageBox.Show(Lan.g(this, "Please fix data entry errors first."));
                return false;
            }
            if (PIn.Date(textDate.Text).Date > DateTime.Today.Date
                    && !Preference.GetBool(PreferenceName.FutureTransDatesAllowed) && !Preference.GetBool(PreferenceName.AccountAllowFutureDebits))
            {
                MsgBox.Show(this, "Payment date cannot be in the future.");
                return false;
            }
            if (checkPayTypeNone.Checked)
            {
                if (PIn.Double(textAmount.Text) != 0)
                {
                    MsgBox.Show(this, "Amount must be zero for a transfer.");
                    return false;
                }
            }
            else
            {
                double amt = PIn.Double(textAmount.Text);
                if (amt == 0 && _listSplitsCur.Count == 0)
                {
                    MessageBox.Show(Lan.g(this, "Please enter an amount or create payment splits."));
                    return false;
                }
                if (amt != 0 && listPayType.SelectedIndex == -1)
                {
                    MsgBox.Show(this, "A payment type must be selected.");
                    return false;
                }
            }
            if (Preference.GetInt(PreferenceName.RigorousAccounting) == (int)RigorousAccounting.EnforceFully)
            {
                if (_listSplitsCur.Any(x => x.ProcNum == 0 && x.UnearnedType == 0 && x.AdjNum == 0))
                {//if no procs and no adjust and not an unearned type
                    MsgBox.Show(this, "A procedure, adjustment, or unearned type must be selected for each of the payment splits.");
                    return false;
                }
            }
            double unearnedCur = _listSplitsCur.FindAll(x => x.UnearnedType > 0).Sum(x => x.SplitAmt);
            if (unearnedCur.IsLessThan(0)
                && !(unearnedCur + (double)PaySplits.GetUnearnedForFam(_famCur, payNumExcluded: _paymentCur.PayNum)).IsGreaterThanOrEqualToZero()
                && !MsgBox.Show(this, MsgBoxButtons.YesNo, "You are attempting to create a negative unearned amount. Continue?"))
            {
                return false;
            }
            if (_isCCDeclined)
            {
                textAmount.Text = 0.ToString("f");//So that a declined transaction does not affect account balance
                _listSplitsCur.ForEach(x => x.SplitAmt = 0);
                textSplitTotal.Text = 0.ToString("f");
            }
            if (IsNew)
            {
                //prevents backdating of initial payment
                if (!Security.IsAuthorized(Permissions.PaymentCreate, PIn.Date(textDate.Text)))
                {
                    return false;
                }
            }
            else
            {
                //Editing an old entry will already be blocked if the date was too old, and user will not be able to click OK button
                //This catches it if user changed the date to be older. If user has SplitCreatePastLockDate permission and has not changed the date, then
                //it is okay to save the payment.
                if ((!Security.IsAuthorized(Permissions.SplitCreatePastLockDate, true)
                    || _paymentOld.PayDate != PIn.Date(textDate.Text))
                    && !Security.IsAuthorized(Permissions.PaymentEdit, PIn.Date(textDate.Text)))
                {
                    return false;
                }
            }
            bool accountingSynchRequired = false;
            double accountingOldAmt = _paymentCur.PayAmt;
            long accountingNewAcct = -1;//the old acctNum will be retrieved inside the validation code.
            if (textDepositAccount.Visible)
            {
                accountingNewAcct = -1;//indicates no change
            }
            else if (comboDepositAccount.Visible && comboDepositAccount.Items.Count > 0 && comboDepositAccount.SelectedIndex != -1)
            {
                accountingNewAcct = _arrayDepositAcctNums[comboDepositAccount.SelectedIndex];
            }
            else
            {//neither textbox nor combo visible. Or something's wrong with combobox
                accountingNewAcct = 0;
            }
            try
            {
                accountingSynchRequired = Payments.ValidateLinkedEntries(accountingOldAmt, PIn.Double(textAmount.Text), IsNew,
                    _paymentCur.PayNum, accountingNewAcct);
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message);//not able to alter, so must not allow user to continue.
                return false;
            }
            if (_paymentCur.ProcessStatus != ProcessStat.OfficeProcessed)
            {
                if (checkProcessed.Checked)
                {
                    _paymentCur.ProcessStatus = ProcessStat.OnlineProcessed;
                }
                else
                {
                    _paymentCur.ProcessStatus = ProcessStat.OnlinePending;
                }
            }
            _paymentCur.PayAmt = PIn.Double(textAmount.Text);//handles blank
            _paymentCur.PayDate = PIn.Date(textDate.Text);
            #region Recurring charge logic
            //User chose to have a recurring payment so we need to know if the card has recurring setup and which month to apply the payment to.
            if (IsNew && checkRecurring.Checked && comboCreditCards.SelectedIndex != _listCreditCards.Count)
            {
                //Check if a recurring charge is setup for the selected card.
                if (_listCreditCards[comboCreditCards.SelectedIndex].ChargeAmt == 0
                    || _listCreditCards[comboCreditCards.SelectedIndex].DateStart.Year < 1880)
                {
                    MsgBox.Show(this, "The selected credit card has not been setup for recurring charges.");
                    return false;
                }
                //Check if a stop date was set and if that date falls in on today or in the past.
                if (_listCreditCards[comboCreditCards.SelectedIndex].DateStop.Year > 1880
                    && _listCreditCards[comboCreditCards.SelectedIndex].DateStop <= DateTime.Now)
                {
                    MsgBox.Show(this, "This card is no longer accepting recurring charges based on the stop date.");
                    return false;
                }
                //Have the user decide what month to apply the recurring charge towards.
                FormCreditRecurringDateChoose formDateChoose = new FormCreditRecurringDateChoose(_listCreditCards[comboCreditCards.SelectedIndex], _patCur);
                formDateChoose.ShowDialog();
                if (formDateChoose.DialogResult != DialogResult.OK)
                {
                    MsgBox.Show(this, "Uncheck the \"Apply to Recurring Charge\" box.");
                    return false;
                }
                //This will change the PayDate to work better with the recurring charge automation.  User was notified in previous window.
                if (!Preference.GetBool(PreferenceName.RecurringChargesUseTransDate))
                {
                    _paymentCur.PayDate = formDateChoose.PayDate;
                }
                _paymentCur.RecurringChargeDate = formDateChoose.PayDate;
            }
            else if (IsNew && checkRecurring.Checked && comboCreditCards.SelectedIndex == _listCreditCards.Count)
            {
                MsgBox.Show(this, "Cannot apply a recurring charge to a new card.");
                return false;
            }
            #endregion
            _paymentCur.CheckNum = textCheckNum.Text;
            _paymentCur.BankBranch = textBankBranch.Text;
            _paymentCur.PayNote = textNote.Text;
            _paymentCur.IsRecurringCC = checkRecurring.Checked;
            if ((PIn.Double(textAmount.Text) == 0 && listPayType.SelectedIndex == -1) || checkPayTypeNone.Checked)
            {
                _paymentCur.PayType = 0;
            }
            else
            {
                _paymentCur.PayType = _listPaymentTypeDefs[listPayType.SelectedIndex].Id;
            }
            if (_listSplitsCur.Count == 0)
            {//Existing payment with no splits.
                if (!_isCCDeclined && Preference.GetInt(PreferenceName.RigorousAccounting) < (int)RigorousAccounting.DontEnforce)
                {
                    _listSplitsCur.AddRange(AutoSplitForPayment(_paymentCur.PayDate, _loadData));
                    _paymentCur.PayAmt = PIn.Double(textAmount.Text);//AutoSplitForPayment reduces PayAmt - Set it back to what it should be.
                }
                else if (!_isCCDeclined
                    && Payments.AllocationRequired(_paymentCur.PayAmt, _paymentCur.PatNum)
                    && _curFamOrSuperFam.Members.Length > 1 //Has other family members
                    && MsgBox.Show(this, MsgBoxButtons.YesNo, "Apply part of payment to other family members?"))
                {
                    _listSplitsCur = Payments.Allocate(_paymentCur);//PayAmt needs to be set first
                }
                else
                {//Either no allocation required, or user does not want to allocate.  Just add one split.
                    if (checkPayTypeNone.Checked)
                    {//No splits created and it's an income transfer.  Delete payment? (it's not a useful payment)
                        Payments.Delete(_paymentCur);
                        return true;
                    }
                    else
                    {
                        if (!AddOneSplit(true))
                        {
                            return false;
                        }
                    }
                }
                if (_listSplitsCur.Count == 0)
                {//There's still no split.
                    if (!AddOneSplit(true))
                    {
                        return false;
                    }
                }
            }
            else
            {//A new or existing payment with splits.
                if (_listSplitsCur.Count == 1//if one split
                    && _listSplitsCur[0].PayPlanNum != 0//and split is on a payment plan
                    && PIn.Double(textAmount.Text) != _listSplitsCur[0].SplitAmt)//and amount doesn't match payment
                {
                    _listSplitsCur[0].SplitAmt = PIn.Double(textAmount.Text);//make amounts match automatically
                    textSplitTotal.Text = textAmount.Text;
                }
                if (_paymentCur.PayAmt != PIn.Double(textSplitTotal.Text))
                {
                    MsgBox.Show(this, "Split totals must equal payment amount.");
                    //work on reallocation schemes here later
                    return false;
                }
            }
            if (_listSplitsCur.Count > 1)
            {
                _paymentCur.IsSplit = true;
            }
            else
            {
                _paymentCur.IsSplit = false;
            }
            try
            {
                Payments.Update(_paymentCur, true);
            }
            catch (ApplicationException ex)
            {//this catches bad dates.
                MessageBox.Show(ex.Message);
                return false;
            }
            //Set all DatePays the same.
            for (int i = 0; i < _listSplitsCur.Count; i++)
            {
                _listSplitsCur[i].DatePay = _paymentCur.PayDate;
            }
            bool hasChanged = PaySplits.Sync(_listSplitsCur, _listPaySplitsOld);
            foreach (PaySplits.PaySplitAssociated split in _listPaySplitsAssociated)
            {
                //Update the FSplitNum after inserts are made. 
                if (split.PaySplitLinked != null && split.PaySplitOrig != null)
                {
                    PaySplits.UpdateFSplitNum(split.PaySplitOrig.SplitNum, split.PaySplitLinked.SplitNum);
                }
            }
            //Accounting synch is done here.  All validation was done further up
            //If user is trying to change the amount or linked account of an entry that was already copied and linked to accounting section
            if (accountingSynchRequired && !checkPayTypeNone.Checked)
            {
                Payments.AlterLinkedEntries(accountingOldAmt, _paymentCur.PayAmt, IsNew, _paymentCur.PayNum, accountingNewAcct, _paymentCur.PayDate,
                    _curFamOrSuperFam.GetNameInFamFL(_paymentCur.PatNum));
            }
            if (IsNew)
            {
                SecurityLog.Write(Permissions.PaymentCreate, _paymentCur.PatNum, Patients.GetLim(_paymentCur.PatNum).GetNameLF() + ", "
                    + _paymentCur.PayAmt.ToString("c"));
            }
            else
            {
                SecurityLog.Write(Permissions.PaymentEdit, _paymentCur.PatNum, Patients.GetLim(_paymentCur.PatNum).GetNameLF() + ", "
                    + _paymentCur.PayAmt.ToString("c"), 0, _paymentOld.SecDateTEdit);
            }
            if (hasChanged)
            {
                string strErrorMsg = Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(_patCur.PatNum, _listSplitsCur.Union(_listPaySplitsOld).ToList());
                if (!string.IsNullOrEmpty(strErrorMsg))
                {
                    MessageBox.Show(strErrorMsg);
                }
            }
            return true;
        }

        private void butShowHide_Click(object sender, EventArgs e)
        {
            ToggleShowHideSplits();
        }

        private void butOK_Click(object sender, System.EventArgs e)
        {
            if (!SavePaymentToDb())
            {
                return;
            }
            Plugin.Trigger(this, "FormPayment_OK", _paymentCur, _listSplitsCur);
            DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void FormPayment_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                return;
            }
            if (!IsNew && !_wasCreditCardSuccessful)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            if (!_wasCreditCardSuccessful)
            {//new payment that was not a credit card payment that has already been processed
                Payments.Delete(_paymentCur);
                DialogResult = DialogResult.Cancel;
                return;
            }
            //Successful CC payment
            if (!MsgBox.Show(this, MsgBoxButtons.YesNo, "This will void the transaction that has just been completed. Are you sure you want to continue?"))
            {
                e.Cancel = true;//Stop the form from closing
                return;
            }
            DateTime payDateCur = PIn.Date(textDate.Text);
            if (payDateCur == null || payDateCur == DateTime.MinValue)
            {
                MsgBox.Show(this, "Invalid Payment Date");
                e.Cancel = true;//Stop the form from closing
                return;
            }
            if (payDateCur.Date > DateTime.Today.Date && !Preference.GetBool(PreferenceName.AccountAllowFutureDebits) && !Preference.GetBool(PreferenceName.FutureTransDatesAllowed))
            {
                MsgBox.Show(this, "Payment Date must not be a future date.");
                e.Cancel = true;//Stop the form from closing
                return;
            }
            //Save the credit card transaction as a new payment
            _paymentCur.PayAmt = PIn.Double(textAmount.Text);//handles blank
            _paymentCur.PayDate = payDateCur;
            _paymentCur.CheckNum = textCheckNum.Text;
            _paymentCur.BankBranch = textBankBranch.Text;
            _paymentCur.IsRecurringCC = false;
            _paymentCur.PayNote = textNote.Text;
            if (checkPayTypeNone.Checked)
            {
                _paymentCur.PayType = 0;
            }
            else
            {
                _paymentCur.PayType = _listPaymentTypeDefs[listPayType.SelectedIndex].Id;
            }
            if (_listSplitsCur.Count == 0)
            {
                AddOneSplit();
                //FillMain();
            }
            else if (_listSplitsCur.Count == 1//if one split
                && _listSplitsCur[0].PayPlanNum != 0//and split is on a payment plan
                && _listSplitsCur[0].SplitAmt != _paymentCur.PayAmt)//and amount doesn't match payment
            {
                _listSplitsCur[0].SplitAmt = _paymentCur.PayAmt;//make amounts match automatically
                textSplitTotal.Text = textAmount.Text;
            }
            if (_paymentCur.PayAmt != PIn.Double(textSplitTotal.Text))
            {
                MsgBox.Show(this, "Split totals must equal payment amount.");
                DialogResult = DialogResult.None;
                return;
            }
            if (_listSplitsCur.Count > 1)
            {
                _paymentCur.IsSplit = true;
            }
            else
            {
                _paymentCur.IsSplit = false;
            }
            try
            {
                Payments.Update(_paymentCur, true);
            }
            catch (ApplicationException ex)
            {//this catches bad dates.
                MessageBox.Show(ex.Message);
                e.Cancel = true;
                return;
            }
            //Set all DatePays the same.
            for (int i = 0; i < _listSplitsCur.Count; i++)
            {
                _listSplitsCur[i].DatePay = _paymentCur.PayDate;
            }
            bool hasChanged = PaySplits.Sync(_listSplitsCur, _listPaySplitsOld);
            if (IsNew)
            {
                SecurityLog.Write(Permissions.PaymentCreate, _paymentCur.PatNum, Patients.GetLim(_paymentCur.PatNum).GetNameLF() + ", " +
                    _paymentCur.PayAmt.ToString("c"));
            }
            else
            {
                SecurityLog.Write(Permissions.PaymentEdit, _paymentCur.PatNum, Patients.GetLim(_paymentCur.PatNum).GetNameLF() + ", " +
                    _paymentCur.PayAmt.ToString("c"), 0, _paymentOld.SecDateTEdit);
            }
            if (hasChanged)
            {
                string strErrorMsg = Ledgers.ComputeAgingForPaysplitsAllocatedToDiffPats(_patCur.PatNum, _listSplitsCur.Union(_listPaySplitsOld).ToList());
                if (!string.IsNullOrEmpty(strErrorMsg))
                {
                    MessageBox.Show(strErrorMsg);
                }
            }
            string refNum = "";
            string amount = "";
            string transactionID = "";
            string paySimplePaymentId = "";
            bool isDebit = false;
            string[] arrayNoteFields = textNote.Text.Replace("\r\n", "\n").Replace("\r", "\n").Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arrayNoteFields.Length; i++)
            {
                if (arrayNoteFields[i].StartsWith("Amount: "))
                {
                    amount = arrayNoteFields[i].Substring(8);
                }
                if (arrayNoteFields[i].StartsWith("Ref Number: "))
                {
                    refNum = arrayNoteFields[i].Substring(12);
                }
                if (arrayNoteFields[i].StartsWith("XCTRANSACTIONID="))
                {
                    transactionID = arrayNoteFields[i].Substring(16);
                }
                if (arrayNoteFields[i].StartsWith("APPROVEDAMOUNT="))
                {
                    amount = arrayNoteFields[i].Substring(15);
                }
                if (arrayNoteFields[i].StartsWith("TYPE=") && arrayNoteFields[i].Substring(5) == "Debit Purchase")
                {
                    isDebit = true;
                }
                if (arrayNoteFields[i].StartsWith(Lan.g("PaySimple", "PaySimple Transaction Number")))
                {
                    paySimplePaymentId = arrayNoteFields[i].Split(':')[1].Trim();//Better than substring 28, because we do not know how long the translation will be.
                }
            }
            if (refNum != "")
            {//Void the PayConnect transaction if there is one
                VoidPayConnectTransaction(refNum, amount);
            }
            else if (transactionID != "" && HasXCharge())
            {//Void the X-Charge transaction if there is one
                VoidXChargeTransaction(transactionID, amount, isDebit);
            }
            else if (!string.IsNullOrWhiteSpace(paySimplePaymentId))
            {
                //string originalReceipt=_paymentCur.Receipt;
                //if(_paySimpleResponse!=null) {
                //	originalReceipt=_paySimpleResponse.TransactionReceipt;
                //}
                //VoidPaySimpleTransaction(paySimplePaymentId,originalReceipt);
            }
            else
            {
                MsgBox.Show(this, "Unable to void transaction");
            }
            DialogResult = DialogResult.Cancel;
        }
    }
}
