using System.Data;

namespace OpenDentBusiness
{
    public class FeeNoCache : IFeeCache
    {
        public void Initialize()
        {
        }

        public Fee GetFee(long codeId, long feeSchedNum, long clinicId = 0, long providerId = 0, bool getExactMatch = false) => 
            Fees.GetFeeNoCache(codeId, feeSchedNum, clinicId, providerId, getExactMatch);
        
        public IFeeCache GetCopy() => this;

        public void FillCacheFromTable(DataTable dataTable)
        {
        }

        public DataTable GetTableFromCache(bool refreshCache) => new DataTable();

        public void Invalidate(long feeSchedNum)
        {
        }
    }
}
