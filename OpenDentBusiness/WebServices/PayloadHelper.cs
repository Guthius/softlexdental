using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using WebServiceSerializer;

namespace OpenDentBusiness
{
    ///<summary>This class provides helper methods when creating payloads to send to HQ hosted web services (e.g. WebServiceMainHQ.asmx and SheetsSynch.asmx)</summary>
    public class PayloadHelper
    {

        ///<summary>Returns an XML payload that includes common information required by most HQ hosted services (e.g. reg key, program version, etc).</summary>
        ///<param name="registrationKey">An Open Dental distributed registration key that HQ has on file.  Do not include hyphens.</param>
        ///<param name="practiceTitle">Any string is acceptable.</param>
        ///<param name="practicePhone">Any string is acceptable.</param>
        ///<param name="programVersion">Typically major.minor.build.revision.  E.g. 12.4.58.0</param>
        ///<param name="payloadContentxAsXml">Use CreateXmlWriterSettings(true) to create your payload xml. Outer-most xml element MUST be labeled 'Payload'.</param>
        ///<param name="serviceCode">Used on case by case basis to validate that customer is registered for the given service.</param>
        ///<returns>An XML string that can be passed into an HQ hosted web method.</returns>
        public static string CreatePayload(
            string payloadContentxAsXml, eServiceCode serviceCode, string registrationKey = null, string practiceTitle = null, string practicePhone = null, string programVersion = null)
        {
            StringBuilder strbuild = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(strbuild, WebSerializer.CreateXmlWriterSettings(false)))
            {
                writer.WriteStartElement("Request");
                writer.WriteStartElement("Credentials");
                writer.WriteStartElement("RegistrationKey");
                writer.WriteString(registrationKey ?? Preference.GetString(PreferenceName.RegistrationKey));
                writer.WriteEndElement();
                writer.WriteStartElement("PracticeTitle");
                writer.WriteString(practiceTitle ?? Preference.GetString(PreferenceName.PracticeTitle));
                writer.WriteEndElement();
                writer.WriteStartElement("PracticePhone");
                writer.WriteString(practicePhone ?? Preference.GetString(PreferenceName.PracticePhone));
                writer.WriteEndElement();
                writer.WriteStartElement("ProgramVersion");
                writer.WriteString(programVersion ?? Preference.GetString(PreferenceName.ProgramVersion));
                writer.WriteEndElement();
                writer.WriteStartElement("ServiceCode");
                writer.WriteString(serviceCode.ToString());
                writer.WriteEndElement();
                writer.WriteEndElement(); //Credentials
                writer.WriteRaw(payloadContentxAsXml);
                writer.WriteEndElement(); //Request
            }
            return strbuild.ToString();
        }

        public static string CreatePayloadWebHostSynch(string registrationKey, params PayloadItem[] listPayloadItems)
        {
            return CreatePayload(PayloadHelper.CreatePayloadContent(listPayloadItems.ToList()), eServiceCode.WebHostSynch, registrationKey, "", "", "");
        }

        ///<summary>Creates an XML string for the payload of the provided content. The list passed in is a tuple where the first item is the content to
        ///be serialized and the second item is the tag name for the content.</summary>
        public static string CreatePayloadContent(List<PayloadItem> listPayloadItems)
        {
            StringBuilder strbuild = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(strbuild, WebSerializer.CreateXmlWriterSettings(true)))
            {
                writer.WriteStartElement("Payload");
                foreach (PayloadItem payLoadItem in listPayloadItems)
                {
                    XmlSerializer xmlListConfirmationRequestSerializer = new XmlSerializer(payLoadItem.Content.GetType());
                    writer.WriteStartElement(payLoadItem.TagName);
                    xmlListConfirmationRequestSerializer.Serialize(writer, payLoadItem.Content);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement(); //Payload	
            }
            return strbuild.ToString();
        }
    }
    public class PayloadItem : Tuple<object, string>
    {
        public object Content { get { return Item1; } }
        public string TagName { get { return Item2; } }
        public PayloadItem(object content, string tagName) : base(content, tagName)
        {
        }
    }
}