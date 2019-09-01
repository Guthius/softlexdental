using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormClaimCustomTrackingUpdate:ODForm {
		/// <summary>List of claims selected from outstanding claim report</summary>
		private List<Claim> _listClaims;
		///<summary>When creating a new claimTracking this list contains 1 for every claim in _listClaims. Otherwise null</summary>
		public List<ClaimTracking> ListNewClaimTracks;

		///<summary>Used when creating a brand new claimcustomtracking.</summary>
		public FormClaimCustomTrackingUpdate(List<Claim> listClaims) {
			InitializeComponent();
			
			_listClaims=listClaims;
			ListNewClaimTracks=new List<ClaimTracking>(new ClaimTracking[_listClaims.Count]);
		}

		///<summary>Used when creating a brand new claimcustomtracking.</summary>
		public FormClaimCustomTrackingUpdate(Claim claimCur,string noteText) {
			InitializeComponent();
			
			_listClaims=new List<Claim>() {claimCur};
			ListNewClaimTracks=new List<ClaimTracking>(new ClaimTracking[_listClaims.Count]);//Default to list of nulls.
			textNotes.Text=noteText;
		}

		///<summary>Used for editing a ClaimTracking object from FormClaimEdit.</summary>
		public FormClaimCustomTrackingUpdate(Claim claimCur,ClaimTracking claimTrack) {
			InitializeComponent();
			
			_listClaims=new List<Claim>() { claimCur };
			ListNewClaimTracks=new List<ClaimTracking>() { claimTrack };
		}

		private void FormClaimCustomTrackingUpdate_Load(object sender,EventArgs e) {
			comboCustomTracking.Items.Clear();
			int hasNone=0; //used to offset the index in the loop when none is an option for the combo box.
			if(!Preference.GetBool(PreferenceName.ClaimTrackingStatusExcludesNone)) {
				//None is allowed as an option
				comboCustomTracking.Items.Add(new ODBoxItem<Definition>(Lan.g(this,"None"),new Definition() {Value=""}));
				comboCustomTracking.SelectedIndex=0;
				hasNone=1;
			}
			Definition[] arrayCustomTrackingDefs=Definition.GetByCategory(DefinitionCategory.ClaimCustomTracking).ToArray();
			//When creating a new ClaimTracking this is null, otherwise list contains 1 given ClaimTracking to edit.
			ClaimTracking claimTrack=ListNewClaimTracks.FirstOrDefault();
			for(int i=0;i<arrayCustomTrackingDefs.Length;i++) { 
				comboCustomTracking.Items.Add(new ODBoxItem<Definition>(arrayCustomTrackingDefs[i].Description,arrayCustomTrackingDefs[i]));
				if(claimTrack?.TrackingDefNum==arrayCustomTrackingDefs[i].Id) {
					comboCustomTracking.SelectedIndex=i+hasNone;//adding 1 to the index because we have added a 'none' option above
				}
			}
			if(claimTrack==null) {//Creating a new ClaimTracking
				comboCustomTracking.SelectedIndex=0;
			}
			else if(comboCustomTracking.SelectedIndex==-1) {//Editing an existing ClaimTracking, 'None' not showing and def not matched.
				//We must be editing an existing ClaimTracking with a previous TrackingDefNum of 'None'
				//Previously we allowed users to add ClaimTracking entries with a TrackingDefNum of 0 which represnets 'None'.
				//We have since added a new pref to remove 'None' as an option. 
				string none=Lan.g(this,"None");
				comboCustomTracking.IndexSelectOrSetText(-1,() => { return none; });
			}
			textNotes.Text=claimTrack?.Note??"";
			FillComboErrorCode();
		}

		private void FillComboErrorCode() {
			Definition[] arrayErrorCodeDefs = Definition.GetByCategory(DefinitionCategory.ClaimErrorCode).ToArray();
			comboErrorCode.Items.Clear();
			//Add "none" option.
			comboErrorCode.Items.Add(new ODBoxItem<Definition>(Lan.g(this,"None"),new Definition() {Value=""}));
			comboErrorCode.SelectedIndex=0;
			if(arrayErrorCodeDefs.Length==0) {
				//if the list is empty, then disable the comboBox.
				comboErrorCode.Enabled=false;
				return;
			}
			//Fill comboErrorCode.
			ClaimTracking claimTrack=ListNewClaimTracks.FirstOrDefault();
			for(int i=0;i<arrayErrorCodeDefs.Length;i++) {
				//hooray for using new ODBoxItems!
				comboErrorCode.Items.Add(new ODBoxItem<Definition>(arrayErrorCodeDefs[i].Description,arrayErrorCodeDefs[i]));
				if(claimTrack?.TrackingErrorDefNum==arrayErrorCodeDefs[i].Id) {
					comboErrorCode.SelectedIndex=i+1;//adding 1 to the index because we have added a 'none' option above
				}
			}
		}

		private void comboErrorCode_SelectionChangeCommitted(object sender,EventArgs e) {
			if((!comboErrorCode.Enabled) || ((ODBoxItem<Definition>)comboErrorCode.SelectedItem).Tag==null) {
				textErrorDesc.Text="";
			}
			else {
				textErrorDesc.Text=((ODBoxItem<Definition>)comboErrorCode.SelectedItem).Tag.Value.ToString();
			}	
		}

		private void butUpdate_Click(object sender,EventArgs e) {
			if(comboCustomTracking.SelectedIndex==-1) {
				//Defaults to -1 when editing and old ClaimTracking where TrackingDefNum is 0 ('None') and ClaimTrackingStatusExcludesNone is true.
				MsgBox.Show(this,"You must specify a Custom Track Status.");
				return;
			}
			if(Preference.GetBool(PreferenceName.ClaimTrackingRequiresError) 
				&& ((ODBoxItem<Definition>)comboErrorCode.SelectedItem).Tag == null 
				&& comboErrorCode.Enabled )
			{
				MsgBox.Show(this,"You must specify an error code."); //Do they have to specify an error code even if they set the status to None?
				return;
			}
			Definition customTrackingDef=((ODBoxItem<Definition>)comboCustomTracking.SelectedItem).Tag;
			if(customTrackingDef.Id==0) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Setting the status to none will disable filtering in the Outstanding Claims Report."
						+"  Do you wish to set the status of this claim to none?")) {
					return;
				}
			}
			Definition errorCodeDef=((ODBoxItem<Definition>)comboErrorCode.SelectedItem).Tag;
			for(int i=0;i<_listClaims.Count;i++) { //when not called from FormRpOutstandingIns, this should only have one claim.
				_listClaims[i].CustomTracking=customTrackingDef.Id;
				Claims.Update(_listClaims[i]);
				ClaimTracking trackCur=ListNewClaimTracks[i];
				if(trackCur==null) {
					trackCur=new ClaimTracking();
					trackCur.ClaimNum=_listClaims[i].ClaimNum;
				}
				trackCur.Note=textNotes.Text;
				trackCur.TrackingDefNum=customTrackingDef.Id;
				trackCur.TrackingErrorDefNum=errorCodeDef.Id;
				trackCur.UserNum=Security.CurUser.UserNum;
				if(trackCur.ClaimTrackingNum==0) { //new claim tracking status.
					ClaimTrackings.Insert(trackCur);
				}
				else { //existing claim tracking status
					ClaimTrackings.Update(trackCur);
				}
				ListNewClaimTracks[i]=trackCur;//Update list.
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}