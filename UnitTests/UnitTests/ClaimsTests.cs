using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnitTestsCore;

namespace UnitTests {
	[TestClass]
	public class ClaimsTests:TestBase {
		
		///<summary>This test is for making sure that writeoffs are correct even when given a preauth estimate before a claim estimate.</summary>
		[TestMethod]
		public void Claims_CalculateAndUpdate_PreauthOrderWriteoff() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			//create the patient and insurance information
			Patient pat=PatientT.CreatePatient(suffix);
			//proc - Crown
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2750",ProcStat.C,"8",1000);
			long feeSchedNum1=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix);
			FeeT.CreateFee(feeSchedNum1,proc.CodeNum,900);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan insPlan=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum1);
			BenefitT.CreateAnnualMax(insPlan.PlanNum,1000);
			BenefitT.CreateCategoryPercent(insPlan.PlanNum,EbenefitCategory.Crowns,100);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,insPlan.PlanNum);
			PatPlan pp=PatPlanT.CreatePatPlan(1,pat.PatNum,sub.InsSubNum);
			//create lists and variables required for ComputeEstimates()
			List<InsSub> SubList=InsSubs.RefreshForFam(Patients.GetFamily(pat.PatNum));
			List<InsPlan> listInsPlan=InsPlans.RefreshForSubList(SubList);
			List<PatPlan> listPatPlan=PatPlans.Refresh(pat.PatNum);
			List<Benefit> listBenefits=Benefits.Refresh(listPatPlan,SubList);
			List<Procedure> listProcsForPat=Procedures.Refresh(pat.PatNum);
			List<Procedure> procsForClaim=new List<Procedure>();
			procsForClaim.Add(proc);
			//Create the claim and associated claimprocs
			//The order of these claimprocs is the whole point of the unit test.
			//Create Preauth
			ClaimProcs.CreateEst(new ClaimProc(),proc,insPlan,sub,0,500,true,true);
			//Create Estimate 
			ClaimProcs.CreateEst(new ClaimProc(),proc,insPlan,sub,1000,1000,true,false);
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			Claim claimWaiting=ClaimT.CreateClaim("W",listPatPlan,listInsPlan,listClaimProcs,listProcsForPat,pat,procsForClaim,listBenefits,SubList,false);
			Assert.AreEqual(100,claimWaiting.WriteOff,"WriteOff Amount");
		}

		///<summary></summary>
		[TestMethod]
		public void Claims_ValidatePOBoxAddress() {
			List<string> listValidPOBox=new List<string> {
				"PO Box 12345",
				"P O box 12345",
				"P. O. Box 12345",
				"P.O.Box 12345",
				"post box 12345",
				"post office box 12345",
				"P.O.B 12345",
				"P.O.B. 12345",
				"Post Box #12345",
				"Postal Box 12345",
				"P.O. Box 12345",
				"PO. Box 12345",
				"P.o box 12345",
				"Pobox 12345",
				"p.o. Box12345",
				"po-box12345",
				"p.o.-box 12345",
				"PO-Box 12345",
				"p-o-box 12345",
				"p-o box 12345",
				"box 12345",
				"Box12345",
				"Box-12345"
			};
			List<string> listInValidPOBox=new List<string> {
				"12345 Tapo Cannon Rd.",
				"12345 Tapo 1st Ave",
				"12345 Box Turtle Circle",
				"12345 Boxing Pass",
				"12345 Poblano Lane",
				"12345 P O Davis Drive",
				"12345 P O Boxing Drive",
				"12345 PO Boxing Drive",
				"12345 Postal Circle"
			};
			foreach(string address in listValidPOBox) {
				if(!X837_5010.HasPOBox(address)) {
					Assert.Fail(address);
				}
			}
			foreach(string address in listInValidPOBox) {
				if(X837_5010.HasPOBox(address)) {
					Assert.Fail(address);
				}
			}
		}
	}
}
