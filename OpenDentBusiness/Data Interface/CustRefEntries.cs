using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class CustRefEntries
    {
        ///<summary>Gets one CustRefEntry from the db.</summary>
        public static CustRefEntry GetOne(long custRefEntryNum)
        {
            return Crud.CustRefEntryCrud.SelectOne(custRefEntryNum);
        }

        ///<summary></summary>
        public static long Insert(CustRefEntry custRefEntry)
        {
            return Crud.CustRefEntryCrud.Insert(custRefEntry);
        }

        ///<summary></summary>
        public static void Update(CustRefEntry custRefEntry)
        {
            Crud.CustRefEntryCrud.Update(custRefEntry);
        }

        ///<summary></summary>
        public static void Delete(long custRefEntryNum)
        {
            string command = "DELETE FROM custrefentry WHERE CustRefEntryNum = " + POut.Long(custRefEntryNum);
            Db.NonQ(command);
        }

        ///<summary>Gets all the entries for the customer.</summary>
        public static List<CustRefEntry> GetEntryListForCustomer(long patNum)
        {
            string command = "SELECT * FROM custrefentry WHERE PatNumCust=" + POut.Long(patNum) + " OR PatNumRef=" + POut.Long(patNum);
            return Crud.CustRefEntryCrud.SelectMany(command);
        }

        ///<summary>Gets all the entries for the reference.</summary>
        public static List<CustRefEntry> GetEntryListForReference(long patNum)
        {
            string command = "SELECT * FROM custrefentry WHERE PatNumRef=" + POut.Long(patNum);
            return Crud.CustRefEntryCrud.SelectMany(command);
        }
    }
}