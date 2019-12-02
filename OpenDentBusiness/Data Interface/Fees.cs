using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using CodeBase;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Fees {
		///<summary>When the fee cache is going to be filled for the first time by a thread, 
		///make everyone wait until _cache has been filled the first time.</summary>
		public static bool IsFilledByThread=false;
		///<summary>Access _Cache instead. This is a unique cache class used for caching and manipulating fees.</summary>
		private static IFeeCache _cache;

		///<summary>This is a very unique cache class. Not generally available for use, instead either get a copy of the cache for
		///local use or use some of the functions in the S class.</summary>
		private static IFeeCache _Cache {
			get	{
				FillCacheOrWait();
				return _cache;
			}
			set	{
				_cache=value;
			}
		}

		///<summary>If the cache has not been filled, waits for the cache to fill if it is being filled by another thread, 
		///otherwise fills the cache itself.</summary>
		private static void FillCacheOrWait() {
			//No need to check RemotingRole; no call to db.
			if(!Preference.GetBool(PreferenceName.FeesUseCache)){
				return;
			}
			if(_cache!=null) {
				return;
			}
			if(IsFilledByThread) {
				//The fee cache is special in the fact that we fill it for the first time within a thread that was spawned via FormOpenDental.
				//All other threads that want a copy of the fee cache need to sit here and wait until the thread has filled it for the first time.
				int loopcount=0;
				while(_cache==null) {
					loopcount++;
					if(loopcount>6000) {//~a minute, plus the time it takes to run this small while loop 6000 times.
						throw new Exception("Unable to fill fee cache.");
					}
					Thread.Sleep(10);
				}
			}
			else {//Fill the fee cache on the first time that the fee cache is being requested (old logic).
						//This was too slow for larger offices so we had to introduce IsFilledByThread so that this cache can be filled behind the scenes.
				FillCache();
			}
		}

		///<summary>Initializes the Cache, with fees for the HQ Clinic, and for the current user's selected clinic.</summary>
		public static void FillCache() {
			//No need to check RemotingRole; no call to db.
			if(!Preference.GetBool(PreferenceName.FeesUseCache)){
				return;
			}
			IFeeCache cache=new FeeCache();
			
			_Cache=cache;
		}

		///<summary>Fills the cache with the passed in DataTable. Note that this might push out fees from other clinics from the cache.</summary>
		public static void FillCacheFromTable(DataTable dataTable) {
			if(!Preference.GetBool(PreferenceName.FeesUseCache)){
				return;
			}
			_Cache.FillCacheFromTable(dataTable);
		}

		#region Get Methods
		///<summary>Gets the list of fees by clinic num from the db.</summary>
		public static List<Fee> GetByClinicNum(long clinicNum) {
			return GetByClinicNums(new List<long>() { clinicNum });
		}

		///<summary>Gets the list of fees by clinic nums from the db.</summary>
		public static List<Fee> GetByClinicNums(List<long> listClinicNums) {
			if(listClinicNums.Count==0) {
				return new List<Fee>();
			}
			string command="SELECT * FROM fee WHERE ClinicNum IN ("+string.Join(",",listClinicNums.Select(x => POut.Long(x)))+")";
			return Crud.FeeCrud.SelectMany(command);
		}
		
		///<summary>Gets the list of fees by feeschednums and clinicnums from the db.  Returns an empty list if listFeeSchedNums is null or empty.
		///Throws an application exception if listClinicNums is null or empty.  Always provide at least one ClinicNum.
		///We throw instead of returning an empty list which would make it look like there are no fees for the fee schedules passed in.
		///If this method returns an empty list it is because no valied fee schedules were given or the database truly doesn't have any fees.</summary>
		public static List<FeeLim> GetByFeeSchedNumsClinicNums(List<long> listFeeSchedNums,List<long> listClinicNums) {
			if(listFeeSchedNums==null || listFeeSchedNums.Count==0) {
				return new List<FeeLim>();//This won't hurt the FeeCache because there will be no corresponding fee schedules to "blank out".
			}
			if(listClinicNums==null || listClinicNums.Count==0) {
				//Returning an empty list here would be detrimental to the FeeCache.
				throw new ApplicationException("Invalid listClinicNums passed into GetByFeeSchedNumsClinicNums()");
			}
			string command="SELECT FeeNum,Amount,FeeSched,CodeNum,ClinicNum,ProvNum,SecDateTEdit FROM fee "
				+"WHERE FeeSched IN ("+string.Join(",",listFeeSchedNums.Select(x => POut.Long(x)))+") "
				+"AND ClinicNum IN ("+string.Join(",",listClinicNums.Select(x => POut.Long(x)))+")";
			return Db.GetTable(command).AsEnumerable()
				.Select(x => new FeeLim {
					FeeNum=PIn.Long(x["FeeNum"].ToString()),
					Amount=PIn.Double(x["Amount"].ToString()),
					FeeSched=PIn.Long(x["FeeSched"].ToString()),
					CodeNum=PIn.Long(x["CodeNum"].ToString()),
					ClinicNum=PIn.Long(x["ClinicNum"].ToString()),
					ProvNum=PIn.Long(x["ProvNum"].ToString()),
					SecDateTEdit=PIn.DateT(x["SecDateTEdit"].ToString()),
				}).ToList();
		}

		///<summary>Counts the number of fees in the db for this fee sched, including all clinic and prov overrides.</summary>
		public static int GetCountByFeeSchedNum(long feeSchedNum) {
			string command="SELECT COUNT(*) FROM fee WHERE FeeSched ="+POut.Long(feeSchedNum);
			return PIn.Int(Db.GetCount(command));
		}

		///<summary>Searches for the given codeNum and feeSchedNum and finds the most appropriate match for the clinicNum and provNum.  If listFees is null, it will go to db.</summary>
		public static Fee GetFee(long codeNum,long feeSchedNum,long clinicNum,long provNum,List<Fee> listFees=null) {
			//No need to check RemotingRole; no call to db.
			//use listFees if supplied regardless of the FeesUseCache pref since the fee cache is not really thread safe
			if(listFees!=null) {
				return GetFeeFromList(listFees,codeNum,feeSchedNum,clinicNum,provNum);
			}
			if(Preference.GetBool(PreferenceName.FeesUseCache)){
				return _Cache.GetFee(codeNum,feeSchedNum,clinicNum,provNum);
			}
			return GetFeeNoCache(codeNum,feeSchedNum,clinicNum,provNum);
		}

		///<summary>Searches the db for a fee with the exact codeNum, feeSchedNum, clinicNum, and provNum provided.  Returns null if no exact match found.
		///The goal of this method is to have a way to check the database for "duplicate" fees before adding more fees to the db. Set doGetExactMatch to
		///true to exactly match all passed in parameters.</summary>
		public static Fee GetFeeNoCache(long codeNum,long feeSchedNum,long clinicNum=0,long provNum=0,bool doGetExactMatch=false) {
			if(FeeScheds.IsGlobal(feeSchedNum) && !doGetExactMatch) {
				clinicNum=0;
				provNum=0;
			}
			//If the logic changes here, then we need to change FeeCache.GetFee.
			string command=
				//Search for exact match first.  This would include a clinic and provider override.
				@"SELECT fee.*
				FROM fee
				WHERE fee.CodeNum="+POut.Long(codeNum)+@"
				AND fee.FeeSched="+POut.Long(feeSchedNum)+@"
				AND fee.ClinicNum="+POut.Long(clinicNum)+@"
				AND fee.ProvNum="+POut.Long(provNum);
			if(!doGetExactMatch) {
				//Provider override 
				command+=@"
					UNION ALL
					SELECT fee.*
					FROM fee
					WHERE fee.CodeNum="+POut.Long(codeNum)+@"
					AND fee.FeeSched="+POut.Long(feeSchedNum)+@"
					AND fee.ClinicNum=0
					AND fee.ProvNum="+POut.Long(provNum);
				//Clinic override
				command+=@"
					UNION ALL
					SELECT fee.*
					FROM fee
					WHERE fee.CodeNum="+POut.Long(codeNum)+@"
					AND fee.FeeSched="+POut.Long(feeSchedNum)+@"
					AND fee.ClinicNum="+POut.Long(clinicNum)+@"
					AND fee.ProvNum=0";
				//Unassigned clinic with no override
				command+=@"
					UNION ALL
					SELECT fee.*
					FROM fee
					WHERE fee.CodeNum="+POut.Long(codeNum)+@"
					AND fee.FeeSched="+POut.Long(feeSchedNum)+@"
					AND fee.ClinicNum=0
					AND fee.ProvNum=0";
			}
			return Crud.FeeCrud.SelectOne(command);
		}

		///<summary>Same logic as above, in Fees.GetFeeNoCache().  Also, same logic as in FeeCache.GetFee().
		///Typical to pass in a list of fees for just one or a few feescheds so that the search goes quickly.
		///When doGetExactMatch is true, this will return either the fee that matches the parameters exactly, or null if no such fee exists.
		///When doGetExactMatch is false, and the fee schedule is global, we ignore the clinicNum and provNum and return the HQ fee that matches the given codeNum and feeSchedNum.
		///When doGetExactMatch is false, and the fee schedule is not global, and no exact match exists we attempt to return the closest matching fee in this order:
		///1 - The fee with the same codeNum, feeSchedNum, and providerNum, with a clinicNum of 0
		///2 - The fee with the same codeNum, feeSchedNum, and clinicNum, with a providerNum of 0
		///3 - The fee with the same codeNum, feeSchedNum, and both a clinicNum and providerNum of 0
		///If no partial match can be found, return null.</summary>
		private static Fee GetFeeFromList(List<Fee> listFees,long codeNum,long feeSched=0,long clinicNum=0,long provNum=0,bool doGetExactMatch=false){
			//No need to check RemotingRole; no call to db.
			if(FeeScheds.IsGlobal(feeSched) && !doGetExactMatch) {//speed things up here with less loops
				clinicNum=0;
				provNum=0;
			}
			Fee fee=listFees.Find(f => f.CodeNum==codeNum && f.FeeSched==feeSched && f.ClinicNum==clinicNum && f.ProvNum==provNum);
			if(fee!=null){
				return fee;//match found.  Would include a clinic and provider override.
			}
			if(doGetExactMatch || FeeScheds.IsGlobal(feeSched)) {
				return null;//couldn't find exact match
			}
			//no exact match exists, so we look for closest match
			//2: Prov override
			fee=listFees.Find(f => f.CodeNum==codeNum && f.FeeSched==feeSched && f.ClinicNum==0 && f.ProvNum==provNum);
			if(fee!=null){
				return fee;
			}
			//3: Clinic override
			fee=listFees.Find(f => f.CodeNum==codeNum && f.FeeSched==feeSched && f.ClinicNum==clinicNum && f.ProvNum==0);
			if(fee!=null){
				return fee;
			}
			//4: Just unassigned clinic default
			fee=listFees.Find(f => f.CodeNum==codeNum && f.FeeSched==feeSched && f.ClinicNum==0 && f.ProvNum==0);
			//whether it's null or not:
			return fee;
		}

		///<summary>Gets fees for up to three feesched/clinic/prov combos. If filtering with a ClinicNum and/or ProvNum, it only includes fees that match that clinicNum/provnum or have zero.  This reduces the result set if there are clinic or provider overrides. This could easily scale to many thousands of clinics and providers.</summary>
		public static List<Fee> GetListForScheds(long feeSched1,long clinic1=0,long prov1=0,long feeSched2=0,long clinic2=0,long prov2=0,long feeSched3=0,long clinic3=0,long prov3=0){
			string command="SELECT * FROM fee WHERE "
				+"(FeeSched="+POut.Long(feeSched1)+" AND (ClinicNum=0 OR ClinicNum="+POut.Long(clinic1)+") AND (ProvNum=0 OR ProvNum="+POut.Long(prov1)+"))";
			if(feeSched2!=0){
				command+=" OR (FeeSched="+POut.Long(feeSched2)+" AND (ClinicNum=0 OR ClinicNum="+POut.Long(clinic2)+") AND (ProvNum=0 OR ProvNum="+POut.Long(prov2)+"))";
			}
			if(feeSched3!=0){
				command+=" OR (FeeSched="+POut.Long(feeSched3)+" AND (ClinicNum=0 OR ClinicNum="+POut.Long(clinic3)+") AND (ProvNum=0 OR ProvNum="+POut.Long(prov3)+"))";
			}
			return Crud.FeeCrud.SelectMany(command);
		}

		///<summary>Gets all possible fees associated with the various objects passed in.  Gets fees from db based on code and fee schedule combos.  Includes all provider overrides.  Includes default/no clinic as well as any specified clinic overrides. Although the list always includes extra fees from scheds that we don't need, it's still a very small list.  That list is then used repeatedly by other code in loops to find the actual individual fee amounts.</summary>
		public static List<Fee> GetListFromObjects(List<ProcedureCode> listProcedureCodes,List<string> listMedicalCodes,List<long> listProvNumsTreat,long patPriProv,
			long patSecProv,long patFeeSched,List<InsPlan> listInsPlans,List<long> listClinicNums,List<Appointment> listAppts,
			List<SubstitutionLink> listSubstLinks,long discountPlan
			//listCodeNums,listProvNumsTreat,listProcCodesProvNumDefault,patPriProv,patSecProv,patFeeSched,listInsPlans,listClinicNums
			//List<long> listProcCodesProvNumDefault
			)
		{
			//listMedicalCodes: it already automatically gets the medical codes from procCodes.  This is just for procs. If no procs yet, it will be null.
			//listMedicalCodes can be done by: listProcedures.Select(x=>x.MedicalCode).ToList();  //this is just the strings
			//One way to get listProvNumsTreat is listProcedures.Select(x=>x.ProvNum).ToList()
			//One way to specify a single provNum in listProvNumsTreat is new List<long>(){provNum}
			//One way to get clinicNums is listProcedures.Select(x=>x.ClinicNum).ToList()
			//Another way to get clinicNums is new List<long>(){clinicNum}.
			//These objects will be cleaned up, so they can have duplicates, zeros, invalid keys, nulls, etc
			//In some cases, we need to pass in a list of appointments to make sure we've included all possible providers, both ProvNum and ProvHyg
			//In that case, it's common to leave listProvNumsTreat null because we clearly do not have any of those providers set yet.
			if(listProcedureCodes==null){
				return new List<Fee>();
			}
			List<long> listCodeNumsOut=new List<long>();
			foreach(ProcedureCode procedureCode in listProcedureCodes){
				if(procedureCode==null){
					continue;
				}
				if(!listCodeNumsOut.Contains(procedureCode.CodeNum)){
					listCodeNumsOut.Add(procedureCode.CodeNum);
				}
				if(ProcedureCodes.IsValidCode(procedureCode.MedicalCode)){
					long codeNumMed=ProcedureCodes.GetCodeNum(procedureCode.MedicalCode);
					if(!listCodeNumsOut.Contains(codeNumMed)){
						listCodeNumsOut.Add(codeNumMed);
					}
				}
				if(ProcedureCodes.IsValidCode(procedureCode.SubstitutionCode)) {
					long codeNumSub=ProcedureCodes.GetCodeNum(procedureCode.SubstitutionCode);
					if(!listCodeNumsOut.Contains(codeNumSub)) {
						listCodeNumsOut.Add(codeNumSub);
					}
				}
			}
			if(listMedicalCodes!=null){
				foreach(string strMedCode in listMedicalCodes){
					if(ProcedureCodes.IsValidCode(strMedCode)){
						long codeNumMed=ProcedureCodes.GetCodeNum(strMedCode);
						if(!listCodeNumsOut.Contains(codeNumMed)){
							listCodeNumsOut.Add(codeNumMed);
						}
					}
				}
			}
			foreach(SubstitutionLink substitutionLink in listSubstLinks){//Grab all subst codes, since we don't know which ones we will need.
				if(ProcedureCodes.IsValidCode(substitutionLink.SubstitutionCode)){
					long codeNum=ProcedureCodes.GetCodeNum(substitutionLink.SubstitutionCode);
					if(!listCodeNumsOut.Contains(codeNum)){
						listCodeNumsOut.Add(codeNum);
					}
				}
			}
			//Fee schedules. Will potentially include many.=======================================================================================
			List<long> listFeeScheds=new List<long>();
			//Add feesched for first provider (See Claims.CalculateAndUpdate)---------------------------------------------------------------------
			Provider provFirst= Provider.GetDefault();
			if(provFirst!=null && provFirst.FeeScheduleId!=0 && !listFeeScheds.Contains(provFirst.FeeScheduleId)){
				listFeeScheds.Add(provFirst.FeeScheduleId);
			}
			//Add feesched for PracticeDefaultProv------------------------------------------------------------------------------------------------
			Provider provPracticeDefault= Provider.GetById(Preference.GetLong(PreferenceName.PracticeDefaultProv));
			if(provPracticeDefault!=null && provPracticeDefault.FeeScheduleId!=0 && !listFeeScheds.Contains(provPracticeDefault.FeeScheduleId)){
				listFeeScheds.Add(provPracticeDefault.FeeScheduleId);
			}
			//Add feescheds for all treating providers---------------------------------------------------------------------------------------------
			if(listProvNumsTreat!=null){
				foreach(long provNumTreat in listProvNumsTreat){
					Provider provTreat= Provider.GetById(provNumTreat);
					if(provTreat!=null && provTreat.FeeScheduleId!=0 && !listFeeScheds.Contains(provTreat.FeeScheduleId)){
						listFeeScheds.Add(provTreat.FeeScheduleId);//treating provs fee scheds
					}
				}
			}
			//Add feescheds for the patient's primary and secondary providers----------------------------------------------------------------------
			Provider providerPatPri= Provider.GetById(patPriProv);
			if(providerPatPri!=null && providerPatPri.FeeScheduleId!=0 && !listFeeScheds.Contains(providerPatPri.FeeScheduleId)){
				listFeeScheds.Add(providerPatPri.FeeScheduleId);
			}
			Provider providerPatSec= Provider.GetById(patSecProv);
			if(providerPatSec!=null && providerPatSec.FeeScheduleId!=0 && !listFeeScheds.Contains(providerPatSec.FeeScheduleId)){
				listFeeScheds.Add(providerPatSec.FeeScheduleId);
			}
			//Add feescheds for all procedurecode.ProvNumDefaults---------------------------------------------------------------------------------
			foreach(ProcedureCode procedureCode in listProcedureCodes){
				if(procedureCode==null){
					continue;
				}
				long provNumDefault=procedureCode.ProvNumDefault;
				if(provNumDefault==0){
					continue;
				}
				Provider provDefault= Provider.GetById(provNumDefault);
				if(provDefault!=null && provDefault.FeeScheduleId!=0 && !listFeeScheds.Contains(provDefault.FeeScheduleId)){
					listFeeScheds.Add(provDefault.FeeScheduleId);
				}
			}
			//Add feescheds for appointment providers---------------------------------------------------------------------------------------------
			if(listAppts!=null){
				foreach(Appointment appointment in listAppts){
					Provider provAppt= Provider.GetById(appointment.ProvNum);
					if(provAppt!=null && provAppt.FeeScheduleId!=0 && !listFeeScheds.Contains(provAppt.FeeScheduleId)){
						listFeeScheds.Add(provAppt.FeeScheduleId);
					}
					Provider provApptHyg= Provider.GetById(appointment.ProvHyg);
					if(provApptHyg!=null && provApptHyg.FeeScheduleId!=0 && !listFeeScheds.Contains(provApptHyg.FeeScheduleId)){
						listFeeScheds.Add(provApptHyg.FeeScheduleId);
					}
				}
			}
			//Add feesched for patient.  Rare. --------------------------------------------------------------------------------------------------
			if(patFeeSched!=0){
				if(!listFeeScheds.Contains(patFeeSched)){
					listFeeScheds.Add(patFeeSched);
				}
			}
			//Add feesched for each insplan, both reg and allowed--------------------------------------------------------------------------------
			if(listInsPlans!=null){
				foreach(InsPlan insPlan in listInsPlans){
					if(insPlan.FeeSched!=0 && !listFeeScheds.Contains(insPlan.FeeSched)){
						listFeeScheds.Add(insPlan.FeeSched);//insplan feeSched
					}
					if(insPlan.AllowedFeeSched!=0 && !listFeeScheds.Contains(insPlan.AllowedFeeSched)){
						listFeeScheds.Add(insPlan.AllowedFeeSched);//allowed feeSched
					}
					if(insPlan.CopayFeeSched!=0 && !listFeeScheds.Contains(insPlan.CopayFeeSched)) {
						listFeeScheds.Add(insPlan.CopayFeeSched);//copay feeSched
					}
				}
			}
			if(discountPlan!=0) {
				long discountPlanFeeSched=DiscountPlans.GetPlan(discountPlan).FeeSchedNum;
				if(!listFeeScheds.Contains(discountPlanFeeSched)) {
					listFeeScheds.Add(discountPlanFeeSched);
				}
			}
			//ClinicNums========================================================================================================================
			List<long> listClinicNumsOut=new List<long>();//usually empty or one entry
			if(listClinicNums!=null){
				foreach(long clinicNum in listClinicNums){
					if(clinicNum!=0 && !listClinicNumsOut.Contains(clinicNum)){
						listClinicNumsOut.Add(clinicNum);//proc ClinicNums
					}
				}
			}
			if(listFeeScheds.Count==0 || listProcedureCodes.Count==0){
				return new List<Fee>();
			}
			string command="SELECT * FROM fee WHERE (";
			for(int i=0;i<listFeeScheds.Count;i++){
				if(i>0){
					command+=" OR ";
				}
				command+="FeeSched="+POut.Long(listFeeScheds[i]);
			}
			command+=") AND (";
			for(int i=0;i<listCodeNumsOut.Count;i++){
				if(i>0){
					command+=" OR ";
				}
				command+="CodeNum="+POut.Long(listCodeNumsOut[i]);
			}
			command+=") AND (ClinicNum=0";
			for(int i=0;i<listClinicNumsOut.Count;i++){
				command+=" OR ClinicNum="+POut.Long(listClinicNumsOut[i]);
			}
			command+=")";
			return Crud.FeeCrud.SelectMany(command);
		}

		///<summary>Gets fees that exactly match criteria.</summary>
		public static List<Fee> GetListExact(long feeSched,long clinicNum,long provNum){
			string command="SELECT * FROM fee WHERE "
				+"FeeSched="+POut.Long(feeSched)+" AND ClinicNum="+POut.Long(clinicNum)+" AND ProvNum="+POut.Long(provNum)+" "
				+"GROUP BY CodeNum";//There should not be duplicates, but group by codeNum, just in case.
			return Crud.FeeCrud.SelectMany(command);
		}

		///<summary>Pass in new list and original list.  This will synch everything with Db.</summary>
		public static bool SynchList(List<Fee> listNew,List<Fee> listDB){
			return Crud.FeeCrud.Sync(listNew,listDB,0);
		}

		///<summary>Gets a copy of the cache for local use.</summary>
		public static FeeCache GetCache() {
			if(!Preference.GetBool(PreferenceName.FeesUseCache)){
				throw new Exception("No fee cache.");
			}
			//We need to coalesce to a new FeeCache in case GetCopy() doesn't return a FeeCache.
			return (_Cache.GetCopy() as FeeCache)??new FeeCache(initialize: false);
		}

		///<summary>Gets from Db.  Returns all fees associated to the procedure code passed in.</summary>
		public static List<Fee> GetFeesForCode(long codeNum,List<long> listClinicNums=null) {
			string command="SELECT * FROM fee WHERE CodeNum="+POut.Long(codeNum)+" ";
			if(listClinicNums!=null && listClinicNums.Count>0) {
				command+="AND ClinicNum IN("+string.Join(",",listClinicNums.Select(x => POut.Long(x)))+")";
			}
			if(!Preference.GetBool(PreferenceName.FeesUseCache)){
				//ordering was being done in the form. Easier to do it here.
				command+=" ORDER BY ClinicNum,ProvNum";
			}
			return Crud.FeeCrud.SelectMany(command);
		}

		///<summary>Gets fees from Db, not including any prov or clinic overrides.</summary>
		public static List<Fee> GetFeesForCodeNoOverrides(long codeNum) {
			string command="SELECT * FROM fee WHERE CodeNum="+POut.Long(codeNum)+" "
				+"AND ClinicNum=0 AND ProvNum=0";
			return Crud.FeeCrud.SelectMany(command);
		}

		///<summary>Returns an amount if a fee has been entered.  Prefers local clinic fees over HQ fees.  Otherwise returns -1.
		///Not usually used directly.  If you don't pass in a list of Fees, it will go directly to the Db.</summary>
		public static double GetAmount(long codeNum,long feeSchedNum,long clinicNum,long provNum,List<Fee> listFees=null) {
			//No need to check RemotingRole; no call to db.
			if(FeeScheds.GetIsHidden(feeSchedNum)) {
				return -1;//you cannot obtain fees for hidden fee schedules
			}
			Fee fee=GetFee(codeNum,feeSchedNum,clinicNum,provNum,listFees);
			if(fee==null) {
				return -1;
			}
			return fee.Amount;
		}

		///<summary>Almost the same as GetAmount.  But never returns -1;  Returns an amount if a fee has been entered.  
		///Prefers local clinic fees over HQ fees. Returns 0 if code can't be found.
		///If you don't pass in a list of fees, it will go directly to the database.</summary>
		public static double GetAmount0(long codeNum,long feeSched,long clinicNum=0,long provNum=0,List<Fee> listFees=null) {
			//No need to check RemotingRole; no call to db.
			double retVal=GetAmount(codeNum,feeSched,clinicNum,provNum,listFees);
			if(retVal==-1) {
				return 0;
			}
			return retVal;
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(!Preference.GetBool(PreferenceName.FeesUseCache)){
				throw new Exception("No fee cache.");
			}
			return _Cache.GetTableFromCache(doRefreshCache);
		}

		///<summary>Gets the UCR fee for the provided procedure.</summary>
		public static double GetFeeUCR(Procedure proc) {
			//No need to check RemotingRole; no call to db.
			long provNum=proc.ProvNum;
			if(provNum==0) {//if no prov set, then use practice default.
				provNum= Preference.GetLong(PreferenceName.PracticeDefaultProv);
			}
			Provider providerFirst= Provider.GetDefault();//Used in order to preserve old behavior...  If this fails, then old code would have failed.
			Provider provider= Provider.GetById(provNum)??providerFirst;
			//get the fee based on code and prov fee sched
			double ppoFee=GetAmount0(proc.CodeNum,provider.FeeScheduleId,proc.ClinicNum,provNum);
			double ucrFee=proc.ProcFee;
			if(ucrFee > ppoFee) {
				return proc.Quantity * ucrFee;
			}
			else {
				return proc.Quantity * ppoFee;
			}
		}
		#endregion Get Methods

		#region Modification Methods

		#region Insert
		///<summary></summary>
		public static long Insert(Fee fee) {
			//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
			fee.SecUserNumEntry=Security.CurrentUser.Id;
			return Crud.FeeCrud.Insert(fee);
		}

		/// <summary>Bulk Insert</summary>
		public static void InsertMany(List<Fee> listFees) {
			//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
			listFees.ForEach(x => x.SecUserNumEntry=Security.CurrentUser.Id);
			Crud.FeeCrud.InsertMany(listFees);
		}
		#endregion Insert

		#region Update
		///<summary></summary>
		public static void Update(Fee fee){
			Crud.FeeCrud.Update(fee);
		}

		///<summary>Commit changes logged during Cache.BeginTransaction.</summary>
		public static List<long> UpdateFromCache(List<FeeUpdate> listFeeUpdates) {
			if(!Preference.GetBool(PreferenceName.FeesUseCache)){
				throw new Exception("No fee cache.");
			}
			long feeNum=0;
			List<long> listFeeScheds=new List<long>();
			List<Fee> insertedFees=new List<Fee>();
			//We need to go through each fee in the order that they were added to the list in case the same fee is in the list multiple times.
			foreach(FeeUpdate update in listFeeUpdates) {
				try {
					switch(update.UpdateType) {
						case FeeUpdateType.Add:
							//Make sure that this fee doesn't already exist within the database because we do not want to create a duplicate fee.
							Fee feeDb=GetFeeNoCache(update.Fee.CodeNum,update.Fee.FeeSched,update.Fee.ClinicNum,update.Fee.ProvNum,true);
							if(feeDb!=null) {
								//This fee already exists so we need to turn this Add into an Update and set the FeeNum to the PK that we found in the db.
								update.Fee.FeeNum=feeDb.FeeNum;
								//Immediately call Update and pass in the fee from our cache, we already know that all other fields that matter match exactly.
								Update(update.Fee);
							}
							else {
								feeNum=Insert(update.Fee);
								update.Fee.FeeNum=feeNum; //We need to keep track of the feeNum in case there is a change to this fee later in the transaction
							}
							insertedFees.Add(update.Fee);
							break;
						case FeeUpdateType.Update:
							if(update.Fee.FeeNum==0) { //this is an update for a fee that was added in this same transaction
								update.Fee.FeeNum=insertedFees.Where(x => x.FeeSched==update.Fee.FeeSched && x.ClinicNum==update.Fee.ClinicNum
									&& x.CodeNum==update.Fee.CodeNum && x.ProvNum==update.Fee.ProvNum).Select(x => x.FeeNum).LastOrDefault();
							}
							feeNum=update.Fee.FeeNum;
							Update(update.Fee);
							break;
						case FeeUpdateType.Remove:
							if(update.Fee.FeeNum==0) { //this is a delete for a fee that was added in this same transaction
								update.Fee.FeeNum=insertedFees.Where(x => x.FeeSched==update.Fee.FeeSched && x.ClinicNum==update.Fee.ClinicNum
									&& x.CodeNum==update.Fee.CodeNum && x.ProvNum==update.Fee.ProvNum).Select(x => x.FeeNum).LastOrDefault();
							}
							feeNum=update.Fee.FeeNum;
							Delete(update.Fee);
							break;
						default:
							break;
					}
					if(!listFeeScheds.Contains(update.Fee.FeeSched)) {
						listFeeScheds.Add(update.Fee.FeeSched);
					}
				}
				catch (Exception e) {
					throw new Exception("An error occurred "+update.UpdateType+"ing Fee "+feeNum +": "+e.Message, e);
				}
			}
			return listFeeScheds;
		}
		#endregion Update

		#region Delete
		///<summary></summary>
		public static void Delete(Fee fee){
			//No need to check RemotingRole; no call to db.
			Delete(fee.FeeNum);
		}

		///<summary></summary>
		public static void Delete(long feeNum){
			ClearFkey(feeNum);
			string command="DELETE FROM fee WHERE FeeNum="+feeNum;
			Db.NonQ(command);
		}

		///<summary></summary>
		public static void DeleteMany(List<long> listFeeNums) {
			if(listFeeNums.Count==0) {
				return;
			}
			ClearFkey(listFeeNums);
			string command="DELETE FROM fee WHERE FeeNum IN ("+string.Join(",",listFeeNums)+")";
			Db.NonQ(command);
		}

		///<summary>Deletes all fees for the supplied FeeSched that aren't for the HQ clinic.</summary>
		public static void DeleteNonHQFeesForSched(long feeSchedNum) {
			string command="SELECT FeeNum FROM fee WHERE FeeSched="+POut.Long(feeSchedNum)+" AND ClinicNum!=0";
			List<long> listFeeNums=Db.GetListLong(command);
			DeleteMany(listFeeNums);
		}

		/// <summary>Deletes all fees with the exact specified FeeSchedule, ClinicNum, and ProvNum combination.</summary>
		public static void DeleteFees(long feeSched,long clinicNum,long provNum) {
			string command="DELETE FROM fee WHERE "
				+"FeeSched="+POut.Long(feeSched)+" AND ClinicNum="+POut.Long(clinicNum)+" AND ProvNum="+POut.Long(provNum);
			Db.NonQ(command);
		}
		#endregion Delete

		#endregion Modification Methods

		#region Misc Methods
		///<summary></summary>
		public static void InvalidateFeeSchedules(List<long> listFeeScheduleNums) {
            FillCacheOrWait();
            //Using _cache instead of _Cache because we are changing the internal dictionary.
            //if we add a preference to remove lazy loading, it would put a refreshcache call right here.
            if (Preference.GetBool(PreferenceName.FeesUseCache)){
				listFeeScheduleNums.ForEach(x => _cache.Invalidate(x));
			}
		}

		public static void InvalidateFeeSchedule(long feeScheduleNum) {
			if(Preference.GetBool(PreferenceName.FeesUseCache)){
				InvalidateFeeSchedules(new List<long> { feeScheduleNum });
			}
		}

		///<summary>Deprecated. See IncreaseNew, below. Increases the fee schedule by percent.  Round should be the number of decimal places, either 0,1,or 2.
		///This method will manipulate listFees passed in so pass in a deep copy if it should not be altered.
		///Returns listFees back after increasing the fees from the passed in fee schedule information.</summary>
		public static List<Fee> Increase(long feeSchedNum,int percent,int round,List<Fee> listFees,long clinicNum,long provNum) {
			//No need to check RemotingRole; no call to db.
			FeeSched feeSched=FeeScheds.GetFirst(x => x.FeeSchedNum==feeSchedNum);
			List<long> listCodeNums=new List<long>(); //Contains only the fee codeNums that have been increased.  Used for keeping track.
			List<Fee> listFeesNew=new List<Fee>();//Keep track of any fees that need to be added to listFees.
			foreach(Fee feeCur in listFees) {
				if(listCodeNums.Contains(feeCur.CodeNum)) {
					continue; //Skip the fee if it's associated to a procedure code that has already been increased / added.
				}
				//The best match isn't 0, and we haven't already done this CodeNum
				if(feeCur!=null && feeCur.Amount!=0) {
					double newVal=(double)feeCur.Amount*(1+(double)percent/100);
					if(round>0) {
						newVal=Math.Round(newVal,round);
					}
					else {
						newVal=Math.Round(newVal,MidpointRounding.AwayFromZero);
					}
					//The fee showing in the fee schedule is not a perfect match.  Make a new one that is.
					//E.g. We are increasing all fees for clinicNum of 1 and provNum of 5 and the best match found was for clinicNum of 3 and provNum of 7.
					//We would then need to make a copy of that fee, increase it, and then associate it to the clinicNum and provNum passed in (1 and 5).
					if(!feeSched.IsGlobal && (feeCur.ClinicNum!=clinicNum || feeCur.ProvNum!=provNum)) {
						Fee fee=new Fee();
						fee.Amount=newVal;
						fee.CodeNum=feeCur.CodeNum;
						fee.ClinicNum=clinicNum;
						fee.ProvNum=provNum;
						fee.FeeSched=feeSchedNum;
						listFeesNew.Add(fee);
					}
					else { //Just update the match found.
						feeCur.Amount=newVal;
					}
				}
				listCodeNums.Add(feeCur.CodeNum);
			}
			listFees.AddRange(listFeesNew);
			return listFees;
		}

		///<summary>The old Increase method above is deprecated but untouched.  I'm not sure it's even working correctly.  This is the replacement.
		///Increases the fees passed in by percent.  Round should be the number of decimal places, either 0,1,or 2.
		///This method will not manipulate listFees passed in, although there is no particular reason for this choice.</summary>
		public static List<Fee> IncreaseNew(long feeSchedNum,int percent,int round,List<Fee> listFees,long clinicNum,long provNum) {
			//No need to check RemotingRole; no call to db.
			FeeSched feeSched=FeeScheds.GetFirst(x => x.FeeSchedNum==feeSchedNum);
			List<Fee> listFeesRetVal=new List<Fee>();
			foreach(Fee fee in listFees) {
				if(fee.Amount==0 || fee.Amount==-1){
					listFeesRetVal.Add(fee.Copy());
					continue;
				}
				double newVal=(double)fee.Amount*(1+(double)percent/100);
				if(round>0) {
					newVal=Math.Round(newVal,round);
				}
				else {
					newVal=Math.Round(newVal,MidpointRounding.AwayFromZero);
				}
				Fee feeNew=fee.Copy();
				feeNew.Amount=newVal;
				listFeesRetVal.Add(feeNew);
			}
			return listFeesRetVal;
		}

        ///<summary>Deprecated.  Part of the old cache pattern. This method will remove and/or add a fee for the fee information passed in.
        ///codeText will typically be one valid procedure code.  E.g. D1240
        ///If an amt of -1 is passed in, then it indicates a "blank" entry which will cause deletion of any existing fee.
        ///Returns listFees back after importing the passed in fee information.
        ///Does not make any database calls.  This is left up to the user to take action on the list of fees returned.
        ///Also, makes security log entries based on how the fee changed.  Does not make a log for codes that were removed (user already warned)</summary>
        public static List<Fee> Import(string codeText, double amt, long feeSchedNum, long clinicNum, long provNum, List<Fee> listFees)
        {
            //No need to check RemotingRole; no call to db.
            if (!ProcedureCodes.IsValidCode(codeText))
            {
                return listFees;//skip for now. Possibly insert a code in a future version.
            }
            string feeOldStr = "";
            long codeNum = ProcedureCodes.GetCodeNum(codeText);
            Fee fee = listFees.FirstOrDefault(x => x.CodeNum == codeNum && x.FeeSched == feeSchedNum && x.ClinicNum == clinicNum && x.ProvNum == provNum);
            DateTime datePrevious = DateTime.MinValue;
            if (fee != null)
            {
                feeOldStr = Lans.g("FormFeeSchedTools", "Old Fee") + ": " + fee.Amount.ToString("c") + ", ";
                datePrevious = fee.SecDateTEdit;
                listFees.Remove(fee);
            }
            if (amt == -1)
            {
                return listFees;
            }
            fee = new Fee();
            fee.Amount = amt;
            fee.FeeSched = feeSchedNum;
            fee.CodeNum = ProcedureCodes.GetCodeNum(codeText);
            fee.ClinicNum = clinicNum;//Either 0 because you're importing on an HQ schedule or local clinic because the feesched is localizable.
            fee.ProvNum = provNum;
            listFees.Add(fee);//Insert new fee specific to the active clinic.
            SecurityLog.Write(null, SecurityLogEvents.ProcFeeEdit,
                "Procedure: " + codeText + ", " + feeOldStr + "New Fee: " + amt.ToString("c") + ", " +
                "Fee Schedule: " + FeeScheds.GetDescription(feeSchedNum) + ". " +
                "Fee changed using the Import button in the Fee Tools window.", ProcedureCodes.GetCodeNum(codeText),
                DateTime.MinValue);
            SecurityLog.Write(null, SecurityLogEvents.LogFeeEdit, "Fee changed", fee.FeeNum, datePrevious);
            return listFees;
        }

		///<summary>Zeros securitylog FKey column for rows that are using the matching feeNum as FKey and are related to Fee.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the Fee table type.</summary>
		public static void ClearFkey(long feeNum) {
			Crud.FeeCrud.ClearFkey(feeNum);
		}

		///<summary>Zeros securitylog FKey column for rows that are using the matching feeNums as FKey and are related to Fee.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the Fee table type.</summary>
		public static void ClearFkey(List<long> listFeeNums) {
			Crud.FeeCrud.ClearFkey(listFeeNums);
		}

		///<summary>Returns true if the user currently logged in has permissions to edit the given column.</summary>
		public static bool CanEditFee(FeeSched feeSched,long provNum,long clinicNum,out string error) {
			//User doens't have permission 
			error="";
			if(!Security.IsAuthorized(Permissions.FeeSchedEdit,out error)) {
				return false;
			}
			//Check if a provider fee schedule is selected and if the current user has permissions to edit provider fees.
			if(provNum!=0 && !Security.IsAuthorized(Permissions.ProviderFeeEdit,out error)) {
				return false;
			}
			//Make sure the user has permission to edit the clinic of the fee schedule being edited.
			if(Security.CurrentUser.ClinicRestricted && clinicNum!=Clinics.ClinicId) {
				if(clinicNum==0 && feeSched!=null && feeSched.IsGlobal) {
					//Allow restricted users to edit the default Fee when the FeeSched is global.
					//Intentionally blank so logic in more readable, will return true below.
				}
				else {
					error=Lans.g("Fee","User is clinic restricted and")+" "+feeSched.Description+" "+Lans.g("Fee","is not global.");
					return false;
				}
			}
			error="";
			return true;
		}

		///<summary>Returns true if the feeAmtNewStr is an amount that does not match fee, either because fee is null and feeAmtNewStr is not, or because
		///fee not null and the feeAmtNewStr is an equal amount, including a blank entry.</summary>
		public static bool IsFeeAmtEqual(Fee fee,string feeAmtNewStr) {
			//There is no fee in the database and the user didn't set a new fee value so there is no change.
			if(fee==null && feeAmtNewStr=="") {
				return true;
			}
			//Fee exists, but new amount is the same.
			if(fee!=null && (feeAmtNewStr!="" && fee.Amount==PIn.Double(feeAmtNewStr) || (fee.Amount==-1 && feeAmtNewStr==""))) {
				return true;
			}
			return false;
		}
		#endregion Misc Methods
	}

	///<summary>A class with a fee and an update type. 
	///Used by the FeeCache, to keep an in-memory list of pending changes for saving to the db.</summary>
	public class FeeUpdate {
		public Fee Fee {get;set;}
		/// <summary>Indicates whether the record is an Add, Update, or Delete</summary>
		public FeeUpdateType UpdateType {get;set;}

		///<summary>For serialization.</summary>
		public FeeUpdate() { }

		///<summary></summary>
		public FeeUpdate(Fee fee, FeeUpdateType updateType) {
			Fee=fee;
			UpdateType=updateType;
		}

		///<summary></summary>
		public FeeUpdate Copy() {
			return new FeeUpdate(Fee.Copy(),UpdateType);
		}
	}

	public enum FeeUpdateType {
		Add,
		Update,
		Remove
	}
}