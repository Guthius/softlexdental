using System;
using System.ServiceProcess;
using System.Windows.Forms;

namespace ServiceManager
{
    public partial class FormMain : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormMain"/> class.
        /// </summary>
        public FormMain() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormMain_Load(object sender, EventArgs e) => LoadServices();

        /// <summary>
        /// Populates the services list.
        /// </summary>
        void LoadServices()
        {
            servicesListBox.Items.Clear();

            var services = ServiceController.GetServices();
            foreach (var service in services)
            {
                if (service.ServiceName.StartsWith("OpenDent"))
                {
                    servicesListBox.Items.Add(service.ServiceName);
                }
            }
        }

        /// <summary>
        /// Opens the details of the selected service.
        /// </summary>
        void servicesListBox_DoubleClick(object sender, EventArgs e)
        {
            if (servicesListBox.SelectedIndex == -1) return;

            using (var formServiceManage = new FormServiceManage(servicesListBox.SelectedItem.ToString(), false))
            {
                formServiceManage.ShowInTaskbar = false;
                formServiceManage.StartPosition = FormStartPosition.CenterParent;
                if (formServiceManage.ShowDialog(this) == DialogResult.OK)
                {
                    LoadServices();
                }
            }
        }

        /// <summary>
        /// Opens the form to create a new service.
        /// </summary>
        void addButton_Click(object sender, EventArgs e)
        {
            using (var formServiceManage = new FormServiceManage("OpenDent", true))
            {
                formServiceManage.ShowInTaskbar = false;
                formServiceManage.StartPosition = FormStartPosition.CenterParent;
                if (formServiceManage.ShowDialog(this) == DialogResult.OK)
                {
                    LoadServices();
                }
            }
        }
    }
}
