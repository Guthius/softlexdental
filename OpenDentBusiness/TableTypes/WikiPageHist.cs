using System;

namespace OpenDentBusiness
{
    ///<summary>Rows never edited, just added.  Contains all historical versions of each page as well.</summary>
    public class WikiPageHist
    {
        public long WikiPageNum;

        ///<summary>FK to userod.UserNum.</summary>
        public long UserNum;

        ///<summary>Will not be unique because there are multiple revisions per page.</summary>
        public string PageTitle;

        ///<summary>The entire contents of the revision are stored in "wiki markup language".  This should never be updated.  Medtext (16M)</summary>
        public string PageContent;

        ///<summary>The DateTime from the original WikiPage object.</summary>
        public DateTime DateTimeSaved;

        ///<summary>This flag will only be set for the revision where the user marked it deleted, not the ones prior.</summary>
        public bool IsDeleted;
    }
}
