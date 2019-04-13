namespace OpenDentalCloud.Core
{
    public abstract class TaskStateDownload : TaskStateFile
    {
        ulong _downloadFileSize;

        /// <summary>
        /// The total size of the file that is being downloaded.
        /// </summary>
        public ulong DownloadFileSize
        {
            get
            {
                ulong downloadFileSize = 0;
                lock (_lock)
                {
                    downloadFileSize = _downloadFileSize;
                }
                return downloadFileSize;
            }
            set
            {
                lock (_lock)
                {
                    _downloadFileSize = value;
                }
            }
        }
    }
}
