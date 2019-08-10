using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests
{
    [TestClass]
    public class DataConnectionTests : TestBase
    {
        [TestMethod]
        public void DataConnectionTests_NonQ_ParametersTest()
        {
            Patient pat = PatientT.CreatePatient(fName: "Data", lName: "Connection");
            //The parameter value is set to syntax characters which would fail if we scrubbed them to avoid injection attacks.
            //Parameters should always safely maintain any text in the input, including syntax characters.
            OdSqlParameter p1 = new OdSqlParameter("p1", OdDbType.Text, "'@`");
            string command = "UPDATE patient SET LName=" + DbHelper.ParamChar + "p1 WHERE PatNum=" + POut.Long(pat.PatNum);
            DataCore.NonQ(command, false, "", "", p1);//Creates a new connection to unittest### database.
            Patient patModified = Patients.GetPat(pat.PatNum);
            Assert.AreEqual(patModified.LName, p1.Value);
        }

        [TestMethod]
        public void DataConnectionTests_SOut_HasInjectionChars()
        {
            Assert.IsTrue(SOut.HasInjectionChars("'"));
            Assert.IsTrue(SOut.HasInjectionChars("\""));
            Assert.IsTrue(SOut.HasInjectionChars("\r"));
            Assert.IsTrue(SOut.HasInjectionChars("\n"));
            Assert.IsTrue(SOut.HasInjectionChars("\t"));
            Assert.IsTrue(SOut.HasInjectionChars("'\"\r\n\t"));
            Assert.IsTrue(SOut.HasInjectionChars("abc\t"));
            Assert.IsFalse(SOut.HasInjectionChars(""));
            Assert.IsFalse(SOut.HasInjectionChars("a"));
            Assert.IsFalse(SOut.HasInjectionChars("12345"));
            Assert.IsFalse(SOut.HasInjectionChars("abcde"));
            Assert.IsFalse(SOut.HasInjectionChars("^@bcd3$"));
        }
    }
}