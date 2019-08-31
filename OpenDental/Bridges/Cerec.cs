using OpenDentBusiness;
using System.Diagnostics;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public static class Cerec
    {
        /// <summary>
        /// Launches the program using a combination of command line characters and the patient.Cur data.
        /// </summary>
        /// <remarks>
        /// Example CerPI.exe -<patNum>;<fname>;<lname>;<birthday DD.MM.YYYY>; (Date format specified in the windows Regional Settings)
        /// </remarks>
        public static void SendData(Program program, Patient patient)
        {
            string programPath = Programs.GetProgramPath(program);
            if (patient == null)
            {
                try
                {
                    Process.Start(programPath);
                }
                catch
                {
                    MessageBox.Show(
                        programPath + " is not available.",
                        "Cerec",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                return;
            }

            string payload = " -";
            if (ProgramProperties.GetPropVal(program.ProgramNum, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "0")
            {
                payload += patient.PatNum.ToString() + ";";
            }
            else
            {
                payload += patient.ChartNumber.ToString() + ";";
            }
            payload += patient.FName + ";" + patient.LName + ";" + patient.Birthdate.ToShortDateString() + ";";

            try
            {
                Process.Start(programPath, payload);
            }
            catch
            {
                MessageBox.Show(
                    programPath + " is not available, or there is an error in the command line options.", 
                    "Cerec", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }
    }
}