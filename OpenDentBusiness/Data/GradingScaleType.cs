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
    /// Used in GradingScale to determine how grades are assigned.
    /// </summary>
    public enum GradingScaleType
    {
        /// <summary>
        /// User-Defined list of possible grades. Grade is calculated as an average.
        /// </summary>
        PickList = 0,

        /// <summary>
        /// Percentage Scale 0-100. Grade is calculated as an average.
        /// </summary>
        Percentage = 1,

        /// <summary>
        /// Allows point values for grades. Grade is calculated as a sum of all points out of points possible.
        /// </summary>
        Weighted = 2
    }
}
