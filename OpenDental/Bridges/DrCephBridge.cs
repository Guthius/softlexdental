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
using OpenDentBusiness;
using OpenDentBusiness.Bridges;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class DrCephBridge : Bridge
    {
        private static readonly BridgePreference[] preferences =
        {
            BridgePreference.ProgramPath
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="DrCephBridge"/> class.
        /// </summary>
        public DrCephBridge() : base("Dr. Ceph", "", "http://www.fyitek.com/", preferences)
        {
        }

        /// <summary>
        ///     <para>
        ///         Sends the specified <paramref name="patient"/> data to the remote program or 
        ///         service.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient details.</param>
        public override void Send(long programId, Patient patient)
        {
            if (patient == null)
            {
                MessageBox.Show(
                  "Please select a patient first.",
                  Name,
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Information);

                return;
            }

            var programPath = ProgramPreference.GetString(programId, ProgramPreferenceName.ProgramPath);
            if (Process.GetProcessesByName("DrCeph").Length == 0)
            {
                try
                {
                    Process.Start(programPath);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        exception.Message,
                        Name, 
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                Thread.Sleep(TimeSpan.FromSeconds(4));
            }

            try
            {
                PatientRaceOld raceOld = PatientRaces.GetPatientRaceOldFromPatientRaces(patient.PatNum);
                List<RefAttach> referalList = RefAttaches.Refresh(patient.PatNum);

                var provider = Providers.GetProv(Patients.GetProvNum(patient));
                var providerName = provider.FName + " " + provider.MI + " " + provider.LName + " " + provider.Suffix;

                var family = Patients.GetFamily(patient.PatNum);
                var guardian = family.Members[0];

                string relationship = "Unknown";
                if (guardian.PatNum == patient.PatNum)
                {
                    relationship = "Self";
                }
                else if (guardian.Gender == PatientGender.Male && patient.Position == PatientPosition.Child)
                {
                    relationship = "Father";
                }
                else if (guardian.Gender == PatientGender.Female && patient.Position == PatientPosition.Child)
                {
                    relationship = "Mother";
                }

                // TODO: For some reason DrCephNew is in a closed source class, we need to figure out how it works
                //       and move the Launch method here...

                VBbridges.DrCephNew.Launch(
                    GetPatientId(programId, patient), 
                    patient.FName, 
                    patient.MiddleI, 
                    patient.LName,
                    patient.Address, 
                    patient.Address2, 
                    patient.City,
                    patient.State,
                    patient.Zip,
                    patient.HmPhone,
                    patient.SSN, 
                    patient.Gender.ToString(), 
                    raceOld.ToString(), 
                    "",
                    patient.Birthdate.ToString(),
                    DateTime.Today.ToShortDateString(), 
                    RefAttachL.GetReferringDr(referalList), 
                    providerName,
                    guardian.GetNameFL(), 
                    guardian.Address, 
                    guardian.Address2,
                    guardian.City, 
                    guardian.State,
                    guardian.Zip,
                    guardian.HmPhone, 
                    relationship);
            }
            catch
            {
                MessageBox.Show(
                    "DrCeph not responding. It might not be installed properly.",
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
