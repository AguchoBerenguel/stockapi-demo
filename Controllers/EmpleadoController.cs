using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockApi.Data;
using StockApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockApi.Controllers
{
    [ApiController]
    [Route("api/empleado")]
    public class EmpleadoController : ControllerBase
    {
        private readonly StockDbContext _context;

        public EmpleadoController(StockDbContext context)
        {
            _context = context;
        }

        // GET /api/empleado
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
        {
            return await _context.Empleados.ToListAsync();
        }

        // GET /api/empleado/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> GetEmpleado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
                return NotFound();
            return empleado;
        }

        // POST /api/empleado
        [HttpPost]
        public async Task<IActionResult> PostEmpleado([FromBody] Empleado empleado)
        {
            if (empleado == null)
                return BadRequest();

            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmpleado), new { id = empleado.Id }, empleado);
        }

        // DELETE /api/empleado/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            var emp = await _context.Empleados.FindAsync(id);
            if (emp == null)
                return NotFound();

            _context.Empleados.Remove(emp);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT /api/empleado/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpleado(int id, [FromBody] Empleado empleado)
        {
            if (empleado == null || id != empleado.Id)
                return BadRequest();

            var empExistente = await _context.Empleados.FindAsync(id);
            if (empExistente == null)
                return NotFound();

            empExistente.NombreyApellido = empleado.NombreyApellido;
            empExistente.Area = empleado.Area;
            empExistente.Sueldo = empleado.Sueldo;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}