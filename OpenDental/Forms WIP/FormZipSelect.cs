using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormZipSelect : FormBase
    {
        bool changed;
        readonly string zipCodeDigits;
        List<ZipCode> zipCodesList;

        /// <summary>
        /// Gets the selected zip code.
        /// </summary>
        public ZipCode ZipSelected
        {
            get
            {
                if (matchesListBox.SelectedIndex == -1) return null;
                else
                {
                    return zipCodesList[matchesListBox.SelectedIndex];
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormZipSelect"/> class.
        /// </summary>
        public FormZipSelect() => InitializeComponent();

        /// <summary>
        /// Initializes a new instance of the <see cref="FormZipSelect"/> class.
        /// </summary>
        /// <param name="zipCodeDigits"></param>
        public FormZipSelect(string zipCodeDigits) : this()
        {
            this.zipCodeDigits = zipCodeDigits;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FormZipSelect_Load(object sender, EventArgs e) => LoadZipCodes();
        
        /// <summary>
        /// Invalidates the zip codes cache if changes were made.
        /// </summary>
        void FormZipSelect_Closing(object sender, CancelEventArgs e)
        {
            if (changed)
            {
                DataValid.SetInvalid(InvalidType.ZipCodes);
            }
        }

        /// <summary>
        /// Loads the zip codes and populates the listbox.
        /// </summary>
        void LoadZipCodes()
        {
            matchesListBox.Items.Clear();

            zipCodesList = ZipCodes.GetDeepCopy();
            if (!string.IsNullOrWhiteSpace(zipCodeDigits))
            {
                zipCodesList.RemoveAll(x => x.ZipCodeDigits != zipCodeDigits);
            }

            for (int i = 0; i < zipCodesList.Count; i++)
            {
                var itemText = zipCodesList[i].City + " " + zipCodesList[i].State;
                if (zipCodesList[i].IsFrequent)
                {
                    itemText += " (freq)";
                }
                matchesListBox.Items.Add(itemText);
            }

            matchesListBox.SelectedIndex = -1;
        }

        void matchesListBox_DoubleClick(object sender, EventArgs e)
        {
            if (matchesListBox.SelectedIndex == -1) return;

            acceptButton_Click(sender, e);
        }

        /// <summary>
        /// Opens the form to add a new zip code.
        /// </summary>
        void addButton_Click(object sender, EventArgs e)
        {
            using (var formZipCodeEdit = new FormZipCodeEdit())
            {
                formZipCodeEdit.ZipCodeCur = new ZipCode
                {
                    ZipCodeDigits = zipCodesList[0].ZipCodeDigits
                };
                formZipCodeEdit.IsNew = true;

                if (formZipCodeEdit.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                changed = true;

                ZipCodes.RefreshCache();
                ZipCodes.GetALMatches(formZipCodeEdit.ZipCodeCur.ZipCodeDigits);

                LoadZipCodes();
            }
        }

        /// <summary>
        /// Opens the form to edit the selected zip code.
        /// </summary>
        void editButton_Click(object sender, EventArgs e)
        {
            if (matchesListBox.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select an item first.",
                    "Select Zipcode",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            using (var formZipCodeEdit = new FormZipCodeEdit())
            {
                formZipCodeEdit.ZipCodeCur = zipCodesList[matchesListBox.SelectedIndex];
                if (formZipCodeEdit.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                changed = true;

                ZipCodes.RefreshCache();
                ZipCodes.GetALMatches(formZipCodeEdit.ZipCodeCur.ZipCodeDigits);

                LoadZipCodes();
            }
        }

        /// <summary>
        /// Deletes the selected zip code.
        /// </summary>
        void deleteButton_Click(object sender, EventArgs e)
        {
            if (matchesListBox.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select an item first.",
                    "Select Zipcode",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            ZipCode ZipCodeCur = zipCodesList[matchesListBox.SelectedIndex];
            ZipCodes.Delete(ZipCodeCur);

            changed = true;

            ZipCodes.RefreshCache();

            LoadZipCodes();
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        void acceptButton_Click(object sender, EventArgs e)
        {
            if (matchesListBox.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select an item first.",
                    "Select Zipcode", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}