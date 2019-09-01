using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental
{
    public partial class FormDrugManufacturerEdit : ODForm
    {
        public Manufacturer DrugManufacturerCur;
        public bool IsNew;

        public FormDrugManufacturerEdit()
        {
            InitializeComponent();
            
        }

        private void FormDrugManufacturerEdit_Load(object sender, EventArgs e)
        {
            textManufacturerName.Text = DrugManufacturerCur.Name;
            textManufacturerCode.Text = DrugManufacturerCur.Code;
        }

        private void butDelete_Click(object sender, EventArgs e)
        {
            if (IsNew)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            if (!MsgBox.Show(this, MsgBoxButtons.OKCancel, "Delete?"))
            {
                return;
            }
            try
            {
                Manufacturer.Delete(DrugManufacturerCur.Id);
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            if (textManufacturerName.Text == "" || textManufacturerCode.Text == "")
            {
                MsgBox.Show(this, "Bank fields are not allowed.");
                return;
            }
            DrugManufacturerCur.Name = textManufacturerName.Text;
            DrugManufacturerCur.Code = textManufacturerCode.Text;
            if (IsNew)
            {
                if (Manufacturer.GetByCode(textManufacturerCode.Text) != null)
                {
                    MsgBox.Show(this, "Manufacturer with this code already exists.");
                    return;
                }
                Manufacturer.Insert(DrugManufacturerCur);
            }
            else
            {
                Manufacturer.Update(DrugManufacturerCur);
            }
            DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}