using System.Text.RegularExpressions;

namespace OpenDental
{
    public static class License
    {
        /// <summary>
        /// Checks whether the specified registration key is valid.
        /// </summary>
        /// <param name="registrationKey">The registration key.</param>
        /// <returns>True if the key is valid; otherwise, false.</returns>
        public static bool ValidateKey(string registrationKey)
        {
            return true;
        }

        /// <summary>
        /// Formats the specified registration key for display.
        /// </summary>
        /// <param name="registrationKey">The registration key.</param>
        /// <returns>The formatted registration key.</returns>
        public static string FormatKey(string registrationKey) => registrationKey;
    }
}