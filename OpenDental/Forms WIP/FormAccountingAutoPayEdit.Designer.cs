namespace OpenDental
{
    partial class FormAccountingAutoPayEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccountingAutoPayEdit));
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.paymentTypeComboBox = new System.Windows.Forms.ComboBox();
            this.paymentTypeLabel = new System.Windows.Forms.Label();
            this.accountsListBox = new System.Windows.Forms.ListBox();
            this.accountsLabel = new System.Windows.Forms.Label();
            this.removeButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(441, 268);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(325, 268);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 1;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // paymentTypeComboBox
            // 
            this.paymentTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paymentTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.paymentTypeComboBox.FormattingEnabled = true;
            this.paymentTypeComboBox.Location = new System.Drawing.Point(260, 22);
            this.paymentTypeComboBox.Name = "paymentTypeComboBox";
            this.paymentTypeComboBox.Size = new System.Drawing.Size(291, 23);
            this.paymentTypeComboBox.TabIndex = 43;
            // 
            // paymentTypeLabel
            // 
            this.paymentTypeLabel.AutoSize = true;
            this.paymentTypeLabel.Location = new System.Drawing.Point(50, 25);
            this.paymentTypeLabel.Name = "paymentTypeLabel";
            this.paymentTypeLabel.Size = new System.Drawing.Size(204, 15);
            this.paymentTypeLabel.TabIndex = 42;
            this.paymentTypeLabel.Text = "When this type of payment is entered";
            this.paymentTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // accountsListBox
            // 
            this.accountsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accountsListBox.FormattingEnabled = true;
            this.accountsListBox.IntegralHeight = false;
            this.accountsListBox.ItemHeight = 15;
            this.accountsListBox.Location = new System.Drawing.Point(260, 51);
            this.accountsListBox.Name = "accountsListBox";
            this.accountsListBox.Size = new System.Drawing.Size(291, 120);
            this.accountsListBox.TabIndex = 41;
            // 
            // accountsLabel
            // 
            this.accountsLabel.Location = new System.Drawing.Point(13, 51);
            this.accountsLabel.Name = "accountsLabel";
            this.accountsLabel.Size = new System.Drawing.Size(241, 120);
            this.accountsLabel.TabIndex = 40;
            this.accountsLabel.Text = "User will get to pick from this list of accounts to deposit into.";
            this.accountsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(376, 177);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(110, 30);
            this.removeButton.TabIndex = 45;
            this.removeButton.Text = "Remove";
            this.removeButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(260, 177);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(110, 30);
            this.addButton.TabIndex = 44;
            this.addButton.Text = "Add";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deleteButton.Location = new System.Drawing.Point(13, 268);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 46;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormAccountingAutoPayEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(564, 311);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.paymentTypeComboBox);
            this.Controls.Add(this.paymentTypeLabel);
            this.Controls.Add(this.accountsListBox);
            this.Controls.Add(this.accountsLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAccountingAutoPayEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto Pay Entry";
            this.Load += new System.EventHandler(this.FormAccountingAutoPayEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.ComboBox paymentTypeComboBox;
        private System.Windows.Forms.Label paymentTypeLabel;
        private System.Windows.Forms.ListBox accountsListBox;
        private System.Windows.Forms.Label accountsLabel;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button deleteButton;
    }
}
