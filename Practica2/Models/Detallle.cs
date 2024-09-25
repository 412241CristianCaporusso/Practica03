namespace Practica2.Models
{
    public class Detalle
    {

        public int id { get; set; }
        public Articulo Articulo { get; set; }
        public int cantidad { get; set; }
        public int nro_factura { get; set; }

    }
}
