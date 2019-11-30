namespace OpenDentBusiness
{
    public class Programs
    {
        /// <summary></summary>
        //public static bool UsingOrion
        //{
        //    //No need to check RemotingRole; no call to db.
        //    get
        //    {
        //        return Programs.IsEnabled(ProgramName.Orion);
        //    }
        //}

        ///<summary>Returns true if more than 1 credit card processing program is enabled.</summary>
        public static bool HasMultipleCreditCardProgramsEnabled()
        {
            return false;

            // TODO: Fix me...

            //No need to check RemotingRole; no call to db.
            //return new List<bool> {
            //    Programs.IsEnabled(ProgramName.Xcharge),Programs.IsEnabled(ProgramName.PayConnect),Programs.IsEnabled(ProgramName.PaySimple)
            //}.Count(x => x == true) >= 2;
        }
    }
}