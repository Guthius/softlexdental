using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class HL7ProcAttaches
    {
        ///<summary></summary>
        public static long Insert(HL7ProcAttach hL7ProcAttach)
        {
            return Crud.HL7ProcAttachCrud.Insert(hL7ProcAttach);
        }
    }
}