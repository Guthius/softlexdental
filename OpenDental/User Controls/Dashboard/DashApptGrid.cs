using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class DashApptGrid:UserControl,IDashWidgetField {
		public const string SheetFieldName="ApptsGrid";

		public Patient PatCur;
		private List<ApptOther> _listApptOthers=new List<ApptOther>();
		private List<PlannedAppt> _listPlannedAppts=new List<PlannedAppt>();
		private List<PlannedAppt> _listPlannedIncompletes=new List<PlannedAppt>();
		private List<Definition> _listProgNoteColorDefs=new List<Definition>();
		private Action _actionFillFamily;
		public bool IsShowCompletePlanned;
		private bool _isInDashboard;

		///<summary>Returns the selected ApptOther.  Returns null if no ApptOther is selected.</summary>
		public ApptOther SelectedApptOther {
			get {
				if(gridMain.GetSelectedIndex()==-1) {
					return null;
				}
				return _listApptOthers.FirstOrDefault(x => x.AptNum==gridMain.SelectedTag<long>());
			}
		}

		public List<ApptOther> ListApptOthers {
			get {
				return _listApptOthers;
			}
		}

		public DashApptGrid() {
			InitializeComponent();
			gridMain.MouseDown+=OnChildMouseDown;
			gridMain.DragOver+=OnChildDragOver;
			gridMain.DragDrop+=OnChildDragDrop;
		}

		private void DashApptGrid_Load(object sender,EventArgs e) {
			AppointmentEvent.Fired+=AppointmentEvent_Fired;//Only subscribe to this event if actually showing.
			//Need to be able to unsubscribe when the Parent's handle is destroyed, otherwise this subscription sticks around, i.e. memory leak that will 
			//cause the AppointmentEvent_Fired handler to run for the Parent even though it's already "closed" (though not actually disposed due to this 
			//subscription).
			this.Parent.HandleDestroyed+=UnsubscribeApptEvent;
		}

		private void UnsubscribeApptEvent(object sender,EventArgs e) {
			AppointmentEvent.Fired-=AppointmentEvent_Fired;
		}

		private void AppointmentEvent_Fired(ODEventArgs e) {
			bool isRefreshRequired=false;
			List<Appointment> listAppts=new List<Appointment>();
			if(e.Tag is Appointment) {
				listAppts.Add((Appointment)e.Tag);
			}
			else if(e.Tag is List<Appointment>) {
				listAppts=(List<Appointment>)e.Tag;
			}
			else {
				return;//Event fired with unexpected Tag.
			}
			foreach(Appointment appt in listAppts) {
				if(appt.PatNum==PatCur.PatNum) {
					isRefreshRequired=true;
					break;
				}
			}
			if(isRefreshRequired) {
				//_isInDashboard flag will already be set by this point which is used for enabling/disabling the vertical scroll bar when this control is 
				//used in the Patient Dashboard, so we don't care if we pass a sheetField or not.
				RefreshData(PatCur,null);
				RefreshView();
			}
		}

		private void OnChildMouseDown(object sender,MouseEventArgs e) {
			//Convert the click location to a position relative to the UserControlDashboardWidget.
			int x=e.X+((Control)sender).Location.X;
			int y=e.Y+((Control)sender).Location.Y;
			OnMouseDown(new MouseEventArgs(e.Button,e.Clicks,x,y,e.Delta));
			base.OnMouseDown(e);
		}

		private void OnChildDragOver(object sender,DragEventArgs drgevent) {
			OnDragOver(drgevent);
		}

		private void OnChildDragDrop(object sender,DragEventArgs drgevent) {
			OnDragDrop(drgevent);
		}

		public void SetFillFamilyAction(Action action) {
			_actionFillFamily=action;
		}

		public void ScrollToEnd() {
			gridMain.ScrollToEnd();
		}

		public void RefreshAppts() {
			RefreshData(PatCur,null);
			RefreshView();
		}

		public void RefreshData(Patient pat,SheetField sheetField) {
			PatCur=pat;
			if(sheetField!=null) {
				_isInDashboard=true;
			}
			if(PatCur==null) {
				return;
			}
			_listApptOthers=Appointments.GetApptOthersForPat(PatCur.PatNum);
			_listPlannedAppts=PlannedAppts.Refresh(PatCur.PatNum);
			_listPlannedIncompletes=_listPlannedAppts.FindAll(x => !_listApptOthers.ToList()
				.Exists(y => y.NextAptNum==x.AptNum && y.AptStatus==ApptStatus.Complete))
				.OrderBy(x => x.ItemOrder).ToList();
			_listProgNoteColorDefs=Definition.GetByCategory(DefinitionCategory.ProgNoteColors);;
		}

		public void RefreshView() {
			if(_isInDashboard) {
				//Enable horizontal scrolling so call to ODGrid.ComputeColumns() does not disable vertical scrolling when in the PatientDashboard.
				gridMain.HScrollVisible=true;
			}
			FillGrid();
			if(Parent.Width<gridMain.Width || Width<gridMain.Columns.Sum(x => x.Width)) {
				gridMain.HScrollVisible=true;
			}
			else {
				gridMain.HScrollVisible=false;
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			int currentSelection=e.Row;
			int currentScroll=gridMain.ScrollValue;
			long aptNum=gridMain.SelectedTag<long>();//Tag is AptNum
			FormApptEdit formApptEdit=new FormApptEdit(aptNum);
			formApptEdit.IsInViewPatAppts=true;
			formApptEdit.PinIsVisible=true;
			formApptEdit.ShowDialog();
			if(formApptEdit.DialogResult!=DialogResult.OK) {
				return;
			}
			if(formApptEdit.PinClicked) {
				SendToPinboardEvent.Fire(ODEventType.SendToPinboard,new PinBoardArgs(PatCur,SelectedApptOther,_listApptOthers));
			}
			else{
				RefreshData(PatCur,null);
				_actionFillFamily?.Invoke();
				FillGrid();
				gridMain.SetSelected(currentSelection,true);
				gridMain.ScrollValue=currentScroll;
			}
		}

		private void FillGrid() {
			long selectedApptOtherNum=SelectedApptOther?.AptNum??-1;
			int selectedIndex=-1;
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("FormApptsOther","Appt Status"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormApptsOther","Prov"),50);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormApptsOther","Clinic"),80);
		    gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormApptsOther","Date"),70);//If the order changes, reflect the change for dateIndex below.
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormApptsOther","Time"),70);//Must immediately follow Date column.
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormApptsOther","Min"),40);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormApptsOther","Procedures"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormApptsOther","Notes"),320);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			int dateIndex=3;

			for(int i=0;i<_listApptOthers.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listApptOthers[i].AptStatus.ToString());
				row.Cells.Add(Providers.GetAbbr(_listApptOthers[i].ProvNum));
                row.Cells.Add(Clinic.GetById(_listApptOthers[i].ClinicNum).Abbr);
				row.Cells.Add("");//Date
				row.Cells.Add("");//Time
				if(_listApptOthers[i].AptDateTime.Year > 1880) {
					//only regular still scheduled appts
					if(_listApptOthers[i].AptStatus!=ApptStatus.Planned && _listApptOthers[i].AptStatus!=ApptStatus.PtNote 
						&& _listApptOthers[i].AptStatus!=ApptStatus.PtNoteCompleted && _listApptOthers[i].AptStatus!=ApptStatus.UnschedList 
						&& _listApptOthers[i].AptStatus!=ApptStatus.Broken) 
					{
						row.Cells[dateIndex].Text=_listApptOthers[i].AptDateTime.ToString("d");
						row.Cells[dateIndex+1].Text=_listApptOthers[i].AptDateTime.ToString("t");
						if(_listApptOthers[i].AptDateTime < DateTime.Today) { //Past
							row.BackColor=_listProgNoteColorDefs[11].Color;
							row.ColorText=_listProgNoteColorDefs[10].Color;
						}
						else if(_listApptOthers[i].AptDateTime.Date==DateTime.Today.Date) { //Today
							row.BackColor=_listProgNoteColorDefs[9].Color;
							row.ColorText=_listProgNoteColorDefs[8].Color;
							row.Cells[0].Text=Lan.g(this,"Today");
						}
						else if(_listApptOthers[i].AptDateTime > DateTime.Today) { //Future
							row.BackColor=_listProgNoteColorDefs[13].Color;
							row.ColorText=_listProgNoteColorDefs[12].Color;
						}
					}
					else if(_listApptOthers[i].AptStatus==ApptStatus.Planned) { //show line for planned appt
						row.BackColor=_listProgNoteColorDefs[17].Color;
						row.ColorText=_listProgNoteColorDefs[16].Color;
						string txt=Lan.g("enumApptStatus","Planned")+" ";
						int plannedAptIdx=_listPlannedIncompletes.FindIndex(x => x.AptNum==_listApptOthers[i].AptNum);
						if(IsShowCompletePlanned) {
							for(int p=0;p<_listPlannedAppts.Count;p++) {
								if(_listPlannedAppts[p].AptNum==_listApptOthers[i].AptNum) {
									txt+="#"+_listPlannedAppts[p].ItemOrder.ToString();
								}
							}
						}
						else {
							if(plannedAptIdx>=0) {
								txt+="#"+(plannedAptIdx+1);
							}
							else {
								continue;
							}
						}
						if(plannedAptIdx<0) {//attached to a completed appointment
							txt+=" ("+Lan.g("enumApptStatus",ApptStatus.Complete.ToString())+")";
						}
						if(_listApptOthers.ToList().FindAll(x => x.NextAptNum==_listApptOthers[i].AptNum)
							.Exists(x => x.AptStatus==ApptStatus.Scheduled)) //attached to a scheduled appointment
						{
							txt+=" ("+Lan.g("enumApptStatus",ApptStatus.Scheduled.ToString())+")";
						}
						row.Cells[0].Text=txt;
					}
					else if(_listApptOthers[i].AptStatus==ApptStatus.PtNote) {
						row.BackColor=_listProgNoteColorDefs[19].Color;
						row.ColorText=_listProgNoteColorDefs[18].Color;
						row.Cells[0].Text=Lan.g("enumApptStatus","PtNote");
					}
					else if(_listApptOthers[i].AptStatus==ApptStatus.PtNoteCompleted) {
						row.BackColor=_listProgNoteColorDefs[21].Color;
						row.ColorText=_listProgNoteColorDefs[20].Color;
						row.Cells[0].Text=Lan.g("enumApptStatus","PtNoteCompleted");
					}
					else if(_listApptOthers[i].AptStatus==ApptStatus.Broken) {
						row.Cells[0].Text=Lan.g("enumApptStatus","Broken");
						row.Cells[dateIndex].Text=_listApptOthers[i].AptDateTime.ToString("d");
						row.Cells[dateIndex+1].Text=_listApptOthers[i].AptDateTime.ToString("t");
						row.BackColor=_listProgNoteColorDefs[15].Color;
						row.ColorText=_listProgNoteColorDefs[14].Color;
					}
					else if(_listApptOthers[i].AptStatus==ApptStatus.UnschedList) {
						row.Cells[0].Text=Lan.g("enumApptStatus","UnschedList");
						row.BackColor=_listProgNoteColorDefs[15].Color;
						row.ColorText=_listProgNoteColorDefs[14].Color;
					}
				}
				row.Cells.Add((_listApptOthers[i].Pattern.Length * 5).ToString());
				row.Cells.Add(_listApptOthers[i].ProcDescript);
				row.Cells.Add(_listApptOthers[i].Note);
				row.Tag=_listApptOthers[i].AptNum;
				gridMain.Rows.Add(row);
				if((long)row.Tag==selectedApptOtherNum) {
					selectedIndex=i;
				}
			}
			gridMain.EndUpdate();
			if(selectedIndex>-1) {
				gridMain.SetSelected(selectedIndex,true);
			}
		}
	}
	
	public class PinBoardArgs {
		public ApptOther ApptOther;
		public List<ApptOther> ListApptOthers;
		public Patient Pat;

		public PinBoardArgs(Patient pat,ApptOther apptOther,List<ApptOther> listApptOthers) {
			Pat=pat;
			ApptOther=apptOther;
			ListApptOthers=listApptOthers;
		}
	}
}
