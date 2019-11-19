using OpenDentBusiness;
using SLDental.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace OpenDental
{
    public class EmailAttachL
    {
        /// <summary>
        ///     <para>
        ///         Allow the user to pick the files to be attached. The <paramref name="patient"/>
        ///         parameter may be null. If the user cancels at any step, the return value will 
        ///         be an empty list.
        ///     </para>
        /// </summary>
        public static List<EmailAttachment> PickAttachments(Patient patient)
        {
            var attachments = new List<EmailAttachment>();

            List<string> attachmentFileNames;
            if (patient != null)
            {
                string patFolder = ImageStore.GetPatientFolder(patient);

                attachmentFileNames = Storage.Default.BrowseFile(patFolder, true);
            }
            else
            {
                attachmentFileNames = Storage.Default.BrowseFile("", true);
            }

            try
            {
                for (int i = 0; i < attachmentFileNames.Count; i++)
                {
                    attachments.Add(
                        EmailAttachment.CreateAttachment(
                            Path.GetFileName(attachmentFileNames[i]),
                            Storage.Default.ReadAllBytes(attachmentFileNames[i])));
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    "Attachments", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return attachments;
        }
    }
}
