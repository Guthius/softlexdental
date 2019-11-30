using CodeBase;
using CodeBase.MVC;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public class ODHelpButtonAttribute : Attribute
    {
        public string FormName { get; set; }
    }

    public class ODHelpEventArgs : EventArgs
    {
        public string FormName { get; set; }

        public bool Handled { get; set; }

        public ODHelpEventArgs(string formName)
        {
            FormName = formName;
        }
    }

    ///<summary>Most OD forms extend this class. It does help and signal processing.</summary>
    public class ODForm : ODFormAbs<Signal>
    {
        string helpFormName;

        ///<summary>List of controls in the form that are used to filter something in the form.</summary>
        private List<Control> _listFilterControls = new List<Control>();
        ///<summary>The given action to run after filter input is commited for FilterCommitMs.</summary>
        private Action _filterAction;
        ///<summary>The thread that is ran to check if filter controls have had their changes commited.
        ///If a single control is considered to have commited changes then the thread will only fire the _filterAction once and then will wait for more input.</summary>
        private ODThread _threadFilter;
        ///<summary></summary>
        private DateTime _timeLastModified = DateTime.MaxValue;
        ///<summary>The number of milliseconds to wait after the last user input on one of the specified filter controls to wait before calling _filterAction.
        ///After some testing, 1 second felt most natural.</summary>
        private int _filterCommitMs = 1000;

        public ODForm()
        {
            StartPosition = FormStartPosition.CenterScreen;

            InitializeHelp();
        }

        protected override void OnShown(EventArgs e)
        {
            Signal.Process += OnProcessSignals;

            CacheManager.CacheRefreshed += OnDataCacheRefresh;

            base.OnShown(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Signal.Process -= OnProcessSignals;

            CacheManager.CacheRefreshed -= OnDataCacheRefresh;

            base.OnFormClosed(e);
        }

        protected virtual void OnDataCacheRefresh(Type type, IDataRecordCache dataRecordCache)
        {
        }

        /// <summary>
        /// Initializes the help button for this form.
        /// </summary>
        void InitializeHelp()
        {
            var attributes = GetType().GetCustomAttributes(typeof(ODHelpButtonAttribute), false);

            if (attributes != null &&
                attributes.Length > 0 &&
                attributes[0] is ODHelpButtonAttribute helpAttribute)
            {
                helpFormName = helpAttribute.FormName;
                HelpButton = true;
            }
        }

        /// <summary>
        /// Open help for the form.
        /// </summary>
        protected override void OnHelpButtonClicked(CancelEventArgs e)
        {
            e.Cancel = true;

            if (string.IsNullOrEmpty(helpFormName))
            {
                helpFormName = Name;
            }

            OnHelp(new ODHelpEventArgs(helpFormName));
        }

        /// <summary>
        /// Raised whenever the user requests help information for the form.
        /// </summary>
        public event EventHandler<ODHelpEventArgs> Help;

        /// <summary>
        /// Raises the <see cref="Help"/> event.
        /// </summary>
        protected virtual void OnHelp(ODHelpEventArgs e)
        {
            Help?.Invoke(this, e);

            if (!e.Handled)
            {
                // TODO: OpenDentalHelp.ODHelp.GetManualPage(e.FormName, Preference.GetString(PreferenceName.ProgramVersion));
            }
        }

        public void SetControlsVisible(bool isVisible, params Control[] arrayControls)
        {
            foreach (Control ctrl in arrayControls)
            {
                ctrl.Visible = isVisible;
            }
        }

        public virtual void OnProcessSignals(IEnumerable<Signal> signals)
        {
        }

        ///<summary>Call before form is Shown. Adds the given controls to the list of filter controls.
        ///We will loop through all the controls in the list to identify the first control that has had its filter change commited for FilterCommitMs.
        ///Once a filter is commited, the filter action will be invoked and the thread will wait for the next filter change to start the thread again.
        ///Controls which are not text-based will commit immediately and will not use a thread (ex checkboxes).</summary>
        ///<param name="filterCommitMs">The number of milliseconds to wait after the last user input on one of the specified filter controls to wait before calling _filterAction.</param>
        protected void SetFilterControlsAndAction(Action action, int filterCommitMs, params Control[] arrayControls)
        {
            SetFilterControlsAndAction(action, arrayControls);
            _filterCommitMs = filterCommitMs;
        }

        ///<summary>Call before form is Shown. Adds the given controls to the list of filter controls.
        ///We will loop through all the controls in the list to identify the first control that has had its filter change commited for FilterCommitMs.
        ///Once a filter is commited, the filter action will be invoked and the thread will wait for the next filter change to start the thread again.
        ///Controls which are not text-based will commit immediately and will not use a thread (ex checkboxes).</summary>
        protected void SetFilterControlsAndAction(Action action, params Control[] arrayControls)
        {
            if (HasShown)
            {
                return;
            }
            _filterAction = action;
            foreach (Control control in arrayControls)
            {
                //Keep the following if/else block in alphabetical order to it is easy to see which controls are supported.
                if (control.GetType().IsSubclassOf(typeof(CheckBox)) || control.GetType() == typeof(CheckBox))
                {
                    CheckBox checkbox = (CheckBox)control;
                    checkbox.CheckedChanged += Control_FilterCommitImmediate;
                }
                else if (control.GetType().IsSubclassOf(typeof(ComboBox)) || control.GetType() == typeof(ComboBox))
                {
                    ComboBox comboBox = (ComboBox)control;
                    comboBox.SelectionChangeCommitted += Control_FilterCommitImmediate;
                }
                else if (control.GetType().IsSubclassOf(typeof(ComboBoxMulti)) || control.GetType() == typeof(ComboBoxMulti))
                {
                    ComboBoxMulti comboBoxMulti = (ComboBoxMulti)control;
                    comboBoxMulti.SelectionChangeCommitted += Control_FilterCommitImmediate;
                }
                else if (control.GetType().IsSubclassOf(typeof(ODDateRangePicker)) || control.GetType() == typeof(ODDateRangePicker))
                {
                    ODDateRangePicker dateRangePicker = (ODDateRangePicker)control;
                    dateRangePicker.CalendarSelectionChanged += Control_FilterCommitImmediate;
                }
                else if (control.GetType().IsSubclassOf(typeof(TextBoxBase)) || control.GetType() == typeof(TextBoxBase))
                {
                    //This includes TextBox and RichTextBox, therefore also includes ODtextBox, ValidNum, ValidNumber, ValidDouble.
                    control.TextChanged += Control_FilterChange;
                }
                else if (control.GetType().IsSubclassOf(typeof(ListBox)) || control.GetType() == typeof(ListBox))
                {
                    control.MouseUp += Control_FilterChange;
                }
                else
                {
                    throw new NotImplementedException("Filter control of type " + control.GetType().Name + " is undefined.  Define it in ODForm.AddFilterControl().");
                }
                _listFilterControls.Add(control);
            }
        }

        ///<summary>Goes through all the controls on the form that implement IValid and shows a message box and returns false if any are not valid.
        ///</summary>
        protected bool ValidateInput()
        {
            if (this.GetAllControls().OfType<IValid>().Any(x => !x.IsValid))
            {
                MessageBox.Show("Please fix data errors first.");
                return false;
            }
            return true;
        }

        ///<summary>Invokes the Save method on all IPrefBinding controls that have AutoSave turned on.  
        ///Returns true if any preference value changed.  Otherwise false.</summary>
        protected bool AutoSave()
        {
            bool hasChanged = false;
            foreach (IPreferenceBinding control in this.GetAllControls().OfType<IPreferenceBinding>())
            {
                if (control.DoAutoSave)
                {
                    hasChanged = control.Save() || hasChanged;
                }
            }
            return hasChanged;
        }

        /// <summary>
        /// A typical try-get, with an additional check to see if the form is disposed or control is disposed.
        /// </summary>
        private bool TryGetFilterInfo(Control control)
        {
            if (Disposing || IsDisposed || control.IsDisposed)
            {
                return false;
            }
            return true;
        }

        ///<summary>Commits the filter action immediately.</summary>
        private void Control_FilterCommitImmediate(object sender, EventArgs e)
        {
            if (!HasShown)
            {
                //Form has not finished the Load(...) function.
                //Odds are the form is initializing a filter in the form load and the TextChanged, CheckChanged, etc fired prematurely.
                return;
            }
            _timeLastModified = DateTime.Now;
            FilterActionCommit();//Immediately commit checkbox changes.
        }

        ///<summary>Commits the filter action according to the delayed interval and input wakeup algorithm which uses FilterCommitMs.</summary>
        private void Control_FilterChange(object sender, EventArgs e)
        {
            if (!HasShown)
            {
                //Form has not finished the Load(...) function.
                //Odds are the form is initializing a filter in the form load and the TextChanged, CheckChanged, etc fired prematurely.
                return;
            }
            Control control = (Control)sender;
            if (!TryGetFilterInfo(control))
            {
                return;
            }
            if (IsDisplosedOrClosed(this))
            {
                //FormClosed even has already occurred.  Can happen if a control in _listFilterControls has a filter action subscribed to an event that occurs after the 
                //FormClosed event, ex CellLeave in FormQueryParser triggers TextBox.TextChanged when closing via shortcut keys (Alt+O).
                return;
            }
            _timeLastModified = DateTime.Now;
            if (_threadFilter == null)
            {//Ignore if we are already running the thread to perform a refresh.
             //The thread does not ever run in a form where the user has not modified the filters.
                #region Init _threadFilter      
                this.FormClosed += new FormClosedEventHandler(this.ODForm_FormClosed); //Wait until closed event so inheritor has a chance to cancel closing event.
                                                                                       //No need to add thread waiting. We will take care of this with FilterCommitMs within our own thread when it runs.
                _threadFilter = new ODThread(1, ((t) => { ThreadCheckFilterChangeCommited(t); }));
                _threadFilter.Name = "ODFormFilterThread_" + Name;
                //Do not add an exception handler here. It would inadvertantly swallow real exceptions as thrown by the Main thread.
                _threadFilter.Start(false);//We will quit the thread ourselves so we can track other variables.
                #endregion
            }
            else
            {
                _threadFilter.Wakeup();
            }
        }

        ///<summary>The thread belonging to Control_FilterChange.</summary>
        private void ThreadCheckFilterChangeCommited(ODThread thread)
        {
            //Might be running after FormClosing()
            foreach (Control control in _listFilterControls)
            {
                if (thread.HasQuit)
                {//In case the thread is executing when the user closes the form and QuitSync() is called in FormClosing().
                    return;
                }
                if (!TryGetFilterInfo(control))
                {//Just in case.
                    continue;
                }
                double diff = (DateTime.Now - _timeLastModified).TotalMilliseconds;
                if (diff <= _filterCommitMs)
                {//Time elapsed is less than specified time.
                    continue;//Check again later.
                }
                FilterActionCommit();
                thread.Wait(int.MaxValue);//Wait forever... or until Control_FilterChange(...) wakes the thread up or until form is closed.
                break;//Do not check other controls since we just called the filters action.
            }
        }

        private void FilterActionCommit()
        {
            Exception ex = null;
            //Synchronously invoke the "Refresh"/filter action function for the form on the main thread and invoke to prevent thread access violation exceptions.
            this.Invoke(() =>
            {
                //Only invoke if action handler has been set.
                try
                {
                    _filterAction?.Invoke();
                }
                catch (Exception e)
                {
                    //Simply throwing here would replace the stack trace with this thread's stack. 
                    //Provide this exception as the inner exception below once we are out of the main thread's invoke to preserve both.
                    ex = e;
                }
            });

            if (ex != null) throw ex;
        }

        private void ODForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_threadFilter != null)
            {
                _threadFilter.QuitAsync();//It's fine if our thread loop finishes, it protects against unhandled exceptions.
                _threadFilter = null;
                //We don't want an enumeration exception here so don't clear _listFilterControls. It will get garbage collected anyways.
            }
        }
    }
}