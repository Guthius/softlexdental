using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormCarrierCombine : FormBase
    {
        private List<Carrier> carrierList;
        private List<long> CarrierIds;

        /// <summary>
        /// Gets the ID of the selected carrier.
        /// </summary>
        public long SelectedCarrierId { get; private set; }

        ///<summary>Before opening this Form, set the carrierNums to show.</summary>
        

        public FormCarrierCombine(List<long> carrierIds)
        {
            InitializeComponent();

            CarrierIds = new List<long>(carrierIds);
        }

        private void FormCarrierCombine_Load(object sender, EventArgs e) => LoadCarriers();

        private void LoadCarriers()
        {
            carrierList = Carriers.GetCarriers(CarrierIds);
            tbCarriers.ResetRows(carrierList.Count);
            tbCarriers.SetGridColor(Color.Gray);
            tbCarriers.SetBackGColor(Color.White);
            for (int i = 0; i < carrierList.Count; i++)
            {
                tbCarriers.Cell[0, i] = carrierList[i].CarrierName;
                tbCarriers.Cell[1, i] = carrierList[i].Phone;
                tbCarriers.Cell[2, i] = carrierList[i].Address;
                tbCarriers.Cell[3, i] = carrierList[i].Address2;
                tbCarriers.Cell[4, i] = carrierList[i].City;
                tbCarriers.Cell[5, i] = carrierList[i].State;
                tbCarriers.Cell[6, i] = carrierList[i].Zip;
                tbCarriers.Cell[7, i] = carrierList[i].ElectID;
            }
            tbCarriers.LayoutTables();
        }

        private void tbCarriers_CellDoubleClicked(object sender, CellEventArgs e)
        {
            SelectedCarrierId = carrierList[e.Row].CarrierNum;
            DialogResult = DialogResult.OK;
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (tbCarriers.SelectedRow == -1)
            {
                MessageBox.Show(
                    "Please select an item first.", 
                    "", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }
            SelectedCarrierId = carrierList[tbCarriers.SelectedRow].CarrierNum;
            DialogResult = DialogResult.OK;
        }
    }
}