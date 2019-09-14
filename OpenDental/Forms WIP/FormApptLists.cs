using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormApptLists : FormBase
    {
        /// <summary>
        /// After this window closes, if dialog result is OK, this will contain which list was selected.
        /// </summary>
        public ApptListSelection SelectionResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormApptLists"/> class.
        /// </summary>
        public FormApptLists() => InitializeComponent();
        
        /// <summary>
        /// Loads the form.
        /// </summary>
        void FormApptLists_Load(object sender, System.EventArgs e) => Plugin.Trigger(this, "FormApptLists_Load");

        void recallsButton_Click(object sender, EventArgs e) => Close(ApptListSelection.Recall);

        void confirmationsButton_Click(object sender, EventArgs e) => Close(ApptListSelection.Confirm);

        void plannedTrackerButton_Click(object sender, EventArgs e) => Close(ApptListSelection.Planned);

        void unscheduledButton_Click(object sender, EventArgs e) => Close(ApptListSelection.Unsched);

        void asapButton_Click(object sender, EventArgs e) => Close(ApptListSelection.ASAP);

        void radiologyButton_Click(object sender, EventArgs e) => Close(ApptListSelection.Radiology);

        void insuranceVerifyButton_Click(object sender, EventArgs e) => Close(ApptListSelection.InsVerify);

        /// <summary>
        /// Closes the form using the specified selection result.
        /// </summary>
        /// <param name="apptListSelection"></param>
        void Close(ApptListSelection apptListSelection)
        {
            SelectionResult = apptListSelection;
            DialogResult = DialogResult.OK;
        }
    }

    /// <summary>
    /// Used in FormApptLists as the selection result.
    /// </summary>
    public enum ApptListSelection
    {
        Recall,
        Confirm,
        Planned,
        Unsched,
        ASAP,
        Radiology,
        InsVerify
    }
}