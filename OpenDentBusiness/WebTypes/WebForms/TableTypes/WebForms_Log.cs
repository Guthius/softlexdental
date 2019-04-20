using System;

namespace OpenDentBusiness.WebTypes.WebForms {
	[Serializable]
	[ODTable(IsMissingInGeneral=true,CrudLocationOverride=@"..\..\..\OpenDentBusiness\WebTypes\WebForms\Crud",NamespaceOverride="OpenDentBusiness.WebTypes.WebForms.Crud",CrudExcludePrefC=true)]
	public class WebForms_Log:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long LogNum;
		///<summary></summary>
		public long DentalOfficeID;
		///<summary></summary>
		public string WebSheetDefIDs;
		///<summary></summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string LogMessage;
		///<summary></summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.TimeStamp)]
		public DateTime DateTStamp;
		
    public WebForms_Log(){

		}
		
    public WebForms_Log Copy(){
			return (WebForms_Log)this.MemberwiseClone();
		}
	}
}