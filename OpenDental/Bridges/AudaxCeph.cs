using OpenDentBusiness;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace OpenDental.Bridges
{
    public static class AudaxCeph
    {
        public static void SendData(Program program, Patient patient)
        {
            string programPath = Programs.GetProgramPath(program);
            if (!File.Exists(programPath))
            {
                MessageBox.Show(
                    programPath + " could not be found.",
                    "Audax Ceph",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            var processArray = Process.GetProcessesByName("AxCeph");

            string updateXmlPath = Path.Combine(Path.GetDirectoryName(programPath), "update.xml");

            if (File.Exists(updateXmlPath)) File.Delete(updateXmlPath);

            if (processArray.Length == 0)
            {
                MessageBox.Show(
                    "AxCeph.exe not found. Please make sure AudaxCeph is running and try again.",
                    "Audax Ceph", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
            else if (patient == null)
            {
                Process.Start(programPath);
            }
            else
            {
                var stringBuilder = new StringBuilder();

                var writerSettings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "   ",
                    NewLineChars = "\r\n",
                    OmitXmlDeclaration = true
                };

                using (var writer = XmlWriter.Create(stringBuilder, writerSettings))
                {
                    writer.WriteProcessingInstruction("xml", "version='1.0' encoding='utf-8'");
                    writer.WriteStartElement("AxCephComData");
                    writer.WriteStartElement("Patients");
                        writer.WriteStartElement("Patient");
                            writer.WriteElementString("PIDOutside", patient.PatNum.ToString());
                            writer.WriteElementString("NameOfPatient", Tidy(patient.LName) + ", " + Tidy(patient.FName));
                            writer.WriteElementString("DateOfBirth", patient.Birthdate.ToString("yyyyMMdd"));
                            writer.WriteElementString("Sex", patient.Gender.ToString() == "Female" ? "F" : "M");
                            writer.WriteElementString("Active", "1");
                        writer.WriteEndElement();//Patient
                    writer.WriteEndElement();//Patients
                    writer.WriteElementString("Command", "UpdateOrInsertPatient");
                    writer.WriteElementString("ResultXMLFileName", "result.xml");
                    writer.WriteElementString("ResultStatus", "");
                    writer.WriteElementString("ResultMessage", "0");
                    writer.WriteEndElement();//AxCephComData
                    writer.Flush();
                }

                File.WriteAllText(updateXmlPath, stringBuilder.ToString());
                try
                {
                    Process.Start(programPath, " update.xml");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        exception.Message,
                        "Audax Ceph",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Get rid of any character that isn't A-Z, a hyphen or a period.
        /// </summary>
        private static string Tidy(string input)
        {
            string result = string.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '-' || input[i] == '.' || char.IsLetter(input[i]))
                {
                    result += char.ToUpper(input[i]);
                }
            }
            return result;
        }
    }
}