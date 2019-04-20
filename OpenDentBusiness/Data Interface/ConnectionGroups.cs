using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class ConnectionGroups
    {
        #region CachePattern

        private class ConnectionGroupCache : CacheListAbs<ConnectionGroup>
        {
            protected override List<ConnectionGroup> GetCacheFromDb()
            {
                string command = "SELECT * FROM connectiongroup ORDER BY Description";
                return Crud.ConnectionGroupCrud.SelectMany(command);
            }
            protected override List<ConnectionGroup> TableToList(DataTable table)
            {
                return Crud.ConnectionGroupCrud.TableToList(table);
            }
            protected override ConnectionGroup Copy(ConnectionGroup connectionGroup)
            {
                return connectionGroup.Clone();
            }
            protected override DataTable ListToTable(List<ConnectionGroup> listConnectionGroups)
            {
                return Crud.ConnectionGroupCrud.ListToTable(listConnectionGroups, "ConnectionGroup");
            }
            protected override void FillCacheIfNeeded()
            {
                ConnectionGroups.GetTableFromCache(false);
            }
        }

        ///<summary>The object that accesses the cache in a thread-safe manner.</summary>
        private static ConnectionGroupCache _connectionGroupCache = new ConnectionGroupCache();

        public static List<ConnectionGroup> GetDeepCopy(bool isShort = false)
        {
            return _connectionGroupCache.GetDeepCopy(isShort);
        }

        ///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
        public static DataTable RefreshCache()
        {
            return GetTableFromCache(true);
        }

        ///<summary>Fills the local cache with the passed in DataTable.</summary>
        public static void FillCacheFromTable(DataTable table)
        {
            _connectionGroupCache.FillCacheFromTable(table);
        }

        ///<summary>Always refreshes the ClientWeb's cache.</summary>
        public static DataTable GetTableFromCache(bool doRefreshCache)
        {
            return _connectionGroupCache.GetTableFromCache(doRefreshCache);
        }

        #endregion

        ///<summary></summary>
        public static List<ConnectionGroup> Refresh(long patNum)
        {
            string command = "SELECT * FROM connectiongroup ORDER BY Description";
            return Crud.ConnectionGroupCrud.SelectMany(command);
        }

        ///<summary>Inserts, updates, or deletes database rows to match supplied list.</summary>
        public static void Sync(List<ConnectionGroup> listNew)
        {
            ConnectionGroups.RefreshCache();
            Crud.ConnectionGroupCrud.Sync(listNew, ConnectionGroups.GetDeepCopy());
        }

        ///<summary>Gets one ConnectionGroup from the db based on the ConnectionGroupNum.</summary>
        public static ConnectionGroup GetOne(long connectionGroupNum)
        {
            return Crud.ConnectionGroupCrud.SelectOne(connectionGroupNum);
        }

        ///<summary>Gets ConnectionGroups based on description.</summary>
        public static List<ConnectionGroup> GetByDescription(string description)
        {
            string command = "SELECT * FROM connectiongroup WHERE Description LIKE '%" + POut.String(description) + "%'";
            return Crud.ConnectionGroupCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static long Insert(ConnectionGroup connectionGroup)
        {
            return Crud.ConnectionGroupCrud.Insert(connectionGroup);
        }

        ///<summary></summary>
        public static void Update(ConnectionGroup connectionGroup)
        {
            Crud.ConnectionGroupCrud.Update(connectionGroup);
        }

        ///<summary></summary>
        public static void Delete(long connectionGroupNum)
        {
            string command = "DELETE FROM connectiongroup WHERE ConnectionGroupNum = " + POut.Long(connectionGroupNum);
            Db.NonQ(command);
        }
    }
}