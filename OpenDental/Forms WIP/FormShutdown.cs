using OpenDentBusiness;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormShutdown : FormBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether a update triggered this form.
        /// </summary>
        public bool IsUpdate { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormShutdown"/> class.
        /// </summary>
        public FormShutdown() => InitializeComponent();

        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormShutdown_Load(object sender, EventArgs e)
        {
            var computersList = Computer.AllActive().Select(x => x.Name).ToList();
            for (int i = 0; i < computersList.Count; i++)
            {
                workstationList.Items.Add(computersList[i]);
            }

            if (IsUpdate) shutdownButton.Text = "Continue";
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        void ShutdownButton_Click(object sender, EventArgs e)
        {
            if (IsUpdate)
            {
                DialogResult = DialogResult.OK;
                return;
            }

            var result =
                MessageBox.Show(
                    "Shutdown this program on all workstations except this one?  Users will be given a 15 second warning to save data.", 
                    "Shutdown", 
                    MessageBoxButtons.OKCancel, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            DialogResult = DialogResult.OK;
        }
    }
}