using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormArManager:ODForm {
		private List<Provider> _listProviders;
		private List<Clinic> _listClinics;
		private List<Definition> _listBillTypesNoColl;
		private Definition _collectionBillType;
		private List<PatAging> _listPatAgingUnsentAll;
		private List<PatAging> _listPatAgingSentAll;
		private List<TsiTransType> _listNewStatuses;
		private List<TsiTransType> _listSentTabTransTypes;
		private bool _isResizing;
		private bool _hasResizeBegan;
		///<summary>Used to reselect rows after sorting the grid by column.  Filled on grid MouseDown event and used in grid OnSortByColumn event to reselect.</summary>
		private List<long> _listSelectedPatNums;
		private Program _tsiProg;
		private Dictionary<long,List<ProgramProperty>> _dictClinicProgProps;
		private ToolTip _toolTipUnsentErrors;
		private Point _lastCursorPos;

		public FormArManager() {
			InitializeComponent();
			
		}

		private void FormArManager_Load(object sender,EventArgs e) {
			#region Get Variables for Both Tabs
			//List<Definition> billTypeDefs=Definition.GetByCategory(DefinitionCategory.BillingTypes);
			//_collectionBillType=billTypeDefs.FirstOrDefault(x => x.Value.ToLower()=="c")?.Copy();
			//_listBillTypesNoColl=billTypeDefs.Where(x => x.Value.ToLower()!="c").Select(x => x.Copy()).ToList();
			//_listClinics=new List<Clinic>();
			//if(Preferences.HasClinicsEnabled) {
			//	_listClinics.AddRange(Clinics.GetForUserod(Security.CurrentUser,true,Lan.g(this,"Unassigned")).OrderBy(x => x.ClinicNum!=0).ThenBy(x => x.ItemOrder));
			//}
			//else {//clinics disabled
			//	_listClinics.Add(Clinics.GetPracticeAsClinicZero(Lan.g(this,"Unassigned")));
			//}
			//_listProviders=Providers.GetDeepCopy(true);
			//_tsiProg=Programs.GetCur(ProgramName.Transworld);
			//_dictClinicProgProps=new Dictionary<long,List<ProgramProperty>>();
			//if(_tsiProg!=null && _tsiProg.Enabled) {
			//	_dictClinicProgProps=ProgramProperties.GetForProgram(_tsiProg.ProgramNum)
			//		.FindAll(x => _listClinics.Any(y => y.ClinicNum==x.ClinicId))//will contain the HQ "clinic" if clinics are disabled or user unrestricted
			//		.GroupBy(x => x.ClinicId).ToDictionary(x => x.Key,x => x.ToList());
			//}
			//_toolTipUnsentErrors=new ToolTip() { InitialDelay=1000,ReshowDelay=1000,ShowAlways=true };
			//_lastCursorPos=gridUnsent.PointToClient(Cursor.Position);
			#endregion Get Variables for Both Tabs
			#region Fill Unsent Tab Filter ComboBoxes, CheckBoxes, and Fields
			#region Unsent Tab Clinic Combo
			if(Preferences.HasClinicsEnabled) {
				comboBoxMultiUnsentClinics.Visible=true;
				labelUnsentClinics.Visible=true;
				comboBoxMultiSentClinics.Visible=true;
				labelSentClinics.Visible=true;
				comboBoxMultiUnsentClinics.Items.Add(Lan.g(this,"All"));
				comboBoxMultiSentClinics.Items.Add(Lan.g(this,"All"));
				if(Clinics.ClinicNum==0) {
					comboBoxMultiUnsentClinics.SetSelected(0,true);
					comboBoxMultiSentClinics.SetSelected(0,true);
				}
				foreach(Clinic clinCur in _listClinics) {
					comboBoxMultiUnsentClinics.Items.Add(clinCur.Abbr);
					comboBoxMultiSentClinics.Items.Add(clinCur.Abbr);
					if(Clinics.ClinicNum>0 && clinCur.ClinicNum==Clinics.ClinicNum) {
						comboBoxMultiUnsentClinics.SetSelected(comboBoxMultiUnsentClinics.Items.Count-1,true);
						comboBoxMultiSentClinics.SetSelected(comboBoxMultiSentClinics.Items.Count-1,true);
					}
				}
				if(comboBoxMultiUnsentClinics.SelectedIndices.Count==0) {
					comboBoxMultiUnsentClinics.SetSelected(0,true);
					comboBoxMultiSentClinics.SetSelected(0,true);//if unsent clinic combo has 0 selected, so will sent clinic combo
				}
			}
			#endregion Unsent Tab Clinic Combo
			#region Unsent Tab Prov Combo
			comboBoxMultiUnsentProvs.Items.Add("All");
			comboBoxMultiUnsentProvs.SetSelected(0,true);
			_listProviders.ForEach(x => comboBoxMultiUnsentProvs.Items.Add(x.GetLongDesc()));
			#endregion Unsent Tab Prov Combo
			#region Unsent Tab Bill Type Combo
			List<long> listDefaultBillTypes=Preference.GetString(PreferenceName.ArManagerBillingTypes)
				.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries)
				.Select(x => PIn.Long(x)).ToList();
			comboBoxMultiBillTypes.Items.Add("All");
			comboBoxMultiBillTypes.SetSelected(0,_listBillTypesNoColl.All(x => !listDefaultBillTypes.Contains(x.Id)));//select All if no valid defaults are set
			for(int i=0;i<_listBillTypesNoColl.Count;i++) {
				comboBoxMultiBillTypes.Items.Add(_listBillTypesNoColl[i].Description);
				if(listDefaultBillTypes.Contains(_listBillTypesNoColl[i].Id)) {
					comboBoxMultiBillTypes.SetSelected(i+1,true);//+1 for All
				}
			}
			#endregion Unsent Tab Bill Type Combo
			#region Unsent Tab Account Age Combo
			comboUnsentAccountAge.Items.Add(Lan.g(this,"Any Balance"));
			comboUnsentAccountAge.Items.Add(Lan.g(this,"Over 30 Days"));
			comboUnsentAccountAge.Items.Add(Lan.g(this,"Over 60 Days"));
			comboUnsentAccountAge.Items.Add(Lan.g(this,"Over 90 Days"));
			comboUnsentAccountAge.SelectedIndexChanged-=comboUnsentAccountAge_SelectedIndexChanged;
			comboUnsentAccountAge.SelectedIndex=new List<string>() { "30","60","90" }.IndexOf(Preference.GetString(PreferenceName.ArManagerUnsentAgeOfAccount))+1;//+1 for any bal
			comboUnsentAccountAge.SelectedIndexChanged+=comboUnsentAccountAge_SelectedIndexChanged;
			#endregion Unsent Tab Account Age Combo
			#region Unsent Tab Textbox Filters
			//text min bal
			textUnsentMinBal.TextChanged-=textUnsentMinBal_TextChanged;
			textUnsentMinBal.Text=Preference.GetDouble(PreferenceName.ArManagerUnsentMinBal).ToString();
			textUnsentMinBal.TextChanged+=textUnsentMinBal_TextChanged;
			//text days since last payment
			textUnsentDaysLastPay.TextChanged-=textUnsentDaysLastPay_TextChanged;
			textUnsentDaysLastPay.Text=Preference.GetInt(PreferenceName.ArManagerUnsentDaysSinceLastPay).ToString();
			textUnsentDaysLastPay.TextChanged+=textUnsentDaysLastPay_TextChanged;
			#endregion Unsent Tab Textbox Filters
			#region Unsent Tab Checkbox Filters
			//exclude if ins pending
			checkExcludeInsPending.CheckedChanged-=checkExcludeInsPending_CheckedChanged;
			checkExcludeInsPending.Checked=Preference.GetBool(PreferenceName.ArManagerExcludeInsPending);
			checkExcludeInsPending.CheckedChanged+=checkExcludeInsPending_CheckedChanged;
			//exclude if unsent procs
			checkExcludeIfProcs.CheckedChanged-=checkExcludeIfProcs_CheckedChanged;
			checkExcludeIfProcs.Checked=Preference.GetBool(PreferenceName.ArManagerExcludeIfUnsentProcs);
			checkExcludeIfProcs.CheckedChanged+=checkExcludeIfProcs_CheckedChanged;
			//exclude if bad address (no zipcode)
			checkExcludeBadAddress.CheckedChanged-=checkExcludeBadAddress_CheckedChanged;
			checkExcludeBadAddress.Checked=Preference.GetBool(PreferenceName.ArManagerExcludeBadAddresses);
			checkExcludeBadAddress.CheckedChanged+=checkExcludeBadAddress_CheckedChanged;
			#endregion Unsent Tab Checkbox Filters
			#region Unsent Tab Demand Type Combo
			FillDemandTypes();
			#endregion Unsent Tab Demand Type Combo
			#region Unsent Tab Show PatNums
			checkUnsentShowPatNums.CheckedChanged-=checkUnsentShowPatNums_CheckedChanged;
			checkUnsentShowPatNums.Checked=Preference.GetBool(PreferenceName.ReportsShowPatNum);
			checkUnsentShowPatNums.CheckedChanged+=checkUnsentShowPatNums_CheckedChanged;
			#endregion Unsent Tab Show PatNums
			#endregion Fill Unsent Tab Filter ComboBoxes, CheckBoxes, and Fields
			#region Fill Sent Tab Filter ComboBoxes, CheckBoxes, and Fields
			#region Sent Tab Provs Combo
			comboBoxMultiSentProvs.Items.Add("All");
			comboBoxMultiSentProvs.SetSelected(0,true);
			_listProviders.ForEach(x => comboBoxMultiSentProvs.Items.Add(x.GetLongDesc()));
			#endregion Sent Tab Provs Combo
			#region Sent Tab Trans Type Combo
			_listSentTabTransTypes=Enum.GetValues(typeof(TsiTransType)).OfType<TsiTransType>()
				.Where(x => !x.In(TsiTransType.PF,TsiTransType.PT,TsiTransType.SS,TsiTransType.CN,TsiTransType.Agg)).ToList();
			List<TsiTransType> listDefaultLastTransTypes=Preference.GetString(PreferenceName.ArManagerLastTransTypes)
				.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries)
				.Select(x => PIn.Enum<TsiTransType>(x,true))
				.Where(x => !x.In(TsiTransType.PF,TsiTransType.PT,TsiTransType.SS,TsiTransType.CN,TsiTransType.Agg)).ToList();
			comboBoxMultiLastTransType.Items.Add(Lan.g(this,"All"));
			comboBoxMultiLastTransType.SetSelected(0,_listSentTabTransTypes.All(x => !listDefaultLastTransTypes.Contains(x)));//select All if no valid defaults are set
			for(int i=0;i<_listSentTabTransTypes.Count;i++) {
				comboBoxMultiLastTransType.Items.Add(_listSentTabTransTypes[i].GetDescription());
				if(listDefaultLastTransTypes.Contains(_listSentTabTransTypes[i])) {
					comboBoxMultiLastTransType.SetSelected(i+1,true);//+1 for All
				}
			}
			#endregion Sent Tab Trans Type Combo
			#region Sent Tab Account Age Combo
			comboSentAccountAge.Items.Add(Lan.g(this,"Any Balance"));
			comboSentAccountAge.Items.Add(Lan.g(this,"Over 30 Days"));
			comboSentAccountAge.Items.Add(Lan.g(this,"Over 60 Days"));
			comboSentAccountAge.Items.Add(Lan.g(this,"Over 90 Days"));
			comboSentAccountAge.SelectedIndexChanged-=comboSentAccountAge_SelectedIndexChanged;
			comboSentAccountAge.SelectedIndex=new List<string>() { "30","60","90" }.IndexOf(Preference.GetString(PreferenceName.ArManagerSentAgeOfAccount))+1;//+1 for any bal
			comboSentAccountAge.SelectedIndexChanged+=comboSentAccountAge_SelectedIndexChanged;
			#endregion Sent Tab Account Age Combo
			#region Sent Tab Textbox Filters
			//text min bal
			textSentMinBal.TextChanged-=textSentMinBal_TextChanged;
			textSentMinBal.Text=Preference.GetDouble(PreferenceName.ArManagerSentMinBal).ToString();
			textSentMinBal.TextChanged+=textSentMinBal_TextChanged;
			//text days since last payment
			textSentDaysLastPay.TextChanged-=textSentDaysLastPay_TextChanged;
			textSentDaysLastPay.Text=Preference.GetInt(PreferenceName.ArManagerSentDaysSinceLastPay).ToString();
			textSentDaysLastPay.TextChanged+=textSentDaysLastPay_TextChanged;
			#endregion Sent Tab Textbox Filters
			#region Sent Tab New Statuses Combo
			_listNewStatuses=new List<TsiTransType>() { TsiTransType.SS,TsiTransType.CN };
			_listNewStatuses.ForEach(x => comboNewStatus.Items.Add(x.GetDescription()));
			#endregion Sent Tab New Statuses Combo
			#region Sent Tab New Bill Types Combo
			_listBillTypesNoColl.ForEach(x => comboNewBillType.Items.Add(x.Description));
			errorProvider1.SetError(comboNewBillType,"");
			#endregion Sent Tab New Bill Types Combo
			#region Sent Tab Show PatNums
			checkSentShowPatNums.CheckedChanged-=checkSentShowPatNums_CheckedChanged;
			checkSentShowPatNums.Checked=Preference.GetBool(PreferenceName.ReportsShowPatNum);
			checkSentShowPatNums.CheckedChanged+=checkSentShowPatNums_CheckedChanged;
			#endregion Sent Tab Show PatNums
			#endregion Fill Sent Tab Filter ComboBoxes, CheckBoxes, and Fields
			#region Run Aging if Necessary
			string msgText="";
			try {
				msgText="There was a problem running aging.  Would you like to load the accounts grid with currently existing account information?";
				while(!RunAgingIfNecessary()) {
					if(!MsgBox.Show(this,MsgBoxButtons.YesNo,msgText)) {
						Close();
						return;
					}
				}
				SecurityLog.Write(SecurityLogEvents.AgingRan,"Aging Ran Automatically - AR Manager");
			}
			catch {
				msgText="There was a problem running aging.  Would you like to load the accounts grid with currently existing account information?";
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,msgText)) {
					Close();
					return;
				}
			}
			#endregion Run Aging if Necessary
			#region Get Aging List and Fill Grids
			RefreshAll();
			FillGrids();
			#endregion Get Aging List and Fill Grids
		}

		#region Methods For Both Tabs

		private bool RunAgingIfNecessary() {
			string msgText="";
			if(Preference.GetBool(PreferenceName.AgingIsEnterprise)) {
				return RunAgingEnterprise();
			}
			else if(!Preference.GetBool(PreferenceName.AgingCalculatedMonthlyInsteadOfDaily)) {
				Cursor=Cursors.WaitCursor;
				msgText=Lan.g(this,"Calculating aging for all patients as of")+" "+DateTime.Today.ToShortDateString()+"...";
				bool result=true;
				ODProgress.ShowAction(() => Ledgers.RunAging(),
					startingMessage:msgText,
					actionException:e => {
						Ledgers.AgingExceptionHandler(e,this);
						result=false;
					});
				if(!result) {
					return false;
				}
				Cursor=Cursors.Default;
			}
			msgText="Last aging date seems old.  Would you like to run aging now?  The account list will load whether or not aging gets updated.";
			//All places in the program where aging can be run for all patients, the Setup permission is required because it can take a long time.
			if(Preference.GetBool(PreferenceName.AgingCalculatedMonthlyInsteadOfDaily) 
				&& Security.IsAuthorized(Permissions.Setup,true)
				&& Preference.GetDate(PreferenceName.DateLastAging)<DateTime.Today.AddDays(-15)
				&& MsgBox.Show(this,MsgBoxButtons.YesNo,msgText))
			{
				FormAging FormA=new FormAging();
				FormA.BringToFront();
				FormA.ShowDialog();
			}
			return true;
		}

		private bool RunAgingEnterprise() {
			DateTime dtNow=MiscData.GetNowDateTime();
			DateTime dtToday=dtNow.Date;
			DateTime dateLastAging=Preference.GetDate(PreferenceName.DateLastAging);
			string msgText=Lan.g(this,"Aging has already been calculated for")+" "+dtToday.ToShortDateString()+" "
				+Lan.g(this,"and does not normally need to run more than once per day.")+"\r\n\r\n"+Lan.g(this,"Run anway?");
			if(dateLastAging.Date==dtToday.Date && MessageBox.Show(this,msgText,"",MessageBoxButtons.YesNo)!=DialogResult.Yes) {
				return true;
			}
			Preference.Refresh();
			DateTime dateTAgingBeganPref=Preference.GetDateTime(PreferenceName.AgingBeginDateTime);
			if(dateTAgingBeganPref>DateTime.MinValue) {
				msgText=Lan.g(this,"In order to manage accounts receivable, aging must be calculated, but you cannot run aging until it has finished the current "
					+"calculations which began on")+" "+dateTAgingBeganPref.ToString()+".\r\n"+Lans.g(this,"If you believe the current aging process has finished, "
					+"a user with SecurityAdmin permission can manually clear the date and time by going to Setup | Miscellaneous and pressing the 'Clear' button.");
				MessageBox.Show(this,msgText);
				return false;
			}
			SecurityLog.Write(SecurityLogEvents.AgingRan,"Aging Ran - AR Manager");
			Preference.Update(PreferenceName.AgingBeginDateTime,POut.DateT(dtNow,false));//get lock on pref to block others
            CacheManager.InvalidateEverywhere<Preference>();
            Cursor =Cursors.WaitCursor;
			msgText=Lan.g(this,"Calculating enterprise aging for all patients as of")+" "+dtToday.ToShortDateString()+"...";
			bool result=true;
			ODProgress.ShowAction(
				() => {
					Ledgers.ComputeAging(0,dtToday);
					Preference.Update(PreferenceName.DateLastAging,POut.Date(dtToday,false));
				},
				startingMessage:msgText,
				actionException:ex => {
					Ledgers.AgingExceptionHandler(ex,this);
					result=false;
				});
			Cursor=Cursors.Default;
			Preference.Update(PreferenceName.AgingBeginDateTime,"");//clear lock on pref whether aging was successful or not
            CacheManager.InvalidateEverywhere<Preference>();
            return result;
		}

		private void RefreshAll() {
			Cursor=Cursors.WaitCursor;
			//clear rows here because the rows are tagged with the PatAging objects and we want to dispose of them before we get the 
			//aging list again so we don't have an out of memory error for large db's
			gridSent.BeginUpdate();//Clears selected indexes.
			gridSent.Rows.Clear();
			gridSent.EndUpdate();
			gridUnsent.BeginUpdate();//Clears selected indexes.
			gridUnsent.Rows.Clear();
			gridUnsent.EndUpdate();
			string msgText=Lan.g(this,"Retrieving aging list as of")+" "+MiscData.GetNowDateTime().ToShortDateString()+"...";
			ODProgress.ShowAction(
				() => {
					_listPatAgingSentAll=new List<PatAging>();
					_listPatAgingUnsentAll=new List<PatAging>();
					List<PatAging> listPatAgingAll=Patients.GetAgingList();
					GC.Collect();//necessary because we are in a thread and need to garbage collect before starting the next method or we may get an OOM error
					Patients.SetDateBalBegan(ref listPatAgingAll);
					GC.Collect();//to reclaim the temporary memory used by the above method
					foreach(PatAging ptAgeCur in listPatAgingAll) {
						if(_collectionBillType!=null && ptAgeCur.BillingType==_collectionBillType.Id) {
							_listPatAgingSentAll.Add(ptAgeCur);
						}
						else {
							_listPatAgingUnsentAll.Add(ptAgeCur);
						}
					}
				},
				startingMessage:msgText);
			Cursor=Cursors.Default;
		}

		private void FillGrids(bool retainSelection=true) {
			FillGridUnsent(retainSelection);
			FillGridSent(retainSelection);
		}

		///<summary>Line the totals textboxes up with their corresponding grid column.  Since the columns resize dynamically and can be visible or hidden
		///based on selected display fields, we need to move the location of the textboxes and set visibility.  This is called when the grid is filled as
		///well as when the user horizontally scrolls.</summary>
		private void SetTotalsLocAndVisible(DisplayFieldCategory displayCat,int hScrollValue) {
			//list of selected display fields for the grid
			List<DisplayField> listDisplayFields=DisplayFields.GetForCategory(displayCat);
			//dictionary linking display field internal names to the corresponding textboxes
			Dictionary<string,TextBox> dictColTextBox = new Dictionary<string,TextBox>() {
				{ "Guarantor",(displayCat==DisplayFieldCategory.ArManagerUnsentGrid?textUnsentTotalNumAccts:textSentTotalNumAccts) },
				{ "0-30 Days",(displayCat==DisplayFieldCategory.ArManagerUnsentGrid?textUnsent0to30:textSent0to30) },
				{ "31-60 Days",(displayCat==DisplayFieldCategory.ArManagerUnsentGrid?textUnsent31to60:textSent31to60) },
				{ "61-90 Days",(displayCat==DisplayFieldCategory.ArManagerUnsentGrid?textUnsent61to90:textSent61to90) },
				{ "> 90 Days",(displayCat==DisplayFieldCategory.ArManagerUnsentGrid?textUnsentOver90:textSentOver90) },
				{ "Total",(displayCat==DisplayFieldCategory.ArManagerUnsentGrid?textUnsentTotal:textSentTotal) },
				{ "-Ins Est",(displayCat==DisplayFieldCategory.ArManagerUnsentGrid?textUnsentInsEst:textSentInsEst) },
				{ "=Patient",(displayCat==DisplayFieldCategory.ArManagerUnsentGrid?textUnsentPatient:textSentPatient) },
				{ "PayPlan Due",(displayCat==DisplayFieldCategory.ArManagerUnsentGrid?textUnsentPayPlanDue:textSentPayPlanDue) }
			};
			Label labelTotals=(displayCat==DisplayFieldCategory.ArManagerUnsentGrid?labelUnsentTotals:labelSentTotals);
			labelTotals.Visible=false;//set to visible if there are any of the dollar amount columns showing
			Label labelTotalNumAccounts=(displayCat==DisplayFieldCategory.ArManagerUnsentGrid?labelUnsentTotalNumAccts:labelSentTotalNumAccts);
			labelTotalNumAccounts.Visible=false;//set to visible if the Guarantor column is showing
			ODGrid gridCur=(displayCat==DisplayFieldCategory.ArManagerUnsentGrid?gridUnsent:gridSent);
			if(listDisplayFields.Any(x => dictColTextBox.ContainsKey(x.InternalName))) {//if any of the display fields that have a textbox are selected
				Dictionary<string,string> dictColInternalNames=listDisplayFields
					.ToDictionary(x => (string.IsNullOrEmpty(x.Description)?x.InternalName:x.Description),x => x.InternalName);
				Dictionary<string,Tuple<int,int>> dictColPosAndWidth=GetXPosAndWidths(gridCur,hScrollValue);//key=column heading, value=Tuple<x-pos,width>
				bool firstCol=true;//used to set location and visibility of the Totals label, set to the left of the first textbox
				foreach(string colName in dictColPosAndWidth.Keys.ToList()) {//go through all columns in the grid
					Tuple<int,int> colLocWidthCur=dictColPosAndWidth[colName];
					string colInternalName;
					if(!dictColInternalNames.TryGetValue(colName,out colInternalName)) {
						//if the grid column heading is not in the list of display fields continue (shouldn't be possible)
						continue;
					}
					TextBox textBoxCur;
					if(!dictColTextBox.TryGetValue(colInternalName,out textBoxCur)) {//if the grid column heading is not associated to a textbox, skip it
						continue;
					}
					if(colInternalName=="Guarantor") {
						//+1 so label doesn't cover right side of textbox if it happens to follow a totals textbox
						labelTotalNumAccounts.Location=new Point(colLocWidthCur.Item1+1,labelTotalNumAccounts.Location.Y);
						labelTotalNumAccounts.Visible=true;
						//+2 to account for +1 above and a 1 pixel space between label and textbox
						textBoxCur.Location=new Point(colLocWidthCur.Item1+labelTotalNumAccounts.Width+2,textBoxCur.Location.Y);
						//width of label+width of textbox+2 will be width of Guarantor column up to a max textbox width of 65, which will hold at least 10 digits,
						//so works for all numbers of accounts up to 9,999,999,999.  It is highly unlikely that any customer will ever have 10 billion guarantors
						//in their aging list.  (That's more than the number of people on the planet, as of 6/27/2018 anyway.)
						textBoxCur.Width=Math.Max(Math.Min(colLocWidthCur.Item2-labelTotalNumAccounts.Width-1,65),1);
						continue;
					}
					if(firstCol) {
						//left of the first textbox label width+1 for a space between label and textbox
						labelTotals.Location=new Point(colLocWidthCur.Item1-labelTotals.Width-1,labelTotals.Location.Y);
						labelTotals.Visible=true;
						firstCol=false;
					}
					textBoxCur.Location=new Point(colLocWidthCur.Item1,textBoxCur.Location.Y);//set location and width to the column's x-pos and width
					textBoxCur.Width=colLocWidthCur.Item2;
				}
			}
			//hide/unhide textboxes based on the associated display fields
			dictColTextBox.ToList().ForEach(x => x.Value.Visible=listDisplayFields.Any(y => y.InternalName==x.Key));
		}

		private void menuItemGoTo_Click(object sender,EventArgs e) {
			List<PatAging> listPatAgingsAll;
			ODGrid gridCur;
			if(tabControlMain.SelectedTab==tabUnsent) {
				gridCur=gridUnsent;
				listPatAgingsAll=_listPatAgingUnsentAll;
			}
			else {
				gridCur=gridSent;
				listPatAgingsAll=_listPatAgingSentAll;
			}
			if(gridCur.SelectedGridRows.Count!=1) {
				MsgBox.Show(this,"Please select one patient first.");
				return;
			}
			object pAgeIndex=gridCur.SelectedGridRows[0].Tag;
			if(!(pAgeIndex is int) || (int)pAgeIndex<0 || (int)pAgeIndex>=listPatAgingsAll.Count) {
				return;
			}
			FormOpenDental.S_Contr_PatientSelected(Patients.GetPat(listPatAgingsAll[(int)pAgeIndex].PatNum),false);
			GotoModule.GotoAccount(0);
			SendToBack();
		}

		private void timerFillGrid_Tick(object sender,EventArgs e) {
			timerFillGrid.Enabled=false;
			ValidateChildren(ValidationConstraints.Enabled|ValidationConstraints.Visible|ValidationConstraints.Selectable);
			if(tabControlMain.SelectedTab==tabUnsent) {
				FillGridUnsent();
			}
			else {
				FillGridSent();
			}
		}

		private void tabControlMain_SelectedIndexChanged(object sender,EventArgs e) {
			timerFillGrid.Enabled=false;
		}

		///<summary>Fills the grid, but only if not preceded by a ResizeBegin event, i.e. the form is being manually resized or moved around the screen.</summary>
		private void FormArManager_Resize(object sender,EventArgs e) {
			_isResizing=true;
			if(_hasResizeBegan || WindowState==FormWindowState.Minimized) {
				return;//handle fill grid in ResizeEnd
			}
			//Don't attempt to fill the grid if either of these lists are null.  They're set on load and should never be null once load has finished, but
			//this was firing before load and caused an error on one customer PC.
			if(_listPatAgingUnsentAll==null || _listPatAgingSentAll==null) {
				_isResizing=false;
				return;
			}
			FillGrids();
			_isResizing=false;
		}

		///<summary>Fires when manual resizing begins.  Sets _hasResizeBegan so the grid is only filled once manual resizing is finished.</summary>
		private void FormArManager_ResizeBegin(object sender,EventArgs e) {
			_hasResizeBegan=true;
		}

		///<summary>Fires when manual resizing is complete, NOT when changing window state. i.e. this is not fired when the window is maximized, minimized
		///or restored.  But this also fires when just moving the form around, so we will use both bools to determine if we have a ResizeBegin and actual 
		///resize events and only then refill the grid.</summary>
		private void FormArManager_ResizeEnd(object sender,EventArgs e) {
			if(_isResizing && _hasResizeBegan) {
				FillGrids();
			}
			_isResizing=false;
			_hasResizeBegan=false;
		}

		private void SaveDefaults() {
			if(textUnsentMinBal.errorProvider1.GetError(textUnsentMinBal)!="" || textUnsentDaysLastPay.errorProvider1.GetError(textUnsentDaysLastPay)!=""
				|| textSentMinBal.errorProvider1.GetError(textSentMinBal)!="" || textSentDaysLastPay.errorProvider1.GetError(textSentDaysLastPay)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			#region Sent Defaults
			string selectedTransTypes="";//indicates all.
			if(comboBoxMultiLastTransType.SelectedIndices.Count>0 && !comboBoxMultiLastTransType.SelectedIndices.Contains(0)) {
				selectedTransTypes=string.Join(",",comboBoxMultiLastTransType.ListSelectedIndices.Select(x => _listSentTabTransTypes[x-1]));//-1 for All
			}
			string sentAgeOfAccount="";//indicates any age
			if(comboSentAccountAge.SelectedIndex.In(1,2,3)) {
				sentAgeOfAccount=(30*comboSentAccountAge.SelectedIndex).ToString();//ageOfAccount is 30, 60, or 90
			}
			int sentDaysSinceLastPay=0;
			if(!string.IsNullOrWhiteSpace(textSentDaysLastPay.Text)) {
				sentDaysSinceLastPay=PIn.Int(textSentDaysLastPay.Text);
			}
			double sentMinBal=0.00;
			if(!string.IsNullOrWhiteSpace(textSentMinBal.Text)) {
				sentMinBal=PIn.Double(textSentMinBal.Text);
			}
			#endregion Sent Defaults
			#region Unsent Defaults
			string selectedBillTypes="";//indicates all.
			if(comboBoxMultiBillTypes.SelectedIndices.Count>0 && !comboBoxMultiBillTypes.SelectedIndices.Contains(0)) {
				selectedBillTypes=string.Join(",",comboBoxMultiBillTypes.ListSelectedIndices.Select(x => _listBillTypesNoColl[x-1].Id));//-1 for All
			}
			string unsentAgeOfAccount="";//indicates any age
			if(comboUnsentAccountAge.SelectedIndex.In(1,2,3)) {
				unsentAgeOfAccount=(30*comboUnsentAccountAge.SelectedIndex).ToString();//ageOfAccount is 30, 60, or 90
			}
			int unsentDaysSinceLastPay=0;
			if(!string.IsNullOrWhiteSpace(textUnsentDaysLastPay.Text)) {
				unsentDaysSinceLastPay=PIn.Int(textUnsentDaysLastPay.Text);
			}
			double unsentMinBal=0.00;
			if(!string.IsNullOrWhiteSpace(textUnsentMinBal.Text)) {
				unsentMinBal=PIn.Double(textUnsentMinBal.Text);
			}
			#endregion Unsent Defaults
			if( Preference.Update(PreferenceName.ArManagerBillingTypes,selectedBillTypes)
				| Preference.Update(PreferenceName.ArManagerExcludeBadAddresses,checkExcludeBadAddress.Checked)
				| Preference.Update(PreferenceName.ArManagerExcludeIfUnsentProcs,checkExcludeIfProcs.Checked)
				| Preference.Update(PreferenceName.ArManagerExcludeInsPending,checkExcludeInsPending.Checked)
				| Preference.Update(PreferenceName.ArManagerLastTransTypes,selectedTransTypes)
				| Preference.Update(PreferenceName.ArManagerSentAgeOfAccount,sentAgeOfAccount)
				| Preference.Update(PreferenceName.ArManagerSentDaysSinceLastPay,sentDaysSinceLastPay)
				| Preference.Update(PreferenceName.ArManagerSentMinBal,POut.Double(sentMinBal))
				| Preference.Update(PreferenceName.ArManagerUnsentAgeOfAccount,unsentAgeOfAccount)
				| Preference.Update(PreferenceName.ArManagerUnsentDaysSinceLastPay,unsentDaysSinceLastPay)
				| Preference.Update(PreferenceName.ArManagerUnsentMinBal,POut.Double(unsentMinBal)))
			{
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}

		private void butTsiOcp_Click(object sender,EventArgs e) {
			Process.Start("https://service.transworldsystems.com/FormsLogin.asp?/rep/repview.asp");
		}

		private bool ValidateSendUpdateData(List<long> listClinicNums,out List<long> listClinicsSkipped) {
			listClinicsSkipped=new List<long>();
			//if(_tsiProg==null) {
			//	MsgBox.Show(this,"The Transworld program link does not exist.  Please contact support.");
			//	return false;
			//}
			//if(!_tsiProg.Enabled) {
			//	MsgBox.Show(this,"The Transworld program link is not enabled.");
			//	return false;
			//}
			//if(_dictClinicProgProps.Count==0) {
			//	MsgBox.Show(this,"The Transworld program link is not setup.  Try again after entering the program link properties.");
			//	return false;
			//}
			//Cursor=Cursors.WaitCursor;
			//List<long> listClinicsValidated=new List<long>();
			//foreach(long clinicNum in listClinicNums.Distinct()) {
			//	if(!Preferences.HasClinicsEnabled && clinicNum>0) {
			//		continue;//Only test the HQ clinic (ClinicNum=0) if clinics are not enabled
			//	}
			//	List<ProgramProperty> listProgProps;
			//	if(!_dictClinicProgProps.TryGetValue(clinicNum,out listProgProps) //if the clinic doesn't have prog props, try to use the HQ prog props
			//		&& (clinicNum==0 || !Preferences.HasClinicsEnabled || Security.CurrentUser.ClinicRestricted || !_dictClinicProgProps.TryGetValue(0,out listProgProps)))
			//	{
			//		listClinicsSkipped.Add(clinicNum);
			//		continue;
			//	}
			//	if(listProgProps.Count==0 || (!listClinicsValidated.Contains(listProgProps[0].ClinicId) && !TsiTransLogs.ValidateClinicSftpDetails(listProgProps))) {
			//		listClinicsSkipped.Add(clinicNum);
			//		continue;
			//	}
			//	listClinicsValidated.Add(listProgProps[0].ClinicId);
			//}
			//if(Preferences.HasClinicsEnabled) {
			//	if(listClinicsSkipped.Contains(0)) {
			//		listClinicsSkipped.AddRange(_listClinics.FindAll(x => !_dictClinicProgProps.ContainsKey(x.ClinicNum)).Select(x => x.ClinicNum));
			//	}
			//	else if(!Security.CurrentUser.ClinicRestricted && _dictClinicProgProps.ContainsKey(0)) {
			//		//if clinics are enabled and the user is not restricted, any clinic without prog props will use the HQ prog props for the sftp connection
			//		_listClinics.FindAll(x => !_dictClinicProgProps.ContainsKey(x.ClinicNum)).ForEach(x => _dictClinicProgProps[x.ClinicNum]=_dictClinicProgProps[0]);
			//	}
			//}
			Cursor=Cursors.Default;
			return true;
		}

		///<summary>Gets the selected PatAgings for the given grid.</summary>
		private List<PatAging> GetSelectedPatAgings(ODGrid grid) {
			List<PatAging> listPatAgingsAll=_listPatAgingSentAll;
			if(grid==gridUnsent) {
				listPatAgingsAll=_listPatAgingUnsentAll;
			}
			return grid.SelectedGridRows
					.Where(x => x.Tag is int && (int)x.Tag>=0 && (int)x.Tag<listPatAgingsAll.Count)
					.Select(x => listPatAgingsAll[(int)x.Tag]).ToList();
		}

		///<summary>Selects the given pat nums for the given grid.</summary>
		private void SetSelectedRows(List<long> listPatNums,ODGrid grid) {
			if(listPatNums==null || listPatNums.Count==0) {
				return;
			}
			List<PatAging> listPatAgingsAll=_listPatAgingSentAll;
			if(grid==gridUnsent) {
				listPatAgingsAll=_listPatAgingUnsentAll;
			}
			grid.SetSelected(listPatNums.Select(x => grid.Rows.ToList()
					.FindIndex(y => y.Tag is int && (int)y.Tag>=0 && (int)y.Tag<listPatAgingsAll.Count && x==listPatAgingsAll[(int)y.Tag].PatNum))
					.Where(x => x>-1)//Ignore any entries within listPatNums that were not found in our grid.
					.ToArray(),true);
		}

		private void butHistory_Click(object sender,EventArgs e) {
			ODForm FormTHist=Application.OpenForms.OfType<ODForm>().Where(x => x!=this).FirstOrDefault(x => x.Name=="FormTsiHistory");
			if(FormTHist==null) {
				FormTHist=new FormTsiHistory();
			}
			FormTHist.Restore();
			FormTHist.Show();
			FormTHist.BringToFront();
		}

		#endregion Methods For Both Tabs
		#region Unsent Tab Methods

		///<summary>Returns a dictionary of key=column heading, value=tuple of Item1=xPos,Item2=width for the given column heading.  Used to position
		///totals texboxes below the corresponding column and to resize/reposition as the form is resized.</summary>
		private Dictionary<string,Tuple<int,int>> GetXPosAndWidths(ODGrid grid,int hScrollValue) {
			Dictionary<string,Tuple<int,int>> retval=new Dictionary<string,Tuple<int,int>>();
			int xPos=grid.Location.X-hScrollValue;
			foreach(ODGridColumn column in grid.Columns) {
				retval[column.Heading]=Tuple.Create(xPos+1,column.Width+1);//+1 because the textbox lines seem to be slightly thinner than the grid column lines
				xPos+=column.Width;
			}
			return retval;
		}

		private void FillGridUnsent(bool retainSelection=true) {
			Cursor=Cursors.WaitCursor;
			List<long> listSelectedPatNums=new List<long>();
			if(retainSelection) {
				listSelectedPatNums=GetSelectedPatAgings(gridUnsent).Select(x => x.PatNum).ToList();
			}
			List<int> listPatAgingIndexFiltered=GetPatAgingIndexUnsentFiltered();
			#region Set Grid Title and Columns
			gridUnsent.BeginUpdate();
			gridUnsent.Columns.Clear();
			List<DisplayField> listDisplayFields=DisplayFields.GetForCategory(DisplayFieldCategory.ArManagerUnsentGrid);
			foreach(DisplayField fieldCur in listDisplayFields) {
				if(fieldCur.InternalName=="Clinic" && !Preferences.HasClinicsEnabled) {
					continue;//skip the clinic column if clinics are not enabled
				}
				ODGridSortingStrategy sortingStrat=ODGridSortingStrategy.StringCompare;
				HorizontalAlignment hAlign=HorizontalAlignment.Left;
				if(fieldCur.InternalName.In("0-30 Days","31-60 Days","61-90 Days","> 90 Days","Total","-Ins Est","=Patient","PayPlan Due",
					DisplayFields.InternalNames.ArManagerUnsentGrid.DaysBalBegan)) 
				{
					sortingStrat=ODGridSortingStrategy.AmountParse;
					hAlign=HorizontalAlignment.Right;
				}
				else if(fieldCur.InternalName.In("Last Paid","Last Proc","DateTime Suspended",DisplayFields.InternalNames.ArManagerUnsentGrid.DateBalBegan)) {
					sortingStrat=ODGridSortingStrategy.DateParse;
					hAlign=HorizontalAlignment.Center;
				}
				else if(!fieldCur.InternalName.In("Guarantor","Clinic","Prov","Billing Type")) {
					continue;//shouldn't happen, but the loop to fill the rows will skip any columns that aren't one of these so we'll skip here as well
				}
				gridUnsent.Columns.Add(new ODGridColumn(string.IsNullOrEmpty(fieldCur.Description)?fieldCur.InternalName:fieldCur.Description,
					fieldCur.ColumnWidth,hAlign,sortingStrat));
			}
			//this form initially set to the max allowed (by OD) form size 1246, which is also the minimum size for this form.  If the user resizes the form
			//to be larger, increase each column width by the same ratio to spread out the additional real estate
			int widthColsAndScroll=gridUnsent.WidthAllColumns+20;//+20 for vertical scroll bar
			//widthColsAndScroll is width of columns as set in the display fields, i.e. haven't grown or shrunk due to form resizing so won't be shrunk to
			//less than the sizes set in display fields, only grown from there.  Thus the display field sizes are basically a minimum size.
			if(widthColsAndScroll<gridUnsent.Width) {
				gridUnsent.Columns.ToList().ForEach(x => x.Width=(int)Math.Round((float)x.Width*gridUnsent.Width/widthColsAndScroll,MidpointRounding.AwayFromZero));
				//adjust the last col width to take any remaining pixels so that the cols take the full width of the grid (to account for rounding above)
				gridUnsent.Columns[gridUnsent.Columns.Count-1].Width-=gridUnsent.WidthAllColumns+20-gridUnsent.Width;
			}
			#endregion Set Grid Title and Columns
			#region Fill Grid Rows
			gridUnsent.Rows.Clear();
			Dictionary<long,string> dictClinicAbbrs=_listClinics.ToDictionary(x => x.ClinicNum,x => x.Abbr);
			Dictionary<long,string> dictProvAbbrs=_listProviders.ToDictionary(x => x.ProvNum,x => x.Abbr);
			Dictionary<long,string> dictBillTypeNames=Definition.GetByCategory(DefinitionCategory.BillingTypes).ToDictionary(x => x.Id,x => x.Description);
			Dictionary<long,DateTime> dictSuspendDateTimes=new Dictionary<long,DateTime>();
			foreach(PatAging pAgeCur in listPatAgingIndexFiltered.Select(x => _listPatAgingUnsentAll[x])) {
				TsiTransLog tsiLogMostRecentStatusChange=pAgeCur.ListTsiLogs
					.Find(x => x.TransType.In(TsiTransType.CN,TsiTransType.PF,TsiTransType.PL,TsiTransType.PT,TsiTransType.RI,TsiTransType.SS));
				if(tsiLogMostRecentStatusChange!=null && tsiLogMostRecentStatusChange.TransType==TsiTransType.SS) {
					dictSuspendDateTimes.Add(pAgeCur.PatNum,tsiLogMostRecentStatusChange.TransDateTime);
				}
			}
			double bal0_30=0;
			double bal31_60=0;
			double bal61_90=0;
			double balOver90=0;
			double balTotal=0;
			double insEst=0;
			double amtDue=0;
			double ppDue=0;
			ODGridRow row;
			List<int> listIndicesToReselect=new List<int>();
			foreach(int i in listPatAgingIndexFiltered) {
				PatAging patAgeCur=_listPatAgingUnsentAll[i];
				bal0_30+=patAgeCur.Bal_0_30;
				bal31_60+=patAgeCur.Bal_31_60;
				bal61_90+=patAgeCur.Bal_61_90;
				balOver90+=patAgeCur.BalOver90;
				balTotal+=patAgeCur.BalTotal;
				insEst+=patAgeCur.InsEst;
				amtDue+=patAgeCur.AmountDue;
				ppDue+=patAgeCur.PayPlanDue;
				row=new ODGridRow();
				foreach(DisplayField field in listDisplayFields) {
					switch(field.InternalName) {
						case "Guarantor":
							row.Cells.Add((checkUnsentShowPatNums.Checked?(patAgeCur.PatNum.ToString()+" - "):"")+patAgeCur.PatName);
							continue;
						case "Clinic":
							if(!Preferences.HasClinicsEnabled) {
								continue;//skip the clinic column if clinics are not enabled
							}
							string clinicAbbr;
							row.Cells.Add(dictClinicAbbrs.TryGetValue(patAgeCur.ClinicNum,out clinicAbbr)?clinicAbbr:"");
							continue;
						case "Prov":
							string provAbbr;
							row.Cells.Add(dictProvAbbrs.TryGetValue(patAgeCur.PriProv,out provAbbr)?provAbbr:"");
							continue;
						case "Billing Type":
							string billTypeName;
							row.Cells.Add(dictBillTypeNames.TryGetValue(patAgeCur.BillingType,out billTypeName)?billTypeName:"");
							continue;
						case "0-30 Days":
							row.Cells.Add(patAgeCur.Bal_0_30.ToString("n"));
							continue;
						case "31-60 Days":
							row.Cells.Add(patAgeCur.Bal_31_60.ToString("n"));
							continue;
						case "61-90 Days":
							row.Cells.Add(patAgeCur.Bal_61_90.ToString("n"));
							continue;
						case "> 90 Days":
							row.Cells.Add(patAgeCur.BalOver90.ToString("n"));
							continue;
						case "Total":
							row.Cells.Add(patAgeCur.BalTotal.ToString("n"));
							continue;
						case "-Ins Est":
							row.Cells.Add(patAgeCur.InsEst.ToString("n"));
							continue;
						case "=Patient":
							row.Cells.Add(patAgeCur.AmountDue.ToString("n"));
							continue;
						case "PayPlan Due":
							row.Cells.Add(patAgeCur.PayPlanDue.ToString("n"));
							continue;
						case "Last Paid":
							row.Cells.Add(patAgeCur.DateLastPay.Year>1880?patAgeCur.DateLastPay.ToString("d"):"");
							continue;
						case "DateTime Suspended":
							TsiTransLog suspendLog=patAgeCur.ListTsiLogs.Find(x => x.TransType==TsiTransType.SS);
							row.Cells.Add(suspendLog!=null?suspendLog.TransDateTime.ToString():"");
							continue;
						case "Last Proc":
							row.Cells.Add(patAgeCur.DateLastProc.Year>1880?patAgeCur.DateLastProc.ToString("d"):"");
							continue;
						case DisplayFields.InternalNames.ArManagerUnsentGrid.DateBalBegan:
							row.Cells.Add(patAgeCur.DateBalBegan.Year>1880?patAgeCur.DateBalBegan.ToString("d"):"");
							continue;
						case DisplayFields.InternalNames.ArManagerUnsentGrid.DaysBalBegan:
							row.Cells.Add(patAgeCur.DateBalBegan.Year>1880?(DateTime.Today-patAgeCur.DateBalBegan).Days.ToString():"");
							continue;
						default:
							continue;//skip any columns not defined here, as we did above when adding the columns
					}
				}
				if(patAgeCur.Birthdate.Year<1880//invalid bday
					|| patAgeCur.Birthdate>DateTime.Today.AddYears(-18)//under 18 years old
					|| new[] { patAgeCur.Address,patAgeCur.City,patAgeCur.State,patAgeCur.Zip }.Any(x => string.IsNullOrWhiteSpace(x)))//missing address information
				{
					//color row light red/pink, using cell color so selecting row still shows color
					row.Cells.OfType<ODGridCell>().ToList().ForEach(x => x.CellColor=Color.FromArgb(255,255,230,234));
				}
				row.Tag=i;//tag the row with the index in the class-wide list of all unsent PatAgings
				gridUnsent.Rows.Add(row);
				if(retainSelection && listSelectedPatNums.Contains(patAgeCur.PatNum)) {
					listIndicesToReselect.Add(gridUnsent.Rows.Count-1);
				}
			}
			gridUnsent.EndUpdate();
			#endregion Fill Grid Rows
			groupPlaceAccounts.Enabled=(gridUnsent.Rows.Count>0);
			if(retainSelection && listIndicesToReselect.Count>0) {
				gridUnsent.SetSelected(listIndicesToReselect.ToArray(),true);
			}
			SetTotalsLocAndVisible(DisplayFieldCategory.ArManagerUnsentGrid,gridUnsent.HScrollValue);
			textUnsentTotalNumAccts.Text=listPatAgingIndexFiltered.Count.ToString();
			textUnsent0to30.Text=bal0_30.ToString("n");
			textUnsent31to60.Text=bal31_60.ToString("n");
			textUnsent61to90.Text=bal61_90.ToString("n");
			textUnsentOver90.Text=balOver90.ToString("n");
			textUnsentTotal.Text=balTotal.ToString("n");
			textUnsentInsEst.Text=insEst.ToString("n");
			textUnsentPatient.Text=amtDue.ToString("n");
			textUnsentPayPlanDue.Text=ppDue.ToString("n");
			Cursor=Cursors.Default;
		}

		private List<int> GetPatAgingIndexUnsentFiltered() {
			List<int> retval=new List<int>();
			#region Validate Inputs
			if(textUnsentMinBal.errorProvider1.GetError(textUnsentMinBal)!="" || textUnsentDaysLastPay.errorProvider1.GetError(textUnsentDaysLastPay)!="") {
				MsgBox.Show(this,"Please fix data entry errors in Unsent tab first.");
				return retval;//return empty list, filter inputs cannot be applied since there are errors
			}
			#endregion Validate Inputs
			#region Get Filter Data
			double minBalance=Math.Round(PIn.Double(textUnsentMinBal.Text),3);
			DateTime dtLastPay=DateTime.Today.AddDays(-PIn.Int(textUnsentDaysLastPay.Text));
			AgeOfAccount accountAge=new[] { AgeOfAccount.Any,AgeOfAccount.Over30,AgeOfAccount.Over60,AgeOfAccount.Over90 }[comboUnsentAccountAge.SelectedIndex];
			List<long> listBillTypes=new List<long>();
			if(!comboBoxMultiBillTypes.ListSelectedIndices.Contains(0)) {
				listBillTypes=comboBoxMultiBillTypes.ListSelectedIndices.Select(x => _listBillTypesNoColl[x-1].Id).ToList();
			}
			List<long> listProvNums=new List<long>();
			if(!comboBoxMultiUnsentProvs.ListSelectedIndices.Contains(0)) {
				listProvNums=comboBoxMultiUnsentProvs.ListSelectedIndices.Select(x => _listProviders[x-1].ProvNum).ToList();
			}
			List<long> listClinicNums=new List<long>();
			if(Preferences.HasClinicsEnabled) {
				if(comboBoxMultiUnsentClinics.ListSelectedIndices.Contains(0)) {
					listClinicNums=_listClinics.Select(x => x.ClinicNum).ToList();
				}
				else {
					//x-1 works because we know index 0 isn't selected(from above contains(0)) and we -1 for All clinics
					listClinicNums=comboBoxMultiUnsentClinics.ListSelectedIndices.Select(x => _listClinics[x-1].ClinicNum).ToList();
				}
			}
			#endregion Get Filter Data
			#region Apply Filter Data to PatAging List
			for(int i=0;i<_listPatAgingUnsentAll.Count;i++) {
				PatAging pAge=_listPatAgingUnsentAll[i];
				if(Math.Round(pAge.AmountDue,3) >= minBalance
					&& (dtLastPay.Date>=DateTime.Today.Date || pAge.DateLastPay.Date<dtLastPay.Date)
					&& (listBillTypes.Count==0 || listBillTypes.Contains(pAge.BillingType))
					&& (listProvNums.Count==0 || listProvNums.Contains(pAge.PriProv))
					&& (listClinicNums.Count==0 || listClinicNums.Contains(pAge.ClinicNum))
					&& (!checkExcludeBadAddress.Checked || new[] { pAge.Address,pAge.City,pAge.State,pAge.Zip }.All(y => !string.IsNullOrWhiteSpace(y)))
					&& (!checkExcludeIfProcs.Checked || !pAge.HasUnsentProcs)
					&& (!checkExcludeInsPending.Checked || !pAge.HasInsPending)
					&& ( ((int)accountAge < 4 && pAge.BalOver90 > 0.005)//if Any, Over30, Over60 or Over90 are selected, check BalOver90
						|| ((int)accountAge < 3 && pAge.Bal_61_90 > 0.005)//if Any, Over30 or Over60 are selected, check Bal_61_90
						|| ((int)accountAge < 2 && pAge.Bal_31_60 > 0.005)//if Any or Over30 are selected, check Bal_31_60
						|| (int)accountAge < 1 ))//or if Any bal is selected
				{
					retval.Add(i);
				}
			}
			#endregion Apply Filter Data to PatAging List
			return retval;
		}

		private void gridUnsentMain_MouseDown(object sender,MouseEventArgs e) {
			if(e.Button==MouseButtons.Right){
				gridUnsent.SetSelected(false);
			}
			else {
				_listSelectedPatNums=GetSelectedPatAgings(gridUnsent).Select(x => x.PatNum).ToList();
			}
		}

		private void gridUnsentMain_OnSortByColumn(object sender,EventArgs e) {
				SetSelectedRows(_listSelectedPatNums,gridUnsent);
		}

		private void gridUnsentMain_OnHScroll(object sender,ScrollEventArgs e) {
			SetTotalsLocAndVisible(DisplayFieldCategory.ArManagerUnsentGrid,e.NewValue);
		}

		private void gridUnsent_MouseMove(object sender,MouseEventArgs e) {
			try {
				if(_lastCursorPos==e.Location) {
					return;
				}
				if(e.Button!=MouseButtons.None) {
					_toolTipUnsentErrors.RemoveAll();
					return;
				}
				int colIndex=gridUnsent.PointToCol(e.X);
				if(colIndex<0) {
					return;
				}
				int rowIndex=gridUnsent.PointToRow(e.Y);
				if(rowIndex<0) {
					return;
				}
				int lastRowIndex=gridUnsent.PointToRow(_lastCursorPos.Y);
				if(rowIndex==lastRowIndex) {
					return;
				}
				object pAgeIndex=gridUnsent.Rows[rowIndex].Tag;
				if(!(pAgeIndex is int) || (int)pAgeIndex<0 || (int)pAgeIndex>=_listPatAgingUnsentAll.Count) {
					_toolTipUnsentErrors.RemoveAll();
					return;
				}
				PatAging pAgeCur=_listPatAgingUnsentAll[(int)pAgeIndex];
				List<string> listErrors=new List<string>();
				if(pAgeCur.Birthdate.Year<1880 || pAgeCur.Birthdate>DateTime.Today.AddYears(-18)) {
					listErrors.Add(Lan.g(this,"Birthdate"));
				}
				if(new[] { pAgeCur.Address,pAgeCur.City,pAgeCur.State,pAgeCur.Zip }.Any(x => string.IsNullOrWhiteSpace(x))) {
					listErrors.Add(Lan.g(this,"Address"));
				}
				if(listErrors.Count==0) {
					_toolTipUnsentErrors.RemoveAll();
					return;
				}
				_toolTipUnsentErrors.SetToolTip(gridUnsent,Lan.g(this,"Invalid")+" "+string.Join(" "+Lan.g(this,"and")+" ",listErrors));
			}
			catch{
				_toolTipUnsentErrors.RemoveAll();

			}
			finally {
				_lastCursorPos=e.Location;
			}
		}

		private void comboBoxMultiUnsentClinics_Leave(object sender,EventArgs e) {
			if(comboBoxMultiUnsentClinics.SelectedIndices.Contains(0)) {
				comboBoxMultiUnsentClinics.SelectedIndicesClear();
			}
			if(comboBoxMultiUnsentClinics.SelectedIndices.Count==0) {
				comboBoxMultiUnsentClinics.SetSelected(0,true);
			}
		}

		private void comboBoxMultiUnsentProvs_Leave(object sender,EventArgs e) {
			if(comboBoxMultiUnsentProvs.SelectedIndices.Contains(0)) {
				comboBoxMultiUnsentProvs.SelectedIndicesClear();
			}
			if(comboBoxMultiUnsentProvs.SelectedIndices.Count==0) {
				comboBoxMultiUnsentProvs.SetSelected(0,true);
			}
		}

		private void comboBoxMultiBillTypes_Leave(object sender,EventArgs e) {
			if(comboBoxMultiBillTypes.SelectedIndices.Contains(0)) {
				comboBoxMultiBillTypes.SelectedIndicesClear();
			}
			if(comboBoxMultiBillTypes.SelectedIndices.Count==0) {
				comboBoxMultiBillTypes.SetSelected(0,true);
			}
		}

		private void FillDemandTypes() {
			TsiDemandType selectedType=comboDemandType.SelectedTag<TsiDemandType>();//If they have nothing selected, this will default to 'Accelerator'.
			List<long> listClinicNums=new List<long>();
			if(!Preferences.HasClinicsEnabled || comboBoxMultiUnsentClinics.ListSelectedIndices.Contains(0)) {
				listClinicNums=_listClinics.Select(x => x.ClinicNum).ToList();//if clinics are disabled, this will only contain the HQ "clinic"
			}
			else {
				//x-1 works because we know index 0 isn't selected(from above contains(0)) and we -1 due to the "All"" clinic
				listClinicNums=comboBoxMultiUnsentClinics.ListSelectedIndices.Select(x => _listClinics[x-1].ClinicNum).ToList();
			}
			comboDemandType.Items.Clear();
			Dictionary<long,string[]> dictClinicSelectedServices=_dictClinicProgProps
				.Where(x => x.Value.Any(y => y.Key=="SelectedServices" && !string.IsNullOrEmpty(y.Value)))
				.ToDictionary(x => x.Key,x => x.Value.Find(y => y.Key=="SelectedServices").Value.Split(','));
			if(listClinicNums.Any(x => dictClinicSelectedServices.ContainsKey(x) 
				&& dictClinicSelectedServices[x].Contains(((int)TsiDemandType.Accelerator).ToString()))) 
			{
				comboDemandType.Items.Add(new ODBoxItem<TsiDemandType>(TsiDemandType.Accelerator.GetDescription(),TsiDemandType.Accelerator));
			}
			if(listClinicNums.Any(x => dictClinicSelectedServices.ContainsKey(x) 
				&& dictClinicSelectedServices[x].Contains(((int)TsiDemandType.ProfitRecovery).ToString()))) 
			{
				comboDemandType.Items.Add(new ODBoxItem<TsiDemandType>(TsiDemandType.ProfitRecovery.GetDescription(),TsiDemandType.ProfitRecovery));
			}
			if(listClinicNums.Any(x => dictClinicSelectedServices.ContainsKey(x) 
				&& dictClinicSelectedServices[x].Contains(((int)TsiDemandType.Collection).ToString()))) 
			{
				comboDemandType.Items.Add(new ODBoxItem<TsiDemandType>(TsiDemandType.Collection.GetDescription(),TsiDemandType.Collection));
			}
			if(comboDemandType.Items.Count>0) {
				comboDemandType.SetSelectedItem<TsiDemandType>(x => x==selectedType,"");
			}
		}

		private void comboBoxMultiUnsentClinics_SelectionChangeCommitted(object sender,EventArgs e) {
			FillDemandTypes();
			FillGridUnsent();
		}

		private void comboBoxMultiUnsentProvs_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGridUnsent();
		}

		private void comboBoxMultiUnsentBillTypes_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGridUnsent();
		}

		private void comboUnsentAccountAge_SelectedIndexChanged(object sender,EventArgs e) {
			FillGridUnsent();
		}

		private void textUnsentMinBal_TextChanged(object sender,EventArgs e) {
			timerFillGrid.Enabled=false;
			timerFillGrid.Enabled=true;
		}

		private void textUnsentDaysLastPay_TextChanged(object sender,EventArgs e) {
			timerFillGrid.Enabled=false;
			timerFillGrid.Enabled=true;
		}

		private void checkExcludeInsPending_CheckedChanged(object sender,EventArgs e) {
			FillGridUnsent();
		}

		private void checkExcludeIfProcs_CheckedChanged(object sender,EventArgs e) {
			FillGridUnsent();
		}

		private void checkExcludeBadAddress_CheckedChanged(object sender,EventArgs e) {
			FillGridUnsent();
		}

		private void checkUnsentShowPatNums_CheckedChanged(object sender,EventArgs e) {
			FillGridUnsent();
		}

		private void butUnsentSaveDefault_Click(object sender,EventArgs e) {
			SaveDefaults();
		}

		private void butUnsentAll_Click(object sender,EventArgs e) {
			gridUnsent.SetSelected(true);
		}

		private void butUnsentNone_Click(object sender,EventArgs e) {
			gridUnsent.SetSelected(false);
		}

		private void butUnsentPrint_Click(object sender,EventArgs e) {
//TODO
		}

		private void butRunAging_Click(object sender,EventArgs e) {
			RunAgingIfNecessary();
			SecurityLog.Write(SecurityLogEvents.AgingRan,"Aging Ran - AR Manager");
			RefreshAll();
			FillGrids();
		}

		private void butSend_Click(object sender,EventArgs e) {
			#region Get and Validate Data
			if(!Security.IsAuthorized(Permissions.Billing)) {
				return;
			}
			if(gridUnsent.SelectedIndices.Length<1) {
				MsgBox.Show(this,"Please select accounts to send to TSI first.");
				return;
			}
			if(_collectionBillType==null) {
				if(Security.IsAuthorized(Permissions.Setup)
					&& MsgBox.Show(this,MsgBoxButtons.YesNo,"There must be a collections billing type defined in order to send accounts to TSI.  Would you like "
						+"to open the definitions window now to create a collections billing type?"))
				{
					FormDefinitions FormDefs=new FormDefinitions(DefinitionCategory.BillingTypes);
					FormDefs.ShowDialog();//no OK button, only Close which returns DialogResult.Cancel, just get the billing type again in case they created it
					_collectionBillType=Definition.GetByCategory(DefinitionCategory.BillingTypes).FirstOrDefault(x => x.Value.ToLower()=="c");
				}
				FormDefinitions FormD=new FormDefinitions(DefinitionCategory.BillingTypes);
				FormD.ShowDialog();//no OK button, only Close which returns DialogResult.Cancel, just get the billing type again in case they created it
				_collectionBillType=Definition.GetByCategory(DefinitionCategory.BillingTypes).FirstOrDefault(x => x.Value.ToLower()=="c");
				if(_collectionBillType==null) {//still no collections billing type
					MsgBox.Show(this,"Please create a collections billing type and try again later.");
					return;
				}
			}
			List<PatAging> listPatAging=GetSelectedPatAgings(gridUnsent);
			List<long> listClinicsSkipped;
			if(!ValidateSendUpdateData(listPatAging.Select(x => x.ClinicNum).ToList(),out listClinicsSkipped)) {
				return;
			}
			if(_dictClinicProgProps.All(x => listClinicsSkipped.Contains(x.Key))) {
				MsgBox.Show(this,"An SFTP connection could not be made using the connection details "+(Preferences.HasClinicsEnabled ? "for any clinic " : "")
					+"in the enabled Transworld (TSI) program link.  Accounts cannot be sent to collection until the program link is setup.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			//TSI connection details validated, at least one clinic the user has access to is setup with valid connection details
			#region Get Age of Accounts Dictionary
			DateTime dateAsOf=DateTime.Today;//used to determine when the balance on this date began
			if(Preference.GetBool(PreferenceName.AgingCalculatedMonthlyInsteadOfDaily)) {//if aging calculated monthly, use the last aging date instead of today
				dateAsOf=Preference.GetDate(PreferenceName.DateLastAging);
			}
			#endregion Get PatAgings and Age of Accounts Dictionary
			#region Validate Selected Pats and Demand Type
			Dictionary<long,string[]> dictClinicSelectedServices=_dictClinicProgProps
				.Where(x => x.Value.Any(y => y.Key=="SelectedServices" && !string.IsNullOrEmpty(y.Value)))
				.ToDictionary(x => x.Key,x => x.Value.Find(y => y.Key=="SelectedServices").Value.Split(','));
			TsiDemandType demandType=comboDemandType.SelectedTag<TsiDemandType>();
			List<long> listPatNumsToReselect=listPatAging.FindAll(x => !dictClinicSelectedServices.ContainsKey(Preferences.HasClinicsEnabled?x.ClinicNum:0)
						|| !dictClinicSelectedServices[Preferences.HasClinicsEnabled?x.ClinicNum:0].Contains(((int)demandType).ToString())).Select(x => x.PatNum).ToList();
			string msgTxt="";
			if(listPatNumsToReselect.Count > 0)	{
				Cursor=Cursors.Default;
				msgTxt=Lan.g(this,"At least one of the selected guarantors is assigned to a clinic that does not have the")+" "+demandType.GetDescription()
					+" "+Lan.g(this,"service enabled.  Those account(s) will not be sent to TSI and will remain in the unsent grid.");
				MessageBox.Show(msgTxt);
			}
			List<long> listPatNumsWrongService=new List<long>();
			if(demandType==TsiDemandType.Accelerator) {
				listPatNumsWrongService=listPatAging.FindAll(x => !listPatNumsToReselect.Contains(x.PatNum) && x.DateBalBegan.Date<dateAsOf.AddDays(-120).Date)
					.Select(x => x.PatNum).ToList();
				msgTxt=Lan.g(this,"The accelerator service is recommended for accounts 31-120 days old and one or more of the selected account balances "
					+"is over 120 days old.  You may wish to consider placing those accounts directly to the Profit Recovery or Collection service.")+"\r\n"
					+Lan.g(this,"Would you like to send the accounts to Accelerator?")+"\r\n\r\n"+Lan.g(this,"Press Yes to send all accounts to Accelerator.")
					+"\r\n\r\n"+Lan.g(this,"Press No to send only the accounts 120 days old or less to Accelerator and leave the older accounts in the unsent "
					+"grid to send to Profit Recovery or Collection later.")+"\r\n\r\n"+Lan.g(this,"Press Cancel to cancel sending all accounts.");
			}
			else if(demandType==TsiDemandType.ProfitRecovery) {
				listPatNumsWrongService=listPatAging.FindAll(x => !listPatNumsToReselect.Contains(x.PatNum) && x.DateBalBegan.Date<dateAsOf.AddDays(-180).Date)
					.Select(x => x.PatNum).ToList();
				msgTxt=Lan.g(this,"The Profit Recovery service is recommended for accounts 121-180 days old and one or more of the selected account "
					+"balances is over 180 days old.  You may wish to consider placing those accounts directly to the Collection service.")+"\r\n"
					+Lan.g(this,"Would you like to send the accounts to Profit Recovery?")+"\r\n\r\n"+Lan.g(this,"Press Yes to send all accounts to Profit "
					+"Recovery.")+"\r\n\r\n"+Lan.g(this,"Press No to send only the accounts 180 days old or less to Profit Recovery and leave the older "
					+"accounts in the unsent grid to send to Collections later.")+"\r\n\r\n"+Lan.g(this,"Press Cancel to cancel sending all accounts.");
			}
			if(listPatNumsWrongService.Count > 0) {
				Cursor=Cursors.Default;
				switch(MessageBox.Show(msgTxt,"",MessageBoxButtons.YesNoCancel)) {
					case DialogResult.No:
						listPatNumsToReselect.AddRange(listPatNumsWrongService);
						break;
					case DialogResult.Cancel:
						return;
					default:
						break;
				}
			}
			#endregion Validate Selected Pats and Demand Type
			#region Validate Birthdate and Address
			List<string> listErrorMsgs=new List<string>();
			List<long> listPatNumsBadBday=listPatAging
				.FindAll(x => !listPatNumsToReselect.Contains(x.PatNum) && (x.Birthdate.Year<1880 || x.Birthdate>DateTime.Today.AddYears(-18)))
				.Select(x => x.PatNum).ToList();
			if(listPatNumsBadBday.Count>0) {
				listErrorMsgs.Add(Lan.g(this,"Invalid birthdate or under the age of 18"));
				listPatNumsToReselect.AddRange(listPatNumsBadBday);
			}
			List<long> listPatNumsBadAddress=listPatAging
				.FindAll(x => !listPatNumsToReselect.Contains(x.PatNum)
					&& new[] { x.Address,x.City,x.State,x.Zip }.Any(y => string.IsNullOrEmpty(y)))
				.Select(x => x.PatNum).ToList();
			if(listPatNumsBadAddress.Count>0) {
				listErrorMsgs.Add(Lan.g(this,"Bad address"));
				listPatNumsToReselect.AddRange(listPatNumsBadAddress);
			}
			if(listErrorMsgs.Count>0) {
				Cursor=Cursors.Default;
				msgTxt=Lan.g(this,"One or more of the selected guarantors has the following error(s) and will not be sent to TSI")+":\r\n\r\n"
					+string.Join("\r\n",listErrorMsgs);
				MessageBox.Show(msgTxt);
			}
			#endregion Validate Birthdate and Address
			#region Validate Balances
			List<long> listPatNumsNegBal=listPatAging
				.FindAll(x => !listPatNumsToReselect.Contains(x.PatNum) && Math.Round(x.AmountDue,3) < 0.005)
				.Select(x => x.PatNum).ToList();
			if(listPatNumsNegBal.Count>0) {
				Cursor=Cursors.Default;
				msgTxt=listPatNumsNegBal.Count+" "+Lan.g(this,"of the selected guarantor(s) have a balance less than or equal to 0.  Are you sure you want "
					+"to send the account(s) to TSI?")+"\r\n\r\n"
					+Lan.g(this,"Press Yes to send the account(s) with a balance less than or equal to 0 anyway.")+"\r\n\r\n"
					+Lan.g(this,"Press No to skip the account(s) with a balance less than or equal to 0 and send the remaining account(s) to TSI.")+"\r\n\r\n"
					+Lan.g(this,"Press Cancel to cancel sending all accounts.");
				switch(MessageBox.Show(msgTxt,"",MessageBoxButtons.YesNoCancel)) {
					case DialogResult.No:
						listPatNumsToReselect.AddRange(listPatNumsNegBal);
						break;
					case DialogResult.Cancel:
						return;
					default:
						break;
				}
			}
			#endregion Validate Balances
			Cursor=Cursors.WaitCursor;
			listPatAging.RemoveAll(x => listPatNumsToReselect.Contains(x.PatNum));
			if(listPatAging.Count==0) {
				SetSelectedRows(listPatNumsToReselect,gridUnsent);
				Cursor=Cursors.Default;
				return;
			}
			//dictionary key=PatNum, value=dictionary key=Tuple<TsiFKeyType,long>, value=TsiTrans with that type and key for that pat.  Used for placement
			//msgs to keep all trans for a fam associated with the placement msg to determine later if the past account details were changed after being
			//placed for collection with Transworld.
			Dictionary<long,Dictionary<Tuple<TsiFKeyType,long>,TsiTrans>> dictPatTrans=Ledgers.GetDictTransForGuars(listPatAging.Select(x => x.PatNum).ToList());
			#endregion Get and Validate Data
			#region Create Messages and TsiTransLogs
			Dictionary<long,Dictionary<long,string>> dictClinicUpdateMsgs=new Dictionary<long,Dictionary<long,string>>();
			Dictionary<long,Dictionary<long,string>> dictClinicPlacementMsgs=new Dictionary<long,Dictionary<long,string>>();
			Dictionary<long,List<TsiTransLog>> dictClinicNumListTransLogs=new Dictionary<long,List<TsiTransLog>>();
			List<long> listFailedPatNums=new List<long>();
			foreach(PatAging pAgingCur in listPatAging) {
				long clinicNum=Preferences.HasClinicsEnabled?pAgingCur.ClinicNum:0;
				if(listClinicsSkipped.Contains(clinicNum)) {
					listFailedPatNums.Add(pAgingCur.PatNum);
					continue;
				}
				string clientID="";
				List<ProgramProperty> listProgProps;
				if(!_dictClinicProgProps.TryGetValue(clinicNum,out listProgProps) && !_dictClinicProgProps.TryGetValue(0,out listProgProps)) {
					listClinicsSkipped.Add(clinicNum);
					listFailedPatNums.Add(pAgingCur.PatNum);
					continue;
				}
				if(demandType==TsiDemandType.Accelerator) {
					clientID=listProgProps.Find(x => x.Key=="ClientIdAccelerator")?.Value??"";
				}
				else {
					clientID=listProgProps.Find(x => x.Key=="ClientIdCollection")?.Value??"";
				}
				try {
					//find most recent account change log less than 50 days ago and if it was a suspend trans send reinstate update msg instead of placement msg
					TsiTransLog logMostRecentAcctChange=pAgingCur.ListTsiLogs
						.Find(x => x.TransType.In(TsiTransType.CN,TsiTransType.PF,TsiTransType.PL,TsiTransType.PT,TsiTransType.RI,TsiTransType.SS)
							&& x.TransDateTime>=DateTime.Today.AddDays(-50));
					if(logMostRecentAcctChange!=null && logMostRecentAcctChange.TransType==TsiTransType.SS) {
						//most recent account change trans was less than 50 days ago and was to suspend the account, so generate and send update message to reinstate
						string updateStatusMsg=TsiMsgConstructor.GenerateUpdate(pAgingCur.PatNum,clientID,TsiTransType.RI,0.00,pAgingCur.AmountDue);
						if(!dictClinicUpdateMsgs.ContainsKey(clinicNum)) {
							dictClinicUpdateMsgs[clinicNum]=new Dictionary<long,string>();
						}
						dictClinicUpdateMsgs[clinicNum].Add(pAgingCur.PatNum,updateStatusMsg);
						if(!dictClinicNumListTransLogs.ContainsKey(clinicNum)) {
							dictClinicNumListTransLogs[clinicNum]=new List<TsiTransLog>();
						}
						dictClinicNumListTransLogs[clinicNum].Add(new TsiTransLog() {
							PatNum=pAgingCur.PatNum,
							UserNum=Security.CurrentUser.Id,
							TransType=TsiTransType.RI,
							//TransDateTime=DateTime.Now,//set on insert, not editable by user
							//DemandType=TsiDemandType.Accelerator,//only used for placement messages
							//ServiceCode=TsiServiceCode.Diplomatic,//only used for placement messages
							ClientId=clientID,
							TransAmt=0.00,
							AccountBalance=pAgingCur.AmountDue,
							FKeyType=TsiFKeyType.None,//only used for account trans updates
							FKey=0,//only used for account trans updates
							RawMsgText=updateStatusMsg,
							ClinicNum=clinicNum,
							DictTransByType=new Dictionary<Tuple<TsiFKeyType,long>,TsiTrans>()//sets string field TransJson to empty string
						});
					}
					else {
						string msg=TsiMsgConstructor.GeneratePlacement(pAgingCur,clientID,demandType);
						if(!dictClinicPlacementMsgs.ContainsKey(clinicNum)) {
							dictClinicPlacementMsgs[clinicNum]=new Dictionary<long,string>();
						}
						dictClinicPlacementMsgs[clinicNum].Add(pAgingCur.PatNum,msg);
						TsiTransLog logCur=new TsiTransLog() {
							PatNum=pAgingCur.PatNum,
							UserNum=Security.CurrentUser.Id,
							TransType=TsiTransType.PL,
							//TransDateTime=DateTime.Now,//set on insert, not editable by user
							DemandType=demandType,
							ServiceCode=TsiServiceCode.Diplomatic,
							ClientId=clientID,
							TransAmt=0.00,
							AccountBalance=pAgingCur.AmountDue,
							FKeyType=TsiFKeyType.None,//not used for placement messages
							FKey=0,//not used for placement messages
							RawMsgText=msg,
							ClinicNum=clinicNum,
							DictTransByType=new Dictionary<Tuple<TsiFKeyType,long>,TsiTrans>()//sets string field TransJson to empty string
						};
						Dictionary<Tuple<TsiFKeyType,long>,TsiTrans> dictPatCurTrans;
						if(dictPatTrans.TryGetValue(logCur.PatNum,out dictPatCurTrans)) {
							logCur.DictTransByType=new Dictionary<Tuple<TsiFKeyType,long>,TsiTrans>(dictPatCurTrans);//sets TransJson to Json serialized string
						}
						if(!dictClinicNumListTransLogs.ContainsKey(clinicNum)) {
							dictClinicNumListTransLogs[clinicNum]=new List<TsiTransLog>();
						}
						dictClinicNumListTransLogs[clinicNum].Add(logCur);
						if(!logCur.AccountBalance.IsEqual(logCur.DictTransByType.Sum(x => x.Value.TranAmt))) {
							throw new ApplicationException("The guarantor's amount due does not match the sum of ledger transactions.  The following guarantor was "
								+"not sent to Transworld.  Try running aging and/or Database Maintenance and then try sending this guarantor again.\r\n"
								+"Patient: "+pAgingCur.PatNum+" - "+pAgingCur.PatName);
						}
					}
				}
				catch(ApplicationException ex) {
					listFailedPatNums.Add(pAgingCur.PatNum);
					if(dictClinicUpdateMsgs.ContainsKey(clinicNum)) {
						dictClinicUpdateMsgs[clinicNum].Remove(pAgingCur.PatNum);
					}
					if(dictClinicPlacementMsgs.ContainsKey(clinicNum)) {
						dictClinicPlacementMsgs[clinicNum].Remove(pAgingCur.PatNum);
					}
					if(dictClinicNumListTransLogs.ContainsKey(clinicNum)) {
						dictClinicNumListTransLogs[clinicNum].RemoveAll(x => x.PatNum==pAgingCur.PatNum);
					}
					Cursor=Cursors.Default;
					if(MsgBox.Show(this,MsgBoxButtons.YesNo,ex.Message+"\r\nDo you want to continue attempting to send the remaining accounts?")) {
						Cursor=Cursors.WaitCursor;
						continue;
					}
					else {
						break;
					}
				}
			}
			#endregion Create Messages and TsiTransLogs
			#region Send Clinic Batch Placement Files, Insert TsiTransLogs, and Update Patient Billing Types
			foreach(KeyValuePair<long,Dictionary<long,string>> kvp in dictClinicPlacementMsgs) {
				if(kvp.Value.Count<1) {
					continue;
				}
				List<ProgramProperty> listProps=new List<ProgramProperty>();
				if(!_dictClinicProgProps.TryGetValue(kvp.Key,out listProps) && !_dictClinicProgProps.TryGetValue(0,out listProps)) {
					//should never happen, dictClinicProps should contain all clinicNums the user has access to, including clinicnum 0
					listFailedPatNums.AddRange(kvp.Value.Keys);
					continue;
				}
				string sftpAddress=listProps.Find(x => x.Key=="SftpServerAddress")?.Value??"";
				int sftpPort;
				if(!int.TryParse(listProps.Find(x => x.Key=="SftpServerPort")?.Value??"",out sftpPort)) {
					sftpPort=22;//default to port 22
				}
				string userName=listProps.Find(x => x.Key=="SftpUsername")?.Value??"";
				string userPassword=listProps.Find(x => x.Key=="SftpPassword")?.Value??"";
				byte[] fileContents=Encoding.ASCII.GetBytes(TsiMsgConstructor.GetPlacementFileHeader()+"\r\n"+string.Join("\r\n",kvp.Value.Values));
				try {
                    // TODO: Fix me
					//TaskStateUpload state=new Sftp.Upload(sftpAddress,userName,userPassword,sftpPort) {
					//	Folder="/xfer/incoming",
					//	FileName="TsiPlacements_"+DateTime.Now.ToString("yyyyMMddhhmmss")+".txt",
					//	FileContent=fileContents,
					//	HasExceptions=true
					//};
					//state.Execute(false);
				}
				catch {
					listFailedPatNums.AddRange(kvp.Value.Keys);
					continue;
				}
				//Upload was successful
				List<TsiTransLog> listLogsForInsert=new List<TsiTransLog>();
				//dictClinicNumListTransLogs should always contain the same clinicNums as dictClinicMsgs, so this should always insert the messages,
				//i.e. TryGetValue never returns false
				if(dictClinicNumListTransLogs.TryGetValue(kvp.Key,out listLogsForInsert)) {
					TsiTransLogs.InsertMany(listLogsForInsert);
				}
				Patients.UpdateAllFamilyBillingTypes(_collectionBillType.Id,kvp.Value.Keys.ToList());//mark all family members as sent to collection
			}
			#endregion Send Clinic Batch Placement Files, Insert TsiTransLogs, and Update Patient Billing Types
			#region Send Clinic Batch Update Files, Insert TsiTransLogs, and Update Patient Billing Types
			foreach(KeyValuePair<long,Dictionary<long,string>> kvp in dictClinicUpdateMsgs) {
				if(kvp.Value.Count<1) {
					continue;
				}
				List<ProgramProperty> listProps=new List<ProgramProperty>();
				if(!_dictClinicProgProps.TryGetValue(kvp.Key,out listProps) && !_dictClinicProgProps.TryGetValue(0,out listProps)) {
					//should never happen, dictClinicProps should contain all clinicNums the user has access to, including clinicnum 0
					listFailedPatNums.AddRange(kvp.Value.Keys);
					continue;
				}
				string sftpAddress=listProps.Find(x => x.Key=="SftpServerAddress")?.Value??"";
				int sftpPort;
				if(!int.TryParse(listProps.Find(x => x.Key=="SftpServerPort")?.Value??"",out sftpPort)) {
					sftpPort=22;//default to port 22
				}
				string userName=listProps.Find(x => x.Key=="SftpUsername")?.Value??"";
				string userPassword=listProps.Find(x => x.Key=="SftpPassword")?.Value??"";
				byte[] fileContents=Encoding.ASCII.GetBytes(TsiMsgConstructor.GetUpdateFileHeader()+"\r\n"+string.Join("\r\n",kvp.Value.Values));
				try {
                    // TODO: Fix me
					//TaskStateUpload state=new Sftp.Upload(sftpAddress,userName,userPassword,sftpPort) {
					//	Folder="/xfer/incoming",
					//	FileName="TsiUpdates_"+DateTime.Now.ToString("yyyyMMddhhmmss")+".txt",
					//	FileContent=fileContents,
					//	HasExceptions=true
					//};
					//state.Execute(false);
				}
				catch{
					listFailedPatNums.AddRange(kvp.Value.Keys);
					continue;
				}
				//Upload was successful
				List<TsiTransLog> listLogsForInsert=new List<TsiTransLog>();
				//dictClinicNumListTransLogs should always contain the same clinicNums as dictClinicMsgs, so this should always insert the messages,
				//i.e. TryGetValue never returns false
				if(dictClinicNumListTransLogs.TryGetValue(kvp.Key,out listLogsForInsert)) {
					TsiTransLogs.InsertMany(listLogsForInsert);
				}
				//update all family billing types to the collection bill type
				Patients.UpdateAllFamilyBillingTypes(_collectionBillType.Id,kvp.Value.Keys.ToList());
			}
			#endregion Send Clinic Batch Update Files, Insert TsiTransLogs, and Update Patient Billing Types
			#region FillGrids With Updated Info
			RefreshAll();
			FillGrids(false);
			#endregion FillGrids With Updated Info
			SetSelectedRows((listPatNumsToReselect.Union(listFailedPatNums)).ToList(),gridUnsent);
			Cursor=Cursors.Default;
			if(listFailedPatNums.Count>0) {
				MessageBox.Show(listFailedPatNums.Count+" "+Lan.g(this,"accounts did not upload successfully.  They have not been marked as sent to "
					+"TSI and will have to be resent."));
			}
		}

		#endregion Unsent Tab Methods
		#region Sent Tab Methods

		private void FillGridSent(bool retainSelection=true) {
			Cursor=Cursors.WaitCursor;
			List<long> listSelectedPatNums=new List<long>();
			if(retainSelection) {
				listSelectedPatNums=GetSelectedPatAgings(gridSent).Select(x => x.PatNum).ToList();
			}
			List<int> listPatAgingIndexFiltered=GetPatAgingIndexSentFiltered();
			#region Set Grid Title and Columns
			gridSent.BeginUpdate();
			gridSent.Columns.Clear();
			List<DisplayField> listDisplayFields=DisplayFields.GetForCategory(DisplayFieldCategory.ArManagerSentGrid);
			foreach(DisplayField fieldCur in listDisplayFields) {
				if(fieldCur.InternalName=="Clinic" && !Preferences.HasClinicsEnabled) {
					continue;//skip the clinic column if clinics are not enabled
				}
				ODGridSortingStrategy sortingStrat=ODGridSortingStrategy.StringCompare;
				HorizontalAlignment hAlign=HorizontalAlignment.Left;
				if(fieldCur.InternalName.In("0-30 Days","31-60 Days","61-90 Days","> 90 Days","Total","-Ins Est","=Patient","PayPlan Due",
					DisplayFields.InternalNames.ArManagerSentGrid.DaysBalBegan)) 
				{
					sortingStrat=ODGridSortingStrategy.AmountParse;
					hAlign=HorizontalAlignment.Right;
				}
				else if(fieldCur.InternalName.In("Last Paid","Last Proc",DisplayFields.InternalNames.ArManagerSentGrid.DateBalBegan)) {
					sortingStrat=ODGridSortingStrategy.DateParse;
					hAlign=HorizontalAlignment.Center;
				}
				else if(!fieldCur.InternalName.In("Guarantor","Clinic","Prov","Demand Type","Last Transaction")) {
					continue;//shouldn't happen, but the loop to fill the rows will skip any columns that aren't one of these so we'll skip here as well
				}
				gridSent.Columns.Add(new ODGridColumn(string.IsNullOrEmpty(fieldCur.Description)?fieldCur.InternalName:fieldCur.Description,
					fieldCur.ColumnWidth,hAlign,sortingStrat));
			}
			//this form initially set to the max allowed (by OD) form size 1246, which is also the minimum size for this form.  If the user resizes the form
			//to be larger, increase each column width by the same ratio to spread out the additional real estate
			int widthColsAndScroll=gridSent.WidthAllColumns+20;//+20 for vertical scroll bar
			if(widthColsAndScroll<gridSent.Width) {
				//don't grow/shrink column widths until all columns are visible, i.e no horizontal scroll bar active, all columns fully visible
				gridSent.Columns.ToList().ForEach(x => x.Width=(int)Math.Round((float)x.Width*gridSent.Width/widthColsAndScroll,MidpointRounding.AwayFromZero));
				//increase the last col width to be the full width of the grid (to account for rounding away from zero above)
				gridSent.Columns[gridSent.Columns.Count-1].Width-=gridSent.WidthAllColumns+20-gridSent.Width;
			}
			#endregion Set Grid Title and Columns
			#region Fill Grid Rows
			gridSent.Rows.Clear();
			Dictionary<long,string> dictClinicAbbrs=_listClinics.ToDictionary(x => x.ClinicNum,x => x.Abbr);
			Dictionary<long,string> dictProvAbbrs=_listProviders.ToDictionary(x => x.ProvNum,x => x.Abbr);
			double bal0_30=0;
			double bal31_60=0;
			double bal61_90=0;
			double balOver90=0;
			double balTotal=0;
			double insEst=0;
			double amtDue=0;
			double ppDue=0;
			ODGridRow row;
			List<int> listIndicesToReselect=new List<int>();
			foreach(int i in listPatAgingIndexFiltered) {
				PatAging patAgeCur=_listPatAgingSentAll[i];
				bal0_30+=patAgeCur.Bal_0_30;
				bal31_60+=patAgeCur.Bal_31_60;
				bal61_90+=patAgeCur.Bal_61_90;
				balOver90+=patAgeCur.BalOver90;
				balTotal+=patAgeCur.BalTotal;
				insEst+=patAgeCur.InsEst;
				amtDue+=patAgeCur.AmountDue;
				ppDue+=patAgeCur.PayPlanDue;
				row=new ODGridRow();
				foreach(DisplayField field in listDisplayFields) {
					switch(field.InternalName) {
						case "Guarantor":
							row.Cells.Add((checkSentShowPatNums.Checked?(patAgeCur.PatNum.ToString()+" - "):"")+patAgeCur.PatName);
							continue;
						case "Clinic":
							if(!Preferences.HasClinicsEnabled) {
								continue;//skip the clinic column if clinics are not enabled
							}
							string clinicAbbr;
							row.Cells.Add(dictClinicAbbrs.TryGetValue(patAgeCur.ClinicNum,out clinicAbbr)?clinicAbbr:"");
							continue;
						case "Prov":
							string provAbbr;
							row.Cells.Add(dictProvAbbrs.TryGetValue(patAgeCur.PriProv,out provAbbr)?provAbbr:"");
							continue;
						case "0-30 Days":
							row.Cells.Add(patAgeCur.Bal_0_30.ToString("n"));
							continue;
						case "31-60 Days":
							row.Cells.Add(patAgeCur.Bal_31_60.ToString("n"));
							continue;
						case "61-90 Days":
							row.Cells.Add(patAgeCur.Bal_61_90.ToString("n"));
							continue;
						case "> 90 Days":
							row.Cells.Add(patAgeCur.BalOver90.ToString("n"));
							continue;
						case "Total":
							row.Cells.Add(patAgeCur.BalTotal.ToString("n"));
							continue;
						case "-Ins Est":
							row.Cells.Add(patAgeCur.InsEst.ToString("n"));
							continue;
						case "=Patient":
							row.Cells.Add(patAgeCur.AmountDue.ToString("n"));
							continue;
						case "PayPlan Due":
							row.Cells.Add(patAgeCur.PayPlanDue.ToString("n"));
							continue;
						case "Last Paid":
							row.Cells.Add(patAgeCur.DateLastPay.Year>1880?patAgeCur.DateLastPay.ToString("d"):"");
							continue;
						case "Demand Type":
							TsiTransLog placementlog=patAgeCur.ListTsiLogs.FirstOrDefault(x => x.TransType==TsiTransType.PL);
							row.Cells.Add(placementlog!=null?placementlog.DemandType.GetDescription():"");
							continue;
						case "Last Transaction":
							string lastTransTypeDate="";
							if(patAgeCur.ListTsiLogs.Count>0) {
								lastTransTypeDate=patAgeCur.ListTsiLogs[0].TransType.GetDescription()+" - "+patAgeCur.ListTsiLogs[0].TransDateTime.ToString("g");
							}
							row.Cells.Add(lastTransTypeDate);
							continue;
						case "Last Proc":
							row.Cells.Add(patAgeCur.DateLastProc.Year>1880?patAgeCur.DateLastProc.ToString("d"):"");
							continue;
						case DisplayFields.InternalNames.ArManagerSentGrid.DateBalBegan:
							row.Cells.Add(patAgeCur.DateBalBegan.Year>1880?patAgeCur.DateBalBegan.ToString("d"):"");
							continue;
						case DisplayFields.InternalNames.ArManagerSentGrid.DaysBalBegan:
							row.Cells.Add(patAgeCur.DateBalBegan.Year>1880?(DateTime.Today-patAgeCur.DateBalBegan).Days.ToString():"");
							continue;
						default:
							continue;//skip any columns not defined here, as we did above when adding the columns
					}
				}
				row.Tag=i;
				gridSent.Rows.Add(row);
				if(retainSelection && listSelectedPatNums.Contains(patAgeCur.PatNum)) {
					listIndicesToReselect.Add(gridSent.Rows.Count-1);
				}
			}
			gridSent.EndUpdate();
			#endregion Fill Grid Rows
			groupUpdateAccounts.Enabled=(gridSent.Rows.Count>0);
			if(retainSelection && listIndicesToReselect.Count>0) {
				gridSent.SetSelected(listIndicesToReselect.ToArray(),true);
			}
			SetTotalsLocAndVisible(DisplayFieldCategory.ArManagerSentGrid,gridSent.HScrollValue);
			textSentTotalNumAccts.Text=listPatAgingIndexFiltered.Count.ToString();
			textSent0to30.Text=bal0_30.ToString("n");
			textSent31to60.Text=bal31_60.ToString("n");
			textSent61to90.Text=bal61_90.ToString("n");
			textSentOver90.Text=balOver90.ToString("n");
			textSentTotal.Text=balTotal.ToString("n");
			textSentInsEst.Text=insEst.ToString("n");
			textSentPatient.Text=amtDue.ToString("n");
			textSentPayPlanDue.Text=ppDue.ToString("n");
			Cursor=Cursors.Default;
		}

		private List<int> GetPatAgingIndexSentFiltered() {
			List<int> retval=new List<int>();
			#region Validate Inputs
			if(textSentMinBal.errorProvider1.GetError(textSentMinBal)!="" || textSentDaysLastPay.errorProvider1.GetError(textSentDaysLastPay)!="") {
				MsgBox.Show(this,"Please fix data entry errors in Sent tab first.");//return empty list, filter inputs cannot be applied since there are errors
				return retval;
			}
			#endregion Validate Inputs
			#region Get Filter Data
			double minBalance=Math.Round(PIn.Double(textSentMinBal.Text),3);
			DateTime dtLastPay=DateTime.Today.AddDays(-PIn.Int(textSentDaysLastPay.Text));
			AgeOfAccount accountAge=new[] { AgeOfAccount.Any,AgeOfAccount.Over30,AgeOfAccount.Over60,AgeOfAccount.Over90 }[comboSentAccountAge.SelectedIndex];
			List<TsiTransType> listTranTypes=new List<TsiTransType>();
			if(!comboBoxMultiLastTransType.ListSelectedIndices.Contains(0)) {
				listTranTypes=comboBoxMultiLastTransType.ListSelectedIndices.Select(x => _listSentTabTransTypes[x-1]).ToList();
			}
			List<long> listProvNums=new List<long>();
			if(!comboBoxMultiSentProvs.ListSelectedIndices.Contains(0)) {
				listProvNums=comboBoxMultiSentProvs.ListSelectedIndices.Select(x => _listProviders[x-1].ProvNum).ToList();
			}
			List<long> listClinicNums=new List<long>();
			if(Preferences.HasClinicsEnabled) {
				if(comboBoxMultiSentClinics.ListSelectedIndices.Contains(0)) {
					listClinicNums=_listClinics.Select(x => x.ClinicNum).ToList();
				}
				else {
					//x-1 works because we know index 0 isn't selected(from above contains(0)) and we -1 for All clinics
					listClinicNums=comboBoxMultiSentClinics.ListSelectedIndices.Select(x => _listClinics[x-1].ClinicNum).ToList();
				}
			}
			#endregion Get Filter Data
			#region Apply Filter Data to PatAging List
			for(int i=0;i<_listPatAgingSentAll.Count;i++) {
				PatAging pAge=_listPatAgingSentAll[i];
				long mostRecentLogClinicNum=pAge.ListTsiLogs.FirstOrDefault()?.ClinicNum??0;
				if(Math.Round(pAge.AmountDue,3) >= minBalance
					&& (dtLastPay.Date>=DateTime.Today.Date || pAge.DateLastPay.Date<dtLastPay.Date)
					&& (listTranTypes.Count==0 || pAge.ListTsiLogs.Count<1 || listTranTypes.Contains(pAge.ListTsiLogs[0].TransType))
					&& (listProvNums.Count==0 || listProvNums.Contains(pAge.PriProv))
					&& (listClinicNums.Count==0 || listClinicNums.Contains(pAge.ClinicNum) || listClinicNums.Contains(mostRecentLogClinicNum))
					&& ( ((int)accountAge < 4 && pAge.BalOver90 > 0.005)//if Any, Over30, Over60 or Over90 are selected, check BalOver90
						|| ((int)accountAge < 3 && pAge.Bal_61_90 > 0.005)//if Any, Over30 or Over60 are selected, check Bal_61_90
						|| ((int)accountAge < 2 && pAge.Bal_31_60 > 0.005)//if Any or Over30 are selected, check Bal_31_60
						|| (int)accountAge < 1 ))//or if Any bal is selected
				{
					retval.Add(i);
				}
			}
			#endregion Apply Filter Data to PatAging List
			return retval;
		}

		private void gridSentMain_MouseDown(object sender,MouseEventArgs e) {
			if(e.Button==MouseButtons.Right){
				gridSent.SetSelected(false);
			}
			else {
				_listSelectedPatNums=GetSelectedPatAgings(gridSent).Select(x => x.PatNum).ToList();
			}
		}

		private void gridSentMain_OnHScroll(object sender,ScrollEventArgs e) {
			SetTotalsLocAndVisible(DisplayFieldCategory.ArManagerSentGrid,e.NewValue);
		}

		private void gridSentMain_OnSortByColumn(object sender,EventArgs e) {
			SetSelectedRows(_listSelectedPatNums,gridSent);
		}

		private void comboBoxMultiSentClinics_Leave(object sender,EventArgs e) {
			if(comboBoxMultiSentClinics.SelectedIndices.Contains(0)) {
				comboBoxMultiSentClinics.SelectedIndicesClear();
			}
			if(comboBoxMultiSentClinics.SelectedIndices.Count==0) {
				comboBoxMultiSentClinics.SetSelected(0,true);
			}
		}

		private void comboBoxMultiSentProvs_Leave(object sender,EventArgs e) {
			if(comboBoxMultiSentProvs.SelectedIndices.Contains(0)) {
				comboBoxMultiSentProvs.SelectedIndicesClear();
			}
			if(comboBoxMultiSentProvs.SelectedIndices.Count==0) {
				comboBoxMultiSentProvs.SetSelected(0,true);
			}
		}

		private void comboBoxMultiLastTransType_Leave(object sender,EventArgs e) {
			if(comboBoxMultiLastTransType.SelectedIndices.Contains(0)) {
				comboBoxMultiLastTransType.SelectedIndicesClear();
			}
			if(comboBoxMultiLastTransType.SelectedIndices.Count==0) {
				comboBoxMultiLastTransType.SetSelected(0,true);
			}
		}

		private void comboBoxMultiSentClinics_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGridSent();
		}

		private void comboBoxMultiSentProvs_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGridSent();
		}

		private void comboBoxMultiLastTransType_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGridSent();
		}

		private void comboSentAccountAge_SelectedIndexChanged(object sender,EventArgs e) {
			FillGridSent();
		}

		private void textSentMinBal_TextChanged(object sender,EventArgs e) {
			timerFillGrid.Enabled=false;
			timerFillGrid.Enabled=true;
		}

		private void textSentDaysLastPay_TextChanged(object sender,EventArgs e) {
			timerFillGrid.Enabled=false;
			timerFillGrid.Enabled=true;
		}

		private void comboNewStatus_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboNewStatus.SelectedIndex<0 || comboNewStatus.SelectedIndex>_listNewStatuses.Count-1) {
				return;
			}
			if(comboNewBillType.SelectedIndex<0 || comboNewBillType.SelectedIndex>_listBillTypesNoColl.Count-1) {
				errorProvider1.SetError(comboNewBillType,Lan.g(this,"Select billing type for accounts that will no longer be managed by Transworld."));
			}
		}

		private void comboNewBillType_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboNewBillType.SelectedIndex<0 || comboNewBillType.SelectedIndex>_listBillTypesNoColl.Count-1) {
				errorProvider1.SetError(comboNewBillType,Lan.g(this,"Select billing type for accounts that will no longer be managed by Transworld."));
				return;
			}
			errorProvider1.SetError(comboNewBillType,"");
		}

		private void checkSentShowPatNums_CheckedChanged(object sender,EventArgs e) {
			FillGridSent();
		}

		private void butSentSaveDefaults_Click(object sender,EventArgs e) {
			SaveDefaults();
		}

		private void butSentAll_Click(object sender,EventArgs e) {
			gridSent.SetSelected(true);
		}

		private void butSentNone_Click(object sender,EventArgs e) {
			gridSent.SetSelected(false);
		}

		private void butSentPrint_Click(object sender,EventArgs e) {
//TODO
		}

		private void butUpdateStatus_Click(object sender,EventArgs e) {
			#region Get and Validate Data
			if(!Security.IsAuthorized(Permissions.Billing)) {
				return;
			}
			if(comboNewStatus.SelectedIndex<0 || comboNewStatus.SelectedIndex>=_listNewStatuses.Count) {
				MsgBox.Show(this,"Please select a new status first.");
				return;
			}
			if(gridSent.SelectedIndices.Length<1) {
				MsgBox.Show(this,"Please select accounts to update first.");
				return;
			}
			List<PatAging> listPatAging=GetSelectedPatAgings(gridSent);
			List<long> listClinicsSkipped;
			if(!ValidateSendUpdateData(listPatAging.Select(x => x.ClinicNum).ToList(),out listClinicsSkipped)) {
				return;
			}
			if(_dictClinicProgProps.All(x => listClinicsSkipped.Contains(x.Key))) {
				MsgBox.Show(this,"An SFTP connection could not be made using the connection details "+(Preferences.HasClinicsEnabled ? "for any clinic " : "")
					+"in the enabled Transworld (TSI) program link.  Account statuses cannot be updated with TSI until the program link is setup.");
				return;
			}
			TsiTransType transType=_listNewStatuses[comboNewStatus.SelectedIndex];
			if(transType==TsiTransType.SS) {
				string msgTxt=Lan.g(this,"The account(s) will be suspended for a maximum of 50 days but only if they are in the Accelerator or Profit Recovery "
					+"stage.  Accounts in the Transworld Systems Collection stage will NOT be suspended and will have to be reinstated from the unsent grid.  "
					+"During the 50 day suspension you may reinstate the account(s) at any time.  However, after 50 days has passed, the account(s) will expire "
					+"and will no longer be available to reinstate.")+"\r\n\r\n"+Lan.g(this,"Do you want to suspend the service for the selected account(s)?");
				if(MessageBox.Show(msgTxt,"",MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
					return;
				}
			}
			if(comboNewBillType.SelectedIndex<0 || comboNewBillType.SelectedIndex>=_listBillTypesNoColl.Count) {
				MsgBox.Show(this,"Please select a new billing type to assign to the guarantors that are no longer going to be managed by Transworld.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			long newBillType=_listBillTypesNoColl[comboNewBillType.SelectedIndex].Id;
			#endregion Get and Validate Data
			#region Create Messages and TsiTransLogs
			//TSI connection details validated, at least one clinic the user has access to is setup with valid connection details
			Dictionary<long,Dictionary<long,string>> dictClinicMsgs=new Dictionary<long,Dictionary<long,string>>();
			Dictionary<long,List<TsiTransLog>> dictClinicNumListTransLogs=new Dictionary<long,List<TsiTransLog>>();
			List<long> listFailedPatNums=new List<long>();
			foreach(PatAging pAgingCur in listPatAging) {
				long clinicNum=Preferences.HasClinicsEnabled?pAgingCur.ClinicNum:0;
				if(listClinicsSkipped.Contains(clinicNum)) {
					listFailedPatNums.Add(pAgingCur.PatNum);
					continue;
				}
				List<ProgramProperty> listProgProps;
				if(!_dictClinicProgProps.TryGetValue(clinicNum,out listProgProps) && !_dictClinicProgProps.TryGetValue(0,out listProgProps)) {
					listFailedPatNums.Add(pAgingCur.PatNum);
					listClinicsSkipped.Add(clinicNum);
					continue;
				}
				string clientId="";
				if(pAgingCur.ListTsiLogs.Count>0) {
					clientId=pAgingCur.ListTsiLogs[0].ClientId;
				}
				if(string.IsNullOrEmpty(clientId)) {
					clientId=listProgProps.Find(x => x.Key=="ClientIdAccelerator")?.Value;
				}
				if(string.IsNullOrEmpty(clientId)) {
					clientId=listProgProps.Find(x => x.Key=="ClientIdCollection")?.Value;
				}
				if(string.IsNullOrEmpty(clientId)) {
					listFailedPatNums.Add(pAgingCur.PatNum);
					listClinicsSkipped.Add(clinicNum);
					continue;
				}
				try {
					string msg=TsiMsgConstructor.GenerateUpdate(pAgingCur.PatNum,clientId,transType,0.00,pAgingCur.AmountDue);
					if(!dictClinicMsgs.ContainsKey(clinicNum)) {
						dictClinicMsgs[clinicNum]=new Dictionary<long,string>();
					}
					dictClinicMsgs[clinicNum].Add(pAgingCur.PatNum,msg);
					if(!dictClinicNumListTransLogs.ContainsKey(clinicNum)) {
						dictClinicNumListTransLogs[clinicNum]=new List<TsiTransLog>();
					}
					dictClinicNumListTransLogs[clinicNum].Add(new TsiTransLog() {
						PatNum=pAgingCur.PatNum,
						UserNum=Security.CurrentUser.Id,
						TransType=transType,
						//TransDateTime=DateTime.Now,//set on insert, not editable by user
						//DemandType=TsiDemandType.Accelerator,//only valid for placement msgs
						//ServiceCode=TsiServiceCode.Diplomatic,//only valid for placement msgs
						ClientId=clientId,
						TransAmt=0.00,
						AccountBalance=pAgingCur.AmountDue,
						FKeyType=TsiFKeyType.None,//only used for account trans updates
						FKey=0,//only used for account trans updates
						RawMsgText=msg,
						ClinicNum=clinicNum
						//,TransJson=""//only valid for placement msgs
					});
				}
				catch(ApplicationException ex) {
					listFailedPatNums.Add(pAgingCur.PatNum);
					if(dictClinicMsgs.ContainsKey(clinicNum)) {
						dictClinicMsgs[clinicNum].Remove(pAgingCur.PatNum);
					}
					if(dictClinicNumListTransLogs.ContainsKey(clinicNum)) {
						dictClinicNumListTransLogs[clinicNum].RemoveAll(x => x.PatNum==pAgingCur.PatNum);
					}
					Cursor=Cursors.Default;
					if(MsgBox.Show(this,MsgBoxButtons.YesNo,ex.Message+"\r\nDo you want to continue attempting to send any remaining accounts?")) {
						Cursor=Cursors.WaitCursor;
						continue;
					}
					else {
						break;
					}
				}
			}
			#endregion Create Messages and TsiTransLogs
			#region Send Clinic Batch Files, Insert TsiTransLogs, and Update Patient Billing Types
			foreach(KeyValuePair<long,Dictionary<long,string>> kvp in dictClinicMsgs) {
				if(kvp.Value.Count<1) {
					continue;
				}
				List<ProgramProperty> listProps=new List<ProgramProperty>();
				if(!_dictClinicProgProps.TryGetValue(kvp.Key,out listProps) && !_dictClinicProgProps.TryGetValue(0,out listProps)) {
					//should never happen, dictClinicProps should contain all clinicNums the user has access to, including clinicnum 0
					listFailedPatNums.AddRange(kvp.Value.Keys);
					continue;
				}
				string sftpAddress=listProps.Find(x => x.Key=="SftpServerAddress")?.Value??"";
				int sftpPort;
				if(!int.TryParse(listProps.Find(x => x.Key=="SftpServerPort")?.Value??"",out sftpPort)) {
					sftpPort=22;//default to port 22
				}
				string userName=listProps.Find(x => x.Key=="SftpUsername")?.Value??"";
				string userPassword=listProps.Find(x => x.Key=="SftpPassword")?.Value??"";
				byte[] fileContents=Encoding.ASCII.GetBytes(TsiMsgConstructor.GetUpdateFileHeader()+"\r\n"+string.Join("\r\n",kvp.Value.Values));
				try {
                    // TODO: Fix me
					//TaskStateUpload state=new Sftp.Upload(sftpAddress,userName,userPassword,sftpPort) {
					//	Folder="/xfer/incoming",
					//	FileName="TsiUpdates_"+DateTime.Now.ToString("yyyyMMddhhmmss")+".txt",
					//	FileContent=fileContents,
					//	HasExceptions=true
					//};
					//state.Execute(false);
				}
				catch {
					listFailedPatNums.AddRange(kvp.Value.Keys);
					continue;
				}
				//Upload was successful
				List<TsiTransLog> listLogsForInsert=new List<TsiTransLog>();
				//dictClinicNumListTransLogs should always contain the same clinicNums as dictClinicMsgs, so this should always insert the messages,
				//i.e. TryGetValue never returns false
				if(dictClinicNumListTransLogs.TryGetValue(kvp.Key,out listLogsForInsert)) {
					TsiTransLogs.InsertMany(listLogsForInsert);
				}
				//update all family billing types to the collection bill type if transtype is reinstated, otherwise to the selected new billing type
				Patients.UpdateAllFamilyBillingTypes((transType==TsiTransType.RI?_collectionBillType.Id:newBillType),kvp.Value.Keys.ToList());
			}
			#endregion Send Clinic Batch Files, Insert TsiTransLogs, and Update Patient Billing Types
			RefreshAll();
			FillGrids(false);
			Cursor=Cursors.Default;
			if(listFailedPatNums.Count>0) {
				MessageBox.Show(listFailedPatNums.Count+" "+Lan.g(this,"accounts did not upload successfully.  They have not been marked as sent to "
					+"collection and will have to be resent."));
				SetSelectedRows(listFailedPatNums,gridSent);
			}
		}

		#endregion Sent Tab Methods

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		private void FormArManager_FormClosing(object sender,FormClosingEventArgs e) {
			timerFillGrid?.Dispose();
			_toolTipUnsentErrors?.Dispose();
			_listPatAgingSentAll?.Clear();
			_listPatAgingSentAll=null;
			_listPatAgingUnsentAll?.Clear();
			_listPatAgingUnsentAll=null;
		}

	}
}