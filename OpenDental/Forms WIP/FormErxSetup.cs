using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using OpenDental.UI;

namespace OpenDental {
	///<summary>This form is used to make changes for the eRx program link.
	///With the integration of DoseSpot, the default program link form is no longer sufficient.</summary>
	public partial class FormErxSetup:ODForm {

		private Program _progCur;
		private ErxOption _eRxOption;
		private List<ProgramProperty> _listProgramProperties=new List<ProgramProperty>();

		private ProgramProperty ErxOptionPP {
			get {
				ProgramProperty retVal=_listProgramProperties.FirstOrDefault(x => x.Key==Erx.PropertyDescs.ErxOption);
				if(retVal==null) {
					throw new Exception("The database is missing an eRx option program property.");
				}
				return retVal;
			}
			set {
				int pos=_listProgramProperties.IndexOf(ErxOptionPP);
				_listProgramProperties[pos]=value;
			}
		}

		public FormErxSetup() {
			InitializeComponent();
			
			//We only show the tabs in the designer for development purposes.  We want to hide them for our users.
			//Because the tab control is in "flat buttons" appearance and "fixed size" style the tabs will not show even if they are one pixel tall.
			//0,0 does not work because some size is required.
			tabControlErxSoftware.ItemSize=new Size(0,1);
		}

		private void FormErxSetup_Load(object sender,EventArgs e) {
			try {
				_progCur=Programs.GetCur(ProgramName.eRx);
				if(_progCur==null) {
					throw new Exception("The eRx bridge is missing from the database.");
				}
				_listProgramProperties=ProgramProperties.GetForProgram(_progCur.ProgramNum);
				checkEnabled.Checked=_progCur.Enabled;
				_eRxOption=PIn.Enum<ErxOption>(ErxOptionPP.Value);
				if(_eRxOption==ErxOption.Legacy) {
					radioNewCrop.Checked=true;
				}
				else if(_eRxOption==ErxOption.DoseSpot) {
					radioDoseSpot.Checked=true;
					//HideLegacy();
				}
				else if(_eRxOption==ErxOption.DoseSpotWithLegacy) {
					radioDoseSpotLegacy.Checked=true;
					//HideLegacy();
				}
				textNewCropAccountID.Text=Preference.GetString(PreferenceName.NewCropAccountId);
				List<ProgramProperty> listClinicIDs=_listProgramProperties.FindAll(x => x.Key==Erx.PropertyDescs.ClinicID);
				List<ProgramProperty> listClinicKeys=_listProgramProperties.FindAll(x => x.Key==Erx.PropertyDescs.ClinicKey);
				//Always make sure clinicnum 0 (HQ) exists, regardless of if clinics are enabled
				if(!listClinicIDs.Exists(x => x.ClinicId==0)) {
					ProgramProperty ppClinicID=new ProgramProperty();
					ppClinicID.ProgramId=_progCur.ProgramNum;
					ppClinicID.ClinicId=0;
					ppClinicID.Key=Erx.PropertyDescs.ClinicID;
					ppClinicID.Value="";
					_listProgramProperties.Add(ppClinicID);
				}
				if(!listClinicKeys.Exists(x => x.ClinicId==0)) {
					ProgramProperty ppClinicKey=new ProgramProperty();
					ppClinicKey.ProgramId=_progCur.ProgramNum;
					ppClinicKey.ClinicId=0;
					ppClinicKey.Key=Erx.PropertyDescs.ClinicKey;
					ppClinicKey.Value="";
					_listProgramProperties.Add(ppClinicKey);
				}

					foreach(Clinic clinicCur in Clinic.GetByUser(Security.CurrentUser, true)) {
						if(!listClinicIDs.Exists(x => x.ClinicId==clinicCur.Id)) {//Only add a program property if it doesn't already exist.
							ProgramProperty ppClinicID=new ProgramProperty();
							ppClinicID.ProgramId=_progCur.ProgramNum;
							ppClinicID.ClinicId=clinicCur.Id;
							ppClinicID.Key=Erx.PropertyDescs.ClinicID;
							ppClinicID.Value="";
							_listProgramProperties.Add(ppClinicID);
						}
						if(!listClinicKeys.Exists(x => x.ClinicId==clinicCur.Id)) {//Only add a program property if it doesn't already exist.
							ProgramProperty ppClinicKey=new ProgramProperty();
							ppClinicKey.ProgramId=_progCur.ProgramNum;
							ppClinicKey.ClinicId=clinicCur.Id;
							ppClinicKey.Key=Erx.PropertyDescs.ClinicKey;
							ppClinicKey.Value="";
							_listProgramProperties.Add(ppClinicKey);
						}
					}

				FillGridDoseSpot();
				SetRadioButtonChecked(_eRxOption);
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Error loading the eRx program: ")+ex.Message);
				DialogResult=DialogResult.Cancel;
				return;
			}
		}

        private void FillGridDoseSpot()
        {
            gridProperties.BeginUpdate();
            gridProperties.Columns.Clear();
            ODGridColumn col = new ODGridColumn(Lan.g(this, "Clinic"), 120);
            gridProperties.Columns.Add(col);
            col = new ODGridColumn(Lan.g(this, "Clinic ID"), 160);
            gridProperties.Columns.Add(col);
            col = new ODGridColumn(Lan.g(this, "Clinic Key"), 160);
            gridProperties.Columns.Add(col);
            gridProperties.Rows.Clear();
            DoseSpotGridRowModel clinicHqModel = new DoseSpotGridRowModel();
            clinicHqModel.Clinic = new Clinic();
            clinicHqModel.Clinic.Abbr = Lan.g(this, "Headquarters");
            clinicHqModel.ClinicIDProperty = GetPropertyForClinic(0, Erx.PropertyDescs.ClinicID);
            clinicHqModel.ClinicKeyProperty = GetPropertyForClinic(0, Erx.PropertyDescs.ClinicKey);
            gridProperties.Rows.Add(CreateDoseSpotGridRow(clinicHqModel));//If clinics isn't enabled, this will be the only row in the grid.

            foreach (Clinic clinicCur in Clinic.GetByUser(Security.CurrentUser, true))
            {
                if (!checkShowHiddenClinics.Checked && clinicCur.IsHidden)
                {
                    continue;
                }
                DoseSpotGridRowModel model = new DoseSpotGridRowModel();
                model.Clinic = clinicCur;
                model.ClinicIDProperty = GetPropertyForClinic(clinicCur.Id, Erx.PropertyDescs.ClinicID);
                model.ClinicKeyProperty = GetPropertyForClinic(clinicCur.Id, Erx.PropertyDescs.ClinicKey);
                gridProperties.Rows.Add(CreateDoseSpotGridRow(model));
            }

            gridProperties.EndUpdate();
        }

		private ODGridRow CreateDoseSpotGridRow(DoseSpotGridRowModel model) {
			ODGridRow row=new ODGridRow();
			row.Cells.Add(model.Clinic.Abbr);
			row.Cells.Add(model.ClinicIDProperty==null ? "" : model.ClinicIDProperty.Value);
			row.Cells.Add(model.ClinicKeyProperty==null ? "" : model.ClinicKeyProperty.Value);
			row.Tag=model;
			return row;
		}

		private ProgramProperty GetPropertyForClinic(long clinicNum,string propDesc) {
			return _listProgramProperties.FindAll(x => x.ClinicId==clinicNum)
					.FirstOrDefault(x => x.Key==propDesc);
		}

		///<summary>All references removed in I12045.</summary>
		private void HideLegacy() {
			radioNewCrop.Visible=false;
			radioDoseSpotLegacy.Location=radioDoseSpot.Location;
			radioDoseSpot.Location=radioNewCrop.Location;
			groupErxOptions.Size=new Size(groupErxOptions.Size.Width,radioDoseSpotLegacy.Location.Y+radioDoseSpotLegacy.Height+5);
		}

		private void SetRadioButtonChecked(ErxOption option) {
			_eRxOption=option;
			if(option==ErxOption.Legacy) {
				tabControlErxSoftware.SelectedTab=tabNewCrop;
			}
			else {
				//This will also display the DoseSpot tab if DoseSpotWithLegacy is checked.
				//This is important because the user is migrating away from Legacy to use DoseSpot.
				//Plus, the user cannot do anything in the NewCrop tab.
				tabControlErxSoftware.SelectedTab=tabDoseSpot;
			}
		}

		private void checkShowHiddenClinics_CheckedChanged(object sender,EventArgs e) {
			FillGridDoseSpot();
		}

		private void radioNewCrop_Click(object sender,EventArgs e) {
			SetRadioButtonChecked(ErxOption.Legacy);
		}

		private void radioDoseSpot_Click(object sender,EventArgs e) {
			MsgBox.Show(this,"This enables the DoseSpot program link only.  You must contact support to cancel current eRx Legacy charges and sign up for DoseSpot.");
			SetRadioButtonChecked(ErxOption.DoseSpot);
		}

		private void radioDoseSpotLegacy_Click(object sender,EventArgs e) {
			MsgBox.Show(this,"This enables the DoseSpot program link only.  You must contact support to cancel current eRx Legacy charges and sign up for DoseSpot.");
			SetRadioButtonChecked(ErxOption.DoseSpotWithLegacy);
		}

		private void gridProperties_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			DoseSpotGridRowModel model=(DoseSpotGridRowModel)gridProperties.Rows[e.Row].Tag;
			FormDoseSpotPropertyEdit FormDPE=new FormDoseSpotPropertyEdit(model.Clinic,model.ClinicIDProperty.Value,model.ClinicKeyProperty.Value,_listProgramProperties);
			FormDPE.ShowDialog();
			if(FormDPE.DialogResult==DialogResult.OK) {
				int clinicIdPos=_listProgramProperties.IndexOf(GetPropertyForClinic(model.Clinic.Id,Erx.PropertyDescs.ClinicID));
				_listProgramProperties[clinicIdPos].Value=FormDPE.ClinicIdVal;
				int clinicKeyPos=_listProgramProperties.IndexOf(GetPropertyForClinic(model.Clinic.Id,Erx.PropertyDescs.ClinicKey));
				_listProgramProperties[clinicKeyPos].Value=FormDPE.ClinicKeyVal;
			}
			FillGridDoseSpot();//Always fill grid because clinics could have been editted in FormDoseSpotPropertyEdit.
		}

		private void butOK_Click(object sender,EventArgs e) {
			ErxOptionPP.Value=POut.Int((int)_eRxOption);
			_progCur.Enabled=checkEnabled.Checked;
			Programs.Update(_progCur);
			//ProgramProperties.Sync(_listProgramProperties,_progCur.ProgramNum);
			DataValid.SetInvalid(InvalidType.Programs);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private class DoseSpotGridRowModel {
			public Clinic Clinic;
			public ProgramProperty ClinicIDProperty;
			public ProgramProperty ClinicKeyProperty;

			public DoseSpotGridRowModel() {
			}
		}
	}
}