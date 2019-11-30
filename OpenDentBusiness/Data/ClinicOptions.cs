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
using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// Identifies the options that are enabled for a <see cref="Clinic"/>.
    /// </summary>
    [Flags]
    public enum ClinicOptions
    {
        /// <summary>
        /// Indicates the clinic should be excluded from showing up in the Insurance Verification List.
        /// </summary>
        ExcludeFromInsuranceVerification = 1,

        /// <summary>
        /// Indicate that a procedure must be attached to controlled prescriptions written from this clinic.
        /// </summary>
        RequireProcedureOnRx = 2,

        /// <summary>
        /// Indicates the clinic is a medical clinic.
        /// </summary>
        MedicalOnly = 4,

        /// <summary>
        /// Indicates automatic reminders and confirmations are enabled for the clinic.
        /// </summary>
        AutomaticConfirmationsEnabled = 8,

        /// <summary>
        /// Indicates the clinic is using the default automated reminder and confirmation settings.
        /// </summary>
        UseDefaultConfirmations = 16,

        /// <summary>
        /// 
        /// </summary>
        UseBillingAddressOnClaims = 32,
    }
}
