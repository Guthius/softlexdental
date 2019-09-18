using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// When a reminder is sent for an appointment a record of that send is stored here. 
    /// This is used to prevent re-sends of the same reminder.
    /// </summary>
    public class ApptReminderSent : DataRecord
    {
        public long AppointmentId;

        /// <summary>The Date and time of the original appointment. We need this in case the appointment was moved and needs another reminder sent out.</summary>
        public DateTime ApptDateTime;
        
        /// <summary>Once sent, this was the date and time that the reminder was sent out on.</summary>
        public DateTime DateTimeSent;
        
        /// <summary>This was the TSPrior used to send this reminder. </summary>
        public TimeSpan TSPrior;

        /// <summary>FK to apptreminderrule.ApptReminderRuleNum. Allows us to look up the rules to determine how to send this apptcomm out.</summary>
        public long ApptReminderRuleNum;
        
        /// <summary>
        /// Indicates if an SMS message was succesfully sent.
        /// </summary>
        public bool IsSmsSent;
        
        /// <summary>
        /// Indicates if an email was succesfully sent.
        /// </summary>
        public bool IsEmailSent;
    }
}
