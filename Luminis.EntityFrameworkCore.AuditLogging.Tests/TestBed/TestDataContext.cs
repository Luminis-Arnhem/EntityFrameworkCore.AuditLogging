using System;
using Luminis.EntityFrameworkCore.AuditLogging.Tests.TestBed.Models;
using Microsoft.EntityFrameworkCore;

namespace Luminis.EntityFrameworkCore.AuditLogging.Tests.TestBed
{
    internal class TestDataContext : AuditLoggingContext
    {
        public TestDataContext(DbContextOptions options, IUserIdProvider userIdProvider) : base(options, () => userIdProvider.GetUserId())
        {
        }


        public DbSet<NotAuditedEntity> NotAuditedEntities { get; set; } = default!;
        public DbSet<AuditedEntity> AuditedEntities { get; set; } = default!;
    }
}
