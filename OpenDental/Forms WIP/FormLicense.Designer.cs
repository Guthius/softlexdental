namespace OpenDental
{
    partial class FormLicense
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLicense));
            this.cancelButton = new System.Windows.Forms.Button();
            this.licenseListBox = new System.Windows.Forms.ListBox();
            this.licenseTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(711, 518);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Close";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // licenseListBox
            // 
            this.licenseListBox.IntegralHeight = false;
            this.licenseListBox.ItemHeight = 17;
            this.licenseListBox.Location = new System.Drawing.Point(13, 19);
            this.licenseListBox.Name = "licenseListBox";
            this.licenseListBox.Size = new System.Drawing.Size(170, 250);
            this.licenseListBox.TabIndex = 1;
            this.licenseListBox.SelectedIndexChanged += new System.EventHandler(this.licenseListBox_SelectedIndexChanged);
            // 
            // licenseTextBox
            // 
            this.licenseTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.licenseTextBox.Location = new System.Drawing.Point(189, 19);
            this.licenseTextBox.Multiline = true;
            this.licenseTextBox.Name = "licenseTextBox";
            this.licenseTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.licenseTextBox.Size = new System.Drawing.Size(632, 493);
            this.licenseTextBox.TabIndex = 2;
            // 
            // FormLicense
            // 
            this.ClientSize = new System.Drawing.Size(834, 561);
            this.Controls.Add(this.licenseTextBox);
            this.Controls.Add(this.licenseListBox);
            this.Controls.Add(this.cancelButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLicense";
            this.ShowInTaskbar = false;
            this.Text = "Licenses";
            this.Load += new System.EventHandler(this.FormLicense_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ListBox licenseListBox;
        private System.Windows.Forms.TextBox licenseTextBox;
    }
}