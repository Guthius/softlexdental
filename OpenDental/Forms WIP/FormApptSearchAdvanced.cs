/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormApptSearchAdvanced : FormBase
    {
        /// <summary>
        /// The appointment passed in that we are trying to find a slot for.
        /// </summary>
        private Appointment appointment;

        /// <summary>The list of providers possibly set on previous search window before entering advanced search.</summary>
        private List<long> _listProvNums = new List<long>();
        /// <summary>The before time that was possibly set before entering this window. </summary>
        private string _beforeTime;
        /// <summary>The after time that was possibly set before entering this window. </summary>
        private string _afterTime;
        /// <summary>The after date that was possibly set before entering this window. </summary>
        private DateTime _afterDate;
        private List<ScheduleOpening> _listOpenings = new List<ScheduleOpening>();

        ///<summary>Pass in the currently selected apptNum along with all ApptNums that are associated to the current pinboard.</summary>
        public FormApptSearchAdvanced(long apptNum)
        {
            InitializeComponent();

            appointment = Appointments.GetOneApt(apptNum);

        }

        void FormApptSearchAdvanced_Load(object sender, EventArgs e)
        {
            if (appointment == null)
            {
                MessageBox.Show(
                    "Invalid appointment on the Pinboard.",
                    "Appointment",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                DialogResult = DialogResult.Abort;

                return;
            }

            if (_afterDate != DateTime.MinValue.Date && _afterDate != null)
            {
                dateSearchFrom.Text = _afterDate.ToShortDateString();
            }
            else
            {
                dateSearchFrom.Text = DateTime.Today.AddDays(1).ToShortDateString();
            }
            dateSearchTo.Text = PIn.Date(dateSearchFrom.Text).AddYears(2).AddDays(1).ToShortDateString();//default to 2 years from the afterDate. 
            textBefore.Text = _beforeTime;//just blindly set these. They will be validated when the search is ran. 
            textAfter.Text = _afterTime;
            //Fill all combo and listboxes.
            FillClinics();
            FillApptViews();
            FillBlockouts();
            FillProviders(GetProvidersForSelectedClinic(), _listProvNums);
        }

        internal void SetSearchArgs(long apptNum, List<long> listProvNums, string beforeTime, string afterTime, DateTime dateAfter)
        {
            _listProvNums = listProvNums;
            _beforeTime = beforeTime;
            _afterTime = afterTime;
            _afterDate = dateAfter.Date;
        }

        ///<summary>Fills comboBox with providers.  Optionally pass in a list of providers to set prov box with specific provs.</summary>
        private void FillProviders(List<Provider> listProvsForClinic, List<long> listProvNumsToSelect = null)
        {
            if (listProvNumsToSelect == null || listProvNumsToSelect.Count == 0)
            {
                listProvNumsToSelect = new List<long>();
            }
            comboBoxMultiProv.Items.Clear();
            comboBoxMultiProv.Items.Add(new ODBoxItem<Provider>(Lan.g(this, "None"), null));//tag=null
            foreach (Provider prov in listProvsForClinic)
            {
                ODBoxItem<Provider> boxProvItem = new ODBoxItem<Provider>(prov.GetLongDesc(), prov);
                comboBoxMultiProv.Items.Add(boxProvItem);
                if (prov.ProvNum.In(listProvNumsToSelect))
                {
                    comboBoxMultiProv.SetSelected(comboBoxMultiProv.Items.IndexOf(boxProvItem), true);
                }
            }
            if (comboBoxMultiProv.ListSelectedItems.Count == 0)
            {//no providers coming in, set default to 'none'
                comboBoxMultiProv.SetSelected(0, true);
            }
        }

        private List<Provider> GetProvidersForSelectedClinic(ProvMode provMode = ProvMode.All)
        {
            List<Provider> listProvs = Providers.GetProvsForClinic(comboBoxClinic.SelectedClinicNum);//returns all for no clinics
            switch (provMode)
            {
                case ProvMode.Dent:
                    listProvs.RemoveAll(x => x.IsSecondary);
                    break;
                case ProvMode.Hyg:
                    listProvs.RemoveAll(x => !x.IsSecondary);
                    break;
                case ProvMode.All:
                default:
                    break;
            }
            return listProvs;
        }

        ///<summary>Puts the form into clinic mode when using the clinics feature.  Otherwise; does nothing.</summary>
        private void FillClinics()
        {
            if (!Preferences.HasClinicsEnabled)
            {
                return;
            }
            labelClinic.Visible = true;
            browseClinicButton.Visible = true;
            if (comboBoxClinic.IsNothingSelected)
            {
                comboBoxClinic.SelectedIndex = 0;
            }
            if (comboBoxClinic.IsUnassignedSelected)
            {
                comboApptView.Visible = true;
                labelAptViews.Visible = true;
            }
        }

        ///<summary>Only fill appt views when clinic is HQ. </summary>
        private void FillApptViews()
        {
            if (!Preferences.HasClinicsEnabled || comboBoxClinic.SelectedTag<Clinic>().ClinicNum != 0)
            {
                return;//Either clinics are enabled and we're on a sepecific clinic OR clinics are not enabled. In either case don't fill appt views. 
            }
            List<ApptView> listApptViews = ApptViews.GetForClinic(comboBoxClinic.SelectedClinicNum);
            comboApptView.Items.Clear();
            foreach (ApptView view in listApptViews)
            {
                comboApptView.Items.Add(new ODBoxItem<ApptView>(view.Description, view));
            }
            if (listApptViews.Count != 0)
            {//default to the first
                comboApptView.SelectedIndex = 0;
            }
        }

        private void FillBlockouts()
        {
            List<Definition> listBlockoutDefs = Definition.GetByCategory(DefinitionCategory.BlockoutTypes);
            comboBlockout.Items.Clear();
            comboBlockout.Items.Add(new ODBoxItem<Definition>(Lan.g(this, "None"), null));//tag=null //if they select 'None' then only search for the provider(s).
            comboBlockout.SelectedIndex = 0;//default to none
            foreach (Definition blockoutType in listBlockoutDefs)
            {
                if (blockoutType.Value.Contains(BlockoutType.NoSchedule.GetDescription()))
                {
                    continue;//The search will already not show results for Do Not Schedule Blockout types, so users shouldn't be able to select them.
                }
                comboBlockout.Items.Add(new ODBoxItem<Definition>(blockoutType.Description, blockoutType));
            }
            comboBlockout.SelectedIndex = 0;
        }

        private void FillGrid()
        {
            resultsGrid.BeginUpdate();
            resultsGrid.Columns.Clear();
            ODGridColumn col = new ODGridColumn("Day", 85);
            resultsGrid.Columns.Add(col);
            col = new ODGridColumn("Date", 85, HorizontalAlignment.Center);
            resultsGrid.Columns.Add(col);
            col = new ODGridColumn("Time", 85, HorizontalAlignment.Center);
            resultsGrid.Columns.Add(col);
            resultsGrid.Rows.Clear();
            ODGridRow row;
            foreach (ScheduleOpening opening in _listOpenings)
            {
                row = new ODGridRow();
                row.Cells.Add(opening.DateTimeAvail.DayOfWeek.ToString());
                row.Cells.Add(opening.DateTimeAvail.Date.ToShortDateString());
                row.Cells.Add(opening.DateTimeAvail.ToShortTimeString());
                row.Tag = opening;
                resultsGrid.Rows.Add(row);
            }
            resultsGrid.EndUpdate();
        }

        private void DoSearch()
        {
            Cursor = Cursors.WaitCursor;
            DateTime startDate = dateSearchFrom.Value.Date.AddDays(-1);//Text on boxes is To/From. This will effecitvely make it the 'afterDate'. 
            DateTime endDate = dateSearchTo.Value.Date.AddDays(1);
            _listOpenings.Clear();
            #region validation
            if (startDate.Year < 1880 || endDate.Year < 1880)
            {
                Cursor = Cursors.Default;
                MsgBox.Show(this, "Invalid date selection.");
                return;
            }
            TimeSpan beforeTime = new TimeSpan(0);
            if (textBefore.Text != "")
            {
                try
                {
                    beforeTime = GetBeforeAfterTime(textBefore.Text, radioBeforePM.Checked);
                }
                catch
                {
                    Cursor = Cursors.Default;
                    MsgBox.Show(this, "Invalid 'Starting before' time.");
                    return;
                }
            }
            TimeSpan afterTime = new TimeSpan(0);
            if (textAfter.Text != "")
            {
                try
                {
                    afterTime = GetBeforeAfterTime(textAfter.Text, radioAfterPM.Checked);
                }
                catch
                {
                    Cursor = Cursors.Default;
                    MsgBox.Show(this, "Invalid 'Starting after' time.");
                    return;
                }
            }
            if (comboBoxMultiProv.SelectedTags<Provider>().Contains(null) && comboBlockout.SelectedTag<Definition>() == null)
            {
                Cursor = Cursors.Default;
                MsgBox.Show(this, "Please pick a provider or a blockout type.");
                return;
            }
            #endregion
            //get lists of info to do the search
            List<long> listOpNums = new List<long>();
            List<long> listClinicNums = new List<long>();
            List<long> listProvNums = new List<long>();
            long blockoutType = 0;
            if (comboBlockout.SelectedTag<Definition>() != null)
            {
                blockoutType = comboBlockout.SelectedTag<Definition>().Id;
                listProvNums.Add(0);//providers don't matter for blockouts
            }
            if (!comboBoxMultiProv.SelectedTags<Provider>().Contains(null))
            {
                foreach (ODBoxItem<Provider> provBoxItem in comboBoxMultiProv.ListSelectedItems)
                {
                    listProvNums.Add(provBoxItem.Tag.ProvNum);
                }
            }
            if (Preferences.HasClinicsEnabled)
            {
                if (comboBoxClinic.SelectedClinicNum != 0)
                {
                    listClinicNums.Add(comboBoxClinic.SelectedClinicNum);
                    listOpNums = Operatories.GetOpsForClinic(comboBoxClinic.SelectedClinicNum).Select(x => x.OperatoryNum).ToList();
                }
                else
                {//HQ //and unassigned (which is clinic num 0)
                    long apptViewNum = comboApptView.SelectedTag<ApptView>().ApptViewNum;
                    //get the disctinct clinic nums for the operatories in the current appointment view
                    List<long> listOpsForView = ApptViewItems.GetOpsForView(apptViewNum);
                    List<Operatory> listOperatories = Operatories.GetOperatories(listOpsForView, true);
                    listClinicNums = listOperatories.Select(x => x.ClinicNum).Distinct().ToList();
                    listOpNums = listOperatories.Select(x => x.OperatoryNum).ToList();
                }
            }
            else
            {//no clinics
                listOpNums = Operatories.GetDeepCopy(true).Select(x => x.OperatoryNum).ToList();
            }
            if (blockoutType != 0 && listProvNums.Max() > 0)
            {
                _listOpenings.AddRange(ApptSearch.GetSearchResultsForBlockoutAndProvider(listProvNums, appointment.AptNum, startDate, endDate, listOpNums, listClinicNums
                    , beforeTime, afterTime, blockoutType, 15));
            }
            else
            {
                _listOpenings = ApptSearch.GetSearchResults(appointment.AptNum, startDate, endDate, listProvNums, listOpNums, listClinicNums
                    , beforeTime, afterTime, blockoutType, resultCount: 15);
            }
            Cursor = Cursors.Default;
            FillGrid();
        }

        ///<summary>Helper method that returns a TimeSpan based on the time fields specified by the user.</summary>
        private TimeSpan GetBeforeAfterTime(string timeText, bool isAfterPM)
        {
            TimeSpan time;
            string[] hrMin = timeText.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);//doesn't work with foreign times.
            string hr = "0";
            if (hrMin.Length > 0)
            {
                hr = hrMin[0];
            }
            string min = "0";
            if (hrMin.Length > 1)
            {
                min = hrMin[1];
            }
            time = TimeSpan.FromHours(PIn.Double(hr)) + TimeSpan.FromMinutes(PIn.Double(min));
            if (isAfterPM && time.Hours < 12)
            {
                time = time + TimeSpan.FromHours(12);
            }
            return time;
        }

        private void butProviders_Click(object sender, EventArgs e)
        {
            List<Provider> listProviders = null;
            ODException.SwallowAnyException(() =>
            {
                listProviders = comboBoxMultiProv.Items.Cast<ODBoxItem<Provider>>()
                    .Where(x => x.Tag != null)
                    .Select(x => x.Tag)
                    .ToList();
            });
            FormProvidersMultiPick FormPMP = new FormProvidersMultiPick(listProviders);
            FormPMP.SelectedProviders = comboBoxMultiProv.SelectedTags<Provider>().FindAll(x => x != null);
            FormPMP.ShowDialog();
            if (FormPMP.DialogResult != DialogResult.OK)
            {
                return;
            }
            List<long> listProvNums = new List<long>();
            foreach (Provider prov in FormPMP.SelectedProviders)
            {
                listProvNums.Add(prov.ProvNum);
            }
            FillProviders(GetProvidersForSelectedClinic(), listProvNums);
        }

        private void comboBoxMultiProv_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBoxMultiProv.SelectedTags<Provider>().Contains(null))
            {
                //If the user every has 'none' selected, force it to be the only item selected.
                comboBoxMultiProv.SetSelected(false);
                comboBoxMultiProv.SetSelected(0, true);
            }
        }

        void BrowseClinicButton_Click(object sender, EventArgs e)
        {
            using (var formClinics = new FormClinics())
            {
                formClinics.IsMultiSelect = false;
                formClinics.IsSelectionMode = true;

                if (formClinics.ShowDialog() == DialogResult.OK)
                {
                    comboBoxClinic.SelectedClinicNum = formClinics.SelectedClinicNum;
                }
            }
        }

        private void comboBoxClinic_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBoxClinic.SelectedTag<Clinic>().ClinicNum == 0)
            {
                comboApptView.Visible = true;
                labelAptViews.Visible = true;
                FillApptViews();
            }
            else
            {
                comboApptView.Visible = false;
                labelAptViews.Visible = false;
            }
            List<long> listCurProvNums = comboBoxMultiProv.SelectedTags<Provider>().FindAll(x => x != null).Select(x => x.ProvNum).ToList();
            //attempt to re-select the providers if they still exist in the newly selected clinic.
            FillProviders(GetProvidersForSelectedClinic(), listCurProvNums);
        }

        private void ResultsGrid_CellClick(object sender, ODGridClickEventArgs e)
        {
            //get the day for the row that was clicked on.
            DateTime rowDate = ((ScheduleOpening)resultsGrid.Rows[e.Row].Tag).DateTimeAvail;

            //move the calendar day on the appt module to the day that was clicked on. 
            GotoModule.GotoAppointment(rowDate.Date, appointment.AptNum);
            //if clinics, move to the clinic as well? 
        }

        void MoreButton_Click(object sender, EventArgs e)
        {
            if (_listOpenings.Count < 1)
            {
                return;
            }
            //use the same appointment we were using from before, even if there is a new appointment on the pinboard
            dateSearchFrom.Text = _listOpenings[_listOpenings.Count - 1].DateTimeAvail.ToShortDateString();
            DoSearch();//should we prevent them (disable the button if they have changed any of their settings? 
        }

        private void butProvDentist_Click(object sender, EventArgs e)
        {
            List<Provider> listProvDents = GetProvidersForSelectedClinic(ProvMode.Dent);
            FillProviders(listProvDents, listProvDents.Select(x => x.ProvNum).ToList());//fill box with dentists and select them all
            DoSearch();
        }

        private void butProvHygenist_Click(object sender, EventArgs e)
        {
            List<Provider> listProvHyg = GetProvidersForSelectedClinic(ProvMode.Hyg);
            FillProviders(listProvHyg, listProvHyg.Select(x => x.ProvNum).ToList());
            DoSearch();
        }

        void SearchButton_Click(object sender, EventArgs e)
        {
            // Validate that there is an appointment on the pinboard. 
            if (appointment.AptNum <= 0)
            {
                MessageBox.Show(
                    "Invalid appointments on pinboard.", 
                    "Appointment",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            DoSearch();

            moreButton.Enabled = true;
        }

        private enum ProvMode
        {
            All,
            Dent,
            Hyg
        }
    }
}