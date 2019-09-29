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
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAutoCode : FormBase
    {
        private bool changed;
        private List<AutoCode> autoCodes;

        public FormAutoCode() => InitializeComponent();
        
        private void FormAutoCode_Load(object sender, EventArgs e) => LoadAutoCodeList();

        private void FormAutoCode_Closing(object sender, CancelEventArgs e)
        {
            if (changed)
            {
                CacheManager.InvalidateEverywhere<AutoCode>();
            }

            DialogResult = DialogResult.OK;
        }

        private void LoadAutoCodeList()
        {
            CacheManager.Invalidate<AutoCode>();

            autoCodes = AutoCode.All();

            autoCodesListBox.Items.Clear();
            foreach (var autoCode in autoCodes)
            {
                autoCodesListBox.Items.Add(autoCode);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            using (var formAutoCodeEdit = new FormAutoCodeEdit())
            {
                formAutoCodeEdit.AutoCode = new AutoCode();

                if (formAutoCodeEdit.ShowDialog() == DialogResult.OK)
                {
                    autoCodesListBox.Items.Add(formAutoCodeEdit.AutoCode);

                    changed = true;
                }
            }
        }

        private void AutoCodesListBox_DoubleClick(object sender, EventArgs e)
        {
            if (autoCodesListBox.SelectedItem is AutoCode autoCode)
            {
                using (var formAutoCodeEdit = new FormAutoCodeEdit())
                {
                    formAutoCodeEdit.AutoCode = autoCode;

                    if (formAutoCodeEdit.ShowDialog() == DialogResult.OK)
                    {
                        autoCodesListBox.Invalidate(
                            autoCodesListBox.GetItemRectangle(
                                autoCodesListBox.SelectedIndex));

                        changed = true;
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (autoCodesListBox.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "You must first select a row", 
                    "Auto Codes",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            if (autoCodesListBox.SelectedItem is AutoCode autoCode)
            {
                try
                {
                    AutoCode.Delete(autoCode);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        exception.Message,
                        "Auto Codes",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                autoCodesListBox.Items.Remove(autoCode);

                changed = true;
            }
        }

        private void CloseButton_Click(object sender, EventArgs e) => Close();
    }
}
