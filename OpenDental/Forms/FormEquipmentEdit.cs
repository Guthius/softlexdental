using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormEquipmentEdit : FormBase
    {
        public Equipment Equip;
        public bool IsNew;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormEquipmentEdit"/> class.
        /// </summary>
        public FormEquipmentEdit() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormEquipmentEdit_Load(object sender, EventArgs e)
        {
            dateEntryTextBox.Text = Equip.DateEntry.ToShortDateString();
            descriptionTextBox.Text = Equip.Description;
            serialNumberTextBox.Text = Equip.SerialNumber;
            modelYearTextBox.Text = Equip.ModelYear;

            if (Equip.DatePurchased.Year > 1880)
            {
                datePurchasedTextBox.Text = Equip.DatePurchased.ToShortDateString();
            }

            if (Equip.DateSold.Year > 1880)
            {
                dateSoldTextBox.Text = Equip.DateSold.ToShortDateString();
            }

            if (Equip.PurchaseCost > 0)
            {
                purchaseCostTextBox.Text = Equip.PurchaseCost.ToString("N2");
            }

            if (Equip.MarketValue > 0)
            {
                marketValueTextBox.Text = Equip.MarketValue.ToString("N2");
            }

            locationTextBox.Text = Equip.Location;
            statusTextBox.Text = Equip.Status;
        }

        /// <summary>
        /// Generates a serial number for the equipment.
        /// </summary>
        void GenerateButton_Click(object sender, EventArgs e)
        {
            Equip.SerialNumber = Equipments.GenerateSerialNum();
            serialNumberTextBox.Text = Equip.SerialNumber;
        }

        /// <summary>
        /// Deletes the equipment.
        /// </summary>
        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (IsNew)
            {
                DialogResult = DialogResult.Cancel;
            }

            if (!Security.IsAuthorized(Permissions.EquipmentDelete, Equip.DateEntry))  return;

            var result =
                MessageBox.Show(
                    Translation.Language.ConfirmDelete,
                    Translation.Language.Equipment, 
                    MessageBoxButtons.OKCancel, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            try
            {
                Equipments.Delete(Equip);
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    Translation.Language.Equipment, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Saves the equipment and closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (datePurchasedTextBox.errorProvider1.GetError(datePurchasedTextBox) != "" ||
                dateSoldTextBox.errorProvider1.GetError(dateSoldTextBox) != "" ||
                purchaseCostTextBox.errorProvider1.GetError(purchaseCostTextBox) != "" ||
                marketValueTextBox.errorProvider1.GetError(marketValueTextBox) != "")
            {
                MessageBox.Show(
                    Translation.Language.PleaseFixDataEntryErrors,
                    Translation.Language.Equipment,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (descriptionTextBox.Text == "")
            {
                MessageBox.Show(
                    Translation.Language.PleaseEnterADescription,
                    Translation.Language.Equipment,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (datePurchasedTextBox.Text == "")
            {
                MessageBox.Show(
                    Translation.Language.PleaseEnterDatePurchased,
                    Translation.Language.Equipment,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (PIn.Date(datePurchasedTextBox.Text) > DateTime.Today)
            {
                var result =
                    MessageBox.Show(
                        Translation.Language.DateIsInFutureContinueAnyway,
                        Translation.Language.Equipment,
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question);

                if (result == DialogResult.Cancel) return;
            }

            Equip.Description = descriptionTextBox.Text;
            Equip.SerialNumber = serialNumberTextBox.Text;
            Equip.ModelYear = modelYearTextBox.Text;
            Equip.DatePurchased = PIn.Date(datePurchasedTextBox.Text);
            Equip.DateSold = PIn.Date(dateSoldTextBox.Text);
            Equip.PurchaseCost = PIn.Double(purchaseCostTextBox.Text);
            Equip.MarketValue = PIn.Double(marketValueTextBox.Text);
            Equip.Location = locationTextBox.Text;
            Equip.Status = statusTextBox.Text;

            // TODO: This seems a bit dodgy. What if the user doesn't change the serial and cancels out this form? Then we'll potentially have filled Equip with bad data...
            //       This check needs to take place before we make modifications to Equip...
            if (!string.IsNullOrEmpty(serialNumberTextBox.Text) && Equipments.HasExisting(Equip))
            {
                MessageBox.Show(
                    Translation.Language.EquipmentSerialNumberIsAlreadInUse,
                    Translation.Language.Equipment,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (IsNew)
            {
                Equipments.Insert(Equip);
            }
            else
            {
                Equipments.Update(Equip);
            }

            DialogResult = DialogResult.OK;
        }
    }
}