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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDentBusiness.Bridges;

namespace OpenDental.Bridges
{
    public class SchickBridge : Bridge
    {
        private static CDRDicom.Application cdrApplication = null;
        private static CDRDicom.ExamDocument cdrExam = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchickBridge"/> class.
        /// </summary>
        public SchickBridge() : base("Schick CDR", "")
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
                    MessageBoxIcon.Error);

                return;
            }

            var patientId = GetPatientId(programId, patient);

            try
            {
                var programVersion = ProgramPreference.GetLong(programId, "program_version", 4);
                switch (programVersion)
                {
                    case 4:
                        // TODO: Merge logic from VBbridges.Schick3 class into this class...
                        VBbridges.Schick3.Launch(patientId, patient.LName, patient.FName); 
                        break;

                    case 5:
                        ShowExam(patient, patientId);
                        break;
                }
            }
            catch
            {
                MessageBox.Show("Error launching Schick CDR Dicom.");
            }
        }

        /// <summary>
        /// Shows the exam of the specified <paramref name="patient"/>.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="patientId">The ID of the patient.</param>
        private static void ShowExam(Patient patient, string patientId)
        {
            // If the CDR application is not running yet, then getting a new instance will start the CDR application.
            // If the CDR application is already running, then getting a new instance will simply reference the existing CDR application.
            // Thus there can never be more than one CDR application running.
            cdrApplication = new CDRDicom.Application();

            CreateExamWindow();

            if (!cdrExam.LoadPatient(patient.LName, patient.FName, patientId))
            {
                // Attempt to locate an existing patient in the Schick database.				
                // If the user clicks accross with a new patient when an existing patient is already loaded into Schick, then there are only two options:
                // 1) Reuse the same CDRDicom.Application() object and pass "true" into exam.NewExam() below.  The problem with this option is that
                // 		the user must set the create patient checkbox, otherwise the exam is created for the previous patient, which is terrifying.
                // 2) Quit the application, then create a new exam with the patient create checkbox hidden.  In this specific scenario, Schick will create
                // 	the patient even though the create patient checkbox is neither showing nor allowed, because there is no previous patient currently loaded.
                // We chose option #2, because it more closely follows our typical bridge patterns.
                // Quit() causes OnExamClosed() to fire, which sets cdrApp to null so it will be reinitialized in LoadCdrApp().  Var cdrApp is still valid.
                cdrApplication.Quit();

                CreateExamWindow();
                CreateExam(patient, patientId);
            }
        }


        /// <summary>
        ///     <para>
        ///         Creates a exam window if one doesn't exist.
        ///     </para>
        /// </summary>
        private static void CreateExamWindow()
        {
            if (cdrExam == null)
            {
                cdrExam = (CDRDicom.ExamDocument)cdrApplication.CreateExamDocument();
                cdrExam.OnClose += () => cdrExam = null;
            }

            cdrExam.Visible = true;

            SetForegroundWindow(cdrExam.hWnd);
        }

        /// <summary>
        ///     <para>
        ///         When the user clicks OK from the exam window, the patient will be created. The 
        ///         user can also cancel.
        ///     </para>
        /// </summary>
        private static void CreateExam(Patient patient, string patientId)
        {
            CDRDATALib.ICDRPatient cdrPatient = (CDRDATALib.ICDRPatient)cdrExam.Patient;
            if (cdrPatient != null)
            {
                cdrPatient.LastName = patient.LName;
                cdrPatient.FirstName = patient.FName;
                cdrPatient.IDNumber = patientId;

                // Display the new exam.
                cdrExam.NewExam(false);
            }
        }

        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(int hndRef);
    }
}
