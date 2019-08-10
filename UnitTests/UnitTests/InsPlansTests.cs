using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;
using System.Globalization;
using System.Threading;

namespace UnitTests {
	[TestClass]
	public class InsPlansTests:TestBase {

		private static List<ProcedureCode> _listProcCodes;
		private static List<ProcedureCode> _listProcCodesOrig;

		[ClassInitialize]
		public static void SetUp(TestContext context) {
			_listProcCodes=ProcedureCodes.GetAllCodes();
			_listProcCodesOrig=_listProcCodes.Select(x => x.Copy()).ToList();
		}

		[TestCleanup]
		public void TearDownTest() {
			//Setting substitution codes can mess up fees for other tests.
			_listProcCodesOrig.ForEach(x => ProcedureCodes.Update(x));
		}

		///<summary></summary>
		[TestMethod]
		public void InsPlans_ComputeEstimatesForSubscriber_CanadianLabFees() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			CultureInfo curCulture=CultureInfo.CurrentCulture;
			Thread.CurrentThread.CurrentCulture=new CultureInfo("en-CA");//Canada
			try {
				//Create a patient and treatment plan a procedure with a lab fee.
				Patient pat=PatientT.CreatePatient();
				ProcedureCodeT.AddIfNotPresent("14611");
				ProcedureCodeT.AddIfNotPresent("99111",isCanadianLab:true);
				Procedure proc=ProcedureT.CreateProcedure(pat,"14611",ProcStat.TP,"",250);
				Procedure procLab=ProcedureT.CreateProcedure(pat,"99111",ProcStat.TP,"",149,procNumLab:proc.ProcNum);
				//Create a new primary insurance plan for this patient.
				//It is important that we add the insurance plan after the procedure has already been created for this particular scenario.
				Carrier carrier=CarrierT.CreateCarrier(suffix);
				InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
				InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
				PatPlan patPlan=PatPlanT.CreatePatPlan(1,pat.PatNum,sub.InsSubNum);
				//Invoking ComputeEstimatesForAll() will simulate the logic of adding a new insurance plan from the Family module.
				//The bug that this unit test is preventing is that a duplicate claimproc was being created for the lab fee.
				//This was causing a faux line to show up when a claim was created for the procedure in question.
				//It ironically doesn't matter if the procedures above are even covered by insurance because they'll get claimprocs created regardless.
				InsPlans.ComputeEstimatesForSubscriber(sub.Subscriber);
				//Check to see how many claimproc enteries there are for the current patient.  There should only be two.
				List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
				Assert.AreEqual(2,listClaimProcs.Count);
			}
			finally {
				Thread.CurrentThread.CurrentCulture=curCulture;
			}
		}

		/// <summary>Get the copay value for when there is no patient copay</summary>
		[TestMethod]
		public void InsPlans_GetCopay_Blank() {
			ProcedureCode procCode=_listProcCodes[0];
			InsPlan plan=GenerateMediFlatInsPlan(MethodBase.GetCurrentMethod().Name);
			double amt=InsPlans.GetCopay(procCode.CodeNum,plan.FeeSched,plan.CopayFeeSched,plan.CodeSubstNone,"",0,0,plan.PlanNum);
			Assert.AreEqual(-1,amt);
		}

		///<summary>Get the copay amount when there is no exact fee on the copay schedule but there is a fee in the default schedule.</summary>
		[TestMethod]
		public void InsPlans_GetCopay_NoExactFeeUseDefault() {
			ProcedureCode procCode=_listProcCodes[1];
			InsPlan plan=GenerateMediFlatInsPlan(MethodBase.GetCurrentMethod().Name);
			Fee feeDefault=FeeT.GetNewFee(plan.FeeSched,procCode.CodeNum,25);
			Prefs.UpdateBool(PrefName.CoPay_FeeSchedule_BlankLikeZero,false);
			double amt=InsPlans.GetCopay(procCode.CodeNum,plan.FeeSched,plan.CopayFeeSched,plan.CodeSubstNone,"",0,0,plan.PlanNum);
			Assert.AreEqual(feeDefault.Amount,amt);
		}

		///<summary>Get the copay amount where there is no exact fee and the Preference CoPay_FeeSchedule_BlankLikeZero is true.</summary>
		[TestMethod]
		public void InsPlans_GetCopay_NoExactFeeUseZero() {
			ProcedureCode procCode=_listProcCodes[2];
			InsPlan plan=GenerateMediFlatInsPlan(MethodBase.GetCurrentMethod().Name);
			Fee feeDefault=FeeT.GetNewFee(plan.FeeSched,procCode.CodeNum,35);
			Prefs.UpdateBool(PrefName.CoPay_FeeSchedule_BlankLikeZero,true);
			double amt=InsPlans.GetCopay(procCode.CodeNum,plan.FeeSched,plan.CopayFeeSched,plan.CodeSubstNone,"",0,0,plan.PlanNum);
			Assert.AreEqual(-1,amt);
			Prefs.UpdateBool(PrefName.CoPay_FeeSchedule_BlankLikeZero,false);
		}

		///<summary>Get the copay value for when there is no substitute fee and the exact copay fee exists.</summary>
		[TestMethod]
		public void InsPlans_GetCopay_ExactFee() {
			ProcedureCode procCode=_listProcCodes[3];
			InsPlan plan=GenerateMediFlatInsPlan(MethodBase.GetCurrentMethod().Name);
			Fee feeDefault=FeeT.GetNewFee(plan.FeeSched,procCode.CodeNum,50);
			Fee feeCopay=FeeT.GetNewFee(plan.CopayFeeSched,procCode.CodeNum,15);
			double amt=InsPlans.GetCopay(procCode.CodeNum,plan.FeeSched,plan.CopayFeeSched,plan.CodeSubstNone,"",0,0,plan.PlanNum);
			Assert.AreEqual(feeCopay.Amount,amt);
		}

		///<summary>Get the copay value for when there is a substitute fee.</summary>
		[TestMethod]
		public void InsPlans_GetCopay_SubstituteFee() {
			ProcedureCode procCode=_listProcCodes[4];
			procCode.SubstitutionCode=_listProcCodes[5].ProcCode;
			ProcedureCodes.Update(procCode);
			InsPlan plan=GenerateMediFlatInsPlan(MethodBase.GetCurrentMethod().Name,false);
			Fee feeDefault=FeeT.GetNewFee(plan.FeeSched,procCode.CodeNum,100);
			Fee feeSubstitute=FeeT.GetNewFee(plan.CopayFeeSched,ProcedureCodes.GetSubstituteCodeNum(procCode.ProcCode,"",plan.PlanNum),45);
			double amt=InsPlans.GetCopay(procCode.CodeNum,plan.FeeSched,plan.CopayFeeSched,plan.CodeSubstNone,"",0,0,plan.PlanNum);
			Assert.AreEqual(feeSubstitute.Amount,amt);
		}

		///<summary>Get the allowed amount for the procedure code for the PPO plan.</summary>
		[TestMethod]
		public void InsPlans_GetAllowed_PPOExact() {
			InsPlan plan=GeneratePPOPlan(MethodBase.GetCurrentMethod().Name);
			ProcedureCode procCode=_listProcCodes[6];
			Fee fee=FeeT.GetNewFee(plan.FeeSched,procCode.CodeNum,65);
			double allowed=InsPlans.GetAllowed(procCode.ProcCode,plan.FeeSched,plan.AllowedFeeSched,plan.CodeSubstNone,plan.PlanType,"",0,0,plan.PlanNum);
			Assert.AreEqual(fee.Amount,allowed);
		}

		///<summary>Get the allowed amount when there is a substitution code for the PPO plan.</summary>
		[TestMethod]
		public void InsPlans_GetAllowed_PPOSubstitute() {
			InsPlan plan=GeneratePPOPlan(MethodBase.GetCurrentMethod().Name,false);
			ProcedureCode procCode=_listProcCodes[7];
			procCode.SubstitutionCode=_listProcCodes[8].ProcCode;
			ProcedureCodes.Update(procCode);
			ProcedureCodes.RefreshCache();
			Fee feeOrig=FeeT.GetNewFee(plan.FeeSched,procCode.CodeNum,85);
			Fee feeSubs=FeeT.GetNewFee(plan.FeeSched,ProcedureCodes.GetSubstituteCodeNum(procCode.ProcCode,"",plan.PlanNum),20);
			double allowed=InsPlans.GetAllowed(procCode.ProcCode,plan.FeeSched,plan.AllowedFeeSched,plan.CodeSubstNone,plan.PlanType,"",0,0,plan.PlanNum);
			Assert.AreEqual(feeSubs.Amount,allowed);
		}

		///<summary>Get the allowed amount where there is a substitution code that is more expensive than the original code for the PPO plan.</summary>
		[TestMethod]
		public void InsPlans_GetAllowed_PPOSubstituteMoreExpensive() {
			InsPlan plan=GeneratePPOPlan(MethodBase.GetCurrentMethod().Name,false);
			ProcedureCode procCode=_listProcCodes[9];
			procCode.SubstitutionCode=_listProcCodes[10].ProcCode;
			ProcedureCodes.Update(procCode);
			Fee feeOrig=FeeT.GetNewFee(plan.FeeSched,procCode.CodeNum,85);
			Fee feeSubs=FeeT.GetNewFee(plan.FeeSched,ProcedureCodes.GetSubstituteCodeNum(procCode.SubstitutionCode,"",plan.PlanNum),200);
			double allowed=InsPlans.GetAllowed(procCode.ProcCode,plan.FeeSched,plan.AllowedFeeSched,plan.CodeSubstNone,plan.PlanType,"",0,0,plan.PlanNum);
			Assert.AreEqual(feeOrig.Amount,allowed);
		}

		///<summary>Get the allowed amount for a capitation plan that has an allowed fee schedule.</summary>
		[TestMethod]
		public void InsPlans_GetAllowed_CapAllowedFeeSched() {
			InsPlan plan=GenerateCapPlan(MethodBase.GetCurrentMethod().Name);
			ProcedureCode procCode=_listProcCodes[11];
			Fee feeAllowed=FeeT.GetNewFee(plan.AllowedFeeSched,procCode.CodeNum,70);
			double amt=InsPlans.GetAllowed(procCode.ProcCode,plan.FeeSched,plan.AllowedFeeSched,plan.CodeSubstNone,plan.PlanType,"",0,0,plan.PlanNum);
			Assert.AreEqual(feeAllowed.Amount,amt);
		}

		///<summary>Get the allowed amount for a capitation plan where there is no allowed fee schedule and there is no substitution code.</summary>
		[TestMethod]
		public void InsPlan_GetAllowed_CapNoAllowedNoSubs() {
			InsPlan plan=GenerateCapPlan(MethodBase.GetCurrentMethod().Name,false);
			ProcedureCode procCode=_listProcCodes[12];
			double amt=InsPlans.GetAllowed(procCode.ProcCode,plan.FeeSched,plan.AllowedFeeSched,plan.CodeSubstNone,plan.PlanType,"",0,0,plan.PlanNum);
			Assert.AreEqual(-1,amt);
		}

		///<summary>Get the allowed amount for a capitation plan where there is no fee schedule assigned to the plan</summary>
		[TestMethod]
		public void InsPlans_GetAllowed_NoFeeSched() {
			Carrier carrier=CarrierT.CreateCarrier(MethodBase.GetCurrentMethod().Name);
			InsPlan plan=new InsPlan();
			plan.CarrierNum=carrier.CarrierNum;
			plan.PlanType="";
			plan.CobRule=EnumCobRule.Basic;
			plan.PlanNum=InsPlans.Insert(plan);
			ProcedureCode procCode=_listProcCodes[13];
			procCode.SubstitutionCode=_listProcCodes[14].ProcCode;
			ProcedureCodes.Update(procCode);
			ProcedureCodes.RefreshCache();
			Provider prov=Providers.GetProv(Preferences.GetLong(PrefName.PracticeDefaultProv));
			long provFeeSched=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,MethodBase.GetCurrentMethod().Name);
			prov.FeeSched=provFeeSched;
			Providers.Update(prov);
			Providers.RefreshCache();
			Fee defaultFee=FeeT.GetNewFee(Providers.GetProv(Preferences.GetLong(PrefName.PracticeDefaultProv)).FeeSched,
				ProcedureCodes.GetSubstituteCodeNum(procCode.ProcCode,"",plan.PlanNum),80);
			double amt=InsPlans.GetAllowed(procCode.ProcCode,plan.FeeSched,plan.AllowedFeeSched,plan.CodeSubstNone,plan.PlanType,"",0,0,plan.PlanNum);
			Assert.AreEqual(defaultFee.Amount,amt);
		}

		#region Factory Methods

		private InsPlan GenerateMediFlatInsPlan(string suffix,bool codeSubstNone=true) {
			long baseFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"Normal_"+suffix,true);
			long copayFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.CoPay,"Copay_"+suffix,true);
			Carrier carrier = CarrierT.CreateCarrier("Carrier_"+suffix);
			return InsPlanT.CreateInsPlanMediFlatCopay(carrier.CarrierNum,baseFeeSchedNum,copayFeeSchedNum,codeSubstNone);
		}

		private InsPlan GeneratePPOPlan(string suffix,bool codeSubstNone=true) {
			long baseFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"Normal_"+suffix,true);
			Carrier carrier=CarrierT.CreateCarrier("Carrier_"+suffix);
			return InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,baseFeeSchedNum,codeSubstNone);
		}

		private InsPlan GenerateCapPlan(string suffix,bool createAllowed=true,bool codeSubstNone=true) {
			long baseFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"Normal_"+suffix,true);
			long allowedFeeSchedNum=0;
			if(createAllowed) {
				allowedFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"Allowed_"+suffix,true);
			}
			Carrier carrier=CarrierT.CreateCarrier("Carrier_"+suffix);
			return InsPlanT.CreateInsPlanCapitation(carrier.CarrierNum,baseFeeSchedNum,allowedFeeSchedNum,codeSubstNone);
		}

		#endregion

		///<summary>Creates a procedure on an ins plan that does not calculate PPO writeoffs for substituted codes.</summary>
		[TestMethod]
		public void InsPlan_PpoSubNoWriteoffs() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			long ucrFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"UCR Fees"+suffix);
			long ppoFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO "+suffix);
			InsuranceInfo ins=InsuranceT.AddInsurance(pat,suffix,planType:"p",feeSchedNum:ppoFeeSchedNum);
			ins.PrimaryInsPlan.HasPpoSubstWriteoffs=false;
			InsPlans.Update(ins.PrimaryInsPlan);
			BenefitT.CreateCategoryPercent(ins.PrimaryInsPlan.PlanNum,EbenefitCategory.Restorative,50);
			ProcedureCode originalProcCode=ProcedureCodes.GetProcCode("D2330");
			ProcedureCode downgradeProcCode=ProcedureCodes.GetProcCode("D2140");
			originalProcCode.SubstitutionCode="D2140";
			originalProcCode.SubstOnlyIf=SubstitutionCondition.Always;
			ProcedureCodeT.Update(originalProcCode);
			FeeT.CreateFee(ucrFeeSchedNum,originalProcCode.CodeNum,100);
			FeeT.CreateFee(ucrFeeSchedNum,downgradeProcCode.CodeNum,80);
			FeeT.CreateFee(ppoFeeSchedNum,originalProcCode.CodeNum,60);
			FeeT.CreateFee(ppoFeeSchedNum,downgradeProcCode.CodeNum,50);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2330",ProcStat.C,"9",100);//Tooth 9
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<Procedure> listProcs=Procedures.Refresh(pat.PatNum);
			ins.RefreshBenefits();
			Claim claim=ClaimT.CreateClaim("P",ins.ListPatPlans,ins.ListInsPlans,listClaimProcs,listProcs,pat,listProcs,ins.ListBenefits,ins.ListInsSubs);
			ClaimProc clProc=ClaimProcs.Refresh(pat.PatNum)[0];//Should only be one
			Assert.AreEqual(50,clProc.Percentage);
			Assert.AreEqual(25,clProc.BaseEst);
			Assert.AreEqual(25,clProc.InsPayEst);
			Assert.AreEqual(-1,clProc.WriteOffEst);
		}

		///<summary>Creates a procedure on an ins plan that does calculate PPO writeoffs for substituted codes.</summary>
		[TestMethod]
		public void InsPlan_PpoSubWriteoffs() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			long ucrFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"UCR Fees"+suffix);
			long ppoFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO "+suffix);
			InsuranceInfo ins=InsuranceT.AddInsurance(pat,suffix,planType:"p",feeSchedNum:ppoFeeSchedNum);
			BenefitT.CreateCategoryPercent(ins.PrimaryInsPlan.PlanNum,EbenefitCategory.Restorative,50);
			ProcedureCode originalProcCode=ProcedureCodes.GetProcCode("D2330");
			ProcedureCode downgradeProcCode=ProcedureCodes.GetProcCode("D2140");
			originalProcCode.SubstitutionCode="D2140";
			originalProcCode.SubstOnlyIf=SubstitutionCondition.Always;
			ProcedureCodeT.Update(originalProcCode);
			FeeT.CreateFee(ucrFeeSchedNum,originalProcCode.CodeNum,100);
			FeeT.CreateFee(ucrFeeSchedNum,downgradeProcCode.CodeNum,80);
			FeeT.CreateFee(ppoFeeSchedNum,originalProcCode.CodeNum,60);
			FeeT.CreateFee(ppoFeeSchedNum,downgradeProcCode.CodeNum,50);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2330",ProcStat.C,"8",100);//Tooth 8
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<Procedure> listProcs=Procedures.Refresh(pat.PatNum);
			ins.RefreshBenefits();
			Claim claim=ClaimT.CreateClaim("P",ins.ListPatPlans,ins.ListInsPlans,listClaimProcs,listProcs,pat,listProcs,ins.ListBenefits,ins.ListInsSubs);
			ClaimProc clProc=ClaimProcs.Refresh(pat.PatNum)[0];//Should only be one
			Assert.AreEqual(50,clProc.Percentage);
			Assert.AreEqual(25,clProc.BaseEst);
			Assert.AreEqual(25,clProc.InsPayEst);
			Assert.AreEqual(40,clProc.WriteOffEst);
		}

		///<summary>Creates a procedure on an ins plan that does not calculate PPO writeoffs for substituted codes where the procedure is not
		///substitued.</summary>
		[TestMethod]
		public void InsPlan_PpoNoSubWriteoffsNoSub() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			long ucrFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"UCR Fees"+suffix);
			long ppoFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO "+suffix);
			InsuranceInfo ins=InsuranceT.AddInsurance(pat,suffix,planType:"p",feeSchedNum:ppoFeeSchedNum);
			ins.PrimaryInsPlan.HasPpoSubstWriteoffs=false;
			InsPlans.Update(ins.PrimaryInsPlan);
			BenefitT.CreateCategoryPercent(ins.PrimaryInsPlan.PlanNum,EbenefitCategory.Restorative,50);
			ProcedureCode originalProcCode=ProcedureCodes.GetProcCode("D2330");
			ProcedureCode downgradeProcCode=ProcedureCodes.GetProcCode("D2140");
			originalProcCode.SubstitutionCode="";//NOT substituting
			originalProcCode.SubstOnlyIf=SubstitutionCondition.Always;
			ProcedureCodeT.Update(originalProcCode);
			FeeT.CreateFee(ucrFeeSchedNum,originalProcCode.CodeNum,100);
			FeeT.CreateFee(ucrFeeSchedNum,downgradeProcCode.CodeNum,80);
			FeeT.CreateFee(ppoFeeSchedNum,originalProcCode.CodeNum,60);
			FeeT.CreateFee(ppoFeeSchedNum,downgradeProcCode.CodeNum,50);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2330",ProcStat.C,"9",100);//Tooth 9
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<Procedure> listProcs=Procedures.Refresh(pat.PatNum);
			ins.RefreshBenefits();
			Claim claim=ClaimT.CreateClaim("P",ins.ListPatPlans,ins.ListInsPlans,listClaimProcs,listProcs,pat,listProcs,ins.ListBenefits,ins.ListInsSubs);
			ClaimProc clProc=ClaimProcs.Refresh(pat.PatNum)[0];//Should only be one
			Assert.AreEqual(50,clProc.Percentage);
			Assert.AreEqual(30,clProc.BaseEst);
			Assert.AreEqual(30,clProc.InsPayEst);
			Assert.AreEqual(40,clProc.WriteOffEst);
		}
	}
}
