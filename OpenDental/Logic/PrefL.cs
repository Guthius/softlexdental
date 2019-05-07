using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace OpenDental
{
    public class PrefL
    {
        /// <summary>
        /// destinationPath includes filename (Setup.exe).
        /// destinationPath2 will create a second copy at the specified path/filename, or it will be skipped if null or empty.
        /// </summary>
        public static void DownloadInstallPatchFromURI(string downloadUri, string destinationPath, bool runSetupAfterDownload, bool showShutdownWindow, string destinationPath2)
        {
            bool isShutdownWindowNeeded = showShutdownWindow;

            while (isShutdownWindowNeeded)
            {
                using (var formShutdown = new FormShutdown())
                {
                    formShutdown.IsUpdate = true;
                    if (formShutdown.ShowDialog() == DialogResult.OK)
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
                    else if (formShutdown.DialogResult == DialogResult.Cancel)
                    {
                        var result =
                            MessageBox.Show(
                                "Are you sure you want to cancel the update?", 
                                "Update", 
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                        if (result == DialogResult.No) return;

                        continue;
                    }
                }

                // No other workstation will be able to start up until this value is reset.
                Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, Environment.MachineName);
            }

            try
            {
                File.Delete(destinationPath);
            }
            catch (Exception ex)
            {
                FormFriendlyException.Show("Error deleting file:\r\n" + ex.Message, ex);

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
                MsgBoxCopyPaste msgbox = new MsgBoxCopyPaste(ex.Message + "\r\nUri: " + downloadUri);

                msgbox.ShowDialog();

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
                Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");
                return;
            }

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
                        FormFriendlyException.Show("Error deleting file:\r\n" + ex.Message, ex);
                        Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");
                        return;
                    }
                }
                File.Copy(destinationPath, destinationPath2);
            }

            if (!runSetupAfterDownload) return;

            string msg = "Download succeeded. Setup program will now begin.  When done, restart the program on this computer, then on the other computers.";

            if (MessageBox.Show(msg, "", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                //Clicking cancel gives the user a chance to avoid running the setup program,
                Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");//unlock workstations, since nothing was actually done.
                return;
            }

            #region Stop OpenDent Services
            //If the update has been initiated from the designated update server then try and stop all "OpenDent..." services.
            //They will be automatically restarted once Open Dental has successfully upgraded.
            if (Preferences.GetString(PrefName.WebServiceServerName) != "" && ODEnvironment.IdIsThisComputer(Preferences.GetString(PrefName.WebServiceServerName)))
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
                    MsgBoxCopyPaste msgBCP = new MsgBoxCopyPaste(
                        "The following services could not be stopped.  You need to manually stop them before continuing.\r\n" + servicesNotStopped);

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
                Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName, "");
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

        /// <summary>
        /// Creates a default OpenDentalServiceConfig.xml file for Open Dental Service if one is not already present.
        /// Uses the current connection settings in DataConnection.
        /// </summary>
        public static bool CreateConfigForOpenDentalService()
        {
            var configFileName = Path.Combine(Directory.GetCurrentDirectory(), "OpenDentalService", "OpenDentalServiceConfig.xml");

            if (File.Exists(configFileName)) return true;

            Encryption.TryEncrypt(DataConnection.Password, out string passwordHash);

            return 
                ServicesHelper.CreateServiceConfigFile(
                    configFileName, 
                    DataConnection.Server, 
                    DataConnection.Database, 
                    DataConnection.UserID, 
                    DataConnection.Password, 
                    passwordHash, 
                    "", "");
        }
    }
}