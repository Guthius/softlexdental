using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDental.UI
{
    public class ODGridPageEventArgs : EventArgs
    {
        public int PageCur;
        public List<int> ListLinkVals;

        public ODGridPageEventArgs(int pageCur, List<int> listLinkVals)
        {
            this.PageCur = pageCur;
            this.ListLinkVals = listLinkVals;
        }
    }
}
