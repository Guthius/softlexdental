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
namespace SparksToothChart
{
    /// <summary>
    /// Contains one vertex (X, Y, Z), one normal, and possibly one texture coordinate.
    /// </summary>
    public class VertexNormal
    {
        public Vertex3f Vertex;
        public Vertex3f Normal;

        /// <summary>
        /// 2D, So the third value is always zero. Values are between 0 and 1. Can be null.
        /// </summary>
        public Vertex3f Texture;

        public override string ToString() => $"{Vertex},{Normal},{Texture}";

        public VertexNormal Copy()
        {
            var vertexNormal = new VertexNormal
            {
                Vertex = Vertex.Copy(),
                Normal = Normal.Copy()
            };

            if (vertexNormal.Texture != null)
            {
                vertexNormal.Texture = this.Texture.Copy();
            }

            return vertexNormal;
        }
    }
}
