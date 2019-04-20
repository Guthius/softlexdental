using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class JobSprints
    {
        #region Get Methods
        ///<summary></summary>
        public static List<JobSprint> GetAll()
        {
            string command = "SELECT * FROM jobsprint";
            return Crud.JobSprintCrud.SelectMany(command);
        }

        ///<summary>Gets one JobSprint from the db.</summary>
        public static JobSprint GetOne(long jobSprintNum)
        {
            return Crud.JobSprintCrud.SelectOne(jobSprintNum);
        }
        #endregion Get Methods
        #region Modification Methods
        #region Insert
        ///<summary></summary>
        public static long Insert(JobSprint jobSprint)
        {
            return Crud.JobSprintCrud.Insert(jobSprint);
        }
        #endregion Insert
        #region Update
        ///<summary></summary>
        public static void Update(JobSprint jobSprint)
        {
            Crud.JobSprintCrud.Update(jobSprint);
        }
        #endregion Update
        #region Delete
        ///<summary></summary>
        public static void Delete(long jobSprintNum)
        {
            JobSprintLinks.DeleteForSprint(jobSprintNum);
            Crud.JobSprintCrud.Delete(jobSprintNum);
        }
        #endregion Delete
        #endregion Modification Methods
        #region Misc Methods



        #endregion Misc Methods
    }
}