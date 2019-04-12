using System;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class ProcedureCodeT {
		
		public static void Update(ProcedureCode procCode) {
			ProcedureCodes.Update(procCode);
			ProcedureCodes.RefreshCache();
		}

		/// <summary>Returns true if a procedureCode was added.</summary>
		public static bool AddIfNotPresent(string procCode,bool isCanadianLab=false) {
			if(!ProcedureCodes.GetContainsKey(procCode)) {
				ProcedureCodes.Insert(new ProcedureCode {
					ProcCode=procCode,
					IsCanadianLab=isCanadianLab,
				});
				ProcedureCodes.RefreshCache();
				return true;
			}
			return false;
		}

	}
}
