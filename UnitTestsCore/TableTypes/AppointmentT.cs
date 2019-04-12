using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class AppointmentT {

		///<summary></summary>
		public static Appointment CreateAppointment(long patNum,DateTime aptDateTime,long opNum,long provNum,long provHyg=0,string pattern="//XXXX//"
			,long clinicNum=0,bool isHygiene=false,ApptStatus aptStatus=ApptStatus.Scheduled,ApptPriority priority=ApptPriority.Normal,string aptNote=""
			,long appointmentTypeNum=0)
		{
			Appointment appointment=new Appointment();
			appointment.AptDateTime=aptDateTime;
			appointment.AptStatus=aptStatus;
			appointment.ClinicNum=clinicNum;
			appointment.IsHygiene=isHygiene;
			appointment.Op=opNum;
			appointment.PatNum=patNum;
			appointment.Pattern=pattern;
			appointment.ProvNum=provNum;
			appointment.ProvHyg=provHyg;
			appointment.Priority=priority;
			appointment.Note=aptNote;
			appointment.AppointmentTypeNum=appointmentTypeNum;
			Appointments.Insert(appointment);
			return appointment;
		}

		public static Appointment CreateAppointmentFromRecall(Recall recall,Patient pat,DateTime aptDateTime,long opNum,long provNum) {
			RecallType recallType=RecallTypes.GetFirstOrDefault(x => x.RecallTypeNum==recall.RecallTypeNum);
			Appointment appt=CreateAppointment(pat.PatNum,aptDateTime,opNum,provNum,pattern:recallType.TimePattern,clinicNum:pat.ClinicNum);
			foreach(string procCode in RecallTypes.GetProcs(recallType.RecallTypeNum)) {
				ProcedureT.CreateProcedure(pat,procCode,ProcStat.TP,"",50,appt.AptDateTime,provNum:provNum,aptNum:appt.AptNum);
			}
			return appt;
		}

		///<summary>Deletes everything from the appointment table.  Does not truncate the table so that PKs are not reused on accident.</summary>
		public static void ClearAppointmentTable() {
			string command="DELETE FROM appointment WHERE AptNum > 0";
			DataCore.NonQ(command);
		}

		///<summary>Optionally pass in daysForSchedule to create a schedule for the number of days for each provider. Schedules will all be default 8 to 4. </summary>
		public static AppointmentSearchData CreateDataForAppointmentSearch(long numProvs,long numOps,long numClinics,int daysForSchedule=0) {
			AppointmentSearchData appSearchData=new UnitTestsCore.AppointmentSearchData();
			appSearchData.Patient=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name+"Pat");
			for(int i = 0;i < numOps;i++) {
				Operatory op=OperatoryT.CreateOperatory("abbr"+i,"opName"+i);
				appSearchData.ListOps.Add(op);
			}
			for(int i = 0;i < numClinics;i++) {
				Clinic clinic=ClinicT.CreateClinic("Clinic "+i);
				appSearchData.ListClinics.Add(clinic);
			}
			//loop through and create all the providers. 
			for(int i = 0;i < numProvs;i++) {
				long prov=ProviderT.CreateProvider(MethodBase.GetCurrentMethod().Name+i);
				appSearchData.ListProvNums.Add(prov);
				//create a general schedule for each prov. Can manipulate later.
				if(daysForSchedule>0) {
					for(int j = 0;j < daysForSchedule;j++) {
						if(DateTime.Today.AddDays(j).DayOfWeek==DayOfWeek.Saturday || DateTime.Today.AddDays(j).DayOfWeek==DayOfWeek.Sunday) {
							daysForSchedule++;//add another day to the loop since we want to skip but still add the schedule on a weekday
							continue;
						}
						Schedule sched=ScheduleT.CreateSchedule(DateTime.Today.AddDays(j)
							,new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.AddDays(j).Day,8,0,0).TimeOfDay
							,new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.AddDays(j).Day,16,0,0).TimeOfDay
							,ScheduleType.Provider,provNum:prov);//default to tomorrow from 8-4
					}
				}
			}
			return appSearchData;
		}

		///<summary>Optionally pass in daysForSchedule to create a schedule for the number of days for each provider. Schedules will all be default 8 to 4. 
		///Note this is for non dynamic scheduling. daysForSchedule will include today, so pass in one extra if search starts with tomorrow.</summary>
		public static AppointmentSearchData CreateScheduleAndOpsForProv(long numOps,long numClinics,long provNum,int daysForSchedule=0,long hygNum=0,bool isDynamic=false) 
		{
			AppointmentSearchData appSearchData=new UnitTestsCore.AppointmentSearchData();
			appSearchData.Patient=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name+"Pat");
			for(int i = 0;i < numOps;i++) {
				Operatory op=OperatoryT.CreateOperatory("abbr"+i,"opName"+i,provDentist:provNum,provHygienist:hygNum);
				appSearchData.ListOps.Add(op);
			}
			for(int i = 0;i < numClinics;i++) {
				Clinic clinic=ClinicT.CreateClinic("Clinic "+i);
				appSearchData.ListClinics.Add(clinic);
			}
			//create a general schedule for each prov. Can manipulate later.
			if(daysForSchedule>0) {
				for(int j = 0;j < daysForSchedule;j++) {
					if(DateTime.Today.AddDays(j).DayOfWeek==DayOfWeek.Saturday || DateTime.Today.AddDays(j).DayOfWeek==DayOfWeek.Sunday) {
						daysForSchedule++;//add another day to the loop since we want to skip but still add the schedule on a weekday
						continue;
					}
					Schedule sched;
					if(isDynamic) {
						//create schedule for provider but do not assign it to an operatory
						sched=ScheduleT.CreateSchedule(DateTime.Today.AddDays(j)
						,new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.AddDays(j).Day,8,0,0).TimeOfDay
						,new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.AddDays(j).Day,16,0,0).TimeOfDay
						,ScheduleType.Provider,provNum:provNum,listOpNums:new List<long>() { });//default to 8-4
					}
					else {
						sched=ScheduleT.CreateSchedule(DateTime.Today.AddDays(j)
						,new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.AddDays(j).Day,8,0,0).TimeOfDay
						,new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.AddDays(j).Day,16,0,0).TimeOfDay
						,ScheduleType.Provider,provNum:provNum,listOpNums:appSearchData.ListOps.Select(x => x.OperatoryNum).ToList());//default to 8-4
					}
					
					appSearchData.ListSchedules.Add(sched);
				}
			}
			return appSearchData;
		}
	
	}

	

	public class AppointmentSearchData {//this can maybe be moved to appointments tests and made private. Same with the helper method. 
		public Patient Patient;
		public List<long> ListProvNums=new List<long>();
		public List<Operatory> ListOps=new List<Operatory>();
		public List<Clinic> ListClinics=new List<Clinic>();
		public List<Schedule> ListSchedules=new List<Schedule>();//just for convenience when setting up test
	}
}
