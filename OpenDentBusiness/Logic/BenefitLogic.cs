using System;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness
{
    public class BenefitLogic
    {
        /// <summary>
        /// This function is unit tested for accuracy because it has been a repeated source of bugs in the past.
        /// </summary>
        public static DateTime ComputeRenewDate(DateTime asofDate, int monthRenew)
        {
            if (asofDate.Year < 1880)
            {
                return DateTime.Today;
            }

            if (monthRenew == 0) return new DateTime(asofDate.Year, 1, 1);

            if (monthRenew == asofDate.Month) // any day this month
            {
                return new DateTime(asofDate.Year, monthRenew, 1);
            }

            if (monthRenew < asofDate.Month) // previous month
            { 
                return new DateTime(asofDate.Year, monthRenew, 1);
            }

            // late last year
            return new DateTime(asofDate.Year - 1, monthRenew, 1);
        }
    }
}