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

namespace OpenDentBusiness
{
    /// <summary>
    /// When a reminder is sent for an appointment a record of that send is stored here. 
    /// This is used to prevent re-sends of the same reminder.
    /// </summary>
    public class AppointmentReminderSent : DataRecord
    {
        public long AppointmentId;

        /// <summary>
        /// The date and time of the original appointment. 
        /// We need this in case the appointment was moved and needs another reminder sent out.
        /// </summary>
        public DateTime AppointmentDate;
        
        /// <summary>
        /// Yhe date and time that the reminder was sent out on.
        /// </summary>
        public DateTime Date;
        
        /// <summary>
        /// FK to apptreminderrule.ApptReminderRuleNum. Allows us to look up the rules to determine how to send this apptcomm out.
        /// </summary>
        public long AppointmentReminderRuleId;
        
        /// <summary>
        /// Indicates if an SMS message was succesfully sent.
        /// </summary>
        public bool SentSms;
        
        /// <summary>
        /// Indicates if an email was succesfully sent.
        /// </summary>
        public bool SentEmail;

        /// <summary>
        /// Constructs a new instance of the <see cref="AppointmentReminderSent"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="AppointmentReminderSent"/> instance.</returns>
        private static AppointmentReminderSent FromReader(MySqlDataReader dataReader)
        {
            return new AppointmentReminderSent
            {
                Id = (long)dataReader["id"],
                AppointmentId = (long)dataReader["appointment_id"],
                AppointmentDate = (DateTime)dataReader["appointment_date"],
                Date = (DateTime)dataReader["date"],
                AppointmentReminderRuleId = (long)dataReader["appointment_reminder_rule_id"],
                SentSms = Convert.ToBoolean(dataReader["sent_sms"]),
                SentEmail = Convert.ToBoolean(dataReader["sent_email"])
            };
        }

        /// <summary>
        /// Gets a list of reminders that were sent for the specified appointment.
        /// </summary>
        /// <param name="appointmentId">The ID of the appointment.</param>
        /// <returns>A list of sent reminders.</returns>
        public static List<AppointmentReminderSent> GetByAppointment(long appointmentId) =>
            SelectMany("SELECT * FROM `appointment_reminders_sent` WHERE `appointment_id` = " + appointmentId, FromReader);

        /// <summary>
        /// Inserts the specified sent appointment reminder into the database.
        /// </summary>
        /// <param name="apptReminderSent">The sent appointment reminder.</param>
        /// <returns>The ID assigned to the sent appointment reminder.</returns>
        public static long Insert(AppointmentReminderSent apptReminderSent) =>
            apptReminderSent.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `appointment_sent_reminders` (`appointment_id`, `appointment_date`, `date`, `appointment_reminder_rule_id`, " +
                "`sent_sms`, `sent_email`) VALUES (?appointment_id, ?appointment_date, ?date, ?appointment_reminder_rule_id, " +
                "?sent_sms, ?sent_email)",
                    new MySqlParameter("appointment_id", apptReminderSent.AppointmentId),
                    new MySqlParameter("appointment_date", apptReminderSent.AppointmentDate),
                    new MySqlParameter("date", apptReminderSent.Date),
                    new MySqlParameter("appointment_reminder_rule_id", apptReminderSent.AppointmentReminderRuleId),
                    new MySqlParameter("sent_sms", apptReminderSent.SentSms),
                    new MySqlParameter("sent_email", apptReminderSent.SentEmail));

        /// <summary>
        /// Inserts the specified sent appointment reminders into the database.
        /// </summary>
        /// <param name="apptRemindersSent">The sent appointment reminders.</param>
        public static void InsertMany(List<AppointmentReminderSent> apptRemindersSent) =>
            apptRemindersSent.ForEach(apptReminderSent => Insert(apptReminderSent));

        /// <summary>
        /// Updates the specified appointment reminder in the database.
        /// </summary>
        /// <param name="apptReminderSent">The sent appointment reminder.</param>
        public static void Update(AppointmentReminderSent apptReminderSent) =>
            DataConnection.ExecuteInsert(
                "UPDATE `appointment_sent_reminders` SET `appointment_id` = ?appointment_id, `appointment_date` = ?appointment_date, " +
                "`date` = ?date, `appointment_reminder_rule_id` = ?appointment_reminder_rule_id, `sent_sms` = ?sent_sms, " +
                "`sent_email` = ?sent_email WHERE `id` = ?id",
                    new MySqlParameter("appointment_id", apptReminderSent.AppointmentId),
                    new MySqlParameter("appointment_date", apptReminderSent.AppointmentDate),
                    new MySqlParameter("date", apptReminderSent.Date),
                    new MySqlParameter("appointment_reminder_rule_id", apptReminderSent.AppointmentReminderRuleId),
                    new MySqlParameter("sent_sms", apptReminderSent.SentSms),
                    new MySqlParameter("sent_email", apptReminderSent.SentEmail),
                    new MySqlParameter("id", apptReminderSent.Id));

        /// <summary>
        /// Deletes the specified sent appointment reminder from the database.
        /// </summary>
        /// <param name="apptReminderSentId">The ID of the sent reminder.</param>
        public static void Delete(long apptReminderSentId) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `appointment_reminders_sent` WHERE `id` = " + apptReminderSentId);
    }
}
