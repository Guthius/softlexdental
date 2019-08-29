namespace OpenDental
{
    partial class FormEmailMessageEdit
    {
        private System.ComponentModel.IContainer components;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailMessageEdit));
            this.templatesListBox = new System.Windows.Forms.ListBox();
            this.templateEditButton = new System.Windows.Forms.Button();
            this.templateInsertButton = new System.Windows.Forms.Button();
            this.templateDeleteButton = new System.Windows.Forms.Button();
            this.templateAddButton = new System.Windows.Forms.Button();
            this.butEditAutograph = new System.Windows.Forms.Button();
            this.butInsertAutograph = new System.Windows.Forms.Button();
            this.butDeleteAutograph = new System.Windows.Forms.Button();
            this.butAddAutograph = new System.Windows.Forms.Button();
            this.listAutographs = new System.Windows.Forms.ListBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.sendButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.emailPreview = new OpenDental.EmailPreviewControl();
            this.editHtmlButton = new System.Windows.Forms.Button();
            this.editTextButton = new System.Windows.Forms.Button();
            this.templatesGroupBox = new System.Windows.Forms.GroupBox();
            this.autographsGroupBox = new System.Windows.Forms.GroupBox();
            this.draftSplitContainer = new System.Windows.Forms.SplitContainer();
            this.templatesGroupBox.SuspendLayout();
            this.autographsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.draftSplitContainer)).BeginInit();
            this.draftSplitContainer.Panel1.SuspendLayout();
            this.draftSplitContainer.Panel2.SuspendLayout();
            this.draftSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // templatesListBox
            // 
            this.templatesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.templatesListBox.HorizontalScrollbar = true;
            this.templatesListBox.IntegralHeight = false;
            this.templatesListBox.ItemHeight = 15;
            this.templatesListBox.Location = new System.Drawing.Point(6, 22);
            this.templatesListBox.Name = "templatesListBox";
            this.templatesListBox.Size = new System.Drawing.Size(188, 186);
            this.templatesListBox.TabIndex = 0;
            this.templatesListBox.TabStop = false;
            this.templatesListBox.DoubleClick += new System.EventHandler(this.TemplatesListBox_DoubleClick);
            // 
            // templateEditButton
            // 
            this.templateEditButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.templateEditButton.Image = ((System.Drawing.Image)(resources.GetObject("templateEditButton.Image")));
            this.templateEditButton.Location = new System.Drawing.Point(47, 214);
            this.templateEditButton.Name = "templateEditButton";
            this.templateEditButton.Size = new System.Drawing.Size(35, 30);
            this.templateEditButton.TabIndex = 19;
            this.templateEditButton.Click += new System.EventHandler(this.TemplateEditButton_Click);
            // 
            // templateInsertButton
            // 
            this.templateInsertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.templateInsertButton.Image = global::OpenDental.Properties.Resources.Right;
            this.templateInsertButton.Location = new System.Drawing.Point(159, 214);
            this.templateInsertButton.Name = "templateInsertButton";
            this.templateInsertButton.Size = new System.Drawing.Size(35, 30);
            this.templateInsertButton.TabIndex = 2;
            this.templateInsertButton.Click += new System.EventHandler(this.TemplateInsertButton_Click);
            // 
            // templateDeleteButton
            // 
            this.templateDeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.templateDeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("templateDeleteButton.Image")));
            this.templateDeleteButton.Location = new System.Drawing.Point(88, 214);
            this.templateDeleteButton.Name = "templateDeleteButton";
            this.templateDeleteButton.Size = new System.Drawing.Size(35, 30);
            this.templateDeleteButton.TabIndex = 3;
            this.templateDeleteButton.Click += new System.EventHandler(this.TemplateDeleteButton_Click);
            // 
            // templateAddButton
            // 
            this.templateAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.templateAddButton.Image = ((System.Drawing.Image)(resources.GetObject("templateAddButton.Image")));
            this.templateAddButton.Location = new System.Drawing.Point(6, 214);
            this.templateAddButton.Name = "templateAddButton";
            this.templateAddButton.Size = new System.Drawing.Size(35, 30);
            this.templateAddButton.TabIndex = 1;
            this.templateAddButton.Click += new System.EventHandler(this.TemplateAddButton_Click);
            // 
            // butEditAutograph
            // 
            this.butEditAutograph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butEditAutograph.Image = ((System.Drawing.Image)(resources.GetObject("butEditAutograph.Image")));
            this.butEditAutograph.Location = new System.Drawing.Point(47, 214);
            this.butEditAutograph.Name = "butEditAutograph";
            this.butEditAutograph.Size = new System.Drawing.Size(35, 30);
            this.butEditAutograph.TabIndex = 20;
            this.butEditAutograph.Click += new System.EventHandler(this.EditAutographButton_Click);
            // 
            // butInsertAutograph
            // 
            this.butInsertAutograph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butInsertAutograph.Image = global::OpenDental.Properties.Resources.Right;
            this.butInsertAutograph.Location = new System.Drawing.Point(159, 214);
            this.butInsertAutograph.Name = "butInsertAutograph";
            this.butInsertAutograph.Size = new System.Drawing.Size(35, 30);
            this.butInsertAutograph.TabIndex = 2;
            this.butInsertAutograph.Click += new System.EventHandler(this.InsertAutographButton_Click);
            // 
            // butDeleteAutograph
            // 
            this.butDeleteAutograph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butDeleteAutograph.Image = ((System.Drawing.Image)(resources.GetObject("butDeleteAutograph.Image")));
            this.butDeleteAutograph.Location = new System.Drawing.Point(88, 214);
            this.butDeleteAutograph.Name = "butDeleteAutograph";
            this.butDeleteAutograph.Size = new System.Drawing.Size(35, 30);
            this.butDeleteAutograph.TabIndex = 3;
            this.butDeleteAutograph.Click += new System.EventHandler(this.DeleteAutographButton_Click);
            // 
            // butAddAutograph
            // 
            this.butAddAutograph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butAddAutograph.Image = ((System.Drawing.Image)(resources.GetObject("butAddAutograph.Image")));
            this.butAddAutograph.Location = new System.Drawing.Point(6, 214);
            this.butAddAutograph.Name = "butAddAutograph";
            this.butAddAutograph.Size = new System.Drawing.Size(35, 30);
            this.butAddAutograph.TabIndex = 1;
            this.butAddAutograph.Click += new System.EventHandler(this.AddAutographButton_Click);
            // 
            // listAutographs
            // 
            this.listAutographs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listAutographs.IntegralHeight = false;
            this.listAutographs.ItemHeight = 15;
            this.listAutographs.Location = new System.Drawing.Point(6, 22);
            this.listAutographs.Name = "listAutographs";
            this.listAutographs.Size = new System.Drawing.Size(188, 186);
            this.listAutographs.TabIndex = 0;
            this.listAutographs.TabStop = false;
            this.listAutographs.DoubleClick += new System.EventHandler(this.listAutographs_DoubleClick);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::OpenDental.Properties.Resources.IconDelete;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deleteButton.Location = new System.Drawing.Point(13, 638);
            this.deleteButton.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(110, 30);
            this.deleteButton.TabIndex = 11;
            this.deleteButton.Text = "Delete";
            this.deleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveButton.Location = new System.Drawing.Point(156, 638);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(110, 30);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Save";
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // sendButton
            // 
            this.sendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sendButton.Location = new System.Drawing.Point(745, 638);
            this.sendButton.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(110, 30);
            this.sendButton.TabIndex = 9;
            this.sendButton.Text = "&Send";
            this.sendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(861, 638);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // emailPreview
            // 
            this.emailPreview.BccAddress = "";
            this.emailPreview.Body = "";
            this.emailPreview.CcAddress = "";
            this.emailPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emailPreview.Location = new System.Drawing.Point(0, 0);
            this.emailPreview.Name = "emailPreview";
            this.emailPreview.Size = new System.Drawing.Size(754, 613);
            this.emailPreview.Subject = "";
            this.emailPreview.TabIndex = 38;
            this.emailPreview.ToAddress = "";
            // 
            // editHtmlButton
            // 
            this.editHtmlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editHtmlButton.Location = new System.Drawing.Point(388, 638);
            this.editHtmlButton.Name = "editHtmlButton";
            this.editHtmlButton.Size = new System.Drawing.Size(110, 30);
            this.editHtmlButton.TabIndex = 39;
            this.editHtmlButton.Text = "Edit HTML";
            this.editHtmlButton.Click += new System.EventHandler(this.butEditHtml_Click);
            // 
            // editTextButton
            // 
            this.editTextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editTextButton.Location = new System.Drawing.Point(272, 638);
            this.editTextButton.Name = "editTextButton";
            this.editTextButton.Size = new System.Drawing.Size(110, 30);
            this.editTextButton.TabIndex = 40;
            this.editTextButton.Text = "Edit Text";
            // 
            // templatesGroupBox
            // 
            this.templatesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.templatesGroupBox.Controls.Add(this.templatesListBox);
            this.templatesGroupBox.Controls.Add(this.templateAddButton);
            this.templatesGroupBox.Controls.Add(this.templateDeleteButton);
            this.templatesGroupBox.Controls.Add(this.templateInsertButton);
            this.templatesGroupBox.Controls.Add(this.templateEditButton);
            this.templatesGroupBox.Location = new System.Drawing.Point(0, 0);
            this.templatesGroupBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.templatesGroupBox.Name = "templatesGroupBox";
            this.templatesGroupBox.Size = new System.Drawing.Size(200, 250);
            this.templatesGroupBox.TabIndex = 41;
            this.templatesGroupBox.TabStop = false;
            this.templatesGroupBox.Text = "E-mail Templates";
            // 
            // autographsGroupBox
            // 
            this.autographsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autographsGroupBox.Controls.Add(this.butEditAutograph);
            this.autographsGroupBox.Controls.Add(this.butInsertAutograph);
            this.autographsGroupBox.Controls.Add(this.butAddAutograph);
            this.autographsGroupBox.Controls.Add(this.butDeleteAutograph);
            this.autographsGroupBox.Controls.Add(this.listAutographs);
            this.autographsGroupBox.Location = new System.Drawing.Point(0, 255);
            this.autographsGroupBox.Margin = new System.Windows.Forms.Padding(0);
            this.autographsGroupBox.Name = "autographsGroupBox";
            this.autographsGroupBox.Size = new System.Drawing.Size(200, 250);
            this.autographsGroupBox.TabIndex = 42;
            this.autographsGroupBox.TabStop = false;
            this.autographsGroupBox.Text = "E-mail Autograph";
            // 
            // draftSplitContainer
            // 
            this.draftSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.draftSplitContainer.Location = new System.Drawing.Point(13, 19);
            this.draftSplitContainer.Name = "draftSplitContainer";
            // 
            // draftSplitContainer.Panel1
            // 
            this.draftSplitContainer.Panel1.Controls.Add(this.templatesGroupBox);
            this.draftSplitContainer.Panel1.Controls.Add(this.autographsGroupBox);
            this.draftSplitContainer.Panel1MinSize = 200;
            // 
            // draftSplitContainer.Panel2
            // 
            this.draftSplitContainer.Panel2.Controls.Add(this.emailPreview);
            this.draftSplitContainer.Size = new System.Drawing.Size(958, 613);
            this.draftSplitContainer.SplitterDistance = 200;
            this.draftSplitContainer.TabIndex = 43;
            // 
            // FormEmailMessageEdit
            // 
            this.ClientSize = new System.Drawing.Size(984, 681);
            this.Controls.Add(this.draftSplitContainer);
            this.Controls.Add(this.editTextButton);
            this.Controls.Add(this.editHtmlButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(900, 720);
            this.Name = "FormEmailMessageEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mail Message";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEmailMessageEdit_FormClosing);
            this.Load += new System.EventHandler(this.FormEmailMessageEdit_Load);
            this.templatesGroupBox.ResumeLayout(false);
            this.autographsGroupBox.ResumeLayout(false);
            this.draftSplitContainer.Panel1.ResumeLayout(false);
            this.draftSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.draftSplitContainer)).EndInit();
            this.draftSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion


        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.Button templateDeleteButton;
        private System.Windows.Forms.Button templateAddButton;
        private System.Windows.Forms.ListBox templatesListBox;
        private System.Windows.Forms.Button templateInsertButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button butInsertAutograph;
        private System.Windows.Forms.Button butDeleteAutograph;
        private System.Windows.Forms.Button butAddAutograph;
        private System.Windows.Forms.ListBox listAutographs;
        private System.Windows.Forms.Button templateEditButton;
        private System.Windows.Forms.Button butEditAutograph;
        private EmailPreviewControl emailPreview;
        private System.Windows.Forms.Button editHtmlButton;
        private System.Windows.Forms.Button editTextButton;
        private System.Windows.Forms.GroupBox templatesGroupBox;
        private System.Windows.Forms.GroupBox autographsGroupBox;
        private System.Windows.Forms.SplitContainer draftSplitContainer;
    }
}