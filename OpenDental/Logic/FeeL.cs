using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace OpenDental
{
    public class FeeL
    {
        /// <summary>
        ///     <para>
        ///         Imports fees into the database from the provided file.
        ///     </para>
        /// </summary>
        /// <param name="fileName">
        ///     Must be a tab-delimited file. Each row must have two columns. The first column must
        ///     be the procedure code and the second column must be the fee amount.
        /// </param>
        public static void ImportFees(long feeScheduleId, long clinicId, long providerId, string fileName, Form currentForm, bool showMessage = true)
        {
            currentForm.Cursor = Cursors.WaitCursor;
            ODProgress.ShowAction(() => ImportFeesWorker(fileName, currentForm, feeScheduleId, clinicId, providerId), startingMessage: "Importing fees, please wait...");
            currentForm.Cursor = Cursors.Default;

            if (showMessage)
            {
                MessageBox.Show(
                    "Fee schedule imported.", 
                    "Fees", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Logic that imports fees. Runs on a background thread while the progress bar is up.
        /// </summary>
        public static void ImportFeesWorker(string fileName, Form currentForm, long feeScheduleId, long clinicId, long providerId)
        {
            FeeCache feeCache = new FeeCache();
            List<Fee> listNewFees = feeCache.GetListFees(feeScheduleId, clinicId, providerId);
            string[] fields;
            double feeAmt;
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    fields = line.Split(new string[1] { "\t" }, StringSplitOptions.None);
                    if (fields.Length > 1)
                    {// && fields[1]!=""){//we no longer skip blank fees
                        if (fields[1] == "")
                        {
                            feeAmt = -1;//triggers deletion of existing fee, but no insert.
                        }
                        else
                        {
                            feeAmt = PIn.Double(fields[1]);
                        }
                        listNewFees = Fees.Import(fields[0], feeAmt, feeScheduleId, clinicId, providerId, listNewFees);
                    }
                    line = sr.ReadLine();
                }
            }
            feeCache.BeginTransaction();
            feeCache.RemoveFees(feeScheduleId, clinicId, providerId);
            foreach (Fee fee in listNewFees)
            {
                feeCache.Add(fee);
            }
            feeCache.SaveToDb();
        }

        ///<summary>ImportFees and ImportFeesWorker methods above are deprecated.  This is the replacement. Runs on a background thread while the progress bar is up.</summary>
        public static void ImportFees2(string fileName, long feeSchedNum, long clinicNum, long provNum)
        {
            List<Fee> listFees = Fees.GetListExact(feeSchedNum, clinicNum, provNum);
            string[] fields;
            int counter = 0;
            int lineCount = File.ReadAllLines(fileName).Length;//quick and dirty
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    fields = line.Split(new string[1] { "\t" }, StringSplitOptions.None);
                    if (fields.Length < 2)
                    {// && fields[1]!=""){//we no longer skip blank fees
                        line = sr.ReadLine();
                        continue;
                    }
                    long codeNum = ProcedureCodes.GetCodeNum(fields[0]);
                    if (codeNum == 0)
                    {
                        line = sr.ReadLine();
                        continue;
                    }
                    Fee fee = Fees.GetFee(codeNum, feeSchedNum, clinicNum, provNum, listFees);
                    string feeOldStr = "";
                    DateTime datePrevious = DateTime.MinValue;
                    if (fee != null)
                    {
                        feeOldStr = "Old Fee: " + fee.Amount.ToString("c") + ", ";
                        datePrevious = fee.SecDateTEdit;
                    }
                    if (fields[1] == "")
                    {//an empty entry will delete an existing fee, but not insert a blank override
                        if (fee == null)
                        {//nothing to do
                         //counter++;
                         //line=sr.ReadLine();
                         //continue;
                        }
                        else
                        {
                            Fees.Delete(fee);

                            SecurityLog.Write(null, SecurityLogEvents.ProcFeeEdit, 
                                $"Procedure: {fields[0]}, {feeOldStr}, " +
                                $"Fee Schedule: {FeeScheds.GetDescription(feeSchedNum)}. " +
                                $"Fee deleted using the Import button in the Fee Tools window.", 
                                codeNum, null);

                            SecurityLog.Write(null, SecurityLogEvents.LogFeeEdit, "Fee deleted", fee.FeeNum, datePrevious);
                        }
                    }
                    else
                    {
                        if (fee == null)
                        {
                            fee = new Fee
                            {
                                Amount = PIn.Double(fields[1]),
                                FeeSched = feeSchedNum,
                                CodeNum = codeNum,
                                ClinicNum = clinicNum,
                                ProvNum = provNum
                            };
                            Fees.Insert(fee);
                        }
                        else
                        {
                            fee.Amount = PIn.Double(fields[1]);
                            Fees.Update(fee);
                        }

                        SecurityLog.Write(null, SecurityLogEvents.ProcFeeEdit, 
                            $"Procedure: {fields[0]}, {feeOldStr}, New Fee: {fee.Amount.ToString("c")}, " +
                            $"Fee Schedule: {FeeScheds.GetDescription(feeSchedNum)}. " +
                            $"Fee changed using the Import button in the Fee Tools window.", 
                            codeNum, null);

                        SecurityLog.Write(null, SecurityLogEvents.LogFeeEdit, "Fee changed", fee.FeeNum, datePrevious);
                    }
                    //FeeSchedEvent.Fire(ODEventType.FeeSched,new ProgressBarHelper("Importing fees...",));
                    double percent = (double)counter * 100d / (double)lineCount;
                    counter++;
                    FeeSchedEvent.Fire(ODEventType.FeeSched, new ProgressBarHelper(
                        "Importing fees...", ((int)percent).ToString(), blockValue: (int)percent, progressStyle: ProgBarStyle.Continuous));
                    line = sr.ReadLine();
                }
            }
        }

        /// <summary>
        /// Returns true if current user can edit the given feeSched, otherwise false.
        /// Shows a MessageBox if user is not allowed to edit.
        /// </summary>
        public static bool CanEditFee(FeeSched feeSched, long provNum, long clinicNum)
        {
            if (!Fees.CanEditFee(feeSched, provNum, clinicNum, out var error))
            {
                MessageBox.Show(error);
                return false;
            }

            return true;
        }
    }
}
