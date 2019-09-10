namespace OpenDental
{
    partial class FormTimeAdjustEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTimeAdjustEdit));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textTimeEntry = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textNote = new System.Windows.Forms.TextBox();
            this.checkOvertime = new System.Windows.Forms.CheckBox();
            this.textHours = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.butDelete = new System.Windows.Forms.Button();
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.radioAuto = new System.Windows.Forms.RadioButton();
            this.radioManual = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Date/Time Entry";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(11, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Hours";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textTimeEntry
            // 
            this.textTimeEntry.Location = new System.Drawing.Point(137, 50);
            this.textTimeEntry.Name = "textTimeEntry";
            this.textTimeEntry.Size = new System.Drawing.Size(155, 20);
            this.textTimeEntry.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "Note";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textNote
            // 
            this.textNote.Location = new System.Drawing.Point(137, 124);
            this.textNote.Multiline = true;
            this.textNote.Name = "textNote";
            this.textNote.Size = new System.Drawing.Size(377, 96);
            this.textNote.TabIndex = 6;
            // 
            // checkOvertime
            // 
            this.checkOvertime.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkOvertime.Location = new System.Drawing.Point(12, 103);
            this.checkOvertime.Name = "checkOvertime";
            this.checkOvertime.Size = new System.Drawing.Size(139, 17);
            this.checkOvertime.TabIndex = 5;
            this.checkOvertime.Text = "Overtime Adjustment";
            this.checkOvertime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkOvertime.UseVisualStyleBackColor = true;
            // 
            // textHours
            // 
            this.textHours.Location = new System.Drawing.Point(137, 77);
            this.textHours.Name = "textHours";
            this.textHours.Size = new System.Drawing.Size(68, 20);
            this.textHours.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(152, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(300, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "(the hours will be shifted from regular time to overtime)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // butDelete
            // 
            this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
            this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butDelete.Location = new System.Drawing.Point(37, 269);
            this.butDelete.Name = "butDelete";
            this.butDelete.Size = new System.Drawing.Size(79, 24);
            this.butDelete.TabIndex = 0;
            this.butDelete.TabStop = false;
            this.butDelete.Text = "&Delete";
            this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
            // 
            // butOK
            // 
            this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOK.Location = new System.Drawing.Point(439, 237);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 24);
            this.butOK.TabIndex = 7;
            this.butOK.Text = "&OK";
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.Location = new System.Drawing.Point(439, 269);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 24);
            this.butCancel.TabIndex = 8;
            this.butCancel.Text = "&Cancel";
            // 
            // radioAuto
            // 
            this.radioAuto.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioAuto.Location = new System.Drawing.Point(12, 10);
            this.radioAuto.Name = "radioAuto";
            this.radioAuto.Size = new System.Drawing.Size(139, 18);
            this.radioAuto.TabIndex = 1;
            this.radioAuto.TabStop = true;
            this.radioAuto.Text = "Automatically entered";
            this.radioAuto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioAuto.UseVisualStyleBackColor = true;
            // 
            // radioManual
            // 
            this.radioManual.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioManual.Location = new System.Drawing.Point(12, 27);
            this.radioManual.Name = "radioManual";
            this.radioManual.Size = new System.Drawing.Size(139, 18);
            this.radioManual.TabIndex = 2;
            this.radioManual.TabStop = true;
            this.radioManual.Text = "Manually entered";
            this.radioManual.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioManual.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(152, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(170, 18);
            this.label5.TabIndex = 0;
            this.label5.Text = "(protected from auto deletion)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormTimeAdjustEdit
            // 
            this.ClientSize = new System.Drawing.Size(540, 313);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.radioManual);
            this.Controls.Add(this.radioAuto);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textHours);
            this.Controls.Add(this.checkOvertime);
            this.Controls.Add(this.textNote);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textTimeEntry);
            this.Controls.Add(this.butDelete);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.butCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTimeAdjustEdit";
            this.ShowInTaskbar = false;
            this.Text = "Edit Time Adjustment";
            this.Load += new System.EventHandler(this.FormTimeAdjustEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textTimeEntry;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textNote;
        private System.Windows.Forms.CheckBox checkOvertime;
        private System.Windows.Forms.Button butDelete;
        private System.Windows.Forms.TextBox textHours;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioAuto;
        private System.Windows.Forms.RadioButton radioManual;
        private System.Windows.Forms.Label label5;
    }
}
