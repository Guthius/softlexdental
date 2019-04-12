using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests {
	[TestClass]
	public class ProceduresTest:TestBase {
		
		#region Medicaid COB
		///<summary>Secondary insurance has a COB rule of Medicaid. Tests that secondary writeoff is correct.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_MedicaidCOBSecWO1() {
			ComputeEstimatesMedicaidCOB(MethodBase.GetCurrentMethod().Name,100,70,20,50,100,
				((claimProcPri,claimProcSec,proc) => {
					Assert.AreEqual(35,claimProcSec.WriteOffEst);
				})
			);
		}

		///<summary>Secondary insurance has a COB rule of Medicaid. Tests that patient portion is correct.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_MedicaidCOBPatPort1() {
			ComputeEstimatesMedicaidCOB(MethodBase.GetCurrentMethod().Name,100,70,20,50,100,
				((claimProcPri,claimProcSec,proc) => {
					double patPort=proc.ProcFeeTotal-claimProcPri.InsEstTotal-claimProcPri.WriteOffEst
						-claimProcSec.InsEstTotal-claimProcSec.WriteOffEst;
					Assert.AreEqual(0,patPort);
				})
			);
		}

		///<summary>Secondary insurance has a COB rule of Medicaid. Tests that secondary writeoff is correct.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_MedicaidCOBSecWO2() {
			ComputeEstimatesMedicaidCOB(MethodBase.GetCurrentMethod().Name,100,40,30,50,100,
				((claimProcPri,claimProcSec,proc) => {
					Assert.AreEqual(10,claimProcSec.WriteOffEst);
				})
			);
		}

		///<summary>Secondary insurance has a COB rule of Medicaid. Tests that secondary ins pay is correct.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_MedicaidCOBSecInsPay2() {
			ComputeEstimatesMedicaidCOB(MethodBase.GetCurrentMethod().Name,100,40,30,50,100,
				((claimProcPri,claimProcSec,proc) => {
					Assert.AreEqual(10,claimProcSec.InsPayEst);
				})
			);
		}

		///<summary>Secondary insurance has a COB rule of Medicaid. Tests that patient portion is correct.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_MedicaidCOBPatPort2() {
			ComputeEstimatesMedicaidCOB(MethodBase.GetCurrentMethod().Name,100,40,30,50,100,
				((claimProcPri,claimProcSec,proc) => {
					double patPort=proc.ProcFeeTotal-claimProcPri.InsPayEst-claimProcPri.WriteOffEst
						-claimProcSec.InsPayEst-claimProcSec.WriteOffEst;
					Assert.AreEqual(0,patPort);
				})
			);
		}

		///<summary>Creates a procedure and computes estimates for a patient where the secondary insurance has a COB rule of Medicaid.</summary>
		private void ComputeEstimatesMedicaidCOB(string suffix,double procFee,double priAllowed,double secAllowed,int priPercentCovered,
			int secPercentCovered,Action<ClaimProc/*Primary*/,ClaimProc/*Secondary*/,Procedure> assertAct) 
		{
			Patient pat=PatientT.CreatePatient(suffix);
			long ppoFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO "+suffix);
			InsuranceT.AddInsurance(pat,suffix,"p",ppoFeeSchedNum);
			long medicaidFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO "+suffix);
			InsuranceT.AddInsurance(pat,suffix,"p",medicaidFeeSchedNum,2,cobRule: EnumCobRule.SecondaryMedicaid);
			List<InsSub> listSubs=InsSubT.GetInsSubs(pat);
			List<InsPlan> listPlans=InsPlans.RefreshForSubList(listSubs);
			List<PatPlan> listPatPlans=PatPlans.Refresh(pat.PatNum);
			InsPlan priPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Primary,listPatPlans,listPlans,listSubs);
			BenefitT.CreateCategoryPercent(priPlan.PlanNum,EbenefitCategory.Diagnostic,priPercentCovered);
			InsPlan secPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Secondary,listPatPlans,listPlans,listSubs);
			BenefitT.CreateCategoryPercent(secPlan.PlanNum,EbenefitCategory.Diagnostic,secPercentCovered);
			List<Benefit> listBens=Benefits.Refresh(listPatPlans,listSubs);
			string procStr="D0150";
			Procedure proc=ProcedureT.CreateProcedure(pat,procStr,ProcStat.TP,"",procFee);
			ProcedureCode procCode=ProcedureCodes.GetProcCode(procStr);
			FeeT.CreateFee(ppoFeeSchedNum,procCode.CodeNum,priAllowed);
			FeeT.CreateFee(medicaidFeeSchedNum,procCode.CodeNum,secAllowed);
			Procedures.ComputeEstimates(proc,pat.PatNum,new List<ClaimProc>(),false,listPlans,listPatPlans,listBens,pat.Age,listSubs);
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			assertAct(listClaimProcs.FirstOrDefault(x => x.PlanNum==priPlan.PlanNum),listClaimProcs.FirstOrDefault(x => x.PlanNum==secPlan.PlanNum),proc);
		}

		[TestMethod]
		public void Procedures_ComputeEstimates_PrimaryInsuranceMedicaidCOB() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			long ppoFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO "+suffix);
			InsuranceInfo insInfo=InsuranceT.AddInsurance(pat,suffix,"p",ppoFeeSchedNum,1,cobRule: EnumCobRule.SecondaryMedicaid);
			InsPlan priPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Primary,insInfo.ListPatPlans,insInfo.ListInsPlans,insInfo.ListInsSubs);
			BenefitT.CreateCategoryPercent(priPlan.PlanNum,EbenefitCategory.Diagnostic,percent: 50);
			List<Benefit> listBens=Benefits.Refresh(insInfo.ListPatPlans,insInfo.ListInsSubs);
			string procStr="D0150";
			Procedure proc=ProcedureT.CreateProcedure(pat,procStr,ProcStat.TP,"",125);
			ProcedureCode procCode=ProcedureCodes.GetProcCode(procStr);
			FeeT.CreateFee(ppoFeeSchedNum,procCode.CodeNum,amount: 80);
			Procedures.ComputeEstimates(proc,pat.PatNum,new List<ClaimProc>(),false,insInfo.ListInsPlans,insInfo.ListPatPlans,listBens,pat.Age,
				insInfo.ListInsSubs);
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			Assert.AreEqual(40,listClaimProcs[0].InsEstTotal,0.001);
			Assert.AreEqual(45,listClaimProcs[0].WriteOffEst,0.001);
		}

		#endregion Medicaid COB

		#region Fixed Benefits

		///<summary>.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_FixedBenefitNoFixedBenefitFeeAmtNoPpoFee() {
			ComputeEstimatesFixedBenefits(MethodBase.GetCurrentMethod().Name,100,-1,0,-1,false,0,false,0,
				((assertItem) => {
					Assert.AreEqual(0,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(100,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(0,assertItem.PrimaryClaimProc.WriteOffEst);
				})
			);
		}

		///<summary>.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_FixedBenefitNoFixedBenefitFeeAmt() {
			ComputeEstimatesFixedBenefits(MethodBase.GetCurrentMethod().Name,100,55,0,-1,false,0,false,0,
				((assertItem) => {
					Assert.AreEqual(0,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(55,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(45,assertItem.PrimaryClaimProc.WriteOffEst);
				})
			);
		}

		///<summary>.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_FixedBenefitNoPpoFee() {
			ComputeEstimatesFixedBenefits(MethodBase.GetCurrentMethod().Name,100,-1,12,-1,false,0,false,0,
				((assertItem) => {
					Assert.AreEqual(12,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(88,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(0,assertItem.PrimaryClaimProc.WriteOffEst);
				})
			);
		}

		///<summary>.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_FixedBenefitWithAllFees() {
			ComputeEstimatesFixedBenefits(MethodBase.GetCurrentMethod().Name,100,55,12,-1,false,0,false,0,
				((assertItem) => {
					Assert.AreEqual(12,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(43,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(45,assertItem.PrimaryClaimProc.WriteOffEst);
				})
			);
		}

		///<summary>.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_FixedBenefitWithAllFeesAndSubstitution() {
			ComputeEstimatesFixedBenefits(MethodBase.GetCurrentMethod().Name,100,55,12,-1,false,0,true,5,
				((assertItem) => {
					Assert.AreEqual(5,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(50,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(45,assertItem.PrimaryClaimProc.WriteOffEst);
				})
			);
		}

		///<summary>.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_FixedBenefitWithPercentOverride() {
			ComputeEstimatesFixedBenefits(MethodBase.GetCurrentMethod().Name,100,55,12,25,false,0,false,0,
				((assertItem) => {
					Assert.AreEqual(3,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(43,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(45,assertItem.PrimaryClaimProc.WriteOffEst);
				})
			);
		}

		///<summary>.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_FixedBenefitWithSecondaryIns() {
			ComputeEstimatesFixedBenefits(MethodBase.GetCurrentMethod().Name,100,55,12,-1,true,15,false,0,
				((assertItem) => {
					Assert.AreEqual(12,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(43,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(45,assertItem.PrimaryClaimProc.WriteOffEst);
					Assert.AreEqual(43,assertItem.SecondaryClaimProc.InsEstTotal);
				})
			);
		}

		///<summary>Creates a procedure and computes estimates for a patient where the secondary insurance has a COB rule of Medicaid.</summary>
		private void ComputeEstimatesFixedBenefits(string suffix,double procFee,double ppoFee,double fixedBenefitFee
			,int priPercentCoveredOverride,bool hasSecondary,double secFee,bool hasSubstitution,double fixedBenefitFeeSub,Action<FixedBenefitAssertItem> assertAct)
		{
			Patient pat=PatientT.CreatePatient(suffix);
			string procStr="D0150";
			string procStrSubst="D0160";
			Procedure proc=ProcedureT.CreateProcedure(pat,procStr,ProcStat.TP,"",procFee);
			ProcedureCode procCode=ProcedureCodes.GetProcCode(procStr);
			procCode.SubstitutionCode=hasSubstitution ? procStrSubst : "";
			ProcedureCodes.Update(procCode);
			Cache.Refresh(InvalidType.ProcCodes);
			ProcedureCode procCodeSubst=ProcedureCodes.GetProcCode(procStrSubst);
			long ppoFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO "+suffix);
			long catPercFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"Category % "+suffix);
			long fixedBenefitFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.FixedBenefit,"Fixed Benefit "+suffix);
			if(ppoFee>-1) {
				FeeT.CreateFee(ppoFeeSchedNum,procCode.CodeNum,ppoFee);
			}
			FeeT.CreateFee(ppoFeeSchedNum,procCodeSubst.CodeNum,ppoFee);
			FeeT.CreateFee(fixedBenefitFeeSchedNum,procCode.CodeNum,fixedBenefitFee);
			FeeT.CreateFee(fixedBenefitFeeSchedNum,procCodeSubst.CodeNum,fixedBenefitFeeSub);
			InsuranceT.AddInsurance(pat,suffix,"p",ppoFeeSchedNum,copayFeeSchedNum: fixedBenefitFeeSchedNum);
			if(hasSecondary) {
				FeeT.CreateFee(catPercFeeSchedNum,procCode.CodeNum,secFee);
				InsuranceT.AddInsurance(pat,suffix,"",catPercFeeSchedNum,2,false,EnumCobRule.Standard);
			}
			List<InsSub> listSubs=InsSubT.GetInsSubs(pat);
			List<InsPlan> listPlans=InsPlans.RefreshForSubList(listSubs);
			List<PatPlan> listPatPlans=PatPlans.Refresh(pat.PatNum);
			InsPlan priPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Primary,listPatPlans,listPlans,listSubs);
			InsPlan secPlan=null;
			if(hasSecondary) {
				secPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Secondary,listPatPlans,listPlans,listSubs);
				//TODO:  Add diagnostic code benefit for 100%
				BenefitT.CreateCategoryPercent(secPlan.PlanNum,EbenefitCategory.Diagnostic,100);
			}
			List<Benefit> listBens=Benefits.Refresh(listPatPlans,listSubs);
			Procedures.ComputeEstimates(proc,pat.PatNum,new List<ClaimProc>(),false,listPlans,listPatPlans,listBens,pat.Age,listSubs);
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			if(priPercentCoveredOverride>0) {
				foreach(ClaimProc cpCur in listClaimProcs) {
					cpCur.PercentOverride=priPercentCoveredOverride;
					ClaimProcs.Update(cpCur);
				}
				Procedures.ComputeEstimates(proc,pat.PatNum,new List<ClaimProc>(),false,listPlans,listPatPlans,listBens,pat.Age,listSubs);
				listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			}
			foreach(ClaimProc cpCur in listClaimProcs) {
				cpCur.PercentOverride=priPercentCoveredOverride;
				ClaimProcs.Update(cpCur);
			}
			Procedures.ComputeEstimates(proc,pat.PatNum,listClaimProcs,false,listPlans,listPatPlans,listBens,pat.Age,listSubs);
			listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			assertAct(new FixedBenefitAssertItem() {
				Procedure=proc,
				PrimaryClaimProc=listClaimProcs.FirstOrDefault(x => x.PlanNum==priPlan.PlanNum),
				SecondaryClaimProc=secPlan==null ? null : listClaimProcs.FirstOrDefault(x => x.PlanNum==secPlan.PlanNum),
			});
		}

		private class FixedBenefitAssertItem {
			public Procedure Procedure;
			public ClaimProc PrimaryClaimProc;
			public ClaimProc SecondaryClaimProc;
		}

		#endregion

		#region GetProcFee

		///<summary>The procedure fee for a medical insurance ppo is the UCR fee.</summary>
		[TestMethod]
		public void Procedures_GetProcFee_WithMedicalInsurance() {
			double procFee=GetProcFee(MethodBase.GetCurrentMethod().Name,false);
			Assert.AreEqual(300,procFee);
		}

		///<summary>The procedure fee for a medical insurance ppo is the UCR fee of the medical code.</summary>
		[TestMethod]
		public void Procedures_GetProcFee_WithMedicalCode() {
			double procFee=GetProcFee(MethodBase.GetCurrentMethod().Name,true);
			Assert.AreEqual(300,procFee);
		}

		///<summary>Creates a procedure and returns its procedure fee.</summary>
		private double GetProcFee(string suffix,bool doUseMedicalCode) {
			Prefs.UpdateBool(PrefName.InsPpoAlwaysUseUcrFee,true);
			Prefs.UpdateBool(PrefName.MedicalFeeUsedForNewProcs,doUseMedicalCode);
			Patient pat=PatientT.CreatePatient(suffix);
			long ucrFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"UCR "+suffix);
			FeeSchedT.UpdateUCRFeeSched(pat,ucrFeeSchedNum);
			long ppoFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO "+suffix);
			InsuranceT.AddInsurance(pat,suffix,"p",ppoFeeSchedNum,1,true);
			List<InsSub> listSubs=InsSubT.GetInsSubs(pat);
			List<InsPlan> listPlans=InsPlans.RefreshForSubList(listSubs);
			List<PatPlan> listPatPlans=PatPlans.Refresh(pat.PatNum);
			string procStr="D0150";
			string procStrMed="D0120";
			ProcedureCode procCode=ProcedureCodes.GetProcCode(procStr);
			ProcedureCode procCodeMed=ProcedureCodes.GetProcCode(procStrMed);
			procCode.MedicalCode=procCodeMed.ProcCode;
			FeeT.CreateFee(ucrFeeSchedNum,procCode.CodeNum,300);
			FeeT.CreateFee(ppoFeeSchedNum,procCode.CodeNum,120);
			FeeT.CreateFee(ucrFeeSchedNum,procCodeMed.CodeNum,175);
			FeeT.CreateFee(ppoFeeSchedNum,procCodeMed.CodeNum,85);
			Procedure proc=ProcedureT.CreateProcedure(pat,procStr,ProcStat.TP,"",300);
			return Procedures.GetProcFee(pat,listPatPlans,listSubs,listPlans,procCode.CodeNum,proc.ProvNum,proc.ClinicNum,procCode.MedicalCode);
		}

		#endregion GetProcFee

		#region IsSameProcedureArea
		[TestMethod]
		public void Procedures_IsSameProcedureArea_SameToothNums() {
			Assert.IsTrue(Procedures.IsSameProcedureArea("2","2","","","","",TreatmentArea.Tooth));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_DifferentToothNums() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("","1","","","","",TreatmentArea.Tooth));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_PartialMatchToothNums() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("22","2","","","","",TreatmentArea.Tooth));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_EmptyProcCurToothRange() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("","","2,4,7","","","",TreatmentArea.ToothRange));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_PartialMatchToothRanges() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("","","7,16,22","2","","",TreatmentArea.ToothRange));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_ToothRangeMatchingTails() {
			Assert.IsTrue(Procedures.IsSameProcedureArea("","","7,3","15,7,3","","",TreatmentArea.ToothRange));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_ToothRangeMatchingElementsOutOfOrder() {
			Assert.IsTrue(Procedures.IsSameProcedureArea("","","15,7,3,4","7,15","","",TreatmentArea.ToothRange));
		}

		[TestMethod]
		public void Procedures_IsSameProcedureArea_OneToothRangeBlank() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("","","1,4","","","",TreatmentArea.Tooth));
		}

		[TestMethod]
		public void Procedures_IsSameProcedureArea_InvalidToothRangeEntry() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("","","1,,5,","","","",TreatmentArea.ToothRange));
		}

		[TestMethod]
		public void Procedures_IsSameProcedureArea_InvalidEntryWithMatch() {
			Assert.IsTrue(Procedures.IsSameProcedureArea("","","1,,4","4","","",TreatmentArea.ToothRange));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_SameToothNumEmptySurf() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("4","4","","","","M",TreatmentArea.Tooth));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_SameToothNumSameSurf() {
			Assert.IsTrue(Procedures.IsSameProcedureArea("9","9","","","L","L",TreatmentArea.Tooth));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_SameToothNumPartialSurfMatch() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("5","5","","","ML","M",TreatmentArea.Tooth));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_SameToothNumEmptySurfs() {
			Assert.IsTrue(Procedures.IsSameProcedureArea("14","14","","","","",TreatmentArea.Tooth));
		}

		[TestMethod]
		public void Procedures_IsSameProcedureArea_HasNoArea() {
			//E.g. exams and BW's will not have any 'procedure areas' and should return true
			Assert.IsTrue(Procedures.IsSameProcedureArea("","","","","","",TreatmentArea.Tooth));
		}

		[TestMethod]
		public void Procedures_IsSameProcedureArea_ArchesNotMatching() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("","","","","U","L",TreatmentArea.Arch));
		}

		[TestMethod]
		public void Procedures_IsSameProcedureArea_QuadsNotMatching() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("","","","","UL","UR",TreatmentArea.Quad));
		}

		[TestMethod]
		public void Procedures_IsSameProcedureArea_SextantsNotMatching() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("","","","","1","2",TreatmentArea.Sextant));
		}

		[TestMethod]
		public void Procedures_IsSameProcedureArea_Arches() {
			Assert.IsTrue(Procedures.IsSameProcedureArea("","","","","U","U",TreatmentArea.Arch));
		}

		[TestMethod]
		public void Procedures_IsSameProcedureArea_Quads() {
			Assert.IsTrue(Procedures.IsSameProcedureArea("","","","","UL","UL",TreatmentArea.Quad));
		}

		[TestMethod]
		public void Procedures_IsSameProcedureArea_Sextants() {
			Assert.IsTrue(Procedures.IsSameProcedureArea("","","","","1","1",TreatmentArea.Sextant));
		}

		#endregion

		#region InsHist

		///<summary>Update procedure date on an existing procedure. Test that the procedure and claimproc has the updated procedure date.</summary>
		[TestMethod]
		public void Procedures_InsertOrUpdateInsHistProcedure_ExistingProc() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			DateTime date=DateTime.Today;
			Procedure proc=ProcedureT.CreateProcedure(pat,"D4341",ProcStat.EO,"",0,procDate:date,surf:"UL");
			InsuranceInfo ins=InsuranceT.AddInsurance(pat,suffix);
			//Create a Claimproc with status InsHist
			ClaimProc priClaimProc=ClaimProcT.CreateClaimProc(pat.PatNum,proc.ProcNum,ins.ListInsPlans.FirstOrDefault().PlanNum,
				ins.ListInsSubs.FirstOrDefault().InsSubNum,date,-1,-1,-1,ClaimProcStatus.InsHist);
			//Change the proc date
			date=date.AddDays(4);
			List<ClaimProc> listClaimProcsForEoProcs;
			Dictionary<PrefName,Procedure> listEoProcs=Procedures.GetDictInsHistProcs(pat.PatNum,priClaimProc.InsSubNum,out listClaimProcsForEoProcs);
			//The procedure and claimproc need the new date.
			Procedures.InsertOrUpdateInsHistProcedure(pat,PrefName.InsHistPerioULCodes,date,ins.ListInsPlans.FirstOrDefault().PlanNum,
				ins.ListInsSubs.FirstOrDefault().InsSubNum,listEoProcs[PrefName.InsHistPerioULCodes],listClaimProcsForEoProcs);
			Procedure procFromDb=Procedures.GetOneProc(proc.ProcNum,false);
			Assert.AreEqual(date,procFromDb.ProcDate);
			ClaimProc claimProcFromDb=ClaimProcs.GetForProcs(new List<long> { proc.ProcNum }).FirstOrDefault();
			Assert.AreEqual(date,claimProcFromDb.ProcDate);
		}

		///<summary>Update procedure date on an existing Completed procedure. Test that a new procedure and InsHist claimproc is created with the new procedure date.</summary>
		[TestMethod]
		public void Procedures_InsertOrUpdateInsHistProcedure_ExistingCProc() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			DateTime date=DateTime.Today;
			Procedure proc=ProcedureT.CreateProcedure(pat,"D4341",ProcStat.C,"",0,procDate:date,surf:"UL");
			InsuranceInfo ins=InsuranceT.AddInsurance(pat,suffix);
			ins.ListAllProcs.Add(proc);
			Claim claim=ClaimT.CreateClaim(new List<Procedure> { proc },ins);
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			ClaimT.ReceiveClaim(claim,listClaimProcs);
			//Change the proc date
			date=date.AddDays(4);
			List<ClaimProc> listClaimProcsForEoProcs;
			Dictionary<PrefName,Procedure> dictEOAndCProcs=Procedures.GetDictInsHistProcs(pat.PatNum,ins.PrimaryInsSub.InsSubNum,out listClaimProcsForEoProcs);
			//The procedure and claimproc need the new date.
			Procedures.InsertOrUpdateInsHistProcedure(pat,PrefName.InsHistPerioULCodes,date,ins.ListInsPlans.FirstOrDefault().PlanNum,
				ins.ListInsSubs.FirstOrDefault().InsSubNum,dictEOAndCProcs[PrefName.InsHistPerioULCodes],listClaimProcsForEoProcs);
			Procedure procFromDb=Procedures.Refresh(pat.PatNum).FirstOrDefault(x=>x.ProcDate.Date==date.Date);
			Assert.AreEqual(date.Date,procFromDb.ProcDate.Date);
			Assert.AreEqual(ProcStat.EO,procFromDb.ProcStatus);
			ClaimProc claimProcFromDb=ClaimProcs.GetForProcs(new List<long> { procFromDb.ProcNum }).FirstOrDefault();
			Assert.AreEqual(date.Date,claimProcFromDb.ProcDate.Date);
			Assert.AreEqual(ClaimProcStatus.InsHist,claimProcFromDb.Status);
		}

		///<summary>Update procedure date on an existing Completed procedure. Test that a new procedure and InsHist claimproc is created with the new procedure date.</summary>
		[TestMethod]
		public void Procedures_GetDictInsHistProcs_ExistingCProc() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			DateTime date=DateTime.Today;
			Procedure proc=ProcedureT.CreateProcedure(pat,"D4341",ProcStat.C,"",0,procDate:date,surf:"UL");
			InsuranceInfo ins=InsuranceT.AddInsurance(pat,suffix);
			ins.ListAllProcs.Add(proc);
			Claim claim=ClaimT.CreateClaim(new List<Procedure> { proc },ins);
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			ClaimT.ReceiveClaim(claim,listClaimProcs);
			List<ClaimProc> listClaimProcsForEoProcs;
			Dictionary<PrefName,Procedure> dictEOAndCProcs=Procedures.GetDictInsHistProcs(pat.PatNum,ins.PrimaryInsSub.InsSubNum,out listClaimProcsForEoProcs);
			Procedure procFromDict=dictEOAndCProcs[PrefName.InsHistPerioULCodes];
			Assert.AreEqual(date.Date,procFromDict.ProcDate.Date);
			Assert.AreEqual(ProcStat.C,procFromDict.ProcStatus);
		}

		///<summary>New procedures is added. Test that the new procecdure has the correct date,status, and inssubnum.</summary>
		[TestMethod]
		public void Procedures_InsertOrUpdateInsHistProcedure_NewProc() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			DateTime date=DateTime.Today;
			InsuranceInfo ins=InsuranceT.AddInsurance(pat,suffix);
			List<ClaimProc> listClaimProcsForEoProcs;
			Dictionary<PrefName,Procedure> listEoProcs=Procedures.GetDictInsHistProcs(pat.PatNum,ins.ListInsSubs.FirstOrDefault().InsSubNum,out listClaimProcsForEoProcs);
			//The procedure and claimproc need the new date.
			Procedures.InsertOrUpdateInsHistProcedure(pat,PrefName.InsHistProphyCodes,date,ins.ListInsPlans.FirstOrDefault().PlanNum,
				ins.ListInsSubs.FirstOrDefault().InsSubNum,listEoProcs[PrefName.InsHistProphyCodes],listClaimProcsForEoProcs);
			Procedure procFromDb=Procedures.GetProcsByStatusForPat(pat.PatNum,ProcStat.EO).FirstOrDefault();
			Assert.AreEqual(date,procFromDb.ProcDate);
			Assert.AreEqual(ProcStat.EO,procFromDb.ProcStatus);
			ClaimProc claimProcFromDb=ClaimProcs.GetForProcs(new List<long> { procFromDb.ProcNum }).FirstOrDefault();
			Assert.AreEqual(date,claimProcFromDb.ProcDate);
			Assert.AreEqual(ClaimProcStatus.InsHist,claimProcFromDb.Status);
			Assert.AreEqual(ins.ListInsSubs.FirstOrDefault().InsSubNum,claimProcFromDb.InsSubNum);
		}
		#endregion

		#region RpProcOverpaid
		[TestMethod]
		public void Procedures_RpProcOverpaid_HasOverPay() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			InsuranceInfo insInfo=InsuranceT.AddInsurance(pat,suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",88,DateTime.Today);
			insInfo.ListAllProcs.Add(proc1);
			Claim claim=ClaimT.CreateClaim(new List<Procedure> { proc1 },insInfo);
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			foreach(ClaimProc cp in listClaimProcs) {
				cp.WriteOff=33;
				cp.InsPayAmt=25;
			}
			ClaimT.ReceiveClaim(claim,listClaimProcs);
			//Procedure has credits equal to 58
			AdjustmentT.MakeAdjustment(pat.PatNum,-20,procDate: proc1.ProcDate,procNum: proc1.ProcNum);//-adjustment=-20
			PaymentT.MakePayment(pat.PatNum,11,proc1.ProcDate,provNum: proc1.ProvNum,procNum: proc1.ProcNum,clinicNum: proc1.ClinicNum);//Pat pay=11
			//ProcFee=88. Total credits=89. Overpayment=-1
			DataTable table=RpProcOverpaid.GetOverPaidProcs(pat.PatNum,new List<long> { proc1.ProvNum },new List<long> { proc1.ClinicNum },proc1.ProcDate,proc1.ProcDate);
			Assert.AreEqual(1,table.Rows.Count);
			Assert.IsTrue(PIn.Double(table.Rows[0]["Overpay"].ToString())==-1);
		}

		[TestMethod]
		public void Procedures_RpProcOverpaid_NotOverPay() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			InsuranceInfo insInfo=InsuranceT.AddInsurance(pat,suffix);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",88,DateTime.Today);
			insInfo.ListAllProcs.Add(proc1);
			Claim claim=ClaimT.CreateClaim(new List<Procedure> { proc1 },insInfo);
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			foreach(ClaimProc cp in listClaimProcs) {
				cp.WriteOff=33;
				cp.InsPayAmt=25;
			}
			ClaimT.ReceiveClaim(claim,listClaimProcs);
			//Procedure has credits equal to 58
			AdjustmentT.MakeAdjustment(pat.PatNum,-20,procDate: proc1.ProcDate,procNum: proc1.ProcNum);//-adjustment=-20
			PaymentT.MakePayment(pat.PatNum,10,proc1.ProcDate,provNum: proc1.ProvNum,procNum: proc1.ProcNum,clinicNum: proc1.ClinicNum);//Pat pay=10
			//ProcFee=88. Total credits=88. Overpayment=0
			DataTable table=RpProcOverpaid.GetOverPaidProcs(pat.PatNum,new List<long> { proc1.ProvNum },new List<long> { proc1.ClinicNum },proc1.ProcDate,proc1.ProcDate);
			Assert.AreEqual(0,table.Rows.Count);
		}
		#endregion

		[TestMethod]
		public void Procedures_SetCompleteInAppt_MetFrequencyLimit() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			PrefT.UpdateBool(PrefName.ClaimProcsAllowedToBackdate,true);
			Patient pat=PatientT.CreatePatient(suffix);
			InsuranceInfo insInfo=InsuranceT.AddInsurance(pat,suffix);
			insInfo.ListBenefits.Add(BenefitT.CreateCategoryPercent(insInfo.PrimaryInsPlan.PlanNum,EbenefitCategory.RoutinePreventive,100));
			insInfo.ListBenefits.Add(BenefitT.CreateFrequencyLimitation("D1110",1,BenefitQuantity.NumberOfServices,insInfo.PrimaryInsPlan.PlanNum,
				BenefitTimePeriod.CalendarYear));
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",88,new DateTime(2018,1,10));
			insInfo.ListAllProcs.Add(proc1);
			Claim claim=ClaimT.CreateClaim(new List<Procedure> { proc1 },insInfo);
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			ClaimT.ReceiveClaim(claim,listClaimProcs);
			Operatory op=OperatoryT.CreateOperatory(suffix);
			Appointment apt=AppointmentT.CreateAppointment(pat.PatNum,new DateTime(2018,2,1),op.OperatoryNum,pat.PriProv);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.TP,"",77,new DateTime(2018,2,1),aptNum:apt.AptNum);
			insInfo.ListAllProcs.Add(proc2);
			//This will compute estimates.
			Procedures.SetCompleteInAppt(apt,insInfo.ListInsPlans,insInfo.ListPatPlans,pat,insInfo.ListInsSubs,false);
			listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			//2nd proc has reached frequency limitation.
			Assert.AreEqual(0,listClaimProcs.First(x => x.ProcNum==proc2.ProcNum).InsPayEst);
		}

		[TestMethod]
		public void Procedures_NotReceivedCP_MetFrequencyLimit() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			PrefT.UpdateBool(PrefName.ClaimProcsAllowedToBackdate,true);
			Patient pat=PatientT.CreatePatient(suffix);
			InsuranceInfo insInfo=InsuranceT.AddInsurance(pat,suffix);
			//add a frequency limit of 1 for procedure D1110
			insInfo.ListBenefits.Add(BenefitT.CreateCategoryPercent(insInfo.PrimaryInsPlan.PlanNum,EbenefitCategory.RoutinePreventive,100));
			insInfo.ListBenefits.Add(BenefitT.CreateFrequencyLimitation("D1110",1,BenefitQuantity.NumberOfServices,insInfo.PrimaryInsPlan.PlanNum,
				BenefitTimePeriod.CalendarYear));
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",88,DateTime.Today.AddDays(-1));
			insInfo.ListAllProcs.Add(proc1);
			//Creates a cp with status of NotReceived
			Claim claim=ClaimT.CreateClaim(new List<Procedure> { proc1 },insInfo);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.TP,"",77,DateTime.Today);
			insInfo.ListAllProcs.Add(proc2);
			//This will compute estimates.
			List<ClaimProc> listClaimProcs=ProcedureT.ComputeEstimates(pat,insInfo);
			//2nd proc has reached frequency limitation.
			Assert.AreEqual(0,listClaimProcs.First(x => x.ProcNum==proc2.ProcNum).InsPayEst);
		}

		[TestMethod]
		public void Procedures_NotReceivedCP_BenefitTimePeriod_NumberInLast12Months_MetFrequencyLimit() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			PrefT.UpdateBool(PrefName.ClaimProcsAllowedToBackdate,true);
			Patient pat=PatientT.CreatePatient(suffix);
			InsuranceInfo insInfo=InsuranceT.AddInsurance(pat,suffix);
			//add a frequency limit of 1 for procedure D1110
			insInfo.ListBenefits.Add(BenefitT.CreateCategoryPercent(insInfo.PrimaryInsPlan.PlanNum,EbenefitCategory.RoutinePreventive,100));
			insInfo.ListBenefits.Add(BenefitT.CreateFrequencyLimitation("D1110",1,BenefitQuantity.NumberOfServices,insInfo.PrimaryInsPlan.PlanNum,
				BenefitTimePeriod.NumberInLast12Months));
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",88,DateTime.Today.AddDays(-1));
			insInfo.ListAllProcs.Add(proc1);
			//Creates a cp with status of NotReceived
			Claim claim=ClaimT.CreateClaim(new List<Procedure> { proc1 },insInfo);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.TP,"",77,DateTime.Today);
			insInfo.ListAllProcs.Add(proc2);
			//This will compute estimates.
			List<ClaimProc> listClaimProcs=ProcedureT.ComputeEstimates(pat,insInfo);
			//2nd proc has reached frequency limitation.
			Assert.AreEqual(0,listClaimProcs.First(x => x.ProcNum==proc2.ProcNum).InsPayEst);
		}

		///<summary>A completed procedure is attached to a completed appointment. Then a different TP procedure is attached to the same appointment and
		///the method to complete the procedures on the appointment is called. The test then verifies that the deductible is calculated correctly for
		///each procedure.</summary>
		[TestMethod]
		public void Procedures_SetCompleteInApptInList_CompletedProcAttachedToClaimDeductible() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			InsuranceInfo insInfo=InsuranceT.AddInsurance(pat,suffix);
			insInfo.ListBenefits.Add(BenefitT.CreateDeductibleGeneral(insInfo.PrimaryInsPlan.PlanNum,BenefitCoverageLevel.Individual,50));
			insInfo.ListBenefits.Add(BenefitT.CreateCategoryPercent(insInfo.PrimaryInsPlan.PlanNum,EbenefitCategory.RoutinePreventive,100));
			insInfo.ListBenefits.Add(BenefitT.CreateCategoryPercent(insInfo.PrimaryInsPlan.PlanNum,EbenefitCategory.Diagnostic,100));
			Appointment apt=AppointmentT.CreateAppointment(pat.PatNum,DateTime.Today.AddHours(9),0,pat.PriProv,aptStatus: ApptStatus.Complete);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",88,DateTime.Today,aptNum: apt.AptNum);
			insInfo.ListAllProcs.Add(proc1);
			//Creates a cp with status of NotReceived
			Claim claim=ClaimT.CreateClaim(new List<Procedure> { proc1 },insInfo);
			insInfo.ListAllClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			Assert.AreEqual(50,insInfo.ListAllClaimProcs.First(x => x.ProcNum==proc1.ProcNum).DedEst);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D2740",ProcStat.TP,"",77,DateTime.Today,aptNum: apt.AptNum);
			insInfo.ListAllProcs.Add(proc2);
			List<Procedure> listProcs=new List<Procedure> { proc1, proc2 };
			//Also computes estimates
			Procedures.SetCompleteInApptInList(apt,insInfo.ListInsPlans,insInfo.ListPatPlans,pat,listProcs,insInfo.ListInsSubs,Security.CurUser);
			insInfo.ListAllClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			Assert.AreEqual(50,insInfo.ListAllClaimProcs.First(x => x.ProcNum==proc1.ProcNum).DedEst);
			Assert.AreEqual(0,insInfo.ListAllClaimProcs.First(x => x.ProcNum==proc2.ProcNum).DedEst);
		}

		///<summary>Two completed procedures are attached to a completed appointment. Then a different TP procedure is attached to the same appointment 
		///and the method to complete the procedures on the appointment is called. The test then verifies that the deductible is calculated correctly for
		///each procedure.</summary>
		[TestMethod]
		public void Procedures_SetCompleteInApptInList_MultipleCompletedProcAttachedToClaimDeductible() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix);
			InsuranceInfo insInfo=InsuranceT.AddInsurance(pat,suffix);
			insInfo.ListBenefits.Add(BenefitT.CreateDeductibleGeneral(insInfo.PrimaryInsPlan.PlanNum,BenefitCoverageLevel.Individual,50));
			insInfo.ListBenefits.Add(BenefitT.CreateCategoryPercent(insInfo.PrimaryInsPlan.PlanNum,EbenefitCategory.RoutinePreventive,100));
			insInfo.ListBenefits.Add(BenefitT.CreateCategoryPercent(insInfo.PrimaryInsPlan.PlanNum,EbenefitCategory.Diagnostic,100));
			Appointment apt=AppointmentT.CreateAppointment(pat.PatNum,DateTime.Today.AddHours(9),0,pat.PriProv,aptStatus: ApptStatus.Complete);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",88,DateTime.Today,aptNum: apt.AptNum);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",99,DateTime.Today,aptNum: apt.AptNum);
			insInfo.ListAllProcs.Add(proc1);
			insInfo.ListAllProcs.Add(proc2);
			//Creates a claim with the proc charted 2nd as the 1st proc on the claim.
			Claim claim=ClaimT.CreateClaim(new List<Procedure> { proc2,proc1 },insInfo);
			insInfo.ListAllClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			Assert.AreEqual(50,insInfo.ListAllClaimProcs.First(x => x.ProcNum==proc2.ProcNum).DedEst);
			Procedure proc3=ProcedureT.CreateProcedure(pat,"D2740",ProcStat.TP,"",77,DateTime.Today,aptNum: apt.AptNum);
			insInfo.ListAllProcs.Add(proc3);
			List<Procedure> listProcs=new List<Procedure> { proc1, proc2, proc3 };
			//Also computes estimates
			Procedures.SetCompleteInApptInList(apt,insInfo.ListInsPlans,insInfo.ListPatPlans,pat,listProcs,insInfo.ListInsSubs,Security.CurUser);
			insInfo.ListAllClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			Assert.AreEqual(0,insInfo.ListAllClaimProcs.First(x => x.ProcNum==proc1.ProcNum).DedEst);
			Assert.AreEqual(50,insInfo.ListAllClaimProcs.First(x => x.ProcNum==proc2.ProcNum).DedEst);
			Assert.AreEqual(0,insInfo.ListAllClaimProcs.First(x => x.ProcNum==proc3.ProcNum).DedEst);
		}

		///<summary>Primary claim is received, and since Pref.InsEstRecalcReceived is false, its ClaimProc estimates should not be recalculated.  
		///However, the secondary estimates should still recalcuate since they are not received.  Secondary claimprocs must factor in the paid estimates
		///from the primary claim.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_InsEstRecalcReceivedPriNotReceivedSec_False() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			const int coveragePercent=100;
			const double ucrFee=50;
			string procStr="D0120";
			string planType="p";//PPO percentage
			double feeSchedFee=40;
			//Expected that estimates will not be recalculated for Received ClaimProc.  However, secondary should calculate, factoring in estimates from primary.
			double estimatedSecBaseEst=0;
			double estimatedSecInsEstTotal=0;
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
			#region Primary InsPlan Setup
			InsuranceT.AddInsurance(pat,suffix,planType,feeSchedNum,ordinal:1);
			List<InsSub> listSubs=InsSubT.GetInsSubs(pat);
			List<InsPlan> listInsPlans=InsPlans.RefreshForSubList(listSubs);
			List<PatPlan> listPatPlans=PatPlans.Refresh(pat.PatNum);
			InsPlan priPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Primary,listPatPlans,listInsPlans,listSubs);
			InsSub priSub=InsSubT.GetSubForPriSecMed(PriSecMed.Primary,listPatPlans,listInsPlans,listSubs);
			if(planType!="f" && planType!="c") {//Only add a 50 percent coverage to the plan for the example proc if not Medicaid/Flat Copay or capitation
				BenefitT.CreatePercentForProc(priPlan.PlanNum,procCode.CodeNum,coveragePercent);
			}
			#endregion
			#region Secondary InsPlan Setup
			InsuranceT.AddInsurance(pat,suffix,planType,feeSchedNum,ordinal:2);
			listSubs=InsSubT.GetInsSubs(pat);
			listInsPlans=InsPlans.RefreshForSubList(listSubs);
			listPatPlans=PatPlans.Refresh(pat.PatNum);
			InsPlan secPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Secondary,listPatPlans,listInsPlans,listSubs);
			InsSub secSub=InsSubT.GetSubForPriSecMed(PriSecMed.Secondary,listPatPlans,listInsPlans,listSubs);
			if(planType!="f" && planType!="c") {//Only add a 50 percent coverage to the plan for the example proc if not Medicaid/Flat Copay or capitation
				BenefitT.CreatePercentForProc(secPlan.PlanNum,procCode.CodeNum,coveragePercent);
			}
			#endregion
			PatPlan priPatPlan=listPatPlans.FirstOrDefault(x => x.InsSubNum==priSub.InsSubNum);
			double procFee=Procedures.GetProcFee(pat,listPatPlans,listSubs,listInsPlans,procCode.CodeNum,pat.PriProv,0,"");
			Procedure proc=ProcedureT.CreateProcedure(pat,procStr,ProcStat.C,"",procFee);
			ClaimProc priClaimProc=ClaimProcT.CreateClaimProc(pat.PatNum,proc.ProcNum,priPlan.PlanNum,priSub.InsSubNum,DateTime.Today,-1,-1,-1,ClaimProcStatus.Estimate);
			ClaimProc secClaimProc=ClaimProcT.CreateClaimProc(pat.PatNum,proc.ProcNum,secPlan.PlanNum,secSub.InsSubNum,DateTime.Today,-1,-1,-1,ClaimProcStatus.Estimate);
			List<Benefit> listBens=Benefits.Refresh(listPatPlans,listSubs);
			List<SubstitutionLink> listSubLinks=SubstitutionLinks.GetAllForPlans(listInsPlans);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<ClaimProc> listClaimProcs=new List<ClaimProc>() { priClaimProc, secClaimProc };
			PrefT.UpdateBool(PrefName.InsEstRecalcReceived,false);//Set InsEstRecalcReceived preference to false.
			Procedures.ComputeEstimates(proc,pat.PatNum,listClaimProcs,true,listInsPlans,listPatPlans,listBens,pat.Age,listSubs);
			priClaimProc.Status=ClaimProcStatus.Received;//Set the primary claimproc received.
			priClaimProc.InsPayAmt=feeSchedFee*(coveragePercent/100d);
			priClaimProc.WriteOff=(ucrFee-feeSchedFee);//(ucr-schedfee)
			Procedures.ComputeEstimates(proc,pat.PatNum,listClaimProcs,false,listInsPlans,listPatPlans,listBens,pat.Age,listSubs);
			Assert.AreEqual(estimatedSecBaseEst,secClaimProc.BaseEst);
			Assert.AreEqual(estimatedSecInsEstTotal,secClaimProc.InsEstTotal);
		}

		///<summary>Primary claim is received, and since Pref.InsEstRecalcReceived is false, its ClaimProc estimates should not be recalculated.  
		///However, the secondary estimates should still recalcuate since they are not received.  Secondary claimprocs must factor in the paid estimates
		///from the primary claim.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_InsEstRecalcReceivedPriNotReceivedSecTwentyFivePercentCoverage_False() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			const int coveragePercent=25;
			const double ucrFee=50;
			string procStr="D0120";
			string planType="p";//PPO percentage
			double feeSchedFee=40;
			//Expected that estimates will not be recalculated for Received ClaimProc.  However, secondary should calculate, factoring in estimates from primary.
			double estimatedSecBaseEst=10;
			double estimatedSecInsEstTotal=10;
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
			#region Primary InsPlan Setup
			InsuranceT.AddInsurance(pat,suffix,planType,feeSchedNum,ordinal:1);
			List<InsSub> listSubs=InsSubT.GetInsSubs(pat);
			List<InsPlan> listInsPlans=InsPlans.RefreshForSubList(listSubs);
			List<PatPlan> listPatPlans=PatPlans.Refresh(pat.PatNum);
			InsPlan priPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Primary,listPatPlans,listInsPlans,listSubs);
			InsSub priSub=InsSubT.GetSubForPriSecMed(PriSecMed.Primary,listPatPlans,listInsPlans,listSubs);
			if(planType!="f" && planType!="c") {//Only add a 50 percent coverage to the plan for the example proc if not Medicaid/Flat Copay or capitation
				BenefitT.CreatePercentForProc(priPlan.PlanNum,procCode.CodeNum,coveragePercent);
			}
			#endregion
			#region Secondary InsPlan Setup
			InsuranceT.AddInsurance(pat,suffix,planType,feeSchedNum,ordinal:2);
			listSubs=InsSubT.GetInsSubs(pat);
			listInsPlans=InsPlans.RefreshForSubList(listSubs);
			listPatPlans=PatPlans.Refresh(pat.PatNum);
			InsPlan secPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Secondary,listPatPlans,listInsPlans,listSubs);
			InsSub secSub=InsSubT.GetSubForPriSecMed(PriSecMed.Secondary,listPatPlans,listInsPlans,listSubs);
			if(planType!="f" && planType!="c") {//Only add a 50 percent coverage to the plan for the example proc if not Medicaid/Flat Copay or capitation
				BenefitT.CreatePercentForProc(secPlan.PlanNum,procCode.CodeNum,coveragePercent);
			}
			#endregion
			PatPlan priPatPlan=listPatPlans.FirstOrDefault(x => x.InsSubNum==priSub.InsSubNum);
			double procFee=Procedures.GetProcFee(pat,listPatPlans,listSubs,listInsPlans,procCode.CodeNum,pat.PriProv,0,"");
			Procedure proc=ProcedureT.CreateProcedure(pat,procStr,ProcStat.C,"",procFee);
			ClaimProc priClaimProc=ClaimProcT.CreateClaimProc(pat.PatNum,proc.ProcNum,priPlan.PlanNum,priSub.InsSubNum,DateTime.Today,-1,-1,-1,ClaimProcStatus.Estimate);
			ClaimProc secClaimProc=ClaimProcT.CreateClaimProc(pat.PatNum,proc.ProcNum,secPlan.PlanNum,secSub.InsSubNum,DateTime.Today,-1,-1,-1,ClaimProcStatus.Estimate);
			List<Benefit> listBens=Benefits.Refresh(listPatPlans,listSubs);
			List<SubstitutionLink> listSubLinks=SubstitutionLinks.GetAllForPlans(listInsPlans);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<ClaimProc> listClaimProcs=new List<ClaimProc>() { priClaimProc, secClaimProc };
			PrefT.UpdateBool(PrefName.InsEstRecalcReceived,false);//Set InsEstRecalcReceived preference to false.
			Procedures.ComputeEstimates(proc,pat.PatNum,listClaimProcs,true,listInsPlans,listPatPlans,listBens,pat.Age,listSubs);
			priClaimProc.Status=ClaimProcStatus.Received;//Set the primary claimproc received.
			priClaimProc.InsPayAmt=feeSchedFee*(coveragePercent/100d);
			priClaimProc.WriteOff=(ucrFee-feeSchedFee);//(ucr-schedfee)
			Procedures.ComputeEstimates(proc,pat.PatNum,listClaimProcs,false,listInsPlans,listPatPlans,listBens,pat.Age,listSubs);
			Assert.AreEqual(estimatedSecBaseEst,secClaimProc.BaseEst);
			Assert.AreEqual(estimatedSecInsEstTotal,secClaimProc.InsEstTotal);
		}

		///<summary>Primary claim is received, and since Pref.InsEstRecalcReceived is true, its ClaimProc estimates should be recalculated.  
		///Additionally, the secondary estimates should still recalcuate since they are not received.  Secondary claimprocs must factor in the paid 
		///estimates from the primary claim.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_InsEstRecalcReceivedPriNotReceivedSec_True() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			const int coveragePercent=100;
			const double ucrFee=50;
			string procStr="D0120";
			string planType="p";//PPO percentage
			double feeSchedFee=40;
			//Expected that estimates will not be recalculated for Received ClaimProc.  However, secondary should calculate, factoring in estimates from primary.
			double estimatedSecBaseEst=0;
			double estimatedSecInsEstTotal=0;
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
			#region Primary InsPlan Setup
			InsuranceT.AddInsurance(pat,suffix,planType,feeSchedNum,ordinal:1);
			List<InsSub> listSubs=InsSubT.GetInsSubs(pat);
			List<InsPlan> listInsPlans=InsPlans.RefreshForSubList(listSubs);
			List<PatPlan> listPatPlans=PatPlans.Refresh(pat.PatNum);
			InsPlan priPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Primary,listPatPlans,listInsPlans,listSubs);
			InsSub priSub=InsSubT.GetSubForPriSecMed(PriSecMed.Primary,listPatPlans,listInsPlans,listSubs);
			if(planType!="f" && planType!="c") {//Only add a 50 percent coverage to the plan for the example proc if not Medicaid/Flat Copay or capitation
				BenefitT.CreatePercentForProc(priPlan.PlanNum,procCode.CodeNum,coveragePercent);
			}
			#endregion
			#region Secondary InsPlan Setup
			InsuranceT.AddInsurance(pat,suffix,planType,feeSchedNum,ordinal:2);
			listSubs=InsSubT.GetInsSubs(pat);
			listInsPlans=InsPlans.RefreshForSubList(listSubs);
			listPatPlans=PatPlans.Refresh(pat.PatNum);
			InsPlan secPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Secondary,listPatPlans,listInsPlans,listSubs);
			InsSub secSub=InsSubT.GetSubForPriSecMed(PriSecMed.Secondary,listPatPlans,listInsPlans,listSubs);
			if(planType!="f" && planType!="c") {//Only add a 50 percent coverage to the plan for the example proc if not Medicaid/Flat Copay or capitation
				BenefitT.CreatePercentForProc(secPlan.PlanNum,procCode.CodeNum,coveragePercent);
			}
			#endregion
			PatPlan priPatPlan=listPatPlans.FirstOrDefault(x => x.InsSubNum==priSub.InsSubNum);
			double procFee=Procedures.GetProcFee(pat,listPatPlans,listSubs,listInsPlans,procCode.CodeNum,pat.PriProv,0,"");
			Procedure proc=ProcedureT.CreateProcedure(pat,procStr,ProcStat.C,"",procFee);
			ClaimProc priClaimProc=ClaimProcT.CreateClaimProc(pat.PatNum,proc.ProcNum,priPlan.PlanNum,priSub.InsSubNum,DateTime.Today,-1,-1,-1,ClaimProcStatus.Estimate);
			ClaimProc secClaimProc=ClaimProcT.CreateClaimProc(pat.PatNum,proc.ProcNum,secPlan.PlanNum,secSub.InsSubNum,DateTime.Today,-1,-1,-1,ClaimProcStatus.Estimate);
			List<Benefit> listBens=Benefits.Refresh(listPatPlans,listSubs);
			List<SubstitutionLink> listSubLinks=SubstitutionLinks.GetAllForPlans(listInsPlans);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<ClaimProc> listClaimProcs=new List<ClaimProc>() { priClaimProc, secClaimProc };
			PrefT.UpdateBool(PrefName.InsEstRecalcReceived,true);//Set InsEstRecalcReceived preference to true.
			Procedures.ComputeEstimates(proc,pat.PatNum,listClaimProcs,true,listInsPlans,listPatPlans,listBens,pat.Age,listSubs);

			priClaimProc.Status=ClaimProcStatus.Received;//Set the primary claimproc received.
			priClaimProc.InsPayAmt=feeSchedFee*(coveragePercent/100d);
			priClaimProc.WriteOff=(ucrFee-feeSchedFee);//(ucr-schedfee)
			Procedures.ComputeEstimates(proc,pat.PatNum,listClaimProcs,false,listInsPlans,listPatPlans,listBens,pat.Age,listSubs);
			Assert.AreEqual(estimatedSecBaseEst,secClaimProc.BaseEst);
			Assert.AreEqual(estimatedSecInsEstTotal,secClaimProc.InsEstTotal);
		}
	}

}

