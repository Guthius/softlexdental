using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using DataConnectionBase;
using CodeBase;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class AlertItems
    {
        ///<summary>Inserts a generic alert where description will show in the menu item and itemValue will be shown within a MsgBoxCopyPaste.
        ///Set itemValue to more specific reason for the alert.  E.g. exception text details as to help the techs give better support.</summary>
        public static void CreateGenericAlert(string description, string itemValue)
        {
            AlertItem alert = new AlertItem();
            alert.Type = AlertType.Generic;
            alert.Actions = ActionType.MarkAsRead | ActionType.Delete | ActionType.ShowItemValue;
            alert.Description = description;
            alert.Severity = SeverityType.Low;
            alert.ItemValue = itemValue;
            AlertItems.Insert(alert);
        }

        #region Misc Methods

        ///<summary>Checks to see if the heartbeat for Open Dental Service was within the last six minutes. If not, an alert will be sent telling 
        ///users OpenDental Service is down.</summary>
        public static void CheckODServiceHeartbeat()
        {
            if (!IsODServiceRunning())
            {//If the heartbeat is over 6 minutes old, send the alert if it does not already exist
             //Check if there are any previous alert items
             //Get previous alerts of this type
                List<AlertItem> listOldAlerts = RefreshForType(AlertType.OpenDentalServiceDown);
                if (listOldAlerts.Count == 0)
                {//an alert does not already exist
                    AlertItem alert = new AlertItem();
                    alert.Actions = ActionType.MarkAsRead;
                    alert.ClinicNum = -1;//all clinics
                    alert.Description = Lans.g("Alerts", "No instance of Open Dental Service is running.");
                    alert.Type = AlertType.OpenDentalServiceDown;
                    alert.Severity = SeverityType.Medium;
                    Insert(alert);
                }
            }
        }

        ///<summary>Returns true if the heartbeat is less than 6 minutes old.</summary>
        public static bool IsODServiceRunning()
        {
            string command = "SELECT ValueString,NOW() FROM preference WHERE PrefName='OpenDentalServiceHeartbeat'";
            DataTable table = DataCore.GetTable(command);
            DateTime lastHeartbeat = PIn.DateT(table.Rows[0][0].ToString());
            DateTime dateTimeNow = PIn.DateT(table.Rows[0][1].ToString());
            if (lastHeartbeat.AddMinutes(6) < dateTimeNow)
            {
                return false;
            }
            return true;
        }

        ///<summary>Returns true if the heartbeat is less than 5 seconds old. Also returns the date time of the heartbeat.</summary>
        public static ODTuple<bool, DateTime> IsPhoneTrackingServerHeartbeatValid(DateTime dateTimeLastHeartbeat)
        {
            //Default to using our local time just in case we can't query MySQL every second (lessens false positives due to query / network failure).
            DateTime dateTimeNow = DateTime.Now;
            DateTime dateTimeRecentHeartbeat = dateTimeLastHeartbeat;
            DataTable table = null;
            //Check to make sure the asterisk server is still processing messages.
            ODException.SwallowAnyException(() =>
            {
                table = DataCore.GetTable("SELECT ValueString,NOW() DateTNow FROM preference WHERE PrefName='AsteriskServerHeartbeat'");
            });
            if (table != null && table.Rows.Count >= 1 && table.Columns.Count >= 2)
            {
                dateTimeRecentHeartbeat = PIn.DateT(table.Rows[0]["ValueString"].ToString());
                dateTimeNow = PIn.DateT(table.Rows[0]["DateTNow"].ToString());
            }
            //Check to see if the asterisk server heartbeat has stopped beating for the last 5 seconds.
            if ((dateTimeNow - dateTimeRecentHeartbeat).TotalSeconds > 5)
            {
                return new ODTuple<bool, DateTime>(false, dateTimeRecentHeartbeat);
            }
            return new ODTuple<bool, DateTime>(true, dateTimeRecentHeartbeat);
        }

        /// <summary>This method grabs all unread webmails, and creates/modifies/deletes alerts for the providers and linked users the webmails are addressed to.</summary>
        public static void CreateAlertsForNewWebmail(Logger.IWriteLine log)
        {
            //This method first collect all unread webmails, and counts how many each provider has.
            //It then fetches all WebMailRecieved alerts, and will create/modify alerts for each provider who was counted before.
            //Finally, it will sync the alerts in the database with the ones we created.
            //If the user has unread webmail and an existing alert, it is modified.
            //If the user has unread webmail and no existing alert, an alert is created.
            //If the user has no unread webmail and an existing alert, it will be deleted.
            //Key: ProvNum, Value: Number of unread webmails
            Dictionary<long, long> dictRecievedCount = EmailMessages.GetProvUnreadWebMailCount();
            log.WriteLine("Collected Webmails for the following providers (ProvNum: # Webmails): "
                + String.Join(", ", dictRecievedCount.Select(x => POut.Long(x.Key) + ":" + POut.Long(x.Value))), LogLevel.Verbose);
            //This list contains every single WebMailRecieved alert and is synced with listAlerts later.
            List<AlertItem> listOldAlerts = AlertItems.RefreshForType(AlertType.WebMailRecieved);
            log.WriteLine("Fetched current alerts for users: " + String.Join(", ", listOldAlerts.Select(x => POut.Long(x.UserNum))), LogLevel.Verbose);
            //If a user doesn't have any unread webmail, they won't be placed on this list, and any alert they have in listOldAlerts will be deleted.
            List<AlertItem> listAlerts = new List<AlertItem>();
            List<long> listChangedAlertItemNums = new List<long>();
            //Go through each provider value, and create/update alerts for each patnum under that provider.
            //There will only be a value if they have atleast 1 unread webmail.
            foreach (KeyValuePair<long, long> kvp in dictRecievedCount)
            {
                List<Userod> listUsers = Providers.GetAttachedUsers(kvp.Key);
                //Go through each usernum and create/update their alert item.
                foreach (long usernum in listUsers.Select(x => x.UserNum))
                {
                    AlertItem alertForUser = listOldAlerts.FirstOrDefault(x => x.UserNum == usernum);
                    //If an alert doesn't exist for the user, we'll create it.
                    if (alertForUser == null)
                    {
                        alertForUser = new AlertItem();
                        alertForUser.Type = AlertType.WebMailRecieved;
                        alertForUser.FormToOpen = FormType.FormEmailInbox;
                        alertForUser.Actions = ActionType.MarkAsRead | ActionType.OpenForm; //Removed delete because the alert will just be re-added next time it checks.
                        alertForUser.Severity = SeverityType.Normal;
                        alertForUser.ClinicNum = -1;    //The alert is user dependent, not clinic dependent.
                        alertForUser.UserNum = usernum;
                        alertForUser.Description = POut.Long(kvp.Value);
                        listAlerts.Add(alertForUser);
                        log.WriteLine("Created webmail alert for user " + POut.Long(usernum), LogLevel.Verbose);
                    }
                    else
                    {
                        //If the alert already exists, we'll be updating it and usually mark it as unread.
                        AlertItem selectedAlert = alertForUser.Copy();
                        long previousValue = PIn.Long(selectedAlert.Description);
                        //We only need to modify the alert if the amount of unread webmails changed.
                        if (previousValue != kvp.Value)
                        {
                            selectedAlert.Description = POut.Long(kvp.Value);
                            //If the new value is greater, the user has recieved more webmails so we want to mark the alert as "Unread".
                            if (previousValue < kvp.Value)
                            {
                                listChangedAlertItemNums.Add(selectedAlert.AlertItemNum);
                            }
                        }
                        listAlerts.Add(selectedAlert);
                        log.WriteLine("Modified webmail alert for user " + POut.Long(usernum), LogLevel.Verbose);
                    }
                }
            }
            //Push our changes to the database.
            AlertItems.Sync(listAlerts, listOldAlerts);
            List<AlertItem> listDeletedAlerts = listOldAlerts.Where(x => !listAlerts.Any(y => y.AlertItemNum == x.AlertItemNum)).ToList();
            log.WriteLine("Deleted webmail alerts for users: " + String.Join(", ", listDeletedAlerts.Select(x => POut.Long(x.UserNum))), LogLevel.Verbose);
            //Make sure to mark alerts that were deleted, modified (not created) and increased as unread.
            listChangedAlertItemNums.AddRange(listDeletedAlerts.Select(x => x.AlertItemNum));
            AlertReads.DeleteForAlertItems(listChangedAlertItemNums);
        }

        #endregion

        ///<summary>Returns a list of AlertItems for the given clinicNum.  Doesn't include alerts that are assigned to other users.</summary>
        public static List<AlertItem> RefreshForClinicAndTypes(long clinicNum, List<AlertType> listAlertTypes = null)
        {
            if (listAlertTypes == null || listAlertTypes.Count == 0)
            {
                return new List<AlertItem>();
            }
            long provNum = 0;
            if (Security.CurUser != null && Userods.IsUserCpoe(Security.CurUser))
            {
                provNum = Security.CurUser.ProvNum;
            }
            long curUserNum = 0;
            if (Security.CurUser != null)
            {
                curUserNum = Security.CurUser.UserNum;
            }
            string command = "";
            command = "SELECT * FROM alertitem "
                + "WHERE Type IN (" + String.Join(",", listAlertTypes.Cast<int>().ToList()) + ") "
                + "AND (UserNum=0 OR UserNum=" + POut.Long(curUserNum) + ") "
                //For AlertType.RadiologyProcedures we only care if the alert is associated to the current logged in provider.
                //When provNum is 0 the initial WHEN check below will not bring any rows by definition of the FKey column.
                + "AND (CASE TYPE WHEN " + POut.Int((int)AlertType.RadiologyProcedures) + " THEN FKey=" + POut.Long(provNum) + " "
                + "ELSE ClinicNum = " + POut.Long(clinicNum) + " OR ClinicNum=-1 END)";

            return Crud.AlertItemCrud.SelectMany(command);
        }

        ///<summary>Returns a list of AlertItems for the given alertType.</summary>
        public static List<AlertItem> RefreshForType(AlertType alertType)
        {
            string command = "SELECT * FROM alertitem WHERE Type=" + POut.Int((int)alertType) + ";";
            return Crud.AlertItemCrud.SelectMany(command);
        }

        ///<summary>Gets one AlertItem from the db.</summary>
        public static AlertItem GetOne(long alertItemNum)
        {
            return Crud.AlertItemCrud.SelectOne(alertItemNum);
        }

        ///<summary></summary>
        public static long Insert(AlertItem alertItem)
        {
            return Crud.AlertItemCrud.Insert(alertItem);
        }

        ///<summary></summary>
        public static void Update(AlertItem alertItem)
        {
            Crud.AlertItemCrud.Update(alertItem);
        }

        ///<summary>If null listFKeys is provided then all rows of the given alertType will be deleted. Otherwise only rows which match listFKeys entries.</summary>
        public static void DeleteFor(AlertType alertType, List<long> listFKeys = null)
        {
            //No need to check RemotingRole; no call to db.
            List<AlertItem> listAlerts = RefreshForType(alertType);
            if (listFKeys != null)
            { //Narrow down to just the FKeys provided.
                listAlerts = listAlerts.FindAll(x => listFKeys.Contains(x.FKey));
            }
            foreach (AlertItem alert in listAlerts)
            {
                Delete(alert.AlertItemNum);
            }
        }

        ///<summary>Also deletes any AlertRead objects for this AlertItem.</summary>
        public static void Delete(long alertItemNum)
        {
            AlertReads.DeleteForAlertItem(alertItemNum);
            Crud.AlertItemCrud.Delete(alertItemNum);
        }

        ///<summary>Inserts, updates, or deletes db rows to match listNew.  No need to pass in userNum, it's set before remoting role check and passed to
        ///the server if necessary.  Doesn't create ApptComm items, but will delete them.  If you use Sync, you must create new Apptcomm items.</summary>
        public static void Sync(List<AlertItem> listNew, List<AlertItem> listOld)
        {
            Crud.AlertItemCrud.Sync(listNew, listOld);
        }
    }
}