using OpenDentBusiness;
using System.Diagnostics;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class RayMage
    {
        public RayMage()
        {
        }

        public static void SendData(Program ProgramCur, Patient pat)
        {
            string path = Programs.GetProgramPath(ProgramCur);
            if (pat == null)
            {
                try
                {
                    Process.Start(path);//should start rayMage without bringing up a pt.
                }
                catch
                {
                    MessageBox.Show(path + " is not available.");
                }
            }
            else
            {
                string info = " /PATID \"";
                if (ProgramProperties.GetPropVal(ProgramCur.Id, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "0")
                {
                    info += pat.PatNum.ToString();
                }
                else
                {
                    info += pat.ChartNumber;
                }
                info += "\" /NAME \"" + pat.FName.Replace(" ", "").Replace("\"", "") + "\" /SURNAME \"" + pat.LName.Replace(" ", "").Replace("\"", "") + "\"";
                try
                {
                    Process.Start(path, ProgramCur.CommandLine + info);
                }
                catch
                {
                    MessageBox.Show(path + " is not available, or there is an error in the command line options.");
                }
            }
        }

    }
}
