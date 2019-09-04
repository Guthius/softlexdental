using OpenDental.User_Controls.SetupWizard;
using OpenDentBusiness;
using System;
using System.Drawing;
using System.Linq;

namespace OpenDental
{
    public partial class SetupWizard
    {
        public class SetupWizardStep
        {
            public SetupWizardStep(string name, ODSetupCategory category, SetupWizardControl control)
            {
                Name = name;
                Category = category;
                Control = control;
            }

            public SetupWizardStep(string name, string description, ODSetupCategory category, SetupWizardControl control)
            {
                Name = name;
                Description = description;
                Category = category;
                Control = control;
            }

            /// <summary>
            /// Gets the name of the setup item that will appear in the list of setup items.
            /// This should be one or a few words. It needs to make sense in the following sentences (where XX is the Name):
            /// "Let's set up your XX..."
            /// "Congratulations! You have finished setting up your XX!"
            /// "You can always go back through this setup wizard if you need to make any modifications to your XX."
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Gets a description and explanation for this setup item.
            /// </summary>
            public virtual string Description { get; }

            /// <summary>
            /// Gets the category for this setup item.
            /// </summary>
            public ODSetupCategory Category { get; }

            /// <summary>
            /// Gets the status of this setup item.
            /// </summary>
            public virtual ODSetupStatus Status { get; } = ODSetupStatus.Optional;

            /// <summary>
            /// Gets the setup wizard control.
            /// </summary>
            public SetupWizardControl Control { get; }
        }

        public class SetupIntro : SetupWizardStep
        {
            public SetupIntro(string name, string description) : 
                base(name, description, ODSetupCategory.None, new SetupWizardControlIntro(name, description))
            {
            }
        }

        public class SetupComplete : SetupWizardStep
        {
            public SetupComplete(string name) :
                base(name, "", ODSetupCategory.None, new SetupWizardControlComplete(name))
            {
            }
        }

        public class RegKeySetup : SetupWizardStep
        {
            public RegKeySetup(): 
                base("Registration Key", ODSetupCategory.PreSetup, new SetupWizardControlRegistrationKey())
            {
            }

            public override string Description
            {
                get
                {
                    string description = 
                        "Some items need to be set up before the program can be used effectively.\r\n" +
                        "This wizard's purpose is to help you quickly set those items up so that you can get started using the program.";

                    if (Status != ODSetupStatus.Complete)
                    {
                        description += "\r\n\r\nIt looks like you have yet to enter your Registration Key.";
                    }

                    description += "\r\nEntering your Registration Key is a necessary first step in order for the program to function.";
                    return description;
                }
            }

            public override ODSetupStatus Status
            {
                get
                {
                    if (string.IsNullOrEmpty(Preference.GetString(PreferenceName.RegistrationKey)))
                    {
                        return ODSetupStatus.NotStarted;
                    }
                    return ODSetupStatus.Complete;
                }
            }
        }

        public class FeatureSetup : SetupWizardStep
        {
            public FeatureSetup() : 
                base(
                    "Basic Features", 
                    "Turn features that your office uses on/off. Settings will affect all computers using the same database.", 
                    ODSetupCategory.Basic, new UserControlSetupWizFeatures())
            {
            }
        }

        public class ClinicSetup : SetupWizardStep
        {
            public ClinicSetup() :
                base(
                    "Clinics",
                    "You have indicated that you will be using the Clinics feature. " +
                    "Clinics can be used when you have multiple locations. Once clinics are set up, you " +
                    "can assign clinics throughout Open Dental. If you follow basic guidelines, default " +
                    "clinic assignments for patient information should be accurate, thus reducing data entry.",
                    ODSetupCategory.Basic,
                    new SetupWizardControlClinics())
            {

            }

            public override ODSetupStatus Status
            {
                get
                {
                    var clinics = Clinics.GetDeepCopy(true);
                    if (clinics.Count == 0)
                    {
                        return ODSetupStatus.NotStarted;
                    }

                    foreach (var clinic in clinics)
                    {
                        if (string.IsNullOrEmpty(clinic.Abbr) || 
                            string.IsNullOrEmpty(clinic.Description) ||
                            string.IsNullOrEmpty(clinic.Phone) || 
                            string.IsNullOrEmpty(clinic.Address))
                        {
                            return ODSetupStatus.NeedsAttention;
                        }
                    }

                    return ODSetupStatus.Complete;
                }
            }
        }

        public class DefinitionSetup : SetupWizardStep
        {
            public DefinitionSetup() :
                base(
                    "Definitions",
                    "Definitions are an easy way to customize your software experience. Setup the " +
                    "colors, categories, and other customizable areas within the program from this " +
                    "window.\r\nWe've selected some of the definitions you may be interested in " +
                    "customizing for this Setup Wizard. You may view the entire list of definitions " +
                    "by going to Setup -> Definitions from the main tool bar.",
                    ODSetupCategory.Basic, 
                    new UserControlSetupWizDefinitions())
            {
            }

            public override ODSetupStatus Status => ODSetupStatus.Optional;
        }

        public class ProvSetup : SetupWizardStep
        {
            public ProvSetup() :
                base(
                    "Providers",
                    "Providers will show up in almost every part of OpenDental." +
                    "It is important that all provider information is up-to-date so that claims, " +
                    "reports, procedures, fee schedules, and estimates will function correctly.",
                    ODSetupCategory.Basic,
                    new UserControlSetupWizProvider())
            {
            }

            public override ODSetupStatus Status
            {
                get
                {
                    var providers = Providers.GetDeepCopy(true);
                    if (providers.Count == 0)
                    {
                        return ODSetupStatus.NotStarted;
                    }

                    foreach (var provider in providers)
                    {
                        bool isDentist = IsPrimary(provider);
                        bool isHyg = provider.IsSecondary;

                        if (((isDentist || isHyg) && string.IsNullOrEmpty(provider.Abbr)) ||
                            ((isDentist || isHyg) && string.IsNullOrEmpty(provider.LName)) ||
                            ((isDentist || isHyg) && string.IsNullOrEmpty(provider.FName)) ||
                            (isDentist && string.IsNullOrEmpty(provider.Suffix)) ||
                            (isDentist && string.IsNullOrEmpty(provider.SSN)) ||
                            (isDentist && string.IsNullOrEmpty(provider.NationalProvID)))
                        {
                            // TODO: It would be nice if we had a way to communicate what is still is need of attention...

                            return ODSetupStatus.NeedsAttention; 
                        }
                    }

                    return ODSetupStatus.Complete;
                }
            }

            public static bool IsPrimary(Provider provider) // TODO: Move this to the Provider class.
            {
                if (provider.IsSecondary || provider.IsNotPerson) return false;

                if (Defs.GetName(DefinitionCategory.ProviderSpecialties, provider.Specialty).ToLower() == "hygienist" ||
                    Defs.GetName(DefinitionCategory.ProviderSpecialties, provider.Specialty).ToLower() == "assistant" || 
                    Defs.GetName(DefinitionCategory.ProviderSpecialties, provider.Specialty).ToLower() == "labtech" || 
                    Defs.GetName(DefinitionCategory.ProviderSpecialties, provider.Specialty).ToLower() == "other" || 
                    Defs.GetName(DefinitionCategory.ProviderSpecialties, provider.Specialty).ToLower() == "notes" || 
                    Defs.GetName(DefinitionCategory.ProviderSpecialties, provider.Specialty).ToLower() == "none")
                {
                    return false;
                }

                return true;
            }
        }

        public class OperatorySetup : SetupWizardStep
        {
            public OperatorySetup() :
                base(
                    "Operatories",
                    "Operatories define locations in which appointments take place, and are used " +
                    "to organize appointments shown on the schedule. Normally, every chair in your " +
                    "office will have an unique operatory.", 
                    ODSetupCategory.Basic,
                    new UserControlSetupWizOperatory())
            {
            }

            public override ODSetupStatus Status
            {
                get
                {
                    var operatories = Operatories.GetDeepCopy(true);
                    if (operatories.Count == 0)
                    {
                        return ODSetupStatus.NotStarted;
                    }

                    foreach (var operatory in operatories)
                    {
                        if (string.IsNullOrEmpty(operatory.OpName) || 
                            string.IsNullOrEmpty(operatory.Abbrev))
                        {
                            return ODSetupStatus.NeedsAttention;
                        }
                    }

                    return ODSetupStatus.Complete;
                }
            }
        }

        public class PracticeSetup : SetupWizardStep
        {
            public PracticeSetup() :
                base(
                    "Practice Info",
                    "Practice information includes general contact information, billing and pay-to " +
                    "addresses, and default providers. This information will appear on most " +
                    "reports, claims, statements, and letters.", 
                    ODSetupCategory.Basic, 
                    new UserControlSetupWizPractice())
            {
            }

            public override ODSetupStatus Status => ODSetupStatus.Complete;
        }

        public class EmployeeSetup : SetupWizardStep
        {
            public EmployeeSetup() :
                base(
                    "Employees", 
                    "The Employee list is used to set up User profiles in Security and to set up " +
                    "Schedules. This list also determines who can use the Time Clock.", 
                    ODSetupCategory.Basic, 
                    new UserControlSetupWizEmployee())
            {
            }

            public override ODSetupStatus Status
            {
                get
                {
                    var employees = Employee.All();
                    if (employees.Count == 0)
                    {
                        return ODSetupStatus.NotStarted;
                    }

                    foreach (var employee in employees)
                    {
                        if (string.IsNullOrEmpty(employee.FirstName) || 
                            string.IsNullOrEmpty(employee.LastName))
                        {
                            return ODSetupStatus.NeedsAttention;
                        }
                    }

                    return ODSetupStatus.Complete;
                }
            }
        }

        public class FeeSchedSetup : SetupWizardStep
        {
            public FeeSchedSetup() :
                base(
                    "Fee Schedules",
                    "Fee Schedules determine the fees billed for each procedure.", 
                    ODSetupCategory.Basic, 
                    new UserControlSetupWizFeeSched())
            {
            }

            /// <summary>
            /// Returns Complete if all fee schedules have at least one fee entered.
            /// </summary>
            public override ODSetupStatus Status
            {
                get
                {
                    var feeSchedules = FeeScheds.GetDeepCopy(true);
                    if (feeSchedules.Count == 0)
                    {
                        return ODSetupStatus.NotStarted;
                    }

                    var feeScheduleIds = feeSchedules.Select(x => x.FeeSchedNum).ToList();
                    foreach (var feeScheduleId in feeScheduleIds)
                    {
                        if (Fees.GetCountByFeeSchedNum(feeScheduleId) <= 0)
                        {
                            return ODSetupStatus.NeedsAttention;
                        }
                    }

                    return ODSetupStatus.Complete;
                }
            }
        }

        public class PrinterSetup : SetupWizardStep
        {
            public PrinterSetup() :
                base(
                    "Printer/Scanner",
                    "Set up print and scan options for the current workstation. You can leave all " +
                    "settings to the default, or you can control where specific items are are " +
                    "printed.", 
                    ODSetupCategory.Basic, 
                    new UserControlSetupWizPrinter())
            {
            }
        }

        public class ScheduleSetup : SetupWizardStep
        {
            public ScheduleSetup() :
                base(
                    "Schedule",
                    "Schedule setup lets you enter all Provider and Employee schedules. You can " +
                    "define any kind of rotating or alternating schedule you want. Enter " +
                    "individual work hours, holidays, lunch hours, and staff meetings. Once " +
                    "schedules are entered, open/closed hours will be indicated in the Appointment " +
                    "module.",
                    ODSetupCategory.Basic, 
                    new UserControlSetupWizSchedule())
            {
            }

            public override ODSetupStatus Status
            {
                get
                {
                    var schedules = Schedules.GetTwoYearPeriod(DateTime.Today.AddYears(-1));
                    if (schedules.Count == 0)
                    {
                        return ODSetupStatus.NotStarted;
                    }

                    var providers = Providers.GetWhere(x => !x.IsNotPerson, true).ToList();
                    foreach (var provider in providers)
                    {
                        if (!schedules.Select(x => x.ProvNum).Contains(provider.ProvNum))
                        {
                            return ODSetupStatus.NeedsAttention;
                        }
                    }

                    return ODSetupStatus.Complete;
                }
            }
        }

        public static Color GetColor(ODSetupStatus status)
        {
            switch (status)
            {
                case ODSetupStatus.NotStarted:
                case ODSetupStatus.NeedsAttention:
                    return Color.FromArgb(255, 255, 204, 204);

                case ODSetupStatus.Complete:
                case ODSetupStatus.Optional:
                    return Color.FromArgb(255, 204, 255, 204);

                default:
                    return Color.White;
            }
        }
    }

    public enum ODSetupCategory
    {
        Misc,
        None,
        PreSetup,
        Basic,
        Advanced,
    }

    public enum ODSetupStatus
    {
        /// <summary>
        /// User hasn't started this setup item.
        /// </summary>
        NotStarted,

        /// <summary>
        /// User has left this setup item in an incomplete state.
        /// </summary>
        NeedsAttention,

        /// <summary>
        /// Setup item has been considered and required elements have been filled in.
        /// </summary>
        Complete,

        /// <summary>
        /// Setup item is not required.
        /// </summary>
        Optional
    }

    public interface ISetupWizard
    {
        /// <summary>
        /// Gets the name of the wizard.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a description of the wizard.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Raised when the <see cref="Description"/> is changed.
        /// </summary>
        event EventHandler DescriptionChanged;

        /// <summary>
        /// Gets the category of the wizard.
        /// </summary>
        SetupCategory Category { get; }

        /// <summary>
        /// Gets the status of the wizard.
        /// </summary>
        SetupStatus Status { get; }

        /// <summary>
        /// Gets the wizard control.
        /// </summary>
        SetupWizardControl Control { get; }

        /// <summary>
        /// Validates the wizard.
        /// </summary>
        /// <returns>True if the wizard validated succesfully; otherwise, false.</returns>
        bool Validate();

        /// <summary>
        /// Completes the wizard.
        /// </summary>
        void Complete();
    }

    public class SetupWizard<TControl> : ISetupWizard where TControl : SetupWizardControl
    {
        string description;
        readonly TControl control;

        /// <summary>
        /// Gets the name of the wizard.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Raised when the <see cref="Description"/> is changed.
        /// </summary>
        public string Description
        {
            get => description;
            protected set
            {
                if (value != description)
                {
                    description = value ?? "";

                    OnDescriptionChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets the category of the wizard.
        /// </summary>
        public SetupCategory Category { get; }

        /// <summary>
        /// Gets the status of the wizard.
        /// </summary>
        public SetupStatus Status { get; protected set; } = SetupStatus.NotStarted;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupWizard"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="category"></param>
        public SetupWizard(string name, string description, SetupCategory category)
        {
            Name = name;
            Description = description;
            Category = category;

            try
            {
                control = Activator.CreateInstance<TControl>();
            }
            catch { }
        }

        /// <summary>
        /// Raised when the <see cref="Description"/> is changed.
        /// </summary>
        public event EventHandler DescriptionChanged;

        /// <summary>
        /// Raises the <see cref="DescriptionChanged"/> event.
        /// </summary>
        protected virtual void OnDescriptionChanged(EventArgs e) => DescriptionChanged?.Invoke(this, e);

        /// <summary>
        /// Gets the wizard control.
        /// </summary>
        public SetupWizardControl Control => control;

        /// <summary>
        /// Validates the wizard.
        /// </summary>
        /// <returns>True if the wizard validated succesfully; otherwise, false.</returns>
        public bool Validate() => OnValidate();

        /// <summary>
        /// Validates the wizard.
        /// </summary>
        /// <returns>True if the wizard validated succesfully; otherwise, false.</returns>
        protected virtual bool OnValidate() => true;

        /// <summary>
        /// Completes the wizard.
        /// </summary>
        public void Complete() => OnComplete();

        /// <summary>
        /// Called when the wizard is completed.
        /// </summary>
        protected virtual void OnComplete()
        {
        }
    }

    public enum SetupCategory
    {
        None,
        Miscellaneous,
        Prerequisite,
        Basic,
        Advanced,
        Optional
    }

    public enum SetupStatus
    {
        NotStarted,
        Complete,
        NeedsAttention
    }
}
