/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using OpenDentBusiness;
using OpenDentBusiness.Bridges;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace OpenDental.Bridges
{
    public class DemandForceBridge : CommandLineBridge
    {
        // TODO: Test this bridge...

        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Define("license_key", "DemandForce License Key (required)", BridgePreferenceType.String)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="DemandForceBridge"/> class.
        /// </summary>
        public DemandForceBridge() : base("DemandForce", "", "https://www.demandforce.com/", preferences)
        {
        }

        /// <summary>
        ///     <para>
        ///         Generates a file with the patient details.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient.</param>
        /// <param name="arguments">The command line arguments to pass to the program.</param>
        /// <returns>
        ///     True if the preparation was successful and the program can be started; otherwise, false.
        /// </returns>
        protected override bool PrepareToRun(long programId, Patient patient, out string arguments)
        {
            arguments = "";

            var result = 
                MessageBox.Show(
                    "This may take 20 minutes or longer. Continue?", 
                    Name, 
                    MessageBoxButtons.OKCancel, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return false;

            using (var formProgress = new FormProgress())
            {
                formProgress.MaxVal = 100;
                formProgress.NumberMultiplication = 1;
                formProgress.DisplayText = "";
                formProgress.NumberFormat = "F";

                var programPath = GetProgramPath(programId);

                var workerThread = new Thread(() =>
                {
                    ExportDataForBridge(
                        programId, 
                        programPath, 
                        formProgress);
                });

                workerThread.Start(formProgress);

                if (formProgress.ShowDialog() == DialogResult.Cancel)
                {
                    workerThread.Abort();

                    MessageBox.Show(
                        "Export cancelled. Partially created file has been deleted.", 
                        Name, 
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    DeleteFile(Path.Combine(Path.GetDirectoryName(programPath), "extract.xml"));

                    return false;
                }
            }

            MessageBox.Show(
                "Export complete. Press OK to launch DemandForce.",
                Name, 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);

            return true;
        }

        /// <summary>
        ///     <para>
        ///         Exports all patient and appointment data to the extract.xml file.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="programPath">The full path to where DemandForce is installed.</param>
        /// <param name="form">The progress indicator form.</param>
        private void ExportDataForBridge(long programId, string programPath, FormProgress form)
        {
            var extractFilePath = 
                Path.Combine(
                    Path.GetDirectoryName(programPath), "extract.xml");

            DeleteFile(extractFilePath);

            var licenseKey = ProgramPreference.GetString(programId, "license_key");
            var currentVersion = new Version(Application.ProductVersion).ToString();
            var extractDateTime = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffK");
            var dateLastVisit = Appointments.GetDateLastVisit();
            var patientAppointments = Appointments.GetAptsForPats(DateTime.Now.ToUniversalTime(), DateTime.Now.AddDays(210).ToUniversalTime());//appointments from todays date forward 210 days
            var patientIds = Patients.GetAllPatNums(false);
            var appointmentProcedureIds = Appointments.GetCodeNumsAllApts();

            var linesTotal = CalculateExportLineCount(patientIds, patientAppointments, appointmentProcedureIds);
            var linesProcessed = 0.0d;

            // Wait for the form to show the first time, or else the Invoke calls will cause an exception.
            while (!form.IsHandleCreated) { }

            form.Invoke(new UpdateProgressDelegate(UpdateProgress), 
                new object[] { 
                    form,
                    linesProcessed,
                    "Executing the bridge to DemandForce",
                    100.0,
                    "" });

            Thread.Sleep(1000); // Wait 1 second so the user can see the progress bar popup.
            try
            {
                StringBuilder strb = new StringBuilder();

                var settings = new XmlWriterSettings
                {
                    Encoding = Encoding.UTF8,
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    OmitXmlDeclaration = true
                };

                using (var stream = File.Open(extractFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("DemandForce");
                    writer.WriteAttributeString("licenseKey", licenseKey);
                    writer.WriteAttributeString("scope", "full");
                    writer.WriteStartElement("Business");
                    writer.WriteStartElement("Extract");
                    writer.WriteAttributeString("extractDateTime", extractDateTime);
                    writer.WriteAttributeString("managementSystemName", "Open Dental");
                    writer.WriteAttributeString("managementSystemVersion", currentVersion);
                    writer.WriteEndElement();

                    foreach (var patientId in patientIds)
                    {
                        var patient = Patients.GetPat(patientId);

                        writer.WriteStartElement("Customer");
                        writer.WriteAttributeString("id", patient.PatNum.ToString());
                        if (patient.ChartNumber != "")
                        {
                            writer.WriteAttributeString("chartId", patient.ChartNumber);
                        }

                        if (dateLastVisit.ContainsKey(patient.PatNum))
                        {
                            writer.WriteAttributeString("lastVisit",
                                dateLastVisit[patient.PatNum].ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffK"));
                        }
                        else
                        {
                            writer.WriteAttributeString("lastVisit", 
                                DateTime.MinValue.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffK"));
                        }

                        writer.WriteStartElement("Demographics");
                        writer.WriteAttributeString("firstName", string.IsNullOrEmpty(patient.FName) ? "X" : patient.FName);
                        writer.WriteAttributeString("lastName", patient.LName);
                        writer.WriteAttributeString("gender", patient.Gender == PatientGender.Female ? "Female" : "Male");

                        if (patient.Birthdate.Year > 1880)
                        {
                            writer.WriteAttributeString("birthday", patient.Birthdate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffK"));
                        }

                        if (patient.Address != "") writer.WriteAttributeString("address1", patient.Address);
                        if (patient.City != "") writer.WriteAttributeString("city", patient.City);
                        if (patient.State != "") writer.WriteAttributeString("State", patient.State);
                        if (patient.Zip != "") writer.WriteAttributeString("Zip", patient.Zip);
                        if (patient.Email != "") writer.WriteAttributeString("Email", patient.Email);

                        writer.WriteEndElement();

                        if (patientAppointments.TryGetValue(patient.PatNum, out var appointments))
                        {
                            foreach (var apt in appointments)
                            {
                                writer.WriteStartElement("Appointment");
                                writer.WriteAttributeString("id", apt.AptNum.ToString());
                                writer.WriteAttributeString("status", apt.AptStatus == ApptStatus.Complete ? "1" : "3");

                                if (Defs.GetDef(DefinitionCategory.ApptConfirmed, apt.Confirmed).Description.ToLower() == "unconfirmed")
                                {
                                    writer.WriteAttributeString("confirmed", "0");
                                }
                                else
                                {
                                    writer.WriteAttributeString("confirmed", "1");
                                }
                                writer.WriteAttributeString("date", apt.AptDateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffK"));
                                writer.WriteAttributeString("duration", (apt.Pattern.Length * 5).ToString());

                                if (appointmentProcedureIds.TryGetValue(apt.AptNum, out var procedureIds))
                                {
                                    var procedureCodes = string.Join(", ", procedureIds.Select(procedureId => ProcedureCodes.GetStringProcCode(procedureId)));

                                    linesProcessed += 2 * procedureIds.Count;

                                    if (linesProcessed < linesTotal)
                                    {
                                        form.Invoke(new UpdateProgressDelegate(UpdateProgress),
                                            new object[] {
                                                    form,
                                                    linesProcessed / linesTotal * 100,
                                                    "Creating export file: ?currentVal % of ?maxVal % completed",
                                                    100.0,
                                                    ""});
                                    }

                                    if (procedureCodes != "") writer.WriteAttributeString("code", procedureCodes);
                                }
                                writer.WriteEndElement();

                                if (linesProcessed < linesTotal)
                                {
                                    form.Invoke(new UpdateProgressDelegate(UpdateProgress),
                                        new object[] {
                                            form,
                                            linesProcessed / linesTotal * 100,
                                            "Creating export file: ?currentVal % of ?maxVal % completed",
                                            100.0,
                                            ""});
                                }

                                linesProcessed += 12;
                            }
                        }
                        writer.WriteEndElement();

                        if (linesProcessed < linesTotal)
                        {
                            form.Invoke(new UpdateProgressDelegate(UpdateProgress),
                                new object[] {
                                    form,
                                    linesProcessed / linesTotal * 100,
                                    "Creating export file: ?currentVal % of ?maxVal % completed",
                                    100.0,
                                    ""});
                        }

                        linesProcessed += 20;
                    }

                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Export file creation failed. " + exception.Message,
                    Name, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }

            if (linesProcessed >= linesTotal)
            {
                form.DisplayText = "Creating export file: 100 % of 100 % completed";
            }

            // Wait a little bit so the user can see that it got to 100% complete.
            Thread.Sleep(600);

            // Force dialog to close even if no files copied or calculation was slightly off.
            form.Invoke(new UpdateProgressDelegate(UpdateProgress), new object[] { form, 0, "", 0, "" });
        }

        /// <summary>
        /// Deletes the specified file if it exists.
        /// </summary>
        /// <param name="filePath">The full path of the file.</param>
        private static void DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch { }
        }

        /// <summary>
        ///     <para>
        ///         Calculates the total amount of lines that will be written to the export.xml 
        ///         file based on the specified dataset.
        ///     </para>
        /// </summary>
        /// <param name="patientIds">A array of ID's of the patients to exports.</param>
        /// <param name="patientAppointments">A a list of appointments per patient.</param>
        /// <param name="appointmentProcedureIds">A list of procedure ID's for every appointment.</param>
        /// <returns>The total number of lines that will be generated.</returns>
        private static long CalculateExportLineCount(long[] patientIds, Dictionary<long, List<Appointment>> patientAppointments, Dictionary<long, List<long>> appointmentProcedureIds)
        {
            long numberOfLines = 0;

            foreach (var patientId in patientIds)
            {
                if (patientAppointments.TryGetValue(patientId, out var appointments))
                {
                    foreach (var appointment in appointments)
                    {
                        if (appointmentProcedureIds.TryGetValue(appointment.AptNum, out var procedureIds))
                        {
                            numberOfLines += procedureIds.Count * 2;
                        }
                    }

                    numberOfLines += 12;
                }

                numberOfLines += 20;
            }

            return numberOfLines;
        }

        private static void UpdateProgress(FormProgress form, double currentValue, string newDisplayText, double newMaxValue, string errorMessage)
        {
            form.CurrentVal = currentValue;
            form.DisplayText = newDisplayText;
            form.MaxVal = newMaxValue;
            form.ErrorMessage = errorMessage;
        }

        private delegate void UpdateProgressDelegate(FormProgress form, double currentValue, string newDisplayText, double newMaxValue, string errorMessage);
    }
}
