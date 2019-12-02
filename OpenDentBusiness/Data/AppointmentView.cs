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
using System.Linq;

namespace OpenDentBusiness
{
    public class AppointmentView : DataRecord
    {
        public long ClinicId;
        public string Description;

        /// <summary>
        /// Number of rows per time increment.  Usually 1 or 2.  Programming note: Value updated to ApptDrawing.RowsPerIncr to track current state.
        /// </summary>
        public int RowsPerIncrement;

        /// <summary>
        ///     <para>
        ///         A value indicating whether operatories will only be shown for providers that 
        ///         have schedules for a given day and operatories that have no provider assigned.
        ///     </para>
        /// </summary>
        public bool OnlyScheduledProviders;

        /// <summary>
        ///     <para>
        ///         When <see cref="OnlyScheduledProviders"/> is true and this time is not 
        ///         <see cref="TimeSpan.Zero"/>, only provider schedules that start or stop after
        ///         this time will be included.
        ///     </para>
        /// </summary>
        public TimeSpan OnlyScheduleAfter;

        /// <summary>
        ///     <para>
        ///         When <see cref="OnlyScheduledProviders"/> is true and this time is not 
        ///         <see cref="TimeSpan.Zero"/>, only provider schedules that start or stop before
        ///         this time will be included.
        ///     </para>
        /// </summary>
        public TimeSpan OnlyScheduleBefore;

        public AppointmentViewStackBehaviour StackBehaviourUR;
        public AppointmentViewStackBehaviour StackBehaviourLR;

        /// <summary>
        ///     <para>
        ///         The time to auto scroll to when the appointment view is loaded. Only used when
        ///         <see cref="ScrollStartDynamic"/> is set to false.
        ///     </para>
        /// </summary>
        public TimeSpan ScrollStartTime;

        /// <summary>
        ///     <para>
        ///         A value indicating whether the appointment view should auto scroll to the first
        ///         scheduled appointment. If set the false the appointment view will auto scroll 
        ///         to the time indicated by <see cref="ScrollStartTime"/>.
        ///     </para>
        /// </summary>
        public bool ScrollStartDynamic;

        /// <summary>
        ///     <para>
        ///         A value indicating whether to hide appointment bubbles.
        ///     </para>
        /// </summary>
        public bool HideAppointmentBubbles;

        /// <summary>
        /// Returns a string representation of the appointment view.
        /// </summary>
        public override string ToString() => Description ?? "";

        /// <summary>
        /// Constructs a new instance of the <see cref="AppointmentView"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="AppointmentView"/> instance.</returns>
        public static AppointmentView FromReader(MySqlDataReader dataReader)
        {
            return new AppointmentView
            {
                Id = (long)dataReader["id"],
                ClinicId = (long)dataReader["clinic_id"],
                Description = (string)dataReader["description"],
                RowsPerIncrement = (int)dataReader["rows_per_increment"],
                OnlyScheduledProviders = (bool)dataReader["only_scheduled_providers"],
                OnlyScheduleAfter = (TimeSpan)dataReader["only_schedule_after"],
                OnlyScheduleBefore = (TimeSpan)dataReader["only_schedule_before"],
                StackBehaviourUR = (AppointmentViewStackBehaviour)(int)dataReader["stack_behaviour_ur"],
                StackBehaviourLR = (AppointmentViewStackBehaviour)(int)dataReader["stack_behaviour_lr"],
                ScrollStartTime = (TimeSpan)dataReader["scroll_start_time"],
                ScrollStartDynamic = (bool)dataReader["scroll_start_dynamic"],
                HideAppointmentBubbles = (bool)dataReader["hide_appointment_bubbles"]
            };
        }

        /// <summary>
        /// Gets all the appointment views.
        /// </summary>
        public static IEnumerable<AppointmentView> All =>
            SelectMany("SELECT * FROM `appointment_views` ORDER BY `description`", FromReader);

        /// <summary>
        /// Gets the appointment view with the specified ID from the database.
        /// </summary>
        /// <param name="appointmentViewId">The ID of the appointment view.</param>
        /// <returns>The appointment view.</returns>
        public static AppointmentView GetById(long appointmentViewId) =>
            SelectOne("SELECT * FROM `appointment_views` WHERE `id` = " + appointmentViewId, FromReader);

        /// <summary>
        /// Gets the appointment views for the specified clinic from the database.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic.</param>
        /// <returns>A list of appointment views.</returns>
        public static IEnumerable<AppointmentView> GetByClinic(long clinicId) =>
            SelectMany(
                "SELECT * FROM `appointment_views` WHERE `clinic_id` = " + clinicId + " OR `clinic_id` IS NULL ORDER BY `description`",
                FromReader);

        /// <summary>
        /// Gets the ID's of all operatories assigned to the appointment view with the specified ID.
        /// </summary>
        /// <param name="appointmentViewId">The ID of the appointment view.</param>
        /// <returns>The ID's of the operatories assigned to the appointment view.</returns>
        public static IEnumerable<long> GetOperatoryIds(long appointmentViewId) =>
            SelectMany("SELECT * FROM `appointment_view_items` WHERE `appointment_view_id` = " + appointmentViewId, AppointmentViewItem.FromReader)
                .Where(appointmentViewItem => appointmentViewItem.OperatoryId.HasValue)
                .Select(appointmentViewItem => appointmentViewItem.OperatoryId.Value);

        /// <summary>
        /// Gets the ID's of all providers assigned to the appointment view with the specified ID.
        /// </summary>
        /// <param name="appointmentViewId">The ID of the appointment view.</param>
        /// <returns>The ID's of the providers assigned to the appointment view.</returns>
        public static IEnumerable<long> GetProviderIds(long appointmentViewId) =>
            SelectMany("SELECT * FROM `appointment_view_items` WHERE `appointment_view_id` = " + appointmentViewId, AppointmentViewItem.FromReader)
                .Where(appointmentViewItem => appointmentViewItem.ProviderId.HasValue)
                .Select(appointmentViewItem => appointmentViewItem.ProviderId.Value);

        /// <summary>
        /// Inserts the specified appointment view into the database.
        /// </summary>
        /// <param name="appointmentView">The appointment view.</param>
        /// <returns>The ID assigned to the appointment view.</returns>
        public static long Insert(AppointmentView appointmentView) =>
            appointmentView.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `appointment_views` (`clinic_id`, `description`, `rows_per_increment`, `only_scheduled_providers`, " +
                "`only_schedule_after`, `only_schedule_before`, `stack_behaviour_ur`, `stack_behaviour_lr`, `scroll_start_time`, `scroll_start_dynamic`, " +
                "`hide_appointment_bubbles`) VALUES (?clinic_id, ?description, ?rows_per_increment, ?only_scheduled_providers, ?only_schedule_after, " +
                "?only_schedule_before, ?stack_behaviour_ur, ?stack_behaviour_lr, ?scroll_start_time, ?scroll_start_dynamic, " +
                "?hide_appointment_bubbles)",
                    new MySqlParameter("clinic_id", appointmentView.ClinicId),
                    new MySqlParameter("description", appointmentView.Description ?? ""),
                    new MySqlParameter("rows_per_increment", appointmentView.RowsPerIncrement),
                    new MySqlParameter("only_scheduled_providers", appointmentView.OnlyScheduledProviders),
                    new MySqlParameter("only_schedule_after", appointmentView.OnlyScheduleAfter),
                    new MySqlParameter("only_schedule_before", appointmentView.OnlyScheduleBefore),
                    new MySqlParameter("stack_behaviour_ur", (int)appointmentView.StackBehaviourUR),
                    new MySqlParameter("stack_behaviour_lr", (int)appointmentView.StackBehaviourLR),
                    new MySqlParameter("scroll_start_time", appointmentView.ScrollStartTime),
                    new MySqlParameter("scroll_start_dynamic", appointmentView.ScrollStartDynamic),
                    new MySqlParameter("hide_appointment_bubbles", appointmentView.HideAppointmentBubbles));

        /// <summary>
        /// Updates the specified appointment view in the database.
        /// </summary>
        /// <param name="appointmentView">The appointment view.</param>
        public static void Update(AppointmentView appointmentView) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `appointment_views` SET `clinic_id` = ?clinic_id, `description` = ?description, `rows_per_increment` = ?rows_per_increment, " +
                "`only_scheduled_providers` = ?only_scheduled_providers, `only_schedule_after` = ?only_schedule_after, `only_schedule_before` = ?only_schedule_before, " +
                "`stack_behaviour_ur` = ?stack_behaviour_ur, `stack_behaviour_lr` = ?stack_behaviour_lr, `scroll_start_time` = ?scroll_start_time, " +
                "`scroll_start_dynamic` = ?scroll_start_dynamic, `hide_appointment_bubbles` = ?hide_appointment_bubbles WHERE `id` = ?id",
                    new MySqlParameter("clinic_id", appointmentView.ClinicId),
                    new MySqlParameter("description", appointmentView.Description ?? ""),
                    new MySqlParameter("rows_per_increment", appointmentView.RowsPerIncrement),
                    new MySqlParameter("only_scheduled_providers", appointmentView.OnlyScheduledProviders),
                    new MySqlParameter("only_schedule_after", appointmentView.OnlyScheduleAfter),
                    new MySqlParameter("only_schedule_before", appointmentView.OnlyScheduleBefore),
                    new MySqlParameter("stack_behaviour_ur", (int)appointmentView.StackBehaviourUR),
                    new MySqlParameter("stack_behaviour_lr", (int)appointmentView.StackBehaviourLR),
                    new MySqlParameter("scroll_start_time", appointmentView.ScrollStartTime),
                    new MySqlParameter("scroll_start_dynamic", appointmentView.ScrollStartDynamic),
                    new MySqlParameter("hide_appointment_bubbles", appointmentView.HideAppointmentBubbles),
                    new MySqlParameter("id", appointmentView.Id));

        /// <summary>
        /// Deletes the appointment view with the specified ID from the database.
        /// </summary>
        /// <param name="appointmentViewId">The ID of the appointment view.</param>
        public static void Delete(long appointmentViewId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM `appointment_views` WHERE `id` = " + appointmentViewId);
    }
}
