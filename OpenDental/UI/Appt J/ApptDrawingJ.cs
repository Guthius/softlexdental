using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Globalization;
using System.Text;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental.UI {
	/// <summary>Encapsulates both Data and Drawing for the main area of Appt module and probably the operatories header.  The Appt module gathers the data and stores it here.  The Appt module creates an instance of this type, and uses it for data storage and for drawing.  Anything that wants to pass in data will need to have access to that object.  This class does its drawing based solely on internal information.  It is not be capable of retrieving any information of any kind from anywhere.  It has defaults so that we will always be able to draw something reasonable, even if we're missing data.  This class has methods that perform computations ahead of time for layout drawing.  This class contains extra data that it doesn't actually need.  Appt module uses this data for other things besides drawing appointments.  This class supports a single thread only.  Once proven to work well, this single class will replace ApptDrawing and ContrApptSheet (and ApptOverlapOrdering?).  The suffix J is for Jordan.  It can later be changed after all similarly named classes are removed.</summary>
	public class ApptDrawingJ {
		#region Private isValid flags
		///<summary>This will trigger redrawing everything in the main area.  Needed if main background changes, because appt sizes and provbars will also need update.  This does not include a redraw of the ProvOp bar, which must be set invalid separately.</summary>
		private bool _isValidAllMain=false;//start false to trigger initial draw
		///<summary>This will trigger redrawing all appointments, including outlines and appt provbars.</summary>
		private bool _isValidAppts=true;
		///<summary>This will trigger redrawing only the outlines around appointments.</summary>
		private bool _isValidOutlines=true;
		///<summary>This will trigger redrawing only the red time line.</summary>
		private bool _isValidTimeRed=true;
		///<summary>This will trigger redrawing only the prov ops header.  This happens less frequently than main area.  This does not depend at all on schedules or appointments, just providers and ops, which are set by views.  This depends on ApptView.OnlyScheduledProvs affecting visible Ops.  So it's visible Ops that then sets this flag.</summary>
		private bool _isValidProvOp=false;
		#endregion Private isValid flags

		#region Private 
		//<summary>This list was originally the Controls attached to ContrApptSheet.</summary>
		//private List<ContrApptSingleJ> _listContrApptSingleJ;
		///<summary>Always calculated internally.</summary>
		private int _height=800;
		///<summary>Stores the shading info for the provider bars on the left of the appointments module.  First dim is number of visible providers.  Second dim is cells.</summary>
		private int[][] _provBar=new int[0][];
		///<summary>Width of each operatory.</summary>
		private float _colWidth=200f;
		///<summary>Width of timebars.</summary>
		private float _timeWidth=37f;
		///<summary>Width of provider bars.</summary>
		private float _provWidth=8f;
		///<summary>Line height.  This is currently treated like a constant that the user has no control over.  Also in ApptSingleDrawingJ.</summary>
		private float _lineH=12f;
		///<summary>The number of columns.  Stays consistent even if weekly view.  The number of colums showing for one day.  Derived from VisOps</summary>
		private int _colCount=3;
		///<summary>Usually VisProvs.Count, but can be used to override to 0 for weekview.</summary>
		private int _provCount=0;
		///<summary>Typical values would be 10,15,5,or 7.5.</summary>
		private float _minPerRow=10f;
		///<summary>Rows per hour, based on RowsPerIncr and MinPerIncr</summary>
		private int _rowsPerHr=6;
		///<summary>Typically 5 or 7. Only used with weekview.</summary>
		private int _numOfWeekDaysToDisplay=7;
		///<summary>The width of an entire day if using week view.</summary>
		private float _weekDayWidth=100f;
		///<summary>Only used with weekview. The width of individual appointments within each day.</summary>
		private float _weekApptWidth=33f;
		///<summary>ADA colsPerpage. Sets number of columns if printing.  Otherwise, use 0.  Was originally a parameter on ComputeColWidth.  Only affects daily view.</summary>
		private int _printingColCountOverride=0;
		///<summary>Only used for printing, but could be expanded.  Was originally passed in as part of drawing command. 0 means not in use.</summary>
		private TimeSpan _timeStart=TimeSpan.Zero;
		///<summary>Only used for printing, but could be expanded.  Was originally passed in as part of drawing command. 0 means not in use.</summary>
		private TimeSpan _timeStop=TimeSpan.Zero;	
		///<summary>This was being passed in.  Not sure where it's calculated.</summary>
		//private float _fontSize=9;
		///<summary>This was being passed in.  Not sure where it's set.</summary>
		private bool _isPrinting=false;
		///<summary>This was being passed in when printing as pageColumn.  Otherwise zero.</summary>
		private int _printingPageColumn;
		///<summary>Constant 17.</summary>
		private int _heightProvOpHeader=17;
		#endregion Private 

		#region Private Drawing Objects
		///<summary>Just the background.  Must place all the appts on it.</summary>
		private Bitmap _bitmapBack;
		///<summary>Background plus Appts.  No outlines or time line.</summary>
		private Bitmap _bitmapBackAppts;
		///<summary>Background plus Appts plus outlines.  No time line.</summary>
		private Bitmap _bitmapBackApptsOutlines;
		///<summary>Easily assembled by combining one of the other bitmaps with a red time line.</summary>
		private Bitmap _bitmapMain;
		///<summary>The header that shows provider and operatory colors and info. Only redrawn if _isValidAll is true</summary>
		private Bitmap _bitmapProvOp;
		private Graphics _graphicsBack;
		private Graphics _graphicsBackAppts;
		private Graphics _graphicsBackApptsOutlines;
		private Graphics _graphicsMain;
		private Graphics _graphicsProvOp;
		//Brushes and pens are rarely changed, so disposing is overkill
		//Some colors such as prov and blockout are handled locally, since they change frequently
		private Brush _brushBackground=SystemBrushes.Control;
		private Brush _brushTimeBar=Brushes.LightGray;
		private Brush _brushLinesAndText=Brushes.Black;
		private Brush _brushOpen=Brushes.White;
		private Brush _brushClosed=new SolidBrush(Color.FromArgb(219,219,219));
		private Brush _brushHoliday=new SolidBrush(Color.FromArgb(255,128,128));
		private Brush _brushBlockText=Brushes.Black;
		private Brush _brushTextBlack=Brushes.Black;
		private Pen _penVertPrimary=Pens.DarkGray;
		private Pen _penHorizDark=Pens.DarkSlateGray;
		private Pen _penVertWhite=Pens.White;
		private Pen _penHorizBlack=Pens.Black;
		private Pen _penHorizMinutes=Pens.LightGray;
		private Font _fontBlockout=new Font("Arial",9f);//_fontSize);
		private Font _fontDefault=new Font("Arial",8.25f);
		private Pen _penTimeLine=Pens.Red;
		#endregion Private Drawing Objects

		#region Properties for Complex Data
		///<summary>The appointment table. Refreshed in RefreshAppointmentsIfNeeded.</summary>
		public DataTable TableAppointments {
			get{ return _tableAppointments; }
			set{
				_tableAppointments=value;
				_isValidAppts=false;
			}
		}
		private DataTable _tableAppointments=new DataTable();

		///<summary>The employee schedule table. Refreshed in RefreshSchedulesIfNeeded.</summary>
		public DataTable TableEmpSched {
			get{ return _tableEmpSched; }
			set{
				_tableEmpSched=value;
				_isValidAllMain=false;
			}
		}
		private DataTable _tableEmpSched=new DataTable();

		///<summary>The provider schedule table. Refreshed in RefreshSchedulesIfNeeded.</summary>
		public DataTable TableProvSched {
			get{ return _tableProvSched; }
			set{
				_tableProvSched=value;
				_isValidAllMain=false;
			}
		}
		private DataTable _tableProvSched=new DataTable();

		///<summary>The schedule table. Refreshed in RefreshSchedulesIfNeeded.</summary>
		public DataTable TableSchedule {
			get{ return _tableSchedule; }
			set{
				_tableSchedule=value;
				_isValidAllMain=false;
			}
		}
		private DataTable _tableSchedule=new DataTable();

		///<summary>The waiting room table. Refreshed in RefreshWaitingRoomTable.</summary>
		public DataTable TableWaitingRoom {
			get{ return _tableWaitingRoom; }
			set{
				_tableWaitingRoom=value;
				//_isValid=false; ?
			}
		}
		private DataTable _tableWaitingRoom=new DataTable();

		///<summary>The appointment fields table. Refreshed in RefreshAppointmentsIfNeeded.</summary>
		public DataTable TableApptFields {
			get{ return _tableApptFields; }
			set{
				_tableApptFields=value;
				_isValidAppts=false;
			}
		}
		private DataTable _tableApptFields=new DataTable();

		///<summary>The patient fields table. Refreshed in RefreshAppointmentsIfNeeded.</summary>
		public DataTable TablePatFields {
			get{ return _tablePatFields; }
			set{
				_tablePatFields=value;
				_isValidAppts=false;
			}
		}
		private DataTable _tablePatFields=new DataTable();
		
		///<summary>This gets set externally each time the module is selected.  It is the background schedule for the entire period.  Includes all types.</summary>
		public List<Schedule> SchedListPeriod {
			get{ return _schedListPeriod; }
			set{
				_schedListPeriod=value;
				_isValidAppts=false;
			}
		}
		private List<Schedule> _schedListPeriod=new List<Schedule>();

		///<summary>Visible provider bars in appt module.  Subset of all provs.  Can't include a hidden prov in this list.</summary>
		public List<Provider> VisProvs {
			get{ return _visProvs; }
			set{
				_visProvs=value;
				_provBar=new int[VisProvs.Count][];
				for(int i=0;i<VisProvs.Count;i++) {
					_provBar[i]=new int[24*_rowsPerHr];//RowsPerHr=60/MinPerIncr*RowsPerIncr.
				}
				_isValidAppts=false;
				_isValidProvOp=false;
			}
		}	
		private List<Provider> _visProvs=new List<Provider>();

		///<summary>Visible ops in appt module.  Subset of all ops.  Can't include a hidden op in this list.  If user has set View.OnlyScheduledProvs, and not isWeekly, then the only VisOps will be for providers that have schedules for the day and ops with no provs assigned.</summary>
		public List<Operatory> VisOps {
			get{ return _visOps; }
			set{
				_visOps=value;
				_isValidAppts=false;
				_isValidProvOp=false;
			}
		}	
		private List<Operatory> _visOps=new List<Operatory>();

		///<summary>Previously, we looped through VisOps in order to find the 0-based column index for a given OpNum. This was too slow so we now use this helper dictionary to do the same lookup.</summary>
		public Dictionary<long,int> DictOpNumToColumnNum{
			get{ return _dictOpNumToColumnNum; }
			set{
				_dictOpNumToColumnNum=value;
			}
		}
		private Dictionary<long,int> _dictOpNumToColumnNum=new Dictionary<long, int>();

		///<summary>Previously, we looped through VisProvs in order to find the 0-based column index for a given ProvNum. This was too slow so we now use this helper dictionary to do the same lookup.</summary>
		public Dictionary<long,int> DictProvNumToColumnNum{
			get{ return _dictProvNumToColumnNum; }
			set{
				_dictProvNumToColumnNum=value;
			}
		}
		private Dictionary<long,int> _dictProvNumToColumnNum=new Dictionary<long, int>();
#endregion Properties for Complex Data

		#region Properties for Simple Data
		///<summary>Always set from outside.</summary>
		public int Width{
			get{
				return _width;
			}
			set{
				if(value==_width){
					return;
				}
				_width=value;
				_isValidAllMain=false;//if width changes, we need to redraw everything.
				_isValidProvOp=false;
			}
		}
		private int _width=800;

		///<summary>WeekStartDate and WeekEndDate were also in ContrAppt, but aren't needed.</summary>
		public bool IsWeeklyView{
			get{
				return _isWeeklyView;
			}
			set{
				if(value==_isWeeklyView){
					return;
				}
				_isWeeklyView=value;
				//the logic below was formerly in ContrAppt.SetWeekDates
				if(_isWeeklyView){//if changing to weekly view
					if(_dateSelected.DayOfWeek==DayOfWeek.Sunday) {
						_dateStart=_dateSelected.AddDays(-6).Date;//go back to previous monday
					}
					else {
						_dateStart=_dateSelected.AddDays(1-(int)_dateSelected.DayOfWeek).Date;//go back to current monday
					}
					_dateEnd=_dateStart.AddDays(_numOfWeekDaysToDisplay-1).Date;
				}
				else {//changing to daily view
					_dateStart=_dateSelected;
					_dateEnd=_dateSelected;
				}
				_isValidAllMain=false;
				_isValidProvOp=false;
			}
		}
		private bool _isWeeklyView=false;

		///<summary>The date currently selected in the appointment module.  If switching to week view, this date does not change.  That way, it is remembered when switching back to day view.</summary>
		public DateTime DateSelected{
			get{
				return _dateSelected;
			}
			set{
				if(value==_dateSelected){
					return;
				}
				_dateSelected=value;
				if(_isWeeklyView){
					//user can click on a new date, which selects a new week.
					//There is a rare case where, in week view, you can select a different date within the existing week.
					//We don't technically need to set _isValidAll to false, but it's rare and harmless.
					if(_dateSelected.DayOfWeek==DayOfWeek.Sunday) {
						_dateStart=_dateSelected.AddDays(-6).Date;//go back to previous monday
					}
					else {
						_dateStart=_dateSelected.AddDays(1-(int)_dateSelected.DayOfWeek).Date;//go back to current monday
					}
					_dateEnd=_dateStart.AddDays(_numOfWeekDaysToDisplay-1).Date;
				}
				else {
					_dateStart=_dateSelected;
					_dateEnd=_dateSelected;
				}
				_isValidAllMain=false;
				_isValidProvOp=false;
			}
		}
		private DateTime _dateSelected;

		///<summary>Computed from DateSelected and IsWeeklyView. If weekly view, this is the first date.  If daily view, this is the same as dateSelected.</summary>
		public DateTime DateStart{
			get{
				return _dateStart;
			}
		}
		private DateTime _dateStart;

		///<summary>Computed from DateSelected and IsWeeklyView. If weekly view, this is the second date.  If daily view, this is the same as dateSelected.</summary>
		public DateTime DateEnd{
			get{
				return _dateEnd;
			}
		}
		private DateTime _dateEnd;

		///<summary>Based on the view.  If no view, then it is set to 1. Different computers can be showing different views.</summary>
		public int RowsPerIncr{
			get{
				return _rowsPerIncr;
			}
			set{
				_rowsPerIncr=value;
				_isValidAllMain=false;
			}
		}
		private int _rowsPerIncr=1;

		///<summary>Pulled from Prefs AppointmentTimeIncrement.  Either 5, 10, or 15. An increment can be one or more rows.</summary>
		public int MinPerIncr{
			get{
				return _minPerIncr;
			}
			set{
				_minPerIncr=value;
				_isValidAllMain=false;
			}
		}
		private int _minPerIncr=10;

		///<summary>Was in ContrApptSingle.</summary>
		public int SelectedAptNum{
			get{
				return _selectedAptNum;
			}
			set{
				_selectedAptNum=value;
				_isValidOutlines=false;
			}
		}
		private int _selectedAptNum=-1;
		#endregion Properties for Simple Data

    #region Constructor
		public ApptDrawingJ(){
			//=new List<ContrApptSingleJ>();
			//So dummy ops and provs will draw nicely, even if not initialized properly.
			VisOps.Add(new Operatory());
			VisOps.Add(new Operatory());
			VisOps.Add(new Operatory());
			VisProvs.Add(new Provider());
		}
    #endregion Constructor

#region Public Methods
		///<summary>Usually just grabs existing bitmap and draws a red timebar on it.  Has four fullsize bitmaps to choose from, depending on what data is invalid.  So it usually doesn't need to redraw much.</summary>
		public Bitmap GetBitmapMain(){
			//Disposing of each bitmap and graphics to free memory before reinitializing
			//On the first call to this method, they will be null, causing silent exception.
			if(!_isValidAllMain){
				ComputeColWidth();
				_height=(int)(_lineH*24*_rowsPerHr)+1;
				ODException.SwallowAnyException(() => {
					_bitmapBack.Dispose();
					_graphicsBack.Dispose();
				});
				_bitmapBack=new Bitmap(_width,_height);
				_graphicsBack=Graphics.FromImage(_bitmapBack);
				_graphicsBack.SmoothingMode=SmoothingMode.HighQuality;
				DrawBackground(_graphicsBack);
			}
			if(!_isValidAllMain || !_isValidAppts){
				ODException.SwallowAnyException(() => {
					_bitmapBackAppts.Dispose();
					_graphicsBackAppts.Dispose();
				});
				_bitmapBackAppts=new Bitmap(_width,_height);
				_graphicsBackAppts=Graphics.FromImage(_bitmapBackAppts);
				_graphicsBackAppts.DrawImage(_bitmapBack,0,0);
				DrawAppts(_graphicsBackAppts);
			}
			if(!_isValidAllMain || !_isValidAppts || !_isValidOutlines){
				ODException.SwallowAnyException(() => {
					_bitmapBackApptsOutlines.Dispose();
					_graphicsBackApptsOutlines.Dispose();
				});
				_bitmapBackApptsOutlines=new Bitmap(_width,_height);
				_graphicsBackApptsOutlines=Graphics.FromImage(_bitmapBackApptsOutlines);
				_graphicsBackApptsOutlines.DrawImage(_bitmapBackAppts,0,0);
				DrawOutlines(_graphicsBackApptsOutlines);
			}
			if(!_isValidAllMain || !_isValidAppts || !_isValidOutlines || !_isValidTimeRed){
				ODException.SwallowAnyException(() => {
					_bitmapMain.Dispose();
					_graphicsMain.Dispose();
				});
				_bitmapMain=new Bitmap(_width,_height);
				_graphicsMain=Graphics.FromImage(_bitmapMain);
				_graphicsMain.DrawImage(_bitmapBackApptsOutlines,0,0);
				DrawRedTimeLine(_graphicsMain);
			}
			_isValidAllMain=true;
			_isValidAppts=true;
			_isValidOutlines=true;
			_isValidTimeRed=true;
			return _bitmapMain;
		}

		///<summary>Usually just grabs existing bitmap.</summary>
		public Bitmap GetBitmapProvOp(){
			if(!_isValidProvOp){
				ComputeColWidth();//super fast.  If we are going to do more calculations, we might need to come up with something better
				ODException.SwallowAnyException(() => {
					_bitmapProvOp.Dispose();
					_graphicsProvOp.Dispose();
				});
				_bitmapProvOp=new Bitmap(_width,17);
				_graphicsProvOp=Graphics.FromImage(_bitmapProvOp);
				DrawProvOpHeader(_graphicsProvOp);
			}
			_isValidProvOp=true;
			return _bitmapProvOp;
		}

		///<summary>Called on startup and if color prefs change.</summary>
		public void SetColors(Color colorOpen,Color colorClosed,Color colorHoliday,Color colorBlockText,Color colorTimeLine){
			//Example pseudocode hints for calling this method:
			//List<Def> listDefs=Definition.GetByCategory(DefCat.AppointmentColors);
			//colorOpen=(listDefs[0].ItemColor);
			//colorClosed=(listDefs[1].ItemColor);
			//colorHoliday=(listDefs[3].ItemColor);
			//colorBlockText=Definition.GetByCategory(DefCat.AppointmentColors,true)[4].ItemColor;
			//colorTimeLine=PrefC.GetColor(PrefName.AppointmentTimeLineColor)
			_brushOpen=new SolidBrush(colorOpen);
			_brushClosed=new SolidBrush(colorClosed);
			_brushHoliday=new SolidBrush(colorHoliday);
			_brushBlockText=new SolidBrush(colorBlockText);
			_penTimeLine=new Pen(colorTimeLine);
			_isValidAllMain=false;//if we change colors, redraw everything
		}
#endregion Public Methods

#region DrawBackground
		///<summary></summary>
		private void DrawBackground(Graphics g) {
			g.FillRectangle(_brushBackground,0,0,_width,_height);
			g.FillRectangle(_brushTimeBar,0,0,_timeWidth,_height);//L time bar
			g.FillRectangle(_brushTimeBar,_timeWidth+_colWidth*_colCount+_provWidth*_provCount,0,_timeWidth,_height);//R time bar
			DrawMainBackground(g);
			DrawBlockouts(g);
			DrawWebSchedASAPSlots(g);
			if(!_isWeeklyView) {
				DrawProvScheds(g);
				DrawProvBars(g);
			}
			DrawGridLines(g);
			DrawMinutes(g);
		}

		///<summary>Including the practice schedule, provbars, and blockouts.</summary>
		private void DrawMainBackground(Graphics g) {
			List<Schedule> provSchedsForOp;
			//one giant rectangle for everything closed
			g.FillRectangle(_brushClosed,_timeWidth,0,_colWidth*_colCount+_provWidth*_provCount,_height);
			//then, loop through each day and operatory
			//int startHour=startTime.Hour;
			if(_isWeeklyView) {
				for(int d=0;d<_numOfWeekDaysToDisplay;d++) {//for each day of the week displayed
					//if any schedule for this day is type practice and status holiday and not assigned to specific ops (so either clinics are not enabled
					//or the holiday applies to all ops for the clinic), color all of the op columns in the view for this day with the holiday brush
					if(SchedListPeriod.FindAll(x => x.SchedType==ScheduleType.Practice && x.Status==SchedStatus.Holiday) //find all holidays
						.Any(x => (int)x.SchedDate.DayOfWeek==d+1 //for this day of the week
							&& (x.ClinicNum==0 || (x.ClinicNum==Clinics.ClinicId)))) //and either practice or for this clinic
					{
						g.FillRectangle(_brushHoliday,_timeWidth+1+d*_weekDayWidth,0,_weekDayWidth,_height);
					}
					//DayOfWeek enum goes from Sunday to Saturday 0-6, OD goes Monday to Sunday 0-6
					DayOfWeek dayofweek=(DayOfWeek)((d+1)%7);//when d==0, dayofweek=monday (or (DayOfWeek)1).  when d==6 we want dayofweek=sunday (or (DayOfWeek)0)
					for(int i=0;i<_colCount;i++) {//for each operatory visible for this view and day
						provSchedsForOp=Schedules.GetProvSchedsForOp(SchedListPeriod,dayofweek,VisOps[i]);
						//for each schedule assigned to this op
						foreach(Schedule schedCur in provSchedsForOp.Where(x => x.SchedType==ScheduleType.Provider)) {
							g.FillRectangle(_brushOpen
								,_timeWidth+1+d*_weekDayWidth+(float)i*_weekApptWidth
								,(schedCur.StartTime.Hours-_timeStart.Hours)*_lineH*_rowsPerHr+(int)schedCur.StartTime.Minutes*_lineH/_minPerRow//RowsPerHr=6, MinPerRow=10
								,_weekApptWidth
								,(schedCur.StopTime-schedCur.StartTime).Hours*_lineH*_rowsPerHr+(schedCur.StopTime-schedCur.StartTime).Minutes*_lineH/_minPerRow);
						}
					}
				}
			}
			else {//only one day showing
				//if any schedule for the period is type practice and status holiday and either ClinicNum is 0 (HQ clinic or clinics not enabled) or clinics
				//are enabled and the schedule.ClinicNum is the currently selected clinic
				//SchedListPeriod contains scheds for only one day, not for a week
				if(SchedListPeriod.FindAll(x => x.SchedType==ScheduleType.Practice && x.Status==SchedStatus.Holiday) //find all holidays
					.Any(x => x.ClinicNum==0 || (x.ClinicNum==Clinics.ClinicId)))//for the practice or clinic
				{
					g.FillRectangle(_brushHoliday,_timeWidth+1,0,_colWidth*_colCount+_provWidth*_provCount,_height);
				}
				for(int i=0;i<_colCount;i++) {//ops per page in day view
					if(i==VisOps.Count) {
						break;
					}
					//int k=colsPerPage*pageColumn+i;//pageColumn is always 0. All that's left is i.  Whole thing was redundant.
					//if(k>=ApptDrawing.VisOps.Count) {
					//	break;
					//}
					//this next line needs to eventually be moved into this class:
					provSchedsForOp=Schedules.GetProvSchedsForOp(SchedListPeriod,VisOps[i]);
					foreach(Schedule schedCur in provSchedsForOp) {
						if(schedCur.StartTime.Hours>=24 || (_timeStop.Hours!=0 && schedCur.StartTime.Hours>=_timeStop.Hours)) {
							continue;
						}
						g.FillRectangle(_brushOpen
							,_timeWidth+_provWidth*_provCount+i*_colWidth
							,(schedCur.StartTime.Hours-_timeStart.Hours)*_lineH*_rowsPerHr+(int)schedCur.StartTime.Minutes*_lineH/_minPerRow//6RowsPerHr 10MinPerRow
							,_colWidth
							,(schedCur.StopTime-schedCur.StartTime).Hours*_lineH*_rowsPerHr//6
								+(schedCur.StopTime-schedCur.StartTime).Minutes*_lineH/_minPerRow);//10
					}
					//now, fill up to 2 timebars along the left side of each rectangle.
					foreach(Schedule schedCur in provSchedsForOp) {
						if(schedCur.Ops.Count==0) {//if this schedule is not assigned to specific ops, skip
							continue;
						}
						float provWidthAdj=0f;//use to adjust for primary and secondary provider bars in op
						if(Provider.GetById(schedCur.ProvNum).IsSecondary) {
							provWidthAdj=_provWidth;//drawing secondary prov bar so shift right
						}
						using(SolidBrush solidBrushProvColor=new SolidBrush(Provider.GetById(schedCur.ProvNum).Color)) {
							g.FillRectangle(solidBrushProvColor
								,_timeWidth+_provWidth*_provCount+i*_colWidth+provWidthAdj
								,(schedCur.StartTime.Hours-_timeStart.Hours)*_lineH*_rowsPerHr+(int)schedCur.StartTime.Minutes*_lineH/_minPerRow//6RowsPerHr 10MinPerRow
								,_provWidth
								,(schedCur.StopTime-schedCur.StartTime).Hours*_lineH*_rowsPerHr//6
									+(schedCur.StopTime-schedCur.StartTime).Minutes*_lineH/_minPerRow);//10
						}
					}
				}
			}
		}

		private void DrawProvOpHeader(Graphics g){
			g.FillRectangle(_brushBackground,0,0,_width,_heightProvOpHeader);
			if(_isWeeklyView) {
				for(int d=0;d<_numOfWeekDaysToDisplay;d++) {//for each day of the week displayed

				}
			}
			else{//only one day showing
				for(int i=0;i<VisProvs.Count;i++){
					using(SolidBrush solidBrushProvColor=new SolidBrush(VisProvs[i].Color)) {
						g.FillRectangle(solidBrushProvColor,_timeWidth+_provWidth*i,0,_provWidth,_heightProvOpHeader);
					}
					g.DrawLine(_penVertPrimary,_timeWidth+_provWidth*i,0,_timeWidth+_provWidth*i,_heightProvOpHeader);//to left of prov cell
				}
				for(int i=0;i<_colCount;i++) {//ops per page in day view
					if(i==VisOps.Count) {//for printing
						break;
					}
					Color colorOp=Color.Empty;
					//operatory color is based solely on provider colors and IsHygiene.  It has nothing to do with schedules
					if(VisOps[i].ProvDentistId.HasValue && !VisOps[i].IsHygiene) {
						colorOp=Provider.GetById(VisOps[i].ProvDentistId.Value).Color;
					}
					else if(VisOps[i].ProvHygienistId.HasValue && VisOps[i].IsHygiene) {
						colorOp=Provider.GetById(VisOps[i].ProvHygienistId.Value).Color;
					}
					if(colorOp!=Color.Empty){
						using(SolidBrush solidBrushProvColor=new SolidBrush(colorOp)) {
							g.FillRectangle(solidBrushProvColor,_timeWidth+_provWidth*_provCount+i*_colWidth,0,_colWidth,_heightProvOpHeader);
						}
					}
					SizeF sizeTextMeasured=g.MeasureString(VisOps[i].Description,_fontDefault);
					float widthText=sizeTextMeasured.Width;
					float xPos;
					if(widthText>_colWidth-2){
						xPos=_timeWidth+_provWidth*_provCount+i*_colWidth+1;//left side of column
						widthText=_colWidth-2;
					}
					else{
						xPos=_timeWidth+_provWidth*_provCount+i*_colWidth+(_colWidth/2f)-(widthText/2f);//centered
					}
					PointF locationText=new PointF(xPos,2);
					float heightText=_heightProvOpHeader-2;
					g.DrawString(VisOps[i].Description,_fontDefault,_brushTextBlack,new RectangleF(locationText.X,locationText.Y,widthText,heightText));
					g.DrawLine(_penVertPrimary,_timeWidth+_provWidth*_provCount+i*_colWidth,0,_timeWidth+_provWidth*_provCount+i*_colWidth,_heightProvOpHeader);//to left of op cell
				}
			}
			//outline
			g.DrawRectangle(_penVertPrimary,_timeWidth,0,_width-(_timeWidth*2),_heightProvOpHeader-1);
		}

		private void DrawBlockouts(Graphics g){
			Schedule[] schedForType=Schedules.GetForType(SchedListPeriod,ScheduleType.Blockout,0);
			SolidBrush brushBlockBackg;//gets disposed here after each loop through i blockoutType
			Pen penOutline;//gets disposed here after each loop through i blockoutType
			string blockText;
			RectangleF rect;
			for(int i=0;i<schedForType.Length;i++) {
				brushBlockBackg=new SolidBrush(Defs.GetColor(DefinitionCategory.BlockoutTypes,schedForType[i].BlockoutType));
				penOutline=new Pen(Defs.GetColor(DefinitionCategory.BlockoutTypes,schedForType[i].BlockoutType),2);
				blockText=Defs.GetName(DefinitionCategory.BlockoutTypes,schedForType[i].BlockoutType)+"\r\n"+schedForType[i].Note;
				for(int o=0;o<schedForType[i].Ops.Count;o++) {
					int startHour=_timeStart.Hours;
					if(_isPrinting) {//Filtering logic for printing.
						int stopHour=_timeStop.Hours;
						if(stopHour==0) {
							stopHour=24;
						}
						if(schedForType[i].StartTime.Hours>=stopHour) {
							continue;//Blockout starts after the current time frame.
						}
						if(schedForType[i].StopTime.Hours<=stopHour) {
							stopHour=schedForType[i].StopTime.Hours;
						}
						if(GetIndexOp(schedForType[i].Ops[o])>=(_printingColCountOverride*_printingPageColumn+_printingColCountOverride)
						|| GetIndexOp(schedForType[i].Ops[o])<_printingColCountOverride*_printingPageColumn) {
							continue;//Blockout not on current page.
						}
					}
					if(IsWeeklyView) {
						if(GetIndexOp(schedForType[i].Ops[o])==-1) {
							continue;//don't display if op not visible
						}
						//this is a workaround because we start on Monday:
						int dayofweek=(int)schedForType[i].SchedDate.DayOfWeek-1;
						if(dayofweek==-1) {
							dayofweek=6;
						}
						rect=new RectangleF(
							_timeWidth+1+(dayofweek)*_weekDayWidth
							+_weekApptWidth*(GetIndexOp(schedForType[i].Ops[o],VisOps)-(_printingColCountOverride*_printingPageColumn))
							,(schedForType[i].StartTime.Hours-startHour)*_lineH*_rowsPerHr
							+schedForType[i].StartTime.Minutes*_lineH/_minPerRow
							,_weekApptWidth-1
							,(schedForType[i].StopTime-schedForType[i].StartTime).Hours*_lineH*_rowsPerHr
							+(schedForType[i].StopTime-schedForType[i].StartTime).Minutes*_lineH/_minPerRow);
					}
					else {
						if(GetIndexOp(schedForType[i].Ops[o])==-1) {
							continue;//don't display if op not visible
						}
						rect=new RectangleF(
							_timeWidth+_provWidth*_provCount
							+_colWidth*(GetIndexOp(schedForType[i].Ops[o],VisOps)-(_printingColCountOverride*_printingPageColumn))
							+_provWidth*2//so they don't overlap prov bars
							,(schedForType[i].StartTime.Hours-startHour)*_lineH*_rowsPerHr
							+schedForType[i].StartTime.Minutes*_lineH/_minPerRow
							,_colWidth-1-_provWidth*2
							,(schedForType[i].StopTime-schedForType[i].StartTime).Hours*_lineH*_rowsPerHr
							+(schedForType[i].StopTime-schedForType[i].StartTime).Minutes*_lineH/_minPerRow);
					}
					//paint either solid block or outline
					if(Preference.GetBool(PreferenceName.SolidBlockouts)) {
						g.FillRectangle(brushBlockBackg,rect);
						g.DrawLine(Pens.Black,rect.X,rect.Y+1,rect.Right-1,rect.Y+1);
					}
					else {
						g.DrawRectangle(penOutline,rect.X+1,rect.Y+2,rect.Width-2,rect.Height-3);
					}
					g.DrawString(blockText,_fontBlockout,_brushBlockText,rect);
				}//for o ops
				brushBlockBackg.Dispose();
				penOutline.Dispose();
			}//for i blockoutType
		}

		private void DrawWebSchedASAPSlots(Graphics g){

		}

		///<summary>Fills a rectangle with downward-sloping or upward-sloping diagonal lines.</summary>
		public static void FillWithDiagonalLines(Graphics g,RectangleF rectToFill,Pen linePen,float pixelsBetweenLines,bool isUpwardSloping=false) {
			if(isUpwardSloping) {
				//Walk along the bottom of the rectangle and draw lines that go to the right side or the top.
				for(float x=rectToFill.X+pixelsBetweenLines;x<rectToFill.Right;x+=pixelsBetweenLines) {
					float y=rectToFill.Bottom;
					float y2=y-(rectToFill.Right-x);
					float x2=rectToFill.Right-Math.Max(rectToFill.Top-y2,0);
					if(y2 < rectToFill.Top) {
						y2=rectToFill.Top;
					}
					g.DrawLine(linePen,x,y,x2,y2);
				}
				//Walk along the left side of the rectangle a draw lines that go to the top or right side.
				for(float y=rectToFill.Bottom;y>rectToFill.Top;y-=pixelsBetweenLines) {
					float x=rectToFill.Left;
					float x2=x+(y-rectToFill.Top);
					float y2=rectToFill.Top-Math.Max(rectToFill.Left-x2,0);
					if(x2 < rectToFill.Left) {
						x2=rectToFill.Left;
					}
					g.DrawLine(linePen,x,y,x2,y2);
				}
				return;
			}
			else {
				//Walk along the bottom of the rectangle and draw lines that go to the left side or the top.
				for(float x=rectToFill.X+pixelsBetweenLines;x<rectToFill.Right;x+=pixelsBetweenLines) {
					float y2=rectToFill.Bottom-(x-rectToFill.X);
					float x2=rectToFill.Left+Math.Max(rectToFill.Top-y2,0);
					if(y2 < rectToFill.Top) {
						y2=rectToFill.Top;
					}
					g.DrawLine(linePen,x,rectToFill.Bottom,x2,y2);
				}
				//Walk along the right side of the rectangle a draw lines that go to the top or left side.
				float offsetY=pixelsBetweenLines-rectToFill.Width%pixelsBetweenLines;
				for(float y=rectToFill.Bottom-offsetY;y>rectToFill.Top;y-=pixelsBetweenLines) {
					float x2=rectToFill.Right-(y-rectToFill.Top);
					float y2=rectToFill.Top+Math.Max(rectToFill.Left-x2,0);
					if(x2 < rectToFill.Left) {
						x2=rectToFill.Left;
					}
					g.DrawLine(linePen,rectToFill.Right,y,x2,y2);
				}
			}
		}

		///<summary>Returns the index of the opNum within VisOps.  Returns -1 if not in VisOps.</summary>
		private int GetIndexOp(long opNum,List<Operatory> VisOps) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<VisOps.Count;i++) {
				if(VisOps[i].Id==opNum)
					return i;
			}
			return -1;
		}

		private void DrawProvScheds(Graphics g){
			Provider provCur;
			Schedule[] schedForType;
			int startHour=_timeStart.Hours;
			int stopHour=_timeStop.Hours;
			if(stopHour==0) {
				stopHour=24;
			}
			for(int j=0;j<VisProvs.Count;j++) {
				provCur=VisProvs[j];
				schedForType=Schedules.GetForType(SchedListPeriod,ScheduleType.Provider,provCur.Id);
				for(int i=0;i<schedForType.Length;i++) {
					stopHour=_timeStop.Hours;//Reset stopHour every time.
					if(stopHour==0) {
						stopHour=24;
					}
					if(schedForType[i].StartTime.Hours>=stopHour) {
						continue;
					}
					if(schedForType[i].StopTime.Hours<=stopHour) {
						stopHour=schedForType[i].StopTime.Hours;
					}
					g.FillRectangle(_brushOpen
						,_timeWidth+_provWidth*j
						,(schedForType[i].StartTime.Hours-startHour)*_lineH*_rowsPerHr//6
						+(int)schedForType[i].StartTime.Minutes*_lineH/_minPerRow//10
						,_provWidth
						,(schedForType[i].StopTime-schedForType[i].StartTime).Hours*_lineH*_rowsPerHr//6
						+(schedForType[i].StopTime-schedForType[i].StartTime).Minutes*_lineH/_minPerRow);//10
				}
			}
		}

		private void DrawProvBars(Graphics g){
			if(!IsWeeklyView) {
				foreach(DataRow dataRow in TableAppointments.Rows) {
					ProvBarShading(dataRow);
				}
			}
			int startingPoint=_timeStart.Hours*_rowsPerHr;
			int stopHour=_timeStop.Hours;
			if(stopHour==0) {
				stopHour=24;
			}
			int endingPoint=stopHour*_rowsPerHr;
			for(int j=0;j<_provBar.Length;j++) {
				for(int i=0;i<24*_rowsPerHr;i++) {
					if(i<startingPoint) {
						continue;
					}
					if(i>=endingPoint) {
						break;
					}
					switch(_provBar[j][i]) {
						case 0:
							break;
						case 1:
							try {
								using(SolidBrush solidBrushProvColor=new SolidBrush(VisProvs[j].Color)) {
									g.FillRectangle(solidBrushProvColor,_timeWidth+_provWidth*j+1,((i-startingPoint)*_lineH)+1,_provWidth-1,_lineH-1);
								}
							}
							catch {//design-time
								g.FillRectangle(Brushes.White,_timeWidth+_provWidth*j+1,((i-startingPoint)*_lineH)+1,_provWidth-1,_lineH-1);
							}
							break;
						case 2:
							using(HatchBrush hatchBrushProvColor=new HatchBrush(HatchStyle.DarkUpwardDiagonal,Color.Black,VisProvs[j].Color)) {
								g.FillRectangle(hatchBrushProvColor,_timeWidth+_provWidth*j+1,((i-startingPoint)*_lineH)+1,_provWidth-1,_lineH-1);
							}
							break;
						default://more than 2
							g.FillRectangle(_brushLinesAndText,_timeWidth+_provWidth*j+1,((i-startingPoint)*_lineH)+1,_provWidth-1,_lineH-1);
							break;
					}
				}
			}
		}

		private void DrawGridLines(Graphics g){
			//Vert
			if(_isWeeklyView) {
				g.DrawLine(_penVertPrimary,0,0,0,_height);
				g.DrawLine(_penVertWhite,_timeWidth-1,0,_timeWidth-1,_height);
				g.DrawLine(_penVertPrimary,_timeWidth,0,_timeWidth,_height);
				for(int d=0;d<_numOfWeekDaysToDisplay;d++) {
					g.DrawLine(_penVertPrimary,_timeWidth+_weekDayWidth*d,0,_timeWidth+_weekDayWidth*d,_height);
				}
				g.DrawLine(_penVertPrimary,_timeWidth+_weekDayWidth*_numOfWeekDaysToDisplay,0,_timeWidth+1+_weekDayWidth*_numOfWeekDaysToDisplay,_height);
				g.DrawLine(_penVertPrimary,_timeWidth*2+_weekDayWidth*_numOfWeekDaysToDisplay,0,_timeWidth*2+1+_weekDayWidth*_numOfWeekDaysToDisplay,_height);
			}
			else {
				g.DrawLine(_penVertPrimary,0,0,0,_height);
				g.DrawLine(_penVertWhite,_timeWidth-2,0,_timeWidth-2,_height);
				g.DrawLine(_penVertPrimary,_timeWidth-1,0,_timeWidth-1,_height);
				for(int i=0;i<_provCount;i++) {
					g.DrawLine(_penVertPrimary,_timeWidth+_provWidth*i,0,_timeWidth+_provWidth*i,_height);
				}
				for(int i=0;i<_colCount;i++) {
					g.DrawLine(_penVertPrimary,_timeWidth+_provWidth*_provCount+_colWidth*i,0,_timeWidth+_provWidth*_provCount+_colWidth*i,_height);
				}
				g.DrawLine(_penVertPrimary,_timeWidth+_provWidth*_provCount+_colWidth*_colCount,0,_timeWidth+_provWidth*_provCount+_colWidth*_colCount,_height);
				g.DrawLine(_penVertPrimary,_timeWidth*2+_provWidth*_provCount+_colWidth*_colCount,0,_timeWidth*2+_provWidth*_provCount+_colWidth*_colCount,_height);
			}
			//horiz gray
			for(float i=0;i<_height;i+=_lineH*_rowsPerIncr) {
				g.DrawLine(_penHorizMinutes,_timeWidth,i,_timeWidth+_colWidth*_colCount+_provWidth*_provCount,i);
			}
			//horiz Hour lines
			for(float i=0;i<_height;i+=_lineH*_rowsPerHr) {
				g.DrawLine(_penHorizMinutes,0,i-1,_timeWidth*2+_colWidth*_colCount+_provWidth*_provCount,i-1);
				//the line below is black in the main area and dark gray in the timebar areas
				g.DrawLine(_penHorizDark,0,i,_timeWidth,i);
				g.DrawLine(_penHorizBlack,_timeWidth,i,_timeWidth+_colWidth*_colCount+_provWidth*_provCount,i);
				g.DrawLine(_penHorizDark,_timeWidth+_colWidth*_colCount+_provWidth*_provCount,i,_timeWidth*2+_colWidth*_colCount+_provWidth*_provCount,i);
			}
		}

		private void DrawMinutes(Graphics g){
			Font fontMinutes=new Font(FontFamily.GenericSansSerif,8);//was msSans
			Font fontMinutesBold=new Font(FontFamily.GenericSansSerif,8,FontStyle.Bold);//was Arial
			//g.TextRenderingHint=TextRenderingHint.SingleBitPerPixelGridFit;//to make printing clearer
			DateTime hour;
			CultureInfo ci=(CultureInfo)CultureInfo.CurrentCulture.Clone();
			string hFormat=Lans.GetShortTimeFormat(ci);
			string strTime;
			int stop=_timeStop.Hours;
			if(stop==0) {//12AM, but we want to end on the next day so set to 24
				stop=24;
			}
			int index=0;//This will cause drawing times to always start at the top.
			for(int i=_timeStart.Hours;i<stop;i++) {
				hour=new DateTime(2000,1,1,i,0,0);//hour is the only important part of this time.
				strTime=hour.ToString(hFormat,ci);
				strTime=strTime.Replace("a. m.","am");//So that the times are not cutoff for foreign users
				strTime=strTime.Replace("p. m.","pm");
				SizeF sizef=g.MeasureString(strTime,fontMinutesBold);
				g.DrawString(strTime,fontMinutesBold,_brushLinesAndText,_timeWidth-sizef.Width-2,index*_lineH*_rowsPerHr+1);
				g.DrawString(strTime,fontMinutesBold,_brushLinesAndText,_timeWidth+_colWidth*_colCount+_provWidth*_provCount,index*_lineH*_rowsPerHr+1);
				if(MinPerIncr==5) {
					g.DrawString(":15",fontMinutes,_brushLinesAndText,_timeWidth-19,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*3);
					g.DrawString(":30",fontMinutes,_brushLinesAndText,_timeWidth-19,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*6);
					g.DrawString(":45",fontMinutes,_brushLinesAndText,_timeWidth-19,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*9);
					g.DrawString(":15",fontMinutes,_brushLinesAndText,_timeWidth+_colWidth*_colCount+_provWidth*_provCount,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*3);
					g.DrawString(":30",fontMinutes,_brushLinesAndText,_timeWidth+_colWidth*_colCount+_provWidth*_provCount,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*6);
					g.DrawString(":45",fontMinutes,_brushLinesAndText,_timeWidth+_colWidth*_colCount+_provWidth*_provCount,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*9);
				}
				else if(MinPerIncr==10) {
					g.DrawString(":10",fontMinutes,_brushLinesAndText,_timeWidth-19,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr);
					g.DrawString(":20",fontMinutes,_brushLinesAndText,_timeWidth-19,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*2);
					g.DrawString(":30",fontMinutes,_brushLinesAndText,_timeWidth-19,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*3);
					g.DrawString(":40",fontMinutes,_brushLinesAndText,_timeWidth-19,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*4);
					g.DrawString(":50",fontMinutes,_brushLinesAndText,_timeWidth-19,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*5);
					g.DrawString(":10",fontMinutes,_brushLinesAndText,_timeWidth+_colWidth*_colCount+_provWidth*_provCount,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr);
					g.DrawString(":20",fontMinutes,_brushLinesAndText,_timeWidth+_colWidth*_colCount+_provWidth*_provCount,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*2);
					g.DrawString(":30",fontMinutes,_brushLinesAndText,_timeWidth+_colWidth*_colCount+_provWidth*_provCount,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*3);
					g.DrawString(":40",fontMinutes,_brushLinesAndText,_timeWidth+_colWidth*_colCount+_provWidth*_provCount,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*4);
					g.DrawString(":50",fontMinutes,_brushLinesAndText,_timeWidth+_colWidth*_colCount+_provWidth*_provCount,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*5);
				}
				else {//15
					g.DrawString(":15",fontMinutes,_brushLinesAndText,_timeWidth-19,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr);
					g.DrawString(":30",fontMinutes,_brushLinesAndText,_timeWidth-19,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*2);
					g.DrawString(":45",fontMinutes,_brushLinesAndText,_timeWidth-19,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*3);
					g.DrawString(":15",fontMinutes,_brushLinesAndText,_timeWidth+_colWidth*_colCount+_provWidth*_provCount,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr);
					g.DrawString(":30",fontMinutes,_brushLinesAndText,_timeWidth+_colWidth*_colCount+_provWidth*_provCount,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*2);
					g.DrawString(":45",fontMinutes,_brushLinesAndText,_timeWidth+_colWidth*_colCount+_provWidth*_provCount,index*_lineH*_rowsPerHr+_lineH*_rowsPerIncr*3);
				}
				index++;
			}
		}
#endregion DrawBackground

		private void DrawAppts(Graphics g){
			//Filling ContrApptSheet2 with appointmnets was originally in OD ContrAppt.RefreshModuleScreenPeriod line 1978
			foreach(DataRow dataRow in TableAppointments.Rows) {
				if(PIn.Date(dataRow["AptDateTime"].ToString()).Date < DateStart.Date || PIn.Date(dataRow["AptDateTime"].ToString()).Date > DateEnd.Date){
					continue;//Appointment is outside of our date range.
				}
				PointF location=SetLocation(dataRow,0,VisOps.Count,0);
				List<CodeBase.DateRange> listDateRangesOverlap=new List<DateRange>();
				string pattern=PIn.String(dataRow["Pattern"].ToString());
				string patternShowing=GetPatternShowing(pattern);
				//Size was originally set in ContrApptSingle.ResetData
				Size sizeAppt=SetSize(pattern);
				if(sizeAppt.Width<1){
					continue;
				}
				//bitmap drawing was originally in ContrApptSheet.DoubleBufferDraw line 255
				Bitmap bitmapAppt=new Bitmap(sizeAppt.Width,sizeAppt.Height);
				//we will later improve by just passing a graphics, and specifying loc within that g to draw
				using(Graphics graphicsAppt=Graphics.FromImage(bitmapAppt)) {
					ApptSingleDrawingJ.DrawEntireAppt(graphicsAppt,dataRow,patternShowing,sizeAppt.Width,sizeAppt.Height,ApptViewItemL.ApptRows,ApptViewItemL.ApptViewCur, TableApptFields,TablePatFields,8,false,listDateRangesOverlap);
				}
				g.DrawImage(bitmapAppt,location.X,location.Y);
				bitmapAppt.Dispose();
				//if(!IsWeeklyView) {
				//	ProvBarShading(dataRow);//moved over to provbar drawing
				//}
			}
		}

		private void DrawOutlines(Graphics g){

		}

		private void DrawRedTimeLine(Graphics g){
			//if(showRedTimeLine) {
			//	DrawTimeIndicatorLine(g);
			//}
			int curTimeY=(int)(DateTime.Now.Hour*_lineH*_rowsPerHr+DateTime.Now.Minute/60f*(float)_lineH*_rowsPerHr);
			//using(Pen penAppointmentTimeLineColor=new Pen(PrefC.GetColor(PrefName.AppointmentTimeLineColor))) {
			g.DrawLine(_penTimeLine,0,curTimeY
				,_timeWidth*2+_provWidth*_provCount+_colWidth*_colCount,curTimeY);
			g.DrawLine(_penTimeLine,0,curTimeY+1
				,_timeWidth*2+_provWidth*_provCount+_colWidth*_colCount,curTimeY+1);
			//}
		}

		#region Computations
		/*
		///<summary></summary>
		private void ComputeColDayWidth() {
			WeekDayWidth=(ApptSheetWidth-TimeWidth*2)/NumOfWeekDaysToDisplay;
		}

		///<summary></summary>
		private void ComputeColAptWidth() {
			WeekApptWidth=(float)(WeekDayWidth-1)/(float)ColCount;
		}

		///<summary></summary>
		private void SetLineHeight(int fontSize) {
			LineH=new Font("Arial",fontSize).Height;
		}

		///<summary></summary>
		private int XPosToOpIdx(int xPos) {
			int retVal;
			if(IsWeeklyView) {
				int day=XPosToDay(xPos);
				retVal=(int)Math.Floor((double)(xPos-TimeWidth-day*WeekDayWidth)/WeekApptWidth);
			}
			else {
				retVal=(int)Math.Floor((double)(xPos-TimeWidth-ProvWidth*ProvCount)/ColWidth);
			}
			if(retVal>ColCount-1)
				retVal=ColCount-1;
			if(retVal<0)
				retVal=0;
			return retVal;
		}

		///<summary>If not weekview, then it always returns 0.  If weekview, then it gives the dayofweek as int. Always based on current view, so 0 will be first day showing.</summary>
		private int XPosToDay(int xPos) {
			if(!IsWeeklyView) {
				return 0;
			}
			int retVal=(int)Math.Floor((double)(xPos-TimeWidth)/WeekDayWidth);
			if(retVal>NumOfWeekDaysToDisplay-1)
				retVal=NumOfWeekDaysToDisplay-1;
			if(retVal<0)
				retVal=0;
			return retVal;
		}

		///<summary>Called when mouse down anywhere on apptSheet. Automatically rounds down.</summary>
		private int YPosToHour(int yPos) {
			//int retVal=yPos/LineH/RowsPerHr;//newY/LineH/6;
			//return retVal;
			return 0;
		}

		///<summary>Called when mouse down anywhere on apptSheet. This will give very precise minutes. It is not rounded for accuracy.</summary>
		private int YPosToMin(int yPos) {
			int hourPortion=YPosToHour(yPos)*LineH*RowsPerHr;
			float MinPerPixel=60/(float)LineH/(float)RowsPerHr;
			int minutes=(int)((yPos-hourPortion)*MinPerPixel);
			return minutes;
		}

		///<summary>Used when dropping an appointment to a new location.  Converts x-coordinate to operatory index of ApptCatItems.VisOps, rounding to the nearest.  In this respect it is very different from XPosToOp.</summary>
		private int ConvertToOp(int newX) {
			int retVal=0;
			if(IsWeeklyView) {
				int dayI=XPosToDay(newX);//does not round
				int deltaDay=dayI*(int)WeekDayWidth;
				int adjustedX=newX-(int)TimeWidth-deltaDay;
				retVal=(int)Math.Round((double)(adjustedX)/WeekApptWidth);
				//when there are multiple days, special situation where x is within the last op for the day, so it goes to next day.
				if(retVal>VisOps.Count-1 && dayI<NumOfWeekDaysToDisplay-1) {
					retVal=0;
				}
			}
			else {
				retVal=(int)Math.Round((double)(newX-TimeWidth-ProvWidth*ProvCount)/ColWidth);
			}
			//make sure it's not outside bounds of array:
			if(retVal > VisOps.Count-1)
				retVal=VisOps.Count-1;
			if(retVal<0)
				retVal=0;
			return retVal;
		}

		///<summary>Used when dropping an appointment to a new location.  Converts x-coordinate to day index.  Only used in weekly view.</summary>
		private int ConvertToDay(int newX) {
			int retVal=(int)Math.Floor((double)(newX-TimeWidth)/(double)WeekDayWidth);
			//the above works for every situation except when in the right half of the last op for a day. Test for that situation:
			if(newX-TimeWidth > (retVal+1)*WeekDayWidth-WeekApptWidth/2) {
				retVal++;
			}
			//make sure it's not outside bounds of array:
			if(retVal>NumOfWeekDaysToDisplay-1)
				retVal=NumOfWeekDaysToDisplay-1;
			if(retVal<0)
				retVal=0;
			return retVal;
		}

		///<summary>Used when dropping an appointment to a new location. Rounds to the nearest increment.</summary>
		private int ConvertToHour(int newY) {
			//return (int)((newY+LineH/2)/6/LineH);
			return (int)(((double)newY+(double)LineH*(double)RowsPerIncr/2)/(double)RowsPerHr/(double)LineH);
		}

		///<summary>Used when dropping an appointment to a new location. Rounds to the nearest increment.</summary>
		private int ConvertToMin(int newY) {
			//int retVal=(int)(Decimal.Remainder(newY,6*LineH)/LineH)*10;
			//first, add pixels equivalent to 1/2 increment: newY+LineH*RowsPerIncr/2
			//Yloc     Height     Rows      1
			//---- + ( ------ x --------- x - )
			//  1       Row     Increment   2
			//then divide by height per hour: RowsPerHr*LineH
			//Rows   Height
			//---- * ------
			//Hour    Row
			int pixels=(int)Decimal.Remainder(
				(decimal)newY+(decimal)LineH*(decimal)RowsPerIncr/2
				,(decimal)RowsPerHr*(decimal)LineH);
			//We are only interested in the remainder, and this is called pixels.
			//Convert pixels to increments. Round down to nearest increment when converting to int.
			//pixels/LineH/RowsPerIncr:
			//pixels    Rows    Increment
			//------ x ------ x ---------
			//  1      pixels     Rows
			int increments=(int)((double)pixels/(double)LineH/(double)RowsPerIncr);
			//Convert increments to minutes: increments*MinPerIncr
			int retVal=increments*MinPerIncr;
			if(retVal==60)
				return 0;
			return retVal;
		}*/

		///<summary>Originally called from ContrAppt.comboView_SelectedIndexChanged and ContrAppt.RefreshVisops.</summary>
		private void ComputeColWidth() {
			if(_printingColCountOverride>0) {
				_colCount=_printingColCountOverride;
			}
			else {
				_colCount=VisOps.Count;
			}
			if(_isWeeklyView) {
				_colCount=VisOps.Count;
				_provCount=0;
			}
			else {
				_provCount=VisProvs.Count;
			}
			if(_colCount==0) {
				_colWidth=0;
			}
			else {
				if(_isWeeklyView) {
					_weekDayWidth=(_width-_timeWidth*2)/_numOfWeekDaysToDisplay;
					_weekApptWidth=(float)(_weekDayWidth-1)/(float)_colCount;
					_colWidth=(_width-_timeWidth*2-_provWidth*_provCount)/_colCount;
				}
				else {
					_colWidth=(_width-_timeWidth*2-_provWidth*_provCount)/(float)_colCount;
				}
			}
			//MinPerIncr=PrefC.GetInt(PrefName.AppointmentTimeIncrement);//moved this outside
			_minPerRow=(float)MinPerIncr/(float)_rowsPerIncr;
			_rowsPerHr=60/MinPerIncr*_rowsPerIncr;
		}

		///<summary>Returns the index of the opNum within VisOps.  Returns -1 if not in VisOps.</summary>
		private int GetIndexOp(long opNum) {
			//No need to check RemotingRole; no call to db.
			int index;
			return _dictOpNumToColumnNum.TryGetValue(opNum,out index) ? index : -1;
		}

		///<summary>Returns the index of the provNum within VisProvs.</summary>
		private int GetIndexProv(long provNum) {
			//No need to check RemotingRole; no call to db.
			int index;
			return _dictProvNumToColumnNum.TryGetValue(provNum,out index) ? index : -1;
		}

		///<summary></summary>
		private void ProvBarShading(DataRow row) {
			string patternShowing=GetPatternShowing(row["Pattern"].ToString());
			int indexProv=-1;
			if(row["IsHygiene"].ToString()=="1") {
				indexProv=GetIndexProv(PIn.Long(row["ProvHyg"].ToString()));
			}
			else {
				indexProv=GetIndexProv(PIn.Long(row["ProvNum"].ToString()));
			}
			if(indexProv!=-1 && row["AptStatus"].ToString()!=((int)ApptStatus.Broken).ToString()) {
				int startIndex=(int)(ConvertToY(row,0)/_lineH);//rounds down
				for(int k=0;k<patternShowing.Length;k++) {
					if(patternShowing.Substring(k,1)=="X") {
						try {
							_provBar[indexProv][startIndex+k]++;
						}
						catch {
							//appointment must extend past midnight.  Very rare
						}
					}
				}
			}
		}

		#endregion Computations

		#region Computations originally in ApptSingleDrawing
		///<summary>Was ApptSingleDrawing.SetLocation.  This is only called when viewing appointments on the Appt module.  For Planned apt and pinboard, use SetSize instead so that the location won't change.  Pass 0 for startHour unless printing.  Pass visible ops for colsPerPage unless printing.  Pass 0 for pageColumn unless printing.</summary>
		private PointF SetLocation(DataRow dataRoww,int beginHour,int colsPerPage,int pageColumn) {
			PointF pointReturn;
			if(IsWeeklyView) {
				pointReturn=new PointF(ConvertToX(dataRoww,colsPerPage,pageColumn),ConvertToY(dataRoww,beginHour));
			}
			else {
				pointReturn=new PointF(ConvertToX(dataRoww,colsPerPage,pageColumn)+2,ConvertToY(dataRoww,beginHour));
			}
			return pointReturn;
		}

		///<summary>Was ApptSingleDrawing.SetSize.  Used for Planned apt and pinboard instead of SetLocation so that the location won't be altered.</summary>
		private Size SetSize(string pattern) {
			float apptSingleWidth;
			if(_colWidth<5){
				apptSingleWidth=0;
			}
			else{
				apptSingleWidth=_colWidth-5;
			}
			if(IsWeeklyView) {
				apptSingleWidth=(int)_weekApptWidth;
			}
			//height is based on original 5 minute pattern. Might result in half-rows
			float apptSingleHeight=pattern.Length*_lineH*RowsPerIncr;
			if(MinPerIncr==10) {
				apptSingleHeight=apptSingleHeight/2;
			}
			if(MinPerIncr==15) {
				apptSingleHeight=apptSingleHeight/3;
			}
			return new Size((int)apptSingleWidth,(int)apptSingleHeight);
		}

		///<summary>Was ApptSingleDrawing.ConvertToX. Called from SetLocation to establish X position of control.</summary>
		private float ConvertToX(DataRow dataRoww,int colsPerPage,int pageColumn) {
			if(IsWeeklyView) {
				//the next few lines are because we start on Monday instead of Sunday
				int dayofweek=(int)PIn.DateT(dataRoww["AptDateTime"].ToString()).DayOfWeek-1;
				if(dayofweek==-1) {
					dayofweek=6;
				}
				return _timeWidth
					+_weekDayWidth*(dayofweek)+1
					+(_weekApptWidth*(GetIndexOp(PIn.Long(dataRoww["Op"].ToString()))-(colsPerPage*pageColumn)));
			}
			else {
				string strOp=dataRoww["Op"].ToString();
				int idxOp=GetIndexOp(PIn.Long(strOp));
				return _timeWidth+_provWidth*_provCount
					+_colWidth*(idxOp-(colsPerPage*pageColumn))+1;
				//Info.MyApt.Op))+1;
			}
		}

		///<summary>Was ApptSingleDrawing.ConvertToY.  Called from SetLocation to establish Y position of control.  Also called from ContrAppt.RefreshDay when determining ProvBar markings. Does not round to the nearest row.</summary>
		private float ConvertToY(DataRow dataRoww,int beginHour) {
			DateTime aptDateTime=PIn.DateT(dataRoww["AptDateTime"].ToString());
			float retVal=(float)(((double)(aptDateTime.Hour-beginHour)*(double)60
				/(double)MinPerIncr
				+(double)aptDateTime.Minute
				/(double)MinPerIncr
				)*(double)_lineH*RowsPerIncr);
			return retVal;
		}

		///<summary>Was ApptSingleDrawing.GetPatternShowing.  This converts the dbPattern in 5 minute interval into the pattern that will be viewed based on RowsPerIncrement and AppointmentTimeIncrement.  So it will always depend on the current view.Therefore, it should only be used for visual display purposes rather than within the FormAptEdit. If height of appointment allows a half row, then this includes an increment for that half row.</summary>
		private string GetPatternShowing(string dbPattern) {
			StringBuilder strBTime=new StringBuilder();
			for(int i=0;i<dbPattern.Length;i++) {
				for(int j=0;j<RowsPerIncr;j++) {
					strBTime.Append(dbPattern.Substring(i,1));
				}
				if(MinPerIncr==10) {
					i++;//skip
				}
				if(MinPerIncr==15) {
					i++;
					i++;//skip two
				}
			}
			return strBTime.ToString();
		}

		///<summary>Was ApptSingleDrawing.ApptWithinTimeFrame.  Tests if the appt is in the allotted time frame and is in a visible operatory.  Returns false in order to skip drawing for appointment printing.</summary>
		private bool ApptWithinTimeFrame(long opNum,DateTime aptDateTime,string pattern,DateTime beginTime,DateTime endTime,int colsPerPage,int pageColumn) {
			//Test if appts op is currently visible.
			bool visible=false;
			if(IsWeeklyView) {
				if(GetIndexOp(opNum) > -1) {
					visible=true;
				}
			}
			else {//Daily view
				for(int i=0;i<colsPerPage;i++) {
					if(i==VisOps.Count) {
						return false;
					}
					int k=colsPerPage*pageColumn+i;
					if(k>=VisOps.Count) {
						return false;
					}
					if(k==GetIndexOp(opNum)) {
						visible=true;
						break;
					}
				}
			}
			if(!visible) {//Op not visible so don't test time frame.
				return false;
			}
			//Test if any portion of appt is within time frame.
			TimeSpan aptTimeBegin=aptDateTime.TimeOfDay;
			TimeSpan aptTimeEnd=aptTimeBegin.Add(new TimeSpan(0,pattern.Length*5,0));
			int aptHourBegin=aptTimeBegin.Hours;
			int aptHourEnd=aptTimeEnd.Hours;
			if(aptHourEnd==0) {
				aptHourEnd=24;
			}
			int beginHour=beginTime.Hour;
			int endHour=endTime.Hour;
			if(endHour==0) {
				endHour=24;
			}
			//If the appointment begins on or after the stopping hour (because we don't support minutes currently) then this appointment is not visible.
			//However, we need to check the time portion of the appointment ending time in correlation to the begin hour 
			//because the appointment could end within the same hour that the printing begin hour is set to.
			//E.g. an appointment from 8 AM to 8:40 AM needs to show as visible when printing a schedule from 8 AM to 5 PM.
			TimeSpan timePrintBegin=new TimeSpan(beginHour,0,0);
			if(aptHourBegin>=endHour || aptTimeEnd<=timePrintBegin) {
				return false;
			}
			return true;
		}
		#endregion Computations originally in ApptSingleDrawing

		

	}
}

//List of followup items:
//ApptViewItemL.GetForCurView, line 44.  Makes calls to two methods below it.  This is what fills VisOps, VisProvs, the 2 dicts, and RowsPerIncrement.  Called from ContrAppt.  
//I think it might be useful to go back to indices for visOps and visProvs instead of full objects. That's a big part of why we originally lost our speed.
//Schedules.GetProvSchedsForOp, line 732.  This is called for each column of each day during DrawMainBackground.  It's also called from ContrAppt.OpPanelProv_MouseDown, line 2270 in order to show provider info when click op.  I'll bet we can organize this scheds ahead of time faster than these loops.
