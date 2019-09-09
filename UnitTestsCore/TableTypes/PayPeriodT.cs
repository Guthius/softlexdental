using System;
using System.Collections.Generic;
using System.Text;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class PayPeriodT {

		/// <summary>Returns existing payperiod if it exists, otherwise inserts and returns a new payperiod. Throws exception if payperiod overlaps existing payperiod.</summary>
		public static PayPeriod CreateTwoWeekPayPeriodIfNotExists(DateTime start){
			PayPeriod ppNew=new PayPeriod();
			ppNew.DateStart=start;
			ppNew.DateEnd=start.AddDays(13);
			ppNew.DatePaycheck=start.AddDays(16);
			//check for identical or overlapping pay periods
			PayPeriods.RefreshCache();
			foreach(PayPeriod ppInDb in PayPeriods.GetDeepCopy()) {
				if(ppInDb.DateStart == ppNew.DateStart && ppInDb.DateEnd == ppNew.DateEnd	&& ppInDb.DatePaycheck == ppNew.DatePaycheck){
					//identical pay period already exists.
					return ppInDb;
				}
				//if(pp.DateStart == payP.DateStart && pp.DateStop == payP.DateStop	&& pp.DatePaycheck != payP.DatePaycheck) {
				//  //identical pay period already exists, just with a different pay check date.
				//  //This is a seperate check because it may be important in the future.
				//  continue;
				//}
				if(ppInDb.DateEnd > ppNew.DateStart && ppInDb.DateStart < ppNew.DateEnd) {
					//pay periods overlap
					throw new Exception("Error inserting pay period. New Pay period overlaps existing pay period.\r\n");
				}
			}
			PayPeriods.Insert(ppNew);
			return ppNew;
		}


	}
}
