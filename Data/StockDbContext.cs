using Microsoft.EntityFrameworkCore;
using StockApi.Models;

namespace StockApi.Data
{
    public class StockDbContext : DbContext
    {
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empleado> Empleados { get; set;}
        public DbSet<MovimientoStock> Movimientos {get; set;}
        public DbSet<Proveedor> Proveedores { get; set;}
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options) { }
    }
}
