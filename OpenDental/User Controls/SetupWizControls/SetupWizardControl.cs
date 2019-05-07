using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental.User_Controls.SetupWizard
{
    [ToolboxItem(false)]
    public partial class SetupWizardControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetupWizardControl"/> class.
        /// </summary>
        public SetupWizardControl()
        {
            Padding = new Padding(40);
            Font = new Font("Segoe UI", 9f, FontStyle.Regular);
        }

        /// <summary>
        /// Gets a value indicating whether the wizard is done.
        /// </summary>
        public bool IsDone { get; protected set; } = false;

        /// <summary>
        /// Gets the last validation error.
        /// </summary>
        public string Error { get; protected set; } = Translation.Language.PleaseFillInTheMissingFieldsFirst;

        /// <summary>
        /// Called when the wizard is done.
        /// </summary>
        public void Done() => OnControlDone(EventArgs.Empty);

        /// <summary>
        /// Raises the <see cref="ControlDone"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnControlDone(EventArgs e) { }
    }
}