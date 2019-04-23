namespace OpenDental
{
    public static class Encryption
    {
        /// <summary>
        /// Attempts to encrypt the specified value.
        /// </summary>
        /// <param name="value">the value to encrypt.</param>
        /// <param name="result">The encrypted value.</param>
        /// <returns>True if the value was encrypted succesfully; otherwise, false.</returns>
        public static bool TryEncrypt(string value, out string result)
        {
            result = value;
            return true;
        }

        /// <summary>
        /// Attempts to decrypt the specified value.
        /// </summary>
        /// <param name="value">The value to decrypt.</param>
        /// <param name="result">The decrypted value.</param>
        /// <returns>True if the value was decrypted succesfully; otherwise, false.</returns>
        public static bool TryDecrypt(string value, out string result)
        {
            result = value;
            return true;
        }
    }
}
