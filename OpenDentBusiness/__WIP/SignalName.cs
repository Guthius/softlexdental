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

namespace OpenDentBusiness
{
    /// <summary>
    /// Names of commonly used signals.
    /// </summary>
    public static class SignalName
    {
        /// <summary>
        /// <para>This forces Softlex Dental to close on every workstation.</para>
        /// <para>
        /// Intended for scenarios where the program must be restarted on every client. For 
        /// example, this is required after a database upgrade.
        /// </para>
        /// </summary>
        public const string Shutdown = "shutdown";

        /// <summary>
        /// <para>Forces every workstation to invalidate a certain cache.</para>
        /// <para>
        /// For this signal the <see cref="Signal.Param1"/> field must contain the full name of 
        /// the <see cref="DataRecord"/> type to invalidate. If a no type name or a invalid type 
        /// name is specified, or if the specified type does not derive from 
        /// <see cref="DataRecord"/>, the signal will be ignored.
        /// </para> 
        /// </summary>
        public const string CacheInvalidate = "cache_invalidate";

        /// <summary>
        /// <para>Indicates something related to the scheduling requires a refresh.</para>
        /// </summary>
        public const string ScheduleChanged = "schedule_changed";
    }
}
