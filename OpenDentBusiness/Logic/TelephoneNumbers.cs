using CodeBase;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OpenDentBusiness
{
    public class TelephoneNumbers
    {
        /// <summary>
        /// Formatting is only allowed when computer is set to en-US, en-CA, or fr-CA.
        /// </summary>
        public static bool IsFormattingAllowed
        {
            get
            {
                return 
                    CultureInfo.CurrentCulture.Name == "en-US" || 
                    CultureInfo.CurrentCulture.Name.EndsWith("CA");
            }
        }

        /// <summary>
        /// Returns true if the phone number is a valid format.  
        /// The number passed in can contain formating.  
        /// Will strip out formatting of the passed in phone number.  
        /// Phone number will be considered invalid if a '1' is found at the beginning.
        /// </summary>
        public static bool IsNumberValidTenDigit(ref string phoneNumber)
        {
            if (IsFormattingAllowed)
            {
                phoneNumber = phoneNumber
                    .Replace("(", "")
                    .Replace(")", "")
                    .Replace(" ", "")
                    .Replace("-", "");

                if (phoneNumber.Length != 0 && phoneNumber.Length != 10)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Used in the tool that loops through the database fixing telephone numbers.
        /// Also used in the patient import from XML tool, carrier edit window, and PT Dental bridge.
        /// </summary>
        public static string ReFormat(string phoneNum)
        {
            if (string.IsNullOrEmpty(phoneNum))
            {
                return "";
            }

            if (!IsFormattingAllowed)
            {
                return phoneNum;
            }

            var regex = new Regex(@"^\d{10}$");//eg. 5033635432
            if (regex.IsMatch(phoneNum))
            {
                return "(" + phoneNum.Substring(0, 3) + ")" + phoneNum.Substring(3, 3) + "-" + phoneNum.Substring(6);
            }

            regex = new Regex(@"^\d{11}$");//eg. 15033635432
            if (regex.IsMatch(phoneNum))
            {
                return phoneNum.Substring(0, 1) + "(" + phoneNum.Substring(1, 3) + ")" + phoneNum.Substring(4, 3) + "-" + phoneNum.Substring(7);
            }

            regex = new Regex(@"^\d{3}-\d{3}-\d{4}");//eg. 503-363-5432
            if (regex.IsMatch(phoneNum))
            {
                return "(" + phoneNum.Substring(0, 3) + ")" + phoneNum.Substring(4);
            }

            regex = new Regex(@"^\d-\d{3}-\d{3}-\d{4}");//eg. 1-503-363-5432 to 1(503)363-5432
            if (regex.IsMatch(phoneNum))
            {
                return phoneNum.Substring(0, 1) + "(" + phoneNum.Substring(2, 3) + ")" + phoneNum.Substring(6);
            }

            regex = new Regex(@"^\d{3} \d{3}-\d{4}");//eg 503 363-5432
            if (regex.IsMatch(phoneNum))
            {
                return "(" + phoneNum.Substring(0, 3) + ")" + phoneNum.Substring(4);
            }

            regex = new Regex(@"^\d{3} \d{3} \d{4}");//eg 916 363 5432
            if (regex.IsMatch(phoneNum))
            {
                return "(" + phoneNum.Substring(0, 3) + ")" + phoneNum.Substring(4, 3) + "-" + phoneNum.Substring(8);
            }

            regex = new Regex(@"^\(\d{3}\) \d{3} \d{4}");//eg (916) 363 5432
            if (regex.IsMatch(phoneNum))
            {
                return "(" + phoneNum.Substring(1, 3) + ")" + phoneNum.Substring(6, 3) + "-" + phoneNum.Substring(10);
            }

            regex = new Regex(@"^\(\d{3}\) \d{3}-\d{4}");//eg (916) 363-5432
            if (regex.IsMatch(phoneNum))
            {
                return "(" + phoneNum.Substring(1, 3) + ")" + phoneNum.Substring(6, 3) + "-" + phoneNum.Substring(10);
            }

            regex = new Regex(@"^\d{7}");//eg 3635432
            if (regex.IsMatch(phoneNum))
            {//this must be run after the d{10} match up above.
                return (phoneNum.Substring(0, 3) + "-" + phoneNum.Substring(3));
            }

            regex = new Regex(@"^\(\d{3}-\d{3}-\d{4}");//eg (916-363-5432
            if (regex.IsMatch(phoneNum))
            {
                return "(" + phoneNum.Substring(1, 3) + ")" + phoneNum.Substring(5, 3) + "-" + phoneNum.Substring(9);
            }

            regex = new Regex(@"^\d{3}\)\d{3}-\d{4}");//eg 916)363-5432
            if (regex.IsMatch(phoneNum))
            {
                return "(" + phoneNum.Substring(0, 3) + ")" + phoneNum.Substring(4, 3) + "-" + phoneNum.Substring(8);
            }
            return phoneNum;
        }

        /// <summary>
        /// Reformats initial entry with each keystroke.
        /// </summary>
        public static string AutoFormat(string phoneNum)
        {
            if (!IsFormattingAllowed)
            {
                return phoneNum;
            }
            if (Regex.IsMatch(phoneNum, @"^[2-9]$"))
            {
                return "(" + phoneNum;
            }
            if (Regex.IsMatch(phoneNum, @"^1\d$"))
            {
                return "1(" + phoneNum.Substring(1);
            }
            if (Regex.IsMatch(phoneNum, @"^\(\d\d\d\d$"))
            {
                return (phoneNum.Substring(0, 4) + ")" + phoneNum.Substring(4));
            }
            if (Regex.IsMatch(phoneNum, @"^1\(\d\d\d\d$"))
            {
                return (phoneNum.Substring(0, 5) + ")" + phoneNum.Substring(5));
            }
            if (Regex.IsMatch(phoneNum, @"^\(\d\d\d\)\d\d\d\d$"))
            {
                return (phoneNum.Substring(0, 8) + "-" + phoneNum.Substring(8));
            }
            if (Regex.IsMatch(phoneNum, @"^1\(\d\d\d\)\d\d\d\d$"))
            {
                return (phoneNum.Substring(0, 9) + "-" + phoneNum.Substring(9));
            }
            if (Regex.IsMatch(phoneNum, @"^1\d\d\d\d\d\d\d\d\d\d$"))
            {//If the value is pasted into the field, this could be the format.
                return (phoneNum.Substring(0, 1) + "(" + phoneNum.Substring(1, 3) + ")" + phoneNum.Substring(4, 3) + "-" + phoneNum.Substring(7));
            }
            if (Regex.IsMatch(phoneNum, @"^\d\d\d\d\d\d\d\d\d\d$"))
            {//If the value is pasted into the field, this could be the format.
                return ("1(" + phoneNum.Substring(0, 3) + ")" + phoneNum.Substring(3, 3) + "-" + phoneNum.Substring(6));
            }
            //Got through all other validation.  Make sure phoneNum is in correct final format.
            if (!Regex.IsMatch(phoneNum, @"^\(\d\d\d\)\d\d\d-\d\d\d\d$") || !Regex.IsMatch(phoneNum, @"^1\(\d\d\d\)\d\d\d-\d\d\d\d$"))
            {
                return ReFormat(phoneNum);
            }
            return phoneNum;
        }

        public static string FormatNumbersExactTen(string phoneNumber)
        {
            string result = "";

            for (int i = 0; i < phoneNumber.Length; i++)
            {
                if (char.IsNumber(phoneNumber, i))
                {
                    if (result == "" && phoneNumber[i] == '1')
                    {
                        continue; // Skip leading 1.
                    }
                    result += phoneNumber[i];
                }

                if (result.Length == 10) return result;
            }

            return ""; // Never made it to 10.
        }

        /// <summary>
        /// Returns true if these are the same numbers. Will still return true if the numbers have differnt formatting or only one of them has a leading 1.
        /// </summary>
        public static bool AreNumbersEqual(string phoneNum1, string phoneNum2)
        {
            phoneNum1 = phoneNum1.StripNonDigits();
            phoneNum2 = phoneNum2.StripNonDigits();
            if (phoneNum1.Length == 10 && phoneNum2.Length == 11 && phoneNum2[0] == '1')
            {
                phoneNum1 = "1" + phoneNum1;
            }
            if (phoneNum2.Length == 10 && phoneNum1.Length == 11 && phoneNum1[0] == '1')
            {
                phoneNum2 = "1" + phoneNum2;
            }
            return (phoneNum1 == phoneNum2);
        }
    }
}