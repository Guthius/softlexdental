using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using System;

namespace UnitTests
{
    [TestClass]
    public class ODInstallerTests : TestBase
    {
        [ClassInitialize]
        public static void SetupClass(TestContext testContext)
        {
            //Add anything here that you want to run once before the tests in this class run.
        }

        [TestMethod]
        public void ODInstallerTests_DbAdminMysql_CreateAndDestroyAdminUsers()
        {
            DataConnection conAdmin = new DataConnection();//A brand new admin connection to the unittest### database.
            Assert.AreEqual(null, DbAdminMysql.ModifyUser("fakeuser1", "od123", "fakeuser1"));//Grants, verifies new user.
            Assert.AreEqual(null, DbAdminMysql.ModifyUser("fakeuser2", "abcde", "fakeuser1"));//Drops fakeuser1 and verifies.
            try
            {
                DbAdminMysql.DropUser("fakeuser2");//Drops fakeuser2 and verifies.
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}