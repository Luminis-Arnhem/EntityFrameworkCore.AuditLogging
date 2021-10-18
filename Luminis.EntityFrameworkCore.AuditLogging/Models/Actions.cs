namespace Luminis.EntityFrameworkCore.AuditLogging
{
    public enum Action
    {
        Detached,
        Unchanged,
        Deleted,
        Modified,
        Added,
    }
}
