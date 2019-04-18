using OpenDentBusiness;
using System;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public class SecurityL
    {
        /// <summary>
        /// Called to change the password for <see cref="Security.CurUser"/>.
        /// </summary>
        /// <param name="isForcedLogOff">A value indicating whether to force a log off if the user cancels out the change password dialog.</param>
        /// <param name="doRefreshSecurityCache"></param>
        /// <returns>True if the password was changed succesfully; otherwise, false.</returns>
        public static bool ChangePassword(bool isForcedLogOff, bool doRefreshSecurityCache = true)
        {
            if (Security.CurUser.UserNumCEMT != 0)
            {
                MessageBox.Show(
                    Translation.Language.UseTheCEMTToolToChangeYourPassword, 
                    Translation.Language.OpenDental, 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                return false;
            }

            using (var formUserPassword = new FormUserPassword(false, Security.CurUser.UserName))
            {
                if (formUserPassword.ShowDialog() == DialogResult.Cancel)
                {
                    if (isForcedLogOff)
                    {
                        var forms = Application.OpenForms.OfType<FormOpenDental>().ToList();
                        if (forms.Count > 0)
                        {
                            forms[0].LogOffNow(true);
                        }
                        else // Theoretically there should always should be exactly 1 FormOpenDental, but just in case something went wrong and there isn't...
                        {
                            Application.Exit();
                        }
                    }
                    return false;
                }

                try
                {
                    Userods.UpdatePassword(
                        Security.CurUser, 
                        formUserPassword.LoginDetails, 
                        formUserPassword.PasswordIsStrong);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message, 
                        Translation.Language.OpenDental, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);

                    return false;
                }

                Security.CurUser.PasswordIsStrong = formUserPassword.PasswordIsStrong;
                Security.CurUser.LoginDetails = formUserPassword.LoginDetails;
                Security.PasswordTyped = formUserPassword.PasswordTyped;
            }

            if (doRefreshSecurityCache)
            {
                DataValid.SetInvalid(InvalidType.Security);
            }
            return true;
        }
    }
}