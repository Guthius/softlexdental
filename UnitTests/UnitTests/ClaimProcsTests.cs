using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests {
	[TestClass]
	public class ClaimProcsTests:TestBase {

		const double _ucrFee=50;
		const double _fee=25;
		const int _coveragePercent=50;
		const double _patPortionFromFee=_fee*((double)_coveragePercent/100.0);
		const double _blankFee=-1;

		#region FixedBenefits

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_FixedBenefitBlankLikeZeroOffFalse_PPO60_FixedBenefitFeeBlank() {
			PrefT.UpdateBool(PrefName.FixedBenefitBlankLikeZero,false);
			double procFee=100;
			double ppoFee=60;
			double fixedBenefitFee=-1;//blank
			ComputeBaseEstFixedBenefits(MethodBase.GetCurrentMethod().Name,procFee,ppoFee,fixedBenefitFee,-1,-1,-1,0,0,0
				,((assertItem) =>
				{
					Assert.AreEqual(60,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(-1,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(40,assertItem.PrimaryClaimProc.WriteOffEst);
				}));
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_FixedBenefitBlankLikeZeroOffTrue_PPO60_FixedBenefitFeeBlank() {
			PrefT.UpdateBool(PrefName.FixedBenefitBlankLikeZero,true);
			double procFee=100;
			double ppoFee=60;
			double fixedBenefitFee=-1;//blank
			ComputeBaseEstFixedBenefits(MethodBase.GetCurrentMethod().Name,procFee,ppoFee,fixedBenefitFee,-1,-1,-1,0,0,0
				,((assertItem) =>
				{
					Assert.AreEqual(0,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(60,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(40,assertItem.PrimaryClaimProc.WriteOffEst);
				}));
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_FixedBenefitBlankLikeZeroOffFalse_PPO60_FixedBenefitFeeZero() {
			PrefT.UpdateBool(PrefName.FixedBenefitBlankLikeZero,false);
			double procFee=100;
			double ppoFee=60;
			double fixedBenefitFee=0;
			ComputeBaseEstFixedBenefits(MethodBase.GetCurrentMethod().Name,procFee,ppoFee,fixedBenefitFee,-1,-1,-1,0,0,0
				,((assertItem) =>
				{
					Assert.AreEqual(0,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(60,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(40,assertItem.PrimaryClaimProc.WriteOffEst);
				}));
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_FixedBenefitBlankLikeZeroOffTrue_PPO60_FixedBenefitFeeZero() {
			PrefT.UpdateBool(PrefName.FixedBenefitBlankLikeZero,true);
			double procFee=100;
			double ppoFee=60;
			double fixedBenefitFee=0;
			ComputeBaseEstFixedBenefits(MethodBase.GetCurrentMethod().Name,procFee,ppoFee,fixedBenefitFee,-1,-1,-1,0,0,0
				,((assertItem) =>
				{
					Assert.AreEqual(0,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(60,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(40,assertItem.PrimaryClaimProc.WriteOffEst);
				}));
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_FixedBenefitBlankLikeZeroOffFalse_PPOBlank_FixedBenefitFeeBlank() {
			PrefT.UpdateBool(PrefName.FixedBenefitBlankLikeZero,false);
			double procFee=100;
			double ppoFee=-1;//blank
			double fixedBenefitFee=-1;//blank
			ComputeBaseEstFixedBenefits(MethodBase.GetCurrentMethod().Name,procFee,ppoFee,fixedBenefitFee,-1,-1,-1,0,0,0
				,((assertItem) =>
				{
					Assert.AreEqual(100,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(-1,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(0,assertItem.PrimaryClaimProc.WriteOffEst);
				}));
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_FixedBenefitBlankLikeZeroOffTrue_PPOBlank_FixedBenefitFeeBlank() {
			PrefT.UpdateBool(PrefName.FixedBenefitBlankLikeZero,true);
			double procFee=100;
			double ppoFee=-1;//blank
			double fixedBenefitFee=-1;//blank
			ComputeBaseEstFixedBenefits(MethodBase.GetCurrentMethod().Name,procFee,ppoFee,fixedBenefitFee,-1,-1,-1,0,0,0
				,((assertItem) =>
				{
					Assert.AreEqual(0,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(100,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(0,assertItem.PrimaryClaimProc.WriteOffEst);
				}));
		}

		///<summary>Creates a procedure and computes estimates for a patient with a fixed benefit PPO plan.</summary>
		private void ComputeBaseEstFixedBenefits(string suffix,double procFee,double ppoFee,double fixedBenefitFee,double copayOverride,double allowedOverride,int percentOverride
			,double paidOtherInsTot,double paidOtherInsBase,double writeOffOtherIns,Action<BenefitsAssertItem> assertAct)
		{
			Patient pat=PatientT.CreatePatient(suffix);
			string procStr="D0150";
			Procedure proc=ProcedureT.CreateProcedure(pat,procStr,ProcStat.TP,"",procFee);
			ProcedureCode procCode=ProcedureCodes.GetProcCode(procStr);
			long ppoFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO "+suffix);
			long catPercFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"Category % "+suffix);
			long fixedBenefitFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.FixedBenefit,"Fixed Benefit "+suffix);
			if(ppoFee>-1) {
				FeeT.CreateFee(ppoFeeSchedNum,procCode.CodeNum,ppoFee);
			}
			if(fixedBenefitFee>-1) {
				FeeT.CreateFee(fixedBenefitFeeSchedNum,procCode.CodeNum,fixedBenefitFee);
			}
			InsuranceT.AddInsurance(pat,suffix,"p",ppoFeeSchedNum,copayFeeSchedNum: fixedBenefitFeeSchedNum);
			List<InsSub> listSubs=InsSubT.GetInsSubs(pat);
			List<InsPlan> listPlans=InsPlans.RefreshForSubList(listSubs);
			List<PatPlan> listPatPlans=PatPlans.Refresh(pat.PatNum);
			InsPlan priPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Primary,listPatPlans,listPlans,listSubs);
			InsSub priSub=InsSubT.GetSubForPriSecMed(PriSecMed.Primary,listPatPlans,listPlans,listSubs);
			PatPlan priPatPlan=listPatPlans.FirstOrDefault(x => x.InsSubNum==priSub.InsSubNum);
			ClaimProc priClaimProc=ClaimProcT.CreateClaimProc(pat.PatNum,proc.ProcNum,priPlan.PlanNum,priSub.InsSubNum,DateTime.Today,copayOverride,allowedOverride,percentOverride);
			List<Benefit> listBens=Benefits.Refresh(listPatPlans,listSubs);
			List<SubstitutionLink> listSubLinks=SubstitutionLinks.GetAllForPlans(listPlans);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			ClaimProcs.ComputeBaseEst(priClaimProc,proc,priPlan,priPatPlan.PatPlanNum,listBens,histList,loopList,listPatPlans,paidOtherInsTot
				,paidOtherInsBase,pat.Age,writeOffOtherIns,listPlans,listSubs,listSubLinks,false,null);
			assertAct(new BenefitsAssertItem() {
				Procedure=proc,
				PrimaryClaimProc=priClaimProc,
			});
		}

		#endregion

		#region PPO
		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_NoExclusion_BlankFee_PPO() {
			AssertExclusions("p",feeSchedFee: _blankFee,hasExclusion: false,eProcFee: _ucrFee,eWriteOff: 0,ePatPortion: _ucrFee*((double)_coveragePercent/100.0));
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_NoExclusion_ZeroFee_PPO() {
			AssertExclusions("p",feeSchedFee: 0,hasExclusion: false,eProcFee: _ucrFee,eWriteOff: _ucrFee,ePatPortion: 0);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_NoExclusion_NormalFee_PPO() {
			AssertExclusions("p",feeSchedFee: _fee,hasExclusion: false,eProcFee: _ucrFee,eWriteOff: _ucrFee-_fee,ePatPortion: _patPortionFromFee);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_BlankFee_PPO() {
			AssertExclusions("p",feeSchedFee: _blankFee,hasExclusion: true,eProcFee: _ucrFee,eWriteOff: 0,ePatPortion: _ucrFee);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_ZeroFee_PPO() {
			AssertExclusions("p",feeSchedFee: 0,hasExclusion: true,eProcFee: _ucrFee,eWriteOff: _ucrFee,ePatPortion: 0);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_NormalFee_PPO() {
			AssertExclusions("p",feeSchedFee: _fee,hasExclusion: true,eProcFee: _ucrFee,eWriteOff: _ucrFee-_fee,ePatPortion: _ucrFee-(_ucrFee-_fee));
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_BlankFee_PPO_ExcludedUseUCR() {
			PrefT.UpdateBool(PrefName.InsPlanUseUcrFeeForExclusions,true);
			AssertExclusions("p",feeSchedFee: _blankFee,hasExclusion: true,eProcFee: _ucrFee,eWriteOff: 0,ePatPortion:_ucrFee);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_ZeroFee_PPO_ExcludedUseUCR() {
			PrefT.UpdateBool(PrefName.InsPlanUseUcrFeeForExclusions,true);
			AssertExclusions("p",feeSchedFee: 0,hasExclusion: true,eProcFee: _ucrFee,eWriteOff: 0,ePatPortion:_ucrFee);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_NormalFee_PPO_ExcludedUseUCR() {
			PrefT.UpdateBool(PrefName.InsPlanUseUcrFeeForExclusions,true);
			AssertExclusions("p",feeSchedFee: _fee,hasExclusion: true,eProcFee: _ucrFee,eWriteOff: 0,ePatPortion:_ucrFee);
		}
		#endregion

		#region Medicaid
		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_NoExclusion_BlankFee_Medicaid() {
			AssertExclusions("f",feeSchedFee: _blankFee,hasExclusion: false,eProcFee: 0,eWriteOff: -1,ePatPortion: 0);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_NoExclusion_ZeroFee_Medicaid() {
			AssertExclusions("f",feeSchedFee: 0,hasExclusion: false,eProcFee: 0,eWriteOff: -1,ePatPortion: 0);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_NoExclusion_NormalFee_Medicaid() {
			AssertExclusions("f",feeSchedFee: _fee,hasExclusion: false,eProcFee: _fee,eWriteOff: -1,ePatPortion: 0);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_BlankFee_Medicaid() {
			AssertExclusions("f",feeSchedFee: _blankFee,hasExclusion: true,eProcFee: 0,eWriteOff: -1,ePatPortion: 0);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_ZeroFee_Medicaid() {
			AssertExclusions("f",feeSchedFee: 0,hasExclusion: true,eProcFee: 0,eWriteOff: -1,ePatPortion: 0);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_NormalFee_Medicaid() {
			AssertExclusions("f",feeSchedFee: _fee,hasExclusion: true,eProcFee: _fee,eWriteOff: -1,ePatPortion: _fee);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_BlankFee_Medicaid_ExcludedUseUCR() {
			PrefT.UpdateBool(PrefName.InsPlanUseUcrFeeForExclusions,true);
			AssertExclusions("f",feeSchedFee: _blankFee,hasExclusion: true,eProcFee: _ucrFee,eWriteOff: -1,ePatPortion:_ucrFee);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_ZeroFee_Medicaid_ExcludedUseUCR() {
			PrefT.UpdateBool(PrefName.InsPlanUseUcrFeeForExclusions,true);
			AssertExclusions("f",feeSchedFee: 0,hasExclusion: true,eProcFee: _ucrFee,eWriteOff: -1,ePatPortion:_ucrFee);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_NormalFee_Medicaid_ExcludedUseUCR() {
			PrefT.UpdateBool(PrefName.InsPlanUseUcrFeeForExclusions,true);
			AssertExclusions("f",feeSchedFee: _fee,hasExclusion: true,eProcFee: _ucrFee,eWriteOff: -1,ePatPortion:_ucrFee);
		}
		#endregion

		#region Capitation
		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_NoExclusion_BlankFee_Capitation() {
			AssertExclusions("c",feeSchedFee: _blankFee,hasExclusion: false,eProcFee: 0,eWriteOff: 0,ePatPortion: 0);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_NoExclusion_ZeroFee_Capitation() {
			AssertExclusions("c",feeSchedFee: 0,hasExclusion: false,eProcFee: 0,eWriteOff: 0,ePatPortion: 0);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_NoExclusion_NormalFee_Capitation() {
			AssertExclusions("c",feeSchedFee: _fee,hasExclusion: false,eProcFee: _fee,eWriteOff: _fee,ePatPortion: 0);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_BlankFee_Capitation() {
			AssertExclusions("c",feeSchedFee: _blankFee,hasExclusion: true,eProcFee: 0,eWriteOff: 0,ePatPortion: 0);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_ZeroFee_Capitation() {
			AssertExclusions("c",feeSchedFee: 0,hasExclusion: true,eProcFee: 0,eWriteOff: 0,ePatPortion: 0);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_NormalFee_Capitation() {
			AssertExclusions("c",feeSchedFee: _fee,hasExclusion: true,eProcFee: _fee,eWriteOff: _fee,ePatPortion: 0);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_BlankFee_Capitation_ExcludedUseUCR() {
			PrefT.UpdateBool(PrefName.InsPlanUseUcrFeeForExclusions,true);
			AssertExclusions("c",feeSchedFee: _blankFee,hasExclusion: true,eProcFee: _ucrFee,eWriteOff: 0,ePatPortion:_ucrFee);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_ZeroFee_Capitation_ExcludedUseUCR() {
			PrefT.UpdateBool(PrefName.InsPlanUseUcrFeeForExclusions,true);
			AssertExclusions("c",feeSchedFee: 0,hasExclusion: true,eProcFee: _ucrFee,eWriteOff: 0,ePatPortion:_ucrFee);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_NormalFee_Capitation_ExcludedUseUCR() {
			PrefT.UpdateBool(PrefName.InsPlanUseUcrFeeForExclusions,true);
			AssertExclusions("c",feeSchedFee: _fee,hasExclusion: true,eProcFee: _ucrFee,eWriteOff: 0,ePatPortion:_ucrFee);
		}

		[TestMethod]
		///<summary>We have a DBM named ClaimProcDateNotMatchCapComplete which fixes this issue if present in historic data.</summary>
		public void ClaimProcs_ComputeEstimates_Capitation() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			InsuranceInfo insInfo=InsuranceT.AddInsurance(pat,"Medicaid","c");
			DateTime dateToday=DateTime.Today;//Store the value so it does not change when used in muiltiple places below (in case run at midnight).
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.TP,"",50,dateToday.AddMonths(-1));
			List<ClaimProc> listClaimProcs=ProcedureT.ComputeEstimates(pat,insInfo);
			Assert.AreEqual(listClaimProcs[0].Status,ClaimProcStatus.CapEstimate);//Must be CapEstimate for TP procedures.
			Assert.AreEqual(listClaimProcs[0].ProcDate,proc.ProcDate);//ProcDate must be synchronized.
			Assert.AreEqual(listClaimProcs[0].DateCP,proc.ProcDate);//DateCP (Payment Date) starts as the ProcDate for TP procs and is updated when completed.
			Procedure procOld=proc.Copy();
			proc.ProcStatus=ProcStat.C;
			proc.ProcDate=dateToday;//When we set procedures complete anywhere in the program, we also set the ProcDate to today.
			Procedures.Update(proc,procOld);
			Procedures.ComputeEstimates(proc,pat.PatNum,listClaimProcs,false,
				insInfo.ListInsPlans,insInfo.ListPatPlans,insInfo.ListBenefits,pat.Age,insInfo.ListInsSubs);
			List<ClaimProc> listCompClaimProcs=ClaimProcs.RefreshForProc(proc.ProcNum);
			Assert.AreEqual(listCompClaimProcs[0].Status,ClaimProcStatus.CapComplete);//Must be CapComplete for complete procedures.
			Assert.AreEqual(listCompClaimProcs[0].ProcDate,dateToday);//ProcDate must be set to today's date when completed.
			Assert.AreEqual(listCompClaimProcs[0].DateCP,dateToday);//DateCP (Payment Date) must be set to today's date when completed.
		}

		#endregion

		#region CatPercent
		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_NoExclusion_BlankFee_CatPercent() {
			AssertExclusions("",feeSchedFee: _blankFee,hasExclusion: false,eProcFee: 0,eWriteOff: -1,ePatPortion: 0);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_NoExclusion_ZeroFee_CatPercent() {
			AssertExclusions("",feeSchedFee: 0,hasExclusion: false,eProcFee: 0,eWriteOff: -1,ePatPortion: 0);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_NoExclusion_NormalFee_CatPercent() {
			AssertExclusions("",feeSchedFee: _fee,hasExclusion: false,eProcFee: _fee,eWriteOff: -1,ePatPortion: _patPortionFromFee);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_BlankFee_CatPercent() {
			AssertExclusions("",feeSchedFee: _blankFee,hasExclusion: true,eProcFee: 0,eWriteOff: -1,ePatPortion: 0);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_ZeroFee_CatPercent() {
			AssertExclusions("",feeSchedFee: 0,hasExclusion: true,eProcFee: 0,eWriteOff: -1,ePatPortion: 0);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_NormalFee_CatPercent() {
			AssertExclusions("",feeSchedFee: _fee,hasExclusion: true,eProcFee: _fee,eWriteOff: -1,ePatPortion: _fee);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_BlankFee_CatPercent_ExcludedUseUCR() {
			PrefT.UpdateBool(PrefName.InsPlanUseUcrFeeForExclusions,true);
			AssertExclusions("",feeSchedFee: _blankFee,hasExclusion: true,eProcFee: _ucrFee,eWriteOff: -1,ePatPortion:_ucrFee);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_ZeroFee_CatPercent_ExcludedUseUCR() {
			PrefT.UpdateBool(PrefName.InsPlanUseUcrFeeForExclusions,true);
			AssertExclusions("",feeSchedFee: 0,hasExclusion: true,eProcFee: _ucrFee,eWriteOff: -1,ePatPortion:_ucrFee);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_NormalFee_CatPercent_ExcludedUseUCR() {
			PrefT.UpdateBool(PrefName.InsPlanUseUcrFeeForExclusions,true);
			AssertExclusions("",feeSchedFee: _fee,hasExclusion: true,eProcFee: _ucrFee,eWriteOff: -1,ePatPortion:_ucrFee);
		}
		#endregion

		#region NoBillins Preference
		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_Exclusion_NoBillIns() {
			PrefT.UpdateBool(PrefName.InsPlanExclusionsMarkDoNotBillIns,true);
			AssertExclusions("",feeSchedFee: _fee,hasExclusion: true,eProcFee: _fee,eWriteOff: -1,ePatPortion: _fee,eNoBillIns: true);
		}
		#endregion

		#region InsEstRecalcReceived
		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_InsEstRecalcReceived_True() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			const int coveragePercent=_coveragePercent;
			const double ucrFee=_ucrFee;
			string procStr="D0145";
			string planType="";//percentage
			double feeSchedFee=_fee;
			double eBaseEst=feeSchedFee*(coveragePercent/100d);//Expected BaseEst
			ProcedureCode procCode=ProcedureCodes.GetProcCode(procStr);
			#region Provider Ucr Fee Setup
			long provFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"FeeSched "+suffix);
			long provNum=ProviderT.CreateProvider($"Provider {suffix}",feeSchedNum:provFeeSchedNum);
			FeeT.CreateFee(provFeeSchedNum,procCode.CodeNum,ucrFee);
			#endregion
			Patient pat=PatientT.CreatePatient(suffix,priProvNum:provNum);
			#region Fee Schedule Setup
			long feeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"FeeSched "+suffix);
			if(feeSchedFee>-1) {
				FeeT.CreateFee(feeSchedNum,procCode.CodeNum,feeSchedFee);
			}
			#endregion
			#region InsPlan Setup
			InsuranceT.AddInsurance(pat,suffix,planType,feeSchedNum);
			List<InsSub> listSubs=InsSubT.GetInsSubs(pat);
			List<InsPlan> listPlans=InsPlans.RefreshForSubList(listSubs);
			List<PatPlan> listPatPlans=PatPlans.Refresh(pat.PatNum);
			InsPlan priPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Primary,listPatPlans,listPlans,listSubs);
			InsSub priSub=InsSubT.GetSubForPriSecMed(PriSecMed.Primary,listPatPlans,listPlans,listSubs);
			if(planType!="f" && planType!="c") {//Only add a 50 percent coverage to the plan for the example proc if not Medicaid/Flat Copay or capitation
				BenefitT.CreatePercentForProc(priPlan.PlanNum,procCode.CodeNum,coveragePercent);
			}
			#endregion
			PatPlan priPatPlan=listPatPlans.FirstOrDefault(x => x.InsSubNum==priSub.InsSubNum);
			double procFee=Procedures.GetProcFee(pat,listPatPlans,listSubs,listPlans,procCode.CodeNum,pat.PriProv,0,"");
			Procedure proc=ProcedureT.CreateProcedure(pat,procStr,ProcStat.C,"",procFee);
			ClaimProc priClaimProc=ClaimProcT.CreateClaimProc(pat.PatNum,proc.ProcNum,priPlan.PlanNum,priSub.InsSubNum,DateTime.Today,-1,-1,-1,ClaimProcStatus.Received);
			List<Benefit> listBens=Benefits.Refresh(listPatPlans,listSubs);
			List<SubstitutionLink> listSubLinks=SubstitutionLinks.GetAllForPlans(listPlans);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			PrefT.UpdateBool(PrefName.InsEstRecalcReceived,true);//Set InsEstRecalcReceived preference to true.
			ClaimProcs.ComputeBaseEst(priClaimProc,proc,priPlan,priPatPlan.PatPlanNum,listBens,histList,loopList,listPatPlans,0
				,0,pat.Age,0,listPlans,listSubs,listSubLinks,false,null);
			Assert.AreEqual(eBaseEst,priClaimProc.BaseEst);
		}

		[TestMethod]
		public void ClaimProcs_ComputeBaseEst_InsEstRecalcReceived_False() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			const int coveragePercent=_coveragePercent;
			const double ucrFee=_ucrFee;
			string procStr="D0145";
			string planType="";//percentage
			double feeSchedFee=_fee;
			double eBaseEst=0;//Expected BaseEst will not be recalculated for Received ClaimProc, therefore, will stay 0.
			ProcedureCode procCode=ProcedureCodes.GetProcCode(procStr);
			#region Provider Ucr Fee Setup
			long provFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"FeeSched "+suffix);
			long provNum=ProviderT.CreateProvider($"Provider {suffix}",feeSchedNum:provFeeSchedNum);
			FeeT.CreateFee(provFeeSchedNum,procCode.CodeNum,ucrFee);
			#endregion
			Patient pat=PatientT.CreatePatient(suffix,priProvNum:provNum);
			#region Fee Schedule Setup
			long feeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"FeeSched "+suffix);
			if(feeSchedFee>-1) {
				FeeT.CreateFee(feeSchedNum,procCode.CodeNum,feeSchedFee);
			}
			#endregion
			#region InsPlan Setup
			InsuranceT.AddInsurance(pat,suffix,planType,feeSchedNum);
			List<InsSub> listSubs=InsSubT.GetInsSubs(pat);
			List<InsPlan> listPlans=InsPlans.RefreshForSubList(listSubs);
			List<PatPlan> listPatPlans=PatPlans.Refresh(pat.PatNum);
			InsPlan priPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Primary,listPatPlans,listPlans,listSubs);
			InsSub priSub=InsSubT.GetSubForPriSecMed(PriSecMed.Primary,listPatPlans,listPlans,listSubs);
			if(planType!="f" && planType!="c") {//Only add a 50 percent coverage to the plan for the example proc if not Medicaid/Flat Copay or capitation
				BenefitT.CreatePercentForProc(priPlan.PlanNum,procCode.CodeNum,coveragePercent);
			}
			#endregion
			PatPlan priPatPlan=listPatPlans.FirstOrDefault(x => x.InsSubNum==priSub.InsSubNum);
			double procFee=Procedures.GetProcFee(pat,listPatPlans,listSubs,listPlans,procCode.CodeNum,pat.PriProv,0,"");
			Procedure proc=ProcedureT.CreateProcedure(pat,procStr,ProcStat.C,"",procFee);
			ClaimProc priClaimProc=ClaimProcT.CreateClaimProc(pat.PatNum,proc.ProcNum,priPlan.PlanNum,priSub.InsSubNum,DateTime.Today,-1,-1,-1,ClaimProcStatus.Received);
			List<Benefit> listBens=Benefits.Refresh(listPatPlans,listSubs);
			List<SubstitutionLink> listSubLinks=SubstitutionLinks.GetAllForPlans(listPlans);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			PrefT.UpdateBool(PrefName.InsEstRecalcReceived,false);//Set InsEstRecalcReceived preference to false.
			ClaimProcs.ComputeBaseEst(priClaimProc,proc,priPlan,priPatPlan.PatPlanNum,listBens,histList,loopList,listPatPlans,0
				,0,pat.Age,0,listPlans,listSubs,listSubLinks,false,null);
			Assert.AreEqual(eBaseEst,priClaimProc.BaseEst);
		}
		#endregion InsEstRecalcReceived

		/// <summary>
		/// Sets up a plan based on the passed in plan type/fee schedule fee/if the proc should be an exclusion.
		/// eProcFee/eWriteOff/ePatPortion are the expected values of the assertions
		/// </summary>
		private void AssertExclusions(string planType,double feeSchedFee,bool hasExclusion,double eProcFee,double eWriteOff,double ePatPortion,bool eNoBillIns=false) {
			string suffix=MethodBase.GetCurrentMethod().Name;
			const int coveragePercent=50;
			const int ucrFee=50;
			string procStr="D0145";
			ProcedureCode procCode=ProcedureCodes.GetProcCode(procStr);
			#region Provider Ucr Fee Setup
			long provFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"FeeSched "+suffix);
			long provNum=ProviderT.CreateProvider($"Provider Exclusion {suffix}",feeSchedNum:provFeeSchedNum);
			FeeT.CreateFee(provFeeSchedNum,procCode.CodeNum,ucrFee);
			#endregion
			Patient pat=PatientT.CreatePatient(suffix,priProvNum:provNum);
			#region Fee Schedule Setup
			long feeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"FeeSched "+suffix);
			if(feeSchedFee>-1) {
				FeeT.CreateFee(feeSchedNum,procCode.CodeNum,feeSchedFee);
			}
			#endregion
			#region InsPlan Setup
			InsuranceT.AddInsurance(pat,suffix,planType,feeSchedNum);
			List<InsSub> listSubs=InsSubT.GetInsSubs(pat);
			List<InsPlan> listPlans=InsPlans.RefreshForSubList(listSubs);
			List<PatPlan> listPatPlans=PatPlans.Refresh(pat.PatNum);
			InsPlan priPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Primary,listPatPlans,listPlans,listSubs);
			InsSub priSub=InsSubT.GetSubForPriSecMed(PriSecMed.Primary,listPatPlans,listPlans,listSubs);
			if(planType!="f" && planType!="c") {//Only add a 50 percent coverage to the plan for the example proc if not Medicaid/Flat Copay or capitation
				BenefitT.CreatePercentForProc(priPlan.PlanNum,procCode.CodeNum,coveragePercent);
			}
			if(hasExclusion) {
				BenefitT.CreateExclusion(priPlan.PlanNum,procCode.CodeNum);
			}
			#endregion
			PatPlan priPatPlan=listPatPlans.FirstOrDefault(x => x.InsSubNum==priSub.InsSubNum);
			double procFee=Procedures.GetProcFee(pat,listPatPlans,listSubs,listPlans,procCode.CodeNum,pat.PriProv,0,"");
			Procedure proc=ProcedureT.CreateProcedure(pat,procStr,ProcStat.TP,"",procFee);
			ClaimProcStatus cps=ClaimProcStatus.NotReceived;
			if(planType=="c") {
				cps=ClaimProcStatus.CapEstimate;
			}
			ClaimProc priClaimProc=ClaimProcT.CreateClaimProc(pat.PatNum,proc.ProcNum,priPlan.PlanNum,priSub.InsSubNum,DateTime.Today,-1,-1,-1,cps);
			List<Benefit> listBens=Benefits.Refresh(listPatPlans,listSubs);
			List<SubstitutionLink> listSubLinks=SubstitutionLinks.GetAllForPlans(listPlans);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			ClaimProcs.ComputeBaseEst(priClaimProc,proc,priPlan,priPatPlan.PatPlanNum,listBens,histList,loopList,listPatPlans,0
				,0,pat.Age,0,listPlans,listSubs,listSubLinks,false,null);
			#region Assert
			BenefitsAssertItem assertItem=new BenefitsAssertItem();
			assertItem.PrimaryClaimProc=priClaimProc;
			assertItem.Procedure=proc;
			Assert.AreEqual(eProcFee,assertItem.Procedure.ProcFee,"ProcFee calculation");
			Assert.AreEqual(eWriteOff,assertItem.PrimaryClaimProc.WriteOffEst,"WriteOffEst calculation");
			double patPort=(double)ClaimProcs.GetPatPortion(assertItem.Procedure,new List<ClaimProc>() { assertItem.PrimaryClaimProc });
			Assert.AreEqual(ePatPortion,patPort,"PatPortion calculation");
			Assert.AreEqual(eNoBillIns,assertItem.PrimaryClaimProc.NoBillIns,"NoBillIns");
			#endregion
		}

		private class BenefitsAssertItem {
			public Procedure Procedure;
			public ClaimProc PrimaryClaimProc;

			public BenefitsAssertItem() { }

		}
	}
}
