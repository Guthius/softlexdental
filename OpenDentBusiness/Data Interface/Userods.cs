using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Services;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using CodeBase;
using ODCrypt;

namespace OpenDentBusiness
{
    ///<summary>(Users OD)</summary>
    public class Userods
    {
        #region Get Methods

        ///<summary>Returns the UserNum of the first non-hidden admin user if they have no password set.
        ///It is very important to order by UserName in order to preserve old behavior of only considering the first Admin user we come across.
        ///This method does not simply return the first admin user with no password.  It is explicit in only considering the FIRST admin user.
        ///Returns 0 if there are no admin users or the first admin user found has a password set.</summary>
        public static long GetFirstSecurityAdminUserNumNoPasswordNoCache()
        {
            //The query will order by UserName in order to preserve old behavior (mimics the cache).
            string command = @"SELECT userod.UserNum,CASE WHEN COALESCE(userod.Password,'')='' THEN 0 ELSE 1 END HasPassword 
				FROM userod
				INNER JOIN usergroupattach ON userod.UserNum=usergroupattach.UserNum
				INNER JOIN grouppermission ON usergroupattach.UserGroupNum=grouppermission.UserGroupNum 
				WHERE userod.IsHidden=0
				AND grouppermission.PermType=" + POut.Int((int)Permissions.SecurityAdmin) + @"
				GROUP BY userod.UserNum
				ORDER BY userod.UserName
				LIMIT 1";
            DataTable table = Db.GetTable(command);
            long userNumAdminNoPass = 0;
            if (table != null && table.Rows.Count > 0 && table.Rows[0]["HasPassword"].ToString() == "0")
            {
                //The first admin user in the database does NOT have a password set.  Return their UserNum.
                userNumAdminNoPass = PIn.Long(table.Rows[0]["UserNum"].ToString());
            }
            return userNumAdminNoPass;
        }

        ///<summary>Gets the corresponding user for the userNum passed in without using the cache.</summary>
        public static User GetUserNoCache(long userNum)
        {
            string command = "SELECT * FROM userod WHERE userod.UserNum=" + POut.Long(userNum);
            return User.SelectOne(command);
        }

        ///<summary>Gets the user name for the userNum passed in.  Returns empty string if not found in the database.</summary>
        public static string GetUserNameNoCache(long userNum)
        {
            string command = "SELECT userod.UserName FROM userod WHERE userod.UserNum=" + POut.Long(userNum);
            return Db.GetScalar(command);
        }

        ///<summary>Returns a list of non-hidden, non-CEMT user names.  Set hasOnlyCEMT to true if you only want non-hidden CEMT users.</summary>
        public static List<string> GetUserNamesNoCache(bool hasOnlyCEMT)
        {
            string command = @"SELECT userod.UserName FROM userod 
				WHERE userod.IsHidden=0 
				AND userod.UserNumCEMT" + (hasOnlyCEMT ? "!=" : "=") + @"0
				ORDER BY userod.UserName";
            return Db.GetListString(command);
        }

        ///<summary>Returns all non-hidden UserNums (key) and UserNames (value) associated with the domain user name passed in.
        ///Returns an empty dictionary if no matches were found.</summary>
        public static Dictionary<long, string> GetUsersByDomainUserNameNoCache(string domainUser)
        {
            string command = @"SELECT userod.UserNum, userod.UserName, userod.DomainUser 
				FROM userod 
				WHERE IsHidden=0";
            //Not sure how to do an InvariantCultureIgnoreCase via a query so doing it over in C# in order to preserve old behavior.
            return Db.GetTable(command).Select()
                .Where(x => PIn.String(x["DomainUser"].ToString()).Equals(domainUser, StringComparison.InvariantCultureIgnoreCase))
                .ToDictionary(x => PIn.Long(x["UserNum"].ToString()), x => PIn.String(x["UserName"].ToString()));
        }

        #endregion

        #region Modification Methods

        #region Insert
        #endregion

        #region Update
        #endregion

        #region Delete
        #endregion

        #endregion

        #region Misc Methods

        ///<summary>Returns true if at least one admin user is present within the database.  Otherwise; false.</summary>
        public static bool HasSecurityAdminUserNoCache()
        {
            string command = @"SELECT COUNT(*) FROM userod
				INNER JOIN usergroupattach ON userod.UserNum=usergroupattach.UserNum
				INNER JOIN grouppermission ON usergroupattach.UserGroupNum=grouppermission.UserGroupNum 
				WHERE userod.IsHidden=0
				AND grouppermission.PermType=" + POut.Int((int)Permissions.SecurityAdmin) + @"
				GROUP BY userod.UserNum";
            return (Db.GetCount(command) != "0");
        }

        ///<summary>Returns true if there are any users (including hidden) with a UserNumCEMT set.  Otherwise; false.</summary>
        public static bool HasUsersForCEMTNoCache()
        {
            string command = @"SELECT COUNT(*) FROM userod
				WHERE userod.UserNumCEMT > 0";
            return (Db.GetCount(command) != "0");
        }

        #endregion

        #region CachePattern

        private class UserodCache : CacheListAbs<User>
        {
            protected override List<User> GetCacheFromDb()
            {
                string command = "SELECT * FROM userod ORDER BY UserName";
                return User.SelectMany(command);
            }
            protected override List<User> TableToList(DataTable table)
            {
                return Crud.UserodCrud.TableToList(table);
            }
            protected override User Copy(User userod)
            {
                return userod.Copy();
            }
            protected override DataTable ListToTable(List<User> listUserods)
            {
                return Crud.UserodCrud.ListToTable(listUserods, "Userod");
            }
            protected override void FillCacheIfNeeded()
            {
                Userods.GetTableFromCache(false);
            }
            protected override bool IsInListShort(User userod)
            {
                return !userod.IsHidden && userod.UserNumCEMT == 0;
            }
        }

        ///<summary>The object that accesses the cache in a thread-safe manner.</summary>
        private static UserodCache _userodCache = new UserodCache();

        public static User GetFirstOrDefault(Func<User, bool> match, bool isShort = false)
        {
            return _userodCache.GetFirstOrDefault(match, isShort);
        }

        ///<summary>Gets a deep copy of all matching items from the cache via ListLong.  Set isShort true to search through ListShort instead.</summary>
        public static List<User> GetWhere(Predicate<User> match, bool isShort = false)
        {
            return _userodCache.GetWhere(match, isShort);
        }

        public static List<User> GetDeepCopy(bool isShort = false)
        {
            return _userodCache.GetDeepCopy(isShort);
        }

        ///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
        public static DataTable RefreshCache()
        {
            return GetTableFromCache(true);
        }

        ///<summary>Fills the local cache with the passed in DataTable.</summary>
        public static void FillCacheFromTable(DataTable table)
        {
            _userodCache.FillCacheFromTable(table);
        }

        ///<summary>Always refreshes the ClientWeb's cache.</summary>
        public static DataTable GetTableFromCache(bool doRefreshCache)
        {
            return _userodCache.GetTableFromCache(doRefreshCache);
        }

        ///<summary>Returns the boolean indicating if the user cache has been turned off or not.</summary>
        public static bool GetIsCacheAllowed()
        {
            return _userodCache.IsCacheAllowed;
        }

        ///<summary>Set isCacheAllowed false to immediately clear out the userod cache and then set the cache into a state where it will throw an
        ///exception if any method attempts to have the cache fill itself.  This is designed to keep sensitive data from being cached until a
        ///verified user has logged in to the program.  Once a user has logged in then it is acceptable to fill the userod cache.</summary>
        public static void SetIsCacheAllowed(bool isCacheAllowed)
        {
            _userodCache.IsCacheAllowed = isCacheAllowed;
        }

        #endregion

        ///<summary></summary>
        public static List<User> GetAll()
        {
            string command = "SELECT * FROM userod ORDER BY UserName";
            return Crud.UserodCrud.TableToList(Db.GetTable(command));
        }

        ///<summary></summary>
        public static User GetUser(long userNum)
        {
            //No need to check RemotingRole; no call to db.
            return GetFirstOrDefault(x => x.UserNum == userNum);
        }

        ///<summary>Returns a list of users from the list of usernums.</summary>
        public static List<User> GetUsers(List<long> listUserNums)
        {
            //No need to check RemotingRole; no call to db.
            return GetWhere(x => listUserNums.Contains(x.UserNum));
        }

        ///<summary>Returns a list of all non-hidden users.  Set includeCEMT to true if you want CEMT users included.</summary>
        public static List<User> GetUsers(bool includeCEMT = false)
        {
            //No need to check RemotingRole; no call to db.
            List<User> retVal = new List<User>();
            List<User> listUsersLong = Userods.GetDeepCopy();
            for (int i = 0; i < listUsersLong.Count; i++)
            {
                if (listUsersLong[i].IsHidden)
                {
                    continue;
                }
                if (!includeCEMT && listUsersLong[i].UserNumCEMT != 0)
                {
                    continue;
                }
                retVal.Add(listUsersLong[i]);
            }
            return retVal;
        }

        ///<summary>Returns a list of all non-hidden users.  Does not include CEMT users.</summary>
        public static List<User> GetUsersByClinic(long clinicNum)
        {
            //No need to check RemotingRole; no call to db.
            return Userods.GetWhere(x => !x.IsHidden)//all non-hidden users
                .FindAll(x => !x.ClinicIsRestricted || x.ClinicNum == clinicNum); //for the given clinic or unassigned to clinic
                                                                                  //CEMT user filter not required. CEMT users SHOULD be unrestricted to a clinic.
        }

        ///<summary>Returns a list of all users without using the local cache.  Useful for multithreaded connections.</summary>
        public static List<User> GetUsersNoCache()
        {
            List<User> retVal = new List<User>();
            string command = "SELECT * FROM userod";
            DataTable tableUsers = Db.GetTable(command);
            retVal = Crud.UserodCrud.TableToList(tableUsers);
            return retVal;
        }

        ///<summary>Returns a list of all CEMT users.</summary>
        public static List<User> GetUsersForCEMT()
        {
            //No need to check RemotingRole; no call to db.
            return GetWhere(x => x.UserNumCEMT != 0);
        }

        ///<summary>Returns null if not found.  Is not case sensitive.  isEcwTight isn't even used.</summary>
        public static User GetUserByName(string userName, bool isEcwTight)
        {
            //No need to check RemotingRole; no call to db.
            return GetFirstOrDefault(x => !x.IsHidden && x.UserName.ToLower() == userName.ToLower());
        }

        ///<summary>Gets the first user with the matching userName passed in.  Not case sensitive.  Returns null if not found.
        ///Does not use the cache to find a corresponding user with the passed in userName.  Every middle tier call passes through here.</summary>
        public static User GetUserByNameNoCache(string userName)
        {
            string command = "SELECT * FROM userod WHERE UserName='" + POut.String(userName) + "'";
            List<User> listUserods = Crud.UserodCrud.TableToList(Db.GetTable(command));
            return listUserods.FirstOrDefault(x => !x.IsHidden && x.UserName.ToLower() == userName.ToLower());
        }

        ///<summary>Returns null if not found.</summary>
        public static User GetUserByEmployeeNum(long employeeNum)
        {
            //No need to check RemotingRole; no call to db.
            return GetFirstOrDefault(x => x.EmployeeNum == employeeNum);
        }

        ///<summary>Returns all users that are associated to the employee passed in.  Returns empty list if no matches found.</summary>
        public static List<User> GetUsersByEmployeeNum(long employeeNum)
        {
            //No need to check RemotingRole; no call to db.
            return GetWhere(x => x.EmployeeNum == employeeNum);
        }

        ///<summary>Returns all users that are associated to the permission passed in.  Returns empty list if no matches found.</summary>
        public static List<User> GetUsersByPermission(Permissions permission, bool showHidden)
        {
            //No need to check RemotingRole; no call to db.
            List<User> listAllUsers = Userods.GetDeepCopy(!showHidden);
            List<User> listUserods = new List<User>();
            for (int i = 0; i < listAllUsers.Count; i++)
            {
                if (GroupPermissions.HasPermission(listAllUsers[i], permission, 0))
                {
                    listUserods.Add(listAllUsers[i]);
                }
            }
            return listUserods;
        }

        ///<summary>Returns all users that are associated to the permission passed in.  Returns empty list if no matches found.</summary>
        public static List<User> GetUsersByJobRole(JobPerm jobPerm, bool showHidden)
        {
            //No need to check RemotingRole; no call to db.
            List<JobPermission> listJobRoles = JobPermissions.GetList().FindAll(x => x.JobPermType == jobPerm);
            return Userods.GetWhere(x => listJobRoles.Any(y => x.UserNum == y.UserNum), !showHidden);
        }

        ///<summary>Gets all non-hidden users that have an associated provider.</summary>
        public static List<User> GetUsersWithProviders()
        {
            //No need to check RemotingRole; no call to db.
            return Userods.GetWhere(x => x.ProvNum != 0, true);
        }

        ///<summary>Returns all users associated to the provider passed in.  Returns empty list if no matches found.</summary>
        public static List<User> GetUsersByProvNum(long provNum)
        {
            //No need to check RemotingRole; no call to db.
            return Userods.GetWhere(x => x.ProvNum == provNum, true);
        }

        public static List<User> GetUsersByInbox(long taskListNum)
        {
            //No need to check RemotingRole; no call to db.
            return Userods.GetWhere(x => x.TaskListInBox == taskListNum, true);
        }

        ///<summary>Returns all users selectable for the insurance verification list.  
        ///Pass in an empty list to not filter by clinic.  
        ///Set isAssigning to false to return only users who have an insurance already assigned.</summary>
        public static List<User> GetUsersForVerifyList(List<long> listClinicNums, bool isAssigning)
        {
            //No need to check RemotingRole; no explicit call to db.
            List<long> listUserNumsInInsVerify = InsVerifies.GetAllInsVerifyUserNums();
            List<long> listUserNumsInClinic = new List<long>();
            if (listClinicNums.Count > 0)
            {
                List<UserClinic> listUserClinics = new List<UserClinic>();
                for (int i = 0; i < listClinicNums.Count; i++)
                {
                    listUserNumsInClinic.AddRange(UserClinics.GetForClinic(listClinicNums[i]).Select(y => y.UserNum).Distinct().ToList());
                }
                listUserNumsInClinic.AddRange(GetUsers().FindAll(x => !x.ClinicIsRestricted).Select(x => x.UserNum).Distinct().ToList());//Always add unrestricted users into the list.
                listUserNumsInClinic = listUserNumsInClinic.Distinct().ToList();//Remove duplicates that could possibly be in the list.
                if (listUserNumsInClinic.Count > 0)
                {
                    listUserNumsInInsVerify = listUserNumsInInsVerify.FindAll(x => listUserNumsInClinic.Contains(x));
                }
                listUserNumsInInsVerify.AddRange(GetUsers(listUserNumsInInsVerify).FindAll(x => !x.ClinicIsRestricted).Select(x => x.UserNum).Distinct().ToList());//Always add unrestricted users into the list.
                listUserNumsInInsVerify = listUserNumsInInsVerify.Distinct().ToList();
            }
            List<User> listUsersWithPerm = GetUsersByPermission(Permissions.InsPlanVerifyList, false);
            if (isAssigning)
            {
                if (listClinicNums.Count == 0)
                {
                    return listUsersWithPerm;//Return unfiltered list of users with permission
                }
                //Don't limit user list to already assigned insurance verifications.
                return listUsersWithPerm.FindAll(x => listUserNumsInClinic.Contains(x.UserNum));//Return users with permission, limited by their clinics
            }
            return listUsersWithPerm.FindAll(x => listUserNumsInInsVerify.Contains(x.UserNum));//Return users limited by permission, clinic, and having an insurance already assigned.
        }

        ///<summary>Returns all non-hidden users associated with the domain user name passed in. Returns an empty list if no matches found.</summary>
        public static List<User> GetUsersByDomainUserName(string domainUser)
        {
            return Userods.GetWhere(x => x.DomainUser.Equals(domainUser, StringComparison.InvariantCultureIgnoreCase), true);
        }

        ///<summary>This handles situations where we have a usernum, but not a user.  And it handles usernum of zero.</summary>
        public static string GetName(long userNum)
        {
            //No need to check RemotingRole; no call to db.
            User user = GetFirstOrDefault(x => x.UserNum == userNum);
            return (user == null ? "" : user.UserName);
        }

        ///<summary>Returns true if the user passed in is associated with a provider that has (or had) an EHR prov key.</summary>
        public static bool IsUserCpoe(User user)
        {
            //No need to check RemotingRole; no call to db.
            if (user == null)
            {
                return false;
            }
            Provider prov = Providers.GetProv(user.ProvNum);
            if (prov == null)
            {
                return false;
            }
            //Check to see if this provider has had a valid key at any point in history.
            return EhrProvKeys.HasProvHadKey(prov.LName, prov.FName);
        }

        ///<summary>Searches the database for a corresponding user by username (not case sensitive).  Returns null is no match found.
        ///Once a user has been found, if the number of failed log in attempts exceeds the limit an exception is thrown with a message to display to the 
        ///user.  Then the hash of the plaintext password (if usingEcw is true, password needs to be hashed before passing into this method) is checked 
        ///against the password hash that is currently in the database.  Once the plaintext password passed in is validated, this method will upgrade the 
        ///hashing algorithm for the password (if necessary) and then returns the entire user object for the corresponding user found.  Throws exceptions 
        ///with error message to display to the user if anything goes wrong.  Manipulates the appropriate log in failure columns in the db as 
        ///needed.</summary>
        public static User CheckUserAndPassword(string username, string plaintext, bool isEcw)
        {
            //Do not use the cache here because an administrator could have cleared the log in failure attempt columns for this user.
            //Also, middle tier calls this method every single time a process request comes to it.
            User userDb = GetUserByNameNoCache(username);
            if (userDb == null)
            {
                throw new ODException(Lans.g("Userods", "Invalid username or password."), ODException.ErrorCodes.CheckUserAndPasswordFailed);
            }
            DateTime dateTimeNowDb = MiscData.GetNowDateTime();
            //We found a user via matching just the username passed in.  Now we need to check to see if they have exceeded the log in failure attempts.
            //For now we are hardcoding a 5 minute delay when the user has failed to log in 5 times in a row.  
            //An admin user can reset the password or the failure attempt count for the user failing to log in via the Security window.
            if (userDb.DateTFail.Year > 1880 //The user has failed to log in recently
                && dateTimeNowDb.Subtract(userDb.DateTFail) < TimeSpan.FromMinutes(5) //The last failure has been within the last 5 minutes.
                && userDb.FailedAttempts >= 5) //The user failed 5 or more times.
            {
                throw new ApplicationException(Lans.g("Userods", "Account has been locked due to failed log in attempts."
                    + "\r\nCall your security admin to unlock your account or wait at least 5 minutes."));
            }
            bool isPasswordValid = Authentication.CheckPassword(userDb, plaintext, isEcw);
            User userNew = userDb.Copy();
            //If the last failed log in attempt was more than 5 minutes ago, reset the columns in the database so the user can try 5 more times.
            if (userDb.DateTFail.Year > 1880 && dateTimeNowDb.Subtract(userDb.DateTFail) > TimeSpan.FromMinutes(5))
            {
                userNew.FailedAttempts = 0;
                userNew.DateTFail = DateTime.MinValue;
            }
            if (!isPasswordValid)
            {
                userNew.DateTFail = dateTimeNowDb;
                userNew.FailedAttempts += 1;
            }
            //Synchronize the database with the results of the log in attempt above
            Crud.UserodCrud.Update(userNew, userDb);
            if (isPasswordValid)
            {
                //Upgrade the encryption for the password if this is not an eCW user (eCW uses md5) and the password is using an outdated hashing algorithm.
                if (!isEcw && !string.IsNullOrEmpty(plaintext) && userNew.LoginDetails.HashType != HashTypes.SHA3_512)
                {
                    //Update the password to the default hash type which should be the most secure hashing algorithm possible.
                    Authentication.UpdatePasswordUserod(userNew, plaintext, HashTypes.SHA3_512);
                    //The above method is almost guaranteed to have changed the password for userNew so go back out the db and get the changes that were made.
                    userNew = GetUserNoCache(userNew.UserNum);
                }
                return userNew;
            }
            else
            {//Password was not valid.
                throw new ODException(Lans.g("Userods", "Invalid username or password."), ODException.ErrorCodes.CheckUserAndPasswordFailed);
            }
        }

        //public static void LoadDatabaseInfoFromFile(string configFilePath)
        //{
        //    //No need to check RemotingRole; no call to db.
        //    if (!File.Exists(configFilePath))
        //    {
        //        throw new Exception("Could not find " + configFilePath + " on the web server.");
        //    }
        //    XmlDocument doc = new XmlDocument();
        //    try
        //    {
        //        doc.Load(configFilePath);
        //    }
        //    catch
        //    {
        //        throw new Exception("Web server " + configFilePath + " could not be opened or is in an invalid format.");
        //    }
        //    XPathNavigator Navigator = doc.CreateNavigator();
        //    //always picks the first database entry in the file:
        //    XPathNavigator navConn = Navigator.SelectSingleNode("//DatabaseConnection");//[Database='"+database+"']");
        //    if (navConn == null)
        //    {
        //        throw new Exception(configFilePath + " does not contain a valid database entry.");//database+" is not an allowed database.");
        //    }
        //
        //    #region Verify ApplicationName Config File Value
        //    XPathNavigator configFileNode = navConn.SelectSingleNode("ApplicationName");//usually /OpenDentalServer
        //    if (configFileNode == null)
        //    {//when first updating, this node will not exist in the xml file, so just add it.
        //        try
        //        {
        //            //AppendChild does not affect the position of the XPathNavigator; adds <ApplicationName>/OpenDentalServer<ApplicationName/> to the xml
        //            using (XmlWriter writer = navConn.AppendChild())
        //            {
        //                writer.WriteElementString("ApplicationName", HostingEnvironment.ApplicationVirtualPath);
        //            }
        //            doc.Save(configFilePath);
        //        }
        //        catch { }//do nothing, unable to write to the XML file, move on anyway
        //    }
        //    else if (string.IsNullOrWhiteSpace(configFileNode.Value))
        //    {//empty node, add the Application Virtual Path
        //        try
        //        {
        //            configFileNode.SetValue(HostingEnvironment.ApplicationVirtualPath);//sets value to /OpenDentalServer or whatever they named their app
        //            doc.Save(configFilePath);
        //        }
        //        catch { }//do nothing, unable to write to the XML file, move on anyway
        //    }
        //    else if (configFileNode.Value.ToLower() != HostingEnvironment.ApplicationVirtualPath.ToLower())
        //    {
        //        //the xml node exists and this file already has an Application Virtual Path in it that does not match the name of the IIS attempting to access it
        //        string filePath = ODFileUtils.CombinePaths(Path.GetDirectoryName(configFilePath), HostingEnvironment.ApplicationVirtualPath.Trim('/') + "Config.xml");
        //        throw new Exception("Multiple middle tier servers are potentially trying to connect to the same database.\r\n"
        //            + "This middle tier server cannot connect to the database within the config file found.\r\n"
        //            + "This middle tier server should be using the following config file:\r\n\t" + filePath + "\r\n"
        //            + "The config file is expecting an ApplicationName of:\r\n\t" + HostingEnvironment.ApplicationVirtualPath);
        //    }
        //    #endregion Verify ApplicationName Config File Value
        //
        //
        //    string connString = "", server = "", database = "", mysqlUser = "", mysqlPassword = "", mysqlUserLow = "", mysqlPasswordLow = "";
        //    XPathNavigator navConString = navConn.SelectSingleNode("ConnectionString");
        //    if (navConString != null)
        //    {//If there is a connection string then use it.
        //        connString = navConString.Value;
        //    }
        //    else
        //    {
        //        //return navOne.SelectSingleNode("summary").Value;
        //        //now, get the values for this connection
        //        server = navConn.SelectSingleNode("ComputerName").Value;
        //        database = navConn.SelectSingleNode("Database").Value;
        //        mysqlUser = navConn.SelectSingleNode("User").Value;
        //        mysqlPassword = navConn.SelectSingleNode("Password").Value;
        //        XPathNavigator encryptedPwdNode = navConn.SelectSingleNode("MySQLPassHash");
        //        string decryptedPwd;
        //        if (mysqlPassword == "" && encryptedPwdNode != null && encryptedPwdNode.Value != "" && CDT.Class1.Decrypt(encryptedPwdNode.Value, out decryptedPwd))
        //        {
        //            mysqlPassword = decryptedPwd;
        //        }
        //        mysqlUserLow = navConn.SelectSingleNode("UserLow").Value;
        //        mysqlPasswordLow = navConn.SelectSingleNode("PasswordLow").Value;
        //    }
        //    DataConnection dcon = new DataConnection();
        //    if (connString != "")
        //    {
        //        try
        //        {
        //            dcon.SetDb(connString, "");
        //        }
        //        catch (Exception e)
        //        {
        //            throw new Exception(e.Message + "\r\n" + "Connection to database failed.  Check the values in the config file on the web server " + configFilePath);
        //        }
        //    }
        //    else
        //    {
        //        try
        //        {
        //            dcon.SetDb(server, database, mysqlUser, mysqlPassword, mysqlUserLow, mysqlPasswordLow);
        //        }
        //        catch (Exception e)
        //        {
        //            throw new Exception(e.Message + "\r\n" + "Connection to database failed.  Check the values in the config file on the web server " + configFilePath);
        //        }
        //    }
        //    //todo?: make sure no users have blank passwords.
        //}

        ///<summary>DEPRICATED DO NOT USE.  Use OpenDentBusiness.Authentication class instead.  For middle tier backward-compatability only.</summary>
        public static string HashPassword(string inputPass)
        {
            //No need to check RemotingRole; no call to db.
            bool useEcwAlgorithm = Programs.IsEnabled(ProgramName.eClinicalWorks);
            return HashPassword(inputPass, useEcwAlgorithm);
        }

        ///<summary>DEPRICATED DO NOT USE.  Use OpenDentBusiness.Authentication class instead.  For middle tier backward-compatability only.</summary>
        public static string HashPassword(string inputPass, bool useEcwAlgorithm)
        {
            //No need to check RemotingRole; no call to db.
            if (inputPass == "")
            {
                return "";
            }
            return Authentication.HashPasswordMD5(inputPass, useEcwAlgorithm);
        }

        ///<summary>Updates all students/instructors to the specified user group.  Surround with try/catch because it can throw exceptions.</summary>
        public static void UpdateUserGroupsForDentalSchools(UserGroup userGroup, bool isInstructor)
        {
            string command;
            //Check if the user group that the students or instructors are trying to go to has the SecurityAdmin permission.
            if (!GroupPermissions.HasPermission(userGroup.UserGroupNum, Permissions.SecurityAdmin, 0))
            {
                //We need to make sure that moving these users to the new user group does not eliminate all SecurityAdmin users in db.
                command = "SELECT COUNT(*) FROM usergroupattach "
                    + "INNER JOIN usergroup ON usergroupattach.UserGroupNum=usergroup.UserGroupNum "
                    + "INNER JOIN grouppermission ON grouppermission.UserGroupNum=usergroup.UserGroupNum "
                    + "WHERE usergroupattach.UserNum NOT IN "
                    + "(SELECT userod.UserNum FROM userod,provider "
                        + "WHERE userod.ProvNum=provider.ProvNum ";
                if (!isInstructor)
                {
                    command += "AND provider.IsInstructor=" + POut.Bool(isInstructor) + " ";
                    command += "AND provider.SchoolClassNum!=0) ";
                }
                else
                {
                    command += "AND provider.IsInstructor=" + POut.Bool(isInstructor) + ") ";
                }
                command += "AND grouppermission.PermType=" + POut.Int((int)Permissions.SecurityAdmin) + " ";
                int lastAdmin = PIn.Int(Db.GetCount(command));
                if (lastAdmin == 0)
                {
                    throw new Exception("Cannot move students or instructors to the new user group because it would leave no users with the SecurityAdmin permission.");
                }
            }
            command = "UPDATE userod INNER JOIN provider ON userod.ProvNum=provider.ProvNum "
                    + "SET UserGroupNum=" + POut.Long(userGroup.UserGroupNum) + " "
                    + "WHERE provider.IsInstructor=" + POut.Bool(isInstructor);
            if (!isInstructor)
            {
                command += " AND provider.SchoolClassNum!=0";
            }
            Db.NonQ(command);
        }

        ///<summary>Surround with try/catch because it can throw exceptions.</summary>
        public static void Update(User userod, List<long> listUserGroupNums = null)
        {
            Validate(false, userod, false, listUserGroupNums);
            Crud.UserodCrud.Update(userod);
            if (listUserGroupNums == null)
            {
                return;
            }
            UserGroupAttaches.SyncForUser(userod, listUserGroupNums);
        }

        ///<summary>Update for CEMT only.  Used when updating Remote databases with information from the CEMT.  Because of potentially different primary keys we have to update based on UserNumCEMT.</summary>
        public static void UpdateCEMT(User userod)
        {
            //This should never happen, but is a failsafe to prevent the overwriting of all non-CEMT users in the remote database.
            if (userod.UserNumCEMT == 0)
            {
                return;
            }
            //Validate(false,userod,false);//Can't use this validate. it's for normal updating only.
            string command = "UPDATE userod SET "
                + "UserName          = '" + POut.String(userod.UserName) + "', "
                + "Password          = '" + POut.String(userod.Password) + "', "
                //+"UserGroupNum      =  "+POut.Long(userod.UserGroupNum)+", "//need to find primary key of remote user group
                + "EmployeeNum       =  " + POut.Long(userod.EmployeeNum) + ", "
                + "ClinicNum         =  " + POut.Long(userod.ClinicNum) + ", "
                + "ProvNum           =  " + POut.Long(userod.ProvNum) + ", "
                + "IsHidden          =  " + POut.Bool(userod.IsHidden) + ", "
                + "TaskListInBox     =  " + POut.Long(userod.TaskListInBox) + ", "
                + "AnesthProvType    =  " + POut.Int(userod.AnesthProvType) + ", "
                + "DefaultHidePopups =  " + POut.Bool(userod.DefaultHidePopups) + ", "
                + "PasswordIsStrong  =  " + POut.Bool(userod.PasswordIsStrong) + ", "
                + "ClinicIsRestricted=  " + POut.Bool(userod.ClinicIsRestricted) + ", "
                + "InboxHidePopups   =  " + POut.Bool(userod.InboxHidePopups) + " "
                + "WHERE UserNumCEMT = " + POut.Long(userod.UserNumCEMT);
            Db.NonQ(command);
        }

        ///<summary>DEPRICATED DO NOT USE. Use OpenDentBusiness.Authentication class instead.  For middle tier backward-compatability only.</summary>
        public static void UpdatePassword(User userod, string newPassHashed, bool isPasswordStrong)
        {
            //Before 18.3, we only used MD5
            UpdatePassword(userod, new PasswordContainer(HashTypes.MD5, "", newPassHashed), isPasswordStrong);
        }

        ///<summary>Surround with try/catch because it can throw exceptions.
        ///Same as Update(), only the Validate call skips checking duplicate names for hidden users.</summary>
        public static void UpdatePassword(User userod, PasswordContainer loginDetails, bool isPasswordStrong)
        {
            User userToUpdate = userod.Copy();
            userToUpdate.LoginDetails = loginDetails;
            userToUpdate.PasswordIsStrong = isPasswordStrong;
            List<UserGroup> listUserGroups = userToUpdate.GetGroups(); //do not include CEMT users.
            if (listUserGroups.Count < 1)
            {
                throw new Exception(Lans.g("Userods", "The current user must be in at least one user group."));
            }
            Validate(false, userToUpdate, true, listUserGroups.Select(x => x.UserGroupNum).ToList());
            Crud.UserodCrud.Update(userToUpdate);
        }

        ///<summary>A user must always have at least one associated userGroupAttach. Pass in the usergroup(s) that should be attached.
        ///Surround with try/catch because it can throw exceptions.</summary>
        public static long Insert(User userod, List<long> listUserGroupNums, bool isForCEMT = false)
        {
            if (userod.IsHidden && UserGroups.IsAdminGroup(listUserGroupNums))
            {
                throw new Exception(Lans.g("Userods", "Admins cannot be hidden."));
            }
            Validate(true, userod, false, listUserGroupNums);
            long userNum = Crud.UserodCrud.Insert(userod);
            UserGroupAttaches.SyncForUser(userod, listUserGroupNums);
            if (isForCEMT)
            {
                userod.UserNumCEMT = userNum;
                Crud.UserodCrud.Update(userod);
            }
            return userNum;
        }

        public static long InsertNoCache(User userod)
        {
            return Crud.UserodCrud.InsertNoCache(userod);
        }

        ///<summary>Surround with try/catch because it can throw exceptions.  
        ///We don't really need to make this public, but it's required in order to follow the RemotingRole pattern.
        ///listUserGroupNum can only be null when validating for an Update.</summary>
        public static void Validate(bool isNew, User user, bool excludeHiddenUsers, List<long> listUserGroupNum)
        {
            //should add a check that employeenum and provnum are not both set.
            //make sure username is not already taken
            string command;
            long excludeUserNum;
            if (isNew)
            {
                excludeUserNum = 0;
            }
            else
            {
                excludeUserNum = user.UserNum;//it's ok if the name matches the current username
            }
            //It doesn't matter if the UserName is already in use if the user being updated is going to be hidden.  This check will block them from unhiding duplicate users.
            if (!user.IsHidden)
            {//if the user is now not hidden
             //CEMT users will not be visible from within Open Dental.  Therefore, make a different check so that we can know if the name
             //the user typed in is a duplicate of a CEMT user.  In doing this, we are able to give a better message.
                if (!IsUserNameUnique(user.UserName, excludeUserNum, excludeHiddenUsers, true))
                {
                    throw new ApplicationException(Lans.g("Userods", "UserName already in use by CEMT member."));
                }
                if (!IsUserNameUnique(user.UserName, excludeUserNum, excludeHiddenUsers))
                {
                    //IsUserNameUnique doesn't care if it's a CEMT user or not.. It just gets a count based on username.
                    throw new ApplicationException(Lans.g("Userods", "UserName already in use."));
                }
            }
            if (listUserGroupNum == null)
            {//Not validating UserGroup selections.
                return;
            }
            if (listUserGroupNum.Count < 1)
            {
                throw new ApplicationException(Lans.g("Userods", "The current user must be in at least one user group."));
            }
            //an admin user can never be hidden
            command = "SELECT COUNT(*) FROM grouppermission "
                + "WHERE PermType='" + POut.Long((int)Permissions.SecurityAdmin) + "' "
                + "AND UserGroupNum IN (" + string.Join(",", listUserGroupNum) + ") ";
            if (!isNew//Updating.
                && Db.GetCount(command) == "0"//if this user would not have admin
                && !IsSomeoneElseSecurityAdmin(user))//make sure someone else has admin
            {
                throw new ApplicationException(Lans.g("Users", "At least one user must have Security Admin permission."));
            }
            if (user.IsHidden//hidden 
                && user.UserNumCEMT == 0//and non-CEMT
                && Db.GetCount(command) != "0")//if this user is admin
            {
                throw new ApplicationException(Lans.g("Userods", "Admins cannot be hidden."));
            }
        }

        /// <summary>Returns true if there is at least one user part of the SecurityAdmin permission excluding the user passed in.</summary>
        public static bool IsSomeoneElseSecurityAdmin(User user)
        {
            string command = "SELECT COUNT(*) FROM userod "
                + "INNER JOIN usergroupattach ON usergroupattach.UserNum=userod.UserNum "
                + "INNER JOIN grouppermission ON usergroupattach.UserGroupNum=grouppermission.UserGroupNum "
                + "WHERE grouppermission.PermType='" + POut.Long((int)Permissions.SecurityAdmin) + "'"
                + " AND userod.IsHidden =0"
                + " AND userod.UserNum != " + POut.Long(user.UserNum);
            if (Db.GetCount(command) == "0")
            {//there are no other users with this permission
                return false;
            }
            return true;
        }

        public static bool IsUserNameUnique(string username, long excludeUserNum, bool excludeHiddenUsers)
        {
            return IsUserNameUnique(username, excludeUserNum, excludeHiddenUsers, false);
        }

        ///<summary>Supply 0 or -1 for the excludeUserNum to not exclude any.</summary>
        public static bool IsUserNameUnique(string username, long excludeUserNum, bool excludeHiddenUsers, bool searchCEMTUsers)
        {
            if (username == "")
            {
                return false;
            }
            string command = "SELECT COUNT(*) FROM userod WHERE ";
            //if(Programs.UsingEcwTight()){
            //	command+="BINARY ";//allows different usernames based on capitalization.//we no longer allow this
            //Does not need to be tested under Oracle because eCW users do not use Oracle.
            //}
            command += "UserName='" + POut.String(username) + "' "
                + "AND UserNum !=" + POut.Long(excludeUserNum) + " ";
            if (excludeHiddenUsers)
            {
                command += "AND IsHidden=0 ";//not hidden
            }
            if (searchCEMTUsers)
            {
                command += "AND UserNumCEMT!=0";
            }
            DataTable table = Db.GetTable(command);
            if (table.Rows[0][0].ToString() == "0")
            {
                return true;
            }
            return false;
        }

        ///<summary>Used in FormSecurity.FillTreeUsers</summary>
        public static List<User> GetForGroup(long userGroupNum)
        {
            //No need to check RemotingRole; no call to db.
            return GetWhere(x => x.IsInUserGroup(userGroupNum));
        }

        ///<summary>Gets a list of users for which the passed-in clinicNum is the only one they have access to.</summary>
        public static List<User> GetUsersOnlyThisClinic(long clinicNum)
        {
            string command = "SELECT userod.* "
            + "FROM( "
                + "SELECT userclinic.UserNum,COUNT(userclinic.ClinicNum) Clinics FROM userclinic "
                + "GROUP BY userNum "
                + "HAVING Clinics = 1 "
            + ") users "
            + "INNER JOIN userclinic ON userclinic.UserNum = users.UserNum "
                + "AND userclinic.ClinicNum = " + POut.Long(clinicNum) + " "
            + "INNER JOIN userod ON userod.UserNum = userclinic.UserNum ";
            return User.SelectMany(command);
        }

        /// <summary>Will return 0 if no inbox found for user.</summary>
        public static long GetInbox(long userNum)
        {
            //No need to check RemotingRole; no call to db.
            User userod = GetFirstOrDefault(x => x.UserNum == userNum);
            return (userod == null ? 0 : userod.TaskListInBox);
        }

        ///<summary>Returns 3, which is non-admin provider type, if no match found.</summary>
        public static long GetAnesthProvType(long anesthProvType)
        {
            //No need to check RemotingRole; no call to db.
            User userod = GetFirstOrDefault(x => x.AnesthProvType == anesthProvType);
            return (userod == null ? 3 : userod.AnesthProvType);
        }

        public static List<User> GetUsersForJobs()
        {
            string command = "SELECT * FROM userod "
                + "INNER JOIN jobpermission ON userod.UserNum=jobpermission.UserNum "
                + "WHERE IsHidden=0 GROUP BY userod.UserNum ORDER BY UserName";
            return User.SelectMany(command);
        }

        ///<summary>Returns empty string if password is strong enough.  Otherwise, returns explanation of why it's not strong enough.</summary>
        public static string IsPasswordStrong(string pass)
        {
            //No need to check RemotingRole; no call to db.
            if (pass == "")
            {
                return Lans.g("FormUserPassword", "Password may not be blank when the strong password feature is turned on.");
            }
            if (pass.Length < 8)
            {
                return Lans.g("FormUserPassword", "Password must be at least eight characters long when the strong password feature is turned on.");
            }
            bool containsCap = false;
            for (int i = 0; i < pass.Length; i++)
            {
                if (Char.IsUpper(pass[i]))
                {
                    containsCap = true;
                }
            }
            if (!containsCap)
            {
                return Lans.g("FormUserPassword", "Password must contain at least one capital letter when the strong password feature is turned on.");
            }
            bool containsLower = false;
            for (int i = 0; i < pass.Length; i++)
            {
                if (Char.IsLower(pass[i]))
                {
                    containsLower = true;
                }
            }
            if (!containsLower)
            {
                return Lans.g("FormUserPassword", "Password must contain at least one lower case letter when the strong password feature is turned on.");
            }
            if (Preferences.GetBool(PrefName.PasswordsStrongIncludeSpecial))
            {
                bool hasSpecial = false;
                for (int i = 0; i < pass.Length; i++)
                {
                    if (!Char.IsLetterOrDigit(pass[i]))
                    {
                        hasSpecial = true;
                        break;
                    }
                }
                if (!hasSpecial)
                {
                    return Lans.g("FormUserPassword", "Password must contain at least one special character when the 'strong passwords require a special character' feature is turned on.");
                }
            }
            bool containsNum = false;
            for (int i = 0; i < pass.Length; i++)
            {
                if (Char.IsNumber(pass[i]))
                {
                    containsNum = true;
                }
            }
            if (!containsNum)
            {
                return Lans.g("FormUserPassword", "Password must contain at least one number when the strong password feature is turned on.");
            }
            return "";
        }

        ///<summary>This resets the strong password flag on all users after an admin turns off pref PasswordsMustBeStrong.  If strong passwords are again turned on later, then each user will have to edit their password in order set the strong password flag again.</summary>
        public static void ResetStrongPasswordFlags()
        {
            string command = "UPDATE userod SET PasswordIsStrong=0";
            Db.NonQ(command);
        }

        ///<summary>Returns true if the passed-in user is apart of the passed-in usergroup.</summary>
        public static bool IsInUserGroup(long userNum, long userGroupNum)
        {
            //No need to check RemotingRole; no call to db.
            List<UserGroupAttach> listAttaches = UserGroupAttaches.GetForUser(userNum);
            return listAttaches.Select(x => x.UserGroupNum).Contains(userGroupNum);
        }
    }
}