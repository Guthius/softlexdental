using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace OpenDentBusiness{

	///<summary>Quick paste categories are used by the quick paste notes feature.</summary>
	[Serializable]
	[ODTable(IsSynchable=true)]
	public class QuickPasteCat:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long QuickPasteCatNum;
		///<summary>.</summary>
		public string Description;
		///<summary>The order of this category within the list. 0-based.</summary>
		public int ItemOrder;
		///<summary>Enum:QuickPasteType  Each Category can be set to be the default category for multiple types of notes. Stored as integers separated by commas.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string DefaultForTypes;

		///<summary>Helper property for a list of DefaultForTypes as an actual enumeration list.</summary>
		[XmlIgnore,JsonIgnore]
		public List<QuickPasteType> ListDefaultForTypes {
			get {
				if(string.IsNullOrEmpty(DefaultForTypes)) {
					return new List<QuickPasteType>();
				}
				return DefaultForTypes.Split(',').Select(x => PIn.Enum<QuickPasteType>(x)).ToList();
			}
		}
		
		public QuickPasteCat Copy() {
			return (QuickPasteCat)this.MemberwiseClone();
		}
		


	}

	


}









