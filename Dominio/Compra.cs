using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Compra
    {
        public int Id { get; set; }
        public string NumeroFactura { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Today;
        public Proveedor Proveedor { get; set; }
        public Usuario Usuario { get; set; }
        public List<DetalleCompra> Detalles { get; set; } = new List<DetalleCompra>();

        public decimal Total
        {
            get { return Detalles.Sum(d => d.Subtotal); }
        }
    }
}
