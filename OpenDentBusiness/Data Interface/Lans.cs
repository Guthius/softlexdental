using CodeBase;
using System;
using System.Globalization;

namespace OpenDentBusiness
{
    [Obsolete]
    public class Lans
    {
        public static string g(string classType, string text) => ConvertString(classType, text);

        public static string g(object sender, string text) => ConvertString(sender.GetType().Name, text);

        public static string ConvertString(string classType, string text)
        {
            if (classType == null || text == null)
            {
                return "";
            }

            return text.Trim();
        }

        public static string GetShortDateTimeFormat()
        {
            if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                return "MM/dd/yyyy";
            }
            else
            {
                return CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            }
        }

        /// <summary>
        /// Gets a short time format for displaying in appt and schedule along the sides. 
        /// Pass in a clone of the current culture; it will get altered. 
        /// Returns a string format.
        /// </summary>
        public static string GetShortTimeFormat(CultureInfo ci)
        {
            string format = "";
            ci.DateTimeFormat.AMDesignator = ci.DateTimeFormat.AMDesignator.ToLower();
            ci.DateTimeFormat.PMDesignator = ci.DateTimeFormat.PMDesignator.ToLower();
            string shortTimePattern = ci.DateTimeFormat.ShortTimePattern;
            if (shortTimePattern.IndexOf("hh") != -1)
            {//if hour is 01-12
                format += "hh";
            }
            else if (shortTimePattern.IndexOf("h") != -1)
            {//or if hour is 1-12
                format += "h";
            }
            else if (shortTimePattern.IndexOf("HH") != -1)
            {//or if hour is 00-23
                format += "HH";
            }
            else
            {//hour is 0-23
                format += "H";
            }
            if (shortTimePattern.IndexOf("t") != -1)
            {//if there is an am/pm designator
                format += "tt";
            }
            else
            {//if no am/pm designator, then use :00
                format += ":00";//time separator will actually change according to region
            }
            return format;
        }
    }
}
