using CodeBase;
using OpenDentBusiness;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental
{
    /// <summary>
    /// Launches a splash screen with a progress bar that will listen specifically for ODEvents with the passed in name.
    /// Returns an ODProgressWindow that has a Close method that should be invoked whenever the progress window should close.
    /// eventName should be set to the name of the ODEvents that this specific progress window should be processing.
    /// </summary>
    public partial class FormSplash : FormProgressBase
    {
        /// <summary>
        /// Do not instatiate this class. It is not meant for public use. Use ODProgress.ShowSplash() instead.
        /// Launches a splash screen with a progress bar that will display status updates for SplashProgressEvent events of type SplashScreenProgress.
        /// Set hasProgress to true in order to show the actual progress bar and label to the user. Hidden by default.
        /// </summary>
        /// <param name="hasProgress"></param>
        public FormSplash(bool hasProgress = false) : base(ODEventType.SplashScreenProgress, typeof(SplashProgressEvent))
        {
            InitializeComponent();

            if (!hasProgress)
            {
                progressBar.Hide();
                labelProgress.Hide();

                ClientSize = new Size(500, 300);
            }
        }

        void FormSplash_Load(object sender, EventArgs e)
        {
            Plugin.Trigger(this, "FormSplash_Loaded"); // TODO: Are plugins even loaded before FormSplash is loaded?
        }

        public sealed override void UpdateProgress(string status, ProgressBarHelper progHelper, bool hasProgHelper)
        {
            labelProgress.Text = status + "... (" + progHelper.PercentValue + ")";
            if (hasProgHelper)
            {
                if (progHelper.BlockMax != 0)
                {
                    progressBar.Maximum = progHelper.BlockMax;
                }
                if (progHelper.BlockValue != 0)
                {
                    // When the progress bar draws itself it gradually fills in the bar. This causes the progress bar to be much further behind the percent
                    // label when the program loads quickly. A trick to get around this is to set the value and then set the value to a lower value.
                    progressBar.Value = progHelper.BlockValue;
                    progressBar.Value = Math.Max(progHelper.BlockValue - 1, 0);
                    progressBar.Value = progHelper.BlockValue;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (var pen = new Pen(Color.FromArgb(0, 70, 140)))
            {
                e.Graphics.DrawRectangle(
                    pen,
                    new Rectangle(0, 0, 
                        ClientSize.Width - 1, 
                        ClientSize.Height - 1));
            }
        }
    }
}
