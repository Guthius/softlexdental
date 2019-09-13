using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormUserSetting : FormBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormUserSetting"/> class.
        /// </summary>
        public FormUserSetting() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormUserSetting_Load(object sender, EventArgs e)
        {
            suppressMessageCheckBox.Checked = UserPreference.GetBool(Security.CurrentUser.Id, UserPreferenceName.SuppressLogOffMessage);
        }

        /// <summary>
        /// Saves the preferences and closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            UserPreference.Update(Security.CurrentUser.Id, UserPreferenceName.SuppressLogOffMessage, suppressMessageCheckBox.Checked);

            DialogResult = DialogResult.OK;
        }
    }
}
