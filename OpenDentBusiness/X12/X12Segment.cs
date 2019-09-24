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
    /// Represents a X12 segment.
    /// </summary>
    public class X12Segment
    {
        private readonly X12Separators separators;

        /// <summary>
        /// The ID of the segment. Usually 2 or 3 letters. 
        /// This can also be found as the first item in the <see cref="Elements"/> property.
        /// </summary>
        public string ID { get; }

        /// <summary>
        /// Gets the segment elements.
        /// </summary>
        public string[] Elements { get; }

        /// <summary>
        /// The zero-based segment index within the X12 document.
        /// </summary>
        public int SegmentIndex { get; set; }

        /// <summary>
        /// The raw unmodified segment data.
        /// </summary>
        public string Raw { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="X12Segment"/> class.
        /// </summary>
        /// <param name="segmentData"></param>
        /// <param name="separators"></param>
        public X12Segment(string segmentData, X12Separators separators)
        {
            Raw = segmentData ?? throw new ArgumentNullException(nameof(segmentData));

            this.separators = separators;

            segmentData = segmentData.Replace(separators.Segment, "");

            Elements = segmentData.Split(char.Parse(separators.Element));

            ID = Elements[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="X12Segment"/> class.
        /// </summary>
        /// <param name="segment">The segment to copy.</param>
        public X12Segment(X12Segment segment)
        {
            ID = segment.ID;
            Elements = (string[])segment.Elements.Clone();
            SegmentIndex = segment.SegmentIndex;
            separators = segment.separators;
            Raw = segment.Raw;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="X12Segment"/> class that will represent an
        /// invalid segment or end of file segment. Useful to escape X12 loops which require a 
        /// segment to be present. The <see cref="ID"/> will be set to "INVALID", because this 
        /// string will never match a real segment ID (real segment IDs are 3 characters)
        /// </summary>
        public X12Segment() : this("INVALID", new X12Separators())
        {
        }

        /// <summary>
        /// Returns a string representation of the <see cref="X12Segment"/>.
        /// </summary>
        public override string ToString() => Raw ?? "";

        /// <summary>
        /// Creates a copy of this <see cref="X12Segment"/>.
        /// </summary>
        public X12Segment Copy() => new X12Segment(this);

        /// <summary>
        /// Returns the string representation of the given element within this segment.
        /// If the element does not exist, as can happen with optional elements, then a empty 
        /// string is returned.
        /// </summary>
        public string Get(int elementIndex) => 
            elementIndex >= 0 && elementIndex < Elements.Length ? 
                Elements[elementIndex] : "";

        /// <summary>
        /// <para>Returns the string representation of the given element, subelement within this
        /// segment. If the element or subelement does not exist, as can happen with optional 
        /// elements, then a empty string is returned.</para>
        /// <para><paramref name="subElementIndex"/> is 1-based, just like the X12 specs.</para>
        /// </summary>
        public string Get(int elementIndex, int subElementIndex)
        {
            if (elementIndex >= 0 && elementIndex < Elements.Length)
            {
                var values = Elements[elementIndex].Split(char.Parse(separators.Subelement));

                // Subtract 1 from sub element index because the value passed in will be 1-indexed...
                subElementIndex--; 

                if (subElementIndex >= 0 && subElementIndex < values.Length)
                {
                    return values[subElementIndex];
                }
            }

            return "";
        }

        /// <summary>
        /// True if the segment matches the specified segmentId, and the first element of the 
        /// segment is one of the specified values.
        /// </summary>
        public bool IsType(string segmentId, params string[] validElement01Values)
        {
            if (ID != segmentId) return false;
            
            var value = Get(1);
            for (int i = 0; i < validElement01Values.Length; i++)
            {
                if (value == validElement01Values[i])
                {
                    return true;
                }
            }

            return validElement01Values.Length == 0;
        }

        /// <summary>
        /// Verifies the segment matches the specified segmentId, and the first element of the 
        /// segment is one of the specified values.
        /// </summary>
        public void AssertType(string segmentId, params string[] validElement01Values)
        {
            if (ID != segmentId)
                throw new Exception(segmentId + " segment expected.");

            bool elementValid = false;

            var value = Get(1);
            for (int i = 0; i < validElement01Values.Length; i++)
            {
                if (value == validElement01Values[i])
                {
                    elementValid = true;
                    break;
                }
            }

            if (validElement01Values.Length > 0 && !elementValid)
                throw new Exception(
                    segmentId + " segment expected with element 01 set to one of the following values: " + string.Join(",", validElement01Values));
        }
    }
}
