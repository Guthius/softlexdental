using System;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI.Voice;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormPerio {
		///<summary>Performs the action for the recognized command.</summary>
		private void VoiceController_SpeechRecognized(object sender,ODSpeechRecognizedEventArgs e) {
			if(listExams.SelectedIndex < 0 //No exam selected
				&& !e.Command.ActionToPerform.In(VoiceCommandAction.StartListening,
					VoiceCommandAction.StopListening,
					VoiceCommandAction.CreatePerioExam,
					VoiceCommandAction.CopyPrevious,
					VoiceCommandAction.DidntGetThat)) 
			{ 
				_voiceController?.StopListening();
				VoiceMsgBox.Show("Please select an exam first.");
				_voiceController?.StartListening();
				return;
			}
			DateTime dateSecurity;
			if(listExams.SelectedIndex > -1) {
				dateSecurity=PerioExams.ListExams[listExams.SelectedIndex].ExamDate;
			}
			else {
				dateSecurity=MiscData.GetNowDateTime();
			}
			if(!Security.IsAuthorized(Permissions.PerioEdit,dateSecurity,true) 
				&& !e.Command.ActionToPerform.In(VoiceCommandAction.StartListening,
					VoiceCommandAction.StopListening,
					VoiceCommandAction.DidntGetThat)) 
			{
				_voiceController?.StopListening();
				VoiceMsgBox.Show("Not authorized to edit the perio chart.");
				return;
			}
			gridP.Focus();
			_curLocation=gridP.GetCurrentCell();
			switch(e.Command.ActionToPerform) {
				#region Initialization
				case VoiceCommandAction.StartListening:
					labelListening.Visible=true;
					break;
				case VoiceCommandAction.StopListening:
					labelListening.Visible=false;
					break;
				case VoiceCommandAction.CreatePerioExam:
					butAdd.PerformClick();
					break;
				case VoiceCommandAction.CopyPrevious:
					if(listExams.SelectedIndex==-1) {
						VoiceMsgBox.Show("There are currently no exams to copy from.");
						return;
					}
					butCopyPrevious.PerformClick();
					break;
				#endregion Initialization
				#region Probing Depths
				case VoiceCommandAction.Zero:
					but0.PerformClick();
					break;
				case VoiceCommandAction.One:
					but1.PerformClick();
					break;
				case VoiceCommandAction.Two:
					but2.PerformClick();
					break;
				case VoiceCommandAction.Three:
					but3.PerformClick();
					break;
				case VoiceCommandAction.Four:
					but4.PerformClick();
					break;
				case VoiceCommandAction.Five:
					but5.PerformClick();
					break;
				case VoiceCommandAction.Six:
					but6.PerformClick();
					break;
				case VoiceCommandAction.Seven:
					but7.PerformClick();
					break;
				case VoiceCommandAction.Eight:
					but8.PerformClick();
					break;
				case VoiceCommandAction.Nine:
					but9.PerformClick();
					break;
				case VoiceCommandAction.Ten:
					but10.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.Eleven:
					but10.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.Twelve:
					but10.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.Thirteen:
					but10.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.Fourteen:
					but10.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.Fifteen:
					but10.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.Sixteen:
					but10.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.Seventeen:
					but10.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.Eighteen:
					but10.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.Nineteen:
					but10.PerformClick();
					but9.PerformClick();
					break;
				#region Hard-Coded Triplets
				case VoiceCommandAction.ThreeTwoThree:
					but3.PerformClick();
					but2.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FourThreeFour:
					but4.PerformClick();
					but3.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ThreeThreeThree:
					but3.PerformClick();
					but3.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.TwoTwoTwo:
					but2.PerformClick();
					but2.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FourFourFour:
					but4.PerformClick();
					but4.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.TwoOneTwo:
					but2.PerformClick();
					but1.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FourThreeThree:
					but4.PerformClick();
					but3.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ThreeThreeFour:
					but3.PerformClick();
					but3.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.TwoTwoThree:
					but2.PerformClick();
					but2.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ThreeTwoTwo:
					but3.PerformClick();
					but2.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FiveFourFive:
					but5.PerformClick();
					but4.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FiveThreeFive:
					but5.PerformClick();
					but3.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ThreeThreeFive:
					but3.PerformClick();
					but3.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FiveThreeThree:
					but5.PerformClick();
					but3.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FourFourFive:
					but4.PerformClick();
					but4.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FiveFourFour:
					but5.PerformClick();
					but4.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FiveFiveFive:
					but5.PerformClick();
					but5.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ThreeFourThree:
					but3.PerformClick();
					but4.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FourThreeFive:
					but4.PerformClick();
					but3.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FiveThreeFour:
					but5.PerformClick();
					but3.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ZeroZeroZero:
					but0.PerformClick();
					but0.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ZeroZeroOne:
					but0.PerformClick();
					but0.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ZeroZeroTwo:
					but0.PerformClick();
					but0.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ZeroZeroThree:
					but0.PerformClick();
					but0.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ZeroZeroFour:
					but0.PerformClick();
					but0.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ZeroZeroFive:
					but0.PerformClick();
					but0.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ZeroZeroSix:
					but0.PerformClick();
					but0.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ZeroZeroSeven:
					but0.PerformClick();
					but0.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ZeroZeroEight:
					but0.PerformClick();
					but0.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ZeroZeroNine:
					but0.PerformClick();
					but0.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ZeroOneZero:
					but0.PerformClick();
					but1.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ZeroOneOne:
					but0.PerformClick();
					but1.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ZeroOneTwo:
					but0.PerformClick();
					but1.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ZeroOneThree:
					but0.PerformClick();
					but1.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ZeroOneFour:
					but0.PerformClick();
					but1.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ZeroOneFive:
					but0.PerformClick();
					but1.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ZeroOneSix:
					but0.PerformClick();
					but1.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ZeroOneSeven:
					but0.PerformClick();
					but1.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ZeroOneEight:
					but0.PerformClick();
					but1.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ZeroOneNine:
					but0.PerformClick();
					but1.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ZeroTwoZero:
					but0.PerformClick();
					but2.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ZeroTwoOne:
					but0.PerformClick();
					but2.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ZeroTwoTwo:
					but0.PerformClick();
					but2.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ZeroTwoThree:
					but0.PerformClick();
					but2.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ZeroTwoFour:
					but0.PerformClick();
					but2.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ZeroTwoFive:
					but0.PerformClick();
					but2.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ZeroTwoSix:
					but0.PerformClick();
					but2.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ZeroTwoSeven:
					but0.PerformClick();
					but2.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ZeroTwoEight:
					but0.PerformClick();
					but2.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ZeroTwoNine:
					but0.PerformClick();
					but2.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ZeroThreeZero:
					but0.PerformClick();
					but3.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ZeroThreeOne:
					but0.PerformClick();
					but3.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ZeroThreeTwo:
					but0.PerformClick();
					but3.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ZeroThreeThree:
					but0.PerformClick();
					but3.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ZeroThreeFour:
					but0.PerformClick();
					but3.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ZeroThreeFive:
					but0.PerformClick();
					but3.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ZeroThreeSix:
					but0.PerformClick();
					but3.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ZeroThreeSeven:
					but0.PerformClick();
					but3.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ZeroThreeEight:
					but0.PerformClick();
					but3.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ZeroThreeNine:
					but0.PerformClick();
					but3.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ZeroFourZero:
					but0.PerformClick();
					but4.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ZeroFourOne:
					but0.PerformClick();
					but4.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ZeroFourTwo:
					but0.PerformClick();
					but4.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ZeroFourThree:
					but0.PerformClick();
					but4.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ZeroFourFour:
					but0.PerformClick();
					but4.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ZeroFourFive:
					but0.PerformClick();
					but4.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ZeroFourSix:
					but0.PerformClick();
					but4.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ZeroFourSeven:
					but0.PerformClick();
					but4.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ZeroFourEight:
					but0.PerformClick();
					but4.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ZeroFourNine:
					but0.PerformClick();
					but4.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ZeroFiveZero:
					but0.PerformClick();
					but5.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ZeroFiveOne:
					but0.PerformClick();
					but5.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ZeroFiveTwo:
					but0.PerformClick();
					but5.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ZeroFiveThree:
					but0.PerformClick();
					but5.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ZeroFiveFour:
					but0.PerformClick();
					but5.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ZeroFiveFive:
					but0.PerformClick();
					but5.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ZeroFiveSix:
					but0.PerformClick();
					but5.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ZeroFiveSeven:
					but0.PerformClick();
					but5.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ZeroFiveEight:
					but0.PerformClick();
					but5.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ZeroFiveNine:
					but0.PerformClick();
					but5.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ZeroSixZero:
					but0.PerformClick();
					but6.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ZeroSixOne:
					but0.PerformClick();
					but6.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ZeroSixTwo:
					but0.PerformClick();
					but6.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ZeroSixThree:
					but0.PerformClick();
					but6.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ZeroSixFour:
					but0.PerformClick();
					but6.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ZeroSixFive:
					but0.PerformClick();
					but6.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ZeroSixSix:
					but0.PerformClick();
					but6.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ZeroSixSeven:
					but0.PerformClick();
					but6.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ZeroSixEight:
					but0.PerformClick();
					but6.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ZeroSixNine:
					but0.PerformClick();
					but6.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ZeroSevenZero:
					but0.PerformClick();
					but7.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ZeroSevenOne:
					but0.PerformClick();
					but7.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ZeroSevenTwo:
					but0.PerformClick();
					but7.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ZeroSevenThree:
					but0.PerformClick();
					but7.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ZeroSevenFour:
					but0.PerformClick();
					but7.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ZeroSevenFive:
					but0.PerformClick();
					but7.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ZeroSevenSix:
					but0.PerformClick();
					but7.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ZeroSevenSeven:
					but0.PerformClick();
					but7.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ZeroSevenEight:
					but0.PerformClick();
					but7.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ZeroSevenNine:
					but0.PerformClick();
					but7.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ZeroEightZero:
					but0.PerformClick();
					but8.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ZeroEightOne:
					but0.PerformClick();
					but8.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ZeroEightTwo:
					but0.PerformClick();
					but8.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ZeroEightThree:
					but0.PerformClick();
					but8.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ZeroEightFour:
					but0.PerformClick();
					but8.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ZeroEightFive:
					but0.PerformClick();
					but8.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ZeroEightSix:
					but0.PerformClick();
					but8.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ZeroEightSeven:
					but0.PerformClick();
					but8.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ZeroEightEight:
					but0.PerformClick();
					but8.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ZeroEightNine:
					but0.PerformClick();
					but8.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ZeroNineZero:
					but0.PerformClick();
					but9.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ZeroNineOne:
					but0.PerformClick();
					but9.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ZeroNineTwo:
					but0.PerformClick();
					but9.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ZeroNineThree:
					but0.PerformClick();
					but9.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ZeroNineFour:
					but0.PerformClick();
					but9.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ZeroNineFive:
					but0.PerformClick();
					but9.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ZeroNineSix:
					but0.PerformClick();
					but9.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ZeroNineSeven:
					but0.PerformClick();
					but9.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ZeroNineEight:
					but0.PerformClick();
					but9.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ZeroNineNine:
					but0.PerformClick();
					but9.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.OneZeroZero:
					but1.PerformClick();
					but0.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.OneZeroOne:
					but1.PerformClick();
					but0.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.OneZeroTwo:
					but1.PerformClick();
					but0.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.OneZeroThree:
					but1.PerformClick();
					but0.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.OneZeroFour:
					but1.PerformClick();
					but0.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.OneZeroFive:
					but1.PerformClick();
					but0.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.OneZeroSix:
					but1.PerformClick();
					but0.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.OneZeroSeven:
					but1.PerformClick();
					but0.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.OneZeroEight:
					but1.PerformClick();
					but0.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.OneZeroNine:
					but1.PerformClick();
					but0.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.OneOneZero:
					but1.PerformClick();
					but1.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.OneOneOne:
					but1.PerformClick();
					but1.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.OneOneTwo:
					but1.PerformClick();
					but1.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.OneOneThree:
					but1.PerformClick();
					but1.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.OneOneFour:
					but1.PerformClick();
					but1.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.OneOneFive:
					but1.PerformClick();
					but1.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.OneOneSix:
					but1.PerformClick();
					but1.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.OneOneSeven:
					but1.PerformClick();
					but1.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.OneOneEight:
					but1.PerformClick();
					but1.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.OneOneNine:
					but1.PerformClick();
					but1.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.OneTwoZero:
					but1.PerformClick();
					but2.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.OneTwoOne:
					but1.PerformClick();
					but2.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.OneTwoTwo:
					but1.PerformClick();
					but2.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.OneTwoThree:
					but1.PerformClick();
					but2.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.OneTwoFour:
					but1.PerformClick();
					but2.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.OneTwoFive:
					but1.PerformClick();
					but2.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.OneTwoSix:
					but1.PerformClick();
					but2.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.OneTwoSeven:
					but1.PerformClick();
					but2.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.OneTwoEight:
					but1.PerformClick();
					but2.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.OneTwoNine:
					but1.PerformClick();
					but2.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.OneThreeZero:
					but1.PerformClick();
					but3.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.OneThreeOne:
					but1.PerformClick();
					but3.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.OneThreeTwo:
					but1.PerformClick();
					but3.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.OneThreeThree:
					but1.PerformClick();
					but3.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.OneThreeFour:
					but1.PerformClick();
					but3.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.OneThreeFive:
					but1.PerformClick();
					but3.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.OneThreeSix:
					but1.PerformClick();
					but3.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.OneThreeSeven:
					but1.PerformClick();
					but3.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.OneThreeEight:
					but1.PerformClick();
					but3.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.OneThreeNine:
					but1.PerformClick();
					but3.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.OneFourZero:
					but1.PerformClick();
					but4.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.OneFourOne:
					but1.PerformClick();
					but4.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.OneFourTwo:
					but1.PerformClick();
					but4.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.OneFourThree:
					but1.PerformClick();
					but4.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.OneFourFour:
					but1.PerformClick();
					but4.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.OneFourFive:
					but1.PerformClick();
					but4.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.OneFourSix:
					but1.PerformClick();
					but4.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.OneFourSeven:
					but1.PerformClick();
					but4.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.OneFourEight:
					but1.PerformClick();
					but4.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.OneFourNine:
					but1.PerformClick();
					but4.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.OneFiveZero:
					but1.PerformClick();
					but5.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.OneFiveOne:
					but1.PerformClick();
					but5.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.OneFiveTwo:
					but1.PerformClick();
					but5.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.OneFiveThree:
					but1.PerformClick();
					but5.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.OneFiveFour:
					but1.PerformClick();
					but5.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.OneFiveFive:
					but1.PerformClick();
					but5.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.OneFiveSix:
					but1.PerformClick();
					but5.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.OneFiveSeven:
					but1.PerformClick();
					but5.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.OneFiveEight:
					but1.PerformClick();
					but5.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.OneFiveNine:
					but1.PerformClick();
					but5.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.OneSixZero:
					but1.PerformClick();
					but6.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.OneSixOne:
					but1.PerformClick();
					but6.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.OneSixTwo:
					but1.PerformClick();
					but6.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.OneSixThree:
					but1.PerformClick();
					but6.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.OneSixFour:
					but1.PerformClick();
					but6.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.OneSixFive:
					but1.PerformClick();
					but6.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.OneSixSix:
					but1.PerformClick();
					but6.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.OneSixSeven:
					but1.PerformClick();
					but6.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.OneSixEight:
					but1.PerformClick();
					but6.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.OneSixNine:
					but1.PerformClick();
					but6.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.OneSevenZero:
					but1.PerformClick();
					but7.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.OneSevenOne:
					but1.PerformClick();
					but7.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.OneSevenTwo:
					but1.PerformClick();
					but7.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.OneSevenThree:
					but1.PerformClick();
					but7.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.OneSevenFour:
					but1.PerformClick();
					but7.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.OneSevenFive:
					but1.PerformClick();
					but7.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.OneSevenSix:
					but1.PerformClick();
					but7.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.OneSevenSeven:
					but1.PerformClick();
					but7.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.OneSevenEight:
					but1.PerformClick();
					but7.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.OneSevenNine:
					but1.PerformClick();
					but7.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.OneEightZero:
					but1.PerformClick();
					but8.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.OneEightOne:
					but1.PerformClick();
					but8.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.OneEightTwo:
					but1.PerformClick();
					but8.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.OneEightThree:
					but1.PerformClick();
					but8.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.OneEightFour:
					but1.PerformClick();
					but8.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.OneEightFive:
					but1.PerformClick();
					but8.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.OneEightSix:
					but1.PerformClick();
					but8.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.OneEightSeven:
					but1.PerformClick();
					but8.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.OneEightEight:
					but1.PerformClick();
					but8.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.OneEightNine:
					but1.PerformClick();
					but8.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.OneNineZero:
					but1.PerformClick();
					but9.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.OneNineOne:
					but1.PerformClick();
					but9.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.OneNineTwo:
					but1.PerformClick();
					but9.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.OneNineThree:
					but1.PerformClick();
					but9.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.OneNineFour:
					but1.PerformClick();
					but9.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.OneNineFive:
					but1.PerformClick();
					but9.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.OneNineSix:
					but1.PerformClick();
					but9.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.OneNineSeven:
					but1.PerformClick();
					but9.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.OneNineEight:
					but1.PerformClick();
					but9.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.OneNineNine:
					but1.PerformClick();
					but9.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.TwoZeroZero:
					but2.PerformClick();
					but0.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.TwoZeroOne:
					but2.PerformClick();
					but0.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.TwoZeroTwo:
					but2.PerformClick();
					but0.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.TwoZeroThree:
					but2.PerformClick();
					but0.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.TwoZeroFour:
					but2.PerformClick();
					but0.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.TwoZeroFive:
					but2.PerformClick();
					but0.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.TwoZeroSix:
					but2.PerformClick();
					but0.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.TwoZeroSeven:
					but2.PerformClick();
					but0.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.TwoZeroEight:
					but2.PerformClick();
					but0.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.TwoZeroNine:
					but2.PerformClick();
					but0.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.TwoOneZero:
					but2.PerformClick();
					but1.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.TwoOneOne:
					but2.PerformClick();
					but1.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.TwoOneThree:
					but2.PerformClick();
					but1.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.TwoOneFour:
					but2.PerformClick();
					but1.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.TwoOneFive:
					but2.PerformClick();
					but1.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.TwoOneSix:
					but2.PerformClick();
					but1.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.TwoOneSeven:
					but2.PerformClick();
					but1.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.TwoOneEight:
					but2.PerformClick();
					but1.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.TwoOneNine:
					but2.PerformClick();
					but1.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.TwoTwoZero:
					but2.PerformClick();
					but2.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.TwoTwoOne:
					but2.PerformClick();
					but2.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.TwoTwoFour:
					but2.PerformClick();
					but2.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.TwoTwoFive:
					but2.PerformClick();
					but2.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.TwoTwoSix:
					but2.PerformClick();
					but2.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.TwoTwoSeven:
					but2.PerformClick();
					but2.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.TwoTwoEight:
					but2.PerformClick();
					but2.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.TwoTwoNine:
					but2.PerformClick();
					but2.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.TwoThreeZero:
					but2.PerformClick();
					but3.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.TwoThreeOne:
					but2.PerformClick();
					but3.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.TwoThreeTwo:
					but2.PerformClick();
					but3.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.TwoThreeThree:
					but2.PerformClick();
					but3.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.TwoThreeFour:
					but2.PerformClick();
					but3.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.TwoThreeFive:
					but2.PerformClick();
					but3.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.TwoThreeSix:
					but2.PerformClick();
					but3.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.TwoThreeSeven:
					but2.PerformClick();
					but3.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.TwoThreeEight:
					but2.PerformClick();
					but3.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.TwoThreeNine:
					but2.PerformClick();
					but3.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.TwoFourZero:
					but2.PerformClick();
					but4.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.TwoFourOne:
					but2.PerformClick();
					but4.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.TwoFourTwo:
					but2.PerformClick();
					but4.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.TwoFourThree:
					but2.PerformClick();
					but4.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.TwoFourFour:
					but2.PerformClick();
					but4.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.TwoFourFive:
					but2.PerformClick();
					but4.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.TwoFourSix:
					but2.PerformClick();
					but4.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.TwoFourSeven:
					but2.PerformClick();
					but4.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.TwoFourEight:
					but2.PerformClick();
					but4.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.TwoFourNine:
					but2.PerformClick();
					but4.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.TwoFiveZero:
					but2.PerformClick();
					but5.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.TwoFiveOne:
					but2.PerformClick();
					but5.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.TwoFiveTwo:
					but2.PerformClick();
					but5.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.TwoFiveThree:
					but2.PerformClick();
					but5.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.TwoFiveFour:
					but2.PerformClick();
					but5.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.TwoFiveFive:
					but2.PerformClick();
					but5.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.TwoFiveSix:
					but2.PerformClick();
					but5.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.TwoFiveSeven:
					but2.PerformClick();
					but5.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.TwoFiveEight:
					but2.PerformClick();
					but5.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.TwoFiveNine:
					but2.PerformClick();
					but5.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.TwoSixZero:
					but2.PerformClick();
					but6.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.TwoSixOne:
					but2.PerformClick();
					but6.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.TwoSixTwo:
					but2.PerformClick();
					but6.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.TwoSixThree:
					but2.PerformClick();
					but6.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.TwoSixFour:
					but2.PerformClick();
					but6.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.TwoSixFive:
					but2.PerformClick();
					but6.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.TwoSixSix:
					but2.PerformClick();
					but6.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.TwoSixSeven:
					but2.PerformClick();
					but6.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.TwoSixEight:
					but2.PerformClick();
					but6.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.TwoSixNine:
					but2.PerformClick();
					but6.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.TwoSevenZero:
					but2.PerformClick();
					but7.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.TwoSevenOne:
					but2.PerformClick();
					but7.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.TwoSevenTwo:
					but2.PerformClick();
					but7.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.TwoSevenThree:
					but2.PerformClick();
					but7.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.TwoSevenFour:
					but2.PerformClick();
					but7.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.TwoSevenFive:
					but2.PerformClick();
					but7.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.TwoSevenSix:
					but2.PerformClick();
					but7.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.TwoSevenSeven:
					but2.PerformClick();
					but7.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.TwoSevenEight:
					but2.PerformClick();
					but7.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.TwoSevenNine:
					but2.PerformClick();
					but7.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.TwoEightZero:
					but2.PerformClick();
					but8.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.TwoEightOne:
					but2.PerformClick();
					but8.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.TwoEightTwo:
					but2.PerformClick();
					but8.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.TwoEightThree:
					but2.PerformClick();
					but8.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.TwoEightFour:
					but2.PerformClick();
					but8.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.TwoEightFive:
					but2.PerformClick();
					but8.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.TwoEightSix:
					but2.PerformClick();
					but8.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.TwoEightSeven:
					but2.PerformClick();
					but8.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.TwoEightEight:
					but2.PerformClick();
					but8.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.TwoEightNine:
					but2.PerformClick();
					but8.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.TwoNineZero:
					but2.PerformClick();
					but9.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.TwoNineOne:
					but2.PerformClick();
					but9.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.TwoNineTwo:
					but2.PerformClick();
					but9.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.TwoNineThree:
					but2.PerformClick();
					but9.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.TwoNineFour:
					but2.PerformClick();
					but9.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.TwoNineFive:
					but2.PerformClick();
					but9.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.TwoNineSix:
					but2.PerformClick();
					but9.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.TwoNineSeven:
					but2.PerformClick();
					but9.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.TwoNineEight:
					but2.PerformClick();
					but9.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.TwoNineNine:
					but2.PerformClick();
					but9.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ThreeZeroZero:
					but3.PerformClick();
					but0.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ThreeZeroOne:
					but3.PerformClick();
					but0.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ThreeZeroTwo:
					but3.PerformClick();
					but0.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ThreeZeroThree:
					but3.PerformClick();
					but0.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ThreeZeroFour:
					but3.PerformClick();
					but0.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ThreeZeroFive:
					but3.PerformClick();
					but0.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ThreeZeroSix:
					but3.PerformClick();
					but0.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ThreeZeroSeven:
					but3.PerformClick();
					but0.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ThreeZeroEight:
					but3.PerformClick();
					but0.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ThreeZeroNine:
					but3.PerformClick();
					but0.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ThreeOneZero:
					but3.PerformClick();
					but1.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ThreeOneOne:
					but3.PerformClick();
					but1.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ThreeOneTwo:
					but3.PerformClick();
					but1.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ThreeOneThree:
					but3.PerformClick();
					but1.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ThreeOneFour:
					but3.PerformClick();
					but1.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ThreeOneFive:
					but3.PerformClick();
					but1.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ThreeOneSix:
					but3.PerformClick();
					but1.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ThreeOneSeven:
					but3.PerformClick();
					but1.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ThreeOneEight:
					but3.PerformClick();
					but1.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ThreeOneNine:
					but3.PerformClick();
					but1.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ThreeTwoZero:
					but3.PerformClick();
					but2.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ThreeTwoOne:
					but3.PerformClick();
					but2.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ThreeTwoFour:
					but3.PerformClick();
					but2.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ThreeTwoFive:
					but3.PerformClick();
					but2.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ThreeTwoSix:
					but3.PerformClick();
					but2.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ThreeTwoSeven:
					but3.PerformClick();
					but2.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ThreeTwoEight:
					but3.PerformClick();
					but2.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ThreeTwoNine:
					but3.PerformClick();
					but2.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ThreeThreeZero:
					but3.PerformClick();
					but3.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ThreeThreeOne:
					but3.PerformClick();
					but3.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ThreeThreeTwo:
					but3.PerformClick();
					but3.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ThreeThreeSix:
					but3.PerformClick();
					but3.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ThreeThreeSeven:
					but3.PerformClick();
					but3.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ThreeThreeEight:
					but3.PerformClick();
					but3.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ThreeThreeNine:
					but3.PerformClick();
					but3.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ThreeFourZero:
					but3.PerformClick();
					but4.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ThreeFourOne:
					but3.PerformClick();
					but4.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ThreeFourTwo:
					but3.PerformClick();
					but4.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ThreeFourFour:
					but3.PerformClick();
					but4.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ThreeFourFive:
					but3.PerformClick();
					but4.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ThreeFourSix:
					but3.PerformClick();
					but4.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ThreeFourSeven:
					but3.PerformClick();
					but4.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ThreeFourEight:
					but3.PerformClick();
					but4.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ThreeFourNine:
					but3.PerformClick();
					but4.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ThreeFiveZero:
					but3.PerformClick();
					but5.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ThreeFiveOne:
					but3.PerformClick();
					but5.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ThreeFiveTwo:
					but3.PerformClick();
					but5.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ThreeFiveThree:
					but3.PerformClick();
					but5.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ThreeFiveFour:
					but3.PerformClick();
					but5.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ThreeFiveFive:
					but3.PerformClick();
					but5.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ThreeFiveSix:
					but3.PerformClick();
					but5.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ThreeFiveSeven:
					but3.PerformClick();
					but5.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ThreeFiveEight:
					but3.PerformClick();
					but5.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ThreeFiveNine:
					but3.PerformClick();
					but5.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ThreeSixZero:
					but3.PerformClick();
					but6.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ThreeSixOne:
					but3.PerformClick();
					but6.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ThreeSixTwo:
					but3.PerformClick();
					but6.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ThreeSixThree:
					but3.PerformClick();
					but6.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ThreeSixFour:
					but3.PerformClick();
					but6.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ThreeSixFive:
					but3.PerformClick();
					but6.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ThreeSixSix:
					but3.PerformClick();
					but6.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ThreeSixSeven:
					but3.PerformClick();
					but6.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ThreeSixEight:
					but3.PerformClick();
					but6.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ThreeSixNine:
					but3.PerformClick();
					but6.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ThreeSevenZero:
					but3.PerformClick();
					but7.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ThreeSevenOne:
					but3.PerformClick();
					but7.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ThreeSevenTwo:
					but3.PerformClick();
					but7.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ThreeSevenThree:
					but3.PerformClick();
					but7.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ThreeSevenFour:
					but3.PerformClick();
					but7.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ThreeSevenFive:
					but3.PerformClick();
					but7.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ThreeSevenSix:
					but3.PerformClick();
					but7.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ThreeSevenSeven:
					but3.PerformClick();
					but7.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ThreeSevenEight:
					but3.PerformClick();
					but7.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ThreeSevenNine:
					but3.PerformClick();
					but7.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ThreeEightZero:
					but3.PerformClick();
					but8.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ThreeEightOne:
					but3.PerformClick();
					but8.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ThreeEightTwo:
					but3.PerformClick();
					but8.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ThreeEightThree:
					but3.PerformClick();
					but8.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ThreeEightFour:
					but3.PerformClick();
					but8.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ThreeEightFive:
					but3.PerformClick();
					but8.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ThreeEightSix:
					but3.PerformClick();
					but8.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ThreeEightSeven:
					but3.PerformClick();
					but8.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ThreeEightEight:
					but3.PerformClick();
					but8.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ThreeEightNine:
					but3.PerformClick();
					but8.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ThreeNineZero:
					but3.PerformClick();
					but9.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ThreeNineOne:
					but3.PerformClick();
					but9.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ThreeNineTwo:
					but3.PerformClick();
					but9.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ThreeNineThree:
					but3.PerformClick();
					but9.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ThreeNineFour:
					but3.PerformClick();
					but9.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ThreeNineFive:
					but3.PerformClick();
					but9.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ThreeNineSix:
					but3.PerformClick();
					but9.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ThreeNineSeven:
					but3.PerformClick();
					but9.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ThreeNineEight:
					but3.PerformClick();
					but9.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ThreeNineNine:
					but3.PerformClick();
					but9.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FourZeroZero:
					but4.PerformClick();
					but0.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FourZeroOne:
					but4.PerformClick();
					but0.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FourZeroTwo:
					but4.PerformClick();
					but0.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FourZeroThree:
					but4.PerformClick();
					but0.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FourZeroFour:
					but4.PerformClick();
					but0.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FourZeroFive:
					but4.PerformClick();
					but0.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FourZeroSix:
					but4.PerformClick();
					but0.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FourZeroSeven:
					but4.PerformClick();
					but0.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FourZeroEight:
					but4.PerformClick();
					but0.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FourZeroNine:
					but4.PerformClick();
					but0.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FourOneZero:
					but4.PerformClick();
					but1.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FourOneOne:
					but4.PerformClick();
					but1.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FourOneTwo:
					but4.PerformClick();
					but1.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FourOneThree:
					but4.PerformClick();
					but1.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FourOneFour:
					but4.PerformClick();
					but1.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FourOneFive:
					but4.PerformClick();
					but1.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FourOneSix:
					but4.PerformClick();
					but1.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FourOneSeven:
					but4.PerformClick();
					but1.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FourOneEight:
					but4.PerformClick();
					but1.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FourOneNine:
					but4.PerformClick();
					but1.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FourTwoZero:
					but4.PerformClick();
					but2.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FourTwoOne:
					but4.PerformClick();
					but2.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FourTwoTwo:
					but4.PerformClick();
					but2.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FourTwoThree:
					but4.PerformClick();
					but2.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FourTwoFour:
					but4.PerformClick();
					but2.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FourTwoFive:
					but4.PerformClick();
					but2.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FourTwoSix:
					but4.PerformClick();
					but2.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FourTwoSeven:
					but4.PerformClick();
					but2.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FourTwoEight:
					but4.PerformClick();
					but2.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FourTwoNine:
					but4.PerformClick();
					but2.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FourThreeZero:
					but4.PerformClick();
					but3.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FourThreeOne:
					but4.PerformClick();
					but3.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FourThreeTwo:
					but4.PerformClick();
					but3.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FourThreeSix:
					but4.PerformClick();
					but3.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FourThreeSeven:
					but4.PerformClick();
					but3.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FourThreeEight:
					but4.PerformClick();
					but3.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FourThreeNine:
					but4.PerformClick();
					but3.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FourFourZero:
					but4.PerformClick();
					but4.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FourFourOne:
					but4.PerformClick();
					but4.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FourFourTwo:
					but4.PerformClick();
					but4.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FourFourThree:
					but4.PerformClick();
					but4.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FourFourSix:
					but4.PerformClick();
					but4.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FourFourSeven:
					but4.PerformClick();
					but4.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FourFourEight:
					but4.PerformClick();
					but4.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FourFourNine:
					but4.PerformClick();
					but4.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FourFiveZero:
					but4.PerformClick();
					but5.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FourFiveOne:
					but4.PerformClick();
					but5.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FourFiveTwo:
					but4.PerformClick();
					but5.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FourFiveThree:
					but4.PerformClick();
					but5.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FourFiveFour:
					but4.PerformClick();
					but5.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FourFiveFive:
					but4.PerformClick();
					but5.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FourFiveSix:
					but4.PerformClick();
					but5.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FourFiveSeven:
					but4.PerformClick();
					but5.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FourFiveEight:
					but4.PerformClick();
					but5.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FourFiveNine:
					but4.PerformClick();
					but5.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FourSixZero:
					but4.PerformClick();
					but6.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FourSixOne:
					but4.PerformClick();
					but6.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FourSixTwo:
					but4.PerformClick();
					but6.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FourSixThree:
					but4.PerformClick();
					but6.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FourSixFour:
					but4.PerformClick();
					but6.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FourSixFive:
					but4.PerformClick();
					but6.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FourSixSix:
					but4.PerformClick();
					but6.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FourSixSeven:
					but4.PerformClick();
					but6.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FourSixEight:
					but4.PerformClick();
					but6.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FourSixNine:
					but4.PerformClick();
					but6.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FourSevenZero:
					but4.PerformClick();
					but7.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FourSevenOne:
					but4.PerformClick();
					but7.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FourSevenTwo:
					but4.PerformClick();
					but7.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FourSevenThree:
					but4.PerformClick();
					but7.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FourSevenFour:
					but4.PerformClick();
					but7.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FourSevenFive:
					but4.PerformClick();
					but7.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FourSevenSix:
					but4.PerformClick();
					but7.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FourSevenSeven:
					but4.PerformClick();
					but7.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FourSevenEight:
					but4.PerformClick();
					but7.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FourSevenNine:
					but4.PerformClick();
					but7.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FourEightZero:
					but4.PerformClick();
					but8.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FourEightOne:
					but4.PerformClick();
					but8.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FourEightTwo:
					but4.PerformClick();
					but8.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FourEightThree:
					but4.PerformClick();
					but8.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FourEightFour:
					but4.PerformClick();
					but8.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FourEightFive:
					but4.PerformClick();
					but8.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FourEightSix:
					but4.PerformClick();
					but8.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FourEightSeven:
					but4.PerformClick();
					but8.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FourEightEight:
					but4.PerformClick();
					but8.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FourEightNine:
					but4.PerformClick();
					but8.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FourNineZero:
					but4.PerformClick();
					but9.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FourNineOne:
					but4.PerformClick();
					but9.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FourNineTwo:
					but4.PerformClick();
					but9.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FourNineThree:
					but4.PerformClick();
					but9.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FourNineFour:
					but4.PerformClick();
					but9.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FourNineFive:
					but4.PerformClick();
					but9.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FourNineSix:
					but4.PerformClick();
					but9.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FourNineSeven:
					but4.PerformClick();
					but9.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FourNineEight:
					but4.PerformClick();
					but9.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FourNineNine:
					but4.PerformClick();
					but9.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FiveZeroZero:
					but5.PerformClick();
					but0.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FiveZeroOne:
					but5.PerformClick();
					but0.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FiveZeroTwo:
					but5.PerformClick();
					but0.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FiveZeroThree:
					but5.PerformClick();
					but0.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FiveZeroFour:
					but5.PerformClick();
					but0.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FiveZeroFive:
					but5.PerformClick();
					but0.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FiveZeroSix:
					but5.PerformClick();
					but0.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FiveZeroSeven:
					but5.PerformClick();
					but0.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FiveZeroEight:
					but5.PerformClick();
					but0.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FiveZeroNine:
					but5.PerformClick();
					but0.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FiveOneZero:
					but5.PerformClick();
					but1.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FiveOneOne:
					but5.PerformClick();
					but1.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FiveOneTwo:
					but5.PerformClick();
					but1.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FiveOneThree:
					but5.PerformClick();
					but1.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FiveOneFour:
					but5.PerformClick();
					but1.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FiveOneFive:
					but5.PerformClick();
					but1.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FiveOneSix:
					but5.PerformClick();
					but1.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FiveOneSeven:
					but5.PerformClick();
					but1.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FiveOneEight:
					but5.PerformClick();
					but1.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FiveOneNine:
					but5.PerformClick();
					but1.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FiveTwoZero:
					but5.PerformClick();
					but2.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FiveTwoOne:
					but5.PerformClick();
					but2.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FiveTwoTwo:
					but5.PerformClick();
					but2.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FiveTwoThree:
					but5.PerformClick();
					but2.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FiveTwoFour:
					but5.PerformClick();
					but2.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FiveTwoFive:
					but5.PerformClick();
					but2.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FiveTwoSix:
					but5.PerformClick();
					but2.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FiveTwoSeven:
					but5.PerformClick();
					but2.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FiveTwoEight:
					but5.PerformClick();
					but2.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FiveTwoNine:
					but5.PerformClick();
					but2.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FiveThreeZero:
					but5.PerformClick();
					but3.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FiveThreeOne:
					but5.PerformClick();
					but3.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FiveThreeTwo:
					but5.PerformClick();
					but3.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FiveThreeSix:
					but5.PerformClick();
					but3.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FiveThreeSeven:
					but5.PerformClick();
					but3.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FiveThreeEight:
					but5.PerformClick();
					but3.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FiveThreeNine:
					but5.PerformClick();
					but3.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FiveFourZero:
					but5.PerformClick();
					but4.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FiveFourOne:
					but5.PerformClick();
					but4.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FiveFourTwo:
					but5.PerformClick();
					but4.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FiveFourThree:
					but5.PerformClick();
					but4.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FiveFourSix:
					but5.PerformClick();
					but4.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FiveFourSeven:
					but5.PerformClick();
					but4.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FiveFourEight:
					but5.PerformClick();
					but4.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FiveFourNine:
					but5.PerformClick();
					but4.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FiveFiveZero:
					but5.PerformClick();
					but5.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FiveFiveOne:
					but5.PerformClick();
					but5.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FiveFiveTwo:
					but5.PerformClick();
					but5.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FiveFiveThree:
					but5.PerformClick();
					but5.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FiveFiveFour:
					but5.PerformClick();
					but5.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FiveFiveSix:
					but5.PerformClick();
					but5.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FiveFiveSeven:
					but5.PerformClick();
					but5.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FiveFiveEight:
					but5.PerformClick();
					but5.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FiveFiveNine:
					but5.PerformClick();
					but5.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FiveSixZero:
					but5.PerformClick();
					but6.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FiveSixOne:
					but5.PerformClick();
					but6.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FiveSixTwo:
					but5.PerformClick();
					but6.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FiveSixThree:
					but5.PerformClick();
					but6.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FiveSixFour:
					but5.PerformClick();
					but6.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FiveSixFive:
					but5.PerformClick();
					but6.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FiveSixSix:
					but5.PerformClick();
					but6.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FiveSixSeven:
					but5.PerformClick();
					but6.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FiveSixEight:
					but5.PerformClick();
					but6.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FiveSixNine:
					but5.PerformClick();
					but6.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FiveSevenZero:
					but5.PerformClick();
					but7.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FiveSevenOne:
					but5.PerformClick();
					but7.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FiveSevenTwo:
					but5.PerformClick();
					but7.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FiveSevenThree:
					but5.PerformClick();
					but7.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FiveSevenFour:
					but5.PerformClick();
					but7.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FiveSevenFive:
					but5.PerformClick();
					but7.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FiveSevenSix:
					but5.PerformClick();
					but7.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FiveSevenSeven:
					but5.PerformClick();
					but7.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FiveSevenEight:
					but5.PerformClick();
					but7.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FiveSevenNine:
					but5.PerformClick();
					but7.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FiveEightZero:
					but5.PerformClick();
					but8.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FiveEightOne:
					but5.PerformClick();
					but8.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FiveEightTwo:
					but5.PerformClick();
					but8.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FiveEightThree:
					but5.PerformClick();
					but8.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FiveEightFour:
					but5.PerformClick();
					but8.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FiveEightFive:
					but5.PerformClick();
					but8.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FiveEightSix:
					but5.PerformClick();
					but8.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FiveEightSeven:
					but5.PerformClick();
					but8.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FiveEightEight:
					but5.PerformClick();
					but8.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FiveEightNine:
					but5.PerformClick();
					but8.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FiveNineZero:
					but5.PerformClick();
					but9.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FiveNineOne:
					but5.PerformClick();
					but9.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FiveNineTwo:
					but5.PerformClick();
					but9.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FiveNineThree:
					but5.PerformClick();
					but9.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FiveNineFour:
					but5.PerformClick();
					but9.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FiveNineFive:
					but5.PerformClick();
					but9.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FiveNineSix:
					but5.PerformClick();
					but9.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FiveNineSeven:
					but5.PerformClick();
					but9.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FiveNineEight:
					but5.PerformClick();
					but9.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FiveNineNine:
					but5.PerformClick();
					but9.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SixZeroZero:
					but6.PerformClick();
					but0.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SixZeroOne:
					but6.PerformClick();
					but0.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SixZeroTwo:
					but6.PerformClick();
					but0.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SixZeroThree:
					but6.PerformClick();
					but0.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SixZeroFour:
					but6.PerformClick();
					but0.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SixZeroFive:
					but6.PerformClick();
					but0.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SixZeroSix:
					but6.PerformClick();
					but0.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SixZeroSeven:
					but6.PerformClick();
					but0.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SixZeroEight:
					but6.PerformClick();
					but0.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SixZeroNine:
					but6.PerformClick();
					but0.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SixOneZero:
					but6.PerformClick();
					but1.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SixOneOne:
					but6.PerformClick();
					but1.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SixOneTwo:
					but6.PerformClick();
					but1.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SixOneThree:
					but6.PerformClick();
					but1.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SixOneFour:
					but6.PerformClick();
					but1.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SixOneFive:
					but6.PerformClick();
					but1.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SixOneSix:
					but6.PerformClick();
					but1.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SixOneSeven:
					but6.PerformClick();
					but1.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SixOneEight:
					but6.PerformClick();
					but1.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SixOneNine:
					but6.PerformClick();
					but1.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SixTwoZero:
					but6.PerformClick();
					but2.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SixTwoOne:
					but6.PerformClick();
					but2.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SixTwoTwo:
					but6.PerformClick();
					but2.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SixTwoThree:
					but6.PerformClick();
					but2.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SixTwoFour:
					but6.PerformClick();
					but2.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SixTwoFive:
					but6.PerformClick();
					but2.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SixTwoSix:
					but6.PerformClick();
					but2.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SixTwoSeven:
					but6.PerformClick();
					but2.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SixTwoEight:
					but6.PerformClick();
					but2.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SixTwoNine:
					but6.PerformClick();
					but2.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SixThreeZero:
					but6.PerformClick();
					but3.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SixThreeOne:
					but6.PerformClick();
					but3.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SixThreeTwo:
					but6.PerformClick();
					but3.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SixThreeThree:
					but6.PerformClick();
					but3.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SixThreeFour:
					but6.PerformClick();
					but3.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SixThreeFive:
					but6.PerformClick();
					but3.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SixThreeSix:
					but6.PerformClick();
					but3.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SixThreeSeven:
					but6.PerformClick();
					but3.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SixThreeEight:
					but6.PerformClick();
					but3.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SixThreeNine:
					but6.PerformClick();
					but3.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SixFourZero:
					but6.PerformClick();
					but4.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SixFourOne:
					but6.PerformClick();
					but4.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SixFourTwo:
					but6.PerformClick();
					but4.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SixFourThree:
					but6.PerformClick();
					but4.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SixFourFour:
					but6.PerformClick();
					but4.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SixFourFive:
					but6.PerformClick();
					but4.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SixFourSix:
					but6.PerformClick();
					but4.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SixFourSeven:
					but6.PerformClick();
					but4.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SixFourEight:
					but6.PerformClick();
					but4.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SixFourNine:
					but6.PerformClick();
					but4.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SixFiveZero:
					but6.PerformClick();
					but5.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SixFiveOne:
					but6.PerformClick();
					but5.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SixFiveTwo:
					but6.PerformClick();
					but5.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SixFiveThree:
					but6.PerformClick();
					but5.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SixFiveFour:
					but6.PerformClick();
					but5.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SixFiveFive:
					but6.PerformClick();
					but5.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SixFiveSix:
					but6.PerformClick();
					but5.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SixFiveSeven:
					but6.PerformClick();
					but5.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SixFiveEight:
					but6.PerformClick();
					but5.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SixFiveNine:
					but6.PerformClick();
					but5.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SixSixZero:
					but6.PerformClick();
					but6.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SixSixOne:
					but6.PerformClick();
					but6.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SixSixTwo:
					but6.PerformClick();
					but6.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SixSixThree:
					but6.PerformClick();
					but6.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SixSixFour:
					but6.PerformClick();
					but6.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SixSixFive:
					but6.PerformClick();
					but6.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SixSixSix:
					but6.PerformClick();
					but6.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SixSixSeven:
					but6.PerformClick();
					but6.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SixSixEight:
					but6.PerformClick();
					but6.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SixSixNine:
					but6.PerformClick();
					but6.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SixSevenZero:
					but6.PerformClick();
					but7.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SixSevenOne:
					but6.PerformClick();
					but7.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SixSevenTwo:
					but6.PerformClick();
					but7.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SixSevenThree:
					but6.PerformClick();
					but7.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SixSevenFour:
					but6.PerformClick();
					but7.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SixSevenFive:
					but6.PerformClick();
					but7.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SixSevenSix:
					but6.PerformClick();
					but7.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SixSevenSeven:
					but6.PerformClick();
					but7.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SixSevenEight:
					but6.PerformClick();
					but7.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SixSevenNine:
					but6.PerformClick();
					but7.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SixEightZero:
					but6.PerformClick();
					but8.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SixEightOne:
					but6.PerformClick();
					but8.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SixEightTwo:
					but6.PerformClick();
					but8.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SixEightThree:
					but6.PerformClick();
					but8.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SixEightFour:
					but6.PerformClick();
					but8.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SixEightFive:
					but6.PerformClick();
					but8.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SixEightSix:
					but6.PerformClick();
					but8.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SixEightSeven:
					but6.PerformClick();
					but8.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SixEightEight:
					but6.PerformClick();
					but8.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SixEightNine:
					but6.PerformClick();
					but8.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SixNineZero:
					but6.PerformClick();
					but9.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SixNineOne:
					but6.PerformClick();
					but9.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SixNineTwo:
					but6.PerformClick();
					but9.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SixNineThree:
					but6.PerformClick();
					but9.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SixNineFour:
					but6.PerformClick();
					but9.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SixNineFive:
					but6.PerformClick();
					but9.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SixNineSix:
					but6.PerformClick();
					but9.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SixNineSeven:
					but6.PerformClick();
					but9.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SixNineEight:
					but6.PerformClick();
					but9.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SixNineNine:
					but6.PerformClick();
					but9.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SevenZeroZero:
					but7.PerformClick();
					but0.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SevenZeroOne:
					but7.PerformClick();
					but0.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SevenZeroTwo:
					but7.PerformClick();
					but0.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SevenZeroThree:
					but7.PerformClick();
					but0.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SevenZeroFour:
					but7.PerformClick();
					but0.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SevenZeroFive:
					but7.PerformClick();
					but0.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SevenZeroSix:
					but7.PerformClick();
					but0.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SevenZeroSeven:
					but7.PerformClick();
					but0.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SevenZeroEight:
					but7.PerformClick();
					but0.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SevenZeroNine:
					but7.PerformClick();
					but0.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SevenOneZero:
					but7.PerformClick();
					but1.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SevenOneOne:
					but7.PerformClick();
					but1.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SevenOneTwo:
					but7.PerformClick();
					but1.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SevenOneThree:
					but7.PerformClick();
					but1.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SevenOneFour:
					but7.PerformClick();
					but1.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SevenOneFive:
					but7.PerformClick();
					but1.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SevenOneSix:
					but7.PerformClick();
					but1.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SevenOneSeven:
					but7.PerformClick();
					but1.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SevenOneEight:
					but7.PerformClick();
					but1.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SevenOneNine:
					but7.PerformClick();
					but1.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SevenTwoZero:
					but7.PerformClick();
					but2.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SevenTwoOne:
					but7.PerformClick();
					but2.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SevenTwoTwo:
					but7.PerformClick();
					but2.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SevenTwoThree:
					but7.PerformClick();
					but2.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SevenTwoFour:
					but7.PerformClick();
					but2.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SevenTwoFive:
					but7.PerformClick();
					but2.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SevenTwoSix:
					but7.PerformClick();
					but2.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SevenTwoSeven:
					but7.PerformClick();
					but2.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SevenTwoEight:
					but7.PerformClick();
					but2.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SevenTwoNine:
					but7.PerformClick();
					but2.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SevenThreeZero:
					but7.PerformClick();
					but3.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SevenThreeOne:
					but7.PerformClick();
					but3.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SevenThreeTwo:
					but7.PerformClick();
					but3.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SevenThreeThree:
					but7.PerformClick();
					but3.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SevenThreeFour:
					but7.PerformClick();
					but3.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SevenThreeFive:
					but7.PerformClick();
					but3.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SevenThreeSix:
					but7.PerformClick();
					but3.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SevenThreeSeven:
					but7.PerformClick();
					but3.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SevenThreeEight:
					but7.PerformClick();
					but3.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SevenThreeNine:
					but7.PerformClick();
					but3.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SevenFourZero:
					but7.PerformClick();
					but4.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SevenFourOne:
					but7.PerformClick();
					but4.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SevenFourTwo:
					but7.PerformClick();
					but4.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SevenFourThree:
					but7.PerformClick();
					but4.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SevenFourFour:
					but7.PerformClick();
					but4.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SevenFourFive:
					but7.PerformClick();
					but4.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SevenFourSix:
					but7.PerformClick();
					but4.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SevenFourSeven:
					but7.PerformClick();
					but4.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SevenFourEight:
					but7.PerformClick();
					but4.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SevenFourNine:
					but7.PerformClick();
					but4.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SevenFiveZero:
					but7.PerformClick();
					but5.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SevenFiveOne:
					but7.PerformClick();
					but5.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SevenFiveTwo:
					but7.PerformClick();
					but5.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SevenFiveThree:
					but7.PerformClick();
					but5.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SevenFiveFour:
					but7.PerformClick();
					but5.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SevenFiveFive:
					but7.PerformClick();
					but5.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SevenFiveSix:
					but7.PerformClick();
					but5.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SevenFiveSeven:
					but7.PerformClick();
					but5.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SevenFiveEight:
					but7.PerformClick();
					but5.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SevenFiveNine:
					but7.PerformClick();
					but5.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SevenSixZero:
					but7.PerformClick();
					but6.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SevenSixOne:
					but7.PerformClick();
					but6.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SevenSixTwo:
					but7.PerformClick();
					but6.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SevenSixThree:
					but7.PerformClick();
					but6.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SevenSixFour:
					but7.PerformClick();
					but6.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SevenSixFive:
					but7.PerformClick();
					but6.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SevenSixSix:
					but7.PerformClick();
					but6.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SevenSixSeven:
					but7.PerformClick();
					but6.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SevenSixEight:
					but7.PerformClick();
					but6.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SevenSixNine:
					but7.PerformClick();
					but6.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SevenSevenZero:
					but7.PerformClick();
					but7.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SevenSevenOne:
					but7.PerformClick();
					but7.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SevenSevenTwo:
					but7.PerformClick();
					but7.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SevenSevenThree:
					but7.PerformClick();
					but7.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SevenSevenFour:
					but7.PerformClick();
					but7.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SevenSevenFive:
					but7.PerformClick();
					but7.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SevenSevenSix:
					but7.PerformClick();
					but7.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SevenSevenSeven:
					but7.PerformClick();
					but7.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SevenSevenEight:
					but7.PerformClick();
					but7.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SevenSevenNine:
					but7.PerformClick();
					but7.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SevenEightZero:
					but7.PerformClick();
					but8.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SevenEightOne:
					but7.PerformClick();
					but8.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SevenEightTwo:
					but7.PerformClick();
					but8.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SevenEightThree:
					but7.PerformClick();
					but8.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SevenEightFour:
					but7.PerformClick();
					but8.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SevenEightFive:
					but7.PerformClick();
					but8.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SevenEightSix:
					but7.PerformClick();
					but8.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SevenEightSeven:
					but7.PerformClick();
					but8.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SevenEightEight:
					but7.PerformClick();
					but8.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SevenEightNine:
					but7.PerformClick();
					but8.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SevenNineZero:
					but7.PerformClick();
					but9.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SevenNineOne:
					but7.PerformClick();
					but9.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SevenNineTwo:
					but7.PerformClick();
					but9.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SevenNineThree:
					but7.PerformClick();
					but9.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SevenNineFour:
					but7.PerformClick();
					but9.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SevenNineFive:
					but7.PerformClick();
					but9.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SevenNineSix:
					but7.PerformClick();
					but9.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SevenNineSeven:
					but7.PerformClick();
					but9.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SevenNineEight:
					but7.PerformClick();
					but9.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SevenNineNine:
					but7.PerformClick();
					but9.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.EightZeroZero:
					but8.PerformClick();
					but0.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.EightZeroOne:
					but8.PerformClick();
					but0.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.EightZeroTwo:
					but8.PerformClick();
					but0.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.EightZeroThree:
					but8.PerformClick();
					but0.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.EightZeroFour:
					but8.PerformClick();
					but0.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.EightZeroFive:
					but8.PerformClick();
					but0.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.EightZeroSix:
					but8.PerformClick();
					but0.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.EightZeroSeven:
					but8.PerformClick();
					but0.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.EightZeroEight:
					but8.PerformClick();
					but0.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.EightZeroNine:
					but8.PerformClick();
					but0.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.EightOneZero:
					but8.PerformClick();
					but1.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.EightOneOne:
					but8.PerformClick();
					but1.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.EightOneTwo:
					but8.PerformClick();
					but1.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.EightOneThree:
					but8.PerformClick();
					but1.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.EightOneFour:
					but8.PerformClick();
					but1.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.EightOneFive:
					but8.PerformClick();
					but1.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.EightOneSix:
					but8.PerformClick();
					but1.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.EightOneSeven:
					but8.PerformClick();
					but1.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.EightOneEight:
					but8.PerformClick();
					but1.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.EightOneNine:
					but8.PerformClick();
					but1.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.EightTwoZero:
					but8.PerformClick();
					but2.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.EightTwoOne:
					but8.PerformClick();
					but2.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.EightTwoTwo:
					but8.PerformClick();
					but2.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.EightTwoThree:
					but8.PerformClick();
					but2.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.EightTwoFour:
					but8.PerformClick();
					but2.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.EightTwoFive:
					but8.PerformClick();
					but2.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.EightTwoSix:
					but8.PerformClick();
					but2.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.EightTwoSeven:
					but8.PerformClick();
					but2.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.EightTwoEight:
					but8.PerformClick();
					but2.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.EightTwoNine:
					but8.PerformClick();
					but2.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.EightThreeZero:
					but8.PerformClick();
					but3.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.EightThreeOne:
					but8.PerformClick();
					but3.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.EightThreeTwo:
					but8.PerformClick();
					but3.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.EightThreeThree:
					but8.PerformClick();
					but3.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.EightThreeFour:
					but8.PerformClick();
					but3.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.EightThreeFive:
					but8.PerformClick();
					but3.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.EightThreeSix:
					but8.PerformClick();
					but3.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.EightThreeSeven:
					but8.PerformClick();
					but3.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.EightThreeEight:
					but8.PerformClick();
					but3.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.EightThreeNine:
					but8.PerformClick();
					but3.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.EightFourZero:
					but8.PerformClick();
					but4.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.EightFourOne:
					but8.PerformClick();
					but4.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.EightFourTwo:
					but8.PerformClick();
					but4.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.EightFourThree:
					but8.PerformClick();
					but4.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.EightFourFour:
					but8.PerformClick();
					but4.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.EightFourFive:
					but8.PerformClick();
					but4.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.EightFourSix:
					but8.PerformClick();
					but4.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.EightFourSeven:
					but8.PerformClick();
					but4.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.EightFourEight:
					but8.PerformClick();
					but4.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.EightFourNine:
					but8.PerformClick();
					but4.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.EightFiveZero:
					but8.PerformClick();
					but5.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.EightFiveOne:
					but8.PerformClick();
					but5.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.EightFiveTwo:
					but8.PerformClick();
					but5.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.EightFiveThree:
					but8.PerformClick();
					but5.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.EightFiveFour:
					but8.PerformClick();
					but5.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.EightFiveFive:
					but8.PerformClick();
					but5.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.EightFiveSix:
					but8.PerformClick();
					but5.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.EightFiveSeven:
					but8.PerformClick();
					but5.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.EightFiveEight:
					but8.PerformClick();
					but5.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.EightFiveNine:
					but8.PerformClick();
					but5.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.EightSixZero:
					but8.PerformClick();
					but6.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.EightSixOne:
					but8.PerformClick();
					but6.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.EightSixTwo:
					but8.PerformClick();
					but6.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.EightSixThree:
					but8.PerformClick();
					but6.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.EightSixFour:
					but8.PerformClick();
					but6.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.EightSixFive:
					but8.PerformClick();
					but6.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.EightSixSix:
					but8.PerformClick();
					but6.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.EightSixSeven:
					but8.PerformClick();
					but6.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.EightSixEight:
					but8.PerformClick();
					but6.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.EightSixNine:
					but8.PerformClick();
					but6.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.EightSevenZero:
					but8.PerformClick();
					but7.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.EightSevenOne:
					but8.PerformClick();
					but7.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.EightSevenTwo:
					but8.PerformClick();
					but7.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.EightSevenThree:
					but8.PerformClick();
					but7.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.EightSevenFour:
					but8.PerformClick();
					but7.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.EightSevenFive:
					but8.PerformClick();
					but7.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.EightSevenSix:
					but8.PerformClick();
					but7.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.EightSevenSeven:
					but8.PerformClick();
					but7.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.EightSevenEight:
					but8.PerformClick();
					but7.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.EightSevenNine:
					but8.PerformClick();
					but7.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.EightEightZero:
					but8.PerformClick();
					but8.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.EightEightOne:
					but8.PerformClick();
					but8.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.EightEightTwo:
					but8.PerformClick();
					but8.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.EightEightThree:
					but8.PerformClick();
					but8.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.EightEightFour:
					but8.PerformClick();
					but8.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.EightEightFive:
					but8.PerformClick();
					but8.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.EightEightSix:
					but8.PerformClick();
					but8.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.EightEightSeven:
					but8.PerformClick();
					but8.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.EightEightEight:
					but8.PerformClick();
					but8.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.EightEightNine:
					but8.PerformClick();
					but8.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.EightNineZero:
					but8.PerformClick();
					but9.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.EightNineOne:
					but8.PerformClick();
					but9.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.EightNineTwo:
					but8.PerformClick();
					but9.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.EightNineThree:
					but8.PerformClick();
					but9.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.EightNineFour:
					but8.PerformClick();
					but9.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.EightNineFive:
					but8.PerformClick();
					but9.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.EightNineSix:
					but8.PerformClick();
					but9.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.EightNineSeven:
					but8.PerformClick();
					but9.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.EightNineEight:
					but8.PerformClick();
					but9.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.EightNineNine:
					but8.PerformClick();
					but9.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.NineZeroZero:
					but9.PerformClick();
					but0.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.NineZeroOne:
					but9.PerformClick();
					but0.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.NineZeroTwo:
					but9.PerformClick();
					but0.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.NineZeroThree:
					but9.PerformClick();
					but0.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.NineZeroFour:
					but9.PerformClick();
					but0.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.NineZeroFive:
					but9.PerformClick();
					but0.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.NineZeroSix:
					but9.PerformClick();
					but0.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.NineZeroSeven:
					but9.PerformClick();
					but0.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.NineZeroEight:
					but9.PerformClick();
					but0.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.NineZeroNine:
					but9.PerformClick();
					but0.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.NineOneZero:
					but9.PerformClick();
					but1.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.NineOneOne:
					but9.PerformClick();
					but1.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.NineOneTwo:
					but9.PerformClick();
					but1.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.NineOneThree:
					but9.PerformClick();
					but1.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.NineOneFour:
					but9.PerformClick();
					but1.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.NineOneFive:
					but9.PerformClick();
					but1.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.NineOneSix:
					but9.PerformClick();
					but1.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.NineOneSeven:
					but9.PerformClick();
					but1.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.NineOneEight:
					but9.PerformClick();
					but1.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.NineOneNine:
					but9.PerformClick();
					but1.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.NineTwoZero:
					but9.PerformClick();
					but2.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.NineTwoOne:
					but9.PerformClick();
					but2.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.NineTwoTwo:
					but9.PerformClick();
					but2.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.NineTwoThree:
					but9.PerformClick();
					but2.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.NineTwoFour:
					but9.PerformClick();
					but2.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.NineTwoFive:
					but9.PerformClick();
					but2.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.NineTwoSix:
					but9.PerformClick();
					but2.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.NineTwoSeven:
					but9.PerformClick();
					but2.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.NineTwoEight:
					but9.PerformClick();
					but2.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.NineTwoNine:
					but9.PerformClick();
					but2.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.NineThreeZero:
					but9.PerformClick();
					but3.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.NineThreeOne:
					but9.PerformClick();
					but3.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.NineThreeTwo:
					but9.PerformClick();
					but3.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.NineThreeThree:
					but9.PerformClick();
					but3.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.NineThreeFour:
					but9.PerformClick();
					but3.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.NineThreeFive:
					but9.PerformClick();
					but3.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.NineThreeSix:
					but9.PerformClick();
					but3.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.NineThreeSeven:
					but9.PerformClick();
					but3.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.NineThreeEight:
					but9.PerformClick();
					but3.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.NineThreeNine:
					but9.PerformClick();
					but3.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.NineFourZero:
					but9.PerformClick();
					but4.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.NineFourOne:
					but9.PerformClick();
					but4.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.NineFourTwo:
					but9.PerformClick();
					but4.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.NineFourThree:
					but9.PerformClick();
					but4.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.NineFourFour:
					but9.PerformClick();
					but4.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.NineFourFive:
					but9.PerformClick();
					but4.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.NineFourSix:
					but9.PerformClick();
					but4.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.NineFourSeven:
					but9.PerformClick();
					but4.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.NineFourEight:
					but9.PerformClick();
					but4.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.NineFourNine:
					but9.PerformClick();
					but4.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.NineFiveZero:
					but9.PerformClick();
					but5.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.NineFiveOne:
					but9.PerformClick();
					but5.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.NineFiveTwo:
					but9.PerformClick();
					but5.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.NineFiveThree:
					but9.PerformClick();
					but5.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.NineFiveFour:
					but9.PerformClick();
					but5.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.NineFiveFive:
					but9.PerformClick();
					but5.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.NineFiveSix:
					but9.PerformClick();
					but5.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.NineFiveSeven:
					but9.PerformClick();
					but5.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.NineFiveEight:
					but9.PerformClick();
					but5.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.NineFiveNine:
					but9.PerformClick();
					but5.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.NineSixZero:
					but9.PerformClick();
					but6.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.NineSixOne:
					but9.PerformClick();
					but6.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.NineSixTwo:
					but9.PerformClick();
					but6.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.NineSixThree:
					but9.PerformClick();
					but6.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.NineSixFour:
					but9.PerformClick();
					but6.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.NineSixFive:
					but9.PerformClick();
					but6.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.NineSixSix:
					but9.PerformClick();
					but6.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.NineSixSeven:
					but9.PerformClick();
					but6.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.NineSixEight:
					but9.PerformClick();
					but6.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.NineSixNine:
					but9.PerformClick();
					but6.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.NineSevenZero:
					but9.PerformClick();
					but7.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.NineSevenOne:
					but9.PerformClick();
					but7.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.NineSevenTwo:
					but9.PerformClick();
					but7.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.NineSevenThree:
					but9.PerformClick();
					but7.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.NineSevenFour:
					but9.PerformClick();
					but7.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.NineSevenFive:
					but9.PerformClick();
					but7.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.NineSevenSix:
					but9.PerformClick();
					but7.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.NineSevenSeven:
					but9.PerformClick();
					but7.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.NineSevenEight:
					but9.PerformClick();
					but7.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.NineSevenNine:
					but9.PerformClick();
					but7.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.NineEightZero:
					but9.PerformClick();
					but8.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.NineEightOne:
					but9.PerformClick();
					but8.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.NineEightTwo:
					but9.PerformClick();
					but8.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.NineEightThree:
					but9.PerformClick();
					but8.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.NineEightFour:
					but9.PerformClick();
					but8.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.NineEightFive:
					but9.PerformClick();
					but8.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.NineEightSix:
					but9.PerformClick();
					but8.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.NineEightSeven:
					but9.PerformClick();
					but8.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.NineEightEight:
					but9.PerformClick();
					but8.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.NineEightNine:
					but9.PerformClick();
					but8.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.NineNineZero:
					but9.PerformClick();
					but9.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.NineNineOne:
					but9.PerformClick();
					but9.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.NineNineTwo:
					but9.PerformClick();
					but9.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.NineNineThree:
					but9.PerformClick();
					but9.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.NineNineFour:
					but9.PerformClick();
					but9.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.NineNineFive:
					but9.PerformClick();
					but9.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.NineNineSix:
					but9.PerformClick();
					but9.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.NineNineSeven:
					but9.PerformClick();
					but9.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.NineNineEight:
					but9.PerformClick();
					but9.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.NineNineNine:
					but9.PerformClick();
					but9.PerformClick();
					but9.PerformClick();
					break;
				#endregion Hard-Coded Triplets
				#region Hard-Coded Doubles
				case VoiceCommandAction.ZeroZero:
					but0.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ZeroOne:
					but0.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ZeroTwo:
					but0.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ZeroThree:
					but0.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ZeroFour:
					but0.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ZeroFive:
					but0.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ZeroSix:
					but0.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ZeroSeven:
					but0.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ZeroEight:
					but0.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ZeroNine:
					but0.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.OneZero:
					but1.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.OneOne:
					but1.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.OneTwo:
					but1.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.OneThree:
					but1.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.OneFour:
					but1.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.OneFive:
					but1.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.OneSix:
					but1.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.OneSeven:
					but1.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.OneEight:
					but1.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.OneNine:
					but1.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.TwoZero:
					but2.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.TwoOne:
					but2.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.TwoTwo:
					but2.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.TwoThree:
					but2.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.TwoFour:
					but2.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.TwoFive:
					but2.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.TwoSix:
					but2.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.TwoSeven:
					but2.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.TwoEight:
					but2.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.TwoNine:
					but2.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ThreeZero:
					but3.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.ThreeOne:
					but3.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.ThreeTwo:
					but3.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.ThreeThree:
					but3.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ThreeFour:
					but3.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ThreeFive:
					but3.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ThreeSix:
					but3.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.ThreeSeven:
					but3.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.ThreeEight:
					but3.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.ThreeNine:
					but3.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FourZero:
					but4.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FourOne:
					but4.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FourTwo:
					but4.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FourThree:
					but4.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FourFour:
					but4.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FourFive:
					but4.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FourSix:
					but4.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FourSeven:
					but4.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FourEight:
					but4.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FourNine:
					but4.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.FiveZero:
					but5.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.FiveOne:
					but5.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.FiveTwo:
					but5.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FiveThree:
					but5.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FiveFour:
					but5.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FiveFive:
					but5.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FiveSix:
					but5.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.FiveSeven:
					but5.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.FiveEight:
					but5.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.FiveNine:
					but5.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SixZero:
					but6.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SixOne:
					but6.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SixTwo:
					but6.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SixThree:
					but6.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SixFour:
					but6.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SixFive:
					but6.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SixSix:
					but6.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SixSeven:
					but6.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SixEight:
					but6.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SixNine:
					but6.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.SevenZero:
					but7.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.SevenOne:
					but7.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.SevenTwo:
					but7.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.SevenThree:
					but7.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.SevenFour:
					but7.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.SevenFive:
					but7.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.SevenSix:
					but7.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.SevenSeven:
					but7.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.SevenEight:
					but7.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.SevenNine:
					but7.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.EightZero:
					but8.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.EightOne:
					but8.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.EightTwo:
					but8.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.EightThree:
					but8.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.EightFour:
					but8.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.EightFive:
					but8.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.EightSix:
					but8.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.EightSeven:
					but8.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.EightEight:
					but8.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.EightNine:
					but8.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.NineZero:
					but9.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.NineOne:
					but9.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.NineTwo:
					but9.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.NineThree:
					but9.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.NineFour:
					but9.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.NineFive:
					but9.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.NineSix:
					but9.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.NineSeven:
					but9.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.NineEight:
					but9.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.NineNine:
					but9.PerformClick();
					but9.PerformClick();
					break;
				#endregion Hard-Coded Doubles
				#endregion Probing Depths
				#region Misc Buttons/Checkboxes
				case VoiceCommandAction.Triplets:
					checkThree.Checked=(!checkThree.Checked);
					gridP.ThreeAtATime=checkThree.Checked;
					break;
				case VoiceCommandAction.CheckTriplets:
					checkThree.Checked=true;
					gridP.ThreeAtATime=true;
					break;
				case VoiceCommandAction.UncheckTriplets:
					checkThree.Checked=false;
					gridP.ThreeAtATime=false;
					break;
				case VoiceCommandAction.Bleeding:
					butBleed.PerformClick();
					break;
				case VoiceCommandAction.Plaque:
					butPlaque.PerformClick();
					break;
				case VoiceCommandAction.Calculus:
					butCalculus.PerformClick();
					break;
				case VoiceCommandAction.Suppuration:
					butPus.PerformClick();
					break;
				#endregion Misc Buttons/Checkboxes
				#region Navigation Keys
				case VoiceCommandAction.Backspace:
					SendKeys.Send("{BACKSPACE}");
					break;
				case VoiceCommandAction.Right:
					SendKeys.Send("{RIGHT}");
					break;
				case VoiceCommandAction.Left:
					SendKeys.Send("{LEFT}");
					break;
				case VoiceCommandAction.Delete:
					SendKeys.Send("{DELETE}");
					break;
				#endregion Navigation Keys
				#region Go To Tooth
				case VoiceCommandAction.GoToToothOneFacial:
					GoToTooth(1,true);
					break;
				case VoiceCommandAction.GoToToothTwoFacial:
					GoToTooth(2,true);
					break;
				case VoiceCommandAction.GoToToothThreeFacial:
					GoToTooth(3,true);
					break;
				case VoiceCommandAction.GoToToothFourFacial:
					GoToTooth(4,true);
					break;
				case VoiceCommandAction.GoToToothFiveFacial:
					GoToTooth(5,true);
					break;
				case VoiceCommandAction.GoToToothSixFacial:
					GoToTooth(6,true);
					break;
				case VoiceCommandAction.GoToToothSevenFacial:
					GoToTooth(7,true);
					break;
				case VoiceCommandAction.GoToToothEightFacial:
					GoToTooth(8,true);
					break;
				case VoiceCommandAction.GoToToothNineFacial:
					GoToTooth(9,true);
					break;
				case VoiceCommandAction.GoToToothTenFacial:
					GoToTooth(10,true);
					break;
				case VoiceCommandAction.GoToToothElevenFacial:
					GoToTooth(11,true);
					break;
				case VoiceCommandAction.GoToToothTwelveFacial:
					GoToTooth(12,true);
					break;
				case VoiceCommandAction.GoToToothThirteenFacial:
					GoToTooth(13,true);
					break;
				case VoiceCommandAction.GoToToothFourteenFacial:
					GoToTooth(14,true);
					break;
				case VoiceCommandAction.GoToToothFifteenFacial:
					GoToTooth(15,true);
					break;
				case VoiceCommandAction.GoToToothSixteenFacial:
					GoToTooth(16,true);
					break;
				case VoiceCommandAction.GoToToothSeventeenFacial:
					GoToTooth(17,true);
					break;
				case VoiceCommandAction.GoToToothEighteenFacial:
					GoToTooth(18,true);
					break;
				case VoiceCommandAction.GoToToothNineteenFacial:
					GoToTooth(19,true);
					break;
				case VoiceCommandAction.GoToToothTwentyFacial:
					GoToTooth(20,true);
					break;
				case VoiceCommandAction.GoToToothTwentyOneFacial:
					GoToTooth(21,true);
					break;
				case VoiceCommandAction.GoToToothTwentyTwoFacial:
					GoToTooth(22,true);
					break;
				case VoiceCommandAction.GoToToothTwentyThreeFacial:
					GoToTooth(23,true);
					break;
				case VoiceCommandAction.GoToToothTwentyFourFacial:
					GoToTooth(24,true);
					break;
				case VoiceCommandAction.GoToToothTwentyFiveFacial:
					GoToTooth(25,true);
					break;
				case VoiceCommandAction.GoToToothTwentySixFacial:
					GoToTooth(26,true);
					break;
				case VoiceCommandAction.GoToToothTwentySevenFacial:
					GoToTooth(27,true);
					break;
				case VoiceCommandAction.GoToToothTwentyEightFacial:
					GoToTooth(28,true);
					break;
				case VoiceCommandAction.GoToToothTwentyNineFacial:
					GoToTooth(29,true);
					break;
				case VoiceCommandAction.GoToToothThirtyFacial:
					GoToTooth(30,true);
					break;
				case VoiceCommandAction.GoToToothThirtyOneFacial:
					GoToTooth(31,true);
					break;
				case VoiceCommandAction.GoToToothThirtyTwoFacial:
					GoToTooth(32,true);
					break;
				case VoiceCommandAction.GoToToothOneLingual:
					GoToTooth(1,false);
					break;
				case VoiceCommandAction.GoToToothTwoLingual:
					GoToTooth(2,false);
					break;
				case VoiceCommandAction.GoToToothThreeLingual:
					GoToTooth(3,false);
					break;
				case VoiceCommandAction.GoToToothFourLingual:
					GoToTooth(4,false);
					break;
				case VoiceCommandAction.GoToToothFiveLingual:
					GoToTooth(5,false);
					break;
				case VoiceCommandAction.GoToToothSixLingual:
					GoToTooth(6,false);
					break;
				case VoiceCommandAction.GoToToothSevenLingual:
					GoToTooth(7,false);
					break;
				case VoiceCommandAction.GoToToothEightLingual:
					GoToTooth(8,false);
					break;
				case VoiceCommandAction.GoToToothNineLingual:
					GoToTooth(9,false);
					break;
				case VoiceCommandAction.GoToToothTenLingual:
					GoToTooth(10,false);
					break;
				case VoiceCommandAction.GoToToothElevenLingual:
					GoToTooth(11,false);
					break;
				case VoiceCommandAction.GoToToothTwelveLingual:
					GoToTooth(12,false);
					break;
				case VoiceCommandAction.GoToToothThirteenLingual:
					GoToTooth(13,false);
					break;
				case VoiceCommandAction.GoToToothFourteenLingual:
					GoToTooth(14,false);
					break;
				case VoiceCommandAction.GoToToothFifteenLingual:
					GoToTooth(15,false);
					break;
				case VoiceCommandAction.GoToToothSixteenLingual:
					GoToTooth(16,false);
					break;
				case VoiceCommandAction.GoToToothSeventeenLingual:
					GoToTooth(17,false);
					break;
				case VoiceCommandAction.GoToToothEighteenLingual:
					GoToTooth(18,false);
					break;
				case VoiceCommandAction.GoToToothNineteenLingual:
					GoToTooth(19,false);
					break;
				case VoiceCommandAction.GoToToothTwentyLingual:
					GoToTooth(20,false);
					break;
				case VoiceCommandAction.GoToToothTwentyOneLingual:
					GoToTooth(21,false);
					break;
				case VoiceCommandAction.GoToToothTwentyTwoLingual:
					GoToTooth(22,false);
					break;
				case VoiceCommandAction.GoToToothTwentyThreeLingual:
					GoToTooth(23,false);
					break;
				case VoiceCommandAction.GoToToothTwentyFourLingual:
					GoToTooth(24,false);
					break;
				case VoiceCommandAction.GoToToothTwentyFiveLingual:
					GoToTooth(25,false);
					break;
				case VoiceCommandAction.GoToToothTwentySixLingual:
					GoToTooth(26,false);
					break;
				case VoiceCommandAction.GoToToothTwentySevenLingual:
					GoToTooth(27,false);
					break;
				case VoiceCommandAction.GoToToothTwentyEightLingual:
					GoToTooth(28,false);
					break;
				case VoiceCommandAction.GoToToothTwentyNineLingual:
					GoToTooth(29,false);
					break;
				case VoiceCommandAction.GoToToothThirtyLingual:
					GoToTooth(30,false);
					break;
				case VoiceCommandAction.GoToToothThirtyOneLingual:
					GoToTooth(31,false);
					break;
				case VoiceCommandAction.GoToToothThirtyTwoLingual:
					GoToTooth(32,false);
					break;
				#endregion Go To Tooth
				#region Skip Tooth
				case VoiceCommandAction.SkipToothOne:
					e.Command.DoSayResponse=SkipTooth(1);
					break;
				case VoiceCommandAction.SkipToothTwo:
					e.Command.DoSayResponse=SkipTooth(2);
					break;
				case VoiceCommandAction.SkipToothThree:
					e.Command.DoSayResponse=SkipTooth(3);
					break;
				case VoiceCommandAction.SkipToothFour:
					e.Command.DoSayResponse=SkipTooth(4);
					break;
				case VoiceCommandAction.SkipToothFive:
					e.Command.DoSayResponse=SkipTooth(5);
					break;
				case VoiceCommandAction.SkipToothSix:
					e.Command.DoSayResponse=SkipTooth(6);
					break;
				case VoiceCommandAction.SkipToothSeven:
					e.Command.DoSayResponse=SkipTooth(7);
					break;
				case VoiceCommandAction.SkipToothEight:
					e.Command.DoSayResponse=SkipTooth(8);
					break;
				case VoiceCommandAction.SkipToothNine:
					e.Command.DoSayResponse=SkipTooth(9);
					break;
				case VoiceCommandAction.SkipToothTen:
					e.Command.DoSayResponse=SkipTooth(10);
					break;
				case VoiceCommandAction.SkipToothEleven:
					e.Command.DoSayResponse=SkipTooth(11);
					break;
				case VoiceCommandAction.SkipToothTwelve:
					e.Command.DoSayResponse=SkipTooth(12);
					break;
				case VoiceCommandAction.SkipToothThirteen:
					e.Command.DoSayResponse=SkipTooth(13);
					break;
				case VoiceCommandAction.SkipToothFourteen:
					e.Command.DoSayResponse=SkipTooth(14);
					break;
				case VoiceCommandAction.SkipToothFifteen:
					e.Command.DoSayResponse=SkipTooth(15);
					break;
				case VoiceCommandAction.SkipToothSixteen:
					e.Command.DoSayResponse=SkipTooth(16);
					break;
				case VoiceCommandAction.SkipToothSeventeen:
					e.Command.DoSayResponse=SkipTooth(17);
					break;
				case VoiceCommandAction.SkipToothEighteen:
					e.Command.DoSayResponse=SkipTooth(18);
					break;
				case VoiceCommandAction.SkipToothNineteen:
					e.Command.DoSayResponse=SkipTooth(19);
					break;
				case VoiceCommandAction.SkipToothTwenty:
					e.Command.DoSayResponse=SkipTooth(20);
					break;
				case VoiceCommandAction.SkipToothTwentyOne:
					e.Command.DoSayResponse=SkipTooth(21);
					break;
				case VoiceCommandAction.SkipToothTwentyTwo:
					e.Command.DoSayResponse=SkipTooth(22);
					break;
				case VoiceCommandAction.SkipToothTwentyThree:
					e.Command.DoSayResponse=SkipTooth(23);
					break;
				case VoiceCommandAction.SkipToothTwentyFour:
					e.Command.DoSayResponse=SkipTooth(24);
					break;
				case VoiceCommandAction.SkipToothTwentyFive:
					e.Command.DoSayResponse=SkipTooth(25);
					break;
				case VoiceCommandAction.SkipToothTwentySix:
					e.Command.DoSayResponse=SkipTooth(26);
					break;
				case VoiceCommandAction.SkipToothTwentySeven:
					e.Command.DoSayResponse=SkipTooth(27);
					break;
				case VoiceCommandAction.SkipToothTwentyEight:
					e.Command.DoSayResponse=SkipTooth(28);
					break;
				case VoiceCommandAction.SkipToothTwentyNine:
					e.Command.DoSayResponse=SkipTooth(29);
					break;
				case VoiceCommandAction.SkipToothThirty:
					e.Command.DoSayResponse=SkipTooth(30);
					break;
				case VoiceCommandAction.SkipToothThirtyOne:
					e.Command.DoSayResponse=SkipTooth(31);
					break;
				case VoiceCommandAction.SkipToothThirtyTwo:
					e.Command.DoSayResponse=SkipTooth(32);
					break;
				case VoiceCommandAction.SkipCurrentTooth:
					e.Command.DoSayResponse=SkipTooth(_curLocation.ToothNum);
					break;
				#endregion Skip Tooth
				#region Position
				case VoiceCommandAction.Probing:
					GoToProbing();
					break;
				case VoiceCommandAction.MucoGingivalJunction:
					GoToMGJ();
					break;
				case VoiceCommandAction.GingivalMargin:
					GoToGingivalMargin();
					break;
				case VoiceCommandAction.Furcation:
					GoToFurcation();
					break;
				case VoiceCommandAction.Mobility:
					GoToMobility();
					break;
				#endregion Position
				#region Pluses
				case VoiceCommandAction.PlusOne:
					gridP.GingMargPlus=true;
					but1.PerformClick();
					gridP.GingMargPlus=false;
					break;
				case VoiceCommandAction.PlusTwo:
					gridP.GingMargPlus=true;
					but2.PerformClick();
					gridP.GingMargPlus=false;
					break;
				case VoiceCommandAction.PlusThree:
					gridP.GingMargPlus=true;
					but3.PerformClick();
					gridP.GingMargPlus=false;
					break;
				case VoiceCommandAction.PlusFour:
					gridP.GingMargPlus=true;
					but4.PerformClick();
					gridP.GingMargPlus=false;
					break;
				case VoiceCommandAction.PlusFive:
					gridP.GingMargPlus=true;
					but5.PerformClick();
					gridP.GingMargPlus=false;
					break;
				case VoiceCommandAction.PlusSix:
					gridP.GingMargPlus=true;
					but6.PerformClick();
					gridP.GingMargPlus=false;
					break;
				case VoiceCommandAction.PlusSeven:
					gridP.GingMargPlus=true;
					but7.PerformClick();
					gridP.GingMargPlus=false;
					break;
				case VoiceCommandAction.PlusEight:
					gridP.GingMargPlus=true;
					but8.PerformClick();
					gridP.GingMargPlus=false;
					break;
				case VoiceCommandAction.PlusNine:
					gridP.GingMargPlus=true;
					but9.PerformClick();
					gridP.GingMargPlus=false;
					break;
				#endregion Pluses
				default:
					return;//Do nothing for any other command
			}
			if(e.Command.DoSayResponse) {
				_voiceController.SayResponseAsync(e.Command.Response);
			}
			_curLocation=gridP.GetCurrentCell();
			SetAutoAdvance();
			_prevLocation=_curLocation;
		}
	}
}
