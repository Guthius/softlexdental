using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CodeBase.MVC
{
    /// <summary
    /// >Base class that all forms should extend.  Provides accessibility to features for all forms like Help and object processing.
    /// </summary>
    /// <typeparam name="TProcessor">Processor Type - Typically set to Signalod but can be set to any object type that gets processed by the form.</typeparam>
    public class ODFormAbs<TProcessor> : Form, IODProcessor<TProcessor>
    {
        FormWindowState previousWindowState;

        /// <summary>
        /// Returns a value indicating whether the form has been shown.
        /// </summary>
        public bool HasShown { get; private set; } = false;

        /// <summary>
        /// Returns a value indicating whether the form has been closed.
        /// </summary>
        public bool HasClosed { get; private set; } = false;

        /// <summary>
        /// Returns true if the form passed in has been disposed or if it extends ODFormAbs and HasClosed is true.
        /// </summary>
        public static bool IsDisplosedOrClosed(Form form)
        {
            bool isClosed = false;

            if (form.IsDisposed) isClosed = true;
            else if (form.GetType().GetProperty("HasClosed") != null)
            {
                if ((bool)form.GetType().GetProperty("HasClosed").GetValue(form))
                {
                    isClosed = true;
                }
            }

            return isClosed;
        }

        protected override void OnShown(EventArgs e)
        {
            HasShown = true;//Occurs after Load(...)
                             //This form has just invoked the "Shown" event which probably means it is important and needs to actually show to the user.
                             //There are times in the application that a progress window (e.g. splash screen) will be showing to the user and a new form is trying to show.
                             //Therefore, forcefully invoke "Activate" if there is a progress window currently on the screen.
                             //Invoking Activate will cause the new form to show above the progress window (if TopMost=false) even though it is in another thread.
            if (ODProgress.FormProgressActive != null)
            {
                this.Activate();
            }
            base.OnShown(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
            {
                previousWindowState = WindowState;
            }
            base.OnResize(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            HasClosed = true;
        }

        /// <summary>
        /// Sets the entire form into "read only" mode by disabling all controls on the form.
        /// Pass in any controls that should say enabled (e.g. Cancel button). 
        /// This can be used to stop users from clicking items they do not have permission for.
        /// </summary>
        public void DisableForm(params Control[] enabledControls)
        {
            foreach (Control control in Controls)
            {
                if (enabledControls.Contains(control)) continue;

                try
                {
                    control.Enabled = false;
                }
                catch { }
            }
        }

        public void Restore()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = previousWindowState;
            }
        }

        /// <summary>
        /// Base handler for the IODProcessor interface. Wrap it with logging and callback to OnProcess().
        /// </summary>
        public void ProcessObjects(List<TProcessor> listObjs)
        {
            OnProcessObjects(listObjs);
        }

        /// <summary>
        /// Override this if your form cares about object processing.
        /// </summary>
        public virtual void OnProcessObjects(List<TProcessor> listObjs)
        {
        }
    }
}