using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDental.UI
{
    public class ODPrintRow
    {
        ///<summary>YPos relative to top of entire grid.  When printing this includes adjustments for page breaks.  If row has title/header the title/header should be drawn at this position.</summary>
        public int YPos;
        ///<summary>Usually only true for some grids, and only for the first row.</summary>
        public bool IsTitleRow;
        ///<summary>Usually true if row is at the top of a new page, or when changing patients in a statement grid.</summary>
        public bool IsHeaderRow;
        ///<summary>True for rows that require a bold bottom line, at end of entire grid, at page breaks, or at a separation in the grid.</summary>
        public bool IsBottomRow;
        ///<summary>Rarely true, usually only for last row in particular grids.</summary>
        public bool IsFooterRow;

        public ODPrintRow(int yPos, bool isTitleRow, bool isHeaderRow, bool isBottomRow, bool isFooterRow)
        {
            YPos = yPos;
            IsTitleRow = isTitleRow;
            IsHeaderRow = isHeaderRow;
            IsBottomRow = isBottomRow;
            IsFooterRow = isFooterRow;
        }
    }
}
