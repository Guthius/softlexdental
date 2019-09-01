using System.Collections.Generic;
using System.IO;

namespace SLDental.Storage
{
    public interface IStorageDevice
    {
        string CombinePath(params string[] path);

        void DeleteFile(string filePath);

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="filePath">The full path of the file.</param>
        /// <returns>True if the file exists; otherwise, false.</returns>
        bool FileExists(string filePath);

        void CopyFile(string sourcePath, string destinationPath);

        void CopyFile(string sourcePath, string destinationPath, IStorageDevice destinationDriver);

        bool DirectoryExists(string path);

        void CreateDirectory(string path);

        string ReadAllText(string path);

        byte[] ReadAllBytes(string path);

        void WriteAllText(string path, string contents);

        void WriteAllBytes(string path, byte[] bytes);

        Stream OpenWrite(string path);

        Stream OpenRead(string path);

        void OpenFile(string path);

        void OpenDirectory(string path);

        string[] GetFiles(string path);

        string[] GetDirectories(string path);

        List<string> BrowseFile(string path, bool multiselect);
    }
}
