namespace Luminis.EntityFrameworkCore.AuditLogging
{
    public interface IUserIdProvider
    {
        string? GetUserId();
    }
}
