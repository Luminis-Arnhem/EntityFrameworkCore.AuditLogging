using System;

namespace Luminis.EntityFrameworkCore.AuditLogging.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class AuditIgnoreAttribute : Attribute
{
}
