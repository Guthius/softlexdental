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
        [Obsolete] public static bool RandomKeys => Preference.GetBool(PreferenceName.RandomPrimaryKeys);

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
        public static DataStorageType AtoZfolderUsed => (DataStorageType)Preference.GetInt(PreferenceName.AtoZfolderUsed);

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
                    format = "d";
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

        /// <summary>
        /// Returns true if the XCharge program is enabled and at least one clinic has online payments enabled.
        /// </summary>
        public static bool HasOnlinePaymentEnabled()
        {
            var program = Programs.GetCur(ProgramName.Xcharge);

            if (!program.Enabled)
            {
                return false;
            }

            var programPropertyList = ProgramProperties.GetForProgram(program.ProgramNum);

            return programPropertyList.Any(x => x.PropertyDesc == "IsOnlinePaymentsEnabled" && x.PropertyValue == "1");
        }

        /// <summary>
        /// Used by an outside developer.
        /// </summary>
        public static bool ContainsKey(string prefName) => Preference.Exists(prefName);

        /// <summary>
        /// Static variable used to always reflect FormOpenDental.IsTreatPlanSortByTooth. This
        /// setter should only be called in FormOpenDental.IsTreatPlanSortByTooth. This getter
        /// should only be called from the Client side when used with MiddleTier.
        /// </summary>
        public static bool IsTreatPlanSortByTooth { get; set; }

        /// <summary>
        /// Returns the path to the temporary opendental directory, temp/opendental. Also performs
        /// one-time cleanup, if necessary.  In FormOpenDental_FormClosing, the contents of
        /// temp/opendental get cleaned up.
        /// </summary>
        public static string GetTempFolderPath()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), "OpenDental");

            if (Directory.Exists(tempPath))
            {
                try
                {
                    Directory.CreateDirectory(tempPath);
                }
                catch { }
            }

            return tempPath;
        }

        /// <summary>
        /// Creates a new randomly named file in the given directory path with the given extension and returns the full path to the new file.
        /// </summary>
        public static string GetRandomTempFile(string ext) => ODFileUtils.CreateRandomFile(GetTempFolderPath(), ext);

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
        /// Returns true if the office has a report server set up.
        /// </summary>
        [Obsolete] public static bool HasReportServer
        {
            get
            {
                return !string.IsNullOrEmpty(ReportingServer.Server) || !string.IsNullOrEmpty(ReportingServer.URI);
            }
        }

        /// <summary>
        /// A helper class to get Reporting Server preferences.
        /// </summary>
        [Obsolete]
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
                            return "Remote Server";
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
        }
    }
}