using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAutoCodeLessIntrusive : FormBase
    {
        public string mainText;

        private Patient _patCur;
        private ProcedureCode _procCodeCur;
        private long _verifyCode;
        private readonly List<PatPlan> patPlans;
        private readonly List<InsSub> _listInsSubs;
        private readonly List<InsPlan> _listInsPlans;
        private readonly List<Benefit> _listBenefits;
        private readonly List<ClaimProc> _listClaimProcs;
        private readonly string _teethText;

        public Procedure Proc { get; }

        public FormAutoCodeLessIntrusive(Patient pat, Procedure proc, ProcedureCode procCode, long verifyCode, List<PatPlan> listPatPlans, List<InsSub> listInsSubs, List<InsPlan> listInsPlans, List<Benefit> listBenefits, List<ClaimProc> listClaimProcs, string teethText = null)
        {
            _patCur = pat;
            Proc = proc;
            _procCodeCur = procCode;
            _verifyCode = verifyCode;
            patPlans = listPatPlans;
            _listInsSubs = listInsSubs;
            _listInsPlans = listInsPlans;
            _listBenefits = listBenefits;
            _listClaimProcs = listClaimProcs;
            _teethText = teethText;

            InitializeComponent();
        }

        private void FormAutoCodeLessIntrusive_Load(object sender, EventArgs e)
        {
            mainLabel.Text = 
                ProcedureCodes.GetProcCode(_verifyCode).ProcCode
                + " (" + ProcedureCodes.GetProcCode(_verifyCode).Descript + ") "
                + "is the recommended procedure code for this procedure. Change procedure code and fee?";

            if (Preference.GetBool(PreferenceName.ProcEditRequireAutoCodes))
            {
                cancelButton.Text = "Edit Proc";
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            //Customers have been complaining about procedurelog entries changing their CodeNum column to 0.
            //Based on a security log provided by a customer, we were able to determine that this is one of two potential violators.
            //The following code is here simply to try and get the user to call us so that we can have proof and hopefully find the core of the issue.
            try
            {
                if (_verifyCode < 1)
                {
                    throw new ApplicationException("Invalid Verify Code");
                }
            }
            catch (ApplicationException ae)
            {
                string error = "Please notify support with the following information.\r\n"
                    + "Error: " + ae.Message + "\r\n"
                    + "_verifyCode: " + _verifyCode.ToString() + "\r\n"
                    + "_procCur.CodeNum: " + (Proc == null ? "NULL" : Proc.CodeNum.ToString()) + "\r\n"
                    + "_procCodeCur.CodeNum: " + (_procCodeCur == null ? "NULL" : _procCodeCur.CodeNum.ToString()) + "\r\n"
                    + "\r\n"
                    + "StackTrace:\r\n" + ae.StackTrace;
                MsgBoxCopyPaste MsgBCP = new MsgBoxCopyPaste(error);
                MsgBCP.Text = "Fatal Error!!!";
                MsgBCP.Show();//Use .Show() to make it easy for the user to keep this window open while they call in.
                return;
            }
            //Moved from FormProcEdit.SaveAndClose() in version 16.3+
            Procedure procOld = Proc.Copy();
            Proc.CodeNum = _verifyCode;
            if (new[] { ProcStat.TP, ProcStat.C, ProcStat.TPi, ProcStat.Cn }.Contains(Proc.ProcStatus))
            {//Only change the fee if Complete, TP, TPi, or Cn.
                InsSub prisub = null;
                InsPlan priplan = null;
                if (patPlans.Count > 0)
                {
                    prisub = InsSubs.GetSub(patPlans[0].InsSubNum, _listInsSubs);
                    priplan = InsPlans.GetPlan(prisub.PlanNum, _listInsPlans);
                }
                Proc.ProcFee = Fees.GetAmount0(Proc.CodeNum, FeeScheds.GetFeeSched(_patCur, _listInsPlans, patPlans, _listInsSubs, Proc.ProvNum),
                    Proc.ClinicNum, Proc.ProvNum);
                if (priplan != null && priplan.PlanType == "p")
                {//PPO
                    double standardfee = Fees.GetAmount0(Proc.CodeNum, Provider.GetById(Patients.GetProvNum(_patCur)).FeeScheduleId, Proc.ClinicNum,
                        Proc.ProvNum);
                    Proc.ProcFee = Math.Max(Proc.ProcFee, standardfee);
                }
            }
            Procedures.Update(Proc, procOld);

            //Compute estimates required, otherwise if adding through quick add, it could have incorrect WO or InsEst if code changed.
            Procedures.ComputeEstimates(Proc, _patCur.PatNum, _listClaimProcs, true, _listInsPlans, patPlans, _listBenefits, _patCur.Age, _listInsSubs);
            Recalls.Synch(Proc.PatNum);
            if (Proc.ProcStatus.In(ProcStat.C, ProcStat.EO, ProcStat.EC))
            {
                string logText = _procCodeCur.ProcCode + " (" + Proc.ProcStatus + "), ";
                if (_teethText != null && _teethText.Trim() != "")
                {
                    logText += "Teeth: " + _teethText + ", ";
                }
                logText += "Fee: " + Proc.ProcFee.ToString("F") + ", " + _procCodeCur.Descript;
                string perm = Permissions.EditCompletedProcedure;
                if (Proc.ProcStatus.In(ProcStat.EO, ProcStat.EC))
                {
                    perm = Permissions.EditProcedure;
                }

                SecurityLog.Write(perm, _patCur.PatNum, logText);
            }
            DialogResult = DialogResult.OK;
        }
    }
}