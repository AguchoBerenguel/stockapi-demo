using Microsoft.EntityFrameworkCore;
using StockApi.Data;
using StockApi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StockApi
{
    public class MigrationService
    {
        private readonly string _sqliteConnection = "Data Source=database.db";
        private readonly string _mysqlConnection;

        public MigrationService(string mysqlConnection)
        {
            _mysqlConnection = mysqlConnection;
        }

        public async Task MigrarAsync()
        {
            var sqliteOptions = new DbContextOptionsBuilder<StockDbContext>()
                .UseSqlite(_sqliteConnection)
                .Options;

            var mysqlOptions = new DbContextOptionsBuilder<StockDbContext>()
                .UseMySql(_mysqlConnection, ServerVersion.AutoDetect(_mysqlConnection))
                .Options;

            using var sqliteContext = new StockDbContext(sqliteOptions);
            using var mysqlContext = new StockDbContext(mysqlOptions);

            Console.WriteLine("🟢 Iniciando migración de datos...");

            // PRODUCTOS
            var productos = await sqliteContext.Productos.ToListAsync();
            foreach (var p in productos)
            {
                if (!mysqlContext.Productos.Any(x => x.Id == p.Id))
                    mysqlContext.Productos.Add(p);
            }

            // EMPLEADOS
            var empleados = await sqliteContext.Empleados.ToListAsync();
            foreach (var e in empleados)
            {
                if (!mysqlContext.Empleados.Any(x => x.Id == e.Id))
                    mysqlContext.Empleados.Add(e);
            }

            // PROVEEDORES
            var proveedores = await sqliteContext.Proveedores.ToListAsync();
            foreach (var pr in proveedores)
            {
                if (!mysqlContext.Proveedores.Any(x => x.Id == pr.Id))
                    mysqlContext.Proveedores.Add(pr);
            }

            // USUARIOS
            var usuarios = await sqliteContext.Usuarios.ToListAsync();
            foreach (var u in usuarios)
            {
                if (!mysqlContext.Usuarios.Any(x => x.Id == u.Id))
                    mysqlContext.Usuarios.Add(u);
            }

            // MOVIMIENTOS DE STOCK (relación con Producto)
            var movimientos = await sqliteContext.Movimientos
                .Include(m => m.Producto)
                .ToListAsync();

            foreach (var m in movimientos)
            {
                if (!mysqlContext.Movimientos.Any(x => x.Id == m.Id))
                {
                    mysqlContext.Movimientos.Add(new MovimientoStock
                    {
                        ProductoId = m.ProductoId,
                        Tipo = m.Tipo,
                        Cantidad = m.Cantidad,
                        Responsable = m.Responsable,
                        Fecha = m.Fecha
                    });
                }
            }

            Console.WriteLine("💾 Guardando cambios en MySQL...");
            await mysqlContext.SaveChangesAsync();
            Console.WriteLine("✅ Migración completada con éxito.");
        }
    }
}
