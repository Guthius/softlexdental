using OpenDentBusiness;
using System.Diagnostics;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public interface IProgramBridge
    {
        void Launch(int programId, Patient patient);
    }

    public class ActeonImagingSuiteProgramBridge : IProgramBridge
    {
        public static class Properties
        {
            /// <summary>
            /// Specifies the location of the program executable path.
            /// </summary>
            public const string ProgramPath = "program_path";

            /// <summary>
            /// Indicates whether to use the ID of the patient; when false the chart ID is used instead.
            /// </summary>
            public const string UsePatientId = "use_patient_id";

            /// <summary>
            /// Specifies the date format to use when exporting dates.
            /// </summary>
            public const string DateFormat = "date_format";
        }

        public void Launch(int programId, Patient patient)
        {
            var programProperties = ProgramProperty.GetByProgram(programId);

            var programPath = programProperties.GetString(Properties.ProgramPath);
            if (string.IsNullOrEmpty(programPath))
            {
                return;
            }

            try
            {
                if (patient != null)
                {
                    bool usePatientId = programProperties.GetBool(Properties.UsePatientId, false);
                    var dateFormat = programProperties.GetString(Properties.DateFormat, "yyyyMMdd");

                    string payload = usePatientId ?
                        patient.PatNum.ToString() : 
                        patient.ChartNumber;

                    payload += " \"" + patient.LName.Replace("\"", "") + "\" \"" + patient.FName.Replace("\"", "") + "\" \"" + patient.Birthdate.ToString(dateFormat) + "\"";

                    Process.Start(programPath, payload);
                }
                else
                {
                    Process.Start(programPath);
                }
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


    public static class ActeonImagingSuite
    {
        public static void SendData(Program program, Patient patient)
        {
            string programPath = Programs.GetProgramPath(program);

            var propertyList = ProgramProperties.GetForProgram(program.ProgramNum);
            if (patient != null)
            {
                string propertyId = ProgramProperties.GetCur(propertyList, "Enter 0 to use PatientNum, or 1 to use ChartNum").Value;
                string dateFormat = ProgramProperties.GetCur(propertyList, "Birthdate format (default yyyyMMdd)").Value;

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