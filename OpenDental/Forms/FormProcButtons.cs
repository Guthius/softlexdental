using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormProcButtons : FormBase
    {
        bool changed;
        long selectedCat;
        ProcButton[] ButtonList;
        List<ProcButtonQuick> procButtonsList;
        List<Definition> procButtonCategoryDefsList;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormProcButtons"/> class.
        /// </summary>
        public FormProcButtons() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormChartProcedureEntry_Load(object sender, EventArgs e)
        {
            LoadQuickButtons();
            LoadCategories();
            LoadButtons();
            SetVisibility();
        }

        void SetVisibility()
        {
            foreach (Control control in Controls) control.Visible = true;
            
            if (categoriesListBox.SelectedIndex == 0)
            {
                buttonsListView.Visible = false;
                addButton.Visible = false;
                deleteButton.Visible = false;
                upButton.Visible = false;
                downButton.Visible = false;
            }
            else
            {
                buttonsPanel.Visible = false;
                labelEdit.Visible = false;
            }
        }

        /// <summary>
        /// Loads the list of quick buttons.
        /// </summary>
        void LoadQuickButtons()
        {
            buttonsPanel.Items.Clear();
            procButtonsList = ProcButtonQuicks.GetAll();
            procButtonsList.Sort(ProcButtonQuicks.sortYX);
 
            for (int i = 0; i < procButtonsList.Count; i++)
            {
                var panelItem = new ODButtonPanelItem
                {
                    Text = procButtonsList[i].Description,
                    Row = procButtonsList[i].YPos,
                    Order = procButtonsList[i].ItemOrder,
                    Type = procButtonsList[i].IsLabel ? ODButtonPanelItemType.Label : ODButtonPanelItemType.Button
                };
                panelItem.Tags.Add(procButtonsList[i]);
                buttonsPanel.Items.Add(panelItem);
            }
        }

        /// <summary>
        /// Loads the button categories.
        /// </summary>
        void LoadCategories()
        {
            ProcButtonQuicks.ValidateAll();
            categoriesListBox.Items.Clear();
            categoriesListBox.Items.Add("Quick Buttons");

            procButtonCategoryDefsList = Definition.GetByCategory(DefinitionCategory.ProcButtonCats);;

            if (procButtonCategoryDefsList.Count == 0)
            {
                selectedCat = 0;
                categoriesListBox.SelectedIndex = 0;
                return;
            }

            for (int i = 0; i < procButtonCategoryDefsList.Count; i++)
            {
                categoriesListBox.Items.Add(procButtonCategoryDefsList[i].Description);
                if (selectedCat == procButtonCategoryDefsList[i].Id)
                {
                    categoriesListBox.SelectedIndex = i + 1;
                }
            }

            if (categoriesListBox.SelectedIndex == -1)
            {
                categoriesListBox.SelectedIndex = 0;
                selectedCat = 0;
            }

            if (categoriesListBox.SelectedIndex > 0)
            {
                selectedCat = procButtonCategoryDefsList[categoriesListBox.SelectedIndex - 1].Id;
            }
        }

        /// <summary>
        /// Loads buttons and populates the list.
        /// </summary>
        void LoadButtons()
        {
            buttonsListView.Items.Clear();
            imageListProcButtons.Images.Clear();
            if (selectedCat == 0)
            {
                //empty button list and return because we will be using and OD grid to display these buttons.
                ButtonList = new ProcButton[0];
                return;
            }

            ProcButtons.RefreshCache();

            ButtonList = ProcButtons.GetForCat(selectedCat);
            for (int i = 0; i < ButtonList.Length; i++)
            {
                if (ButtonList[i].ItemOrder != i)
                {
                    ButtonList[i].ItemOrder = i;
                    ProcButtons.Update(ButtonList[i]);
                }
            }

            for (int i = 0; i < ButtonList.Length; i++)
            {
                if (ButtonList[i].ButtonImage != "")
                {
                    try
                    {
                        imageListProcButtons.Images.Add(ButtonList[i].ProcButtonNum.ToString(), PIn.Bitmap(ButtonList[i].ButtonImage));
                    }
                    catch
                    {
                        imageListProcButtons.Images.Add(new Bitmap(20, 20));
                    }
                }

                buttonsListView.Items.Add(
                    new ListViewItem(
                        new string[] {
                            ButtonList[i].Description
                        },
                        ButtonList[i].ProcButtonNum.ToString()));
            }
        }

        /// <summary>
        /// Reloads the list of buttons to show the buttons in the selected category.
        /// </summary>
        void categoriesListBox_Click(object sender, EventArgs e)
        {
            if (categoriesListBox.SelectedIndex == -1) return;

            SetVisibility();

            selectedCat =
                categoriesListBox.SelectedIndex > 0 ?
                     procButtonCategoryDefsList[categoriesListBox.SelectedIndex - 1].Id :
                     0;

            LoadButtons();
        }

        /// <summary>
        /// Opens the form to edit the button categories.
        /// </summary>
        void EditButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.Setup)) return;

            using (var formDefinitions = new FormDefinitions(DefinitionCategory.ProcButtonCats))
            {
                formDefinitions.ShowDialog();
            }

            LoadCategories();
            LoadButtons();
        }

        /// <summary>
        /// Opens the form to edit a button when the user double clicks on a button in the listview.
        /// </summary>
        void buttonsListView_DoubleClick(object sender, EventArgs e)
        {
            if (buttonsListView.SelectedIndices.Count == 0) return;

            var procButton = ButtonList[buttonsListView.SelectedIndices[0]].Copy();

            using (var formProcButtonEdit = new FormProcButtonEdit(procButton))
            {
                formProcButtonEdit.ShowDialog();

                changed = true;

                LoadButtons();
            }
        }

        /// <summary>
        /// Moves the selected button up by one in the list.
        /// </summary>
        void UpButton_Click(object sender, EventArgs e)
        {
            if (buttonsListView.SelectedIndices.Count == 0 ||
                buttonsListView.SelectedIndices[0] == 0)
                return;

            var procButton = ButtonList[buttonsListView.SelectedIndices[0]].Copy();
            procButton.ItemOrder--;
            ProcButtons.Update(procButton);

            int selected = procButton.ItemOrder;
            procButton = ButtonList[buttonsListView.SelectedIndices[0] - 1].Copy();
            procButton.ItemOrder++;
            ProcButtons.Update(procButton);

            LoadButtons();

            changed = true;

            buttonsListView.SelectedIndices.Clear();
            buttonsListView.SelectedIndices.Add(selected);
        }

        /// <summary>
        /// Moves the selected button down by one in the list.
        /// </summary>
        void DownButton_Click(object sender, EventArgs e)
        {
            if (buttonsListView.SelectedIndices.Count == 0 ||
                buttonsListView.SelectedIndices[0] == buttonsListView.Items.Count - 1)
                return;

            var procButton = ButtonList[buttonsListView.SelectedIndices[0]].Copy();
            procButton.ItemOrder++;
            ProcButtons.Update(procButton);

            int selected = procButton.ItemOrder;
            procButton = ButtonList[buttonsListView.SelectedIndices[0] + 1].Copy();
            procButton.ItemOrder--;
            ProcButtons.Update(procButton);

            LoadButtons();

            changed = true;

            buttonsListView.SelectedIndices.Clear();
            buttonsListView.SelectedIndices.Add(selected);
        }

        /// <summary>
        /// Opens the form to add a new button.
        /// </summary>
        void AddButton_Click(object sender, EventArgs e)
        {
            if (categoriesListBox.SelectedIndex == -1) return;
            
            var procButton = new ProcButton
            {
                Category = selectedCat,
                ItemOrder = buttonsListView.Items.Count
            };

            using (var formProcButtonEdit = new FormProcButtonEdit(procButton))
            {
                formProcButtonEdit.IsNew = true;
                formProcButtonEdit.ShowDialog();

                changed = true;

                LoadButtons();
            }
        }

        /// <summary>
        /// Deletes the selected button.
        /// </summary>
        void DeleteButton_Click(object sender, EventArgs e)
        {
            if (buttonsListView.SelectedIndices.Count == 0)
            {
                MessageBox.Show(
                    "Please select an item first.",
                    "Procedure Buttons", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            ProcButtons.Delete(ButtonList[buttonsListView.SelectedIndices[0]]);

            changed = true;

            LoadButtons();
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        void CloseButton_Click(object sender, EventArgs e) => Close();
        
        /// <summary>
        /// Invalidates the procedure buttons cache if changes were made and the form is closing.
        /// </summary>
        void FormProcButtons_Closing(object sender, CancelEventArgs e)
        {
            if (changed)
            {
                DataValid.SetInvalid(InvalidType.ProcButtons);
            }
        }

        void ButtonsPanel_RowMouseDoubleClick(object sender, ODButtonPanelRowMouseEventArgs e)
        {
            using (var formProcButtonQuickEdit = new FormProcButtonQuickEdit())
            {
                formProcButtonQuickEdit.IsNew = true;
                formProcButtonQuickEdit.pbqCur = new ProcButtonQuick();
                formProcButtonQuickEdit.pbqCur.YPos = e.Row;

                for (int i = 0; i < procButtonsList.Count; i++)
                {
                    if (formProcButtonQuickEdit.pbqCur.YPos != procButtonsList[i].YPos || 
                        formProcButtonQuickEdit.pbqCur.ItemOrder > procButtonsList[i].ItemOrder)
                    {
                        continue;
                    }
                    formProcButtonQuickEdit.pbqCur.ItemOrder = procButtonsList[i].ItemOrder + 1;//new PBQ should have the highest item order in the row.
                }

                if (formProcButtonQuickEdit.ShowDialog() == DialogResult.OK)
                {
                    LoadQuickButtons();
                } 
            }
        }

        void ButtonsPanel_ItemMouseDoubleClick(object sender, ODButtonPanelItemMouseEventArgs e)
        {
            using (var formProcButtonQuickEdit = new FormProcButtonQuickEdit())
            {
                for (int i = 0; e.Item != null && i < e.Item.Tags.Count; i++)
                {
                    if (e.Item.Tags[i].GetType() == typeof(ProcButtonQuick))
                    {
                        formProcButtonQuickEdit.pbqCur = (ProcButtonQuick)e.Item.Tags[i];
                        break;
                    }
                }

                if (formProcButtonQuickEdit.pbqCur == null) return;

                if (formProcButtonQuickEdit.ShowDialog() == DialogResult.OK)
                {
                    LoadQuickButtons();
                }
            }
        }
    }
}