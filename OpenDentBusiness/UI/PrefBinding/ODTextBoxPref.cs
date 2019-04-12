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
	public partial class ODTextBoxPref:TextBox,IPrefBinding {

		private PrefName _prefNameBinding=PrefName.NotApplicable;//Set this by default, if it's set in the designer it will overwite this later.
		public bool DoAutoSave { get; set; }

		public ODTextBoxPref() {
			InitializeComponent();
		}

		public PrefName PrefNameBinding
		{
			get { return _prefNameBinding; }
			set
			{
				_prefNameBinding=value;
				if(!DesignMode) { //VisualStudio designer will break if we try to make a db call here
					Text=value.GetValueAsText();
				}
			}
		}

		public bool Save() {
			return PrefNameBinding.Update(Text);
		}

	}
}
