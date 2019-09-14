namespace OpenDental
{
    partial class FormAccountingSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccountingSetup));
            this.accountDepositLabel = new System.Windows.Forms.Label();
            this.depositAccountsListBox = new System.Windows.Forms.ListBox();
            this.accountDepositRemoveButton = new System.Windows.Forms.Button();
            this.changeDepositAccountButton = new System.Windows.Forms.Button();
            this.depositAccountLabel = new System.Windows.Forms.Label();
            this.depositAccountTextBox = new System.Windows.Forms.TextBox();
            this.depositInfoLabel = new System.Windows.Forms.Label();
            this.accountDepositAddButton = new System.Windows.Forms.Button();
            this.autoPayGrid = new OpenDental.UI.ODGrid();
            this.changeIncomeAccountButton = new System.Windows.Forms.Button();
            this.accountLabel = new System.Windows.Forms.Label();
            this.accountTextBox = new System.Windows.Forms.TextBox();
            this.autoPayInfoLabel = new System.Windows.Forms.Label();
            this.autoPayAddButton = new System.Windows.Forms.Button();
            this.softwareLabel = new System.Windows.Forms.Label();
            this.quickBooksClassRefRemoveButton = new System.Windows.Forms.Button();
            this.quickBooksClassRefAddButton = new System.Windows.Forms.Button();
            this.quickBooksClassRefsListBox = new System.Windows.Forms.ListBox();
            this.quickBooksClassRefsLabel = new System.Windows.Forms.Label();
            this.quickBooksIncomeAccountRemoveButton = new System.Windows.Forms.Button();
            this.quickBooksClassRefsCheckBox = new System.Windows.Forms.CheckBox();
            this.quickBooksIncomeAccountAddButton = new System.Windows.Forms.Button();
            this.quickBooksIncomeAccountsListBox = new System.Windows.Forms.ListBox();
            this.quickBooksTitleLabel = new System.Windows.Forms.Label();
            this.quickBooksIncomeAccountsLabel = new System.Windows.Forms.Label();
            this.quickBooksWarningLabel = new System.Windows.Forms.Label();
            this.testQuickBooksButton = new System.Windows.Forms.Button();
            this.browseCompanyFileButton = new System.Windows.Forms.Button();
            this.quickBooksCompanyFileLabel = new System.Windows.Forms.Label();
            this.quickBooksCompanyFileTextBox = new System.Windows.Forms.TextBox();
            this.quickBooksDepositAccountsListBox = new System.Windows.Forms.ListBox();
            this.quickBooksDepositAccountRemoveButton = new System.Windows.Forms.Button();
            this.quickBooksDepositAccountAddButton = new System.Windows.Forms.Button();
            this.quickBooksDepositAccountsLabel = new System.Windows.Forms.Label();
            this.quickBooksDepositInfoLabel = new System.Windows.Forms.Label();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.accountingTabControl = new System.Windows.Forms.TabControl();
            this.accountAutoPaysTabPage = new System.Windows.Forms.TabPage();
            this.softlexDentalTabPage = new System.Windows.Forms.TabPage();
            this.quickBooksTabPage = new System.Windows.Forms.TabPage();
            this.softwareComboBox = new System.Windows.Forms.ComboBox();
            this.accountingTabControl.SuspendLayout();
            this.accountAutoPaysTabPage.SuspendLayout();
            this.softlexDentalTabPage.SuspendLayout();
            this.quickBooksTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // accountDepositLabel
            // 
            this.accountDepositLabel.Location = new System.Drawing.Point(6, 46);
            this.accountDepositLabel.Name = "accountDepositLabel";
            this.accountDepositLabel.Size = new System.Drawing.Size(168, 60);
            this.accountDepositLabel.TabIndex = 1;
            this.accountDepositLabel.Text = "User will get to pick from this list of accounts to deposit into";
            this.accountDepositLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // depositAccountsListBox
            // 
            this.depositAccountsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.depositAccountsListBox.FormattingEnabled = true;
            this.depositAccountsListBox.IntegralHeight = false;
            this.depositAccountsListBox.ItemHeight = 15;
            this.depositAccountsListBox.Location = new System.Drawing.Point(180, 46);
            this.depositAccountsListBox.Name = "depositAccountsListBox";
            this.depositAccountsListBox.Size = new System.Drawing.Size(338, 160);
            this.depositAccountsListBox.TabIndex = 2;
            // 
            // accountDepositRemoveButton
            // 
            this.accountDepositRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.accountDepositRemoveButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.accountDepositRemoveButton.Location = new System.Drawing.Point(524, 82);
            this.accountDepositRemoveButton.Name = "accountDepositRemoveButton";
            this.accountDepositRemoveButton.Size = new System.Drawing.Size(40, 30);
            this.accountDepositRemoveButton.TabIndex = 4;
            this.accountDepositRemoveButton.Click += new System.EventHandler(this.AccountDepositRemoveButton_Click);
            // 
            // changeDepositAccountButton
            // 
            this.changeDepositAccountButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.changeDepositAccountButton.Location = new System.Drawing.Point(454, 211);
            this.changeDepositAccountButton.Name = "changeDepositAccountButton";
            this.changeDepositAccountButton.Size = new System.Drawing.Size(110, 25);
            this.changeDepositAccountButton.TabIndex = 7;
            this.changeDepositAccountButton.Text = "Change";
            this.changeDepositAccountButton.Click += new System.EventHandler(this.ChangeDepositAccountButton_Click);
            // 
            // depositAccountLabel
            // 
            this.depositAccountLabel.AutoSize = true;
            this.depositAccountLabel.Location = new System.Drawing.Point(79, 216);
            this.depositAccountLabel.Name = "depositAccountLabel";
            this.depositAccountLabel.Size = new System.Drawing.Size(95, 15);
            this.depositAccountLabel.TabIndex = 5;
            this.depositAccountLabel.Text = "Income Account";
            this.depositAccountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // depositAccountTextBox
            // 
            this.depositAccountTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.depositAccountTextBox.Location = new System.Drawing.Point(180, 212);
            this.depositAccountTextBox.Name = "depositAccountTextBox";
            this.depositAccountTextBox.ReadOnly = true;
            this.depositAccountTextBox.Size = new System.Drawing.Size(268, 23);
            this.depositAccountTextBox.TabIndex = 6;
            // 
            // depositInfoLabel
            // 
            this.depositInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.depositInfoLabel.Location = new System.Drawing.Point(6, 3);
            this.depositInfoLabel.Name = "depositInfoLabel";
            this.depositInfoLabel.Size = new System.Drawing.Size(558, 40);
            this.depositInfoLabel.TabIndex = 0;
            this.depositInfoLabel.Text = "Every time a deposit is created, an accounting transaction will also be automatic" +
    "ally created.";
            // 
            // accountDepositAddButton
            // 
            this.accountDepositAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.accountDepositAddButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.accountDepositAddButton.Location = new System.Drawing.Point(524, 46);
            this.accountDepositAddButton.Name = "accountDepositAddButton";
            this.accountDepositAddButton.Size = new System.Drawing.Size(40, 30);
            this.accountDepositAddButton.TabIndex = 3;
            this.accountDepositAddButton.Click += new System.EventHandler(this.AccountDepositAddButton_Click);
            // 
            // autoPayGrid
            // 
            this.autoPayGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.autoPayGrid.EditableEnterMovesDown = false;
            this.autoPayGrid.HasAddButton = false;
            this.autoPayGrid.HasDropDowns = false;
            this.autoPayGrid.HasMultilineHeaders = false;
            this.autoPayGrid.HScrollVisible = false;
            this.autoPayGrid.Location = new System.Drawing.Point(6, 66);
            this.autoPayGrid.Name = "autoPayGrid";
            this.autoPayGrid.ScrollValue = 0;
            this.autoPayGrid.Size = new System.Drawing.Size(512, 199);
            this.autoPayGrid.TabIndex = 1;
            this.autoPayGrid.Title = "Auto Payment Entries";
            this.autoPayGrid.TitleVisible = true;
            this.autoPayGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.AutoPayGrid_CellDoubleClick);
            // 
            // changeIncomeAccountButton
            // 
            this.changeIncomeAccountButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.changeIncomeAccountButton.Location = new System.Drawing.Point(454, 339);
            this.changeIncomeAccountButton.Name = "changeIncomeAccountButton";
            this.changeIncomeAccountButton.Size = new System.Drawing.Size(110, 25);
            this.changeIncomeAccountButton.TabIndex = 5;
            this.changeIncomeAccountButton.Text = "Change";
            this.changeIncomeAccountButton.Click += new System.EventHandler(this.ChangeIncomeAccountButton_Click);
            // 
            // accountLabel
            // 
            this.accountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.accountLabel.AutoSize = true;
            this.accountLabel.Location = new System.Drawing.Point(79, 344);
            this.accountLabel.Name = "accountLabel";
            this.accountLabel.Size = new System.Drawing.Size(95, 15);
            this.accountLabel.TabIndex = 3;
            this.accountLabel.Text = "Income Account";
            this.accountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // accountTextBox
            // 
            this.accountTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accountTextBox.Location = new System.Drawing.Point(180, 340);
            this.accountTextBox.Name = "accountTextBox";
            this.accountTextBox.ReadOnly = true;
            this.accountTextBox.Size = new System.Drawing.Size(268, 23);
            this.accountTextBox.TabIndex = 4;
            // 
            // autoPayInfoLabel
            // 
            this.autoPayInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoPayInfoLabel.Location = new System.Drawing.Point(6, 3);
            this.autoPayInfoLabel.Name = "autoPayInfoLabel";
            this.autoPayInfoLabel.Size = new System.Drawing.Size(558, 60);
            this.autoPayInfoLabel.TabIndex = 0;
            this.autoPayInfoLabel.Text = resources.GetString("autoPayInfoLabel.Text");
            // 
            // autoPayAddButton
            // 
            this.autoPayAddButton.Image = global::OpenDental.Properties.Resources.IconAdd;
            this.autoPayAddButton.Location = new System.Drawing.Point(524, 66);
            this.autoPayAddButton.Name = "autoPayAddButton";
            this.autoPayAddButton.Size = new System.Drawing.Size(40, 30);
            this.autoPayAddButton.TabIndex = 2;
            this.autoPayAddButton.Click += new System.EventHandler(this.AutoPayAddButton_Click);
            // 
            // softwareLabel
            // 
            this.softwareLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.softwareLabel.AutoSize = true;
            this.softwareLabel.Location = new System.Drawing.Point(269, 22);
            this.softwareLabel.Name = "softwareLabel";
            this.softwareLabel.Size = new System.Drawing.Size(96, 15);
            this.softwareLabel.TabIndex = 0;
            this.softwareLabel.Text = "Deposit Software";
            // 
            // quickBooksClassRefRemoveButton
            // 
            this.quickBooksClassRefRemoveButton.Image = ((System.Drawing.Image)(resources.GetObject("quickBooksClassRefRemoveButton.Image")));
            this.quickBooksClassRefRemoveButton.Location = new System.Drawing.Point(524, 496);
            this.quickBooksClassRefRemoveButton.Name = "quickBooksClassRefRemoveButton";
            this.quickBooksClassRefRemoveButton.Size = new System.Drawing.Size(40, 30);
            this.quickBooksClassRefRemoveButton.TabIndex = 18;
            this.quickBooksClassRefRemoveButton.Visible = false;
            this.quickBooksClassRefRemoveButton.Click += new System.EventHandler(this.QuickBooksClassRefRemoveButton_Click);
            // 
            // quickBooksClassRefAddButton
            // 
            this.quickBooksClassRefAddButton.Image = ((System.Drawing.Image)(resources.GetObject("quickBooksClassRefAddButton.Image")));
            this.quickBooksClassRefAddButton.Location = new System.Drawing.Point(524, 460);
            this.quickBooksClassRefAddButton.Name = "quickBooksClassRefAddButton";
            this.quickBooksClassRefAddButton.Size = new System.Drawing.Size(40, 30);
            this.quickBooksClassRefAddButton.TabIndex = 17;
            this.quickBooksClassRefAddButton.Visible = false;
            this.quickBooksClassRefAddButton.Click += new System.EventHandler(this.QuickBooksClassRefAddButton_Click);
            // 
            // quickBooksClassRefsListBox
            // 
            this.quickBooksClassRefsListBox.FormattingEnabled = true;
            this.quickBooksClassRefsListBox.IntegralHeight = false;
            this.quickBooksClassRefsListBox.ItemHeight = 15;
            this.quickBooksClassRefsListBox.Location = new System.Drawing.Point(180, 460);
            this.quickBooksClassRefsListBox.Name = "quickBooksClassRefsListBox";
            this.quickBooksClassRefsListBox.Size = new System.Drawing.Size(338, 94);
            this.quickBooksClassRefsListBox.TabIndex = 16;
            this.quickBooksClassRefsListBox.Visible = false;
            // 
            // quickBooksClassRefsLabel
            // 
            this.quickBooksClassRefsLabel.AutoSize = true;
            this.quickBooksClassRefsLabel.Location = new System.Drawing.Point(119, 460);
            this.quickBooksClassRefsLabel.Name = "quickBooksClassRefsLabel";
            this.quickBooksClassRefsLabel.Size = new System.Drawing.Size(55, 15);
            this.quickBooksClassRefsLabel.TabIndex = 15;
            this.quickBooksClassRefsLabel.Text = "Class List";
            this.quickBooksClassRefsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.quickBooksClassRefsLabel.Visible = false;
            // 
            // quickBooksIncomeAccountRemoveButton
            // 
            this.quickBooksIncomeAccountRemoveButton.Image = ((System.Drawing.Image)(resources.GetObject("quickBooksIncomeAccountRemoveButton.Image")));
            this.quickBooksIncomeAccountRemoveButton.Location = new System.Drawing.Point(524, 342);
            this.quickBooksIncomeAccountRemoveButton.Name = "quickBooksIncomeAccountRemoveButton";
            this.quickBooksIncomeAccountRemoveButton.Size = new System.Drawing.Size(40, 30);
            this.quickBooksIncomeAccountRemoveButton.TabIndex = 13;
            this.quickBooksIncomeAccountRemoveButton.Click += new System.EventHandler(this.QuickBooksIncomeAccountRemoveButton_Click);
            // 
            // quickBooksClassRefsCheckBox
            // 
            this.quickBooksClassRefsCheckBox.AutoSize = true;
            this.quickBooksClassRefsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.quickBooksClassRefsCheckBox.Location = new System.Drawing.Point(180, 434);
            this.quickBooksClassRefsCheckBox.Name = "quickBooksClassRefsCheckBox";
            this.quickBooksClassRefsCheckBox.Size = new System.Drawing.Size(188, 20);
            this.quickBooksClassRefsCheckBox.TabIndex = 14;
            this.quickBooksClassRefsCheckBox.Text = "Enable QuickBooks Class Refs";
            this.quickBooksClassRefsCheckBox.CheckedChanged += new System.EventHandler(this.QuickBooksClassRefsCheckBox_CheckedChanged);
            // 
            // quickBooksIncomeAccountAddButton
            // 
            this.quickBooksIncomeAccountAddButton.Image = ((System.Drawing.Image)(resources.GetObject("quickBooksIncomeAccountAddButton.Image")));
            this.quickBooksIncomeAccountAddButton.Location = new System.Drawing.Point(524, 306);
            this.quickBooksIncomeAccountAddButton.Name = "quickBooksIncomeAccountAddButton";
            this.quickBooksIncomeAccountAddButton.Size = new System.Drawing.Size(40, 30);
            this.quickBooksIncomeAccountAddButton.TabIndex = 12;
            this.quickBooksIncomeAccountAddButton.Click += new System.EventHandler(this.QuickBooksIncomeAccountAddButton_Click);
            // 
            // quickBooksIncomeAccountsListBox
            // 
            this.quickBooksIncomeAccountsListBox.FormattingEnabled = true;
            this.quickBooksIncomeAccountsListBox.IntegralHeight = false;
            this.quickBooksIncomeAccountsListBox.ItemHeight = 15;
            this.quickBooksIncomeAccountsListBox.Location = new System.Drawing.Point(180, 306);
            this.quickBooksIncomeAccountsListBox.Name = "quickBooksIncomeAccountsListBox";
            this.quickBooksIncomeAccountsListBox.Size = new System.Drawing.Size(338, 100);
            this.quickBooksIncomeAccountsListBox.TabIndex = 11;
            // 
            // quickBooksTitleLabel
            // 
            this.quickBooksTitleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.quickBooksTitleLabel.Location = new System.Drawing.Point(6, 3);
            this.quickBooksTitleLabel.Name = "quickBooksTitleLabel";
            this.quickBooksTitleLabel.Size = new System.Drawing.Size(558, 80);
            this.quickBooksTitleLabel.TabIndex = 0;
            this.quickBooksTitleLabel.Text = resources.GetString("quickBooksTitleLabel.Text");
            // 
            // quickBooksIncomeAccountsLabel
            // 
            this.quickBooksIncomeAccountsLabel.Location = new System.Drawing.Point(6, 306);
            this.quickBooksIncomeAccountsLabel.Name = "quickBooksIncomeAccountsLabel";
            this.quickBooksIncomeAccountsLabel.Size = new System.Drawing.Size(168, 100);
            this.quickBooksIncomeAccountsLabel.TabIndex = 10;
            this.quickBooksIncomeAccountsLabel.Text = "User will get to pick from this list of income accounts.";
            this.quickBooksIncomeAccountsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // quickBooksWarningLabel
            // 
            this.quickBooksWarningLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.quickBooksWarningLabel.Location = new System.Drawing.Point(6, 573);
            this.quickBooksWarningLabel.Name = "quickBooksWarningLabel";
            this.quickBooksWarningLabel.Size = new System.Drawing.Size(558, 40);
            this.quickBooksWarningLabel.TabIndex = 19;
            this.quickBooksWarningLabel.Text = "Open Dental will run faster if your QuickBooks company file is open in the backgr" +
    "ound.";
            this.quickBooksWarningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // testQuickBooksButton
            // 
            this.testQuickBooksButton.Location = new System.Drawing.Point(494, 85);
            this.testQuickBooksButton.Name = "testQuickBooksButton";
            this.testQuickBooksButton.Size = new System.Drawing.Size(70, 25);
            this.testQuickBooksButton.TabIndex = 4;
            this.testQuickBooksButton.Text = "Test";
            this.testQuickBooksButton.Click += new System.EventHandler(this.TestQuickBooksButton_Click);
            // 
            // browseCompanyFileButton
            // 
            this.browseCompanyFileButton.Image = global::OpenDental.Properties.Resources.IconFolder;
            this.browseCompanyFileButton.Location = new System.Drawing.Point(453, 85);
            this.browseCompanyFileButton.Name = "browseCompanyFileButton";
            this.browseCompanyFileButton.Size = new System.Drawing.Size(35, 25);
            this.browseCompanyFileButton.TabIndex = 3;
            this.browseCompanyFileButton.Click += new System.EventHandler(this.BrowseCompanyFileButton_Click);
            // 
            // quickBooksCompanyFileLabel
            // 
            this.quickBooksCompanyFileLabel.AutoSize = true;
            this.quickBooksCompanyFileLabel.Location = new System.Drawing.Point(94, 89);
            this.quickBooksCompanyFileLabel.Name = "quickBooksCompanyFileLabel";
            this.quickBooksCompanyFileLabel.Size = new System.Drawing.Size(80, 15);
            this.quickBooksCompanyFileLabel.TabIndex = 1;
            this.quickBooksCompanyFileLabel.Text = "Company File";
            this.quickBooksCompanyFileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // quickBooksCompanyFileTextBox
            // 
            this.quickBooksCompanyFileTextBox.Location = new System.Drawing.Point(180, 86);
            this.quickBooksCompanyFileTextBox.Name = "quickBooksCompanyFileTextBox";
            this.quickBooksCompanyFileTextBox.Size = new System.Drawing.Size(267, 23);
            this.quickBooksCompanyFileTextBox.TabIndex = 2;
            // 
            // quickBooksDepositAccountsListBox
            // 
            this.quickBooksDepositAccountsListBox.FormattingEnabled = true;
            this.quickBooksDepositAccountsListBox.IntegralHeight = false;
            this.quickBooksDepositAccountsListBox.ItemHeight = 15;
            this.quickBooksDepositAccountsListBox.Location = new System.Drawing.Point(180, 200);
            this.quickBooksDepositAccountsListBox.Name = "quickBooksDepositAccountsListBox";
            this.quickBooksDepositAccountsListBox.Size = new System.Drawing.Size(338, 100);
            this.quickBooksDepositAccountsListBox.TabIndex = 7;
            // 
            // quickBooksDepositAccountRemoveButton
            // 
            this.quickBooksDepositAccountRemoveButton.Image = ((System.Drawing.Image)(resources.GetObject("quickBooksDepositAccountRemoveButton.Image")));
            this.quickBooksDepositAccountRemoveButton.Location = new System.Drawing.Point(524, 236);
            this.quickBooksDepositAccountRemoveButton.Name = "quickBooksDepositAccountRemoveButton";
            this.quickBooksDepositAccountRemoveButton.Size = new System.Drawing.Size(40, 30);
            this.quickBooksDepositAccountRemoveButton.TabIndex = 9;
            this.quickBooksDepositAccountRemoveButton.Click += new System.EventHandler(this.QuickBooksDepositAccountRemoveButton_Click);
            // 
            // quickBooksDepositAccountAddButton
            // 
            this.quickBooksDepositAccountAddButton.Image = ((System.Drawing.Image)(resources.GetObject("quickBooksDepositAccountAddButton.Image")));
            this.quickBooksDepositAccountAddButton.Location = new System.Drawing.Point(524, 200);
            this.quickBooksDepositAccountAddButton.Name = "quickBooksDepositAccountAddButton";
            this.quickBooksDepositAccountAddButton.Size = new System.Drawing.Size(40, 30);
            this.quickBooksDepositAccountAddButton.TabIndex = 8;
            this.quickBooksDepositAccountAddButton.Click += new System.EventHandler(this.QuickBooksDepositAccountAddButton_Click);
            // 
            // quickBooksDepositAccountsLabel
            // 
            this.quickBooksDepositAccountsLabel.Location = new System.Drawing.Point(6, 200);
            this.quickBooksDepositAccountsLabel.Name = "quickBooksDepositAccountsLabel";
            this.quickBooksDepositAccountsLabel.Size = new System.Drawing.Size(168, 100);
            this.quickBooksDepositAccountsLabel.TabIndex = 6;
            this.quickBooksDepositAccountsLabel.Text = "User will get to pick from this list of accounts to deposit to.";
            this.quickBooksDepositAccountsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // quickBooksDepositInfoLabel
            // 
            this.quickBooksDepositInfoLabel.Location = new System.Drawing.Point(6, 157);
            this.quickBooksDepositInfoLabel.Name = "quickBooksDepositInfoLabel";
            this.quickBooksDepositInfoLabel.Size = new System.Drawing.Size(558, 40);
            this.quickBooksDepositInfoLabel.TabIndex = 5;
            this.quickBooksDepositInfoLabel.Text = "Every time a deposit is created, a deposit will be created within QuickBooks usin" +
    "g these settings.\r\n(Commas must be removed from account names within QuickBooks)" +
    "";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(365, 698);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(481, 698);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            // 
            // accountingTabControl
            // 
            this.accountingTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accountingTabControl.Controls.Add(this.accountAutoPaysTabPage);
            this.accountingTabControl.Controls.Add(this.softlexDentalTabPage);
            this.accountingTabControl.Controls.Add(this.quickBooksTabPage);
            this.accountingTabControl.Location = new System.Drawing.Point(13, 48);
            this.accountingTabControl.Name = "accountingTabControl";
            this.accountingTabControl.SelectedIndex = 0;
            this.accountingTabControl.Size = new System.Drawing.Size(578, 644);
            this.accountingTabControl.TabIndex = 2;
            // 
            // accountAutoPaysTabPage
            // 
            this.accountAutoPaysTabPage.Controls.Add(this.autoPayInfoLabel);
            this.accountAutoPaysTabPage.Controls.Add(this.autoPayGrid);
            this.accountAutoPaysTabPage.Controls.Add(this.autoPayAddButton);
            this.accountAutoPaysTabPage.Controls.Add(this.changeIncomeAccountButton);
            this.accountAutoPaysTabPage.Controls.Add(this.accountLabel);
            this.accountAutoPaysTabPage.Controls.Add(this.accountTextBox);
            this.accountAutoPaysTabPage.Location = new System.Drawing.Point(4, 24);
            this.accountAutoPaysTabPage.Name = "accountAutoPaysTabPage";
            this.accountAutoPaysTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.accountAutoPaysTabPage.Size = new System.Drawing.Size(570, 616);
            this.accountAutoPaysTabPage.TabIndex = 0;
            this.accountAutoPaysTabPage.Text = "Automatic Payment Entries";
            this.accountAutoPaysTabPage.UseVisualStyleBackColor = true;
            // 
            // softlexDentalTabPage
            // 
            this.softlexDentalTabPage.Controls.Add(this.depositAccountsListBox);
            this.softlexDentalTabPage.Controls.Add(this.accountDepositRemoveButton);
            this.softlexDentalTabPage.Controls.Add(this.depositInfoLabel);
            this.softlexDentalTabPage.Controls.Add(this.changeDepositAccountButton);
            this.softlexDentalTabPage.Controls.Add(this.accountDepositLabel);
            this.softlexDentalTabPage.Controls.Add(this.depositAccountLabel);
            this.softlexDentalTabPage.Controls.Add(this.accountDepositAddButton);
            this.softlexDentalTabPage.Controls.Add(this.depositAccountTextBox);
            this.softlexDentalTabPage.Location = new System.Drawing.Point(4, 24);
            this.softlexDentalTabPage.Name = "softlexDentalTabPage";
            this.softlexDentalTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.softlexDentalTabPage.Size = new System.Drawing.Size(570, 616);
            this.softlexDentalTabPage.TabIndex = 1;
            this.softlexDentalTabPage.Text = "Softlex Dental";
            this.softlexDentalTabPage.UseVisualStyleBackColor = true;
            // 
            // quickBooksTabPage
            // 
            this.quickBooksTabPage.Controls.Add(this.quickBooksClassRefRemoveButton);
            this.quickBooksTabPage.Controls.Add(this.quickBooksClassRefAddButton);
            this.quickBooksTabPage.Controls.Add(this.quickBooksTitleLabel);
            this.quickBooksTabPage.Controls.Add(this.quickBooksClassRefsListBox);
            this.quickBooksTabPage.Controls.Add(this.quickBooksCompanyFileTextBox);
            this.quickBooksTabPage.Controls.Add(this.quickBooksClassRefsLabel);
            this.quickBooksTabPage.Controls.Add(this.quickBooksCompanyFileLabel);
            this.quickBooksTabPage.Controls.Add(this.quickBooksClassRefsCheckBox);
            this.quickBooksTabPage.Controls.Add(this.quickBooksWarningLabel);
            this.quickBooksTabPage.Controls.Add(this.quickBooksIncomeAccountRemoveButton);
            this.quickBooksTabPage.Controls.Add(this.browseCompanyFileButton);
            this.quickBooksTabPage.Controls.Add(this.testQuickBooksButton);
            this.quickBooksTabPage.Controls.Add(this.quickBooksIncomeAccountAddButton);
            this.quickBooksTabPage.Controls.Add(this.quickBooksIncomeAccountsListBox);
            this.quickBooksTabPage.Controls.Add(this.quickBooksIncomeAccountsLabel);
            this.quickBooksTabPage.Controls.Add(this.quickBooksDepositInfoLabel);
            this.quickBooksTabPage.Controls.Add(this.quickBooksDepositAccountAddButton);
            this.quickBooksTabPage.Controls.Add(this.quickBooksDepositAccountsLabel);
            this.quickBooksTabPage.Controls.Add(this.quickBooksDepositAccountsListBox);
            this.quickBooksTabPage.Controls.Add(this.quickBooksDepositAccountRemoveButton);
            this.quickBooksTabPage.Location = new System.Drawing.Point(4, 24);
            this.quickBooksTabPage.Name = "quickBooksTabPage";
            this.quickBooksTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.quickBooksTabPage.Size = new System.Drawing.Size(570, 616);
            this.quickBooksTabPage.TabIndex = 2;
            this.quickBooksTabPage.Text = "QuickBooks";
            this.quickBooksTabPage.UseVisualStyleBackColor = true;
            // 
            // softwareComboBox
            // 
            this.softwareComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.softwareComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.softwareComboBox.FormattingEnabled = true;
            this.softwareComboBox.Items.AddRange(new object[] {
            "Softlex Dental",
            "QuickBooks"});
            this.softwareComboBox.Location = new System.Drawing.Point(371, 19);
            this.softwareComboBox.Name = "softwareComboBox";
            this.softwareComboBox.Size = new System.Drawing.Size(220, 23);
            this.softwareComboBox.TabIndex = 1;
            this.softwareComboBox.SelectionChangeCommitted += new System.EventHandler(this.SoftwareComboBox_SelectionChangeCommitted);
            // 
            // FormAccountingSetup
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(604, 741);
            this.Controls.Add(this.softwareComboBox);
            this.Controls.Add(this.accountingTabControl);
            this.Controls.Add(this.softwareLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAccountingSetup";
            this.ShowInTaskbar = false;
            this.Text = "Setup Accounting";
            this.Load += new System.EventHandler(this.FormAccountingSetup_Load);
            this.accountingTabControl.ResumeLayout(false);
            this.accountAutoPaysTabPage.ResumeLayout(false);
            this.accountAutoPaysTabPage.PerformLayout();
            this.softlexDentalTabPage.ResumeLayout(false);
            this.softlexDentalTabPage.PerformLayout();
            this.quickBooksTabPage.ResumeLayout(false);
            this.quickBooksTabPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Label accountDepositLabel;
        private System.Windows.Forms.Button accountDepositAddButton;
        private System.Windows.Forms.Label depositInfoLabel;
        private System.Windows.Forms.Button changeDepositAccountButton;
        private System.Windows.Forms.Label depositAccountLabel;
        private System.Windows.Forms.TextBox depositAccountTextBox;
        private System.Windows.Forms.Button accountDepositRemoveButton;
        private System.Windows.Forms.ListBox depositAccountsListBox;
        private System.Windows.Forms.Button changeIncomeAccountButton;
        private System.Windows.Forms.Label accountLabel;
        private System.Windows.Forms.TextBox accountTextBox;
        private System.Windows.Forms.Label autoPayInfoLabel;
        private System.Windows.Forms.Button autoPayAddButton;
        private OpenDental.UI.ODGrid autoPayGrid;
        private System.Windows.Forms.Label softwareLabel;
        private System.Windows.Forms.Label quickBooksDepositInfoLabel;
        private System.Windows.Forms.ListBox quickBooksDepositAccountsListBox;
        private System.Windows.Forms.Button quickBooksDepositAccountRemoveButton;
        private System.Windows.Forms.Button quickBooksDepositAccountAddButton;
        private System.Windows.Forms.Label quickBooksDepositAccountsLabel;
        private System.Windows.Forms.Button testQuickBooksButton;
        private System.Windows.Forms.Button browseCompanyFileButton;
        private System.Windows.Forms.Label quickBooksCompanyFileLabel;
        private System.Windows.Forms.TextBox quickBooksCompanyFileTextBox;
        private System.Windows.Forms.Label quickBooksWarningLabel;
        private System.Windows.Forms.Label quickBooksIncomeAccountsLabel;
        private System.Windows.Forms.Label quickBooksTitleLabel;
        private System.Windows.Forms.Button quickBooksIncomeAccountRemoveButton;
        private System.Windows.Forms.Button quickBooksIncomeAccountAddButton;
        private System.Windows.Forms.ListBox quickBooksIncomeAccountsListBox;
        private System.Windows.Forms.CheckBox quickBooksClassRefsCheckBox;
        private System.Windows.Forms.Button quickBooksClassRefRemoveButton;
        private System.Windows.Forms.Button quickBooksClassRefAddButton;
        private System.Windows.Forms.ListBox quickBooksClassRefsListBox;
        private System.Windows.Forms.Label quickBooksClassRefsLabel;
        private System.Windows.Forms.TabControl accountingTabControl;
        private System.Windows.Forms.TabPage accountAutoPaysTabPage;
        private System.Windows.Forms.TabPage softlexDentalTabPage;
        private System.Windows.Forms.TabPage quickBooksTabPage;
        private System.Windows.Forms.ComboBox softwareComboBox;
    }
}
