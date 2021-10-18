using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Luminis.EntityFrameworkCore.AuditLogging.Models
{
    [Table("AuditLog", Schema = "audit")]
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }
        public string TableName { get; set; } = null!;
        public DateTimeOffset DateTime { get; set; }
        public string KeyValues { get; set; } = null!;
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public Guid? TransactionId { get; set; }
        public string? UserId { get; set; }
        public Action Action { get; set; }
    }
}
