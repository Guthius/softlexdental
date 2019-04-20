using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness {
	///<summary>Handles database commands related to the apptview table in the database.</summary>
	public class ApptViews {
		#region Get Methods
		///<summary>Optionally pass in a clinic to filter the list of appointment views returned.</summary>
		public static List<ApptView> GetForClinic(long clinicNum=0,bool isShort=true) {
			//No need to check RemotingRole; no call to db.
			if(clinicNum > 0) {
				return GetWhere(x => x.ClinicNum==clinicNum,isShort);
			}
			else {
				return GetDeepCopy(isShort);
			}
		}

		///<summary>Gets an ApptView from the cache.  If apptviewnum is not valid, then it returns null.</summary>
		public static ApptView GetApptView(long apptViewNum) {
			//No need to check RemotingRole; no call to db.
			return GetFirstOrDefault(x => x.ApptViewNum==apptViewNum);
		}
		#endregion

		#region Modification Methods

		#region Insert
		///<summary></summary>
		public static long Insert(ApptView apptView) {
			return Crud.ApptViewCrud.Insert(apptView);
		}
		#endregion

		#region Update
		///<summary></summary>
		public static void Update(ApptView apptView){
			Crud.ApptViewCrud.Update(apptView);
		}
		#endregion

		#region Delete
		///<summary></summary>
		public static void Delete(ApptView Cur){
			string command="DELETE FROM apptview WHERE ApptViewNum = '"
				+POut.Long(Cur.ApptViewNum)+"'";
			Db.NonQ(command);
		}
		#endregion

		#endregion

		#region Misc Methods
		#endregion

		#region Cache Pattern
		private class ApptViewCache : CacheListAbs<ApptView> {
			protected override List<ApptView> GetCacheFromDb() {
				string command="SELECT * FROM apptview ORDER BY ClinicNum,ItemOrder";
				return Crud.ApptViewCrud.SelectMany(command);
			}
			protected override List<ApptView> TableToList(DataTable table) {
				return Crud.ApptViewCrud.TableToList(table);
			}
			protected override ApptView Copy(ApptView apptView) {
				return apptView.Copy();
			}
			protected override DataTable ListToTable(List<ApptView> listApptViews) {
				return Crud.ApptViewCrud.ListToTable(listApptViews,"ApptView");
			}
			protected override void FillCacheIfNeeded() {
				ApptViews.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static ApptViewCache _apptViewCache=new ApptViewCache();

		public static List<ApptView> GetDeepCopy(bool isShort=false) {
			return _apptViewCache.GetDeepCopy(isShort);
		}

		private static ApptView GetFirstOrDefault(Func<ApptView,bool> match,bool isShort=false) {
			return _apptViewCache.GetFirstOrDefault(match,isShort);
		}

		private static List<ApptView> GetWhere(Predicate<ApptView> match,bool isShort=false) {
			return _apptViewCache.GetWhere(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_apptViewCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			return _apptViewCache.GetTableFromCache(doRefreshCache);
		}

		#endregion Cache Pattern
	}

}









