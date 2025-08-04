namespace StockApi.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public string NombreyApellido { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public double Sueldo { get; set; }
    }
}
