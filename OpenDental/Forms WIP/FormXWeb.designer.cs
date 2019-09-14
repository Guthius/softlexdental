namespace OpenDental
{
    partial class FormXWeb
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormXWeb));
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.labelRefNumber = new System.Windows.Forms.Label();
            this.textRefNumber = new System.Windows.Forms.TextBox();
            this.groupTransType = new System.Windows.Forms.GroupBox();
            this.radioReturn = new System.Windows.Forms.RadioButton();
            this.textZipCode = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textSecurityCode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textNameOnCard = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textExpDate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textCardNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textAmount = new System.Windows.Forms.TextBox();
            this.labelAmount = new System.Windows.Forms.Label();
            this.textPayNote = new OpenDental.ODtextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupTransType.SuspendLayout();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(185, 318);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 30);
            this.acceptButton.TabIndex = 17;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(301, 318);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 18;
            this.cancelButton.Text = "&Cancel";
            // 
            // labelRefNumber
            // 
            this.labelRefNumber.AutoSize = true;
            this.labelRefNumber.Location = new System.Drawing.Point(277, 28);
            this.labelRefNumber.Name = "labelRefNumber";
            this.labelRefNumber.Size = new System.Drawing.Size(71, 15);
            this.labelRefNumber.TabIndex = 7;
            this.labelRefNumber.Text = "Ref Number";
            this.labelRefNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelRefNumber.Visible = false;
            // 
            // textRefNumber
            // 
            this.textRefNumber.Location = new System.Drawing.Point(280, 46);
            this.textRefNumber.Name = "textRefNumber";
            this.textRefNumber.Size = new System.Drawing.Size(120, 23);
            this.textRefNumber.TabIndex = 8;
            this.textRefNumber.Visible = false;
            // 
            // groupTransType
            // 
            this.groupTransType.Controls.Add(this.radioReturn);
            this.groupTransType.Location = new System.Drawing.Point(13, 19);
            this.groupTransType.Name = "groupTransType";
            this.groupTransType.Size = new System.Drawing.Size(150, 50);
            this.groupTransType.TabIndex = 0;
            this.groupTransType.TabStop = false;
            this.groupTransType.Text = "Transaction Type";
            // 
            // radioReturn
            // 
            this.radioReturn.AutoSize = true;
            this.radioReturn.Location = new System.Drawing.Point(17, 19);
            this.radioReturn.Name = "radioReturn";
            this.radioReturn.Size = new System.Drawing.Size(60, 19);
            this.radioReturn.TabIndex = 0;
            this.radioReturn.Text = "Return";
            this.radioReturn.UseVisualStyleBackColor = true;
            // 
            // textZipCode
            // 
            this.textZipCode.Location = new System.Drawing.Point(280, 134);
            this.textZipCode.Name = "textZipCode";
            this.textZipCode.Size = new System.Drawing.Size(120, 23);
            this.textZipCode.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(277, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 15);
            this.label7.TabIndex = 11;
            this.label7.Text = "Zip Code";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textSecurityCode
            // 
            this.textSecurityCode.Location = new System.Drawing.Point(280, 90);
            this.textSecurityCode.Name = "textSecurityCode";
            this.textSecurityCode.Size = new System.Drawing.Size(120, 23);
            this.textSecurityCode.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(277, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 15);
            this.label6.TabIndex = 9;
            this.label6.Text = "Security Code";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textNameOnCard
            // 
            this.textNameOnCard.Location = new System.Drawing.Point(14, 178);
            this.textNameOnCard.Name = "textNameOnCard";
            this.textNameOnCard.Size = new System.Drawing.Size(220, 23);
            this.textNameOnCard.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 15);
            this.label5.TabIndex = 5;
            this.label5.Text = "Name On Card";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textExpDate
            // 
            this.textExpDate.Location = new System.Drawing.Point(13, 134);
            this.textExpDate.Name = "textExpDate";
            this.textExpDate.Size = new System.Drawing.Size(150, 23);
            this.textExpDate.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Expiration (MMYY)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textCardNumber
            // 
            this.textCardNumber.Location = new System.Drawing.Point(13, 90);
            this.textCardNumber.Name = "textCardNumber";
            this.textCardNumber.Size = new System.Drawing.Size(220, 23);
            this.textCardNumber.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Card Number";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textAmount
            // 
            this.textAmount.Location = new System.Drawing.Point(280, 178);
            this.textAmount.Name = "textAmount";
            this.textAmount.Size = new System.Drawing.Size(120, 23);
            this.textAmount.TabIndex = 14;
            // 
            // labelAmount
            // 
            this.labelAmount.AutoSize = true;
            this.labelAmount.Location = new System.Drawing.Point(277, 160);
            this.labelAmount.Name = "labelAmount";
            this.labelAmount.Size = new System.Drawing.Size(51, 15);
            this.labelAmount.TabIndex = 13;
            this.labelAmount.Text = "Amount";
            this.labelAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textPayNote
            // 
            this.textPayNote.AcceptsTab = true;
            this.textPayNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textPayNote.BackColor = System.Drawing.SystemColors.Window;
            this.textPayNote.DetectLinksEnabled = false;
            this.textPayNote.DetectUrls = false;
            this.textPayNote.Location = new System.Drawing.Point(13, 222);
            this.textPayNote.Name = "textPayNote";
            this.textPayNote.QuickPasteType = OpenDentBusiness.QuickPasteType.Payment;
            this.textPayNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textPayNote.Size = new System.Drawing.Size(398, 60);
            this.textPayNote.TabIndex = 16;
            this.textPayNote.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 204);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "Payment Note";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormXWeb
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(424, 361);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textPayNote);
            this.Controls.Add(this.labelRefNumber);
            this.Controls.Add(this.textRefNumber);
            this.Controls.Add(this.groupTransType);
            this.Controls.Add(this.textZipCode);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textSecurityCode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textNameOnCard);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textExpDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textCardNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textAmount);
            this.Controls.Add(this.labelAmount);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormXWeb";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "XWeb";
            this.Load += new System.EventHandler(this.FormXWeb_Load);
            this.groupTransType.ResumeLayout(false);
            this.groupTransType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label labelRefNumber;
        private System.Windows.Forms.TextBox textRefNumber;
        private System.Windows.Forms.GroupBox groupTransType;
        private System.Windows.Forms.RadioButton radioReturn;
        private System.Windows.Forms.TextBox textZipCode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textSecurityCode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textNameOnCard;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textExpDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textCardNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textAmount;
        private System.Windows.Forms.Label labelAmount;
        private ODtextBox textPayNote;
        private System.Windows.Forms.Label label1;
    }
}