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

namespace OpenDentBusiness
{
    /// <summary>
    ///     <para>
    ///         Some insurance companies require special provider ID #s, and this table holds them.
    ///     </para>
    /// </summary>
    public class ProviderIdentity : DataRecord
    {
        public static readonly DataRecordCacheBase<ProviderIdentity> cache =
            new DataRecordCache<ProviderIdentity>("SELECT * FROM `provider_identities`", FromReader)
                .LinkedTo<Provider>();

        /// <summary>
        /// The ID of the provider.
        /// </summary>
        public long ProviderId;

        /// <summary>
        ///     <para>
        ///         Electronic ID. An ID only applies to one insurance carrier.
        ///     </para>
        ///     <para>
        ///         Foreign key, references <see cref="Carrier.ElectID"/>...
        ///     </para>
        /// </summary>
        public string PayorId;

        /// <summary>
        /// The identity type.
        /// </summary>
        public ProviderIdentityType Type;

        /// <summary>
        /// The number assigned by the insurance carrier.
        /// </summary>
        public string Number;

        /// <summary>
        /// Constructs a new instance of the <see cref="ProviderIdentity"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="ProviderIdentity"/> instance.</returns>
        public static ProviderIdentity FromReader(MySqlDataReader dataReader)
        {
            return new ProviderIdentity
            {
                Id = (long)dataReader["id"],
                ProviderId = (long)dataReader["provider_id"],
                PayorId = (string)dataReader["payor_id"],
                Type = (ProviderIdentityType)(int)dataReader["type"],
                Number = (string)dataReader["number"]
            };
        }

        /// <summary>
        /// Inserts the specified provider identity into the database.
        /// </summary>
        /// <param name="providerIdentity">The provider identity.</param>
        /// <returns>The ID assigned to the provider identity.</returns>
        public static long Insert(ProviderIdentity providerIdentity) =>
            providerIdentity.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `provider_identities` (`provider_id`, `payor_id`, `type`, `number`) " +
                "VALUES (?provider_id, ?payor_id, ?type, ?number)",
                    new MySqlParameter("provider_id", providerIdentity.ProviderId),
                    new MySqlParameter("payor_id", providerIdentity.PayorId ?? ""),
                    new MySqlParameter("type", (int)providerIdentity.Type),
                    new MySqlParameter("number", providerIdentity.Number ?? ""));

        /// <summary>
        /// Updates the specfied provider identity in the database.
        /// </summary>
        /// <param name="providerIdentity">The provider identity.</param>
        public static void Update(ProviderIdentity providerIdentity) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `provider_identities` SET `provider_id` = ?provider_id, `payor_id` = ?payor_id, " +
                "`type` = ?type, `number` = ?number WHERE `id` = ?id",
                    new MySqlParameter("provider_id", providerIdentity.ProviderId),
                    new MySqlParameter("payor_id", providerIdentity.PayorId ?? ""),
                    new MySqlParameter("type", (int)providerIdentity.Type),
                    new MySqlParameter("number", providerIdentity.Number ?? ""),
                    new MySqlParameter("id", providerIdentity.Id));

        /// <summary>
        /// Deletes the specified provider identity from the database.
        /// </summary>
        /// <param name="providerIdentityId">The ID of the provider identity to delete.</param>
        public static void Delete(long providerIdentityId) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `provider_identities` WHERE `id` = " + providerIdentityId);

        /// <summary>
        /// Gets all supplemental identifiers that have been attached to this provider. 
        /// Used in the provider edit window.
        /// </summary>
        public static IEnumerable<ProviderIdentity> GetForProv(long provNum) =>
            cache.SelectMany(x => x.ProviderId == provNum);

        /// <summary>
        /// Gets all supplemental identifiers that have been attached to this provider and for this particular payorID. 
        /// Called from X12 when creating a claim file.  Also used now on printed claims.
        /// </summary>
        public static IEnumerable<ProviderIdentity> GetForPayor(long provNum, string payorID) =>
            cache.SelectMany(x => x.ProviderId == provNum && x.PayorId == payorID);

        /// <summary>
        ///     <para>
        ///         Checks whether a identity with the given <paramref name="type"/> and 
        ///         <paramref name="payorId"/> exists for the provider with specified ID.
        ///     </para>
        /// </summary>
        /// <param name="providerId">The ID of the provider.</param>
        /// <param name="type">The identity type.</param>
        /// <param name="payorId">The payor ID.</param>
        /// <returns>True if the identity exists; otherwise, false.</returns>
        public static bool Exists(long providerId, ProviderIdentityType type, string payorId)
        {
            ProviderIdentity providerIdent = 
                cache.SelectOne(x => 
                    x.ProviderId == providerId && 
                    x.Type == type && 
                    x.PayorId == payorId);

            return providerIdent != null;
        }
    }

    /// <summary>
    /// Used when submitting e-claims to some carriers who require extra provider identifiers. 
    /// Usage varies by company.  Only used as needed. 
    /// SiteNumber is the only one that is still used on 5010s.  
    /// The other 3 have been deprecated and replaced by NPI.
    /// </summary>
    public enum ProviderIdentityType
    {
        BlueCross,
        BlueShield,
        SiteNumber,
        CommercialNumber
    }
}
