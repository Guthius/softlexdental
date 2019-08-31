using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    /// <summary>
    /// Link to CDS Backup Solutions.
    /// </summary>
    [Obsolete]
    public static class CDS
    {
        public static void ShowPage()
        {
            try
            {
                Process.Start("http://www.opendental.com/resources/redirects/redirectcds.html");
            }
            catch
            {
                MessageBox.Show(
                    "Failed to open web browser. Please make sure you have a default browser set and are connected to the internet then try again.",
                    "CDS",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
        }
    }
}