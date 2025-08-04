namespace StockApi.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaIngreso { get; set; } = DateTime.Now;
    }
}
