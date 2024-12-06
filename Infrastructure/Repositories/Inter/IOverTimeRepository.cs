using PruebaSisteCredito.Domain.Entities;

namespace PruebaSisteCredito.Infrastructure.Repositories.Inter
{
    public interface IOverTimeRepository
    {
        public Task AddOverTime(OverTime overTime);
        public Task<OverTime?> GetByIdAsync(int id);
        public Task<IEnumerable<OverTime>> GetOverTimeByEmployeeAndDateRange(int employeeId, DateTime startDate, DateTime endDate);
        public Task<decimal> GetSumByUserAndDateRangeAsync(int usuarioId, DateTime startDate, DateTime endDate);
        public Task UpdateAsync(OverTime overTime);
    }
}