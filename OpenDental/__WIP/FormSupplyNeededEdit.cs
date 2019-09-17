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
        void FormSupplyNeededEdit_Load(object sender, EventArgs e)
        {
            dateTextBox.Text = SupplyNeeded.DateAdded.ToShortDateString();
            descriptionTextBox.Text = SupplyNeeded.Description;
        }

        /// <summary>
        /// Deletes the needed supply.
        /// </summary>
        void DeleteButton_Click(object sender, EventArgs e)
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
        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (dateTextBox.errorProvider1.GetError(dateTextBox) != "")
            {
                MessageBox.Show(
                    Translation.Language.PleaseFixDataEntryErrors,
                    Translation.Language.SupplyNeeded,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            SupplyNeeded.DateAdded = PIn.Date(dateTextBox.Text);
            SupplyNeeded.Description = descriptionTextBox.Text;

            if (SupplyNeeded.IsNew)
            {
                SupplyNeeded.Insert(SupplyNeeded);
            }
            else
            {
                SupplyNeeded.Update(SupplyNeeded);
            }

            DialogResult = DialogResult.OK;
        }
    }
}
