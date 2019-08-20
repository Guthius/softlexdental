namespace CodeBase
{
    public class ODMethodsT
    {
        /// <summary>
        /// Returns a new instance of T if input is null.
        /// </summary>
        public static T Coalesce<T>(T input) where T : class, new() => input ?? new T();

        /// <summary>
        /// Returns the specified defaultValue of T if input is null.
        /// </summary>
        public static T Coalesce<T>(T input, T defaultVal) where T : class => input ?? defaultVal;
    }
}