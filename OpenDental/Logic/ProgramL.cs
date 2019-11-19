using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using OpenDental.Bridges;
using OpenDentBusiness;
using OpenDental.UI;
using System.Drawing;
using System.IO;
using OpenDental.Properties;

namespace OpenDental
{
    public class ProgramL
    {
        /// <summary>
        /// Typically used when user clicks a button to a Program link.
        /// This method attempts to identify and execute the program based on the given programNum.
        /// </summary>
        public static void Execute(long programId, Patient patient)
        {
            var program = Programs.GetFirstOrDefault(x => x.ProgramNum == programId);

            if (program == null)
            {
                MessageBox.Show(
                    "Error, program entry not found in database.", 
                    "Program", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            if (patient != null && Preference.GetBool(PreferenceName.ShowFeaturePatientClone))
            {
                patient = Patients.GetOriginalPatientForClone(patient);
            }

            if (program.PluginDllName != "")
            {
                if (patient == null)
                {
                    Plugins.LaunchToolbarButton(programId, 0);
                }
                else
                {
                    Plugins.LaunchToolbarButton(programId, patient.PatNum);
                }
                return;
            }
            if (program.ProgName == ProgramName.ActeonImagingSuite.ToString())
            {
                ActeonImagingSuite.SendData(program, patient);
                return;
            }
            if (program.ProgName == ProgramName.Adstra.ToString())
            {
                Adstra.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Apixia.ToString())
            {
                Apixia.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Apteryx.ToString())
            {
                Apteryx.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.AudaxCeph.ToString())
            {
                AudaxCeph.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.BioPAK.ToString())
            {
                BioPAK.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.CADI.ToString())
            {
                CADI.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Camsight.ToString())
            {
                Camsight.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.CaptureLink.ToString())
            {
                CaptureLink.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Carestream.ToString())
            {
                Carestream.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Cerec.ToString())
            {
                Cerec.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.CleaRay.ToString())
            {
                CleaRay.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.CliniView.ToString())
            {
                Cliniview.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.ClioSoft.ToString())
            {
                ClioSoft.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.DBSWin.ToString())
            {
                DBSWin.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.DemandForce.ToString())
            {
                DemandForce.SendData(program, patient);
                return;
            }
#if !DISABLE_WINDOWS_BRIDGES
            else if (program.ProgName == ProgramName.DentalEye.ToString())
            {
                DentalEye.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.DentalStudio.ToString())
            {
                DentalStudio.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.DentX.ToString())
            {
                DentX.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.DrCeph.ToString())
            {
                DrCeph.SendData(program, patient);
                return;
            }
#endif
            else if (program.ProgName == ProgramName.DentalTekSmartOfficePhone.ToString())
            {
                DentalTek.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.DentForms.ToString())
            {
                DentForms.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Dexis.ToString())
            {
                Dexis.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Digora.ToString())
            {
                Digora.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Dimaxis.ToString())
            {
                Planmeca.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Office.ToString())
            {
                Office.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Dolphin.ToString())
            {
                Dolphin.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.DXCPatientCreditScore.ToString())
            {
                DentalXChange.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Dxis.ToString())
            {
                Dxis.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.EvaSoft.ToString())
            {
                EvaSoft.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.EwooEZDent.ToString())
            {
                Ewoo.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.FloridaProbe.ToString())
            {
                FloridaProbe.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Guru.ToString())
            {
                Guru.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.HandyDentist.ToString())
            {
                HandyDentist.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.HouseCalls.ToString())
            {
                FormHouseCalls FormHC = new FormHouseCalls();
                FormHC.ProgramCur = program;
                FormHC.ShowDialog();
                return;
            }
            else if (program.ProgName == ProgramName.iCat.ToString())
            {
                ICat.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.HdxWill.ToString())
            {
                HdxWill.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.iDixel.ToString())
            {
                iDixel.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.ImageFX.ToString())
            {
                ImageFX.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.iRYS.ToString())
            {
                Irys.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Lightyear.ToString())
            {
                Lightyear.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.NewTomNNT.ToString())
            {
                NewTomNNT.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.MediaDent.ToString())
            {
                MediaDent.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Midway.ToString())
            {
                Midway.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.MiPACS.ToString())
            {
                MiPACS.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.OrthoCAD.ToString())
            {
                OrthoCad.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Oryx.ToString())
            {
                Oryx.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.OrthoInsight3d.ToString())
            {
                OrthoInsight3d.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Owandy.ToString())
            {
                Owandy.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.PandaPerio.ToString())
            {
                PandaPerio.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.PandaPeriodAdvanced.ToString())
            {
                PandaPeriodAdvanced.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Patterson.ToString())
            {
                Patterson.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.PerioPal.ToString())
            {
                PerioPal.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Progeny.ToString())
            {
                Progeny.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.PT.ToString())
            {
                PaperlessTechnology.SendData(program, patient, false);
                return;
            }
            else if (program.ProgName == ProgramName.PTupdate.ToString())
            {
                PaperlessTechnology.SendData(program, patient, true);
                return;
            }
            else if (program.ProgName == ProgramName.RayMage.ToString())
            {
                RayMage.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Romexis.ToString())
            {
                Romexis.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Scanora.ToString())
            {
                Scanora.SendData(program, patient);
                return;
            }
#if !DISABLE_WINDOWS_BRIDGES
            else if (program.ProgName == ProgramName.Schick.ToString())
            {
                Schick.SendData(program, patient);
                return;
            }
#endif
            else if (program.ProgName == ProgramName.Sirona.ToString())
            {
                Sirona.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.SMARTDent.ToString())
            {
                SmartDent.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Sopro.ToString())
            {
                Sopro.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.TigerView.ToString())
            {
                TigerView.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Triana.ToString())
            {
                Triana.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Trophy.ToString())
            {
                Trophy.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.TrophyEnhanced.ToString())
            {
                TrophyEnhanced.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.Tscan.ToString())
            {
                Tscan.SendData(program, patient);
                return;
            }
#if !DISABLE_WINDOWS_BRIDGES
            else if (program.ProgName == ProgramName.Vipersoft.ToString())
            {
                Vipersoft.SendData(program, patient);
                return;
            }
#endif
            else if (program.ProgName == ProgramName.visOra.ToString())
            {
                Visora.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.VistaDent.ToString())
            {
                VistaDent.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.VixWin.ToString())
            {
                VixWin.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.VixWinBase36.ToString())
            {
                VixWinBase36.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.VixWinBase41.ToString())
            {
                VixWinBase41.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.VixWinNumbered.ToString())
            {
                VixWinNumbered.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.VixWinOld.ToString())
            {
                VixWinOld.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.XDR.ToString())
            {
                Dexis.SendData(program, patient);//XDR uses the Dexis protocol
                return;
            }
            else if (program.ProgName == ProgramName.XVWeb.ToString())
            {
                XVWeb.SendData(program, patient);
                return;
            }
            else if (program.ProgName == ProgramName.ZImage.ToString())
            {
                ZImage.SendData(program, patient);
                return;
            }

            //all remaining programs:
            try
            {
                string cmdline = program.CommandLine;
                string path = Programs.GetProgramPath(program);
                string outputFilePath = program.FilePath;
                string fileTemplate = program.FileTemplate;

                if (patient != null)
                {
                    cmdline = ReplaceHelper(cmdline, patient);
                    path = ReplaceHelper(path, patient);
                    if (!string.IsNullOrEmpty(outputFilePath) && !string.IsNullOrEmpty(fileTemplate))
                    {
                        fileTemplate = ReplaceHelper(fileTemplate, patient);
                        fileTemplate = fileTemplate.Replace("\n", "\r\n");

                        File.WriteAllText(outputFilePath, fileTemplate);
                    }
                }

                Process.Start(path, cmdline);
            }
            catch
            {
                MessageBox.Show(
                    string.Format("{0} is not available.", program.ProgDesc), 
                    "Program", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }
        }


        ///<summary>Helper method that replaces the message with all of the Message Replacements available for ProgramLinks.</summary>
        private static string ReplaceHelper(string message, Patient pat)
        {
            string retVal = message;
            retVal = Patients.ReplacePatient(retVal, pat);
            retVal = Patients.ReplaceGuarantor(retVal, pat);
            retVal = Referrals.ReplaceRefProvider(retVal, pat);
            return retVal;
        }

        public static void LoadToolbar(ODToolBar ToolBarMain, ToolBarsAvail toolBarsAvail)
        {
            List<ToolButItem> toolButItems = ToolButItems.GetForToolBar(toolBarsAvail);
            foreach (var toolButItem in toolButItems)
            {
                var program = Programs.GetProgram(toolButItem.ProgramNum);
                //if(ProgramProperties.IsAdvertisingDisabled(programCur)) {
                //	continue;
                //}

                Image image = null;
                if (program.ButtonImage != "")
                {
                    image = PIn.Bitmap(program.ButtonImage);
                }
                else if (program.ProgName == ProgramName.Midway.ToString())
                {
                    image = Resources.Midway_Icon_22x22;
                }

                if (toolBarsAvail != ToolBarsAvail.MainToolbar)
                {
                    ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
                }

                var button = new ODToolBarButton(toolButItem.ButtonText, image, "", program);

                AddDropDown(button, program);

                ToolBarMain.Buttons.Add(button);
            }
        }

        /// <summary>
        ///     <para>
        ///         Adds a drop down menu if this program requires it.
        ///     </para>
        /// </summary>
        private static void AddDropDown(ODToolBarButton button, Program program)
        {
            if (program.ProgName == ProgramName.Oryx.ToString())
            {
                var menuItem = new MenuItem
                {
                    Index = 0,
                    Text = "User Settings"
                };

                menuItem.Click += Oryx.menuItemUserSettingsClick;

                var contextMenu = new ContextMenu();

                contextMenu.MenuItems.AddRange(new MenuItem[] {
                        menuItem,
                });

                button.Style = ODToolBarButtonStyle.DropDownButton;
                button.DropDownMenu = contextMenu;
            }
        }
    }
}
