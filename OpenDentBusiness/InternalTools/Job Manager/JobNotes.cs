using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class JobNotes
    {

        public static List<JobNote> GetForJob(long jobNum)
        {
            string command = "SELECT * FROM jobnote WHERE JobNum = " + POut.Long(jobNum) + " ORDER BY DateTimeNote";
            return Crud.JobNoteCrud.SelectMany(command);
        }

        ///<summary>Gets one JobNote from the db.</summary>
        public static JobNote GetOne(long jobNoteNum)
        {
            return Crud.JobNoteCrud.SelectOne(jobNoteNum);
        }

        ///<summary></summary>
        public static long Insert(JobNote jobNote)
        {
            return Crud.JobNoteCrud.Insert(jobNote);
        }

        ///<summary></summary>
        public static void Update(JobNote jobNote)
        {
            Crud.JobNoteCrud.Update(jobNote);
        }

        ///<summary></summary>
        public static void Delete(long jobNoteNum)
        {
            Crud.JobNoteCrud.Delete(jobNoteNum);
        }

        public static void DeleteForJob(long jobNum)
        {
            string command = "DELETE FROM jobnote WHERE JobNum=" + POut.Long(jobNum);
            Db.NonQ(command);
        }

        ///<summary></summary>
        public static void Sync(List<JobNote> listNotesNew, long jobNum)
        {
            List<JobNote> listNotesDB = JobNotes.GetForJob(jobNum);
            Crud.JobNoteCrud.Sync(listNotesNew, listNotesDB);
        }

        ///<summary>Gets JobLinks for a specified JobNum. Only gets Bugs, Feature Requests, and Tasks.</summary>
        public static List<JobNote> GetJobNotesForJobs(List<long> jobNums)
        {
            if (jobNums == null || jobNums.Count == 0)
            {
                return new List<JobNote>();
            }
            string command = "SELECT * FROM jobnote WHERE JobNum IN (" + string.Join(",", jobNums) + ") "
                + "ORDER BY DateTimeNote";
            return Crud.JobNoteCrud.SelectMany(command);
        }
    }
}