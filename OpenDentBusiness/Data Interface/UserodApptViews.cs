using System.Reflection;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class UserodApptViews
    {
        #region Get Methods
        #endregion

        #region Modification Methods

        #region Insert
        #endregion

        #region Update
        #endregion

        #region Delete
        #endregion

        #endregion

        #region Misc Methods
        #endregion


        ///<summary>Gets the most recent UserodApptView from the db for the user and clinic.  clinicNum can be 0.  Returns null if no match found.</summary>
        public static UserodApptView GetOneForUserAndClinic(long userNum, long clinicNum)
        {
            string command = "SELECT * FROM userodapptview "
                + "WHERE UserNum = " + POut.Long(userNum) + " "
                + "AND ClinicNum = " + POut.Long(clinicNum) + " ";//If clinicNum of 0 passed in, we MUST filter by 0 because that is a valid entry in the db.
            return Crud.UserodApptViewCrud.SelectOne(command);
        }

        public static void InsertOrUpdate(long userNum, long clinicNum, long apptViewNum)
        {
            //No need to check RemotingRole; no call to db.
            UserodApptView userodApptView = new UserodApptView();
            userodApptView.UserNum = userNum;
            userodApptView.ClinicNum = clinicNum;
            userodApptView.ApptViewNum = apptViewNum;
            //Check if there is already a row in the database for this user, clinic, and apptview.
            UserodApptView userodApptViewDb = GetOneForUserAndClinic(userodApptView.UserNum, userodApptView.ClinicNum);
            if (userodApptViewDb == null)
            {
                Insert(userodApptView);
            }
            else if (userodApptViewDb.ApptViewNum != userodApptView.ApptViewNum)
            {
                userodApptViewDb.ApptViewNum = userodApptView.ApptViewNum;
                Update(userodApptViewDb);
            }
        }

        ///<summary></summary>
        public static long Insert(UserodApptView userodApptView)
        {
            return Crud.UserodApptViewCrud.Insert(userodApptView);
        }

        ///<summary></summary>
        public static void Update(UserodApptView userodApptView)
        {
            Crud.UserodApptViewCrud.Update(userodApptView);
        }
    }
}