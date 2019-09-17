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
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAlertCategorySetup : FormBase
    {
        private readonly List<AlertCategory> internalAlertCategories = new List<AlertCategory>();
        private readonly List<AlertCategory> customAlertCategories = new List<AlertCategory>();

        public FormAlertCategorySetup() => InitializeComponent();

        private void FormAlertCategorySetup_Load(object sender, EventArgs e) => LoadAlertCategories();

        private void LoadAlertCategories()
        {
            customAlertCategories.Clear();
            internalAlertCategories.Clear();

            var alertCategories = AlertCategory.All();
            foreach (var alertCategory in alertCategories)
            {
                if (alertCategory.Locked)
                {
                    internalAlertCategories.Add(alertCategory);
                }
                else
                {
                    customAlertCategories.Add(alertCategory);
                }
            }

            internalAlertCategories.OrderBy(x => x.Name);
            customAlertCategories.OrderBy(x => x.Name);

            FillInternalGrid(internalAlertCategories);
            FillCustomGrid(internalAlertCategories);
        }

        private void FillInternalGrid(IEnumerable<AlertCategory> alertCategories)
        {
            internalGrid.BeginUpdate();
            internalGrid.Columns.Clear();
            internalGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDescription, 100));
            internalGrid.Rows.Clear();

            foreach (var alertCategory in alertCategories)
            {
                var row = new ODGridRow();
                row.Cells.Add(alertCategory.Description);
                row.Tag = alertCategory;

                internalGrid.Rows.Add(row);
            }

            internalGrid.EndUpdate();
        }

        private void FillCustomGrid(IEnumerable<AlertCategory> alertCategories)
        {
            customGrid.BeginUpdate();
            customGrid.Columns.Clear();
            customGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDescription, 100));
            customGrid.Rows.Clear();

            foreach (var alertCategory in alertCategories)
            {
                var row = new ODGridRow();
                row.Cells.Add(alertCategory.Description);
                row.Tag = alertCategory;

                customGrid.Rows.Add(row);
            }

            customGrid.EndUpdate();
        }

        private void InternalGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formAlertCategoryEdit = new FormAlertCategoryEdit(internalAlertCategories[e.Row]))
            {
                if (formAlertCategoryEdit.ShowDialog() == DialogResult.OK)
                {
                    LoadAlertCategories();
                }
            }
        }

        private void CustomGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formAlertCategoryEdit = new FormAlertCategoryEdit(customAlertCategories[e.Row]))
            {
                if (formAlertCategoryEdit.ShowDialog() == DialogResult.OK)
                {
                    LoadAlertCategories();
                }
            }
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            if (internalGrid.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                    "Please select an internal alert category from the list.", 
                    "Alert Category Setup", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            CopyAlertCategory(internalAlertCategories[internalGrid.GetSelectedIndex()]);
        }

        private void DuplicateButton_Click(object sender, EventArgs e)
        {
            if (customGrid.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                   "Please select a custom alert category from the list.",
                   "Alert Category Setup",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information);

                return;
            }

            CopyAlertCategory(customAlertCategories[customGrid.GetSelectedIndex()]);
        }

        private void CopyAlertCategory(AlertCategory alertCategory)
        {
            alertCategory.Locked = false;
            alertCategory.Description += " (Copy)";

            var alertCategoryLinks = AlertCategoryLink.GetByAlertCategory(alertCategory.Id);

            AlertCategory.Insert(alertCategory);

            foreach (var alertCategorylink in alertCategoryLinks)
            {
                alertCategorylink.AlertCategoryId = alertCategory.Id;

                AlertCategoryLink.Insert(alertCategorylink);
            }

            DataValid.SetInvalid(InvalidType.AlertCategories, InvalidType.AlertCategoryLinks);

            LoadAlertCategories();
        }
    }
}
