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
using System;

namespace OpenDentBusiness
{
    /// <summary>
    ///     <para>
    ///         Identifies the possible actions that can be taken on a alert. Multiple actions can 
    ///         be available for one alert.
    ///     </para>
    /// </summary>
    [Flags]
    public enum AlertActionType
    {
        None = 0,
        MarkAsRead = 1,
        OpenForm = 2,
        Delete = 4,
        ShowItemValue = 8
    }
}
