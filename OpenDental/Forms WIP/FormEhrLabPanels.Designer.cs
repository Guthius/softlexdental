namespace OpenDental
{
    partial class FormEhrLabPanels
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEhrLabPanels));
            this.cancelButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.panelsGrid = new OpenDental.UI.ODGrid();
            this.submitButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.showButton = new System.Windows.Forms.Button();
            this.hl7GroupBox = new System.Windows.Forms.GroupBox();
            this.hl7GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(461, 368);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(13, 19);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(110, 30);
            this.addButton.TabIndex = 0;
            this.addButton.Text = "Add Panel";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // panelsGrid
            // 
            this.panelsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelsGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.panelsGrid.EditableEnterMovesDown = false;
            this.panelsGrid.HasAddButton = false;
            this.panelsGrid.HasDropDowns = false;
            this.panelsGrid.HasMultilineHeaders = false;
            this.panelsGrid.HScrollVisible = false;
            this.panelsGrid.Location = new System.Drawing.Point(13, 55);
            this.panelsGrid.Name = "panelsGrid";
            this.panelsGrid.ScrollValue = 0;
            this.panelsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.Multiple;
            this.panelsGrid.Size = new System.Drawing.Size(558, 277);
            this.panelsGrid.TabIndex = 1;
            this.panelsGrid.Title = "Lab Panels";
            this.panelsGrid.TitleVisible = true;
            this.panelsGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.PanelsGrid_CellDoubleClick);
            // 
            // submitButton
            // 
            this.submitButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.submitButton.Location = new System.Drawing.Point(10, 22);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(110, 30);
            this.submitButton.TabIndex = 0;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(345, 368);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "OK";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // showButton
            // 
            this.showButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.showButton.Location = new System.Drawing.Point(126, 22);
            this.showButton.Name = "showButton";
            this.showButton.Size = new System.Drawing.Size(110, 30);
            this.showButton.TabIndex = 1;
            this.showButton.Text = "Show";
            this.showButton.UseVisualStyleBackColor = true;
            this.showButton.Click += new System.EventHandler(this.ShowButton_Click);
            // 
            // hl7GroupBox
            // 
            this.hl7GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hl7GroupBox.Controls.Add(this.showButton);
            this.hl7GroupBox.Controls.Add(this.submitButton);
            this.hl7GroupBox.Location = new System.Drawing.Point(13, 338);
            this.hl7GroupBox.Name = "hl7GroupBox";
            this.hl7GroupBox.Size = new System.Drawing.Size(250, 60);
            this.hl7GroupBox.TabIndex = 2;
            this.hl7GroupBox.TabStop = false;
            this.hl7GroupBox.Text = "HL7 Msg";
            // 
            // FormEhrLabPanels
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(584, 411);
            this.Controls.Add(this.hl7GroupBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.panelsGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEhrLabPanels";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Lab Panels";
            this.Load += new System.EventHandler(this.FormLabPanels_Load);
            this.hl7GroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenDental.UI.ODGrid panelsGrid;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button showButton;
        private System.Windows.Forms.GroupBox hl7GroupBox;
    }
}