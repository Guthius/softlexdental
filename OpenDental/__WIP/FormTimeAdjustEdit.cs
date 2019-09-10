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

            TimeAdjustCur = timeAdjustCur.Copy();
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
                textHours.Text = ClockEvents.Format(TimeAdjustCur.HoursRegular);
            }
            else
            {
                checkOvertime.Checked = true;
                textHours.Text = ClockEvents.Format(TimeAdjustCur.HoursOvertime);
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
            TimeAdjusts.Delete(TimeAdjustCur);
            DialogResult = DialogResult.OK;
        }

        private void butOK_Click(object sender, System.EventArgs e)
        {
            try
            {
                DateTime.Parse(textTimeEntry.Text);
            }
            catch
            {
                MsgBox.Show(this, "Please enter a valid Date/Time.");
                return;
            }
            try
            {
                if (textHours.Text.Contains(":"))
                {
                    ClockEvents.ParseHours(textHours.Text);
                }
                else
                {
                    Double.Parse(textHours.Text);
                }
            }
            catch
            {
                MsgBox.Show(this, "Please enter valid Hours and Minutes.");
                return;
            }
            //end of validation
            TimeAdjustCur.IsAuto = radioAuto.Checked;
            TimeAdjustCur.Date = DateTime.Parse(textTimeEntry.Text);
            TimeSpan hoursEntered;
            if (textHours.Text.Contains(":"))
            {
                hoursEntered = ClockEvents.ParseHours(textHours.Text);//we know this will work because we tested ParseHours above.
            }
            else
            {
                hoursEntered = TimeSpan.FromHours(Double.Parse(textHours.Text));
            }
            if (checkOvertime.Checked)
            {
                TimeAdjustCur.HoursRegular = -hoursEntered;
                TimeAdjustCur.HoursOvertime = hoursEntered;
            }
            else
            {
                TimeAdjustCur.HoursRegular = hoursEntered;
                TimeAdjustCur.HoursOvertime = TimeSpan.FromHours(0);
            }
            TimeAdjustCur.Note = textNote.Text;
            if (IsNew)
            {
                TimeAdjusts.Insert(TimeAdjustCur);
            }
            else
            {
                TimeAdjusts.Update(TimeAdjustCur);
            }
            DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
