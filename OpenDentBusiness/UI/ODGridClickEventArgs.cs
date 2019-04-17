using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDental.UI
{
    public class ODGridClickEventArgs: EventArgs
    {
        public ODGridClickEventArgs(int col, int row, MouseButtons button)
        {
            Col = col;
            Row = row;
            Button = button;
        }

        public int Row { get; }

        public int Col { get; }

        public MouseButtons Button { get; }
    }
}