using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormCountyEdit : FormBase
    {
        public bool IsNew;

        public County County { get; set; }

        public FormCountyEdit() => InitializeComponent();

        void FormCountyEdit_Load(object sender, EventArgs e)
        {
            nameTextBox.Text = County?.CountyName;
            codeTextBox.Text = County?.CountyCode;
        }

        void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (nameTextBox.Text.Length == 1)
            {
                nameTextBox.Text = nameTextBox.Text.ToUpper();
                nameTextBox.SelectionStart = 1;
            }
        }

        void AceptButton_Click(object sender, System.EventArgs e)
        {
            var countryName = nameTextBox.Text.Trim();
            if (IsNew)
            {
                if (Counties.DoesExist(countryName))
                {
                    MessageBox.Show(
                        "County name already exists. Duplicate not allowed.",
                        "Country",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }
            }
            else
            {
                if (County.CountyName != countryName)
                {
                    if (Counties.DoesExist(County.CountyName))
                    {
                        MessageBox.Show(
                            "County name already exists. Duplicate not allowed.",
                            "Country",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        return;
                    }
                }
            }

            County.CountyName = countryName;
            County.CountyCode = codeTextBox.Text;

            if (IsNew)
            {
                Counties.Insert(County);
            }
            else
            {
                Counties.Update(County);
            }

            DialogResult = DialogResult.OK;
        }
    }
}