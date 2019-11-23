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
    public partial class FormSupplyNeededEdit : FormBase
    {
        /// <summary>
        /// Gets the needed supply.
        /// </summary>
        public SupplyNeeded SupplyNeeded { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSupplyNeededEdit"/> class.
        /// </summary>
        public FormSupplyNeededEdit(SupplyNeeded supplyNeeded)
        {
            InitializeComponent();

            SupplyNeeded = supplyNeeded;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        private void FormSupplyNeededEdit_Load(object sender, EventArgs e)
        {
            dateTextBox.Text = SupplyNeeded.DateAdded.ToShortDateString();
            descriptionTextBox.Text = SupplyNeeded.Description;
        }

        /// <summary>
        /// Deletes the needed supply.
        /// </summary>
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (SupplyNeeded.IsNew)
            {
                DialogResult = DialogResult.Cancel;
            }

            SupplyNeeded.Delete(SupplyNeeded);

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Saves the needed supply and closes the form.
        /// </summary>
        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (!DateTime.TryParse(dateTextBox.Text, out var dateAdded))
            {
                MessageBox.Show(
                    "Please enter a valid date.",
                    Translation.Language.SupplyNeeded,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                dateTextBox.Focus();

                return;
            }

            SupplyNeeded.DateAdded = dateAdded;
            SupplyNeeded.Description = descriptionTextBox.Text;
            SupplyNeeded.Save();

            DialogResult = DialogResult.OK;
        }
    }
}
