using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormEquipment : FormBase
    {
        List<Equipment> equipmentList;
        int pagesPrinted;
        bool headingPrinted;
        int headingPrintHeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormEquipment"/> class.
        /// </summary>
        public FormEquipment() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormEquipment_Load(object sender, EventArgs e) => LoadEquipment();
        
        /// <summary>
        /// Reloads the equipment list using the current filter criteria.
        /// </summary>
        void SearchTextBox_TextChanged(object sender, EventArgs e) => LoadEquipment();

        /// <summary>
        /// Refreshes the equipment list.
        /// </summary>
        void RefreshButton_Click(object sender, EventArgs e)
        {
            if (dateStartTextBox.errorProvider1.GetError(dateStartTextBox) != "" ||
                dateEndTextBox.errorProvider1.GetError(dateEndTextBox) != "")
            {
                MessageBox.Show(
                    Translation.Language.InvalidDate,
                    Translation.Language.Equipment, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }
            LoadEquipment();
        }

        /// <summary>
        /// Reloads the equipment list using the current filter criteria.
        /// </summary>
        void radioPurchased_Click(object sender, EventArgs e) => LoadEquipment();

        /// <summary>
        /// Reloads the equipment list using the current filter criteria.
        /// </summary>
        void radioSold_Click(object sender, EventArgs e) => LoadEquipment();

        /// <summary>
        /// Reloads the equipment list using the current filter criteria.
        /// </summary>
        void radioAll_Click(object sender, EventArgs e) => LoadEquipment();

        /// <summary>
        /// Loads the list of equipment and populates the grid.
        /// </summary>
        void LoadEquipment()
        {
            if (dateStartTextBox.errorProvider1.GetError(dateStartTextBox) != "" || 
                dateEndTextBox.errorProvider1.GetError(dateEndTextBox) != "")
            {
                return;
            }

            DateTime fromDate;
            DateTime toDate;
            if (dateStartTextBox.Text == "")
            {
                fromDate = DateTime.MinValue.AddDays(1);
            }
            else
            {
                fromDate = PIn.Date(dateStartTextBox.Text);
            }

            if (dateEndTextBox.Text == "")
            {
                toDate = DateTime.MaxValue;
            }
            else
            {
                toDate = PIn.Date(dateEndTextBox.Text);
            }

            var displayMode = EnumEquipmentDisplayMode.All;
            if (purchasedRadioButton.Checked)
            {
                displayMode = EnumEquipmentDisplayMode.Purchased;
            }
            if (soldRadioButton.Checked)
            {
                displayMode = EnumEquipmentDisplayMode.Sold;
            }

            equipmentList = Equipments.GetList(fromDate, toDate, displayMode, searchTextBox.Text);

            equipmentGrid.BeginUpdate();
            if (purchasedRadioButton.Checked)
            {
                equipmentGrid.HScrollVisible = true;
            }
            else
            {
                equipmentGrid.HScrollVisible = false;
            }

            equipmentGrid.Columns.Clear();
            equipmentGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDescription, 150));
            equipmentGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnSerialNumber, 90));
            equipmentGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnYr, 40));
            equipmentGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDatePurchased, 90));
            if (displayMode != EnumEquipmentDisplayMode.Purchased) // Purchased mode is designed for submission to tax authority, only certain columns
            {
                equipmentGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDateSold, 90));
            }
            equipmentGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnCost, 80, HorizontalAlignment.Right));
            equipmentGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnEstValue, 80, HorizontalAlignment.Right));
            if (displayMode != EnumEquipmentDisplayMode.Purchased)
            {
                equipmentGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnLocation, 80));
            }
            equipmentGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnStatus, 160));
            equipmentGrid.Rows.Clear();

            for (int i = 0; i < equipmentList.Count; i++)
            {
                var row = new ODGridRow();
                row.Cells.Add(equipmentList[i].Description);
                row.Cells.Add(equipmentList[i].SerialNumber);
                row.Cells.Add(equipmentList[i].ModelYear);
                row.Cells.Add(equipmentList[i].DatePurchased.ToShortDateString());
                if (displayMode != EnumEquipmentDisplayMode.Purchased)
                {
                    if (equipmentList[i].DateSold.Year < 1880)
                    {
                        row.Cells.Add("");
                    }
                    else
                    {
                        row.Cells.Add(equipmentList[i].DateSold.ToShortDateString());
                    }
                }
                row.Cells.Add(equipmentList[i].PurchaseCost.ToString("F"));
                row.Cells.Add(equipmentList[i].MarketValue.ToString("F"));
                if (displayMode != EnumEquipmentDisplayMode.Purchased)
                {
                    row.Cells.Add(equipmentList[i].Location);
                }
                row.Cells.Add(equipmentList[i].Status.ToString());
                equipmentGrid.Rows.Add(row);
            }
            equipmentGrid.EndUpdate();
        }

        /// <summary>
        /// Opens the form to add new equipment.
        /// </summary>
        void AddButton_Click(object sender, EventArgs e)
        {
            var equipment = new Equipment
            {
                SerialNumber    = Equipments.GenerateSerialNum(),
                DateEntry       = DateTime.Today,
                DatePurchased   = DateTime.Today
            };

            using (var formEquipmentEdit = new FormEquipmentEdit())
            {
                formEquipmentEdit.IsNew = true;
                formEquipmentEdit.Equip = equipment;

                if (formEquipmentEdit.ShowDialog(this) == DialogResult.OK)
                {
                    LoadEquipment();
                }
            }
        }

        /// <summary>
        /// Opens the form to edit equipment when the user double clicks on equipment in the grid.
        /// </summary>
        void EquipmentGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formEquipmentEdit = new FormEquipmentEdit())
            {
                formEquipmentEdit.Equip = equipmentList[e.Row];
                if (formEquipmentEdit.ShowDialog(this) == DialogResult.OK)
                {
                    LoadEquipment();
                }
            }
        }

        /// <summary>
        /// Prints the equipment list.
        /// </summary>
        void PrintButton_Click(object sender, EventArgs e)
        {
            pagesPrinted = 0;
            headingPrinted = false;

            PrinterL.TryPrintOrDebugRpPreview(
                PrintPage,
                Translation.LanguageSecurity.EquipmentListPrinted);
        }

        void PrintPage(object sender, PrintPageEventArgs e)
        {
            Rectangle bounds = e.MarginBounds;
            Graphics g = e.Graphics;
            string text;
            Font headingFont = new Font("Arial", 13, FontStyle.Bold);
            Font subHeadingFont = new Font("Arial", 10, FontStyle.Bold);
            int yPos = bounds.Top;
            int center = bounds.X + bounds.Width / 2;
            if (!headingPrinted)
            {
                text = Translation.Language.EquipmentList;
                if (purchasedRadioButton.Checked)
                {
                    text += " - " + Translation.Language.Purchased;
                }
                if (soldRadioButton.Checked)
                {
                    text += " - " + Translation.Language.Sold;
                }
                if (allRadioButton.Checked)
                {
                    text += " - " + Translation.Language.All;
                }
                g.DrawString(text, headingFont, Brushes.Black, center - g.MeasureString(text, headingFont).Width / 2, yPos);
                yPos += (int)g.MeasureString(text, headingFont).Height;
                text = string.Format(Translation.Language.DateRangeFromTo, dateStartTextBox.Text, dateEndTextBox.Text);
                g.DrawString(text, subHeadingFont, Brushes.Black, center - g.MeasureString(text, subHeadingFont).Width / 2, yPos);
                yPos += 20;
                headingPrinted = true;
                headingPrintHeight = yPos;
            }
            yPos = equipmentGrid.PrintPage(g, pagesPrinted, bounds, headingPrintHeight);
            pagesPrinted++;
            if (yPos == -1)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
                double total = 0;
                for (int i = 0; i < equipmentList.Count; i++)
                {
                    total += equipmentList[i].MarketValue;
                }
                g.DrawString(Translation.Language.TotalEstValue + ": " + total.ToString("N2"), Font, Brushes.Black, 550, yPos);
            }
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        void CloseButton_Click(object sender, EventArgs e) => Close();
    }
}