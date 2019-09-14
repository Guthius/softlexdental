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
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormApptBreak : FormBase
    {
        readonly Appointment appointment;

        /// <summary>
        /// Gets the selected break type.
        /// </summary>
        public ApptBreakSelection Selection { get; private set; } = ApptBreakSelection.None;

        /// <summary>
        /// Gets the selected procedure code.
        /// </summary>
        public ProcedureCode ProcedureCode => 
            radioMissed.Checked ?
                    ProcedureCodes.GetProcCode("D9986") :
                    ProcedureCodes.GetProcCode("D9987");

        /// <summary>
        /// Initializes a new instance of the <see cref="FormApptBreak"/> class.
        /// </summary>
        /// <param name="appointment">The appointment that was broken.</param>
        public FormApptBreak(Appointment appointment)
        {
            InitializeComponent();

            this.appointment = appointment;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormApptBreak_Load(object sender, EventArgs e)
        {
            var brokenApptProcs = (BrokenApptProcedure)Preference.GetInt(PreferenceName.BrokenApptProcedure);

            radioMissed.Enabled = brokenApptProcs.In(BrokenApptProcedure.Missed, BrokenApptProcedure.Both);
            radioCancelled.Enabled = brokenApptProcs.In(BrokenApptProcedure.Cancelled, BrokenApptProcedure.Both);

            if (radioMissed.Enabled && !radioCancelled.Enabled)
            {
                radioMissed.Checked = true;
            }
            else if (!radioMissed.Enabled && radioCancelled.Enabled)
            {
                radioMissed.Checked = true;
            }
        }

        /// <summary>
        /// Checks whether a valid selection was mode.
        /// </summary>
        /// <returns>True if the selection is valid; otherwise, false.</returns>
        bool ValidateSelection()
        {
            if (!radioMissed.Checked && !radioCancelled.Checked)
            {
                MessageBox.Show(
                    "Please select a broken procedure type.",
                    "Broken Appointment", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return false;
            }
            return true;
        }

        /// <summary>
        /// Asks the user if they want to offer this opening to patients on the ASAP list.
        /// </summary>
        void PromptTextASAPList()
        {
            // Check whether the webscheduler ASAP function is enabled and whether there are appointments on the ASAP list.
            if (!Preference.GetBool(PreferenceName.WebSchedAsapEnabled) ||  Appointments.RefreshASAP(0, 0, appointment.ClinicNum, new List<ApptStatus>()).Count == 0)
            {
                return;
            }

            var result =
                MessageBox.Show(
                    "Text patients on the ASAP List and offer them this opening?",
                    "Broken Appointment", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            DateTime slotStart = AppointmentL.DateSelected.Date; // Midnight
            DateTime slotEnd = AppointmentL.DateSelected.Date.AddDays(1); // Midnight tomorrow

            // Loop through all other appts in the op to find a slot that will not overlap.
            var appointmentsList = Appointments.GetAppointmentsForOpsByPeriod(new List<long> { appointment.Op }, appointment.AptDateTime);
            foreach (var otherAppt in appointmentsList.Where(x => x.AptNum != appointment.AptNum))
            {
                var dateEndApt = otherAppt.AptDateTime.AddMinutes(otherAppt.Pattern.Length * 5);
                if (dateEndApt.Between(slotStart, appointment.AptDateTime))
                {
                    slotStart = dateEndApt;
                }
                if (otherAppt.AptDateTime.Between(appointment.AptDateTime, slotEnd))
                {
                    slotEnd = otherAppt.AptDateTime;
                }
            }

            slotStart = ODMathLib.Max(slotStart, appointment.AptDateTime.AddHours(-1));
            slotEnd = ODMathLib.Min(slotEnd, appointment.AptDateTime.AddHours(3));

            using (var formAsap = new FormASAP(appointment.AptDateTime, slotStart, slotEnd, appointment.Op))
            {
                formAsap.ShowDialog();
            }
        }

        /// <summary>
        /// Move the appointment to the unscheduled list.
        /// </summary>
        void UnscheduledListButton_Click(object sender, EventArgs e)
        {
            if (ValidateSelection())
            {
                PromptTextASAPList();

                Selection = ApptBreakSelection.Unsched;
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Move the appointment to the pinboard.
        /// </summary>
        void PinboardButton_Click(object sender, EventArgs e)
        {
            if (ValidateSelection())
            {
                PromptTextASAPList();

                Selection = ApptBreakSelection.Pinboard;
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Leave the appointment in the appointment book.
        /// </summary>
        void AppointmentBookButton_Click(object sender, EventArgs e)
        {
            if (ValidateSelection())
            {
                Selection = ApptBreakSelection.ApptBook;
                DialogResult = DialogResult.OK;
            }
        }
    }

    public enum ApptBreakSelection
    {
        None,
        Unsched,
        Pinboard,
        ApptBook
    }
}