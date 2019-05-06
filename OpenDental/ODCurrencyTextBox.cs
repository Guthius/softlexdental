using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace OpenDental.UI
{
    public class ODCurrencyTextBox : TextBox
    {
        double value;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        const int EM_SETCUEBANNER = 0x1501;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODCurrencyTextBox"/> class.
        /// </summary>
        public ODCurrencyTextBox()
        {
        }

        /// <summary>
        /// Sets the watermark text as soon as the handle for the textbox is created.
        /// </summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            SendMessage(Handle, EM_SETCUEBANNER, 1, 0d.ToString("N2"));
        }

        /// <summary>
        /// Gets or sets the value of the textbox.
        /// </summary>
        public double Value
        {
            get => value;
            set
            {
                if (value != this.value)
                {
                    this.value = value;

                    if (value != 0)
                    {
                        Text = value.ToString("N2");
                    }
                    else
                    {
                        Text = "";
                    }
                }
            }
        }

        /// <summary>
        /// Prevent invalid characters from being entered in the textbox.
        /// </summary>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar))
            {
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.')
                {
                    e.Handled = true;
                }
                else
                {
                    if (e.KeyChar == ',' || e.KeyChar == '.')
                    {
                        e.KeyChar = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
                    }

                    var newText =
                        SelectionStart == Text.Length ?
                            Text + e.KeyChar :
                            Text.Substring(0, SelectionStart) + e.KeyChar + Text.Substring(SelectionStart);

                    if (!double.TryParse(newText, out _))
                    {
                        e.Handled = true;
                    }
                }
            }

            base.OnKeyPress(e);
        }

        /// <summary>
        /// Validates the value.
        /// </summary>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            if (!string.IsNullOrWhiteSpace(Text))
            {
                if (!double.TryParse(Text, out var result))
                {
                    e.Cancel = true;
                }
                else
                {
                    Value = result;
                }
            }
            else
            {
                Value = 0;
            }
        }
    }
}
