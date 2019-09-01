namespace SLDental.Storage
{
    public static class Storage
    {
        /// <summary>
        /// Gets the local storage driver.
        /// </summary>
        public static IStorageDevice Local { get; } = new LocalStorageDevice();

        /// <summary>
        /// Gets the default storage driver.
        /// </summary>
        public static IStorageDevice Default { get; } = Local;
    }
}