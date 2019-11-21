using OpenDentBusiness;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class CleaRay
    {
        public CleaRay()
        {
        }

        public static void SendData(Program ProgramCur, Patient pat)
        {
            string path = Programs.GetProgramPath(ProgramCur);
            if (pat == null)
            {
                try
                {
                    Process.Start(path);
                }
                catch
                {
                    MessageBox.Show(path + " is not available.");
                }
                return;
            }

            string str = "";

            var id =
                ProgramPreference.GetLong(ProgramCur.Id, ProgramPreferenceName.UsePatientIdOrChartNumber) == 0 ?
                    pat.PatNum.ToString() : pat.ChartNumber;

            str += "/ID:" + Tidy(id) + " ";
            str += "/LN:" + Tidy(pat.LName) + " ";
            str += "/N:" + Tidy(pat.FName) + " ";
            try
            {
                Process.Start(path, str);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static string Tidy(string input) => "\"" + input.Replace(";", "").Replace("/", "") + "\"";
    }
}
