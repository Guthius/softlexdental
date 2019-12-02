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
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAppointmentViewItemEdit : FormBase
    {
        private readonly AppointmentViewItem appointmentViewItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormAppointmentViewItemEdit"/> class.
        /// </summary>
        /// <param name="appointmentViewItem">The appointment view item to edit.</param>
        public FormAppointmentViewItemEdit(AppointmentViewItem appointmentViewItem)
        {
            InitializeComponent();

            this.appointmentViewItem = appointmentViewItem;
        }

        private void FormApptViewItemEdit_Load(object sender, EventArgs e)
        {
            if (appointmentViewItem.AppointmentFieldDefinitionId.HasValue)
            {
                descriptionTextBox.Text = AppointmentFieldDefinition.GetFieldName(appointmentViewItem.AppointmentFieldDefinitionId.Value);
            }
            else
            {
                descriptionTextBox.Text = appointmentViewItem.Description;
            }

            colorPanel.BackColor = appointmentViewItem.Color;

            locationListBox.SelectedIndex = (int)appointmentViewItem.Alignment;
        }

        private void TextColorButton_Click(object sender, EventArgs e)
        {
            using (var colorDialog = new ColorDialog())
            {
                colorDialog.Color = colorPanel.BackColor;
                if (colorDialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                colorPanel.BackColor = colorDialog.Color;
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            appointmentViewItem.Color = colorPanel.BackColor;
            appointmentViewItem.Alignment = (AppointmentViewLocation)locationListBox.SelectedIndex;

            DialogResult = DialogResult.OK;
        }
    }
}
