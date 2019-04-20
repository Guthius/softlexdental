using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class DispSupplies
    {
        ///<summary></summary>
        public static DataTable RefreshDispensary(long provNum)
        {
            string command = "SELECT supply.Descript,dispsupply.DateDispensed,dispsupply.DispQuantity,dispsupply.Note "
                + "FROM dispsupply LEFT JOIN supply ON dispsupply.SupplyNum=supply.SupplyNum "
                    + "WHERE dispsupply.ProvNum=" + POut.Long(provNum) + " "
                    + "ORDER BY DateDispensed,Descript";
            return Db.GetTable(command);
        }

        ///<summary></summary>
        public static long Insert(DispSupply dispSupply)
        {
            return Crud.DispSupplyCrud.Insert(dispSupply);
        }
    }
}