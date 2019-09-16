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
            DialogResult result;

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
                        Signalods.Insert(new Signalod
                        {
                            IType = InvalidType.ShutDownNow
                        });

                        Computer.ClearAllHeartBeats(Environment.MachineName);

                        isShutdownWindowNeeded = false;
                    }
                    else if (formShutdown.DialogResult == DialogResult.Cancel)
                    {
                        result =
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

            var webRequest = WebRequest.Create(downloadUri);
            WebResponse webResponse;
            try
            {
                webResponse = webRequest.GetResponse();
            }
            catch (Exception exception)
            {
                using (var msgBoxCopyPaste = new MsgBoxCopyPaste(exception.Message + "\r\nUri: " + downloadUri))
                {
                    msgBoxCopyPaste.ShowDialog();
                }

                Preference.Update(PreferenceName.UpdateInProgressOnComputerName, "");
                return;
            }

            int fileSize = (int)webResponse.ContentLength / 1024;
            var formProgress = new FormProgress
            {
                MaxVal = (double)fileSize / 1024,
                NumberMultiplication = 100,
                DisplayText = "?currentVal MB of ?maxVal MB copied",
                NumberFormat = "F"
            };

            //start the thread that will perform the download

            void downloadDelegate()
            {
                DownloadInstallPatchWorker(downloadUri, destinationPath, webResponse.ContentLength, ref formProgress);
            }

            var workerThread = new Thread(downloadDelegate);

            workerThread.Start();
            if (formProgress.ShowDialog() == DialogResult.Cancel)
            {
                workerThread.Abort();

                Preference.Update(PreferenceName.UpdateInProgressOnComputerName, "");

                formProgress.Dispose();

                return;
            }
            formProgress.Dispose();

            if (!string.IsNullOrEmpty(destinationPath2))
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

            result =
                MessageBox.Show(
                    "Download succeeded. Setup program will now begin. When done, restart the program on this computer, then on the other computers.", 
                    "Update", 
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question);

            if (result == DialogResult.Cancel)
            {
                Preference.Update(PreferenceName.UpdateInProgressOnComputerName, "");

                return;
            }

            #region Stop OpenDent Services
            //If the update has been initiated from the designated update server then try and stop all "OpenDent..." services.
            //They will be automatically restarted once Open Dental has successfully upgraded.
            if (Preference.GetString(PreferenceName.WebServiceServerName) != "" && ODEnvironment.IdIsThisComputer(Preference.GetString(PreferenceName.WebServiceServerName)))
            {
                Action actionCloseStopServicesProgress = ODProgress.Show(ODEventType.MiscData, null, "Stopping services...");
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
                    var msgBoxCopyPaste = new MsgBoxCopyPaste(
                        "The following services could not be stopped. You need to manually stop them before continuing.\r\n" + servicesNotStopped);

                    msgBoxCopyPaste.ShowDialog();
                    msgBoxCopyPaste.Dispose();
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

                MessageBox.Show(
                    "Could not launch setup.",
                    "Update", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// This is the function that the worker thread uses to actually perform the download. Can 
        /// also call this method in the ordinary way if the file to be transferred is short.
        /// </summary>
        private static void DownloadInstallPatchWorker(string downloadUri, string destinationPath, long contentLength, ref FormProgress progressIndicator)
        {
            using (var webClient = new WebClient())
            using (var stream = webClient.OpenRead(downloadUri))
            using (var fileStream = new FileStream(destinationPath, FileMode.Create))
            {
                int bytesRead;
                long position = 0;
                byte[] buffer = new byte[10 * 1024];

                try
                {
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        position += bytesRead;
                        if (position != contentLength)
                        {
                            progressIndicator.CurrentVal = ((double)position / 1024) / 1024;
                        }
                        fileStream.Write(buffer, 0, bytesRead);
                    }
                }
                catch (Exception exception)
                {
                    // Set the error message so that the user can call in and complain and we can 
                    // get more information about what went wrong. This error message will NOT show
                    // if the user hit the Cancel button and a random exception happened (because 
                    // the window will have closed).

                    progressIndicator.ErrorMessage = exception.Message;
                }
            }

            // If the file was successfully downloaded, set the progress indicator to maximum so 
            // that it closes the progress window. Otherwise leave the window open so that the 
            // error message can be displayed to the user in red text.

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