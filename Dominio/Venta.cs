using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Venta
    {
        public int Id { get; set; }
        // Número correlativo de factura: se guarda como número y se muestra como 0001-00000006.
        public int NumeroFactura { get; set; }

        public string NumeroFacturaFormateado
        {
            get { return "0001-" + NumeroFactura.ToString("D8"); }
        }

        public DateTime Fecha { get; set; } = DateTime.Now;
        public Cliente Cliente { get; set; }
        // Vendedor que registró la venta (el usuario logueado).
        public Usuario Usuario { get; set; }
        public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();

        public decimal Total
        {
            get { return Detalles.Sum(d => d.Subtotal); }
        }
    }
}
