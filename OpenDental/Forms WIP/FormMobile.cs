using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;


namespace OpenDental
{
    ///<summary>THIS FORM HAS BEEN DEPRECATED!!! All functionality that previously existed in this form has been moved to FormPatientPortalSetup.</summary>
    public partial class FormMobile : ODForm
    {
        private static int BatchSize = 100;
        /////<summary>All statements of a patient are not uploaded. The limit is defined by the recent [statementLimitPerPatient] records</summary>
        //private static int statementLimitPerPatient=5;
        ///<summary>This variable prevents the synching methods from being called when a previous synch is in progress.</summary>
        private static bool IsSynching;
        ///<summary>This variable prevents multiple error message boxes from popping up if mobile synch server is not available.</summary>
        private static bool IsServerAvail = true;
        ///<summary>True if a pref was saved and the other workstations need to have their cache refreshed when this form closes.</summary>
        private bool changed;
        ///<summary>If this variable is true then records are uploaded one at a time so that an error in uploading can be traced down to a single record</summary>
        private static bool IsTroubleshootMode = false;
        private static FormProgress FormP;

        private enum SynchEntity
        {
            patient,
            appointment,
            prescription,
            provider,
            pharmacy,
            labpanel,
            labresult,
            medication,
            medicationpat,
            allergy,
            allergydef,
            disease,
            diseasedef,
            icd9,
            statement,
            document,
            recall,
            deletedobject,
            patientdel
        }

        public FormMobile()
        {
            InitializeComponent();
            
        }

        private void FormMobileSetup_Load(object sender, EventArgs e)
        {
            textMobileSyncServerURL.Text = Preference.GetString(PreferenceName.MobileSyncServerURL);
            textSynchMinutes.Text = Preference.GetInt(PreferenceName.MobileSyncIntervalMinutes).ToString();
            textDateBefore.Text = Preference.GetDate(PreferenceName.MobileExcludeApptsBeforeDate).ToShortDateString();
            textMobileSynchWorkStation.Text = Preference.GetString(PreferenceName.MobileSyncWorkstationName);
            textMobileUserName.Text = Preference.GetString(PreferenceName.MobileUserName);
            textMobilePassword.Text = "";//not stored locally, and not pulled from web server
            DateTime lastRun = Preference.GetDateTime(PreferenceName.MobileSyncDateTimeLastRun);
            if (lastRun.Year > 1880)
            {
                textDateTimeLastRun.Text = lastRun.ToShortDateString() + " " + lastRun.ToShortTimeString();
            }
            //Web server is not contacted when loading this form.  That would be too slow.
            //CreateAppointments(5);
        }

        private void butCurrentWorkstation_Click(object sender, EventArgs e)
        {
            textMobileSynchWorkStation.Text = System.Environment.MachineName.ToUpper();
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (!SavePrefs())
            {
                Cursor = Cursors.Default;
                return;
            }
            Cursor = Cursors.Default;
            MsgBox.Show(this, "Done");
        }

        ///<summary>Returns false if validation failed.  This also makes sure the web service exists, the customer is paid, and the registration key is correct.</summary>
        private bool SavePrefs()
        {
            //validation
            if (textSynchMinutes.errorProvider1.GetError(textSynchMinutes) != ""
                || textDateBefore.errorProvider1.GetError(textDateBefore) != "")
            {
                MsgBox.Show(this, "Please fix data entry errors first.");
                return false;
            }
            //yes, workstation is allowed to be blank.  That's one way for user to turn off auto synch.
            //if(textMobileSynchWorkStation.Text=="") {
            //	MsgBox.Show(this,"WorkStation cannot be empty");
            //	return false;
            //}
            // the text field is read because the keyed in values have not been saved yet
            //if(textMobileSyncServerURL.Text.Contains("192.168.0.196") || textMobileSyncServerURL.Text.Contains("localhost")) {
            if (textMobileSyncServerURL.Text.Contains("10.10.1.196") || textMobileSyncServerURL.Text.Contains("localhost"))
            {
                IgnoreCertificateErrors();// done so that TestWebServiceExists() does not thow an error.
            }
            // if this is not done then an old non-functional url prevents any new url from being saved.
            Preference.Update(PreferenceName.MobileSyncServerURL, textMobileSyncServerURL.Text);
            if (!TestWebServiceExists())
            {
                MsgBox.Show(this, "Web service not found.");
                return false;
            }

            if (!VerifyPaidCustomer())
            {
                return false;
            }
            //Minimum 10 char.  Must contain uppercase, lowercase, numbers, and symbols. Valid symbols are: !@#$%^&+= 
            //The set of symbols checked was far too small, not even including periods, commas, and parentheses.
            //So I rewrote it all.  New error messages say exactly what's wrong with it.
            if (textMobileUserName.Text != "")
            {//allowed to be blank
                if (textMobileUserName.Text.Length < 10)
                {
                    MsgBox.Show(this, "User Name must be at least 10 characters long.");
                    return false;
                }
                if (!Regex.IsMatch(textMobileUserName.Text, "[A-Z]+"))
                {
                    MsgBox.Show(this, "User Name must contain an uppercase letter.");
                    return false;
                }
                if (!Regex.IsMatch(textMobileUserName.Text, "[a-z]+"))
                {
                    MsgBox.Show(this, "User Name must contain an lowercase letter.");
                    return false;
                }
                if (!Regex.IsMatch(textMobileUserName.Text, "[0-9]+"))
                {
                    MsgBox.Show(this, "User Name must contain a number.");
                    return false;
                }
                if (!Regex.IsMatch(textMobileUserName.Text, "[^0-9a-zA-Z]+"))
                {//absolutely anything except number, lower or upper.
                    MsgBox.Show(this, "User Name must contain punctuation or symbols.");
                    return false;
                }
            }
            if (textDateBefore.Text == "")
            {//default to one year if empty
                textDateBefore.Text = DateTime.Today.AddYears(-1).ToShortDateString();
                //not going to bother informing user.  They can see it.
            }
            //save to db------------------------------------------------------------------------------------
            if (Preference.Update(PreferenceName.MobileSyncServerURL, textMobileSyncServerURL.Text)
                | Preference.Update(PreferenceName.MobileSyncIntervalMinutes, PIn.Int(textSynchMinutes.Text))//blank entry allowed
                | Preference.Update(PreferenceName.MobileExcludeApptsBeforeDate, POut.Date(PIn.Date(textDateBefore.Text), false))//blank 
                | Preference.Update(PreferenceName.MobileSyncWorkstationName, textMobileSynchWorkStation.Text)
                | Preference.Update(PreferenceName.MobileUserName, textMobileUserName.Text)
            )
            {
                changed = true;
                Preference.Refresh();
            }
            //Username and password-----------------------------------------------------------------------------
            return true;
        }

        ///<summary>Uploads Preferences to the Patient Portal /Mobile Web.</summary>
        public static void UploadPreference(PreferenceName prefname)
        {
            if (Preference.GetString(PreferenceName.RegistrationKey) == "")
            {
                return;//Prevents a bug when using the trial version with no registration key.  Practice edit, OK, was giving error.
            }
            try
            {
                if (TestWebServiceExists())
                {
                    //Prefm prefm = Prefms.GetPrefm(prefname.ToString());
                    //mb.SetPreference(Preference.GetString(PreferenceName.RegistrationKey),prefm);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//may not show if called from a thread but that does not matter - the failing of this method should not stop the  the code from proceeding.
            }
        }

        private void butDelete_Click(object sender, EventArgs e)
        {
            if (!SavePrefs())
            {
                return;
            }
            if (!MsgBox.Show(this, MsgBoxButtons.OKCancel, "Delete all your data from our server?  This happens automatically before a full synch."))
            {
                return;
            }
            //mb.DeleteAllRecords(Preference.GetString(PreferenceName.RegistrationKey));
            MsgBox.Show(this, "Done");
        }

        private void butFullSync_Click(object sender, EventArgs e)
        {
        }

        private void butSync_Click(object sender, EventArgs e)
        {
        }

        /// <summary>An empty method to test if the webservice is up and running. This was made with the intention of testing the correctness of the webservice URL. If an incorrect webservice URL is used in a background thread the exception cannot be handled easily to a point where even a correct URL cannot be keyed in by the user. Because an exception in a background thread closes the Form which spawned it.</summary>
        private static bool TestWebServiceExists()
        {
            try
            {
            }
            catch
            {
                return false;
            }
            return false;
        }

        private bool VerifyPaidCustomer()
        {
            //if(textMobileSyncServerURL.Text.Contains("192.168.0.196") || textMobileSyncServerURL.Text.Contains("localhost")) {
            if (textMobileSyncServerURL.Text.Contains("10.10.1.196") || textMobileSyncServerURL.Text.Contains("localhost"))
            {
                IgnoreCertificateErrors();
            }
            //bool isPaidCustomer = mb.IsPaidCustomer(Preference.GetString(PreferenceName.RegistrationKey));
            //if (!isPaidCustomer)
            //{
            //    textSynchMinutes.Text = "0";
            //    Preference.Update(PreferenceName.MobileSyncIntervalMinutes, 0);
            //    changed = true;
            //    MsgBox.Show(this, "This feature requires a separate monthly payment.  Please call customer support.");
            //    return false;
            //}
            return true;
        }

        #region Testing
        ///<summary>This allows the code to continue by not throwing an exception even if there is a problem with the security certificate.</summary>
        private static void IgnoreCertificateErrors()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
        }

        #endregion Testing

        private void butClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormMobile_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (changed)
            {
                DataValid.SetInvalid(InvalidType.Prefs);
            }
        }
    }
}