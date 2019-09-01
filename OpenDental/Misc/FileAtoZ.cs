using CodeBase;
using OpenDentBusiness;
using SLDental.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OpenDental
{
    public class FileAtoZ
    {
        public static string AppendSuffix(string filePath, string suffix)
        {
            return ODFileUtils.AppendSuffix(filePath, suffix);
        }

        public static Image GetImage(string imagePath)
        {
            using (var stream = Storage.Default.OpenRead(imagePath))
            {
                return Image.FromStream(stream);
            }
        }

        ///<summary>The first parameter, 'sourceFileName', must be a file that exists.</summary>
        public static void Copy(string sourceFileName, string destinationFileName, string uploadMessage = "Copying file...", bool isFolder = false, bool doOverwrite = false)
        {
            File.Copy(sourceFileName, destinationFileName, doOverwrite);
        }

        public static void OpenDirectory(string folderName, bool doHideLocalButton = true)
        {
            Storage.Default.OpenDirectory(folderName);
        }

        ///<summary>Method to create a FormProgress. Set isSingleFile to false if copying or moving a folder.</summary>
        private static FormProgress CreateFormProgress(string displayMessage, bool isFolder = false)
        {
            FormProgress FormP = new FormProgress();
            FormP.DisplayText = displayMessage;
            if (isFolder)
            {
                FormP.NumberFormat = "";//Display whole numbers
            }
            else
            {
                FormP.NumberFormat = "F";//Display decimal places
            }
            FormP.NumberMultiplication = 1;
            FormP.MaxVal = 100;//Doesn't matter what this value is as long as it is greater than 0
            FormP.TickMS = 1000;
            return FormP;
        }


        public static void Download(string AtoZFilePath, string localFilePath, string downloadMessage = "Downloading file...")
        {
            Copy(AtoZFilePath, localFilePath, downloadMessage);
        }

        ///<summary>Uploads a local file to the A to Z folder.</summary>
        public static void Upload(string sourceFileName, string destinationFileName)
        {
            Copy(sourceFileName, destinationFileName, "Uploading file...");
        }
    }
}