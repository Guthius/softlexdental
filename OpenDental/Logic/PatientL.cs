using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public class PatientL
    {
        ///<summary>Collection of Patient Names. The last five patients. Gets displayed on dropdown button.</summary>
        private static List<string> buttonLastFiveNames;
        ///<summary>Collection of PatNums. The last five patients. Used when clicking on dropdown button.</summary>
        private static List<long> buttonLastFivePatNums;
        ///<summary>Static variable to store the currently selected patient for updating the main title bar for the Update Time countdown.
        ///Stored so we don't have to make a database call every second.</summary>
        private static Patient _patSelectedCur;
        ///<summary>Static variable to store the currently selected clinicNum for updating the main title bar for the Update Time countdown.
        ///Stored so we don't have to make a database call every second.</summary>
        private static long _clinSelectedCur;

        ///<summary>Removes a patient from the dropdown menu.  Only used when Delete Patient is called.</summary>
        public static void RemoveFromMenu(string nameLF, long patNum)
        {
            //No need to check RemotingRole; no call to db.
            buttonLastFivePatNums.Remove(patNum);
            buttonLastFiveNames.Remove(nameLF);
        }

        ///<summary>Removes all patients from the dropdown menu and from the history.  Only used when a user logs off and a different user logs on.  Important for enterprise customers with clinic restrictions for users.</summary>
        public static void RemoveAllFromMenu(ContextMenu menu)
        {
            menu.MenuItems.Clear();
            buttonLastFivePatNums.Clear();
            buttonLastFiveNames.Clear();
        }

        ///<summary>The current patient will already be on the button.  This adds the family members when user clicks dropdown arrow. Can handle null values for pat and fam.  Need to supply the menu to fill as well as the EventHandler to set for each item (all the same).</summary>
        public static void AddFamilyToMenu(ContextMenu menu, EventHandler onClick, long patNum, Family fam)
        {
            //No need to check RemotingRole; no call to db.
            //fill menu
            menu.MenuItems.Clear();
            if (buttonLastFiveNames.Count == 0 && patNum == 0)
            {
                return;//Without this the Select Patient dropdown would only have a bar and FAMILY.
            }
            for (int i = 0; i < buttonLastFiveNames.Count; i++)
            {
                menu.MenuItems.Add(buttonLastFiveNames[i].ToString(), onClick);
            }
            menu.MenuItems.Add("-");
            menu.MenuItems.Add("FAMILY");
            if (patNum != 0 && fam != null)
            {
                for (int i = 0; i < fam.Members.Length; i++)
                {
                    menu.MenuItems.Add(fam.Members[i].GetNameLF(), onClick);
                }
            }
        }

        ///<summary>Does not handle null values. Use zero.  Does not handle adding family members.  Returns true if patient has changed.</summary>
        public static bool AddPatsToMenu(ContextMenu menu, EventHandler onClick, string nameLF, long patNum)
        {
            //No need to check RemotingRole; no call to db.
            //add current patient
            if (buttonLastFivePatNums == null)
            {
                buttonLastFivePatNums = new List<long>();
            }
            if (buttonLastFiveNames == null)
            {
                buttonLastFiveNames = new List<string>();
            }
            if (patNum == 0)
            {
                return false;
            }
            if (buttonLastFivePatNums.Count > 0 && patNum == buttonLastFivePatNums[0])
            {//same patient selected
                return false;
            }
            //Patient has changed
            int idx = buttonLastFivePatNums.IndexOf(patNum);
            if (idx > -1)
            {//It exists in this list of patnums
                buttonLastFivePatNums.RemoveAt(idx);
                buttonLastFiveNames.RemoveAt(idx);
            }
            buttonLastFivePatNums.Insert(0, patNum);
            buttonLastFiveNames.Insert(0, nameLF);
            if (buttonLastFivePatNums.Count > 5)
            {
                buttonLastFivePatNums.RemoveAt(5);
                buttonLastFiveNames.RemoveAt(5);
            }
            return true;
        }

        ///<summary>Determines which menu Item was selected from the Patient dropdown list and returns the patNum for that patient. This will not be activated when click on 'FAMILY' or on separator, because they do not have events attached.  Calling class then does a ModuleSelected.</summary>
        public static long ButtonSelect(ContextMenu menu, object sender, Family fam)
        {
            //No need to check RemotingRole; no call to db.
            int index = menu.MenuItems.IndexOf((MenuItem)sender);
            //Patients.PatIsLoaded=true;
            if (index < buttonLastFivePatNums.Count)
            {
                return (long)buttonLastFivePatNums[index];
            }
            if (fam == null)
            {
                return 0;//will never happen
            }
            return fam.Members[index - buttonLastFivePatNums.Count - 2].PatNum;
        }

        /// <summary>
        ///     <para>
        ///         Returns a string representation of the current state of the application 
        ///         designed for display in the main title. Accepts null for <paramref name="patient"/> and 0 for clinicNum.
        ///     </para>
        /// </summary>
        public static string GetMainTitle(Patient patient, long? clinicId)
        {
            string title = Preference.GetString(PreferenceName.MainWindowTitle);

            title = Plugin.Filter(null, "Patient_FilterMainTitle", title);

            // Figure out if the patient passed in is different than the currently selected patient.
            bool hasPatChanged =
                (_patSelectedCur == null && patient != null) ||
                (_patSelectedCur != null && patient == null) ||
                (_patSelectedCur != null && patient != null && _patSelectedCur.PatNum != patient.PatNum);

            _patSelectedCur = patient;
            _clinSelectedCur = clinicId.Value;

            if (clinicId.HasValue)
            {
                if (title != "")
                {
                    title += " - Clinic: ";
                }

                title +=
                    Preference.GetBool(PreferenceName.TitleBarClinicUseAbbr) ?
                        Clinic.GetById(clinicId.Value).Abbr :
                        Clinic.GetById(clinicId.Value).Description;
            }

            if (Security.CurrentUser != null)
            {
                title += " {" + Security.CurrentUser.UserName + "}";
            }

            if (patient == null || patient.PatNum == 0 || patient.PatNum == -1)
            {
                if (FormOpenDental.RegKeyIsForTesting)
                {
                    title += " - Developer Only License - Not for use with live patient data - ";
                }

                return title;
            }

            title += " - " + patient.GetNameLF();
            //A query is required to get the Specialty for the selected patient so only run this code if the patient has changed.
            if (Preference.GetBool(PreferenceName.TitleBarShowSpecialty) && hasPatChanged)
            {
                string specialty = Patients.GetPatientSpecialtyDef(patient.PatNum)?.Description ?? "";
                title += string.IsNullOrWhiteSpace(specialty) ? "" : " (" + specialty + ")";
            }
            if (Preference.GetLong(PreferenceName.ShowIDinTitleBar) == 1)
            {
                title += " - " + patient.PatNum.ToString();
            }
            else if (Preference.GetLong(PreferenceName.ShowIDinTitleBar) == 2)
            {
                title += " - " + patient.ChartNumber;
            }
            else if (Preference.GetLong(PreferenceName.ShowIDinTitleBar) == 3)
            {
                if (patient.Birthdate.Year > 1880)
                {
                    title += " - " + patient.Birthdate.ToShortDateString();
                }
            }
            if (patient.SiteNum != 0)
            {
                title += " - " + Sites.GetDescription(patient.SiteNum);
            }

            if (FormOpenDental.RegKeyIsForTesting)
            {
                title += " - " + "Developer Only License - Not for use with live patient data - ";
            }

            return title;
        }

        ///<summary>Used to update the main title bar when neither the patient nor the clinic need to change. 
        ///Currently only used to refresh the title bar when the timer ticks for the update time countdown.</summary>
        public static string GetMainTitleSamePat()
        {
            string title = Preference.GetString(PreferenceName.MainWindowTitle);
            if (_clinSelectedCur > 0)
            {
                if (title != "")
                {
                    title += " - Clinic: ";
                }

                title +=
                    Preference.GetBool(PreferenceName.TitleBarClinicUseAbbr) ?
                        Clinic.GetById(_clinSelectedCur).Abbr :
                        Clinic.GetById(_clinSelectedCur).Description;
            }

            if (Security.CurrentUser != null)
            {
                title += " {" + Security.CurrentUser.UserName + "}";
            }

            if (_patSelectedCur == null || _patSelectedCur.PatNum == 0 || _patSelectedCur.PatNum == -1)
            {
                if (FormOpenDental.RegKeyIsForTesting)
                {
                    title += " - Developer Only License - Not for use with live patient data - ";
                }
                //Now check to see if this database has been put into "Testing Mode"
                if (Introspection.IsTestingMode)
                {
                    title += " <TESTING MODE ENABLED> ";
                }
                return title;
            }

            title += " - " + _patSelectedCur.GetNameLF();
            if (Preference.GetBool(PreferenceName.TitleBarShowSpecialty))
            {
                string specialty = Patients.GetPatientSpecialtyDef(_patSelectedCur.PatNum)?.Description ?? "";
                title += string.IsNullOrWhiteSpace(specialty) ? "" : " (" + specialty + ")";
            }

            if (Preference.GetLong(PreferenceName.ShowIDinTitleBar) == 1)
            {
                title += " - " + _patSelectedCur.PatNum.ToString();
            }
            else if (Preference.GetLong(PreferenceName.ShowIDinTitleBar) == 2)
            {
                title += " - " + _patSelectedCur.ChartNumber;
            }
            else if (Preference.GetLong(PreferenceName.ShowIDinTitleBar) == 3)
            {
                if (_patSelectedCur.Birthdate.Year > 1880)
                {
                    title += " - " + _patSelectedCur.Birthdate.ToShortDateString();
                }
            }

            if (_patSelectedCur.SiteNum != 0)
            {
                title += " - " + Sites.GetDescription(_patSelectedCur.SiteNum);
            }

            if (FormOpenDental.RegKeyIsForTesting)
            {
                title += " - Developer Only License - Not for use with live patient data - ";
            }

            //Now check to see if this database has been put into "Testing Mode"
            if (Introspection.IsTestingMode)
            {
                title += " <TESTING MODE ENABLED> ";
            }

            return title;
        }
    }
}