namespace ODR
{
    public class Aggregate
    {
        static decimal runningSum;
        static string groupByVal;

        public static string RunningSumForAccounts(object groupBy, object debitAmt, object creditAmt, object acctType)
        {
            if (debitAmt == null || creditAmt == null)
            {
                return 0.ToString("N");
            }
            try
            {
                // Cannot read debitAmt and creditAmt as decimals because it makes the general ledger detail report fail.
                // Simply cast as decimals when doing mathematical operations.
                double debit = (double)debitAmt;
                double credit = (double)creditAmt;

                if (groupByVal == null || groupBy.ToString() != groupByVal) // If new or changed group
                {
                    runningSum = 0;
                }

                groupByVal = groupBy.ToString();
                if (AccountDebitIsPos(acctType.ToString()))
                {
                    runningSum += (decimal)debit - (decimal)credit;
                }
                else
                {
                    runningSum += (decimal)credit - (decimal)debit;
                }
                return runningSum.ToString("N");
            }
            catch
            {
                return 0.ToString("N");
            }
        }

        static bool AccountDebitIsPos(string accountType)
        {
            switch (accountType)
            {
                case "0": // asset
                case "4": // expense
                    return true;

                case "1": // liability
                case "2": // equity // because liabilities and equity are treated the same
                case "3": // revenue
                    return false;
            }
            return true; // will never happen
        }
    }
}