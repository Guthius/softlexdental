using CodeBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using System;
using System.Reflection;
using System.Threading;

namespace UnitTests
{
    [TestClass]
    public class ReportsTests : TestBase
    {
        private const string USER_LOW_NAME = "unittestuserlow";
        private const string USER_LOW_PASS = "Password1";

        [ClassInitialize]
        public static void SetupClass(TestContext testContext)
        {
            //Drop any users that already exist with this specific name.
            DbAdminMysql.DropUser(USER_LOW_NAME);

            //Create a new user with this unit test method name as the database user name.
            DataCore.NonQ($"CREATE USER '{USER_LOW_NAME}'@'localhost' IDENTIFIED BY '{USER_LOW_PASS}'");

            //Only give the SELECT permission to simulate a user of lower status in life.
            DataCore.NonQ($"GRANT SELECT ON *.* TO '{USER_LOW_NAME}'@'localhost'");

            //Reload all privileges to make sure the proletariat permission takes effect.
            DataCore.NonQ("FLUSH PRIVILEGES");
        }

        [ClassCleanup]
        public static void TearDownClass()
        {
            //Drop the user that this unit test class created within SetupClass().
            DbAdminMysql.DropUser(USER_LOW_NAME);
        }

        [TestMethod]
        public void Reports_GetTable_MySqlUserLow()
        {
            Exception ex = null;
            //Spawn a new thread so that we don't manipulate any global DataConnection settings.
            ODThread thread = new ODThread(o =>
            {
                //Prepare some simple queries to verify how both user and user low react.
                string tempTableName = "tmpreportsunittesttable";
                string tempTableDropQuery = $"DROP TABLE IF EXISTS {tempTableName}";
                string tempTableCreateQuery = $"CREATE TABLE {tempTableName} (Col1 VARCHAR(1))";
                string tempTableShowQuery = $"SHOW TABLES LIKE '{tempTableName}'";
                //Make sure that we can create and drop tables as the normal user but cannot do the same thing via Reports.GetTable().
                //First, make sure that the regular user works as intended.
                DataCore.NonQ(tempTableDropQuery);
                DataCore.NonQ(tempTableCreateQuery);
                Assert.AreEqual(1, DataCore.GetTable(tempTableShowQuery).Rows.Count, "Root user was not able to create a new table.");
                DataCore.NonQ(tempTableDropQuery);
                //Next, make sure that user low cannot create the new table.  Required to use the Middle Tier otherwise user low is ignored.

                //User low should be able to run SELECT and SHOW commands.
                if (DataConnection.GetTable(tempTableShowQuery).Rows.Count != 0)
                {//Should have been dropped via root user above.
                    throw new ApplicationException("Temporary table was not dropped correctly.");
                }
                //Reports.GetTable() should throw an exception due to the lack of the CREATE permission.  Swallow it.
                ODException.SwallowAnyException(() => DataConnection.GetTable(tempTableCreateQuery));
                //User low should not have been able to create the table.
                if (DataConnection.GetTable(tempTableShowQuery).Rows.Count != 0)
                {
                    throw new ApplicationException("User low was able to create a table.");
                }
            });
            thread.AddExceptionHandler(e => { ex = e; });//This will cause the unit test to fail.
            thread.Name = "thread" + MethodBase.GetCurrentMethod().Name;
            thread.Start();
            thread.Join(Timeout.Infinite);
            Assert.IsNull(ex, ex?.Message);
        }
    }
}