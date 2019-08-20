using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

//This file is used in conjunction with and must match WebServiceCustomerUpdates\WebSerialize.cs.
namespace WebServiceSerializer
{
    public static class WebSerializer
    {
        public static XmlWriterSettings CreateXmlWriterSettings(bool omitXmlDeclaration)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("    ");
            settings.OmitXmlDeclaration = omitXmlDeclaration;
            return settings;
        }

        public static T DeserializeTag<T>(string resultXml, string tagName)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(resultXml);
            //Validate output.
            XmlNode node = doc.SelectSingleNode("//Error");
            if (node != null)
            {
                throw new Exception(node.InnerText);
            }
            node = doc.SelectSingleNode("//" + tagName);
            if (node == null)
            {
                throw new Exception("tagName node not found: " + tagName);
            }
            T retVal;
            using (XmlReader reader = XmlReader.Create(new StringReader(node.InnerXml)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                retVal = (T)serializer.Deserialize(reader);
            }
            if (retVal == null)
            {
                throw new Exception("tagName node invalid: " + tagName);
            }
            return retVal;
        }
    }

    /// <summary>
    /// Converts various datatypes into strings formatted correctly for MySQL.
    /// "P" was originally short for Parameter because this class was written specifically to replace parameters 
    /// in the mysql queries. Using strings instead of parameters is much easier to debug.  This will later be 
    /// rewritten as a System.IConvertible interface on custom mysql types. I would rather not ever depend 
    /// on the mysql connector for this so that this program remains very db independent.
    /// Marked internal so it doesn't get mistaken or misused in place of OpenDentBusiness.POut.
    /// </summary>
    internal class POut
    {
        public static string PBool(bool myBool)
        {
            if (myBool == true)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        public static string PByte(byte myByte)
        {
            return myByte.ToString();
        }

        public static string PInt(int myInt)
        {
            return myInt.ToString();
        }

        public static string PFloat(float myFloat)
        {
            return myFloat.ToString();
        }
    }

    /// <summary>
    /// Converts strings coming in from the database into the appropriate type. 
    /// "P" was originally short for Parameter because this class was written specifically to 
    /// replace parameters in the mysql queries. Using strings instead of parameters is much easier to debug. 
    /// This will later be rewritten as a System.IConvertible interface on custom mysql types. 
    /// I would rather not ever depend on the mysql connector for this so that this program remains very db independent.
    /// Marked internal so it doesn't get mistaken or misused in place of OpenDentBusiness.PIn.
    /// </summary>
    internal class PIn
    {
        public static bool PBool(string myString)
        {
            return myString == "1";
        }

        public static byte PByte(string myString)
        {
            if (myString == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToByte(myString);
            }
        }

        public static double PDouble(string myString)
        {
            if (myString == "")
            {
                return 0;
            }
            else
            {
                try
                {
                    return Convert.ToDouble(myString);
                }
                catch
                {
                    return 0;
                }
            }

        }

        public static int PInt(string myString)
        {
            if (myString == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(myString);
            }
        }

        public static long PLong(string myString)
        {
            if (myString == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(myString);
            }
        }

        public static float PFloat(string myString)
        {
            if (myString == "")
            {
                return 0;
            }
            return Convert.ToSingle(myString);
        }

        public static string PString(string myString)
        {
            return myString;
        }
    }
}