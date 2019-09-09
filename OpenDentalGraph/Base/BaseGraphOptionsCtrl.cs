using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDentalGraph
{
    public class OptionsControl : UserControl
    {
        public event EventHandler OptionsChanged;

        /// <summary>
        /// Raises the <see cref="OptionsChanged"/> event.
        /// </summary>
        protected virtual void OnOptionsChanged() => OptionsChanged(this, EventArgs.Empty);
    }

    public partial class GraphOptionsBase : UserControl
    {
        public event EventHandler InputsChanged;

        protected void OnBaseInputsChanged(object sender, EventArgs e)
        {
            // Another event is coming shortly that will be for the newly checked radio button. Wait for that one to avoid double processing.
            if (sender is RadioButton radioButton && !radioButton.Checked) return;

            InputsChanged?.Invoke(this, new EventArgs());
        }

        public virtual int GetPanelHeight() => 63;

        /// <summary>
        /// If you override this and your override can return true, make sure you check to see if 
        /// Clinics are enabled before showing the grouping options.
        /// </summary>
        public virtual bool HasGroupOptions => OpenDentBusiness.Preferences.HasClinicsEnabled;
    }
}
