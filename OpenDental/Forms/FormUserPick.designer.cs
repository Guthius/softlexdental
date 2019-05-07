namespace OpenDental
{
    partial class FormUserPick
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUserPick));
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.userListBox = new System.Windows.Forms.ListBox();
            this.showButton = new System.Windows.Forms.Button();
            this.noneButton = new System.Windows.Forms.Button();
            this.allButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(241, 402);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(241, 438);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "&Cancel";
            // 
            // userListBox
            // 
            this.userListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userListBox.FormattingEnabled = true;
            this.userListBox.IntegralHeight = false;
            this.userListBox.ItemHeight = 15;
            this.userListBox.Location = new System.Drawing.Point(13, 19);
            this.userListBox.Name = "userListBox";
            this.userListBox.Size = new System.Drawing.Size(222, 449);
            this.userListBox.TabIndex = 0;
            this.userListBox.DoubleClick += new System.EventHandler(this.UserListBox_DoubleClick);
            // 
            // showButton
            // 
            this.showButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showButton.Location = new System.Drawing.Point(241, 19);
            this.showButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 30);
            this.showButton.Name = "showButton";
            this.showButton.Size = new System.Drawing.Size(110, 30);
            this.showButton.TabIndex = 1;
            this.showButton.Text = "Show All";
            this.showButton.Visible = false;
            this.showButton.Click += new System.EventHandler(this.Showbutton_Click);
            // 
            // noneButton
            // 
            this.noneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.noneButton.Location = new System.Drawing.Point(241, 118);
            this.noneButton.Name = "noneButton";
            this.noneButton.Size = new System.Drawing.Size(110, 30);
            this.noneButton.TabIndex = 3;
            this.noneButton.Text = "Select None";
            this.noneButton.Visible = false;
            this.noneButton.Click += new System.EventHandler(this.NoneButton_Click);
            // 
            // allButton
            // 
            this.allButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.allButton.Location = new System.Drawing.Point(241, 82);
            this.allButton.Name = "allButton";
            this.allButton.Size = new System.Drawing.Size(110, 30);
            this.allButton.TabIndex = 2;
            this.allButton.Text = "Select All";
            this.allButton.Visible = false;
            this.allButton.Click += new System.EventHandler(this.AllButton_Click);
            // 
            // FormUserPick
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(364, 481);
            this.Controls.Add(this.allButton);
            this.Controls.Add(this.noneButton);
            this.Controls.Add(this.showButton);
            this.Controls.Add(this.userListBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUserPick";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pick User";
            this.Load += new System.EventHandler(this.FormUserPick_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ListBox userListBox;
        private System.Windows.Forms.Button showButton;
        private System.Windows.Forms.Button noneButton;
        private System.Windows.Forms.Button allButton;
    }
}