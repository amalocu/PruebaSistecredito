using MongoDB.Driver;
using PruebaSisteCredito.Domain.Entities;
using PruebaSisteCredito.Infrastructure.Repositories.Inter;

namespace PruebaSisteCredito.Infrastructure.Repositories.Impl
{

    public class  AuditLogRepository : IAuditLogRepository
    {
        private readonly IMongoCollection<AuditLog> _collection;

        public AuditLogRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<AuditLog>("AuditLogs");
        }
        public Task InsertLogAsync(AuditLog log)=> _collection.InsertOneAsync(log);
    }
}
