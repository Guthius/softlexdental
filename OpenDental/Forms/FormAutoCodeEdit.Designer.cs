namespace OpenDental
{
    partial class FormAutoCodeEdit
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAutoCodeEdit));
            this.hiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.duplicatesLabel = new System.Windows.Forms.Label();
            this.deleteButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.lessIntrusiveCheckBox = new System.Windows.Forms.CheckBox();
            this.autoCodeItemsListView = new System.Windows.Forms.ListView();
            this.codeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.descriptionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.conditionsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // hiddenCheckBox
            // 
            this.hiddenCheckBox.AutoSize = true;
            this.hiddenCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.hiddenCheckBox.Location = new System.Drawing.Point(400, 19);
            this.hiddenCheckBox.Name = "hiddenCheckBox";
            this.hiddenCheckBox.Size = new System.Drawing.Size(71, 20);
            this.hiddenCheckBox.TabIndex = 2;
            this.hiddenCheckBox.Text = "Hidden";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(661, 318);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(545, 318);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 8;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(120, 19);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(252, 23);
            this.descriptionTextBox.TabIndex = 1;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(47, 24);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(67, 15);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Description";
            // 
            // duplicatesLabel
            // 
            this.duplicatesLabel.AutoSize = true;
            this.duplicatesLabel.Location = new System.Drawing.Point(10, 92);
            this.duplicatesLabel.Name = "duplicatesLabel";
            this.duplicatesLabel.Size = new System.Drawing.Size(277, 15);
            this.duplicatesLabel.TabIndex = 4;
            this.duplicatesLabel.Text = "You may have duplicate codes  in the following list.";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.Location = new System.Drawing.Point(59, 318);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(40, 30);
            this.deleteButton.TabIndex = 7;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addButton.Location = new System.Drawing.Point(13, 318);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(40, 30);
            this.addButton.TabIndex = 6;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // lessIntrusiveCheckBox
            // 
            this.lessIntrusiveCheckBox.AutoSize = true;
            this.lessIntrusiveCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lessIntrusiveCheckBox.Location = new System.Drawing.Point(400, 45);
            this.lessIntrusiveCheckBox.Name = "lessIntrusiveCheckBox";
            this.lessIntrusiveCheckBox.Size = new System.Drawing.Size(368, 35);
            this.lessIntrusiveCheckBox.TabIndex = 3;
            this.lessIntrusiveCheckBox.Text = "Do not check codes in the procedure edit window, but only use \r\nthis auto code fo" +
    "r procedure buttons.";
            this.lessIntrusiveCheckBox.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // autoCodeItemsListView
            // 
            this.autoCodeItemsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoCodeItemsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.codeColumnHeader,
            this.descriptionColumnHeader,
            this.conditionsColumnHeader});
            this.autoCodeItemsListView.FullRowSelect = true;
            this.autoCodeItemsListView.GridLines = true;
            this.autoCodeItemsListView.HideSelection = false;
            this.autoCodeItemsListView.Location = new System.Drawing.Point(13, 110);
            this.autoCodeItemsListView.Name = "autoCodeItemsListView";
            this.autoCodeItemsListView.Size = new System.Drawing.Size(758, 202);
            this.autoCodeItemsListView.TabIndex = 5;
            this.autoCodeItemsListView.UseCompatibleStateImageBehavior = false;
            this.autoCodeItemsListView.View = System.Windows.Forms.View.Details;
            this.autoCodeItemsListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.AutoCodeItemsListView_MouseDoubleClick);
            // 
            // codeColumnHeader
            // 
            this.codeColumnHeader.Text = "Code";
            this.codeColumnHeader.Width = 100;
            // 
            // descriptionColumnHeader
            // 
            this.descriptionColumnHeader.Text = "Description";
            this.descriptionColumnHeader.Width = 320;
            // 
            // conditionsColumnHeader
            // 
            this.conditionsColumnHeader.Text = "Conditions";
            this.conditionsColumnHeader.Width = 250;
            // 
            // FormAutoCodeEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(784, 361);
            this.Controls.Add(this.autoCodeItemsListView);
            this.Controls.Add(this.lessIntrusiveCheckBox);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.duplicatesLabel);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.hiddenCheckBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAutoCodeEdit";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Auto Code";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAutoCodeEdit_FormClosing);
            this.Load += new System.EventHandler(this.FormAutoCodeEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.CheckBox hiddenCheckBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label duplicatesLabel;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.CheckBox lessIntrusiveCheckBox;
        private System.Windows.Forms.ListView autoCodeItemsListView;
        private System.Windows.Forms.ColumnHeader codeColumnHeader;
        private System.Windows.Forms.ColumnHeader descriptionColumnHeader;
        private System.Windows.Forms.ColumnHeader conditionsColumnHeader;
    }
}
