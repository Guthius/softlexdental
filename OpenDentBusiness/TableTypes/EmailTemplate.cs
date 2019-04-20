using System;
using System.Collections;

namespace OpenDentBusiness{

	///<summary>A template email which can be used as the basis for a new email.</summary>
	[Serializable]
	public class EmailTemplate:ODTable {
		///<summary>Primary key.</summary>
		[ODTableColumn(PrimaryKey=true)]
		public long EmailTemplateNum;
		///<summary>Default subject line.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string Subject;
		///<summary>Body of the email</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string BodyText;
		///<summary>Different than Subject.  The description of the email template.  This is what the user sees in the list.</summary>
		[ODTableColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string Description;
		///<summary>True flags the emailtemplate as being in HTML format, false if plain text.</summary>
		public bool IsHtml;

		///<summary>Returns a copy of this EmailTemplate.</summary>
		public EmailTemplate Copy(){
			return (EmailTemplate)this.MemberwiseClone();
		}

		

		
		
	}

	
	

}













