using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class PhoneNumbers
    {

        public static List<PhoneNumber> GetPhoneNumbers(long patNum)
        {
            string command = "SELECT * FROM phonenumber WHERE PatNum=" + POut.Long(patNum);
            return Crud.PhoneNumberCrud.SelectMany(command);
        }

        public static PhoneNumber GetByVal(string phoneNumberVal)
        {
            string command = "SELECT * FROM phonenumber WHERE PhoneNumberVal='" + POut.String(phoneNumberVal) + "'";
            return Crud.PhoneNumberCrud.SelectOne(command);
        }

        ///<summary></summary>
        public static long Insert(PhoneNumber phoneNumber)
        {
            return Crud.PhoneNumberCrud.Insert(phoneNumber);
        }

        ///<summary></summary>
        public static void Update(PhoneNumber phoneNumber)
        {
            Crud.PhoneNumberCrud.Update(phoneNumber);
        }

        public static void DeleteObject(long phoneNumberNum)
        {
            Crud.PhoneNumberCrud.Delete(phoneNumberNum);
        }
    }
}