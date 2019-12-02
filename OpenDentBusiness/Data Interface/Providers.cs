using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OpenDentBusiness
{
    public class Providers
    {
        /// <summary>
        /// Checks to see if the providers passed in have term dates that occur after the date passed in.
        /// Returns a list of the ProvNums that have invalid term dates.  Otherwise; empty list.
        /// </summary>
        public static List<long> GetInvalidProvsByTermDate(List<long> providerIds, DateTime dateCompare)
        {
            return Provider.All()
                .Where(provider => 
                    providerIds.Any(y => y == provider.Id) && 
                    provider.DateTermEnd.HasValue && 
                    provider.DateTermEnd.Value.Date < dateCompare.Date)
                .Select(provider => 
                    provider.Id)
                .ToList();
        }

        /// <summary>
        /// Checks the appointment's provider and hygienist's term dates to see if an appointment should be scheduled or marked complete.
        /// Returns an empty string if the appointment does not violate the Term Date for the provider or hygienist.
        /// A non-empty return value should be displayed to the user in a message box (already translated).
        /// isSetComplete simply modifies the message. Use this when checking if an appointment should be set complete.
        /// </summary>
        public static string CheckApptProvidersTermDates(Appointment appointment, bool isSetComplete = false)
        {
            string message = "";

            var providerIds = new List<long> { appointment.ProvNum, appointment.ProvHyg };
            var invalidProviderIds = GetInvalidProvsByTermDate(providerIds, appointment.AptDateTime);

            if (invalidProviderIds.Count == 0) return message;

            if (invalidProviderIds.Contains(appointment.ProvNum))
            {
                message += "provider";
            }

            if (invalidProviderIds.Contains(appointment.ProvHyg))
            {
                if (message != "")
                {
                    message += " and ";
                }

                message += "hygienist";
            }

            if (invalidProviderIds.Contains(appointment.ProvNum) && invalidProviderIds.Contains(appointment.ProvHyg))
            {
                message = 
                    "The " + message + " selected for this appointment have Term Dates prior to the selected day and time. " + 
                    "Please select another " + message + (isSetComplete ? " to set the appointment complete." : ".");
            }
            else
            {
                message = 
                    "The " + message + " selected for this appointment has a Term Date prior to the selected day and time. " + 
                    "Please select another " + message + (isSetComplete ? " to set the appointment complete." : ".");
            }

            return message;
        }

        /// <summary>
        /// This checks for the maximum number of provnum in the database and then returns the one directly after.
        /// Not guaranteed to be a unique primary key.
        /// </summary>
        public static long GetNextAvailableProvNum()
        {
            return DataConnection.ExecuteLong("SELECT MAX(provNum) FROM provider") + 1;
        }

        /// <summary>
        /// Gets table for the FormProviderSetup window. Always orders by ItemOrder.
        /// </summary>
        public static DataTable RefreshStandard()
        {
            string command = "SELECT Abbr,LName,FName,provider.IsHidden,provider.ItemOrder,provider.ProvNum,MAX(UserName) UserName,"//Max function used for Oracle compatability (some providers may have multiple user names).
                + "PatCountPri,PatCountSec,ProvStatus,IsHiddenReport "
                + "FROM provider "
                + "LEFT JOIN userod ON userod.ProvNum=provider.ProvNum "//there can be multiple userods attached to one provider
                + "LEFT JOIN (SELECT PriProv, COUNT(*) PatCountPri FROM patient "
                    + "WHERE patient.PatStatus!=" + (int)PatientStatus.Deleted + " AND patient.PatStatus!=" +  (int)PatientStatus.Deceased + " "
                    + "GROUP BY PriProv) patPri ON provider.ProvNum=patPri.PriProv  ";
            command += "LEFT JOIN (SELECT SecProv,COUNT(*) PatCountSec FROM patient "
                + "WHERE patient.PatStatus!=" + (int)PatientStatus.Deleted + " AND patient.PatStatus!=" + (int)PatientStatus.Deceased + " "
                + "GROUP BY SecProv) patSec ON provider.ProvNum=patSec.SecProv ";
            command += "GROUP BY Abbr,LName,FName,provider.IsHidden,provider.ItemOrder,provider.ProvNum,PatCountPri,PatCountSec,ProvStatus,IsHiddenReport ";
            command += "ORDER BY ItemOrder";
            return DataConnection.ExecuteDataTable(command);
        }

        public static string GetAbbr(long provNum) => Provider.GetById(provNum)?.Abbr ?? "";

        /// <summary>
        /// Gets a list of providers from ListLong.
        /// If none found or if either LName or FName are an empty string, returns an empty list.
        /// There may be more than on provider with the same FName and LName so we will return a list of all such providers.
        /// Usually only one will exist with the FName and LName provided so list returned will have count 0 or 1 normally.
        /// Name match is not case sensitive.
        /// </summary>
        public static List<Provider> GetProvsByFLName(string lastName, string firstName)
        {
            if (string.IsNullOrWhiteSpace(lastName) || 
                string.IsNullOrWhiteSpace(firstName))
            {
                return new List<Provider>();
            }

            return Provider.All().Where(x => x.LastName.ToLower() == lastName.ToLower() && x.FirstName.ToLower() == firstName.ToLower()).ToList();
        }

        /// <summary>
        /// Gets a list of providers from ListLong with either the NPI provided or a blank NPI and the Medicaid ID provided. medicaidId can be blank.
        /// If the npi param is blank, or there are no matching provs, returns an empty list.
        /// Shouldn't be two separate functions or we would have to compare the results of the two lists.
        /// </summary>
        public static IEnumerable<Provider> GetProvsByNpiOrMedicaidId(string nationalProviderId, string medicaidId)
        {
            if (string.IsNullOrEmpty(nationalProviderId)) yield break;

            foreach (var provider in Provider.All())
            {
                //if the prov has a NPI set and it's a match, add this prov to the list
                if (provider.NationalProviderId != "")
                {
                    if (provider.NationalProviderId.Trim().ToLower() == nationalProviderId.Trim().ToLower())
                    {
                        yield return provider;
                    }
                }
                else
                {//if the NPI is blank and the Medicaid ID is set and it's a match, add this prov to the list
                    if (provider.MedicaidID != "" && provider.MedicaidID.Trim().ToLower() == medicaidId.Trim().ToLower())
                    {
                        yield return provider;
                    }
                }
            }
        }

        public static List<User> GetAttachedUsers(long provNum) => User.GetByProvider(provNum);

        /// <summary>
        /// If useClinic, then clinicInsBillingProv will be used. 
        /// Otherwise, the pref for the practice. 
        /// Either way, there are three different choices for getting the billing provider. 
        /// One of the three is to use the treating provider, so supply that as an argument. 
        /// It will return a valid provNum unless the supplied treatProv was invalid.
        /// </summary>
        public static long GetBillingProviderId(long treatingProviderId, long? clinicId)
        {
            long providerId = treatingProviderId;

            if (clinicId.HasValue)
            {
                var clinic = Clinic.GetById(clinicId.Value);
                if (clinic != null && clinic.InsuranceBillingProviderId.HasValue)
                {
                    providerId = clinic.InsuranceBillingProviderId.Value;
                }
            }

            return providerId;
        }

        ///<summary>Returns list of providers that are either not restricted to a clinic, or are restricted to the ClinicNum provided. 
        ///Passing ClinicNum=0 returns all unrestricted providers. Ordered by provider.Abbr.</summary>
        public static List<Provider> GetProvsForClinic(long clinicNum)
        {
            Dictionary<long, List<long>> dictUserClinics = User.All()
                .ToDictionary(x => x.Id, x => ClinicUser.GetForUser(x.Id).Select(y => y.ClinicId).ToList());
            Dictionary<long, List<long>> dictProvUsers = User.AllProviders().GroupBy(x => x.ProviderId.Value)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Id).ToList());
            HashSet<long> hashSetProvsRestrictedOtherClinic = new HashSet<long>(ProviderClinic.GetProvsRestrictedToOtherClinics(clinicNum).Select(x => x.ProviderId));
            return Provider.All().Where(x =>
                (!dictProvUsers.ContainsKey(x.Id) //provider not associated to any users.
                || dictProvUsers[x.Id].Any(y => dictUserClinics[y].Count == 0) //provider associated with user not restricted to any clinics
                || dictProvUsers[x.Id].Any(y => dictUserClinics[y].Contains(clinicNum))) //provider associated to user restricted to clinic at hand
                && !hashSetProvsRestrictedOtherClinic.Contains(x.Id)).ToList();
        }

        ///<Summary>Used once in the Provider Select window to warn user of duplicate Abbrs.</Summary>
        public static string GetDuplicateAbbrs()
        {
            string command = "SELECT Abbr FROM provider WHERE ProvStatus!=" + (int)ProviderStatus.Deleted;
            List<string> listDuplicates = Db.GetListString(command).GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
            return string.Join(",", listDuplicates);//returns empty string when listDuplicates is empty
        }

        public static DataTable GetDefaultPracticeProvider()
        {
            return DataConnection.ExecuteDataTable(
                "SELECT FName,LName,Suffix,StateLicense FROM provider WHERE provnum=" + Preference.GetLong(PreferenceName.PracticeDefaultProv));
        }

        ///<summary>We should merge these results with GetDefaultPracticeProvider(), but
        ///that would require restructuring indexes in different places in the code and this is
        ///faster to do as we are just moving the queries down in to the business layer for now.</summary>
        public static DataTable GetDefaultPracticeProvider2()
        {
            return DataConnection.ExecuteDataTable(
                "SELECT FName,LName,Specialty FROM provider WHERE provnum=" + Preference.GetLong(PreferenceName.PracticeDefaultProv));
        }

        ///<summary>We should merge these results with GetDefaultPracticeProvider(), but
        ///that would require restructuring indexes in different places in the code and this is
        ///faster to do as we are just moving the queries down in to the business layer for now.</summary>
        public static DataTable GetDefaultPracticeProvider3()
        {
            return DataConnection.ExecuteDataTable(
                "SELECT NationalProvID FROM provider WHERE provnum=" + Preference.GetLong(PreferenceName.PracticeDefaultProv));
        }

        public static DataTable GetPrimaryProviders(long PatNum)
        {
            string command = @"SELECT Fname,Lname from provider
                        WHERE provnum in (select priprov from 
                        patient where patnum = " + PatNum + ")";
            return DataConnection.ExecuteDataTable(command);
        }

        ///<summary>Returns the patient's last seen hygienist.  Returns null if no hygienist has been seen.</summary>
        public static Provider GetLastSeenHygienistForPat(long patNum)
        {
            //Look at all completed appointments and get the most recent secondary provider on it.
            string command = @"SELECT appointment.ProvHyg
				FROM appointment
				WHERE appointment.PatNum=" + patNum + @"
				AND appointment.ProvHyg!=0
				AND appointment.AptStatus=" + (int)ApptStatus.Complete + " " +
                "ORDER BY AptDateTime DESC";
            List<long> listPatHygNums = Db.GetListLong(command);
            //Now that we have all hygienists for this patient.  Lets find the last non-hidden hygienist and return that one.
            var listProviders = Provider.All();
            List<long> listProvNums = listProviders.Select(x => x.Id).Distinct().ToList();
            long lastHygNum = listPatHygNums.FirstOrDefault(x => listProvNums.Contains(x));
            return listProviders.FirstOrDefault(x => x.Id == lastHygNum);
        }

        /// <summary>
        /// Currently only used for Dental Schools and will only return Providers.ListShort if Dental Schools is not active. 
        /// Otherwise this will return a filtered provider list.
        /// </summary>
        public static IEnumerable<Provider> GetFilteredProviderList(long? providerId, string lastName, string firstName)
        {
            lastName = lastName.Trim();
            firstName = firstName.Trim();

            foreach (var provider in Provider.All())
            {
                if (providerId != null && provider.Id != providerId) 
                    continue;

                if (lastName.Length > 0 && !provider.LastName.Contains(lastName))
                    continue;

                if (firstName.Length > 0 && !provider.FirstName.Contains(firstName))
                    continue;

                yield return provider;
            }
        }

        /// <summary>
        /// Returns a dictionary, with the key being ProvNum and the value being the production goal amount.
        /// </summary>
        public static Dictionary<long, decimal> GetProductionGoalForProviders(List<long> providerIds, List<long> operatoryIds, DateTime start, DateTime end)
        {
            var scheduledProviders = Schedules.GetHoursSchedForProvsInRange(providerIds, operatoryIds, start, end);

            var productionGoals = new Dictionary<long, decimal>();

            foreach (var kvp in scheduledProviders)
            {
                var provider = Provider.GetById(kvp.Key);

                if (provider != null)
                {
                    productionGoals[provider.Id] = (decimal)(kvp.Value * provider.HourlyProducationGoal);
                }
            }

            return productionGoals;
        }

        /// <summary>
        /// Removes a provider from the future schedule. Currently called after a provider is hidden.
        /// </summary>
        public static void RemoveProvFromFutureSchedule(long providerId)
        {
            if (providerId < 1) return;

            RemoveProvsFromFutureSchedule(
                new List<long>
                {
                    providerId
                });
        }

        /// <summary>
        /// Removes the providers from the future schedule.  Currently called from DBM to clean up hidden providers still on the schedule.
        /// </summary>
        public static void RemoveProvsFromFutureSchedule(List<long> providerIds)
        {
            if (providerIds==null || providerIds.Count == 0)
            {
                return;
            }

            var provs = string.Join(", ", providerIds);

            DataTable table = 
                DataConnection.ExecuteDataTable(
                    "SELECT ScheduleNum FROM schedule WHERE ProvNum IN (" + provs + ") AND SchedDate > " + DbHelper.Now());

            List<string> listScheduleNums = new List<string>();//Used for deleting scheduleops below
            for (int i = 0; i < table.Rows.Count; i++)
            {
                //Add entry to deletedobjects table if it is a provider schedule type
                DeletedObjects.SetDeleted(DeletedObjectType.ScheduleProv, PIn.Long(table.Rows[i]["ScheduleNum"].ToString()));
                listScheduleNums.Add(table.Rows[i]["ScheduleNum"].ToString());
            }

            if (listScheduleNums.Count != 0)
            {
                DataConnection.ExecuteNonQuery(
                    "DELETE FROM scheduleop WHERE ScheduleNum IN(" + string.Join(",", listScheduleNums) + ")");
            }

            DataConnection.ExecuteNonQuery(
                "DELETE FROM schedule WHERE ProvNum IN (" + provs + ") AND SchedDate > " + DbHelper.Now());
        }

        /// <summary>
        /// Used to check if a specialty is in use when user is trying to hide it.
        /// </summary>
        public static bool IsSpecialtyInUse(long definitionId)
        {
            return DataConnection.ExecuteLong("SELECT COUNT(*) FROM `providers` WHERE `specialty_id` = " + definitionId) > 0;
        }

        ///<summary>Used to get a list of providers that are scheduled for today.  
        ///Pass in specific clinicNum for providers scheduled in specific clinic, clinicNum of -1 for all clinics</summary>
        public static List<Provider> GetProvsScheduledToday(long clinicId = -1)
        {
            var schedulesForToday = Schedules.GetAllForDateAndType(DateTime.Today, ScheduleType.Provider);

            if (clinicId >= 0)
            {
                schedulesForToday.FindAll(x => x.ClinicNum == clinicId);
            }

            var providerIds = schedulesForToday.Select(x => x.ProvNum).ToList();

            return Provider.GetByIds(providerIds).ToList();
        }

        ///<summary>Provider merge tool.  Returns the number of rows changed when the tool is used.</summary>
        public static long Merge(long provNumFrom, long provNumInto)
        {
            string[] provNumForeignKeys = new string[] { //add any new FKs to this list.
				"adjustment.ProvNum",
                "anestheticrecord.ProvNum",
                "appointment.ProvNum",
                "appointment.ProvHyg",
                "apptviewitem.ProvNum",
                "claim.ProvTreat",
                "claim.ProvBill",
                "claim.ReferringProv",
                "claim.ProvOrderOverride",
                "claimproc.ProvNum",
                "clinic.DefaultProv",
                "clinic.InsBillingProv",
                "dispsupply.ProvNum",
                "ehrnotperformed.ProvNum",
                "emailmessage.ProvNumWebMail",
                "encounter.ProvNum",
                "equipment.ProvNumCheckedOut",
                "erxlog.ProvNum",
                "evaluation.InstructNum",
                "evaluation.StudentNum",
                "fee.ProvNum",
                "intervention.ProvNum",
        "labcase.ProvNum",
                "medicalorder.ProvNum",
                "medicationpat.ProvNum",
                "operatory.ProvDentist",
                "operatory.ProvHygienist",
                "patient.PriProv",
                "patient.SecProv",
                "payplancharge.ProvNum",
        "paysplit.ProvNum",
                "perioexam.ProvNum",
                "proccodenote.ProvNum",
                "procedurecode.ProvNumDefault",
        "procedurelog.ProvNum",
                "procedurelog.ProvOrderOverride",
                "provider.ProvNumBillingOverride",
                "providerclinic.ProvNum",
                "providerident.ProvNum",
                "refattach.ProvNum",
                "reqstudent.ProvNum",
                "reqstudent.InstructorNum",
                "rxpat.ProvNum",
                "schedule.ProvNum",
                "userod.ProvNum",
                "vaccinepat.ProvNumAdminister",
                "vaccinepat.ProvNumOrdering"
            };
            string command = "";
            long retVal = 0;
            for (int i = 0; i < provNumForeignKeys.Length; i++)
            { //actually change all of the FKs in the above tables.
                string[] tableAndKeyName = provNumForeignKeys[i].Split(new char[] { '.' });
                command = "UPDATE " + tableAndKeyName[0]
                    + " SET " + tableAndKeyName[1] + "=" + POut.Long(provNumInto)
                    + " WHERE " + tableAndKeyName[1] + "=" + POut.Long(provNumFrom);
                retVal += Db.NonQ(command);
            }
            command = "UPDATE provider SET IsHidden=1 WHERE ProvNum=" + POut.Long(provNumFrom);
            Db.NonQ(command);
            command = "UPDATE provider SET ProvStatus=" + POut.Int((int)ProviderStatus.Deleted) + " WHERE ProvNum=" + POut.Long(provNumFrom);
            Db.NonQ(command);
            return retVal;
        }


    }
}