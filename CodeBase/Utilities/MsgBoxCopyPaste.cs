using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace CodeBase
{
    public partial class MsgBoxCopyPaste : Form
    {
        protected int pagesPrinted;

        /// <summary>
        /// Gets or sets the content of the form.
        /// </summary>
        public string Content
        {
            get => mainTextBox.Text.Trim();
            set
            {
                mainTextBox.Text = value;
            }
        }

        /// <summary>
        /// This presents a message box to the user, but is better because it allows us to copy the text and paste it into another program for testing.
        /// Especially useful for queries.
        /// </summary>
        public MsgBoxCopyPaste(string displayText)
        {
            InitializeComponent();

            Content = displayText;
        }

        /// <summary>
        /// Print the contents of the form.
        /// </summary>
        void printButton_Click(object sender, EventArgs e)
        {
            //TODO: Implement ODprintout pattern

            pagesPrinted = 0;

            var printDocument = new PrintDocument();

            printDocument.PrintPage += (s, pge) => pge.HasMorePages = !Print(pge.Graphics, pagesPrinted++, pge.MarginBounds);
            printDocument.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);
            if (printDocument.DefaultPageSettings.PrintableArea.Height == 0)
            {
                printDocument.DefaultPageSettings.PaperSize = new PaperSize("default", 850, 1100);
            }
            printDocument.PrinterSettings.Duplex = Duplex.Horizontal;
            printDocument.Print();
        }

        /// <summary>
        /// Prints one page. Returns true if pageToPrint is the last page in this print job.
        /// </summary>
        bool Print(Graphics g, int pageToPrint, Rectangle margins)
        {
            //Messages may span multiple pages. We print the header on each page as well as the page number.
            float baseY = margins.Top;
            string text = "Page " + (pageToPrint + 1);
            Font font = Font;
            SizeF textSize = g.MeasureString(text, font);
            g.DrawString(text, font, Brushes.Black, margins.Right - textSize.Width, baseY);
            baseY += textSize.Height;
            text = Text;
            font = new Font(Font.FontFamily, 16, FontStyle.Bold);
            textSize = g.MeasureString(text, font);
            g.DrawString(text, font, Brushes.Black, (margins.Width - textSize.Width) / 2, baseY);
            baseY += textSize.Height;
            font.Dispose();
            font = new Font(Font.FontFamily, 14, FontStyle.Bold);
            text = DateTime.Now.ToString();
            textSize = g.MeasureString(text, font);
            g.DrawString(text, font, Brushes.Black, (margins.Width - textSize.Width) / 2, baseY);
            baseY += textSize.Height;
            font.Dispose();
            string[] messageLines = mainTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            font = Font;
            bool isLastPage = false;
            float y = 0;
            for (int curPage = 0, msgLine = 0; curPage <= pageToPrint; curPage++)
            {
                //Set y to its initial value for the current page (right after the header).
                y = curPage * (margins.Bottom - baseY);
                while (msgLine < messageLines.Length)
                {
                    //If a line is blank, we need to make sure that is counts for some vertical space.
                    if (messageLines[msgLine] == "")
                    {
                        textSize = g.MeasureString("A", font);
                    }
                    else
                    {
                        textSize = g.MeasureString(messageLines[msgLine], font);
                    }
                    //Would the current text line go past the bottom margin?
                    if (y + textSize.Height > (curPage + 1) * (margins.Bottom - baseY))
                    {
                        break;
                    }
                    if (curPage == pageToPrint)
                    {
                        g.DrawString(messageLines[msgLine], font, Brushes.Black, margins.Left, baseY + y - curPage * (margins.Bottom - baseY));
                        if (msgLine == messageLines.Length - 1)
                        {
                            isLastPage = true;
                        }
                    }
                    y += textSize.Height;
                    msgLine++;
                }
            }
            return isLastPage;
        }

        /// <summary>
        /// Copies all the text to the clipboard.
        /// </summary>
        void copyAllButton_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetData("Text", Content);
            }
            catch
            {
                MessageBox.Show(
                    "Could not copy contents to the clipboard. Please try again.", 
                    "Open Dental", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }
    }
}