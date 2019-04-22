namespace OpenDental
{
    partial class FormFriendlyException
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFriendlyException));
            this.cancelButton = new System.Windows.Forms.Button();
            this.detailsTabControl = new System.Windows.Forms.TabControl();
            this.stackTraceTabPage = new System.Windows.Forms.TabPage();
            this.stackTraceTextBox = new System.Windows.Forms.TextBox();
            this.queryTabPage = new System.Windows.Forms.TabPage();
            this.queryTextBox = new System.Windows.Forms.TextBox();
            this.printButton = new System.Windows.Forms.Button();
            this.friendlyMessageLabel = new System.Windows.Forms.Label();
            this.detailsButton = new System.Windows.Forms.Button();
            this.copyAllButton = new System.Windows.Forms.Button();
            this.detailsTabControl.SuspendLayout();
            this.stackTraceTabPage.SuspendLayout();
            this.queryTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(421, 318);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Close";
            // 
            // detailsTabControl
            // 
            this.detailsTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.detailsTabControl.Controls.Add(this.stackTraceTabPage);
            this.detailsTabControl.Controls.Add(this.queryTabPage);
            this.detailsTabControl.Location = new System.Drawing.Point(13, 89);
            this.detailsTabControl.Name = "detailsTabControl";
            this.detailsTabControl.SelectedIndex = 0;
            this.detailsTabControl.Size = new System.Drawing.Size(518, 224);
            this.detailsTabControl.TabIndex = 2;
            // 
            // stackTraceTabPage
            // 
            this.stackTraceTabPage.Controls.Add(this.stackTraceTextBox);
            this.stackTraceTabPage.Location = new System.Drawing.Point(4, 24);
            this.stackTraceTabPage.Name = "stackTraceTabPage";
            this.stackTraceTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.stackTraceTabPage.Size = new System.Drawing.Size(510, 196);
            this.stackTraceTabPage.TabIndex = 0;
            this.stackTraceTabPage.Text = "StackTrace";
            this.stackTraceTabPage.UseVisualStyleBackColor = true;
            // 
            // stackTraceTextBox
            // 
            this.stackTraceTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.stackTraceTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stackTraceTextBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.stackTraceTextBox.Location = new System.Drawing.Point(3, 3);
            this.stackTraceTextBox.MaximumSize = new System.Drawing.Size(1200, 800);
            this.stackTraceTextBox.Multiline = true;
            this.stackTraceTextBox.Name = "stackTraceTextBox";
            this.stackTraceTextBox.ReadOnly = true;
            this.stackTraceTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.stackTraceTextBox.Size = new System.Drawing.Size(504, 190);
            this.stackTraceTextBox.TabIndex = 0;
            this.stackTraceTextBox.TabStop = false;
            this.stackTraceTextBox.Text = "Error Details";
            // 
            // queryTabPage
            // 
            this.queryTabPage.Controls.Add(this.queryTextBox);
            this.queryTabPage.Location = new System.Drawing.Point(4, 24);
            this.queryTabPage.Name = "queryTabPage";
            this.queryTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.queryTabPage.Size = new System.Drawing.Size(510, 196);
            this.queryTabPage.TabIndex = 1;
            this.queryTabPage.Text = "Query";
            this.queryTabPage.UseVisualStyleBackColor = true;
            // 
            // queryTextBox
            // 
            this.queryTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.queryTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queryTextBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.queryTextBox.Location = new System.Drawing.Point(3, 3);
            this.queryTextBox.MaximumSize = new System.Drawing.Size(1200, 800);
            this.queryTextBox.Multiline = true;
            this.queryTextBox.Name = "queryTextBox";
            this.queryTextBox.ReadOnly = true;
            this.queryTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.queryTextBox.Size = new System.Drawing.Size(504, 190);
            this.queryTextBox.TabIndex = 0;
            this.queryTextBox.TabStop = false;
            this.queryTextBox.Text = "Query Detail";
            // 
            // printButton
            // 
            this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.printButton.Image = global::OpenDental.Properties.Resources.IconPrint;
            this.printButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.printButton.Location = new System.Drawing.Point(263, 320);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(110, 30);
            this.printButton.TabIndex = 5;
            this.printButton.Text = "&Print";
            this.printButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.printButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // friendlyMessageLabel
            // 
            this.friendlyMessageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.friendlyMessageLabel.Location = new System.Drawing.Point(13, 16);
            this.friendlyMessageLabel.Name = "friendlyMessageLabel";
            this.friendlyMessageLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.friendlyMessageLabel.Size = new System.Drawing.Size(518, 70);
            this.friendlyMessageLabel.TabIndex = 1;
            this.friendlyMessageLabel.Text = "Friendly Error Message";
            // 
            // detailsButton
            // 
            this.detailsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.detailsButton.Image = global::OpenDental.Properties.Resources.IconBulletArrowUp;
            this.detailsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.detailsButton.Location = new System.Drawing.Point(14, 319);
            this.detailsButton.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.detailsButton.Name = "detailsButton";
            this.detailsButton.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.detailsButton.Size = new System.Drawing.Size(110, 30);
            this.detailsButton.TabIndex = 3;
            this.detailsButton.Text = "&Details";
            this.detailsButton.Click += new System.EventHandler(this.detailsButton_Click);
            // 
            // copyAllButton
            // 
            this.copyAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copyAllButton.Location = new System.Drawing.Point(147, 319);
            this.copyAllButton.Name = "copyAllButton";
            this.copyAllButton.Size = new System.Drawing.Size(110, 30);
            this.copyAllButton.TabIndex = 4;
            this.copyAllButton.Text = "Copy All";
            this.copyAllButton.Click += new System.EventHandler(this.copyAllButton_Click);
            // 
            // FormFriendlyException
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(544, 361);
            this.Controls.Add(this.detailsTabControl);
            this.Controls.Add(this.friendlyMessageLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.copyAllButton);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.detailsButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 380);
            this.Name = "FormFriendlyException";
            this.ShowInTaskbar = false;
            this.Text = "Error Encountered";
            this.Load += new System.EventHandler(this.FormFriendlyException_Load);
            this.detailsTabControl.ResumeLayout(false);
            this.stackTraceTabPage.ResumeLayout(false);
            this.stackTraceTabPage.PerformLayout();
            this.queryTabPage.ResumeLayout(false);
            this.queryTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button detailsButton;
        private System.Windows.Forms.Label friendlyMessageLabel;
        private System.Windows.Forms.TextBox stackTraceTextBox;
        private System.Windows.Forms.TabControl detailsTabControl;
        private System.Windows.Forms.TabPage stackTraceTabPage;
        private System.Windows.Forms.TabPage queryTabPage;
        private System.Windows.Forms.TextBox queryTextBox;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.Button copyAllButton;
    }
}