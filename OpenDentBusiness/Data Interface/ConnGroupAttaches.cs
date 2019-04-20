using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class ConnGroupAttaches
    {
        #region Get Methods
        #endregion

        #region Modification Methods

        #region Insert
        #endregion

        #region Update
        #endregion

        #region Delete
        #endregion

        #endregion

        #region Misc Methods
        #endregion


        ///<summary>Inserts, updates, or deletes database rows to match supplied list.  Must always pass in ConnectionGroupNum.</summary>
        public static void Sync(List<ConnGroupAttach> listNew, long connectionGroupNum)
        {
            List<ConnGroupAttach> listDB = ConnGroupAttaches.GetForGroup(connectionGroupNum);
            Crud.ConnGroupAttachCrud.Sync(listNew, listDB);
        }

        ///<summary>Gets one ConnGroupAttach from the db.</summary>
        public static ConnGroupAttach GetOne(long connGroupAttachNum)
        {
            return Crud.ConnGroupAttachCrud.SelectOne(connGroupAttachNum);
        }

        ///<summary>Gets all conn group attaches from the database.</summary>
        public static List<ConnGroupAttach> GetAll()
        {
            string command = "SELECT * FROM conngroupattach";
            return Crud.ConnGroupAttachCrud.SelectMany(command);
        }

        ///<summary>Gets all ConnGroupAttaches for a given ConnectionGroupNum.</summary>
        public static List<ConnGroupAttach> GetForGroup(long connectionGroupNum)
        {
            string command = "SELECT * FROM conngroupattach WHERE ConnectionGroupNum=" + POut.Long(connectionGroupNum);
            return Crud.ConnGroupAttachCrud.SelectMany(command);
        }

        ///<summary>Gets all ConnGroupAttaches for a given CentralConnectionNum.</summary>
        public static List<ConnGroupAttach> GetForConnection(long connectionNum)
        {
            string command = "SELECT * FROM conngroupattach WHERE CentralConnectionNum=" + POut.Long(connectionNum);
            return Crud.ConnGroupAttachCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static long Insert(ConnGroupAttach connGroupAttach)
        {
            return Crud.ConnGroupAttachCrud.Insert(connGroupAttach);
        }

        ///<summary></summary>
        public static void Update(ConnGroupAttach connGroupAttach)
        {
            Crud.ConnGroupAttachCrud.Update(connGroupAttach);
        }

        ///<summary></summary>
        public static void Delete(long connGroupAttachNum)
        {
            string command = "DELETE FROM conngroupattach WHERE ConnGroupAttachNum = " + POut.Long(connGroupAttachNum);
            Db.NonQ(command);
        }
    }
}