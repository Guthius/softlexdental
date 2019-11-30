/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
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
using OpenDentBusiness;
using System.Windows.Forms;

namespace OpenDental
{
    /// <summary>
    ///     TODO: Merge this with the ClearingHouse DTO class.
    /// </summary>
    public class ClearinghouseL
    {
        /// <summary>
        ///     <para>
        ///         Returns the clearinghouse with the specified ID.
        ///     </para>
        ///     <para>
        ///         Will only return an HQ-level clearinghouse. Do not attempt to pass in a 
        ///         clinic-level <paramref name="clearinghouseId"/>.
        ///     </para>
        /// </summary>
        /// <param name="clearinghouseId">The ID of the clearinghouse.</param>
        public static Clearinghouse GetClearinghouseHq(long clearinghouseId) => GetClearinghouseHq(clearinghouseId, false);

        /// <summary>
        ///     <para>
        ///         Returns the clearinghouse with the specified ID.
        ///     </para>
        ///     <para>
        ///         Will only return an HQ-level clearinghouse. Do not attempt to pass in a 
        ///         clinic-level <paramref name="clearinghouseId"/>.
        ///     </para>
        ///     <para>
        ///         Can return null if no match found.
        ///     </para>
        /// </summary>
        /// <param name="clearinghouseId">The ID of the clearinghouse.</param>
        /// <param name="suppressError">
        ///     Value indicating whether the error message indicating no match was found should be
        ///     suppressed.
        /// </param>
        public static Clearinghouse GetClearinghouseHq(long clearinghouseId, bool suppressError)
        {
            var clearinghouse = Clearinghouses.GetClearinghouse(clearinghouseId);

            if (clearinghouse == null && !suppressError)
            {
                MessageBox.Show(
                    "Error. Could not locate clearinghouse.", 
                    "Clearinghouses", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }

            return clearinghouse;
        }

        /// <summary>
        ///     <para>
        ///         Gets the description of the clearinghouse with the specified ID.
        ///     </para>
        /// </summary>
        /// <param name="clearinghouseId">The ID of the clearinghouse.</param>
        /// <returns>
        ///     The description of the clearinghouse with the specified ID or a empty string
        ///     if the clearinghouse was not found.
        /// </returns>
        public static string GetDescription(long clearinghouseId) => GetClearinghouseHq(clearinghouseId)?.Description ?? "";
    }
}