using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using UpdateFileCopier.Properties;

namespace UpdateFileCopier
{
    public partial class FormMain : Form
    {
        SynchronizationContext synchronizationContext;

        /// <summary>
        /// If anything goes wrong with the file copying, this string should be used to hold more details about the error.
        /// </summary>
        string error = "";

        /// <summary>
        /// The source path for the copy.
        /// </summary>
        string sourcePath;

        /// <summary>
        /// The destination path for the copy.
        /// </summary>
        string destPath;

        /// <summary>
        /// This indicates whether the file copier should kill all services before copying files.
        /// </summary>
        bool killServices = true;

        /// <summary>
        /// Indicates whether Open Dental will be launched after copying is complete.
        /// </summary>
        bool launchOpenDental = true;

        /// <summary>
        /// Gets the temporary path to use for backups.
        /// </summary>
        string TempPath => Path.Combine(Path.GetTempPath(), "OpenDental", "UpdateFileCopier");

        /// <summary>
        /// Initializes a new instance of the <see cref="FormMain"/> class.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destPath">The destination path.</param>
        /// <param name="killServices">Value indicating whether to kill active Open Dental processes.</param>
        /// <param name="launchOpenDental">Value indicating whether to launch Open Dental after copy.</param>
        public FormMain(string sourcePath, string destPath, bool killServices, bool launchOpenDental)
        {
            InitializeComponent();

            this.sourcePath = sourcePath;
            this.destPath = destPath;
            this.killServices = killServices;
            this.launchOpenDental = launchOpenDental;
        }

        /// <summary>
        /// Start the copy process once the form is shown for the first time.
        /// </summary>
        void FormMain_Shown(object sender, EventArgs e) => retryButton_Click(this, EventArgs.Empty);

        /// <summary>
        /// Kills the process with the specified name.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        static void KillProcess(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            for (int i = 0; i < processes.Length; i++)
            {
                try
                {
                    processes[i].Kill();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Runs the file copy operation.
        /// </summary>
        void Run()
        {
            synchronizationContext.Send(_ => statusLabel.Text = Resources.LangPreparingToCopyFiles, null);

            // Delay the thread for 300 milliseconds to make sure the above processes have really exited.
            Thread.Sleep(300);

            // Check if any of the corresponding files in the installation directory are in use.
            if (!CreateTemporaryDirectories() || HasFilesInUse(sourcePath, destPath))
            {
                synchronizationContext.Post(_ =>
                {
                    Cursor = Cursors.Default;

                    if (!string.IsNullOrEmpty(error))
                    {
                        statusLabel.Text = error + "\r\n\r\n" +
                            Resources.LangThereAreFilesInUse;
                    }
                    else
                    {
                        statusLabel.Text = 
                            Resources.LangThereAreFilesInUse;
                    }

                    retryButton.Visible = true;

                }, null);

                return;
            }

            // Create a backup of all files in the destination directory.
            if (!CopyFiles(destPath, TempPath, Resources.LangStatusBackingUp))
            {
                synchronizationContext.Post(_ =>
                {
                    Cursor = Cursors.Default;

                    statusLabel.Text = Resources.LangSomeFilesFailedToCopy;

                    retryButton.Visible = true;

                }, null);

                return;
            }

            // Copy the files to the target directory.
            if (!CopyFiles(sourcePath, destPath, Resources.LangStatusCopying, true))
            {
                // Revert the files in the destination directory back to the way they were before trying to update them.
                CopyFiles(TempPath, destPath, Resources.LangStatusReverting, true);

                // Display a error message.
                synchronizationContext.Post(_ =>
                {

                    statusLabel.Text = Resources.LangSomeFilesFailedToCopy;

                    retryButton.Visible = true;

                }, null);

                return;
            }

            // Copy was a success, perform cleanup.
            CleanUp();

            // Start Open Dental.
            synchronizationContext.Send(_ =>
            {
                Cursor = Cursors.Default;

                // Everything copied correctly, try and launch Open Dental.
                statusLabel.Text = Resources.LangDone;

                // Wait for a small amount of time before launching...
                Thread.Sleep(300);
                try
                {
                    if (launchOpenDental)
                    {
                        Process.Start(Path.Combine(destPath, "OpenDental.exe"));
                    }
                }
                catch
                {
                }

                Application.Exit();

            }, null);
        }

        /// <summary>
        /// Tries to recreate the two temporary directories that will be used in the copying process.
        /// Returns false if deleting or creating the directories fails.
        /// </summary>
        bool CreateTemporaryDirectories()
        {
            try
            {
                if (Directory.Exists(TempPath))
                {
                    Directory.Delete(TempPath, true);
                }
                Directory.CreateDirectory(TempPath);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Clean up after copying has completed. Deletes temporary directories.
        /// </summary>
        void CleanUp()
        {
            synchronizationContext.Send(_ => statusLabel.Text = Resources.LangCleaningUp, null);
            try
            {
                if (Directory.Exists(TempPath))
                {
                    Directory.Delete(TempPath, true);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Checks whether any of the files we are going to overwrite in the destination direction
        /// are currently in use. If so we can't perform the copy.
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        /// <returns>True if there are files in use; otherwise, false.</returns>
        bool HasFilesInUse(string sourcePath, string destPath)
        {
            try
            {
                var sourceFileNames = Directory.GetFiles(sourcePath);
                foreach (var sourceFileName in sourceFileNames)
                {
                    var destFileName = Path.Combine(destPath, Path.GetFileName(sourceFileName));
                    if (File.Exists(destFileName))
                    {
                        try
                        {
                            using (var stream = File.OpenRead(destFileName))
                            {
                                stream.Close();

                                // The quickest, easiest way to tell if a file is already in use is to simply try to open it with write permissions.
                                // If the file is in use and we try to open it for writing, then an exception will occur.  We also need read permissions to copy.
                                // If successful, the stream will close right away and move on to the next file. This does not combat thread race conditions. 
                                // That is an impossible issue to combat (no such thing as an accurate "file in use" check).
                            }
                        }
                        catch (Exception ex)
                        {
                            error = string.Format(Resources.LangUnableToAccess, destFileName) + " " + ex.Message;

                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Copies all files from the source path to the destination path.
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        /// <param name="labelText"></param>
        /// <param name="overwrite"></param>
        /// <returns>True if all files were copied succesfully; otherwise, false.</returns>
        bool CopyFiles(string sourcePath, string destPath, string labelText, bool overwrite = false)
        {
            try
            {
                var sourceFiles = Directory.GetFiles(sourcePath);
                foreach (var sourceFileName in sourceFiles)
                {
                    synchronizationContext.Send(_ => statusLabel.Text = string.Format(labelText, Path.GetFileName(sourceFileName)), null);

                    var destFileName = Path.Combine(destPath, Path.GetFileName(sourceFileName));

                    File.Copy(sourceFileName, destFileName, overwrite);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Starts the fily copy action.
        /// </summary>
        void retryButton_Click(object sender, EventArgs e)
        {
            retryButton.Visible = false;

            Cursor = Cursors.WaitCursor;

            // Kill all the Open Dental services that might be active and keeping files in use.
            if (killServices)
            {
                KillProcess("OpenDental");
                KillProcess("WebCamOD");
                KillProcess("ProximityOD");
                KillProcess("CentralManager");
                KillProcess("DatabaseIntegrityCheck");
                KillProcess("ServiceManager");
            }

            synchronizationContext = SynchronizationContext.Current;

            new Thread(Run).Start();
        }
    }
}