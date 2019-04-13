namespace OpenDentalCloud.Core
{
    public abstract class TaskStateFile : TaskState
    {
        string _folder;
        string _fileName;
        byte[] _fileContent = new byte[1];
        string _fileId;

        /// <summary>
        /// If a file is greater than 2MB in size, we will break it up into chunks when uploading it to Dropbox.
        /// </summary>
        protected const int MAX_FILE_SIZE_BYTES = 2000000;

        /// <summary>
        /// The folder of the corresponding file to be downloaded
        /// </summary>
        public string Folder
        {
            get
            {
                string folder = "";
                lock (_lock)
                {
                    folder = _folder;
                }
                return folder;
            }
            set
            {
                lock (_lock)
                {
                    _folder = value;
                }
            }
        }

        /// <summary>
        /// The file name of the file to be downloaded.
        /// </summary>
        public string FileName
        {
            get
            {
                string fileName = "";
                lock (_lock)
                {
                    fileName = _fileName;
                }
                return fileName;
            }
            set
            {
                lock (_lock)
                {
                    _fileName = value;
                }
            }
        }

        /// <summary>
        /// The file stored in bytes.  This value will grow while the download is still in progress.
        /// </summary>
        public byte[] FileContent
        {
            get
            {
                byte[] fileContent = new byte[1];
                lock (_lock)
                {
                    fileContent = _fileContent;
                }
                return fileContent;
            }
            set
            {
                lock (_lock)
                {
                    _fileContent = value;
                }
            }
        }
    }
}
