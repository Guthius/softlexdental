namespace OpenDentalCloud.Core
{
    public abstract class TaskStateDelete : TaskState
    {
        string _path;

        /// <summary>
        /// The folder of the corresponding file to be deleted
        /// </summary>
        public string Path
        {
            get
            {
                string path = "";
                lock (_lock)
                {
                    path = _path;
                }
                return path;
            }
            set
            {
                lock (_lock)
                {
                    _path = value;
                }
            }
        }
    }
}
