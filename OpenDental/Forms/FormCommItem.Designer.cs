namespace OpenDental
{
    partial class FormCommItem
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCommItem));
            this.editAutoNoteButton = new System.Windows.Forms.Button();
            this.clearNoteButton = new System.Windows.Forms.Button();
            this.userPrefsButton = new System.Windows.Forms.Button();
            this.autoNoteButton = new System.Windows.Forms.Button();
            this.savedManuallyLabel = new System.Windows.Forms.Label();
            this.endNowButton = new System.Windows.Forms.Button();
            this.endTextBox = new System.Windows.Forms.TextBox();
            this.endLabel = new System.Windows.Forms.Label();
            this.startNowButton = new System.Windows.Forms.Button();
            this.signatureBoxWrapper = new OpenDental.UI.SignatureBoxWrapper();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.userLabel = new System.Windows.Forms.Label();
            this.patientTextBox = new System.Windows.Forms.TextBox();
            this.patientLabel = new System.Windows.Forms.Label();
            this.noteTextBox = new OpenDental.ODtextBox();
            this.sendOrReceivedListBox = new System.Windows.Forms.ListBox();
            this.sendOrReceivedLabel = new System.Windows.Forms.Label();
            this.modeListBox = new System.Windows.Forms.ListBox();
            this.modeLabel = new System.Windows.Forms.Label();
            this.startTextBox = new System.Windows.Forms.TextBox();
            this.typeListBox = new System.Windows.Forms.ListBox();
            this.noteLabel = new System.Windows.Forms.Label();
            this.deleteButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.typeLabel = new System.Windows.Forms.Label();
            this.startLabel = new System.Windows.Forms.Label();
            this.autoSaveTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // editAutoNoteButton
            // 
            this.editAutoNoteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editAutoNoteButton.Location = new System.Drawing.Point(339, 222);
            this.editAutoNoteButton.Name = "editAutoNoteButton";
            this.editAutoNoteButton.Size = new System.Drawing.Size(100, 25);
            this.editAutoNoteButton.TabIndex = 20;
            this.editAutoNoteButton.Text = "Edit Auto Note";
            this.editAutoNoteButton.Click += new System.EventHandler(this.editAutoNoteButton_Click);
            // 
            // clearNoteButton
            // 
            this.clearNoteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearNoteButton.Location = new System.Drawing.Point(551, 222);
            this.clearNoteButton.Name = "clearNoteButton";
            this.clearNoteButton.Size = new System.Drawing.Size(100, 25);
            this.clearNoteButton.TabIndex = 22;
            this.clearNoteButton.Text = "Clear";
            this.clearNoteButton.UseVisualStyleBackColor = true;
            this.clearNoteButton.Click += new System.EventHandler(this.clearNoteButton_Click);
            // 
            // userPrefsButton
            // 
            this.userPrefsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.userPrefsButton.Location = new System.Drawing.Point(571, 18);
            this.userPrefsButton.Name = "userPrefsButton";
            this.userPrefsButton.Size = new System.Drawing.Size(80, 25);
            this.userPrefsButton.TabIndex = 6;
            this.userPrefsButton.Text = "User Prefs";
            this.userPrefsButton.Visible = false;
            this.userPrefsButton.Click += new System.EventHandler(this.userPrefsButton_Click);
            // 
            // autoNoteButton
            // 
            this.autoNoteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.autoNoteButton.Location = new System.Drawing.Point(445, 222);
            this.autoNoteButton.Name = "autoNoteButton";
            this.autoNoteButton.Size = new System.Drawing.Size(100, 25);
            this.autoNoteButton.TabIndex = 21;
            this.autoNoteButton.Text = "Auto Note";
            this.autoNoteButton.Click += new System.EventHandler(this.autoNoteButton_Click);
            // 
            // savedManuallyLabel
            // 
            this.savedManuallyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.savedManuallyLabel.ForeColor = System.Drawing.SystemColors.Highlight;
            this.savedManuallyLabel.Location = new System.Drawing.Point(129, 538);
            this.savedManuallyLabel.Name = "savedManuallyLabel";
            this.savedManuallyLabel.Size = new System.Drawing.Size(290, 30);
            this.savedManuallyLabel.TabIndex = 28;
            this.savedManuallyLabel.Text = "Saved";
            this.savedManuallyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.savedManuallyLabel.Visible = false;
            // 
            // endNowButton
            // 
            this.endNowButton.Location = new System.Drawing.Point(307, 76);
            this.endNowButton.Name = "endNowButton";
            this.endNowButton.Size = new System.Drawing.Size(50, 25);
            this.endNowButton.TabIndex = 12;
            this.endNowButton.Text = "Now";
            this.endNowButton.Click += new System.EventHandler(this.endNowButton_Click);
            // 
            // endTextBox
            // 
            this.endTextBox.Location = new System.Drawing.Point(100, 77);
            this.endTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.endTextBox.Name = "endTextBox";
            this.endTextBox.Size = new System.Drawing.Size(205, 23);
            this.endTextBox.TabIndex = 11;
            // 
            // endLabel
            // 
            this.endLabel.AutoSize = true;
            this.endLabel.Location = new System.Drawing.Point(67, 80);
            this.endLabel.Name = "endLabel";
            this.endLabel.Size = new System.Drawing.Size(27, 15);
            this.endLabel.TabIndex = 10;
            this.endLabel.Text = "End";
            this.endLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // startNowButton
            // 
            this.startNowButton.Location = new System.Drawing.Point(307, 47);
            this.startNowButton.Name = "startNowButton";
            this.startNowButton.Size = new System.Drawing.Size(50, 25);
            this.startNowButton.TabIndex = 9;
            this.startNowButton.Text = "Now";
            this.startNowButton.Click += new System.EventHandler(this.startNowButton_Click);
            // 
            // signatureBoxWrapper
            // 
            this.signatureBoxWrapper.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.signatureBoxWrapper.BackColor = System.Drawing.SystemColors.ControlDark;
            this.signatureBoxWrapper.Location = new System.Drawing.Point(100, 416);
            this.signatureBoxWrapper.Name = "signatureBoxWrapper";
            this.signatureBoxWrapper.SignatureMode = OpenDental.UI.SignatureBoxWrapper.SigMode.Default;
            this.signatureBoxWrapper.Size = new System.Drawing.Size(364, 81);
            this.signatureBoxWrapper.TabIndex = 24;
            this.signatureBoxWrapper.UserSig = null;
            this.signatureBoxWrapper.SignatureChanged += new System.EventHandler(this.signatureBoxWrapper_SignatureChanged);
            // 
            // userTextBox
            // 
            this.userTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.userTextBox.Location = new System.Drawing.Point(449, 19);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.ReadOnly = true;
            this.userTextBox.Size = new System.Drawing.Size(120, 23);
            this.userTextBox.TabIndex = 5;
            // 
            // userLabel
            // 
            this.userLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(413, 23);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(30, 15);
            this.userLabel.TabIndex = 4;
            this.userLabel.Text = "User";
            this.userLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // patientTextBox
            // 
            this.patientTextBox.Location = new System.Drawing.Point(100, 19);
            this.patientTextBox.Name = "patientTextBox";
            this.patientTextBox.ReadOnly = true;
            this.patientTextBox.Size = new System.Drawing.Size(205, 23);
            this.patientTextBox.TabIndex = 3;
            // 
            // patientLabel
            // 
            this.patientLabel.AutoSize = true;
            this.patientLabel.Location = new System.Drawing.Point(50, 22);
            this.patientLabel.Name = "patientLabel";
            this.patientLabel.Size = new System.Drawing.Size(44, 15);
            this.patientLabel.TabIndex = 2;
            this.patientLabel.Text = "Patient";
            this.patientLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // noteTextBox
            // 
            this.noteTextBox.AcceptsTab = true;
            this.noteTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noteTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.noteTextBox.DetectLinksEnabled = false;
            this.noteTextBox.DetectUrls = false;
            this.noteTextBox.Location = new System.Drawing.Point(100, 250);
            this.noteTextBox.Name = "noteTextBox";
            this.noteTextBox.QuickPasteType = OpenDentBusiness.QuickPasteType.CommLog;
            this.noteTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.noteTextBox.Size = new System.Drawing.Size(551, 160);
            this.noteTextBox.TabIndex = 23;
            this.noteTextBox.Text = "";
            this.noteTextBox.TextChanged += new System.EventHandler(this.ClearSignature_Event);
            // 
            // sendOrReceivedListBox
            // 
            this.sendOrReceivedListBox.ItemHeight = 15;
            this.sendOrReceivedListBox.Location = new System.Drawing.Point(312, 128);
            this.sendOrReceivedListBox.Name = "sendOrReceivedListBox";
            this.sendOrReceivedListBox.Size = new System.Drawing.Size(140, 49);
            this.sendOrReceivedListBox.TabIndex = 18;
            this.sendOrReceivedListBox.SelectedValueChanged += new System.EventHandler(this.ClearSignature_Event);
            // 
            // sendOrReceivedLabel
            // 
            this.sendOrReceivedLabel.AutoSize = true;
            this.sendOrReceivedLabel.Location = new System.Drawing.Point(309, 110);
            this.sendOrReceivedLabel.Name = "sendOrReceivedLabel";
            this.sendOrReceivedLabel.Size = new System.Drawing.Size(94, 15);
            this.sendOrReceivedLabel.TabIndex = 17;
            this.sendOrReceivedLabel.Text = "Sent or Received";
            this.sendOrReceivedLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // modeListBox
            // 
            this.modeListBox.ItemHeight = 15;
            this.modeListBox.Location = new System.Drawing.Point(226, 128);
            this.modeListBox.Name = "modeListBox";
            this.modeListBox.Size = new System.Drawing.Size(80, 94);
            this.modeListBox.TabIndex = 16;
            this.modeListBox.SelectedIndexChanged += new System.EventHandler(this.ClearSignature_Event);
            // 
            // modeLabel
            // 
            this.modeLabel.AutoSize = true;
            this.modeLabel.Location = new System.Drawing.Point(223, 110);
            this.modeLabel.Name = "modeLabel";
            this.modeLabel.Size = new System.Drawing.Size(38, 15);
            this.modeLabel.TabIndex = 15;
            this.modeLabel.Text = "Mode";
            this.modeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // startTextBox
            // 
            this.startTextBox.Location = new System.Drawing.Point(100, 48);
            this.startTextBox.Name = "startTextBox";
            this.startTextBox.Size = new System.Drawing.Size(205, 23);
            this.startTextBox.TabIndex = 8;
            this.startTextBox.TextChanged += new System.EventHandler(this.ClearSignature_Event);
            // 
            // typeListBox
            // 
            this.typeListBox.ItemHeight = 15;
            this.typeListBox.Location = new System.Drawing.Point(100, 128);
            this.typeListBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.typeListBox.Name = "typeListBox";
            this.typeListBox.Size = new System.Drawing.Size(120, 94);
            this.typeListBox.TabIndex = 14;
            this.typeListBox.SelectedIndexChanged += new System.EventHandler(this.ClearSignature_Event);
            // 
            // noteLabel
            // 
            this.noteLabel.AutoSize = true;
            this.noteLabel.Location = new System.Drawing.Point(97, 232);
            this.noteLabel.Name = "noteLabel";
            this.noteLabel.Size = new System.Drawing.Size(33, 15);
            this.noteLabel.TabIndex = 19;
            this.noteLabel.Text = "Note";
            this.noteLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(13, 538);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 27;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(541, 538);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(425, 538);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 0;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(97, 110);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(32, 15);
            this.typeLabel.TabIndex = 13;
            this.typeLabel.Text = "Type";
            this.typeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // startLabel
            // 
            this.startLabel.AutoSize = true;
            this.startLabel.Location = new System.Drawing.Point(25, 51);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(69, 15);
            this.startLabel.TabIndex = 7;
            this.startLabel.Text = "Date / Time";
            this.startLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // autoSaveTimer
            // 
            this.autoSaveTimer.Interval = 10000;
            this.autoSaveTimer.Tick += new System.EventHandler(this.autoSaveTimer_Tick);
            // 
            // FormCommItem
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(664, 581);
            this.Controls.Add(this.editAutoNoteButton);
            this.Controls.Add(this.clearNoteButton);
            this.Controls.Add(this.userPrefsButton);
            this.Controls.Add(this.autoNoteButton);
            this.Controls.Add(this.savedManuallyLabel);
            this.Controls.Add(this.endNowButton);
            this.Controls.Add(this.endTextBox);
            this.Controls.Add(this.endLabel);
            this.Controls.Add(this.startNowButton);
            this.Controls.Add(this.signatureBoxWrapper);
            this.Controls.Add(this.userTextBox);
            this.Controls.Add(this.userLabel);
            this.Controls.Add(this.patientTextBox);
            this.Controls.Add(this.patientLabel);
            this.Controls.Add(this.noteTextBox);
            this.Controls.Add(this.sendOrReceivedListBox);
            this.Controls.Add(this.sendOrReceivedLabel);
            this.Controls.Add(this.modeListBox);
            this.Controls.Add(this.modeLabel);
            this.Controls.Add(this.startTextBox);
            this.Controls.Add(this.typeListBox);
            this.Controls.Add(this.noteLabel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.typeLabel);
            this.Controls.Add(this.startLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(680, 600);
            this.Name = "FormCommItem";
            this.ShowInTaskbar = false;
            this.Text = "Communication Item";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCommItem_FormClosing);
            this.Load += new System.EventHandler(this.FormCommItem_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button editAutoNoteButton;
        private System.Windows.Forms.Button clearNoteButton;
        private System.Windows.Forms.Button userPrefsButton;
        private System.Windows.Forms.Button autoNoteButton;
        private System.Windows.Forms.Label savedManuallyLabel;
        private System.Windows.Forms.Button endNowButton;
        private System.Windows.Forms.TextBox endTextBox;
        private System.Windows.Forms.Label endLabel;
        private System.Windows.Forms.Button startNowButton;
        private UI.SignatureBoxWrapper signatureBoxWrapper;
        private System.Windows.Forms.TextBox userTextBox;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.TextBox patientTextBox;
        private System.Windows.Forms.Label patientLabel;
        private ODtextBox noteTextBox;
        private System.Windows.Forms.ListBox sendOrReceivedListBox;
        private System.Windows.Forms.Label sendOrReceivedLabel;
        private System.Windows.Forms.ListBox modeListBox;
        private System.Windows.Forms.Label modeLabel;
        private System.Windows.Forms.TextBox startTextBox;
        private System.Windows.Forms.ListBox typeListBox;
        private System.Windows.Forms.Label noteLabel;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.Label startLabel;
        private System.Windows.Forms.Timer autoSaveTimer;
    }
}