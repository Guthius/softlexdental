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
                        Computer.ClearAllHeartBeats(Environment.MachineName);//always assume success
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
                Preference.Update(PreferenceName.UpdateInProgressOnComputerName, Environment.MachineName);
            }

            try
            {
                File.Delete(destinationPath);
            }
            catch (Exception ex)
            {
                FormFriendlyException.Show("Error deleting file:\r\n" + ex.Message, ex);

                Preference.Update(PreferenceName.UpdateInProgressOnComputerName, "");
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

                Preference.Update(PreferenceName.UpdateInProgressOnComputerName, "");
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
                Preference.Update(PreferenceName.UpdateInProgressOnComputerName, "");
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
                        Preference.Update(PreferenceName.UpdateInProgressOnComputerName, "");
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
                Preference.Update(PreferenceName.UpdateInProgressOnComputerName, "");//unlock workstations, since nothing was actually done.
                return;
            }

            #region Stop OpenDent Services
            //If the update has been initiated from the designated update server then try and stop all "OpenDent..." services.
            //They will be automatically restarted once Open Dental has successfully upgraded.
            if (Preference.GetString(PreferenceName.WebServiceServerName) != "" && ODEnvironment.IdIsThisComputer(Preference.GetString(PreferenceName.WebServiceServerName)))
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
                Preference.Update(PreferenceName.UpdateInProgressOnComputerName, "");
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
            {
                try
                {
                    File.Delete(destinationPath);
                }
                catch
                {
                }
            }
        }
    }
}