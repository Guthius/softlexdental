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
using System;
using System.IO;

namespace OpenDental.Bridges
{
    public class VixWinBase41Bridge : VixWinNumberedBridge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VixWinBase41Bridge"/> class.
        /// </summary>
        public VixWinBase41Bridge() : base("VixWin (Base41)", "")
        {
        }

        /// <summary>
        /// Gets the image storage location for the specified <paramref name="patient"/>.
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="baseImagePath">The base path for images.</param>
        /// <param name="patient">The patient.</param>
        /// <returns>The image location for the specified patient.</returns>
        protected override string GetPatientImagePath(long programId, string baseImagePath, Patient patient) =>
            Path.Combine(baseImagePath, GetIdentifier(programId, patient));

        /// <summary>
        /// Gets the base-41 identifier for the specified patient.
        /// </summary>
        /// <param name="programId">The ID of the program.</param>
        /// <param name="patient">The patient.</param>
        /// <returns>The ID of the patient.</returns>
        protected override string GetIdentifier(long programId, Patient patient)
        {
            string result = "";
            string base41Characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ@#$%^";

            long id = patient.PatNum;
            long temp ;
            long multiplier;

            for (int i = 5; i >= 0; i--)
            {
                multiplier = Pow(41, i);

                temp = 0;
                if (id >= multiplier)
                {
                    temp = id / multiplier;
                }
                id -= temp * multiplier;

                result += base41Characters[(int)temp];
            }

            return result;
        }

        /// <summary>
        ///     <para>
        ///         Returns the value of x ^ y. We assume y >= 0.
        ///     </para>
        ///     <para>
        ///         We cannot use <see cref="Math.Pow(double, double)"/>, because it uses doubles 
        ///         only, which has rounding errors with large numbers. We need our result to be a 
        ///         perfect integer.
        ///     </para>
        /// </summary>
        private static long Pow(long x, long y)
        {
            long result = 1;

            for (int p = 0; p < y; p++) result *= x;

            return result;
        }
    }
}
