using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental.User_Controls.SetupWizard
{
    [ToolboxItem(false)]
    public partial class UserControlSetupWizEmployee : SetupWizardControl
    {
        List<Employee> employeesList;
        bool isChanged;

        public UserControlSetupWizEmployee() => InitializeComponent();

        void UserControlSetupWizEmployee_Load(object sender, EventArgs e)
        {
            LoadEmployees();

            if (employeesList.Where(x => x.FName.ToLower() != "default").ToList().Count == 0)
            {
                MessageBox.Show(
                    "You have no valid employees. Please click the 'Add' button to add an employee.", 
                    "Setup",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
        }

        void LoadEmployees()
        {
            employeesList = Employees.GetDeepCopy(true);
            Color colorNeedsAttn = OpenDental.SetupWizard.GetColor(ODSetupStatus.NeedsAttention);

            employeesGrid.BeginUpdate();
            employeesGrid.Columns.Clear();
            employeesGrid.Columns.Add(new ODGridColumn("Last Name", 135));
            employeesGrid.Columns.Add(new ODGridColumn("First Name", 135));
            employeesGrid.Columns.Add(new ODGridColumn("MI", 65));
            employeesGrid.Columns.Add(new ODGridColumn("Payroll ID", 105));
            employeesGrid.Rows.Clear();

            bool isAllComplete = true;
            if (employeesList.Where(x => x.FName.ToLower() != "default").ToList().Count == 0)
            {
                isAllComplete = false;
            }

            foreach (Employee emp in employeesList)
            {
                var row = new ODGridRow();

                row.Cells.Add(emp.LName);
                if (string.IsNullOrEmpty(emp.LName) || emp.LName.ToLower() == "default")
                {
                    row.Cells[row.Cells.Count - 1].CellColor = colorNeedsAttn;
                    isAllComplete = false;
                }

                row.Cells.Add(emp.FName);
                if (string.IsNullOrEmpty(emp.FName) || emp.FName.ToLower() == "default")
                {
                    row.Cells[row.Cells.Count - 1].CellColor = colorNeedsAttn;
                    isAllComplete = false;
                }

                row.Cells.Add(emp.MiddleI);
                row.Cells.Add(emp.PayrollID);
                row.Tag = emp;

                employeesGrid.Rows.Add(row);
            }
            employeesGrid.EndUpdate();

            IsDone = isAllComplete;
        }

        void EmployeesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            var employee = (Employee)employeesGrid.Rows[e.Row].Tag;

            using (var formEmployeeEdit = new FormEmployeeEdit())
            {
                formEmployeeEdit.EmployeeCur = employee;
                if (formEmployeeEdit.ShowDialog() == DialogResult.OK)
                {
                    Employees.RefreshCache();

                    LoadEmployees();

                    isChanged = true;
                }
            }
        }

        void AddButton_Click(object sender, EventArgs e)
        {
            using (var formEmployeeEdit = new FormEmployeeEdit())
            {
                formEmployeeEdit.IsNew = true;
                formEmployeeEdit.EmployeeCur = new Employee();

                if (formEmployeeEdit.ShowDialog() == DialogResult.OK)
                {
                    Employees.RefreshCache();

                    LoadEmployees();

                    isChanged = true;
                }
            }
        }

        void AdvancedButton_Click(object sender, EventArgs e)
        {
            using (var formEmployeeSelect = new FormEmployeeSelect())
            {
                formEmployeeSelect.ShowDialog(this);
            }

            LoadEmployees();
        }

        protected override void OnControlDone(EventArgs e)
        {
            if (isChanged)
            {
                DataValid.SetInvalid(InvalidType.Employees);
            }
        }
    }
}