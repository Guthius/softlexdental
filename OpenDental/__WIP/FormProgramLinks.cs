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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormProgramLinks : FormBase
    {
        private bool changed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormProgramLinks"/> class.
        /// </summary>
        public FormProgramLinks() => InitializeComponent();

        /// <summary>
        ///     <para>
        ///         Adds the specified <paramref name="program"/> to the programs grid.
        ///     </para>
        /// </summary>
        /// <param name="program">The program to add to the grid.</param>
        private void AddProgramToGrid(Program program)
        {
            var row = new ODGridRow();

            Color color = Color.FromArgb(230, 255, 238);

            row.BackColor = program.Enabled ? color : row.BackColor;
            row.Cells.Add(program.Enabled ? "X" : "");
            row.Cells.Add(program.Description);
            row.Tag = program;

            programsGrid.Rows.Add(row);
        }

        /// <summary>
        ///     <para>
        ///         Fills the programs grid.
        ///     </para>
        /// </summary>
        private void FillPrograms()
        {
            programsGrid.BeginUpdate();
            programsGrid.Columns.Clear();
            programsGrid.Columns.Add(new ODGridColumn("Enabled", 55, HorizontalAlignment.Center));
            programsGrid.Columns.Add(new ODGridColumn("Program Name", -1));
            programsGrid.Rows.Clear();

            foreach (var program in Program.All)
            {
                AddProgramToGrid(program);
            }

            programsGrid.EndUpdate();
        }

        private void FormProgramLinks_Load(object sender, EventArgs e) => FillPrograms();

        private void AddButton_Click(object sender, EventArgs e)
        {
            // TODO: Add a form to allow the user to select the program to link...

            //var program = new Program();

            //using (var formProgramLinkEdit = new FormProgramLinkEdit(program))
            //{
            //    if (formProgramLinkEdit.ShowDialog(this) != DialogResult.OK)
            //    {
            //        return;
            //    }

            //    changed = true;

            //    AddProgramToGrid(program);
            //}
        }

        private void ProgramsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            var program = programsGrid.SelectedTag<Program>();

            using (var formProgramLinkEdit = new FormProgramLinkEdit(program))
            {
                if (formProgramLinkEdit.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            changed = true;

            // TODO: Add a method to the IBridge interface for opening the setup UI.

            //DialogResult dResult = DialogResult.None;
            //Program program = programsGrid.SelectedTag<Program>();
            //switch (program.TypeName)
            //{
            //    case "UAppoint":
            //        FormUAppoint FormU = new FormUAppoint();
            //        FormU.ProgramCur = program;
            //        dResult = FormU.ShowDialog();
            //        break;

            //    case "eRx":
            //        FormErxSetup FormES = new FormErxSetup();
            //        dResult = FormES.ShowDialog();
            //        break;

            //    case "Mountainside":
            //        FormMountainside FormM = new FormMountainside();
            //        FormM.ProgramCur = program;
            //        dResult = FormM.ShowDialog();
            //        break;

            //    case "PayConnect":
            //        FormPayConnectSetup fpcs = new FormPayConnectSetup();
            //        dResult = fpcs.ShowDialog();
            //        break;

            //    case "Podium":
            //        FormPodiumSetup FormPS = new FormPodiumSetup();
            //        dResult = FormPS.ShowDialog();
            //        break;

            //    case "Xcharge":
            //        FormXchargeSetup fxcs = new FormXchargeSetup();
            //        dResult = fxcs.ShowDialog();
            //        break;

            //    case "FHIR":
            //        FormFHIRSetup FormFS = new FormFHIRSetup();
            //        dResult = FormFS.ShowDialog();
            //        break;

            //    case "Transworld":
            //        FormTransworldSetup FormTs = new FormTransworldSetup();
            //        dResult = FormTs.ShowDialog();
            //        break;

            //    case "PaySimple":
            //        FormPaySimpleSetup formPS = new FormPaySimpleSetup();
            //        dResult = formPS.ShowDialog();
            //        break;

            //    case "AvaTax":
            //        FormAvaTax formAT = new FormAvaTax();
            //        formAT.ProgramCur = program;
            //        dResult = formAT.ShowDialog();
            //        break;
            //}
        }

        private void CloseButton_Click(object sender, EventArgs e) => Close();

        private void FormProgramLinks_Closing(object sender, CancelEventArgs e)
        {
            if (changed)
            {
                CacheManager.Invalidate<Program>();

                // TODO: Proper caching...

                DataValid.SetInvalid(InvalidType.ToolBut);
            }
        }
    }
}