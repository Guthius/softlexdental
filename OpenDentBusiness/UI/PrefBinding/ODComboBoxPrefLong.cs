using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenDental
{
    [ToolboxItem(false)]
    public partial class ODComboBoxPrefLong : ComboBox, IPreferenceBinding
    {
        public PreferenceName Preference { get; set; }

        public ComboBoxSpecialValues SpecialOption { get; set; }

        public bool DoAutoSave { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long SelectedLong
        {
            get
            {
                if (SelectedItem is ODBoxItem<long> item)
                {
                    return item.Tag;
                }

                return -1;
            }
            set
            {
                object selectedItem = null;

                foreach (ODBoxItem<long> item in Items)
                {
                    if (item.Tag == value)
                    {
                        selectedItem = item;

                        break;
                    }
                }

                SelectedItem = selectedItem;
            }
        }

        public ODComboBoxPrefLong()
        {
            Preference = PreferenceName.NotApplicable;
        }

        public bool IsNothingSelected()
        {
            return SelectedIndex == -1;
        }

        ///<summary>Returns true if this provider box has the "None" or "All" option and that option is currently selected</summary>
        public bool IsSpecialOptionSelected()
        {
            return SpecialOption != ComboBoxSpecialValues.NotApplicable && SelectedIndex == 0;
        }

        public void SetItems<T>(List<T> listItems, Func<T, ODBoxItem<long>> itemsToBoxItems)
        {
            Items.Clear();
            if (SpecialOption != ComboBoxSpecialValues.NotApplicable)
            {
                long enumVal = -(long)SpecialOption; //set this to a negative tag value so we don't mistake it for a different item
                Items.Add(new ODBoxItem<long>(SpecialOption.GetDescription(useShortVersionIfAvailable: true), enumVal));
            }
            foreach (T item in listItems)
            {
                Items.Add(itemsToBoxItems(item));
            }
            SetSelected();
        }

        public void SetSelected()
        {
            if (Preference.GetValueType() == PrefValueType.NONE)
            {
                SelectedIndex = 0;
            }
            else
            {
                string overrideText = SpecialOption == ComboBoxSpecialValues.NotApplicable ? "" : SpecialOption.GetDescription();

                this.SetSelectedItem((long x) => x == OpenDentBusiness.Preference.GetLong(Preference), overrideText);
            }
        }

        public bool Save() => Preference.Update(this.SelectedTag<long>());
    }
}
