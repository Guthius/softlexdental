using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    [Obsolete]
    public class Lan
    {
        public static string g(string classType, string text) => Lans.ConvertString(classType, text);

        public static string g(object sender, string text) => Lans.ConvertString(sender == null ? "All" : sender.GetType().Name, text);
    }
}