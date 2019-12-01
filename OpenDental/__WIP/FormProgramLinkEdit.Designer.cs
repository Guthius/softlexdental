namespace OpenDental
{
    partial class FormProgramLinkEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProgramLinkEdit));
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.enabledCheckBox = new System.Windows.Forms.CheckBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.programLabel = new System.Windows.Forms.Label();
            this.programTextBox = new System.Windows.Forms.TextBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.toolbarsListBox = new System.Windows.Forms.ListBox();
            this.toolbarsLabel = new System.Windows.Forms.Label();
            this.noteTextBox = new System.Windows.Forms.TextBox();
            this.noteLabel = new System.Windows.Forms.Label();
            this.preferencesGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(661, 518);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(545, 518);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 9;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // enabledCheckBox
            // 
            this.enabledCheckBox.AutoSize = true;
            this.enabledCheckBox.Location = new System.Drawing.Point(200, 77);
            this.enabledCheckBox.Name = "enabledCheckBox";
            this.enabledCheckBox.Size = new System.Drawing.Size(68, 19);
            this.enabledCheckBox.TabIndex = 2;
            this.enabledCheckBox.Text = "Enabled";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(13, 518);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 8;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // programLabel
            // 
            this.programLabel.AutoSize = true;
            this.programLabel.Location = new System.Drawing.Point(141, 22);
            this.programLabel.Name = "programLabel";
            this.programLabel.Size = new System.Drawing.Size(53, 15);
            this.programLabel.TabIndex = 11;
            this.programLabel.Text = "Program";
            // 
            // programTextBox
            // 
            this.programTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.programTextBox.Location = new System.Drawing.Point(200, 19);
            this.programTextBox.Name = "programTextBox";
            this.programTextBox.ReadOnly = true;
            this.programTextBox.Size = new System.Drawing.Size(571, 23);
            this.programTextBox.TabIndex = 12;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.Location = new System.Drawing.Point(200, 48);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(571, 23);
            this.descriptionTextBox.TabIndex = 1;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(127, 51);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(67, 15);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Description";
            // 
            // toolbarsListBox
            // 
            this.toolbarsListBox.IntegralHeight = false;
            this.toolbarsListBox.ItemHeight = 15;
            this.toolbarsListBox.Location = new System.Drawing.Point(200, 374);
            this.toolbarsListBox.Name = "toolbarsListBox";
            this.toolbarsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.toolbarsListBox.Size = new System.Drawing.Size(250, 110);
            this.toolbarsListBox.TabIndex = 7;
            // 
            // toolbarsLabel
            // 
            this.toolbarsLabel.AutoSize = true;
            this.toolbarsLabel.Location = new System.Drawing.Point(26, 377);
            this.toolbarsLabel.Name = "toolbarsLabel";
            this.toolbarsLabel.Size = new System.Drawing.Size(168, 15);
            this.toolbarsLabel.TabIndex = 6;
            this.toolbarsLabel.Text = "Add a button to these toolbars";
            this.toolbarsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // noteTextBox
            // 
            this.noteTextBox.AcceptsReturn = true;
            this.noteTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noteTextBox.Location = new System.Drawing.Point(200, 102);
            this.noteTextBox.MaxLength = 4000;
            this.noteTextBox.Multiline = true;
            this.noteTextBox.Name = "noteTextBox";
            this.noteTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.noteTextBox.Size = new System.Drawing.Size(571, 110);
            this.noteTextBox.TabIndex = 4;
            // 
            // noteLabel
            // 
            this.noteLabel.AutoSize = true;
            this.noteLabel.Location = new System.Drawing.Point(156, 105);
            this.noteLabel.Name = "noteLabel";
            this.noteLabel.Size = new System.Drawing.Size(38, 15);
            this.noteLabel.TabIndex = 3;
            this.noteLabel.Text = "Notes";
            this.noteLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // preferencesGrid
            // 
            this.preferencesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.preferencesGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.preferencesGrid.EditableEnterMovesDown = false;
            this.preferencesGrid.HasAddButton = false;
            this.preferencesGrid.HasDropDowns = false;
            this.preferencesGrid.HasMultilineHeaders = false;
            this.preferencesGrid.HScrollVisible = false;
            this.preferencesGrid.Location = new System.Drawing.Point(200, 218);
            this.preferencesGrid.Name = "preferencesGrid";
            this.preferencesGrid.ScrollValue = 0;
            this.preferencesGrid.Size = new System.Drawing.Size(571, 150);
            this.preferencesGrid.TabIndex = 5;
            this.preferencesGrid.Title = "Program Preferences";
            this.preferencesGrid.TitleVisible = true;
            this.preferencesGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.PrefrencesGrid_CellDoubleClick);
            // 
            // FormProgramLinkEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.preferencesGrid);
            this.Controls.Add(this.noteLabel);
            this.Controls.Add(this.noteTextBox);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.programTextBox);
            this.Controls.Add(this.toolbarsLabel);
            this.Controls.Add(this.toolbarsListBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.programLabel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.enabledCheckBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProgramLinkEdit";
            this.ShowInTaskbar = false;
            this.Text = "Program Link";
            this.Load += new System.EventHandler(this.FormProgramLinkEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.CheckBox enabledCheckBox;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Label programLabel;
        private System.Windows.Forms.TextBox programTextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label toolbarsLabel;
        private System.Windows.Forms.ListBox toolbarsListBox;
        private System.Windows.Forms.Label noteLabel;
        private System.Windows.Forms.TextBox noteTextBox;
        private OpenDental.UI.ODGrid preferencesGrid;
    }
}
