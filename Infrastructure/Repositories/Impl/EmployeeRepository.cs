using PruebaSisteCredito.Domain.Entities;
using PruebaSisteCredito.Domain.Context;
using PruebaSisteCredito.Infrastructure.Repositories.Inter;

namespace PruebaSisteCredito.Infrastructure.Repositories.Impl
{

    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<IEnumerable<Employee>> GetAllAsync() => Task.FromResult(_context.Employees.AsEnumerable());

        public Task<Employee?> GetByIdAsync(int id) =>
            Task.FromResult(_context.Employees.FirstOrDefault(e => e.Id == id));

        public Task AddAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Employee employee)
        {
             _context.Employees.Update(employee);
             _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                 _context.SaveChangesAsync();
            }
            return Task.CompletedTask;
        }
    }
}