using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class XWebResponses
    {
        #region Get Methods
        #endregion

        #region Modification Methods

        #region Insert
        #endregion

        #region Update
        #endregion

        #region Delete
        #endregion

        #endregion

        #region Misc Methods
        #endregion

        ///<summary>Gets one XWebResponse from the db.</summary>
        public static XWebResponse GetOne(long xWebResponseNum)
        {
            return Crud.XWebResponseCrud.SelectOne(xWebResponseNum);
        }

        ///<summary>Gets the XWeb transactions for approved transactions. To get for all clinics, pass in a list of empty clinicNums.</summary>
        public static DataTable GetApprovedTransactions(List<long> listClinicNums, DateTime dateFrom, DateTime dateTo)
        {
            string command = "SELECT " + DbHelper.Concat("patient.LName", "', '", "patient.FName") + " Patient,xwebresponse.DateTUpdate,xwebresponse.TransactionID,"
                + "xwebresponse.MaskedAcctNum,xwebresponse.ExpDate,xwebresponse.Amount,xwebresponse.PaymentNum,xwebresponse.TransactionStatus,"
                + "(CASE WHEN payment.PayNum IS NULL THEN 0 ELSE 1 END) doesPaymentExist,COALESCE(clinic.Abbr,'Unassigned') Clinic,xwebresponse.PatNum, "
                + "xwebresponse.XWebResponseNum,xwebresponse.Alias "
                + "FROM xwebresponse "
                + "INNER JOIN patient ON patient.PatNum=xwebresponse.PatNum "
                + "LEFT JOIN payment ON payment.PayNum=xwebresponse.PaymentNum "
                + "LEFT JOIN clinic ON clinic.ClinicNum=xwebresponse.ClinicNum "
                + "WHERE xwebresponse.TransactionStatus IN("
                + POut.Int((int)XWebTransactionStatus.DtgPaymentApproved) + ","
                + POut.Int((int)XWebTransactionStatus.HpfCompletePaymentApproved) + ","
                + POut.Int((int)XWebTransactionStatus.HpfCompletePaymentApprovedPartial) + ","
                + POut.Int((int)XWebTransactionStatus.DtgPaymentReturned) + ","
                + POut.Int((int)XWebTransactionStatus.DtgPaymentVoided) + ") "
                + "AND xwebresponse.ResponseCode IN("
                + POut.Int((int)XWebResponseCodes.Approval) + ","
                + POut.Int((int)XWebResponseCodes.PartialApproval) + ") "
                + "AND xwebresponse.DateTUpdate BETWEEN " + POut.DateT(dateFrom) + " AND " + POut.DateT(dateTo.AddDays(1)) + " ";
            if (listClinicNums.Count > 0)
            {
                command += "AND xwebresponse.ClinicNum IN (" + string.Join(",", listClinicNums.Select(x => POut.Long(x))) + ") ";
            }
            command += "ORDER BY xwebresponse.DateTUpdate,patient.LName,patient.FName ";
            return Db.GetTable(command);
        }

        ///<summary>Gets the XWebResponse that is associated with this payNum. Returns null if the XWebResponse does not exist.</summary>
        public static XWebResponse GetOneByPaymentNum(long payNum)
        {
            string command = "SELECT * FROM xwebresponse WHERE PaymentNum=" + POut.Long(payNum);
            return Crud.XWebResponseCrud.SelectOne(command);
        }

        ///<summary>Gets all XWebResponses where TransactionStatus==XWebTransactionStatus.HpfPending from the db.</summary>
        public static List<XWebResponse> GetPendingHPFs()
        {
            return Crud.XWebResponseCrud.SelectMany("SELECT * FROM xwebresponse "
                + "WHERE TransactionStatus = " + POut.Int((int)XWebTransactionStatus.HpfPending) + " "
                + "AND (TransactionType = '" + POut.String(XWebTransactionType.AliasCreateTransaction.ToString()) + "' OR TransactionType = '" + POut.String(XWebTransactionType.CreditSaleTransaction.ToString()) + "')");
        }

        ///<summary></summary>
        public static long Insert(XWebResponse xWebResponse)
        {
            return Crud.XWebResponseCrud.Insert(xWebResponse);
        }

        ///<summary></summary>
        public static void Update(XWebResponse xWebResponse)
        {
            Crud.XWebResponseCrud.Update(xWebResponse);
        }
    }
}