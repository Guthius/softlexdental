using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace OpenDental
{
    public class Shared
    {
        /// <summary>
        /// Converts numbers to ordinals. 
        /// For example, 120 to 120th, 73 to 73rd. 
        /// Probably doesn't work too well with foreign language translations. 
        /// Used in the Birthday postcards.
        /// </summary>
        public static string NumberToOrdinal(int number)
        {
            // TODO: This is language specific and needs to be moved to the OpenDental.Translation library.

            if (number == 11) return "11th";
            if (number == 12) return "12th";
            if (number == 13) return "13th";
            
            string str = number.ToString();
            string last = str.Substring(str.Length - 1);

            switch (last)
            {
                case "0":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9": return str + "th";
                case "1": return str + "st";
                case "2": return str + "nd";
                case "3": return str + "rd";
            }

            return "";
        }
    }

    /// <summary>
    /// Handles a global event to keep local data synchronized.
    /// </summary>
    [Obsolete] public class DataValid
    {
        static void L(ValidEventArgs e)
        {
            string suffix = "Refreshing Caches: ";

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

            if (!e.ITypes.Contains(InvalidType.Appointment) && 
                !e.ITypes.Contains(InvalidType.Task) && 
                !e.ITypes.Contains(InvalidType.TaskPopup))
            {
                RefreshLocalData(e.ITypes);
            }

            if (e.ITypes.Contains(InvalidType.Task) || 
                e.ITypes.Contains(InvalidType.TaskPopup))
            {
                Plugin.Trigger(null, "FormOpenDental_DataBecameInvalid");
                if (ContrChart2?.Visible ?? false)
                {
                    ODEvent.Fire(ODEventType.Cache, suffix + "Chart Module");
                    ContrChart2.ModuleSelected(CurPatNum);
                }
                return;//All task signals should already be sent. Sending more Task signals here would cause unnecessary refreshes.
            }

            ODEvent.Fire(ODEventType.Cache, suffix + "Inserting Signals");

            foreach (InvalidType iType in e.ITypes)
            {
                Signal sig = new Signal();
                sig.IType = iType;
                if (iType == InvalidType.Task || iType == InvalidType.TaskPopup)
                {
                    sig.ExternalId = e.TaskNum;
                    sig.FKeyType = KeyType.Task;
                }
                Signalods.Insert(sig);
            }
        }


        /// <summary>
        /// Triggers an event that causes a signal to be sent to all other computers telling them what kind of locally stored data needs to be updated.
        /// Either supply a set of flags for the types, or supply a date if the appointment screen needs to be refreshed.
        /// Yes, this does immediately refresh the local data, too. 
        /// The AllLocal override does all types except appointment date for the local computer only, such as when starting up.
        /// </summary>
        public static void SetInvalid(params InvalidType[] arrayITypes)
        {
            FormOpenDental.S_DataValid_BecomeInvalid(new ValidEventArgs(DateTime.MinValue, arrayITypes, false, 0));
        }

        /// <summary>
        /// Triggers an event that causes a signal to be sent to all other computers telling them what kind of locally stored data needs to be updated. 
        /// Either supply a set of flags for the types, or supply a date if the appointment screen needs to be refreshed. 
        /// Yes, this does immediately refresh the local data, too. The AllLocal override does all types except appointment date for the local computer 
        /// only, such as when starting up.
        /// </summary>
        public static void SetInvalid(bool onlyLocal)
        {
            FormOpenDental.S_DataValid_BecomeInvalid(new ValidEventArgs(DateTime.MinValue, new[] { InvalidType.AllLocal }, onlyLocal, 0));
        }
    }

    public delegate void ValidEventHandler(ValidEventArgs e);

    public class ValidEventArgs : EventArgs
    {
        public ValidEventArgs(DateTime dateViewing, InvalidType[] itypes, bool onlyLocal, long taskNum) : base()
        {
            DateViewing = dateViewing;
            ITypes = itypes;
            OnlyLocal = onlyLocal;
            TaskNum = taskNum;
        }

        public DateTime DateViewing { get; }

        public InvalidType[] ITypes { get; }

        public bool OnlyLocal { get; }

        public long TaskNum { get; }
    }

    /// <summary>
    /// Used to trigger a global event to jump between modules and perform actions in other modules.  
    /// PatNum is optional. If 0, then no effect.
    /// </summary>
    public class GotoModule
    {
        /// <summary>
        /// Goes directly to an existing appointment.
        /// </summary>
        public static void GotoAppointment(DateTime dateSelected, long selectedAptNum)
        {
            OnModuleSelected(new ModuleEventArgs(dateSelected, new List<long>(), selectedAptNum, 0, 0, 0, 0));
        }

        /// <summary>
        /// Goes directly to a claim in someone's Account.
        /// </summary>
        public static void GotoClaim(long claimNum)
        {
            OnModuleSelected(new ModuleEventArgs(DateTime.MinValue, new List<long>(), 0, 2, claimNum, 0, 0));
        }

        /// <summary>
        /// Goes directly to an Account.
        /// Sometimes, patient is selected some other way instead of being passed in here, so OK to pass in a patNum of zero.
        /// </summary>
        public static void GotoAccount(long patNum)
        {
            OnModuleSelected(new ModuleEventArgs(DateTime.MinValue, new List<long>(), 0, 2, 0, patNum, 0));
        }

        /// <summary>
        /// Goes directly to Family module.
        /// Sometimes, patient is selected some other way instead of being passed in here, so OK to pass in a patNum of zero.
        /// </summary>
        public static void GotoFamily(long patNum)
        {
            OnModuleSelected(new ModuleEventArgs(DateTime.MinValue, new List<long>(), 0, 1, 0, patNum, 0));
        }

        /// <summary>
        /// Goes directly to TP module.
        /// Sometimes, patient is selected some other way instead of being passed in here, so OK to pass in a patNum of zero.
        /// </summary>
        public static void GotoTreatmentPlan(long patNum)
        {
            OnModuleSelected(new ModuleEventArgs(DateTime.MinValue, new List<long>(), 0, 3, 0, patNum, 0));
        }

        public static void GotoChart(long patNum)
        {
            OnModuleSelected(new ModuleEventArgs(DateTime.MinValue, new List<long>(), 0, 4, 0, patNum, 0));
        }

        public static void GotoManage(long patNum)
        {
            OnModuleSelected(new ModuleEventArgs(DateTime.MinValue, new List<long>(), 0, 6, 0, patNum, 0));
        }

        /// <summary>
        /// Puts appointment on pinboard, then jumps to Appointments module. 
        /// Sometimes, patient is selected some other way instead of being passed in here, so OK to pass in a patNum of zero.
        /// </summary>
        public static void PinToAppt(List<long> pinAptNums, long patNum)
        {
            OnModuleSelected(new ModuleEventArgs(DateTime.Today, pinAptNums, 0, 0, 0, patNum, 0));
        }

        /// <summary>
        /// Jumps to Images module and pulls up the specified image.
        /// </summary>
        public static void GotoImage(long patNum, long docNum)
        {
            OnModuleSelected(new ModuleEventArgs(DateTime.MinValue, new List<long>(), 0, 5, 0, patNum, docNum));
        }

        protected static void OnModuleSelected(ModuleEventArgs e) => FormOpenDental.S_GotoModule_ModuleSelected(e);
    }

    public class ModuleEventArgs : EventArgs
    {
        public ModuleEventArgs(DateTime dateSelected, List<long> listPinApptNums, long selectedAptNum, int iModule, long claimNum, long patNum, long docNum)
        {
            DateSelected = dateSelected;
            ListPinApptNums = listPinApptNums;
            SelectedAptNum = selectedAptNum;
            IModule = iModule;
            ClaimNum = claimNum;
            PatNum = patNum;
            DocNum = docNum;
        }

        /// <summary>
        /// If going to the ApptModule, this lets you pick a date.
        /// </summary>
        public DateTime DateSelected { get; }

        /// <summary>
        /// The aptNums of the appointments that we want to put on the pinboard of the Apt Module.
        /// </summary>
        public List<long> ListPinApptNums { get; }

        public long SelectedAptNum { get; }

        public int IModule { get; }

        /// <summary>
        /// If going to Account module, this lets you pick a claim.
        /// </summary>
        public long ClaimNum { get; }

        public long PatNum { get; }

        /// <summary>
        /// If going to Images module, this lets you pick which image.
        /// </summary>
        public long DocNum { get; }
    }


    /// <summary>
    /// Used to log where a backup was initiated from. 
    /// These enum values are named in a way so that they sound good at the end of this sentence: 
    /// "A backup was created when running the [enumValHere]"
    /// </summary>
    public enum BackupLocation
    {
        ConvertScript,
        DatabaseMaintenanceTool,
        OptimizeTool,
        InnoDbTool
    }
}