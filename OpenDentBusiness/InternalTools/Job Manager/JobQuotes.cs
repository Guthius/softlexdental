using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class JobQuotes
    {

        ///<summary></summary>
        public static List<JobQuote> Refresh(long patNum)
        {
            string command = "SELECT * FROM jobquote WHERE PatNum = " + POut.Long(patNum);
            return Crud.JobQuoteCrud.SelectMany(command);
        }

        public static List<JobQuote> GetForJob(long jobNum)
        {
            string command = "SELECT * FROM jobquote WHERE JobNum = " + POut.Long(jobNum);
            return Crud.JobQuoteCrud.SelectMany(command);
        }

        public static List<JobQuote> GetAll()
        {
            string command = "SELECT * FROM jobquote ";
            return Crud.JobQuoteCrud.SelectMany(command);
        }

        ///<summary>Gets one JobQuote from the db.</summary>
        public static JobQuote GetOne(long jobQuoteNum)
        {
            return Crud.JobQuoteCrud.SelectOne(jobQuoteNum);
        }

        ///<summary></summary>
        public static long Insert(JobQuote jobQuote)
        {
            return Crud.JobQuoteCrud.Insert(jobQuote);
        }

        ///<summary></summary>
        public static void Update(JobQuote jobQuote)
        {
            Crud.JobQuoteCrud.Update(jobQuote);
        }

        ///<summary></summary>
        public static void Delete(long jobQuoteNum)
        {
            Crud.JobQuoteCrud.Delete(jobQuoteNum);
        }

        public static void DeleteForJob(long jobNum)
        {
            string command = "DELETE FROM jobquote WHERE JobNum=" + POut.Long(jobNum);
            Db.NonQ(command);
        }

        public static void Sync(List<JobQuote> listNew, long jobNum)
        {
            List<JobQuote> listDB = GetForJob(jobNum);
            Crud.JobQuoteCrud.Sync(listNew, listDB);
        }

        public static List<JobQuote> GetJobQuotesForJobs(List<long> jobNums)
        {
            if (jobNums == null || jobNums.Count == 0)
            {
                return new List<JobQuote>();
            }
            string command = "SELECT * FROM jobquote WHERE JobNum IN (" + string.Join(",", jobNums) + ")";
            return Crud.JobQuoteCrud.SelectMany(command);
        }

        public static List<JobQuote> GetUnfinishedJobQuotes()
        {
            string command = "SELECT jq.* FROM JobQuote jq INNER JOIN Job j ON j.JobNum=jq.JobNum AND j.PhaseCur!='Complete'";
            return Crud.JobQuoteCrud.SelectMany(command);
        }
    }
}