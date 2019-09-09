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

namespace OpenDental.UI
{
    /// <summary>
    /// Specifies the selection behavior of an <see cref="ODGrid"/>.
    /// </summary>
    public enum GridSelectionMode
    {
        /// <summary>
        /// No items can be selected.
        /// </summary>  
        None,

        /// <summary>
        /// Only one row can be selected.
        /// </summary>  
        Row,

        /// <summary>
        /// Only one cell can be selected.
        /// </summary>
        Cell,

        /// <summary>
        /// Multiple items can be selected, and the user can use the SHIFT, CTRL, and arrow keys to make selections
        /// </summary>   
        Multiple,
    }
}
