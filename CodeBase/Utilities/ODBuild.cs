namespace CodeBase
{
    public class ODBuild
    {

        /// <summary>
        /// Returns true if the current build is debug. Useful when you want the release code to show up when searching for references.
        /// </summary>
        public static bool IsDebug()
        {
#if DEBUG
            return true;
#else
			return false;
#endif
        }
    }
}