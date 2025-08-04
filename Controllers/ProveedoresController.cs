using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockApi.Data;
using StockApi.Models;

namespace StockApi.Controllers
{
    [ApiController]
    [Route("api/proveedores")]
    public class ProveedoresController : ControllerBase
    {
        private readonly StockDbContext _context;

        public ProveedoresController(StockDbContext context)
        {
            _context = context;
        }

        // GET /Proveedor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedor>>> GetProveedor()
        {
            return await _context.Proveedores.ToListAsync();
        }

        // GET /Proveedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor>> GetProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return proveedor;
        }

        // POST /Proveedor
        [HttpPost]
        public async Task<IActionResult> PostProveedor([FromBody] Proveedor proveedor)
        {
            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProveedor), new { id = proveedor.Id }, proveedor);
        }

        // DELETE /Proveedor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProveedor(int id)
        {
            var prov = await _context.Proveedores.FindAsync(id);
            if (prov == null)
                return NotFound();

            _context.Proveedores.Remove(prov);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT /Proveedor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProveedor(int id, [FromBody] Proveedor proveedor)
        {
            if (id != proveedor.Id)
                return BadRequest();

            var  proveedorExistente = await _context.Proveedores.FindAsync(id);
            if ( proveedorExistente == null)
                return NotFound();

             proveedorExistente.Nombre = proveedor.Nombre;
             proveedorExistente.Correo = proveedor.Correo;
             proveedorExistente.Telefono = proveedor.Telefono;
            proveedorExistente.Direccion = proveedor.Direccion;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
