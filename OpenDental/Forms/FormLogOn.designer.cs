namespace OpenDental
{
    partial class FormLogOn
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogOn));
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.userListBox = new System.Windows.Forms.ListBox();
            this.userLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.CEMTUsersCheckBox = new System.Windows.Forms.CheckBox();
            this.pictureOpenDental = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureOpenDental)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(341, 338);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Exit";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(341, 302);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 2;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // userListBox
            // 
            this.userListBox.IntegralHeight = false;
            this.userListBox.ItemHeight = 15;
            this.userListBox.Location = new System.Drawing.Point(13, 108);
            this.userListBox.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.userListBox.Name = "userListBox";
            this.userListBox.Size = new System.Drawing.Size(150, 260);
            this.userListBox.TabIndex = 6;
            this.userListBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listUser_MouseUp);
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(10, 90);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(30, 15);
            this.userLabel.TabIndex = 4;
            this.userLabel.Text = "User";
            this.userLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(193, 90);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(57, 15);
            this.passwordLabel.TabIndex = 7;
            this.passwordLabel.Text = "Password";
            this.passwordLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(196, 108);
            this.passwordTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(215, 23);
            this.passwordTextBox.TabIndex = 0;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // userTextBox
            // 
            this.userTextBox.Location = new System.Drawing.Point(13, 108);
            this.userTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(150, 23);
            this.userTextBox.TabIndex = 5;
            this.userTextBox.Visible = false;
            // 
            // CEMTUsersCheckBox
            // 
            this.CEMTUsersCheckBox.AutoSize = true;
            this.CEMTUsersCheckBox.Location = new System.Drawing.Point(196, 154);
            this.CEMTUsersCheckBox.Name = "CEMTUsersCheckBox";
            this.CEMTUsersCheckBox.Size = new System.Drawing.Size(120, 19);
            this.CEMTUsersCheckBox.TabIndex = 1;
            this.CEMTUsersCheckBox.Text = "Show CEMT users";
            this.CEMTUsersCheckBox.UseVisualStyleBackColor = true;
            this.CEMTUsersCheckBox.Visible = false;
            this.CEMTUsersCheckBox.CheckedChanged += new System.EventHandler(this.CEMTUsersCheckBox_CheckedChanged);
            // 
            // pictureOpenDental
            // 
            this.pictureOpenDental.BackColor = System.Drawing.Color.White;
            this.pictureOpenDental.Image = ((System.Drawing.Image)(resources.GetObject("pictureOpenDental.Image")));
            this.pictureOpenDental.Location = new System.Drawing.Point(13, 10);
            this.pictureOpenDental.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.pictureOpenDental.Name = "pictureOpenDental";
            this.pictureOpenDental.Size = new System.Drawing.Size(200, 50);
            this.pictureOpenDental.TabIndex = 59;
            this.pictureOpenDental.TabStop = false;
            // 
            // FormLogOn
            // 
            this.AcceptButton = this.acceptButton;
            this.ClientSize = new System.Drawing.Size(464, 381);
            this.Controls.Add(this.pictureOpenDental);
            this.Controls.Add(this.CEMTUsersCheckBox);
            this.Controls.Add(this.userTextBox);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.userLabel);
            this.Controls.Add(this.userListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLogOn";
            this.ShowInTaskbar = false;
            this.Text = "Log On";
            this.Load += new System.EventHandler(this.FormLogOn_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureOpenDental)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.ListBox userListBox;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.TextBox userTextBox;
        private System.Windows.Forms.CheckBox CEMTUsersCheckBox;
        private System.Windows.Forms.PictureBox pictureOpenDental;
    }
}