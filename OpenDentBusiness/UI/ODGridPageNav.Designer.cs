using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDental.UI
{
    partial class ODGridPageNav
    {
        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.jumpToPageTextBox = new System.Windows.Forms.TextBox();
            this.panelPageLinks = new System.Windows.Forms.Panel();
            this.linkOne = new System.Windows.Forms.LinkLabel();
            this.linkTwo = new System.Windows.Forms.LinkLabel();
            this.linkThree = new System.Windows.Forms.LinkLabel();
            this.linkFour = new System.Windows.Forms.LinkLabel();
            this.firstButton = new System.Windows.Forms.Button();
            this.lastButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.previousButton = new System.Windows.Forms.Button();
            this.panelPageLinks.SuspendLayout();
            this.SuspendLayout();
            // 
            // jumpToPageTextBox
            // 
            this.jumpToPageTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.jumpToPageTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.jumpToPageTextBox.Location = new System.Drawing.Point(123, 3);
            this.jumpToPageTextBox.MaxLength = 10000;
            this.jumpToPageTextBox.Name = "jumpToPageTextBox";
            this.jumpToPageTextBox.Size = new System.Drawing.Size(34, 20);
            this.jumpToPageTextBox.TabIndex = 8;
            this.jumpToPageTextBox.Text = "3";
            this.jumpToPageTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.jumpToPageTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.jumpToPageTextBox_KeyUp);
            // 
            // panelPageLinks
            // 
            this.panelPageLinks.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panelPageLinks.Controls.Add(this.linkOne);
            this.panelPageLinks.Controls.Add(this.linkTwo);
            this.panelPageLinks.Controls.Add(this.linkThree);
            this.panelPageLinks.Controls.Add(this.linkFour);
            this.panelPageLinks.Location = new System.Drawing.Point(55, 3);
            this.panelPageLinks.Name = "panelPageLinks";
            this.panelPageLinks.Size = new System.Drawing.Size(170, 20);
            this.panelPageLinks.TabIndex = 6;
            // 
            // linkOne
            // 
            this.linkOne.Location = new System.Drawing.Point(1, 1);
            this.linkOne.Name = "linkOne";
            this.linkOne.Size = new System.Drawing.Size(33, 17);
            this.linkOne.TabIndex = 9;
            this.linkOne.TabStop = true;
            this.linkOne.Text = "1";
            this.linkOne.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkOne.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClickedEventHandler);
            // 
            // linkTwo
            // 
            this.linkTwo.Location = new System.Drawing.Point(34, 1);
            this.linkTwo.Name = "linkTwo";
            this.linkTwo.Size = new System.Drawing.Size(34, 17);
            this.linkTwo.TabIndex = 10;
            this.linkTwo.TabStop = true;
            this.linkTwo.Text = "2";
            this.linkTwo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkTwo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClickedEventHandler);
            // 
            // linkThree
            // 
            this.linkThree.Location = new System.Drawing.Point(102, 1);
            this.linkThree.Name = "linkThree";
            this.linkThree.Size = new System.Drawing.Size(34, 17);
            this.linkThree.TabIndex = 11;
            this.linkThree.TabStop = true;
            this.linkThree.Text = "4";
            this.linkThree.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkThree.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClickedEventHandler);
            // 
            // linkFour
            // 
            this.linkFour.Location = new System.Drawing.Point(136, 1);
            this.linkFour.Name = "linkFour";
            this.linkFour.Size = new System.Drawing.Size(33, 17);
            this.linkFour.TabIndex = 12;
            this.linkFour.TabStop = true;
            this.linkFour.Text = "5";
            this.linkFour.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkFour.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClickedEventHandler);
            // 
            // firstButton
            // 
            this.firstButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.firstButton.Location = new System.Drawing.Point(0, 0);
            this.firstButton.Name = "firstButton";
            this.firstButton.Size = new System.Drawing.Size(27, 26);
            this.firstButton.TabIndex = 4;
            this.firstButton.Text = "<<";
            this.firstButton.UseVisualStyleBackColor = true;
            this.firstButton.Click += new System.EventHandler(this.firstButton_Click);
            // 
            // lastButton
            // 
            this.lastButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lastButton.Location = new System.Drawing.Point(253, 0);
            this.lastButton.Name = "lastButton";
            this.lastButton.Size = new System.Drawing.Size(27, 26);
            this.lastButton.TabIndex = 9;
            this.lastButton.Text = ">>";
            this.lastButton.UseVisualStyleBackColor = true;
            this.lastButton.Click += new System.EventHandler(this.lastButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nextButton.Location = new System.Drawing.Point(227, 0);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(26, 26);
            this.nextButton.TabIndex = 10;
            this.nextButton.Text = ">";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // previousButton
            // 
            this.previousButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.previousButton.Location = new System.Drawing.Point(27, 0);
            this.previousButton.Name = "previousButton";
            this.previousButton.Size = new System.Drawing.Size(26, 26);
            this.previousButton.TabIndex = 11;
            this.previousButton.Text = "<";
            this.previousButton.UseVisualStyleBackColor = true;
            this.previousButton.Click += new System.EventHandler(this.previousButton_Click);
            // 
            // ODGridPageNav
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.previousButton);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.lastButton);
            this.Controls.Add(this.firstButton);
            this.Controls.Add(this.jumpToPageTextBox);
            this.Controls.Add(this.panelPageLinks);
            this.MinimumSize = new System.Drawing.Size(143, 26);
            this.Name = "ODGridPageNav";
            this.Size = new System.Drawing.Size(280, 26);
            this.panelPageLinks.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion Component Designer generated code

        private TextBox jumpToPageTextBox;
        private Panel panelPageLinks;
        private Button firstButton;
        private Button lastButton;
        private LinkLabel linkOne;
        private LinkLabel linkTwo;
        private LinkLabel linkThree;
        private LinkLabel linkFour;
        private Button nextButton;
        private Button previousButton;
    }
}
