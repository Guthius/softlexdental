using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
                        defCOption.HelpText = "Changes the color of text for different types of entries in Account Module";
                        break;

                    case DefinitionCategory.AccountQuickCharge:
                        defCOption.CanDelete = true;
                        defCOption.EnableValue = true;
                        defCOption.ValueText = "Procedure Codes";
                        defCOption.HelpText = "Account Proc Quick Add items.  Each entry can be a series of procedure codes separated by commas (e.g. D0180,D1101,D8220).  Used in the account module to quickly charge patients for items.";
                        break;

                    case DefinitionCategory.AdjTypes:
                        defCOption.EnableValue = true;
                        defCOption.ValueText = "+, -, or dp";
                        defCOption.HelpText = "Plus increases the patient balance.  Minus decreases it.  Dp means discount plan.  Not allowed to change value after creating new type since changes affect all patient accounts.";
                        break;

                    case DefinitionCategory.AppointmentColors:
                        defCOption.CanEditName = false;
                        defCOption.EnableColor = true;
                        defCOption.HelpText = "Changes colors of background in Appointments Module, and colors for completed appointments.";
                        break;

                    case DefinitionCategory.ApptConfirmed:
                        defCOption.EnableColor = true;
                        defCOption.EnableValue = true;
                        defCOption.ValueText = "Abbrev";
                        defCOption.HelpText = "Color shows on each appointment if Appointment View is set to show ConfirmedColor.";
                        break;

                    case DefinitionCategory.ApptProcsQuickAdd:
                        defCOption.EnableValue = true;
                        defCOption.ValueText = Lans.g("FormDefinitions", "ADA Code(s)");
                        if (Clinic.GetById(Clinics.ClinicId).IsMedicalOnly)
                        {
                            defCOption.HelpText = "These are the procedures that you can quickly add to the treatment plan from within the appointment editing window.  Multiple procedures may be separated by commas with no spaces. These definitions may be freely edited without affecting any patient records.";
                        }
                        else
                        {
                            defCOption.HelpText ="These are the procedures that you can quickly add to the treatment plan from within the appointment editing window.  They must not require a tooth number. Multiple procedures may be separated by commas with no spaces. These definitions may be freely edited without affecting any patient records.";
                        }
                        break;

                    case DefinitionCategory.AutoDeposit:
                        defCOption.CanDelete = true;
                        defCOption.CanHide = true;
                        defCOption.EnableValue = true;
                        defCOption.ValueText = "Account Number";
                        break;

                    case DefinitionCategory.AutoNoteCats:
                        defCOption.CanDelete = true;
                        defCOption.CanHide = false;
                        defCOption.EnableValue = true;
                        defCOption.IsValueDefNum = true;
                        defCOption.ValueText = "Parent Category";
                        defCOption.HelpText = "Leave the Parent Category blank for categories at the root level. Assign a Parent Category to move a category within another. The order set here will only affect the order within the assigned Parent Category in the Auto Note list. For example, a category may be moved above its parent in this list, but it will still be within its Parent Category in the Auto Note list.";
                        break;

                    case DefinitionCategory.BillingTypes:
                        defCOption.EnableValue = true;
                        defCOption.ValueText = "E=Email bill, C=Collection";
                        defCOption.HelpText = "It is recommended to use as few billing types as possible.  They can be useful when running reports to separate delinquent accounts, but can cause 'forgotten accounts' if used without good office procedures. Changes affect all patients.";
                        break;

                    case DefinitionCategory.BlockoutTypes:
                        defCOption.EnableColor = true;
                        defCOption.HelpText = "Blockout types are used in the appointments module.";
                        defCOption.EnableValue = true;
                        defCOption.ValueText = "Flags";
                        break;

                    case DefinitionCategory.ChartGraphicColors:
                        defCOption.CanEditName = false;
                        defCOption.EnableColor = true;
                        if (Clinic.GetById(Clinics.ClinicId).IsMedicalOnly)
                        {
                            defCOption.HelpText = "These colors will be used to graphically display treatments.";
                        }
                        else
                        {
                            defCOption.HelpText = "These colors will be used on the graphical tooth chart to draw restorations.";
                        }
                        break;

                    case DefinitionCategory.ClaimCustomTracking:
                        defCOption.CanDelete = true;
                        defCOption.CanHide = false;
                        defCOption.EnableValue = true;
                        defCOption.ValueText = "Days Suppressed";
                        defCOption.HelpText = "Some offices may set up claim tracking statuses such as 'review', 'hold', 'riskmanage', etc.\r\n"
                            + "Set the value of 'Days Suppressed' to the number of days the claim will be suppressed from the Outstanding Claims Report "
                            + "when the status is changed to the selected status.";
                        break;
                    case DefinitionCategory.ClaimErrorCode:
                        defCOption.CanDelete = true;
                        defCOption.CanHide = false;
                        defCOption.EnableValue = true;
                        defCOption.ValueText = "Description";
                        defCOption.HelpText = "Used to track error codes when entering claim custom statuses.";
                        break;
                    case DefinitionCategory.ClaimPaymentTracking:
                        defCOption.ValueText = "Value";
                        defCOption.HelpText = "EOB adjudication method codes to be used for insurance payments.  Last entry cannot be hidden.";
                        break;
                    case DefinitionCategory.ClaimPaymentGroups:
                        defCOption.ValueText = "Value";
                        defCOption.HelpText = "Used to group claim payments in the daily payments report.";
                        break;
                    case DefinitionCategory.ClinicSpecialty:
                        defCOption.CanHide = true;
                        defCOption.CanDelete = false;
                        defCOption.HelpText = "You can add as many specialties as you want.  Changes affect all current records.";
                        break;
                    case DefinitionCategory.CommLogTypes:
                        defCOption.EnableValue = true;
                        defCOption.EnableColor = true;
                        defCOption.DoShowNoColor = true;
                        string commItemTypes = string.Join(", ", Commlogs.GetCommItemTypes().Select(x => x.GetDescription(useShortVersionIfAvailable: true)));
                        defCOption.ValueText = "Usage";
                        defCOption.HelpText = "Changes affect all current commlog entries.  Optionally set Usage to one of the following: "
                            + commItemTypes + ". Only one of each. This helps automate new entries.";
                        break;
                    case DefinitionCategory.ContactCategories:
                        defCOption.HelpText = "You can add as many categories as you want.  Changes affect all current contact records.";
                        break;
                    case DefinitionCategory.Diagnosis:
                        defCOption.EnableValue = true;
                        defCOption.ValueText = "1 or 2 letter abbreviation";
                        defCOption.HelpText = "The diagnosis list is shown when entering a procedure.  Ones that are less used should go lower on the list.  The abbreviation is shown in the progress notes.  BE VERY CAREFUL.  Changes affect all patients.";
                        break;
                    case DefinitionCategory.FeeColors:
                        defCOption.CanEditName = false;
                        defCOption.CanHide = false;
                        defCOption.EnableColor = true;
                        defCOption.HelpText = "These are the colors associated to fee types.";
                        break;
                    case DefinitionCategory.ImageCats:
                        defCOption.ValueText = "Usage";
                        defCOption.HelpText = "These are the categories that will be available in the image and chart modules.  If you hide a category, images in that category will be hidden, so only hide a category if you are certain it has never been used.  Multiple categories can be set to show in the Chart module, but only one category should be set for patient pictures, statements, and tooth charts. Selecting multiple categories for treatment plans will save the treatment plan in each category. Affects all patient records.";
                        break;
                    case DefinitionCategory.InsurancePaymentType:
                        defCOption.CanDelete = true;
                        defCOption.CanHide = false;
                        defCOption.EnableValue = true;
                        defCOption.ValueText = "N=Not selected for deposit";
                        defCOption.HelpText = "These are claim payment types for insurance payments attached to claims.";
                        break;

                    case DefinitionCategory.InsuranceVerificationStatus:
                        defCOption.HelpText = "These are statuses for the insurance verification list.";
                        break;

                    case DefinitionCategory.LetterMergeCats:
                        defCOption.HelpText = "Categories for Letter Merge. You can safely make any changes you want.";
                        break;

                    case DefinitionCategory.MiscColors:
                        defCOption.CanEditName = false;
                        defCOption.EnableColor = true;
                        defCOption.HelpText = "";
                        break;

                    case DefinitionCategory.PaymentTypes:
                        defCOption.EnableValue = true;
                        defCOption.ValueText = "N=Not selected for deposit";
                        defCOption.HelpText = "Types of payments that patients might make. Any changes will affect all patients.";
                        break;

                    case DefinitionCategory.PayPlanCategories:
                        defCOption.HelpText = "Assign payment plans to different categories";
                        break;

                    case DefinitionCategory.PaySplitUnearnedType:
                        defCOption.HelpText = "Usually only used by offices that use accrual basis accounting instead of cash basis accounting. Any changes will affect all patients.";
                        break;

                    case DefinitionCategory.ProcButtonCats:
                        defCOption.HelpText = "These are similar to the procedure code categories, but are only used for organizing and grouping the procedure buttons in the Chart module.";
                        break;
                    case DefinitionCategory.ProcCodeCats:
                        defCOption.HelpText = "These are the categories for organizing procedure codes. They do not have to follow ADA categories.  There is no relationship to insurance categories which are setup in the Ins Categories section.  Does not affect any patient records.";
                        break;
                    case DefinitionCategory.ProgNoteColors:
                        defCOption.CanEditName = false;
                        defCOption.EnableColor = true;
                        defCOption.HelpText = "Changes color of text for different types of entries in the Chart Module Progress Notes.";
                        break;
                    case DefinitionCategory.Prognosis:
                        //Nothing special. Might add HelpText later.
                        break;
                    case DefinitionCategory.ProviderSpecialties:
                        defCOption.HelpText = "Provider specialties cannot be deleted.  Changes to provider specialties could affect e-claims.";
                        break;
                    case DefinitionCategory.RecallUnschedStatus:
                        defCOption.EnableValue = true;
                        defCOption.ValueText = "Abbreviation";
                        defCOption.HelpText = "Recall/Unsched Status.  Abbreviation must be 7 characters or less.  Changes affect all patients.";
                        break;
                    case DefinitionCategory.Regions:
                        defCOption.CanHide = false;
                        defCOption.HelpText = "The region identifying the clinic it is assigned to.";
                        break;
                    case DefinitionCategory.SupplyCats:
                        defCOption.CanDelete = true;
                        defCOption.CanHide = false;
                        defCOption.HelpText = "The categories for inventory supplies.";
                        break;
                    case DefinitionCategory.TaskPriorities:
                        defCOption.EnableColor = true;
                        defCOption.EnableValue = true;
                        defCOption.ValueText = "D = Default, R = Reminder";
                        defCOption.HelpText = "Priorities available for selection within the task edit window.  Task lists are sorted using the order of these priorities.  They can have any description and color.  At least one priority should be Default (D).  If more than one priority is flagged as the default, the last default in the list will be used.  If no default is set, the last priority will be used.  Use (R) to indicate the initial reminder task priority to use when creating reminder tasks.  Changes affect all tasks where the definition is used.";
                        break;
                    case DefinitionCategory.TxPriorities:
                        defCOption.EnableColor = true;
                        defCOption.EnableValue = true;
                        defCOption.DoShowItemOrderInValue = true;
                        defCOption.ValueText = "Internal Priority";
                        defCOption.HelpText = "Displayed order should match order of priority of treatment.  They are used in Treatment Plan and Chart "
                            + "modules. They can be simple numbers or descriptive abbreviations 7 letters or less.  Changes affect all procedures where the "
                            + "definition is used.  'Internal Priority' does not show, but is used for list order and for automated selection of which procedures "
                            + "are next in a planned appointment.";
                        break;
                    case DefinitionCategory.WebSchedNewPatApptTypes:
                        defCOption.CanDelete = true;
                        defCOption.CanHide = false;
                        defCOption.ValueText = "Appointment Type";
                        defCOption.HelpText = "Appointment types to be displayed in the Web Sched New Pat Appt web application.  These are selectable for the new patients and will be saved to the appointment note.";
                        break;
                    case DefinitionCategory.CarrierGroupNames:
                        defCOption.CanHide = true;
                        defCOption.HelpText = "These are group names for Carriers.";
                        break;
                }
                listDefCatOptions.Add(defCOption);
            }
            return listDefCatOptions;
        }

        private static string GetItemDescForImages(string itemValue)
        {
            var values = new List<string>();
            if (itemValue.Contains("X")) values.Add("ChartModule");
            if (itemValue.Contains("F")) values.Add("PatientForm");
            if (itemValue.Contains("P")) values.Add("PatientPic");
            if (itemValue.Contains("S")) values.Add("Statement");
            if (itemValue.Contains("T")) values.Add("ToothChart");
            if (itemValue.Contains("R")) values.Add("TreatPlans");
            if (itemValue.Contains("L")) values.Add("PatientPortal");
            if (itemValue.Contains("A")) values.Add("PayPlans");

            return string.Join(", ", values);
        }
        #endregion

        /// <summary>
        ///     <para>
        ///         Fills the specified <paramref name="grid"/> with specified list of 
        ///         <paramref name="definitions"/>.
        ///     </para>
        /// </summary>
        /// <param name="grid">The grid to populate.</param>
        public static void FillGridDefs(ODGrid grid, DefCatOptions defCatOptions, List<Definition> definitions)
        {
            Definition selectedDefinition = null;
            if (grid.GetSelectedIndex() > -1)
            {
                selectedDefinition = (Definition)grid.Rows[grid.GetSelectedIndex()].Tag;
            }

            int scroll = grid.ScrollValue;

            grid.BeginUpdate();
            grid.Columns.Clear();
            grid.Columns.Add(new ODGridColumn("Name", 190));
            grid.Columns.Add(new ODGridColumn(defCatOptions.ValueText, 190));
            grid.Columns.Add(new ODGridColumn(defCatOptions.EnableColor ? "Color" : "", 40));
            grid.Columns.Add(new ODGridColumn(defCatOptions.CanHide ? "Hide" : "", 30, HorizontalAlignment.Center));
            grid.Rows.Clear();

            ODGridRow row;
            foreach (var definition in definitions)
            {
                if (Defs.IsDefDeprecated(definition))
                {
                    definition.Hidden = true;
                }

                row = new ODGridRow();
                row.Cells.Add(definition.Description);

                if (defCatOptions.DefCat == DefinitionCategory.ImageCats)
                {
                    row.Cells.Add(GetItemDescForImages(definition.Value));
                }
                else if (defCatOptions.DefCat == DefinitionCategory.AutoNoteCats)
                {
                    var dictAutoNoteDefs = new Dictionary<string, string>();
                    dictAutoNoteDefs = definitions.ToDictionary(x => x.Id.ToString(), x => x.Description);
                    string nameCur;
                    row.Cells.Add(dictAutoNoteDefs.TryGetValue(definition.Value, out nameCur) ? nameCur : definition.Value);
                }
                else if (defCatOptions.DefCat == DefinitionCategory.WebSchedNewPatApptTypes)
                {
                    AppointmentType appointmentType = AppointmentType.GetWebSchedNewPatApptTypeByDef(definition.Id);
                    row.Cells.Add(appointmentType == null ? "" : appointmentType.Name);
                }
                else if (defCatOptions.DoShowItemOrderInValue)
                {
                    row.Cells.Add(definition.SortOrder.ToString());
                }
                else
                {
                    row.Cells.Add(definition.Value);
                }
                row.Cells.Add("");
                if (defCatOptions.EnableColor)
                {
                    row.Cells[row.Cells.Count - 1].CellColor = definition.Color;
                }
                row.Cells.Add(definition.Hidden ? "X" : "");
                row.Tag = definition;

                grid.Rows.Add(row);
            }
            grid.EndUpdate();

            if (selectedDefinition != null)
            {
                for (int i = 0; i < grid.Rows.Count; i++)
                {
                    if (((Definition)grid.Rows[i].Tag).Id == selectedDefinition.Id)
                    {
                        grid.SetSelected(i, true);
                        break;
                    }
                }
            }

            grid.ScrollValue = scroll;
        }

        public static bool GridDefsDoubleClick(Definition definition, ODGrid gridDefs, DefCatOptions selectedDefCatOpt, List<Definition> listDefsCur, List<Definition> listDefsAll, bool isDefChanged)
        {
            switch (selectedDefCatOpt.DefCat)
            {
                case DefinitionCategory.BlockoutTypes:
                    using (var formDefEditBlockout = new FormDefEditBlockout(definition))
                    {
                        if (formDefEditBlockout.ShowDialog() == DialogResult.OK)
                        {
                            isDefChanged = true;
                        }
                    }
                    break;

                case DefinitionCategory.ImageCats:
                    using (var formDefEditImages = new FormDefEditImages(definition))
                    {
                        formDefEditImages.IsNew = false;
                        if (formDefEditImages.ShowDialog() == DialogResult.OK)
                        {
                            isDefChanged = true;
                        }
                    }
                    break;

                case DefinitionCategory.WebSchedNewPatApptTypes:
                    using (var formDefEditWSNPApptTypes = new FormDefEditWSNPApptTypes(definition))
                    {
                        if (formDefEditWSNPApptTypes.ShowDialog() == DialogResult.OK)
                        {
                            if (formDefEditWSNPApptTypes.IsDeleted)
                            {
                                listDefsAll.Remove(definition);
                            }
                            isDefChanged = true;
                        }
                    }
                    break;

                default:
                    using (var formDefEdit = new FormDefEdit(definition, listDefsCur, selectedDefCatOpt))
                    {
                        formDefEdit.IsNew = false;
                        if (formDefEdit.ShowDialog() == DialogResult.OK)
                        {
                            if (formDefEdit.IsDeleted)
                            {
                                listDefsAll.Remove(definition);
                            }
                            isDefChanged = true;
                        }
                    }
                    break;
            }

            return isDefChanged;
        }

        public static bool AddDef(ODGrid gridDefs, DefCatOptions selectedDefCatOpt)
        {
            int itemOrder = 0;
            if (Definition.GetByCategory(selectedDefCatOpt.DefCat).Count > 0)
            {
                itemOrder = Definition.GetByCategory(selectedDefCatOpt.DefCat).Max(x => x.SortOrder) + 1;
            }

            var definition = new Definition
            {
                SortOrder = itemOrder,
                Category = selectedDefCatOpt.DefCat,
                Description = "",
                Value = ""
            };

            if (selectedDefCatOpt.DefCat == DefinitionCategory.InsurancePaymentType)
            {
                definition.Value = "N";
            }

            switch (selectedDefCatOpt.DefCat)
            {
                case DefinitionCategory.BlockoutTypes:
                    using (var formDefEditBlockout = new FormDefEditBlockout(definition))
                    {
                        formDefEditBlockout.IsNew = true;
                        if (formDefEditBlockout.ShowDialog() != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                    break;

                case DefinitionCategory.ImageCats:
                    using (var formDefEditImages = new FormDefEditImages(definition))
                    {
                        formDefEditImages.IsNew = true;
                        if (formDefEditImages.ShowDialog() != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                    break;

                case DefinitionCategory.WebSchedNewPatApptTypes:
                    using (var formDefEditWSNPApptTypes = new FormDefEditWSNPApptTypes(definition))
                    {
                        if (formDefEditWSNPApptTypes.ShowDialog() != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                    break;

                default:
                    var currentDefinitions = new List<Definition>();
                    foreach (ODGridRow row in gridDefs.Rows)
                    {
                        currentDefinitions.Add((Definition)row.Tag);
                    }

                    using (var formDefEdit = new FormDefEdit(definition, currentDefinitions, selectedDefCatOpt))
                    {
                        formDefEdit.IsNew = true;
                        if (formDefEdit.ShowDialog() != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                    break;
            }

            return true;
        }

        public static bool HideDef(ODGrid gridDefs, DefCatOptions selectedDefCatOpt)
        {
            if (gridDefs.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                    "Please select item first.",
                    "Definition",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return false;
            }

            var definition = (Definition)gridDefs.Rows[gridDefs.GetSelectedIndex()].Tag;

            // Warn the user if they are about to hide a billing type currently in use.
            if (selectedDefCatOpt.DefCat == DefinitionCategory.BillingTypes && Patients.IsBillingTypeInUse(definition.Id))
            {
                var result =
                    MessageBox.Show(
                        "Warning: Billing type is currently in use by patients, insurance plans, or preferences.",
                        "Definition",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question);

                if (result == DialogResult.Cancel)
                {
                    return false;
                }
            }

            if (definition.Category == DefinitionCategory.ProviderSpecialties && (Providers.IsSpecialtyInUse(definition.Id) || Referrals.IsSpecialtyInUse(definition.Id)))
            {
                MessageBox.Show(
                    "You cannot hide a specialty if it is in use by a provider or a referral source.", 
                    "Definition", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return false;
            }

            if (Defs.IsDefinitionInUse(definition))
            {
                if (definition.Id == Preference.GetLong(PreferenceName.BrokenAppointmentAdjustmentType) || 
                    definition.Id == Preference.GetLong(PreferenceName.AppointmentTimeArrivedTrigger) || 
                    definition.Id == Preference.GetLong(PreferenceName.AppointmentTimeSeatedTrigger) || 
                    definition.Id == Preference.GetLong(PreferenceName.AppointmentTimeDismissedTrigger) || 
                    definition.Id == Preference.GetLong(PreferenceName.TreatPlanDiscountAdjustmentType) || 
                    definition.Id == Preference.GetLong(PreferenceName.BillingChargeAdjustmentType) || 
                    definition.Id == Preference.GetLong(PreferenceName.PracticeDefaultBillType) || 
                    definition.Id == Preference.GetLong(PreferenceName.FinanceChargeAdjustmentType) || 
                    definition.Id == Preference.GetLong(PreferenceName.RecurringChargesPayTypeCC))
                {
                    MessageBox.Show("You cannot hide a definition if it is in use within Module Preferences.");
                    return false;
                }
                else
                {
                    var result =
                        MessageBox.Show(
                            "Warning: This definition is currently in use within the program.",
                            "Definition",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Question);

                    if (result == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }

            // Stop users from hiding the last definition in categories that must have at least one def in them.
            if (Defs.IsHidable(definition.Category))
            {
                var visibleDefinitions = Definition.GetByCategory(selectedDefCatOpt.DefCat);
                if (visibleDefinitions.Count == 1)
                {
                    MessageBox.Show(
                        "You cannot hide the last definition in this category.", 
                        "Definition", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);

                    return false;
                }
            }

            definition.Hidden = true;

            Definition.Update(definition);

            return true;
        }

        public static bool UpClick(ODGrid gridDefs)
        {
            if (gridDefs.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                    "Please select item first.",
                    "Definition",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

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
                MessageBox.Show(
                    "Please select item first.",
                    "Definition",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return false;
            }

            if (gridDefs.GetSelectedIndex() == gridDefs.Rows.Count - 1) return false;

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