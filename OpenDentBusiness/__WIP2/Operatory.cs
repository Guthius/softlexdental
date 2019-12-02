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
using OpenDentBusiness.Crud;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenDentBusiness
{
    public class Operatory : DataRecord
    {
        private static readonly DataRecordCache<Operatory> cache =
            new DataRecordCache<Operatory>("SELECT * FROM `operatories` ORDER BY `sort_order`", FromReader);

        public long ClinicId;
        public long? ProvDentistId;
        public long? ProvHygienistId;
        public string Description;
        public string Abbr;

        ///<summary>If true patients put into this operatory will have status set to prospective.</summary>
        public bool IsProspective;

        public bool IsHygiene;
        public int SortOrder;
        public DateTime DateModified;
        public bool IsHidden;

        /// <summary>
        /// Returns a string representation of the operatory.
        /// </summary>
        public override string ToString() => Description ?? "";

        /// <summary>
        /// Constructs a new instance of the <see cref="Operatory"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Operatory"/> instance.</returns>
        public static Operatory FromReader(MySqlDataReader dataReader)
        {
            return new Operatory
            {
                Id = (long)dataReader["id"],
                ClinicId = (long)dataReader["clinic_id"],
                ProvDentistId = dataReader["prov_dentist_id"] as long?,
                ProvHygienistId = dataReader["prov_hygienist_id"] as long?,
                Description = (string)dataReader["description"],
                Abbr = (string)dataReader["abbr"],
                IsProspective = (bool)dataReader["is_prospective"],
                IsHygiene = (bool)dataReader["is_hygiene"],
                SortOrder = (int)dataReader["sort_order"],
                DateModified = (DateTime)dataReader["date_modified"],
                IsHidden = (bool)dataReader["hidden"]
            };
        }

        /// <summary>
        /// Gets all operatories.
        /// </summary>
        /// <param name="includeHidden"></param>
        public static IEnumerable<Operatory> All(bool includeHidden = false) =>
            includeHidden ? cache.All() : cache.SelectMany(operatory => !operatory.IsHidden);

        /// <summary>
        /// Gets all operatories associated with the specified clinic.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic.</param>
        /// <param name="includeHidden"></param>
        /// <returns></returns>
        public static IEnumerable<Operatory> GetByClinic(long clinicId, bool includeHidden = false) =>
            cache.SelectMany(op => op.ClinicId == clinicId && (includeHidden || !op.IsHidden));

        /// <summary>
        /// Gets the operatory with the specified ID.
        /// </summary>
        /// <param name="operatoryId">The ID of the operatory.</param>
        /// <returns>The operatory.</returns>
        public static Operatory GetById(long operatoryId) =>
            cache.SelectOne(operatory => operatory.Id == operatoryId);

        /// <summary>
        /// Inserst the specified <paramref name="operatory"/> into the database.
        /// </summary>
        /// <param name="operatory">The operatory.</param>
        /// <returns>The ID assigned to the operatory.</returns>
        public static long Insert(Operatory operatory) =>
            operatory.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `operatories` (`clinic_id`, `prov_dentist_id`, `prov_hygienist_id`, `description`, `abbr`, " +
                "`is_prospective`, `is_hygiene`, `sort_order`, `hidden`) VALUES (?clinic_id, ?prov_dentist_id, " +
                "?prov_hygienist_id, ?description, ?abbr, ?is_prospective, ?is_hygiene, ?sort_order, ?hidden)",
                    new MySqlParameter("clinic_id", operatory.ClinicId),
                    new MySqlParameter("prov_dentist_id", ValueOrDbNull(operatory.ProvDentistId)),
                    new MySqlParameter("prov_hygienist_id", ValueOrDbNull(operatory.ProvHygienistId)),
                    new MySqlParameter("description", operatory.Description ?? ""),
                    new MySqlParameter("abbr", operatory.Abbr ?? ""),
                    new MySqlParameter("is_prospective", operatory.IsProspective),
                    new MySqlParameter("is_hygiene", operatory.IsHygiene),
                    new MySqlParameter("sort_order", operatory.SortOrder),
                    new MySqlParameter("hidden", operatory.IsHidden));

        /// <summary>
        /// Updates the specified <paramref name="operatory"/> in the database.
        /// </summary>
        /// <param name="operatory">The operatory.</param>
        public static void Update(Operatory operatory) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `operatories` SET `clinic_id` = ?clinic_id, `prov_dentist_id` = ?prov_dentist_id, `prov_hygienist_id` = ?prov_hygienist_id, " +
                "`description` = ?description, `abbr` = ?abbr, `is_prospective` = ?is_prospective, `is_hygiene` = ?is_hygiene, `sort_order` = ?sort_order, " +
                "`hidden` = ?hidden WHERE `id` = ?id",
                    new MySqlParameter("clinic_id", operatory.ClinicId),
                    new MySqlParameter("prov_dentist_id", ValueOrDbNull(operatory.ProvDentistId)),
                    new MySqlParameter("prov_hygienist_id", ValueOrDbNull(operatory.ProvHygienistId)),
                    new MySqlParameter("description", operatory.Description ?? ""),
                    new MySqlParameter("abbr", operatory.Abbr ?? ""),
                    new MySqlParameter("is_prospective", operatory.IsProspective),
                    new MySqlParameter("is_hygiene", operatory.IsHygiene),
                    new MySqlParameter("sort_order", operatory.SortOrder),
                    new MySqlParameter("hidden", operatory.IsHidden),
                    new MySqlParameter("id", operatory.Id));


        /// <summary>
        ///     <para>
        ///         Checks whether there are any future appointments scheduled for the operatory 
        ///         with the specified ID.
        ///     </para>
        /// </summary>
        /// <param name="operatoryId">The ID of the operatory.</param>
        /// <param name="ignoreStatuses">The statusses the ignore.</param>
        /// <returns>True if there are future appointments for the operatory; otherwise, false.</returns>
        public static bool HasFutureApts(long operatoryId, params ApptStatus[] ignoreStatuses)
        {
            string commandText = "SELECT COUNT(*) FROM `appointment` WHERE `Op` = " + operatoryId + " ";

            if (ignoreStatuses.Length > 0)
            {
                commandText += "AND `AptStatus` NOT IN (" + string.Join(", ", ignoreStatuses.Select(s => (int)s)) + ") ";
            }

            commandText += "AND `AptDateTime` > " + DbHelper.Now();

            return DataConnection.ExecuteLong(commandText) > 0;
        }

        ///<summary>Returns a list of all appointments and whether that appointment has a conflict for the given listChildOpNums.
        ///Used to determine if there are any overlapping appointments for ALL time between a 'master' op appointments and the 'child' ops appointments.
        ///If an appointment from one of the give child ops has a confilict with the master op, then the appointment.Tag will be true.
        ///Throws exceptions.</summary>
        public static List<Tuple<Appointment, bool>> MergeApptCheck(long keepOperatoryId, List<long> mergeOperatoryIds)
        {
            if (mergeOperatoryIds == null || mergeOperatoryIds.Count == 0)
            {
                return new List<Tuple<Appointment, bool>>();
            }

            if (mergeOperatoryIds.Contains(keepOperatoryId))
            {
                throw new ApplicationException(
                    "The operatory to keep cannot be within the selected list of operatories to combine.");
            }

            string commandText = 
                "SELECT * FROM appointment " +
                "WHERE Op IN (" + string.Join(",", mergeOperatoryIds.Concat(new[] { keepOperatoryId })) + ") " +
                "AND AptStatus IN (" + string.Join(",", new[] { (int)ApptStatus.Scheduled, (int)ApptStatus.Complete, (int)ApptStatus.Broken, (int)ApptStatus.PtNote }) + ")";

            var appointments = AppointmentCrud.SelectMany(commandText);

            return 
                appointments
                    .Where(appointment => appointment.Op != keepOperatoryId)
                    .Select(appointment => new Tuple<Appointment, bool>(appointment, HasConflict(appointment, appointments))).ToList();
        }

        private static bool HasConflict(Appointment appointment, List<Appointment> allAppointments)
        {
            return 
                allAppointments.Any(
                    otherAppointment => 
                        otherAppointment.AptNum != appointment.AptNum && 
                        ((otherAppointment.AptDateTime <= appointment.AptDateTime && otherAppointment.AptDateTime.AddMinutes(otherAppointment.Pattern.Length * 5) > appointment.AptDateTime) || 
                         (appointment.AptDateTime <= otherAppointment.AptDateTime && appointment.AptDateTime.AddMinutes(appointment.Pattern.Length * 5) > otherAppointment.AptDateTime)));
        }

        /// <summary>
        /// Hides all operatories that are not the master op and moves any appointments passed in into the master op.
        /// </summary>
        public static void MergeOperatoriesIntoMaster(long keepOperatoryId, List<Operatory> mergeOperatories, List<Appointment> mergeAppointments)
        {
            var keepOperatory = GetById(keepOperatoryId);
            if (keepOperatory == null)
            {
                throw new ApplicationException(
                    "Operatory to merge into no longer exists.");
            }

            if (mergeAppointments.Count > 0)
            {
                var newMergeAppointments = mergeAppointments.Select(x => x.Copy()).ToList();

                foreach (var appointment in newMergeAppointments)
                {
                    appointment.Op = keepOperatoryId;
                }

                Appointments.Sync(newMergeAppointments, mergeAppointments, 0);
            }

            foreach (var operatory in mergeOperatories)
            {
                if (operatory.Id == keepOperatoryId) continue;

                operatory.IsHidden = true;

                Update(operatory);
            }

            SecurityLog.Write(
                SecurityLogEvents.Setup,
                "The following operatories and all of their appointments were merged into the " + keepOperatory.Abbr + " operatory; " +
                string.Join(", ", mergeOperatories.Select(operatory => operatory.Abbr)));
        }
    }
}
