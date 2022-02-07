using System;
using System.Runtime.Serialization;

namespace Luminis.EntityFrameworkCore.AuditLogging.Exceptions
{
    public class AuditLoggingException : Exception
    {
        public AuditLoggingException()
        {
        }

        public AuditLoggingException(string message) : base(message)
        {
        }

        public AuditLoggingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AuditLoggingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
