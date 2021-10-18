namespace Luminis.EntityFrameworkCore.AuditLogging.Tests.TestBed
{
    public interface IUserIdProvider
    {
        string? GetUserId();
    }
}
