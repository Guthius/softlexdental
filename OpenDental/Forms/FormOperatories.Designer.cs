namespace OpenDental
{
    partial class FormOperatories
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOperatories));
            this.infoLabel = new System.Windows.Forms.Label();
            this.operatoriesGrid = new OpenDental.UI.ODGrid();
            this.clinicComboBox = new System.Windows.Forms.ComboBox();
            this.clinicLabel = new System.Windows.Forms.Label();
            this.pickClinicButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.combineButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(13, 22);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(226, 15);
            this.infoLabel.TabIndex = 12;
            this.infoLabel.Text = "(Also, see the appointment views section)";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // operatoriesGrid
            // 
            this.operatoriesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.operatoriesGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.operatoriesGrid.EditableEnterMovesDown = false;
            this.operatoriesGrid.HasAddButton = false;
            this.operatoriesGrid.HasDropDowns = false;
            this.operatoriesGrid.HasMultilineHeaders = false;
            this.operatoriesGrid.HScrollVisible = false;
            this.operatoriesGrid.Location = new System.Drawing.Point(13, 48);
            this.operatoriesGrid.Name = "operatoriesGrid";
            this.operatoriesGrid.ScrollValue = 0;
            this.operatoriesGrid.SelectionMode = OpenDental.UI.GridSelectionMode.Multiple;
            this.operatoriesGrid.Size = new System.Drawing.Size(802, 440);
            this.operatoriesGrid.TabIndex = 11;
            this.operatoriesGrid.Title = "Operatories";
            this.operatoriesGrid.TitleVisible = true;
            this.operatoriesGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.OperatoriesGrid_CellDoubleClick);
            // 
            // clinicComboBox
            // 
            this.clinicComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clinicComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clinicComboBox.Location = new System.Drawing.Point(549, 19);
            this.clinicComboBox.MaxDropDownItems = 30;
            this.clinicComboBox.Name = "clinicComboBox";
            this.clinicComboBox.Size = new System.Drawing.Size(230, 23);
            this.clinicComboBox.TabIndex = 119;
            this.clinicComboBox.SelectionChangeCommitted += new System.EventHandler(this.ClinicComboBox_SelectionChangeCommitted);
            // 
            // clinicLabel
            // 
            this.clinicLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clinicLabel.AutoSize = true;
            this.clinicLabel.Location = new System.Drawing.Point(506, 22);
            this.clinicLabel.Name = "clinicLabel";
            this.clinicLabel.Size = new System.Drawing.Size(37, 15);
            this.clinicLabel.TabIndex = 120;
            this.clinicLabel.Text = "Clinic";
            this.clinicLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pickClinicButton
            // 
            this.pickClinicButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pickClinicButton.Location = new System.Drawing.Point(785, 18);
            this.pickClinicButton.Name = "pickClinicButton";
            this.pickClinicButton.Size = new System.Drawing.Size(30, 25);
            this.pickClinicButton.TabIndex = 121;
            this.pickClinicButton.Text = "...";
            this.pickClinicButton.Click += new System.EventHandler(this.PickClinicButton_Click);
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.Image = global::OpenDental.Properties.Resources.IconArrowDown;
            this.downButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.downButton.Location = new System.Drawing.Point(821, 230);
            this.downButton.Name = "downButton";
            this.downButton.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.downButton.Size = new System.Drawing.Size(110, 30);
            this.downButton.TabIndex = 14;
            this.downButton.Text = "&Down";
            this.downButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.Image = global::OpenDental.Properties.Resources.IconArrowUp;
            this.upButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.upButton.Location = new System.Drawing.Point(821, 194);
            this.upButton.Name = "upButton";
            this.upButton.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.upButton.Size = new System.Drawing.Size(110, 30);
            this.upButton.TabIndex = 13;
            this.upButton.Text = "&Up";
            this.upButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(821, 48);
            this.addButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 40);
            this.addButton.Name = "addButton";
            this.addButton.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.addButton.Size = new System.Drawing.Size(110, 30);
            this.addButton.TabIndex = 10;
            this.addButton.Text = "&Add";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(821, 458);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(110, 30);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // combineButton
            // 
            this.combineButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.combineButton.Location = new System.Drawing.Point(821, 121);
            this.combineButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 40);
            this.combineButton.Name = "combineButton";
            this.combineButton.Size = new System.Drawing.Size(110, 30);
            this.combineButton.TabIndex = 122;
            this.combineButton.Text = "Co&mbine";
            this.combineButton.Click += new System.EventHandler(this.CombineButton_Click);
            // 
            // FormOperatories
            // 
            this.ClientSize = new System.Drawing.Size(944, 501);
            this.Controls.Add(this.combineButton);
            this.Controls.Add(this.pickClinicButton);
            this.Controls.Add(this.clinicComboBox);
            this.Controls.Add(this.clinicLabel);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.operatoriesGrid);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.closeButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(850, 350);
            this.Name = "FormOperatories";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Operatories";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormOperatories_Closing);
            this.Load += new System.EventHandler(this.FormOperatories_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button closeButton;
        private OpenDental.UI.ODGrid operatoriesGrid;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Label clinicLabel;
        private System.Windows.Forms.Button pickClinicButton;
        private System.Windows.Forms.Button combineButton;
        private System.Windows.Forms.ComboBox clinicComboBox;
    }
}
