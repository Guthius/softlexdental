using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormApptTypes : FormBase
    {
        List<AppointmentType> appointmentTypeList = new List<AppointmentType>();


        bool hasChanged = false;

        ///<summary>Stale deep copy of _listApptTypes to use with sync.</summary>
        private List<AppointmentType> _listApptTypesOld;
        
        public bool IsNoneAllowed;
        public bool IsSelectionMode;
        ///<summary>Set to true when IsSelectionMode is true and the user will be able to select multiple appointment types instead of just one.
        ///ListSelectedApptTypes will contain all of the types that the user selected.</summary>
        public bool AllowMultipleSelections;
        ///<summary>The appointment type that was selected if IsSelectionMode is true.
        ///If IsSelectionMode is true and this object is prefilled with an appointment type then the grid will preselect that type if possible.
        ///It is not guaranteed that the appointment type will be selected.
        ///This object should only be read from externally after DialogResult.OK has been returned.  Can be null.</summary></summary>
        public AppointmentType SelectedAptType;
        ///<summary>Contains all of the selected appointment types if IsSelectionMode is true.
        ///If IsSelectionMode and AllowMultiple are true, this object can be prefilled with appointment types which will be preselected if possible.
        ///It is not guaranteed that all appointment types will be selected (due to hidden).
        ///This list should only be read from externally after DialogResult.OK has been returned.</summary>
        public List<AppointmentType> ListSelectedApptTypes = new List<AppointmentType>();

        public FormApptTypes() => InitializeComponent();

        void LoadAppointmentTypes()
        {
            typesGrid.BeginUpdate();
            typesGrid.Columns.Clear();
            typesGrid.Columns.Add(new ODGridColumn("Name", 200));
            typesGrid.Columns.Add(new ODGridColumn("Color", 50, HorizontalAlignment.Center));
            typesGrid.Columns.Add(new ODGridColumn("Hidden", 0, HorizontalAlignment.Center));
            typesGrid.Rows.Clear();

            appointmentTypeList.Sort(AppointmentTypes.SortItemOrder);
            foreach (AppointmentType appointmentType in appointmentTypeList)
            {
                var row = new ODGridRow();
                row.Cells.Add(appointmentType.AppointmentTypeName);
                row.Cells.Add(""); //color row, no text.
                row.Cells[1].CellColor = appointmentType.AppointmentTypeColor;
                row.Cells.Add(appointmentType.IsHidden ? "X" : "");
                row.Tag = appointmentType;
                typesGrid.Rows.Add(row);
            }

            //Always add a None option to the end of the list when in selection mode.
            if (IsNoneAllowed)
            {
                var row = new ODGridRow();
                row.Cells.Add("None");
                typesGrid.Rows.Add(row);
                row.Tag = null;
            }

            typesGrid.EndUpdate();
        }

        void FormApptTypes_Load(object sender, EventArgs e)
        {
            if (IsSelectionMode)
            {
                acceptButton.Visible = true;
                addButton.Visible = false;
                downButton.Visible = false;
                upButton.Visible = false;
                warnCheckBox.Visible = false;
                promptCheckBox.Visible = false;

                if (AllowMultipleSelections)
                {
                    Text = "Select Appointment Types";
                    typesGrid.SelectionMode = GridSelectionMode.MultiExtended;
                }
                else
                {
                    Text = "Select Appointment Type";
                }

                typesGrid.Location = new Point(13, 19);
                typesGrid.Size = new Size(292, 447);
            }

            promptCheckBox.Checked = Preference.GetBool(PreferenceName.AppointmentTypeShowPrompt);
            warnCheckBox.Checked = Preference.GetBool(PreferenceName.AppointmentTypeShowWarning);
            //don't show hidden appointment types in selection mode
            appointmentTypeList = AppointmentTypes.GetDeepCopy(IsSelectionMode);
            _listApptTypesOld = AppointmentTypes.GetDeepCopy();
            LoadAppointmentTypes();

            //Preselect the corresponding appointment type(s) once on load.  Do not do this within FillMain().
            if (IsSelectionMode)
            {
                if (SelectedAptType != null)
                {
                    ListSelectedApptTypes.Add(SelectedAptType);
                }
                for (int i = 0; i < typesGrid.Rows.Count; i++)
                {
                    if (((AppointmentType)typesGrid.Rows[i].Tag) != null //The "None" option will always be null
                        && ListSelectedApptTypes.Any(x => x.AppointmentTypeNum == ((AppointmentType)typesGrid.Rows[i].Tag).AppointmentTypeNum))
                    {
                        typesGrid.SetSelected(i, true);
                    }
                }
            }
        }

        void FormApptTypes_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsSelectionMode)
            {
                if (hasChanged)
                {
                    Preference.Update(PreferenceName.AppointmentTypeShowPrompt, promptCheckBox.Checked);
                    Preference.Update(PreferenceName.AppointmentTypeShowWarning, warnCheckBox.Checked);

                    for (int i = 0; i < appointmentTypeList.Count; i++)
                    {
                        appointmentTypeList[i].ItemOrder = i;
                    }
                    AppointmentTypes.Sync(appointmentTypeList, _listApptTypesOld);

                    DataValid.SetInvalid(InvalidType.AppointmentTypes);
                    DataValid.SetInvalid(InvalidType.Prefs);
                }
                DialogResult = DialogResult.OK;
            }
        }

        void UpButton_Click(object sender, EventArgs e)
        {
            if (typesGrid.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                    "Please select an item in the grid first.", 
                    "Appointment Types", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                return;
            }

            if (typesGrid.GetSelectedIndex() == 0) return;
            
            int index = typesGrid.GetSelectedIndex();

            hasChanged = true;

            appointmentTypeList[index - 1].ItemOrder += 1;
            appointmentTypeList[index].ItemOrder -= 1;

            LoadAppointmentTypes();

            index -= 1;

            typesGrid.SetSelected(index, true);
        }

        void DownButton_Click(object sender, EventArgs e)
        {
            if (typesGrid.GetSelectedIndex() == -1)
            {
                MessageBox.Show(
                    "Please select an item in the grid first.",
                    "Appointment Types",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (typesGrid.GetSelectedIndex() == appointmentTypeList.Count - 1) return;

            int index = typesGrid.GetSelectedIndex();

            hasChanged = true;

            appointmentTypeList[index + 1].ItemOrder -= 1;
            appointmentTypeList[index].ItemOrder += 1;

            LoadAppointmentTypes();

            index += 1;

            typesGrid.SetSelected(index, true);
        }

        void TypesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            if (IsSelectionMode)
            {
                AcceptButton_Click(this, EventArgs.Empty);
            }
            else
            {
                using (var formApptTypeEdit = new FormApptTypeEdit())
                {
                    formApptTypeEdit.AppointmentTypeCur = appointmentTypeList[e.Row];

                    if (formApptTypeEdit.ShowDialog() == DialogResult.OK)
                    {
                        if (formApptTypeEdit.AppointmentTypeCur == null)
                        {
                            appointmentTypeList.RemoveAt(e.Row);
                        }
                        else
                        {
                            appointmentTypeList[e.Row] = formApptTypeEdit.AppointmentTypeCur;
                        }

                        hasChanged = true;

                        LoadAppointmentTypes();
                    }
                }
            }
        }

        void PromptCheckBox_CheckedChanged(object sender, EventArgs e) => hasChanged = true;

        void WarnCheckBox_CheckedChanged(object sender, EventArgs e) => hasChanged = true;
        
        void AddButton_Click(object sender, EventArgs e)
        {
            using (var formApptTypeEdit = new FormApptTypeEdit())
            {
                formApptTypeEdit.AppointmentTypeCur = new AppointmentType
                {
                    ItemOrder = appointmentTypeList.Count,
                    IsNew = true,
                    AppointmentTypeColor = Color.White
                };

                if (formApptTypeEdit.ShowDialog() != DialogResult.OK)
                {
                    appointmentTypeList.Add(formApptTypeEdit.AppointmentTypeCur);

                    hasChanged = true;

                    LoadAppointmentTypes();
                }
            }
        }

        void AcceptButton_Click(object sender, EventArgs e)
        {
            ListSelectedApptTypes = typesGrid.SelectedTags<AppointmentType>();
            SelectedAptType = ListSelectedApptTypes.FirstOrDefault();

            DialogResult = DialogResult.OK;
        }
    }
}