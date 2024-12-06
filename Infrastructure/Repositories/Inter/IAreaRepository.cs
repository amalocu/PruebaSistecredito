using PruebaSisteCredito.Domain.Entities;

namespace PruebaSisteCredito.Infrastructure.Repositories.Inter
{
    public interface IAreaRepository
    {
        Task<IEnumerable<Area>> GetAllAsync();
        Task<Area?> GetByIdAsync(int id);
        Task AddAsync(Area area);
        Task UpdateAsync(Area area);
        Task DeleteAsync(int id);
    }
}