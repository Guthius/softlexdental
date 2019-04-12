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

namespace OpenDental {
	public partial class ODCheckBoxPref:CheckBox,IPrefBinding {

		private PrefName _prefNameBinding=PrefName.NotApplicable;
		private bool _reverseValue;
		public bool DoAutoSave { get; set; }
		///<summary>For all those "EasyHide" prefs etc. where we store the opposite value of what we mean.</summary>
		public bool ReverseValue {
			get {
				return _reverseValue;
			}
			set {
				_reverseValue=value;
				if(!DesignMode) { //VisualStudio designer will break if we try to make a db call here
					Checked=_reverseValue?!PrefC.GetBool(PrefNameBinding):PrefC.GetBool(PrefNameBinding);
				}
			}
		}

		public PrefName PrefNameBinding
		{
			get {
				return _prefNameBinding;
			}
			set {
				_prefNameBinding=value;
				if(!DesignMode) { //VisualStudio designer will break if we try to make a db call here
					Checked=ReverseValue?!PrefC.GetBool(value):PrefC.GetBool(value);
				}
			}
		}

		public ODCheckBoxPref() {
			InitializeComponent();
		}

		public bool Save() {
			return PrefNameBinding.Update(ReverseValue?!Checked:Checked);
		}
	}
}
