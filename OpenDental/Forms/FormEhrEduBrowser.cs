using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental
{
    public partial class FormEhrEduBrowser : ODForm
    {
        public string ResourceURL;
        public bool DidPrint;

        public FormEhrEduBrowser(string resourceURL)
        {
            ResourceURL = resourceURL;
            InitializeComponent();
        }

        void FormEduBrowser_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                webBrowser1.Url = new Uri(ResourceURL);
            }
            catch (UriFormatException)
            {
                MessageBox.Show(
                    "The specified URL is in an incorrect format.  Did you include the http:// ?",
                    "", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                DialogResult = DialogResult.Cancel;
            }

            Cursor = Cursors.Default;
        }

        void PrintButton_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintDialog();

            DidPrint = true;
        }
    }
}