using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class ProviderClinics
    {
        #region Get Methods
        ///<summary>Gets one ProviderClinic from the db. Can be null.</summary>
        public static ProviderClinic GetOne(long providerClinicNum)
        {
            return Crud.ProviderClinicCrud.SelectOne(providerClinicNum);
        }

        public static List<ProviderClinic> GetByProvNums(List<long> listProvNums)
        {
            if (listProvNums == null || listProvNums.Count == 0)
            {
                return new List<ProviderClinic>();
            }
            string command = "SELECT * FROM providerclinic WHERE ProvNum IN(" + String.Join(", ", listProvNums.Select(x => POut.Long(x))) + ")";
            return Crud.ProviderClinicCrud.SelectMany(command);
        }

        public static ProviderClinic GetOneOrDefault(long provNum, long clinicNum)
        {
            //No need to check RemotingRole; no call to db.;
            //Get ProviderClinic by passed in provnum and clinic
            List<ProviderClinic> listProvClinics = GetListForProvider(provNum);
            return GetFromList(provNum, clinicNum, listProvClinics, true);
        }

        ///<summary>Gets one ProviderClinic from the db. Can be null.</summary>
        public static ProviderClinic GetOne(long provNum, long clinicNum)
        {
            string command = "SELECT * FROM providerclinic WHERE ProvNum = " + POut.Long(provNum) + " AND ClinicNum = " + POut.Long(clinicNum);
            return Crud.ProviderClinicCrud.SelectOne(command);
        }

        ///<summary>Gets one DEANum from the db. If the DEANum for the specified clinic is not set, will return the default DEANum(clinicNum=0). Returns empty string if none set.</summary>
        public static string GetDEANum(long provNum, long clinicNum = 0)
        {
            string command = "SELECT DEANum FROM providerclinic WHERE ProvNum = " + POut.Long(provNum) + " AND ClinicNum = " + POut.Long(clinicNum);
            string retVal = Db.GetScalar(command);
            if (clinicNum != 0 && string.IsNullOrWhiteSpace(retVal))
            {
                retVal = GetDEANum(provNum);
            }
            return retVal;
        }

        ///<summary>Gets a list of ProviderClinics from the db.</summary>
        public static List<ProviderClinic> GetListForProvider(long provNum, List<long> listClinicNums = null)
        {
            string command = "SELECT * FROM providerclinic WHERE ProvNum = " + POut.Long(provNum);
            if (listClinicNums != null && listClinicNums.Count > 0)
            {
                command += " AND ClinicNum IN(" + String.Join(", ", listClinicNums) + ") ";
            }
            return Crud.ProviderClinicCrud.SelectMany(command);
        }

        ///<summary>Gets one ProviderClinic from the list. Optional param to get default for ClinicNum 0 if passed in params result in a no match. Can be null.</summary>
        public static ProviderClinic GetFromList(long provNum, long clinicNum, List<ProviderClinic> listProvClinics, bool canUseDefault = false)
        {
            //No need to check RemotingRole; no call to db.;
            ProviderClinic retVal = listProvClinics.FirstOrDefault(x => x.ProvNum == provNum && x.ClinicNum == clinicNum);
            if (canUseDefault && retVal == null)
            {
                retVal = listProvClinics.FirstOrDefault(x => x.ProvNum == provNum && x.ClinicNum == 0);
            }
            return retVal;
        }
        #endregion

        ///<summary>Inserts, updates, or deletes db rows to match listNew.  No need to pass in userNum, it's set before remoting role check and passed to
        ///the server if necessary.  Doesn't create ApptComm items, but will delete them.  If you use Sync, you must create new AlertCategories items.</summary>
        public static bool Sync(List<ProviderClinic> listNew, List<ProviderClinic> listOld)
        {
            return Crud.ProviderClinicCrud.Sync(listNew, listOld);
        }
    }
}