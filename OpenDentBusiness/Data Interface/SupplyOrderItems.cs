using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness
{

    ///<summary></summary>
    public class SupplyOrderItems
    {
        ///<summary>Items in the table are not ItemOrderObjects</summary>
        public static DataTable GetItemsForOrder(long orderNum)
        {
            string command = "SELECT CatalogNumber,Descript,Qty,supplyorderitem.Price,SupplyOrderItemNum,supplyorderitem.SupplyNum "
                + "FROM supplyorderitem,definition,supply "
                + "WHERE definition.DefNum=supply.Category "
                + "AND supply.SupplyNum=supplyorderitem.SupplyNum "
                + "AND supplyorderitem.SupplyOrderNum=" + POut.Long(orderNum) + " "
                + "ORDER BY definition.ItemOrder,supply.ItemOrder";
            return Db.GetTable(command);
        }

        public static SupplyOrderItem CreateObject(long supplyOrderItemNum)
        {
            string command = "SELECT * FROM supplyorderitem WHERE SupplyOrderItemNum=" + POut.Long(supplyOrderItemNum);
            return Crud.SupplyOrderItemCrud.SelectOne(command);
        }

        ///<summary></summary>
        public static long Insert(SupplyOrderItem supp)
        {
            return Crud.SupplyOrderItemCrud.Insert(supp);
        }

        ///<summary></summary>
        public static void Update(SupplyOrderItem supp)
        {
            Crud.SupplyOrderItemCrud.Update(supp);
        }

        ///<summary>Surround with try-catch.</summary>
        public static void DeleteObject(SupplyOrderItem supp)
        {

            //validate that not already in use.
            Crud.SupplyOrderItemCrud.Delete(supp.SupplyOrderItemNum);
        }
    }
}