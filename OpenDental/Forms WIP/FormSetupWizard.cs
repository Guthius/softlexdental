using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSetupWizard : FormBase
    {
        List<SetupWizard.SetupWizardStep> setupWizardsList;
        List<ODGridRow> setupRows = new List<ODGridRow>();
        Dictionary<ODSetupCategory, ODGridRow> setupCategoryRows = new Dictionary<ODSetupCategory, ODGridRow>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSetupWizard"/> class.
        /// </summary>
        public FormSetupWizard() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FormSetupWizard_Load(object sender, EventArgs e) => LoadWizards();
        
        /// <summary>
        /// Loads the list of wizards.
        /// </summary>
        void LoadWizardsList()
        {
            setupWizardsList = new List<SetupWizard.SetupWizardStep>
            {
                new SetupWizard.RegKeySetup(),
                new SetupWizard.FeatureSetup(),
                new SetupWizard.ProvSetup(),
                new SetupWizard.EmployeeSetup(),
                new SetupWizard.FeeSchedSetup()
            };

            setupWizardsList.Add(new SetupWizard.ClinicSetup());
            setupWizardsList.Add(new SetupWizard.OperatorySetup());
            setupWizardsList.Add(new SetupWizard.PracticeSetup());
            setupWizardsList.Add(new SetupWizard.PrinterSetup());
            setupWizardsList.Add(new SetupWizard.DefinitionSetup());

            //_listSetupWizItems.Add(new SetupWizard.ScheduleSetup());
            //_listSetupWizItems.Add(new SetupWizard.CarrierSetup());
            //_listSetupWizItems.Add(new SetupWizard.ClearinghouseSetup());
            //Add more here.
        }

        /// <summary>
        /// Loads the list of wizards and populates the grid.
        /// </summary>
        void LoadWizards()
        {
            LoadWizardsList();

            wizardsGrid.BeginUpdate();
            wizardsGrid.Columns.Clear();
            wizardsGrid.Columns.Add(new ODGridColumn("Setup Item", 250));
            wizardsGrid.Columns.Add(new ODGridColumn("Status", 100, HorizontalAlignment.Center));
            wizardsGrid.Columns.Add(new ODGridColumn("?", 35, HorizontalAlignment.Center) { ImageList = gridImageList });
            wizardsGrid.Rows.Clear();

            var listRows = CreateRows();
            foreach (ODGridRow row in listRows)
            {
                wizardsGrid.Rows.Add(row);
            }
            wizardsGrid.EndUpdate();
        }

        /// <summary>
        /// Creates a grid row for the specified category (if one doesn't exist).
        /// </summary>
        /// <param name="setupCategory"></param>
        void CreateCategoryRow(ODSetupCategory setupCategory)
        {
            if (!setupCategoryRows.TryGetValue(setupCategory, out var gridRow))
            {
                gridRow = new ODGridRow();
                gridRow.Cells.Add("\r\n" + setupCategory.GetDescription() + "\r\n");
                gridRow.Cells.Add("");
                gridRow.Cells.Add("");
                gridRow.Tag = setupCategory;
                gridRow.Bold = true;

                setupRows.Add(gridRow);
                setupCategoryRows.Add(setupCategory, gridRow);
            }
        }

        /// <summary>
        /// Creates all rows for the grid.
        /// </summary>
        /// <returns>The grid rows.</returns>
        List<ODGridRow> CreateRows()
        {
            setupRows.Clear();
            setupCategoryRows.Clear();

            foreach (var setupWizard in setupWizardsList)
            {
                CreateCategoryRow(setupWizard.Category);

                var gridRow = new ODGridRow();
                gridRow.Cells.Add("     " + setupWizard.Name);
                gridRow.Cells.Add(setupWizard.Status.GetDescription());
                gridRow.Cells[gridRow.Cells.Count - 1].CellColor = SetupWizard.GetColor(setupWizard.Status);
                gridRow.Cells.Add("0");
                gridRow.Tag = setupWizard;

                setupRows.Add(gridRow);
            }

            foreach (var category in setupCategoryRows)
            {
                var completed =
                    setupRows
                        .Where(row => row.Tag is SetupWizard.SetupWizardStep setupWizard && setupWizard.Category == category.Key)
                        .All(row => row.Tag is SetupWizard.SetupWizardStep setupWizard && (setupWizard.Status == ODSetupStatus.Complete || setupWizard.Status == ODSetupStatus.Optional));

                if (completed)
                {
                    category.Value.Cells[1].Text = "\r\n" + "Complete";
                    category.Value.Cells[1].CellColor = SetupWizard.GetColor(ODSetupStatus.Complete);
                }
                else
                {
                    category.Value.Cells[1].Text = "\r\n" + "Needs Attention";
                    category.Value.Cells[1].CellColor = SetupWizard.GetColor(ODSetupStatus.NeedsAttention);
                }
                        
            }

            return setupRows;
        }

        /// <summary>
        /// If the user clicks on a category row, select all rows that are part of that category.
        /// </summary>
        void WizardsGrid_CellClick(object sender, ODGridClickEventArgs e)
        {
            if (wizardsGrid.Rows[e.Row].Tag is ODSetupCategory setupCategory)
            {
                for (int i = 0; i < wizardsGrid.Rows.Count; i++)
                {
                    var gridRow = wizardsGrid.Rows[i];
                    if (gridRow.Tag is SetupWizard.SetupWizardStep setupWizard && setupWizard.Category == setupCategory)
                    {
                        wizardsGrid.SetSelected(i, true);
                    }
                }
                return;
            }
            else
            {
                if (wizardsGrid.Rows[e.Row].Tag is SetupWizard.SetupWizardStep setupWizard && wizardsGrid.Columns[e.Column].ImageList != null)
                {
                    MessageBox.Show(
                        setupWizard.Description,
                        Translation.Language.Setup,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Opens the setup wizards when the user double clicks on a wizard or category.
        /// </summary>
        void WizardsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            var tempWizardsList = new List<SetupWizard.SetupWizardStep>();

            if (wizardsGrid.Rows[e.Row].Tag is ODSetupCategory setupCategory)
            {
                foreach (var setupWizClass in setupWizardsList)
                {
                    if (setupWizClass.Category == setupCategory)
                    {
                        tempWizardsList.Add(new SetupWizard.SetupIntro(setupWizClass.Name, setupWizClass.Description));
                        tempWizardsList.Add(setupWizClass);
                        tempWizardsList.Add(new SetupWizard.SetupComplete(setupWizClass.Name));
                    }
                }
            }
            else if(wizardsGrid.Rows[e.Row].Tag is SetupWizard.SetupWizardStep setupWizard)
            { 
                tempWizardsList.Add(new SetupWizard.SetupIntro(setupWizard.Name, setupWizard.Description));
                tempWizardsList.Add(setupWizard);
                tempWizardsList.Add(new SetupWizard.SetupComplete(setupWizard.Name));
            }

            if (tempWizardsList.Count > 0)
            {
                using (var formSetupWizardProgress = new FormSetupWizardProgress(tempWizardsList, false))
                {
                    formSetupWizardProgress.ShowDialog();
                }

                LoadWizards();
            }
        }

        /// <summary>
        /// Opens all the wizards.
        /// </summary>
        void AllButton_Click(object sender, EventArgs e)
        {
            var tempWizardsList = new List<SetupWizard.SetupWizardStep>();

            foreach (var setupWizard in setupWizardsList)
            {
                tempWizardsList.Add(new SetupWizard.SetupIntro(setupWizard.Name, setupWizard.Description));
                tempWizardsList.Add(setupWizard);
                tempWizardsList.Add(new SetupWizard.SetupComplete(setupWizard.Name));
            }

            using (var formSetupWizardProgress = new FormSetupWizardProgress(tempWizardsList, true))
            {
                formSetupWizardProgress.ShowDialog();
            }

            LoadWizards();
        }
    }
}