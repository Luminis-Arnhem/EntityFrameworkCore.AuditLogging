using Luminis.EntityFramework.AuditLogging.Attributes;

namespace Luminis.EntityFrameworkCore.AuditLogging.Tests.TestBed.Models
{
    [Audit]
    public class AuditLogWithShadowProperties
    {
        public int Id { get; set; }
        public NotAuditedEntity OtherEntity { get; set; } = default!;
    }
}
