using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDental.UI
{
    public class ODGridKeyEventArgs : EventArgs
    {
        // TODO: Make this class extend KeyEventArgs...

        private KeyEventArgs _keyEventArgs;

        public ODGridKeyEventArgs(KeyEventArgs keyEventArgs)
        {
            _keyEventArgs = keyEventArgs;
        }

        /// <summary>
        /// Gets which key was pressed.
        /// </summary>
        public KeyEventArgs KeyEventArgs
        {
            get
            {
                return _keyEventArgs;
            }
        }
    }
}