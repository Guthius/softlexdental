using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness
{
    public class WebChatSurveys
    {
        ///<summary>Also sets primary key</summary>
        public static long Insert(WebChatSurvey webChatSurvey)
        {
            WebChatMisc.DbAction(delegate ()
            {
                Crud.WebChatSurveyCrud.Insert(webChatSurvey);
            });
            return webChatSurvey.WebChatSessionNum;
        }

        public static List<WebChatSurvey> GetSurveysForSessions(List<long> listWebChatSessionNums)
        {
            if (listWebChatSessionNums.Count == 0)
            {
                return new List<WebChatSurvey>();
            }
            List<WebChatSurvey> listWebChatSurveys = null;
            WebChatMisc.DbAction(delegate ()
            {
                string command = "SELECT * FROM webchatsurvey WHERE WebChatSessionNum IN (" + String.Join(",", listWebChatSessionNums) + ")";
                listWebChatSurveys = Crud.WebChatSurveyCrud.SelectMany(command);
            });
            return listWebChatSurveys;
        }

    }
}