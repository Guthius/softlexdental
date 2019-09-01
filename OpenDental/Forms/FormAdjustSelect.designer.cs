namespace OpenDental
{
    partial class FormAdjustSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdjustSelect));
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.sumGroupBox = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelAmtEnd = new System.Windows.Forms.Label();
            this.labelCurSplitAmt = new System.Windows.Forms.Label();
            this.labelAmtAvail = new System.Windows.Forms.Label();
            this.labelAmtUsed = new System.Windows.Forms.Label();
            this.amtOriginialLabel = new System.Windows.Forms.Label();
            this.adjustmentGrid = new OpenDental.UI.ODGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.sumGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(481, 382);
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
            this.cancelButton.Location = new System.Drawing.Point(481, 418);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 30);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            // 
            // sumGroupBox
            // 
            this.sumGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sumGroupBox.Controls.Add(this.label9);
            this.sumGroupBox.Controls.Add(this.label12);
            this.sumGroupBox.Controls.Add(this.label6);
            this.sumGroupBox.Controls.Add(this.label3);
            this.sumGroupBox.Controls.Add(this.label2);
            this.sumGroupBox.Controls.Add(this.labelAmtEnd);
            this.sumGroupBox.Controls.Add(this.labelCurSplitAmt);
            this.sumGroupBox.Controls.Add(this.labelAmtAvail);
            this.sumGroupBox.Controls.Add(this.labelAmtUsed);
            this.sumGroupBox.Controls.Add(this.amtOriginialLabel);
            this.sumGroupBox.Location = new System.Drawing.Point(411, 79);
            this.sumGroupBox.Name = "sumGroupBox";
            this.sumGroupBox.Size = new System.Drawing.Size(180, 192);
            this.sumGroupBox.TabIndex = 2;
            this.sumGroupBox.TabStop = false;
            this.sumGroupBox.Text = "Adjustment Sum";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 115);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 18);
            this.label9.TabIndex = 8;
            this.label9.Text = "Amt End:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(6, 93);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(86, 18);
            this.label12.TabIndex = 6;
            this.label12.Text = "Current Split:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 18);
            this.label6.TabIndex = 4;
            this.label6.Text = "Amt Avail:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Already Used:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Amt Original:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAmtEnd
            // 
            this.labelAmtEnd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAmtEnd.Location = new System.Drawing.Point(98, 118);
            this.labelAmtEnd.Name = "labelAmtEnd";
            this.labelAmtEnd.Size = new System.Drawing.Size(52, 13);
            this.labelAmtEnd.TabIndex = 9;
            this.labelAmtEnd.Text = "0.00";
            // 
            // labelCurSplitAmt
            // 
            this.labelCurSplitAmt.Location = new System.Drawing.Point(98, 96);
            this.labelCurSplitAmt.Name = "labelCurSplitAmt";
            this.labelCurSplitAmt.Size = new System.Drawing.Size(52, 13);
            this.labelCurSplitAmt.TabIndex = 7;
            this.labelCurSplitAmt.Text = "0.00";
            // 
            // labelAmtAvail
            // 
            this.labelAmtAvail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAmtAvail.Location = new System.Drawing.Point(98, 74);
            this.labelAmtAvail.Name = "labelAmtAvail";
            this.labelAmtAvail.Size = new System.Drawing.Size(52, 13);
            this.labelAmtAvail.TabIndex = 5;
            this.labelAmtAvail.Text = "0.00";
            // 
            // labelAmtUsed
            // 
            this.labelAmtUsed.Location = new System.Drawing.Point(98, 52);
            this.labelAmtUsed.Name = "labelAmtUsed";
            this.labelAmtUsed.Size = new System.Drawing.Size(52, 13);
            this.labelAmtUsed.TabIndex = 3;
            this.labelAmtUsed.Text = "0.00";
            // 
            // amtOriginialLabel
            // 
            this.amtOriginialLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.amtOriginialLabel.Location = new System.Drawing.Point(98, 30);
            this.amtOriginialLabel.Name = "amtOriginialLabel";
            this.amtOriginialLabel.Size = new System.Drawing.Size(52, 13);
            this.amtOriginialLabel.TabIndex = 1;
            this.amtOriginialLabel.Text = "0.00";
            // 
            // adjustmentGrid
            // 
            this.adjustmentGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.adjustmentGrid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.adjustmentGrid.EditableEnterMovesDown = false;
            this.adjustmentGrid.HasAddButton = false;
            this.adjustmentGrid.HasDropDowns = false;
            this.adjustmentGrid.HasMultilineHeaders = false;
            this.adjustmentGrid.HScrollVisible = false;
            this.adjustmentGrid.Location = new System.Drawing.Point(16, 79);
            this.adjustmentGrid.Name = "adjustmentGrid";
            this.adjustmentGrid.ScrollValue = 0;
            this.adjustmentGrid.Size = new System.Drawing.Size(389, 369);
            this.adjustmentGrid.TabIndex = 1;
            this.adjustmentGrid.Title = "Unattached Adjustments";
            this.adjustmentGrid.TitleVisible = true;
            this.adjustmentGrid.CellDoubleClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.AdjustmentGrid_CellDoubleClick);
            this.adjustmentGrid.CellClick += new System.EventHandler<OpenDental.UI.ODGridClickEventArgs>(this.AdjustmentGrid_CellClick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(578, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "Adjustments listed below are unattached to procedures and aren\'t fully allocated " +
    "via paysplits. \r\n\r\nSelect which Adjustment to use.";
            // 
            // FormAdjustSelect
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(604, 461);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.adjustmentGrid);
            this.Controls.Add(this.sumGroupBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(525, 467);
            this.Name = "FormAdjustSelect";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Adjustment";
            this.Load += new System.EventHandler(this.FormAdjustSelect_Load);
            this.sumGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox sumGroupBox;
        private UI.ODGrid adjustmentGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelAmtAvail;
        private System.Windows.Forms.Label labelAmtUsed;
        private System.Windows.Forms.Label amtOriginialLabel;
        private System.Windows.Forms.Label labelAmtEnd;
        private System.Windows.Forms.Label labelCurSplitAmt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label9;
    }
}