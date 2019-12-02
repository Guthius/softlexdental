using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental
{
    public partial class ODComboBoxProv : ODComboBoxPrefLong
    {

        private List<Provider> _listProvs;
        public bool IsUsingProvAbbr { get; set; }

        public new ComboBoxSpecialValues SpecialOption
        {
            get { return base.SpecialOption; }
            set
            {
                base.SpecialOption = value;
                FillProviders(); //Don't fill providers until we know if there will be a "None" or "All" option
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Provider SelectedProvider
        {
            get
            {
                if (IsNothingSelected() || IsSpecialOptionSelected())
                {
                    return null;
                }
                return _listProvs.FirstOrDefault(x => x.Id == this.SelectedTag<long>());
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long SelectedProvNum
        {
            get
            {
                if (IsNothingSelected() || IsSpecialOptionSelected())
                {
                    return -1;
                }
                return ((ODBoxItem<long>)SelectedItem).Tag;
            }
            set
            {
                SelectedIndex = -1;
                for (int i = 0; i < Items.Count; i++)
                {
                    if (value == ((ODBoxItem<long>)Items[i]).Tag)
                    {
                        SelectedIndex = i;
                    }
                }
            }
        }

        public ODComboBoxProv()
        {
            Preference = PreferenceName.NotApplicable;
        }

        public void FillProviders()
        {
            if (!Db.HasDatabaseConnection && !Security.IsUserLoggedIn)
            {
                return;
            }
            _listProvs = Provider.All().ToList();

            SetItems(_listProvs, (prov) => new ODBoxItem<long>(IsUsingProvAbbr ? prov.GetAbbr() : prov.GetLongDesc(), prov.Id));
            SetSelected();
        }
    }
}
