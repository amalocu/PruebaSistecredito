using PruebaSisteCredito.Domain.Entities;
using PruebaSisteCredito.Domain.Context;
using PruebaSisteCredito.Infrastructure.Repositories.Inter;

namespace PruebaSisteCredito.Infrastructure.Repositories.Impl
{

    public class AreaRepository : IAreaRepository
    {
        private readonly ApplicationDbContext _context;

        public AreaRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<IEnumerable<Area>> GetAllAsync() => Task.FromResult(_context.Areas.AsEnumerable());

        public Task<Area?> GetByIdAsync(int id) =>
            Task.FromResult(_context.Areas.FirstOrDefault(e => e.Id == id));

        public Task AddAsync(Area area)
        {
            _context.Areas.Add(area);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Area area)
        {
             _context.Areas.Update(area);
             _context.SaveChangesAsync();
           return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var area = _context.Areas.Find(id);
            if (area != null)
            {
                _context.Areas.Remove(area);
                 _context.SaveChangesAsync();
            }
            return Task.CompletedTask;
        }
    }
}