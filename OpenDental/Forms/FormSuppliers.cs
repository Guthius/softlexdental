using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSuppliers : FormBase
    {
        List<Supplier> suppliersList;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSuppliers"/> class.
        /// </summary>
        public FormSuppliers() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormSuppliers_Load(object sender, EventArgs e) => LoadSuppliers();

        /// <summary>
        /// Loads the list of suppliers and populates the grid.
        /// </summary>
        void LoadSuppliers()
        {
            suppliersList = Suppliers.GetAll();

            suppliersGrid.BeginUpdate();
            suppliersGrid.Columns.Clear();
            suppliersGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnName, 110));
            suppliersGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnPhone, 90));
            suppliersGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnCustomerID, 80));
            suppliersGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnWebsite, 180));
            suppliersGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnUserName, 80));
            suppliersGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnPassword, 80));
            suppliersGrid.Columns.Add(new ODGridColumn(Translation.Language.ColumnNote, 150));
            suppliersGrid.Rows.Clear();

            for (int i = 0; i < suppliersList.Count; i++)
            {
                var row = new ODGridRow();
                row.Cells.Add(suppliersList[i].Name);
                row.Cells.Add(suppliersList[i].Phone);
                row.Cells.Add(suppliersList[i].CustomerId);
                row.Cells.Add(suppliersList[i].Website);
                row.Cells.Add(suppliersList[i].UserName);
                row.Cells.Add(suppliersList[i].Password);
                row.Cells.Add(suppliersList[i].Note);
                suppliersGrid.Rows.Add(row);
            }
            suppliersGrid.EndUpdate();
        }

        /// <summary>
        /// Opens the form to add a new supplier.
        /// </summary>
        void AddButton_Click(object sender, EventArgs e)
        {
            var supplier = new Supplier();

            using (var formSupplierEdit = new FormSupplierEdit())
            {
                formSupplierEdit.Supp = supplier;
                if (formSupplierEdit.ShowDialog(this) == DialogResult.OK)
                {
                    LoadSuppliers();
                }
            }
        }

        /// <summary>
        /// Opens the form to edit a supplier when the user double clicks on a supplier in the list.
        /// </summary>
        void SuppliersGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formSupplierEdit = new FormSupplierEdit())
            {
                formSupplierEdit.Supp = suppliersList[e.Row];
                if (formSupplierEdit.ShowDialog(this) == DialogResult.OK)
                {
                    LoadSuppliers();
                }
            }
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        void CloseButton_Click(object sender, EventArgs e) => Close();
    }
}