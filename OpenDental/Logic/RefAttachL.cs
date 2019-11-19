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
using System.Collections.Generic;

namespace OpenDental
{
    public class RefAttachL
    {
        /// <summary>
        ///     <para>
        ///         Pass in all the refattaches for the patient. This funtion finds the first 
        ///         referral from a Dr and returns that Dr's name. Used in specialty practices. 
        ///         Function is only used right now in the Dr. Ceph bridge.
        ///     </para>
        /// </summary>
        public static string GetReferringDr(List<RefAttach> attachList)
        {
            if (attachList.Count == 0 || attachList[0].RefType != ReferralType.RefFrom)
            {
                return "";
            }

            var referral = ReferralL.GetReferral(attachList[0].ReferralNum);
            if (referral == null || referral.PatNum != 0)
            {
                return "";
            }

            string result = referral.FName + " " + referral.MName + " " + referral.LName;
            if (referral.Title != "")
            {
                result += ", " + referral.Title;
            }

            // TODO: Add a property to Referral that generates this full name.

            return result;
        }
    }
}
