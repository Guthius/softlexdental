using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace CodeBase
{
    public class ODFileUtils
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetDiskFreeSpaceEx(string lpDirectoryName, out ulong lpFreebytesAvailable, out ulong lpTotalNumberOfBytes, out ulong lpTotalNumberOfFreeBytes);

        /// <summary>
        /// This is a class scope variable in order to ensure that the random value is only seeded 
        /// once for each time OD is launched. Otherwise, if instantiated more often, then the same 
        /// random numbers are generated over and over again.
        /// </summary>
        private static readonly Random _rand = new Random();

        /// <summary>
        /// Removes a trailing path separator from the given string if one exists.
        /// </summary>
        public static string RemoveTrailingSeparators(string path)
        {
            while (path != null && path.Length > 0 && (path[path.Length - 1] == '\\' || path[path.Length - 1] == '/'))
            {
                path = path.Substring(0, path.Length - 1);
            }
            return path;
        }

        [Obsolete("Use Path.Combine instead.")]
        public static string CombinePaths(string path1, string path2) => CombinePaths(new string[] { path1, path2 });

        [Obsolete("Use Path.Combine instead.")]
        public static string CombinePaths(string path1, string path2, char separator) => CombinePaths(new string[] { path1, path2 }, separator);

        [Obsolete("Use Path.Combine instead.")]
        public static string CombinePaths(string path1, string path2, string path3) => CombinePaths(new string[] { path1, path2, path3 });
        
        [Obsolete("Use Path.Combine instead.")]
        public static string CombinePaths(string path1, string path2, string path3, char separator) => CombinePaths(new string[] { path1, path2, path3 }, separator);

        [Obsolete("Use Path.Combine instead.")]
        public static string CombinePaths(string path1, string path2, string path3, string path4) => CombinePaths(new string[] { path1, path2, path3, path4 });

        /// <summary>
        /// OS independent path cominations. Ensures that each of the given path pieces are
        /// separated by the correct path separator for the current operating system. There is 
        /// guaranteed not to be a trailing path separator at the end of the returned string 
        /// (to accomodate file paths), unless the last specified path piece in the array is the 
        /// empty string.
        /// </summary>
        [Obsolete("Use Path.Combine instead.")]
        public static string CombinePaths(string[] paths)
        {
            string finalPath = "";
            for (int i = 0; i < paths.Length; i++)
            {
                string path = RemoveTrailingSeparators(paths[i]);

                // Add an appropriate slash to divide the path peices, but do not use a trailing slash on the last piece.
                if (i < paths.Length - 1)
                {
                    if (path != null && path.Length > 0)
                    {
                        path += Path.DirectorySeparatorChar;
                    }
                }
                finalPath += path;
            }
            return finalPath;
        }

        /// <summary>
        /// Ensures that each of the given path pieces are separated by the passed in separator character. 
        /// There is guaranteed not to be a trailing path separator at the end of the returned string (to accomodate file paths), 
        /// unless the last specified path piece in the array is the empty string.
        /// </summary>
        [Obsolete("Use Path.Combine instead.")]
        public static string CombinePaths(string[] paths, char separator)
        {
            return CombinePaths(paths).Replace(Path.DirectorySeparatorChar, separator);
        }

        /// <summary>
        /// Reduces image size by changing it to Jpeg format and reducing image quality to 40%.
        /// </summary>
        public static string Compress(Bitmap image)
        {
            using (var bitmap = new Bitmap(image))
            using (var memoryStream = new MemoryStream())
            {
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpgEncoder = codecs.First(x => x.FormatID == ImageFormat.Jpeg.Guid);
                Encoder encoder = Encoder.Quality;
                EncoderParameters encoderParameters = new EncoderParameters(1);
                EncoderParameter encoderParameter = new EncoderParameter(encoder, 40L);//Reduce quality to 40% of original
                encoderParameters.Param[0] = encoderParameter;
                bitmap.Save(memoryStream, jpgEncoder, encoderParameters);
                encoderParameters.Dispose();

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        /// <summary>
        /// Attempts to get the total number of free space of the drive containing the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="freeDiskSpace">Will be updated with the total number of free bytes.</param>
        /// <returns>True if the free space was retrieved succesfully; otherwise, false.</returns>
        public static bool TryGetFreeDiskSpace(string path, out ulong freeDiskSpace)
        {
            if (!path.EndsWith("\\"))
            {
                path += "\\";
            }

            return GetDiskFreeSpaceEx(path, out freeDiskSpace, out _, out _);
        }

        /// <summary>
        /// Creates a new randomly named file in the specified path with the specified extension
        /// and returns the full path of the newly created file.
        /// </summary>
        /// <returns>The full path of the new file.</returns>
        public static string CreateRandomFile(string path, string ext, string prefix = "")
        {
            while (true)
            {
                var fileName = string.Concat(prefix, Path.GetRandomFileName(), ext);

                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                {
                    continue;
                }

                try
                {
                    using (var fileStream = File.Create(fullPath))
                    {
                        return fullPath;
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Creates a new randomly named subdirectory inside the given directory path and returns the full path to the new subfolder.
        /// </summary>
        public static string CreateRandomFolder(string path)
        {
            while (true)
            {
                var fullPath = Path.Combine(path, Path.GetRandomFileName());
                if (Directory.Exists(fullPath))
                {
                    continue;
                }

                Directory.CreateDirectory(fullPath);

                return fullPath;
            }
        }

        /// <summary>
        /// Appends the suffix at the end of the file name but before the extension.
        /// </summary>
        public static string AppendSuffix(string filePath, string suffix)
        {
            var extension = Path.GetExtension(filePath);

            return 
                Path.Combine(
                    Path.GetDirectoryName(filePath), 
                    string.Concat(Path.GetFileNameWithoutExtension(filePath), suffix, extension));
        }

        /// <summary>
        /// Removes invalid characters from the passed in file name.
        /// </summary>
        public static string CleanFileName(string fileName) => string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
    }
}