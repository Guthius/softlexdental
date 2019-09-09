using System;
using System.Collections.Generic;
using System.Text;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class ClockEventT {

		public static long InsertWorkPeriod(long emp,DateTime start,DateTime stop,long clinicNum=0,double adjustHours=0) {
			ClockEvent ce=new ClockEvent();
			ce.Status=ClockEventStatus.Home;
			ce.EmployeeId=emp;
			ce.Date1Displayed=start;
			ce.Date1Entered=start;
			ce.Date2Displayed=stop;
			ce.Date2Entered=stop;
			ce.ClinicId=clinicNum;
			ce.AdjustAuto=TimeSpan.FromHours(-adjustHours);
			ce.Id = ClockEvents.Insert(ce);
			ClockEvents.Update(ce);//Updates TimeDisplayed1 because it defaults to now().
			return ce.Id;
		}

		public static long InsertBreak(long emp,DateTime start,double minutes,long clinicNum=0) {
			ClockEvent ce=new ClockEvent();
			ce.Status=ClockEventStatus.Break;
			ce.EmployeeId=emp;
			ce.Date1Displayed=start;
			ce.Date1Entered=start;
			ce.Date2Displayed=start.AddMinutes(minutes);
			ce.Date2Entered=start.AddMinutes(minutes);
			ce.ClinicId=clinicNum;
			ce.Id = ClockEvents.Insert(ce);
			ClockEvents.Update(ce);//Updates TimeDisplayed1 because it defaults to now().
			return ce.Id;
		}


	}
}
