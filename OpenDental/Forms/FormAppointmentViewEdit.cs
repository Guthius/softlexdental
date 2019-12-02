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
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormAppointmentViewEdit : FormBase
    {
        private readonly AppointmentView appointmentView;
        private readonly List<AppointmentViewItem> appointmentViewItems;
        private readonly List<AppointmentViewItem> displayedFields = new List<AppointmentViewItem>();
        private TreeNode treeNodeFields;
        private TreeNode treeNodeAppointmentFields;
        private TreeNode treeNodePatientFields;
        
        // TODO: We need to rework the way fields work so they can be translated...

        private static readonly List<string> fieldNames = new List<string>
            {
                "Address",
                "AddrNote",
                "Age",
                "ASAP",
                "ASAP[A]",
                "AssistantAbbr",
                "Birthdate",
                "ChartNumAndName",
                "ChartNumber",
                "ConfirmedColor",
                "CreditType",
                "DiscountPlan",
                "EstPatientPortion",
                "Guardians",
                "HasDiscount[D]",
                "HasIns[I]",
                "HmPhone",
                "InsToSend[!]",
                "Insurance",
                "InsuranceColor",
                "IsLate[L]",
                "Lab",
                "LateColor",
                "MedOrPremed[+]",
                "MedUrgNote",
                "NetProduction",
                "Note",
                "PatientName",
                "PatientNameF",
                "PatientNamePref",
                "PatNum",
                "PatNumAndName",
                "PremedFlag",
                "Procs",
                "ProcsColored",
                "Production",
                "Provider",
                "TimeAskedToArrive",
                "WirelessPhone",
                "WkPhone"
            };

        /// <summary>
        /// Gets the ID of the selected clinic.
        /// </summary>
        private long SelectedClinicId
        {
            get
            {
                if (clinicComboBox.SelectedItem is Clinic clinic)
                {
                    return clinic.Id;
                }
                return 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormAppointmentViewEdit"/> class.
        /// </summary>
        /// <param name="appointmentView">The appointment view to edit.</param>
        public FormAppointmentViewEdit(AppointmentView appointmentView)
        {
            InitializeComponent();

            this.appointmentView = appointmentView;

            appointmentViewItems = AppointmentViewItem.GetByAppointmentView(appointmentView.Id).ToList();
        }

        private void FormAppointmentViewEdit_Load(object sender, EventArgs e)
        {
            descriptionTextBox.Text = appointmentView.Description;

            if (appointmentView.RowsPerIncrement == 0)
            {
                rowsPerIncrementTextBox.Text = "1";
            }
            else
            {
                rowsPerIncrementTextBox.Text = appointmentView.RowsPerIncrement.ToString();
            }

            onlyShowScheduledProvidersCheckBox.Checked = appointmentView.OnlyScheduledProviders;
            if (appointmentView.OnlyScheduleBefore > new TimeSpan(0, 0, 0))
            {
                timeBeforeTextBox.Text = appointmentView.OnlyScheduleBefore.ToString(@"hh\:mm");
            }
            if (appointmentView.OnlyScheduleAfter > new TimeSpan(0, 0, 0))
            {
                timeAfterTextBox.Text = appointmentView.OnlyScheduleAfter.ToString(@"hh\:mm");
            }

            FillClinics();

            UpdateDisplayFilterGroup();

            displayedFields.AddRange(appointmentViewItems.FindAll(x => !x.OperatoryId.HasValue && !x.ProviderId.HasValue));

            FillOperatories();
            FillProviders();

            upperRightStackBehaviourComboBox.SelectedIndex = (int)appointmentView.StackBehaviourUR;
            lowerRightStackBehaviourComboBox.SelectedIndex = (int)appointmentView.StackBehaviourLR;

            scrollStartDynamicCheckBox.Checked = appointmentView.ScrollStartDynamic;
            scrollStartTimeTextBox.Text = appointmentView.ScrollStartTime.ToString(@"hh\:mm");
            hideAppointmentBubblesCheckBox.Checked = appointmentView.HideAppointmentBubbles;

            if (appointmentView.IsNew)
            {
                hideAppointmentBubblesCheckBox.Checked = Preference.GetBool(PreferenceName.AppointmentBubblesDisabled);
            }

            FillElements();
        }

        private void FillClinics()
        {
            clinicComboBox.Items.Clear();

            foreach (var clinic in Clinic.GetByUser(Security.CurrentUser))
            {
                clinicComboBox.Items.Add(clinic);

                if (Clinics.ClinicId == clinic.Id)
                {
                    clinicComboBox.SelectedItem = clinic;
                }
            }
        }

        private void FillProviders()
        {
            providersListBox.Items.Clear();

            var providers = Providers.GetDeepCopy(true);

            foreach (Provider provider in providers)
            {
                int index = providersListBox.Items.Add(provider);

                if (appointmentViewItems.Select(x => x.ProviderId).Contains(provider.ProvNum))
                {
                    providersListBox.SetSelected(index, true);
                }
            }
        }

        private void FillOperatories()
        {
            operatoriesListBox.Items.Clear();

            var operatories = Operatory.All();

            foreach (var operatory in operatories)
            {
                if (operatory.ClinicId == SelectedClinicId)
                {
                    var index = operatoriesListBox.Items.Add(operatory);

                    if (appointmentViewItems.Select(x => x.OperatoryId).Contains(operatory.Id))
                    {
                        operatoriesListBox.SetSelected(index, true);
                    }
                }
            }
        }

        private void FillElements()
        {
            void AddAppointmentViewItemToListBox(AppointmentViewItem appointmentViewItem, ListBox listBox)
            {
                if (appointmentViewItem.AppointmentFieldDefinitionId.HasValue)
                {
                    listBox.Items.Add(new AppointmentViewItem
                    {
                        Description = AppointmentFieldDefinition.GetFieldName(appointmentViewItem.AppointmentFieldDefinitionId.Value),
                        AppointmentFieldDefinitionId = appointmentViewItem.AppointmentFieldDefinitionId
                    });
                }
                else if (appointmentViewItem.PatientFieldDefinitionId.HasValue)
                {
                    listBox.Items.Add(new AppointmentViewItem
                    {
                        Description = PatFieldDefs.GetFieldName(appointmentViewItem.PatientFieldDefinitionId.Value),
                        PatientFieldDefinitionId = appointmentViewItem.PatientFieldDefinitionId
                    });
                }
                else
                {
                    listBox.Items.Add(appointmentViewItem);
                }

                //if (item.Description == "MedOrPremed[+]" || 
                //    item.Description == "HasIns[I]" || 
                //    item.Description == "InsToSend[!]" || 
                //    item.Description == "LateColor")
                //{
                //    row.BackColor = item.Color;
                //}
                //else
                //{
                //    row.ColorText = item.Color;
                //}
            }

            foreach (var appointmentViewItem in displayedFields)
            {
                switch (appointmentViewItem.Alignment)
                {
                    case AppointmentViewLocation.Main:
                        AddAppointmentViewItemToListBox(appointmentViewItem, fieldsMainListBox);
                        break;

                    case AppointmentViewLocation.UpperRight:
                        AddAppointmentViewItemToListBox(appointmentViewItem, fieldsUpperRightListBox);
                        break;

                    case AppointmentViewLocation.LowerRight:
                        AddAppointmentViewItemToListBox(appointmentViewItem, fieldsLowerRightListBox);
                        break;
                }
            }

            fieldsAvailableTreeView.BeginUpdate();
            fieldsAvailableTreeView.Nodes.Clear();

            treeNodeFields = fieldsAvailableTreeView.Nodes.Add("Fields");
            treeNodeAppointmentFields = fieldsAvailableTreeView.Nodes.Add("Appointment Fields");
            treeNodePatientFields = fieldsAvailableTreeView.Nodes.Add("Patient Fields");

            foreach (var fieldName in fieldNames)
            {
                if (!ElementIsDisplayed(fieldName))
                {
                    var treeNode = treeNodeFields.Nodes.Add(fieldName);

                    treeNode.Tag = new AppointmentViewItem
                    {
                        Description = fieldName
                    };
                }
            }

            var appointmentFieldDefinitions = AppointmentFieldDefinition.All();
            foreach (var appointmentFieldDefinition in appointmentFieldDefinitions)
            {
                if (!IsAppointmentFieldDisplayed(appointmentFieldDefinition.Id))
                {
                    var treeNode = treeNodeAppointmentFields.Nodes.Add(appointmentFieldDefinition.FieldName);

                    treeNode.Tag = new AppointmentViewItem
                    {
                        Description = appointmentFieldDefinition.FieldName,
                        AppointmentFieldDefinitionId = appointmentFieldDefinition.Id
                    };
                }
            }

            var patientFieldDefinitions = PatFieldDefs.GetDeepCopy(true);
            foreach (var patientFieldDefinition in patientFieldDefinitions)
            {
                if (!IsPatientFieldDisplayed(patientFieldDefinition.PatFieldDefNum))
                {
                    var treeNode = treeNodeAppointmentFields.Nodes.Add(patientFieldDefinition.FieldName);

                    treeNode.Tag = new AppointmentViewItem
                    {
                        Description = patientFieldDefinition.FieldName,
                        PatientFieldDefinitionId = patientFieldDefinition.PatFieldDefNum
                    };
                }
            }

            fieldsAvailableTreeView.ExpandAll();
            fieldsAvailableTreeView.EndUpdate();
        }



        private bool ElementIsDisplayed(string fieldName)
        {
            foreach (var item in displayedFields)
            {
                if (!item.AppointmentFieldDefinitionId.HasValue && 
                    !item.PatientFieldDefinitionId.HasValue && 
                    item.Description == fieldName)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAppointmentFieldDisplayed(long appointmentFieldDefinitionId)
        {
            foreach (var item in displayedFields)
            {
                if (item.AppointmentFieldDefinitionId == appointmentFieldDefinitionId)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsPatientFieldDisplayed(long patientFieldDefinitionId)
        {
            foreach (var item in displayedFields)
            {
                if (item.PatientFieldDefinitionId == patientFieldDefinitionId)
                {
                    return true;
                }
            }
            return false;
        }

        private void OnlyShowScheduledProvidersCheckBox_Click(object sender, EventArgs e)
        {
            UpdateDisplayFilterGroup();
        }

        private void UpdateDisplayFilterGroup()
        {
            if (onlyShowScheduledProvidersCheckBox.Checked)
            {
                timeBeforeLabel.Visible = true;
                timeAfterLabel.Visible = true;
                timeBeforeTextBox.Visible = true;
                timeAfterTextBox.Visible = true;
            }
            else
            {
                timeBeforeLabel.Visible = false;
                timeAfterLabel.Visible = false;
                timeBeforeTextBox.Visible = false;
                timeAfterTextBox.Visible = false;
            }
        }

        private void FieldsAvailableTreeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var hitTestInfo = fieldsAvailableTreeView.HitTest(e.Location);
            if (hitTestInfo.Node == null)
            {
                return;
            }

            fieldsAvailableTreeView.SelectedNode = hitTestInfo.Node;

            MoveRightButton_Click(this, EventArgs.Empty);
        }

        private void MoveLeftButton_Click(object sender, EventArgs e)
        {
            void MoveSelectedItemToTreeView(ListBox listbox)
            {
                if (listbox.SelectedIndices.Count > 0)
                {
                    listbox.BeginUpdate();

                    var itemsToRemove = new List<AppointmentViewItem>();
                    foreach (AppointmentViewItem appointmentViewItem in listbox.SelectedItems)
                    {
                        var rootTreeNode = treeNodeFields;
                        if (appointmentViewItem.AppointmentFieldDefinitionId.HasValue)
                        {
                            rootTreeNode = treeNodeAppointmentFields;
                        }
                        else if (appointmentViewItem.PatientFieldDefinitionId.HasValue)
                        {
                            rootTreeNode = treeNodePatientFields;
                        }

                        var treeNode = rootTreeNode.Nodes.Add(appointmentViewItem.Description);
                        treeNode.Tag = appointmentViewItem;

                        itemsToRemove.Add(appointmentViewItem);
                    }

                    itemsToRemove.ForEach(item => fieldsMainListBox.Items.Remove(item));

                    listbox.EndUpdate();
                }
            }

            fieldsAvailableTreeView.BeginUpdate();

            MoveSelectedItemToTreeView(fieldsMainListBox);
            MoveSelectedItemToTreeView(fieldsUpperRightListBox);
            MoveSelectedItemToTreeView(fieldsLowerRightListBox);

            fieldsAvailableTreeView.EndUpdate();
        }

        private void MoveRightButton_Click(object sender, EventArgs e)
        {
            if (fieldsAvailableTreeView.SelectedNode == null ||
                fieldsAvailableTreeView.SelectedNode.Level == 0) return;

            if (fieldsAvailableTreeView.SelectedNode.Tag is AppointmentViewItem appointmentViewItem)
            {
                var targetListBox = fieldsMainListBox;
                switch (appointmentViewItem.Alignment)
                {
                    case AppointmentViewLocation.UpperRight:
                        targetListBox = fieldsUpperRightListBox;
                        break;

                    case AppointmentViewLocation.LowerRight:
                        targetListBox = fieldsLowerRightListBox;
                        break;
                }

                if (targetListBox.SelectedIndex != -1)
                {
                    targetListBox.Items.Insert(
                        fieldsMainListBox.SelectedIndex, 
                        appointmentViewItem);
                }
                else
                {
                    targetListBox.Items.Add(appointmentViewItem);
                }

                fieldsAvailableTreeView.SelectedNode.Remove();
            }
        }

        private void MoveUpButton_Click(object sender, EventArgs e)
        {
            void MoveItemsUp(ListBox listBox)
            {
                var oldIndex = listBox.SelectedIndex;
                if (oldIndex > 0)
                {
                    var selectedItem = listBox.Items[oldIndex];

                    var newIndex = listBox.SelectedIndex - 1;
                    listBox.Items.RemoveAt(oldIndex);
                    listBox.Items.Insert(newIndex, selectedItem);

                    listBox.SelectedItem = selectedItem;
                }
            }

            MoveItemsUp(fieldsMainListBox);
            MoveItemsUp(fieldsUpperRightListBox);
            MoveItemsUp(fieldsLowerRightListBox);
        }

        private void MoveDownButton_Click(object sender, EventArgs e)
        {
            void MoveItemsDown(ListBox listBox)
            {
                var oldIndex = listBox.SelectedIndex;
                if (oldIndex != -1 && oldIndex < (listBox.Items.Count - 1))
                {
                    var selectedItem = listBox.Items[oldIndex];

                    var newIndex = listBox.SelectedIndex + 1;
                    listBox.Items.RemoveAt(oldIndex);
                    listBox.Items.Insert(newIndex, selectedItem);

                    listBox.SelectedItem = selectedItem;
                }
            }

            MoveItemsDown(fieldsMainListBox);
            MoveItemsDown(fieldsUpperRightListBox);
            MoveItemsDown(fieldsLowerRightListBox);
        }

        private void FieldsListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (sender is ListBox sourceListBox)
            {
                var index = sourceListBox.IndexFromPoint(e.Location);
                if (index == -1)
                {
                    return;
                }

                sourceListBox.SelectedIndex = index;

                if (sourceListBox.Items[index] is AppointmentViewItem item)
                {
                    using (var formAppointmentViewItemEdit = new FormAppointmentViewItemEdit(item))
                    {
                        if (formAppointmentViewItemEdit.ShowDialog(this) == DialogResult.Cancel)
                        {
                            return;
                        }
                    }

                    var targetListBox = fieldsMainListBox;
                    switch (item.Alignment)
                    {
                        case AppointmentViewLocation.UpperRight:
                            targetListBox = fieldsUpperRightListBox;
                            break;

                        case AppointmentViewLocation.LowerRight:
                            targetListBox = fieldsLowerRightListBox;
                            break;
                    }

                    if (targetListBox!= sourceListBox)
                    {
                        sourceListBox.Items.Remove(item);

                        targetListBox.Items.Add(item);
                        targetListBox.SelectedItem = item;
                        targetListBox.Focus();
                    }
                }
            }
        }

        private void FieldsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ListBox sourceListBox)
            {
                if (sourceListBox != fieldsMainListBox)
                    fieldsMainListBox.ClearSelected();

                if (sourceListBox != fieldsUpperRightListBox)
                    fieldsUpperRightListBox.ClearSelected();

                if (sourceListBox != fieldsLowerRightListBox)
                    fieldsLowerRightListBox.ClearSelected();
            }
        }

        private void RowsPerIncrementTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (!int.TryParse(rowsPerIncrementTextBox.Text, out var rowsPerIncrement) || rowsPerIncrement < 1 || rowsPerIncrement > 3)
            {
                MessageBox.Show(
                    "Rows per increment must be a number between 1 and 3.", 
                    "Appointment View",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                e.Cancel = true;
            }
        }

        private void ClinicComboBox_SelectionChangeCommitted(object sender, EventArgs e) => FillOperatories();

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (providersListBox.SelectedIndices.Count == 0)
            {
                MessageBox.Show(
                    "At least one provider must be selected.",
                    "Appointment View",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (operatoriesListBox.SelectedIndices.Count == 0)
            {
                MessageBox.Show(
                    "At least one operatory must be selected.",
                    "Appointment View",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            var description = descriptionTextBox.Text.Trim();
            if (description.Length == 0)
            {
                MessageBox.Show(
                    "A description must be entered.",
                    "Appointment View",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (fieldsMainListBox.Items.Count == 0)
            {
                MessageBox.Show(
                    "At least one row type must be displayed.",
                    "Appointment View",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            var timeBefore = TimeSpan.Zero;
            if (onlyShowScheduledProvidersCheckBox.Checked && timeBeforeTextBox.Text != "")
            {
                try
                {
                    timeBefore = TimeSpan.Parse(timeBeforeTextBox.Text);
                }
                catch
                {
                    MessageBox.Show(
                        "Time before invalid.",
                        "Appointment View",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }

            var timeAfter = TimeSpan.Zero;
            if (onlyShowScheduledProvidersCheckBox.Checked && timeAfterTextBox.Text != "")
            {
                try
                {
                    timeAfter = TimeSpan.Parse(timeAfterTextBox.Text);
                }
                catch
                {
                    MessageBox.Show(
                        "Time after invalid.",
                        "Appointment View",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }

            DateTime scrollStartTime;
            if (scrollStartTimeTextBox.Text == "") scrollStartTime = DateTime.Parse("08:00:00");
            else
            {
                try
                {
                    scrollStartTime = DateTime.Parse(scrollStartTimeTextBox.Text);
                }
                catch
                {
                    MessageBox.Show(
                        "Scroll start time invalid.",
                        "Appointment View",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }

            appointmentView.StackBehaviourUR = (AppointmentViewStackBehaviour)upperRightStackBehaviourComboBox.SelectedIndex;
            appointmentView.StackBehaviourLR = (AppointmentViewStackBehaviour)lowerRightStackBehaviourComboBox.SelectedIndex;
            appointmentView.Description = description;
            appointmentView.RowsPerIncrement = int.Parse(rowsPerIncrementTextBox.Text);
            appointmentView.OnlyScheduledProviders = onlyShowScheduledProvidersCheckBox.Checked;
            appointmentView.OnlyScheduleBefore = timeBefore;
            appointmentView.OnlyScheduleAfter = timeAfter;
            appointmentView.ScrollStartDynamic = scrollStartDynamicCheckBox.Checked;
            appointmentView.ScrollStartTime = scrollStartTime.TimeOfDay;
            appointmentView.HideAppointmentBubbles = hideAppointmentBubblesCheckBox.Checked;
            appointmentView.ClinicId = SelectedClinicId;

            if (appointmentView.IsNew)
            {
                AppointmentView.Insert(appointmentView);
            }
            else
            {
                AppointmentView.Update(appointmentView);
                AppointmentViewItem.DeleteForAppointmentView(appointmentView.Id);
            }

            foreach (Operatory operatory in operatoriesListBox.SelectedItems)
            {
                AppointmentViewItem.Insert(new AppointmentViewItem
                {
                    AppointmentViewId = appointmentView.Id,
                    OperatoryId = operatory.Id
                });
            }

            foreach (Provider provider in providersListBox.SelectedItems)
            {
                AppointmentViewItem.Insert(new AppointmentViewItem
                {
                    AppointmentViewId = appointmentView.Id,
                    ProviderId = provider.ProvNum
                });
            }

            void InsertItemsFromListBox(ListBox listBox)
            {
                for (int i = 0; i < listBox.Items.Count; i++)
                {
                    if (listBox.Items[i] is AppointmentViewItem item)
                    {
                        item.AppointmentViewId = appointmentView.Id;
                        item.Order = i;

                        AppointmentViewItem.Insert(item);
                    }
                }
            }

            InsertItemsFromListBox(fieldsMainListBox);
            InsertItemsFromListBox(fieldsUpperRightListBox);
            InsertItemsFromListBox(fieldsLowerRightListBox);

            DialogResult = DialogResult.OK;
        }
    }
}
