using CodeBase;
using OpenDental;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace OpenDentBusiness
{
    public class CentralConnections
    {
        /// <summary>
        /// Gets all of the connection setting information from the FreeDentalConfig.xml Throws exceptions.
        /// </summary>
        public static void GetChooseDatabaseConnectionSettings(out CentralConnection centralConnection, out bool noShow)
        {
            centralConnection = new CentralConnection();
            noShow = false;

            string xmlPath = ODFileUtils.CombinePaths(Application.StartupPath, "FreeDentalConfig.xml");
            #region Permission Check
            //Improvement should be made here to avoid requiring admin priv.
            //Search path should be something like this:
            //1. /home/username/.opendental/config.xml (or corresponding user path in Windows)
            //2. /etc/opendental/config.xml (or corresponding machine path in Windows) (should be default for new installs) 
            //3. Application Directory/FreeDentalConfig.xml (read-only if user is not admin)
            if (!File.Exists(xmlPath))
            {
                FileStream fs;
                try
                {
                    fs = File.Create(xmlPath);
                }
                catch (Exception)
                {
                    throw new ODException(
                        "The very first time that the program is run, it must be run as an Admin. If using Vista, right click, run as Admin.");
                }
                fs.Close();
            }
            #endregion
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(xmlPath);
                XPathNavigator Navigator = document.CreateNavigator();
                XPathNavigator nav;

                #region Nodes with No UI

                //See if there's a UseXWebTestGateway
                nav = Navigator.SelectSingleNode("//UseXWebTestGateway");
                if (nav != null)
                {
                    WebTypes.Shared.XWeb.XWebs.UseXWebTestGateway = nav.Value.ToLower() == "true";
                }

                #endregion

                #region Nodes from Choose Database Window

                #region Connection Settings Group Box
                //See if there's a DatabaseConnection
                nav = Navigator.SelectSingleNode("//DatabaseConnection");
                if (nav != null)
                {
                    //If there is a DatabaseConnection, then use it.
                    centralConnection.ServerName = nav.SelectSingleNode("ComputerName").Value;
                    centralConnection.DatabaseName = nav.SelectSingleNode("Database").Value;
                    centralConnection.MySqlUser = nav.SelectSingleNode("User").Value;
                    centralConnection.MySqlPassword = nav.SelectSingleNode("Password").Value;
                    XPathNavigator encryptedPwdNode = nav.SelectSingleNode("MySQLPassHash");
                    //If the Password node is empty, but there is a value in the MySQLPassHash node, decrypt the node value and use that instead
                    string _decryptedPwd;
                    if (centralConnection.MySqlPassword == ""
                        && encryptedPwdNode != null
                        && encryptedPwdNode.Value != ""
                        && Encryption.TryDecrypt(encryptedPwdNode.Value, out _decryptedPwd))
                    {
                        //decrypted value could be an empty string, which means they don't have a password set, so textPassword will be an empty string
                        centralConnection.MySqlPassword = _decryptedPwd;
                    }
                    XPathNavigator noshownav = nav.SelectSingleNode("NoShowOnStartup");
                    if (noshownav != null)
                    {
                        if (noshownav.Value == "True")
                        {
                            noShow = true;
                        }
                        else
                        {
                            noShow = false;
                        }
                    }
                }
                #endregion

                #region Connect to Middle Tier Group Box
                nav = Navigator.SelectSingleNode("//ServerConnection");
                if (nav != null)
                {
                    centralConnection.ServiceURI = nav.SelectSingleNode("URI").Value;
                }
                #endregion

                #endregion
            }
            catch (Exception)
            {
                //Common error: root element is missing
                centralConnection.ServerName = "localhost";
                centralConnection.DatabaseName = "opendental";
                centralConnection.MySqlUser = "root";
            }
        }

        public static string[] GetDatabases(CentralConnection centralConnection)
        {
            if (centralConnection.ServerName == "") return new string[0];

            try
            {
                // Use the one table that we know exists.
                if (centralConnection.MySqlUser == "")
                {
                    DataConnection.Configure(centralConnection.ServerName, "mysql", "root", centralConnection.MySqlPassword);
                }
                else
                {
                    DataConnection.Configure(centralConnection.ServerName, "mysql", centralConnection.MySqlUser, centralConnection.MySqlPassword);
                }

                // If this next step fails, table will simply have 0 rows
                using (DataTable table = DataConnection.GetTable("SHOW DATABASES"))
                {
                    string[] dbNames = new string[table.Rows.Count];
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        dbNames[i] = table.Rows[i][0].ToString();
                    }
                    return dbNames;
                }
            }
            catch
            {
                return new string[0];
            }
        }

        /// <summary>
        /// Throws an exception to display to the user if anything goes wrong.
        /// </summary>
        public static void TryToConnect(CentralConnection centralConnection, bool noShowOnStartup = false, bool isCommandLineArgs = false)
        {
            // Password could be plain text password from the Password field of the config file,
            // the decrypted password from the MySQLPassHash field of the config file, or password 
            // entered by the user and can be blank (empty string) in all cases
            DataConnection.Configure(centralConnection.ServerName, centralConnection.DatabaseName, centralConnection.MySqlUser, centralConnection.MySqlPassword);

            TrySaveConnectionSettings(centralConnection, noShowOnStartup, isCommandLineArgs);
        }

        /// <summary>
        /// Returns true if the connection settings were successfully saved to the FreeDentalConfig file. Otherwise, false.
        /// Set isCommandLineArgs to true in order to preserve settings within FreeDentalConfig.xml that are not command line args.
        /// E.g. the current value within the FreeDentalConfig.xml for NoShowOnStartup will be preserved instead of the value passed in.
        /// </summary>
        public static bool TrySaveConnectionSettings(CentralConnection centralConnection, bool noShowOnStartup = false, bool isCommandLineArgs = false)
        {
            try
            {
                //The parameters passed in might have misleading information (like noShowOnStartup) if they were comprised from command line arguments.
                //Non-command line settings within the FreeDentalConfig.xml need to be preserved when command line arguments are used.
                if (isCommandLineArgs)
                {
                    //Updating the freedentalconfig.xml file when connecting via command line arguments causes issues for users
                    //who prefer to have a desktop icon pointing to their main database and additional icons for other databases (popular amongst CEMT users).
                    return false;
                }
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ("    ");
                using (XmlWriter writer = XmlWriter.Create(ODFileUtils.CombinePaths(Application.StartupPath, "FreeDentalConfig.xml"), settings))
                {
                    writer.WriteStartElement("ConnectionSettings");

                    writer.WriteStartElement("DatabaseConnection");
                    writer.WriteStartElement("ComputerName");
                    writer.WriteString(centralConnection.ServerName);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Database");
                    writer.WriteString(centralConnection.DatabaseName);
                    writer.WriteEndElement();
                    writer.WriteStartElement("User");
                    writer.WriteString(centralConnection.MySqlUser);
                    writer.WriteEndElement();
                    string encryptedPwd;
                    Encryption.TryEncrypt(centralConnection.MySqlPassword, out encryptedPwd);//sets encryptedPwd ot value or null
                    writer.WriteStartElement("Password");
                    //If encryption fails, write plain text password to xml file; maintains old behavior.
                    writer.WriteString(string.IsNullOrEmpty(encryptedPwd) ? centralConnection.MySqlPassword : "");
                    writer.WriteEndElement();
                    writer.WriteStartElement("MySQLPassHash");
                    writer.WriteString(encryptedPwd ?? "");
                    writer.WriteEndElement();
                    writer.WriteStartElement("NoShowOnStartup");
                    if (noShowOnStartup)
                    {
                        writer.WriteString("True");
                    }
                    else
                    {
                        writer.WriteString("False");
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    if (WebTypes.Shared.XWeb.XWebs.UseXWebTestGateway)
                    {
                        writer.WriteStartElement("UseXWebTestGateway");
                        writer.WriteString("True");
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.Flush();
                }//using writer
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}