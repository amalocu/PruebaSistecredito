using Microsoft.EntityFrameworkCore;
using PruebaSisteCredito.Domain.Entities;

namespace PruebaSisteCredito.Domain.Context{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public required DbSet<Employee> Employees { get; set; }  // Representa la tabla Employees en la base de datos
        public required DbSet<Area> Areas { get; set; }  // Representa la tabla Areas en la base de datos
        public required DbSet<OverTime> OverTime { get; set; }  // Representa la tabla OverTime en la base de datos

    }
}