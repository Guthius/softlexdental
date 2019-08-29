using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using CodeBase;

namespace OpenDentBusiness
{
    public class AlertItems
    {
        /// <summary>
        /// Inserts a generic alert where description will show in the menu item and itemValue will 
        /// be shown within a MsgBoxCopyPaste. Set itemValue to more specific reason for the alert.
        /// E.g. exception text details as to help the techs give better support.
        /// </summary>
        public static void CreateGenericAlert(string description, string itemValue)
        {
            AlertItem alert = new AlertItem();
            alert.Type = AlertType.Generic;
            alert.Actions = ActionType.MarkAsRead | ActionType.Delete | ActionType.ShowItemValue;
            alert.Description = description;
            alert.Severity = SeverityType.Low;
            alert.ItemValue = itemValue;
            Insert(alert);
        }

        /// <summary>
        /// Checks to see if the heartbeat for Open Dental Service was within the last six minutes. 
        /// If not, an alert will be sent telling users OpenDental Service is down.
        /// </summary>
        public static void CheckODServiceHeartbeat()
        {
            if (!IsODServiceRunning())
            {
                // If the heartbeat is over 6 minutes old, send the alert if it does not already exist
                // Check if there are any previous alert items
                // Get previous alerts of this type
                List<AlertItem> listOldAlerts = RefreshForType(AlertType.OpenDentalServiceDown);
                if (listOldAlerts.Count == 0) //an alert does not already exist
                {
                    AlertItem alert = new AlertItem();
                    alert.Actions = ActionType.MarkAsRead;
                    alert.ClinicNum = -1; // All clinics
                    alert.Description = "No instance of Open Dental Service is running.";
                    alert.Type = AlertType.OpenDentalServiceDown;
                    alert.Severity = SeverityType.Medium;
                    Insert(alert);
                }
            }
        }

        /// <summary>
        /// Returns true if the heartbeat is less than 6 minutes old.
        /// </summary>
        public static bool IsODServiceRunning()
        {
            DataTable table = DataConnection.GetTable("SELECT ValueString,NOW() FROM preference WHERE PrefName='OpenDentalServiceHeartbeat'");
            DateTime lastHeartbeat = PIn.DateT(table.Rows[0][0].ToString());
            DateTime dateTimeNow = PIn.DateT(table.Rows[0][1].ToString());
            if (lastHeartbeat.AddMinutes(6) < dateTimeNow)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if the heartbeat is less than 5 seconds old. 
        /// Also returns the date time of the heartbeat.
        /// </summary>
        public static Tuple<bool, DateTime> IsPhoneTrackingServerHeartbeatValid(DateTime dateTimeLastHeartbeat)
        {
            // Default to using our local time just in case we can't query MySQL every second (lessens false positives due to query / network failure).
            DateTime dateTimeNow = DateTime.Now;
            DateTime dateTimeRecentHeartbeat = dateTimeLastHeartbeat;
            DataTable table = null;

            // Check to make sure the asterisk server is still processing messages.
            ODException.SwallowAnyException(() =>
            {
                table = DataConnection.GetTable("SELECT ValueString,NOW() DateTNow FROM preference WHERE PrefName='AsteriskServerHeartbeat'");
            });

            if (table != null && table.Rows.Count >= 1 && table.Columns.Count >= 2)
            {
                dateTimeRecentHeartbeat = PIn.DateT(table.Rows[0]["ValueString"].ToString());
                dateTimeNow = PIn.DateT(table.Rows[0]["DateTNow"].ToString());
            }

            // Check to see if the asterisk server heartbeat has stopped beating for the last 5 seconds.
            if ((dateTimeNow - dateTimeRecentHeartbeat).TotalSeconds > 5)
            {
                return new Tuple<bool, DateTime>(false, dateTimeRecentHeartbeat);
            }
            return new Tuple<bool, DateTime>(true, dateTimeRecentHeartbeat);
        }

        /// <summary>
        /// Returns a list of AlertItems for the given clinicNum.  Doesn't include alerts that are assigned to other users.
        /// </summary>
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

            // For AlertType.RadiologyProcedures we only care if the alert is associated to the current logged in provider.
            // When provNum is 0 the initial WHEN check below will not bring any rows by definition of the FKey column.
            return Crud.AlertItemCrud.SelectMany(
                "SELECT * FROM alertitem " +
                "WHERE Type IN (" + String.Join(",", listAlertTypes.Cast<int>().ToList()) + ") " +
                "AND (UserNum=0 OR UserNum=" + POut.Long(curUserNum) + ") " +
                "AND (CASE TYPE WHEN " + POut.Int((int)AlertType.RadiologyProcedures) + " THEN FKey=" + POut.Long(provNum) + " " +
                "ELSE ClinicNum = " + POut.Long(clinicNum) + " OR ClinicNum=-1 END)");
        }

        /// <summary>
        /// Returns a list of AlertItems for the given alertType.
        /// </summary>
        public static List<AlertItem> RefreshForType(AlertType alertType) => Crud.AlertItemCrud.SelectMany("SELECT * FROM alertitem WHERE Type=" + POut.Int((int)alertType) + ";");

        /// <summary>
        /// Gets one AlertItem from the db.
        /// </summary>
        public static AlertItem GetOne(long alertItemNum) => Crud.AlertItemCrud.SelectOne(alertItemNum);

        public static long Insert(AlertItem alertItem) => Crud.AlertItemCrud.Insert(alertItem);

        public static void Update(AlertItem alertItem) => Crud.AlertItemCrud.Update(alertItem);

        /// <summary>
        /// If null listFKeys is provided then all rows of the given alertType will be deleted. Otherwise only rows which match listFKeys entries.
        /// </summary>
        public static void DeleteFor(AlertType alertType, List<long> listFKeys = null)
        {
            List<AlertItem> listAlerts = RefreshForType(alertType);
            if (listFKeys != null) // Narrow down to just the FKeys provided.
            {
                listAlerts = listAlerts.FindAll(x => listFKeys.Contains(x.FKey));
            }

            foreach (AlertItem alert in listAlerts) Delete(alert.AlertItemNum);
        }

        /// <summary>
        /// Also deletes any AlertRead objects for this AlertItem.
        /// </summary>
        public static void Delete(long alertItemNum)
        {
            AlertReads.DeleteForAlertItem(alertItemNum);
            Crud.AlertItemCrud.Delete(alertItemNum);
        }

        /// <summary>
        /// Inserts, updates, or deletes db rows to match listNew.
        /// No need to pass in userNum, it's set before remoting role check and passed to
        /// the server if necessary.  Doesn't create ApptComm items, but will delete them.
        /// If you use Sync, you must create new Apptcomm items.
        /// </summary>
        public static void Sync(List<AlertItem> listNew, List<AlertItem> listOld) => Crud.AlertItemCrud.Sync(listNew, listOld);
    }
}