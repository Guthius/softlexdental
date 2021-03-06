﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using System.Xml;
using OpenDental.UI;
using SLDental.Storage;

namespace OpenDental {
	public partial class FormEhrClinicalSummary:ODForm {
		public Patient PatCur;
		private List<EhrMeasureEvent> summariesSentList;

		public FormEhrClinicalSummary() {
			InitializeComponent();
		}

		private void FormClinicalSummary_Load(object sender,EventArgs e) {
			FillGridEHRMeasureEvents();
		}

		private void FillGridEHRMeasureEvents() {
			gridEHRMeasureEvents.BeginUpdate();
			gridEHRMeasureEvents.Columns.Clear();
			ODGridColumn col = new ODGridColumn("DateTime",140);
			gridEHRMeasureEvents.Columns.Add(col);
			//col = new ODGridColumn("Details",600);
			//gridEHRMeasureEvents.Columns.Add(col);
			summariesSentList = EhrMeasureEvents.RefreshByType(PatCur.PatNum,EhrMeasureEventType.ClinicalSummaryProvidedToPt);
			gridEHRMeasureEvents.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<summariesSentList.Count;i++) {
				row = new ODGridRow();
				row.Cells.Add(summariesSentList[i].DateTEvent.ToString());
				//row.Cells.Add(summariesSentList[i].EventType.ToString());
				gridEHRMeasureEvents.Rows.Add(row);
			}
			gridEHRMeasureEvents.EndUpdate();
		}

		private void butExport_Click(object sender,EventArgs e) {
			string ccd="";
			try {
				FormEhrExportCCD FormEEC=new FormEhrExportCCD(PatCur);
				FormEEC.ShowDialog();
				if(FormEEC.DialogResult==DialogResult.OK) {
					ccd=FormEEC.CCD;
				}
				else {
					return;
				}
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			FolderBrowserDialog dlg=new FolderBrowserDialog();
			dlg.SelectedPath=ImageStore.GetPatientFolder(PatCur);//Default to patient image folder.
			DialogResult result=dlg.ShowDialog();
			if(result!=DialogResult.OK) {
				return;
			}
			if(File.Exists(Path.Combine(dlg.SelectedPath,"ccd.xml"))){
				if(MessageBox.Show("Overwrite existing ccd.xml?","",MessageBoxButtons.OKCancel)!=DialogResult.OK){
					return;
				}
			}
			File.WriteAllText(Path.Combine(dlg.SelectedPath,"ccd.xml"),ccd);
			File.WriteAllText(Path.Combine(dlg.SelectedPath,"ccd.xsl"),FormEHR.GetEhrResource("CCD"));
			EhrMeasureEvent newMeasureEvent = new EhrMeasureEvent();
			newMeasureEvent.DateTEvent = DateTime.Now;
			newMeasureEvent.EventType = EhrMeasureEventType.ClinicalSummaryProvidedToPt;
			newMeasureEvent.PatNum = PatCur.PatNum;
			EhrMeasureEvents.Insert(newMeasureEvent);
			FillGridEHRMeasureEvents();
			MessageBox.Show("Exported");	
		}

		private void butSendToPortal_Click(object sender,EventArgs e) {
			//Validate
			string strCcdValidationErrors=EhrCCD.ValidateSettings();
			if(strCcdValidationErrors!="") {//Do not even try to export if global settings are invalid.
				MessageBox.Show(strCcdValidationErrors);//We do not want to use translations here, because the text is dynamic. The errors are generated in the business layer, and Lan.g() is not available there.
				return;
			}
			strCcdValidationErrors=EhrCCD.ValidatePatient(PatCur);//Patient cannot be null, because a patient must be selected before the EHR dashboard will open.
			if(strCcdValidationErrors!="") {
				MessageBox.Show(strCcdValidationErrors);//We do not want to use translations here, because the text is dynamic. The errors are generated in the business layer, and Lan.g() is not available there.
				return;
			}
			Provider prov=null;
			if(Security.CurrentUser.ProviderId.HasValue) {//If the current user is a provider.
				prov=Provider.GetById(Security.CurrentUser.ProviderId.Value);
			}
			else {
				prov=Provider.GetById(PatCur.PriProv);//PriProv is not 0, because EhrCCD.ValidatePatient() will block if PriProv is 0.
			}
			try {
				//Create the Clinical Summary.
				FormEhrExportCCD FormEEC=new FormEhrExportCCD(PatCur);
				FormEEC.ShowDialog();
				if(FormEEC.DialogResult!=DialogResult.OK) {//Canceled
					return;
				}
				//Save the clinical summary (ccd.xml) and style sheet (ccd.xsl) as webmail message attachments.
				//TODO: It would be more patient friendly if we instead generated a PDF file containing the Clinical Summary printout, or if we simply displayed the Clinical Summary in the portal.
				//The CMS definition does not prohibit sending human readable files, and sending a PDF to the portal mimics printing the Clinical Summary and handing to patient.
				Random rnd=new Random();
				string attachPath=EmailAttachment.GetAttachmentPath();
				List<EmailAttachment> listAttachments=new List<EmailAttachment>();
				EmailAttachment attachCcd=new EmailAttachment();//Save Clinical Summary to file in the email attachments folder.
				attachCcd.Description="ccd.xml";
				attachCcd.FileName=DateTime.Now.ToString("yyyyMMdd")+"_"+DateTime.Now.TimeOfDay.Ticks.ToString()+rnd.Next(1000).ToString()+".xml";
				listAttachments.Add(attachCcd);
                Storage.Default.WriteAllText(Storage.Default.CombinePath(attachPath,attachCcd.FileName),FormEEC.CCD);            // TODO: Status = "Uploading Attachment for Clinical Summary..."
                EmailAttachment attachSs=new EmailAttachment();//Style sheet attachment.
				attachSs.Description="ccd.xsl";
				attachSs.FileName=attachCcd.FileName.Substring(0,attachCcd.FileName.Length-4)+".xsl";//Same base name as the CCD.  The base names must match or the file will not display properly in internet browsers.
				listAttachments.Add(attachSs);
                Storage.Default.WriteAllText(Storage.Default.CombinePath(attachPath,attachSs.FileName),FormEHR.GetEhrResource("CCD")); // TODO: Status = "Uploading Attachment for Clinical Summary..."
                                                                                                                                       //Create and save the webmail message containing the attachments.
                EmailMessage msgWebMail=new EmailMessage();				
				msgWebMail.FromAddress=prov.GetFormalName();
				msgWebMail.ToAddress=PatCur.GetNameFL();
				msgWebMail.PatientId=PatCur.PatNum;
				msgWebMail.Status=EmailMessageStatus.Sent; // WebMailSent
				msgWebMail.ProviderId=prov.Id;
				msgWebMail.Subject="Clinical Summary";
				msgWebMail.Body="To view the clinical summary:\r\n1) Download all attachments to the same folder.  Do not rename the files.\r\n2) Open the ccd.xml file in an internet browser.";
				msgWebMail.Date=DateTime.Now;
				msgWebMail.Attachments=listAttachments;
				EmailMessage.Insert(msgWebMail);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			EhrMeasureEvent newMeasureEvent=new EhrMeasureEvent();
			newMeasureEvent.DateTEvent=DateTime.Now;
			newMeasureEvent.EventType=EhrMeasureEventType.ClinicalSummaryProvidedToPt;
			newMeasureEvent.PatNum=PatCur.PatNum;
			EhrMeasureEvents.Insert(newMeasureEvent);
			FillGridEHRMeasureEvents();//This will cause the measure event to show in the grid below the popup message on the next line.  Reassures the user that the event was immediately recorded.
			MsgBox.Show(this,"Clinical Summary Sent");
		}

		private void butShowXhtml_Click(object sender,EventArgs e) {
			string ccd="";
			try {
				FormEhrExportCCD FormEEC=new FormEhrExportCCD(PatCur);
				FormEEC.ShowDialog();
				if(FormEEC.DialogResult==DialogResult.OK) {
					ccd=FormEEC.CCD;
				}
				else {
					return;
				}
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			bool didPrint=FormEhrSummaryOfCare.DisplayCCD(ccd);
			if(didPrint) {
				//we are printing a ccd so add new measure event.					
				EhrMeasureEvent measureEvent = new EhrMeasureEvent();
				measureEvent.DateTEvent = DateTime.Now;
				measureEvent.EventType = EhrMeasureEventType.ClinicalSummaryProvidedToPt;
				measureEvent.PatNum = PatCur.PatNum;
				EhrMeasureEvents.Insert(measureEvent);
				FillGridEHRMeasureEvents();
			}		
		}

		private void butShowXml_Click(object sender,EventArgs e) {
			string ccd="";
			try {
				FormEhrExportCCD FormEEC=new FormEhrExportCCD(PatCur);
				FormEEC.ShowDialog();
				if(FormEEC.DialogResult==DialogResult.OK) {
					ccd=FormEEC.CCD;
				}
				else {
					return;
				}
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(ccd);
			msgbox.ShowDialog();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(gridEHRMeasureEvents.SelectedIndices.Length < 1) {
				MessageBox.Show("Please select at least one record to delete.");
				return;
			}
			for(int i=0;i<gridEHRMeasureEvents.SelectedIndices.Length;i++) {
				EhrMeasureEvents.Delete(summariesSentList[gridEHRMeasureEvents.SelectedIndices[i]].EhrMeasureEventNum);
			}
			FillGridEHRMeasureEvents();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	

		











	}
}
