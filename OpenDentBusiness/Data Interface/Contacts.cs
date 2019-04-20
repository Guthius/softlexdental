using System;
using System.Collections;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class Contacts
    {
        ///<summary></summary>
        public static Contact[] Refresh(long category)
        {
            string command = "SELECT * from contact WHERE category = '" + category + "'"
                + " ORDER BY LName";
            return Crud.ContactCrud.SelectMany(command).ToArray();
        }

        ///<summary></summary>
        public static long Insert(Contact Cur)
        {
            return Crud.ContactCrud.Insert(Cur);
        }

        ///<summary></summary>
        public static void Update(Contact Cur)
        {
            Crud.ContactCrud.Update(Cur);
        }

        ///<summary></summary>
        public static void Delete(Contact Cur)
        {
            string command = "DELETE FROM contact WHERE contactnum = '" + Cur.ContactNum.ToString() + "'";
            Db.NonQ(command);
        }
    }
}