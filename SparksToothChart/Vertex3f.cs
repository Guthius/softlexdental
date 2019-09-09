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
    public class Vertex3f
    {
        public float X;
        public float Y;
        public float Z;

        public Vertex3f()
        {
        }

        public Vertex3f(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float[] GetFloatArray() => new float[] { X, Y, Z };

        public override string ToString() => X.ToString() + "," + Y.ToString() + "," + Z.ToString();

        public Vertex3f Copy() => new Vertex3f(X, Y, Z);
    }
}
