/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormEmailAutographEdit : FormBase
    {
        public EmailAutograph Autograph { get; }

        public FormEmailAutographEdit(EmailAutograph emailAutograph)
        {
            InitializeComponent();

            Autograph = emailAutograph;
        }

        void FormEmailTemplateEdit_Load(object sender, EventArgs e)
        {
            descriptionTextBox.Text = Autograph.Description;
            autographTextBox.Text = Autograph.Autograph;
        }

        void AcceptButton_Click(object sender, EventArgs e)
        {
            var autograph = autographTextBox.Text.Trim();
            if (autograph.Length == 0)
            {
                MessageBox.Show(
                    Translation.Language.AutographCannotBeBlank, 
                    Translation.Language.Autograph, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            var description = descriptionTextBox.Text.Trim();
            if (description.Length == 0)
            {
                MessageBox.Show(
                    Translation.Language.DescriptionCannotBeBlank,
                    Translation.Language.Autograph,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            Autograph.Description = description;
            Autograph.Autograph = autograph;

            if (Autograph.IsNew)
            {
                EmailAutograph.Insert(Autograph);
            }
            else
            {
                EmailAutograph.Update(Autograph);
            }

            DialogResult = DialogResult.OK;
        }
    }
}