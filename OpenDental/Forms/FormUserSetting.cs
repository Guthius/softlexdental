using OpenDentBusiness;
using System;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormUserSetting : FormBase
    {
        UserOdPref suppressLogOffMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormUserSetting"/> class.
        /// </summary>
        public FormUserSetting() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormUserSetting_Load(object sender, EventArgs e)
        {
            suppressLogOffMessage = UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum, UserOdFkeyType.SuppressLogOffMessage).FirstOrDefault();
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
                UserOdPrefs.Insert(new UserOdPref()
                {
                    UserNum = Security.CurUser.UserNum,
                    FkeyType = UserOdFkeyType.SuppressLogOffMessage
                });
            }
            else if (!suppressMessageCheckBox.Checked && suppressLogOffMessage != null)
            {
                UserOdPrefs.Delete(suppressLogOffMessage.UserOdPrefNum);
            }
            DialogResult = DialogResult.OK;
        }
    }
}