namespace OpenDentBusiness
{
    public class SheetsSynchProxy
    {
        ///<summary>Used when we would like to override the service timeout.
        ///Will reset itself back to the default value after service instance is finished.</summary>
        public static int TimeoutOverride { get; set; } = 100000;

        ///<summary>Used when we would like to override the service URL.
        ///Will reset itself back to the default value after service instance is finished.</summary>
        public static string UrlOverride { get; set; } = "";

        public static ISheetsSynch MockSheetSynchService
        {
            private get;//Use GetWebServiceInstance()
            set;
        }

        public static ISheetsSynch GetWebServiceInstance()
        {
            if (MockSheetSynchService != null)
            {
                return MockSheetSynchService;
            }
            SheetsSynchReal service = new SheetsSynchReal();
            service.Timeout = 100000;
            if (TimeoutOverride != service.Timeout)
            {
                service.Timeout = TimeoutOverride;
                TimeoutOverride = 100000;
            }
            if (string.IsNullOrEmpty(UrlOverride))
            {
                service.Url = Preference.GetString(PreferenceName.WebHostSynchServerURL);
            }
            else
            {
                service.Url = UrlOverride;
                UrlOverride = "";
            }
#if DEBUG
            //service.Url="http://localhost:2923/SheetsSynch.asmx";
#endif
            return service;
        }
    }
}