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
            var program = Programs.GetFirstOrDefault(x => x.Id == programId);

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
            if (program.TypeName == ProgramName.ActeonImagingSuite.ToString())
            {
                ActeonImagingSuite.SendData(program, patient);
                return;
            }
            if (program.TypeName == ProgramName.Adstra.ToString())
            {
                AdstraBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Apixia.ToString())
            {
                ApixiaBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Apteryx.ToString())
            {
                ApteryxBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.AudaxCeph.ToString())
            {
                AudaxCephBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.BioPAK.ToString())
            {
                BioPAK.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.CADI.ToString())
            {
                CadiBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Camsight.ToString())
            {
                CamsightBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.CaptureLink.ToString())
            {
                CaptureLinkBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Carestream.ToString())
            {
                CarestreamBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Cerec.ToString())
            {
                CerecBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.CleaRay.ToString())
            {
                CleaRayBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.CliniView.ToString())
            {
                CliniviewBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.ClioSoft.ToString())
            {
                ClioSoftBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.DBSWin.ToString())
            {
                DbsWinBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.DemandForce.ToString())
            {
                DemandForce.SendData(program, patient);
                return;
            }
#if !DISABLE_WINDOWS_BRIDGES
            else if (program.TypeName == ProgramName.DentalEye.ToString())
            {
                DentalEyeBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.DentalStudio.ToString())
            {
                DentalStudioBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.DentX.ToString())
            {
                DentXBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.DrCeph.ToString())
            {
                DrCephBridge.SendData(program, patient);
                return;
            }
#endif
            else if (program.TypeName == ProgramName.DentalTekSmartOfficePhone.ToString())
            {
                DentalTekBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.DentForms.ToString())
            {
                DentFormsBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Dexis.ToString())
            {
                DexisBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Digora.ToString())
            {
                Digora.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Dimaxis.ToString())
            {
                PlanmecaBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Office.ToString())
            {
                OfficeBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Dolphin.ToString())
            {
                Dolphin.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.DXCPatientCreditScore.ToString())
            {
                DentalXChangeBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Dxis.ToString())
            {
                DxisBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.EvaSoft.ToString())
            {
                EvaSoftBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.EwooEZDent.ToString())
            {
                EwooBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.FloridaProbe.ToString())
            {
                FloridaProbeBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Guru.ToString())
            {
                Guru.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.HandyDentist.ToString())
            {
                HandyDentistBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.HouseCalls.ToString())
            {
                FormHouseCalls FormHC = new FormHouseCalls();
                FormHC.ProgramCur = program;
                FormHC.ShowDialog();
                return;
            }
            else if (program.TypeName == ProgramName.iCat.ToString())
            {
                ICatBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.HdxWill.ToString())
            {
                HdxWillBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.iDixel.ToString())
            {
                IDixelBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.ImageFX.ToString())
            {
                ImageFXBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.iRYS.ToString())
            {
                IrysBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Lightyear.ToString())
            {
                Lightyear.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.NewTomNNT.ToString())
            {
                NewTomNNTBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.MediaDent.ToString())
            {
                MediaDent.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Midway.ToString())
            {
                MidwayBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.MiPACS.ToString())
            {
                MiPACSBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.OrthoCAD.ToString())
            {
                OrthoCadBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Oryx.ToString())
            {
                OryxBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.OrthoInsight3d.ToString())
            {
                OrthoInsight3dBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Owandy.ToString())
            {
                Owandy.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.PandaPerio.ToString())
            {
                PandaPerio.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.PandaPeriodAdvanced.ToString())
            {
                PandaPeriodAdvanced.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Patterson.ToString())
            {
                Patterson.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.PerioPal.ToString())
            {
                PerioPalBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Progeny.ToString())
            {
                Progeny.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.PT.ToString())
            {
                PaperlessTechnology.SendData(program, patient, false);
                return;
            }
            else if (program.TypeName == ProgramName.PTupdate.ToString())
            {
                PaperlessTechnology.SendData(program, patient, true);
                return;
            }
            else if (program.TypeName == ProgramName.RayMage.ToString())
            {
                RayMage.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Romexis.ToString())
            {
                Romexis.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Scanora.ToString())
            {
                Scanora.SendData(program, patient);
                return;
            }
#if !DISABLE_WINDOWS_BRIDGES
            else if (program.TypeName == ProgramName.Schick.ToString())
            {
                Schick.SendData(program, patient);
                return;
            }
#endif
            else if (program.TypeName == ProgramName.Sirona.ToString())
            {
                SironaBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.SMARTDent.ToString())
            {
                SmartDentBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Sopro.ToString())
            {
                SoproBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.TigerView.ToString())
            {
                TigerView.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Triana.ToString())
            {
                TrianaBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Trophy.ToString())
            {
                TrophyBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.TrophyEnhanced.ToString())
            {
                TrophyEnhancedBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.Tscan.ToString())
            {
                Tscan.SendData(program, patient);
                return;
            }
#if !DISABLE_WINDOWS_BRIDGES
            else if (program.TypeName == ProgramName.Vipersoft.ToString())
            {
                Vipersoft.SendData(program, patient);
                return;
            }
#endif
            else if (program.TypeName == ProgramName.visOra.ToString())
            {
                VisoraBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.VistaDent.ToString())
            {
                VistaDentBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.VixWin.ToString())
            {
                VixWinBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.VixWinBase36.ToString())
            {
                VixWinBase36Bridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.VixWinBase41.ToString())
            {
                VixWinBase41Bridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.VixWinNumbered.ToString())
            {
                VixWinNumberedBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.VixWinOld.ToString())
            {
                VixWinOldBridge.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.XDR.ToString())
            {
                DexisBridge.SendData(program, patient);//XDR uses the Dexis protocol
                return;
            }
            else if (program.TypeName == ProgramName.XVWeb.ToString())
            {
                XVWeb.SendData(program, patient);
                return;
            }
            else if (program.TypeName == ProgramName.ZImage.ToString())
            {
                ZImageBridge.SendData(program, patient);
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
                    string.Format("{0} is not available.", program.Description), 
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
                else if (program.TypeName == ProgramName.Midway.ToString())
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
            if (program.TypeName == ProgramName.Oryx.ToString())
            {
                var menuItem = new MenuItem
                {
                    Index = 0,
                    Text = "User Settings"
                };

                menuItem.Click += OryxBridge.MenuItemUserSettingsClick;

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
