using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness
{
    public class WebChatSessions
    {

        ///<summary>Also sets primary key and DateTcreated.</summary>
        public static long Insert(WebChatSession webChatSession)
        {
            WebChatMisc.DbAction(delegate ()
            {
                Crud.WebChatSessionCrud.Insert(webChatSession);
            });
            WebChatMisc.DbAction(delegate ()
            {
                Signalods.SetInvalid(InvalidType.WebChatSessions);//Signal OD HQ to refresh sessions.
            }, false);
            return webChatSession.WebChatSessionNum;
        }

        public static List<WebChatSession> GetSessions(bool hasEndedSessionsIncluded, DateTime dateCreatedFrom, DateTime dateCreatedTo)
        {
            List<WebChatSession> listWebChatSessions = null;
            WebChatMisc.DbAction(delegate ()
            {
                string command = "SELECT * FROM webchatsession "
                    + "WHERE DateTcreated >= " + POut.DateT(dateCreatedFrom) + " AND DateTcreated <= " + POut.DateT(dateCreatedTo) + " ";
                if (!hasEndedSessionsIncluded)
                {//Do not show ended sessions?
                    command += "AND DateTend < " + POut.DateT(new DateTime(1880, 1, 1)) + " ";//Session is ended if DateTend is set.
                }
                command += "ORDER BY DateTend,DateTcreated";//By DateTend first, so that currently active sessions show at the top of the list.
                listWebChatSessions = Crud.WebChatSessionCrud.SelectMany(command);
            });
            return listWebChatSessions;
        }

        public static List<WebChatSession> GetActiveSessions()
        {
            List<WebChatSession> listWebChatSessions = new List<WebChatSession>();
            WebChatMisc.DbAction(() =>
            {
                string command = "SELECT * FROM webchatsession WHERE DateTend < " + POut.DateT(new DateTime(1880, 1, 1)) + " ";//Session is ended if DateTend is set.
                command += "ORDER BY DateTcreated";
                listWebChatSessions = Crud.WebChatSessionCrud.SelectMany(command);
            });
            return listWebChatSessions;
        }

        public static WebChatSession GetActiveSessionsForEmployee(string techName)
        {
            WebChatSession webChatSession = null;
            WebChatMisc.DbAction(() =>
            {
                string command = "SELECT * FROM webchatsession WHERE DateTend < " + POut.DateT(new DateTime(1880, 1, 1))
                    + " AND webchatsession.TechName = '" + POut.String(techName) + "'"
                    + " ORDER BY webchatsession.DateTcreated ";
                webChatSession = Crud.WebChatSessionCrud.SelectOne(command);
            });
            return webChatSession;
        }

        public static WebChatSession GetOne(long webChatSessionNum)
        {
            WebChatSession session = null;
            WebChatMisc.DbAction(delegate ()
            {
                session = Crud.WebChatSessionCrud.SelectOne(webChatSessionNum);
            });
            return session;
        }

        public static void Update(WebChatSession webChatSession, WebChatSession oldWebChatSession, bool hasSignal = true)
        {
            WebChatMisc.DbAction(delegate ()
            {
                Crud.WebChatSessionCrud.Update(webChatSession, oldWebChatSession);
            });
            if (hasSignal)
            {
                WebChatMisc.DbAction(delegate ()
                {
                    Signalods.SetInvalid(InvalidType.WebChatSessions);//Signal OD HQ to refresh sessions.
                }, false);
            }
        }

        public static void SendWelcomeMessage(long webChatSessionNum)
        {
            //No need to check RemotingRole; no call to db.
            WebChatMessage welcomeMessage = new WebChatMessage();
            welcomeMessage.WebChatSessionNum = webChatSessionNum;
            welcomeMessage.UserName = WebChatPrefs.GetString(WebChatPrefName.SystemName);
            welcomeMessage.MessageText = WebChatPrefs.GetString(WebChatPrefName.SystemWelcomeMessage);
            welcomeMessage.MessageType = WebChatMessageType.System;
            WebChatMessages.Insert(welcomeMessage);
        }

        ///<summary>Sets the DateTend field to now and inserts a message into the chat so all users can see the session has ended.</summary>
        public static void EndSession(long webChatSessionNum)
        {
            WebChatMisc.DbAction(delegate ()
            {
                string command = "UPDATE webchatsession SET DateTend=NOW() WHERE WebChatSessionNum=" + POut.Long(webChatSessionNum);
                DataCore.NonQ(command);
                //Last message just after session ended, in case someone types another message into the thread just as the thread is ending.
                //This way the end session message is guaranteed to be last, since the timestamp on it is after the session technically ended.
                WebChatMessage endSessionMessage = new WebChatMessage();
                endSessionMessage.WebChatSessionNum = webChatSessionNum;
                endSessionMessage.UserName = WebChatPrefs.GetString(WebChatPrefName.SystemName);
                endSessionMessage.MessageText = WebChatPrefs.GetString(WebChatPrefName.SystemSessionEndMessage);
                endSessionMessage.MessageType = WebChatMessageType.EndSession;
                WebChatMessages.Insert(endSessionMessage);
            });
        }

        public static void SendCustomerMessage(long webChatSessionNum, string userName, string messageText)
        {
            //No need to check RemotingRole; no call to db.
            if (String.IsNullOrEmpty(messageText))
            {
                return;//No blank messages allowed.
            }
            WebChatMessage customerMessage = new WebChatMessage();
            customerMessage.WebChatSessionNum = webChatSessionNum;
            if (String.IsNullOrEmpty(userName))
            {
                customerMessage.UserName = "Customer";
            }
            else
            {
                customerMessage.UserName = userName;
            }
            customerMessage.MessageText = messageText;
            customerMessage.MessageType = WebChatMessageType.Customer;
            WebChatMessages.Insert(customerMessage);
        }

        public static void SendTechMessage(long webChatSessionNum, string messageText)
        {
            //No need to check RemotingRole; no call to db.
            if (string.IsNullOrEmpty(messageText))
            {
                return;//No blank messages allowed.
            }
            WebChatMessage techMessage = new WebChatMessage();
            techMessage.WebChatSessionNum = webChatSessionNum;
            techMessage.UserName = Security.CurUser.UserName;
            techMessage.MessageText = messageText;
            techMessage.MessageType = WebChatMessageType.Technician;
            WebChatMessages.Insert(techMessage);
        }
    }
}