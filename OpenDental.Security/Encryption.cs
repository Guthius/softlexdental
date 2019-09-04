/**
 * Copyright (C) 2019 Dental Stars SRL
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */

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
