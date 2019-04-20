using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness{
	///<summary>Used to store preferences specific to clinics.</summary>
	[Serializable()]
	[ODTable(IsSynchable=true)]
	public class ClinicPref:ODTable{
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long ClinicPrefNum;
		///<summary>FK to clinic.ClinicNum.</summary>
		public long ClinicNum;
		///<summary>Enum: </summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.EnumAsString)]
		public PrefName PrefName;
		///<summary>The stored value.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string ValueString;

		public ClinicPref() {
			
		}

		public ClinicPref(long clinicNum, PrefName prefName, bool valueBool) {
			this.ClinicNum=clinicNum;
			this.PrefName=prefName;
			this.ValueString=POut.Bool(valueBool);
		}

		public ClinicPref(long clinicNum, PrefName prefName, string valueString) {
			this.ClinicNum=clinicNum;
			this.PrefName=prefName;
			this.ValueString=valueString;
		}

		///<summary></summary>
		public ClinicPref Clone() {
			return (ClinicPref)this.MemberwiseClone();
		}

	}

	
}



