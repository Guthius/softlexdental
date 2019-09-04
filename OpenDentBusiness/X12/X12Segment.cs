using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness
{

    /// <summary>
    /// Represents a X12 segment.
    /// </summary>
    public class X12Segment
    {
        ///<summary>Usually 2 or 3 letters. Can also be found at Elements[0].</summary>
        public string SegmentID;

        ///<summary></summary>
        public string[] Elements;

        ///<summary>The zero-based segment index within the X12 document.</summary>
        public int SegmentIndex;

        ///<summary></summary>
        private X12Separators Separators;
        public string RawText;

        /// <summary>
        /// Initializes a new instance of the <see cref="X12Segment"/> class.
        /// </summary>
        /// <param name="segmentData"></param>
        /// <param name="separators"></param>
        public X12Segment(string segmentData, X12Separators separators)
        {
            RawText = segmentData ?? throw new ArgumentNullException(nameof(segmentData));

            Separators = separators;
            //first, remove the segment terminator
            segmentData = segmentData.Replace(separators.Segment, "");
            //then, split the row into elements, eliminating the DataElementSeparator
            Elements = RawText.Split(Char.Parse(separators.Element));
            SegmentID = Elements[0];
        }

        public X12Segment(X12Segment segment)
        {
            SegmentID = segment.SegmentID;
            Elements = (string[])segment.Elements.Clone();
            SegmentIndex = segment.SegmentIndex;
            Separators = segment.Separators;
            RawText = segment.RawText;
        }

        /// <summary>
        /// The segment will represent an invalid segment or end of file segment.
        /// Useful to escape X12 loops which require a segment to be present.
        /// The SegmentID will be set to "INVALID", because this string will never match a real segment ID (real segment IDs are 3 characters)
        /// </summary>
        public X12Segment() : this("INVALID", new X12Separators())
        {
        }

        public override string ToString() => RawText;

        public X12Segment Copy()
        {
            return new X12Segment(this);
        }

        /// <summary>
        /// Returns the string representation of the given element within this segment.
        /// If the element does not exist, as can happen with optional elements, then "" is returned.
        /// </summary>
        public string Get(int elementPosition)
        {
            if (Elements.Length <= elementPosition)
            {
                return "";
            }
            return Elements[elementPosition];
        }

        /// <summary>
        /// Returns the string representation of the given element, subelement within this segment.
        /// If the element or subelement does not exist, as can happen with optional elements, then "" is returned.
        /// Subelement is 1-based, just like the x12 specs.
        /// </summary>
        public string Get(int elementPosition, int subelementPosition)
        {
            if (Elements.Length <= elementPosition)
            {
                return "";
            }
            string[] subelements = Elements[elementPosition].Split(Char.Parse(Separators.Subelement));
            //example, subelement passed in is 2.  Convert to 0-indexed means [1].  If Length < 2, then we have a problem.
            if (subelements.Length < subelementPosition)
            {
                return "";
            }
            return subelements[subelementPosition - 1];
        }

        /// <summary>
        /// True if the segment matches the specified segmentId, and the first element of the segment is one of the specified values.
        /// </summary>
        public bool IsType(string segmentId, params string[] arrayElement01Values)
        {
            if (SegmentID != segmentId) return false;
            

            bool isElement01Valid = false;
            string element01 = Get(1);
            for (int i = 0; i < arrayElement01Values.Length; i++)
            {
                if (element01 == arrayElement01Values[i])
                {
                    isElement01Valid = true;
                    break;
                }
            }
            if (arrayElement01Values.Length > 0 && !isElement01Valid)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Verifies the segment matches the specified segmentId, and the first element of the segment is one of the specified values.
        /// </summary>
        public void AssertType(string segmentId, params string[] arrayElement01Values)
        {
            if (SegmentID != segmentId)
            {
                throw new ApplicationException(segmentId + " segment expected.");
            }
            bool isElement01Valid = false;

            string element01 = Get(1);
            for (int i = 0; i < arrayElement01Values.Length; i++)
            {
                if (element01 == arrayElement01Values[i])
                {
                    isElement01Valid = true;
                    break;
                }
            }

            if (arrayElement01Values.Length > 0 && !isElement01Valid)
            {
                throw new ApplicationException(
                    segmentId + " segment expected with element 01 set to one of the following values: " + string.Join(",", arrayElement01Values));
            }
        }
    }
}
