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
            Application.EnableVisualStyles();

            try
            {
                // The default SecurityProtocol is "Ssl3|Tls". We must add Tls12 in order to support Tls1.2 web reference 
                // handshakes, without breaking any web references using Ssl3 or Tls.
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                ODInitialize.Initialize();

                Security.CurComputerName = Environment.MachineName;
            }
            catch (Exception exception)
            {
                FormFriendlyException.Show(
                    string.Format("A critical error has occurred: {0}", exception.Message), 
                    exception, "&Quit");

                return;
            }

            DataConnection.Configure("localhost", "demo", "root", "softlex");

            //Register an EventHandler which handles unhandled exceptions.
            //AppDomain.CurrentDomain.UnhandledException+=new UnhandledExceptionEventHandler(OnUnhandeledExceptionPolicy);
            bool isSecondInstance = false;

            var processes = Process.GetProcesses();
            for (int i = 0; i < processes.Length; i++)
            {
                if (processes[i].Id == Process.GetCurrentProcess().Id) continue;

                // We have to do it this way because during debugging, the name has vshost tacked onto the end.
                if (processes[i].ProcessName.StartsWith("OpenDental"))
                {
                    isSecondInstance = true;
                    break;
                }
            }

            Application.SetCompatibleTextRenderingDefault(false);
            Application.AddMessageFilter(new ODGlobalUserActiveHandler());
            Application.DoEvents();

            var formOpenDental = new FormOpenDental(args);

            Application.ThreadException += (s, e) =>
            {
                FormFriendlyException.Show(
                    "Critical Error: " + e.Exception.Message, 
                    e.Exception, "Quit");

                formOpenDental.ProcessKillCommand();
            };

            formOpenDental.IsSecondInstance = isSecondInstance;

            Application.Run(formOpenDental);
        }
    }
}