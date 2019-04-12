using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests {
	[TestClass]
	public class FeeSchedToolsTests:FeeTestBase {

		[ClassInitialize]
		public static void SetupClass(TestContext testContext) {
			FeeTestSetup();
		}

		///<summary>Creates a single fee for a given fee schedule, copies the feeschedule to an empty schedule and checks that the fee is the same
		///in both fee schedules.</summary>
		[TestMethod]
		public void FeeSchedTools_CopyFeeSched() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			//Create two fee schedules; from and to
			long fromSched=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,MethodBase.GetCurrentMethod().Name+"_FROM");
			long toSched=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,MethodBase.GetCurrentMethod().Name+"_TO");
			//Create a single fee and associate it to the "from" fee schedule.
			long feeCodeNum=_listProcCodes[_rand.Next(_listProcCodes.Count-1)].CodeNum;
			FeeT.CreateFee(fromSched,feeCodeNum,_defaultFeeAmt*_rand.NextDouble());
			FeeCache cache=new FeeCache();
			FeeScheds.CopyFeeSchedule(cache,FeeScheds.GetFirst(x => x.FeeSchedNum==fromSched),0,0,FeeScheds.GetFirst(x => x.FeeSchedNum==toSched),null,0);
			cache.SaveToDb();
			//Get the two fees and check that they are the same.
			Fee fromFee=Fees.GetFee(feeCodeNum,fromSched,0,0);
			Fee toFee=Fees.GetFee(feeCodeNum,toSched,0,0);
			Assert.AreEqual(fromFee.Amount,toFee.Amount);
		}

		///<summary>Mimics two instances of Open Dental being open and copying a fee schedule into the same fee schedule from each instance.
		///No matter how many instances of Open Dental invoke the same fee schedule action they should never create duplicate fees.</summary>
		[TestMethod]
		public void FeeSchedTools_CopyFeeSched_Concurrency() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			//Make sure there are no duplicate fees already present within the database.
			string dbmResult=DatabaseMaintenances.FeeDeleteDuplicates(true,DbmMode.Check);
			if(!dbmResult.Trim().EndsWith(": 0")) {
				DatabaseMaintenances.FeeDeleteDuplicates(true,DbmMode.Fix);
			}
			//Create two fee schedules; from and to
			long feeSchedNumFrom=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,MethodBase.GetCurrentMethod().Name+"_FROM");
			long feeSchedNumTo=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,MethodBase.GetCurrentMethod().Name+"_TO");
			//Create a single fee and associate it to the "from" fee schedule.
			FeeT.CreateFee(feeSchedNumFrom,_listProcCodes[_rand.Next(_listProcCodes.Count-1)].CodeNum,_defaultFeeAmt);
			//Create a helper action that will simply copy the "from" schedule into the "to" schedule for the given fee cache passed in.
			Action<FeeCache> actionCopyFromTo=new Action<FeeCache>((feeCache) => {
				FeeScheds.CopyFeeSchedule(feeCache,
					FeeScheds.GetFirst(x => x.FeeSchedNum==feeSchedNumFrom),
					0,
					0,
					FeeScheds.GetFirst(x => x.FeeSchedNum==feeSchedNumTo),
					null,
					0);
			});
			//Create two identical copies of the "fee cache" which mimics two separate instances of Open Dental running at the same time.
			FeeCache feeCache1=new FeeCache();
			FeeCache feeCache2=new FeeCache();
			//Mimic each user clicking the "Copy" button from within the Fee Tools window one right after the other (before they click OK).
			actionCopyFromTo(feeCache1);
			actionCopyFromTo(feeCache2);
			//Now act like the each instance of Open Dental just clicked the "OK" button which simply saves the cache changes to the db.
			feeCache1.SaveToDb();
			feeCache2.SaveToDb();
			//Make sure that there was NOT a duplicate fee inserted into the database.
			dbmResult=DatabaseMaintenances.FeeDeleteDuplicates(true,DbmMode.Check);
			Assert.IsTrue(dbmResult.Trim().EndsWith(": 0"),"Duplicate fees detected due to concurrent copying.");
		}

		///<summary>Users can copy one fee schedule to multiple clinics at once.  There was a problem with copying fee schedules to more than six clinics
		///at the same time due to running the copy fee schedule logic in parallel.  This unit test is designed to make sure that the bug fix of running
		///the copy fee schedule logic synchronously is preserved OR if parallel threads are reintroduced that they have fixed the bug.</summary>
		[TestMethod]
		public void FeeSchedTools_CopyFeeSched_Clinics() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			//Make sure there are no duplicate fees already present within the database.
			string dbmResult=DatabaseMaintenances.FeeDeleteDuplicates(true,DbmMode.Check);
			if(!dbmResult.Trim().EndsWith(": 0")) {
				DatabaseMaintenances.FeeDeleteDuplicates(true,DbmMode.Fix);
			}
			//Make sure that there are more than six clinics
			ClinicT.ClearClinicTable();
			for(int i=0;i<10;i++) {
				ClinicT.CreateClinic(MethodBase.GetCurrentMethod().Name+"_"+i);
			}
			//Create two fee schedules; from and to
			long feeSchedNumFrom=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,MethodBase.GetCurrentMethod().Name+"_FROM");
			long feeSchedNumTo=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,MethodBase.GetCurrentMethod().Name+"_TO");
			//Create a fee for every single procedure code in the database and associate it to the "from" fee schedule.
			foreach(ProcedureCode code in _listProcCodes) {
				FeeT.CreateFee(feeSchedNumFrom,code.CodeNum,_rand.Next(5000),hasCacheRefresh:false);
			}
			FeeCache feeCache=new FeeCache();
			//Copy the "from" fee schedule into the "to" fee schedule and do it for at least seven clinics.
			FeeScheds.CopyFeeSchedule(feeCache,
					FeeScheds.GetFirst(x => x.FeeSchedNum==feeSchedNumFrom),
					0,
					0,
					FeeScheds.GetFirst(x => x.FeeSchedNum==feeSchedNumTo),
					Clinics.GetDeepCopy(true).Select(x => x.ClinicNum).ToList(),
					0);
			//Now act like the user just clicked the "OK" button which simply saves the cache changes to the db.
			feeCache.SaveToDb();
			//Make sure that there was NOT a duplicate fee inserted into the database.
			dbmResult=DatabaseMaintenances.FeeDeleteDuplicates(true,DbmMode.Check);
			Assert.IsTrue(dbmResult.Trim().EndsWith(": 0"),"Duplicate fees detected due to concurrent copying.");
		}

		///<summary>Test importing FeeSchedule by copying from one list to another.</summary>
		[TestMethod]
		public void FeeSchedTools_Import() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			FeeTestArgs args=CreateManyFees(1,1,1,MethodBase.GetCurrentMethod().Name);
			FeeCache cache=new FeeCache();
			List<Fee> listImportedFees = cache.GetListFees(args.EmptyFeeSchedNum,0,0);
			foreach(Fee fee in cache.GetListFees(_standardFeeSchedNum,0,0)) {
				string codeText = _listProcCodes.Where(x => x.CodeNum==fee.CodeNum).Select(x => x.ProcCode).FirstOrDefault();
				listImportedFees=Fees.Import(codeText,fee.Amount,args.EmptyFeeSchedNum,fee.ClinicNum,fee.ProvNum,listImportedFees);
			}
			Fees.InsertMany(listImportedFees);
			Assert.IsTrue(DoAmountsMatch(listImportedFees,_standardFeeSchedNum,0,0));
		}

		///<summary>Creates and exports a fee schedule, then tries to import the fee schedule from the exported file then checks that the new fee 
		///schedule was imported correctly. If there are procedurecodes with an empty proccode we exclude these from the check, as we cannot look up
		///the correct code nums during the import (intended behavior). </summary>
		[TestMethod]
		public void FeeSchedTools_ImportExport() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			FeeTestArgs feeArgs=CreateManyFees(1,1,1,MethodBase.GetCurrentMethod().Name);
			long exportedSched=feeArgs.ListFeeSchedNums[0];
			long importedSched=feeArgs.EmptyFeeSchedNum;
			long clinicNum=feeArgs.ListClinics[0].ClinicNum;
			string filename=MethodBase.GetCurrentMethod().Name;
			FeeScheds.ExportFeeSchedule(new FeeCache(),exportedSched,clinicNum,feeArgs.ListProvNums[0],filename);
			OpenDental.FeeL.ImportFees(importedSched,clinicNum,feeArgs.ListProvNums[0],filename,new System.Windows.Forms.Form(),false);
			FeeCache cache=new FeeCache();
			foreach(ProcedureCode procCode in _listProcCodes.Where(x => !string.IsNullOrWhiteSpace(x.ProcCode))) { //unable to import without a proccodes
				Fee expected=cache.GetFee(procCode.CodeNum,exportedSched,clinicNum,feeArgs.ListProvNums[0]);
				Fee actual=cache.GetFee(procCode.CodeNum,importedSched,clinicNum,feeArgs.ListProvNums[0]);
				Assert.AreEqual(expected.Amount,actual.Amount);
			}
		}

		///<summary>Import canada fees from a file.</summary>
		[TestMethod]
		public void FeeSchedTools_ImportCanada() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			string canadianCodes=Properties.Resources.canadianprocedurecodes;
			//If we need to import these procedures codes, do so
			foreach(string line in canadianCodes.Split(new[] { "\r\n", "\r", "\n" },StringSplitOptions.None)) {
				string[] properties=line.Split('\t');
				if(properties.Count()!=10) {
					continue;
				}
				if(ProcedureCodes.GetCodeNum(properties[0])!=0) {
					continue;
				}
				ProcedureCode procCode=new ProcedureCode()
				{
					ProcCode=properties[0],
					Descript=properties[1],
					ProcTime=properties[8],
					AbbrDesc=properties[9],
				};
				ProcedureCodes.Insert(procCode);
			}
			//Now import the fees
			string feeData=Properties.Resources.BC_BCDA_2018_GPOOC;
			FeeSched feeSched=FeeSchedT.GetNewFeeSched(FeeScheduleType.Normal,MethodBase.GetCurrentMethod().Name,isGlobal:false);
			FeeCache feeCache=new FeeCache();
			int numImported;
			int numSkipped;
			List<Fee> listNewFees=FeeScheds.ImportCanadaFeeSchedule(feeCache,feeSched,feeData,0,0,out numImported,out numSkipped);
			feeCache.BeginTransaction();
			feeCache.RemoveFees(feeSched.FeeSchedNum,0,0);
			foreach(Fee fee in listNewFees) {
				feeCache.Add(fee);
			}
			feeCache.SaveToDb();
			Assert.IsTrue(DoAmountsMatch(listNewFees,feeSched.FeeSchedNum,0,0));
		}

		///<summary>Create a fill a fee schedule, then clear the fee schedule and make sure it is empty.</summary>
		[TestMethod]
		public void FeeSchedTools_ClearFeeSchedule() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			FeeTestArgs feeArgs=CreateManyFees(1,1,1,MethodBase.GetCurrentMethod().Name);
			long feeSchedNum=feeArgs.ListFeeSchedNums[0];
			Assert.IsTrue(Fees.GetCountByFeeSchedNum(feeSchedNum) > 0);
			FeeCache cache=new FeeCache();
			cache.BeginTransaction();
			cache.RemoveFees(feeSchedNum,feeArgs.ListClinics[0].ClinicNum,feeArgs.ListProvNums[0]);
			cache.SaveToDb();
			cache.FillCacheIfNeeded();
			Assert.AreEqual(0,Fees.GetCountByFeeSchedNum(feeSchedNum));
		}

		///<summary>Create the standard fee schedule and increase by 5% to the nearest penny.</summary>
		[TestMethod]
		public void FeeSchedTools_Increase() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			FeeTestArgs args = CreateManyFees(0,0,0,MethodBase.GetCurrentMethod().Name);
			int percent = 5;
			FeeCache cache = new FeeCache();
			List<Fee> listStandardFees = cache.GetListFees(_standardFeeSchedNum,0,0).OrderBy(x => x.FeeNum).ToList();
			List<Fee> listIncreasedFees = listStandardFees.Select(x => x.Copy()).ToList();
			listIncreasedFees=Fees.Increase(_standardFeeSchedNum,percent,2,listIncreasedFees,0,0).OrderBy(x => x.FeeNum).ToList();
			foreach(Fee fee in listIncreasedFees) {
				Fee expectedFee = cache.GetFee(fee.CodeNum,fee.FeeSched,fee.ClinicNum,fee.ProvNum);
				double amount = fee.Amount/(1+(double)percent/100);
				amount=Math.Round(amount,2);
				try {
					Assert.AreEqual(expectedFee.Amount,amount);
				}
				catch(Exception e) {
					throw new Exception("Failed for fee: " + expectedFee.FeeNum,e);
				}
			}
		}

		///<summary>There was a bug where an exception would throw when trying to increase a fee schedule for a specific fee whose best match was a fee
		///from a different clinic and provider combination.</summary>
		[TestMethod]
		public void FeeSchedTools_Increase_NonPerfectMatch() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			//Make sure that there are more than six clinics
			ClinicT.ClearClinicTable();
			int clinicCount=2;
			for(int i=0;i<clinicCount;i++) {
				ClinicT.CreateClinic(MethodBase.GetCurrentMethod().Name+"_"+i);
			}
			//Create two providers.
			int providerCount=2;
			List<long> listProvNums=new List<long>();
			for(int i=0;i<providerCount;i++) {
				listProvNums.Add(ProviderT.CreateProvider(MethodBase.GetCurrentMethod().Name+"_"+i));
			}
			//Create a fee schedule that will be used by all clinics
			long feeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,MethodBase.GetCurrentMethod().Name,isGlobal:false);
			//Create a fee for every single procedure code in the database and associate it to the "from" fee schedule.
			foreach(ProcedureCode code in _listProcCodes) {
				FeeT.CreateFee(feeSchedNum,code.CodeNum,_rand.Next(1,5000),hasCacheRefresh:false);
			}
			int feeCountBefore=Fees.GetCountByFeeSchedNum(feeSchedNum);
			FeeCache feeCache=new FeeCache();
			feeCache.BeginTransaction();
			//Begin a transaction to update all of the fees for the two providers in all of the clinics.
			List<Action> listActions=new List<Action>();
			foreach(Clinic clinic in Clinics.GetDeepCopy(true)) {
				listActions.Add(() => {
					List<Fee> listFees=feeCache.GetListFees(feeSchedNum,0,0);
					foreach(long provNum in listProvNums) {
						Fees.Increase(feeSchedNum,10,2,listFees,clinic.ClinicNum,provNum);
					}
					feeCache.RemoveFees(feeSchedNum,clinic.ClinicNum,0);
					foreach(long provNum in listProvNums) {
						feeCache.RemoveFees(feeSchedNum,clinic.ClinicNum,provNum);
					}
					foreach(Fee fee in listFees) {
						feeCache.Add(fee);
					}
				});
			}
			//FeeCache can't handle parallel threads ATM but when it can the following line can be manipulated in order to increase unit test speed.
			ODThread.RunParallel(listActions,TimeSpan.FromMinutes(30),numThreads:1);
			feeCache.SaveToDb();
			int feeCountAfter=Fees.GetCountByFeeSchedNum(feeSchedNum);
			//Assert that both the Dent and the Hyg providers had new fee schedules created for every single clinic.
			int expectedCount=feeCountBefore + (feeCountBefore * clinicCount * providerCount);
			Assert.AreEqual(expectedCount,feeCountAfter);
		}

		///<summary>Attach some fees to procedures, change half the fees and call Global Update Fees.</summary>
		[TestMethod]
		public void FeeSchedTools_GlobalUpdateFees() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			Prefs.UpdateBool(PrefName.MedicalFeeUsedForNewProcs,false);
			Prefs.RefreshCache();
			string suffix=MethodBase.GetCurrentMethod().Name;
			string procStr="D0120";
			string procStr2="D0145";
			double procFee=100;
			ProcedureCode procCode=ProcedureCodes.GetProcCode(procStr);
			ProcedureCode procCode2=ProcedureCodes.GetProcCode(procStr2);
			//Set up clinic, prov, pat
			Clinic clinic=ClinicT.CreateClinic(suffix);
			long feeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix,false);
			long provNum=ProviderT.CreateProvider(suffix,feeSchedNum:feeSchedNum);
			Fee fee=FeeT.GetNewFee(feeSchedNum,procCode.CodeNum,procFee,clinic.ClinicNum,provNum);
			Fee fee2=FeeT.GetNewFee(feeSchedNum,procCode2.CodeNum,procFee,clinic.ClinicNum,provNum);
			Patient pat=PatientT.CreatePatient(suffix,provNum,clinic.ClinicNum);
			//Chart a procedure for this proccode/pat as well as a different proccode
			Procedure proc=ProcedureT.CreateProcedure(pat,procStr,ProcStat.TP,"",fee.Amount);
			Procedure proc2=ProcedureT.CreateProcedure(pat,procStr2,ProcStat.TP,"",fee2.Amount);
			//Update the fee amount for only the D0120 code
			fee.Amount=50;
			Fees.Update(fee);
			//Now run global update fees
			long numUpdated=Procedures.GlobalUpdateFees(new FeeCache().GetFeesForClinics(new List<long>() { clinic.ClinicNum }),clinic.ClinicNum,clinic.Abbr);
			//Make sure we have the same number of updated fees, and fee amounts for both procs
			//Assert.AreEqual(1,numUpdated);
			proc=Procedures.GetOneProc(proc.ProcNum,false);
			proc2=Procedures.GetOneProc(proc2.ProcNum,false);
			Assert.AreEqual(fee.Amount,proc.ProcFee);
			Assert.AreEqual(fee2.Amount,proc2.ProcFee);
		}
		
		///<summary>Create a single procedure, and call GlobalUpdateWriteoffs.</summary>
		[TestMethod]
		public void FeeSchedTools_GlobalUpdateWriteoffEstimates() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			string suffix=MethodBase.GetCurrentMethod().Name;
			string procStr="D0145";
			double procFee=100;
			ProcedureCode procCode=ProcedureCodes.GetProcCode(procStr);
			//Set up clinic, prov, pat
			Clinic clinic=ClinicT.CreateClinic(suffix);
			long feeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.FixedBenefit,suffix);
			long provNum=ProviderT.CreateProvider(suffix,feeSchedNum:feeSchedNum);
			Fee fee=FeeT.GetNewFee(feeSchedNum,procCode.CodeNum,procFee,clinic.ClinicNum,provNum);
			Patient pat=PatientT.CreatePatient(suffix,provNum,clinic.ClinicNum);
			//Set up insurance
			InsuranceInfo info=InsuranceT.AddInsurance(pat,suffix,"c",feeSchedNum);
			List<InsSub> listSubs=info.ListInsSubs;
			List<InsPlan> listPlans=info.ListInsPlans;
			List<PatPlan> listPatPlans=info.ListPatPlans;
			InsPlan priPlan=info.PrimaryInsPlan;
			InsSub priSub=info.PrimaryInsSub;
			info.ListBenefits.Add(BenefitT.CreatePercentForProc(priPlan.PlanNum,procCode.CodeNum,90));
			//Create the procedure and claimproc
			Procedure proc=ProcedureT.CreateProcedure(pat,procStr,ProcStat.TP,"",procFee);
			ClaimProc priClaimProc=ClaimProcT.CreateClaimProc(pat.PatNum,proc.ProcNum,priPlan.PlanNum,priSub.InsSubNum,DateTime.Today,-1,-1,-1,ClaimProcStatus.CapEstimate);
			//ClaimT.CreateClaim(new List<Procedure> { proc },info);
			Procedures.ComputeEstimates(proc,pat.PatNum,new List<ClaimProc>(),true,listPlans,listPatPlans,info.ListBenefits,pat.Age,info.ListInsSubs);
			priClaimProc=ClaimProcs.Refresh(pat.PatNum).FirstOrDefault(x => x.ProcNum==proc.ProcNum);
			Assert.AreEqual(procFee,priClaimProc.WriteOff);
			procFee=50;
			Procedure procNew=proc.Copy();
			procNew.ProcFee=procFee;
			Procedures.Update(procNew,proc);
			//GlobalUpdate
			List<Clinic> listWriteoffClinics = new List<Clinic>() { clinic };
			ODProgressExtended progress=new ODProgressExtended(ODEventType.FeeSched,new FeeSchedEvent(),new System.Windows.Forms.Form(),
				tag:new ProgressBarHelper(Lans.g(this,"Write-off Update Progress"),progressBarEventType: ProgBarEventType.Header),
				cancelButtonText:Lans.g(this,"Close"));
			progress.Fire(ODEventType.FeeSched,new ProgressBarHelper("","0%"
						,0,100,ProgBarStyle.Blocks,"WriteoffProgress"));
			long updated=FeeScheds.GlobalUpdateWriteoffs(listWriteoffClinics,progress);
			Assert.AreEqual(1,updated);
			ClaimProc priClaimProcDb=ClaimProcs.Refresh(pat.PatNum).FirstOrDefault(x => x.ClaimProcNum==priClaimProc.ClaimProcNum);
			Assert.AreEqual(procFee,priClaimProcDb.WriteOff);
		}

	}
}
