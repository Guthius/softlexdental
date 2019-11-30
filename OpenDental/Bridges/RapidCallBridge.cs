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
    public class RapidCallBridge : CommandLineBridge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RapidCallBridge"/> class.
        /// </summary>
        public RapidCallBridge() : base("Rapid Call", "", "https://dentaltek.com/")
        {
        }

        /// <summary>
        ///     <para>
        ///         Does nothing. No preperation required for RapidCall.
        ///     </para>
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="patient"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        protected override bool PrepareToRun(long programId, Patient patient, out string arguments)
        {
            arguments = "";

            return true;
        }
    }
}
