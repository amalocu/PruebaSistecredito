using PruebaSisteCredito.Domain.Entities;
using PruebaSisteCredito.Domain.Context;
using PruebaSisteCredito.Infrastructure.Repositories.Inter;
using Microsoft.EntityFrameworkCore;

namespace PruebaSisteCredito.Infrastructure.Repositories.Impl
{

    public class OverTimeRepository : IOverTimeRepository
    {
        private readonly ApplicationDbContext _context;

        public OverTimeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public Task AddOverTime(OverTime overTime)
        {
            _context.OverTime.Add(overTime);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task<OverTime?> GetByIdAsync(int id)=>
            Task.FromResult(_context.OverTime.AsNoTracking().FirstOrDefault(e => e.Id == id));

        public async Task<decimal> GetSumByUserAndDateRangeAsync(int usuarioId, DateTime startDate, DateTime endDate)
        {
            return await _context.OverTime
                .Where(r => r.EmployeeId == usuarioId && r.StartDate >= startDate && r.StartDate <= endDate)
                .SumAsync(r => r.HoursWorked);
        }

        Task<IEnumerable<OverTime>> IOverTimeRepository.GetOverTimeByEmployeeAndDateRange(int employeeId, DateTime startDate, DateTime endDate) => Task.FromResult(
            _context.OverTime.Where(r => r.EmployeeId == employeeId && r.StartDate >= startDate && r.StartDate <= endDate).AsEnumerable()
        );

        public Task UpdateAsync(OverTime overTime)
        {
             _context.OverTime.Update(overTime);
             _context.SaveChanges();
            return Task.CompletedTask;
        }

 
    }
}