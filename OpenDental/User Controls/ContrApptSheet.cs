/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using CodeBase;
using OpenDentBusiness.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public class ContrApptSheet : UserControl
    {
        /// <summary>
        /// Protects multi-threaded code in RedrawAll(). Non-static is ok here because we only 
        /// care about locking per instance of this ContrApptSheet. There is only 1 instance in the
        /// entire app anyways so it doesn't really matter.
        /// </summary>
        private readonly object redrawAllLock = new object();

        /// <summary>
        ///     <para>
        ///         Used for double buffering. We will do all the heavy lifting drawing to this 
        ///         bitmap. Then we will draw this bitmap to the screen.
        ///     </para>
        ///     <para>
        ///         Also used in <see cref="OnPaint(PaintEventArgs)"/> to redraw what has already 
        ///         been drawn.
        ///     </para>
        /// </summary>
        private Bitmap backBuffer;

        public bool IsScrolling = false;

        /// <summary>
        /// Protects DoubleBufferDraw() from main thread re-entrance when we threaded drawing is in process. 
        /// Any main thread draw requests (like OnPaint, MouseUp/Down, etc) will be rejected in the event that 
        /// a threaded draw event is already in process.
        /// </summary>
        private bool isRedrawingOnThread = false;

        /// <summary>
        /// Used to store the order of any number of overlapping appointments.
        /// </summary>
        public ApptOverlapOrdering OverlapOrdering = new ApptOverlapOrdering();

        /// <summary>
        ///     <para>
        ///         Return true if <see cref="backBuffer"/> is non-null. Otherwise returns false. Check
        ///         this before using <see cref="backBuffer"/>.
        ///     </para>
        /// </summary>
        private bool IsBackBufferValid => backBuffer != null;

        /// <summary>
        ///     <para>
        ///         Gets the <see cref="ContrApptSingle"/> at the specified 
        ///         <paramref name="index"/>; or null if <paramref name="index"/> is out of range.
        ///     </para>
        /// </summary>
        public ContrApptSingle this[int index]
        {
            get
            {
                if (index < 0 || index >= Controls.Count)
                {
                    return null;
                }
                return (ContrApptSingle)Controls[index];
            }
        }

        /// <summary>
        /// Includes each AptNum which belongs to each ContrApptSingle which has been added to .Controls.
        /// </summary>
        public List<long> ListAptNums => ListContrApptSingles.Select(x => x.AptNum).ToList();

        /// <summary>
        /// Includes each Control that has been added to .Controls. They must all be of type ContrApptSingle.
        /// </summary>
        public List<ContrApptSingle> ListContrApptSingles => Controls.Cast<ContrApptSingle>().ToList();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeBackBuffer();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Simply redraw the double buffer bitmap which we already have stored.
        /// </summary>
        protected override void OnPaint(PaintEventArgs pea)
        {
            DoubleBufferDraw(
                // This does nothing here but just for legibility.
                drawToScreen: true,

                // Skip all double buffer bitmap modification and just draw the existing to the screen.
                drawCachedBitmapOnly: true);
        }

        private void DisposeBackBuffer()
        {
            if (IsBackBufferValid)
            {
                backBuffer.Dispose();
                backBuffer = null;
            }
        }

        /// <summary>
        /// First disposes of main ContrApptSheet shadow. Then removes all  ContrApptSingle controls and disposes of their shadows.
        /// </summary>
        public void DisposeAppointments()
        {
            DisposeBackBuffer();

            // Loop backwards to avoid missing some.
            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                Controls[i].Dispose();
            }

            Controls.Clear();
        }

        /// <summary>
        /// Returns a copy of the current double buffered bitmap with the given time constraints. Throws exceptions.
        /// </summary>
        public Bitmap CloneBackBuffer(DateTime startTime, DateTime stopTime)
        {
            if (!IsBackBufferValid) return null;
            
            int x = 0;
            int y = ApptDrawing.LineH * ApptDrawing.RowsPerHr * startTime.Hour;
            int w = backBuffer.Width;
            int h = ApptDrawing.LineH * ApptDrawing.RowsPerHr * (stopTime.Hour - startTime.Hour + 1);

            // Clones image and sets size to only show the time open for that day. (Needs to be rewritten)
            return backBuffer.Clone(new Rectangle(x, y, w, h), PixelFormat.DontCare);  
        }

        /// <summary>
        ///     <para>
        ///         Wipes out existing double buffer bitmap and re-creates it before performing 
        ///         the typical behavior of 
        ///         <see cref="DoubleBufferDraw(List{long}, bool, bool, bool, bool, bool, long)"/>.
        ///         The appointments module is drawn and rendered on a helper thread.
        ///     </para>
        /// </summary>
        public void RedrawAll()
        {
            // Save local copies to be used below.
            int height = ApptDrawing.LineH * 24 * ApptDrawing.RowsPerHr;
            int width = (int)(ApptDrawing.TimeWidth * 2 + ApptDrawing.ProvWidth * ApptDrawing.ProvCount + ApptDrawing.ColWidth * ApptDrawing.ColCount);
            
            // We will either thread this or not below. Same code either way.
            var actionRedraw = new Action(() =>
            {
                // We are about to invalidate the doubule buffer bitmap. 
                // Prevent re-entrance. Any subsequent entrants will need to wait for each predecessor to complete before attempting to draw.
                // This has the effect of last-in-wins as far as the UI is concerned.
                lock (redrawAllLock)
                {
                    try
                    {
                        isRedrawingOnThread = true;

                        // Dismantle the previous double buffer bitmap.
                        DisposeBackBuffer();
                        if (width < 2)
                        {
                            return;
                        }

                        // Create the new double buffer bitmap.
                        backBuffer = new Bitmap(width, height);
                        if (ApptDrawing.RowsPerIncr == 0)
                        {
                            ApptDrawing.RowsPerIncr = 1;
                        }
                        ApptDrawing.ApptSheetWidth = width;
                        ApptDrawing.ApptSheetHeight = height;

                        // It is now safe to draw all of our bitmaps to the screen.
                        DoubleBufferDraw(drawSheetBackground: true, createApptShadows: true, drawToScreen: true, isThreaded: true);

                        // We made it this far so it is now safe to set the Size of the control itself.
                        Invoke(new Action(() =>
                        { //Now that we are back on the main thread, it is safe to set control properties.
                            Height = height;
                            Width = width;
                        }));
                    }
                    catch
                    {
                        //ApptDrawing uses a lot of statics that are being set outside of this thread. 
                        //Expect those to throw exceptions when they are being modified by the main thread while this worker thread is still drawing.				
                        //Also, in rare cases (changing modules during a redraw for instance) our double buffer bitmap will be disposed by the main thread right out from under this thread.
                        //All of these cases happen when the thread we are working on has become invalid anyways. For this reason it is safe to expect and swallow these errors.
                        //The last in thread will always have current and correct statics. Last in wins!
                    }
                    finally
                    {
                        isRedrawingOnThread = false;
                    }
                }
            });

            new ODThread(new ODThread.WorkerDelegate((o) => { actionRedraw(); })).Start();
        }

        private void DrawBackBufferToScreen()
        {
            if (!IsBackBufferValid) return;

            using (var graphics = CreateGraphics())
            {
                try
                {
                    graphics.DrawImage(backBuffer, 0, 0);
                }
                catch
                {
                    //Out of memory exceptions have been occurring for some of our large customers.  See job #9459, task #1570555.
                    //Online research suggests that out of memory exceptions are a more generic exception which could happen for a variety of reasons.
                    //This issue could be due to the shadow bitmap being small enough to fit in system memory but too small to fit in graphcis memory.
                    //When the system sends the bitmap data from the system memory into the graphics memory, the out of memory exception is thrown.
                    //The shadow bitmap is a double buffering mechanism.  We might be able to fix this out of memory issue in the future,
                    //if we instead use the Microsoft flag ContrApptSheet.DoubleBuffered and draw directly onto the graphics device provided in OnPaint().
                    //For now, when this exception occurs, the shadow will not be drawn and will cause the appointment book to show a red X.
                    DisposeBackBuffer();//Invalidate the shadow so that it might be regenerated correctly next time.
                }
            }
        }

        /// <summary>
        /// Handles all double buffer drawing and rendering to screen.
        /// Creates one bitmap image for each appointment if visible, and draws those bitmaps onto the main appt background image.
        /// </summary>
        /// <param name="listAptNumsOnlyRedraw">Specify which appts to redraw. If null or empty then all appts will be redrawn.</param>
        /// <param name="drawSheetBackground">Recreates the background and everything other than the appointments. Typically only used by RedrawAll().</param>
        /// <param name="createApptShadows">Each individual child ContrApptSingle control will have it's own double buffer bitmap re-created.</param>
        /// <param name="drawToScreen">Draws the double buffer bitmap directly to screen once it has been modified. In rare cases the screen has already been updated so this wouldn't be necessary here.</param>
        /// <param name="isThreaded">Spreads the work of generating each appt shadow over multiple threads. Typically only used by RedrawAll().</param>
        /// <param name="drawCachedBitmapOnly">Skips all double buffer bitmap modifications and simply redraws existing to screen. Typically only used by OnPaint().</param>
        /// <param name="selectedAptNum">Pass to draw the selected appointment always in front. Only time it is optional, is when drawCachedBitmapOnly is true or when clearing the previous selection.</param>
        public void DoubleBufferDraw(List<long> listAptNumsOnlyRedraw = null,
            bool drawSheetBackground = false, bool createApptShadows = false, bool drawToScreen = false, bool isThreaded = false,
            bool drawCachedBitmapOnly = false, long selectedAptNum = -1)
        {
            if (isRedrawingOnThread && !isThreaded)
            { //We are already performing a RedrawAll on a thread so do not allow re-entrance at this time.
                return;
            }

            if (!IsBackBufferValid)
            {//if user resizes window to be very narrow
                ODException.SwallowAnyException(() =>
                {
                    //Draw a large red X accross the entire sheet to indicate an error has occurred.
                    using (Graphics g = this.CreateGraphics())
                    {
                        g.DrawLine(Pens.Red, new Point(0, 0), new Point(Width, Height));
                        g.DrawLine(Pens.Red, new Point(0, Height), new Point(Width, 0));
                    }
                });//If this fails, the user will see a blank (cleared to default control color) appointment book.
                return;
            }

            if (drawCachedBitmapOnly)
            {
                DrawBackBufferToScreen();
                return;
            }

            if (createApptShadows)
            {
                //Make a list of actions. We will process these in threads below.
                var actions = new List<Action>();
                foreach (ContrApptSingle ctrl in ListContrApptSingles)
                {
                    if (listAptNumsOnlyRedraw != null && !listAptNumsOnlyRedraw.Contains(ctrl.AptNum) && !OverlapOrdering.IsOverlappingAppt(ctrl.AptNum))
                    {
                        continue;
                    }

                    actions.Add(new Action(() =>
                    {
                        Bitmap shadow = ctrl.CreateShadow();
                        shadow?.Dispose();
                    }));
                }



                if (isThreaded)
                {//Spread the workload over a group of threads.
                    ODThread.RunParallel(actions, TimeSpan.FromMinutes(1));
                }
                else
                { //Syncronous.
                    actions.ForEach(x => x());
                }
            }

            using (var graphics = Graphics.FromImage(backBuffer))
            {
                if (drawSheetBackground) //Draw background first.	
                {
                    ApptDrawing.DrawAllButAppts(graphics, true, 
                        new DateTime(2011, 1, 1, 0, 0, 0), 
                        new DateTime(2011, 1, 1, 0, 0, 0), 
                        ApptDrawing.VisOps.Count, 0, 8, 
                        false);
                }

                var sortedAppointmentsList = SortAppointmentsForOverlap(ListContrApptSingles, selectedAptNum);
                foreach (var appointmentControl in sortedAppointmentsList)
                {
                    // Filter based on AptNum where applicable.
                    // If the appoinment is in overlapOrders, draw again anyway to ensure the order.
                    if (listAptNumsOnlyRedraw != null && !listAptNumsOnlyRedraw.Contains(appointmentControl.AptNum) && !OverlapOrdering.IsOverlappingAppt(appointmentControl.AptNum))
                    {
                        continue;
                    }

                    // Make sure that appointment shadow was created one way or another.
                    if (!appointmentControl.IsShadowValid) continue;

                    using (var appointmentBitmap = appointmentControl.Shadow)
                    {
                        if (appointmentBitmap == null) continue;

                        if (appointmentControl.Location.X >= ApptDrawing.TimeWidth + (ApptDrawing.ProvWidth * ApptDrawing.ProvCount) && appointmentControl.Width > 3)
                        {
                            graphics.DrawImage(appointmentBitmap, appointmentControl.Location.X, appointmentControl.Location.Y);
                        }
                    }
                }
            }

            if (drawToScreen) DrawBackBufferToScreen();
        }

        /// <summary>
        ///     <para>
        ///         This method sorts the list of appointments to be drawn before hand. This takes 
        ///         into account the order of overlapping appiontments.
        ///     </para>
        /// </summary>
        private List<ContrApptSingle> SortAppointmentsForOverlap(List<ContrApptSingle> appointments, long selectedAptNum)
        {
            var sortedAppointmentsList = new List<ContrApptSingle>();
            foreach (var appointment in appointments)
            {
                // if added by a previous ordered ctrl.
                if (sortedAppointmentsList.Any(x => x.AptNum == appointment.AptNum)) continue;

                if (!OverlapOrdering.IsOverlappingAppt(appointment.AptNum))
                {
                    sortedAppointmentsList.Add(appointment); //if they are not ordered, the order is insignificant as no overlap
                    continue;
                }

                // Otherwise, it is a special order. Pull all appointments in the List and insert them into the listApptsSorted in the correct order
                List<long> listOverlapAptNums = OverlapOrdering.GetApptNumsInSet(appointment.AptNum);
                ContrApptSingle selectedCtrl = null;
                if (selectedAptNum > 0 && listOverlapAptNums.Contains(selectedAptNum))
                {//always draw selected appointment on top
                    selectedCtrl = appointments.Find(x => x.AptNum == selectedAptNum);
                    listOverlapAptNums.Remove(selectedAptNum);
                }

                foreach (long aptNum in listOverlapAptNums)
                {//already in order to add by GetApptNumsInSet above
                    sortedAppointmentsList.Add(appointments.Find(x => x.AptNum == aptNum));
                }

                if (selectedCtrl != null)
                {//insert here as the selected appt should be on top
                    sortedAppointmentsList.Add(selectedCtrl);
                }
            }
            return sortedAppointmentsList;
        }
    }
}
