using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using OpenDental.UI;
using SparksToothChart;
using System.Drawing.Drawing2D;

namespace OpenDental {
	public partial class UserControlDashboardWidget:UserControl {
		private SheetDef _sheetDefWidget;
		private Sheet _sheetWidget;
		private Patient _pat;
		///<summary>The original location of your mouse when you first click.</summary>
		private Point _ptDragOffset;
		///<summary>Event is fired when the close button is clicked.</summary>
		public event WidgetClosedHandler WidgetClosed=null;
		///<summary>Event is fired when the widget is dropped in a new location.</summary>
		public event WidgetDroppedHandler WidgetDragDropped=null;


		public SheetDef SheetDefWidget {
			get {
				return _sheetDefWidget;
			}
			set {
				_sheetDefWidget=value;
				Invalidate();
			}
		}

		public UserControlDashboardWidget(SheetDef sheetDefWidget,string name,int width,int height) {
			InitializeComponent();
			_sheetDefWidget=sheetDefWidget;
			Width=width;
			Height=height;
			if(Width==-1) {
				Width=sheetDefWidget.Width;//Use defaults if not specified.
			}
			if(Height==-1) {
				Height=sheetDefWidget.Height;//Use defaults if not specified.
			}
			Name=name;
			ContextMenuStrip=contextMenu;
			AllowDrop=true;
		}

		public bool Initialize() {
			if(!TryRefreshData()) {
				return false;
			}
			if(IsHandleCreated) {
				RefreshView();
			}
			return true;
		}

		public bool TryRefreshData() {
			if(_sheetDefWidget==null) {
				return false;
			}
			RefreshPatient();
			RefreshDefAndFields();
			foreach(Control ctr in this.GetAllControls()) {
				SheetField sheetField=_sheetWidget.SheetFields.FirstOrDefault(x => GetSheetFieldID(x)==ctr.Name);
				if(ctr is IDashWidgetField) {
					((IDashWidgetField)ctr).RefreshData(_pat,sheetField);
				}
			}
			return true;
		}

		private void RefreshPatient() {
			_pat=Patients.GetPat(FormOpenDental.CurPatNum);
		}

		private void RefreshDefAndFields() {
			_sheetDefWidget=SheetDefs.GetSheetDef(_sheetDefWidget.SheetDefNum);
			SheetDefs.GetFieldsAndParameters(_sheetDefWidget);
			_sheetWidget=SheetUtil.CreateSheet(_sheetDefWidget,_pat?.PatNum??0);
			SheetFiller.FillFields(_sheetWidget);
		}

		public void RefreshView() {
			this.InvokeIfRequired(() => {
				RefreshDimensions();
				foreach(SheetField sheetField in _sheetWidget.SheetFields) {
					RefreshSheetField(sheetField);
				}
				CleanupDisplay();
			});
		}

		private void RefreshDimensions() {
			if(IsWidgetClosed()) {
				//Since we BeginInvoke back to the MainThread, it's possible to have queued up this method, and then Closed the Widget,which sets 
				//_sheetDefWidget=null, before this method executes.
				return;
			}
			Width=_sheetDefWidget.Width;
			Height=_sheetDefWidget.Height;
		}

		///<summary>Handles how each type of SheetField should be drawn to the control.</summary>
		private void RefreshSheetField(SheetField field) {
			if(IsWidgetClosed()) {
				//Since we BeginInvoke back to the MainThread, it's possible to have queued up this method, and then Closed the Widget,which sets 
				//_sheetDefWidget=null, before this method executes.
				return;
			}
			Type type=GetControlTypeForDisplay(field);
			Control ctr=this.GetAllControls().FirstOrDefault(x => x.Name==GetSheetFieldID(field) && x.GetType()==type);
			if(ctr==null) {
				ctr=CreateControl(field,type);
			}
			ctr.Location=new Point(field.XPos,field.YPos);
			ctr.Text=field.FieldValue;
			ctr.ForeColor=field.ItemColor;
			ctr.Size=new Size(field.Width,field.Height);
			if(ctr is IDashWidgetField) {
				((IDashWidgetField)ctr).RefreshView();
			}
			ctr.Visible=true;
		}

		private void HookUpDragEvents(Control ctr) {
			ctr.AllowDrop=true;
			ctr.MouseDown+=OnChildMouseDown;
			ctr.DragOver+=OnChildDragOver;
			ctr.DragDrop+=OnChildDragDrop;
		}

		///<summary>Returns a unique ID that can be used in the _dictDocPatPicture and _dicPatPicture.</summary>
		private string GetPatImageKey(Patient pat,SheetField sheetField) {
			if(pat==null || sheetField==null) {
				return "-1";
			}
			return POut.Long(_pat.PatNum)+_pat.GetNameLF()+sheetField.FieldValue;
		}

		///<summary>Determines which Type of Control will be used in the UserControlDashboardWidget for various SheetField.FieldTypes</summary>
		private Type GetControlTypeForDisplay(SheetField field) {
			Type type=typeof(Control);
			switch(field.FieldType) {
				case SheetFieldType.StaticText:
					type=typeof(DashLabel);
					break;
				case SheetFieldType.PatImage:
					type=typeof(DashPatPicture);
					break;
				case SheetFieldType.Grid:
					type=typeof(DashApptGrid);
					break;
				case SheetFieldType.Special:
					if(field.FieldName=="toothChart") {
						type=typeof(DashToothChart);
					}
					else if(field.FieldName=="toothChartLegend") {
						type=typeof(DashToothChartLegend);
					}
					else if(field.FieldName=="individualInsurance") {
						type=typeof(DashIndividualInsurance);
					}
					else if(field.FieldName=="familyInsurance") {
						type=typeof(DashFamilyInsurance);
					}
					break;
				default:
					throw new NotImplementedException("FieldType: "+field.FieldType.GetDescription()+" has not been implemented for Dashboards.");
			}
			return type;
		}

		///<summary>Removes any controls that are no longer included in the SheetDef, i.e. SheetFieldDef was removed in between refreshes.</summary>
		private void CleanupDisplay() {
			if(IsWidgetClosed()) {
				return;
			}
			for(int i=Controls.Count-1;i>=0;i--) {
				Control ctr=Controls[i];
				if(!ctr.Name.In(_sheetWidget.SheetFields.Select(x => GetSheetFieldID(x)))) {
					CloseControl(ctr);//Old sheetfield/def that was removed from the SheetDef in between refreshes.
					continue;
				}
			}
		}

		private void CloseControl(Control ctr) {
			Controls.Remove(ctr);
			if((ctr as PictureBox)!=null) {
				((PictureBox)ctr).Image?.Dispose();
			}
			if((ctr as ODPictureBox)!=null) {
				((ODPictureBox)ctr).Image?.Dispose();
			}
			ctr.Dispose();
		}

		///<summary>Gets a unique id for the field, since the field has not been inserted into the database and therefore does not have a PK.</summary>
		private string GetSheetFieldID(SheetField field) {
			//Since field has not been inserted into the db, field.SheetFieldNum=0.  It's possible to have two fields with the same FieldName.
			//Use NameTypeXPosYPos as a likely unique id.
			return (field.FieldName+field.FieldType.GetDescription()+field.XPos+field.YPos+field.Width+field.Height);
		}

		///<summary>Creates a new Control of Type type, setting its Name as a unique identifier corresponding to properties in field.
		///Hooks up appropriate events so that drag/drop on the control causes the UserControlDashboardWidget to be dragged/dropped.</summary>
		private Control CreateControl(SheetField field, Type type) {
			Control ctr=(Control)Activator.CreateInstance(type);
			//Since field has not been inserted into the db, field.SheetFieldNum=0.  It's possible to have two fields with the same FieldName.
			//Use NameXPosYPos as a likely unique id.
			ctr.Name=GetSheetFieldID(field);
			Lan.C(this,new Control[] { ctr });
			Controls.Add(ctr);
			HookUpDragEvents(ctr);
			if(ctr is IDashWidgetField) {
				((IDashWidgetField)ctr).RefreshData(_pat,field);
			}
			return ctr;
		}
		
		private void closeToolStripMenuItem_Click(object sender,EventArgs e) {
			CloseWidget();
		}

		private bool IsWidgetClosed() {
			if(_sheetDefWidget==null && _sheetWidget==null) {
				return true;
			}
			return false;
		}

		public void CloseWidget() {
			foreach(Control ctr in this.GetAllControls()) {
				CloseControl(ctr);
			}
			WidgetClosed?.Invoke(this,new EventArgs());
			_sheetDefWidget=null;
			_sheetWidget=null;
		}

		#region DragAndDrop
		protected override void OnMouseDown(MouseEventArgs e) {
			BringToFront();
			if(e.Button==MouseButtons.Right) {
				return;
			}
			_ptDragOffset=new Point(e.X,e.Y);
			DoDragDrop(this,DragDropEffects.Move);
		}

		private void OnChildMouseDown(object sender,MouseEventArgs e) {
			if(e.Clicks>1) {
				return;
			}
			//Convert the click location to a position relative to the UserControlDashboardWidget.
			int x=e.X+((Control)sender).Location.X;
			int y=e.Y+((Control)sender).Location.Y;
			OnMouseDown(new MouseEventArgs(e.Button,e.Clicks,x,y,e.Delta));
		}

		protected override void OnDragOver(DragEventArgs drgevent) {
			UserControlDashboardWidget widget=drgevent.Data.GetData(typeof(UserControlDashboardWidget)) as UserControlDashboardWidget;
			drgevent.Effect=DragDropEffects.Move;
			widget.DragWidget(new Point(drgevent.X,drgevent.Y));
		}

		private void OnChildDragOver(object sender,DragEventArgs drgevent) {
			OnDragOver(drgevent);
		}

		protected override void OnDragDrop(DragEventArgs drgevent) {
			WidgetDragDropped?.Invoke(this,new EventArgs());
		}

		private void OnChildDragDrop(object sender,DragEventArgs drgevent) {
			OnDragDrop(drgevent);
		}

		///<summary>Logic to move widget around the dashboard, accounting for mouse movement and location of original click when drag began.
		///ptMouse should be in screen coordinates.</summary>
		public void DragWidget(Point ptMouse) {
			Point ptClient=PointToClient(ptMouse);
			int x=(Location.X-_ptDragOffset.X)+ptClient.X;
			int y=(Location.Y-_ptDragOffset.Y)+ptClient.Y;
			Location=new Point(x,y);
			Invalidate();
		}
		#endregion

		public delegate void WidgetClosedHandler(UserControlDashboardWidget sender,EventArgs e);
		public delegate void WidgetDroppedHandler(UserControlDashboardWidget sender,EventArgs e);
		public delegate void ApptEditHandler(List<ApptOther> listApptOther);
	}
}
