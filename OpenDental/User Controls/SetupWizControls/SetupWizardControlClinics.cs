using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental.User_Controls.SetupWizard
{
    [ToolboxItem(false)]
    public partial class SetupWizardControlClinics : SetupWizardControl
    {
        public SetupWizardControlClinics() => InitializeComponent();

        void SetupWizardControlClinics_Load(object sender, EventArgs e)
        {
            LoadClinics();
            //if (Clinics.GetCount(true) == 0)
            //{
            //    MessageBox.Show("You have no valid clinics. Please click the Add button to add a clinic.");
            //}
        }

        void LoadClinics()
        {
            Color needsAttnCol = OpenDental.SetupWizard.GetColor(ODSetupStatus.NeedsAttention);
            clinicsGrid.BeginUpdate();
            clinicsGrid.Columns.Clear();

            clinicsGrid.Columns.Add(new ODGridColumn("Clinic", 110));
            clinicsGrid.Columns.Add(new ODGridColumn("Abbrev", 70));
            clinicsGrid.Columns.Add(new ODGridColumn("Phone", 100));
            clinicsGrid.Columns.Add(new ODGridColumn("Address", 120));
            clinicsGrid.Columns.Add(new ODGridColumn("City", 90));
            clinicsGrid.Columns.Add(new ODGridColumn("State", 50));
            clinicsGrid.Columns.Add(new ODGridColumn("ZIP", 80));
            clinicsGrid.Columns.Add(new ODGridColumn("Default Prov", 75));
            clinicsGrid.Columns.Add(new ODGridColumn("Hidden", 55, HorizontalAlignment.Center));
            clinicsGrid.Rows.Clear();

            bool IsAllComplete = true;

            var clinicsList = Clinic.All().ToList();
            if (clinicsList.Count == 0)
            {
                IsAllComplete = false;
            }

            foreach (Clinic clinCur in clinicsList)
            {
                var row = new ODGridRow();
                row.Cells.Add(clinCur.Description);
                if (!clinCur.IsHidden && string.IsNullOrEmpty(clinCur.Description))
                {
                    row.Cells[row.Cells.Count - 1].CellColor = needsAttnCol;
                    IsAllComplete = false;
                }
                row.Cells.Add(clinCur.Abbr);
                if (!clinCur.IsHidden && string.IsNullOrEmpty(clinCur.Abbr))
                {
                    row.Cells[row.Cells.Count - 1].CellColor = needsAttnCol;
                    IsAllComplete = false;
                }
                row.Cells.Add(TelephoneNumbers.FormatNumbersExactTen(clinCur.Phone));
                if (!clinCur.IsHidden && string.IsNullOrEmpty(clinCur.Phone))
                {
                    row.Cells[row.Cells.Count - 1].CellColor = needsAttnCol;
                    IsAllComplete = false;
                }
                row.Cells.Add(clinCur.AddressLine1);
                if (!clinCur.IsHidden && string.IsNullOrEmpty(clinCur.AddressLine1))
                {
                    row.Cells[row.Cells.Count - 1].CellColor = needsAttnCol;
                    IsAllComplete = false;
                }
                row.Cells.Add(clinCur.City);
                if (!clinCur.IsHidden && string.IsNullOrEmpty(clinCur.City))
                {
                    row.Cells[row.Cells.Count - 1].CellColor = needsAttnCol;
                    IsAllComplete = false;
                }
                row.Cells.Add(clinCur.State);
                if (!clinCur.IsHidden && string.IsNullOrEmpty(clinCur.State))
                {
                    row.Cells[row.Cells.Count - 1].CellColor = needsAttnCol;
                    IsAllComplete = false;
                }
                row.Cells.Add(clinCur.Zip);
                if (!clinCur.IsHidden && string.IsNullOrEmpty(clinCur.Zip))
                {
                    row.Cells[row.Cells.Count - 1].CellColor = needsAttnCol;
                    IsAllComplete = false;
                }
                row.Cells.Add(clinCur.ProviderId.HasValue ? Providers.GetAbbr(clinCur.ProviderId.Value) : "");
                row.Cells.Add(clinCur.IsHidden ? "X" : "");
                row.Tag = clinCur;
                clinicsGrid.Rows.Add(row);
            }
            clinicsGrid.EndUpdate();

            if (IsAllComplete)
            {
                IsDone = true;
            }
            else
            {
                IsDone = false;
            }
        }

        void AddButton_Click(object sender, EventArgs e)
        {
            using (var formClinicEdit = new FormClinicEdit(new Clinic()))
            {
                formClinicEdit.ShowDialog();
                if (formClinicEdit.DialogResult == DialogResult.OK)
                {
                    Clinic.Insert(formClinicEdit.ClinicCur);

                    DataValid.SetInvalid(InvalidType.Providers);

                    CacheManager.InvalidateEverywhere<Clinic>();

                    LoadClinics();
                }
            }
        }

        void ClinicsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            var clinic = (Clinic)clinicsGrid.Rows[e.Row].Tag;

            using (var formClinicEdit = new FormClinicEdit(clinic))
            {
                formClinicEdit.ShowDialog();
                if (formClinicEdit.DialogResult == DialogResult.OK)
                {
                    Clinic.Update(clinic);

                    DataValid.SetInvalid(InvalidType.Providers);

                    CacheManager.InvalidateEverywhere<Clinic>();

                    LoadClinics();
                }
            }
        }

        void AdvancedButton_Click(object sender, EventArgs e)
        {
            using (var formClinics = new FormClinics())
            {
                formClinics.ShowDialog();
            } 
            LoadClinics();
        }
    }
}