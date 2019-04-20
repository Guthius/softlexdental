using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using OpenDentBusiness;

namespace OpenDentBusiness
{
    public class Mounts
    {
        public static long Insert(Mount mount)
        {
            return Crud.MountCrud.Insert(mount);
        }

        public static void Update(Mount mount)
        {
            Crud.MountCrud.Update(mount);
        }

        public static void Delete(Mount mount)
        {
            string command = "DELETE FROM mount WHERE MountNum='" + POut.Long(mount.MountNum) + "'";
            Db.NonQ(command);
        }

        ///<summary>Returns a single mount object corresponding to the given mount number key.</summary>
        public static Mount GetByNum(long mountNum)
        {
            Mount mount = Crud.MountCrud.SelectOne(mountNum);
            if (mount == null)
            {
                return new Mount();
            }
            return mount;
        }

    }
}