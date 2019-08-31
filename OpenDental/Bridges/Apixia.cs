using OpenDentBusiness;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    /// <remarks>
    /// Data like the following:
    /// [Patient]
    /// ID=A123456789
    /// Gender = Male
    /// First=John
    /// Last = Smith
    /// Year=1955
    /// Month=1
    /// Day=23
    /// [Dentist]
    /// ID=001
    /// Password=1234
    /// 
    /// Should appear in the following file: C:/Program Files/Digirex/Switch.ini and should be accessed/opened by C:/Program Files/Digirex/digirex.ini
    /// </remarks>
    public static class Apixia
    {
        public static void SendData(Program program, Patient patient)
        {
            string path = Programs.GetProgramPath(program);
            if (patient == null)
            {
                try
                {
                    Process.Start(path);
                }
                catch
                {
                    MessageBox.Show(path + " is not available.");
                }
            }
            else
            {
                string payload = "[Patient]\r\n";
                if (ProgramProperties.GetPropVal(program.ProgramNum, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "0")
                {
                    payload += "ID=" + patient.PatNum.ToString() + "\r\n";
                }
                else
                {
                    payload += "ID=" + patient.ChartNumber + "\r\n";
                }

                var provider = Providers.GetProv(patient.PriProv);
                if (provider == null)
                {
                    MessageBox.Show(
                        "Invalid provider for the selected patient.", 
                        "Apixia",
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);

                    return;
                }

                payload += 
                    "Gender=" + patient.Gender.ToString() + "\r\n" + 
                    "First=" + patient.FName + "\r\n" + 
                    "Last=" + patient.LName + "\r\n" + 
                    "Year=" + patient.Birthdate.Year.ToString() + "\r\n" + 
                    "Month=" + patient.Birthdate.Month.ToString() + "\r\n" + 
                    "Day=" + patient.Birthdate.Day.ToString() + "\r\n" + 
                    "[Dentist]\r\n" + 
                    "ID=" + provider.Abbr + "\r\n" +
                    "Password=digirex";
  
                string iniPath = ProgramProperties.GetPropVal(program.ProgramNum, "System path to Apixia Digital Imaging ini file");
                try
                {
                    File.WriteAllText(iniPath, payload);

                    Process.Start(path);
                }
                catch (UnauthorizedAccessException exception)
                {
                    MessageBox.Show(
                        exception.Message,
                        "Apixia",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                catch
                {
                    MessageBox.Show(
                        path + " is not available.",
                        "Apixia",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}