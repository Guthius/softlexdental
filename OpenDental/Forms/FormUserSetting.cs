using OpenDentBusiness;
using System;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormUserSetting : FormBase
    {
        UserPreference suppressLogOffMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormUserSetting"/> class.
        /// </summary>
        public FormUserSetting() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormUserSetting_Load(object sender, EventArgs e)
        {
            suppressLogOffMessage = UserOdPrefs.GetByUserAndFkeyType(Security.CurrentUser.Id, UserPreferenceName.SuppressLogOffMessage).FirstOrDefault();
            if (suppressLogOffMessage != null)
            {
                suppressMessageCheckBox.Checked = true;
            }
        }

        /// <summary>
        /// Saves the preferences and closes the form.
        /// </summary>
        void acceptButton_Click(object sender, EventArgs e)
        {
            if (suppressMessageCheckBox.Checked && suppressLogOffMessage == null)
            {
                UserOdPrefs.Insert(new UserPreference()
                {
                    UserId = Security.CurrentUser.Id,
                    FkeyType = UserPreferenceName.SuppressLogOffMessage
                });
            }
            else if (!suppressMessageCheckBox.Checked && suppressLogOffMessage != null)
            {
                UserOdPrefs.Delete(suppressLogOffMessage.Id);
            }
            DialogResult = DialogResult.OK;
        }
    }
}