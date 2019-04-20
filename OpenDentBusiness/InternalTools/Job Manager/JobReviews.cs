using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class JobReviews
    {

        ///<summary></summary>
        public static List<JobReview> GetForReviewer(long userNum)
        {
            string command = "SELECT * FROM jobreview WHERE Reviewer = " + POut.Long(userNum) + " AND ReviewStatus!='" + POut.String(JobReviewStatus.TimeLog.ToString()) + "' ";
            return Crud.JobReviewCrud.SelectMany(command);
        }

        ///<summary>Gets one JobReview from the db.</summary>
        public static JobReview GetOne(long jobReviewNum)
        {
            return Crud.JobReviewCrud.SelectOne(jobReviewNum);
        }

        public static List<JobReview> GetReviewsForJobs(params long[] arrayJobNums)
        {
            if (arrayJobNums == null || arrayJobNums.Length == 0)
            {
                return new List<JobReview>();
            }
            string command = "SELECT * FROM jobreview WHERE JobNum IN (" + string.Join(",", arrayJobNums) + ") "
                + "AND ReviewStatus!='" + POut.String(JobReviewStatus.TimeLog.ToString()) + "' "
                + "ORDER BY DateTStamp";
            return Crud.JobReviewCrud.SelectMany(command);
        }

        public static List<JobReview> GetTimeLogsForJobs(params long[] arrayJobNums)
        {
            if (arrayJobNums == null || arrayJobNums.Length == 0)
            {
                return new List<JobReview>();
            }
            string command = "SELECT * FROM jobreview WHERE JobNum IN (" + string.Join(",", arrayJobNums) + ") "
                + "AND ReviewStatus='" + POut.String(JobReviewStatus.TimeLog.ToString()) + "' "
                + "ORDER BY DateTStamp";
            List<JobReview> listReviews = Crud.JobReviewCrud.SelectMany(command);
            return listReviews;
        }

        public static DataTable GetOutstandingForUser(long userNum)
        {
            string command = "SELECT jobreview.*,job.JobNum,job.Owner,job.Title FROM jobreview "
                + "INNER JOIN joblink ON jobreview.JobReviewNum=joblink.FKey "
                + "INNER JOIN job ON joblink.JobNum=job.JobNum "
                + "WHERE jobreview.Reviewer=" + POut.Long(userNum) + " "
                + "AND jobreview.ReviewStatus IN(" + POut.Long((int)JobReviewStatus.Sent)
                + "," + POut.Long((int)JobReviewStatus.Seen) + "," + POut.Long((int)JobReviewStatus.UnderReview) + ") ";
            //+"AND joblink.LinkType="+POut.Long((int)JobLinkType.Review);
            return Db.GetTable(command);
        }

        public static void SetSeen(long reviewNum)
        {
            string command = "UPDATE jobreview SET ReviewStatus=" + POut.Long((int)JobReviewStatus.Seen) + " "
                + "WHERE JobReviewNum=" + POut.Long(reviewNum);
            Db.NonQ(command);
        }

        ///<summary></summary>
        public static long Insert(JobReview jobReview)
        {
            return Crud.JobReviewCrud.Insert(jobReview);
        }

        ///<summary></summary>
        public static void Update(JobReview jobReview)
        {
            Crud.JobReviewCrud.Update(jobReview);
        }

        ///<summary>Deletes a joblink of the specified type and num.</summary>
        public static void Delete(long jobReviewNum)
        {//, JobLinkType jobLinkType) {
            Crud.JobReviewCrud.Delete(jobReviewNum);
        }

        public static void DeleteForJob(long jobNum)
        {
            string command = "DELETE FROM jobquote WHERE JobNum=" + POut.Long(jobNum);
            Db.NonQ(command);
        }

        public static void SyncReviews(List<JobReview> listNew, long jobNum)
        {
            List<JobReview> listDB = GetReviewsForJobs(jobNum);
            Crud.JobReviewCrud.Sync(listNew, listDB);
        }

        public static void SyncTimeLogs(List<JobReview> listNew, long jobNum)
        {
            List<JobReview> listDB = GetTimeLogsForJobs(jobNum);
            Crud.JobReviewCrud.Sync(listNew, listDB);
        }
    }
}