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
    public partial class FormAutoCodeEdit : FormBase
    {
        /// <summary>
        /// Gets or sets the auto code being edited.
        /// </summary>
        public AutoCode AutoCode { get; set; }

        public FormAutoCodeEdit() => InitializeComponent();

        private void FormAutoCodeEdit_Load(object sender, EventArgs e)
        {
            descriptionTextBox.Text = AutoCode.Description;
            hiddenCheckBox.Checked = AutoCode.Hidden;
            lessIntrusiveCheckBox.Checked = AutoCode.LessIntrusive;

            LoadAutoCodeItems();
        }

        private void LoadAutoCodeItems()
        {
            CacheManager.Invalidate<AutoCodeItem>();
            CacheManager.Invalidate<AutoCodeItemCondition>();

            var autoCodeItems = AutoCodeItem.GetByAutoCode(AutoCode.Id);

            foreach (var autoCodeItem in autoCodeItems)
            {
                autoCodeItem.Conditions = 
                    AutoCodeItemCondition.GetByAutoCodeItem(autoCodeItem.Id)
                        .Select(autoCodeItemCondition => autoCodeItemCondition.Condition).ToList();

                CreateListViewItem(autoCodeItem);
            }
        }

        private void CreateListViewItem(AutoCodeItem autoCodeItem)
        {
            var procedureCode = ProcedureCodes.GetProcCode(autoCodeItem.ProcedureCodeId);

            var listViewItem = autoCodeItemsListView.Items.Add(procedureCode.ProcCode);

            listViewItem.SubItems.Add(procedureCode.Descript);

            if (autoCodeItem.Conditions.Count > 0)
            {
                listViewItem.SubItems.Add(string.Join(", ", autoCodeItem.Conditions));
            }
            else
            {
                listViewItem.SubItems.Add("<none>");
            }

            listViewItem.Tag = autoCodeItem;
        }

        private void AutoCodeItemsListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var hitInfo = autoCodeItemsListView.HitTest(e.Location);

            if (hitInfo.Item != null && hitInfo.Item.Tag is AutoCodeItem autoCodeItem)
            {
                using (var formAutoItemEdit = new FormAutoItemEdit())
                {
                    formAutoItemEdit.AutoCodeItem = autoCodeItem;

                    if (formAutoItemEdit.ShowDialog() == DialogResult.OK)
                    {
                        var procedureCode = ProcedureCodes.GetProcCode(autoCodeItem.ProcedureCodeId);

                        hitInfo.Item.SubItems[0].Text = procedureCode.ProcCode;
                        hitInfo.Item.SubItems[1].Text = procedureCode.Descript;
                        hitInfo.Item.SubItems[2].Text =
                            autoCodeItem.Conditions.Count > 0 ?
                                string.Join(", ", autoCodeItem.Conditions) : "<none>";

                        autoCodeItemsListView.Invalidate(
                            autoCodeItemsListView.GetItemRect(
                                hitInfo.Item.Index));
                    }
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            using (var formAutoItemEdit = new FormAutoItemEdit())
            {
                formAutoItemEdit.AutoCodeItem = new AutoCodeItem
                {
                    AutoCodeId = AutoCode.Id
                };

                if (formAutoItemEdit.ShowDialog() == DialogResult.OK)
                {
                    CreateListViewItem(formAutoItemEdit.AutoCodeItem);
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (autoCodeItemsListView.SelectedItems.Count == 0)
            {
                MessageBox.Show(
                    "Please select an item first.",
                    "Auto Code", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            if (autoCodeItemsListView.SelectedItems[0].Tag is AutoCodeItem autoCodeItem)
            {
                AutoCodeItem.Delete(autoCodeItem);

                autoCodeItemsListView.SelectedItems[0].Remove();
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            var description = descriptionTextBox.Text.Trim();
            if (description.Length == 0)
            {
                MessageBox.Show(
                    "The description cannot be blank", 
                    "Auto Code", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            var autoCodeItems = new List<AutoCodeItem>();
            foreach (ListViewItem listViewItem in autoCodeItemsListView.Items)
            {
                if (listViewItem.Tag is AutoCodeItem autoCodeItem)
                {
                    autoCodeItems.Add(autoCodeItem);
                }
            }

            if (autoCodeItems.Count == 0)
            {
                MessageBox.Show(
                    "Must have at least one item in the list.",
                    "Auto Code",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            for (int i = 1; i < autoCodeItems.Count; i++)
            {
                if (autoCodeItems[i].Conditions.Count != autoCodeItems[0].Conditions.Count)
                {
                    MessageBox.Show(
                        "All auto code items must have the same number of conditions.",
                        "Auto Code",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }

            if (autoCodeItems[0].Conditions.Count > 0)
            {
                // Check for duplicate conditions
                foreach (var autoCodeItem in autoCodeItems)
                {
                    foreach (var otherAutoCodeItem in autoCodeItems)
                    {
                        if (autoCodeItem.Id == otherAutoCodeItem.Id) continue;

                        int matches = 0;
                        foreach (var condition in autoCodeItem.Conditions)
                        {
                            if (otherAutoCodeItem.Conditions.Contains(condition))
                            {
                                matches++;
                            }
                        }

                        if (matches == autoCodeItem.Conditions.Count)
                        {
                            MessageBox.Show(
                                "Cannot have two auto code items with duplicate conditions.",
                                "Auto Code",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                            return;
                        }
                    }
                }

                if (!ValidateConditions(autoCodeItems)) return;
            }

            AutoCode.Description = descriptionTextBox.Text;
            AutoCode.Hidden = hiddenCheckBox.Checked;
            AutoCode.LessIntrusive = lessIntrusiveCheckBox.Checked;

            if (AutoCode.IsNew)
            {
                AutoCode.Insert(AutoCode);
            }
            else
            {
                AutoCode.Update(AutoCode);
            }

            foreach (var autoCodeItem in autoCodeItems)
            {
                autoCodeItem.AutoCodeId = AutoCode.Id;
                if (autoCodeItem.IsNew)
                {
                    AutoCodeItem.Insert(autoCodeItem);
                }
                else
                {
                    AutoCodeItem.Update(autoCodeItem);
                }

                AutoCodeItemCondition.DeleteByAutoCodeItem(autoCodeItem.Id);
                foreach (var condition in autoCodeItem.Conditions)
                {
                    AutoCodeItemCondition.Insert(
                        new AutoCodeItemCondition {
                            AutoCodeItemId = autoCodeItem.Id,
                            Condition = condition });
                }
            }

            DialogResult = DialogResult.OK;
        }

        private bool ValidateConditions(List<AutoCodeItem> autoCodeItems)
        {
            bool isAnterior = false;
            bool isPosterior = false;
            bool isMolarOrPremolar = false;
            bool isSurface = false;
            bool isFirstOrEachAdditional = false;
            bool isMaxillaryMandibular = false;
            bool isPrimaryOrPermanent = false;
            bool isPonticOrRetainer = false;

            foreach (var autoCodeItem in autoCodeItems)
            {
                foreach (var condition in autoCodeItem.Conditions)
                {

                    switch (condition)
                    {
                        case AutoCodeItemConditionType.Anterior:
                            isAnterior = true;
                            break;

                        case AutoCodeItemConditionType.Posterior:
                            isPosterior = true;
                            break;

                        case AutoCodeItemConditionType.Premolar:
                        case AutoCodeItemConditionType.Molar:
                            isMolarOrPremolar = true;
                            break;

                        case AutoCodeItemConditionType.OneSurface:
                        case AutoCodeItemConditionType.TwoSurfaces:
                        case AutoCodeItemConditionType.ThreeSurfaces:
                        case AutoCodeItemConditionType.FourSurfaces:
                        case AutoCodeItemConditionType.FiveSurfaces:
                            isSurface = true;
                            break;

                        case AutoCodeItemConditionType.First:
                        case AutoCodeItemConditionType.EachAdditional:
                            isFirstOrEachAdditional = true;
                            break;

                        case AutoCodeItemConditionType.Maxillary:
                        case AutoCodeItemConditionType.Mandibular:
                            isMaxillaryMandibular = true;
                            break;

                        case AutoCodeItemConditionType.Primary:
                        case AutoCodeItemConditionType.Permanent:
                            isPrimaryOrPermanent = true;
                            break;

                        case AutoCodeItemConditionType.Pontic:
                        case AutoCodeItemConditionType.Retainer:
                            isPonticOrRetainer = true;
                            break;
                    }
                }
            }

            if (isPosterior && isMolarOrPremolar)
            {
                MessageBox.Show(
                    "Cannot have both Posterior and Premolar/Molar categories.",
                    "Auto Code",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            if (isAnterior)
            {
                if (!isPosterior && !isMolarOrPremolar)
                {
                    MessageBox.Show(
                        "Anterior condition is present without any corresponding posterior or premolar/molar condition.",
                        "Auto Code",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return false;
                }
            }

            int categories = 0;

            if (isPosterior) categories++;
            if (isMolarOrPremolar) categories++;
            if (isSurface) categories++;
            if (isFirstOrEachAdditional) categories++;
            if (isMaxillaryMandibular)  categories++;
            if (isPrimaryOrPermanent) categories++;
            if (isPonticOrRetainer)  categories++;

            if (categories != autoCodeItems[0].Conditions.Count)
            {
                MessageBox.Show(
                    "When using " + autoCodeItems[0].Conditions.Count + " condition(s), you must use conditions from " + autoCodeItems[0].Conditions.Count + " logical categories. " +
                    "You are using conditions from " + categories + " logical categories.",
                    "Auto Code",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            int requiredAutoCodeItems = 1;

            if (isPosterior) requiredAutoCodeItems *= 2;
            if (isMolarOrPremolar)
            {
                requiredAutoCodeItems *= isPrimaryOrPermanent ? 5 : 3;
            }
            else
            {
                if (isPrimaryOrPermanent)
                {
                    requiredAutoCodeItems *= 2;
                }
            }

            if (isSurface) requiredAutoCodeItems *= 5;
            if (isFirstOrEachAdditional) requiredAutoCodeItems *= 2;
            if (isMaxillaryMandibular) requiredAutoCodeItems *= 2;
            if (isPonticOrRetainer) requiredAutoCodeItems *= 2;

            if (autoCodeItems.Count != requiredAutoCodeItems)
            {
                MessageBox.Show(
                    "For the condition categories you are using, you should have " + requiredAutoCodeItems +  " entries in your list. " +
                    "You have " + autoCodeItems.Count + ".",
                    "Auto Code",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private void FormAutoCodeEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            CacheManager.Invalidate<AutoCodeItem>();
            CacheManager.Invalidate<AutoCodeItemCondition>();
        }
    }
}
