namespace OpenDental
{
    partial class FormRegistrationKey
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRegistrationKey));
            this.keyLabel = new System.Windows.Forms.Label();
            this.keyTextBox = new System.Windows.Forms.TextBox();
            this.agreementLabel = new System.Windows.Forms.Label();
            this.agreeCheckBox = new System.Windows.Forms.CheckBox();
            this.agreementRichTextBox = new System.Windows.Forms.RichTextBox();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // keyLabel
            // 
            this.keyLabel.AutoSize = true;
            this.keyLabel.Location = new System.Drawing.Point(62, 62);
            this.keyLabel.Name = "keyLabel";
            this.keyLabel.Size = new System.Drawing.Size(92, 15);
            this.keyLabel.TabIndex = 2;
            this.keyLabel.Text = "Registration Key";
            this.keyLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // keyTextBox
            // 
            this.keyTextBox.Location = new System.Drawing.Point(160, 59);
            this.keyTextBox.Name = "keyTextBox";
            this.keyTextBox.Size = new System.Drawing.Size(243, 23);
            this.keyTextBox.TabIndex = 0;
            this.keyTextBox.WordWrap = false;
            this.keyTextBox.TextChanged += new System.EventHandler(this.keyTextBox_TextChanged);
            this.keyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyTextBox_KeyPress);
            this.keyTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyTextBox_KeyUp);
            // 
            // agreementLabel
            // 
            this.agreementLabel.AutoSize = true;
            this.agreementLabel.Location = new System.Drawing.Point(10, 102);
            this.agreementLabel.Name = "agreementLabel";
            this.agreementLabel.Size = new System.Drawing.Size(108, 15);
            this.agreementLabel.TabIndex = 6;
            this.agreementLabel.Text = "License Agreement";
            this.agreementLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // agreeCheckBox
            // 
            this.agreeCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.agreeCheckBox.Location = new System.Drawing.Point(13, 480);
            this.agreeCheckBox.Name = "agreeCheckBox";
            this.agreeCheckBox.Size = new System.Drawing.Size(373, 17);
            this.agreeCheckBox.TabIndex = 7;
            this.agreeCheckBox.Text = "I agree to the terms of the above license agreement in its entirety.";
            this.agreeCheckBox.UseVisualStyleBackColor = true;
            this.agreeCheckBox.CheckedChanged += new System.EventHandler(this.agreeCheckBox_CheckedChanged);
            // 
            // agreementRichTextBox
            // 
            this.agreementRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.agreementRichTextBox.Location = new System.Drawing.Point(13, 120);
            this.agreementRichTextBox.Name = "agreementRichTextBox";
            this.agreementRichTextBox.Size = new System.Drawing.Size(708, 354);
            this.agreementRichTextBox.TabIndex = 8;
            this.agreementRichTextBox.Text = "";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Enabled = false;
            this.acceptButton.Location = new System.Drawing.Point(495, 518);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(611, 518);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // FormRegistrationKey
            // 
            this.ClientSize = new System.Drawing.Size(734, 561);
            this.Controls.Add(this.agreementRichTextBox);
            this.Controls.Add(this.agreeCheckBox);
            this.Controls.Add(this.agreementLabel);
            this.Controls.Add(this.keyTextBox);
            this.Controls.Add(this.keyLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HeaderText = "Enter your registration key below to activate Open Dental.";
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRegistrationKey";
            this.ShowInTaskbar = false;
            this.Text = "Registration Key";
            this.Load += new System.EventHandler(this.FormRegistrationKey_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Label keyLabel;
        private System.Windows.Forms.TextBox keyTextBox;
        private System.Windows.Forms.Label agreementLabel;
        private System.Windows.Forms.CheckBox agreeCheckBox;
        private System.Windows.Forms.RichTextBox agreementRichTextBox;
    }
}