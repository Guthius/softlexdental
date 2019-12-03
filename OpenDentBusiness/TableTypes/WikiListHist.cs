using System;

namespace OpenDentBusiness
{
    ///<summary>Rows never edited, just added.  Contains all historical versions of each list.</summary>
    public class WikiListHist
    {
        public long WikiListHistNum;

        ///<summary>FK to userod.UserNum.</summary>
        public long UserNum;

        ///<summary>Will not be unique because there are multiple revisions per page.</summary>
        public string ListName;

        ///<summary>The contents of the corresponding WikiListHeaderWidths row converted to a string in format ColName1,ColWidth1;ColName2,ColWidth2;...  Database type text/varChar2(4000) (65K/4K)</summary>
        public string ListHeaders;

        ///<summary>The entire contents of the revision are stored as XML.  Database type mediumtext/clob (16M,4G)</summary>
        public string ListContent;

        ///<summary>The DateTime from the original WikiPage object.</summary>
        public DateTime DateTimeSaved;
    }
}
