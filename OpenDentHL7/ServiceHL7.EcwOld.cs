/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using CodeBase;
using OpenDentBusiness;
using OpenDentBusiness.HL7;
using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace OpenDentHL7
{
    public partial class ServiceHL7 : ServiceBase
    {
        private static bool ecwOldIsReceiving;
        private Timer ecwOldTimerSend;
        private Timer ecwOldTimerReceive;
        private static string ecwOldHl7FolderIn;
        private static string ecwOldHl7FolderOut;
        private static bool ecwOldIsSending;
        private static DateTime ecwOldDateTimeOldMsgsDeleted;

        /// <summary>
        /// Indicates the standalone mode for eClinicalWorks, or the use of Mountainside.
        /// In both cases, chartNumber will be used instead of PatNum.
        /// </summary>
        private static bool ecwOldIsStandalone;

        private void EcwOldSendAndReceive()
        {
            ecwOldIsStandalone = true;
            if (Programs.UsingEcwTightOrFullMode())
            {
                ecwOldIsStandalone = false;
            }

            ecwOldHl7FolderOut = Preference.GetString(PreferenceName.HL7FolderOut);
            if (!Directory.Exists(ecwOldHl7FolderOut))
            {
                throw new ApplicationException(ecwOldHl7FolderOut + " does not exist.");
            }

            // Start polling the folder for waiting messages to import. Every 5 seconds.
            ecwOldTimerReceive = new Timer(EcwOldProcessMessages, null, 5000, 5000);
            if (ecwOldIsStandalone)
            {
                return; //do not continue with the HL7 sending code below
            }

            // Start polling the database for new HL7 messages to send. Every 1.8 seconds.
            ecwOldHl7FolderIn = Preference.GetString(PreferenceName.HL7FolderIn);
            if (!Directory.Exists(ecwOldHl7FolderIn))
            {
                throw new ApplicationException(ecwOldHl7FolderIn + " does not exist.");
            }

            ecwOldDateTimeOldMsgsDeleted = DateTime.MinValue;

            ecwOldIsSending = false;
            ecwOldTimerSend = new Timer(EcwOldSendMessages, null, 1800, 1800);
        }

        private void EcwOldProcessMessages(object stateInfo)
        {
            if (ecwOldIsReceiving) return;
            
            ecwOldIsReceiving = true;

            var existingFiles = Directory.GetFiles(ecwOldHl7FolderOut);
            for (int i = 0; i < existingFiles.Length; i++)
            {
                EcwOldProcessMessage(existingFiles[i]);
            }

            ecwOldIsReceiving = false;
        }

        private void EcwOldProcessMessage(string fileName)
        {
            string data = "";

            for (int i = 0; i < 5;)
            {
                try
                {
                    data = File.ReadAllText(fileName);

                    break;
                }
                catch { }

                Thread.Sleep(200);

                i++;
                if (i == 5)
                {
                    Log.Error("Could not read text from file due to file locking issues.");

                    return;
                }
            }

            try
            {
                var message = new MessageHL7(data);

                if (message.MsgType == MessageTypeHL7.ADT)
                {
                    // EcwADT.ProcessMessage(msg, ecwOldIsStandalone, IsVerboseLogging);

                    Log.Info("Processed ADT message");
                }
                else if (message.MsgType == MessageTypeHL7.SIU && !ecwOldIsStandalone) // Appointments don't get imported if standalone mode.
                {
                    // EcwSIU.ProcessMessage(msg, IsVerboseLogging);

                    Log.Info("Processed SUI message");
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message, exception);

                return;
            }

            try
            {
                File.Delete(fileName);
            }
            catch (Exception exception)
            {
                Log.Error("Unable to delete file " + fileName, exception);
            }
        }

        private void EcwOldSendMessages(object stateInfo)
        {
            if (ecwOldIsSending) return;
            
            try
            {
                ecwOldIsSending = true;

                var messages = HL7Msgs.GetOnePending();

                if (messages.Count > 0)
                {
                    string fileName;
                    foreach (var message in messages)
                    {
                        if (message.AptNum == 0)
                        {
                            fileName = ODFileUtils.CreateRandomFile(ecwOldHl7FolderIn, ".txt");
                        }
                        else
                        {
                            fileName = Path.Combine(ecwOldHl7FolderIn, message.AptNum.ToString() + ".txt");
                        }

                        File.WriteAllText(fileName, message.MsgText);

                        message.HL7Status = HL7MessageStatus.OutSent;

                        HL7Msgs.Update(message);
                    }
                }

                if (ecwOldDateTimeOldMsgsDeleted.Date < DateTime.Now.Date)
                {
                    Log.Info("Running DeleteOldMsgText");

                    ecwOldDateTimeOldMsgsDeleted = DateTime.Now;

                    // This function deletes if DateTStamp is less than CURDATE-INTERVAL 4 MONTH.
                    // That means it will delete message text only once a day, not time based.
                    HL7Msgs.DeleteOldMsgText(); 
                }
            }
            catch (Exception exception)
            {
                Log.Warn("Error while sending HL7 message.", exception);
            }
            finally
            {
                ecwOldIsSending = false;
            }
        }

        private void EcwOldStop()
        {
            if (ecwOldTimerSend != null)
            {
                ecwOldTimerSend.Dispose();
            }

            if (ecwOldTimerReceive != null)
            {
                ecwOldTimerReceive.Dispose();
            }
        }
    }
}
