namespace CodeBase
{
    partial class MsgBoxCopyPaste
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MsgBoxCopyPaste));
            this.acceptButton = new System.Windows.Forms.Button();
            this.mainTextBox = new System.Windows.Forms.TextBox();
            this.printButton = new System.Windows.Forms.Button();
            this.copyAllButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.acceptButton.Location = new System.Drawing.Point(562, 569);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 1;
            this.acceptButton.Text = "&OK";
            // 
            // mainTextBox
            // 
            this.mainTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.mainTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainTextBox.Location = new System.Drawing.Point(12, 12);
            this.mainTextBox.Multiline = true;
            this.mainTextBox.Name = "mainTextBox";
            this.mainTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mainTextBox.Size = new System.Drawing.Size(660, 551);
            this.mainTextBox.TabIndex = 2;
            // 
            // printButton
            // 
            this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.printButton.Location = new System.Drawing.Point(128, 569);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(110, 30);
            this.printButton.TabIndex = 3;
            this.printButton.Text = "    &Print";
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // copyAllButton
            // 
            this.copyAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copyAllButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.copyAllButton.Location = new System.Drawing.Point(12, 569);
            this.copyAllButton.Name = "copyAllButton";
            this.copyAllButton.Size = new System.Drawing.Size(110, 30);
            this.copyAllButton.TabIndex = 4;
            this.copyAllButton.Text = "Copy All";
            this.copyAllButton.Click += new System.EventHandler(this.copyAllButton_Click);
            // 
            // MsgBoxCopyPaste
            // 
            this.AcceptButton = this.acceptButton;
            this.ClientSize = new System.Drawing.Size(684, 611);
            this.Controls.Add(this.copyAllButton);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.mainTextBox);
            this.Controls.Add(this.acceptButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "MsgBoxCopyPaste";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button copyAllButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.TextBox mainTextBox;
        private System.Windows.Forms.Button printButton;
    }
}