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

namespace OpenDental
{
    /// <summary>
    /// Names of all the core Softlex Dental events.
    /// </summary>
    public static class EventName
    {
        /// <summary>
        /// Triggered after a user has logged in.
        /// </summary>
        public const string Login = "sys.security.login";

        /// <summary>
        /// Triggered after a user has logged out.
        /// </summary>
        public const string Logout = "sys.security.logout";
    }
}
