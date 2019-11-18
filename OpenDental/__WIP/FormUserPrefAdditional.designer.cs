namespace OpenDental
{
    partial class FormUserPrefAdditional
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUserPrefAdditional));
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.userPropertiesGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(145, 518);
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
            this.cancelButton.Location = new System.Drawing.Point(261, 518);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            // 
            // userPropertiesGrid
            // 
            this.userPropertiesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userPropertiesGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.userPropertiesGrid.EditableEnterMovesDown = false;
            this.userPropertiesGrid.HasAddButton = false;
            this.userPropertiesGrid.HasDropDowns = false;
            this.userPropertiesGrid.HasMultilineHeaders = false;
            this.userPropertiesGrid.HScrollVisible = false;
            this.userPropertiesGrid.Location = new System.Drawing.Point(13, 19);
            this.userPropertiesGrid.Name = "userPropertiesGrid";
            this.userPropertiesGrid.ScrollValue = 0;
            this.userPropertiesGrid.SelectionMode = OpenDental.UI.GridSelectionMode.Cell;
            this.userPropertiesGrid.Size = new System.Drawing.Size(358, 493);
            this.userPropertiesGrid.TabIndex = 4;
            this.userPropertiesGrid.Title = "Additional User Properties";
            this.userPropertiesGrid.TitleVisible = true;
            this.userPropertiesGrid.CellLeave += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.UserPropertiesGrid_CellLeave);
            // 
            // FormUserPrefAdditional
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(384, 561);
            this.Controls.Add(this.userPropertiesGrid);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(280, 360);
            this.Name = "FormUserPrefAdditional";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Additional User Properties";
            this.Load += new System.EventHandler(this.FormProvAdditional_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private OpenDental.UI.ODGrid userPropertiesGrid;
    }
}
