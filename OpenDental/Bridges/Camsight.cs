using OpenDentBusiness;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public static class Camsight
    {
        public static void SendData(Program program, Patient patient)
        {
            string programPath = Programs.GetProgramPath(program);

            //usage: C:\cdm\cdm\cdmx\cdmx.exe ;patID;fname;lname;SSN;birthdate
            //example: ;5001;John;Smith;123456789;01012000
            //We did not get this information from Camsight.

            if (patient == null)
            {
                MessageBox.Show(
                    Translation.Language.PleaseSelectAPatientFirst,
                    "Camsight",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (!File.Exists(programPath))
            {
                MessageBox.Show(
                    string.Format(Translation.Language.BridgeExecutableNotFound, programPath),
                    "Camsight",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            //List<ProgramProperty> listForProgram=ProgramProperties.GetListForProgram(ProgramCur.ProgramNum);

            string payload = ";";
            if (ProgramProperties.GetPropVal(program.ProgramNum, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "1")
            {
                if (patient.ChartNumber == "")
                {
                    MessageBox.Show(
                        "This patient has no ChartNumber entered.", 
                        "Camsight", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);

                    return;
                }
                payload += patient.ChartNumber;
            }
            else
            {
                payload += patient.PatNum.ToString();
            }

            payload +=  
                ";" + Tidy(patient.FName) + 
                ";" + Tidy(patient.LName) + 
                ";" + patient.SSN + 
                ";" + patient.Birthdate.ToString("MM/dd/yyyy");

            try
            {
                Process.Start(programPath, payload);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static string Tidy(string input) => input.Replace(";", "").Replace(" ", "");
    }
}