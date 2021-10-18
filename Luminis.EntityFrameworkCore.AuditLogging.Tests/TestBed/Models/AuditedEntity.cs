using System.ComponentModel.DataAnnotations;
using Luminis.EntityFramework.AuditLogging.Attributes;

namespace Luminis.EntityFrameworkCore.AuditLogging.Tests.TestBed.Models
{
    [Audit]
    public class AuditedEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = default!;

        [Required]
        [MaxLength(100)]
        [AuditIgnore]
        public string IgnoredField { get; set; } = default!;

        [MaxLength(100)]
        public string? NotChangingField { get; set; }
    }
}
