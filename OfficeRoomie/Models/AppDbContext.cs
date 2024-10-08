using Microsoft.EntityFrameworkCore;

namespace OfficeRoomie.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Administrador> Administradores { get; set; }
    }
}
