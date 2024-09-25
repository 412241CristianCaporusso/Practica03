namespace Practica2.Models
{
    public class Factura
    {

        public int NroFactura { get; set; }
        public DateTime? Fecha { get; set; }
        public FormaDePago? FormaPago { get; set; }
        public string? Cliente { get; set; }

        public List<Detalle> Detalles { get; set; } = new List<Detalle>();
    }
}
