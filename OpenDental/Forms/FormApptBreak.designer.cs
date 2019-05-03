namespace OpenDental
{
    partial class FormApptBreak
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApptBreak));
            this.cancelButton = new System.Windows.Forms.Button();
            this.typeGroupBox = new System.Windows.Forms.GroupBox();
            this.radioMissed = new System.Windows.Forms.RadioButton();
            this.radioCancelled = new System.Windows.Forms.RadioButton();
            this.unscheduledListButton = new System.Windows.Forms.Button();
            this.pinboardButton = new System.Windows.Forms.Button();
            this.appointmentBookButton = new System.Windows.Forms.Button();
            this.typeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(131, 218);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            // 
            // typeGroupBox
            // 
            this.typeGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.typeGroupBox.Controls.Add(this.radioMissed);
            this.typeGroupBox.Controls.Add(this.radioCancelled);
            this.typeGroupBox.Location = new System.Drawing.Point(13, 19);
            this.typeGroupBox.Name = "typeGroupBox";
            this.typeGroupBox.Padding = new System.Windows.Forms.Padding(5);
            this.typeGroupBox.Size = new System.Drawing.Size(228, 60);
            this.typeGroupBox.TabIndex = 0;
            this.typeGroupBox.TabStop = false;
            this.typeGroupBox.Text = "Broken Procedure Type";
            // 
            // radioMissed
            // 
            this.radioMissed.AutoSize = true;
            this.radioMissed.Location = new System.Drawing.Point(8, 24);
            this.radioMissed.Name = "radioMissed";
            this.radioMissed.Size = new System.Drawing.Size(62, 19);
            this.radioMissed.TabIndex = 0;
            this.radioMissed.TabStop = true;
            this.radioMissed.Text = "Missed";
            this.radioMissed.UseVisualStyleBackColor = true;
            // 
            // radioCancelled
            // 
            this.radioCancelled.AutoSize = true;
            this.radioCancelled.Location = new System.Drawing.Point(76, 24);
            this.radioCancelled.Name = "radioCancelled";
            this.radioCancelled.Size = new System.Drawing.Size(77, 19);
            this.radioCancelled.TabIndex = 1;
            this.radioCancelled.TabStop = true;
            this.radioCancelled.Text = "Cancelled";
            this.radioCancelled.UseVisualStyleBackColor = true;
            // 
            // unscheduledListButton
            // 
            this.unscheduledListButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unscheduledListButton.Location = new System.Drawing.Point(13, 85);
            this.unscheduledListButton.Name = "unscheduledListButton";
            this.unscheduledListButton.Size = new System.Drawing.Size(228, 30);
            this.unscheduledListButton.TabIndex = 1;
            this.unscheduledListButton.Text = "Send to Unscheduled List";
            this.unscheduledListButton.UseVisualStyleBackColor = true;
            this.unscheduledListButton.Click += new System.EventHandler(this.unscheduledListButton_Click);
            // 
            // pinboardButton
            // 
            this.pinboardButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pinboardButton.Location = new System.Drawing.Point(13, 121);
            this.pinboardButton.Name = "pinboardButton";
            this.pinboardButton.Size = new System.Drawing.Size(228, 30);
            this.pinboardButton.TabIndex = 2;
            this.pinboardButton.Text = "Copy to Pinboard";
            this.pinboardButton.UseVisualStyleBackColor = true;
            this.pinboardButton.Click += new System.EventHandler(this.pinboardButton_Click);
            // 
            // appointmentBookButton
            // 
            this.appointmentBookButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.appointmentBookButton.Location = new System.Drawing.Point(13, 157);
            this.appointmentBookButton.Name = "appointmentBookButton";
            this.appointmentBookButton.Size = new System.Drawing.Size(228, 30);
            this.appointmentBookButton.TabIndex = 3;
            this.appointmentBookButton.Text = "Leave on Appt Book";
            this.appointmentBookButton.UseVisualStyleBackColor = true;
            this.appointmentBookButton.Click += new System.EventHandler(this.appointmentBookButton_Click);
            // 
            // FormApptBreak
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(254, 261);
            this.ControlBox = false;
            this.Controls.Add(this.appointmentBookButton);
            this.Controls.Add(this.pinboardButton);
            this.Controls.Add(this.unscheduledListButton);
            this.Controls.Add(this.typeGroupBox);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormApptBreak";
            this.ShowInTaskbar = false;
            this.Text = "Broken Appointment";
            this.Load += new System.EventHandler(this.FormApptBreak_Load);
            this.typeGroupBox.ResumeLayout(false);
            this.typeGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox typeGroupBox;
        private System.Windows.Forms.RadioButton radioCancelled;
        private System.Windows.Forms.RadioButton radioMissed;
        private System.Windows.Forms.Button unscheduledListButton;
        private System.Windows.Forms.Button pinboardButton;
        private System.Windows.Forms.Button appointmentBookButton;
    }
}