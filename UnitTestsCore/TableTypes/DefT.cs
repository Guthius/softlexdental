using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenDentBusiness;
using System.Drawing;

namespace UnitTestsCore {
	public class DefT {

		///<summary></summary>
		public static Definition CreateDefinition(DefinitionCategory category,string itemName,string itemValue="",Color itemColor=new Color()) {
			Definition def=new Definition();
			def.Category=category;
			def.Color=itemColor;
			def.Description=itemName;
			def.Value=itemValue;
			Defs.Insert(def);
			Defs.RefreshCache();
			return def;
		}
	}
}
