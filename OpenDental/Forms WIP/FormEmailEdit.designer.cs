namespace OpenDental
{
    partial class FormEmailEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailEdit));
            this.bodyTextBox = new System.Windows.Forms.TextBox();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.setupButton = new System.Windows.Forms.Button();
            this.externalLinkButton = new System.Windows.Forms.Button();
            this.heading1Button = new System.Windows.Forms.Button();
            this.heading2Button = new System.Windows.Forms.Button();
            this.heading3Button = new System.Windows.Forms.Button();
            this.pasteButton = new System.Windows.Forms.Button();
            this.copyButton = new System.Windows.Forms.Button();
            this.cutButton = new System.Windows.Forms.Button();
            this.undoButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bodyTextBox
            // 
            this.bodyTextBox.AcceptsReturn = true;
            this.bodyTextBox.AcceptsTab = true;
            this.bodyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bodyTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bodyTextBox.Location = new System.Drawing.Point(13, 55);
            this.bodyTextBox.Multiline = true;
            this.bodyTextBox.Name = "bodyTextBox";
            this.bodyTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.bodyTextBox.Size = new System.Drawing.Size(758, 497);
            this.bodyTextBox.TabIndex = 86;
            this.bodyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BodyTextBox_KeyPress);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.acceptButton.Location = new System.Drawing.Point(545, 558);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 87;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(661, 558);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 86;
            this.cancelButton.Text = "&Cancel";
            // 
            // setupButton
            // 
            this.setupButton.Image = global::OpenDental.Properties.Resources.IconCog;
            this.setupButton.Location = new System.Drawing.Point(13, 19);
            this.setupButton.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.setupButton.Name = "setupButton";
            this.setupButton.Size = new System.Drawing.Size(35, 30);
            this.setupButton.TabIndex = 90;
            this.setupButton.UseVisualStyleBackColor = true;
            this.setupButton.Click += new System.EventHandler(this.SetupButton_Click);
            // 
            // externalLinkButton
            // 
            this.externalLinkButton.Image = global::OpenDental.Properties.Resources.IconLinkExternal;
            this.externalLinkButton.Location = new System.Drawing.Point(71, 19);
            this.externalLinkButton.Name = "externalLinkButton";
            this.externalLinkButton.Size = new System.Drawing.Size(35, 30);
            this.externalLinkButton.TabIndex = 91;
            this.externalLinkButton.UseVisualStyleBackColor = true;
            this.externalLinkButton.Click += new System.EventHandler(this.ExternalLinkButton_Click);
            // 
            // heading1Button
            // 
            this.heading1Button.Image = global::OpenDental.Properties.Resources.IconTextHeading1;
            this.heading1Button.Location = new System.Drawing.Point(112, 19);
            this.heading1Button.Name = "heading1Button";
            this.heading1Button.Size = new System.Drawing.Size(35, 30);
            this.heading1Button.TabIndex = 92;
            this.heading1Button.UseVisualStyleBackColor = true;
            this.heading1Button.Click += new System.EventHandler(this.Heading1Button_Click);
            // 
            // heading2Button
            // 
            this.heading2Button.Image = global::OpenDental.Properties.Resources.IconTextHeading2;
            this.heading2Button.Location = new System.Drawing.Point(153, 19);
            this.heading2Button.Name = "heading2Button";
            this.heading2Button.Size = new System.Drawing.Size(35, 30);
            this.heading2Button.TabIndex = 93;
            this.heading2Button.UseVisualStyleBackColor = true;
            this.heading2Button.Click += new System.EventHandler(this.Heading2Button_Click);
            // 
            // heading3Button
            // 
            this.heading3Button.Image = global::OpenDental.Properties.Resources.IconTextHeading3;
            this.heading3Button.Location = new System.Drawing.Point(194, 19);
            this.heading3Button.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.heading3Button.Name = "heading3Button";
            this.heading3Button.Size = new System.Drawing.Size(35, 30);
            this.heading3Button.TabIndex = 94;
            this.heading3Button.UseVisualStyleBackColor = true;
            this.heading3Button.Click += new System.EventHandler(this.Heading3Button_Click);
            // 
            // pasteButton
            // 
            this.pasteButton.Image = global::OpenDental.Properties.Resources.IconPaste;
            this.pasteButton.Location = new System.Drawing.Point(334, 19);
            this.pasteButton.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.pasteButton.Name = "pasteButton";
            this.pasteButton.Size = new System.Drawing.Size(35, 30);
            this.pasteButton.TabIndex = 101;
            this.pasteButton.UseVisualStyleBackColor = true;
            this.pasteButton.Click += new System.EventHandler(this.PasteButton_Click);
            // 
            // copyButton
            // 
            this.copyButton.Image = global::OpenDental.Properties.Resources.IconCopy;
            this.copyButton.Location = new System.Drawing.Point(293, 19);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(35, 30);
            this.copyButton.TabIndex = 100;
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // cutButton
            // 
            this.cutButton.Image = global::OpenDental.Properties.Resources.IconCut;
            this.cutButton.Location = new System.Drawing.Point(252, 19);
            this.cutButton.Name = "cutButton";
            this.cutButton.Size = new System.Drawing.Size(35, 30);
            this.cutButton.TabIndex = 99;
            this.cutButton.UseVisualStyleBackColor = true;
            this.cutButton.Click += new System.EventHandler(this.CutButton_Click);
            // 
            // undoButton
            // 
            this.undoButton.Image = global::OpenDental.Properties.Resources.IconUndo;
            this.undoButton.Location = new System.Drawing.Point(392, 19);
            this.undoButton.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.undoButton.Name = "undoButton";
            this.undoButton.Size = new System.Drawing.Size(35, 30);
            this.undoButton.TabIndex = 102;
            this.undoButton.UseVisualStyleBackColor = true;
            this.undoButton.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // FormEmailEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(784, 601);
            this.Controls.Add(this.bodyTextBox);
            this.Controls.Add(this.undoButton);
            this.Controls.Add(this.pasteButton);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.cutButton);
            this.Controls.Add(this.heading3Button);
            this.Controls.Add(this.heading2Button);
            this.Controls.Add(this.heading1Button);
            this.Controls.Add(this.externalLinkButton);
            this.Controls.Add(this.setupButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormEmailEdit";
            this.Text = "E-mail Editor";
            this.Load += new System.EventHandler(this.FormEmailEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.TextBox bodyTextBox;
        private System.Windows.Forms.Button setupButton;
        private System.Windows.Forms.Button externalLinkButton;
        private System.Windows.Forms.Button heading1Button;
        private System.Windows.Forms.Button heading2Button;
        private System.Windows.Forms.Button heading3Button;
        private System.Windows.Forms.Button pasteButton;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.Button cutButton;
        private System.Windows.Forms.Button undoButton;
    }
}