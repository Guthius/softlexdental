using OpenDentBusiness;
using System.Diagnostics;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public static class ActeonImagingSuite
    {
        public static void SendData(Program program, Patient patient)
        {
            string programPath = Programs.GetProgramPath(program);

            var propertyList = ProgramProperties.GetForProgram(program.ProgramNum);
            if (patient != null)
            {
                string propertyId = ProgramProperties.GetCur(propertyList, "Enter 0 to use PatientNum, or 1 to use ChartNum").PropertyValue;
                string dateFormat = ProgramProperties.GetCur(propertyList, "Birthdate format (default yyyyMMdd)").PropertyValue;

                string payload = propertyId == "0" ? patient.PatNum.ToString() : patient.ChartNumber;
                payload += " \"" + patient.LName.Replace("\"", "") + "\" \"" + patient.FName.Replace("\"", "") + "\" \"" + patient.Birthdate.ToString(dateFormat) + "\"";
                try
                {
                    Process.Start(programPath, program.CommandLine + payload);
                }
                catch
                {
                    MessageBox.Show(
                        string.Format(Translation.Language.BridgeExecutableNotAvailable, programPath),
                        "Acteon Imaging Suite", 
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    Process.Start(programPath);
                }
                catch
                {
                    MessageBox.Show(
                        string.Format(Translation.Language.BridgeExecutableNotAvailable, programPath),
                        "Acteon Imaging Suite",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}