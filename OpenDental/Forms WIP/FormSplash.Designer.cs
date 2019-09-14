namespace OpenDental
{
    partial class FormSplash
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components=null;

        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSplash));
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelProgress = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(1, 324);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(498, 15);
            this.progressBar.TabIndex = 1;
            // 
            // labelProgress
            // 
            this.labelProgress.BackColor = System.Drawing.Color.White;
            this.labelProgress.Location = new System.Drawing.Point(1, 300);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.labelProgress.Size = new System.Drawing.Size(498, 25);
            this.labelProgress.TabIndex = 0;
            this.labelProgress.Text = "Initializing Open Dental... (0%)";
            this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormSplash
            // 
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(500, 340);
            this.ControlBox = false;
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.labelProgress);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSplash";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "5";
            this.Load += new System.EventHandler(this.FormSplash_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelProgress;
    }
}