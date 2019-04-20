using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using CodeBase;

namespace OpenDentBusiness
{
    ///<summary>In ProcGroupItems the ProcNum is a procedure in a group and GroupNum is the group the procedure is in. GroupNum is a FK to the Procedure table. There is a special type of procedure with the procedure code "~GRP~" that is used to indicate this is a group Procedure.</summary>
    public class ProcGroupItems
    {
        #region Modification Methods

        public static void InsertMany(List<ProcGroupItem> listGroupItems)
        {
            if (listGroupItems.IsNullOrEmpty())
            {
                return;
            }
            Crud.ProcGroupItemCrud.InsertMany(listGroupItems);
        }

        #endregion

        ///<summary></summary>
        public static List<ProcGroupItem> Refresh(long patNum)
        {
            string command = "SELECT procgroupitem.* FROM procgroupitem "
                    + "INNER JOIN procedurelog ON procedurelog.ProcNum=procgroupitem.GroupNum AND procedurelog.PatNum=" + POut.Long(patNum);
            return Crud.ProcGroupItemCrud.SelectMany(command);
        }

        ///<summary>Gets all the ProcGroupItems for a Procedure Group.</summary>
        public static List<ProcGroupItem> GetForGroup(long groupNum)
        {
            string command = "SELECT * FROM procgroupitem WHERE GroupNum = " + POut.Long(groupNum) + " ORDER BY ProcNum ASC";//Order is important for creating signature key in FormProcGroup.cs.
            return Crud.ProcGroupItemCrud.SelectMany(command);
        }

        ///<summary>Adds a procedure to a group.</summary>
        public static long Insert(ProcGroupItem procGroupItem)
        {
            return Crud.ProcGroupItemCrud.Insert(procGroupItem);
        }

        ///<summary>Deletes a ProcGroupItem based on its procGroupItemNum.</summary>
        public static void Delete(long procGroupItemNum)
        {
            DeleteMany(new List<long> { procGroupItemNum });
        }

        ///<summary>Deletes many ProcGroupItems based on the given list of ProcGroupItemNums.</summary>
        public static void DeleteMany(List<long> listProcGroupItemNums)
        {
            if (listProcGroupItemNums.IsNullOrEmpty())
            {
                return;
            }
            string command = $@"
					DELETE
					FROM procgroupitem
					WHERE ProcGroupItemNum IN({string.Join(",", listProcGroupItemNums.Select(x => POut.Long(x)))})";
            Db.NonQ(command);
        }

        ///<summary>Returns a count of the number of procedures attached to a group note.  Takes the ProcNum of a group note.
        ///Used when deleting group notes to determine which permission to check. If a list of complete statuses is not included, will default to
        ///C, EO, and EC.</summary>
        public static int GetCountCompletedProcsForGroup(long groupNum, List<ProcStat> listStatusComplete = null)
        {
            if (listStatusComplete == null)
            {
                listStatusComplete = new List<ProcStat> { ProcStat.C, ProcStat.EO, ProcStat.EC };
            }
            string command = $@"
				SELECT COUNT(*) 
				FROM procgroupitem 
				INNER JOIN procedurelog 
					ON procedurelog.ProcNum=procgroupitem.ProcNum
					AND procedurelog.ProcStatus IN ({string.Join(",", listStatusComplete.Select(x => POut.Int((int)x)))}) 
				WHERE GroupNum = {POut.Long(groupNum)}";
            return PIn.Int(Db.GetCount(command));
        }

        ///<summary>For the procnums passed in, returns a dictionary containing all grouped Procedures with the given statuses. The key is the groupNum.
        ///If no attached group nums, the group num will not be in the dictionary.</summary>
        public static Dictionary<long, List<Procedure>> GetCompletedProcsForGroups(List<long> listProcNums, List<ProcStat> listStatusComplete)
        {
            if (listProcNums.IsNullOrEmpty() || listStatusComplete.IsNullOrEmpty())
            {
                return new Dictionary<long, List<Procedure>>();
            }
            string command = $@"
				SELECT procedurelog.*,procgroupitem.GroupNum ProcGroupItemGroupNum
				FROM procgroupitem 
				INNER JOIN procedurelog 
					ON procedurelog.ProcNum=procgroupitem.ProcNum
					AND procedurelog.ProcStatus IN ({string.Join("", listStatusComplete.Select(x => POut.Int((int)x)))}) 
				WHERE procgroupitem.GroupNum IN ({string.Join(",", listProcNums)})";
            DataTable table = Db.GetTable(command);
            List<Procedure> listProcs = Crud.ProcedureCrud.TableToList(table);
            Dictionary<long, List<Procedure>> retVal = new Dictionary<long, List<Procedure>>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                long groupNum = PIn.Long(table.Rows[i]["ProcGroupItemGroupNum"].ToString());
                if (!retVal.ContainsKey(groupNum))
                {
                    retVal[groupNum] = new List<Procedure>();
                }
                retVal[groupNum].Add(listProcs[i]);
            }
            return retVal;
        }
    }
}