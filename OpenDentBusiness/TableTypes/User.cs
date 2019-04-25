using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenDentBusiness
{
    public class User : ODTable
    {
        ///<summary>Primary key.</summary>
        [ODTableColumn(PrimaryKey = true)]
        public long UserNum;
        ///<summary>.</summary>
        public string UserName;
        ///<summary>The password details in a "HashType$Salt$Hash" format, separating the different fields by '$'.
        ///This is NOT the actual password but the encoded password hash.
        ///If the contents of this variable are not in the aforementioned format, it is assumed to be a legacy password hash (MD5).</summary>
        public string Password;
        ///<summary>Deprecated. Use UserGroupAttaches to link Userods to UserGroups.</summary>
        public long UserGroupNum;
        ///<summary>FK to employee.EmployeeNum. Cannot be used if provnum is used. Used for timecards to block access by other users.</summary>
        public long EmployeeNum;
        ///<summary>FK to clinic.ClinicNum.  Default clinic for this user.  It causes new patients to default to this clinic when entered by this user.  
        ///If 0, then user has no default clinic or default clinic is HQ if clinics are enabled.</summary> 		
        public long ClinicNum;
        ///<summary>FK to provider.ProvNum.  Cannot be used if EmployeeNum is used.  It is possible to have multiple userods attached to a single provider.</summary>
        public long ProvNum;
        ///<summary>Set true to hide user from login list.</summary>
        public bool IsHidden;
        ///<summary>FK to tasklist.TaskListNum.  0 if no inbox setup yet.  It is assumed that the TaskList is in the main trunk, but this is not strictly enforced.  User can't delete an attached TaskList, but they could move it.</summary>
        public long TaskListInBox;
        /// <summary> Defaults to 3 (regular user) unless specified. Helps populates the Anesthetist, Surgeon, Assistant and Circulator dropdowns properly on FormAnestheticRecord/// </summary>
        public int AnesthProvType;
        ///<summary>If set to true, the BlockSubsc button will start out pressed for this user.</summary>
        public bool DefaultHidePopups;
        ///<summary>Gets set to true if strong passwords are turned on, and this user changes their password to a strong password.  We don't store actual passwords, so this flag is the only way to tell.</summary>
        public bool PasswordIsStrong;
        ///<summary>Only used when userod.ClinicNum is set to not be zero.  Prevents user from having access to other clinics.</summary>
        public bool ClinicIsRestricted;
        ///<summary>If set to true, the BlockInbox button will start out pressed for this user.</summary>
        public bool InboxHidePopups;
        ///<summary>FK to userod.UserNum.  The user num within the Central Manager database.  Only editable via CEMT.  Can change when CEMT syncs.</summary>
        public long UserNumCEMT;
        ///<summary>The date and time of the most recent log in failure for this user.  Set to MinValue after user logs in successfully.</summary>
        [ODTableColumn(SpecialType = CrudSpecialColType.DateT)]
        public DateTime DateTFail;
        ///<summary>The number of times this user has failed to log into their account.  Set to 0 after user logs in successfully.</summary>
        public byte FailedAttempts;
        /// <summary>The username for the ActiveDirectory user to link the account to.</summary>
        public string DomainUser;
        ///<summary>Boolean.  If true, the user's password needs to be reset on next login.</summary>
        public bool IsPasswordResetRequired;

        /// <summary>
        /// The getter will return a struct created from the database-ready password which is stored in the Password field.
        /// The setter will manipulate the Password variable to the string representation of this PasswordContainer object.
        /// </summary>
        public PasswordContainer LoginDetails
        {
            get
            {
                return Authentication.DecodePass(Password);
            }
            set
            {
                Password = value.ToString();
            }
        }

        /// <summary>
        /// The password hash, not the actual password. 
        /// If no password has been entered, then this will be blank.
        /// </summary>
        public string PasswordHash
        {
            get
            {
                return LoginDetails.Hash;
            }
        }

        /// <summary>
        /// All valid users should NOT set this value to anything other than None otherwise permission checking will act unexpectedly.
        /// Programmatically set this value from the init method of the corresponding eService. Helps prevent unhandled exceptions.
        /// Custom property only meant to be used via eServices. Not a column in db.  Not to be used in middle tier environment.
        /// </summary>
        [XmlIgnore]
        public EServiceTypes EServiceType { get; set; }


        public User()
        {
        }

        public User Copy()
        {
            return (User)MemberwiseClone();
        }

        public override string ToString()
        {
            return UserName;
        }

        public bool IsInUserGroup(long userGroupNum)
        {
            return Userods.IsInUserGroup(UserNum, userGroupNum);
        }

        /// <summary>
        /// Gets all of the usergroups attached to this user.
        /// </summary>
        public List<UserGroup> GetGroups(bool includeCEMT = false)
        {
            return UserGroups.GetForUser(UserNum, includeCEMT);
        }

        /// <summary>
        /// Gets one <see cref="User"/> object from the database using the primary key. 
        /// Returns null if not found.
        /// </summary>
        public static User SelectOne(long userId) => SelectOne("SELECT * FROM userod WHERE UserNum = " + userId);

        /// <summary>
        /// Gets one <see cref="User"/> object from the database using a query.
        /// </summary>
        public static User SelectOne(string command) => TableToList(Db.GetTable(command)).FirstOrDefault();
        

        /// <summary>
        /// Gets a list of Userod objects from the database using a query.
        /// </summary>
        public static List<User> SelectMany(string command) => TableToList(Db.GetTable(command));
        

        public static List<User> TableToList(DataTable table)
        {
            var users = new List<User>();

            foreach (DataRow row in table.Rows)
            {
                var user = new User
                {
                    UserNum                 = Convert.ToInt64(row["UserNum"]),
                    UserName                = Convert.ToString(row["UserName"]),
                    Password                = Convert.ToString(row["Password"]),
                    UserGroupNum            = Convert.ToInt64(row["UserGroupNum"]),
                    EmployeeNum             = Convert.ToInt64(row["EmployeeNum"]),
                    ClinicNum               = Convert.ToInt64(row["ClinicNum"]),
                    ProvNum                 = Convert.ToInt64(row["ProvNum"]),
                    IsHidden                = Convert.ToBoolean(row["IsHidden"]),
                    TaskListInBox           = Convert.ToInt64(row["TaskListInBox"]),
                    AnesthProvType          = Convert.ToInt32(row["AnesthProvType"]),
                    DefaultHidePopups       = Convert.ToBoolean(row["DefaultHidePopups"]),
                    PasswordIsStrong        = Convert.ToBoolean(row["PasswordIsStrong"]),
                    ClinicIsRestricted      = Convert.ToBoolean(row["ClinicIsRestricted"]),
                    InboxHidePopups         = Convert.ToBoolean(row["InboxHidePopups"]),
                    UserNumCEMT             = Convert.ToInt64(row["UserNumCEMT"]),
                    DateTFail               = PIn.DateT(row["DateTFail"].ToString()),
                    FailedAttempts          = Convert.ToByte(row["FailedAttempts"]),
                    DomainUser              = Convert.ToString(row["DomainUser"]),
                    IsPasswordResetRequired = Convert.ToBoolean(row["IsPasswordResetRequired"])
                };
                users.Add(user);
            }

            return users;
        }
    }

    ///<summary></summary>
    public enum EServiceTypes
    {
        /// <summmary>
        /// Not an eService user. All valid users should be this type otherwise permission checking will act differently.
        /// </summmary>
        None,

        EConnector,
        Broadcaster,
        BroadcastMonitor,
        ServiceMainHQ,
    }
}