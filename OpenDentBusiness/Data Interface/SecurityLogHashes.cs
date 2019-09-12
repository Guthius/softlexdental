using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Security.Cryptography;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class SecurityLogHashes
    {
        #region Get Methods
        public static SecurityLogHash GetOne(long securityLogHashNum)
        {
            return Crud.SecurityLogHashCrud.SelectOne(securityLogHashNum);
        }
        #endregion


        public static void DeleteWithMaxPriKey(long maxSecurityLogHashNum)
        {
            if (maxSecurityLogHashNum == 0)
            {
                return;
            }
            string command = "DELETE FROM securityloghash WHERE SecurityLogHashNum <= " + POut.Long(maxSecurityLogHashNum);
            Db.NonQ(command);
        }

        ///<summary>Inserts securityloghash into Db.</summary>
        public static long Insert(SecurityLogHash securityLogHash)
        {
            return Crud.SecurityLogHashCrud.Insert(securityLogHash);
        }

        ///<summary>Insertion logic that doesn't use the cache. Has special cases for generating random PK's and handling Oracle insertions.</summary>
        public static long InsertNoCache(SecurityLogHash securityLogHash)
        {
            return Crud.SecurityLogHashCrud.InsertNoCache(securityLogHash);
        }

        ///<summary>Creates a new SecurityLogHash entry in the Db.</summary>
        public static void InsertSecurityLogHash(long securityLogNum)
        {
            SecurityLog securityLog = SecurityLogs.GetOne(securityLogNum); //need a fresh copy because of time stamps, etc.
                                                                           //Attempted fix for NADG problems with SecurityLogHash Insert attempts throwing null reference UEs. Job #695
            if (securityLog == null)
            {
                System.Threading.Thread.Sleep(100);
                securityLog = SecurityLogs.GetOne(securityLogNum); //need a fresh copy because of time stamps, etc.
            }
            if (securityLog == null)
            {
                //We give up at this point.  The end result will be the securitylog row shows up as RED in the audit trail.
                //We don't want other things to fail/practice flow to be interrupted just because of securitylog issues.
                return;
            }
            SecurityLogHash securityLogHash = new SecurityLogHash();
            //Set the FK
            securityLogHash.SecurityLogNum = securityLog.SecurityLogNum;
            //Hash the securityLog
            securityLogHash.LogHash = GetHashString(securityLog);
            Insert(securityLogHash);
        }

        ///<summary>Used for inserting without using the cache.  Usually used when multithreading connections.</summary>
        public static long InsertSecurityLogHashNoCache(long securityLogNum)
        {
            SecurityLog securityLog = Crud.SecurityLogCrud.SelectOne(securityLogNum);
            SecurityLogHash securityLogHash = new SecurityLogHash();
            securityLogHash.SecurityLogNum = securityLog.SecurityLogNum;
            securityLogHash.LogHash = GetHashString(securityLog);
            return InsertNoCache(securityLogHash);
        }

        ///<summary></summary>
        public static void InsertMany(List<SecurityLogHash> listSecurityLogHashes)
        {
            Crud.SecurityLogHashCrud.InsertMany(listSecurityLogHashes);
        }

        ///<summary>Does not make a call to the db.  Returns a SHA-256 hash of the entire security log.  Length of 32 bytes.  Only called from CreateSecurityLogHash() and FormAudit.FillGrid()</summary>
        public static string GetHashString(SecurityLog securityLog)
        {
            //No need to check RemotingRole; no call to db.
            HashAlgorithm algorithm = SHA256.Create();
            //Build string to hash
            string logString = "";
            //logString+=securityLog.SecurityLogNum;
            logString += securityLog.PermType;
            logString += securityLog.UserNum;
            logString += POut.DateT(securityLog.LogDateTime, false);
            logString += securityLog.LogText;
            //logString+=securityLog.CompName;
            logString += securityLog.PatNum;
            //logString+=securityLog.FKey.ToString();
            if (securityLog.DateTPrevious != DateTime.MinValue)
            {
                logString += POut.DateT(securityLog.DateTPrevious, false);
            }
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(logString);
            byte[] hashbytes = algorithm.ComputeHash(unicodeBytes);
            return Convert.ToBase64String(hashbytes);
        }
    }
}