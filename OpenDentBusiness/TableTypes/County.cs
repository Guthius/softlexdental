namespace OpenDentBusiness
{
    public class County
    {
        public long CountyNum;
        public string CountyName;
        public string CountyCode;

        /// <summary>Not a database field. This is the unaltered CountyName. Used for Update.</summary>
        public string OldCountyName;
    }
}