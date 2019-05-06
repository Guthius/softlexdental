namespace OpenDental
{
    partial class FormSupplyOrderEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSupplyOrderEdit));
            this.supplierLabel = new System.Windows.Forms.Label();
            this.supplierTextBox = new System.Windows.Forms.TextBox();
            this.noteTextBox = new System.Windows.Forms.TextBox();
            this.noteLabel = new System.Windows.Forms.Label();
            this.datePlacedLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.datePlacedTextBox = new OpenDental.ValidDate();
            this.deleteButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.totalTextBox = new OpenDental.UI.ODCurrencyTextBox();
            this.totalLabel = new System.Windows.Forms.Label();
            this.totalInfoLabel = new System.Windows.Forms.Label();
            this.shippingChargeTextBox = new OpenDental.UI.ODCurrencyTextBox();
            this.shippingChargeLabel = new System.Windows.Forms.Label();
            this.userComboBox = new System.Windows.Forms.ComboBox();
            this.userLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // supplierLabel
            // 
            this.supplierLabel.AutoSize = true;
            this.supplierLabel.Location = new System.Drawing.Point(104, 22);
            this.supplierLabel.Name = "supplierLabel";
            this.supplierLabel.Size = new System.Drawing.Size(50, 15);
            this.supplierLabel.TabIndex = 10;
            this.supplierLabel.Text = "Supplier";
            this.supplierLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // supplierTextBox
            // 
            this.supplierTextBox.Location = new System.Drawing.Point(160, 19);
            this.supplierTextBox.Name = "supplierTextBox";
            this.supplierTextBox.ReadOnly = true;
            this.supplierTextBox.Size = new System.Drawing.Size(300, 23);
            this.supplierTextBox.TabIndex = 7;
            this.supplierTextBox.TabStop = false;
            // 
            // noteTextBox
            // 
            this.noteTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noteTextBox.Location = new System.Drawing.Point(160, 164);
            this.noteTextBox.Multiline = true;
            this.noteTextBox.Name = "noteTextBox";
            this.noteTextBox.Size = new System.Drawing.Size(551, 132);
            this.noteTextBox.TabIndex = 4;
            // 
            // noteLabel
            // 
            this.noteLabel.AutoSize = true;
            this.noteLabel.Location = new System.Drawing.Point(121, 167);
            this.noteLabel.Name = "noteLabel";
            this.noteLabel.Size = new System.Drawing.Size(33, 15);
            this.noteLabel.TabIndex = 14;
            this.noteLabel.Text = "Note";
            this.noteLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // datePlacedLabel
            // 
            this.datePlacedLabel.AutoSize = true;
            this.datePlacedLabel.Location = new System.Drawing.Point(85, 51);
            this.datePlacedLabel.Name = "datePlacedLabel";
            this.datePlacedLabel.Size = new System.Drawing.Size(69, 15);
            this.datePlacedLabel.TabIndex = 11;
            this.datePlacedLabel.Text = "Date Placed";
            this.datePlacedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(266, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(445, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Leave date and user blank to indicate order has not been placed yet and is pendin" +
    "g.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // datePlacedTextBox
            // 
            this.datePlacedTextBox.Location = new System.Drawing.Point(160, 48);
            this.datePlacedTextBox.Name = "datePlacedTextBox";
            this.datePlacedTextBox.Size = new System.Drawing.Size(100, 23);
            this.datePlacedTextBox.TabIndex = 0;
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deleteButton.Location = new System.Drawing.Point(13, 328);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 7;
            this.deleteButton.Text = "Delete";
            this.deleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(485, 328);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 5;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(601, 328);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "&Cancel";
            // 
            // totalTextBox
            // 
            this.totalTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.totalTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.totalTextBox.Location = new System.Drawing.Point(160, 106);
            this.totalTextBox.Name = "totalTextBox";
            this.totalTextBox.Size = new System.Drawing.Size(80, 23);
            this.totalTextBox.TabIndex = 2;
            this.totalTextBox.Value = 0D;
            // 
            // totalLabel
            // 
            this.totalLabel.AutoSize = true;
            this.totalLabel.Location = new System.Drawing.Point(74, 109);
            this.totalLabel.Name = "totalLabel";
            this.totalLabel.Size = new System.Drawing.Size(80, 15);
            this.totalLabel.TabIndex = 12;
            this.totalLabel.Text = "Total Amount";
            this.totalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // totalInfoLabel
            // 
            this.totalInfoLabel.AutoSize = true;
            this.totalInfoLabel.Location = new System.Drawing.Point(246, 109);
            this.totalInfoLabel.Name = "totalInfoLabel";
            this.totalInfoLabel.Size = new System.Drawing.Size(289, 15);
            this.totalInfoLabel.TabIndex = 9;
            this.totalInfoLabel.Text = "Auto calculates unless some items have zero amount.";
            this.totalInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // shippingChargeTextBox
            // 
            this.shippingChargeTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.shippingChargeTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.shippingChargeTextBox.Location = new System.Drawing.Point(160, 135);
            this.shippingChargeTextBox.Name = "shippingChargeTextBox";
            this.shippingChargeTextBox.Size = new System.Drawing.Size(80, 23);
            this.shippingChargeTextBox.TabIndex = 3;
            this.shippingChargeTextBox.Value = 0D;
            // 
            // shippingChargeLabel
            // 
            this.shippingChargeLabel.AutoSize = true;
            this.shippingChargeLabel.Location = new System.Drawing.Point(59, 138);
            this.shippingChargeLabel.Name = "shippingChargeLabel";
            this.shippingChargeLabel.Size = new System.Drawing.Size(95, 15);
            this.shippingChargeLabel.TabIndex = 13;
            this.shippingChargeLabel.Text = "Shipping Charge";
            this.shippingChargeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userComboBox
            // 
            this.userComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.userComboBox.FormattingEnabled = true;
            this.userComboBox.Location = new System.Drawing.Point(160, 77);
            this.userComboBox.Name = "userComboBox";
            this.userComboBox.Size = new System.Drawing.Size(120, 23);
            this.userComboBox.TabIndex = 1;
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(96, 80);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(58, 15);
            this.userLabel.TabIndex = 37;
            this.userLabel.Text = "Placed By";
            this.userLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormSupplyOrderEdit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(724, 371);
            this.Controls.Add(this.userLabel);
            this.Controls.Add(this.userComboBox);
            this.Controls.Add(this.shippingChargeTextBox);
            this.Controls.Add(this.shippingChargeLabel);
            this.Controls.Add(this.totalInfoLabel);
            this.Controls.Add(this.totalTextBox);
            this.Controls.Add(this.totalLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.datePlacedLabel);
            this.Controls.Add(this.datePlacedTextBox);
            this.Controls.Add(this.noteTextBox);
            this.Controls.Add(this.noteLabel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.supplierTextBox);
            this.Controls.Add(this.supplierLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSupplyOrderEdit";
            this.ShowInTaskbar = false;
            this.Text = "Supply Order";
            this.Load += new System.EventHandler(this.FormSupplyOrderEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label supplierLabel;
        private System.Windows.Forms.TextBox supplierTextBox;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.TextBox noteTextBox;
        private System.Windows.Forms.Label noteLabel;
        private ValidDate datePlacedTextBox;
        private System.Windows.Forms.Label datePlacedLabel;
        private System.Windows.Forms.Label label3;
        private OpenDental.UI.ODCurrencyTextBox totalTextBox;
        private System.Windows.Forms.Label totalLabel;
        private System.Windows.Forms.Label totalInfoLabel;
        private OpenDental.UI.ODCurrencyTextBox shippingChargeTextBox;
        private System.Windows.Forms.Label shippingChargeLabel;
        private System.Windows.Forms.ComboBox userComboBox;
        private System.Windows.Forms.Label userLabel;
    }
}