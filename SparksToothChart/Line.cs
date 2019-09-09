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
    /// A series of vertices that are all connected into one continuous simple line.
    /// </summary>
    public class LineSimple
    {
        public List<Vertex3f> Vertices;

        public LineSimple()
        {
            Vertices = new List<Vertex3f>();
        }

        /// <summary>
        /// Specify a line as a series of points. It's implied that they are grouped by threes.
        /// </summary>
        public LineSimple(params float[] coords)
        {
            Vertices = new List<Vertex3f>();

            var vertex = new Vertex3f();
            for (int i = 0; i < coords.Length; i++)
            {
                vertex.X = coords[i];
                i++;
                vertex.Y = coords[i];
                i++;
                vertex.Z = coords[i];
                Vertices.Add(vertex);
                vertex = new Vertex3f();
            }
        }
    }
}
