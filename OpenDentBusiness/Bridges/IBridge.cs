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
using System.Collections.Generic;

namespace OpenDentBusiness.Bridges
{
    /// <summary>
    /// Represents a interface between OpenDental and a external application of service.
    /// </summary>
    public interface IBridge
    {
        /// <summary>
        /// Gets the name of the external program or service.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a short description of the functionality provided by the bridge.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     <para>
        ///         Sends the specified <paramref name="patient"/> data to the remote program or 
        ///         service.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient details.</param>
        void Send(long programId, Patient patient);

        /// <summary>
        /// Gets the prferences that can be configured for the bridge.
        /// </summary>
        IEnumerable<BridgePreference> Preferences { get; }
    }
}
