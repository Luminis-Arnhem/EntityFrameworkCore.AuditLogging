namespace Luminis.EntityFrameworkCore.AuditLogging.Models;

public enum Action
{
    Detached,
    Unchanged,
    Deleted,
    Modified,
    Added,
}
