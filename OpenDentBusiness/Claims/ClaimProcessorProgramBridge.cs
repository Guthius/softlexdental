/**
 * Copyright (C) 2019 Dental Stars SRL
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
using System.Diagnostics;

namespace OpenDentBusiness
{
    public abstract class ClaimProcessorProgramBridge : IClaimProcessorProgramBridge
    {
        /// <summary>
        /// Gets the name of the claim processor.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the last error message of the claim processor.
        /// </summary>
        public string ErrorMessage { get; protected set; } = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimProcessorProgramBridge"/> class.
        /// </summary>
        /// <param name="name"></param>
        public ClaimProcessorProgramBridge(string name) => Name = name ?? "";

        /// <summary>
        /// Launches the external program.
        /// </summary>
        /// <param name="clearinghouse"></param>
        /// <param name="batchNumber"></param>
        /// <returns></returns>
        public bool Launch(Clearinghouse clearinghouse, int batchNumber) => OnLaunch(clearinghouse, batchNumber);

        /// <summary>
        /// Launches the external program.
        /// </summary>
        /// <param name="clearinghouse"></param>
        /// <param name="batchNumber"></param>
        /// <returns></returns>
        protected virtual bool OnLaunch(Clearinghouse clearinghouse, int batchNumber)
        {
            try
            {
                Process.Start(clearinghouse.ClientProgram);
            }
            catch (Exception exception)
            {
                ErrorMessage = exception.Message;

                return false;
            }

            return true;
        }
    }
}
