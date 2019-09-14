/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
using System;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental
{
    public partial class FormEmailMasterTemplate : FormBase
    {
        public FormEmailMasterTemplate() => InitializeComponent();

        void FormEmailSetupMasterTemplate_Load(object sender, EventArgs e)
        {
            masterTextBox.Text = Preference.GetString(PreferenceName.EmailMasterTemplate);
        }

        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (Preference.Update(PreferenceName.EmailMasterTemplate, masterTextBox.Text))
            {
                DataValid.SetInvalid(InvalidType.Prefs);
            }
            DialogResult = DialogResult.OK;
        }
    }
}