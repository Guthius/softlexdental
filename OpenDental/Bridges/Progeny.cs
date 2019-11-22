using OpenDentBusiness;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class Progeny
    {
        public static void SendData(Program ProgramCur, Patient pat)
        {
            string path = Programs.GetProgramPath(ProgramCur);
            Process pibridge;
            List<ProgramPreference> ForProgram = ProgramProperties.GetForProgram(ProgramCur.Id);
            try
            {
                if (pat != null)
                {
                    ProgramPreference PPCur = ProgramProperties.GetCur(ForProgram, "Enter 0 to use PatientNum, or 1 to use ChartNum");
                    string id = "";
                    if (PPCur.Value == "0")
                    {
                        id = pat.PatNum.ToString();
                    }
                    else
                    {
                        id = pat.ChartNumber;
                    }
                    string lname = pat.LName.Replace("\"", "").Replace(",", "");
                    string fname = pat.FName.Replace("\"", "").Replace(",", "");
                    pibridge = new Process();
                    pibridge.StartInfo.CreateNoWindow = false;
                    pibridge.StartInfo.UseShellExecute = true;
                    pibridge.StartInfo.FileName = path;
                    //Double-quotes are removed from id and name to prevent malformed command. ID could have double-quote if chart number.
                    pibridge.StartInfo.Arguments = "cmd=open id=\"" + id.Replace("\"", "") + "\" first=\"" + fname.Replace("\"", "") + "\" last=\"" + lname.Replace("\"", "") + "\"";
                    pibridge.Start();
                }//if patient is loaded
                else
                {
                    //Should start Progeny without bringing up a pt.
                    pibridge = new Process();
                    pibridge.StartInfo.CreateNoWindow = false;
                    pibridge.StartInfo.UseShellExecute = true;
                    pibridge.StartInfo.FileName = path;
                    pibridge.StartInfo.Arguments = "cmd=start";
                    pibridge.Start();
                }
            }
            catch
            {
                MessageBox.Show(path + " is not available.");
            }
        }

    }
}
