using System;
using System.IO;
using System.Windows.Forms;
using UpdateFileCopier.Properties;

namespace UpdateFileCopier
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] arguments)
        {
            // Get the source path.
            string sourcePath = arguments.Length > 0 ? arguments[0] : @"C:\OpenDentImages\UpdateFiles";
            if (!Directory.Exists(sourcePath))
            {
                return;
            }

            // The 2nd argument (arguments[1]) is reserved. This is used by Open Dental to pass the Open Dental process ID.
            // As far as I am aware this isn't actually used anywhere... To make sure this version of the file copier remains
            // fully compatible with Open Dental 19.1 we simply skip past this argument for now.

            // Get the destination path.
            string destPath = arguments.Length > 2 ? arguments[2] : @"C:\Program Files\Open Dental";
            if (!Directory.Exists(destPath))
            {
                try
                {
                    Directory.CreateDirectory(destPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message, 
                        Resources.LangUpdateFileCopier, 
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }

            // Should we kill all services related to Open Dental?
            bool killServices = true;
            if (arguments.Length > 3)
            {
                if (bool.TryParse(arguments[3], out killServices))
                {
                    killServices = true;
                }
            }

            // Should we launch Open Dental after copy?
            bool launchOpenDental = true;
            if (arguments.Length > 4)
            {
                if (bool.TryParse(arguments[4], out launchOpenDental))
                {
                    launchOpenDental = true;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain(sourcePath, destPath, killServices, launchOpenDental));
        }
    }
}