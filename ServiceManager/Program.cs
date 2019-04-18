using System;
using System.Windows.Forms;

namespace ServiceManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args != null && args.Length > 0 && args[0] != null)
            {
                Application.Run(new FormServiceManage(args[0], false));
            }
            else
            {
                Application.Run(new FormMain());
            }
        }
    }
}