using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    public class ApptFieldDefs
    {
        private class ApptFieldDefCache : CacheListAbs<ApptFieldDef>
        {
            protected override List<ApptFieldDef> GetCacheFromDb()
            {
                return Crud.ApptFieldDefCrud.SelectMany("SELECT * FROM apptfielddef ORDER BY FieldName");
            }

            protected override List<ApptFieldDef> TableToList(DataTable table) => Crud.ApptFieldDefCrud.TableToList(table);


            protected override ApptFieldDef Copy(ApptFieldDef apptFieldDef) => apptFieldDef.Clone();

            protected override DataTable ListToTable(List<ApptFieldDef> listApptFieldDefs)
            {
                return Crud.ApptFieldDefCrud.ListToTable(listApptFieldDefs, "ApptFieldDef");
            }

            protected override void FillCacheIfNeeded() => ApptFieldDefs.GetTableFromCache(false);
        }

        static ApptFieldDefCache _apptFieldDefCache = new ApptFieldDefCache();

        public static bool GetExists(Predicate<ApptFieldDef> match, bool isShort = false)
        {
            return _apptFieldDefCache.GetExists(match, isShort);
        }

        public static List<ApptFieldDef> GetDeepCopy(bool isShort = false)
        {
            return _apptFieldDefCache.GetDeepCopy(isShort);
        }

        public static ApptFieldDef GetFirstOrDefault(Func<ApptFieldDef, bool> match, bool isShort = false)
        {
            return _apptFieldDefCache.GetFirstOrDefault(match, isShort);
        }

        /// <summary>
        /// Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.
        /// </summary>
        public static DataTable RefreshCache() => GetTableFromCache(true);

        public static void FillCacheFromTable(DataTable table)
        {
            _apptFieldDefCache.FillCacheFromTable(table);
        }

        public static DataTable GetTableFromCache(bool doRefreshCache)
        {
            return _apptFieldDefCache.GetTableFromCache(doRefreshCache);
        }

        /// <summary>
        /// Must supply the old field name so that the apptFields attached to appointments can be updated. 
        /// Will throw exception if new FieldName is already in use.
        /// </summary>
        public static void Update(ApptFieldDef apptFieldDef, string oldFieldName)
        {
            string command =
                "SELECT COUNT(*) FROM apptfielddef " +
                "WHERE FieldName='" + POut.String(apptFieldDef.FieldName) + "' " +
                "AND ApptFieldDefNum != " + POut.Long(apptFieldDef.Id);

            if (Db.GetCount(command) != "0")
            {
                throw new ApplicationException("Field name already in use.");
            }

            Crud.ApptFieldDefCrud.Update(apptFieldDef);

            Db.NonQ(
                "UPDATE apptfield SET FieldName='" + POut.String(apptFieldDef.FieldName) + "' " +
                "WHERE FieldName='" + POut.String(oldFieldName) + "'");
        }

        /// <summary>
        /// Surround with try/catch in case field name already in use.
        /// </summary>
        public static long Insert(ApptFieldDef apptFieldDef)
        {
            if (Db.GetCount("SELECT COUNT(*) FROM apptfielddef WHERE FieldName='" + POut.String(apptFieldDef.FieldName) + "'") != "0")
            {
                throw new ApplicationException("Field name already in use.");
            }

            return Crud.ApptFieldDefCrud.Insert(apptFieldDef);
        }

        ///<summary>Surround with try/catch, because it will throw an exception if any appointment is using this def.</summary>
        public static void Delete(ApptFieldDef apptFieldDef)
        {
            string command =
                "SELECT LName,FName,AptDateTime " +
                "FROM patient,apptfield,appointment " +
                "WHERE patient.PatNum=appointment.PatNum " +
                "AND appointment.AptNum=apptfield.AptNum " +
                "AND FieldName='" + POut.String(apptFieldDef.FieldName) + "'";

            DataTable table = Db.GetTable(command);
            DateTime aptDateTime;
            if (table.Rows.Count > 0)
            {
                string s = "Not allowed to delete. Already in use by " + table.Rows.Count.ToString() + " appointments, including\r\n";
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (i > 5)
                    {
                        break;
                    }
                    aptDateTime = PIn.DateT(table.Rows[i]["AptDateTime"].ToString());
                    s += table.Rows[i]["LName"].ToString() + ", " + table.Rows[i]["FName"].ToString() + POut.DateT(aptDateTime, false) + "\r\n";
                }
                throw new ApplicationException(s);
            }
            Db.NonQ("DELETE FROM apptfielddef WHERE ApptFieldDefNum =" + POut.Long(apptFieldDef.Id));
        }

        public static string GetFieldName(long apptFieldDefNum)
        {
            ApptFieldDef apptFieldDef = GetFirstOrDefault(x => x.Id == apptFieldDefNum);
            return (apptFieldDef == null ? "" : apptFieldDef.FieldName);
        }

        /// <summary>
        /// GetPickListByFieldName returns the pick list identified by the field name passed as a parameter.
        /// </summary>
        public static string GetPickListByFieldName(string FieldName)
        {
            ApptFieldDef apptFieldDef = GetFirstOrDefault(x => x.FieldName == FieldName);
            return (apptFieldDef == null ? "" : apptFieldDef.PickList);
        }

        /// <summary>
        /// Returns true if there are any duplicate field names in the entire apptfielddef table.
        /// </summary>
        public static bool HasDuplicateFieldNames()
        {
            return (Db.GetScalar("SELECT COUNT(*) FROM apptfielddef GROUP BY FieldName HAVING COUNT(FieldName) > 1") != "");
        }

        /// <summary>
        /// Returns the ApptFieldDef for the specified field name. 
        /// Returns null if an ApptFieldDef does not exist for that field name.
        /// </summary>
        public static ApptFieldDef GetFieldDefByFieldName(string fieldName) => GetFirstOrDefault(x => x.FieldName == fieldName);
    }
}