using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using OpenDentBusiness;
using OpenDentBusiness.UI;

namespace OpenDental{
	public class ApptViewItemL{
		///<summary>A list of the ApptViewItems for the current view.</summary>
		public static List<AppointmentViewItem> ForCurView;
		///<summary>Subset of ForCurView. Just items for rowElements, including apptfielddefs. If no view is selected, then the elements are filled with default info.</summary>
		public static List<AppointmentViewItem> ApptRows;
		public static AppointmentView ApptViewCur;

		//Deprecated function, we no longer store the index of the veiw in the computer pref table
		//public static void GetForCurView(int indexInList,bool isWeekly,List<Schedule> dailySched) {
		//	if(indexInList<0){//might be -1 or -2
		//		GetForCurView(null,isWeekly,dailySched);
		//	}
		//	else{
		//		GetForCurView(ApptViewC.List[indexInList],isWeekly,dailySched);
		//	}
		//}

		///<summary>Gets the 'visible' operatories for todays date with the currently selected appointment view.  This is strictly used when filtering the
		///waiting room for clinics.  Pass in null for the dailySched if this is a weekly view.</summary>
		public static List<Operatory> GetOpsForApptView(AppointmentView apptViewCur,bool isWeekly,List<Schedule> dailySched) {
			List<AppointmentViewItem> forCurView;
			List<Provider> visProvs;
			List<Operatory> visOps;
			List<AppointmentViewItem> apptRows;
			int rowsPerIncr;
			FillForApptView(isWeekly,apptViewCur,out visProvs,out visOps,out forCurView,out apptRows,out rowsPerIncr,false);
			AddOpsForScheduledProvs(isWeekly,dailySched,apptViewCur,ref visOps);
			visOps.Sort(CompareOps);
			return visOps;
		}

		///<summary>Gets (list)ForCurView, ApptDrawing.VisOps, ApptDrawing.VisProvs, and ApptRows.  Also sets TwoRows.  Pass in null for the dailySched if this is a weekly view or if in FormApptViewEdit.</summary>
		public static void GetForCurView(AppointmentView apptViewCur,bool isWeekly,List<Schedule> dailySched) {
			ApptViewCur=apptViewCur;
			FillForApptView(isWeekly,ApptViewCur,out ApptDrawing.VisProvs,out ApptDrawing.VisOps,out ForCurView,out ApptRows,out ApptDrawing.RowsPerIncr);
			AddOpsForScheduledProvs(isWeekly,dailySched,ApptViewCur,ref ApptDrawing.VisOps);
			ApptDrawing.VisOps.Sort(CompareOps);
			ApptDrawing.VisProvs.Sort(CompareProvs);
			ApptDrawing.DictOpNumToColumnNum=ApptDrawing.VisOps.ToDictionary(x => x.Id,x => ApptDrawing.VisOps.FindIndex(y => y.Id==x.Id));
			ApptDrawing.DictProvNumToColumnNum=ApptDrawing.VisProvs.ToDictionary(x => x.ProvNum,x => ApptDrawing.VisProvs.FindIndex(y => y.ProvNum==x.ProvNum));
		}

		///<summary>Fills visProvs, visOps, forCurView, apptRows, and rowsPerIncr based on the appointment view passed in and whether it is for the week view or not.  This method uses 'out' variables so that the encompassing logic doesn't ALWAYS affect the global static variables used to draw the appointment views.  We don't want the following logic to affect the global static variables in the case where we are trying to get information needed to filter the waiting room.</summary>
		private static void FillForApptView(bool isWeekly,AppointmentView apptViewCur,out List<Provider> visProvs,out List<Operatory> visOps,
			out List<AppointmentViewItem> forCurView,out List<AppointmentViewItem> apptRows,out int rowsPerIncr,bool isFillVisProvs=true)
		{
			forCurView=new List<AppointmentViewItem>();
			visProvs=new List<Provider>();
			visOps=new List<Operatory>();
			apptRows=new List<AppointmentViewItem>();
			//If there are no appointment views set up (therefore, none selected), then use a hard-coded default view.
			if(apptViewCur==null) {
				//make visible ops exactly the same as the short ops list (all except hidden)
				visOps.AddRange(
					Operatory.GetByClinic(Clinics.ClinicId)
				);
				if(isFillVisProvs) {

						foreach(Operatory op in visOps) {
							Provider provDent=Providers.GetProv(op.ProvDentistId.GetValueOrDefault());
							Provider provHyg=Providers.GetProv(op.ProvHygienistId.GetValueOrDefault());
							if(provDent!=null) {
								visProvs.Add(provDent);
							}
							if(provHyg!=null) {
								visProvs.Add(provHyg);
							}
						}

				}
				//Hard coded elements showing
				apptRows.Add(new AppointmentViewItem("PatientName",0,Color.Black));
				apptRows.Add(new AppointmentViewItem("ASAP",1,Color.DarkRed));
				apptRows.Add(new AppointmentViewItem("MedUrgNote",2,Color.DarkRed));
				apptRows.Add(new AppointmentViewItem("PremedFlag",3,Color.DarkRed));
				apptRows.Add(new AppointmentViewItem("Lab",4,Color.DarkRed));
				apptRows.Add(new AppointmentViewItem("Procs",5,Color.Black));
				apptRows.Add(new AppointmentViewItem("Note",6,Color.Black));
				rowsPerIncr=1;
			}
			//An appointment view is selected, so add provs and ops from the view to our lists of indexes.
			else {
				List<AppointmentViewItem> listApptViewItems=AppointmentViewItem.GetByAppointmentView(apptViewCur.Id).ToList();
				for(int i=0;i<listApptViewItems.Count;i++) {
					forCurView.Add(listApptViewItems[i]);
					if(listApptViewItems[i].OperatoryId.HasValue) {//op
						if(apptViewCur.OnlyScheduledProviders && !isWeekly) {
							continue;//handled below in AddOpsForScheduledProvs 
						}
						Operatory op= Operatory.GetById(listApptViewItems[i].OperatoryId.Value);
						if(op!=null) {
							visOps.Add(op);
						}
					}
					else if(listApptViewItems[i].ProviderId>0) {//prov
						if(!isFillVisProvs) {
							continue;
						}
						Provider prov=Providers.GetFirstOrDefault(x => x.ProvNum==listApptViewItems[i].ProviderId,true);
						if(prov!=null) {
							visProvs.Add(prov);
						}
					}
					else {//element or apptfielddef
						apptRows.Add(listApptViewItems[i]);
					}
				}
				rowsPerIncr=apptViewCur.RowsPerIncrement;
			}
			//Remove any duplicates before return.
			visOps=visOps.GroupBy(x => x.Id).Select(x => x.First()).ToList();
			if(isFillVisProvs) {
				visProvs=visProvs.GroupBy(x => x.ProvNum).Select(x => x.First()).ToList();
			}
		}

		///<summary>When looking at a daily appointment module and the current appointment view is has 'OnlyScheduleProvs' turned on, this method will dynamically add additional operatories to visOps for providers that are scheduled to work.</summary>
		private static void AddOpsForScheduledProvs(bool isWeekly,List<Schedule> dailySched,AppointmentView apptViewCur,ref List<Operatory> visOps) {
			//if this appt view has the option to show only scheduled providers and this is daily view.
			//Remember that there is no intelligence in weekly view for this option, and it behaves just like it always did.
			if(apptViewCur==null 
				|| dailySched==null
				|| apptViewCur==null
				|| visOps==null
				|| !apptViewCur.OnlyScheduledProviders
				|| isWeekly) 
			{
				return;
			}
			//intelligently decide what ops to show.  It's based on the schedule for the day.
			//visOps will be totally empty right now because it looped out of the above section of code.
			List<long> listSchedOps;
			bool opAdded;
			int indexOp;
			List<Operatory> listOpsShort= Operatory.All(true).ToList();
			List<long> listApptViewOpNums=AppointmentView.GetOperatoryIds(apptViewCur.Id).ToList();
			for(int i=0;i<listOpsShort.Count;i++) {//loop through all ops for all views (except the hidden ones, of course)
				//If this operatory was not one of the selected Ops from the Appt View Edit window, skip it.
				if(!listApptViewOpNums.Contains(listOpsShort[i].Id)) {
					continue;
				}
				//find any applicable sched for the op
				opAdded=false;
				for(int s=0;s<dailySched.Count;s++) {
					if(dailySched[s].SchedType!=ScheduleType.Provider) {
						continue;
					}
					if(dailySched[s].StartTime==new TimeSpan(0)) {//skip if block starts at midnight.
						continue;
					}
					if(dailySched[s].StartTime==dailySched[s].StopTime) {//skip if block has no length.
						continue;
					}
					if(apptViewCur.OnlyScheduleAfter > new TimeSpan(0,0,0)) {
						if(dailySched[s].StartTime < apptViewCur.OnlyScheduleAfter
								|| dailySched[s].StopTime < apptViewCur.OnlyScheduleAfter) 
						{
							continue;
						}
					}
					if(apptViewCur.OnlyScheduleBefore > new TimeSpan(0,0,0)) {
						if(dailySched[s].StartTime > apptViewCur.OnlyScheduleBefore
								|| dailySched[s].StopTime > apptViewCur.OnlyScheduleBefore) 
						{
							continue;
						}
					}
					//this 'sched' must apply to this situation.
					//listSchedOps is the ops for this 'sched'.
					listSchedOps=dailySched[s].Ops;
					//Add all the ops for this 'sched' to the list of visible ops
					for(int p=0;p<listSchedOps.Count;p++) {
						//Filter the ops if the clinic option was set for the appt view.
						if(apptViewCur.ClinicId>0 && apptViewCur.ClinicId!= Operatory.GetById(listSchedOps[p]).ClinicId) {
							continue;
						}
						if(listSchedOps[p]==listOpsShort[i].Id) {
							Operatory op=listOpsShort[i];
							indexOp= Operatory.GetById(listSchedOps[p]).SortOrder;
							if(indexOp!=-1 && !visOps.Contains(op)) {//prevents adding duplicate ops
								visOps.Add(op);
								opAdded=true;
								break;
							}
						}
					}
					//If the provider is not scheduled to any op(s), add their default op(s).
					if(listOpsShort[i].ProvDentistId==dailySched[s].ProvNum && listSchedOps.Count==0) {//only if the sched does not specify any ops
						//Only add the op if the clinic option was not set in the appt view or if the op is assigned to that clinic.
						if(apptViewCur.ClinicId==0 || apptViewCur.ClinicId==listOpsShort[i].ClinicId) {
							indexOp= Operatory.GetById(listOpsShort[i].Id).SortOrder;
							if(indexOp!=-1 && !visOps.Contains(listOpsShort[i])) {
								visOps.Add(listOpsShort[i]);
								opAdded=true;
							}
						}
					}
					if(opAdded) {
						break;//break out of the loop of schedules.  Continue with the next op.
					}
				}
			}
			//Remove any duplicates before return.
			visOps=visOps.GroupBy(x => x.Id).Select(x => x.First()).ToList();
		}

		///<summary>Sorts list of operatories by ItemOrder.</summary>
		private static int CompareOps(Operatory op1,Operatory op2) {
			if(op1.SortOrder<op2.SortOrder) {
				return -1;
			}
			else if(op1.SortOrder>op2.SortOrder) {
				return 1;
			}
			return 0;
		}

		///<summary>Sorts list of providers by ItemOrder.</summary>
		private static int CompareProvs(Provider prov1,Provider prov2) {
			if(prov1.ItemOrder<prov2.ItemOrder) {
				return -1;
			}
			else if(prov1.ItemOrder>prov2.ItemOrder) {
				return 1;
			}
			return 0;
		}

		///<summary>Only used in FormApptViewEdit. Must have run GetForCurView first.</summary>
		public static bool OpIsInView(long opNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<ForCurView.Count;i++) {
				if(ForCurView[i].OperatoryId==opNum)
					return true;
			}
			return false;
		}

		///<summary>Only used in ApptViewItem setup and ContrAppt (for search function - search for appt with prov in this view). Must have run GetForCurView first.</summary>
		public static bool ProvIsInView(long provNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<ForCurView.Count;i++) {
				if(ForCurView[i].ProviderId==provNum)
					return true;
			}
			return false;
		}



	}
}
