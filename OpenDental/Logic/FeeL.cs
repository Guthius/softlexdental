using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace OpenDental
{
    ///<summary></summary>
    public class FeeL {

		///<summary>Imports fees into the database from the provided file.</summary>
		///<param name="fileName">Must be a tab-delimited .xls or .csv file. Each row must have two columns. The first column must be the proc code
		///and the second column must be the fee amount.</param>
		public static void ImportFees(long feeSchedNum,long clinicNum,long provNum,string fileName,Form currentForm,bool showMessage=true) {
			currentForm.Cursor=Cursors.WaitCursor;
			ODProgress.ShowAction(() => ImportFeesWorker(fileName,currentForm,feeSchedNum,clinicNum,provNum),
				startingMessage:"Importing fees, please wait...");
			currentForm.Cursor=Cursors.Default;
			if(showMessage) {
				MsgBox.Show(currentForm,"Fee schedule imported.");
			}
		}

		///<summary>Logic that imports fees. Runs on a background thread while the progress bar is up.</summary>
		public static void ImportFeesWorker(string fileName,Form currentForm,long feeSchedNum,long clinicNum,long provNum) {
			FeeCache feeCache=new FeeCache();
			List<Fee> listNewFees=feeCache.GetListFees(feeSchedNum,clinicNum,provNum);
			string[] fields;
			double feeAmt;
			using(StreamReader sr=new StreamReader(fileName)) {
				string line=sr.ReadLine();
				while(line!=null) {
					fields=line.Split(new string[1] {"\t"},StringSplitOptions.None);
					if(fields.Length>1){// && fields[1]!=""){//we no longer skip blank fees
						if(fields[1]=="") {
							feeAmt=-1;//triggers deletion of existing fee, but no insert.
						}
						else {
							feeAmt=PIn.Double(fields[1]);
						}
						listNewFees=Fees.Import(fields[0],feeAmt,feeSchedNum,clinicNum,provNum,listNewFees);
					}
					line=sr.ReadLine();
				}
			}
			feeCache.BeginTransaction();
			feeCache.RemoveFees(feeSchedNum,clinicNum,provNum);
			foreach(Fee fee in listNewFees) {
				feeCache.Add(fee);
			}
			feeCache.SaveToDb();
		}

		///<summary>ImportFees and ImportFeesWorker methods above are deprecated.  This is the replacement. Runs on a background thread while the progress bar is up.</summary>
		public static void ImportFees2(string fileName,long feeSchedNum,long clinicNum,long provNum) {
			List<Fee> listFees=Fees.GetListExact(feeSchedNum,clinicNum,provNum);
			string[] fields;
			int counter=0;
			int lineCount=File.ReadAllLines(fileName).Length;//quick and dirty
			using(StreamReader sr=new StreamReader(fileName)) {
				string line=sr.ReadLine();
				while(line!=null) {
					fields=line.Split(new string[1] {"\t"},StringSplitOptions.None);
					if(fields.Length<2){// && fields[1]!=""){//we no longer skip blank fees
						line=sr.ReadLine();
						continue;
					}
					long codeNum = ProcedureCodes.GetCodeNum(fields[0]);
					if(codeNum==0){
						line=sr.ReadLine();
						continue;
					}
					Fee fee=Fees.GetFee(codeNum,feeSchedNum,clinicNum,provNum,listFees);
					string feeOldStr="";
					DateTime datePrevious=DateTime.MinValue;
					if(fee!=null) {
						feeOldStr="Old Fee: "+fee.Amount.ToString("c")+", ";
						datePrevious=fee.SecDateTEdit;
					}
					if(fields[1]=="") {//an empty entry will delete an existing fee, but not insert a blank override
						if(fee==null){//nothing to do
							//counter++;
							//line=sr.ReadLine();
							//continue;
						}
						else{
							//doesn't matter if the existing fee is an override or not.
							Fees.Delete(fee);
							SecurityLogs.MakeLogEntry(Permissions.ProcFeeEdit,0,"Procedure: "+fields[0]+", "
								+feeOldStr
								//+", Deleted Fee: "+fee.Amount.ToString("c")+", "
								+"Fee Schedule: "+FeeScheds.GetDescription(feeSchedNum)+". "
								+"Fee deleted using the Import button in the Fee Tools window.",codeNum,
								DateTime.MinValue);
							SecurityLogs.MakeLogEntry(Permissions.LogFeeEdit,0,"Fee deleted",fee.FeeNum,datePrevious);
						}
					}
					else {//value found
						if(fee==null){//no current fee
							fee=new Fee();
							fee.Amount=PIn.Double(fields[1]);
							fee.FeeSched=feeSchedNum;
							fee.CodeNum=codeNum;
							fee.ClinicNum=clinicNum;//Either 0 because you're importing on an HQ schedule or local clinic because the feesched is localizable.
							fee.ProvNum=provNum;
							Fees.Insert(fee);
						}
						else{
							fee.Amount=PIn.Double(fields[1]);
							Fees.Update(fee);
						}
						SecurityLogs.MakeLogEntry(Permissions.ProcFeeEdit,0,"Procedure: "+fields[0]+", "
							+feeOldStr
							+", New Fee: "+fee.Amount.ToString("c")+", "
							+"Fee Schedule: "+FeeScheds.GetDescription(feeSchedNum)+". "
							+"Fee changed using the Import button in the Fee Tools window.",codeNum,
							DateTime.MinValue);
						SecurityLogs.MakeLogEntry(Permissions.LogFeeEdit,0,"Fee changed",fee.FeeNum,datePrevious);
					}
					//FeeSchedEvent.Fire(ODEventType.FeeSched,new ProgressBarHelper("Importing fees...",));
					double percent=(double)counter*100d/(double)lineCount;
					counter++;
					FeeSchedEvent.Fire(ODEventType.FeeSched,new ProgressBarHelper(
						"Importing fees...",((int)percent).ToString(),blockValue:(int)percent,progressStyle:ProgBarStyle.Continuous));
					line=sr.ReadLine();
				}
			}
			
		}

		///<summary>Returns true if current user can edit the given feeSched, otherwise false.
		///Shows a MessageBox if user is not allowed to edit.</summary>
		public static bool CanEditFee(FeeSched feeSched,long provNum,long clinicNum) {
			string error;
			if(!Fees.CanEditFee(feeSched,provNum,clinicNum,out error)) {
				MessageBox.Show(error);
				return false;
			}
			return true;
		}
		/* doesn't seem to be used from anywhere
		///<summary>If the named fee schedule does not exist, then it will be created.  It always returns the defnum for the feesched used, regardless of whether it already existed.  procCode must have already been tested for valid code, and feeSchedName must not be blank.</summary>
		public static long ImportTrojan(string procCode,double amt,string feeSchedName) {
			FeeSched feeSched=FeeScheds.GetByExactName(feeSchedName);
			//if isManaged, then this should be done differently from here on out.
			if(feeSched==null){
				//add the new fee schedule
				feeSched=new FeeSched();
				feeSched.ItemOrder=FeeSchedC.ListLong.Count;
				feeSched.Description=feeSchedName;
				feeSched.FeeSchedType=FeeScheduleType.Normal;
				//feeSched.IsNew=true;
				FeeScheds.Insert(feeSched);
				//Cache.Refresh(InvalidType.FeeScheds);
				//Fees.Refresh();
				DataValid.SetInvalid(InvalidType.FeeScheds, InvalidType.Fees);
			}
			if(feeSched.IsHidden){
				feeSched.IsHidden=false;//unhide it
				FeeScheds.Update(feeSched);
				DataValid.SetInvalid(InvalidType.FeeScheds);
			}
			Fee fee=Fees.GetFee(ProcedureCodes.GetCodeNum(procCode),feeSched.FeeSchedNum);
			if(fee==null) {
				fee=new Fee();
				fee.Amount=amt;
				fee.FeeSched=feeSched.FeeSchedNum;
				fee.CodeNum=ProcedureCodes.GetCodeNum(procCode);
				Fees.Insert(fee);
			}
			else{
				fee.Amount=amt;
				Fees.Update(fee);
			}
			return feeSched.FeeSchedNum;
		}	*/

	}
}