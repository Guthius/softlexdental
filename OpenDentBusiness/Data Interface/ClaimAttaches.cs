using System;
using System.Collections;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class ClaimAttaches
    {
        public static long Insert(ClaimAttach attach)
        {
            return Crud.ClaimAttachCrud.Insert(attach);
        }
    }
}