using CodeBase;
using OpenDentBusiness;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace OpenDental
{
    static class ProgramEntry
    {
        [STAThread]
        static void Main(string[] args)
        {
            var res = Web.Start();

            Application.EnableVisualStyles();

            // Initialize Open Dental.
            try
            {
                // The default SecurityProtocol is "Ssl3|Tls". We must add Tls12 in order to support Tls1.2 web reference 
                // handshakes, without breaking any web references using Ssl3 or Tls.
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                ODInitialize.Initialize();

                Security.CurComputerName = Environment.MachineName;
            }
            catch (Exception ex)
            {
                FormFriendlyException.Show(
                    string.Format("A critical error has occurred: {0}", ex.Message), 
                    ex, "&Quit");

                return;
            }


            //Register an EventHandler which handles unhandled exceptions.
            //AppDomain.CurrentDomain.UnhandledException+=new UnhandledExceptionEventHandler(OnUnhandeledExceptionPolicy);
            bool isSecondInstance = false;//or more.

            Process[] processes = Process.GetProcesses();
            for (int i = 0; i < processes.Length; i++)
            {
                if (processes[i].Id == Process.GetCurrentProcess().Id)
                {
                    continue;
                }
                //we have to do it this way because during debugging, the name has vshost tacked onto the end.
                if (processes[i].ProcessName.StartsWith("OpenDental"))
                {
                    isSecondInstance = true;
                    break;
                }
            }

            Application.SetCompatibleTextRenderingDefault(false);
            Application.DoEvents();
            string[] cla = new string[args.Length];
            args.CopyTo(cla, 0);
            FormOpenDental formOD = new FormOpenDental(cla);
            Exception submittedException = null;
            Action<Exception, string> onUnhandled = new Action<Exception, string>((e, threadName) =>
            {
                //Try to automatically submit a bug report to HQ.
                try
                {
                    //We want to submit a maximum of one exception per instance of OD.
                    if (submittedException == null)
                    {
                        submittedException = e;
                        BugSubmissions.SubmitException(e, threadName, FormOpenDental.CurPatNum, formOD.GetSelectedModuleName());
                    }
                }
                catch (Exception ex)
                {
                    ex.DoNothing();
                }
                FormFriendlyException.Show("Critical Error: " + e.Message, e, "Quit");
                formOD.ProcessKillCommand();
            });
            CodeBase.ODThread.RegisterForUnhandledExceptions(formOD, onUnhandled);
            formOD.IsSecondInstance = isSecondInstance;
            Application.AddMessageFilter(new ODGlobalUserActiveHandler());
            Application.ThreadException += new ThreadExceptionEventHandler((object s, ThreadExceptionEventArgs e) =>
            {
                onUnhandled(e.Exception, "ProgramEntry");
            });
            Application.Run(formOD);
        }
    }
}