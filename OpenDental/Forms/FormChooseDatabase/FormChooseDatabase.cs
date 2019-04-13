using OpenDentBusiness;
using System;
using System.Windows.Forms;
using DataConnectionBase;

namespace OpenDental
{
    public partial class FormChooseDatabase : BaseFormChooseDatabase
    {

        public FormChooseDatabase()
        {
            InitializeComponent();
            Lan.F(this);
        }

        private void ChooseDatabaseView_Load(object sender, EventArgs e)
        {
            if (_model.IsAccessedFromMainMenu)
            {
                comboComputerName.Enabled = false;
                _model.CentralConnectionCur.ServerName = DataConnection.GetServerName();
                comboDatabase.Enabled = false;
                _model.CentralConnectionCur.DatabaseName = DataConnection.GetDatabaseName();
            }

            groupDirect.Enabled = true;

            comboComputerName.Text = _model.CentralConnectionCur.ServerName;
            comboDatabase.Text = _model.CentralConnectionCur.DatabaseName;
            textUser.Text = _model.CentralConnectionCur.MySqlUser;
            textPassword.Text = _model.CentralConnectionCur.MySqlPassword;
            textPassword.PasswordChar = (textPassword.Text == "" ? default(char) : '*');
            textConnectionString.Text = _model.ConnectionString;
            checkNoShow.Checked = (_model.NoShow == YN.Yes);
            checkDynamicMode.Checked = _model.UseDynamicMode;
        }

        public override bool TryGetModelFromView(out ChooseDatabaseModel model)
        {
            model = null;
            try
            {
                _model.CentralConnectionCur.ServerName = comboComputerName.Text;
                _model.CentralConnectionCur.DatabaseName = comboDatabase.Text;
                _model.CentralConnectionCur.MySqlUser = textUser.Text;
                _model.CentralConnectionCur.MySqlPassword = textPassword.Text;
                _model.NoShow = (checkNoShow.Checked ? YN.Yes : YN.No);
                _model.DbType = DatabaseType.MySql;
                _model.ConnectionString = textConnectionString.Text;
                _model.UseDynamicMode = checkDynamicMode.Checked;
            }
            catch (Exception)
            {
                return false;
            }
            model = _model.Copy();
            return true;
        }

        public void SetController(ChooseDatabaseController controller)
        {
            _controller = controller;
        }

        public void FillComboComputerNames(string[] arrayComputerNames)
        {
            comboComputerName.Items.Clear();
            comboComputerName.Items.AddRange(arrayComputerNames);
        }

        public void FillComboDatabases(string[] arrayDatabases)
        {
            comboDatabase.Items.Clear();
            comboDatabase.Items.AddRange(arrayDatabases);
        }

        private void comboDatabase_DropDown(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _controller.DatabaseDropDown(sender, e);
            Cursor = Cursors.Default;
        }

        private void textPassword_TextChanged(object sender, EventArgs e)
        {
            if (textPassword.Text == "")
            {
                textPassword.PasswordChar = default(char);//if text is cleared, turn off password char mask
            }
        }

        private void textPassword_Leave(object sender, EventArgs e)
        {
            textPassword.PasswordChar = (textPassword.Text == "" ? default(char) : '*');//mask password if loaded from the config file
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            if (!_controller.butOK_Click(sender, e))
            {
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void checkDynamicMode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkDynamicMode.Checked)
            {
                checkNoShow.Checked = false;
            }
            checkNoShow.Enabled = !checkDynamicMode.Checked;
        }
    }

    ///<summary>Required so that Visual Studio can design this form.  The designer does not allow directly extending classes with generics.</summary>
    public class BaseFormChooseDatabase : ODFormMVC<ChooseDatabaseModel, FormChooseDatabase, ChooseDatabaseController> { }
}