using CodeBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenDentBusiness.FileIO
{
    /// <summary>
    /// This class is used to access files in the A to Z folder. Depending on the storage type in
    /// use, it will read/write to a local location or it will download/upload from the cloud.
    /// All methods are synchronous.
    /// </summary>
    public class FileAtoZ
    {
        /// <summary>
        /// Returns the string contents of the file. Sychronous for cloud storage.
        /// </summary>
        public static string ReadAllText(string fileName)
        {
            if (CloudStorage.IsCloudStorage)
            {
                var state = CloudStorage.Download(Path.GetDirectoryName(fileName), Path.GetFileName(fileName));
                return Encoding.UTF8.GetString(state.FileContent);
            }
            else
            {
                return File.ReadAllText(fileName);
            }
        }

        /// <summary>
        /// Returns the byte contents of the file. Sychronous for cloud storage.
        /// </summary>
        public static byte[] ReadAllBytes(string fileName)
        {
            if (CloudStorage.IsCloudStorage)
            {
                var state = CloudStorage.Download(Path.GetDirectoryName(fileName), Path.GetFileName(fileName));
                return state.FileContent;
            }
            else
            {
                return File.ReadAllBytes(fileName);
            }
        }

        /// <summary>
        /// Writes or uploads the text to the specified file name. Sychronous for cloud storage.
        /// </summary>
        public static void WriteAllText(string fileName, string textForFile)
        {
            if (CloudStorage.IsCloudStorage)
            {
                CloudStorage.Upload(Path.GetDirectoryName(fileName), Path.GetFileName(fileName), Encoding.UTF8.GetBytes(textForFile));
            }
            else
            {
                File.WriteAllText(fileName, textForFile);
            }
        }

        /// <summary>
        /// Writes or uploads the bytes to the specified file name. Sychronous for cloud storage.
        /// </summary>
        public static void WriteAllBytes(string fileName, byte[] byteArray)
        {
            if (CloudStorage.IsCloudStorage)
            {
                CloudStorage.Upload(Path.GetDirectoryName(fileName), Path.GetFileName(fileName), byteArray);
            }
            else
            {
                File.WriteAllBytes(fileName, byteArray);
            }
        }

        /// <summary>
        /// Gets a list of the files in the specified directory.  Sychronous for cloud storage.
        /// </summary>
        public static List<string> GetFilesInDirectory(string folder)
        {
            if (CloudStorage.IsCloudStorage)
            {
                return CloudStorage.ListFolderContents(folder).ListFolderPathsDisplay;
            }
            return Directory.GetFiles(folder).ToList();
        }

        /// <summary>
        /// Use this instead of ODFileUtils.CombinePaths when the path is in the A to Z folder.
        /// </summary>
        public static string CombinePaths(params string[] paths) => 
            CloudStorage.PathTidy(ODFileUtils.CombinePaths(paths));

        /// <summary>
        /// Use this instead of ODFileUtils.AppendSuffix when the path is in the A to Z folder.
        /// </summary>
        public static string AppendSuffix(string filePath, string suffix) =>
            CloudStorage.PathTidy(ODFileUtils.AppendSuffix(filePath, suffix));

        /// <summary>
        /// Returns true if the file exists. If cloud, checks if the file exists in the cloud. Sychronous for cloud storage.
        /// </summary>
        public static bool Exists(string filePath)
        {
            if (CloudStorage.IsCloudStorage)
            {
                return CloudStorage.FileExists(filePath);
            }
            return File.Exists(filePath);
        }

        /// <summary>
        /// The first parameter, 'sourceFileName', must be a file that exists. Sychronous for cloud storage.
        /// </summary>
        public static void Copy(string sourceFileName, string destinationFileName, FileAtoZSourceDestination sourceDestination, bool isFolder = false, bool doOverwrite = false)
        {
            if (CloudStorage.IsCloudStorage)
            {
                sourceFileName = CloudStorage.PathTidy(sourceFileName);
                destinationFileName = CloudStorage.PathTidy(destinationFileName);
                OpenDentalCloud.TaskState state;
                if (sourceDestination == FileAtoZSourceDestination.AtoZToAtoZ)
                {
                    state = CloudStorage.Copy(sourceFileName, destinationFileName);
                }
                else if (sourceDestination == FileAtoZSourceDestination.LocalToAtoZ)
                {
                    state = CloudStorage.Upload(Path.GetDirectoryName(destinationFileName), Path.GetFileName(destinationFileName),
                        File.ReadAllBytes(sourceFileName));
                }
                else if (sourceDestination == FileAtoZSourceDestination.AtoZToLocal)
                {
                    state = CloudStorage.Download(Path.GetDirectoryName(sourceFileName), Path.GetFileName(sourceFileName));
                }
                else
                {
                    throw new Exception("Unsupported " + nameof(FileAtoZSourceDestination) + ": " + sourceDestination);
                }
                if (sourceDestination == FileAtoZSourceDestination.AtoZToLocal)
                {
                    File.WriteAllBytes(destinationFileName, ((OpenDentalCloud.Core.TaskStateDownload)state).FileContent);
                }
            }
            else
            {
                File.Copy(sourceFileName, destinationFileName, doOverwrite);
            }
        }

        /// <summary>
        /// Deletes the file. Sychronous for cloud storage.
        /// </summary>
        public static void Delete(string fileName)
        {
            if (CloudStorage.IsCloudStorage)
            {
                CloudStorage.Delete(fileName);
            }
            else
            {
                File.Delete(fileName);
            }
        }

        /// <summary>
        /// Returns true if the directory exists. If cloud, checks if that directory exists in the cloud. Sychronous for cloud storage.
        /// </summary>
        public static bool DirectoryExists(string folderName)
        {
            if (CloudStorage.IsCloudStorage)
            {
                return CloudStorage.FileExists(folderName);
            }
            return Directory.Exists(folderName);
        }

        /// <summary>
        /// Returns null if the the image could not be downloaded. Sychronous.
        /// </summary>
        public static Bitmap GetImage(string imagePath)
        {
            if (CloudStorage.IsCloudStorage)
            {
                var state = CloudStorage.Download(Path.GetDirectoryName(imagePath), Path.GetFileName(imagePath));
                if (state == null || state.FileContent == null || state.FileContent.Length < 2)
                {
                    return null;
                }

                using (var memoryStream = new MemoryStream(state.FileContent))
                {
                    return new Bitmap(Image.FromStream(memoryStream));
                }
            }

            return new Bitmap(Image.FromFile(imagePath));
        }

        ///<summary>Returns null if the the image could not be downloaded. Sychronous.</summary>
        public static Bitmap GetThumbnail(string imagePath, int size = 128)
        {
            if (CloudStorage.IsCloudStorage)
            {
                var state = CloudStorage.GetThumbnail(Path.GetDirectoryName(imagePath), Path.GetFileName(imagePath));
                if (state == null || state.FileContent == null || state.FileContent.Length < 2)
                {
                    return null;
                }

                using (var memoryStream = new MemoryStream(state.FileContent))
                {
                    return ImageHelper.GetThumbnail(Image.FromStream(memoryStream), size);
                }
            }

            return ImageHelper.GetThumbnail(Image.FromFile(imagePath), size);
        }
    }

    /// <summary>
    /// A class that does the same thing that FileAtoZ does and implements the IFileAtoZ interface.
    /// </summary>
    public class FileAtoZInst : IFileAtoZ
    {
        public string AppendSuffix(string filePath, string suffix) =>
            FileAtoZ.AppendSuffix(filePath, suffix);

        public string CombinePaths(params string[] paths) => 
            FileAtoZ.CombinePaths(paths);
        

        public void Copy(string sourceFileName, string destinationFileName, FileAtoZSourceDestination sourceDestination, string uploadMessage = "Copying file...", bool isFolder = false, bool doOverwrite = false)
        {
            FileAtoZ.Copy(sourceFileName, destinationFileName, sourceDestination, isFolder);
        }

        public void Delete(string fileName) => 
            FileAtoZ.Delete(fileName);

        public bool DirectoryExists(string folderName) => 
            FileAtoZ.DirectoryExists(folderName);

        public bool Exists(string filePath) => 
            FileAtoZ.Exists(filePath);

        public List<string> GetFilesInDirectory(string folder) =>
            FileAtoZ.GetFilesInDirectory(folder);

        public Image GetImage(string imagePath) => 
            FileAtoZ.GetImage(imagePath);

        public string ReadAllText(string fileName) => 
            FileAtoZ.ReadAllText(fileName);

        public void WriteAllText(string fileName, string textForFile) => 
            FileAtoZ.WriteAllText(fileName, textForFile);
    }
}

namespace OpenDentBusiness
{
    /// <summary>Used to specify where the files are coming from and going when copying.</summary>
    public enum FileAtoZSourceDestination
    {
        /// <summary>Copying a local file to AtoZ folder. Equivalent to 'upload.'</summary>
        LocalToAtoZ,
        
        /// <summary>Copying an AtoZ file to a local file. Equivalent to 'download'.</summary>
        AtoZToLocal,
        
        /// <summary>Copying an AtoZ file to another AtoZ file. Equivalent to 'download' then 'upload'.</summary>
        AtoZToAtoZ
    }
}