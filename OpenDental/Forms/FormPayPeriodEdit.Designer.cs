namespace OpenDental
{
    partial class FormPayPeriodEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPayPeriodEdit));
            this.deleteButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.dateStartTextBox = new System.Windows.Forms.TextBox();
            this.dateStartLabel = new System.Windows.Forms.Label();
            this.dateEndTextBox = new System.Windows.Forms.TextBox();
            this.dateEndLabel = new System.Windows.Forms.Label();
            this.datePaycheckTextBox = new System.Windows.Forms.TextBox();
            this.datePaycheckLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deleteButton.Location = new System.Drawing.Point(13, 118);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 6;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(281, 118);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(165, 118);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 7;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // dateStartTextBox
            // 
            this.dateStartTextBox.Location = new System.Drawing.Point(40, 50);
            this.dateStartTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.dateStartTextBox.Name = "dateStartTextBox";
            this.dateStartTextBox.Size = new System.Drawing.Size(100, 23);
            this.dateStartTextBox.TabIndex = 1;
            this.dateStartTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ValidateDateTime);
            // 
            // dateStartLabel
            // 
            this.dateStartLabel.AutoSize = true;
            this.dateStartLabel.Location = new System.Drawing.Point(37, 32);
            this.dateStartLabel.Name = "dateStartLabel";
            this.dateStartLabel.Size = new System.Drawing.Size(58, 15);
            this.dateStartLabel.TabIndex = 0;
            this.dateStartLabel.Text = "Start Date";
            this.dateStartLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateEndTextBox
            // 
            this.dateEndTextBox.Location = new System.Drawing.Point(153, 50);
            this.dateEndTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.dateEndTextBox.Name = "dateEndTextBox";
            this.dateEndTextBox.Size = new System.Drawing.Size(100, 23);
            this.dateEndTextBox.TabIndex = 3;
            this.dateEndTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ValidateDateTime);
            // 
            // dateEndLabel
            // 
            this.dateEndLabel.AutoSize = true;
            this.dateEndLabel.Location = new System.Drawing.Point(150, 32);
            this.dateEndLabel.Name = "dateEndLabel";
            this.dateEndLabel.Size = new System.Drawing.Size(54, 15);
            this.dateEndLabel.TabIndex = 2;
            this.dateEndLabel.Text = "End Date";
            this.dateEndLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // datePaycheckTextBox
            // 
            this.datePaycheckTextBox.Location = new System.Drawing.Point(266, 50);
            this.datePaycheckTextBox.Name = "datePaycheckTextBox";
            this.datePaycheckTextBox.Size = new System.Drawing.Size(100, 23);
            this.datePaycheckTextBox.TabIndex = 5;
            this.datePaycheckTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.DatePaycheckTextBox_Validating);
            // 
            // datePaycheckLabel
            // 
            this.datePaycheckLabel.AutoSize = true;
            this.datePaycheckLabel.Location = new System.Drawing.Point(263, 32);
            this.datePaycheckLabel.Name = "datePaycheckLabel";
            this.datePaycheckLabel.Size = new System.Drawing.Size(84, 15);
            this.datePaycheckLabel.TabIndex = 4;
            this.datePaycheckLabel.Text = "Paycheck Date";
            this.datePaycheckLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormPayPeriodEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(404, 161);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.datePaycheckTextBox);
            this.Controls.Add(this.datePaycheckLabel);
            this.Controls.Add(this.dateEndTextBox);
            this.Controls.Add(this.dateEndLabel);
            this.Controls.Add(this.dateStartTextBox);
            this.Controls.Add(this.dateStartLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPayPeriodEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pay Period";
            this.Load += new System.EventHandler(this.FormPayPeriodEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.TextBox dateStartTextBox;
        private System.Windows.Forms.Label dateStartLabel;
        private System.Windows.Forms.TextBox dateEndTextBox;
        private System.Windows.Forms.Label dateEndLabel;
        private System.Windows.Forms.TextBox datePaycheckTextBox;
        private System.Windows.Forms.Label datePaycheckLabel;
        private System.Windows.Forms.Button deleteButton;
    }
}
