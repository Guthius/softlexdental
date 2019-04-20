using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class SupplyOrders
    {
        ///<summary>Gets all SupplyOrders for one supplier, ordered by date.</summary>
        public static List<SupplyOrder> CreateObjects(long supplierNum)
        {
            string command = "SELECT * FROM supplyorder "
                + "WHERE SupplierNum=" + POut.Long(supplierNum)
                + " ORDER BY DatePlaced";
            return Crud.SupplyOrderCrud.SelectMany(command);
        }

        ///<summary>Gets all SupplyOrders, ordered by date.</summary>
        public static List<SupplyOrder> GetAll()
        {
            string command = "SELECT * FROM supplyorder ORDER BY DatePlaced";
            return Crud.SupplyOrderCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static long Insert(SupplyOrder order)
        {
            return Crud.SupplyOrderCrud.Insert(order);
        }

        ///<summary></summary>
        public static void Update(SupplyOrder order)
        {
            Crud.SupplyOrderCrud.Update(order);
        }

        ///<summary>No need to surround with try-catch.</summary>
        public static void DeleteObject(SupplyOrder order)
        {
            //validate that not already in use-no
            //delete associated orderItems
            string command = "DELETE FROM supplyorderitem WHERE SupplyOrderNum=" + POut.Long(order.SupplyOrderNum);
            Db.NonQ(command);
            Crud.SupplyOrderCrud.Delete(order.SupplyOrderNum);
        }

        //Retotals all items attached to order and updates AmountTotal.
        public static void UpdateOrderPrice(long orderNum)
        {
            string command = "SELECT SUM(Qty*Price) FROM supplyorderitem WHERE SupplyOrderNum=" + orderNum;
            double amountTotal = PIn.Double(Db.GetScalar(command));
            command = "SELECT * FROM supplyorder WHERE SupplyOrderNum=" + orderNum;
            SupplyOrder so = Crud.SupplyOrderCrud.SelectOne(command);
            so.AmountTotal = amountTotal;
            Update(so);
        }
    }
}