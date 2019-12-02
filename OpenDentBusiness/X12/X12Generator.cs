/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using System.Text.RegularExpressions;

namespace OpenDentBusiness
{
    public class X12Generator
    {
        /// <summary>
        /// If <see cref="Clearinghouse.SenderTIN"/> is blank, then 810624427 will be used to indicate Open Dental.
        /// </summary>
        public static string GetISA06(Clearinghouse clearinghouseClin)
        {
            return 
                Sout(
                    clearinghouseClin.SenderTIN == "" ?
                        "810624427" :
                        clearinghouseClin.SenderTIN, 
                    15, 15);
        }

        /// <summary>
        /// Sometimes SenderTIN, sometimes Open Dental's TIN.
        /// </summary>
        public static string GetGS02(Clearinghouse clearinghouseClin)
        {
            return
                Sout(
                    clearinghouseClin.SenderTIN == "" ?
                        "810624427" :
                        clearinghouseClin.SenderTIN,
                    15, 15);
        }

        /// <summary>
        /// Returns the Provider Taxonomy code for the given specialty. Always 10 characters, validated.
        /// </summary>
        public static string GetTaxonomy(Provider provider)
        {
            if (provider.TaxonomyCodeOverride != "") return provider.TaxonomyCodeOverride;
            

            string spec = "1223G0001X";//general
            if (!provider.SpecialtyId.HasValue)
            {
                return spec;
            }

            Definition provSpec = Defs.GetDef(DefinitionCategory.ProviderSpecialties, provider.SpecialtyId.Value);
            if (provSpec == null)
            {
                return spec;
            }

            switch (provSpec.Description)
            {
                case "General": spec = "1223G0001X"; break;
                case "Hygienist": spec = "124Q00000X"; break;
                case "PublicHealth": spec = "1223D0001X"; break;
                case "Endodontics": spec = "1223E0200X"; break;
                case "Pathology": spec = "1223P0106X"; break;
                case "Radiology": spec = "1223X0008X"; break;
                case "Surgery": spec = "1223S0112X"; break;
                case "Ortho": spec = "1223X0400X"; break;
                case "Pediatric": spec = "1223P0221X"; break;
                case "Perio": spec = "1223P0300X"; break;
                case "Prosth": spec = "1223P0700X"; break;
                case "Denturist": spec = "122400000X"; break;
                case "Assistant": spec = "126800000X"; break;
                case "LabTech": spec = "126900000X"; break;
            }
            return spec;
        }

        /// <summary>
        /// Converts any string to an acceptable format for X12. Converts to all caps and strips 
        /// off all invalid characters. Optionally shortens the string to the specified length 
        /// and/or makes sure the string is long enough by padding with spaces.
        /// </summary>
        public static string Sout(string inputStr, int maxLength, int minLength)
        {
            string retStr = inputStr.ToUpper();

            retStr = Regex.Replace(retStr,//replaces characters in this input string
                                          //Allowed: !"&'()+,-./;?=(space)#   # is actually part of extended character set
                "[^\\w!\"&'\\(\\)\\+,-\\./;\\?= #]",//[](any single char)^(that is not)\w(A-Z or 0-9) or one of the above chars.
                "");
            retStr = Regex.Replace(retStr, "[_]", "");//replaces _

            if (maxLength != -1)
            {
                if (retStr.Length > maxLength)
                {
                    retStr = retStr.Substring(0, maxLength);
                }
            }

            if (minLength != -1)
            {
                if (retStr.Length < minLength)
                {
                    retStr = retStr.PadRight(minLength, ' ');
                }
            }
            //Debug.WriteLine(retStr);
            return retStr;
        }
    }
}
