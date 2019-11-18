namespace OpenDental
{
    partial class FormApptLists
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApptLists));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.asapButton = new System.Windows.Forms.Button();
            this.unscheduledButton = new System.Windows.Forms.Button();
            this.plannedTrackerButton = new System.Windows.Forms.Button();
            this.confirmationsButton = new System.Windows.Forms.Button();
            this.recallsButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.radiologyButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.insuranceVerifyButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(139, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(532, 60);
            this.label1.TabIndex = 2;
            this.label1.Text = "A list of due dates for patients who have previously been in for an exam or clean" +
    "ing";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(139, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(532, 60);
            this.label2.TabIndex = 4;
            this.label2.Text = "A list of scheduled appointments. These patients need to be reminded about their " +
    "appointments.";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(139, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(532, 60);
            this.label3.TabIndex = 6;
            this.label3.Text = "Planned appointments are created in the Chart module. Every patient should have o" +
    "ne or be marked done. This list is work that has been planned but never schedule" +
    "d.";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(139, 249);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(532, 60);
            this.label5.TabIndex = 8;
            this.label5.Text = "A short list of appointments that need to be followed up on. Keep this list short" +
    " by deleting any that don\'t get scheduled quickly. You would then track them usi" +
    "ng the Planned Appointment Tracker";
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(13, 16);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(658, 50);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "These lists may be used for calling patients, sending postcards, running reports," +
    " etc..  Make sure to make good Comm Log entries for everything.";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(139, 309);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(532, 60);
            this.label4.TabIndex = 10;
            this.label4.Text = "The short call list. A list of appointments and recalls where the patient is avai" +
    "lable on short notice. To show this list, the \"ASAP\" checkbox must be checked on" +
    " the appointment or the recall";
            // 
            // asapButton
            // 
            this.asapButton.Location = new System.Drawing.Point(13, 309);
            this.asapButton.Name = "asapButton";
            this.asapButton.Size = new System.Drawing.Size(120, 40);
            this.asapButton.TabIndex = 9;
            this.asapButton.Text = "ASAP";
            this.asapButton.Click += new System.EventHandler(this.AsapButton_Click);
            // 
            // unscheduledButton
            // 
            this.unscheduledButton.Location = new System.Drawing.Point(13, 249);
            this.unscheduledButton.Name = "unscheduledButton";
            this.unscheduledButton.Size = new System.Drawing.Size(120, 40);
            this.unscheduledButton.TabIndex = 7;
            this.unscheduledButton.Text = "Unscheduled";
            this.unscheduledButton.Click += new System.EventHandler(this.UnscheduledButton_Click);
            // 
            // plannedTrackerButton
            // 
            this.plannedTrackerButton.Location = new System.Drawing.Point(13, 189);
            this.plannedTrackerButton.Name = "plannedTrackerButton";
            this.plannedTrackerButton.Size = new System.Drawing.Size(120, 40);
            this.plannedTrackerButton.TabIndex = 5;
            this.plannedTrackerButton.Text = "Planned Tracker";
            this.plannedTrackerButton.Click += new System.EventHandler(this.PlannedTrackerButton_Click);
            // 
            // confirmationsButton
            // 
            this.confirmationsButton.Location = new System.Drawing.Point(13, 129);
            this.confirmationsButton.Name = "confirmationsButton";
            this.confirmationsButton.Size = new System.Drawing.Size(120, 40);
            this.confirmationsButton.TabIndex = 3;
            this.confirmationsButton.Text = "Confirmations";
            this.confirmationsButton.Click += new System.EventHandler(this.ConfirmationsButton_Click);
            // 
            // recallsButton
            // 
            this.recallsButton.Location = new System.Drawing.Point(13, 69);
            this.recallsButton.Name = "recallsButton";
            this.recallsButton.Size = new System.Drawing.Size(120, 40);
            this.recallsButton.TabIndex = 1;
            this.recallsButton.Text = "Recall";
            this.recallsButton.Click += new System.EventHandler(this.RecallsButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(561, 518);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 15;
            this.cancelButton.Text = "&Cancel";
            // 
            // radiologyButton
            // 
            this.radiologyButton.Location = new System.Drawing.Point(13, 369);
            this.radiologyButton.Name = "radiologyButton";
            this.radiologyButton.Size = new System.Drawing.Size(120, 40);
            this.radiologyButton.TabIndex = 11;
            this.radiologyButton.Text = "Radiology";
            this.radiologyButton.Click += new System.EventHandler(this.RadiologyButton_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(139, 369);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(532, 60);
            this.label7.TabIndex = 12;
            this.label7.Text = "A list of radiology orders associated to appointments that have not been marked a" +
    "s CPOE. This list allows providers to quickly approve radiology orders which wil" +
    "l help meet EHR measures.";
            // 
            // insuranceVerifyButton
            // 
            this.insuranceVerifyButton.Location = new System.Drawing.Point(13, 429);
            this.insuranceVerifyButton.Name = "insuranceVerifyButton";
            this.insuranceVerifyButton.Size = new System.Drawing.Size(120, 40);
            this.insuranceVerifyButton.TabIndex = 13;
            this.insuranceVerifyButton.Text = "Ins Verify";
            this.insuranceVerifyButton.Click += new System.EventHandler(this.InsuranceVerifyButton_Click);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.Location = new System.Drawing.Point(139, 429);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(532, 60);
            this.label8.TabIndex = 14;
            this.label8.Text = "A list of insurance verifications for upcoming appointments.";
            // 
            // FormApptLists
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(684, 561);
            this.Controls.Add(this.insuranceVerifyButton);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.radiologyButton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.asapButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.unscheduledButton);
            this.Controls.Add(this.plannedTrackerButton);
            this.Controls.Add(this.confirmationsButton);
            this.Controls.Add(this.recallsButton);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormApptLists";
            this.ShowInTaskbar = false;
            this.Text = "Appointment Lists";
            this.ResumeLayout(false);

        }
        #endregion


        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button recallsButton;
        private System.Windows.Forms.Button confirmationsButton;
        private System.Windows.Forms.Button plannedTrackerButton;
        private System.Windows.Forms.Button unscheduledButton;
        private System.Windows.Forms.Button asapButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button radiologyButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button insuranceVerifyButton;
        private System.Windows.Forms.Label label8;
    }
}
