using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class MovimientoStock
    {
        public int Id { get; set; }
        public Producto Producto { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public TipoMovimiento Tipo { get; set; }
        public int Cantidad { get; set; }
        public int? IdCompra { get; set; }
        public int? IdVenta { get; set; }
    }
}
