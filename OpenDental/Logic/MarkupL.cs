using System.Windows.Forms;

namespace OpenDental
{
    public class MarkupL
    {
        public static void AddTag(string tagStart, string tagClose, TextBox textBox)
        {
            int selectionStart = textBox.SelectionStart;

            textBox.SelectedText = tagStart + textBox.SelectedText + tagClose;
            textBox.SelectionStart = selectionStart + tagStart.Length;
            textBox.SelectionLength = 0;
        }

        public static bool ValidateMarkup(TextBox textBox, bool isForSaving, bool showMsgBox = true, bool isEmail = false) => true;
    }
}