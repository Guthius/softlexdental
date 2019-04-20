using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class JobControls
    {

        ///<summary></summary>
        public static List<JobControl> Refresh(long jobControlNum)
        {
            string command = "SELECT * FROM jobcontrol WHERE JobControlNum = " + POut.Long(jobControlNum);
            return Crud.JobControlCrud.SelectMany(command);
        }

        ///<summary>Gets one JobControl from the db.</summary>
        public static JobControl GetOne(long jobControlNum)
        {
            return Crud.JobControlCrud.SelectOne(jobControlNum);
        }

        ///<summary></summary>
        public static long Insert(JobControl jobControl)
        {
            return Crud.JobControlCrud.Insert(jobControl);
        }

        ///<summary></summary>
        public static void Update(JobControl jobControl)
        {
            Crud.JobControlCrud.Update(jobControl);
        }

        ///<summary></summary>
        public static void Delete(long jobControlNum)
        {
            Crud.JobControlCrud.Delete(jobControlNum);
        }
    }
}