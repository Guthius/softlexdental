using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    /// Corresponds to the autocodeitem table in the database.
    /// There are multiple AutoCodeItems for a given AutoCode.
    /// Each Item has one ADA code.
    /// </summary>
    public class AutoCodeItem : DataRecord
    {
        /// <summary>FK to autocode.AutoCodeNum</summary>
        public long AutoCodeId;

        /// <summary>Do not use</summary>
        public string OldCode;

        /// <summary>FK to procedurecode.CodeNum</summary>
        public long ProcedureCodeId;

        /// <summary>
        /// Only used in the validation section when closing FormAutoCodeEdit.
        /// Will normally be empty.
        /// </summary>
        public List<AutoCodeCond> Conditions;



        public static long Insert(AutoCodeItem Cur)
        {
            return Crud.AutoCodeItemCrud.Insert(Cur);
        }


        public static void Update(AutoCodeItem Cur)
        {
            Crud.AutoCodeItemCrud.Update(Cur);
        }


        public static void Delete(AutoCodeItem Cur)
        {
            DataConnection.ExecuteNonQuery(
                "DELETE FROM auto_code_items WHERE id = " + Cur.Id);
        }

        ///<summary>Gets from cache.  No call to db.</summary>
        public static List<AutoCodeItem> GetListForCode(long autoCodeNum)
        {
            //No need to check RemotingRole; no call to db.
            return _autoCodeItemCache.GetWhereFromList(x => x.AutoCodeId == autoCodeNum);
        }

        //-----

        ///<summary>Only called from ContrChart.listProcButtons_Click.  Called once for each tooth selected and for each autocode item attached to the button.</summary>
        public static long GetCodeNum(long autoCodeNum, string toothNum, string surf, bool isAdditional, long patNum, int age, bool willBeMissing)
        {
            //No need to check RemotingRole; no call to db.
            bool allCondsMet;
            List<AutoCodeItem> listForCode = AutoCodeItems.GetListForCode(autoCodeNum);
            if (listForCode.Count == 0)
            {
                return 0;
            }
            //bool willBeMissing=Procedures.WillBeMissing(toothNum,patNum);//moved this out so that this method has no db call
            List<AutoCodeCond> condList;
            for (int i = 0; i < listForCode.Count; i++)
            {
                condList = AutoCodeConds.GetListForItem(listForCode[i].Id);
                allCondsMet = true;
                for (int j = 0; j < condList.Count; j++)
                {
                    if (!AutoCodeConds.ConditionIsMet(condList[j].Condition, toothNum, surf, isAdditional, willBeMissing, age))
                    {
                        allCondsMet = false;
                    }
                }
                if (allCondsMet)
                {
                    return listForCode[i].ProcedureCodeId;
                }
            }
            return listForCode[0].ProcedureCodeId;//if couldn't find a better match
        }

        ///<summary>Only called when closing the procedure edit window. Usually returns the supplied CodeNum, unless a better match is found.</summary>
        public static long VerifyCode(long codeNum, string toothNum, string surf, bool isAdditional, long patNum, int age,
            out AutoCode AutoCodeCur)
        {
            //No need to check RemotingRole; no call to db.
            bool allCondsMet;
            AutoCodeCur = null;
            if (!GetContainsKey(codeNum))
            {
                return codeNum;
            }
            if (!AutoCodes.GetContainsKey(GetOne(codeNum).AutoCodeId))
            {
                return codeNum;//just in case.
            }
            AutoCodeCur = AutoCodes.GetOne(GetOne(codeNum).AutoCodeId);
            if (AutoCodeCur.LessIntrusive)
            {
                return codeNum;
            }
            bool willBeMissing = Procedures.WillBeMissing(toothNum, patNum);
            List<AutoCodeItem> listForCode = AutoCodeItems.GetListForCode(GetOne(codeNum).AutoCodeId);
            List<AutoCodeCond> condList;
            for (int i = 0; i < listForCode.Count; i++)
            {
                condList = AutoCodeConds.GetListForItem(listForCode[i].Id);
                allCondsMet = true;
                for (int j = 0; j < condList.Count; j++)
                {
                    if (!AutoCodeConds.ConditionIsMet(condList[j].Condition, toothNum, surf, isAdditional, willBeMissing, age))
                    {
                        allCondsMet = false;
                    }
                }
                if (allCondsMet)
                {
                    return listForCode[i].ProcedureCodeId;
                }
            }
            return codeNum;//if couldn't find a better match
        }

        ///<summary>Checks inputs and determines if user should be prompted to pick a more applicable procedure code.</summary>
        ///<param name="verifyCode">This is the recommended code based on input. If it matches procCode return value will be false.</param>
        public static bool ShouldPromptForCodeChange(Procedure proc, ProcedureCode procCode, Patient pat, bool isMandibular,
            List<ClaimProc> claimProcsForProc, out long verifyCode)
        {
            //No remoting role check; no call to db and method utilizes an out parameter.
            verifyCode = proc.CodeNum;
            //these areas have no autocodes
            if (procCode.TreatArea == TreatmentArea.Mouth
                || procCode.TreatArea == TreatmentArea.Quad
                || procCode.TreatArea == TreatmentArea.Sextant
                || Procedures.IsAttachedToClaim(proc, claimProcsForProc))
            {
                return false;
            }
            //this represents the suggested code based on the autocodes set up.
            AutoCode AutoCodeCur = null;
            if (procCode.TreatArea == TreatmentArea.Arch)
            {
                if (string.IsNullOrEmpty(proc.Surf))
                {
                    return false;
                }
                if (proc.Surf == "U")
                {
                    verifyCode = AutoCodeItems.VerifyCode(procCode.CodeNum, "1", "", proc.IsAdditional, pat.PatNum, pat.Age, out AutoCodeCur);//max
                }
                else
                {
                    verifyCode = AutoCodeItems.VerifyCode(procCode.CodeNum, "32", "", proc.IsAdditional, pat.PatNum, pat.Age, out AutoCodeCur);//mand
                }
            }
            else if (procCode.TreatArea == TreatmentArea.ToothRange)
            {
                //test for max or mand.
                verifyCode = AutoCodeItems.VerifyCode(procCode.CodeNum, (isMandibular) ? "32" : "1", "", proc.IsAdditional, pat.PatNum, pat.Age, out AutoCodeCur);
            }
            else
            {//surf or tooth
                string claimSurf = Tooth.SurfTidyForClaims(proc.Surf, proc.ToothNum);
                verifyCode = AutoCodeItems.VerifyCode(procCode.CodeNum, proc.ToothNum, claimSurf, proc.IsAdditional, pat.PatNum, pat.Age, out AutoCodeCur);
            }
            return procCode.CodeNum != verifyCode;
        }
    }
}
