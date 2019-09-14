using CodeBase;
using OpenDental.Properties;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows;

namespace OpenDental
{
    public partial class FormFriendlyException : FormBase
    {
        string friendlyMessage;
        Exception exception;
        int detailsTabControlHeight;
        int pagesPrinted;

        /// <summary>
        /// Gets the stack trace of the exception.
        /// </summary>
        public string StackTrace => stackTraceTextBox.Text.Trim();

        /// <summary>
        /// Gets the query that caused the exception.
        /// </summary>
        public string Query { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormFriendlyException"/> class.
        /// </summary>
        /// <param name="friendlyMessage"></param>
        /// <param name="exception"></param>
        /// <param name="closeButtonText"></param>
        public FormFriendlyException(string friendlyMessage, Exception exception, string closeButtonText = "")
        {
            InitializeComponent();

            this.friendlyMessage = friendlyMessage;
            this.exception = exception;

            if (!string.IsNullOrWhiteSpace(closeButtonText))
            {
                cancelButton.Text = closeButtonText;
            }

            // Append the current date to the 
            Text += " - " + DateTime.Now.ToString();
        }


        static void StringFromInnerException(StringBuilder stringBuilder, Exception ex, int depth = 0)
        {
            if (ex == null || depth == 5) return;

            if (ex.InnerException != null)
            {
                StringFromInnerException(
                    stringBuilder,
                    ex.InnerException,
                    depth + 1);
            }
        }

        static string StringFromException(Exception e, string threadName = null)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(e.GetType().FullName + ": ");
            stringBuilder.AppendLine(e.Message);
            if (!string.IsNullOrWhiteSpace(e.StackTrace))
            {
                stringBuilder.Append(e.StackTrace);
            }

            if (e.InnerException != null)
            {
                StringFromInnerException(
                    stringBuilder, 
                    e.InnerException);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormFriendlyException_Load(object sender, EventArgs e)
        {
            friendlyMessageLabel.Text = friendlyMessage;
            stackTraceTextBox.Text = StringFromException(exception);

            // Fetch the query from the exception.
            Query = ((exception as ODException)?.Query ?? "");

            //textDetails is visible by default so that it actually has height.
            ToggleDetailsTabControl();
            if (string.IsNullOrEmpty(Query))
            {
                detailsTabControl.TabPages.RemoveByKey("queryTabPage");
            }
            else
            {
                queryTextBox.Text = Query;
            }
        }

        /// <summary>
        /// Toggles visibility of the detailsTabControl.
        /// </summary>
        void detailsButton_Click(object sender, EventArgs e) => ToggleDetailsTabControl();
        
        /// <summary>
        /// A helper method that toggles visibility of the details text box and adjusts the size of the form to accomodate the UI change.
        /// </summary>
        void ToggleDetailsTabControl()
        {
            if (detailsTabControl.Visible)
            {
                detailsTabControlHeight = detailsTabControl.Height;
                detailsTabControl.Visible = false;
                detailsButton.Image = Resources.IconBulletArrowDown;
                copyAllButton.Visible = false;
                printButton.Visible = false;
                MinimumSize = new System.Drawing.Size(MinimumSize.Width, MinimumSize.Height - detailsTabControlHeight);
                Height -= detailsTabControlHeight;
            }
            else
            {
                detailsTabControl.Visible = true;
                detailsButton.Image = Resources.IconBulletArrowUp;
                copyAllButton.Visible = true;
                printButton.Visible = true;
                MinimumSize = new System.Drawing.Size(MinimumSize.Width, MinimumSize.Height + detailsTabControlHeight);
            }
        }

        /// <summary>
        /// Copies all the details of the exception to the clipboard.
        /// </summary>
        void copyAllButton_Click(object sender, EventArgs e)
        {
            try
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append(Text + "\r\n" + StackTrace);

                if (Query.Length > 0)
                {
                    stringBuilder.Append("\r\n-------------------------------------------\r\n");
                    stringBuilder.Append(Query);
                }

                Clipboard.SetData("Text", stringBuilder.ToString());
            }
            catch
            {
                MessageBox.Show(
                    "Could not copy contents to the clipboard. Please try again.", 
                    "Error", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Prints the details of the exception.
        /// </summary>
        void printButton_Click(object sender, EventArgs e)
        {
            pagesPrinted = 0;

            ODprintout.InvalidMinDefaultPageWidth = 0;

            //No print previews, since this form is in and of itself a print preview.

            PrinterL.TryPrint(pd_PrintPage, margins: new Margins(50, 50, 50, 50), duplex: Duplex.Horizontal);
        }

        ///<summary>Called for each page to be printed.</summary>
        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.HasMorePages = !Print(e.Graphics, pagesPrinted++, e.MarginBounds);
        }

        ///<summary>Prints one page. Returns true if pageToPrint is the last page in this print job.</summary>
        private bool Print(Graphics g, int pageToPrint, Rectangle margins)
        {
            //Messages may span multiple pages. We print the header on each page as well as the page number.
            float baseY = margins.Top;
            string text = "Page " + (pageToPrint + 1);
            Font font = Font;
            SizeF textSize = g.MeasureString(text, font);
            g.DrawString(text, font, Brushes.Black, margins.Right - textSize.Width, baseY);
            baseY += textSize.Height;
            text = Text;
            font = new Font(Font.FontFamily, 16, System.Drawing.FontStyle.Bold);
            textSize = g.MeasureString(text, font);
            g.DrawString(text, font, Brushes.Black, (margins.Width - textSize.Width) / 2, baseY);
            baseY += textSize.Height;
            font.Dispose();
            string[] messageLines = (stackTraceTextBox.Text + Query)
                .Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
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
        /// Displays the exception dialog with the specified details.
        /// </summary>
        /// <param name="friendlyMessage"></param>
        /// <param name="exception"></param>
        /// <param name="closeButtonText"></param>
        public static void Show(string friendlyMessage, Exception exception, string closeButtonText = "")
        {
            if (exception == null) return;

            if (friendlyMessage == null)
            {
                friendlyMessage = exception.Message;
            }
            else
            {
                friendlyMessage = friendlyMessage.Trim();
                if (friendlyMessage.Length == 0)
                {
                    friendlyMessage = exception.Message;
                }
            }

            new FormFriendlyException(friendlyMessage, exception, closeButtonText).ShowDialog();
        }
    }
}