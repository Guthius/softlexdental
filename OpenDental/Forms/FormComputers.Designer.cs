namespace OpenDental
{
    partial class FormComputers
    {
        private System.ComponentModel.Container components = null;

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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormComputers));
            this.computerListBox = new System.Windows.Forms.ListBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.deleteButton = new System.Windows.Forms.Button();
            this.computerNameLabel = new System.Windows.Forms.Label();
            this.setSimpleGraphicsButton = new System.Windows.Forms.Button();
            this.fixLabel = new System.Windows.Forms.Label();
            this.fixGroupBox = new System.Windows.Forms.GroupBox();
            this.currentComputerTextBox = new System.Windows.Forms.TextBox();
            this.currentComputerLabel = new System.Windows.Forms.Label();
            this.workstationGroupBox = new System.Windows.Forms.GroupBox();
            this.databaseGroupBox = new System.Windows.Forms.GroupBox();
            this.serviceCommentTextBox = new System.Windows.Forms.TextBox();
            this.serviceVersionTextBox = new System.Windows.Forms.TextBox();
            this.serviceNameTextBox = new System.Windows.Forms.TextBox();
            this.serverNameTextBox = new System.Windows.Forms.TextBox();
            this.serviceCommentLabel = new System.Windows.Forms.Label();
            this.serviceVersionLabel = new System.Windows.Forms.Label();
            this.serviceNameLabel = new System.Windows.Forms.Label();
            this.serverNameLabel = new System.Windows.Forms.Label();
            this.fixGroupBox.SuspendLayout();
            this.workstationGroupBox.SuspendLayout();
            this.databaseGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // computerListBox
            // 
            this.computerListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.computerListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.computerListBox.IntegralHeight = false;
            this.computerListBox.ItemHeight = 24;
            this.computerListBox.Items.AddRange(new object[] {
            ""});
            this.computerListBox.Location = new System.Drawing.Point(6, 130);
            this.computerListBox.Name = "computerListBox";
            this.computerListBox.Size = new System.Drawing.Size(280, 218);
            this.computerListBox.TabIndex = 4;
            this.computerListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ComputerListBox_DrawItem);
            this.computerListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.computerListBox_MouseDoubleClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(441, 598);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Close";
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(6, 62);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(526, 50);
            this.infoLabel.TabIndex = 2;
            this.infoLabel.Text = "Computers are added to this list every time you use Open Dental.  You can safely " +
    "delete unused computer names from this list to speed up messaging.";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Location = new System.Drawing.Point(6, 354);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 5;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // computerNameLabel
            // 
            this.computerNameLabel.AutoSize = true;
            this.computerNameLabel.Location = new System.Drawing.Point(6, 112);
            this.computerNameLabel.Name = "computerNameLabel";
            this.computerNameLabel.Size = new System.Drawing.Size(96, 15);
            this.computerNameLabel.TabIndex = 3;
            this.computerNameLabel.Text = "Computer Name";
            this.computerNameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // setSimpleGraphicsButton
            // 
            this.setSimpleGraphicsButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.setSimpleGraphicsButton.Location = new System.Drawing.Point(47, 184);
            this.setSimpleGraphicsButton.Name = "setSimpleGraphicsButton";
            this.setSimpleGraphicsButton.Size = new System.Drawing.Size(150, 30);
            this.setSimpleGraphicsButton.TabIndex = 1;
            this.setSimpleGraphicsButton.Text = "Use Simple Graphics";
            this.setSimpleGraphicsButton.Click += new System.EventHandler(this.setSimpleGraphicsButton_Click);
            // 
            // fixLabel
            // 
            this.fixLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fixLabel.Location = new System.Drawing.Point(13, 26);
            this.fixLabel.Name = "fixLabel";
            this.fixLabel.Size = new System.Drawing.Size(214, 155);
            this.fixLabel.TabIndex = 0;
            this.fixLabel.Text = resources.GetString("fixLabel.Text");
            this.fixLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // fixGroupBox
            // 
            this.fixGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fixGroupBox.Controls.Add(this.setSimpleGraphicsButton);
            this.fixGroupBox.Controls.Add(this.fixLabel);
            this.fixGroupBox.Location = new System.Drawing.Point(292, 122);
            this.fixGroupBox.Name = "fixGroupBox";
            this.fixGroupBox.Padding = new System.Windows.Forms.Padding(10);
            this.fixGroupBox.Size = new System.Drawing.Size(240, 227);
            this.fixGroupBox.TabIndex = 6;
            this.fixGroupBox.TabStop = false;
            this.fixGroupBox.Text = "Fix a Workstation";
            // 
            // currentComputerTextBox
            // 
            this.currentComputerTextBox.Enabled = false;
            this.currentComputerTextBox.Location = new System.Drawing.Point(120, 29);
            this.currentComputerTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.currentComputerTextBox.Name = "currentComputerTextBox";
            this.currentComputerTextBox.ReadOnly = true;
            this.currentComputerTextBox.Size = new System.Drawing.Size(282, 23);
            this.currentComputerTextBox.TabIndex = 1;
            // 
            // currentComputerLabel
            // 
            this.currentComputerLabel.AutoSize = true;
            this.currentComputerLabel.Location = new System.Drawing.Point(10, 32);
            this.currentComputerLabel.Name = "currentComputerLabel";
            this.currentComputerLabel.Size = new System.Drawing.Size(104, 15);
            this.currentComputerLabel.TabIndex = 0;
            this.currentComputerLabel.Text = "Current Computer";
            this.currentComputerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // workstationGroupBox
            // 
            this.workstationGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workstationGroupBox.Controls.Add(this.computerNameLabel);
            this.workstationGroupBox.Controls.Add(this.fixGroupBox);
            this.workstationGroupBox.Controls.Add(this.currentComputerTextBox);
            this.workstationGroupBox.Controls.Add(this.computerListBox);
            this.workstationGroupBox.Controls.Add(this.deleteButton);
            this.workstationGroupBox.Controls.Add(this.currentComputerLabel);
            this.workstationGroupBox.Controls.Add(this.infoLabel);
            this.workstationGroupBox.Location = new System.Drawing.Point(13, 185);
            this.workstationGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.workstationGroupBox.Name = "workstationGroupBox";
            this.workstationGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.workstationGroupBox.Size = new System.Drawing.Size(538, 390);
            this.workstationGroupBox.TabIndex = 1;
            this.workstationGroupBox.TabStop = false;
            this.workstationGroupBox.Text = "Workstation";
            // 
            // databaseGroupBox
            // 
            this.databaseGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.databaseGroupBox.Controls.Add(this.serviceCommentTextBox);
            this.databaseGroupBox.Controls.Add(this.serviceVersionTextBox);
            this.databaseGroupBox.Controls.Add(this.serviceNameTextBox);
            this.databaseGroupBox.Controls.Add(this.serverNameTextBox);
            this.databaseGroupBox.Controls.Add(this.serviceCommentLabel);
            this.databaseGroupBox.Controls.Add(this.serviceVersionLabel);
            this.databaseGroupBox.Controls.Add(this.serviceNameLabel);
            this.databaseGroupBox.Controls.Add(this.serverNameLabel);
            this.databaseGroupBox.Location = new System.Drawing.Point(13, 19);
            this.databaseGroupBox.Name = "databaseGroupBox";
            this.databaseGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.databaseGroupBox.Size = new System.Drawing.Size(538, 160);
            this.databaseGroupBox.TabIndex = 0;
            this.databaseGroupBox.TabStop = false;
            this.databaseGroupBox.Text = "Database Server";
            // 
            // serviceCommentTextBox
            // 
            this.serviceCommentTextBox.Enabled = false;
            this.serviceCommentTextBox.Location = new System.Drawing.Point(120, 116);
            this.serviceCommentTextBox.Name = "serviceCommentTextBox";
            this.serviceCommentTextBox.ReadOnly = true;
            this.serviceCommentTextBox.Size = new System.Drawing.Size(282, 23);
            this.serviceCommentTextBox.TabIndex = 7;
            // 
            // serviceVersionTextBox
            // 
            this.serviceVersionTextBox.Enabled = false;
            this.serviceVersionTextBox.Location = new System.Drawing.Point(120, 87);
            this.serviceVersionTextBox.Name = "serviceVersionTextBox";
            this.serviceVersionTextBox.ReadOnly = true;
            this.serviceVersionTextBox.Size = new System.Drawing.Size(282, 23);
            this.serviceVersionTextBox.TabIndex = 5;
            // 
            // serviceNameTextBox
            // 
            this.serviceNameTextBox.Enabled = false;
            this.serviceNameTextBox.Location = new System.Drawing.Point(120, 58);
            this.serviceNameTextBox.Name = "serviceNameTextBox";
            this.serviceNameTextBox.ReadOnly = true;
            this.serviceNameTextBox.Size = new System.Drawing.Size(282, 23);
            this.serviceNameTextBox.TabIndex = 3;
            // 
            // serverNameTextBox
            // 
            this.serverNameTextBox.Enabled = false;
            this.serverNameTextBox.Location = new System.Drawing.Point(120, 29);
            this.serverNameTextBox.Name = "serverNameTextBox";
            this.serverNameTextBox.ReadOnly = true;
            this.serverNameTextBox.Size = new System.Drawing.Size(282, 23);
            this.serverNameTextBox.TabIndex = 1;
            // 
            // serviceCommentLabel
            // 
            this.serviceCommentLabel.AutoSize = true;
            this.serviceCommentLabel.Location = new System.Drawing.Point(13, 119);
            this.serviceCommentLabel.Name = "serviceCommentLabel";
            this.serviceCommentLabel.Size = new System.Drawing.Size(101, 15);
            this.serviceCommentLabel.TabIndex = 6;
            this.serviceCommentLabel.Text = "Service Comment";
            this.serviceCommentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // serviceVersionLabel
            // 
            this.serviceVersionLabel.AutoSize = true;
            this.serviceVersionLabel.Location = new System.Drawing.Point(29, 90);
            this.serviceVersionLabel.Name = "serviceVersionLabel";
            this.serviceVersionLabel.Size = new System.Drawing.Size(85, 15);
            this.serviceVersionLabel.TabIndex = 4;
            this.serviceVersionLabel.Text = "Service Version";
            this.serviceVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // serviceNameLabel
            // 
            this.serviceNameLabel.AutoSize = true;
            this.serviceNameLabel.Location = new System.Drawing.Point(35, 61);
            this.serviceNameLabel.Name = "serviceNameLabel";
            this.serviceNameLabel.Size = new System.Drawing.Size(79, 15);
            this.serviceNameLabel.TabIndex = 2;
            this.serviceNameLabel.Text = "Service Name";
            this.serviceNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // serverNameLabel
            // 
            this.serverNameLabel.AutoSize = true;
            this.serverNameLabel.Location = new System.Drawing.Point(40, 32);
            this.serverNameLabel.Name = "serverNameLabel";
            this.serverNameLabel.Size = new System.Drawing.Size(74, 15);
            this.serverNameLabel.TabIndex = 0;
            this.serverNameLabel.Text = "Server Name";
            this.serverNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormComputers
            // 
            this.AcceptButton = this.cancelButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(564, 641);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.workstationGroupBox);
            this.Controls.Add(this.databaseGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(0, 0);
            this.Name = "FormComputers";
            this.ShowInTaskbar = false;
            this.Text = "Computers";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormComputers_Closing);
            this.Load += new System.EventHandler(this.FormComputers_Load);
            this.fixGroupBox.ResumeLayout(false);
            this.workstationGroupBox.ResumeLayout(false);
            this.workstationGroupBox.PerformLayout();
            this.databaseGroupBox.ResumeLayout(false);
            this.databaseGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.ListBox computerListBox;
        private System.Windows.Forms.Label computerNameLabel;
        private System.Windows.Forms.Label fixLabel;
        private System.Windows.Forms.GroupBox fixGroupBox;
        private System.Windows.Forms.Label currentComputerLabel;
        private System.Windows.Forms.TextBox currentComputerTextBox;
        private System.Windows.Forms.GroupBox workstationGroupBox;
        private System.Windows.Forms.GroupBox databaseGroupBox;
        private System.Windows.Forms.TextBox serviceCommentTextBox;
        private System.Windows.Forms.TextBox serviceVersionTextBox;
        private System.Windows.Forms.TextBox serviceNameTextBox;
        private System.Windows.Forms.TextBox serverNameTextBox;
        private System.Windows.Forms.Label serviceCommentLabel;
        private System.Windows.Forms.Label serviceVersionLabel;
        private System.Windows.Forms.Label serviceNameLabel;
        private System.Windows.Forms.Label serverNameLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button setSimpleGraphicsButton;
    }
}