namespace OpenDental
{
    partial class FormApptTypes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApptTypes));
            this.addButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.typesGrid = new OpenDental.UI.ODGrid();
            this.downButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.promptCheckBox = new System.Windows.Forms.CheckBox();
            this.warnCheckBox = new System.Windows.Forms.CheckBox();
            this.acceptButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(421, 69);
            this.addButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 30);
            this.addButton.Name = "addButton";
            this.addButton.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.addButton.Size = new System.Drawing.Size(110, 30);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(421, 518);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Close";
            // 
            // typesGrid
            // 
            this.typesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.typesGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.typesGrid.EditableEnterMovesDown = false;
            this.typesGrid.HasAddButton = false;
            this.typesGrid.HasDropDowns = false;
            this.typesGrid.HasMultilineHeaders = false;
            this.typesGrid.HScrollVisible = false;
            this.typesGrid.Location = new System.Drawing.Point(13, 69);
            this.typesGrid.Name = "typesGrid";
            this.typesGrid.ScrollValue = 0;
            this.typesGrid.Size = new System.Drawing.Size(402, 479);
            this.typesGrid.TabIndex = 0;
            this.typesGrid.Title = "Appointment Types";
            this.typesGrid.TitleVisible = true;
            this.typesGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.TypesGrid_CellDoubleClick);
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.Image = global::OpenDental.Properties.Resources.IconArrowDown;
            this.downButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.downButton.Location = new System.Drawing.Point(421, 168);
            this.downButton.Name = "downButton";
            this.downButton.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.downButton.Size = new System.Drawing.Size(110, 30);
            this.downButton.TabIndex = 3;
            this.downButton.Text = "&Down";
            this.downButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.Image = global::OpenDental.Properties.Resources.IconArrowUp;
            this.upButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.upButton.Location = new System.Drawing.Point(421, 132);
            this.upButton.Name = "upButton";
            this.upButton.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.upButton.Size = new System.Drawing.Size(110, 30);
            this.upButton.TabIndex = 2;
            this.upButton.Text = "&Up";
            this.upButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // promptCheckBox
            // 
            this.promptCheckBox.AutoSize = true;
            this.promptCheckBox.Location = new System.Drawing.Point(13, 19);
            this.promptCheckBox.Name = "promptCheckBox";
            this.promptCheckBox.Size = new System.Drawing.Size(286, 19);
            this.promptCheckBox.TabIndex = 6;
            this.promptCheckBox.Text = "New appointments prompt for appointment type";
            this.promptCheckBox.UseVisualStyleBackColor = true;
            this.promptCheckBox.CheckedChanged += new System.EventHandler(this.PromptCheckBox_CheckedChanged);
            // 
            // warnCheckBox
            // 
            this.warnCheckBox.AutoSize = true;
            this.warnCheckBox.Location = new System.Drawing.Point(13, 44);
            this.warnCheckBox.Name = "warnCheckBox";
            this.warnCheckBox.Size = new System.Drawing.Size(377, 19);
            this.warnCheckBox.TabIndex = 7;
            this.warnCheckBox.Text = "Warn users before disassociating procedures from an appointment";
            this.warnCheckBox.UseVisualStyleBackColor = true;
            this.warnCheckBox.CheckedChanged += new System.EventHandler(this.WarnCheckBox_CheckedChanged);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(421, 482);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Visible = false;
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // FormApptTypes
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(544, 561);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.warnCheckBox);
            this.Controls.Add(this.promptCheckBox);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.typesGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormApptTypes";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Appointment Types";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormApptTypes_FormClosing);
            this.Load += new System.EventHandler(this.FormApptTypes_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addButton;
        private UI.ODGrid typesGrid;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.CheckBox promptCheckBox;
        private System.Windows.Forms.CheckBox warnCheckBox;
        private System.Windows.Forms.Button acceptButton;
    }
}