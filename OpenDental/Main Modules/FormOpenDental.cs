using CodeBase;
using Microsoft.Win32;
using OpenDental.Properties;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.UI;
using SLDental.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace OpenDental
{
    public partial class FormOpenDental : ODForm
    {
        #region Classwide Variables
        ///<summary>This is the singleton instance of the FormOpenDental. This allows us to have S_ methods that are public static
        ///and can be called from anywhere in the program to update FormOpenDental.</summary>
        private static FormOpenDental _formOpenDentalS;

        ///<summary>A list of button definitions for this computer.  These button defs display in the lightSignalGrid1 control.</summary>
        private SigButDef[] SigButDefList;

        ///<summary>When user logs out, this keeps track of where they were for when they log back in.</summary>
        private int LastModule;

        ///<summary>This list will only contain events for this computer where the users clicked to disable a popup for a specified period of time.  So it won't typically have many items in it.</summary>
        private List<PopupEvent> PopupEventList;

        //private UserControlPhonePanel phonePanel;
        ///<summary>Command line args passed in when program starts.</summary>
        public string[] CommandLineArgs;
        ///<summary>True if there is already a different instance of OD running.  This prevents attempting to start the listener.</summary>
        public bool IsSecondInstance;
        private UserControlTasks userControlTasks1;
        private UserControlDashboard userControlPatientDashboard;
        private ContrAppt ContrAppt2;
        private ContrFamily ContrFamily2;
        private ContrFamilyEcw ContrFamily2Ecw;
        private ContrAccount ContrAccount2;
        private ContrTreat ContrTreat2;
        private ContrChart ContrChart2;
        private ContrImages ContrImages2;
        private ContrStaff ContrManage2;
        private OutlookBar myOutlookBar;
        private ODToolBar ToolBarMain;
        private Form FormRecentlyOpenForLogoff;
        ///<summary>When auto log off is in use, we don't want to log off user if they are in the FormLogOn window.  Mostly a problem when using web service because CurUser is not null.</summary>
        private bool IsFormLogOnLastActive;

        private FormCreditRecurringCharges FormCRC;

        private FormTerminalManager formTerminalManager;

        private long _previousPatNum;
        private DateTime _datePopupDelay;
        ///<summary>A secondary cache only used to determine if preferences related to the redrawing of the Chart module have been changed.</summary>
        private Dictionary<string, object> dictChartPrefsCache = new Dictionary<string, object>();
        ///<summary>A secondary cache only used to determine if preferences related to the redrawing of the non-modal task list have been changed.</summary>
        private Dictionary<string, object> dictTaskListPrefsCache = new Dictionary<string, object>();
        //(Deprecated) Moved to Clinics.ClinicNum
        //public static long ClinicNum=0;
        ///<summary>This is used to determine how Open Dental closed.  If this is set to anything but 0 then some kind of error occurred and Open Dental was forced to close.  Currently only used when updating Open Dental silently.</summary>
        public static int ExitCode = 0;
        ///<summary>Will be set to true if the STOP SLAVE SQL was run on the replication server for which the local replication monitor is watching.
        ///Replicaiton is NOT broken when this flag is true, because the user can re-enable replicaiton using the START SLAVE SQL without any ill effects.
        ///This flag is used to display a warning to the user, but will not ever block the user from using OD.</summary>
        private bool _isReplicationSlaveStopped = false;
       

        ///<summary>A specific reference to the "Text" button.  This special reference helps us preserve the notification text on the button after setup is modified.</summary>
        private ODToolBarButton _butText;
        ///<summary>A specific reference to the "Task" button.
        ///This special reference helps us refresh the notification text on the button after the user changes.</summary>
        private ODToolBarButton _butTask;

        /// <summary>Command line can pass in show=... "Popup", "Popups", "ApptsForPatient", or "SearchPatient".  Stored here as lowercase.</summary>
        private string _StrCmdLineShow = "";

        private FormSmsTextMessaging _formSmsTextMessaging;
        private FormQuery _formUserQuery;
        private OpenDentalGraph.FormDashboardEditTab _formDashboardEditTab;

        private static Dictionary<long, Dictionary<long, DateTime>> _dicBlockedAutomations;
        
        
        ///<summary>Dictionary of AutomationNums mapped to a dictionary of patNums and dateTimes. 
        ///<para>The dateTime is the time that the given automation for a specific patient should be blocked until.</para>
        ///<para>Dictionary removes any entries whos blocked until dateTime is greater than DateTime.Now before returning.</para>
        ///<para>Currently only used when triggered Automation.AutoAction == AutomationAction.PopUp</para></summary>
        public static Dictionary<long, Dictionary<long, DateTime>> DicBlockedAutomations
        {
            get
            {
                if (_dicBlockedAutomations == null)
                {
                    _dicBlockedAutomations = new Dictionary<long, Dictionary<long, DateTime>>();
                    return _dicBlockedAutomations;
                }
                List<long> listAutoNums = _dicBlockedAutomations.Keys.ToList();
                List<long> listPatNums;
                foreach (long automationNum in listAutoNums)
                {//Key is an AutomationNum
                    listPatNums = _dicBlockedAutomations[automationNum].Keys.ToList();
                    foreach (long patNum in listPatNums)
                    {//Key is a patNum for current AutomationNum key.
                        if (_dicBlockedAutomations[automationNum][patNum] > DateTime.Now)
                        {//Disable time has not expired yet.
                            continue;
                        }
                        _dicBlockedAutomations[automationNum].Remove(patNum);//Remove automation for current user since block time has expired.
                                                                             //Since we removed an entry from the lower level dictionary we need to check if there are still entries in the top level dictionary. 
                    }
                    if (_dicBlockedAutomations[automationNum].Count() == 0)
                    {//Top level dictionary no longer contains entries for current automationNum.
                        _dicBlockedAutomations.Remove(automationNum);
                    }
                }
                return _dicBlockedAutomations;
            }
        }








        ///<summary>Tracks the reminder tasks for the currently logged in user.  Is null until the first signal refresh.  
        ///Includes new and viewed tasks.</summary>
        private List<Task> _listReminderTasks = null;
        ///<summary>Tracks reminder tasks that were not allowed to popup because we had too many FormTaskEdit windows open already.</summary>
        private List<Task> _listReminderTasksOverLimit = null;
        ///<summary>Tracks the normal (non-reminder) tasks for the currently logged in user.  Is null until the first signal refresh.</summary>
        private List<long> _listNormalTaskNums = null;
        ///<summary>Tracks the UserNum of the user for which the _listReminderTaskNums and _listOtherTaskNums belong to
        ///so we can compensate for different users logging off/on.</summary>
        private long _tasksUserNum = 0;
        ///<summary>Task Popups use this upper limit of open FormTaskEdit instances to determine if a task should popup.  More than 115 open FormTaskEdit
        ///has been observed to crash the program.  See task #1481164.</summary>
        private static int _popupPressureReliefLimit = 20;//20 is chosen arbitrarily.  We could implement a preference for this, with a max of 115.
                                                          ///<summary>The date the appointment module reminders tab was last refreshed.</summary>
        private DateTime _dateReminderRefresh = DateTime.MinValue;
        
        ///<summary>HQ only. Keep track of the last time the office down was checked. Too taxing on the server to perform every 1.6 seconds with the rest 
        ///of the HQ thread metrics. Will be refreshed on ProcessSigsIntervalInSecs interval.</summary>
        private DateTime _hqOfficeDownLastRefreshed = DateTime.MinValue;
        ///<summary>List of AlerReads for the current User.</summary>
        List<AlertRead> _listAlertReads = new List<AlertRead>();
        ///<summary>List of AlertItems for the current user and clinic.</summary>
        List<AlertItem> _listAlertItems = new List<AlertItem>();

        private FormXWebTransactions FormXWT;

        private static bool _isTreatPlanSortByTooth;
        
        ///<summary>In most cases, CurPatNum should be used instead of _CurPatNum.</summary>
        private static long _curPatNum;

        ///<summary>We will send a maximum of 1 exception to HQ that occurs when processing signals.</summary>
        private Exception _signalsTickException;

        private SplitContainerNoFlicker splitContainerNoFlickerDashboard;

        ///<summary>List of tab titles for the TabProc control. Used to get accurate preview in sheet layout design view. 
        ///Returns a list of one item called "Tab" if something goes wrong.</summary>
        public static List<string> S_Contr_TabProcPageTitles
        {
            get
            {
                return _formOpenDentalS.ContrChart2.TabProcPageTitles;
            }
        }
        #endregion Classwide Variables

        ///<summary>Represents if the regkey is a developer regkey.</summary>
        public static bool RegKeyIsForTesting;

        ///<summary>PatNum for currently loaded patient.</summary>
        public static long CurPatNum
        {
            get
            {
                return _curPatNum;
            }
            set
            {
                if (value == _curPatNum)
                {
                    return;
                }
                _curPatNum = value;
                PatientChangedEvent.Fire(ODEventType.Patient, value);
            }
        }

        /// <summary>
        /// Inherits value from PrefName.TreatPlanSortByTooth on startup.  
        /// The user can change this value without changing the pref from the treatplan module.
        /// </summary>
        public static bool IsTreatPlanSortByTooth
        {
            get
            {
                return _isTreatPlanSortByTooth;
            }
            set
            {
                _isTreatPlanSortByTooth = value;
                Preferences.IsTreatPlanSortByTooth = value;
            }
        }

        ///<summary></summary>
        public FormOpenDental(string[] cla)
        {
            _formOpenDentalS = this;

            Logger.Write(LogLevel.Info, "Initializing Open Dental...");

            CommandLineArgs = cla;
            Action actionCloseSplashWindow = null;
            if (CommandLineArgs.Length == 0)
            {
                actionCloseSplashWindow = ShowSplash(true);
            }
            InitializeComponent();




            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
            //toolbar		
            ToolBarMain = new ODToolBar();
            UpdateSplashProgress("Loading toolbar", 5);
            ToolBarMain.Location = new Point(51, 0);
            ToolBarMain.Size = new Size(931, 25);
            ToolBarMain.Dock = DockStyle.Top;
            ToolBarMain.ButtonClick += new EventHandler<ODToolBarButtonClickEventArgs>(ToolBarMain_ButtonClick);
            this.Controls.Add(ToolBarMain);
            //outlook bar
            UpdateSplashProgress("Loading outlook bar", 10);


            myOutlookBar = new OutlookBar();
            myOutlookBar.Location = new Point(0, 0);
            myOutlookBar.Size = new Size(70, 400);
            myOutlookBar.Dock = DockStyle.Left;
            myOutlookBar.ButtonClicked += new EventHandler<OutlookBarButtonEventArgs>(myOutlookBar_ButtonClicked);
            this.Controls.Add(myOutlookBar);


            //MAIN MODULE CONTROLS
            //contrAppt
            UpdateSplashProgress("Initializing appointment module", 15);
            ContrAppt2 = new ContrAppt() { Visible = false };
            ContrAppt2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom)));
            ContrAppt2.Size = new Size(splitContainerNoFlickerDashboard.Panel1.Width, splitContainerNoFlickerDashboard.Panel1.Height);
            splitContainerNoFlickerDashboard.Panel1.Controls.Add(ContrAppt2);

            //contrFamily
            UpdateSplashProgress("Initializing family module", 20);
            ContrFamily2 = new ContrFamily() { Visible = false };
            ContrFamily2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom)));
            ContrFamily2.Size = new Size(splitContainerNoFlickerDashboard.Panel1.Width, splitContainerNoFlickerDashboard.Panel1.Height);
            splitContainerNoFlickerDashboard.Panel1.Controls.Add(ContrFamily2);
            //contrFamilyEcw
            UpdateSplashProgress("Initializing family ecw", 25);
            ContrFamily2Ecw = new ContrFamilyEcw() { Visible = false };
            ContrFamily2Ecw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom)));
            ContrFamily2Ecw.Size = new Size(splitContainerNoFlickerDashboard.Panel1.Width, splitContainerNoFlickerDashboard.Panel1.Height);
            splitContainerNoFlickerDashboard.Panel1.Controls.Add(ContrFamily2Ecw);
            //contrAccount
            UpdateSplashProgress("Initializing account module", 30);
            ContrAccount2 = new ContrAccount() { Visible = false };
            ContrAccount2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom)));
            ContrAccount2.Size = new Size(splitContainerNoFlickerDashboard.Panel1.Width, splitContainerNoFlickerDashboard.Panel1.Height);
            splitContainerNoFlickerDashboard.Panel1.Controls.Add(ContrAccount2);
            //contrTreat
            UpdateSplashProgress("Initializing treatement module", 35);
            ContrTreat2 = new ContrTreat() { Visible = false };
            ContrTreat2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom)));
            ContrTreat2.Size = new Size(splitContainerNoFlickerDashboard.Panel1.Width, splitContainerNoFlickerDashboard.Panel1.Height);
            splitContainerNoFlickerDashboard.Panel1.Controls.Add(ContrTreat2);
            //contrChart
            UpdateSplashProgress("Initializing chart module", 40);
            ContrChart2 = new ContrChart() { Visible = false };
            ContrChart2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom)));
            ContrChart2.Size = new Size(splitContainerNoFlickerDashboard.Panel1.Width, splitContainerNoFlickerDashboard.Panel1.Height);
            splitContainerNoFlickerDashboard.Panel1.Controls.Add(ContrChart2);
            //contrImages
            UpdateSplashProgress("Initializing document module", 70);
            ContrImages2 = new ContrImages() { Visible = false };
            ContrImages2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom)));
            ContrImages2.Size = new Size(splitContainerNoFlickerDashboard.Panel1.Width, splitContainerNoFlickerDashboard.Panel1.Height);
            splitContainerNoFlickerDashboard.Panel1.Controls.Add(ContrImages2);
            //contrManage
            UpdateSplashProgress("Initializing management module", 80);
            ContrManage2 = new ContrStaff() { Visible = false };
            ContrManage2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom)));
            ContrManage2.Size = new Size(splitContainerNoFlickerDashboard.Panel1.Width, splitContainerNoFlickerDashboard.Panel1.Height);

            modules.Add(ContrManage2);

            splitContainerNoFlickerDashboard.Panel1.Controls.Add(ContrManage2);
            UpdateSplashProgress("Loading dashboards", 85);
            userControlPatientDashboard = new UserControlDashboard();
            userControlPatientDashboard.Size = new Size(splitContainerNoFlickerDashboard.Panel2.Width, splitContainerNoFlickerDashboard.Panel2.Height);
            splitContainerNoFlickerDashboard.Panel2.Controls.Add(userControlPatientDashboard);
            UpdateSplashProgress("Loading tasks", 90);
            userControlTasks1 = new UserControlTasks() { Visible = false };
            this.Controls.Add(userControlTasks1);

            Logger.Write(LogLevel.Info, "Open Dental initialization complete.");

            //Plugins.HookAddCode(this,"FormOpenDental.Constructor_end");//Can't do this because no plugins loaded.
            UpdateSplashProgress("Initialization complete", 100);
            actionCloseSplashWindow?.Invoke();
        }


        protected override void OnHelp(ODHelpEventArgs e)
        {
            string moduleText = GetSelectedModuleName();

            switch (moduleText)
            {
                case "Appts":
                    e.FormName = nameof(ContrAppt);
                    break;

                case "Family":
                    e.FormName = nameof(ContrFamily);
                    break;

                case "Account":
                    e.FormName = nameof(ContrAccount);
                    break;

                case "Treat' Plan":
                    e.FormName = nameof(ContrTreat);
                    break;

                case "Chart":
                    e.FormName = nameof(ContrChart);
                    break;

                case "Images":
                    e.FormName = nameof(ContrImages);
                    break;

                case "Manage":
                    e.FormName = nameof(ContrStaff);
                    break;

                default:
                    e.Handled = true;
                    break;
            }
        }


        private void FormOpenDental_Load(object sender, System.EventArgs e)
        {
            //In order for the "Automatically show the touch keyboard in windowed apps when there's no keyboard attached to your device" Windows setting
            //to work we have to invoke the following line.  Surrounded in a try catch because the user can simply put the OS into tablet mode.
            //Affects WPF RichTextBoxes accross the entire program.
            ODException.SwallowAnyException(() =>
            {
                System.Windows.Automation.AutomationElement.FromHandle(Handle);//Just invoking this method wakes up something deep within Windows...
            });

            //Flag the userod cache as NOT allowed to cache any items for security purposes.
            Userods.SetIsCacheAllowed(false);
            //FormSplash can cause FormOpenDental to open behind other applications.
            TopMost = true;
            Application.DoEvents();
            TopMost = false;
            Activate();

            //Have the auto retry timeout monitor throw an exception after the timeout specified above.
            //If left false, then the application would fall into an infinite wait and we can't afford to have that happen at this point.
            //This will get set to false down below after we register for the DataConnectionLost event which will display the Data Connection Lost window.

            allNeutral();
            string odUser = "";
            string odPassword = "";
            string databaseName = "";


            var versionOd = Assembly.GetAssembly(typeof(FormOpenDental)).GetName().Version;
            var versionObBus = Assembly.GetAssembly(typeof(Db)).GetName().Version;
            if (versionOd != versionObBus)
            {
                MessageBox.Show(
                    "Mismatched program file versions. Please run the Open Dental setup file again on this computer.",
                    "Open Dental", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                
                Environment.Exit(ExitCode);
                return;
            }

            // Open the form to pick the database.
            using (var formChooseDatabase = new FormChooseDatabase())
            {
                formChooseDatabase.NoShow = databaseName != "";

                while (true)
                {
                    // Most users will loop through once. If user tries to connect to a db with replication failure, they will loop through again.
                    if (formChooseDatabase.NoShow)
                    {
                        try
                        {
                            CentralConnections.TryToConnect(
                                formChooseDatabase.CentralConnectionCur,
                                formChooseDatabase.NoShow,
                                CommandLineArgs.Length != 0);
                        }
                        catch (Exception)
                        {
                            if (formChooseDatabase.ShowDialog(this) == DialogResult.Cancel)
                            {
                                Environment.Exit(ExitCode);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (formChooseDatabase.ShowDialog(this) == DialogResult.Cancel)
                        {
                            Environment.Exit(ExitCode);
                            return;
                        }
                    }

                    Cursor = Cursors.WaitCursor;
                    PluginManager.LoadDirectory(
                        Path.Combine(
                            Application.StartupPath, "Plugins"));

                    if (!LoadPreferences()) //In Release, refreshes the Pref cache if conversion successful.
                    {
                        Cursor = Cursors.Default;
                        if (ExitCode == 0)
                        {
                            // PrefsStartup failed and ExitCode is still 0 which means an unexpected error must have occured.
                            // Set the exit code to 999 which will represent an Unknown Error
                            ExitCode = 999;
                        }
                        Environment.Exit(ExitCode);
                        return;
                    }

                    if (ReplicationServers.Server_id != 0 && 
                        ReplicationServers.Server_id == Preference.GetLong(PreferenceName.ReplicationFailureAtServer_id))
                    {
                        MessageBox.Show( 
                            "This database is temporarily unavailable. Please connect instead to your alternate database at the other location.", 
                            "Open Dental", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Information);

                        formChooseDatabase.NoShow = false; // This ensures they will get a choose db window next time through the loop.
                        ReplicationServers.Server_id = -1;
                        continue;
                    }
                    break;
                }
            }

            // TODO: Logger.DoVerboseLogging = PrefC.IsVerboseLoggingSession;

            CreateFHIRConfigFile();
            //Setting the time that we want to wait when the database connection has been lost.
            //We don't want a LostConnection event to fire when updating because of Silent Updating which would fail due to window pop-ups from this event.
            //When the event is triggered a "connection lost" window will display allowing the user to attempt reconnecting to the database
            //and then resume what they were doing.  The purpose of this is to prevent UE's from happening with poor connections or temporary outages.
            DataConnectionEvent.Fired += DataConnection_ConnectionLost;//Hook up the connection lost event. Nothing prior to this point will have LostConnection events fired.

            ODUIExtensions.ItemTranslator = new LansTranslate();
            RefreshLocalData(InvalidType.Prefs);//should only refresh preferences so that SignalLastClearedDate preference can be used in ClearOldSignals()
            Signalods.ClearOldSignals();
            //We no longer do this shotgun approach because it can slow the loading time.
            //RefreshLocalData(InvalidType.AllLocal);
            List<InvalidType> invalidTypes = new List<InvalidType>();
            //invalidTypes.Add(InvalidType.Prefs);//Preferences were refreshed above.  The only preference which might be stale is SignalLastClearedDate, but it is not used anywhere after calling ClearOldSignals() above.
            invalidTypes.Add(InvalidType.Defs);
            invalidTypes.Add(InvalidType.Providers);//obviously heavily used
            invalidTypes.Add(InvalidType.Programs);//already done above, but needs to be done explicitly to trigger the PostCleanup 
            invalidTypes.Add(InvalidType.ToolBut);//so program buttons will show in all the toolbars
                                                  //InvalidType.PatFields is necessary because the appts for the day are drawn in parallel threads and without it the cache is filled in each
                                                  //thread resulting in multiple calls to fill the ApptFieldDef and PatFieldDef caches.
            invalidTypes.Add(InvalidType.PatFields);
            if (Programs.UsingEcwTightMode())
            {
                lightSignalGrid1.Visible = false;
            }
            else
            {
                invalidTypes.Add(InvalidType.SigMessages);//so when mouse moves over light buttons, it won't crash
            }
            //Plugins.LoadAllPlugins(this);//moved up from right after optimizing tooth chart graphics.  New position might cause problems.
            //It was moved because RefreshLocalData()=>RefreshLocalDataPostCleanup()=>ContrChart2.InitializeLocalData()=>LayoutToolBar() has a hook.
            //Moved it up again on 10/3/13
            RefreshLocalData(invalidTypes.ToArray());
            FillSignalButtons();
            ContrManage2.InitializeOnStartup();//so that when a signal is received, it can handle it.
                                               //Lan.Refresh();//automatically skips if current culture is en-US
                                               //LanguageForeigns.Refresh(CultureInfo.CurrentCulture);//automatically skips if current culture is en-US			

            // TODO: Create buttons
            //myOutlookBar.RefreshButtons();

            if (!File.Exists("Help.chm"))
            {
                menuItemHelpWindows.Visible = false;
            }

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {//Create A to Z unsupported on Unix for now.
                menuItemCreateAtoZFolders.Visible = false;
            }
            if (!Preference.GetBool(PreferenceName.ProcLockingIsAllowed))
            {
                menuItemProcLockTool.Visible = false;
            }
            if (Security.IsAuthorized(Permissions.ProcCodeEdit, true) && !Preference.GetBool(PreferenceName.ADAdescriptionsReset))
            {
                ProcedureCodes.ResetADAdescriptions();
                Preference.Update(PreferenceName.ADAdescriptionsReset, true);
            }
            //Spawn a thread so that attempting to start services on this computer does not hinder the loading time of Open Dental.
            //This is placed before login on pupose so it will run even when the user does not login properly.
            BeginODServiceStarterThread();

            LogOnOpenDentalUser(odUser, odPassword);
            //At this point a user has successfully logged in.  Flag the userod cache as safe to cache data.
            Userods.SetIsCacheAllowed(true);
            //If clinics are enabled, we will set the public ClinicNum variable
            //If the user is restricted to a clinic(s), and the computerpref clinic is not one of the user's restricted clinics, the user's clinic will be selected
            //If the user is not restricted, or if the user is restricted but has access to the computerpref clinic, the computerpref clinic will be selected
            //The ClinicNum will determine which view is loaded, either from the computerpref table or from the userodapptview table
            if (Preferences.HasClinicsEnabled && Security.CurUser != null)
            {//If block must be run before StartCacheFillForFees() so correct clinic filtration occurs.
                Clinics.LoadClinicNumForUser();
                RefreshMenuClinics();
            }
            BeginODDashboardStarterThread();
            FillSignalButtons();

            // TODO: Fix...

            //if (Preferences.AtoZfolderUsed == DataStorageType.LocalAtoZ)
            //{
            //    string prefImagePath = ImageStore.GetPreferredAtoZpath();
            //    if (prefImagePath == null || !Directory.Exists(prefImagePath))
            //    {
            //        using (var formPath = new FormPath())
            //        {
            //            formPath.IsStartingUp = true;
            //            if (formPath.ShowDialog() != DialogResult.OK)
            //            {
            //                MessageBox.Show(
            //                    "Invalid A to Z path. Closing program.",
            //                    "Open Dental",
            //                    MessageBoxButtons.OK,
            //                    MessageBoxIcon.Error);
            //
            //                Application.Exit();
            //            }
            //        }
            //    }
            //}

            IsTreatPlanSortByTooth = Preference.GetBool(PreferenceName.TreatPlanSortByTooth); //not a great place for this, but we don't have a better alternative.
            if (userControlTasks1.Visible)
            {
                userControlTasks1.InitializeOnStartup();
            }
            myOutlookBar.SelectedIndex = Security.GetModule(0);//for eCW, this fails silently.
            if (Programs.UsingEcwTightOrFullMode()
                || (HL7Defs.IsExistingHL7Enabled() && !HL7Defs.GetOneDeepEnabled().ShowAppts))
            {
                myOutlookBar.SelectedIndex = 4;//Chart module
                LayoutControls();
            }
            if (Programs.UsingOrion)
            {
                myOutlookBar.SelectedIndex = 1;//Family module
            }
            myOutlookBar.Invalidate();
            LayoutToolBar();
            RefreshMenuReports();
            Cursor = Cursors.Default;

            if (myOutlookBar.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "You do not have permission to use any modules.", 
                    "Open Dental", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }


            Bridges.Trojan.StartupCheck();
            FormUAppoint.StartThreadIfEnabled();
            Bridges.ICat.StartFileWatcher();
            Bridges.TigerView.StartFileWatcher();

            if (Preference.GetDate(PreferenceName.BackupReminderLastDateRun).AddMonths(1) < DateTime.Today)
            {
                FormBackupReminder FormBR = new FormBackupReminder();
                FormBR.ShowDialog();
                if (FormBR.DialogResult == DialogResult.OK)
                {
                    Preference.Update(PreferenceName.BackupReminderLastDateRun, DateTimeOD.Today);
                }
                else
                {
                    Application.Exit();
                    return;
                }
            }

            FillPatientButton(null);
            ProcessCommandLine(CommandLineArgs);
            ODException.SwallowAnyException(() =>
            {
                Computer.UpdateHeartBeat(Environment.MachineName);
            });
            Text = PatientL.GetMainTitle(Patients.GetPat(CurPatNum), Clinics.ClinicNum);
            Security.DateTimeLastActivity = DateTime.Now;



            Patient pat = Patients.GetPat(CurPatNum);
            if (pat != null && (_StrCmdLineShow == "popup" || _StrCmdLineShow == "popups") && myOutlookBar.SelectedIndex != -1)
            {
                FormPopupsForFam FormP = new FormPopupsForFam(PopupEventList);
                FormP.PatCur = pat;
                FormP.ShowDialog();
            }
            bool isApptModuleSelected = false;
            if (myOutlookBar.SelectedIndex == 0)
            {
                isApptModuleSelected = true;
            }
            if (CurPatNum != 0 && _StrCmdLineShow == "apptsforpatient" && isApptModuleSelected)
            {
                ContrAppt2.DisplayOtherDlg(false);
            }
            if (_StrCmdLineShow == "searchpatient")
            {
                FormPatientSelect formPatientSelect = new FormPatientSelect();
                formPatientSelect.ShowDialog();
                if (formPatientSelect.DialogResult == DialogResult.OK)
                {
                    CurPatNum = formPatientSelect.SelectedPatNum;
                    pat = Patients.GetPat(CurPatNum);
                    if (ContrChart2.Visible)
                    {
                        ContrChart2.ModuleSelectedErx(CurPatNum);
                    }
                    else
                    {
                        RefreshCurrentModule();
                    }
                    FillPatientButton(pat);
                }
            }
            //if (!Preferences.IsODHQ)
            //{
            //    //Remove the menu items that are only needed for HQ like Default CC Procedures
            //    menuItemAccount.MenuItems.Clear();
            //}
            if (Preference.GetString(PreferenceName.LanguageAndRegion) != CultureInfo.CurrentCulture.Name && !ComputerPrefs.LocalComputer.NoShowLanguage)
            {
                if (MsgBox.Show(this, MsgBoxButtons.YesNo, "Warning, having mismatched language setting between the workstation and server may cause the program "
                    + "to behave in unexpected ways. Would you like to view the setup window?"))
                {
                    FormLanguageAndRegion FormLAR = new FormLanguageAndRegion();
                    FormLAR.ShowDialog();
                }
            }
            if (CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits != 2 //We want our users to have their currency decimal setting set to 2.
                && !ComputerPrefs.LocalComputer.NoShowDecimal)
            {
                FormDecimalSettings FormDS = new FormDecimalSettings();
                FormDS.ShowDialog();
            }
            //Choose a default DirectX format when no DirectX format has been specified and running in DirectX tooth chart mode.
            if (ComputerPrefs.LocalComputer.GraphicsSimple == DrawingMode.DirectX && ComputerPrefs.LocalComputer.DirectXFormat == "")
            {
                try
                {
                    ComputerPrefs.LocalComputer.DirectXFormat = FormGraphics.GetPreferredDirectXFormat(this);
                    if (ComputerPrefs.LocalComputer.DirectXFormat == "invalid")
                    {
                        //No valid local DirectX format could be found.
                        ComputerPrefs.LocalComputer.GraphicsSimple = DrawingMode.Simple2D;
                    }
                    ComputerPrefs.Update(ComputerPrefs.LocalComputer);
                    //Reinitialize the tooth chart because the graphics mode was probably changed which should change the tooth chart appearence.
                    ContrChart2.InitializeOnStartup();
                }
                catch (Exception)
                {
                    //The tooth chart will default to Simple2D mode if the above code fails for any reason.  This will at least get the user into the program.
                }
            }
            //Only show enterprise setup if it is enabled
            menuItemEnterprise.Visible = Preference.GetBool(PreferenceName.ShowFeatureEnterprise);
            menuItemReactivation.Visible = Preference.GetBool(PreferenceName.ShowFeatureReactivations);
            ComputerPrefs.UpdateLocalComputerOS();
            WikiPages.NavPageDelegate = S_WikiLoadPage;
            BeginCheckAlertsThread();
            //We are about to start signal processing for the first time so set the initial refresh timestamp.
            Signalods.SignalLastRefreshed = MiscData.GetNowDateTime();
            Signalods.ApptSignalLastRefreshed = Signalods.SignalLastRefreshed;
            SetTimersAndThreads(true);


            Plugin.Trigger(this, "FormOpenDental_Loaded");
        }










        MenuItem CreateMenuItem(string name, string caption, Action action)
        {
            var menuItem = CreateMenu(name, caption);

            if (action == null)
            {
                menuItem.Enabled = false;
            }
            else
            {
                menuItem.Click += (s, e) =>
                {
                    try
                    {
                        action();
                    }
                    catch { } // TODO: Add some better error handling and logging here...
                };
            }

            return menuItem;
        }

        MenuItem CreateMenu(string name, string caption)
        {
            return
                Plugin.Filter(this, "new_menu",
                    mainMenu.MenuItems.Add(caption),
                    name, 
                    mainMenu);
        }

        MenuItem CreateMenuItem(MenuItem menu, string name, string caption, Action action)
        {
            var menuItem = CreateMenuItem(menu, name, caption);

            if (action == null)
            {
                menuItem.Enabled = false;
            }
            else
            {
                menuItem.Click += (s, e) =>
                {
                    try
                    {
                        action();
                    }
                    catch { } // TODO: Add some better error handling and logging here...
                };
            }

            return menuItem;
        }

        MenuItem CreateMenuItem(MenuItem menu, string name, string caption)
        {
            return
                Plugin.Filter(this, "new_menu_item", 
                    menu.MenuItems.Add(caption), 
                    name, 
                    mainMenu, 
                    menu);
        }



        ///<summary>Creates an OpenDentalFHIRConfig.xml file if one does not exist.</summary>
        private void CreateFHIRConfigFile()
        {
            //We don't include OpenDentalFHIRConfig.xml in Setup.exe because we don't want to overwrite existing settings. We will check to see if it 
            //exists, and create it if it doesn't.
            if (!Directory.Exists(Path.Combine(Application.StartupPath, "OpenDentalFHIR"))
                || File.Exists(Path.Combine(Application.StartupPath, "OpenDentalFHIR/OpenDentalFHIRConfig.xml")))
            {
                return;
            }
            //Use default values that can be edited later if needed.
            string dbType = "MySQL";
            string connectionString = "";
            string computerName = "localhost";
            string databaseName = "opendental";
            string user = "root";
            string password = "";
            string passwordHash = "";
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(Path.Combine(Application.StartupPath, "FreeDentalConfig.xml"));
                XPathNavigator Navigator = document.CreateNavigator();
                XPathNavigator nav;
                //Database type
                nav = Navigator.SelectSingleNode("//DatabaseType");
                if (nav != null)
                {
                    dbType = nav.Value;
                }
                //See if there's a ConnectionString
                nav = Navigator.SelectSingleNode("//ConnectionString");
                if (nav != null)
                {
                    connectionString = nav.Value;
                }
                //See if there's a DatabaseConnection
                nav = Navigator.SelectSingleNode("//DatabaseConnection");
                if (nav != null)
                {
                    computerName = nav.SelectSingleNode("ComputerName").Value;
                    databaseName = nav.SelectSingleNode("Database").Value;
                    user = nav.SelectSingleNode("User").Value;
                    password = nav.SelectSingleNode("Password").Value;
                    XPathNavigator encryptedPwdNode = nav.SelectSingleNode("MySQLPassHash");
                    if (password == ""
                        && encryptedPwdNode != null
                        && encryptedPwdNode.Value != "")
                    {
                        passwordHash = encryptedPwdNode.Value;
                    }
                }
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ("    ");
                using (XmlWriter writer = XmlWriter.Create(Path.Combine(Application.StartupPath, "OpenDentalFHIR/OpenDentalFHIRConfig.xml"),
                    settings))
                {
                    writer.WriteStartElement("ConnectionSettings");
                    if (connectionString != "")
                    {
                        writer.WriteStartElement("ConnectionString");
                        writer.WriteString(connectionString);
                        writer.WriteEndElement();
                    }
                    else
                    {
                        writer.WriteStartElement("DatabaseConnection");
                        writer.WriteStartElement("ComputerName");
                        writer.WriteString(computerName);
                        writer.WriteEndElement();
                        writer.WriteStartElement("Database");
                        writer.WriteString(databaseName);
                        writer.WriteEndElement();
                        writer.WriteStartElement("User");
                        writer.WriteString(user);
                        writer.WriteEndElement();
                        writer.WriteStartElement("Password");
                        writer.WriteString(string.IsNullOrEmpty(passwordHash) ? password : "");
                        writer.WriteEndElement();
                        writer.WriteStartElement("MySQLPassHash");
                        writer.WriteString(passwordHash);
                        writer.WriteEndElement();
                        writer.WriteStartElement("UserLow");
                        writer.WriteString("");
                        writer.WriteEndElement();
                        writer.WriteStartElement("PasswordLow");
                        writer.WriteString("");
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                    writer.WriteStartElement("DatabaseType");
                    writer.WriteString(dbType);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.Flush();
                }//using writer
            }
            catch (Exception)
            {
                //Config file not created.
            }
        }

        private bool SetAdvertising(ProgramName progName, XmlDocument doc)
        {
            ProgramProperty property = ProgramProperties.GetForProgram(Programs.GetCur(progName).ProgramNum).FirstOrDefault(x => x.PropertyDesc == "Disable Advertising HQ");
            ProgramProperty propOld = null;
            XmlNode node = doc.SelectSingleNode("//" + progName.ToString());
            if (node == null)
            {
                return false;
            }
            if (property == null)
            {
                property = new ProgramProperty();
                property.PropertyDesc = "Disable Advertising HQ";
                property.ProgramNum = Programs.GetCur(progName).ProgramNum;
            }
            else
            {
                propOld = property.Copy();
            }
            //"true" from HQ == 0 for the property value.
            //"false" from HQ == 1 for the property value.
            //This is because the boolean from HQ is whether or not to show the advertisement, whereas in OD the boolean is whether or not to hide the advertisement
            bool isDisabledByHQ = !(node.InnerText.ToLower() == "true");
            property.PropertyValue = POut.Bool(isDisabledByHQ);
            if (propOld == null)
            {
                ProgramProperties.Insert(property);
                return true;
            }
            else
            {
                return ProgramProperties.Update(property, propOld);
            }
        }

        /// <summary>
        /// Loads and validates the preferences.
        /// </summary>
        bool LoadPreferences()
        {
            try
            {
                Cache.Refresh(InvalidType.Prefs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            // TODO: Check if there is a recent backup.

            try
            {
                MiscData.SetSqlMode();
            }
            catch
            {
                MessageBox.Show(
                    "Unable to set global sql mode. User probably does not have enough permission.",
                    "Open Dental",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            string updateComputerName = Preference.GetString(PreferenceName.UpdateInProgressOnComputerName);
            if (updateComputerName != "" && Environment.MachineName.ToUpper() != updateComputerName.ToUpper())
            {
                using (var formUpdateInProgress = new FormUpdateInProgress(updateComputerName))
                {
                    if (formUpdateInProgress.ShowDialog() != DialogResult.OK)
                    {
                        return false;
                    }
                }
            }

            // Check whether there is a valid registration key.
            if (!License.ValidateKey(Preference.GetString(PreferenceName.RegistrationKey)))
            {
                using (var formRegistrationKey = new FormRegistrationKey())
                {
                    if (formRegistrationKey.ShowDialog() != DialogResult.OK)
                    {
                        Environment.Exit(ExitCode);
                        return false;
                    }
                    Cache.Refresh(InvalidType.Prefs);
                }
            }

            return true;
        }

        /// <summary>
        /// Refreshes certain rarely used data from database. Must supply the types of data to refresh as flags.
        /// Also performs a few other tasks that must be done when local data is changed.
        /// </summary>
        void RefreshLocalData(params InvalidType[] arrayITypes)
        {
            RefreshLocalData(true, arrayITypes);
        }

        /// <summary>
        /// Refreshes certain rarely used data from database. Must supply the types of data to refresh as flags.
        /// Also performs a few other tasks that must be done when local data is changed.
        /// </summary>
        void RefreshLocalData(bool doRefreshServerCache, params InvalidType[] arrayITypes)
        {
            if (arrayITypes == null || arrayITypes.Length == 0)
            {
                return;//Just in case.
            }
            Cache.Refresh(doRefreshServerCache, arrayITypes);
            RefreshLocalDataPostCleanup(arrayITypes);
        }

        ///<summary>Performs a few tasks that must be done when local data is changed.</summary>
        private void RefreshLocalDataPostCleanup(params InvalidType[] arrayITypes)
        {//This is where the flickering and reset of windows happens
            bool isAll = arrayITypes.Contains(InvalidType.AllLocal);
            #region IvalidType.Prefs
            if (arrayITypes.Contains(InvalidType.Prefs) || isAll)
            {
                if (Preference.GetBool(PreferenceName.EasyHidePublicHealth))
                {
                    menuItemSchools.Visible = false;
                    menuItemCounties.Visible = false;
                    menuItemScreening.Visible = false;
                }
                else
                {
                    menuItemSchools.Visible = true;
                    menuItemCounties.Visible = true;
                    menuItemScreening.Visible = true;
                }
                if (Preference.GetBool(PreferenceName.EasyNoClinics))
                {
                    menuItemClinics.Visible = false;
                    menuClinics.Visible = false;
                }
                else
                {
                    menuItemClinics.Visible = true;
                    menuClinics.Visible = true;
                }
                //See other solution @3401 for past commented out code.
                // TODO: myOutlookBar.RefreshButtons();
                if (Preference.GetBool(PreferenceName.EasyHideDentalSchools))
                {
                    menuItemSchoolClass.Visible = false;
                    menuItemSchoolCourses.Visible = false;
                    menuItemDentalSchools.Visible = false;
                    menuItemRequirementsNeeded.Visible = false;
                    menuItemReqStudents.Visible = false;
                    menuItemEvaluations.Visible = false;
                }
                else
                {
                    menuItemSchoolClass.Visible = true;
                    menuItemSchoolCourses.Visible = true;
                    menuItemRequirementsNeeded.Visible = true;
                    menuItemReqStudents.Visible = true;
                }
                if (Preference.GetBool(PreferenceName.EasyHideRepeatCharges))
                {
                    menuItemRepeatingCharges.Visible = false;
                }
                else
                {
                    menuItemRepeatingCharges.Visible = true;
                }
                if (Preferences.HasOnlinePaymentEnabled())
                {
                    menuItemPendingPayments.Visible = true;
                    menuItemXWebTrans.Visible = true;
                }
                else
                {
                    menuItemPendingPayments.Visible = false;
                    menuItemXWebTrans.Visible = false;
                }
                //if (Preference.GetString(PrefName.DistributorKey) == "")
                //{
                //    menuItemCustomerManage.Visible = false;
                //    menuItemNewCropBilling.Visible = false;
                //}
                //else
                //{
                //    menuItemCustomerManage.Visible = true;
                //    menuItemNewCropBilling.Visible = true;
                //}
                CheckCustomReports();
                if (NeedsRedraw("ChartModule"))
                {
                    ContrChart2.InitializeLocalData();
                }
                if (NeedsRedraw("TaskLists"))
                {
                    if (Preference.GetBool(PreferenceName.TaskListAlwaysShowsAtBottom))
                    {//Refreshing task list here may not be the best course of action.
                     //separate if statement to prevent database call if not showing task list at bottom to begin with
                     //ComputerPref computerPref = ComputerPrefs.GetForLocalComputer();
                        if (ComputerPrefs.LocalComputer.TaskKeepListHidden)
                        {
                            userControlTasks1.Visible = false;
                        }
                        else if (this.WindowState != FormWindowState.Minimized)
                        {//task list show and window is not minimized.
                            userControlTasks1.Visible = true;
                            userControlTasks1.InitializeOnStartup();
                        }
                    }
                    else
                    {
                        userControlTasks1.Visible = false;
                    }
                }
                LayoutControls();
            }
            else if (arrayITypes.Contains(InvalidType.Sheets) && userControlPatientDashboard.IsInitialized)
            {
                LayoutControls();//The current dashboard may have changed.
                userControlPatientDashboard.RefreshDashboard();
                RefreshMenuDashboards();
            }
            else if (arrayITypes.Contains(InvalidType.Security) || isAll)
            {
                RefreshMenuDashboards();
            }
            #endregion
            #region InvalidType.Signals
            if (arrayITypes.Contains(InvalidType.SigMessages) || isAll)
            {
                FillSignalButtons();
            }
            #endregion
            #region InvalidType.Programs
            if (arrayITypes.Contains(InvalidType.Programs) || isAll)
            {
                if (Programs.GetCur(ProgramName.PT).Enabled)
                {
                    Bridges.PaperlessTechnology.InitializeFileWatcher();
                }
            }
            #endregion
            #region InvalidType.Programs OR InvalidType.Prefs

            // TODO: Fix this....
            //if (arrayITypes.Contains(InvalidType.Programs) || arrayITypes.Contains(InvalidType.Prefs) || isAll)
            //{
            //    if (PrefC.GetBool(PrefName.EasyBasicModules))
            //    {
            //        myOutlookBar.Buttons[3].Visible = false;
            //        myOutlookBar.Buttons[5].Visible = false;
            //        myOutlookBar.Buttons[6].Visible = false;
            //    }
            //    else
            //    {
            //        myOutlookBar.Buttons[3].Visible = true;
            //        myOutlookBar.Buttons[5].Visible = true;
            //        myOutlookBar.Buttons[6].Visible = true;
            //    }
            //    if (Programs.UsingEcwTightOrFullMode())
            //    {//has nothing to do with HL7
            //        if (ProgramProperties.GetPropVal(ProgramName.eClinicalWorks, "ShowImagesModule") == "1")
            //        {
            //            myOutlookBar.Buttons[5].Visible = true;
            //        }
            //        else
            //        {
            //            myOutlookBar.Buttons[5].Visible = false;
            //        }
            //    }
            //    if (Programs.UsingEcwTightMode())
            //    {//has nothing to do with HL7
            //        myOutlookBar.Buttons[6].Visible = false;
            //    }
            //    if (Programs.UsingEcwTightOrFullMode())
            //    {//old eCW interfaces
            //        if (Programs.UsingEcwTightMode())
            //        {
            //            myOutlookBar.Buttons[0].Visible = false;//Appt
            //            myOutlookBar.Buttons[2].Visible = false;//Account
            //        }
            //        else if (Programs.UsingEcwFullMode())
            //        {
            //            //We might create a special Appt module for eCW full users so they can access Recall.
            //            myOutlookBar.Buttons[0].Visible = false;//Appt
            //        }
            //    }
            //    else if (HL7Defs.IsExistingHL7Enabled())
            //    {//There may be a def enabled as well as the old program link enabled. In this case, do not look at the def for whether or not to show the appt and account modules, instead go by the eCW interface enabled.
            //        HL7Def def = HL7Defs.GetOneDeepEnabled();
            //        myOutlookBar.Buttons[0].Visible = def.ShowAppts;//Appt
            //        myOutlookBar.Buttons[2].Visible = def.ShowAccount;//Account
            //    }
            //    else
            //    {//no def and not using eCW tight or full program link
            //        myOutlookBar.Buttons[0].Visible = true;//Appt
            //        myOutlookBar.Buttons[2].Visible = true;//Account
            //    }
            //    if (Programs.UsingOrion)
            //    {
            //        myOutlookBar.Buttons[0].Visible = false;//Appt module
            //        myOutlookBar.Buttons[2].Visible = false;//Account module
            //        myOutlookBar.Buttons[3].Visible = false;//TP module
            //    }
            //    myOutlookBar.Invalidate();
            //}


            #endregion
            #region InvalidType.ToolBut
            if (arrayITypes.Contains(InvalidType.ToolBut) || isAll)
            {
                ContrAccount2.LayoutToolBar();
                ContrAppt2.LayoutToolBar();
                if (ContrChart2.Visible)
                {
                    //When the invalidated (running DBM) if we just layout the tool bar the buttons would be enabled, need to consider if no patient is selected.
                    //The following line calls LayoutToolBar() and then does the toolbar enable/disable logic.
                    ContrChart2.RefreshModuleScreen(false);//false because module is already selected.
                }
                else
                {
                    ContrChart2.LayoutToolBar();
                }
                ContrImages2.LayoutToolBar();
                ContrFamily2.LayoutToolBar();
            }
            #endregion
            #region InvalidType.Views
            if (arrayITypes.Contains(InvalidType.Views) || isAll)
            {
                ContrAppt2.FillViews();//Triggers ModuleSelected()
            }
            #endregion
            //TODO: If there are still issues with TP refreshing, include TP prefs in needsRedraw()
            ContrTreat2.InitializeLocalData();//easier to leave this here for now than to split it.
            dictChartPrefsCache.Clear();
            dictTaskListPrefsCache.Clear();
            //Chart Drawing Prefs
            dictChartPrefsCache.Add(PreferenceName.UseInternationalToothNumbers.ToString(), Preference.GetInt(PreferenceName.UseInternationalToothNumbers));
            dictChartPrefsCache.Add("GraphicsUseHardware", ComputerPrefs.LocalComputer.GraphicsUseHardware);
            dictChartPrefsCache.Add("PreferredPixelFormatNum", ComputerPrefs.LocalComputer.PreferredPixelFormatNum);
            dictChartPrefsCache.Add("GraphicsSimple", ComputerPrefs.LocalComputer.GraphicsSimple);
            dictChartPrefsCache.Add(PreferenceName.ShowFeatureEhr.ToString(), Preference.GetBool(PreferenceName.ShowFeatureEhr));
            dictChartPrefsCache.Add("DirectXFormat", ComputerPrefs.LocalComputer.DirectXFormat);
            //Task list drawing prefs
            dictTaskListPrefsCache.Add("TaskDock", ComputerPrefs.LocalComputer.TaskDock);
            dictTaskListPrefsCache.Add("TaskY", ComputerPrefs.LocalComputer.TaskY);
            dictTaskListPrefsCache.Add("TaskX", ComputerPrefs.LocalComputer.TaskX);
            dictTaskListPrefsCache.Add(PreferenceName.TaskListAlwaysShowsAtBottom.ToString(), Preference.GetBool(PreferenceName.TaskListAlwaysShowsAtBottom));
            dictTaskListPrefsCache.Add(PreferenceName.TasksUseRepeating.ToString(), Preference.GetBool(PreferenceName.TasksUseRepeating));
            dictTaskListPrefsCache.Add(PreferenceName.TasksNewTrackedByUser.ToString(), Preference.GetBool(PreferenceName.TasksNewTrackedByUser));
            dictTaskListPrefsCache.Add(PreferenceName.TasksShowOpenTickets.ToString(), Preference.GetBool(PreferenceName.TasksShowOpenTickets));
            dictTaskListPrefsCache.Add("TaskKeepListHidden", ComputerPrefs.LocalComputer.TaskKeepListHidden);
            if (Security.IsAuthorized(Permissions.UserQueryAdmin, true))
            {
                menuItemReportsUserQuery.Text = Lan.g(this, "User Query");
            }
            else
            {
                menuItemReportsUserQuery.Text = Lan.g(this, "Released User Queries");
            }
        }

        ///<summary>Compares preferences related to sections of the program that require redraws and returns true if a redraw is necessary, false otherwise.  If anything goes wrong with checking the status of any preference this method will return true.</summary>
        private bool NeedsRedraw(string section)
        {
            try
            {
                switch (section)
                {
                    case "ChartModule":
                        if (dictChartPrefsCache.Count == 0
                            || Preference.GetInt(PreferenceName.UseInternationalToothNumbers) != (int)dictChartPrefsCache["UseInternationalToothNumbers"]
                            || ComputerPrefs.LocalComputer.GraphicsUseHardware != (bool)dictChartPrefsCache["GraphicsUseHardware"]
                            || ComputerPrefs.LocalComputer.PreferredPixelFormatNum != (int)dictChartPrefsCache["PreferredPixelFormatNum"]
                            || ComputerPrefs.LocalComputer.GraphicsSimple != (DrawingMode)dictChartPrefsCache["GraphicsSimple"]
                            || Preference.GetBool(PreferenceName.ShowFeatureEhr) != (bool)dictChartPrefsCache["ShowFeatureEhr"]
                            || ComputerPrefs.LocalComputer.DirectXFormat != (string)dictChartPrefsCache["DirectXFormat"])
                        {
                            return true;
                        }
                        break;
                    case "TaskLists":
                        if (dictTaskListPrefsCache.Count == 0
                            || ComputerPrefs.LocalComputer.TaskDock != (int)dictTaskListPrefsCache["TaskDock"] //Checking for task list redrawing
                            || ComputerPrefs.LocalComputer.TaskY != (int)dictTaskListPrefsCache["TaskY"]
                            || ComputerPrefs.LocalComputer.TaskX != (int)dictTaskListPrefsCache["TaskX"]
                            || Preference.GetBool(PreferenceName.TaskListAlwaysShowsAtBottom) != (bool)dictTaskListPrefsCache["TaskListAlwaysShowsAtBottom"]
                            || Preference.GetBool(PreferenceName.TasksUseRepeating) != (bool)dictTaskListPrefsCache["TasksUseRepeating"]
                            || Preference.GetBool(PreferenceName.TasksNewTrackedByUser) != (bool)dictTaskListPrefsCache["TasksNewTrackedByUser"]
                            || Preference.GetBool(PreferenceName.TasksShowOpenTickets) != (bool)dictTaskListPrefsCache["TasksShowOpenTickets"]
                            || ComputerPrefs.LocalComputer.TaskKeepListHidden != (bool)dictTaskListPrefsCache["TaskKeepListHidden"])
                        {
                            return true;
                        }
                        break;
                        //case "TreatmentPlan":
                        //	//If needed implement this section
                        //	break;
                }//end switch
                return false;
            }
            catch
            {
                return true;//Should never happen.  Would most likely be caused by invalid preferences within the database.
            }
        }

        ///<summary>Sets up the custom reports list in the main menu when certain requirements are met, or disables the custom reports menu item when those same conditions are not met. This function is called during initialization, and on the event that the A to Z folder usage has changed.</summary>
        private void CheckCustomReports()
        {
            menuItemCustomReports.MenuItems.Clear();
            //Try to load custom reports, but only if using the A to Z folders.
            if (Preferences.AtoZfolderUsed == DataStorageType.LocalAtoZ)
            {
                string reportFolderName = Preference.GetString(PreferenceName.ReportFolderName);
                string reportDir = reportFolderName;
                try
                {
                    if (Directory.Exists(reportDir))
                    {
                        DirectoryInfo infoDir = new DirectoryInfo(reportDir);
                        FileInfo[] filesRdl = infoDir.GetFiles("*.rdl");
                        for (int i = 0; i < filesRdl.Length; i++)
                        {
                            string itemName = Path.GetFileNameWithoutExtension(filesRdl[i].Name);
                            menuItemCustomReports.MenuItems.Add(itemName, new System.EventHandler(this.menuItemRDLReport_Click));
                        }
                    }
                }
                catch
                {
                    MsgBox.Show(this, "Failed to retrieve custom reports.");
                }
            }
            if (menuItemCustomReports.MenuItems.Count == 0)
            {
                menuItemCustomReports.Visible = false;
            }
            else
            {
                menuItemCustomReports.Visible = true;
            }
        }

        ///<summary>Causes the toolbar to be laid out again.</summary>
        private void LayoutToolBar()
        {
            ToolBarMain.Buttons.Clear();
            ODToolBarButton button;
            button = new ODToolBarButton(Lan.g(this, "Select Patient"), Resources.IconUser, "", "Patient");
            button.Style = ODToolBarButtonStyle.DropDownButton;
            button.DropDownMenu = menuPatient;
            ToolBarMain.Buttons.Add(button);
            if (!Programs.UsingEcwTightMode())
            {//eCW tight only gets Patient Select and Popups toolbar buttons
                button = new ODToolBarButton(Lan.g(this, "Commlog"), Resources.IconLog, Lan.g(this, "New Commlog Entry"), "Commlog");
                //if (Preferences.IsODHQ)
                //{
                //    button.Style = ODToolBarButtonStyle.DropDownButton;
                //    button.DropDownMenu = menuCommlog;
                //}
                ToolBarMain.Buttons.Add(button);
                button = new ODToolBarButton(Lan.g(this, "E-mail"), Resources.IconEmail, Lan.g(this, "Send E-mail"), "Email");
                button.Style = ODToolBarButtonStyle.DropDownButton;
                button.DropDownMenu = menuEmail;
                ToolBarMain.Buttons.Add(button);
                button = new ODToolBarButton(Lan.g(this, "WebMail"), Resources.IconEmail, Lan.g(this, "Secure WebMail"), "WebMail");
                button.Enabled = true;//Always enabled.  If the patient does not have an email address, then the user will be blocked from the FormWebMailMessageEdit window.
                ToolBarMain.Buttons.Add(button);
                if (_butText == null)
                {//If laying out again (after modifying setup), we keep the button to preserve the current notification text.
                    _butText = new ODToolBarButton(Lan.g(this, "Text"), Resources.IconTextMessage, Lan.g(this, "Send Text Message"), "Text");
                    _butText.Style = ODToolBarButtonStyle.DropDownButton;
                    _butText.DropDownMenu = menuText;
                    _butText.Enabled = Programs.IsEnabled(ProgramName.CallFire) || SmsPhones.IsIntegratedTextingEnabled();
                    //The Notification text has not been set since startup.  We need an accurate starting count.
                    if (SmsPhones.IsIntegratedTextingEnabled())
                    {
                        SetSmsNotificationText();
                    }
                }
                ToolBarMain.Buttons.Add(_butText);
                button = new ODToolBarButton(Lan.g(this, "Letter"), null, Lan.g(this, "Quick Letter"), "Letter");
                button.Style = ODToolBarButtonStyle.DropDownButton;
                button.DropDownMenu = menuLetter;
                ToolBarMain.Buttons.Add(button);
                button = new ODToolBarButton(Lan.g(this, "Forms"), null, "", "Form");
                //button.Style=ODToolBarButtonStyle.DropDownButton;
                //button.DropDownMenu=menuForm;
                ToolBarMain.Buttons.Add(button);
                if (_butTask == null)
                {
                    _butTask = new ODToolBarButton(Lan.g(this, "Tasks"), Resources.IconTasks, Lan.g(this, "Open Tasks"), "Tasklist");
                    _butTask.Style = ODToolBarButtonStyle.DropDownButton;
                    _butTask.DropDownMenu = menuTask;
                }
                ToolBarMain.Buttons.Add(_butTask);
                button = new ODToolBarButton(Lan.g(this, "Label"), Resources.IconPrint, Lan.g(this, "Print Label"), "Label");
                button.Style = ODToolBarButtonStyle.DropDownButton;
                button.DropDownMenu = menuLabel;
                ToolBarMain.Buttons.Add(button);
            }
            ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this, "Popups"), null, Lan.g(this, "Edit popups for this patient"), "Popups"));
            ProgramL.LoadToolbar(ToolBarMain, ToolBarsAvail.MainToolbar);

            Plugin.Trigger(this, "FormOpenDental_LayoutToolBar", ToolBarMain);
            ToolBarMain.Invalidate();
        }

        private void menuPatient_Popup(object sender, EventArgs e)
        {
            Family fam = null;
            if (CurPatNum != 0)
            {
                fam = Patients.GetFamily(CurPatNum);
            }
            //Always refresh the patient menu to reflect any patient status changes.
            PatientL.AddFamilyToMenu(menuPatient, new EventHandler(menuPatient_Click), CurPatNum, fam);
        }

        private void ToolBarMain_ButtonClick(object sender, ODToolBarButtonClickEventArgs e)
        {
            if (e.Button.Tag.GetType() == typeof(string))
            {
                //standard predefined button
                switch (e.Button.Tag.ToString())
                {
                    case "Patient":
                        OnPatient_Click();
                        break;
                    case "Commlog":
                        OnCommlog_Click();
                        break;
                    case "Email":
                        OnEmail_Click();
                        break;
                    case "WebMail":
                        OnWebMail_Click();
                        break;
                    case "Text":
                        OnTxtMsg_Click(CurPatNum);
                        break;
                    case "Letter":
                        OnLetter_Click();
                        break;
                    case "Form":
                        OnForm_Click();
                        break;
                    case "Tasklist":
                        OnTasks_Click();
                        break;
                    case "Label":
                        OnLabel_Click();
                        break;
                    case "Popups":
                        OnPopups_Click();
                        break;
                }
            }
            else if (e.Button.Tag.GetType() == typeof(Program))
            {
                ProgramL.Execute(((Program)e.Button.Tag).ProgramNum, Patients.GetPat(CurPatNum));
            }
        }

        private void OnPatient_Click()
        {
            FormPatientSelect formPatientSelect = new FormPatientSelect();
            formPatientSelect.ShowDialog();
            if (formPatientSelect.DialogResult == DialogResult.OK)
            {
                CurPatNum = formPatientSelect.SelectedPatNum;
                Patient pat = Patients.GetPat(CurPatNum);
                if (ContrChart2.Visible)
                {
                    userControlTasks1.RefreshPatTicketsIfNeeded();//This is a special case.  Normally it's called in RefreshCurrentModule()
                    ContrChart2.ModuleSelectedErx(CurPatNum);
                }
                else
                {
                    RefreshCurrentModule();
                }
                FillPatientButton(pat);
                Plugin.Trigger(this, "FormOpenDental_PatientClicked", pat);
            }
        }

        private void menuPatient_Click(object sender, System.EventArgs e)
        {
            Family fam = Patients.GetFamily(CurPatNum);
            CurPatNum = PatientL.ButtonSelect(menuPatient, sender, fam);
            //new family now
            Patient pat = Patients.GetPat(CurPatNum);
            RefreshCurrentModule();
            FillPatientButton(pat);
        }

        ///<summary>If the call to this is followed by ModuleSelected or GotoModule, set isRefreshCurModule=false to prevent the module from being
        ///refreshed twice.  If the current module is ContrAppt and the call to this is preceded by a call to RefreshModuleDataPatient, set
        ///isApptRefreshDataPat=false so the query to get the patient does not run twice.</summary>
        public static void S_Contr_PatientSelected(Patient pat, bool isRefreshCurModule, bool isApptRefreshDataPat = true, bool hasForcedRefresh = false)
        {
            _formOpenDentalS.Contr_PatientSelected(pat, isRefreshCurModule, isApptRefreshDataPat, hasForcedRefresh);
        }

        ///<summary>Happens when any of the modules changes the current patient or when this main form changes the patient.  The calling module should
        ///refresh itself.  The current patNum is stored here in the parent form so that when switching modules, the parent form knows which patient to
        ///call up for that module.</summary>
        private void Contr_PatientSelected(Patient pat, bool isRefreshCurModule, bool isApptRefreshDataPat, bool hasForcedRefresh)
        {
            CurPatNum = pat.PatNum;
            if (isRefreshCurModule)
            {
                RefreshCurrentModule(hasForcedRefresh, isApptRefreshDataPat);
            }
            userControlTasks1.RefreshPatTicketsIfNeeded();
            FillPatientButton(pat);
        }

        ///<Summary>Serves four functions.  
        ///1. Sends the new patient to the dropdown menu for select patient.  
        ///2. Changes which toolbar buttons are enabled.  
        ///3. Sets main form text. 
        ///4. Displays any popup.</Summary>
        private void FillPatientButton(Patient pat)
        {
            if (pat == null)
            {
                pat = new Patient();
            }
            Text = PatientL.GetMainTitle(pat, Clinics.ClinicNum);
            bool patChanged = PatientL.AddPatsToMenu(menuPatient, new EventHandler(menuPatient_Click), pat.GetNameLF(), pat.PatNum);
            if (patChanged)
            {
                if (AutomationL.Trigger(AutomationTrigger.OpenPatient, null, pat.PatNum))
                {//if a trigger happened
                    if (ContrAppt2.Visible)
                    {
                        ContrAppt2.MouseUpForced();
                    }
                }
            }
            if (ToolBarMain.Buttons == null || ToolBarMain.Buttons.Count < 2)
            {//on startup.  js Not sure why it's checking count.
                return;
            }
            if (CurPatNum == 0)
            {//Only on startup, I think.
                if (!Programs.UsingEcwTightMode())
                {//eCW tight only gets Patient Select and Popups toolbar buttons
                 //We need a drafts folder the user can view saved emails in before we allow the user to save email without a patient selected.
                    ToolBarMain.Buttons["Email"].Enabled = false;
                    ToolBarMain.Buttons["WebMail"].Enabled = false;
                    ToolBarMain.Buttons["Commlog"].Enabled = false;
                    ToolBarMain.Buttons["Letter"].Enabled = false;
                    ToolBarMain.Buttons["Form"].Enabled = false;
                    ToolBarMain.Buttons["Tasklist"].Enabled = true;
                    ToolBarMain.Buttons["Label"].Enabled = false;
                }
                ToolBarMain.Buttons["Popups"].Enabled = false;
            }
            else
            {
                if (!Programs.UsingEcwTightMode())
                {//eCW tight only gets Patient Select and Popups toolbar buttons
                    ToolBarMain.Buttons["Commlog"].Enabled = true;
                    ToolBarMain.Buttons["Email"].Enabled = true;
                    if (_butText != null)
                    {
                        _butText.Enabled = Programs.IsEnabled(ProgramName.CallFire) || SmsPhones.IsIntegratedTextingEnabled();
                    }
                    ToolBarMain.Buttons["WebMail"].Enabled = true;
                    ToolBarMain.Buttons["Letter"].Enabled = true;
                    ToolBarMain.Buttons["Form"].Enabled = true;
                    ToolBarMain.Buttons["Tasklist"].Enabled = true;
                    ToolBarMain.Buttons["Label"].Enabled = true;
                }
                ToolBarMain.Buttons["Popups"].Enabled = true;
            }
            ToolBarMain.Invalidate();

            if (PopupEventList == null)
            {
                PopupEventList = new List<PopupEvent>();
            }

            PopupEventList = Plugin.Filter(this, "FormOpenDental_FillPatientButtonPopups", PopupEventList, pat, patChanged);
            if (PopupEventList.Count == 0)
            {
                return;
            }

            if (!patChanged)
            {
                return;
            }
            if (ContrChart2.Visible)
            {
                TryNonPatientPopup();
            }
            //New patient selected.  Everything below here is for popups.
            //First, remove all expired popups from the event list.
            for (int i = PopupEventList.Count - 1; i >= 0; i--)
            {//go backwards
                if (PopupEventList[i].DisableUntil < DateTime.Now)
                {//expired
                    PopupEventList.RemoveAt(i);
                }
            }
            //Now, loop through all popups for the patient.
            List<Popup> popList = Popups.GetForPatient(pat);//get all possible 
            for (int i = 0; i < popList.Count; i++)
            {
                //skip any popups that are disabled because they are on the event list
                bool popupIsDisabled = false;
                for (int e = 0; e < PopupEventList.Count; e++)
                {
                    if (popList[i].PopupNum == PopupEventList[e].PopupNum)
                    {
                        popupIsDisabled = true;
                        break;
                    }
                }
                if (popupIsDisabled)
                {
                    continue;
                }
                //This popup is not disabled, so show it.
                //A future improvement would be to assemble all the popups that are to be shown and then show them all in one large window.
                //But for now, they will show in sequence.
                if (ContrAppt2.Visible)
                {
                    ContrAppt2.MouseUpForced();
                }
                FormPopupDisplay FormP = new FormPopupDisplay();
                FormP.PopupCur = popList[i];
                FormP.ShowDialog();
                if (FormP.MinutesDisabled > 0)
                {
                    PopupEvent popevent = new PopupEvent();
                    popevent.PopupNum = popList[i].PopupNum;
                    popevent.DisableUntil = DateTime.Now + TimeSpan.FromMinutes(FormP.MinutesDisabled);
                    popevent.LastViewed = DateTime.Now;
                    PopupEventList.Add(popevent);
                    PopupEventList.Sort();
                }
            }
        }

        private void OnEmail_Click()
        {
            if (CurPatNum == 0)
            {
                MsgBox.Show(this, "Please select a patient to send an email.");
                return;
            }
            if (!Security.IsAuthorized(Permissions.EmailSend))
            {
                return;
            }
            EmailMessage message = new EmailMessage();
            message.PatientId = CurPatNum;
            Patient pat = Patients.GetPat(CurPatNum);
            message.ToAddress = pat.Email;
            EmailAddress selectedAddress = EmailAddress.GetDefault(Security.CurUser.UserNum, pat.ClinicNum);
            message.FromAddress = selectedAddress.GetFrom();
            FormEmailMessageEdit FormE = new FormEmailMessageEdit(message, selectedAddress);
            FormE.ShowDialog();
            if (FormE.DialogResult == DialogResult.OK)
            {
                RefreshCurrentModule();
            }
        }

        private void menuEmail_Popup(object sender, EventArgs e)
        {
            menuEmail.MenuItems.Clear();
            MenuItem menuItem;
            menuItem = new MenuItem(Lan.g(this, "Referrals:"));
            menuItem.Tag = null;
            menuEmail.MenuItems.Add(menuItem);
            List<RefAttach> refAttaches = RefAttaches.Refresh(CurPatNum);
            string referralDescript = DisplayFields.GetForCategory(DisplayFieldCategory.PatientInformation)
                .FirstOrDefault(x => x.InternalName == "Referrals")?.Description;
            if (string.IsNullOrWhiteSpace(referralDescript))
            {//either not displaying the Referral field or no description entered, default to 'Referral'
                referralDescript = Lan.g(this, "Referral");
            }
            Referral refer;
            string str;
            for (int i = 0; i < refAttaches.Count; i++)
            {
                if (!Referrals.TryGetReferral(refAttaches[i].ReferralNum, out refer))
                {
                    continue;
                }
                if (refAttaches[i].RefType == ReferralType.RefFrom)
                {
                    str = Lan.g(this, "From");
                }
                else if (refAttaches[i].RefType == ReferralType.RefTo)
                {
                    str = Lan.g(this, "To");
                }
                else
                {
                    str = referralDescript;
                }
                str += " " + Referrals.GetNameFL(refer.ReferralNum) + " <";
                if (refer.EMail == "")
                {
                    str += Lan.g(this, "no email");
                }
                else
                {
                    str += refer.EMail;
                }
                str += ">";
                menuItem = new MenuItem(str, menuEmail_Click);
                menuItem.Tag = refer;
                menuEmail.MenuItems.Add(menuItem);
            }
        }

        private void OnWebMail_Click()
        {
            //if (!Security.IsAuthorized(Permissions.WebMailSend))
            //{
            //    return;
            //}
            //FormWebMailMessageEdit FormWMME = new FormWebMailMessageEdit(CurPatNum);
            //FormWMME.ShowDialog();
        }

        private void menuEmail_Click(object sender, System.EventArgs e)
        {
            if (((MenuItem)sender).Tag == null)
            {
                return;
            }
            LabelSingle label = new LabelSingle();
            if (((MenuItem)sender).Tag.GetType() == typeof(Referral))
            {
                Referral refer = (Referral)((MenuItem)sender).Tag;
                if (refer.EMail == "")
                {
                    return;
                    //MsgBox.Show(this,"");
                }
                EmailMessage message = new EmailMessage();
                message.PatientId = CurPatNum;
                Patient pat = Patients.GetPat(CurPatNum);
                message.ToAddress = refer.EMail;//pat.Email;
                EmailAddress address = EmailAddress.GetByClinic(pat.ClinicNum);
                message.FromAddress = address.GetFrom();
                message.Subject = Lan.g(this, "RE: ") + pat.GetNameFL();
                FormEmailMessageEdit FormE = new FormEmailMessageEdit(message, address);
                FormE.ShowDialog();
                if (FormE.DialogResult == DialogResult.OK)
                {
                    RefreshCurrentModule();
                }
            }
        }

        private void OnCommlog_Click()
        {
            if (Plugin.Trigger(this, "FormOpenDental_Button_Commlog", CurPatNum)) return;

            using (var formCommItem = new FormCommItem(GetNewCommlog()))
            {
                formCommItem.IsNew = true;
                if (formCommItem.ShowDialog() == DialogResult.OK)
                {
                    RefreshCurrentModule();
                }
            }
        }

        private void menuItemCommlogPersistent_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.CommlogPersistent))
            {
                return;
            }
            FormCommItem FormCI = Application.OpenForms.OfType<FormCommItem>().FirstOrDefault(x => !x.IsDisposed);
            if (FormCI == null)
            {
                FormCI = new FormCommItem(GetNewCommlog());
                FormCI.IsPersistent = true;
            }
            if (FormCI.WindowState == FormWindowState.Minimized)
            {
                FormCI.WindowState = FormWindowState.Normal;
            }
            FormCI.Show();
            FormCI.BringToFront();
        }

        /// <summary>
        /// This is a helper method to get a new commlog object for the commlog tool bar buttons.
        /// </summary>
        Commlog GetNewCommlog()
        {
            return new Commlog
            {
                PatNum = CurPatNum,
                CommDateTime = DateTime.Now,
                CommType = Commlogs.GetTypeAuto(CommItemTypeAuto.MISC),
                Mode_ = CommItemMode.Phone,
                SentOrReceived = CommSentOrReceived.Received,
                UserNum = Security.CurUser.UserNum
            };
        }

        private void OnLetter_Click()
        {
            FormSheetPicker FormS = new FormSheetPicker();
            FormS.SheetType = SheetTypeEnum.PatientLetter;
            FormS.ShowDialog();
            if (FormS.DialogResult != DialogResult.OK)
            {
                return;
            }
            SheetDef sheetDef = FormS.SelectedSheetDefs[0];
            Sheet sheet = SheetUtil.CreateSheet(sheetDef, CurPatNum);
            SheetParameter.SetParameter(sheet, "PatNum", CurPatNum);
            //SheetParameter.SetParameter(sheet,"ReferralNum",referral.ReferralNum);
            SheetFiller.FillFields(sheet);
            SheetUtil.CalculateHeights(sheet);
            FormSheetFillEdit.ShowForm(sheet, FormSheetFillEdit_FormClosing);
            //Patient pat=Patients.GetPat(CurPatNum);
            //FormLetters FormL=new FormLetters(pat);
            //FormL.ShowDialog();
        }

        private void menuLetter_Popup(object sender, EventArgs e)
        {
            menuLetter.MenuItems.Clear();
            MenuItem menuItem;
            menuItem = new MenuItem(Lan.g(this, "Merge"), menuLetter_Click);
            menuItem.Tag = "Merge";
            menuLetter.MenuItems.Add(menuItem);
            //menuItem=new MenuItem(Lan.g(this,"Stationery"),menuLetter_Click);
            //menuItem.Tag="Stationery";
            //menuLetter.MenuItems.Add(menuItem);
            menuLetter.MenuItems.Add("-");
            //Referrals---------------------------------------------------------------------------------------
            menuItem = new MenuItem(Lan.g(this, "Referrals:"));
            menuItem.Tag = null;
            menuLetter.MenuItems.Add(menuItem);
            string referralDescript = DisplayFields.GetForCategory(DisplayFieldCategory.PatientInformation)
                .FirstOrDefault(x => x.InternalName == "Referrals")?.Description;
            if (string.IsNullOrWhiteSpace(referralDescript))
            {//either not displaying the Referral field or no description entered, default to 'Referral'
                referralDescript = Lan.g(this, "Referral");
            }
            List<RefAttach> refAttaches = RefAttaches.Refresh(CurPatNum);
            Referral refer;
            string str;
            for (int i = 0; i < refAttaches.Count; i++)
            {
                if (!Referrals.TryGetReferral(refAttaches[i].ReferralNum, out refer))
                {
                    continue;
                }
                if (refAttaches[i].RefType == ReferralType.RefFrom)
                {
                    str = Lan.g(this, "From");
                }
                else if (refAttaches[i].RefType == ReferralType.RefTo)
                {
                    str = Lan.g(this, "To");
                }
                else
                {
                    str = referralDescript;
                }
                str += " " + Referrals.GetNameFL(refer.ReferralNum);
                menuItem = new MenuItem(str, menuLetter_Click);
                menuItem.Tag = refer;
                menuLetter.MenuItems.Add(menuItem);
            }
        }

        private void menuLetter_Click(object sender, System.EventArgs e)
        {
            if (((MenuItem)sender).Tag == null)
            {
                return;
            }
            Patient pat = Patients.GetPat(CurPatNum);
            if (((MenuItem)sender).Tag.GetType() == typeof(string))
            {
                if (((MenuItem)sender).Tag.ToString() == "Merge")
                {
                    FormLetterMerges FormL = new FormLetterMerges(pat);
                    FormL.ShowDialog();
                }
                //if(((MenuItem)sender).Tag.ToString()=="Stationery") {
                //	FormCommunications.PrintStationery(pat);
                //}
            }
            if (((MenuItem)sender).Tag.GetType() == typeof(Referral))
            {
                Referral refer = (Referral)((MenuItem)sender).Tag;
                FormSheetPicker FormS = new FormSheetPicker();
                FormS.SheetType = SheetTypeEnum.ReferralLetter;
                FormS.ShowDialog();
                if (FormS.DialogResult != DialogResult.OK)
                {
                    return;
                }
                SheetDef sheetDef = FormS.SelectedSheetDefs[0];
                Sheet sheet = SheetUtil.CreateSheet(sheetDef, CurPatNum);
                SheetParameter.SetParameter(sheet, "PatNum", CurPatNum);
                SheetParameter.SetParameter(sheet, "ReferralNum", refer.ReferralNum);
                //Don't fill these params if the sheet doesn't use them.
                if (sheetDef.SheetFieldDefs.Any(x =>
                     (x.FieldType == SheetFieldType.Grid && x.FieldName == "ReferralLetterProceduresCompleted")
                     || (x.FieldType == SheetFieldType.Special && x.FieldName == "toothChart")))
                {
                    List<Procedure> listProcs = Procedures.GetCompletedForDateRange(sheet.DateTimeSheet, sheet.DateTimeSheet
                        , listPatNums: new List<long>() { CurPatNum }
                        , includeNote: true
                        , includeGroupNote: true);
                    if (sheetDef.SheetFieldDefs.Any(x => x.FieldType == SheetFieldType.Grid && x.FieldName == "ReferralLetterProceduresCompleted"))
                    {
                        SheetParameter.SetParameter(sheet, "CompletedProcs", listProcs);
                    }
                    if (sheetDef.SheetFieldDefs.Any(x => x.FieldType == SheetFieldType.Special && x.FieldName == "toothChart"))
                    {
                        SheetParameter.SetParameter(sheet, "toothChartImg", SheetPrinting.GetToothChartHelper(CurPatNum, false, listProceduresFilteredOverride: listProcs));
                    }
                }
                SheetFiller.FillFields(sheet);
                SheetUtil.CalculateHeights(sheet);
                FormSheetFillEdit.ShowForm(sheet, FormSheetFillEdit_FormClosing);
                //FormLetters FormL=new FormLetters(pat);
                //FormL.ReferralCur=refer;
                //FormL.ShowDialog();
            }
        }

        /// <summary>Event handler for closing FormSheetFillEdit when it is non-modal.</summary>
        private void FormSheetFillEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (((FormSheetFillEdit)sender).DialogResult == DialogResult.OK || ((FormSheetFillEdit)sender).DidChangeSheet)
            {
                RefreshCurrentModule();
            }
        }

        private void OnForm_Click()
        {
            FormPatientForms formP = new FormPatientForms();
            formP.PatNum = CurPatNum;
            formP.ShowDialog();
            //always refresh, especially to get the titlebar right after an import.
            Patient pat = Patients.GetPat(CurPatNum);
            RefreshCurrentModule(docNum: formP.DocNum);
            FillPatientButton(pat);
        }

        private void OnTasks_Click()
        {
            FormTaskListSelect FormT = new FormTaskListSelect(TaskObjectType.Patient);
            FormT.Location = new Point(50, 50);
            FormT.Text = Lan.g(FormT, "Add Task") + " - " + FormT.Text;
            FormT.ShowDialog();
            if (FormT.DialogResult != DialogResult.OK)
            {
                return;
            }
            Task task = new Task();
            task.TaskListNum = -1;//don't show it in any list yet.
            Tasks.Insert(task);
            Task taskOld = task.Copy();
            task.KeyNum = CurPatNum;
            task.ObjectType = TaskObjectType.Patient;
            task.TaskListNum = FormT.ListSelectedLists[0];
            task.UserNum = Security.CurUser.UserNum;
            FormTaskEdit FormTE = new FormTaskEdit(task, taskOld);
            FormTE.IsNew = true;
            FormTE.Show();
        }

        private void menuTask_Popup(object sender, EventArgs e)
        {
            menuItemTaskNewForUser.Text = Lan.g(this, "for") + " " + Security.CurUser.UserName;
            menuItemTaskReminders.Text = Lan.g(this, "Reminders");
            int reminderTaskNewCount = GetNewReminderTaskCount();
            if (reminderTaskNewCount > 0)
            {
                menuItemTaskReminders.Text += " (" + reminderTaskNewCount + ")";
            }
            int otherTaskCount = (_listNormalTaskNums != null) ? _listNormalTaskNums.Count : 0;
            if (otherTaskCount > 0)
            {
                menuItemTaskNewForUser.Text += " (" + otherTaskCount + ")";
            }
        }

        private void RefreshTasksNotification()
        {
            if (_butTask == null)
            {
                return;
            }

            int otherTaskCount = (_listNormalTaskNums != null) ? _listNormalTaskNums.Count : 0;
            int totalTaskCount = GetNewReminderTaskCount() + otherTaskCount;
            string notificationText = "";
            if (totalTaskCount > 0)
            {
                notificationText = Math.Min(totalTaskCount, 99).ToString();
            }
            if (notificationText != _butTask.NotificationText)
            {
                _butTask.NotificationText = notificationText;
                ToolBarMain.Invalidate(_butTask.Bounds);//Cause the notification text on the Task button to update as soon as possible.
            }
        }

        private int GetNewReminderTaskCount()
        {
            if (_listReminderTasks == null)
            {
                return 0;
            }
            //Mimics how checkNew is set in FormTaskEdit.
            if (Preference.GetBool(PreferenceName.TasksNewTrackedByUser))
            {//Per definition of task.IsUnread.
                return _listReminderTasks.FindAll(x => x.IsUnread && x.DateTimeEntry <= DateTime.Now).Count;
            }
            return _listReminderTasks.FindAll(x => x.TaskStatus == TaskStatusEnum.New && x.DateTimeEntry <= DateTime.Now).Count;
        }

        List<Module> modules = new List<Module>();

        public bool Navigate(string target, params object[] args)
        {
            if (target == null) return false;

            target = target.Trim().ToLower();
            if (target.Length == 0)
            {
                return false;
            }

            foreach (var module in modules)
            {
                if (module.Navigate(target, args))
                {
                    return true;
                }
            }

            return false;
        }

        void menuItemTaskNewForUser_Click(object sender, EventArgs e)
        {
            Navigate(NavigationTargets.Tasks, false, UserControlTasksTab.ForUser);
        }

        void menuItemTaskReminders_Click(object sender, EventArgs e)
        {
            Navigate(NavigationTargets.Tasks, false, UserControlTasksTab.Reminders);
        }



        private delegate void ToolBarMainClick(long patNum);

        private void OnLabel_Click()
        {
            //The reason we are using a delegate and BeginInvoke() is because of a Microsoft bug that causes the Print Dialog window to not be in focus			
            //when it comes from a toolbar click.
            //https://social.msdn.microsoft.com/Forums/windows/en-US/681a50b4-4ae3-407a-a747-87fb3eb427fd/first-mouse-click-after-showdialog-hits-the-parent-form?forum=winforms
            ToolBarMainClick toolClick = LabelSingle.PrintPat;
            this.BeginInvoke(toolClick, CurPatNum);
        }

        private void menuLabel_Popup(object sender, EventArgs e)
        {
            menuLabel.MenuItems.Clear();
            MenuItem menuItem;
            List<SheetDef> LabelList = SheetDefs.GetCustomForType(SheetTypeEnum.LabelPatient);
            if (LabelList.Count == 0)
            {
                menuItem = new MenuItem(Lan.g(this, "LName, FName, Address"), menuLabel_Click);
                menuItem.Tag = "PatientLFAddress";
                menuLabel.MenuItems.Add(menuItem);
                menuItem = new MenuItem(Lan.g(this, "Name, ChartNumber"), menuLabel_Click);
                menuItem.Tag = "PatientLFChartNumber";
                menuLabel.MenuItems.Add(menuItem);
                menuItem = new MenuItem(Lan.g(this, "Name, PatNum"), menuLabel_Click);
                menuItem.Tag = "PatientLFPatNum";
                menuLabel.MenuItems.Add(menuItem);
                menuItem = new MenuItem(Lan.g(this, "Radiograph"), menuLabel_Click);
                menuItem.Tag = "PatRadiograph";
                menuLabel.MenuItems.Add(menuItem);
            }
            else
            {
                for (int i = 0; i < LabelList.Count; i++)
                {
                    menuItem = new MenuItem(LabelList[i].Description, menuLabel_Click);
                    menuItem.Tag = LabelList[i];
                    menuLabel.MenuItems.Add(menuItem);
                }
            }
            menuLabel.MenuItems.Add("-");
            //Carriers---------------------------------------------------------------------------------------
            Family fam = Patients.GetFamily(CurPatNum);
            //Received multiple bug submissions where CurPatNum==0, even though this toolbar button should not be enabled when no patient is selected.
            if (fam.ListPats != null && fam.ListPats.Length > 0)
            {
                List<PatPlan> PatPlanList = PatPlans.Refresh(CurPatNum);
                List<InsSub> subList = InsSubs.RefreshForFam(fam);
                List<InsPlan> PlanList = InsPlans.RefreshForSubList(subList);
                Carrier carrier;
                InsPlan plan;
                InsSub sub;
                for (int i = 0; i < PatPlanList.Count; i++)
                {
                    sub = InsSubs.GetSub(PatPlanList[i].InsSubNum, subList);
                    plan = InsPlans.GetPlan(sub.PlanNum, PlanList);
                    carrier = Carriers.GetCarrier(plan.CarrierNum);
                    menuItem = new MenuItem(carrier.CarrierName, menuLabel_Click);
                    menuItem.Tag = carrier;
                    menuLabel.MenuItems.Add(menuItem);
                }
                menuLabel.MenuItems.Add("-");
            }
            //Referrals---------------------------------------------------------------------------------------
            menuItem = new MenuItem(Lan.g(this, "Referrals:"));
            menuItem.Tag = null;
            menuLabel.MenuItems.Add(menuItem);
            string referralDescript = DisplayFields.GetForCategory(DisplayFieldCategory.PatientInformation)
                .FirstOrDefault(x => x.InternalName == "Referrals")?.Description;
            if (string.IsNullOrWhiteSpace(referralDescript))
            {//either not displaying the Referral field or no description entered, default to 'Referral'
                referralDescript = Lan.g(this, "Referral");
            }
            List<RefAttach> refAttaches = RefAttaches.Refresh(CurPatNum);
            Referral refer;
            string str;
            for (int i = 0; i < refAttaches.Count; i++)
            {
                if (!Referrals.TryGetReferral(refAttaches[i].ReferralNum, out refer))
                {
                    continue;
                }
                if (refAttaches[i].RefType == ReferralType.RefFrom)
                {
                    str = Lan.g(this, "From");
                }
                else if (refAttaches[i].RefType == ReferralType.RefTo)
                {
                    str = Lan.g(this, "To");
                }
                else
                {
                    str = referralDescript;
                }
                str += " " + Referrals.GetNameFL(refer.ReferralNum);
                menuItem = new MenuItem(str, menuLabel_Click);
                menuItem.Tag = refer;
                menuLabel.MenuItems.Add(menuItem);
            }
        }

        private void menuLabel_Click(object sender, System.EventArgs e)
        {
            if (((MenuItem)sender).Tag == null)
            {
                return;
            }
            //LabelSingle label=new LabelSingle();
            if (((MenuItem)sender).Tag.GetType() == typeof(string))
            {
                if (((MenuItem)sender).Tag.ToString() == "PatientLFAddress")
                {
                    LabelSingle.PrintPatientLFAddress(CurPatNum);
                }
                if (((MenuItem)sender).Tag.ToString() == "PatientLFChartNumber")
                {
                    LabelSingle.PrintPatientLFChartNumber(CurPatNum);
                }
                if (((MenuItem)sender).Tag.ToString() == "PatientLFPatNum")
                {
                    LabelSingle.PrintPatientLFPatNum(CurPatNum);
                }
                if (((MenuItem)sender).Tag.ToString() == "PatRadiograph")
                {
                    LabelSingle.PrintPatRadiograph(CurPatNum);
                }
            }
            else if (((MenuItem)sender).Tag.GetType() == typeof(SheetDef))
            {
                LabelSingle.PrintCustomPatient(CurPatNum, (SheetDef)((MenuItem)sender).Tag);
            }
            else if (((MenuItem)sender).Tag.GetType() == typeof(Carrier))
            {
                Carrier carrier = (Carrier)((MenuItem)sender).Tag;
                LabelSingle.PrintCarrier(carrier.CarrierNum);
            }
            else if (((MenuItem)sender).Tag.GetType() == typeof(Referral))
            {
                Referral refer = (Referral)((MenuItem)sender).Tag;
                LabelSingle.PrintReferral(refer.ReferralNum);
            }
        }

        private void OnPopups_Click()
        {
            FormPopupsForFam FormPFF = new FormPopupsForFam(PopupEventList);
            FormPFF.PatCur = Patients.GetPat(CurPatNum);
            FormPFF.ShowDialog();
        }

        #region SMS Text Messaging

        ///<summary>Returns true if the message was sent successfully.</summary>
        public static bool S_OnTxtMsg_Click(long patNum, string startingText = "")
        {
            return _formOpenDentalS.OnTxtMsg_Click(patNum, startingText);
        }

        ///<summary>Called from the text message button and the right click context menu for an appointment. Returns true if the message was sent
        ///successfully.</summary>
        private bool OnTxtMsg_Click(long patNum, string startingText = "")
        {
            if (patNum == 0)
            {
                FormTxtMsgEdit FormTxtME = new FormTxtMsgEdit();
                FormTxtME.Message = startingText;
                FormTxtME.PatNum = 0;
                FormTxtME.ShowDialog();
                if (FormTxtME.DialogResult == DialogResult.OK)
                {
                    RefreshCurrentModule();
                    return true;
                }
                return false;
            }
            Patient pat = Patients.GetPat(patNum);
            bool updateTextYN = false;
            if (pat.TxtMsgOk == YN.No)
            {
                if (MsgBox.Show(this, MsgBoxButtons.YesNo, "This patient is marked to not receive text messages. "
                    + "Would you like to mark this patient as okay to receive text messages?"))
                {
                    updateTextYN = true;
                }
                else
                {
                    return false;
                }
            }
            if (pat.TxtMsgOk == YN.Unknown && Preference.GetBool(PreferenceName.TextMsgOkStatusTreatAsNo))
            {
                if (MsgBox.Show(this, MsgBoxButtons.YesNo, "This patient might not want to receive text messages. "
                    + "Would you like to mark this patient as okay to receive text messages?"))
                {
                    updateTextYN = true;
                }
                else
                {
                    return false;
                }
            }
            if (updateTextYN)
            {
                Patient patOld = pat.Copy();
                pat.TxtMsgOk = YN.Yes;
                Patients.Update(pat, patOld);
            }
            FormTxtMsgEdit FormTME = new FormTxtMsgEdit();
            FormTME.Message = startingText;
            FormTME.PatNum = patNum;
            FormTME.WirelessPhone = pat.WirelessPhone;
            FormTME.TxtMsgOk = pat.TxtMsgOk;
            FormTME.ShowDialog();
            if (FormTME.DialogResult == DialogResult.OK)
            {
                RefreshCurrentModule();
                return true;
            }
            return false;
        }

        private void menuItemTextMessagesReceived_Click(object sender, EventArgs e)
        {
            ShowFormTextMessagingModeless(false, true);
        }

        private void menuItemTextMessagesSent_Click(object sender, EventArgs e)
        {
            ShowFormTextMessagingModeless(true, false);
        }

        private void menuItemTextMessagesAll_Click(object sender, EventArgs e)
        {
            ShowFormTextMessagingModeless(true, true);
        }

        private void ShowFormTextMessagingModeless(bool isSent, bool isReceived)
        {
            if (_formSmsTextMessaging == null || _formSmsTextMessaging.IsDisposed)
            {
                _formSmsTextMessaging = new FormSmsTextMessaging(isSent, isReceived, () => { SetSmsNotificationText(); });
                _formSmsTextMessaging.FormClosed += new FormClosedEventHandler((o, e) => { _formSmsTextMessaging = null; });
            }
            _formSmsTextMessaging.Show();
            _formSmsTextMessaging.BringToFront();
        }

        ///<summary>Set signalSmsCount to null if you want to query the db for the current value and send a signal.
        ///If responding to a signal then the structured data will be parsed from signalSmsCount.MsgValue and not new signal will be generated.</summary>
        private void SetSmsNotificationText(Signalod signalSmsCount = null)
        {
            if (_butText == null)
            {
                return;//This button does not exist in eCW tight integration mode.
            }
            try
            {
                if (!_butText.Enabled)
                {
                    return;//This button is disabled when neither of the Text Messaging bridges have been enabled.
                }
                List<SmsFromMobiles.SmsNotification> listNotifications = null;
                if (signalSmsCount != null)
                { //Try to pull structured data out of the signal directly. We will get null back if this fails.
                    listNotifications = SmsFromMobiles.SmsNotification.GetListFromJson(signalSmsCount.MsgValue);
                }
                if (listNotifications == null)
                { //Notification not provided or signal was malformed. Either way recalculate and post a new signal.
                    listNotifications = SmsFromMobiles.UpdateSmsNotification();
                }
                int smsUnreadCount = 0;
                if (!Preferences.HasClinicsEnabled || Clinics.ClinicNum == 0)
                {
                    //No clinics or HQ clinic is active so sum them all.
                    smsUnreadCount = listNotifications.Sum(x => x.Count);
                }
                else
                {
                    //Only count the active clinic.
                    smsUnreadCount = listNotifications.Where(x => x.ClinicNum == Clinics.ClinicNum).Sum(x => x.Count);
                }
                //Default to empty so we show nothing if there aren't any notifications.
                string smsNotificationText = "";
                if (smsUnreadCount > 99)
                { //We only have room in the UI for a 2-digit number.
                    smsNotificationText = "99";
                }
                else if (smsUnreadCount > 0)
                { //We have a "real" number so show it.
                    smsNotificationText = smsUnreadCount.ToString();
                }
                if (_butText.NotificationText == smsNotificationText)
                { //Prevent the toolbar from being invalidated unnecessarily.
                    return;
                }
                _butText.NotificationText = smsNotificationText;
                if (menuItemTextMessagesReceived.Text.Contains("("))
                {//Remove the old count from the menu item.
                    menuItemTextMessagesReceived.Text = menuItemTextMessagesReceived.Text.Substring(0, menuItemTextMessagesReceived.Text.IndexOf("(") - 1);
                }
                if (smsNotificationText != "")
                {
                    menuItemTextMessagesReceived.Text += " (" + smsNotificationText + ")";
                }
            }
            finally
            { //Always redraw the toolbar item.
                ToolBarMain.Invalidate(_butText.Bounds);//To cause the Text button to redraw.			
            }
        }

        #endregion SMS Text Messaging

        private void RefreshMenuClinics()
        {
            menuClinics.MenuItems.Clear();
            List<Clinic> listClinics = Clinics.GetForUserod(Security.CurUser);
            if (listClinics.Count < 30)
            { //This number of clinics will fit in a 990x735 form.
                MenuItem menuItem;
                if (!Security.CurUser.ClinicIsRestricted)
                {
                    menuItem = new MenuItem(Lan.g(this, "Headquarters"), menuClinic_Click);
                    menuItem.Tag = new Clinic();//Having a ClinicNum of 0 will make OD act like 'Headquarters'.  This allows the user to see unassigned appt views, all operatories, etc.
                    if (Clinics.ClinicNum == 0)
                    {
                        menuItem.Checked = true;
                    }
                    menuClinics.MenuItems.Add(menuItem);
                    menuClinics.MenuItems.Add("-");//Separator
                }
                for (int i = 0; i < listClinics.Count; i++)
                {
                    menuItem = new MenuItem(listClinics[i].Abbr, menuClinic_Click);
                    menuItem.Tag = listClinics[i];
                    if (Clinics.ClinicNum == listClinics[i].ClinicNum)
                    {
                        menuItem.Checked = true;
                    }
                    menuClinics.MenuItems.Add(menuItem);
                }
            }
            else
            {//too many clinics to put in a menu drop down
                menuClinics.Click -= menuClick_OpenPickList;
                menuClinics.Click += menuClick_OpenPickList;
            }
            RefreshLocalData(InvalidType.Views);//fills apptviews, sets the view, and then calls ContrAppt.ModuleSelected
            if (!ContrAppt2.Visible)
            {
                RefreshCurrentModule();//calls ModuleSelected of the current module, don't do this if ContrAppt2 is visible since it was just done above
            }
            // TODO: myOutlookBar.RefreshButtons();
            //myOutlookBar.Invalidate();
        }

        private void menuClick_OpenPickList(object sender, EventArgs e)
        {
            FormClinics FormC = new FormClinics();
            FormC.IsSelectionMode = true;
            if (!Security.CurUser.ClinicIsRestricted)
            {
                FormC.IncludeHQInList = true;
            }
            FormC.ShowDialog();
            if (FormC.DialogResult != DialogResult.OK)
            {
                return;
            }
            if (FormC.SelectedClinicNum == 0)
            {//'Headquarters' was selected.
                RefreshCurrentClinic(new Clinic());
                return;
            }
            Clinic clinicCur = Clinics.GetFirstOrDefault(x => x.ClinicNum == FormC.SelectedClinicNum);
            if (clinicCur != null)
            { //Should never be null because the clinic should always be in the list
                RefreshCurrentClinic(clinicCur);
            }
            BeginCheckAlertsThread();
        }

        ///<summary>This is will set the private class wide variable _clinicNum and refresh the current module.</summary>
        private void menuClinic_Click(object sender, System.EventArgs e)
        {
            if (sender.GetType() != typeof(MenuItem) && ((MenuItem)sender).Tag != null)
            {
                return;
            }
            Clinic clinicCur = (Clinic)((MenuItem)sender).Tag;
            RefreshCurrentClinic(clinicCur);
        }

        ///<summary>This is used to set the private class wide variable _clinicNum and refreshes the current module.</summary>
        private void RefreshCurrentClinic(Clinic clinicCur)
        {
            bool isChangingClinic = (Clinics.ClinicNum != clinicCur.ClinicNum);
            Clinics.ClinicNum = clinicCur.ClinicNum;
            Text = PatientL.GetMainTitle(Patients.GetPat(CurPatNum), Clinics.ClinicNum);
            SetSmsNotificationText();
            if (Preference.GetBool(PreferenceName.AppointmentClinicTimeReset))
            {
                AppointmentL.DateSelected = DateTimeOD.Today;
                if (AppointmentL.DateSelected.DayOfWeek == DayOfWeek.Sunday)
                {
                    ContrAppt.WeekStartDate = AppointmentL.DateSelected.AddDays(-6).Date;//go back to previous monday
                }
                else
                {
                    ContrAppt.WeekStartDate = AppointmentL.DateSelected.AddDays(1 - (int)AppointmentL.DateSelected.DayOfWeek).Date;//go back to current monday
                }
                ContrAppt.WeekEndDate = ContrAppt.WeekStartDate.AddDays(ApptDrawing.NumOfWeekDaysToDisplay - 1).Date;
            }
            RefreshMenuClinics();
            if (isChangingClinic)
            {
                _listNormalTaskNums = null;//Will cause task preprocessing to run again.
                _listReminderTasks = null;//Will cause task preprocessing to run again.
                UserControlTasks.ResetGlobalTaskFilterTypesToDefaultAllInstances();
                UserControlTasks.RefreshTasksForAllInstances(null);//Refresh tasks so any filter changes are applied immediately.
            }
        }

        private void FormOpenDental_Resize(object sender, EventArgs e)
        {
            LayoutControls();

            Plugin.Trigger(this, "FormOpenDental_Resized");
        }

        ///<summary>This used to be called much more frequently when it was an actual layout event.</summary>
        private void LayoutControls()
        {
            //Debug.WriteLine("layout");
            if (this.WindowState == FormWindowState.Minimized)
            {
                return;
            }
            if (Width < 200)
            {
                Width = 200;
            }
            Point position = new Point(myOutlookBar.Width, ToolBarMain.Height);
            int width = this.ClientSize.Width - position.X;
            int height = this.ClientSize.Height - position.Y;

            splitContainerNoFlickerDashboard.Location = position;
            splitContainerNoFlickerDashboard.Height = height;
            splitContainerNoFlickerDashboard.Width = width;
            if (userControlPatientDashboard.IsInitialized && userControlPatientDashboard.ListOpenWidgets.Count > 0)
            {
                if (splitContainerNoFlickerDashboard.Panel2.Width != userControlPatientDashboard.Width)
                {//Width has changed.
                    splitContainerNoFlickerDashboard.SplitterDistance = splitContainerNoFlickerDashboard.Width - splitContainerNoFlickerDashboard.SplitterWidth
                        - userControlPatientDashboard.Width;
                }
                if (splitContainerNoFlickerDashboard.Panel2Collapsed)
                {
                    //Un-collapsing this panel needs to happen before setting the Height of userControlPatientDashboard, otherwise, userControlPatientDashboard
                    //will sometimes be invisible on load (until resized), even though the Visible property is true.
                    splitContainerNoFlickerDashboard.Panel2Collapsed = false;//Make the Patient Dashboard visible.
                }
                if (splitContainerNoFlickerDashboard.Height != userControlPatientDashboard.Height)
                {//Height has changed.
                    userControlPatientDashboard.Height = splitContainerNoFlickerDashboard.Panel2.Height;
                }
            }
            else
            {
                splitContainerNoFlickerDashboard.Panel2Collapsed = true;
            }
            FillSignalButtons(null);//Refresh using cache only, do not run query, because this is fired a lot when resizing window or docted task control.
        }

        private void splitContainerNoFlickerDashboard_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (userControlPatientDashboard == null || splitContainerNoFlickerDashboard.Panel2Collapsed)
            {
                return;
            }
            userControlPatientDashboard.UpdateDimensions(splitContainerNoFlickerDashboard.Panel2.Width, splitContainerNoFlickerDashboard.Panel2.Height);
            if (ContrAppt2.Visible)
            {
                ContrAppt2.ModuleSelected(CurPatNum);
            }
        }

        ///<summary>Every time user changes doc position, it will save automatically.</summary>
        private void TaskDockSavePos()
        {
            //ComputerPref computerPref = ComputerPrefs.GetForLocalComputer();
            //if (menuItemDockBottom.Checked)
            //{
            //    ComputerPrefs.LocalComputer.TaskY = panelSplitter.Top;
            //    ComputerPrefs.LocalComputer.TaskDock = 0;
            //}
            //else
            //{
            //    ComputerPrefs.LocalComputer.TaskX = panelSplitter.Left;
            //    ComputerPrefs.LocalComputer.TaskDock = 1;
            //}
            //ComputerPrefs.Update(ComputerPrefs.LocalComputer);
        }

        public static void S_DataValid_BecomeInvalid(OpenDental.ValidEventArgs e)
        {
            _formOpenDentalS?.DataValid_BecameInvalid(e);//Can be null if called from other projects like CEMT
        }

        ///<summary>This is called when any local data becomes outdated.  It's purpose is to tell the other computers to update certain local data.</summary>
        private void DataValid_BecameInvalid(OpenDental.ValidEventArgs e)
        {
            string suffix = Lan.g(nameof(Cache), "Refreshing Caches") + ": ";
            ODEvent.Fire(ODEventType.Cache, suffix);
            if (e.OnlyLocal)
            {//Currently used after doing a restore from FormBackup so that the local cache is forcefully updated.
                ODEvent.Fire(ODEventType.Cache, suffix + Lan.g(nameof(Cache), "PrefsStartup"));
                if (!LoadPreferences())
                {//??
                    return;
                }
                ODEvent.Fire(ODEventType.Cache, suffix + Lan.g(nameof(Cache), "AllLocal"));
                RefreshLocalData(InvalidType.AllLocal);//does local computer only
                return;
            }
            if (!e.ITypes.Contains(InvalidType.Appointment) //local refresh for dates is handled within ContrAppt, not here
                && !e.ITypes.Contains(InvalidType.Task)//Tasks are not "cached" data.
                && !e.ITypes.Contains(InvalidType.TaskPopup))
            {
                RefreshLocalData(e.ITypes);//does local computer
            }
            if (e.ITypes.Contains(InvalidType.Task) || e.ITypes.Contains(InvalidType.TaskPopup))
            {
                Plugin.Trigger(this, "FormOpenDental_DataBecameInvalid");
                if (ContrChart2?.Visible ?? false)
                {
                    ODEvent.Fire(ODEventType.Cache, suffix + Lan.g(nameof(Cache), "Chart Module"));
                    ContrChart2.ModuleSelected(CurPatNum);
                }
                return;//All task signals should already be sent. Sending more Task signals here would cause unnecessary refreshes.
            }
            ODEvent.Fire(ODEventType.Cache, suffix + Lan.g(nameof(Cache), "Inserting Signals"));
            foreach (InvalidType iType in e.ITypes)
            {
                Signalod sig = new Signalod();
                sig.IType = iType;
                if (iType == InvalidType.Task || iType == InvalidType.TaskPopup)
                {
                    sig.FKey = e.TaskNum;
                    sig.FKeyType = KeyType.Task;
                }
                Signalods.Insert(sig);
            }
        }

        ///<summary>Referenced at least 40 times indirectly.</summary>
        public static void S_GotoModule_ModuleSelected(ModuleEventArgs e)
        {
            _formOpenDentalS.GotoModule_ModuleSelected(e);
        }

        ///<summary>This is a way that any form within Open Dental can ask the main form to refresh whatever module is currently selected.</summary>
        public static void S_RefreshCurrentModule(bool hasForceRefresh = false, bool isApptRefreshDataPat = true, bool isClinicRefresh = true)
        {
            _formOpenDentalS.RefreshCurrentModule(hasForceRefresh, isApptRefreshDataPat, isClinicRefresh);
        }

        private void GotoModule_ModuleSelected(ModuleEventArgs e)
        {
            if (e.DateSelected.Year > 1880)
            {
                AppointmentL.DateSelected = e.DateSelected;
            }
            if (e.SelectedAptNum > 0)
            {
                ContrApptSingle.SelectedAptNum = e.SelectedAptNum;
            }
            //patient can also be set separately ahead of time instead of doing it this way:
            if (e.PatNum != 0)
            {
                if (e.PatNum != CurPatNum)
                { //Currently selected patient changed.
                    CurPatNum = e.PatNum;
                    //Going to Chart Module, to specifically handle the SendToMeCreateTask_Click in FormVoiceMails to make sure Patient tab refreshes.
                    //if (Preferences.IsODHQ && e.IModule == 4)
                    //{
                    //    UserControlTasks.RefreshTasksForAllInstances(null, UserControlTasksTab.PatientTickets);//Force a refresh on Task area or Triage.
                    //}
                }
                Patient pat = Patients.GetPat(CurPatNum);
                FillPatientButton(pat);
            }
            UnselectActive();
            allNeutral();
            if (e.ClaimNum > 0)
            {
                myOutlookBar.SelectedIndex = e.IModule;
                ContrAccount2.Visible = true;
                this.ActiveControl = this.ContrAccount2;
                ContrAccount2.ModuleSelected(CurPatNum, e.ClaimNum);
            }
            else if (e.ListPinApptNums.Count != 0)
            {
                myOutlookBar.SelectedIndex = e.IModule;
                ContrAppt2.Visible = true;
                this.ActiveControl = this.ContrAppt2;
                ContrAppt2.ModuleSelectedWithPinboard(CurPatNum, e.ListPinApptNums);
            }
            else if (e.DocNum > 0)
            {
                myOutlookBar.SelectedIndex = e.IModule;
                ContrImages2.Visible = true;
                this.ActiveControl = this.ContrImages2;
                ContrImages2.ModuleSelected(CurPatNum, e.DocNum);
            }
            else if (e.IModule != -1)
            {
                myOutlookBar.SelectedIndex = e.IModule;
                SetModuleSelected();
            }
            myOutlookBar.Invalidate();
        }

        ///<summary>Manipulates the current lightSignalGrid1 control based on the SigMessages passed in.
        ///Pass in a null list in order to simply refresh the lightSignalGrid1 control in its current state (no database call).</summary>
        private void FillSignalButtons(List<SigMessage> listSigMessages)
        {
            if (!DoFillSignalButtons())
            {
                return;
            }
            if (SigButDefList == null)
            {
                SigButDefList = SigButDefs.GetByComputer(SystemInformation.ComputerName);
            }
            int maxButton = SigButDefList.Select(x => x.ButtonIndex).DefaultIfEmpty(-1).Max() + 1;
            int lightGridHeightOld = lightSignalGrid1.Height;
            int lightGridHeightNew = Math.Min(maxButton * 25 + 1, this.ClientRectangle.Height - lightSignalGrid1.Location.Y);
            if (lightGridHeightOld != lightGridHeightNew)
            {
                lightSignalGrid1.Visible = false;//"erases" light signal grid that has been drawn on FormOpenDental
                lightSignalGrid1.Height = lightGridHeightNew;
                lightSignalGrid1.Visible = true;//re-draws light signal grid to the correct size.
            }
            if (listSigMessages == null)
            {
                return;//No new SigMessages to process.
            }
            SigButDef butDef;
            int row;
            Color color;
            bool hadErrorPainting = false;
            foreach (SigMessage sigMessage in listSigMessages)
            {
                if (sigMessage.AckDateTime.Year > 1880)
                {//process ack
                    int buttonIndex = lightSignalGrid1.ProcessAck(sigMessage.SigMessageNum);
                    if (buttonIndex != -1)
                    {
                        butDef = SigButDefs.GetByIndex(buttonIndex, SigButDefList);
                        if (butDef != null)
                        {
                            try
                            {
                                //PaintOnIcon(butDef.SynchIcon, Color.White);
                            }
                            catch
                            {
                                hadErrorPainting = true;
                            }
                        }
                    }
                }
                else
                {//process normal message
                    row = 0;
                    color = Color.White;
                    List<SigElementDef> listSigElementDefs = SigElementDefs.GetDefsForSigMessage(sigMessage);
                    foreach (SigElementDef sigElementDef in listSigElementDefs)
                    {
                        if (sigElementDef.LightRow != 0)
                        {
                            row = sigElementDef.LightRow;
                        }
                        if (sigElementDef.LightColor.ToArgb() != Color.White.ToArgb())
                        {
                            color = sigElementDef.LightColor;
                        }
                    }
                    if (row != 0 && color != Color.White)
                    {
                        lightSignalGrid1.SetButtonActive(row - 1, color, sigMessage);
                        butDef = SigButDefs.GetByIndex(row - 1, SigButDefList);
                        if (butDef != null)
                        {
                            try
                            {
                                //PaintOnIcon(butDef.SynchIcon, color);
                            }
                            catch 
                            {
                                hadErrorPainting = true;
                            }
                        }
                    }
                }
            }
            if (hadErrorPainting)
            {
                MessageBox.Show("Error painting on program icon.  Probably too many non-ack'd messages.");
            }
        }

        /// <summary>
        /// Refreshes the entire lightSignalGrid1 control to the current state according to the database.
        /// This is typically used when the program is first starting up or when a signal is processed for a change to the SigButDef cache.</summary>
        private void FillSignalButtons()
        {
            if (!DoFillSignalButtons())
            {
                return;
            }
            SigButDefList = SigButDefs.GetByComputer(SystemInformation.ComputerName);
            lightSignalGrid1.SetButtons(SigButDefList);
            lightSignalGrid1.Visible = (SigButDefList.Length > 0);
            FillSignalButtons(SigMessages.RefreshCurrentButState());//Get the current SigMessages from the database.
        }

        private bool DoFillSignalButtons()
        {
            if (!Security.IsUserLoggedIn)
            {
                return false;
            }
            if (!lightSignalGrid1.Visible && Programs.UsingEcwTightOrFullMode())
            {//for faster eCW loading
                return false;
            }
            return true;
        }

        
        private void lightSignalGrid1_ButtonClick(object sender, OpenDental.UI.ODLightSignalGridClickEventArgs e)
        {
            if (e.ActiveSignal != null)
            {//user trying to ack an existing light signal
             //Acknowledge all sigmessages in the database which correspond with the button that was just clicked.
             //Only acknowledge sigmessages which have a MessageDateTime prior to the last time we processed signals in the singal timer.
             //This is so that we don't accidentally acknowledge any sigmessages that we are currently unaware of.
                SigMessages.AckButton(e.ButtonIndex + 1, Signalods.SignalLastRefreshed);
                //Immediately update the signal button instead of waiting on our instance to process its own signals.
                e.ActiveSignal.AckDateTime = DateTime.Now;
                FillSignalButtons(new List<SigMessage>() { e.ActiveSignal });//Does not run query.
                return;
            }
            if (e.ButtonDef == null || (e.ButtonDef.SigElementDefNumUser == 0 && e.ButtonDef.SigElementDefNumExtra == 0 && e.ButtonDef.SigElementDefNumMsg == 0))
            {
                return;//There is no signal to send.
            }
            //user trying to send a signal
            SigMessage sigMessage = new SigMessage();
            sigMessage.SigElementDefNumUser = e.ButtonDef.SigElementDefNumUser;
            sigMessage.SigElementDefNumExtra = e.ButtonDef.SigElementDefNumExtra;
            sigMessage.SigElementDefNumMsg = e.ButtonDef.SigElementDefNumMsg;
            SigElementDef sigElementDefUser = SigElementDefs.GetElementDef(e.ButtonDef.SigElementDefNumUser);
            if (sigElementDefUser != null)
            {
                sigMessage.ToUser = sigElementDefUser.SigText;
            }
            SigMessages.Insert(sigMessage);
            FillSignalButtons(new List<SigMessage>() { sigMessage });//Does not run query.
                                                                     //Let the other computers in the office know to refresh this specific light.
            Signalod signal = new Signalod();
            signal.IType = InvalidType.SigMessages;
            signal.FKeyType = KeyType.SigMessage;
            signal.FKey = sigMessage.SigMessageNum;
            Signalods.Insert(signal);
        }

        private void timerTimeIndic_Tick(object sender, System.EventArgs e)
        {
            //every minute:
            if (WindowState != FormWindowState.Minimized && ContrAppt2.Visible)
            {
                ContrAppt2.TickRefresh();
            }
        }

        ///<summary>Usually set at 4 to 6 second intervals.</summary>
        private void timerSignals_Tick(object sender, System.EventArgs e)
        {
            try
            {
                SignalsTick();
            }
            catch (Exception ex)
            {
                SignalsTickExceptionHandler(ex);
            }
        }

        ///<summary>Processes signals.</summary>
        private void SignalsTick()
        {
            try
            {
                //This checks if any forms are open that make us want to continue processing signals even if inactive. Currently only FormTerminal.
                if (Application.OpenForms.OfType<Form>().All(x => x.Name != "FormTerminal"))
                {
                    DateTime dtInactive = Security.DateTimeLastActivity + TimeSpan.FromMinutes((double)Preference.GetInt(PreferenceName.SignalInactiveMinutes));
                    if ((double)Preference.GetInt(PreferenceName.SignalInactiveMinutes) != 0 && DateTime.Now > dtInactive)
                    {
                        return;
                    }
                }
                if (Security.CurUser == null)
                {
                    //User must be at the log in screen, so no need to process signals. We will need to look for shutdown signals since the last refreshed time when the user attempts to log in.
                    return;
                }
                BeginCheckAlertsThread();
            }
            catch
            {
                //Currently do nothing.
            }
            #region Task Preprocessing
            if (_tasksUserNum != Security.CurUser.UserNum //The user has changed since the last signal tick was run (when logoff then logon),
                || _listReminderTasks == null || _listNormalTaskNums == null)//or first time processing signals since the program started.
            {
                _tasksUserNum = Security.CurUser.UserNum;
                List<Task> listRefreshedTasks = Tasks.GetNewTasksThisUser(Security.CurUser.UserNum, Clinics.ClinicNum);//Get all tasks pertaining to current user.
                _listNormalTaskNums = new List<long>();
                _listReminderTasks = new List<Task>();
                _listReminderTasksOverLimit = new List<Task>();
                List<UserOdPref> listBlockedTaskLists = UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum, UserOdFkeyType.TaskListBlock);
                foreach (Task taskForUser in listRefreshedTasks)
                {//Construct the initial task meta data for the current user's tasks.
                    bool isTrackedByUser = Preference.GetBool(PreferenceName.TasksNewTrackedByUser);
                    if (String.IsNullOrEmpty(taskForUser.ReminderGroupId))
                    {//A normal task.
                     //Mimics how checkNew is set in FormTaskEdit.
                        if ((isTrackedByUser && taskForUser.IsUnread) || (!isTrackedByUser && taskForUser.TaskStatus == TaskStatusEnum.New))
                        {//See def of task.IsUnread
                            _listNormalTaskNums.Add(taskForUser.TaskNum);
                        }
                    }
                    else if (!Preference.GetBool(PreferenceName.TasksUseRepeating))
                    {//A reminder task (new or viewed).  Reminders not allowed if repeating tasks enabled.
                        _listReminderTasks.Add(taskForUser);
                        if (taskForUser.DateTimeEntry <= DateTime.Now)
                        {//Do not show reminder popups for future reminders which are not due yet.
                         //Mimics how checkNew is set in FormTaskEdit.
                            if ((isTrackedByUser && taskForUser.IsUnread) || (!isTrackedByUser && taskForUser.TaskStatus == TaskStatusEnum.New))
                            {//See def of task.IsUnread
                             //NOTE: POPUPS ONLY HAPPEN IF THEY ARE MARKED AS NEW. (Also, they will continue to pop up as long as they are marked "new")
                                TaskPopupHelper(taskForUser, listBlockedTaskLists);
                            }
                        }
                    }
                }
                //Refresh the appt module to show the current list of reminders, even if the appt module not visible.  This refresh is fast.
                //The user will load the appt module eventually and these refreshes are the only updates the appointment module receives for reminders.
                ContrAppt2.RefreshReminders(_listReminderTasks);
                _dateReminderRefresh = DateTimeOD.Today;
            }
            //Check to see if a reminder task became due between the last signal interval and the current signal interval.
            else if (_listReminderTasks.FindAll(x => x.DateTimeEntry <= DateTime.Now
                 && x.DateTimeEntry >= DateTime.Now.AddSeconds(-Preference.GetInt(PreferenceName.ProcessSigsIntervalInSecs))).Count > 0)
            {
                List<Task> listDueReminderTasks = _listReminderTasks.FindAll(x => x.DateTimeEntry <= DateTime.Now
                      && x.DateTimeEntry >= DateTime.Now.AddSeconds(-Preference.GetInt(PreferenceName.ProcessSigsIntervalInSecs)));

                List<Signalod> listSignals = new List<Signalod>();
                foreach (Task task in listDueReminderTasks)
                {
                    Signalod sig = new Signalod();
                    sig.IType = InvalidType.TaskList;
                    sig.FKey = task.TaskListNum;
                    sig.FKeyType = KeyType.Undefined;
                    listSignals.Add(sig);
                }
                UserControlTasks.RefreshTasksForAllInstances(listSignals);
                List<UserOdPref> listBlockedTaskLists = UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum, UserOdFkeyType.TaskListBlock);
                foreach (Task reminderTask in listDueReminderTasks)
                {
                    TaskPopupHelper(reminderTask, listBlockedTaskLists);
                }
            }
            else if (_listReminderTasksOverLimit.Count > 0)
            {//Try to display any due reminders that previously exceeded our limit of FormTaskEdit to show.
                List<UserOdPref> listBlockedTaskLists = UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum, UserOdFkeyType.TaskListBlock);
                for (int i = _listReminderTasksOverLimit.Count - 1; i >= 0; i--)
                {//TaskPopupHelper
                    TaskPopupHelper(_listReminderTasksOverLimit[i], listBlockedTaskLists);
                }
            }
            else if (_dateReminderRefresh.Date < DateTimeOD.Today)
            {
                Logger.Write(LogLevel.Info, "Daily reminder refresh is due");

                //Refresh the appt module to show the current list of reminders, even if the appt module not visible.  This refresh is fast.
                //The user will load the appt module eventually and these refreshes are the only updates the appointment module receives for reminders.
                ContrAppt2.RefreshReminders(_listReminderTasks);
                _dateReminderRefresh = DateTimeOD.Today;
            }
            RefreshTasksNotification();
            #endregion Task Preprocessing
            //Signal Processing
            timerSignals.Stop();
            var processSignalForms = new Action<List<ISignalProcessor>, List<Signalod>>((listISignalProcessors, listSignals) =>
            {
                //Broadcast to all subscribed signal processors.
                this.Invoke((() =>
                {
                    listISignalProcessors.ToList().ForEach(x =>
                    {
                        try
                        {
                            x.ProcessObjects(listSignals);
                        }
                        catch
                        {
                        }
                    });
                }));
            });
            Signalods.SignalsTick(new Action(() => this.Invoke(OnShutdown)), processSignalForms, new Action(() => this.Invoke(timerSignals.Start)));
            //Be careful about doing anything that takes a long amount of computation time after the SignalsTick.
            //The UI will appear invalid for the time it takes any methods to process.
            //Post Signal Processing

            //STOP! 
            //If you are trying to do something in FormOpenDental that uses a signal, you should use FormOpenDental.OnProcessSignals() instead.
            //This Function is only for processing things at regular intervals IF IT DOES NOT USE SIGNALS.
        }

        ///<summary>Catches an exception from signal processing and sends the first one to HQ.</summary>
        private void SignalsTickExceptionHandler(Exception ex)
        {
            //If an exception happens during processing signals, we will not close the program because the user is not trying to do anything. We will
            //send the first exception to HQ.
            if (_signalsTickException == null)
            {
                _signalsTickException = new Exception("SignalsTick exception.", ex);
                ODException.SwallowAnyException(() =>
                {
                    //BugSubmissions.SubmitException(_signalsTickException, patNumCur: CurPatNum, moduleName: GetSelectedModuleName());
                });
            }
        }

        ///<summary>Adds the alert items to the alert menu item.</summary>
        private void AddAlertsToMenu()
        {
            //At this point _listAlertItems and _listAlertReads should be user, clinic and subscription filtered.
            //If the counts match this means they have read all AlertItems. 
            //This will result in the 'Alerts' menu item to not be colored.
            int alertCount = _listAlertItems.Count - _listAlertReads.Count;
            if (alertCount > 99)
            {
                menuItemAlerts.Text = Lan.g(this, "Alerts") + " (99)";
            }
            else
            {
                menuItemAlerts.Text = Lan.g(this, "Alerts") + " (" + alertCount + ")";
            }
            List<MenuItem> listMenuItem = menuItemAlerts.MenuItems.Cast<MenuItem>().ToList();
            bool doRedrawMenu = false;
            foreach (MenuItem menuItem in listMenuItem)
            {
                if (menuItem == menuItemAlerts || menuItem == menuItemNoAlerts)
                {//Never want to remove these MenuItems.
                    continue;
                }
                if (_listAlertItems.Any(x => x.AlertItemNum == ((AlertItem)menuItem.Tag).AlertItemNum))
                {
                    continue;//A menu item already exists for this alert. May update the description later.
                }
                menuItemAlerts.MenuItems.Remove(menuItem);//New MenuItem needed for new AlertItem.
                doRedrawMenu = true;
            }
            List<ActionType> listActionTypes = Enum.GetValues(typeof(ActionType)).Cast<ActionType>().ToList();
            listActionTypes.Sort(AlertItem.CompareActionType);
            //Loop through the _listAlertItems to either update or create our MenuItems.
            foreach (AlertItem alertItemCur in _listAlertItems)
            {
                string alertItemKey = alertItemCur.Type.ToString();
                string alertDescriptNew = AlertMenuItemHelper(alertItemCur) + alertItemCur.Description;
                MenuItem menuItem = listMenuItem.Where(x => x != menuItemAlerts && x != menuItemNoAlerts)
                    .FirstOrDefault(x => alertItemCur.AlertItemNum == ((AlertItem)x.Tag).AlertItemNum);
                if (menuItem != null)
                {//Menu already has an item for this alert, so update text if needed.
                    if (menuItem.Text != alertDescriptNew)
                    {
                        menuItem.Text = alertDescriptNew;
                        doRedrawMenu = true;
                    }
                    continue;
                }
                //A List of sub menuitems based off of the available actions for the current AlertItem.
                List<MenuItem> listSubMenuItems = new List<MenuItem>();
                foreach (ActionType actionTypeCur in listActionTypes)
                {
                    if (actionTypeCur == ActionType.None || //This should never be shown to the user. Simply a default ActionType.
                        !alertItemCur.Actions.HasFlag(actionTypeCur))//Current AlertItem does not have this ActionType associated with it.
                    {
                        continue;
                    }
                    MenuItem menuItemSub = new MenuItem(AlertSubMenuItemHelper(actionTypeCur, alertItemCur));
                    menuItemSub.Name = actionTypeCur.ToString();//Used in menuItemAlerts_Click(...) 
                    menuItemSub.Tag = alertItemCur;//Used in menuItemAlerts_Click(...) .
                    menuItemSub.Click += this.menuItemAlerts_Click;
                    listSubMenuItems.Add(menuItemSub);
                }
                MenuItem itemCur = new MenuItem(alertDescriptNew, items: listSubMenuItems.ToArray());
                itemCur.Name = alertItemKey;//Used to find existing menuitems.
                itemCur.Tag = alertItemCur;//Used in menuItemAlerts_DrawItem(...) .
                itemCur.OwnerDraw = true;
                itemCur.DrawItem += this.menuItemAlerts_DrawItem;
                itemCur.MeasureItem += this.menuItemAlerts_MeasureItem;
                menuItemAlerts.MenuItems.Add(itemCur);
                doRedrawMenu = true;
            }
            menuItemAlerts.MenuItems[0].Visible = !(menuItemAlerts.MenuItems.Count > 1);//1 for 'No Alerts' MenuItem which is always there.
            if (doRedrawMenu)
            {
                InvalidateAlertsMenuItem();//Forces menuItemAlerts_DrawItem(...) logic to run again.
            }
        }

        ///<summary>Helper function to translate the title for the given alertItem.</summary>
        private string AlertMenuItemHelper(AlertItem alertItem)
        {
            string value = "";
            switch (alertItem.Type)
            {
                case AlertType.Generic:
                case AlertType.ClinicsChangedInternal:
                    break;
                case AlertType.OnlinePaymentsPending:
                    value += Lan.g(this, "Pending Online Payments") + ": ";
                    break;
                case AlertType.VoiceMailMonitor:
                    value += Lan.g(this, "Voice Mail Monitor") + ": ";
                    break;
                case AlertType.RadiologyProcedures:
                    value += Lan.g(this, "Radiology Orders") + ": ";
                    break;
                case AlertType.CallbackRequested:
                    value += Lan.g(this, "Patient would like a callback regarding this appointment") + ": ";
                    break;
                case AlertType.WebSchedNewPat:
                    value += Lan.g(this, "eServices") + ": ";
                    break;
                case AlertType.WebSchedNewPatApptCreated:
                    value += Lan.g(this, "New Web Sched New Patient Appointment") + ": ";
                    break;
                case AlertType.MaxConnectionsMonitor:
                    value += Lan.g(this, "MySQL Max Connections") + ": ";
                    break;
                case AlertType.WebSchedASAPApptCreated:
                    value += Lan.g(this, "New Web Sched ASAP Appointment") + ": ";
                    break;
                case AlertType.AsteriskServerMonitor:
                    value += Lan.g(this, "Phone Tracking Server") + ": ";
                    break;
                case AlertType.WebSchedRecallApptCreated:
                    value += Lan.g(this, "New Web Sched Recall Appointment") + ": ";
                    break;
                case AlertType.WebMailRecieved:
                    value += Lan.g(this, "Unread Web Mails") + ": ";
                    break;
                case AlertType.NumberBarredFromTexting:
                case AlertType.MultipleEConnectors:
                case AlertType.EConnectorDown:
                case AlertType.EConnectorError:
                case AlertType.DoseSpotProviderRegistered:
                case AlertType.DoseSpotClinicRegistered:
                case AlertType.ClinicsChanged:
                default:
                    value += Lan.g(this, alertItem.Type.GetDescription()) + ": ";
                    break;
            }
            return value;
        }

        ///<summary>Helper function to translate the title for the given alerttype and alertItem.</summary>
        private string AlertSubMenuItemHelper(ActionType actionType, AlertItem parentAlertItem)
        {
            string value = "";
            switch (actionType)
            {
                case ActionType.None://This should never happen.
                    value += Lan.g(this, "None");
                    break;
                case ActionType.MarkAsRead:
                    value += Lan.g(this, "Mark As Read");
                    break;
                case ActionType.OpenForm:
                    value += Lan.g(this, "Open " + parentAlertItem.FormToOpen.GetDescription());
                    break;
                case ActionType.Delete:
                    value += Lan.g(this, "Delete Alert");
                    break;
                case ActionType.ShowItemValue:
                    value += Lan.g(this, "View Details");
                    break;
            }
            return value;
        }

        ///<summary>Takes one task and determines if it should popup for the current user.  Displays task popup if needed.</summary>
        private void TaskPopupHelper(Task taskPopup, List<UserOdPref> listBlockedTaskLists, List<TaskNote> listNotesForTask = null)
        {
            try
            {
                if (taskPopup.DateTimeEntry > DateTime.Now && taskPopup.ReminderType != TaskReminderType.NoReminder)
                {
                    return;//Don't pop up future dated reminder tasks
                }
                //Don't pop up reminders if we reach our upper limit of open FormTaskEdit windows to avoid overwhelming users with popups.
                //Add the task to another list that temporarily holds the reminder task until it is allowed to popup.
                if (taskPopup.ReminderType != TaskReminderType.NoReminder)
                {//Is a reminder task.
                    if (Application.OpenForms.OfType<FormTaskEdit>().ToList().Count >= _popupPressureReliefLimit)
                    {//Open Task Edit windows over display limit.
                        if (!_listReminderTasksOverLimit.Exists(x => x.TaskNum == taskPopup.TaskNum))
                        {
                            _listReminderTasksOverLimit.Add(taskPopup);//Add to list to be shown later to prevent too many windows from being open at same time.
                        }
                        return;//We are over the display limit for now.   Will try again later after user closes some Task Edit windows.
                    }
                    _listReminderTasksOverLimit.RemoveAll(x => x.TaskNum == taskPopup.TaskNum);//Remove from list if present.
                }
                //Even though this is triggered to popup, if this is my own task, then do not popup.
                List<TaskNote> notesForThisTask = (listNotesForTask ?? TaskNotes.GetForTask(taskPopup.TaskNum)).OrderBy(x => x.DateTimeNote).ToList();
                if (taskPopup.ReminderType == TaskReminderType.NoReminder)
                {//We care about notes and task sender only if it's not a reminder.
                    if (notesForThisTask.Count == 0)
                    {//'sender' is the usernum on the task and it's not a reminder
                        if (taskPopup.UserNum == Security.CurUser.UserNum)
                        {
                            return;
                        }
                    }
                    else
                    {//'sender' is the user on the last added note
                        if (notesForThisTask[notesForThisTask.Count - 1].UserNum == Security.CurUser.UserNum)
                        {
                            return;
                        }
                    }
                }
                List<TaskList> listUserTaskListSubsTrunk = TaskLists.RefreshUserTrunk(Security.CurUser.UserNum);//Get the list of directly subscribed tasklists.
                List<long> listUserTaskListSubNums = listUserTaskListSubsTrunk.Select(x => x.TaskListNum).ToList();
                bool isUserSubscribed = listUserTaskListSubNums.Contains(taskPopup.TaskListNum);//First check if user is directly subscribed.
                if (!isUserSubscribed)
                {
                    isUserSubscribed = listUserTaskListSubsTrunk.Any(x => TaskLists.IsAncestor(x.TaskListNum, taskPopup.TaskListNum));//Check ancestors for subscription.
                }
                if (isUserSubscribed)
                {//User is subscribed to this TaskList, or one of its ancestors.
                    if (!listBlockedTaskLists.Any(x => x.Fkey == taskPopup.TaskListNum && PIn.Bool(x.ValueString)))
                    {//Subscribed and Unblocked, Show it!
                        SoundPlayer soundplay = new SoundPlayer(Properties.Resources.notify);
                        soundplay.Play();
                        FormTaskEdit FormT = new FormTaskEdit(taskPopup);
                        FormT.IsPopup = true;
                        FormT.Show();//non-modal
                    }
                }
            }
            finally
            {
            }
        }

        ///<summary>MenuItem does not have an invalidate or refresh so we quickly disable and enable the menu item so that the OwnerDraw methods get called.</summary>
        private void InvalidateAlertsMenuItem()
        {
            menuItemAlerts.Enabled = false;
            menuItemAlerts.Enabled = true;
            foreach (MenuItem menuItem in menuItemAlerts.MenuItems)
            {
                menuItem.Enabled = false;
                menuItem.Enabled = true;
            }
        }

        ///<summary>Called when a shutdown signal is found.</summary>
        private void OnShutdown()
        {
            if (timerSignals.Tag?.ToString() == "shutdown")
            {
                //We have already responded to the shutdown signal.
                return;
            }
            timerSignals.Enabled = false;//quit receiving signals.
            timerSignals.Tag = "shutdown";
            string msg = "";
            if (Process.GetCurrentProcess().ProcessName == "OpenDental")
            {
                msg += "All copies of Open Dental ";
            }
            else
            {
                msg += Process.GetCurrentProcess().ProcessName + " ";
            }
            msg += Lan.g(this, "will shut down in 15 seconds.  Quickly click OK on any open windows with unsaved data.");
            MsgBoxCopyPaste msgbox = new MsgBoxCopyPaste(msg);
            msgbox.Size = new Size(300, 300);
            msgbox.TopMost = true;
            msgbox.Show();
            BeginShutdownThread();
            return;
        }

        ///<summary>This only contains UI signal processing. See Signalods.SignalsTick() for cache updates.</summary>
        public override void OnProcessSignals(List<Signalod> listSignals)
        {
            if (listSignals.Exists(x => x.IType == InvalidType.Programs))
            {
                RefreshMenuReports();
            }
            #region SMS Notifications
            Signalod signalSmsCount = listSignals.OrderByDescending(x => x.SigDateTime)
                .FirstOrDefault(x => x.IType == InvalidType.SmsTextMsgReceivedUnreadCount && x.FKeyType == KeyType.SmsMsgUnreadCount);
            if (signalSmsCount != null)
            {
                //Provide the pre-existing value here. This will act as a flag indicating that we should not resend the signal.  This would cause infinite signal loop.
                SetSmsNotificationText(signalSmsCount);
            }
            #endregion SMS Notifications
            #region Tasks
            List<Signalod> listSignalTasks = listSignals.FindAll(x => x.IType == InvalidType.Task || x.IType == InvalidType.TaskPopup
                  || x.IType == InvalidType.TaskList || x.IType == InvalidType.TaskAuthor || x.IType == InvalidType.TaskPatient);
            List<long> listEditedTaskNums = listSignalTasks.FindAll(x => x.FKeyType == KeyType.Task).Select(x => x.FKey).ToList();
            BeginTasksThread(listSignalTasks, listEditedTaskNums);
            #endregion Tasks
            #region Appointment Module
            if (ContrAppt2.Visible)
            {
                bool isRefreshAppts = Signalods.IsApptRefreshNeeded(AppointmentL.DateSelected.Date, listSignals);
                bool isRefreshScheds = Signalods.IsSchedRefreshNeeded(AppointmentL.DateSelected.Date, listSignals);
                if (isRefreshAppts || isRefreshScheds)
                {
                    ContrAppt2.RefreshPeriod(false, isRefreshAppointments: isRefreshAppts, isRefreshSchedules: isRefreshScheds);
                }
            }
            #endregion Appointment Module
            #region Unfinalize Pay Menu Update
            UpdateUnfinalizedPayCount(listSignals.FindAll(x => x.IType == InvalidType.UnfinalizedPayMenuUpdate));
            #endregion Unfinalize Pay Menu Update
            #region Refresh
            InvalidType[] arrInvalidTypes = Signalods.GetInvalidTypes(listSignals);
            if (arrInvalidTypes.Length > 0)
            {
                RefreshLocalDataPostCleanup(arrInvalidTypes);
            }
            #endregion Refresh
            //Sig Messages must be the last code region to run in the process signals method because it changes the application icon.
            #region Sig Messages (In the manual as "Internal Messages")
            //Check to see if any signals are sigmessages.
            List<long> listSigMessageNums = listSignals.FindAll(x => x.IType == InvalidType.SigMessages && x.FKeyType == KeyType.SigMessage).Select(x => x.FKey).ToList();
            if (listSigMessageNums.Count > 0)
            {
                //Any SigMessage iType means we need to refresh our lights or buttons.
                List<SigMessage> listSigMessages = SigMessages.GetSigMessages(listSigMessageNums);
                ContrManage2.LogMsgs(listSigMessages);
                FillSignalButtons(listSigMessages);
                //Need to add a test to this: do not play messages that are over 2 minutes old.
                BeginPlaySoundsThread(listSigMessages);
            }
            #endregion Sig Messages

            Plugin.Trigger(this, "FormOpenDental_ProcessSignals", listSignals);
        }

        ///<summary>Will invoke a refresh of tasks on the only instance of FormOpenDental. listRefreshedTaskNotes and listBlockedTaskLists are only used 
        ///for Popup tasks, only used if listRefreshedTasks includes at least one popup task.</summary>
        public static void S_HandleRefreshedTasks(List<Signalod> listSignalTasks, List<long> listEditedTaskNums, List<Task> listRefreshedTasks,
            List<TaskNote> listRefreshedTaskNotes, List<UserOdPref> listBlockedTaskLists)
        {
            _formOpenDentalS.HandleRefreshedTasks(listSignalTasks, listEditedTaskNums, listRefreshedTasks, listRefreshedTaskNotes, listBlockedTaskLists);
        }

        ///<summary>Refreshes tasks and pops up as necessary. Invoked from thread callback in OnProcessSignals(). listRefreshedTaskNotes and 
        ///listBlockedTaskLists are only used for Popup tasks, only used if listRefreshedTasks includes at least one popup task.</summary>
        private void HandleRefreshedTasks(List<Signalod> listSignalTasks, List<long> listEditedTaskNums, List<Task> listRefreshedTasks,
            List<TaskNote> listRefreshedTaskNotes, List<UserOdPref> listBlockedTaskLists)
        {
            bool hasChangedReminders = UpdateTaskMetaData(listEditedTaskNums, listRefreshedTasks);
            RefreshTasksNotification();
            RefreshOpenTasksOrPopupNewTasks(listSignalTasks, listRefreshedTasks, listRefreshedTaskNotes, listBlockedTaskLists);
            //Refresh the appt module if reminders have changed, even if the appt module not visible.
            //The user will load the appt module eventually and these refreshes are the only updates the appointment module receives for reminders.
            if (hasChangedReminders)
            {
                ContrAppt2.RefreshReminders(_listReminderTasks);
                _dateReminderRefresh = DateTimeOD.Today;
            }
        }

        ///<summary>Updates the class-wide meta data used for updating the task notification UI elements.
        ///Returns true if a reminder task has changed.  Otherwise; false.</summary>
        private bool UpdateTaskMetaData(List<long> listEditedTaskNums, List<Task> listRefreshedTasks)
        {
            //Check to make sure there are edited task nums passed in and that the meta data lists have been initialized by the signal processor.
            if (listEditedTaskNums == null || _listReminderTasks == null || _listNormalTaskNums == null)
            {
                return false;//Nothing to do.
            }
            bool hasChangedReminders = false;
            for (int i = 0; i < listEditedTaskNums.Count; i++)
            {//Update the task meta data for the current user based on the query results.
                long editedTaskNum = listEditedTaskNums[i];//The tasknum mentioned in the signal.
                Task taskForUser = listRefreshedTasks?.FirstOrDefault(x => x.TaskNum == editedTaskNum);
                Task taskNewForUser = null;
                if (taskForUser != null)
                {
                    bool isTrackedByUser = Preference.GetBool(PreferenceName.TasksNewTrackedByUser);
                    //Mimics how checkNew is set in FormTaskEdit.
                    if (((isTrackedByUser && taskForUser.IsUnread) || (!isTrackedByUser && taskForUser.TaskStatus == TaskStatusEnum.New))//See def of task.IsUnread
                                                                                                                                         //Reminders not due yet are excluded from Tasks.RefreshUserNew().
                        && (string.IsNullOrEmpty(taskForUser.ReminderGroupId) || taskForUser.DateTimeEntry <= DateTime.Now))
                    {
                        taskNewForUser = taskForUser;
                    }
                }
                Task taskReminderOld = _listReminderTasks.FirstOrDefault(x => x.TaskNum == editedTaskNum);
                if (taskReminderOld != null)
                {//The task is a reminder which is relevant to the current user.
                    hasChangedReminders = true;
                    _listReminderTasks.RemoveAll(x => x.TaskNum == editedTaskNum);//Remove the old copy of the task.
                    if (taskForUser != null)
                    {//The updated reminder task is relevant to the current user.
                        _listReminderTasks.Add(taskForUser);//Add the updated reminder task into the list (replacing the old reminder task).
                    }
                }
                else if (_listNormalTaskNums.Contains(editedTaskNum))
                {//The task is a normal task which is relevant to the current user.
                    if (taskNewForUser == null)
                    {//But now the task is no longer relevant to the user.
                        _listNormalTaskNums.Remove(editedTaskNum);
                    }
                }
                else
                {//The edited tasknum is not currently in our meta data.
                    if (taskNewForUser != null && String.IsNullOrEmpty(taskNewForUser.ReminderGroupId))
                    {//A new normal task has now become relevant.
                        _listNormalTaskNums.Add(editedTaskNum);
                    }
                    else if (taskForUser != null && !String.IsNullOrEmpty(taskForUser.ReminderGroupId))
                    {//A reminder task has become relevant (new or viewed)
                        hasChangedReminders = true;
                        _listReminderTasks.Add(taskForUser);
                    }
                }//else
            }//for
            return hasChangedReminders;
        }

        private void RefreshOpenTasksOrPopupNewTasks(List<Signalod> listSignalTasks, List<Task> listRefreshedTasks, List<TaskNote> listRefreshedTaskNotes,
            List<UserOdPref> listBlockedTaskLists)
        {
            if (listSignalTasks == null)
            {
                return;//Nothing to do if there was no signal sent which means no task has been flagged as needing to be refreshed.
            }
            List<long> listSignalTasksNums = listSignalTasks.Select(x => x.FKey).ToList();
            List<long> listTaskNumsOpen = new List<long>();
            for (int i = 0; i < Application.OpenForms.Count; i++)
            {
                Form form = Application.OpenForms[i];
                if (!(form is FormTaskEdit))
                {
                    continue;
                }
                FormTaskEdit FormTE = (FormTaskEdit)form;
                if (listSignalTasksNums.Contains(FormTE.TaskNumCur))
                {
                    FormTE.OnTaskEdited();
                    listTaskNumsOpen.Add(FormTE.TaskNumCur);
                }
            }
            List<Task> tasksPopup = new List<Task>();
            if (listRefreshedTasks != null)
            {
                for (int i = 0; i < listRefreshedTasks.Count; i++)
                {//Locate any popup tasks in the returned list of tasks.
                 //Verify the current task is a popup task.
                    if (!listSignalTasks.Exists(x => x.FKeyType == KeyType.Task && x.IType == InvalidType.TaskPopup && x.FKey == listRefreshedTasks[i].TaskNum)
                        || listTaskNumsOpen.Contains(listRefreshedTasks[i].TaskNum))
                    {
                        continue;//Not a popup task or is already open.
                    }
                    tasksPopup.Add(listRefreshedTasks[i]);
                }
            }
            for (int i = 0; i < tasksPopup.Count; i++)
            {
                //Reminders sent to a subscribed tasklist will pop up prior to the reminder date/time.
                TaskPopupHelper(tasksPopup[i], listBlockedTaskLists, listRefreshedTaskNotes?.FindAll(x => x.TaskNum == tasksPopup[i].TaskNum));
            }
            if (listSignalTasks.Count > 0 || tasksPopup.Count > 0)
            {
                UserControlTasks.RefreshTasksForAllInstances(listSignalTasks);
            }
        }

        ///<summary></summary>
        public void ProcessKillCommand()
        {
            //It is crucial that every form be forcefully closed so that they do not stay connected to a database that has been updated to a more recent version.
            CloseOpenForms(true);
            Application.Exit();//This will call FormOpenDental's closing event which will clean up all threads that are currently running.
        }

        ///<summary></summary>
        public static void S_ProcessKillCommand()
        {
            _formOpenDentalS.ProcessKillCommand();
        }

        private void myOutlookBar_ButtonClicked(object sender, OpenDental.OutlookBarButtonEventArgs e)
        {
            switch (myOutlookBar.SelectedIndex)
            {
                case 0:
                    if (!Security.IsAuthorized(Permissions.AppointmentsModule))
                    {
                        e.Cancel = true;
                        return;
                    }
                    break;
                case 1:
                    if (Preference.GetBool(PreferenceName.EhrEmergencyNow))
                    {//if red emergency button is on
                        if (Security.IsAuthorized(Permissions.EhrEmergencyAccess, true))
                        {
                            break;//No need to check other permissions.
                        }
                    }
                    //Whether or not they were authorized by the special situation above,
                    //they can get into the Family module with the ordinary permissions.
                    if (!Security.IsAuthorized(Permissions.FamilyModule))
                    {
                        e.Cancel = true;
                        return;
                    }
                    break;
                case 2:
                    if (!Security.IsAuthorized(Permissions.AccountModule))
                    {
                        e.Cancel = true;
                        return;
                    }
                    break;
                case 3:
                    if (!Security.IsAuthorized(Permissions.TPModule))
                    {
                        e.Cancel = true;
                        return;
                    }
                    break;
                case 4:
                    if (!Security.IsAuthorized(Permissions.ChartModule))
                    {
                        e.Cancel = true;
                        return;
                    }
                    break;
                case 5:
                    if (!Security.IsAuthorized(Permissions.ImagesModule))
                    {
                        e.Cancel = true;
                        return;
                    }
                    break;
                case 6:
                    if (!Security.IsAuthorized(Permissions.ManageModule))
                    {
                        e.Cancel = true;
                        return;
                    }
                    break;
            }
            UnselectActive();
            allNeutral();
            SetModuleSelected(true);
        }

        ///<summary>Returns the translated name of the currently selected module.</summary>
        public string GetSelectedModuleName()
        {
            try
            {
                return myOutlookBar.Buttons[myOutlookBar.SelectedIndex].Caption;
            }
            catch
            {
                return "";
            }
        }

        ///<summary>Sets the currently selected module based on the selectedIndex of the outlook bar. If selectedIndex is -1, which might happen if user does not have permission to any module, then this does nothing.</summary>
        private void SetModuleSelected()
        {
            SetModuleSelected(false);
        }

        ///<summary>Sets the currently selected module based on the selectedIndex of the outlook bar. If selectedIndex is -1, which might happen if user does not have permission to any module, then this does nothing. The menuBarClicked variable should be set to true when a module button is clicked, and should be false when called for refresh purposes.</summary>
        private void SetModuleSelected(bool menuBarClicked)
        {
            switch (myOutlookBar.SelectedIndex)
            {
                case 0:
                    ContrAppt2.InitializeOnStartup();
                    ContrAppt2.Visible = true;
                    this.ActiveControl = this.ContrAppt2;
                    ContrAppt2.ModuleSelected(CurPatNum);
                    break;
                case 1:
                    if (HL7Defs.IsExistingHL7Enabled())
                    {
                        HL7Def def = HL7Defs.GetOneDeepEnabled();
                        if (def.ShowDemographics == HL7ShowDemographics.Hide)
                        {
                            ContrFamily2Ecw.Visible = true;
                            this.ActiveControl = this.ContrFamily2Ecw;
                            ContrFamily2Ecw.ModuleSelected(CurPatNum);
                        }
                        else
                        {
                            ContrFamily2.InitializeOnStartup();
                            ContrFamily2.Visible = true;
                            this.ActiveControl = this.ContrFamily2;
                            ContrFamily2.ModuleSelected(CurPatNum);
                        }
                    }
                    else
                    {
                        if (Programs.UsingEcwTightMode())
                        {
                            ContrFamily2Ecw.Visible = true;
                            this.ActiveControl = this.ContrFamily2Ecw;
                            ContrFamily2Ecw.ModuleSelected(CurPatNum);
                        }
                        else
                        {
                            ContrFamily2.InitializeOnStartup();
                            ContrFamily2.Visible = true;
                            this.ActiveControl = this.ContrFamily2;
                            ContrFamily2.ModuleSelected(CurPatNum);
                        }
                    }
                    break;
                case 2:
                    ContrAccount2.InitializeOnStartup();
                    ContrAccount2.Visible = true;
                    this.ActiveControl = this.ContrAccount2;
                    ContrAccount2.ModuleSelected(CurPatNum);
                    break;
                case 3:
                    ContrTreat2.InitializeOnStartup();
                    ContrTreat2.Visible = true;
                    this.ActiveControl = this.ContrTreat2;
                    if (menuBarClicked)
                    {
                        ContrTreat2.ModuleSelected(CurPatNum, true);//Set default date to true when button is clicked.
                    }
                    else
                    {
                        ContrTreat2.ModuleSelected(CurPatNum);
                    }
                    break;
                case 4:
                    ContrChart2.InitializeOnStartup();
                    ContrChart2.Visible = true;
                    this.ActiveControl = this.ContrChart2;
                    if (menuBarClicked)
                    {
                        ContrChart2.ModuleSelectedErx(CurPatNum);
                    }
                    else
                    {
                        ContrChart2.ModuleSelected(CurPatNum, true);
                    }
                    TryNonPatientPopup();
                    break;
                case 5:
                    ContrImages2.InitializeOnStartup();
                    ContrImages2.Visible = true;
                    this.ActiveControl = this.ContrImages2;
                    ContrImages2.ModuleSelected(CurPatNum);
                    break;
                case 6:
                    //ContrManage2.InitializeOnStartup();//This gets done earlier.
                    ContrManage2.Visible = true;
                    this.ActiveControl = this.ContrManage2;
                    ContrManage2.ModuleSelected(CurPatNum);
                    break;
            }
        }

        private void allNeutral()
        {
            ContrAppt2.Visible = false;
            ContrFamily2.Visible = false;
            ContrFamily2Ecw.Visible = false;
            ContrAccount2.Visible = false;
            ContrTreat2.Visible = false;
            ContrChart2.Visible = false;
            ContrImages2.Visible = false;
            ContrManage2.Visible = false;
        }

        private void UnselectActive(bool isLoggingOff = false)
        {
            if (ContrAppt2.Visible)
            {
                ContrAppt2.ModuleUnselected();
            }
            if (ContrFamily2.Visible)
            {
                ContrFamily2.ModuleUnselected();
            }
            if (ContrFamily2Ecw.Visible)
            {
                //ContrFamily2Ecw.ModuleUnselected();
            }
            if (ContrAccount2.Visible)
            {
                ContrAccount2.ModuleUnselected();
            }
            if (ContrTreat2.Visible)
            {
                ContrTreat2.ModuleUnselected();
            }
            if (ContrChart2.Visible)
            {
                ContrChart2.ModuleUnselected(isLoggingOff);
            }
            if (ContrImages2.Visible)
            {
                ContrImages2.ModuleUnselected();
            }
        }

        ///<Summary>This also passes CurPatNum down to the currently selected module (except the Manage module).  If calling from ContrAppt and RefreshModuleDataPatient was called before calling this method, set isApptRefreshDataPat=false so the get pat query isn't run twice.</Summary>
        private void RefreshCurrentModule(bool hasForceRefresh = false, bool isApptRefreshDataPat = true, bool isClinicRefresh = true, long docNum = 0)
        {
            if (ContrAppt2.Visible)
            {
                if (hasForceRefresh)
                {
                    ContrAppt2.ModuleSelected(CurPatNum);
                }
                else
                {
                    if (isApptRefreshDataPat)
                    {//don't usually skip data refresh, only if CurPatNum was set just prior to calling this method
                        ContrAppt2.RefreshModuleDataPatient(CurPatNum);
                    }
                    ContrAppt2.RefreshModuleScreenPatient();
                }
            }
            if (ContrFamily2.Visible)
            {
                ContrFamily2.ModuleSelected(CurPatNum);
            }
            if (ContrFamily2Ecw.Visible)
            {
                ContrFamily2Ecw.ModuleSelected(CurPatNum);
            }
            if (ContrAccount2.Visible)
            {
                ContrAccount2.ModuleSelected(CurPatNum);
            }
            if (ContrTreat2.Visible)
            {
                ContrTreat2.ModuleSelected(CurPatNum);
            }
            if (ContrChart2.Visible)
            {
                ContrChart2.ModuleSelected(CurPatNum, isClinicRefresh);
            }
            if (ContrImages2.Visible)
            {
                ContrImages2.ModuleSelected(CurPatNum, docNum);
            }
            if (ContrManage2.Visible)
            {
                ContrManage2.ModuleSelected(CurPatNum);
            }
            userControlTasks1.RefreshPatTicketsIfNeeded();
        }

        /// <summary>sends function key presses to the appointment module and chart module</summary>
        private void FormOpenDental_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //This suppresses the base windows functionality for giving focus to the main menu on F10. See Job 8289
            if (e.KeyCode == Keys.F10)
            {
                e.SuppressKeyPress = true;
            }
            if (ContrAppt2.Visible && e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F12)
            {
                ContrAppt2.FunctionKeyPress(e.KeyCode);
                return;
            }
            if (ContrChart2.Visible && e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F12)
            {
                ContrChart2.FunctionKeyPressContrChart(e.KeyCode);
                return;
            }
            //Ctrl-Alt-R is supposed to show referral window, but it doesn't work on some computers.
            //so we're also going to use Ctrl-X to show the referral window.
            if (CurPatNum != 0
                && (e.Modifiers == (Keys.Alt | Keys.Control) && e.KeyCode == Keys.R)
                    || (e.Modifiers == Keys.Control && e.KeyCode == Keys.X))
            {
                FormReferralsPatient FormRE = new FormReferralsPatient();
                FormRE.PatNum = CurPatNum;
                FormRE.ShowDialog();
            }

            Plugin.Trigger(this, "FormOpenDental_KeyDown", e);
        }

        ///<summary>This method stops all (local) timers and displays a connection lost window that will let users attempt to reconnect.
        ///At any time during the lifespan of the application connection to the database can be lost for unknown reasons.
        ///When anything spawned by FormOpenDental (main thread) tries to connect to the database and fails, this event will get fired.</summary>
        private void DataConnection_ConnectionLost(DataConnectionEventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(() => DataConnection_ConnectionLost(e));
                return;
            }
            if (e == null || e.EventType != ODEventType.DataConnection || e.IsConnectionRestored)
            {
                return;
            }
            BeginDataConnectionLostThread(e);
        }

        public static void S_TaskGoTo(TaskObjectType taskOT, long keyNum)
        {
            _formOpenDentalS.TaskGoTo(taskOT, keyNum);
        }

        private void TaskGoTo(TaskObjectType taskOT, long keyNum)
        {
            if (taskOT == TaskObjectType.None || keyNum == 0)
            {
                return;
            }
            if (taskOT == TaskObjectType.Patient)
            {
                CurPatNum = keyNum;
                Patient pat = Patients.GetPat(CurPatNum);
                RefreshCurrentModule();
                FillPatientButton(pat);
            }
            if (taskOT == TaskObjectType.Appointment)
            {
                Appointment apt = Appointments.GetOneApt(keyNum);
                if (apt == null)
                {
                    MsgBox.Show(this, "Appointment has been deleted, so it's not available.");
                    return;
                }
                DateTime dateSelected = DateTime.MinValue;
                if (apt.AptStatus == ApptStatus.Planned || apt.AptStatus == ApptStatus.UnschedList)
                {
                    //I did not add feature to put planned or unsched apt on pinboard.
                    MsgBox.Show(this, "Cannot navigate to appointment.  Use the Other Appointments button.");
                    //return;
                }
                else
                {
                    dateSelected = apt.AptDateTime;
                }
                CurPatNum = apt.PatNum;//OnPatientSelected(apt.PatNum);
                FillPatientButton(Patients.GetPat(CurPatNum));
                GotoModule.GotoAppointment(dateSelected, apt.AptNum);
            }
        }

        private void comboTriageCoordinator_MouseWheel(object sender, MouseEventArgs e)
        {
            ComboBox comboControl = (ComboBox)sender;
            if (!comboControl.DroppedDown)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        #region Menu

        /// <summary>
        /// Log out of the current database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void menuItemLogOff_Click(object sender, EventArgs e)
        {
            var logOffMessage = UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum, UserOdFkeyType.SuppressLogOffMessage).FirstOrDefault();
            if (logOffMessage == null) // Doesn't exist in the database
            {
                var checkResult = 
                    new InputBox(
                        OpenDental.Translation.Language.LogOffConfirmation,
                        OpenDental.Translation.Language.DoNotShowThisMessageAgain,
                        true, new Point(0, 40));

                checkResult.ShowDialog();
                if (checkResult.DialogResult == DialogResult.Cancel)
                {
                    return;
                }
                else if (checkResult.DialogResult == DialogResult.OK)
                {
                    if (checkResult.checkBoxResult.Checked)
                    {
                        UserOdPrefs.Insert(new UserOdPref()
                        {
                            UserNum = Security.CurUser.UserNum,
                            FkeyType = UserOdFkeyType.SuppressLogOffMessage
                        });
                    }
                }
            }

            LogOffNow(false);
        }

        #region Menu: File

        /// <summary>
        /// Open the dialog to let the user change their password.
        /// </summary>
        void menuItemPassword_Click(object sender, EventArgs e) => SecurityL.ChangePassword(false);
        
        /// <summary>
        /// Open the dialog to let the user configurate their e-mail address.
        /// </summary>
        void menuItemUserEmailAddress_Click(object sender, EventArgs e)
        {
            var emailAddress = EmailAddress.GetByUser(Security.CurUser.UserNum) ?? new EmailAddress { UserId = Security.CurUser.UserNum };

            using (var formEmailAddressEdit = new FormEmailAddressEdit(emailAddress))
            {
                formEmailAddressEdit.ShowDialog();
            }
        }

        /// <summary>
        /// Opens the user settings dialog.
        /// </summary>
        void menuItemUserSettings_Click(object sender, EventArgs e)
        {
            using (var formUserSetting = new FormUserSetting())
            {
                formUserSetting.ShowDialog();
            }
        }

        /// <summary>
        /// Opens the printer setup dialog.
        /// </summary>
        void menuItemPrinter_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup)) return;

            using (var formPrinterSetup = new FormPrinterSetup())
            {
                formPrinterSetup.ShowDialog();

                SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Printers");
            }
        }

        /// <summary>
        /// Opens the graphics settings dialog.
        /// </summary>
        void menuItemGraphics_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.GraphicsEdit)) return;
            
            Cursor = Cursors.WaitCursor;

            using (var formGraphics = new FormGraphics())
            {
                if (formGraphics.ShowDialog() == DialogResult.OK)
                {
                    ContrChart2.InitializeLocalData();

                    RefreshCurrentModule();
                }
            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Open the dialog to switch to another database.
        /// </summary>
        void menuItemConfig_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.ChooseDatabase)) return;
            
            SecurityLogs.MakeLogEntry(Permissions.ChooseDatabase, 0, "");//make the entry before switching databases.

            using (var formChooseDatabase = new FormChooseDatabase())
            {
                if (formChooseDatabase.ShowDialog(this) == DialogResult.Cancel)
                {
                    return;
                }
            }

            CurPatNum = 0;
            RefreshCurrentModule(); // clumsy but necessary. Sets child PatNums to 0.
            FillPatientButton(null);

            if (!LoadPreferences())
            {
                return;
            }

            RefreshLocalData(InvalidType.AllLocal);
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        void menuItemExit_Click(object sender, EventArgs e) => Application.Exit();
        
        #endregion



        #region Setup


        //Setup
        private void menuItemApptFieldDefs_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormApptFieldDefs FormA = new FormApptFieldDefs();
            FormA.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Appointment Field Defs");
        }

        private void menuItemApptRules_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormApptRules FormA = new FormApptRules();
            FormA.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Appointment Rules");
        }

        private void menuItemApptTypes_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormApptTypes FormA = new FormApptTypes();
            FormA.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Appointment Types");
        }

        private void menuItemApptViews_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormApptViews FormAV = new FormApptViews();
            FormAV.ShowDialog();
            RefreshCurrentModule(true);
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Appointment Views");
        }

        private void menuItemAlertCategories_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.SecurityAdmin))
            {
                return;
            }
            FormAlertCategorySetup FormACS = new FormAlertCategorySetup();
            FormACS.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin, 0, "Alert Categories");
        }

        private void menuItemAutoCodes_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormAutoCode FormAC = new FormAutoCode();
            FormAC.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Auto Codes");
        }

        private void menuItemAutomation_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormAutomation FormA = new FormAutomation();
            FormA.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Automation");
        }

        private void menuItemAutoNotes_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.AutoNoteQuickNoteEdit))
            {
                return;
            }
            FormAutoNotes FormA = new FormAutoNotes();
            FormA.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.AutoNoteQuickNoteEdit, 0, "Auto Notes Setup");
        }

        private void menuItemMobileAppDevices_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormMobileAppDevices formMobileAppDevices = new FormMobileAppDevices();
            formMobileAppDevices.ShowDialog();
        }

        private void menuItemClaimForms_Click(object sender, System.EventArgs e)
        {
            if (Preferences.AtoZfolderUsed == DataStorageType.InDatabase)
            {
                MsgBox.Show(this, "Claim Forms feature is unavailable when data path A to Z folder is disabled.");
                return;
            }
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormClaimForms FormCF = new FormClaimForms();
            FormCF.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Claim Forms");
        }

        private void menuItemClearinghouses_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormClearinghouses FormC = new FormClearinghouses();
            FormC.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Clearinghouses");
        }

        private void menuItemDiscountPlans_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormDiscountPlans FormDP = new FormDiscountPlans();
            FormDP.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Discount Plans");
        }

        private void menuItemComputers_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormComputers FormC = new FormComputers();
            FormC.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Computers");
        }

        private void menuItemDataPath_Click(object sender, System.EventArgs e)
        {
            //security is handled from within the form.
            FormPath FormP = new FormPath();
            FormP.ShowDialog();
            CheckCustomReports();
            this.RefreshCurrentModule();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Data Path");
        }

        private void menuItemDefaultCCProcs_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormDefaultCCProcs FormD = new FormDefaultCCProcs();
            FormD.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Default CC Procedures");
        }

        private void menuItemDefinitions_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormDefinitions FormD = new FormDefinitions(DefinitionCategory.AccountColors);//just the first cat.
            FormD.ShowDialog();
            RefreshCurrentModule(true);
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Definitions");
        }

        private void menuItemDentalSchools_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormDentalSchoolSetup FormDSS = new FormDentalSchoolSetup();
            FormDSS.ShowDialog();
            RefreshCurrentModule();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Dental Schools");
        }

        private void menuItemDisplayFields_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormDisplayFieldCategories FormD = new FormDisplayFieldCategories();
            FormD.ShowDialog();
            RefreshCurrentModule(true);
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Display Fields");
        }

        private void menuItemEnterprise_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormEnterpriseSetup FormES = new FormEnterpriseSetup();
            FormES.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Enterprise");
        }

        private void menuItemEmail_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormEmailAddresses FormEA = new FormEmailAddresses();
            FormEA.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Email");
        }

        private void menuItemEHR_Click(object sender, EventArgs e)
        {
            //if(!Security.IsAuthorized(Permissions.Setup)) {
            //  return;
            //}
            FormEhrSetup FormE = new FormEhrSetup();
            FormE.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "EHR");
        }

        private void menuItemFeeScheds_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.FeeSchedEdit))
            {
                return;
            }
            FormFeeScheds FormF = new FormFeeScheds(false);
            FormF.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.FeeSchedEdit, 0, "Fee Schedules");
        }

        private void menuItemFHIR_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormFHIRSetup FormFS = new FormFHIRSetup();
            FormFS.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "FHIR");
        }

        private void menuItemHL7_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormHL7Defs FormH = new FormHL7Defs();
            FormH.CurPatNum = CurPatNum;
            FormH.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "HL7");
        }

        private void menuItemImaging_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormImagingSetup FormI = new FormImagingSetup();
            FormI.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Imaging");
        }

        private void menuItemInsCats_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormInsCatsSetup FormE = new FormInsCatsSetup();
            FormE.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Insurance Categories");
        }

        private void menuItemInsFilingCodes_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormInsFilingCodes FormF = new FormInsFilingCodes();
            FormF.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Insurance Filing Codes");
        }

        private void menuItemLaboratories_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }

            if (Plugin.Trigger(this, "FormOpenDental_MenuItem_Laboratories")) return;

            FormLaboratories FormL = new FormLaboratories();
            FormL.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Laboratories");
        }

        private void menuItemLetters_Click(object sender, EventArgs e)
        {
            FormLetters FormL = new FormLetters();
            FormL.ShowDialog();
        }

        private void menuItemMessaging_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormMessagingSetup FormM = new FormMessagingSetup();
            FormM.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Messaging");
        }

        private void menuItemMessagingButs_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormMessagingButSetup FormM = new FormMessagingButSetup();
            FormM.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Messaging");
        }

        private void menuItemMisc_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormMisc FormM = new FormMisc();
            if (FormM.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            if (Preference.GetInt(PreferenceName.ProcessSigsIntervalInSecs) == 0)
            {
                timerSignals.Enabled = false;
            }
            else
            {
                timerSignals.Interval = Preference.GetInt(PreferenceName.ProcessSigsIntervalInSecs) * 1000;
                timerSignals.Enabled = true;
            }

            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Misc");
        }

        ///<summary>Only used when theme is changed.</summary>
        private void RecursiveInvalidate(Control control)
        {
            foreach (Control c in control.Controls)
            {
                RecursiveInvalidate(c);
            }
            control.Invalidate();
        }

        private void menuItemModules_Click(object sender, EventArgs e)
        {
            LaunchModuleSetupWithTab(0);//Default to Appts tab.
        }

        private void menuItemOrtho_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormOrthoSetup FormOS = new FormOrthoSetup();
            FormOS.ShowDialog();
        }

        private void menuItemPreferencesAppts_Click(object sender, EventArgs e)
        {
            LaunchModuleSetupWithTab(0);
        }

        private void menuItemPreferencesFamily_Click(object sender, EventArgs e)
        {
            LaunchModuleSetupWithTab(1);
        }

        private void menuItemPreferencesAccount_Click(object sender, EventArgs e)
        {
            LaunchModuleSetupWithTab(2);
        }

        private void menuItemPreferencesTreatPlan_Click(object sender, EventArgs e)
        {
            LaunchModuleSetupWithTab(3);
        }

        private void menuItemPreferencesChart_Click(object sender, EventArgs e)
        {
            LaunchModuleSetupWithTab(4);
        }

        private void menuItemPreferencesImages_Click(object sender, EventArgs e)
        {
            LaunchModuleSetupWithTab(5);
        }

        private void menuItemPreferencesManage_Click(object sender, EventArgs e)
        {
            LaunchModuleSetupWithTab(6);
        }

        ///<summary>Checks setup permission, launches the module setup window with the specified tab and then makes an audit entry.
        ///This is simply a helper method because every preferences menu item will do the exact same code.</summary>
        private void LaunchModuleSetupWithTab(int selectedTab)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormModuleSetup FormM = new FormModuleSetup(selectedTab);
            if (FormM.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            FillPatientButton(Patients.GetPat(CurPatNum));
            RefreshCurrentModule(true);
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Modules");
        }

        private void menuItemOperatories_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormOperatories FormO = new FormOperatories();
            FormO.ShowDialog();
            if (FormO.ListConflictingAppts.Count > 0)
            {
                FormApptConflicts FormAC = new FormApptConflicts(FormO.ListConflictingAppts);
                FormAC.Show();
                FormAC.BringToFront();
            }
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Operatories");
        }

        private void menuItemPatFieldDefs_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormPatFieldDefs FormP = new FormPatFieldDefs();
            FormP.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Patient Field Defs");
        }

        private void menuItemPayerIDs_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormElectIDs FormE = new FormElectIDs();
            FormE.IsSelectMode = false;
            FormE.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Payer IDs");
        }

        private void menuItemPractice_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormPractice FormPr = new FormPractice();
            FormPr.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Practice Info");
            if (FormPr.DialogResult != DialogResult.OK)
            {
                return;
            }
            // TODO: myOutlookBar.RefreshButtons();
            RefreshCurrentModule();
        }

        private void menuItemProblems_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormDiseaseDefs FormD = new FormDiseaseDefs();
            FormD.ShowDialog();
            //RefreshCurrentModule();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Disease Defs");
        }

        private void menuItemProcedureButtons_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormProcButtons FormPB = new FormProcButtons();
            FormPB.Owner = this;
            FormPB.ShowDialog();
            SetModuleSelected();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Procedure Buttons");
        }

        private void menuItemLinks_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormProgramLinks FormPL = new FormProgramLinks();
            FormPL.ShowDialog();
            ContrChart2.InitializeLocalData();//for eCW
            LayoutToolBar();
            RefreshMenuReports();
            if (CurPatNum > 0)
            {
                Patient pat = Patients.GetPat(CurPatNum);
                FillPatientButton(pat);
            }
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Program Links");
        }

        /*
		private void menuItem_ProviderAllocatorSetup_Click(object sender,EventArgs e) {
			// Check Permissions
			if(!Security.IsAuthorized(Permissions.Setup)) {
				// Failed security prompts message box. Consider adding overload to not show message.
				//MessageBox.Show("Not Authorized to Run Setup for Provider Allocation Tool");
				return;
			}
			Reporting.Allocators.MyAllocator1.FormInstallAllocator_Provider fap = new OpenDental.Reporting.Allocators.MyAllocator1.FormInstallAllocator_Provider();
			fap.ShowDialog();
		}*/

        private void menuItemAsapList_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormAsapSetup FormAS = new FormAsapSetup();
            FormAS.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "ASAP List Setup");
        }

        private void menuItemConfirmations_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormConfirmationSetup FormCS = new FormConfirmationSetup();
            FormCS.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Confirmation Setup");
        }

        private void menuItemInsVerify_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormInsVerificationSetup FormIV = new FormInsVerificationSetup();
            FormIV.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Insurance Verification");
        }

        private void menuItemQuestions_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormQuestionDefs FormQ = new FormQuestionDefs();
            FormQ.ShowDialog();
            //RefreshCurrentModule();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Questionnaire");
        }

        private void menuItemRecall_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormRecallSetup FormRS = new FormRecallSetup();
            FormRS.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Recall");
        }

        private void menuItemRecallTypes_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormRecallTypes FormRT = new FormRecallTypes();
            FormRT.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Recall Types");
        }

        private void menuItemReactivation_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormReactivationSetup FormRS = new FormReactivationSetup();
            FormRS.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Reactivation");
        }

        private void menuItemReplication_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.ReplicationSetup))
            {
                return;
            }
            FormReplicationSetup FormRS = new FormReplicationSetup();
            FormRS.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.ReplicationSetup, 0, "Replication setup.");
        }

        private void menuItemReports_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormReportSetup FormRS = new FormReportSetup(0, false);
            FormRS.ShowDialog();
        }

        private void menuItemRequiredFields_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormRequiredFields FormRF = new FormRequiredFields();
            FormRF.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Required Fields");
        }

        private void menuItemRequirementsNeeded_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormReqNeededs FormR = new FormReqNeededs();
            FormR.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Requirements Needed");
        }

        private void menuItemSched_Click(object sender, EventArgs e)
        {
            //anyone should be able to view. Security must be inside schedule window.
            //if(!Security.IsAuthorized(Permissions.Schedules)) {
            //	return;
            //}
            FormSchedule FormS = new FormSchedule();
            FormS.ShowDialog();
            //SecurityLogs.MakeLogEntry(Permissions.Schedules,0,"");
        }

        /*private void menuItemBlockoutDefault_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Blockouts)) {
				return;
			}
			FormSchedDefault FormSD=new FormSchedDefault(ScheduleType.Blockout);
			FormSD.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Blockouts,0,"Default");
		}*/

        public static void S_MenuItemSecurity_Click(object sender, EventArgs e)
        {
            _formOpenDentalS.menuItemSecuritySettings_Click(sender, e);
        }

        private void menuItemSecuritySettings_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.SecurityAdmin))
            {
                return;
            }
            FormSecurity FormS = new FormSecurity();
            FormS.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin, 0, "Security Window");
            if (!Preferences.HasClinicsEnabled)
            {//clinics not enabled, refresh current module and return
                RefreshCurrentModule();
                return;
            }
            //clinics is enabled
            if (Security.CurUser.ClinicIsRestricted)
            {
                Clinics.ClinicNum = Security.CurUser.ClinicNum;
            }
            Text = PatientL.GetMainTitle(Patients.GetPat(CurPatNum), Clinics.ClinicNum);
            SetSmsNotificationText();
            RefreshMenuClinics();//this calls ModuleSelected, so no need to call RefreshCurrentModule
            RefreshMenuDashboards();
        }

        private void menuItemAddUser_Click(object sender, EventArgs e)
        {
            bool isAuthorizedAddNewUser = Security.IsAuthorized(Permissions.AddNewUser, true);
            bool isAuthorizedSecurityAdmin = Security.IsAuthorized(Permissions.SecurityAdmin, true);
            if (!(isAuthorizedAddNewUser || isAuthorizedSecurityAdmin))
            {
                MsgBox.Show(this, "Not authorized to add a new user.");
                return;
            }
            if (Preference.GetLong(PreferenceName.DefaultUserGroup) == 0)
            {
                if (isAuthorizedSecurityAdmin)
                {
                    //Prompt to go to form.
                    string msg = "Default user group is not set.  Would you like to set the default user group now?";
                    if (MsgBox.Show(this, MsgBoxButtons.YesNo, msg, "Default user group"))
                    {
                        FormGlobalSecurity FormGS = new FormGlobalSecurity();
                        FormGS.ShowDialog();//No refresh needed; Signals sent from this form.
                    }
                }
                else
                {
                    //Using verbage similar to that found in the manual for describing how to navigate to a window in the program.
                    string msg = "Default user group is not set.  A user with the SecurityAdmin permission must set a default user group.  "
                        + "To view the default user group, in the Main Menu, click Setup, Security, Security Settings, Global Security Settings.";
                    MsgBox.Show(this, msg, "Default user group");
                }
                return;
            }
            FormUserEdit FormUE = new FormUserEdit(new User(), true);
            FormUE.IsNew = true;
            FormUE.ShowDialog();
        }

        private void menuItemSheets_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormSheetDefs FormSD = new FormSheetDefs();
            FormSD.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Sheets");
        }

        //This shows as "Show Features"
        private void menuItemEasy_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormShowFeatures FormE = new FormShowFeatures();
            FormE.ShowDialog();
            ContrAccount2.LayoutToolBar();//for repeating charges
            RefreshCurrentModule(true);
            //Show enterprise setup if it was enabled
            menuItemEnterprise.Visible = Preference.GetBool(PreferenceName.ShowFeatureEnterprise);
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Show Features");
        }

        private void menuItemSpellCheck_Click(object sender, EventArgs e)
        {
            FormSpellCheck FormD = new FormSpellCheck();
            FormD.ShowDialog();
        }

        private void menuItemTimeCards_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormTimeCardSetup FormTCS = new FormTimeCardSetup();
            FormTCS.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Time Card Setup");
        }

        private void menuItemTask_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormTaskPreferences formTaskSetup = new FormTaskPreferences();
            if (formTaskSetup.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            if (userControlTasks1.Visible)
            {
                userControlTasks1.InitializeOnStartup();
            }
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Task");
        }

        private void menuItemQuickPasteNotes_Click(object sender, EventArgs e)
        {
            FormQuickPaste formQP = new FormQuickPaste(true);
            formQP.QuickType = QuickPasteType.None;
            formQP.ShowDialog();
        }

        #endregion

        #region Lists

        //Lists
        private void menuItemProcCodes_Click(object sender, System.EventArgs e)
        {
            //security handled within form
            FormProcCodes FormP = new FormProcCodes(true);
            FormP.ProcCodeSort = (ProcCodeListSort)Preference.GetInt(PreferenceName.ProcCodeListSortOrder);
            FormP.ShowDialog();
        }

        private void menuItemAllergies_Click(object sender, EventArgs e)
        {
            new FormAllergySetup().ShowDialog();
        }

        private void menuItemClinics_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormClinics FormC = new FormClinics();
            FormC.IncludeHQInList = true;
            FormC.IsMultiSelect = true;
            FormC.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Clinics");
            //this menu item is only visible if the clinics show feature is enabled (!EasyNoClinics)
            if (Clinics.GetDesc(Clinics.ClinicNum) == "")
            {//will be empty string if ClinicNum is not valid, in case they deleted the clinic
                Clinics.ClinicNum = Security.CurUser.ClinicNum;
                SetSmsNotificationText();
                Text = PatientL.GetMainTitle(Patients.GetPat(CurPatNum), Clinics.ClinicNum);
            }
            RefreshMenuClinics();
            //reset the main title bar in case the user changes the clinic description for the selected clinic
            Patient pat = Patients.GetPat(CurPatNum);
            Text = PatientL.GetMainTitle(pat, Clinics.ClinicNum);
            //reset the tip text in case the user changes the clinic description
        }

        private void menuItemContacts_Click(object sender, System.EventArgs e)
        {
            FormContacts FormC = new FormContacts();
            FormC.ShowDialog();
        }

        private void menuItemCounties_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormCounties FormC = new FormCounties();
            FormC.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Counties");
        }

        private void menuItemSchoolClass_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormSchoolClasses FormS = new FormSchoolClasses();
            FormS.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Dental School Classes");
        }

        private void menuItemSchoolCourses_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormSchoolCourses FormS = new FormSchoolCourses();
            FormS.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Dental School Courses");
        }

        private void menuItemEmployees_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormEmployeeSelect FormEmp = new FormEmployeeSelect();
            FormEmp.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Employees");
        }

        private void menuItemEmployers_Click(object sender, System.EventArgs e)
        {
            FormEmployers FormE = new FormEmployers();
            FormE.ShowDialog();
        }

        private void menuItemInstructors_Click(object sender, System.EventArgs e)
        {
            /*if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormInstructors FormI=new FormInstructors();
			FormI.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Dental School Instructors");*/
        }

        private void menuItemCarriers_Click(object sender, System.EventArgs e)
        {
            FormCarriers FormC = new FormCarriers();
            FormC.ShowDialog();
            RefreshCurrentModule();
        }

        private void menuItemInsPlans_Click(object sender, System.EventArgs e)
        {
            FormInsPlans FormIP = new FormInsPlans();
            FormIP.ShowDialog();
            RefreshCurrentModule(true);
        }

        private void menuItemLabCases_Click(object sender, EventArgs e)
        {
            FormLabCases FormL = new FormLabCases();
            FormL.ShowDialog();
            if (FormL.GoToAptNum != 0)
            {
                Appointment apt = Appointments.GetOneApt(FormL.GoToAptNum);
                Patient pat = Patients.GetPat(apt.PatNum);
                S_Contr_PatientSelected(pat, false);
                //OnPatientSelected(pat.PatNum,pat.GetNameLF(),pat.Email!="",pat.ChartNumber);
                GotoModule.GotoAppointment(apt.AptDateTime, apt.AptNum);
            }
        }

        private void menuItemMedications_Click(object sender, System.EventArgs e)
        {
            FormMedications FormM = new FormMedications();
            FormM.ShowDialog();
        }

        private void menuItemPharmacies_Click(object sender, EventArgs e)
        {
            FormPharmacies FormP = new FormPharmacies();
            FormP.ShowDialog();
        }

        private void menuItemProviders_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Providers, true) && !Security.IsAuthorized(Permissions.AdminDentalStudents, true))
            {
                MessageBox.Show(Lans.g("Security", "Not authorized for") + "\r\n"
                    + GroupPermissions.GetDesc(Permissions.Providers) + " " + Lans.g("Security", "or") + " " + GroupPermissions.GetDesc(Permissions.AdminDentalStudents));
                return;
            }
            FormProviderSetup FormPS = new FormProviderSetup();
            FormPS.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Providers");
        }

        private void menuItemPrescriptions_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormRxSetup FormRxSetup2 = new FormRxSetup();
            FormRxSetup2.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Rx");
        }

        private void menuItemReferrals_Click(object sender, System.EventArgs e)
        {
            FormReferralSelect FormRS = new FormReferralSelect();
            FormRS.ShowDialog();
        }

        private void menuItemSites_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormSites FormS = new FormSites();
            FormS.ShowDialog();
            RefreshCurrentModule();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Sites");
        }

        private void menuItemStateAbbrs_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormStateAbbrs formSA = new FormStateAbbrs();
            formSA.ShowDialog();
            RefreshCurrentModule();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "StateAbbrs");
        }

        private void menuItemZipCodes_Click(object sender, System.EventArgs e)
        {
            //if(!Security.IsAuthorized(Permissions.Setup)){
            //	return;
            //}
            FormZipCodes FormZ = new FormZipCodes();
            FormZ.ShowDialog();
            //SecurityLogs.MakeLogEntry(Permissions.Setup,"Zip Codes");
        }

        #endregion

        #region Reports

        private void menuItemReportsStandard_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Reports))
            {
                return;
            }
            FormReportsMore FormR = new FormReportsMore();
            FormR.ShowDialog();
            NonModalReportSelectionHelper(FormR.RpNonModalSelection);
        }

        private void menuItemReportsGraphic_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.GraphicalReports))
            {
                return;
            }
            if (_formDashboardEditTab != null)
            {
                _formDashboardEditTab.BringToFront();
                return;
            }
            //on extremely large dbs, the ctor can take a few seconds to load, so show the wait cursor.
            Cursor = Cursors.WaitCursor;
            //Check if the user has permission to view all providers in production and income reports
            bool hasAllProvsPermission = Security.IsAuthorized(Permissions.ReportProdIncAllProviders, true);
            if (!hasAllProvsPermission && Security.CurUser.ProvNum == 0)
            {
                if (!MsgBox.Show(this, true, "The current user must be a provider or have the 'All Providers' permission to view provider reports. Continue?"))
                {
                    return;
                }
            }
            _formDashboardEditTab = new OpenDentalGraph.FormDashboardEditTab(Security.CurUser.ProvNum, !Security.IsAuthorized(Permissions.ReportProdIncAllProviders, true)) { IsEditMode = false };
            _formDashboardEditTab.FormClosed += new FormClosedEventHandler((object senderF, FormClosedEventArgs eF) => { _formDashboardEditTab = null; });
            Cursor = Cursors.Default;
            _formDashboardEditTab.Show();
        }

        private void menuItemReportsUserQuery_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.UserQuery))
            {
                return;
            }
            if (Security.IsAuthorized(Permissions.UserQueryAdmin, true))
            {
                SecurityLogs.MakeLogEntry(Permissions.UserQuery, 0, Lan.g(this, "User query form accessed."));
                if (_formUserQuery != null)
                {
                    _formUserQuery.BringToFront();
                    return;
                }
                _formUserQuery = new FormQuery(null);
                _formUserQuery.FormClosed += new FormClosedEventHandler((object senderF, FormClosedEventArgs eF) => { _formUserQuery = null; });
                _formUserQuery.Show(this);
            }
            else
            {
                FormQueryFavorites FormQF = new FormQueryFavorites();
                FormQF.ShowDialog();
                if (FormQF.DialogResult == DialogResult.OK)
                {
                    ExecuteQueryFavorite(FormQF.UserQueryCur);
                }
            }
        }

        private void menuItemReportsUnfinalizedPay_Click(object sender, EventArgs e)
        {
            FormRpUnfinalizedInsPay formRp = new FormRpUnfinalizedInsPay();
            formRp.ShowDialog();
        }

        private void UpdateUnfinalizedPayCount(List<Signalod> listSignals)
        {
            if (listSignals.Count == 0)
            {
                menuItemReportsUnfinalizedPay.Text = Lan.g(this, "Unfinalized Payments");
                return;
            }
            Signalod signal = listSignals.OrderByDescending(x => x.SigDateTime).First();
            menuItemReportsUnfinalizedPay.Text = Lan.g(this, "Unfinalized Payments") + ": " + signal.MsgValue;
        }

        private void RefreshMenuReports()
        {
            //Find the index of the last separator which separates the static menu items from the dynamic menu items.
            int separatorIndex = -1;
            for (int i = 0; i < menuItemReportsHeader.MenuItems.Count; i++)
            {
                if (menuItemReportsHeader.MenuItems[i].Text == "-")
                {
                    separatorIndex = i;
                }
            }
            //Remove dynamic items and separator.  Leave hard coded items.
            if (separatorIndex != -1)
            {
                for (int i = menuItemReportsHeader.MenuItems.Count - 1; i >= separatorIndex; i--)
                {
                    menuItemReportsHeader.MenuItems.RemoveAt(i);
                }
            }
            List<ToolButItem> listToolButItems = ToolButItems.GetForToolBar(ToolBarsAvail.ReportsMenu);
            if (listToolButItems.Count == 0)
            {
                return;//Return early to avoid adding a useless separator in the menu.
            }
            //Add separator, then dynamic items to the bottom of the menu.
            menuItemReportsHeader.MenuItems.Add("-");//Separator			
            listToolButItems.Sort(ToolButItem.Compare);//Alphabetical order
            foreach (ToolButItem toolButItemCur in listToolButItems)
            {
                MenuItem menuItem = new MenuItem(toolButItemCur.ButtonText, menuReportLink_Click);
                menuItem.Tag = toolButItemCur;
                menuItemReportsHeader.MenuItems.Add(menuItem);
            }
        }

        private void menuReportLink_Click(object sender, System.EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            ToolButItem toolButItemCur = ((ToolButItem)menuItem.Tag);
            ProgramL.Execute(toolButItemCur.ProgramNum, Patients.GetPat(CurPatNum));
        }

        #endregion

        #region CustomReports

        //Custom Reports
        private void menuItemRDLReport_Click(object sender, System.EventArgs e)
        {
            //This point in the code is only reached if the A to Z folders are enabled, thus
            //the image path should exist.
            FormReportCustom FormR = new FormReportCustom();
            FormR.SourceFilePath =
                Storage.Default.CombinePath(Preference.GetString(PreferenceName.ReportFolderName), ((MenuItem)sender).Text + ".rdl");
            FormR.ShowDialog();
        }

        #endregion

        #region Tools

        //Tools
        private void menuItemPrintScreen_Click(object sender, System.EventArgs e)
        {
            FormPrntScrn FormPS = new FormPrntScrn();
            FormPS.ShowDialog();
        }

        #region MiscTools
        private void menuItemDuplicateBlockouts_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormBlockoutDuplicatesFix form = new FormBlockoutDuplicatesFix();
            Cursor = Cursors.WaitCursor;
            form.ShowDialog();
            Cursor = Cursors.Default;
            //Security log entries are made from within the form.
        }

        private void menuItemCreateAtoZFolders_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormAtoZFoldersCreate FormA = new FormAtoZFoldersCreate();
            FormA.ShowDialog();
            //Security log entries are made from within the form.
        }

        private void menuItemDatabaseMaintenancePat_Click(object sender, EventArgs e)
        {
            //Purposefully not checking permissions.  All users need the ability to call patient specific DBMs ATM.
            //Get all patient specific DBM methods via reflection.
            List<MethodInfo> listPatDbmMethods = DatabaseMaintenances.GetMethodsForDisplay(Clinics.ClinicNum, true);
            //Add any missing patient specific DBM methods to the database that are not currently present.
            DatabaseMaintenances.InsertMissingDBMs(listPatDbmMethods.Select(x => x.Name).ToList());
            //Get the names of all DBM methods that are not hidden.
            List<string> listNonHiddenDbmMethods = DatabaseMaintenances.GetAll()
                .FindAll(x => x.IsHidden == false && x.IsOld == false)
                .Select(y => y.MethodName).ToList();
            //Filter down our list of patient specific DBM methods found via reflection based on hidden status.
            listPatDbmMethods.RemoveAll(x => !x.Name.In(listNonHiddenDbmMethods));
            if (listPatDbmMethods.Count == 0)
            {
                MsgBox.Show(this, "All patient database maintenance methods are marked as hidden.");
                return;
            }
            FormDatabaseMaintenancePat FormDMP = new FormDatabaseMaintenancePat(listPatDbmMethods, CurPatNum);
            FormDMP.ShowDialog();
        }

        private void menuItemMergeDPs_Click(object sender, EventArgs e)
        {
            FormDiscountPlanMerge FormDPM = new FormDiscountPlanMerge();
            FormDPM.ShowDialog();
        }

        private void menuItemMergeMedications_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.MedicationMerge))
            {
                return;
            }
            FormMedicationMerge FormMM = new FormMedicationMerge();
            FormMM.ShowDialog();
            //Securitylog entries are handled within the form.
        }

        private void menuItemMergePatients_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.PatientMerge))
            {
                return;
            }
            FormPatientMerge fpm = new FormPatientMerge();
            fpm.ShowDialog();
            //Security log entries are made from within the form.
        }

        private void menuItemMergeReferrals_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.ReferralMerge))
            {
                return;
            }
            FormReferralMerge FormRM = new FormReferralMerge();
            FormRM.ShowDialog();
            //Security log entries are made from within the form.
        }

        private void menuItemMergeProviders_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.ProviderMerge))
            {
                return;
            }
            FormProviderMerge FormPM = new FormProviderMerge();
            FormPM.ShowDialog();
            //Security log entries are made from within the form.
        }

        private void menuItemMoveSubscribers_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.InsPlanChangeSubsc))
            {
                return;
            }
            FormSubscriberMove formSM = new FormSubscriberMove();
            formSM.ShowDialog();
            //Security log entries are made from within the form.
        }

        private void menuPatientStatusSetter_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.SecurityAdmin))
            {
                return;
            }
            FormPatientStatusTool formPST = new FormPatientStatusTool();
            formPST.ShowDialog();
            //Security log entries are made from within the form.
        }

        private void menuItemProcLockTool_Click(object sender, EventArgs e)
        {
            FormProcLockTool FormT = new FormProcLockTool();
            FormT.ShowDialog();
            //security entries made inside the form
            //SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Proc Lock Tool");
        }

        private void menuItemSetupWizard_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormSetupWizard FormSW = new FormSetupWizard();
            FormSW.ShowDialog();
        }

        private void menuItemShutdown_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormShutdown FormS = new FormShutdown();
            FormS.ShowDialog();
            if (FormS.DialogResult != DialogResult.OK)
            {
                return;
            }
            //turn off signal reception for 5 seconds so this workstation will not shut down.
            Signalods.SignalLastRefreshed = MiscData.GetNowDateTime().AddSeconds(5);
            Signalod sig = new Signalod();
            sig.IType = InvalidType.ShutDownNow;
            Signalods.Insert(sig);
            Computer.ClearAllHeartBeats(Environment.MachineName);//always assume success
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Shutdown all workstations.");
        }

        private void menuTelephone_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormTelephone FormT = new FormTelephone();
            FormT.ShowDialog();
            //Security log entries are made from within the form.
        }

        private void menuItemTestLatency_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormTestLatency formTL = new FormTestLatency();
            formTL.ShowDialog();
        }

        private void menuItemXChargeReconcile_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Accounting))
            {
                return;
            }
            FormXChargeReconcile FormXCR = new FormXChargeReconcile();
            FormXCR.ShowDialog();
        }
        #endregion MiscTools

        private void menuItemAging_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormAging FormAge = new FormAging();
            FormAge.ShowDialog();
        }

        private void menuItemAuditTrail_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.AuditTrail))
            {
                return;
            }
            FormAudit FormA = new FormAudit();
            FormA.CurPatNum = CurPatNum;
            FormA.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.AuditTrail, 0, "Audit Trail");
        }

        private void menuItemFinanceCharge_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormFinanceCharges FormFC = new FormFinanceCharges();
            FormFC.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Run Finance Charges");
        }

        private void menuItemCCRecurring_Click(object sender, EventArgs e)
        {
            if (FormCRC == null || FormCRC.IsDisposed)
            {
                FormCRC = new FormCreditRecurringCharges();
            }
            Cursor = Cursors.WaitCursor;
            FormCRC.Show();
            Cursor = Cursors.Default;
            if (FormCRC.WindowState == FormWindowState.Minimized)
            {
                FormCRC.WindowState = FormWindowState.Normal;
            }
            FormCRC.BringToFront();
        }

        private void menuItemDatabaseMaintenance_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormDatabaseMaintenance FormDM = new FormDatabaseMaintenance();
            FormDM.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Database Maintenance");
        }

        private void menuItemDispensary_Click(object sender, System.EventArgs e)
        {
            FormDispensary FormD = new FormDispensary();
            FormD.ShowDialog();
        }

        private void menuItemEvaluations_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.AdminDentalEvaluations, true) && (Security.CurUser.ProvNum == 0 || Providers.GetProv(Security.CurUser.ProvNum).SchoolClassNum != 0))
            {
                MsgBox.Show(this, "Only Instructors may view or edit evaluations.");
                return;
            }
            FormEvaluations FormE = new FormEvaluations();
            FormE.ShowDialog();
        }

        private void menuItemTerminal_Click(object sender, EventArgs e)
        {
            if (Preference.GetLong(PreferenceName.ProcessSigsIntervalInSecs) == 0)
            {
                MsgBox.Show(this, "Cannot open terminal unless process signal interval is set. To set it, go to Setup > Miscellaneous.");
                return;
            }
            FormTerminal FormT = new FormTerminal();
            FormT.ShowDialog();
            Application.Exit();//always close after coming out of terminal mode as a safety precaution.*/
        }

        private void menuItemTerminalManager_Click(object sender, EventArgs e)
        {
            if (formTerminalManager == null || formTerminalManager.IsDisposed)
            {
                formTerminalManager = new FormTerminalManager();
            }
            formTerminalManager.Show();
            formTerminalManager.BringToFront();
        }

        private void menuItemTranslation_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            FormTranslationCat FormTC = new FormTranslationCat();
            FormTC.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Translations");
        }

        private void menuItemMobileSetup_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
        }

        private void menuItemNewCropBilling_Click(object sender, EventArgs e)
        {
            FormNewCropBilling FormN = new FormNewCropBilling();
            FormN.ShowDialog();
        }

        private void menuItemPendingPayments_Click(object sender, EventArgs e)
        {
            FormPendingPayments FormPP = new FormPendingPayments();
            FormPP.Show();//Non-modal so the user can view the patient's account
        }

        private void menuItemRepeatingCharges_Click(object sender, System.EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.RepeatChargeTool))
            {
                return;
            }
            FormRepeatChargesUpdate FormR = new FormRepeatChargesUpdate();
            FormR.ShowDialog();
        }

        private void menuItemScreening_Click(object sender, System.EventArgs e)
        {
            FormScreenGroups FormS = new FormScreenGroups();
            FormS.ShowDialog();
        }

        private void menuItemReqStudents_Click(object sender, EventArgs e)
        {
            Provider prov = Providers.GetProv(Security.CurUser.ProvNum);
            if (prov == null)
            {
                MsgBox.Show(this, "The current user is not attached to a provider. Attach the user to a provider to gain access to this feature.");
                return;
            }
            if (!prov.IsInstructor)
            {//if a student is logged in
             //the student always has permission to view their own requirements
                FormReqStudentOne FormO = new FormReqStudentOne();
                FormO.ProvNum = prov.ProvNum;
                FormO.ShowDialog();
                return;
            }
            if (prov.IsInstructor)
            {
                FormReqStudentsMany FormM = new FormReqStudentsMany();
                FormM.ShowDialog();
            }
        }

        private void menuItemWiki_Click(object sender, EventArgs e)
        {
            if (Plugin.Filter(this, "FormOpenDental_MenuItem_Wiki", false))
            {
                return;
            }
            new FormWiki().Show();
        }

        private void menuItemXWebTrans_Click(object sender, EventArgs e)
        {
            if (FormXWT == null || FormXWT.IsDisposed)
            {
                FormXWT = new FormXWebTransactions();
                FormXWT.FormClosed += new FormClosedEventHandler((o, e1) => { FormXWT = null; });
                FormXWT.Show();
            }
            if (FormXWT.WindowState == FormWindowState.Minimized)
            {
                FormXWT.WindowState = FormWindowState.Normal;
            }
            FormXWT.BringToFront();
        }

        public static void S_WikiLoadPage(string pageTitle)
        {
            if (!Preference.GetBool(PreferenceName.WikiCreatePageFromLink) && !WikiPages.CheckPageNamesExist(new List<string> { pageTitle })[0])
            {
                MsgBox.Show("FormOpenDental", "Wiki page does not exist.");
                return;
            }
            FormWiki FormW = new FormWiki();
            FormW.Show();
            FormW.LoadWikiPagePublic(pageTitle);//This has to be after the form has loaded
        }

        private void menuItemAutoClosePayPlans_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup))
            {
                return;
            }
            if (!MsgBox.Show(this, MsgBoxButtons.OKCancel, "Running this tool will automatically mark all payment plans that have"
                + " been paid off and have no future charges as closed.  Do you want to continue?"))
            {
                return;
            }
            long plansClosed = PayPlans.AutoClose(); //returns # of payplans closed.
            string msgText;
            if (plansClosed > 0)
            {
                msgText = Lan.g(this, "Success.") + "  " + plansClosed + " " + Lan.g(this, "plan(s) closed.");
            }
            else
            {
                msgText = Lan.g(this, "There were no plans to close.");
            }
            MessageBox.Show(msgText);
        }

        private void menuItemOrthoAuto_Click(object sender, EventArgs e)
        {
            FormOrthoAutoClaims FormOAC = new FormOrthoAutoClaims();
            FormOAC.ShowDialog();
        }

        #endregion

        #region Clinics
        //menuClinics is a dynamic menu that is maintained within RefreshMenuClinics()
        #endregion

        #region Dashboard
        private void RefreshMenuDashboards()
        {
            List<SheetDef> listDashboards = SheetDefs.GetWhere(x => x.SheetType == SheetTypeEnum.PatientDashboardWidget
                  && Security.IsAuthorized(Permissions.DashboardWidget, x.SheetDefNum, true), true);
            bool isAuthorizedForSetup = Security.IsAuthorized(Permissions.Setup, true);
            this.InvokeIfRequired(() =>
            {
                menuItemDashboard.MenuItems.Clear();
                if (listDashboards.Count > 28)
                {//This number of items+line+Setup will fit in a 990x735 form.
                    FormDashboardWidgets formDashboards = new FormDashboardWidgets();//Open the LaunchDashboard window.
                    if (formDashboards.ShowDialog() == DialogResult.OK)
                    {
                        LaunchWidget(formDashboards.SheetDefDashboardWidget, new Point(0, 0));
                    }
                    return;
                }
                List<long> listOpenDashboardsSheetDefNums = userControlPatientDashboard.ListOpenWidgets.Select(x => x.SheetDefWidget.SheetDefNum).ToList();
                MenuItem menuItem;
                foreach (SheetDef dashboardDef in listDashboards)
                {
                    menuItem = new MenuItem(dashboardDef.Description, DashboardMenuClick);
                    menuItem.Tag = dashboardDef;
                    if (dashboardDef.SheetDefNum.In(listOpenDashboardsSheetDefNums))
                    {//Currently open Dashboard.
                        menuItem.Checked = true;
                    }
                    menuItemDashboard.MenuItems.Add(menuItem);
                }
                if (listDashboards.Count > 0)
                {
                    menuItemDashboard.MenuItems.Add(new MenuItem("-"));
                }
                menuItem = new MenuItem("Setup Dashboards", OpenDashboardSetup);
                if (!isAuthorizedForSetup)
                {
                    menuItem.Enabled = false;
                }
                menuItemDashboard.MenuItems.Add(menuItem);
            });
        }

        private void OpenDashboardSetup(object sender, System.EventArgs e)
        {
            FormDashboardWidgetSetup formDS = new FormDashboardWidgetSetup();
            formDS.ShowDialog();
        }

        ///<summary>Opens a UserControlDashboardWidget, or closes the corresponding UserControlDashboardWidget if it is already open.</summary>
        private void DashboardMenuClick(object sender, System.EventArgs e)
        {
            if (sender.GetType() != typeof(MenuItem) || ((MenuItem)sender).Tag == null || ((MenuItem)sender).Tag.GetType() != typeof(SheetDef))
            {
                return;
            }
            UserControlDashboardWidget widgetOpen = userControlPatientDashboard.ListOpenWidgets
                .FirstOrDefault(x => x.SheetDefWidget.SheetDefNum == ((SheetDef)((MenuItem)sender).Tag).SheetDefNum);
            if (widgetOpen != null)
            {
                widgetOpen.CloseWidget();
                return;
            }
            LaunchWidget((SheetDef)((MenuItem)sender).Tag, Point.Empty);
        }

        ///<summary>Opens a UserControlDashboardWidget.  The user's permissions should be validated prior to calling this method.</summary>
        private void LaunchWidget(SheetDef sheetDefWidget, Point ptWidgetStartingLocation)
        {
            if (!userControlPatientDashboard.IsInitialized)
            {//Dashboard is not open currently.
                InitDashboards(Security.CurUser.UserNum, CurPatNum, true);
                if (!userControlPatientDashboard.IsInitialized)
                {//Failed to initialize, possibly due to Task docking.
                    return;
                }
            }
            userControlPatientDashboard.AddWidget(sheetDefWidget, ptWidgetStartingLocation);
            RefreshMenuDashboards();
        }

        ///<summary>Determines if there is a user preference for which Dashboard to open on startup, and launches it if the user has permissions to 
        ///launch the dashboard.</summary>
        private void InitDashboards(long userNum, long patNum, bool canCreateNewPref = false)
        {
            UserOdPref userPrefDashboard = UserOdPrefs.GetByUserAndFkeyType(userNum, UserOdFkeyType.Dashboard).FirstOrDefault();
            if (!canCreateNewPref && userPrefDashboard == null)
            {
                return;//User didn't have the dashboard open the last time logged out.
            }
            if (userControlTasks1.Visible && ComputerPrefs.LocalComputer.TaskDock == 1)
            {//Tasks are docked right
                this.InvokeIfRequired(() =>
                {
                    MsgBox.Show(this, "Dashboards are disabled when Tasks are docked to the right.");
                });
                return;
            }
            SheetDef sheetDefDashboard = GetUserDashboard(ref userPrefDashboard);
            long userPrefNum = userPrefDashboard.UserOdPrefNum;//When the DashboardClosing action is called, the reference to the UserOdPref object is lost.
            long sheetDefDashboardNum = sheetDefDashboard.SheetDefNum;
            //Pass in SheetDef describing Dashboard layout.
            userControlPatientDashboard.Initialize(sheetDefDashboard, () => { this.InvokeIfRequired(() => LayoutControls()); }
                , () =>
                {//What to do when the user closes the dashboard.
                    UserOdPrefs.Delete(userPrefNum);
                    SheetDefs.DeleteObject(sheetDefDashboardNum);
                    DataValid.SetInvalid(InvalidType.Sheets);
                    RefreshMenuDashboards();
                    if (ContrAppt2.Visible)
                    {//Ensure appointment view redraws.
                        ContrAppt2.ModuleSelected(CurPatNum);
                    }
                }
            );
            RefreshMenuDashboards();
        }

        ///<summary>Gets the current user's SheetDef Dashboard, or creates one if the current user does not have one defined yet.</summary>
        private static SheetDef GetUserDashboard(ref UserOdPref userPrefDashboard)
        {
            if (userPrefDashboard == null)
            {
                userPrefDashboard = new UserOdPref()
                {
                    UserNum = Security.CurUser.UserNum,
                    Fkey = 0,//Will get set later.
                    FkeyType = UserOdFkeyType.Dashboard,
                    ClinicNum = Clinics.ClinicNum
                };
            }
            long sheetDefDashboardNum = userPrefDashboard.Fkey;//Can't reference userPrefDashboard (because passed by ref) in an anonymous func.
            SheetDef sheetDefDashboard = SheetDefs.GetFirstOrDefault(x => x.SheetDefNum == sheetDefDashboardNum);
            if (sheetDefDashboard == null)
            {
                sheetDefDashboard = new SheetDef()
                {
                    Description = Userods.GetName(userPrefDashboard.UserNum) + "_Dashboard",
                    SheetType = SheetTypeEnum.PatientDashboard,
                    Width = UserControlDashboard.DefaultWidth,
                    Height = UserControlDashboard.DefaultHeight,
                    SheetFieldDefs = new List<SheetFieldDef>(),
                    IsNew = true,
                };
                sheetDefDashboard.SheetDefNum = SheetDefs.InsertOrUpdate(sheetDefDashboard);
                sheetDefDashboard.IsNew = false;//Otherwise, resizing the Dashboard will insert new SheetDefs. 
                userPrefDashboard.Fkey = sheetDefDashboard.SheetDefNum;
                DataValid.SetInvalid(InvalidType.Sheets);
                UserOdPrefs.Upsert(userPrefDashboard);
            }
            return sheetDefDashboard;
        }

        ///<summary>Determines if the Dashboard is currently visible.</summary>
        public static bool IsDashboardVisible
        {
            get
            {
                return (!_formOpenDentalS.splitContainerNoFlickerDashboard.Panel2Collapsed && _formOpenDentalS.userControlPatientDashboard.IsInitialized);
            }
        }
        #endregion

        #region Alerts

        private void menuItemAlerts_Popup(object sender, EventArgs e)
        {
            BeginCheckAlertsThread(false);
        }

        ///<summary>Handles the drawing and coloring for the Alerts menu and its sub items.</summary>
        private void menuItemAlerts_DrawItem(object sender, DrawItemEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            AlertItem alertItem = ((AlertItem)menuItem.Tag);//Can be Null
            Color colorText = SystemColors.MenuText;
            Color backGroundColor = SystemColors.Control;
            if (menuItem == menuItemAlerts)
            {
                if (_listAlertItems != null && _listAlertReads != null)
                {
                    List<long> listAlertItemNums = _listAlertItems.Select(x => x.AlertItemNum).ToList();//All alert nums for current alertItems.
                    List<long> listAlertReadItemNums = _listAlertReads.Select(x => x.AlertItemNum).ToList();//All alert nums for read alertItems.
                    if (!menuItemNoAlerts.Visible && //menuItemNoAlerts is only Visible when there are no AlertItems to show.
                            !listAlertItemNums.All(x => listAlertReadItemNums.Contains(x)))
                    {
                        //Max SeverityType for all unread AlertItems.
                        SeverityType maxSeverity = _listAlertItems.FindAll(x => !listAlertReadItemNums.Contains(x.AlertItemNum)).Select(x => x.Severity).Max();
                        backGroundColor = AlertBackgroudColorHelper(maxSeverity);
                        colorText = AlertTextColorHelper(maxSeverity);
                    }
                    else
                    {//Either there are no AlertItems to show or they all have an AlertRead row.
                        colorText = SystemColors.MenuText;
                    }
                }
            }
            else if (menuItem == menuItemNoAlerts)
            {
                //Keep this menuItem colors as system defaults.
            }
            else
            {//This is an alert menuItem.
                if (!_listAlertReads.Exists(x => x.AlertItemNum == alertItem.AlertItemNum))
                {//User has not acknowleged alert yet.
                    backGroundColor = AlertBackgroudColorHelper(alertItem.Severity);
                    colorText = AlertTextColorHelper(alertItem.Severity);
                }
                else
                {//User has an AlertRead row for this AlertItem.
                    colorText = SystemColors.MenuText;
                }
            }
            if (!menuItem.Enabled || e.State == (DrawItemState.NoAccelerator | DrawItemState.Inactive))
            {
                colorText = SystemColors.ControlDark;
            }
            //Check if selected or hovering over.
            if (e.State == (DrawItemState.NoAccelerator | DrawItemState.Selected)
                || e.State == (DrawItemState.NoAccelerator | DrawItemState.HotLight))
            {
                if (backGroundColor == Color.OrangeRed || backGroundColor == Color.DarkOrange)
                {
                    colorText = Color.Yellow;
                }
                else if (backGroundColor == Color.LightGoldenrodYellow)
                {
                    colorText = Color.OrangeRed;
                }
                else
                {
                    backGroundColor = SystemColors.Highlight;
                    colorText = SystemColors.HighlightText;
                }
            }
            using (SolidBrush brushBackground = new SolidBrush(backGroundColor))
            using (SolidBrush brushFont = new SolidBrush(colorText))
            {
                //Get the text that is displaying from the menu item compenent.
                string menuText = menuItem.Text;
                //Create a string format to center the text to mimic the other menu items.
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                if (menuItem != menuItemAlerts)
                {
                    stringFormat.Alignment = StringAlignment.Near;
                }
                Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);//Copy e.bounds as default.
                if (menuItem != menuItemAlerts)
                {
                    rect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + 30, e.Bounds.Height);//Sub menu items need some extra width.
                }
                e.Graphics.FillRectangle(brushBackground, rect);
                if (menuItem != menuItemAlerts)
                {
                    rect = new Rectangle(e.Bounds.X + 15, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);//Mimic the spacing of other menu items.
                }
                e.Graphics.DrawString(menuText, ODColorTheme.FontMenuItem, brushFont, rect, stringFormat);
            }
        }

        ///<summary>Helper function to determin backgroud color of an AlertItem.</summary>
        private Color AlertBackgroudColorHelper(SeverityType type)
        {
            switch (type)
            {
                default:
                case SeverityType.Normal:
                    return SystemColors.Control;
                case SeverityType.Low:
                    return Color.LightGoldenrodYellow;
                case SeverityType.Medium:
                    return Color.DarkOrange;
                case SeverityType.High:
                    return Color.OrangeRed;
            }
        }

        ///<summary>Helper function to determin text color of an AlertItem.</summary>
        private Color AlertTextColorHelper(SeverityType type)
        {
            switch (type)
            {
                default:
                    return Color.White;
                case SeverityType.Low:
                    return Color.Black;
            }
        }

        private void menuItemAlerts_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            //Measure the text showing.
            MenuItem menuItem = (MenuItem)sender;
            Size sizeString = TextRenderer.MeasureText(menuItem.Text, ODColorTheme.FontMenuItem);
            e.ItemWidth = sizeString.Width;
            if (menuItem != menuItemAlerts)
            {
                e.ItemWidth = sizeString.Width + 15;//Due to logic in menuItemAlerts_DrawItem(...).
            }
            e.ItemHeight = sizeString.Height + 5;//Pad the bottom
        }

        private void menuItemAlerts_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            AlertItem alertItem = (AlertItem)menuItem.Tag;
            if (menuItem.Name == ActionType.MarkAsRead.ToString())
            {
                alertReadsHelper(alertItem);
                BeginCheckAlertsThread(false);
                return;
            }
            if (menuItem.Name == ActionType.Delete.ToString())
            {
                if (!MsgBox.Show(this, MsgBoxButtons.OKCancel, "This will delete the alert for all users. Are you sure you want to delete it?"))
                {
                    return;
                }
                AlertItems.Delete(alertItem.AlertItemNum);
                BeginCheckAlertsThread(false);
                return;
            }
            if (menuItem.Name == ActionType.OpenForm.ToString())
            {
                alertReadsHelper(alertItem);
                switch (alertItem.FormToOpen)
                {
                    case FormType.FormPendingPayments:
                        FormPendingPayments FormPP = new FormPendingPayments();
                        FormPP.Show();//Non-modal so the user can view the patient's account
                        FormPP.FormClosed += this.alertFormClosingHelper;
                        break;
                    case FormType.FormEServicesWebSchedRecall:
                        //ShowEServicesSetup(FormEServicesSetup.EService.WebSched);
                        break;
                    case FormType.FormRadOrderList:
                        List<FormRadOrderList> listFormROLs = Application.OpenForms.OfType<FormRadOrderList>().ToList();
                        if (listFormROLs.Count > 0)
                        {
                            listFormROLs[0].RefreshRadOrdersForUser(Security.CurUser);
                            listFormROLs[0].BringToFront();
                        }
                        else
                        {
                            FormRadOrderList FormROL = new FormRadOrderList(Security.CurUser);
                            FormROL.Show();
                            FormROL.FormClosed += this.alertFormClosingHelper;
                        }
                        break;
                    case FormType.FormEServicesSignupPortal:
                        //ShowEServicesSetup(FormEServicesSetup.EService.SignupPortal);
                        break;
                    case FormType.FormEServicesWebSchedNewPat:
                        //ShowEServicesSetup(FormEServicesSetup.EService.WebSchedNewPat);
                        break;
                    case FormType.FormEServicesEConnector:
                        //ShowEServicesSetup(FormEServicesSetup.EService.ListenerService);
                        break;
                    case FormType.FormApptEdit:
                        Appointment appt = Appointments.GetOneApt(alertItem.FKey);
                        Patient pat = Patients.GetPat(appt.PatNum);
                        S_Contr_PatientSelected(pat, false);
                        FormApptEdit FormAE = new FormApptEdit(appt.AptNum);
                        FormAE.ShowDialog();
                        break;
                    case FormType.FormWebSchedAppts:
                        FormWebSchedAppts FormWebSchedAppts = new FormWebSchedAppts(alertItem.Type == AlertType.WebSchedNewPatApptCreated,
                            alertItem.Type == AlertType.WebSchedRecallApptCreated, alertItem.Type == AlertType.WebSchedASAPApptCreated);
                        FormWebSchedAppts.Show();
                        break;
                    case FormType.FormPatientEdit:
                        pat = Patients.GetPat(alertItem.FKey);
                        Family fam = Patients.GetFamily(pat.PatNum);
                        S_Contr_PatientSelected(pat, false);
                        FormPatientEdit FormPE = new FormPatientEdit(pat, fam);
                        FormPE.ShowDialog();
                        break;
                    case FormType.FormDoseSpotAssignUserId:
                        if (!Security.IsAuthorized(Permissions.SecurityAdmin))
                        {
                            break;
                        }
                        FormDoseSpotAssignUserId FormAU = new FormDoseSpotAssignUserId(alertItem.FKey);
                        FormAU.ShowDialog();
                        break;
                    case FormType.FormDoseSpotAssignClinicId:
                        if (!Security.IsAuthorized(Permissions.SecurityAdmin))
                        {
                            break;
                        }
                        FormDoseSpotAssignClinicId FormACI = new FormDoseSpotAssignClinicId(alertItem.FKey);
                        FormACI.ShowDialog();
                        break;
                    case FormType.FormEmailInbox:
                        //Will open the email inbox form and set the current inbox to "WebMail".
                        FormEmailInbox FormEI = new FormEmailInbox("WebMail");
                        FormEI.FormClosed += this.alertFormClosingHelper;
                        FormEI.Show();
                        break;
                }
            }
            if (menuItem.Name == ActionType.ShowItemValue.ToString())
            {
                alertReadsHelper(alertItem);
                MsgBoxCopyPaste msgBCP = new MsgBoxCopyPaste(alertItem.ItemValue);
                msgBCP.Show();
            }
        }

        ///<summary>This is used to force the alert logic to run on the server in OpenDentalService.
        ///OpenDentalService Alerts logic will re run on signal update interval time.
        ///This could be enhanced eventually only invalidate when something from the form changed.</summary>
        private void alertFormClosingHelper(object sender, FormClosedEventArgs e)
        {
            DataValid.SetInvalid(InvalidType.AlertItems);//THIS IS NOT CACHED. But is used to make server run the alert logic in OpenDentalService.
        }

        ///<summary>Refreshes AlertReads for current user and creates a new one if one does not exist for given alertItem.</summary>
        private void alertReadsHelper(AlertItem alertItem)
        {
            if (_listAlertReads.Exists(x => x.AlertItemNum == alertItem.AlertItemNum))
            {//User has already read this alertitem.
                return;
            }
            AlertReads.Insert(new AlertRead(alertItem.AlertItemNum, Security.CurUser.UserNum));
        }
        #endregion Alerts

        #region Standard and Query reports
        private void menuItemReportsHeader_Popup(object sender, EventArgs e)
        {
            menuItemReportsStandard.MenuItems.Clear();
            menuItemReportsUserQuery.MenuItems.Clear();
            if (Security.CurUser == null)
            {
                return;
            }
            #region Standard
            List<DisplayReport> listDisplayReports = DisplayReports.GetSubMenuReports();
            if (listDisplayReports.Count > 0)
            {
                List<long> listReportPermissionFkeys = GroupPermissions.GetPermsForReports()
                    .Where(x => Security.CurUser.IsInUserGroup(x.UserGroupNum))
                    .Select(x => x.FKey)
                    .ToList();
                listDisplayReports.RemoveAll(x => !listReportPermissionFkeys.Contains(x.DisplayReportNum));//Remove reports user does not have permission for
                menuItemReportsStandard.MenuItems.Add(Lans.g(this, "Standard Reports"), menuItemReportsStandard_Click);
                menuItemReportsStandard.MenuItems.Add("-");//Horizontal line.
                listDisplayReports.ForEach(x =>
                {
                    MenuItem menuItem = new MenuItem(x.Description, StandardReport_ClickEvent);
                    menuItem.Tag = x;
                    menuItemReportsStandard.MenuItems.Add(menuItem);
                });
            }
            #endregion
            #region UserQueries
            List<UserQuery> listReleasedQuries = UserQueries.GetDeepCopy(true);
            if (listReleasedQuries.Count > 0)
            {
                menuItemReportsUserQuery.MenuItems.Add(Lans.g(this, "User Query"), menuItemReportsUserQuery_Click);
                menuItemReportsUserQuery.MenuItems.Add("-");//Horizontal line.
                listReleasedQuries.ForEach(x =>
                {
                    MenuItem menuItem = new MenuItem(x.Description, UserQuery_ClickEvent);
                    menuItem.Tag = x;
                    menuItemReportsUserQuery.MenuItems.Add(menuItem);
                });
            }
            #endregion
        }

        private void StandardReport_ClickEvent(object sender, EventArgs e)
        {
            DisplayReport displayReport = (DisplayReport)((MenuItem)sender).Tag;
            ReportNonModalSelection selection = FormReportsMore.OpenReportHelper(displayReport, doValidatePerm: false);//Permission already validated.
            NonModalReportSelectionHelper(selection);
        }

        private void NonModalReportSelectionHelper(ReportNonModalSelection selection)
        {
            switch (selection)
            {
                case ReportNonModalSelection.TreatmentFinder:
                    FormRpTreatmentFinder FormT = new FormRpTreatmentFinder();
                    FormT.Show();
                    break;
                case ReportNonModalSelection.OutstandingIns:
                    FormRpOutstandingIns FormOI = new FormRpOutstandingIns();
                    FormOI.Show();
                    break;
                case ReportNonModalSelection.UnfinalizedInsPay:
                    FormRpUnfinalizedInsPay FormU = new FormRpUnfinalizedInsPay();
                    FormU.Show();
                    break;
                case ReportNonModalSelection.UnsentClaim:
                    FormRpClaimNotSent FormCNS = new FormRpClaimNotSent();
                    FormCNS.Show();
                    break;
                case ReportNonModalSelection.WebSchedAppointments:
                    FormWebSchedAppts formWSA = new FormWebSchedAppts(true, true, true);
                    formWSA.Show();
                    break;
                case ReportNonModalSelection.CustomAging:
                    FormRpCustomAging FormCAO = new FormRpCustomAging();
                    FormCAO.Show();
                    break;
                case ReportNonModalSelection.IncompleteProcNotes:
                    FormRpProcNote FormPN = new FormRpProcNote();
                    FormPN.Show();
                    break;
                case ReportNonModalSelection.ProcNotBilledIns:
                    FormRpProcNotBilledIns FormProc = new FormRpProcNotBilledIns();
                    FormProc.FormClosed += (s, ea) => { ODEvent.Fired -= formProcNotBilled_GoToChanged; };
                    ODEvent.Fired += formProcNotBilled_GoToChanged;
                    FormProc.Show();//FormProcSend has a GoTo option and is shown as a non-modal window.
                    FormProc.BringToFront();
                    break;
                case ReportNonModalSelection.ODProcsOverpaid:
                    FormRpProcOverpaid FormPO = new FormRpProcOverpaid();
                    FormPO.Show();
                    break;
                case ReportNonModalSelection.None:
                default:
                    //Do nothing.
                    break;
            }
        }

        private void formProcNotBilled_GoToChanged(ODEventArgs e)
        {
            if (e.EventType != ODEventType.FormProcNotBilled_GoTo)
            {
                return;
            }
            Patient pat = Patients.GetPat((long)e.Tag);
            FormOpenDental.S_Contr_PatientSelected(pat, false);
            GotoModule.GotoClaim((long)e.Tag);
        }

        private void UserQuery_ClickEvent(object sender, EventArgs e)
        {
            UserQuery userQuery = (UserQuery)((MenuItem)sender).Tag;
            ExecuteQueryFavorite(userQuery);
        }

        private void ExecuteQueryFavorite(UserQuery userQuery)
        {
            SecurityLogs.MakeLogEntry(Permissions.UserQuery, 0, Lan.g(this, "User query form accessed."));
            //ReportSimpleGrid report=new ReportSimpleGrid();
            if (userQuery.IsPromptSetup && UserQueries.ParseSetStatements(userQuery.QueryText).Count > 0)
            {
                //if the user is not a query admin, they will not have the ability to edit 
                //the query before it is run, so show them the SET statement edit window.
                FormQueryParser FormQP = new FormQueryParser(userQuery);
                FormQP.ShowDialog();
                if (FormQP.DialogResult != DialogResult.OK)
                {
                    //report.Query=userQuery.QueryText;
                    return;
                }
            }
            if (_formUserQuery != null)
            {
                _formUserQuery.textQuery.Text = userQuery.QueryText;
                _formUserQuery.textTitle.Text = userQuery.FileName;
                _formUserQuery.SubmitQueryThreaded();
                _formUserQuery.BringToFront();
                return;
            }
            _formUserQuery = new FormQuery(null, true);
            _formUserQuery.FormClosed += new FormClosedEventHandler((object senderF, FormClosedEventArgs eF) => { _formUserQuery = null; });
            _formUserQuery.textQuery.Text = userQuery.QueryText;
            _formUserQuery.textTitle.Text = userQuery.FileName;
            _formUserQuery.Show();
        }

        #endregion

        #region Help

        //Help
        private void menuItemRemote_Click(object sender, System.EventArgs e)
        {
            try
            {
                Process.Start("http://www.opendental.com/contact.html");
            }
            catch (Exception)
            {
                MessageBox.Show(Lan.g(this, "Could not find") + " http://www.opendental.com/contact.html" + "\r\n"
                    + Lan.g(this, "Please set up a default web browser."));
            }
            /*
			if(!MsgBox.Show(this,true,"A remote connection will now be attempted. Do NOT continue unless you are already on the phone with us.  Do you want to continue?"))
			{
				return;
			}
			try{
				Process.Start("remoteclient.exe");//Network streaming remote client or any other similar client
			}
			catch{
				MsgBox.Show(this,"Could not find file.");
			}*/
        }

        private void menuItemHelpWindows_Click(object sender, System.EventArgs e)
        {
            try
            {
                Process.Start("Help.chm");
            }
            catch
            {
                MsgBox.Show(this, "Could not find file.");
            }
        }

        private void menuItemHelpContents_Click(object sender, System.EventArgs e)
        {
            try
            {
                Process.Start("https://www.opendental.com/manual/manual.html");
            }
            catch
            {
                MsgBox.Show(this, "Could not find file.");
            }
        }

        private void menuItemHelpIndex_Click(object sender, System.EventArgs e)
        {
            try
            {
                Process.Start("https://www.opendental.com/site/searchsite.html");
            }
            catch
            {
                MsgBox.Show(this, "Could not find file.");
            }
        }

        private void menuItemWebinar_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://opendental.com/webinars/webinars.html");
            }
            catch
            {
                MsgBox.Show(this, "Could not open page.");
            }
        }

        private void menuItemRemoteSupport_Click(object sender, EventArgs e)
        {
            //Check the installation directory for the GoToAssist corporate exe.
            string fileGTA = Storage.Default.CombinePath(Application.StartupPath, "GoToAssist_Corporate_Customer_ver11_9.exe");
            try
            {
                if (!File.Exists(fileGTA))
                {
                    throw new ApplicationException();//No message because a different message shows below.
                }
                //GTA exe is available, so load it up
                Process.Start(fileGTA);
            }
            catch
            {
                MsgBox.Show(this, "Could not find file.  Please use Online Support instead.");
            }
        }

        private void menuItemUpdate_Click(object sender, System.EventArgs e)
        {
            //If A to Z folders are disabled, this menu option is unavailable, since
            //updates are handled more automatically.
            FormUpdate FormU = new FormUpdate();
            FormU.ShowDialog();
            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Update Version");
        }

        /// <summary>
        /// Shows the about dialog.
        /// </summary>
        void menuItemAbout_Click(object sender, EventArgs e)
        {
            using (var formAbout = new FormAbout())
            {
                formAbout.ShowDialog();
            }
        }


        #endregion

        #endregion

        ///<summary></summary>
        private void ProcessCommandLine(string[] args)
        {
            //if(!Programs.UsingEcwTight() && args.Length==0){
            if (!Programs.UsingEcwTightOrFullMode() && args.Length == 0)
            {//May have to modify to accept from other sw.
                SetModuleSelected();
                return;
            }
            /*string descript="";
			for(int i=0;i<args.Length;i++) {
				if(i>0) {
					descript+="\r\n";
				}
				descript+=args[i];
			}
			MessageBox.Show(descript);*/
            /*
			PatNum (the integer primary key)
			ChartNumber (alphanumeric)
			SSN (exactly nine digits. If required, we can gracefully handle dashes, but that is not yet implemented)
			UserName
			Password*/
            long patNum = 0;
            string chartNumber = "";
            string ssn = "";
            string userName = "";
            string passHash = "";
            string aptNum = "";
            string ecwConfigPath = "";
            long userId = 0;
            string jSessionId = "";
            string jSessionIdSSO = "";
            string lbSessionId = "";
            Dictionary<string, int> dictModules = new Dictionary<string, int>();
            dictModules.Add("appt", 0);
            dictModules.Add("family", 1);
            dictModules.Add("account", 2);
            dictModules.Add("txplan", 3);
            dictModules.Add("treatplan", 3);
            dictModules.Add("chart", 4);
            dictModules.Add("images", 5);
            dictModules.Add("manage", 6);
            int startingModuleIdx = -1;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("PatNum=") && args[i].Length > 7)
                {
                    string patNumStr = args[i].Substring(7).Trim('"');
                    try
                    {
                        patNum = Convert.ToInt64(patNumStr);
                    }
                    catch { }
                }
                if (args[i].StartsWith("ChartNumber=") && args[i].Length > 12)
                {
                    chartNumber = args[i].Substring(12).Trim('"');
                }
                if (args[i].StartsWith("SSN=") && args[i].Length > 4)
                {
                    ssn = args[i].Substring(4).Trim('"');
                }
                if (args[i].StartsWith("UserName=") && args[i].Length > 9)
                {
                    userName = args[i].Substring(9).Trim('"');
                }
                if (args[i].StartsWith("PassHash=") && args[i].Length > 9)
                {
                    passHash = args[i].Substring(9).Trim('"');
                }
                if (args[i].StartsWith("AptNum=") && args[i].Length > 7)
                {
                    aptNum = args[i].Substring(7).Trim('"');
                }
                if (args[i].StartsWith("EcwConfigPath=") && args[i].Length > 14)
                {
                    ecwConfigPath = args[i].Substring(14).Trim('"');
                }
                if (args[i].StartsWith("UserId=") && args[i].Length > 7)
                {
                    string userIdStr = args[i].Substring(7).Trim('"');
                    try
                    {
                        userId = Convert.ToInt64(userIdStr);
                    }
                    catch { }
                }
                if (args[i].StartsWith("JSESSIONID=") && args[i].Length > 11)
                {
                    jSessionId = args[i].Substring(11).Trim('"');
                }
                if (args[i].StartsWith("JSESSIONIDSSO=") && args[i].Length > 14)
                {
                    jSessionIdSSO = args[i].Substring(14).Trim('"');
                }
                if (args[i].StartsWith("LBSESSIOINID=") && args[i].Length > 12)
                {
                    lbSessionId = args[i].Substring(12).Trim('"');
                }
                if (args[i].ToLower().StartsWith("module=") && args[i].Length > 7)
                {
                    string moduleName = args[i].Substring(7).Trim('"').ToLower();
                    if (dictModules.ContainsKey(moduleName))
                    {
                        startingModuleIdx = dictModules[moduleName];
                    }
                }
                if (args[i].ToLower().StartsWith("show=") && args[i].Length > 5)
                {
                    _StrCmdLineShow = args[i].Substring(5).Trim('"').ToLower();
                }
            }
            if (ProgramProperties.GetPropVal(Programs.GetProgramNum(ProgramName.eClinicalWorks), "IsLBSessionIdExcluded") == "1" //if check box in Program Links is checked
                && lbSessionId == "" //if lbSessionId not previously set
                && args.Length > 0 //there is at least one argument passed in
                && !args[args.Length - 1].StartsWith("LBSESSIONID="))//if there is an argument that is the last argument that is not called "LBSESSIONID", then use that argument, including the "name=" part
            {
                //An example of this is command line includes LBSESSIONID= icookie=ECWAPP3ECFH. The space makes icookie a separate parameter. We want to set lbSessionId="icookie=ECWAPP3ECFH". 
                //We are not guaranteed that the parameter is always going to be named icookie, in fact it will be different on each load balancer depending on the setup of the LB.  
                //Therefore, we cannot look for parameter name, but Aislinn from eCW guaranteed that it would be the last parameter every time during our (Cameron and Aislinn's) conversation on 3/5/2014.
                //jsalmon - This is very much a hack but the customer is very large and needs this change ASAP.  Nathan has suggested that we create a ticket with eCW to complain about this and make them fix it.
                lbSessionId = args[args.Length - 1].Trim('"');
            }
            //eCW bridge values-------------------------------------------------------------
            Bridges.ECW.AptNum = PIn.Long(aptNum);
            Bridges.ECW.EcwConfigPath = ecwConfigPath;
            Bridges.ECW.UserId = userId;
            Bridges.ECW.JSessionId = jSessionId;
            Bridges.ECW.JSessionIdSSO = jSessionIdSSO;
            Bridges.ECW.LBSessionId = lbSessionId;
            //Username and password-----------------------------------------------------
            //users are allowed to use ecw tight integration without command line.  They can manually launch Open Dental.
            //if((Programs.UsingEcwTight() && Security.CurUser==null)//We always want to trigger login window for eCW tight, even if no username was passed in.
            if ((Programs.UsingEcwTightOrFullMode() && Security.CurUser == null)//We always want to trigger login window for eCW tight, even if no username was passed in.
                || (userName != ""//if a username was passed in, but not in tight eCW mode
                && (Security.CurUser == null || Security.CurUser.UserName != userName))//and it's different from the current user
            )
            {
                //The purpose of this loop is to use the username and password that were passed in to determine which user to log in
                //log out------------------------------------
                LastModule = myOutlookBar.SelectedIndex;
                myOutlookBar.SelectedIndex = -1;
                myOutlookBar.Invalidate();
                UnselectActive();
                allNeutral();
                User user = Userods.GetUserByName(userName, true);
                if (user == null)
                {
                    //if(Programs.UsingEcwTight() && userName!="") {
                    if (Programs.UsingEcwTightOrFullMode() && userName != "")
                    {
                        user = new User();
                        user.UserName = userName;
                        user.LoginDetails = Authentication.GenerateLoginDetailsMD5(passHash, true);
                        //This can fail if duplicate username because of capitalization differences.
                        Userods.Insert(user, new List<long> { PIn.Long(ProgramProperties.GetPropVal(ProgramName.eClinicalWorks, "DefaultUserGroup")) });
                        DataValid.SetInvalid(InvalidType.Security);
                    }
                    else
                    {//not using eCW in tight integration mode
                     //So present logon screen
                        ShowLogOn();
                        user = Security.CurUser.Copy();
                    }
                }
                //Can't use Userods.CheckPassword, because we only have the hashed password.
                //if(passHash!=user.Password || !Programs.UsingEcwTight())//password not accepted or not using eCW
                if (passHash != user.PasswordHash || !Programs.UsingEcwTightOrFullMode())//password not accepted or not using eCW
                {
                    //So present logon screen
                    ShowLogOn();
                }
                else
                {//password accepted and using eCW tight.
                 //this part usually happens in the logon window
                    Security.CurUser = user.Copy();
                }
                myOutlookBar.SelectedIndex = Security.GetModule(LastModule);
                myOutlookBar.Invalidate();
                SetModuleSelected();
                Patient pat = Patients.GetPat(CurPatNum);//pat could be null
                Text = PatientL.GetMainTitle(pat, Clinics.ClinicNum);//handles pat==null by not displaying pat name in title bar
                if (userControlTasks1.Visible)
                {
                    userControlTasks1.InitializeOnStartup();
                }
                if (myOutlookBar.SelectedIndex == -1)
                {
                    MsgBox.Show(this, "You do not have permission to use any modules.");
                }
            }
            if (startingModuleIdx != -1 && startingModuleIdx == Security.GetModule(startingModuleIdx))
            {
                UnselectActive();
                allNeutral();//Sets all controls to false.  Needed to set the new module as selected.
                myOutlookBar.SelectedIndex = startingModuleIdx;
                myOutlookBar.Invalidate();
            }
            SetModuleSelected();
            //patient id----------------------------------------------------------------
            if (patNum != 0)
            {
                Patient pat = Patients.GetPat(patNum);
                if (pat == null)
                {
                    CurPatNum = 0;
                    RefreshCurrentModule();
                    FillPatientButton(null);
                }
                else
                {
                    CurPatNum = patNum;
                    RefreshCurrentModule();
                    FillPatientButton(pat);
                }
            }
            else if (chartNumber != "")
            {
                Patient pat = Patients.GetPatByChartNumber(chartNumber);
                if (pat == null)
                {
                    //todo: decide action
                    CurPatNum = 0;
                    RefreshCurrentModule();
                    FillPatientButton(null);
                }
                else
                {
                    CurPatNum = pat.PatNum;
                    RefreshCurrentModule();
                    FillPatientButton(pat);
                }
            }
            else if (ssn != "")
            {
                Patient pat = Patients.GetPatBySSN(ssn);
                if (pat == null)
                {
                    //todo: decide action
                    CurPatNum = 0;
                    RefreshCurrentModule();
                    FillPatientButton(null);
                }
                else
                {
                    CurPatNum = pat.PatNum;
                    RefreshCurrentModule();
                    FillPatientButton(pat);
                }
            }
            else
            {
                FillPatientButton(null);
            }
        }


        ///< summary>
        ///Fires an OD event for the Splash Screen Progress Bar
        ///</summary>
        void UpdateSplashProgress(string status, int percentage)
        {
            SplashProgressEvent.Fire(
                ODEventType.SplashScreenProgress, 
                new ProgressBarHelper(
                    status, percentage.ToString() + "%", 
                    percentage, 100, 
                    ProgBarStyle.Continuous, 
                    "Update"));
        }
        private void TryNonPatientPopup()
        {
            if (CurPatNum != 0 && _previousPatNum != CurPatNum)
            {
                _datePopupDelay = DateTime.Now;
                _previousPatNum = CurPatNum;
            }
            if (!Preference.GetBool(PreferenceName.ChartNonPatientWarn))
            {
                return;
            }
            Patient patCur = Patients.GetPat(CurPatNum);
            if (patCur != null
                        && patCur.PatStatus.ToString() == "NonPatient"
                        && _datePopupDelay <= DateTime.Now)
            {
                MsgBox.Show(this, "A patient with the status NonPatient is currently selected.");
                _datePopupDelay = DateTime.Now.AddMinutes(5);
            }
        }

        #region LogOn
        ///<summary>Logs on a user using the passed in credentials or Active Directory or the good old-fashioned log on window.</summary>
        private void LogOnOpenDentalUser(string odUser, string odPassword)
        {
            //CurUser could already be set if using web service because login from ChooseDatabase window.
            if (Security.CurUser != null)
            {
                Security.IsUserLoggedIn = true;//This might be wrong.  We set to true for backward compatibility.
                return;
            }

            #region eCW Tight or Full
            //Leave Security.CurUser null if a user was passed in on the commandline.  If starting OD manually, it continues below.
            if (Programs.UsingEcwTightOrFullMode() && odUser != "")
            {
                //Purposefully leave Security.CurUser as null.
                Security.IsUserLoggedIn = true;//This might be wrong.  We set to true for backward compatibility.
                return;
            }
            #endregion
            #region Command Line Args
            //Both a username and password was passed in via command line arguments.
            if (odUser != "" && odPassword != "")
            {
                try
                {
                    bool isEcwTightOrFullMode = Programs.UsingEcwTightOrFullMode();
                    Security.CurUser = Userods.CheckUserAndPassword(odUser, odPassword, isEcwTightOrFullMode);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Application.Exit();
                    return;
                }
            }
            #endregion
            #region Good Old-fashioned Log On
            if (Security.CurUser == null)
            {//Security.CurUser could be set if valid command line arguments were passed in.
                #region Admin User No Password
                if (!Userods.HasSecurityAdminUserNoCache())
                {
                    MsgBox.Show(this, "There are no users with the SecurityAdmin permission.  Call support.");
                    Application.Exit();
                    return;
                }
                long userNumFirstAdminNoPass = Userods.GetFirstSecurityAdminUserNumNoPasswordNoCache();
                if (userNumFirstAdminNoPass > 0)
                {
                    Security.CurUser = Userods.GetUserNoCache(userNumFirstAdminNoPass);
                    CheckForPasswordReset();

                    SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, Lan.g(this, "User:") + " " + Security.CurUser.UserName + " " + Lan.g(this, "has logged on."));
                }
                #endregion
                #region Domain Longin
                else if (Preference.GetBool(PreferenceName.DomainLoginEnabled) && !string.IsNullOrWhiteSpace(Preference.GetString(PreferenceName.DomainLoginPath)))
                {
                    string loginPath = Preference.GetString(PreferenceName.DomainLoginPath);
                    try
                    {
                        DirectoryEntry loginEntry = new DirectoryEntry(loginPath);
                        string distinguishedName = loginEntry.Properties["distinguishedName"].Value.ToString();
                        //All LDAP servers must expose a special entry, called the root DSE. This gets the current user's domain path.
                        DirectoryEntry rootDSE = new DirectoryEntry("LDAP://RootDSE");
                        string defaultNamingContext = rootDSE.Properties["defaultNamingContext"].Value.ToString();
                        if (!distinguishedName.ToLower().Contains(defaultNamingContext.ToLower()))
                        {
                            //If the domain of the current user doesn't match the provided LDAP Path, log on normally
                            ShowLogOn();
                            Security.IsUserLoggedIn = true;
                            return;
                        }
                        Dictionary<long, string> dictDomainUserNumsAndNames = Userods.GetUsersByDomainUserNameNoCache(Environment.UserName);
                        if (dictDomainUserNumsAndNames.Count == 0)
                        { //Log on normally if no user linked the current domain user
                            ShowLogOn();
                        }
                        else if (dictDomainUserNumsAndNames.Count > 1)
                        { //Select a user if multiple users linked to the current domain user
                            InputBox box = new InputBox(Lan.g(this, "Select an Open Dental user to log in with:"), dictDomainUserNumsAndNames.Select(x => x.Value).ToList());
                            box.ShowDialog();
                            if (box.DialogResult == DialogResult.OK)
                            {
                                Security.CurUser = Userods.GetUserNoCache(dictDomainUserNumsAndNames.Keys.ElementAt(box.SelectedIndex));
                                CheckForPasswordReset();

                                SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, Lan.g(this, "User:") + " " + Security.CurUser.UserName + " "
                                    + Lan.g(this, "has logged on automatically via ActiveDirectory."));
                            }
                            else
                            {
                                ShowLogOn();
                            }
                        }
                        else
                        { //log on automatically if only one user is linked to current domain user
                            Security.CurUser = Userods.GetUserNoCache(dictDomainUserNumsAndNames.Keys.First());
                            CheckForPasswordReset();

                            SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, Lan.g(this, "User:") + " " + Security.CurUser.UserName + " "
                                    + Lan.g(this, "has logged on automatically via ActiveDirectory."));
                        }
                    }
                    catch
                    {
                        ShowLogOn();
                        Security.IsUserLoggedIn = true;
                        return;
                    }
                }
                #endregion
                #region Manual LogOn Window
                else
                {
                    ShowLogOn();
                }
                #endregion
            }
            #endregion
            Security.IsUserLoggedIn = true;//User is guaranteed to be logged in at this point.
        }

        /// <summary>
        /// Show the log on window.
        /// </summary>
        void ShowLogOn()
        {
            Userods.SetIsCacheAllowed(false);

            using (var formLogOn = new FormLogOn(doRefreshSecurityCache: false))
            {
                if (formLogOn.ShowDialog(this) == DialogResult.OK)
                {
                    Cursor = Cursors.Default;

                    Application.Exit();

                    return;
                }

                CheckForPasswordReset();

                Userods.SetIsCacheAllowed(true);

                //Refresh the cache if we need to since cache allowed was just set to true
                if (formLogOn.RefreshSecurityCache)
                {
                    DataValid.SetInvalid(InvalidType.Security);
                }
            }
        }

        /// <summary>
        /// Checks to see if the currently logged-in user needs to reset their password.
        /// If they do, then this method will force the user to reset the password otherwise the program will exit.
        /// </summary>
        void CheckForPasswordReset()
        {
            if (Security.CurUser.IsPasswordResetRequired)
            {
                using (var formUserPassword = new FormUserPassword(false, Security.CurUser.UserName, true))
                {
                    if (formUserPassword.ShowDialog() == DialogResult.Cancel)
                    {
                        Cursor = Cursors.Default;

                        Application.Exit();

                        return;
                    }

                    var isStrongPassword = formUserPassword.PasswordIsStrong;
                    try
                    {
                        Security.CurUser.IsPasswordResetRequired = false;

                        Userods.Update(Security.CurUser);
                        Userods.UpdatePassword(Security.CurUser, formUserPassword.LoginDetails, isStrongPassword);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    Security.CurUser.PasswordIsStrong = formUserPassword.PasswordIsStrong;
                    Security.CurUser.LoginDetails = formUserPassword.LoginDetails;
                    Security.PasswordTyped = formUserPassword.Password;

                    DataValid.SetInvalid(InvalidType.Security);
                }
            }
        }

        #endregion LogOn
        #region Logoff

        ///<summary>Returns a list of forms that are currently open excluding FormOpenDental and FormLogOn.
        ///This method is typically called in order to close any open forms sans the aforementioned forms.
        ///Therefore, the list returned is ordered with the intent that the calling method will close children first and then parents last.</summary>
        private List<Form> GetOpenForms()
        {
            if (this.InvokeRequired)
            {
                return (List<Form>)this.Invoke(new Func<List<Form>>(() => GetOpenForms()));
            }
            List<Form> listOpenForms = new List<Form>();
            for (int f = Application.OpenForms.Count - 1; f >= 0; f--)
            {//Loop backwards assuming children are added later in the collection.
                Form openForm = Application.OpenForms[f];
                if (openForm == this)
                {// main form
                    continue;
                }
                if (openForm.Name == "FormLogOn")
                {
                    continue;
                }
                listOpenForms.Add(Application.OpenForms[f]);
            }
            return listOpenForms;
        }

        ///<summary>Enumerates open forms and saves work for those forms which have a save handler.  Some forms are closed as part of saving work.</summary>
        private bool SaveWork(bool isForceClose)
        {
            if (InvokeRequired) return (bool)Invoke(new Func<bool>(() => SaveWork(isForceClose)));

            var formList = GetOpenForms();
            foreach (Form openForm in formList)
            {
                // If force closing, we HAVE to forcefully close everything related to Open Dental, regardless of plugins.
                // Otherwise, give plugins a chance to stop the log off event.
                if (!isForceClose)
                {
                    if (Plugin.Filter(this, "FormOpenDental_CloseForm", true, openForm) == false)
                    {
                        continue;
                    }
                }

                if (openForm.Name == "FormWikiEdit")
                {
                    if (!isForceClose)
                    {
                        if (!MsgBox.Show(this, MsgBoxButtons.OKCancel, "You are currently editing a wiki page and it will be saved as a draft. Continue?"))
                        {
                            return false;//This form needs to stay open and the close operation should be aborted.
                        }
                    }
                }
            }
            GeneralProgramEvent.Fire(ODEventType.Shutdown, isForceClose);
            foreach (Form formToClose in formList)
            {
                if (formToClose.Name == "FormWikiEdit")
                {
                    WikiSaveEvent.Fire(ODEventType.WikiSave);
                }
                if (formToClose.Name == "FormCommItem")
                {
                    CommItemSaveEvent.Fire(ODEventType.CommItemSave);
                }
                if (formToClose.Name == "FormEmailMessageEdit")
                {
                    EmailSaveEvent.Fire(ODEventType.EmailSave);
                }
            }
            return true;
        }

        ///<summary>Do not call this function inside of an invoke, or else the form closing events will not return from ShowDialog() calls in time.
        ///Closes all open forms except FormOpenDental.  Set isForceClose to true if you want to close all forms asynchronously.  Set 
        ///forceCloseTimeoutMS when isForceClose is set to true to specify a timeout value for forms that take too long to close, e.g. a form hanging in 
        ///a FormClosing event on a MessageBox.  If the timeout value is reached, the program will exit.  E.g. FormWikiEdit will ask users on closing if 
        ///they are sure they want to discard unsaved work.  Returns false if there is an open form that requests attention, thus needs to stop the 
        ///closing of the forms.</summary>
        private bool CloseOpenForms(bool isForceClose, int forceCloseTimeoutMS = 15000)
        {
            if (!SaveWork(isForceClose))
            {
                return false;
            }
            List<Form> listCloseForms = GetOpenForms();
            #region Close forms and quit threads.  Some form closing events rely on the closing events of parent forms.
            while (listCloseForms.Count > 0)
            {
                Form formToClose = listCloseForms[0];
                if (isForceClose)
                {
                    ODThread threadCloseForm = new ODThread((o) =>
                    {
                        formToClose.Invoke(formToClose.Close);
                    });
                    threadCloseForm.Name = "ForceCloseForm";
                    bool hasError = false;
                    threadCloseForm.AddExceptionHandler((ex) =>
                    {
                        hasError = true;
                        ODException.SwallowAnyException(() =>
                        {
                            //A FormClosing() or FormClosed() event caused an exception.  Try to submit the exception so that we are made aware.
                            //BugSubmissions.SubmitException(ex, threadCloseForm.Name);
                        });
                    });
                    threadCloseForm.Start();
                    threadCloseForm.Join(1000);//Give the form a maximum amount of time to close, and continue if not responsive.
                    this.Invoke(Application.DoEvents);//Run on main thread so that ShowDialog() for the form will continue in the parent context immediately.
                    if (hasError || !IsDisplosedOrClosed(formToClose))
                    {
                        formToClose.Invoke(formToClose.Dispose);//If failed to close, kill window so that the ShowDialog() call can continue in parent context.
                    }
                    //In case the form we just closed created new popup forms inside the FormClosing or FormClosed event,
                    //we need to check for newly created forms and add them to the queue of forms to close.
                    //Any new forms will be closed next, so that child forms are closed as soon as possible before closing any parent forms.
                    List<Form> listNewForms = GetOpenForms();
                    listCloseForms.ForEach(x => listNewForms.Remove(x));
                    foreach (Form brandNewForm in listNewForms)
                    {
                        listCloseForms.Insert(0, brandNewForm);
                    }
                }
                else
                {//User manually chose to logoff/shutdown.  Gracefully close each window.
                 //If the window which showed the messagebox popup causes the form to stay open, then stop the log off event, because the user chose to.
                    formToClose.InvokeIfRequired(() => formToClose.Close());//Attempt to close the form, even if created in another thread (ex FormHelpBrowser).
                                                                            //Run Applicaiton.DoEvents() to allow the FormClosing/FormClosed events to fire in the form before checking if they have closed below.
                    Application.DoEvents();//Required due to invoking.  Otherwise FormClosing/FormClosed will not fire until after we exit CloseOpenForms.
                    if (!IsDisplosedOrClosed(formToClose))
                    {
                        //E.g. The wiki edit window will ask users if they want to lose their work or continue working.  This will get hit if they chose to continue working.
                        return false;//This form needs to stay open and stop all other forms from being closed.
                    }
                }
                listCloseForms.Remove(formToClose);
            }
            #endregion
            return true;//All open forms have been closed at this point.
        }

        private void LogOffNow()
        {
            bool isForceClose = Preference.GetLong(PreferenceName.SecurityLogOffAfterMinutes) > 0;
            LogOffNow(isForceClose);
        }

        public void LogOffNow(bool isForced)
        {
            if (!CloseOpenForms(isForced))
            {
                return; //A form is still open.  Do not continue to log the user off.
            }
            FinishLogOff(isForced);
        }

        private void FinishLogOff(bool isForced)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => { FinishLogOff(isForced); });
                return;
            }
            Plugin.Trigger(this, "LogOff", isForced);

            LastModule = myOutlookBar.SelectedIndex;
            myOutlookBar.SelectedIndex = -1;
            myOutlookBar.Invalidate();
            UnselectActive(true);
            allNeutral();
            if (userControlTasks1.Visible)
            {
                userControlTasks1.ClearLogOff();
            }

            if (isForced)
            {
                SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, "User: " + Security.CurUser.UserName + " has auto logged off.");
            }
            else
            {
                SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, "User: " + Security.CurUser.UserName + " has logged off.");
            }


            Clinics.LogOff();
            User oldUser = Security.CurUser;
            Security.CurUser = null;
            _listReminderTasks = null;
            _listNormalTaskNums = null;
            ContrAppt2.RefreshReminders(new List<Task>());
            RefreshTasksNotification();
            Security.IsUserLoggedIn = false;
            Text = PatientL.GetMainTitle(null, 0);
            SetTimersAndThreads(false);
            userControlPatientDashboard.CloseDashboard(true);
            ShowLogOn();
            //If a different user logs on and they have clinics enabled, then clear the patient drop down history
            //since the current user may not have permission to access patients from the same clinic(s) as the old user
            if (oldUser.UserNum != Security.CurUser.UserNum && Preferences.HasClinicsEnabled)
            {
                CurPatNum = 0;
                PatientL.RemoveAllFromMenu(menuPatient);
            }
            myOutlookBar.SelectedIndex = Security.GetModule(LastModule);
            myOutlookBar.Invalidate();
            if (Preferences.HasClinicsEnabled)
            {
                Clinics.LoadClinicNumForUser();
                RefreshMenuClinics();
            }
            SetModuleSelected();
            Patient pat = Patients.GetPat(CurPatNum);//pat could be null
            Text = PatientL.GetMainTitle(pat, Clinics.ClinicNum);//handles pat==null by not displaying pat name in title bar
            FillPatientButton(pat);
            if (userControlTasks1.Visible)
            {
                userControlTasks1.InitializeOnStartup();
            }
            BeginODDashboardStarterThread();
            SetTimersAndThreads(true);
            //User logged back in so log on form is no longer the active window.
            IsFormLogOnLastActive = false;
            BeginCheckAlertsThread();
            Security.DateTimeLastActivity = DateTime.Now;
            if (myOutlookBar.SelectedIndex == -1)
            {
                MsgBox.Show(this, "You do not have permission to use any modules.");
            }
        }
        #endregion Logoff

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            if (e.Reason != SessionSwitchReason.SessionLock)
            {
                return;
            }
            //CurUser will be null if Open Dental is already in a 'logged off' state.  Check Security.IsUserLoggedIn as well because Middle Tier does not 
            //set CurUser to null when logging off.
            //Also catches the case where Open Dental has NEVER connected to a database yet and checking PrefC would throw an exception (no db conn).
            if (Security.CurUser == null || !Security.IsUserLoggedIn)
            {
                return;
            }
            if (!Preference.GetBool(PreferenceName.SecurityLogOffWithWindows))
            {
                return;
            }
            LogOffNow(true);
        }

        private void FormOpenDental_Deactivate(object sender, EventArgs e)
        {
            //There is a chance that the user has gone to a non-modal form (e.g. task) and can change the patient from that form.
            //We need to save the Treatment Note in the chart module because the "on leave" event might not get fired for the text box.
            if (ContrChart2.TreatmentNoteChanged)
            {
                ContrChart2.UpdateTreatmentNote();
            }
            if (ContrAccount2.UrgFinNoteChanged)
            {
                ContrAccount2.UpdateUrgFinNote();
            }
            if (ContrAccount2.FinNoteChanged)
            {
                ContrAccount2.UpdateFinNote();
            }
            if (ContrTreat2.HasNoteChanged)
            {
                ContrTreat2.UpdateTPNoteIfNeeded();
            }
        }

        void FormOpenDental_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormOpenDentalClosing(sender, e);
        }

        private void FormOpenDentalClosing(object sender, FormClosingEventArgs e)
        {
            // ExitCode will only be set if trying to silently update.  
            // If we start using ExitCode for anything other than silently updating, this can be moved towards the bottom of this closing.
            // If moved to the bottom, all of the clean up code that this closing event does needs to be considered in regards to updating silently from a CEMT computer.
            if (ExitCode != 0)
            {
                Environment.Exit(ExitCode);
            }

            bool hadMultipleFormsOpen = Application.OpenForms.Count > 1;

            //CloseOpenForms should have already been called with isForceClose=true if we are force closing Open Dental
            //In that scenario, calling CloseOpenForms with isForceClose=false should not leave the program open.
            //However, if Open Dental is closing from any other means, we want to give all forms the opportunity to stop closing.
            //Example, if you have FormWikiEdit open, it will attempt to save it as a draft unless the user wants to back out.
            if (!CloseOpenForms(false))
            {
                e.Cancel = true;
                return;
            }
            if (hadMultipleFormsOpen)
            {
                //If this form is closing because someone called Application.Exit, then the call above to CloseOpenForms would cause an exception later in 
                //Application.Exit because CloseOpenForms altered a collection inside a foreach loop inside of Application.Exit. We still want to exit, but
                //we need to start afresh in order to not cause an exception.
                e.Cancel = true;
                this.BeginInvoke(() => Application.Exit());
                return;
            }
            try
            {
                Programs.ScrubExportedPatientData();//Required for EHR module d.7.
            }
            catch
            {
                //Can happen if cancel is clicked in Choose Database window.
            }
            try
            {
                Computer.ClearHeartBeat(Environment.MachineName);
            }
            catch { }
            FormUAppoint.AbortThread();
            ODThread.QuitSyncAllOdThreads();
            if (Security.CurUser != null)
            {
                try
                {
                    SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff, 0, "User: " + Security.CurUser.UserName + " has logged off.");
                    Clinics.LogOff();
                }
                catch
                {
                }
            }
            //if(PrefC.GetBool(PrefName.DistributorKey)) {//for OD HQ
            //  for(int f=Application.OpenForms.Count-1;f>=0;f--) {
            //    if(Application.OpenForms[f]==this) {// main form
            //      continue;
            //    }
            //    Application.OpenForms[f].Close();
            //  }
            //}
            string tempPath = "";
            string[] arrayFileNames;
            List<string> listDirectories;
            try
            {
                tempPath = Preferences.GetTempFolderPath();
                arrayFileNames = Directory.GetFiles(tempPath, "*.*", SearchOption.AllDirectories);//All files in the current directory plus all files in all subdirectories.
                listDirectories = new List<string>(Directory.GetDirectories(tempPath, "*", SearchOption.AllDirectories));//All subdirectories.
            }
            catch
            {
                //We will only reach here if we error out of getting the temp folder path
                //If we can't get the path, then none of the stuff below matters
                return;
            }
            for (int i = 0; i < arrayFileNames.Length; i++)
            {
                try
                {
                    //All files related to updates need to stay.  They do not contain PHI information and will not harm anything if left around.
                    if (arrayFileNames[i].Contains("UpdateFileCopier.exe"))
                    {
                        continue;//Skip any files related to updates.
                    }
                    //When an update is in progress, the binaries will be stored in a subfolder called UpdateFiles within the temp directory.
                    if (arrayFileNames[i].Contains("UpdateFiles"))
                    {
                        continue;//Skip any files related to updates.
                    }
                    //The UpdateFileCopier will create temporary backups of source and destination setup files so that it can revert if copying fails.
                    if (arrayFileNames[i].Contains("updatefilecopier"))
                    {
                        continue;//Skip any files related to updates.
                    }
                    File.Delete(arrayFileNames[i]);
                }
                catch
                {
                    //Do nothing because the file could have been in use or there were not sufficient permissions.
                    //This file will most likely get deleted next time a temp file is created.
                }
            }
            listDirectories.Sort();//We need to sort so that we know for certain which directories are parent directories of other directories.
            for (int i = listDirectories.Count - 1; i >= 0; i--)
            {//Easier than recursion.  Since the list is ordered ascending, then going backwards means we delete subdirectories before their parent directories.
                try
                {
                    //When an update is in progress, the binaries will be stored in a subfolder called UpdateFiles within the temp directory.
                    if (listDirectories[i].Contains("UpdateFiles"))
                    {
                        continue;//Skip any files related to updates.
                    }
                    //The UpdateFileCopier will create temporary backups of source and destination setup files so that it can revert if copying fails.
                    if (listDirectories[i].Contains("updatefilecopier"))
                    {
                        continue;//Skip any files related to updates.
                    }
                    Directory.Delete(listDirectories[i]);
                }
                catch
                {
                    //Do nothing because the folder could have been in use or there were not sufficient permissions.
                    //This folder will most likely get deleted next time Open Dental closes.
                }
            }
            Plugin.Trigger(this, "FormOpenDental_FormClosed");
        }

        private void FormOpenDental_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Cleanup all resources related to the program which have their Dispose methods properly defined.
            //This helps ensure that the chart module and its tooth chart wrapper are properly disposed of in particular.
            //This step is necessary so that graphics memory does not fill up.
            Dispose();
            //"=====================================================
            //https://msdn.microsoft.com/en-us/library/system.environment.exit%28v=vs.110%29.aspx
            //Environment.Exit Method:
            //Terminates this process and gives the underlying operating system the specified exit code.
            //For the exitCode parameter, use a non-zero number to indicate an error. In your application, you can define your own error codes in an
            //enumeration, and return the appropriate error code based on the scenario. For example, return a value of 1 to indicate that the required file
            //is not present and a value of 2 to indicate that the file is in the wrong format. For a list of exit codes used by the Windows operating
            //system, see System Error Codes in the Windows documentation.
            //Calling the Exit method differs from using your programming language's return statement in the following ways:
            //*Exit always terminates an application. Using the return statement may terminate an application only if it is used in the application entry
            //	point, such as in the Main method.
            //*Exit terminates an application immediately, even if other threads are running. If the return statement is called in the application entry
            //	point, it causes an application to terminate only after all foreground threads have terminated.
            //*Exit requires the caller to have permission to call unmanaged code. The return statement does not.
            //*If Exit is called from a try or finally block, the code in any catch block does not execute. If the return statement is used, the code in the
            //catch block does execute.
            //====================================================="
            //Call Environment.Exit() to kill all threads which we forgot to close.  Also sends exit code 0 to the command line to indicate success.
            //If a thread needs to be gracefully quit, then it is up to the designing engineer to Join() to that thread before we get to this point.
            //We considered trying to get a list of active threads and logging debug information for those threads, but there is no way
            //to get the list of managed threads from the system.  It is our responsibility to keep track of our own managed threads.  There is a way
            //to get the list of unmanaged system threads for our application using Process.GetCurrentProcess().Threads, but that does not help us enough.
            //See http://stackoverflow.com/questions/466799/how-can-i-enumerate-all-managed-threads-in-c.  To keep track of a managed thread, use ODThread.
            //Environment.Exit requires permission for unmanaged code, which we have explicitly specified in the solution already.
            Environment.Exit(0);
        }
    }

    public class PopupEvent : IComparable
    {
        public long PopupNum;
        ///<summary>Disable this popup until this time.</summary>
        public DateTime DisableUntil;
        ///<summary>The last time that this popup popped up.</summary>
        public DateTime LastViewed;

        public int CompareTo(object obj)
        {
            PopupEvent pop = (PopupEvent)obj;
            return DisableUntil.CompareTo(pop.DisableUntil);
        }

        public override string ToString()
        {
            return PopupNum.ToString() + ", " + DisableUntil.ToString();
        }
    }

    ///<summary>This is a global class because it must run at the application level in order to catch application level system input events.
    ///WM_KEYDOWN (0x0100) message details: https://msdn.microsoft.com/en-us/library/windows/desktop/ms646280(v=vs.85).aspx.
    ///WM_MOUSEMOVE (0x0200) message details: https://msdn.microsoft.com/en-us/library/windows/desktop/ms645616(v=vs.85).aspx. ///</summary>
    public class ODGlobalUserActiveHandler : IMessageFilter
    {
        ///<summary>Compare position of mouse at the time of the message to the previously stored mouse position to correctly identify a mouse movement.
        ///In testing, a mouse will sometimes fire a series of multiple MouseMove events with the same position, possibly due to wireless mouse chatter.
        ///Comparing to previos position allows us to only update the last activity timer when the mouse actually changes position.</summary>
        private Point _prevMousePos;

        ///<summary>Returning false guarantees that the message will continue to the next filter control.  Therefore this method inspects the messages,
        ///but the messages are not consumed.</summary>
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x0100)
            {//Any keyboard input (WM_KEYDOWN=0x0100).
                Security.DateTimeLastActivity = DateTime.Now;
            }
            else if (m.Msg == 0x0200 && _prevMousePos != Cursor.Position)
            {//Mouse input (WM_MOUSEMOVE=0x0200) and position changed since last checked.
                _prevMousePos = Cursor.Position;
                Security.DateTimeLastActivity = DateTime.Now;
            }
            return false;//Always allow the message to continue to the next filter control
        }
    }
}