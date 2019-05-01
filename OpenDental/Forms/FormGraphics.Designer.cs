namespace OpenDental
{
    partial class FormGraphics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGraphics));
            this.hardwareAccelerationCheckBox = new System.Windows.Forms.CheckBox();
            this.doubleBufferingCheckBox = new System.Windows.Forms.CheckBox();
            this.formatsGroupBox = new System.Windows.Forms.GroupBox();
            this.formatInfoLabel = new System.Windows.Forms.Label();
            this.formatTextBox = new System.Windows.Forms.TextBox();
            this.formatLabel = new System.Windows.Forms.Label();
            this.formatsGrid = new OpenDental.UI.ODGrid();
            this.simpleGraphicsRadioButton = new System.Windows.Forms.RadioButton();
            this.openGLGraphicsRadioButton = new System.Windows.Forms.RadioButton();
            this.directXGraphicsRadioButton = new System.Windows.Forms.RadioButton();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.optionsGroupBox = new System.Windows.Forms.GroupBox();
            this.graphicsModeGroupBox = new System.Windows.Forms.GroupBox();
            this.formatsGroupBox.SuspendLayout();
            this.optionsGroupBox.SuspendLayout();
            this.graphicsModeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // hardwareAccelerationCheckBox
            // 
            this.hardwareAccelerationCheckBox.AutoSize = true;
            this.hardwareAccelerationCheckBox.Location = new System.Drawing.Point(23, 29);
            this.hardwareAccelerationCheckBox.Name = "hardwareAccelerationCheckBox";
            this.hardwareAccelerationCheckBox.Size = new System.Drawing.Size(180, 19);
            this.hardwareAccelerationCheckBox.TabIndex = 2;
            this.hardwareAccelerationCheckBox.Text = "Enable hardware acceleration\r\n";
            this.hardwareAccelerationCheckBox.UseVisualStyleBackColor = true;
            this.hardwareAccelerationCheckBox.Click += new System.EventHandler(this.hardwareAccelerationCheckBox_Click);
            // 
            // doubleBufferingCheckBox
            // 
            this.doubleBufferingCheckBox.AutoSize = true;
            this.doubleBufferingCheckBox.Location = new System.Drawing.Point(23, 55);
            this.doubleBufferingCheckBox.Name = "doubleBufferingCheckBox";
            this.doubleBufferingCheckBox.Size = new System.Drawing.Size(137, 19);
            this.doubleBufferingCheckBox.TabIndex = 4;
            this.doubleBufferingCheckBox.Text = "Use double buffering";
            this.doubleBufferingCheckBox.UseVisualStyleBackColor = true;
            this.doubleBufferingCheckBox.Click += new System.EventHandler(this.doubleBufferingCheckBox_Click);
            // 
            // formatsGroupBox
            // 
            this.formatsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formatsGroupBox.Controls.Add(this.formatInfoLabel);
            this.formatsGroupBox.Controls.Add(this.formatTextBox);
            this.formatsGroupBox.Controls.Add(this.formatLabel);
            this.formatsGroupBox.Controls.Add(this.formatsGrid);
            this.formatsGroupBox.Location = new System.Drawing.Point(13, 172);
            this.formatsGroupBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.formatsGroupBox.Name = "formatsGroupBox";
            this.formatsGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.formatsGroupBox.Size = new System.Drawing.Size(758, 290);
            this.formatsGroupBox.TabIndex = 5;
            this.formatsGroupBox.TabStop = false;
            this.formatsGroupBox.Text = "Graphics Formats";
            // 
            // formatInfoLabel
            // 
            this.formatInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.formatInfoLabel.AutoSize = true;
            this.formatInfoLabel.Location = new System.Drawing.Point(289, 32);
            this.formatInfoLabel.Name = "formatInfoLabel";
            this.formatInfoLabel.Size = new System.Drawing.Size(463, 15);
            this.formatInfoLabel.TabIndex = 15;
            this.formatInfoLabel.Text = "Formats are listed from most recommended on top to least recommended on bottom.";
            // 
            // formatTextBox
            // 
            this.formatTextBox.Location = new System.Drawing.Point(198, 29);
            this.formatTextBox.Name = "formatTextBox";
            this.formatTextBox.ReadOnly = true;
            this.formatTextBox.Size = new System.Drawing.Size(53, 23);
            this.formatTextBox.TabIndex = 14;
            // 
            // formatLabel
            // 
            this.formatLabel.AutoSize = true;
            this.formatLabel.Location = new System.Drawing.Point(6, 32);
            this.formatLabel.Name = "formatLabel";
            this.formatLabel.Size = new System.Drawing.Size(186, 15);
            this.formatLabel.TabIndex = 13;
            this.formatLabel.Text = "Currently selected format number";
            // 
            // formatsGrid
            // 
            this.formatsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formatsGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.formatsGrid.EditableEnterMovesDown = false;
            this.formatsGrid.HasAddButton = false;
            this.formatsGrid.HasDropDowns = false;
            this.formatsGrid.HasMultilineHeaders = false;
            this.formatsGrid.HScrollVisible = false;
            this.formatsGrid.Location = new System.Drawing.Point(6, 58);
            this.formatsGrid.Name = "formatsGrid";
            this.formatsGrid.ScrollValue = 0;
            this.formatsGrid.Size = new System.Drawing.Size(746, 226);
            this.formatsGrid.TabIndex = 8;
            this.formatsGrid.Title = "Available Graphics Formats";
            this.formatsGrid.TitleVisible = true;
            this.formatsGrid.CellClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.gridFormats_CellClick);
            // 
            // simpleGraphicsRadioButton
            // 
            this.simpleGraphicsRadioButton.AutoSize = true;
            this.simpleGraphicsRadioButton.Location = new System.Drawing.Point(23, 29);
            this.simpleGraphicsRadioButton.Name = "simpleGraphicsRadioButton";
            this.simpleGraphicsRadioButton.Size = new System.Drawing.Size(132, 19);
            this.simpleGraphicsRadioButton.TabIndex = 6;
            this.simpleGraphicsRadioButton.TabStop = true;
            this.simpleGraphicsRadioButton.Text = "Use Simple Graphics";
            this.simpleGraphicsRadioButton.UseVisualStyleBackColor = true;
            this.simpleGraphicsRadioButton.Click += new System.EventHandler(this.simpleGraphicsRadioButton_Click);
            // 
            // openGLGraphicsRadioButton
            // 
            this.openGLGraphicsRadioButton.AutoSize = true;
            this.openGLGraphicsRadioButton.Location = new System.Drawing.Point(23, 80);
            this.openGLGraphicsRadioButton.Name = "openGLGraphicsRadioButton";
            this.openGLGraphicsRadioButton.Size = new System.Drawing.Size(139, 19);
            this.openGLGraphicsRadioButton.TabIndex = 7;
            this.openGLGraphicsRadioButton.TabStop = true;
            this.openGLGraphicsRadioButton.Text = "Use OpenGL Graphics";
            this.openGLGraphicsRadioButton.UseVisualStyleBackColor = true;
            this.openGLGraphicsRadioButton.Click += new System.EventHandler(this.openGLGraphicsRadioButton_Click);
            // 
            // directXGraphicsRadioButton
            // 
            this.directXGraphicsRadioButton.AutoSize = true;
            this.directXGraphicsRadioButton.Location = new System.Drawing.Point(23, 54);
            this.directXGraphicsRadioButton.Name = "directXGraphicsRadioButton";
            this.directXGraphicsRadioButton.Size = new System.Drawing.Size(223, 19);
            this.directXGraphicsRadioButton.TabIndex = 8;
            this.directXGraphicsRadioButton.TabStop = true;
            this.directXGraphicsRadioButton.Text = "Use DirectX Graphics (recommended)";
            this.directXGraphicsRadioButton.UseVisualStyleBackColor = true;
            this.directXGraphicsRadioButton.Click += new System.EventHandler(this.directXGraphicsRadioButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(545, 468);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 1;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(661, 468);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // optionsGroupBox
            // 
            this.optionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsGroupBox.Controls.Add(this.hardwareAccelerationCheckBox);
            this.optionsGroupBox.Controls.Add(this.doubleBufferingCheckBox);
            this.optionsGroupBox.Location = new System.Drawing.Point(471, 19);
            this.optionsGroupBox.Name = "optionsGroupBox";
            this.optionsGroupBox.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.optionsGroupBox.Size = new System.Drawing.Size(300, 140);
            this.optionsGroupBox.TabIndex = 17;
            this.optionsGroupBox.TabStop = false;
            this.optionsGroupBox.Text = "Options";
            // 
            // graphicsModeGroupBox
            // 
            this.graphicsModeGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.graphicsModeGroupBox.Controls.Add(this.simpleGraphicsRadioButton);
            this.graphicsModeGroupBox.Controls.Add(this.openGLGraphicsRadioButton);
            this.graphicsModeGroupBox.Controls.Add(this.directXGraphicsRadioButton);
            this.graphicsModeGroupBox.Location = new System.Drawing.Point(13, 19);
            this.graphicsModeGroupBox.Name = "graphicsModeGroupBox";
            this.graphicsModeGroupBox.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.graphicsModeGroupBox.Size = new System.Drawing.Size(452, 140);
            this.graphicsModeGroupBox.TabIndex = 18;
            this.graphicsModeGroupBox.TabStop = false;
            this.graphicsModeGroupBox.Text = "Graphics Mode";
            // 
            // FormGraphics
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(784, 511);
            this.Controls.Add(this.graphicsModeGroupBox);
            this.Controls.Add(this.optionsGroupBox);
            this.Controls.Add(this.formatsGroupBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGraphics";
            this.ShowInTaskbar = false;
            this.Text = "Graphics Preferences";
            this.Load += new System.EventHandler(this.FormGraphics_Load);
            this.formatsGroupBox.ResumeLayout(false);
            this.formatsGroupBox.PerformLayout();
            this.optionsGroupBox.ResumeLayout(false);
            this.optionsGroupBox.PerformLayout();
            this.graphicsModeGroupBox.ResumeLayout(false);
            this.graphicsModeGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.CheckBox hardwareAccelerationCheckBox;
        private System.Windows.Forms.CheckBox doubleBufferingCheckBox;
        private System.Windows.Forms.GroupBox formatsGroupBox;
        private OpenDental.UI.ODGrid formatsGrid;
        private System.Windows.Forms.RadioButton simpleGraphicsRadioButton;
        private System.Windows.Forms.RadioButton openGLGraphicsRadioButton;
        private System.Windows.Forms.TextBox formatTextBox;
        private System.Windows.Forms.Label formatLabel;
        private System.Windows.Forms.RadioButton directXGraphicsRadioButton;
        private System.Windows.Forms.Label formatInfoLabel;
        private System.Windows.Forms.GroupBox optionsGroupBox;
        private System.Windows.Forms.GroupBox graphicsModeGroupBox;
    }
}