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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormEmailEdit : FormBase
    {
        public string Body;

        public FormEmailEdit() => InitializeComponent();

        void FormEmailEdit_Load(object sender, EventArgs e)
        {
            bodyTextBox.Text = Body;
            bodyTextBox.Focus();
            bodyTextBox.SelectionStart = 0;
            bodyTextBox.SelectionLength = 0;
            bodyTextBox.ScrollToCaret();
        }

        void SetupButton_Click(object sender, EventArgs e)
        {
            using (var formEmailMasterTemplate = new FormEmailMasterTemplate())
            {
                formEmailMasterTemplate.ShowDialog();
                if (formEmailMasterTemplate.DialogResult != DialogResult.OK)
                {
                    return;
                }
            }
        }

        void ExternalLinkButton_Click(object sender, EventArgs e)
        {
            using (var formExternalLink = new FormExternalLink())
            {
                formExternalLink.ShowDialog();

                int selectionStart = bodyTextBox.SelectionStart;
                if (formExternalLink.DialogResult != DialogResult.OK)
                {
                    return;
                }

                bodyTextBox.SelectedText = "<a href=\"" + formExternalLink.URL + "\">" + formExternalLink.DisplayText + "</a>";
                bodyTextBox.SelectionLength = 0;
                if (formExternalLink.URL == "" && formExternalLink.DisplayText == "")
                {
                    bodyTextBox.SelectionStart = selectionStart + 11;
                }

                bodyTextBox.Focus();
            }
        }

        void Heading1Button_Click(object sender, EventArgs e) => MarkupL.AddTag("<h1>", "</h1>", bodyTextBox);

        void Heading2Button_Click(object sender, EventArgs e) => MarkupL.AddTag("<h2>", "</h2>", bodyTextBox);

        void Heading3Button_Click(object sender, EventArgs e) => MarkupL.AddTag("<h3>", "</h3>", bodyTextBox);

        void CutButton_Click(object sender, EventArgs e)
        {
            bodyTextBox.Cut();
            bodyTextBox.Focus();
        }

        void CopyButton_Click(object sender, EventArgs e)
        {
            bodyTextBox.Copy();
            bodyTextBox.Focus();
        }

        void PasteButton_Click(object sender, EventArgs e)
        {
            bodyTextBox.Paste();
            bodyTextBox.Focus();
        }

        void UndoButton_Click(object sender, EventArgs e)
        {
            bodyTextBox.Undo();
            bodyTextBox.Focus();
        }

        void BodyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\t')
            {
                bodyTextBox.SelectedText = "   ";

                e.Handled = true;
            }
        }

        void AcceptButton_Click(object sender, EventArgs e)
        {
            Body = bodyTextBox.Text.Trim();

            DialogResult = DialogResult.OK;
        }
    }
}