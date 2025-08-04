using Microsoft.AspNetCore.Mvc;
using StockApi.Data;
using StockApi.Models;

namespace StockApi.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private readonly StockDbContext _context;

        public LoginController(StockDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Login([FromBody] Usuario loginData)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.NombreUsuario == loginData.NombreUsuario && u.Contraseña == loginData.Contraseña);

            if (usuario == null)
            {
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });
            }

            return Ok(new { usuario.NombreUsuario, usuario.Rol });
        }
    }
}
