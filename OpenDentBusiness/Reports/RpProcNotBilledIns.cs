using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace OpenDentBusiness {
	public class RpProcNotBilledIns {
		///<summary>If not using clinics then supply an empty list of clinicNums.  listClinicNums must have at least one item if using clinics.
		///The table returned has the following columns in this order: 
		///PatientName, ProcDate, Descript, ProcFee, ProcNum, ClinicNum, PatNum, IsInProcess</summary>
		public static DataTable GetProcsNotBilled(List<long> listClinicNums,bool includeMedProcs,DateTime dateStart,DateTime dateEnd,
			bool showProcsBeforeIns,bool hasMultiVisitProcs)
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),listClinicNums,includeMedProcs,dateStart,dateEnd,showProcsBeforeIns,hasMultiVisitProcs);
			}
			string query="SELECT ";
			if(PrefC.GetBool(PrefName.ReportsShowPatNum)) {
				query+=DbHelper.Concat("CAST(patient.PatNum AS CHAR)","'-'","patient.LName","', '","patient.FName","' '","patient.MiddleI");
			}
			else {
				query+=DbHelper.Concat("patient.LName","', '","patient.FName","' '","patient.MiddleI");
			}
			query+=" AS 'PatientName',"
				+"CASE WHEN procmultivisit.ProcMultiVisitNum IS NULL "
					+"THEN '"+Lans.g("enumProcStat",ProcStat.C.ToString())+"' ELSE '"+Lans.g("enumProcStat",ProcStatExt.InProcess)+"' END Stat,"
				+"procedurelog.ProcDate,procedurecode.Descript,procedurelog.ProcFee*(procedurelog.UnitQty+procedurelog.BaseUnits),"
				+"procedurelog.ProcNum,procedurelog.ClinicNum,patient.PatNum "
				+"FROM patient "
				+"INNER JOIN procedurelog ON procedurelog.PatNum = patient.PatNum "
					+"AND procedurelog.ProcFee>0 "
					+"AND procedurelog.procstatus="+(int)ProcStat.C+" "
					+"AND procedurelog.ProcDate	BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd)+" "
				+"INNER JOIN ( "
					+"SELECT PatNum FROM patplan GROUP BY PatNum "
				+" )HasIns ON HasIns.PatNum = patient.PatNum ";
			if(listClinicNums.Count>0) {
				query+="AND procedurelog.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			query+="INNER JOIN procedurecode ON procedurecode.CodeNum = procedurelog.CodeNum ";
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {
				query+="AND procedurecode.IsCanadianLab=0 ";//ignore Canadian labs
			}
			query+="LEFT JOIN claimproc ON claimproc.ProcNum = procedurelog.ProcNum "
				+"LEFT JOIN insplan ON insplan.PlanNum = claimproc.PlanNum "
				+"LEFT JOIN procmultivisit ON procmultivisit.ProcNum=procedurelog.ProcNum AND procmultivisit.IsInProcess=1 "
				+"WHERE ((claimproc.NoBillIns=0 AND claimproc.Status="+(int)ClaimProcStatus.Estimate+") ";
			if(showProcsBeforeIns) {
				query+="OR claimproc.ClaimProcNum IS NULL ";
			}
			query+=") ";
			if(!includeMedProcs) {
				query+="AND (insplan.IsMedical=0 ";
				if(showProcsBeforeIns) {
					query+="OR insplan.PlanNum IS NULL ";
				}
				query+=") ";
			}
			if(!hasMultiVisitProcs) {
				query+="AND (procmultivisit.ProcMultiVisitNum IS NULL) ";
			}
			query+="GROUP BY procedurelog.ProcNum "
				+"ORDER BY patient.LName,patient.FName,patient.PatNum,procedurelog.ProcDate";
			return Db.GetTable(query);
		}

	}
}
