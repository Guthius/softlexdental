namespace OpenDental
{
    partial class FormEmailTemplateEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailTemplateEdit));
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.subjectLabel = new System.Windows.Forms.Label();
            this.bodyTextBox = new System.Windows.Forms.TextBox();
            this.bodyLabel = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.bodyFieldsButton = new System.Windows.Forms.Button();
            this.subjectTextBox = new System.Windows.Forms.TextBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.subjectFieldsButton = new System.Windows.Forms.Button();
            this.attachmentButton = new System.Windows.Forms.Button();
            this.attachmentsContextMenu = new System.Windows.Forms.ContextMenu();
            this.openMenuItem = new System.Windows.Forms.MenuItem();
            this.renameMenuItem = new System.Windows.Forms.MenuItem();
            this.removeMenuItem = new System.Windows.Forms.MenuItem();
            this.attachmentsListView = new System.Windows.Forms.ListView();
            this.attachmentsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(761, 538);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(645, 538);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 5;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // subjectLabel
            // 
            this.subjectLabel.AutoSize = true;
            this.subjectLabel.Location = new System.Drawing.Point(58, 82);
            this.subjectLabel.Name = "subjectLabel";
            this.subjectLabel.Size = new System.Drawing.Size(46, 15);
            this.subjectLabel.TabIndex = 0;
            this.subjectLabel.Text = "Subject";
            this.subjectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bodyTextBox
            // 
            this.bodyTextBox.AcceptsReturn = true;
            this.bodyTextBox.AcceptsTab = true;
            this.bodyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bodyTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.bodyTextBox.Location = new System.Drawing.Point(110, 108);
            this.bodyTextBox.Multiline = true;
            this.bodyTextBox.Name = "bodyTextBox";
            this.bodyTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.bodyTextBox.Size = new System.Drawing.Size(761, 280);
            this.bodyTextBox.TabIndex = 3;
            // 
            // bodyLabel
            // 
            this.bodyLabel.AutoSize = true;
            this.bodyLabel.Location = new System.Drawing.Point(70, 111);
            this.bodyLabel.Name = "bodyLabel";
            this.bodyLabel.Size = new System.Drawing.Size(34, 15);
            this.bodyLabel.TabIndex = 0;
            this.bodyLabel.Text = "Body";
            this.bodyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(37, 53);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(67, 15);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Description";
            this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bodyFieldsButton
            // 
            this.bodyFieldsButton.Location = new System.Drawing.Point(226, 19);
            this.bodyFieldsButton.Name = "bodyFieldsButton";
            this.bodyFieldsButton.Size = new System.Drawing.Size(110, 25);
            this.bodyFieldsButton.TabIndex = 4;
            this.bodyFieldsButton.Text = "Body Fields";
            this.bodyFieldsButton.Click += new System.EventHandler(this.BodyFieldsButton_Click);
            // 
            // subjectTextBox
            // 
            this.subjectTextBox.AcceptsTab = true;
            this.subjectTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subjectTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.subjectTextBox.Location = new System.Drawing.Point(110, 79);
            this.subjectTextBox.Name = "subjectTextBox";
            this.subjectTextBox.Size = new System.Drawing.Size(761, 23);
            this.subjectTextBox.TabIndex = 2;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.AcceptsTab = true;
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.descriptionTextBox.Location = new System.Drawing.Point(110, 50);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(761, 23);
            this.descriptionTextBox.TabIndex = 1;
            // 
            // subjectFieldsButton
            // 
            this.subjectFieldsButton.Location = new System.Drawing.Point(110, 19);
            this.subjectFieldsButton.Name = "subjectFieldsButton";
            this.subjectFieldsButton.Size = new System.Drawing.Size(110, 25);
            this.subjectFieldsButton.TabIndex = 7;
            this.subjectFieldsButton.Text = "Subject Fields";
            this.subjectFieldsButton.Click += new System.EventHandler(this.SubjectFieldsButton_Click);
            // 
            // attachmentButton
            // 
            this.attachmentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.attachmentButton.Location = new System.Drawing.Point(110, 538);
            this.attachmentButton.Name = "attachmentButton";
            this.attachmentButton.Size = new System.Drawing.Size(150, 30);
            this.attachmentButton.TabIndex = 9;
            this.attachmentButton.Text = "Attach...";
            this.attachmentButton.Click += new System.EventHandler(this.AttachmentButton_Click);
            // 
            // attachmentsContextMenu
            // 
            this.attachmentsContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.openMenuItem,
            this.renameMenuItem,
            this.removeMenuItem});
            // 
            // openMenuItem
            // 
            this.openMenuItem.Index = 0;
            this.openMenuItem.Text = "Open";
            this.openMenuItem.Click += new System.EventHandler(this.OpenMenuItem_Click);
            // 
            // renameMenuItem
            // 
            this.renameMenuItem.Index = 1;
            this.renameMenuItem.Text = "Rename";
            this.renameMenuItem.Click += new System.EventHandler(this.RenameMenuItem_Click);
            // 
            // removeMenuItem
            // 
            this.removeMenuItem.Index = 2;
            this.removeMenuItem.Text = "Remove";
            this.removeMenuItem.Click += new System.EventHandler(this.RemoveMenuItem_Click);
            // 
            // attachmentsListView
            // 
            this.attachmentsListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.attachmentsListView.HideSelection = false;
            this.attachmentsListView.Location = new System.Drawing.Point(110, 429);
            this.attachmentsListView.Name = "attachmentsListView";
            this.attachmentsListView.Size = new System.Drawing.Size(761, 103);
            this.attachmentsListView.TabIndex = 44;
            this.attachmentsListView.UseCompatibleStateImageBehavior = false;
            this.attachmentsListView.View = System.Windows.Forms.View.List;
            this.attachmentsListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.AttachmentsListView_MouseDoubleClick);
            this.attachmentsListView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AttachmentsListView_MouseUp);
            // 
            // attachmentsLabel
            // 
            this.attachmentsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.attachmentsLabel.AutoSize = true;
            this.attachmentsLabel.Location = new System.Drawing.Point(107, 411);
            this.attachmentsLabel.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.attachmentsLabel.Name = "attachmentsLabel";
            this.attachmentsLabel.Size = new System.Drawing.Size(75, 15);
            this.attachmentsLabel.TabIndex = 47;
            this.attachmentsLabel.Text = "Attachments";
            this.attachmentsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormEmailTemplateEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.ClientSize = new System.Drawing.Size(884, 581);
            this.Controls.Add(this.attachmentsLabel);
            this.Controls.Add(this.attachmentsListView);
            this.Controls.Add(this.attachmentButton);
            this.Controls.Add(this.subjectFieldsButton);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.subjectTextBox);
            this.Controls.Add(this.bodyFieldsButton);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.bodyLabel);
            this.Controls.Add(this.bodyTextBox);
            this.Controls.Add(this.subjectLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(900, 620);
            this.Name = "FormEmailTemplateEdit";
            this.ShowInTaskbar = false;
            this.Text = "E-mail Template";
            this.Load += new System.EventHandler(this.FormEmailTemplateEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Label subjectLabel;
        private System.Windows.Forms.TextBox bodyTextBox;
        private System.Windows.Forms.Label bodyLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Button bodyFieldsButton;
        private System.Windows.Forms.TextBox subjectTextBox;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Button attachmentButton;
        private System.Windows.Forms.Button subjectFieldsButton;
        private System.Windows.Forms.ContextMenu attachmentsContextMenu;
        private System.Windows.Forms.MenuItem openMenuItem;
        private System.Windows.Forms.MenuItem renameMenuItem;
        private System.Windows.Forms.MenuItem removeMenuItem;
        private System.Windows.Forms.ListView attachmentsListView;
        private System.Windows.Forms.Label attachmentsLabel;
    }
}