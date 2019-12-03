namespace OpenDentBusiness
{
    ///<summary>Links one orthocharttab to one displayfield.  Allows for displayfields to be part of multiple orthocharttabs.</summary>
    public class OrthoChartTabLink
    {
        public long OrthoChartTabLinkNum;

        ///<summary>Overrides the displayfield ItemOrder, so that each display field can have a different order in each ortho chart tab.</summary>
        public int ItemOrder;

        ///<summary>FK to orthocharttab.OrthoChartTabNum.</summary>
        public long OrthoChartTabNum;

        ///<summary>FK to displayfield.DisplayFieldNum.</summary>
        public long DisplayFieldNum;

        public OrthoChartTabLink Copy()
        {
            return (OrthoChartTabLink)MemberwiseClone();
        }
    }
}
