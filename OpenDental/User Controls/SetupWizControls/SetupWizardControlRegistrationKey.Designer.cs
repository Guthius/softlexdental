namespace OpenDental.User_Controls.SetupWizard
{
    partial class SetupWizardControlRegistrationKey
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupProcTools = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.procedureCodesButton = new System.Windows.Forms.Button();
            this.advancedButton = new System.Windows.Forms.Button();
            this.advancedLabel = new System.Windows.Forms.Label();
            this.changeButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textRegKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupProcTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupProcTools
            // 
            this.groupProcTools.Controls.Add(this.label1);
            this.groupProcTools.Controls.Add(this.procedureCodesButton);
            this.groupProcTools.Location = new System.Drawing.Point(242, 221);
            this.groupProcTools.Name = "groupProcTools";
            this.groupProcTools.Size = new System.Drawing.Size(450, 120);
            this.groupProcTools.TabIndex = 4;
            this.groupProcTools.TabStop = false;
            this.groupProcTools.Text = "Procedure Code Tools";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(419, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "After a valid registration key has been entered, you should run the procedure cod" +
    "e tools to add in the correct procedure codes and remove any trial version \"T\" c" +
    "odes.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // procedureCodesButton
            // 
            this.procedureCodesButton.Location = new System.Drawing.Point(294, 84);
            this.procedureCodesButton.Name = "procedureCodesButton";
            this.procedureCodesButton.Size = new System.Drawing.Size(150, 30);
            this.procedureCodesButton.TabIndex = 1;
            this.procedureCodesButton.Text = "Procedure Code Tools";
            this.procedureCodesButton.Click += new System.EventHandler(this.ProcedureCodesButton_Click);
            // 
            // advancedButton
            // 
            this.advancedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.advancedButton.Image = global::OpenDental.Properties.Resources.IconCog;
            this.advancedButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.advancedButton.Location = new System.Drawing.Point(777, 457);
            this.advancedButton.Name = "advancedButton";
            this.advancedButton.Size = new System.Drawing.Size(110, 30);
            this.advancedButton.TabIndex = 6;
            this.advancedButton.Text = "Advanced";
            this.advancedButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.advancedButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.advancedButton.Click += new System.EventHandler(this.AdvancedButton_Click);
            // 
            // advancedLabel
            // 
            this.advancedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.advancedLabel.Location = new System.Drawing.Point(486, 456);
            this.advancedLabel.Name = "advancedLabel";
            this.advancedLabel.Size = new System.Drawing.Size(285, 31);
            this.advancedLabel.TabIndex = 5;
            this.advancedLabel.Text = "Further changes can be made by going to \r\nHelp -> Update -> Setup or clicking \'Ad" +
    "vanced\'.\r\n";
            this.advancedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // changeButton
            // 
            this.changeButton.Location = new System.Drawing.Point(582, 158);
            this.changeButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 30);
            this.changeButton.Name = "changeButton";
            this.changeButton.Size = new System.Drawing.Size(110, 30);
            this.changeButton.TabIndex = 3;
            this.changeButton.Text = "Change";
            this.changeButton.Click += new System.EventHandler(this.ChangeButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(466, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(226, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "Valid for one office ONLY.  This is tracked.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // textRegKey
            // 
            this.textRegKey.Location = new System.Drawing.Point(242, 129);
            this.textRegKey.Name = "textRegKey";
            this.textRegKey.ReadOnly = true;
            this.textRegKey.Size = new System.Drawing.Size(450, 23);
            this.textRegKey.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(239, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Registration Key";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // SetupWizardControlRegistrationKey
            // 
            this.Controls.Add(this.changeButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textRegKey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.advancedButton);
            this.Controls.Add(this.advancedLabel);
            this.Controls.Add(this.groupProcTools);
            this.Name = "SetupWizardControlRegistrationKey";
            this.Size = new System.Drawing.Size(930, 530);
            this.Load += new System.EventHandler(this.SetupWizardControlRegistrationKey_Load);
            this.groupProcTools.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button procedureCodesButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupProcTools;
        private System.Windows.Forms.Button advancedButton;
        private System.Windows.Forms.Label advancedLabel;
        private System.Windows.Forms.Button changeButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textRegKey;
        private System.Windows.Forms.Label label2;
    }
}