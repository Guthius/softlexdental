using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenDentBusiness
{
    public class Programs
    {
        /// <summary></summary>
        public static bool UsingOrion
        {
            //No need to check RemotingRole; no call to db.
            get
            {
                return Programs.IsEnabled(ProgramName.Orion);
            }
        }

        ///<summary>Returns the local override path if available or returns original program path.  Always returns a valid path.</summary>
        public static string GetProgramPath(Program program)
        {
            //No need to check RemotingRole; no call to db.
            //string overridePath = ProgramProperties.GetLocalPathOverrideForProgram(program.ProgramNum);
            //if (overridePath != "")
            //{
            //    return overridePath;
            //}
            return program.Path;
        }

        /// <summary>Returns true if input program is a static program. Static programs are ones we do not want the user to be able to modify in some way.</summary>
        public bool IsStatic(Program prog)
        {
            //Currently there is just one static program. As more are created they will need to be added to this check.
            if (prog.TypeName == ProgramName.RapidCall.ToString())
            {
                return true;
            }
            return false;
        }

        ///<summary>For each enabled bridge, if the bridge uses a file to transmit patient data to the other software, then we need to remove the files or clear the files when OD is exiting.
        ///Required for EHR 2014 module d.7 (as stated by proctor).</summary>
        public static void ScrubExportedPatientData()
        {
            //List all program links here. If there is nothing to do for that link, then create a comment stating so.
            string path = "";
            //Adstra: Has no file paths containing outgoing pateint data from Open Dental.
            //Apixia:
            ScrubFileForProperty(ProgramName.Apixia, "System path to Apixia Digital Imaging ini file", "", true);//C:\Program Files\Digirex\Switch.ini
                                                                                                                 //Apteryx: Has no file paths containing outgoing patient data from Open Dental.
                                                                                                                 //BioPAK: Has no file paths containing outgoing patient data from Open Dental.
                                                                                                                 //CADI has no file paths containing outgoing patient data from Open Dental.
                                                                                                                 //CallFire: Has no file paths containing outgoing patient data from Open Dental.
                                                                                                                 //Camsight: Has no file paths containing outgoing patient data from Open Dental.
                                                                                                                 //CaptureLink: Has no file paths containing outgoing patient data from Open Dental.
                                                                                                                 //Carestream:
            ScrubFileForProperty(ProgramName.Carestream, "Patient.ini path", "", true);//C:\Carestream\Patient.ini
                                                                                       //Cerec: Has no file paths containing outgoing patient data from Open Dental.
                                                                                       //CliniView: Has no file paths containing outgoing patient data from Open Dental.
                                                                                       //ClioSoft: Has no file paths containing outgoing patient data from Open Dental.
                                                                                       //DBSWin:
            ScrubFileForProperty(ProgramName.DBSWin, "Text file path", "", true);//C:\patdata.txt
                                                                                 //DentalEye: Has no file paths containing outgoing patient data from Open Dental.
                                                                                 //DentalStudio: Has no file paths containing outgoing patient data from Open Dental.
                                                                                 //DentForms: Has no file paths containing outgoing patient data from Open Dental.
                                                                                 //DentX: Has no file paths containing outgoing patient data from Open Dental.
                                                                                 //Dexis:
            ScrubFileForProperty(ProgramName.Dexis, "InfoFile path", "", true);//InfoFile.txt
                                                                               //Digora: Has no file paths containing outgoing patient data from Open Dental.
                                                                               //Divvy: Has no file paths containing outgoing patient data from Open Dental.
                                                                               //Dolphin:
            ScrubFileForProperty(ProgramName.Dolphin, "Filename", "", true);//C:\Dolphin\Import\Import.txt
                                                                            //DrCeph: Has no file paths containing outgoing patient data from Open Dental.
                                                                            //Dxis: Has no file paths containing outgoing patient data from Open Dental.
                                                                            //EasyNotesPro: Has no file paths containing outgoing patient data from Open Dental.
                                                                            //eClinicalWorks: HL7 files are created, but eCW is supposed to consume and delete them.
                                                                            //EvaSoft: Has no file paths containing outgoing patient data from Open Dental.
                                                                            //EwooEZDent:
            Program program = Programs.GetCur(ProgramName.EwooEZDent);
            if (program.Enabled)
            {
                path = Programs.GetProgramPath(program);
                if (File.Exists(path))
                {
                    string dir = Path.GetDirectoryName(path);
                    string linkage = Path.Combine(dir, "linkage.xml");
                    if (File.Exists(linkage))
                    {
                        try
                        {
                            File.Delete(linkage);
                        }
                        catch
                        {
                            //Another instance of OD might be closing at the same time, in which case the delete will fail. Could also be a permission issue or a concurrency issue. Ignore.
                        }
                    }
                }
            }
            //FloridaProbe: Has no file paths containing outgoing patient data from Open Dental.
            //Guru: Has no file paths containing outgoing patient data from Open Dental.
            //HandyDentist: Has no file paths containing outgoing patient data from Open Dental.
            //HouseCalls:
            ScrubFileForProperty(ProgramName.HouseCalls, "Export Path", "Appt.txt", true);//C:\HouseCalls\Appt.txt
                                                                                          //IAP: Has no file paths containing outgoing patient data from Open Dental.
                                                                                          //iCat:
            ScrubFileForProperty(ProgramName.iCat, "XML output file path", "", true);//C:\iCat\Out\pm.xml
                                                                                     //ImageFX: Has no file paths containing outgoing patient data from Open Dental.
                                                                                     //Lightyear: Has no file paths containing outgoing patient data from Open Dental.
                                                                                     //MediaDent:
            ScrubFileForProperty(ProgramName.MediaDent, "Text file path", "", true);//C:\MediadentInfo.txt
                                                                                    //MiPACS: Has no file paths containing outgoing patient data from Open Dental.
                                                                                    //Mountainside: Has no file paths containing outgoing patient data from Open Dental.
                                                                                    //NewCrop: Has no file paths containing outgoing patient data from Open Dental.
                                                                                    //Orion: Has no file paths containing outgoing patient data from Open Dental.
                                                                                    //OrthoPlex: Has no file paths containing outgoing patient data from Open Dental.
                                                                                    //Owandy: Has no file paths containing outgoing patient data from Open Dental.
                                                                                    //PayConnect: Has no file paths containing outgoing patient data from Open Dental.
                                                                                    //Patterson:
            ScrubFileForProperty(ProgramName.Patterson, "System path to Patterson Imaging ini", "", true);//C:\Program Files\PDI\Shared files\Imaging.ini
                                                                                                          //PerioPal: Has no file paths containing outgoing patient data from Open Dental.
                                                                                                          //Planmeca: Has no file paths containing outgoing patient data from Open Dental.
                                                                                                          //PracticeWebReports: Has no file paths containing outgoing patient data from Open Dental.
                                                                                                          //Progeny: Has no file paths containing outgoing patient data from Open Dental.
                                                                                                          //PT: Per our website "The files involved get deleted immediately after they are consumed."
                                                                                                          //PTupdate: Per our website "The files involved get deleted immediately after they are consumed."
                                                                                                          //RayMage: Has no file paths containing outgoing patient data from Open Dental.
                                                                                                          //Schick: Has no file paths containing outgoing patient data from Open Dental.
                                                                                                          //Sirona:
            program = Programs.GetCur(ProgramName.Sirona);
            if (program.Enabled)
            {
                path = Programs.GetProgramPath(program);
                //read file C:\sidexis\sifiledb.ini
                string iniFile = Path.GetDirectoryName(path) + "\\sifiledb.ini";
                if (File.Exists(iniFile))
                {
                    string sendBox = ReadValueFromIni("FromStation0", "File", iniFile);
                    if (File.Exists(sendBox))
                    {
                        File.WriteAllText(sendBox, "");//Clear the sendbox instead of deleting.
                    }
                }
            }
            //Sopro: Has no file paths containing outgoing patient data from Open Dental.
            //TigerView:
            ScrubFileForProperty(ProgramName.TigerView, "Tiger1.ini path", "", false);//C:\Program Files\PDI\Shared files\Imaging.ini.  TigerView complains if the file is not present.
                                                                                      //Trojan: Has no file paths containing outgoing patient data from Open Dental.
                                                                                      //Trophy: Has no file paths containing outgoing patient data from Open Dental.
                                                                                      //TrophyEnhanced: Has no file paths containing outgoing patient data from Open Dental.
                                                                                      //Tscan: Has no file paths containing outgoing patient data from Open Dental.
                                                                                      //UAppoint: Has no file paths containing outgoing patient data from Open Dental.
                                                                                      //Vipersoft: Has no file paths containing outgoing patient data from Open Dental.
                                                                                      //VixWin: Has no file paths containing outgoing patient data from Open Dental.
                                                                                      //VixWinBase41: Has no file paths containing outgoing patient data from Open Dental.
                                                                                      //VixWinOld: Has no file paths containing outgoing patient data from Open Dental.
                                                                                      //Xcharge: Has no file paths containing outgoing patient data from Open Dental.
                                                                                      //XVWeb: Has no file paths containing outgoing patient data from Open Dental.
            ScrubFileForProperty(ProgramName.XDR, "InfoFile path", "", true);//C:\XDRClient\Bin\infofile.txt
        }

        ///<summary>Needed for Sirona bridge data scrub in ScrubExportedPatientData().</summary>
        [DllImport("kernel32")]//this is the Windows function for reading from ini files.
        private static extern int GetPrivateProfileStringFromIni(string section, string key, string def
            , StringBuilder retVal, int size, string filePath);

        ///<summary>Needed for Sirona bridge data scrub in ScrubExportedPatientData().</summary>
        private static string ReadValueFromIni(string section, string key, string iniFile)
        {
            StringBuilder strBuild = new StringBuilder(255);
            int i = GetPrivateProfileStringFromIni(section, key, "", strBuild, 255, iniFile);
            return strBuild.ToString();
        }

        ///<summary>If isRemovable is false, then the file referenced in the program property will be cleared.
        ///If isRemovable is true, then the file referenced in the program property will be deleted.</summary>
        private static void ScrubFileForProperty(ProgramName programName, string strFileProperty, string strFilePropertySuffix, bool isRemovable)
        {
            Program program = Programs.GetCur(programName);
            if (!program.Enabled)
            {
                return;
            }
            string strFileToScrub = Path.Combine(ProgramProperties.GetPropVal(program.Id, strFileProperty), strFilePropertySuffix);
            if (!File.Exists(strFileToScrub))
            {
                return;
            }
            try
            {
                File.WriteAllText(strFileToScrub, "");//Always clear the file contents, in case deleting fails below.
            }
            catch
            {
                //Another instance of OD might be closing at the same time, in which case the delete will fail. Could also be a permission issue or a concurrency issue. Ignore.
            }

            if (!isRemovable)
            {
                return;
            }
            try
            {
                File.Delete(strFileToScrub);
            }
            catch
            {
                //Another instance of OD might be closing at the same time, in which case the delete will fail. Could also be a permission issue or a concurrency issue. Ignore.
            }
        }

        ///<summary>Returns true if more than 1 credit card processing program is enabled.</summary>
        public static bool HasMultipleCreditCardProgramsEnabled()
        {
            //No need to check RemotingRole; no call to db.
            return new List<bool> {
                Programs.IsEnabled(ProgramName.Xcharge),Programs.IsEnabled(ProgramName.PayConnect),Programs.IsEnabled(ProgramName.PaySimple)
            }.Count(x => x == true) >= 2;
        }
    }
}