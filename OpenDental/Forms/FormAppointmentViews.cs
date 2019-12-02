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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAppointmentViews : FormBase
    {
        private bool viewChanged;
        private List<Clinic> clinics;
        private List<AppointmentView> appointmentViews;

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
        /// Initializes a new instance of the <see cref="FormAppointmentViews"/> class.
        /// </summary>
        public FormAppointmentViews() => InitializeComponent();

        private void FormAppointmentViews_Load(object sender, EventArgs e)
        {
            clinics = Clinic.GetByUser(Security.CurrentUser).ToList();

            clinicComboBox.Items.Clear();
            foreach (var clinic in clinics)
            {
                clinicComboBox.Items.Add(clinic);
                if (clinic.Id == Clinics.ClinicId)
                {
                    clinicComboBox.SelectedItem = clinic;
                }
            }

            appointmentViews = AppointmentView.GetByClinic(SelectedClinicId).ToList();

            FillAppointmentViews();
            if (Preference.GetInt(PreferenceName.AppointmentTimeIncrement) == 5)
            {
                increment5RadioButton.Checked = true;
            }
            else if (Preference.GetInt(PreferenceName.AppointmentTimeIncrement) == 10)
            {
                increment10RadioButton.Checked = true;
            }
            else
            {
                increment15RadioButton.Checked = true;
            }
        }

        private void FillAppointmentViews()
        {
            appointmentViewsListBox.Items.Clear();

            string shortcutKey;

            foreach (var appointmentView in appointmentViews)
            {
                if (appointmentView.ClinicId == SelectedClinicId)
                {
                    if (appointmentViewsListBox.Items.Count < 12)
                    {
                        shortcutKey = "F" + (appointmentViewsListBox.Items.Count + 1).ToString() + " - ";
                    }
                    else shortcutKey = "";

                    appointmentViewsListBox.Items.Add(shortcutKey + appointmentView.Description);
                }
            }
        }

        private void ClinicComboBox_SelectionChangeCommitted(object sender, EventArgs e) => FillAppointmentViews();
        
        private void AppointmentViewsListBox_DoubleClick(object sender, EventArgs e)
        {
            if (appointmentViewsListBox.SelectedIndex == -1) return;

            int selectedIndex = appointmentViewsListBox.SelectedIndex;

            var appointmentView = appointmentViews[selectedIndex];

            using (var formApptViewEdit = new FormAppointmentViewEdit(appointmentView))
            {
                if (formApptViewEdit.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            viewChanged = true;

            FillAppointmentViews();

            if (selectedIndex < appointmentViewsListBox.Items.Count)
            {
                appointmentViewsListBox.SelectedIndex = selectedIndex;
            }
            else
            {
                appointmentViewsListBox.SelectedIndex = -1;
            }
        }

        private void AppointmentViewsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            deleteButton.Enabled = appointmentViewsListBox.SelectedItem != null;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var newAppointmentView = new AppointmentView
            {
                ClinicId = SelectedClinicId,
                ScrollStartTime = new TimeSpan(8, 0, 0)
            };

            using (var formApptViewEdit = new FormAppointmentViewEdit(newAppointmentView))
            {
                if (formApptViewEdit.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            viewChanged = true;

            FillAppointmentViews();

            appointmentViewsListBox.SelectedIndex = appointmentViewsListBox.Items.Count - 1;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (appointmentViewsListBox.SelectedIndex == -1)
            {
                return;
            }

            var appointmentView = appointmentViews[appointmentViewsListBox.SelectedIndex];

            var result =
                MessageBox.Show(
                    "Are you sure you want to delete the selected view?",
                    "Appointment View",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            AppointmentView.Delete(appointmentView.Id);

            appointmentViews.Remove(appointmentView);

            FillAppointmentViews();

            viewChanged = true;
        }

        private void ProcedureColorsButton_Click(object sender, EventArgs e)
        {
            using (var formProcColors = new FormProcApptColors())
            {
                formProcColors.ShowDialog(this);
            }
        }

        private void FormAppointmentViews_FormClosing(object sender, FormClosingEventArgs e)
        {
            int newIncrement = 15;
            if (increment5RadioButton.Checked)
            {
                newIncrement = 5;
            }

            if (increment10RadioButton.Checked)
            {
                newIncrement = 10;
            }

            if (Preference.Update(PreferenceName.AppointmentTimeIncrement, newIncrement))
            {
                // TODO: The Preference class should be updated so that Update() automatically update the local cache...

                CacheManager.Invalidate<Preference>();
            }

            if (viewChanged)
            {
                CacheManager.Invalidate<AppointmentView>();
            }
        }
    }
}
