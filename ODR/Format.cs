using System;
using System.Windows.Forms;
using OpenDentBusiness;

namespace ODR
{
    public class Format
    {
        public static string NumberHideZero(object original)
        {
            if (original == null)
            {
                return "";
            }
            double num = (double)original;//PIn.PDouble(original);
            if (num == 0)
            {
                return "";
            }
            return num.ToString("N");
        }
    }
}