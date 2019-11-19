using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental
{
    public class MedicationL
    {
        /// <summary>
        ///     <para>
        ///         Downloads default medications list from OpenDental.com; returns filename of temp file.
        ///     </para>
        /// </summary>
        /// <returns>The local path of the downloaded file.</returns>
        /// <exception cref="WebException"></exception>
        public static string DownloadDefaultMedicationsFile()
        {
            var fileName = Path.GetTempFileName();

            using (var client = new WebClient())
            {
                client.DownloadFile("http://www.opendental.com/medications/DefaultMedications.txt", fileName);
            }

            return fileName;
        }

        /// <summary>
        /// Inserts any new medications in listNewMeds, as well as updating any existing medications in listExistingMeds in conflict with the corresponding new medication.
        /// </summary>
        public static int ImportMedications(List<Tuple<Medication, string>> listImportMeds, List<Medication> listMedsExisting)
        {
            int countImportedMedications = 0;
            foreach (Tuple<Medication, string> medGenPair in listImportMeds)
            {//Loop through new medications/given generic name pairs.
             //Find any duplicate existing medications with the new medication
                if (IsDuplicateMed(medGenPair, listMedsExisting))
                {
                    continue;//medNew already exists, skip it.
                }
                InsertNewMed(medGenPair, listMedsExisting);
                countImportedMedications++;
            }

            SecurityLog.Write(SecurityLogEvents.Setup, $"Imported {countImportedMedications} medications.");

            return countImportedMedications;
        }

        /// <summary>Determines if med is a duplicate of another Medication in listMedsExisting.
        ///Given medGenNamePair is a medication that we are checking and the given generic name if set.
        ///A duplicate is defined as MedName is equal, GenericName is equal, RxCui is equal and either Notes is equal or not defined.
        ///A new medication with all properties being equal to an existing medication except with a blank Notes property is considered to be a 
        ///duplicate, as it is likely the existing Medication is simply a user edited version of the same Medication.</summary>
        private static bool IsDuplicateMed(Tuple<Medication, string> medGenNamePair, List<Medication> listMedsExisting)
        {
            Medication med = medGenNamePair.Item1;
            string genericName = medGenNamePair.Item2;
            bool isNoteChecked = true;
            //If everything is identical, except med.Notes is blank while x.Notes is not blank, we consider this to be a duplicate.
            if (string.IsNullOrEmpty(med.Notes))
            {
                isNoteChecked = false;
            }

            return listMedsExisting.Any(
                x => x.Description.Trim().ToLower() == med.Description.Trim().ToLower()
                && Medication.GetGenericName(x.GenericId.Value).Trim().ToLower() == genericName.Trim().ToLower()
                && x.RxCui == med.RxCui
                && (isNoteChecked ? (x.Notes.Trim().ToLower() == med.Notes.Trim().ToLower()) : true)
            );
        }

        /// <summary>
        /// Inserts the given medNew.
        /// Given medGennamePair is a medication that we are checking and the given generic name if set.
        /// ListMedsExisting is used to identify the GenericNum for medNew.
        /// </summary>
        private static void InsertNewMed(Tuple<Medication, string> medGenNamePair, List<Medication> listMedsExisting)
        {
            Medication medNew = medGenNamePair.Item1;
            string genericName = medGenNamePair.Item2;
            long genNum = listMedsExisting.FirstOrDefault(x => x.Description == genericName)?.Id ?? 0;
            if (genNum != 0)
            {//Found a match.
                medNew.GenericId = genNum;
            }
            Medication.Insert(medNew);//Assigns new primary key.
            if (genNum == 0)
            {//Found no match initially, assume given medication is the generic.
                medNew.GenericId = medNew.Id;
                Medication.Update(medNew);
            }
            listMedsExisting.Add(medNew);//Keep in memory list and database in sync.
        }

        /// <summary>
        ///     <para>
        ///         Exports the specified <paramref name="medications"/> to the passed in 
        ///         <paramref name="filename"/>.
        ///     </para>
        /// </summary>
        /// <param name="filename">The name of the file to write to.</param>
        /// <param name="medications">The list of medications to export.</param>
        /// <returns>The number of medications exported.</returns>
        public static int ExportMedications(string filename, List<Medication> medications)
        {
            var stringBuilder = new StringBuilder();

            foreach (var medication in medications)
            {
                stringBuilder.AppendLine(
                    medication.Description + '\t' + 
                    Medication.GetGenericName(medication.GenericId.Value) + '\t' + 
                    medication.Notes + '\t' + 
                    medication.RxCui);
            }

            File.WriteAllText(filename, stringBuilder.ToString());

            SecurityLog.Write(SecurityLogEvents.Setup, $"Exported {medications.Count} medications to '{filename}'.");

            return medications.Count;
        }

        /// <summary>
        /// Throws exception.  Reads tab delimited medication information from given filename.
        /// Returns the list of new medications with all generic medications before brand medications.
        /// File required to be formatted such that each row contain: MedName\tGenericName\tNotes\tRxCui
        /// </summary>
        /// <param name="filename">The name of the medications file to load.</param>
        /// <param name="isTempFile">
        ///     A value indicating whether the specified file is a temporary file. IF the file
        ///     is a temporary file, it will be deleted after it has been loaded in memory.
        /// </param>
        public static List<Tuple<Medication, string>> GetMedicationsFromFile(string filename, bool isTempFile = false)
        {
            List<Tuple<Medication, string>> newMedicationsList = new List<Tuple<Medication, string>>();
            if (string.IsNullOrEmpty(filename))
            {
                return newMedicationsList;
            }

            var lines = File.ReadAllLines(filename);
            if (isTempFile)
            {
                File.Delete(filename);
            }

            foreach (var line in lines)
            {
                var lineData = line.Split(',');
                if (lineData.Length != 4)
                {
                    throw new ODException("Invalid formatting detected in file.");
                }

                string genericName = lineData[1].Trim();

                newMedicationsList.Add(
                    new Tuple<Medication, string>(new Medication
                    {
                        Description = lineData[0].Trim(),
                        Notes = lineData[2].Trim(),
                        RxCui = lineData[3]
                    }, 
                    genericName));
            }

            return SortMedGenericsFirst(newMedicationsList);
        }

        /// <summary>
        /// Custom sorting so that generic medications are above branded medications.
        /// Given list elements are a ODTuple of a medication and the given generic name if set.
        /// </summary>
        private static List<Tuple<Medication, string>> SortMedGenericsFirst(List<Tuple<Medication, string>> listMedLines)
        {
            List<Tuple<Medication, string>> listMedGeneric = new List<Tuple<Medication, string>>();
            List<Tuple<Medication, string>> listMedBranded = new List<Tuple<Medication, string>>();
            foreach (Tuple<Medication, string> pair in listMedLines)
            {
                Medication med = pair.Item1;
                string genericName = pair.Item2;
                if (med.Description.ToLower().In(genericName.ToLower(), ""))
                {//Generic if names directly match, or assume generic if no genericName provided.
                    listMedGeneric.Add(pair);
                }
                else
                {//Branded
                    listMedBranded.Add(pair);
                }
            }
            listMedGeneric.AddRange(listMedBranded);
            return listMedGeneric;
        }
    }
}