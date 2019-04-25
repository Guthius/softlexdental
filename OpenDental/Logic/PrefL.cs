using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using CodeBase;
using Ionic.Zip;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System.Net;

namespace OpenDental
{
    public class PrefL
    {
        ///<summary>Copies the installation directory files into the database.</summary>
        ///<param name="versionCurrent">The versioning information that will go into the Manifest.txt</param>
        ///<param name="isSilent">Set to true when upgrading silently.  No message boxes will show but errors will log and exit codes will be set.</param>
        ///<param name="hasAtoZ">Set to true when a copy of the update files needs to be made in the AtoZ share (for backwards compatibility).</param>
        ///<param name="hasConcatFiles">Set to true to also make one large concatenated row in the database (for backwards compatibility).
        ///This method will not return false if this particular option has problems executing.
        ///Making this singular row often times violates MySQL limitations which cause errors that cannot be easily avoided.
        ///Therefore, this method has the potential to log an error of the concat files failing yet the method can still return true.</param>
        ///<returns>Returns true if the update files were successfully copied into the database.</returns>
        public static bool CopyFromHereToUpdateFiles(Version versionCurrent, bool isSilent, bool hasAtoZ, bool hasConcatFiles, Form currentForm)
        {
            #region Get Valid AtoZ path
            if (hasAtoZ && PrefC.AtoZfolderUsed == DataStorageType.LocalAtoZ)
            {
                string prefImagePath = ImageStore.GetPreferredAtoZpath();
                if (prefImagePath == null || !Directory.Exists(prefImagePath))
                {//AtoZ folder not found
                    if (isSilent)
                    {
                        FormOpenDental.ExitCode = 300;//AtoZ folder not found (Warning)
                        return false;
                    }
                    FormPath FormP = new FormPath();
                    FormP.IsStartingUp = true;
                    FormP.ShowDialog();
                    if (FormP.DialogResult != DialogResult.OK)
                    {
                        MsgBox.Show("Prefs", "Invalid A to Z path.  Closing program.");
                        FormOpenDental.ExitCode = 300;//AtoZ folder not found (Warning)
                        return false;
                    }
                }
            }
            #endregion
            #region Start Thread
            bool result = false;
            if (!isSilent)
            {//show progress bar, run on separate thread
                ODProgress.ShowAction(() => result = CopyFilesToDatabase(hasAtoZ, isSilent, versionCurrent, hasConcatFiles, currentForm),
                    odEventType: ODEventType.PrefL);
            }
            else
            {//otherwise call on main thread
                result = CopyFilesToDatabase(hasAtoZ, isSilent, versionCurrent, hasConcatFiles, currentForm);
            }
            #endregion
            return result;//if true, inserting a compressed version of the files in the current installation directory into the database was successful.
        }

        ///<summary>Core copying logic for copying the files from the installation directory into the database. Can run on main thread or a separate
        ///thread. Returns true if the files were copied sucessfully.</summary>
        public static bool CopyFilesToDatabase(bool hasAtoZ, bool isSilent, Version versionCurrent, bool hasConcatFiles, Form currentForm)
        {
            #region Delete Old UpdateFiles Folders
            string folderTempUpdateFiles = ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(), "UpdateFiles");
            string folderAtoZUpdateFiles = "";
            if (PrefC.AtoZfolderUsed == DataStorageType.LocalAtoZ && hasAtoZ)
            {
                folderAtoZUpdateFiles = ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(), "UpdateFiles");
            }
            ODEvent.Fire(ODEventType.PrefL, Lan.g("Prefs", "Removing old update files..."));
            //Try to delete the UpdateFiles folder from both the AtoZ share and the local TEMP dir.
            if (!DeleteFolder(folderAtoZUpdateFiles) | !DeleteFolder(folderTempUpdateFiles))
            {//Logical OR to prevent short circuit.
                FormOpenDental.ExitCode = 301;//UpdateFiles folder cannot be deleted (Warning)
                if (!isSilent)
                {
                    currentForm.InvokeIfRequired(() =>
                    {
                        MsgBox.Show("Prefs", "Unable to delete old UpdateFiles folder.  Go manually delete the UpdateFiles folder then retry.");
                    });
                }
                return false;
            }
            #endregion
            #region Copy Current Installation Directory To UpdateFiles Folders
            ODEvent.Fire(ODEventType.PrefL, Lan.g("Prefs", "Backing up new update files..."));
            //Copy the installation directory files to the UpdateFiles share and a TEMP dir that we just created which we will zip up and insert into the db.
            //When PrefC.AtoZfolderUsed is true and we're upgrading from a version prior to 15.3.10, this copy that we are about to make allows backwards 
            //compatibility for versions of OD that do not look at the database for their UpdateFiles.
            if (!CopyInstallFilesToPath(folderAtoZUpdateFiles, versionCurrent) | !CopyInstallFilesToPath(folderTempUpdateFiles, versionCurrent))
            {
                FormOpenDental.ExitCode = 302;//Installation files could not be copied.
                if (!isSilent)
                {
                    currentForm.InvokeIfRequired(() =>
                    {
                        MsgBox.Show("Prefs", "Failed to copy the current installation files on this computer.\r\n"
                            + "This could be due to a lack of permissions, or file(s) in the installation directory are still in use.");
                    });
                }
                return false;
            }
            #endregion
            #region Get Current MySQL max_allowed_packet Setting
            //Starting in v15.3, we always insert the UpdateFiles into the database.
            int maxAllowedPacket = 0;
            int defaultMaxAllowedPacketSize = 41943040;//40MB

            ODEvent.Fire(ODEventType.PrefL, Lan.g("Prefs", "Getting MySQL max allowed packet setting..."));
            maxAllowedPacket = MiscData.GetMaxAllowedPacket();
            //If trying to get the max_allowed_packet value for MySQL failed, assume they can handle 40MB of data.
            //Our installations of MySQL defaults the global property 'max_allowed_packet' to 40MB.
            //Nathan suggested forcing the global and local max_allowed_packet to 40MB if it was set to anything less.
            if (maxAllowedPacket < defaultMaxAllowedPacketSize)
            {
                try
                {
                    maxAllowedPacket = MiscData.SetMaxAllowedPacket(defaultMaxAllowedPacketSize);
                }
                catch (Exception ex)
                {
                    //Do nothing.  Either maxAllowedPacket is set to something small (e.g. 10MB) and we failed to update it to 40MB (should be fine)
                    //             OR we failed to get and set the global variable due to MySQL permissions and a UE was thrown.
                    //             Regardless, if maxAllowedPacket is 0 (the only thing that we can't have happen) it will get updated to 40MB later down.
                    ODException.SwallowAnyException(() =>
                    {
                        EventLog.WriteEntry("OpenDental", "Error updating max_allowed_packet from " + maxAllowedPacket
                            + " to " + defaultMaxAllowedPacketSize + ":\r\n" + ex.Message, EventLogEntryType.Error);
                    });
                }
            }

            //Only change maxAllowedPacket if we couldn't successfully get the current value from the database or using Oracle.
            //This will let the program attempt to insert the UpdateFiles into the db with the assumption that they are using our default setting (40MB).
            //Worst case scenario, the user will hit the max_packet_allowed error below which will simply notify them to update their my.ini manually.
            if (maxAllowedPacket == 0)
            {
                maxAllowedPacket = defaultMaxAllowedPacketSize;
            }
            //Now we need to break up the memory stream into a Base64 string but each payload needs to be small enough to send to MySQL.
            //Each character in Base64 represents 6 bits.  Therefore, 4 chars are used to represent 3 bytes
            //Therefore we have to read an amout of bytes per loop that must be divisible by 3. 
            //Also, we want to 'buffer' a few KB for MySQL because the query itself and the parameter information will take up some bytes (unknown).
            int charsPerPayload = maxAllowedPacket - 8192;//Arbitrarily subtracted 8KB from max allowed bytes for MySQL "header" information.
            charsPerPayload -= (charsPerPayload % 3);//Use the closest amount of bytes divisible by 3.
            #endregion
            #region Zip Update Files Into Memory
            MemoryStream memStream = new MemoryStream();
            ZipFile zipFile = new ZipFile();
            //Take the entire directory in the temp dir that we just created and zip it up.
            ODEvent.Fire(ODEventType.PrefL, Lan.g("Prefs", "Compressing new update files..."));
            try
            {
                zipFile.AddDirectory(folderTempUpdateFiles);
                zipFile.Save(memStream);
            }
            catch (Exception ex)
            {
                memStream.Dispose();
                zipFile.Dispose();
                FormOpenDental.ExitCode = 304;//Error compressing UpdateFiles
                if (!isSilent)
                {
                    currentForm.InvokeIfRequired(() =>
                    {
                        MessageBox.Show(Lan.g("Prefs", "Error compressing UpdateFiles:") + "\r\n" + ex.Message);
                    });
                }
                return false;
            }
            #endregion
            #region Insert Update Files Into One Row
            if (hasConcatFiles)
            {
                //For backwards compatibility we have to try and store the entire UpdateFiles content into one row
                //Everything within this section will be in a try catch because we found out that it can fail due to the amount of data being sent.
                //The MySQL CONCAT command gives up on life after so much data and sets the column to 0 bytes but does not throw an exception.
                //This is simply here to help reduce the number of offices that might have problems updating from older versions.
                //E.g. buying a new workstation and using an old trial installer could require this single large Update Files column.
                try
                {
                    //Converting the file to Base64String bloats the size by approximately 30% so we need to make sure that the chunk size is well below 40MB
                    //Old code used 15MB and that seemed to work very well for the majority of users.
                    charsPerPayload = Math.Min(charsPerPayload, 15728640);//15728640 is divisible by 3 which is important for Base64 "appending" logic.
                    ODEvent.Fire(ODEventType.PrefL, Lan.g("Prefs", "Deleting old update files row..."));
                    DocumentMiscs.DeleteAllForType(DocumentMiscType.UpdateFiles);
                    byte[] zipFileBytes = new byte[charsPerPayload];
                    memStream.Position = 0;//Start at the beginning of the stream.
                    ODEvent.Fire(ODEventType.PrefL, Lan.g("Prefs", "Inserting new update files into database row..."));
                    DocumentMisc docUpdateFiles = new DocumentMisc();
                    docUpdateFiles.DateCreated = DateTime.Today;
                    docUpdateFiles.DocMiscType = DocumentMiscType.UpdateFiles;
                    docUpdateFiles.FileName = "UpdateFiles.zip";
                    DocumentMiscs.Insert(docUpdateFiles);
                    while ((memStream.Read(zipFileBytes, 0, zipFileBytes.Length)) > 0)
                    {
                        DocumentMiscs.AppendRawBase64ForUpdateFiles(Convert.ToBase64String(zipFileBytes));
                    }
                }
                catch (Exception ex)
                {
                    //Only log the error, do not stop the update process.  The above code is known to fail for various reasons and we abandoned it.
                    ODEvent.Fire(ODEventType.PrefL, Lan.g("Prefs", "Error inserting new update files into database row..."));
                    ODException.SwallowAnyException(() =>
                    {
                        EventLog.WriteEntry("OpenDental", "Error inserting new update files into database row:\r\n" + ex.Message,
                            EventLogEntryType.Error);
                    });
                }
            }
            #endregion
            #region Insert Update Files Segments Into Many Rows
            //When we try and send over ~40MB of data, MySQL can drop our connection randomly giving a "MySQL server has gone away" error.
            //Use a maximum of ~1MB payloads so that the likelyhood of this error is less.
            charsPerPayload = Math.Min(charsPerPayload, 1048575);//1048575 is divisible by 3 which is important for Base64 "appending" logic.
            try
            {
                //Clear and prep the current UpdateFiles row in the documentmisc table for the updated binaries.
                ODEvent.Fire(ODEventType.PrefL, Lan.g("Prefs", "Deleting old update files segments..."));
                DocumentMiscs.DeleteAllForType(DocumentMiscType.UpdateFilesSegment);
                byte[] zipFileBytes = new byte[charsPerPayload];
                memStream.Position = 0;//Start at the beginning of the stream.
                                       //Convert the zipped up bytes into Base64 and instantly insert it into the database little by little.
                ODEvent.Fire(ODEventType.PrefL, Lan.g("Prefs", "Inserting new update files segments into database..."));
                try
                {
                    int count = 1;
                    DocumentMisc docUpdateFilesSegment = new DocumentMisc();
                    docUpdateFilesSegment.DateCreated = DateTime.Today;
                    docUpdateFilesSegment.DocMiscType = DocumentMiscType.UpdateFilesSegment;
                    while ((memStream.Read(zipFileBytes, 0, zipFileBytes.Length)) > 0)
                    {
                        docUpdateFilesSegment.FileName = count.ToString().PadLeft(4, '0');
                        docUpdateFilesSegment.RawBase64 = Convert.ToBase64String(zipFileBytes);
                        DocumentMiscs.Insert(docUpdateFilesSegment);
                        count++;
                    }
                    ODEvent.Fire(ODEventType.PrefL, Lan.g("Prefs", "Done..."));
                }
                catch (MySqlException myEx)
                {
                    ODException.SwallowAnyException(() =>
                    {
                        EventLog.WriteEntry("OpenDental", "Error inserting UpdateFiles into database:"
                            + "\r\nMySqlException: " + myEx.Message
                            + "\r\n  maxAllowedPacket: " + maxAllowedPacket
                            + "\r\n  charsPerPayload: " + charsPerPayload
                            + "\r\n  memStream.Length: " + memStream.Length, EventLogEntryType.Error);
                    });
                    throw myEx;
                }
                catch (Exception ex)
                {
                    ODException.SwallowAnyException(() =>
                    {
                        EventLog.WriteEntry("OpenDental", "Error inserting UpdateFiles into database:"
                            + "\r\n" + ex.Message
                            + "\r\n  maxAllowedPacket: " + maxAllowedPacket
                            + "\r\n  charsPerPayload: " + charsPerPayload
                            + "\r\n  memStream.Length: " + memStream.Length, EventLogEntryType.Error);
                    });
                    throw ex;
                }
            }
            catch (MySqlException)
            {
                FormOpenDental.ExitCode = 303;//Failed inserting update files into the database.
                if (isSilent)
                {
                    return false;
                }
                string errorStr = Lan.g("Prefs", "Failed inserting update files into the database.");
                currentForm.InvokeIfRequired(() =>
                {
                    MessageBox.Show(errorStr);
                });
                return false;
            }
            catch (Exception ex)
            {//non-MySqlException
                FormOpenDental.ExitCode = 303;//Failed inserting update files into the database.
                if (isSilent)
                {
                    return false;
                }
                currentForm.InvokeIfRequired(() =>
                {
                    MessageBox.Show(Lan.g("Prefs", "Failed inserting update files into the database.") + "\r\n" + ex.Message);
                });
                return false;
            }
            finally
            {
                if (memStream != null)
                {
                    memStream.Dispose();
                }
                if (zipFile != null)
                {
                    zipFile.Dispose();
                }
            }
            #endregion
            return true;
        }

        ///<summary>Creates the directory passed in and copies the files in the current installation directory to the specified folder path.
        ///Creates a Manifest.txt file with the version information passed in.
        ///Returns false if anything goes wrong.  Returns true if copy was successful or if the folder path passed in is null or blank.</summary>
        private static bool CopyInstallFilesToPath(string folderPath, Version versionCurrent)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                return true;//Some customers will not be using the AtoZ folder which will pass in blank.
            }
            try
            {
                Directory.CreateDirectory(folderPath);
                DirectoryInfo dirInfo = new DirectoryInfo(Application.StartupPath);
                FileInfo[] appfiles = dirInfo.GetFiles();
                for (int i = 0; i < appfiles.Length; i++)
                {
                    if (appfiles[i].Name == "FreeDentalConfig.xml")
                    {
                        continue;//skip this one.
                    }
                    if (appfiles[i].Name == "OpenDentalServerConfig.xml")
                    {
                        continue;//skip also
                    }
                    if (appfiles[i].Name == "ProximitySettings.txt")
                    {
                        continue;//skip as well
                    }
                    if (appfiles[i].Name == "DpsPos.dll.config")
                    {
                        continue;//yep, skip it
                    }
                    if (appfiles[i].Name.StartsWith("openlog"))
                    {
                        continue;//these can be big and are irrelevant
                    }
                    if (appfiles[i].Name.Contains("__"))
                    {//double underscore
                        continue;//So that plugin dlls can purposely skip the file copy.
                    }
                    //include UpdateFileCopier
                    File.Copy(appfiles[i].FullName, ODFileUtils.CombinePaths(folderPath, appfiles[i].Name));
                }
                //Create a simple manifest file so that we know what version the files are for.
                File.WriteAllText(ODFileUtils.CombinePaths(folderPath, "Manifest.txt"), versionCurrent.ToString(3));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        ///<summary>Recursively deletes all folders and files in the provided folder path.
        ///Waits up to 10 seconds for the delete command to finish.  Returns false if anything goes wrong.</summary>
        private static bool DeleteFolder(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                return true;//Nothing to delete.
            }
            if (!Directory.Exists(folderPath))
            {
                return true;//Already deleted
            }
            //The directory we want to delete is present so try and recursively delete it and all its content.
            try
            {
                Directory.Delete(folderPath, true);
            }
            catch
            {
                return false;//Something went wrong, typically a permission issue or files are still in use.
            }
            //The delete seems to have been successful, wait up to 10 seconds so that CreateDirectory won't malfunction.
            DateTime dateTimeWait = DateTime.Now.AddSeconds(10);
            while (Directory.Exists(folderPath) && DateTime.Now < dateTimeWait)
            {
                Application.DoEvents();
            }
            if (Directory.Exists(folderPath))
            {//Dir is still present after 10 seconds of waiting for the delete to complete.
                return false;
            }
            return true;//Dir deleted successfully.
        }

        ///<summary>Returns true if the directory passed in was created or already exists.
        ///Otherwise; returns false after displaying an error message to the user.</summary>
        private static bool TryCreateDirectory(string fullPath)
        {
            try
            {
                Directory.CreateDirectory(fullPath);
            }
            catch (Exception ex)
            {
                FormFriendlyException.Show(Lans.g("Prefs", "Unable to create the following directory:") + " " + fullPath, ex);
                return false;
            }
            return true;
        }

        ///<summary>If AtoZ.manifest was wrong, or if user is not using AtoZ, then just download again.  Will use dir selected by user.  If an appropriate download is not available, it will fail and inform user.</summary>
        private static void DownloadAndRunSetup(Version storedVersion, Version currentVersion)
        {
            string patchName = "Setup.exe";
            string updateUri = PrefC.GetString(PrefName.UpdateWebsitePath);
            string updateCode = PrefC.GetString(PrefName.UpdateCode);
            string updateInfoMajor = "";
            string updateInfoMinor = "";
            if (!ShouldDownloadUpdate(updateUri, updateCode, out updateInfoMajor, out updateInfoMinor))
            {
                return;
            }
            if (MessageBox.Show(
                Lan.g("Prefs", "Setup file will now be downloaded.") + "\r\n"
                + Lan.g("Prefs", "Workstation version will be updated from ") + currentVersion.ToString(3)
                + Lan.g("Prefs", " to ") + storedVersion.ToString(3),
                "", MessageBoxButtons.OKCancel)
                != DialogResult.OK)//they don't want to update for some reason.
            {
                return;
            }
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = ImageStore.GetPreferredAtoZpath();
            dlg.Description = Lan.g("Prefs", "Setup.exe will be downloaded to the folder you select below");
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;//app will exit
            }
            string tempFile = ODFileUtils.CombinePaths(dlg.SelectedPath, patchName);
            //ODFileUtils.CombinePaths(GetTempFolderPath(),patchName);
            DownloadInstallPatchFromURI(updateUri + updateCode + "/" + patchName,//Source URI
                tempFile, true, false, null);//Local destination file.
            if (File.Exists(tempFile))
            {//If user canceld in DownloadInstallPatchFromURI file will not exist.
                File.Delete(tempFile);//Cleanup install file.
            }
        }

        ///<summary>Returns true if the download at the specified remoteUri with the given registration code should be downloaded and installed as an update, and false is returned otherwise. Also, information about the decision making process is stored in the updateInfoMajor and updateInfoMinor strings, but only holds significance to a human user.</summary>
        public static bool ShouldDownloadUpdate(string remoteUri, string updateCode, out string updateInfoMajor, out string updateInfoMinor)
        {
            updateInfoMajor = "";
            updateInfoMinor = "";
            bool shouldDownload = false;
            string fileName = "Manifest.txt";
            WebClient myWebClient = new WebClient();
            string myStringWebResource = remoteUri + updateCode + "/" + fileName;
            Version versionNewBuild = null;
            string strNewVersion = "";
            string newBuild = "";
            bool buildIsAlpha = false;
            bool buildIsBeta = false;
            bool versionIsAlpha = false;
            bool versionIsBeta = false;
            try
            {
                using (StreamReader sr = new StreamReader(myWebClient.OpenRead(myStringWebResource)))
                {
                    newBuild = sr.ReadLine();//must be be 3 or 4 components (revision is optional)
                    strNewVersion = sr.ReadLine();//returns null if no second line
                }
                if (newBuild.EndsWith("a"))
                {
                    buildIsAlpha = true;
                    newBuild = newBuild.Replace("a", "");
                }
                if (newBuild.EndsWith("b"))
                {
                    buildIsBeta = true;
                    newBuild = newBuild.Replace("b", "");
                }
                versionNewBuild = new Version(newBuild);
                if (versionNewBuild.Revision == -1)
                {
                    versionNewBuild = new Version(versionNewBuild.Major, versionNewBuild.Minor, versionNewBuild.Build, 0);
                }
                if (strNewVersion != null && strNewVersion.EndsWith("a"))
                {
                    versionIsAlpha = true;
                    strNewVersion = strNewVersion.Replace("a", "");
                }
                if (strNewVersion != null && strNewVersion.EndsWith("b"))
                {
                    versionIsBeta = true;
                    strNewVersion = strNewVersion.Replace("b", "");
                }
            }
            catch
            {
                updateInfoMajor += Lan.g("FormUpdate", "Registration number not valid, or internet connection failed.  ");
                return false;
            }
            if (versionNewBuild == new Version(Application.ProductVersion))
            {
                updateInfoMajor += Lan.g("FormUpdate", "You are using the most current build of this version.  ");
            }
            else
            {
                //this also allows users to install previous versions.
                updateInfoMajor += Lan.g("FormUpdate", "A new build of this version is available for download:  ")
                    + versionNewBuild.ToString();
                if (buildIsAlpha)
                {
                    updateInfoMajor += Lan.g("FormUpdate", "(alpha)  ");
                }
                if (buildIsBeta)
                {
                    updateInfoMajor += Lan.g("FormUpdate", "(beta)  ");
                }
                shouldDownload = true;
            }
            //Whether or not build is current, we want to inform user about the next minor version
            if (strNewVersion != null)
            {//we don't really care what it is.
                updateInfoMinor += Lan.g("FormUpdate", "A newer version is also available.  ");
                if (versionIsAlpha)
                {
                    updateInfoMinor += Lan.g("FormUpdate", "It is alpha (experimental), so it has bugs and " +
                        "you will need to update it frequently.  ");
                }
                if (versionIsBeta)
                {
                    updateInfoMinor += Lan.g("FormUpdate", "It is beta (test), so it has some bugs and " +
                        "you will need to update it frequently.  ");
                }
                updateInfoMinor += Lan.g("FormUpdate", "Contact us for a new Registration number if you wish to use it.  ");
            }
            return shouldDownload;
        }

        /// <summary>destinationPath includes filename (Setup.exe).  destinationPath2 will create a second copy at the specified path/filename, or it will be skipped if null or empty.</summary>
        public static void DownloadInstallPatchFromURI(string downloadUri, string destinationPath, bool runSetupAfterDownload, bool showShutdownWindow, string destinationPath2)
        {
            string[] dblist = PrefC.GetString(PrefName.UpdateMultipleDatabases).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            bool isShutdownWindowNeeded = showShutdownWindow;
            while (isShutdownWindowNeeded)
            {
                //Even if updating multiple databases, extra shutdown signals are not needed.
                FormShutdown FormSD = new FormShutdown();
                FormSD.IsUpdate = true;
                FormSD.ShowDialog();
                if (FormSD.DialogResult == DialogResult.OK)
                {
                    //turn off signal reception for 5 seconds so this workstation will not shut down.
                    Signalods.SignalLastRefreshed = MiscData.GetNowDateTime().AddSeconds(5);
                    Signalod sig = new Signalod();
                    sig.IType = InvalidType.ShutDownNow;
                    Signalods.Insert(sig);
                    Computers.ClearAllHeartBeats(Environment.MachineName);//always assume success
                    isShutdownWindowNeeded = false;
                    //SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Shutdown all workstations.");//can't do this because sometimes no user.
                }
                else if (FormSD.DialogResult == DialogResult.Cancel)
                {//Cancel
                    if (MsgBox.Show("FormUpdate", MsgBoxButtons.YesNo, "Are you sure you want to cancel the update?"))
                    {
                        return;
                    }
                    continue;
                }
                //no other workstation will be able to start up until this value is reset.
                Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, Environment.MachineName);
            }
            MiscData.LockWorkstationsForDbs(dblist);//lock workstations for other db's.
            try
            {
                File.Delete(destinationPath);
            }
            catch (Exception ex)
            {
                FormFriendlyException.Show(Lan.g("FormUpdate", "Error deleting file:") + "\r\n" + ex.Message, ex);
                MiscData.UnlockWorkstationsForDbs(dblist);//unlock workstations since nothing was actually done.
                Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");
                return;
            }
            WebRequest wr = WebRequest.Create(downloadUri);
            WebResponse webResp = null;
            try
            {
                webResp = wr.GetResponse();
            }
            catch (Exception ex)
            {
                CodeBase.MsgBoxCopyPaste msgbox = new MsgBoxCopyPaste(ex.Message + "\r\nUri: " + downloadUri);
                msgbox.ShowDialog();
                MiscData.UnlockWorkstationsForDbs(dblist);//unlock workstations since nothing was actually done.
                Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");
                return;
            }
            int fileSize = (int)webResp.ContentLength / 1024;
            FormProgress FormP = new FormProgress();
            //start the thread that will perform the download
            ThreadStart downloadDelegate = delegate { DownloadInstallPatchWorker(downloadUri, destinationPath, webResp.ContentLength, ref FormP); };
            Thread workerThread = new Thread(downloadDelegate);
            workerThread.Start();
            //display the progress dialog to the user:
            FormP.MaxVal = (double)fileSize / 1024;
            FormP.NumberMultiplication = 100;
            FormP.DisplayText = "?currentVal MB of ?maxVal MB copied";
            FormP.NumberFormat = "F";
            FormP.ShowDialog();
            if (FormP.DialogResult == DialogResult.Cancel)
            {
                workerThread.Abort();
                MiscData.UnlockWorkstationsForDbs(dblist);//unlock workstations since nothing was actually done.
                Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");
                return;
            }
            //copy to second destination directory
            if (!CloudStorage.IsCloudStorage)
            {
                if (destinationPath2 != null && destinationPath2 != "")
                {
                    if (File.Exists(destinationPath2))
                    {
                        try
                        {
                            File.Delete(destinationPath2);
                        }
                        catch (Exception ex)
                        {
                            FormFriendlyException.Show(Lan.g("FormUpdate", "Error deleting file:") + "\r\n" + ex.Message, ex);
                            MiscData.UnlockWorkstationsForDbs(dblist);//unlock workstations since nothing was actually done.
                            Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");
                            return;
                        }
                    }
                    File.Copy(destinationPath, destinationPath2);
                }
            }
            else
            {//Cloud storing
                OpenDentalCloud.Core.TaskStateUpload state = null;
                byte[] arrayBytes = File.ReadAllBytes(destinationPath);
                FormP = new FormProgress();
                FormP.DisplayText = Lan.g("FormUpdate", "Uploading Setup File...");//Upload unversioned setup file to AtoZ main folder.
                FormP.NumberFormat = "F";
                FormP.NumberMultiplication = 1;
                FormP.MaxVal = 100;//Doesn't matter what this value is as long as it is greater than 0
                FormP.TickMS = 1000;
                state = CloudStorage.UploadAsync(
                    CloudStorage.AtoZPath
                    , Path.GetFileName(destinationPath)
                    , arrayBytes
                    , new OpenDentalCloud.ProgressHandler(FormP.OnProgress));
                if (FormP.ShowDialog() == DialogResult.Cancel)
                {
                    state.DoCancel = true;
                    MiscData.UnlockWorkstationsForDbs(dblist);//unlock workstations since nothing was actually done.
                    Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");
                    return;
                }
                if (destinationPath2 != null && destinationPath2 != "")
                {//Upload a copy of the Setup.exe to a versioned setup file to SetupFiles folder.  Not always used.
                    FormP = new FormProgress();
                    FormP.DisplayText = Lan.g("FormUpdate", "Uploading Setup File SetupFiles folder...");
                    FormP.NumberFormat = "F";
                    FormP.NumberMultiplication = 1;
                    FormP.MaxVal = 100;//Doesn't matter what this value is as long as it is greater than 0
                    FormP.TickMS = 1000;
                    state = CloudStorage.UploadAsync(
                        ODFileUtils.CombinePaths(CloudStorage.AtoZPath, "SetupFiles")
                        , Path.GetFileName(destinationPath2)
                        , arrayBytes
                        , new OpenDentalCloud.ProgressHandler(FormP.OnProgress));
                    if (FormP.ShowDialog() == DialogResult.Cancel)
                    {
                        state.DoCancel = true;
                        MiscData.UnlockWorkstationsForDbs(dblist);//unlock workstations since nothing was actually done.
                        Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");
                        return;
                    }
                }
            }
            //copy the Setup.exe to the AtoZ folders for the other db's.
            List<string> atozNameList = MiscData.GetAtoZforDb(dblist);
            for (int i = 0; i < atozNameList.Count; i++)
            {
                if (destinationPath == Path.Combine(atozNameList[i], "Setup.exe"))
                {//if they are sharing an AtoZ folder.
                    continue;
                }
                if (Directory.Exists(atozNameList[i]))
                {
                    File.Copy(destinationPath,//copy the Setup.exe that was just downloaded to this AtoZ folder
                        Path.Combine(atozNameList[i], "Setup.exe"),//to the other atozFolder
                        true);//overwrite
                }
            }
            if (!runSetupAfterDownload)
            {
                return;
            }
            string msg = Lan.g("FormUpdate", "Download succeeded.  Setup program will now begin.  When done, restart the program on this computer, then on the other computers.");
            if (dblist.Length > 0)
            {
                msg = "Download succeeded.  Setup file probably copied to other AtoZ folders as well.  Setup program will now begin.  When done, restart the program for each database on this computer, then on the other computers.";
            }
            if (MessageBox.Show(msg, "", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                //Clicking cancel gives the user a chance to avoid running the setup program,
                Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");//unlock workstations, since nothing was actually done.
                return;
            }
            #region Stop OpenDent Services
            //If the update has been initiated from the designated update server then try and stop all "OpenDent..." services.
            //They will be automatically restarted once Open Dental has successfully upgraded.
            if (PrefC.GetString(PrefName.WebServiceServerName) != "" && ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.WebServiceServerName)))
            {
                Action actionCloseStopServicesProgress = ODProgress.Show(ODEventType.MiscData, typeof(MiscDataEvent), "Stopping services...");
                List<ServiceController> listOpenDentServices = ServicesHelper.GetAllOpenDentServices();
                //Newer versions of Windows have heightened security measures for managing services.
                //We get lots of calls where users do not have the correct permissions to start and stop Open Dental services.
                //Open Dental services are not important enough to warrent "Admin" rights to manage so we want to allow "Everyone" to start and stop them.
                ServicesHelper.SetSecurityDescriptorToAllowEveryoneToManageServices(listOpenDentServices);
                //Loop through all Open Dental services and stop them if they have not stopped or are not pending a stop so that their binaries can be updated.
                string servicesNotStopped = ServicesHelper.StopServices(listOpenDentServices);
                actionCloseStopServicesProgress?.Invoke();
                //Notify the user to go manually stop the services that could not automatically stop.
                if (!string.IsNullOrEmpty(servicesNotStopped))
                {
                    MsgBoxCopyPaste msgBCP = new MsgBoxCopyPaste(Lan.g("FormUpdate", "The following services could not be stopped.  You need to manually stop them before continuing.")
                    + "\r\n" + servicesNotStopped);
                    msgBCP.ShowDialog();
                }
            }
            #endregion
            try
            {
                Process.Start(destinationPath);
                FormOpenDental.S_ProcessKillCommand();
            }
            catch
            {
                Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");//unlock workstations, since nothing was actually done.
                MsgBox.Show(FormP, "Could not launch setup");
            }
        }

        ///<summary>This is the function that the worker thread uses to actually perform the download.
        ///Can also call this method in the ordinary way if the file to be transferred is short.</summary>
        private static void DownloadInstallPatchWorker(string downloadUri, string destinationPath, long contentLength, ref FormProgress progressIndicator)
        {
            using (WebClient webClient = new WebClient())
            using (Stream streamRead = webClient.OpenRead(downloadUri))
            using (FileStream fileStream = new FileStream(destinationPath, FileMode.Create))
            {
                int bytesRead;
                long position = 0;
                byte[] buffer = new byte[10 * 1024];
                try
                {
                    while ((bytesRead = streamRead.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        position += bytesRead;
                        if (position != contentLength)
                        {
                            progressIndicator.CurrentVal = ((double)position / 1024) / 1024;
                        }
                        fileStream.Write(buffer, 0, bytesRead);
                    }
                }
                catch (Exception ex)
                {
                    //Set the error message so that the user can call in and complain and we can get more information about what went wrong.
                    //This error message will NOT show if the user hit the Cancel button and a random exception happened (because the window will have closed).
                    progressIndicator.ErrorMessage = ex.Message;
                }
            }
            //If the file was successfully downloaded, set the progress indicator to maximum so that it closes the progress window.
            //Otherwise leave the window open so that the error message can be displayed to the user in red text.
            if (string.IsNullOrEmpty(progressIndicator.ErrorMessage))
            {
                progressIndicator.CurrentVal = (double)contentLength / 1024;
            }
            else
            {//There was an unexpected error.
                try
                {
                    File.Delete(destinationPath);//Try to clean up after ourselves.
                }
                catch
                {
                }
            }
        }

        ///<summary></summary>
        private static bool CheckProgramVersionClassic()
        {
            Version storedVersion = new Version(PrefC.GetString(PrefName.ProgramVersion));
            Version currentVersion = new Version(Application.ProductVersion);
            string database = MiscData.GetCurrentDatabase();
            if (storedVersion < currentVersion)
            {
                Prefs.UpdateString(PrefName.ProgramVersion, currentVersion.ToString());
                UpdateHistory updateHistory = new UpdateHistory(currentVersion.ToString());
                UpdateHistories.Insert(updateHistory);
                Cache.Refresh(InvalidType.Prefs);
            }
            if (storedVersion > currentVersion)
            {
                if (PrefC.AtoZfolderUsed == DataStorageType.LocalAtoZ)
                {
                    string setupBinPath = ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(), "Setup.exe");
                    if (File.Exists(setupBinPath))
                    {
                        if (MessageBox.Show("You are attempting to run version " + currentVersion.ToString(3) + ",\r\n"
                            + "But the database " + database + "\r\n"
                            + "is already using version " + storedVersion.ToString(3) + ".\r\n"
                            + "A newer version must have already been installed on at least one computer.\r\n"
                            + "The setup program stored in your A to Z folder will now be launched.\r\n"
                            + "Or, if you hit Cancel, then you will have the option to download again."
                            , "", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        {
                            if (MessageBox.Show("Download again?", "", MessageBoxButtons.OKCancel)
                                == DialogResult.OK)
                            {
                                FormUpdate FormU = new FormUpdate();
                                FormU.ShowDialog();
                            }
                            Application.Exit();
                            return false;
                        }
                        try
                        {
                            Process.Start(setupBinPath);
                        }
                        catch
                        {
                            MessageBox.Show("Could not launch Setup.exe");
                        }
                    }
                    else if (MessageBox.Show("A newer version has been installed on at least one computer," +
                            "but Setup.exe could not be found in any of the following paths: " +
                            ImageStore.GetPreferredAtoZpath() + ".  Download again?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        FormUpdate FormU = new FormUpdate();
                        FormU.ShowDialog();
                    }
                }
                else
                {//Not using image path.
                 //perform program update automatically.
                    string patchName = "Setup.exe";
                    string updateUri = PrefC.GetString(PrefName.UpdateWebsitePath);
                    string updateCode = PrefC.GetString(PrefName.UpdateCode);
                    string updateInfoMajor = "";
                    string updateInfoMinor = "";
                    if (ShouldDownloadUpdate(updateUri, updateCode, out updateInfoMajor, out updateInfoMinor))
                    {
                        if (MessageBox.Show(updateInfoMajor + Lan.g("Prefs", "Perform program update now?"), "",
                            MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            string tempFile = ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(), patchName);//Resort to a more common temp file name.
                            DownloadInstallPatchFromURI(updateUri + updateCode + "/" + patchName,//Source URI
                                tempFile, true, true, null);//Local destination file.
                            if (File.Exists(tempFile))
                            {//If user canceld in DownloadInstallPatchFromURI file will not exist.
                                File.Delete(tempFile);//Cleanup install file.
                            }
                        }
                    }
                }
                Application.Exit();//always exits, whether launch of setup worked or not
                return false;
            }
            return true;
        }

        ///<summary>Checks to see any OpenDentalCustListener services are currently installed.
        ///If present, each CustListener service will be uninstalled.
        ///After successfully removing all CustListener services, one eConnector service will be installed.
        ///Returns true if the CustListener service was successfully upgraded to the eConnector service.</summary>
        ///<param name="isSilent">Set to false to show meaningful error messages, otherwise fails silently.</param>
        ///<param name="isListening">Will get set to true if the customer was previously using the CustListener service.</param>
        ///<returns>True if only one CustListener services present and was successfully uninstalled along with the eConnector service getting installed.
        ///False if more than one CustListener service is present or the eConnector service could not install.</returns>
        public static bool UpgradeOrInstallEConnector(bool isSilent, out bool isListening)
        {
            isListening = false;
            try
            {
                //Check to see if CustListener service is installed and needs to be uninstalled.
                List<ServiceController> listCustListenerServices = ServicesHelper.GetServicesByExe("OpenDentalCustListener.exe");
                if (listCustListenerServices.Count > 0)
                {
                    isListening = true;
                }
                if (listCustListenerServices.Count == 1)
                {//Only uninstall the listener service if there is exactly one found.  This is just a nicety.
                    ServicesHelper.Uninstall(listCustListenerServices[0]);
                }
                List<ServiceController> listEConnectorServices = ServicesHelper.GetServicesByExe("OpenDentalEConnector.exe");
                if (listEConnectorServices.Count > 0)
                {
                    return true;//An eConnector service is already installed.
                }
                string eConnectorExePath = ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(), "OpenDentalEConnector", "OpenDentalEConnector.exe");
                if (!ServicesHelper.Install("OpenDentalEConnector", eConnectorExePath))
                {
                    if (!isSilent)
                    {
                        throw new ApplicationException(Lans.g("ServicesHelper", "Unable to install the OpenDentalEConnector service."));
                    }
                    return false;
                }
                //Create a new OpenDentalWebConfig.xml file for the eConnector if one is not already present.
                if (!CreateConfigForEConnector(isListening))
                {
                    if (!isSilent)
                    {
                        throw new ApplicationException(Lans.g("ServicesHelper", "The config file for the OpenDentalEConnector service could not be created."));
                    }
                    return false;
                }
                //Now that the service has finally installed we need to try and start it.
                listEConnectorServices = ServicesHelper.GetServicesByExe("OpenDentalEConnector.exe");
                if (listEConnectorServices.Count < 1)
                {
                    if (!isSilent)
                    {
                        throw new ApplicationException(Lans.g("ServicesHelper", "OpenDentalEConnector service could not be found in order to automatically start it."));
                    }
                    return false;
                }
                string eConnectorStartingErrors = ServicesHelper.StartServices(listEConnectorServices);
                if (!string.IsNullOrEmpty(eConnectorStartingErrors))
                {
                    if (!isSilent)
                    {
                        throw new ApplicationException(Lans.g("ServicesHelper", "Unable to start the following eConnector services:") + "\r\n" + eConnectorStartingErrors);
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                if (!isSilent)
                {
                    MessageBox.Show(Lans.g("ServicesHelper", "Failed upgrading to the eConnector service:") + "\r\n" + ex.Message
                        + "\r\n\r\n" + Lans.g("ServicesHelper", "Try running as Administrator."));
                }
                return false;
            }
        }

        ///<summary>Tries to install the OpenDentalService if needed.  Returns false if failed.
        ///Set isSilent to false to show meaningful error messages, otherwise fails silently.</summary>
        public static bool TryInstallOpenDentalService(bool isSilent)
        {
            try
            {
                List<ServiceController> listOpenDentalServices = ServicesHelper.GetServicesByExe("OpenDentalService.exe");
                if (listOpenDentalServices.Count > 0)
                {
                    return true;//An Open Dental Service is already installed.
                }
                string odServiceFilePath = ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(), "OpenDentalService", "OpenDentalService.exe");
                if (!ServicesHelper.Install("OpenDentalService", odServiceFilePath))
                {
                    AlertItems.CreateGenericAlert(Lans.g("ServicesHelper", "Open Dental Service Error"), Lans.g("ServicesHelper", "Failed to install OpenDentalService, try running as admin."));
                    return false;
                }
                //Create a new OpenDentalServiceConfig.xml file for Open Dental Service if one is not already present.
                if (!CreateConfigForOpenDentalService())
                {
                    AlertItems.CreateGenericAlert(Lans.g("ServicesHelper", "Open Dental Service Error"), Lans.g("ServicesHelper", "Failed to create OpenDentalServiceConfig.xml file."));
                    return false;
                }
                //Now that the service has finally installed we need to try and start it.
                listOpenDentalServices = ServicesHelper.GetServicesByExe("OpenDentalService.exe");
                if (listOpenDentalServices.Count < 1)
                {
                    AlertItems.CreateGenericAlert(Lans.g("ServicesHelper", "Open Dental Service Error"), Lans.g("ServicesHelper", "OpenDental Service could not be found."));
                    return false;
                }
                string openDentalServiceStartingErrors = ServicesHelper.StartServices(listOpenDentalServices);
                if (!string.IsNullOrEmpty(openDentalServiceStartingErrors))
                {
                    AlertItems.CreateGenericAlert(Lans.g("ServicesHelper", "Open Dental Service Error"), Lans.g("ServicesHelper", "The following service(s) could not start:") + " " + openDentalServiceStartingErrors);
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                AlertItems.CreateGenericAlert(Lans.g("ServicesHelper", "Open Dental Service Error"), Lans.g("ServicesHelper", "Unknown exception:") + " " + e.Message);
                return false;
            }
        }

        ///<summary>Creates a default OpenDentalWebConfig.xml file for the eConnector if one is not already present.
        ///Uses the current connection settings in DataConnection.  This method does NOT work if called via middle tier.
        ///Users should not be installing the eConnector via the middle tier.</summary>
        private static bool CreateConfigForEConnector(bool isListening)
        {
            string eConnectorConfigPath = ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(), "OpenDentalEConnector", "OpenDentalWebConfig.xml");
            string custListenerConfigPath = ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(), "OpenDentalCustListener", "OpenDentalWebConfig.xml");
            //Check to see if there is already a config file present.
            if (File.Exists(eConnectorConfigPath))
            {
                return true;//Nothing to do.
            }
            //At this point we know that the eConnector does not have a config file present.
            //Check to see if the user is currently using the CustListener service.
            if (isListening)
            {
                //Try and grab a copy of the CustListener service config file first.
                if (File.Exists(custListenerConfigPath))
                {
                    try
                    {
                        File.Copy(custListenerConfigPath, "", false);
                        //If we got to this point the copy was successful and now the eConnector has a valid config file.
                        return true;
                    }
                    catch (Exception)
                    {
                        //The copy didn't work for some reason.  Simply try to create a new file in the eConnector directory.
                    }
                }
            }
            string mySqlPassHash;
            Encryption.TryEncrypt(DataConnection.Password, out mySqlPassHash);
            return ServicesHelper.CreateServiceConfigFile(eConnectorConfigPath
                , DataConnection.Server
                , DataConnection.Database
                , DataConnection.UserID
                , DataConnection.Password
                , mySqlPassHash
                , ""
                , "");
        }

        ///<summary>Creates a default OpenDentalServiceConfig.xml file for Open Dental Service if one is not already present.
        ///Uses the current connection settings in DataConnection.  This method does NOT work if called via middle tier.
        ///Users should not be installing Open Dental Service via the middle tier.</summary>
        public static bool CreateConfigForOpenDentalService()
        {
            string odServiceConfigPath = ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(), "OpenDentalService", "OpenDentalServiceConfig.xml");
            //Check to see if there is already a config file present.
            if (File.Exists(odServiceConfigPath))
            {
                return true;//Nothing to do.
            }
            //At this point we know that Open Dental Service does not have a config file present.
            string mySqlPassHash;
            Encryption.TryEncrypt(DataConnection.Password, out mySqlPassHash);
            return ServicesHelper.CreateServiceConfigFile(odServiceConfigPath
                , DataConnection.Server
                , DataConnection.Database
                , DataConnection.UserID
                , DataConnection.Password
                , mySqlPassHash
                , ""
                , "");
        }

        ///<summary>Downloads the update files from the database and places them in the given folder. Returns false if anything went wrong.</summary>
        ///<param name="tempFolderUpdate">The temporary folder used to store the update files before being copied.</param>
        private static bool DownloadUpdateFilesFromDatabase(string tempFolderUpdate)
        {
            if (Directory.Exists(tempFolderUpdate))
            {
                try
                {
                    Directory.Delete(tempFolderUpdate, true);
                }
                catch (Exception ex)
                {
                    FormFriendlyException.Show(Lan.g("Prefs", "Unable to delete update files from local temp folder. Try closing and reopening the program."), ex);
                    FormOpenDental.ExitCode = 301;//UpdateFiles folder cannot be deleted
                    Environment.Exit(FormOpenDental.ExitCode);
                    return false;
                }
            }
            StringBuilder strBuilder = new StringBuilder();
            DocumentMisc docUpdateFilesPart = null;
            int count = 1;
            string fileName = count.ToString().PadLeft(4, '0');
            while ((docUpdateFilesPart = DocumentMiscs.GetByTypeAndFileName(fileName, DocumentMiscType.UpdateFilesSegment)) != null)
            {
                strBuilder.Append(docUpdateFilesPart.RawBase64);
                count++;
                fileName = count.ToString().PadLeft(4, '0');
            }
            ODException.SwallowAnyException(() =>
            {
                //strBuilder.ToString() has a tendency to fail when the string contains roughly 170MB of data.
                //If that becomes a typical size for our Update Files folder we should consider not storing the data as Base64.
                byte[] rawBytes = Convert.FromBase64String(strBuilder.ToString());
                using (ZipFile unzipped = ZipFile.Read(rawBytes))
                {
                    unzipped.ExtractAll(tempFolderUpdate);
                }
            });//fail silently
            return true;
        }

        ///<summary>Sets up the executable file and opens the UpdateFileCopier with the correct command line arguments passed in. Returns whether
        ///the file copier was successfully started.</summary>
        ///<param name="folderUpdate">Where the update files are stored.</param>
        ///<param name="destDir">Where the update files will be copied to.</param>
        ///<param name="doKillServices">Will tell the file copier whether to kill all Open Dental services or not.</param>
        ///<param name="useLocalUpdateFileCopier">Will use the update file copier in the local installation directory rather than the one downloaded from
        ///the server.</param>
        ///<param name="openCopiedFiles">Tells the file copier to open the copied files after completion.</param>
        public static bool OpenFileCopier(string folderUpdate, string destDir, bool doKillServices, bool useLocalUpdateFileCopier, bool openCopiedFiles)
        {
            string tempDir = PrefC.GetTempFolderPath();
            //copy UpdateFileCopier.exe to the temp directory
            //In the case of using dynamic mode, because we have modified the update file copier when we released this feature, we need to be 
            //guarenteed we are using the correct version. We know that the version in our installation directory has the updates we need as otherwise
            //they would never be able to reach these lines of code.
            string updateFileCopierLocation = "";
            if (useLocalUpdateFileCopier)
            {
                if (Application.StartupPath.Contains("DynamicMode"))
                {
                    //If they are within a different dynamic mode folder, the installation directory will be two directories up.
                    updateFileCopierLocation = Directory.GetParent(Directory.GetParent(Application.StartupPath).FullName).FullName;
                }
                else
                {
                    //Otherwise, this is the installation directory.
                    updateFileCopierLocation = Application.StartupPath;
                }
            }
            else
            {//Otherwise use the update file copier from the server.
                updateFileCopierLocation = folderUpdate;
            }
            try
            {
                File.Copy(ODFileUtils.CombinePaths(updateFileCopierLocation, "UpdateFileCopier.exe"),//source
                    ODFileUtils.CombinePaths(tempDir, "UpdateFileCopier.exe"),//dest
                    true);//overwrite
            }
            catch (Exception ex)
            {
                FormFriendlyException.Show(Lans.g("Prefs", "Unable to copy ") + "UpdateFileCopier.exe " + Lans.g("Prefs", "from ") + updateFileCopierLocation + ".", ex);
                return false;
            }
            //wait a moment to make sure the file was copied
            Thread.Sleep(500);
            //launch UpdateFileCopier to copy all files to here.
            int processId = Process.GetCurrentProcess().Id;
            string startFileName = ODFileUtils.CombinePaths(tempDir, "UpdateFileCopier.exe");
            string arguments = "\"" + folderUpdate + "\""//pass the source directory to the file copier.
                + " " + processId.ToString()//and the processId of Open Dental.
                + " \"" + destDir + "\""//and the destination directory
                + " " + doKillServices.ToString()//and whether to kill all processes or not
                + " " + openCopiedFiles.ToString();//and whether to open the copied files or not.
            try
            {
                Process proc = new Process();
                proc.StartInfo.FileName = startFileName;
                proc.StartInfo.Arguments = arguments;
                proc.Start();
                proc.WaitForExit();//Waits for the file copier to be complete.
            }
            catch (Exception ex)
            {
                FormFriendlyException.Show(Lan.g("Prefs", "Unable to start the update file copier. Try closing and reopening the program."), ex);
                FormOpenDental.ExitCode = 305;//Unable to start the UpdateFileCopier.exe process.
                Environment.Exit(FormOpenDental.ExitCode);
                return false;
            }
            return true;
        }

        /// <summary>Check for a developer only license</summary>
        public static bool IsRegKeyForTesting()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("    ");
            StringBuilder strbuild = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(strbuild, settings))
            {
                writer.WriteStartElement("RegistrationKey");
                writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
                writer.WriteEndElement();
            }
            try
            {
                string response = CustomerUpdatesProxy.GetWebServiceInstance().RequestIsDevKey(strbuild.ToString());
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                XmlNode node = doc.SelectSingleNode("//IsDevKey");
                return PIn.Bool(node.InnerText);
            }
            catch 
            {
                //They don't have an external internet connection.
                return false;
            }
        }
    }
}
