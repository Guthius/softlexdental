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

namespace OpenDentBusiness.X12
{
    /// <summary>
    /// ST/SE combination.
    /// Containted within functional group (GS/GE).
    /// In claims, there will be one transaction per carrier.
    /// </summary>
    public class X12Transaction
    {
        /// <summary>
        /// A collection of all the X12Segments for this transaction, in the order they originally appeared.
        /// </summary>
        public List<X12Segment> Segments { get; } = new List<X12Segment>();

        /// <summary>
        /// The segment that identifies this functional group
        /// </summary>
        public X12Segment Header { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="X12Transaction"/> class.
        /// </summary>
        public X12Transaction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="X12Transaction"/> class.
        /// </summary>
        public X12Transaction(X12Segment header) => Header = header.Copy();

        /// <summary>
        /// Gets the segment with the specified ID.
        /// </summary>
        /// <param name="segmentId">The ID of the segment.</param>
        /// <returns>The segment with the specified ID or null if no segment with the specified ID exists.</returns>
        public X12Segment GetSegmentByID(string segmentId)
        {
            foreach (var segment in Segments)
            {
                if (segment.ID == segmentId)
                {
                    return segment;
                }
            }

            return null;
        }
    }
}
