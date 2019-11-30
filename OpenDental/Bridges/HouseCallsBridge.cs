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
using OpenDentBusiness;
using OpenDentBusiness.Bridges;

namespace OpenDental.Bridges
{
    public class HouseCallsBridge : Bridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.Custom("export_path", "Export Path", BridgePreferenceType.FolderPath)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="HouseCallsBridge"/> class.
        /// </summary>
        public HouseCallsBridge() : base("HouseCalls", "", "", preferences)
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
            using (var formHouseCalls = new FormHouseCalls(programId, !IsUsingChartNumber(programId)))
            {
                formHouseCalls.ShowDialog();
            }
        }
    }
}
