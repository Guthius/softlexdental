﻿using System.Collections.Generic;
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

        ///<summary>The proxy's working directory.</summary>
        public static string SendAndReceiveUpdateRequestXml()
        {
            List<string> listProgramsEnabled = Programs.GetWhere(x => x.Enabled && !string.IsNullOrWhiteSpace(x.ProgName))
                .Select(x => x.ProgName).ToList();
            //prepare the xml document to send--------------------------------------------------------------------------------------
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("    ");
            StringBuilder strbuild = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(strbuild, settings))
            {
                writer.WriteStartElement("UpdateRequest");
                writer.WriteStartElement("RegistrationKey");
                writer.WriteString(Preference.GetString(PreferenceName.RegistrationKey));
                writer.WriteEndElement();
                writer.WriteStartElement("PracticeTitle");
                writer.WriteString(Preference.GetString(PreferenceName.PracticeTitle));
                writer.WriteEndElement();
                writer.WriteStartElement("PracticePhone");
                writer.WriteString(Preference.GetString(PreferenceName.PracticePhone));
                writer.WriteEndElement();
                writer.WriteStartElement("ProgramVersion");
                writer.WriteString(Preference.GetString(PreferenceName.ProgramVersion));
                writer.WriteEndElement();
                writer.WriteStartElement("ClinicCount");
                writer.WriteString(Preferences.HasClinicsEnabled ? Clinics.GetCount(true).ToString() : "0");
                writer.WriteEndElement();
                writer.WriteStartElement("ListProgramsEnabled");
                new XmlSerializer(typeof(List<string>)).Serialize(writer, listProgramsEnabled);
                writer.WriteEndElement();
                writer.WriteStartElement("DateFormat");
                writer.WriteString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                writer.WriteEndElement();
                writer.WriteEndElement();//UpdateRequest
            }
            return GetWebServiceInstance().RequestUpdate(strbuild.ToString());
        }
    }
}