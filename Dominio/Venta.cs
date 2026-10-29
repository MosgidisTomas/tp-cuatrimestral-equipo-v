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
        public string NumeroFactura { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public Cliente Cliente { get; set; }
        public Usuario Usuario { get; set; }
        public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();

        public decimal Total
        {
            get { return Detalles.Sum(d => d.Subtotal); }
        }
    }
}
