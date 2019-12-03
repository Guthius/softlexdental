using System;
using System.Reflection;

namespace OpenDentBusiness
{
    ///<summary>Inherits from appointment. A historical copy of an appointment.  These are generated as a result of an appointment being edited.  
    ///When creating for insertion it needs a passed in Appointment object.</summary>
    public class HistAppointment : Appointment
    {
        // TODO: Is this in use?

        public long HistApptNum;

        ///<summary>FK to userod.UserNum  Identifies the user that changed this appointment from previous state, not the person who originally wrote it.</summary>
        public long HistUserNum;

        ///<summary>The date and time that this appointment was edited and added to the Hist table.</summary>
        public DateTime HistDateTStamp;

        ///<summary>Enum:HistAppointmentAction .</summary>
        public HistAppointmentAction HistApptAction;

        //Note that the columns ProcsColored and Note are VARCHAR(255) in this table while they are TEXT in the appointment table. This is intentional
        //because it is less important to store the entire note and color when the appointment is not current.

        public int ApptSource;

        ///<summary>Pass in the old appointment that needs to be recorded.</summary>
        public HistAppointment(Appointment appt)
        {
            SetAppt(appt);
        }

        ///<summary>Updates the base appointment object but maintains HistAppointment filed values.</summary>
        public void SetAppt(Appointment appt)
        {
            FieldInfo[] arrayFieldInfos = typeof(Appointment).GetFields();
            foreach (FieldInfo aptField in arrayFieldInfos)
            {
                FieldInfo aptDelField = typeof(HistAppointment).GetField(aptField.Name);
                aptDelField.SetValue(this, aptField.GetValue(appt));
            }
        }

        public HistAppointment()
        {
        }

        ///<summary>Overrides Appointment.Copy() which is desired behavior because HistAppointment extends Appointment.</summary>
        public new HistAppointment Copy()
        {
            return (HistAppointment)MemberwiseClone();
        }
    }

    public enum HistAppointmentAction
    {
        Created,
        Changed,
        Missed,
        Cancelled,
        Deleted,
    }
}
