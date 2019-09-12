namespace OpenDental
{
    partial class FormTimeCardSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTimeCardSetup));
            this.useDecimalCheckBox = new System.Windows.Forms.CheckBox();
            this.optionsGroupBox = new System.Windows.Forms.GroupBox();
            this.showSecondsCheckBox = new System.Windows.Forms.CheckBox();
            this.adjOverBreaksCheckBox = new System.Windows.Forms.CheckBox();
            this.addRuleButton = new System.Windows.Forms.Button();
            this.rulesGrid = new OpenDental.UI.ODGrid();
            this.payPeriodsGrid = new OpenDental.UI.ODGrid();
            this.addButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.adpCompanyCodeLabel = new System.Windows.Forms.Label();
            this.adpCompanyCodeTextBox = new System.Windows.Forms.TextBox();
            this.generateButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.hideOlderCheckBox = new System.Windows.Forms.CheckBox();
            this.deleteRulesButton = new System.Windows.Forms.Button();
            this.optionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // useDecimalCheckBox
            // 
            this.useDecimalCheckBox.AutoSize = true;
            this.useDecimalCheckBox.Location = new System.Drawing.Point(6, 22);
            this.useDecimalCheckBox.Name = "useDecimalCheckBox";
            this.useDecimalCheckBox.Size = new System.Drawing.Size(262, 19);
            this.useDecimalCheckBox.TabIndex = 0;
            this.useDecimalCheckBox.Text = "Use decimal format rather than colon format";
            this.useDecimalCheckBox.UseVisualStyleBackColor = true;
            this.useDecimalCheckBox.Click += new System.EventHandler(this.UseDecimalCheckBox_Click);
            // 
            // optionsGroupBox
            // 
            this.optionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.optionsGroupBox.Controls.Add(this.showSecondsCheckBox);
            this.optionsGroupBox.Controls.Add(this.adjOverBreaksCheckBox);
            this.optionsGroupBox.Controls.Add(this.useDecimalCheckBox);
            this.optionsGroupBox.Location = new System.Drawing.Point(356, 548);
            this.optionsGroupBox.Name = "optionsGroupBox";
            this.optionsGroupBox.Size = new System.Drawing.Size(450, 100);
            this.optionsGroupBox.TabIndex = 10;
            this.optionsGroupBox.TabStop = false;
            this.optionsGroupBox.Text = "Options";
            // 
            // showSecondsCheckBox
            // 
            this.showSecondsCheckBox.AutoSize = true;
            this.showSecondsCheckBox.Location = new System.Drawing.Point(6, 72);
            this.showSecondsCheckBox.Name = "showSecondsCheckBox";
            this.showSecondsCheckBox.Size = new System.Drawing.Size(297, 19);
            this.showSecondsCheckBox.TabIndex = 2;
            this.showSecondsCheckBox.Text = "Use seconds on time card when using colon format";
            this.showSecondsCheckBox.UseVisualStyleBackColor = true;
            this.showSecondsCheckBox.Click += new System.EventHandler(this.ShowSecondsCheckBox_Click);
            // 
            // adjOverBreaksCheckBox
            // 
            this.adjOverBreaksCheckBox.AutoSize = true;
            this.adjOverBreaksCheckBox.Location = new System.Drawing.Point(6, 47);
            this.adjOverBreaksCheckBox.Name = "adjOverBreaksCheckBox";
            this.adjOverBreaksCheckBox.Size = new System.Drawing.Size(359, 19);
            this.adjOverBreaksCheckBox.TabIndex = 1;
            this.adjOverBreaksCheckBox.Text = "Calc. Daily button makes adjustments if breaks over 30 minutes";
            this.adjOverBreaksCheckBox.UseVisualStyleBackColor = true;
            this.adjOverBreaksCheckBox.Click += new System.EventHandler(this.AdjOverBreaksCheckBox_Click);
            // 
            // addRuleButton
            // 
            this.addRuleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addRuleButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addRuleButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addRuleButton.Location = new System.Drawing.Point(356, 512);
            this.addRuleButton.Name = "addRuleButton";
            this.addRuleButton.Size = new System.Drawing.Size(110, 30);
            this.addRuleButton.TabIndex = 6;
            this.addRuleButton.Text = "Add";
            this.addRuleButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addRuleButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.addRuleButton.Click += new System.EventHandler(this.AddRuleButton_Click);
            // 
            // rulesGrid
            // 
            this.rulesGrid.AllowSortingByColumn = true;
            this.rulesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rulesGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.rulesGrid.EditableEnterMovesDown = false;
            this.rulesGrid.HasAddButton = false;
            this.rulesGrid.HasDropDowns = false;
            this.rulesGrid.HasMultilineHeaders = false;
            this.rulesGrid.HScrollVisible = false;
            this.rulesGrid.Location = new System.Drawing.Point(356, 19);
            this.rulesGrid.Name = "rulesGrid";
            this.rulesGrid.ScrollValue = 0;
            this.rulesGrid.SelectionMode = OpenDental.UI.GridSelectionMode.Multiple;
            this.rulesGrid.Size = new System.Drawing.Size(685, 487);
            this.rulesGrid.TabIndex = 5;
            this.rulesGrid.Title = "Rules";
            this.rulesGrid.TitleVisible = true;
            this.rulesGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.RulesGrid_CellDoubleClick);
            // 
            // payPeriodsGrid
            // 
            this.payPeriodsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.payPeriodsGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.payPeriodsGrid.EditableEnterMovesDown = false;
            this.payPeriodsGrid.HasAddButton = false;
            this.payPeriodsGrid.HasDropDowns = false;
            this.payPeriodsGrid.HasMultilineHeaders = false;
            this.payPeriodsGrid.HScrollVisible = false;
            this.payPeriodsGrid.Location = new System.Drawing.Point(13, 44);
            this.payPeriodsGrid.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.payPeriodsGrid.Name = "payPeriodsGrid";
            this.payPeriodsGrid.ScrollValue = 0;
            this.payPeriodsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.Multiple;
            this.payPeriodsGrid.Size = new System.Drawing.Size(320, 462);
            this.payPeriodsGrid.TabIndex = 1;
            this.payPeriodsGrid.Title = "Pay Periods";
            this.payPeriodsGrid.TitleVisible = true;
            this.payPeriodsGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.PayPeriodsGrid_CellDoubleClick);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = ((System.Drawing.Image)(resources.GetObject("addButton.Image")));
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addButton.Location = new System.Drawing.Point(13, 512);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(150, 30);
            this.addButton.TabIndex = 2;
            this.addButton.Text = "&Add One";
            this.addButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(931, 618);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(110, 30);
            this.closeButton.TabIndex = 11;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // adpCompanyCodeLabel
            // 
            this.adpCompanyCodeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.adpCompanyCodeLabel.AutoSize = true;
            this.adpCompanyCodeLabel.Location = new System.Drawing.Point(61, 626);
            this.adpCompanyCodeLabel.Name = "adpCompanyCodeLabel";
            this.adpCompanyCodeLabel.Size = new System.Drawing.Size(116, 15);
            this.adpCompanyCodeLabel.TabIndex = 8;
            this.adpCompanyCodeLabel.Text = "ADP Company Code";
            this.adpCompanyCodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // adpCompanyCodeTextBox
            // 
            this.adpCompanyCodeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.adpCompanyCodeTextBox.Location = new System.Drawing.Point(183, 623);
            this.adpCompanyCodeTextBox.Name = "adpCompanyCodeTextBox";
            this.adpCompanyCodeTextBox.Size = new System.Drawing.Size(97, 23);
            this.adpCompanyCodeTextBox.TabIndex = 9;
            // 
            // generateButton
            // 
            this.generateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.generateButton.Image = ((System.Drawing.Image)(resources.GetObject("generateButton.Image")));
            this.generateButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.generateButton.Location = new System.Drawing.Point(13, 548);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(150, 30);
            this.generateButton.TabIndex = 3;
            this.generateButton.Text = "Generate Many";
            this.generateButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.generateButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteButton.Image")));
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deleteButton.Location = new System.Drawing.Point(183, 512);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(150, 30);
            this.deleteButton.TabIndex = 4;
            this.deleteButton.Text = "Delete Selected";
            this.deleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // hideOlderCheckBox
            // 
            this.hideOlderCheckBox.AutoSize = true;
            this.hideOlderCheckBox.Checked = true;
            this.hideOlderCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hideOlderCheckBox.Location = new System.Drawing.Point(13, 19);
            this.hideOlderCheckBox.Name = "hideOlderCheckBox";
            this.hideOlderCheckBox.Size = new System.Drawing.Size(225, 19);
            this.hideOlderCheckBox.TabIndex = 0;
            this.hideOlderCheckBox.Text = "Hide pay periods older than 6 months";
            this.hideOlderCheckBox.UseVisualStyleBackColor = true;
            this.hideOlderCheckBox.CheckedChanged += new System.EventHandler(this.HideOlderCheckBox_CheckedChanged);
            // 
            // deleteRulesButton
            // 
            this.deleteRulesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteRulesButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteRulesButton.Image")));
            this.deleteRulesButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deleteRulesButton.Location = new System.Drawing.Point(891, 512);
            this.deleteRulesButton.Name = "deleteRulesButton";
            this.deleteRulesButton.Size = new System.Drawing.Size(150, 30);
            this.deleteRulesButton.TabIndex = 7;
            this.deleteRulesButton.Text = "Delete Selected";
            this.deleteRulesButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteRulesButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteRulesButton.Click += new System.EventHandler(this.DeleteRulesButton_Click);
            // 
            // FormTimeCardSetup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1054, 661);
            this.Controls.Add(this.deleteRulesButton);
            this.Controls.Add(this.hideOlderCheckBox);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.adpCompanyCodeLabel);
            this.Controls.Add(this.adpCompanyCodeTextBox);
            this.Controls.Add(this.addRuleButton);
            this.Controls.Add(this.optionsGroupBox);
            this.Controls.Add(this.rulesGrid);
            this.Controls.Add(this.payPeriodsGrid);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.closeButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(910, 500);
            this.Name = "FormTimeCardSetup";
            this.ShowInTaskbar = false;
            this.Text = "Time Card Setup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPayPeriods_FormClosing);
            this.Load += new System.EventHandler(this.FormPayPeriods_Load);
            this.optionsGroupBox.ResumeLayout(false);
            this.optionsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button closeButton;
        private UI.ODGrid payPeriodsGrid;
        private System.Windows.Forms.CheckBox useDecimalCheckBox;
        private UI.ODGrid rulesGrid;
        private System.Windows.Forms.GroupBox optionsGroupBox;
        private System.Windows.Forms.Button addRuleButton;
        private System.Windows.Forms.CheckBox adjOverBreaksCheckBox;
        private System.Windows.Forms.Label adpCompanyCodeLabel;
        private System.Windows.Forms.TextBox adpCompanyCodeTextBox;
        private System.Windows.Forms.CheckBox showSecondsCheckBox;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.CheckBox hideOlderCheckBox;
        private System.Windows.Forms.Button deleteRulesButton;
    }
}
