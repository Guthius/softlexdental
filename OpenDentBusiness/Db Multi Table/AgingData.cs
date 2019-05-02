using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using CodeBase;

namespace OpenDentBusiness
{
    ///<summary>Helper class that holds all of the data necessary for generating a billing list.
    ///This class centralizes some logic which allows things to execute faster and with far less network trips when utilizing the Middle Tier.
    ///Only used in billing options window for generating statements.</summary>
    public class AgingData
    {

        ///<summary>Returns a SerializableDictionary with key=PatNum, value=PatAgingData with the filters applied.</summary>
        public static Dictionary<long, PatAgingData> GetAgingData(bool isSinglePatient, bool includeChanged, bool excludeInsPending,
            bool excludeIfUnsentProcs, bool isSuperBills, List<long> listClinicNums)
        {
            Dictionary<long, PatAgingData> dictPatAgingData = new Dictionary<long, PatAgingData>();
            string command = "";
            string guarOrPat = "guar";
            if (isSinglePatient)
            {
                guarOrPat = "patient";
            }
            string whereAndClinNum = "";
            if (!listClinicNums.IsNullOrEmpty())
            {
                whereAndClinNum = $@"AND {guarOrPat}.ClinicNum IN ({string.Join(",", listClinicNums)})";
            }
            if (includeChanged || excludeIfUnsentProcs)
            {
                command = $@"SELECT {guarOrPat}.PatNum,{guarOrPat}.ClinicNum,MAX(procedurelog.ProcDate) MaxProcDate";
                if (excludeIfUnsentProcs)
                {
                    command += ",MAX(CASE WHEN insplan.IsMedical=1 THEN 0 ELSE COALESCE(claimproc.ProcNum,0) END)>0 HasUnsentProcs";
                }
                command += $@" FROM patient
					INNER JOIN patient guar ON guar.PatNum=patient.Guarantor
					INNER JOIN procedurelog ON procedurelog.PatNum = patient.PatNum ";
                if (excludeIfUnsentProcs)
                {
                    command += $@"LEFT JOIN claimproc ON claimproc.ProcNum = procedurelog.ProcNum
						AND claimproc.NoBillIns=0
						AND claimproc.Status = {POut.Int((int)ClaimProcStatus.Estimate)}
						AND procedurelog.ProcDate > CURDATE()-INTERVAL 6 MONTH
					LEFT JOIN insplan ON insplan.PlanNum=claimproc.PlanNum ";
                }
                command += $@"WHERE procedurelog.ProcFee > 0
					AND procedurelog.ProcStatus = {POut.Int((int)ProcStat.C)}
					{whereAndClinNum}
					GROUP BY {guarOrPat}.PatNum
					ORDER BY NULL";
                using (DataTable tableChangedAndUnsent = Db.GetTable(command))
                {
                    foreach (DataRow row in tableChangedAndUnsent.Rows)
                    {
                        long patNum = PIn.Long(row["PatNum"].ToString());
                        if (!dictPatAgingData.ContainsKey(patNum))
                        {
                            dictPatAgingData[patNum] = new PatAgingData(PIn.Long(row["ClinicNum"].ToString()));
                        }
                        if (includeChanged)
                        {
                            dictPatAgingData[patNum].DateLastTrans = new[] { PIn.Date(row["MaxProcDate"].ToString()), dictPatAgingData[patNum].DateLastTrans }.Max();
                        }
                        if (excludeIfUnsentProcs)
                        {
                            dictPatAgingData[patNum].HasUnsentProcs = PIn.Bool(row["HasUnsentProcs"].ToString());
                        }
                    }
                }
            }
            if (includeChanged)
            {
                command = $@"SELECT {guarOrPat}.PatNum,{guarOrPat}.ClinicNum,MAX(claimproc.DateCP) maxDateCP
					FROM claimproc
					INNER JOIN patient ON patient.PatNum = claimproc.PatNum
					INNER JOIN patient guar ON guar.PatNum=patient.Guarantor
					WHERE claimproc.InsPayAmt > 0
					{whereAndClinNum}
					GROUP BY {guarOrPat}.PatNum";
                using (DataTable tableMaxPayDate = Db.GetTable(command))
                {
                    foreach (DataRow row in tableMaxPayDate.Rows)
                    {
                        long patNum = PIn.Long(row["PatNum"].ToString());
                        if (!dictPatAgingData.ContainsKey(patNum))
                        {
                            dictPatAgingData[patNum] = new PatAgingData(PIn.Long(row["ClinicNum"].ToString()));
                        }
                        dictPatAgingData[patNum].DateLastTrans = new[] { PIn.Date(row["maxDateCP"].ToString()), dictPatAgingData[patNum].DateLastTrans }.Max();
                    }
                }
                command = $@"SELECT {guarOrPat}.PatNum,{guarOrPat}.ClinicNum,MAX(payplancharge.ChargeDate) maxDatePPC
					FROM payplancharge
					INNER JOIN patient ON patient.PatNum = payplancharge.PatNum
					INNER JOIN patient guar ON guar.PatNum=patient.Guarantor
					INNER JOIN payplan ON payplan.PayPlanNum = payplancharge.PayPlanNum
						AND payplan.PlanNum = 0 "//don't want insurance payment plans to make patients appear in the billing list
                    + $@"WHERE payplancharge.Principal + payplancharge.Interest>0
					AND payplancharge.ChargeType = {(int)PayPlanChargeType.Debit} "
                    //include all charges in the past or due 'PayPlanBillInAdvance' days into the future.
                    + $@"AND payplancharge.ChargeDate <= {POut.Date(DateTime.Today.AddDays(Preferences.GetDouble(PrefName.PayPlansBillInAdvanceDays)))}
					{whereAndClinNum}
					GROUP BY {guarOrPat}.PatNum";
                using (DataTable tableMaxPPCDate = Db.GetTable(command))
                {
                    foreach (DataRow row in tableMaxPPCDate.Rows)
                    {
                        long patNum = PIn.Long(row["PatNum"].ToString());
                        if (!dictPatAgingData.ContainsKey(patNum))
                        {
                            dictPatAgingData[patNum] = new PatAgingData(PIn.Long(row["ClinicNum"].ToString()));
                        }
                        dictPatAgingData[patNum].DateLastTrans = new[] { PIn.Date(row["maxDatePPC"].ToString()), dictPatAgingData[patNum].DateLastTrans }.Max();
                    }
                }
            }
            if (excludeInsPending)
            {
                command = $@"SELECT {guarOrPat}.PatNum,{guarOrPat}.ClinicNum
					FROM claim
					INNER JOIN patient ON patient.PatNum=claim.PatNum
					INNER JOIN patient guar ON guar.PatNum=patient.Guarantor
					WHERE claim.ClaimStatus IN ('U','H','W','S')
					AND claim.ClaimType IN ('P','S','Other')
					{whereAndClinNum}
					GROUP BY {guarOrPat}.PatNum";
                using (DataTable tableInsPending = Db.GetTable(command))
                {
                    foreach (DataRow row in tableInsPending.Rows)
                    {
                        long patNum = PIn.Long(row["PatNum"].ToString());
                        if (!dictPatAgingData.ContainsKey(patNum))
                        {
                            dictPatAgingData[patNum] = new PatAgingData(PIn.Long(row["ClinicNum"].ToString()));
                        }
                        dictPatAgingData[patNum].HasPendingIns = true;
                    }
                }
            }
            DateTime dateAsOf = DateTime.Today;//used to determine when the balance on this date began
            if (Preferences.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily))
            {//if aging calculated monthly, use the last aging date instead of today
                dateAsOf = Preferences.GetDate(PrefName.DateLastAging);
            }
            List<PatComm> listPatComms = new List<PatComm>();
            using (DataTable tableDateBalsBegan = Ledgers.GetDateBalanceBegan(null, dateAsOf, isSuperBills, listClinicNums))
            {
                foreach (DataRow row in tableDateBalsBegan.Rows)
                {
                    long patNum = PIn.Long(row["PatNum"].ToString());
                    if (!dictPatAgingData.ContainsKey(patNum))
                    {
                        dictPatAgingData[patNum] = new PatAgingData(PIn.Long(row["ClinicNum"].ToString()));
                    }
                    dictPatAgingData[patNum].DateBalBegan = PIn.Date(row["DateAccountAge"].ToString());
                    dictPatAgingData[patNum].DateBalZero = PIn.Date(row["DateZeroBal"].ToString());
                }
                listPatComms = Patients.GetPatComms(tableDateBalsBegan.Select().Select(x => PIn.Long(x["PatNum"].ToString())).ToList(), null);
            }
            foreach (PatComm pComm in listPatComms)
            {
                if (!dictPatAgingData.ContainsKey(pComm.PatNum))
                {
                    dictPatAgingData[pComm.PatNum] = new PatAgingData(pComm.ClinicNum);
                }
                dictPatAgingData[pComm.PatNum].PatComm = pComm;
            }
            return dictPatAgingData;
        }
    }

    ///<summary>Not a db table.  Used for generating statements</summary>
    [Serializable]
    public class PatAgingData
    {
        public DateTime DateBalBegan;
        public DateTime DateBalZero;
        ///<summary>The most recent of all proc dates, ins payment dates, or pay plan charge dates for this pat.</summary>
        public DateTime DateLastTrans;
        public PatComm PatComm;
        public bool HasUnsentProcs;
        public bool HasPendingIns;
        public long ClinicNum;

        ///<summary>Only for serialization purposes.  Private so it won't be used.  Use the constructor that takes a ClinicNum instead.  This means we can
        ///always expect the PatAgingData to have a ClinicNum.</summary>
        private PatAgingData() { }

        public PatAgingData(long clinicNum)
        {
            ClinicNum = clinicNum;
            DateBalBegan = DateTime.MinValue;
            DateBalZero = DateTime.MinValue;
            DateLastTrans = DateTime.MinValue;
        }

        ///<summary></summary>
        public PatAgingData Copy()
        {
            return (PatAgingData)this.MemberwiseClone();
        }
    }
}