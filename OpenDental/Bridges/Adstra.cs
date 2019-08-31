using OpenDentBusiness;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public static class Adstra
    {
        public static void SendData(Program program, Patient patient)
        {
            string path = Programs.GetProgramPath(program);
            if (patient == null)
            {
                MessageBox.Show(
                    Translation.Language.PleaseSelectAPatientFirst, 
                    "Adstra", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            string payload = Tidy(patient.LName) + "," + Tidy(patient.FName) + ",";
            if (ProgramProperties.GetPropVal(program.ProgramNum, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "0")
            {
                payload += patient.PatNum.ToString() + ",,";
            }
            else
            {
                payload += "," + Tidy(patient.ChartNumber) + ",";
            }
            payload += patient.Birthdate.ToString("yyyy/MM/dd");

            try
            {
                Process.Start(path, payload);
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message, 
                    "Adstra", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private static string Tidy(string input) => input.Replace(";", "").Replace(" ", "");
    }
}