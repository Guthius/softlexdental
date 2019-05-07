using System;
using System.ComponentModel;

namespace OpenDental.User_Controls.SetupWizard
{
    [ToolboxItem(false)]
    public partial class SetupWizardControlComplete : SetupWizardControl
    {
        public SetupWizardControlComplete(string name)
        {
            InitializeComponent();

            titleLabel.Text = "Congratulations! You have finished setting up your " + name + "!";
            descriptionLabel.Text = "You can always go back through this setup wizard if you need to make any modifications to your " + name + ".";
        }

        void SetupWizardControlComplete_Load(object sender, EventArgs e)
        {
            IsDone = true;
        }
    }
}