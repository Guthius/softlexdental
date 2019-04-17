using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class WikiPageHists
    {

        ///<summary>Ordered by dateTimeSaved.  Objects will not have the PageContent field populated.  Use GetPageContent to get the content for a
        ///specific revision.</summary>
        public static List<WikiPageHist> GetByTitleNoPageContent(string pageTitle)
        {
            string command = 
                "SELECT WikiPageNum, UserNum, PageTitle, '' AS PageContent, DateTimeSaved, IsDeleted " + 
                "FROM wikipagehist " +
                "WHERE PageTitle = '" + POut.String(pageTitle) + "' " +
                "ORDER BY DateTimeSaved";

            return Crud.WikiPageHistCrud.SelectMany(command);
        }

        public static string GetPageContent(long wikiPageNum)
        {
            if (wikiPageNum < 1)
            {
                return "";
            }

            return Db.GetScalar("SELECT PageContent FROM wikipagehist WHERE WikiPageNum=" + POut.Long(wikiPageNum));
        }

        public static List<string> GetDeletedPages(string searchText, bool ignoreContent)
        {
            List<string> pageTitleList = new List<string>();
            DataTable tableResults = new DataTable();
            DataTable tableNewestDateTimes = new DataTable();
            string[] searchTokens = searchText.Split(' ');

            string command = 
                "SELECT PageTitle, MAX(DateTimeSaved) AS DateTimeSaved " +
                "FROM wikipagehist " +
                "GROUP BY PageTitle";

            tableNewestDateTimes = Db.GetTable(command);

            command =
                "SELECT PageTitle,DateTimeSaved " +
                "FROM wikipagehist " +
                "WHERE PageTitle NOT LIKE '\\_%' ";

            for (int i = 0; i < searchTokens.Length; i++)
            {
                var searchToken = searchTokens[i].Trim();
                if (searchToken.Length > 0)
                {
                    if (ignoreContent)
                    {
                        command += "AND PageTitle LIKE '%" + POut.String(searchToken) + "%' ";
                    }
                    else
                    {
                        command += "AND (PageTitle LIKE '%" + POut.String(searchToken) + "%' OR PageContent LIKE '%" + POut.String(searchToken) + "%') ";
                    }
                }
            }

            command +=
                "AND PageTitle NOT IN (SELECT PageTitle FROM wikipage WHERE IsDraft = 0) " +
                "AND IsDeleted = 1 " +
                "ORDER BY PageTitle";

            tableResults = Db.GetTable(command);
            for (int i = 0; i < tableResults.Rows.Count; i++)
            {
                var pageTitle = tableResults.Rows[i]["PageTitle"].ToString();
                if (pageTitleList.Contains(pageTitle))
                {
                    continue;
                }

                for (int j = 0; j < tableNewestDateTimes.Rows.Count; j++)
                {
                    if (tableNewestDateTimes.Rows[j]["PageTitle"].ToString() == pageTitle &&
                        tableNewestDateTimes.Rows[j]["DateTimeSaved"].ToString() == tableResults.Rows[i]["DateTimeSaved"].ToString())
                    {
                        pageTitleList.Add(pageTitle);
                        break;
                    }
                }
            }

            return pageTitleList;
        }

        /// <summary>
        /// Only returns the most recently deleted version of the page. Returns null if not found.
        /// </summary>
        public static WikiPageHist GetDeletedByTitle(string pageTitle)
        {
            string command = 
                "SELECT * " +
                "FROM wikipagehist " +
                "WHERE PageTitle = '" + POut.String(pageTitle) + "' " +
                "AND IsDeleted = 1 " +
                "AND DateTimeSaved = (" +
                    "SELECT MAX(DateTimeSaved) " +
                    "FROM wikipagehist " +
                    "WHERE PageTitle = '" + POut.String(pageTitle) + "' " +
                    "AND IsDeleted = 1)"
                                            ;
            return Crud.WikiPageHistCrud.SelectOne(command);
        }

        public static long Insert(WikiPageHist wikiPageHist) => Crud.WikiPageHistCrud.Insert(wikiPageHist);
        
        public static WikiPage RevertFrom(WikiPageHist wikiPageHist)
        {
            var wikiPage = WikiPages.GetByTitle(wikiPageHist.PageTitle);
            if (wikiPage == null)
            {
                wikiPage = new WikiPage();
            }

            wikiPage.PageTitle = wikiPageHist.PageTitle;
            wikiPage.PageContent = wikiPageHist.PageContent;
            wikiPage.KeyWords = "";

            Match m = Regex.Match(wikiPageHist.PageContent, @"\[\[(keywords:).*?\]\]");
            if (m.Length > 0)
            {
                wikiPage.KeyWords = m.Value.Substring(11).TrimEnd(']');
            }

            return wikiPage;
        }
    }
}