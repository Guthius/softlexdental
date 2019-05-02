using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using CodeBase;
using System.Diagnostics;
using System.IO;

namespace OpenDental {
	public partial class FormJobManagerOverview:ODForm {
		private List<long> _listEngUserNums = new List<long>() {7,24,29,52,59,61,72,77,98,31,138,149,154,157,223,227,273};
		private List<Job> _listJobsAll;
		private bool _headingPrinted;
		private int _pagesPrinted;
		private int _headingPrintH;

		public FormJobManagerOverview(List<Job> listJobsAll) {
			_listJobsAll=listJobsAll;
			InitializeComponent();
			Lan.F(this);
		}

		private void FormJobManagerOverview_Load(object sender,EventArgs e) {
			WindowState=FormWindowState.Maximized;
			MinimumSize=Size;
			MaximumSize=Size;
			foreach(Def def in Defs.GetDefsForCategory(DefCat.JobPriorities,true).OrderBy(x => x.ItemOrder).ToList()) {
				listPriorities.Items.Add(new ODBoxItem<Def>(def.ItemName,def));
			}
			listPriorities.SelectedIndex=0;
			foreach(JobPhase phase in Enum.GetValues(typeof(JobPhase))) {
				listPhases.Items.Add(new ODBoxItem<JobPhase>(phase.ToString(),phase));
			}
			listPhases.SelectedIndex=0;
			foreach(JobCategory category in Enum.GetValues(typeof(JobCategory))) {
				listCategories.Items.Add(new ODBoxItem<JobCategory>(category.ToString(),category));
			}
			listCategories.SelectedIndex=0;
			comboEngineers.Items.Add(new ODBoxItem<User>("All",new User() { UserNum=-1 }));
			comboEngineers.Items.Add(new ODBoxItem<User>("Unassigned",new User() { UserNum=0 }));
			foreach(long engNum in _listEngUserNums) {
				User eng=Userods.GetUser(engNum);
				comboEngineers.Items.Add(new ODBoxItem<User>(eng.UserName,eng));
			}
			comboEngineers.SelectedIndex=0;
			dateRangeJobCompleted.SetDateTimeFrom(VersionReleases.GetBetaDevelopmentStartDate());
			dateRangeJobCompleted.SetDateTimeTo(VersionReleases.GetBetaReleaseDate());
			SetFilters();
			FillGridJobs(GetFilteredJobList());
			FillGridSprints();
		}

		#region Menu Clicks
		private void butSprintManager_Click(object sender,EventArgs e) {
			panelJobsReport.Visible=false;
			panelSprintManager.Visible=true;
		}

		private void butJobsReport_Click(object sender,EventArgs e) {
			panelJobsReport.Visible=true;
			panelSprintManager.Visible=false;
		}
		#endregion

		#region Sprint Manager
		private void FillGridSprints() {
			gridSprints.BeginUpdate();
			gridSprints.Columns.Clear();
			gridSprints.Columns.Add(new ODGridColumn("Start Date",70));
			gridSprints.Columns.Add(new ODGridColumn("Title",50));
			gridSprints.Rows.Clear();
			foreach(JobSprint sprint in JobSprints.GetAll().OrderByDescending(x => x.DateStart)){
				ODGridRow row=new ODGridRow() { Tag=sprint };
				row.Cells.Add(sprint.DateStart.ToShortDateString());
				row.Cells.Add(sprint.Title.ToString());
				gridSprints.Rows.Add(row);
			}
			gridSprints.EndUpdate();
		}

		private void gridSprints_MouseClick(object sender,MouseEventArgs e) {
			if(e.Button!=MouseButtons.Right) {
				return;
			}
			ContextMenu menu=new ContextMenu();
			menu.MenuItems.Add("Remove Sprint",(o,arg) => {
				int selectedIndex=gridSprints.GetSelectedIndex();
				if(selectedIndex==-1) {
					return;//Nothing to remove.
				}
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will permanently delete this sprint. Continue?")) {
					return;
				}
				long sprintNum=((JobSprint)gridSprints.Rows[selectedIndex].Tag).JobSprintNum;
				userControlSprintManager.Enabled=false;
				JobSprints.Delete(sprintNum);
				FillGridSprints();
			});
			menu.MenuItems.Add("Copy Sprint",(o,arg) => {
				int selectedIndex=gridSprints.GetSelectedIndex();
				if(selectedIndex==-1) {
					return;//Nothing to copy.
				}
				JobSprint sprint=((JobSprint)gridSprints.Rows[selectedIndex].Tag);
				sprint.Title+="_Copy";
				List<JobSprintLink> listSprintLinks=JobSprintLinks.GetForSprint(sprint.JobSprintNum);
				JobSprints.Insert(sprint);
				foreach(JobSprintLink sprintLink in listSprintLinks) {
					sprintLink.JobSprintNum=sprint.JobSprintNum;
					JobSprintLinks.Insert(sprintLink);
				}
				FillGridSprints();
				userControlSprintManager.LoadSprint(sprint,_listJobsAll);
			});
			menu.MenuItems.Add("End Sprint",(o,arg) => {
				int selectedIndex=gridSprints.GetSelectedIndex();
				if(selectedIndex==-1) {
					return;//Nothing to end.
				}
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will set the sprints actual end date to today. Continue?")) {
					return;
				}
				JobSprint sprint;
				//This gets the most recent sprint instance to save and unloads it so we can load it in
				if(!userControlSprintManager.Enabled || !userControlSprintManager.UnloadSprint(out sprint)) {
					return;
				}
				else {
					sprint=((JobSprint)gridSprints.Rows[selectedIndex].Tag);
				}
				sprint.DateEndActual=DateTime.Today;
				JobSprints.Update(sprint);
				FillGridSprints();
				userControlSprintManager.LoadSprint(sprint,_listJobsAll);
			});
			menu.Show(gridSprints,gridSprints.PointToClient(Cursor.Position));
		}

		private void gridSprints_CellClick(object sender,ODGridClickEventArgs e) {
			userControlSprintManager.LoadSprint((JobSprint)gridSprints.Rows[e.Row].Tag,_listJobsAll);
		}

		private void gridSprints_TitleAddClick(object sender,EventArgs e) {
			JobSprint jobSprintNew=new JobSprint();
			jobSprintNew.Title="Version";
			jobSprintNew.Note="";
			jobSprintNew.DateStart=DateTime.Today;
			jobSprintNew.DateEndTarget=DateTime.Today.AddDays(70);//Add 10 weeks
			jobSprintNew.DateEndActual=DateTime.MinValue;
			jobSprintNew.JobPercent=0.173;
			jobSprintNew.HoursAverageDevelopment=9.43;
			jobSprintNew.HoursAverageBreak=0.85;
			JobSprints.Insert(jobSprintNew);
			FillGridSprints();
			userControlSprintManager.LoadSprint(jobSprintNew,_listJobsAll);
		}

		private void userControlSprintManager_SaveClick(object sender,EventArgs e) {
			FillGridSprints();
		}
		#endregion

		#region Jobs Report
		private void FillGridJobs(List<Job> listJobs) {
			int totalJobs=listJobs.Count();
			int totalBugs=0;
			int totalFeatures=0;
			double totalHrsEst=0;
			double totalHrsSpent=0;
			double totalQuotedDollars=0;
			List<Def> listJobPriorities=Defs.GetDefsForCategory(DefCat.JobPriorities);
			gridJobs.BeginUpdate();
			gridJobs.Columns.Clear();
			gridJobs.Columns.Add(new ODGridColumn("Job",0));
			gridJobs.Columns.Add(new ODGridColumn("Owner",70) { TextAlign=HorizontalAlignment.Center });
			gridJobs.Columns.Add(new ODGridColumn("Owner Action",90));
			gridJobs.Columns.Add(new ODGridColumn("Phase",75) { TextAlign=HorizontalAlignment.Center });
			gridJobs.Columns.Add(new ODGridColumn("Priority",75) { TextAlign=HorizontalAlignment.Center });
			gridJobs.Columns.Add(new ODGridColumn("Expert",75) { TextAlign=HorizontalAlignment.Center });
			gridJobs.Columns.Add(new ODGridColumn("Engineer",75) { TextAlign=HorizontalAlignment.Center });
			gridJobs.Columns.Add(new ODGridColumn("Hrs Est",90) { TextAlign=HorizontalAlignment.Center,SortingStrategy=ODGridSortingStrategy.AmountParse });
			gridJobs.Columns.Add(new ODGridColumn("Hrs Last 7 Days",90) { TextAlign=HorizontalAlignment.Center,SortingStrategy=ODGridSortingStrategy.AmountParse });
			gridJobs.Columns.Add(new ODGridColumn("Hrs Total",90) { TextAlign=HorizontalAlignment.Center,SortingStrategy=ODGridSortingStrategy.AmountParse });
			gridJobs.Columns.Add(new ODGridColumn("Est Completion %",110) { TextAlign=HorizontalAlignment.Center,SortingStrategy=ODGridSortingStrategy.AmountParse });
			gridJobs.Columns.Add(new ODGridColumn("Last Updated",90) { TextAlign=HorizontalAlignment.Center });
			gridJobs.Rows.Clear();
			foreach(Job job in listJobs) {
				Def jobPriority = listJobPriorities.FirstOrDefault(y => y.DefNum==job.Priority);
				totalBugs+=job.ListJobLinks.Count(x => x.LinkType==JobLinkType.Bug);
				totalFeatures+=job.ListJobLinks.Count(x => x.LinkType==JobLinkType.Request);
				totalHrsEst+=job.HoursEstimate;
				totalHrsSpent+=job.HoursActual;
				foreach(JobQuote quote in job.ListJobQuotes) {
					totalQuotedDollars+=PIn.Double(quote.Amount);
				}
				ODGridRow row=new ODGridRow() { Tag=job };
				row.Cells.Add(job.ToString());
				row.Cells.Add(Userods.GetName(job.OwnerNum));
				row.Cells.Add(job.OwnerAction.GetDescription());
				row.Cells.Add(job.PhaseCur.ToString());
				ODGridCell cell=new ODGridCell(jobPriority.ItemName);
				cell.CellColor=jobPriority.ItemColor;
				cell.ColorText=(job.Priority==listJobPriorities.FirstOrDefault(y => y.ItemValue.Contains("Urgent")).DefNum) ? Color.White : Color.Black;
				row.Cells.Add(cell);
				row.Cells.Add(Userods.GetName(job.UserNumExpert));
				row.Cells.Add(Userods.GetName(job.UserNumEngineer));
				row.Cells.Add(job.HoursEstimate.ToString());
				row.Cells.Add(job.ListJobTimeLogs.Where(x => x.DateTStamp>=DateTime.Today.AddDays(-7)).Sum(y => y.TimeReview.TotalHours).ToString());
				row.Cells.Add(job.HoursActual.ToString());
				row.Cells.Add(Math.Round((job.HoursActual/job.HoursEstimate*100),0).ToString());
				DateTime lastUpdated=job.ListJobLogs.Where(x => x.Description!="Job Viewed").Select(y => y.DateTimeEntry).OrderByDescending(x => x.Ticks).FirstOrDefault();
				row.Cells.Add(lastUpdated==DateTime.MinValue?"N/A":lastUpdated.ToShortDateString());
				gridJobs.Rows.Add(row);
			}
			gridJobs.EndUpdate();
			textTotalJobs.Text=totalJobs.ToString();
			textTotalHrsEst.Text=totalHrsEst.ToString();
			textTotalHrsSpent.Text=totalHrsSpent.ToString();
			textTotalQuote.Text=totalQuotedDollars.ToString();
			textTotalBugs.Text=totalBugs.ToString();
			textTotalFeatures.Text=totalFeatures.ToString();
			textCompletionPercent.Text=Math.Round(totalHrsSpent/totalHrsEst*100,0).ToString();
		}

		private void radioCurrentVersion_CheckedChanged(object sender,EventArgs e) {
			SetFilters();
		}

		private void radioNewVersion_CheckedChanged(object sender,EventArgs e) {
			SetFilters();
		}

		private void radioStaleJobs_CheckedChanged(object sender,EventArgs e) {
			SetFilters();
		}

		private void radioCustom_CheckedChanged(object sender,EventArgs e) {
			SetFilters();
		}

		///<summary>This is a very hard-coded method. We can change it when we need to though.</summary>
		private void SetFilters() {
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.JobPriorities);
			if(radioCurrentVersion.Checked) {
				listPhases.ClearSelected();
				listCategories.ClearSelected();
				listPriorities.ClearSelected();
				comboEngineers.SelectedIndex=0;
				textPatNum.Text="";
				checkRemoveApprovalJobs.Checked=false;
				checkRemoveNoQuote.Checked=false;
				textWeeksStale.Text="";
				listPhases.SetSelectedItem<JobPhase>(x => x.In(
					JobPhase.Definition,
					JobPhase.Development));
				listCategories.SetSelectedItem<JobCategory>(x => x.In(
					JobCategory.Bug,
					JobCategory.Enhancement,
					JobCategory.Feature,
					JobCategory.HqRequest,
					JobCategory.InternalRequest,
					JobCategory.ProgramBridge,
					JobCategory.Research));
				listPriorities.SetSelectedItem<Def>(x => x.DefNum.In(
					listDefs.FirstOrDefault(y => y.ItemValue.Split(',').Contains("Urgent")).DefNum,
					listDefs.FirstOrDefault(y => y.ItemValue.Split(',').Contains("High")).DefNum));
			}
			if(radioNewVersion.Checked) {
				listPhases.ClearSelected();
				listCategories.ClearSelected();
				listPriorities.ClearSelected();
				comboEngineers.SelectedIndex=0;
				textPatNum.Text="";
				checkRemoveApprovalJobs.Checked=false;
				checkRemoveNoQuote.Checked=false;
				textWeeksStale.Text="";
				listPhases.SetSelectedItem<JobPhase>(x => x.In(
					JobPhase.Concept,
					JobPhase.Quote,
					JobPhase.Definition,
					JobPhase.Development));
				listCategories.SetSelectedItem<JobCategory>(x => x.In(
					JobCategory.Bug,
					JobCategory.Enhancement,
					JobCategory.Feature,
					JobCategory.HqRequest,
					JobCategory.InternalRequest,
					JobCategory.ProgramBridge,
					JobCategory.Research));
				listPriorities.SetSelectedItem<Def>(x => x.DefNum.In(
					listDefs.FirstOrDefault(y => y.ItemValue.Split(',').Contains("MediumHigh")).DefNum,
					listDefs.FirstOrDefault(y => y.ItemValue.Split(',').Contains("Normal")).DefNum));
			}
			if(radioStaleJobs.Checked) {
				listPhases.ClearSelected();
				listCategories.ClearSelected();
				listPriorities.ClearSelected();
				comboEngineers.SelectedIndex=0;
				textPatNum.Text="";
				checkRemoveApprovalJobs.Checked=false;
				checkRemoveNoQuote.Checked=false;
				textWeeksStale.Text="12";
				listPhases.SetSelectedItem<JobPhase>(x => x.In(
					JobPhase.Concept,
					JobPhase.Quote,
					JobPhase.Definition,
					JobPhase.Development));
				listCategories.SetSelectedItem<JobCategory>(x => x.In(
					JobCategory.Bug,
					JobCategory.Enhancement,
					JobCategory.Feature,
					JobCategory.HqRequest,
					JobCategory.InternalRequest,
					JobCategory.ProgramBridge,
					JobCategory.Research));
				listPriorities.SetSelectedItem<Def>(x => x.DefNum.In(
					listDefs.FirstOrDefault(y => y.ItemValue.Split(',').Contains("Urgent")).DefNum,
					listDefs.FirstOrDefault(y => y.ItemValue.Split(',').Contains("High")).DefNum,
					listDefs.FirstOrDefault(y => y.ItemValue.Split(',').Contains("MediumHigh")).DefNum,
					listDefs.FirstOrDefault(y => y.ItemValue.Split(',').Contains("Normal")).DefNum,
					listDefs.FirstOrDefault(y => y.ItemValue.Split(',').Contains("Low")).DefNum,
					listDefs.FirstOrDefault(y => y.ItemValue.Split(',').Contains("OnHold")).DefNum));
			}
			if(radioCustom.Checked) {
				//comboEngineers.SelectedIndex=0;
				//textPatNum.Text="";
				//checkRemoveApprovalJobs.Checked=false;
				//checkRemoveNoQuote.Checked=false;
				//textWeeksStale.Text="";
				//listPhases.SelectedTags<JobPhase>().Add(JobPhase.Concept);
				//listPhases.SelectedTags<JobPhase>().Add(JobPhase.Quote);
				//listPhases.SelectedTags<JobPhase>().Add(JobPhase.Definition);
				//listPhases.SelectedTags<JobPhase>().Add(JobPhase.Development);
				//listCategories.SelectedTags<JobCategory>().Add(JobCategory.Bug);
				//listCategories.SelectedTags<JobCategory>().Add(JobCategory.Enhancement);
				//listCategories.SelectedTags<JobCategory>().Add(JobCategory.Feature);
				//listCategories.SelectedTags<JobCategory>().Add(JobCategory.HqRequest);
				//listCategories.SelectedTags<JobCategory>().Add(JobCategory.InternalRequest);
				//listCategories.SelectedTags<JobCategory>().Add(JobCategory.ProgramBridge);
				//listCategories.SelectedTags<JobCategory>().Add(JobCategory.Research);
				//listPriorities.SetSelectedItem<Def>(x => x.DefNum.In(
				//	listDefs.FirstOrDefault(y => y.ItemValue.Split(',').Contains("Urgent")).DefNum,
				//	listDefs.FirstOrDefault(y => y.ItemValue.Split(',').Contains("High")).DefNum,
				//	listDefs.FirstOrDefault(y => y.ItemValue.Split(',').Contains("MediumHigh")).DefNum));
			}
		}

		private List<Job> GetFilteredJobList() {
			int weeksOld=PIn.Int(textWeeksStale.Text);
			User selectedUser=comboEngineers.SelectedTag<User>();
			List<Job> listJobs=_listJobsAll;
			if(checkRemoveNoQuote.Checked) {
				listJobs=listJobs.Where(x => x.ListJobQuotes.Count()!=0).ToList();
			}
			if(checkRemoveApprovalJobs.Checked) {
				listJobs=listJobs.Where(x => !x.IsApprovalNeeded).ToList();
			}
			if(selectedUser.UserNum!=-1) {//All is not selected
				listJobs = listJobs.Where(x => (x.UserNumConcept.In(selectedUser.UserNum)
					|| x.UserNumEngineer.In(selectedUser.UserNum)
					|| x.UserNumExpert.In(selectedUser.UserNum)
					|| x.UserNumQuoter.In(selectedUser.UserNum))).ToList();
			}
			if(!string.IsNullOrEmpty(textPatNum.Text)) {
				listJobs=listJobs.Where(x => x.ListJobLinks.Any(y => y.LinkType==JobLinkType.Customer && y.FKey.ToString()==textPatNum.Text.Trim())
				|| x.ListJobQuotes.Any(y => y.PatNum.ToString()==textPatNum.Text.Trim())).ToList();
			}
			if(weeksOld>0) {
				listJobs=listJobs.Where(x => (x.ListJobLogs.Where(y => y.Description!="Job Viewed")
				.Select(z => z.DateTimeEntry).OrderByDescending(y => y.Ticks).FirstOrDefault()<DateTime.Now.AddDays(-(weeksOld*7)) 
					&& x.ListJobTimeLogs.Select(z => z.DateTStamp).OrderByDescending(y => y.Ticks).FirstOrDefault()<DateTime.Now.AddDays(-(weeksOld*7))
					&& x.ListJobReviews.Select(z => z.DateTStamp).OrderByDescending(y => y.Ticks).FirstOrDefault()<DateTime.Now.AddDays(-(weeksOld*7)))).ToList();
			}
			//TODO Make this work -- maybe add a dateimplemented column
			//if(dateRangeJobCompleted.GetDateTimeFrom()!=DateTime.MinValue || dateRangeJobCompleted.GetDateTimeTo()!=DateTime.MinValue) {
			//	listJobs=listJobs.RemoveAll(x => x.PhaseCur==JobPhase.Completed && x.ListJobLogs.
			//}
			return listJobs=listJobs.Where(x => x.Priority.In(listPriorities.SelectedTags<Def>().Select(y => y.DefNum))
					&& x.PhaseCur.In(listPhases.SelectedTags<JobPhase>())
					&& x.Category.In(listCategories.SelectedTags<JobCategory>())).OrderByDescending(x => x.HoursActual).ToList();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGridJobs(GetFilteredJobList());
		}

		private void gridJobs_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormJobManager.OpenNonModalJob((Job)gridJobs.Rows[e.Row].Tag,_listJobsAll);
		}

		private void butPatSelect_Click(object sender,EventArgs e) {
			FormPatientSelect FormPS=new FormPatientSelect();
			FormPS.SelectionModeOnly=true;
			FormPS.ShowDialog();
			if(FormPS.DialogResult!=DialogResult.OK) {
				return;
			}
			textPatNum.Text=FormPS.SelectedPatNum.ToString();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			_pagesPrinted=0;	
			_headingPrinted=false;
			PrinterL.TryPrintOrDebugRpPreview(pd_PrintPage,Lan.g(this,"Job List report printed"),PrintoutOrientation.Landscape);
		}

		private void pd_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!_headingPrinted) {
				text=Lan.g(this,"Job List");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				_headingPrinted=true;
				_headingPrintH=yPos;
			}
			#endregion
			yPos=gridJobs.PrintPage(g,_pagesPrinted,bounds,_headingPrintH);
			_pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		private void butExport_Click(object sender,System.EventArgs e) {
			SaveFileDialog saveFileDialog=new SaveFileDialog();
			saveFileDialog.AddExtension=true;
			saveFileDialog.FileName="Jobs List";
			if(!Directory.Exists(Preferences.GetString(PrefName.ExportPath))) {
				try {
					Directory.CreateDirectory(Preferences.GetString(PrefName.ExportPath));
					saveFileDialog.InitialDirectory=Preferences.GetString(PrefName.ExportPath);
				}
				catch {
					//initialDirectory will be blank
				}
			}
			else {
				saveFileDialog.InitialDirectory=Preferences.GetString(PrefName.ExportPath);
			}
			saveFileDialog.Filter="Text files(*.txt)|*.txt|Excel Files(*.xls)|*.xls|All files(*.*)|*.*";
			saveFileDialog.FilterIndex=0;
			if(saveFileDialog.ShowDialog()!=DialogResult.OK) {
				return;
			}
			try {
				using(StreamWriter sw=new StreamWriter(saveFileDialog.FileName,false))
				//new FileStream(,FileMode.Create,FileAccess.Write,FileShare.Read)))
				{
					String line="";
					for(int i=0;i<gridJobs.Columns.Count;i++) {
						line+=gridJobs.Columns[i].Heading+"\t";
					}
					sw.WriteLine(line);
					for(int i=0;i<gridJobs.Rows.Count;i++) {
						line="";
						for(int j=0;j<gridJobs.Columns.Count;j++) {
							line+=gridJobs.Rows[i].Cells[j].Text;
							if(j<gridJobs.Columns.Count-1) {
								line+="\t";
							}
						}
						sw.WriteLine(line);
					}
				}
			}
			catch {
				MessageBox.Show(Lan.g(this,"File in use by another program.  Close and try again."));
				return;
			}
			MessageBox.Show(Lan.g(this,"File created successfully"));
		}
		#endregion

		#region Shortcuts
		private void butHomePage_Click(object sender,EventArgs e) {
			try{
				Process.Start("https://www.opendental.com");
			}
			catch{
				MsgBox.Show(this,"Could not open Home Page");
			}
		}

		private void butForum_Click(object sender,EventArgs e) {
			try{
				Process.Start("http://opendentalsoft.com/forum/");
			}
			catch{
				MsgBox.Show(this,"Could not open Forum");
			}
		}

		private void butBugManager_Click(object sender,EventArgs e) {
			try{
				Process.Start("http://opendentalsoft.com:1942/ODBugTracker/BugList.aspx");
			}
			catch{
				MsgBox.Show(this,"Could not open Bugs Manager");
			}
		}

		private void butSchema_Click(object sender,EventArgs e) {
			try{
				Process.Start("https://www.opendental.com/OpenDentalDocumentation18-2.xml");
			}
			catch{
				MsgBox.Show(this,"Could not open Schema");
			}
		}

		private void butBugsList_Click(object sender,EventArgs e) {
			try{
				Process.Start("http://opendentalsoft.com:1942/ODBugTracker/PreviousVersions.aspx");
			}
			catch{
				MsgBox.Show(this,"Could not open Bugs List");
			}
		}

		private void butUserGroup_Click(object sender,EventArgs e) {
			try{
				Process.Start("https://www.facebook.com/groups/OpenDentalUsers/");
			}
			catch{
				MsgBox.Show(this,"Could not open User Group");
			}
		}

		private void butSchedules_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Schedules)) {
				return;
			}
			FormScheduleDayEdit FormSDE=new FormScheduleDayEdit(DateTime.Now,Clinics.ClinicNum);
			FormSDE.ShowOkSchedule=true;
			FormSDE.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Schedules,0,"");
			if(FormSDE.GotoScheduleOnClose) {
				FormSchedule FormS = new FormSchedule();
				FormS.ShowDialog();
			}
		}
		#endregion
	}
}