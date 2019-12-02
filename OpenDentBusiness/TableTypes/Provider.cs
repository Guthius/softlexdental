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
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OpenDentBusiness
{
    /// <summary>
    /// A provider is usually a dentist or a hygienist. 
    /// But a provider might also be a denturist, a dental student, or a dental hygiene student. 
    /// A provider might also be a 'dummy', used only for billing purposes or for notes in the Appointments module. 
    /// There is no limit to the number of providers that can be added.
    /// </summary>
    public class Provider : DataRecord
    {
        private static readonly DataRecordCache<Provider> cache = 
            new DataRecordCache<Provider>("SELECT * FROM `providers`", FromReader);

        /// <summary>
        /// A abbreviation of the provider name.
        /// </summary>
        public string Abbr;

        /// <summary>
        /// The last name of the provider.
        /// </summary>
        public string LastName;

        /// <summary>
        /// The first name of the provider.
        /// </summary>
        public string FirstName;

        /// <summary>
        /// The middle initial (or name) of the provider.
        /// </summary>
        public string MiddleInitial;

        /// <summary>
        /// The suffix / title of the provider. eg. DMD or DDS.
        /// </summary>
        public string Suffix;

        public long FeeScheduleId;

        /// <summary>
        /// The ID of the definition that represents the specialty of the provider.
        /// </summary>
        public long? SpecialtyId;

        /// <summary>
        ///     <para>
        ///         A value indicating whether <see cref="SSN"/> is holding a TIN 
        ///         (Tax Identification Number) instead.
        ///     </para>
        /// </summary>
        public bool UsingTIN;

        /// <summary>
        /// The SSN (or TIN) or the provider (without punctuation).
        /// </summary>
        public string SSN;

        ///<summary>True if hygienist.</summary>
        public bool IsSecondary;

        /// <summary>
        /// The color used to represent the provider in the scheduling module.
        /// </summary>
        public Color Color = Color.White;

        /// <summary>
        /// A value indicating whether the provider has been hidden.
        /// </summary>
        public bool IsHidden;

        /// <summary>
        /// Gets a value indicating there is a signature on file for the provider.
        /// </summary>
        public bool SignatureOnFile;

        /// <summary>
        /// Gets the Medicaid ID of he provider.
        /// </summary>
        public string MedicaidID;

        /// <summary>
        /// The outline color of highlighted appointments scheduled for this provider.
        /// </summary>
        public Color OutlineColor;

        /// <summary>
        /// The US NPI or Canadian CDA provider number.
        /// </summary>
        public string NationalProviderId;

        /// <summary>
        /// Canadian field required for e-claims.  
        /// Assigned by CDA.  
        /// It's OK to have multiple providers with the same OfficeNum. 
        /// Max length should be 4.
        /// </summary>
        public string CanadianOfficeNumber;

        /// <summary>
        /// FK to ??. Field used to set the Anesthesia Provider type. Used to filter the provider dropdowns on FormAnestheticRecord
        /// </summary>
        public long AnesthesiaProviderType;

        /// <summary>
        /// If none of the supplied taxonomies works. This will show on claims.
        /// </summary>
        public string TaxonomyCodeOverride;

        /// <summary>
        ///     <para>
        ///         A value indicating whether the provider is a CDA Net provider.
        ///     </para>
        ///     <para>For Canada.</para>
        /// </summary>
        public bool IsCDAnet;

        /// <summary>
        /// The name of this field is bad and will soon be changed to MedicalSoftID. 
        /// This allows an ID field that can be used for HL7 synch with other software.  
        /// Before this field was added, we were using prov abbreviation, which did not work well.
        /// </summary>
        public string EcwId;

        /// <summary>
        ///     <para>
        ///         A value indicating whether the provider is not a person.
        ///     </para>
        ///     <para>
        ///         Most providers are persons but some dummy providers may be used to represent 
        ///         practices or billing entities, which are not persons. This is needed on 837's.
        ///     </para>
        /// </summary>
        public bool IsNotPerson;

        /// <summary>
        /// Not currently used. Optional, can be null.
        /// </summary>
        public long? EmailAddressId;

        /// <summary>
        ///     <para>
        ///         Value used to determine which stage of MU the provider is shown.
        ///     </para>
        ///     <list type="table">
        ///         <item>
        ///             <term>0</term>
        ///             <description>Global preference (default)</description>
        ///         </item>
        ///         <item>
        ///             <term>1</term>
        ///             <description>Stage 1</description>
        ///         </item>
        ///         <item>
        ///             <term>2</term>
        ///             <description>Stage 2</description>
        ///         </item>
        ///         <item>
        ///             <term>3</term>
        ///             <description>Modified Stage 2</description>
        ///         </item>
        ///     </list>
        /// </summary>
        public int EhrMuStage;

        /// <summary>
        /// The ID of the provider to use when billing for this provider.
        /// </summary>
        public long? BillingOverrideProviderId;

        /// <summary>
        /// Custom ID used for reports or bridges only.
        /// </summary>
        public string CustomID;

        /// <summary>
        /// The status of the provider.
        /// </summary>
        public ProviderStatus Status;

        /// <summary>
        /// A value indicating whether the user is hidden from standard reports.
        /// </summary>
        public bool IsHiddenReport;

        /// <summary>
        ///     <para>
        ///         Value indicating whether or not the provider has individually agreed to 
        ///         accept eRx charges.
        ///     </para>
        ///     <para>
        ///         Defaults to disabled for new providers.
        ///     </para>
        /// </summary>
        public ProviderErxStatus IsErxEnabled = ProviderErxStatus.Disabled;

        /// <summary>
        /// A note indicating if the provider should only be scheduled in a certain way (e.g. Root canals only).
        /// </summary>
        public string SchedulingNote;

        /// <summary>
        /// The birth date of the provider.
        /// </summary>
        public DateTime BirthDate;

        /// <summary>
        /// The hourly production goal amount of the provider.
        /// </summary>
        public double HourlyProducationGoal;

        /// <summary>
        ///     <para>
        ///         The date on which the provider's term ends. 
        ///     </para>
        ///     <para>
        ///         This can be used to prevent appointments from being scheduled, appointments 
        ///         from being marked complete, prescriptions from being prescribed, and claims 
        ///         from being sent.
        ///     </para>
        /// </summary>
        public DateTime? DateTermEnd;

        /// <summary>
        /// The date on which the provider was last modified.
        /// </summary>
        public DateTime DateModified;

        /// <summary>
        /// Returns a string representation of the provider.
        /// </summary>
        public override string ToString() => Abbr ?? "";

        /// <summary>
        /// Constructs a new instance of the <see cref="Provider"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Provider"/> instance.</returns>
        public static Provider FromReader(MySqlDataReader dataReader)
        {
            return new Provider
            {
                Id = (long)dataReader["id"],
                Abbr = (string)dataReader["abbr"],
                FirstName = (string)dataReader["first_name"],
                LastName = (string)dataReader["last_name"],
                MiddleInitial = (string)dataReader["initials"],
                Suffix = (string)dataReader["suffix"],
                FeeScheduleId = (long)dataReader["fee_schedule_id"],
                SpecialtyId = dataReader["specialty_id"] as long?,
                UsingTIN = (bool)dataReader["using_tin"],
                SSN = (string)dataReader["ssn"],
                IsSecondary = (bool)dataReader["is_secondary"],
                Color = ColorTranslator.FromHtml((string)dataReader["color"]),
                IsHidden = (bool)dataReader["hidden"],
                SignatureOnFile = (bool)dataReader["has_signature"],
                MedicaidID = (string)dataReader["medicaid_id"],
                OutlineColor = ColorTranslator.FromHtml((string)dataReader["outline_color"]),
                NationalProviderId = (string)dataReader["national_provider_id"],
                CanadianOfficeNumber = (string)dataReader["canadian_office_number"],
                AnesthesiaProviderType = (long)dataReader["anesthesia_provider_type"],
                TaxonomyCodeOverride = (string)dataReader["taxonomy_code"],
                IsCDAnet = (bool)dataReader["is_cda_net"],
                EcwId = (string)dataReader["ecw_id"],
                IsNotPerson = (bool)dataReader["is_not_person"],
                EmailAddressId = dataReader["email_address_id"] as long?,
                EhrMuStage = (int)dataReader["ehr_mu_stage"],
                BillingOverrideProviderId = dataReader["billing_provider_id"] as long?,
                CustomID = (string)dataReader["custom_id"],
                Status = (ProviderStatus)(int)dataReader["status"],
                IsHiddenReport = (bool)dataReader["is_hidden_report"],
                IsErxEnabled = (ProviderErxStatus)(int)dataReader["erx_status"],
                SchedulingNote = (string)dataReader["scheduling_note"],
                BirthDate = (DateTime)dataReader["birth_date"],
                HourlyProducationGoal = (double)dataReader["hourly_production_goal"],
                DateTermEnd = dataReader["date_term_end"] as DateTime?,
                DateModified = (DateTime)dataReader["date_modified"]
            };
        }

        /// <summary>
        /// Gets all providers.
        /// </summary>
        /// <returns>All providers.</returns>
        public static IEnumerable<Provider> All(bool includeHidden = false) =>
            includeHidden ? cache.All() : cache.SelectMany(x => !x.IsHidden);

        /// <summary>
        ///     <para>
        ///         Gets all providers that have been modified after the specified 
        ///         <paramref name="date"/>.
        ///     </para>
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>All providers modified after the specified date.</returns>
        public static IEnumerable<Provider> GetModifiedAfter(DateTime date) =>
            cache.SelectMany(x => x.DateModified > date);

        /// <summary>
        /// Gets the provider with the specified ID.
        /// </summary>
        /// <param name="providerId">The ID of the provider.</param>
        /// <returns>The provider.</returns>
        public static Provider GetById(long providerId) =>
            cache.SelectOne(x => x.Id == providerId);

        /// <summary>
        /// Gets all providers with the specified ID's.
        /// </summary>
        /// <param name="providerIds">The ID's of the providers.</param>
        /// <returns>The providers.</returns>
        public static IEnumerable<Provider> GetByIds(List<long> providerIds) =>
            cache.SelectMany(x => providerIds.Contains(x.Id));
        
        /// <summary>
        ///     <para>
        ///         Gets all providers that are associated with the specified clinic.
        ///     </para>
        ///     <para>
        ///         There is no direct connection between providers and clinics. Providers are 
        ///         linked to clinics through users.
        ///     </para>
        /// </summary>
        public static IEnumerable<Provider> GetByClinic(long clinicId)
        {
            foreach (var userClinic in ClinicUser.GetForClinic(clinicId))
            {
                var user = User.GetById(userClinic.UserId);
                if (user == null || !user.ProviderId.HasValue)
                {
                    continue;
                }

                var provider = GetById(user.ProviderId.Value);
                if (provider == null)
                {
                    continue;
                }

                yield return provider;
            }
        }

        /// <summary>
        /// Gets the provider with the specified Ecw ID.
        /// </summary>
        public static Provider GetByEcwId(string ecwId) => 
            cache.SelectOne(x => x.EcwId == ecwId);

        /// <summary>
        ///     <para>
        ///         Gets the default provider for the specified clinic.
        ///     </para>
        /// </summary>
        /// <param name="clinicId">The ID of the clinic.</param>
        /// <returns>The default provider.</returns>
        public static Provider GetDefault(long? clinicId = null)
        {
            Provider provider = null;

            if (clinicId.HasValue)
            {
                var clinic = Clinic.GetById(clinicId.Value);

                if (clinic != null && clinic.ProviderId.HasValue)
                {
                    provider = GetById(clinic.ProviderId.Value);
                }
            }

            if (provider == null)
            {
                provider = GetById(
                    Preference.GetLong(
                        PreferenceName.PracticeDefaultProv));
            }

            return provider;
        }

        /// <summary>
        /// Inserts the specified <paramref name="provider"/> into the database.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns>The ID assigned to the provider.</returns>
        public static long Insert(Provider provider) =>
            provider.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `providers` (`abbr`, `first_name`, `last_name`, `initials`, `suffix`, `fee_schedule_id`, `specialty_id`, `using_tin`," +
                "`ssn`, `is_secondary`, `color`, `hidden`, `has_signature`, `medicaid_id`, `outline_color`, `national_provider_id`, " +
                "`canadian_office_number`, `anesthesia_provider_type`, `taxonomy_code`, `is_cda_net`, `ecw_id`, `is_not_person`, " +
                "`email_address_id`, `ehr_mu_stage`, `billing_provider_id`, `custom_id`, `status`, `is_hidden_report`, `erx_status`, " +
                "`scheduling_note`, `birth_date`, `hourly_production_goal`, `date_term_end`, `date_modified`) VALUES (?abbr, ?first_name, " +
                "?last_name, ?initials, ?suffix, ?fee_schedule_id, ?specialty_id, ?using_tin, ?ssn, ?is_secondary, ?color, " +
                "?hidden, ?has_signature, ?medicaid_id, ?outline_color, ?national_provider_id, ?canadian_office_number, " +
                "?anesthesia_provider_type, ?taxonomy_code, ?is_cda_net, ?ecw_id, ?is_not_person, ?email_address_id, " +
                "?ehr_mu_stage, ?billing_provider_id, ?custom_id, ?status, ?is_hidden_report, ?erx_status, ?scheduling_note, " +
                "?birth_date, ?hourly_production_goal, ?date_term_end, ?date_modified)",
                    new MySqlParameter("abbr", provider.Abbr ?? ""),
                    new MySqlParameter("first_name", provider.FirstName ?? ""),
                    new MySqlParameter("last_name", provider.LastName ?? ""),
                    new MySqlParameter("initials", provider.MiddleInitial ?? ""),
                    new MySqlParameter("suffix", provider.Suffix ?? ""),
                    new MySqlParameter("fee_schedule_id", provider.Abbr),
                    new MySqlParameter("specialty_id", ValueOrDbNull(provider.SpecialtyId)),
                    new MySqlParameter("using_tin", provider.UsingTIN),
                    new MySqlParameter("ssn", provider.SSN ?? ""),
                    new MySqlParameter("is_secondary", provider.IsSecondary),
                    new MySqlParameter("color", ColorTranslator.ToHtml(provider.Color)),
                    new MySqlParameter("hidden", provider.IsHidden),
                    new MySqlParameter("has_signature", provider.SignatureOnFile),
                    new MySqlParameter("medicaid_id", provider.MedicaidID ?? ""),
                    new MySqlParameter("outline_color", ColorTranslator.ToHtml(provider.OutlineColor)),
                    new MySqlParameter("national_provider_id", provider.NationalProviderId ?? ""),
                    new MySqlParameter("canadian_office_number", provider.CanadianOfficeNumber ?? ""),
                    new MySqlParameter("anesthesia_provider_type", provider.AnesthesiaProviderType),
                    new MySqlParameter("taxonomy_code", provider.TaxonomyCodeOverride ?? ""),
                    new MySqlParameter("is_cda_net", provider.IsCDAnet),
                    new MySqlParameter("ecw_id", provider.EcwId ?? ""),
                    new MySqlParameter("is_not_person", provider.IsNotPerson),
                    new MySqlParameter("email_address_id", ValueOrDbNull(provider.EmailAddressId)),
                    new MySqlParameter("ehr_mu_stage", provider.EhrMuStage),
                    new MySqlParameter("billing_provider_id", provider.Abbr),
                    new MySqlParameter("custom_id", provider.CustomID ?? ""),
                    new MySqlParameter("status", (int)provider.Status),
                    new MySqlParameter("is_hidden_report", provider.IsHiddenReport),
                    new MySqlParameter("erx_status", (int)provider.IsErxEnabled),
                    new MySqlParameter("scheduling_note", provider.SchedulingNote ?? ""),
                    new MySqlParameter("birth_date", provider.BirthDate),
                    new MySqlParameter("hourly_production_goal", provider.HourlyProducationGoal),
                    new MySqlParameter("date_term_end", ValueOrDbNull(provider.DateTermEnd)),
                    new MySqlParameter("date_modified", provider.DateModified));

        /// <summary>
        /// Updates the specified <paramref name="provider"/> in the database.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public static void Update(Provider provider) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `providers` SET `abbr` = ?abbr, `first_name` = ?first_name, `last_name` = ?last_name, `initials` = ?initials, " +
                "`suffix` = ?suffix, `fee_schedule_id` = ?fee_schedule_id, `specialty_id` = ?specialty_id, `using_tin` = ?using_tin, " +
                "`ssn` = ?ssn, `is_secondary` = ?is_secondary, `color` = ?color, `hidden` = ?hidden, `has_signature` = ?has_signature, " +
                "`medicaid_id` = ?medicaid_id, `outline_color` = ?outline_color, `national_provider_id` = ?national_provider_id, " +
                "`canadian_office_number` = ?canadian_office_number, `taxonomy_code` = ?taxonomy_code, `is_cda_net` = ?is_cda_net, " +
                "`ecw_id` = ?ecw_id, `is_not_person` = ?is_not_person, `email_address_id` = ?email_address_id, `ehr_mu_state` = ?ehr_mu_stage, " +
                "`billing_provider_id` = ?billing_provider_id, `custom_id` = ?custom_id, `status` = ?status, " +
                "`is_hidden_report` = ?is_hidden_report, `erx_status` = ?erx_status, `scheduling_note` = ?scheduling_note, " +
                "`birth_date` = ?birth_date, `hourly_production_goal` = ?hourly_production_goal, `date_term_end` = ?date_term_end, " +
                "`date_modified` = ?date_modified WHERE `id` = ?id",
                    new MySqlParameter("abbr", provider.Abbr ?? ""),
                    new MySqlParameter("first_name", provider.FirstName ?? ""),
                    new MySqlParameter("last_name", provider.LastName ?? ""),
                    new MySqlParameter("initials", provider.MiddleInitial ?? ""),
                    new MySqlParameter("suffix", provider.Suffix ?? ""),
                    new MySqlParameter("fee_schedule_id", provider.Abbr),
                    new MySqlParameter("specialty_id", ValueOrDbNull(provider.SpecialtyId)),
                    new MySqlParameter("using_tin", provider.UsingTIN),
                    new MySqlParameter("ssn", provider.SSN ?? ""),
                    new MySqlParameter("is_secondary", provider.IsSecondary),
                    new MySqlParameter("color", ColorTranslator.ToHtml(provider.Color)),
                    new MySqlParameter("hidden", provider.IsHidden),
                    new MySqlParameter("has_signature", provider.SignatureOnFile),
                    new MySqlParameter("medicaid_id", provider.MedicaidID ?? ""),
                    new MySqlParameter("outline_color", ColorTranslator.ToHtml(provider.OutlineColor)),
                    new MySqlParameter("national_provider_id", provider.NationalProviderId ?? ""),
                    new MySqlParameter("canadian_office_number", provider.CanadianOfficeNumber ?? ""),
                    new MySqlParameter("anesthesia_provider_type", provider.AnesthesiaProviderType),
                    new MySqlParameter("taxonomy_code", provider.TaxonomyCodeOverride ?? ""),
                    new MySqlParameter("is_cda_net", provider.IsCDAnet),
                    new MySqlParameter("ecw_id", provider.EcwId ?? ""),
                    new MySqlParameter("is_not_person", provider.IsNotPerson),
                    new MySqlParameter("email_address_id", ValueOrDbNull(provider.EmailAddressId)),
                    new MySqlParameter("ehr_mu_stage", provider.EhrMuStage),
                    new MySqlParameter("billing_provider_id", provider.Abbr),
                    new MySqlParameter("custom_id", provider.CustomID ?? ""),
                    new MySqlParameter("status", (int)provider.Status),
                    new MySqlParameter("is_hidden_report", provider.IsHiddenReport),
                    new MySqlParameter("erx_status", (int)provider.IsErxEnabled),
                    new MySqlParameter("scheduling_note", provider.SchedulingNote ?? ""),
                    new MySqlParameter("birth_date", provider.BirthDate),
                    new MySqlParameter("hourly_production_goal", provider.HourlyProducationGoal),
                    new MySqlParameter("date_term_end", ValueOrDbNull(provider.DateTermEnd)),
                    new MySqlParameter("date_modified", provider.DateModified = DateTime.Now),
                    new MySqlParameter("id", provider.Id));

        /// <summary>
        /// Deletes the provider with the specified ID from the database.
        /// </summary>
        /// <param name="providerId">The ID of the provider.</param>
        public static void Delete(long providerId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `providers` WHERE `id` = " + providerId);

        /// <summary>
        /// For use in areas of the program where we have more room than just simple abbr.
        /// Such as pick boxes in reports. 
        /// This will give Abbr - LName, FName (hidden).
        /// </summary>
        public string GetLongDesc()
        {
            string result = Abbr + "- " + LastName + ", " + FirstName;

            if (IsHidden)
            {
                result += " (hidden)";
            }

            return result;
        }

        /// <summary>
        /// For use in areas of the program where we have only have room for the simple abbr. 
        /// Such as pick boxes in the claim edit window. 
        /// This will give Abbr (hidden).
        /// </summary>
        public string GetAbbr()
        {
            string result = Abbr;

            if (IsHidden)
            {
                result += " (hidden)";
            }

            return result;
        }

        public string GetFormalName()
        {
            string result = FirstName;

            if (!string.IsNullOrEmpty(MiddleInitial))
            {
                result += " " + MiddleInitial;
                if (MiddleInitial.Length == 1)
                {
                    result += ".";
                }
            }

            result +=  " " + LastName;

            if (!string.IsNullOrEmpty(Suffix))
            {
                result += ", " + Suffix;
            }

            return result;
        }


        public static long CountPatients(long providerId)
        {
            // usp_provider_get_patient_count(p_provider_id)

            return
                DataConnection.ExecuteLong(
                    "SELECT COUNT(DISTINCT patient.PatNum) " +
                    "FROM patient " +
                    "WHERE (patient.PriProv = " + providerId + " OR patient.SecProv = " + providerId + ") " +
                    "AND patient.PatStatus = 0");
        }

        public static long CountClaims(long providerId)
        { 
            // usp_provider_get_claim_count(p_provider_id)

            return 
                DataConnection.ExecuteLong(
                    "SELECT COUNT(DISTINCT claim.ClaimNum) " +
                    "FROM claim " +
                    "WHERE claim.ProvBill = " + providerId + " " +
                    "OR claim.ProvTreat = " + providerId);
        }

        /// <summary>
        /// Gets the providers that can be used in reports.
        /// </summary>
        /// <returns>A list of providers.</returns>
        public static IEnumerable<Provider> GetForReporting() =>
            cache.SelectMany(x => !x.IsHiddenReport && x.Status != ProviderStatus.Deleted);


        public static bool IsAttachedToUser(long providerId)
        {
            var count = DataConnection.ExecuteLong("SELECT COUNT(*) FROM `providers` p JOIN `users` u ON (u.`provider_id` = p.`id`) WHERE p.`id` = " + providerId);

            if (count > 0)
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Identifies the status of a <see cref="Provider"/>.
    /// </summary>
    public enum ProviderStatus
    {
        /// <summary>
        /// Indicates a provider is active.
        /// </summary>
        Active,

        /// <summary>
        /// Indicates a provider has been deleted.
        /// </summary>
        Deleted
    }

    /// <summary>
    /// Identifies the eRx status of a <see cref="Provider"/>.
    /// </summary>
    public enum ProviderErxStatus
    {
        Disabled,
        Enabled,
        EnabledWithLegacy,
    }
}
