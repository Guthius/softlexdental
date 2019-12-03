using System;

namespace OpenDentBusiness
{
    public class ClaimTracking
    {
        public long ClaimTrackingNum;

        ///<summary>FK to claim.ClaimNum</summary>
        public long ClaimNum;

        ///<summary>Enum:ClaimTrackingType Identifies the type of claimtracking row.</summary>
        public ClaimTrackingType TrackingType;

        ///<summary>FK to user.UserNum</summary>
        public long UserNum;

        ///<summary>When the row was inserted.</summary>
        public DateTime DateTimeEntry;

        ///<summary>Generic column for additional info.</summary>
        public string Note;

        ///<summary>FK to definition.DefNum for custom tracking when TrackingType=StatusHistory</summary>
        public long TrackingDefNum;

        ///<summary>FK to definition.DefNum for custom tracking errors when TrackingType=StatusHistory</summary>
        public long TrackingErrorDefNum;

        public ClaimTracking Copy()
        {
            return (ClaimTracking)this.MemberwiseClone();
        }
    }

    public enum ClaimTrackingType
    {
        StatusHistory,
        ClaimUser,
        ClaimProcReceived
    }
}
