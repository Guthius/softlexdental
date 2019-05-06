using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSupplyEdit : FormBase
    {
        List<Def> supplyCategoriesList;
        long categoryInitialVal;

        public Supply Supp;
        public List<Supplier> ListSupplier;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSupplyEdit"/> class.
        /// </summary>
        public FormSupplyEdit() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormSupplyEdit_Load(object sender, EventArgs e)
        {
            supplierTextBox.Text = Suppliers.GetName(ListSupplier, Supp.SupplierNum);

            supplyCategoriesList = Defs.GetDefsForCategory(DefCat.SupplyCats, true);
            for (int i = 0; i < supplyCategoriesList.Count; i++)
            {
                categoryComboBox.Items.Add(supplyCategoriesList[i].ItemName);
                if (Supp.Category == supplyCategoriesList[i].DefNum)
                {
                    categoryComboBox.SelectedIndex = i;
                }
            }

            if (categoryComboBox.SelectedIndex == -1) categoryComboBox.SelectedIndex = 0;

            categoryInitialVal = Supp.Category;
            catalogNumberTextBox.Text = Supp.CatalogNumber;
            descriptionTextBox.Text = Supp.Descript;

            if (Supp.LevelDesired != 0)
            {
                levelDesiredTextBox.Text = Supp.LevelDesired.ToString();
            }

            if (Supp.LevelOnHand != 0)
            {
                levelOnHandTextBox.Text = Supp.LevelOnHand.ToString();
            }

            if (Supp.Price != 0)
            {
                priceTextBox.Text = Supp.Price.ToString("n");
            }

            hiddenCheckBox.Checked = Supp.IsHidden;
        }

        /// <summary>
        /// Deletes the supply.
        /// </summary>
        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (Supp.IsNew)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            var result =
                MessageBox.Show(
                    Translation.Language.ConfirmDelete,
                    Translation.Language.Supply, 
                    MessageBoxButtons.OKCancel, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) ;

            try
            {
                Supplies.DeleteObject(Supp);
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    Translation.Language.Supply, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            Supp = null;
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Saves the supply and closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (levelDesiredTextBox.errorProvider1.GetError(levelDesiredTextBox) != "")
            {
                MessageBox.Show(
                    Translation.Language.PleaseFixDataEntryErrors,
                    Translation.Language.Supply,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (descriptionTextBox.Text == "")
            {
                MessageBox.Show(
                    Translation.Language.PleaseEnterADescription,
                    Translation.Language.Supply,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            Supp.Category = supplyCategoriesList[categoryComboBox.SelectedIndex].DefNum;
            Supp.CatalogNumber = catalogNumberTextBox.Text;
            Supp.Descript = descriptionTextBox.Text;
            Supp.LevelDesired = PIn.Float(levelDesiredTextBox.Text);
            Supp.LevelOnHand = PIn.Float(levelOnHandTextBox.Text);
            Supp.Price = priceTextBox.Value;
            Supp.IsHidden = hiddenCheckBox.Checked;

            if (Supp.Category != categoryInitialVal)
            {
                Supp.ItemOrder = int.MaxValue;
            }

            DialogResult = DialogResult.OK;
        }
    }
}