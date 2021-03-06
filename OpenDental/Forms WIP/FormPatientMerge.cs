using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using SLDental.Storage;

namespace OpenDental {
	public partial class FormPatientMerge:ODForm {
		///<summary>For display purposes only.  Reloaded from the db when merge actually occurs.</summary>
		private Patient _patTo;
		///<summary>For display purposes only.  Reloaded from the db when merge actually occurs.</summary>
		private Patient _patFrom;

		public FormPatientMerge() {
			InitializeComponent();
			
		}

		private void FormPatientMerge_Load(object sender,EventArgs e) {
		}

		private void butChangePatientInto_Click(object sender,EventArgs e) {
			FormPatientSelect fps=new FormPatientSelect();
			if(fps.ShowDialog()==DialogResult.OK){
				long selectedPatNum=fps.SelectedPatNum;//to prevent warning about marshal-by-reference
				textPatientIDInto.Text=selectedPatNum.ToString();
				_patTo=Patients.GetPat(selectedPatNum);
				textPatientNameInto.Text=_patTo.GetNameFLFormal();
				textPatToBirthdate.Text=_patTo.Birthdate.ToShortDateString();
			}
			CheckUIState();
		}

		private void butChangePatientFrom_Click(object sender,EventArgs e) {
			FormPatientSelect fps=new FormPatientSelect();
			if(fps.ShowDialog()==DialogResult.OK) {
				long selectedPatNum=fps.SelectedPatNum;//to prevent warning about marshal-by-reference
				textPatientIDFrom.Text=selectedPatNum.ToString();
				_patFrom=Patients.GetPat(selectedPatNum);
				textPatientNameFrom.Text=_patFrom.GetNameFLFormal();
				textPatFromBirthdate.Text=_patFrom.Birthdate.ToShortDateString();
			}
			CheckUIState();
		}

		private void CheckUIState(){
			butMerge.Enabled=(_patTo!=null && _patFrom!=null);
		}

        private void butMerge_Click(object sender, EventArgs e)
        {
            if (_patTo.PatNum == _patFrom.PatNum)
            {
                MsgBox.Show(this, "Cannot merge a patient account into itself. Please select a different patient to merge from.");
                return;
            }
            string msgText = "";
            if (_patFrom.FName.Trim().ToLower() != _patTo.FName.Trim().ToLower()
                || _patFrom.LName.Trim().ToLower() != _patTo.LName.Trim().ToLower()
                || _patFrom.Birthdate != _patTo.Birthdate)
            {//mismatch
                msgText = Lan.g(this, "The two patients do not have the same first name, last name, and birthdate.");

                msgText += "\r\n\r\n"
                    + Lan.g(this, "Into patient name") + ": " + Patients.GetNameFLnoPref(_patTo.LName, _patTo.FName, "") + ", "//using Patients.GetNameFLnoPref to omit MiddleI
                    + Lan.g(this, "Into patient birthdate") + ": " + _patTo.Birthdate.ToShortDateString() + "\r\n"
                    + Lan.g(this, "From patient name") + ": " + Patients.GetNameFLnoPref(_patFrom.LName, _patFrom.FName, "") + ", "//using Patients.GetNameFLnoPref to omit MiddleI
                    + Lan.g(this, "From patient birthdate") + ": " + _patFrom.Birthdate.ToShortDateString() + "\r\n\r\n"
                    + Lan.g(this, "Merge the patient on the bottom into the patient shown on the top?");
                if (MessageBox.Show(msgText, "", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;//The user chose not to merge
                }
            }
            else
            {//name and bd match
                if (!MsgBox.Show(this, MsgBoxButtons.YesNo, "Merge the patient at the bottom into the patient shown at the top?"))
                {
                    return;//The user chose not to merge.
                }
            }
            if (_patFrom.PatNum == _patFrom.Guarantor)
            {
                Family fam = Patients.GetFamily(_patFrom.PatNum);
                if (fam.Members.Length > 1)
                {
                    msgText = Lan.g(this, "The patient you have chosen to merge from is a guarantor.  Merging this patient into another account will cause all "
                        + "family members of the patient being merged from to be moved into the same family as the patient account being merged into.") + "\r\n"
                        + Lan.g(this, "Do you wish to continue with the merge?");
                    if (MessageBox.Show(msgText, "", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;//The user chose not to merge.
                    }
                }
            }
            Cursor = Cursors.WaitCursor;
            List<Task> listPatientTasks = Tasks.RefreshPatientTickets(_patFrom.PatNum);//Get this before the merge, because the merge updates Task.KeyNum.
            bool isSuccessfulMerge = false;
            try
            {
                isSuccessfulMerge = Patients.MergeTwoPatients(_patTo.PatNum, _patFrom.PatNum);
            }
            catch (Exception ex)
            {
                SecurityLog.Write(Permissions.PatientMerge, _patTo.PatNum,
                    "Error occurred while merging Patient: " + _patFrom.GetNameFL() + "\r\nPatNum From: " + _patFrom.PatNum + "\r\nPatNum To: " + _patTo.PatNum);
                Cursor = Cursors.Default;
                FormFriendlyException.Show(Lan.g(this, "Unable to fully merge patients.  Contact support."), ex);
            }
            if (isSuccessfulMerge)
            {
                //The patient has been successfully merged.
                #region Refresh Patient's Tasks
                //List<Signal> listSignals = new List<Signal>();
                //foreach (Task task in listPatientTasks)
                //{
                //    Signal signal = new Signal();
                //    signal.IType = InvalidType.Task;
                //    signal.FKeyType = KeyType.Task;
                //    signal.ExternalId = task.TaskNum;
                //    signal.DateViewing = DateTime.MinValue;//Mimics Signalods.SetInvalid()
                //    listSignals.Add(signal);
                //}
                //Signalods.SetInvalid(InvalidType.TaskPatient, KeyType.Undefined, _patTo.PatNum);//Ensure anyone viewing Patient tab of new pat gets refreshed.
                //Signalods.Insert(listSignals.ToArray());//Refreshes existing tasks in all other tabs.
                //                                        //Causes Task area and open Task Edit windows to refresh immediately.  No popups, alright to pass empty lists for listRefreshedTaskNotes and 
                //                                        //listBlockedTaskLists.
                //FormOpenDental.S_HandleRefreshedTasks(listSignals, listPatientTasks.Select(x => x.TaskNum).ToList(), listPatientTasks, new List<TaskNote>()
                //    , new List<UserPreference>());
                #endregion
                //Now copy the physical images from the old patient to the new if they are using an AtoZ image share.
                //This has to happen in the UI because the middle tier server might not have access to the image share.
                //If the users are storing images within the database, those images have already been taken care of in the merge method above.
                int fileCopyFailures = 0;

                #region Copy AtoZ Documents
                //Move the patient documents within the 'patFrom' A to Z folder to the 'patTo' A to Z folder.
                //We have to be careful here of documents with the same name. We have to rename such documents
                //so that no documents are overwritten/lost.
                string atozFrom = ImageStore.GetPatientFolder(_patFrom);
                string atozTo = ImageStore.GetPatientFolder(_patTo);
                string[] fromFiles = Directory.GetFiles(atozFrom);
                if (atozFrom == atozTo)
                {
                    //Very rarely, two patients have the same image folder.  PatFrom and PatTo both have Documents that point to the same file.  Since we 
                    //are about to copy the image file for PatFrom to PatTo's directory and delete the file from PatFrom's directory, we would break the 
                    //file reference for PatTo's Document.  In this case, skip deleting the original file, since PatTo's Document still references it.
                    Documents.MergePatientDocuments(_patFrom.PatNum, _patTo.PatNum);
                }
                else
                {
                    foreach (string fileCur in fromFiles)
                    {
                        string fileName = Path.GetFileName(fileCur);
                        string destFileName = fileName;
                        string destFilePath = Storage.Default.CombinePath(atozTo, fileName);
                        if (Storage.Default.FileExists(destFilePath))
                        {
                            //The file being copied has the same name as a possibly different file within the destination a to z folder.
                            //We need to copy the file under a unique file name and then make sure to update the document table to reflect
                            //the change.
                            destFileName = _patFrom.PatNum.ToString() + "_" + fileName;
                            destFilePath = Storage.Default.CombinePath(atozTo, destFileName);
                            while (Storage.Default.FileExists(destFilePath))
                            {
                                destFileName = _patFrom.PatNum.ToString() + "_" + fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss");
                                destFilePath = Storage.Default.CombinePath(atozTo, destFileName);
                            }
                        }
                        try
                        {
                            Storage.Default.CopyFile(fileCur, destFilePath);
                        }
                        catch
                        {

                            fileCopyFailures++;
                            continue;//copy failed, increment counter and move onto the next file
                        }
                        //If the copy did not fail, try to delete the old file.
                        //We can now safely update the document FileName and PatNum to the "to" patient.
                        Documents.MergePatientDocument(_patFrom.PatNum, _patTo.PatNum, fileName, destFileName);
                        try
                        {
                            File.Delete(fileCur);
                        }
                        catch
                        {

                            //If we were unable to delete the file then it is probably because someone has the document open currently.
                            //Just skip deleting the file. This means that occasionally there will be an extra file in their backup
                            //which is just clutter but at least the merge is guaranteed this way.
                        }
                    }
                }
                #endregion Copy AtoZ Documents


                Cursor = Cursors.Default;
                if (fileCopyFailures > 0)
                {
                    MessageBox.Show(Lan.g(this, "Some files belonging to the from patient were not copied.") + "\r\n"
                        + Lan.g(this, "Number of files not copied") + ": " + fileCopyFailures);
                }
                textPatientIDFrom.Text = "";
                textPatientNameFrom.Text = "";
                textPatFromBirthdate.Text = "";
                CheckUIState();
                //Make log entry here not in parent form because we can merge multiple patients at a time.
                SecurityLog.Write(Permissions.PatientMerge, _patTo.PatNum,
                    "Patient: " + _patFrom.GetNameFL() + "\r\nPatNum From: " + _patFrom.PatNum + "\r\nPatNum To: " + _patTo.PatNum);
                MsgBox.Show(this, "Patients merged successfully.");
            }//end MergeTwoPatients
            Cursor = Cursors.Default;
        }

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	}
}