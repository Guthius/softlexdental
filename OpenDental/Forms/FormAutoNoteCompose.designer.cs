namespace OpenDental
{
    partial class FormAutoNoteCompose
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAutoNoteCompose));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textMain = new System.Windows.Forms.RichTextBox();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.treeListMain = new System.Windows.Forms.TreeView();
            this.imageListTree = new System.Windows.Forms.ImageList(this.components);
            this.insertButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Auto Note";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(382, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Note Text";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textMain
            // 
            this.textMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textMain.Location = new System.Drawing.Point(385, 34);
            this.textMain.Name = "textMain";
            this.textMain.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textMain.Size = new System.Drawing.Size(486, 479);
            this.textMain.TabIndex = 4;
            this.textMain.Text = "";
            this.textMain.TextChanged += new System.EventHandler(this.textMain_TextChanged);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(645, 518);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 5;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Visible = false;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(761, 518);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "&Cancel";
            // 
            // treeListMain
            // 
            this.treeListMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeListMain.HideSelection = false;
            this.treeListMain.ImageIndex = 0;
            this.treeListMain.ImageList = this.imageListTree;
            this.treeListMain.Indent = 12;
            this.treeListMain.Location = new System.Drawing.Point(13, 34);
            this.treeListMain.Name = "treeListMain";
            this.treeListMain.SelectedImageIndex = 0;
            this.treeListMain.Size = new System.Drawing.Size(250, 490);
            this.treeListMain.TabIndex = 1;
            this.treeListMain.DoubleClick += new System.EventHandler(this.treeListMain_DoubleClick);
            // 
            // imageListTree
            // 
            this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
            this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTree.Images.SetKeyName(0, "imageFolder");
            this.imageListTree.Images.SetKeyName(1, "imageText");
            // 
            // insertButton
            // 
            this.insertButton.Location = new System.Drawing.Point(269, 256);
            this.insertButton.Margin = new System.Windows.Forms.Padding(3, 240, 3, 3);
            this.insertButton.Name = "insertButton";
            this.insertButton.Size = new System.Drawing.Size(110, 30);
            this.insertButton.TabIndex = 2;
            this.insertButton.Text = "&Insert";
            this.insertButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.insertButton.Click += new System.EventHandler(this.insertButton_Click);
            // 
            // FormAutoNoteCompose
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.insertButton);
            this.Controls.Add(this.treeListMain);
            this.Controls.Add(this.textMain);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAutoNoteCompose";
            this.Text = "Compose Auto Note";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAutoNoteCompose_FormClosing);
            this.Load += new System.EventHandler(this.FormAutoNoteCompose_Load);
            this.Shown += new System.EventHandler(this.FormAutoNoteCompose_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox textMain;
        private System.Windows.Forms.TreeView treeListMain;
        private System.Windows.Forms.ImageList imageListTree;
        private System.Windows.Forms.Button insertButton;
    }
}