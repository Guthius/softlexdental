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

namespace OpenDental {
	public partial class UserControlDashboard:UserControl {
		private ODThread _threadRefresh;
		private bool _isLoggingOff=false;
		private SheetDef _sheetDefDashboard;
		private Action _actionDashboardContentsChanged;
		private Action _actionDashboardClosing;
		private Point _ptClick;

		public const int DefaultWidth=400;
		public const int DefaultHeight=800;

		public List<UserControlDashboardWidget> ListOpenWidgets {
			get {
				return Controls.OfType<UserControlDashboardWidget>().ToList();
			}
		}

		public bool IsInitialized {
			get;
			private set;
		}

		public UserControlDashboard() {
			InitializeComponent();
			ContextMenuStrip=contextMenu;
			AllowDrop=true;
		}

		/// <summary>Initializes a Dashboard.</summary>
		/// <param name="sheetDefDashboard">SheetDef that describes the layout of the Dashboard.</param>
		/// <param name="actionOnDashboardContentsChanged">Action is fired when the contents of the Dashboard change.</param>
		/// <param name="actionOnDashboardClosing">Action is fired when the Dashboard is closing.</param>
		/// <param name="actionOnApptPinClicked">Action is fired when a user double clicks on an ODApptGrid in a widget, then clicks 'Send To Pinboard' 
		/// in the Appointment Edit window. </param>
		public void Initialize(SheetDef sheetDefDashboard,Action actionOnDashboardContentsChanged,Action actionOnDashboardClosing) {
			if(sheetDefDashboard==null) {
				//Maybe just use a messagebox...
				throw new ApplicationException("User Dashboard does not exist");
			}
			_sheetDefDashboard=sheetDefDashboard;
			_actionDashboardContentsChanged=actionOnDashboardContentsChanged;
			_actionDashboardClosing=actionOnDashboardClosing;
			SheetDefs.GetFieldsAndParameters(_sheetDefDashboard);
			if(_sheetDefDashboard.SheetFieldDefs.Count>0) {//Only resize on init if there is an attached widget.
				this.InvokeIfRequired(() =>
				{
					Width=sheetDefDashboard.Width;
					Height=sheetDefDashboard.Height;
				});
			}
			foreach(SheetFieldDef sheetFieldDef in _sheetDefDashboard.SheetFieldDefs) {//Load Widgets from a saved Dashboard.
				SheetDef sheetDefWidget=SheetDefs.GetFirstOrDefault(x => x.SheetDefNum==PIn.Long(sheetFieldDef.FieldValue));
				if(sheetDefWidget!=null) {
					string name=POut.Long(sheetFieldDef.SheetFieldDefNum);//Necessary to differentiate between multiple instances of the same Widget.
					AddWidget(sheetDefWidget,name,new Point(sheetFieldDef.XPos,sheetFieldDef.YPos),sheetFieldDef.Width,sheetFieldDef.Height);
				}
			}
			//Failed to open any of the previously attached widgets, likely due to user no longer having permissions to widgets.
			//Continue if initializing Dashboard for the first time so that a widget can be added later.
			if(_sheetDefDashboard.SheetFieldDefs.Count>0 && ListOpenWidgets.Count==0) {
				CloseDashboard(false);
				return;
			}
			PatientEvent.Fired+=PatientEvent_Fired;
			PatientChangedEvent.Fired+=PatientChangedEvent_Fired;
			IsInitialized=true;
			_actionDashboardContentsChanged?.Invoke();//Only after all the widgets are added.
			StartRefreshThread();
		}

		private void PatientEvent_Fired(ODEventArgs e) {
			if(((e.Tag as Patient)?.PatNum??-1)==FormOpenDental.CurPatNum) {
				RefreshDashboard();
			}
		}

		private void PatientChangedEvent_Fired(ODEventArgs e) {
			RefreshDashboard();
		}

		///<summary>Causes the refresh thread to either start, or wakeup and refresh the data in the background and refresh the UI on the main thread.
		///</summary>
		public void RefreshDashboard() {
			if(_threadRefresh==null || _threadRefresh.HasQuit) {
				StartRefreshThread();
				return;
			}
			_threadRefresh.Wakeup();
		}

		private void StartRefreshThread() {
			if(_threadRefresh!=null && !_threadRefresh.HasQuit) {
				return;
			}
			if(_sheetDefDashboard==null) {
				return;
			}
			//20 seconds chosen arbitrarily.  In FormOpenDental, anytime the patient changes we will trigger this thread.  We could trigger this thread on 
			//other events as well, such as certain signals.  That approach, while more responsive, does run the risk of excessive chattiness.  20 second 
			//refresh intervals may be a happy balance.
			_threadRefresh=new ODThread((int)TimeSpan.FromSeconds(20).TotalMilliseconds,(o) => {
				foreach(UserControlDashboardWidget widget in ListOpenWidgets) {
					if(widget.TryRefreshData()) {
						if(widget.IsDisposed || !widget.IsHandleCreated) {
							continue;
						}
						if(!o.HasQuit) {
							widget.RefreshView();//Invokes back to UI thread for UI update.
						}
					}
				}
#if DEBUG
				Console.WriteLine("Dashboard refreshed.");
#endif
			});
			_threadRefresh.Name="DashboardRefresh_"+_sheetDefDashboard.Description+"_Thread";
			_threadRefresh.Start();
		}

		///<summary>Updates the SheetDef that describes the Dashboard layout if the dimensions have changed.</summary>
		public void UpdateDimensions(int width,int height) {
			if(_sheetDefDashboard.Width==width && _sheetDefDashboard.Height==height) {
				return;
			}
			this.InvokeIfRequired(() => {
				Width=width;
				Height=height;
			});
			_sheetDefDashboard.Width=width;
			_sheetDefDashboard.Height=height;
			SheetDefs.Update(_sheetDefDashboard);
		}

		///<summary>Creates a new UserControlDashboardWidget control and adds it to the Dashboard.</summary>
		private UserControlDashboardWidget AddWidget(SheetDef sheetDefWidget,string name,Point ptLoc,int width=-1,int height=-1) {
			if(sheetDefWidget==null || !Security.IsAuthorized(Permissions.DashboardWidget,sheetDefWidget.SheetDefNum,true)) {
				return null;
			}
			UserControlDashboardWidget widget=null;
			this.InvokeIfRequired(() => { 
				//Trying to open a widget that is already open.
				if(ListOpenWidgets.Any(x => x.SheetDefWidget.SheetDefNum==sheetDefWidget.SheetDefNum)) {
					return;
				}
				widget=new UserControlDashboardWidget(sheetDefWidget,name,width,height);
				widget.WidgetClosed+=CloseWidget;
				widget.WidgetDragDropped+=DragDropWidget;
				widget.Anchor=((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left));
				if(!widget.IsHandleCreated) {
					IntPtr handle=widget.Handle;//Ensure the handle for this object is created on the Main thread.
				}
			});
			if(widget!=null && widget.Initialize()) {
				this.InvokeIfRequired(() => { 
						Controls.Add(widget);
						widget.Location=GetValidLocation(widget,ptLoc);
						widget.BringToFront();
						if(widget.Width>Width) {//Widget is wider than Dashboard.
							UpdateDimensions(widget.Width,Height);//Update width only.
						}
				});
			}
			else {
				widget=null;
			}
			return widget;
		}

		///<summary>Adds a new Widget to the current Dashboard.</summary>
		public bool AddWidget(SheetDef sheetDefWidget,Point ptLoc) {
			//Newly added to this Dashboard.  Name will be set in AddWidgetSheetFieldDef(), since we don't have SheetFieldDef.SheetFieldDefNum until after 
			//the Insert()
			UserControlDashboardWidget widget=AddWidget(sheetDefWidget,"",ptLoc,-1,-1);
			if(widget==null) {//Failed to open the widget, either due to lack of permission or failure to get the widget's SheetDef.
				return false;
			}
			AddWidgetSheetFieldDef(widget);
			if(ListOpenWidgets.Count==1) {//When this is the first opened widget, dynamically size the Dashboard to fit.
				UpdateDimensions(widget.Width,Height);//Update width only.
			}
			_actionDashboardContentsChanged?.Invoke();
			return true;
		}

		///<summary>Adds a SheetFieldDef to the database that is linked to the SheetDef that describes the current user's Dashboard.  The SheetFieldDef 
		///is further linked to a SheetDef that defines a Widget.</summary>
		private void AddWidgetSheetFieldDef(UserControlDashboardWidget widget) {
			SheetFieldDef sheetFieldDefWidget=new SheetFieldDef() {
				SheetDefNum=_sheetDefDashboard.SheetDefNum,//Dashboard
				FieldType=SheetFieldType.Special,//Indicates this is a Widget
				FieldValue=widget.SheetDefWidget.SheetDefNum.ToString(),//FK to Widget (SheetDef)
				FieldName=widget.Name,
				XPos=widget.Location.X,
				YPos=widget.Location.Y,
				Width=widget.Width,
				Height=widget.Height,
			};
			widget.Name=POut.Long(SheetFieldDefs.Insert(sheetFieldDefWidget));
			_sheetDefDashboard.SheetFieldDefs.Add(sheetFieldDefWidget);
			DataValid.SetInvalid(InvalidType.Sheets);
		}

		///<summary>Updates an existing SheetFieldDef that links a SheetDef Dashboard to a SheetDef Widget.</summary>
		private void UpdateWidgetSheetFieldDef(UserControlDashboardWidget widget) {
			SheetFieldDef sheetFieldDef=GetSheetFieldDef(widget);
			if(sheetFieldDef==null) {
				throw new ApplicationException("Widget not found");
			}
			//Editing an existing widget
			sheetFieldDef.XPos=widget.Location.X;
			sheetFieldDef.YPos=widget.Location.Y;
			sheetFieldDef.Width=widget.Width;
			sheetFieldDef.Height=widget.Height;
			SheetFieldDefs.Update(sheetFieldDef);
		}

		///<summary>Removes an existing SheetFieldDef that links a SheetDef Dashboard to a SheetDef Widget.  The effect is the widget is removed from 
		///the Dashboard.</summary>
		private void RemoveWidgetSheetFieldDef(UserControlDashboardWidget widget) {
			SheetFieldDef sheetFieldDef=GetSheetFieldDef(widget);
			if(sheetFieldDef==null) {
				throw new ApplicationException("Widget not found");
			}
			_sheetDefDashboard.SheetFieldDefs.Remove(sheetFieldDef);//Remove from in memory list.
			SheetFieldDefs.Delete(sheetFieldDef.SheetFieldDefNum);//Remove from db.
			DataValid.SetInvalid(InvalidType.Sheets);
		}

		private SheetFieldDef GetSheetFieldDef(UserControlDashboardWidget widget) {
			return _sheetDefDashboard.SheetFieldDefs.FirstOrDefault(x => x.SheetFieldDefNum==PIn.Long(widget.Name));
		}			

		private Point GetValidLocation(UserControlDashboardWidget widget,Point ptLoc) {
			Point location=ptLoc;
			if(location==null || !IsPartiallyContained(new Rectangle(0,0,Width,Height),new Rectangle(location.X,location.Y,widget.Width,widget.Height))) {
				location=new Point(0,0);
			}
			return location;
		}

		private bool IsPartiallyContained(Rectangle dashboard,Rectangle widget) {
			Point upperLeftWidget=widget.Location;//UpperLeft Widget
			Point lowerRightWidget=new Point(widget.Location.X+widget.Width,widget.Location.Y+widget.Height);//LowerRight Widget
			if(lowerRightWidget.X<0 || lowerRightWidget.Y<0) {
				return false;
			}
			if(upperLeftWidget.X>dashboard.Width || upperLeftWidget.Y>dashboard.Height) {
				return false;
			}
			return true; 
		} 

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			_ptClick=e.Location;
		}

		#region DragAndDrop
		protected override void OnDragOver(DragEventArgs drgevent) {
			//Without this code, dragging fast and moving the mouse off the dashboard widget would not have brought the widget with it.
			UserControlDashboardWidget widget=drgevent.Data.GetData(typeof(UserControlDashboardWidget)) as UserControlDashboardWidget;
			if(widget!=null) {
				drgevent.Effect=DragDropEffects.Move;
				widget.DragWidget(new Point(drgevent.X,drgevent.Y));
			}
		}

		private void DragDropWidget(UserControlDashboardWidget widget,EventArgs e) {
			if(widget==null) {
				return;
			}
			UpdateWidgetSheetFieldDef(widget);
		}
		#endregion

		private void menuItemAdd_Click(object sender,EventArgs e) {
			FormDashboardWidgets FormD=new FormDashboardWidgets();
			if(FormD.ShowDialog()==DialogResult.OK) {
				AddWidget(FormD.SheetDefDashboardWidget,_ptClick);
			}
		}

		protected override void OnPaint(PaintEventArgs e) {
			labelExit.BringToFront();
			base.OnPaint(e);
		}

		private void CloseWidget(UserControlDashboardWidget widget,EventArgs e) {
			if(widget==null) {
				return;
			}
			Controls.Remove(widget);
			if(!_isLoggingOff) {
				RemoveWidgetSheetFieldDef(widget);
			}
			if(ListOpenWidgets.Count==0) {
				IsInitialized=false;
				_actionDashboardContentsChanged?.Invoke();
				if(!_isLoggingOff) {
					_actionDashboardClosing?.Invoke();
				}
				_threadRefresh?.QuitSync(100);
			}
		}

		private void labelExit_Click(object sender,EventArgs e) {
			CloseDashboard(false);
		}

		public void CloseDashboard(bool isLoggingOff) {
			if(InvokeRequired) {
				this.Invoke(() => { CloseDashboard(isLoggingOff); });
				return;
			}
			_isLoggingOff=isLoggingOff;
			if(ListOpenWidgets.Count==0) { //In case a dashboard manages to be left open with no widgets.
				IsInitialized=false;
				_actionDashboardContentsChanged?.Invoke();
				_actionDashboardClosing?.Invoke();
				_threadRefresh?.QuitSync(100);
			}
			for(int i=ListOpenWidgets.Count-1;i>=0;i--) {
				ListOpenWidgets[i].CloseWidget();
			}
			_isLoggingOff=false;
			//Clear these actions set during UserA's log on.  Otherwise, if UserB logs on and doesn't have a saved Dashboard, when UserB logs off, the actions defined for UserA (specifically, removing UserOdPrefs and Dashboard SheetDefs) will occur.
			_actionDashboardContentsChanged=null;
			_actionDashboardClosing=null;
		}
	}
}
