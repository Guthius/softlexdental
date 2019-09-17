namespace OpenDental
{
    partial class FormAllergySetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAllergySetup));
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.showHiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.allergiesGrid = new OpenDental.UI.ODGrid();
            this.addButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(311, 418);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "&Close";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(311, 382);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 0;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Visible = false;
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // showHiddenCheckBox
            // 
            this.showHiddenCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showHiddenCheckBox.AutoSize = true;
            this.showHiddenCheckBox.Location = new System.Drawing.Point(311, 72);
            this.showHiddenCheckBox.Margin = new System.Windows.Forms.Padding(3, 20, 3, 20);
            this.showHiddenCheckBox.Name = "showHiddenCheckBox";
            this.showHiddenCheckBox.Size = new System.Drawing.Size(97, 19);
            this.showHiddenCheckBox.TabIndex = 4;
            this.showHiddenCheckBox.TabStop = false;
            this.showHiddenCheckBox.Text = "Show Hidden";
            this.showHiddenCheckBox.UseVisualStyleBackColor = true;
            this.showHiddenCheckBox.CheckedChanged += new System.EventHandler(this.ShowHiddenCheckBox_CheckedChanged);
            // 
            // allergiesGrid
            // 
            this.allergiesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.allergiesGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.allergiesGrid.EditableEnterMovesDown = false;
            this.allergiesGrid.HasAddButton = false;
            this.allergiesGrid.HasDropDowns = false;
            this.allergiesGrid.HasMultilineHeaders = false;
            this.allergiesGrid.HScrollVisible = false;
            this.allergiesGrid.Location = new System.Drawing.Point(13, 19);
            this.allergiesGrid.Name = "allergiesGrid";
            this.allergiesGrid.ScrollValue = 0;
            this.allergiesGrid.Size = new System.Drawing.Size(292, 429);
            this.allergiesGrid.TabIndex = 2;
            this.allergiesGrid.Title = "Allergies";
            this.allergiesGrid.TitleVisible = true;
            this.allergiesGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.AllergiesGrid_CellDoubleClick);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addButton.Location = new System.Drawing.Point(311, 19);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(110, 30);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "&Add";
            this.addButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // FormAllergySetup
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(434, 461);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.showHiddenCheckBox);
            this.Controls.Add(this.allergiesGrid);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAllergySetup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Allergy Setup";
            this.Load += new System.EventHandler(this.FormAllergySetup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button cancelButton;
        private UI.ODGrid allergiesGrid;
        private System.Windows.Forms.CheckBox showHiddenCheckBox;
        private System.Windows.Forms.Button acceptButton;
    }
}