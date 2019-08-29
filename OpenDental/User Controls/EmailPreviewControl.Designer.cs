namespace OpenDental
{
    partial class EmailPreviewControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textMsgDateTime = new System.Windows.Forms.TextBox();
            this.labelSent = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.contextMenuAttachments = new System.Windows.Forms.ContextMenu();
            this.menuItemOpen = new System.Windows.Forms.MenuItem();
            this.menuItemRename = new System.Windows.Forms.MenuItem();
            this.menuItemRemove = new System.Windows.Forms.MenuItem();
            this.textSentOrReceived = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lableUserName = new System.Windows.Forms.Label();
            this.textUserName = new System.Windows.Forms.TextBox();
            this.gridAttachments = new OpenDental.UI.ODGrid();
            this.butAccountPicker = new System.Windows.Forms.Button();
            this.textBccAddress = new System.Windows.Forms.TextBox();
            this.textCcAddress = new System.Windows.Forms.TextBox();
            this.textSubject = new System.Windows.Forms.TextBox();
            this.textToAddress = new System.Windows.Forms.TextBox();
            this.textFromAddress = new System.Windows.Forms.TextBox();
            this.textBodyText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textMsgDateTime
            // 
            this.textMsgDateTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textMsgDateTime.BackColor = System.Drawing.SystemColors.Control;
            this.textMsgDateTime.ForeColor = System.Drawing.Color.Red;
            this.textMsgDateTime.Location = new System.Drawing.Point(104, 61);
            this.textMsgDateTime.Name = "textMsgDateTime";
            this.textMsgDateTime.Size = new System.Drawing.Size(347, 23);
            this.textMsgDateTime.TabIndex = 0;
            this.textMsgDateTime.TabStop = false;
            this.textMsgDateTime.Text = "Unsent";
            // 
            // labelSent
            // 
            this.labelSent.AutoSize = true;
            this.labelSent.Location = new System.Drawing.Point(30, 64);
            this.labelSent.Name = "labelSent";
            this.labelSent.Size = new System.Drawing.Size(68, 15);
            this.labelSent.TabIndex = 0;
            this.labelSent.Text = "Date / Time";
            this.labelSent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "From";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(79, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "To";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 209);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Subject";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // contextMenuAttachments
            // 
            this.contextMenuAttachments.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemOpen,
            this.menuItemRename,
            this.menuItemRemove});
            this.contextMenuAttachments.Popup += new System.EventHandler(this.contextMenuAttachments_Popup);
            // 
            // menuItemOpen
            // 
            this.menuItemOpen.Index = 0;
            this.menuItemOpen.Text = "Open";
            this.menuItemOpen.Click += new System.EventHandler(this.OpenMenuItem_Click);
            // 
            // menuItemRename
            // 
            this.menuItemRename.Index = 1;
            this.menuItemRename.Text = "Rename";
            this.menuItemRename.Click += new System.EventHandler(this.RenameMenuItem_Click);
            // 
            // menuItemRemove
            // 
            this.menuItemRemove.Index = 2;
            this.menuItemRemove.Text = "Remove";
            this.menuItemRemove.Click += new System.EventHandler(this.RemoveMenuItem_Click);
            // 
            // textSentOrReceived
            // 
            this.textSentOrReceived.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSentOrReceived.BackColor = System.Drawing.SystemColors.Control;
            this.textSentOrReceived.Location = new System.Drawing.Point(104, 32);
            this.textSentOrReceived.Name = "textSentOrReceived";
            this.textSentOrReceived.Size = new System.Drawing.Size(347, 23);
            this.textSentOrReceived.TabIndex = 0;
            this.textSentOrReceived.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Sent/Received";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(75, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "CC";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(68, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "BCC";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lableUserName
            // 
            this.lableUserName.AutoSize = true;
            this.lableUserName.Location = new System.Drawing.Point(38, 6);
            this.lableUserName.Name = "lableUserName";
            this.lableUserName.Size = new System.Drawing.Size(60, 15);
            this.lableUserName.TabIndex = 11;
            this.lableUserName.Text = "Username";
            this.lableUserName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textUserName
            // 
            this.textUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textUserName.BackColor = System.Drawing.SystemColors.Control;
            this.textUserName.Location = new System.Drawing.Point(104, 3);
            this.textUserName.Name = "textUserName";
            this.textUserName.Size = new System.Drawing.Size(347, 23);
            this.textUserName.TabIndex = 12;
            this.textUserName.TabStop = false;
            this.textUserName.Text = "userName";
            // 
            // gridAttachments
            // 
            this.gridAttachments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gridAttachments.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.gridAttachments.EditableEnterMovesDown = false;
            this.gridAttachments.HasAddButton = true;
            this.gridAttachments.HasDropDowns = false;
            this.gridAttachments.HasMultilineHeaders = false;
            this.gridAttachments.HScrollVisible = false;
            this.gridAttachments.Location = new System.Drawing.Point(457, 3);
            this.gridAttachments.Name = "gridAttachments";
            this.gridAttachments.ScrollValue = 0;
            this.gridAttachments.Size = new System.Drawing.Size(220, 226);
            this.gridAttachments.TabIndex = 0;
            this.gridAttachments.TabStop = false;
            this.gridAttachments.Title = "Attachments";
            this.gridAttachments.TitleVisible = true;
            this.gridAttachments.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.gridAttachments_CellDoubleClick);
            this.gridAttachments.TitleAddClick += new System.EventHandler(this.gridAttachmentsAdd_Click);
            this.gridAttachments.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridAttachments_MouseDown);
            // 
            // butAccountPicker
            // 
            this.butAccountPicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butAccountPicker.Location = new System.Drawing.Point(421, 89);
            this.butAccountPicker.Name = "butAccountPicker";
            this.butAccountPicker.Size = new System.Drawing.Size(30, 25);
            this.butAccountPicker.TabIndex = 14;
            this.butAccountPicker.Text = "...";
            this.butAccountPicker.Click += new System.EventHandler(this.butAccountPicker_Click);
            // 
            // textBccAddress
            // 
            this.textBccAddress.AcceptsTab = true;
            this.textBccAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBccAddress.BackColor = System.Drawing.SystemColors.Window;
            this.textBccAddress.Location = new System.Drawing.Point(104, 177);
            this.textBccAddress.Name = "textBccAddress";
            this.textBccAddress.Size = new System.Drawing.Size(347, 23);
            this.textBccAddress.TabIndex = 5;
            this.textBccAddress.WordWrap = false;
            // 
            // textCcAddress
            // 
            this.textCcAddress.AcceptsTab = true;
            this.textCcAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textCcAddress.BackColor = System.Drawing.SystemColors.Window;
            this.textCcAddress.Location = new System.Drawing.Point(104, 148);
            this.textCcAddress.Name = "textCcAddress";
            this.textCcAddress.Size = new System.Drawing.Size(347, 23);
            this.textCcAddress.TabIndex = 4;
            this.textCcAddress.WordWrap = false;
            // 
            // textSubject
            // 
            this.textSubject.AcceptsTab = true;
            this.textSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSubject.BackColor = System.Drawing.SystemColors.Window;
            this.textSubject.Location = new System.Drawing.Point(104, 206);
            this.textSubject.Name = "textSubject";
            this.textSubject.Size = new System.Drawing.Size(347, 23);
            this.textSubject.TabIndex = 7;
            this.textSubject.WordWrap = false;
            // 
            // textToAddress
            // 
            this.textToAddress.AcceptsTab = true;
            this.textToAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textToAddress.BackColor = System.Drawing.SystemColors.Window;
            this.textToAddress.Location = new System.Drawing.Point(104, 119);
            this.textToAddress.Name = "textToAddress";
            this.textToAddress.Size = new System.Drawing.Size(347, 23);
            this.textToAddress.TabIndex = 3;
            this.textToAddress.WordWrap = false;
            // 
            // textFromAddress
            // 
            this.textFromAddress.AcceptsTab = true;
            this.textFromAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textFromAddress.BackColor = System.Drawing.SystemColors.Window;
            this.textFromAddress.Location = new System.Drawing.Point(104, 90);
            this.textFromAddress.Name = "textFromAddress";
            this.textFromAddress.Size = new System.Drawing.Size(311, 23);
            this.textFromAddress.TabIndex = 1;
            this.textFromAddress.WordWrap = false;
            // 
            // textBodyText
            // 
            this.textBodyText.AcceptsReturn = true;
            this.textBodyText.AcceptsTab = true;
            this.textBodyText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBodyText.BackColor = System.Drawing.SystemColors.Window;
            this.textBodyText.Location = new System.Drawing.Point(104, 235);
            this.textBodyText.Multiline = true;
            this.textBodyText.Name = "textBodyText";
            this.textBodyText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBodyText.Size = new System.Drawing.Size(573, 162);
            this.textBodyText.TabIndex = 8;
            this.textBodyText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBodyText_KeyDown);
            // 
            // EmailPreviewControl
            // 
            this.Controls.Add(this.gridAttachments);
            this.Controls.Add(this.butAccountPicker);
            this.Controls.Add(this.textUserName);
            this.Controls.Add(this.lableUserName);
            this.Controls.Add(this.textBccAddress);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textCcAddress);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textSentOrReceived);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textSubject);
            this.Controls.Add(this.textToAddress);
            this.Controls.Add(this.textFromAddress);
            this.Controls.Add(this.textBodyText);
            this.Controls.Add(this.textMsgDateTime);
            this.Controls.Add(this.labelSent);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "EmailPreviewControl";
            this.Size = new System.Drawing.Size(680, 400);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenu contextMenuAttachments;
        private System.Windows.Forms.MenuItem menuItemOpen;
        private System.Windows.Forms.MenuItem menuItemRename;
        private System.Windows.Forms.MenuItem menuItemRemove;
        private UI.ODGrid gridAttachments;
        private System.Windows.Forms.TextBox textFromAddress;
        private System.Windows.Forms.TextBox textToAddress;
        private System.Windows.Forms.TextBox textSubject;
        private System.Windows.Forms.TextBox textBodyText;
        private System.Windows.Forms.TextBox textMsgDateTime;
        private System.Windows.Forms.Label labelSent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textSentOrReceived;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textCcAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBccAddress;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lableUserName;
        private System.Windows.Forms.TextBox textUserName;
        private System.Windows.Forms.Button butAccountPicker;
    }
}