using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSupplyInventory : FormBase
    {
        List<SupplyNeeded> suppliesList;
        int pagesPrinted;
        bool headingPrinted;
        int headingPrintHeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSupplyInventory"/> class.
        /// </summary>
        public FormSupplyInventory() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormInventory_Load(object sender, EventArgs e)
        {
            categoriesButton.Enabled = Security.IsAuthorized(Permissions.Setup);
            equipmentButton.Enabled = Security.IsAuthorized(Permissions.EquipmentSetup);

            LoadSuppliesNeeded();
        }

        /// <summary>
        /// Loads the supplies needed and populates the grid.
        /// </summary>
        void LoadSuppliesNeeded()
        {
            suppliesList = SupplyNeededs.CreateObjects();

            suppliesGrid.BeginUpdate();
            suppliesGrid.Columns.Clear();
            suppliesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDateAdded, 80));
            suppliesGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnDescription, 300));
            suppliesGrid.Rows.Clear();

            for (int i = 0; i < suppliesList.Count; i++)
            {
                var row = new ODGridRow();
                row.Cells.Add(suppliesList[i].DateAdded.ToShortDateString());
                row.Cells.Add(suppliesList[i].Description);
                suppliesGrid.Rows.Add(row);
            }
            suppliesGrid.EndUpdate();
        }

        /// <summary>
        /// Opens the form the edit the needed supply.
        /// </summary>
        void SuppliesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formSupplyNeededEdit = new FormSupplyNeededEdit())
            {
                formSupplyNeededEdit.Supp = suppliesList[e.Row];
                if (formSupplyNeededEdit.ShowDialog(this) == DialogResult.OK)
                {
                    LoadSuppliesNeeded();
                }
            }
        }

        /// <summary>
        /// Opens the form to edit the supply categories.
        /// </summary>
        void CategoriesButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup)) return;

            using (var formDefinitions = new FormDefinitions(DefCat.SupplyCats))
            {
                formDefinitions.ShowDialog(this);
            }

            SecurityLogs.MakeLogEntry(Permissions.Setup, 0, Translation.LanguageSecurity.DefinitionsAccessed);
        }

        /// <summary>
        /// Opens the form to edit the suppliers.
        /// </summary>
        void SuppliersButton_Click(object sender, EventArgs e)
        {
            using (var formSuppliers = new FormSuppliers())
            {
                formSuppliers.ShowDialog();
            }
        }

        /// <summary>
        /// Opens the form to edit the supplies.
        /// </summary>
        void SuppliesButton_Click(object sender, EventArgs e)
        {
            using (var formSupplies = new FormSupplies())
            {
                formSupplies.ShowDialog();
            }
        }

        /// <summary>
        /// Opens the equipment list form.
        /// </summary>
        void EquipmentButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.EquipmentSetup)) return;

            using (var formEquipment = new FormEquipment())
            {
                formEquipment.ShowDialog();
            }  
        }

        /// <summary>
        /// Opens the order list form.
        /// </summary>
        void OrdersButton_Click(object sender, EventArgs e)
        {
            using (var formSupplyOrders = new FormSupplyOrders())
            {
                formSupplyOrders.ShowDialog();
            }
        }

        /// <summary>
        /// Opens the form to add a needed supply.
        /// </summary>
        void AddNeededButton_Click(object sender, EventArgs e)
        {
            var supplyNeeded = new SupplyNeeded
            {
                IsNew       = true,
                DateAdded   = DateTime.Today
            };

            using (var formSupplyNeededEdit = new FormSupplyNeededEdit())
            {
                formSupplyNeededEdit.Supp = supplyNeeded;
                if (formSupplyNeededEdit.ShowDialog(this) == DialogResult.OK)
                {
                    LoadSuppliesNeeded();
                }
            }
        }

        /// <summary>
        /// Prints the list of supplies needed.
        /// </summary>
        void PrintButton_Click(object sender, EventArgs e)
        {
            pagesPrinted = 0;
            headingPrinted = false;

            PrinterL.TryPrintOrDebugRpPreview(
                PrintPage,
                Translation.LanguageSecurity.SuppliesNeededListPrinted, 
                PrintoutOrientation.Portrait);
        }

        void PrintPage(object sender, PrintPageEventArgs e)
        {
            Rectangle bounds = e.MarginBounds;
            Graphics g = e.Graphics;
            string text;
            Font headingFont = new Font("Arial", 13, FontStyle.Bold);
            int yPos = bounds.Top;
            int center = bounds.X + bounds.Width / 2;

            if (!headingPrinted)
            {
                text = Translation.Language.SuppliesNeeded;
                g.DrawString(text, headingFont, Brushes.Black, center - g.MeasureString(text, headingFont).Width / 2, yPos);
                yPos += (int)g.MeasureString(text, headingFont).Height;
                yPos += 20;
                headingPrinted = true;
                headingPrintHeight = yPos;
            }

            yPos = suppliesGrid.PrintPage(g, pagesPrinted, bounds, headingPrintHeight);
            pagesPrinted++;
            if (yPos == -1)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }
            g.Dispose();
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        void CloseButton_Click(object sender, EventArgs e) => Close();
    }
}