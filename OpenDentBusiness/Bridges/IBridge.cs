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
using System;

namespace OpenDentBusiness.Bridges
{
    /// <summary>
    /// Represents a interface between OpenDental and a external application of service.
    /// </summary>
    public interface IBridge
    {
        /// <summary>
        /// Gets the name of the external service.
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// Gets a short description of the functionality provided by the bridge.
        /// </summary>
        string Description { get; }
    }

    public class Bridge : IBridge
    {
        /// <summary>
        /// Gets the name of the external service.
        /// </summary>
        public string ServiceName { get; }

        /// <summary>
        /// Gets a short description of the functionality provided by the bridge.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bridge"/> class.
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="description"></param>
        public Bridge(string serviceName, string description = "")
        {
            ServiceName = serviceName ?? 
                throw new ArgumentNullException(nameof(serviceName));

            Description = description ?? "";
        }
    }
}
