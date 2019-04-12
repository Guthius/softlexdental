using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness {
	public class FeeNoCache:IFeeCache {

		///<summary>Does nothing.</summary>
		public void Initialize() {
			//No need to initialize anything
		}

		///<summary>Gets the fee directly from the database every time.</summary>
		public Fee GetFee(long codeNum,long feeSchedNum,long clinicNum=0,long provNum=0,bool doGetExactMatch=false) {
			return Fees.GetFeeNoCache(codeNum,feeSchedNum,clinicNum,provNum,doGetExactMatch);	
		}

		///<summary>Returns a reference to itself. This class stores no state, so there is no need to make a deep copy.</summary>
		public IFeeCache GetCopy() {
			return this;
		}

		///<summary>Does nothing.</summary>
		public void FillCacheFromTable(DataTable table) {
			//No need to fill anything
		}

		///<summary>Returns an empty DataTable.</summary>
		public DataTable GetTableFromCache(bool doRefreshCache) {
			return new DataTable();
		}

		///<summary>Does nothing.</summary>
		public void Invalidate(long feeSchedNum) {
			//No need to invalidate anything since we're not caching anything
		}
	}
}
