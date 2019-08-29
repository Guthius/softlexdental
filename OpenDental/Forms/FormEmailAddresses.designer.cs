namespace OpenDental
{
    partial class FormEmailAddresses
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailAddresses));
            this.infoLabel = new System.Windows.Forms.Label();
            this.defaultGroupBox = new System.Windows.Forms.GroupBox();
            this.defaultLabel = new System.Windows.Forms.Label();
            this.defaultButton = new System.Windows.Forms.Button();
            this.checkIntervalTextBox = new System.Windows.Forms.TextBox();
            this.checkIntervalHelpLabel = new System.Windows.Forms.Label();
            this.checkIntervalLabel = new System.Windows.Forms.Label();
            this.emailAddressGrid = new OpenDental.UI.ODGrid();
            this.addButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.disclaimerCheckBox = new System.Windows.Forms.CheckBox();
            this.defaultGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(13, 16);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(728, 40);
            this.infoLabel.TabIndex = 13;
            this.infoLabel.Text = "Setup clinic, practice, user, and group email addresses here.\r\nIndividual user in" +
    "boxes can also be setup in File | User Email Address.";
            // 
            // defaultGroupBox
            // 
            this.defaultGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.defaultGroupBox.Controls.Add(this.defaultLabel);
            this.defaultGroupBox.Controls.Add(this.defaultButton);
            this.defaultGroupBox.Location = new System.Drawing.Point(747, 51);
            this.defaultGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.defaultGroupBox.Name = "defaultGroupBox";
            this.defaultGroupBox.Size = new System.Drawing.Size(150, 140);
            this.defaultGroupBox.TabIndex = 3;
            this.defaultGroupBox.TabStop = false;
            // 
            // defaultLabel
            // 
            this.defaultLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.defaultLabel.Location = new System.Drawing.Point(6, 19);
            this.defaultLabel.Name = "defaultLabel";
            this.defaultLabel.Size = new System.Drawing.Size(138, 82);
            this.defaultLabel.TabIndex = 12;
            this.defaultLabel.Text = "Highlight an email address in the grid, then use one of these buttons.  Not requi" +
    "red.";
            this.defaultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // defaultButton
            // 
            this.defaultButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.defaultButton.Location = new System.Drawing.Point(6, 104);
            this.defaultButton.Name = "defaultButton";
            this.defaultButton.Size = new System.Drawing.Size(138, 30);
            this.defaultButton.TabIndex = 3;
            this.defaultButton.Text = "Set Default";
            this.defaultButton.Click += new System.EventHandler(this.DefaultButton_Click);
            // 
            // checkIntervalTextBox
            // 
            this.checkIntervalTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkIntervalTextBox.Location = new System.Drawing.Point(172, 503);
            this.checkIntervalTextBox.MaxLength = 2147483647;
            this.checkIntervalTextBox.Name = "checkIntervalTextBox";
            this.checkIntervalTextBox.Size = new System.Drawing.Size(47, 23);
            this.checkIntervalTextBox.TabIndex = 8;
            // 
            // checkIntervalHelpLabel
            // 
            this.checkIntervalHelpLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkIntervalHelpLabel.AutoSize = true;
            this.checkIntervalHelpLabel.Location = new System.Drawing.Point(225, 506);
            this.checkIntervalHelpLabel.Margin = new System.Windows.Forms.Padding(3, 0, 20, 0);
            this.checkIntervalHelpLabel.Name = "checkIntervalHelpLabel";
            this.checkIntervalHelpLabel.Size = new System.Drawing.Size(96, 15);
            this.checkIntervalHelpLabel.TabIndex = 9;
            this.checkIntervalHelpLabel.Text = "minutes (1 to 60)";
            this.checkIntervalHelpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkIntervalLabel
            // 
            this.checkIntervalLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkIntervalLabel.AutoSize = true;
            this.checkIntervalLabel.Location = new System.Drawing.Point(10, 506);
            this.checkIntervalLabel.Name = "checkIntervalLabel";
            this.checkIntervalLabel.Size = new System.Drawing.Size(156, 15);
            this.checkIntervalLabel.TabIndex = 7;
            this.checkIntervalLabel.Text = "Check for new e-mails every";
            this.checkIntervalLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // emailAddressGrid
            // 
            this.emailAddressGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.emailAddressGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.emailAddressGrid.EditableEnterMovesDown = false;
            this.emailAddressGrid.HasAddButton = false;
            this.emailAddressGrid.HasDropDowns = false;
            this.emailAddressGrid.HasMultilineHeaders = false;
            this.emailAddressGrid.HScrollVisible = false;
            this.emailAddressGrid.Location = new System.Drawing.Point(13, 59);
            this.emailAddressGrid.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.emailAddressGrid.Name = "emailAddressGrid";
            this.emailAddressGrid.ScrollValue = 0;
            this.emailAddressGrid.Size = new System.Drawing.Size(728, 431);
            this.emailAddressGrid.TabIndex = 4;
            this.emailAddressGrid.Title = "Email Addresses";
            this.emailAddressGrid.TitleVisible = true;
            this.emailAddressGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.EmailAddressGrid_CellDoubleClick);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addButton.Location = new System.Drawing.Point(747, 214);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(150, 30);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "Add";
            this.addButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(747, 424);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(150, 30);
            this.acceptButton.TabIndex = 2;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(747, 460);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(150, 30);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            // 
            // disclaimerCheckBox
            // 
            this.disclaimerCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.disclaimerCheckBox.AutoSize = true;
            this.disclaimerCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.disclaimerCheckBox.Location = new System.Drawing.Point(344, 504);
            this.disclaimerCheckBox.Name = "disclaimerCheckBox";
            this.disclaimerCheckBox.Size = new System.Drawing.Size(176, 20);
            this.disclaimerCheckBox.TabIndex = 14;
            this.disclaimerCheckBox.Text = "Include Opt-Out Statement";
            this.disclaimerCheckBox.UseVisualStyleBackColor = true;
            // 
            // FormEmailAddresses
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(910, 539);
            this.Controls.Add(this.disclaimerCheckBox);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.defaultGroupBox);
            this.Controls.Add(this.checkIntervalTextBox);
            this.Controls.Add(this.checkIntervalHelpLabel);
            this.Controls.Add(this.checkIntervalLabel);
            this.Controls.Add(this.emailAddressGrid);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(545, 400);
            this.Name = "FormEmailAddresses";
            this.Text = "Email Addresses";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEmailAddresses_FormClosing);
            this.Load += new System.EventHandler(this.FormEmailAddresses_Load);
            this.defaultGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button defaultButton;
        private System.Windows.Forms.Button cancelButton;
        private UI.ODGrid emailAddressGrid;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Label checkIntervalLabel;
        private System.Windows.Forms.Label checkIntervalHelpLabel;
        private System.Windows.Forms.TextBox checkIntervalTextBox;
        private System.Windows.Forms.GroupBox defaultGroupBox;
        private System.Windows.Forms.Label defaultLabel;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.CheckBox disclaimerCheckBox;
    }
}