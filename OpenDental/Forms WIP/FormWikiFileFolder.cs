using System;
using System.IO;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormWikiFileFolder : FormBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the 
        /// </summary>
        public bool IsFolderMode { get; set; } = false;

        /// <summary>
        /// Gets or sets the selected link.
        /// </summary>
        public string SelectedLink
        {
            get => linkTextBox.Text.Trim();
            set
            {
                linkTextBox.Text = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormWikiFileFolder"/> class.
        /// </summary>
        public FormWikiFileFolder()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormWikiFileFolder_Load(object sender, EventArgs e)
        {
            if (IsFolderMode)
            {
                Text = Translation.Language.WikiInsertFolderLink;
            }
        }

        /// <summary>
        /// Opens the browse dialog to select a file or folder, depending on mode.
        /// </summary>
        void browseButton_Click(object sender, EventArgs e)
        {
            if (IsFolderMode)
            {
                using (var folderBrowserDialog = new FolderBrowserDialog())
                {
                    if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        SelectedLink = folderBrowserDialog.SelectedPath;
                    }
                }
                return;
            }

            using (var openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    SelectedLink = openFileDialog.FileName;
                }
            }
        }

        /// <summary>
        /// Checks whether a valid path has been selected and if so, closes the form.
        /// </summary>
        void acceptButton_Click(object sender, EventArgs e)
        {
            if (IsFolderMode)
            {
                if (!Directory.Exists(SelectedLink))
                {
                    var result =
                        MessageBox.Show(
                            Translation.Language.WikiFolderDoesNotExistContinueAnyway,
                            Translation.Language.WikiInsertFolderLink,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                    if (result == DialogResult.No) return;

                    // Try to create the folder.
                    try
                    {
                        Directory.CreateDirectory(SelectedLink);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            ex.Message,
                            Translation.Language.WikiInsertFolderLink,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        return;
                    }
                }
            }
            else
            {
                if (!File.Exists(SelectedLink))
                {
                    var result = 
                        MessageBox.Show(
                            Translation.Language.WikiFileDoesNotExistContinueAnyway,
                            Translation.Language.WikiInsertFileLink,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                    if (result == DialogResult.No) return;
                }
            }

            DialogResult = DialogResult.OK;
        }
    }
}