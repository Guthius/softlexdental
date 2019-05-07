using System;
using System.ComponentModel;

namespace OpenDental.User_Controls.SetupWizard
{
    [ToolboxItem(false)]
    public partial class SetupWizardControlIntro : SetupWizardControl
    {
        public SetupWizardControlIntro(string name, string description)
        {
            InitializeComponent();

            titleLabel.Text = "Let's set up your " + name + "...";
            descriptionLabel.Text = description + "\r\n\r\nIf you do not want to set up your " + name + " at this time, click 'Skip' below.";
        }

        void SetupWizardControlIntro_Load(object sender, EventArgs e)
        {
            IsDone = true;
        }
    }
}