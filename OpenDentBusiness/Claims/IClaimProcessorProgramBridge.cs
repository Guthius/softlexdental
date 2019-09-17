﻿/**
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

namespace OpenDentBusiness
{
    /// <summary>
    /// Represents a bridge with a 3rd party application that can used for processing e-Claims.
    /// </summary>
    public interface IClaimProcessorProgramBridge : IClaimProcessor
    {
        bool Launch(Clearinghouse clearinghouse, int batchNumber);
    }
}
