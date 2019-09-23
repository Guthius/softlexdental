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
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAlertCategoryEdit : FormBase
    {
        private List<string> alertTypes;
        private List<AlertCategoryLink> oldAlertCategoryLinks;

        public AlertCategory Category { get; }

        public FormAlertCategoryEdit(AlertCategory category)
        {
            InitializeComponent();

            Category = category;
        }

        private void FormAlertCategoryEdit_Load(object sender, EventArgs e)
        {
            descriptionTextBox.Text = Category.Description;

            alertTypes = 
                typeof(AlertType)
                    .GetFields(BindingFlags.Public | BindingFlags.Static |  BindingFlags.FlattenHierarchy)
                    .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                    .Select(fi => (string)fi.GetRawConstantValue())
                    .ToList();

            if (Category.Locked)
            {
                descriptionTextBox.Enabled = false;
                deleteButton.Visible = false;
                acceptButton.Enabled = false;
            }

            oldAlertCategoryLinks = AlertCategoryLink.GetByAlertCategory(Category.Id);

            LoadAlertTypes();
        }

        private void LoadAlertTypes()
        {
            alertTypesListBox.Items.Clear();

            var categoryAlertTypes = oldAlertCategoryLinks.Select(x => x.AlertType).ToList();
            foreach (var alertType in alertTypes)
            {
                int index = alertTypesListBox.Items.Add(alertType);

                alertTypesListBox.SetSelected(index, categoryAlertTypes.Contains(alertType));
            }
        }

        private void AlertTypesListBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (Category.Locked)
            {
                LoadAlertTypes();

                MessageBox.Show(
                    "You can only edit custom alert categories.", 
                    "Alert Category", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var result =
                MessageBox.Show(
                    Translation.Language.ConfirmDelete,
                    "Alert Category",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No) return;

            AlertCategory.Delete(Category.Id);

            DataValid.SetInvalid(InvalidType.AlertCategories, InvalidType.AlertCategories);

            DialogResult = DialogResult.OK;
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            Category.Description = descriptionTextBox.Text;

            var newAlertCategoryLinks = new List<AlertCategoryLink>();
            foreach (string alertType in alertTypesListBox.SelectedItems)
            {
                newAlertCategoryLinks.Add(
                    new AlertCategoryLink
                    {
                        AlertCategoryId = Category.Id,
                        AlertType = alertType
                    });
            }

            AlertCategoryLink.Synchronize(newAlertCategoryLinks, oldAlertCategoryLinks);
            AlertCategory.Update(Category);

            DataValid.SetInvalid(InvalidType.AlertCategoryLinks, InvalidType.AlertCategories);

            DialogResult = DialogResult.OK;
        }
    }
}
