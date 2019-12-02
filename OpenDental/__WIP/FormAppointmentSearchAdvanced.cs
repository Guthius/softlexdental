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
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAppointmentSearchAdvanced : FormBase
    {
        private readonly Appointment appointment;
        private List<ScheduleOpening> scheduleOpenings = new List<ScheduleOpening>();
        private string timeBefore;
        private string timeAfter;
        private DateTime dateAfter = DateTime.Today.AddDays(1);

        /// <summary>
        /// Gets or sets the ID of the selected clinic.
        /// </summary>
        private long SelectedClinicId
        {
            get
            {
                if (clinicComboBox.SelectedItem is Clinic clinic)
                {
                    return clinic.Id;
                }
                return 0;
            }
            set
            {
                foreach (Clinic clinic in clinicComboBox.Items)
                {
                    if (clinic.Id == value)
                    {
                        clinicComboBox.SelectedItem = clinic;

                        return;
                    }
                }

                clinicComboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormAppointmentSearchAdvanced"/> class.
        /// </summary>
        /// <param name="appointmentId">The ID of the appointment</param>
        public FormAppointmentSearchAdvanced(long appointmentId)
        {
            InitializeComponent();

            appointment = Appointments.GetOneApt(appointmentId);
        }

        /// <summary>
        ///     <para>
        ///         Parses time interval in format HH:MM and returns it as a <see cref="TimeSpan"/>.
        ///     </para>
        /// </summary>
        /// <param name="time">The time string to parse.</param>
        /// <returns>The time interval as a <see cref="TimeSpan"/> instance.</returns>
        private TimeSpan ParseTime(string time)
        {
            time = time.Trim();

            int i = time.IndexOf(':');
            if (i == -1)
            {
                return new TimeSpan(0, 0, 0);
            }

            if (int.TryParse(time.Substring(0, i), out var hours))
            {
                if (hours < 0) hours = 0;
                else if (hours > 24)
                {
                    hours = 24;
                }
            }

            if (int.TryParse(time.Substring(i + 1), out var minutes))
            {
                if (minutes < 0) minutes = 0;
                else if (minutes > 59)
                {
                    minutes = 59;
                }
            }

            return new TimeSpan(hours, minutes, 0);
        }

        private void FormAppointmentSearchAdvanced_Load(object sender, EventArgs e)
        {
            if (appointment == null)
            {
                MessageBox.Show(
                    "Invalid appointment on the Pinboard.",
                    "Appointment",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                DialogResult = DialogResult.Abort;

                return;
            }

            dateFromDateTimePicker.Value = dateAfter;
            dateToDateTimePicker.Value = dateAfter.AddYears(2).AddDays(1);
            timeBeforeTextBox.Text = timeBefore;
            timeAfterTextBox.Text = timeAfter;

            FillProviders(GetProvidersForSelectedClinic());
            FillBlockouts();
            FillClinics();
            FillAppointmentViews();
        }

        internal void SetSearchArgs(long appointmentId, List<long> providerIds, string timeBefore, string timeAfter, DateTime dateAfter)
        {
            // TODO: Do something with the unused args?

            this.timeBefore = timeBefore;
            this.timeAfter = timeAfter;
            this.dateAfter = dateAfter.Date;
        }

        private void FillProviders(List<Provider> providers, List<long> selectedProviderIds = null)
        {
            if (selectedProviderIds == null || selectedProviderIds.Count == 0)
            {
                selectedProviderIds = new List<long>();
            }

            providersComboBox.Items.Clear();
            providersComboBox.Items.Add(new ODBoxItem<Provider>("None", null));

            foreach (var provider in providers)
            {
                var providerListItem = new ODBoxItem<Provider>(provider.GetLongDesc(), provider);

                providersComboBox.Items.Add(providerListItem);
                if (provider.Id.In(selectedProviderIds))
                {
                    providersComboBox.SetSelected(providersComboBox.Items.IndexOf(providerListItem), true);
                }
            }

            if (providersComboBox.ListSelectedItems.Count == 0)
            {
                providersComboBox.SetSelected(0, true);
            }
        }

        private List<Provider> GetProvidersForSelectedClinic(ProviderMode providerMode = ProviderMode.All)
        {
            var providers = Providers.GetProvsForClinic(SelectedClinicId);

            switch (providerMode)
            {
                case ProviderMode.Dentist:
                    providers.RemoveAll(x => x.IsSecondary);
                    break;

                case ProviderMode.Hygienist:
                    providers.RemoveAll(x => !x.IsSecondary);
                    break;

                case ProviderMode.All:
                default:
                    break;
            }

            return providers;
        }

        private void FillBlockouts()
        {
            var blockoutTypes = Definition.GetByCategory(DefinitionCategory.BlockoutTypes);

            blockoutComboBox.Items.Clear();
            blockoutComboBox.Items.Add("None");
            blockoutComboBox.SelectedIndex = 0;

            foreach (var blockoutType in blockoutTypes)
            {
                if (blockoutType.Value.Contains(BlockoutType.NoSchedule.GetDescription()))
                {
                    continue; // TODO: Fix me...
                }

                blockoutComboBox.Items.Add(blockoutType);
            }

            blockoutComboBox.SelectedIndex = 0;
        }

        private void FillClinics()
        {
            clinicComboBox.Items.Clear();

            foreach (var clinic in Clinic.GetByUser(Security.CurrentUser))
            {
                clinicComboBox.Items.Add(clinic);
                if (clinic.Id == appointment.ClinicNum)
                {
                    clinicComboBox.SelectedItem = clinic;
                }
            }

            if (clinicComboBox.SelectedIndex == -1) 
                clinicComboBox.SelectedIndex = 0;
        }

        private void FillAppointmentViews()
        {
            appointmentViewComboBox.Items.Clear();

            foreach (var appointmentView in AppointmentView.GetByClinic(SelectedClinicId))
            {
                appointmentViewComboBox.Items.Add(appointmentView);
            }

            if (appointmentViewComboBox.Items.Count > 0) 
                appointmentViewComboBox.SelectedIndex = 0;
        }

        private void FillGrid()
        {
            resultsGrid.BeginUpdate();
            resultsGrid.Columns.Clear();
            resultsGrid.Columns.Add(new ODGridColumn("Day", 85));
            resultsGrid.Columns.Add(new ODGridColumn("Date", 85, HorizontalAlignment.Center));
            resultsGrid.Columns.Add(new ODGridColumn("Time", 85, HorizontalAlignment.Center));
            resultsGrid.Rows.Clear();

            foreach (var scheduleOpening in scheduleOpenings)
            {
                var row = new ODGridRow();

                row.Cells.Add(scheduleOpening.DateTimeAvail.DayOfWeek.ToString());
                row.Cells.Add(scheduleOpening.DateTimeAvail.Date.ToShortDateString());
                row.Cells.Add(scheduleOpening.DateTimeAvail.ToShortTimeString());
                row.Tag = scheduleOpening;

                resultsGrid.Rows.Add(row);
            }

            resultsGrid.EndUpdate();
        }

        private void DoSearch()
        {

            var dateStart = dateFromDateTimePicker.Value.Date.AddDays(-1);
            var dateEnd = dateToDateTimePicker.Value.Date.AddDays(1);

            scheduleOpenings.Clear();

            if (dateStart.Year < 1880 || dateEnd.Year < 1880)
            {
                MessageBox.Show(
                    "Invalid date selection.",
                    "Search",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            var timeBefore = TimeSpan.Zero;
            if (timeBeforeTextBox.Text != "")
            {
                try
                {
                    timeBefore = ParseTime(timeBeforeTextBox.Text);
                }
                catch
                {
                    MessageBox.Show(
                        "Invalid 'Starting before' time.",
                        "Search",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }

            var timeAfter = TimeSpan.Zero;
            if (timeAfterTextBox.Text != "")
            {
                try
                {
                    timeAfter = ParseTime(timeAfterTextBox.Text);
                }
                catch
                {
                    MessageBox.Show(
                        "Invalid 'Starting after' time.",
                        "Search",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }

            var blockoutType = blockoutComboBox.SelectedItem as Definition;
            if (blockoutType == null && providersComboBox.SelectedTags<Provider>().Contains(null))
            {
                MessageBox.Show(
                    "Please pick a provider or a blockout type.",
                    "Search",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            Cursor = Cursors.WaitCursor;

            var operatoryIds = new List<long>();
            var clinicIds = new List<long>();
            var providerIds = new List<long>();

            long blockoutTypeId = blockoutType.Id;

            providerIds.Add(0);//providers don't matter for blockouts

            if (!providersComboBox.SelectedTags<Provider>().Contains(null))
            {
                foreach (ODBoxItem<Provider> provBoxItem in providersComboBox.ListSelectedItems)
                {
                    providerIds.Add(provBoxItem.Tag.Id);
                }
            }

            clinicIds.Add(SelectedClinicId);

            operatoryIds = Operatory.GetByClinic(SelectedClinicId).Select(x => x.Id).ToList();

            if (blockoutTypeId != 0 && providerIds.Max() > 0)
            {
                scheduleOpenings.AddRange(
                    ApptSearch.GetSearchResultsForBlockoutAndProvider(
                        providerIds, appointment.AptNum, dateStart, dateEnd, operatoryIds, clinicIds, timeBefore, timeAfter, blockoutTypeId, 15));
            }
            else
            {
                scheduleOpenings = ApptSearch.GetSearchResults(
                    appointment.AptNum, dateStart, dateEnd, providerIds, operatoryIds, clinicIds, timeBefore, timeAfter, blockoutTypeId, resultCount: 15);
            }

            Cursor = Cursors.Default;

            FillGrid();
        }

        private void ProvidersComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (providersComboBox.SelectedTags<Provider>().Contains(null))
            {
                providersComboBox.SetSelected(false);
                providersComboBox.SetSelected(0, true);
            }
        }

        private void ProvidersBrowseButton_Click(object sender, EventArgs e)
        {
            var selectedProviders = 
                providersComboBox.Items.Cast<ODBoxItem<Provider>>()
                    .Where(x => x.Tag != null)
                    .Select(x => x.Tag)
                    .ToList();

            using (var formProvidersMultiPick = new FormProvidersMultiPick(selectedProviders))
            {
                formProvidersMultiPick.SelectedProviders = providersComboBox.SelectedTags<Provider>().FindAll(x => x != null);
                if (formProvidersMultiPick.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                var providerIds = new List<long>();
                foreach (var provider in formProvidersMultiPick.SelectedProviders)
                {
                    providerIds.Add(provider.Id);
                }

                FillProviders(GetProvidersForSelectedClinic(), providerIds);
            }
        }

        private void ClinicComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (clinicComboBox.SelectedItem is Clinic clinic)
            {
                appointmentViewComboBox.Visible = true;
                appointmentViewLabel.Visible = true;

                FillAppointmentViews();
            }
            else
            {
                appointmentViewComboBox.Visible = false;
                appointmentViewLabel.Visible = false;
            }

            var selectedProviderIds = providersComboBox.SelectedTags<Provider>().FindAll(x => x != null).Select(x => x.Id).ToList();

            FillProviders(GetProvidersForSelectedClinic(), selectedProviderIds);
        }

        private void ClinicBrowseButton_Click(object sender, EventArgs e)
        {
            using (var formClinics = new FormClinics())
            {
                formClinics.IsMultiSelect = false;
                formClinics.IsSelectionMode = true;

                if (formClinics.ShowDialog() == DialogResult.OK)
                {
                    SelectedClinicId = formClinics.SelectedClinicId;
                }
            }
        }

        private void ResultsGrid_CellClick(object sender, ODGridClickEventArgs e)
        {
            DateTime rowDate = ((ScheduleOpening)resultsGrid.Rows[e.Row].Tag).DateTimeAvail;

            // Move the calendar day on the appt module to the day that was clicked on. 
            GotoModule.GotoAppointment(rowDate.Date, appointment.AptNum);

            // TODO: If clinics, move to the clinic as well? 
        }

        private void MoreButton_Click(object sender, EventArgs e)
        {
            if (scheduleOpenings.Count < 1)
            {
                return;
            }

            dateFromDateTimePicker.Value = scheduleOpenings[scheduleOpenings.Count - 1].DateTimeAvail;

            DoSearch(); // should we prevent them (disable the button if they have changed any of their settings? 
        }

        private void ProvidersDentistButton_Click(object sender, EventArgs e)
        {
            var providers = GetProvidersForSelectedClinic(ProviderMode.Dentist);

            FillProviders(providers, providers.Select(x => x.Id).ToList());

            DoSearch();
        }

        private void ProvidersHygienistButton_Click(object sender, EventArgs e)
        {
            var providers = GetProvidersForSelectedClinic(ProviderMode.Hygienist);

            FillProviders(providers, providers.Select(x => x.Id).ToList());

            DoSearch();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (appointment.AptNum <= 0)
            {
                MessageBox.Show(
                    "Invalid appointments on pinboard.", 
                    "Appointment",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            DoSearch();

            moreButton.Enabled = true;
        }

        private enum ProviderMode
        {
            All,
            Dentist,
            Hygienist
        }
    }
}
