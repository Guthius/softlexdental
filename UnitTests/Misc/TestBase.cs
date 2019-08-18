using System;
using CodeBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using System.Collections.Generic;
using UnitTestsCore;
using System.Reflection;

namespace UnitTests
{
    [TestClass]
    public abstract class TestBase
    {

        protected const string UnitTestUserName = "UnitTest";
        protected const string UnitTestPassword = "password";
        public const string TestEmaiAddress = "opendentalunittests@gmail.com";
        public const string TestEmaiPwd = "welovetesting!";
        ///<summary>This is the OD test department's Google Voice number. See [[eService Regression Test Database Setup]]. 
        ///Linked to email eServices.od.test@gmail.com.</summary>
        public const string TestPatPhone = "1(971)301-2247";
        public TestContext TestContext { get; set; }
        ///<summary>Put this in the watch window to see the logger results.</summary>
        protected string _logStr = "";

        public static string UnitTestDbName
        {
            get
            {
                Version versionBusiness = Assembly.GetAssembly(typeof(PatientStatus)).GetName().Version;
                return "unittest" + versionBusiness.Major + versionBusiness.Minor;
            }
        }

        /// <summary>
        /// TestBase.Initialize will be called before the ClassInitialize and TestInitialize methods specific to each class.
        /// Do this first so that the time the Initialize and ClassInitialize methods take doesn't get counted in the test times.
        /// </summary>
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            ODInitialize.IsRunningInUnitTest = true;//Causes FormFriendlyException to throw rather than displaying a MessageBox.
            ODInitialize.Initialize();
            if (!UnitTestsCore.DatabaseTools.SetDbConnection(UnitTestDbName, "localhost", "3306", "root", "", false))
            {//Put this in a config file in the future.
                UnitTestsCore.DatabaseTools.SetDbConnection("", "localhost", "3306", "root", "", false);
                DatabaseTools.FreshFromDump("localhost", "3306", "root", "", false);//this also sets database to be unittest.
            }
            else
            {
                //Clear the database before running the unittests (instead of after) for two reasons
                //1- if the cleanup is done using [TestCleanup], the cleanup will not be run if the user cancels in the middle of a test while debugging
                //2- if a test fails, we may want to look at the data in the db to see why it failed.
                UnitTestsCore.DatabaseTools.ClearDb();
            }
#if !DEBUG
			throw new Exception("You're running tests in release. BAD!!!");
#endif
            CreateUnitTestUser();
            //Get the Admin user, should always exist
            Security.CurUser = Userods.GetUserByName(UnitTestUserName, false);
            Security.PasswordTyped = UnitTestPassword;//For middle tier unit tests.
        }

        [TestInitialize]
        public void ResetTest()
        {
            PrefT.RevertPrefChanges();
        }

        public static EmailAddress InsertEmailAddress()
        {
            EmailAddress email = new EmailAddress()
            {
                SenderAddress = TestEmaiAddress,
                EmailUsername = TestEmaiAddress,
                EmailPassword = MiscUtils.Encrypt(TestEmaiPwd),
                ServerPort = 587,
                UseSSL = true,
                Pop3ServerIncoming = "pop.gmail.com",
                ServerPortIncoming = 110,
                SMTPserver = "smtp.gmail.com",
                UserNum = 0,
                WebmailProvNum = 0,
            };
            EmailAddresses.Insert(email);
            return email;
        }

        public static void CreateUnitTestUser()
        {
            if (Userods.GetUserByName(UnitTestUserName, false) == null)
            {
                UserodT.CreateUser(UnitTestUserName, UnitTestPassword, new List<long> { 1 });
            }
        }
    }
}