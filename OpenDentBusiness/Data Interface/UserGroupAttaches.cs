using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class UserGroupAttaches
    {
        //If this table type will exist as cached data, uncomment the Cache Pattern region below and edit.

        #region Cache Pattern
        //This region can be eliminated if this is not a table type with cached data.
        //If leaving this region in place, be sure to add GetTableFromCache and FillCacheFromTable to the Cache.cs file with all the other Cache types.
        //Also, consider making an invalid type for this class in Cache.GetAllCachedInvalidTypes() if needed.

        private class UserGroupAttachCache : CacheListAbs<UserGroupAttach>
        {
            protected override List<UserGroupAttach> GetCacheFromDb()
            {
                string command = "SELECT * FROM usergroupattach";
                return Crud.UserGroupAttachCrud.SelectMany(command);
            }
            protected override List<UserGroupAttach> TableToList(DataTable table)
            {
                return Crud.UserGroupAttachCrud.TableToList(table);
            }
            protected override UserGroupAttach Copy(UserGroupAttach userGroupAttach)
            {
                return userGroupAttach.Copy();
            }
            protected override DataTable ListToTable(List<UserGroupAttach> listUserGroupAttaches)
            {
                return Crud.UserGroupAttachCrud.ListToTable(listUserGroupAttaches, "UserGroupAttach");
            }
            protected override void FillCacheIfNeeded()
            {
                UserGroupAttaches.GetTableFromCache(false);
            }
            //protected override bool IsInListShort(UserGroupAttach userGroupAttach) {
            //	return true;//Either change this method or delete it.
            //}
        }

        ///<summary>The object that accesses the cache in a thread-safe manner.</summary>
        private static UserGroupAttachCache _userGroupAttachCache = new UserGroupAttachCache();

        public static List<UserGroupAttach> GetWhere(Predicate<UserGroupAttach> match, bool isShort = false)
        {
            return _userGroupAttachCache.GetWhere(match, isShort);
        }

        ///<summary>Fills the local cache with the passed in DataTable.</summary>
        public static void FillCacheFromTable(DataTable table)
        {
            //No need to check RemotingRole; no call to db.
            _userGroupAttachCache.FillCacheFromTable(table);
        }

        ///<summary>Returns the cache in the form of a DataTable. Always refreshes the ClientWeb's cache.</summary>
        ///<param name="doRefreshCache">If true, will refresh the cache if RemotingRole is ClientDirect or ServerWeb.</param> 
        public static DataTable GetTableFromCache(bool doRefreshCache)
        {
            return _userGroupAttachCache.GetTableFromCache(doRefreshCache);
        }

        public static void RefreshCache()
        {
            GetTableFromCache(true);
        }
        #endregion Cache Pattern

        #region Get Methods
        ///<summary>Returns all usergroupattaches for a single user from the cache.</summary>
        public static List<UserGroupAttach> GetForUser(long userNum)
        {
            //No need to check RemotingRole; no call to db.
            return GetWhere(x => x.UserNum == userNum);
        }

        public static List<UserGroupAttach> GetForUserGroup(long usergroupNum)
        {
            //No need to check RemotingRole; no call to db.
            return GetWhere(x => x.UserGroupNum == usergroupNum);
        }

        ///<summary>Gets all UserGroupAttaches from the database where the associated users or usergroups' CEMTNums are not 0.</summary>
        public static List<UserGroupAttach> GetForCEMTUsersAndUserGroups()
        {
            string command = @"
				SELECT usergroupattach.* 
				FROM usergroupattach
				INNER JOIN userod ON userod.UserNum = usergroupattach.UserNum
					AND userod.UserNumCEMT != 0";
            return Crud.UserGroupAttachCrud.SelectMany(command);
        }

        #endregion
        #region Modification Methods
        #region Insert
        ///<summary></summary>
        public static long Insert(UserGroupAttach userGroupAttach)
        {
            return Crud.UserGroupAttachCrud.Insert(userGroupAttach);
        }
        #endregion

        #region Delete
        public static void Delete(UserGroupAttach userGroupAttach)
        {
            Crud.UserGroupAttachCrud.Delete(userGroupAttach.UserGroupAttachNum);
        }

        ///<summary>Does not add a new usergroupattach if the passed-in userCur is already attached to userGroup.</summary>
        public static void AddForUser(User userCur, int userGroupNum)
        {
            if (!userCur.IsInUserGroup(userGroupNum))
            {
                UserGroupAttach userGroupAttach = new UserGroupAttach();
                userGroupAttach.UserGroupNum = userGroupNum;
                userGroupAttach.UserNum = userCur.UserNum;
                Crud.UserGroupAttachCrud.Insert(userGroupAttach);
            }
        }

        ///<summary>Pass in the user and all of the userGroups that the user should be attached to.
        ///Detaches the userCur from any usergroups that are not in the given list.
        ///Returns a count of how many user group attaches were affected.</summary>
        public static long SyncForUser(User userCur, List<long> listUserGroupNums)
        {
            long rowsChanged = 0;
            foreach (int userGroupNum in listUserGroupNums)
            {
                if (!userCur.IsInUserGroup(userGroupNum))
                {
                    UserGroupAttach userGroupAttach = new UserGroupAttach();
                    userGroupAttach.UserGroupNum = userGroupNum;
                    userGroupAttach.UserNum = userCur.UserNum;
                    Crud.UserGroupAttachCrud.Insert(userGroupAttach);
                    rowsChanged++;
                }
            }
            foreach (UserGroupAttach userGroupAttach in UserGroupAttaches.GetForUser(userCur.UserNum))
            {
                if (!listUserGroupNums.Contains(userGroupAttach.UserGroupNum))
                {
                    Crud.UserGroupAttachCrud.Delete(userGroupAttach.UserGroupAttachNum);
                    rowsChanged++;
                }
            }
            return rowsChanged;
        }

        #endregion
        #endregion
        #region Misc Methods

        #endregion
    }
}