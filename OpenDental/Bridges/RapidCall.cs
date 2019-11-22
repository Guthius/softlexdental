using System;
using System.Diagnostics;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental.Bridges
{
    public class RapidCall
    {
        public RapidCall()
        {
        }

        public static void SendData(Program ProgramCur)
        {
            string path = Programs.GetProgramPath(ProgramCur);
            try
            {
                Process.Start(path, ProgramCur.CommandLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void ShowPage()
        {
            try
            {
                if (Programs.IsEnabled(ProgramName.RapidCall))
                {
                    SendData(Programs.GetCur(ProgramName.RapidCall));
                }
                else
                {
                    Process.Start("http://www.opendental.com/resources/redirects/redirectdentaltekrapidcall.html");
                }
            }
            catch
            {
                throw new Exception("Failed to open web browser. Please make sure you have a default browser set and are connected to the internet and then try again.");
            }
        }

        private static string Tidy(string input) => input.Replace(";", "").Replace(" ", "");
    }
}
