using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OpenDentBusiness
{
    public class Family
    {
        /// <summary>
        /// List of patients in the family.
        /// </summary>
        public Patient[] Members { get; set; }

        public Family()
        {
        }

        public Family(List<Patient> patients) => Members = patients.ToArray();

        /// <summary>
        /// Gets the guarantor of the family.
        /// </summary>
        public Patient Guarantor => Members.FirstOrDefault(x => x.Guarantor == x.PatNum);

        /// <summary>
        /// Tries to get the LastName, FirstName of the patient from this family.
        /// If not found, then gets the name from the database.
        /// </summary>
        public string GetNameInFamLF(long patientId)
        {
            foreach (var patient in Members)
            {
                if (patient.PatNum == patientId)
                {
                    return patient.GetNameLF();
                }
            }

            return GetLim(patientId).GetNameLF();
        }

        /// <summary>
        /// Gets last, (preferred) first middle
        /// /summary>
        public string GetNameInFamLFI(int myi)
        {
            return Patients.GetNameLF(Members[myi].LName, Members[myi].FName, Members[myi].Preferred, Members[myi].MiddleI);
        }

        /// <summary>
        /// Gets a formatted name from the family list.
        /// If the patient is not in the family list, then it gets that info from the database.
        /// </summary>
        public string GetNameInFamFL(long myPatNum)
        {
            for (int i = 0; i < Members.Length; i++)
            {
                if (Members[i].PatNum == myPatNum)
                {
                    return Members[i].GetNameFL();
                }
            }
            return GetLim(myPatNum).GetNameFL();
        }

        /// <summary>
        /// Gets a formatted name from the family list.
        /// If the patient is not in the family list, then it gets that info from the database.
        /// </summary>
        public string GetNameInFamFLnoPref(long patientId)
        {
            for (int i = 0; i < Members.Length; i++)
            {
                if (Members[i].PatNum == patientId)
                {
                    return Members[i].GetNameFLnoPref();
                }
            }
            return GetLim(patientId).GetNameFLnoPref();
        }

        /// <summary>
        /// Gets (preferred)first middle last
        /// </summary>
        public string GetNameInFamFLI(int index)
        {
            string result = "";
            if (Members[index].Preferred != "")
            {
                result = "'" + Members[index].Preferred + "' ";
            }

            result += Patients.GetNameFLnoPref(Members[index].LName, Members[index].FName, Members[index].MiddleI);

            return result;
        }

        /// <summary>
        /// Gets first name from the family list.
        /// If the patient is not in the family list, then it gets that info from the database.
        /// Includes preferred.
        /// </summary>
        public string GetNameInFamFirst(long myPatNum)
        {
            for (int i = 0; i < Members.Length; i++)
            {
                if (Members[i].PatNum == myPatNum)
                {
                    return Members[i].GetNameFirst();
                }
            }
            return GetLim(myPatNum).GetNameFirst();
        }

        /// <summary>
        /// Gets first name from the family list. 
        /// If the patient is not in the family list, then it gets that info from the database.
        /// Includes preferred and last name.
        /// </summary>
        public string GetNameInFamFirstOrPreferredOrLast(long myPatNum)
        {
            for (int i = 0; i < Members.Length; i++)
            {
                if (Members[i].PatNum == myPatNum)
                {
                    return Members[i].GetNameFirstOrPreferredOrLast();
                }
            }
            return GetLim(myPatNum).GetNameFirstOrPreferredOrLast();
        }

        /// <summary>
        /// The index of the patient within the family. 
        /// Returns -1 if not found.
        /// </summary>
        public int GetIndex(long patientId)
        {
            for (int i = 0; i < Members.Length; i++)
            {
                if (Members[i].PatNum == patientId)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets a copy of a specific patient from within the family.
        /// </summary>
        public Patient GetPatient(long patientId)
        {
            foreach (var patient in Members)
            {
                if (patient.PatNum == patientId)
                {
                    return patient;
                }
            }

            return default;
        }

        /// <summary>
        /// Duplicate of the same class in Patients. 
        /// Gets nine of the most useful fields from the db for the given patnum.
        /// </summary>
        public static Patient GetLim(long patientId)
        {
            if (patientId == 0) return new Patient();
            
            string command =
                "SELECT PatNum,LName,FName,MiddleI,Preferred,CreditType,Guarantor,HasIns,SSN " +
                "FROM patient " +
                "WHERE PatNum = '" + patientId.ToString() + "'";

            DataTable table = Db.GetTable(command);
            if (table.Rows.Count == 0)
            {
                return new Patient();
            }

            return new Patient
            {
                PatNum = PIn.Int(table.Rows[0][0].ToString()),
                LName = PIn.String(table.Rows[0][1].ToString()),
                FName = PIn.String(table.Rows[0][2].ToString()),
                MiddleI = PIn.String(table.Rows[0][3].ToString()),
                Preferred = PIn.String(table.Rows[0][4].ToString()),
                CreditType = PIn.String(table.Rows[0][5].ToString()),
                Guarantor = PIn.Long(table.Rows[0][6].ToString()),
                HasIns = PIn.String(table.Rows[0][7].ToString()),
                SSN = PIn.String(table.Rows[0][8].ToString())
            };
        }

        public bool IsInFamily(long patientId) => Members.Any(x => x.PatNum == patientId);

        public bool HasArchivedMember() => Members.Any(x => x.PatStatus == PatientStatus.Archived);

        /// <summary>
        /// Replaces all patient family fields in the given message with the given patient's family 
        /// information. Returns the resulting string.
        /// Replaces: [FamilyList]
        /// </summary>
        public static string ReplaceFamily(string message, Patient pat)
        {
            if (pat == null) return message;
            
            var family = Patients.GetFamily(pat.PatNum);
            if (family == null)
            {
                return message;
            }

            return message.Replace("[FamilyList]", 
                string.Join(",", 
                    family.Members.Select(x => Patients.GetNameFirstOrPrefML(x.LName, x.FName, x.Preferred, x.MiddleI))));
        }
    }
}
