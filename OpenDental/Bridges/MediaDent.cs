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
using OpenDentBusiness;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class MediaDent
    {
        public MediaDent()
        {
        }

        ///<summary>Launches the program by passing the name of a file with data in it.  </summary>
        public static void SendData(Program ProgramCur, Patient pat)
        {
            string path = Programs.GetProgramPath(ProgramCur);
            //ArrayList ForProgram=ProgramProperties.GetForProgram(ProgramCur.ProgramNum); ;
            string version4or5 = ProgramProperties.GetPropVal(ProgramCur.Id, "MediaDent Version 4 or 5");
            if (version4or5 == "4")
            {
                SendData4(ProgramCur, pat);
                return;
            }
            if (pat == null)
            {
                try
                {
                    Process.Start(path);//should start Mediadent without bringing up a pt.
                }
                catch
                {
                    MessageBox.Show(path + " is not available.");
                }
            }
            string infoFile = ProgramProperties.GetPropVal(ProgramCur.Id, "Text file path");
            try
            {
                using (StreamWriter sw = new StreamWriter(infoFile, false))
                {
                    string id = "";
                    if (ProgramProperties.GetPropVal(ProgramCur.Id, "Enter 0 to use PatientNum, or 1 to use ChartNum") == "0")
                    {
                        id = pat.PatNum.ToString();
                    }
                    else
                    {
                        id = pat.ChartNumber;
                    }
                    sw.WriteLine(pat.LName + ", " + pat.FName
                        + " " + pat.Birthdate.ToShortDateString()
                        + " " + id);
                    sw.WriteLine();
                    sw.WriteLine("PN=" + id);
                    sw.WriteLine("LN=" + pat.LName);
                    sw.WriteLine("FN=" + pat.FName);
                    sw.WriteLine("BD=" + pat.Birthdate.ToString("MM/dd/yyyy"));
                    if (pat.Gender == PatientGender.Female)
                    {
                        sw.WriteLine("SX=F");
                    }
                    else
                    {
                        sw.WriteLine("SX=M");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Unable to write to text file: " + infoFile);
                return;
            }
            try
            {
                Process.Start(path, "@" + infoFile);
            }
            catch
            {
                MessageBox.Show(path + " is not available.");
            }
        }

        ///<summary>Launches the program using command line.</summary>
        private static void SendData4(Program ProgramCur, Patient pat)
        {
            string path = Programs.GetProgramPath(ProgramCur);
            //Usage: mediadent.exe /P<Patient Name> /D<Practitioner> /L<Language> /F<Image folder> /B<Birthdate>
            //Example: mediadent.exe /PJan Met De Pet /DOtté Gunter /L1 /Fc:\Mediadent\patients\1011 /B27071973
            List<ProgramPreference> ForProgram = ProgramProperties.GetForProgram(ProgramCur.Id); ;
            if (pat == null)
            {
                return;
            }
            string info = "/P" + Cleanup(pat.FName + " " + pat.LName);
            Provider prov = Providers.GetProv(Patients.GetProvNum(pat));
            info += " /D" + prov.FName + " " + prov.LName
                + " /L1 /F";
            ProgramPreference PPCur = ProgramProperties.GetCur(ForProgram, ProgramProperties.PropertyDescs.ImageFolder);
            info += PPCur.Value;
            PPCur = ProgramProperties.GetCur(ForProgram, ProgramProperties.PropertyDescs.PatOrChartNum); ;
            if (PPCur.Value == "0")
            {
                info += pat.PatNum.ToString();
            }
            else
            {
                info += Cleanup(pat.ChartNumber);
            }
            info += " /B" + pat.Birthdate.ToString("ddMMyyyy");
            //MessageBox.Show(info);
            //not used yet: /inputfile "path to file"
            try
            {
                Process.Start(path, info);
            }
            catch
            {
                MessageBox.Show(path + " " + info + " is not available.");
            }
        }

        private static string Cleanup(string input) => input.Replace("\"", "").Replace("'", "").Replace("/", "");
    }
}
