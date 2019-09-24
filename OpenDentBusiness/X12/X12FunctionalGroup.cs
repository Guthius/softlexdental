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
    /// GS/GE combination.
    /// Contained within an interchange control combination (ISA/IEA).
    /// Contains at least one transaction (ST/SE).
    /// </summary>
    public class X12FunctionalGroup
    {
        /// <summary>
        /// A collection of X12Transactions. ST segments.
        /// </summary>
        public List<X12Transaction> Transactions { get; } = new List<X12Transaction>();

        /// <summary>
        /// The segment that identifies this functional group
        /// </summary>
        public X12Segment Header { get; }

        /// <summary>
        /// Initializes a new instance fo the <see cref="X12FunctionalGroup"/> class.
        /// </summary>
        public X12FunctionalGroup()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="X12FunctionalGroup"/> class.
        /// </summary>
        public X12FunctionalGroup(X12Segment header) => Header = header.Copy();
    }
}
