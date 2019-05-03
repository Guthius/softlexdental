namespace OpenDental
{
    partial class FormZipSelect
    {
        private System.ComponentModel.Container components = null;

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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormZipSelect));
            this.acceptButton = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.matchesListBox = new System.Windows.Forms.ListBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.butEdit = new System.Windows.Forms.Button();
            this.butDelete = new System.Windows.Forms.Button();
            this.butAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(261, 185);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 5;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(261, 218);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(110, 30);
            this.butCancel.TabIndex = 6;
            this.butCancel.Text = "&Cancel";
            // 
            // matchesListBox
            // 
            this.matchesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.matchesListBox.IntegralHeight = false;
            this.matchesListBox.ItemHeight = 15;
            this.matchesListBox.Location = new System.Drawing.Point(13, 34);
            this.matchesListBox.Name = "matchesListBox";
            this.matchesListBox.Size = new System.Drawing.Size(210, 178);
            this.matchesListBox.TabIndex = 1;
            this.matchesListBox.DoubleClick += new System.EventHandler(this.matchesListBox_DoubleClick);
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(13, 16);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(165, 15);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "Cities attached to this zipcode\r\n";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // butEdit
            // 
            this.butEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butEdit.Image = global::OpenDental.Properties.Resources.IconEdit;
            this.butEdit.Location = new System.Drawing.Point(105, 218);
            this.butEdit.Name = "butEdit";
            this.butEdit.Size = new System.Drawing.Size(40, 30);
            this.butEdit.TabIndex = 4;
            this.butEdit.Click += new System.EventHandler(this.editButton_Click);
            // 
            // butDelete
            // 
            this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butDelete.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.butDelete.Location = new System.Drawing.Point(59, 218);
            this.butDelete.Name = "butDelete";
            this.butDelete.Size = new System.Drawing.Size(40, 30);
            this.butDelete.TabIndex = 3;
            this.butDelete.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // butAdd
            // 
            this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butAdd.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.butAdd.Location = new System.Drawing.Point(13, 218);
            this.butAdd.Name = "butAdd";
            this.butAdd.Size = new System.Drawing.Size(40, 30);
            this.butAdd.TabIndex = 2;
            this.butAdd.Click += new System.EventHandler(this.addButton_Click);
            // 
            // FormZipSelect
            // 
            this.AcceptButton = this.acceptButton;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.butEdit);
            this.Controls.Add(this.butDelete);
            this.Controls.Add(this.butAdd);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.matchesListBox);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.acceptButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormZipSelect";
            this.ShowInTaskbar = false;
            this.Text = "Select Zipcode";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormZipSelect_Closing);
            this.Load += new System.EventHandler(this.FormZipSelect_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.ListBox matchesListBox;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button butEdit;
        private System.Windows.Forms.Button butDelete;
        private System.Windows.Forms.Button butAdd;
    }
}