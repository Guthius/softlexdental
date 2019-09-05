using System.Data;

namespace OpenDentBusiness
{
    /// <summary>
    /// This interfaces represents the functionality of a cache that stores fees.
    /// </summary>
    public interface IFeeCache
    {
        /// <summary>
        /// Initializes the cache.
        /// </summary>
        void Initialize();
        
        /// <summary>
        /// Gets the fee that matches the given parameters. If <paramref name="exactMatch"/> is 
        /// false, then the fee returned might have a clinic id or provider id of 0.
        /// </summary>
        Fee GetFee(long codeId, long feeSchedId, long clinicId = 0, long providerId = 0, bool exactMatch = false);
       
        /// <summary>
        /// Invalidates the fees stored for the given fee schedule.
        /// </summary>
        void Invalidate(long feeSchedId);
        
        /// <summary>
        /// Returns a copy of itself.
        /// </summary>
        IFeeCache GetCopy();

        /// <summary>
        /// Fills the cache with the passed in datatable.
        /// </summary>
        void FillCacheFromTable(DataTable dataTable);

        /// <summary>
        /// Gets the fees in the cache in the form of a DataTable.
        /// </summary>
        DataTable GetTableFromCache(bool refreshCache);
    }
}
