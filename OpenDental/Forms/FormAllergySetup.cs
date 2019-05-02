using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAllergySetup : FormBase
    {
        List<AllergyDef> allergiesList;
        
        /// <summary>
        /// Gets or sets a value indicating whether the form is in selection mode.
        /// </summary>
        public bool IsSelectionMode { get; set; }

        /// <summary>
        /// Gets or sets the number of the selected allergy.
        /// </summary>
        public long SelectedAllergyDefNum { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormAllergySetup"/> class.
        /// </summary>
        public FormAllergySetup() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FormAllergySetup_Load(object sender, EventArgs e)
        {
            if (IsSelectionMode)
            {
                acceptButton.Visible = true;
                cancelButton.Text = Translation.Language.ButtonCancel;
            }
            LoadAllergies();
        }

        /// <summary>
        /// Fills the allergies grid.
        /// </summary>
        void LoadAllergies()
        {
            allergiesList = AllergyDefs.GetAll(showHiddenCheckBox.Checked);

            allergiesGrid.BeginUpdate();
            allergiesGrid.Columns.Clear();
            allergiesGrid.Columns.Add(new ODGridColumn(Translation.Language.Description, 160));
            allergiesGrid.Columns.Add(new ODGridColumn(Translation.Language.Hidden, 60));

            allergiesGrid.Rows.Clear();
            for (int i = 0; i < allergiesList.Count; i++)
            {
                var row = new ODGridRow();
                row.Cells.Add(allergiesList[i].Description);
                if (allergiesList[i].IsHidden)
                {
                    row.Cells.Add("X");
                }
                else
                {
                    row.Cells.Add("");
                }
                allergiesGrid.Rows.Add(row);
            }
            allergiesGrid.EndUpdate();
        }

        /// <summary>
        /// Reloads the list of allergies when visibility of hidden items is toggled.
        /// </summary>
        void showHiddenCheckBox_CheckedChanged(object sender, EventArgs e) => LoadAllergies();
        
        /// <summary>
        /// Closes the form when in selection mode; otherwise, opens the form to edit the selected allergy.
        /// </summary>
        void allergiesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (IsSelectionMode)
            {
                SelectedAllergyDefNum = allergiesList[e.Row].AllergyDefNum;
                DialogResult = DialogResult.OK;
            }
            else
            {
                using (var formAllergyDefEdit = new FormAllergyDefEdit())
                {
                    formAllergyDefEdit.AllergyDefCur = allergiesList[allergiesGrid.GetSelectedIndex()];
                    if (formAllergyDefEdit.ShowDialog() == DialogResult.OK)
                    {
                        LoadAllergies();
                    }
                }
            }
        }

        /// <summary>
        /// Opens the form to add a new allergy.
        /// </summary>
        void addButton_Click(object sender, EventArgs e)
        {
            using (var formAllergyDefEdit = new FormAllergyDefEdit())
            {
                formAllergyDefEdit.AllergyDefCur = new AllergyDef();
                formAllergyDefEdit.AllergyDefCur.IsNew = true;

                if (formAllergyDefEdit.ShowDialog() == DialogResult.OK)
                {
                    LoadAllergies();
                }
            }
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        void acceptButton_Click(object sender, EventArgs e)
        {
            //Only visible in IsSelectionMode.
            if (allergiesGrid.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                    Translation.Language.AllergySelectAtLeastOne,
                    Translation.Language.AllergySetup, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            SelectedAllergyDefNum = allergiesList[allergiesGrid.GetSelectedIndex()].AllergyDefNum;

            DialogResult = DialogResult.OK;
        }
    }
}