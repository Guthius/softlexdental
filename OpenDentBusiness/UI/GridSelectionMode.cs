using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDental.UI
{
    /// <summary>
    /// Specifies the selection behavior of an ODGrid.
    /// </summary>
    public enum GridSelectionMode
    {
        /// <summary>
        /// 0-No items can be selected.
        /// </summary>  
        None,

        /// <summary>
        /// 1-Only one row can be selected.
        /// </summary>  
        One,

        /// <summary>
        /// 2-Only one cell can be selected.
        /// </summary>
        OneCell,

        /// <summary>
        /// 3-Multiple items can be selected, and the user can use the SHIFT, CTRL, and arrow keys to make selections
        /// </summary>   
        MultiExtended,
    }
}
