using PruebaSisteCredito.Domain.Entities;

namespace PruebaSisteCredito.Infrastructure.Repositories.Inter
{
    public interface IAuditLogRepository
    {
        public Task InsertLogAsync(AuditLog log);

    }
}