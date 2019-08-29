using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental
{
    public class EmailAttachL
    {

        ///<summary>Allow the user to pick the files to be attached. The 'pat' argument can be null. If the user cancels at any step, the return value
        ///will be an empty list.</summary>
        public static List<EmailAttachment> PickAttachments(Patient pat)
        {
            List<EmailAttachment> listAttaches = new List<EmailAttachment>();
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            bool isLocalFileSelected = false;
            List<string> listFileNames;
            if (pat != null && Preferences.AtoZfolderUsed != DataStorageType.InDatabase)
            {
                string patFolder = ImageStore.GetPatientFolder(pat, ImageStore.GetPreferredAtoZpath());
                if (CloudStorage.IsCloudStorage)
                {
                    FormFilePicker FormFP = new FormFilePicker(patFolder);
                    if (FormFP.ShowDialog() != DialogResult.OK)
                    {
                        return listAttaches;
                    }
                    isLocalFileSelected = FormFP.WasLocalFileSelected;
                    listFileNames = FormFP.SelectedFiles;
                }
                else
                {
                    dlg.InitialDirectory = patFolder;
                    if (dlg.ShowDialog() != DialogResult.OK)
                    {
                        return listAttaches;
                    }
                    isLocalFileSelected = true;
                    listFileNames = dlg.FileNames.ToList();
                }
            }
            else
            {//No patient selected or images in database
             //Use the OS default directory for this type of file viewer.
                dlg.InitialDirectory = "";
                if (dlg.ShowDialog() != DialogResult.OK)
                {
                    return listAttaches;
                }
                isLocalFileSelected = true;
                listFileNames = dlg.FileNames.ToList();
            }
            try
            {
                for (int i = 0; i < listFileNames.Count; i++)
                {
                    if (CloudStorage.IsCloudStorage)
                    {
                        FileAtoZSourceDestination sourceDestination;
                        if (isLocalFileSelected)
                        {
                            sourceDestination = FileAtoZSourceDestination.LocalToAtoZ;
                        }
                        else
                        {
                            sourceDestination = FileAtoZSourceDestination.AtoZToAtoZ;
                        }
                        //Create EmailAttach using EmailAttaches.CreateAttach logic, shortened for our specific purpose.
                        EmailAttachment emailAttach = new EmailAttachment();
                        emailAttach.Description = Path.GetFileName(listFileNames[i]);
                        string attachDir = EmailAttachment.GetAttachmentPath();
                        string subDir = "Out";
                        emailAttach.FileName = ODFileUtils.CombinePaths(subDir,
                            DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.TimeOfDay.Ticks.ToString()
                                + "_" + MiscUtils.CreateRandomAlphaNumericString(4) + "_" + emailAttach.Description).Replace("\\", "/");
                        FileAtoZ.Copy(listFileNames[i], FileAtoZ.CombinePaths(attachDir, emailAttach.FileName), sourceDestination);
                        listAttaches.Add(emailAttach);
                    }
                    else
                    {//Not cloud
                        listAttaches.Add(EmailAttachment.CreateAttachment(Path.GetFileName(listFileNames[i]), File.ReadAllBytes(listFileNames[i])));
                    }
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