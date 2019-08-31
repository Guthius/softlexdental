using CodeBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Xml;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class OIDInternals
    {
        public static string OpenDentalOID = "2.16.840.1.113883.3.4337";
        private static long _customerPatNum = 0;

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


        ///<summary>Returns the currently defined OID for a given IndentifierType.  If not defined, IDroot will be empty string.</summary>
        public static OIDInternal GetForType(IdentifierType IDType)
        {
            InsertMissingValues();//
            string command = "SELECT * FROM oidinternal WHERE IDType='" + IDType.ToString() + "'";//should only return one row.
            return Crud.OIDInternalCrud.SelectOne(command);
        }

        ///<summary>There should always be one entry in the DB per IdentifierType enumeration.</summary>
        public static void InsertMissingValues()
        {
            //string command= "SELECT COUNT(*) FROM oidinternal";
            //if(PIn.Long(Db.GetCount(command))==Enum.GetValues(typeof(IdentifierType)).Length) {
            //	return;//The DB table has the right count. Which means there is probably nothing wrong with the values in it. This may need to be enhanced if customers have any issues.
            //}
            string command = "SELECT * FROM oidinternal";
            List<OIDInternal> listOIDInternals = Crud.OIDInternalCrud.SelectMany(command);
            List<IdentifierType> listIDTypes = new List<IdentifierType>();
            for (int i = 0; i < listOIDInternals.Count; i++)
            {
                listIDTypes.Add(listOIDInternals[i].IDType);
            }
            for (int i = 0; i < Enum.GetValues(typeof(IdentifierType)).Length; i++)
            {
                if (listIDTypes.Contains((IdentifierType)i))
                {
                    continue;//DB contains a row for this enum value.
                }
                //Insert missing row with blank OID.

                command = "INSERT INTO oidinternal (IDType,IDRoot) "
                + "VALUES('" + ((IdentifierType)i).ToString() + "','')";
                DataConnection.ExecuteNonQuery(command);

            }
        }

        public static List<OIDInternal> GetAll()
        {
            InsertMissingValues();//there should always be one entry in the DB for each IdentifierType enumeration, insert any missing
            string command = "SELECT * FROM oidinternal";
            return Crud.OIDInternalCrud.SelectMany(command);
        }

        public static void Update(OIDInternal oIDInternal)
        {
            Crud.OIDInternalCrud.Update(oIDInternal);
        }
    }
}