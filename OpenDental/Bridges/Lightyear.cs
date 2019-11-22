using OpenDentBusiness;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class Lightyear
    {
        public Lightyear()
        {
        }

        ///<summary>Launches the program using the patient.Cur data.</summary>
        public static void SendData(Program ProgramCur, Patient pat)
        {
            string path = Programs.GetProgramPath(ProgramCur);
            List<ProgramPreference> ForProgram = ProgramProperties.GetForProgram(ProgramCur.Id); ;
            if (pat == null)
            {
                MessageBox.Show("Please select a patient first");
                return;
            }
            string info = "";
            //Patient id can be any string format
            ProgramPreference PPCur = ProgramProperties.GetCur(ForProgram, "Enter 0 to use PatientNum, or 1 to use ChartNum"); ;
            if (PPCur.Value == "0")
            {
                info += "-i \"" + pat.PatNum.ToString() + "\" ";
            }
            else
            {
                info += "-i \"" + pat.ChartNumber.Replace("\"", "") + "\" ";
            }
            info += "-n \"" + pat.LName.Replace("\"", "") + ", "
                + pat.FName.Replace("\"", "") + "\"";
            //MessageBox.Show(info);
            try
            {
                Process.Start(path, info);
            }
            catch
            {
                MessageBox.Show(path + " is not available.");
            }
        }
    }
}
