using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;
using CodeBase;

namespace UnitTests {
	[TestClass]
	/// <summary>Test class for the Fees and FeeCache classes.</summary>
	public class FeesTests:FeeTestBase {

		[ClassInitialize]
		public static void SetupClass(TestContext testContext) {
			FeeTestSetup();
		}

		#region S-Class Tests

		/// <summary>Create a single fee and get the exact match from the cache.</summary>
		[TestMethod]
		public void Fees_GetFee_Exact() {
			Fee expectedFee=CreateSingleFee(MethodBase.GetCurrentMethod().Name,_defaultFeeAmt*_rand.NextDouble(),true,true);
			Fee actualFee=Fees.GetFee(expectedFee.CodeNum,expectedFee.FeeSched,expectedFee.ClinicNum,expectedFee.ProvNum);
			Assert.IsTrue(AreSimilar(expectedFee,actualFee));
		}

		/// <summary>Create a single fee, search the wrong code num, the exact match returns null.</summary>
		[TestMethod]
		public void Fees_GetFee_ExactNotFound() {
			Fee createdFee=CreateSingleFee(MethodBase.GetCurrentMethod().Name,_defaultFeeAmt*_rand.NextDouble(),true,true);
			Assert.IsNull(Fees.GetFee(_listProcCodes.Last().CodeNum,createdFee.FeeSched,createdFee.ClinicNum,createdFee.ProvNum));
		}

		/// <summary></summary>
		[TestMethod]
		public void Fees_GetFee_PartialProvDefaultClinic() {
			Fee expectedFee=CreateSingleFee(MethodBase.GetCurrentMethod().Name,_defaultFeeAmt*_rand.NextDouble(),false,true);
			Clinic clinic=ClinicT.CreateClinic(MethodBase.GetCurrentMethod().Name);
			Fee actualFee=Fees.GetFee(expectedFee.CodeNum,expectedFee.FeeSched,clinic.ClinicNum,expectedFee.ProvNum);
			Assert.IsTrue(AreSimilar(expectedFee,actualFee));
		}

		[TestMethod]
		public void Fees_GetFee_PartialClinicNoProv() {
			Fee expectedFee=CreateSingleFee(MethodBase.GetCurrentMethod().Name,_defaultFeeAmt*_rand.NextDouble(),true,false);
			long provNum=ProviderT.CreateProvider(MethodBase.GetCurrentMethod().Name);
			Fee actualFee=Fees.GetFee(expectedFee.CodeNum,expectedFee.FeeSched,expectedFee.ClinicNum,provNum);
			Assert.IsTrue(AreSimilar(expectedFee,actualFee));
		}

		[TestMethod]
		public void Fees_GetFee_PartialDefaultForCode() {
			string name=MethodBase.GetCurrentMethod().Name;
			Fee expectedFee=CreateSingleFee(name,_defaultFeeAmt*_rand.NextDouble());
			Clinic clinic=ClinicT.CreateClinic(name);
			long provNum=ProviderT.CreateProvider(name);
			Fee actualFee=Fees.GetFee(expectedFee.CodeNum,expectedFee.FeeSched,clinic.ClinicNum,provNum);
			Assert.IsTrue(AreSimilar(expectedFee,actualFee));
		}

		[TestMethod]
		public void Fees_GetFee_PartialNotFound() {
			string name=MethodBase.GetCurrentMethod().Name;
			Fee createdFee=CreateSingleFee(name,_defaultFeeAmt*_rand.NextDouble());
			Clinic clinic=ClinicT.CreateClinic(name);
			long provNum=ProviderT.CreateProvider(name);
			Assert.IsNull(Fees.GetFee(_listProcCodes.Last().CodeNum,createdFee.FeeSched,clinic.ClinicNum,provNum));
		}

		/// <summary>Create a single fee. Test that searching for the wrong fee returns -1 and searching for the correct fee returns the amount.</summary>
		[TestMethod]
		public void Fees_GetAmount() {
			double amt=_defaultFeeAmt * _rand.NextDouble();
			Fee fee=CreateSingleFee(MethodBase.GetCurrentMethod().Name,amt,true,true);
			Assert.AreEqual(-1,Fees.GetAmount(_listProcCodes.Last().CodeNum,fee.FeeSched,fee.ClinicNum,fee.ProvNum));
			Assert.AreEqual(fee.Amount,Fees.GetAmount(fee.CodeNum,fee.FeeSched,fee.ClinicNum,fee.ProvNum),_precision);
		}

		/// <summary>Create a single fee. Test that searching for the wrong fee returns 0 and searching for the correct fee returns the amount.</summary>
		[TestMethod]
		public void Fees_GetAmount0() {
			double amt=_defaultFeeAmt * _rand.NextDouble();
			Fee fee=CreateSingleFee(MethodBase.GetCurrentMethod().Name,amt,true,true);
			Assert.AreEqual(0,Fees.GetAmount0(_listProcCodes.Last().CodeNum,fee.FeeSched,fee.ClinicNum,fee.ProvNum));
			Assert.AreEqual(fee.Amount,Fees.GetAmount0(fee.CodeNum,fee.FeeSched,fee.ClinicNum,fee.ProvNum),_precision);
		}

		///<summary>Fill the cache with the fees from the HQ clinic and the currently selected Clinic.</summary>
		[TestMethod]
		public void Fees_FillCache() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			FeeTestArgs args=CreateManyFees(1,1,1,MethodBase.GetCurrentMethod().Name);
			Clinics.ClinicNum=args.ListClinics.First().ClinicNum;
			List<Fee> listExpectedFees=args.ListFees.OrderBy(x => x.FeeNum).ToList();
			Fees.FillCache();
			FeeCache cacheCopy=Fees.GetCache();
			//It is possible another test created a fee with a different fee schednum and clinicNum=0. The fee cache *should* load these if they exist
			//but for the purpose of the test it is simpler to just exclude them here.
			List<Fee> listCachedFees=cacheCopy.Where(x => !(x.ClinicNum==0 && x.FeeSched!=_standardFeeSchedNum)).OrderBy(x => x.FeeNum).ToList();
			Assert.IsTrue(AreListsSimilar(listExpectedFees,listCachedFees));
		}

        ///<summary>Gets fees for a bunch of fee schedules over Middle Tier.</summary>
        [TestMethod]
        public void Fees_GetByFeeSchedNumsClinicNums_MiddleTier()
        {
            List<long> listFeeSchedNums = new List<long>();
            long codeNum1 = ProcedureCodes.GetCodeNum("D1110");
            long codeNum2 = ProcedureCodes.GetCodeNum("D1206");
            for (int i = 0; i < 300; i++)
            {
                FeeSched feeSched = FeeSchedT.GetNewFeeSched(FeeScheduleType.Normal, "FS" + i);
                FeeT.GetNewFee(feeSched.FeeSchedNum, codeNum1, 11, hasCacheRefresh: false);
                FeeT.GetNewFee(feeSched.FeeSchedNum, codeNum2, 13, hasCacheRefresh: false);
                listFeeSchedNums.Add(feeSched.FeeSchedNum);
            }

            List<FeeLim> listFees = Fees.GetByFeeSchedNumsClinicNums(listFeeSchedNums, new List<long> { 0 });
            Assert.AreEqual(600, listFees.Count);
        }

		#endregion S-Class Tests

		#region FeeCache Tests

		/// <summary>Test the FeeCache initializes with the expected standard fees.</summary>
		[TestMethod]
		public void FeeCache_Initialize() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			FeeTestArgs args=CreateManyFees(1,1,1,MethodBase.GetCurrentMethod().Name);
			Clinics.ClinicNum=args.ListClinics.First().ClinicNum;
			FeeCache cache=new FeeCache();
			//If we are running multiple tests, it is possible that some additional fees with clinic 0 were created, and these would be loaded into the cache,
			//which is the correct behavior. To check this is right, first check that all of the expected fees are there
			List<Fee> listExpected=args.ListFees.OrderBy(x => x.FeeNum).ToList();
			List<Fee> listActual=cache.Where(x => x.FeeSched==_standardFeeSchedNum || args.ListFeeSchedNums.Contains(x.FeeSched)).OrderBy(x => x.FeeNum).ToList();
			Assert.IsTrue(AreListsSimilar(listExpected,listActual));
			//Second, we check to make sure that no additional clinics, beyond HQ and the current clinic were loaded into the cache.
			Assert.IsNull(cache.Where(x => x.ClinicNum!=0 && x.ClinicNum!=Clinics.ClinicNum).FirstOrDefault());
		}

		/// <summary>Creates a number of fees, loads the fees into the cache, invalidates a fee schedule and makes a copy. Checks that the invalidated 
		/// fee schedule is refreshed and loaded into the copy.</summary>
		[TestMethod]
		public void FeeCache_GetCopy() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			FeeTestArgs args=CreateManyFees(5,3,2,MethodBase.GetCurrentMethod().Name);
			FeeCache cache=new FeeCache();
			cache.GetFeesForClinics(args.ListClinics.Select(x => x.ClinicNum)); //load these clinics into memory
			long invalidatedFeeSched = args.ListFeeSchedNums.Last();
			Fee fee=args.ListFees.First(x => x.FeeSched==invalidatedFeeSched);
			//Before we invalidate the fee sched, make sure it's actually in our cache
			Assert.AreEqual(fee.Amount,cache.GetFee(fee.CodeNum,fee.FeeSched,fee.ClinicNum,fee.ProvNum).Amount); 
			cache.Invalidate(invalidatedFeeSched);
			FeeCache copiedCache = (FeeCache)cache.GetCopy();
			Assert.AreEqual(fee.Amount,copiedCache.GetFee(fee.CodeNum,fee.FeeSched,fee.ClinicNum,fee.ProvNum).Amount);
		}

		///<summary>Create a single fee and add to the local cache only.</summary>
		[TestMethod]
		public void FeeCache_AddFee() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			FeeCache cache=new FeeCache();
			long feeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,MethodBase.GetCurrentMethod().Name);
			Fee fee=new Fee()
			{
				FeeSched=feeSchedNum,
				ClinicNum=0,
				ProvNum=0,
				CodeNum=_listProcCodes[_rand.Next(_listProcCodes.Count-1)].CodeNum,
				Amount=_defaultFeeAmt
			};
			cache.Add(fee);
			Assert.IsNotNull(cache.GetFee(fee.CodeNum,fee.FeeSched,fee.ClinicNum,fee.ProvNum,true));
		}

		///<summary>Fill the cache, then get a fee from a clinic. Use to load test the time it takes to fill the cache with a new clinic.</summary>
		[TestMethod]
		public void FeeCache_AddClinic() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			FeeTestArgs args=CreateManyFees(1,1,1,MethodBase.GetCurrentMethod().Name);
			FeeCache cache=new FeeCache();
			List<Fee> listExpectedFees=args.ListFees.Where(x => x.ClinicNum==args.ListClinics.Select(y => y.ClinicNum).Last()).OrderBy(x => x.FeeNum).ToList();
			List<Fee> listActualFees=cache.GetFeesForClinic(args.ListClinics.Select(y => y.ClinicNum).Last()).OrderBy(x => x.FeeNum).ToList();
			Assert.IsTrue(AreListsSimilar(listExpectedFees,listActualFees));
		}

		///<summary>Update a single fee in the local cache only.</summary>
		[TestMethod]
		public void FeeCache_UpdateFee() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			Fee fee=CreateSingleFee(MethodBase.GetCurrentMethod().Name);
			fee.Amount=75;
			FeeCache cache=new FeeCache();
			cache.Update(fee);
			Assert.AreEqual(fee.Amount,cache.GetAmount0(fee.CodeNum,fee.FeeSched,fee.ClinicNum,fee.ProvNum));
			Fees.Delete(fee);
		}

		///<summary>Remove a single fee from the local cache only.</summary>
		[TestMethod]
		public void FeeCache_RemoveFee() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			Fee fee=CreateSingleFee(MethodBase.GetCurrentMethod().Name);
			FeeCache cache=new FeeCache();
			Assert.IsTrue(cache.Remove(fee));
			Fees.Delete(fee);
		}

		///<summary>Creates several fees and randomly performs 100 inserts,updates,and deletes and checks they are saved correctly.
		///Implicitly tests Fees.UpdateFromCache.</summary>
		[TestMethod]
		public void FeeCache_Transaction() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			FeeTestArgs args=CreateManyFees(2,6,2,MethodBase.GetCurrentMethod().Name);
			List<Tuple<Fee,long>> listChanges=new List<Tuple<Fee, long>>();
			List<Fee> listFeesToUse=args.ListFees.OrderBy(x => _rand.Next()).Take(100).ToList();
			FeeCache cache=new FeeCache();
			cache.BeginTransaction();
			foreach(Fee fee in listFeesToUse) {
				int flip=_rand.Next(2);
				Fee newFee=null;
				switch(flip) {
					case 0:
						newFee=new Fee()
						{
							FeeSched=args.EmptyFeeSchedNum,
							Amount=_defaultFeeAmt,
							CodeNum=_listProcCodes[_rand.Next(_listProcCodes.Count-1)].CodeNum,
							ClinicNum=args.ListClinics[_rand.Next(args.ListClinics.Count-1)].ClinicNum,
						};
						args.ListFees.Add(newFee);
						cache.Add(newFee);
						break;
					case 1:						
						fee.Amount=fee.Amount * _rand.NextDouble();
						cache.Update(fee);
						break;
					case 2:
						cache.Remove(fee);
						args.ListFees.Remove(fee);
						break;
				}
				listChanges.Add(Tuple.Create<Fee,long>(newFee??fee,flip));
			}
			cache.SaveToDb();
			foreach(Tuple<Fee,long> change in listChanges) {
				Fee fee=change.Item1;
				switch(change.Item2) {
					case 0:
						Assert.IsNotNull(cache.GetFee(fee.CodeNum,fee.FeeSched,fee.ClinicNum,fee.ProvNum,true));
						break;
					case 1:
						Assert.AreEqual(fee.Amount,cache.GetAmount0(fee.CodeNum,fee.FeeSched,fee.ClinicNum,fee.ProvNum),0.01);
						break;
					case 2:
						Assert.IsNull(cache.GetFee(fee.CodeNum,fee.FeeSched,fee.ClinicNum,fee.ProvNum,true));
						break;
				}
			}
		}

		///<summary>Cycle through getting clinic fees from the local cache. This should be used to confirm the queueClinicNums is working. 
		///It is not intended for load testing, or it may take a very long time.</summary>
		[TestMethod]
		public void FeeCache_CycleClinics() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			FeeCache cache=new FeeCache();//Fees.GetCache();
			List<Fee> listFees=CreateManyFees(2,7,2,MethodBase.GetCurrentMethod().Name).ListFees;
			//Only check a random subset of elements in the created list in order to prevent this test from taking a long time
			List<Fee> listFeesToCheck=listFees.OrderBy(x => _rand.Next()).Take(500).ToList();
			foreach(Fee fee in listFeesToCheck) {
				Assert.IsNotNull(cache.GetFee(fee.CodeNum,fee.FeeSched,fee.ClinicNum,fee.ProvNum,true));
			}
		}

		/// <summary>Creates three fee schedules. Invalidate two fee schedules, confirm that getting the fees still returns the proper amount.</summary>
		[TestMethod]
		public void Fees_InvalidateFeeSchedules() {
			PrefT.UpdateBool(PrefName.FeesUseCache,true);
			FeeTestArgs args=CreateManyFees(3,1,1,MethodBase.GetCurrentMethod().Name);
			FeeCache cache=new FeeCache(args.ListFees);
			Fee fee1=args.ListFees.First();
			Fee fee2=args.ListFees.First(x => x.FeeSched!=fee1.FeeSched);
			Fee fee3=args.ListFees.First(x => x.FeeSched!=fee2.FeeSched);
			cache.Invalidate(fee1.FeeSched);
			cache.Invalidate(fee2.FeeSched);
			Assert.AreEqual(fee1.Amount,cache.GetFee(fee1.CodeNum,fee1.FeeSched,fee1.ClinicNum,fee1.ProvNum).Amount);
			Assert.AreEqual(fee2.Amount,cache.GetFee(fee2.CodeNum,fee2.FeeSched,fee2.ClinicNum,fee2.ProvNum).Amount);
			Assert.AreEqual(fee3.Amount,cache.GetFee(fee3.CodeNum,fee3.FeeSched,fee3.ClinicNum,fee3.ProvNum).Amount);
		}

		#endregion FeeCache Tests

		#region FeeNoCache Tests

		///<summary>Create a single fee and get the exact match from the database.</summary>
		[TestMethod]
		public void FeeNoCache_GetFee_Exact() {
			FeeNoCache feeNoCache=new FeeNoCache();
			Fee expectedFee=CreateSingleFee(MethodBase.GetCurrentMethod().Name,_defaultFeeAmt*_rand.NextDouble(),true,true);
			Fee actualFee=feeNoCache.GetFee(expectedFee.CodeNum,expectedFee.FeeSched,expectedFee.ClinicNum,expectedFee.ProvNum);
			Assert.IsTrue(AreSimilar(expectedFee,actualFee));
		}

		///<summary>Create a single fee, search the wrong code num, the exact match returns null.</summary>
		[TestMethod]
		public void FeeNoCache_GetFee_ExactNotFound() {
			FeeNoCache feeNoCache=new FeeNoCache();
			Fee createdFee=CreateSingleFee(MethodBase.GetCurrentMethod().Name,_defaultFeeAmt*_rand.NextDouble(),true,true);
			Fee actualFee=feeNoCache.GetFee(_listProcCodes.Last().CodeNum,createdFee.FeeSched,createdFee.ClinicNum,createdFee.ProvNum);
			Assert.IsNull(actualFee);
		}

		///<summary>Searches for a fee with a clinic but accepts the fee for the default clinic.</summary>
		[TestMethod]
		public void FeeNoCache_GetFee_PartialProvDefaultClinic() {
			FeeNoCache feeNoCache=new FeeNoCache();
			Fee expectedFee=CreateSingleFee(MethodBase.GetCurrentMethod().Name,_defaultFeeAmt*_rand.NextDouble(),false,true);
			Clinic clinic=ClinicT.CreateClinic(MethodBase.GetCurrentMethod().Name);
			Fee actualFee=feeNoCache.GetFee(expectedFee.CodeNum,expectedFee.FeeSched,clinic.ClinicNum,expectedFee.ProvNum);
			Assert.IsTrue(AreSimilar(expectedFee,actualFee));
		}

		///<summary>Searches for a fee for a provider but accepts the fee for the current clinic and no provider.</summary>
		[TestMethod]
		public void FeeNoCache_GetFee_PartialClinicNoProv() {
			FeeNoCache feeNoCache=new FeeNoCache();
			Fee expectedFee=CreateSingleFee(MethodBase.GetCurrentMethod().Name,_defaultFeeAmt*_rand.NextDouble(),true,false);
			long provNum=ProviderT.CreateProvider(MethodBase.GetCurrentMethod().Name);
			Fee actualFee=feeNoCache.GetFee(expectedFee.CodeNum,expectedFee.FeeSched,expectedFee.ClinicNum,provNum);
			Assert.IsTrue(AreSimilar(expectedFee,actualFee));
		}

		///<summary>Searches for a fee for a provider and clinic but accepts the fee for the default clinic and no provider.</summary>
		[TestMethod]
		public void FeeNoCache_GetFee_PartialDefaultForCode() {
			FeeNoCache feeNoCache=new FeeNoCache();
			string name=MethodBase.GetCurrentMethod().Name;
			Fee expectedFee=CreateSingleFee(name,_defaultFeeAmt*_rand.NextDouble(),isGlobalFeeSched: true);
			Clinic clinic=ClinicT.CreateClinic(name);
			long provNum=ProviderT.CreateProvider(name);
			Fee actualFee=feeNoCache.GetFee(expectedFee.CodeNum,expectedFee.FeeSched,clinic.ClinicNum,provNum);
			Assert.IsTrue(AreSimilar(expectedFee,actualFee));
		}

		///<summary>Searches for a fee where the code num does not exist.</summary>
		[TestMethod]
		public void FeeNoCache_GetFee_PartialNotFound() {
			FeeNoCache feeNoCache=new FeeNoCache();
			string name=MethodBase.GetCurrentMethod().Name;
			Fee createdFee=CreateSingleFee(name,_defaultFeeAmt*_rand.NextDouble());
			Clinic clinic=ClinicT.CreateClinic(name);
			long provNum=ProviderT.CreateProvider(name);
			Assert.IsNull(feeNoCache.GetFee(_listProcCodes.Last().CodeNum,createdFee.FeeSched,clinic.ClinicNum,provNum));
		}

		///<summary>Searches for a fee where there are multiple potential matches.</summary>
		[TestMethod]
		public void FeeNoCache_GetFee_ManyPossibleMatches() {
			FeeNoCache feeNoCache=new FeeNoCache();
			string name=MethodBase.GetCurrentMethod().Name;
			Fee expectedFee=CreateSingleFee(name,_defaultFeeAmt*_rand.NextDouble(),true,true);
			Fee otherFee1=CreateSingleFee(name,_defaultFeeAmt*_rand.NextDouble(),true,false,expectedFee.FeeSched,expectedFee.CodeNum,expectedFee.ClinicNum);
			Fee otherFee2=CreateSingleFee(name,_defaultFeeAmt*_rand.NextDouble(),false,true,expectedFee.FeeSched,expectedFee.CodeNum,0,expectedFee.ProvNum);
			Fee otherFee3=CreateSingleFee(name,_defaultFeeAmt*_rand.NextDouble(),false,false,expectedFee.FeeSched,expectedFee.CodeNum);
			Fee actualFee=feeNoCache.GetFee(expectedFee.CodeNum,expectedFee.FeeSched,expectedFee.ClinicNum,expectedFee.ProvNum);
			Assert.IsTrue(AreSimilar(expectedFee,actualFee));
		}

		///<summary>Searches for a fee for a provider and clinic but accepts the fee for the default clinic and no provider due to the fee schedule being
		///a global fee schedule.</summary>
		[TestMethod]
		public void FeeNoCache_GetFee_GlobalFeeSched() {
			FeeNoCache feeNoCache=new FeeNoCache();
			string name=MethodBase.GetCurrentMethod().Name;
			Fee expectedFee=CreateSingleFee(name,_defaultFeeAmt*_rand.NextDouble(),false,false,isGlobalFeeSched: true);
			Fee otherFee1=CreateSingleFee(name,_defaultFeeAmt*_rand.NextDouble(),true,false,expectedFee.FeeSched,expectedFee.CodeNum);
			Fee otherFee2=CreateSingleFee(name,_defaultFeeAmt*_rand.NextDouble(),false,true,expectedFee.FeeSched,expectedFee.CodeNum);
			Fee otherFee3=CreateSingleFee(name,_defaultFeeAmt*_rand.NextDouble(),true,true,expectedFee.FeeSched,expectedFee.CodeNum);
			Fee actualFee=feeNoCache.GetFee(expectedFee.CodeNum,expectedFee.FeeSched,otherFee1.ClinicNum,otherFee2.ProvNum);
			Assert.IsTrue(AreSimilar(expectedFee,actualFee));
		}

		#endregion FeeNoCache Tests
	}
}
