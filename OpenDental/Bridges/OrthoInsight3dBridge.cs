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

namespace OpenDental.Bridges
{
    public class OrthoInsight3dBridge : CommandLineBridge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrthoInsight3dBridge"/> class.
        /// </summary>
        public OrthoInsight3dBridge() : base(
            "Ortho Insight 3D",
            "", 
            "http://www.motionview3d.com/")
        {
            RequirePatient = true;
        }

        /// <summary>
        ///     <para>
        ///         Generates the appropriate command line arguments for the specified patient.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient.</param>
        /// <param name="arguments">The command line arguments to pass to the program.</param>
        /// <returns>
        ///     True if the preparation was successful and the program can be started; otherwise, false.
        /// </returns>
        protected override bool PrepareToRun(long programId, Patient patient, out string arguments)
        {
            string Tidy(string input) => input.Replace(";", "").Replace(" ", "");

            arguments = " -TOOLBAR " +
                "\"" + Tidy(GetPatientId(programId, patient)) + "\" " +
                "\"-DateNeutralFormat=mm/dd/yyyy\" " +
                "\"-FirstName=" + Tidy(patient.FName) + "\" " +
                "\"-LastName=" + Tidy(patient.LName) + "\" ";

            if (!string.IsNullOrEmpty(patient.MiddleI))
                arguments += "\"-MiddleName=" + Tidy(patient.MiddleI) + "\" ";

            if (!string.IsNullOrEmpty(patient.Title))
                arguments += "\"-Title=" + Tidy(patient.Title) + "\" ";

            if (!string.IsNullOrEmpty(patient.Preferred))
                arguments += "\"-PreferredName=" + Tidy(patient.Preferred) + "\" ";

            if (!string.IsNullOrEmpty(patient.SSN) && (patient.SSN.Replace("0", "").Trim() != ""))
                arguments += "\"-SocialSecurityNumber=" + patient.SSN + "\" ";

            arguments += "\"-BirthDateNeutral=" + patient.Birthdate.ToString("MM/dd/yyyy") + "\" ";

            if (patient.DateFirstVisit.Year > 1880)
                arguments += "\"-InitialVisitNeutral=" + patient.DateFirstVisit.ToString("MM/dd/yyyy") + "\" ";

            arguments += "\"-Gender=" + (patient.Gender == PatientGender.Female ? "1" : "0") + "\" ";

            if (!string.IsNullOrEmpty(patient.Address))
                arguments += "\"-Address1=" + patient.Address + "\" ";

            if (!string.IsNullOrEmpty(patient.Address2))
                arguments += "\"-Address2=" + patient.Address2 + "\" ";

            if (!string.IsNullOrEmpty(patient.City))
                arguments += "\"-City=" + Tidy(patient.City) + "\" ";

            if (!string.IsNullOrEmpty(patient.State))
                arguments += "\"-StateProvince=" + Tidy(patient.State) + "\" ";

            if (!string.IsNullOrEmpty(patient.Zip))
                arguments += "\"-ZipPostalCode=" + Tidy(patient.Zip) + "\" ";

            return true;
        }
    }
}
