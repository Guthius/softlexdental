using System;

namespace OpenDentBusiness{

	///<summary>Some insurance companies require special provider ID #s, and this table holds them.</summary>
	[Serializable]
	public class ProviderIdent:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long ProviderIdentNum;
		///<summary>FK to provider.ProvNum.  An ID only applies to one provider.</summary>
		public long ProvNum;
		///<summary>FK to carrier.ElectID  aka Electronic ID. An ID only applies to one insurance carrier.</summary>
		public string PayorID;
		///<summary>Enum:ProviderSupplementalID</summary>
		public ProviderSupplementalID SuppIDType;
		///<summary>The number assigned by the ins carrier.</summary>
		public string IDNumber;

		public ProviderIdent Copy() {
			return (ProviderIdent)this.MemberwiseClone();
		}
	}

}










