using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class VaccineObses
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
        public static long Insert(VaccineObs vaccineObs)
        {
            return Crud.VaccineObsCrud.Insert(vaccineObs);
        }

        ///<summary>Gets one VaccineObs from the db.</summary>
        public static List<VaccineObs> GetForVaccine(long vaccinePatNum)
        {
            string command = "SELECT * FROM vaccineobs WHERE VaccinePatNum=" + POut.Long(vaccinePatNum) + " ORDER BY VaccineObsNumGroup";
            return Crud.VaccineObsCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static void Update(VaccineObs vaccineObs)
        {
            Crud.VaccineObsCrud.Update(vaccineObs);
        }

        ///<summary></summary>
        public static void Delete(long vaccineObsNum)
        {
            string command = "DELETE FROM vaccineobs WHERE VaccineObsNum = " + POut.Long(vaccineObsNum);
            Db.NonQ(command);
        }

        public static void DeleteForVaccinePat(long vaccinePatNum)
        {
            string command = "DELETE FROM vaccineobs WHERE VaccinePatNum=" + POut.Long(vaccinePatNum);
            Db.NonQ(command);
        }
    }
}