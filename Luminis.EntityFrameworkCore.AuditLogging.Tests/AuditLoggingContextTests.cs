using System;
using System.Linq;
using System.Threading.Tasks;
using Luminis.EntityFrameworkCore.AuditLogging.Exceptions;
using Luminis.EntityFrameworkCore.AuditLogging.Tests.TestBed;
using Luminis.EntityFrameworkCore.AuditLogging.Tests.TestBed.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using Xunit;

namespace Luminis.EntityFrameworkCore.AuditLogging.Tests
{
    public class AuditLoggingContextTests
    {
        [Fact]
        public void ContextShouldConstructCorrectly()
        {
            // Arrange - Act
            var dbContext = PrepareDataContext();

            // Assert
            dbContext.ShouldNotBeNull();
        }

        [Fact]
        public async Task AddingANotAuditedEntityShouldNotAddAnItemToTheAuditLog()
        {
            // Arrange
            var dbContext = PrepareDataContext();

            // Act
            dbContext.NotAuditedEntities.Add(new NotAuditedEntity { Name = "John Doe" });
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.Audits.Count().ShouldBe(0);
        }


        [Fact]
        public async Task AddingAnEntityWithShadowPropertiesShouldThrowAnException()
        {
            // Arrange
            var dbContext = PrepareDataContext();


            // Act
            dbContext.AuditLogWithShadowProperties.Add(new AuditLogWithShadowProperties { OtherEntity = new NotAuditedEntity() });
            // Assert
            var ex = await Assert.ThrowsAsync<AuditLoggingException>(async () => await dbContext.SaveChangesAsync());
            Assert.Equal("Cannot audit with shadow properties. Please supply a property from OtherEntityId", ex.Message);
        }
        

        [Fact]
        public async Task AddingAnAuditedEntityShouldAddAnItemToTheAuditLog()
        {
            // Arrange
            var dbContext = PrepareDataContext();


            // Act
            var auditedEntity = new AuditedEntity { Name = "John Doe", IgnoredField = "Ignored" };
            dbContext.AuditedEntities.Add(auditedEntity);
            await dbContext.SaveChangesAsync();

            // Assert
            var auditLogEntity = dbContext.Audits.FirstOrDefault();
            auditLogEntity.ShouldNotBeNull();
            auditLogEntity.TableName.ShouldBe("AuditedEntity");
            auditLogEntity.DateTime.Date.ShouldBe(DateTimeOffset.UtcNow.Date);
            auditLogEntity.KeyValues.ShouldBe($"{{\"Id\":{auditedEntity.Id}}}");
            auditLogEntity.OldValues.ShouldBeNull();
            auditLogEntity.NewValues.ShouldBe("{\"Name\":\"John Doe\",\"NotChangingField\":null}");
            auditLogEntity.TransactionId.ShouldNotBeNull();
            auditLogEntity.TransactionId.ShouldNotBe(Guid.Empty);
            auditLogEntity.UserId.ShouldBe("john.doe@email.com");
            auditLogEntity.Action.ShouldBe(Action.Added);
        }

        [Fact]
        public async Task UpdatingAnAuditedEntityShouldAddAnItemToTheAuditLog()
        {
            // Arrange
            var dbContext = PrepareDataContext();

            var existing = new AuditedEntity { Name = "John Doe", IgnoredField = "Ignored", NotChangingField = "OriginalValue" };
            dbContext.AuditedEntities.Add(existing);
            await dbContext.SaveChangesAsync();

            // Act
            var auditedEntity = dbContext.AuditedEntities.First();
            auditedEntity.Name = "Jane Doe";
            await dbContext.SaveChangesAsync();

            // Assert
            var auditLogEntity = dbContext.Audits.LastOrDefault();
            auditLogEntity.ShouldNotBeNull();
            auditLogEntity.TableName.ShouldBe("AuditedEntity");
            auditLogEntity.DateTime.Date.ShouldBe(DateTimeOffset.UtcNow.Date);
            auditLogEntity.KeyValues.ShouldBe($"{{\"Id\":{auditedEntity.Id}}}");
            auditLogEntity.OldValues.ShouldBe("{\"Name\":\"John Doe\"}");
            auditLogEntity.NewValues.ShouldBe("{\"Name\":\"Jane Doe\"}");
            auditLogEntity.TransactionId.ShouldNotBeNull();
            auditLogEntity.TransactionId.ShouldNotBe(Guid.Empty);
            auditLogEntity.UserId.ShouldBe("john.doe@email.com");
            auditLogEntity.Action.ShouldBe(Action.Modified);
        }

        [Fact]
        public async Task UpdatingAnIgnoredFieldInAnAuditedEntityShouldNotAddAnItemToTheAuditLog()
        {
            // Arrange
            var dbContext = PrepareDataContext();

            var existing = new AuditedEntity { Name = "John Doe", IgnoredField = "Ignored", NotChangingField = "OriginalValue" };
            dbContext.AuditedEntities.Add(existing);
            await dbContext.SaveChangesAsync();
            dbContext.Audits.Remove(dbContext.Audits.First());
            await dbContext.SaveChangesAsync();

            // Act
            var auditedEntity = dbContext.AuditedEntities.First();
            auditedEntity.IgnoredField = "IgnoredAgain";
            await dbContext.SaveChangesAsync();

            // Assert
            var auditLogEntity = dbContext.Audits.FirstOrDefault(a => a.Action == Action.Modified);
            auditLogEntity.ShouldBeNull();
        }

        [Fact]
        public async Task DeletingAnAuditedEntityShouldAddAnItemToTheAuditLog()
        {
            // Arrange
            var dbContext = PrepareDataContext();

            var existing = new AuditedEntity { Name = "John Doe", IgnoredField = "Ignored", NotChangingField = "OriginalValue" };
            dbContext.AuditedEntities.Add(existing);
            await dbContext.SaveChangesAsync();

            // Act
            var auditedEntity = dbContext.AuditedEntities.First();
            dbContext.Remove(auditedEntity);
            await dbContext.SaveChangesAsync();

            // Assert
            var auditLogEntity = dbContext.Audits.LastOrDefault();
            auditLogEntity.ShouldNotBeNull();
            auditLogEntity.TableName.ShouldBe("AuditedEntity");
            auditLogEntity.DateTime.Date.ShouldBe(DateTimeOffset.UtcNow.Date);
            auditLogEntity.KeyValues.ShouldBe($"{{\"Id\":{auditedEntity.Id}}}");
            auditLogEntity.OldValues.ShouldBe("{\"Name\":\"John Doe\",\"NotChangingField\":\"OriginalValue\"}");
            auditLogEntity.NewValues.ShouldBeNull();
            auditLogEntity.TransactionId.ShouldNotBeNull();
            auditLogEntity.TransactionId.ShouldNotBe(Guid.Empty);
            auditLogEntity.UserId.ShouldBe("john.doe@email.com");
            auditLogEntity.Action.ShouldBe(Action.Deleted);
        }

        private static TestDataContext PrepareDataContext()
        {
            var moqUserIdProvider = new Mock<IUserIdProvider>();
            moqUserIdProvider.Setup(u => u.GetUserId()).Returns("john.doe@email.com");

            var dbOptions = new DbContextOptionsBuilder<TestDataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TestDataContext(dbOptions, moqUserIdProvider.Object);
        }
    }
}
