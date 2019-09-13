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
using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// Names of common user preferences.
    /// </summary>
    public static class UserPreferenceName
    {
        public const string Definition = "sys.definition";

        /// <summary>
        /// The ID of the last selected clinic.
        /// </summary>
        public const string LastSelectedClinic = "sys.last_selected_clinic";

        /// <summary>
        /// The name of the wiki home page.
        /// </summary>
        public const string WikiHomePage = "sys.wiki_home_page";

        /// <summary>
        /// <para>A comma delimited list of category definition ID's of the last expanded 
        /// categories for the user.</para>
        /// <para>Used by the auto notes form to restore expanded categories on load.</para>
        /// </summary>
        public const string AutoNoteExpandedCats = "sys.auto_note_expanded_categories";

        /// <summary>
        /// Indicates whether tasks should be collapsed or not by default.
        /// </summary>
        public const string TaskCollapse = "sys.task_collapse";

        /// <summary>
        /// When FormCommItem is in Persistent mode, clear the note text box after the user 
        /// creates a commlog.
        /// </summary>
        public const string CommlogPersistClearNote = "sys.commlog_persist_clear_note";

        /// <summary>
        /// When FormCommItem is in Persistent mode, clear the End text box after the user creates 
        /// a commlog.
        /// </summary>
        public const string CommlogPersistClearEndDate = "sys.commlog_persist_clear_end_date";

        /// <summary>
        /// When FormCommItem is in Persistent mode, update the Date / Time text box with NOW() 
        /// whenver the patient changes.
        /// </summary>
        public const string CommlogPersistUpdateDateTimeWithNewPatient = "sys.commlog_persist_update_date_with_new_patient";

        /// <summary>
        /// Whether or not to display just the currently selected exam in the Perio Chart.
        /// </summary>
        public const string PerioCurrentExamOnly = "sys.perio_show_current_exam_only";
       
        /// <summary>
        /// Text message grouping preference. 0 - None; 1 - By Patient
        /// </summary>
        public const string SmsGroupBy = "sys.sms_group_by";

        /// <summary>
        /// Stores a TaskListNum that the corresponding user wants to block all pop ups from.
        /// </summary>
        public const string TaskListBlock = "sys.task_list_block";
        
        /// <summary>
        /// Stores user specific values for programs.
        /// Currently only used in DoseSpot for the DoseSpot User ID.
        /// </summary>
        public const string Program = "sys.program";
        
        /// <summary>
        /// </summary>
        public const string SuppressLogOffMessage = "sys.supress_log_off_message";
        
        /// <summary>
        /// Sets the default state of the Account Module "Show Proc Breakdowns" checkbox.
        /// </summary>
        public const string AcctProcBreakdown = "sys.acct_show_proc_breakdowns";

        /// <summary>
        /// Stores user specific username for programs.
        /// </summary>
        [Obsolete("Use program specific preference instead.")]
        public const string ProgramUserName = "sys.program_username";

        /// <summary>
        /// Stores user specific password for programs.
        /// </summary>
        [Obsolete("Use program specific preference instead.")]
        public const string ProgramPassword = "sys.program_password";
        
        ///<summary>
        ///Stores user specific dashboard to open on load. FKey points to the SheetDef.SheetDefNum that the user last had open.
        ///</summary>
        public const string Dashboard = "sys.dashboard";
        
        /// <summary>
        /// Stores the Dynamic Chart Layout SheetDef.SheetDefNum selected by a user.
        /// </summary>
        public const string DynamicChartLayout = "sys.dynamic_chart_layout";
    }
}
