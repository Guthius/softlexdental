/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using CodeBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDentBusiness
{
    public class Security
    {
        /// <summary>
        /// Tracks whether or not the user is logged in. 
        /// Security.CurUser==null usually is used for this purpose, 
        /// but in Middle Tier we do not null out CurUser so that queries can continue to be run on the web service.
        /// </summary>
        public static bool IsUserLoggedIn { get; set; }

        /// <summary>
        /// The last local datetime that there was any mouse or keyboard activity. 
        /// Used for auto logoff comparison and for disabling signal processing due to inactivity.
        /// Must be public so that it can be accessed from multiple application level classes.
        /// </summary>
        public static DateTime DateTimeLastActivity { get; set; }

        /// <summary>
        /// Gets or sets the currently logged in user.
        /// </summary>
        public static User CurrentUser { get; set; }

        [Obsolete("Use Environment.MachineName instead.")]
        public static string CurrentComputerName { get; set; }

        [Obsolete("Don't use the plaintext password of the user.")]
        public static string PasswordTyped { get; set; }

        /// <summary>
        /// <para>Checks whether the current user has the specified permissions.</para>
        /// <para>Displays a message box if the user does not have the specified permissions.</para>
        /// </summary>
        public static bool IsAuthorized(string permission) => 
            IsAuthorized(permission, DateTime.MinValue, false);

        public static bool IsAuthorized(string permission, DateTime date) =>
            IsAuthorized(permission, date, false);

        public static bool IsAuthorized(string permission, bool suppressMessage) =>
            IsAuthorized(permission, DateTime.MinValue, suppressMessage);

        public static bool IsAuthorized(string permission, DateTime date, bool suppressMessage) =>
            IsAuthorized(permission, date, suppressMessage, false);

        public static bool IsAuthorized(string permission, DateTime date, bool suppressMessage, bool suppressLockDateMessage) => 
            IsAuthorized(permission, date, suppressMessage, suppressLockDateMessage, null, -1, null, null);

        public static bool IsAuthorized(string permission, long? externalId, bool suppressMessage) =>
            IsAuthorized(permission, DateTime.MinValue, suppressMessage, true, null, -1, null, externalId);

        public static bool IsAuthorized(string permission, out string msg) => 
            IsAuthorized(permission, DateTime.MinValue, true, true, null, -1, null, null, out msg);

        public static bool IsAuthorized(string permission, DateTime date, long? procedureCodeId, double procedureCodeFee) => 
            IsAuthorized(permission, date, false, false, procedureCodeId, procedureCodeFee, null, null);
        
        public static bool IsAuthorized(string permission, DateTime date, bool suppressMessage, bool suppressLockDateMessage, long? procedureCodeId, double procedureCodeFee, long? sheetDefinitionId, long? externalId) => 
            IsAuthorized(permission, date, suppressMessage, suppressLockDateMessage, procedureCodeId, procedureCodeFee, sheetDefinitionId, externalId, out _);
        
        /// <summary>
        /// Checks to see if current user is authorized. It also checks any date restrictions. 
        /// If not authorized, it gives a Message box saying so and returns false.
        /// </summary>
        public static bool IsAuthorized(string permission, DateTime date, bool suppressMessage, bool suppressLockDateMessage, long? procedureCodeId, double procedureCodeFee, long? sheetDefinitionId, long? externalId, out string message)
        {
            message = "";

            if (CurrentUser == null)
            {
                message = "You have insufficient permissions to perform that action.";
                if (!suppressMessage)
                {
                    MessageBox.Show(
                        message, 
                        "Softlex Dental", 
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }

                return false;
            }

            try
            {
                return IsAuthorized(permission, date, suppressMessage, suppressLockDateMessage, CurrentUser, procedureCodeId, procedureCodeFee, sheetDefinitionId, externalId);
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    "Softlex Dental",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }
        }

        /// <summary>
        /// Will throw an error if not authorized and message not suppressed.
        /// </summary>
        public static bool IsAuthorized(string permission, DateTime date, bool suppressMessage, bool suppressLockDateMessage, User user, long? procedureCodeId, double procedureFee, long? sheetDefinitionId, long? externalId)
        {
            date = date.Date;

            if (!UserGroupPermission.HasPermission(user, permission, externalId))
            {
                if (!suppressMessage)
                {
                    throw new Exception(
                        $"You have insufficient permissions to perform that action.\r\n\r\n" +
                        $"To perform this action a administrator must grant you the '{UserGroupPermission.GetDescription(permission).ToUpper()}' permission.");
                }

                return false;
            }

            if (permission == Permissions.AccountingCreate || 
                permission == Permissions.AccountingEdit)
            {
                if (date <= Preference.GetDate(PreferenceName.AccountingLockDate))
                {
                    if (!suppressMessage && !suppressLockDateMessage)
                    {
                        throw new Exception("Locked by Administrator.");
                    }
                    return false;
                }
            }

            if (IsGlobalDateLock(permission, date, suppressMessage || suppressLockDateMessage, procedureCodeId.GetValueOrDefault(), procedureFee, sheetDefinitionId.GetValueOrDefault()))
            {
                return false;
            }

            //Check date/days limits on individual permission----------------------------------------------------------------
            //if (!GroupPermission.PermTakesDates(permission))
            //{
            //    return true;
            //}


            var dateLimit = UserGroupPermission.GetDateRestrictedForPermission(permission, user.GetGroups().Select(x => x.Id).ToList());
            if (!dateLimit.HasValue || date > dateLimit)
            {
                return true;
            }


            //Prevents certain bugs when 1/1/1 dates are passed in and compared----------------------------------------------
            //Handling of min dates.  There might be others, but we have to handle them individually to avoid introduction of bugs.
            if (permission == Permissions.ClaimDelete//older versions did not have SecDateEntry
                || permission == Permissions.ClaimSentEdit//no date sent was entered before setting claim received
                || permission == Permissions.EditCompletedProcedure//a completed procedure with a min date.
                || permission == Permissions.ProcComplEditLimited//because ProcComplEdit was in this list
                || permission == Permissions.EditProcedure//a completed EO or EC procedure with a min date.
                || permission == Permissions.InsPayEdit//a claim payment with no date.
                || permission == Permissions.InsWriteOffEdit//older versions did not have SecDateEntry or DateEntryC
                || permission == Permissions.TreatPlanEdit
                || permission == Permissions.AdjustmentEdit
                || permission == Permissions.CommlogEdit//usually from a conversion
                || permission == Permissions.ProcDelete//because older versions did not set the DateEntryC.
                || permission == Permissions.ImageDelete//In case an image has a creation date of DateTime.MinVal.
                || permission == Permissions.PerioEdit//In case perio chart exam has a creation date of DateTime.MinValue.
                || permission == Permissions.PreAuthSentEdit)//older versions did not have SecDateEntry
            {
                if (date.Year < 1880 && dateLimit.Value.Year < 1880)
                {
                    return true;
                }
            }
            if (!suppressMessage)
            {
                throw new Exception("Not authorized for" + "\r\n"
                    + UserGroupPermission.GetDescription(permission) + "\r\n" + "Date limitation");
            }
            return false;
        }

        public static bool IsGlobalDateLock(string perm, DateTime date, bool isSilent = false, long codeNum = 0, double procFee = -1, long sheetDefNum = 0)
        {
            if (!(new[] {
                 Permissions.AdjustmentCreate
                ,Permissions.AdjustmentEdit
                ,Permissions.PaymentCreate
                ,Permissions.PaymentEdit
                ,Permissions.CreateCompletedProcedure
                ,Permissions.EditCompletedProcedure
                ,Permissions.EditProcedure
			//,Permissions.ProcComplEditLimited
			//,Permissions.ImageDelete
				,Permissions.InsPayCreate
                ,Permissions.InsPayEdit
			//,Permissions.InsWriteOffEdit//per Nathan 7/5/2016 this should not be affected by the global date lock
				,Permissions.SheetEdit
                ,Permissions.SheetDelete
                ,Permissions.CommlogEdit
			//,Permissions.ClaimDelete //per Nathan 01/18/2018 this should not be affected by the global date lock
				,Permissions.PayPlanEdit
			//,Permissions.ClaimHistoryEdit //per Nathan & Mark 03/01/2018 this should not be affected by the global lock date, not financial data.
			}).Contains(perm))
            {
                return false;//permission being checked is not affected by global lock date.
            }
            if (date.Year == 1)
            {
                return false;//Invalid or MinDate passed in.
            }
            if (!Preference.GetBool(PreferenceName.SecurityLockIncludesAdmin) && UserGroupPermission.HasPermission(CurrentUser, Permissions.SecurityAdmin, 0))
            {
                return false;//admins are never affected by global date limitation when preference is false.
            }
            if (perm.In(Permissions.CreateCompletedProcedure, Permissions.EditCompletedProcedure, Permissions.EditProcedure)
                && ProcedureCodes.CanBypassLockDate(codeNum, procFee))
            {
                return false;
            }
            if (perm.In(Permissions.SheetEdit, Permissions.SheetDelete) && sheetDefNum > 0 && SheetDefs.CanBypassLockDate(sheetDefNum))
            {
                return false;
            }
            //If global lock is Date based.
            if (date <= Preference.GetDate(PreferenceName.SecurityLockDate))
            {
                if (!isSilent)
                {
                    MessageBox.Show("Locked by Administrator before " + Preference.GetDate(PreferenceName.SecurityLockDate).ToShortDateString());
                }
                return true;
            }
            //If global lock is days based.
            int lockDays = Preference.GetInt(PreferenceName.SecurityLockDays);
            if (lockDays > 0 && date <= DateTime.Today.AddDays(-lockDays))
            {
                if (!isSilent)
                {
                    MessageBox.Show("Locked by Administrator before " + lockDays.ToString() + " days.");
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a module that the user has permission to use. 
        /// 
        /// Tries the suggestedI first.  If a -1 is supplied, it tries to find any authorized module. 
        /// If no authorization for any module, it returns a -1, causing no module to be selected.
        /// </summary>
        public static int GetModule(int suggestI)
        {
            if (IsAuthorized(Permissions.SecurityAdmin)) return 0;

            if (suggestI != -1 && IsAuthorized(PermofModule(suggestI), DateTime.MinValue, true))
            {
                return suggestI;
            }

            for (int i = 0; i < 7; i++)
            {
                if (IsAuthorized(PermofModule(i), DateTime.MinValue, true))
                {
                    return i;
                }
            }

            return -1;
        }

        private static string PermofModule(int i)
        {
            switch (i)
            {
                case 0: return Permissions.ModuleAppointments;
                case 1: return Permissions.ModuleFamily;
                case 2: return Permissions.ModuleAccount;
                case 3: return Permissions.ModuleTreatmentPlan;
                case 4: return Permissions.ModuleChart;
                case 5: return Permissions.ModuleImages;
                case 6: return Permissions.ModuleManagement;
            }
            return "";
        }
    }
}
