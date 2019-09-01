using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using SLDental.Storage;

namespace OpenDental
{
    public class EmailAttachL
    {

        ///<summary>Allow the user to pick the files to be attached. The 'pat' argument can be null. If the user cancels at any step, the return value
        ///will be an empty list.</summary>
        public static List<EmailAttachment> PickAttachments(Patient pat)
        {
            List<EmailAttachment> listAttaches = new List<EmailAttachment>();
            List<string> listFileNames;
            if (pat != null)
            {
                string patFolder = ImageStore.GetPatientFolder(pat);

                listFileNames = Storage.Default.BrowseFile(patFolder, true);
            }
            else
            {
                listFileNames = Storage.Default.BrowseFile("", true);
            }

            try
            {
                for (int i = 0; i < listFileNames.Count; i++)
                {
                    listAttaches.Add(
                        EmailAttachment.CreateAttachment(
                            Path.GetFileName(listFileNames[i]),
                            Storage.Default.ReadAllBytes(listFileNames[i])));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return listAttaches;
        }
    }
}