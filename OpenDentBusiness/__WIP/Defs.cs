using CodeBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OpenDentBusiness
{
    public class Defs
    {
        #region Get Methods


        public static Definition[][] GetArrayShortNoCache()
        {
            Definition[][] arrayShort = new Definition[Enum.GetValues(typeof(DefinitionCategory)).Length][];

            for (int j = 0; j < Enum.GetValues(typeof(DefinitionCategory)).Length; j++)
            {
                arrayShort[j] = Definition.GetByCategory((DefinitionCategory)j).ToArray();
            }

            return arrayShort;
        }

        ///<summary>Gets a list of defs from the list of defnums and passed-in cat.</summary>
        public static List<Definition> GetDefs(DefinitionCategory Cat, List<long> listDefNums)
        {
            return Definition.GetByCategory(Cat).FindAll(x => listDefNums.Contains(x.Id));
        }

        ///<summary>Get one def from Long.  Returns null if not found.  Only used for very limited situations.
        ///Other Get functions tend to be much more useful since they don't return null.
        ///There is also BIG potential for silent bugs if you use this.ItemOrder instead of GetOrder().</summary>
        public static Definition GetDef(DefinitionCategory category, long definitionId, List<Definition> definitionList = null) =>
            (definitionList ?? Definition.GetByCategory(category)).FirstOrDefault(d => d.Id == definitionId);

        ///<summary>Returns 0 if it can't find the named def.  If the name is blank, then it returns the first def in the category.</summary>
        public static long GetByExactName(DefinitionCategory myCat, string itemName)
        {
            //No need to check RemotingRole; no call to db.
            List<Definition> listDefs = Definition.GetByCategory(myCat);
            //jsalmon - The following line doesn't make much sense because the def list could be empty but this is preserving old behavior...
            if (itemName == "")
            {
                return listDefs[0].Id;//return the first one in the list
            }
            Definition def = listDefs.FirstOrDefault(x => x.Description == itemName);
            return (def == null ? 0 : def.Id);
        }

        ///<summary>Returns the named def.  If it can't find the name, then it returns the first def in the category.</summary>
        public static long GetByExactNameNeverZero(DefinitionCategory myCat, string itemName)
        {
            //No need to check RemotingRole; no call to db.
            List<Definition> listDefs = Definition.GetByCategory(myCat);
            Definition def;
            //We have been getting bug submissions from customers where listDefs will be null (e.g. DefCat.ProviderSpecialties cat itemName "General")
            //Therefore, we should check for null or and entirely empty category first before looking for a match.
            if (listDefs == null || listDefs.Count == 0)
            {
                //There are no defs for the category passed in, create one because this method should never return zero.
                def = new Definition();
                def.Category = myCat;
                def.SortOrder = 0;
                def.Description = itemName;
                Definition.Insert(def);
                CacheManager.Invalidate<Definition>();
                return def.Id;
            }
            //From this point on, we know our list of definitions contains at least one def.
            def = listDefs.FirstOrDefault(x => x.Description == itemName);
            if (def != null)
            {
                return def.Id;
            }
            //Couldn't find a match so return the first definition from our list as a last resort.
            return listDefs[0].Id;
        }

        ///<summary>Returns defs from the AdjTypes that contain '+' in the ItemValue column.</summary>
        public static List<Definition> GetPositiveAdjTypes()
        {
            return Definition.GetByCategory(DefinitionCategory.AdjTypes).FindAll(x => x.Value == "+");
        }

        ///<summary>Returns defs from the AdjTypes that contain '-' in the ItemValue column.</summary>
        public static List<Definition> GetNegativeAdjTypes()
        {
            return Definition.GetByCategory(DefinitionCategory.AdjTypes).FindAll(x => x.Value == "-");
        }

        ///<summary>Returns defs from the AdjTypes that contain 'dp' in the ItemValue column.</summary>
        public static List<Definition> GetDiscountPlanAdjTypes()
        {
            return Definition.GetByCategory(DefinitionCategory.AdjTypes).FindAll(x => x.Value == "dp");
        }

        ///<summary>Returns a DefNum for the special image category specified.  Returns 0 if no match found.</summary>
        public static long GetImageCat(ImageCategorySpecial specialCat)
        {
            //No need to check RemotingRole; no call to db.
            Definition def = Definition.GetByCategory(DefinitionCategory.ImageCats).FirstOrDefault(x => x.Value.Contains(specialCat.ToString()));
            return (def == null ? 0 : def.Id);
        }

        ///<summary>Gets the order of the def within Short or -1 if not found.</summary>
        public static int GetOrder(DefinitionCategory myCat, long myDefNum)
        {
            //No need to check RemotingRole; no call to db.
            //gets the index in the list of unhidden (the Short list).
            return Definition.GetByCategory(myCat).FindIndex(x => x.Id == myDefNum);
        }



        public static string GetValue(DefinitionCategory category, long definitionId, string defaultValue = "")
        {
            var definition = Definition.GetByCategory(category, definitionId);

            if (definition != null)
            {
                return definition.Value;
            }

            return defaultValue;
        }


        public static Color GetColor(DefinitionCategory category, long definitionId, Color defaultColor)
        {
            var definition = Definition.GetByCategory(category, definitionId);

            if (definition != null)
            {
                return definition.Color;
            }

            return defaultColor;
        }

        public static Color GetColor(DefinitionCategory category, long definitionId) =>
            GetColor(category, definitionId, Color.Black);

        ///<summary>Returns Color.White if no match found. Pass in a list of defs to save from making deep copies of the cache if you are going to 
        ///call this method repeatedly.</summary>
        //public static Color GetColor(DefinitionCategory myCat, long myDefNum, List<Definition> listDefs = null)
        //{
        //    //No need to check RemotingRole; no call to db.
        //    listDefs = listDefs ?? Defs.GetDefsForCategory(myCat);
        //    Definition def = listDefs.LastOrDefault(x => x.Id == myDefNum);
        //    return (def == null ? Color.White : def.Color.Value);
        //}

        public static bool IsHidden(long definitionId)
        {
            var definition = Definition.GetById(definitionId);

            return definition == null ? false : definition.Hidden;
        }

        ///<summary>Pass in a list of all defs to save from making deep copies of the cache if you are going to call this method repeatedly.</summary>
        public static string GetName(DefinitionCategory myCat, long myDefNum, List<Definition> listDefs = null)
        {
            //No need to check RemotingRole; no call to db.
            if (myDefNum == 0)
            {
                return "";
            }
            Definition def = GetDef(myCat, myDefNum, listDefs);
            return (def == null ? "" : def.Description);
        }

        ///<summary>Throws an exception if there are no definitions in the category provided.  This is to preserve old behavior.</summary>
        public static Definition GetFirstForCategory(DefinitionCategory defCat, bool isShort = false)
        {
            //No need to check RemotingRole; no call to db.
            List<Definition> listDefs = Definition.GetByCategory(defCat);
            return listDefs.First();
        }
        #endregion

        #region Modification Methods

        /// <summary>CAUTION.  This does not perform all validations.  Throws exceptions.</summary>
        public static void Delete(Definition def)
        {
            string command;

            var commandList = new List<string>();
            switch (def.Category)
            {
                case DefinitionCategory.ClaimCustomTracking:
                    commandList.Add("SELECT COUNT(*) FROM securitylog WHERE DefNum=" + def.Id);
                    commandList.Add("SELECT COUNT(*) FROM claim WHERE CustomTracking=" + def.Id);
                    break;

                case DefinitionCategory.ClaimErrorCode:
                    commandList.Add("SELECT COUNT(*) FROM claimtracking WHERE TrackingErrorDefNum=" + def.Id);
                    break;

                case DefinitionCategory.InsurancePaymentType:
                    commandList.Add("SELECT COUNT(*) FROM claimpayment WHERE PayType=" + def.Id);
                    break;

                case DefinitionCategory.SupplyCats:
                    commandList.Add("SELECT COUNT(*) FROM supply WHERE Category=" + def.Id);
                    break;

                case DefinitionCategory.AccountQuickCharge:
                    break;//Users can delete AcctProcQuickCharge entries.  Nothing has an FKey to a AcctProcQuickCharge Def so no need to check anything.

                case DefinitionCategory.AutoNoteCats:
                    AutoNotes.RemoveFromCategory(def.Id);//set any autonotes assinged to this category to 0 (unassigned), user already warned about this
                    commandList.Add("SELECT COUNT(*) FROM autonote WHERE Category=" + def.Id);//just in case update failed or concurrency issue
                    break;

                case DefinitionCategory.WebSchedNewPatApptTypes:
                    //Do not let the user delete the last WebSchedNewPatApptTypes definition.  Must be at least one.
                    command = "SELECT COUNT(*) FROM definition WHERE Category=" + (int)DefinitionCategory.WebSchedNewPatApptTypes;
                    if (DataConnection.ExecuteLong(command) <= 1)
                    {
                        throw new ApplicationException("NOT Allowed to delete the last def of this type.");
                    }
                    break;

                default:
                    throw new ApplicationException("NOT Allowed to delete this type of def.");
            }

            for (int i = 0; i < commandList.Count; i++)
            {
                if (DataConnection.ExecuteLong(commandList[i]) > 0)
                {
                    throw new ApplicationException(Lans.g("Defs", "Def is in use.  Not allowed to delete."));
                }
            }

            DataConnection.ExecuteNonQuery("DELETE FROM definition WHERE DefNum=" + def.Id);
            DataConnection.ExecuteNonQuery("UPDATE definition SET ItemOrder=ItemOrder-1 WHERE Category=" + (int)def.Category + " AND ItemOrder > " + def.SortOrder);
        }

        #endregion

        #region Misc Methods

        ///<summary>Returns true if the passed-in def is deprecated.  This method must be updated whenever another def is deprecated.</summary>
        public static bool IsDefDeprecated(Definition def)
        {
            if (def.Category == DefinitionCategory.AccountColors && def.Description == "Received Pre-Auth")
            {
                return true;
            }
            return false;
        }

        ///<summary>Returns true if this category has definitions that can be hidden and we want to disallow hiding all definitions.</summary>
        public static bool IsHidable(DefinitionCategory category)
        {
            if (category == DefinitionCategory.AdjTypes
                || category == DefinitionCategory.ApptConfirmed
                || category == DefinitionCategory.ApptProcsQuickAdd
                || category == DefinitionCategory.BillingTypes
                || category == DefinitionCategory.BlockoutTypes
                || category == DefinitionCategory.ClaimPaymentGroups
                || category == DefinitionCategory.ClaimPaymentTracking
                //|| category==DefCat.ClinicSpecialty //ClinicSpecialties can be hidden and we want to allow users to hide ALL specialties if they so desire.
                || category == DefinitionCategory.CommLogTypes
                || category == DefinitionCategory.ContactCategories
                || category == DefinitionCategory.Diagnosis
                || category == DefinitionCategory.ImageCats
                || category == DefinitionCategory.InsuranceFilingCodeGroup
                || category == DefinitionCategory.LetterMergeCats
                || category == DefinitionCategory.PaymentTypes
                || category == DefinitionCategory.PaySplitUnearnedType
                || category == DefinitionCategory.ProcButtonCats
                || category == DefinitionCategory.ProcCodeCats
                || category == DefinitionCategory.Prognosis
                || category == DefinitionCategory.ProviderSpecialties
                || category == DefinitionCategory.RecallUnschedStatus
                || category == DefinitionCategory.TaskPriorities
                || category == DefinitionCategory.TxPriorities)
            {
                return true;
            }
            return false;
        }

        ///<summary>Returns true if there are any entries in definition that do not have a Category named "General".  
        ///Returning false means the user has ProcButtonCategory customizations.</summary>
        public static bool HasCustomCategories()
        {
            List<Definition> listDefs = Definition.GetByCategory(DefinitionCategory.ProcButtonCats);
            foreach (Definition defCur in listDefs)
            {
                if (!defCur.Description.Equals("General"))
                {
                    return true;
                }
            }
            return false;
        }

        ///<summary>Returns true if this definition is in use within the program. Consider enhancing this method if you add a definition category.
        ///Does not check patient billing type or provider specialty since those are handled in their S-class.</summary>
        public static bool IsDefinitionInUse(Definition definition)
        {
            var commandList = new List<string>();
            switch (definition.Category)
            {
                case DefinitionCategory.AdjTypes:
                    if (new[] {
                            PreferenceName.BrokenAppointmentAdjustmentType,
                            PreferenceName.TreatPlanDiscountAdjustmentType,
                            PreferenceName.BillingChargeAdjustmentType,
                            PreferenceName.FinanceChargeAdjustmentType,
                            PreferenceName.SalesTaxAdjustmentType
                        }.Any(x => Preference.GetLong(x) == definition.Id))
                    {
                        return true;
                    }
                    commandList.Add("SELECT COUNT(*) FROM adjustment WHERE AdjType=" + definition.Id);
                    break;

                case DefinitionCategory.ApptConfirmed:
                    if (new[] {
                            PreferenceName.AppointmentTimeArrivedTrigger,
                            PreferenceName.AppointmentTimeSeatedTrigger,
                            PreferenceName.AppointmentTimeDismissedTrigger,
                            PreferenceName.WebSchedNewPatConfirmStatus,
                            PreferenceName.WebSchedRecallConfirmStatus,
                        }.Any(x => Preference.GetLong(x) == definition.Id))
                    {
                        return true;
                    }

                    if (new[] {
                            PreferenceName.ApptEConfirmStatusSent,
                            PreferenceName.ApptEConfirmStatusAccepted,
                            PreferenceName.ApptEConfirmStatusDeclined,
                            PreferenceName.ApptEConfirmStatusSendFailed
                        }.Any(x => Preference.GetLong(x) == definition.Id))
                    {
                        return true;
                    }

                    commandList.Add("SELECT COUNT(*) FROM appointment WHERE Confirmed=" + definition.Id);
                    break;

                case DefinitionCategory.AutoNoteCats:
                    commandList.Add("SELECT COUNT(*) FROM autonote WHERE Category=" + definition.Id);
                    break;

                case DefinitionCategory.BillingTypes:
                    if (new[] {
                            PreferenceName.PracticeDefaultBillType
                        }.Any(x => Preference.GetLong(x) == definition.Id))
                    {
                        return true;
                    }
                    break;

                case DefinitionCategory.ContactCategories:
                    commandList.Add("SELECT COUNT(*) FROM contact WHERE Category=" + definition.Id);
                    break;

                case DefinitionCategory.Diagnosis:
                    commandList.Add("SELECT COUNT(*) FROM procedurelog WHERE Dx=" + definition.Id);
                    break;

                case DefinitionCategory.ImageCats:
                    commandList.Add("SELECT COUNT(*) FROM document WHERE DocCategory=" + definition.Id);
                    commandList.Add("SELECT COUNT(*) FROM sheetfielddef WHERE FieldType=" + (int)SheetFieldType.PatImage + " AND FieldName=" + definition.Id);
                    break;

                case DefinitionCategory.PaymentTypes:
                    if (Preference.GetLong(PreferenceName.RecurringChargesPayTypeCC) == definition.Id)
                    {
                        return true;
                    }
                    commandList.Add("SELECT COUNT(*) FROM payment WHERE PayType=" + definition.Id);
                    break;

                case DefinitionCategory.PaySplitUnearnedType:
                    commandList.Add("SELECT COUNT(*) FROM paysplit WHERE UnearnedType=" + definition.Id);
                    break;

                case DefinitionCategory.Prognosis:
                    commandList.Add("SELECT COUNT(*) FROM procedurelog WHERE Prognosis=" + definition.Id);
                    break;

                case DefinitionCategory.RecallUnschedStatus:
                    if (definition.Id.In(
                        Preference.GetInt(PreferenceName.RecallStatusMailed),
                        Preference.GetInt(PreferenceName.RecallStatusTexted),
                        Preference.GetInt(PreferenceName.RecallStatusEmailed),
                        Preference.GetInt(PreferenceName.RecallStatusEmailedTexted)))
                    {
                        return true;
                    }
                    commandList.Add("SELECT COUNT(*) FROM appointment WHERE UnschedStatus=" + definition.Id);
                    commandList.Add("SELECT COUNT(*) FROM recall WHERE RecallStatus=" + definition.Id);
                    break;
                case DefinitionCategory.TaskPriorities:
                    commandList.Add("SELECT COUNT(*) FROM task WHERE PriorityDefNum=" + definition.Id);
                    break;

                case DefinitionCategory.TxPriorities:
                    commandList.Add("SELECT COUNT(*) FROM procedurelog WHERE Priority=" + definition.Id);
                    break;

                case DefinitionCategory.CommLogTypes:
                    commandList.Add("SELECT COUNT(*) FROM commlog WHERE CommType=" + definition.Id);
                    break;

                default:
                    break;
            }
            return commandList.Any(x => DataConnection.ExecuteLong(x) > 0);
        }

        #endregion
    }

    ///<summary></summary>
    public enum ImageCategorySpecial
    {
        ///<summary>Show in Chart module.</summary>
        X,
        ///<summary>Show in patient forms.</summary>
        F,
        /// <summary>Show in patient portal.</summary>
        L,
        ///<summary>Patient picture (only one)</summary>
        P,
        ///<summary>Statements (only one)</summary>
        S,
        ///<summary>Graphical tooth charts and perio charts (only one)</summary>
        T,
        /// <summary>Treatment plan (only one)</summary>
        R,
        /// <summary>Expanded by default.</summary>
        E
    }


}
