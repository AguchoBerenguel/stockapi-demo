using System;

namespace StockApi.Models
{
    public class MovimientoStock
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; } 
        public string? Tipo { get; set; }
        public int Cantidad { get; set; }
        public string? Responsable { get; set; }
        public DateTime Fecha { get; set; }
    }

}