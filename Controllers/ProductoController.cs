using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockApi.Data;
using StockApi.Models;

namespace StockApi.Controllers
{
    [ApiController]
    [Route("api/producto")]
    public class ProductoController : ControllerBase
    {
        private readonly StockDbContext _context;

        public ProductoController(StockDbContext context)
        {
            _context = context;
        }

        // GET /producto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Productos.ToListAsync();
        }

        // GET /producto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return producto;
        }

        [HttpPost]
        public async Task<IActionResult> PostProducto([FromBody] Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var prod = _context.Productos.Find(id);
            if (prod == null) return NotFound();
            _context.Productos.Remove(prod);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, [FromBody] Producto producto)
        {
            if (id != producto.Id)
                return BadRequest();

            var prodExistente = await _context.Productos.FindAsync(id);
            if (prodExistente == null)
                return NotFound();

            prodExistente.Nombre = producto.Nombre;
            prodExistente.Stock = producto.Stock;
            prodExistente.Precio = producto.Precio;

            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
