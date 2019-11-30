using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental
{
    public partial class FormDoseSpotAssignClinicId : ODForm
    {
        private List<Clinic> _listClinicsInComboBox;
        private ClinicErx _clinicErxCur;
        List<ProgramPreference> _listClinicIDs;
        List<ProgramPreference> _listClinicKeys;

        public FormDoseSpotAssignClinicId(long clinicErxNum)
        {
            InitializeComponent();
            _clinicErxCur = ClinicErxs.GetFirstOrDefault(x => x.ClinicErxNum == clinicErxNum);

        }

        private void FormDoseSpotAssignUserId_Load(object sender, EventArgs e)
        {
            //_listClinicsInComboBox = Clinic.GetByUser(Security.CurrentUser).ToList();
            //List<ProgramProperty> listProgramProperties = ProgramProperties.GetForProgram(Programs.GetCur(ProgramName.eRx).ProgramNum);

            //_listClinicIDs = listProgramProperties.FindAll(x => x.Key == Erx.PropertyDescs.ClinicID);
            //_listClinicKeys = listProgramProperties.FindAll(x => x.Key == Erx.PropertyDescs.ClinicKey);
            //_listClinicsInComboBox.RemoveAll(x =>//Remove all clinics that already have a DoseSpot Clinic ID OR Clinic Key entered
            //  _listClinicIDs.FindAll(y => !string.IsNullOrWhiteSpace(y.Value)).Select(y => y.ClinicId).Contains(x.Id)
            //  || _listClinicKeys.FindAll(y => !string.IsNullOrWhiteSpace(y.Value)).Select(y => y.ClinicId).Contains(x.Id)
            //);
            //FillComboBox();
            //textClinicId.Text = _clinicErxCur.ClinicId;//ClinicID passed from Alert
            //textClinicKey.Text = _clinicErxCur.ClinicKey;//ClinicKey passed from Alert
            //textClinicDesc.Text = _clinicErxCur.ClinicDesc;//ClinicDesc passed from Alert
        }

        private void FillComboBox(long selectedClinicNum = -1)
        {
            comboClinics.Items.Clear();
            foreach (Clinic clinicCur in _listClinicsInComboBox)
            {
                ODBoxItem<Clinic> boxItemCur = new ODBoxItem<Clinic>(clinicCur.Description, clinicCur);
                comboClinics.Items.Add(boxItemCur);
                if (clinicCur.Id == selectedClinicNum)
                {
                    comboClinics.SelectedIndex = comboClinics.Items.Count - 1;//Select The item that was just added if it is the selected num.
                }
            }
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            //if (comboClinics.SelectedIndex == -1)
            //{
            //    MsgBox.Show(this, "Please select a clinic.");
            //    return;
            //}
            //_clinicErxCur.ClinicNum = comboClinics.SelectedTag<Clinic>().ClinicNum;
            //Program progErx = Programs.GetCur(ProgramName.eRx);
            //ProgramProperty ppClinicID = _listClinicIDs.FirstOrDefault(x => x.ClinicId == _clinicErxCur.ClinicNum);
            //if (ppClinicID == null)
            //{
            //    ppClinicID = new ProgramProperty();
            //    ppClinicID.ProgramId = progErx.ProgramNum;
            //    ppClinicID.ClinicId = _clinicErxCur.ClinicNum;
            //    ppClinicID.Key = Erx.PropertyDescs.ClinicID;
            //    ppClinicID.Value = _clinicErxCur.ClinicId;
            //    ProgramProperties.Insert(ppClinicID);
            //}
            //else
            //{
            //    ppClinicID.Value = _clinicErxCur.ClinicId;
            //    ProgramProperties.Update(ppClinicID);
            //}
            //ProgramProperty ppClinicKey = _listClinicKeys.FirstOrDefault(x => x.ClinicId == _clinicErxCur.ClinicNum);
            //if (ppClinicKey == null)
            //{
            //    ppClinicKey = new ProgramProperty();
            //    ppClinicKey.ProgramId = progErx.ProgramNum;
            //    ppClinicKey.ClinicId = _clinicErxCur.ClinicNum;
            //    ppClinicKey.Key = Erx.PropertyDescs.ClinicKey;
            //    ppClinicKey.Value = _clinicErxCur.ClinicKey;
            //    ProgramProperties.Insert(ppClinicKey);
            //}
            //else
            //{
            //    ppClinicKey.Value = _clinicErxCur.ClinicKey;
            //    ProgramProperties.Update(ppClinicKey);
            //}
            //DataValid.SetInvalid(InvalidType.Programs);
            DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void butClinicPick_Click(object sender, EventArgs e)
        {
            using (var formClinics = new FormClinics(_listClinicsInComboBox))
            {
                formClinics.IsSelectionMode = true;

                if (formClinics.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                FillComboBox(formClinics.SelectedClinicId);
            }
        }
    }
}