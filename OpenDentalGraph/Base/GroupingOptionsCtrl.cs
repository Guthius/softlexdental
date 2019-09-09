using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDentalGraph
{
    public partial class GroupingOptionsControl : OptionsControl
    {
        public enum Grouping { Provider, Clinic };

        public Grouping CurGrouping
        {
            get => radioGroupProvs.Checked ? Grouping.Provider : Grouping.Clinic;
            set
            {
                switch (value)
                {
                    case Grouping.Provider:
                        radioGroupProvs.Checked = true;
                        break;

                    case Grouping.Clinic:
                        radioGroupClinics.Checked = true;
                        break;
                }
            }
        }

        public GroupingOptionsControl() => InitializeComponent();

        private void RadioGroupByChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton radioButton && !radioButton.Checked)
            {
                return;
            }

            OnOptionsChanged();
        }
    }
}
