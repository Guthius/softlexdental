using OpenDentBusiness;
using System.Diagnostics;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public static class ZImage
    {
        public static void SendData(Program ProgramCur, Patient pat)
        {
            string path = Programs.GetProgramPath(ProgramCur);
            //filepath.exe -patid 123 -fname John -lname Doe -dob 01/25/1962 -ssn 123456789 -gender M
            if (pat == null)
            {
                MessageBox.Show("Please select a patient first");
                return;
            }

            string payload = "-patid ";
            if (ProgramProperties.GetPropVal(ProgramCur.ProgramNum, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "0")
            {
                payload += pat.PatNum.ToString() + " ";
            }
            else
            {
                payload += pat.ChartNumber + " ";
            }
            payload += 
                "-fname " + Tidy(pat.FName) + " " + 
                "-lname " + Tidy(pat.LName) + " ";

            if (pat.Birthdate.Year > 1880)
            {
                payload += "-dob " + pat.Birthdate.ToShortDateString() + " ";
            }
            else
            {
                payload += "-dob  ";
            }
            if (pat.SSN.Replace("0", "").Trim() != "")
            {//An SSN which is all zeros will be treated as a blank SSN.  Needed for eCW, since eCW sets SSN to 000-00-0000 if the patient does not have an SSN.
                payload += "-ssn " + pat.SSN + " ";
            }
            else
            {
                payload += "-ssn  ";
            }

            if (pat.Gender == PatientGender.Female)
            {
                payload += "-gender F";
            }
            else
            {
                payload += "-gender M";
            }

            try
            {
                Process.Start(path, payload);
            }
            catch
            {
                MessageBox.Show(path + " is not available.");
            }
        }

        /// <summary>
        /// Removes semicolons and spaces.
        /// </summary>
        private static string Tidy(string input) => input.Replace(";", "").Replace(" ", "");
    }
}