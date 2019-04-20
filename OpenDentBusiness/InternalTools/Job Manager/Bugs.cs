using CodeBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class Bugs
    {
        private const int THREAD_TIMEOUT = 5000;

        ///<summary>Returns a list of bugs for given bugIds.</summary>
        public static List<Bug> GetMany(List<long> listBugIds)
        {
            if (listBugIds == null || listBugIds.Count == 0)
            {
                return new List<Bug>();
            }
            List<Bug> listBugs = new List<Bug>();
            DataAction.RunBugsHQ(() =>
            {
                listBugs = Crud.BugCrud.TableToList(DataCore.GetTable("SELECT * FROM bug WHERE BugID IN (" + string.Join(",", listBugIds) + ")"));
            }, false);
            return listBugs;
        }

        ///<summary>Must pass in version as "Maj" or "Maj.Min" or "Maj.Min.Rev". Uses like operator.</summary>
        public static List<Bug> GetByVersion(string versionMajMin, string filter = "")
        {
            List<Bug> listBugs = new List<Bug>();
            DataAction.RunBugsHQ(() =>
            {
                string command = "SELECT * FROM bug "
                + "WHERE (VersionsFound LIKE '" + POut.String(versionMajMin) + "%' "
                    + "OR VersionsFound LIKE '%;" + POut.String(versionMajMin) + "%' "
                    + "OR VersionsFixed LIKE '" + POut.String(versionMajMin) + "%' "
                    + "OR VersionsFixed LIKE '%;" + POut.String(versionMajMin) + "%') ";
                if (filter != "")
                {
                    command += "AND Description LIKE '%" + POut.String(filter) + "%'";
                }
                listBugs = Crud.BugCrud.SelectMany(command);
            }, false);
            return listBugs;
        }

        ///<summary>Gets all bugs ordered by CreationDate DESC</summary>
        public static List<Bug> GetAll()
        {
            List<Bug> listBugs = new List<Bug>();
            DataAction.RunBugsHQ(() =>
            {
                string command = "SELECT * FROM bug ORDER BY CreationDate DESC";
                listBugs = Crud.BugCrud.SelectMany(command);
            }, false);
            return listBugs;
        }

        ///<summary>Gets one Bug from the db.</summary>
        public static Bug GetOne(long bugId)
        {
            Bug bug = null;
            DataAction.RunBugsHQ(() => bug = Crud.BugCrud.SelectOne(bugId), false);
            return bug;
        }

        ///<summary></summary>
        public static long Insert(Bug bug)
        {
            DataAction.RunBugsHQ(() => Crud.BugCrud.Insert(bug), false);
            return bug.BugId;
        }

        ///<summary></summary>
        public static void Update(Bug bug)
        {
            DataAction.RunBugsHQ(() => Crud.BugCrud.Update(bug), false);
        }

        ///<summary></summary>
        public static void Delete(long bugId)
        {
            JobLinks.DeleteForType(JobLinkType.Bug, bugId);
            DataAction.RunBugsHQ(() => Crud.BugCrud.Delete(bugId), false);
        }

        ///<Summary>Gets name from database for the submitter passed in.  Not very efficient.</Summary>
        public static string GetSubmitterName(long bugUserId)
        {
            string submitterName = "";
            DataAction.RunBugsHQ(() =>
            {
                string command = "SELECT UserName FROM buguser WHERE BugUserId=" + POut.Long(bugUserId);
                submitterName = Db.GetScalar(command);
            }, false);
            return submitterName;
        }

        ///<Summary>Returns a dictionary where key: BugUserId, value: UserName from database for the submitters passed in.  Not very efficient.</Summary>
        public static Dictionary<long, string> GetDictSubmitterNames(List<long> listBugUserIds)
        {
            if (listBugUserIds == null || listBugUserIds.Count == 0)
            {
                return new Dictionary<long, string>();
            }
            Dictionary<long, string> dictSubmitterNames = new Dictionary<long, string>();
            DataAction.RunBugsHQ(() =>
            {
                dictSubmitterNames = Db.GetTable("SELECT BugUserId,UserName FROM buguser WHERE BugUserId IN (" + string.Join(",", listBugUserIds) + ")").Select()
                    .ToDictionary(x => PIn.Long(x["BugUserId"].ToString()), x => PIn.String(x["UserName"].ToString()));
            }, false);
            return dictSubmitterNames;
        }

        ///<Summary>Checks bugIDs in list for incompletes. Returns false if incomplete exists.</Summary>
        public static bool CheckForCompletion(List<long> listBugIDs)
        {
            int count = 0;
            DataAction.RunBugsHQ(() =>
            {
                string command = "SELECT COUNT(*) FROM bug "
                    + "WHERE VersionsFixed='' "
                    + "AND BugId IN (" + String.Join(",", listBugIDs) + ")";
                count = PIn.Int(Db.GetCount(command));
            }, false);
            return (count == 0);
        }

        public static Bug GetNewBugForUser()
        {
            Bug bug = new Bug();
            bug.CreationDate = DateTime.Now;
            bug.Status_ = BugStatus.None;
            switch (System.Environment.MachineName)
            {
                case "ANDREW":
                case "ANDREW1":
                    bug.Submitter = 29;//Andrew
                    break;
                case "JORDANS":
                case "JORDANS3":
                    bug.Submitter = 4;//jsparks
                    break;
                case "JASON":
                    bug.Submitter = 18;//jsalmon
                    break;
                case "DAVID":
                    bug.Submitter = 27;//david
                    break;
                case "DEREK":
                    bug.Submitter = 1;//grahamde
                    break;
                case "SAM":
                    bug.Submitter = 25;//sam
                    break;
                case "RYAN":
                case "RYAN1":
                    bug.Submitter = 20;//Ryan
                    break;
                case "CAMERON":
                    bug.Submitter = 21;//Cameron
                    break;
                case "TRAVIS":
                    bug.Submitter = 22;//tgriswold
                    break;
                case "ALLEN":
                    bug.Submitter = 24;//allen
                    break;
                case "JOSH":
                    bug.Submitter = 26;//josh
                    break;
                case "JOE":
                    bug.Submitter = 28;//joe
                    break;
                case "CHRISM":
                    bug.Submitter = 30;//chris
                    break;
                case "SAUL":
                    bug.Submitter = 31;//saul
                    break;
                case "MATHERINL":
                    bug.Submitter = 32;//matherinl
                    break;
                case "LINDSAYS":
                    bug.Submitter = 33;//linsdays
                    break;
                case "BRENDANB":
                    bug.Submitter = 34;//brendanb
                    break;
                case "KENDRAS":
                    bug.Submitter = 35;//kendras
                    break;
                case "STEVENS":
                    bug.Submitter = 37;//stevens
                    break;
                case "LUKEM":
                    bug.Submitter = 41;//lukem
                    break;
                case "DEVINF":
                    bug.Submitter = 43;
                    break;
                case "NICHOLASL":
                    bug.Submitter = 45;
                    break;
                default:
                    bug.Submitter = 2;//Tech Support
                    break;
            }
            bug.Type_ = BugType.Bug;
            bug.VersionsFound = VersionReleases.GetLastReleases(2);
            return bug;
        }

        ///<summary>Attempts to get the ValueString from the bug's preference table for the given PrefName.  The ValueString retrieved from the db will be converted to T.
        ///Returns true if the preference was found and successfully converted to the type of T.  Otherwise retVal will be set to the default for T and false will be returned.</summary>
        public static bool TryGetPrefValue<T>(string prefName, out T retVal)
        {
            retVal = default(T);
            try
            {
                string val = "";
                DataAction.RunBugsHQ(() =>
                {
                    val = Db.GetScalar("SELECT ValueString FROM preference WHERE PrefName='" + prefName + "'");
                }, false);
                retVal = (T)Convert.ChangeType(val, typeof(T));
            }
            catch (Exception ex)
            {
                ex.DoNothing();
                return false;
            }
            return true;
        }
    }
}