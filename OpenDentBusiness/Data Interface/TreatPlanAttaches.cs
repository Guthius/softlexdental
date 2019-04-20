using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class TreatPlanAttaches
    {
        #region Get Methods
        #endregion

        #region Modification Methods

        #region Insert
        #endregion

        #region Update
        #endregion

        #region Delete
        #endregion

        #endregion

        #region Misc Methods
        #endregion


        ///<summary></summary>
        public static long Insert(TreatPlanAttach treatPlanAttach)
        {
            return Crud.TreatPlanAttachCrud.Insert(treatPlanAttach);
        }

        ///<summary></summary>
        public static void Update(TreatPlanAttach treatPlanAttach)
        {
            Crud.TreatPlanAttachCrud.Update(treatPlanAttach);
        }

        ///<summary></summary>
        public static void Delete(long treatPlanAttachNum)
        {
            Crud.TreatPlanAttachCrud.Delete(treatPlanAttachNum);
        }

        ///<summary></summary>
        public static List<TreatPlanAttach> GetAllForPatNum(long patNum)
        {
            List<TreatPlan> listTreatPlans = TreatPlans.GetAllForPat(patNum);
            if (listTreatPlans.Count == 0)
            {
                return new List<TreatPlanAttach>();
            }
            return GetAllForTPs(listTreatPlans.Select(x => x.TreatPlanNum).Distinct().ToList());
        }

        ///<summary>Gets all treatplanattaches with TreatPlanNum in listTpNums.</summary>
        public static List<TreatPlanAttach> GetAllForTPs(List<long> listTpNums)
        {
            if (listTpNums.Count == 0)
            {
                return new List<TreatPlanAttach>();
            }
            string command = "SELECT * FROM treatplanattach WHERE TreatPlanNum IN (" + string.Join(",", listTpNums) + ")";
            return Crud.TreatPlanAttachCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static List<TreatPlanAttach> GetAllForTreatPlan(long treatPlanNum)
        {
            string command = "SELECT * FROM treatplanattach WHERE TreatPlanNum=" + POut.Long(treatPlanNum);
            return Crud.TreatPlanAttachCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static void DeleteOrphaned()
        {
            //Orphaned TreatPlanAttaches due to missing treatment plans
            string command = "DELETE FROM treatplanattach WHERE TreatPlanNum NOT IN (SELECT TreatPlanNum FROM treatplan)";
            Db.NonQ(command);
            //Orphaned TreatPlanAttaches due to missing procedures or procedures that are no longer TP or TPi status.
            command = "DELETE FROM treatplanattach WHERE ProcNum NOT IN (SELECT ProcNum FROM procedurelog " +
                "WHERE ProcStatus IN (" + string.Join(",", new[] { (int)ProcStat.TP, (int)ProcStat.TPi }) + "))";
            Db.NonQ(command);
        }

        ///<summary></summary>
        public static void Sync(List<TreatPlanAttach> listTreatPlanAttachNew, long treatPlanNum)
        {
            List<TreatPlanAttach> listTreatPlanAttachDB = TreatPlanAttaches.GetAllForTreatPlan(treatPlanNum);
            Crud.TreatPlanAttachCrud.Sync(listTreatPlanAttachNew, listTreatPlanAttachDB);
        }

        ///<summary></summary>
        public static void DeleteMany(List<TreatPlanAttach> listTreatPlanAttaches)
        {
            foreach (TreatPlanAttach treatPlanAttach in listTreatPlanAttaches)
            {
                Crud.TreatPlanAttachCrud.Delete(treatPlanAttach.TreatPlanAttachNum);
            }
        }
    }
}