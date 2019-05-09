namespace OpenDental
{
    partial class FormProcButtonQuickEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProcButtonQuickEdit));
            this.labelCheckBox = new System.Windows.Forms.CheckBox();
            this.procedureCodeTextBox = new System.Windows.Forms.TextBox();
            this.procedureCodeLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.pickProcedureButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.surfacesLabel = new System.Windows.Forms.Label();
            this.surfacesTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelCheckBox
            // 
            this.labelCheckBox.AutoSize = true;
            this.labelCheckBox.Location = new System.Drawing.Point(160, 106);
            this.labelCheckBox.Name = "labelCheckBox";
            this.labelCheckBox.Size = new System.Drawing.Size(109, 19);
            this.labelCheckBox.TabIndex = 7;
            this.labelCheckBox.Text = "Display as Label";
            this.labelCheckBox.UseVisualStyleBackColor = true;
            this.labelCheckBox.CheckedChanged += new System.EventHandler(this.LabelCheckBox_CheckedChanged);
            // 
            // procedureCodeTextBox
            // 
            this.procedureCodeTextBox.Location = new System.Drawing.Point(160, 48);
            this.procedureCodeTextBox.Name = "procedureCodeTextBox";
            this.procedureCodeTextBox.Size = new System.Drawing.Size(255, 23);
            this.procedureCodeTextBox.TabIndex = 3;
            // 
            // procedureCodeLabel
            // 
            this.procedureCodeLabel.AutoSize = true;
            this.procedureCodeLabel.Location = new System.Drawing.Point(62, 51);
            this.procedureCodeLabel.Name = "procedureCodeLabel";
            this.procedureCodeLabel.Size = new System.Drawing.Size(92, 15);
            this.procedureCodeLabel.TabIndex = 2;
            this.procedureCodeLabel.Text = "Procedure Code";
            this.procedureCodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(160, 19);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(255, 23);
            this.descriptionTextBox.TabIndex = 1;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(85, 22);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(69, 15);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Display Text";
            this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pickProcedureButton
            // 
            this.pickProcedureButton.Location = new System.Drawing.Point(421, 47);
            this.pickProcedureButton.Name = "pickProcedureButton";
            this.pickProcedureButton.Size = new System.Drawing.Size(30, 25);
            this.pickProcedureButton.TabIndex = 4;
            this.pickProcedureButton.Text = "...";
            this.pickProcedureButton.UseVisualStyleBackColor = true;
            this.pickProcedureButton.Click += new System.EventHandler(this.PickProcedureButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deleteButton.Location = new System.Drawing.Point(13, 158);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 8;
            this.deleteButton.Text = "Delete";
            this.deleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(225, 158);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 9;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(341, 158);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "&Cancel";
            // 
            // surfacesLabel
            // 
            this.surfacesLabel.AutoSize = true;
            this.surfacesLabel.Location = new System.Drawing.Point(103, 80);
            this.surfacesLabel.Name = "surfacesLabel";
            this.surfacesLabel.Size = new System.Drawing.Size(51, 15);
            this.surfacesLabel.TabIndex = 5;
            this.surfacesLabel.Text = "Surfaces";
            this.surfacesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // surfacesTextBox
            // 
            this.surfacesTextBox.Location = new System.Drawing.Point(160, 77);
            this.surfacesTextBox.Name = "surfacesTextBox";
            this.surfacesTextBox.Size = new System.Drawing.Size(255, 23);
            this.surfacesTextBox.TabIndex = 6;
            // 
            // FormProcButtonQuickEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(464, 201);
            this.Controls.Add(this.pickProcedureButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.procedureCodeTextBox);
            this.Controls.Add(this.procedureCodeLabel);
            this.Controls.Add(this.labelCheckBox);
            this.Controls.Add(this.surfacesTextBox);
            this.Controls.Add(this.surfacesLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProcButtonQuickEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Quick Procedure Button";
            this.Load += new System.EventHandler(this.FormProcButtonQuickEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox labelCheckBox;
        private System.Windows.Forms.TextBox procedureCodeTextBox;
        private System.Windows.Forms.Label procedureCodeLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button pickProcedureButton;
        private System.Windows.Forms.Label surfacesLabel;
        private System.Windows.Forms.TextBox surfacesTextBox;
    }
}