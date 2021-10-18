using System;

namespace Luminis.EntityFramework.AuditLogging.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class AuditIgnoreAttribute : Attribute
    {
    }
}
