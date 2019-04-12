using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenDentBusiness;

namespace OpenDental {
	public interface IPrefBinding {
		PrefName PrefNameBinding { get; set; }
		bool DoAutoSave { get; set; }
		///<summary>Returns true if a change was required, or false if no change needed.</summary>
		bool Save();

	}
}
