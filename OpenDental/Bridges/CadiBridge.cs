/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using OpenDentBusiness;
using OpenDentBusiness.Bridges;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    /// <summary>
    /// CADI uses their own OLE format for passing data to CADI. 
    /// See the bridging documents in \\opendental.od\serverfiles\Storage\OPEN DENTAL\Programmers Documents\Bridge Info\CADI for details.
    /// </summary>
    public class CadiBridge : Bridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Define("image_path", "Image Folder", BridgePreferenceType.Folder)
        };

        /// <summary>
        /// Keep as a static object so it doesn't get garbage collected.
        /// </summary>
        private static CadiNativeWindow CADIWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="CadiBridge"/> class.
        /// </summary>
        public CadiBridge() : base("CADI", "", "https://www.cadi.net/", preferences)
        {
        }

        /// <summary>
        ///     <para>
        ///         Sends the specified <paramref name="patient"/> data to the remote program or 
        ///         service.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient details.</param>
        public override void Send(long programId, Patient patient)
        {
            try
            {
                if (CADIWindow == null)
                { 
                    CADIWindow = new CadiNativeWindow(() => CADIWindow = null);
                }

                CADIWindow.Send(programId, patient);
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message, 
                    "CADI", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                SecurityLog.Write(patient?.PatNum, SecurityLogEvents.ModuleChart, exception.Message);
            }
        }

        /// <summary>
        ///     <para>
        ///         Contains the logic for opening the CADI application, sending OLE commands to 
        ///         it, and listening to messages.
        ///     </para>
        /// </summary>
        private class CadiNativeWindow : NativeWindow
        {
            /// <summary>
            /// Per bridging documents, this is the programID used to identify CADI.
            /// </summary>
            private const string PROGRAM_ID = "Mediadent.Application";

            /// <summary>
            /// Per bridging documents, this is the value of the sysuint we expect back when the 
            /// CADI window is closed by the user.
            /// </summary>
            private const int OLECLIENTNOTIFY_APPLICATIONEXIT = 1;

            /// <summary>
            /// Per bridging documents, birthdate is specified using COM TDATE, which is calculated 
            /// as the number of days that have elapsed since 12/30/1899.
            /// </summary>
            private static readonly DateTime TDATE_START = new DateTime(1899, 12, 30);

            private readonly Action closeAction = null;
            private dynamic comObject;
            private int windowHandle;

            /// <summary>
            /// Initializes a new instance of the <see cref="CadiNativeWindow"/> class.
            /// </summary>
            /// <param name="closeAction">The action to perform when the window is closed.</param>
            public CadiNativeWindow(Action closeAction)
            {
                this.closeAction = closeAction;

                CreateHandle(new CreateParams());
            }

            /// <summary>
            ///     <para>
            ///         Launches the program as a COM object, and passes CADI OLE commands to the 
            ///         program.
            ///     </para>
            /// </summary>
            public void Send(long programId, Patient patient)
            {
                const uint WM_CLOSE = 0x0010;

                try
                {
                    if (comObject == null)
                    {
                        var programType = Type.GetTypeFromProgID(PROGRAM_ID);
                        if (programType == null)
                        {
                            return;
                        }

                        comObject = Activator.CreateInstance(programType);

                        // Register the handle of this window with CADI to listen for application closing events.
                        int result = comObject.OleClientWindowHandle(windowHandle, OLECLIENTNOTIFY_APPLICATIONEXIT, WM_CLOSE, 0, 0);
                        if (result != 0) // 0=success; 14=Invalid argument; -1=Unspecified error.
                        {
                            throw new Exception("Unable to communicate with CADI. Verify it is installed and try again.");
                        }
                    }

                    if (patient == null) return;

                    string imagePath = ProgramPreference.GetString(programId, "image_path");

                    // Set the patient information.
                    comObject.OleBringToFrontApp();
                    comObject.OleBeginTransaction();
                    comObject.OleSetPatientName(patient.LName + ", " + patient.FName);
                    comObject.OleSetPatientDateOfBirth(patient.Birthdate.Subtract(TDATE_START).TotalDays);
                    comObject.OleSetPatientBiologicalAge(patient.Age);
                    comObject.OleSetPatientSex(patient.Gender == PatientGender.Female ? "F" : "M");
                    comObject.OleLoadPicLib(Path.Combine(imagePath, patient.PatNum.ToString()));
                    comObject.OleEndTransaction();
                }
                catch (COMException)
                {
                    comObject = null;

                    CloseWindow();

                    throw new Exception("Unable to access CADI. Verify it is installed and try again.");
                }
                catch (Exception exception)
                {
                    bool throwFriendly = comObject == null;

                    CloseWindow();

                    if (throwFriendly)
                    {
                        throw new Exception("Unable to open CADI. Verify it is installed and try again.");
                    }

                    throw exception;
                }
            }

            /// <summary>
            ///     <para>
            ///         Release <see cref="comObject"/> and inform the owner window that is can release
            ///         it's reference to this class instance.
            ///     </para>
            /// </summary>
            private void CloseWindow()
            {
                try
                {
                    ReleaseHandle();
                }
                catch { }

                try
                {
                    if (comObject != null)
                    {
                        Marshal.ReleaseComObject(comObject);

                        comObject = null;
                    }

                    closeAction?.Invoke();
                }
                catch { }
            }

            /// <summary>
            /// Listen to when the handle changes to keep the variable in sync
            /// See https://msdn.microsoft.com/en-us/library/system.windows.forms.nativewindow.handle(v=vs.110).aspx
            /// </summary>
            [PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
            protected override void OnHandleChange() => windowHandle = (int)Handle;

            /// <summary>
            /// Listen for messages being sent back to this handle by CADI. We need to include this to allow the CADI window to close properly.
            /// See https://msdn.microsoft.com/en-us/library/system.windows.forms.nativewindow.handle(v=vs.110).aspx
            /// </summary>
            [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
            protected override void WndProc(ref Message m)
            {
                const uint WM_CLOSE = 0x0010;

                switch ((uint)m.Msg)
                {
                    case WM_CLOSE: // Note: if in demo mode and user clicks 'Bye', this event won't be raised. This will fix itself if they open the bridge again.
                        CloseWindow();
                        break;
                }

                base.WndProc(ref m);
            }
        }
    }
}
