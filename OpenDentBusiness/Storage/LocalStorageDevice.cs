using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SLDental.Storage
{
    public class LocalStorageDevice : IStorageDevice
    {
        /// <summary>
        /// Combines a array of strings into a path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string CombinePath(params string[] paths) => Path.Combine(paths);

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public bool FileExists(string filePath) => File.Exists(filePath);

        public void CopyFile(string sourcePath, string destinationPath) => CopyFile(sourcePath, destinationPath, this);

        public void CopyFile(string sourcePath, string destinationPath, IStorageDevice destinationDriver)
        {
            if (destinationDriver != null)
            {
                destinationDriver.WriteAllBytes(destinationPath, ReadAllBytes(sourcePath));
            }
        }

        public bool DirectoryExists(string path) => Directory.Exists(path);

        public void CreateDirectory(string path) => Directory.CreateDirectory(path);

        public string ReadAllText(string path) => File.ReadAllText(path);

        public byte[] ReadAllBytes(string path) => File.ReadAllBytes(path);

        public void WriteAllText(string path, string contents) => File.WriteAllText(path, contents);

        public void WriteAllBytes(string path, byte[] bytes) => File.WriteAllBytes(path, bytes);

        public Stream OpenWrite(string path) => File.OpenWrite(path);

        public Stream OpenRead(string path) => File.OpenRead(path);

        public void OpenFile(string path) => Process.Start(path);

        public void OpenDirectory(string path) => OpenFile(path);

        public string[] GetFiles(string path) => Directory.GetFiles(path);

        public string[] GetDirectories(string path) => Directory.GetDirectories(path);

        public List<string> BrowseFile(string path, bool multiselect)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = path;
                openFileDialog.Multiselect = multiselect;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return new List<string>(openFileDialog.FileNames);
                }
            }

            return new List<string>();
        }
    }
}