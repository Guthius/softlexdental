using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class MapAreas
    {
        ///<summary></summary>
        public static List<MapArea> Refresh()
        {
            string command = "SELECT * FROM maparea";
            return Crud.MapAreaCrud.SelectMany(command);
        }

        ///<summary></summary>
        public static long Insert(MapArea mapArea)
        {
            return Crud.MapAreaCrud.Insert(mapArea);
        }

        ///<summary></summary>
        public static void Update(MapArea mapArea)
        {
            Crud.MapAreaCrud.Update(mapArea);
        }

        ///<summary></summary>
        public static void Delete(long mapAreaNum)
        {
            string command = "DELETE FROM maparea WHERE MapAreaNum = " + POut.Long(mapAreaNum);
            Db.NonQ(command);
        }
    }
}