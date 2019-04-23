using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;

namespace CodeBase.MVC {
	///<summary>Base class that all forms should extend.  Provides accessibility to features for all forms like Help and object processing.</summary>
	///<typeparam name="P">Processor Type - Typically set to Signalod but can be set to any object type that gets processed by the form.</typeparam>
	public class ODFormAbs<P> : Form, IODProcessor<P> {

		///<summary>True when form has been shown by the system.
		///Shown occurs last in the forms construction life cycle.
		///The Shown event is only raised the first time a form is displayed.</summary>
		private bool _hasShown=false;
		///<summary>Only true if FormClosed has been called by the system.</summary>
		private bool _hasClosed=false;
		///<summary>Keeps track of window state changes.  We use it to restore minimized forms to their previous state.</summary>
		private FormWindowState _windowStateOld;
		private FormHelpWrapper _formHelp=null;
		///<summary>Holds the windows edition of the client. HelpForm and CreateHelpMenu() use this.
		///Linux users will be treated as Windows 10 as far as the HelpForm is concerned. Formatting
		///issues on Linux will have to be handled as they are found.</summary>
		private string _windowsEdition;

		///<summary>Override and set to false if you want your form not to respond to F1 key for help.</summary>
		protected virtual bool HasHelpKey {
			get {
				return true;
			}
		}

		///<summary>True when form has been shown by the system.</summary>
		public bool HasShown {
			get {
				return _hasShown;
			}
		}

		///<summary>Only true if FormClosed has been called by the system.</summary>
		public bool HasClosed {
			get {
				return _hasClosed;
			}
		}

		///<summary>Returns true if the form passed in has been disposed or if it extends ODFormAbs and HasClosed is true.</summary>
		public static bool IsDisplosedOrClosed(Form form) {
			bool isClosed=false;
			if(form.IsDisposed) {//Usually the system will set IsDisposed to true after a form has closed.  Not true for FormHelpBrowser.
				isClosed=true;
			}
			else if(form.GetType().GetProperty("HasClosed")!=null) {//Is a Form and has property HasClosed => Assume is an ODFormAbs.
				//Difficult to compare type to ODFormAbs, because it is a template class.
				if((bool)form.GetType().GetProperty("HasClosed").GetValue(form)) {//This is how we know FormHelpBrowser has closed.
					isClosed=true;
				}
			}
			return isClosed;
		}

		public ODFormAbs() {
			#region Designer Properties
			this.AutoScaleMode=AutoScaleMode.None;
			this.ClientSize=new System.Drawing.Size(974,696);
			this.KeyPreview=true;
			this.MinimumSize=new System.Drawing.Size(100,100);
			this.Name="ODFormAbs";
			this.StartPosition=FormStartPosition.CenterScreen;
			this.Text="ODFormAbs";
			#endregion
			this.Load+=ODFormAbs_Load;
			this.Shown+=new EventHandler(this.ODFormAbs_Shown);
			this.FormClosing+=new FormClosingEventHandler(this.ODFormAbs_FormClosing);//Will fire first for all FormClosing events of this form.
			this.Resize+=new EventHandler(this.ODFormAbs_Resize);
		}

		private void InitializeComponent() {
			this.SuspendLayout();
			// 
			// ODFormAbs
			// 
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Name = "ODFormAbs";
			this.ResumeLayout(false);
		}

		private void ODFormAbs_Load(object sender,EventArgs e) {
			BackColor=ODColorTheme.FormBackColor;
			_windowsEdition=new ComputerInfo().OSFullName;
		}

		private void ODFormAbs_Shown(object sender,EventArgs e) {
			_hasShown=true;//Occurs after Load(...)
			//This form has just invoked the "Shown" event which probably means it is important and needs to actually show to the user.
			//There are times in the application that a progress window (e.g. splash screen) will be showing to the user and a new form is trying to show.
			//Therefore, forcefully invoke "Activate" if there is a progress window currently on the screen.
			//Invoking Activate will cause the new form to show above the progress window (if TopMost=false) even though it is in another thread.
			if(ODProgress.FormProgressActive!=null) {
				this.Activate();
			}
			ShowHelpButtonSafe();
		}

        ///<summary>Sets the entire form into "read only" mode by disabling all controls on the form.
        ///Pass in any controls that should say enabled (e.g. Cancel button). 
        ///This can be used to stop users from clicking items they do not have permission for.</summary>
        public void DisableForm(params Control[] enabledControls)
        {
            foreach (Control ctrl in this.Controls)
            {
                if (enabledControls.Contains(ctrl))
                {
                    continue;
                }
                //Attempt to disable the control.
                try
                {
                    ctrl.Enabled = false;
                }
                catch
                {
                }
            }
        }

		public void Restore() {
			if(WindowState==FormWindowState.Minimized) {
				WindowState=_windowStateOld;
			}
		}

		///<summary>Override this if your form needs to do something outside of the ODHelp base functionality.</summary>
		protected virtual void ShowHelp(string name) {
		}

        ///<summary>Base handler for the IODProcessor interface. Wrap it with logging and callback to OnProcess().</summary>
        public void ProcessObjects(List<P> listObjs)
        {
            OnProcessObjects(listObjs);
        }

		///<summary>Override this if your form cares about object processing.</summary>
		public virtual void OnProcessObjects(List<P> listObjs) {
		}

		private void ODFormAbs_Resize(object sender,EventArgs e) {
			if(WindowState!=FormWindowState.Minimized) {
				_windowStateOld=WindowState;
			}
		}

		///<summary>Fires first for all FormClosing events of this form.</summary>
		private void ODFormAbs_FormClosing(object sender,FormClosingEventArgs e) {
			//FormClosed event added to list of closing events as late as possible.
			//This allows the implementing form to set another FormClosing event to be fired before our base event here.
			//The advantage is that HasClosed will only be true if ALL FormClosing events have fired for this form.
			this.FormClosed+=new FormClosedEventHandler(this.ODFormAbs_FormClosed);
		}

		///<summary>Fires last for all FormClosed events of this form.</summary>
		private void ODFormAbs_FormClosed(object sender,FormClosedEventArgs e) {
			DisposeHelpButton();
			_hasClosed=true;
		}

		#region OD Help Logic
		///<summary>Creates or recreates the OD Help button. There are some spots in the program that close or modify all forms.
		///If that is the case the help button may have been affected and needs to be refreshed. Example- FormOpendental log off
		///closes all forms.</summary>
		public void ShowHelpButtonSafe() {
			ODException.SwallowAnyException(ShowHelpButton);
		}

		///<summary>This method creates the help button for all forms (unless turned off at the form level).
		///This is a hard-coded house of cards. If you modify this method you MUST test it on all of our supported 
		///Operating Systems.</summary>
		private void ShowHelpButton() {
			//Due to a Visual Studio bug, LicenseUsageMode.Designtime gives false positives when a form inherits ODFormAbs.
			//The help button is too absolute when drawing itself in "Designtime" and will draw over Visual Studio itself and will stay there even after leaving the design file.
			//Therefore, only execute the Help Menu drawing logic when ODInitialize.Initialize() has been invoked.
			if(!ODInitialize.HasInitialized) {
				return;
			}
			if(!HasHelpKey) {//Some forms might not want the help feature.
				return;
			}
			if(_formHelp!=null) {
				_formHelp.CloseForms();
			}
			double sizeEnhancer=1.3;
			int buttonWidth=(int)(SystemInformation.CaptionButtonSize.Width*sizeEnhancer);
			Rectangle screenRectangle=RectangleToScreen(this.ClientRectangle);
			int height=screenRectangle.Top-this.Top;
			if(this.Menu!=null) {
				//If this hardcoded value stops working we can call win32 methods to get the exact size at a performance cost.
				height-=20;
			}
			Size buttonSize=new Size(buttonWidth,height);
			_formHelp=new FormHelpWrapper(_windowsEdition,buttonSize);
			EventHandler handler=(o1,e1) => {
				var thisRect=this.RectangleToScreen(this.DisplayRectangle);
				//The x coordinate to place our HelpForm. We find this by starting at x 
				//and subtracting the sizes of the various buttons that can be show in the titlebar.
				int x=thisRect.X+thisRect.Width;
				//Subtract ? button width
				x-=buttonSize.Width;
				//Subtract X button width
				x-=buttonSize.Width;
				//If one is present they both are, subtract their buton sizes to find our location.
				if(MaximizeBox || MinimizeBox) {
					x-=(buttonSize.Width)*2;
					x+=GetHelpButtonXLocationAdjustment(true);
				}
				else {
					x+=GetHelpButtonXLocationAdjustment(false);
				}
				if(!_formHelp.IsDisposed) {
					if(this.WindowState==FormWindowState.Maximized) {
						_formHelp.SetLocation(x,this.Location.Y+3);
					}
					else {
						_formHelp.SetLocation(x,this.Location.Y);
					}
					//This block hides the "?" form when it is resized past the parent form's dimensions.
					if(this.Location.X>_formHelp.Location.X) {
						_formHelp.IsVisible=false;
					}
					else if(!_formHelp.IsVisible) {
						ShowHelpButton();
					}
				}
			};
			this.Resize+=handler;
			this.Move+=(o1,e1) => {
				handler(o1,e1);
			};
			_formHelp.LoadHelpUI+=(o1,e1) => {
				handler(o1,e1);
			};
			_formHelp.ClickHelp+=(o1,e1) => {
				ShowHelp(this.Name);
			};
			_formHelp.Show(this);
		}

		///<summary>Disposes and nullifies the forms that are used to comprise the help button.
		///The "help button" is not technically a control that is owned by the parent form thus is not included in the disposal of the parent form.
		///This method needs to get invoked separately so that the references to the help button are removed thus allowing the garbage collector to clean.
		///Without explicitly disposing of these forms the memory used by the parent form cannot be correctly freed thus causing a memory leak.</summary>
		private void DisposeHelpButton() {
			if(_formHelp==null) {
				return;//Nothing to do.
			}
			ODException.SwallowAnyException(() => {
				_formHelp.CloseForms();
				if(!_formHelp.IsDisposed) {
					_formHelp.DisposeForms();
				}
			});
		}

		/// <summary>Returns an int value representing how much to adjust the x position for the ODHelp
		/// button based off of testing with Windows 7, 8, and 10. ComputerInfo().OSFullName returns the 
		/// OS product name (edition). Here is a list of all Window's product names as of 2/21/19
		/// https://en.wikipedia.org/wiki/List_of_Microsoft_Windows_versions </summary>
		private int GetHelpButtonXLocationAdjustment(bool hasMinMaxButtons) {
			if(_windowsEdition.Contains("Windows 7")) {
				return hasMinMaxButtons ? 30 : 10;
			}
			else if(_windowsEdition.Contains("Windows 8")) {
				return hasMinMaxButtons ? 35 : 10;
			}
			else {//Treat everything else as Windows 10
				return 0;
			}
		}
	}

	///<summary>The class that encompasses the two forms used to create the help "button". This class 
	///reduces duplicate code. Simple eventing happens at this level. More complex events are tied to methods
	///after instantiation. If this class is modified please reconsider CreateHelpMenu().</summary>
	public class FormHelpWrapper {
		#region Properties and private data
		private FormHelpUI _formHelpUI;
		private FormHelpEvents _formHelpEvents;

		public bool IsDisposed {
			get {
				return (_formHelpUI==null || _formHelpEvents==null);
			}
		}

		public Size Size {
			set {
				_formHelpUI.Size=value;
				_formHelpEvents.Size=value;
			}
		}
		public Point Location {
			get { 
				return _formHelpUI.Location;
			}
		}
		public bool IsVisible {
			get {
				return (_formHelpUI.Visible || _formHelpEvents.Visible);
			}
			set {
				if(value) {
					_formHelpUI.Visible=true;
					_formHelpEvents.Visible=true;
				}
				else {
					_formHelpUI.Visible=false;
					_formHelpEvents.Visible=false;
				}
			}
		}
		#endregion

		#region Events
		public event EventHandler LoadHelpUI;
		public event EventHandler ClickHelp;
		#endregion

		#region Public methods
		public FormHelpWrapper(string osEdition,Size buttonSize) {
			_formHelpUI=new FormHelpUI(osEdition);// { Size=buttonSize };
			_formHelpEvents=new FormHelpEvents();// { Size=buttonSize };
			_formHelpUI.FormClosed+=(o1,e1) => {
				_formHelpUI=null;
			};
			_formHelpEvents.FormClosed+=(o1,e1) => {
				_formHelpEvents=null;
			};
			//For some reason _formHelpEvents resizes after Show() is called. This keeps it synched with _formHelpUI's size.
			_formHelpEvents.Shown+=(o1,e1) => {
				//_formHelpEvents.Size=_formHelpUI.Size;
				_formHelpEvents.Size=buttonSize;
				_formHelpUI.Size=buttonSize;
			};
			_formHelpUI.Load+=(o1,e1) => {
				LoadHelpUI?.Invoke(this,new EventArgs());
			};
			_formHelpEvents.Click+=(o1,e1) => {
				ClickHelp?.Invoke(this,new EventArgs());
			};
		}

		public void SetLocation(int x, int y) {
			_formHelpUI.Location=new Point(x,y);
			_formHelpEvents.Location=new Point(x,y);
		}

		public void Show(IWin32Window owner) {
			//Order matters here for z-index.
			_formHelpUI.Show(owner);
			_formHelpEvents.Show(owner);
		}

		///<summary>Close forms if they exist.</summary>
		public void CloseForms() {
			if(_formHelpUI!=null) {
				_formHelpUI.Close();
			}
			if(_formHelpEvents!=null) {
				_formHelpEvents.Close();
			}
		}

		///<summary>Disposes forms if they exist and have yet to be disposed.</summary>
		public void DisposeForms() {
			if(_formHelpUI!=null && !_formHelpUI.IsDisposed) {
				_formHelpUI.Dispose();
				_formHelpUI=null;
			}
			if(_formHelpEvents!=null) {
				_formHelpEvents.Dispose();
				_formHelpEvents=null;
			}
		}
		#endregion

		///<summary>HelpForm1 draws the text of the help "button". This form is 100% transparent.
		///This form handles the custom drawing and UI of the help "button".</summary>
		private class FormHelpUI : FormHelpBase {
			public string OsEdition;
			public FormHelpUI(string osEdition) {
				OsEdition=osEdition;
				BackColor=Color.Red;
				TransparencyKey=Color.Red;
				SetStyle(ControlStyles.ResizeRedraw,true);
			}

			protected override void OnPaint(PaintEventArgs e) {
				base.OnPaint(e);
				//Leave this here. Allows drawing on a form that is transparent.
				e.Graphics.TextRenderingHint=System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
				//Rectangle used by all drawing logic for positioning and sizing
				Rectangle rect=new Rectangle(new Point(0, 0), this.Size);
				//Create a smaller rectangle to use for drawing "?"
				rect.Inflate(-(rect.Width/4),-(rect.Height/4));
				//Get adjusted Y value for drawing based on OS
				int yAdjustment=0;
				if(OsEdition.Contains("Windows 7")) {
					yAdjustment+=3;
				}
				else if(OsEdition.Contains("Windows 8")) {
					yAdjustment-=2;
				}
				else {//treat everything else as Windows 10
					yAdjustment+=1;
				}
				using (SolidBrush brush=new SolidBrush(Color.FromArgb(255,80,80,80))) 
				using (Font font=new Font("Segoe UI",10,FontStyle.Bold))
				using(StringFormat format=new StringFormat() { Alignment=StringAlignment.Center, LineAlignment=StringAlignment.Center })
				{		 
						e.Graphics.DrawString("?",font,brush, new Rectangle(rect.X,rect.Y+yAdjustment,rect.Width,rect.Height),format);
				}
			}
		}

		///<summary>HelpForm2 is a blank form set to 99% opacity that lays on top of HelpForm1.
		///This form handles the eventing logic.</summary>
		private class FormHelpEvents : FormHelpBase {

			public bool IsHover {
				get {
					return Opacity==.5;
				}
				set {
					if(value) {
						this.Opacity=.5;
					}
					else {
						this.Opacity=.01;
					}
				}
			}

			public FormHelpEvents() {
				IsHover=false;
				this.MouseEnter+=(o1,e1) => {
					IsHover=true;
				};
				this.MouseLeave+=(o1,e1) => {
					IsHover=false;
				};
			}
		}

		private class FormHelpBase : Form {

			protected override bool ShowWithoutActivation {
				get {
					return true;
				}
			}

			public FormHelpBase() {
				MaximizeBox=false;
				MinimizeBox=false;
				ShowIcon=false;
				ControlBox=false;
				ShowInTaskbar=false;
				AutoSize=false;
				FormBorderStyle=FormBorderStyle.None;
				StartPosition=FormStartPosition.Manual;
			}
		}
	}
		#endregion
}
