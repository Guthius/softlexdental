using CodeBase;
using OpenDental;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace OpenDentBusiness
{
    public static class Preferences
    {
        /// <summary>
        /// This property is just a shortcut to this pref to make typing faster. This pref is used a lot.
        /// </summary>
        public static bool RandomKeys => Preference.GetBool(PreferenceName.RandomPrimaryKeys);

        /// <summary>
        /// Logical shortcut to the ClaimPaymentNoShowZeroDate pref.  Returns 0001-01-01 if pref is disabled.
        /// </summary>
        public static DateTime DateClaimReceivedAfter
        {
            get
            {
                DateTime date = DateTime.MinValue;
                int days = Preference.GetInt(PreferenceName.ClaimPaymentNoShowZeroDate);
                if (days >= 0)
                {
                    date = DateTime.Today.AddDays(-days);
                }
                return date;
            }
        }

        /// <summary>
        /// This property is just a shortcut to this pref to make typing faster.
        /// </summary>
        public static DataStorageType AtoZfolderUsed => SIn.Enum<DataStorageType>(Preference.GetInt(PreferenceName.AtoZfolderUsed));

        /// <summary>
        /// This property returns true if the preference for clinics is on and there is at least one non-hidden clinic.
        /// </summary>
        public static bool HasClinicsEnabled => !Preference.GetBoolNoCache(PreferenceName.EasyNoClinics) && Clinics.GetCount(true) > 0;

        /// <summary>
        /// True if the practice has set a window to restrict the times that automatic communications will be sent out.
        /// </summary>
        public static bool DoRestrictAutoSendWindow
        {
            get
            {
                //Setting the auto start window equal to the auto stop window is how the restriction is removed.
                return Preference.GetDateTime(PreferenceName.AutomaticCommunicationTimeStart).TimeOfDay != Preference.GetDateTime(PreferenceName.AutomaticCommunicationTimeEnd).TimeOfDay;
            }
        }

        /// <summary>
        /// Returns a valid DateFormat for patient communications.
        /// If the current preference is invalid, returns "d" which is equivalent to .ToShortDateString()
        /// </summary>
        public static string PatientCommunicationDateFormat
        {
            get
            {
                string format = Preference.GetString(PreferenceName.PatientCommunicationDateFormat);
                try
                {
                    DateTime.Today.ToString(format);
                }
                catch
                {
                    format = "d";//Default to "d" which is equivalent to .ToShortDateString()
                }
                return format;
            }
        }




        /// <summary>
        /// Gets culture info from DB if possible, if not returns current culture.
        /// </summary>
        public static CultureInfo GetLanguageAndRegion()
        {
            var cultureInfo = CultureInfo.CurrentCulture;

            try
            {
                var preference = Preference.GetByKey("LanguageAndRegion");
                if (!string.IsNullOrEmpty(preference.Value))
                {
                    cultureInfo = CultureInfo.GetCultureInfo(preference.Value);
                }
            }
            catch { }

            return cultureInfo;
        }

        ///<summary>Returns true if the XCharge program is enabled and at least one clinic has online payments enabled.</summary>
        public static bool HasOnlinePaymentEnabled()
        {
            Program prog = Programs.GetCur(ProgramName.Xcharge);
            if (!prog.Enabled)
            {
                return false;
            }
            List<ProgramProperty> listXChargeProps = ProgramProperties.GetForProgram(prog.ProgramNum);
            return listXChargeProps.Any(x => x.PropertyDesc == "IsOnlinePaymentsEnabled" && x.PropertyValue == "1");
        }

        ///<summary>Used by an outside developer.</summary>
        public static bool ContainsKey(string prefName) => Preference.Exists(prefName);

        ///<summary>Static variable used to always reflect FormOpenDental.IsTreatPlanSortByTooth.  
        ///This setter should only be called in FormOpenDental.IsTreatPlanSortByTooth.  
        ///This getter should only be called from the Client side when used with MiddleTier.</summary>
        public static bool IsTreatPlanSortByTooth { get; set; }

        ///<summary>Returns the path to the temporary opendental directory, temp/opendental.  Also performs one-time cleanup, if necessary.  In FormOpenDental_FormClosing, the contents of temp/opendental get cleaned up.</summary>
        public static string GetTempFolderPath()
        {
            //Will clean up entire temp folder for a month after the enhancement of temp file cleanups as long as the temp\opendental folder doesn't already exist.
            string tempPathOD = ODFileUtils.CombinePaths(Path.GetTempPath(), "opendental");
            if (Directory.Exists(tempPathOD))
            {
                //Cleanup has already run for the old temp folder.  Do nothing.
                return tempPathOD;
            }
            Directory.CreateDirectory(tempPathOD);
            if (DateTime.Today > Preference.GetDate(PreferenceName.TempFolderDateFirstCleaned).AddMonths(1))
            {
                return tempPathOD;
            }
            //This might be used if this is the first time running this version on the computer that did the db update.
            //This might also be used if this is a computer that was turned off for a few weeks around the time of update conversion.
            //We need some sort of time limit just in case it's annoying and keeps happening.
            //So this will have a small risk of missing a computer, but the benefit of limiting outweighs the risk.
            //Empty entire temp folder.  Blank folders will be left behind because they do not matter.
            string[] arrayFileNames = Directory.GetFiles(Path.GetTempPath());
            for (int i = 0; i < arrayFileNames.Length; i++)
            {
                try
                {
                    if (arrayFileNames[i].Substring(arrayFileNames[i].LastIndexOf('.')) == ".exe" || arrayFileNames[i].Substring(arrayFileNames[i].LastIndexOf('.')) == ".cs")
                    {
                        //Do nothing.  We don't care about .exe or .cs files and don't want to interrupt other programs' files.
                    }
                    else
                    {
                        File.Delete(arrayFileNames[i]);
                    }
                }
                catch
                {
                    //Do nothing because the file could have been in use or there were not sufficient permissions.
                    //This file will most likely get deleted next time a temp file is created.
                }
            }
            return tempPathOD;
        }

        ///<summary>Creates a new randomly named file in the given directory path with the given extension and returns the full path to the new file.</summary>
        public static string GetRandomTempFile(string ext)
        {
            return ODFileUtils.CreateRandomFile(GetTempFolderPath(), ext);
        }

        public static long GetDefaultSheetDefNum(SheetTypeEnum sheetType)
        {
            long retVal;
            switch (sheetType)
            {
                case SheetTypeEnum.Consent:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultConsent);
                    break;
                case SheetTypeEnum.DepositSlip:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultDepositSlip);
                    break;
                case SheetTypeEnum.ExamSheet:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultExamSheet);
                    break;
                case SheetTypeEnum.LabelAppointment:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultLabelAppointment);
                    break;
                case SheetTypeEnum.LabelCarrier:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultLabelCarrier);
                    break;
                case SheetTypeEnum.LabelPatient:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultLabelPatient);
                    break;
                case SheetTypeEnum.LabelReferral:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultLabelReferral);
                    break;
                case SheetTypeEnum.LabSlip:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultLabSlip);
                    break;
                case SheetTypeEnum.MedicalHistory:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultMedicalHistory);
                    break;
                case SheetTypeEnum.MedLabResults:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultMedLabResults);
                    break;
                case SheetTypeEnum.PatientForm:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultPatientForm);
                    break;
                case SheetTypeEnum.PatientLetter:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultPatientLetter);
                    break;
                case SheetTypeEnum.PaymentPlan:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultPaymentPlan);
                    break;
                case SheetTypeEnum.ReferralLetter:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultReferralLetter);
                    break;
                case SheetTypeEnum.ReferralSlip:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultReferralSlip);
                    break;
                case SheetTypeEnum.RoutingSlip:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultRoutingSlip);
                    break;
                case SheetTypeEnum.Rx:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultRx);
                    break;
                case SheetTypeEnum.RxMulti:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultRxMulti);
                    break;
                case SheetTypeEnum.Screening:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultScreening);
                    break;
                case SheetTypeEnum.Statement:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultStatement);
                    break;
                case SheetTypeEnum.TreatmentPlan:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultTreatmentPlan);
                    break;
                case SheetTypeEnum.RxInstruction:
                    retVal = Preference.GetLong(PreferenceName.SheetsDefaultRxInstructions);
                    break;
                default:
                    throw new Exception(Lans.g("SheetDefs", "Unsupported SheetTypeEnum") + "\r\n" + sheetType.ToString());
            }
            return retVal;
        }

        /// <summary>
        /// Using Pay Plan Version 2 (Aged Credits and Debits).
        /// </summary>
        public static bool IsPayPlanVersion2 => Preference.GetInt(PreferenceName.PayPlansVersion) == (int)PayPlanVersions.AgeCreditsAndDebits;

        /// <summary>
        /// Returns true if the database hosted by Open Dental.
        /// </summary>
        public static bool IsCloudMode => Preference.GetInt(PreferenceName.DatabaseMode) == (int)DatabaseModeEnum.Cloud;

        /// <summary>
        /// Returns true if the office has a report server set up.
        /// </summary>
        public static bool HasReportServer
        {
            get
            {
                return !string.IsNullOrEmpty(ReportingServer.Server) || !string.IsNullOrEmpty(ReportingServer.URI);
            }
        }

        ///<summary>A helper class to get Reporting Server preferences.</summary>
        public static class ReportingServer
        {
            public static string DisplayStr
            {
                get
                {
                    if (Server == "")
                    {
                        if (URI != "")
                        {
                            return "Remote Server"; //will be blank if there is no reporting server set up.
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else
                    {
                        return Server + ": " + Database;
                    }
                }
            }

            public static string URI => Preference.GetString(PreferenceName.ReportingServerURI);

            public static string Server => Preference.GetString(PreferenceName.ReportingServerCompName);

            public static string Database => Preference.GetString(PreferenceName.ReportingServerDbName);
          
            public static string MySqlUser => Preference.GetString(PreferenceName.ReportingServerMySqlUser);

            public static string MySqlPass
            {
                get
                {
                    Encryption.TryDecrypt(Preference.GetString(PreferenceName.ReportingServerMySqlPassHash), out string pass);

                    return pass;
                }
            }

            public static string ConnectionString
            {
                get
                {
                    return "";//Connection string is not currently supported for ReportingServers.
                }
            }
        }
    }
}