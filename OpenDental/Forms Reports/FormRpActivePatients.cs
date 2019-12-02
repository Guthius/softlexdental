using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.ReportingComplex;
using System.Linq;

namespace OpenDental
{
    public partial class FormRpActivePatients : ODForm
    {
        private List<Clinic> _listClinics;
        private List<Provider> _listProviders;
        private List<Definition> _listBillingTypeDefs;

        public FormRpActivePatients()
        {
            InitializeComponent();

        }

        private void FormRpActivePatients_Load(object sender, EventArgs e)
        {
            dateStart.SelectionStart = DateTime.Today;
            dateEnd.SelectionStart = DateTime.Today;
            _listBillingTypeDefs = Definition.GetByCategory(DefinitionCategory.BillingTypes);
            for (int i = 0; i < _listBillingTypeDefs.Count; i++)
            {
                listBillingTypes.Items.Add(_listBillingTypeDefs[i].Description);
            }
            _listProviders = Provider.GetForReporting().ToList();
            for (int i = 0; i < _listProviders.Count; i++)
            {
                listProv.Items.Add(_listProviders[i].GetLongDesc());
            }

            _listClinics = Clinic.GetByUser(Security.CurrentUser).ToList();
            if (!Security.CurrentUser.ClinicRestricted)
            {
                listClin.Items.Add(Lan.g(this, "Unassigned"));
                listClin.SetSelected(0, true);
            }
            for (int i = 0; i < _listClinics.Count; i++)
            {
                int curIndex = listClin.Items.Add(_listClinics[i].Abbr);
                if (Clinics.ClinicId == 0)
                {
                    checkAllClin.Checked = true;
                }
                if (_listClinics[i].Id == Clinics.ClinicId)
                {
                    listClin.SelectedIndices.Clear();
                    listClin.SetSelected(curIndex, true);
                }
            }

        }

        private void checkAllProv_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAllProv.Checked)
            {
                listProv.SelectedIndices.Clear();
            }
        }

        private void listProv_Click(object sender, EventArgs e)
        {
            if (listProv.SelectedIndices.Count > 0)
            {
                checkAllProv.Checked = false;
            }
        }

        private void checkAllClin_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAllClin.Checked)
            {
                listClin.SelectedIndices.Clear();
            }
        }

        private void listClin_Click(object sender, EventArgs e)
        {
            if (listClin.SelectedIndices.Count > 0)
            {
                checkAllClin.Checked = false;
            }
        }

        private void checkAllBilling_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAllBilling.Checked)
            {
                listBillingTypes.SelectedIndices.Clear();
            }
        }

        private void listBillingTypes_Click(object sender, EventArgs e)
        {
            if (listBillingTypes.SelectedIndices.Count > 0)
            {
                checkAllBilling.Checked = false;
            }
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            if (!checkAllBilling.Checked && listBillingTypes.SelectedIndices.Count == 0)
            {
                MsgBox.Show(this, "At least one Billing Type must be selected.");
                return;
            }
            if (!checkAllProv.Checked && listProv.SelectedIndices.Count == 0)
            {
                MsgBox.Show(this, "At least one Provider must be selected.");
                return;
            }

            if (!checkAllClin.Checked && listClin.SelectedIndices.Count == 0)
            {
                MsgBox.Show(this, "At least one Clinic must be selected.");
                return;
            }

            ReportComplex report = new ReportComplex(true, false);
            List<long> listProvNums = new List<long>();
            List<long> listClinicNums = new List<long>();
            List<long> listBillingTypeDefNums = new List<long>();
            if (checkAllProv.Checked)
            {
                for (int i = 0; i < _listProviders.Count; i++)
                {
                    listProvNums.Add(_listProviders[i].Id);
                }
            }
            else
            {
                for (int i = 0; i < listProv.SelectedIndices.Count; i++)
                {
                    listProvNums.Add(_listProviders[listProv.SelectedIndices[i]].Id);
                }
            }

            if (checkAllClin.Checked)
            {
                if (!Security.CurrentUser.ClinicRestricted)
                {
                    listClinicNums.Add(0);
                }
                for (int i = 0; i < _listClinics.Count; i++)
                {
                    listClinicNums.Add(_listClinics[i].Id);
                }
            }
            else
            {
                for (int i = 0; i < listClin.SelectedIndices.Count; i++)
                {
                    if (Security.CurrentUser.ClinicRestricted)
                    {
                        listClinicNums.Add(_listClinics[listClin.SelectedIndices[i]].Id);//we know that the list is a 1:1 to _listClinics
                    }
                    else
                    {
                        if (listClin.SelectedIndices[i] == 0)
                        {
                            listClinicNums.Add(0);
                        }
                        else
                        {
                            listClinicNums.Add(_listClinics[listClin.SelectedIndices[i] - 1].Id);//Minus 1 from the selected index
                        }
                    }
                }
            }

            if (checkAllBilling.Checked)
            {
                for (int i = 0; i < _listBillingTypeDefs.Count; i++)
                {
                    listBillingTypeDefNums.Add(_listBillingTypeDefs[i].Id);
                }
            }
            else
            {
                for (int i = 0; i < listBillingTypes.SelectedIndices.Count; i++)
                {
                    listBillingTypeDefNums.Add(_listBillingTypeDefs[listBillingTypes.SelectedIndices[i]].Id);
                }
            }
            DataTable tablePats = RpActivePatients.GetActivePatientTable(dateStart.SelectionStart, dateEnd.SelectionStart, listProvNums, listClinicNums
                , listBillingTypeDefNums, checkAllProv.Checked, checkAllClin.Checked, checkAllBilling.Checked);
            string subtitleProvs = "";
            string subtitleClinics = "";
            string subtitleBilling = "";
            if (checkAllProv.Checked)
            {
                subtitleProvs = Lan.g(this, "All Providers");
            }
            else
            {
                for (int i = 0; i < listProv.SelectedIndices.Count; i++)
                {
                    if (i > 0)
                    {
                        subtitleProvs += ", ";
                    }
                    subtitleProvs += _listProviders[listProv.SelectedIndices[i]].Abbr;
                }
            }

                if (checkAllClin.Checked)
                {
                    subtitleClinics = Lan.g(this, "All Clinics");
                }
                else
                {
                    for (int i = 0; i < listClin.SelectedIndices.Count; i++)
                    {
                        if (i > 0)
                        {
                            subtitleClinics += ", ";
                        }
                        if (Security.CurrentUser.ClinicRestricted)
                        {
                            subtitleClinics += _listClinics[listClin.SelectedIndices[i]].Abbr;
                        }
                        else
                        {
                            if (listClin.SelectedIndices[i] == 0)
                            {
                                subtitleClinics += Lan.g(this, "Unassigned");
                            }
                            else
                            {
                                subtitleClinics += _listClinics[listClin.SelectedIndices[i] - 1].Abbr;//Minus 1 from the selected index
                            }
                        }
                    }
                }
            
            if (checkAllBilling.Checked)
            {
                subtitleBilling = Lan.g(this, "All Billing Types");
            }
            else
            {
                for (int i = 0; i < listBillingTypes.SelectedIndices.Count; i++)
                {
                    if (i > 0)
                    {
                        subtitleBilling += ", ";
                    }
                    subtitleBilling += Defs.GetValue(DefinitionCategory.BillingTypes, _listBillingTypeDefs[listBillingTypes.SelectedIndices[i]].Id);
                }
            }
            report.ReportName = Lan.g(this, "Active Patients");
            report.AddTitle("Title", Lan.g(this, "Active Patients"));
            report.AddSubTitle("Date", dateStart.SelectionStart.ToShortDateString() + " - " + dateEnd.SelectionStart.ToShortDateString());
            report.AddSubTitle("Providers", subtitleProvs);

            report.AddSubTitle("Clinics", subtitleClinics);

            report.AddSubTitle("Billing", subtitleBilling);
            QueryObject query;

            query = report.AddQuery(tablePats, "", "clinic", SplitByKind.Value, 0);

            query.AddColumn("Name", 150, FieldValueType.String);
            query.AddColumn("Provider", 80, FieldValueType.String);
            query.AddColumn("Address", 150, FieldValueType.String);
            query.AddColumn("Address2", 90, FieldValueType.String);
            query.AddColumn("City", 100, FieldValueType.String);
            query.AddColumn("State", 40, FieldValueType.String);
            query.AddColumn("Zip", 70, FieldValueType.String);
            query.AddColumn("Carrier", 120, FieldValueType.String);
            query.AddGroupSummaryField("Patient Count:", "Carrier", "Name", SummaryOperation.Count);
            report.AddPageNum();
            if (!report.SubmitQueries())
            {
                return;
            }
            FormReportComplex FormR = new FormReportComplex(report);
            FormR.ShowDialog();
            //DialogResult=DialogResult.OK;		
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

    }
}
