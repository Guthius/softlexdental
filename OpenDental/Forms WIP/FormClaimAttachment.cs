﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.Eclaims;

namespace OpenDental {
	///<summary></summary>
	public partial class FormClaimAttachment:Form {
		private Claim _claimCur;
		private Patient _claimPat;
		//private List<ClaimConnect.ImageAttachment> _listImageAttachments;
		private static ODThread _threadFormClaimAttach=null;
		///<summary>Initialize the form and refresh the claim we are adding attachments to.</summary>
		private FormClaimAttachment(Claim claim) {
			InitializeComponent();
			_claimCur=claim.Copy();
			//_listImageAttachments=new List<ClaimConnect.ImageAttachment>();
		}

		private void FormClaimAttachment_Load(object sender,EventArgs e) {
			_claimPat=Patients.GetPat(_claimCur.PatNum);
			List<Definition> imageTypeCategories=new List<Definition>();
			List<Definition> listClaimAttachmentDefs=CheckImageCatDefs();
			if(listClaimAttachmentDefs.Count<1) {//At least one Claim Attachment image definition exists.
				labelClaimAttachWarning.Visible=true;
			}
			FillGrid();
			ODProgress.ShowAction(()=> {ValidateClaimHelper();},"Communicating with DentalXChange...");
		}

		///<summary></summary>
		public static void Open(Claim claim) {
			//Show a dialog if the user tries to open more than one claim at a time
			if(_threadFormClaimAttach!=null) {
				MsgBox.Show("A claim attachment window is already open.");
				return;
			}
			_threadFormClaimAttach=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				FormClaimAttachment formCA=new FormClaimAttachment(claim);
				formCA.ShowDialog();
			}));
			//This thread needs to be STA because FormClaimAttachment will have the ability to open child forms,
			//which will need to block FormClaimAttachment while it is open.
			//STA mode is the only way to have ShowDialog() behavior in a non-main thread,
			//without throwing an exception (the exception literally says change the thread to STA)
			_threadFormClaimAttach.SetApartmentState(System.Threading.ApartmentState.STA);
			_threadFormClaimAttach.AddExitHandler(new ODThread.WorkerDelegate((ODThread o) => {
				_threadFormClaimAttach=null;
			}));
			_threadFormClaimAttach.Start(true);
		}

		///<summary>Purposely does not load in historical data. This form is only for creating new attachments.
		///The grid is populated by AddImageToGrid().</summary>
		private void FillGrid() {
			gridAttachedImages.BeginUpdate();
			gridAttachedImages.Columns.Clear();
			gridAttachedImages.Rows.Clear();
			ODGridColumn col;
			col=new ODGridColumn("Date", 80);
			gridAttachedImages.Columns.Add(col);
			col=new ODGridColumn("Image Type",150);
			gridAttachedImages.Columns.Add(col);
			col=new ODGridColumn("File",150);
			gridAttachedImages.Columns.Add(col);
			//ODGridRow row;
			//for(int i=0;i<_listImageAttachments.Count;i++) {
			//	row=new ODGridRow();
			//	row.Cells.Add(_listImageAttachments[i].ImageDate.ToShortDateString());
			//	row.Cells.Add(_listImageAttachments[i].ImageType.GetDescription());
			//	row.Cells.Add(_listImageAttachments[i].ImageFileNameDisplay);
			//	row.Tag=_listImageAttachments[i];
			//	gridAttachedImages.Rows.Add(row);
			//}
			gridAttachedImages.EndUpdate();
		}

		///<summary>Checks to see if the user has made a Claim Attachment image category definition. Returns the list of Defs found.</summary>
		private List<Definition> CheckImageCatDefs() {
			//Filter down to image categories that have been marked as Claim Attachment.
			return Definition.GetByCategory(DefinitionCategory.ImageCats).FindAll(x => x.Value.Contains("C"));
		}

		///<summary>A helper method that does the actual validation of the claim. 
		///Can be called elsewhere in this form.</summary>
		private bool ValidateClaimHelper() {
			try {
				//ClaimConnect.ValidateClaimResponse response=ClaimConnect.ValidateClaim(_claimCur,true);
				//if(response._isValidClaim) {
				//	textClaimStatus.Text="The claim is valid.";
				//	return true;
				//}
				////Otherwise the claim must have errors, display them to the user.
				//StringBuilder strBuild=new StringBuilder();
				//for(int i=0;i<response.ValidationErrors.Length;i++) {
				//	strBuild.AppendLine(response.ValidationErrors[i]);
				//}
				//textClaimStatus.Text=strBuild.ToString();
				return false;
			}
			catch(ODException ex) {
				textClaimStatus.Text=ex.Message;
				return false;
			}
			catch(Exception ex) {
				textClaimStatus.Text=ex.Message;
				return false;
			}
		}
		
		///<summary>Creates the ClaimAttach objects for each item in the grid and associates it to the given claim.
		///The parameter path should be the full path to the image and fileName should simply be the file name by itself.</summary>
		private ClaimAttach CreateClaimAttachment(string fileNameDisplay,string fileNameActual) {
			ClaimAttach claimAttachment=new ClaimAttach();
			claimAttachment.DisplayedFileName=fileNameDisplay;
			claimAttachment.ActualFileName=fileNameActual;
			claimAttachment.ClaimNum=_claimCur.ClaimNum;
			return claimAttachment;
		}

		private void buttonSnipTool_Click(object sender,EventArgs e) {
			this.WindowState=FormWindowState.Minimized;
			Image snip=FormODSnippingTool.Snip();
			ShowImageAttachmentItemEdit(snip);
			this.WindowState=FormWindowState.Normal;
		}

		private void ShowImageAttachmentItemEdit(Image img) {
			//if(img==null) {
			//	return;
			//}
			//FormClaimAttachmentItemEdit form=new FormClaimAttachmentItemEdit(img);
			//form.ShowDialog();
			//if(form.DialogResult==DialogResult.OK) {
			//	_listImageAttachments.Add(form.ImageAttachment);
			//	FillGrid();
			//}
		}

		private void buttonAddImage_Click(object sender,EventArgs e) {
			string patFolder=ImageStore.GetPatientFolder(_claimPat);
			OpenFileDialog fileDialog=new OpenFileDialog();
			fileDialog.Multiselect=false;
			fileDialog.InitialDirectory=patFolder;
			if(fileDialog.ShowDialog()!=DialogResult.OK) {
				return;
			}
			//The filename property is the entire path of the file.
			string selectedFile=fileDialog.FileName;
			if(selectedFile.EndsWith(".pdf")) {
				MessageBox.Show(this,"DentalXChange does not support PDF attachments.");
				return;
			}
			//There is purposely no validation that the user selected an image as that will be handled on Dentalxchange's end.
			Image image;
			try {
				image=Image.FromFile(selectedFile);
				ShowImageAttachmentItemEdit(image);
			}
			catch(System.IO.FileNotFoundException ex) {
                FormFriendlyException.Show(Lan.g(this,"The selected file at: "+selectedFile+" could not be found"),ex);
			}
			catch(System.OutOfMemoryException ex) {
                //Image.FromFile() will throw an OOM exception when the image format is invalid or not supported.
                //See MSDN if you have trust issues:  https://msdn.microsoft.com/en-us/library/stf701f5(v=vs.110).aspx
                FormFriendlyException.Show(Lan.g(this,"The file does not have a valid image format. Please try again or call support."+"\r\n"+ex.Message),ex);
			}
			catch(Exception ex) {
                FormFriendlyException.Show(Lan.g(this,"An error occurred. Please try again or call support.")+"\r\n"+ex.Message,ex);
			}
		}

		///<summary>Allows the user to edit an existing ImageAttachment object.</summary>
		private void CellDoubleClick_EditImage(object sender,ODGridClickEventArgs e) {
			//ODGridRow selectedRow=gridAttachedImages.Rows[gridAttachedImages.GetSelectedIndex()];
			//ClaimConnect.ImageAttachment selectedAttachment=(ClaimConnect.ImageAttachment)selectedRow.Tag;
			//FormClaimAttachmentItemEdit FormCAIE=new FormClaimAttachmentItemEdit(selectedAttachment.Image
			//	,selectedAttachment.ImageFileNameDisplay,selectedAttachment.ImageDate,selectedAttachment.ImageType);
			//FormCAIE.ShowDialog();
			//if(FormCAIE.DialogResult==DialogResult.OK) {//Update row
			//	_listImageAttachments[gridAttachedImages.GetSelectedIndex()]=FormCAIE.ImageAttachment;
			//	FillGrid();
			//}
		}

		private void ContextMenu_ItemClicked(object sender,ToolStripItemClickedEventArgs e) {
			//ToolStripItem item=e.ClickedItem;
			//if(item.Text=="Delete") {
			//	//Delete every selected row
			//	foreach(int selectedIndex in gridAttachedImages.SelectedIndices) {
			//		_listImageAttachments.RemoveAt(selectedIndex);
			//	}
			//	FillGrid();
			//}
		}

		///<summary>Sends every attachment in the grid to DentalXChange. Sets the claims attachmentID to
		///the response from Dentalxchange. Will also prompt the user to re-validate the claim.</summary>
		private void CreateAndSendAttachments() {
			////Grab all ImageAttachments from the grid.
			//List<ClaimConnect.ImageAttachment> listImagesToSend=new List<ClaimConnect.ImageAttachment>();
			//for(int i=0;i<gridAttachedImages.Rows.Count;i++) {
			//	listImagesToSend.Add((ClaimConnect.ImageAttachment)gridAttachedImages.Rows[i].Tag);
			//}
			//if(string.IsNullOrWhiteSpace(_claimCur.AttachmentID)) {
			//	//If an attachment has not already been created, create one.
			//	string attachmentId=ClaimConnect.CreateAttachment(listImagesToSend,textNarrative.Text,_claimCur);
			//	//Update claim if attachmentID was set. Must happen here so that the validation will consider the new attachmentID.
			//	_claimCur.AttachmentID=attachmentId;
			//	//Set the claims attached flag to 'Misc' so that the attachmentID will write to the PWK segment 
			//	//when the claim is generated as an 837.
			//	if(string.IsNullOrEmpty(_claimCur.AttachedFlags)) {
			//		_claimCur.AttachedFlags="Misc";
			//	}
			//	else {//Comma delimited
			//		_claimCur.AttachedFlags=",Misc";
			//	}
			//}
			//else {//An attachment already exists for this claim.
			//	ClaimConnect.AddAttachment(_claimCur,listImagesToSend);
			//}
			//Claims.Update(_claimCur);
		}

		///<summary>Saves all images in the grid to the patient on the claim's directory in the images module. Also creates
		///a list of ClaimAttach objects to associate to the given claim.</summary>
		private void buttonOK_Click(object sender,EventArgs e) {
			////Do not let the user continue if they haven't added any images to the grid.
			//if(gridAttachedImages.Rows.Count==0) {
			//	MsgBox.Show(this,"Add an image to be sent with the snipping tool or by attaching an existing file.");
			//	return;
			//}
			//try {
			//	CreateAndSendAttachments();
			//}
			//catch(ODException ex) {
			//	//ODExceptions should already be Lans.g when throwing meaningful messages.
			//	//If they weren't translated, the message was from a third party and shouldn't be translated anyway.
			//	MessageBox.Show(ex.Message);
			//	return;
			//}
			////Validate the claim, if it isn't valid let the user decide if they want to continue
			//if(!ValidateClaimHelper()) {
			//	if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"There were errors validating the claim, would you like to continue?")) {
			//		return;
			//	}
			//}
			////Used for determining which category to save the image attachments to. 0 will save the image to the first category in the Images module.
			//long imageTypeDefNum=0;
			//Definition defClaimAttachCat=CheckImageCatDefs().FirstOrDefault();
			//if(defClaimAttachCat!=null) {
			//	imageTypeDefNum=defClaimAttachCat.Id;
			//}
			//else {//User does not have a Claim Attachment image category, just use the first image category available.
			//	imageTypeDefNum= Definition.GetByCategory(DefinitionCategory.ImageCats).FirstOrDefault().Id;
			//}
			//List<ClaimAttach> listClaimAttachments=new List<ClaimAttach>();
			//for(int i=0;i<gridAttachedImages.Rows.Count;i++) {
			//	ClaimConnect.ImageAttachment imageRow=((ClaimConnect.ImageAttachment)gridAttachedImages.Rows[i].Tag);
			//	if(Preference.GetBool(PreferenceName.SaveDXCAttachments)) {
			//		Bitmap imageBitmap=new Bitmap(imageRow.Image);
			//		Document docCur=ImageStore.Import(imageBitmap,imageTypeDefNum,ImageType.Document,_claimPat);
			//		imageRow.ImageFileNameActual=docCur.FileName;
			//	}
			//	//Create attachment objects
			//	listClaimAttachments.Add(CreateClaimAttachment(imageRow.ImageFileNameDisplay,imageRow.ImageFileNameActual));
			//}
			////Keep a running list of attachments sent to DXC for the claim. This will show in the attachments listbox.
			//_claimCur.Attachments.AddRange(listClaimAttachments);
			//Claims.Update(_claimCur);
			DialogResult=DialogResult.OK;
		}

		private void buttonCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
