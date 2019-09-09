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
using System.Collections.Generic;

namespace SparksToothChart
{
    /// <summary>
    /// A face is a single polygon, usually a rectangle. 
    /// Will soon be only triangles.
    /// </summary>
    public class Face
    {
        /// <summary>
        /// A list of indices to the VertexNormal list contained in the ToothGraphic object. 
        /// 0 indexed, unlike the raw files which are 1 indexed. Always exactly 3 for WPF.
        /// </summary>
        public List<int> IndexList;

        public Face() => IndexList = new List<int>();

        public override string ToString() => string.Join(",", IndexList);

        public Face Copy()
        {
            return new Face
            {
                IndexList = new List<int>(IndexList)
            };
        }
    }
}
