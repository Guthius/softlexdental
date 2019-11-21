namespace OpenDentBusiness
{
    public class ProgramProperties
    {
        ///<summary>Exception means failed. Return means success. paymentsAllowed should be check after return. If false then assume payments cannot be made for this clinic.</summary>
        public static void GetXWebCreds(long clinicNum, out WebTypes.Shared.XWeb.WebPaymentProperties xwebProperties)
        {
            //string xWebID;
            //string authKey;
            //string terminalID;
            //long paymentTypeDefNum;
            xwebProperties = new WebTypes.Shared.XWeb.WebPaymentProperties();
            ////No need to check RemotingRole;no call to db.
            ////Secure arguments are held in the db.
            //Program programXcharge = Programs.GetCur(OpenDentBusiness.ProgramName.Xcharge);
            //if (programXcharge == null)
            //{ //XCharge not setup.
            //    throw new ODException("X-Charge program link not found.", ODException.ErrorCodes.XWebProgramProperties);
            //}
            //if (!programXcharge.Enabled)
            //{ //XCharge not turned on.
            //    throw new ODException("X-Charge program link is disabled.", ODException.ErrorCodes.XWebProgramProperties);
            //}
            ////Validate ALL XWebID, AuthKey, and TerminalID.  Each is required for X-Web to work.
            //List<ProgramPreference> listXchargeProperties = ProgramProperties.GetListForProgramAndClinic(programXcharge.Id, clinicNum);
            //xWebID = ProgramProperties.GetPropValFromList(listXchargeProperties, "XWebID", clinicNum);
            //authKey = ProgramProperties.GetPropValFromList(listXchargeProperties, "AuthKey", clinicNum);
            //terminalID = ProgramProperties.GetPropValFromList(listXchargeProperties, "TerminalID", clinicNum);
            //string paymentTypeDefString = ProgramProperties.GetPropValFromList(listXchargeProperties, "PaymentType", clinicNum);
            //if (string.IsNullOrEmpty(xWebID) || string.IsNullOrEmpty(authKey) || string.IsNullOrEmpty(terminalID) || !long.TryParse(paymentTypeDefString, out paymentTypeDefNum))
            //{
            //    throw new ODException("X-Web program properties not found.", ODException.ErrorCodes.XWebProgramProperties);
            //}
            ////XWeb ID must be 12 digits, Auth Key 32 alphanumeric characters, and Terminal ID 8 digits.
            //if (!Regex.IsMatch(xWebID, "^[0-9]{12}$")
            //    || !Regex.IsMatch(authKey, "^[A-Za-z0-9]{32}$")
            //    || !Regex.IsMatch(terminalID, "^[0-9]{8}$"))
            //{
            //    throw new ODException("X-Web program properties not valid.", ODException.ErrorCodes.XWebProgramProperties);
            //}
            //string asString = ProgramProperties.GetPropValFromList(listXchargeProperties, "IsOnlinePaymentsEnabled", clinicNum);
            //xwebProperties.XWebID = xWebID;
            //xwebProperties.TerminalID = terminalID;
            //xwebProperties.AuthKey = authKey;
            //xwebProperties.PaymentTypeDefNum = paymentTypeDefNum;
            //xwebProperties.IsPaymentsAllowed = SIn.Bool(asString);
        }

        public static class PropertyDescs
        {
            public const string ImageFolder = "Image Folder";
            public const string PatOrChartNum = "Enter 0 to use PatientNum, or 1 to use ChartNum";
            public const string Username = "Username";
            public const string Password = "Password";
        }
    }
}
