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
    class ReferralL
    {
        /// <summary>
        ///     <para>
        ///         Attempts to get the referral with the specified ID.
        ///     </para>
        ///     <para>
        ///         If no referral with the given iD could be find a error message will be 
        ///         displayed. Set to <paramref name="suppressError"/> to false to prevent the
        ///         error message from being displayed.
        ///     </para>
        /// </summary>
        /// <returns>
        ///     The referral with the specified ID; or null if no referral with the specified ID
        ///     exists.
        /// </returns>
        public static Referral GetReferral(long referralId, bool suppressError = false)
        {
            if (!Referrals.TryGetReferral(referralId, out var referral) && !suppressError)
            {
                ShowReferralErrorMsg();
            }

            return referral;
        }

        public static void ShowReferralErrorMsg()
        {
            MessageBox.Show(
                "Could not retrieve referral. Please run Database Maintenance or call support.",
                "Referrals", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }
    }
}
