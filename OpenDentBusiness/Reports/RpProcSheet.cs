﻿using System;
using System.Collections.Generic;
using System.Data;

namespace OpenDentBusiness
{
    public class RpProcSheet
    {
        ///<summary>If not using clinics then supply an empty list of clinicNums.  listClinicNums must have at least one item if using clinics. Not formatted for display</summary>
        public static DataTable GetIndividualTable(DateTime dateFrom, DateTime dateTo, List<long> listProvNums, List<long> listClinicNums, string procCode,
            bool isAnyClinicMedical, bool hasAllProvs, bool hasClinicsEnabled)
        {
            string query = "SELECT procedurelog.ProcDate,"
              + DbHelper.Concat("patient.LName", "', '", "patient.FName", "' '", "patient.MiddleI") + " "
              + "AS plfname, procedurecode.ProcCode,";
            if (!isAnyClinicMedical)
            {
                query += "procedurelog.ToothNum,";
            }
            query += "procedurecode.Descript,provider.Abbr,";
            if (hasClinicsEnabled)
            {
                query += "COALESCE(clinic.Abbr,\"Unassigned\") Clinic,";
            }
            query += "procedurelog.ProcFee*(procedurelog.UnitQty+procedurelog.BaseUnits)"
                + "-COALESCE(SUM(claimproc.WriteOff),0) ";//\"$fee\" "  //if no writeoff, then subtract 0
            query += "$fee ";

            query += "FROM patient "
                + "INNER JOIN procedurelog ON procedurelog.PatNum=patient.PatNum "
                + "INNER JOIN procedurecode ON procedurecode.CodeNum=procedurelog.CodeNum "
                + "INNER JOIN provider ON provider.ProvNum=procedurelog.ProvNum ";
            if (hasClinicsEnabled)
            {
                query += "LEFT JOIN clinic ON clinic.ClinicNum=procedurelog.ClinicNum ";
            }
            query += "LEFT JOIN claimproc ON procedurelog.ProcNum=claimproc.ProcNum "
                + "AND claimproc.Status=" + POut.Int((int)ClaimProcStatus.CapComplete) + " "//only CapComplete writeoffs are subtracted here.
                + "WHERE procedurelog.ProcStatus=" + POut.Int((int)ProcStat.C) + " ";
            if (!hasAllProvs)
            {
                query += "AND procedurelog.ProvNum IN (" + String.Join(",", listProvNums) + ") ";
            }
            if (hasClinicsEnabled)
            {
                query += "AND procedurelog.ClinicNum IN (" + String.Join(",", listClinicNums) + ") ";
            }
            query += "AND procedurecode.ProcCode LIKE '%" + POut.String(procCode) + "%' "
                + "AND procedurelog.ProcDate >= " + POut.Date(dateFrom) + " "
                + "AND procedurelog.ProcDate <= " + POut.Date(dateTo) + " "
                + "GROUP BY procedurelog.ProcNum "
                + "ORDER BY procedurelog.ProcDate,plfname,procedurecode.ProcCode,ToothNum";
            return DataConnection.ExecuteDataTable(query);
        }

        public static DataTable GetGroupedTable(DateTime dateFrom, DateTime dateTo, List<long> listProvNums, List<long> listClinicNums, string procCode, bool hasAllProvs)
        {
            string query = "SELECT procs.ItemName,procs.ProcCode,procs.Descript,Count(*),FORMAT(ROUND(AVG(procs.fee),2),2) $AvgFee,SUM(procs.fee) AS $TotFee ";

            query += "FROM ( "
                + "SELECT procedurelog.ProcFee*(procedurelog.UnitQty+procedurelog.BaseUnits) -COALESCE(SUM(claimproc.WriteOff),0) fee, "
                + "procedurecode.ProcCode,	procedurecode.Descript,	definition.ItemName, definition.ItemOrder "
                + "FROM procedurelog "
                + "INNER JOIN procedurecode ON procedurelog.CodeNum=procedurecode.CodeNum "
                + "INNER JOIN definition ON definition.DefNum=procedurecode.ProcCat "
                + "LEFT JOIN claimproc ON claimproc.ProcNum=procedurelog.ProcNum AND claimproc.Status=" + POut.Int((int)ClaimProcStatus.CapComplete) + " "
                + "WHERE procedurelog.ProcStatus=" + POut.Int((int)ProcStat.C) + " ";
            if (!hasAllProvs)
            {
                query += "AND procedurelog.ProvNum IN (" + String.Join(",", listProvNums) + ") ";
            }

            query += "AND procedurelog.ClinicNum IN (" + String.Join(",", listClinicNums) + ") ";

            query += "AND procedurecode.ProcCode LIKE '%" + POut.String(procCode) + "%' "
            + "AND procedurelog.ProcDate >= " + POut.Date(dateFrom) + " "
            + "AND procedurelog.ProcDate <= " + POut.Date(dateTo) + " "
            + "GROUP BY procedurelog.ProcNum ) procs "
            + "GROUP BY procs.ProcCode "
            + "ORDER BY procs.ItemOrder,procs.ProcCode";
            return DataConnection.ExecuteDataTable(query);
        }
    }
}