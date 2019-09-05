using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormEhrHash : FormBase
    {
        public FormEhrHash() => InitializeComponent();

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (hashTextBox.Text.Trim() == "" || messageTextBox.Text.Trim() == "")
            {
                MessageBox.Show(
                    "Data or hash should not be blank.", 
                    "Hash", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            string attachContents = "Original message:\r\n" + messageTextBox.Text + "\r\n\r\n\r\nHash:\r\n" + hashTextBox.Text;
            Cursor = Cursors.WaitCursor;
            try
            {
                // TODO: EmailMessages.SendTestUnsecure("Hash","hash.txt",attachContents);
            }
            catch (Exception exception)
            {
                Cursor = Cursors.Default;

                MessageBox.Show(
                    exception.Message, 
                    "Hash", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            Cursor = Cursors.Default;

            MessageBox.Show(
                "Sent",
                "Hash",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.ASCII.GetBytes(messageTextBox.Text);

            using (var sha1 = SHA1.Create())
            {
                byte[] hashbytes = sha1.ComputeHash(data);
                byte digit1;
                byte digit2;
                string char1;
                string char2;
                StringBuilder strbuild = new StringBuilder();
                for (int i = 0; i < hashbytes.Length; i++)
                {
                    if (hashbytes[i] == 0)
                    {
                        digit1 = 0;
                        digit2 = 0;
                    }
                    else
                    {
                        digit1 = (byte)Math.Floor((double)hashbytes[i] / 16d);
                        //double remainder=Math.IEEERemainder((double)hashbytes[i],16d);
                        digit2 = (byte)(hashbytes[i] - (byte)(16 * digit1));
                    }
                    char1 = ByteToStr(digit1);
                    char2 = ByteToStr(digit2);
                    strbuild.Append(char1);
                    strbuild.Append(char2);
                }
            }


            //return strbuild.ToString();
            //string strHash="";
            //for(int i=0;i<hash.Length;i++) {
            //	strHash+=ByteToStr(hash[i]);
            //}
            hashTextBox.Text = strbuild.ToString();
        }

        ///<summary>The only valid input is a value between 0 and 15.  Text returned will be 1-9 or a-f.</summary>
        private static string ByteToStr(byte byteVal)
        {
            switch (byteVal)
            {
                case 10:
                    return "a";
                case 11:
                    return "b";
                case 12:
                    return "c";
                case 13:
                    return "d";
                case 14:
                    return "e";
                case 15:
                    return "f";
                default:
                    return byteVal.ToString();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e) => Close();
    }
}
