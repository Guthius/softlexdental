namespace OpenDental
{
    partial class FormTimeCardManage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTimeCardManage));
            this.payPeriodGroupBox = new System.Windows.Forms.GroupBox();
            this.datePaycheckTextBox = new System.Windows.Forms.TextBox();
            this.dateEndTextBox = new System.Windows.Forms.TextBox();
            this.dateStartTextBox = new System.Windows.Forms.TextBox();
            this.rightButton = new System.Windows.Forms.Button();
            this.leftButton = new System.Windows.Forms.Button();
            this.datePaycheckLabel = new System.Windows.Forms.Label();
            this.dateStartLabel = new System.Windows.Forms.Label();
            this.dateEndLabel = new System.Windows.Forms.Label();
            this.grid = new OpenDental.UI.ODGrid();
            this.dailyButton = new System.Windows.Forms.Button();
            this.weeklyButton = new System.Windows.Forms.Button();
            this.printAllButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.clearAutoButton = new System.Windows.Forms.Button();
            this.clearManualButton = new System.Windows.Forms.Button();
            this.printSelectedButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.clinicComboBox = new System.Windows.Forms.ComboBox();
            this.clinicLabel = new System.Windows.Forms.Label();
            this.printGridButton = new System.Windows.Forms.Button();
            this.exportGridButton = new System.Windows.Forms.Button();
            this.exportAdpButton = new System.Windows.Forms.Button();
            this.setupButton = new System.Windows.Forms.Button();
            this.payPeriodGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // payPeriodGroupBox
            // 
            this.payPeriodGroupBox.Controls.Add(this.datePaycheckTextBox);
            this.payPeriodGroupBox.Controls.Add(this.dateEndTextBox);
            this.payPeriodGroupBox.Controls.Add(this.dateStartTextBox);
            this.payPeriodGroupBox.Controls.Add(this.rightButton);
            this.payPeriodGroupBox.Controls.Add(this.leftButton);
            this.payPeriodGroupBox.Controls.Add(this.datePaycheckLabel);
            this.payPeriodGroupBox.Controls.Add(this.dateStartLabel);
            this.payPeriodGroupBox.Controls.Add(this.dateEndLabel);
            this.payPeriodGroupBox.Location = new System.Drawing.Point(13, 19);
            this.payPeriodGroupBox.Name = "payPeriodGroupBox";
            this.payPeriodGroupBox.Size = new System.Drawing.Size(500, 80);
            this.payPeriodGroupBox.TabIndex = 0;
            this.payPeriodGroupBox.TabStop = false;
            this.payPeriodGroupBox.Text = "Pay Period";
            // 
            // datePaycheckTextBox
            // 
            this.datePaycheckTextBox.Location = new System.Drawing.Point(332, 37);
            this.datePaycheckTextBox.Name = "datePaycheckTextBox";
            this.datePaycheckTextBox.ReadOnly = true;
            this.datePaycheckTextBox.Size = new System.Drawing.Size(100, 23);
            this.datePaycheckTextBox.TabIndex = 7;
            // 
            // dateEndTextBox
            // 
            this.dateEndTextBox.Location = new System.Drawing.Point(226, 37);
            this.dateEndTextBox.Name = "dateEndTextBox";
            this.dateEndTextBox.ReadOnly = true;
            this.dateEndTextBox.Size = new System.Drawing.Size(100, 23);
            this.dateEndTextBox.TabIndex = 5;
            // 
            // dateStartTextBox
            // 
            this.dateStartTextBox.Location = new System.Drawing.Point(120, 37);
            this.dateStartTextBox.Name = "dateStartTextBox";
            this.dateStartTextBox.ReadOnly = true;
            this.dateStartTextBox.Size = new System.Drawing.Size(100, 23);
            this.dateStartTextBox.TabIndex = 3;
            // 
            // rightButton
            // 
            this.rightButton.Image = global::OpenDental.Properties.Resources.IconArrowRight;
            this.rightButton.Location = new System.Drawing.Point(60, 32);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(35, 30);
            this.rightButton.TabIndex = 1;
            this.rightButton.Click += new System.EventHandler(this.RightButton_Click);
            // 
            // leftButton
            // 
            this.leftButton.Image = global::OpenDental.Properties.Resources.IconArrowLeft;
            this.leftButton.Location = new System.Drawing.Point(19, 32);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(35, 30);
            this.leftButton.TabIndex = 0;
            this.leftButton.Click += new System.EventHandler(this.LeftButton_Click);
            // 
            // datePaycheckLabel
            // 
            this.datePaycheckLabel.AutoSize = true;
            this.datePaycheckLabel.Location = new System.Drawing.Point(329, 19);
            this.datePaycheckLabel.Name = "datePaycheckLabel";
            this.datePaycheckLabel.Size = new System.Drawing.Size(84, 15);
            this.datePaycheckLabel.TabIndex = 6;
            this.datePaycheckLabel.Text = "Paycheck Date";
            // 
            // dateStartLabel
            // 
            this.dateStartLabel.AutoSize = true;
            this.dateStartLabel.Location = new System.Drawing.Point(117, 19);
            this.dateStartLabel.Name = "dateStartLabel";
            this.dateStartLabel.Size = new System.Drawing.Size(58, 15);
            this.dateStartLabel.TabIndex = 2;
            this.dateStartLabel.Text = "Start Date";
            // 
            // dateEndLabel
            // 
            this.dateEndLabel.AutoSize = true;
            this.dateEndLabel.Location = new System.Drawing.Point(223, 19);
            this.dateEndLabel.Name = "dateEndLabel";
            this.dateEndLabel.Size = new System.Drawing.Size(54, 15);
            this.dateEndLabel.TabIndex = 4;
            this.dateEndLabel.Text = "End Date";
            // 
            // grid
            // 
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.grid.EditableEnterMovesDown = false;
            this.grid.HasAddButton = false;
            this.grid.HasDropDowns = false;
            this.grid.HasMultilineHeaders = false;
            this.grid.HScrollVisible = false;
            this.grid.Location = new System.Drawing.Point(13, 105);
            this.grid.Name = "grid";
            this.grid.ScrollValue = 0;
            this.grid.SelectionMode = OpenDental.UI.GridSelectionMode.Multiple;
            this.grid.Size = new System.Drawing.Size(1118, 477);
            this.grid.TabIndex = 9;
            this.grid.Title = "Employee Time Cards";
            this.grid.TitleVisible = true;
            this.grid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.Grid_CellDoubleClick);
            // 
            // dailyButton
            // 
            this.dailyButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dailyButton.Location = new System.Drawing.Point(6, 22);
            this.dailyButton.Name = "dailyButton";
            this.dailyButton.Size = new System.Drawing.Size(110, 30);
            this.dailyButton.TabIndex = 0;
            this.dailyButton.Text = "Daily";
            this.dailyButton.Click += new System.EventHandler(this.DailyButton_Click);
            // 
            // weeklyButton
            // 
            this.weeklyButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.weeklyButton.Location = new System.Drawing.Point(122, 22);
            this.weeklyButton.Name = "weeklyButton";
            this.weeklyButton.Size = new System.Drawing.Size(110, 30);
            this.weeklyButton.TabIndex = 1;
            this.weeklyButton.Text = "Weekly";
            this.weeklyButton.Click += new System.EventHandler(this.WeeklyButton_Click);
            // 
            // printAllButton
            // 
            this.printAllButton.Image = ((System.Drawing.Image)(resources.GetObject("printAllButton.Image")));
            this.printAllButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.printAllButton.Location = new System.Drawing.Point(6, 22);
            this.printAllButton.Name = "printAllButton";
            this.printAllButton.Size = new System.Drawing.Size(110, 30);
            this.printAllButton.TabIndex = 0;
            this.printAllButton.Text = "&Print All";
            this.printAllButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.printAllButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.printAllButton.Click += new System.EventHandler(this.PrintAllButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(1021, 618);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(110, 30);
            this.closeButton.TabIndex = 12;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // clearAutoButton
            // 
            this.clearAutoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearAutoButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.clearAutoButton.Location = new System.Drawing.Point(749, 55);
            this.clearAutoButton.Name = "clearAutoButton";
            this.clearAutoButton.Size = new System.Drawing.Size(150, 30);
            this.clearAutoButton.TabIndex = 4;
            this.clearAutoButton.Text = "Clear Auto Adjusts";
            this.clearAutoButton.Click += new System.EventHandler(this.ClearAutoButton_Click);
            // 
            // clearManualButton
            // 
            this.clearManualButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearManualButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.clearManualButton.Location = new System.Drawing.Point(749, 19);
            this.clearManualButton.Name = "clearManualButton";
            this.clearManualButton.Size = new System.Drawing.Size(150, 30);
            this.clearManualButton.TabIndex = 3;
            this.clearManualButton.Text = "Clear Manual Adjusts";
            this.clearManualButton.Click += new System.EventHandler(this.ClearManualButton_Click);
            // 
            // printSelectedButton
            // 
            this.printSelectedButton.Image = ((System.Drawing.Image)(resources.GetObject("printSelectedButton.Image")));
            this.printSelectedButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.printSelectedButton.Location = new System.Drawing.Point(122, 22);
            this.printSelectedButton.Name = "printSelectedButton";
            this.printSelectedButton.Size = new System.Drawing.Size(150, 30);
            this.printSelectedButton.TabIndex = 1;
            this.printSelectedButton.Text = "Print Selected";
            this.printSelectedButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.printSelectedButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.printSelectedButton.Click += new System.EventHandler(this.PrintSelectedButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.dailyButton);
            this.groupBox2.Controls.Add(this.weeklyButton);
            this.groupBox2.Location = new System.Drawing.Point(13, 588);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(250, 60);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Calculations";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.printAllButton);
            this.groupBox3.Controls.Add(this.printSelectedButton);
            this.groupBox3.Location = new System.Drawing.Point(269, 588);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(290, 60);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Time Cards";
            // 
            // clinicComboBox
            // 
            this.clinicComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clinicComboBox.Location = new System.Drawing.Point(541, 56);
            this.clinicComboBox.MaxDropDownItems = 30;
            this.clinicComboBox.Name = "clinicComboBox";
            this.clinicComboBox.Size = new System.Drawing.Size(160, 23);
            this.clinicComboBox.TabIndex = 2;
            this.clinicComboBox.Visible = false;
            this.clinicComboBox.SelectionChangeCommitted += new System.EventHandler(this.ClinicComboBox_SelectionChangeCommitted);
            // 
            // clinicLabel
            // 
            this.clinicLabel.AutoSize = true;
            this.clinicLabel.Location = new System.Drawing.Point(538, 38);
            this.clinicLabel.Name = "clinicLabel";
            this.clinicLabel.Size = new System.Drawing.Size(37, 15);
            this.clinicLabel.TabIndex = 1;
            this.clinicLabel.Text = "Clinic";
            this.clinicLabel.Visible = false;
            // 
            // printGridButton
            // 
            this.printGridButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.printGridButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.printGridButton.Location = new System.Drawing.Point(1021, 19);
            this.printGridButton.Name = "printGridButton";
            this.printGridButton.Size = new System.Drawing.Size(110, 30);
            this.printGridButton.TabIndex = 7;
            this.printGridButton.Text = "Print Grid";
            this.printGridButton.Click += new System.EventHandler(this.PrintGridButton_Click);
            // 
            // exportGridButton
            // 
            this.exportGridButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportGridButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.exportGridButton.Location = new System.Drawing.Point(905, 55);
            this.exportGridButton.Name = "exportGridButton";
            this.exportGridButton.Size = new System.Drawing.Size(110, 30);
            this.exportGridButton.TabIndex = 6;
            this.exportGridButton.Text = "Export Grid";
            this.exportGridButton.Click += new System.EventHandler(this.ExportGridButton_Click);
            // 
            // exportAdpButton
            // 
            this.exportAdpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportAdpButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.exportAdpButton.Location = new System.Drawing.Point(905, 19);
            this.exportAdpButton.Name = "exportAdpButton";
            this.exportAdpButton.Size = new System.Drawing.Size(110, 30);
            this.exportAdpButton.TabIndex = 5;
            this.exportAdpButton.Text = "Export ADP";
            this.exportAdpButton.Click += new System.EventHandler(this.ExportAdpButton_Click);
            // 
            // setupButton
            // 
            this.setupButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setupButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.setupButton.Location = new System.Drawing.Point(1021, 55);
            this.setupButton.Name = "setupButton";
            this.setupButton.Size = new System.Drawing.Size(110, 30);
            this.setupButton.TabIndex = 8;
            this.setupButton.Text = "Setup";
            this.setupButton.Click += new System.EventHandler(this.SetupButton_Click);
            // 
            // FormTimeCardManage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(1144, 661);
            this.Controls.Add(this.setupButton);
            this.Controls.Add(this.exportAdpButton);
            this.Controls.Add(this.exportGridButton);
            this.Controls.Add(this.printGridButton);
            this.Controls.Add(this.clinicComboBox);
            this.Controls.Add(this.clinicLabel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.clearManualButton);
            this.Controls.Add(this.clearAutoButton);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.payPeriodGroupBox);
            this.Controls.Add(this.closeButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormTimeCardManage";
            this.Text = "Manage Time Cards";
            this.Load += new System.EventHandler(this.FormTimeCardManage_Load);
            this.Shown += new System.EventHandler(this.FormTimeCardManage_Shown);
            this.payPeriodGroupBox.ResumeLayout(false);
            this.payPeriodGroupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.GroupBox payPeriodGroupBox;
        private System.Windows.Forms.TextBox datePaycheckTextBox;
        private System.Windows.Forms.TextBox dateEndTextBox;
        private System.Windows.Forms.TextBox dateStartTextBox;
        private System.Windows.Forms.Button rightButton;
        private System.Windows.Forms.Button leftButton;
        private System.Windows.Forms.Label datePaycheckLabel;
        private System.Windows.Forms.Label dateStartLabel;
        private System.Windows.Forms.Label dateEndLabel;
        private UI.ODGrid grid;
        private System.Windows.Forms.Button printAllButton;
        private System.Windows.Forms.Button dailyButton;
        private System.Windows.Forms.Button weeklyButton;
        private System.Windows.Forms.Button clearAutoButton;
        private System.Windows.Forms.Button clearManualButton;
        private System.Windows.Forms.Button printSelectedButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox clinicComboBox;
        private System.Windows.Forms.Label clinicLabel;
        private System.Windows.Forms.Button printGridButton;
        private System.Windows.Forms.Button exportGridButton;
        private System.Windows.Forms.Button exportAdpButton;
        private System.Windows.Forms.Button setupButton;
    }
}
