using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class PayConnectResponseWebs
    {
        ///<summary>Gets one PayConnectResponseWeb from the db.</summary>
        public static PayConnectResponseWeb GetOne(long payConnectResponseWebNum)
        {
            return Crud.PayConnectResponseWebCrud.SelectOne(payConnectResponseWebNum);
        }

        ///<summary></summary>
        public static long Insert(PayConnectResponseWeb payConnectResponseWeb)
        {
            return Crud.PayConnectResponseWebCrud.Insert(payConnectResponseWeb);
        }

        ///<summary></summary>
        public static void Update(PayConnectResponseWeb payConnectResponseWeb)
        {
            Crud.PayConnectResponseWebCrud.Update(payConnectResponseWeb);
        }
    }
}