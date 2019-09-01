using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {

	public partial class FormMobileAppDevices : ODForm {

		#region Private Variables

		///<summary>A list of all mobile app devices in the database..</summary>
		List<MobileAppDevice> _listAllMobileDevicesDb;
		///<summary>A list of all mobile app devices that are edited in this window. Synced with the database list on the ok click.</summary>
		List<MobileAppDevice> _listAllMobileDevicesNew;

		#endregion

		#region Properties

		///<summary>Returns the index in the main grid of the "Enabled" column.</summary>
		private int _indexOfEnabledColumn {
			get {
				return (Preferences.HasClinicsEnabled ? 4 : 3);
			}
		}

		#endregion

		#region Ctor

		public FormMobileAppDevices() {
			InitializeComponent();
			
		}

		#endregion

		#region UI Events

		private void FormMobileAppDevices_Load(object sender,EventArgs e) {
			SetFilterControlsAndAction(() => FillGrid(),comboClinic);
			comboClinic.SelectedClinicNum=Clinics.ClinicNum;
			_listAllMobileDevicesDb=MobileAppDevices.GetAll();
			//Make a deep copy of the database list.
			_listAllMobileDevicesNew=_listAllMobileDevicesDb.Select(x => x.Copy()).ToList();
			FillGrid();
			labelClinic.Visible=Preferences.HasClinicsEnabled;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			MobileAppDevice device=gridMain.SelectedTag<MobileAppDevice>();
			if(device==null) {
				MsgBox.Show("Please select a device first.");
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"Are you sure you want to delete this device?")) {
				return;
			}
			_listAllMobileDevicesNew.Remove(device);
			FillGrid();
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			//They did not select the right column.
			if(e.Col!=_indexOfEnabledColumn) {
				return;
			}
			MobileAppDevice device=gridMain.SelectedTag<MobileAppDevice>();
			//There is not a tag somehow.
			if(device==null) {
				return;
			}
			//Flip the bit.
			device.IsAllowed=!device.IsAllowed;
			//Fill the grid to show the changes.
			FillGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			MobileAppDevices.Sync(_listAllMobileDevicesNew,_listAllMobileDevicesDb);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		#endregion

		#region Helper Methods

		///<summary>Fills the main grid. Uses the classwide list to fill the specific clinics devices (or all).</summary>
		private void FillGrid() {
			gridMain.BeginUpdate();
			if(gridMain.Columns.Count==0) {
				ODGridColumn col;
				col=new ODGridColumn(Lan.g(this,"Device Name"),125);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Last Attempt"),135);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Last Login"),135);
				gridMain.Columns.Add(col);
				if(Preferences.HasClinicsEnabled) {
					col=new ODGridColumn(Lan.g(this,"Clinic"),150);
					gridMain.Columns.Add(col);
				}
				col=new ODGridColumn(Lan.g(this,"Enabled"),50,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
			}
			gridMain.Rows.Clear();
			List<MobileAppDevice> listDevicesToShow;
			if(!Preferences.HasClinicsEnabled || (Preferences.HasClinicsEnabled && comboClinic.IsAllSelected)) {
				//No clinics are enabled or all is selected. Show all.
				listDevicesToShow=_listAllMobileDevicesNew;
			}
			else {
				//Otherwise, find the devices that match the clinic num.
				listDevicesToShow=_listAllMobileDevicesNew.Where(x => x.ClinicNum==comboClinic.SelectedClinicNum).ToList();
			}
			foreach(MobileAppDevice device in listDevicesToShow) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(device.DeviceName);
				row.Cells.Add((device.LastAttempt.Year > 1880 ? device.LastAttempt.ToString() : ""));
				row.Cells.Add((device.LastLogin.Year > 1880 ? device.LastLogin.ToString() : ""));
				if(Preferences.HasClinicsEnabled) {
					row.Cells.Add((device.ClinicNum==0 ? Clinics.GetPracticeAsClinicZero() : Clinics.GetClinic(device.ClinicNum)).Abbr);
				}
				row.Cells.Add((device.IsAllowed ? "X" : ""));
				row.Tag=device;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		#endregion

	}

}