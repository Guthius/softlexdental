using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
    ///<summary></summary>
    public class SupplyNeededs
    {
        ///<summary>Gets all SupplyNeededs.</summary>
        public static List<SupplyNeeded> CreateObjects()
        {
            string command = "SELECT * FROM supplyneeded ORDER BY DateAdded";
            return Crud.SupplyNeededCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static long Insert(SupplyNeeded supp)
        {
            return Crud.SupplyNeededCrud.Insert(supp);
        }

        ///<summary></summary>
        public static void Update(SupplyNeeded supp)
        {
            Crud.SupplyNeededCrud.Update(supp);
        }

        ///<summary></summary>
        public static void DeleteObject(SupplyNeeded supp)
        {
            Crud.SupplyNeededCrud.Delete(supp.SupplyNeededNum);
        }
    }
}