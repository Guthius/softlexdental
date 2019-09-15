using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormTimeCardManage : FormBase
    {
        private PayPeriod selectedPayPeriod;
        private int selectedIndex = 0;
        private readonly List<Employee> employees;
        private readonly List<Clinic> clinics;
        private List<PayPeriod> payPeriods;
        private TimeAdjustment timeAdjustNote;
        private int pagesPrinted;
        private bool headingPrinted;



        private DataTable MainTable;
        private string totalTime;
        private string overTime;
        private string rate2Time;
        private string totalTime2;
        private string overTime2;
        private string rate2Time2;
        



        public DateTime X_DateStart
        {
            get
            {
                if (DateTime.TryParse(dateStartTextBox.Text, out var dateStart))
                {
                    return dateStart;
                }
                return DateTime.MinValue;
            }
            set => dateStartTextBox.Text = value.ToShortDateString();
        }

        public DateTime X_DateEnd
        {
            get
            {
                if (DateTime.TryParse(dateEndTextBox.Text, out var dateEnd))
                {
                    return dateEnd;
                }
                return DateTime.MinValue;
            }
            set => dateEndTextBox.Text = value.ToShortDateString();
        }

        public FormTimeCardManage(List<Employee> employees)
        {
            InitializeComponent();

            this.employees = employees;
        }

        private void FormTimeCardManage_Load(object sender, EventArgs e)
        {
            selectedPayPeriod = PayPeriod.GetByDate(DateTime.UtcNow.Date);
            if (selectedPayPeriod == null)
            {
                MessageBox.Show(
                    "At least one pay period needs to exist before you can manage time cards.",
                    "Manage Time Cards", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.Cancel;
                return;
            }

            if (Preferences.HasClinicsEnabled)
            {
                clinicLabel.Visible = true;
                clinicComboBox.Visible = true;
                clinicComboBox.Items.Clear();

                if (!Security.CurrentUser.ClinicRestricted)
                {
                    clinicComboBox.Items.Add("All");
                    clinicComboBox.Items.Add("Headquarters");
                    clinicComboBox.SelectedIndex = 1;
                }

                var clinics = Clinics.GetForUserod(Security.CurrentUser);
                foreach (var clinic in clinics)
                {
                    int curIndex = clinicComboBox.Items.Add(clinic);
                    if (clinic.ClinicNum == Clinics.ClinicNum)
                    {
                        clinicComboBox.SelectedItem = clinic;
                    }
                }
            }

            payPeriods = PayPeriod.All();

            FillPayPeriod();
        }

        private void FormTimeCardManage_Shown(object sender, EventArgs e) => FillMain();

        private void FillMain()
        {
            long clinicId = 0;
            bool isAll = false;

            if (Preferences.HasClinicsEnabled)
            {
                if (Security.CurrentUser.ClinicRestricted)
                {
                    clinicId = clinics[clinicComboBox.SelectedIndex].ClinicNum;
                }
                else
                {//All and Headquarters are the first two available options.
                    if (clinicComboBox.SelectedIndex == 0)
                    {//All is selected.
                        isAll = true;
                    }
                    else if (clinicComboBox.SelectedIndex == 1)
                    {
                        //Do nothing since the defaults are this selection
                    }
                    else
                    {//A specific clinic was selected.
                        clinicId = clinics[clinicComboBox.SelectedIndex - 2].ClinicNum;//Subtract 2, because All and Headquarters are added to the list.
                    }
                }
            }
            else
            {
                isAll = true;
            }

            MainTable = 
                ClockEvent.GetTimeCardManage(
                    selectedPayPeriod.DateStart, 
                    selectedPayPeriod.DateEnd, 
                    clinicId, isAll);

            grid.BeginUpdate();
            grid.Columns.Clear();
            grid.Columns.Add(new ODGridColumn("Employee", 140));
            grid.Columns.Add(new ODGridColumn("Total Hrs", 75, HorizontalAlignment.Right));
            grid.Columns.Add(new ODGridColumn("Rate1", 75, HorizontalAlignment.Right));
            grid.Columns.Add(new ODGridColumn("Rate1 OT", 75, HorizontalAlignment.Right));
            grid.Columns.Add(new ODGridColumn("Rate2", 75, HorizontalAlignment.Right));
            grid.Columns.Add(new ODGridColumn("Rate2 OT", 75, HorizontalAlignment.Right));
            grid.Columns.Add(new ODGridColumn("Notes", 0));
            grid.Rows.Clear();

            for (int i = 0; i < MainTable.Rows.Count; i++)
            {
                var row = new ODGridRow();
                row.Cells.Add(MainTable.Rows[i]["lastName"] + ", " + MainTable.Rows[i]["firstName"]);
                if (Preference.GetBool(PreferenceName.TimeCardsUseDecimalInsteadOfColon))
                {
                    row.Cells.Add(PIn.Time(MainTable.Rows[i]["totalHours"].ToString()).TotalHours.ToString("n"));
                    row.Cells.Add(PIn.Time(MainTable.Rows[i]["rate1Hours"].ToString()).TotalHours.ToString("n"));
                    row.Cells.Add(PIn.Time(MainTable.Rows[i]["rate1OTHours"].ToString()).TotalHours.ToString("n"));
                    row.Cells.Add(PIn.Time(MainTable.Rows[i]["rate2Hours"].ToString()).TotalHours.ToString("n"));
                    row.Cells.Add(PIn.Time(MainTable.Rows[i]["rate2OTHours"].ToString()).TotalHours.ToString("n"));
                }
                else if (Preference.GetBool(PreferenceName.TimeCardShowSeconds))
                {//Colon format with seconds
                    row.Cells.Add(PIn.Time(MainTable.Rows[i]["totalHours"].ToString()).ToStringHmmss());
                    row.Cells.Add(PIn.Time(MainTable.Rows[i]["rate1Hours"].ToString()).ToStringHmmss());
                    row.Cells.Add(PIn.Time(MainTable.Rows[i]["rate1OTHours"].ToString()).ToStringHmmss());
                    row.Cells.Add(PIn.Time(MainTable.Rows[i]["rate2Hours"].ToString()).ToStringHmmss());
                    row.Cells.Add(PIn.Time(MainTable.Rows[i]["rate2OTHours"].ToString()).ToStringHmmss());
                }
                else
                {//Colon format without seconds
                    row.Cells.Add(PIn.Time(MainTable.Rows[i]["totalHours"].ToString()).ToStringHmm());
                    row.Cells.Add(PIn.Time(MainTable.Rows[i]["rate1Hours"].ToString()).ToStringHmm());
                    row.Cells.Add(PIn.Time(MainTable.Rows[i]["rate1OTHours"].ToString()).ToStringHmm());
                    row.Cells.Add(PIn.Time(MainTable.Rows[i]["rate2Hours"].ToString()).ToStringHmm());
                    row.Cells.Add(PIn.Time(MainTable.Rows[i]["rate2OTHours"].ToString()).ToStringHmm());
                }
                row.Cells.Add(MainTable.Rows[i]["Note"].ToString());
                grid.Rows.Add(row);
            }
            grid.EndUpdate();
        }

        private void FillPayPeriod()
        {
            X_DateStart = selectedPayPeriod.DateStart;
            X_DateEnd = selectedPayPeriod.DateEnd;

            datePaycheckTextBox.Text =
                selectedPayPeriod.DatePaycheck.Year >= 1880 ?
                    selectedPayPeriod.DatePaycheck.ToShortDateString() : "";
        }

        private void Grid_CellDoubleClick(object sender, ODGridClickEventArgs e)
        {
            var employees = new List<Employee>(this.employees);

            using (var formTimeCard = new FormTimeCard(employees))
            {
                formTimeCard.IsByLastName = true;
                formTimeCard.EmployeeCur = Employee.GetById(PIn.Long(MainTable.Rows[e.Row]["EmployeeNum"].ToString()));
                formTimeCard.SelectedPayPeriod = selectedPayPeriod;
                formTimeCard.ShowDialog(this);

                FillMain();
            }
        }

        /// <summary>
        /// This is a modified version of FormTimeCard.FillMain().
        /// It fills one time card per employee.
        /// </summary>
        private ODGrid GetGridForPrinting(Employee emp)
        {
            ODGrid gridTimeCard = new ODGrid();
            List<ClockEvent> clockEventList = ClockEvent.GetByEmployee(emp.Id, PIn.Date(dateStartTextBox.Text), PIn.Date(dateEndTextBox.Text), false);
            List<TimeAdjustment> timeAdjustList = TimeAdjustment.Refresh(emp.Id, PIn.Date(dateStartTextBox.Text), PIn.Date(dateEndTextBox.Text));




            DateTime date = selectedPayPeriod.DateStart.Date;
            DateTime midnightFirstDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);

            timeAdjustNote = TimeAdjustment.GetPayPeriodNote(emp.Id, midnightFirstDay);
            if (timeAdjustNote != null)
            {
                timeAdjustList.RemoveAll(x => x.Id == timeAdjustNote.Id);
            }



            var mergedClockEvents = new List<IDateTimeStamped>();
            mergedClockEvents.AddRange(clockEventList);
            mergedClockEvents.AddRange(timeAdjustList);
            mergedClockEvents.Sort((x, y) => x.GetDateTime().CompareTo(y.GetDateTime()));

            gridTimeCard.BeginUpdate();
            gridTimeCard.Columns.Clear();
            gridTimeCard.Columns.Add(new ODGridColumn("Date", 70));
            gridTimeCard.Columns.Add(new ODGridColumn("Weekday", 70));
            gridTimeCard.Columns.Add(new ODGridColumn("In", 60, HorizontalAlignment.Right));
            gridTimeCard.Columns.Add(new ODGridColumn("Out", 60, HorizontalAlignment.Right));
            gridTimeCard.Columns.Add(new ODGridColumn("Total", 50, HorizontalAlignment.Right));
            gridTimeCard.Columns.Add(new ODGridColumn("Adjust", 55, HorizontalAlignment.Right));
            gridTimeCard.Columns.Add(new ODGridColumn("Rate2", 55, HorizontalAlignment.Right));
            gridTimeCard.Columns.Add(new ODGridColumn("Overtime", 55, HorizontalAlignment.Right));
            gridTimeCard.Columns.Add(new ODGridColumn("Daily", 50, HorizontalAlignment.Right));
            gridTimeCard.Columns.Add(new ODGridColumn("Weekly", 50, HorizontalAlignment.Right));
            if (Preferences.HasClinicsEnabled)
            {
                gridTimeCard.Columns.Add(new ODGridColumn("Clinic", 50, HorizontalAlignment.Left));
            }
            gridTimeCard.Columns.Add(new ODGridColumn("Note", 5));
            gridTimeCard.Rows.Clear();

            TimeSpan[] weeklyTotals = new TimeSpan[mergedClockEvents.Count];
            TimeSpan alteredSpan = new TimeSpan(0);//used to display altered times
            TimeSpan oneSpan = new TimeSpan(0);//used to sum one pair of clock-in/clock-out
            TimeSpan oneAdj;
            TimeSpan oneOT;
            TimeSpan daySpan = new TimeSpan(0);//used for daily totals.
            TimeSpan weekSpan = new TimeSpan(0);//used for weekly totals.
            if (mergedClockEvents.Count > 0)
            {
                weekSpan = ClockEvent.GetWeekTotal(emp.Id, GetDateForRow(0, mergedClockEvents));
            }
            TimeSpan periodSpan = new TimeSpan(0);//used to add up totals for entire page.
            TimeSpan otspan = new TimeSpan(0);//overtime for the entire period
            TimeSpan rate2span = new TimeSpan(0);//rate2 hours total
            Calendar cal = CultureInfo.CurrentCulture.Calendar;
            CalendarWeekRule rule = CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule;
            DateTime curDate = DateTime.MinValue;
            DateTime previousDate = DateTime.MinValue;
            Type type;
            ClockEvent clock;
            TimeAdjustment adjust;
            for (int i = 0; i < mergedClockEvents.Count; i++)
            {
                var row = new ODGridRow();
                type = mergedClockEvents[i].GetType();
                row.Tag = mergedClockEvents[i];
                previousDate = curDate;
                //clock event row---------------------------------------------------------------------------------------------
                if (type == typeof(ClockEvent))
                {
                    clock = (ClockEvent)mergedClockEvents[i];
                    curDate = clock.Date1Displayed.Date;
                    if (curDate == previousDate)
                    {
                        row.Cells.Add("");
                        row.Cells.Add("");
                    }
                    else
                    {
                        row.Cells.Add(curDate.ToShortDateString());
                        row.Cells.Add(curDate.DayOfWeek.ToString());
                    }
                    //altered--------------------------------------
                    //deprecated
                    //status--------------------------------------
                    //row.Cells.Add(clock.ClockStatus.ToString());
                    //in------------------------------------------
                    row.Cells.Add(clock.Date1Displayed.ToShortTimeString());
                    if (clock.Date1Entered != clock.Date1Displayed)
                    {
                        row.Cells[row.Cells.Count - 1].ColorText = Color.Red;
                    }
                    //out-----------------------------
                    if (!clock.Date2Displayed.HasValue)
                    {
                        row.Cells.Add("");//not clocked out yet
                    }
                    else
                    {
                        row.Cells.Add(clock.Date2Displayed.Value.ToShortTimeString());
                        if (clock.Date2Entered != clock.Date2Displayed)
                        {
                            row.Cells[row.Cells.Count - 1].ColorText = Color.Red;
                        }
                    }
                    //total-------------------------------
                    if (!clock.Date2Displayed.HasValue)
                    {
                        row.Cells.Add("");
                    }
                    else
                    {
                        oneSpan = clock.Date2Displayed.Value - clock.Date1Displayed;
                        row.Cells.Add(ClockEvent.Format(oneSpan));
                        daySpan += oneSpan;
                        weekSpan += oneSpan;
                        periodSpan += oneSpan;
                    }
                    //Adjust---------------------------------
                    oneAdj = clock.Adjust ?? clock.AdjustAuto;  oneAdj += clock.AdjustAuto;//typically zero
                    
                    daySpan += oneAdj;
                    weekSpan += oneAdj;
                    periodSpan += oneAdj;
                    row.Cells.Add(ClockEvent.Format(oneAdj));
                    if (clock.Adjust.HasValue)
                    {
                        row.Cells[row.Cells.Count - 1].ColorText = Color.Red;
                    }
                    //Rate2---------------------------------
                    if (clock.Rate2.HasValue)
                    {
                        rate2span += clock.Rate2.Value;
                        row.Cells.Add(ClockEvent.Format(clock.Rate2.Value));
                        row.Cells[row.Cells.Count - 1].ColorText = Color.Red;
                    }
                    else
                    {
                        rate2span += clock.Rate2Auto;
                        row.Cells.Add(ClockEvent.Format(clock.Rate2Auto));
                    }
                    //Overtime------------------------------
                    oneOT = clock.Overtime ?? clock.OvertimeAuto;
                    otspan += oneOT;
                    daySpan -= oneOT;
                    weekSpan -= oneOT;
                    periodSpan -= oneOT;
                    row.Cells.Add(ClockEvent.Format(oneOT));
                    if (clock.Overtime != TimeSpan.FromHours(-1))
                    {//overridden
                        row.Cells[row.Cells.Count - 1].ColorText = Color.Red;
                    }
                    //Daily-----------------------------------
                    //if this is the last entry for a given date
                    if (i == mergedClockEvents.Count - 1//if this is the last row
                        || GetDateForRow(i + 1, mergedClockEvents) != curDate)//or the next row is a different date
                    {
                        row.Cells.Add(ClockEvent.Format(daySpan));
                        daySpan = new TimeSpan(0);
                    }
                    else
                    {//not the last entry for the day
                        row.Cells.Add("");
                    }
                    //Weekly-------------------------------------
                    weeklyTotals[i] = weekSpan;
                    //if this is the last entry for a given week
                    if (i == mergedClockEvents.Count - 1//if this is the last row 
                        || cal.GetWeekOfYear(GetDateForRow(i + 1, mergedClockEvents), rule, (DayOfWeek)Preference.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek))//or the next row has a
                        != cal.GetWeekOfYear(clock.Date1Displayed.Date, rule, (DayOfWeek)Preference.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek)))//different week of year
                    {
                        row.Cells.Add(ClockEvent.Format(weekSpan));
                        weekSpan = new TimeSpan(0);
                    }
                    else
                    {
                        //row.Cells.Add(ClockEvents.Format(weekSpan));
                        row.Cells.Add("");
                    }
                    //Clinic---------------------------------------
                    if (Preferences.HasClinicsEnabled)
                    {
                        if (clock.ClinicId.HasValue)
                        {
                            row.Cells.Add(Clinics.GetAbbr(clock.ClinicId.Value));
                        }
                        else
                        {
                            row.Cells.Add("");
                        }

                       
                    }
                    //Note-----------------------------------------
                    row.Cells.Add(clock.Note);
                }
                //adjustment row--------------------------------------------------------------------------------------
                else if (type == typeof(TimeAdjustment))
                {
                    adjust = (TimeAdjustment)mergedClockEvents[i];
                    curDate = adjust.Date.Date;
                    if (curDate == previousDate)
                    {
                        row.Cells.Add("");
                        row.Cells.Add("");
                    }
                    else
                    {
                        row.Cells.Add(curDate.ToShortDateString());
                        row.Cells.Add(curDate.DayOfWeek.ToString());
                    }
                    //altered--------------------------------------
                    //Deprecated
                    //status--------------------------------------
                    //row.Cells.Add("");//3
                    //in/out------------------------------------------
                    row.Cells.Add("");//4
                                      //time-----------------------------
                    row.Cells.Add(adjust.Date.ToShortTimeString());//5
                                                                        //total-------------------------------
                    row.Cells.Add("");//
                                      //Adjust------------------------------
                    daySpan += adjust.HoursRegular;//might be negative
                    weekSpan += adjust.HoursRegular;
                    periodSpan += adjust.HoursRegular;
                    row.Cells.Add(ClockEvent.Format(adjust.HoursRegular));//6
                                                                       //Rate2-------------------------------
                    row.Cells.Add("");//
                                      //Overtime------------------------------
                    otspan += adjust.HoursOvertime;
                    row.Cells.Add(ClockEvent.Format(adjust.HoursOvertime));//7
                                                                         //Daily-----------------------------------
                                                                         //if this is the last entry for a given date
                    if (i == mergedClockEvents.Count - 1//if this is the last row
                        || GetDateForRow(i + 1, mergedClockEvents) != curDate)//or the next row is a different date
                    {
                        row.Cells.Add(ClockEvent.Format(daySpan));//
                        daySpan = new TimeSpan(0);
                    }
                    else
                    {
                        row.Cells.Add("");
                    }
                    //Weekly-------------------------------------
                    weeklyTotals[i] = weekSpan;
                    //if this is the last entry for a given week
                    if (i == mergedClockEvents.Count - 1//if this is the last row 
                        || cal.GetWeekOfYear(GetDateForRow(i + 1, mergedClockEvents), rule, (DayOfWeek)Preference.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek))//or the next row has a
                        != cal.GetWeekOfYear(adjust.Date.Date, rule, (DayOfWeek)Preference.GetInt(PreferenceName.TimeCardOvertimeFirstDayOfWeek)))//different week of year
                    {
                        ODGridCell cell = new ODGridCell(ClockEvent.Format(weekSpan));
                        cell.ColorText = Color.Black;
                        row.Cells.Add(cell);
                        weekSpan = new TimeSpan(0);
                    }
                    else
                    {
                        row.Cells.Add("");
                    }
                    //Clinic---------------------------------------
                    if (Preferences.HasClinicsEnabled)
                    {
                        if (adjust.ClinicId.HasValue)
                        {
                            row.Cells.Add(Clinics.GetAbbr(adjust.ClinicId.Value));
                        }
                        else
                        {
                            row.Cells.Add("");
                        }
                        
                    }
                    //Note-----------------------------------------
                    row.Cells.Add("(Adjust)" + adjust.Note);//used to indicate adjust rows.
                    row.Cells[row.Cells.Count - 1].ColorText = Color.Red;
                }
                gridTimeCard.Rows.Add(row);
            }
            gridTimeCard.EndUpdate();
            totalTime = periodSpan.ToStringHmm();
            overTime = otspan.ToStringHmm();
            rate2Time = rate2span.ToStringHmm();
            totalTime2 = periodSpan.TotalHours.ToString("n");
            overTime2 = otspan.TotalHours.ToString("n");
            rate2Time2 = rate2span.TotalHours.ToString("n");
            return gridTimeCard;
        }

        private DateTime GetDateForRow(int index, IEnumerable<IDateTimeStamped> mergedAL) =>
            mergedAL.ElementAt(index).GetDateTime().Date;

        private void PrintAllButton_Click(object sender, EventArgs e)
        {
            pagesPrinted = 0;

            PrinterL.TryPreview(pd2_PrintPage,
                "Employee time cards printed",
                totalPages: grid.Rows.Count
            );
        }

        private void pd2_PrintPage(object sender, PrintPageEventArgs e) => PrintEveryTimeCard(sender, e);

        private void PrintEveryTimeCard(object sender, PrintPageEventArgs e)
        {
            //A preview of every single emp on their own page will show up. User will print from there.
            Graphics g = e.Graphics;
            Employee employeeCur = Employee.GetById(PIn.Long(MainTable.Rows[pagesPrinted]["EmployeeNum"].ToString()));
            ODGrid timeCardGrid = GetGridForPrinting(employeeCur);
            int linesPrinted = 0;
            //Create a timecardgrid for this employee?
            float yPos = 75;
            float xPos = 55;
            string str;
            Font font = new Font(FontFamily.GenericSansSerif, 8);
            Font fontTitle = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Bold);
            Font fontHeader = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold);
            SolidBrush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(Color.Black);
            //Title
            str = employeeCur.FirstName + " " + employeeCur.LastName;
            str += "\r\nNote: " + timeAdjustNote?.Note.ToString() ?? "";
            g.DrawString(str, fontTitle, brush, xPos, yPos);
            yPos += 42;
            //define columns
            int[] colW = new int[11];
            if (Preferences.HasClinicsEnabled)
            {
                colW = new int[12];
            }
            colW[0] = 70;//date
            colW[1] = 70;//weekday
                         //colW[2]=50;//altered
            colW[2] = 60;//in
            colW[3] = 60;//out
            colW[4] = 50;//total
            colW[5] = 50;//adjust
            colW[6] = 50;//Rate2 //added
            colW[7] = 55;//overtime
            colW[8] = 50;//daily
            colW[9] = 50;//weekly
            colW[10] = 160;//note
            if (Preferences.HasClinicsEnabled)
            {
                colW[10] = 50;//clinic
                colW[11] = 160;//note
            }
            int[] colPos = new int[colW.Length + 1];
            colPos[0] = 45;
            for (int i = 1; i < colPos.Length; i++)
            {
                colPos[i] = colPos[i - 1] + colW[i - 1];
            }
            string[] ColCaption = new string[11];
            if (Preferences.HasClinicsEnabled)
            {
                ColCaption = new string[12];
            }
            ColCaption[0] = "Date";
            ColCaption[1] = "Weekday";
            ColCaption[2] = "In";
            ColCaption[3] = "Out";
            ColCaption[4] = "Total";
            ColCaption[5] = "Adjust";
            ColCaption[6] = "Rate 2";
            ColCaption[7] = "Overtime";
            ColCaption[8] = "Daily";
            ColCaption[9] = "Weekly";
            ColCaption[10] = "Note";
            if (Preferences.HasClinicsEnabled)
            {
                ColCaption[10] = "Clinic";
                ColCaption[11] = "Note";
            }
            //column headers-----------------------------------------------------------------------------------------
            e.Graphics.FillRectangle(Brushes.LightGray, colPos[0], yPos, colPos[colPos.Length - 1] - colPos[0], 18);
            e.Graphics.DrawRectangle(pen, colPos[0], yPos, colPos[colPos.Length - 1] - colPos[0], 18);
            for (int i = 1; i < colPos.Length; i++)
            {
                e.Graphics.DrawLine(new Pen(Color.Black), colPos[i], yPos, colPos[i], yPos + 18);
            }
            //Prints the Column Titles
            for (int i = 0; i < ColCaption.Length; i++)
            {
                e.Graphics.DrawString(ColCaption[i], fontHeader, brush, colPos[i] + 2, yPos + 1);
            }
            yPos += 18;
            while (yPos < e.PageBounds.Height - 75 - 50 - 32 - 16 && linesPrinted < timeCardGrid.Rows.Count)
            {
                for (int i = 0; i < colPos.Length - 1; i++)
                {
                    if (timeCardGrid.Rows[linesPrinted].Cells[i].ColorText == Color.Empty || timeCardGrid.Rows[linesPrinted].Cells[i].ColorText == Color.Black)
                    {
                        e.Graphics.DrawString(timeCardGrid.Rows[linesPrinted].Cells[i].Text, font, brush
                            , new RectangleF(colPos[i] + 2, yPos, colPos[i + 1] - colPos[i] - 5, font.GetHeight(e.Graphics)));
                    }
                    else
                    { //The only other color currently supported is red.
                        e.Graphics.DrawString(timeCardGrid.Rows[linesPrinted].Cells[i].Text, font, Brushes.Red
                            , new RectangleF(colPos[i] + 2, yPos, colPos[i + 1] - colPos[i] - 5, font.GetHeight(e.Graphics)));
                    }
                }
                //Column lines		
                for (int i = 0; i < colPos.Length; i++)
                {
                    e.Graphics.DrawLine(Pens.Gray, colPos[i], yPos + 16, colPos[i], yPos);
                }
                linesPrinted++;
                yPos += 16;
                e.Graphics.DrawLine(new Pen(Color.Gray), colPos[0], yPos, colPos[colPos.Length - 1], yPos);
            }
            //totals will print on every page for simplicity
            yPos += 10;
            g.DrawString(Lan.g(this, "Regular Time") + ": " + totalTime + " (" + totalTime2 + ")", fontHeader, brush, xPos, yPos);
            yPos += 16;
            g.DrawString(Lan.g(this, "Overtime") + ": " + overTime + " (" + overTime2 + ")", fontHeader, brush, xPos, yPos);
            yPos += 16;
            g.DrawString(Lan.g(this, "Rate 2 Time") + ": " + rate2Time + " (" + rate2Time2 + ")", fontHeader, brush, xPos, yPos);
            pagesPrinted++;
            if (grid.Rows.Count == pagesPrinted)
            {
                pagesPrinted = 0;
                e.HasMorePages = false;
            }
            else
            {
                e.HasMorePages = true;
            }
        }

        private void PrintSelectedButton_Click(object sender, EventArgs e)
        {
            if (grid.SelectedIndices.Length == 0)
            {
                MessageBox.Show(
                    "No employees selected, please select one or more employees or click 'Print All' to print all employees.");

                return;
            }

            pagesPrinted = 0;
            PrinterL.TryPreview(pd2_PrintPageSelective,
                "Employee time cards printed",
                totalPages: grid.SelectedIndices.Length
            );
        }

        ///<summary>Similar to pd2_PrintPage except it iterates through selected indices instead of all indices.</summary>
        private void pd2_PrintPageSelective(object sender, PrintPageEventArgs e)
        {
            PrintEmployeeTimeCard(sender, e);
        }

        private void PrintEmployeeTimeCard(object sender, PrintPageEventArgs e)
        {
            //A preview of every single emp on their own page will show up. User will print from there.
            Graphics g = e.Graphics;
            Employee employeeCur = Employee.GetById(PIn.Long(MainTable.Rows[grid.SelectedIndices[pagesPrinted]]["EmployeeNum"].ToString()));
            ODGrid timeCardGrid = GetGridForPrinting(employeeCur);
            int linesPrinted = 0;
            //Create a timecardgrid for this employee?
            float yPos = 75;
            float xPos = 55;
            string str;
            Font font = new Font(FontFamily.GenericSansSerif, 8);
            Font fontTitle = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Bold);
            Font fontHeader = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold);
            SolidBrush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(Color.Black);
            //Title
            str = employeeCur.FirstName + " " + employeeCur.LastName;
            str += "\r\n" + Lan.g(this, "Note") + ": " + timeAdjustNote?.Note.ToString() ?? "";
            g.DrawString(str, fontTitle, brush, xPos, yPos);
            yPos += 42;
            //define columns
            int[] colW = new int[11];
            if (Preferences.HasClinicsEnabled)
            {
                colW = new int[12];
            }
            colW[0] = 70;//date
            colW[1] = 70;//weekday
                         //colW[2]=50;//altered
            colW[2] = 60;//in
            colW[3] = 60;//out
            colW[4] = 50;//total
            colW[5] = 50;//adjust
            colW[6] = 50;//Rate2 //added
            colW[7] = 55;//overtime
            colW[8] = 50;//daily
            colW[9] = 50;//weekly
            colW[10] = 160;//note
            if (Preferences.HasClinicsEnabled)
            {
                colW[10] = 50;//clinic
                colW[11] = 160;//note
            }
            int[] colPos = new int[colW.Length + 1];
            colPos[0] = 45;
            for (int i = 1; i < colPos.Length; i++)
            {
                colPos[i] = colPos[i - 1] + colW[i - 1];
            }
            string[] ColCaption = new string[11];
            if (Preferences.HasClinicsEnabled)
            {
                ColCaption = new string[12];
            }
            ColCaption[0] = "Date";
            ColCaption[1] = "Weekday";
            ColCaption[2] = "In";
            ColCaption[3] = "Out";
            ColCaption[4] = "Total";
            ColCaption[5] = "Adjust";
            ColCaption[6] = "Rate 2";
            ColCaption[7] = "Overtime";
            ColCaption[8] = "Daily";
            ColCaption[9] = "Weekly";
            ColCaption[10] = "Note";
            if (Preferences.HasClinicsEnabled)
            {
                ColCaption[10] = "Clinic";
                ColCaption[11] = "Note";
            }
            //column headers-----------------------------------------------------------------------------------------
            e.Graphics.FillRectangle(Brushes.LightGray, colPos[0], yPos, colPos[colPos.Length - 1] - colPos[0], 18);
            e.Graphics.DrawRectangle(pen, colPos[0], yPos, colPos[colPos.Length - 1] - colPos[0], 18);
            for (int i = 1; i < colPos.Length; i++)
            {
                e.Graphics.DrawLine(new Pen(Color.Black), colPos[i], yPos, colPos[i], yPos + 18);
            }
            //Prints the Column Titles
            for (int i = 0; i < ColCaption.Length; i++)
            {
                e.Graphics.DrawString(ColCaption[i], fontHeader, brush, colPos[i] + 2, yPos + 1);
            }
            yPos += 18;
            while (yPos < e.PageBounds.Height - 75 - 50 - 32 - 16 && linesPrinted < timeCardGrid.Rows.Count)
            {
                for (int i = 0; i < colPos.Length - 1; i++)
                {
                    if (timeCardGrid.Rows[linesPrinted].Cells[i].ColorText == Color.Empty || timeCardGrid.Rows[linesPrinted].Cells[i].ColorText == Color.Black)
                    {
                        e.Graphics.DrawString(timeCardGrid.Rows[linesPrinted].Cells[i].Text, font, brush
                            , new RectangleF(colPos[i] + 2, yPos, colPos[i + 1] - colPos[i] - 5, font.GetHeight(e.Graphics)));
                    }
                    else
                    { //The only other color currently supported is red.
                        e.Graphics.DrawString(timeCardGrid.Rows[linesPrinted].Cells[i].Text, font, Brushes.Red
                            , new RectangleF(colPos[i] + 2, yPos, colPos[i + 1] - colPos[i] - 5, font.GetHeight(e.Graphics)));
                    }
                }
                //Column lines		
                for (int i = 0; i < colPos.Length; i++)
                {
                    e.Graphics.DrawLine(Pens.Gray, colPos[i], yPos + 16, colPos[i], yPos);
                }
                linesPrinted++;
                yPos += 16;
                e.Graphics.DrawLine(new Pen(Color.Gray), colPos[0], yPos, colPos[colPos.Length - 1], yPos);
            }
            //totals will print on every page for simplicity
            yPos += 10;
            g.DrawString("Regular Time: " + totalTime + " (" + totalTime2 + ")", fontHeader, brush, xPos, yPos);
            yPos += 16;
            g.DrawString("Overtime: " + overTime + " (" + overTime2 + ")", fontHeader, brush, xPos, yPos);
            yPos += 16;
            g.DrawString("Rate 2 Time: " + rate2Time + " (" + rate2Time2 + ")", fontHeader, brush, xPos, yPos);
            pagesPrinted++;
            if (grid.SelectedIndices.Length == pagesPrinted)
            {
                pagesPrinted = 0;
                e.HasMorePages = false;
            }
            else
            {
                e.HasMorePages = true;
            }
        }

        private void LeftButton_Click(object sender, EventArgs e)
        {
            if (selectedIndex == 0) return;

            selectedIndex--;
            selectedPayPeriod = payPeriods[selectedIndex];

            FillPayPeriod();
            FillMain();
        }

        private void RightButton_Click(object sender, EventArgs e)
        {
            if (selectedIndex == payPeriods.Count - 1) return;

            selectedIndex++;
            selectedPayPeriod = payPeriods[selectedIndex];

            FillPayPeriod();
            FillMain();
        }

        private void DailyButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.TimecardsEditAll)) return;

            var errorMessage = TimeCardRule.ValidateOvertimeRules(new List<long> { 0 }); // Validates the "all employees" timecard rules first.
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(
                    errorMessage,
                    "Manage Time Cards",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (grid.SelectedIndices.Length == 0)
            {
                var result =
                    MessageBox.Show(
                        "No employees selected. Would you like to run calculations for all employees?",
                        "Manage Time Cards", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question);

                if (result == DialogResult.No) return;

                grid.SetSelected(true);
            }

            Cursor = Cursors.WaitCursor;

            errorMessage = "";
            for (int i = 0; i < grid.SelectedIndices.Length; i++)
            {
                try
                {
                    TimeCardRule.CalculateDailyOvertime(
                        Employee.GetById(PIn.Long(MainTable.Rows[grid.SelectedIndices[i]]["EmployeeNum"].ToString())), 
                        X_DateStart, 
                        X_DateEnd);
                }
                catch (Exception exception)
                {
                    errorMessage = exception.Message;
                }
            }

            Cursor = Cursors.Default;

            var selectedIndices = new List<int>();
            for (int i = 0; i < grid.SelectedIndices.Length; i++)
            {
                selectedIndices.Add(grid.SelectedIndices[i]);
            }

            FillMain();

            foreach (var index in selectedIndices) grid.SetSelected(index, true);

            if (string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show(
                    "Done.",
                    "Manage Time Cards",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    "Time cards were not calculated for some Employees for the following reasons:\r\n" + errorMessage,
                    "Manage Time Cards",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void WeeklyButton_Click(object sender, EventArgs e)
        {
            if (!Security.IsAuthorized(Permissions.TimecardsEditAll)) return;

            if (grid.SelectedIndices.Length == 0)
            {
                var result =
                    MessageBox.Show(
                        "No employees selected. Would you like to run calculations for all employees?",
                        "Manage Time Cards", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question);

                if (result == DialogResult.No) return;

                grid.SetSelected(true);
            }

            Cursor = Cursors.WaitCursor;

            var errorMessage = "";
            for (int i = 0; i < grid.SelectedIndices.Length; i++)
            {
                try
                {
                    TimeCardRule.CalculateWeeklyOvertime(
                        Employee.GetById(PIn.Long(MainTable.Rows[grid.SelectedIndices[i]]["EmployeeNum"].ToString())),
                        X_DateStart,
                        X_DateEnd);
                }
                catch (Exception exception)
                {
                    errorMessage += exception.Message + "\r\n";
                }
            }

            Cursor = Cursors.Default;

            var selectedIndices = new List<int>();
            for (int i = 0; i < grid.SelectedIndices.Length; i++)
            {
                selectedIndices.Add(grid.SelectedIndices[i]);
            }

            FillMain();

            foreach (var index in selectedIndices) grid.SetSelected(index, true);

            if (string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show(
                    "Done.",
                    "Manage Time Cards",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    "Time cards were not calculated for some Employees for the following reasons:\r\n" + errorMessage,
                    "Manage Time Cards", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void ClearManualButton_Click(object sender, EventArgs e)
        {
            var result =
                MessageBox.Show(
                    "This cannot be undone. Would you like to continue?",
                    "Manage Time Cards", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            for (int i = 0; i < grid.SelectedIndices.Length; i++)
            {
                try
                {
                    TimeCardRule.ClearManual(
                        PIn.Long(MainTable.Rows[grid.SelectedIndices[i]]["EmployeeNum"].ToString()), 
                        X_DateStart, 
                        X_DateEnd);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        exception.Message,
                        "Manage Time Cards",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            var selectedIndices = new List<int>();
            for (int i = 0; i < grid.SelectedIndices.Length; i++)
            {
                selectedIndices.Add(grid.SelectedIndices[i]);
            }

            FillMain();

            foreach (var index in selectedIndices) grid.SetSelected(index, true);
        }

        private void ClearAutoButton_Click(object sender, EventArgs e)
        {
            var result =
                MessageBox.Show(
                    "This cannot be undone, but you can run the calculate buttons again later. Would you like to continue?",
                    "Manage Time Cards", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            for (int i = 0; i < grid.SelectedIndices.Length; i++)
            {
                try
                {
                    TimeCardRule.ClearAuto(
                        PIn.Long(MainTable.Rows[grid.SelectedIndices[i]]["EmployeeNum"].ToString()),
                        X_DateStart, 
                        X_DateEnd);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        exception.Message,
                        "Manage Time Cards",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            var selectedIndices = new List<int>();
            for (int i = 0; i < grid.SelectedIndices.Length; i++)
            {
                selectedIndices.Add(grid.SelectedIndices[i]);
            }

            FillMain();

            foreach (var index in selectedIndices) grid.SetSelected(index, true);
        }

        private void PrintGridButton_Click(object sender, EventArgs e)
        {
            pagesPrinted = 0;
            headingPrinted = false;

            PrinterL.TryPrintOrDebugRpPreview(pd_PrintPage, 
                "Printed employee time card grid.");
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            Rectangle bounds = e.MarginBounds;
            string text;

            int y = bounds.Top;
            int center = bounds.X + bounds.Width / 2;
            int headingHeight = 0;

            if (!headingPrinted)
            {
                text = 
                    "Pay Period: " + dateStartTextBox.Text + " - " + dateEndTextBox.Text + "\r\n" +
                    "Paycheck Date: " + datePaycheckTextBox.Text;

                if (Preferences.HasClinicsEnabled)
                {
                    text += "\r\nClinic: ";
                    if (Security.CurrentUser.ClinicRestricted)
                    {
                        text += Clinics.GetAbbr(clinics[clinicComboBox.SelectedIndex].ClinicNum);
                    }
                    else
                    {
                        if (clinicComboBox.SelectedIndex == 0)
                        {
                            text += "All";
                        }
                        else if (clinicComboBox.SelectedIndex == 1)
                        {
                            text += "Headquarters";
                        }
                        else
                        {
                            text += Clinics.GetAbbr(clinics[clinicComboBox.SelectedIndex - 2].ClinicNum); ;//Subtract 2, because All and Headquarters are in the list.
                        }
                    }
                }

                using (var font = new Font("Arial", 13, FontStyle.Bold))
                {
                    e.Graphics.DrawString(text, font, Brushes.Black, center - e.Graphics.MeasureString(text, font).Width / 2, y);
                    if (Preferences.HasClinicsEnabled)
                    {
                        y += 75;
                    }
                    else
                    {
                        y += 50;
                    }
                }

                headingPrinted = true;
                headingHeight = y;
            }

            y = grid.PrintPage(e.Graphics, pagesPrinted, bounds, headingHeight);

            pagesPrinted++;

            e.HasMorePages = y == -1;
        }

        private void ClinicComboBox_SelectionChangeCommitted(object sender, EventArgs e) => FillMain();

        private void ExportGridButton_Click(object sender, EventArgs e)
        {
            string exportPath;

            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
                {
                    return;
                }

                exportPath =
                    Path.Combine(
                        folderBrowserDialog.SelectedPath,
                        string.Concat("ODPayroll", DateTime.Now.ToString("yyyyMMdd_hhmmss"), ".txt"));
            }

            var exportBuilder = new StringBuilder();
            string headers = "";
            for (int i = 0; i < MainTable.Columns.Count; i++)
            {
                headers += (i > 0 ? "\t" : "") + MainTable.Columns[i].ColumnName;
            }
            exportBuilder.AppendLine(headers);

            for (int i = 0; i < MainTable.Rows.Count; i++)
            {
                string row = "";
                for (int j = 0; j < MainTable.Columns.Count; j++)
                {
                    if (j > 0) row += "\t";
                    
                    switch (MainTable.Columns[j].ColumnName)
                    {
                        case "PayrollID":
                        case "EmployeeNum":
                        case "firstName":
                        case "lastName":
                        case "Note":
                            row += MainTable.Rows[i][j].ToString().Replace("\t", "").Replace("\r\n", ";  ");
                            break;

                        case "totalHours":
                        case "rate1Hours":
                        case "rate1OTHours":
                        case "rate2Hours":
                        case "rate2OTHours":
                            //Time must me formatted differently.
                            if (Preference.GetBool(PreferenceName.TimeCardsUseDecimalInsteadOfColon))
                            {
                                row += PIn.Time(MainTable.Rows[i][j].ToString()).TotalHours.ToString("n");
                            }
                            else if (Preference.GetBool(PreferenceName.TimeCardShowSeconds))
                            {
                                row += PIn.Time(MainTable.Rows[i][j].ToString()).ToStringHmmss();
                            }
                            else
                            {
                                row += PIn.Time(MainTable.Rows[i][j].ToString()).ToStringHmm();
                            }
                            break;

                        default:
                            throw new Exception("Unexpected column found in payroll table : " + MainTable.Columns[j].ColumnName);

                    }
                }
                exportBuilder.AppendLine(row);
            }

            try
            {
                File.WriteAllText(exportPath, exportBuilder.ToString());

                MessageBox.Show( 
                    "Succesfully exported to " + exportPath,
                    "Manage Time Cards", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Unable to create file.\r\n" + exception.Message,
                    "Manage Time Cards",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void ExportAdpButton_Click(object sender, EventArgs e)
        {
            var exportBuilder = new StringBuilder();

            string errors = "";
            string warnings = "";
            string errorIndent = "  ";

            exportBuilder.AppendLine("Co Code,Batch ID,File #" + (Preference.GetBool(PreferenceName.TimeCardADPExportIncludesName) ? ",Employee Name" : "") + ",Rate Code,Reg Hours,O/T Hours");
            
            string batchID = X_DateEnd.ToString("yyyyMMdd");

            var companyCode = Preference.GetString(PreferenceName.ADPCompanyCode);
            if (companyCode.Length < 2 || companyCode.Length > 3)
            {
                errors += errorIndent + "Company code must be two to three alpha numeric characters long.  Go to Setup>TimeCards to edit.\r\n";
            }

            companyCode = companyCode.PadRight(3, '_');//for two digit company codes.
            for (int i = 0; i < MainTable.Rows.Count; i++)
            {
                string errorsForEmployee = "";
                string warningsForEmployee = "";
                string fileNum = "";
                string employeeName = "";
                fileNum = MainTable.Rows[i]["PayrollID"].ToString();
                try
                {
                    if (PIn.Int(fileNum) < 51 || PIn.Int(fileNum) > 999999)
                    {
                        errorsForEmployee += errorIndent + "Payroll ID not between 51 and 999999.\r\n";
                    }
                }
                catch
                {
                    //same error message as above.
                    errorsForEmployee += errorIndent + "Payroll ID not between 51 and 999999.\r\n";
                }
                if (fileNum.Length > 6)
                {
                    errorsForEmployee += errorIndent + "Payroll ID must be less than 6 digits long.\r\n";
                }
                else
                {//pad payrollIDs that are too short. No effect if payroll ID is 6 digits long.
                    fileNum = fileNum.PadLeft(6, '0');
                }
                try
                {
                    employeeName = Employee.GetNameFL(Employee.GetById(PIn.Long(MainTable.Rows[i]["EmployeeNum"].ToString())));
                }
                catch
                {
                    employeeName = "Error";
                }
                string r1hours = (PIn.TSpan(MainTable.Rows[i]["rate1Hours"].ToString())).TotalHours.ToString("F2");//adp allows 2 digit precision
                if (r1hours == "0.00")
                {//Was changing Exactly 80.00 hours with 8 hours.
                    r1hours = "";
                }
                string r1OThours = (PIn.TSpan(MainTable.Rows[i]["rate1OTHours"].ToString())).TotalHours.ToString("F2");//adp allows 2 digit precision
                if (r1OThours == "0.00")
                {
                    r1OThours = "";
                }
                string r2hours = (PIn.TSpan(MainTable.Rows[i]["rate2Hours"].ToString())).TotalHours.ToString("F2");//adp allows 2 digit precision
                if (r2hours == "0.00")
                {
                    r2hours = "";
                }
                string r2OThours = (PIn.TSpan(MainTable.Rows[i]["rate2OTHours"].ToString())).TotalHours.ToString("F2");//adp allows 2 digit precision
                if (r2OThours == "0.00")
                {
                    r2OThours = "";
                }
                string textToAdd = "";
                if (r1hours != "" || r1OThours != "")
                {//no entry should be made unless there are actually hours for this employee.
                    textToAdd += companyCode + "," + batchID + "," + fileNum + (Preference.GetBool(PreferenceName.TimeCardADPExportIncludesName) ? "," + employeeName : "") + ",," + r1hours + "," + r1OThours + "\r\n";
                }
                if (r2hours != "" || r2OThours != "")
                {//no entry should be made unless there are actually hours for this employee.
                    textToAdd += companyCode + "," + batchID + "," + fileNum + (Preference.GetBool(PreferenceName.TimeCardADPExportIncludesName) ? "," + employeeName : "") + ",2," + r2hours + "," + r2OThours + "\r\n";
                }
                if (textToAdd == "")
                {
                    warningsForEmployee += errorIndent + "No clocked hours.\r\n";// for "+Employees.GetNameFL(Employees.GetEmp(PIn.Long(MainTable.Rows[i]["EmployeeNum"].ToString())))+"\r\n";
                }
                else
                {
                    exportBuilder.Append(textToAdd);
                }
                //validate characters in text.  Allowed values are 32 to 91 and 93 to 122----------------------------------------------------------------
                for (int j = 0; j < textToAdd.Length; j++)
                {
                    int charAsInt = (int)textToAdd[j];
                    //these are the characters explicitly allowed by ADP per thier documentation.
                    if (charAsInt >= 32 && charAsInt <= 122 && charAsInt != 92)
                    {//
                        continue;//valid character
                    }
                    if (charAsInt == 10 || charAsInt == 13)
                    {//CR LF, not allowed as values but allowed to deliniate rows.
                        continue;//valid character
                    }
                    errorsForEmployee += "Invalid character found (ASCII=" + charAsInt + "): " + textToAdd.Substring(j, 1) + ".\r\n";
                }
                //Aggregate employee errors into aggregate error messages.--------------------------------------------------------------------------------
                if (errorsForEmployee != "")
                {
                    errors += Employee.GetNameFL(Employee.GetById(PIn.Long(MainTable.Rows[i]["EmployeeNum"].ToString()))) + ":\r\n" + errorsForEmployee + "\r\n";
                }
                if (warningsForEmployee != "")
                {
                    warnings += Employee.GetNameFL(Employee.GetById(PIn.Long(MainTable.Rows[i]["EmployeeNum"].ToString()))) + ":\r\n" + warningsForEmployee + "\r\n";
                }
            }
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string fileSuffix = "";
            for (int i = 0; i <= 1297; i++)
            {//1296=36*36 to represent all acceptable suffixes for file name consisting of two alphanumeric digits; +1 to catch error. (A-Z, 0-9)
                fileSuffix = "";
                //generate suffix from i
                if (i == 1297)
                {
                    //could not find acceptable file name.
                    fileSuffix = "NamingError";
                    break;
                }
                if (i / 36 < 10)
                {
                    fileSuffix += (i / 36);//truncated to int on purpose.  (0 to 9)
                }
                else
                {
                    fileSuffix += (char)((i / 36) - 10 + 65);//65='A' in ASCII.  (A to Z)
                }
                if (i % 36 < 10)
                {
                    fileSuffix += (i % 36);//(0 to 9)
                }
                else
                {
                    fileSuffix += (char)((i % 36) - 10 + 65);//65='A' in ASCII.  (A to Z)
                }

                if (!File.Exists(fbd.SelectedPath + "\\EPI" + companyCode + fileSuffix + ".CSV")) break;
            }

            try
            {
                File.WriteAllText(
                    Path.Combine(fbd.SelectedPath, string.Concat("EPI", companyCode, fileSuffix, ".csv")), 
                    exportBuilder.ToString());

                if (!string.IsNullOrEmpty(errors))
                {
                    MessageBox.Show(
                        "The following errors will prevent ADP from properly processing this export:\r\n" + errors, 
                        "Manage Time Cards", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning);
                }

                if (!string.IsNullOrEmpty(warnings))
                {
                    MessageBox.Show(
                        "The following warnings were detected:\r\n" + warnings, 
                        "Manage Time Cards", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning);
                }

                MessageBox.Show(
                    "File created : " + fbd.SelectedPath + "\\EPI" + companyCode + fileSuffix + ".CSV");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "File not created:\r\n" + ex.Message, 
                    "Manage Time Cards", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private void SetupButton_Click(object sender, EventArgs e)
        {
            using (var formTimeCardSetup = new FormTimeCardSetup())
            {
                formTimeCardSetup.ShowDialog();

                SecurityLog.Write(null, SecurityLogEvents.Setup, "Time Card Setup");
            }
        }

        private void CloseButton_Click(object sender, EventArgs e) => Close();
    }
}
