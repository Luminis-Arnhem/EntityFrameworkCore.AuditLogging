using System;

namespace Luminis.EntityFrameworkCore.AuditLogging.Exceptions;

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
}
