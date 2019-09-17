/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSuppliers : FormBase
    {
        private List<Supplier> suppliers;

        public FormSuppliers() => InitializeComponent();

        private void FormSuppliers_Load(object sender, EventArgs e) => LoadSuppliers();

        private void LoadSuppliers()
        {
            suppliers = Suppliers.GetAll();

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

            foreach (var supplier in suppliers)
            {
                suppliersGrid.Rows.Add(
                    new ODGridRow(
                        supplier.Name,
                        supplier.Phone,
                        supplier.CustomerId,
                        supplier.Website,
                        supplier.UserName,
                        supplier.Password,
                        supplier.Note));
            }
            suppliersGrid.EndUpdate();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var supplier = new Supplier();

            using (var formSupplierEdit = new FormSupplierEdit())
            {
                formSupplierEdit.Supplier = supplier;

                if (formSupplierEdit.ShowDialog(this) == DialogResult.OK)
                {
                    LoadSuppliers();
                }
            }
        }

        private void SuppliersGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            using (var formSupplierEdit = new FormSupplierEdit())
            {
                formSupplierEdit.Supplier = suppliers[e.Row];

                if (formSupplierEdit.ShowDialog(this) == DialogResult.OK)
                {
                    LoadSuppliers();
                }
            }
        }

        private void CloseButton_Click(object sender, EventArgs e) => Close();
    }
}