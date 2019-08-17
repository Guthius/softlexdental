using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// Represents errors that occur during interaction with the database.
    /// </summary>
    public class DataException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public DataException(string message) :
            base(message)
        {
        }
    }
}