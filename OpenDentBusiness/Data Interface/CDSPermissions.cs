using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class CDSPermissions
    {
        //TODO: implement caching;

        public static CDSPermission GetForUser(long usernum)
        {
            string command = "SELECT * FROM cdspermission WHERE UserNum=" + POut.Long(usernum);
            CDSPermission retval = Crud.CDSPermissionCrud.SelectOne(command);
            if (retval != null)
            {
                return retval;
            }
            return new CDSPermission();//return new CDS permission that has no permissions granted.
        }

        ///<summary></summary>
        public static List<CDSPermission> GetAll()
        {
            InsertMissingValues();
            string command = "SELECT * FROM cdspermission";
            return Crud.CDSPermissionCrud.SelectMany(command);
        }

        ///<summary>Inserts one row per UserOD if they do not have one already.</summary>
        private static void InsertMissingValues()
        {
            //No need to check RemotingRole; private static.
            string command = "SELECT * FROM userod WHERE IsHidden=0 AND UserNum NOT IN (SELECT UserNum from cdsPermission)";
            List<Userod> uods = Crud.UserodCrud.SelectMany(command);
            CDSPermission cdsp;
            for (int i = 0; i < uods.Count; i++)
            {
                cdsp = new CDSPermission();
                cdsp.UserNum = uods[i].UserNum;
                CDSPermissions.Insert(cdsp);
            }
            return;
        }

        ///<summary></summary>
        public static long Insert(CDSPermission cDSPermission)
        {
            return Crud.CDSPermissionCrud.Insert(cDSPermission);
        }

        ///<summary></summary>
        public static void Update(CDSPermission cDSPermission)
        {
            Crud.CDSPermissionCrud.Update(cDSPermission);
        }
    }
}