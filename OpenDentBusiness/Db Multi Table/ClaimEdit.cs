﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;

namespace OpenDentBusiness
{
    public class ClaimEdit
    {
        public static LoadData GetLoadData(Patient pat, Family fam, Claim claim)
        {
            LoadData data = new LoadData();
            data.ListPatPlans = PatPlans.Refresh(pat.PatNum);
            data.ListInsSubs = InsSubs.RefreshForFam(fam);
            data.ListInsPlans = InsPlans.RefreshForSubList(data.ListInsSubs);
            data.ListClaimProcs = ClaimProcs.Refresh(pat.PatNum);
            data.ListProcs = Procedures.Refresh(pat.PatNum);
            data.ListClaimValCodes = ClaimValCodeLogs.GetForClaim(claim.ClaimNum);
            data.ClaimCondCodeLogCur = ClaimCondCodeLogs.GetByClaimNum(claim.ClaimNum);
            data.TablePayments = ClaimPayments.GetForClaim(claim.ClaimNum);
            data.TablePayments.TableName = "ClaimPayments";
            data.ListToothInitials = ToothInitials.Refresh(pat.PatNum);
            data.ListCustomStatusEntries = ClaimTrackings.RefreshForClaim(ClaimTrackingType.StatusHistory, claim.ClaimNum);
            return data;
        }

        /// <summary>
        /// Updates the claim to the database.
        /// </summary>
        public static UpdateData UpdateClaim(Claim ClaimCur, List<ClaimValCodeLog> listClaimValCodes, ClaimCondCodeLog claimCondCodeLog, List<Procedure> listProcsToUpdatePlaceOfService, Patient pat, bool doMakeSecLog, string permissionToLog)
        {
            UpdateData data = new UpdateData();

            Claims.Update(ClaimCur);

            if (listClaimValCodes != null)
            {
                ClaimValCodeLogs.UpdateList(listClaimValCodes);
            }

            if (claimCondCodeLog != null)
            {
                if (claimCondCodeLog.IsNew)
                {
                    ClaimCondCodeLogs.Insert(claimCondCodeLog);
                }
                else
                {
                    ClaimCondCodeLogs.Update(claimCondCodeLog);
                }
            }

            foreach (Procedure proc in listProcsToUpdatePlaceOfService)
            {
                Procedure oldProc = proc.Copy();
                proc.PlaceService = ClaimCur.PlaceService;
                Procedures.Update(proc, oldProc);
            }

            if (doMakeSecLog)
            {
                SecurityLog.Write(ClaimCur.PatNum, permissionToLog,
                    pat.GetNameLF() + ", Date of service: " + ClaimCur.DateService.ToShortDateString());
            }

            data.ListSendQueueItems = Claims.GetQueueList(ClaimCur.ClaimNum, ClaimCur.ClinicNum, 0);

            return data;
        }

        /// <summary>
        /// Most of the data needed when updating a claim.
        /// </summary>
        [Serializable]
        public class UpdateData
        {
            public ClaimSendQueueItem[] ListSendQueueItems;
        }

        /// <summary>
        /// Most of the data needed to load FormClaimEdit.
        /// </summary>
        [Serializable]
        public class LoadData
        {
            public List<PatPlan> ListPatPlans;
            public List<InsSub> ListInsSubs;
            public List<InsPlan> ListInsPlans;
            public List<ClaimProc> ListClaimProcs;
            public List<Procedure> ListProcs;
            public List<ClaimValCodeLog> ListClaimValCodes;
            public ClaimCondCodeLog ClaimCondCodeLogCur;
            [XmlIgnore]
            public DataTable TablePayments;
            public List<ToothInitial> ListToothInitials;
            public List<ClaimTracking> ListCustomStatusEntries;
        }
    }
}
