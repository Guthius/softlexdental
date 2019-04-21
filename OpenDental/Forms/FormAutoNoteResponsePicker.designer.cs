namespace OpenDental
{
    partial class FormAutoNoteResponsePicker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAutoNoteResponsePicker));
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.responseLabel = new System.Windows.Forms.Label();
            this.responseTextBox = new System.Windows.Forms.TextBox();
            this.autoNotesGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(341, 482);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(341, 518);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // responseLabel
            // 
            this.responseLabel.AutoSize = true;
            this.responseLabel.Location = new System.Drawing.Point(33, 22);
            this.responseLabel.Name = "responseLabel";
            this.responseLabel.Size = new System.Drawing.Size(81, 15);
            this.responseLabel.TabIndex = 1;
            this.responseLabel.Text = "Response Text";
            this.responseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // responseTextBox
            // 
            this.responseTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.responseTextBox.Location = new System.Drawing.Point(120, 19);
            this.responseTextBox.Name = "responseTextBox";
            this.responseTextBox.Size = new System.Drawing.Size(215, 23);
            this.responseTextBox.TabIndex = 2;
            // 
            // autoNotesGrid
            // 
            this.autoNotesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoNotesGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.autoNotesGrid.EditableEnterMovesDown = false;
            this.autoNotesGrid.HasAddButton = false;
            this.autoNotesGrid.HasDropDowns = false;
            this.autoNotesGrid.HasMultilineHeaders = false;
            this.autoNotesGrid.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.autoNotesGrid.HeaderHeight = 15;
            this.autoNotesGrid.HScrollVisible = false;
            this.autoNotesGrid.Location = new System.Drawing.Point(15, 48);
            this.autoNotesGrid.Name = "autoNotesGrid";
            this.autoNotesGrid.ScrollValue = 0;
            this.autoNotesGrid.Size = new System.Drawing.Size(320, 500);
            this.autoNotesGrid.TabIndex = 3;
            this.autoNotesGrid.Title = "Available Auto Notes";
            this.autoNotesGrid.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.autoNotesGrid.TitleHeight = 18;
            this.autoNotesGrid.TranslationName = "FormAutoNoteEdit";
            // 
            // FormAutoNoteResponsePicker
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(464, 561);
            this.Controls.Add(this.autoNotesGrid);
            this.Controls.Add(this.responseLabel);
            this.Controls.Add(this.responseTextBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAutoNoteResponsePicker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto Note Response Picker";
            this.Load += new System.EventHandler(this.FormAutoNoteResponsePicker_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label responseLabel;
        private System.Windows.Forms.TextBox responseTextBox;
        private UI.ODGrid autoNotesGrid;
    }
}