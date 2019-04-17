using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental.UI
{
    public partial class ODGridPageNav : UserControl
    {
        int currentPage;

        /// <summary>
        /// Gets or sets the <see cref="ODGrid"/> the navigator is bound to.
        /// </summary>
        [Category("Data")]
        [Description("The grid that this control will navigate through. This grid must have paging enabled.")]
        public ODGrid Grid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODGridPageNav"/> class.
        /// </summary>
        public ODGridPageNav() => InitializeComponent();

        /// <summary>
        /// Loads the control.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Grid != null)
            {
                Grid.PageChanged += PagingChangeEventHandler;
            }
        }

        /// <summary>
        /// Navigate to a page when the user clicks on a link.
        /// </summary>
        void LinkClickedEventHandler(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Link.LinkData == null)
            {
                return;
            }
            Grid.NavigateToPage((int)e.Link.LinkData);
        }

        /// <summary>
        /// Navigate to the first page.
        /// </summary>
        void firstButton_Click(object sender, EventArgs e) => Grid.NavigateToPage(-1);

        /// <summary>
        /// Navigate the the previous page.
        /// </summary>
        void previousButton_Click(object sender, EventArgs e) => Grid.NavigateToPage(currentPage - 1);

        /// <summary>
        /// Navigate to the next page.
        /// </summary>
        void nextButton_Click(object sender, EventArgs e) => Grid.NavigateToPage(currentPage + 1);

        /// <summary>
        /// Navigate to the last page.
        /// </summary>
        void lastButton_Click(object sender, EventArgs e) => Grid.NavigateToPage(999999);

        /// <summary>
        /// When the user presses a arrow key move in that direction (left or right),
        /// or if the user presses return, move the page entered in the textbox.
        /// </summary>
        void jumpToPageTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // TODO: Test this way of navigation. The left and right arrows are typically used to
            //       move the caret left or right, overriding this seems like a strange thing to do
            //       and might lead to awkward behaviour of the textbox once there are 10 or more pages.
            //       Perhaps changing this to up/down rather then left/right would be better.

            int newPage = 0;
            switch (e.KeyCode)
            {
                case Keys.Left:
                    newPage = currentPage - 1;
                    break;

                case Keys.Right:
                    newPage = currentPage + 1;
                    break;

                case Keys.Enter:
                    if (!int.TryParse(jumpToPageTextBox.Text, out newPage))
                    {
                        newPage = 0;
                    }
                    break;

                default: return;
            }
            Grid.NavigateToPage(newPage);
        }

        void PagingChangeEventHandler(object sender, ODGridPageEventArgs e)
        {
            currentPage = e.PageCur;
            jumpToPageTextBox.Text = currentPage.ToString();

            // TODO: It seems strange to rely on ODGrid to pass a array of values for the link label
            //       back through the ODGridPageEventArgs. It would be better to update ODGrid to expose
            //       a 'PageCount' property and use that to determine the values for the links... As a bonus
            //       we can update lastButton_Click to navigate to the correct page when we know exactly how
            //       many pages there are and we can set the enabled state of the navigation buttons 
            //       (e.g. disable firstButton when on page 1, disable lastButton when on the last page, etc...)

            // We reuse the same controls and just change their text and data to avoid flicker in UI.
            for (int i = 0; i < panelPageLinks.Controls.Count; i++)
            {
                if (panelPageLinks.Controls[i] is LinkLabel linkLabel)
                {
                    var pageNumber = e.ListLinkVals[i];
                }

                var pageLink = (LinkLabel)panelPageLinks.Controls[i];
                int pageLinkVal = e.ListLinkVals[i];
                if (pageLinkVal == -1)
                {
                    pageLink.Visible = false;
                    continue;
                }
                pageLink.Visible = true;
                pageLink.Text = pageLinkVal.ToString();
                pageLink.Links[0].LinkData = pageLinkVal;
            }
        }
    }
}