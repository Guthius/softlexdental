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

namespace CodeBase
{
    public class ODException : ApplicationException
    {
        /// <summary>
        /// Gets the error code associated to this exception.
        /// </summary>		
        public int ErrorCode { get; } = 0;

        /// <summary>
        /// Contains query text when an ErrorCode in the 700s was thrown. This is the query that 
        /// was attempted prior to an exception.
        /// </summary>
        public string Query { get; } = "";

        public ODException(string message) : this(message, 0)
        {
        }

        public ODException(string message, ErrorCodes errorCodeAsEnum) : this(message, (int)errorCodeAsEnum)
        {
        }

        public ODException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public ODException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Wrap the given action in a try/catch and swallow any exceptions that are thrown.
        /// This should be used sparingly as we typically want to handle the exception or let it 
        /// bubble up to the UI but sometimes you just want to ignore it.
        /// </summary>
        public static void SwallowAnyException(Action a)
        {
            try
            {
                a();
            }
            catch
            {
            }
        }

        ///<summary>Predefined ODException.ErrorCode field values. ErrorCode field is not limited to these values but this is a convenient way defined known error types.
        ///These values must be converted to/from int in order to be stored in ODException.ErrorCode.
        ///Number ranges are arbitrary but should reserve plenty of padding for the future of a given range.
        ///Each number range should share a similar prefix between all of it's elements.</summary>
        public enum ErrorCodes
        {
            ///<summary>0 is the default. If the given (int) ErrorCode is not defined here, it will be returned at 0 - NotDefined.</summary>
            NotDefined = 0,
            //100-199 range. Values used by ODSocket architecture.
            ///<summary>No immortal socket connection found for this RegistrationKeyNum.
            ///The Proxy is trying to communicate with this eConnector but the eConnector does not have an active connection.</summary>
            ODSocketNotFoundForRegKeyNum = 100,
            ///<summary>Immortal socket connection was found by Proxy but the remote eConnector socket is not responding. 
            ///Most likely because the eConnector has been turned off but the Proxy has not performed an ACK to discover that it's off.</summary>
            ODSocketEConnectorNotResponding = 101,
            //200-299 range. Values used by XWeb/XCharge integration.
            ///<summary>.</summary>
            OtkArgsInvalid = 200,
            ///<summary>.</summary>
            OtkCreationFailed = 201,
            ///<summary>.</summary>
            MaxRequestDataExceeded = 202,
            ///<summary>.</summary>
            XWebProgramProperties = 203,
            ///<summary>.</summary>
            PayConnectProgramProperties = 204,
            ///<summary>.</summary>
            WebPaySetup = 205,
            //400-499 range. Values used by web apps
            ///<summary>No patient found that matches the specified parameters.</summary>
            NoPatientFound = 400,
            ///<summary>More than one patient found that matches the specified parameters.</summary>
            MultiplePatientsFound = 401,
            ///<summary>No appointment found that matches the specified parameters.</summary>
            NoAppointmentFound = 402,
            ///<summary>The time slot provided was not found or invalid.</summary>
            TimeSlotInvalid = 403,
            ///<summary>The response status provided is not acceptable.</summary>
            ResponseStatusInvalid = 404,
            ///<summary>No asapcomm found that matches the specified parameters.</summary>
            NoAsapCommFound = 405,
            ///<summary>No operatories have been set up for Web Sched.</summary>
            NoOperatoriesSetup = 406,
            //500-599 range. Values used by Open Dental UI.
            FormClosed = 500,
            //600-699 range. Values used by RemotingClient/MiddleTier
            ///<summary>After successfully logging in to Open Dental, a middle tier call to Userods.CheckUserAndPassword returned an "Invalid user or password" error.</summary>
            CheckUserAndPasswordFailed = 600,
            //700-799 range. Values used by failed query exceptions.
            ///<summary>Generic database command failed to execute.</summary>
            DbQueryError = 700,
        }
    }
}
