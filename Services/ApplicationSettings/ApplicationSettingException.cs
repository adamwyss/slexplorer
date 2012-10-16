using System;

namespace Ijv.Redstone.Services
{
    /// <summary>
    /// Represents application setting errors that occur during application execution.
    /// </summary>
    public class ApplicationSettingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ApplicationSettingException class with serialized data.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ApplicationSettingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
