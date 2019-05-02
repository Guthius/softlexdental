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

namespace OpenDental {
	public partial class ODComboBoxPrefLong:ComboBox,IPrefBinding {

		public PrefName PrefNameBinding { get; set; }
		public ComboBoxSpecialValues SpecialOption { get; set; }
		public bool DoAutoSave { get; set; }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public long SelectedLong
		{
			get
			{
				if(IsNothingSelected() || IsSpecialOptionSelected()) {
					return -1;
				}
				return ((ODBoxItem<long>)SelectedItem).Tag;
			}
			set
			{
				int selectedIndex=-1;
				for(int i=0;i<Items.Count;i++) {
					if(value==((ODBoxItem<long>)Items[i]).Tag) {
						selectedIndex=i;
						break;
					}
				}
				SelectedIndex=selectedIndex;
			}
		}

		public ODComboBoxPrefLong() {
			InitializeComponent();
			PrefNameBinding=PrefName.NotApplicable;//Set this by default, if it's set in the designer it will overwite this later.
		}

		public bool IsNothingSelected() {
			return SelectedIndex==-1;
		}

		///<summary>Returns true if this provider box has the "None" or "All" option and that option is currently selected</summary>
		public bool IsSpecialOptionSelected() {
			return SpecialOption!=ComboBoxSpecialValues.NotApplicable && SelectedIndex==0;
		}

		public void SetItems<T>(List<T> listItems,Func<T,ODBoxItem<long>> itemsToBoxItems) {
			Items.Clear();
			if(SpecialOption!=ComboBoxSpecialValues.NotApplicable) {
				long enumVal=-(long)SpecialOption; //set this to a negative tag value so we don't mistake it for a different item
				Items.Add(new ODBoxItem<long>(SpecialOption.GetDescription(useShortVersionIfAvailable:true),enumVal));
			}
			foreach(T item in listItems) {
				Items.Add(itemsToBoxItems(item));
			}
			SetSelected();
		}

		public void SetSelected() {
			if(PrefNameBinding.GetValueType()==PrefValueType.NONE) {
				this.SelectedIndex=0;
			}
			else {
				string overrideText=(SpecialOption==ComboBoxSpecialValues.NotApplicable?"":SpecialOption.GetDescription());
				this.SetSelectedItem<long>(x => x==Preferences.GetLong(PrefNameBinding),overrideText);
			}
		}

		public bool Save() {
			return PrefNameBinding.Update(this.SelectedTag<long>());
		}
	}
}
