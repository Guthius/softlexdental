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
using System.Diagnostics;
using System.Windows.Forms;

namespace OpenDental.Bridges
{
    public class MidwayBridge : Bridge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MidwayBridge"/> class.
        /// </summary>
        public MidwayBridge() : base(
            "Midway Dental", 
            "Midway Dental Supply is an independent dental supply company that services Indiana, Michigan and Northeastern Illinois.")
        {
        }

        /// <summary>
        ///     <para>
        ///         Opens the Midway Dental site.
        ///     </para>
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient details.</param>
        public override void Send(long programId, Patient patient)
        {
            try
            {
                Process.Start("https://midwaydental.com/");
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
