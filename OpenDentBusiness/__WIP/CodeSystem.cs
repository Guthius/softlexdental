/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenDentBusiness
{
    /// <summary>
    /// Used for tracking code systems imported to OD. HL7OID used for sending messages.
    /// This must be a database table in order to keep track of VersionCur between sessions.
    /// </summary>
    public class CodeSystem : DataRecord
    {
        /// <summary>
        /// The name of the code system.
        /// </summary>
        public string Name;

        /// <summary>
        /// Only used for display, not actually interpreted. Updated by Code System importer.  Examples: 2013 or 1
        /// </summary>
        public string Version;

        /// <summary>
        /// Only used for display, not actually interpreted. Updated by Convert DB script.
        /// </summary>
        public string VersionAvail;

        /// <summary>
        /// Example: 2.16.840.1.113883.6.13
        /// </summary>
        public string HL7OID;

        /// <summary>
        /// Notes to display to user. Examples: "CDT codes distributed via program updates.", 
        /// "CPT codes require purchase and download from www.ama.com
        /// </summary>
        public string Note;

        /// <summary>
        /// Constructs a new instance of the <see cref="CodeSystem"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="CodeSystem"/> instance.</returns>
        private static CodeSystem FromReader(MySqlDataReader dataReader)
        {
            return new CodeSystem
            {
                Id = (long)dataReader["id"],
                Name = (string)dataReader["name"],
                Version = (string)dataReader["version"],
                VersionAvail = (string)dataReader["version_available"],
                HL7OID = (string)dataReader["hl7_oid"],
                Note = (string)dataReader["note"]
            };
        }


        public delegate void ProgressArgs(int numTotal, int numDone);

        /// <summary>
        /// Returns a list of code systems in the code system table.
        /// This query will change from version to version depending on what code systems we have available.
        /// </summary>
        public static List<CodeSystem> GetForCurrentVersion(bool IsMemberNation) =>
            SelectMany("SELECT * FROM code_systems", FromReader);
        
        /// <summary>
        /// Updates the specified code system in the database.
        /// </summary>
        /// <param name="codeSystem">The code system.</param>
        public static void Update(CodeSystem codeSystem)
        {
            // TODO: Implement me.
        }

        /// <summary>
        /// Updates <see cref="Version"/> to the <see cref="VersionAvail"/> of the 
        /// <see cref="CodeSystem"/> object passed in. Used by code system importer after 
        /// successful import.
        /// </summary>
        /// <param name="codeSystem">The code system.</param>
        public static void UpdateCurrentVersion(CodeSystem codeSystem)
        {
            codeSystem.Version = codeSystem.VersionAvail;

            Update(codeSystem);
        }

        /// <summary>
        /// Updates <see cref="Version"/> of the specified code system to the version passed in.
        /// </summary>
        public static void UpdateCurrentVersion(CodeSystem codeSystem, string version)
        {
            if (string.Compare(codeSystem.Version, version) > 0)
            { 
                return;
            }

            codeSystem.Version = version;

            Update(codeSystem);
        }


        /// <summary>
        /// Called after file is downloaded. 
        /// Throws exceptions.
        /// It is assumed that this is called from a worker thread. 
        /// Progress delegate will be called every 100th iteration to inform thread of current progress. 
        /// Quit flag can be set at any time in order to quit importing prematurely.
        /// </summary>
        public static void ImportCdcrec(string tempFileName, ProgressArgs progress, ref bool quit, ref int numCodesImported, ref int numCodesUpdated, bool updateExisting)
        {
            if (tempFileName == null)
            {
                return;
            }

            Dictionary<string, Cdcrec> dictCdcrecs = Cdcrecs.GetAll().ToDictionary(x => x.CdcrecCode, x => x);
            string[] lines = File.ReadAllLines(tempFileName);
            string[] arrayCDCREC;
            Cdcrec cdcrec = new Cdcrec();
            for (int i = 0; i < lines.Length; i++)
            {//each loop should read exactly one line of code. and each line of code should be a unique code
                if (quit)
                {
                    return;
                }
                if (i % 100 == 0)
                {
                    progress(i + 1, lines.Length);
                }
                arrayCDCREC = lines[i].Split('\t');
                if (dictCdcrecs.ContainsKey(arrayCDCREC[0]))
                {//code already exists
                    cdcrec = dictCdcrecs[arrayCDCREC[0]];
                    if (updateExisting &&
                        (cdcrec.HeirarchicalCode != arrayCDCREC[1]
                        || cdcrec.Description != arrayCDCREC[2]))
                    {
                        cdcrec.HeirarchicalCode = arrayCDCREC[1];
                        cdcrec.Description = arrayCDCREC[2];
                        Cdcrecs.Update(cdcrec);
                        numCodesUpdated++;
                    }
                    continue;
                }
                cdcrec.CdcrecCode = arrayCDCREC[0];
                cdcrec.HeirarchicalCode = arrayCDCREC[1];
                cdcrec.Description = arrayCDCREC[2];
                Cdcrecs.Insert(cdcrec);
                numCodesImported++;
            }
        }

        /////<summary>Called after file is downloaded.  Throws exceptions.</summary>
        //public static void ImportCDT(string tempFileName) ... not necessary.

        ///<summary>Called after user provides resource file.  Throws exceptions.  It is assumed that this is called from a worker thread.  Progress delegate will be called every 100th iteration to inform thread of current progress. Quit flag can be set at any time in order to quit importing prematurely.
        ///No UpdateExisting parameter because we force users to accept new descriptions.</summary>
        public static void ImportCpt(string tempFileName, ProgressArgs progress, ref bool quit, ref int numCodesImported, ref int numCodesUpdated,
            string versionID)
        {
            if (tempFileName == null)
            {
                return;
            }
            Dictionary<string, string> dictCodes = Cpts.GetAll().ToDictionary(x => x.CptCode, x => x.Description);
            Regex regx = new Regex(@"^([\d]{4}[\d\w])\s+(.+?)$");//Regex = "At the beginning of the string, find five numbers, followed by a white space (tab or space) followed by one or more characters (but as few as possible) to the end of the line."
            string[] lines = File.ReadAllLines(tempFileName);
            string[] arrayCpt;
            bool isHeader = true;
            Cpt cpt = new Cpt();
            for (int i = 0; i < lines.Length; i++)
            {//each loop should read exactly one line of code. and each line of code should be a unique code
                if (quit)
                {
                    return;
                }
                if (i % 100 == 0)
                {
                    progress(i + 1, lines.Length);
                }
                if (isHeader)
                {
                    if (!regx.IsMatch(lines[i]))
                    {                   //if(!lines[i].Contains("\t")) {	
                        continue;//Copyright info is present at the head of the file.
                    }
                    isHeader = false;
                }
                arrayCpt = new string[2];
                arrayCpt[0] = regx.Match(lines[i]).Groups[1].Value;//First five alphanumeric characters
                arrayCpt[1] = regx.Match(lines[i]).Groups[2].Value;//Everything after the 6th character
                if (dictCodes.Keys.Contains(arrayCpt[0]))
                {//code already exists
                    Cpts.UpdateDescription(arrayCpt[0], arrayCpt[1], versionID);
                    if (dictCodes[arrayCpt[0]] != arrayCpt[1])
                    {//The description is different
                        numCodesUpdated++;
                    }
                }
                else
                {
                    cpt.CptCode = arrayCpt[0];
                    cpt.Description = arrayCpt[1];
                    cpt.VersionIDs = versionID;
                    Cpts.Insert(cpt);
                    numCodesImported++;
                }
            }
        }

        ///<summary>Called after file is downloaded.  Throws exceptions.  It is assumed that this is called from a worker thread.  Progress delegate will be called every 100th iteration to inform thread of current progress. Quit flag can be set at any time in order to quit importing prematurely.</summary>
        public static void ImportCvx(string tempFileName, ProgressArgs progress, ref bool quit, ref int numCodesImported, ref int numCodesUpdated,
            bool updateExisting)
        {
            if (tempFileName == null)
            {
                return;
            }
            Dictionary<string, CVX> dictCodes = CVX.All().ToDictionary(x => x.Code, x => x);
            string[] lines = File.ReadAllLines(tempFileName);
            string[] arrayCvx;
            CVX cvx = new CVX();
            for (int i = 0; i < lines.Length; i++)
            {//each loop should read exactly one line of code. and each line of code should be a unique code
                if (quit)
                {
                    return;
                }
                if (i % 100 == 0)
                {
                    progress(i + 1, lines.Length);
                }
                arrayCvx = lines[i].Split('\t');
                if (dictCodes.ContainsKey(arrayCvx[0]))
                {//code already exists
                    cvx = dictCodes[arrayCvx[0]];
                    if (updateExisting && cvx.Description != arrayCvx[1])
                    {//We do want to update and description is different.
                        cvx.Description = arrayCvx[1];
                        CVX.Update(cvx);
                        numCodesUpdated++;
                    }
                    continue;
                }
                cvx.Code = arrayCvx[0];
                cvx.Description = arrayCvx[1];
                CVX.Insert(cvx);
                numCodesImported++;
            }
        }

        ///<summary>Called after file is downloaded.  Throws exceptions.  It is assumed that this is called from a worker thread.  Progress delegate will be called every 100th iteration to inform thread of current progress. Quit flag can be set at any time in order to quit importing prematurely.</summary>
        public static void ImportHcpcs(string tempFileName, ProgressArgs progress, ref bool quit, ref int numCodesImported, ref int numCodesUpdated,
            bool updateExisting)
        {
            if (tempFileName == null)
            {
                return;
            }
            Dictionary<string, Hcpcs> dictHcpcs = Hcpcses.GetAll().ToDictionary(x => x.HcpcsCode, x => x);
            string[] lines = File.ReadAllLines(tempFileName);
            string[] arrayHCPCS;
            Hcpcs hcpcs = new Hcpcs();
            for (int i = 0; i < lines.Length; i++)
            {//each loop should read exactly one line of code. and each line of code should be a unique code
                if (quit)
                {
                    return;
                }
                if (i % 100 == 0)
                {
                    progress(i + 1, lines.Length);
                }
                arrayHCPCS = lines[i].Split('\t');
                if (dictHcpcs.ContainsKey(arrayHCPCS[0]))
                {//code already exists
                    hcpcs = dictHcpcs[arrayHCPCS[0]];
                    if (updateExisting && hcpcs.DescriptionShort != arrayHCPCS[1])
                    {
                        hcpcs.DescriptionShort = arrayHCPCS[1];
                        Hcpcses.Update(hcpcs);
                        numCodesUpdated++;
                    }
                    continue;
                }
                hcpcs.HcpcsCode = arrayHCPCS[0];
                hcpcs.DescriptionShort = arrayHCPCS[1];
                Hcpcses.Insert(hcpcs);
                numCodesImported++;
            }
        }

        ///<summary>Called after file is downloaded.  Throws exceptions.  It is assumed that this is called from a worker thread.  Progress delegate will be called every 100th iteration to inform thread of current progress. Quit flag can be set at any time in order to quit importing prematurely.</summary>
        public static void ImportIcd10(string tempFileName, ProgressArgs progress, ref bool quit, ref int numCodesImported, ref int numCodesUpdated,
            bool updateExisting)
        {
            // TODO: Fix me.

            //if(tempFileName==null) {
            //	return;
            //}
            //Dictionary<string,ICD10> dictIcd10s=ICD10.All().ToDictionary(x => x.Code,x => x);
            //string[] lines=File.ReadAllLines(tempFileName);
            //string[] arrayICD10;
            //ICD10 icd10=new ICD10();
            //for(int i=0;i<lines.Length;i++) {//each loop should read exactly one line of code. and each line of code should be a unique code
            //	if(quit) {
            //		return;
            //	}
            //	if(i%100==0) {
            //		progress(i+1,lines.Length);
            //	}
            //	arrayICD10=lines[i].Split('\t');
            //	if(dictIcd10s.ContainsKey(arrayICD10[0])) {//code already exists
            //		icd10=dictIcd10s[arrayICD10[0]];
            //		if(updateExisting && 
            //			(icd10.Description!=arrayICD10[1] || icd10.Header!=arrayICD10[2])) //Code informatin is different
            //		{
            //			icd10.Description=arrayICD10[1];
            //			icd10.Header=arrayICD10[2];
            //			Icd10s.Update(icd10);
            //			numCodesUpdated++;
            //		}
            //		continue;
            //	}
            //	icd10.Code		=arrayICD10[0];
            //	icd10.Description	=arrayICD10[1];
            //	icd10.Header			=arrayICD10[2];
            //	Icd10s.Insert(icd10);
            //	numCodesImported++;
            //}
        }

        ///<summary>Called after file is downloaded.  Throws exceptions.  It is assumed that this is called from a worker thread.  Progress delegate will be called every 100th iteration to inform thread of current progress. Quit flag can be set at any time in order to quit importing prematurely.</summary>
        public static void ImportIcd9(string tempFileName, ProgressArgs progress, ref bool quit, ref int numCodesImported, ref int numCodesUpdated,
            bool updateExisting)
        {
            if (tempFileName == null)
            {
                return;
            }
            //Customers may have an old codeset that has a truncated uppercase description, if so we want to update with new descriptions.

            Dictionary<string, ICD9> dictCodes = ICD9.All().ToDictionary(x => x.Code, x => x);
            string[] lines = File.ReadAllLines(tempFileName);
            string[] arrayICD9;
            ICD9 icd9 = new ICD9();
            for (int i = 0; i < lines.Length; i++)
            {//each loop should read exactly one line of code. and each line of code should be a unique code
                if (quit)
                {
                    return;
                }
                if (i % 100 == 0)
                {
                    progress(i + 1, lines.Length);
                }
                arrayICD9 = lines[i].Split('\t');
                if (dictCodes.ContainsKey(arrayICD9[0]))
                {//code already exists
                    icd9 = dictCodes[arrayICD9[0]];
                    if (updateExisting && icd9.Description != arrayICD9[1])
                    {//The new description does not match the description in the database.
                        icd9.Description = arrayICD9[1];
                        ICD9.Update(icd9);
                        numCodesUpdated++;
                    }
                    continue;
                }
                icd9.Code = arrayICD9[0];
                icd9.Description = arrayICD9[1];
                ICD9.Insert(icd9);
                numCodesImported++;
            }
        }

        ///<summary>Called after file is downloaded.  Throws exceptions.  It is assumed that this is called from a worker thread.  Progress delegate will be called every 100th iteration to inform thread of current progress. Quit flag can be set at any time in order to quit importing prematurely.</summary>
        public static void ImportLoinc(string tempFileName, ProgressArgs progress, ref bool quit, ref int numCodesImported, ref int numCodesUpdated,
            bool updateExisting)
        {
            if (tempFileName == null)
            {
                return;
            }
            Dictionary<string, Loinc> dictLoincs = Loincs.GetAll().ToDictionary(x => x.LoincCode, x => x);
            string[] lines = File.ReadAllLines(tempFileName);
            string[] arrayLoinc;
            Loinc oldLoinc = new Loinc();
            Loinc newLoinc = new Loinc();
            for (int i = 0; i < lines.Length; i++)
            {//each loop should read exactly one line of code. and each line of code should be a unique code
                if (quit)
                {
                    return;
                }
                if (i % 100 == 0)
                {
                    progress(i + 1, lines.Length);
                }
                arrayLoinc = lines[i].Split('\t');
                newLoinc.LoincCode = arrayLoinc[0];
                newLoinc.Component = arrayLoinc[1];
                newLoinc.PropertyObserved = arrayLoinc[2];
                newLoinc.TimeAspct = arrayLoinc[3];
                newLoinc.SystemMeasured = arrayLoinc[4];
                newLoinc.ScaleType = arrayLoinc[5];
                newLoinc.MethodType = arrayLoinc[6];
                newLoinc.StatusOfCode = arrayLoinc[7];
                newLoinc.NameShort = arrayLoinc[8];
                newLoinc.ClassType = arrayLoinc[9];
                newLoinc.UnitsRequired = arrayLoinc[10] == "Y";
                newLoinc.OrderObs = arrayLoinc[11];
                newLoinc.HL7FieldSubfieldID = arrayLoinc[12];
                newLoinc.ExternalCopyrightNotice = arrayLoinc[13];
                newLoinc.NameLongCommon = arrayLoinc[14];
                newLoinc.UnitsUCUM = arrayLoinc[15];
                newLoinc.RankCommonTests = PIn.Int(arrayLoinc[16]);
                newLoinc.RankCommonOrders = PIn.Int(arrayLoinc[17]);
                if (dictLoincs.ContainsKey(arrayLoinc[0]))
                {//code already exists; arrayLoinc[0]==Loinc Code
                    oldLoinc = dictLoincs[arrayLoinc[0]];
                    if (updateExisting &&
                        (oldLoinc.LoincCode != arrayLoinc[0]
                         || oldLoinc.Component != arrayLoinc[1]
                         || oldLoinc.PropertyObserved != arrayLoinc[2]
                         || oldLoinc.TimeAspct != arrayLoinc[3]
                         || oldLoinc.SystemMeasured != arrayLoinc[4]
                         || oldLoinc.ScaleType != arrayLoinc[5]
                         || oldLoinc.MethodType != arrayLoinc[6]
                         || oldLoinc.StatusOfCode != arrayLoinc[7]
                         || oldLoinc.NameShort != arrayLoinc[8]
                         || oldLoinc.ClassType != arrayLoinc[9]
                         || oldLoinc.UnitsRequired != (arrayLoinc[10] == "Y")
                         || oldLoinc.OrderObs != arrayLoinc[11]
                         || oldLoinc.HL7FieldSubfieldID != arrayLoinc[12]
                         || oldLoinc.ExternalCopyrightNotice != arrayLoinc[13]
                         || oldLoinc.NameLongCommon != arrayLoinc[14]
                         || oldLoinc.UnitsUCUM != arrayLoinc[15]
                         || oldLoinc.RankCommonTests != PIn.Int(arrayLoinc[16])
                         || oldLoinc.RankCommonOrders != PIn.Int(arrayLoinc[17])))
                    {
                        Loincs.Update(newLoinc);
                        numCodesUpdated++;
                    }
                    continue;
                }
                Loincs.Insert(newLoinc);
                numCodesImported++;
            }
        }

        ///<summary>Called after file is downloaded.  Throws exceptions.  It is assumed that this is called from a worker thread.  Progress delegate will be called every 100th iteration to inform thread of current progress. Quit flag can be set at any time in order to quit importing prematurely.</summary>
        public static void ImportRxNorm(string tempFileName, ProgressArgs progress, ref bool quit, ref int numCodesImported, ref int numCodesUpdated,
            bool updateExisting)
        {
            if (tempFileName == null)
            {
                return;
            }
            //RxNorms can have two codes for each RxCui. One RxNorm will have a value in the MmslCode and a blank description and the other will have a
            //value in the Description and a blank MmslCode. 
            List<RxNorm> listRxNorms = RxNorms.GetAll();
            Dictionary<string, RxNorm> dictRxNormsMmslCodes = listRxNorms.Where(x => x.MmslCode != "").ToDictionary(x => x.RxCui, x => x);
            Dictionary<string, RxNorm> dictRxNormsDefinitions = listRxNorms.Where(x => x.Description != "").ToDictionary(x => x.RxCui, x => x);
            string[] lines = File.ReadAllLines(tempFileName);
            string[] arrayRxNorm;
            RxNorm rxNorm = new RxNorm();
            for (int i = 0; i < lines.Length; i++)
            {//Each loop should read exactly one line of code. Each line will NOT be a unique code.
                if (quit)
                {
                    return;
                }
                if (i % 100 == 0)
                {
                    progress(i + 1, lines.Length);
                }
                arrayRxNorm = lines[i].Split('\t');
                if (dictRxNormsMmslCodes.ContainsKey(arrayRxNorm[0]))
                {//code with an MmslCode already exists
                    rxNorm = dictRxNormsMmslCodes[arrayRxNorm[0]];
                    if (updateExisting)
                    {
                        if (arrayRxNorm[1] != "" && arrayRxNorm[1] != rxNorm.MmslCode)
                        {
                            rxNorm.MmslCode = arrayRxNorm[1];
                            rxNorm.Description = "";//Should be blank for all MMSL code entries. See below for non-MMSL entries with descriptions.
                            RxNorms.Update(rxNorm);
                            numCodesUpdated++;
                        }
                    }
                    continue;
                }
                if (dictRxNormsDefinitions.ContainsKey(arrayRxNorm[0]))
                {//code with a Description already exists
                    rxNorm = dictRxNormsDefinitions[arrayRxNorm[0]];
                    if (updateExisting)
                    {
                        string newDescript = arrayRxNorm[2];
                        //if(newDescript.Length>255) {
                        //	newDescript=newDescript.Substring(0,255);//Description column is only varchar(255) so some descriptions will get truncated.
                        //}
                        //if(arrayRxNorm[2]!="" && newDescript!=rxNorm.Description) {
                        if (arrayRxNorm[2] != "" && arrayRxNorm[2] != rxNorm.Description)
                        {
                            rxNorm.MmslCode = "";//should be blank for all entries that have a description.
                            rxNorm.Description = arrayRxNorm[2];
                            RxNorms.Update(rxNorm);
                            numCodesUpdated++;
                        }
                    }
                    continue;
                }
                rxNorm.RxCui = arrayRxNorm[0];
                rxNorm.MmslCode = arrayRxNorm[1];
                rxNorm.Description = arrayRxNorm[2];
                RxNorms.Insert(rxNorm);
                numCodesImported++;
            }
        }

        ///<summary>Called after file is downloaded.  Throws exceptions.  It is assumed that this is called from a worker thread.  Progress delegate will be called every 100th iteration to inform thread of current progress. Quit flag can be set at any time in order to quit importing prematurely.</summary>
        public static void ImportSnomed(string tempFileName, ProgressArgs progress, ref bool quit, ref int numCodesImported, ref int numCodesUpdated,
            bool updateExisting)
        {
            if (tempFileName == null)
            {
                return;
            }
            Dictionary<string, Snomed> dictSnomeds = Snomeds.GetAll().ToDictionary(x => x.SnomedCode, x => x);
            string[] lines = File.ReadAllLines(tempFileName);
            string[] arraySnomed;
            Snomed snomed = new Snomed();
            for (int i = 0; i < lines.Length; i++)
            {//each loop should read exactly one line of code. and each line of code should be a unique code
                if (quit)
                {
                    return;
                }
                if (i % 100 == 0)
                {
                    progress(i + 1, lines.Length);
                }
                arraySnomed = lines[i].Split('\t');
                if (dictSnomeds.ContainsKey(arraySnomed[0]))
                {//code already exists
                    snomed = dictSnomeds[arraySnomed[0]];
                    if (updateExisting && snomed.Description != arraySnomed[1])
                    {
                        snomed.Description = arraySnomed[1];
                        Snomeds.Update(snomed);
                        numCodesUpdated++;
                    }
                    continue;
                }
                snomed.SnomedCode = arraySnomed[0];
                snomed.Description = arraySnomed[1];
                Snomeds.Insert(snomed);
                numCodesImported++;
            }
        }

        ///<summary>Called after file is downloaded.  Throws exceptions.  It is assumed that this is called from a worker thread.  Progress delegate will be called every 100th iteration to inform thread of current progress. Quit flag can be set at any time in order to quit importing prematurely.</summary>
        public static void ImportSop(string tempFileName, ProgressArgs progress, ref bool quit, ref int numCodesImported, ref int numcodesUpdated,
            bool updateExisting)
        {
            if (tempFileName == null)
            {
                return;
            }
            Dictionary<string, Sop> dictSops = Sops.GetDeepCopy().ToDictionary(x => x.SopCode, x => x);
            string[] lines = File.ReadAllLines(tempFileName);
            string[] arraySop;
            Sop sop = new Sop();
            for (int i = 0; i < lines.Length; i++)
            {//each loop should read exactly one line of code. and each line of code should be a unique code
                if (quit)
                {
                    return;
                }
                if (i % 100 == 0)
                {
                    progress(i + 1, lines.Length);
                }
                arraySop = lines[i].Split('\t');
                if (dictSops.ContainsKey(arraySop[0]))
                {//code already exists
                    sop = dictSops[arraySop[0]];
                    if (updateExisting && sop.Description != arraySop[1])
                    {
                        sop.Description = arraySop[1];
                        Sops.Update(sop);
                        numcodesUpdated++;
                    }
                    continue;
                }
                sop.SopCode = arraySop[0];
                sop.Description = arraySop[1];
                Sops.Insert(sop);
                numCodesImported++;
            }
        }

        ///<summary>Called after file is downloaded.  Throws exceptions.  It is assumed that this is called from a worker thread.  Progress delegate will be called every 100th iteration to inform thread of current progress. Quit flag can be set at any time in order to quit importing prematurely.</summary>
        public static void ImportUcum(string tempFileName, ProgressArgs progress, ref bool quit, ref int numCodesImported, ref int numCodesUpdated,
            bool updateExisting)
        {
            if (tempFileName == null)
            {
                return;
            }
            Dictionary<string, Ucum> dictUcums = Ucum.All().ToDictionary(x => x.Code, x => x);
            string[] lines = File.ReadAllLines(tempFileName);
            string[] arrayUcum;
            Ucum ucum = new Ucum();
            for (int i = 0; i < lines.Length; i++)
            {//each loop should read exactly one line of code. and each line of code should be a unique code
                if (quit)
                {
                    return;
                }
                if (i % 100 == 0)
                {
                    progress(i + 1, lines.Length);
                }
                arrayUcum = lines[i].Split('\t');
                if (dictUcums.ContainsKey(arrayUcum[0]))
                {//code already exists
                    ucum = dictUcums[arrayUcum[0]];
                    if (updateExisting && ucum.Description != arrayUcum[1])
                    {
                        ucum.Description = arrayUcum[1];
                        Ucum.Update(ucum);
                        numCodesUpdated++;
                    }
                    continue;
                }
                ucum.Code = arrayUcum[0];
                ucum.Description = arrayUcum[1];
                Ucum.Insert(ucum);
                numCodesImported++;
            }
        }
    }
}
