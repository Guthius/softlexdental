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
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormHouseCalls : ODForm
    {
        private readonly long programId;
        private readonly bool usePatientIds;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormHouseCalls"/> class.
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="usePatientIds"></param>
        public FormHouseCalls(long programId, bool usePatientIds)
        {
            InitializeComponent();

            this.programId = programId;
            this.usePatientIds = usePatientIds;
        }

        private void FormHouseCalls_Load(object sender, EventArgs e)
        {
            textDateFrom.Text = DateTime.Today.AddDays(1).ToShortDateString();
            textDateTo.Text = DateTime.Today.AddDays(7).ToShortDateString();
        }

        private void Next7DaysButton_Click(object sender, EventArgs e)
        {
            textDateFrom.Text = DateTime.Today.AddDays(1).ToShortDateString();
            textDateTo.Text = DateTime.Today.AddDays(7).ToShortDateString();
        }

        private void AllButton_Click(object sender, EventArgs e)
        {
            textDateFrom.Text = DateTime.Today.AddDays(1).ToShortDateString();
            textDateTo.Text = "";
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            string Tidy(string input) => '"' + input.Replace("\"", "") + '"';

            textDateFrom.Text = textDateFrom.Text.Trim();
            if (textDateFrom.Text.Length == 0)
            {
                MessageBox.Show(
                    "From date cannot be left blank.",
                    "House Calls",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (!DateTime.TryParse(textDateFrom.Text, out var dateFrom))
            {
                MessageBox.Show(
                    "Please enter a valid from date.",
                    "House Calls",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            textDateTo.Text = textDateTo.Text.Trim();
            if (textDateFrom.Text.Length == 0 || !DateTime.TryParse(textDateTo.Text, out var dateTo))
            {
                dateTo = DateTime.MaxValue.AddDays(-1);

                textDateTo.Text = "";
            }

            try
            {
                var exportPath = ProgramPreference.GetString(programId, "export_path");
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }

                var fileName = Path.Combine(exportPath, "Appt.txt");

                using (var stream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var streamWriter = new StreamWriter(stream))
                {
                    streamWriter.WriteLine("\"LastName\",\"FirstName\",\"PatientNumber\",\"HomePhone\",\"WorkNumber\",\"EmailAddress\",\"SendEmail\",\"Address\",\"Address2\",\"City\",\"State\",\"Zip\",\"ApptDate\",\"ApptTime\",\"ApptReason\",\"DoctorNumber\",\"DoctorName\",\"IsNewPatient\",\"WirelessPhone\"");

                    DataTable table = HouseCallsQueries.GetHouseCalls(dateFrom, dateTo);

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DateTime.TryParse(table.Rows[i][13].ToString(), out var appointmentDate);

                        // Use the preferred name if one has been set, if not just use normal first name.
                        var patientName =
                            table.Rows[i][2].ToString() != "" ?
                                table.Rows[i][2].ToString() :
                                table.Rows[i][1].ToString();

                        var patientId =
                            usePatientIds ?
                                table.Rows[i][3].ToString() :
                                table.Rows[i][4].ToString();

                        var providerName =
                            Providers.GetLName(
                                long.Parse(table.Rows[i][15].ToString()));

                        streamWriter.Write(Tidy(table.Rows[i][0].ToString()) + ",");                     // 0 - LastName
                        streamWriter.Write(Tidy(patientName) + ",");                                     // FirstName or PreferredName
                        streamWriter.Write(Tidy(patientId) + ",");                                       // PatientId or ChartNumber
                        streamWriter.Write(Tidy(table.Rows[i][5].ToString()) + ",");                     // 5 - HomePhone
                        streamWriter.Write(Tidy(table.Rows[i][6].ToString()) + ",");                     // 6 - WorkNumber
                        streamWriter.Write(Tidy(table.Rows[i][7].ToString()) + ",");                     // 7 - EmailAddress
                        streamWriter.Write(Tidy(table.Rows[i][7].ToString() != "" ? "T" : "F") + ",");   // 7 - EmailAddress (SendEmail True/False)
                        streamWriter.Write(Tidy(table.Rows[i][8].ToString()) + ",");                     // 8 -Address
                        streamWriter.Write(Tidy(table.Rows[i][9].ToString()) + ",");                     // 9 -Address2
                        streamWriter.Write(Tidy(table.Rows[i][10].ToString()) + ",");                    // 10 - City
                        streamWriter.Write(Tidy(table.Rows[i][11].ToString()) + ",");                    // 11 - State
                        streamWriter.Write(Tidy(table.Rows[i][12].ToString()) + ",");                    // 12 - Zip
                        streamWriter.Write(Tidy(appointmentDate.ToString("MM/dd/yyyy")) + ",");          // 13 - ApptDate
                        streamWriter.Write(Tidy(appointmentDate.ToString("hh:mm tt")) + ",");            // 13 - ApptTime eg 01:30 PM
                        streamWriter.Write(Tidy(table.Rows[i][14].ToString()) + ",");                    // 14 - ApptReason
                        streamWriter.Write(Tidy(table.Rows[i][15].ToString()) + ",");                    // 15 - ProviderId (might possibly be 0)
                        streamWriter.Write(Tidy(providerName) + ",");                                    // ProviderName
                        streamWriter.Write(Tidy(table.Rows[i][16].ToString() == "1" ? "T" : "F") + ","); // 16 - IsNewPatient (SendEmail True/False)
                        streamWriter.Write(Tidy(table.Rows[i][17].ToString()) + "");                     // 17 - WirelessPhone
                        streamWriter.WriteLine();
                    }
                }

                MessageBox.Show(
                    "Done",
                    "House Calls",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    "House Calls",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            DialogResult = DialogResult.OK;
        }
    }
}
