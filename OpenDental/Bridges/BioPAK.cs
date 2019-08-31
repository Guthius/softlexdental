using OpenDentBusiness;
using System.Diagnostics;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public static class BioPAK
    {
        public static void SendData(Program program, Patient patient)
        {
            var programPath = Programs.GetProgramPath(program);
            if (patient == null)
            {
                // Should start rayMage without bringing up a pt.
                try
                {
                    Process.Start(programPath);
                }
                catch
                {
                    MessageBox.Show(
                        programPath + " is not available.",
                        "BioPAK",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                string payload = " -n";
                if (ProgramProperties.GetPropVal(program.ProgramNum, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "0")
                {
                    payload += patient.PatNum.ToString();
                }
                else
                {
                    payload += patient.ChartNumber;
                }
                payload += " -l" + patient.LName.Replace(" ", "").Replace("\"", "") + " -f" + patient.FName.Replace(" ", "").Replace("\"", "") + " -i" + patient.MiddleI.Replace(" ", "").Replace("\"", "");
                payload += " -s" + (patient.Gender == PatientGender.Female ? 'F' : 'M');

                if (patient.Birthdate.Year > 1880)
                {
                    payload += " -m" + patient.Birthdate.Month + " -d" + patient.Birthdate.Day + " -y" + patient.Birthdate.Year;
                }

                try
                {
                    Process.Start(programPath, program.CommandLine + payload);
                }
                catch
                {
                    MessageBox.Show(
                        programPath + " is not available, or there is an error in the command line options.",
                        "BioPAK", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}