using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDental.ReportingComplex;
using OpenDentBusiness;
using System.Linq;
using CodeBase;

namespace OpenDental
{
    public partial class FormRpPresentedTreatmentProduction : ODForm
    {
        private List<User> _listUsers;
        private List<Clinic> _listClinics;
        public FormRpPresentedTreatmentProduction()
        {
            InitializeComponent();

        }

        private void FormRpTreatPlanPresenter_Load(object sender, EventArgs e)
        {
            date1.SelectionStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
            date2.SelectionStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);
            _listUsers = User.All();
            listUser.Items.AddRange(_listUsers.Select(x => x.UserName).ToArray());
            checkAllUsers.Checked = true;

            if (!Security.CurrentUser.ClinicRestricted)
            {
                listClin.Items.Add(Lan.g(this, "Unassigned"));
            }
            _listClinics = Clinic.GetByUser(Security.CurrentUser).ToList();
            listClin.Items.AddRange(_listClinics.Select(x => x.Abbr).ToArray());
            checkAllClinics.Checked = true;
        }

        private void RunTotals(List<long> listUserNums, List<long> listClinicsNums)
        {
            ReportComplex report = new ReportComplex(true, false);
            report.AddTitle("Title", Lan.g(this, "Presented Treatment Production"));
            report.AddSubTitle("SubTitle", "Totals Report");
            report.AddSubTitle("PracTitle", Preference.GetString(PreferenceName.PracticeTitle));
            report.AddSubTitle("Date", date1.SelectionStart.ToShortDateString() + " - " + date2.SelectionStart.ToShortDateString());
            if (checkAllUsers.Checked)
            {
                report.AddSubTitle("Users", Lan.g(this, "All Users"));
            }
            else
            {
                string strUsers = "";
                for (int i = 0; i < listUser.SelectedIndices.Count; i++)
                {
                    if (i == 0)
                    {
                        strUsers = _listUsers[listUser.SelectedIndices[i]].UserName;
                    }
                    else
                    {
                        strUsers += ", " + _listUsers[listUser.SelectedIndices[i]].UserName;
                    }
                }
                report.AddSubTitle("Users", strUsers);
            }

            if (checkAllClinics.Checked)
            {
                report.AddSubTitle("Clinics", Lan.g(this, "All Clinics"));
            }
            else
            {
                string clinNames = "";
                for (int i = 0; i < listClin.SelectedIndices.Count; i++)
                {
                    if (i > 0)
                    {
                        clinNames += ", ";
                    }
                    if (Security.CurrentUser.ClinicRestricted)
                    {
                        clinNames += _listClinics[listClin.SelectedIndices[i]].Abbr;
                    }
                    else
                    {
                        if (listClin.SelectedIndices[i] == 0)
                        {
                            clinNames += Lan.g(this, "Unassigned");
                        }
                        else
                        {
                            clinNames += _listClinics[listClin.SelectedIndices[i] - 1].Abbr;//Minus 1 from the selected index
                        }
                    }
                }
                report.AddSubTitle("Clinics", clinNames);
            }

            DataTable table = RpPresentedTreatmentProduction.GetPresentedTreatmentProductionTable(
                date1.SelectionStart, date2.SelectionStart, listClinicsNums, checkAllClinics.Checked, true, radioPresenter.Checked, radioFirstPresented.Checked, listUserNums, false);
            QueryObject query = report.AddQuery(table, "", "", SplitByKind.None, 1, true);
            query.AddColumn(Lan.g(this, "Presenter"), 100, FieldValueType.String);
            query.AddColumn(Lan.g(this, "# of Procs"), 70, FieldValueType.Integer);
            query.AddColumn(Lan.g(this, "GrossProd"), 100, FieldValueType.Number);
            query.AddColumn(Lan.g(this, "WriteOffs"), 100, FieldValueType.Number);
            query.AddColumn(Lan.g(this, "Adjustments"), 100, FieldValueType.Number);
            query.AddColumn(Lan.g(this, "NetProduction"), 100, FieldValueType.Number);
            if (!report.SubmitQueries())
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            FormReportComplex FormR = new FormReportComplex(report);
            FormR.ShowDialog();
            DialogResult = DialogResult.OK;
        }

        private void RunDetailed(List<long> listUserNums, List<long> listClinicsNums)
        {
            ReportComplex report = new ReportComplex(true, false);
            report.AddTitle("Title", Lan.g(this, "Presented Treatment Production"));
            report.AddSubTitle("SubTitle", "Detailed Report");
            report.AddSubTitle("PracTitle", Preference.GetString(PreferenceName.PracticeTitle));
            report.AddSubTitle("Date", date1.SelectionStart.ToShortDateString() + " - " + date2.SelectionStart.ToShortDateString());
            if (checkAllUsers.Checked)
            {
                report.AddSubTitle("Users", Lan.g(this, "All Users"));
            }
            else
            {
                string strUsers = "";
                for (int i = 0; i < listUser.SelectedIndices.Count; i++)
                {
                    if (i == 0)
                    {
                        strUsers = _listUsers[listUser.SelectedIndices[i]].UserName;
                    }
                    else
                    {
                        strUsers += ", " + _listUsers[listUser.SelectedIndices[i]].UserName;
                    }
                }
                report.AddSubTitle("Users", strUsers);
            }

            if (checkAllClinics.Checked)
            {
                report.AddSubTitle("Clinics", Lan.g(this, "All Clinics"));
            }
            else
            {
                string clinNames = "";
                for (int i = 0; i < listClin.SelectedIndices.Count; i++)
                {
                    if (i > 0)
                    {
                        clinNames += ", ";
                    }
                    if (Security.CurrentUser.ClinicRestricted)
                    {
                        clinNames += _listClinics[listClin.SelectedIndices[i]].Abbr;
                    }
                    else
                    {
                        if (listClin.SelectedIndices[i] == 0)
                        {
                            clinNames += Lan.g(this, "Unassigned");
                        }
                        else
                        {
                            clinNames += _listClinics[listClin.SelectedIndices[i] - 1].Abbr;//Minus 1 from the selected index
                        }
                    }
                }
                report.AddSubTitle("Clinics", clinNames);
            }

            DataTable tableReport = RpPresentedTreatmentProduction.GetPresentedTreatmentProductionTable(date1.SelectionStart, date2.SelectionStart, listClinicsNums, checkAllClinics.Checked, true, radioPresenter.Checked, radioFirstPresented.Checked, listUserNums, true);
            QueryObject query = report.AddQuery(tableReport, "", "", SplitByKind.None, 1, true);
            query.AddColumn("\r\n" + Lan.g(this, "Presenter"), 90, FieldValueType.String);
            query.AddColumn(Lan.g(this, "Date") + "\r\n" + Lan.g(this, "Presented"), 75, FieldValueType.Date);
            query.AddColumn(Lan.g(this, "Date") + "\r\n" + Lan.g(this, "Completed"), 75, FieldValueType.Date);
            query.AddColumn("\r\n" + Lan.g(this, "Descript"), 200, FieldValueType.String);
            query.AddColumn("\r\n" + Lan.g(this, "GrossProd"), 90, FieldValueType.Number);
            query.AddColumn("\r\n" + Lan.g(this, "WriteOffs"), 90, FieldValueType.Number);
            query.AddColumn("\r\n" + Lan.g(this, "Adjustments"), 90, FieldValueType.Number);
            query.AddColumn("\r\n" + Lan.g(this, "NetProduction"), 90, FieldValueType.Number);
            if (!report.SubmitQueries())
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            FormReportComplex FormR = new FormReportComplex(report);
            FormR.ShowDialog();
            DialogResult = DialogResult.OK;
        }

        private void checkAllUsers_Click(object sender, EventArgs e)
        {
            if (checkAllUsers.Checked)
            {
                listUser.SelectedIndices.Clear();
            }
        }

        private void listUser_Click(object sender, EventArgs e)
        {
            if (listUser.SelectedIndices.Count > 0)
            {
                checkAllUsers.Checked = false;
            }
        }

        private void checkAllClinics_Click(object sender, EventArgs e)
        {
            if (checkAllClinics.Checked)
            {
                listClin.SelectedIndices.Clear();
            }
        }

        private void listClin_Click(object sender, EventArgs e)
        {
            if (listClin.SelectedIndices.Count > 0)
            {
                checkAllClinics.Checked = false;
            }
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            if (date2.SelectionStart < date1.SelectionStart)
            {
                MsgBox.Show(this, "End date cannot be before start date.");
                return;
            }
            if (!checkAllUsers.Checked && listUser.SelectedIndices.Count == 0)
            {
                MsgBox.Show(this, "Please select at least one user.");
                return;
            }
            if (!checkAllClinics.Checked && listClin.SelectedIndices.Count == 0)
            {
                MsgBox.Show(this, "Please select at least one clinic.");
                return;
            }
            List<long> listUserNums = new List<long>();
            List<long> listClinicNums = new List<long>();
            if (checkAllUsers.Checked)
            {
                listUserNums = _listUsers.Select(x => x.Id).ToList();
            }
            else
            {
                listUserNums = listUser.SelectedIndices.OfType<int>().ToList().Select(x => _listUsers[x].Id).ToList();
            }

            if (checkAllClinics.Checked)
            {
                listClinicNums = _listClinics.Select(x => x.Id).ToList();
            }
            else
            {
                for (int i = 0; i < listClin.SelectedIndices.Count; i++)
                {
                    if (Security.CurrentUser.ClinicRestricted)
                    {
                        listClinicNums.Add(_listClinics[listClin.SelectedIndices[i]].Id);
                    }
                    else if (listClin.SelectedIndices[i] != 0)
                    {
                        listClinicNums.Add(_listClinics[listClin.SelectedIndices[i] - 1].Id);
                    }
                }
            }
            if (!Security.CurrentUser.ClinicRestricted && (listClin.GetSelected(0) || checkAllClinics.Checked))
            {
                listClinicNums.Add(0);
            }

            if (radioDetailed.Checked)
            {
                RunDetailed(listUserNums, listClinicNums);
            }
            else
            {
                RunTotals(listUserNums, listClinicNums);
            }
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
