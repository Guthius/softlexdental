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
using System.Collections.Generic;

namespace OpenDentBusiness.Bridges
{
    public static class BridgeManager
    {
        private static readonly Dictionary<string, IBridge> bridges = new Dictionary<string, IBridge>();

        /// <summary>
        ///     <para>
        ///         Gets the bridge with the specified type name.
        ///     </para>
        /// </summary>
        /// <param name="typeName">The type name.</param>
        /// <returns>The bridge.</returns>
        public static IBridge GetByTypeName(string typeName)
        {
            lock (bridges)
            {
                if (bridges.TryGetValue(typeName, out var bridge))
                {
                    return bridge;
                }

                return CreateBridge(typeName);
            }
        }

        /// <summary>
        ///     <para>
        ///         Gets a new instance of <see cref="IBridge"/> of the specified type.
        ///     </para>
        /// </summary>
        /// <param name="typeName">The type name.</param>
        /// <returns>The bridge.</returns>
        private static IBridge CreateBridge(string typeName)
        {
            var type = Type.GetType(typeName);

            // TODO: Throw a exception if the bridge type is invalid.

            if (type != null)
            {
                try
                {
                    var instance = Activator.CreateInstance(type);
                    if (instance != null && instance is IBridge bridge)
                    {
                        bridges[typeName] = bridge;

                        return bridge;
                    }
                }
                catch
                {
                }
            }

            return null;
        }
    }
}
