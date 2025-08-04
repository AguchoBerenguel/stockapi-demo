using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockApi.Data;
using StockApi.Models;

namespace StockApi.Controllers
{
  [ApiController]
  [Route("api/movimientos")]
  public class MovimientoStockController : ControllerBase
  {
    private readonly StockDbContext _context;

    public MovimientoStockController(StockDbContext context)
    {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovimientoStock>>> GetMovimientos()
    {
      return await _context.Movimientos
          .Include(m => m.Producto)
          .OrderByDescending(m => m.Fecha)
          .ToListAsync();
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarMovimiento([FromBody] MovimientoStockDTO mov)
    {
      var producto = await _context.Productos.FindAsync(mov.ProductoId);
      if (producto == null)
        return BadRequest("Producto inexistente.");

      if (mov.Tipo == "Ingreso")
      {
        producto.Stock += mov.Cantidad;
      }
      else if (mov.Tipo == "Egreso")
      {
        // --- CONTROL CRÍTICO ---
        if (producto.Stock < mov.Cantidad)
          return BadRequest("No hay suficiente stock para realizar el egreso.");
        producto.Stock -= mov.Cantidad;
      }
      else
      {
        return BadRequest("Tipo de movimiento inválido.");
      }

      var nuevoMov = new MovimientoStock
      {
        ProductoId = mov.ProductoId,
        Tipo = mov.Tipo,
        Cantidad = mov.Cantidad,
        Responsable = mov.Responsable,
        Fecha = DateTime.Now
      };

      _context.Movimientos.Add(nuevoMov);
      await _context.SaveChangesAsync();

      return Ok(nuevoMov);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarMovimiento(int id)
    {
      var movimiento = await _context.Movimientos.FindAsync(id);
      if (movimiento == null)
      {
        return NotFound("Movimiento no encontrado.");
      }

      // Opcional: revertir el stock
      var producto = await _context.Productos.FindAsync(movimiento.ProductoId);
      if (producto != null)
      {
        if (movimiento.Tipo == "Ingreso")
          producto.Stock -= movimiento.Cantidad;
        else if (movimiento.Tipo == "Egreso")
          producto.Stock += movimiento.Cantidad;
      }

      _context.Movimientos.Remove(movimiento);
      await _context.SaveChangesAsync();

      return NoContent(); // 204
    }

  }
}
