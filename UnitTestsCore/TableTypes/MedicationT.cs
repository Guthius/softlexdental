using System;
using System.Collections.Generic;
using System.Text;
using CodeBase;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class MedicationT {

		///<summary>Inserts the new medication and returns it.</summary>
		public static Medication CreateMedication(string medName="",string rxCui="") {
			Medication medication=new Medication();
			medication.Description=medName;
			if(medication.Description=="") {
				medication.Description="Med_"+MiscUtils.CreateRandomAlphaNumericString(8);
			}
			medication.RxCui=PIn.Long(rxCui,false);
			if(medication.RxCui!=0 && RxNorms.GetByRxCUI(rxCui)==null) {
				RxNorm rxNorm=new RxNorm();
				rxNorm.RxCui=rxCui;
				rxNorm.Description=medication.Description;
				RxNorms.Insert(rxNorm);
			}
			Medications.Insert(medication);
			return medication;
		}

		public static void ClearMedicationTable() {
			string command="DELETE FROM medication";
			DataCore.NonQ(command);
		}
	}
}
