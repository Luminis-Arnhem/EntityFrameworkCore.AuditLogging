using System.ComponentModel.DataAnnotations;

namespace Luminis.EntityFrameworkCore.AuditLogging.Tests.TestBed.Models
{
    public class NotAuditedEntity
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Name of the company
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = default!;
    }
}
