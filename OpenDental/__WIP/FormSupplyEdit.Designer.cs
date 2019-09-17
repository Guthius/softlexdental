namespace OpenDental
{
    partial class FormSupplyEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSupplyEdit));
            this.supplierLabel = new System.Windows.Forms.Label();
            this.supplierTextBox = new System.Windows.Forms.TextBox();
            this.catalogNumberTextBox = new System.Windows.Forms.TextBox();
            this.catalogNumberLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.categoryLabel = new System.Windows.Forms.Label();
            this.levelDesiredLabel = new System.Windows.Forms.Label();
            this.hiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.priceLabel = new System.Windows.Forms.Label();
            this.categoryComboBox = new System.Windows.Forms.ComboBox();
            this.levelDesiredTextBox = new OpenDental.ValidDouble();
            this.priceTextBox = new OpenDental.UI.ODCurrencyTextBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.levelOnHandTextBox = new OpenDental.ValidDouble();
            this.levelOnHandLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // supplierLabel
            // 
            this.supplierLabel.AutoSize = true;
            this.supplierLabel.Location = new System.Drawing.Point(104, 22);
            this.supplierLabel.Name = "supplierLabel";
            this.supplierLabel.Size = new System.Drawing.Size(50, 15);
            this.supplierLabel.TabIndex = 0;
            this.supplierLabel.Text = "Supplier";
            this.supplierLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // supplierTextBox
            // 
            this.supplierTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.supplierTextBox.Location = new System.Drawing.Point(160, 19);
            this.supplierTextBox.Name = "supplierTextBox";
            this.supplierTextBox.ReadOnly = true;
            this.supplierTextBox.Size = new System.Drawing.Size(331, 23);
            this.supplierTextBox.TabIndex = 1;
            // 
            // catalogNumberTextBox
            // 
            this.catalogNumberTextBox.Location = new System.Drawing.Point(160, 77);
            this.catalogNumberTextBox.Name = "catalogNumberTextBox";
            this.catalogNumberTextBox.Size = new System.Drawing.Size(140, 23);
            this.catalogNumberTextBox.TabIndex = 5;
            // 
            // catalogNumberLabel
            // 
            this.catalogNumberLabel.AutoSize = true;
            this.catalogNumberLabel.Location = new System.Drawing.Point(32, 80);
            this.catalogNumberLabel.Name = "catalogNumberLabel";
            this.catalogNumberLabel.Size = new System.Drawing.Size(122, 15);
            this.catalogNumberLabel.TabIndex = 4;
            this.catalogNumberLabel.Text = "Catalog Item Number";
            this.catalogNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.Location = new System.Drawing.Point(160, 106);
            this.descriptionTextBox.MaxLength = 255;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(331, 23);
            this.descriptionTextBox.TabIndex = 7;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(87, 109);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(67, 15);
            this.descriptionLabel.TabIndex = 6;
            this.descriptionLabel.Text = "Description";
            this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // categoryLabel
            // 
            this.categoryLabel.AutoSize = true;
            this.categoryLabel.Location = new System.Drawing.Point(99, 51);
            this.categoryLabel.Name = "categoryLabel";
            this.categoryLabel.Size = new System.Drawing.Size(55, 15);
            this.categoryLabel.TabIndex = 2;
            this.categoryLabel.Text = "Category";
            this.categoryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // levelDesiredLabel
            // 
            this.levelDesiredLabel.AutoSize = true;
            this.levelDesiredLabel.Location = new System.Drawing.Point(78, 138);
            this.levelDesiredLabel.Name = "levelDesiredLabel";
            this.levelDesiredLabel.Size = new System.Drawing.Size(76, 15);
            this.levelDesiredLabel.TabIndex = 8;
            this.levelDesiredLabel.Text = "Level Desired";
            this.levelDesiredLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hiddenCheckBox
            // 
            this.hiddenCheckBox.AutoSize = true;
            this.hiddenCheckBox.Location = new System.Drawing.Point(160, 222);
            this.hiddenCheckBox.Name = "hiddenCheckBox";
            this.hiddenCheckBox.Size = new System.Drawing.Size(65, 19);
            this.hiddenCheckBox.TabIndex = 14;
            this.hiddenCheckBox.Text = "Hidden";
            this.hiddenCheckBox.UseVisualStyleBackColor = true;
            // 
            // priceLabel
            // 
            this.priceLabel.AutoSize = true;
            this.priceLabel.Location = new System.Drawing.Point(121, 196);
            this.priceLabel.Name = "priceLabel";
            this.priceLabel.Size = new System.Drawing.Size(33, 15);
            this.priceLabel.TabIndex = 12;
            this.priceLabel.Text = "Price";
            this.priceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // categoryComboBox
            // 
            this.categoryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.categoryComboBox.FormattingEnabled = true;
            this.categoryComboBox.ItemHeight = 15;
            this.categoryComboBox.Location = new System.Drawing.Point(160, 48);
            this.categoryComboBox.Name = "categoryComboBox";
            this.categoryComboBox.Size = new System.Drawing.Size(220, 23);
            this.categoryComboBox.TabIndex = 3;
            // 
            // levelDesiredTextBox
            // 
            this.levelDesiredTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.levelDesiredTextBox.Location = new System.Drawing.Point(160, 135);
            this.levelDesiredTextBox.MaxVal = 100000000D;
            this.levelDesiredTextBox.MinVal = -100000000D;
            this.levelDesiredTextBox.Name = "levelDesiredTextBox";
            this.levelDesiredTextBox.Size = new System.Drawing.Size(60, 23);
            this.levelDesiredTextBox.TabIndex = 9;
            // 
            // priceTextBox
            // 
            this.priceTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.priceTextBox.Location = new System.Drawing.Point(160, 193);
            this.priceTextBox.Name = "priceTextBox";
            this.priceTextBox.Size = new System.Drawing.Size(80, 23);
            this.priceTextBox.TabIndex = 13;
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deleteButton.Location = new System.Drawing.Point(13, 278);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 15;
            this.deleteButton.Text = "Delete";
            this.deleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(265, 278);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 16;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(381, 278);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 17;
            this.cancelButton.Text = "&Cancel";
            // 
            // levelOnHandTextBox
            // 
            this.levelOnHandTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.levelOnHandTextBox.Location = new System.Drawing.Point(160, 164);
            this.levelOnHandTextBox.MaxVal = 100000000D;
            this.levelOnHandTextBox.MinVal = -100000000D;
            this.levelOnHandTextBox.Name = "levelOnHandTextBox";
            this.levelOnHandTextBox.Size = new System.Drawing.Size(60, 23);
            this.levelOnHandTextBox.TabIndex = 11;
            // 
            // levelOnHandLabel
            // 
            this.levelOnHandLabel.AutoSize = true;
            this.levelOnHandLabel.Location = new System.Drawing.Point(69, 167);
            this.levelOnHandLabel.Name = "levelOnHandLabel";
            this.levelOnHandLabel.Size = new System.Drawing.Size(85, 15);
            this.levelOnHandLabel.TabIndex = 10;
            this.levelOnHandLabel.Text = "Level On Hand";
            this.levelOnHandLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormSupplyEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(504, 321);
            this.Controls.Add(this.levelOnHandTextBox);
            this.Controls.Add(this.levelOnHandLabel);
            this.Controls.Add(this.levelDesiredTextBox);
            this.Controls.Add(this.categoryComboBox);
            this.Controls.Add(this.priceTextBox);
            this.Controls.Add(this.priceLabel);
            this.Controls.Add(this.hiddenCheckBox);
            this.Controls.Add(this.levelDesiredLabel);
            this.Controls.Add(this.categoryLabel);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.catalogNumberTextBox);
            this.Controls.Add(this.catalogNumberLabel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.supplierTextBox);
            this.Controls.Add(this.supplierLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSupplyEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Supply";
            this.Load += new System.EventHandler(this.FormSupplyEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label supplierLabel;
        private System.Windows.Forms.TextBox supplierTextBox;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.TextBox catalogNumberTextBox;
        private System.Windows.Forms.Label catalogNumberLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label categoryLabel;
        private System.Windows.Forms.Label levelDesiredLabel;
        private System.Windows.Forms.CheckBox hiddenCheckBox;
        private System.Windows.Forms.Label priceLabel;
        private OpenDental.UI.ODCurrencyTextBox priceTextBox;
        private System.Windows.Forms.ComboBox categoryComboBox;
        private ValidDouble levelDesiredTextBox;
        private ValidDouble levelOnHandTextBox;
        private System.Windows.Forms.Label levelOnHandLabel;
    }
}