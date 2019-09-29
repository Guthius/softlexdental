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
    public partial class FormAutoItemEdit : FormBase
    {
        /// <summary>
        /// Gets or sets the <see cref="AutoCodeItem"/> being edited.
        /// </summary>
        public AutoCodeItem AutoCodeItem { get; set; }

        /// <summary>
        /// Gets all selected conditions.
        /// </summary>
        public IEnumerable<AutoCodeItemConditionType> Conditions
        {
            get
            {
                foreach (AutoCodeItemConditionType condition in conditionsListBox.SelectedItems)
                {
                    yield return condition;
                }
            }
        }

        public FormAutoItemEdit() => InitializeComponent();

        private void FormAutoItemEdit_Load(object sender, EventArgs e)
        {
            CacheManager.Invalidate<AutoCodeItemCondition>();

            codeTextBox.Text = ProcedureCodes.GetStringProcCode(AutoCodeItem.ProcedureCodeId);

            LoadConditions();
        }

        private void LoadConditions()
        {
            conditionsListBox.Items.Clear();
            foreach (var value in Enum.GetValues(typeof(AutoCodeItemConditionType)))
            {
                conditionsListBox.Items.Add(value);
            }

            var autoCodeConditions = AutoCodeItemCondition.GetByAutoCodeItem(AutoCodeItem.Id);

            foreach (var autoCodeCondition in autoCodeConditions)
            {
                var index = conditionsListBox.Items.IndexOf(autoCodeCondition.Condition);
                if (index != -1)
                {
                    conditionsListBox.SetSelected(index, true);
                }
            }
        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            using (var formProcCodes = new FormProcCodes())
            {
                formProcCodes.IsSelectionMode = true;

                if (formProcCodes.ShowDialog() == DialogResult.Cancel)
                {
                    codeTextBox.Text = ProcedureCodes.GetStringProcCode(AutoCodeItem.ProcedureCodeId);

                    return;
                }
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            var code = codeTextBox.Text.Trim();
            if (code.Length == 0)
            {
                MessageBox.Show(
                    "Code cannot be left blank.",
                    "Auto Code Item", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            AutoCodeItem.ProcedureCodeId = ProcedureCodes.GetCodeNum(codeTextBox.Text);
            AutoCodeItem.Conditions = Conditions.ToList();

            DialogResult = DialogResult.OK;
        }
    }
}
