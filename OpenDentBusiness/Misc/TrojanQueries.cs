using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace OpenDentBusiness
{
    public class TrojanQueries
    {
        public static DataTable GetMaxProcedureDate(long patientId)
        {
            return DataConnection.GetTable(
                "SELECT MAX(`ProcDate`) FROM `procedurelog`, `patient` " +
                "WHERE `patient`.`PatNum` = `procedurelog`.`PatNum` " +
                "AND `patient`.`Guarantor` = " + patientId);
        }

        public static DataTable GetMaxPaymentDate(long patientId)
        {
            return DataConnection.GetTable(
                "SELECT MAX(`DatePay`) FROM `paysplit`, `patient` " +
                "WHERE `patient`.PatNum = `paysplit`.`PatNum` " +
                "AND `patient`.`Guarantor` = " + patientId);
        }

        public static int GetUniqueFileNum()
        {
            var dataTable =
                DataConnection.GetTable(
                    "SELECT `ValueString` FROM `preference` " +
                    "WHERE `PrefName` = 'TrojanExpressCollectPreviousFileNumber'");

            int previousNum = Convert.ToInt32(dataTable.Rows[0][0].ToString());

            dataTable.Dispose();

            var result =
                DataConnection.ExecuteNonQuery(
                    "UPDATE `preference` SET `ValueString` = '" + (previousNum + 1) + "' " +
                    "WHERE `PrefName` = 'TrojanExpressCollectPreviousFileNumber' " +
                    "AND `ValueString` = '" + previousNum + "'");

            while (result != 1) // Someone else sent one at the same time
            {
                previousNum++;

                DataConnection.ExecuteNonQuery(
                    "UPDATE `preference` SET `ValueString` = '" + (previousNum + 1) + "' " +
                    "WHERE `PrefName` = 'TrojanExpressCollectPreviousFileNumber' " +
                    "AND `ValueString`  ='" + previousNum + "'");
            }

            return previousNum + 1;
        }

        /// <summary>
        /// Get the list of records for the pending plan deletion report for plans that need to be brought to the patient's attention.
        /// </summary>
        public static DataTable GetPendingDeletionTable(Collection<string[]> deletePatientRecords)
        {
            string whereTrojanID = "";
            for (int i = 0; i < deletePatientRecords.Count; i++)
            {
                if (i > 0)
                {
                    whereTrojanID += "OR ";
                }
                whereTrojanID += "i.`TrojanID` = '" + deletePatientRecords[i][0] + "' ";
            }

            return DataConnection.GetTable(
                "SELECT DISTINCT " +
                "p.FName," +
                "p.LName," +
                "p.FName," +
                "p.LName," +
                "p.SSN," +
                "p.Birthdate," +
                "i.GroupNum," +
                "s.SubscriberID," +
                "i.TrojanID," +
                "CASE i.EmployerNum WHEN 0 THEN '' ELSE e.EmpName END," +
                "CASE i.EmployerNum WHEN 0 THEN '' ELSE e.Phone END," +
                "c.CarrierName," +
                "c.Phone " +
                "FROM patient p,insplan i,employer e,carrier c,inssub s " +
                "WHERE p.PatNum=s.Subscriber AND " +
                "(" + whereTrojanID + ") AND " +
                "i.CarrierNum=c.CarrierNum AND " +
                "s.PlanNum=i.PlanNum AND " +
                "(i.EmployerNum=e.EmployerNum OR i.EmployerNum=0) AND " +
                "(SELECT COUNT(*) FROM patplan a WHERE a.PatNum=p.PatNum AND a.InsSubNum=s.InsSubNum) > 0 " +
                "ORDER BY i.TrojanID,p.LName,p.FName");
        }

        /// <summary>
        /// Get the list of records for the pending plan deletion report for plans which need to be bought to Trojan's attention.
        /// </summary>
        public static DataTable GetPendingDeletionTableTrojan(Collection<string[]> deleteTrojanRecords)
        {
            string whereTrojanID = "";
            for (int i = 0; i < deleteTrojanRecords.Count; i++)
            {
                if (i > 0)
                {
                    whereTrojanID += "OR ";
                }
                whereTrojanID += "i.`TrojanID` = '" + deleteTrojanRecords[i][0] + "' ";
            }

            return DataConnection.GetTable(
                "SELECT DISTINCT " +
                "p.FName," +
                "p.LName," +
                "p.FName," +
                "p.LName," +
                "p.SSN," +
                "p.Birthdate," +
                "i.GroupNum," +
                "s.SubscriberID," +
                "i.TrojanID," +
                "CASE i.EmployerNum WHEN 0 THEN '' ELSE e.EmpName END," +
                "CASE i.EmployerNum WHEN 0 THEN '' ELSE e.Phone END," +
                "c.CarrierName," +
                "c.Phone " +
                "FROM patient p,insplan i,employer e,carrier c,inssub s " +
                "WHERE p.PatNum=s.Subscriber AND " +
                "(" + whereTrojanID + ") AND " +
                "i.CarrierNum=c.CarrierNum AND " +
                "s.PlanNum=i.PlanNum AND " +
                "(i.EmployerNum=e.EmployerNum OR i.EmployerNum=0) AND " +
                "(SELECT COUNT(*) FROM patplan a WHERE a.PatNum=p.PatNum AND a.InsSubNum=s.InsSubNum) > 0 " +
                "ORDER BY i.TrojanID,p.LName,p.FName");
        }

        public static InsPlan GetPlanWithTrojanID(string trojanID)
        {
            return Crud.InsPlanCrud.SelectOne(
                "SELECT * FROM `insplan` WHERE `TrojanID` = '" + MySqlHelper.EscapeString(trojanID) + "'");
        }

        /// <summary>
        /// This returns the number of plans affected.
        /// </summary>
        public static void UpdatePlan(TrojanObject troj, long planNum, bool updateBenefits)
        {
            long employerNum = Employers.GetEmployerNum(troj.ENAME);

            DataConnection.ExecuteNonQuery(
                "UPDATE insplan SET " +
                "EmployerNum = " + employerNum + ", " +
                "GroupName = '" + MySqlHelper.EscapeString(troj.PLANDESC) + "', " +
                "GroupNum = '" + MySqlHelper.EscapeString(troj.POLICYNO) + "', " +
                "CarrierNum = " + troj.CarrierNum + " " +
                "WHERE PlanNum = " + planNum);

            DataConnection.ExecuteNonQuery(
                "UPDATE `inssub` " +
                "SET `BenefitNotes` = '" + MySqlHelper.EscapeString(troj.BenefitNotes) + "' " +
                "WHERE `PlanNum` = " + planNum);

            if (updateBenefits)
            {
                DataConnection.ExecuteNonQuery("DELETE FROM `benefit` WHERE `PlanNum` = " + planNum);

                for (int j = 0; j < troj.BenefitList.Count; j++)
                {
                    troj.BenefitList[j].PlanNum = planNum;

                    Benefits.Insert(troj.BenefitList[j]);
                }

                InsPlans.ComputeEstimatesForTrojanPlan(planNum);
            }
        }

    }

    ///<summary>This is used as a container for plan and benefit info coming in from Trojan.</summary>
    [Serializable()]
    public class TrojanObject
    {
        ///<summary>TrojanID</summary>
        public string TROJANID;
        ///<summary>Employer name</summary>
        public string ENAME;
        ///<summary>GroupName</summary>
        public string PLANDESC;
        ///<summary>Carrier phone</summary>
        public string ELIGPHONE;
        ///<summary>GroupNum</summary>
        public string POLICYNO;
        ///<summary>Accepts eclaims</summary>
        public bool ECLAIMS;
        ///<summary>ElectID</summary>
        public string PAYERID;
        ///<summary>CarrierName</summary>
        public string MAILTO;
        ///<summary>Address</summary>
        public string MAILTOST;
        ///<summary>City</summary>
        public string MAILCITYONLY;
        ///<summary>State</summary>
        public string MAILSTATEONLY;
        ///<summary>Zip</summary>
        public string MAILZIPONLY;
        ///<summary>The only thing that will be missing from these benefits is the PlanNum.</summary>
        public List<Benefit> BenefitList;
        ///<summary>This can be filled at some point based on the carrier fields.</summary>
        public long CarrierNum;
        ///<summary></summary>
        public string BenefitNotes;
        ///<summary></summary>
        public string PlanNote;
        ///<summary></summary>
        public int MonthRenewal;
    }
}