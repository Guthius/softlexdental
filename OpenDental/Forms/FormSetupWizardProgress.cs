using OpenDental.User_Controls.SetupWizard;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSetupWizardProgress : FormBase
    {
        readonly List<SetupWizard.SetupWizardStep> setupWizardsList;
        int setupWizardIndex = 0;
        readonly bool setupAll;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSetupWizardProgress"/> class.
        /// </summary>
        /// <param name="setupWizardsList"></param>
        /// <param name="setupAll"></param>
        public FormSetupWizardProgress(List<SetupWizard.SetupWizardStep> setupWizardsList, bool setupAll)
        {
            InitializeComponent();

            this.setupAll = setupAll;
            this.setupWizardsList = setupWizardsList;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormSetupWizardProgress_Load(object sender, EventArgs e)
        {
            SetCurrentWizardControl(setupWizardsList[setupWizardIndex].Control);

            nextButton.Focus();
        }

        /// <summary>
        /// Switches the specified control as the active wizard control.
        /// </summary>
        /// <param name="wizardControl">The control to set active.</param>
        void SetCurrentWizardControl(SetupWizardControl wizardControl)
        {
            for (int i = wizardPanel.Controls.Count - 1; i > -1; i--)
            {
                wizardPanel.Controls.RemoveAt(i);
            }
            wizardPanel.Controls.Add(wizardControl);

            wizardControl.Dock = DockStyle.Fill;

            backButton.Enabled = setupWizardIndex > 0;

            titleLabel.Text =
                string.Format(
                    Translation.Language.SetupTitle,
                    setupWizardsList[setupWizardIndex].Name);
        }

        /// <summary>
        /// Any validation should be done here.
        /// </summary>
        bool StepValidate() => setupWizardsList[setupWizardIndex].Control.Validate();

        /// <summary>
        /// Any conditional relational setup should be done here.
        /// Eg, clinic setup should be added if the user the user is setting up "All" and they checked "Clinics" when setting up Basic Features.
        /// </summary>
        void StepDone()
        {
            setupWizardsList[setupWizardIndex].Control.Done();
            if (!setupAll)
            {
                return;
            }

            if (setupWizardsList[setupWizardIndex].GetType() != typeof(SetupWizard.FeatureSetup))
            {
                SetupWizard.ClinicSetup clinicSetup = new SetupWizard.ClinicSetup();

                // If clinics got enabled but there is no clinic setup item, add it.
                if (Preferences.HasClinicsEnabled && setupWizardsList.Where(x => x.Name == clinicSetup.Name).Count() == 0)
                {
                    int endCat = setupWizardIndex;
                    for (int i = setupWizardIndex; i < setupWizardsList.Count; i++)
                    {
                        if (setupWizardsList[i].GetType() == typeof(SetupWizard.ProvSetup))
                        {
                            endCat += 2;
                            break;
                        }
                        endCat++;
                    }
                    setupWizardsList.Insert(endCat++, new SetupWizard.SetupIntro(clinicSetup.Name, clinicSetup.Description));
                    setupWizardsList.Insert(endCat++, clinicSetup);
                    setupWizardsList.Insert(endCat, new SetupWizard.SetupComplete(clinicSetup.Name));
                }
                // Otherwise, if clinics got disabled and there is a clinic setup item, remove it.
                else if (!Preferences.HasClinicsEnabled && setupWizardsList.Where(x => x.Name == clinicSetup.Name).Count() != 0)
                {
                    setupWizardsList.RemoveAll(x => x.Name == clinicSetup.Name);
                }
            }
        }

        /// <summary>
        /// Move back to the previous wizard.
        /// </summary>
        void BackButton_Click(object sender, EventArgs e) => SetCurrentWizardControl(setupWizardsList[--setupWizardIndex].Control);

        /// <summary>
        /// Move to the next wizard.
        /// </summary>
        void NextButton_Click(object sender, EventArgs e)
        {
            if (!StepValidate()) return;
            
            if (!setupWizardsList[setupWizardIndex].Control.IsDone)
            {
                MessageBox.Show(
                    string.Format(
                        Translation.Language.SetupStepIncomplete, 
                        setupWizardsList[setupWizardIndex].Control.Error),
                    Translation.Language.Setup, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            StepDone();
            if (setupWizardsList.Count - 1 < ++setupWizardIndex)
            {
                MessageBox.Show(
                    Translation.Language.SetupComplete,
                    Translation.Language.Setup,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                Close();

                return;
            }

            SetCurrentWizardControl(setupWizardsList[setupWizardIndex].Control);
        }

        /// <summary>
        /// Skips to the list wizard, this should usually be a wizard indicating setup completion.
        /// </summary>
        void SkipButton_Click(object sender, EventArgs e)
        {
            for (int i = setupWizardIndex; i < setupWizardsList.Count; i++)
            {
                if (setupWizardsList[i].GetType() == typeof(SetupWizard.SetupComplete))
                {
                    setupWizardIndex++;
                    break;
                }
                setupWizardIndex++;
            }

            if (setupWizardsList.Count - 1 < setupWizardIndex)
            {
                MessageBox.Show(
                    Translation.Language.SetupComplete,
                    Translation.Language.Setup, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                Close();

                return;
            }

            SetCurrentWizardControl(setupWizardsList[setupWizardIndex].Control);
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        void CloseButton_Click(object sender, EventArgs e) => Close();
    }
}