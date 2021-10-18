using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Luminis.EntityFrameworkCore.AuditLogging.Models
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry, Guid transactionId, string? userId)
        {
            Entry = entry;
            TransactionId = transactionId;
            UserId = userId;
            Action = DetermineActionFromEntityState(entry.State);
        }

        public string? UserId { get; }
        public Action Action { get; }
        public Guid TransactionId { get; }
        public EntityEntry Entry { get; }
        public string TableName { get; set; } = default!;
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public AuditLog ToAudit()
        {
            var audit = new AuditLog
            {
                TableName = TableName,
                DateTime = DateTime.UtcNow,
                KeyValues = JsonConvert.SerializeObject(KeyValues),
                OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues),
                NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues),
                Action = Action,
                TransactionId = TransactionId,
                UserId = UserId
            };
            return audit;
        }

        private Action DetermineActionFromEntityState(EntityState state)
        {
            return state switch
            {
                EntityState.Detached => Action.Detached,
                EntityState.Unchanged => throw new NotImplementedException(),
                EntityState.Deleted => Action.Deleted,
                EntityState.Modified => Action.Modified,
                EntityState.Added => Action.Added,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
