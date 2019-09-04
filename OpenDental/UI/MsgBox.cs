using System;
using System.Windows.Forms;

namespace OpenDental
{
    [Obsolete]
    public class MsgBox
    {
        public static void Show(object sender, string text) => Show(sender, text, "");

        public static void Show(string text) => MessageBox.Show(text);

        public static void Show(object sender, string text, string titleBarText) => MessageBox.Show(text, titleBarText);

        public static bool Show(object sender, MsgBoxButtons buttons, string question) => Show(sender, buttons, question, "");

        public static bool Show(MsgBoxButtons buttons, string question) => Show(buttons, question, "");

        public static bool Show(object sender, MsgBoxButtons buttons, string question, string titleBarText)
        {
            switch (buttons)
            {
                case MsgBoxButtons.OKCancel:
                    return MessageBox.Show(question, titleBarText, MessageBoxButtons.OKCancel) == DialogResult.OK;

                case MsgBoxButtons.YesNo:
                    return MessageBox.Show(question, titleBarText, MessageBoxButtons.YesNo) == DialogResult.Yes;

                default:
                    return false;
            }
        }

        public static bool Show(MsgBoxButtons buttons, string question, string titleBarText)
        {
            switch (buttons)
            {
                case MsgBoxButtons.OKCancel:
                    return MessageBox.Show(question, titleBarText, MessageBoxButtons.OKCancel) == DialogResult.OK;

                case MsgBoxButtons.YesNo:
                    return MessageBox.Show(question, titleBarText, MessageBoxButtons.YesNo) == DialogResult.Yes;

                default:
                    return false;
            }
        }

        public static bool Show(object sender, bool okCancel, string question)
        {
            return Show(sender, MsgBoxButtons.OKCancel, question);
        }
    }

    public enum MsgBoxButtons
    {
        OKCancel,
        YesNo
    }
}
