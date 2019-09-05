using System.Collections.Generic;

namespace OpenDentBusiness
{
    public class Cache
    {
        public static void Refresh(params InvalidType[] arrayITypes)
        {
        }

        public static void Refresh(bool doRefreshServerCache, params InvalidType[] arrayITypes)
        {
        }

        public static List<InvalidType> GetAllCachedInvalidTypes() => new List<InvalidType>();
    }
}
