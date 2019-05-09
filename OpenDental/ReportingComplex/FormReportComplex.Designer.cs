namespace OpenDental.ReportingComplex
{
    partial class FormReportComplex
    {
        private System.ComponentModel.IContainer components;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReportComplex));
            this.setupDialog2 = new System.Windows.Forms.PageSetupDialog();
            this.imageListMain = new System.Windows.Forms.ImageList(this.components);
            this.printPreviewControl2 = new System.Windows.Forms.PrintPreviewControl();
            this.ToolBarMain = new OpenDental.UI.ODToolBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.butClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListMain
            // 
            this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
            this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMain.Images.SetKeyName(0, "");
            this.imageListMain.Images.SetKeyName(1, "");
            this.imageListMain.Images.SetKeyName(2, "");
            this.imageListMain.Images.SetKeyName(3, "");
            this.imageListMain.Images.SetKeyName(4, "butZoomIn.gif");
            this.imageListMain.Images.SetKeyName(5, "butZoomOut.gif");
            // 
            // printPreviewControl2
            // 
            this.printPreviewControl2.AutoZoom = false;
            this.printPreviewControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printPreviewControl2.Location = new System.Drawing.Point(0, 0);
            this.printPreviewControl2.Name = "printPreviewControl2";
            this.printPreviewControl2.Size = new System.Drawing.Size(831, 553);
            this.printPreviewControl2.TabIndex = 6;
            // 
            // ToolBarMain
            // 
            this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.ToolBarMain.Location = new System.Drawing.Point(0, 0);
            this.ToolBarMain.Name = "ToolBarMain";
            this.ToolBarMain.Size = new System.Drawing.Size(831, 30);
            this.ToolBarMain.TabIndex = 5;
            this.ToolBarMain.ButtonClick += new System.EventHandler<OpenDental.UI.ODToolBarButtonClickEventArgs>(this.ToolBarMain_ButtonClick);
            this.ToolBarMain.PageNav += new System.EventHandler<OpenDental.UI.ODToolBarButtonPageNavEventArgs>(this.ToolBarMain_PageNav);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 30);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.printPreviewControl2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.butClose);
            this.splitContainer1.Size = new System.Drawing.Size(831, 586);
            this.splitContainer1.SplitterDistance = 553;
            this.splitContainer1.TabIndex = 7;
            // 
            // butClose
            // 
            this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butClose.Location = new System.Drawing.Point(753, 3);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(75, 23);
            this.butClose.TabIndex = 0;
            this.butClose.Text = "Close";
            this.butClose.UseVisualStyleBackColor = true;
            this.butClose.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // FormReportComplex
            // 
            this.CancelButton = this.butClose;
            this.ClientSize = new System.Drawing.Size(831, 616);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.ToolBarMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormReportComplex";
            this.Text = "Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormReport_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.FormReport_Layout);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.PageSetupDialog setupDialog2;
        private OpenDental.UI.ODToolBar ToolBarMain;
        private System.Windows.Forms.ImageList imageListMain;
        private System.Windows.Forms.PrintPreviewControl printPreviewControl2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button butClose;
    }
}