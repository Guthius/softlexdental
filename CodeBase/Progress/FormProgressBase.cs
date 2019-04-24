using System;
using System.Windows.Forms;
using System.Reflection;

namespace CodeBase
{
    ///<summary>A base window designed to run in a separate thread so that the progress bar can smoothly spin without waiting on the main thread.
    ///Takes care of registering and unregistering for the ODEvent passed into the constructor.
    ///Also takes care of making sure that this window does not get "stuck" open by spawning a thread that monitors if CloseGracefully has been called.
    ///Extending classes are supposed to take care of the desired UI.  Does not extend ODForm on purpose.</summary>
    public class FormProgressBase : Form
    {
        ///<summary>The specific type of ODEvent that should be fired / monitored.  Can be null which will default to a generic ODEvent.</summary>
        private Type _eventType;
        
        ///<summary>The specific type of ODEvent that should be fired / monitored.  Can be null which will default to a generic ODEvent.</summary>
        private ODEventType _odEventType;
        
        ///<summary>An indicator owned by Open Dental indicating that this progress window needs to close regardless if it is done computing or not.
        ///Set to true by the entity that instantiated this progress form to gracefully close when the long computation has finished.</summary>
        public bool ForceClose;
        
        ///<summary>An indicator if this form has closed itself and no longer needs to monitor the ForceClose boolean.</summary>
        private bool _hasClosed;
        
        ///<summary>The "Fired" event that is currently registered to this form.  This is used when registering to custom ODEvents.
        ///It is necessary to keep track of so that we can unregister it when this progress window is closed.</summary>
        private EventInfo _eventInfoFired;

        public FormProgressBase() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormProgressBase"/> class.
        /// </summary>
        /// <param name="odEventType"></param>
        /// <param name="eventType"></param>
        public FormProgressBase(ODEventType odEventType, Type eventType)
        {

            if (eventType == null)
            {
                eventType = typeof(ODEvent);
            }
            _eventType = eventType;
            _odEventType = odEventType;
            //Registers this form for any progress status updates that happen throughout the entire program.
            ODException.SwallowAnyException(() => _eventInfoFired = _eventType.GetEvent("Fired"));
            if (_eventInfoFired == null)
            {
                throw new ApplicationException("The 'eventType' passed into FormProgressStatus does not have a 'Fired' event.\r\n"
                    + "Type passed in: " + eventType.GetType());
            }
            //Register the "Fired" event to the ODEvent_Fired delegate. 
            Delegate delegateFired = Delegate.CreateDelegate(_eventInfoFired.EventHandlerType, this, "ODEvent_Fired");
            MethodInfo methodAddHandler = _eventInfoFired.GetAddMethod();
            methodAddHandler.Invoke(this, new object[] { delegateFired });
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            ODThread threadForceCloseMonitor = new ODThread(100, new ODThread.WorkerDelegate((o) =>
            {
                if (_hasClosed)
                {
                    o.QuitAsync();//Stop monitoring as soon as we detect that this window has been "closed".
                    return;
                }
                if (ForceClose)
                {
                    //Something triggered the fact that this form should have closed.
                    this.InvokeIfRequired(() =>
                    {
                        DialogResult = DialogResult.OK;
                        ODException.SwallowAnyException(() => Close());//Can throw exceptions for many reasons.
                    });
                    o.QuitAsync();//We tried our best to "unstuck" this window.  The user will have to Alt + F4 this window closed?
                    return;
                }
            }));

            threadForceCloseMonitor.Name = "FormProgressStatusMonitor_" + DateTime.Now.Ticks;
            threadForceCloseMonitor.Start();
        }

        ///<summary>Shows a message to the user with this progress window as the owner.</summary>
        public DialogResult MsgBoxShow(string text, string caption = "")
        {
            return MessageBox.Show(this, text, caption);
        }

        ///<summary>Shows a message to the user with this progress window as the owner.</summary>
        public DialogResult MsgBoxShow(string text, string caption, MessageBoxButtons buttons)
        {
            return MessageBox.Show(this, text, caption, buttons);
        }

        ///<summary>Shows a message to the user with this progress window as the owner.</summary>
        public DialogResult MsgBoxShow(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(this, text, caption, buttons, icon);
        }

        public void ODEvent_Fired(ODEventArgs e)
        {
            try
            {
                //We don't know what thread will cause a progress status change, so invoke this method as a delegate if necessary.
                if (InvokeRequired)
                {
                    Invoke((Action)delegate () { ODEvent_Fired(e); });
                    return;
                }
                //If Tag on the ODEvent is null then there is nothing to for this event handler to do.
                if (e.Tag == null)
                {
                    return;
                }
                //Check to see if an ODEventType was set otherwise check the ODEventName to make sure this is an event that this instance cares to process.
                if (_odEventType != ODEventType.Undefined)
                {
                    //Always use ODEventType if one was explicitly set regardless of what _odEventName is set to.
                    if (_odEventType != e.EventType)
                    {
                        return;
                    }
                }
                ProgressBarHelper progHelper = new ProgressBarHelper("");
                bool hasProgHelper = false;
                string status = "";
                if (e.Tag.GetType() == typeof(string))
                {
                    status = ((string)e.Tag);
                }
                else if (e.Tag.GetType() == typeof(ProgressBarHelper))
                {
                    progHelper = (ProgressBarHelper)e.Tag;
                    status = progHelper.LabelValue;
                    hasProgHelper = true;
                }
                else
                {//Unsupported type passed in.
                    return;
                }
                UpdateProgress(status, progHelper, hasProgHelper);
            }
            catch
            {
            }
        }

        ///<summary>Extending classes are required to implement this method.
        ///This class was originally an abstract class which made this fact apparent but Visual Studio's designer doesn't play nicely.</summary>
        public virtual void UpdateProgress(string status, ProgressBarHelper progHelper, bool hasProgHelper)
        {
            throw new NotImplementedException();//STOP!  It is up to extending methods to implement this.  Go away.
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            Delegate fireDelegate = Delegate.CreateDelegate(_eventInfoFired.EventHandlerType, this, "ODEvent_Fired");
            MethodInfo methodRemoveHandler = _eventInfoFired.GetRemoveMethod();
            methodRemoveHandler.Invoke(this, new object[] { fireDelegate });
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            _hasClosed = true;
        }
    }
}