using SLDental.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace OpenDentBusiness.Eclaims
{
    public class x837Controller
    {
        ///<summary>Gets the filename for this batch. Used when saving or when rolling back.</summary>
        private static string GetFileName(Clearinghouse clearinghouseClin, int batchNum, bool isAutomatic)
        { 
            //called from this.SendBatch. Clinic-level clearinghouse passed in.
            string saveFolder = clearinghouseClin.ExportPath;
            if (!Directory.Exists(saveFolder))
            {
                if (!isAutomatic)
                {
                    MessageBox.Show(saveFolder + " not found.");
                }
                return "";
            }
            if (clearinghouseClin.CommBridge == EclaimsCommBridge.RECS)
            {
                if (File.Exists(Storage.Default.CombinePath(saveFolder, "ecs.txt")))
                {
                    if (!isAutomatic)
                    {
                        MessageBox.Show("You must send your existing claims from the RECS program before you can create another batch.");
                    }
                    return "";//prevents overwriting an existing ecs.txt.
                }
                return Path.Combine(saveFolder, "ecs.txt");
            }
            else
            {
                return Path.Combine(saveFolder, "claims" + batchNum.ToString() + ".txt");
            }
        }

        ///<summary>If file creation was successful but communications failed, then this deletes the X12 file.  This is not used in the Tesia bridge because of the unique filenaming.</summary>
        public static void Rollback(Clearinghouse clearinghouseClin, int batchNum)
        {//called from various eclaims classes. Clinic-level clearinghouse passed in.
            if (clearinghouseClin.CommBridge == EclaimsCommBridge.RECS)
            {
                //A RECS rollback never deletes the file, because there is only one
            }
            else
            {
                //This is a Windows extension, so we do not need to worry about Unix path separator characters.
                File.Delete(Path.Combine(clearinghouseClin.ExportPath, "claims" + batchNum.ToString() + ".txt"));
            }
        }

        ///<summary>Called from Eclaims and includes multiple claims.  Returns the string that was sent.  
        ///The string needs to be parsed to determine the transaction numbers used for each claim.</summary>
        public static string SendBatch(Clearinghouse clearinghouseClin, List<ClaimSendQueueItem> queueItems, int batchNum, EnumClaimMedType medType,
            bool isAutomatic)
        {
            //each batch is already guaranteed to be specific to one clearinghouse, one clinic, and one EnumClaimMedType
            //Clearinghouse clearhouse=ClearinghouseL.GetClearinghouse(queueItems[0].ClearinghouseNum);
            string saveFile = GetFileName(clearinghouseClin, batchNum, isAutomatic);
            if (saveFile == "")
            {
                return "";
            }
            string messageText = GenerateBatch(clearinghouseClin, queueItems, batchNum, medType);
            if (clearinghouseClin.IsClaimExportAllowed)
            {
                if (clearinghouseClin.CommBridge == EclaimsCommBridge.PostnTrack)
                {
                    //need to clear out all CRLF from entire file
                    messageText = messageText.Replace("\r", "");
                    messageText = messageText.Replace("\n", "");
                }
                File.WriteAllText(saveFile, messageText, Encoding.ASCII);
                CopyToArchive(saveFile);
            }
            return messageText;
        }

        ///<summary>Helper method for SendBatch that generates the X837 messagetext. This method is public as it is used in ClaimConnect.cs</summary>
        public static string GenerateBatch(Clearinghouse clearinghouseClin, List<ClaimSendQueueItem> queueItems, int batchNum, EnumClaimMedType medType)
        {
            string messageText = "";
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    if (clearinghouseClin.Eformat == ElectronicClaimFormat.x837D_4010)
                    {
                        X837_4010.GenerateMessageText(sw, clearinghouseClin, batchNum, queueItems);
                    }
                    else
                    {//Any of the 3 kinds of 5010
                        X837_5010.GenerateMessageText(sw, clearinghouseClin, batchNum, queueItems, medType);
                    }
                    sw.Flush();//Write contents of sw into ms.
                    ms.Position = 0;//Reset position to 0, or else the StreamReader below will not read anything.
                    using (StreamReader sr = new StreamReader(ms, Encoding.ASCII))
                    {
                        messageText = sr.ReadToEnd();
                    }
                }
            }
            return messageText;
        }

        ///<summary>Copies the given file to an archive directory within the same directory as the file.</summary>
        private static void CopyToArchive(string fileName)
        {
            string direct = Path.GetDirectoryName(fileName);
            string fileOnly = Path.GetFileName(fileName);
            string archiveDir = Path.Combine(direct, "archive");
            try
            {
                if (!Directory.Exists(archiveDir))
                {
                    Directory.CreateDirectory(archiveDir);
                }
                File.Copy(fileName, Path.Combine(archiveDir, fileOnly), true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to copy file to the archive directory. Check to make sure you have permission to modify the archive located at " + archiveDir + 
                    "\r\n\r\n" +
                    "Error message: " + ex.Message);
            }
        }
    }
}