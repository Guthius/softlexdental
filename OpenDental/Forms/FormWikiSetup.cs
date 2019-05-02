using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormWikiSetup : FormBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormWikiSetup"/> class.
        /// </summary>
        public FormWikiSetup() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormWikiSetup_Load(object sender, EventArgs e)
        {
            textMaster.Text = WikiPages.MasterPage.PageContent;
            detectLinksCheckBox.Checked = Preferences.GetBool(PrefName.WikiDetectLinks);
            createPageFromLinksCheckBox.Checked = Preferences.GetBool(PrefName.WikiCreatePageFromLink);
        }

        /// <summary>
        /// Updates the wiki settings and closes the form.
        /// </summary>
        void acceptButton_Click(object sender, EventArgs e)
        {
            if (Prefs.UpdateBool(PrefName.WikiDetectLinks, detectLinksCheckBox.Checked) | 
                Prefs.UpdateBool(PrefName.WikiCreatePageFromLink, createPageFromLinksCheckBox.Checked))
            {
                DataValid.SetInvalid(InvalidType.Prefs);
            }

            var masterPage = WikiPages.MasterPage;
            masterPage.PageContent = textMaster.Text;
            masterPage.UserNum = Security.CurUser.UserNum;

            WikiPages.InsertAndArchive(masterPage);
            DataValid.SetInvalid(InvalidType.Wiki);

            DialogResult = DialogResult.OK;
        }
    }
}
