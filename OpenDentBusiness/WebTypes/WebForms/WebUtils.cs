using WebServiceSerializer;

namespace OpenDentBusiness.WebTypes.WebForms
{
    public class WebUtils
    {
        /// <summary></summary>
        /// <param name="regKey"></param>
        /// <returns></returns>
        public static long GetDentalOfficeID(string regKey = null)
        {
            if (string.IsNullOrEmpty(regKey))
            {
                regKey = Preference.GetString(PreferenceName.RegistrationKey);
            }
            try
            {
                string payload = PayloadHelper.CreatePayloadWebHostSynch(regKey, new PayloadItem(regKey, "RegKey"));
                return WebSerializer.DeserializeTag<long>(SheetsSynchProxy.GetWebServiceInstance().GetDentalOfficeID(payload), "Success");
            }
            catch
            {

            }
            return 0;
        }

        public static string GetSheetDefAddress(string regKey = null)
        {
            if (string.IsNullOrEmpty(regKey))
            {
                regKey = Preference.GetString(PreferenceName.RegistrationKey);
            }

            try
            {
                string payload = PayloadHelper.CreatePayloadWebHostSynch(regKey, new PayloadItem(regKey, "RegKey"));

                return WebSerializer.DeserializeTag<string>(SheetsSynchProxy.GetWebServiceInstance().GetSheetDefAddress(payload), "Success");
            }
            catch
            {
            }

            return "";
        }
    }
}