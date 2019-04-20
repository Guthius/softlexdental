using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class PhoneConfs
    {

        ///<summary>Throws exceptions.</summary>
        public static long Insert(PhoneConf phoneConf)
        {
            if (phoneConf == null)
            {
                throw new ArgumentException("Invalid conference room.", "phoneConf");
            }
            if (HasConfRoom(phoneConf.Extension))
            {
                throw new ApplicationException("Conference room already in use.");
            }
            return Crud.PhoneConfCrud.Insert(phoneConf);
        }

        ///<summary></summary>
        public static void Update(PhoneConf phoneConf)
        {
            Crud.PhoneConfCrud.Update(phoneConf);
        }

        ///<summary></summary>
        public static void Delete(long phoneConfNum)
        {
            string command = "DELETE FROM phoneconf WHERE PhoneConfNum = " + POut.Long(phoneConfNum);
            Db.NonQ(command);
        }

        ///<summary></summary>
        public static void DeleteMany(List<long> listPhoneConfNums)
        {
            if (listPhoneConfNums == null || listPhoneConfNums.Count < 1)
            {
                return;
            }
            string command = "DELETE FROM phoneconf WHERE PhoneConfNum IN(" + string.Join(",", listPhoneConfNums) + ")";
            Db.NonQ(command);
        }

        ///<summary></summary>
        public static List<PhoneConf> GetAll()
        {
            string command = "SELECT * FROM phoneconf";
            try
            {
                return Crud.PhoneConfCrud.SelectMany(command);
            }
            catch
            {
                return new List<PhoneConf>();
            }
        }

        ///<summary>Returns true if there is already a conference room with the passed in extension within the database.  Otherwise false.</summary>
        public static bool HasConfRoom(long confRoomExtension)
        {
            string command = "SELECT COUNT(*) FROM phoneconf WHERE Extension=" + POut.Long(confRoomExtension);
            return (Db.GetCount(command) != "0");
        }

        ///<summary>Gets the next available conference room from the corresponding site and then reserves it for the currently logged in user.
        ///Returns null if no available conference room found.</summary>
        public static PhoneConf GetAndReserveConfRoom(long siteNum)
        {
            PhoneConf phoneConf;
            //This query purposefully uses the server time instead of passing in a locally manipulated DateTime object.
            string command = "SELECT * FROM phoneconf "
                + "WHERE Occupants = 0 "
                + "AND ButtonIndex < 0 "
                + "AND SiteNum=" + POut.Long(siteNum) + " "
                + "AND DateTimeReserved <= (NOW() - INTERVAL 5 MINUTE) "
                + "ORDER BY Extension "
                + "LIMIT 1";
            phoneConf = Crud.PhoneConfCrud.SelectOne(command);
            if (phoneConf != null)
            {
                //Run a query (so that we always use server time) to update the reserved date time.
                command = "UPDATE phoneconf SET DateTimeReserved=NOW(),UserNum=" + POut.Long(Security.CurUser.UserNum) + " "
                    + "WHERE PhoneConfNum=" + POut.Long(phoneConf.PhoneConfNum);
                Db.NonQ(command);
            }
            return phoneConf;
        }

        ///<summary>Increments the Occupants column by one for the corresponding extension passed in.
        ///A new row will be inserted if no row is found for the extension passed in.</summary>
        public static void AddOccupantForExtension(int extension)
        {
            string command = "UPDATE phoneconf SET Occupants = Occupants+1 WHERE Extension=" + POut.Int(extension);
            Db.NonQ(command);
        }

        ///<summary>Decrements the Occupants column by one for the corresponding extension passed in.</summary>
        public static void RemoveOccupantForExtension(int extension)
        {
            string command = "UPDATE phoneconf SET Occupants = Occupants-1 "
                + "WHERE Extension=" + POut.Int(extension) + " "
                + "AND Occupants > 0";//Never go negative.
            Db.NonQ(command);
        }

        ///<summary>Sets the Occupants column to zero for the corresponding extension passed in.</summary>
        public static void ClearOccupantsForExtension(int extension)
        {
            string command = "UPDATE phoneconf SET Occupants = 0 WHERE Extension=" + POut.Int(extension);
            Db.NonQ(command);
        }

        ///<summary>Kicks all of the occupants within the passed in conference rooms.</summary>
        public static void KickConfRooms(List<long> listConfRoomExtensions)
        {
            //No need to check RemotingRole; no call to db.
            foreach (long extension in listConfRoomExtensions)
            {
                KickConfRoom(extension);
            }
        }

        ///<summary>Kicks all of the occupants within the passed in conference room.</summary>
        public static void KickConfRoom(long confRoomExtension)
        {
            //No need to check RemotingRole; no call to db.
            Signalods.SetInvalid(InvalidType.PhoneAsteriskReload, KeyType.ConfKick, confRoomExtension);
        }
    }
}