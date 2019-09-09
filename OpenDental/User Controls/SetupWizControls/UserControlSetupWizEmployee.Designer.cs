namespace OpenDental.User_Controls.SetupWizard {
	partial class UserControlSetupWizEmployee {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.employeesGrid = new OpenDental.UI.ODGrid();
            this.addButton = new OpenDental.UI.Button();
            this.advancedButton = new OpenDental.UI.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(166, 55);
            this.label3.TabIndex = 34;
            this.label3.Text = "Items that need attention are highlighted in red.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(695, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 66);
            this.label2.TabIndex = 30;
            this.label2.Text = "Double click a row to set up the specific employee.\r\n";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(694, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 77);
            this.label1.TabIndex = 29;
            this.label1.Text = "Further modifications to this list can be made by going from Lists -> Employees.\r" +
    "\n";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(203, 66);
            this.label8.TabIndex = 27;
            this.label8.Text = "Here is the list of your current employees. Click \'Add\' to add more employees. \r\n" +
    "Each employee must have a First Name and a Last Name.";
            this.label8.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // employeesGrid
            // 
            this.employeesGrid.AllowSortingByColumn = true;
            this.employeesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.employeesGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.employeesGrid.EditableEnterMovesDown = false;
            this.employeesGrid.HasAddButton = false;
            this.employeesGrid.HasDropDowns = false;
            this.employeesGrid.HasMultilineHeaders = false;
            this.employeesGrid.HScrollVisible = true;
            this.employeesGrid.Location = new System.Drawing.Point(224, 19);
            this.employeesGrid.Name = "employeesGrid";
            this.employeesGrid.ScrollValue = 0;
            this.employeesGrid.SelectionMode = OpenDental.UI.GridSelectionMode.Multiple;
            this.employeesGrid.Size = new System.Drawing.Size(460, 472);
            this.employeesGrid.TabIndex = 14;
            this.employeesGrid.Title = "Employees";
            this.employeesGrid.TitleVisible = true;
            this.employeesGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.EmployeesGrid_CellDoubleClick);
            // 
            // addButton
            // 
            this.addButton.AdjustImageLocation = new System.Drawing.Point(0, 0);
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Autosize = true;
            this.addButton.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
            this.addButton.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
            this.addButton.CornerRadius = 4F;
            this.addButton.Image = global::OpenDental.Properties.Resources.Add;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(698, 19);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(82, 26);
            this.addButton.TabIndex = 28;
            this.addButton.Text = "&Add";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // advancedButton
            // 
            this.advancedButton.AdjustImageLocation = new System.Drawing.Point(0, 0);
            this.advancedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.advancedButton.Autosize = true;
            this.advancedButton.BtnShape = OpenDental.UI.EnumType.BtnShape.Rectangle;
            this.advancedButton.BtnStyle = OpenDental.UI.EnumType.XPStyle.Silver;
            this.advancedButton.CornerRadius = 4F;
            this.advancedButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.advancedButton.Location = new System.Drawing.Point(698, 464);
            this.advancedButton.Name = "advancedButton";
            this.advancedButton.Size = new System.Drawing.Size(82, 26);
            this.advancedButton.TabIndex = 38;
            this.advancedButton.Text = "Advanced";
            this.advancedButton.Click += new System.EventHandler(this.AdvancedButton_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(695, 394);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(217, 67);
            this.label4.TabIndex = 37;
            this.label4.Text = "Further modifications to this list can be made by going to Lists -> Employees, or" +
    " clicking \"Advanced\".";
            // 
            // UserControlSetupWizEmployee
            // 
            this.Controls.Add(this.advancedButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.employeesGrid);
            this.Name = "UserControlSetupWizEmployee";
            this.Size = new System.Drawing.Size(930, 530);
            this.Load += new System.EventHandler(this.UserControlSetupWizEmployee_Load);
            this.ResumeLayout(false);

		}

		#endregion
		private UI.ODGrid employeesGrid;
		private System.Windows.Forms.Label label8;
		private UI.Button addButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private UI.Button advancedButton;
		private System.Windows.Forms.Label label4;
	}
}
