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
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSupplyEdit : FormBase
    {
        private List<Definition> supplyCategoriesList;
        private long categoryInitialVal;

        /// <summary>
        /// Gets or sets the supply.
        /// </summary>
        public Supply Supply { get; set; }

        public List<Supplier> Suppliers { get; }

        public FormSupplyEdit() => InitializeComponent();

        private void FormSupplyEdit_Load(object sender, EventArgs e)
        {
            supplierTextBox.Text = OpenDentBusiness.Suppliers.GetName(Suppliers, Supply.SupplierId);

            supplyCategoriesList = Definition.GetByCategory(DefinitionCategory.SupplyCats);;
            for (int i = 0; i < supplyCategoriesList.Count; i++)
            {
                categoryComboBox.Items.Add(supplyCategoriesList[i].Description);
                if (Supply.CategoryId == supplyCategoriesList[i].Id)
                {
                    categoryComboBox.SelectedIndex = i;
                }
            }

            if (categoryComboBox.SelectedIndex == -1) categoryComboBox.SelectedIndex = 0;

            categoryInitialVal = Supply.CategoryId;
            catalogNumberTextBox.Text = Supply.CatalogNumber;
            descriptionTextBox.Text = Supply.Description;

            if (Supply.LevelDesired != 0)
            {
                levelDesiredTextBox.Text = Supply.LevelDesired.ToString();
            }

            if (Supply.LevelOnHand != 0)
            {
                levelOnHandTextBox.Text = Supply.LevelOnHand.ToString();
            }

            if (Supply.Price != 0)
            {
                priceTextBox.Value = Supply.Price;
            }

            hiddenCheckBox.Checked = Supply.Hidden;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (Supply.IsNew)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            var result =
                MessageBox.Show(
                    Translation.Language.ConfirmDelete,
                    Translation.Language.Supply, 
                    MessageBoxButtons.OKCancel, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            try
            {
                Supplies.DeleteObject(Supply);
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    Translation.Language.Supply, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            Supply = null;

            DialogResult = DialogResult.OK;
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (levelDesiredTextBox.errorProvider1.GetError(levelDesiredTextBox) != "")
            {
                MessageBox.Show(
                    Translation.Language.PleaseFixDataEntryErrors,
                    Translation.Language.Supply,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (descriptionTextBox.Text == "")
            {
                MessageBox.Show(
                    Translation.Language.PleaseEnterADescription,
                    Translation.Language.Supply,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            Supply.CategoryId = supplyCategoriesList[categoryComboBox.SelectedIndex].Id;
            Supply.CatalogNumber = catalogNumberTextBox.Text;
            Supply.Description = descriptionTextBox.Text;
            Supply.LevelDesired = PIn.Float(levelDesiredTextBox.Text);
            Supply.LevelOnHand = PIn.Float(levelOnHandTextBox.Text);
            Supply.Price = priceTextBox.Value;
            Supply.Hidden = hiddenCheckBox.Checked;

            if (Supply.CategoryId != categoryInitialVal)
            {
                Supply.SortOrder = int.MaxValue;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
