namespace OpenDental
{
    partial class FormReportsMore
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReportsMore));
            this.labelArizonaPrimaryCare = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.labelDaily = new System.Windows.Forms.Label();
            this.labelProdInc = new System.Windows.Forms.Label();
            this.labelMonthly = new System.Windows.Forms.Label();
            this.labelLists = new System.Windows.Forms.Label();
            this.labelPublicHealth = new System.Windows.Forms.Label();
            this.ehrPatientExportButton = new System.Windows.Forms.Button();
            this.ehrPatientListButton = new System.Windows.Forms.Button();
            this.listArizonaPrimaryCare = new OpenDental.UI.ListBoxClickable();
            this.laserLabelsButton = new System.Windows.Forms.Button();
            this.listDaily = new OpenDental.UI.ListBoxClickable();
            this.listProdInc = new OpenDental.UI.ListBoxClickable();
            this.butPW = new System.Windows.Forms.Button();
            this.userQueryButton = new System.Windows.Forms.Button();
            this.listPublicHealth = new OpenDental.UI.ListBoxClickable();
            this.listLists = new OpenDental.UI.ListBoxClickable();
            this.listMonthly = new OpenDental.UI.ListBoxClickable();
            this.butClose = new System.Windows.Forms.Button();
            this.setupButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelArizonaPrimaryCare
            // 
            this.labelArizonaPrimaryCare.AutoSize = true;
            this.labelArizonaPrimaryCare.Location = new System.Drawing.Point(281, 410);
            this.labelArizonaPrimaryCare.Name = "labelArizonaPrimaryCare";
            this.labelArizonaPrimaryCare.Size = new System.Drawing.Size(118, 15);
            this.labelArizonaPrimaryCare.TabIndex = 20;
            this.labelArizonaPrimaryCare.Text = "Arizona Primary Care";
            this.labelArizonaPrimaryCare.Visible = false;
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(13, 600);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(539, 90);
            this.infoLabel.TabIndex = 17;
            this.infoLabel.Text = resources.GetString("infoLabel.Text");
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelDaily
            // 
            this.labelDaily.AutoSize = true;
            this.labelDaily.Location = new System.Drawing.Point(9, 200);
            this.labelDaily.Name = "labelDaily";
            this.labelDaily.Size = new System.Drawing.Size(33, 15);
            this.labelDaily.TabIndex = 15;
            this.labelDaily.Text = "Daily";
            this.labelDaily.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelProdInc
            // 
            this.labelProdInc.AutoSize = true;
            this.labelProdInc.Location = new System.Drawing.Point(9, 54);
            this.labelProdInc.Name = "labelProdInc";
            this.labelProdInc.Size = new System.Drawing.Size(132, 15);
            this.labelProdInc.TabIndex = 13;
            this.labelProdInc.Text = "Production and Income";
            this.labelProdInc.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelMonthly
            // 
            this.labelMonthly.AutoSize = true;
            this.labelMonthly.Location = new System.Drawing.Point(9, 349);
            this.labelMonthly.Name = "labelMonthly";
            this.labelMonthly.Size = new System.Drawing.Size(52, 15);
            this.labelMonthly.TabIndex = 6;
            this.labelMonthly.Text = "Monthly";
            this.labelMonthly.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelLists
            // 
            this.labelLists.AutoSize = true;
            this.labelLists.Location = new System.Drawing.Point(281, 54);
            this.labelLists.Name = "labelLists";
            this.labelLists.Size = new System.Drawing.Size(30, 15);
            this.labelLists.TabIndex = 4;
            this.labelLists.Text = "Lists";
            this.labelLists.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelPublicHealth
            // 
            this.labelPublicHealth.AutoSize = true;
            this.labelPublicHealth.Location = new System.Drawing.Point(281, 305);
            this.labelPublicHealth.Name = "labelPublicHealth";
            this.labelPublicHealth.Size = new System.Drawing.Size(78, 15);
            this.labelPublicHealth.TabIndex = 2;
            this.labelPublicHealth.Text = "Public Health";
            this.labelPublicHealth.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // ehrPatientExportButton
            // 
            this.ehrPatientExportButton.Location = new System.Drawing.Point(557, 111);
            this.ehrPatientExportButton.Name = "ehrPatientExportButton";
            this.ehrPatientExportButton.Size = new System.Drawing.Size(110, 30);
            this.ehrPatientExportButton.TabIndex = 24;
            this.ehrPatientExportButton.Text = "EHR Pat Export";
            this.ehrPatientExportButton.Visible = false;
            this.ehrPatientExportButton.Click += new System.EventHandler(this.EhrPatientExportButton_Click);
            // 
            // ehrPatientListButton
            // 
            this.ehrPatientListButton.Location = new System.Drawing.Point(557, 75);
            this.ehrPatientListButton.Name = "ehrPatientListButton";
            this.ehrPatientListButton.Size = new System.Drawing.Size(110, 30);
            this.ehrPatientListButton.TabIndex = 23;
            this.ehrPatientListButton.Text = "EHR Patient List";
            this.ehrPatientListButton.Visible = false;
            this.ehrPatientListButton.Click += new System.EventHandler(this.EhrPatientListButton_Click);
            // 
            // listArizonaPrimaryCare
            // 
            this.listArizonaPrimaryCare.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listArizonaPrimaryCare.FormattingEnabled = true;
            this.listArizonaPrimaryCare.ItemHeight = 15;
            this.listArizonaPrimaryCare.Location = new System.Drawing.Point(284, 424);
            this.listArizonaPrimaryCare.Name = "listArizonaPrimaryCare";
            this.listArizonaPrimaryCare.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listArizonaPrimaryCare.Size = new System.Drawing.Size(242, 49);
            this.listArizonaPrimaryCare.TabIndex = 19;
            this.listArizonaPrimaryCare.Visible = false;
            this.listArizonaPrimaryCare.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listArizonaPrimaryCare_MouseDown);
            // 
            // laserLabelsButton
            // 
            this.laserLabelsButton.Location = new System.Drawing.Point(245, 19);
            this.laserLabelsButton.Name = "laserLabelsButton";
            this.laserLabelsButton.Size = new System.Drawing.Size(110, 30);
            this.laserLabelsButton.TabIndex = 18;
            this.laserLabelsButton.Text = "Laser Labels";
            this.laserLabelsButton.UseVisualStyleBackColor = true;
            this.laserLabelsButton.Click += new System.EventHandler(this.LaserLabelsButton_Click);
            // 
            // listDaily
            // 
            this.listDaily.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listDaily.FormattingEnabled = true;
            this.listDaily.ItemHeight = 15;
            this.listDaily.Location = new System.Drawing.Point(12, 221);
            this.listDaily.Name = "listDaily";
            this.listDaily.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listDaily.Size = new System.Drawing.Size(242, 124);
            this.listDaily.TabIndex = 16;
            this.listDaily.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listDaily_MouseDown);
            // 
            // listProdInc
            // 
            this.listProdInc.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listProdInc.FormattingEnabled = true;
            this.listProdInc.ItemHeight = 15;
            this.listProdInc.Location = new System.Drawing.Point(12, 75);
            this.listProdInc.Name = "listProdInc";
            this.listProdInc.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listProdInc.Size = new System.Drawing.Size(242, 124);
            this.listProdInc.TabIndex = 14;
            this.listProdInc.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listProdInc_MouseDown);
            // 
            // butPW
            // 
            this.butPW.Location = new System.Drawing.Point(129, 19);
            this.butPW.Name = "butPW";
            this.butPW.Size = new System.Drawing.Size(110, 30);
            this.butPW.TabIndex = 12;
            this.butPW.Text = "PW Reports";
            this.butPW.Click += new System.EventHandler(this.butPW_Click);
            // 
            // userQueryButton
            // 
            this.userQueryButton.Location = new System.Drawing.Point(13, 19);
            this.userQueryButton.Name = "userQueryButton";
            this.userQueryButton.Size = new System.Drawing.Size(110, 30);
            this.userQueryButton.TabIndex = 11;
            this.userQueryButton.Text = "User Query";
            this.userQueryButton.Click += new System.EventHandler(this.butUserQuery_Click);
            // 
            // listPublicHealth
            // 
            this.listPublicHealth.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listPublicHealth.FormattingEnabled = true;
            this.listPublicHealth.ItemHeight = 15;
            this.listPublicHealth.Location = new System.Drawing.Point(284, 325);
            this.listPublicHealth.Name = "listPublicHealth";
            this.listPublicHealth.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listPublicHealth.Size = new System.Drawing.Size(242, 79);
            this.listPublicHealth.TabIndex = 10;
            this.listPublicHealth.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listPublicHealth_MouseDown);
            // 
            // listLists
            // 
            this.listLists.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listLists.FormattingEnabled = true;
            this.listLists.ItemHeight = 15;
            this.listLists.Location = new System.Drawing.Point(284, 75);
            this.listLists.Name = "listLists";
            this.listLists.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listLists.Size = new System.Drawing.Size(242, 229);
            this.listLists.TabIndex = 9;
            this.listLists.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listLists_MouseDown);
            // 
            // listMonthly
            // 
            this.listMonthly.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listMonthly.FormattingEnabled = true;
            this.listMonthly.ItemHeight = 15;
            this.listMonthly.Location = new System.Drawing.Point(12, 370);
            this.listMonthly.Name = "listMonthly";
            this.listMonthly.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listMonthly.Size = new System.Drawing.Size(242, 199);
            this.listMonthly.TabIndex = 8;
            this.listMonthly.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listMonthly_MouseDown);
            // 
            // butClose
            // 
            this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butClose.Location = new System.Drawing.Point(558, 658);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(110, 30);
            this.butClose.TabIndex = 0;
            this.butClose.Text = "Close";
            this.butClose.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // setupButton
            // 
            this.setupButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setupButton.Location = new System.Drawing.Point(557, 19);
            this.setupButton.Name = "setupButton";
            this.setupButton.Size = new System.Drawing.Size(110, 30);
            this.setupButton.TabIndex = 29;
            this.setupButton.Text = "&Setup";
            this.setupButton.Click += new System.EventHandler(this.SetupButton_Click);
            // 
            // FormReportsMore
            // 
            this.CancelButton = this.butClose;
            this.ClientSize = new System.Drawing.Size(680, 700);
            this.Controls.Add(this.setupButton);
            this.Controls.Add(this.ehrPatientExportButton);
            this.Controls.Add(this.ehrPatientListButton);
            this.Controls.Add(this.labelArizonaPrimaryCare);
            this.Controls.Add(this.listArizonaPrimaryCare);
            this.Controls.Add(this.laserLabelsButton);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.listDaily);
            this.Controls.Add(this.labelDaily);
            this.Controls.Add(this.listProdInc);
            this.Controls.Add(this.labelProdInc);
            this.Controls.Add(this.butPW);
            this.Controls.Add(this.userQueryButton);
            this.Controls.Add(this.listPublicHealth);
            this.Controls.Add(this.listLists);
            this.Controls.Add(this.listMonthly);
            this.Controls.Add(this.labelMonthly);
            this.Controls.Add(this.labelLists);
            this.Controls.Add(this.labelPublicHealth);
            this.Controls.Add(this.butClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormReportsMore";
            this.ShowInTaskbar = false;
            this.Text = "Reports";
            this.Load += new System.EventHandler(this.FormReportsMore_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.Label labelPublicHealth;
        private System.Windows.Forms.Label labelLists;
        private System.Windows.Forms.Label labelMonthly;
        private OpenDental.UI.ListBoxClickable listLists;
        private OpenDental.UI.ListBoxClickable listPublicHealth;
        private System.Windows.Forms.Button userQueryButton;
        private System.Windows.Forms.Button butPW;
        private OpenDental.UI.ListBoxClickable listProdInc;
        private System.Windows.Forms.Label labelProdInc;
        private OpenDental.UI.ListBoxClickable listDaily;
        private System.Windows.Forms.Label labelDaily;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button laserLabelsButton;
        private OpenDental.UI.ListBoxClickable listArizonaPrimaryCare;
        private System.Windows.Forms.Label labelArizonaPrimaryCare;
        private OpenDental.UI.ListBoxClickable listMonthly;
        private System.Windows.Forms.Button ehrPatientListButton;
        private System.Windows.Forms.Button ehrPatientExportButton;
        private System.Windows.Forms.Button setupButton;
    }
}