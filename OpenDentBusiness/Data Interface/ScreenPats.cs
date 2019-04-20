using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class ScreenPats
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

        ///<summary></summary>
        public static long Insert(ScreenPat screenPat)
        {
            return Crud.ScreenPatCrud.Insert(screenPat);
        }

        /// <summary></summary>
        public static List<ScreenPat> GetForScreenGroup(long screenGroupNum)
        {

            string command = "SELECT * FROM screenpat WHERE ScreenGroupNum =" + POut.Long(screenGroupNum);
            return Crud.ScreenPatCrud.SelectMany(command);
        }

        ///<summary>Inserts, updates, or deletes rows to reflect changes between listScreenPats and stale listScreenPatsOld.</summary>
        public static bool Sync(List<ScreenPat> listScreenPats, List<ScreenPat> listScreenPatsOld)
        {

            return Crud.ScreenPatCrud.Sync(listScreenPats, listScreenPatsOld);
        }
    }
}