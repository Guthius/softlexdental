namespace OpenDental
{
    partial class FormCarrierCombine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCarrierCombine));
            this.butCancel = new System.Windows.Forms.Button();
            this.butOK = new System.Windows.Forms.Button();
            this.tbCarriers = new OpenDental.Forms.TableCarriers();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(773, 465);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 26);
            this.butCancel.TabIndex = 0;
            this.butCancel.Text = "&Cancel";
            // 
            // butOK
            // 
            this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOK.Location = new System.Drawing.Point(773, 424);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 26);
            this.butOK.TabIndex = 1;
            this.butOK.Text = "&OK";
            this.butOK.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // tbCarriers
            // 
            this.tbCarriers.BackColor = System.Drawing.SystemColors.Window;
            this.tbCarriers.Location = new System.Drawing.Point(9, 42);
            this.tbCarriers.Name = "tbCarriers";
            this.tbCarriers.ScrollValue = 363;
            this.tbCarriers.SelectionMode = System.Windows.Forms.SelectionMode.One;
            this.tbCarriers.Size = new System.Drawing.Size(839, 356);
            this.tbCarriers.TabIndex = 2;
            this.tbCarriers.CellDoubleClicked += new OpenDental.ContrTable.CellEventHandler(this.tbCarriers_CellDoubleClicked);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(476, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Please select the carrier to keep when combining";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // FormCarrierCombine
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(880, 511);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbCarriers);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.butCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCarrierCombine";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Combine Carriers";
            this.Load += new System.EventHandler(this.FormCarrierCombine_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butOK;
        private OpenDental.Forms.TableCarriers tbCarriers;
        private System.Windows.Forms.Label label1;
    }
}
