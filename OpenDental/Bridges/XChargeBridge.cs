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

namespace OpenDental.Bridges
{
    public class XChargeBridge : Bridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Define("force_recurring_charge", "XChargeForceRecurringCharge", BridgePreferenceType.String),
            BridgePreference.Define("prevent_saving_new_cc", "XChargePreventSavingNewCC", BridgePreferenceType.String)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="XChargeBridge"/> class.
        /// </summary>
        public XChargeBridge() : base(
            "XCharge", 
            "XCharge offers the ability to process credit card payments.",
            "http://x-charge.com/",
            preferences)
        {
        }

        /// <summary>
        /// Can't send anything directly, this does nothing.
        /// </summary>
        /// <param name="programId">T</param>
        /// <param name="patient"></param>
        public override void Send(long programId, Patient patient)
        {
        }
    }
}
