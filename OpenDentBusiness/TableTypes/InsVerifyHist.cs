using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary>Inherits from insverify. A historical copy of an insurance verification record.</summary>
    public class InsVerifyHist : InsVerify
    {
        public long InsVerifyHistNum;

        ///<summary>FK to userod.UserNum.  User that was logged on when row was inserted.</summary>
        public long VerifyUserNum;

        public InsVerifyHist()
        {
        }

        public InsVerifyHist(InsVerify insVerify)
        {
            InsVerifyNum = insVerify.InsVerifyNum;
            DateLastVerified = insVerify.DateLastVerified;
            UserNum = insVerify.UserNum;
            VerifyType = insVerify.VerifyType;
            FKey = insVerify.FKey;
            DefNum = insVerify.DefNum;
            Note = insVerify.Note;
            DateLastAssigned = insVerify.DateLastAssigned;
            DateTimeEntry = insVerify.DateTimeEntry;
            HoursAvailableForVerification = insVerify.HoursAvailableForVerification;
            VerifyUserNum = Security.CurrentUser.Id;
        }
    }
}
