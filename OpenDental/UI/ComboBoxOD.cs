using System.ComponentModel;
using System.Windows.Forms;

namespace OpenDental.UI
{
    public partial class ComboBoxOD : ComboBox
    {
        [Category("Behavior")]
        [Description("Set to true by default. Allows the combobox to scroll with the scroll wheel.")]
        [DefaultValue(true)]
        public bool AllowScroll { get; set; } = true;

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!AllowScroll)
            {
                ((HandledMouseEventArgs)e).Handled = true;
                return;
            }
            base.OnMouseWheel(e);
        }
    }
}
