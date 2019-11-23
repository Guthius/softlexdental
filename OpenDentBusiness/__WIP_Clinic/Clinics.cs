using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace OpenDentBusiness
{
    public class Clinics
    {
        /// <summary>
        /// Returns a dicitonary such that the key is a clinicNum and the value is a count of patients whith a matching patient.ClinicNum.
        /// Excludes all patients with PatStatus of Deleted, Archived, Deceased, or NonPatient unless IsAllStatuses is set to true.
        /// </summary>
        public static Dictionary<long, int> GetClinicalPatientCount(bool IsAllStatuses = false)
        {
            string command = "SELECT ClinicNum,COUNT(*) AS Count FROM patient ";
            if (!IsAllStatuses)
            {
                command += "WHERE PatStatus NOT IN (" + POut.Int((int)PatientStatus.Deleted) + "," + POut.Int((int)PatientStatus.Archived) + ","
                    + POut.Int((int)PatientStatus.Deceased) + "," + POut.Int((int)PatientStatus.NonPatient) + ") ";
            }
            command += "GROUP BY ClinicNum";
            return Db.GetTable(command).Select().ToDictionary(x => PIn.Long(x["ClinicNum"].ToString()), x => PIn.Int(x["Count"].ToString()));
        }

        /// <summary>
        /// Gets a SerializableDictionary of Lists of Clinics for given pharmacyNums.
        /// </summary>
        /// <param name="arrPharmacyNums">The primary key of the pharmacy.</param>
        public static Dictionary<long, List<Clinic>> GetDictClinicsForPharmacy(params long[] arrPharmacyNums)
        {
            Dictionary<long, List<Clinic>> dict = new Dictionary<long, List<Clinic>>();
            //if (arrPharmacyNums.Length == 0)
            //{
            //    return dict;
            //}
            //string command = "SELECT pharmclinic.PharmacyNum,clinic.* "
            //    + "FROM clinic "
            //    + "INNER JOIN pharmclinic ON pharmclinic.ClinicNum=clinic.ClinicNum "
            //    + "WHERE pharmclinic.PharmacyNum IN(" + string.Join(",", arrPharmacyNums) + ") "
            //    + "ORDER BY clinic.Abbr";
            //DataTable table = Db.GetTable(command);
            //List<Clinic> listClinics = Crud.ClinicCrud.TableToList(table);
            //for (int i = 0; i < table.Rows.Count; i++)
            //{
            //    long pharmacyNum = PIn.Long(table.Rows[i]["PharmacyNum"].ToString());
            //    Clinic clinic = listClinics[i];//1:1
            //    List<Clinic> listClinicsPharm;
            //    if (!dict.TryGetValue(pharmacyNum, out listClinicsPharm))
            //    {
            //        listClinicsPharm = new List<Clinic>();
            //        dict.Add(pharmacyNum, listClinicsPharm);
            //    }
            //    listClinicsPharm.Add(clinic);
            //}
            return dict;
        }

        private static long selectedClinicId;

        /// <summary>
        ///     <para>
        ///         Gets or sets the ID of the currently selected clinic.
        ///     </para>
        /// </summary>
        public static long ClinicId
        {
            get => selectedClinicId;
            set
            {
                if (selectedClinicId != value)
                {
                    selectedClinicId = value;
                    if (Security.CurrentUser == null)
                    {
                        return;
                    }

                    if (Preference.GetString(PreferenceName.LastClinicTrackingMethod) != "User")
                    {
                        return;
                    }

                    UserPreference.Update(Security.CurrentUser.Id, UserPreferenceName.LastSelectedClinic, value);
                }
            }
        }

        /// <summary>
        ///     <para>
        ///         Sets the ID of the currently selected clinic.
        ///     </para>
        /// </summary>
        public static bool RestoreLastSelectedClinicForUser()
        {
            selectedClinicId = 0;
            if (Security.CurrentUser == null)
            {
                return false;
            }

            var clinics = Clinic.GetByUser(Security.CurrentUser);

            switch (Preference.GetString(PreferenceName.LastClinicTrackingMethod, "none").ToLower())
            {
                case "workstation":
                    if (clinics.Any(clinic => clinic.Id == ComputerPrefs.LocalComputer.ClinicNum))
                    {
                        selectedClinicId = ComputerPrefs.LocalComputer.ClinicNum;
                    }
                    break;

                case "user":
                    var clinicId = UserPreference.GetLong(Security.CurrentUser.Id, UserPreferenceName.LastSelectedClinic);
                    if (clinicId > 0 && clinics.Any(c => c.Id == clinicId))
                    {
                        selectedClinicId = clinicId;
                    }
                    break;
            }

            if (selectedClinicId == 0)
            {
                if (Security.CurrentUser.ClinicId.HasValue && clinics.Any(clinic => clinic.Id == Security.CurrentUser.ClinicId.Value))
                {
                    selectedClinicId = Security.CurrentUser.ClinicId.Value;
                }
                else if (clinics.Count() > 0)
                {
                    selectedClinicId = clinics.ElementAt(0).Id;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public static void LogOff()
        {
            switch (Preference.GetString(PreferenceName.LastClinicTrackingMethod).ToLower())
            {
                case "workstation":
                    ComputerPrefs.LocalComputer.ClinicNum = ClinicId;
                    ComputerPrefs.Update(ComputerPrefs.LocalComputer);
                    break;

                case "user":
                case "none":
                default:
                    break;
            }

            UserPreference.Update(Security.CurrentUser.Id, UserPreferenceName.LastSelectedClinic, ClinicId);
        }

        public static void Delete(Clinic clinic)
        {
            //Check FK dependencies.
            #region Patients
            string command = "SELECT LName,FName FROM patient WHERE ClinicNum =" + clinic.Id;

            DataTable table = Db.GetTable(command);
            if (table.Rows.Count > 0)
            {
                string pats = "";
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    pats += "\r";
                    if (i == 15)
                    {
                        pats += "And" + " " + (table.Rows.Count - i) + " others";
                        break;
                    }
                    pats += table.Rows[i]["LName"].ToString() + ", " + table.Rows[i]["FName"].ToString();
                }
                throw new Exception("Cannot delete clinic because it is in use by the following patients:" + pats);
            }

            #endregion

            #region Payments
            command = "SELECT patient.LName,patient.FName FROM patient,payment "
                + "WHERE payment.ClinicNum =" + POut.Long(clinic.Id)
                + " AND patient.PatNum=payment.PatNum";
            table = Db.GetTable(command);
            if (table.Rows.Count > 0)
            {
                string pats = "";
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    pats += "\r";
                    if (i == 15)
                    {
                        pats += Lans.g("Clinics", "And") + " " + (table.Rows.Count - i) + " " + Lans.g("Clinics", "others");
                        break;
                    }
                    pats += table.Rows[i]["LName"].ToString() + ", " + table.Rows[i]["FName"].ToString();
                }
                throw new Exception(Lans.g("Clinics", "Cannot delete clinic because the following patients have payments using it:") + pats);
            }
            #endregion
            #region ClaimPayments
            command = "SELECT patient.LName,patient.FName FROM patient,claimproc,claimpayment "
                + "WHERE claimpayment.ClinicNum =" + POut.Long(clinic.Id)
                + " AND patient.PatNum=claimproc.PatNum"
                + " AND claimproc.ClaimPaymentNum=claimpayment.ClaimPaymentNum "
                + "GROUP BY patient.LName,patient.FName,claimpayment.ClaimPaymentNum";
            table = Db.GetTable(command);
            if (table.Rows.Count > 0)
            {
                string pats = "";
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    pats += "\r";
                    if (i == 15)
                    {
                        pats += Lans.g("Clinics", "And") + " " + (table.Rows.Count - i) + " " + Lans.g("Clinics", "others");
                        break;
                    }
                    pats += table.Rows[i]["LName"].ToString() + ", " + table.Rows[i]["FName"].ToString();
                }
                throw new Exception(Lans.g("Clinics", "Cannot delete clinic because the following patients have claim payments using it:") + pats);
            }
            #endregion
            #region Appointments
            command = "SELECT patient.LName,patient.FName FROM patient,appointment "
                + "WHERE appointment.ClinicNum =" + POut.Long(clinic.Id)
                + " AND patient.PatNum=appointment.PatNum";
            table = Db.GetTable(command);
            if (table.Rows.Count > 0)
            {
                string pats = "";
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    pats += "\r";
                    if (i == 15)
                    {
                        pats += Lans.g("Clinics", "And") + " " + (table.Rows.Count - i) + " " + Lans.g("Clinics", "others");
                        break;
                    }
                    pats += table.Rows[i]["LName"].ToString() + ", " + table.Rows[i]["FName"].ToString();
                }
                throw new Exception(Lans.g("Clinics", "Cannot delete clinic because the following patients have appointments using it:") + pats);
            }
            #endregion
            #region Procedures
            //reassign procedure.ClinicNum=0 if the procs are status D.
            command = "SELECT ProcNum FROM procedurelog WHERE ProcStatus=" + POut.Int((int)ProcStat.D) + " AND ClinicNum=" + POut.Long(clinic.Id);
            List<long> listProcNums = Db.GetListLong(command);
            if (listProcNums.Count > 0)
            {
                command = "UPDATE procedurelog SET ClinicNum=0 WHERE ProcNum IN (" + string.Join(",", listProcNums.Select(x => POut.Long(x))) + ")";
                Db.NonQ(command);
            }
            command = "SELECT patient.LName,patient.FName FROM patient,procedurelog "
                + "WHERE procedurelog.ClinicNum =" + POut.Long(clinic.Id)
                + " AND patient.PatNum=procedurelog.PatNum";
            table = Db.GetTable(command);
            if (table.Rows.Count > 0)
            {
                string pats = "";
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    pats += "\r";
                    if (i == 15)
                    {
                        pats += Lans.g("Clinics", "And") + " " + (table.Rows.Count - i) + " " + Lans.g("Clinics", "others");
                        break;
                    }
                    pats += table.Rows[i]["LName"].ToString() + ", " + table.Rows[i]["FName"].ToString();
                }
                throw new Exception(Lans.g("Clinics", "Cannot delete clinic because the following patients have procedures using it:") + pats);
            }
            #endregion
            #region Operatories
            command = "SELECT OpName FROM operatory WHERE ClinicNum =" + POut.Long(clinic.Id);
            table = Db.GetTable(command);
            if (table.Rows.Count > 0)
            {
                string ops = "";
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    ops += "\r";
                    if (i == 15)
                    {
                        ops += Lans.g("Clinics", "And") + " " + (table.Rows.Count - i) + " " + Lans.g("Clinics", "others");
                        break;
                    }
                    ops += table.Rows[i]["OpName"].ToString();
                }
                throw new Exception(Lans.g("Clinics", "Cannot delete clinic because the following operatories are using it:") + ops);
            }
            #endregion
            #region Userod
            command = "SELECT UserName FROM userod "
                + "WHERE ClinicNum =" + POut.Long(clinic.Id);
            table = Db.GetTable(command);
            if (table.Rows.Count > 0)
            {
                string userNames = "";
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    userNames += "\r";
                    if (i == 15)
                    {
                        userNames += Lans.g("Clinics", "And") + " " + (table.Rows.Count - i) + " " + Lans.g("Clinics", "others");
                        break;
                    }
                    userNames += table.Rows[i]["UserName"].ToString();
                }
                throw new Exception(Lans.g("Clinics", "Cannot delete clinic because the following Open Dental users are using it:") + userNames);
            }
            #endregion
            #region AlertSub

            table = DataConnection.ExecuteDataTable("SELECT DISTINCT UserNum FROM AlertSub WHERE ClinicNum =" + clinic.Id);
            if (table.Rows.Count > 0)
            {
                List<string> listUsers = new List<string>();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    long userNum = PIn.Long(table.Rows[i]["UserNum"].ToString());
                    User user = User.GetById(userNum);
                    if (user == null)
                    {//Should not happen.
                        continue;
                    }
                    listUsers.Add(user.UserName);
                }
                throw new Exception("Cannot delete clinic because the following Open Dental users are subscribed to it:\r\n" + String.Join("\r", listUsers.OrderBy(x => x).ToArray()));
            }

            #endregion
            #region UserClinics
            command = "SELECT userod.UserName FROM userclinic INNER JOIN userod ON userclinic.UserNum=userod.UserNum WHERE userclinic.ClinicNum=" + POut.Long(clinic.Id);
            table = Db.GetTable(command);
            if (table.Rows.Count > 0)
            {
                string users = "";
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (i > 0)
                    {
                        users += ",";
                    }
                    users += table.Rows[i][0].ToString();
                }
                throw new Exception("Cannot delete clinic because the following users are restricted to this clinic in security setup: " + users);
            }
            #endregion

            DataConnection.ExecuteNonQuery("DELETE FROM `clinics` WHERE id = " + clinic.Id);
        }

        ///<summary>Returns the patient's clinic based on the recall passed in.
        ///If the patient is no longer associated to a clinic, 
        ///  returns the clinic associated to the appointment (scheduled or completed) with the largest date.
        ///Returns null if the patient doesn't have a clinic or if the clinics feature is not activate.</summary>
        public static Clinic GetClinicForRecall(long recallNum)
        {
    //        string command = "SELECT patient.ClinicNum FROM patient "
    //            + "INNER JOIN recall ON patient.PatNum=recall.PatNum "
    //            + "WHERE recall.RecallNum=" + POut.Long(recallNum) + " "
    //            + DbHelper.LimitAnd(1);
    //        long patientClinicNum = PIn.Long(DataConnection.ExecuteScalar(command));
    //        if (patientClinicNum > 0)
    //        {
    //            return GetFirstOrDefault(x => x.Id == patientClinicNum);
    //        }
    //        //Patient does not have an assigned clinic.  Grab the clinic from a scheduled or completed appointment with the largest date.
    //        command = @"SELECT appointment.ClinicNum,appointment.AptDateTime 
				//FROM appointment
				//INNER JOIN recall ON appointment.PatNum=recall.PatNum AND recall.RecallNum=" + POut.Long(recallNum) + @"
				//WHERE appointment.AptStatus IN (" + POut.Int((int)ApptStatus.Scheduled) + "," + POut.Int((int)ApptStatus.Complete) + ")" + @"
				//ORDER BY AptDateTime DESC";
    //        command = DbHelper.LimitOrderBy(command, 1);
    //        long appointmentClinicNum = PIn.Long(DataConnection.ExecuteScalar(command));
    //        if (appointmentClinicNum > 0)
    //        {
    //            return GetFirstOrDefault(x => x.Id == appointmentClinicNum);
    //        }
            return null;
        }


        ///<summary>Replaces all clinic fields in the given message with the supplied clinic's information.  Returns the resulting string.
        ///Will use clinic information when available, otherwise defaults to practice info.
        ///Replaces: [OfficePhone], [OfficeFax], [OfficeName], [OfficeAddress]. </summary>
        public static string ReplaceOffice(string message, Clinic clinic)
        {
            string retVal = message;
            string officePhone = Preference.GetString(PreferenceName.PracticePhone);
            string officeFax = Preference.GetString(PreferenceName.PracticeFax);
            string officeName = Preference.GetString(PreferenceName.PracticeTitle);
            string officeAddr = Patients.GetAddressFull(
                Preference.GetString(PreferenceName.PracticeAddress),
                Preference.GetString(PreferenceName.PracticeAddress2),
                Preference.GetString(PreferenceName.PracticeCity),
                Preference.GetString(PreferenceName.PracticeST),
                Preference.GetString(PreferenceName.PracticeZip));
            if (clinic != null && !String.IsNullOrEmpty(clinic.Phone))
            {
                officePhone = clinic.Phone;
            }
            if (clinic != null && !String.IsNullOrEmpty(clinic.Fax))
            {
                officeFax = clinic.Fax;
            }
            if (clinic != null && !String.IsNullOrEmpty(clinic.Description))
            {
                officeName = clinic.Description;
            }
            if (clinic != null && !String.IsNullOrEmpty(clinic.AddressLine1))
            {
                officeAddr = Patients.GetAddressFull(clinic.AddressLine1, clinic.AddressLine2, clinic.City, clinic.State, clinic.Zip);
            }
            if (CultureInfo.CurrentCulture.Name == "en-US" && officePhone.Length == 10)
            {
                officePhone = "(" + officePhone.Substring(0, 3) + ")" + officePhone.Substring(3, 3) + "-" + officePhone.Substring(6);
            }
            if (CultureInfo.CurrentCulture.Name == "en-US" && officeFax.Length == 10)
            {
                officeFax = "(" + officeFax.Substring(0, 3) + ")" + officeFax.Substring(3, 3) + "-" + officeFax.Substring(6);
            }
            retVal = retVal.Replace("[OfficePhone]", officePhone);
            retVal = retVal.Replace("[OfficeFax]", officeFax);
            retVal = retVal.Replace("[OfficeName]", officeName);
            retVal = retVal.Replace("[OfficeAddress]", officeAddr);
            return retVal;
        }
    }
}