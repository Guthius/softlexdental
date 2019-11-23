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
using System.Collections.Generic;
using System.Linq;

namespace OpenDentBusiness
{
    /// <summary>
    ///     <para>
    ///         Allows the user to specify DEA number override and other overrides for the provider 
    ///         at the specified clinic.
    ///     </para>
    /// </summary>
    public class ProviderClinic : DataRecordBase
    {
        private static readonly DataRecordCacheBase<ProviderClinic> cache =
            new DataRecordCacheBase<ProviderClinic>("SELECT * FROM `provider_clinics`", FromReader)
                .LinkedTo<Clinic>();

        /// <summary>
        /// The ID of the provider.
        /// </summary>
        public long ProviderId;

        /// <summary>
        /// The ID of the clinic.
        /// </summary>
        public long ClinicId;

        /// <summary>
        /// The DEA number for this provider and clinic.
        /// </summary>
        public string DEANum;

        /// <summary>
        ///     <para>
        ///         License number for this provider and clinic.
        ///     </para>
        /// </summary>
        public string StateLicense;

        /// <summary>
        ///     <para>
        ///         The state abbreviation where the state license number in the StateLicense field 
        ///         is legally registered.
        ///     </para>
        /// </summary>
        public string StateWhereLicensed;

        /// <summary>
        /// Provider medical State ID.
        /// </summary>
        public string StateRxId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderClinic"/> class.
        /// </summary>
        /// <param name="providerId"></param>
        /// <param name="clinicId"></param>
        public ProviderClinic(long providerId, long clinicId)
        {
            ProviderId = providerId;
            ClinicId = clinicId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderClinic"/> class.
        /// </summary>
        ProviderClinic()
        {
        }

        public static ProviderClinic FromReader(MySqlDataReader dataReader)
        {
            return new ProviderClinic
            {
                ProviderId = (long)dataReader["provider_id"],
                ClinicId = (long)dataReader["clinic_id"],
                DEANum = (string)dataReader["dea_number"],
                StateLicense = (string)dataReader["state_license"],
                StateRxId = (string)dataReader["state_rx_id"],
                StateWhereLicensed = (string)dataReader["state_where_licensed"]
            };
        }

        public static IEnumerable<ProviderClinic> GetByClinic(long clinicId) =>
            cache.SelectMany(providerClinic => providerClinic.ClinicId == clinicId);

        public static IEnumerable<ProviderClinic> GetByProvider(long providerId) =>
            cache.SelectMany(providerClinic => providerClinic.ProviderId == providerId);

        public static IEnumerable<ProviderClinic> GetByProviders(IEnumerable<long> providerIds) =>
            cache.SelectMany(providerClinic => providerIds.Contains(providerClinic.ProviderId));

        public static ProviderClinic GetByProviderAndClinic(long providerId, long clinicId) =>
            cache.SelectOne(providerClinic => providerClinic.ProviderId == providerId && providerClinic.ClinicId == clinicId);

        /// <summary>
        /// Returns the providers that are associated to other clinics but not this one.
        /// </summary>
        public static IEnumerable<ProviderClinic> GetProvsRestrictedToOtherClinics(long clinicId)
        {
            var providersInClinic = GetByClinic(clinicId);

            return cache.SelectMany(x => x.ClinicId != clinicId && !providersInClinic.Any(y => y.ProviderId == x.ProviderId));
        }
    }
}
