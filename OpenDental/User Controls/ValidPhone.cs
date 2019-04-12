using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	///<summary>A simple text box that will automatically format phone numbers for US and Canadian users as the user types.</summary>
	public partial class ValidPhone:System.Windows.Forms.TextBox {

		[Category("Behavior")]
		[DefaultValue(true)]
		[Description("Controls whether the content typed in will be automatically formatted (US and Canada only).")]
		public bool IsFormattingEnabled { get;set; }=true;

		///<summary></summary>
		public ValidPhone() {
			InitializeComponent();
		}

		private void ValidPhone_TextChanged(object sender, System.EventArgs e) {
			if(sender.GetType()!=typeof(ValidPhone)) {
				return;
			}
			if(!IsFormattingEnabled) { 
				return;
			}
			ValidPhone textPhone=(ValidPhone)sender;
			int phoneTextPosition=textPhone.SelectionStart;
			int textLength=textPhone.Text.Length;
			textPhone.Text=TelephoneNumbers.AutoFormat(textPhone.Text);
			int diff=textPhone.Text.Length-textLength;
			int newSelectionStartPosition=phoneTextPosition+diff;
			//If there are characters that get removed from calling AutoFormat (i.e. spaces) 
			// and the cursor was at the start (which happens if/when the ValidPhone control initially gets filled with a value)
			//the calculated new selection start index would be an invalid value.
			if(newSelectionStartPosition<0) {
				newSelectionStartPosition=0;
			}
			textPhone.SelectionStart=newSelectionStartPosition;
		}

	}
}










