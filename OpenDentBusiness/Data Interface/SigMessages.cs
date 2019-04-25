using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class SigMessages
    {
        ///<summary>Gets one SigMessage from the db.</summary>
        public static List<SigMessage> GetSigMessages(List<long> listSigMessageNums)
        {
            if (listSigMessageNums == null || listSigMessageNums.Count < 1)
            {
                return new List<SigMessage>();
            }
            string command = "SELECT * FROM sigmessage WHERE SigMessageNum IN (" + String.Join(",", listSigMessageNums) + ")";
            return Crud.SigMessageCrud.SelectMany(command);
        }

        ///<summary>Gets one SigMessage from the db.</summary>
        public static List<SigMessage> GetAll()
        {
            string command = "SELECT * FROM sigmessage";
            return Crud.SigMessageCrud.SelectMany(command);
        }

        ///<summary>Only used when starting up to get the current button state.  Only gets unacked messages.
        ///There may well be extra and useless messages included.  But only the lights will be used anyway, so it doesn't matter.</summary>
        public static List<SigMessage> RefreshCurrentButState()
        {
            List<SigMessage> listSigMessages = new List<SigMessage>();
            string command = @"SELECT * FROM sigmessage "
                + "WHERE AckDateTime < " + POut.DateT(new DateTime(1880, 1, 1)) + " "
                + "ORDER BY MessageDateTime";
            listSigMessages = Crud.SigMessageCrud.SelectMany(command);
            listSigMessages.Sort();
            return listSigMessages;
        }

        ///<summary>Includes all messages, whether acked or not.  It's up to the UI to filter out acked if necessary.</summary>
        public static List<SigMessage> GetSigMessagesSinceDateTime(DateTime dateTimeSince)
        {
            List<SigMessage> listSigMessages = new List<SigMessage>();
            string command = "SELECT * FROM sigmessage "
                + "WHERE (MessageDateTime > " + POut.DateT(dateTimeSince) + " "
                + "OR AckDateTime > " + POut.DateT(dateTimeSince) + " "
                + "OR AckDateTime < " + POut.Date(new DateTime(1880, 1, 1), true) + ") "//always include all unacked.
                + "ORDER BY MessageDateTime";
            //note: this might return an occasional row that has both times newer.
            listSigMessages = Crud.SigMessageCrud.SelectMany(command);
            listSigMessages.Sort();
            return listSigMessages;
        }

        ///<summary>When user clicks on a colored light, they intend to ack it to turn it off.  This acks all sigmessages with the specified index.
        ///This is in case multiple sigmessages have been created from different workstations.  This acks them all in one shot.
        ///Must specify a time because you only want to ack sigmessages earlier than the last time this workstation was refreshed.
        ///A newer sigmessage would not get acked. If this seems slow, then I will need to check to make sure all these tables are properly indexed.
        ///Inserts a signal for every SigMessageNum that was updated.</summary>
        public static void AckButton(int buttonIndex, DateTime time)
        {
            List<long> listSigMessageNums = new List<long>();
            string command = "SELECT DISTINCT sigmessage.SigMessageNum FROM sigmessage "
                + "INNER JOIN sigelementdef ON (sigmessage.SigElementDefNumUser=sigelementdef.SigElementDefNum "
                    + "OR sigmessage.SigElementDefNumExtra=sigelementdef.SigElementDefNum "
                    + "OR sigmessage.SigElementDefNumMsg=sigelementdef.SigElementDefNum) "
                + "WHERE sigmessage.AckDateTime < " + POut.Date(new DateTime(1880, 1, 1), true) + " "
                + "AND MessageDateTime <= " + POut.DateT(time) + " "
                + "AND sigelementdef.LightRow=" + POut.Long(buttonIndex);
            DataTable table = Db.GetTable(command);
            if (table.Rows.Count == 0)
            {
                return;
            }
            listSigMessageNums = table.Select().Select(x => PIn.Long(x["SigMessageNum"].ToString())).ToList();
            command = "UPDATE sigmessage SET AckDateTime = " + DbHelper.Now() + " "
                + "WHERE SigMessageNum IN (" + string.Join(",", listSigMessageNums) + ")";
            Db.NonQ(command);
            listSigMessageNums.ForEach(x => Signalods.SetInvalid(InvalidType.SigMessages, KeyType.SigMessage, x));
        }

        ///<summary>Acknowledge one sig message from the manage module grid.</summary>
        public static void AckSigMessage(SigMessage sigMessage)
        {
            //To ack a message, simply update the AckDateTime on the original row.
            sigMessage.AckDateTime = MiscData.GetNowDateTime();
            Update(sigMessage);
        }

        ///<summary></summary>
        public static long Insert(SigMessage sigMessage)
        {
            return Crud.SigMessageCrud.Insert(sigMessage);
        }

        ///<summary></summary>
        public static void Update(SigMessage sigMessage)
        {
            Crud.SigMessageCrud.Update(sigMessage);
        }

        ///<summary>Deletes all sigmessages older than 2 days.  Will fail silently if anything goes wrong.</summary>
        public static void ClearOldSigMessages()
        {
            try
            {
                //Get all ack'd messages older than two days.
                string command = "";
                DataTable table;

                command = "SELECT SigMessageNum FROM sigmessage WHERE AckDateTime > " + POut.DateT(new DateTime(1880, 1, 1)) + " "
                    + "AND AckDateTime < DATE_ADD(NOW(),INTERVAL -2 DAY)";
                table = Db.GetTable(command);

                if (table.Rows.Count < 1)
                {
                    return;//Nothing to delete.
                }
                //Delete all of the acks.
                command = "DELETE FROM sigmessage "
                    + "WHERE SigMessageNum IN (" + String.Join(",", table.Select().Select(x => PIn.Long(x["SigMessageNum"].ToString()))) + ")";
                Db.NonQ(command);
            }
            catch (Exception)
            {
                //fail silently
            }
        }
    }
}