using System;
using System.Collections.Generic;
using System.Text;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class TimeCardRuleT {

		public static void CreateAMTimeRule(long emp,TimeSpan beforeTime) {
			TimeCardRule tcr=new TimeCardRule();
			tcr.EmployeeId=emp;
			tcr.TimeStart=beforeTime;
			TimeCardRules.Insert(tcr);
			return;
		}

		public static void CreatePMTimeRule(long emp,TimeSpan afterTime) {
			TimeCardRule tcr=new TimeCardRule();
			tcr.EmployeeId=emp;
			tcr.TimeEnd=afterTime;
			TimeCardRules.Insert(tcr);
			return;
		}

		public static void CreateHoursTimeRule(long emp,TimeSpan hoursPerDay) {
			TimeCardRule tcr=new TimeCardRule();
			tcr.EmployeeId=emp;
			tcr.Hours=hoursPerDay;
			TimeCardRules.Insert(tcr);
			return;
		}


	}
}
