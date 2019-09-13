using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental
{

    public partial class FormTimeAdjustEdit : ODForm
    {


        ///<summary></summary>
        public bool IsNew;

        private TimeAdjustment TimeAdjustCur;

        ///<summary></summary>
        public FormTimeAdjustEdit(TimeAdjustment timeAdjustCur)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            TimeAdjustCur = timeAdjustCur;
        }


        private void FormTimeAdjustEdit_Load(object sender, System.EventArgs e)
        {
            if (TimeAdjustCur.IsAuto)
            {
                radioAuto.Checked = true;
            }
            else
            {
                radioManual.Checked = true;
            }
            textTimeEntry.Text = TimeAdjustCur.Date.ToString();
            if (TimeAdjustCur.HoursOvertime.TotalHours == 0)
            {
                textHours.Text = ClockEvent.Format(TimeAdjustCur.HoursRegular);
            }
            else
            {
                checkOvertime.Checked = true;
                textHours.Text = ClockEvent.Format(TimeAdjustCur.HoursOvertime);
            }
            textNote.Text = TimeAdjustCur.Note;
        }

        private void butDelete_Click(object sender, EventArgs e)
        {
            if (IsNew)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            TimeAdjustment.Delete(TimeAdjustCur);
            DialogResult = DialogResult.OK;
        }

        private void butOK_Click(object sender, System.EventArgs e)
        {
            if (!DateTime.TryParse(textTimeEntry.Text, out var date))
            {
                MsgBox.Show(this, "Please enter a valid Date/Time.");
                return;
            }

            if (!TimeSpan.TryParse(textHours.Text, out var hoursEntered))
            {
                MsgBox.Show(this, "Please enter valid Hours and Minutes.");
                return;
            }

            //end of validation
            TimeAdjustCur.IsAuto = radioAuto.Checked;
            TimeAdjustCur.Date = date;
 
            if (checkOvertime.Checked)
            {
                TimeAdjustCur.HoursRegular = -hoursEntered;
                TimeAdjustCur.HoursOvertime = hoursEntered;
            }
            else
            {
                TimeAdjustCur.HoursRegular = hoursEntered;
                TimeAdjustCur.HoursOvertime = TimeSpan.Zero;
            }
            TimeAdjustCur.Note = textNote.Text;
            if (IsNew)
            {
                TimeAdjustment.Insert(TimeAdjustCur);
            }
            else
            {
                TimeAdjustment.Update(TimeAdjustCur);
            }
            DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
