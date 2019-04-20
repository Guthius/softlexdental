using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    public class ApptReminderSents
    {
        public static List<ApptReminderSent> GetForApt(long aptNum)
        {
            return Crud.ApptReminderSentCrud.SelectMany("SELECT * FROM apptremindersent WHERE ApptNum=" + POut.Long(aptNum));
        }

        public static long Insert(ApptReminderSent apptReminderSent) => Crud.ApptReminderSentCrud.Insert(apptReminderSent);

        public static void InsertMany(List<ApptReminderSent> listApptReminderSents) => Crud.ApptReminderSentCrud.InsertMany(listApptReminderSents);

        public static void Update(ApptReminderSent apptReminderSent) => Crud.ApptReminderSentCrud.Update(apptReminderSent);

        public static void Delete(long apptReminderSentNum) => Crud.ApptReminderSentCrud.Delete(apptReminderSentNum);
    }
}