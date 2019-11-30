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
using OpenDentBusiness.Bridges;
using System.Windows.Forms;

namespace OpenDental
{
    /// <summary>
    /// This class is for the bridge to DentalXCharge. Specifically, this is for patient credit Score.
    /// </summary>
    public class DentalXChangeBridge : Bridge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DentalXChangeBridge"/> class.
        /// </summary>
        public DentalXChangeBridge() : base("DentalXChange", "")
        {
        }

        /// <summary>
        ///     <para>
        ///         Sends the specified <paramref name="patient"/> data to the remote program or 
        ///         service.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient details.</param>
        public override void Send(long programId, Patient patient)
        {
#if DEBUG
            string url = "https://prelive.dentalxchange.com";
#else
            string url ="https://register.dentalxchange.com";
#endif
            url += "/reg/pmslogin";

            var clearingHouse = Clearinghouses.GetFirstOrDefault(x => x.CommBridge == EclaimsCommBridge.ClaimConnect);
            if (clearingHouse == null)
            {
                MessageBox.Show(
                    "ClaimConnect clearinghouse not found.", 
                    Name, 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            var formWebBrowser = 
                new FormWebBrowser(url, "",
                    $"username={clearingHouse.LoginID}&pwd={clearingHouse.Password}&app=pfs&pagename=creditcheck", 
                    "Content-Type: application/x-www-form-urlencoded\r\n");

            formWebBrowser.Show();
        }
    }
}
