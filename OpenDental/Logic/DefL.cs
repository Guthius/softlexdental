using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental
{
    public class DefL
    {
        private static string _lanThis = "FormDefinitions";
        #region GetMethods
        public static List<DefCatOptions> GetOptionsForDefCats(Array defCatVals)
        {
            List<DefCatOptions> listDefCatOptions = new List<DefCatOptions>();
            foreach (DefinitionCategory defCatCur in defCatVals)
            {
                if (defCatCur.GetDescription() == "NotUsed")
                {
                    continue;
                }
                if (defCatCur.GetDescription().Contains("HqOnly"))
                {
                    continue;
                }
                DefCatOptions defCOption = new DefCatOptions(defCatCur);
                switch (defCatCur)
                {
                    case DefinitionCategory.AccountColors:
                        defCOption.CanEditName = false;
                        defCOption.EnableColor = true;
                        defCOption.HelpText = Lans.g("FormDefinitions", "Changes the color of text for different types of entries in Account Module");
                        break;
                    case DefinitionCategory.AccountQuickCharge:
                        defCOption.CanDelete = true;
                        defCOption.EnableValue = true;
                        defCOption.ValueText = Lans.g("FormDefinitions", "Procedure Codes");
                        defCOption.HelpText = Lans.g("FormDefinitions", "Account Proc Quick Add items.  Each entry can be a series of procedure codes separated by commas (e.g. D0180,D1101,D8220).  Used in the account module to quickly charge patients for items.");
                        break;
                    case DefinitionCategory.AdjTypes:
                        defCOption.EnableValue = true;
                        defCOption.ValueText = Lans.g("FormDefinitions", "+, -, or dp");
                        defCOption.HelpText = Lans.g("FormDefinitions", "Plus increases the patient balance.  Minus decreases it.  Dp means discount plan.  Not allowed to change value after creating new type since changes affect all patient accounts.");
                        break;
                    case DefinitionCategory.AppointmentColors:
                        defCOption.CanEditName = false;
                        defCOption.EnableColor = true;
                        defCOption.HelpText = Lans.g("FormDefinitions", "Changes colors of background in Appointments Module, and colors for completed appointments.");
                        break;
                    case DefinitionCategory.ApptConfirmed:
                        defCOption.EnableColor = true;
                        defCOption.EnableValue = true;
                        defCOption.ValueText = Lans.g("FormDefinitions", "Abbrev");
                        defCOption.HelpText = Lans.g("FormDefinitions", "Color shows on each appointment if Appointment View is set to show ConfirmedColor.");
                        break;
                    case DefinitionCategory.ApptProcsQuickAdd:
                        defCOption.EnableValue = true;
                        defCOption.ValueText = Lans.g("FormDefinitions", "ADA Code(s)");
                        if (Clinic.GetById(Clinics.ClinicId).IsMedicalOnly)
                        {
                            defCOption.HelpText = Lans.g("FormDefinitions", "These are the procedures that you can quickly add to the treatment plan from within the appointment editing window.  Multiple procedures may be separated by commas with no spaces. These definitions may be freely edited without affecting any patient records.");
                        }
                        else
                        {
                            defCOption.HelpText = Lans.g("FormDefinitions", "These are the procedures that you can quickly add to the treatment plan from within the appointment editing window.  They must not require a tooth number. Multiple procedures may be separated by commas with no spaces. These definitions may be freely edited without affecting any patient records.");
                        }
                        break;
                    case DefinitionCategory.AutoDeposit:
                        defCOption.CanDelete = true;
                        defCOption.CanHide = true;
                        defCOption.EnableValue = true;
                        defCOption.ValueText = Lans.g("FormDefinitions", "Account Number");
                        break;
                    case DefinitionCategory.AutoNoteCats:
                        defCOption.CanDelete = true;
                        defCOption.CanHide = false;
                        defCOption.EnableValue = true;
                        defCOption.IsValueDefNum = true;
                        defCOption.ValueText = Lans.g("FormDefinitions", "Parent Category");
                        defCOption.HelpText = Lans.g("FormDefinitions", "Leave the Parent Category blank for categories at the root level. Assign a Parent Category to move a category within another. The order set here will only affect the order within the assigned Parent Category in the Auto Note list. For example, a category may be moved above its parent in this list, but it will still be within its Parent Category in the Auto Note list.");
                        break;
                    case DefinitionCategory.BillingTypes:
                        defCOption.EnableValue = true;
                        defCOption.ValueText = Lans.g("FormDefinitions", "E=Email bill, C=Collection");
                        defCOption.HelpText = Lans.g("FormDefinitions", "It is recommended to use as few billing types as possible.  They can be useful when running reports to separate delinquent accounts, but can cause 'forgotten accounts' if used without good office procedures. Changes affect all patients.");
                        break;
                    case DefinitionCategory.BlockoutTypes:
                        defCOption.EnableColor = true;
                        defCOption.HelpText = Lans.g("FormDefinitions", "Blockout types are used in the appointments module.");
                        defCOption.EnableValue = true;
                        defCOption.ValueText = Lans.g("FormDefinitions", "Flags");
                        break;
                    case DefinitionCategory.ChartGraphicColors:
                        defCOption.CanEditName = false;
                        defCOption.EnableColor = true;
                        if (Clinic.GetById(Clinics.ClinicId).IsMedicalOnly)
                        {
                            defCOption.HelpText = Lans.g("FormDefinitions", "These colors will be used to graphically display treatments.");
                        }
                        else
                        {
                            defCOption.HelpText = Lans.g("FormDefinitions", "These colors will be used on the graphical tooth chart to draw restorations.");
                        }
                        break;
                    case DefinitionCategory.ClaimCustomTracking:
                        defCOption.CanDelete = true;
                        defCOption.CanHide = false;
                        defCOption.EnableValue = true;
                        defCOption.ValueText = Lans.g("FormDefinitions", "Days Suppressed");
                        defCOption.HelpText = Lans.g("FormDefinitions", "Some offices may set up claim tracking statuses such as 'review', 'hold', 'riskmanage', etc.") + "\r\n"
                            + Lans.g("FormDefinitions", "Set the value of 'Days Suppressed' to the number of days the claim will be suppressed from the Outstanding Claims Report "
                            + "when the status is changed to the selected status.");
                        break;
                    case DefinitionCategory.ClaimErrorCode:
                        defCOption.CanDelete = true;
                        defCOption.CanHide = false;
                        defCOption.EnableValue = true;
                        defCOption.ValueText = Lans.g("FormDefinitions", "Description");
                        defCOption.HelpText = Lans.g("FormDefinitions", "Used to track error codes when entering claim custom statuses.");
                        break;
                    case DefinitionCategory.ClaimPaymentTracking:
                        defCOption.ValueText = Lans.g("FormDefinitions", "Value");
                        defCOption.HelpText = Lans.g("FormDefinitions", "EOB adjudication method codes to be used for insurance payments.  Last entry cannot be hidden.");
                        break;
                    case DefinitionCategory.ClaimPaymentGroups:
                        defCOption.ValueText = Lans.g("FormDefinitions", "Value");
                        defCOption.HelpText = Lans.g("FormDefinitions", "Used to group claim payments in the daily payments report.");
                        break;
                    case DefinitionCategory.ClinicSpecialty:
                        defCOption.CanHide = true;
                        defCOption.CanDelete = false;
                        defCOption.HelpText = Lans.g("FormDefinitions", "You can add as many specialties as you want.  Changes affect all current records.");
                        break;
                    case DefinitionCategory.CommLogTypes:
                        defCOption.EnableValue = true;
                        defCOption.EnableColor = true;
                        defCOption.DoShowNoColor = true;
                        string commItemTypes = string.Join(", ", Commlogs.GetCommItemTypes().Select(x => x.GetDescription(useShortVersionIfAvailable: true)));
                        defCOption.ValueText = Lans.g("FormDefinitions", "Usage");
                        defCOption.HelpText = Lans.g("FormDefinitions", "Changes affect all current commlog entries.  Optionally set Usage to one of the following: "
                            + commItemTypes + ". Only one of each. This helps automate new entries.");
                        break;
                    case DefinitionCategory.ContactCategories:
                        defCOption.HelpText = Lans.g("FormDefinitions", "You can add as many categories as you want.  Changes affect all current contact records.");
                        break;
                    case DefinitionCategory.Diagnosis:
                        defCOption.EnableValue = true;
                        defCOption.ValueText = Lans.g("FormDefinitions", "1 or 2 letter abbreviation");
                        defCOption.HelpText = Lans.g("FormDefinitions", "The diagnosis list is shown when entering a procedure.  Ones that are less used should go lower on the list.  The abbreviation is shown in the progress notes.  BE VERY CAREFUL.  Changes affect all patients.");
                        break;
                    case DefinitionCategory.FeeColors:
                        defCOption.CanEditName = false;
                        defCOption.CanHide = false;
                        defCOption.EnableColor = true;
                        defCOption.HelpText = Lans.g("FormDefinitions", "These are the colors associated to fee types.");
                        break;
                    case DefinitionCategory.ImageCats:
                        defCOption.ValueText = Lans.g("FormDefinitions", "Usage");
                        defCOption.HelpText = Lans.g("FormDefinitions", "These are the categories that will be available in the image and chart modules.  If you hide a category, images in that category will be hidden, so only hide a category if you are certain it has never been used.  Multiple categories can be set to show in the Chart module, but only one category should be set for patient pictures, statements, and tooth charts. Selecting multiple categories for treatment plans will save the treatment plan in each category. Affects all patient records.");
                        break;
                    case DefinitionCategory.InsurancePaymentType:
                        defCOption.CanDelete = true;
                        defCOption.CanHide = false;
                        defCOption.EnableValue = true;
                        defCOption.ValueText = Lans.g("FormDefinitions", "N=Not selected for deposit");
                        defCOption.HelpText = Lans.g("FormDefinitions", "These are claim payment types for insurance payments attached to claims.");
                        break;
                    case DefinitionCategory.InsuranceVerificationStatus:
                        defCOption.HelpText = Lans.g("FormDefinitions", "These are statuses for the insurance verification list.");
                        break;
                    case DefinitionCategory.JobPriorities:
                        defCOption.CanDelete = false;
                        defCOption.CanHide = true;
                        defCOption.EnableValue = true;
                        defCOption.EnableColor = true;
                        defCOption.ValueText = Lans.g("FormDefinitions", "Comma-delimited keywords");
                        defCOption.HelpText = Lans.g("FormDefinitions", "These are job priorities that determine how jobs are sorted in the Job Manager System.  Required values are: OnHold, Low, Normal, MediumHigh, High, Urgent, BugDefault, JobDefault, DocumentationDefault.");
                        break;
                    case DefinitionCategory.LetterMergeCats:
                        defCOption.HelpText = Lans.g("FormDefinitions", "Categories for Letter Merge.  You can safely make any changes you want.");
                        break;
                    case DefinitionCategory.MiscColors:
                        defCOption.CanEditName = false;
                        defCOption.EnableColor = true;
                        defCOption.HelpText = "";
                        break;
                    case DefinitionCategory.PaymentTypes:
                        defCOption.EnableValue = true;
                        defCOption.ValueText = Lans.g("FormDefinitions", "N=Not selected for deposit");
                        defCOption.HelpText = Lans.g("FormDefinitions", "Types of payments that patients might make. Any changes will affect all patients.");
                        break;
                    case DefinitionCategory.PayPlanCategories:
                        defCOption.HelpText = Lans.g("FormDefinitions", "Assign payment plans to different categories");
                        break;
                    case DefinitionCategory.PaySplitUnearnedType:
                        defCOption.HelpText = Lans.g("FormDefinitions", "Usually only used by offices that use accrual basis accounting instead of cash basis accounting. Any changes will affect all patients.");
                        break;
                    case DefinitionCategory.ProcButtonCats:
                        defCOption.HelpText = Lans.g("FormDefinitions", "These are similar to the procedure code categories, but are only used for organizing and grouping the procedure buttons in the Chart module.");
                        break;
                    case DefinitionCategory.ProcCodeCats:
                        defCOption.HelpText = Lans.g("FormDefinitions", "These are the categories for organizing procedure codes. They do not have to follow ADA categories.  There is no relationship to insurance categories which are setup in the Ins Categories section.  Does not affect any patient records.");
                        break;
                    case DefinitionCategory.ProgNoteColors:
                        defCOption.CanEditName = false;
                        defCOption.EnableColor = true;
                        defCOption.HelpText = Lans.g("FormDefinitions", "Changes color of text for different types of entries in the Chart Module Progress Notes.");
                        break;
                    case DefinitionCategory.Prognosis:
                        //Nothing special. Might add HelpText later.
                        break;
                    case DefinitionCategory.ProviderSpecialties:
                        defCOption.HelpText = Lans.g("FormDefinitions", "Provider specialties cannot be deleted.  Changes to provider specialties could affect e-claims.");
                        break;
                    case DefinitionCategory.RecallUnschedStatus:
                        defCOption.EnableValue = true;
                        defCOption.ValueText = Lans.g("FormDefinitions", "Abbreviation");
                        defCOption.HelpText = Lans.g("FormDefinitions", "Recall/Unsched Status.  Abbreviation must be 7 characters or less.  Changes affect all patients.");
                        break;
                    case DefinitionCategory.Regions:
                        defCOption.CanHide = false;
                        defCOption.HelpText = Lans.g("FormDefinitions", "The region identifying the clinic it is assigned to.");
                        break;
                    case DefinitionCategory.SupplyCats:
                        defCOption.CanDelete = true;
                        defCOption.CanHide = false;
                        defCOption.HelpText = Lans.g("FormDefinitions", "The categories for inventory supplies.");
                        break;
                    case DefinitionCategory.TaskPriorities:
                        defCOption.EnableColor = true;
                        defCOption.EnableValue = true;
                        defCOption.ValueText = Lans.g("FormDefinitions", "D = Default, R = Reminder");
                        defCOption.HelpText = Lans.g("FormDefinitions", "Priorities available for selection within the task edit window.  Task lists are sorted using the order of these priorities.  They can have any description and color.  At least one priority should be Default (D).  If more than one priority is flagged as the default, the last default in the list will be used.  If no default is set, the last priority will be used.  Use (R) to indicate the initial reminder task priority to use when creating reminder tasks.  Changes affect all tasks where the definition is used.");
                        break;
                    case DefinitionCategory.TxPriorities:
                        defCOption.EnableColor = true;
                        defCOption.EnableValue = true;
                        defCOption.DoShowItemOrderInValue = true;
                        defCOption.ValueText = Lan.g(_lanThis, "Internal Priority");
                        defCOption.HelpText = Lan.g(_lanThis, "Displayed order should match order of priority of treatment.  They are used in Treatment Plan and Chart "
                            + "modules. They can be simple numbers or descriptive abbreviations 7 letters or less.  Changes affect all procedures where the "
                            + "definition is used.  'Internal Priority' does not show, but is used for list order and for automated selection of which procedures "
                            + "are next in a planned appointment.");
                        break;
                    case DefinitionCategory.WebSchedNewPatApptTypes:
                        defCOption.CanDelete = true;
                        defCOption.CanHide = false;
                        defCOption.ValueText = Lans.g("FormDefinitions", "Appointment Type");
                        defCOption.HelpText = Lans.g("FormDefinitions", "Appointment types to be displayed in the Web Sched New Pat Appt web application.  These are selectable for the new patients and will be saved to the appointment note.");
                        break;
                    case DefinitionCategory.CarrierGroupNames:
                        defCOption.CanHide = true;
                        defCOption.HelpText = Lans.g("FormDefinitions", "These are group names for Carriers.");
                        break;
                }
                listDefCatOptions.Add(defCOption);
            }
            return listDefCatOptions;
        }

        private static string GetItemDescForImages(string itemValue)
        {
            List<string> listVals = new List<string>();
            if (itemValue.Contains("X"))
            {
                listVals.Add(Lan.g(_lanThis, "ChartModule"));
            }
            if (itemValue.Contains("F"))
            {
                listVals.Add(Lan.g(_lanThis, "PatientForm"));
            }
            if (itemValue.Contains("P"))
            {
                listVals.Add(Lan.g(_lanThis, "PatientPic"));
            }
            if (itemValue.Contains("S"))
            {
                listVals.Add(Lan.g(_lanThis, "Statement"));
            }
            if (itemValue.Contains("T"))
            {
                listVals.Add(Lan.g(_lanThis, "ToothChart"));
            }
            if (itemValue.Contains("R"))
            {
                listVals.Add(Lan.g(_lanThis, "TreatPlans"));
            }
            if (itemValue.Contains("L"))
            {
                listVals.Add(Lan.g(_lanThis, "PatientPortal"));
            }
            if (itemValue.Contains("A"))
            {
                listVals.Add(Lan.g(_lanThis, "PayPlans"));
            }
            return string.Join(", ", listVals);
        }
        #endregion
        ///<summary>Fills the passed in grid with the definitions in the passed in list.</summary>
        public static void FillGridDefs(ODGrid gridDefs, DefCatOptions selectedDefCatOpt, List<Definition> listDefsCur)
        {
            Definition selectedDef = null;
            if (gridDefs.GetSelectedIndex() > -1)
            {
                selectedDef = (Definition)gridDefs.Rows[gridDefs.GetSelectedIndex()].Tag;
            }
            int scroll = gridDefs.ScrollValue;
            gridDefs.BeginUpdate();
            gridDefs.Columns.Clear();
            ODGridColumn col;
            col = new ODGridColumn(Lan.g("TableDefs", "Name"), 190);
            gridDefs.Columns.Add(col);
            col = new ODGridColumn(selectedDefCatOpt.ValueText, 190);
            gridDefs.Columns.Add(col);
            col = new ODGridColumn(selectedDefCatOpt.EnableColor ? Lan.g("TableDefs", "Color") : "", 40);
            gridDefs.Columns.Add(col);
            col = new ODGridColumn(selectedDefCatOpt.CanHide ? Lan.g("TableDefs", "Hide") : "", 30, HorizontalAlignment.Center);
            gridDefs.Columns.Add(col);
            gridDefs.Rows.Clear();
            ODGridRow row;
            foreach (Definition defCur in listDefsCur)
            {
                if (Defs.IsDefDeprecated(defCur))
                {
                    defCur.Hidden = true;
                }
                row = new ODGridRow();
                if (selectedDefCatOpt.CanEditName)
                {
                    row.Cells.Add(defCur.Description);
                }
                else
                {//Users cannot edit the item name so let them translate them.
                    row.Cells.Add(Lan.g("FormDefinitions", defCur.Description));//Doesn't use 'this' so that renaming the form doesn't change the translation
                }
                if (selectedDefCatOpt.DefCat == DefinitionCategory.ImageCats)
                {
                    row.Cells.Add(GetItemDescForImages(defCur.Value));
                }
                else if (selectedDefCatOpt.DefCat == DefinitionCategory.AutoNoteCats)
                {
                    Dictionary<string, string> dictAutoNoteDefs = new Dictionary<string, string>();
                    dictAutoNoteDefs = listDefsCur.ToDictionary(x => x.Id.ToString(), x => x.Description);
                    string nameCur;
                    row.Cells.Add(dictAutoNoteDefs.TryGetValue(defCur.Value, out nameCur) ? nameCur : defCur.Value);
                }
                else if (selectedDefCatOpt.DefCat == DefinitionCategory.WebSchedNewPatApptTypes)
                {
                    AppointmentType appointmentType = AppointmentType.GetWebSchedNewPatApptTypeByDef(defCur.Id);
                    row.Cells.Add(appointmentType == null ? "" : appointmentType.Name);
                }
                else if (selectedDefCatOpt.DoShowItemOrderInValue)
                {
                    row.Cells.Add(defCur.SortOrder.ToString());
                }
                else
                {
                    row.Cells.Add(defCur.Value);
                }
                row.Cells.Add("");
                if (selectedDefCatOpt.EnableColor)
                {
                    row.Cells[row.Cells.Count - 1].CellColor = defCur.Color;
                }
                if (defCur.Hidden)
                {
                    row.Cells.Add("X");
                }
                else
                {
                    row.Cells.Add("");
                }
                row.Tag = defCur;
                gridDefs.Rows.Add(row);
            }
            gridDefs.EndUpdate();
            if (selectedDef != null)
            {
                for (int i = 0; i < gridDefs.Rows.Count; i++)
                {
                    if (((Definition)gridDefs.Rows[i].Tag).Id == selectedDef.Id)
                    {
                        gridDefs.SetSelected(i, true);
                        break;
                    }
                }
            }
            gridDefs.ScrollValue = scroll;
        }

        public static bool GridDefsDoubleClick(Definition selectedDef, ODGrid gridDefs, DefCatOptions selectedDefCatOpt, List<Definition> listDefsCur, List<Definition> listDefsAll, bool isDefChanged)
        {
            switch (selectedDefCatOpt.DefCat)
            {
                case DefinitionCategory.BlockoutTypes:
                    FormDefEditBlockout FormDEB = new FormDefEditBlockout(selectedDef);
                    FormDEB.ShowDialog();
                    if (FormDEB.DialogResult == DialogResult.OK)
                    {
                        isDefChanged = true;
                    }
                    break;
                case DefinitionCategory.ImageCats:
                    FormDefEditImages FormDEI = new FormDefEditImages(selectedDef);
                    FormDEI.IsNew = false;
                    FormDEI.ShowDialog();
                    if (FormDEI.DialogResult == DialogResult.OK)
                    {
                        isDefChanged = true;
                    }
                    break;
                case DefinitionCategory.WebSchedNewPatApptTypes:
                    FormDefEditWSNPApptTypes FormDEWSNPAT = new FormDefEditWSNPApptTypes(selectedDef);
                    if (FormDEWSNPAT.ShowDialog() == DialogResult.OK)
                    {
                        if (FormDEWSNPAT.IsDeleted)
                        {
                            listDefsAll.Remove(selectedDef);
                        }
                        isDefChanged = true;
                    }
                    break;
                default://Show the normal FormDefEdit window.
                    FormDefEdit FormDefEdit2 = new FormDefEdit(selectedDef, listDefsCur, selectedDefCatOpt);
                    FormDefEdit2.IsNew = false;
                    FormDefEdit2.ShowDialog();
                    if (FormDefEdit2.DialogResult == DialogResult.OK)
                    {
                        if (FormDefEdit2.IsDeleted)
                        {
                            listDefsAll.Remove(selectedDef);
                        }
                        isDefChanged = true;
                    }
                    break;
            }
            return isDefChanged;
        }

        public static bool AddDef(ODGrid gridDefs, DefCatOptions selectedDefCatOpt)
        {
            Definition defCur = new Definition();
            int itemOrder = 0;
            if (Definition.GetByCategory(selectedDefCatOpt.DefCat).Count > 0)
            {
                itemOrder = Definition.GetByCategory(selectedDefCatOpt.DefCat).Max(x => x.SortOrder) + 1;
            }
            defCur.SortOrder = itemOrder;
            defCur.Category = selectedDefCatOpt.DefCat;
            defCur.Description = "";
            defCur.Value = "";//necessary
            if (selectedDefCatOpt.DefCat == DefinitionCategory.InsurancePaymentType)
            {
                defCur.Value = "N";
            }
            switch (selectedDefCatOpt.DefCat)
            {
                case DefinitionCategory.BlockoutTypes:
                    FormDefEditBlockout FormDEB = new FormDefEditBlockout(defCur);
                    FormDEB.IsNew = true;
                    if (FormDEB.ShowDialog() != DialogResult.OK)
                    {
                        return false;
                    }
                    break;
                case DefinitionCategory.ImageCats:
                    FormDefEditImages FormDEI = new FormDefEditImages(defCur);
                    FormDEI.IsNew = true;
                    FormDEI.ShowDialog();
                    if (FormDEI.DialogResult != DialogResult.OK)
                    {
                        return false;
                    }
                    break;
                case DefinitionCategory.WebSchedNewPatApptTypes:
                    FormDefEditWSNPApptTypes FormDEWSNPAT = new FormDefEditWSNPApptTypes(defCur);
                    if (FormDEWSNPAT.ShowDialog() != DialogResult.OK)
                    {
                        return false;
                    }
                    break;
                default:
                    List<Definition> listCurrentDefs = new List<Definition>();
                    foreach (ODGridRow rowCur in gridDefs.Rows)
                    {
                        listCurrentDefs.Add((Definition)rowCur.Tag);
                    }
                    FormDefEdit FormDE = new FormDefEdit(defCur, listCurrentDefs, selectedDefCatOpt);
                    FormDE.IsNew = true;
                    FormDE.ShowDialog();
                    if (FormDE.DialogResult != DialogResult.OK)
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }

        public static bool HideDef(ODGrid gridDefs, DefCatOptions selectedDefCatOpt)
        {
            if (gridDefs.GetSelectedIndex() == -1)
            {
                MsgBox.Show(_lanThis, "Please select item first,");
                return false;
            }
            Definition selectedDef = (Definition)gridDefs.Rows[gridDefs.GetSelectedIndex()].Tag;

            //Warn the user if they are about to hide a billing type currently in use.
            if (selectedDefCatOpt.DefCat == DefinitionCategory.BillingTypes && Patients.IsBillingTypeInUse(selectedDef.Id))
            {
                if (!MsgBox.Show(_lanThis, MsgBoxButtons.OKCancel, "Warning: Billing type is currently in use by patients, insurance plans, or preferences."))
                {
                    return false;
                }
            }

            if (selectedDef.Category == DefinitionCategory.ProviderSpecialties && (Providers.IsSpecialtyInUse(selectedDef.Id) || Referrals.IsSpecialtyInUse(selectedDef.Id)))
            {
                MsgBox.Show(_lanThis, "You cannot hide a specialty if it is in use by a provider or a referral source.");
                return false;
            }

            if (Defs.IsDefinitionInUse(selectedDef))
            {
                if (selectedDef.Id == Preference.GetLong(PreferenceName.BrokenAppointmentAdjustmentType) || 
                    selectedDef.Id == Preference.GetLong(PreferenceName.AppointmentTimeArrivedTrigger) || 
                    selectedDef.Id == Preference.GetLong(PreferenceName.AppointmentTimeSeatedTrigger) || 
                    selectedDef.Id == Preference.GetLong(PreferenceName.AppointmentTimeDismissedTrigger) || 
                    selectedDef.Id == Preference.GetLong(PreferenceName.TreatPlanDiscountAdjustmentType) || 
                    selectedDef.Id == Preference.GetLong(PreferenceName.BillingChargeAdjustmentType) || 
                    selectedDef.Id == Preference.GetLong(PreferenceName.PracticeDefaultBillType) || 
                    selectedDef.Id == Preference.GetLong(PreferenceName.FinanceChargeAdjustmentType) || 
                    selectedDef.Id == Preference.GetLong(PreferenceName.RecurringChargesPayTypeCC))
                {
                    MsgBox.Show(_lanThis, "You cannot hide a definition if it is in use within Module Preferences.");
                    return false;
                }
                else
                {
                    if (!MsgBox.Show(_lanThis, MsgBoxButtons.OKCancel, "Warning: This definition is currently in use within the program."))
                    {
                        return false;
                    }
                }
            }

            //Stop users from hiding the last definition in categories that must have at least one def in them.
            if (Defs.IsHidable(selectedDef.Category))
            {
                List<Definition> listDefsCurNotHidden = Definition.GetByCategory(selectedDefCatOpt.DefCat);
                if (listDefsCurNotHidden.Count == 1)
                {
                    MsgBox.Show(_lanThis, "You cannot hide the last definition in this category.");
                    return false;
                }
            }
            selectedDef.Hidden = true;
            Definition.Update(selectedDef);
            return true;
        }

        public static bool UpClick(ODGrid gridDefs)
        {
            if (gridDefs.GetSelectedIndex() == -1)
            {
                MessageBox.Show(Lan.g("Defs", "Please select an item first."));
                return false;
            }
            if (gridDefs.GetSelectedIndex() == 0)
            {
                return false;
            }
            Definition defSelected = (Definition)gridDefs.Rows[gridDefs.GetSelectedIndex()].Tag;
            Definition defAbove = (Definition)gridDefs.Rows[gridDefs.GetSelectedIndex() - 1].Tag;
            int indexDefSelectedItemOrder = defSelected.SortOrder;
            defSelected.SortOrder = defAbove.SortOrder;
            defAbove.SortOrder = indexDefSelectedItemOrder;
            Definition.Update(defSelected);
            Definition.Update(defAbove);
            return true;
        }


        public static bool DownClick(ODGrid gridDefs)
        {
            if (gridDefs.GetSelectedIndex() == -1)
            {
                MessageBox.Show(Lan.g("Defs", "Please select an item first."));
                return false;
            }
            if (gridDefs.GetSelectedIndex() == gridDefs.Rows.Count - 1)
            {
                return false;
            }
            Definition defSelected = (Definition)gridDefs.Rows[gridDefs.GetSelectedIndex()].Tag;
            Definition defBelow = (Definition)gridDefs.Rows[gridDefs.GetSelectedIndex() + 1].Tag;
            int indexDefSelectedItemOrder = defSelected.SortOrder;
            defSelected.SortOrder = defBelow.SortOrder;
            defBelow.SortOrder = indexDefSelectedItemOrder;
            Definition.Update(defSelected);
            Definition.Update(defBelow);
            return true;
        }

    }
}