namespace OpenDental
{
    partial class FormAllergyEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAllergyEdit));
            this.snomedNoneButton = new System.Windows.Forms.Button();
            this.snomedBrowseButton = new System.Windows.Forms.Button();
            this.snomedLabel = new System.Windows.Forms.Label();
            this.snomedTextBox = new System.Windows.Forms.TextBox();
            this.dateLabel = new System.Windows.Forms.Label();
            this.dateTextBox = new System.Windows.Forms.TextBox();
            this.butCancel = new System.Windows.Forms.Button();
            this.allergyComboBox = new System.Windows.Forms.ComboBox();
            this.allergyLabel = new System.Windows.Forms.Label();
            this.activeCheckBox = new System.Windows.Forms.CheckBox();
            this.reactionTextBox = new System.Windows.Forms.TextBox();
            this.reactionLabel = new System.Windows.Forms.Label();
            this.acceptButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // snomedNoneButton
            // 
            this.snomedNoneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.snomedNoneButton.Location = new System.Drawing.Point(491, 47);
            this.snomedNoneButton.Name = "snomedNoneButton";
            this.snomedNoneButton.Size = new System.Drawing.Size(60, 25);
            this.snomedNoneButton.TabIndex = 5;
            this.snomedNoneButton.Text = "None";
            this.snomedNoneButton.Click += new System.EventHandler(this.snomedNoneButton_Click);
            // 
            // snomedBrowseButton
            // 
            this.snomedBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.snomedBrowseButton.Image = global::OpenDental.Properties.Resources.IconSearch;
            this.snomedBrowseButton.Location = new System.Drawing.Point(455, 47);
            this.snomedBrowseButton.Name = "snomedBrowseButton";
            this.snomedBrowseButton.Size = new System.Drawing.Size(30, 25);
            this.snomedBrowseButton.TabIndex = 4;
            this.snomedBrowseButton.Click += new System.EventHandler(this.snomedBrowseButton_Click);
            // 
            // snomedLabel
            // 
            this.snomedLabel.AutoSize = true;
            this.snomedLabel.Location = new System.Drawing.Point(31, 52);
            this.snomedLabel.Name = "snomedLabel";
            this.snomedLabel.Size = new System.Drawing.Size(123, 15);
            this.snomedLabel.TabIndex = 2;
            this.snomedLabel.Text = "SNOMED CT Reaction";
            this.snomedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // snomedTextBox
            // 
            this.snomedTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.snomedTextBox.Location = new System.Drawing.Point(160, 48);
            this.snomedTextBox.Name = "snomedTextBox";
            this.snomedTextBox.ReadOnly = true;
            this.snomedTextBox.Size = new System.Drawing.Size(289, 23);
            this.snomedTextBox.TabIndex = 3;
            this.snomedTextBox.TabStop = false;
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(29, 156);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(125, 15);
            this.dateLabel.TabIndex = 8;
            this.dateLabel.Text = "Date Adverse Reaction";
            this.dateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTextBox
            // 
            this.dateTextBox.Location = new System.Drawing.Point(160, 153);
            this.dateTextBox.Name = "dateTextBox";
            this.dateTextBox.Size = new System.Drawing.Size(100, 23);
            this.dateTextBox.TabIndex = 9;
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.Location = new System.Drawing.Point(441, 248);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(110, 30);
            this.butCancel.TabIndex = 13;
            this.butCancel.Text = "&Cancel";
            // 
            // allergyComboBox
            // 
            this.allergyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.allergyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.allergyComboBox.FormattingEnabled = true;
            this.allergyComboBox.Location = new System.Drawing.Point(160, 19);
            this.allergyComboBox.Name = "allergyComboBox";
            this.allergyComboBox.Size = new System.Drawing.Size(289, 23);
            this.allergyComboBox.TabIndex = 1;
            // 
            // allergyLabel
            // 
            this.allergyLabel.AutoSize = true;
            this.allergyLabel.Location = new System.Drawing.Point(110, 22);
            this.allergyLabel.Name = "allergyLabel";
            this.allergyLabel.Size = new System.Drawing.Size(44, 15);
            this.allergyLabel.TabIndex = 0;
            this.allergyLabel.Text = "Allergy";
            this.allergyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // activeCheckBox
            // 
            this.activeCheckBox.AutoSize = true;
            this.activeCheckBox.Checked = true;
            this.activeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.activeCheckBox.Location = new System.Drawing.Point(160, 189);
            this.activeCheckBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.activeCheckBox.Name = "activeCheckBox";
            this.activeCheckBox.Size = new System.Drawing.Size(70, 19);
            this.activeCheckBox.TabIndex = 10;
            this.activeCheckBox.Text = "Is Active";
            this.activeCheckBox.UseVisualStyleBackColor = true;
            // 
            // reactionTextBox
            // 
            this.reactionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reactionTextBox.Location = new System.Drawing.Point(160, 77);
            this.reactionTextBox.Multiline = true;
            this.reactionTextBox.Name = "reactionTextBox";
            this.reactionTextBox.Size = new System.Drawing.Size(289, 70);
            this.reactionTextBox.TabIndex = 7;
            // 
            // reactionLabel
            // 
            this.reactionLabel.AutoSize = true;
            this.reactionLabel.Location = new System.Drawing.Point(38, 80);
            this.reactionLabel.Name = "reactionLabel";
            this.reactionLabel.Size = new System.Drawing.Size(116, 15);
            this.reactionLabel.TabIndex = 6;
            this.reactionLabel.Text = "Reaction Description";
            this.reactionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(325, 248);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 12;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deleteButton.Location = new System.Drawing.Point(13, 248);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 11;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // FormAllergyEdit
            // 
            this.ClientSize = new System.Drawing.Size(564, 291);
            this.Controls.Add(this.snomedNoneButton);
            this.Controls.Add(this.snomedBrowseButton);
            this.Controls.Add(this.snomedLabel);
            this.Controls.Add(this.snomedTextBox);
            this.Controls.Add(this.dateLabel);
            this.Controls.Add(this.dateTextBox);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.allergyComboBox);
            this.Controls.Add(this.allergyLabel);
            this.Controls.Add(this.activeCheckBox);
            this.Controls.Add(this.reactionTextBox);
            this.Controls.Add(this.reactionLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.deleteButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAllergyEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Allergy";
            this.Load += new System.EventHandler(this.FormAllergyEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.CheckBox activeCheckBox;
        private System.Windows.Forms.TextBox reactionTextBox;
        private System.Windows.Forms.Label reactionLabel;
        private System.Windows.Forms.Label allergyLabel;
        private System.Windows.Forms.ComboBox allergyComboBox;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.TextBox dateTextBox;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.Button snomedNoneButton;
        private System.Windows.Forms.Button snomedBrowseButton;
        private System.Windows.Forms.Label snomedLabel;
        private System.Windows.Forms.TextBox snomedTextBox;
    }
}