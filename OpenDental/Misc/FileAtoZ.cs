using CodeBase;
using SLDental.Storage;
using System.Drawing;
using System.IO;

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

        public static void Copy(string sourceFileName, string destinationFileName, string uploadMessage = "Copying file...", bool isFolder = false, bool doOverwrite = false)
        {
            File.Copy(sourceFileName, destinationFileName, doOverwrite);
        }

        public static void Upload(string sourceFileName, string destinationFileName)
        {
            Copy(sourceFileName, destinationFileName, "Uploading file...");
        }
    }
}