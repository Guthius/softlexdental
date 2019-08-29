namespace OpenDental
{
    partial class FormEmailInbox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailInbox));
            this.messageGrid = new OpenDental.UI.ODGrid();
            this.markNotReadButton = new System.Windows.Forms.Button();
            this.markReadButton = new System.Windows.Forms.Button();
            this.changePatientButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.composeButton = new System.Windows.Forms.Button();
            this.replyButton = new System.Windows.Forms.Button();
            this.searchGroupBox = new System.Windows.Forms.GroupBox();
            this.toTextBox = new OpenDental.ValidDate();
            this.showHiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.toLabel = new System.Windows.Forms.Label();
            this.clearButton = new System.Windows.Forms.Button();
            this.searchButton = new System.Windows.Forms.Button();
            this.showAttachmentsOnlyCheckBox = new System.Windows.Forms.CheckBox();
            this.subjectLabel = new System.Windows.Forms.Label();
            this.subjectTextBox = new System.Windows.Forms.TextBox();
            this.emailAddressLabel = new System.Windows.Forms.Label();
            this.emailAddessTextBox = new System.Windows.Forms.TextBox();
            this.fromTextBox = new OpenDental.ValidDate();
            this.fromLabel = new System.Windows.Forms.Label();
            this.pickPatientButton = new System.Windows.Forms.Button();
            this.patientLabel = new System.Windows.Forms.Label();
            this.patientTextBox = new System.Windows.Forms.TextBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.replyAllButton = new System.Windows.Forms.Button();
            this.forwardButton = new System.Windows.Forms.Button();
            this.setupButton = new System.Windows.Forms.Button();
            this.messageViewTreeView = new System.Windows.Forms.TreeView();
            this.emailAddressComboBox = new System.Windows.Forms.ComboBox();
            this.searchGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // messageGrid
            // 
            this.messageGrid.AllowSortingByColumn = true;
            this.messageGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.messageGrid.EditableEnterMovesDown = false;
            this.messageGrid.HasAddButton = false;
            this.messageGrid.HasDropDowns = false;
            this.messageGrid.HasMultilineHeaders = false;
            this.messageGrid.HScrollVisible = false;
            this.messageGrid.Location = new System.Drawing.Point(219, 125);
            this.messageGrid.Name = "messageGrid";
            this.messageGrid.ScrollValue = 0;
            this.messageGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.messageGrid.Size = new System.Drawing.Size(696, 503);
            this.messageGrid.TabIndex = 140;
            this.messageGrid.Title = "Inbox";
            this.messageGrid.TitleVisible = true;
            this.messageGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.MessageGrid_CellDoubleClick);
            this.messageGrid.CellClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.MessageGrid_CellClick);
            // 
            // markNotReadButton
            // 
            this.markNotReadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.markNotReadButton.Enabled = false;
            this.markNotReadButton.Image = global::OpenDental.Properties.Resources.IconEmail;
            this.markNotReadButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.markNotReadButton.Location = new System.Drawing.Point(921, 464);
            this.markNotReadButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.markNotReadButton.Name = "markNotReadButton";
            this.markNotReadButton.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.markNotReadButton.Size = new System.Drawing.Size(130, 30);
            this.markNotReadButton.TabIndex = 8;
            this.markNotReadButton.Text = "Mark Unread";
            this.markNotReadButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.markNotReadButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.markNotReadButton.Click += new System.EventHandler(this.MarkNotReadButton_Click);
            // 
            // markReadButton
            // 
            this.markReadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.markReadButton.Enabled = false;
            this.markReadButton.Image = global::OpenDental.Properties.Resources.IconEmailOpen;
            this.markReadButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.markReadButton.Location = new System.Drawing.Point(921, 428);
            this.markReadButton.Name = "markReadButton";
            this.markReadButton.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.markReadButton.Size = new System.Drawing.Size(130, 30);
            this.markReadButton.TabIndex = 7;
            this.markReadButton.Text = "Mark Read";
            this.markReadButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.markReadButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.markReadButton.Click += new System.EventHandler(this.MarkReadButton_Click);
            // 
            // changePatientButton
            // 
            this.changePatientButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.changePatientButton.Enabled = false;
            this.changePatientButton.Image = global::OpenDental.Properties.Resources.IconUserGo;
            this.changePatientButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.changePatientButton.Location = new System.Drawing.Point(921, 392);
            this.changePatientButton.Name = "changePatientButton";
            this.changePatientButton.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.changePatientButton.Size = new System.Drawing.Size(130, 30);
            this.changePatientButton.TabIndex = 6;
            this.changePatientButton.Text = "Change Patient";
            this.changePatientButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.changePatientButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.changePatientButton.Click += new System.EventHandler(this.ChangePatientButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(921, 517);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.deleteButton.Size = new System.Drawing.Size(130, 30);
            this.deleteButton.TabIndex = 9;
            this.deleteButton.Text = "Delete";
            this.deleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // composeButton
            // 
            this.composeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.composeButton.Image = global::OpenDental.Properties.Resources.IconEmailAdd;
            this.composeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.composeButton.Location = new System.Drawing.Point(921, 231);
            this.composeButton.Name = "composeButton";
            this.composeButton.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.composeButton.Size = new System.Drawing.Size(130, 30);
            this.composeButton.TabIndex = 4;
            this.composeButton.Text = "Compose";
            this.composeButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.composeButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.composeButton.Click += new System.EventHandler(this.ComposeButton_Click);
            // 
            // replyButton
            // 
            this.replyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.replyButton.Enabled = false;
            this.replyButton.Image = global::OpenDental.Properties.Resources.IconEmailReply;
            this.replyButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.replyButton.Location = new System.Drawing.Point(921, 267);
            this.replyButton.Name = "replyButton";
            this.replyButton.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.replyButton.Size = new System.Drawing.Size(130, 30);
            this.replyButton.TabIndex = 5;
            this.replyButton.Text = "Reply";
            this.replyButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.replyButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.replyButton.Click += new System.EventHandler(this.ReplyButton_Click);
            // 
            // searchGroupBox
            // 
            this.searchGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchGroupBox.Controls.Add(this.toTextBox);
            this.searchGroupBox.Controls.Add(this.showHiddenCheckBox);
            this.searchGroupBox.Controls.Add(this.toLabel);
            this.searchGroupBox.Controls.Add(this.clearButton);
            this.searchGroupBox.Controls.Add(this.searchButton);
            this.searchGroupBox.Controls.Add(this.showAttachmentsOnlyCheckBox);
            this.searchGroupBox.Controls.Add(this.subjectLabel);
            this.searchGroupBox.Controls.Add(this.subjectTextBox);
            this.searchGroupBox.Controls.Add(this.emailAddressLabel);
            this.searchGroupBox.Controls.Add(this.emailAddessTextBox);
            this.searchGroupBox.Controls.Add(this.fromTextBox);
            this.searchGroupBox.Controls.Add(this.fromLabel);
            this.searchGroupBox.Controls.Add(this.pickPatientButton);
            this.searchGroupBox.Controls.Add(this.patientLabel);
            this.searchGroupBox.Controls.Add(this.patientTextBox);
            this.searchGroupBox.Location = new System.Drawing.Point(13, 19);
            this.searchGroupBox.Name = "searchGroupBox";
            this.searchGroupBox.Size = new System.Drawing.Size(1038, 100);
            this.searchGroupBox.TabIndex = 2;
            this.searchGroupBox.TabStop = false;
            this.searchGroupBox.Text = "Search Criteria";
            // 
            // toTextBox
            // 
            this.toTextBox.Location = new System.Drawing.Point(488, 37);
            this.toTextBox.Name = "toTextBox";
            this.toTextBox.Size = new System.Drawing.Size(70, 23);
            this.toTextBox.TabIndex = 5;
            // 
            // showHiddenCheckBox
            // 
            this.showHiddenCheckBox.AutoSize = true;
            this.showHiddenCheckBox.Location = new System.Drawing.Point(254, 75);
            this.showHiddenCheckBox.Name = "showHiddenCheckBox";
            this.showHiddenCheckBox.Size = new System.Drawing.Size(95, 19);
            this.showHiddenCheckBox.TabIndex = 174;
            this.showHiddenCheckBox.Text = "Show hidden";
            this.showHiddenCheckBox.UseVisualStyleBackColor = true;
            // 
            // toLabel
            // 
            this.toLabel.AutoSize = true;
            this.toLabel.Location = new System.Drawing.Point(485, 19);
            this.toLabel.Name = "toLabel";
            this.toLabel.Size = new System.Drawing.Size(19, 15);
            this.toLabel.TabIndex = 166;
            this.toLabel.Text = "To";
            // 
            // clearButton
            // 
            this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearButton.Enabled = false;
            this.clearButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.clearButton.Location = new System.Drawing.Point(932, 33);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(100, 30);
            this.clearButton.TabIndex = 8;
            this.clearButton.Text = "Clear";
            this.clearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.Image = global::OpenDental.Properties.Resources.IconFolderSearch;
            this.searchButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.searchButton.Location = new System.Drawing.Point(826, 33);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(100, 30);
            this.searchButton.TabIndex = 7;
            this.searchButton.Text = "Search";
            this.searchButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.searchButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.searchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // showAttachmentsOnlyCheckBox
            // 
            this.showAttachmentsOnlyCheckBox.AutoSize = true;
            this.showAttachmentsOnlyCheckBox.Location = new System.Drawing.Point(6, 75);
            this.showAttachmentsOnlyCheckBox.Name = "showAttachmentsOnlyCheckBox";
            this.showAttachmentsOnlyCheckBox.Size = new System.Drawing.Size(242, 19);
            this.showAttachmentsOnlyCheckBox.TabIndex = 3;
            this.showAttachmentsOnlyCheckBox.Text = "Only include messages with attachments";
            this.showAttachmentsOnlyCheckBox.UseVisualStyleBackColor = true;
            // 
            // subjectLabel
            // 
            this.subjectLabel.AutoSize = true;
            this.subjectLabel.Location = new System.Drawing.Point(561, 18);
            this.subjectLabel.Name = "subjectLabel";
            this.subjectLabel.Size = new System.Drawing.Size(78, 15);
            this.subjectLabel.TabIndex = 163;
            this.subjectLabel.Text = "Subject/Body";
            // 
            // subjectTextBox
            // 
            this.subjectTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subjectTextBox.Location = new System.Drawing.Point(564, 36);
            this.subjectTextBox.Name = "subjectTextBox";
            this.subjectTextBox.Size = new System.Drawing.Size(256, 23);
            this.subjectTextBox.TabIndex = 6;
            // 
            // emailAddressLabel
            // 
            this.emailAddressLabel.AutoSize = true;
            this.emailAddressLabel.Location = new System.Drawing.Point(187, 19);
            this.emailAddressLabel.Name = "emailAddressLabel";
            this.emailAddressLabel.Size = new System.Drawing.Size(86, 15);
            this.emailAddressLabel.TabIndex = 161;
            this.emailAddressLabel.Text = "E-mail Address";
            // 
            // emailAddessTextBox
            // 
            this.emailAddessTextBox.Location = new System.Drawing.Point(190, 37);
            this.emailAddessTextBox.Name = "emailAddessTextBox";
            this.emailAddessTextBox.Size = new System.Drawing.Size(216, 23);
            this.emailAddessTextBox.TabIndex = 2;
            // 
            // fromTextBox
            // 
            this.fromTextBox.Location = new System.Drawing.Point(412, 37);
            this.fromTextBox.Name = "fromTextBox";
            this.fromTextBox.Size = new System.Drawing.Size(70, 23);
            this.fromTextBox.TabIndex = 4;
            // 
            // fromLabel
            // 
            this.fromLabel.AutoSize = true;
            this.fromLabel.Location = new System.Drawing.Point(409, 19);
            this.fromLabel.Name = "fromLabel";
            this.fromLabel.Size = new System.Drawing.Size(35, 15);
            this.fromLabel.TabIndex = 158;
            this.fromLabel.Text = "From";
            // 
            // pickPatientButton
            // 
            this.pickPatientButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.pickPatientButton.Location = new System.Drawing.Point(153, 36);
            this.pickPatientButton.Name = "pickPatientButton";
            this.pickPatientButton.Size = new System.Drawing.Size(30, 25);
            this.pickPatientButton.TabIndex = 1;
            this.pickPatientButton.Text = "...";
            this.pickPatientButton.Click += new System.EventHandler(this.PickPatientButton_Click);
            // 
            // patientLabel
            // 
            this.patientLabel.AutoSize = true;
            this.patientLabel.Location = new System.Drawing.Point(6, 19);
            this.patientLabel.Name = "patientLabel";
            this.patientLabel.Size = new System.Drawing.Size(44, 15);
            this.patientLabel.TabIndex = 156;
            this.patientLabel.Text = "Patient";
            this.patientLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // patientTextBox
            // 
            this.patientTextBox.Location = new System.Drawing.Point(6, 37);
            this.patientTextBox.Name = "patientTextBox";
            this.patientTextBox.ReadOnly = true;
            this.patientTextBox.Size = new System.Drawing.Size(146, 23);
            this.patientTextBox.TabIndex = 0;
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshButton.Image = global::OpenDental.Properties.Resources.IconRefresh;
            this.refreshButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.refreshButton.Location = new System.Drawing.Point(921, 178);
            this.refreshButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.refreshButton.Size = new System.Drawing.Size(130, 30);
            this.refreshButton.TabIndex = 3;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.refreshButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(921, 598);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(130, 30);
            this.closeButton.TabIndex = 10;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // replyAllButton
            // 
            this.replyAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.replyAllButton.Enabled = false;
            this.replyAllButton.Image = global::OpenDental.Properties.Resources.IconEmailReplyAll;
            this.replyAllButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.replyAllButton.Location = new System.Drawing.Point(921, 303);
            this.replyAllButton.Name = "replyAllButton";
            this.replyAllButton.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.replyAllButton.Size = new System.Drawing.Size(130, 30);
            this.replyAllButton.TabIndex = 11;
            this.replyAllButton.Text = "Reply All";
            this.replyAllButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.replyAllButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.replyAllButton.Click += new System.EventHandler(this.ReplyAllButton_Click);
            // 
            // forwardButton
            // 
            this.forwardButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.forwardButton.Enabled = false;
            this.forwardButton.Image = global::OpenDental.Properties.Resources.IconEmailForward;
            this.forwardButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.forwardButton.Location = new System.Drawing.Point(921, 339);
            this.forwardButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.forwardButton.Name = "forwardButton";
            this.forwardButton.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.forwardButton.Size = new System.Drawing.Size(130, 30);
            this.forwardButton.TabIndex = 12;
            this.forwardButton.Text = "Forward";
            this.forwardButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.forwardButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.forwardButton.Click += new System.EventHandler(this.ForwardButton_Click);
            // 
            // setupButton
            // 
            this.setupButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setupButton.Image = global::OpenDental.Properties.Resources.IconCog;
            this.setupButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.setupButton.Location = new System.Drawing.Point(921, 125);
            this.setupButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.setupButton.Name = "setupButton";
            this.setupButton.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.setupButton.Size = new System.Drawing.Size(130, 30);
            this.setupButton.TabIndex = 172;
            this.setupButton.Text = "&Setup";
            this.setupButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.setupButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.setupButton.Click += new System.EventHandler(this.SetupButton_Click);
            // 
            // messageViewTreeView
            // 
            this.messageViewTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.messageViewTreeView.Location = new System.Drawing.Point(13, 154);
            this.messageViewTreeView.Name = "messageViewTreeView";
            this.messageViewTreeView.Size = new System.Drawing.Size(200, 474);
            this.messageViewTreeView.TabIndex = 173;
            this.messageViewTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.MessageViewTreeView_AfterSelect);
            // 
            // emailAddressComboBox
            // 
            this.emailAddressComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.emailAddressComboBox.FormattingEnabled = true;
            this.emailAddressComboBox.Location = new System.Drawing.Point(13, 125);
            this.emailAddressComboBox.Name = "emailAddressComboBox";
            this.emailAddressComboBox.Size = new System.Drawing.Size(199, 23);
            this.emailAddressComboBox.TabIndex = 175;
            this.emailAddressComboBox.SelectionChangeCommitted += new System.EventHandler(this.EmailAddressComboBox_SelectionChangeCommitted);
            // 
            // FormEmailInbox
            // 
            this.ClientSize = new System.Drawing.Size(1064, 641);
            this.Controls.Add(this.emailAddressComboBox);
            this.Controls.Add(this.messageViewTreeView);
            this.Controls.Add(this.messageGrid);
            this.Controls.Add(this.setupButton);
            this.Controls.Add(this.forwardButton);
            this.Controls.Add(this.replyAllButton);
            this.Controls.Add(this.markNotReadButton);
            this.Controls.Add(this.markReadButton);
            this.Controls.Add(this.changePatientButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.composeButton);
            this.Controls.Add(this.replyButton);
            this.Controls.Add(this.searchGroupBox);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.closeButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1080, 680);
            this.Name = "FormEmailInbox";
            this.Text = "E-mail";
            this.Load += new System.EventHandler(this.FormEmailInbox_Load);
            this.searchGroupBox.ResumeLayout(false);
            this.searchGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button markNotReadButton;
        private System.Windows.Forms.Button markReadButton;
        private System.Windows.Forms.Button changePatientButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button composeButton;
        private System.Windows.Forms.Button replyButton;
        private System.Windows.Forms.GroupBox searchGroupBox;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.CheckBox showAttachmentsOnlyCheckBox;
        private System.Windows.Forms.Label subjectLabel;
        private System.Windows.Forms.TextBox subjectTextBox;
        private System.Windows.Forms.Label emailAddressLabel;
        private System.Windows.Forms.TextBox emailAddessTextBox;
        private ValidDate fromTextBox;
        private System.Windows.Forms.Label fromLabel;
        private System.Windows.Forms.Button pickPatientButton;
        private System.Windows.Forms.Label patientLabel;
        private System.Windows.Forms.TextBox patientTextBox;
        private System.Windows.Forms.Button refreshButton;
        private UI.ODGrid messageGrid;
        private System.Windows.Forms.Button closeButton;
        private ValidDate toTextBox;
        private System.Windows.Forms.Label toLabel;
        private System.Windows.Forms.Button replyAllButton;
        private System.Windows.Forms.Button forwardButton;
        private System.Windows.Forms.Button setupButton;
        private System.Windows.Forms.TreeView messageViewTreeView;
        private System.Windows.Forms.CheckBox showHiddenCheckBox;
        private System.Windows.Forms.ComboBox emailAddressComboBox;
    }
}