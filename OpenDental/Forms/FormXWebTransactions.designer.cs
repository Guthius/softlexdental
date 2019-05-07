namespace OpenDental
{
    partial class FormXWebTransactions
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormXWebTransactions));
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.goToMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openPaymentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.voidPaymentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processReturnMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dateFromLabel = new System.Windows.Forms.Label();
            this.dateToLabel = new System.Windows.Forms.Label();
            this.transactionsGrid = new OpenDental.UI.ODGrid();
            this.butRefresh = new System.Windows.Forms.Button();
            this.dateFromTextBox = new OpenDental.ValidDate();
            this.dateToTextBox = new OpenDental.ValidDate();
            this.closeButton = new System.Windows.Forms.Button();
            this.clinicComboBox = new System.Windows.Forms.ComboBox();
            this.clinicLabel = new System.Windows.Forms.Label();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToMenuItem,
            this.openPaymentMenuItem,
            this.voidPaymentMenuItem,
            this.processReturnMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(154, 92);
            // 
            // goToMenuItem
            // 
            this.goToMenuItem.Name = "goToMenuItem";
            this.goToMenuItem.Size = new System.Drawing.Size(153, 22);
            this.goToMenuItem.Text = "Go To Account";
            this.goToMenuItem.Click += new System.EventHandler(this.GoToMenuItem_Click);
            // 
            // openPaymentMenuItem
            // 
            this.openPaymentMenuItem.Name = "openPaymentMenuItem";
            this.openPaymentMenuItem.Size = new System.Drawing.Size(153, 22);
            this.openPaymentMenuItem.Text = "Open Payment";
            this.openPaymentMenuItem.Click += new System.EventHandler(this.OpenPaymentMenuItem_Click);
            // 
            // voidPaymentMenuItem
            // 
            this.voidPaymentMenuItem.Name = "voidPaymentMenuItem";
            this.voidPaymentMenuItem.Size = new System.Drawing.Size(153, 22);
            this.voidPaymentMenuItem.Text = "Void Payment";
            this.voidPaymentMenuItem.Click += new System.EventHandler(this.VoidPaymentMenuItem_Click);
            // 
            // processReturnMenuItem
            // 
            this.processReturnMenuItem.Name = "processReturnMenuItem";
            this.processReturnMenuItem.Size = new System.Drawing.Size(153, 22);
            this.processReturnMenuItem.Text = "Process Return";
            this.processReturnMenuItem.Click += new System.EventHandler(this.ProcessReturnMenuItem_Click);
            // 
            // dateFromLabel
            // 
            this.dateFromLabel.AutoSize = true;
            this.dateFromLabel.Location = new System.Drawing.Point(32, 23);
            this.dateFromLabel.Name = "dateFromLabel";
            this.dateFromLabel.Size = new System.Drawing.Size(62, 15);
            this.dateFromLabel.TabIndex = 2;
            this.dateFromLabel.Text = "From Date";
            this.dateFromLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateToLabel
            // 
            this.dateToLabel.AutoSize = true;
            this.dateToLabel.Location = new System.Drawing.Point(207, 23);
            this.dateToLabel.Name = "dateToLabel";
            this.dateToLabel.Size = new System.Drawing.Size(47, 15);
            this.dateToLabel.TabIndex = 4;
            this.dateToLabel.Text = "To Date";
            this.dateToLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // transactionsGrid
            // 
            this.transactionsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.transactionsGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.transactionsGrid.ContextMenuStrip = this.contextMenu;
            this.transactionsGrid.EditableEnterMovesDown = false;
            this.transactionsGrid.HasAddButton = false;
            this.transactionsGrid.HasDropDowns = false;
            this.transactionsGrid.HasMultilineHeaders = false;
            this.transactionsGrid.HScrollVisible = false;
            this.transactionsGrid.Location = new System.Drawing.Point(13, 48);
            this.transactionsGrid.Name = "transactionsGrid";
            this.transactionsGrid.ScrollValue = 0;
            this.transactionsGrid.Size = new System.Drawing.Size(818, 484);
            this.transactionsGrid.TabIndex = 0;
            this.transactionsGrid.Title = "XWeb Transactions";
            this.transactionsGrid.TitleVisible = true;
            this.transactionsGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.TransactionsGrid_CellDoubleClick);
            this.transactionsGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TransactionsGrid_MouseClick);
            this.transactionsGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TransactionsGrid_MouseDown);
            // 
            // butRefresh
            // 
            this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butRefresh.Location = new System.Drawing.Point(761, 18);
            this.butRefresh.Name = "butRefresh";
            this.butRefresh.Size = new System.Drawing.Size(70, 25);
            this.butRefresh.TabIndex = 8;
            this.butRefresh.Text = "Refresh";
            this.butRefresh.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // dateFromTextBox
            // 
            this.dateFromTextBox.Location = new System.Drawing.Point(100, 19);
            this.dateFromTextBox.Name = "dateFromTextBox";
            this.dateFromTextBox.Size = new System.Drawing.Size(90, 23);
            this.dateFromTextBox.TabIndex = 3;
            // 
            // dateToTextBox
            // 
            this.dateToTextBox.Location = new System.Drawing.Point(260, 19);
            this.dateToTextBox.Name = "dateToTextBox";
            this.dateToTextBox.Size = new System.Drawing.Size(90, 23);
            this.dateToTextBox.TabIndex = 5;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(721, 538);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(110, 30);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // clinicComboBox
            // 
            this.clinicComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clinicComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clinicComboBox.Location = new System.Drawing.Point(575, 19);
            this.clinicComboBox.MaxDropDownItems = 40;
            this.clinicComboBox.Name = "clinicComboBox";
            this.clinicComboBox.Size = new System.Drawing.Size(180, 23);
            this.clinicComboBox.TabIndex = 7;
            // 
            // clinicLabel
            // 
            this.clinicLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clinicLabel.AutoSize = true;
            this.clinicLabel.Location = new System.Drawing.Point(532, 22);
            this.clinicLabel.Name = "clinicLabel";
            this.clinicLabel.Size = new System.Drawing.Size(37, 15);
            this.clinicLabel.TabIndex = 6;
            this.clinicLabel.Text = "Clinic";
            this.clinicLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // FormXWebTransactions
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(844, 581);
            this.Controls.Add(this.clinicComboBox);
            this.Controls.Add(this.clinicLabel);
            this.Controls.Add(this.butRefresh);
            this.Controls.Add(this.dateFromTextBox);
            this.Controls.Add(this.dateToTextBox);
            this.Controls.Add(this.dateFromLabel);
            this.Controls.Add(this.dateToLabel);
            this.Controls.Add(this.transactionsGrid);
            this.Controls.Add(this.closeButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 400);
            this.Name = "FormXWebTransactions";
            this.ShowInTaskbar = false;
            this.Text = "XWeb Transactions";
            this.Load += new System.EventHandler(this.FormXWebTransactions_Load);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private UI.ODGrid transactionsGrid;
        private ValidDate dateFromTextBox;
        private ValidDate dateToTextBox;
        private System.Windows.Forms.Label dateFromLabel;
        private System.Windows.Forms.Label dateToLabel;
        private System.Windows.Forms.Button butRefresh;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem goToMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openPaymentMenuItem;
        private System.Windows.Forms.ToolStripMenuItem voidPaymentMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processReturnMenuItem;
        private System.Windows.Forms.ComboBox clinicComboBox;
        private System.Windows.Forms.Label clinicLabel;
    }
}