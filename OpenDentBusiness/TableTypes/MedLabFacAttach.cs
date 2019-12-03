using System;

namespace OpenDentBusiness
{
    ///<summary>Links a MedLab or a MedLabResult to a place of service.  Either the MedLabNum OR the MedLabResultNum column will be populated, never
    ///both, so this will link the facility to EITHER a MedLab OR a MedLabResult object.
    ///Every MedLab and MedLabResult will have 1 to many laboratories attached.</summary>
    public class MedLabFacAttach
    {
        public long MedLabFacAttachNum;

        ///<summary>FK to medlab.MedLabNum.</summary>
        public long MedLabNum;

        ///<summary>FK to medlabresult.MedLabResultNum.</summary>
        public long MedLabResultNum;

        ///<summary>FK to medlabfacility.MedLabFacilityNum.</summary>
        public long MedLabFacilityNum;
    }
}
