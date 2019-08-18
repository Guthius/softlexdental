using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace OpenDentBusiness
{
    public class CustomerUpdatesProxy
    {
        ///<summary>Get an instance of OpenDentBusiness.localhost.Service1 (referred to as 'Customer Updates Web Service'.
        ///Also sets IWebProxy and ICredentials if specified for this customer.  Service1 is ready to use on return.</summary>
        public static localhost.Service1 GetWebServiceInstance()
        {
            localhost.Service1 ws = new localhost.Service1();//Points to the debug localhost instance by default.
#if !DEBUG
			ws.Url=PrefC.GetString(PrefName.UpdateServerAddress);
			ws.Timeout=(int)TimeSpan.FromMinutes(20).TotalMilliseconds;
#endif
            if (Preference.GetString(PreferenceName.UpdateWebProxyAddress) != "")
            {
                IWebProxy proxy = new WebProxy(Preference.GetString(PreferenceName.UpdateWebProxyAddress));
                ICredentials cred = new NetworkCredential(Preference.GetString(PreferenceName.UpdateWebProxyUserName), Preference.GetString(PreferenceName.UpdateWebProxyPassword));
                proxy.Credentials = cred;
                ws.Proxy = proxy;
            }
            return ws;
        }
    }
}