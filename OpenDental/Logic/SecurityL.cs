using OpenDentBusiness;
using System;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public class SecurityL
    {
        /// <summary>
        /// Called to change the password for <see cref="Security.CurrentUser"/>.
        /// </summary>
        /// <param name="forceLogOff">A value indicating whether to force a log off if the user cancels out the change password dialog.</param>
        /// <param name="refreshSecurityCache"></param>
        /// <returns>True if the password was changed succesfully; otherwise, false.</returns>
        public static bool ChangePassword(bool forceLogOff)
        {
            using (var formUserPassword = new FormUserPassword(false, Security.CurrentUser.UserName))
            {
                if (formUserPassword.ShowDialog() == DialogResult.Cancel)
                {
                    if (forceLogOff)
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
                    User.UpdatePassword(
                        Security.CurrentUser, 
                        formUserPassword.LoginDetails, 
                        formUserPassword.PasswordIsStrong);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        exception.Message, 
                        Translation.Language.OpenDental, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);

                    return false;
                }

                Security.CurrentUser.PasswordIsStrong = formUserPassword.PasswordIsStrong;
                Security.CurrentUser.Password = formUserPassword.LoginDetails;
            }

            return true;
        }
    }
}