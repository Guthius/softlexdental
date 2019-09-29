namespace OpenDental
{
    partial class FormAutoItemEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAutoItemEdit));
            this.codeTextBox = new System.Windows.Forms.TextBox();
            this.codeLabel = new System.Windows.Forms.Label();
            this.conditionsListBox = new System.Windows.Forms.ListBox();
            this.changeButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.conditionsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // codeTextBox
            // 
            this.codeTextBox.Location = new System.Drawing.Point(123, 19);
            this.codeTextBox.Name = "codeTextBox";
            this.codeTextBox.ReadOnly = true;
            this.codeTextBox.Size = new System.Drawing.Size(120, 23);
            this.codeTextBox.TabIndex = 1;
            // 
            // codeLabel
            // 
            this.codeLabel.AutoSize = true;
            this.codeLabel.Location = new System.Drawing.Point(82, 22);
            this.codeLabel.Name = "codeLabel";
            this.codeLabel.Size = new System.Drawing.Size(35, 15);
            this.codeLabel.TabIndex = 0;
            this.codeLabel.Text = "Code";
            // 
            // conditionsListBox
            // 
            this.conditionsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.conditionsListBox.IntegralHeight = false;
            this.conditionsListBox.ItemHeight = 15;
            this.conditionsListBox.Location = new System.Drawing.Point(123, 48);
            this.conditionsListBox.Name = "conditionsListBox";
            this.conditionsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.conditionsListBox.Size = new System.Drawing.Size(268, 314);
            this.conditionsListBox.TabIndex = 4;
            // 
            // changeButton
            // 
            this.changeButton.Location = new System.Drawing.Point(249, 18);
            this.changeButton.Name = "changeButton";
            this.changeButton.Size = new System.Drawing.Size(80, 25);
            this.changeButton.TabIndex = 2;
            this.changeButton.Text = "C&hange";
            this.changeButton.Click += new System.EventHandler(this.ChangeButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(281, 368);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(165, 368);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 5;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // conditionsLabel
            // 
            this.conditionsLabel.AutoSize = true;
            this.conditionsLabel.Location = new System.Drawing.Point(52, 49);
            this.conditionsLabel.Name = "conditionsLabel";
            this.conditionsLabel.Size = new System.Drawing.Size(65, 15);
            this.conditionsLabel.TabIndex = 3;
            this.conditionsLabel.Text = "Conditions";
            // 
            // FormAutoItemEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(404, 411);
            this.Controls.Add(this.conditionsLabel);
            this.Controls.Add(this.changeButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.conditionsListBox);
            this.Controls.Add(this.codeLabel);
            this.Controls.Add(this.codeTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAutoItemEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto Code Item";
            this.Load += new System.EventHandler(this.FormAutoItemEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label codeLabel;
        private System.Windows.Forms.TextBox codeTextBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Label conditionsLabel;
        private System.Windows.Forms.ListBox conditionsListBox;
        private System.Windows.Forms.Button changeButton;
    }
}
