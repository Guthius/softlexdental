﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class PatientT {
		///<summary>Creates a patient.  Practice default provider and billing type.</summary>
		public static Patient CreatePatient(string suffix="",long priProvNum=0,long clinicNum=0,string email="",string phone="",
			ContactMethod contactMethod=ContactMethod.Email,string lName="",string fName="",string preferredName="",DateTime birthDate=default(DateTime)
			,long secProvNum=0,long guarantor=0,bool setPortalAccessInfo=false) 
		{
			Patient pat=new Patient {
				Email=email,
				PreferConfirmMethod=contactMethod,
				PreferContactConfidential=contactMethod,
				PreferContactMethod=contactMethod,
				PreferRecallMethod=contactMethod,
				HmPhone=phone,
				WirelessPhone=phone,
				IsNew=true,
				LName=lName+suffix,
				FName=fName+suffix,
				BillingType= Preferences.GetLong(PreferenceName.PracticeDefaultBillType),
				ClinicNum=clinicNum,
				Preferred=preferredName,
				Birthdate=birthDate,
				SecProv=secProvNum,
			};
			if(priProvNum!=0) {
				pat.PriProv=priProvNum;
			}
			else {
				pat.PriProv=Preferences.GetLong(PreferenceName.PracticeDefaultProv);//This causes standard fee sched to be 53.
			}
			if(setPortalAccessInfo) {
				pat.Address="666 Church St NE";
				pat.City="Salem";
				pat.State="OR";
				pat.Zip="97301";
				if(pat.Birthdate.Year<1880) {
					pat.Birthdate=new DateTime(1970,1,1);
				}
				if(string.IsNullOrEmpty(pat.WirelessPhone)) {
					pat.WirelessPhone="5555555555";
				}
			}
			Patients.Insert(pat,false);
			Patient oldPatient=pat.Copy();
			pat.Guarantor=pat.PatNum;
			if(guarantor > 0) {
				pat.Guarantor=guarantor;
			}
			if(lName=="") {
				pat.LName=pat.PatNum.ToString()+"L";
			}
			if(fName=="") {
				pat.FName=pat.PatNum.ToString()+"F";
			}
			Patients.Update(pat,oldPatient);
			return pat;
		}

		public static void SetGuarantor(Patient pat,long guarantorNum){
			Patient oldPatient=pat.Copy();
			pat.Guarantor=guarantorNum;
			Patients.Update(pat,oldPatient);
		}
		
		///<summary>Deletes everything from the patient table.  Does not truncate the table so that PKs are not reused on accident.</summary>
		public static void ClearPatientTable() {
			string command="DELETE FROM patient WHERE PatNum > 0";
			DataCore.NonQ(command);
		}

	}
}
