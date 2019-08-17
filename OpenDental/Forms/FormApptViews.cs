using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormApptViews : FormBase
    {
        bool viewChanged;
        long clinicNum;
        List<Clinic> clinicsList;
        List<ApptView> apptViewsList;

        public FormApptViews() => InitializeComponent();

        void FormApptViews_Load(object sender, EventArgs e)
        {
            if (!Preferences.HasClinicsEnabled)
            {
                clinicComboBox.Visible = false;
                clinicLabel.Visible = false;
            }
            else
            {
                clinicNum = Clinics.ClinicNum;
                clinicsList = Clinics.GetForUserod(Security.CurUser);
                clinicComboBox.Items.Clear();

                if (!Security.CurUser.ClinicIsRestricted)
                {
                    clinicComboBox.Items.Add("Headquarters"); // In this form, the Headquarters list is the list of views that are not assigned to a clinic
                    clinicComboBox.SelectedIndex = 0;
                }

                for (int i = 0; i < clinicsList.Count; i++)
                {
                    clinicComboBox.Items.Add(clinicsList[i].Abbr);
                    if (clinicNum == clinicsList[i].ClinicNum)
                    {
                        clinicComboBox.SelectedIndex = Security.CurUser.ClinicIsRestricted ? i : i + 1;
                    }
                }
            }

            FillViewList();
            if (Preference.GetInt(PreferenceName.AppointmentTimeIncrement) == 5)
            {
                increment5RadioButton.Checked = true;
            }
            else if (Preference.GetInt(PreferenceName.AppointmentTimeIncrement) == 10)
            {
                increment10RadioButton.Checked = true;
            }
            else
            {
                increment15RadioButton.Checked = true;
            }
        }

        void FillViewList()
        {
            Cache.Refresh(InvalidType.Views);
            viewsListBox.Items.Clear();

            apptViewsList = new List<ApptView>();

            List<ApptView> listApptViewsTemp = ApptViews.GetDeepCopy();

            string F;
            for (int i = 0; i < listApptViewsTemp.Count; i++)
            {
                // Only add views assigned to the clinic selected
                if (Preferences.HasClinicsEnabled && clinicNum != listApptViewsTemp[i].ClinicNum) continue; 

                if (viewsListBox.Items.Count < 12)
                    F = "F" + (viewsListBox.Items.Count + 1).ToString() + "-";
                else
                    F = "";

                viewsListBox.Items.Add(F + listApptViewsTemp[i].Description);
                apptViewsList.Add(listApptViewsTemp[i]);
            }
        }

        void clinicComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (Security.CurUser.ClinicIsRestricted)
            {
                clinicNum = clinicsList[clinicComboBox.SelectedIndex].ClinicNum;
            }
            else
            {
                if (clinicComboBox.SelectedIndex == 0)
                {
                    clinicNum = 0;
                }
                else
                {
                    clinicNum = clinicsList[clinicComboBox.SelectedIndex - 1].ClinicNum;
                }
            }
            FillViewList();
        }

        void viewsListBox_DoubleClick(object sender, EventArgs e)
        {
            if (viewsListBox.SelectedIndex == -1) return;

            int selected = viewsListBox.SelectedIndex;
            ApptView ApptViewCur = apptViewsList[viewsListBox.SelectedIndex];

            using (var formApptViewEdit = new FormApptViewEdit())
            {
                formApptViewEdit.ApptViewCur = ApptViewCur;
                formApptViewEdit.ClinicNum = clinicNum;

                if (formApptViewEdit.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            viewChanged = true;

            FillViewList();
            if (selected < viewsListBox.Items.Count)
            {
                viewsListBox.SelectedIndex = selected;
            }
            else
            {
                viewsListBox.SelectedIndex = -1;
            }
        }

        void addButton_Click(object sender, EventArgs e)
        {
            ApptView ApptViewCur = new ApptView();
            if (apptViewsList.Count == 0)
            {
                ApptViewCur.ItemOrder = 0;
            }
            else
            {
                ApptViewCur.ItemOrder = apptViewsList[apptViewsList.Count - 1].ItemOrder + 1;
            }

            ApptViewCur.ApptTimeScrollStart = DateTime.Parse("08:00:00").TimeOfDay; // Default to 8 AM

            ApptViews.Insert(ApptViewCur); // This also gets the primary key

            using (var formApptViewEdit = new FormApptViewEdit())
            {
                formApptViewEdit.ApptViewCur = ApptViewCur;
                formApptViewEdit.IsNew = true;
                formApptViewEdit.ClinicNum = clinicNum;

                if (formApptViewEdit.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            viewChanged = true;

            FillViewList();

            viewsListBox.SelectedIndex = viewsListBox.Items.Count - 1;
        }

        void upButton_Click(object sender, EventArgs e)
        {
            if (viewsListBox.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select a category first.",
                    "Appointment Views", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            if (viewsListBox.SelectedIndex == 0) return;
            
            //it will flip flop with the one above it
            ApptView ApptViewCur = apptViewsList[viewsListBox.SelectedIndex - 1];
            ApptViewCur.ItemOrder = viewsListBox.SelectedIndex;
            ApptViews.Update(ApptViewCur);

            //now the other
            ApptViewCur = apptViewsList[viewsListBox.SelectedIndex];
            ApptViewCur.ItemOrder = viewsListBox.SelectedIndex - 1;
            ApptViews.Update(ApptViewCur);

            viewChanged = true;

            FillViewList();

            viewsListBox.SelectedIndex = apptViewsList.FindIndex(x => x.ApptViewNum == ApptViewCur.ApptViewNum);
        }

        void downButton_Click(object sender, EventArgs e)
        {
            if (viewsListBox.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select a category first.",
                    "Appointment Views",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (viewsListBox.SelectedIndex == viewsListBox.Items.Count - 1) return;
            
            //it will flip flop with the one below it
            ApptView ApptViewCur = apptViewsList[viewsListBox.SelectedIndex + 1];
            ApptViewCur.ItemOrder = viewsListBox.SelectedIndex;
            ApptViews.Update(ApptViewCur);

            //now the other
            ApptViewCur = apptViewsList[viewsListBox.SelectedIndex];
            ApptViewCur.ItemOrder = viewsListBox.SelectedIndex + 1;
            ApptViews.Update(ApptViewCur);

            viewChanged = true;

            FillViewList();

            viewsListBox.SelectedIndex = apptViewsList.FindIndex(x => x.ApptViewNum == ApptViewCur.ApptViewNum);
        }

        void procedureColorsButton_Click(object sender, EventArgs e)
        {
            using (var formProcColors = new FormProcApptColors())
            {
                formProcColors.ShowDialog();
            }
        }

        void FormApptViews_FormClosing(object sender, FormClosingEventArgs e)
        {
            int newIncrement = 15;
            if (increment5RadioButton.Checked)
            {
                newIncrement = 5;
            }

            if (increment10RadioButton.Checked)
            {
                newIncrement = 10;
            }

            if (Preference.Update(PreferenceName.AppointmentTimeIncrement, newIncrement))
            {
                DataValid.SetInvalid(InvalidType.Prefs);
            }

            if (viewChanged)
            {
                DataValid.SetInvalid(InvalidType.Views);
            }
        }
    }
}