using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using CodeBase;
using System.Text.RegularExpressions;

namespace OpenDentBusiness
{

    ///<summary></summary>
    public class ProgramProperties
    {

        ///<summary>This is called from FormClinicEdit and from InsertOrUpdateLocalOverridePath.  PayConnect can have clinic specific login credentials,
        ///so the ProgramProperties for PayConnect are duplicated for each clinic.  The properties duplicated are Username, Password, and PaymentType.
        ///There's also a 'Headquarters' or no clinic set of these props with ClinicNum 0, which is the set of props inserted with each new clinic.</summary>
        public static long Insert(ProgramProperty programProp)
        {
            return default;
        }

        ///<summary>Copies rows for a given programNum for each clinic in listClinicNums.  Returns true if changes were made to the db.</summary>
        public static bool InsertForClinic(long programNum, List<long> listClinicNums)
        {
            if (listClinicNums == null || listClinicNums.Count == 0)
            {
                return false;
            }
            bool hasInsert = false;
            string command = "";

            command = "INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue,ComputerName,ClinicNum) ";
            for (int i = 0; i < listClinicNums.Count; i++)
            {
                if (i > 0)
                {
                    command += " UNION ";
                }
                command += "SELECT ProgramNum,PropertyDesc,PropertyValue,ComputerName," + POut.Long(listClinicNums[i]) + " "
                    + "FROM programproperty "
                    + "WHERE ProgramNum=" + POut.Long(programNum) + " "
                    + "AND ClinicNum=0";
            }
            hasInsert = (Db.NonQ(command) > 0);


            return hasInsert;
        }

        ///<summary>Returns a list of ProgramProperties with the specified programNum and the specified clinicNum from the cache.
        ///To get properties when clinics are not enabled or properties for 'Headquarters' use clinicNum 0.
        ///Does not include path overrides.</summary>
        public static List<ProgramProperty> GetListForProgramAndClinic(long programNum, long clinicNum)
        {
            return default;
            //No need to check RemotingRole; no call to db.
           // return ProgramProperties.GetWhere(x => x.ProgramId == programNum && x.ClinicId == clinicNum && x.Key != "");
        }

        ///<summary>Returns a List of ProgramProperties attached to the specified programNum with the given clinicnum.  
        ///Includes the default program properties as well (ClinicNum==0).</summary>
        public static List<ProgramProperty> GetListForProgramAndClinicWithDefault(long programNum, long clinicNum)
        {
            ////No need to check RemotingRole; no call to db.
            //List<ProgramProperty> listClinicProperties = GetWhere(x => x.ProgramId == programNum && x.ClinicId == clinicNum);
            //if (clinicNum == 0)
            //{
            //    return listClinicProperties;//return the defaults cause ClinicNum of 0 is default.
            //}
            ////Get all the defaults and return a list of defaults mixed with overrides.
            //List<ProgramProperty> listClinicAndDefaultProperties = GetWhere(x => x.ProgramId == programNum && x.ClinicId == 0
            //      && !listClinicProperties.Any(y => y.Key == x.Key));
            //listClinicAndDefaultProperties.AddRange(listClinicProperties);
            //return listClinicAndDefaultProperties;//Clinic users need to have all properties, defaults with the clinic overrides.

            return default;
        }

        ///<summary>Returns the property value of the clinic override or default program property if no clinic override is found.</summary>
        public static string GetPropValForClinicOrDefault(long programNum, string desc, long clinicNum)
        {
            //No need to check RemotingRole; no call to db.
            return GetListForProgramAndClinicWithDefault(programNum, clinicNum).FirstOrDefault(x => x.Key == desc).Value;
        }

        ///<summary>Returns a list of ProgramProperties attached to the specified programNum.  Does not include path overrides.
        ///Uses thread-safe caching pattern.  Each call to this method creates an copy of the entire ProgramProperty cache.</summary>
        public static List<ProgramProperty> GetForProgram(long programNum)
        {
            ////No need to check RemotingRole; no call to db.
            //return GetWhere(x => x.ProgramId == programNum && x.Key != "").OrderBy(x => x.ClinicId).ThenBy(x => x.ProgramPropertyNum).ToList();
            return default;
        }

        ///<summary>Sets the program property for all clinics.</summary>
        public static void SetProperty(long programNum, string desc, string propval)
        {
            string command = "UPDATE programproperty SET PropertyValue='" + POut.String(propval) + "' "
                + "WHERE ProgramNum=" + POut.Long(programNum) + " "
                + "AND PropertyDesc='" + POut.String(desc) + "'";
            Db.NonQ(command);
        }

        ///<summary>After GetForProgram has been run, this gets one of those properties.  DO NOT MODIFY the returned property.  Read only.</summary>
        public static ProgramProperty GetCur(List<ProgramProperty> listForProgram, string desc)
        {
            //No need to check RemotingRole; no call to db.
            return listForProgram.FirstOrDefault(x => x.Key == desc);
        }

        ///<summary>Throws exception if program property is not found.</summary>
        public static string GetPropVal(long programNum, string desc)
        {
            ////No need to check RemotingRole; no call to db.
            //ProgramProperty programProperty = GetFirstOrDefault(x => x.ProgramId == programNum && x.Key == desc);
            //if (programProperty != null)
            //{
            //    return programProperty.Value;
            //}
            //throw new ApplicationException("Property not found: " + desc);
            return default;
        }

        public static string GetPropVal(ProgramName programName, string desc)
        {
            //No need to check RemotingRole; no call to db.
            long programNum = Programs.GetProgramNum(programName);
            return GetPropVal(programNum, desc);
        }

        ///<summary>Returns the PropertyVal for programNum and clinicNum specified with the description specified.  If the property doesn't exist,
        ///returns an empty string.  For the PropertyVal for 'Headquarters' or clincs not enabled, use clinicNum 0.</summary>
        public static string GetPropVal(long programNum, string desc, long clinicNum)
        {
            //return GetPropValFromList(ProgramProperties.GetWhere(x => x.ProgramId == programNum), desc, clinicNum);
            return default;
        }

        ///<summary>Returns the PropertyVal from the list by PropertyDesc and ClinicNum.
        ///For the 'Headquarters' or for clinics not enabled, omit clinicNum or send clinicNum 0.  If not found returns an empty string.
        ///Primarily used when a local list has been copied from the cache and may differ from what's in the database.  Also possibly useful if dealing with a filtered list </summary>
        public static string GetPropValFromList(List<ProgramProperty> listProps, string propertyDesc, long clinicNum = 0)
        {
            string retval = "";
            ProgramProperty prop = listProps.Where(x => x.ClinicId == clinicNum).Where(x => x.Key == propertyDesc).FirstOrDefault();
            if (prop != null)
            {
                retval = prop.Value;
            }
            return retval;
        }

        ///<summary>Used in FormUAppoint to get frequent and current data.</summary>
        public static string GetValFromDb(long programNum, string desc)
        {
            string command = "SELECT PropertyValue FROM programproperty WHERE ProgramNum=" + POut.Long(programNum)
                + " AND PropertyDesc='" + POut.String(desc) + "'";
            DataTable table = Db.GetTable(command);
            if (table.Rows.Count == 0)
            {
                return "";
            }
            return table.Rows[0][0].ToString();
        }

        ///<summary>Exception means failed. Return means success. paymentsAllowed should be check after return. If false then assume payments cannot be made for this clinic.</summary>
        public static void GetXWebCreds(long clinicNum, out OpenDentBusiness.WebTypes.Shared.XWeb.WebPaymentProperties xwebProperties)
        {
            string xWebID;
            string authKey;
            string terminalID;
            long paymentTypeDefNum;
            xwebProperties = new WebTypes.Shared.XWeb.WebPaymentProperties();
            //No need to check RemotingRole;no call to db.
            //Secure arguments are held in the db.
            OpenDentBusiness.Program programXcharge = OpenDentBusiness.Programs.GetCur(OpenDentBusiness.ProgramName.Xcharge);
            if (programXcharge == null)
            { //XCharge not setup.
                throw new ODException("X-Charge program link not found.", ODException.ErrorCodes.XWebProgramProperties);
            }
            if (!programXcharge.Enabled)
            { //XCharge not turned on.
                throw new ODException("X-Charge program link is disabled.", ODException.ErrorCodes.XWebProgramProperties);
            }
            //Validate ALL XWebID, AuthKey, and TerminalID.  Each is required for X-Web to work.
            List<OpenDentBusiness.ProgramProperty> listXchargeProperties = OpenDentBusiness.ProgramProperties.GetListForProgramAndClinic(programXcharge.ProgramNum, clinicNum);
            xWebID = OpenDentBusiness.ProgramProperties.GetPropValFromList(listXchargeProperties, "XWebID", clinicNum);
            authKey = OpenDentBusiness.ProgramProperties.GetPropValFromList(listXchargeProperties, "AuthKey", clinicNum);
            terminalID = OpenDentBusiness.ProgramProperties.GetPropValFromList(listXchargeProperties, "TerminalID", clinicNum);
            string paymentTypeDefString = OpenDentBusiness.ProgramProperties.GetPropValFromList(listXchargeProperties, "PaymentType", clinicNum);
            if (string.IsNullOrEmpty(xWebID) || string.IsNullOrEmpty(authKey) || string.IsNullOrEmpty(terminalID) || !long.TryParse(paymentTypeDefString, out paymentTypeDefNum))
            {
                throw new ODException("X-Web program properties not found.", ODException.ErrorCodes.XWebProgramProperties);
            }
            //XWeb ID must be 12 digits, Auth Key 32 alphanumeric characters, and Terminal ID 8 digits.
            if (!Regex.IsMatch(xWebID, "^[0-9]{12}$")
                || !Regex.IsMatch(authKey, "^[A-Za-z0-9]{32}$")
                || !Regex.IsMatch(terminalID, "^[0-9]{8}$"))
            {
                throw new ODException("X-Web program properties not valid.", ODException.ErrorCodes.XWebProgramProperties);
            }
            string asString = OpenDentBusiness.ProgramProperties.GetPropValFromList(listXchargeProperties, "IsOnlinePaymentsEnabled", clinicNum);
            xwebProperties.XWebID = xWebID;
            xwebProperties.TerminalID = terminalID;
            xwebProperties.AuthKey = authKey;
            xwebProperties.PaymentTypeDefNum = paymentTypeDefNum;
            xwebProperties.IsPaymentsAllowed = OpenDentBusiness.PIn.Bool(asString);
        }

        public class PropertyDescs
        {
            public const string ImageFolder = "Image Folder";
            public const string PatOrChartNum = "Enter 0 to use PatientNum, or 1 to use ChartNum";
            public const string Username = "Username";
            public const string Password = "Password";
            //Prevents this class from being instansiated.
            private PropertyDescs() { }
        }
    }
}