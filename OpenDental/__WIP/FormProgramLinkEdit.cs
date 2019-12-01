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
using OpenDentBusiness.Bridges;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormProgramLinkEdit : FormBase
    {
        private readonly Program program;

        private readonly Dictionary<BridgePreference, string> activePreferences = 
            new Dictionary<BridgePreference, string>();

        private readonly Dictionary<BridgePreference, string> updatedPreferences = 
            new Dictionary<BridgePreference, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FormProgramLinkEdit"/> class.
        /// </summary>
        public FormProgramLinkEdit(Program program)
        {
            InitializeComponent();

            this.program = program;
        }

        private void FormProgramLinkEdit_Load(object sender, EventArgs e)
        {
            ToolButItems.RefreshCache();

            CacheManager.Invalidate<ProgramPreference>();

            programTextBox.Text = program.TypeName;
            descriptionTextBox.Text = program.Description;
            enabledCheckBox.Checked = program.Enabled;
            noteTextBox.Text = program.Note;

            toolbarsListBox.Items.Clear();

            var toolbars = Enum.GetNames(typeof(ToolBarsAvail));
            foreach (var toolbar in toolbars)
            {
                toolbarsListBox.Items.Add(toolbars);
            }

            var toolButItems = ToolButItems.GetForProgram(program.Id);
            for (int i = 0; i < toolButItems.Count; i++)
            {
                toolbarsListBox.SetSelected((int)toolButItems[i].ToolBar, true);
            }

            FillPreferences();
        }

        /// <summary>
        ///     <para>
        ///         Format the specified <paramref name="value"/> using the information provided by
        ///         the specified bridge preference for display in the preferences grid.
        ///     </para>
        /// </summary>
        /// <param name="bridgePreference">The preference information.</param>
        /// <param name="value">The value of the preference.</param>
        /// <returns>The formatted value of the preference.</returns>
        private static string FormatPreferenceValue(BridgePreference bridgePreference, string value)
        {
            switch (bridgePreference.Type)
            {
                case BridgePreferenceType.Password:
                    Encryption.TryDecrypt(value, out var password);
                    return new string('*', password.Length);

                case BridgePreferenceType.Definition:
                    {
                        if (int.TryParse(value, out var definitionId))
                        {
                            var definition = ProgramPreference.GetDefinition(definitionId, bridgePreference.Key);
                            if (definition == null)
                            {
                                return "";
                            }
                            else if (definition.Hidden)
                            {
                                return definition.Description + " (hidden)";
                            }
                            else
                            {
                                return definition.Description;
                            }
                        }
                    }
                    break;

                case BridgePreferenceType.Boolean:
                    if (bool.TryParse(value, out var result))
                    {
                        return result ? "Yes" : "No";
                    }
                    return "No";
            }

            return value;
        }

        private void FillPreferences()
        {
            activePreferences.Clear();

            preferencesGrid.BeginUpdate();
            preferencesGrid.Columns.Clear();
            preferencesGrid.Columns.Add(new ODGridColumn("Property", 260));
            preferencesGrid.Columns.Add(new ODGridColumn("Value", 130));
            preferencesGrid.Rows.Clear();

            var bridge = BridgeManager.GetByTypeName(program.TypeName);
            if (bridge == null)
            {
                return;
            }

            foreach (var bridgePreference in bridge.Preferences)
            {
                var value = ProgramPreference.GetString(program.Id, bridgePreference.Key);

                activePreferences[bridgePreference] = value;

                var row = new ODGridRow();
                row.Cells.Add(bridgePreference.Description);
                row.Cells.Add(FormatPreferenceValue(bridgePreference, value));
                row.Tag = bridgePreference;

                preferencesGrid.Rows.Add(row);
            }

            preferencesGrid.EndUpdate();
        }

        private void PrefrencesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (preferencesGrid.Rows[e.Row].Tag is BridgePreference bridgePreference)
            {
                if (!activePreferences.TryGetValue(bridgePreference, out var value)) value = "";

                using (var formProgramProperty = new FormProgramPreference(bridgePreference, value))
                {
                    if (formProgramProperty.ShowDialog(this) == DialogResult.OK)
                    {
                        activePreferences[bridgePreference] = updatedPreferences[bridgePreference] = formProgramProperty.Value;
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var result =
                MessageBox.Show(
                    "Delete this program link?", 
                    "Program Link", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            if (!program.IsNew)
            {
                Program.Delete(program.Id);
            }

            DialogResult = DialogResult.OK;
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            program.Description = descriptionTextBox.Text;
            program.Enabled = enabledCheckBox.Checked;
            program.Note = noteTextBox.Text;

            if (program.IsNew)
            {
                Program.Insert(program);
            }
            else
            {
                Program.Update(program);
            }

            foreach (var keyValuePair in updatedPreferences)
            {
                ProgramPreference.Set(
                    program.Id, 
                    keyValuePair.Key.Key, 
                    keyValuePair.Value);
            }

            ToolButItems.DeleteAllForProgram(program.Id);

            for (int i = 0; i < toolbarsListBox.SelectedIndices.Count; i++)
            {
                ToolButItems.Insert(new ToolButItem
                {
                    ProgramNum = program.Id,
                    ButtonText = "",
                    ToolBar = (ToolBarsAvail)toolbarsListBox.SelectedIndices[i]
                });
            }

            DialogResult = DialogResult.OK;
        }
    }
}
