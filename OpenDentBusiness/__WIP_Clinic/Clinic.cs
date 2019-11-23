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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace OpenDentBusiness
{
    /// <summary>
    ///     <para>
    ///         A <see cref="Clinic"/> usually represents a separate physical office location. If 
    ///         multiple clinics are sharing one database, then this is used.
    ///     </para>  
    ///     <para>
    ///         Patients, Operatories, Claims, and many other types of objects can be assigned to a
    ///         clinic.
    ///     </para>
    /// </summary>
    public class Clinic : DataRecord
    {
        private static readonly DataRecordCache<Clinic> cache =
            new DataRecordCache<Clinic>("SELECT * FROM `clinics`", FromReader);

        /// <summary>
        /// The ID of the default e-mail address of the clinic.
        /// </summary>
        public long? EmailAddressId;

        /// <summary>
        ///     <para>
        ///         The ID of the default provider of the clinic.
        ///     </para>
        ///     <para>
        ///         Used in place of the default practice provider when making new patients.
        ///     </para>
        /// </summary>
        public long? ProviderId;

        /// <summary>
        ///     <para>
        ///         A description of the clinic.
        ///     </para>
        ///     <para>
        ///         Description is required and should not be blank.
        ///     </para>
        /// </summary>
        public string Description;

        /// <summary>
        ///     <para>
        ///         A abbreviation for the clinics description. Clinics are sorted by abbreviations
        ///         if the <see cref="PreferenceName.ClinicListIsAlphabetical"/> preference is true.
        ///     </para>
        ///     <para>
        ///         Abbreviation is required and should not be blank.
        ///     </para>
        /// </summary>
        public string Abbr;

        public string AddressLine1;
        public string AddressLine2;
        public string City;
        public string State;
        public string Zip;

        /// <summary>
        /// The phone number of the clinic. Should not include punctuation.
        /// </summary>
        public string Phone;

        /// <summary>
        /// The fax number of the clinic. Should not include punctuation.
        /// </summary>
        public string Fax;

        /// <summary>
        /// The account number for deposits.
        /// </summary>
        public string BankNumber;

        /// <summary>
        /// Enum:PlaceOfService Usually 0 unless a mobile clinic for instance.
        /// </summary>
        public PlaceOfService DefaultPlaceOfService;

        /// <summary>
        ///     <para>
        ///         The ID of the provider to use for insurance billing.
        ///     </para>
        ///     <para>
        ///         If set to null the treating provider will be used.
        ///     </para>
        /// </summary>
        public long? InsuranceBillingProviderId;

        public string BillingAddressLine1;
        public string BillingAddressLine2;
        public string BillingCity;
        public string BillingState;
        public string BillingZip;
        public string PayToAddressLine1;
        public string PayToAddressLine2;
        public string PayToCity;
        public string PayToState;
        public string PayToZip;

        /// <summary>
        ///     <para>
        ///         The ID definition that represents the region of the clinic.
        ///     </para>
        /// </summary>
        /// <see cref="Definition"/>
        public long? RegionId;

        /// <summary>
        /// Used to filter MedLab results by the MedLab Account Number assigned to each clinic.
        /// </summary>
        public string MedLabAccountId;

        /// <summary>
        ///     <para>
        ///         Optional notes for scheduling appointments for this clinic. Can be used to 
        ///         express scheduling conditions (e.g. ortho only, et).
        ///     </para>
        /// </summary>
        public string SchedulingNote;

        /// <summary>
        /// The clinic options.
        /// </summary>
        public ClinicOptions Options;

        /// <summary>
        ///     <para>
        ///         The sort order of the clinic. 
        ///     </para>
        ///     <para>
        ///         Clinics are only sorted by <see cref="SortOrder"/> if the 
        ///         <see cref="PreferenceName.ClinicListIsAlphabetical"/> preference is set to 
        ///         false. If the preference is true clinics are sorted by <see cref="Abbr"/>.
        ///     </para>
        /// </summary>
        public int SortOrder;

        /// <summary>
        /// Value indicating whether the clinic has been hidden.
        /// </summary>
        public bool IsHidden;

        /// <summary>
        ///     <para>
        ///         Gets a value indicating whether this clinic should be excluded from showing up 
        ///         in the Insurance Verification List.
        ///     </para>
        /// </summary>
        public bool ExcludeFromInsuranceVerification => Options.HasFlag(ClinicOptions.ExcludeFromInsuranceVerification);

        /// <summary>
        ///     <para>
        ///         Gets a value indicating whether a procedure must be attached to controlled 
        ///         prescriptions written from this clinic.
        ///     </para>
        /// </summary>
        public bool RequireProcedureOnRx => Options.HasFlag(ClinicOptions.RequireProcedureOnRx);

        /// <summary>
        ///     <para>
        ///         Gets a value indicating whether the clinic is a medical clinic.
        ///     </para> 
        ///     <para>
        ///         Used to hide/change certain areas of the program, like hiding the tooth chart 
        ///         and changing 'dentist' to 'provider'.
        ///     </para>
        /// </summary>
        public bool IsMedicalOnly => Options.HasFlag(ClinicOptions.MedicalOnly);

        /// <summary>
        ///     <para>
        ///         Gets a value indicating whether automated reminders and confirmations should be
        ///         sent for/from this clinic.
        ///     </para>
        /// </summary>
        public bool AutomaticConfirmationsEnabled => Options.HasFlag(ClinicOptions.AutomaticConfirmationsEnabled);

        /// <summary>
        ///     <para>
        ///         Gets a value indicating whether this clinic is using the default automated 
        ///         reminder/confirmation settings as defined by the user.
        ///     </para>
        /// </summary>
        public bool UseDefaultConfirmations => Options.HasFlag(ClinicOptions.UseDefaultConfirmations);

        /// <summary>
        ///     <para>
        ///         Gets a value indicating whether to use the billing address of this clinic for 
        ///         claims.
        ///     </para>
        /// </summary>
        public bool UseBillingAddressOnClaims => Options.HasFlag(ClinicOptions.UseBillingAddressOnClaims);

        /// <summary>
        ///     <para>
        ///         Gets a value indicating whether texting is enabled for this clinic.
        ///     </para>
        /// </summary>
        public bool IsTextingEnabled => true; // TODO: Implement conditions for this...

        /// <summary>
        /// Returns a string representation of the clinic.
        /// </summary>
        public override string ToString() => Abbr ?? Description ?? "";






        ///<summary>List of specialty DefLinks for the clinic.  Not a database column.  Filled when the clinic cache is filled.</summary>
        [ODTableColumn(IsNotDbColumn = true)]
        private List<DefLink> _listClinicSpecialtyDefLinks;

        ///<summary>List of specialty DefLinks for the clinic.  Not a database column.  Filled when the clinic cache is filled.</summary>
        [XmlIgnore, JsonIgnore]
        public List<DefLink> ListClinicSpecialtyDefLinks
        {
            get
            {
                if (_listClinicSpecialtyDefLinks == null)
                {
                    _listClinicSpecialtyDefLinks = new List<DefLink>();
                    if (Id > 0)
                    {
                        _listClinicSpecialtyDefLinks = DefLinks.GetListByFKey(Id, DefLinkType.Clinic);
                    }
                }
                return _listClinicSpecialtyDefLinks;
            }
            set
            {
                _listClinicSpecialtyDefLinks = value;
            }
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Clinic"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Clinic"/> instance.</returns>
        public static Clinic FromReader(MySqlDataReader dataReader)
        {
            return new Clinic
            {
                Id = (long)dataReader["id"],
                EmailAddressId = dataReader["email_address_id"] as long?,
                ProviderId = dataReader["provider_id"] as long?,
                Description = (string)dataReader["description"],
                Abbr = (string)dataReader["abbr"],
                AddressLine1 = (string)dataReader["address_line1"],
                AddressLine2 = (string)dataReader["address_line2"],
                City = (string)dataReader["city"],
                State = (string)dataReader["state"],
                Zip = (string)dataReader["zip"],
                Phone = (string)dataReader["phone"],
                Fax = (string)dataReader["fax"],
                BankNumber = (string)dataReader["bank_number"],
                DefaultPlaceOfService = (PlaceOfService)(int)dataReader["default_place_of_service"],
                InsuranceBillingProviderId = dataReader["insurance_provider_id"] as long?,
                BillingAddressLine1 = (string)dataReader["billing_address_line1"],
                BillingAddressLine2 = (string)dataReader["billing_address_line2"],
                BillingCity = (string)dataReader["billing_city"],
                BillingState = (string)dataReader["billing_state"],
                BillingZip = (string)dataReader["billing_zip"],
                PayToAddressLine1 = (string)dataReader["pay_to_address_line1"],
                PayToAddressLine2 = (string)dataReader["pay_to_address_line2"],
                PayToCity = (string)dataReader["pay_to_city"],
                PayToState = (string)dataReader["pay_to_state"],
                PayToZip = (string)dataReader["pay_to_zip"],
                RegionId = dataReader["region_id"] as long?,
                MedLabAccountId = dataReader["medlab_account_id"] as string,
                SchedulingNote = (string)dataReader["scheduling_note"],
                Options = (ClinicOptions)(int)dataReader["options"],
                SortOrder = (int)dataReader["sort_order"],
                IsHidden = (bool)dataReader["hidden"]
            };
        }

        /// <summary>
        ///     <para>
        ///         Gets all the clinics.
        ///     </para>
        /// </summary>
        /// <returns>A list of clinics.</returns>
        public static IEnumerable<Clinic> All() =>
            cache.All();

        /// <summary>
        ///     <para>
        ///         Gets the <see cref="Clinic"/> with the specified ID from the database.
        ///     </para>
        /// </summary>
        /// <param name="clinicId">The ID of the clinic.</param>
        /// <returns>The clinic with the specified ID.</returns>
        public static Clinic GetById(long clinicId) =>
            cache.SelectOne(clinic => clinic.Id == clinicId);

        /// <summary>
        ///     <para>
        ///         Gets the <see cref="Clinic"/> whose description matches the specified 
        ///         <paramref name="description"/>.
        ///     </para>
        /// </summary>
        /// <param name="description">The clinic description.</param>
        /// <returns>The clinic matching the given description.</returns>
        public static Clinic GetByDescription(string description) =>
            cache.SelectOne(
                clinic => 
                    clinic.Description != null && 
                    clinic.Description.Equals(description, StringComparison.InvariantCultureIgnoreCase));

        /// <summary>
        ///     <para>
        ///         Gets all clinics with the specified ID's.
        ///     </para>
        /// </summary>
        /// <param name="clinicIds">A list of clinic ID's.</param>
        /// <returns>A list of clinics.</returns>
        public static IEnumerable<Clinic> GetByIds(IEnumerable<long> clinicIds) =>
            cache.SelectMany(clinic => clinicIds.Contains(clinic.Id));

        /// <summary>
        ///     <para>
        ///         Gets all clinics with the specified regions.
        ///     </para>
        /// </summary>
        /// <param name="regions">A list of region definition ID's.</param>
        /// <returns>A list of a clinics.</returns>
        public static IEnumerable<Clinic> GetByRegion(IEnumerable<long> regions) =>
            cache.SelectMany(clinic => clinic.RegionId.HasValue && regions.Contains(clinic.RegionId.Value));

        /// <summary>
        ///     <para>
        ///         Gets the default clinic to use for texting.
        ///     </para>
        /// </summary>
        /// <returns>The clinic to use for texting.</returns>
        public static Clinic GetDefaultForTexting() =>
            cache.SelectOne(clinic => clinic.Id == Preference.GetLong(PreferenceName.TextingDefaultClinicNum));

        /// <summary>
        ///     <para>
        ///         Gets all clinics the specified <paramref name="user"/> has access to.
        ///     </para>
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="includeHidden">A value indicating whether the include hidden clinics.</param>
        /// <returns>A list of clinics.</returns>
        public static IEnumerable<Clinic> GetByUser(User user, bool includeHidden = false)
        {
            foreach (var clinic in cache.All())
            {
                if (clinic.IsHidden && !includeHidden) continue;

                if (user.ClinicRestricted && user.ClinicId.HasValue)
                {
                    if (clinic.Id == user.ClinicId)
                    {
                        yield return clinic;
                    }
                }
                else
                {
                    yield return clinic;
                }
            }
        }

        /// <summary>
        ///     <para>
        ///         Determines whether the provider with the specified ID is assigned as the 
        ///         default provider to any clinic.
        ///     </para>
        /// </summary>
        /// <param name="providerId">The ID of the provider.</param>
        /// <returns>
        ///     True if the provider is the default provider of any clinic; otherwise, false.
        /// </returns>
        public static bool IsDefaultProvider(long providerId) =>
            cache.Any(clinic => clinic.ProviderId == providerId);

        /// <summary>
        ///     <para>
        ///         Determines whether the provider with the specified ID is assigned as the 
        ///         provider for insurance billing to any clinic.
        ///     </para>
        /// </summary>
        /// <param name="providerId">The ID of the provider.</param>
        /// <returns>
        ///     True if the provider is assigned as the provider for insurace billing of any 
        ///     clinic.
        /// </returns>
        public static bool IsProviderForInsuranceBilling(long providerId) =>
            cache.Any(clinic => clinic.InsuranceBillingProviderId == providerId);

        /// <summary>
        /// Inserts the specified <see cref="Clinic"/> into the database.
        /// </summary>
        /// <param name="clinic">The clinic.</param>
        /// <returns>The ID assigned to the clinic.</returns>
        public static long Insert(Clinic clinic) =>
            clinic.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `clinics` (`email_address_id`, `provider_id`, `description`, `abbr`, " +
                "`address_line1`, `address_line2`, `city`, `state`, `zip`, `phone`, `fax`, " +
                "`bank_number`, `default_place_of_service`, `insurance_provider_id`, " +
                "`billing_address_line1`, `billing_address_line2`, `billing_city`, `billing_state`, " +
                "`billing_zip`, `pay_to_address_line1`, `pay_to_address_line2`, `pay_to_city`, " +
                "`pay_to_state`, `pay_to_zip`, `region_id`, `medlab_account_id`, `scheduling_note`, " +
                "`options`, `sort_order`, `hidden`) VALUES (?email_address_id, ?provider_id, " +
                "?description, ?abbr, ?address_line1, ?address_line2, ?city, ?state, ?zip, ?phone, " +
                "?fax, ?bank_number, ?default_place_of_service, ?insurance_provider_id, " +
                "?billing_address_line1, ?billing_address_line2, ?billing_city, ?billing_state, " +
                "?billing_zip, ?pay_to_address_line1, ?pay_to_address_line2, ?pay_to_city, " +
                "?pay_to_state, ?pay_to_zip, ?region_id, ?medlab_account_id, ?scheduling_note, " +
                "?options, ?sort_order, ?hidden)",
                    new MySqlParameter("email_address_id", ValueOrDbNull(clinic.EmailAddressId)),
                    new MySqlParameter("provider_id", ValueOrDbNull(clinic.ProviderId)),
                    new MySqlParameter("description", clinic.Description ?? ""),
                    new MySqlParameter("abbr", clinic.Abbr ?? ""),
                    new MySqlParameter("address_line1", clinic.AddressLine1 ?? ""),
                    new MySqlParameter("address_line2", clinic.AddressLine2 ?? ""),
                    new MySqlParameter("city", clinic.City ?? ""),
                    new MySqlParameter("state", clinic.State ?? ""),
                    new MySqlParameter("zip", clinic.Zip ?? ""),
                    new MySqlParameter("phone", clinic.Phone ?? ""),
                    new MySqlParameter("fax", clinic.Fax ?? ""),
                    new MySqlParameter("bank_number", clinic.BankNumber ?? ""),
                    new MySqlParameter("default_place_of_service", (int)clinic.DefaultPlaceOfService),
                    new MySqlParameter("insurance_provider_id", ValueOrDbNull(clinic.InsuranceBillingProviderId)),
                    new MySqlParameter("billing_address_line1", clinic.BillingAddressLine1 ?? ""),
                    new MySqlParameter("billing_address_line2", clinic.BillingAddressLine2 ?? ""),
                    new MySqlParameter("billing_city", clinic.BillingCity ?? ""),
                    new MySqlParameter("billing_state", clinic.BillingState ?? ""),
                    new MySqlParameter("billing_zip", clinic.BillingZip ?? ""),
                    new MySqlParameter("pay_to_address_line1", clinic.PayToAddressLine1 ?? ""),
                    new MySqlParameter("pay_to_address_line2", clinic.PayToAddressLine2 ?? ""),
                    new MySqlParameter("pay_to_city", clinic.PayToCity ?? ""),
                    new MySqlParameter("pay_to_state", clinic.PayToState ?? ""),
                    new MySqlParameter("pay_to_zip", clinic.PayToZip ?? ""),
                    new MySqlParameter("region_id", ValueOrDbNull(clinic.RegionId)),
                    new MySqlParameter("medlab_account_id", ValueOrDbNull(clinic.MedLabAccountId)),
                    new MySqlParameter("scheduling_note", clinic.SchedulingNote ?? ""),
                    new MySqlParameter("options", (int)clinic.Options),
                    new MySqlParameter("sort_order", clinic.SortOrder),
                    new MySqlParameter("hidden", clinic.IsHidden));

        /// <summary>
        /// Updates the specified <see cref="Clinic"/> in the database.
        /// </summary>
        /// <param name="clinic">The clinic.</param>
        public static void Update(Clinic clinic) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `clinics` SET `email_address_id` = ?email_address_id, `provider_id` = ?provider_id, `description` = ?description, " +
                "`abbr` = ?abbr, `address_line1` = ?address_line1, `address_line2` = ?address_line2, `city` = ?city, `state` = ?state, " +
                "`zip` = ?zip, `phone` = ?phone, `fax` = ?Fax, `bank_number` = ?bank_nunmber, " +
                "`default_place_of_service` = ?default_place_of_service, `insurance_provider_id` = ?insurance_provider_id, " +
                "`billing_address_line1` = ?billing_address_line1, `billing_address_line2` = ?billing_address_line2, " +
                "`billing_city` = ?billing_city, `billing_state` = ?billing_state, `billing_zip` = ?billing_zip, " +
                "`pay_to_address_line1` = ?pay_to_address_line1, `pay_to_adress_line2` = ?pay_to_address_line2, `pay_to_city` = ?pay_to_city, " +
                "`pay_to_zip` = ?pay_to_zip, `region_id` = ?region_id, `medlab_account_id` = ?medlab_account_id, " +
                "`scheduling_note` = ?scheduling_note, `options` = ?options, `sort_order` = ?sort_order, `hidden` = ?hidden " +
                "WHERE `id` = ?id",
                    new MySqlParameter("email_address_id", ValueOrDbNull(clinic.EmailAddressId)),
                    new MySqlParameter("provider_id", ValueOrDbNull(clinic.ProviderId)),
                    new MySqlParameter("description", clinic.Description ?? ""),
                    new MySqlParameter("abbr", clinic.Abbr ?? ""),
                    new MySqlParameter("address_line1", clinic.AddressLine1 ?? ""),
                    new MySqlParameter("address_line2", clinic.AddressLine2 ?? ""),
                    new MySqlParameter("city", clinic.City ?? ""),
                    new MySqlParameter("state", clinic.State ?? ""),
                    new MySqlParameter("zip", clinic.Zip ?? ""),
                    new MySqlParameter("phone", clinic.Phone ?? ""),
                    new MySqlParameter("fax", clinic.Fax ?? ""),
                    new MySqlParameter("bank_number", clinic.BankNumber ?? ""),
                    new MySqlParameter("default_place_of_service", (int)clinic.DefaultPlaceOfService),
                    new MySqlParameter("insurance_provider_id", ValueOrDbNull(clinic.InsuranceBillingProviderId)),
                    new MySqlParameter("billing_address_line1", clinic.BillingAddressLine1 ?? ""),
                    new MySqlParameter("billing_address_line2", clinic.BillingAddressLine2 ?? ""),
                    new MySqlParameter("billing_city", clinic.BillingCity ?? ""),
                    new MySqlParameter("billing_state", clinic.BillingState ?? ""),
                    new MySqlParameter("billing_zip", clinic.BillingZip ?? ""),
                    new MySqlParameter("pay_to_address_line1", clinic.PayToAddressLine1 ?? ""),
                    new MySqlParameter("pay_to_address_line2", clinic.PayToAddressLine2 ?? ""),
                    new MySqlParameter("pay_to_city", clinic.PayToCity ?? ""),
                    new MySqlParameter("pay_to_state", clinic.PayToState ?? ""),
                    new MySqlParameter("pay_to_zip", clinic.PayToZip ?? ""),
                    new MySqlParameter("region_id", ValueOrDbNull(clinic.RegionId)),
                    new MySqlParameter("medlab_account_id", ValueOrDbNull(clinic.MedLabAccountId)),
                    new MySqlParameter("scheduling_note", clinic.SchedulingNote ?? ""),
                    new MySqlParameter("options", (int)clinic.Options),
                    new MySqlParameter("sort_order", clinic.SortOrder),
                    new MySqlParameter("hidden", clinic.IsHidden),
                    new MySqlParameter("id", clinic.Id));

        public static void Delete(long clinicId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `clinics` WHERE `id` = " + clinicId);
    }
}
