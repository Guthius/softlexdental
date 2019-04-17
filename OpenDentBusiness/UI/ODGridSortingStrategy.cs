using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDental.UI
{
    public enum ODGridSortingStrategy
    {
        StringCompare,
        DateParse,
        ToothNumberParse,
        AmountParse,
        TimeParse,
        VersionNumber,
    }
}