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