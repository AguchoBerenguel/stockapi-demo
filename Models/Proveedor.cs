using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Proveedor
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Correo { get; set; } = string.Empty;

    [Required]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    public string Direccion { get; set; } = string.Empty;
}
