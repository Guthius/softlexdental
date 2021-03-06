using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class DefLinks
    {
        #region Get Methods
        ///<summary>Gets list of all DefLinks by defLinkType .</summary>
        public static List<DefLink> GetDefLinksByType(DefLinkType defType)
        {
            string command = "SELECT * FROM deflink WHERE LinkType=" + POut.Int((int)defType);
            return Crud.DefLinkCrud.SelectMany(command);
        }

        ///<summary>Gets list of all DefLinks for the definition and defLinkType passed in.</summary>
        public static List<DefLink> GetDefLinksByType(DefLinkType defType, long defNum)
        {
            string command = "SELECT * FROM deflink "
                + "WHERE LinkType=" + POut.Int((int)defType) + " "
                + "AND DefNum=" + POut.Long(defNum);
            return Crud.DefLinkCrud.SelectMany(command);
        }

        ///<summary>Gets list of all DefLinks for the definitions and defLinkType passed in.</summary>
        public static List<DefLink> GetDefLinksByTypeAndDefs(DefLinkType defType, List<long> listDefNums)
        {
            if (listDefNums == null || listDefNums.Count < 1)
            {
                return new List<DefLink>();
            }
            string command = "SELECT * FROM deflink "
                + "WHERE LinkType=" + POut.Int((int)defType) + " "
                + "AND DefNum IN(" + string.Join(",", listDefNums.Select(x => POut.Long(x))) + ")";
            return Crud.DefLinkCrud.SelectMany(command);
        }

        ///<summary>Gets list of all operatory specific DefLinks associated to the WebSchedNewPatApptTypes definition category.</summary>
        public static List<DefLink> GetDefLinksForWebSchedNewPatApptOperatories()
        {
            //No need to check RemotingRole; no call to db.
            //Get all definitions that are associated to the WebSchedNewPatApptTypes category that are linked to an operatory.
            List<Definition> listWSNPAATDefs = Definition.GetByCategory(DefinitionCategory.WebSchedNewPatApptTypes);//Cannot hide defs of this category at this time.
                                                                                                //Return all of the deflinks that are of type Operatory in order to get the operatory specific deflinks.
            return DefLinks.GetDefLinksByTypeAndDefs(DefLinkType.Operatory, listWSNPAATDefs.Select(x => x.Id).ToList());
        }

        ///<summary>Gets list of all appointment type specific DefLinks associated to the WebSchedNewPatApptTypes definition category.</summary>
        public static List<DefLink> GetDefLinksForWebSchedNewPatApptApptTypes()
        {
            //No need to check RemotingRole; no call to db.
            //Get all definitions that are associated to the WebSchedNewPatApptTypes category that are linked to an operatory.
            List<Definition> listWSNPAATDefs = Definition.GetByCategory(DefinitionCategory.WebSchedNewPatApptTypes);//Cannot hide defs of this category at this time.
                                                                                                //Return all of the deflinks that are of type Operatory in order to get the operatory specific deflinks.
            return DefLinks.GetDefLinksByTypeAndDefs(DefLinkType.AppointmentType, listWSNPAATDefs.Select(x => x.Id).ToList());
        }

        ///<summary>Gets one DefLinks by FKey. Must provide DefLinkType.  Returns null if not found.</summary>
        public static DefLink GetOneByFKey(long fKey, DefLinkType defType)
        {
            //No need to check RemotingRole; no call to db.
            return GetListByFKeys(new List<long>() { fKey }, defType).FirstOrDefault();
        }

        ///<summary>Gets list of DefLinks by FKey. Must provide DefLinkType.</summary>
        public static List<DefLink> GetListByFKey(long fKey, DefLinkType defType)
        {
            //No need to check RemotingRole; no call to db.
            return GetListByFKeys(new List<long>() { fKey }, defType);
        }

        ///<summary>Gets list of DefLinks by FKeys. Must provide DefLinkType.</summary>
        public static List<DefLink> GetListByFKeys(List<long> listFKeys, DefLinkType defType)
        {
            if (listFKeys.Count == 0)
            {
                return new List<DefLink>();
            }
            string command = "SELECT * FROM deflink WHERE FKey IN(" + string.Join(",", listFKeys.Select(x => POut.Long(x))) + ")"
                + " AND LinkType =" + POut.Int((int)defType);
            return Crud.DefLinkCrud.SelectMany(command);
        }

        ///<summary>Gets one DefLink from the db.</summary>
        public static DefLink GetOne(long defLinkNum)
        {
            return Crud.DefLinkCrud.SelectOne(defLinkNum);
        }
        #endregion

        #region Modification Methods
        #region Insert
        ///<summary></summary>
        public static long Insert(DefLink defLink)
        {
            return Crud.DefLinkCrud.Insert(defLink);
        }

        ///<summary>Inserts or updates the FKey entry for the corresponding definition passed in.
        ///This is a helper method that should only be used when there can only be a one to one relationship between DefNum and FKey.</summary>
        public static void SetFKeyForDef(long defNum, long fKey, DefLinkType linkType)
        {
            //No need to check RemotingRole; no call to db.
            //Look for the def link first to decide if we need to run an update or an insert statement.
            List<DefLink> listDefLinks = GetDefLinksByType(linkType, defNum);
            if (listDefLinks.Count > 0)
            {
                UpdateDefWithFKey(defNum, fKey, linkType);
            }
            else
            {
                Insert(new DefLink()
                {
                    DefNum = defNum,
                    FKey = fKey,
                    LinkType = linkType,
                });
            }
        }

        public static void InsertDefLinksForDefs(List<long> listDefNums, long fKey, DefLinkType linkType)
        {
            if (listDefNums == null || listDefNums.Count < 1)
            {
                return;
            }
            foreach (long defNum in listDefNums)
            {
                Insert(new DefLink()
                {
                    DefNum = defNum,
                    FKey = fKey,
                    LinkType = linkType,
                });
            }
        }
        #endregion
        #region Update
        ///<summary></summary>
        public static void Update(DefLink defLink)
        {
            Crud.DefLinkCrud.Update(defLink);
        }

        ///<summary>Updates the FKey column on all deflink rows for the corresponding definition and type.</summary>
        public static void UpdateDefWithFKey(long defNum, long fKey, DefLinkType defType)
        {
            string command = "UPDATE deflink SET FKey=" + POut.Long(fKey) + " "
                + "WHERE LinkType=" + POut.Int((int)defType) + " "
                + "AND DefNum=" + POut.Long(defNum);
            Db.NonQ(command);
        }

        ///<summary>Syncs two supplied lists of DefLink.</summary>
        public static bool Sync(List<DefLink> listNew, List<DefLink> listOld)
        {
            return Crud.DefLinkCrud.Sync(listNew, listOld);
        }
        #endregion
        #region Delete
        ///<summary></summary>
        public static void Delete(long defLinkNum)
        {
            Crud.DefLinkCrud.Delete(defLinkNum);
        }

        ///<summary>Deletes all links for the specified FKey and link type.</summary>
        public static void DeleteAllForFKeys(List<long> listFKeys, DefLinkType defType)
        {
            if (listFKeys == null || listFKeys.Count < 1)
            {
                return;
            }
            string command = "DELETE FROM deflink "
                + "WHERE LinkType=" + POut.Int((int)defType) + " "
                + "AND FKey IN(" + string.Join(",", listFKeys.Select(x => POut.Long(x))) + ")";
            Db.NonQ(command);
        }

        ///<summary>Deletes all links for the specified definition and link type.</summary>
        public static void DeleteAllForDef(long defNum, DefLinkType defType)
        {
            string command = "DELETE FROM deflink "
                + "WHERE LinkType=" + POut.Int((int)defType) + " "
                + "AND DefNum=" + POut.Long(defNum);
            Db.NonQ(command);
        }

        public static void DeleteDefLinks(List<long> listDefLinkNums)
        {
            if (listDefLinkNums == null || listDefLinkNums.Count < 1)
            {
                return;
            }
            string command = "DELETE FROM deflink WHERE DefLinkNum IN (" + string.Join(",", listDefLinkNums) + ")";
            Db.NonQ(command);
        }
        #endregion
        #endregion
    }
}