using System;
using Luminis.EntityFrameworkCore.AuditLogging.Tests.TestBed.Models;
using Microsoft.EntityFrameworkCore;

namespace Luminis.EntityFrameworkCore.AuditLogging.Tests.TestBed
{
    internal class TestDataContext : AuditLoggingContext
    {
        public TestDataContext(DbContextOptions options, IUserIdProvider userIdProvider, bool persistAllProperties) : base(options, userIdProvider, persistAllProperties)
        {
        }

        public DbSet<NotAuditedEntity> NotAuditedEntities { get; set; } = default!;
        public DbSet<AuditedEntity> AuditedEntities { get; set; } = default!;
        public DbSet<AuditLogWithShadowProperties> AuditLogWithShadowProperties { get; set; } = default!;
    }
}
