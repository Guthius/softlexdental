using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormUserPick : FormBase
    {
        List<User> usersList;

        public List<User> ListUserodsFiltered;
        public long SelectedUserNum;
        public List<long> SelectedUserNumsList = new List<long>();
        public long SuggestedUserNum = 0;
        public List<long> SuggestedUserNumsList = new List<long>();
        public bool IsSelectionmode;
        public bool IsShowAllAllowed;
        public bool IsPickNoneAllowed;
        public bool IsPickAllAllowed;

        bool isMultiSelect;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormUserPick"/> class.
        /// </summary>
        /// <param name="isMultiSelect"></param>
        public FormUserPick(bool isMultiSelect = false)
        {
            InitializeComponent();

            this.isMultiSelect = isMultiSelect;
        }

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormUserPick_Load(object sender, EventArgs e)
        {
            showButton.Text = Translation.Language.ShowAll;
            if (IsShowAllAllowed && ListUserodsFiltered != null && ListUserodsFiltered.Count > 0)
            {
                showButton.Visible = true;
            }

            allButton.Visible = IsPickAllAllowed;
            noneButton.Visible = IsPickNoneAllowed;

            if (isMultiSelect)
            {
                userListBox.SelectionMode = SelectionMode.MultiExtended;
                Text = Translation.Language.PickUsers;
            }

            ShowUsers(ListUserodsFiltered);
        }

        /// <summary>
        /// Populates the users listbox with the specified users.
        /// </summary>
        /// <param name="usersList">The list of users.</param>
        void ShowUsers(List<User> usersList)
        {
            this.usersList = usersList.Select(x => x.Copy()).ToList();
            this.usersList.ForEach(x => userListBox.Items.Add(x));

            if (isMultiSelect)
            {
                foreach (long userNum in SuggestedUserNumsList)
                {
                    int index = usersList.FindIndex(x => x.Id == userNum);

                    userListBox.SetSelected(index, true);
                }
            }
            else
            {
                userListBox.SelectedIndex = usersList.FindIndex(x => x.Id == SuggestedUserNum);
            }
        }

        /// <summary>
        /// Selects a user when the user double clicks on a user in the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void UserListBox_DoubleClick(object sender, EventArgs e)
        {
            if (userListBox.SelectedIndex == -1)  return;

            AcceptButton_Click(sender, e);
        }

        /// <summary>
        /// Toggles between showing all users and filtered users.
        /// </summary>
        void Showbutton_Click(object sender, EventArgs e)
        {
            SelectedUserNum = 0;

            if (Text == Translation.Language.ShowAll)
            {
                Text = Translation.Language.ShowFiltered;
                ShowUsers(Userods.GetDeepCopy());
            }
            else
            {
                Text = Translation.Language.ShowAll;
                ShowUsers(ListUserodsFiltered);
            }
        }

        /// <summary>
        /// Select all users and close the form.
        /// </summary>
        void AllButton_Click(object sender, EventArgs e)
        {
            SelectedUserNum = -1;
            SelectedUserNumsList = usersList.Select(x => x.Id).ToList();

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Closes the form without selecting a user.
        /// </summary>
        void NoneButton_Click(object sender, EventArgs e)
        {
            SelectedUserNum = 0;
            SelectedUserNumsList = new List<long>() { };

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        void AcceptButton_Click(object sender, EventArgs e)
        {
            if (userListBox.SelectedIndex == -1)
            {
                MessageBox.Show(
                    Translation.Language.PleasePickAUserFirst,
                    Text, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            if (!IsSelectionmode && !Security.IsAuthorized(Permissions.TaskEdit, true) && Userods.GetInbox(usersList[userListBox.SelectedIndex].Id) != 0)
            {
                MessageBox.Show(
                    Translation.Language.PleaseSelectAUserThatDoesNotHaveAnInbox,
                    Text, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            SelectedUserNum = usersList[userListBox.SelectedIndex].Id;
            foreach (int index in userListBox.SelectedIndices)
            {
                SelectedUserNumsList.Add(usersList[index].Id);
            }

            DialogResult = DialogResult.OK;
        }
    }
}