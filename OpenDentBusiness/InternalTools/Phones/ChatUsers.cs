using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class ChatUsers
    {
        ///<summary></summary>
        public static List<ChatUser> GetAll()
        {
            string command = "SELECT * FROM chatuser";
            return Crud.ChatUserCrud.SelectMany(command);
        }

        ///<summary>Gets one ChatUser from the db.</summary>
        public static ChatUser GetOne(long chatUserNum)
        {
            return Crud.ChatUserCrud.SelectOne(chatUserNum);
        }

        ///<summary></summary>
        public static long Insert(ChatUser chatUser)
        {
            return Crud.ChatUserCrud.Insert(chatUser);
        }

        ///<summary></summary>
        public static void Update(ChatUser chatUser)
        {
            Crud.ChatUserCrud.Update(chatUser);
        }

        ///<summary></summary>
        public static void Delete(long chatUserNum)
        {
            Crud.ChatUserCrud.Delete(chatUserNum);
        }

        ///<summary>Truncates the chatuser table. NBD.</summary>
        public static void Truncate()
        {
            string command = "TRUNCATE chatuser";
            Db.NonQ(command);
        }

        public static ChatUser GetFromExt(int extension)
        {
            string command = "SELECT * FROM chatuser WHERE chatuser.Extension = " + POut.Int(extension);
            return Crud.ChatUserCrud.SelectOne(command);
        }
    }
}